// ServConmTh.h: interface for the CServConmTh class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_SERVCONMTH_H__858A46B1_927A_404C_9ADC_0C288E0A491D__INCLUDED_)
#define AFX_SERVCONMTH_H__858A46B1_927A_404C_9ADC_0C288E0A491D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <time.h>
#include "ControlAB.h"

class CServConmTh : public CThread  
{
private:
	class CSpActividad
	{
	public:
		CSpActividad()
		{
			Enable();
		}
		~CSpActividad(){}

	public:
		BOOL IsEnable();
		void Enable()
		{
			m_maximo = m_cnt = 0;
		}
		void Disable()
		{
			m_cnt = 0;
			m_maximo = (m_maximo == 15) ? m_maximo : m_maximo+1;
			time(&m_time_next);m_time_next += 60;
		}

	protected:
		time_t m_time_next,m_time_now;
		int m_cnt;
		int m_maximo;
	};

public:
	long GetStatus(void );
	long GetEquipo(void );
	long SetEquipo(long iEquipo);
	void Run(void );
	CServConmTh();
	virtual ~CServConmTh();

private:
	CCritSec	m_acceso;
	CControlAB  m_ps;
	CUDPSocket  m_sck;
	CSpActividad m_permiso;

private:
	void AddHistorico(long id);
	int	 m_iEquipoLeido;
	long m_udpPort;
};

#endif // !defined(AFX_SERVCONMTH_H__858A46B1_927A_404C_9ADC_0C288E0A491D__INCLUDED_)
