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

public partial class TFTLC : PageBaseCD40.PageCD40	// System.Web.UI.Page
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

    //  private const uint MAX_POSICIONES_PAGINA = 18;

    private static ServiciosCD40.Tablas[] datosLCint;
    private static ServiciosCD40.Tablas[] datosLCext;
    private static uint[] prefijosPosiciones;
	private static bool Modificando = false;
    private static int NumPosicionesPag = 0;
    static bool PermisoSegunPerfil;
    static int PrioridadTecla;

    private const int PREFIJO_DESTINO_LCI = 0;
    private const int PREFIJO_DESTINO_LCE =1;
    private const int PREFIJO_DESTINO_ATS = 2;
    private const int PREFIJO_DESTINO_ATS_BIS = 3;

    private static bool bEsDestinoATS_IP = false;


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

		//	ServicioCD40.NoTransaction();

        PanelNoPermissions.Visible = false;

        if (!IsPostBack)
        {
            CargaParametrosPanel();

            prefijosPosiciones = new uint[NumPosicionesPag + 1];	// Las posiciones empiezan en 1
            if (Session["NombreSector"] != null)
                Label6.Text = GetLocalResourceObject("Label6Resource1.Text").ToString() + ": " +
                    ((string)Session["NombreSector"]); //"-- Líneas Calientes del Sector: " + ((string)Session["NombreSector"]) + " --";

            CargarPanelDestinos();
            ActualizarPosicionesPanel();
        }
        else
        {
            //Si se ha recargado la página, las variables datos y la variable de session tienen valor nulo es porque 
            // si ha cambiado la sesión del servidor, bien por conmutación o reinicio
            //por lo que se va a la página de login
            if (datosLCint == null || datosLCext == null || Session["idsistema"] == null)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CargaParametrosPanel()
    {
        try
        {
            if (Session["NombreSector"] != null)
            {
                ServiciosCD40.ParametrosSector t = new ServiciosCD40.ParametrosSector();
                t.IdSistema = (string)Session["idsistema"];
                t.IdSector = (string)Session["NombreSector"];
                t.IdNucleo = (string)Session["idnucleo"];
                ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
                if (null != d && d.Length > 0)
                {
                    NumPosicionesPag = (int)((ServiciosCD40.ParametrosSector)d[0]).NumEnlacesAI;
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTLC-CargaParametrosPanel): ", ex);
        }
    }

    private void CargarPanelDestinos()
    {
        try
        {
            ServiciosCD40.Tablas[] destinos = ServicioCD40.DestinosLineaCalienteSinAsignarAlSector((string)Session["idsistema"], (string)Session["NombreSector"], (string)Session["idnucleo"]);
            LBoxDestinos.Items.Clear();

            if (null != destinos)
            {
                for (int i = 0; i < destinos.Length; i++)
                {
                    LBoxDestinos.Items.Add(new ListItem(((ServiciosCD40.DestinosTelefonia)destinos[i]).IdDestino,
                                                        ((ServiciosCD40.DestinosTelefonia)destinos[i]).IdDestino + ((ServiciosCD40.DestinosTelefonia)destinos[i]).IdPrefijo.ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTLC-CargarPanelDestinos): ", ex);
        }
    }

    private void ActualizarPosicionesPanel()
    {
        CargaLC();
        MostrarLC();
    }

    private void CargaLC()
    {
        try
        {
            //El panel de línea caliente puede tener configurados DESTINOS ATS y líneas internas (prefijo=0) que se almacenan en la tabla destinosexternosSector

            ServiciosCD40.Tablas[] d = ServicioCD40.DestinosTelefoniaAsignadosAlSector((string)Session["idsistema"], (string)Session["NombreSector"], false, true);
            datosLCint = d;

            ServiciosCD40.Tablas[] d1 = ServicioCD40.DestinosTelefoniaAsignadosAlSector((string)Session["idsistema"], (string)Session["NombreSector"], false, false);
            datosLCext = d1;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTLC-CargaLC): ", ex);
        }
    }

    private void MostrarLC()
    {
        try
        {
            LimpiarPanel();
            if (datosLCint != null)
            {
                for (int i = 0; i < datosLCint.Length; i++)
                {
                    uint pos = ((ServiciosCD40.DestinosInternosSector)datosLCint[i]).PosHMI;

                    TableCell tCell = (TableCell)TEnlacesLC.FindControl("TableCell" + pos.ToString());
                    if (tCell != null)
                    {
                        tCell.Visible = i <= NumPosicionesPag;

                        Button ibut = (Button)TEnlacesLC.FindControl("Button" + pos.ToString());
                        ibut.CssClass = "BtnPanelTfAsignado";
                        ibut.Text = ((ServiciosCD40.DestinosInternosSector)datosLCint[i]).Literal;
                        prefijosPosiciones[pos] = ((ServiciosCD40.DestinosInternosSector)datosLCint[i]).IdPrefijo;
                    }
                }
            }
            if (datosLCext != null)
            {
                string strEtiLCEN = string.Empty;

                if (GetLocalResourceObject("LbDestinoLCENResource1.Text") != null)
                {
                    strEtiLCEN = GetLocalResourceObject("LbDestinoLCENResource1.Text").ToString();
                }
                for (int i = 0; i < datosLCext.Length; i++)
                {
                    uint pos = ((ServiciosCD40.DestinosExternosSector)datosLCext[i]).PosHMI;

                    TableCell tCell = (TableCell)TEnlacesLC.FindControl("TableCell" + pos.ToString());
                    if (tCell != null)
                    {
                        tCell.Visible = i <= NumPosicionesPag;

                        Button ibut = (Button)TEnlacesLC.FindControl("Button" + pos.ToString());
                        ibut.CssClass = "BtnPanelTfAsignado";
                        ibut.Text = ((ServiciosCD40.DestinosExternosSector)datosLCext[i]).Literal;
                        prefijosPosiciones[pos] = ((ServiciosCD40.DestinosExternosSector)datosLCext[i]).IdPrefijo;

                        if (prefijosPosiciones[pos] == PREFIJO_DESTINO_ATS || prefijosPosiciones[pos] == PREFIJO_DESTINO_ATS_BIS)
                        {
                            if (!string.IsNullOrEmpty(((ServiciosCD40.DestinosExternosSector)datosLCext[i]).IdDestinoLCEN))
                            {
                                ibut.ToolTip = strEtiLCEN + " " + ((ServiciosCD40.DestinosExternosSector)datosLCext[i]).IdDestinoLCEN;
                            }
                            else
                                ibut.ToolTip = string.Empty;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTLC-MostrarLC): ", ex);
        }
    }

    private void LimpiarPanel()
    {
        TEnlacesLC.Height = NumPosicionesPag > 9 ? 80 : 40;

        TableRow tRow = (TableRow)TEnlacesLC.FindControl("TableRow1");
        tRow.Visible = tRow != null && NumPosicionesPag > 9;

        for (int i = 1; i <= NumPosicionesPag; i++)
        {
            TableCell tCell = (TableCell)TEnlacesLC.FindControl("TableCell" + i.ToString());
            if (tCell != null)
            {
                tCell.Visible = i <= NumPosicionesPag;

                Button ibut = (Button)TEnlacesLC.FindControl("Button" + i.ToString());
                ibut.CssClass = "BtnPanelRadioLibre";
                ibut.Text = "";
            }
        }
    }

    private void AsignarDestino(string id)
    {
        Button ibut = (Button)TEnlacesLC.FindControl(id);
        if (ibut.CssClass == "BtnPanelTfAsignado")
            DesasignarDestino(id);
        TBoxLiteral.Text = (string)ViewState["IdDestino"];
		DListPrioridadSIP.SelectedIndex = 0;

        uint prefijo = UInt16.Parse((string)ViewState["IdPrefijo"]);

        if (prefijo == PREFIJO_DESTINO_ATS || prefijo == PREFIJO_DESTINO_ATS_BIS)
        { 
           //Si es un destino ATS, se puede asignar un DESTINO LCEN al destino ATS
           DListDestinosLCEN.Visible = true;
           LbDestinoLCEN.Visible = true;

           bEsDestinoATS_IP = ServicioCD40.DestinoATS_ConCentralIP((string)Session["idsistema"], (string)ViewState["IdDestino"]);
           
           CargaDestinosLCEN_Libres();
        }
        else
        {
           if ( DListDestinosLCEN.Items.Count>0)
             DListDestinosLCEN.Items.Clear();

           DListDestinosLCEN.Visible = false;
           LbDestinoLCEN.Visible = false;
        }

        Panel2.Visible = true;
    }

    private void DesasignarDestino(string id)
    {
        try
        {
            Button ibut = (Button)TEnlacesLC.FindControl(id);
            if (ibut != null && ibut.CssClass == "BtnPanelTfAsignado")
            {
                uint prefijo = prefijosPosiciones[Int16.Parse(id.Replace("Button", ""))];

                if (prefijo == PREFIJO_DESTINO_LCI && datosLCint != null)	// LCI
                {
                    for (int i = 0; i < datosLCint.Length; i++)
                    {
                        if ((((ServiciosCD40.DestinosInternosSector)datosLCint[i]).Literal == ibut.Text) &&
                            ((ServiciosCD40.DestinosInternosSector)datosLCint[i]).PosHMI == Int16.Parse(id.Replace("Button", "")))
                        {
							ServicioCD40.DeleteSQL(datosLCint[i]);

							ServiciosCD40.DestinosInternosSector s = new ServiciosCD40.DestinosInternosSector();
							s = (ServiciosCD40.DestinosInternosSector)datosLCint[i];

							ServicioCD40.EliminaColateralEnUsuarioReciproco(ref s);
							
							#region Sincronizar CD30
							Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
							KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
							if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
							{
								SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

								switch (sincro.BajaColateralTelefonia(((ServiciosCD40.DestinosInternosSector)datosLCint[i]).IdNucleo,
																((ServiciosCD40.DestinosInternosSector)datosLCint[i]).IdSector,
																((ServiciosCD40.DestinosInternosSector)datosLCint[i]).PosHMI + 56))
								{
									case 127:
										sincro.BajaColateralTelefonia(s.IdNucleo, s.IdSector, s.PosHMI + 56);
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
							break;
                        }
                    }
                }
                else
                    if ((prefijo == PREFIJO_DESTINO_LCE || prefijo == PREFIJO_DESTINO_ATS || prefijo == PREFIJO_DESTINO_ATS_BIS) && datosLCext != null)	// LCE=1 o ATS=3
                    {
                        for (int i = 0; i < datosLCext.Length; i++)
                        {
                            if ((((ServiciosCD40.DestinosExternosSector)datosLCext[i]).Literal == ibut.Text) &&
                                ((ServiciosCD40.DestinosExternosSector)datosLCext[i]).PosHMI == Int16.Parse(id.Replace("Button", "")))
                            {
								ServicioCD40.DeleteSQL(datosLCext[i]);
								#region Sincronizar CD30
								Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
								KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
								if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
								{
									SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

									switch (sincro.BajaColateralTelefonia(((ServiciosCD40.DestinosExternosSector)datosLCext[i]).IdNucleo,
																	((ServiciosCD40.DestinosExternosSector)datosLCext[i]).IdSector,
																	((ServiciosCD40.DestinosExternosSector)datosLCext[i]).PosHMI + 56))
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
								break;
                            }
                        }
                    }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTLC-DesasignarDestino): ", ex);
        }
    }

    private void GuardarNuevaPosicionEnBD(string idDest, string literal, uint idPref, uint prioridad, uint prioSip, string idDestinoLCEN)
    {
        if (Session["idsistema"] != null && Session["NombreSector"] != null && Session["idnucleo"] != null)
        {
            try
            {
                if (idPref == PREFIJO_DESTINO_LCE || idPref == PREFIJO_DESTINO_ATS || idPref == PREFIJO_DESTINO_ATS_BIS) //telefonia externa LC (prefijo=1) o Destino ATS(prefijo=3) 
                {
                    ServiciosCD40.DestinosExternosSector t = new ServiciosCD40.DestinosExternosSector();
                    t.IdSistema = (string)Session["idsistema"];
                    t.IdSector = (string)Session["NombreSector"];
                    t.OrigenR2 = (string)Session["NombreSector"];
                    t.IdNucleo = (string)Session["idnucleo"];
                    t.IdDestino = idDest;
                    t.PosHMI = UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", ""));
                    t.IdPrefijo = idPref;
                    t.TipoDestino = 1;
                    t.Literal = literal;
                    t.Prioridad = prioridad;
                    t.PrioridadSIP = prioSip;
                    t.TipoAcceso = "IA";

                    if (!string.IsNullOrEmpty(idDestinoLCEN))
                    {
                        //Se ha asignado un recurso LCEN al Destino ATS
                        t.IdPrefijoDestinoLCEN = PREFIJO_DESTINO_LCE;
                        t.IdDestinoLCEN = idDestinoLCEN;
                    }
                    else
                    {
                        t.IdPrefijoDestinoLCEN = null;
                        t.IdDestinoLCEN = null;
                    }

                    if (!Modificando && ServicioCD40.InsertSQL(t) < 0) logDebugView.Warn("(TFTLC-GuardarNuevaPosicionEnBD): No se ha guardado la posicion de telefonia externa");
                    else if (!Modificando)
                    {
                        #region Sincronizar CD30
                        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                        {
                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                            switch (sincro.AltaColateralTelefonia(t.IdNucleo, t.IdSector, t.PosHMI + 56, t.Literal, t.IdDestino, 2, t.OrigenR2, t.Prioridad))
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

                            switch (sincro.ModificaColateralTelefonia(t.IdNucleo, t.IdSector, t.PosHMI + 56, t.Literal, t.Prioridad))
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
                    if (idPref == PREFIJO_DESTINO_LCI)//telefonia interna
                    {
                        ServiciosCD40.DestinosInternosSector t = new ServiciosCD40.DestinosInternosSector();
                        t.IdSistema = (string)Session["idsistema"];
                        t.IdSector = (string)Session["NombreSector"];
                        t.OrigenR2 = (string)Session["NombreSector"];
                        t.IdNucleo = (string)Session["idnucleo"];
                        t.IdDestino = idDest;
                        t.PosHMI = UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", ""));
                        t.IdPrefijo = idPref;
                        t.TipoDestino = 2;
                        t.Literal = literal;
                        t.Prioridad = prioridad;
                        t.PrioridadSIP = prioSip;
                        t.TipoAcceso = "IA";


                        if (!Modificando)
                        {
                            ServiciosCD40.DestinosInternosSector d = t;
                            if (ServicioCD40.InsertaColateralEnUsuarioReciproco(ref d, (int)NumPosicionesPag))
                            {
                                if (ServicioCD40.InsertSQL(t) < 0)
                                {
                                    ServicioCD40.EliminaColateralEnUsuarioReciproco(ref d);
                                    logDebugView.Warn("(TFTLC-GuardarNuevaPosicionEnBD): No se ha guardado la posicion de telefonia interna");

                                    return;
                                }

                                #region Sincronizar CD30
                                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                                {
                                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                                    sincro.AltaColateralTelefonia(d.IdNucleo, d.IdSector, d.PosHMI + 56, d.Literal, d.IdDestino, 7, d.OrigenR2, d.Prioridad);
                                }
                                #endregion

                                #region Sincronizar CD30
                                //Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                                //KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                                {
                                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                                    switch (sincro.AltaColateralTelefonia(t.IdNucleo, t.IdSector, t.PosHMI + 56, t.Literal, t.IdDestino, 7, t.OrigenR2, t.Prioridad))
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
                            else
                            {
                                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "PanelLcCompleto"));
                            }

                            return;
                        }
                        else
                        {
                            ServiciosCD40.DestinosInternosSector d = t;
                            if (ServicioCD40.UpdateSQL(t) < 0)
                                logDebugView.Warn("(TFTTelefonia-GuardarNuevaPosicionEnBD): fallo en update de telefonia externa");
                            else
                            {
                                if (ServicioCD40.EliminaColateralEnUsuarioReciproco(ref d))
                                {
                                    d = t;
                                    if (ServicioCD40.InsertaColateralEnUsuarioReciproco(ref d, (int)NumPosicionesPag))
                                    {
                                        #region Sincronizar CD30
                                        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                                        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                                        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                                        {
                                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                                            sincro.AltaColateralTelefonia(d.IdNucleo, d.IdSector, d.PosHMI + 56, d.Literal, d.IdDestino, 7, d.OrigenR2, d.Prioridad);
                                        }
                                        #endregion

                                        #region Sincronizar CD30
                                        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                                        {
                                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                                            switch (sincro.ModificaColateralTelefonia(t.IdNucleo, t.IdSector, t.PosHMI + 56, t.Literal, t.Prioridad))
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
                    }
            }
            catch (Exception ex)
            {
                logDebugView.Error("(TFTLC-GuardarNuevaPosicionEnBD): ", ex);
            }
        }
    }

    protected void CeldaEnlaceLineaCaliente_OnClick(object sender, EventArgs e)
    {
        Button ibut = (Button)TEnlacesLC.FindControl(((Button)sender).ID);
		
		if (ibut.CssClass == "BtnPanelRadioLibre")
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


//        if (BtAsignar.Enabled || BtLiberar.Enabled)
        if (ibut.Text != "" || PermisoSegunPerfil)
        {
            ViewState["IdBoton"] = ((Button)sender).ID;
            TEnlacesLC.Enabled = false;
            LBoxDestinos.Enabled = false;
            BtTelefonia.Enabled = false;
            BtRadio.Enabled = false;
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

    protected void BtTelefonia_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracion/TFTTelefonia.aspx");
    }

    protected void BtSector_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracion/Sector.aspx");
    }

    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        EsconderPanelOpciones();
    }

    private void EsconderPanelOpciones()
    {
        TEnlacesLC.Enabled = true;
        LBoxDestinos.Enabled = PermisoSegunPerfil;
        BtTelefonia.Enabled = true;
        BtRadio.Enabled = true;
        Panel1.Visible = false;
        BtSector.Enabled = true;
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
			ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, true, CallbackCompletado, null);
		}
		catch (Exception ex)
		{
			logDebugView.Error("(TFTLC-BtLiberar_Click): ", ex);
		}
	}

    protected void BtAsignar_Click(object sender, EventArgs e)
    {
		Modificando = false;
		AsignarDestino((string)ViewState["IdBoton"]);
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        string strDestinoLCEN = string.Empty;
        uint iPrefijoDest = 0;
        string strIdDestino = string.Empty;

        iPrefijoDest=UInt16.Parse((string)ViewState["IdPrefijo"]);
        strIdDestino = (string)ViewState["IdDestino"];

        //Si destino es ATS y no es IP, es obligatorio seleccionar un destino LCEN

        if ((iPrefijoDest == PREFIJO_DESTINO_ATS||iPrefijoDest == PREFIJO_DESTINO_ATS_BIS) && !bEsDestinoATS_IP && DListDestinosLCEN.SelectedIndex <= 0)
        {
            string strMsg = string.Empty;

            if (null != GetLocalResourceObject("DestinoATS_NoIP_DestinoLCEN_Requerido"))
            {
                strMsg = string.Format(GetLocalResourceObject("DestinoATS_NoIP_DestinoLCEN_Requerido").ToString(), strIdDestino);
            }
            else
            {
                strMsg = string.Format("Debe asignar un destino LCEN al destino ATS no IP {0}.", strIdDestino);
            }
            cMsg.alert(strMsg);
            return;

        }

        if ((!Modificando && UInt16.Parse(DListPrioridadTecla.SelectedValue) == 1) ||
            (Modificando && UInt16.Parse(DListPrioridadTecla.SelectedValue) == 1 && PrioridadTecla != 1))
        {
            if (ServicioCD40.DemasiadasTeclasConPrioridadUno((string)Session["idsistema"], (string)Session["idnucleo"], PREFIJO_DESTINO_LCI == iPrefijoDest))
            {
                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod125"));
                return;
            }
        }

        //El campo strDestinoLCEN sólo se almacena si el destino es ATS
        if ((iPrefijoDest==PREFIJO_DESTINO_ATS || iPrefijoDest==PREFIJO_DESTINO_ATS_BIS) && DListDestinosLCEN.SelectedIndex > 0)
        {
            strDestinoLCEN = DListDestinosLCEN.SelectedValue;
        }

        GuardarNuevaPosicionEnBD((string)ViewState["IdDestino"], TBoxLiteral.Text, iPrefijoDest,
                UInt16.Parse(DListPrioridadTecla.SelectedValue), UInt16.Parse(DListPrioridadSIP.SelectedValue), strDestinoLCEN);
        Panel2.Visible = false;
        EsconderPanelOpciones();
        CargarPanelDestinos();
        ActualizarPosicionesPanel();

		Modificando = false;

		try
		{
			// Llamada asíncrona para regenerar todas las sectorizaciones.
			Session.Add("Sectorizando", true);
			ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, true, CallbackCompletado, null);
		}
		catch (Exception ex)
		{
			logDebugView.Error("(TFTLC-BtAceptar_Click): ", ex);
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
		uint posicionPulsada = UInt16.Parse(((string)ViewState["IdBoton"]).Replace("Button", ""));

		if (!PermisoSegunPerfil)
		{
			BtAceptar.Enabled = TBoxLiteral.Enabled = DListPrioridadSIP.Enabled = DListPrioridadTecla.Enabled = false;
		}

		if (posicionPulsada <= 0)
			return;

        if (prefijosPosiciones[posicionPulsada] == PREFIJO_DESTINO_LCE || prefijosPosiciones[posicionPulsada] == PREFIJO_DESTINO_ATS ||
            prefijosPosiciones[posicionPulsada] == PREFIJO_DESTINO_ATS_BIS)	//línea caliente externa=1 ó ATS=3
		{
			ServiciosCD40.DestinosExternosSector t = new ServiciosCD40.DestinosExternosSector();
			t.IdSistema = (string)Session["idsistema"];
			t.IdNucleo = (string)Session["idnucleo"];
			t.IdSector = (string)Session["NombreSector"];
            t.IdPrefijo = prefijosPosiciones[posicionPulsada];
			t.PosHMI = posicionPulsada;
            t.TipoAcceso="IA";

			try
			{
				DataSet ds = ServicioCD40.DataSetSelectSQL(t);
				if (ds!=null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0)
				{
					DataRow dExterno = ds.Tables[0].Rows[0];
					TBoxLiteral.Text = (string)dExterno["Literal"];
					// DListPrioridadSIP.SelectedIndex = (int)((uint)dExterno["PrioridadSIP"] - 1);
					DListPrioridadSIP.SelectedValue = Convert.ToString(((uint)dExterno["PrioridadSIP"]));
					DListPrioridadTecla.SelectedIndex = (int)((uint)dExterno["Prioridad"] - 1);
                    PrioridadTecla = (int)((uint)dExterno["Prioridad"]);

					ViewState["IdDestino"] = (string)dExterno["IdDestino"];
					ViewState["IdPrefijo"] = prefijosPosiciones[posicionPulsada].ToString();

                    if (prefijosPosiciones[posicionPulsada] == PREFIJO_DESTINO_ATS || prefijosPosiciones[posicionPulsada] == PREFIJO_DESTINO_ATS_BIS)
                    {
                        //Si es un destino ATS
                        DListDestinosLCEN.Visible = true;
                        LbDestinoLCEN.Visible = true;
                        //Se carga la lista de Destinos LCEN libres en el sistema
                        CargaDestinosLCEN_Libres();

                        //Obtenemos si es un Destino ATS con un numero de abonado dentro de un encaminamieno IP
                        bEsDestinoATS_IP = ServicioCD40.DestinoATS_ConCentralIP((string)Session["idsistema"], (string)ViewState["IdDestino"]);

                        if (null != dExterno["IdDestinoLCEN"])
                        {
                            string strDestinoLCEN = dExterno["IdDestinoLCEN"].ToString();

                            if (!string.IsNullOrEmpty(strDestinoLCEN))
                            {
                                if (DListDestinosLCEN.Items.FindByText(strDestinoLCEN) != null)
                                {
                                    DListDestinosLCEN.Items.FindByText(strDestinoLCEN).Selected = true;
                                }
                                else
                                {
                                    //Se añade el destino que tiene actualmente asignado, porque no estará en la lista
                                    DListDestinosLCEN.Items.Add(strDestinoLCEN);
                                    DListDestinosLCEN.Items.FindByText(strDestinoLCEN).Selected = true;
                                }
                            }
                        }
                        else
                        {
                            //Se selecciona el primer elemento que corresponde a "Ningún Destino Seleccionado"
                            DListDestinosLCEN.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        //Si es no es un destino ATS
                        if (DListDestinosLCEN.Items.Count > 0)
                            DListDestinosLCEN.Items.Clear();

                        DListDestinosLCEN.Visible = false;
                        LbDestinoLCEN.Visible = false;
                    }
				}
			}
			catch (Exception ex)
			{
				logDebugView.Error("(TFTTelefonia-MuestraParametrosPosicion): ", ex);
			}
		}
		else				//línea caliente interna
		{
            if (DListDestinosLCEN.Items.Count > 0)
                DListDestinosLCEN.Items.Clear();

            DListDestinosLCEN.Visible = false;
            LbDestinoLCEN.Visible = false;

			ServiciosCD40.DestinosInternosSector t = new ServiciosCD40.DestinosInternosSector();

			t.IdSistema = (string)Session["idsistema"];
			t.IdNucleo = (string)Session["idnucleo"];
			t.IdSector = (string)Session["NombreSector"];
            t.IdPrefijo = PREFIJO_DESTINO_LCI;
			t.PosHMI = posicionPulsada;

			try
			{
				DataSet ds = ServicioCD40.DataSetSelectSQL(t);
				if (ds!=null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0)
				{
					DataRow dInterno = ds.Tables[0].Rows[0];
					TBoxLiteral.Text = (string)dInterno["Literal"];
					//DListPrioridadSIP.SelectedIndex = (int)((uint)dInterno["PrioridadSIP"] - 1);
					DListPrioridadSIP.SelectedValue = Convert.ToString(((uint)dInterno["PrioridadSIP"]));
					DListPrioridadTecla.SelectedIndex = (int)((uint)dInterno["Prioridad"] - 1);
                    PrioridadTecla = (int)((uint)dInterno["Prioridad"]);

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
			logDebugView.Error("(TFTLC-OnCallBackCompleted): ", soapException);
		}
	}

	protected void BCancelar_Click(object sender, EventArgs e)
	{
		Panel2.Visible = false;
	}

    private void CargaDestinosLCEN_Libres()
    {
        //Se obtienen la lista de destinos LCEN que no están asignados en el sistema
        string strFirstItemRecorder = string.Empty;

        if (null != GetLocalResourceObject("DListDestinosLCENResNinguno"))
        {
            strFirstItemRecorder = GetLocalResourceObject("DListDestinosLCENResNinguno").ToString();
        }
        else
            strFirstItemRecorder = "< Ninguno >";

        try
        {
            DListDestinosLCEN.Items.Clear();

            DListDestinosLCEN.Items.Add(strFirstItemRecorder);

            ServiciosCD40.Tablas[] listaDestLC = ServicioCD40.DestinosLineaCalienteSinAsignar((string)Session["idsistema"]);

            if (null != listaDestLC && listaDestLC.Length > 0)
            {
                for (int i = 0; i < listaDestLC.Length; i++)
                {
                    DListDestinosLCEN.Items.Add(((ServiciosCD40.DestinosTelefonia)listaDestLC[i]).IdDestino);
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTLC-CargaDestinosLCEN_Libres): Error al cargar la lista de destinos LCEN Sin asignar en el sistema ", ex);
        }
    }
}
