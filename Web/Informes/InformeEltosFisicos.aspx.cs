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

public partial class InformeEltosFisicos : System.Web.UI.Page
{
    private static ILog _logDebugView;

    const string  INF_HW_ELEMENTS =      "REP_HW_ELEMENTS";
    const string CONF_KEY_CON_SACTA = "SistemaConSACTA";
    const string CONF_KEY_SISTEMA = "Sistema";

    //Nombres de los Ficheros de crystal report que contienen los informes
    const string CR_INF_HW_ELEMENTS = "InfCnfElementosHw.rpt";                    //Informe de configuración de elementos físicos

    //Subinformes contenidos en el informe CR_INF_HW_ELEMENTS
    const string CR_INF_HW_ELEMENTS_SUBINF_TO = "SubInfTerminalOperadores.rpt";   // Subinforme de terminales de operador
    const string CR_INF_HW_ELEMENTS_SUBINF_PASARELAS = "SubInformePasarelas.rpt"; // Subinforme de pasarelas
    const string CR_INF_HW_ELEMENTS_SUBINF_EQUIPOS_EXT = "SubInformeEquiposExternos.rpt"; // Subinforme de equipos Externos

    static string sFicheroReport;

    static DataSet dtst;
    static DataSet dtsPasarelas;
    static DataSet dtsEquiposExternos;
    static string strVersion;
    static string strNucleo;
    static string strIdSistema=string.Empty;
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
            string strIdSistema = string.Empty;

            strVersion=strNucleo=string.Empty;
            dtst = null;
            dtsPasarelas = null;
            dtsEquiposExternos = null;

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
                strNucleo=((ServiciosCD40.Nucleos)dTabNucleos[0]).IdNucleo;
                dTabNucleos = null; //Liberamos la memoria
            }

            //Informe de configuración de elementos físicos
            //sFicheroReport = Server.MapPath(CR_INF_HW_ELEMENTS);
            sFicheroReport = Server.MapPath("~/Informes/" + CR_INF_HW_ELEMENTS);

            //Se obtienen los datos a visualizar en cada subinforme
            //Subinforme de puestos de operador
            ServiciosCD40.Top tabPuestosOp = new ServiciosCD40.Top();
            tabPuestosOp.IdSistema = strIdSistema;

            dtst = Servicio.DataSetSelectSQL(tabPuestosOp);

            //Subinforme pasarelas
            strConsulta.Append("SELECT tifx.IdSistema, tifx.IdTIFX, tifx.ModoArranque, tifx.ModoSincronizacion, tifx.Master, ");
            strConsulta.Append("tifx.SNMPPortLocal, tifx.SNMPPortRemoto, tifx.SNMPTraps, tifx.SIPPortLocal, ");
            strConsulta.Append("tifx.TimeSupervision, tifx.IpRed1, tifx.IpRed2, tifx.Grabador1, tifx.Grabador2, ");
            strConsulta.Append("gwactivas.IpRed AS ipComunicaciones, recursos.IdRecurso, recursos.TipoRecurso,  ");
            strConsulta.Append(" recursos.idEquipos, recursos.Tipo, recursos.Interface, recursos.SlotPasarela, ");
            strConsulta.Append("recursos.NumDispositivoSlot, recursos.ServidorSIP, recursos.Diffserv, tifx.iSupervLanGW, tifx.itmmaxSupervLanGW ");
            strConsulta.Append("FROM tifx LEFT OUTER JOIN recursos ON tifx.IdSistema = recursos.IdSistema ");
            strConsulta.Append("AND tifx.IdTIFX = recursos.IdTIFX LEFT OUTER JOIN ");
            strConsulta.Append("gwactivas ON tifx.IdSistema = gwactivas.IdSistema AND tifx.IdTIFX = gwactivas.IdTifx ");
            strConsulta.AppendFormat("AND tifx.IdSistema='{0}'",strIdSistema);

            dtsPasarelas=Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            //Subinforme Equipos externos
            strConsulta.Append("SELECT IdSistema, idEquipos, IpRed1, IpRed2, TipoEquipo, Interno, SipPort, IpRed3,");
            strConsulta.Append("SrvPresenciaIpRed1, SrvPresenciaIpRed2, SrvPresenciaIpRed3 FROM equiposeu ");
            strConsulta.Append("WHERE (Min!=-1 AND MAX!=-1)");

            dtsEquiposExternos = Servicio.ObtenerDataSet(strConsulta.ToString());

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
                        if (null != GetLocalResourceObject("LB_INFORME_ELEMENTOS_FISICOS_SUMMARY"))
                        {
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = GetLocalResourceObject("LB_INFORME_ELEMENTOS_FISICOS_SUMMARY").ToString();
                        }
                        else
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = "Informe de Configuración de Elementos Físicos";

                        //El informe principal tiene tres subreports
                        //Le asignamos los datos
                        if (CRSourceInf.ReportDocument.Subreports.Count > 0)
                        {
                            for (int i = 0; i < CRSourceInf.ReportDocument.Subreports.Count; i++)
                            {
                                if (CRSourceInf.ReportDocument.Subreports[i] != null)
                                {

                                    switch (CRSourceInf.ReportDocument.Subreports[i].Name.ToString())
                                    {
                                        case CR_INF_HW_ELEMENTS_SUBINF_TO:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtst != null && dtst.Tables.Count > 0)
                                            {
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtst.Tables[0]);
                                            }
                                            break;

                                        case CR_INF_HW_ELEMENTS_SUBINF_PASARELAS:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsPasarelas != null && dtsPasarelas.Tables.Count > 0)
                                            {
                                                dtsPasarelas.Tables[0].TableName = "Pasarelas";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsPasarelas.Tables[0]);
                                            }
                                            break;

                                        case CR_INF_HW_ELEMENTS_SUBINF_EQUIPOS_EXT:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsEquiposExternos != null && dtsEquiposExternos.Tables.Count > 0)
                                            {
                                                dtsEquiposExternos.Tables[0].TableName = "equiposeu";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsEquiposExternos.Tables[0]);
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
                            strVersion=GetGlobalResourceObject("Espaniol", "LB_VERSION").ToString();
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
                    catch (System.Threading.ThreadAbortException)
                    { //ThreadException can happen for internale Response implementation
                        if (dtst != null)
                            dtst.Clear();
                        if (dtsPasarelas!=null)
                            dtsPasarelas.Clear();
                        if (dtsEquiposExternos != null)
                            dtsEquiposExternos.Clear();
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
                logDebugView.Error(string.Format("Report.TraducirTexto: informe {0} -> No se ha encontrado la etiqueta {1} en el fichero de recursos", sFicheroReport, key));

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
                  strNombreInf = string.Format("InfCnfElementosFisicos_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));


              CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, strNombreInf);

          }
      }
      catch (Exception objEx)
      {
          logDebugView.Error(string.Format("InformeEltosFisicos.BtnPdf_Click: informe {0} -> Error al generar informe en PDF. Error:{1}", sFicheroReport, objEx.ToString()));
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
                  strNombreInf = string.Format("InfCnfElementosFisicos_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));


              CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, strNombreInf);
          }

      }
      catch (Exception objEx)
      {
          logDebugView.Error(string.Format("InformeEltosFisicos.BtnPdf_Click: informe {0} -> Error al generar informe en PDF. Error:{1}", sFicheroReport, objEx.ToString()));
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
                strNombreInf = string.Format("InfCnfElementosFisicos_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

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
