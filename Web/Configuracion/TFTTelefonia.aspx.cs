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

public partial class TFTTelefonia :	PageBaseCD40.PageCD40	// System.Web.UI.Page
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

    //private const uint MAX_POSICIONES_PAGINA = 20;

    private static uint NumPaginas;
    private static uint NumPosicionesPag;
    // Número de columnas fijas en la cuadrícula gráfica
    private const uint NUM_COLUMNAS_FIJAS = 5;
    private static uint NumColumnasVisibles = NUM_COLUMNAS_FIJAS;
    private static uint NUM_BUTTONS = 25;
    private static uint numPagActual = 0;
	private static bool Modificando = false;

    private static ServiciosCD40.Tablas[] datosInternos;
    private static ServiciosCD40.Tablas[] datosExternos;
    private static uint[] prefijosPosiciones;
	static bool PermisoSegunPerfil;

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
		}

		if (CallbackCompletado == null)
		    CallbackCompletado = new AsyncCallback(OnCallBackCompleted);

        PanelNoPermissions.Visible = false;

        if (!IsPostBack)
        {
            CargaParametrosPanel();

            prefijosPosiciones = new uint[(NumPaginas * NumPosicionesPag) + 1];	// Las posiciones empiezan en 1. Se indexa por el indice del HMI
            if (Session["NombreSector"] != null)
                Label6.Text = GetLocalResourceObject("Label6Resource1.Text").ToString() + ": " +
                    ((string)Session["NombreSector"]);	// "-- Telefonía del Sector: " + ((string)Session["NombreSector"]) + " --";

            Session["PaginaTF"] = 1;
            numPagActual = 1;
            LabelPag.Text = (string)GetGlobalResourceObject("Espaniol", "Pagina") + " " + numPagActual.ToString();

            CargarPanelDestinos();
            ActualizarPosicionesPanel();
        }
        else
        {
            //Si se ha recargado la página, las variables datos y la variable de session tienen valor nulo es porque 
            // si ha cambiado la sesión del servidor, bien por conmutación o reinicio
            //por lo que se va a la página de login
            if (datosInternos == null || datosExternos == null || Session["idsistema"] == null)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);
            }
        }
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
				NumPaginas = ((ServiciosCD40.ParametrosSector)d[0]).NumPagDestinosInt;
				NumPosicionesPag = ((ServiciosCD40.ParametrosSector)d[0]).NumDestinosInternosPag;
                if (NumPosicionesPag > 15) NumColumnasVisibles = 5;
                else if (NumPosicionesPag > 8) NumColumnasVisibles = 4;
                else if (NumPosicionesPag > 3) NumColumnasVisibles = 3;
                else NumColumnasVisibles = 2;
			}
		}
		catch (Exception ex)
		{
			logDebugView.Error("(TFTRadio-CargaParametrosPanel): ", ex);
		}
	}


    protected void IButPagArriba_Click(object sender, ImageClickEventArgs e)
    {
        if (numPagActual<NumPaginas)
        {
            numPagActual += 1;
        }
        else
        {
            numPagActual = 1;
        }
        Session["PaginaTF"] = numPagActual;
        LabelPag.Text = (string)GetGlobalResourceObject("Espaniol", "Pagina") + " " + numPagActual.ToString();
        LimpiarPanel();
        MostrarInternos();
        MostrarExternos();
    }

    protected void IButPagAbajo_Click(object sender, ImageClickEventArgs e)
    {
        if (numPagActual > 1)
        {
            numPagActual -= 1;
        }
        else
        {
			numPagActual = NumPaginas;
        }
        Session["PaginaTF"] = numPagActual;
        LabelPag.Text = (string)GetGlobalResourceObject("Espaniol", "Pagina") + " " + numPagActual.ToString();
        LimpiarPanel();
        MostrarInternos();
        MostrarExternos();
    }

    private void CargarPanelDestinos()
    {
        try
        {
            ServiciosCD40.Tablas[] destinos = ServicioCD40.DestinosTelefoniaSinAsignarAlSector((string)Session["idsistema"], (string)Session["NombreSector"], (string)Session["idnucleo"]);
            LBoxDestinos.Items.Clear();
            for (int i = 0; i < destinos.Length; i++)
            {
                LBoxDestinos.Items.Add(new ListItem(((ServiciosCD40.DestinosTelefonia)destinos[i]).IdDestino,
                                                  ((ServiciosCD40.DestinosTelefonia)destinos[i]).IdDestino + ((ServiciosCD40.DestinosTelefonia)destinos[i]).IdPrefijo.ToString()));
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTTelefonia-CargarPanelDestinos): ", ex);
        }
    }

    private void CargaDestinosInternos()
    {
        try
        {
			ServiciosCD40.Tablas[] d = ServicioCD40.DestinosTelefoniaAsignadosAlSector((string)Session["idsistema"], (string)Session["NombreSector"], true, true);
            datosInternos = d;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTTelefonia-CargaDestinosInternos): ", ex);
        }
    }

    private void CargaDestinosExternos()
    {
        try
        {
			ServiciosCD40.Tablas[] d = ServicioCD40.DestinosTelefoniaAsignadosAlSector((string)Session["idsistema"], (string)Session["NombreSector"], true, false);
            datosExternos = d;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTTelefonia-CargaDestinosExternos): ", ex);
        }
    }

    private void ActualizarPosicionesPanel()
    {
        CargaDestinosInternos();
        CargaDestinosExternos();
        LimpiarPanel();
        MostrarInternos();
        MostrarExternos();
    }

    private void MostrarInternos()
    {
		uint posini = ((numPagActual - 1) * NumPosicionesPag) + 1;
		uint posfin = posini + NumPosicionesPag - 1;

        if (datosInternos != null)
        {
            for (int i = 0; i < datosInternos.Length; i++)
            {
                uint posHmi = ((ServiciosCD40.DestinosInternosSector)datosInternos[i]).PosHMI;
                if ((posHmi >= posini) && (posHmi <= posfin))
                {
                    uint posenpanel = CalculatePosButton(posHmi);
                    Button ibut = (Button)TEnlacesInternos.FindControl("Button" + posenpanel.ToString());
                    // TextBox tbox = (TextBox)TEnlacesInternos.FindControl("TextBox" + posenpanel.ToString());
                    ibut.CssClass = "BtnPanelTfAsignado";
                    //ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceInternoAs.jpg";
                    ibut.Text = ((ServiciosCD40.DestinosInternosSector)datosInternos[i]).Literal;
                    prefijosPosiciones[posHmi] = ((ServiciosCD40.DestinosInternosSector)datosInternos[i]).IdPrefijo;
                }
            }
        }
    }

    private void MostrarExternos()
    {
		uint posini = ((numPagActual - 1) * NumPosicionesPag) + 1;
		uint posfin = posini + NumPosicionesPag - 1;
        if (datosExternos != null)
        {
            for (int i = 0; i < datosExternos.Length; i++)
            {
                uint posHmi = ((ServiciosCD40.DestinosExternosSector)datosExternos[i]).PosHMI;

                if ((posHmi >= posini) && (posHmi <= posfin))
                {
                    uint posenpanel = CalculatePosButton(posHmi);
                    Button ibut = (Button)TEnlacesInternos.FindControl("Button" + posenpanel.ToString());
                    // TextBox tbox = (TextBox)TEnlacesInternos.FindControl("TextBox" + posenpanel.ToString());
                    ibut.CssClass = "BtnPanelTfAsignado"; 
                    //ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceInternoAs.jpg";
                    ibut.Text = ((ServiciosCD40.DestinosExternosSector)datosExternos[i]).Literal;
                    prefijosPosiciones[posHmi] = ((ServiciosCD40.DestinosExternosSector)datosExternos[i]).IdPrefijo;
                }
            }
        }
    }
    private uint CalculatePosHmi(uint buttonIndex)
    {
        uint fila = ((uint)buttonIndex - 1) / NUM_COLUMNAS_FIJAS; //0..NUM_COLUMNAS_FIJAS
        uint columna = ((uint)buttonIndex - 1) % NUM_COLUMNAS_FIJAS; //0..NUM_COLUMNAS_FIJAS

        return fila * NumColumnasVisibles + columna + 1 + ((numPagActual-1) * NumPosicionesPag);
    }
    private uint CalculatePosButton(uint posHmi)
    {
        //pos HMI 1...NumPosicionesPag*Num pag
        uint fila = ((posHmi-1) % (uint)NumPosicionesPag) / NumColumnasVisibles; 
        uint columna = ((posHmi-1) % (uint)NumPosicionesPag) % NumColumnasVisibles; //0..NUM_COLUMNAS_FIJAS

        return fila * NUM_COLUMNAS_FIJAS + columna + 1;
    }
    private void LimpiarPanel()
    {
        uint numFilas = (NumPosicionesPag / NumColumnasVisibles) +1;
        int visibleCount = 0;
        for (int i = 1; i < numFilas * NUM_COLUMNAS_FIJAS; i++)
        {
            TableCell tCell = (TableCell)TEnlacesInternos.FindControl("TableCell" + i.ToString());
            if (tCell != null)
            {
                int fila = (i - 1) / (int)NUM_COLUMNAS_FIJAS; //0..NUM_COLUMNAS_FIJAS
                int columna = (i - 1) % (int)NUM_COLUMNAS_FIJAS; //0..NUM_COLUMNAS_FIJAS
                if ((fila <= numFilas) && (columna < NumColumnasVisibles))
                {
                    tCell.Visible = true;
                    if (++visibleCount <= NumPosicionesPag)
                        tCell.Enabled = true;
                    else tCell.Enabled = false;
                }
                else
                    tCell.Visible = false;
                Button ibut = (Button)TEnlacesInternos.FindControl("Button" + i.ToString());
                if (ibut != null)
                {
                    ibut.CssClass = "BtnPanelRadioLibre";
                    ibut.Text = "";
                }
            }
        }
    }

    private void AsignarDestino(string id)
    {
        Button ibut = (Button)TEnlacesInternos.FindControl(id);
        if (ibut != null && ibut.CssClass == "BtnPanelTfAsignado")
        //if (ibut != null && ibut.ImageUrl == "~/Configuracion/Images/BotonEnlaceInternoAs.jpg")
            DesasignarDestino(id);
        TBoxLiteral.Text = (string)ViewState["IdDestino"];
		DListPrioridadSIP.SelectedIndex = 0;
        Panel2.Visible = true;        
    }

    private void DesasignarDestino(string id)
    {
        try
        {
            Button ibut = (Button)TEnlacesInternos.FindControl(id);
            // TextBox tbox = (TextBox)TEnlacesInternos.FindControl("TextBox" + id.Replace("Button", ""));
            // if (ibut != null && ibut.ImageUrl == "~/Configuracion/Images/BotonEnlaceInternoAs.jpg")
            if (ibut != null && ibut.CssClass == "BtnPanelTfAsignado")
            {
                UInt16 buttonIndex = UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", ""));
                uint recalcPosHMI = CalculatePosHmi(buttonIndex);
                uint prefijo = prefijosPosiciones[recalcPosHMI];
                if (prefijo == 2)
                {
                    for (int i = 0; i < datosInternos.Length; i++)
                    {
                        if ((((ServiciosCD40.DestinosInternosSector)datosInternos[i]).Literal == ibut.Text) &&
                            ((ServiciosCD40.DestinosInternosSector)datosInternos[i]).PosHMI == recalcPosHMI)
                        {
						    if (ServicioCD40.DeleteSQL(datosInternos[i]) < 0) logDebugView.Warn("(TFTTelefonia-DesasignarDestino): Fallo en el delete enlace interno");
						    else
						    {
							    ServiciosCD40.DestinosInternosSector s = new ServiciosCD40.DestinosInternosSector();
							    s = (ServiciosCD40.DestinosInternosSector)datosInternos[i];
							    ServicioCD40.EliminaColateralEnUsuarioReciproco(ref s);

							    #region Sincronizar CD30
							    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
							    KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
							    if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
							    {
								    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

								    switch (sincro.BajaColateralTelefonia(((ServiciosCD40.DestinosInternosSector)datosInternos[i]).IdNucleo,
																    ((ServiciosCD40.DestinosInternosSector)datosInternos[i]).IdSector,
																    ((ServiciosCD40.DestinosInternosSector)datosInternos[i]).PosHMI))
								    {
									    case 127:
										    sincro.BajaColateralTelefonia(s.IdNucleo, s.IdSector, s.PosHMI);
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
                            break;
                        }
                    }
                }
                else
                    if (prefijo > 2)
                    {
                        for (int i = 0; i < datosExternos.Length; i++)
                        {
                            if ((((ServiciosCD40.DestinosExternosSector)datosExternos[i]).Literal == ibut.Text) &&
                                ((ServiciosCD40.DestinosExternosSector)datosExternos[i]).PosHMI == recalcPosHMI)
                            {
							    if (ServicioCD40.DeleteSQL(datosExternos[i]) < 0) logDebugView.Warn("(TFTTelefonia-DesasignarDestino): Fallo en el delete enlace externo");
							    else
							    {
								    #region Sincronizar CD30
								    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
								    KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
								    if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
								    {
									    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

									    switch (sincro.BajaColateralTelefonia(((ServiciosCD40.DestinosExternosSector)datosExternos[i]).IdNucleo,
																	    ((ServiciosCD40.DestinosExternosSector)datosExternos[i]).IdSector,
																	    ((ServiciosCD40.DestinosExternosSector)datosExternos[i]).PosHMI))
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
							    break;
                            }
                        }
                    }
			}
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTTelefonia-DesasignarDestino): ", ex);
        }
    }

    private void GuardarNuevaPosicionEnBD(string idDest, string literal, uint idPref, uint prioridad, uint prioSip)
    {
        if (Session["idsistema"] != null && Session["NombreSector"] != null && Session["idnucleo"] != null)
        {
            try
            {
                UInt16 buttonIndex = UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", ""));
                uint posHmi = CalculatePosHmi(buttonIndex);
                // TextBox tbox = (TextBox)TEnlacesInternos.FindControl("TextBox" + ((string)ViewState["IdBoton"]).Replace("Button", ""));
                if (idPref > 2)//telefonia externa
                {
                    ServiciosCD40.DestinosExternosSector t = new ServiciosCD40.DestinosExternosSector();
                    t.IdSistema = (string)Session["idsistema"];
                    t.IdSector = (string)Session["NombreSector"];
                    t.OrigenR2 = (string)Session["NombreSector"];
                    t.IdNucleo = (string)Session["idnucleo"];
                    t.IdDestino = idDest;
                    t.PosHMI = posHmi;
                    t.IdPrefijo = idPref;
                    t.TipoDestino = 1;
                    t.Literal = literal;
                    t.Prioridad = prioridad;
                    t.PrioridadSIP = prioSip;
                    t.TipoAcceso = "DA";

                    if (!Modificando && ServicioCD40.InsertSQL(t) < 0) logDebugView.Warn("(TFTTelefonia-GuardarNuevaPosicionEnBD): fallo en insert de telefonia externa");
                    else if (!Modificando)
                    {
                        #region Sincronizar CD30
                        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                        {
                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                            switch (sincro.AltaColateralTelefonia(t.IdNucleo, t.IdSector, t.PosHMI, t.Literal, t.IdDestino, t.IdPrefijo, t.OrigenR2, t.Prioridad))
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
                    if (Modificando && ServicioCD40.UpdateSQL(t) < 0) logDebugView.Warn("(TFTTelefonia-GuardarNuevaPosicionEnBD): fallo en update de telefonia externa");
                    else if (Modificando)
                    {
                        #region Sincronizar CD30
                        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                        {
                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                            switch (sincro.ModificaColateralTelefonia(t.IdNucleo, t.IdSector, t.PosHMI, t.Literal, t.Prioridad))
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
                else
                    if (idPref == 2)//telefonia interna
                    {
                        ServiciosCD40.DestinosInternosSector t = new ServiciosCD40.DestinosInternosSector();
                        t.IdSistema = (string)Session["idsistema"];
                        t.IdSector = (string)Session["NombreSector"];
                        t.OrigenR2 = (string)Session["NombreSector"];
                        t.IdNucleo = (string)Session["idnucleo"];
                        t.IdDestino = idDest;
                        t.PosHMI = posHmi;
                        t.IdPrefijo = idPref;
                        t.TipoDestino = 2;
                        t.Literal = literal;
                        t.Prioridad = prioridad;
                        t.PrioridadSIP = prioSip;
                        t.TipoAcceso = "DA";
                        if (!Modificando && ServicioCD40.InsertSQL(t) < 0) logDebugView.Warn("(TFTTelefonia-GuardarNuevaPosicionEnBD): fallo en insert de telefonia interna");
                        else if (!Modificando)
                        {
                            #region Sincronizar CD30
                            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                            KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                            if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                            {
                                SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                                switch (sincro.AltaColateralTelefonia(t.IdNucleo, t.IdSector, t.PosHMI, t.Literal, t.IdDestino, 1, t.OrigenR2, t.Prioridad))
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
                        if (Modificando && ServicioCD40.UpdateSQL(t) < 0) logDebugView.Warn("(TFTTelefonia-GuardarNuevaPosicionEnBD): fallo en update de telefonia interna");
                        else if (Modificando)
                        {
                            #region Sincronizar CD30
                            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                            KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                            if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                            {
                                SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                                switch (sincro.ModificaColateralTelefonia(t.IdNucleo, t.IdSector, t.PosHMI, t.Literal, t.Prioridad))
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

                        if (!Modificando)
                        {
                            // Dar de baja el colatarel en el usuario recíproco
                            if (ServicioCD40.InsertaColateralEnUsuarioReciproco(ref t, (int)NumPosicionesPag))
                            {
                                #region Sincronizar CD30
                                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                                {
                                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                                    switch (sincro.AltaColateralTelefonia(t.IdNucleo, t.IdSector, t.PosHMI, t.Literal, t.IdDestino, 1, t.OrigenR2, t.Prioridad))
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
                logDebugView.Error("(TFTTelefonia-GuardarNuevaPosicionEnBD): ", ex);
            }
        }
    }

    protected void CeldaEnlaceTelefonia_OnClick(object sender, EventArgs e)
    {
        Button ibut = (Button)TEnlacesInternos.FindControl(((Button)sender).ID);

        if (ibut != null && ibut.CssClass == "BtnPanelRadioLibre")
		{
			BtLiberar.Enabled = false;
			BModificar.Enabled = false;

            //El botón Asignar sólo debe estar habilitado si el botón seleccionado del panel está libre y
            //se ha seleccionado algún destino de la lista
            if (LBoxDestinos.SelectedIndex >= 0)
            {
                BtAsignar.Enabled = PermisoSegunPerfil;
                ViewState["IdDestino"] = LBoxDestinos.SelectedItem.Text;
                ViewState["IdPrefijo"] = LBoxDestinos.SelectedValue.Replace(LBoxDestinos.SelectedItem.Text, "");
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

        if (ibut.Text != "" || PermisoSegunPerfil)
        {
            ViewState["IdBoton"] = ((Button)sender).ID;
            TEnlacesInternos.Enabled = false;
            LBoxDestinos.Enabled = false;
            BtLC.Enabled = false;
            BtRadio.Enabled = false;
            IButPagAbajo.Enabled = false;
            IButPagArriba.Enabled = false;
            BtSector.Enabled = false;

            Panel1.Visible = true;
        }
        else if (!PermisoSegunPerfil)
            PanelNoPermissions.Visible = true;
    }

    protected void BtRadio_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracion/TFTRadio.aspx");
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
        TEnlacesInternos.Enabled = true;
        LBoxDestinos.Enabled = PermisoSegunPerfil;
        BtLC.Enabled = true;
        BtSector.Enabled = true;
        BtRadio.Enabled = true;
        IButPagAbajo.Enabled = true;
        IButPagArriba.Enabled = true;
        Panel1.Visible = false;
    }

    protected void BtLiberar_Click(object sender, EventArgs e)
    {
        DesasignarDestino((string)ViewState["IdBoton"]);
        CargarPanelDestinos();
        ActualizarPosicionesPanel();
        EsconderPanelOpciones();

		try
		{
			// Llamada asíncrona para regenerar todas las sectorizaciones.
			Session.Add("Sectorizando", true);
			ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, true/*Sólo telefonía*/, CallbackCompletado, null);
		}
		catch (Exception ex)
		{
			logDebugView.Error("(TFTTelefonia-BtLiberar_Click): ", ex);
		}
	}

    protected void BtAsignar_Click(object sender, EventArgs e)
    {
		Modificando=false;
        AsignarDestino((string)ViewState["IdBoton"]);
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
		GuardarNuevaPosicionEnBD((string)ViewState["IdDestino"], TBoxLiteral.Text, UInt16.Parse((string)ViewState["IdPrefijo"]),
                UInt16.Parse(DListPrioridadTecla.SelectedValue), UInt16.Parse(DListPrioridadSIP.SelectedValue));
		
		Panel2.Visible = false;
        EsconderPanelOpciones();
        CargarPanelDestinos();
        ActualizarPosicionesPanel();

		Modificando = false;

		try
		{
			// Llamada asíncrona para regenerar todas las sectorizaciones.
			Session.Add("Sectorizando", true);
			ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, true/*Sólo telefonía*/, CallbackCompletado, null);
		}
		catch (Exception ex)
		{
			logDebugView.Error("(TFTTelefonia-BtAceptar_Click): ", ex);
		}
	}

	protected void BtModificar_Click(object sender, EventArgs e)
	{
		//EsconderPanelOpciones();

		Modificando = true;

		//string id = (string)ViewState["IdBoton"];
		//Button ibut = (Button)TEnlacesInternos.FindControl(id);
		//TextBox tbox = (TextBox)TEnlacesInternos.FindControl("TextBox" + id.Replace("Button", ""));

		MuestraParametrosPosicion();

		Panel2.Visible = true;
	}

	private void MuestraParametrosPosicion()
	{
        UInt16 buttonIndex = UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", ""));
        uint posicionPulsada = CalculatePosHmi(buttonIndex);

		if (posicionPulsada <= 0)
			return;

		if (!PermisoSegunPerfil)
		{
			BtAceptar.Enabled = TBoxLiteral.Enabled = DListPrioridadSIP.Enabled = DListPrioridadTecla.Enabled = false;
		}

		if (prefijosPosiciones[posicionPulsada] > 2)//telefonia externa
		{
			ServiciosCD40.DestinosExternosSector t = new ServiciosCD40.DestinosExternosSector();
			t.IdSistema = (string)Session["idsistema"];
			t.IdNucleo = (string)Session["idnucleo"];
			t.IdSector = (string)Session["NombreSector"];
			t.IdPrefijo = prefijosPosiciones[posicionPulsada];
            t.PosHMI = CalculatePosHmi(buttonIndex);
            t.TipoAcceso = "DA";

			try
			{
				DataSet ds = ServicioCD40.DataSetSelectSQL(t);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					DataRow dExterno = ds.Tables[0].Rows[0];
					TBoxLiteral.Text = (string)dExterno["Literal"];
					//DListPrioridadSIP.SelectedIndex = (int)((uint)dExterno["PrioridadSIP"] - 1);
					DListPrioridadSIP.SelectedValue = Convert.ToString(((uint)dExterno["PrioridadSIP"]));
					DListPrioridadTecla.SelectedIndex = (int)((uint)dExterno["Prioridad"] - 1);

					ViewState["IdDestino"] = (string)dExterno["IdDestino"];
					ViewState["IdPrefijo"] = prefijosPosiciones[posicionPulsada].ToString();
				}
			}
			catch (Exception ex)
			{
				logDebugView.Error("(TFTTelefonia-MuestraParametrosPosicion): ", ex);
			}
		}
		else if (prefijosPosiciones[posicionPulsada] == 2)//telefonia interna
		{
			ServiciosCD40.DestinosInternosSector t = new ServiciosCD40.DestinosInternosSector();

			t.IdSistema = (string)Session["idsistema"];
			t.IdNucleo = (string)Session["idnucleo"];
			t.IdSector = (string)Session["NombreSector"];
			t.IdPrefijo = 2;
            t.PosHMI = CalculatePosHmi(buttonIndex);
            t.TipoAcceso = "DA";

			try
			{
				DataSet ds = ServicioCD40.DataSetSelectSQL(t);
				if (ds.Tables[0].Rows.Count > 0)
				{
					DataRow dInterno = ds.Tables[0].Rows[0];
					TBoxLiteral.Text = (string)dInterno["Literal"];
					//DListPrioridadSIP.SelectedIndex = (int)((uint)dInterno["PrioridadSIP"] - 1);
					DListPrioridadSIP.SelectedValue = Convert.ToString(((uint)dInterno["PrioridadSIP"]));
					DListPrioridadTecla.SelectedIndex = (int)((uint)dInterno["Prioridad"] - 1);

					ViewState["IdDestino"] = (string)dInterno["IdDestino"];
					ViewState["IdPrefijo"] = prefijosPosiciones[posicionPulsada].ToString();
				}
			}
			catch (Exception ex)
			{
				logDebugView.Error("(TFTTelefonia-MuestraParametrosPosicion): ", ex);
			}
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
			logDebugView.Error("(TFTTelefonia-OnCallBackCompleted): ", soapException);
		}
	}

	protected void BCancelar_Click(object sender, EventArgs e)
	{
		Panel2.Visible = false;
	}

    protected void BtSector_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracion/Sector.aspx");
    }
}
