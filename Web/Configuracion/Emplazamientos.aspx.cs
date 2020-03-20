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

public partial class Emplazamientos : PageBaseCD40.PageCD40		//System.Web.UI.Page
{
	static bool PermisoSegunPerfil;
    private Mensajes.msgBox cMsg;
    private static ILog _logDebugView;
	private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();
    
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

			PermisoSegunPerfil = BtNuevo.Visible = perfil != "1";
			//BtEliminar.Visible = ListBox1.Items.Count > 0 && PermisoSegunPerfil;
		}

        if (!IsPostBack)
        {
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            MuestraDatos(DameDatos());
        }
        else
        {
            if (null == Session["idsistema"])
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
                Session["idsistema"] = s.Value;
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
                //if (Request.Form["cancelparam"] == "1")   //El usuario elige no guardar los cambios 
                //{
                //    Request.Form["cancelparam"].Replace("1", "0");

                //    CancelarCambios();
                //}
                //if (Request.Form["aceptparam"] == "1")   //El usuario elige guardar los cambios
                //{
                //    Request.Form["aceptparam"].Replace("1", "0");

                //    GuardarCambios();
                //}
			//}
    }


    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.Emplazamientos t = new ServiciosCD40.Emplazamientos();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;

			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);

            return d;
        }
        catch (Exception e)
        {
            logDebugView.Error("(Emplazamientos-DameDatos):",e);
        }
        return null;
    }

    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
		ListBox1.Items.Clear();

        if (nu!=null)
            for (int i = 0; i < nu.Length;i++)
                ListBox1.Items.Add(((ServiciosCD40.Emplazamientos)nu[i]).IdEmplazamiento);
	
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
            
            MostrarRecursos();
		}
	}

    private void MostrarRecursos()
    {
        try
        {
            BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarEmplazamiento"), ListBox1.SelectedValue);

            Label3.Text = (string)GetLocalResourceObject("RecursosEmplazamiento") + " " + ListBox1.SelectedValue;
            Label3.Visible = true;
            LBoxRecursos.Visible = true;
            ServiciosCD40.RecursosRadio t = new ServiciosCD40.RecursosRadio();
            t.IdSistema = (string)Session["idsistema"];
            t.IdEmplazamiento = ListBox1.SelectedValue;
			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            LBoxRecursos.Items.Clear();
            for (int i = 0; i < d.Length; i++)
                LBoxRecursos.Items.Add(((ServiciosCD40.RecursosRadio)d[i]).IdRecurso);
            if (d.Length > 0)
                BtEliminar.Visible = false;
            else
				BtEliminar.Visible = PermisoSegunPerfil;
        }
        catch (Exception e)
        {
            logDebugView.Error("(Emplazamientos-MostrarRecursos):", e);
        }
    }

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            MostrarRecursos();
        }
    }

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        if (ListBox1.SelectedValue != "" && Session["idsistema"]!=null)
        {
            LBoxRecursos.Visible = false;
            Label3.Visible = false;
            IndexListBox1 = ListBox1.SelectedIndex;
            Session["elemento"] = ListBox1.SelectedValue;
            EliminarElemento();
            //string texto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarEmplazamiento"), ListBox1.SelectedValue);
            //cMsg.confirm(texto, "eliminaelemento");
        }
    }

    private void EliminarElemento()
    {
        try
        {
			ServiciosCD40.Emplazamientos n = new ServiciosCD40.Emplazamientos();
            n.IdSistema = (string)Session["idsistema"];
            n.IdEmplazamiento = (string)Session["elemento"];
			if (ServicioCD40.DeleteSQL(n) < 0)
            {
                logDebugView.Warn("(Emplazamientos-EliminarElemento): no se ha borrado el emplazamiento");
                cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorEliminarEmplazamiento"), n.IdEmplazamiento));
            }
            else
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    //string s = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado")+ "\\n\\n"
                    //        + "El Emplazamiento no ha sido dado de Baja en el CD30.";
                    //cMsg.alert(s);
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    switch (sincro.BajaEmplazamiento(n.IdEmplazamiento))
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
                        
            ListBox1.Items.Clear();
            MuestraDatos(DameDatos());
        }
        catch (Exception e)
        {
            logDebugView.Error("(Emplazamientos-EliminarElemento):", e);
        }
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        string StrSistema = (string)Session["idsistema"];
        //Si se está insertando un nuevo emplazamiento, se comprueba que el identificador sea único (Emplazamientos,zonas)
        if (TextBox1.Enabled && bIdentificadorAsignado(StrSistema, TextBox1.Text))
        {
            //Existe otro zona o emplazamiento con el mismo identificador
            string strMsg = string.Empty;

            if (null != GetGlobalResourceObject("Espaniol", "ErrorGuardarEmplazamiento"))
                strMsg = string.Format(GetGlobalResourceObject("Espaniol", "ErrorGuardarEmplazamiento").ToString(), TextBox1.Text);
            else
                strMsg = string.Format("El identificador del emplazamiento {0} ya existe en el sistema.", TextBox1.Text);

            cMsg.alert(strMsg);
            return;
        }
        else
            GuardarCambios(StrSistema);
    }

    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
    }

    private void GuardarCambios(string strSistema)
    {
        try
        {
            ServiciosCD40.Emplazamientos n = new ServiciosCD40.Emplazamientos();
            n.IdSistema = strSistema;
            n.IdEmplazamiento = TextBox1.Text;

            NewItem = TextBox1.Text;
            
            if (ServicioCD40.InsertSQL(n) < 0)
            {
                logDebugView.Warn("(Emplazamientos-GuardarElemento): no se ha guardado el emplazamiento.");
                cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorGuardarEmplazamiento"), n.IdEmplazamiento));
            }
            else
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    switch (sincro.AltaEmplazamiento(n.IdEmplazamiento))
                    {
                        case 108:
                            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod108"), n.IdEmplazamiento));
                            break;
                        case 109:
                            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod109"), n.IdEmplazamiento));
                            break;
                        default:
                            break;
                    }
                }

				ActualizaWebPadre(true);
			}

            BtAceptar.Visible = false;
            BtCancelar.Visible = false;
            Label2.Visible = false;
            TextBox1.Visible = false;
            ListBox1.Enabled = true;
			BtNuevo.Visible = PermisoSegunPerfil;
			//BtEliminar.Visible = ListBox1.Items.Count > 0 && PermisoSegunPerfil;
            ListBox1.Items.Clear();
            MuestraDatos(DameDatos());
            ValidationSummary1.Visible = false;
            RequiredFieldValidator1.Visible = false;

            Panel1.Enabled = false;

        }
        catch (Exception e)
        {
            logDebugView.Error("(Emplazamientos-GuardarCambios):", e);
        }
    }

	protected override void AceptarCambios()
	{
		base.AceptarCambios();
	}

    protected override void CancelarCambios()
    {
		BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        Label2.Visible = false;
        TextBox1.Visible = false;
        ListBox1.Enabled = true;
		BtNuevo.Visible = PermisoSegunPerfil;
		//BtEliminar.Visible = ListBox1.Items.Count > 0 && PermisoSegunPerfil;
        ValidationSummary1.Visible = false;
        RequiredFieldValidator1.Visible = false;
        Panel1.Enabled = false;

		MuestraDatos(DameDatos());
    }

    protected void BtNuevo_Click(object sender, EventArgs e)
    {
		Label3.Visible = false;
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
        {
            if (ListBox1.Items.Count >= 4)
            {
                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "MaxEmplazamientos"));
                return;
            }
        }

        Panel1.Enabled = true;

        Label3.Visible = false;
        LBoxRecursos.Visible = false;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        TextBox1.Text = "";
        Label2.Visible = true;
        TextBox1.Visible = true;
        ListBox1.Enabled = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        ValidationSummary1.Visible = true;
        RequiredFieldValidator1.Visible = true;
        IndexListBox1 = ListBox1.SelectedIndex;
    }

    //Devuelve true si ya existe una zona o emplazamiento en el sistema con el mismo identificador. En caso contrario, false.
    private bool bIdentificadorAsignado(string strIdSistema, string strIdentificador)
    {
        bool bExiste = false;
        int iExiste = -1;

        // Se comprueba que no existe una zona o emplazamiento con el mismo identificador
        iExiste = ServicioCD40.CheckIdentificadorAsignado("NA", strIdSistema, strIdentificador);

        if (iExiste > 0)
            bExiste = true;
        else if (iExiste < 0)
        {
            System.Text.StringBuilder strMsgError = new System.Text.StringBuilder();
            strMsgError.AppendFormat("(Emplazamientos-bIdentificadorAsignado): el servicio servicioCD40.CheckIdentificadorAsignado('NA', '{0}', '{1}') ha devuelto el codigo {2}", strIdSistema, strIdentificador, iExiste);
            logDebugView.Error(strMsgError.ToString());
            strMsgError.Clear();
        }

        return bExiste;
    }
}
