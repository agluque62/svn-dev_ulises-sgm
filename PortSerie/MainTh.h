// MainTh.h: interface for the CMainTh class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_MAINTH_H__99B0AC02_EB8E_4444_AE47_FAC3EEE60A78__INCLUDED_)
#define AFX_MAINTH_H__99B0AC02_EB8E_4444_AE47_FAC3EEE60A78__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Cd30ManInterk.h"
#include "ServConmTh.h"
#include "ServNtpTh.h"
#include "ServManTh.h"
#include "ServHisTh.h"
#include "CpuTh.h"
#include "SrvEypTh.h"
#include "ValorSupervisado.h"

//////////////////////////////////////////////////////////////////////////
//
class CClients
{
#define DEF_SP_CONEX		250

public:
	CClients(){Init();}
	~CClients(){}
	
	bool Register(CIPAddress ip, bool isCfg=false);
	bool UnRegister(CIPAddress ip);
	void Broad(CUDPSocket *pSck, long iLen, void *pData);
	void Supervisa(void ); 
	bool IsCfgPresent();

private:
	void Init();
private:
	struct sReg
	{
		bool		bReg;
		CIPAddress	ip;
		long		iTime;
		bool		isCfg;
	} m_registered[DEF_MAX_CLIENTS];
};


//////////////////////////////////////////////////////////////////////////
//
class CMainTh : public CThread  
{
public:
	void Start(void );
	void Stop(void );

public:
	BOOL IsSelect(int iEquipo);
	void ProcesaHistorico(CIPAddress ip, long equipo, void *pvHis);
	void Broad(long lgLen, void *pData);
	void LoadConfig(void );
	void Responde(CIPAddress ip, BYTE equipo);
	BOOL Interprete(CIPAddress ip, SCd30ManCmd &cmd, BYTE &res);
	void Run(void );
	CMainTh();
	virtual ~CMainTh();

public:
	void EventConfig(int iEquipo);
	void EventSactaOff(char *lan);

private:
	CCritSec   m_acceso;
    CUDPSocket m_sck;
	CIPAddress m_broad,m_ip_local;
	CClients   m_clients;

	int			m_udpPort;
	int			m_dbType;
	int			m_iIdioma;

	char		*m_pDsnMan;
	char		*m_pOdbcMan;
	char		*m_pOdbcCf1;
	char		*m_pOdbcCf2;

private:
//	BOOL		m_bSacta1,m_bSacta2;
	CValorSupervisado m_bSacta1,m_bSacta2;

private:
	CServConmTh m_serv_conm;
	CServNtpTh  m_serv_ntp;
	CServManTh  m_serv_man_a;
	CServManTh  m_serv_man_b;
	CServHisTh  m_serv_his;
	CCpuTh      m_serv_cpu;
	CSrvEypTh	m_serv_eyp;
};

#endif // !defined(AFX_MAINTH_H__99B0AC02_EB8E_4444_AE47_FAC3EEE60A78__INCLUDED_)
