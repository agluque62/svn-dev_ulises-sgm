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

public partial class Troncales : PageBaseCD40.PageCD40	// System.Web.UI.Page
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
    private Mensajes.msgBox cMsg;
    private static ServiciosCD40.Tablas[] datos;
	static bool PermisoSegunPerfil;
	private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();

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
				Response.Redirect("~/Configuracion/Inicio.aspx?Permiso=NO");
				return;
			}

			PermisoSegunPerfil = perfil != "1";
		}

        if (!IsPostBack)
        {
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            IndexListBox1 = -1;
            BtNuevo.Visible = PermisoSegunPerfil;
            MuestraDatos(DameDatos());
            CargarInforme();

            ActualizaWebPadre(true);
        }
        else
        {
            if (datos == null)
                DameDatos();
        }

    }

	protected void CargarInforme()
	{
		LBImprimir.Attributes.Remove("onclick");
		string comando = "AbreVentana('../Informes/Report.aspx?Report=Troncales.rpt');return false;";
		LBImprimir.Attributes.Add("onclick", comando);
	}

    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.Troncales t = new ServiciosCD40.Troncales();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;

			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            datos = d;
            return d;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Troncales-DameDatos): ",ex);
        }
        return null;
    }

    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
		ListBox1.Items.Clear();

        if (nu!=null)
            for (int i = 0; i < nu.Length;i++)
                ListBox1.Items.Add(((ServiciosCD40.Troncales)nu[i]).IdTroncal);
	
		if (ListBox1.Items.Count > 0)
		{
			ActualizaWebPadre(true);

            if (ListBox1.Items.FindByText(NewItem) != null)
            {
                ListBox1.Items.FindByText(NewItem).Selected = true;
                IndexListBox1 = ListBox1.SelectedIndex;
            }
            else
                ListBox1.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < ListBox1.Items.Count ? IndexListBox1 : 0;

            MostrarElemento();
			MostrarRecursos();
			BtEliminar.Visible = BtModificar.Visible = PermisoSegunPerfil;

			GeneraXmlParaInforme();
		}
		else
		{
			BtEliminar.Visible = BtModificar.Visible = false;
		}

        NewItem = string.Empty;
	}

	private void GeneraXmlParaInforme()
	{
		ServiciosCD40.Troncales t = new ServiciosCD40.Troncales();
		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
		t.IdSistema = s.Value;

		DataSet d = ServicioCD40.RecursosPorTroncalParaXML(t.IdSistema);
		d.WriteXml(Server.MapPath("~/Informes/Troncales.xml"));
	}

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            MostrarElemento();
            MostrarRecursos();
        }
    }

    private void MostrarRecursos()
    {
		//int contadoRecursosLadoA = 0;
        Label4.Text = (string)GetGlobalResourceObject("Espaniol", "RecursosTroncal") + " " + ListBox1.SelectedValue;
        Label4.Visible = true;
        ListBox2.Visible = true;
        ServiciosCD40.RecursosTF t = new ServiciosCD40.RecursosTF();
        t.IdSistema = (string)Session["idsistema"];
        t.IdTroncal = ListBox1.SelectedValue;
		ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
        ListBox2.Items.Clear();
		for (int i = 0; i < d.Length; i++)
		{
            ListBox2.Items.Add(((ServiciosCD40.RecursosTF)d[i]).IdRecurso);
		}
    }

    private void MostrarElemento()
    {
        MostrarMenu();
        TxtIdTroncal.ReadOnly = true;
        TxtNumTest.ReadOnly = true;
        
        for (int i = 0; i < datos.Length; i++)
        {
            if (String.Compare((((ServiciosCD40.Troncales)datos[i]).IdTroncal), ListBox1.SelectedValue) == 0)
            {
                BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarTroncal"), ListBox1.SelectedValue);

                TxtIdTroncal.Text = ((ServiciosCD40.Troncales)datos[i]).IdTroncal;
                TxtNumTest.Text = ((ServiciosCD40.Troncales)datos[i]).NumTest;
            }
        }
    }

    private void MostrarMenu()
    {
        TxtIdTroncal.Text = "";        
        TxtIdTroncal.Visible = true;
        TxtNumTest.Text = "";
        TxtNumTest.Visible = true;
        Label1.Visible = true;
        Label2.Visible = true;
//		BtModificar.Visible = PermisoSegunPerfil;
    }

    private void EsconderMenu()
    {
        TxtIdTroncal.Text = "";
        TxtIdTroncal.Visible = false;
        Label1.Visible = false;
        Label2.Visible = false;
        TxtNumTest.Visible = false;
//        BtModificar.Visible = false;
    }

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        if (ListBox1.SelectedValue != "")
        {
            Label4.Visible = false;
            ListBox2.Visible = false;
            EsconderMenu();
            Session["elemento"] = ListBox1.SelectedValue;
            IndexListBox1 = ListBox1.SelectedIndex;
            EliminarElemento();
            //string texto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarTroncal"), ListBox1.SelectedValue);
            //cMsg.confirm(texto, "eliminaelemento");
        }
    }

    private void EliminarElemento()
    {
        try
        {
			ServiciosCD40.Troncales n = new ServiciosCD40.Troncales();
            n.IdSistema = (string)Session["idsistema"];
            n.IdTroncal = (string)Session["elemento"];
			if (ServicioCD40.DeleteSQL(n) < 0) 
				logDebugView.Warn("(Troncales-EliminarElemento): No se ha podido eliminar el troncal.");
            else
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    switch (sincro.BajaTroncal(n.IdTroncal))
                    {
                        case 127:
                            string msg = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado") + "\\n\\n" +
                                (string)GetGlobalResourceObject("Espaniol", "Cod127");
                            cMsg.alert(msg);
                            break;
                        case 128:
                            string msg1 = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado") + "\\n\\n" +
                                (string)GetGlobalResourceObject("Espaniol", "Cod128");
                            cMsg.alert(msg1);
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
            logDebugView.Error("(Troncales-EliminarElemento): ", ex);
        }
        
		MuestraDatos(DameDatos());
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        GuardarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AceptarCambios"), "aceptparam");
    }

    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");
    }

    private void GuardarCambios()
    {
        try
        {
            ServiciosCD40.Troncales n = new ServiciosCD40.Troncales();
            n.IdSistema = (string)Session["idsistema"];
            n.IdTroncal = TxtIdTroncal.Text;
            n.NumTest = TxtNumTest.Text;

            NewItem = TxtIdTroncal.Text;

            if (!TxtIdTroncal.ReadOnly)
			{
				if (ServicioCD40.InsertSQL(n) < 0)
                    logDebugView.Warn("(Troncales-GuardarCambios): No se ha podido guardar el troncal.");
                else
                {
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                    KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                    if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                    {
                        SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                        switch (sincro.AltaTroncal(n.IdTroncal,n.NumTest))
                        {
                            case 129:
                                cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod129"), n.IdTroncal));
                                break;
                            case 130:
                                cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod130"), n.IdTroncal));
                                break;
                            case 131:
                                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod131"));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                IndexListBox1 = ListBox1.SelectedIndex;
                
                ServicioCD40.UpdateSQL(n);
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    switch (sincro.ModificacionTroncal(n.IdTroncal, n.NumTest))
                    {
                        case 127:
                            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod127"), n.IdTroncal));
                            break;
                        case 128:
                            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod128"), n.IdTroncal));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Troncales-GuardarCambios): ", ex);
        }
		BtAceptar.Visible = false;
		BtCancelar.Visible = false;
		BtNuevo.Visible = PermisoSegunPerfil;
		BtModificar.Visible = ListBox1.Items.Count > 0 && PermisoSegunPerfil;
		BtEliminar.Visible = ListBox1.Items.Count > 0 && PermisoSegunPerfil;
        Panel1.Enabled = false;

		TxtIdTroncal.Visible = false;
        ListBox1.Enabled = true;
        Label1.Visible = false;
        Label2.Visible = false;
        TxtNumTest.Visible = false;
        RequiredFieldIdentificador.Visible = false;

		ListBox1.Enabled = true;
		
		ListBox1.Items.Clear();
		MuestraDatos(DameDatos());
    }

	protected override void AceptarCambios()
	{
		base.AceptarCambios();
	}

    protected override void CancelarCambios()
    {
        Panel1.Enabled = false;
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
		BtNuevo.Visible = PermisoSegunPerfil;
		BtModificar.Visible = ListBox1.Items.Count > 0 && PermisoSegunPerfil;
		BtEliminar.Visible = ListBox1.Items.Count > 0 && PermisoSegunPerfil;
		TxtIdTroncal.Visible = false;
        ListBox1.Enabled = true;
        Label1.Visible = false;
        Label2.Visible = false;
        TxtNumTest.Visible = false;
        RequiredFieldIdentificador.Visible = false;
        if (ListBox1.Items.Count > 0)
            ListBox1.SelectedIndex = IndexListBox1 != -1 ? IndexListBox1 : 0;


		MuestraDatos(DameDatos());
    }

    protected void BtNuevo_Click(object sender, EventArgs e)
    {
        RequiredFieldIdentificador.Visible = true;

        Panel1.Enabled = true;
        BtModificar.Visible = false;
        BtAceptar.Visible = true;
		BtNuevo.Visible = false;
		BtCancelar.Visible = true;
		BtEliminar.Visible = false;
        TxtIdTroncal.ReadOnly = false;
        TxtNumTest.ReadOnly = false;
        TxtNumTest.Text = "";
        TxtIdTroncal.Text = "";
        TxtIdTroncal.Visible = true;
        ListBox1.Enabled = false;
        Label1.Visible = true;
        Label2.Visible = true;
        TxtNumTest.Visible = true;
        Label4.Visible = false;
        ListBox2.Visible = false;
        IndexListBox1 = ListBox1.SelectedIndex;
	}

    protected void BtModificar_Click(object sender, EventArgs e)
    {
        IndexListBox1 = ListBox1.SelectedIndex;

        RequiredFieldIdentificador.Visible = true;

        Panel1.Enabled = true;
        BtModificar.Visible = false;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
		BtNuevo.Visible = false;
		BtEliminar.Visible = false;
		TxtIdTroncal.ReadOnly = true;
        TxtNumTest.ReadOnly = false;
		Label4.Visible = false;
		ListBox2.Visible = false;
	}
}
