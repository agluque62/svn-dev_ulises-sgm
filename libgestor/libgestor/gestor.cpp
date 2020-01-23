// This is the main DLL file.

#ifdef WIN32
#include "stdafx.h"

#include "libgestor.h"
#else
#include <poll.h>
#include "gestor.h"
#endif

namespace libgestor
{
#define TIEMPO_POLLING	20000
#define MAX_POLLING_SIN_RESPUESTA	3

void callback(int reason,Snmp *snmp,Pdu &pdu,SnmpTarget &target,void *cd);
void callback1(int reason,Snmp *snmp,Pdu &pdu,SnmpTarget &target,void *cd);
void callbackpolling(int reason,Snmp *snmp,Pdu &pdu,SnmpTarget &target,void *cd);

#ifdef WIN32
int Gestor_Terminal(void *ptr)
#else
void *Gestor_Terminal(void *ptr)
#endif
{
	Def_Gestor_Terminal *gtr_term=(Def_Gestor_Terminal *)ptr;
	const int INTERVALO=1000;	// intervalo en ms

	// Para Windows
	Snmp::socket_startup();

	UdpAddress address(gtr_term->dirip_agente);
	address.set_port(gtr_term->puerto);
	CTarget ctarget(address);
	ctarget.set_version(version2c);
	gtr_term->mictarget=&ctarget;
	gtr_term->Inicializa();

	// datos para respuestas asincronas
	int nfds;
	fd_set fdr,fdw,fde;
	struct timeval timeout;
	timeout.tv_sec=0;
	//timeout.tv_usec=50000;
	timeout.tv_usec=INTERVALO*1000;

	while(1)
	{
		// Siempre que no haya sesion trata de establecerla
		if (!gtr_term->misnmp)
		{
			printf("Trata de establecer sesion\n");
			gtr_term->Inicializa();
		}

		// Tratamiento de polling (solo entrara aqui si no se reciben traps
		// ni respuestas a solicitudes) 
		if (gtr_term->t_rx>=TIEMPO_POLLING)
		{
			printf("Pide estado TOP\n");
			// Pide estado TOP, siempre que haya sesion
			gtr_term->Polling();
			gtr_term->t_rx=0;
		}

		// Configura la espera de respuestas/traps
		gtr_term->misnmp->eventListHolder->SNMPGetFdSets(nfds,fdr,fdw,fde);
		struct timeval timeout_int=timeout;
		if (select(nfds,&fdr,NULL,NULL,&timeout_int)!=0)
			gtr_term->misnmp->eventListHolder->SNMPProcessPendingEvents();
#ifdef WIN32
		Sleep(INTERVALO);
#endif
		gtr_term->t_rx+=INTERVALO;
	}


	// Para Windows
	Snmp::socket_cleanup();
}

Def_Gestor_Terminal::Def_Gestor_Terminal(int &res,
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
						)
{
	misnmp=NULL;
	mictarget=NULL;
	strcpy(recurso,recurso_in);
	strcpy(dirip_agente,dirip_agente_in);
	strcpy(dirip_gestor,dirip_gestor_in);
	puerto=puerto_in;
	puerto_traps=puerto_traps_in;
	strcpy(oidRaiz,oidRaiz_in);
	strcpy(oidMax,oidMax_in);
	strcpy(oidPolling,oidPolling_in);
	t_rx=0;
	cont_polling=0;
	conectado=FALSE;
	Aviso=Funcion_Aviso;

	// Aqui deberia lanzar hilo que se encargara de interrogar peridicamente
	// al agente para determinar si aun sigue vivo
#ifdef WIN32
	DWORD tid;
	res=0;
	if (CreateThread(NULL,0,(LPTHREAD_START_ROUTINE)&Gestor_Terminal,this,0,&tid)==NULL)
	{
		res=-1;
		return;
	}
#else
	pthread_t tid;
	res=pthread_create(&tid,NULL,Gestor_Terminal,this);
	if (res==0)
		pthread_detach(tid);
#endif

#ifdef WIN32
		Sleep(1000);
#else
		poll(NULL,0,50);
#endif

	if (status!=SNMP_CLASS_SUCCESS)
		res=-1;
}

void Def_Gestor_Terminal::Inicializa()
{
	misnmp=new Snmp(status);
	if (status!=SNMP_CLASS_SUCCESS)
	{
		misnmp=NULL;
		return;
	}
	// PARA RECOGER TRAPS
	if (puerto_traps>0)
	{
		misnmp->notify_set_listen_port(puerto_traps);
		OidCollection coloids;
		TargetCollection coltargets;
		misnmp->notify_register(coloids,coltargets,callback,this);
		// Indica al agente que quiere recoger traps
		Traps(TRUE);
	}
	Inicializa_Datos();
}

void Def_Gestor_Terminal::Inicializa_Datos()
{
	// Lee todas las variables del terminal
	Pdu pdu;
	Vb vb;
	vb.set_oid(oidRaiz);
	pdu+=vb;

	// Para preguntar de forma asincrona (es la libreria la que
	// controla)
	// Manda como argumento la propia estructura del gestor
	int res=misnmp->get_bulk(pdu,*mictarget,0,50,callback,this);
	if (res!=SNMP_CLASS_SUCCESS)
		printf("Error en GET ASINC%s\n",misnmp->error_msg(status));
}

void Def_Gestor_Terminal::Polling()
{
	if (!misnmp)
		return;

	// Control de polling enviados sin respuesta (ya que cada vez que se RX
	// una se resetea contador)
	if (++cont_polling>MAX_POLLING_SIN_RESPUESTA)
	{
		// Si cambia estado de conexion con agente anota y avisa
		if (conectado)
		{
			conectado=FALSE;
			if (Aviso)
			{
#ifdef WIN32
				String ^srecurso=gcnew String(recurso);
				Aviso(conectado,srecurso,0,nullptr);
#else
				Aviso(conectado,recurso,0,NULL);
#endif
			}
		}
		cont_polling=0;
	}

	// Genera y envia solicitud del estado del TOP para el agente
	Pdu pdu;
	Vb vb;
	vb.set_oid(oidPolling);
	pdu+=vb;

	// Para preguntar de forma asincrona (es la libreria la que
	// controla)
	// Manda como argumento la propia estructura del gestor
	int res=misnmp->get(pdu,*mictarget,callbackpolling,this);
	if (res!=SNMP_CLASS_SUCCESS)
		printf("Error en GET ASINC%s\n",misnmp->error_msg(res));
}


void Def_Gestor_Terminal::Traps(bool suscripcion)
{
	if (!misnmp)
		return;
	Pdu pdu;
	Vb vb;
	char cadena[50];
	sprintf(cadena,"%s/%d",dirip_gestor,puerto_traps);
	vb.set_oid(oidDirTraps);
	vb.set_syntax(sNMP_SYNTAX_OCTETS);
	OctetStr octetstr(cadena);
	vb.set_value(octetstr);
	pdu+=vb;
	vb.set_oid(oidTraps);
	vb.set_syntax(sNMP_SYNTAX_INT);
	vb.set_value((long)suscripcion);
	pdu+=vb;
	// Para preguntar de forma asincrona (es la libreria la que
	// controla)
	// Manda como argumento la propia estructura del gestor
	// Lo hace sincrono ya que no interesa la respuesta que no se va
	// a comunicar al padre
#ifndef WIN32
	int res=misnmp->set(pdu,*mictarget,callback1,this);
#else
	int res=misnmp->set(pdu,*mictarget);
#endif
	if (res!=SNMP_CLASS_SUCCESS)
		printf("Error en SET ASINC%s\n",misnmp->error_msg(res));
}

#ifdef WIN32
using namespace System::Runtime::InteropServices;
void Def_Gestor_Terminal::Cambia(int num,array<Def_Dato ^> ^datos)
#else
void Def_Gestor_Terminal::Cambia(int num,Def_Dato *datos)
#endif
{
	if (!misnmp)
		return;
	Pdu pdu;
	Vb vb;
	for (int i=0;i<num;i++)
	{
#ifdef WIN32
		char *cadena_oid=(char *)(void *)Marshal::StringToHGlobalAnsi(datos[i]->oid);
		vb.set_oid(cadena_oid);
		Marshal::FreeHGlobal((System::IntPtr)cadena_oid);
		if (datos[i]->cadena)
		{
			vb.set_syntax(sNMP_SYNTAX_OCTETS);
			char *cadena_cadena=(char *)(void *)Marshal::StringToHGlobalAnsi(datos[i]->cadena);
			OctetStr octetstr(cadena_cadena);
			vb.set_value(octetstr);
			Marshal::FreeHGlobal((System::IntPtr)cadena_cadena);
		}
		else
		{
			vb.set_syntax(sNMP_SYNTAX_INT);
			vb.set_value((long)datos[i]->valor);
		}
#else
		vb.set_oid(datos[i].oid);
		vb.set_syntax(sNMP_SYNTAX_OCTETS);
		if (datos[i].cadena)
		{
			OctetStr octetstr(datos[i].cadena);
			vb.set_value(octetstr);
		}
		else
		{
			vb.set_syntax(sNMP_SYNTAX_INT);
			vb.set_value((long)datos[i].valor);
		}
#endif
		pdu+=vb;
	}
	// Manda como argumento la propia estructura del gestor
	// Lo hace sincrono, ya que el cambio llegara por trap que mande agente
	// y si no se hace asi llegaria dos veces
#ifndef WIN32
	int res=misnmp->set(pdu,*mictarget,callback1,this);
#else
	int res=misnmp->set(pdu,*mictarget);
#endif
	if (res!=SNMP_CLASS_SUCCESS)
		printf("Error en SET SINC%s\n",misnmp->error_msg(res));
}

Def_Gestor_Terminal::~Def_Gestor_Terminal()
{
	Snmp::socket_cleanup();
}

void callback(int reason,Snmp *snmp,Pdu &pdu,SnmpTarget &target,void *cd)
{
	Def_Gestor_Terminal *gtr_term=(Def_Gestor_Terminal *)cd;
	if (reason==SNMP_CLASS_TIMEOUT)
		return;
	if (reason==SNMP_CLASS_SESSION_DESTROYED)
	{
		// OJOOOOO Tiene que informar al padre de este cambio
		printf("FIN SESION\n");
		gtr_term->misnmp=NULL;
		return;
	}
	// Ha recibido algo por lo que inicializa y si estaba desconectado
	// anota nuevo estado y avisa del cambio
	gtr_term->t_rx=0;
	gtr_term->cont_polling=0;
	if (!gtr_term->conectado)
	{
		gtr_term->conectado=TRUE;
		// Indica al agente que quiere rx traps y sus datos
		gtr_term->Traps(TRUE);
		// Solicita de nuevo todos los datos del terminal
		gtr_term->Inicializa_Datos();
		if (gtr_term->Aviso)
		{
#ifdef WIN32
			String ^recurso=gcnew String(gtr_term->recurso);
			gtr_term->Aviso(gtr_term->conectado,recurso,0,nullptr);
#else
			gtr_term->Aviso(gtr_term->conectado,gtr_term->recurso,0,NULL);
#endif
		}
	}
#ifdef WIN32
	array<Def_Dato ^> ^datos=gcnew array<Def_Dato ^>(pdu.get_vb_count());
#else
	Def_Dato *datos=new Def_Dato[pdu.get_vb_count()];
#endif
	int ind=0;
	Vb vb;
	//Oid oidraiz(oidRaiz);
	//Oid oidmax(oidMax);
	Oid oidraiz(gtr_term->oidRaiz);
	Oid oidmax(gtr_term->oidMax);
	for (int i=0;i<pdu.get_vb_count();i++)
	{
#ifdef WIN32
		datos[ind]=gcnew Def_Dato();
#endif
		pdu.get_vb(vb,i);
		Oid oid=vb.get_oid();
		// Solo se ocupa de los de MIB de terminales que no sean de tipo 
		// interno
		if (oid.nCompare(oidraiz.len(),oidraiz)==0 && 
			oid.nCompare(oidmax.len(),oidmax)<0)
		{
			printf("%s OID %s VALOR %s\n",
				reason==SNMP_CLASS_NOTIFICATION ? "TRAP" : "RESPUESTA",
				vb.get_printable_oid(),
				vb.get_printable_value());
			if (!vb.get_printable_value())
				continue;
#ifdef WIN32
			datos[ind]->oid=gcnew String(vb.get_printable_oid());
#else
			datos[ind].oid=new char[strlen(vb.get_printable_oid())+1];
			strcpy(datos[ind].oid,vb.get_printable_oid());
#endif
			SmiUINT32 tipo=vb.get_syntax();
			if (tipo==sNMP_SYNTAX_OCTETS)
			{
#ifdef WIN32
				datos[ind++]->cadena=gcnew String(vb.get_printable_value());
#else
				datos[ind].cadena=new char[strlen(vb.get_printable_value())+1];
				strcpy(datos[ind++].cadena,vb.get_printable_value());
#endif
			}
			else if (tipo==sNMP_SYNTAX_INT)
			{
				int valor;
				vb.get_value(valor);
#ifdef WIN32
				datos[ind++]->valor=valor;
#else
				datos[ind++].valor=valor;
#endif
			}
			else if (tipo==sNMP_SYNTAX_TIMETICKS)
			{
				unsigned long valor;
				vb.get_value(valor);	
#ifdef WIN32
				datos[ind]->cadena=gcnew String(vb.get_printable_value());
				datos[ind++]->valor=valor;
#else
				datos[ind].cadena=new char[strlen(vb.get_printable_value())+1];
				strcpy(datos[ind].cadena,vb.get_printable_value());
				datos[ind++].valor=valor;
#endif
			}
		}
		else
			printf("OTROOOO OID %s VALOR %s\n",
				vb.get_printable_oid(),
				vb.get_printable_value());
	}
	if (gtr_term->Aviso && ind>0)
	{
#ifdef WIN32
		String ^recurso=gcnew String(gtr_term->recurso);
		gtr_term->Aviso(gtr_term->conectado,recurso,ind,datos);
#else
		gtr_term->Aviso(gtr_term->conectado,gtr_term->recurso,ind,datos);
#endif
	}
#ifndef WIN32
	else
		delete []datos;
#endif
}

// Para respuestas a set ( no tiene que avisar a nadie, ya que el cambio de
// estado llegara por trap=>llamada a callback)
// Solo sirven para determinar que sigue vivo agente
void callback1(int reason,Snmp *snmp,Pdu &pdu,SnmpTarget &target,void *cd)
{
	Def_Gestor_Terminal *gtr_term=(Def_Gestor_Terminal *)cd;
	if (reason==SNMP_CLASS_TIMEOUT)
		return;
	if (reason==SNMP_CLASS_SESSION_DESTROYED)
	{
		// OJOOOOO Tiene que informar al padre de este cambio
		printf("FIN SESION\n");
		gtr_term->misnmp=NULL;
		return;
	}
	// Ha recibido algo por lo que inicializa y si estaba desconectado
	// anota nuevo estado y avisa del cambio
	gtr_term->t_rx=0;
	gtr_term->cont_polling=0;
	if (!gtr_term->conectado)
	{
		gtr_term->conectado=TRUE;
		// Indica al agente que quiere rx traps y sus datos
		gtr_term->Traps(TRUE);
		// Solicita de nuevo todos los datos del agente
		gtr_term->Inicializa_Datos();
		if (gtr_term->Aviso)
		{
#ifdef WIN32
			String ^recurso=gcnew String(gtr_term->recurso);
			gtr_term->Aviso(gtr_term->conectado,recurso,0,nullptr);
#else
			gtr_term->Aviso(gtr_term->conectado,gtr_term->recurso,0,NULL);
#endif
		}
	}
}

void callbackpolling(int reason,Snmp *snmp,Pdu &pdu,SnmpTarget &target,void *cd)
{
	Def_Gestor_Terminal *gtr_term=(Def_Gestor_Terminal *)cd;
	static int valorpolling=-1;
	if (reason==SNMP_CLASS_TIMEOUT)
		return;
	if (reason==SNMP_CLASS_SESSION_DESTROYED)
	{
		// OJOOOOO Tiene que informar al padre de este cambio
		printf("FIN SESION\n");
		gtr_term->misnmp=NULL;
		return;
	}
	Vb vb;
	pdu.get_vb(vb,0);
	if (!vb.get_printable_value())
		printf("RESPONDE MAL\n");
	// Ha recibido algo por lo que inicializa y si estaba desconectado
	// anota nuevo estado y avisa del cambio
	gtr_term->t_rx=0;
	gtr_term->cont_polling=0;
	if (!gtr_term->conectado)
	{
		gtr_term->conectado=TRUE;
		// Indica al agente que quiere rx traps y sus datos
		gtr_term->Traps(TRUE);
		// Solicita de nuevo todos los datos del terminal
		gtr_term->Inicializa_Datos();
		if (gtr_term->Aviso)
		{
#ifdef WIN32
			String ^recurso=gcnew String(gtr_term->recurso);
			gtr_term->Aviso(gtr_term->conectado,recurso,0,nullptr);
#else
			gtr_term->Aviso(gtr_term->conectado,gtr_term->recurso,0,NULL);
#endif
		}
	}
#ifdef WIN32
	array<Def_Dato ^> ^datos=gcnew array<Def_Dato ^>(1);
#else
	Def_Dato *datos=new Def_Dato[1];
#endif
	int ind=0;
#ifdef WIN32
	datos[ind]=gcnew Def_Dato();
#endif
	pdu.get_vb(vb,0);
	if (vb.get_printable_value())
	{
		int valor;
		vb.get_value(valor);
		if (valor!=valorpolling)
		{
			valorpolling=valor;
#ifdef WIN32
			datos[ind]->oid=gcnew String(vb.get_printable_oid());
			datos[ind++]->valor=valor;
#else
			datos[ind].oid=new char[strlen(vb.get_printable_oid())+1];
			strcpy(datos[ind].oid,vb.get_printable_oid());
			datos[ind++].valor=valor;
#endif
		}
	}
	if (gtr_term->Aviso && ind>0)
	{
#ifdef WIN32
		String ^recurso=gcnew String(gtr_term->recurso);
		gtr_term->Aviso(gtr_term->conectado,recurso,ind,datos);
#else
		gtr_term->Aviso(gtr_term->conectado,gtr_term->recurso,ind,datos);
#endif
	}
#ifndef WIN32
	else
		delete []datos;
#endif
}

}	// fin namespace libgestor
