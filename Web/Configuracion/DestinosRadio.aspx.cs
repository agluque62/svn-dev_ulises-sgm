using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using log4net;
using log4net.Config;

public partial class DestinosRadio : PageBaseCD40.PageCD40	// System.Web.UI.Page
{
    const string DESTINORADIO_TIPOFRECUENCIA_VHF    = "0";
    const string DESTINORADIO_TIPOFRECUENCIA_UHF    = "1";
    const string DESTINORADIO_TIPOFRECUENCIA_HF     = "2";

    private static ServiciosCD40.Tablas[] datos;
    private static ILog _logDebugView;
	private static KeyValueConfigurationElement s;
    static bool PermisoSegunPerfil;
	private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();
	private AsyncCallback CallbackCompletado;
    private Ulises5000Configuration.ToolsUlises5000Section UlisesToolsVersion;
    private static string Frecuencia = "";


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

			PermisoSegunPerfil = BtModificar.Visible = BtNuevo.Visible = perfil != "1";
            //Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            //Version = config.AppSettings.Settings["Version"];

            Ulises5000Configuration.ToolsUlises5000Section UlisesTools = Ulises5000Configuration.ToolsUlises5000Section.Instance;

            UlisesToolsVersion = UlisesTools;
        }

		if (CallbackCompletado == null)
			CallbackCompletado = new AsyncCallback(OnCallBackCompleted);

        if (!IsPostBack)
        {
            IndexListBox1 = -1;

            // Mostrar Tipo destino radio HF sólo para NDjamena (Versión=2)
            if (UlisesToolsVersion.Tools["RadioHF"] == null)
                DListTipo.Items.RemoveAt(2);

            logDebugView.Debug("Entrando en DestinosRadio....");
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            MuestraDatos(DameDatos());
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
    }



    protected void BtNuevo_Click(object sender, EventArgs e)
    {
        Panel1.Enabled = true;

		RequiredFieldIdentificador.Enabled = RequiredFieldIdentificador.Visible = true;
		//RequiredFieldValidator1.Enabled = RequiredFieldValidator1.Visible = true;
        errores.Visible = true;
        MostrarMenu();
        DListTipo.Enabled = true;   /* CheckExclusividad.Enabled = true; */
		BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        BtModificar.Visible = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        ListBox1.Enabled = false;
        TxtIdEnlace.Enabled = true;
        TxtIdEnlace.ReadOnly = false;
        Label4.Visible = true;
        ListRecursos.Items.Clear();
        CargarRecursosSinAsignar();
        ListRecursosLibres.Visible = true;
        IButAsignar.Visible = true;
        IButQuitar.Visible = true;
	}

    private void CargarRecursosSinAsignar()
    {
        try
        {
            ListRecursosLibres.Items.Clear();

            ServiciosCD40.Tablas[] d = ServicioCD40.RecursosSinAsignarAEnlaces1((string)Session["idsistema"], 0, null);
            if (d !=null)
                for (int i = 0; i < d.Length; i++)
                {
                    if (DListTipo.SelectedValue == DESTINORADIO_TIPOFRECUENCIA_HF)
                    {
                        // Si el destino es de HF, el recurso asociado solo puede ser AUDIO_RX
                        if (((ServiciosCD40.Recursos)d[i]).Tipo == 0)
                            ListRecursosLibres.Items.Add(((ServiciosCD40.Recursos)d[i]).IdRecurso);
                    }
                    else
                    {
                        // Mostrar Tipo recurso radio Audio HF-Tx sólo para NDjamena (Versión=2)
                        // if(((ServiciosCD40.Recursos)d[i]).Tipo == 3 /* Audio-HF-Tx */ &&
                        //    Version.Value != "2" /* NDjamena */ &&
                        //    Version.Value != "0" /* General */)
                        if (((ServiciosCD40.Recursos)d[i]).Tipo == 3 /* Audio-HF-Tx */ && 
                            UlisesToolsVersion.Tools["RadioHF"] == null)
                            continue;

                        ListRecursosLibres.Items.Add(((ServiciosCD40.Recursos)d[i]).IdRecurso);
                    }
                }
        }
        catch (Exception e)
        {
            logDebugView.Error("(DestinosRadio-CargarRecursosSinAsignar):", e);
        }
    }

    private void CargarRecursos()
    {
        try
        {
            ServiciosCD40.RecursosRadio t= new ServiciosCD40.RecursosRadio();
            t.IdSistema = (string)Session["idsistema"];
            t.IdDestino = TxtIdEnlace.Text;
            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            LblErrorMismatchFrequency.Visible = false;
            Frecuencia = string.Empty;

            if (d != null)
            {
                for (int i = 0; i < d.Length; i++)
                {
                    ListRecursos.Items.Add(((ServiciosCD40.RecursosRadio)d[i]).IdRecurso);
                    if (((ServiciosCD40.RecursosRadio)d[i]).Tipo >= 4 && ((ServiciosCD40.RecursosRadio)d[i]).Tipo <= 6) // Tipo M+N
                    {
                        ServiciosCD40.HFParams r = new ServiciosCD40.HFParams();

                        r.IdSistema = (string)Session["idsistema"];
                        r.IdRecurso = ((ServiciosCD40.RecursosRadio)d[i]).IdRecurso;
                        ServiciosCD40.Tablas[] h = ServicioCD40.ListSelectSQL(r);

                        if (h.Length > 0)
                        {
                            if (Frecuencia == string.Empty)
                                Frecuencia = ((ServiciosCD40.HFParams)h[0]).Frecuencia;
                            else if (Frecuencia != ((ServiciosCD40.HFParams)h[0]).Frecuencia)
                            {
                                LblErrorMismatchFrequency.Visible = true;
                            }
                        }

                    }
                }
            }
        }
        catch (Exception e)
        {
            logDebugView.Error("(DestinosRadio-CargarRecursos):", e);
        }
    }

    private void MostrarElemento()
    {
        MostrarMenu();
        TxtIdEnlace.ReadOnly = true;
        TxtIdEnlace.Enabled = false;
        DListTipo.Enabled = true;   // CheckExclusividad.Enabled = false;
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
		BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
        ListRecursos.Items.Clear();
        for (int i = 0; i < datos.Length; i++)
        {
            if (String.Compare((((ServiciosCD40.DestinosRadio)datos[i]).IdDestino), ListBox1.SelectedValue) == 0)
            {
                BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarDestino"), ListBox1.SelectedValue);

                TxtIdEnlace.Text = ((ServiciosCD40.DestinosRadio)datos[i]).IdDestino;
                //CheckExclusividad.Checked = ((ServiciosCD40.DestinosRadio)datos[i]).ExclusividadTXRX;
				DListTipo.SelectedValue = ((ServiciosCD40.DestinosRadio)datos[i]).TipoFrec.ToString();
                TblTunedFreq.Visible = ((ServiciosCD40.DestinosRadio)datos[i]).TipoFrec == Convert.ToUInt32(DESTINORADIO_TIPOFRECUENCIA_HF);
                if (TblTunedFreq.Visible)
                {
                    TextBox tb = (TextBox)TblTunedFreq.FindControl("TbTunedFrequency");
                    if (tb != null)
                    {
                        tb.Text = ((ServiciosCD40.DestinosRadio)datos[i]).Frecuencia.ToString();
                    }
                }
                CargarRecursos();
                return;
            }
        }
    }

    private void MostrarMenu()
    {
        LimpiarMenu();
        Label2.Visible = true;
        Label3.Visible = true;
        Label5.Visible = true;
        TxtIdEnlace.Visible = true;
        DListTipo.Visible = true;
        ListRecursos.Visible = true;
        //CheckExclusividad.Visible = true;
    }

    private void EsconderMenu()
    {
        Label2.Visible = false;
        Label3.Visible = false;
        Label5.Visible = false;       
        DListTipo.Visible = false;
        IButAsignar.Visible = false;
        IButQuitar.Visible = false;
        ListRecursos.Visible = false;
        Label4.Visible = false;
        ListRecursosLibres.Visible = false;
        ListRecursos.Items.Clear();
        ListRecursosLibres.Items.Clear();
        TxtIdEnlace.Visible = false;
        LimpiarMenu();
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        BtModificar.Visible = false;
		RequiredFieldIdentificador.Enabled = RequiredFieldIdentificador.Visible = false;
		//RequiredFieldValidator1.Enabled = RequiredFieldValidator1.Visible = false;
		errores.Visible = false;
        //CheckExclusividad.Visible = false;
        TblTunedFreq.Visible = false;
    }

    private void LimpiarMenu()
    {
        TxtIdEnlace.Text = "";
        TBTunedFrequency.Text = string.Empty;
    }

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            BtEliminar.Visible = PermisoSegunPerfil;
            MostrarElemento();
        }
    }

    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.DestinosRadio t = new ServiciosCD40.DestinosRadio();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;

            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            datos = d;
            return d;    
        }
        catch (Exception e)
        {
            logDebugView.Error("(DestinosRadio-DameDatos):", e);
        }
        return null;
    }

    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
       // LError.Visible = false;

        ListBox1.Items.Clear();
        if (nu!=null)
            for (int i = 0; i < nu.Length; i++)
            {
                if (((ServiciosCD40.DestinosRadio)nu[i]).TipoFrec.ToString() == DESTINORADIO_TIPOFRECUENCIA_HF /* Audio-HF-Tx */ &&
                    UlisesToolsVersion.Tools["RadioHF"] == null)
                    // El recurso no se muestra
                    continue;

                ListBox1.Items.Add(((ServiciosCD40.DestinosRadio)nu[i]).IdDestino);
            }

		if (ListBox1.Items.Count > 0)
		{
			ActualizaWebPadre(true);

            if (ListBox1.Items.FindByText(NewItem) != null)
                ListBox1.Items.FindByText(NewItem).Selected = true;
            else
                ListBox1.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < ListBox1.Items.Count ? IndexListBox1 : 0;
    
			MostrarElemento();
		}
		else
		{
			BtModificar.Visible = BtEliminar.Visible = false;
		}
    }



	protected override void AceptarCambios()
	{
		base.AceptarCambios();
	}

    protected override void CancelarCambios()
    {
		EsconderMenu();

        Panel1.Enabled = false;

        BtNuevo.Visible = PermisoSegunPerfil;
        ListBox1.Enabled = true;

        if (ListBox1.Items.Count > 0)
            ListBox1.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < ListBox1.Items.Count ? IndexListBox1 : 0;

		MuestraDatos(DameDatos());
	}

    private void GuardarCambios()
    {
        try
        {
			bool destinoAnadido = false;
            ServiciosCD40.DestinosRadio n = new ServiciosCD40.DestinosRadio();
            n.IdSistema = (string)Session["idsistema"];
            if (TxtIdEnlace.Enabled) //Nuevo Enlace 
                n.IdDestino = TxtIdEnlace.Text;
            else
                n.IdDestino = ListBox1.SelectedValue;

            NewItem = n.IdDestino;

            n.TipoFrec= UInt16.Parse(DListTipo.SelectedValue);
            n.TipoDestino = 0;//externo
            n.ExclusividadTXRX = false; // CheckExclusividad.Checked;
			n.TipoFrec = Convert.ToUInt32(DListTipo.SelectedValue);
            if (TblTunedFreq.Visible)
            {
                TextBox tb = (TextBox)TblTunedFreq.FindControl("TbTunedFrequency");
                if (tb != null)
                {
                    tb.Text = tb.Text.Replace(".", String.Empty);
                    tb.Text = tb.Text.Replace(",", String.Empty);
                    n.Frecuencia = Convert.ToInt32(tb.Text);
                }
            }

            ServiciosCD40.RecursosRadio r = new ServiciosCD40.RecursosRadio();
            r.IdSistema = (string)Session["idsistema"];
            r.TipoDestino = 0;//externo
            r.IdDestino = TxtIdEnlace.Text;

			ServiciosCD40.Tablas[] ltf = new ServiciosCD40.Tablas[ListRecursos.Items.Count];
			for (int i = 0; i < ListRecursos.Items.Count; i++)
			{
				ServiciosCD40.RecursosRadio r1 = new ServiciosCD40.RecursosRadio();
				r1.IdSistema = (string)Session["idsistema"];
				r1.TipoDestino = 0;//externo
				r1.IdDestino = TxtIdEnlace.Text;
				r1.IdRecurso = ListRecursos.Items[i].Text;
				ltf[i] = (ServiciosCD40.Tablas)r1;
			}

			if (TxtIdEnlace.Enabled) //Nuevo Enlace
			{
//				ServicioCD40.AsignaEnlaceARecurso(ltf);
				destinoAnadido = ServicioCD40.AnadeDestinoRadio(n, r, ltf);
				//				if (ServicioCD40.InsertSQL(n) < 0)
				//                    logDebugView.Warn("(DestinosRadio-GuardarCambios): No se han podido insertar los datos (InsertSQL).");
				//                else
				if (destinoAnadido)
				{
					if (ListRecursos.Items.Count > 0)
					{
						Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
						KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
						if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
						{
							SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

							foreach (ListItem idrec in ListRecursos.Items)
							{
								//Obtener el emplazamiento del recurso
								string empl = "";
								ServiciosCD40.RecursosRadio emp = new ServiciosCD40.RecursosRadio();

								emp.IdSistema = (string)Session["IdSistema"];
								emp.IdRecurso = idrec.Text;
								ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(emp);
								empl = ((ServiciosCD40.RecursosRadio)d[0]).IdEmplazamiento;
								sincro.AltaFrecuencia(empl, n.IdDestino, 0, idrec.Text);
							}
						}
					}

					ActualizaWebPadre(true);
				}
			}
			else
			{
                IndexListBox1 = ListBox1.SelectedIndex;
                
                //if (ServicioCD40.UpdateSQL(n) < 0)
				//    logDebugView.Warn("(DestinosRadio-GuardarCambios): No se han podido actualizar los datos (UpdateSQL).");
				destinoAnadido = ServicioCD40.ModificaDestinoRadio(n, r, ltf);
				//				if (ServicioCD40.InsertSQL(n) < 0)
				//                    logDebugView.Warn("(DestinosRadio-GuardarCambios): No se han podido insertar los datos (InsertSQL).");
				//                else
				if (destinoAnadido)
				{
					if (ListRecursos.Items.Count > 0)
					{
						Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
						KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
						if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
						{
							SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
							sincro.BajaFrecuencia(n.IdDestino, false);
							foreach (ListItem idrec in ListRecursos.Items)
							{
								//Obtener el emplazamiento del recurso
								string empl = "";
								ServiciosCD40.RecursosRadio emp = new ServiciosCD40.RecursosRadio();

								emp.IdSistema = (string)Session["IdSistema"];
								emp.IdRecurso = idrec.Text;
								ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(emp);
								empl = ((ServiciosCD40.RecursosRadio)d[0]).IdEmplazamiento;
								sincro.AltaFrecuencia(empl, n.IdDestino, 0, idrec.Text);
							}
						}
					}
				}
			}

			//ServiciosCD40.RecursosRadio r = new ServiciosCD40.RecursosRadio();
			//r.IdSistema = (string)Session["idsistema"];
			//r.TipoDestino = 0;//externo
			//r.IdDestino = TxtIdEnlace.Text;

			//ServicioCD40.LiberaDestinoDeRecurso(r);
        }
        catch (Exception e)
        {
            logDebugView.Error("(DestinosRadio-GuardarCambios):", e);
        }

        EsconderMenu();
        Panel1.Enabled = false;

        ListBox1.Enabled = true;
        BtNuevo.Visible = PermisoSegunPerfil;
        BtEliminar.Visible = false;
        
        MuestraDatos(DameDatos());
    }
    
    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        if (ListRecursos.Items.Count > 0)
            GuardarCambios();
        else
            cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AvisoDestinoSinRecurso"), "aceptparam");
    }

    protected void BtModificar_Click(object sender, EventArgs e)
    {
        IndexListBox1 = ListBox1.SelectedIndex;

        Panel1.Enabled = true;
		//RequiredFieldValidator1.Enabled = RequiredFieldValidator1.Visible = true;
		errores.Visible = true;
        DListTipo.Enabled = true;   // CheckExclusividad.Enabled = true;
		ListBox1.Enabled = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        BtModificar.Visible = false;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        ListRecursosLibres.Visible = true;
        CargarRecursosSinAsignar();
        Label4.Visible = true;
        ListRecursosLibres.Visible = true;
        IButQuitar.Visible = true;
        IButAsignar.Visible = true;
	}

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        EsconderMenu();
        
        if (ListBox1.SelectedValue != "")
        {
            //string texto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarDestino"), ListBox1.SelectedValue);
            Session["elemento"] = ListBox1.SelectedValue;
            EliminarElemento(false);
            //cMsg.confirm(texto, "eliminaelemento");
        }
    }

    private void EliminarElemento(bool forced)
    {
		// Liberar el destino del recurso
		ServiciosCD40.RecursosRadio rD = new ServiciosCD40.RecursosRadio();
		rD.IdSistema = (string)Session["idsistema"];
		rD.IdDestino = ListBox1.SelectedValue;
		rD.TipoDestino = 0;
		//ServicioCD40.LiberaDestinoDeRecurso(rD);

		ServiciosCD40.Destinos n = new ServiciosCD40.Destinos();
        n.IdSistema = (string)Session["idsistema"];
        n.IdDestino = ListBox1.SelectedValue;
		n.TipoDestino = 0;

		//if (ServicioCD40.DeleteSQL(n) > 0)

        if (forced || !DestinoAsignadoATft(ListBox1.SelectedValue))
        {
            if (ServicioCD40.EliminaDestino(n, rD))
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    sincro.BajaFrecuencia(n.IdDestino, true);
                }

                MuestraDatos(DameDatos());

                // Regenerar todas las sectorizaciones excepto la activa porque puede que este destino 
                // estuviera presente en alguna sectorización
                ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, true, CallbackCompletado, null);
            }
        }
        else
        {
            cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "PanelesConDestinoAsignado"), "SoloEliminaDeTFT");
        }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
        
        if (ListRecursosLibres.SelectedIndex >= 0)
            for (int i = 0; i < ListRecursosLibres.Items.Count;i++)
                if (ListRecursosLibres.Items[i].Selected)
                {                    
                    if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                    {//Comprobar que el recurso seleccionado no tiene el mismo emplazamiento que otro recurso ya asignado
                        List<ServiciosCD40.RecursosRadio> emplAsignados = new List<ServiciosCD40.RecursosRadio>();
                        for (int h = 0; h < ListRecursos.Items.Count; h++)
                        {
                            ServiciosCD40.RecursosRadio rec = new ServiciosCD40.RecursosRadio();
                            rec.IdSistema = (string)Session["IdSistema"];
                            rec.IdRecurso = ListRecursos.Items[h].Text;
							ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(rec);
                            rec.IdEmplazamiento = ((ServiciosCD40.RecursosRadio)d[0]).IdEmplazamiento;
                            emplAsignados.Add(rec);

                        }                        
                        //Obtener el emplazamiento del recurso seleccionado
                        string empl = "";
                        ServiciosCD40.RecursosRadio emp = new ServiciosCD40.RecursosRadio();
                        emp.IdSistema = (string)Session["IdSistema"];
                        emp.IdRecurso = ListRecursosLibres.Items[i].Text;
						ServiciosCD40.Tablas[] l = ServicioCD40.ListSelectSQL(emp);
                        empl = ((ServiciosCD40.RecursosRadio)l[0]).IdEmplazamiento;

                        bool asignar=true;
                        foreach (ServiciosCD40.RecursosRadio s in emplAsignados)
                        {
                            if (s.IdEmplazamiento.CompareTo(empl) == 0)
                            {                             
                                asignar = false;
                                cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "MismoEmplazamiento"), emp.IdRecurso, s.IdRecurso));
                                break;
                            }
                        }
                        if (asignar)
                        {
                            ListRecursos.Items.Add(ListRecursosLibres.Items[i]);
                            ListRecursosLibres.Items.Remove(ListRecursosLibres.Items[i]);
                            i--;
                        }
                    }
                    else
                    {
                        ServiciosCD40.HFParams r = new ServiciosCD40.HFParams();

                        r.IdSistema = (string)Session["idsistema"];
                        r.IdRecurso = ListRecursosLibres.Items[i].Text;
                        ServiciosCD40.Tablas[] h = ServicioCD40.ListSelectSQL(r);

                        if (h.Length > 0)
                        {
                            if (Frecuencia == string.Empty)
                                Frecuencia = ((ServiciosCD40.HFParams)h[0]).Frecuencia;
                            else if (Frecuencia != ((ServiciosCD40.HFParams)h[0]).Frecuencia)
                            {
                                LblErrorMismatchFrequency.Visible = true;
                            }
                        }

                        ListRecursos.Items.Add(ListRecursosLibres.Items[i]);
                        ListRecursosLibres.Items.Remove(ListRecursosLibres.Items[i]);
                        i--;
                    }
                }
    }

    protected void IButQuitar_Click(object sender, ImageClickEventArgs e)
    {
        if (ListRecursos.SelectedIndex >= 0)
        {
			LiberaRecursos();
            LblErrorMismatchFrequency.Visible = false;
        }
    }

	private void LiberaRecursos()
	{
		for (int i = 0; i < ListRecursos.Items.Count; i++)
		{
			if (ListRecursos.Items[i].Selected)
			{
				ListRecursosLibres.Items.Add(ListRecursos.Items[i]);
				ListRecursos.Items.Remove(ListRecursos.Items[i]);
				i--;
			}
		}

		if (ListRecursos.Items.Count > 0)
			ListRecursos.SelectedIndex = 0;
	}

	private bool DestinoAsignadoATft(string destino)
	{
		ServiciosCD40.DestinosRadioSector drs = new ServiciosCD40.DestinosRadioSector();
		drs.IdSistema = s.Value;
        drs.IdDestino = destino;

		ServiciosCD40.Tablas[] lista=ServicioCD40.ListSelectSQL(drs);

		if (lista != null && lista.Length > 0)
			return true;

		return false;
	}

	private void EliminaDestinoDeTFT()
	{
		ServiciosCD40.DestinosRadioSector drs = new ServiciosCD40.DestinosRadioSector();
		drs.IdSistema = s.Value;
        drs.IdDestino = ListBox1.SelectedValue;

		// Eliminar de todos los TFT el destino
		ServicioCD40.DeleteSQL(drs);
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
			logDebugView.Error("(DestinosRadio-OnCallBackCompleted): ", soapException);
		}
	}

    protected void DListTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        CargarRecursosSinAsignar();

        TblTunedFreq.Visible = DListTipo.SelectedValue == DESTINORADIO_TIPOFRECUENCIA_HF;
    }


}
