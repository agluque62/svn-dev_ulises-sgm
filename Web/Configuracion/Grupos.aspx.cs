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

public partial class Grupos :	PageBaseCD40.PageCD40	//	System.Web.UI.Page
{
    private Mensajes.msgBox cMsg;
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
	static bool PermisoSegunPerfil;
	static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();

    private AsyncCallback CallbackCompletado;

    protected new void Page_Load(object sender, EventArgs e)
    {
		base.Page_Load(sender, e);

        cMsg = (Mensajes.msgBox)this.Master.FindControl("MsgBox1");

        if (CallbackCompletado == null)
            CallbackCompletado = new AsyncCallback(OnCallBackCompleted);
        
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

		if (!IsPostBack)
		{
			// ServicioCD40.NoTransaction();
			BtNuevo.Visible = PermisoSegunPerfil;
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            IndexListBox1 = -1;
            MuestraDatos(DameDatos());

			ActualizaWebPadre(true);
			CargarInforme();
		}
        //else
        //    if (IsPostBack)
        //    {
        //        if (Request.Form["eliminaelemento"] == "1")   //El usuario elige eliminar el elemento 
        //        {
        //            Request.Form["eliminaelemento"].Replace("1", "0");
        //            EliminarElemento();
        //        }
                //if (Request.Form["cancelparam"] == "1")   //El usuario elige no guardar los cambios 
                //{
                //    Request.Form["cancelparam"].Replace("1", "0");
                //    RequiredFieldNucleo.Visible = false;
                //    CancelarCambios();
                //}
                //if (Request.Form["aceptparam"] == "1")   //El usuario elige guardar los cambios
                //{
                //    Request.Form["aceptparam"].Replace("1", "0");
                //    GuardarCambios();
                //}
			//}
    }

	protected void CargarInforme()
	{
		LBImprimir.Attributes.Remove("onclick");
		string comando = "AbreVentana('../Informes/Report.aspx?Report=Grupos.rpt');return false;";
		LBImprimir.Attributes.Add("onclick", comando);
	}

    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.GruposTelefonia t = new ServiciosCD40.GruposTelefonia();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;
			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            return d;
        }
        catch (Exception e)
        {
            logDebugView.Error("(Grupos-DameDatos)",e);
        }
        return null;
    }

    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
        if (nu!=null)
            for (int i = 0; i < nu.Length;i++)
                ListBox1.Items.Add(((ServiciosCD40.GruposTelefonia)nu[i]).IdGrupo);

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
            
            BtEliminar.Visible = PermisoSegunPerfil;
			MostrarDestinosGrupo();
			GeneraXmlParaInforme();
		}
		else
			BtEliminar.Visible = false;
    }

	private void GeneraXmlParaInforme()
	{
		ServiciosCD40.DestinosRadio t = new ServiciosCD40.DestinosRadio();
		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];

		ServicioCD40.DestinosPorGruposTelefonia(s.Value).WriteXml(Server.MapPath("~/Informes/GruposTelefonia.xml"));
	}

	protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            MostrarDestinosGrupo();
        }
    }

    private void MostrarDestinosGrupo()
    {
        try
        {
            BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarGrupo"), ListBox1.SelectedValue);

            Label3.Text = (string)GetGlobalResourceObject("Espaniol", "DestinosGrupo") + " " + ListBox1.SelectedValue;
            Label3.Visible = true;
            ListBox2.Visible = true;
            ServiciosCD40.DestinosTelefonia t = new ServiciosCD40.DestinosTelefonia();
            t.IdSistema = (string)Session["idsistema"];
            t.IdGrupo = ListBox1.SelectedValue;
			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            ListBox2.Items.Clear();
            for (int i = 0; i < d.Length; i++)
                ListBox2.Items.Add(((ServiciosCD40.DestinosTelefonia)d[i]).IdDestino);
        }
        catch (Exception e)
        {
            logDebugView.Error("(Grupos-MostrarDestinosGrupo)", e);
        }
    }

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        if (ListBox1.SelectedValue != "" && Session["idsistema"]!=null)
        {
            Label3.Visible = false;
            ListBox2.Visible = false;
			//string texto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarGrupo"), ListBox1.SelectedValue);
            IndexListBox1 = ListBox1.SelectedIndex;
            Session["elemento"] = ListBox1.SelectedValue;
            EliminarElemento();

            try
            {
                // Llamada asíncrona para regenerar todas las sectorizaciones.
                Session.Add("Sectorizando", true);
                ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, true, CallbackCompletado, null);
            }
            catch (Exception ex)
            {
                logDebugView.Error("(Grupos-BtEliminar_Click): ", ex);
            }
        }
    }

    private void EliminarElemento()
    {
        try
        {
            ServiciosCD40.GruposTelefonia n = new ServiciosCD40.GruposTelefonia();
            n.IdSistema = (string)Session["idsistema"];
            n.IdGrupo = (string)Session["elemento"];
			if (ServicioCD40.DeleteSQL(n) < 0)
                logDebugView.Warn("(Grupos-EliminarElemento): No se ha podido eliminar el elemento");
            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
        }
        catch (Exception e)
        {
            logDebugView.Error("(Grupos-EliminarElemento): ", e);
        }
        ListBox1.Items.Clear();
        MuestraDatos(DameDatos());
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        if ( Session["idsistema"]!=null)
            GuardarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AceptarCambios"), "aceptparam");
    }

    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        RequiredFieldNucleo.Visible = false; 
        CancelarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");
    }

    private void GuardarCambios()
    {
        try
        {
            ServiciosCD40.GruposTelefonia n = new ServiciosCD40.GruposTelefonia();
            n.IdSistema = (string)Session["idsistema"];
            n.IdGrupo = TextBox1.Text;
			if (ServicioCD40.InsertSQL(n) < 0)
                logDebugView.Warn("(Grupos-EliminarElemento): No se ha podido insertar el elemento.");
        }
        catch (Exception e)
        {
            logDebugView.Error("(Grupos-EliminarElemento):",e);
        }

        try
        {
            // Llamada asíncrona para regenerar todas las sectorizaciones.
            Session.Add("Sectorizando", true);
            ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, true, true, CallbackCompletado, null);
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Grupos.GuardadCambios): ", ex);
        }

        NewItem = TextBox1.Text;

        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        TextBox1.Visible = false;
        ListBox1.Enabled = true;
        BtNuevo.Visible = PermisoSegunPerfil;
		//BtEliminar.Visible = false;
        Label1.Visible = false;
        ListBox1.Items.Clear();
        MuestraDatos(DameDatos());
        RequiredFieldNucleo.Visible = false;
        Panel1.Enabled = false;
    }

    protected override void CancelarCambios()
    {
        Panel1.Enabled = false;
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        TextBox1.Visible = false;
        ListBox1.Enabled = true;
        BtNuevo.Visible = PermisoSegunPerfil;
		BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
        Label1.Visible = false;
    }

    protected void BtNuevo_Click(object sender, EventArgs e)
    {
        Panel1.Enabled = true;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        TextBox1.Text = "";
        TextBox1.Visible = true;
        ListBox1.Enabled = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        RequiredFieldNucleo.Visible = true;
        Label1.Visible = true;
        Label3.Visible = false;
        ListBox2.Visible = false;
        IndexListBox1 = ListBox1.SelectedIndex;
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
            logDebugView.Error("(Grupos-OnCallBackCompleted): ", soapException);
        }
    }

}
