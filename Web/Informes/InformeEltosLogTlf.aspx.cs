using System;
using System.Collections.Generic;
//using System.Linq;
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

public partial class InformeEltosLogTlf : System.Web.UI.Page
{
    private static ILog _logDebugView;

    const string CONF_KEY_SISTEMA = "Sistema";

    //Nombres de los Ficheros de crystal report que contienen los informes
    const string CR_INF_LOGICAL_ELEMENTS = "InfCnfElementosLgTlf.rpt";        //Informe de configuración de elementos lógicos Telefonía

    //Subinformes contenidos en el informe CR_INF_LOGICAL_ELEMENTS
    const string CR_INF_LE_SUBINF_DEST_TLF = "SubInformeELTlfDestinos.rpt";                    //Subinforme de destinos de telefonía
    const string CR_INF_LE_SUBINF_DESTTLF_ATS_LCEN = "SubInformeELTlfDestinosAtsconLCEN.rpt";  //Subinforme de destinos de telefonía ATS con Destinos LCEN
    const string CR_INF_LE_SUBINF_RECTLF_NOASIGNADOS = "SubInformeELTlfNoAsignados.rpt";       //Subinforme de recursos de telefonía no asignados a ningun destino, troncal o red.

    static string sFicheroReport;

    static DataSet dtst;
    static DataSet dtsRecTlfNoAsignados;
    static DataSet dtsDestinoAtsConLCEN;
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
            dtsRecTlfNoAsignados = null;
            dtsDestinoAtsConLCEN = null;

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

            //Consulta de obtención de destinos de telefonía y sus recursos asociados
            strConsulta.Append("SELECT A.IdSistema, A.IdDestino, A.TipoDestino, A.IdGrupo, A.IdPrefijo, B.IdAbonado, C.IdRed, R.IdRecurso, R.TipoRecurso,");
            strConsulta.Append("R.idEquipos, R.IdTIFX,R.Tipo, R.Interface, R.SlotPasarela, R.NumDispositivoSlot, R.ServidorSIP, IFNULL(CAST(R.Diffserv AS CHAR),'0') AS Diffserv, T.IdRed AS recIdRed,");
            strConsulta.Append(" T.IdTroncal, T.Lado, T.Modo,T.TipoDestino AS rec_tipoDestino, PR.GananciaAGCTX, PR.GananciaAGCTXdBm, PR.GananciaAGCRX,");
            strConsulta.Append("PR.GananciaAGCRXdBm, PR.TamRTP, PR.TipoEM,PR.GrabacionEd137, PR.Codec, PR.SupresionSilencio ,0 AS IdDestinoLCEN,NULL AS IdPrefijoDestinoLCEN, NULL AS idSector, L.isuperv_options, L.itm_superv_options, PR.iDetDtmf, PR.iTmLlamEntrante,");
            strConsulta.Append("PR.iDetCallerId, PR.iTmCallerId, PR.iDetInversionPol, PR.iTmDetFinLlamada, PR.iPeriodoSpvRing, PR.iFiltroSpvRing, PR.TReleaseBL ");
            strConsulta.Append("FROM destinostelefonia A ");
            strConsulta.Append("INNER JOIN destinosexternos B ON A.IdSistema=B.IdSistema AND A.IdDestino=B.IdDestino AND A.TipoDestino=B.TipoDestino "); 
            strConsulta.Append("LEFT OUTER JOIN redes C ON C.IdSistema=A.IdSistema AND C.IdPrefijo=A.IdPrefijo LEFT OUTER JOIN recursostf T ON T.IdSistema=A.IdSistema AND ");
            strConsulta.Append(" T.IdDestino=A.IdDestino AND T.TipoDestino=A.TipoDestino AND T.TipoRecurso=1 ");
            strConsulta.Append("LEFT OUTER JOIN recursoslcen L ON L.IdSistema=A.IdSistema AND L.IdDestino=A.IdDestino AND L.TipoDestino=A.TipoDestino AND L.TipoRecurso=2  ");
            strConsulta.Append("LEFT OUTER JOIN recursos R ON ((R.IdSistema=T.IdSistema AND R.IdRecurso=T.IdRecurso AND R.TipoRecurso=T.TipoRecurso) OR ");
            strConsulta.Append("(R.IdSistema=L.IdSistema AND R.IdRecurso=L.IdRecurso AND R.TipoRecurso=L.TipoRecurso)) AND R.TipoRecurso<>0 ");
            strConsulta.Append("LEFT OUTER JOIN parametrosrecurso PR ON PR.IdSistema=R.IdSistema AND PR.IdRecurso=R.IdRecurso AND PR.TipoRecurso=R.TipoRecurso ");
            strConsulta.AppendFormat("WHERE A.IdSistema='{0}' ", strIdSistema);
            strConsulta.Append("AND ((A.IdPrefijo IN (1,3) AND ");
            strConsulta.Append("NOT EXISTS(SELECT 1 FROM destinosexternossector DE ");
            strConsulta.Append("WHERE DE.IdSistema=A.IdSistema AND (DE.IdPrefijoDestinoLCEN=A.IdPrefijo AND DE.IdDestinoLCEN=A.IdDestino AND A.IdPrefijo=1 AND DE.IdPrefijoDestinoLCEN=1) ");
            strConsulta.Append("OR (DE.IdDestino=A.IdDestino and de.IdPrefijo=A.IdPrefijo AND DE.IdDestinoLCEN is not null))) ");
            strConsulta.Append("OR (A.Idprefijo<>1 AND A.Idprefijo<>3) ) ");
            dtst = Servicio.ObtenerDataSet(strConsulta.ToString());

            //Se obtiene la lista de destinos LCEN asociados a los destinos ATS en el panel de línea caliente de algun sector
            strConsulta.Clear();

            strConsulta.Append("SELECT A.IdSistema, A.IdDestino, A.TipoDestino, A.IdGrupo, A.IdPrefijo, B.IdAbonado, C.IdRed, R.IdRecurso, R.TipoRecurso,");
            strConsulta.Append("R.idEquipos, R.IdTIFX,R.Tipo, R.Interface, R.SlotPasarela, R.NumDispositivoSlot, R.ServidorSIP, IFNULL(CAST(R.Diffserv AS CHAR),'0') AS Diffserv, C.IdRed AS recIdRed,");
            strConsulta.Append(" NULL AS IdTroncal,NULL AS Lado, NULL AS Modo,0 AS rec_tipoDestino, PR.GananciaAGCTX, PR.GananciaAGCTXdBm, PR.GananciaAGCRX,");
            strConsulta.Append("PR.GananciaAGCRXdBm, PR.TamRTP, PR.TipoEM,PR.GrabacionEd137, PR.Codec, PR.SupresionSilencio,DE.IdDestinoLCEN AS IdDestinoLCEN,DE.IdPrefijoDestinoLCEN AS IdPrefijoDestinoLCEN, DE.idSector AS idSector, L.isuperv_options, L.itm_superv_options  ");
            strConsulta.Append("FROM destinostelefonia A ");
            strConsulta.Append("INNER JOIN destinosexternos B ON A.IdSistema=B.IdSistema AND A.IdDestino=B.IdDestino AND A.TipoDestino=B.TipoDestino ");
            strConsulta.Append("INNER JOIN destinosexternossector DE ON DE.IdSistema=A.IdSistema AND DE.IdDestino=A.IdDestino  AND DE.TipoDestino=A.TipoDestino AND DE.idPrefijo=A.idPrefijo AND DE.IdDestinoLCEN IS NOT NULL ");
            strConsulta.Append("AND   DE.IdPrefijoDestinoLCEN IS NOT NULL ");
            strConsulta.Append("LEFT OUTER JOIN redes C ON C.IdSistema = A.IdSistema AND C.IdPrefijo = A.IdPrefijo ");
            strConsulta.Append("LEFT OUTER JOIN recursoslcen L ON L.IdSistema=DE.IdSistema AND L.IdDestino=DE.IdDestinoLCEN AND L.TipoDestino=A.TipoDestino AND L.TipoRecurso=2 ");
            strConsulta.Append(" LEFT OUTER JOIN recursos R ON R.IdSistema=L.IdSistema AND R.IdRecurso=L.IdRecurso AND R.TipoRecurso=L.TipoRecurso AND R.TipoRecurso<>0 ");
            strConsulta.Append(" LEFT OUTER JOIN parametrosrecurso PR ON PR.IdSistema=R.IdSistema AND PR.IdRecurso=R.IdRecurso AND PR.TipoRecurso=R.TipoRecurso ");
            strConsulta.AppendFormat(" WHERE A.IdSistema='{0}' AND A.idprefijo=3 ", strIdSistema);

            dtsDestinoAtsConLCEN = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            //Consulta de obtención de recursos de telefonía sin destino troncal o red asociado asociado
            strConsulta.Append("SELECT R.IdRecurso, R.TipoRecurso, R.idEquipos, R.IdTIFX,T.idDestino,L.IDDESTINO AS Lcen_IdDestino,");
            strConsulta.Append("R.Tipo, R.Interface, R.SlotPasarela, R.NumDispositivoSlot, R.ServidorSIP, R.Diffserv, T.IdRed AS recIdRed, T.IdTroncal, T.Lado, T.Modo,");
            strConsulta.Append("T.TipoDestino AS rec_tipoDestino, PR.GananciaAGCTX, PR.GananciaAGCTXdBm, PR.GananciaAGCRX, PR.GananciaAGCRXdBm, PR.TamRTP, PR.TipoEM,");
            strConsulta.Append("PR.GrabacionEd137, PR.Codec, PR.SupresionSilencio, L.isuperv_options, L.itm_superv_options, PR.iDetDtmf, PR.iTmLlamEntrante,"); 
            strConsulta.Append("PR.iDetCallerId, PR.iTmCallerId, PR.iDetInversionPol, PR.iTmDetFinLlamada, PR.iPeriodoSpvRing, PR.iFiltroSpvRing, PR.TReleaseBL ");
            strConsulta.Append("FROM recursos R  ");
            strConsulta.Append("INNER JOIN parametrosrecurso PR ON PR.IdSistema=R.IdSistema AND PR.IdRecurso=R.IdRecurso AND PR.TipoRecurso=R.TipoRecurso ");
            strConsulta.Append("LEFT OUTER JOIN recursostf T ON R.IdSistema=T.IdSistema AND R.IdRecurso=T.IdRecurso AND R.TipoRecurso=T.TipoRecurso ");
            strConsulta.Append("LEFT OUTER JOIN recursoslcen L ON R.IdSistema=L.IdSistema AND R.IdRecurso=L.IdRecurso AND R.TipoRecurso=L.TipoRecurso ");
            strConsulta.Append("WHERE  R.TipoRecurso<>0 AND T.idDestino IS NULL AND L.idDestino IS NULL AND  T.IdTroncal IS NULL AND T.IdRed is null  ");
            strConsulta.AppendFormat("AND R.IdSistema='{0}'", strIdSistema);

            dtsRecTlfNoAsignados = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            if (System.IO.File.Exists(sFicheroReport))
            {
                CRSourceInf.Report.FileName = sFicheroReport;
                CRSourceInf.ReportDocument.Load(sFicheroReport);

                TraducirInforme(CRSourceInf.ReportDocument, strIdioma);

                //Asociamos los datasources
                if (null != dtst && dtst.Tables.Count > 0)
                {
                    try
                    {
                        if (null != GetLocalResourceObject("LB_INFORME_ELEMENTOS_LOG_TLF_SUMMARY"))
                        {
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = GetLocalResourceObject("LB_INFORME_ELEMENTOS_LOG_TLF_SUMMARY").ToString();
                        }
                        else
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = "Informe de Configuración de Destinos y Recursos de Telefonía";

                        //El informe principal tiene 2 subreports
                        //Le asignamos los datos
                        if (CRSourceInf.ReportDocument.Subreports.Count > 0)
                        {
                            for (int i = 0; i < CRSourceInf.ReportDocument.Subreports.Count; i++)
                            {
                                if (CRSourceInf.ReportDocument.Subreports[i] != null)
                                {
                                    switch (CRSourceInf.ReportDocument.Subreports[i].Name.ToString())
                                    {
                                        case CR_INF_LE_SUBINF_DEST_TLF:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtst != null && dtst.Tables.Count > 0)
                                            {
                                                dtst.Tables[0].TableName = "DestinosTlf";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtst);
                                            }
                                            break;

                                         case CR_INF_LE_SUBINF_DESTTLF_ATS_LCEN:

                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsDestinoAtsConLCEN != null && dtsDestinoAtsConLCEN.Tables.Count > 0)
                                            {
                                                dtsDestinoAtsConLCEN.Tables[0].TableName = "RecursosTlf";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsDestinoAtsConLCEN.Tables[0]);
                                            }
                                            break;
                                        case CR_INF_LE_SUBINF_RECTLF_NOASIGNADOS:

                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsRecTlfNoAsignados != null && dtsRecTlfNoAsignados.Tables.Count > 0)
                                            {
                                                dtsRecTlfNoAsignados.Tables[0].TableName = "RecursosTlf";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsRecTlfNoAsignados.Tables[0]);
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
                    catch (System.Threading.ThreadAbortException )
                    { //ThreadException can happen for internale Response implementation
                        if (dtst != null)
                            dtst.Clear();
                        if (dtsRecTlfNoAsignados != null)
                            dtsRecTlfNoAsignados.Clear();
                        if (dtsDestinoAtsConLCEN != null)
                            dtsDestinoAtsConLCEN.Clear();
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
                strNombreInf = string.Format("InfCnfTelefonia_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));


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
                  strNombreInf = string.Format("InfCnfTelefonia_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));


              CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, strNombreInf);
          }

      }
      catch (Exception objEx)
      {
          logDebugView.Error(string.Format("BtnExcel_Click: informe {0} -> Error al generar informe en Excel. Error:{1}", sFicheroReport, objEx.ToString()));
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
                strNombreInf = string.Format("InfCnfTelefonia_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

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
            catch (Exception ex)
            {
                string strError = ex.Message.ToString();
            }
        }
  }

}
