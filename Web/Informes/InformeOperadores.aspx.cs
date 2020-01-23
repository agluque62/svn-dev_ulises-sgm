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

public partial class InformeOperadores : System.Web.UI.Page
{
    private static ILog _logDebugView;

    static string sFicheroReport;

    //Tipos de informes
    const string CONF_KEY_SISTEMA = "Sistema";

    //Nombre deL Fichero de crystal report que contiene el informe
    const string CR_INF_OPERADORES = "InfOperadores.rpt";

    static DataSet dtst;
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
            //A esta pantalla solo tienen acceso los usuarios de perfil 3
            if (string.Compare(perfil,"0")==0)
            {
                Response.Redirect("~/Login.aspx", false);
                return;
            }
            else if (string.Compare(perfil, "3") != 0)
            {
                //solo el usuario con perfil 3 tiene acceso al informe de operadores
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
            strVersion=strNucleo=string.Empty;
            dtst = null;
            strIdSistema = string.Empty;

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


            //El informe de operadores se invoca directamente el árbol de menú
            //sFicheroReport = Server.MapPath("~/Informes/InfOperadores.rpt");
            sFicheroReport = Server.MapPath("~/Informes/" + CR_INF_OPERADORES);

            //Se obtienen los datos a visualizar en el informe en un dataSet
            ServiciosCD40.Operadores tabOp = new ServiciosCD40.Operadores();
            tabOp.IdSistema = strIdSistema;

            dtst = Servicio.DataSetSelectSQL(tabOp);

            if (!string.IsNullOrEmpty(sFicheroReport) && System.IO.File.Exists(sFicheroReport))
            {
                CRSourceInf.Report.FileName = sFicheroReport;
                CRSourceInf.ReportDocument.Load(sFicheroReport);

                TraducirInforme(CRSourceInf.ReportDocument, strIdioma);

                //Asociamos el datasource
                if (null != dtst && dtst.Tables.Count > 0)
                {
                    if (null != GetLocalResourceObject("LB_INFORME_OPERADORES"))
                    {
                        CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = GetLocalResourceObject("LB_INFORME_OPERADORES").ToString();
                    }
                    else
                        CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = "Informe de Operadores";

                    CRSourceInf.ReportDocument.SetDataSource(dtst.Tables[0].Copy());

                    //Se leen los recursos Version LB_VERSION
                    if (null != GetGlobalResourceObject("Espaniol", "LB_VERSION"))
                    {
                        strVersion = GetGlobalResourceObject("Espaniol", "LB_VERSION").ToString();
                    }

                    CRSourceInf.ReportDocument.SetParameterValue("p_version", strVersion.ToString());
                    CRSourceInf.ReportDocument.SetParameterValue("p_idEmplazamiento", strNucleo.ToString());

                    //CRViewerInf.DataBind();
                    //CRViewerInf.Visible = true;
                    VisualizaInformePdf();
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

        var culture = CultureInfo.CreateSpecificCulture(lang);

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
                  strNombreInf = string.Format("InfOperadores_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

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
                  strNombreInf = string.Format("InfOperadores_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));


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
      //Visualiza el informe en formato Pdf

    bool bCorrecto = true;

    if (null != CRSourceInf.ReportDocument && !string.IsNullOrEmpty(CRSourceInf.ReportDocument.FileName))
    {
        string strNombreInf = string.Empty;
        string strIdioma = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        if ((GetLocalResourceObject("REP_NOMBRE_FICHERO") != null) && (!string.IsNullOrEmpty(GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString())))
            strNombreInf = string.Format("{0}_{1}", GetLocalResourceObject("REP_NOMBRE_FICHERO").ToString(), DateTime.Now.ToString("yyyyMMddHHmm"));
        else
            strNombreInf = string.Format("InfOperadores_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

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
            bCorrecto = false;
        }
        catch (Exception)
        {
            bCorrecto = false;
        }

    }
  }

}
