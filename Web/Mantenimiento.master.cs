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

public partial class Mantenimiento : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (Context.Request.IsAuthenticated)
		{
			string[] strPerfiles = { "Operador", "Técnico 1", "Técnico 2", "Técnico 3" };
			
			// retrieve user's identity from httpcontext user 
			FormsIdentity ident = (FormsIdentity)Context.User.Identity;
			string perfil = ident.Ticket.UserData;
			switch (perfil)
			{
				case "0":
					break;
				case "1":
				case "2":
					BtCluster.Visible = false;
					break;
				case "3":
					break;
				default:
					break;
			}

			LabelPerfil.Text = strPerfiles[Convert.ToInt16(perfil)];
			LabelUsuario.Text = ident.Name;
		}
    }

	protected void LinkButton1_Click(object sender, EventArgs e)
	{
		FormsAuthentication.Initialize();
		HttpContext context1 = HttpContext.Current;
		HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
		cookie1.Path = FormsAuthentication.FormsCookiePath;
		cookie1.Expires = new DateTime(0x7cf, 10, 12);
		cookie1.Secure = FormsAuthentication.RequireSSL;
		context1.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
		context1.Response.Cookies.Add(cookie1);

		Response.Redirect("~/Login.aspx");
	}

    protected void BtInicio_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }
    protected void BtCluster_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Cluster/Default.aspx");
    }
    protected void BtConfiguracion_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Configuracion/Inicio.aspx");
    }
}
