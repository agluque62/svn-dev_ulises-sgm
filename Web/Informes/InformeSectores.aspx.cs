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

public partial class InformeSectores : System.Web.UI.Page
{
    private static ILog _logDebugView;

    static string sFicheroReport;

    const string CONF_KEY_CON_SACTA = "SistemaConSACTA";
    const string CONF_KEY_SISTEMA = "Sistema";

    //Nombres de los Ficheros de crystal report que contienen los informes
    const string CR_INF_SECTORES = "InfSectores.rpt";                                 // Informe de CR de Sectores

    //Subinformes contenidos en el informe CR_INF_SECTORES
    const string CR_INF_SECTORES_SUBINF_PERMISOS    = "SubInformePermisosRedes.rpt";  //Subinforme permisos redes del sector
    const string CR_INF_SECTORES_SUBINF_ABONADOS    = "SubInformeAbonadosSector.rpt"; //Subinforme números de abonados del sector
    const string CR_INF_SECTORES_SUBINF_PANEL_TLF   = "SubInfPanelTlf.rpt";           //Subinforme panel de telefonía
    const string CR_INF_SECTORES_SUBINF_PANEL_LC    = "SubInfPanelLC.rpt";            //Subinforme panel de Linea Caliente
    const string CR_INF_SECTORES_SUBINF_PANEL_RADIO = "SubInfPanelRadio.rpt";         //Subinforme panel de Linea Caliente

    static DataSet dtst;
    static DataSet dtstPermisosRedes;
    static DataSet dtstUsuAbonados;
    static DataSet dtstDestinosPanelTlf;
    static DataSet dtstDestinosPanelLC;
    static DataSet dtstDestinosPanelRadio;
    static string strVersion;
    static string strNucleo;
    static string strIdSistema;
    static bool   bConSafta;

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
            CRViewerInf.HasPrintButton = true; //Se muestra la opción de imprimir
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
                strNucleo = ((ServiciosCD40.Nucleos)dTabNucleos[0]).IdNucleo;
                dTabNucleos = null; //Liberamos la memoria
            }

            sFicheroReport = Server.MapPath(CR_INF_SECTORES);

            //Se obtienen los datos a visualizar en el informe

            //Lista de Sectores simples y sus parámetros
            strConsulta.Append("SELECT   sectores.IdSistema,sectores.IdNucleo,sectores.IdSector,sectores.Tipo,sectores.TipoPosicion,sectores.PrioridadR2,");
            strConsulta.Append("sectores.TipoHMI,sectores.NumSacta,sectores.SectorSimple,");
            strConsulta.Append("parametrossector.NumLlamadasEntrantesIDA,parametrossector.NumLlamadasEnIDA,parametrossector.NumFreqPagina,parametrossector.NumPagFreq,");
            strConsulta.Append("parametrossector.NumDestinosInternosPag,parametrossector.NumPagDestinosInt,parametrossector.Intrusion,parametrossector.Intruido, ");
            strConsulta.Append("parametrossector.KeepAlivePeriod,parametrossector.KeepAliveMultiplier,parametrossector.NumEnlacesAI,parametrossector.GrabacionEd137, ");
            strConsulta.Append("teclassector.TransConConsultaPrev, teclassector.TransDirecta, teclassector.Conferencia, teclassector.Escucha, teclassector.Retener,");
            strConsulta.Append("teclassector.Captura,teclassector.Redireccion,teclassector.RepeticionUltLlamada,teclassector.RellamadaAut,teclassector.Tecla55mas1,");
            strConsulta.Append("teclassector.TeclaPrioridad,teclassector.Monitoring,teclassector.CoordinadorTF,teclassector.CoordinadorRD,teclassector.IntegracionRDTF,");
            strConsulta.Append("teclassector.LlamadaSelectiva,teclassector.GrupoBSS,teclassector.LTT, teclassector.SayAgain,teclassector.InhabilitacionRedirec,teclassector.Glp ");
            strConsulta.Append("FROM  sectores LEFT OUTER JOIN ");
            strConsulta.Append("parametrossector ON sectores.IdSistema=parametrossector.IdSistema AND sectores.IdNucleo=parametrossector.IdNucleo AND ");
            strConsulta.Append("sectores.IdSector=parametrossector.IdSector ");
            strConsulta.AppendFormat("LEFT OUTER JOIN teclassector  ON sectores.IdSistema=teclassector.IdSistema AND sectores.IdNucleo=teclassector.IdNucleo AND sectores.IdSector=teclassector.IdSector ");
            strConsulta.AppendFormat(" WHERE sectores.IdSistema='{0}' AND sectores.SectorSimple=1 ", strIdSistema);

            dtst = Servicio.ObtenerDataSet(strConsulta.ToString());

            if (dtst != null && dtst.Tables.Count > 0)
            {
                dtst.Tables[0].TableName = "sectores";

                strConsulta.Clear();

                //Lista de Permisos redes de los sectores
                ServiciosCD40.PermisosRedes tabPermisosRedes = new ServiciosCD40.PermisosRedes();
                tabPermisosRedes.IdSistema = strIdSistema;

                strConsulta.Append("SELECT IdSistema, IdRed, IdSector, IdNucleo, Llamar, Recibir FROM permisosredes ");
                strConsulta.AppendFormat("WHERE IdSistema='{0}' AND ", strIdSistema);
                strConsulta.Append("EXISTS(SELECT 1 FROM SECTORES WHERE SECTORES.IdSector=permisosredes.IdSector ");
                strConsulta.Append("AND SECTORES.IdSistema=permisosredes.IdSistema  AND SECTORES.IdNucleo=permisosredes.IdNucleo AND SECTORES.sectorsimple=1) ");

                dtstPermisosRedes = Servicio.ObtenerDataSet(strConsulta.ToString());

                strConsulta.Clear();

                //Lista de Abonados a los que atiende cada sector
                strConsulta.Append("SELECT usuariosabonados.IdSistema, usuariosabonados.IdPrefijo, usuariosabonados.IdNucleo, usuariosabonados.IdSector,");
                strConsulta.Append("usuariosabonados.IdAbonado, redes.IdRed ");
                strConsulta.Append("FROM usuariosabonados inner JOIN ");
                strConsulta.Append("redes ON usuariosabonados.IdSistema = redes.IdSistema AND usuariosabonados.IdPrefijo = redes.IdPrefijo ");
                strConsulta.AppendFormat("WHERE usuariosabonados.IdSistema='{0}' AND EXISTS(SELECT 1 FROM SECTORES WHERE SECTORES.IdSector=usuariosabonados.IdSector ", strIdSistema);
                strConsulta.Append("AND SECTORES.IdSistema=usuariosabonados.IdSistema  AND SECTORES.IdNucleo=usuariosabonados.IdNucleo AND SECTORES.sectorsimple=1) ");

                dtstUsuAbonados = Servicio.ObtenerDataSet(strConsulta.ToString());

                strConsulta.Clear();

                //Se obtienen los destinos del panel de telefonía de todos los sectores simples
                strConsulta.Append("SELECT IdSistema,IdNucleo,IdSector,IdDestino,TipoDestino,IdPrefijo,PosHMI,Prioridad,OrigenR2,PrioridadSIP,TipoAcceso,Literal,");
                strConsulta.AppendFormat("NULL AS IdPrefijoDestinoLCEN, NULL AS IdDestinoLCEN FROM destinosinternossector WHERE IdSistema='{0}' AND TipoAcceso='DA' AND IdPrefijo=2 ", strIdSistema);
                strConsulta.Append("UNION SELECT IdSistema,IdNucleo,IdSector,IdDestino,TipoDestino,IdPrefijo,PosHMI,Prioridad,OrigenR2,PrioridadSIP,TipoAcceso,Literal,");
                strConsulta.AppendFormat("IdPrefijoDestinoLCEN,IdDestinoLCEN FROM destinosexternossector WHERE IdSistema='{0}' AND TipoAcceso='DA'  AND IdPrefijo<>1 ", strIdSistema);
                dtstDestinosPanelTlf = Servicio.ObtenerDataSet(strConsulta.ToString());
                strConsulta.Clear();

                //Se obtienen los destinos del panel de Línea Caliente
                strConsulta.Append("SELECT IdSistema,IdNucleo,IdSector,IdDestino,TipoDestino,IdPrefijo,PosHMI,Prioridad,OrigenR2,PrioridadSIP,TipoAcceso,Literal,");
                strConsulta.AppendFormat("NULL AS IdPrefijoDestinoLCEN, NULL AS IdDestinoLCEN FROM destinosinternossector WHERE IdSistema='{0}'AND TipoAcceso='IA' AND IdPrefijo=0 ", strIdSistema);
                strConsulta.Append("UNION  SELECT IdSistema,IdNucleo,IdSector,IdDestino,TipoDestino,IdPrefijo,PosHMI,Prioridad,OrigenR2,PrioridadSIP,TipoAcceso,Literal,");
                strConsulta.AppendFormat("IdPrefijoDestinoLCEN,IdDestinoLCEN FROM destinosexternossector WHERE IdSistema='{0}' AND TipoAcceso='IA'  AND IdPrefijo=3 ", strIdSistema);
                dtstDestinosPanelLC = Servicio.ObtenerDataSet(strConsulta.ToString());
                strConsulta.Clear();

                //Se obtienen los destinos del panel de radio
                strConsulta.Append("SELECT IdSistema,IdNucleo,IdSector,IdDestino,TipoDestino,PosHMI,Prioridad,PrioridadSIP,ModoOperacion,Cascos,Literal,SupervisionPortadora ");
                strConsulta.AppendFormat("FROM  destinosradiosector WHERE IdSistema='{0}' ", strIdSistema);
                dtstDestinosPanelRadio = Servicio.ObtenerDataSet(strConsulta.ToString());
                strConsulta.Clear();
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
                        bConSafta = false;

                        //Se obtiene el parametro que indica si se debe o no visualizar la posición Sacta
                        objConf = config.AppSettings.Settings[CONF_KEY_CON_SACTA];

                        if ((objConf != null) && (!string.IsNullOrEmpty(objConf.Value) && string.Compare(objConf.Value, "SI", true) == 0))
                        {
                            //El sistema está configurado con el sistema SACTA
                            bConSafta = true;
                        }

                        if (null != GetLocalResourceObject("LB_INFORME_SECTORES_SUMMARY"))
                        {
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = GetLocalResourceObject("LB_INFORME_SECTORES_SUMMARY").ToString();
                        }
                        else
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = "Informe de Sectores";

                        //Se añaden los dataset al informe principal y a los subinformes
                        CRSourceInf.ReportDocument.SetDataSource(dtst.Tables[0]);

                        //El informe principal tiene 5 subreports
                        if (CRSourceInf.ReportDocument.Subreports.Count > 0)
                        {
                            for (int i = 0; i < CRSourceInf.ReportDocument.Subreports.Count; i++)
                            {
                                if (CRSourceInf.ReportDocument.Subreports[i] != null)
                                {
                                    switch (CRSourceInf.ReportDocument.Subreports[i].Name.ToString())
                                    {
                                        case CR_INF_SECTORES_SUBINF_ABONADOS:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtstUsuAbonados != null && dtstUsuAbonados.Tables.Count > 0)
                                            {
                                                dtstUsuAbonados.Tables[0].TableName = "usuariosabonados";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstUsuAbonados.Tables[0]);
                                            }
                                            break;

                                        case CR_INF_SECTORES_SUBINF_PERMISOS:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtstPermisosRedes != null && dtstPermisosRedes.Tables.Count > 0)
                                            {
                                                dtstPermisosRedes.Tables[0].TableName = "permisosredes";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstPermisosRedes.Tables[0]);
                                            }
                                            break;
                                        case CR_INF_SECTORES_SUBINF_PANEL_TLF:

                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtstDestinosPanelTlf != null && dtstDestinosPanelTlf.Tables.Count > 0)
                                            {
                                                dtstDestinosPanelTlf.Tables[0].TableName = "destinostelefoniasector";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstDestinosPanelTlf.Tables[0]);
                                            }
                                            break;

                                        case CR_INF_SECTORES_SUBINF_PANEL_LC:

                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtstDestinosPanelLC != null && dtstDestinosPanelLC.Tables.Count > 0)
                                            {
                                                dtstDestinosPanelLC.Tables[0].TableName = "destinostelefoniasector";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstDestinosPanelLC.Tables[0]);
                                            }
                                            break;

                                        case CR_INF_SECTORES_SUBINF_PANEL_RADIO:

                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtstDestinosPanelRadio != null && dtstDestinosPanelRadio.Tables.Count > 0)
                                            {
                                                dtstDestinosPanelRadio.Tables[0].TableName = "destinosradiosector";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtstDestinosPanelRadio.Tables[0]);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        //Se leen el recurso Version LB_VERSION
                        if (null != GetGlobalResourceObject("Espaniol", "LB_VERSION"))
                        {
                            strVersion = GetGlobalResourceObject("Espaniol", "LB_VERSION").ToString();
                        }

                        //Se pasan los parámetros al informe. Los parámetros se deben pasar siempre después de configurar 
                        //los data Sources. Si no se hace así, la ejecución del report falla, indicando que le faltan parámetros
                        CRSourceInf.ReportDocument.SetParameterValue("p_version", strVersion.ToString());
                        CRSourceInf.ReportDocument.SetParameterValue("p_idEmplazamiento", strNucleo.ToString());
                        CRSourceInf.ReportDocument.SetParameterValue("p_bSAFTA", bConSafta);


                        //CRViewerInf.DataBind();
                        //CRViewerInf.Visible = true;
                        VisualizaInformePdf();
                    }
                    catch (System.Threading.ThreadAbortException )
                    { //ThreadException can happen for internale Response implementation
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
      try
      {
          if (null != CRSourceInf.ReportDocument && !string.IsNullOrEmpty(CRSourceInf.ReportDocument.FileName))
          {
              string strNombreInf = string.Empty;
              string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

              if ((GetLocalResourceObject("REP_NOMBRE_FICHERO") != null) && (!string.IsNullOrEmpty(GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString())))
                  strNombreInf = string.Format("{0}_{1}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(), DateTime.Now.ToString("yyyyMMddHHmm"));
              else
                  strNombreInf = string.Format("InfSectores_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));


              CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, strNombreInf);
          }
      }
      catch (Exception objEx)
      {
          logDebugView.Error(string.Format("InformeSectores.BtnPdf_Click: informe {0} -> Error al generar informe en PDF. Error:{1}", sFicheroReport, objEx.ToString()));
      }
  }  

  protected void BtnExcel_Click(object sender, ImageClickEventArgs e)
  {
      if (null != CRSourceInf.ReportDocument)
      {
          string strNombreInf = string.Empty;

          strNombreInf = string.Format("InfSectores_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));
          CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, strNombreInf);
      }

      try
      {
          if (null != CRSourceInf.ReportDocument && !string.IsNullOrEmpty(CRSourceInf.ReportDocument.FileName))
          {
              string strNombreInf = string.Empty;
              string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

              if ((GetLocalResourceObject("REP_NOMBRE_FICHERO") != null) && (!string.IsNullOrEmpty(GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString())))
                  strNombreInf = string.Format("{0}_{1}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(), DateTime.Now.ToString("yyyyMMddHHmm"));
              else
                  strNombreInf = string.Format("InfSectores_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

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
                strNombreInf = string.Format("InfSectores_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

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
