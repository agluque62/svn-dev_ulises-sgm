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

public partial class InformeTablasConversionRadio : System.Web.UI.Page
{
    private static ILog _logDebugView;

    const string CONF_KEY_SISTEMA = "Sistema";

    //Nombres de los Ficheros de crystal report que contienen los informes
    const string CR_INF_CONVERSION_TAB_CALIDAD = "InfCnfTabCalidadRadio.rpt";  //Informe de configuración de tablas de conversión de índices de calidad radio

    //Subinformes del informe
    const string CR_INF_CONVERSION_TAB_CALIDAD_VALORES = "SubInformeELCalidadRadio.rpt";     // Subinforme que muestra los valores de la tabla de calidad
    const string CR_INF_CONVERSION_TAB_CALIDAD_RECURSOS = "SubInformeELRecTabCalidadRadio.rpt"; // Subinforme que muestra la lista de recursos que utilizan una tabla de calidad determinada 

    static string sFicheroReport;

    static DataSet dtst;
    static DataSet dtsTabCalidad;
    static DataSet dtsRecTabCalidad;
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
            string strIdSistema = string.Empty;

            strVersion=strNucleo=string.Empty;
            dtst = null;
            dtsTabCalidad = null;
            dtsRecTabCalidad = null;

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

            //Se obtiene el nombre del fichero del informe
            sFicheroReport = Server.MapPath("~/Informes/" + CR_INF_CONVERSION_TAB_CALIDAD);


            strConsulta.Append("SELECT idtabla_bss, name FROM tabla_bss ");
            dtst = Servicio.ObtenerDataSet(strConsulta.ToString());
            strConsulta.Clear();

            //Se obtienen los datos a visualizar en cada subinforme
            //Se recuperan las tablas de calidad configuradas en el sistema
            strConsulta.Append("SELECT tb.name AS nombre, vt.idvalores_tabla, vt.valor_prop, vt.valor_rssi FROM  valores_tabla vt ");
            strConsulta.Append("INNER JOIN tabla_bss tb ON tb.idtabla_bss = vt.tabla_bss_idtabla_bss ORDER BY nombre, vt.valor_prop ");

            dtsTabCalidad = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            //Se recuperan los valores de las tablas de calidad radio
            strConsulta.Append("SELECT tb.idtabla_bss, tb.name AS nombre, vt.idvalores_tabla, vt.valor_prop, vt.valor_rssi FROM  valores_tabla vt ");
            strConsulta.Append("INNER JOIN tabla_bss tb ON tb.idtabla_bss = vt.tabla_bss_idtabla_bss ORDER BY nombre, vt.valor_prop ");

            dtsTabCalidad = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            strConsulta.Append("SELECT RD.IdSistema, RD.IdRecurso, RD.TipoRecurso, RD.IdDestino, RD.tabla_bss_idtabla_bss,R.idEquipos, B.name, R.IdTIFX, R.SlotPasarela, R.NumDispositivoSlot,RD.BSS ");
            strConsulta.Append("FROM  recursosradio RD ");
            strConsulta.Append("INNER JOIN recursos R ON R.IdSistema=RD.IdSistema AND R.IdRecurso=RD.IdRecurso AND R.TipoRecurso=RD.TipoRecurso ");
            strConsulta.Append("INNER JOIN tabla_bss B ON B.idtabla_bss=RD.tabla_bss_idtabla_bss ");
            strConsulta.AppendFormat("WHERE r.IdSistema='{0}' ORDER BY B.NAME,RD.IdDestino, RD.IdRecurso", strIdSistema);

            dtsRecTabCalidad = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            if (System.IO.File.Exists(sFicheroReport))
            {
                CRSourceInf.Report.FileName = sFicheroReport;
                CRSourceInf.ReportDocument.Load(sFicheroReport);

                TraducirInforme(CRSourceInf.ReportDocument, strIdioma);

                //Se añaden los dataset al informe principal y a los subinformes
                if (null != dtst && dtst.Tables.Count > 0)
                {
                    try
                    {
                        if (null != GetLocalResourceObject("LB_INFORME_CONVERSION_TAB_CALIDAD_SUMMARY"))
                        {
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = GetLocalResourceObject("LB_INFORME_CONVERSION_TAB_CALIDAD_SUMMARY").ToString();
                        }
                        else
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = "Informe de Tablas de Conversión de índices de calidad radio";

                        CRSourceInf.ReportDocument.SetDataSource(dtst.Tables[0]);
                        if (CRSourceInf.ReportDocument.Subreports.Count > 0)
                        {
                            for (int i = 0; i < CRSourceInf.ReportDocument.Subreports.Count; i++)
                            {
                                if (CRSourceInf.ReportDocument.Subreports[i] != null)
                                {
                                    switch (CRSourceInf.ReportDocument.Subreports[i].Name.ToString())
                                    {
                                        case CR_INF_CONVERSION_TAB_CALIDAD_VALORES:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsTabCalidad != null && dtsTabCalidad.Tables.Count > 0)
                                            {
                                                dtsTabCalidad.Tables[0].TableName = "tabla_bss";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsTabCalidad.Tables[0]);
                                            }
                                            break;

                                        case CR_INF_CONVERSION_TAB_CALIDAD_RECURSOS:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsRecTabCalidad != null && dtsRecTabCalidad.Tables.Count > 0)
                                            {
                                                dtsRecTabCalidad.Tables[0].TableName = "RecursosRadioTablaBSS";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsRecTabCalidad.Tables[0]);
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

                        //CRViewerInf.DataBind();
                        //CRViewerInf.Visible = true;
                        VisualizaInformePdf();

                    }
                    catch (System.Threading.ThreadAbortException ex)
                    { //ThreadException can happen for internale Response implementation
                        //La visualización del informe en pdf lanza la excepción interna
                        //Se liberan los dataset
                        if (dtst != null)
                            dtst.Clear();

                        if (dtsTabCalidad != null)
                            dtsTabCalidad.Clear();

                        if (dtsRecTabCalidad != null)
                            dtsRecTabCalidad.Clear();

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

      try
      {
          if (null != CRSourceInf.ReportDocument && !string.IsNullOrEmpty(CRSourceInf.ReportDocument.FileName))
          {
              string strNombreInf = string.Empty;
              string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

              if ((GetLocalResourceObject("REP_NOMBRE_FICHERO") != null) && (!string.IsNullOrEmpty(GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString())))
                  strNombreInf = string.Format("{0}_{1}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(), DateTime.Now.ToString("yyyyMMddHHmm"));
              else
                  strNombreInf = string.Format("InfCnfTabCalidadRadio_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));


              CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, strNombreInf);

          }
      }
      catch (Exception objEx)
      {
          logDebugView.Error(string.Format("BtnPdf_Click: informe {0} -> Error al generar informe en PDF. Error:{1}", sFicheroReport, objEx.ToString()));
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
                  strNombreInf = string.Format("InfCnfTabCalidadRadio_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));


              CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, strNombreInf);
          }

      }
      catch (Exception objEx)
      {
          logDebugView.Error(string.Format("BtnPdf_Click: informe {0} -> Error al generar informe en PDF. Error:{1}", sFicheroReport, objEx.ToString()));
      }
  }

  protected void VisualizaInformePdf()
  {
    //Visualiza el informe en formato Pdf
    if (null != CRSourceInf.ReportDocument && !string.IsNullOrEmpty(CRSourceInf.ReportDocument.FileName))
    {
        string strNombreInf = string.Empty;
        string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        if ((GetLocalResourceObject("REP_NOMBRE_FICHERO") != null) && (!string.IsNullOrEmpty(GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString())))
            strNombreInf = string.Format("{0}_{1}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(), DateTime.Now.ToString("yyyyMMddHHmm"));
        else
            strNombreInf = string.Format("InfCnfTabCalidadRadio_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

        Response.Buffer = false;
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/pdf";

        try
        {
            CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, false, strNombreInf);
        }
        catch (System.Threading.ThreadAbortException objEx)
        { //ThreadException can happen for internale Response implementation

        }
        catch (Exception)
        {
        }
    }

  }

}
