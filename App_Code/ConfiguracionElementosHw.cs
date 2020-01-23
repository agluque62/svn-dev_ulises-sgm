using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using CD40.BD;
using CD40.BD.Entidades;


namespace ConfiguracionElementosHw
{
    /// <summary>
    /// Summary description for ConfiguracionElementosHw
    /// </summary>
    /// 

    #region Parametros Multicast
    public class ParametrosMulticast
    {
        // Dirección IP del grupo multicast para configuración
        public string GrupoMulticastConfiguracion;
        // Puerto UDP para recepción de las notificaciones de configuración
        public uint PuertoMulticastConfiguracion;
    }
    #endregion

    #region Configuracion del sistema
    
    public class AsignacionUsuariosTV
    {
        public string IdUsuario;        // Nombre del usuario
        public string IdHost;    // Identificador del terminal de voz en la red
    }

    public class AsignacionRecursosGW
    {
        public string IdRecurso;        // Nombre del recurso
        public string IdHost;           // Identificador de la pasarela en la red
    }

    public class DireccionamientoSIP
    {
        public struct StrNumeroAbonado
        {
            // Prefijo del número de abonado
            public uint Prefijo;
            // Numero de abonado al que atiende el usuario
            public string NumeroAbonado;
        }

        // Nombre del usuario
        public string IdUsuario;
        public System.Collections.Generic.List<StrNumeroAbonado> NumerosAbonadoQueAtiende;
    }

    public class PlanRecursos
    {
        public string IdRecurso;    // Nombre del recurso
        public Tipos.TipoInterface Tipo;  // Tipo del recurso
    }

    public class ListaTroncales
    {
        public string IdTroncal;                // Nombre del troncal
        public string NumeroTest;               // Número de la red ATS para efectuar llamada de test
        public PlanRecursos[] ListaRecursos;    // Lista de recursos que componen el troncal
    }

    public class ListaRedes
    {
        public string IdRed;                    // Nombre de la red
        public uint Prefijo;                  // Prefijo para acceder a la red
        public PlanRecursos[] ListaRecursos;    // Lista de recursos que componen la red
    }

    public class PlanRutas
    {
        public string TipoRuta;             // Tipo de la ruta. D: Directa; A: Alternativa
        public string[] ListaTroncales;     // Lista de identificadores de troncales de componen la ruta
    }

    public class NumeracionATS
    {
        public bool CentralPropia;              // Distinguir si la central que se describe es la propia del CD40 u otra externa
        public bool Throwswitching;             // Indica si la central tiene capacidad de hacer throwswitching
        public RangosSCV[] RangosOperador;         // Lista de rangos de números del plan de numeración ATS para operadores
        public RangosSCV[] RangosPrivilegiados;    // Lista de rangos de números del plan de numeración ATS para operadores provilegiados
        public PlanRutas[] ListaRutas;          // Lista de rutas para el establecimiento de llamadas por la red ATS
		public string NumTest;					// Número de test que la central propia debe aceptar
												// sólo aplicable para la central propia. En el resto irá vacío.
	}

    public class DireccionamientoIP
    {
        public string IdHost;                   // Nombre con el que se identifica el elemento hardware en la red
        public string IpRed1;                   // Dirección IP para la red 1
        public string IpRed2;                   // Dirección IP para la red 2
        public Tipos.Tipo_Elemento_HW TipoHost;       // Tipo del elemento hardware 0: TOP 1: TIFX
    }

    public class ParametrosGeneralesSistema
    {
        public uint TiempoMaximoPTT;            // Tiempo máximo en PTT
        public uint TiempoSinJack1; 
        public uint TiempoSinJack2;
        public uint TamLiteralEnlExt;           // Tamaño de los literales en los enlaces externos
        public uint TamLiteralEnlDA;               // Tamaño de los literales en los enlades de acceso directo
        public uint TamLiteralEnlIA;               // Tamaño de los literales en los enlades de acceso inmediato
        public uint TamLiteralEnlAG;               // Tamaño de los literales en los enlades de la agenda
        public uint TamLiteralEmplazamiento;    // Tamaño de los literales en los emplazamientos
    }

    public class ConfiguracionSistema
    {
        // Estructura con los parámetros generales del CD40
        public ParametrosGeneralesSistema ParametrosGenerales;
        // Plan de numeración ATS
        public NumeracionATS[] PlanNumeracionATS;
        // Plan de direccionamiento IP
        public DireccionamientoIP[] PlanDireccionamientoIP;
        // Plan de troncales
        public ListaTroncales[] PlanTroncales;
        // Lista de redes
        public ListaRedes[] PlanRedes;
        // Tabla de asignación de usuarios a terminales de voz
        public AsignacionUsuariosTV[] PlanAsignacionUsuarios;
        // Tabla de asignación de recursos a pasarelas
        public AsignacionRecursosGW[] PlanAsignacionRecursos;
        // Plan de direccionamiento SIP
        public System.Collections.Generic.List<DireccionamientoSIP> PlanDireccionamientoSIP;
    }

    #endregion

    #region Configuracion de la TIFX
    public class CfgPasarela
    {
        // Nombre de la pasarela con el que se identifica en la red
        public string Nombre;
        // Numero de recursos que aloja
        public int NumRecursos;
        // Modo de sincronización 0: Ninguna; 1: IEE1588; 2: NTP; 3: Protocolo propietario
        public int ModoSincronizacion;
        // Dirección IP:Puerto de master de sincronización.
        public string MasterSincronizacion;
        // Puerto local SNMP
        public uint PuertoLocalSNMP;
        // Puerto remoto SNMP
        public uint PuertoRemotoSNMP;
        // Puerto remoto traps SNMP
        public uint PuertoRemotoTrapsSNMP;
        // Puerto local SIP
        public uint PuertoLocalSIP;
        // Periodo de supervisión de sesiones SIP. 0: no se supervisan
        public uint PeriodoSupervisionSIP;
        // Lista de recursos alojados en la pasarela
        public RecursosSCV[] ListaRecursos;
    }

    #endregion

    #region Configuracion de la TOP
    public class NumerosAbonado
    {
        public uint Prefijo;
        public string Numero;
    }

    public class LoginTerminalVoz
    {
        public string ModoLogin;    // Modo de arranque de la TOP. 'A':Automático; 'M':Manual
        public string IdUsuario;    // Identificador del usuario configurado en la top solicitada.
    }

    public class CfgUsuario
    {
		public string Nombre;   // Nombre de la agrupación
		public string IdIdentificador;   // Nombre original del sector (identificador del usuario dominante)
        public List<NumerosAbonado> ListaAbonados;  // Lista de números de abonado a los que atiende el usuario
        public int NumeroEnlacesInternos;   // Número de enlaces internos
        public int NumeroEnlacesExternos;   // Número de enlaces externos
        public SectoresSCV Sector;             // Incluye Tipo CWP, Prioridad, TipoHMI y Pareja UCS
        public ParametrosSectorSCVKeepAlive ParametrosDelSector;    // Incluye dimensionamiento de páginas, llamadas, enlaces, intrusión y ser intruído.
        public PermisosRedesSCV[] PermisosRedDelSector;   // Lista con los permisos (saliente y entrante) sobre las redes
        public TeclasSectorSCV TeclasDelSector;    // Configuración de la disponibilidad de funciones
        public NivelesSCV NivelesDelSector;        // Niveles para protocolo Q-SIG
    }

    #endregion

    #region Configuracion Enlaces Externos

    public class CfgRecursoEnlaceExterno
    {
        // Identificador del recurso
        public string IdRecurso;
        // Tipo del Recurso. 0..6
        public uint Tipo;
        // Estado de asignación I,A,S
        public string Estado;
        // Modo confirmación PTT. 0,1,2
        public uint ModoConfPTT;
        // Tipo PTT. 0..n
        public uint NumFlujosAudio;
        // Nombre del emplazamiento
        public string IdEmplazamiento;
    }

    public class CfgEnlaceExterno
    {
        // Literal del enlace
        public string Literal;
        // Lista de posiciones donde el enlace está configurado
        public List<uint> ListaPosicionesEnHmi;
        // Tipo de frecuencia
        public uint TipoFrecuencia;
        // Exclusividad en asignación de TxRx
        public bool ExclusividadTxRx;
        // Lista de recursos que atienden el enlace
        public List<CfgRecursoEnlaceExterno> ListaRecursos;
        // Estado de asignación
        public string EstadoAsignacion;
        // Lista de los posibles destinos de audio con su estado
        public List<string> DestinoAudio;
        // Prioridad del enlace
        public uint Prioridad;
    }
    #endregion

    #region Configuracion Enlaces Internos

    public class CfgRecursoEnlaceInterno
    {
        // Prefijo del destino
        public uint Prefijo;
        // Nombre del recurso que da servicio al destino o nombre del usuario interno
        public string NombreRecurso;
        // Número de abonado del destino
        public string NumeroAbonado;
    }

	public class CfgRecursoEnlaceInternoConInterface : CfgRecursoEnlaceInterno
	{
		// Tipo de interfaz del recurso
		public Tipos.TipoInterface Interface;
	}

    public class CfgEnlaceInterno
    {
        // Literal del enlace
        public string Literal;
        // Posición donde el enlace está configurado
        public uint PosicionHMI;
        // Tipo del enlace : DA, AI, AG
        public string TipoEnlaceInterno;
        // Lista de recursos que atienden al enlace
        public List<CfgRecursoEnlaceInternoConInterface> ListaRecursos;
        // Dependencia dentro de la cual se ubica el destino
        public string Dependencia;
        // Prioridad del enlace
        public uint Prioridad;
        // Identificador del usuario origen para hacer la llamada R2
        public string OrigenR2;
    }
    #endregion
}
