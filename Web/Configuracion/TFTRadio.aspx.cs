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
using System.Collections.Generic;
using log4net;
using log4net.Config;

public partial class TFTRadio :	PageBaseCD40.PageCD40	// System.Web.UI.Page
{
    //private Mensajes.msgBox cMsg;
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
    private static uint NumPaginas;
    private static uint NumPosicionesPag;
    // Número de columnas fijas en la cuadrícula gráfica
    private const uint NUM_COLUMNAS_FIJAS = 5;
    private const uint NUM_FILAS_FIJAS = 4;
    private static uint NumColumnasVisibles = NUM_COLUMNAS_FIJAS;
    private static uint NumFilasVisibles = NUM_FILAS_FIJAS;
    private const uint MAX_FREC_PAG = NUM_COLUMNAS_FIJAS * NUM_FILAS_FIJAS;
    private static uint numPagActual = 0;
	private static bool Modificando = false;
	static bool PermisoSegunPerfil;
    private Ulises5000Configuration.ToolsUlises5000Section UlisesToolsVersion;

    private static ServiciosCD40.Tablas[] datosRadio;

	private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();
	private AsyncCallback CallbackCompletado;

    protected new void Page_Load(object sender, EventArgs e)
    {
		base.Page_Load(sender, e);

		if (Context.Request.IsAuthenticated)
		{
			// retrieve user's identity from httpcontext user 
			FormsIdentity ident = (FormsIdentity)Context.User.Identity;
			string perfil = ident.Ticket.UserData;
			if (perfil == "0")
			{
				Response.Redirect("~/Configuracion/Inicio.aspx?Permiso=NO");
				return;
			}

            PermisoSegunPerfil = LBoxDestinos.Enabled = (perfil == "3" || perfil == "2");
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            Ulises5000Configuration.ToolsUlises5000Section UlisesTools = Ulises5000Configuration.ToolsUlises5000Section.Instance;

            UlisesToolsVersion = UlisesTools;
        }

		if (CallbackCompletado == null)
			CallbackCompletado = new AsyncCallback(OnCallBackCompleted);

        PanelNoPermissions.Visible = false;

        if (!IsPostBack)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            Session["idsistema"] = s.Value;

            if (Session["NombreSector"] != null)
                Label6.Text = GetLocalResourceObject("Label6Resource1.Text").ToString() + ": " +
                    ((string)Session["NombreSector"]);	// "-- Radio del Sector: " + ((string)Session["NombreSector"]) + " --";

            Session["PaginaRad"] = 1;
            numPagActual = 1;
            LabelPag.Text = (string)GetGlobalResourceObject("Espaniol", "Pagina") + " " + numPagActual.ToString();

            if (UlisesToolsVersion.Tools["SupervisionPortadora"] == null)
                CBSupervisionPortadora.Visible = false;

            CargaParametrosPanel();
            CargarPanelDestinos();
            ActualizarPosicionesPanel();

            //MVO-20170710: Se oculta la tabla de recursos GViewEstado y se mantienen los valores por defecto. Todos los recursos marcados            
            GViewEstado.Visible = false;
        }
        else
        {
            //Si se ha recargado la página, las variables datos y la variable de session tienen valor nulo es porque 
            // si ha cambiado la sesión del servidor, bien por conmutación o reinicio
            //por lo que se va a la página de login
            if (datosRadio == null || Session["idsistema"] == null)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);
            }
        }
        //else
        //    if (IsPostBack)
        //    {
        //        if (Request.Form["eliminaelemento"] == "1")   //El usuario elige eliminar el elemento 
        //        {
        //            Request.Form["eliminaelemento"].Replace("1", "0");
        //            EliminarElemento();
        //        }
        //        if (Request.Form["cancelparam"] == "1")   //El usuario elige no guardar los cambios 
        //        {
        //            Request.Form["cancelparam"].Replace("1", "0");
        //            RequiredFieldNucleo.Visible = false;
        //            CancelarCambios();
        //        }
        //        if (Request.Form["aceptparam"] == "1")   //El usuario elige guardar los cambios
        //        {
        //            Request.Form["aceptparam"].Replace("1", "0");
        //            GuardarCambios();
        //        }
        //    }
    }

	private void CargaParametrosPanel()
	{
		try
		{
			ServiciosCD40.ParametrosSector t = new ServiciosCD40.ParametrosSector();
			t.IdSistema = (string)Session["idsistema"];
			t.IdSector = (string)Session["NombreSector"];
			t.IdNucleo = (string)Session["idnucleo"];
			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
			if (d.Length > 0)
			{
				NumPaginas = ((ServiciosCD40.ParametrosSector)d[0]).NumPagFrec;
				NumPosicionesPag = ((ServiciosCD40.ParametrosSector)d[0]).NumFrecPagina;
                if (NumPosicionesPag > 12)
                {
                    NumColumnasVisibles = 5;
                    if (NumPosicionesPag < 16) NumFilasVisibles = 3;
                }
                else if (NumPosicionesPag > 9)
                {
                    NumColumnasVisibles = 4;
                    NumFilasVisibles = 3;
                }
                else if (NumPosicionesPag > 4)
                {
                    NumColumnasVisibles = 3;
                    NumFilasVisibles = 3;
                }
                else
                {
                    NumColumnasVisibles = 2;
                    NumFilasVisibles = 2;
                }
			}
		}
		catch (Exception ex)
		{
			logDebugView.Error("(TFTRadio-CargaParametrosPanel): ", ex);
		}
	}

    private uint CalculatePosHmi(uint buttonIndex)
    {
        uint fila = ((uint)buttonIndex - 1) / NUM_COLUMNAS_FIJAS; //0..NUM_COLUMNAS_FIJAS
        uint columna = ((uint)buttonIndex - 1) % NUM_COLUMNAS_FIJAS; //0..NUM_COLUMNAS_FIJAS

        return fila * NumColumnasVisibles + columna + 1 + ((numPagActual - 1) * NumPosicionesPag);
    }
    private uint CalculatePosButton(uint posHmi)
    {
        //pos HMI 1...NumPosicionesPag*Num pag
        uint fila = ((posHmi - 1) % (uint)NumPosicionesPag) / NumColumnasVisibles;
        uint columna = ((posHmi - 1) % (uint)NumPosicionesPag) % NumColumnasVisibles; //0..NUM_COLUMNAS_FIJAS

        return fila * NUM_COLUMNAS_FIJAS + columna + 1;
    }

    protected void IButPagArriba_Click(object sender, ImageClickEventArgs e)
    {
        if (numPagActual<NumPaginas)
            numPagActual += 1;
        else
            numPagActual = 1;

        Session["PaginaRad"] = numPagActual;
        LabelPag.Text = (string)GetGlobalResourceObject("Espaniol", "Pagina") + " " + numPagActual.ToString();
        LimpiarPanel();
        CargarPanelDestinos();
        MostrarRadio();
    }

    protected void IButPagAbajo_Click(object sender, ImageClickEventArgs e)
    {
        if (numPagActual > 1)
            numPagActual -= 1;
        else
            numPagActual = (uint)NumPaginas;

        Session["PaginaRad"] = numPagActual;
        LabelPag.Text = (string)GetGlobalResourceObject("Espaniol", "Pagina") + " " + numPagActual.ToString();
        LimpiarPanel();
        CargarPanelDestinos();
        MostrarRadio();
    }

    private void CargarPanelDestinos()
    {
        try
        {
			string[] destinos = ServicioCD40.DestinosRadioSinAsignarALaPaginaDelSector((string)Session["idsistema"], (string)Session["NombreSector"], numPagActual);
            LBoxDestinos.Items.Clear();
            if (destinos != null)
                for (int i = 0; i < destinos.Length; i++)
                    LBoxDestinos.Items.Add(destinos[i]);
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTRadio-CargarPanelDestinos): ",ex);
        }
    }

    private void CargaRadio()
    {
        try
        {
			ServiciosCD40.Tablas[] d = ServicioCD40.DestinosRadioAsignadosAlSector((string)Session["idsistema"], (string)Session["NombreSector"]);
            //System.Diagnostics.Debug.Assert(d != null);
            datosRadio = d;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTRadio-CargaRadio): ", ex);
        }
    }

    private void ActualizarPosicionesPanel()
    {
        CargaRadio();
        LimpiarPanel();
        MostrarRadio();
    }

    private void MostrarRadio()
    {
        try
        {
            uint posini = ((numPagActual - 1) * NumPosicionesPag) + 1;
            uint posfin = posini + NumPosicionesPag - 1;

            if (datosRadio != null)
            {
                for (int i = 0; i < datosRadio.Length; i++)
                {
                    uint pos = ((ServiciosCD40.DestinosRadioSector)datosRadio[i]).PosHMI;
                    if ((pos >= posini) && (pos <= posfin))
                    {
                        uint posenpanel = CalculatePosButton(pos);
                        Button ibut = (Button)TEnlacesRadio.FindControl("Button" + posenpanel.ToString());
                        TextBox tbox = (TextBox)TEnlacesRadio.FindControl("TextBox" + posenpanel.ToString());
                        //ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceExternoAs.jpg";
                        ibut.CssClass = "BtnPanelRadioAsignado";
                        //En el tooltip del botón se muestra el identificador del destino y en el texto el literal
                        ibut.ToolTip = ((ServiciosCD40.DestinosRadioSector)datosRadio[i]).IdDestino;
                        tbox.Text = ((ServiciosCD40.DestinosRadioSector)datosRadio[i]).Literal;
                        tbox.ToolTip = ((ServiciosCD40.DestinosRadioSector)datosRadio[i]).IdDestino;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTRadio-MostrarRadio): ", ex);
        }
    }

    private void LimpiarPanel()
    {
        int visibleCount = 0;
        for (int i = 1; i <= NumFilasVisibles * NUM_COLUMNAS_FIJAS; i++)
        {
			TableCell tCell = (TableCell)TEnlacesRadio.FindControl("TableCell" + i.ToString());
			if (tCell != null)
			{
                int fila = (i - 1) / (int)NUM_COLUMNAS_FIJAS; //0..NUM_COLUMNAS_FIJAS
                int columna = (i - 1) % (int)NUM_COLUMNAS_FIJAS; //0..NUM_COLUMNAS_FIJAS
                if ((fila <= NumFilasVisibles) && (columna < NumColumnasVisibles))
                {
                    tCell.Visible = true;
                    if (++visibleCount <= NumPosicionesPag)
                        tCell.Enabled = true;
                    else tCell.Enabled = false;
                }
                else
                    tCell.Visible = false;

				Button ibut = (Button)TEnlacesRadio.FindControl("Button" + i.ToString());
				TextBox tbox = (TextBox)TEnlacesRadio.FindControl("TextBox" + i.ToString());
                //ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceExterno.jpg";
                ibut.CssClass = "BtnPanelRadioLibre";
                tbox.Text = "";
                ibut.ToolTip = String.Empty;
                tbox.ToolTip = String.Empty;
			}
        }
    }

    private void AsignarDestino(string id)
    {
        Button ibut = (Button)TEnlacesRadio.FindControl(id);
        //if (ibut.ImageUrl == "~/Configuracion/Images/BotonEnlaceExternoAs.jpg")
        if (ibut.CssClass != "BtnPanelRadioAsignado")
            DesasignarDestino(id);
	
		uint posicion = CalculatePosHmi(UInt16.Parse(id.Replace("Button", "")));
		ServiciosCD40.DestinosRadioSector elemento = (ServiciosCD40.DestinosRadioSector)Array.Find(datosRadio,
														delegate(object d) { return ((ServiciosCD40.DestinosRadioSector)d).PosHMI == posicion; });

        if (elemento != null)
        {
            TBoxLiteral.Text = elemento.Literal;
            TBoxDestino.Text = elemento.IdDestino;
        }
        else
        {
            TBoxLiteral.Text = (string)ViewState["IdDestino"];
            TBoxDestino.Text = (string)ViewState["IdDestino"];
        }

		
		DListPrioridadSIP.SelectedIndex = 0;
		//TBoxLiteral.Text = id;
		Panel2.Visible = true;
        MostrarElementosPanel(TBoxLiteral.Text);
		CargaRecursos(elemento != null ? elemento.IdDestino : (string)ViewState["IdDestino"]);

        //MVO-20170710: Se oculta la tabla de recursos y se mantienen los valores por defecto. Todos los recursos marcados        
		// Por defecto, todos los recursos quedan seleccionados
        //if (GViewEstado.Visible && GViewEstado.Rows.Count > 0)
        if (GViewEstado.Rows.Count > 0)
        {
            foreach (GridViewRow row in GViewEstado.Rows)
            {
               ((CheckBox)row.Cells[0].FindControl("DListEstados")).Checked = true;
            }
        }
    }

    private void MostrarElementosPanel(string frecuenciaTxt)
    {
        // Buscar si esta frecuencia ya está dada de alta en otra página.
        // Si es así, comparte todos los atributos de la frecuencia que ya está dada de alta.
        foreach (ServiciosCD40.DestinosRadioSector t in datosRadio)
        {
            if (t.IdDestino == frecuenciaTxt)
            {
                DListPrioridadTecla.SelectedValue = ((ServiciosCD40.DestinosRadioSector)t).Prioridad.ToString();
                DListPrioridadSIP.SelectedValue = ((ServiciosCD40.DestinosRadioSector)t).PrioridadSIP.ToString();
                DListModoOpe.SelectedValue = ((ServiciosCD40.DestinosRadioSector)t).ModoOperacion;
                DListCascos.SelectedValue = ((ServiciosCD40.DestinosRadioSector)t).Cascos;
                CBSupervisionPortadora.Checked = ((ServiciosCD40.DestinosRadioSector)t).SupervisionPortadora;
            }
        }
    }

    private void DesasignarDestino(string id)
    {
        try
        {
            Button ibut = (Button)TEnlacesRadio.FindControl(id);
            TextBox tbox = (TextBox)TEnlacesRadio.FindControl("TextBox" + id.Replace("Button", ""));
            if (ibut != null && ibut.CssClass == "BtnPanelRadioAsignado")
			{
                uint posicion = CalculatePosHmi(UInt16.Parse(id.Replace("Button", "")));
				ServiciosCD40.DestinosRadioSector elemento = (ServiciosCD40.DestinosRadioSector)Array.Find(datosRadio,
																delegate(object d) { return ((ServiciosCD40.DestinosRadioSector)d).PosHMI == posicion; });

				if (elemento != null)
				{
					if (ServicioCD40.DeleteSQL(elemento) < 0) 
						logDebugView.Warn("(TFTRadio-DesasignarDestino): No se ha podido desasignar el destino");
					else
					{
						#region Sincronizar CD30
						Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
						KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
						if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
						{
							SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

							switch (sincro.BajaColateralRadio(elemento.IdNucleo, elemento.PosHMI, elemento.IdSector))
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
				}
			}
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTRadio-DesasignarDestino): ", ex);
        }
    }

    private void GuardarNuevaPosicionEnBD(string idDest, string literal, uint prioridad, uint prioSip, string cascos, string modo, bool supervisarPortadora)
    {
        if (Session["idsistema"] != null && Session["NombreSector"] != null && Session["idnucleo"] != null)
        {
            try
            {
                TextBox tbox = (TextBox)TEnlacesRadio.FindControl("TextBox" + ((string)ViewState["IdBoton"]).Replace("Button", ""));
                if (tbox != null)
                {
                    ServiciosCD40.DestinosRadioSector t = new ServiciosCD40.DestinosRadioSector();
                    t.IdSistema = (string)Session["idsistema"];
                    t.IdSector = (string)Session["NombreSector"];
                    t.IdNucleo = (string)Session["idnucleo"];
                    t.IdDestino = idDest;
                    t.PosHMI = CalculatePosHmi(UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", "")));
                    t.TipoDestino = 0;
                    t.Literal = literal;
                    t.Prioridad = prioridad;
                    t.PrioridadSIP = prioSip;
                    t.Cascos = cascos;
                    t.ModoOperacion = modo;
                    t.SupervisionPortadora = supervisarPortadora;
                    if (ServicioCD40.InsertSQL(t) < 0) logDebugView.Warn("(TFTRadio-GuardarNuevaPosicionEnBD): No se ha podido guardar la posicion.");
                    //for (int i = 1; i < 8; i++)
                    //{
                    //    ServiciosCD40.Altavoces alt = new ServiciosCD40.Altavoces();
                    //    alt.IdSistema = t.IdSistema;
                    //    alt.IdSector = t.IdSector;
                    //    alt.IdNucleo = t.IdNucleo;
                    //    alt.IdDestino = t.IdDestino;
                    //    alt.PosHMI = t.PosHMI;
                    //    alt.TipoDestino = t.TipoDestino;
                    //    alt.NumAltavoz = (uint)i;
                    //    alt.Estado = ((DropDownList)Table1.FindControl("DropDownList" + i.ToString())).SelectedValue;
                    //    if (ServicioCD40.InsertSQL(alt) < 0) logDebugView.Warn("(TFTRadio-GuardarNuevaPosicionEnBD): No se ha podido guardar el altavoz de la posicion.");
                    //}

                    #region Sincronizar CD30
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                    KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                    if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                    {
                        // Obtener el emplazamiento del recurso asignado al destino
                        ServiciosCD40.RecursosRadio rRadio = new ServiciosCD40.RecursosRadio();
                        rRadio.IdSistema = (string)Session["idsistema"];
                        rRadio.IdDestino = t.IdDestino;

                        ServiciosCD40.Tablas[] listaRecursos = ServicioCD40.ListSelectSQL(rRadio);
                        if (listaRecursos != null && listaRecursos.Length > 0)
                        {

                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                            switch (sincro.AltaColateralRadio(t.IdNucleo, t.IdSector, t.PosHMI, t.Literal, t.IdDestino, ((ServiciosCD40.RecursosRadio)listaRecursos[0]).IdEmplazamiento))
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
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                logDebugView.Error("(TFTRadio-GuardarNuevaPosicionEnBD): ", ex);
            }
        }
    }
    
    protected void CeldaEnlaceRadio_OnClick(object sender, EventArgs e)
    {
        Button ibut = (Button)TEnlacesRadio.FindControl(((Button)sender).ID);
		TextBox tbox = (TextBox)TEnlacesRadio.FindControl("TextBox" + ibut.ID.Replace("Button", ""));

        //if (ibut.ImageUrl == "~/Configuracion/Images/BotonEnlaceExterno.jpg")
        if (ibut.CssClass == "BtnPanelRadioLibre")
        {
            BtLiberar.Enabled = false;
            BModificar.Enabled = false;

            //El botón Asignar sólo debe estar habilitado si el botón del panel está libre y
            //se ha seleccionado algún destino de la lista
            if (LBoxDestinos.SelectedIndex >= 0)
            {
                BtAsignar.Enabled = PermisoSegunPerfil;
                ViewState["IdDestino"] = LBoxDestinos.SelectedItem.Text;
            }
            else
                BtAsignar.Enabled = false;
		}
		else
		{
			BtLiberar.Enabled = PermisoSegunPerfil;
			BModificar.Enabled = true;
            BtAsignar.Enabled = false;
		}



		//if (BtAsignar.Enabled || BtLiberar.Enabled)
		if (tbox.Text != "" || PermisoSegunPerfil)
		{
            ViewState["IdBoton"] = ((Button)sender).ID;
            TEnlacesRadio.Enabled = false;
            LBoxDestinos.Enabled = false;
            BtLC.Enabled = false;
            BtTelefonia.Enabled = false;
            IButPagAbajo.Enabled = false;
            IButPagArriba.Enabled = false;
            BtSector.Enabled = false;
            Panel1.Visible = true;
        }
        else if (!PermisoSegunPerfil)
            PanelNoPermissions.Visible = true;
    }

    protected void BtTelefonia_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracion/TFTTelefonia.aspx");
    }

    protected void BtLC_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracion/TFTLC.aspx");
    }

    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        EsconderPanelOpciones();
    }

    private void EsconderPanelOpciones()
    {
        TEnlacesRadio.Enabled = true;
        LBoxDestinos.Enabled = PermisoSegunPerfil;
        BtLC.Enabled = true;
        BtTelefonia.Enabled = true;
        IButPagAbajo.Enabled = true;
        IButPagArriba.Enabled = true;
        BtSector.Enabled = true;
        Panel1.Visible = false;
    }

    protected void BtLiberar_Click(object sender, EventArgs e)
    {
        if (Session["idsistema"] != null)
        {
            DesasignarDestino((string)ViewState["IdBoton"]);
            CargarPanelDestinos();
            ActualizarPosicionesPanel();
            EsconderPanelOpciones();
            CargarPanelDestinos();

            try
            {
                // Llamada asíncrona para regenerar todas las sectorizaciones.
                Session.Add("Sectorizando", true);
                ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, true, CallbackCompletado, null);
            }
            catch (Exception ex)
            {
                logDebugView.Error("(TFTRadio-BtLiberar_Click): ", ex);
            }
        }
	}

	protected void BtModificar_Click(object sender, EventArgs e)
	{
		EsconderPanelOpciones();

		Modificando = true;

		string id = (string)ViewState["IdBoton"];
		TextBox tbox = (TextBox)TEnlacesRadio.FindControl("TextBox" + id.Replace("Button", ""));

        uint posicion = CalculatePosHmi(UInt16.Parse(id.Replace("Button", "")));
		ServiciosCD40.DestinosRadioSector elemento = (ServiciosCD40.DestinosRadioSector)Array.Find(datosRadio,
														delegate(object d) { return ((ServiciosCD40.DestinosRadioSector)d).PosHMI == posicion; });
		//ServiciosCD40.DestinosRadioSector elemento = (ServiciosCD40.DestinosRadioSector)Array.Find(datosRadio, 
		//            delegate(object d) { return ((ServiciosCD40.DestinosRadioSector)d).PosHMI == Convert.ToUInt32(id.Replace("Button", "")); });
		if (elemento != null)
			MuestraOpcionesFrecuencia(tbox.Text, elemento.IdDestino);
	}

	private void MuestraOpcionesFrecuencia(string strLiteral, string strDestino)
	{
		TBoxLiteral.Text = strLiteral;
        TBoxDestino.Text = strDestino;

		if (!PermisoSegunPerfil)
		{
			BtAceptar.Enabled = TBoxLiteral.Enabled = DListPrioridadSIP.Enabled = DListPrioridadTecla.Enabled = false;
			DListModoOpe.Enabled = DListCascos.Enabled = GViewEstado.Enabled = false;
		}

		//MuestraDatosAltavoces(strDestino);
		MuestraDatosPosicion(strDestino);
		MuestraDatosEstadoRecursos(strDestino);

		Panel2.Visible = true;
	}

	private void MuestraDatosEstadoRecursos(string strDestino)
	{
		CargaRecursos(strDestino);

		ServiciosCD40.EstadosRecursos eRecurso = new ServiciosCD40.EstadosRecursos();

		eRecurso.IdSistema = (string)Session["idsistema"];
		eRecurso.IdSector = (string)Session["NombreSector"];
		eRecurso.IdNucleo = (string)Session["idnucleo"];
        eRecurso.PosHMI = CalculatePosHmi(UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", "")));

		eRecurso.TipoRecurso = 0;	// Radio
		eRecurso.IdDestino = strDestino;
		eRecurso.TipoDestino = 0;

		ServiciosCD40.Tablas[] listaRecursos = ServicioCD40.ListSelectSQL(eRecurso);
		
        //MVO-20170710: Se oculta la tabla de recursos y se mantienen los valores por defecto. Todos los recursos marcados
        //GViewEstado.Visible = listaRecursos.Length > 0;

        for (int i = 0; i < GViewEstado.Rows.Count && i < GViewEstado.PageSize && i < listaRecursos.Length; i++)
        {
            ((CheckBox)GViewEstado.Rows[i].Cells[0].FindControl("DListEstados")).Checked = ((ServiciosCD40.EstadosRecursos)listaRecursos[i]).Estado == "S";
        }
	}

    //private void MuestraDatosAltavoces(string strDestino)
    //{
    //    ServiciosCD40.Altavoces altavoz = new ServiciosCD40.Altavoces();
    //    altavoz.IdSistema = (string)Session["idsistema"];
    //    altavoz.IdSector = (string)Session["NombreSector"];
    //    altavoz.IdNucleo = (string)Session["idnucleo"];
    //    altavoz.IdDestino = strDestino;
    //    altavoz.PosHMI = UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", "")) +
    //                            (uint)((numPagActual - 1) * NumPosicionesPag);
    //    altavoz.TipoDestino = 0;

    //    ServiciosCD40.Tablas[] listaAltavoces = ServicioCD40.ListSelectSQL(altavoz);
    //    for (int i = 0; i < listaAltavoces.Length; i++)
    //    {
    //        ((DropDownList)Table1.FindControl("DropDownList" + (i+1).ToString())).SelectedValue = ((ServiciosCD40.Altavoces)listaAltavoces[i]).Estado;
    //    }
    //}

	private void MuestraDatosPosicion(string strDestino)
	{
        foreach (ServiciosCD40.DestinosRadioSector t in datosRadio)
        {
            if (t.IdDestino == strDestino)
            {
                if (t.PosHMI == CalculatePosHmi(UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", ""))))
                {
                    DListPrioridadTecla.SelectedValue = ((ServiciosCD40.DestinosRadioSector)t).Prioridad.ToString();
                    DListPrioridadSIP.SelectedValue = ((ServiciosCD40.DestinosRadioSector)t).PrioridadSIP.ToString();
                    DListModoOpe.SelectedValue = ((ServiciosCD40.DestinosRadioSector)t).ModoOperacion;
                    DListCascos.SelectedValue = ((ServiciosCD40.DestinosRadioSector)t).Cascos;
                    CBSupervisionPortadora.Checked = ((ServiciosCD40.DestinosRadioSector)t).SupervisionPortadora;
                }
            }
        }
	}

    protected void BtAsignar_Click(object sender, EventArgs e)
    {
		Modificando = false;
        AsignarDestino((string)ViewState["IdBoton"]);
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        string destinoModificar="";
        if (AtLeastOneSelectedResource())
        {
            //Se comprueba que el Literal tiene el formato correcto y una longitud máxima de 10 caracteres
            if (!System.Text.RegularExpressions.Regex.IsMatch(TBoxLiteral.Text, @"^[\w-._]{1,10}$"))
            {
                string strMsgAux = string.Empty;

                // El identificador no tiene el formato correcto 
                if (GetLocalResourceObject("MsgLiteralFormatoIncorrecto") != null)
                {
                    strMsgAux = GetLocalResourceObject("MsgLiteralFormatoIncorrecto").ToString();
                }
                else
                    strMsgAux = "El literal del destino radio no tiene un formato correcto";

                cMsg.confirm(strMsgAux, "aceptparam");
            }
            else
            {
                if (!Modificando)
                {
                    GuardarNuevaPosicionEnBD((string)ViewState["IdDestino"], TBoxLiteral.Text,
                            UInt16.Parse(DListPrioridadTecla.SelectedValue), UInt16.Parse(DListPrioridadSIP.SelectedValue),
                            DListCascos.SelectedValue, DListModoOpe.SelectedValue, CBSupervisionPortadora.Checked);
                    Panel2.Visible = false;
                    EsconderPanelOpciones();
                    CargarPanelDestinos();
                    ActualizarPosicionesPanel();
                    CargarPanelDestinos();
                    // Guardar el estado de los recursos propios del destino
                    GuardaEstado();

                    ActualizaParametrosFrecuencia((string)ViewState["IdDestino"]);

                    //VMG 13/03/2019
                    if (RBLSpreadLiteral.SelectedIndex == 0)//Enviar a todos los sectores con este destino
                        DistribuyeAlias2Sectores((string)Session["idsistema"], (string)ViewState["IdDestino"],
                            (string)Session["idnucleo"], TBoxLiteral.Text);
                }
                else
                {
                    string id = (string)ViewState["IdBoton"];
                    uint posicion = CalculatePosHmi(UInt16.Parse(id.Replace("Button", "")));
                    ServiciosCD40.DestinosRadioSector elemento = (ServiciosCD40.DestinosRadioSector)Array.Find(datosRadio,
                                                                    delegate(object d) { return ((ServiciosCD40.DestinosRadioSector)d).PosHMI == posicion; });

                    //			TextBox tbox = (TextBox)TEnlacesRadio.FindControl("TextBox" + id.Replace("Button", ""));


                    if (elemento != null)
                    {
                        destinoModificar = elemento.IdDestino;
                        ActualizaDatosPosicion(elemento.IdDestino);
                        //ActualizaDatosAltavoces(elemento.IdDestino);
                        ActualizaDatosEstadoRecursos(elemento.IdDestino);
                        ActualizarPosicionesPanel();
                        ActualizaParametrosFrecuencia(elemento.IdDestino);
                    }
                    Panel2.Visible = false;
                    //VMG 13/03/2019
                    if (RBLSpreadLiteral.SelectedIndex == 0)//Enviar a todos los sectores con este destino
                        DistribuyeAlias2Sectores((string)Session["idsistema"], destinoModificar,
                            (string)Session["idnucleo"], TBoxLiteral.Text);
                }
                
                try
                {
                    // Llamada asíncrona para regenerar todas las sectorizaciones.
                    Session.Add("Sectorizando", true);
                    ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, true, CallbackCompletado, null);
                }
                catch (Exception ex)
                {
                    logDebugView.Error("(TFTRadio-BtAceptar_Click): ", ex);
                }
            }
        }
        else
            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "SeleccionarAlgunRecurso"));
	}
    /// VMG 13/03/2019
    /// <summary>
    /// Distribuye el alias a todos los sectores que tengan asignado dicho destino
    /// </summary>
    /// <param name="sistema"></param>
    /// <param name="destino"></param>
    /// <param name="nucleo"></param>
    /// <param name="alias"></param>
    private void DistribuyeAlias2Sectores(string sistema, string destino, string nucleo, string alias)
    {
        ServiciosCD40.DestinosRadioSector drs = new ServiciosCD40.DestinosRadioSector();
        drs.IdSistema = sistema;
        drs.IdDestino = destino;
        drs.IdNucleo = nucleo;
        drs.Literal = alias;
        drs.PosHMI = 0;//Mandamos cero para no entrar en otro update
        //Se comprueba en el update que el sector se mande null

        ServicioCD40.UpdateSQL(drs);
    }

    private bool AtLeastOneSelectedResource()
    {
        bool someOne = false;
        foreach (GridViewRow fila in GViewEstado.Rows)
        {
            someOne |= ((CheckBox)fila.Cells[0].FindControl("DListEstados")).Checked;
        }

        return someOne;
    }

    private void ActualizaParametrosFrecuencia(string idDestino)
    {
        ServiciosCD40.DestinosRadioSector t = new ServiciosCD40.DestinosRadioSector();
        t.IdSistema = (string)Session["idsistema"];
        t.IdSector = (string)Session["NombreSector"];
        t.IdNucleo = (string)Session["idnucleo"];
        t.IdDestino = idDestino;
        t.PosHMI = 0;   // Quiero que actualice el estado de los parámetros para todas las posiciones de esta frecuencia
        t.TipoDestino = 0;
        t.Literal = TBoxLiteral.Text;
        t.Cascos = DListCascos.SelectedValue;
        t.ModoOperacion = DListModoOpe.SelectedValue;
        t.Prioridad = Convert.ToUInt32(DListPrioridadTecla.SelectedValue);
        t.PrioridadSIP = Convert.ToUInt32(DListPrioridadSIP.SelectedValue);
        t.SupervisionPortadora = CBSupervisionPortadora.Checked;

        ServicioCD40.UpdateSQL(t);
    }

	private void ActualizaDatosEstadoRecursos(string id)
	{
		ServiciosCD40.EstadosRecursos eRecurso = new ServiciosCD40.EstadosRecursos();
        ServiciosCD40.EstadosRecursos oneRecurso = new ServiciosCD40.EstadosRecursos();

		oneRecurso.IdSistema = eRecurso.IdSistema = (string)Session["idsistema"];
        eRecurso.IdSector = (string)Session["NombreSector"];
		eRecurso.IdNucleo = (string)Session["idnucleo"];
        eRecurso.PosHMI = CalculatePosHmi(UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", "")));

        oneRecurso.TipoRecurso = eRecurso.TipoRecurso = 0;	// Radio
        oneRecurso.IdDestino = eRecurso.IdDestino = id;
		oneRecurso.TipoDestino = eRecurso.TipoDestino = 0;

		foreach (GridViewRow fila in GViewEstado.Rows)
		{
            oneRecurso.IdRecurso = eRecurso.IdRecurso = fila.Cells[2].Text;
			oneRecurso.Estado = eRecurso.Estado = ((CheckBox)fila.Cells[0].FindControl("DListEstados")).Checked ? "S" : "A";

			ServicioCD40.UpdateSQL(eRecurso);

            // Hacer que todos los paneles con este destino configurado tengan el mismo estado del recurso asignado "S" o "A"
            oneRecurso.IdSector = oneRecurso.IdNucleo = "";
            oneRecurso.PosHMI = 0;
            ServicioCD40.UpdateSQL(oneRecurso);
        }
	}

    //private void ActualizaDatosAltavoces(string id)
    //{
    //    ServiciosCD40.Altavoces altavoz = new ServiciosCD40.Altavoces();
    //    altavoz.IdSistema = (string)Session["idsistema"];
    //    altavoz.IdSector = (string)Session["NombreSector"];
    //    altavoz.IdNucleo = (string)Session["idnucleo"];
    //    altavoz.IdDestino = id;
    //    altavoz.PosHMI = UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", "")) +
    //                            (uint)((numPagActual - 1) * NumPosicionesPag);
    //    altavoz.TipoDestino = 0;

    //    for (int i = 0; i < 7; i++)
    //    {
    //        altavoz.NumAltavoz = (uint)(i + 1);
    //        altavoz.Estado = ((DropDownList)Table1.FindControl("DropDownList" + (i + 1).ToString())).SelectedValue;
    //        ServicioCD40.UpdateSQL(altavoz);
    //    }
    //}

	private void ActualizaDatosPosicion(string id)
	{
		ServiciosCD40.DestinosRadioSector t = new ServiciosCD40.DestinosRadioSector();
		t.IdSistema = (string)Session["idsistema"];
		t.IdSector = (string)Session["NombreSector"];
		t.IdNucleo = (string)Session["idnucleo"];
		t.IdDestino = id;
        t.PosHMI = CalculatePosHmi(UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", "")));
		t.TipoDestino = 0;
		t.Literal = TBoxLiteral.Text;
		t.Cascos = DListCascos.SelectedValue;
		t.ModoOperacion = DListModoOpe.SelectedValue;
		t.Prioridad = Convert.ToUInt32(DListPrioridadTecla.SelectedValue);
		t.PrioridadSIP = Convert.ToUInt32(DListPrioridadSIP.SelectedValue);
        t.SupervisionPortadora = CBSupervisionPortadora.Checked;

		ServicioCD40.UpdateSQL(t);

		#region Sincronizar CD30
		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
		if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
		{
			// Obtener el emplazamiento del recurso asignado al destino
			ServiciosCD40.RecursosRadio rRadio = new ServiciosCD40.RecursosRadio();
			rRadio.IdSistema = (string)Session["idsistema"];
			rRadio.IdDestino = t.IdDestino;

			ServiciosCD40.Tablas[] listaRecursos = ServicioCD40.ListSelectSQL(rRadio);
			if (listaRecursos != null && listaRecursos.Length > 0)
			{

				SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                sincro.ModificaColateralRadio(t.IdNucleo, t.IdSector, t.PosHMI, t.Literal, ((ServiciosCD40.RecursosRadio)listaRecursos[0]).IdEmplazamiento);
				
			}
		}
		#endregion
	}

	private void GuardaEstado()
	{
		ServiciosCD40.EstadosRecursos eRecurso = new ServiciosCD40.EstadosRecursos();
        ServiciosCD40.EstadosRecursos oneRecurso = new ServiciosCD40.EstadosRecursos();

		oneRecurso.IdSistema = eRecurso.IdSistema = (string)Session["idsistema"];
		eRecurso.IdSector = (string)Session["NombreSector"];
		eRecurso.IdNucleo = (string)Session["idnucleo"];
        eRecurso.PosHMI = CalculatePosHmi(UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", "")));
		
		oneRecurso.TipoRecurso = eRecurso.TipoRecurso = 0;	// Radio
		oneRecurso.IdDestino = eRecurso.IdDestino = (string)ViewState["IdDestino"];
		oneRecurso.TipoDestino = eRecurso.TipoDestino = 0;
		
		foreach (GridViewRow fila in GViewEstado.Rows)
		{
			oneRecurso.IdRecurso = eRecurso.IdRecurso = fila.Cells[2].Text;
			oneRecurso.Estado = eRecurso.Estado = ((CheckBox)fila.Cells[0].FindControl("DListEstados")).Checked ? "S" : "A";

			ServicioCD40.InsertSQL(eRecurso);

            // Hacer que todos los paneles con este destino configurado tengan el mismo estado del recurso asignado "S" o "A"
            oneRecurso.IdSector = oneRecurso.IdNucleo = "";
            oneRecurso.PosHMI = 0;
            ServicioCD40.UpdateSQL(oneRecurso);
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
			logDebugView.Error("(TFTRadio-OnCallBackCompleted): ", soapException);
		}
	}

	private void CargaRecursos(string idDestino)
	{
		ServiciosCD40.RecursosRadio rRadio = new ServiciosCD40.RecursosRadio();
		rRadio.IdSistema = (string)Session["idsistema"];
		rRadio.IdDestino = idDestino;

		ServiciosCD40.Tablas[] listaRecursos = ServicioCD40.ListSelectSQL(rRadio);
		if (listaRecursos != null && listaRecursos.Length > 0)
		{
			GViewEstado.DataSource = listaRecursos;
			GViewEstado.DataBind();
		}
	}

	protected void BCancelar_Click(object sender, EventArgs e)
	{
		Panel2.Visible = false;
	}

	protected void OnGVEstado_Changing(object sender, GridViewPageEventArgs e)
	{
		GViewEstado.PageIndex = e.NewPageIndex;
		GViewEstado.DataBind();
	}
    protected void DListEstados_CheckedChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < GViewEstado.Rows.Count; i++)
        {
            ((CheckBox)GViewEstado.Rows[i].Cells[0].FindControl("DListEstados")).Checked = false;
        }
        ((CheckBox)sender).Checked = true;
    }

    protected void BtSector_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracion/Sector.aspx");
    }
}
