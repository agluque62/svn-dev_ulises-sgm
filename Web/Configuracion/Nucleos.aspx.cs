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
using SincroCD30;

public partial class Nucleos :	PageBaseCD40.PageCD40		//System.Web.UI.Page
{
	static bool PermisoSegunPerfil = false;
	static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();

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

		if (!IsPostBack)
		{
			// ServicioCD40.NoTransaction();

            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

			MuestraDatos(DameDatos());

            //MVO 2017/08/04: Sólo se permite configurar un Nucleo en el sistema.
            BtNuevo.Visible = PermisoSegunPerfil && ListBox1.Items.Count < 1;

		}
        //else
        //    if (IsPostBack)
        //    {
                //if (Request.Form["eliminaelemento"] == "1")   //El usuario elige eliminar el elemento 
                //{
                //    Request.Form["eliminaelemento"].Replace("1", "0");
                //    EliminarElemento();
                //}
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

    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.Nucleos t = new ServiciosCD40.Nucleos();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;
			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            return d;
        }
        catch (Exception e)
        {
            logDebugView.Error("(Nucleos-DameDatos): ",e);
        }
        return null;
    }

    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
        ListBox1.Items.Clear();

        if (nu!=null)
            for (int i = 0; i < nu.Length;i++)
                ListBox1.Items.Add(((ServiciosCD40.Nucleos)nu[i]).IdNucleo);

        if (ListBox1.Items.Count > 0)
        {
            if (ListBox1.Items.FindByText(NewItem) != null)
                ListBox1.Items.FindByText(NewItem).Selected = true;
            else
                ListBox1.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < ListBox1.Items.Count ? IndexListBox1 : 0;

            MostrarSectores();
        } 

        BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;

		ActualizaWebPadre(ListBox1.Items.Count > 0);
	}

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            MostrarSectores();
        }
    }

    private void MostrarSectores()
    {
        try
        {
            BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarNucleo"), ListBox1.SelectedValue);

            Label3.Text = (string)GetGlobalResourceObject("Espaniol", "SectoresNucleo") + " " + ListBox1.SelectedValue;
            Label3.Visible = true;
            ListBox2.Visible = true;
            ServiciosCD40.Sectores t = new ServiciosCD40.Sectores();
            t.IdSistema = (string)Session["idsistema"];
            t.IdNucleo = ListBox1.SelectedValue;
            t.SectorSimple = true;
			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            ListBox2.Items.Clear();
            for (int i = 0; i < d.Length; i++)
                ListBox2.Items.Add(((ServiciosCD40.Sectores)d[i]).IdSector);
        }
        catch (Exception e)
        {
            logDebugView.Error("(Nucleos-MostrarSectores): ", e);
        }
    }

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        if (ListBox1.SelectedValue != "" && Session["idsistema"] != null)
        {
			if (ListBox2.Items.Count == 0)
			{
				Label3.Visible = false;
				ListBox2.Visible = false;
				Session["elemento"] = ListBox1.SelectedValue;
				
                EliminarElemento();
                //cMsg.confirm(texto, "eliminaelemento");
				/*
				 * if (ListBox1.SelectedIndex >= 0)
				{
					BtEliminar.Enabled = true;
					MostrarSectores();
				}
				 * */
			}
			else
			{
				cMsg.alert((string)GetGlobalResourceObject("Espaniol", "NucleoConSectores"));
			}
		}
    }

    private void EliminarElemento()
    {
        try
        {
            ServiciosCD40.Nucleos n = new ServiciosCD40.Nucleos();
            n.IdSistema = (string)Session["idsistema"];
            n.IdNucleo = (string)Session["elemento"];
			if (ServicioCD40.DeleteSQL(n) < 0)
            {
                logDebugView.Warn("(Nucleos-EliminarElemento): No se ha borrado el elemento");
                cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorEliminarNucleo"), n.IdNucleo));
            }
            else
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    switch (sincro.BajaNucleo(n.IdNucleo))
                    {
                        case 102:
                            string s = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado") + "\\n\\n"
                                + String.Format((string)GetGlobalResourceObject("Espaniol", "Cod102"), n.IdNucleo);
                            cMsg.alert(s);
                            break;
                        case 103:
                            string s1 = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado") + "\\n\\n" 
                                + String.Format((string)GetGlobalResourceObject("Espaniol", "Cod103"), n.IdNucleo);
                            cMsg.alert(s1);
                            break;
                        default:
                            break;
                    }
                }
                else
                    cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
            }
        }
        catch (Exception e)
        {
            logDebugView.Error("(Nucleos-EliminarElemento): ", e);
        }       
        ListBox1.Items.Clear();
        MuestraDatos(DameDatos());

        //MVO 2017/08/04: Sólo se permite configurar un Nucleo en el sistema.
        BtNuevo.Visible = PermisoSegunPerfil && ListBox1.Items.Count < 1;

        //if (ListBox1.SelectedIndex >= 0)
        //{
        //    BtEliminar.Visible = PermisoSegunPerfil;
        //    MostrarSectores();
        //}
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
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
            ServiciosCD40.Nucleos n = new ServiciosCD40.Nucleos();
            n.IdSistema = (string)Session["idsistema"];
            n.IdNucleo = TextBox1.Text;
			if (ServicioCD40.InsertSQL(n) < 0)
            {
                logDebugView.Warn("(Nucleos-GuardarCambios): No se ha podido guardar el nucleo.");
                cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorGuardarNucleo"), n.IdNucleo));
            }
            else
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar!=null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    switch (sincro.AltaNucleo(n.IdNucleo))
                    {
                        case 100:
                            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod100"), n.IdNucleo));
                            break;
                        case 101:
                            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod101"), n.IdNucleo));
                            break;
                        default:
                            break;
                    }
                }

				ActualizaWebPadre(true);
			}
        }
        catch (Exception e)
        {
            logDebugView.Error("(Nucleos-GuardarCambios): ", e);
        }
        NewItem = TextBox1.Text;
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        TextBox1.Visible = false;
        ListBox1.Enabled = true;

        BtEliminar.Visible = false;
        Label1.Visible = false;
        ListBox1.Items.Clear();
        MuestraDatos(DameDatos());

        //MVO 2017/08/04: Sólo se permite configurar un Nucleo en el sistema.
        BtNuevo.Visible = PermisoSegunPerfil && ListBox1.Items.Count < 1;

        RequiredFieldNucleo.Visible = false;

        //if (ListBox1.SelectedIndex >= 0)
        //{
        //    BtEliminar.Visible = PermisoSegunPerfil;
        //    MostrarSectores();
        //}
    }

    private void CancelarCambios()
    {
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        TextBox1.Visible = false;
        ListBox1.Enabled = true;
        //MVO 2017/08/04: Sólo se permite configurar un Nucleo en el sistema.
        BtNuevo.Visible = PermisoSegunPerfil && ListBox1.Items.Count < 1;

		BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
        Label1.Visible = false;
        if (ListBox1.SelectedIndex >= 0)
        {
            MostrarSectores();
        }
    }

    protected void BtNuevo_Click(object sender, EventArgs e)
    {
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
    }
}
