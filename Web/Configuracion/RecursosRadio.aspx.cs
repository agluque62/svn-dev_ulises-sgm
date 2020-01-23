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
using log4net;
using log4net.Config;

public partial class RecursosDeRadio : PageBaseCD40.PageCD40	//	System.Web.UI.Page
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
    private static ServiciosCD40.Tablas[] paramDatos;
    private static bool RecursoAsignado = false;
    private static int NumInterfaceAsignado;
    private static int NumDispositivoAsignado;
    private static int var_id_radio_param;
    private static bool _TipoDisabled = false;
    private static int[] NumSlotsPorTipoInterface ={ 1, 1, 1, 1, 1, 1, 1, 4, 2, 16, 1, 1, 1, 1, 1 };
	private static KeyValueConfigurationElement s;
    static bool Modificando;
	static bool PermisoSegunPerfil;
	private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();
	private static int NumPaginaActiva = 0;
	private static bool Editando=false;
    private static DataSet DSRangosFrecuencias;
    //El mapa IdTifx_IpTifx almacena la direccion IP:puerto Sip de cada parasela. A cada elemento de la colección se accede por el Identificador de la pasarela
    //private static System.Collections.Generic.Dictionary<string, string> listaIdTifx_IpSip = new System.Collections.Generic.Dictionary<string, string>();
    //El mapa listaEquipoEu_IpSip almacena la direccion IP:puerto Sip de cada equipo externo. A cada elemento de la colección se accede por el Identificador del equipo externo
    //private static System.Collections.Generic.Dictionary<string, string> listaEquipoEu_IpSip = new System.Collections.Generic.Dictionary<string, string>();

    private Ulises5000Configuration.ToolsUlises5000Section UlisesToolsVersion;
    private AsyncCallback CallbackCompletado;

    const string INTERFACE_ATS_QSIG = "7";
    const string INTERFACE_ISDN_2BD = "8";
    const string INTERFACE_ISDN_30BD = "9";

    //MVO
    //Indica si el recurso está asociado a un destino radio de tipo Frecuencia desplazada
    //En este caso, no se permitirá eliminar el recurso desde esta pantalla, ni modificar 
    //el emplazamiento para evitar que la configuración del destino radio se quede inconsistente
    //Antes de borrarlo, el usuario debe eliminar el recurso del destino radio
    private static bool bRecursoConDestinoFD = false;

    private static int iTipoRecursoOriginal;

    //Lista de valores para el tipo equipo (RBLTipoEquipo) de la pestaña Asignación HW
    const string HW_TIPO_PASARELA = "0";
    const string HW_TIPO_EQUIPO_EXTERNO = "1";

    const string METODO_BSS_NUCLEO = "NUCLEO";

    const string EQUIPO_M_N_ROHDE  = "1000";
    const string EQUIPO_M_N_JOTRON = "1001";
    const int PUERTO_TXRX_JOTRON = 161;
    const int PUERTO_TX_ROHDE = 161;
    const int PUERTO_RX_ROHDE = 160;
    

    static int[] Tabla_idbss = new int[16];
    
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

			BtModificar.Visible = BtNuevo.Visible = perfil == "3" && !Editando;
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            Ulises5000Configuration.ToolsUlises5000Section UlisesTools = Ulises5000Configuration.ToolsUlises5000Section.Instance;

            UlisesToolsVersion = UlisesTools;
        }

        if (CallbackCompletado == null)
            CallbackCompletado = new AsyncCallback(OnCallBackCompleted);
        
        if (!IsPostBack)
        {
            // Mostrar grabación ED137 sólo para Nouakchott (Version==1)
            //if (UlisesToolsVersion.Tools["GrabacionRecursoRadio"] == null)
            //    TblRecorders.Visible = CheckGrabacionEd137.Visible = false;
            // Mostrar Tipo recurso radio Audio HF-Tx sólo para NDjamena (Versión=2)
            if (UlisesToolsVersion.Tools["N+M"] == null)
                DListTipo.Items.RemoveAt(4);    // N+M
            if (UlisesToolsVersion.Tools["RadioHF"] == null)
                DListTipo.Items.RemoveAt(3);    // HF


            //MVO.20170711: Se ocultan las pestañas Gestor y OID del Accordion2 que se utiliza para la configuración de recursos de tipo M+N             
            AccordionPane2.Visible = false;
            AccordionPane4.Visible = false;
            IndexListBox1 = -1;
            

            IBGenerales.CssClass = "buttonImageSelected";
            IBVoip.CssClass = "buttonImage";
            IBAudio.CssClass = "buttonImage";
            IBAsignacion.CssClass = "buttonImage";
            IBFuncionalidad.CssClass = "buttonImage";

            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");
            
            NumPaginaActiva = 0;
            iTipoRecursoOriginal = -1;

            CargarZonas();
            CargarMetodosBSS();
            CargarListaTablasBss();

            CargaDDLTifX();
            CargaParametrosHF();
            CargaEmplazamientos();

            CargaDDLEquiposExternos();
            //CargarInforme();
        
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
                
				if (Request.Form["eliminadeTFT"] == "1")
				{
					Request.Form["eliminadeTFT"].Replace("1", "0");

					EliminaDestinoDeTFT();
					GrabaCambios();
				}
				if (Request.Form["SoloEliminaDeTFT"] == "1")
				{
					Request.Form["SoloEliminaDeTFT"].Replace("1", "0");

                    BorrarRecurso();
                    EliminaDestinoDeTFT();
                    try
                    {
                        // Llamada asíncrona para regenerar todas las sectorizaciones.
                        Session.Add("Sectorizando", true);
                        ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, false, CallbackCompletado, null);
                    }
                    catch (Exception ex)
                    {
                        logDebugView.Error("(RecursosDeRadio-EliminarRecurso): ", ex);
                    }

                    MuestraDatos(DameDatos());
                }

            }
    }

    private bool bPasarelaConGrabadores(string strPasarela)
    {
        //Comprueba si la pasarela asociada tiene configurada algún grabador. Si es así devuelve true y en caso contrario false.
        bool bExistenGrabadores = false;

        if (!string.IsNullOrEmpty(strPasarela))
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            Session["idsistema"] = s.Value;

            ServiciosCD40.TifX tifx = new ServiciosCD40.TifX();
            tifx.IdSistema = s.Value;
            tifx.IdTifx = strPasarela;

            DataSet ds = ServicioCD40.DataSetSelectSQL(tifx);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dParela = ds.Tables[0].Rows[0];
                if ((dParela["Grabador1"] != System.DBNull.Value && !string.IsNullOrWhiteSpace((string)dParela["Grabador1"])) ||
                    (dParela["Grabador2"] != System.DBNull.Value && !string.IsNullOrWhiteSpace((string)dParela["Grabador2"])))
                {
                    bExistenGrabadores = true;
                }
                else
                    bExistenGrabadores = false;
            }
        }

        return bExistenGrabadores;
    }

    protected void CargaGVRangos()
    {
        if (ListBox1.SelectedIndex != -1)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            ServiciosCD40.HFRangoFrecuencias r = new ServiciosCD40.HFRangoFrecuencias();

            ((GridView)AccordionPane1.FindControl("GVRangos")).DataSource = null;
            ((GridView)AccordionPane1.FindControl("GVRangos")).DataBind();
            //((GridView)AccordionPane5.FindControl("GVRangos_NM")).DataSource = null;
            //((GridView)AccordionPane5.FindControl("GVRangos_NM")).DataBind();

            r.IdSistema = s.Value;
            r.IdRecurso = TxtIdRecurso.Text;
            DataSet rangoFrecuencias = ServicioCD40.DataSetSelectSQL(r);
            if (rangoFrecuencias.Tables.Count > 0)
            {
                DSRangosFrecuencias = rangoFrecuencias;
                ((GridView)AccordionPane1.FindControl("GVRangos")).DataSource = rangoFrecuencias;
                ((GridView)AccordionPane1.FindControl("GVRangos")).DataBind();
                //((GridView)AccordionPane5.FindControl("GVRangos_NM")).DataSource = rangoFrecuencias;
                //((GridView)AccordionPane5.FindControl("GVRangos_NM")).DataBind();
            }
        }
    }

	private void ModoEdicion(bool edicion)
	{
		Editando = edicion;
		ListBox1.Enabled = !edicion;
		BtAceptar.Visible = BtCancelar.Visible = edicion;
		BtNuevo.Visible = !edicion && PermisoSegunPerfil;
		BtModificar.Visible = BtEliminar.Visible = ListBox1.Items.Count > 0 && !edicion && PermisoSegunPerfil;
        //ParamCell.Visible = edicion;
        Accordion1.Visible = edicion && (UlisesToolsVersion.Tools["RadioHF"] != null && DListTipo.SelectedValue == "3"); // AUDIO RX TX HF

        ((TextBox)AccordioPane2.FindControl("TbIpGestor")).Enabled = edicion;
        ((TextBox)AccordionPane1.FindControl("TbMin")).Visible = edicion;
        ((TextBox)AccordionPane1.FindControl("TBMax")).Visible = edicion;
        ((TextBox)AccordionPane3.FindControl("TBOid")).Enabled = edicion;
        ((Button)AccordionPane1.FindControl("BtnAnadirRango")).Enabled = edicion;
        ((GridView)AccordionPane1.FindControl("GVRangos")).Enabled = edicion;

        Accordion2.Enabled = edicion;

        if (edicion)
        {
            ((TextBox)AccordionPane1.FindControl("TbMin")).Text = (string)GetLocalResourceObject("StrRangoMinimo.Text").ToString();
            ((TextBox)AccordionPane1.FindControl("TBMax")).Text = (string)GetLocalResourceObject("StrRangoMaximo.Text").ToString();
            //if (Accordion2.Visible)
            //{
            //    ((TextBox)AccordionPane5.FindControl("TextBox3")).Text = (string)GetLocalResourceObject("StrRangoMinimo.Text").ToString();
            //    ((TextBox)AccordionPane5.FindControl("TextBox4")).Text = (string)GetLocalResourceObject("StrRangoMaximo.Text").ToString();
            //}
        }
	}

    protected void BtNuevo_Click(object sender, EventArgs e)
    {
        IndexListBox1 = ListBox1.SelectedIndex;

        MultiView1.ActiveViewIndex = NumPaginaActiva = 0;

        _TipoDisabled = false;
		Modificando = false;
        RecursoAsignado = false;
        TxtIdRecurso.Enabled = true;

		DDLEquipoExternos.SelectedIndex = 0;
        DListZonas.SelectedIndex = 0;
        DListEmplazamiento.SelectedIndex = 0;

		ModoEdicion(true);
        HabilitarEscritura(false);

		//MostrarMenu(true);
        HabilitaElementos(true);
		HabilitarValidacion(true);

        EstadoItemsNuevoRecurso();
    }

    private void EstadoItemsNuevoRecurso()
    {
        // Pasar a la primera página
        OnButton_Click(IBGenerales, null);

        TxtIdRecurso.Text = "";
        DListTipo.SelectedValue = "2";    // Audio Tx-Rx

        //Se inicializa a sin destino asociado. La cadena se lee del fichero de recursos
        TBDestino.Text = GetLocalResourceObject("TBDestinoResource1.Text").ToString();

        if (DLTifx.Items.Count > 0)
            DLTifx.SelectedIndex = 0;
//        CheckDiffServ.Checked = false;
        TxtServidorSIP.Text = "";
        CheckSupresionSilencio.Checked = true;
        
        CheckGrabacionEd137.Checked = false;
        TxtTamanioPaquete.Text = "20";
        RBCodecA.Checked = true;
        RBCodecMu.Checked = false;
        RBGananciaTx.Checked = true;
        RBAGCTx.Checked = false;
        RBGananciaRx.Checked = true;
        RBAGCRx.Checked = false;
        TxtTiempoPTT.Text = "120";
        TxtUmbralVAD.Text = "-33";
        TxtDesactivacionSQ.Text = "1";
        TxtRepSQyBSS.Text = "1";
		TxtKAM.Text = "10";
		TxtKAP.Text = "200";
		TxtActivacionPTT.Text = "200";
		TxtUmbralE.Text = "-30";
		TxtUmbralM.Text = "-30";
		TxtUmbralPTT.Text = "-18";
		TxtUmbralSQ.Text = "-33";
        //TextBoxCLD.Text = "1";
        TxtGRSDelay.Text = "0";
        DListTablasCalidad.SelectedIndex = 0;
        //MVO: Por defecto, el tipo de equipo es pasarela
        RBLTipoEquipo.SelectedValue = HW_TIPO_PASARELA;

        /*// MAF
        CheckGrabacionEd137.Checked = false;
        TxtGRSDelay.Text = "0"; */
        
        Label24.Visible = DListMetodoBSS.Visible = DListMetodoBSS.Enabled = CheckBSS.Checked = false;


        RBReceptor.Checked = RBPrincipal.Checked = RBMono.Checked = RBVHF.Checked = true;
        RangeValidator3.Enabled = true;

        //MVO:Se habilita el validor de rango de frecuencias del tipo VHF y se deshabilitan el resto de validadores
        RangeValidatorVHFFrequency.Enabled = true;
        RangeValidatorHFFrequency.Enabled = false;
        RangeValidatorUHFFrequency.Enabled = false;

        RBTransmisor.Checked = RBTransceptor.Checked = RBMulti.Checked = RBOtros.Checked = RBReserva.Checked = /*RBAmbos.Checked =*/ RBHF.Checked = RBUHF.Checked = false;


        //if (DListTipo.SelectedValue == "3" ||
        //    DListTipo.SelectedValue == "4" ||
        //    DListTipo.SelectedValue == "5" ||
        //    DListTipo.SelectedValue == "6")
        {
            DDLCanalizacion.SelectedValue = "0";
            DDLModulacion.SelectedValue = DDLOffsetGeneral.SelectedValue = DDLPotencia.SelectedValue = TbPuerto.Text = "0";
            
            //DDLOffset.SelectedValue = DDLOffsetGeneral.SelectedValue;
            TBPriority.Text = "50";
        }

        IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = true; // AUDIO N+M;
        IBVoip.CssClass = "buttonImage";
        IBAudio.CssClass = "buttonImage";
        IBFuncionalidad.CssClass = "buttonImage";

        Accordion2.Visible = Accordion1.Visible = false;

        GVRangos.DataSource = /*GVRangos_NM.DataSource = */ null;
        GVRangos.DataBind();
        //GVRangos_NM.DataBind();

		MuestraEquipoDelRecurso();

        MostrarParametrosFuncionalidad();

        CargaRecursosDeLaTifx(DLTifx.SelectedValue);
    }

    private void CargaEmplazamientos()
    {
        try
        {
            ServiciosCD40.Emplazamientos emp = new ServiciosCD40.Emplazamientos();

            emp.IdSistema = (string)Session["IdSistema"];
			DListEmplazamiento.DataSource = ServicioCD40.DataSetSelectSQL(emp);
            DListEmplazamiento.DataTextField = "IdEmplazamiento";
            DListEmplazamiento.DataBind();                
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeRadio-CargaEmplazamientos): ", ex);
        }
    }

    private void MostrarElemento()
    {
        if (null != datos && null != paramDatos)
        {
            foreach (ServiciosCD40.Recursos rec in datos)
                if (String.Compare((rec.IdRecurso), ListBox1.SelectedValue) == 0)
                {
                    BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarRecurso"), ListBox1.SelectedValue);

                    TxtIdRecurso.Text = rec.IdRecurso;
                    TxtServidorSIP.Text = rec.ServidorSIP;
                    DListTipo.SelectedValue = rec.Tipo > 3 && rec.TipoRecurso < 7 ? "4" : rec.Tipo.ToString();
                    //Se guarda el valor original del tipo de recurso, para saber si se ha modificado
                    iTipoRecursoOriginal = (int)rec.Tipo;

                    //                CheckDiffServ.Checked = rec.Diffserv;

                    //ParamCell.Visible = DListTipo.SelectedValue == "3";   // Audio HF-TX
                    Accordion1.Visible = DListTipo.SelectedValue == "3";   // Audio HF-TX


                    if (DListTipo.SelectedValue == "4" || DListTipo.SelectedValue == "5" || DListTipo.SelectedValue == "6")
                    {   // N+M
                        RequiredFieldValidator2.Enabled = Accordion2.Visible = true;
                    }
                    else
                    {
                        RequiredFieldValidator2.Enabled = Accordion2.Visible = false;
                    }


                    CargaParametrosHF();

                    MuestraSistemaHardware(rec.IdTifX != null);
                    if (rec.IdTifX != null)
                    {
                        //El recurso está asignado a una pasarela
                        DDLEquipoExternos.SelectedIndex = 0;
                        RBLTipoEquipo.SelectedValue = HW_TIPO_PASARELA;
                        DLTifx.SelectedValue = rec.IdTifX;
                        CargaRecursosDeLaTifx(DLTifx.SelectedValue);

                        IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = true; // AUDIO N+M;
                        IBVoip.CssClass = IBVoip.CssClass == "buttonImageDisabled" ? "buttonImage" : IBVoip.CssClass;
                        IBAudio.CssClass = IBAudio.CssClass == "buttonImageDisabled" ? "buttonImage" : IBAudio.CssClass;
                        IBFuncionalidad.CssClass = IBFuncionalidad.CssClass == "buttonImageDisabled" ? "buttonImage" : IBFuncionalidad.CssClass;

                        //Solo visible para las pasarelas
                        CheckBoxEventosPTT_SQ.Visible = true;
                    }
                    else if ((rec.IdEquipo != null) && (rec.IdEquipo != ""))
                    {
                        //El recurso está asignado a un equipo externo
                        RBLTipoEquipo.SelectedValue = HW_TIPO_EQUIPO_EXTERNO;
                        DDLEquipoExternos.SelectedValue = rec.IdEquipo;

                        IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = false; // AUDIO N+M;
                        IBVoip.CssClass = "buttonImageDisabled";
                        IBAudio.CssClass = "buttonImageDisabled";
                        IBFuncionalidad.CssClass = "buttonImageDisabled";

                        CheckBoxEventosPTT_SQ.Visible = false;
                    }
                    break;
                }

            //MVO: Se inicializa la variable a false por si el recurso no estuviera en la tabla RecursosRadio
            bRecursoConDestinoFD = false;

            foreach (ServiciosCD40.RecursosRadio r in paramDatos)
                if (String.Compare(r.IdRecurso, ListBox1.SelectedValue) == 0)
                {
                    TxtTamanioPaquete.Text = r.TamRTP.ToString();
                    //TxtGananciaRx.Text = (r.GananciaAGCRXdBm.ToString()).Replace('.', ',');
                    //TxtGananciaTx.Text = (r.GananciaAGCTXdBm.ToString()).Replace('.', ',');
                    TxtGananciaRx.Text = r.GananciaAGCRXdBm.ToString();
                    TxtGananciaTx.Text = r.GananciaAGCTXdBm.ToString();
                    DListEmplazamiento.Text = r.IdEmplazamiento;


                    _TipoDisabled = r.IdDestino != null;
                    if (r.IdDestino != null)
                    {
                        TBDestino.Text = r.IdDestino;
                    }
                    else
                    {
                        //TBDestino.Text = "Sin destino asociado";
                        TBDestino.Text = GetLocalResourceObject("TBDestinoResource1.Text").ToString();
                    }


                    //MVO: Obtenemos si el recurso seleccionado está asociado a un destino radio con FD, es decir, un destino radio con frecuencia con valor 5 (TipoFrec= 5)
                    //     Para no permitir eliminar el recurso ni modificar el emplazamiento si está asociado a un destino radio FD               
                    if (!string.IsNullOrEmpty(r.IdDestino) && r.DestRad_tipoFrec == 5)
                    {
                        //No se permite modificar el emplazamiento al que está asociado el recurso                                      
                        bRecursoConDestinoFD = true;
                    }
                    else
                    {
                        bRecursoConDestinoFD = false;
                    }


                    TxtUmbralVAD.Text = r.UmbralVAD.ToString();
                    TxtRepSQyBSS.Text = r.RepSQyBSS.ToString();
                    TxtTiempoPTT.Text = r.TiempoPTT.ToString();
                    TxtActivacionPTT.Text = r.TimeoutPTT.ToString();
                    TxtDesactivacionSQ.Text = r.DesactivacionSQ.ToString();
                    TxtUmbralE.Text = r.UmbralTonoE.ToString();
                    TxtUmbralM.Text = r.UmbralTonoM.ToString();
                    TxtUmbralPTT.Text = r.UmbralTonoPTT.ToString();
                    TxtUmbralSQ.Text = r.UmbralTonoSQ.ToString();
                    CheckEM.Checked = r.EM;
                    CheckNTZ.Checked = r.NTZ;
                    CheckBoxEventosPTT_SQ.Checked = r.EnableEventPttSq;

                    CheckCifrado.Checked = r.Cifrado;
                    CheckTX.Checked = r.SupervPortadoraTx;
                    CheckRX.Checked = r.SupervModuladoraTx;
                    DListSQ.SelectedValue = r.SQ;
                    DListPTT.SelectedValue = r.PTT;
                    DListTipoNTZ.SelectedValue = r.TipoNTZ.ToString();
                    DListModoConfPTT.SelectedValue = r.ModoConfPTT.ToString();
                    DDLOffsetGeneral.SelectedValue = r.OffSetFrequency.ToString();

                    TxtGRSDelay.Text = r.GrsDelay.ToString();
                    //TextBoxCLD.Text = r.CldSupervisionTime.ToString();
                    CheckBSS.Checked = r.BSS;
                    CheckGrabacionEd137.Checked = r.GrabacionEd137;
                    var_id_radio_param = r.Radio_param_idradio_param;

                    DListFlujosAudio.SelectedValue = r.NumFlujosAudio.ToString();
                    TxtKAP.Text = r.KeepAlivePeriod.ToString();
                    TxtKAM.Text = r.KeepAliveMultiplier.ToString();
                    CheckSupresionSilencio.Checked = r.SupresionSilencio;

                    //SeleccionarMetodoBSS(r.Metodos_bss_idmetodos_bss, DListMetodoBSS.Items.Count);
                    SeleccionarZona(r.Zonas_IdZonas, DListZonas.Items.Count);                       //SeleccionarZona(r.Zonas_IdZonas, CargarZonas());

                    MostrarParametrosFuncionalidad();

                    Label33.Visible = TxtUmbralVAD.Visible = DListSQ.SelectedIndex != 0;
                    ///////////////////////////////////////////////////////////////////////////////////////////
                    if (!Panel3.Visible)
                    {
                        Label24.Visible = DListMetodoBSS.Visible = CheckBSS.Checked;
                    }
                    else
                    {
                        //MVO: si el tipo de recurso es  Rx o RxTx
                        if (DListTipo.SelectedValue == "0" || DListTipo.SelectedValue == "2")
                            CheckBSS.Visible = true;
                        else
                        {
                            CheckBSS.Visible = false;
                            CheckBSS.Checked = false;
                        }
                        Label24.Visible = DListMetodoBSS.Visible = (CheckBSS.Visible && CheckBSS.Checked);
                    }


                    DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = CheckBSS.Checked;



                    if (CheckBSS.Checked)
                    {
                        SeleccionarMetodoBSS(r.Metodos_bss_idmetodos_bss, DListMetodoBSS.Items.Count);

                        DListTablasCalidad.SelectedValue = r.IdTablaBss.ToString();//27_03_17
                        DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = true;
                        //DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = String.Compare(DListMetodoBSS.Items[DListMetodoBSS.SelectedIndex].Text, METODO_BSS_NUCLEO) == 0;

                        //if (DListTablasCalidad.Visible)
                        //{
                        MostrarValoresTablasBss((int)r.IdTablaBss);
                        //}

                    }
                    else
                    {
                        DListTablasCalidad.SelectedIndex = 0;
                        DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = false;
                        //    DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = TableCalidad.Visible = false;
                    }

                    TableCalidad.Visible = DListTablasCalidad.SelectedIndex != 0;

                    //DListTablasCalidad.SelectedValue = r.IdTablaBss.ToString();


                    //if (DListTablasCalidad.Visible)
                    //{
                    //    MostrarValoresTablasBss((int)r.IdTablaBss);
                    //}

                    CheckGrabacionEd137.Checked = r.GrabacionEd137;
                    /*                 
                                    TblRecorders.Visible = 
                                    if (r.Grabador1 == null || r.Grabador1 == string.Empty)
                                        DDLRecorder1.SelectedIndex = 0;
                                    else
                                        DDLRecorder1.SelectedValue = r.Grabador1;

                                    if (r.Grabador2 == null || r.Grabador2 == string.Empty)
                                        DDLRecorder2.SelectedIndex = 0;
                                    else
                                        DDLRecorder2.SelectedValue = r.Grabador2;
                    */
                    if (r.Codec == 0)
                    {
                        RBCodecA.Checked = true;
                        RBCodecA.Enabled = true;
                        RBCodecMu.Enabled = false;
                        RBCodecMu.Checked = false;
                    }
                    else
                    {
                        RBCodecMu.Checked = true;
                        RBCodecMu.Enabled = true;
                        RBCodecA.Enabled = false;
                        RBCodecA.Checked = false;
                    }
                    if (r.GananciaAGCRX == 0)
                    {
                        RBGananciaRx.Checked = true;
                        RBGananciaRx.Enabled = true;
                        RBAGCRx.Checked = false;
                        RBAGCRx.Enabled = false;
                    }
                    else
                    {
                        RBGananciaRx.Checked = false;
                        RBGananciaRx.Enabled = false;
                        RBAGCRx.Checked = true;
                        RBAGCRx.Enabled = true;
                    }
                    if (r.GananciaAGCTX == 0)
                    {
                        RBGananciaTx.Checked = true;
                        RBGananciaTx.Enabled = true;
                        RBAGCTx.Checked = false;
                        RBAGCTx.Enabled = false;
                    }
                    else
                    {
                        RBGananciaTx.Checked = false;
                        RBGananciaTx.Enabled = false;
                        RBAGCTx.Checked = true;
                        RBAGCTx.Enabled = true;
                    }
                    break;
                }
            Label10.Visible = TxtGananciaTx.Visible = RBAGCTx.Checked == false;
            Label3.Visible = TxtGananciaRx.Visible = RBAGCRx.Checked == false;

            //if (!IBFuncionalidad.Enabled || (DListTipo.SelectedIndex >= 4 && DListTipo.SelectedIndex <= 6))
            if (!IBFuncionalidad.Enabled || (DListTipo.SelectedValue == "4" || DListTipo.SelectedValue == "5" || DListTipo.SelectedValue == "6"))
            {
                //Audio M+N
                OnButton_Click(IBGenerales, null);
                //MVO1: RBLTipoEquipo.SelectedValue = HW_TIPO_EQUIPO_EXTERNO;
                MuestraEquipoDelRecurso();

                IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = false; // AUDIO N+M;
                IBVoip.CssClass = "buttonImageDisabled";
                IBAudio.CssClass = "buttonImageDisabled";
                IBFuncionalidad.CssClass = "buttonImageDisabled";
            }
            else
            {
                if (IBVoip.Enabled)
                {
                    IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = true;
                    IBVoip.CssClass = IBVoip.CssClass == "buttonImageDisabled" ? "buttonImage" : IBVoip.CssClass;
                    IBAudio.CssClass = IBAudio.CssClass == "buttonImageDisabled" ? "buttonImage" : IBAudio.CssClass;
                    IBFuncionalidad.CssClass = IBFuncionalidad.CssClass == "buttonImageDisabled" ? "buttonImage" : IBFuncionalidad.CssClass;
                }

                //MVO: el campo CheckGrabacionEd137 sólo será visible para los tipo Audio RX (0) y Audio RXTX (2) cuando el recurso esté asignado a una pasarela (value=0)
                if ((DListTipo.SelectedValue == "0" || DListTipo.SelectedValue == "2") && null != DLTifx.SelectedItem && RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA &&
                    ((Modificando && RecursoAsignado) || (!Modificando && 0 == DDLEquipoExternos.SelectedIndex))
                   )
                    CheckGrabacionEd137.Visible = true;
                else
                    CheckGrabacionEd137.Visible = false;
            }
        }
    }


     private int CargarMetodosBSS()
     {
         
        try 
         {
             DListMetodoBSS.Items.Clear();
             ServiciosCD40.MetodosBss metbss = new ServiciosCD40.MetodosBss();

             DListMetodoBSS.DataSource = ServicioCD40.DataSetSelectSQL(metbss);
             DListMetodoBSS.DataTextField = "name";
             DListMetodoBSS.DataValueField = "idmetodos_bss";            
             DListMetodoBSS.DataBind();

             ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(metbss);

             return d.Length;
        }
         catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeRadio-CargarMetodosBSS): ", ex);
            return 0;
        }


     }

     private void SeleccionarMetodoBSS(int idMetodo, int NumMetodosBSS)
     {
         if (NumMetodosBSS > 0/* && idMetodo > 0*/)
         {
             for (int i = 0/*1*/; i < NumMetodosBSS; i++)
             {

                 if (String.Compare(DListMetodoBSS.Items[i].Value, idMetodo.ToString()) == 0)
                 {
                     DListMetodoBSS.SelectedIndex = i;
                     //DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = TableCalidad.Visible = DListMetodoBSS.SelectedIndex == 1;
                     DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible =  DListMetodoBSS.SelectedIndex == 1;
                     break;
                 }

             }
         }
         else
             DListMetodoBSS.SelectedIndex = 0;
     }
   

    private int CargarZonas()
    {
        try
        {   
            ServiciosCD40.Zonas zon = new ServiciosCD40.Zonas();
            zon.IdSistema = (string)Session["IdSistema"];


            DListZonas.DataSource = ServicioCD40.DataSetSelectSQL(zon);
            DListZonas.DataTextField = "Nombre";
            DListZonas.DataValueField = "idZonas";
            DListZonas.DataBind();
            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(zon);

            return d.Length;


        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeRadio-CargarZonas): ", ex);
            return 0;
        }
    }

    private void SeleccionarZona(int idZona, int NumZonas)
    {
        if (NumZonas > 0 && idZona > 0)
        {
            ActualizaWebPadre(true);

            for (int i = 1; i < NumZonas; i++)
            {

                if (String.Compare(DListZonas.Items[i].Value, idZona.ToString()) == 0)
                {
                    DListZonas.SelectedIndex = i;
                    break;
                }

            }
        }
        else
            DListZonas.SelectedIndex = 0;

       

    }

    private int CargarListaTablasBss()
    {
        
        
        try{

            //DListTablasCalidad.Items.Clear();
            ServiciosCD40.Tabla_bss n = new ServiciosCD40.Tabla_bss();

            DListTablasCalidad.DataSource = ServicioCD40.DataSetSelectSQL(n);
            DListTablasCalidad.DataTextField = "name";
            DListTablasCalidad.DataValueField = "idtabla_bss";
            DListTablasCalidad.DataBind();

            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(n);

            return d.Length;


        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeRadio-CargarZonas): ", ex);
            return 0;
        }
    }


    private void MostrarTablaCalidad(int IdTablaBSS)
    {
        try
        {
            TableCalidad.Visible = true;

            // Leo idRecursos asociado al idTabla en Tabla_bss_idtabla_bss[]
            ServiciosCD40.ValoresTabla t = new ServiciosCD40.ValoresTabla();

            t.Tabla_bss_idtabla_bss = IdTablaBSS;
            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);


            for (int i = 0; i < d.Length; i++)
            {

                Tabla_idbss[i] = ((ServiciosCD40.ValoresTabla)d[i]).IdValores_Tabla;
                switch (i)
                {

                    case 0:
                        Label1_2.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow1_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                        break;
                    case 1:

                        Label2_2.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow2_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                        break;
                    case 2:

                        Label3_2.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow3_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                        break;
                    case 3:

                        Label4_2.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow4_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                        break;
                    case 4:

                        Label5_2.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow5_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                        break;
                    case 5:

                        Label6_2.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow6_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                        break;
                }

                // Rellenar la tabla
            }
        }
        catch (Exception e)
        {
            logDebugView.Error("(RecursosDeRadio-MostrarTablaCalidad:", e);
        }
    }
   
    private void MostrarValoresTablasBss(int IdTablaBss)
    {
        int numTablasBss = DListTablasCalidad.Items.Count;

        if (IdTablaBss != 0 && numTablasBss > 0)
        {
            for (int i = 1; i <= numTablasBss; i++)
            {
                // Se muestra el nombre y la tabla f
                if (String.Compare(DListTablasCalidad.Items[i].Value, IdTablaBss.ToString()) == 0)
                {
                    DListTablasCalidad.SelectedIndex = i;
                    MostrarTablaCalidad(Convert.ToInt32(DListTablasCalidad.Items[i].Value));// TablaValores=f(idTablabss)
                    break;
                }
            }
        }
        else
        {
            DListTablasCalidad.SelectedIndex = 0;
        }
    }

    private void CargaParametrosHF()
    {
        //if (Accordion1.Visible)
        //{

            ((TextBox)AccordioPane2.FindControl("TbIpGestor")).Text = string.Empty;
            ((TextBox)AccordionPane3.FindControl("TBOid")).Text = string.Empty;

            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            ServiciosCD40.HFParams r = new ServiciosCD40.HFParams();

            r.IdSistema = s.Value;
            r.IdRecurso = ListBox1.SelectedValue;
            ServiciosCD40.Tablas[] t = ServicioCD40.ListSelectSQL(r);

            if (t.Length > 0)
            {
                TextBox1.Text = TBIpGestor.Text = ((ServiciosCD40.HFParams)t[0]).IpGestor;
                TextBox2.Text = TBOid.Text = ((ServiciosCD40.HFParams)t[0]).Oid;

                RBReceptor.Checked = ((ServiciosCD40.HFParams)t[0]).TipoEquipo == 0;
                RBTransmisor.Checked = ((ServiciosCD40.HFParams)t[0]).TipoEquipo == 1;
                RBTransceptor.Checked = ((ServiciosCD40.HFParams)t[0]).TipoEquipo == 2;


                RBMono.Checked = ((ServiciosCD40.HFParams)t[0]).TipoCanal == 0;
                RBMulti.Checked = ((ServiciosCD40.HFParams)t[0]).TipoCanal == 1;
                RBOtros.Checked = ((ServiciosCD40.HFParams)t[0]).TipoCanal == 2;

                RangeValidatorHFFrequency.Enabled = RBHF.Checked = ((ServiciosCD40.HFParams)t[0]).TipoFrecuencia == 0;
                RangeValidatorVHFFrequency.Enabled = RBVHF.Checked = ((ServiciosCD40.HFParams)t[0]).TipoFrecuencia == 1;
                RangeValidatorUHFFrequency.Enabled = RBUHF.Checked = ((ServiciosCD40.HFParams)t[0]).TipoFrecuencia == 2;

                RBPrincipal.Checked = ((ServiciosCD40.HFParams)t[0]).TipoModo == 0;
                RBReserva.Checked = ((ServiciosCD40.HFParams)t[0]).TipoModo == 1;

                if (RBPrincipal.Checked == true)
                {
                    RangeValidator3.Enabled = true;
                }
                else
                {
                    RangeValidator3.Enabled = false;
                }
                //RBAmbos.Checked = ((ServiciosCD40.HFParams)t[0]).TipoModo == 2;

                ConfiguraOpcionesM_N(null, null);

                // Eliminar el punto decimal
                if (((ServiciosCD40.HFParams)t[0]).Frecuencia == string.Empty)
                {
                    TBTunedFrequency.Text = string.Empty;
                    TBTunedDecimals.Text = string.Empty;
                }
                //else if (((ServiciosCD40.HFParams)t[0]).Frecuencia.Length < 7)
                //    TBTunedFrequency.Text = ((ServiciosCD40.HFParams)t[0]).Frecuencia;
                else
                {
                    string[] f = ((ServiciosCD40.HFParams)t[0]).Frecuencia.Split(new char[] { '.' }, StringSplitOptions.None);

                    TBTunedFrequency.Text = f[0];
                    TBTunedDecimals.Text = f[1];
                }


                TBPriority.Text = ((ServiciosCD40.HFParams)t[0]).PrioridadEquipo.ToString();
                TbPuerto.Text = ((ServiciosCD40.HFParams)t[0]).Puerto.ToString();
                // DDLOffset.SelectedValue = ((ServiciosCD40.HFParams)t[0]).Offset.ToString();                
                DDLCanalizacion.SelectedValue = ((ServiciosCD40.HFParams)t[0]).Canalizacion.ToString();
                DDLModulacion.SelectedValue = ((ServiciosCD40.HFParams)t[0]).Modulacion.ToString();
                DDLPotencia.SelectedValue = ((ServiciosCD40.HFParams)t[0]).Potencia.ToString();
                DDLMarca.SelectedValue =((ServiciosCD40.HFParams)t[0]).ModeloEquipo.ToString();
            }
            else
            {
                TextBox1.Text = TBIpGestor.Text = "";
                TextBox2.Text = TBOid.Text = "";
                TBPriority.Text = "50";
                TBTunedFrequency.Text = "";
                TBTunedDecimals.Text = string.Empty;
                TbPuerto.Text ="0";
                // DDLOffset.SelectedValue = "0";
                DDLCanalizacion.SelectedValue = "0";
                DDLModulacion.SelectedValue = "0";
                DDLPotencia.SelectedValue = "0";
                DDLMarca.SelectedIndex = 0;
            }
            CargaGVRangos();
        //}
    }

	private void MuestraSistemaHardware(bool muestraTifx)
	{
		DLTifx.Visible = muestraTifx;
		Label14.Visible = muestraTifx;
		TTifx.Visible = muestraTifx;

		LblEquipoExterno.Visible = !muestraTifx;
		DDLEquipoExternos.Visible = !muestraTifx;
	}

    private void EsconderMenu()
    {
        LError.Text = "";
        Modificando = false;

        //TxtIdRecurso.Visible = false;
        /*
		 * BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        if (ListBox1.SelectedIndex>=0)
            BtModificar.Visible = PermisoSegunPerfil;
        else
            BtModificar.Visible = false;
		 * */
    }

    private void HabilitaElementos(bool habilita)
    {
		RBLTipoEquipo.Enabled = habilita && !RecursoAsignado && DDLEquipoExternos.SelectedIndex == 0;
		DDLEquipoExternos.Enabled = habilita;
		
		//CheckDiffServ.Enabled = habilita;
        //CheckSupresionSilencio.Enabled = habilita;        
        RBAGCRx.Enabled = habilita;
        RBAGCTx.Enabled = habilita;
        RBCodecA.Enabled = habilita;
        RBCodecMu.Enabled = habilita;
        RBGananciaTx.Enabled = habilita;
        RBGananciaRx.Enabled = habilita;
        DListTipo.Enabled = habilita && !_TipoDisabled;
        RbConfiguracionBasica.Enabled = habilita;
        RbConfiguracionNM.Enabled = habilita;
		DLTifx.Enabled = habilita && !RecursoAsignado && DDLEquipoExternos.SelectedIndex == 0;
        DListEmplazamiento.Enabled = habilita;
        DListZonas.Enabled = habilita;
        CheckEM.Enabled = habilita;
        CheckNTZ.Enabled = habilita;
        CheckTX.Enabled = habilita;
        CheckRX.Enabled = habilita;
        CheckCifrado.Enabled = habilita;
        CheckGrabacionEd137.Enabled = habilita;
        CheckBoxEventosPTT_SQ.Enabled = habilita;
        DDLOffsetGeneral.Enabled = habilita;
        CheckBSS.Enabled = habilita;
        DListMetodoBSS.Enabled = habilita && CheckBSS.Checked;

        DListPTT.Enabled = habilita;
        DListSQ.Enabled = habilita;
        DListTipoNTZ.Enabled = habilita;
        DListModoConfPTT.Enabled = habilita;
        DListTablasCalidad.Enabled = habilita;
        //TextBoxCLD.Enabled = habilita;
        DListFlujosAudio.Enabled = habilita;

        HabilitaTablaTifx(habilita);
    }

    private void HabilitaTablaTifx(bool habilita)
    {
        TTifx.Enabled = true;
        for (int j = 1; j <= 4; j++)
            for (int i = 1; i <= 4; i++)
            {
                CheckBox chk = (CheckBox)TTifx.FindControl("CheckBox" +j.ToString()+ i.ToString());
                if (chk != null)
                    chk.Enabled = habilita;
            }
    }

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            BtEliminar.Enabled = PermisoSegunPerfil;
            MostrarElemento();
			//MostrarMenu(true);            
            BtModificar.Visible = PermisoSegunPerfil;
            BtAceptar.Visible = false;
            BtCancelar.Visible = false;


        }
    }

    /// VMG 22/11/2018
    /// <summary>
    /// Filtro de búsqueda para recursos radio
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FiltroBusquedaRD_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Reset de todos los elementos
        ArrayList ListBoxArray = new ArrayList();
        FiltroNombreRD.Visible = false;
        FiltroNombreRD.Text = "";
        ButtonFiltroBuscarRD.Visible = false;
        FiltroTipoRD.Visible = false;
        FiltroTipoRD.SelectedIndex = 0;

        switch (FiltroBusquedaRD.SelectedIndex)
        {
            case 1://Todos
                MuestraDatos(DameDatos());
                break;
            case 2://Nombre
                FiltroNombreRD.Visible = true;
                ButtonFiltroBuscarRD.Visible = true;
                break;
            case 3://Tipo
                FiltroTipoRD.Visible = true;
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
    /// VMG 22/11/2018
    /// <summary>
    /// Filtro de búsqueda para recursos radio
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FiltroTipoRD_SelectedIndexChanged(object sender, EventArgs e)
    {
        MuestraDatos(DameDatos(), FiltroTipoRD.SelectedIndex);
    }

    /// VMG 22/11/2018
    /// <summary>
    /// Busca los recursos por nombre a través del 'Contains'
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ButtonFiltroBuscarRD_Click(object sender, EventArgs e)
    {
        ListBox1.Items.Clear();

        ServiciosCD40.Tablas[] nu = DameDatos();

        for (int i = 0; i < nu.Length; i++)
        {
            if (((ServiciosCD40.Recursos)nu[i]).TipoRecurso == 0)
            {
                if (((ServiciosCD40.Recursos)nu[i]).IdRecurso.Contains(FiltroNombreRD.Text))
                    ListBox1.Items.Add(((ServiciosCD40.Recursos)nu[i]).IdRecurso);
            }
        }
    }

    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.Recursos t = new ServiciosCD40.Recursos();

			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            s = config.AppSettings.Settings["Sistema"];
            Session["idsistema"] = s.Value;

			t.IdSistema = (string)Session["IdSistema"];
			t.TipoRecurso = 0;  // RD
			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            datos = d;

            ServiciosCD40.RecursosRadio p = new ServiciosCD40.RecursosRadio();
            p.IdSistema = (string)Session["IdSistema"];

			ServiciosCD40.Tablas[] pd = ServicioCD40.ListSelectSQL(p);
            paramDatos = pd;
            return d;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeRadio-DameDatos): ", ex);
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
        int elementoSeleccionado = ListBox1.SelectedIndex;
        ListBox1.Items.Clear();

        //if (nu!=null)
        for (int i = 0; i < nu.Length; i++)
        {
            if ((((ServiciosCD40.Recursos)nu[i]).Tipo == 3 /* Audio-HF-Tx */ &&
                UlisesToolsVersion.Tools["RadioHF"] == null) ||
                ((((ServiciosCD40.Recursos)nu[i]).Tipo == 4 || ((ServiciosCD40.Recursos)nu[i]).Tipo == 5 || ((ServiciosCD40.Recursos)nu[i]).Tipo == 4)/* N+M */ &&
                UlisesToolsVersion.Tools["N+M"] == null))
                // El recurso no se muestra
                continue;

            if (tipo != 0)
            {//Solo los tipos a buscar
                if (((ServiciosCD40.Recursos)nu[i]).Tipo == tipo-1)//Empiezan en cero los tipos
                    ListBox1.Items.Add(((ServiciosCD40.Recursos)nu[i]).IdRecurso);
                if (tipo == 5 && (((ServiciosCD40.Recursos)nu[i]).Tipo == 5 || ((ServiciosCD40.Recursos)nu[i]).Tipo == 6))
                    ListBox1.Items.Add(((ServiciosCD40.Recursos)nu[i]).IdRecurso);
            }
            else//TODOS
                ListBox1.Items.Add(((ServiciosCD40.Recursos)nu[i]).IdRecurso);
        }

		if (ListBox1.Items.Count > 0)
		{
			ActualizaWebPadre(true);

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
			TxtIdRecurso.Text = string.Empty;
		}

		ModoEdicion(false);
		/*
        ListBox1.SelectedIndex = elementoSeleccionado>=ListBox1.Items.Count ? ListBox1.Items.Count-1 : elementoSeleccionado;
		if (ListBox1.SelectedIndex < 0)
		{
			MostrarMenu(false);
			BtModificar.Visible = false;
		}
		else
		{
			MostrarElemento();
		}
		 */
    }
	protected override void AceptarCambios()
	{
		base.AceptarCambios();
	}

    protected override void CancelarCambios()
    {
		EsconderMenu();
		ModoEdicion(false);
        HabilitaElementos(false);
        HabilitarEscritura(true);

		//BtModificar.Visible = PermisoSegunPerfil;
		//BtNuevo.Enabled = PermisoSegunPerfil;
		//BtEliminar.Enabled = PermisoSegunPerfil;
		//ListBox1.Enabled = true;

        RecursoAsignado = false;

		MuestraDatos(DameDatos());
		
        if (ListBox1.SelectedIndex >= 0)
            MostrarElemento();
        else
            if (ListBox1.Items.Count > 0)
            {
                ListBox1.SelectedIndex = IndexListBox1 != -1 ? IndexListBox1 : 0;
                MostrarElemento();
            }
			//else
			//    MostrarMenu(false);
		TxtIdRecurso.Enabled = false;
		HabilitarValidacion(false);
    }

    private void GuardarCambios()
    {
        GrabaCambios();
	}

	private void GrabaCambios()
	{
		try
        {
            ServiciosCD40.Recursos n = new ServiciosCD40.Recursos();
            n.IdSistema = (string)Session["idsistema"];
            if (!Modificando) //Recurso nuevo
                n.IdRecurso = TxtIdRecurso.Text;
            else
                n.IdRecurso = ListBox1.SelectedValue;

            NewItem = n.IdRecurso;

            n.Interface = UInt16.Parse(DListTipo.SelectedValue) < 7 ? 0 : ServiciosCD40.TipoInterface.TI_DATOS;
            n.TipoRecurso = 0 ;  // Radio
            if (DListTipo.SelectedValue == "4") // M+N
                n.Tipo = (uint)(RBReceptor.Checked ? (RBTransmisor.Checked ? 6 : 4) : 5);
            else
                n.Tipo = UInt16.Parse(DListTipo.SelectedValue);
    
            //n.Diffserv = CheckDiffServ.Checked;
            n.ServidorSIP = TxtServidorSIP.Text;

            if (RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA) // TIFX --> Pasarela
			{
				n.IdTifX = DLTifx.SelectedItem.Text;
				n.SlotPasarela = NumInterfaceAsignado;
				n.NumDispositivoSlot = NumDispositivoAsignado;
				n.IdEquipo = null;
			}
			else
			{
				n.IdEquipo = DDLEquipoExternos.SelectedValue;
				n.IdTifX = null;
			}

			if (!Modificando) //Recurso nuevo
			{
				if (!ExisteElRecurso(n))
				{
					if (ServicioCD40.InsertSQL(n) < 0) // Inserta Recursos
						logDebugView.Warn("(RecursosDeRadio-GuardarCambios): No se ha podido insertar el elemento.");
					else
					{
						Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
						KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
						if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
						{
							SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
							switch (sincro.AltaCanalRadio(n.IdRecurso, 0, 'F', 'H', 'H', Int32.Parse(TxtTiempoPTT.Text)))
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
							if (DLDestino.SelectedValue != "0")
							{/*Comprobar que ese destino radio no tiene otro recurso con el mismo emplazamiento*/
								sincro.AltaFrecuencia(DListEmplazamiento.SelectedItem.Text, DLDestino.SelectedItem.Text, 0, n.IdRecurso);
							}
						}
					}
				
					InsertaParametrosRecurso(n);    // Inserta parámetros del recurso

                    TxtIdRecurso.Enabled = false;

					ActualizaWebPadre(true);

				}
				else
				{
					cMsg.alert((string)GetGlobalResourceObject("Espaniol","IdRecursoYaExiste"));
                    return;
				}
			}
			else
			{
                IndexListBox1 = ListBox1.SelectedIndex;
                
                if (ServicioCD40.UpdateSQL(n) < 0)
					logDebugView.Warn("(RecursosDeRadio-GuardarCambios): No se ha podido actualizar el elemento.");
				else
				{
					Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
					KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
					if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
					{
						SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
						switch (sincro.ModificacionCanalRadio(n.IdRecurso, 0, 'F', 'H', 'H', Int32.Parse(TxtTiempoPTT.Text)))
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
						if (DLDestino.SelectedValue != "0")
						{/*Comprobar que ese destino radio no tiene otro recurso con el mismo emplazamiento*/
							sincro.ModificacionFrecuencia(DLDestino.SelectedItem.Text, DListEmplazamiento.SelectedItem.Text, n.IdRecurso, 0);
						}
					}

					InsertaParametrosRecurso(n);    // Inserta parámetros del recurso
				}
			}
		}
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeRadio-GuardarCambios): ", ex);
        }  
        Modificando = false;

        RecursoAsignado = false;

        MuestraDatos(DameDatos());
        EsconderMenu();
		ModoEdicion(false);
        HabilitaElementos(false);
        HabilitarEscritura(true);
        ListBox1.Enabled = true;
        BtNuevo.Enabled = PermisoSegunPerfil;
        if (ListBox1.SelectedIndex >= 0)
            BtEliminar.Enabled = PermisoSegunPerfil;
        else
            BtEliminar.Enabled = false;
    }

	private bool ExisteElRecurso(ServiciosCD40.Recursos rTel)
	{
		ServiciosCD40.Recursos r = new ServiciosCD40.Recursos();
		r.IdSistema = rTel.IdSistema;
		r.IdRecurso = rTel.IdRecurso;
		r.TipoRecurso = 255;	//	DONT_CARE	
		ServiciosCD40.Tablas[] lista = ServicioCD40.ListSelectSQL(r);
		return lista.Length > 0;
	}

	private void EliminaDestinoDeTFT()
	{
		ServiciosCD40.Destinos drs = new ServiciosCD40.Destinos();
		drs.IdSistema = s.Value;
        drs.IdDestino = TBDestino.Text;
        drs.TipoDestino = 0;    // Tipo radio.

		// Eliminar de todos los TFT el destino
		ServicioCD40.DeleteSQL(drs);
	}

	public bool EsPosibleCambiarDestino()
	{
		return !DestinoAsignadoATft(TBDestino.Text) || (CuantosRecursosEnDestino(TBDestino.Text) > 1);
	}

	private int CuantosRecursosEnDestino(string destinoOriginal)
	{
		ServiciosCD40.RecursosRadio rr = new ServiciosCD40.RecursosRadio();

		rr.IdSistema = s.Value;
		rr.IdDestino = destinoOriginal;
		ServiciosCD40.Tablas[] lista = ServicioCD40.ListSelectSQL(rr);

		if (lista == null || lista.Length == 0)
			return 0;

		return lista.Length;
	}

	private bool DestinoAsignadoATft(string destino)
	{
		ServiciosCD40.DestinosRadioSector drs = new ServiciosCD40.DestinosRadioSector();
		drs.IdSistema = s.Value;
		drs.IdDestino = destino;

		ServiciosCD40.Tablas[] lista = ServicioCD40.ListSelectSQL(drs);

		if (lista != null && lista.Length > 0)
			return true;

		return false;
	}

    private void InsertaParametrosRecurso(ServiciosCD40.Recursos n)
    {
        try
        {
            ServiciosCD40.RecursosRadio prRad = new ServiciosCD40.RecursosRadio();
            ServiciosCD40.HFParams hFParam = new ServiciosCD40.HFParams();

            prRad.IdSistema = n.IdSistema;
            prRad.IdRecurso = n.IdRecurso;
            // TipoRecurso viene implícito en la clase a TipoRecurso_TR_TELEFONIA
            prRad.GananciaAGCTX = (uint)(RBGananciaTx.Checked ? 0 : 1);
            prRad.GananciaAGCTXdBm = Convert.ToSingle(TxtGananciaTx.Text);
            prRad.GananciaAGCRX = (uint)(RBGananciaRx.Checked ? 0 : 1);
            prRad.GananciaAGCRXdBm = Convert.ToSingle(TxtGananciaRx.Text);
            prRad.SupresionSilencio = CheckSupresionSilencio.Checked;
            prRad.TamRTP = Convert.ToUInt32(TxtTamanioPaquete.Text);
            prRad.Codec = (uint)(RBCodecA.Checked ? 0 : 1);
			prRad.KeepAlivePeriod = Convert.ToUInt32(TxtKAP.Text);
			prRad.KeepAliveMultiplier = Convert.ToUInt32(TxtKAM.Text);
            prRad.GrabacionEd137 = CheckGrabacionEd137.Checked;

			//if (TBDestino.Text != "Sin destino asociado")
            if (TBDestino.Text!=GetLocalResourceObject("TBDestinoResource1.Text").ToString())
			{
				prRad.IdDestino = TBDestino.Text;
			    prRad.TipoDestino = 0;
			}

			if (DListEmplazamiento.SelectedValue != "0" && DListEmplazamiento.SelectedValue != "")
				prRad.IdEmplazamiento = DListEmplazamiento.SelectedItem.Text;

            prRad.UmbralVAD = Int32.Parse(TxtUmbralVAD.Text);
            prRad.RepSQyBSS = UInt16.Parse(TxtRepSQyBSS.Text);
            prRad.TiempoPTT = UInt16.Parse(TxtTiempoPTT.Text);
            prRad.TimeoutPTT = UInt16.Parse(TxtActivacionPTT.Text);
            prRad.DesactivacionSQ = UInt16.Parse(TxtDesactivacionSQ.Text);
            prRad.UmbralTonoE = Int32.Parse(TxtUmbralE.Text);
            prRad.UmbralTonoM = Int32.Parse(TxtUmbralM.Text);
            prRad.UmbralTonoPTT = Int32.Parse(TxtUmbralPTT.Text);
            prRad.UmbralTonoSQ = Int32.Parse(TxtUmbralSQ.Text);
            prRad.EM = CheckEM.Checked;
            prRad.NTZ = CheckNTZ.Checked;
            prRad.EnableEventPttSq = CheckBoxEventosPTT_SQ.Checked;

            
            prRad.Cifrado = CheckCifrado.Checked;
            prRad.SupervPortadoraTx = CheckTX.Checked;
            prRad.SupervModuladoraTx = CheckRX.Checked;
            prRad.SQ = DListSQ.SelectedValue;
            prRad.PTT = DListPTT.SelectedValue;
            prRad.TipoNTZ = UInt16.Parse(DListTipoNTZ.SelectedValue);
            prRad.ModoConfPTT = UInt16.Parse(DListModoConfPTT.SelectedValue);
           
            prRad.NumFlujosAudio = UInt16.Parse(DListFlujosAudio.SelectedValue);

            //La tabla de calidad de radio solo tiene sentido si el recurso radio está asignado a una pasarela
            prRad.IdTablaBss = (RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA && DListTablasCalidad.SelectedIndex > 0) ? Convert.ToInt32(DListTablasCalidad.SelectedValue) : -1;
            prRad.Zonas_IdZonas = DListZonas.SelectedIndex > 0 ? Convert.ToInt32(DListZonas.Items[DListZonas.SelectedIndex].Value) : 0;

            //MVO: si se ha seleccionado algún elemento de la lista de métodos BSS
            if (!string.IsNullOrEmpty(DListMetodoBSS.SelectedValue))
            {
                prRad.Metodos_bss_idmetodos_bss = UInt16.Parse(DListMetodoBSS.SelectedValue);
            }
            prRad.GrsDelay = UInt16.Parse(TxtGRSDelay.Text);
            //prRad.CldSupervisionTime = Int32.Parse(TextBoxCLD.Text);            
            prRad.BSS = CheckBSS.Checked;
            prRad.GrabacionEd137 = CheckGrabacionEd137.Checked;
            prRad.Radio_param_idradio_param = var_id_radio_param;
            prRad.OffSetFrequency = Int32.Parse(DDLOffsetGeneral.SelectedValue);

           
            if (DListTipo.SelectedValue == "3" ||   // HF
                DListTipo.SelectedValue == "4" ||   // M+N RX
                DListTipo.SelectedValue == "5" ||   // M+N TX
                DListTipo.SelectedValue == "6")     // M+N TXRX
            {
                hFParam.IdSistema = n.IdSistema;
                hFParam.IdRecurso = n.IdRecurso;
                hFParam.IpGestor = (DListTipo.SelectedValue == "3") ? TBIpGestor.Text : TextBox1.Text;
                hFParam.Oid = (DListTipo.SelectedValue == "3") ? TBOid.Text : TextBox2.Text;

                /*
                // El compone el campo sipUri con el siguiente formato <sip:idRecurso@DireccionIP>. El campo DireccionIP puede o no contener el puerto SIP (X.X.X.X:NumPuertoSIP)
                string strSipUri = string.Empty;

                if (!string.IsNullOrEmpty(n.IdTifX) && listaIdTifx_IpSip.ContainsKey(n.IdTifX))
                {
                    //Si el recurso está asignado a una pasarela se indica la dirección IP de la pasarela.
                    if (listaIdTifx_IpSip.ContainsKey(n.IdTifX))
                        strSipUri = string.Format("<sip:{0}@{1}>", n.IdRecurso, listaIdTifx_IpSip[n.IdTifX]);
                }
                else if (!string.IsNullOrEmpty(n.IdEquipo) && listaEquipoEu_IpSip.ContainsKey(n.IdEquipo))
                {
                    //Si el recurso está asignado a un equipo externo se indica la dirección IP del equipo externo.
                    strSipUri = string.Format("<sip:{0}@{1}>", n.IdRecurso, listaEquipoEu_IpSip[n.IdEquipo]);
                }


                hFParam.SipUri = strSipUri;
                */
                hFParam.TipoCanal = RBPrincipal.Checked ? 0 : 1;
                hFParam.TipoEquipo = RBReceptor.Checked ? (RBTransmisor.Checked ? 2 : 0) : 1;
                hFParam.TipoFrecuencia = RBHF.Checked ? 0 : (RBVHF.Checked ? 1 : 2);

                if (RBPrincipal.Checked)
                {
                    hFParam.TipoModo = 0;
                    hFParam.PrioridadEquipo = Convert.ToInt32(TBPriority.Text);
                }
                else
                {
                    //Si es un equipo de reserva, la prioridad del equipo debe tomar un valor superior a prioridad del principal (que puede tomar un valor entre 0-100),
                    // Por lo que por defecto, le ponemos  el valor 101
                    hFParam.TipoModo = 1;
                    hFParam.PrioridadEquipo = 101;
                }
                
                //MVO. Si en los decimales se introducen espacios en blanco, se sustituye por 000
                if (string.IsNullOrWhiteSpace(TBTunedDecimals.Text))
                    TBTunedDecimals.Text="000";

                //hFParam.Frecuencia = DListTipo.SelectedValue != "3" ? (TBTunedFrequency.Text + '.' + TBTunedDecimals.Text.Substring(0,3)) : string.Empty;
                hFParam.Frecuencia = DListTipo.SelectedValue != "3" ? (TBTunedFrequency.Text + '.' + TBTunedDecimals.Text) : string.Empty;

                // El campo puerto está oculto y se configura con los siguientes valores por defecto
                switch (DDLMarca.SelectedValue)
                {
                    case EQUIPO_M_N_ROHDE:
                        if (RBTransmisor.Checked)
                        {
                            // Transmisores 'ROHDE'=> 161
                            hFParam.Puerto = PUERTO_TX_ROHDE;
                        }
                        else if (RBReceptor.Checked)
                        {
                            // Receptores 'ROHDE' => 160
                            hFParam.Puerto = PUERTO_RX_ROHDE;
                        }
                        else if (RBTransceptor.Checked)
                        {
                            hFParam.Puerto = PUERTO_TX_ROHDE;
                        }
                        break;
                    case EQUIPO_M_N_JOTRON:
                        // Transmisores y Receptores 'JOTRON' => 161
                        hFParam.Puerto = PUERTO_TXRX_JOTRON;
                        break;
                    default:
                        hFParam.Puerto = Convert.ToUInt32(TbPuerto.Text);
                        break;
                }

                hFParam.Offset = Convert.ToInt32(DDLOffsetGeneral.SelectedValue);
                hFParam.Canalizacion = Convert.ToInt32(DDLCanalizacion.SelectedValue);
                hFParam.Modulacion = Convert.ToInt32(DDLModulacion.SelectedValue);
                //hFParam.Potencia = Convert.ToInt32(DDLPotencia.SelectedValue);
                hFParam.Potencia = 2; //Por defecto, la potencia se configura con Normal=2 para todos
                hFParam.ModeloEquipo = Convert.ToInt32(DDLMarca.SelectedValue);
            }

            // Inserta ParametrosRecurso y RecursosRD
            if (!Modificando) //Recurso nuevo
            {
                if (ServicioCD40.InsertSQL(prRad) < 0) // Inserta Recursos           
                    logDebugView.Warn("(RecursosDeRadio-InsertaParametrosRecurso): No se ha podido insertar el elemento " + prRad.IdRecurso);
                else
                {
                    // Insertar parametros HF
                    if (n.Tipo == 3 || n.Tipo == 4 || n.Tipo == 5 || n.Tipo == 6) // TX-HF || N+M
                    {
                        ServicioCD40.InsertSQL(hFParam);
                        InsertaRangosFrecuenciaHF(n);
                    }

                    //if (TBDestino.Text != "Sin destino asociado")
                    if (TBDestino.Text != GetLocalResourceObject("TBDestinoResource1.Text").ToString())
                    {
                        ServiciosCD40.Tablas[] listaRecursos = new ServiciosCD40.Tablas[1];
                        listaRecursos[0] = (ServiciosCD40.Tablas)prRad;
                        ServicioCD40.GeneraEstadosRecursosParaUnDestino(listaRecursos);
                    }
                }
            }
			else
			{
				int numRows = ServicioCD40.UpdateSQL(prRad);
				if (numRows < 0)	// Modifica Recurso y ParametrosRecurso 
                    logDebugView.Warn("(RecursosDeRadio-InsertaParametrosRecurso): No se ha podido actualizar el elemento " + prRad.IdRecurso);
				else if (numRows == 0)	// No se ha podido modificar (¿?) pues se inserta
					ServicioCD40.InsertSQL(prRad);

                if (iTipoRecursoOriginal != n.Tipo && 
                    (n.Tipo == 0 || n.Tipo == 1 || n.Tipo == 2) &&
                    (iTipoRecursoOriginal == 3 || iTipoRecursoOriginal == 4 || iTipoRecursoOriginal == 5 || iTipoRecursoOriginal == 6))
                {
                    //Si se ha cambiado el tipo del recurso a Audio RX(0),Audio TX(1)o Audio RXTX(2) y el original era HF o M+N
                    //Se debe eliminar el registro de la tabla hFParam
                    hFParam.IdSistema = n.IdSistema;
                    hFParam.IdRecurso = n.IdRecurso;
                    ServicioCD40.DeleteSQL(hFParam);    // La FK hace que se borre también en la tabla hfRangoFrecuencias
                }
                else if (n.Tipo == 3 || n.Tipo == 4 || n.Tipo == 5 || n.Tipo == 6) // TX-HF || N+M
                {
                    ServicioCD40.DeleteSQL(hFParam);    // La FK hace que se borre también en la tabla hfRangoFrecuencias
                    ServicioCD40.InsertSQL(hFParam);
                    InsertaRangosFrecuenciaHF(n);
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeRadio-InsertaParametrosRecurso): ", ex);
        } 
        HabilitarValidacion(false);
    }

    private void InsertaRangosFrecuenciaHF(ServiciosCD40.Recursos n)
    {
        ServiciosCD40.HFRangoFrecuencias hfRango = new ServiciosCD40.HFRangoFrecuencias();

        hfRango.IdSistema = n.IdSistema;
        hfRango.IdRecurso = n.IdRecurso;

        ServicioCD40.DeleteSQL(hfRango);

        foreach (DataRow dsRango in DSRangosFrecuencias.Tables[0].Rows)
        {
            hfRango.Min = Convert.ToUInt32(dsRango[2].ToString());
            hfRango.Max = Convert.ToUInt32(dsRango[3].ToString());
            
            ServicioCD40.InsertSQL(hfRango);
        }
    }
    
    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        LError.Text = string.Empty;

        string StrSistema = (string)Session["idsistema"];

        if (!Modificando && TxtIdRecurso.Enabled && bIdentificadorAsignado(StrSistema, TxtIdRecurso.Text))
        {
            //MVO: Si se está insertando un recurso radio y existe otro recurso con el mismo identificador en el sistema, se informa al usuario para que indique otro identificador
            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorRecurso_Existente"), TxtIdRecurso));
        }
        else if (DListZonas.SelectedIndex == 0)
            LError.Text = (string)GetGlobalResourceObject("Espaniol", "AvisoRecursoSinZonas");
        else if (DListEmplazamiento.SelectedIndex == 0)
            LError.Text = (string)GetGlobalResourceObject("Espaniol", "AvisoRecursoSinEmplazamiento");
        // Tabla de conversión a indices RSSI sólo si está el BSS seleccionado
        else if (CheckBSS.Checked && DListTablasCalidad.SelectedIndex == 0)
            LError.Text = (string)GetGlobalResourceObject("Espaniol", "AvisoRecursoTablaCalificacion");
        else
        {
            //MVO:Si el recurso es de tipo Audio M+N sólo se puede asignar a un equipo externo
            if ((DListTipo.SelectedValue == "4" || DListTipo.SelectedValue == "5" || DListTipo.SelectedValue == "6") &&
               (HW_TIPO_EQUIPO_EXTERNO != RBLTipoEquipo.SelectedValue || DDLEquipoExternos.SelectedIndex == 0))
            {
                LError.Text = (string)GetGlobalResourceObject("Espaniol", "AvisoCnfHwRecursoRadioMN_EquipExt");
            }
            else
            {
                //MVO: Si el recurso está asignado a una pasarela debe estar asignado en algún slot o si está asignado a un equipo externo, se debe elegir algún equipo externo
                if ((HW_TIPO_PASARELA == RBLTipoEquipo.SelectedValue && RecursoAsignado) || (HW_TIPO_EQUIPO_EXTERNO == RBLTipoEquipo.SelectedValue && DDLEquipoExternos.SelectedIndex != 0))
                {
                    //Si el recurso está asignado a una pasarela y el recurso es de tipo Audio RX (0) y Audio RXTX (2), el campo CheckGrabacionEd137
                    // sólo puede estar marcado, si la pasarela tiene configurada algún grabador
                    if ((RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA && (DListTipo.SelectedValue == "0" || DListTipo.SelectedValue == "2")) && CheckGrabacionEd137.Checked && !bPasarelaConGrabadores(DLTifx.SelectedValue))
                    {
                        //El campo CheckGrabacionEd137.Checked sólo puede estar marcado, si la pasarela tiene configurada algún grabador
                        LError.Text = string.Format((string)GetGlobalResourceObject("Espaniol", "AvisoRecursoRadioCheckGrabacionED137"), DLTifx.SelectedValue);
                    }
                    else
                    {
                        //MVO: el campo CheckGrabacionEd137 sólo es visible para los tipo Audio RX (0) y Audio RXTX (2) cuando el recurso esté asignado a una pasarela.
                        // Por lo que si no se cumple esta condición se quita el tick del campo CheckGrabacionEd137.
                        if ((RBLTipoEquipo.SelectedValue == HW_TIPO_EQUIPO_EXTERNO) || ((RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA && DListTipo.SelectedValue != "0" && DListTipo.SelectedValue != "2")))
                        {
                            if (CheckGrabacionEd137.Checked)
                                CheckGrabacionEd137.Checked = false;
                        }

                        GuardarCambios();
                    }
                }
                else
                    LError.Text = (string)GetGlobalResourceObject("Espaniol", "AvisoRecurso");
            }

        }
    }

    protected void BtModificar_Click(object sender, EventArgs e)
    {
		Modificando = true;
        RecursoAsignado = false;
        IndexListBox1 = ListBox1.SelectedIndex;

        TxtIdRecurso.Enabled = false;

		ModoEdicion(true);  
        HabilitarEscritura(false);
        

        CargaRecursosDeLaTifx(DLTifx.Text);
        
        //Los elementos se deben habilitar, despues de cargar lo recursos de la Tif para que funcione correctamente
        HabilitaElementos(true);

        //MVO: si el recurso está asociado a un destino con FD, no se permite modificar
        // el emplazamiento
        if (bRecursoConDestinoFD)
        {
            DListEmplazamiento.Enabled = false;
        }
        else
        {   
            DListEmplazamiento.Enabled = true;
        }

        HabilitarValidacion(true);
	}

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        //MVO: si el recurso está asociado a un destino con FD, no se permite eliminar el recurso.
        //     Previamente, el usuario debe eliminarlo del destino Radio 
        if (bRecursoConDestinoFD)
        {
            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "AvisoRecursoRadioCon_DestinoFD"));
        }
        else
        {
            EsconderMenu();
            if (ListBox1.SelectedValue != "")
            {
                IndexListBox1 = ListBox1.SelectedIndex;
                //string texto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarRecurso"), ListBox1.SelectedValue);
                Session["elemento"] = ListBox1.SelectedValue;
                EliminarElemento();
                //cMsg.confirm(texto, "eliminaelemento");
            }
        }
    }

    private void EliminarElemento()
    {
		if (EsPosibleCambiarDestino())
		{
			BorrarRecurso();
            MuestraDatos(DameDatos());
        }
		else
		{
			// Es el único recurso para ese destino 
			cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "PanelesConDestinoAsignado"), "SoloEliminaDeTFT");
			//Si el usuario acepta => EliminaDestinoDeTFT()
		}
	}

	private void BorrarRecurso()
	{
		try
		{
			ServiciosCD40.Recursos n = new ServiciosCD40.Recursos();
			n.IdSistema = (string)Session["idsistema"];
			n.IdRecurso = (string)Session["elemento"];
			n.TipoRecurso = 0;  // RD
			if (ServicioCD40.DeleteSQL(n) < 0)
				logDebugView.Warn("(RecursosDeRadio-EliminarElemento): No se ha borrado el elemento.");
            else
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    switch (sincro.BajaCanalRadio(n.IdRecurso))
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
                else
                    cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
            }
			
		}
		catch (Exception ex)
		{
			logDebugView.Error("(RecursosDeRadio-EliminarElemento): ", ex);
		}
	}

    protected void DDLTifx_SelectedIndexChanged(object sender, EventArgs e)
    {
        CargaRecursosDeLaTifx(DLTifx.SelectedItem.Text);
    }

    private void CargaDDLEquiposExternos()
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
        Session["idsistema"] = s.Value;

        string strFirstItemRecorder = string.Empty;
        string strIPPuertoSip=string.Empty;
        strFirstItemRecorder = GetLocalResourceObject("DDLEquipoExternosItem1").ToString();

        ServiciosCD40.EquiposEU objEquiposExternos = new ServiciosCD40.EquiposEU();
        objEquiposExternos.IdSistema = s.Value;
        objEquiposExternos.TipoEquipo = (uint)ServiciosCD40.Tipo_Elemento_HW.TEH_EXTERNO_RADIO;

        if (DDLEquipoExternos.Items.Count>0)
            DDLEquipoExternos.Items.Clear();

        /*
        if (listaEquipoEu_IpSip.Count > 0)
            listaEquipoEu_IpSip.Clear();
         */

        // Se añade el item DDLEquipoExternos.Items.Add("< Ninguno >");
        DDLEquipoExternos.Items.Add(strFirstItemRecorder);

        DDLEquipoExternos.DataSource = ServicioCD40.DataSetSelectSQL(objEquiposExternos);
        DDLEquipoExternos.DataTextField = "IdEquipos";
        DDLEquipoExternos.DataValueField = "IdEquipos";
        DDLEquipoExternos.DataBind();

        /*
        if (DDLEquipoExternos.DataSource!=null && ((DataSet)DDLEquipoExternos.DataSource).Tables.Count > 0)
        {
            foreach (DataRow t in ((DataSet)DDLEquipoExternos.DataSource).Tables[0].Rows)
            {
                string strIdEquipo = t["IdEquipos"].ToString();
                string strIp = string.Empty;

                strIp = t["IpRed1"].ToString();

                //Se compone IP:PuertoSip para almacenarlo en el mapa listaEquipoEu_IpSip
                if (t["SipPort"] != null && !string.IsNullOrEmpty( t["SipPort"].ToString()))
                    strIPPuertoSip = string.Format("{0}:{1}", strIp, t["SipPort"].ToString());
                else
                    strIPPuertoSip = strIp;

                if (!listaEquipoEu_IpSip.ContainsKey(strIdEquipo))
                    listaEquipoEu_IpSip.Add(strIdEquipo, strIPPuertoSip);
                else
                    listaEquipoEu_IpSip[strIdEquipo] = strIPPuertoSip;
            }
        }
        */
    }

    private void CargaDDLTifX()
    {
        string strIPPuertoSip=string.Empty;
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
        Session["idsistema"] = s.Value;
        
        ServiciosCD40.TifX tifx = new ServiciosCD40.TifX();
        tifx.IdSistema = s.Value;

        if (DLTifx.Items.Count>0)
            DLTifx.Items.Clear();

        /*
        if (listaIdTifx_IpSip.Count > 0)
            listaIdTifx_IpSip.Clear();
         */

        DLTifx.DataSource = ServicioCD40.DataSetSelectSQL(tifx);
        if (DLTifx.DataSource == null || ((DataSet)DLTifx.DataSource).Tables.Count == 0)
            return;

        DLTifx.DataTextField = "IdTifX";
        DLTifx.DataValueField = "IdTifx";
        DLTifx.DataBind();
        if (DLTifx.SelectedItem!=null)
            CargaRecursosDeLaTifx(DLTifx.SelectedItem.Text);

        /*
        foreach (DataRow t in ((DataSet)DLTifx.DataSource).Tables[0].Rows)
        {
            ServiciosCD40.GwActivas gw = new ServiciosCD40.GwActivas();
            string strIdPasarela= t["IdTifx"].ToString();

            gw.IdSistema = s.Value;
            gw.IdTifx = strIdPasarela;

            ServiciosCD40.Tablas[] tGw = ServicioCD40.ListSelectSQL(gw);

            if (tGw != null && tGw.Length > 0)
            {
                string strIp=string.Empty;

                strIp= ((ServiciosCD40.GwActivas)tGw[0]).IpRed;

                if (t["SIPPortLocal"] != null && !string.IsNullOrEmpty(t["SIPPortLocal"].ToString()))
                {
                    //Se compone IP:PuertoSip para almacenarlo en el mapa listaIdTifx_IpSip
                    strIPPuertoSip = string.Format("{0}:{1}", strIp, t["SIPPortLocal"].ToString());
                }
                else
                    strIPPuertoSip = strIp;

                if (!listaIdTifx_IpSip.ContainsKey(strIdPasarela))
                    listaIdTifx_IpSip.Add(strIdPasarela, strIPPuertoSip);
                else
                    listaIdTifx_IpSip[strIdPasarela]=strIPPuertoSip;
            }
        }
         */
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

			ServiciosCD40.Tablas[] listaRecursosEnTifX = ServicioCD40.ListSelectSQL(recurso);

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
            logDebugView.Error("(RecursosDeRadio-CargaRecursosDeLaTifx): ", ex);
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
        // Si el recurso ya está asignado no se puede seleccionar otra interface
        // hasta no liberar la ya seleccionada
        if ((((CheckBox)sender).Checked && RecursoAsignado) || DDLEquipoExternos.SelectedIndex != 0)
        {
            ((CheckBox)sender).Checked = false;
            return;
        }

        bool asignar = true;
        int row = Convert.ToInt32(((CheckBox)sender).ID.Substring(8, 1)) - 1;
        int cell = Convert.ToInt32(((CheckBox)sender).ID.Substring(9, 1)) - 1;

        asignar = EsPosibleAsignarGenerico(cell);
        if (!asignar)
            ((CheckBox)sender).Checked = !((CheckBox)sender).Checked;

        RecursoAsignado = ((CheckBox)sender).Checked;
		RBLTipoEquipo.Enabled = DLTifx.Enabled = !RecursoAsignado;
		
        if (asignar && RecursoAsignado)
        {
            NumInterfaceAsignado = cell;
            NumDispositivoAsignado = row;
        }
        else
            NumDispositivoAsignado = NumInterfaceAsignado = -1;

        // No dejar cambiar de tifx mientras se está en proceso de asignación de un recurso a una GateWay
        DLTifx.Enabled = !RecursoAsignado;
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
					ServiciosCD40.Tablas[] listaRecurso = ServicioCD40.ListSelectSQL(unRecurso);
                    if (listaRecurso.Length > 0)
                        if (((ServiciosCD40.Recursos)listaRecurso[0]).Interface == ServiciosCD40.TipoInterface.TI_ISDN_2BD)
                            return false;
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(RecursosDeRadio-EsPosibleAsignarGenerico): ", ex);
        }
        return true;
    }

    private void HabilitarEscritura(bool valor)
    {
        //TxtServidorSIP.Enabled = !valor;
        //TxtTamanioPaquete.Enabled = !valor;
        TxtGananciaRx.Enabled = !valor;
        TxtGananciaTx.Enabled = !valor;
        TxtUmbralE.Enabled = !valor;
        TxtUmbralM.Enabled = !valor;
        TxtUmbralPTT.Enabled = !valor;
        TxtUmbralSQ.Enabled = !valor;
        TxtTiempoPTT.Enabled = !valor;
        TxtDesactivacionSQ.Enabled = !valor;
        TxtActivacionPTT.Enabled = !valor;
        TxtRepSQyBSS.Enabled = !valor;
        TxtUmbralVAD.Enabled = !valor;
		TxtGRSDelay.Enabled = !valor; //MAF
		//TxtKAM.Enabled = !valor;
		//TxtKAP.Enabled = !valor;
	}
    
    private void HabilitarValidacion(bool valor)
    {
        RequiredFieldValidator2.Enabled = valor;
        ValidationSummary1.Visible = valor;
        RequiredFieldValidator1.Visible = valor;
		//RequiredFieldValidator2.Visible = valor;
        RequiredFieldValidator3.Visible = valor;
        RequiredFieldValidator4.Visible = valor;
        RequiredFieldValidator5.Visible = valor;
        RequiredFieldValidator6.Visible = valor;
        RequiredFieldValidator7.Visible = valor;
        RequiredFieldValidator8.Visible = valor;
        RequiredFieldValidator9.Visible = valor;
        RequiredFieldValidator10.Visible = valor;
        RequiredFieldValidator11.Visible = valor;
        RequiredFieldValidator12.Visible = valor;
        RequiredFieldValidator13.Visible = valor;
        RequiredFieldValidator14.Visible = valor;
        RequiredFieldValidator15.Visible = valor;
        RequiredFieldValidator16.Visible = valor;
        CompareValidator1.Visible = valor;
        CompareValidator2.Visible = valor;
        CompareValidator3.Visible = valor;
        CompareValidator4.Visible = valor;
        CompareValidator5.Visible = valor;
        CompareValidator6.Visible = valor;
        CompareValidator7.Visible = valor;
        CompareValidator8.Visible = valor;
        CompareValidator9.Visible = valor;
        CompareValidator10.Visible = valor;
        CompareValidator11.Visible = valor;
        CompareValidator12.Visible = valor;
		RequiredFieldValidator15.Visible = valor;
		RequiredFieldValidator16.Visible = valor;
		RangeValidator1.Visible = valor;
		RangeValidator2.Visible = valor;
		//LError.Visible = valor;
    }

	protected void RBLTipoEquipo_SelectedIndexChanged(object sender, EventArgs e)
	{
		MuestraEquipoDelRecurso();
	}

	private void MuestraEquipoDelRecurso()
	{
        if (RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA)	// TIFX --> pasarela
		{
			Label14.Visible = true;
			DLTifx.Visible = DLTifx.Enabled = true;
			TTifx.Visible = true;
			LblEquipoExterno.Visible = false;
			DDLEquipoExternos.Visible = false;


            CheckBoxEventosPTT_SQ.Visible = true;
            CheckBoxEventosPTT_SQ.Checked = false;


            // (DListTipo.SelectedIndex >= 4 && DListTipo.SelectedIndex <= 6)
            if (DListTipo.SelectedValue == "4" || DListTipo.SelectedValue == "5" || DListTipo.SelectedValue == "6")     // M+N
            {
                IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = false; // AUDIO N+M;
                IBVoip.CssClass = "buttonImageDisabled";
                IBAudio.CssClass = "buttonImageDisabled";
                IBFuncionalidad.CssClass = "buttonImageDisabled";
            }
            else
            {
                IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = true; // AUDIO N+M;
                IBVoip.CssClass = "buttonImage";
                IBAudio.CssClass = "buttonImage";
                IBFuncionalidad.CssClass = "buttonImage";
            }
        }
		else
		{
			Label14.Visible = false;
			DLTifx.Visible = false;
			TTifx.Visible = false;
			LblEquipoExterno.Visible = true;
			DDLEquipoExternos.Visible = true;

            IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = false; // AUDIO N+M;
            IBVoip.CssClass = "buttonImageDisabled";
            IBAudio.CssClass = "buttonImageDisabled";
            IBFuncionalidad.CssClass = "buttonImageDisabled";

            CheckBoxEventosPTT_SQ.Visible = false;
            CheckBoxEventosPTT_SQ.Checked = false;
        }
	}

    private void MostrarParametrosFuncionalidad()
    {
        if (DListTipo.SelectedValue == "0")  //Audio RX
        {
            Panel5.Visible = false;
            Panel6.Visible = true;
            Panel3.Visible = true; //Grupo BSS visible
        }
        else if (DListTipo.SelectedValue == "1") //Audio TX
        {
            Panel6.Visible = false;
            Panel3.Visible = false;
            Panel5.Visible = true;
        }
        else if (DListTipo.SelectedValue == "2") //Audio RX TX
        {
            Panel5.Visible = true;
            Panel6.Visible = true;
            Panel3.Visible = true; //Grupo BSS visible
        }
        else
        {
            Panel5.Visible = true;
            Panel6.Visible = true;
            Panel3.Visible = false; // Grupo BSS

        }
    
    }



	protected void DDLEquipoExternos_SelectedIndexChanged(object sender, EventArgs e)
	{
		RBLTipoEquipo.Enabled = DDLEquipoExternos.SelectedIndex == 0;
	}

	protected void OnButton_Click(object sender, EventArgs e)
	{
		Button ibSelected = (Button)sender;

		switch (ibSelected.ID)
		{
			case "IBGenerales":
                IBGenerales.CssClass = "buttonImageSelected";
                //MVO: Se comprueba con el valor seleccionado y no la posición
                //if ((DListTipo.SelectedIndex >= 4 && DListTipo.SelectedIndex <= 6) ||   //M+N
                if ((DListTipo.SelectedValue == "4" ||DListTipo.SelectedValue == "5"||DListTipo.SelectedValue == "6") ||   //M+N 
                    RBLTipoEquipo.SelectedValue == HW_TIPO_EQUIPO_EXTERNO)                                   // Equipos externos
                {
                    IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = false; // AUDIO N+M;
                    IBVoip.CssClass = "buttonImageDisabled";
                    IBAudio.CssClass = "buttonImageDisabled";
                    IBFuncionalidad.CssClass = "buttonImageDisabled";
                }
                else
                {
                    IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = true; // AUDIO N+M;
                    IBVoip.CssClass = "buttonImage";
                    IBAudio.CssClass = "buttonImage";
                    IBFuncionalidad.CssClass = "buttonImage";
                }

                //IBVoip.CssClass = "buttonImage";
                //IBAudio.CssClass = "buttonImage";
                IBAsignacion.CssClass = "buttonImage";
                //IBFuncionalidad.CssClass = "buttonImage";
                
				MultiView1.ActiveViewIndex = NumPaginaActiva = 0;
				Panel1.Width = 700;
				Panel1.Height = 450;
				break;
			case "IBVoip":
                IBGenerales.CssClass = "buttonImage";
                IBVoip.CssClass = "buttonImageSelected";
                IBAudio.CssClass = "buttonImage";
                IBAsignacion.CssClass = "buttonImage";
                IBFuncionalidad.CssClass = "buttonImage";

                MultiView1.ActiveViewIndex = NumPaginaActiva = 1;
				Panel1.Width = 525;
				Panel1.Height = 385;
				break;
			case "IBAudio":
                IBGenerales.CssClass = "buttonImage";
                IBVoip.CssClass = "buttonImage";
                IBAudio.CssClass = "buttonImageSelected";
                IBAsignacion.CssClass = "buttonImage";
                IBFuncionalidad.CssClass = "buttonImage";

                MultiView1.ActiveViewIndex = NumPaginaActiva = 2;
				Panel1.Width = 570;
				Panel1.Height = 150;
				break;
			case "IBFuncionalidad":
                IBGenerales.CssClass = "buttonImage";
                IBVoip.CssClass = "buttonImage";
                IBAudio.CssClass = "buttonImage";
                IBAsignacion.CssClass = "buttonImage";
                IBFuncionalidad.CssClass = "buttonImageSelected";

                MultiView1.ActiveViewIndex = NumPaginaActiva = 3;
				Panel1.Width = 620;
				Panel1.Height = 450;

                //Si el recurso es de tipo RX o RxTx, se puede configurar el grupo BSS
                if (DListTipo.SelectedValue == "0" || DListTipo.SelectedValue == "2")
                {
                    Panel3.Visible = true;
                    CheckBSS.Visible = true;
                }
                else
                {
                    Panel3.Visible = false;
                    CheckBSS.Visible = false;
                }
                

                //MVO: el campo CheckGrabacionEd137 sólo será visible para los tipo Audio RX (0) y Audio RXTX (2) cuando el recurso esté asignado a una pasarela
                if ((DListTipo.SelectedValue == "0" || DListTipo.SelectedValue == "2") && null != DLTifx.SelectedItem && RBLTipoEquipo.SelectedValue == HW_TIPO_PASARELA &&
                    ((Modificando && RecursoAsignado) || (!Modificando && 0 == DDLEquipoExternos.SelectedIndex && ((Editando && RecursoAsignado) || !Editando)))
                   )
                {
                    if (!CheckGrabacionEd137.Visible)
                        CheckGrabacionEd137.Visible = true;
                }
                else
                {
                    if (CheckGrabacionEd137.Visible)
                    {
                        CheckGrabacionEd137.Visible = false;

                        if (CheckGrabacionEd137.Checked)
                            CheckGrabacionEd137.Checked = false;
                    }

                }

                if (!CheckBSS.Visible) 
                    CheckBSS.Checked = false;

                if (CheckBSS.Checked)
                {
                    DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = true;    // String.Compare(DListMetodoBSS.Items[DListMetodoBSS.SelectedIndex].Text, METODO_BSS_NUCLEO) == 0;
                    //TableCalidad.Visible = DListTablasCalidad.Visible && DListTablasCalidad.SelectedIndex != 0;
                    MostrarValoresTablasBss(Convert.ToInt32(DListTablasCalidad.SelectedValue));
                }
                else
                {
                    //DListTablasCalidad.SelectedIndex = 0;
                    //DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = TableCalidad.Visible = false;
                    DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = false;
                }
                TableCalidad.Visible = CheckBSS.Checked && DListTablasCalidad.SelectedIndex != 0;

                
				break;
			case "IBAsignacion":
                IBGenerales.CssClass = "buttonImage";
                if ((DListTipo.SelectedValue == "4" || DListTipo.SelectedValue == "5" || DListTipo.SelectedValue == "6") ||   //M+N y equipo externo
                    RBLTipoEquipo.SelectedValue == HW_TIPO_EQUIPO_EXTERNO)                                   // Equipos externos
                {
                    IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = false; // AUDIO N+M;
                    IBVoip.CssClass = "buttonImageDisabled";
                    IBAudio.CssClass = "buttonImageDisabled";
                    IBFuncionalidad.CssClass = "buttonImageDisabled";
                }
                else
                {
                    IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = true; // AUDIO N+M;
                    IBVoip.CssClass = "buttonImage";
                    IBAudio.CssClass = "buttonImage";
                    IBFuncionalidad.CssClass = "buttonImage";
                }
                
                //IBVoip.CssClass = "buttonImage";
                //IBAudio.CssClass = "buttonImage";
                IBAsignacion.CssClass = "buttonImageSelected";
                //IBFuncionalidad.CssClass = "buttonImage";

                MultiView1.ActiveViewIndex = NumPaginaActiva = 4;
				Panel1.Width = 525;
				Panel1.Height = 300;
				break;
		}
	}

    protected void BtnAnadirRango_Click(object sender, EventArgs e)
    {
        // CargaGVRangos();

        // Antes de añadir el rango al grid, comprobar que el valor mínimo corresponde con un valor numérico
        try
        {
            Convert.ToInt32(TBMin.Text);
        }
        catch (System.FormatException)
        {
            TBMin.Text = "*";
            return;
        }

        // Antes de añadir el rango al grid, comprobar que el valor máximo corresponde con un valor numérico
        try
        {
            Convert.ToInt32(TBMax.Text);
        }
        catch (System.FormatException)
        {
            TBMax.Text = "*";
            return;
        }

        if (DSRangosFrecuencias.Tables.Count > 0)
        {
            DataRow dr = DSRangosFrecuencias.Tables[0].NewRow();
            dr[2] = TBMin.Text;
            dr[3] = TBMax.Text;
            DSRangosFrecuencias.Tables[0].Rows.Add(dr);
            DSRangosFrecuencias.AcceptChanges();

            GVRangos.DataSource = DSRangosFrecuencias;
            GVRangos.DataBind();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DListTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Accordion1.Visible = DListTipo.SelectedValue == "3"; // AUDIO RX TX HF
                
        Label24.Visible = DListMetodoBSS.Visible = (CheckBSS.Visible && CheckBSS.Checked);

        if (DListTipo.SelectedValue == "4" || DListTipo.SelectedValue == "5" || DListTipo.SelectedValue == "6") //M+N
        {
            Accordion2.Visible = true;
            
            OnButton_Click(IBGenerales, null);

            ConfiguraOpcionesM_N(null, null);
            
            if (Modificando)
            {
                //Se avisa al usuario que revise la configuración HW, por si el recurso estuviera asignado a una pasarela.
                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "AvisoCnfHwRecursoRadioMN"));
            }
        }
        else
        {
            Accordion2.Visible = false;
            IBVoip.Enabled = IBAudio.Enabled = IBFuncionalidad.Enabled = true; 
            IBVoip.CssClass = "buttonImage";
            IBAudio.CssClass = "buttonImage";
            IBFuncionalidad.CssClass = "buttonImage";
            //Se actualiza la configuración de la pestaña funcionalidad en función del tipo de recurso
            MostrarParametrosFuncionalidad();

        }
    }

  //MAF
    /*protected void DListAgrupamientos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DListAgrupamientos.SelectedIndex == 0)
        {
            TBGrupoRecurso.Text = "FS" + " (" + TxtIdRecurso.Text + ")";
        
        }

    }*/

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GVRangos_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DSRangosFrecuencias.Tables[0].Rows[GVRangos.PageIndex * GVRangos.PageSize + e.RowIndex].Delete();
        DSRangosFrecuencias.AcceptChanges();

        GVRangos.DataSource = DSRangosFrecuencias;
        GVRangos.DataBind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GVRangos_OnSelectedIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVRangos.PageIndex = e.NewPageIndex;
        GVRangos.DataSource = DSRangosFrecuencias;
        GVRangos.DataBind();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /*
    protected void GVRangosNM_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DSRangosFrecuencias.Tables[0].Rows[GVRangos_NM.PageIndex * GVRangos.PageSize + e.RowIndex].Delete();
        DSRangosFrecuencias.AcceptChanges();

        GVRangos_NM.DataSource = DSRangosFrecuencias;
        GVRangos_NM.DataBind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GVRangosNM_OnSelectedIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVRangos_NM.PageIndex = e.NewPageIndex;
        GVRangos_NM.DataSource = DSRangosFrecuencias;
        GVRangos_NM.DataBind();
    }
    */

    protected void RbConfiguracionBasica_CheckedChanged(object sender, EventArgs e)
    {
        if (((RadioButton)sender).Checked)
        {
            Label6.Visible = DListTipo.Visible = true;
            Accordion2.Visible = false;
        }
    }
    protected void RbConfiguracionNM_CheckedChanged(object sender, EventArgs e)
    {
        if (((RadioButton)sender).Checked)
        {
            Label6.Visible = DListTipo.Visible = false;
            Accordion2.Visible = true;
        }
    }

    protected void Checked_FrequencyType(object sender, EventArgs e)
    {
        RangeValidatorHFFrequency.Enabled = ((RadioButton)sender).ID == "RBHF";     //RangeValidatorToFrequencyHF.Enabled = RangeValidatorFromFrequencyHF.Enabled = ((RadioButton)sender).ID == "RBHF";
        RangeValidatorVHFFrequency.Enabled = ((RadioButton)sender).ID == "RBVHF";   //RangeValidatorToFrequencyVHF.Enabled = RangeValidatorFromFrequencyVHF.Enabled = ((RadioButton)sender).ID == "RBVHF";
        RangeValidatorUHFFrequency.Enabled = ((RadioButton)sender).ID == "RBUHF";   //RangeValidatorToFrequencyUHF.Enabled = RangeValidatorFromFrequencyUHF.Enabled = ((RadioButton)sender).ID == "RBUHF";
    }

    protected void Checked_BSS(object sender, EventArgs e)
    {
       Label24.Visible = DListMetodoBSS.Visible = DListMetodoBSS.Enabled = CheckBSS.Checked;
      TableCalidad.Visible = CheckBSS.Checked;
        if (CheckBSS.Checked)
       {
           DListTablasCalidad_SelectedIndexChanged(sender, e);
           TableCalidad.Visible = DListTablasCalidad.SelectedIndex != 0;
           DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible =  true;
       }
       else
       {
           DListTablasCalidad.SelectedIndex = 0;
           DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = TableCalidad.Visible = false;
       }


    }
    // Respuesta a la regeneración de las sectorizaciones
    private void OnCallBackCompleted(IAsyncResult result)
    {
        try
        {
            int retorno = ServicioCD40.EndRegeneraSectorizaciones(result);
            Session.Add("Sectorizando", false);
        }
        catch (System.Web.Services.Protocols.SoapException soapException)
        {
            logDebugView.Error("(RecursosRadio-OnCallBackCompleted): ", soapException);
        }
    }

    protected void DListTablasCalidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        // TableCalidad.Visible = (DListTablasCalidad.SelectedIndex != 0 && DListTablasCalidad.Visible);


        if (DListTablasCalidad.SelectedIndex != 0)
            MostrarTablaCalidad(Convert.ToInt32(DListTablasCalidad.Items[DListTablasCalidad.SelectedIndex].Value));// TablaValores=f(idTablabss) 
        else
            TableCalidad.Visible = false;
    }


    protected void DListMetodoBSS_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  DListTablasCalidad.Visible = LabelIDTablaCalidad.Visible = String.Compare(DListMetodoBSS.Items[DListMetodoBSS.SelectedIndex].Text, METODO_BSS_NUCLEO) == 0;

        DListTablasCalidad_SelectedIndexChanged(null, e);
        
    }
    protected void DListSQ_SelectedIndexChanged(object sender, EventArgs e)
    {

        Label33.Visible = TxtUmbralVAD.Visible = DListSQ.SelectedIndex != 0;    
    }
    protected void TxParamAudio_Changed(object sender, EventArgs e)
    {
        Label10.Visible = TxtGananciaTx.Visible = RBAGCTx.Checked == false;
    }
    protected void RxParamAudio_Changed(object sender, EventArgs e)
    {
        Label3.Visible = TxtGananciaRx.Visible = RBAGCRx.Checked == false;
    }

    protected void ConfiguraOpcionesM_N(object sender, EventArgs e)
    {
        //La potencia no es visible y se configura con valor 2 que corresponde a Normal
        DDLPotencia.SelectedValue = "2";

        if (RBPrincipal.Checked)
        {
            //No se visualiza el offset
            DDLOffsetGeneral.Visible = Label42.Visible = true;

            //No se visualiza la prioridad, la canalización y la Modulación
            TBPriority.Visible = Label34.Visible = true;
            TBPriority.Text = "50";
            DDLCanalizacion.Visible = Label37.Visible = true;
            DDLModulacion.Visible = Label38.Visible = true;
            RangeValidator3.Enabled = true;
        }
        else
        {
            //si es un equipo N: 

            //No se visualiza el offset
            DDLOffsetGeneral.Visible = Label42.Visible = false;
            DDLOffsetGeneral.SelectedValue = "0";

            //No se visualiza la prioridad, la canalización y la Modulación
            TBPriority.Visible = Label34.Visible = false;
            DDLCanalizacion.Visible =  Label37.Visible = false;
            DDLModulacion.Visible =  Label38.Visible = false;
            RangeValidator3.Enabled = false;
        }
    }

    //Devuelve true si ya existe algun recurso en el sistema con el mismo identificador. En caso contrario, false.
    private bool bIdentificadorAsignado(string strIdSistema, string strIdentificador)
    {
        bool bExiste = false;
        int iExiste = -1;

        /// Se comprueba que no existe otro recurso radio o de telefonía con el mismo identificador 
        iExiste = ServicioCD40.CheckIdentificadorAsignado("R", strIdSistema, strIdentificador);

        if (iExiste > 0)
            bExiste = true;
        else if (iExiste < 0)
        {
            System.Text.StringBuilder strMsgError = new System.Text.StringBuilder();
            strMsgError.AppendFormat("(RecursosRadio-bIdentificadorAsignado): el servicio servicioCD40.CheckIdentificadorAsignado('R', '{0}', '{1}') ha devuelto el codigo {2}", strIdSistema, strIdentificador, iExiste);
            logDebugView.Error(strMsgError.ToString());
            strMsgError.Clear();
        }

        return bExiste;
    }

}

