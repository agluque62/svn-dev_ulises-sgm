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

public partial class DestinosTelefonia : PageBaseCD40.PageCD40	// System.Web.UI.Page
{
    //System.Data.DataSet listaDestino;
    static bool Modificando;
    private static ILog _logDebugView;
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
	static bool PermisoSegunPerfil;
    /// <summary>
    /// 
    /// </summary>
	static ServiciosCD40.ServiciosCD40 ServiceServiciosCD40 = new ServiciosCD40.ServiciosCD40();
    /// <summary>
    /// 
    /// </summary>
	private AsyncCallback CallbackCompletado;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
		}

		if (CallbackCompletado == null)
			CallbackCompletado = new AsyncCallback(OnCallBackCompleted);

		if (!IsPostBack)
        {
            //logDebugView.Debug("Entrando en DestinosTelefonia....");

            IndexListBox1 = -1;

            BAnadir_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");
            
            MuestraDatos();
			CargaDDL();
			BNuevo.Visible = PermisoSegunPerfil;
			CargarInforme();
		}
        else
        {
            if (Request.Form["SoloEliminaDeTFT"] == "1")
            {
                Request.Form["SoloEliminaDeTFT"].Replace("1", "0");

                EliminaDestinoDeTFT();
                EliminarElemento(true);
            }
        }

        //else
        //    {
        //        if (Request.Form["eliminaelemento"] == "1")//El usuario elige eliminar el elemento 
        //        {
        //            Request.Form["eliminaelemento"].Replace("1", "0");

        //            EliminarElemento();
        //        }
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
            //}
    }
    /// <summary>
    /// 
    /// </summary>
	protected void CargarInforme()
	{
		LBImprimir.Attributes.Remove("onclick");
		string comando = "AbreVentana('../Informes/Report.aspx?Report=DestinosTf.rpt');return false;";
		LBImprimir.Attributes.Add("onclick", comando);
	}

    /// <summary>
    /// 
    /// </summary>
    private void MuestraDatos()
    {
        try
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            Session["idsistema"] = s.Value;

			//listaDestino = ServiceServiciosCD40.DestinosDeTelefoniaEnElSistema(s.Value);
			LBDestinos.DataSource = ServiceServiciosCD40.DestinosDeTelefoniaEnElSistema(s.Value).Tables[0];
			LBDestinos.DataTextField = "IdDestino";
			LBDestinos.DataBind();

			//foreach (DataRow d in ServiceServiciosCD40.DestinosDeTelefoniaEnElSistema(s.Value).Tables[0].Rows)
			//{
			//    LBDestinos.Items.Add(d["IdDestino"].ToString());
			//}

			if (LBDestinos.Items.Count > 0)
			{
				ActualizaWebPadre(true);

                if (LBDestinos.Items.FindByText(NewItem) != null)
                {
                    LBDestinos.Items.FindByText(NewItem).Selected = true;
                    IndexListBox1 = LBDestinos.SelectedIndex;
                    NewItem = string.Empty;
                }
                else
                    LBDestinos.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < LBDestinos.Items.Count ? IndexListBox1 : 0;

				BModificar.Visible = BEliminar.Visible = PermisoSegunPerfil;

				MuestraElemento();
				GeneraXmlParaInforme();
			}
			else
			{
				BModificar.Visible = BEliminar.Visible = false;
			}
        }
        catch (Exception e)
        {
            logDebugView.Error("(DestinosTelefonia-MuestraDatos):", e);
        }
    }
    /// <summary>
    /// 
    /// </summary>
	private void GeneraXmlParaInforme()
	{
		//ServiciosCD40.DestinosTelefonia t = new ServiciosCD40.DestinosTelefonia();
		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
		//t.IdSistema = s.Value;

		ServiceServiciosCD40.DestinosDeTelefoniaEnElSistema(s.Value).WriteXml(Server.MapPath("~/Informes/DestinosTf.xml"));
		// ServiceServiciosCD40.RangosConIdRed(s.Value, null).WriteXml(Server.MapPath("~/Informes/Rangos.xml"));

		// ServiciosCD40.Rutas r = new ServiciosCD40.Rutas();
		// r.IdSistema = s.Value;
		// ServiceServiciosCD40.DataSetSelectSQL(r).WriteXml(Server.MapPath("~/Informes/Rutas.xml"));
		// ServiciosCD40.TroncalesRuta tr = new ServiciosCD40.TroncalesRuta();
		// tr.IdSistema = s.Value;
		// ServiceServiciosCD40.DataSetSelectSQL(tr).WriteXml(Server.MapPath("~/Informes/TroncalesRutas.xml"));
	}

    /// <summary>
    /// 
    /// </summary>
    private void CargaDDL()
    {
        try
        {
            ServiciosCD40.Redes r = new ServiciosCD40.Redes();
            r.IdSistema = (string)Session["idsistema"];
			DDLPrefijo.DataSource = ServiceServiciosCD40.DataSetSelectSQL(r);
            DDLPrefijo.DataTextField = "IdRed";
            DDLPrefijo.DataValueField = "IdPrefijo";
            DDLPrefijo.DataBind();

            ServiciosCD40.GruposTelefonia gTelefonia = new ServiciosCD40.GruposTelefonia();
            gTelefonia.IdSistema = (string)Session["idsistema"];
			DDLGrupo.DataSource = ServiceServiciosCD40.DataSetSelectSQL(gTelefonia);
            DDLGrupo.DataTextField = "IdGrupo";
            DDLGrupo.DataBind();
        }
        catch (Exception e)
        {
            logDebugView.Error("(DestinosTelefonia-CargaDDL):", e);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BNuevo_Click(object sender, EventArgs e)
    {
        Modificando = false;
        NuevoDestino(true);
        //MostrarMenu();
        RequiredFieldValidator1.Visible = true;
        RequiredFieldValidator2.Visible = true;
        ValidationSummary1.Visible = true;
        TBRecurso.Text = "";
        DDLRecursos.Visible = false;
        TBRecurso.Visible = false;
        LblRecurso.Visible = false;
        LblRecursosLibres.Visible = false;
        IndexListBox1 = LBDestinos.SelectedIndex;
        MostrarValidacion();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="habilita"></param>
    private void NuevoDestino(bool habilita)
    {
        if (habilita)
        {
            LBDestinos.Enabled = false;

            //Si se está dando de alta un nuevo destino
            if (!Modificando)
            {
                TBDestino.Text = "";
                TBDestino.ReadOnly = false;
                DDLPrefijo.SelectedValue = "-1";
                TBGrupo.Text = "";
                DDLGrupo.SelectedValue = "-1";
                TBAbonado.Text = "";

                if (DDLPrefijo.SelectedValue != "1" && DDLPrefijo.SelectedValue != "3" && DDLPrefijo.SelectedValue != "-1")
                {
                    LbGrupo.Visible = true;
                    TBGrupo.Visible = true;
                    DDLGrupo.Visible = true;
                }
                else
                {
                    LbGrupo.Visible = false;
                    TBGrupo.Visible = false;
                    DDLGrupo.Visible = false;
                }
            }
            else
            {
				RequiredFieldValidator3.Visible = TBRecurso.Visible = LblRecursosLibres.Visible = 
						LblRecurso.Visible = DDLRecursos.Visible = DDLPrefijo.SelectedValue == "32" || DDLPrefijo.SelectedValue == "1";

                if (DDLPrefijo.SelectedValue != "1" && DDLPrefijo.SelectedValue != "3")
                {
                    DDLGrupo.Visible = true;
                    TBGrupo.Enabled = true;
                }
                else
                {
                    DDLGrupo.Visible = false;
                    TBGrupo.Enabled = false;

                }
            }

            Panel1.Enabled = true;
            DDLPrefijo.Enabled = !Modificando;
            //TBGrupo.Enabled = true;
            BNuevo.Visible = false;
            BEliminar.Visible = false;
            BAnadir.Visible = true;
            BCancelar.Visible = true;
            BModificar.Visible = false;
            DDLRecursos.Enabled = true;
            TBAbonado.Enabled = DDLPrefijo.SelectedValue != "1";
        }
        else
        {
            LBDestinos.Enabled = true;
            TBDestino.Text = "";
            TBDestino.ReadOnly = true;
            DDLPrefijo.SelectedValue = "-1";
            TBGrupo.Text = "";
            DDLGrupo.SelectedValue = "-1";
            TBAbonado.Text = "";
            TBAbonado.Enabled = false;

            BNuevo.Visible = PermisoSegunPerfil;
			BEliminar.Visible = PermisoSegunPerfil && LBDestinos.Items.Count > 0;
            BAnadir.Visible = false;
            BCancelar.Visible = false;
            BModificar.Visible = PermisoSegunPerfil && LBDestinos.Items.Count > 0;
            LBDestinos.SelectedIndex = -1;

            Panel1.Enabled = false;

            if (TBGrupo.Visible)
                TBGrupo.Enabled = false;

            if (DDLGrupo.Visible)
                DDLGrupo.Visible = false;

            DDLPrefijo.Enabled = false;
            DDLRecursos.Visible = false;
            DDLRecursos.Enabled = false;
            LblRecurso.Visible = false;
            LblRecursosLibres.Visible = false;
            TBRecurso.Visible = false;
            RequiredFieldValidator3.Enabled = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BAnadir_Click(object sender, EventArgs e)
    {
        string StrSistema = (string)Session["idsistema"];

        if (!Modificando && TBDestino.Enabled && bIdentificadorAsignado(StrSistema, TBDestino.Text))
        {
            //Existe otro destino radio o de telefonia con el mismo identificaodr
            cMsg.alert((string)GetLocalResourceObject("DestinoExiste.Text"));
            return;
        }

        GuardarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AceptarCambios"), "aceptparam");
    }
    /// <summary>
    /// 
    /// </summary>
    private void GuardarCambios()
    {
        NewItem = TBDestino.Text;

        if (Modificando)
        {
            IndexListBox1 = LBDestinos.SelectedIndex;
            ModificaDestino();
        }
        else
        {
            AnadeDestino();

            ActualizaWebPadre(true);
        }

        try
        {
            // Llamada asíncrona para regenerar todas las sectorizaciones.
            Session.Add("Sectorizando", true);
            ServiceServiciosCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, false, true, CallbackCompletado, null);
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TFTRadio-BtLiberar_Click): ", ex);
        }

        NuevoDestino(false);
        MuestraDatos();
        //EsconderMenu();
        //BModificar.Visible = false;
        //BEliminar.Visible = false;
        OcultarValidacion();
    }
    /// <summary>
    /// 
    /// </summary>
    protected void AnadeDestino()
    {
        try
        {
			/*
			// Si el grupo no está en la lista, se añade a la base de datos.
            if (TBGrupo.Text.Length > 0 && DDLGrupo.Items.FindByText(TBGrupo.Text) == null)
            {
                ServiciosCD40.GruposTelefonia gTel = new ServiciosCD40.GruposTelefonia();
                gTel.IdSistema = (string)Session["idsistema"];
                gTel.IdGrupo = TBGrupo.Text;

				if (ServiceServiciosCD40.InsertSQL(gTel) < 0)
                    logDebugView.Warn("(DestinosTelefonia-AnadeDestino): No se han podido insertar el Grupo de Telefonia(InsertSQL)");
            }
			*/

            ServiciosCD40.DestinosExternos dExterno = new ServiciosCD40.DestinosExternos();
            dExterno.IdSistema = (string)Session["idsistema"];
            dExterno.IdDestino = TBDestino.Text;
            dExterno.TipoDestino = 1;
            dExterno.IdPrefijo = Convert.ToUInt32(DDLPrefijo.SelectedValue);
            dExterno.IdAbonado = (TBAbonado.Text.Length > 0) ? TBAbonado.Text : null;
            
            /**
             * AGL 2012.06.15. ID.125. "No se anade el grupo a la tabla de Destinos de Telefonía"
             *      nota: siguiendo el código, no veo donde se hace el mencionado "base.update()"
             */
            // Aunque IdGrupo no es de la tabla DestinosExternos, la clase hereda de 
            // DestinosTelefonía y se hace una llamada a base.Insert()
			dExterno.IdGrupo = (TBGrupo.Text.Length > 0) ? TBGrupo.Text : null;

            
			ServiceServiciosCD40.AnadeDestino(dExterno, TBRecurso.Text, TBGrupo.Text.Length > 0 && DDLGrupo.Items.FindByText(TBGrupo.Text) == null);

			/*
			if (ServiceServiciosCD40.InsertSQL(dExterno) < 0)
                logDebugView.Warn("(DestinosTelefonia-AnadeDestino): No se han podido insertar el Destino Externo de Telefonia(InsertSQL)");                
            if (dExterno.IdPrefijo == 1)  // LCEN
                ActualizaRecursoLCEN(dExterno, TBRecurso.Text);
            else if (dExterno.IdPrefijo == 32)    // PP
                ActualizaRecursoTelefonia(dExterno, TBRecurso.Text);
			*/

		
			#region Sincroniza CD30
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
            if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
            {
                SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                int prefijoRed=0;
                switch (Convert.ToUInt32(DDLPrefijo.SelectedValue))
                {
                    case 1://LCEN
                        prefijoRed = 2;
                        if (TBAbonado.Text.Length > 0) TBAbonado.Text = "";
                        break;
                    case 32://PP
                        prefijoRed = 5;
                        if (TBAbonado.Text.Length > 0) TBAbonado.Text = "";
                        break;
                    case 3://ATS
                        prefijoRed = 3;                        
                        break;
                    case 4://RTB
                        prefijoRed = 4;
                        break;
                    case 6://PABX
                        prefijoRed = 6;
                        break;
                    case 8:
                        prefijoRed = 8;
                        break;
                    case 9:
                        prefijoRed = 9;
                        break;
                    default:
                        break;
                }
                if (prefijoRed != 0)
                {
                    switch (sincro.AltaDestino(TBDestino.Text, prefijoRed, TBRecurso.Text, (TBGrupo.Text.Length > 0 ? Int32.Parse(TBGrupo.Text) : 0), (TBAbonado.Text.Length > 0) ? TBAbonado.Text : ""))
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
        catch (Exception e)
        {
            logDebugView.Error("(DestinosTelefonia-AnadeDestino):", e);
        }
    }
    /// <summary>
    /// 
    /// </summary>
	protected void ModificaDestino()
	{
        // *** DestinoExternoTelefonía => Modificación en DestinosTelefonía
		ServiciosCD40.DestinosExternos dExterno = new ServiciosCD40.DestinosExternos();
		dExterno.IdSistema = (string)Session["idsistema"];
		dExterno.IdDestino = TBDestino.Text;
		dExterno.TipoDestino = 1;
		dExterno.IdPrefijo = Convert.ToUInt32(DDLPrefijo.SelectedValue);
		dExterno.IdAbonado = (TBAbonado.Text.Length > 0) ? TBAbonado.Text : null;

        /**
         * AGL 2012.06.15. ID.125. "No se anade el grupo a la tabla de Destinos de Telefonía"
         *      nota: siguiendo el código, no veo donde se hace el mencionado "base.update()"
         */
        // Aunque IdGrupo no es de la tabla DestinosExternos, 
        // la clase hereda de DestinosTelefonía y se hace una llamada a base.Update()
		dExterno.IdGrupo = (TBGrupo.Text.Length > 0) ? TBGrupo.Text : null;
		ServiceServiciosCD40.ActualizaDestino(dExterno, TBRecurso.Text, TBGrupo.Text.Length > 0 && DDLGrupo.Items.FindByText(TBGrupo.Text) == null);
        
		#region Sincroniza CD30
		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
		if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
		{
			SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
			int prefijoRed = 0;
			switch (Convert.ToUInt32(DDLPrefijo.SelectedValue))
			{
				case 1://LCEN
					prefijoRed = 2;
					if (TBAbonado.Text.Length > 0) TBAbonado.Text = "";
					break;
				case 32://PP
					prefijoRed = 5;
					if (TBAbonado.Text.Length > 0) TBAbonado.Text = "";
					break;
				case 3://ATS
					prefijoRed = 3;
					break;
				case 8:
					prefijoRed = 8;
					break;
				case 9:
					prefijoRed = 9;
					break;
				default:
					break;
			}
			if (prefijoRed != 0)
			{
				switch (sincro.ModificacionDestino(TBDestino.Text, prefijoRed, TBRecurso.Text, (TBGrupo.Text.Length > 0 ? Int32.Parse(TBGrupo.Text) : 0), (TBAbonado.Text.Length > 0) ? TBAbonado.Text : ""))
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

	/*
    private void ActualizaRecursoTelefonia(ServiciosCD40.DestinosExternos dExterno, string idRecurso)
    {
        try
        {
            ServiciosCD40.RecursosTF rTelefonia = new ServiciosCD40.RecursosTF();
            rTelefonia.IdSistema = dExterno.IdSistema;
            rTelefonia.IdRecurso = idRecurso;
            rTelefonia.IdDestino = dExterno.IdDestino;
            rTelefonia.IdPrefijo = dExterno.IdPrefijo;
            rTelefonia.TipoDestino = dExterno.TipoDestino;

			ServiceServiciosCD40.AsignaDestinoARecurso((ServiciosCD40.ParametrosRecursoGeneral)rTelefonia);
        }
        catch (Exception e)
        {
            logDebugView.Error("(DestinosTelefonia-ActualizaRecursoTelefonia):", e);
        }
    }

    private void ActualizaRecursoLCEN(ServiciosCD40.DestinosExternos dExterno, string idRecurso)
    {
        try
        {
            ServiciosCD40.RecursosLCEN rTelefonia = new ServiciosCD40.RecursosLCEN();
            rTelefonia.IdSistema = dExterno.IdSistema;
            rTelefonia.IdRecurso = idRecurso;
            rTelefonia.IdDestino = dExterno.IdDestino;
            rTelefonia.IdPrefijo = dExterno.IdPrefijo;
            rTelefonia.TipoDestino = dExterno.TipoDestino;

			ServiceServiciosCD40.AsignaDestinoARecurso((ServiciosCD40.ParametrosRecursoGeneral)rTelefonia);
        }
        catch (Exception e)
        {
            logDebugView.Error("(DestinosTelefonia-ActualizaRecursoLCEN):", e);
        }
    }
	*/

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BEliminar_Click(object sender, EventArgs e)
    {
        if (LBDestinos.SelectedValue != "")
        {
            //string texto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarDestino"), LBDestinos.SelectedValue);
            IndexListBox1 = LBDestinos.SelectedIndex;
            Session["elemento"] = LBDestinos.SelectedValue;
            EliminarElemento(false);
            //cMsg.confirm(texto, "eliminaelemento");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void EliminarElemento(bool forced)
    {
        if (LBDestinos.SelectedIndex >= 0 && Session["idsistema"]!=null)
        {
            ServiciosCD40.Destinos d = new ServiciosCD40.Destinos();

            uint iPrefijo= Convert.ToUInt32(DDLPrefijo.SelectedValue);
            string strSistema=string.Empty;
            System.Text.StringBuilder strMsg = new System.Text.StringBuilder();

            strSistema=(string)Session["idsistema"];

            d.IdSistema = strSistema;
            d.IdDestino = TBDestino.Text;
            d.TipoDestino = 1;

            if (iPrefijo == 1 && DestinoLCENAsignadoPanelLC(strSistema, TBDestino.Text, iPrefijo, ref strMsg))
            {
                    cMsg.confirm(strMsg.ToString(), "aceptparam");
                    return;
            }

            if (forced || !DestinoAsignadoATft(strSistema,TBDestino.Text))
            {
                // Liberar el destino del recurso
                if (iPrefijo== 1)   // Si el destino es LCEN
                {
                    ServiciosCD40.RecursosLCEN rLc = new ServiciosCD40.RecursosLCEN();
                    rLc.IdSistema = (string)Session["idsistema"];
                    rLc.IdDestino = TBDestino.Text;
                    rLc.TipoDestino = 1;
                }
                else if (iPrefijo >= 32)
                {
                    ServiciosCD40.RecursosTF rTf = new ServiciosCD40.RecursosTF();
                    rTf.IdSistema = (string)Session["idsistema"];
                    rTf.IdDestino = TBDestino.Text;
                    rTf.TipoDestino = 1;
                }

                if (ServiceServiciosCD40.DeleteSQL(d) < 0)
                    logDebugView.Warn("(DestinosTelefonia-EliminarElemento): No se han borrado los elementos(DeleteSQL)");
                else
                {
                    //Comprobamos si todavía existe el destino de telefonía
                    //En alguna ocasión, me he entrado el destino de telefonía sin el destino correspondiente configurado
                    //Por esta razón, si el registro de la tabla destinostelefonía sigue existiendo se elimina.
                    ServiciosCD40.DestinosTelefonia destTelf = new ServiciosCD40.DestinosTelefonia();
                    destTelf.IdSistema = (string)Session["idsistema"];
                    destTelf.IdDestino = TBDestino.Text;
                    destTelf.TipoDestino = 1;
                    destTelf.IdPrefijo = iPrefijo;

                    ServiciosCD40.Tablas[] lista = ServiceServiciosCD40.ListSelectSQL(destTelf);

                    if (lista != null && lista.Length > 0)
                    {
                        //Borramos el destino
                        ServiceServiciosCD40.DeleteSQL(destTelf);
                    }
                   
                }


                #region Sincroniza CD30
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    int prefijoRed = 0;
                    switch (Convert.ToUInt32(DDLPrefijo.SelectedValue))
                    {
                        case 1://LCEN
                            prefijoRed = 2;
                            break;
                        case 32://PP
                            prefijoRed = 5;
                            break;
                        case 3://ATS
                            prefijoRed = 3;
                            break;
                        case 8:
                            prefijoRed = 8;
                            break;
                        case 9:
                            prefijoRed = 9;
                            break;
                        default:
                            break;
                    }
                    if (prefijoRed != 0)
                    {
                        switch (sincro.BajaDestino(TBDestino.Text, prefijoRed))
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

                NuevoDestino(false);
                MuestraDatos();

                // Regenerar todas las sectorizaciones excepto la activa porque puede que este destino 
                // estuviera presente en alguna sectorización
                ServiceServiciosCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, true, CallbackCompletado, null);
            }
            else
            {
                cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "PanelesConDestinoAsignado"), "SoloEliminaDeTFT");
            }
		}
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");        
    }

    /// <summary>
    /// 
    /// </summary>
	protected override void AceptarCambios()
	{
		base.AceptarCambios();
	}

    /// <summary>
    /// 
    /// </summary>
    protected override void CancelarCambios()
    {
		NuevoDestino(false);
        //EsconderMenu();
        BAnadir.Visible = false;
        BCancelar.Visible = false;
		BEliminar.Visible = BModificar.Visible = PermisoSegunPerfil && LBDestinos.Items.Count > 0;
		BNuevo.Visible = PermisoSegunPerfil;
        RequiredFieldValidator1.Visible = false;
        RequiredFieldValidator2.Visible = false;
        RequiredFieldValidator3.Visible = false;
        ValidationSummary1.Visible = false;
        OcultarValidacion();
        if (LBDestinos.Items.Count > 0)
            LBDestinos.SelectedIndex = IndexListBox1 != -1 ? IndexListBox1 : 0;

		MuestraDatos();
    }

    /// <summary>
    /// 
    /// </summary>
	private void MuestraElemento()
	{
		try
		{
            uint iIdPrefijo=0;
			//MostrarMenu();

			//            BEliminar.Enabled = PermisoSegunPerfil;

			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
			Session["idsistema"] = s.Value;
			System.Data.DataRow dr = ServiceServiciosCD40.DestinosDeTelefoniaEnElSistema(s.Value).Tables[0].Rows[LBDestinos.SelectedIndex];

			//            BModificar.Enabled = LBDestinos.SelectedIndex != -1;
			//            BModificar.Visible = LBDestinos.SelectedIndex != -1 && PermisoSegunPerfil;

            BEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarDestino"), LBDestinos.SelectedValue);
            
            //DestinosTelefonia dt = new DestinosTelefonia();
			TBDestino.Text = (string)dr["IdDestino"];

            iIdPrefijo=Convert.ToUInt32(dr["IdPrefijo"]);

			DDLPrefijo.SelectedValue = Convert.ToString((uint)dr["IdPrefijo"]);

            if (iIdPrefijo != 1 && iIdPrefijo != 3)
            {
                LbGrupo.Visible = true;
                TBGrupo.Visible = true;
                if (dr["IdGrupo"] != System.DBNull.Value)
                {
                    TBGrupo.Text = (string)dr["IdGrupo"];

                    if (DDLGrupo.Items.FindByText(TBGrupo.Text)!=null)
                        DDLGrupo.SelectedValue = TBGrupo.Text;
                }
                else
                {
                    TBGrupo.Text = "";
                    DDLGrupo.SelectedValue = "-1";
                }
            }
            else
            {
                LbGrupo.Visible = false;
                TBGrupo.Visible = false;
                TBGrupo.Text = "";
                DDLGrupo.SelectedValue = "-1";
            }

			if (dr["IdAbonado"] != System.DBNull.Value)
				TBAbonado.Text = (string)dr["IdAbonado"];
			else
				TBAbonado.Text = "";

			// Buscar recurso asignado al destino
            if (iIdPrefijo == 32)   // PP
			{
				DDLRecursos.Visible = true;
				LblRecursosLibres.Visible = true;
				LblRecurso.Visible = true;
				TBRecurso.Visible = true;
				TBAbonado.Visible = false;
				Label5.Visible = false;

				CompletaDDLRecursos(32);

				ServiciosCD40.RecursosTF rt = new ServiciosCD40.RecursosTF();
				rt.IdSistema = (string)Session["idsistema"];
				rt.IdDestino = TBDestino.Text;
				rt.IdPrefijo = 32;

				ServiciosCD40.Tablas[] recurso = ServiceServiciosCD40.ListSelectSQL(rt);
				if (recurso.Length > 0)
				{
					TBRecurso.Text = ((ServiciosCD40.RecursosTF)recurso[0]).IdRecurso;
				}
				else
				{
					DDLRecursos.SelectedIndex = 0;
					TBRecurso.Text = "";
				}
			}
			else
                if (iIdPrefijo == 1) // LCEN
				{
					DDLRecursos.Visible = true;
					LblRecurso.Visible = true;
					LblRecursosLibres.Visible = true;
					TBRecurso.Visible = true;
					TBAbonado.Visible = false;
					Label5.Visible = false;

					CompletaDDLRecursos(1);

					ServiciosCD40.RecursosLCEN rt = new ServiciosCD40.RecursosLCEN();
					rt.IdSistema = (string)Session["idsistema"];
					rt.IdDestino = TBDestino.Text;
					rt.IdPrefijo = 1;

					ServiciosCD40.Tablas[] recurso = ServiceServiciosCD40.ListSelectSQL(rt);
					if (recurso.Length > 0)
					{
						TBRecurso.Text = ((ServiciosCD40.RecursosLCEN)recurso[0]).IdRecurso;
					}
					else
					{
						DDLRecursos.SelectedIndex = 0;
						TBRecurso.Text = "";
					}
				}
				else
				{
					TBAbonado.Visible = true;
					Label5.Visible = true;
					DDLRecursos.Visible = false;
					LblRecursosLibres.Visible = false;
					LblRecurso.Visible = false;
					TBRecurso.Visible = false;
					RequiredFieldValidator3.Enabled = false;
				}
		}
		catch (Exception ex)
		{
			logDebugView.Error("(DestinosTelefonia-LBDestinos_IndexChanged):", ex);
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LBDestinos_IndexChanged(object sender, EventArgs e)
    {
        MuestraElemento();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DDLGrupo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLGrupo.SelectedIndex != 0)
            TBGrupo.Text = DDLGrupo.SelectedValue;
        else
            TBGrupo.Text = "";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BModificar_Click(object sender, EventArgs e)
    {
        IndexListBox1 = LBDestinos.SelectedIndex;

        Modificando = true;
        NuevoDestino(true);
        MostrarValidacion();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DDLPrefijo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPrefijo=((DropDownList)sender).SelectedValue;

        switch(strPrefijo)
        {
            case "32":   //PP
                DDLRecursos.Visible = true;
                LblRecurso.Visible = true;
                LblRecursosLibres.Visible = true;
                TBRecurso.Visible = true;
                RequiredFieldValidator3.Enabled = true;
                TBRecurso.Text = "";
                TBAbonado.Visible = false;
                Label5.Visible = false;

                CompletaDDLRecursos(32);
                break;

            case "1": // LCEN
                DDLRecursos.Visible = true;
                LblRecurso.Visible = true;
                LblRecursosLibres.Visible = true;
                TBRecurso.Visible = true;
                RequiredFieldValidator3.Enabled = true;
                TBRecurso.Text = "";
                TBAbonado.Visible = false;
                Label5.Visible = false;

                CompletaDDLRecursos(1);
                break;

            default:
                TBAbonado.Enabled = true;
                DDLRecursos.Visible = false;
                LblRecurso.Visible = false;
                LblRecursosLibres.Visible = false;
                TBRecurso.Visible = false;
                RequiredFieldValidator3.Enabled = false;
                TBAbonado.Visible = true;
                Label5.Visible = true;
                break;
        }

        if (strPrefijo == "3" || strPrefijo == "1" || strPrefijo == "-1")
        {
            LbGrupo.Visible = false;
            TBGrupo.Visible = false;
            DDLGrupo.Visible = false;
            if (TBGrupo.Text.Length > 0)
            {
                TBGrupo.Text = String.Empty;
                DDLGrupo.SelectedIndex = -1;
            }
        }
        else
        {
            LbGrupo.Visible = true;
            TBGrupo.Visible = true;
            DDLGrupo.Visible = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefijo"></param>
    private void CompletaDDLRecursos(int prefijo)
    {
        try
        {
            ServiciosCD40.DestinosTelefonia dTelefonia = new ServiciosCD40.DestinosTelefonia();

            while (DDLRecursos.Items.Count != 1)
            {
                DDLRecursos.Items.RemoveAt(1);
            }
            //DDLRecursos.Items.Add(new ListItem((string)GetLocalResourceObject("ListItemResource5"),"-1"));
			DDLRecursos.DataSource = ServiceServiciosCD40.RecursosSinAsignarAEnlaces((string)Session["idsistema"], (prefijo == 32) ? 1 : 2); // TF o LCEN
            DDLRecursos.DataTextField = "IDRecurso";
            DDLRecursos.DataBind();
        }
        catch (Exception e)
        {
            logDebugView.Error("(DestinosTelefonia-CompletaDDLRecursos):", e);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DDLRecursos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLRecursos.SelectedIndex != 0)
            TBRecurso.Text = DDLRecursos.SelectedValue;
        else
            TBRecurso.Text = "";
    }

    /// <summary>
    /// 
    /// </summary>
    private void MostrarMenu()
    {
		//Label2.Visible = true;
		//Label3.Visible = true;
		//Label4.Visible = true;
        //Label5.Visible = true;
        //LblRecurso.Visible = true;
        //LblRecursosLibres.Visible = true;
        //TBDestino.Visible = true;
        //TBGrupo.Visible = true;
		DDLGrupo.Visible = true;
        //TBAbonado.Visible = true;

        //TBRecurso.Visible = true;
        //DDLPrefijo.Visible = true;
        //DDLRecursos.Visible = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void EsconderMenu()
    {
		//Label5.Visible = false;
		DDLGrupo.Visible = false;
        //TBAbonado.Visible = false;
        //DDLPrefijo.Visible = false;
        //DDLRecursos.Visible = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void MostrarValidacion()
    {
        ValidationSummary1.Visible = true;
        RequiredFieldValidator1.Visible = true;
        RequiredFieldValidator2.Visible = true;
        RequiredFieldValidator3.Visible = true;
    }
    /// <summary>
    /// 
    /// </summary>
    private void OcultarValidacion()
    {
        ValidationSummary1.Visible = false;
        RequiredFieldValidator1.Visible = false;
        RequiredFieldValidator2.Visible = false;
        RequiredFieldValidator3.Visible = false;    
    }

    private void EliminaDestinoDeTFT()
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];

        ServiciosCD40.DestinosExternosSector drs = new ServiciosCD40.DestinosExternosSector();
        drs.IdSistema = s.Value;
        drs.IdDestino = TBDestino.Text;

        // Eliminar de todos los TFT el destino
        ServiceServiciosCD40.DeleteSQL(drs);
    }


    private bool DestinoAsignadoATft(string strSistema,string destino)
    {
        ServiciosCD40.DestinosExternosSector drs = new ServiciosCD40.DestinosExternosSector();
        drs.IdSistema = strSistema;
        drs.IdDestino = destino;

        ServiciosCD40.Tablas[] lista = ServiceServiciosCD40.ListSelectSQL(drs);

        if (lista != null && lista.Length > 0)
            return true;

        return false;
    }

    private bool DestinoLCENAsignadoPanelLC(string strSistema,string strDestino,uint idPrefijo, ref System.Text.StringBuilder strMsg)
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

                if (GetGlobalResourceObject("Espaniol", "DestinoLCENAsignadoEnPanelLC")!=null)
                    strMsgFormato.Append((string)GetGlobalResourceObject("Espaniol", "DestinoLCENAsignadoEnPanelLC"));
                else
                    strMsgFormato.Append("El destino de línea caliente externa {0} se encuentra asignado al destino ATS {1} en el panel de línea caliente del sector {2}. Por favor, libere el destino {0} antes de eliminarlo");
                
                if (strMsg.Length>0)
                    strMsg.Clear();

                strMsg.AppendFormat(strMsgFormato.ToString(),strDestino,strDestinoATS,strSector);
                strMsgFormato.Clear();
            }

        }
        catch (Exception)
        {
        }

        return bExiste;
    }


	// Respuesta a la regeneración de las sectorizaciones
    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
	private void OnCallBackCompleted(IAsyncResult result)
	{
		try
		{
			int retorno = ServiceServiciosCD40.EndRegeneraSectorizaciones(result);
			Session.Add("Sectorizando", false);
		}
		catch (System.Web.Services.Protocols.SoapException soapException)
		{
			logDebugView.Error("(DestinosTelefonia-OnCallBackCompleted): ", soapException);
		}
	}
    //Devuelve true si ya existe algun destino en el sistema con el mismo identificador. En caso contrario, false.
    private bool bIdentificadorAsignado(string strIdSistema, string strIdentificador)
    {
        bool bExiste = false;
        int iExiste = -1;

        // Se comprueba que no existe otro destino radio o de telefonía con el mismo identificador 
        iExiste = ServiceServiciosCD40.CheckIdentificadorAsignado("D", strIdSistema, strIdentificador);

        if (iExiste > 0)
            bExiste = true;
        else if (iExiste < 0)
        {
            System.Text.StringBuilder strMsgError = new System.Text.StringBuilder();
            strMsgError.AppendFormat("(DestinosTelefonia-bIdentificadorAsignado): el servicio servicioCD40.CheckIdentificadorAsignado('D', '{0}', '{1}') ha devuelto el codigo {2}", strIdSistema, strIdentificador, iExiste);
            logDebugView.Error(strMsgError.ToString());
            strMsgError.Clear();
        }

        return bExiste;
    }

}
