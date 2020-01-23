// ControlAB.cpp: implementation of the CControlAB class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "ControlAB.h"
#include "Cd30ManInterk.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
//
CControlAB::CControlAB()
{
	m_simulador = CCd30LocalConfig().GetPRSimulador();
}

//////////////////////////////////////////////////////////////////////
//
CControlAB::~CControlAB()
{
}

//////////////////////////////////////////////////////////////////////
//
long CControlAB::QueEquipo(void )
{
	if (m_simulador)
	{
		Sleep(100);
		return m_equipo_sel;
	}
	else
	{
	/*
		long iSignal = m_iMscSenal==MS_DSR_ON ? GetDsr() :
  					m_iMscSenal==MS_CTS_ON ? GetCts() : GetCd();

		return iSignal==m_bGetEquA ? EN_EQUIPOA : EN_EQUIPOB;
	*/
		return GetCts() ? EN_EQUIPOB : EN_EQUIPOA;
	}
}

//////////////////////////////////////////////////////////////////////
//
void CControlAB::SetEquipo(int iEquipo)
{
	if (m_simulador)
	{
		Sleep(100);
		m_equipo_sel = iEquipo;
	}
	else
	{
	/*--
		BOOL bMando = iEquipo == EN_EQUIPOA ? m_bSetEquA : !m_bSetEquA;

		if (m_iMscControl==0)
			SetDtr(bMando);
		else
			SetRts(bMando);
	--*/

		if (iEquipo == EN_EQUIPOA)
			SetRts(TRUE);
		else
			SetRts(FALSE);
	}
}

//////////////////////////////////////////////////////////////////////
//
bool CControlAB::Start(void )
{
	bool bConfig = true;

	LoadConfig();

	if (!m_simulador)
	{
		bConfig = Open(m_strPort) ? true : false;
	}
	else
		m_bIsOpen = true;
	return bConfig;
}

//////////////////////////////////////////////////////////////////////
//
void CControlAB::Stop()
{
	if (m_simulador)
		m_bIsOpen = false;
	else
		Close();
}

//////////////////////////////////////////////////////////////////////
//
void CControlAB::LoadConfig()
{
	if (m_simulador)
		m_equipo_sel = EN_EQUIPOA;

	CCd30LocalConfig cfg;

	int iPort         = cfg.GetPRSerialPort();
	int iLineControl  = cfg.GetPRControlLine();
	int iNivelControl = cfg.GetPRControlLevel();
	int iLineSenal    = cfg.GetPRSignalLine();
	int iNivelSenal   = cfg.GetPRSignalLevel();

	std::ostringstream os;

	if (iPort >= 10)
		os << "\\\\.\\COM" << iPort;
	else
		os << "COM" << iPort;

	m_strPort = os.str();
	    
	m_iMscSenal = iLineSenal == 0 ? MS_DSR_ON :
				  iLineSenal == 1 ? MS_CTS_ON :
				  iLineSenal == 2 ? MS_RLSD_ON : 0;

	m_bGetEquA = iNivelSenal ? TRUE : FALSE;
	                                                  
	m_iMscControl = iLineControl;
	m_bSetEquA    = iNivelControl;
}

//////////////////////////////////////////////////////////////////////
//
void CControlAB::SpHsk()
{
	if (!m_simulador)
	{
		m_rts = GetRts();
		m_dtr = GetDtr();
	}
}
