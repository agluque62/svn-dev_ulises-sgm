// ControlAB.h: interface for the CControlAB class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_CONTROLAB_H__3F9E2C31_BCDC_4F8A_86BC_A33C83A7CBDA__INCLUDED_)
#define AFX_CONTROLAB_H__3F9E2C31_BCDC_4F8A_86BC_A33C83A7CBDA__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "PortSerie.h"

class CControlAB : public CPortSerie  
{
public:
	void SpHsk(void );
	void LoadConfig(void );
	void Stop(void );
	bool Start(void );
	void SetEquipo(int iEquipo);
	long QueEquipo(void );
	CControlAB();
	virtual ~CControlAB();

private:
	std::string	m_strPort;
	int			m_iMscSenal;
	int			m_iMscControl;
	BOOL		m_bSetEquA;
	BOOL		m_bGetEquA;

private:
	long		m_simulador;
	long		m_equipo_sel;

private:
	long		m_rts,m_dtr;

};

#endif // !defined(AFX_CONTROLAB_H__3F9E2C31_BCDC_4F8A_86BC_A33C83A7CBDA__INCLUDED_)
