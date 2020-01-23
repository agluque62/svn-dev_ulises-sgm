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

public partial class InformeEltosLogEncamina: System.Web.UI.Page
{
    private static ILog _logDebugView;

    const string CONF_KEY_SISTEMA = "Sistema";

    //Nombres de los Ficheros de crystal report que contienen los informes
    const string CR_INF_LOGICAL_ELEMENTS = "InfCnfElementosLgEncamina.rpt";   //Informe de configuración de encaminamientos (Centrales ATS)

    //Subinformes contenidos en el informe CR_INF_LOGICAL_ELEMENTS
    const string CR_INF_LE_SUBINF_RANGOS = "SubInformeElEncaminaRangos.rpt";             //Subinforme de Rangos de la central
    const string CR_INF_LE_SUBINF_SERVIDORESIP = "SubInformeElEncaminaAccesoIP.rpt";     //Subinforme con los datos de la Central IP
    const string CR_INF_LE_SUBINF_RUTAS = "SubInformeELEncaminaRutas.rpt";               //Subinforme con las rutas de la Central no propia
    const string CR_INF_LE_SUBINF_REDES = "SubInformeELEncaminaRedes.rpt";               //Subinforme con las redes de la Central propia
    const string CR_INF_LE_SUBINF_DESTINOSLCEN = "SubInformeELEncaminaLCEN.rpt";    //Subinforme con los Destinos LCEN asociados a algún Destino ATS en algún sector cuyo número de abonado se encuentra dentro del rango de la central

    static string sFicheroReport;

    static DataSet dtst;
    static DataSet dtsRangos;
    static DataSet dtsDatosServidoresIP;
    static DataSet dtsRutas;
    static DataSet dtsRedes;
    static DataSet dtsDestinosATS;
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
            dtsRangos = null;
            dtsDatosServidoresIP = null;
            dtsRutas=null;
            dtsRedes=null;
            dtsDestinosATS = null;

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
            CRViewerInf.SeparatePages = false;

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


            //Consulta de obtención de los encaminamientos
            strConsulta.Append("SELECT  IdSistema, Central, CentralPropia, Throwswitching, NumTest, CentralIp FROM encaminamientos ");
            strConsulta.AppendFormat("WHERE IdSistema='{0}' ORDER BY CentralPropia DESC,CENTRAL ", strIdSistema);
            
            dtst= Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            //Consulta de obtención de los rangos
            strConsulta.Append("SELECT R.IdSistema, R.Central, R.Tipo, R.Inicial, R.IdPrefijo, R.IdAbonado, R.Final, RD.IdRed ");
            strConsulta.Append("FROM rangos R LEFT OUTER JOIN ");
            strConsulta.Append("redes RD ON R.IdPrefijo=RD.IdPrefijo AND R.IdPrefijo<>0 ");
            strConsulta.AppendFormat("WHERE  R.IdSistema='{0}'", strIdSistema);
            
            dtsRangos = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            //Consulta de obtención de los servidores de proxy y de presencia de las centrales ATS
            strConsulta.Append("SELECT IdSistema, idEquipos, IpRed1, IpRed2, IpRed3, SrvPresenciaIpRed1, SrvPresenciaIpRed2, SrvPresenciaIpRed3, Interno,Min,Max ");
            strConsulta.AppendFormat("FROM  equiposeu WHERE (Min= - 1) AND (Max=-1) AND IdSistema='{0}' ", strIdSistema);
            
            dtsDatosServidoresIP = Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            //Se obtienen la lista de rutas, troncales y recursos asociadas a las centrales ATS
            strConsulta.Append("SELECT R.IdSistema,R.Central,R.IdRuta,R.Tipo,R.Orden,T.IdTroncal,RE.IdRecurso,RE.TipoRecurso,RE.idEquipos,RE.IdTIFX,RE.Tipo AS rec_tipo,");
            strConsulta.Append("RE.Interface,RE.SlotPasarela,RE.NumDispositivoSlot,RE.ServidorSIP,RE.Diffserv,TF.Lado,TF.Modo,TF.IdPrefijo,PR.GananciaAGCTX, PR.GananciaAGCTXdBm,");
            strConsulta.Append("PR.GananciaAGCRX, PR.GananciaAGCRXdBm, PR.TamRTP, PR.TipoEM, PR.GrabacionEd137, PR.Codec, PR.SupresionSilencio, ");
            strConsulta.Append("PR.iTmDetFinLlamada, PR.iTmCallerId, PR.iDetInversionPol, PR.iDetCallerId ");
            strConsulta.Append("FROM rutas R LEFT OUTER JOIN troncalesruta T ON T.IdSistema=R.IdSistema AND T.Central=R.Central AND T.IdRuta=R.IdRuta ");
            strConsulta.Append("LEFT OUTER JOIN recursostf TF ON TF.IdSistema=T.IdSistema AND TF.IdTroncal=T.IdTroncal AND TF.TipoRecurso=1 ");
            strConsulta.Append("LEFT OUTER JOIN recursos RE ON RE.IdSistema=TF.IdSistema AND RE.IdRecurso=TF.IdRecurso AND RE.TipoRecurso=TF.TipoRecurso ");
            strConsulta.Append("LEFT OUTER JOIN parametrosrecurso PR ON PR.IdSistema=RE.IdSistema AND PR.IdRecurso=RE.IdRecurso AND PR.TipoRecurso=RE.TipoRecurso ");
            strConsulta.AppendFormat("WHERE  R.IdSistema='{0}' ORDER BY R.Central,R.Orden,T.IdTroncal,RE.IdRecurso", strIdSistema);
            
            dtsRutas=Servicio.ObtenerDataSet(strConsulta.ToString());

            strConsulta.Clear();

            //Se obtienen la lista de redes con los recursos asociados
            strConsulta.Append("SELECT R.IdSistema,R.IdRed,r.IdPrefijo,RE.IdRecurso,RE.TipoRecurso,RE.idEquipos,RE.IdTIFX,RE.Tipo AS rec_tipo,");
            strConsulta.Append("RE.Interface,RE.SlotPasarela,RE.NumDispositivoSlot,RE.ServidorSIP,RE.Diffserv,TF.Lado,TF.Modo,TF.IdPrefijo,PR.GananciaAGCTX, PR.GananciaAGCTXdBm,");
            strConsulta.Append("PR.GananciaAGCRX, PR.GananciaAGCRXdBm, PR.TamRTP, PR.TipoEM, PR.GrabacionEd137, PR.Codec, PR.SupresionSilencio, ");
            strConsulta.Append("PR.iTmDetFinLlamada, PR.iTmCallerId, PR.iDetInversionPol, PR.iDetCallerId ");
            strConsulta.Append("FROM redes R  ");
            strConsulta.Append("LEFT OUTER JOIN recursostf TF ON TF.IdSistema=R.IdSistema AND TF.IdRed=R.IdRed AND TF.TipoRecurso<>0 ");
            strConsulta.Append("LEFT OUTER JOIN recursos RE ON RE.IdSistema=TF.IdSistema AND RE.IdRecurso=TF.IdRecurso AND RE.TipoRecurso=TF.TipoRecurso ");
            strConsulta.Append("LEFT OUTER JOIN parametrosrecurso PR ON PR.IdSistema=RE.IdSistema AND PR.IdRecurso=RE.IdRecurso AND PR.TipoRecurso=RE.TipoRecurso ");
            strConsulta.AppendFormat("WHERE  R.IdSistema='{0}' ORDER BY R.idRed,RE.IdRecurso", strIdSistema);
            dtsRedes = Servicio.ObtenerDataSet(strConsulta.ToString());
            
            strConsulta.Clear();
            //Se obtienen la lista de destinos LCEN y sus recursos que están asociados a algún Destino ATS en el panel de línea caliente de un sector, cuyo
            //número de abonado se encuentra dentro de algún rango de las centrales configuradas.
            strConsulta.Append("SELECT A.IdSistema,A.IdDestino,A.TipoDestino,A.IdPrefijo,E.CENTRAL,R.IdRecurso,R.TipoRecurso,R.idEquipos, ");
            strConsulta.Append("R.IdTIFX,R.Tipo,R.Interface,R.SlotPasarela,R.NumDispositivoSlot,R.ServidorSIP,IFNULL(CAST(R.Diffserv AS CHAR),'0') AS Diffserv, ");
            strConsulta.Append("PR.GananciaAGCTX,PR.GananciaAGCTXdBm,PR.GananciaAGCRX,PR.GananciaAGCRXdBm,");
            strConsulta.Append("PR.TamRTP, PR.TipoEM,PR.GrabacionEd137, PR.Codec, PR.SupresionSilencio,DE.idsector AS idSector,DE.idDestino AS IdDestinoSector, ");
            strConsulta.Append("PR.iTmDetFinLlamada, PR.iTmCallerId, PR.iDetInversionPol, PR.iDetCallerId ");
            strConsulta.Append("FROM destinostelefonia A ");
            strConsulta.Append("INNER JOIN destinosexternossector DE ON  DE.IdSistema=A.IdSistema AND DE.TipoDestino=A.TipoDestino AND ");
            strConsulta.Append("DE.IdPrefijoDestinoLCEN=A.idPrefijo AND DE.IdDestinoLCEN=A.IdDestino ");
            strConsulta.Append("INNER JOIN ENCAMINAMIENTOS E ON A.IdSistema=E.IdSistema ");
            strConsulta.Append("LEFT OUTER JOIN recursoslcen L ON L.IdSistema=A.IdSistema AND L.IdDestino=A.IdDestino AND L.TipoDestino=A.TipoDestino AND L.TipoRecurso=2 ");
            strConsulta.Append("LEFT OUTER JOIN recursos R ON R.IdSistema=L.IdSistema AND R.IdRecurso=L.IdRecurso AND R.TipoRecurso=L.TipoRecurso AND R.TipoRecurso<>0 ");
            strConsulta.Append("LEFT OUTER JOIN parametrosrecurso PR ON PR.IdSistema=R.IdSistema AND PR.IdRecurso=R.IdRecurso AND PR.TipoRecurso=R.TipoRecurso ");
            strConsulta.AppendFormat("WHERE A.IdSistema='{0}' AND A.idprefijo=1 AND ", strIdSistema);
            strConsulta.Append("EXISTS(SELECT 1 FROM destinosexternos D, RANGOS R ");
            strConsulta.Append("WHERE D.IdSistema=DE.IdSistema AND D.TipoDestino=DE.TipoDestino AND D.IdPrefijo=DE.IdPrefijo AND D.IdDestino=DE.IdDestino AND DE.IdPrefijo=3 AND ");
            strConsulta.Append("R.IdSistema=D.IdSistema AND cast(D.IdAbonado as CHAR) BETWEEN CAST(R.inicial AS CHAR) AND CAST(R.final AS CHAR) ");
            strConsulta.Append("AND E.IdSistema=D.IdSistema AND R.CENTRAL=E.CENTRAL) ORDER BY CENTRAL,idDestino");
            
            dtsDestinosATS=Servicio.ObtenerDataSet(strConsulta.ToString());

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
                        if (null != GetLocalResourceObject("LB_INFORME_EL_ENCAMINA_SUMMARY"))
                        {
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = GetLocalResourceObject("LB_INFORME_EL_ENCAMINA_SUMMARY").ToString();
                        }
                        else
                            CRSourceInf.ReportDocument.SummaryInfo.ReportTitle = "Informe de Configuración de Encaminamientos de Telefonía";

                        dtst.Tables[0].TableName = "Encaminamientos";
                        CRSourceInf.ReportDocument.SetDataSource(dtst);
                        if (CRSourceInf.ReportDocument.Subreports.Count > 0)
                        {
                            for (int i = 0; i < CRSourceInf.ReportDocument.Subreports.Count; i++)
                            {
                                if (CRSourceInf.ReportDocument.Subreports[i] != null)
                                {
                                    switch (CRSourceInf.ReportDocument.Subreports[i].Name.ToString())
                                    {
                                        case CR_INF_LE_SUBINF_RANGOS:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsRangos != null && dtsRangos.Tables.Count > 0)
                                            {
                                                dtsRangos.Tables[0].TableName = "Rangos";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsRangos.Tables[0]);
                                            }
                                            break;

                                        case CR_INF_LE_SUBINF_SERVIDORESIP:

                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsDatosServidoresIP != null && dtsDatosServidoresIP.Tables.Count > 0)
                                            {
                                                dtsDatosServidoresIP.Tables[0].TableName = "equiposEu_SCVIP";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsDatosServidoresIP.Tables[0]);
                                            }
                                            break;

                                        case CR_INF_LE_SUBINF_RUTAS:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsRutas != null && dtsRutas.Tables.Count > 0)
                                            {
                                                dtsRutas.Tables[0].TableName = "RutasTroncalesRecursos";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsRutas.Tables[0]);
                                            }
                                            break;
                                        case CR_INF_LE_SUBINF_REDES:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsRedes != null && dtsRedes.Tables.Count > 0)
                                            {
                                                dtsRedes.Tables[0].TableName = "RedesRecursos";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsRedes.Tables[0]);
                                            }
                                            break;

                                        case CR_INF_LE_SUBINF_DESTINOSLCEN:
                                            TraducirInforme(CRSourceInf.ReportDocument.Subreports[i], strIdioma);
                                            if (dtsDestinosATS != null && dtsDestinosATS.Tables.Count > 0)
                                            {
                                                dtsDestinosATS.Tables[0].TableName = "DestinosLCENEncaminamientos";
                                                CRSourceInf.ReportDocument.Subreports[i].SetDataSource(dtsDestinosATS.Tables[0]);
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
                    catch (System.Threading.ThreadAbortException ae)
                    { //ThreadException can happen for internale Response implementation
                        if (dtst != null)
                            dtst.Clear();
                        if (dtsRangos != null)
                            dtsRangos.Clear();
                        if (dtsDatosServidoresIP != null)
                            dtsDatosServidoresIP.Clear();
                        if (dtsRutas != null)
                            dtsRutas.Clear();
                        if (dtsRedes != null)
                            dtsRedes.Clear();
                        if (dtsDestinosATS != null)
                            dtsDestinosATS.Clear();
                        logDebugView.Error(string.Format("Error{0}", ae.Message.ToString()));
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
                  strNombreInf = string.Format("InfCnfEncaminamientos_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

              CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, strNombreInf);
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
            strNombreInf = string.Format("InfCnfEncaminamientos_{0}", DateTime.Now.ToString("yyyyMMddHHmm"));

        try
        {
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            CRSourceInf.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, false, strNombreInf);
        }
        catch (System.Threading.ThreadAbortException )
        { //ThreadException can happen for internale Response implementation

        }
        catch (Exception )
        {
          
        }
      }
  }


  protected void BtnExcel_Click(object sender, ImageClickEventArgs e)
  {

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
