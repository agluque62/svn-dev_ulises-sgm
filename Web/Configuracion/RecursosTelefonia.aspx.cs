using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Linq;
using log4net;
using log4net.Config;


public partial class RecursosDeTelefonia : PageBaseCD40.PageCD40	// System.Web.UI.Page
{
    private static ILog _logDebugView;
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
    private static ServiciosCD40.Tablas[] datos;
    private static bool RecursoAsignado = false;
    private static int NumInterfaceAsignado;
    private static int NumDispositivoAsignado;
    private static int[] NumSlotsPorTipoInterface = { 1, 1, 1, 1, 1, 1, 1, 4, 2, 16, 1, 1, 1, 1, 1 };
    //private static DropDownList ddlPrefijos = new DropDownList();
    static bool PermisoSegunPerfil;
    private static ServiciosCD40.ServiciosCD40 ServiceServiciosCD40 = new ServiciosCD40.ServiciosCD40();
    private static int NumPaginaActiva = 0;
    private static uint Prefijo = 0;
    private Ulises5000Configuration.ToolsUlises5000Section UlisesToolsVersion;
    private AsyncCallback CallbackCompletado;

    static bool Modificando;

    private static string strRedActual = string.Empty;
    private static bool bUnicoRecursoEnRed = false;

    const string INTERFACE_LCEN = "1";
    const string INTERFACE_AB = "4";
    const string INTERFACE_ATS_QSIG = "7";
    const string INTERFACE_ISDN_2BD = "8";
    const string INTERFACE_ISDN_30BD = "9";

    //Lista de valores para el tipo equipo (RBLTipoEquipo) de la pestaña Asignación HW
    const string HW_TIPO_PASARELA = "0";
    const string HW_TIPO_EQUIPO_EXTERNO = "1";

    //Posiciones de las pestañas de recursos de telefonía
    private enum enindexTAB { PARAM_GENERAL = 0, PARAM_VOIP = 1, PARAM_AUDIO = 2, PARAM_FUNCIONALIDAD = 3, ASIG_HW = 4 };



    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        cMsg = (Mensajes.msgBox)this.Master.FindControl("MsgBox1");

        if (Context.Request.IsAuthenticated)
        {
            // retrieve user's identity from httpcontext user 
            FormsIdentity ident = (FormsIdentity)Context.User.Identity;
            string perfil = ident.Ticket.UserData;
            if (perfil == "0")
            {
                Response.Redirect("~/Configuracion/Inicio.aspx?Permiso=NO", false);
                return;
            }

            PermisoSegunPerfil = perfil != "1";
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            //Version = config.AppSettings.Settings["Version"];
            Ulises5000Configuration.ToolsUlises5000Section ulisesTools = Ulises5000Configuration.ToolsUlises5000Section.Instance;

            UlisesToolsVersion = ulisesTools;
        }

        if (CallbackCompletado == null)
            CallbackCompletado = new AsyncCallback(OnCallBackCompleted);

        if (!IsPostBack)
        {
            IndexListBox1 = -1;

            // Mostrar grabación ED137 sólo para Nouakchott (Version==1)
            if (UlisesToolsVersion.Tools["GrabacionRecursoTelefonia"] == null)
                CheckGrabacionEd137.Visible = false;

            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement keyElement = config.AppSettings.Settings["Sistema"];
            Session["IdSistema"] = keyElement.Value;

            IBGenerales.CssClass = "buttonImageSelected";
            IBVoip.CssClass = "buttonImage";
            IBAudio.CssClass = "buttonImage";
            IBFuncionalidad.CssClass = "buttonImage";
            IBAsignacion.CssClass = "buttonImage";

            //VMG 12/11/2018
            initDefaultValues();

            /*this.ClientScript.RegisterStartupScript(this.GetType(), 
                "Nucleo", "alertify.error('Nucleo', 'Record Saved Successfully', "+
                "function() {"+
                "});", true);
            */
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            BtNuevo.Visible = PermisoSegunPerfil;
            NumPaginaActiva = 0;

            CargaDDLTifX();
            CargaDDLTroncal();
            CargaDDLRed();
            //CargaDDLDestinos();
            CargaDDLEquiposExternos();
            MuestraDatos(DameDatos());
        }
        else
        {
            //Si se ha recargado la página, las variables datos y la variable de session tienen valor nulo es porque 
            // si ha cambiado la sesión del servidor, bien por conmutación o reinicio
            //por lo que se va a la página de login
            if (datos == null || Session["idsistema"] == null)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);
                return;
            }

            MultiView1.ActiveViewIndex = NumPaginaActiva;

            //if (Request.Form["eliminaelemento"] == "1")//El usuario elige eliminar el elemento 
            //{
            //    Request.Form["eliminaelemento"].Replace("1", "0");

            //    EliminarElemento();
            //    MuestraDatos(DameDatos());
            //}
            //if (Request.Form["cancelparam"] == "1")    //El usuario elige no guardar los cambios 
            //{
            //    Request.Form["cancelparam"].Replace("1", "0");

            //    CancelarCambios();
            //}
            //if (Request.Form["aceptparam"] == "1")     //El usuario elige guardar los cambios
            //{
            //    Request.Form["aceptparam"].Replace("1", "0");

            //    GuardarCambios();
            //}
            if (Request.Form["SoloEliminaDeTFT"] == "1")
            {
                Request.Form["SoloEliminaDeTFT"].Replace("1", "0");

                EliminarElemento();
                EliminaDestinoDeTFT();

                try
                {
                    // Llamada asíncrona para regenerar todas las sectorizaciones.
                    Session.Add("Sectorizando", true);
                    ServiceServiciosCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, false, true, CallbackCompletado, null);
                }
                catch (Exception ex)
                {
                    logDebugView.Error("(RecursosDeRadio-EliminarRecurso): ", ex);
                }
                MuestraDatos(DameDatos());
            }
        }
    }



    protected void BtNuevo_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = NumPaginaActiva = 0;
        if (Panel1.Height != 199)
            Panel1.Height = 199;

        Modificando = false;
        RecursoAsignado = false;
        IndexListBox1 = ListBox1.SelectedIndex;

        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        BtModificar.Visible = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        ListBox1.Enabled = false;
        TxtIdRecurso.Enabled = true;

        DListEquipoExternos.SelectedIndex = 0;

        LDestino.Visible = TBDestino.Visible = false;
        TBDestino.Text = string.Empty;
        DListInterface.Enabled = true;

        HabilitaElementos(true);
        EstadoItemsNuevoRecurso();
        MostrarDdls();

        //VMG  Ocultamos todos los elementos de telefonía 
        // hacemos visibles los elementos de LCEN que es 
        // la opción por defecto e inicializamos todos los 
        // campos a valores por defecto para tenerlos listos en
        // el index change
        OcultarOpcionesTelefonia();

        LBSupervisionOptions.Visible = true;
        DDLSupervisionOptions.Visible = true;
        DDLSupervisionOptions.SelectedIndex = 0;
        LBTmSupervisionOptions.Visible = false;
        TxtTmSupervisionOptions.Visible = false;

        initDefaultValues();
    }

    /// VMG 06/11/2018
    /// <summary>
    /// Inicializa los valores de los campos de 
    ///  funcionalidad de telefonía a sus valores por defecto
    /// </summary>
    private void initDefaultValues()
    {
        TxtTReleaseBL.Text = "3";
        DDLSupervisionOptions.SelectedIndex = 1;//No
        TxtTmSupervisionOptions.Text = "10";
        DDLDeteccionCallerId.SelectedIndex = 1;
        TxtTmDeteccionCallerId.Text = "3000";
        DDLDeteccionInversionPol.SelectedIndex = 0;//Yes only for AB
        TxtTmLlamadaEntrante.Text = "30";
        TxtTmDetFinLlamada.Text = "6";
        TxtPeriodoSpvRing.Text = "200";
        TxtFiltroSpvRing.Text = "2";
        DDLDeteccionDtmf.SelectedIndex = 1;//No
    }

    private void EstadoItemsNuevoRecurso()
    {
        TxtIdRecurso.Text = "";
        //DListTipo.SelectedIndex = 2;
        DListInterface.SelectedIndex = 0;

        if (DListTifx.Items.Count > 0)
            DListTifx.SelectedIndex = 0;

        //MVO.20170714: Al dar de alta un nuevo recurso de telefonía se hace no visible el CheckDiffServ, que no aplica al interfaz LCEN que se selecciona por defecto.
        CheckDiffServ.Checked = false;
        CheckDiffServ.Visible = false;

        //Por defecto, el tipo de equipo es pasarela
        RBLTipoEquipo.SelectedValue = HW_TIPO_PASARELA;

        TxtServidorSIP.Text = "";
        CBSupersionSilencio.Checked = false;
        TBTamanoPaquete.Text = "20";
        RBCodecA.Checked = true;
        RBCodecMu.Checked = false;
        CheckGrabacionEd137.Checked = true;
        RBGanaciaTx.Checked = true;
        RBAGCTx.Checked = false;
        RBGananciaRx.Checked = true;
        RBAGCRx.Checked = false;
        TBGananciaTx.Text = "0";
        TBGananciaRx.Text = "0";

        if (DDLTroncal.Items.Count > 0)
            DDLTroncal.SelectedIndex = 0;
        CargaRecursosDeLaTifx(DListTifx.SelectedValue);

        //Se muestran los campos de sistema HW en función del tipo de equipo seleccionado
        MuestraSistemaHardware(RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA);
        //Se habilitan las pestañas en función del tipo de asignación HW y tipo interfaz
        EnableTabs(RBLTipoEquipo.SelectedValue == HW_TIPO_EQUIPO_EXTERNO, ServiciosCD40.TipoInterface.TI_LCEN);

    }

    //private void CargaDDLDestinos()
    //{
    //    try
    //    {
    //        ServiciosCD40.DestinosExternos dExterno = new ServiciosCD40.DestinosExternos();

    //        dExterno.IdSistema = (string)Session["IdSistema"];
    //        // Como existen prefijos repetidos, no se pueden almacenar en DDLDestinos.DataValueField.
    //        // Los guardamos en la droddownlistbox ddlPrefijos
    //        ddlPrefijos.DataSource = DDLDestino.DataSource = ServiceServiciosCD40.DataSetSelectSQL(dExterno);
    //        DDLDestino.DataTextField = "IdDestino";
    //        ddlPrefijos.DataTextField = "IdPrefijo";

    //        DDLDestino.DataBind();
    //        ddlPrefijos.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        logDebugView.Error("(RecursosDeTelefonia-CargaDDLDestinos): ", ex);
    //    }
    //}

    private void CargaDDLTroncal()
    {
        try
        {
            ServiciosCD40.Troncales troncal = new ServiciosCD40.Troncales();

            troncal.IdSistema = (string)Session["IdSistema"];

            DataSet dsTroncales = ServiceServiciosCD40.DataSetSelectSQL(troncal);
            if (dsTroncales.Tables.Count > 0)
            {
                DDLTroncal.DataSource = dsTroncales;
                DDLTroncal.DataTextField = "IdTroncal";
                DDLTroncal.DataValueField = "IdTroncal";
                DDLTroncal.DataBind();
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-CargaDDLTroncal): ", ex);
        }
    }

    private void CargaDDLRed()
    {
        try
        {
            ServiciosCD40.Redes red = new ServiciosCD40.Redes();

            red.IdSistema = (string)Session["IdSistema"];

            DataSet dsRedes = ServiceServiciosCD40.DataSetSelectSQL(red);
            if (dsRedes.Tables.Count > 0)
            {
                DDLRed.DataSource = dsRedes;
                DDLRed.DataTextField = "IdRed";
                DDLRed.DataValueField = "IdRed";
                DDLRed.DataBind();
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-CargaDDLRed): ", ex);
        }
    }

    private void CargaDDLEquiposExternos()
    {
        ServiciosCD40.EquiposEU objEu = new ServiciosCD40.EquiposEU();
        objEu.IdSistema = (string)Session["IdSistema"];
        objEu.TipoEquipo = (uint)ServiciosCD40.Tipo_Elemento_HW.TEH_EXTERNO_TELEFONIA;

        string strFirstItemRecorder = string.Empty;

        if (null != GetLocalResourceObject("DDLEquipoExternosItem1"))
        {
            strFirstItemRecorder = GetLocalResourceObject("DDLEquipoExternosItem1").ToString();
        }
        else
            strFirstItemRecorder = "< Ninguno >";

        DListEquipoExternos.Items.Clear();
        //DListEquipoExternos.Items.Add("< Ninguno >");
        DListEquipoExternos.Items.Add(strFirstItemRecorder);

        ServiciosCD40.Tablas[] listaEqExt = ServiceServiciosCD40.ListSelectSQL(objEu);

        if (null != listaEqExt && listaEqExt.Length > 0)
        {
            for (int i = 0; i < listaEqExt.Length; i++)
            {
                // Se muestran todos los equipos externos de telefonía 
                // que no pertenecen a una central ATS (encaminaminamiento que es Central IP  que se identifican con Min=Max=-1) 
                // y que no es la central propia (Min=Max=-1 y interno=false))
                if ((((ServiciosCD40.EquiposEU)listaEqExt[i]).Min != -1 && ((ServiciosCD40.EquiposEU)listaEqExt[i]).Max != -1) ||
                    (((ServiciosCD40.EquiposEU)listaEqExt[i]).Min == -1 && ((ServiciosCD40.EquiposEU)listaEqExt[i]).Max == -1) &&
                    ((ServiciosCD40.EquiposEU)listaEqExt[i]).Interno == true)
                    DListEquipoExternos.Items.Add(((ServiciosCD40.EquiposEU)listaEqExt[i]).IdEquipos);

            }
        }
    }

    /// <summary>
    /// VMG 29/10/2018
    /// Mostrar el elemento del ListBox1 (Recursos de telefonía)
    /// así como habilitar las pestañas con los parámetros que 
    /// le corresponden a cada tipo.
    /// </summary>
    private void MostrarElemento()
    {
        BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;

        bool bEquipoExt = false;
        string strMsgEliminar = string.Empty;

        try
        {
            ServiciosCD40.Recursos r = new ServiciosCD40.Recursos();
            for (int i = 0; i < datos.Length; i++)
            {
                r = (ServiciosCD40.Recursos)datos[i];//Hay que buscar el elemento en las tablas de datos ya que se las trae todas
                if (r.IdRecurso == ListBox1.SelectedValue)
                    break;
            }

            if (r != null)
            {
                TxtIdRecurso.Text = r.IdRecurso;
                TxtServidorSIP.Text = r.ServidorSIP;
                //DListTipo.SelectedValue = r.Tipo.ToString();

                DListInterface.SelectedValue = ((int)(r.Interface)).ToString();

                // Cambiar a primera página sólo si estoy en ventana de Funcionalidad y ahora no soy un recurso de LC
                //if (MultiView1.ActiveViewIndex == 3 && r.Interface != ServiciosCD40.TipoInterface.TI_LCEN)
                //RecuperaPaginaGeneral();

                OcultarOpcionesTelefonia();

                if (r.Interface == ServiciosCD40.TipoInterface.TI_LCEN)
                {
                    LBSupervisionOptions.Visible = true;
                    DDLSupervisionOptions.Visible = true;
                    LBTmSupervisionOptions.Visible = true;
                    TxtTmSupervisionOptions.Visible = true;

                    MostrarTTMaxLC();
                }

                MostrarDdls();

                if (r.Interface == ServiciosCD40.TipoInterface.TI_BL)
                {
                    LBTReleaseBL.Visible = true;
                    TxtTReleaseBL.Visible = true;
                    LBTmLlamadaEntrante.Visible = true;
                    TxtTmLlamadaEntrante.Visible = true;
                    //LBPeriodoSpvRing.Visible = true;
                    //TxtPeriodoSpvRing.Visible = true;
                    //LBFiltroSpvRing.Visible = true;
                    //TxtFiltroSpvRing.Visible = true;
                }
                if (r.Interface == ServiciosCD40.TipoInterface.TI_AB)
                {
                    LBTmDetFinLlamada.Visible = true;
                    TxtTmDetFinLlamada.Visible = true;
                    LBTmLlamadaEntrante.Visible = true;
                    TxtTmLlamadaEntrante.Visible = true;
                    //LBPeriodoSpvRing.Visible = true;
                    //TxtPeriodoSpvRing.Visible = true;
                    //LBFiltroSpvRing.Visible = true;
                    //TxtFiltroSpvRing.Visible = true;
                    LBDeteccionInversionPol.Visible = true;
                    DDLDeteccionInversionPol.Visible = true;
                    LBDeteccionCallerId.Visible = true;
                    DDLDeteccionCallerId.Visible = true;
                    LBTmDeteccionCallerId.Visible = true;
                    TxtTmDeteccionCallerId.Visible = true;
                }

                // Tratamiento del check particular según el tipo de recurso:
                if (r.Interface == ServiciosCD40.TipoInterface.TI_BL || r.Interface == ServiciosCD40.TipoInterface.TI_EM_PP)
                {
                    CheckDiffServ.Visible = true;
                    //CheckDiffServ.Text = "4 Hilos";
                    //Se lee la cadena del fichero de recursos 
                    CheckDiffServ.Text = GetLocalResourceObject("CheckDiffServ_4Hilos").ToString();
                }
                else if (r.Interface == ServiciosCD40.TipoInterface.TI_BC)
                {
                    LBTmLlamadaEntrante.Visible = true;
                    TxtTmLlamadaEntrante.Visible = true;
                    LBDeteccionDtmf.Visible = true;
                    DDLDeteccionDtmf.Visible = true;

                    CheckDiffServ.Visible = true;
                    //CheckDiffServ.Text = "BC - AB PP";
                    CheckDiffServ.Text = (string)GetLocalResourceObject("CheckDiffServ_TIBC").ToString();
                }
                else
                    CheckDiffServ.Visible = false;

                CheckDiffServ.Checked = r.Diffserv;

                //En BD el tipoRecurso==1 --> recurso de Telefonía
                //         tipoRecurso==2 --> recurso LCEN
                if (r.TipoRecurso == 1)	// Telefonía
                    CargaDatosTelefonia(r.IdSistema, r.IdRecurso, r.Interface);
                else
                    CargaDatosLineaCaliente(r.IdSistema, r.IdRecurso);

                MuestraSistemaHardware(r.IdTifX != null);


                if (r.IdTifX != null)
                {
                    DListEquipoExternos.SelectedIndex = 0;
                    RBLTipoEquipo.SelectedValue = HW_TIPO_PASARELA;
                    DListTifx.SelectedValue = r.IdTifX;
                    CargaRecursosDeLaTifx(DListTifx.SelectedValue);
                    bEquipoExt = false;

                }
                else if (r.IdEquipo != null)
                {
                    RBLTipoEquipo.SelectedValue = HW_TIPO_EQUIPO_EXTERNO;
                    if ((DListEquipoExternos.Items.Count > 0) && (r.IdEquipo != ""))
                        DListEquipoExternos.SelectedValue = r.IdEquipo;
                    bEquipoExt = true;
                }

                if (DDLRed.Visible && DListInterface.SelectedIndex > 0 && bUnicoRecursoAsignado_Red(DListInterface.SelectedValue, DDLRed.SelectedValue))
                {
                    // Se informa de que es el único recurso para esa red, y se pregunta si desea continuar
                    strMsgEliminar = String.Format((string)GetLocalResourceObject("EliminarUnicoRecursoRedTlf"), DDLRed.SelectedValue, ListBox1.SelectedValue);
                    bUnicoRecursoEnRed = true;
                }
                else
                {
                    strMsgEliminar = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarRecurso"), ListBox1.SelectedValue);
                    bUnicoRecursoEnRed = false;
                }

                BtEliminar_ConfirmButtonExtender.ConfirmText = strMsgEliminar;

                //Se habilitan las pestañas en función del tipo de asignación HW y si el recurso es LCEN
                EnableTabs(bEquipoExt, r.Interface);
            }
            //    }
            //}
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-MostrarElemento): ", ex);
        }
    }

    // VMG 06/11/2018
    /// <summary>
    /// Muestra los elementos de funcionalidad de telefonía
    ///  en función del tipo: LCEN, BC, BL, AB
    /// </summary>
    /// <param name="tipoInterfaz"></param>
    private void MostrarElementosFuncTelefonia(string tipoInterfaz)
    {
        OcultarOpcionesTelefonia();
        switch (tipoInterfaz)
        {
            case "1"://LCEN
                LBSupervisionOptions.Visible = true;
                DDLSupervisionOptions.Visible = true;
                LBTmSupervisionOptions.Visible = true;
                TxtTmSupervisionOptions.Visible = true;
                break;
            case "2"://BC
                LBTmLlamadaEntrante.Visible = true;
                TxtTmLlamadaEntrante.Visible = true;
                LBDeteccionDtmf.Visible = true;
                DDLDeteccionDtmf.Visible = true;
                break;
            case "3"://BL
                LBTReleaseBL.Visible = true;
                TxtTReleaseBL.Visible = true;
                LBTmLlamadaEntrante.Visible = true;
                TxtTmLlamadaEntrante.Visible = true;
                //LBPeriodoSpvRing.Visible = true;
                //TxtPeriodoSpvRing.Visible = true;
                //LBFiltroSpvRing.Visible = true;
                //TxtFiltroSpvRing.Visible = true;
                break;
            case "4"://AB
                LBTmDetFinLlamada.Visible = true;
                TxtTmDetFinLlamada.Visible = true;
                LBTmLlamadaEntrante.Visible = true;
                TxtTmLlamadaEntrante.Visible = true;
                //LBPeriodoSpvRing.Visible = true;
                //TxtPeriodoSpvRing.Visible = true;
                //LBFiltroSpvRing.Visible = true;
                //TxtFiltroSpvRing.Visible = true;
                LBDeteccionInversionPol.Visible = true;
                DDLDeteccionInversionPol.Visible = true;
                LBDeteccionCallerId.Visible = true;
                DDLDeteccionCallerId.Visible = true;
                LBTmDeteccionCallerId.Visible = true;
                TxtTmDeteccionCallerId.Visible = true;
                break;
        }
    }
    /// VMG 06/11/2018
    /// <summary>
    /// Oculta las opciones de funcionalidad de telefonía
    /// </summary>
    private void OcultarOpcionesTelefonia()
    {
        LBTReleaseBL.Visible = false;
        TxtTReleaseBL.Visible = false;
        LBDeteccionInversionPol.Visible = false;
        DDLDeteccionInversionPol.Visible = false;
        LBSupervisionOptions.Visible = false;
        DDLSupervisionOptions.Visible = false;
        LBTmSupervisionOptions.Visible = false;
        TxtTmSupervisionOptions.Visible = false;
        LBTmLlamadaEntrante.Visible = false;
        TxtTmLlamadaEntrante.Visible = false;
        LBPeriodoSpvRing.Visible = false;
        TxtPeriodoSpvRing.Visible = false;
        LBFiltroSpvRing.Visible = false;
        TxtFiltroSpvRing.Visible = false;
        LBDeteccionCallerId.Visible = false;
        DDLDeteccionCallerId.Visible = false;
        LBTmDeteccionCallerId.Visible = false;
        TxtTmDeteccionCallerId.Visible = false;
        LBTmDetFinLlamada.Visible = false;
        TxtTmDetFinLlamada.Visible = false;
        LBDeteccionDtmf.Visible = false;
        DDLDeteccionDtmf.Visible = false;

        Label17.Visible = false;
        TxtT1.Visible = false;
        Label18.Visible = false;
        TxtT1Max.Visible = false;

        Label21.Visible = false;
        TxtT2.Visible = false;
        Label22.Visible = false;
        TxtT2Max.Visible = false;

        Label23.Visible = false;
        TxtT3.Visible = false;

        Label24.Visible = false;
        TxtT4.Visible = false;
        Label25.Visible = false;
        TxtT4Max.Visible = false;

        Label26.Visible = false;
        TxtT5.Visible = false;
        Label27.Visible = false;
        TxtT5Max.Visible = false;

        Label28.Visible = false;
        TxtT6.Visible = false;
        Label29.Visible = false;
        TxtT6Max.Visible = false;

        Label30.Visible = false;
        TxtT8.Visible = false;
        Label31.Visible = false;
        TxtT8Max.Visible = false;

        Label32.Visible = false;
        TxtT9.Visible = false;
        Label33.Visible = false;
        TxtT9Max.Visible = false;

        Label34.Visible = false;
        TxtT10.Visible = false;
        Label35.Visible = false;
        TxtT10Max.Visible = false;

        Label36.Visible = false;
        TxtT11.Visible = false;
        Label37.Visible = false;
        TxtT11Max.Visible = false;

        Label38.Visible = false;
        TxtT12.Visible = false;
    }

    /// VMG 06/11/2018
    /// <summary>
    /// Muestra las opciones T y TMax de Líneas Calientes
    /// </summary>
    private void MostrarTTMaxLC()
    {
        Label17.Visible = true;
        TxtT1.Visible = true;
        Label18.Visible = true;
        TxtT1Max.Visible = true;

        Label21.Visible = true;
        TxtT2.Visible = true;
        Label22.Visible = true;
        TxtT2Max.Visible = true;

        Label23.Visible = true;
        TxtT3.Visible = true;

        Label24.Visible = true;
        TxtT4.Visible = true;
        Label25.Visible = true;
        TxtT4Max.Visible = true;

        Label26.Visible = true;
        TxtT5.Visible = true;
        Label27.Visible = true;
        TxtT5Max.Visible = true;

        Label28.Visible = true;
        TxtT6.Visible = true;
        Label29.Visible = true;
        TxtT6Max.Visible = true;

        Label30.Visible = true;
        TxtT8.Visible = true;
        Label31.Visible = true;
        TxtT8Max.Visible = true;

        Label32.Visible = true;
        TxtT9.Visible = true;
        Label33.Visible = true;
        TxtT9Max.Visible = true;

        Label34.Visible = true;
        TxtT10.Visible = true;
        Label35.Visible = true;
        TxtT10Max.Visible = true;

        Label36.Visible = true;
        TxtT11.Visible = true;
        Label37.Visible = true;
        TxtT11Max.Visible = true;

        Label38.Visible = true;
        TxtT12.Visible = true;
    }
    private void CargaDatosLineaCaliente(string idSistema, string idRecurso)
    {
        ServiciosCD40.RecursosLCEN recTf = new ServiciosCD40.RecursosLCEN();

        recTf.IdSistema = idSistema;
        recTf.IdRecurso = idRecurso;

        ServiciosCD40.Tablas[] lista = ServiceServiciosCD40.ListSelectSQL(recTf);
        if (lista.Length > 0)
        {
            recTf = (ServiciosCD40.RecursosLCEN)lista[0];
            TBTamanoPaquete.Text = recTf.TamRTP.ToString();

            //TBGananciaRx.Text = (recTf.GananciaAGCRXdBm.ToString()).Replace('.', ',');
            //TBGananciaTx.Text = (recTf.GananciaAGCTXdBm.ToString()).Replace('.', ',');
            TBGananciaRx.Text = (recTf.GananciaAGCRXdBm.ToString());
            TBGananciaTx.Text = (recTf.GananciaAGCTXdBm.ToString());

            TxtUmbralPTT.Text = recTf.UmbralTonoPTT.ToString();
            TxtUmbralSQ.Text = recTf.UmbralTonoSQ.ToString();
            CheckGrabacionEd137.Checked = recTf.GrabacionEd137;

            //                    TxtLongRafagas.Text = r.LongRafagas.ToString();

            //VMG 20/02/2019
            DDLSupervisionOptions.SelectedValue = recTf.isuperv_options == 0 ? "0" : "1";            
            TxtTmSupervisionOptions.Text = (recTf.itm_superv_options.ToString());

            LBTmSupervisionOptions.Visible = TxtTmSupervisionOptions.Visible = DDLSupervisionOptions.SelectedIndex == 0 ? true : false;

            if (recTf.IdDestino != null)
            {
                LDestino.Visible = TBDestino.Visible = true;
                TBDestino.Text = recTf.IdDestino;
                //BtEliminar.Enabled = false;
                //BtEliminar.ToolTip = "Si el recurso tiene un destino asociado no se puede eliminar.";
                //DListDestino.Visible = true;
                //ListItem item = DListDestino.Items.FindByText(r.IdDestino);
                //if (item != null)
                //    DListDestino.SelectedIndex = DListDestino.Items.IndexOf(item);
            }
            else
            {
                LDestino.Visible = TBDestino.Visible = false;
                TBDestino.Text = string.Empty;
                //BtEliminar.Enabled = true;
                //BtEliminar.ToolTip = null;

                //TBDestino.Text = "Sin destino asociado";
                //DListDestino.SelectedValue = "-1";	// Ninguno
            }

            TxtT1.Text = recTf.T1.ToString();
            TxtT1Max.Text = recTf.T1Max.ToString();
            TxtT2.Text = recTf.T2.ToString();
            TxtT2Max.Text = recTf.T2Max.ToString();
            TxtT3.Text = recTf.T3.ToString();
            TxtT4.Text = recTf.T4.ToString();
            TxtT4Max.Text = recTf.T4Max.ToString();
            TxtT5.Text = recTf.T5.ToString();
            TxtT5Max.Text = recTf.T5Max.ToString();
            TxtT6.Text = recTf.T6.ToString();
            TxtT6Max.Text = recTf.T6Max.ToString();
            TxtT8.Text = recTf.T8.ToString();
            TxtT8Max.Text = recTf.T8Max.ToString();
            TxtT9.Text = recTf.T9.ToString();
            TxtT9Max.Text = recTf.T9Max.ToString();
            TxtT10.Text = recTf.T10.ToString();
            TxtT10Max.Text = recTf.T10Max.ToString();
            TxtT11.Text = recTf.T11.ToString();
            TxtT11Max.Text = recTf.T11Max.ToString();
            TxtT12.Text = recTf.T12.ToString();
            //TxtRefresco.Text = r.RefrescoEstados.ToString();
            //TxtTimeoutRef.Text = r.Timeout.ToString();

            RBCodecA.Checked = recTf.Codec == 0;
            RBCodecMu.Checked = recTf.Codec != 0;

            //if (recTf.Codec == 0)
            //{
            //    RBCodecA.Checked = true;
            //    RBCodecA.Enabled = true;
            //    RBCodecMu.Enabled = false;
            //    RBCodecMu.Checked = false;
            //}
            //else
            //{
            //    RBCodecMu.Checked = true;
            //    RBCodecMu.Enabled = true;
            //    RBCodecA.Enabled = false;
            //    RBCodecA.Checked = false;
            //}

            RBGananciaRx.Checked = recTf.GananciaAGCRX == 0;
            RBAGCRx.Checked = recTf.GananciaAGCRX != 0;

            //if (recTf.GananciaAGCRX == 0)
            //{
            //    RBGananciaRx.Checked = true;
            //    RBGananciaRx.Enabled = true;
            //    RBAGCRx.Checked = false;
            //    RBAGCRx.Enabled = false;
            //}
            //else
            //{
            //    RBGananciaRx.Checked = false;
            //    RBGananciaRx.Enabled = false;
            //    RBAGCRx.Checked = true;
            //    RBAGCRx.Enabled = true;
            //}

            RBGanaciaTx.Checked = recTf.GananciaAGCTX == 0;
            RBAGCTx.Checked = recTf.GananciaAGCTX != 0;

            //if (recTf.GananciaAGCTX == 0)
            //{
            //    RBGanaciaTx.Checked = true;
            //    RBGanaciaTx.Enabled = true;
            //    RBAGCTx.Checked = false;
            //    RBAGCTx.Enabled = false;
            //}
            //else
            //{
            //    RBGanaciaTx.Checked = false;
            //    RBGanaciaTx.Enabled = false;
            //    RBAGCTx.Checked = true;
            //    RBAGCTx.Enabled = true;
            //}
        }
    }

    private void MuestraSistemaHardware(bool muestraTifx)
    {
        DListTifx.Visible = muestraTifx;
        Label14.Visible = muestraTifx;
        TTifx.Visible = muestraTifx;

        LblEquipoExterno.Visible = !muestraTifx;
        DListEquipoExternos.Visible = !muestraTifx;
    }

    private void CargaDatosTelefonia(string idSistema, string idRecurso, ServiciosCD40.TipoInterface tipoRecurso)
    {
        try
        {
            ServiciosCD40.RecursosTF recTf = new ServiciosCD40.RecursosTF();

            recTf.IdSistema = idSistema;
            recTf.IdRecurso = idRecurso;

            ServiciosCD40.Tablas[] lista = ServiceServiciosCD40.ListSelectSQL(recTf);
            if (lista.Length > 0)
            {
                // Parametros generales
                RBAGCRx.Checked = ((ServiciosCD40.RecursosTF)lista[0]).GananciaAGCRX != 0;
                RBGananciaRx.Checked = ((ServiciosCD40.RecursosTF)lista[0]).GananciaAGCRX == 0;

                RBAGCTx.Checked = ((ServiciosCD40.RecursosTF)lista[0]).GananciaAGCTX != 0;
                RBGanaciaTx.Checked = ((ServiciosCD40.RecursosTF)lista[0]).GananciaAGCTX == 0;

                TBGananciaRx.Text = (((ServiciosCD40.RecursosTF)lista[0]).GananciaAGCRXdBm.ToString());
                TBGananciaTx.Text = (((ServiciosCD40.RecursosTF)lista[0]).GananciaAGCTXdBm.ToString());

                //TBGananciaRx.Text = (((ServiciosCD40.RecursosTF)lista[0]).GananciaAGCRXdBm.ToString()).Replace('.', ',');
                //TBGananciaTx.Text = (((ServiciosCD40.RecursosTF)lista[0]).GananciaAGCTXdBm.ToString()).Replace('.', ',');

                CBSupersionSilencio.Checked = ((ServiciosCD40.RecursosTF)lista[0]).SupresionSilencio;
                TBTamanoPaquete.Text = ((ServiciosCD40.RecursosTF)lista[0]).TamRTP.ToString();


                RBCodecA.Checked = ((ServiciosCD40.RecursosTF)lista[0]).Codec == 0;
                RBCodecMu.Checked = ((ServiciosCD40.RecursosTF)lista[0]).Codec != 0;
                CheckGrabacionEd137.Checked = ((ServiciosCD40.RecursosTF)lista[0]).GrabacionEd137;


                // Parametros de telefonía
                if (((ServiciosCD40.RecursosTF)lista[0]).IdTroncal != null)
                {
                    LDestino.Visible = TBDestino.Visible = false;
                    TBDestino.Text = string.Empty;

                    LTroncal.Visible = true;
                    DDLTroncal.Visible = true;
                    DDLTroncal.SelectedValue = ((ServiciosCD40.RecursosTF)lista[0]).IdTroncal;
                }
                else
                {
                    DDLTroncal.SelectedValue = "0";
                    //LTroncal.Visible = false;
                    //DDLTroncal.Visible = false;
                }

                if (((ServiciosCD40.RecursosTF)lista[0]).IdRed != null)
                {
                    LDestino.Visible = TBDestino.Visible = false;
                    TBDestino.Text = string.Empty;

                    LRed.Visible = true;
                    DDLRed.Visible = true;
                    DDLRed.SelectedValue = ((ServiciosCD40.RecursosTF)lista[0]).IdRed;
                    strRedActual = DDLRed.SelectedValue;
                }
                else
                {
                    LRed.Visible = false;
                    DDLRed.Visible = false;
                }

                if (((ServiciosCD40.RecursosTF)lista[0]).IdDestino != null)
                {
                    LDestino.Visible = true;
                    TBDestino.Visible = true;
                    TBDestino.Text = ((ServiciosCD40.RecursosTF)lista[0]).IdDestino;
                    Prefijo = ((ServiciosCD40.RecursosTF)lista[0]).IdPrefijo;
                    //BtEliminar.Enabled = false; 
                    //BtEliminar.ToolTip = "Si el recurso tiene un destino asociado no se puede eliminar.";
                    //DDLDestino.Visible = true;
                    //ListItem item = DDLDestino.Items.FindByText(((ServiciosCD40.RecursosTF)lista[0]).IdDestino);
                    //if (item != null)
                    //	DDLDestino.SelectedIndex = DDLDestino.Items.IndexOf(item);
                }
                else
                {
                    LDestino.Visible = false;
                    TBDestino.Visible = false;
                    TBDestino.Text = string.Empty;
                    //BtEliminar.Enabled = true;
                    //BtEliminar.ToolTip = null;

                    //TBDestino.Text = "Sin destino asociado";
                    //DDLDestino.SelectedValue = "-1";	// Ninguno
                }

                if (DDLLado.Visible && ((ServiciosCD40.RecursosTF)lista[0]).Lado != null)
                {
                    DDLLado.SelectedValue = ((ServiciosCD40.RecursosTF)lista[0]).Lado;
                }
                if (DDLModo.Visible && ((ServiciosCD40.RecursosTF)lista[0]).Modo != null)
                {
                    DDLModo.SelectedValue = ((ServiciosCD40.RecursosTF)lista[0]).Modo;
                }
                if (DDLTipoEM.Visible)
                    DDLTipoEM.SelectedIndex = (int)((ServiciosCD40.RecursosTF)lista[0]).TipoEM;

                //Cargamos datos de funcionalidad en funcion del tipo de recurso
                //Común para AB, BC y BL
                TxtTmLlamadaEntrante.Text = ((ServiciosCD40.RecursosTF)lista[0]).iTmLlamEntrante.ToString();
                switch (tipoRecurso)
                {
                    case ServiciosCD40.TipoInterface.TI_AB:
                        TxtTmDetFinLlamada.Text = ((ServiciosCD40.RecursosTF)lista[0]).iTmDetFinLlamada.ToString();
                        TxtPeriodoSpvRing.Text = ((ServiciosCD40.RecursosTF)lista[0]).iPeriodoSpvRing.ToString();
                        TxtFiltroSpvRing.Text = ((ServiciosCD40.RecursosTF)lista[0]).iFiltroSpvRing.ToString();
                        DDLDeteccionInversionPol.SelectedValue = ((ServiciosCD40.RecursosTF)lista[0]).iDetInversionPol.ToString();
                        DDLDeteccionCallerId.SelectedValue = ((ServiciosCD40.RecursosTF)lista[0]).iDetCallerId.ToString();
                        TxtTmDeteccionCallerId.Text = ((ServiciosCD40.RecursosTF)lista[0]).iTmCallerId.ToString();
                        if (DDLDeteccionCallerId.SelectedIndex == 0)//Viene de Si(Index=0) a No(Index=1) 
                            LBTmDeteccionCallerId.Visible = TxtTmDeteccionCallerId.Visible = true;
                        else
                            LBTmDeteccionCallerId.Visible = TxtTmDeteccionCallerId.Visible = false;
                        break;
                    case ServiciosCD40.TipoInterface.TI_BC:
                        DDLDeteccionDtmf.SelectedValue = ((ServiciosCD40.RecursosTF)lista[0]).iDetDtmf.ToString();
                        break;
                    case ServiciosCD40.TipoInterface.TI_BL:
                        TxtTReleaseBL.Text = ((ServiciosCD40.RecursosTF)lista[0]).TReleaseBL.ToString();
                        TxtPeriodoSpvRing.Text = ((ServiciosCD40.RecursosTF)lista[0]).iPeriodoSpvRing.ToString();
                        TxtFiltroSpvRing.Text = ((ServiciosCD40.RecursosTF)lista[0]).iFiltroSpvRing.ToString();
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-CargaDatosTelefonia): ", ex);
        }
    }

    private void EsconderMenu()
    {
        LError.Text = "";
        Modificando = false;

        //MostrarDdls();
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
    }

    private void MostrarDdls()
    {
        MuestraDDLLado();
        MuestraDDLModo();
        MuestraDDLRed();
        MuestraDDLTroncal();
    }

    private void HabilitaElementos(bool habilita)
    {
        RBLTipoEquipo.Enabled = habilita && !RecursoAsignado && DListEquipoExternos.SelectedIndex == 0;
        DListEquipoExternos.Enabled = habilita;

        /* DListInterface.Enabled = */
        CheckDiffServ.Enabled = TxtServidorSIP.Enabled = habilita;
        DDLTipoEM.Enabled = habilita;

        /* CBSupersionSilencio.Enabled = TBTamanoPaquete.Enabled = */
        Panel1.Enabled = PanelParametrosRx.Enabled = PanelParametrosTx.Enabled = CheckGrabacionEd137.Enabled = habilita;
        //Panel2.Enabled = habilita;

        DDLRed.Enabled = DDLLado.Enabled = DDLModo.Enabled = DDLTroncal.Enabled = habilita;
        DListTifx.Enabled = habilita && !RecursoAsignado && DListEquipoExternos.SelectedIndex == 0;
        TTifx.Enabled = habilita;

        TxtTReleaseBL.Enabled = habilita;
        DDLDeteccionInversionPol.Enabled = habilita;
        DDLSupervisionOptions.Enabled = habilita;
        TxtTmSupervisionOptions.Enabled = habilita;
        TxtTmLlamadaEntrante.Enabled = habilita;
        TxtPeriodoSpvRing.Enabled = habilita;
        TxtFiltroSpvRing.Enabled = habilita;
        DDLDeteccionCallerId.Enabled = habilita;
        TxtTmDeteccionCallerId.Enabled = habilita;
        TxtTmDetFinLlamada.Enabled = habilita;
        DDLDeteccionDtmf.Enabled = habilita;

        //VMG Movemos el boton de Aceptar porque se ha hecho mas grande el panel
        BtAceptar.Style.Remove("top");
        BtAceptar.Style.Add("top", "455px");

        //VMG 29/10/2018
        if (DDLSupervisionOptions.SelectedIndex == 0 && habilita)
            TxtTmSupervisionOptions.ReadOnly = false;
        else
            TxtTmSupervisionOptions.ReadOnly = true;

        if (DDLDeteccionCallerId.SelectedIndex == 0 && habilita)
            TxtTmDeteccionCallerId.ReadOnly = false;
        else
            TxtTmDeteccionCallerId.ReadOnly = true;

        /* MVO.20170707: los valores nominales de protocolo y los valores máximos de temporización o supervisión no son modificables
		TxtT1.Enabled = habilita;
		TxtT1Max.Enabled = habilita;
		TxtT2.Enabled = habilita;
		TxtT2Max.Enabled = habilita;
		TxtT3.Enabled = habilita;
		TxtT4.Enabled = habilita;
		TxtT4Max.Enabled = habilita;
		TxtT5.Enabled = habilita;
		TxtT5Max.Enabled = habilita;
		TxtT6.Enabled = habilita;
		TxtT6Max.Enabled = habilita;
		TxtT8.Enabled = habilita;
		TxtT8Max.Enabled = habilita;
		TxtT9.Enabled = habilita;
		TxtT9Max.Enabled = habilita;
		TxtT10.Enabled = habilita;
		TxtT10Max.Enabled = habilita;
		TxtT11.Enabled = habilita;
		TxtT11Max.Enabled = habilita;
		TxtT12.Enabled = habilita;
        
         */
    }

    // VMG 08/11/2018
    /// <summary>
    /// Habilita el campo numérico o lo deshabilita en función
    /// de si es SI o No el dropBox del que proceda.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DDLElement_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool readOnly = true;

        if (((DropDownList)sender).SelectedIndex == 0)
            readOnly = false;

        switch (((DropDownList)sender).ID)
        {
            case "DDLSupervisionOptions":
                if (DDLSupervisionOptions.SelectedIndex == 0)//Viene de Si(Index=0) a No(Index=1) 
                {
                    LBTmSupervisionOptions.Visible = TxtTmSupervisionOptions.Visible = true;
                    if (TxtTmSupervisionOptions.Text.Length == 0)
                        TxtTmSupervisionOptions.Text = "10";//Caso raro pero inicializamos por defecto
                }
                else
                    LBTmSupervisionOptions.Visible = TxtTmSupervisionOptions.Visible = false;
                TxtTmSupervisionOptions.ReadOnly = readOnly;
                break;
            case "DDLDeteccionCallerId":
                if (DDLDeteccionCallerId.SelectedIndex == 0)//Viene de Si(Index=0) a No(Index=1) 
                {
                    LBTmDeteccionCallerId.Visible = TxtTmDeteccionCallerId.Visible = true;
                    if (TxtTmDeteccionCallerId.Text.Length == 0)
                        TxtTmDeteccionCallerId.Text = "3000";//Caso raro pero inicializamos por defecto
                }
                else
                    LBTmDeteccionCallerId.Visible = TxtTmDeteccionCallerId.Visible = false;
                TxtTmDeteccionCallerId.ReadOnly = readOnly;
                break;
        }
    }

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            //BtEliminar.Visible = PermisoSegunPerfil;
            ListBox1.DataBind();
            MostrarElemento();
        }
    }

    /// VMG 20/11/2018
    /// <summary>
    /// Filtro de búsqueda para recursos telefonía
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FiltroBusquedaTF_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Reset de todos los elementos
        ArrayList ListBoxArray = new ArrayList();
        FiltroNombreTF.Visible = false;
        FiltroNombreTF.Text = "";
        ButtonFiltroBuscarTF.Visible = false;
        FiltroTipoTF.Visible = false;
        FiltroTipoTF.SelectedIndex = 0;

        switch (FiltroBusquedaTF.SelectedIndex)
        {
            case 1://Todos
                MuestraDatos(DameDatos());
                break;
            case 2://Nombre
                FiltroNombreTF.Visible = true;
                ButtonFiltroBuscarTF.Visible = true;
                break;
            case 3://Tipo
                FiltroTipoTF.Visible = true;
                break;
            case 4://AlfaB Desc
                for (int i = 0; i < ListBox1.Items.Count; i++)
                    ListBoxArray.Add(ListBox1.Items[i].Value);

                ListBox1.Items.Clear();
                ListBoxArray.Sort();
                ListBox1.DataSource = ListBoxArray;
                ListBox1.DataBind();
                break;
            case 5://AlfaB Asc
                for (int i = 0; i < ListBox1.Items.Count; i++)
                    ListBoxArray.Add(ListBox1.Items[i].Value);

                ListBox1.Items.Clear();
                ListBoxArray.Sort(new ReverseSort());
                ListBox1.DataSource = ListBoxArray;
                ListBox1.DataBind();
                break;
        }
    }
    /// VMG 20/11/2018
    /// <summary>
    /// Filtro de búsqueda para recursos telefonía
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FiltroTipoTF_SelectedIndexChanged(object sender, EventArgs e)
    {
        MuestraDatos(DameDatos(), FiltroTipoTF.SelectedIndex);
    }

    /// VMG 20/11/2018
    /// <summary>
    /// Busca los recursos por nombre a través del 'Contains'
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ButtonFiltroBuscarTF_Click(object sender, EventArgs e)
    {
        ListBox1.Items.Clear();

        ServiciosCD40.Tablas[] nu = DameDatos();

        for (int i = 0; i < nu.Length; i++)
        {
            if (((ServiciosCD40.Recursos)nu[i]).TipoRecurso != 0)
            {
                if (((ServiciosCD40.Recursos)nu[i]).IdRecurso.Contains(FiltroNombreTF.Text))
                    ListBox1.Items.Add(((ServiciosCD40.Recursos)nu[i]).IdRecurso);
            }
        }
    }

    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            //ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.Recursos t = new ServiciosCD40.Recursos();
            t.IdSistema = (string)Session["IdSistema"];
            t.TipoRecurso = 255;  // DONT_CARE

            ServiciosCD40.Tablas[] d = ServiceServiciosCD40.ListSelectSQL(t);
            datos = Array.FindAll((ServiciosCD40.Tablas[])d, delegate(ServiciosCD40.Tablas obj) { return ((ServiciosCD40.Recursos)obj).TipoRecurso != 0; });

            return datos;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-DameDatos): ", ex);
        }
        return null;
    }

    ///VMG 22/11/2018
    /// <summary>
    /// Muestra los recursos en el ListBox1
    /// </summary>
    /// <param name="nu"></param>
    /// <param name="tipo">Parametro por defecto para llamar por tipo</param>
    private void MuestraDatos(ServiciosCD40.Tablas[] nu, int tipo = 0)
    {
        ListBox1.Items.Clear();
        ServiciosCD40.TipoInterface tipoBuscar = new ServiciosCD40.TipoInterface();

        //Si viene tipo!=0 hay que buscar por tipo de interfaz
        switch (tipo)
        {
            case 1:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_AB;
                break;
            case 2:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_ATS_N5;
                break;
            case 3:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_ATS_QSIG;
                break;
            case 4:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_ATS_R2;
                break;
            case 5:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_BC;
                break;
            case 6:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_BL;
                break;
            case 7:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_EM_MARC;
                break;
            case 8:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_EM_PP;
                break;
            case 9:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_ISDN_2BD;
                break;
            case 10:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_ISDN_30BD;
                break;
            case 11:
                tipoBuscar = ServiciosCD40.TipoInterface.TI_LCEN;
                break;
        }

        //if (nu!=null)
        for (int i = 0; i < nu.Length; i++)
        {
            if (((ServiciosCD40.Recursos)nu[i]).TipoRecurso != 0)
            {
                if (tipo != 0)
                {//Solo los tipos a buscar
                    if (((ServiciosCD40.Recursos)nu[i]).Interface == tipoBuscar)
                        ListBox1.Items.Add(((ServiciosCD40.Recursos)nu[i]).IdRecurso);
                }
                else//TODOS
                    ListBox1.Items.Add(((ServiciosCD40.Recursos)nu[i]).IdRecurso);
            }
        }

        if (ListBox1.Items.Count > 0)
        {
            ActualizaWebPadre(true);

            BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil;

            if (ListBox1.Items.FindByText(NewItem) != null)
            {
                ListBox1.Items.FindByText(NewItem).Selected = true;
                IndexListBox1 = ListBox1.SelectedIndex;
                NewItem = string.Empty;
            }
            else
                ListBox1.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < ListBox1.Items.Count ? IndexListBox1 : 0;

            MostrarElemento();
        }
        else
        {
            EstadoItemsNuevoRecurso();
            BtModificar.Visible = BtEliminar.Visible = false;
        }
    }


    protected override void CancelarCambios()
    {
        RecursoAsignado = false;

        EsconderMenu();
        HabilitaElementos(false);

        BtNuevo.Visible = PermisoSegunPerfil;
        ListBox1.Enabled = true;
        TxtIdRecurso.Enabled = false;

        MuestraDatos(DameDatos());

        MostrarElemento();
    }

    protected override void AceptarCambios()
    {
        base.AceptarCambios();
    }

    protected void GuardarCambios()
    {
        try
        {
            ServiciosCD40.Recursos n = new ServiciosCD40.Recursos();
            n.IdSistema = (string)Session["idsistema"];
            if (!Modificando) //Recurso nuevo
                n.IdRecurso = TxtIdRecurso.Text;
            else
            {
                n.IdRecurso = ListBox1.SelectedValue;
            }

            NewItem = n.IdRecurso;

            n.TipoRecurso = DListInterface.SelectedValue == "1" ? (uint)2 : (uint)1;  // Telefonía
            n.Tipo = 2; // Audio_RxTx UInt16.Parse(DListTipo.SelectedValue);
            n.Interface = (ServiciosCD40.TipoInterface)(UInt16.Parse(DListInterface.SelectedValue));
            n.Diffserv = CheckDiffServ.Checked;
            n.ServidorSIP = TxtServidorSIP.Text;

            if (RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA)	// TIFX
            {
                n.IdTifX = DListTifx.SelectedItem.Text;
                n.SlotPasarela = NumInterfaceAsignado;
                n.NumDispositivoSlot = NumDispositivoAsignado;
                n.IdEquipo = null;
            }
            else
            {
                n.IdEquipo = DListEquipoExternos.SelectedValue;
                n.IdTifX = null;
            }

            if (!Modificando) //Recurso nuevo
            {
                if (!ExisteElRecurso(n))
                {
                    if (ServiceServiciosCD40.InsertSQL(n) < 0) // Inserta Recursos
                        logDebugView.Warn("(RecursosDeTelefonia-GuardarCambios): No se ha podido insertar el elemento.");
                    else
                    {
                        #region Sincronización con CD30
                        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                        {
                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                            int tipoLinea = 0;
                            int tipoLinTroncal = 0;
                            int lado = 0;
                            int acceso = 0;
                            string elTroncal = "";
                            switch (n.Interface)
                            {
                                case ServiciosCD40.TipoInterface.TI_LCEN:
                                    tipoLinea = 8;
                                    tipoLinTroncal = 8;
                                    lado = 2;
                                    break;
                                case ServiciosCD40.TipoInterface.TI_BC:
                                    tipoLinea = 1;
                                    tipoLinTroncal = 1;
                                    lado = 2;
                                    break;
                                case ServiciosCD40.TipoInterface.TI_BL:
                                    tipoLinea = 4;
                                    tipoLinTroncal = 2;
                                    lado = 2;
                                    break;
                                case ServiciosCD40.TipoInterface.TI_AB:
                                    tipoLinea = 4;
                                    tipoLinTroncal = 4;
                                    elTroncal = DDLRed.SelectedValue;
                                    lado = 2;
                                    break;
                                case ServiciosCD40.TipoInterface.TI_ATS_R2:
                                    tipoLinea = 6;
                                    tipoLinTroncal = 6;
                                    elTroncal = DDLTroncal.SelectedValue;
                                    if (DDLLado.SelectedValue.CompareTo("0") == 0)
                                        lado = 0;
                                    else
                                        lado = 1;
                                    break;
                                case ServiciosCD40.TipoInterface.TI_ATS_N5:
                                    tipoLinea = 6;
                                    tipoLinTroncal = 6;
                                    if (DDLLado.SelectedValue.CompareTo("0") == 0)
                                        lado = 0;
                                    else
                                        lado = 1;
                                    break;
                                case ServiciosCD40.TipoInterface.TI_ATS_QSIG:
                                    tipoLinea = 12;
                                    tipoLinTroncal = 12;
                                    elTroncal = DDLTroncal.SelectedValue;
                                    lado = 0;
                                    //if (DDLLado.SelectedValue.CompareTo("A") == 0)
                                    //    lado = 0;
                                    //else
                                    //    lado = 1;
                                    break;
                                case ServiciosCD40.TipoInterface.TI_ISDN_2BD:
                                    tipoLinea = 10;
                                    lado = 3;
                                    tipoLinTroncal = 10;
                                    acceso = 1;
                                    elTroncal = DDLRed.SelectedValue;
                                    break;
                                case ServiciosCD40.TipoInterface.TI_ISDN_30BD:
                                    tipoLinea = 11;
                                    lado = 3;
                                    tipoLinTroncal = 11;
                                    acceso = 1;
                                    elTroncal = DDLRed.SelectedValue;
                                    break;
                                default:
                                    break;
                            }

                            switch (sincro.AltaLineaTelefonia(n.Interface == ServiciosCD40.TipoInterface.TI_LCEN ? 0 : 1, n.IdRecurso, tipoLinea, tipoLinTroncal, lado, acceso, elTroncal))
                            {
                                case 127:
                                    cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod127"));
                                    break;
                                case 128:
                                    cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod128"));
                                    break;
                                default:
                                    break;
                            }
                        }
                        #endregion
                    }

                    if (n.TipoRecurso == 1)	// Telefonía
                        InsertaParametrosRecursoTf(n);    // Inserta o modifica parámetros del recurso
                    else
                        InsertaParametrosRecursoLc(n);

                    ActualizaWebPadre(true);
                }
                else
                {
                    cMsg.alert((string)GetGlobalResourceObject("Espaniol", "IdRecursoYaExiste"));
                    TxtIdRecurso.Focus();
                    return;
                }
            }
            else
            {
                if (ServiceServiciosCD40.UpdateSQL(n) < 0)	// Modifica el recurso
                    logDebugView.Warn("(RecursosDeTelefonia-GuardarCambios): No se ha podido actualizar el elemento.");
                else
                {
                    //VMG 20/02/2019 Modificar los params del recurso
                    ServiciosCD40.ParametrosRecursoTelefonia pr = new ServiciosCD40.ParametrosRecursoTelefonia();
                    //pr.IdSistemaSCV = n.IdSistema
                    #region Sincronización con CD30
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                    KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                    if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                    {
                        SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                        int tipoLinea = 0;
                        int tipoLinTroncal = 0;
                        int lado = 0;
                        int acceso = 0;
                        string elTroncal = "";
                        switch (n.Interface)
                        {
                            case ServiciosCD40.TipoInterface.TI_BC:
                                tipoLinea = 1;
                                tipoLinTroncal = 1;
                                lado = 2;
                                break;
                            case ServiciosCD40.TipoInterface.TI_BL:
                                tipoLinea = 4;
                                tipoLinTroncal = 2;
                                lado = 2;
                                break;
                            case ServiciosCD40.TipoInterface.TI_AB:
                                tipoLinea = 4;
                                tipoLinTroncal = 4;
                                elTroncal = DDLRed.SelectedValue;
                                lado = 2;
                                break;
                            case ServiciosCD40.TipoInterface.TI_ATS_R2:
                                tipoLinea = 6;
                                tipoLinTroncal = 6;
                                elTroncal = DDLTroncal.SelectedValue;
                                if (DDLLado.SelectedValue.CompareTo("0") == 0)
                                    lado = 0;
                                else
                                    lado = 1;
                                break;
                            case ServiciosCD40.TipoInterface.TI_ATS_N5:
                                tipoLinea = 6;
                                tipoLinTroncal = 6;
                                if (DDLLado.SelectedValue.CompareTo("0") == 0)
                                    lado = 0;
                                else
                                    lado = 1;
                                break;
                            case ServiciosCD40.TipoInterface.TI_ATS_QSIG:
                                tipoLinea = 12;
                                tipoLinTroncal = 12;
                                elTroncal = DDLTroncal.SelectedValue;
                                lado = 0;
                                //if (DDLLado.SelectedValue.CompareTo("A") == 0)
                                //    lado = 0;
                                //else
                                //    lado = 1;
                                break;
                            case ServiciosCD40.TipoInterface.TI_ISDN_2BD:
                                tipoLinea = 10;
                                lado = 3;
                                tipoLinTroncal = 10;
                                acceso = 1;
                                elTroncal = DDLRed.SelectedValue;
                                break;
                            case ServiciosCD40.TipoInterface.TI_ISDN_30BD:
                                tipoLinea = 11;
                                lado = 3;
                                tipoLinTroncal = 11;
                                acceso = 1;
                                elTroncal = DDLRed.SelectedValue;
                                break;
                            default:
                                break;
                        }

                        switch (sincro.ModificacionLineaTelefonia(1, n.IdRecurso, tipoLinea, tipoLinTroncal, lado, acceso, elTroncal))
                        {
                            case 127:
                                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod127"));
                                break;
                            case 128:
                                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod128"));
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion
                }
                if (n.TipoRecurso == 1)	// Telefonía
                    InsertaParametrosRecursoTf(n);    // Inserta o modifica parámetros del recurso
                else
                    InsertaParametrosRecursoLc(n);
            }

            //Nos situamos en la pestaña General
            NumPaginaActiva = MultiView1.ActiveViewIndex = 0;
            Panel1.Height = 199;
            EnableTabs(RBLTipoEquipo.SelectedValue == HW_TIPO_EQUIPO_EXTERNO, n.Interface);
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-GuardarCambios): ", ex);
        }

        Modificando = false;
        RecursoAsignado = false;

        MuestraDatos(DameDatos());

        EsconderMenu();
        HabilitaElementos(false);

        TxtIdRecurso.Enabled = false;
        ListBox1.Enabled = true;
        BtNuevo.Visible = PermisoSegunPerfil;
        BtEliminar.Visible = BtModificar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;

        //RecuperaPaginaGeneral();

    }

    private void RecuperaPaginaGeneral()
    {
        IBGenerales.CssClass = "buttonImageSelected";
        IBVoip.CssClass = "buttonImage";
        IBAudio.CssClass = "buttonImage";
        IBFuncionalidad.CssClass = "buttonImage";
        IBAsignacion.CssClass = "buttonImage";
        MultiView1.ActiveViewIndex = NumPaginaActiva = 0;
        Panel1.Height = 199;
        MultiView1.ActiveViewIndex = 0;
    }

    private bool ExisteElRecurso(ServiciosCD40.Recursos rTel)
    {
        ServiciosCD40.Recursos r = new ServiciosCD40.Recursos();
        r.IdSistema = rTel.IdSistema;
        r.IdRecurso = rTel.IdRecurso;
        r.TipoRecurso = 255;	//	DONT_CARE	
        ServiciosCD40.Tablas[] lista = ServiceServiciosCD40.ListSelectSQL(r);
        return lista.Length > 0;
    }

    private void InsertaParametrosRecursoTf(ServiciosCD40.Recursos n)
    {
        try
        {
            ServiciosCD40.RecursosTF prTel = new ServiciosCD40.RecursosTF();
            prTel.IdSistema = n.IdSistema;
            prTel.IdRecurso = n.IdRecurso;
            // TipoRecurso viene implícito en la clase a TipoRecurso_TR_TELEFONIA
            prTel.GananciaAGCTX = (uint)(RBGanaciaTx.Checked ? 0 : 1);
            prTel.GananciaAGCTXdBm = Convert.ToSingle(TBGananciaTx.Text);
            prTel.GananciaAGCRX = (uint)(RBGananciaRx.Checked ? 0 : 1);
            prTel.GananciaAGCRXdBm = Convert.ToSingle(TBGananciaRx.Text);
            prTel.SupresionSilencio = CBSupersionSilencio.Checked;
            prTel.TamRTP = Convert.ToUInt32(TBTamanoPaquete.Text);
            prTel.Codec = (uint)(RBCodecA.Checked ? 0 : 1);
            prTel.GrabacionEd137 = CheckGrabacionEd137.Checked;

            prTel.Lado = DDLLado.SelectedValue;
            prTel.Modo = DListInterface.SelectedValue == "7" ? DDLModo.SelectedValue : null;	// Sólo QSIG

            prTel.tipoInterface = n.Interface;
            //switch (DListInterface.SelectedValue)
            switch (n.Interface)
            {
                case ServiciosCD40.TipoInterface.TI_BC:
                    prTel.IdDestino = TBDestino.Text != string.Empty ? TBDestino.Text : null;
                    prTel.TipoDestino = 1;
                    prTel.IdPrefijo = Prefijo;
                    //Usamos iPrTmLlamEntrante porque en el WS (clase recursosTF) ya hay un iTmLlamEntrante 
                    // que enmascara estos de ParametrosRecursos, por tanto los diferenciamos en esta llamada.
                    // Lo mismo con los de mas abajo.
                    prTel.iPrTmLlamEntrante = Convert.ToUInt32(TxtTmLlamadaEntrante.Text);
                    prTel.iPrDetDtmf = Convert.ToByte(DDLDeteccionDtmf.SelectedValue);
                    break;
                case ServiciosCD40.TipoInterface.TI_BL:
                    prTel.IdDestino = TBDestino.Text != string.Empty ? TBDestino.Text : null;
                    prTel.TipoDestino = 1;
                    prTel.IdPrefijo = Prefijo;
                    prTel.TPrReleaseBL = Convert.ToUInt32(TxtTReleaseBL.Text);
                    prTel.iPrTmLlamEntrante = Convert.ToUInt32(TxtTmLlamadaEntrante.Text);
                    prTel.iPrPeriodoSpvRing = Convert.ToUInt32(TxtPeriodoSpvRing.Text);
                    prTel.iPrFiltroSpvRing = Convert.ToUInt32(TxtFiltroSpvRing.Text);
                    break;
                case ServiciosCD40.TipoInterface.TI_AB:
                    prTel.iPrTmDetFinLlamada = Convert.ToUInt32(TxtTmDetFinLlamada.Text);
                    prTel.iPrTmLlamEntrante = Convert.ToUInt32(TxtTmLlamadaEntrante.Text);
                    prTel.iPrPeriodoSpvRing = Convert.ToUInt32(TxtPeriodoSpvRing.Text);
                    prTel.iPrFiltroSpvRing = Convert.ToUInt32(TxtFiltroSpvRing.Text);
                    prTel.iPrDetInversionPol = Convert.ToByte(DDLDeteccionInversionPol.SelectedValue);
                    prTel.iPrDetCallerId = Convert.ToByte(DDLDeteccionCallerId.SelectedValue);
                    prTel.iPrTmCallerId = Convert.ToUInt32(TxtTmDeteccionCallerId.Text);
                    prTel.IdRed = DDLRed.SelectedValue;
                    break;
                case ServiciosCD40.TipoInterface.TI_ISDN_2BD:
                case ServiciosCD40.TipoInterface.TI_ISDN_30BD:
                    prTel.IdRed = DDLRed.SelectedValue;
                    break;
                case ServiciosCD40.TipoInterface.TI_ATS_R2:
                case ServiciosCD40.TipoInterface.TI_ATS_N5:
                case ServiciosCD40.TipoInterface.TI_ATS_QSIG:
                    prTel.IdTroncal = DDLTroncal.SelectedValue == "0" ? null : DDLTroncal.SelectedValue;
                    break;
                case ServiciosCD40.TipoInterface.TI_EM_PP:
                case ServiciosCD40.TipoInterface.TI_EM_MARC:
                    prTel.Lado = DDLLado.SelectedValue;
                    prTel.TipoEM = (ServiciosCD40.Tipo_EM)DDLTipoEM.SelectedIndex;
                    prTel.Modo = DDLModo.SelectedValue;
                    prTel.IdDestino = TBDestino.Text != string.Empty ? TBDestino.Text : null;
                    prTel.TipoDestino = 1;
                    prTel.IdPrefijo = Prefijo;
                    break;
            }

            if (!Modificando) //Recurso nuevo
            {
                if (ServiceServiciosCD40.InsertSQL(prTel) < 0)   // Inserta Recurso y ParametrosRecurso 
                    logDebugView.Warn("(RecursosDeTelefonia-InsertaParametrosRecurso): No se ha podido insertar el elemento.");
            }
            else
            {
                int numRows = ServiceServiciosCD40.UpdateSQL(prTel);
                if (numRows < 0)	// Modifica Recurso y ParametrosRecurso 
                    logDebugView.Warn("(RecursosDeTelefonia-InsertaParametrosRecurso): No se ha podido actualizar el elemento.");
                else if (numRows == 0)	// No se ha podido modificar (¿?) pues se inserta
                    ServiceServiciosCD40.InsertSQL(prTel);
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-InsertaParametrosRecurso): ", ex);
        }
    }

    private void InsertaParametrosRecursoLc(ServiciosCD40.Recursos n)
    {
        try
        {
            ServiciosCD40.RecursosLCEN prTfn = new ServiciosCD40.RecursosLCEN();
            prTfn.IdSistema = n.IdSistema;
            prTfn.IdRecurso = n.IdRecurso;
            // TipoRecurso viene implícito en la clase a TipoRecurso_TR_TELEFONIA
            prTfn.GananciaAGCTX = (uint)(RBGanaciaTx.Checked ? 0 : 1);
            prTfn.GananciaAGCTXdBm = Convert.ToSingle(TBGananciaTx.Text);
            prTfn.GananciaAGCRX = (uint)(RBGananciaRx.Checked ? 0 : 1);
            prTfn.GananciaAGCRXdBm = Convert.ToSingle(TBGananciaRx.Text);
            prTfn.SupresionSilencio = CBSupersionSilencio.Checked;
            prTfn.TamRTP = Convert.ToUInt32(TBTamanoPaquete.Text);
            prTfn.Codec = (uint)(RBCodecA.Checked ? 0 : 1);
            prTfn.UmbralTonoPTT = Int32.Parse(TxtUmbralPTT.Text);
            prTfn.UmbralTonoSQ = Int32.Parse(TxtUmbralSQ.Text);
            prTfn.GrabacionEd137 = CheckGrabacionEd137.Checked;
            
            prTfn.isuperv_options = DDLSupervisionOptions.SelectedValue=="1"?(uint)1:(uint)0;
            prTfn.itm_superv_options = Convert.ToUInt16(TxtTmSupervisionOptions.Text);

            //VMG 21/03/2019 
            //Error en el rango del tiempo entre 0 y 120 o 0.
            if (Convert.ToUInt16(TxtTmSupervisionOptions.Text) < 0 || Convert.ToUInt16(TxtTmSupervisionOptions.Text) > 20)
            {
                string strMsgAux = string.Format((string)GetGlobalResourceObject("Espaniol", "ToptSpvColl"));
                cMsg.confirm(strMsgAux, "aceptparam");
                return;
            }

            //prRad.LongRafagas = UInt16.Parse(TxtLongRafagas.Text);
            //if (DListDestino.SelectedValue != "-1")
            //{
            //    prRad.IdDestino = DListDestino.SelectedItem.Text;
            //prRad.TipoDestino = 0;
            //}
            if (TBDestino.Text != string.Empty)
            {
                prTfn.IdDestino = TBDestino.Text;
                prTfn.TipoDestino = 1;
                prTfn.IdPrefijo = 1;
            }
            else
            {
                prTfn.IdDestino = null;
            }

            prTfn.T1 = UInt16.Parse(TxtT1.Text);
            prTfn.T1Max = UInt16.Parse(TxtT1Max.Text);
            prTfn.T2 = UInt16.Parse(TxtT2.Text);
            prTfn.T2Max = UInt16.Parse(TxtT2Max.Text);
            prTfn.T3 = UInt16.Parse(TxtT3.Text);
            prTfn.T4 = UInt16.Parse(TxtT4.Text);
            prTfn.T4Max = UInt16.Parse(TxtT4Max.Text);
            prTfn.T5 = UInt16.Parse(TxtT5.Text);
            prTfn.T5Max = UInt16.Parse(TxtT5Max.Text);
            prTfn.T6 = UInt16.Parse(TxtT6.Text);
            prTfn.T6Max = UInt16.Parse(TxtT6Max.Text);
            prTfn.T8 = UInt16.Parse(TxtT8.Text);
            prTfn.T8Max = UInt16.Parse(TxtT8Max.Text);
            prTfn.T9 = UInt16.Parse(TxtT9.Text);
            prTfn.T9Max = UInt16.Parse(TxtT9Max.Text);
            prTfn.T10 = UInt16.Parse(TxtT10.Text);
            prTfn.T10Max = UInt16.Parse(TxtT10Max.Text);
            prTfn.T11 = UInt16.Parse(TxtT11.Text);
            prTfn.T11Max = UInt16.Parse(TxtT11Max.Text);
            prTfn.T12 = UInt16.Parse(TxtT12.Text);

            //prRad.RefrescoEstados = UInt16.Parse(TxtRefresco.Text);
            //prRad.Timeout = UInt16.Parse(TxtTimeoutRef.Text);

            // Inserta ParametrosRecurso y Recursos
            if (!Modificando) //Recurso nuevo
            {
                if (ServiceServiciosCD40.InsertSQL(prTfn) < 0) // Inserta Recursos           
                    logDebugView.Warn("(RecursosLCEN-InsertaParametrosRecurso): No se han podido insertar los parametros del recurso.");
            }
            else
            {
                int numRows = ServiceServiciosCD40.UpdateSQL(prTfn);
                if (numRows < 0)	// Modifica Recurso y ParametrosRecurso 
                    logDebugView.Warn("(RecursosLCEN-InsertaParametrosRecurso): No se ha podido actualizar el elemento.");
                else if (numRows == 0)	// No se ha podido modificar (¿?) pues se inserta
                    ServiceServiciosCD40.InsertSQL(prTfn);
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosLCEN-InsertaParametrosRecurso): ", ex);
        }
    }


    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        FiltroBusquedaTF.SelectedIndex = 0;
        FiltroNombreTF.Visible = ButtonFiltroBuscarTF.Visible = FiltroTipoTF.Visible = false;
        if (bUnicoRecursoEnRed && Modificando)
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");

        CancelarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        FiltroBusquedaTF.SelectedIndex = 0;
        FiltroNombreTF.Visible = ButtonFiltroBuscarTF.Visible = FiltroTipoTF.Visible = false;
        LError.Text = "";

        string StrSistema = (string)Session["idsistema"];

        //VMG Control de errores de valores
        if (CheckErrorRangeRT())
            return;

        if (!Modificando && TxtIdRecurso.Enabled && bIdentificadorAsignado(StrSistema, TxtIdRecurso.Text))
        {
            //MVO: Si se está insertando un recurso radio y existe otro recurso con el mismo identificador en el sistema, se informa al usuario para que indique otro identificador
            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorRecurso_Existente"), TxtIdRecurso));
        }
        else if ((HW_TIPO_PASARELA == RBLTipoEquipo.SelectedValue && RecursoAsignado) || (HW_TIPO_EQUIPO_EXTERNO == RBLTipoEquipo.SelectedValue && DListEquipoExternos.SelectedIndex != 0))
        {
            //Si el recurso está asignado a una pasarela y en algún slot o si está asignado a un equipo externo
            if ((DListInterface.SelectedValue != "4" && // AB
                 DListInterface.SelectedValue != "8" && // 2B+D
                 DListInterface.SelectedValue != "9") ||  // 30B+D
                 (DDLRed.SelectedIndex != 0))
            {
                if (bUnicoRecursoEnRed && Modificando)
                {
                    //Se actualiza el mensaje del botón aceptar al valor por defecto
                    BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
                }

                GuardarCambios();

            }
            else
                LError.Text = string.Format((string)GetGlobalResourceObject("Espaniol", "ErrorRecursoSinRed"), DListInterface.SelectedItem.Text);
        }
        else
            LError.Text = (string)GetGlobalResourceObject("Espaniol", "AvisoRecurso");

    }

    private bool CheckErrorRangeRT()
    {
        string msg = null;
        if (TxtTmSupervisionOptions.Text.Length == 0)
        {
            msg = (string)GetGlobalResourceObject("Espaniol", "ErrorTmSupervisionOptionsValue");
            this.ClientScript.RegisterStartupScript(this.GetType(),
            "NucleoWarning", "var msg = alertify.error('Default message'); " +
            "msg.delay(6).setContent('" + msg + "');", true);
            return true;
        }
        if (Convert.ToInt16(TxtTmSupervisionOptions.Text) < 1 || Convert.ToInt16(TxtTmSupervisionOptions.Text) > 20)
        {
            msg = (string)GetGlobalResourceObject("Espaniol", "ErrorTmSupervisionOptionsRange");
            this.ClientScript.RegisterStartupScript(this.GetType(),
                "NucleoWarning", "var msg = alertify.error('Default message'); " +
                "msg.delay(6).setContent('" + msg + "');", true);
            return true;
        }
        return false;
    }

    protected void BtModificar_Click(object sender, EventArgs e)
    {
        Modificando = true;
        ListBox1.Enabled = false;
        IndexListBox1 = ListBox1.SelectedIndex;


        BtModificar.Visible = BtEliminar.Visible = BtNuevo.Visible = false;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        TxtServidorSIP.ReadOnly = false;
        TxtIdRecurso.Enabled = false;
        DListInterface.Enabled = false;

        HabilitaElementos(true);

        CargaRecursosDeLaTifx(DListTifx.Text);

    }

    private void MuestraDDLModo()
    {
        LblModo.Visible = DDLModo.Visible = DListInterface.SelectedValue == "7" ||		// QSIG
                                            DListInterface.SelectedValue == "13" ||     // E&M PP
                                            DListInterface.SelectedValue == "14";       // E&M Marcación

        LblTipoEM.Visible = DDLTipoEM.Visible = DListInterface.SelectedValue == "13" ||     // E&M PP
                                            DListInterface.SelectedValue == "14";       // E&M Marcación

        if (DListInterface.SelectedValue == "13" ||     // E&M
            DListInterface.SelectedValue == "14")
        {
            DDLModo.Items[0].Text = GetLocalResourceObject("ListItemResource30.Text").ToString();
            DDLModo.Items[0].Value = GetLocalResourceObject("ListItemResource30.Value").ToString();
            DDLModo.Items[1].Text = GetLocalResourceObject("ListItemResource31.Text").ToString();
            DDLModo.Items[1].Value = GetLocalResourceObject("ListItemResource31.Value").ToString();
        }
        else
        {
            DDLModo.Items[0].Text = GetLocalResourceObject("ListItemResource1.Text").ToString();
            DDLModo.Items[0].Value = GetLocalResourceObject("ListItemResource1.Value").ToString();
            DDLModo.Items[1].Text = GetLocalResourceObject("ListItemResource2.Text").ToString();
            DDLModo.Items[1].Value = GetLocalResourceObject("ListItemResource2.Value").ToString();
        }
    }

    private void MuestraDDLLado()
    {
        DDLLado.Visible = (DListInterface.SelectedItem.Text == "ATS-R2") ||
                            (DListInterface.SelectedItem.Text == "ATS-N5") ||
                            (DListInterface.SelectedItem.Text == "ATS-QSIG");

        DDLLado.Visible |= (DListInterface.SelectedValue == "13") ||
                         (DListInterface.SelectedValue == "14");    // Tipo E&M

        LLado.Visible = DDLLado.Visible;

        if (DDLLado.Visible && (DListInterface.SelectedValue == "13" || DListInterface.SelectedValue == "14"))
        {
            DDLLado.Items[0].Text = GetLocalResourceObject("ListItemResource28.Text").ToString();
            DDLLado.Items[0].Value = GetLocalResourceObject("ListItemResource28.Value").ToString();
            DDLLado.Items[1].Text = GetLocalResourceObject("ListItemResource29.Text").ToString();
            DDLLado.Items[1].Value = GetLocalResourceObject("ListItemResource29.Value").ToString();
        }
        else
        {
            DDLLado.Items[0].Text = GetLocalResourceObject("ListItemResource15.Text").ToString();
            DDLLado.Items[0].Value = GetLocalResourceObject("ListItemResource15.Value").ToString();
            DDLLado.Items[1].Text = GetLocalResourceObject("ListItemResource16.Text").ToString();
            DDLLado.Items[1].Value = GetLocalResourceObject("ListItemResource16.Value").ToString();
        }
    }

    //private void MuestraDDLDestinos()
    //{
    //    // Sólo se muestra con BC y BL
    //    switch (DListInterface.SelectedValue)
    //    {
    //        case "2":
    //        case "3":
    //            LDestino.Visible = true;
    //            DDLDestino.Visible = true;
    //            break;
    //        default:
    //            LDestino.Visible = false;
    //            DDLDestino.Visible = false;
    //            break;
    //    }
    //}

    private void MuestraDDLRed()
    {
        // Sólo se muestra con AB e ISDN
        switch (DListInterface.SelectedValue)
        {
            case "4":
            case "8":
            case "9":
            case "51":  // E&M Marcacion
                LRed.Visible = true;
                DDLRed.Visible = true;
                LDestino.Visible = TBDestino.Visible = false;
                TBDestino.Text = string.Empty;

                DDLRed.SelectedIndex = 0;
                DDLTroncal.SelectedIndex = 0;
                break;
            default:
                LRed.Visible = false;
                DDLRed.Visible = false;
                break;
        }
    }

    private void MuestraDDLTroncal()
    {
        // Sólo se muestra con ATS
        switch (DListInterface.SelectedValue)
        {
            case "5":
            case "6":
            case "7":
                LTroncal.Visible = true;
                DDLTroncal.Visible = true;
                LDestino.Visible = TBDestino.Visible = false;
                TBDestino.Text = string.Empty;

                DDLRed.SelectedIndex = 0;
                break;
            default:
                LTroncal.Visible = false;
                DDLTroncal.Visible = false;
                break;
        }
    }

    private bool DestinoLCENAsignadoPanelLC(string strSistema,string strIdRecurso, string strDestino,uint idPrefijo, ref System.Text.StringBuilder strMsg)
    {
        //Devuelve true si el destino LCEN está asignado a algún destino en el panel de LC
        //En principio, sólo puede estar asignado a destinos ATS
        bool bExiste = false;
        try
        {
            ServiciosCD40.DestinosExternosSector dExtSec = new ServiciosCD40.DestinosExternosSector();
            dExtSec.IdSistema = strSistema;
            dExtSec.IdDestinoLCEN = strDestino;
            dExtSec.IdPrefijoDestinoLCEN = idPrefijo;
            dExtSec.TipoAcceso = "IA";       //Panel de LC

            ServiciosCD40.Tablas[] lista = ServiceServiciosCD40.ListSelectSQL(dExtSec);

            if (lista != null && lista.Length > 0)
            {

                bExiste = true;
                string strDestinoATS= ((ServiciosCD40.DestinosExternosSector)lista[0]).IdDestino;
                string strSector= ((ServiciosCD40.DestinosExternosSector)lista[0]).IdSector;

                //Componemos el mensaje  a mostrar en pantalla
                System.Text.StringBuilder strMsgFormato= new System.Text.StringBuilder();

                if (GetGlobalResourceObject("Espaniol", "RecursoDestinoLCENAsignadoEnPanelLC") != null)
                    strMsgFormato.Append((string)GetGlobalResourceObject("Espaniol", "RecursoDestinoLCENAsignadoEnPanelLC"));
                else
                {
                    strMsgFormato.Append("El recurso {3} está asignado al destino de línea caliente externa {0}, que está configurado en el destino ATS '{1}' del panel de línea ");
                    strMsgFormato.Append("caliente del sector {2}.\nPor favor, libere el destino del sector antes de eliminar el recurso.");
                }

                if (strMsg.Length>0)
                    strMsg.Clear();

                strMsg.AppendFormat(strMsgFormato.ToString(), strDestino, strDestinoATS, strSector,strIdRecurso);
                strMsgFormato.Clear();
            }

        }
        catch (Exception)
        {
        }

        return bExiste;
    }

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        string strSistema=string.Empty;
        System.Text.StringBuilder strMsg = new System.Text.StringBuilder();

        strSistema=(string)Session["idsistema"];

        EsconderMenu();

        if (ListBox1.SelectedValue != "")
        {
            IndexListBox1 = ListBox1.SelectedIndex;
            Session["elemento"] = ListBox1.SelectedValue;
            if (TBDestino.Text != string.Empty && DListInterface.SelectedValue == INTERFACE_LCEN && DestinoLCENAsignadoPanelLC(strSistema,TxtIdRecurso.Text, TBDestino.Text, 1, ref strMsg))
            {
                //El recurso está asignado a un destino LCEN (idPrefijo=1).
                //Se comprueba si el destino esta asociado a un destino ATS que esté 
                //configurado en algún sector
                cMsg.alert(strMsg.ToString());
                return;
            }
            else if (!string.IsNullOrEmpty(TBDestino.Text) && DestinoAsignadoATft(strSistema,TBDestino.Text)) // El recurso está asignado a un destino que, a su vez, está asignado a algún panel
            {
                // Es el único recurso para ese destino 
                cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "PanelesConDestinoAsignado"), "SoloEliminaDeTFT");
            }
            else
            {
                EliminarElemento();
                MuestraDatos(DameDatos());
                //string texto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarRecurso"), ListBox1.SelectedValue);
                //cMsg.confirm(texto, "eliminaelemento");
            }
        }
    }

    private bool DestinoAsignadoATft(string strSistema, string destino)
    {
        ServiciosCD40.DestinosExternosSector drs = new ServiciosCD40.DestinosExternosSector();
        drs.IdSistema = strSistema;

        //Si el recurso es de tipo LCEN, el destino puede estar asignado a algún destino ATS que esté 
        //configurado en algún sector
        if (DListInterface.SelectedValue == INTERFACE_LCEN)
        {
            drs.IdPrefijoDestinoLCEN = 1;
            drs.IdDestinoLCEN = destino;
        }
        else
        {
            drs.IdDestino = destino;
        }

        ServiciosCD40.Tablas[] lista = ServiceServiciosCD40.ListSelectSQL(drs);

        if (lista != null && lista.Length > 0)
            return true;

        return false;
    }

    private void EliminaDestinoDeTFT()
    {
        ServiciosCD40.Destinos drs = new ServiciosCD40.Destinos();
        drs.IdSistema = (string)Session["idsistema"];
        drs.IdDestino = TBDestino.Text;
        drs.TipoDestino = DListInterface.SelectedValue == "1" ? (uint)2 : (uint)1;

        // Eliminar de todos los TFT el destino
        ServiceServiciosCD40.DeleteSQL(drs);
    }

    private void EliminarElemento()
    {
        try
        {
            ServiciosCD40.Recursos n = new ServiciosCD40.Recursos();
            n.IdSistema = (string)Session["idsistema"];
            n.IdRecurso = (string)Session["elemento"];
            n.TipoRecurso = DListInterface.SelectedValue == "1" ? (uint)2 : (uint)1;

            if (ServiceServiciosCD40.DeleteSQL(n) < 0)
                logDebugView.Warn("(RecursosDeTelefonia-EliminarElemento): No se ha podido eliminar el elemento.");
            else
            {
                #region Sincroniza CD30
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    int tipoLinea = 0;
                    int tipoLinTroncal = 0;
                    string elTroncal = "";
                    n.Interface = (ServiciosCD40.TipoInterface)(UInt16.Parse(DListInterface.SelectedValue));
                    switch (n.Interface)
                    {
                        case ServiciosCD40.TipoInterface.TI_LCEN:
                            tipoLinea = 8;
                            tipoLinTroncal = 8;
                            elTroncal = n.IdRecurso;
                            break;
                        case ServiciosCD40.TipoInterface.TI_BC:
                            tipoLinea = 1;
                            tipoLinTroncal = 1;
                            elTroncal = n.IdRecurso;
                            break;
                        case ServiciosCD40.TipoInterface.TI_BL:
                            tipoLinea = 4;
                            tipoLinTroncal = 2;
                            elTroncal = n.IdRecurso;
                            break;
                        case ServiciosCD40.TipoInterface.TI_AB:
                            tipoLinea = 4;
                            tipoLinTroncal = 4;
                            elTroncal = DDLRed.SelectedValue;
                            break;
                        case ServiciosCD40.TipoInterface.TI_ATS_R2:
                            tipoLinea = 6;
                            tipoLinTroncal = 6;
                            elTroncal = DDLTroncal.SelectedValue;
                            break;
                        case ServiciosCD40.TipoInterface.TI_ATS_N5:
                            tipoLinea = 6;
                            tipoLinTroncal = 6;
                            break;
                        case ServiciosCD40.TipoInterface.TI_ATS_QSIG:
                            tipoLinea = 12;
                            tipoLinTroncal = 12;
                            elTroncal = DDLTroncal.SelectedValue;
                            break;
                        case ServiciosCD40.TipoInterface.TI_ISDN_2BD:
                            tipoLinea = 10;
                            tipoLinTroncal = 10;
                            elTroncal = DDLRed.SelectedValue;
                            break;
                        case ServiciosCD40.TipoInterface.TI_ISDN_30BD:
                            tipoLinea = 11;
                            tipoLinTroncal = 11;
                            elTroncal = DDLRed.SelectedValue;
                            break;
                        default:
                            break;
                    }

                    switch (sincro.BajaLineaTelefonia(n.IdRecurso, tipoLinea, tipoLinTroncal, elTroncal))
                    {
                        case 127:
                            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod127"));
                            break;
                        case 128:
                            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod128"));
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                // Eliminar el destino asociado si lo hubiera
                if (TBDestino.Text != string.Empty)
                {
                    ServiciosCD40.Destinos d = new ServiciosCD40.Destinos();
                    d.IdSistema = n.IdSistema;
                    d.IdDestino = TBDestino.Text;
                    d.TipoDestino = 1;
                    ServiceServiciosCD40.DeleteSQL(d);
                }
                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-EliminarElemento): ", ex);
        }

    }

    //VMG 06/11/2018 Index Change
    protected void DListInterface_SelectedIndexChanged(object sender, EventArgs e)
    {
        MuestraDDLTroncal();
        MuestraDDLRed();
        //MuestraDDLDestinos();
        MuestraDDLLado();
        MuestraDDLModo();

        // Tratamiento del check particular según el tipo de recurso:
        if (((DropDownList)sender).SelectedValue == ((int)ServiciosCD40.TipoInterface.TI_BL).ToString() ||
            ((DropDownList)sender).SelectedValue == ((int)ServiciosCD40.TipoInterface.TI_EM_PP).ToString())
        {
            CheckDiffServ.Visible = true;
            //CheckDiffServ.Text = "4 Hilos";
            CheckDiffServ.Text = GetLocalResourceObject("CheckDiffServ_4Hilos").ToString();
        }
        else if (((DropDownList)sender).SelectedValue == ((int)ServiciosCD40.TipoInterface.TI_BC).ToString())
        {
            CheckDiffServ.Visible = true;
            //CheckDiffServ.Text = "BC - AB PP";
            CheckDiffServ.Text = GetLocalResourceObject("CheckDiffServ_TIBC").ToString();
        }
        else
            CheckDiffServ.Visible = false;

        //MVO: La pestaña funcionalidad sólo está activa para interfaces LCEN y pasarelas
        if (((DropDownList)sender).SelectedValue == Convert.ToString((int)ServiciosCD40.TipoInterface.TI_LCEN) && (RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA))
        {
            if (!IBFuncionalidad.Enabled)
            {
                IBFuncionalidad.Enabled = true;
                IBFuncionalidad.CssClass = "buttonImage";
            }
        }
        //VMG 05/11/2018 Activar funcionalidad de manera cutre
        else if (((DropDownList)sender).SelectedValue == Convert.ToString((int)ServiciosCD40.TipoInterface.TI_BC) ||
            ((DropDownList)sender).SelectedValue == Convert.ToString((int)ServiciosCD40.TipoInterface.TI_AB) ||
            ((DropDownList)sender).SelectedValue == Convert.ToString((int)ServiciosCD40.TipoInterface.TI_BL))
        {
            if (!IBFuncionalidad.Enabled)
            {
                IBFuncionalidad.Enabled = true;
                IBFuncionalidad.CssClass = "buttonImage";
            }
        }
        else
        {
            //Se deshabilita la pestaña Funcionalidad 
            if (IBFuncionalidad.Enabled)
            {
                IBFuncionalidad.Enabled = false;
                IBFuncionalidad.CssClass = "buttonImageDisabled";
            }
        }

        MostrarElementosFuncTelefonia(((DropDownList)sender).SelectedValue);
    }

    protected void DDLTifx_SelectedIndexChanged(object sender, EventArgs e)
    {
        CargaRecursosDeLaTifx(DListTifx.SelectedItem.Text);
    }

    private void CargaDDLTifX()
    {
        try
        {
            ServiciosCD40.TifX tifx = new ServiciosCD40.TifX();
            tifx.IdSistema = (string)Session["idsistema"];

            DListTifx.Items.Clear();
            DataSet dsTifx = ServiceServiciosCD40.DataSetSelectSQL(tifx);

            if (dsTifx.Tables.Count > 0)
            {
                DListTifx.DataSource = dsTifx;
                DListTifx.DataTextField = "IdTifX";
                DListTifx.DataValueField = "IdTifx";
                DListTifx.DataBind();
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-CargaDDLTifX): ", ex);
        }
        if (DListTifx.SelectedItem != null)
            CargaRecursosDeLaTifx(DListTifx.SelectedItem.Text);
    }

    private void CargaRecursosDeLaTifx(string idTifx)
    {
        try
        {
            System.Drawing.Color[] colores ={System.Drawing.Color.BlueViolet,
                                            System.Drawing.Color.Firebrick,
                                            System.Drawing.Color.ForestGreen,
                                            System.Drawing.Color.ForestGreen,
                                            System.Drawing.Color.ForestGreen,
                                            System.Drawing.Color.ForestGreen,
                                            System.Drawing.Color.ForestGreen,
                                            System.Drawing.Color.SteelBlue,
                                            System.Drawing.Color.Salmon,
                                            System.Drawing.Color.Chocolate,
                                            System.Drawing.Color.ForestGreen,
                                            System.Drawing.Color.ForestGreen,
                                            System.Drawing.Color.ForestGreen,
                                            System.Drawing.Color.GreenYellow,
                                            System.Drawing.Color.GreenYellow
                                            };

            // Limpiar contenido tabla tifx
            LimpiaTablaTifx();

            ServiciosCD40.Recursos recurso = new ServiciosCD40.Recursos();
            recurso.IdSistema = (string)Session["IdSistema"];
            recurso.IdTifX = idTifx;

            ServiciosCD40.Tablas[] listaRecursosEnTifX = ServiceServiciosCD40.ListSelectSQL(recurso);

            for (int i = 0; i < listaRecursosEnTifX.Length; i++)
            {
                int slot = ((ServiciosCD40.Recursos)listaRecursosEnTifX[i]).SlotPasarela;
                int dispositivo = ((ServiciosCD40.Recursos)listaRecursosEnTifX[i]).NumDispositivoSlot;
                TextBox tbSlot = (TextBox)(TTifx.Rows[dispositivo].Cells[slot].FindControl("TextBox" + (dispositivo + 1) + (slot + 1)));
                CheckBox cbSlot = (CheckBox)(TTifx.Rows[dispositivo].Cells[slot].FindControl("CheckBox" + (dispositivo + 1) + (slot + 1)));
                if (tbSlot != null && cbSlot != null)
                {
                    for (int j = 0; j < NumSlotsPorTipoInterface[(int)((ServiciosCD40.Recursos)listaRecursosEnTifX[i]).Interface]; j++)
                    {
                        int fila = dispositivo + (j % 4);
                        int col = slot + (j / 4);
                        tbSlot = (TextBox)(TTifx.FindControl("TextBox" + (fila + 1) + (col + 1)));
                        cbSlot = (CheckBox)(TTifx.FindControl("CheckBox" + (fila + 1) + (col + 1)));
                        if (tbSlot != null && cbSlot != null)
                        {
                            // Eliminar la clase 'textbox' para que muestre los colores según el tipo de recurso
                            tbSlot.CssClass = tbSlot.CssClass.Replace("textbox", "");

                            if (Modificando && TxtIdRecurso.Text == ((ServiciosCD40.Recursos)listaRecursosEnTifX[i]).IdRecurso)
                            {
                                tbSlot.Visible = false;
                                cbSlot.Text = TxtIdRecurso.Text;
                                cbSlot.Visible = true;
                                cbSlot.Checked = true;
                                RecursoAsignado = true;
                                NumInterfaceAsignado = slot;
                                NumDispositivoAsignado = dispositivo;
                            }
                            else
                            {
                                cbSlot.Visible = false;
                                tbSlot.Visible = true;
                                tbSlot.Font.Bold = TxtIdRecurso.Text == ((ServiciosCD40.Recursos)listaRecursosEnTifX[i]).IdRecurso;
                                tbSlot.ForeColor = TxtIdRecurso.Text == ((ServiciosCD40.Recursos)listaRecursosEnTifX[i]).IdRecurso ? System.Drawing.Color.Black : System.Drawing.Color.White;
                                tbSlot.Text = ((ServiciosCD40.Recursos)listaRecursosEnTifX[i]).IdRecurso;
                                tbSlot.BackColor = colores[(int)((ServiciosCD40.Recursos)listaRecursosEnTifX[i]).Interface];
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-CargaRecursosDeLaTifx): ", ex);
        }
    }

    private void LimpiaTablaTifx()
    {
        for (int filas = 0; filas < 4; filas++)
        {
            for (int colum = 0; colum < 4; colum++)
            {
                TextBox tbSlot = (TextBox)(TTifx.Rows[filas].Cells[colum].FindControl("TextBox" + (filas + 1) + (colum + 1)));
                CheckBox cbSlot = (CheckBox)(TTifx.Rows[filas].Cells[colum].FindControl("CheckBox" + (filas + 1) + (colum + 1)));
                if (tbSlot != null && cbSlot != null)
                {
                    cbSlot.Text = "";
                    cbSlot.Visible = true;
                    cbSlot.Checked = false;
                    tbSlot.Visible = false;
                    tbSlot.Text = "";
                    tbSlot.BackColor = System.Drawing.Color.White;
                }
            }
        }
    }

    protected void Checked_CBSlot(object sender, EventArgs e)
    {
        bool asignar = true;

        int row = Convert.ToInt32(((CheckBox)sender).ID.Substring(8, 1)) - 1;
        int cell = Convert.ToInt32(((CheckBox)sender).ID.Substring(9, 1)) - 1;

        // Si el recurso ya está asignado no se puede seleccionar otra interface
        // hasta no liberar la ya seleccionada
        if ((((CheckBox)sender).Checked && RecursoAsignado) || DListEquipoExternos.SelectedIndex != 0)
        {
            ((CheckBox)sender).Checked = false;
            return;
        }

        switch (DListInterface.SelectedValue)
        {
            case INTERFACE_ATS_QSIG:
                row = 0;
                //cell = (cell == 3) ? 2 : cell;
                asignar = EsPosibleAsignarQSig(row, cell);
                if (asignar)
                    AsignaSlots(row, cell, NumSlotsPorTipoInterface[7], ((CheckBox)sender).Checked);
                else
                    ((CheckBox)sender).Checked = !((CheckBox)sender).Checked;
                break;
            case INTERFACE_ISDN_2BD:
                row = row & 2;  // Simular que se ha seleccionado la fila 0 o la fila 2
                asignar = EsPosibleAsignar2BD(row, cell);
                if (asignar)
                    AsignaSlots(row, cell, NumSlotsPorTipoInterface[8], ((CheckBox)sender).Checked);
                else
                    ((CheckBox)sender).Checked = !((CheckBox)sender).Checked;
                break;
            case INTERFACE_ISDN_30BD:
                row = cell = 0;
                asignar = EsPosibleAsignar30BD();
                if (asignar)
                    AsignaSlots(0, 0, NumSlotsPorTipoInterface[9], ((CheckBox)sender).Checked);
                else
                    ((CheckBox)sender).Checked = !((CheckBox)sender).Checked;
                break;
            default:    // RESTO
                asignar = EsPosibleAsignarGenerico(cell);
                if (!asignar)
                    ((CheckBox)sender).Checked = !((CheckBox)sender).Checked;
                break;
        }

        RecursoAsignado = ((CheckBox)sender).Checked;
        RBLTipoEquipo.Enabled = DListTifx.Enabled = !RecursoAsignado;

        if (asignar && RecursoAsignado)
        {
            NumInterfaceAsignado = cell;
            NumDispositivoAsignado = row;
        }
        else
        {
            NumDispositivoAsignado = NumInterfaceAsignado = -1;
        }

        // No dejar cambiar de tifx mientras se está en proceso de asignación de un recurso a una GateWay
        DListTifx.Enabled = !RecursoAsignado;
    }

    private bool EsPosibleAsignarGenerico(int cell)
    {
        try
        {
            ServiciosCD40.Recursos unRecurso = new ServiciosCD40.Recursos();
            unRecurso.IdSistema = (string)Session["IdSistema"];
            unRecurso.TipoRecurso = 255; // DONT_CARE

            for (int i = 0; i < 4; i++)
            {
                TextBox tbSlot = (TextBox)TTifx.Rows[2].Cells[cell].FindControl("TextBox" + (i + 1) + (cell + 1));
                if (tbSlot != null && tbSlot.Visible)
                {
                    unRecurso.IdRecurso = tbSlot.Text;
                    ServiciosCD40.Tablas[] listaRecurso = ServiceServiciosCD40.ListSelectSQL(unRecurso);
                    if (listaRecurso.Length > 0)
                        if (((ServiciosCD40.Recursos)listaRecurso[0]).Interface == ServiciosCD40.TipoInterface.TI_ISDN_2BD)
                            return false;
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-EsPosibleAsignarGenerico): ", ex);
        }
        return true;
    }

    private bool EsPosibleAsignar30BD()
    {
        bool continuar = true;

        for (int i = 0; continuar && i < 4; i++)
        {
            for (int j = 0; continuar && j < 4; j++)
            {
                TextBox tbSlot = (TextBox)TTifx.Rows[j].Cells[i].FindControl("TextBox" + (j + 1) + (i + 1));
                continuar = (tbSlot != null && !tbSlot.Visible);
            }
        }

        return continuar;
    }

    private bool EsPosibleAsignar2BD(int row, int cell)
    {
        try
        {
            bool continuar = true;
            int cuantos = 0;
            ServiciosCD40.Recursos unRecurso = new ServiciosCD40.Recursos();
            unRecurso.IdSistema = (string)Session["IdSistema"];
            unRecurso.TipoRecurso = 255; // DONT_CARE

            for (int i = 0; i < 4; i++)
            {
                TextBox tbSlot = (TextBox)TTifx.Rows[2].Cells[cell].FindControl("TextBox" + (i + 1) + (cell + 1));
                if (tbSlot != null && tbSlot.Visible)
                {
                    unRecurso.IdRecurso = tbSlot.Text;
                    ServiciosCD40.Tablas[] listaRecurso = ServiceServiciosCD40.ListSelectSQL(unRecurso);
                    if (listaRecurso.Length > 0)
                        if (((ServiciosCD40.Recursos)listaRecurso[0]).Interface != ServiciosCD40.TipoInterface.TI_ISDN_2BD)
                            return false;
                }
            }

            // Si está libre el slot o es 2B+D
            for (int j = row; continuar && cuantos < 2 && (j < 4); j++)
            {
                TextBox tbSlot = (TextBox)TTifx.Rows[j].Cells[cell].FindControl("TextBox" + (j + 1) + (cell + 1));
                continuar = (tbSlot != null && !tbSlot.Visible);
                cuantos++;
            }
            return continuar && cuantos == 2;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeTelefonia-EsPosibleAsignar2BD): ", ex);
        }
        return false;
    }

    private void AsignaSlots(int row, int cell, int cuantos, bool asigna)
    {
        for (int i = cell; cuantos > 0 && i < 4; i++)
        {
            for (int j = row; cuantos > 0 && j < 4; j++)
            {
                CheckBox cbSlot = (CheckBox)TTifx.Rows[j].Cells[i].FindControl("CheckBox" + (j + 1) + (i + 1));
                cbSlot.Checked = asigna;
                cuantos--;
            }
            row = 0;     // Si se empieza una nueva columna, se empieza desde cero.
        }
    }

    private bool EsPosibleAsignarQSig(int row, int col)
    {
        bool continuar = true;
        int cuantos = 0;

        for (int i = col; continuar && cuantos < NumSlotsPorTipoInterface[7] && i < 4 && i < col + 2; i++)
        {
            for (int j = row; continuar && cuantos < NumSlotsPorTipoInterface[7] && (j < 4); j++)
            {
                TextBox tbSlot = (TextBox)TTifx.Rows[j].Cells[i].FindControl("TextBox" + (j + 1) + (i + 1));
                if (tbSlot != null)
                    continuar = (tbSlot != null && !tbSlot.Visible);
                cuantos++;
            }
        }

        return continuar && cuantos == NumSlotsPorTipoInterface[7];
    }

    protected void RBLTipoEquipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA)	// TIFX
        {
            Label14.Visible = true;
            DListTifx.Visible = DListTifx.Enabled = true;
            TTifx.Visible = true;
            LblEquipoExterno.Visible = false;
            DListEquipoExternos.Visible = false;

            //Si es de tipo LCEN, se habilita el campo funcionalidad
            if (DListInterface.SelectedValue == "1" || DListInterface.SelectedValue == "2" || 
                DListInterface.SelectedValue == "3" || DListInterface.SelectedValue == "4")
            {
                if (!IBFuncionalidad.Enabled)
                {
                    IBFuncionalidad.Enabled = true;
                    IBFuncionalidad.CssClass = "buttonImage";
                }
            }
            else
            {
                if (IBFuncionalidad.Enabled)
                {
                    IBFuncionalidad.Enabled = false;
                    IBFuncionalidad.CssClass = "buttonImageDisabled";
                }
            }

            //Se habilitan las pestañas VoIP y Audio
            if (!IBVoip.Enabled)
            {
                IBVoip.Enabled = true;
                IBVoip.CssClass = "buttonImage";
            }
            if (!IBAudio.Enabled)
            {
                IBAudio.Enabled = true;
                IBAudio.CssClass = "buttonImage";
            }

        }
        else
        {
            //Equipo Externo
            Label14.Visible = false;
            DListTifx.Visible = false;
            TTifx.Visible = false;
            LblEquipoExterno.Visible = true;
            DListEquipoExternos.Visible = true;

            //Se deshabilitan las pestañas las pestañas VoIP, Audio y Funcionalidad 
            if (IBVoip.Enabled)
            {
                IBVoip.Enabled = false;
                IBVoip.CssClass = "buttonImageDisabled";
            }
            if (IBAudio.Enabled)
            {
                IBAudio.Enabled = false;
                IBAudio.CssClass = "buttonImageDisabled";
            }

            if (IBFuncionalidad.Enabled)
            {
                IBFuncionalidad.Enabled = false;
                IBFuncionalidad.CssClass = "buttonImageDisabled";
            }
        }
    }


    protected void DDLEquipoExternos_SelectedIndexChanged(object sender, EventArgs e)
    {
        RBLTipoEquipo.Enabled = DListEquipoExternos.SelectedIndex == 0;
    }

    protected void OnIBRecursosLCEN_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("RecursosLCEN.aspx", false);
    }

    protected void OnButton_Click(object sender, EventArgs e)
    {
        Button ibSelected = (Button)sender;
        //VMG 05/11/2018 Seleccion de las pestañas 
        string strValorTipoInterfazLCEN = ((int)ServiciosCD40.TipoInterface.TI_LCEN).ToString();
        string strValorTipoInterfazBC = ((int)ServiciosCD40.TipoInterface.TI_BC).ToString();
        
        switch (ibSelected.ID)
        {
            case "IBGenerales":
                IBGenerales.CssClass = "buttonImageSelected";

                if (RBLTipoEquipo.SelectedValue == HW_TIPO_EQUIPO_EXTERNO)
                {
                    //Se cambia la imagen de los botones de las pestañas VoIP, Audio y funcionalidad a deshabilitado
                    IBVoip.CssClass = "buttonImageDisabled";
                    IBAudio.CssClass = "buttonImageDisabled";
                    IBFuncionalidad.CssClass = "buttonImageDisabled";
                }
                else
                {
                    ////Se cambia la imagen de los botones de las pestañas VoIP, Audio y si la interfaz es NO LCEN la pestaña funcionalidad a habilitado
                    //Se habilita la pestaña VoIP, Audio y si la interfaz es LCEN funcionalidad
                    IBVoip.CssClass = "buttonImage";
                    IBAudio.CssClass = "buttonImage";
                    //VMG 05/11/2018 Podría hacerse mejor
                    if (DListInterface.SelectedValue == "2" || 
                        DListInterface.SelectedValue == "3" || DListInterface.SelectedValue == "4")
                        IBFuncionalidad.CssClass = "buttonImage";
                    else
                        IBFuncionalidad.CssClass = "buttonImageDisabled";
                }

                IBAsignacion.CssClass = "buttonImage";

                MultiView1.ActiveViewIndex = NumPaginaActiva = 0;
                Panel1.Height = 199;

                //MostrarDdls();
                break;
            case "IBVoip":
                IBGenerales.CssClass = "buttonImage";
                IBVoip.CssClass = "buttonImageSelected";
                IBAudio.CssClass = "buttonImage";
                //Si el tipo interfaz es NO LCEN y está asignado a una pasarela se cambia la imagen del botón funcionalidad a habilitado
                if ((DListInterface.SelectedValue == "2" || 
                    DListInterface.SelectedValue == "3" || DListInterface.SelectedValue == "4") 
                    && RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA)
                    IBFuncionalidad.CssClass = "buttonImage";
                else
                    IBFuncionalidad.CssClass = "buttonImageDisabled";

                IBAsignacion.CssClass = "buttonImage";
                MultiView1.ActiveViewIndex = NumPaginaActiva = 1;
                Panel1.Height = 260;
                break;
            case "IBAudio":
                IBGenerales.CssClass = "buttonImage";
                IBVoip.CssClass = "buttonImage";
                IBAudio.CssClass = "buttonImageSelected";
                //Si el tipo interfaz es NO LCEN y está asignado a una pasarela se muestra la imagen del botón como habilitado
                if ((DListInterface.SelectedValue == "2" || 
                    DListInterface.SelectedValue == "3" || DListInterface.SelectedValue == "4") 
                    && RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA)
                    IBFuncionalidad.CssClass = "buttonImage";
                else
                    IBFuncionalidad.CssClass = "buttonImageDisabled";
                IBAsignacion.CssClass = "buttonImage";
                MultiView1.ActiveViewIndex = NumPaginaActiva = 2;
                Panel1.Height = 200;
                break;
            case "IBFuncionalidad":
                IBGenerales.CssClass = "buttonImage";
                if ((DListInterface.SelectedValue == "2" || 
                    DListInterface.SelectedValue == "3" || DListInterface.SelectedValue == "4" )
                    && RBLTipoEquipo.SelectedValue == HW_TIPO_EQUIPO_EXTERNO)
                {
                    IBVoip.CssClass = "buttonImageDisabled";
                    IBAudio.CssClass = "buttonImageDisabled";                    
                }
                else
                {
                    IBVoip.CssClass = "buttonImage";
                    IBAudio.CssClass = "buttonImage";

                }
                IBFuncionalidad.CssClass = "buttonImageSelected";
                IBAsignacion.CssClass = "buttonImage";
                MultiView1.ActiveViewIndex = NumPaginaActiva = 3;
                Panel1.Height = 260;
                break;
            case "IBAsignacion":
                IBGenerales.CssClass = "buttonImage";

                if (RBLTipoEquipo.SelectedValue == HW_TIPO_EQUIPO_EXTERNO)
                {
                    //Se deshabilita la pestaña VoIP, Audio y funcionalidad
                    IBVoip.CssClass = "buttonImageDisabled";
                    IBAudio.CssClass = "buttonImageDisabled";
                    IBFuncionalidad.CssClass = "buttonImageDisabled";
                }
                else
                {
                    //Se muestran las imagenes de los botones VoIP, Audio y si la interfaz es LCEN funcionalidad como habilitado
                    IBVoip.CssClass = "buttonImage";
                    IBAudio.CssClass = "buttonImage";
                    if (DListInterface.SelectedValue == "2" || 
                        DListInterface.SelectedValue == "3" || DListInterface.SelectedValue == "4")
                        IBFuncionalidad.CssClass = "buttonImage";
                    else
                        IBFuncionalidad.CssClass = "buttonImageDisabled";
                }

                IBAsignacion.CssClass = "buttonImageSelected";
                MultiView1.ActiveViewIndex = NumPaginaActiva = 4;
                Panel1.Height = 260;
                break;
        }
    }

    // Respuesta a la regeneración de las sectorizaciones
    private void OnCallBackCompleted(IAsyncResult result)
    {
        try
        {
            int retorno = ServiceServiciosCD40.EndRegeneraSectorizaciones(result);
            Session.Add("Sectorizando", false);
        }
        catch (System.Web.Services.Protocols.SoapException soapException)
        {
            logDebugView.Error("(RecursosRadio-OnCallBackCompleted): ", soapException);
        }
    }

    //Habilita las pestañas en función del tipo de recursos y de la asignación HW a un equipo externo o pasarela
    private void EnableTabs(bool bEquipoExterno, ServiciosCD40.TipoInterface tipoInterface)
    {
        //Si el recurso está asignado a un equipo externo, las pestañas VoIP, Audio y Funcionalidad tienen que estar deshabilitadas
        //Si el recurso está asignado a una pasarela, las pestañas VoIP y  Audio deben estar habilitadas. La pestaña Funcionalidad sólo si el recurso es de tipo LCEN

        enindexTAB iPestActiva = (enindexTAB)MultiView1.ActiveViewIndex;

        if (bEquipoExterno)
        {
            IBVoip.Enabled = false;
            IBAudio.Enabled = false;
            IBFuncionalidad.Enabled = false;

            //Se cambia la imagen de los botones para que aparezcan inactivos
            if (IBVoip.CssClass != "buttonImageDisabled")
                IBVoip.CssClass = "buttonImageDisabled";

            if (IBAudio.CssClass != "buttonImageDisabled")
                IBAudio.CssClass = "buttonImageDisabled";

            if (IBFuncionalidad.CssClass != "buttonImageDisabled")
                IBFuncionalidad.CssClass = "buttonImageDisabled";

            switch (iPestActiva)
            {
                case enindexTAB.PARAM_GENERAL:
                case enindexTAB.PARAM_VOIP:
                case enindexTAB.PARAM_AUDIO:
                case enindexTAB.PARAM_FUNCIONALIDAD:
                    //Nos situamos en la pestaña General
                    IBGenerales.CssClass = "buttonImageSelected";
                    IBAsignacion.CssClass = "buttonImage";
                    MultiView1.ActiveViewIndex = NumPaginaActiva = 0;
                    Panel1.Height = 199;
                    break;
                case enindexTAB.ASIG_HW:
                    IBAsignacion.CssClass = "buttonImageSelected";
                    IBGenerales.CssClass = "buttonImage";
                    break;
                default:
                    break;
            }
        }
        else
        {
            //Asignación a una pasarela 
            IBVoip.Enabled = IBAudio.Enabled = true;
            //Las pestaña Funcionalidad sólo aparece habilitada para los recursos de tipo LCEN

            switch (tipoInterface)
            {
                case ServiciosCD40.TipoInterface.TI_BC:
                case ServiciosCD40.TipoInterface.TI_BL:
                case ServiciosCD40.TipoInterface.TI_AB:
                    IBFuncionalidad.Enabled = true;
                    if (enindexTAB.PARAM_FUNCIONALIDAD == iPestActiva)
                    {
                        IBFuncionalidad.CssClass = "buttonImageSelected";
                    }
                    else
                        IBFuncionalidad.CssClass = "buttonImage";
                    break;
                default:
                    IBFuncionalidad.Enabled = false;
                    IBFuncionalidad.CssClass = "buttonImageDisabled";
                    if (enindexTAB.PARAM_FUNCIONALIDAD == iPestActiva)
                    {
                        iPestActiva = enindexTAB.PARAM_GENERAL;
                        MultiView1.ActiveViewIndex = (int)iPestActiva;
                        NumPaginaActiva = MultiView1.ActiveViewIndex;
                        Panel1.Height = 199;
                    }
                    break;
            }

            //Dependiendo de la pestaña que esté activa
            switch (iPestActiva)
            {
                case enindexTAB.PARAM_GENERAL:
                    IBGenerales.CssClass = "buttonImageSelected";
                    IBVoip.CssClass = "buttonImage";
                    IBAudio.CssClass = "buttonImage";
                    IBAsignacion.CssClass = "buttonImage";
                    break;
                case enindexTAB.PARAM_VOIP:
                    IBGenerales.CssClass = "buttonImage";
                    IBVoip.CssClass = "buttonImageSelected";
                    IBAudio.CssClass = "buttonImage";
                    IBAsignacion.CssClass = "buttonImage";
                    break;

                case enindexTAB.PARAM_AUDIO:
                    IBGenerales.CssClass = "buttonImage";
                    IBVoip.CssClass = "buttonImage";
                    IBAudio.CssClass = "buttonImageSelected";
                    IBAsignacion.CssClass = "buttonImage";
                    break;
                case enindexTAB.PARAM_FUNCIONALIDAD:
                    IBGenerales.CssClass = "buttonImage";
                    IBVoip.CssClass = "buttonImage";
                    IBAudio.CssClass = "buttonImage";
                    IBAsignacion.CssClass = "buttonImage";
                    break;
                case enindexTAB.ASIG_HW:
                    IBGenerales.CssClass = "buttonImage";
                    IBVoip.CssClass = "buttonImage";
                    IBAudio.CssClass = "buttonImage";
                    IBAsignacion.CssClass = "buttonImageSelected";
                    break;
                default:
                    break;
            }

        }


    }

    //Devuelve true el recurso de telefonía está asignado a una red que sólo tiene configurado ese recurso
    // En caso contrario, devuelve false
    private bool bUnicoRecursoAsignado_Red(string strTipo, string strRed)
    {
        bool bUnicoRec = false;

        switch (strTipo)
        {
            case INTERFACE_AB: //"4"
                //case INTERFACE_ISDN_2BD:  //"8"
                //case INTERFACE_ISDN_30BD: //"9"

                DataSet drs = ServiceServiciosCD40.RecursosDeUnaRed((string)Session["idsistema"], strRed);

                if (null != drs && drs.Tables.Count > 0 && drs.Tables[0].Rows.Count < 2)
                {
                    bUnicoRec = true;
                }
                break;
            default:
                bUnicoRec = false;
                break;
        }

        return bUnicoRec;
    }

    protected void DDLRed_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Si se está modificando el recurso y se ha cambiado la red, se detecta el cambio para modificar el mensaje del botón guardar
        if (Modificando && DDLRed.Visible && DDLRed.SelectedIndex != -1 && bUnicoRecursoEnRed)
        {
            if (string.Compare(DDLRed.SelectedValue, strRedActual) != 0)
            {
                string strMsg = string.Empty;
                strMsg = String.Format((string)GetLocalResourceObject("LiberarUnicoRecursoRedTlf"), strRedActual, ListBox1.SelectedValue);

                BtAceptar_ConfirmButtonExtender.ConfirmText = strMsg;
            }
            else
            {
                BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            }
        }

    }

    //Devuelve true si ya existe algun recurso en el sistema con el mismo identificador. En caso contrario, false.
    private bool bIdentificadorAsignado(string strIdSistema, string strIdentificador)
    {
        bool bExiste = false;
        int iExiste = -1;

        // Se comprueba que no existe otro recurso radio o de telefonía con el mismo identificador 
        iExiste = ServiceServiciosCD40.CheckIdentificadorAsignado("R", strIdSistema, strIdentificador);

        if (iExiste > 0)
            bExiste = true;
        else if (iExiste < 0)
        {
            System.Text.StringBuilder strMsgError = new System.Text.StringBuilder();
            strMsgError.AppendFormat("(RecursosTelefonia-bIdentificadorAsignado): el servicio servicioCD40.CheckIdentificadorAsignado('R', '{0}', '{1}') ha devuelto el codigo {2}", strIdSistema, strIdentificador, iExiste);
            logDebugView.Error(strMsgError.ToString());
            strMsgError.Clear();
        }

        return bExiste;
    }  

    ///VMG 20/02/2019
    /// <summary>
    /// Intento de función para habilitar deshabilitar las pestañas de los recursos de telefonia
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    protected void enableAllPhoneTabs()
    {
        IBGenerales.Visible = true;
        IBVoip.Visible = true;
        IBAudio.Visible = true;
        IBFuncionalidad.Visible = true;

        IBGenerales.CssClass = "buttonImage";
        IBVoip.CssClass = "buttonImage";
        IBAudio.CssClass = "buttonImage";
        IBFuncionalidad.CssClass = "buttonImage";

        //IBAsignacion.CssClass = "buttonImageSelected";
    }
}

