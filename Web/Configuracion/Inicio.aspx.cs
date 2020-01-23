using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ServiciosCD40;


public partial class Inicio :	PageBaseCD40.PageCD40	//	 System.Web.UI.Page 
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        //Si no está autentificado se redirige a la pagina de login
        if (!Context.Request.IsAuthenticated)
        {
            Response.Redirect("~/Login.aspx", false);
        }
	}
}
