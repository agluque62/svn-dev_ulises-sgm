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

public partial class LoginCD40 : System.Web.UI.Page
{
	static int Perfil = 0;
	private static ServiciosCD40.ServiciosCD40 ServiceServiciosCD40 = new ServiciosCD40.ServiciosCD40();

	protected void Page_Load(object sender, EventArgs e)
	{
        if (!Page.IsPostBack)
        {
            Session.Abandon();
        } 

        SetFocus(Login1);
        Panel3.Visible = Request.Params["Logout"] == "SI";
		Login1.Visible = !Panel3.Visible;
	}

    //public string GetWindowName()
    //{
    //    if (Session["WindowName"] == null)
    //        Session["WindowName"] = Guid.NewGuid().ToString().Replace("-", "");

    //    return Session["WindowName"].ToString();
    //}

	private bool UsuarioValido(string id, string pwd, out int perfil)
	{
		// Comprobación puerta atrás.
		if (id == "*CD40*" && pwd == "*NUCLEOCC*")
		{
			perfil = 3;
			return true;
		}
		else
		{
			Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
			KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];

			perfil = 0;

			ServiciosCD40.Operadores operador = new ServiciosCD40.Operadores();
			operador.IdSistema = s.Value;
			operador.IdOperador = Login1.UserName;
			operador.Clave = Login1.Password;

			ServiciosCD40.Tablas[] validado = ServiceServiciosCD40.ListSelectSQL(operador);

			if (validado.Length > 0)
				perfil = (int)((ServiciosCD40.Operadores)validado[0]).NivelAcceso;

			return validado.Length > 0;
		}
	}

	protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
	{
		int perfil=0;
		if (UsuarioValido(Login1.UserName, Login1.Password, out perfil))
		{
			Perfil = perfil;
            UsuarioAutenticado();
            /*
			if (Perfil==0 || GestionarUltimasModificaciones())
			// Los usuarios con Perfil CONTROLADOR (0) no necesitan comparar la base de datos. Trabajan con la última activa
			{
				UsuarioAutenticado();
			}
             */
		}
		else if (perfil < 0)	// Aplicación bloqueada por estar otro usuario en ella
		{
			Response.Redirect("~/BloqueoAplicacion.aspx");
		}
	}

	private void UsuarioAutenticado()
	{
		string userData = Perfil.ToString();
		FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
																		Login1.UserName,
																		System.DateTime.Now,
																		System.DateTime.Now.AddMinutes(Session.Timeout),
																		false,
																		userData,
																		FormsAuthentication.FormsCookiePath);
		// Encrypt the ticket.
		string encTicket = FormsAuthentication.Encrypt(ticket);
		// Create the cookie.
		Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

		// Redirect back to original URL.
		Response.Redirect(FormsAuthentication.GetRedirectUrl(Login1.UserName, false));
	}

	private bool GestionarUltimasModificaciones()
	{
		if (ServiceServiciosCD40.HayModificacionesPendientes())
		{
			Login1.Visible = false;
			Panel1.Visible = true;

			return false;
		}

		return true;
	}
	
	protected void OnBtnAceptarModoContinuar(object sender, EventArgs e)
	{
        //if (RBContinuarActiva.Checked)
        //{
        //    // Recupera la última configuración activa como configuración de trabajo
        //    ServiceServiciosCD40.Rollback();
        //}

		UsuarioAutenticado();
	}

	protected void LinkButton1_Click(object sender, EventArgs e)
	{
        Response.Redirect("~/Login.aspx", false);
	}
}
