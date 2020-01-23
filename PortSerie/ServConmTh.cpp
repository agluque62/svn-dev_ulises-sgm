// ServConmTh.cpp: implementation of the CServConmTh class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "ServConmTh.h"

#include "Cd30ManInterk.h"
#include "Cd30Historico.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
BOOL CServConmTh::CSpActividad::IsEnable()
{
	// Set Now
	time(&m_time_now);

	if (m_cnt >= m_maximo)
	{
		return TRUE;
	}

	if (m_time_now >= m_time_next)
	{
		m_cnt = m_cnt + 1;
		m_time_next = m_time_now + 60;
	}

	return FALSE;
}

//////////////////////////////////////////////////////////////////////
//
CServConmTh::CServConmTh()
{
	CCd30LocalConfig cfg;
	SetId(2);
	m_udpPort = cfg.GetServUdpPort();
	m_sck.SetBroadcast();
}

//////////////////////////////////////////////////////////////////////
//
CServConmTh::~CServConmTh()
{
}

//////////////////////////////////////////////////////////////////////
//
void CServConmTh::Run()
{
	CCSLock *pLock;

	m_permiso.Enable();

	while (IsRunning())
	{
		pLock = new CCSLock(m_acceso);
		try
		{
			if (m_permiso.IsEnable())
			{
				if (!m_ps.IsOpen())
				{
					m_ps.Start();
					m_iEquipoLeido = m_ps.QueEquipo();
					m_ps.SetEquipo(m_iEquipoLeido);
					m_permiso.Enable();
				}

				m_ps.SpHsk();

				long iSelect = m_ps.QueEquipo();
				if (iSelect != m_iEquipoLeido)
				{ // Historico. Cambio Externo.
					CCd30LocalConfig cfg;
					long nMsg = cfg.GetPRRetencion() * 1000;

					nMsg = nMsg > 10000 ? 10000 : nMsg;
					Sleep(nMsg);	// Última Modificación. Confirmar 1 Segundo 
					// Despues, antes de Actuar.
					iSelect = m_ps.QueEquipo();
					if (iSelect != m_iEquipoLeido)
					{
						m_ps.SetEquipo(iSelect);
						m_iEquipoLeido = iSelect;
						AddHistorico(m_iEquipoLeido==EN_EQUIPOA ? HIS_PR_HW_SEL_A : HIS_PR_HW_SEL_B);
					}
				}
			}
		}

		catch (CPSException *)
		{
			m_ps.Stop();
			m_permiso.Disable();
		}

		catch (...)
		{
			_Module.LogError("CServConmTh::Run(). Excepcion No Controlada");
		}

		delete pLock;
		Sleep(50);
	}

	if (m_ps.IsOpen())
		m_ps.Stop();		// Close();
}

//////////////////////////////////////////////////////////////////////
//
long CServConmTh::SetEquipo(long iEquipo)
{
	CCSLock lock(m_acceso);

	int oldEquipo = m_ps.QueEquipo();

	m_ps.SetEquipo(iEquipo);

	int iCount = 10;				// Espera 1 Segundo para Comprobar Ejecucion.
	while (iCount--)
	{
		Sleep(100);
		long iSelect = m_ps.QueEquipo();
		if (iSelect==iEquipo)
		{	// Historico. Cambio Manual.
			m_iEquipoLeido = iSelect;
			AddHistorico(m_iEquipoLeido==EN_EQUIPOA ? HIS_PR_USER_A_SEL : HIS_PR_USER_B_SEL);
			return iEquipo;
		}
	}
	m_ps.SetEquipo(oldEquipo);
	AddHistorico(HIS_PR_SEL_ERROR);
	return EN_ERROR_SELECT;
}

//////////////////////////////////////////////////////////////////////
//
long CServConmTh::GetEquipo()
{
	CCSLock lock(m_acceso);
	return m_iEquipoLeido;
}


//////////////////////////////////////////////////////////////////////
//
long CServConmTh::GetStatus()
{
	CCSLock lock(m_acceso);
	return IsRunning() ? SERV_CONNECT :SERV_NOOK;
}


//////////////////////////////////////////////////////////////////////
//
void CServConmTh::AddHistorico(long id)
{
	CCd30LocalConfig cfg;

	try
	{
		SCd30ManCmd cmd;
		CIPAddress ipLocal = CIPAddress(cfg.GetIpServidor(), cfg.GetServUdpPort());

		cmd.comando = CMD_HISTORICO;
		cmd.equipo  = 2; // m_iEquipoLeido;
		cmd.serv_conm = 0;
		cmd.serv_mant_a = 0;
		cmd.serv_mant_b = 0;
		cmd.serv_ntp  = 0;
		cmd.time = 0;

		CCd30Historico::Format((LPSTR)(&cmd	.cmdHis),"server", id);

	//	m_sck.SendTo(&cmd, sizeof(cmd), CIPAddress(CInitSocket::ipLocal, m_udpPort));
		m_sck.SendTo(&cmd, sizeof(cmd), ipLocal);
	}
	catch (socket_error &e)
	{
		_Module.LogError("CServConm::AddHistorico. Socket Exception (%d)", e.GetCode());
	}
}
