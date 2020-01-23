using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using log4net;
using log4net.Config;
using System.Data;
using System.Configuration;
using System.Text;
using System.Globalization;
using System.Resources;
using System.Threading;
using CrystalDecisions.CrystalReports.Engine;

public partial class InformeSectorizacion : System.Web.UI.Page
{
    private static ILog _logDebugView;

    static string sFicheroReport;

    const string CONF_KEY_CON_SACTA = "SistemaConSACTA";
    const string CONF_KEY_SISTEMA   = "Sistema";
    const string ID_SECTOR_FS       = "**FS**";

    //Nombres de los Ficheros de crystal report que contienen los informes
    const string CR_INF_SECTORIZACION = "InfSectorizacion.rpt";                       // Informe de CR de Sectorizacion

    //Subinformes contenidos en el informe CR_INF_SECTORES
    const string CR_INF_SCT_SUBINF_PERMISOS = "SubInformePermisosRedes.rpt";          //Subinforme permisos redes del sector
    const string CR_INF_SCT_SUBINF_ABONADOS = "SubInformeAbonadosSector.rpt";         //Subinforme números de abonados del sector
    const string CR_INF_SCT_SUBINF_PANEL_TLF = "SubInfSectorizacionPanelTlf.rpt";     //Subinforme panel de telefonía
    const string CR_INF_SCT_SUBINF_PANEL_LC = "SubInfSectorizacionPanelLC.rpt";       //Subinforme panel de Linea Caliente
    const string CR_INF_SCT_SUBINF_PANEL_RADIO = "SubInfSectorizacionPanelRadio.rpt"; //Subinforme panel de Linea Caliente
    const string CR_INF_SCT_SUBINF_SECTORES = "SubInformeSectoresTop.rpt";            //Subinforme  de sectores en TOP
    const string CR_INT_SCT_SUBINF_PO_FS = "SubInformeTopFS.rpt";                     //Subinformes TOPS fuera de sectorización

    static DataSet dtst;
    static DataSet dtstPermisosRedes;
    static DataSet dtstUsuAbonados;
    static DataSet dtstSectores;
    static DataSet dtstDestinosPanelTlf;
    static DataSet dtstDestinosPanelLC;
    static DataSet dtstDestinosPanelRadio;
    static DataSet dtsTopFS; 
    static string strVersion;
    static string strNucleo;
    static string strIdSistema;
    static string strIdSectorizacion;
    static bool   bConSafta;
    static bool bExistenTopsFS;
    static bool bSectorizacionActiva;

    public static ILog logDebugView
    {
        get
        {
            if (_logDebugView == null)
            {
                log4net.Config.XmlConfigurator.Configure();
                _logDebugView = LogManager.GetLogger("CONFIGURACION");
            }
            return _logDebugView;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        //Si no está autentificado se redirige a la pagina de login
        if (Context.Request.IsAuthenticated)
        {
            // retrieve user's identity from httpcontext user 
            FormsIdentity ident = (FormsIdentity)Context.User.Identity;
            string perfil = ident.Ticket.UserData;
            //A esta pantalla tienen accesos todos los usuarios menos los operadores
            if (string.Compare(perfil, "0") == 0)
            {
                Response.Redirect("~/Login.aspx", false);
                return;
            }
        }
        else
        {
            Response.Redirect("~/Login.aspx", false);
            return;
        }

        if (!IsPostBack)
        {
            StringBuilder strConsulta = new StringBuilder();

            strVersion = strNucleo = string.Empty;
            dtst = null;
            dtstPermisosRedes = null;
            dtstUsuAbonados = null;
            dtstDestinosPanelTlf = null;
            dtstDestinosPanelLC = null;
            dtstDestinosPanelRadio = null;
            bExistenTopsFS = false;
            bSectorizacionActiva=false;

            /*
            CRVInfSectorizacion.DisplayToolbar = true;
            CRVInfSectorizacion.EnableParameterPrompt = false;
            CRVInfSectorizacion.EnableDatabaseLogonPrompt = false;
            CRVInfSectorizacion.Visible = false;
            CRVInfSectorizacion.HasToggleGroupTreeButton = false;
            CRVInfSectorizacion.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CRVInfSectorizacion.HasToggleParameterPanelButton = false;
            CRVInfSectorizacion.HasDrilldownTabs = false; //Se oculta la pestaña pagina principal
            CRVInfSectorizacion.HasExportButton = false; //Se ocultan las opciones de exportacion de la Toolbar
            CRVInfSectorizacion.HasCrystalLogo = false; //Se oculta el logo de Crystal Reports
            CRVInfSectorizacion.HasPrintButton = true; //Se muestra la opción de imprimir
            */
            string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            strIdSectorizacion = (string)Request.Params["SECTORIZACION"];

            if (!string.IsNullOrEmpty(strIdSectorizacion))
            {
                ServiciosCD40.ServiciosCD40 Servicio = new ServiciosCD40.ServiciosCD40();

                //Se obtiene el valor del parámetro Sistema, configurado en el fichero Web.config
                Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement objConf = config.AppSettings.Settings[CONF_KEY_SISTEMA];
                strIdSistema = objConf.Value;

                bConSafta = false;
                //Se obtiene el parametro que indica si se debe o no visualizar la posición Sacta
                objConf = config.AppSettings.Settings[CONF_KEY_CON_SACTA];

                if ((objConf != null) && (!string.IsNullOrEmpty(objConf.Value) && string.Compare(objConf.Value, "SI", true) == 0))
                {
                    //El sistema está configurado con el sistema SACTA
                    bConSafta = true;
                }

                //Obtenemos el Núcleo, que se pasa como parámetro en todos los informes 
                ServiciosCD40.Nucleos tabNucleo = new ServiciosCD40.Nucleos();

                tabNucleo.IdSistema = strIdSistema;
                ServiciosCD40.Tablas[] dTabNucleos = Servicio.ListSelectSQL(tabNucleo);

                if (null != dTabNucleos && dTabNucleos.Length > 0)
                {
                    //Nos quedamos con el primer registro, porque en el sistema sólo se puede definir un nucleo
                    strNucleo = ((ServiciosCD40.Nucleos)dTabNucleos[0]).IdNucleo;
                    dTabNucleos = null; //Liberamos la memoria
                }

                //Comprobamos si se trata de la sectorización activa
                ServiciosCD40.Tablas[] objListaSectorizaciones = null;
                ServiciosCD40.Sectorizaciones objSectorizacion = new ServiciosCD40.Sectorizaciones();

                objSectorizacion.IdSistema = strIdSistema;
                objSectorizacion.IdSectorizacion = strIdSectorizacion;
                DateTime objFechaActivacion= new DateTime();

                objListaSectorizaciones = Servicio.ListSelectSQL(objSectorizacion);

                if (objListaSectorizaciones != null && objListaSectorizaciones.Length > 0)
                {
                    if (((ServiciosCD40.Sectorizaciones)objListaSectorizaciones[0]).Activa == true)
                    {
                        bSectorizacionActiva = true;
                        objFechaActivacion = ((ServiciosCD40.Sectorizaciones)objListaSectorizaciones[0]).FechaActivacion;
                    }
                    
                }

                sFicheroReport = Server.MapPath(CR_INF_SECTORIZACION);

                //Se obtienen los datos a visualizar en el informe
                //Lista de Sectores sectorizados y sus parámetros, se excluyen los puestos de operador que están fuera de sectorización 
                strConsulta.Append("SELECT sectoressectorizacion.IdSistema, sectoressectorizacion.IdSectorizacion, sectoressectorizacion.IdNucleo, sectoressectorizacion.IdSector, ");
                strConsulta.Append("sectoressectorizacion.IdTOP, sectores.Tipo, sectores.TipoPosicion, sectores.PrioridadR2, sectores.TipoHMI, parametrossector.NumLlamadasEntrantesIDA, ");
                strConsulta.Append("parametrossector.NumFreqPagina, parametrossector.NumLlamadasEnIDA, parametrossector.NumPagFreq, parametrossector.NumDestinosInternosPag, ");
                strConsulta.Append("parametrossector.NumPagDestinosInt, parametrossector.Intrusion, parametrossector.Intruido, parametrossector.KeepAlivePeriod, ");
                strConsulta.Append("parametrossector.KeepAliveMultiplier, parametrossector.NumEnlacesAI, parametrossector.GrabacionEd137, teclassector.TransConConsultaPrev, ");
                strConsulta.Append("teclassector.TransDirecta, teclassector.Conferencia, teclassector.Escucha, teclassector.Retener, teclassector.Captura, teclassector.Redireccion, ");
                strConsulta.Append("teclassector.RepeticionUltLlamada, teclassector.RellamadaAut, teclassector.TeclaPrioridad, teclassector.Tecla55mas1, teclassector.Monitoring, ");
                strConsulta.Append("teclassector.CoordinadorTF, teclassector.CoordinadorRD, teclassector.IntegracionRDTF, teclassector.LlamadaSelectiva, teclassector.GrupoBSS, teclassector.LTT, ");
                strConsulta.Append("teclassector.InhabilitacionRedirec, teclassector.SayAgain, teclassector.Glp ");
                strConsulta.Append("FROM sectoressectorizacion INNER JOIN sectores ON sectoressectorizacion.IdSistema = sectores.IdSistema AND sectoressectorizacion.IdNucleo = sectores.IdNucleo AND ");
                strConsulta.Append("sectoressectorizacion.IdSector = sectores.IdSector INNER JOIN ");
                strConsulta.Append("parametrossector ON sectores.IdSistema = parametrossector.IdSistema AND sectores.IdNucleo = parametrossector.IdNucleo AND ");
                strConsulta.Append("sectores.IdSector = parametrossector.IdSector INNER JOIN ");
                strConsulta.Append("teclassector ON sectores.IdSistema = teclassector.IdSistema AND sectores.IdNucleo = teclassector.IdNucleo AND sectores.IdSector = teclassector.IdSector ");
                strConsulta.AppendFormat(" WHERE sectoressectorizacion.IdSistema='{0}' AND sectoressectorizacion.idSectorizacion='{1}' AND sectoressectorizacion.IdSector!='{2}' ", strIdSistema, strIdSectorizacion, ID_SECTOR_FS);
                strConsulta.Append("ORDER BY sectoressectorizacion.idSectorizacion, sectoressectorizacion.idtop");

                dtst = Servicio.ObtenerDataSet(strConsulta.ToString());

                if (dtst != null && dtst.Tables.Count > 0)
                {
                    dtst.Tables[0].TableName = "Sectores";

                    strConsulta.Clear();

                    //Lista de Permisos redes de los sectores
                    ServiciosCD40.PermisosRedes tabPermisosRedes = new ServiciosCD40.PermisosRedes();
                    tabPermisosRedes.IdSistema = strIdSistema;

                    strConsulta.Append("SELECT IdSistema, IdRed, IdSector, IdNucleo, Llamar, Recibir FROM permisosredes P ");
                    strConsulta.Append("WHERE EXISTS(SELECT 1 FROM SectoresSector S, sectoressectorizacion SS  WHERE SS.IdSistema = S.IdSistema AND SS.IdNucleo = S.IdNucleo AND  SS.IdSector = S.IdSector ");
                    strConsulta.AppendFormat("AND S.IdSector=P.IdSector AND S.IdSistema=P.IdSistema  AND S.IdNucleo=P.IdNucleo AND S.IdSistema='{0}' AND SS.idSectorizacion='{1}') ", strIdSistema, strIdSectorizacion);
                    dtstPermisosRedes = Servicio.ObtenerDataSet(strConsulta.ToString());

                    strConsulta.Clear();

                    //Lista de sectores asociados a cada puesto de operador
                    strConsulta.Append("SELECT  sectoressectorizacion.IdSistema, sectoressectorizacion.IdSectorizacion, sectoressectorizacion.IdNucleo, sectoressectorizacion.IdSector, ");
                    strConsulta.Append("sectoressectorizacion.IdTOP, sectoressector.IdSectorOriginal, sectoressector.EsDominante, sectores.NumSacta ");
                    strConsulta.Append("FROM sectoressectorizacion INNER JOIN ");
                    strConsulta.Append("sectoressector ON sectoressectorizacion.IdSistema = sectoressector.IdSistema AND sectoressectorizacion.IdNucleo = sectoressector.IdNucleo AND ");
                    strConsulta.Append("sectoressectorizacion.IdSector = sectoressector.IdSector INNER JOIN ");
                    strConsulta.Append("sectores ON sectoressector.IdSistema = sectores.IdSistema AND sectoressector.IdNucleo = sectores.IdNucleo AND ");
                    strConsulta.Append("sectoressector.IdSectorOriginal = sectores.IdSector ");
                    strConsulta.AppendFormat("WHERE sectoressectorizacion.IdSistema='{0}' AND sectoressectorizacion.idSectorizacion='{1}' AND sectoressectorizacion.idSector!='{2}' ", strIdSistema, strIdSectorizacion, ID_SECTOR_FS);

                    strConsulta.Append("ORDER BY sectoressectorizacion.IdSectorizacion, sectoressectorizacion.IdSector, sectoressector.EsDominante DESC ");

                    dtstSectores = Servicio.ObtenerDataSet(strConsulta.ToString());

                    strConsulta.Clear();

                    //Lista de Abonados a los que atiende cada sector de la sectorización
                    strConsulta.Append("SELECT U.IdSistema, U.IdPrefijo, U.IdNucleo, U.IdSector,");
                    strConsulta.Append("U.IdAbonado, R.IdRed ");
                    strConsulta.Append("FROM usuariosabonados U inner JOIN ");
                    strConsulta.Append("redes R ON R.IdSistema = R.IdSistema AND U.IdPrefijo = R.IdPrefijo ");
                    strConsulta.AppendFormat("WHERE U.IdSistema='{0}' AND ", strIdSistema);

                    strConsulta.Append("EXISTS(SELECT 1 FROM SectoresSector S, sectoressectorizacion SS  WHERE SS.IdSistema = S.IdSistema AND SS.IdNucleo = S.IdNucleo AND  SS.IdSector = S.IdSector ");
                    strConsulta.AppendFormat("AND S.IdSector=U.IdSector AND S.IdSistema=U.IdSistema  AND S.IdNucleo=U.IdNucleo AND S.IdSistema='{0}' AND SS.idSectorizacion='{1}') ", strIdSistema, strIdSectorizacion);

                    dtstUsuAbonados = Servicio.ObtenerDataSet(strConsulta.ToString());

                    strConsulta.Clear();

                    //Se obtienen los destinos del panel de telefonía de todos los sectores de la sectorizacion
                    strConsulta.Append("SELECT IdSectorizacion, IdSistema, IdNucleo, IdSector, IdDestino, IdPrefijo, PosHMI, Prioridad, OrigenR2, PrioridadSIP, TipoAcceso, Literal, NULL AS Grupo ");
                    strConsulta.AppendFormat("FROM internos WHERE IdSistema='{0}' AND idSectorizacion='{1}' AND TipoAcceso='DA' ", strIdSistema, strIdSectorizacion);
                    strConsulta.Append("UNION  SELECT IdSectorizacion, IdSistema, IdNucleo, IdSector, IdDestino, IdPrefijo, PosHMI, Prioridad, OrigenR2, PrioridadSIP, TipoAcceso, Literal, Grupo ");
                    strConsulta.AppendFormat("FROM externos WHERE IdSistema='{0}' AND idSectorizacion='{1}' AND TipoAcceso='DA'  AND IdPrefijo<>1 ", strIdSistema, strIdSectorizacion);
                    dtstDestinosPanelTlf = Servicio.ObtenerDataSet(strConsulta.ToString());

                    strConsulta.Clear();

                    //Se obtienen los destinos del panel de Línea Caliente
                    strConsulta.Append("SELECT IdSectorizacion, IdSistema, IdNucleo, IdSector, IdDestino, IdPrefijo, PosHMI, Prioridad, OrigenR2, PrioridadSIP, TipoAcceso, Literal, NULL AS Grupo ");
                    strConsulta.AppendFormat("FROM internos WHERE IdSistema='{0}' AND idSectorizacion='{1}' AND TipoAcceso='IA' ", strIdSistema, strIdSectorizacion);
                    strConsulta.Append("UNION  SELECT IdSectorizacion, IdSistema, IdNucleo, IdSector, IdDestino, IdPrefijo, PosHMI, Prioridad, OrigenR2, PrioridadSIP, TipoAcceso, Literal, Grupo ");
                    strConsulta.AppendFormat("FROM externos WHERE IdSistema='{0}' AND idSectorizacion='{1}' AND TipoAcceso='IA'  AND IdPrefijo=3 ", strIdSistema, strIdSectorizacion);
                    dtstDestinosPanelLC = Servicio.ObtenerDataSet(strConsulta.ToString());

                    strConsulta.Clear();

                    //Se obtienen los destinos del panel de radio
                    strConsulta.Append("SELECT IdSistema, IdDestino, IdNucleo, IdSector, PosHMI, Prioridad, PrioridadSIP, ModoOperacion, Cascos, Literal, SupervisionPortadora, IdSectorizacion ");
                    strConsulta.AppendFormat("FROM  radio R WHERE IdSistema='{0}' AND idSectorizacion='{1}'  AND EXISTS (SELECT 1 FROM SectoresSector S WHERE S.IdSector=R.IdSector  ", strIdSistema, strIdSectorizacion);
                    strConsulta.Append("AND R.IdSistema=R.IdSistema  AND S.IdNucleo=R.IdNucleo AND S.IdSistema= R.IdSistema ) ");
                    dtstDestinosPanelRadio = Servicio.ObtenerDataSet(strConsulta.ToString());

                    strConsulta.Clear();

                    //Se obtiene la lista de terminales de operador que está fuera de sectorización
                    strConsulta.Append("SELECT  IdSistema, IdSectorizacion, IdNucleo, IdTOP, IdSector FROM sectoressectorizacion ");
                    strConsulta.AppendFormat("WHERE IdSistema='{0}' AND idSectorizacion='{1}' AND IdSector='{2}' ", strIdSistema, strIdSectorizacion, ID_SECTOR_FS);
                    dtsTopFS = Servicio.ObtenerDataSet(strConsulta.ToString());
                }

                if (System.IO.File.Exists(sFicheroReport))
                {
                    CRSourceInf.Report.FileName = sFicheroReport;
                    CRSourceInf.ReportDocument.Load(sFicheroReport);

                    TraducirInforme(CRSourceInf.ReportDocument, strIdioma);

                    //Asociamos el datasource
                    if (null != dtst && dtst.Tables.Count > 0)
                    {
                        try
                        {
                            if (null != GetLocalResourceObject("LB_INFORME_SECTORIZACION_SUMMARY"))
                            {
                                CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = GetLocalResourceObject("LB_INFORME_SECTORIZACION_SUMMARY").ToString() + " " + strIdSectorizacion;
                            }
                            else
                                CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = "Informe de la Sectorización: " + strIdSectorizacion;

                            //Se añaden los dataset al informe principal y a los subinformes
                            CRSourceInf.ReportDocument.SetDataSource(dtst.Tables[0]);

                            //El informe principal tiene 7 subreports

                            if (CRSourceInf.ReportDocument.Subreports.Count > 0)
                            {
                                for (int i = 0; i < CRSourceInf.ReportDocument.Subreports.Count; i++)
                                {
                                    if (CRSourceInf.ReportDocument.Subreports[i] != null)
                                    {
                                        switch (CRSourceInf.ReportDocument.Subreports[i].Name.ToString())
                                        {
                                            case CR_INF_SCT_SUBINF_ABONADOS:
                                                TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                                if (dtstUsuAbonados != null && dtstUsuAbonados.Tables.Count > 0)
                                                {
                                                    dtstUsuAbonados.Tables[0].TableName = "usuariosabonados";
                                                    CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstUsuAbonados.Tables[0]);
                                                }
                                                break;

                                            case CR_INF_SCT_SUBINF_PERMISOS:
                                                TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                                if (dtstPermisosRedes != null && dtstPermisosRedes.Tables.Count > 0)
                                                {
                                                    dtstPermisosRedes.Tables[0].TableName = "permisosredes";
                                                    CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstPermisosRedes.Tables[0]);
                                                }
                                                break;
                                            case CR_INF_SCT_SUBINF_PANEL_TLF:

                                                TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                                if (dtstDestinosPanelTlf != null && dtstDestinosPanelTlf.Tables.Count > 0)
                                                {
                                                    dtstDestinosPanelTlf.Tables[0].TableName = "destinostelefoniasector";
                                                    CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstDestinosPanelTlf.Tables[0]);
                                                }
                                                break;

                                            case CR_INF_SCT_SUBINF_PANEL_LC:

                                                TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                                if (dtstDestinosPanelLC != null && dtstDestinosPanelLC.Tables.Count > 0)
                                                {
                                                    dtstDestinosPanelLC.Tables[0].TableName = "destinostelefoniasector";
                                                    CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstDestinosPanelLC.Tables[0]);
                                                }
                                                break;

                                            case CR_INF_SCT_SUBINF_PANEL_RADIO:

                                                TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                                if (dtstDestinosPanelRadio != null && dtstDestinosPanelRadio.Tables.Count > 0)
                                                {
                                                    dtstDestinosPanelRadio.Tables[0].TableName = "destinosradiosector";
                                                    CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstDestinosPanelRadio.Tables[0]);
                                                }
                                                break;

                                            case CR_INF_SCT_SUBINF_SECTORES:
                                                TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                                if (dtstSectores != null && dtstSectores.Tables.Count > 0)
                                                {
                                                    dtstSectores.Tables[0].TableName = "sectoresSector";
                                                    CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstSectores.Tables[0]);
                                                }
                                                break;
                                            case CR_INT_SCT_SUBINF_PO_FS:
                                                TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                                if (dtsTopFS != null && dtsTopFS.Tables.Count > 0)
                                                {
                                                    dtstSectores.Tables[0].TableName = "sectoresSectorizacion";
                                                    CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsTopFS.Tables[0]);

                                                    if (dtsTopFS.Tables[0].Rows.Count > 0)
                                                        bExistenTopsFS = true;
                                                }
                                                break;

                                            default:
                                                break;
                                        }
                                    }
                                }
                            }

                            //Se lee el recurso Version LB_VERSION
                            if (null != GetGlobalResourceObject("Espaniol", "LB_VERSION"))
                            {
                                strVersion = GetGlobalResourceObject("Espaniol", "LB_VERSION").ToString();
                            }

                            //Se pasan los parámetros al informe. Los parámetros se deben pasar siempre después de configurar 
                            //los data Sources. Si no se hace así, la ejecución del report falla, indicando que le faltan parámetros
                            CRSourceInf.ReportDocument.SetParameterValue("p_version", strVersion.ToString());
                            CRSourceInf.ReportDocument.SetParameterValue("p_idEmplazamiento", strNucleo.ToString());
                            CRSourceInf.ReportDocument.SetParameterValue("p_bSAFTA", bConSafta);
                            CRSourceInf.ReportDocument.SetParameterValue("p_idSectorizacion", strIdSectorizacion);
                            CRSourceInf.ReportDocument.SetParameterValue("p_bExistenTopsFS", bExistenTopsFS); // Se indica la informe si hay puestos de operador FS
                            CRSourceInf.ReportDocument.SetParameterValue("p_bSectorizacionActiva", bSectorizacionActiva); // Se indica la informe si hay puestos de operador FS
                            CRSourceInf.ReportDocument.SetParameterValue("p_fechaActivacion", objFechaActivacion); //Fecha de activación de la sectorización activa

                            //CRVInfSectorizacion.DataBind();
                            //CRVInfSectorizacion.Visible = true;
                            VisualizaInformePdf();
                        }
                        catch (System.Threading.ThreadAbortException )
                        { //ThreadException can happen for internale Response implementation
                            //La visualización del informe en pdf lanza la excepción interna
                            //Se liberan los dataset
                            if (dtst != null)
                                dtst.Clear();

                            if (dtstPermisosRedes != null)
                                dtstPermisosRedes.Clear();

                            if (dtstUsuAbonados != null)
                                dtstUsuAbonados.Clear();

                            if (dtstDestinosPanelTlf != null)
                                dtstDestinosPanelTlf.Clear();

                            if (dtstDestinosPanelLC != null)
                                dtstDestinosPanelLC.Clear();

                            if (dtstDestinosPanelRadio != null)
                                dtstDestinosPanelRadio.Clear();
                        }
                        catch (Exception ex)
                        {
                            logDebugView.Error(string.Format("Error al ejecutar el informe {0}. Error:{1} ", sFicheroReport, ex.Message.ToString()));
                        }


                    }
                    else
                    {
                        logDebugView.Error(string.Format("Error al ejecutar el informe {0}: el fichero no existe ", sFicheroReport));
                        DeshabilitaBotonesExportar();
                    }
                }
            }
        }
    }

    private void DeshabilitaBotonesExportar()
    {
        //Se deshabilitan los botones de exportar y las acciones correspondientes
        BtnPdf.Enabled = false;
        BtnPdf.OnClientClick = null;
        BtnExcel.Enabled = false;
        BtnExcel.OnClientClick = null;
    }

    private void TraducirInforme(ReportDocument document, string lang)
    {
        // Se reemplaza el texto en el informe
        string strTexto = string.Empty;

        foreach (CrystalDecisions.CrystalReports.Engine.ReportObject obj in document.ReportDefinition.ReportObjects)
        {
            var txt = obj as CrystalDecisions.CrystalReports.Engine.TextObject;
            if (txt != null && txt.Name.ToUpper().EndsWith("_"))
            {
                strTexto = TraducirTexto(txt.Name, lang);

                if (!string.IsNullOrEmpty(strTexto))
                    txt.Text = strTexto;
            }
        }
        // Se reemplaza el texto de las fórmulas que continen un texto y que hay que traducir
        foreach (FormulaFieldDefinition objFormula in document.DataDefinition.FormulaFields)
        {
            if (objFormula.Name.ToUpper().EndsWith("_"))
            {
                strTexto = TraducirTexto(objFormula.Name, lang);
                if (!string.IsNullOrEmpty(strTexto))
                    objFormula.Text = "\"" + strTexto + "\"";

                objFormula.Text = objFormula.Text;
            }
        }
    }

    private string TraducirTexto(string key, string lang)
    {
        string text=string.Empty;
        // Se elimina el underscore del final
        key = key.ToUpper().Trim('_');

        try
        {
            if (null!=GetLocalResourceObject(key))
                text = GetLocalResourceObject(key).ToString();
            else
                logDebugView.Debug(string.Format("Report.TraducirTexto: informe {0} -> No se ha encontrado la etiqueta {1} en el fichero de recursos", sFicheroReport, key));

        }
        catch (Exception objEx)
        {
            logDebugView.Error(string.Format("Report.TraducirTexto: informe {0} -> Exception al traducir la etiqueta {1}. Error:{2}", sFicheroReport, key,objEx.ToString()));

        }
        return text;
    } 


  protected void BtnPdf_Click(object sender, ImageClickEventArgs e)
  {
      try
      {
          if (null != CRSourceInf.ReportDocument && !string.IsNullOrEmpty(CRSourceInf.ReportDocument.FileName))
          {
              string strNombreInf = string.Empty;
              string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

              if ((GetLocalResourceObject("REP_NOMBRE_FICHERO") != null) && (!string.IsNullOrEmpty(GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString())))
                  strNombreInf = string.Format("{0}_{1}_{2}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(), strIdSectorizacion, DateTime.Now.ToString("yyyyMMddHHmm"));
              else
                 strNombreInf = string.Format("InfSectorizacion_{0}_{1}", strIdSectorizacion, DateTime.Now.ToString("yyyyMMddHHmm"));


              CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, strNombreInf);
          }
      }
      catch (Exception objEx)
      {
          logDebugView.Error(string.Format("InformeSectorizacion.BtnPdf_Click: informe {0} -> Error al generar informe en PDF. Error:{1}", sFicheroReport, objEx.ToString()));
      }
  }  

  protected void BtnExcel_Click(object sender, ImageClickEventArgs e)
  {
      try
      {
          if (null != CRSourceInf.ReportDocument && !string.IsNullOrEmpty(CRSourceInf.ReportDocument.FileName))
          {
              string strNombreInf = string.Empty;
              string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

              if ((GetLocalResourceObject("REP_NOMBRE_FICHERO") != null) && (!string.IsNullOrEmpty(GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString())))
                  strNombreInf = string.Format("{0}_{1}_{2}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(), strIdSectorizacion, DateTime.Now.ToString("yyyyMMddHHmm"));
              else
                  strNombreInf = string.Format("InfSectorizacion_{0}_{1}", strIdSectorizacion, DateTime.Now.ToString("yyyyMMddHHmm"));

              CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, strNombreInf);
          }
      }
      catch (Exception objEx)
      {
          logDebugView.Error(string.Format("InformeSectorizacion.BtnExcel_Click: informe {0} -> Error al generar informe en excel. Error:{1}", sFicheroReport, objEx.ToString()));
      }
  }

  protected void VisualizaInformePdf()
  {
    //visualiza el informe en formato Pdf
    if (null != CRSourceInf.ReportDocument && !string.IsNullOrEmpty(CRSourceInf.ReportDocument.FileName))
    {
        string strNombreInf = string.Empty;
        string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        if ((GetLocalResourceObject("REP_NOMBRE_FICHERO") != null) && (!string.IsNullOrEmpty(GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString())))
            strNombreInf = string.Format("{0}_{1}_{2}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(),strIdSectorizacion, DateTime.Now.ToString("yyyyMMddHHmm"));
        else
            strNombreInf = string.Format("InfSectorizacion_{0}_{1}",strIdSectorizacion, DateTime.Now.ToString("yyyyMMddHHmm"));

        Response.Buffer = false;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/pdf";
        try
        {
            CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, false, strNombreInf);
        }
        catch (System.Threading.ThreadAbortException )
        { //ThreadException can happen for internale Response implementation

        }
        catch (Exception)
        {
        }
    }

  }

}
