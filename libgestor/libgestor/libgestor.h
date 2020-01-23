// libgestor.h

#pragma once
#include "snmp_pp/snmp_pp.h"

#ifdef WIN32
using namespace System;
#endif

namespace libgestor {
#define oidDirTraps		"1.1.100.80.0"
#define oidTraps		"1.1.100.90.0"

#ifdef WIN32
public ref class Def_Dato
#else
struct Def_Dato
#endif
{
public:
#ifdef WIN32
	String ^oid;
	String ^cadena;
#else
	char *oid;
	char *cadena;
#endif
	unsigned long valor;
	Def_Dato()
#ifdef WIN32
		:oid(nullptr),cadena(nullptr),valor(0)
#else
		:oid(NULL),cadena(NULL),valor(0)
#endif
	{};
};

struct Def_Gestor_Terminal
{
	int res;
	char dirip_agente[20];		// Dir IP agente
	char dirip_gestor[20];		// Dir IP gestor
	int puerto;			// Puerto escucha agente
	int puerto_traps;	// Puerto escucha de traps del gestor
//	int recurso;
	char recurso[100];
	int status;			// Para estado inicial
	Snmp *misnmp;		// Datos de sesion SNMP
	CTarget *mictarget;	// Datos del agente
	int t_rx;			// Para determinar cuando enviar polling
	int cont_polling;	// Para determinar num.polling sin respuesta
	bool conectado;		// Para indicar estado de conexion con agente

	char oidRaiz[200];
	char oidMax[200];
	char oidPolling[200];
#ifdef WIN32
	void *(*Aviso)(bool,String ^,int,array<Def_Dato ^> ^);	// Funcion para avisar de cambios
														// en conexion y valores
#else
	void *(*Aviso)(bool,char *,int,Def_Dato *);	// Funcion para avisar de cambios
												// en conexion y valores
#endif
	Def_Gestor_Terminal(
						char *recurso_in,
						char *dirip_agente_in,
						int puerto_in,
						char *dirip_gestor_in,
						int puerto_traps_in,
						char *oidRaiz_in,
						char *oidMax_in,
						char *oidPolling_in,
#ifdef WIN32
						void *(*Funcion_Aviso)(bool,String ^,int,array<Def_Dato ^> ^)
#else
						void *(*Funcion_Aviso)(bool,char *,int,Def_Dato *)
#endif
						);	
	~Def_Gestor_Terminal();

	void Inicializa();
	void Inicializa_Datos();
	void Polling();
	void Traps(bool suscripcion);
#ifdef WIN32
	void Cambia(int num,array<Def_Dato ^> ^);
#else
	void Cambia(int num,Def_Dato *);
#endif

};

#ifdef WIN32
using namespace System::Runtime::InteropServices;
public delegate void Funcion_Aviso(bool,String ^,int,array<Def_Dato ^> ^);
public ref class Def_Gestor_Terminal_MG
{
public:
	int res;

	event Funcion_Aviso ^ OnAviso
	{
		void add(Funcion_Aviso ^ f)
		{
			pEvent += f;
		}
		void remove(Funcion_Aviso ^ f)
		{
			pEvent -= f;
		}
		void raise(bool b, String ^ s, int i, array<Def_Dato ^> ^ ar)
		{
			Funcion_Aviso ^ tmp = pEvent;
			if (tmp)
				tmp->Invoke(b, s, i, ar);
		}
	}

	Def_Gestor_Terminal_MG(
						String ^recurso_in,		// identifica agente
						String ^ dirip_agente_in,
						int puerto_in,
						String ^ dirip_gestor_in,
						int puerto_traps_in,
						String ^ oidRaiz_in,
						String ^ oidMax_in,
						String ^ oidPolling_in
						)
	{
		pThis = this;
		ptr = new Def_Gestor_Terminal( 
			(char *)(void *)Marshal::StringToHGlobalAnsi(recurso_in),
			(char *)(void *)Marshal::StringToHGlobalAnsi(dirip_agente_in),
			puerto_in,
			(char *)(void *)Marshal::StringToHGlobalAnsi(dirip_gestor_in),
			puerto_traps_in,
			(char *)(void *)Marshal::StringToHGlobalAnsi(oidRaiz_in),
			(char *)(void *)Marshal::StringToHGlobalAnsi(oidMax_in),
			(char *)(void *)Marshal::StringToHGlobalAnsi(oidPolling_in),
			&FuncionAviso);
		res=ptr->res;
	}

	~Def_Gestor_Terminal_MG()
	{
		delete ptr;
	}

	void Traps(bool suscripcion)
	{
		ptr->Traps(suscripcion);
	}
	void Cambia(int num,array<Def_Dato ^> ^ ar)
	{
		ptr->Cambia(num, ar);
	}

private:
	Funcion_Aviso ^ pEvent;
	Def_Gestor_Terminal * ptr;
	static Def_Gestor_Terminal_MG ^ pThis;

	static void * FuncionAviso(bool b,String ^s,int c,array<Def_Dato ^> ^ ar)
	{
		pThis->OnAviso(b, s, c, ar);
		return NULL;
	}
};

#endif

#ifdef WIN32
extern int Gestor_Terminal(void *ptr);
#else
extern void *Gestor_Terminal(void *ptr);
#endif



}

