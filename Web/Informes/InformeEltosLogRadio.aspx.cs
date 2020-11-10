using System;
using System.Collections.Generic;
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

public partial class InformeEltosLogRadio : System.Web.UI.Page
{
    private static ILog _logDebugView;

    const string CONF_KEY_SISTEMA = "Sistema";

    //Nombres de los Ficheros de crystal report que contienen los informes
    const string CR_INF_LOGICAL_ELEMENTS = "InfCnfElementosLgRadio.rpt";        //Informe de configuración de elementos lógicos Radio

    //Subinformes contenidos en el informe CR_INF_HW_ELEMENTS
    const string CR_INF_LE_SUBINF_FREC_RADIO = "SubInformeELFrecRadio.rpt";     // Subinforme de frecuencias radio
    const string CR_INF_LE_SUBINF_RECRADIO_NOASIGNADOS = "SubInformeELRecRadioNoAsignados.rpt"; // Subinforme de Recursos Radio no asignados
    const string CR_INF_LE_SUBINF_RECRADIO_RESERVA = "SubInformeELRecRadioReserva.rpt";         // Subinforme de Recursos Radio en Reserva

    static string sFicheroReport;

    static DataSet dtst;
    static DataSet dtsRecRadio;
    static DataSet dtsRecRadioNoAsignados;
    static DataSet dtsRecRadioReserva;
    static string strVersion;
    static string strNucleo;
    static string strIdSistema;

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
            strIdSistema = string.Empty;

            strVersion=strNucleo=string.Empty;
            dtst = null;
            dtsRecRadio = null;
            dtsRecRadioNoAsignados = null;
            dtsRecRadioReserva = null;

            /*
            CRViewerInf.DisplayToolbar = true;
            CRViewerInf.EnableParameterPrompt = false;
            CRViewerInf.EnableDatabaseLogonPrompt = false;
            CRViewerInf.Visible = false;
            CRViewerInf.HasToggleGroupTreeButton = false;
            CRViewerInf.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CRViewerInf.HasToggleParameterPanelButton = false;
            CRViewerInf.HasDrilldownTabs = false; //Se oculta la pestaña pagina principal
            CRViewerInf.HasExportButton = false; //Se ocultan las opciones de exportacion de la Toolbar
            CRViewerInf.HasCrystalLogo = false; //Se oculta el logo de Crystal Reports
            CRViewerInf.HasPrintButton = true; //Se oculta la opción de imprimir
            */
            string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;


            ServiciosCD40.ServiciosCD40 Servicio = new ServiciosCD40.ServiciosCD40();

            //Se obtiene el valor del parámetro Sistema, configurado en el fichero Web.config
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement objConf = config.AppSettings.Settings[CONF_KEY_SISTEMA];
            strIdSistema = objConf.Value;

            //Obtenemos el Núcleo, que se pasa como parámetro en todos los informes 
            ServiciosCD40.Nucleos tabNucleo = new ServiciosCD40.Nucleos();

            tabNucleo.IdSistema = objConf.Value;
            ServiciosCD40.Tablas[] dTabNucleos = Servicio.ListSelectSQL(tabNucleo);

            if (null != dTabNucleos && dTabNucleos.Length > 0)
            {
                //Nos quedamos con el primer registro, porque en el sistema sólo se puede definir un nucleo
                strNucleo=((ServiciosCD40.Nucleos)dTabNucleos[0]).IdNucleo;
                dTabNucleos = null; //Liberamos la memoria
            }

            //Informe de configuración de elementos lógicos
            sFicheroReport = Server.MapPath("~/Informes/" + CR_INF_LOGICAL_ELEMENTS);


            //Se obtienen los datos a visualizar en cada subinforme

            //Subinforme Frecuencias Radio
            //Consulta de obtención de destinos radio
            strConsulta.Append("SELECT DR.IdSistema, DR.TipoDestino,DR.IdDestino,DR.CnfModoDestino,DR.CnfTipoFrecuencia,DR.PrioridadSesionSip,DR.MetodoCalculoClimax,");
            strConsulta.Append("DR.CldSupervisionTime,DR.VentanaSeleccionBss,DR.ModoTransmision,MB.name as NameBss, DR.EmplazamientoDefecto, DR.TiempoVueltaADefecto, DR.ConRedundancia ");
            strConsulta.Append("FROM DestinosRadio DR LEFT JOIN metodos_bss mb ON mb.idmetodos_bss = DR.MetodosBssOfrecidos ");
            strConsulta.AppendFormat("WHERE DR.IdSistema='{0}' ORDER BY DR.CnfTipoFrecuencia,DR.IdDestino",strIdSistema);

            dtst = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            //Consulta de obtención de recursos radio asociados a los destinos radio junto a los parámetros HF y M+N si aplica
            strConsulta.Append("SELECT r.IdRecurso, r.TipoRecurso, r.IdEmplazamiento, rp.idradio_param, rp.GrsDelay, rp.OffSetFrequency, rp.EnableEventPttSq, rp.metodos_bss_idmetodos_bss,");  
            strConsulta.Append("pr.GananciaAGCTX, pr.GananciaAGCTXdBm, pr.GananciaAGCRX, pr.GananciaAGCRXdBm, pr.TamRTP, pr.TipoEM, pr.GrabacionEd137, z.Nombre AS NameZona,");  
            strConsulta.Append("rec.Tipo, tb.name AS TablaBSS, mb.name AS NameBss, rec.IdTIFX AS rec_idTIFX, rec.idEquipos, rec.SlotPasarela, rec.NumDispositivoSlot, pr.Codec,");
            strConsulta.Append("pr.SupresionSilencio, r.IdDestino, r.TipoDestino, r.IdSistema, r.zonas_idZonas, r.tabla_bss_idtabla_bss, r.SQ, r.PTT, r.TiempoPTT, r.UmbralVAD, r.BSS, "); 
            strConsulta.Append("hfp.IpGestor as hfp_IpGestor, hfp.Oid as hfp_oid, hfp.Frecuencia as hfp_frecuencia, hfp.TipoEquipo as hfp_TipoEquipo, hfp.TipoCanal as hfp_TipoCanal,");
            strConsulta.Append("hfp.TipoFrecuencia as hfp_tipoFrecuencia, hfp.TipoModo as hfp_tipomodo,hfp.PrioridadEquipo as hfp_prioridadEquipo, hfp.Puerto as hfp_puerto, ");
            strConsulta.Append("hfp.Offset as hfp_offset, hfp.Canalizacion as hfp_canalizacion, hfp.Modulacion as hfp_modulacion, hfp.Potencia as hfp_potencia, ");
            strConsulta.Append("hfp.FormatoFrecuenciaPrincipal as hfp_FormatoFrecuenciaPrincipal, hfp.ModeloEquipo as hfp_ModeloEquipo, r.RedundanciaRol, r.RedundanciaIdPareja ");
            strConsulta.Append("FROM  recursosradio r LEFT OUTER JOIN zonas z ON z.idZonas = r.zonas_idZonas ");
            strConsulta.Append("LEFT OUTER JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss ");
            strConsulta.Append("LEFT OUTER JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param ");
            strConsulta.Append("LEFT OUTER JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss ");
            strConsulta.Append("LEFT OUTER JOIN destinosradio destrad ON destrad.IdSistema = r.IdSistema AND destrad.IdDestino = r.IdDestino AND destrad.TipoDestino = r.TipoDestino ");
            strConsulta.Append("INNER JOIN parametrosrecurso pr ON pr.IdSistema = r.IdSistema AND pr.IdRecurso = r.IdRecurso AND pr.TipoRecurso = r.TipoRecurso ");
            strConsulta.Append("INNER JOIN recursos rec ON rec.IdSistema = r.IdSistema AND rec.IdRecurso = r.IdRecurso AND rec.TipoRecurso = r.TipoRecurso ");
            strConsulta.Append("LEFT OUTER JOIN hfparams hfp ON hfp.IdSistema = r.IdSistema and hfp.IdRecurso= r.IdRecurso ");
            strConsulta.AppendFormat("WHERE r.IdSistema='{0}' AND r.IdDestino is not null ", strIdSistema);
            strConsulta.Append("ORDER BY r.IdDestino, r.RedundanciaIdPareja,r.RedundanciaRol, r.IdRecurso ");

            dtsRecRadio = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            //Consulta de obtención de recursos radio sin destino asociados que son M+N de tipo N (tipomodo=1) o HF
            strConsulta.Append("SELECT r.IdRecurso, r.TipoRecurso, r.IdEmplazamiento, rp.idradio_param, rp.GrsDelay, rp.OffSetFrequency, rp.EnableEventPttSq, rp.metodos_bss_idmetodos_bss,");
            strConsulta.Append("pr.GananciaAGCTX, pr.GananciaAGCTXdBm, pr.GananciaAGCRX, pr.GananciaAGCRXdBm, pr.TamRTP, pr.TipoEM, pr.GrabacionEd137, z.Nombre AS NameZona,");
            strConsulta.Append("rec.Tipo, tb.name AS TablaBSS, mb.name AS NameBss, rec.IdTIFX AS rec_idTIFX, rec.idEquipos, rec.SlotPasarela, rec.NumDispositivoSlot, pr.Codec,");
            strConsulta.Append("pr.SupresionSilencio, r.IdDestino, r.TipoDestino, r.IdSistema, r.zonas_idZonas, r.tabla_bss_idtabla_bss, r.SQ, r.PTT, r.TiempoPTT, r.UmbralVAD, r.BSS, ");
            strConsulta.Append("hfp.IpGestor as hfp_IpGestor, hfp.Oid as hfp_oid, hfp.Frecuencia as hfp_frecuencia, hfp.TipoEquipo as hfp_TipoEquipo, hfp.TipoCanal as hfp_TipoCanal,");
            strConsulta.Append("hfp.TipoFrecuencia as hfp_tipoFrecuencia, hfp.TipoModo as hfp_tipomodo,hfp.PrioridadEquipo as hfp_prioridadEquipo, hfp.Puerto as hfp_puerto, ");
            strConsulta.Append("hfp.Offset as hfp_offset, hfp.Canalizacion as hfp_canalizacion, hfp.Modulacion as hfp_modulacion, hfp.Potencia as hfp_potencia, ");
            strConsulta.Append("hfp.FormatoFrecuenciaPrincipal as hfp_FormatoFrecuenciaPrincipal, hfp.ModeloEquipo as hfp_ModeloEquipo ");
            strConsulta.Append("FROM  recursosradio r LEFT OUTER JOIN zonas z ON z.idZonas = r.zonas_idZonas ");
            strConsulta.Append("LEFT OUTER JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss ");
            strConsulta.Append("LEFT OUTER JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param ");
            strConsulta.Append("LEFT OUTER JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss ");
            strConsulta.Append("LEFT OUTER JOIN destinosradio destrad ON destrad.IdSistema = r.IdSistema AND destrad.IdDestino = r.IdDestino AND destrad.TipoDestino = r.TipoDestino ");
            strConsulta.Append("INNER JOIN parametrosrecurso pr ON pr.IdSistema = r.IdSistema AND pr.IdRecurso = r.IdRecurso AND pr.TipoRecurso = r.TipoRecurso ");
            strConsulta.Append("INNER JOIN recursos rec ON rec.IdSistema = r.IdSistema AND rec.IdRecurso = r.IdRecurso AND rec.TipoRecurso = r.TipoRecurso ");
            strConsulta.Append("LEFT OUTER JOIN hfparams hfp ON hfp.IdSistema = r.IdSistema and hfp.IdRecurso= r.IdRecurso ");
            strConsulta.AppendFormat("WHERE r.IdSistema='{0}' AND r.IdDestino is null  AND (rec.tipo=3 OR (rec.tipo>=4 AND rec.tipo<=6 AND hfp.tipomodo=1)) ", strIdSistema);
            strConsulta.Append("ORDER BY r.IdDestino, r.IdRecurso ");

            dtsRecRadioReserva = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            //Consulta de obtención de recursos radio sin destino asociados que no son M+N de tipo N, ni HF
            strConsulta.Append("SELECT r.IdRecurso, r.TipoRecurso, r.IdEmplazamiento, rp.idradio_param, rp.GrsDelay, rp.OffSetFrequency, rp.EnableEventPttSq, rp.metodos_bss_idmetodos_bss,");
            strConsulta.Append("pr.GananciaAGCTX, pr.GananciaAGCTXdBm, pr.GananciaAGCRX, pr.GananciaAGCRXdBm, pr.TamRTP, pr.TipoEM, pr.GrabacionEd137, z.Nombre AS NameZona,");
            strConsulta.Append("rec.Tipo, tb.name AS TablaBSS, mb.name AS NameBss, rec.IdTIFX AS rec_idTIFX, rec.idEquipos, rec.SlotPasarela, rec.NumDispositivoSlot, pr.Codec,");
            strConsulta.Append("pr.SupresionSilencio, r.IdDestino, r.TipoDestino, r.IdSistema, r.zonas_idZonas, r.tabla_bss_idtabla_bss, r.SQ, r.PTT, r.TiempoPTT, r.UmbralVAD, r.BSS, ");
            strConsulta.Append("hfp.IpGestor as hfp_IpGestor, hfp.Oid as hfp_oid, hfp.Frecuencia as hfp_frecuencia, hfp.TipoEquipo as hfp_TipoEquipo, hfp.TipoCanal as hfp_TipoCanal,");
            strConsulta.Append("hfp.TipoFrecuencia as hfp_tipoFrecuencia, hfp.TipoModo as hfp_tipomodo,hfp.PrioridadEquipo as hfp_prioridadEquipo, hfp.Puerto as hfp_puerto, ");
            strConsulta.Append("hfp.Offset as hfp_offset, hfp.Canalizacion as hfp_canalizacion, hfp.Modulacion as hfp_modulacion, hfp.Potencia as hfp_potencia, ");
            strConsulta.Append("hfp.FormatoFrecuenciaPrincipal as hfp_FormatoFrecuenciaPrincipal, hfp.ModeloEquipo as hfp_ModeloEquipo ");
            strConsulta.Append("FROM  recursosradio r LEFT OUTER JOIN zonas z ON z.idZonas = r.zonas_idZonas ");
            strConsulta.Append("LEFT OUTER JOIN tabla_bss tb ON tb.idtabla_bss = r.tabla_bss_idtabla_bss ");
            strConsulta.Append("LEFT OUTER JOIN radio_param rp ON rp.idradio_param = r.radio_param_idradio_param ");
            strConsulta.Append("LEFT OUTER JOIN metodos_bss mb ON mb.idmetodos_bss = rp.metodos_bss_idmetodos_bss ");
            strConsulta.Append("LEFT OUTER JOIN destinosradio destrad ON destrad.IdSistema = r.IdSistema AND destrad.IdDestino = r.IdDestino AND destrad.TipoDestino = r.TipoDestino ");
            strConsulta.Append("INNER JOIN parametrosrecurso pr ON pr.IdSistema = r.IdSistema AND pr.IdRecurso = r.IdRecurso AND pr.TipoRecurso = r.TipoRecurso ");
            strConsulta.Append("INNER JOIN recursos rec ON rec.IdSistema = r.IdSistema AND rec.IdRecurso = r.IdRecurso AND rec.TipoRecurso = r.TipoRecurso ");
            strConsulta.Append("LEFT OUTER JOIN hfparams hfp ON hfp.IdSistema = r.IdSistema and hfp.IdRecurso= r.IdRecurso ");
            strConsulta.AppendFormat("WHERE r.IdSistema='{0}' AND r.IdDestino is null  AND (rec.tipo!=3 AND (rec.tipo<4 OR  rec.tipo>6 AND hfp.tipomodo=0)) ", strIdSistema);

            dtsRecRadioNoAsignados = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

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
                        if (null != GetLocalResourceObject("LB_INFORME_ELEMENTOS_LOG_RADIO_SUMMARY"))
                        {
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = GetLocalResourceObject("LB_INFORME_ELEMENTOS_LOG_RADIO_SUMMARY").ToString();
                        }
                        else
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = "Informe de Configuración de Radio";

                        //Le asignamos los datos
                        if (CRSourceInf.ReportDocument.Subreports.Count > 0)
                        {
                            for (int i = 0; i < CRSourceInf.ReportDocument.Subreports.Count; i++)
                            {
                                if (CRSourceInf.ReportDocument.Subreports[i] != null)
                                {

                                    switch (CRSourceInf.ReportDocument.Subreports[i].Name.ToString())
                                    {
                                        case CR_INF_LE_SUBINF_FREC_RADIO:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtst != null && dtst.Tables.Count > 0)
                                            {
                                                dtst.Tables[0].TableName = "DestinosRadio";

                                                if (dtsRecRadio != null && dtsRecRadio.Tables.Count > 0)
                                                {
                                                    dtsRecRadio.Tables[0].TableName="RecursosRadio";
                                                    dtst.Tables.Add(dtsRecRadio.Tables[0].Copy());
                                                }
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtst);
                                            }
                                            break;
                                        case CR_INF_LE_SUBINF_RECRADIO_RESERVA:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsRecRadioReserva != null && dtsRecRadioReserva.Tables.Count > 0)
                                            {
                                                dtsRecRadioReserva.Tables[0].TableName = "RecursosRadio";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsRecRadioReserva.Tables[0]);
                                            }
                                            break;

                                        case CR_INF_LE_SUBINF_RECRADIO_NOASIGNADOS:

                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsRecRadioNoAsignados != null && dtsRecRadioNoAsignados.Tables.Count > 0)
                                            {
                                                dtsRecRadioNoAsignados.Tables[0].TableName = "RecursosRadio";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsRecRadioNoAsignados.Tables[0]);
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
                            strVersion=GetGlobalResourceObject("Espaniol", "LB_VERSION").ToString();
                        }

                        //Se pasan los parámetros al informe. Los parámetros se deben pasar siempre después de configurar 
                        //los data Sources. Si no se hace así, la ejecución del report falla, indicando que le faltan parámetros
                        CRSourceInf.ReportDocument.SetParameterValue("p_version", strVersion.ToString());
                        CRSourceInf.ReportDocument.SetParameterValue("p_idEmplazamiento", strNucleo.ToString());

                        //CRViewerInf.DataBind();
                        //CRViewerInf.Visible = true;

                        VisualizaInformePdf();

                    }
                    catch (System.Threading.ThreadAbortException )
                    { //ThreadException can happen for internale Response implementation
                        if (dtst != null)
                            dtst.Clear();
                        if (dtsRecRadio != null)
                            dtsRecRadio.Clear();
                        if (dtsRecRadioNoAsignados != null)
                            dtsRecRadioNoAsignados.Clear();
                        if (dtsRecRadioReserva != null)
                            dtsRecRadioReserva.Clear();
                    }
                    catch (Exception ex)
                    {
                        logDebugView.Error(string.Format("Error al ejecutar el informe {0}. Error:{1} ", sFicheroReport, ex.Message.ToString()));
                    }
                }
            }
            else
            {
                logDebugView.Error(string.Format("Error al ejecutar el informe {0}: el fichero no existe ", sFicheroReport));
                DeshabilitaBotonesExportar();
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
        // remove ending underscore
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

        if (null != CRSourceInf.ReportDocument && !string.IsNullOrEmpty(CRSourceInf.ReportDocument.FileName))
        {
            string strNombreInf = string.Empty;
            string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            if ((GetLocalResourceObject("REP_NOMBRE_FICHERO") != null) && (!string.IsNullOrEmpty(GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString())))
                strNombreInf = string.Format("{0}_{1}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(), DateTime.Now.ToString("yyyyMMddHHmm"));
            else
                strNombreInf = string.Format("InfCnfElementosRadio_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

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

  protected void BtnExcel_Click(object sender, ImageClickEventArgs e)
  {
      try
      {
          if (null != CRSourceInf.ReportDocument && !string.IsNullOrEmpty(CRSourceInf.ReportDocument.FileName))
          {
              string strNombreInf = string.Empty;
              string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

              if ((GetLocalResourceObject("REP_NOMBRE_FICHERO") != null) && (!string.IsNullOrEmpty(GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString())))
                  strNombreInf = string.Format("{0}_{1}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(), DateTime.Now.ToString("yyyyMMddHHmm"));
              else
                  strNombreInf = string.Format("InfCnfElementosRadio_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

              CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, strNombreInf);
          }

      }
      catch (Exception objEx)
      {
          logDebugView.Error(string.Format("BtnExcel_Click: informe {0} -> Error al generar informe en PDF. Error:{1}", sFicheroReport, objEx.ToString()));
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
            strNombreInf = string.Format("{0}_{1}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(), DateTime.Now.ToString("yyyyMMddHHmm"));
        else
            strNombreInf = string.Format("InfCnfElementosRadio_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

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
        catch (Exception objEx)
        {
            logDebugView.Error(string.Format("VisualizaInformePdf: informe {0} -> Error al generar informe en PDF. Error:{1}", sFicheroReport, objEx.ToString()));
        }
    }

    }

}
