// MainTh.cpp: implementation of the CMainTh class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include <time.h>

#include "MainTh.h"
#include "Cd30Historico.h"

using namespace std ;

//////////////////////////////////////////////////////////////////////
// CClients
//////////////////////////////////////////////////////////////////////
//
bool CClients::Register(CIPAddress ip, bool isCfg)
{
	UnRegister(ip);
	for (int i=0; i<DEF_MAX_CLIENTS; i++)
	{
		if (m_registered[i].bReg == false)
		{
			m_registered[i].bReg = true;
			m_registered[i].ip = ip;
			m_registered[i].isCfg = isCfg;
			m_registered[i].iTime = DEF_SP_CONEX;
			return true;
		}
	}
	return false;
}

//////////////////////////////////////////////////////////////////////
//
bool CClients::UnRegister(CIPAddress ip)
{
	for (int i=0; i<DEF_MAX_CLIENTS; i++)
	{
		CIPAddress lip = m_registered[i].ip;

		if (lip == ip)
		{
			m_registered[i].bReg = false;
			return true;
		}
	}
	return false;
}

//////////////////////////////////////////////////////////////////////
//
void CClients::Broad(CUDPSocket *pSck, long iLen, void *pData)
{
	for (int i=0; i<DEF_MAX_CLIENTS; i++)
	{
		if (m_registered[i].bReg == false)
			continue;
		try
		{
			pSck->SendTo(pData, iLen, m_registered[i].ip);
		}
		catch (socket_error &e)
		{// se sigue adelante
			_Module.LogError("CClients::Broad. Socket Exception (%d)", e.GetCode());
		}
	}
}

//////////////////////////////////////////////////////////////////////
//
void CClients::Init()
{
	for (int i=0; i<DEF_MAX_CLIENTS; i++)
		m_registered[i].bReg = false;
}

//////////////////////////////////////////////////////////////////////
//
void CClients::Supervisa()
{
	for (int i=0; i<DEF_MAX_CLIENTS; i++)
	{
		sReg *pReg = (sReg *)(&m_registered[i]);

		if (pReg->bReg == false)
			continue;
		
		if (pReg->iTime &&
			!(--pReg->iTime))
		{
			m_registered[i].bReg = false;
		}
	}
}

//////////////////////////////////////////////////////////////////////////
//
bool CClients::IsCfgPresent()
{
	for (int i=0; i<DEF_MAX_CLIENTS; i++)
	{
		if (m_registered[i].bReg  == true &&
			m_registered[i].isCfg == true)
		{
			return true;
		}
	}
	return false;
}

//////////////////////////////////////////////////////////////////////
// Construction/Destruction.
//////////////////////////////////////////////////////////////////////
//
CMainTh::CMainTh()
{
	m_pDsnMan  = new char[256];
	m_pOdbcMan = new char[256];
	m_pOdbcCf1 = new char[256];
	m_pOdbcCf2 = new char[256];

	m_sck.SetBroadcast();
	SetId(1);
}

//////////////////////////////////////////////////////////////////////
//
CMainTh::~CMainTh()
{
	delete m_pDsnMan;
	delete m_pOdbcMan;
	delete m_pOdbcCf1;
	delete m_pOdbcCf2;
}

//////////////////////////////////////////////////////////////////////
//
void CMainTh::Start()
{
	try
	{
		LoadConfig();
		CCd30DbAccess::SetMotorBdt(m_dbType, m_pDsnMan, m_pOdbcMan, m_pOdbcCf1, m_pOdbcCf2);
		CCd30DbAccess::Load(m_iIdioma);
		CThread::Start();
	}
	catch (db_error *e)
	{
		_Module.LogError("CMainTh::Run. Excepcion Cd30DbAccess: %s", e->what());
	}
	catch (...)
	{
		_Module.LogError("CMainTh::Run. Excepcion Cd30DbAccess: No Controlada.");
	}
}

//////////////////////////////////////////////////////////////////////////
//
void CMainTh::Stop()
{
	CThread::Stop();
	CCd30DbAccess::Unload();
}

//////////////////////////////////////////////////////////////////////
//
void CMainTh::Run()
{
	CIPAddress ipFrom;
	CCSLock *pLock;

	try
	{
	//	m_sck.Bind(m_udpPort);
		m_sck.Bind(m_ip_local);
		_Module.LogEvent("CMainTh::Run. Socket Enlazado...");
	}
	catch (socket_error &e)
	{
		_Module.LogError("CMainTh::Run. Socket Exception (%s)", e.what());
		return;
	}

	CCd30LocalConfig cfg;

	if (cfg.IsPREnabled())
		m_serv_conm.Start();
	if (cfg.IsNtpEnabled())
		m_serv_ntp.Start();
	if (cfg.GetClientEyp())
		m_serv_eyp.Start();

	m_serv_man_a.Start(EN_EQUIPOA, this);
	m_serv_man_b.Start(EN_EQUIPOB, this);

	m_serv_his.Start(this);
	m_serv_cpu.Start();

	_Module.LogEvent("CMainTh::Run. SubProcesos Arrancados...");

	while (IsRunning())
	{
		Sleep(50);
		pLock = new CCSLock(m_acceso);

		try
		{
			if (m_sck.IsReadable())
			{
				static SCd30ManCmd cmd;
				BYTE equipo;

				m_sck.RecvFrom(&cmd, sizeof(SCd30ManCmd), &ipFrom);

				_Module.LogTramaExt(m_udpPort, sizeof(SCd30ManCmd), &cmd);

				if (Interprete(ipFrom, cmd, equipo))
				{
					Responde(ipFrom, equipo);
				}
			}

			m_clients.Supervisa();
			
			if (m_bSacta1.EventTick())
				EventSactaOff("0");
			if (m_bSacta2.EventTick())
				EventSactaOff("1");

		}
		catch (socket_error &e)
		{
			if (e.GetCode()!=WSAECONNRESET)
				_Module.LogError("CMainTh::Run. Socket Exception (%s)", e.what());
		}
		catch (db_error *e)
		{
			_Module.LogError("CMainTh::Run. Excepcion de Base de Datos: %s", e->what());
		}
		catch (...)
		{
			_Module.LogError("CMainTh::Run. Excepcion No Controlada");
		}
		delete pLock;
	}

	m_serv_cpu.Stop();
	m_serv_his.Stop();
	m_serv_man_a.Stop();
	m_serv_man_b.Stop();

	if (m_serv_eyp.IsLive())
		m_serv_eyp.Stop();

	if (m_serv_ntp.IsLive())
		m_serv_ntp.Stop();

	if (m_serv_conm.IsLive())
		m_serv_conm.Stop();

	m_sck.Shutdown();
	m_sck.Close();
}

//////////////////////////////////////////////////////////////////////
//
BOOL CMainTh::Interprete(CIPAddress ip, SCd30ManCmd &cmd, BYTE &res)
{
	switch (cmd.comando)
	{
		case CMD_CAMBIA:
			m_clients.Register(ip);
			res = (BYTE )m_serv_conm.SetEquipo(cmd.equipo);
			return TRUE;

		case CMD_CONSULTA:					// Solo las Consolas de CFG.
			m_clients.Register(ip, true);
			res = (BYTE )m_serv_conm.GetEquipo();
			return TRUE;

		case CMD_CONSULTA_MNT:				// Solo las Consolas de MNTO.
			m_clients.Register(ip);
			res = (BYTE )m_serv_conm.GetEquipo();
			return TRUE;
			
		case CMD_HISTORICO:
			m_clients.Register(ip);
			ProcesaHistorico(ip, cmd.equipo, &cmd.cmdHis);
			return FALSE;

		case CMD_EXEC_SQL_MAN:
			m_serv_his.ExecSql((void *)ip.GetStringAddress().c_str(), &cmd.cmdHis.strInci);
			return FALSE;

		case CMD_REGISTER:
			m_clients.Register(ip);
			return FALSE;

		case CMD_UNREGISTER:
			m_clients.UnRegister(ip);
			return FALSE;

		case CMD_LOAD_INCI:
			CCd30DbAccess::Unload();
			CCd30DbAccess::Load(m_iIdioma);
			return FALSE;

		case CMD_NOT_VERSION:
			{
				WORD wIOL_Unidad = CD30_GetUnidad(cmd.cmdVer.wA_IOL);
				_Module.Versiones().NotVersion(EN_GESTION, wIOL_Unidad, &cmd.cmdVer);
			}
			return FALSE;

		case CMD_PET_VERSION:
			return FALSE;
		
	}
	return FALSE;
}

//////////////////////////////////////////////////////////////////////
//
void CMainTh::Responde(CIPAddress ip, BYTE equipo)
{
	SCd30ManCmd cmd;
	time_t timer;

	cmd.comando   = RSP_ESTADO;
	cmd.equipo    = equipo;
	cmd.serv_conm = (DWORD)m_serv_conm.GetStatus();
	cmd.serv_ntp  = (DWORD)m_serv_ntp.GetStatus();
	cmd.serv_mant_a = (DWORD)m_serv_man_a.GetStatus();
	cmd.serv_mant_b = (DWORD)m_serv_man_b.GetStatus();

	/*-----
	cmd.sacta = MAKELONG(m_serv_man_b.GetSacta() | m_serv_man_a.GetSacta(), 
						 m_serv_man_b.GetSacta() | m_serv_man_a.GetSacta());
	------*/
	cmd.sacta = MAKELONG(MAKEWORD(m_bSacta1.IsActive(), m_bSacta2.IsActive()),(WORD)m_clients.IsCfgPresent());
	cmd.cpu   = MAKELONG(m_serv_cpu.Cpu1(), m_serv_cpu.Cpu2());

	cmd.time = (DWORD )time(&timer);
	m_sck.SendTo(&cmd, sizeof(cmd), ip);
}

//////////////////////////////////////////////////////////////////////
//
void CMainTh::LoadConfig()
{
	CCd30LocalConfig cfg;

	m_udpPort = cfg.GetServUdpPort();
	m_broad   = CIPAddress(CIPAddress::BROADCAST, m_udpPort);
	m_ip_local = CIPAddress(cfg.GetIpServidor(), m_udpPort);
	m_dbType = cfg.GetServMotorBdt();

	cfg.GetBdtOdbcMan(m_pOdbcMan);
	cfg.GetBdtOdbcScv1(m_pOdbcCf1);
	cfg.GetBdtOdbcScv2(m_pOdbcCf2);

	string str(m_pOdbcMan);
	int pos = str.find_first_of("=",0);
	char *pName = (LPSTR )(&str.c_str()[pos+1]);
	strcpy(m_pDsnMan, pName);

	m_iIdioma = cfg.GetServIdioma();

	int time = 1000*cfg.GetServBdtSync();
	Sleep(time);
}

//////////////////////////////////////////////////////////////////////
//
void CMainTh::Broad(long lgLen, void *pData)
{
	CCSLock lock(m_acceso);

	try
	{
		m_clients.Broad(&m_sck, lgLen, pData);
	}
	catch (socket_error &e)
	{// se sigue adelante
		_Module.LogError("CMainTh::Broad. Socket Exception (%s)", e.what());
	}
}

//////////////////////////////////////////////////////////////////////
//
void CMainTh::ProcesaHistorico(CIPAddress ip, long equipo, void *pvHis)
{
	SCmdHistorico *pHis = (SCmdHistorico *)pvHis;
	char maquina[128],punto[128],user[128],str1[128],str2[128],str3[128];
	DWORD nInci;

	CCd30Historico::Scan((LPSTR)(pHis), maquina, nInci, punto, user, str1, str2, str3);

	switch(nInci) 
	{
		case HIS_SIS_SACTA_ON:
		case HIS_SIS_SACTA_OFF:
		{
			BOOL m_bNewStd  = (nInci == HIS_SIS_SACTA_ON) ? TRUE : FALSE;
			equipo = 2;
			if (strcmp(punto,"0")==0)
			{
				if (!m_bSacta1.EventOnOff(m_bNewStd))
					return;
			}
			else if (strcmp(punto,"1")==0)
			{
				if (!m_bSacta2.EventOnOff(m_bNewStd))
					return;
			}
		}
		break;

		case HIS_SIS_CONF_CARGADA:
		case HIS_SECT_IMPLANTADA:
			CCd30DbAccess::DbCrossData_Reload();
			EventConfig(DEF_SCV_A);
			EventConfig(DEF_SCV_B);
			break;

		default:
	//		str2 = "*****";
			break;
	}

	char str[128],hora[32];
	CCd30Hist *pCHis = new CCd30Hist(equipo, nInci, punto, user, str1, str2, str3);

	if (pCHis->IsAlarma(hora, str))
	{
		SCd30ManCmd cmd;

		cmd.comando = RSP_HISTORICO;
		cmd.equipo  = 0;
		cmd.serv_conm = 0;
		cmd.serv_mant_a = 0;
		cmd.serv_mant_b = 0;
		cmd.serv_ntp  = 0;
		cmd.time = 0;

		if (CCd30Historico::FormatStr((LPSTR)(&cmd.cmdHis), hora, str))
			Broad(sizeof(cmd), &cmd);
		else
			_Module.LogError("CMainTh::ProcesaHistorico. String de Historico Demasiado Largo");
	}

	m_serv_his.AddHist(pCHis);
}

//////////////////////////////////////////////////////////////////////
//
BOOL CMainTh::IsSelect(int iEquipo)
{
	return m_serv_conm.GetEquipo()==iEquipo ? TRUE : FALSE;
}

//////////////////////////////////////////////////////////////////////////
//
void CMainTh::EventConfig(int iEquipo)
{
	try
	{
		SCd30ManCmd cmd;

		cmd.comando = RSP_SINCBDT;
		cmd.equipo  = iEquipo;
		cmd.serv_conm = 0;
		cmd.serv_mant_a = 0;
		cmd.serv_mant_b = 0;
		cmd.serv_ntp  = 0;
		cmd.time = 0;

		Broad(sizeof(cmd), &cmd);
	}
	catch (...) 
	{
		_Module.LogError("CMainTh::EventConfig. Excepcion no Esperada");
	}
}

//////////////////////////////////////////////////////////////////////////
//
void CMainTh::EventSactaOff(char *lan)
{
	char str[128],hora[32];
	CCd30Hist *pCHis = new CCd30Hist(2, HIS_SIS_SACTA_OFF, lan);
	if (pCHis->IsAlarma(hora, str))
	{
		SCd30ManCmd cmd;

		cmd.comando = RSP_HISTORICO;
		cmd.equipo  = 0;
		cmd.serv_conm = 0;
		cmd.serv_mant_a = 0;
		cmd.serv_mant_b = 0;
		cmd.serv_ntp  = 0;
		cmd.time = 0;

		if (CCd30Historico::FormatStr((LPSTR)(&cmd.cmdHis), hora, str))
			Broad(sizeof(cmd), &cmd);
		else
			_Module.LogError("CMainTh::ProcesaHistorico. String de Historico Demasiado Largo");
	}

	m_serv_his.AddHist(pCHis);
}