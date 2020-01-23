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

public partial class CD40 : System.Web.UI.MasterPage
{
    static private bool bSinTelefoniaATS = false; //Indica si no se deben mostar las opciones de telefonía ATS (troncales y encaminamientos)
    static private bool bAccesoInfOperadores = false; //Indica si se debe mostrar el informe de operadores

	protected void Page_Load(object sender, EventArgs e)
	{
        if (Session.IsNewSession)
            Response.Redirect("~/Login.aspx", false);
		if (Context.Request.IsAuthenticated)
		{
			// Capturar el idioma para el sitemap
			if (System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "es")
				SiteMapDataSource1.Provider = SiteMap.Providers["Spanish-Sitemap"];
            else if (System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fr")
				SiteMapDataSource1.Provider = SiteMap.Providers["French-Sitemap"];
            else
                SiteMapDataSource1.Provider = SiteMap.Providers["English-Sitemap"];
			
			string[] strPerfiles ={ (string)GetGlobalResourceObject("Espaniol", "Operador"),
                                  (string)GetGlobalResourceObject("Espaniol", "Tecnico1"),
                                  (string)GetGlobalResourceObject("Espaniol", "Tecnico2"),
                                  (string)GetGlobalResourceObject("Espaniol", "Tecnico3")};

			// retrieve user's identity from httpcontext user 
			FormsIdentity ident = (FormsIdentity)Context.User.Identity;
            string strPerfil = ident.Ticket.UserData;
            switch (strPerfil)
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

            LabelPerfil.Text = strPerfiles[Convert.ToInt16(strPerfil)];
			LabelUsuario.Text = ident.Name;

            if (false == IsPostBack)
            {
                if (TreeView1.Visible || DivWizard.Visible)
                {
                    //Se lee del fichero de configuración si hay que eliminar del árbol de menú las opciones
                    //Encaminamientos y Troncales
                    if (Ulises5000Configuration.ToolsUlises5000Section.Instance.Tools["SinTelefoniaATS"] != null)
                    {
                        bSinTelefoniaATS = true;
                    }
                    else
                        bSinTelefoniaATS = false;
                }

                if (DivWizard.Visible)
                {
                    //Si estamos en la pantalla del wizard
                    if (bSinTelefoniaATS)
                    {
                        //Se ocultan en la pantalla del wizard los botones de encaminamientos y troncales
                        BtnTroncales.Visible = false;
                        BtnEncaminamientos.Visible = false;
                    }

                    if (string.Compare(strPerfil, "3") == 0)
                        BtnOperadores.Visible = true;
                    else
                        BtnOperadores.Visible = false;

                    if (null != Session["PasoWizard"])
                    {
                        try
                        {
                            int iPaso = (int)Session["PasoWizard"];

                            //Activamos el botón correspondiente al paso en el que estamos
                            ActivarOpcionMenu(iPaso, bSinTelefoniaATS);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else
                {
                    //Solo el perfil 3 tiene acceso al informe de operadores
                    if (string.Compare(strPerfil, "3") == 0)
                        bAccesoInfOperadores=true;
                    else
                        bAccesoInfOperadores=false;
                }

                //Si la aplicación se configura en modo cluster, la entrada del Web Config [Servidor-2] debe tomar el valor SI
                //Si no está configurada en modo cluster (null == config.AppSettings.Settings["Servidor-2"] || config.AppSettings.Settings["Servidor-2"].Value!= "SI") 
                //se oculta el enlace a la pantalla de mantenimiento del cluster
                //La variable Cnf_ModoCluster, se lee en el módulo Global.asax cuando se accede por primera vez a la aplicación
                if  ((null==Application["Cnf_ModoCluster"] || false == (bool) Application["Cnf_ModoCluster"]) && BtCluster.Visible)
                {
                    BtCluster.Visible = false;
                }
            }
        }
    }

    //Activa el botón correspondiente al paso que se está visualizando en el Wizard
    private void ActivarOpcionMenu(int piPaso, bool pbSinTelefoniaATS)
    {
        int iPaso = piPaso;

        if (!BtnOperadores.Visible)
        {
            iPaso = iPaso + 1;
        }

        if (pbSinTelefoniaATS && iPaso >= 5)
        {
            //Nos saltamos los botones BtnTroncales y Encaminamientos que están en la posición 5 y 6
            iPaso = iPaso + 2;
        }

        switch (iPaso)
        {
            case 0:
                BtnOperadores.CssClass = "textbox";
                break;
            case 1:
                BtnTifx.CssClass = "textbox";
                break;
            case 2:
                BtnEqEx.CssClass = "textbox";
                break;
            case 3:
                BtnTops.CssClass = "textbox";
                break;
            case 4:
                BtnRedes.CssClass = "textbox";
                break;
            case 5:
                BtnTroncales.CssClass = "textbox";
                break;
            case 6:
                BtnEncaminamientos.CssClass = "textbox";
                break;
            case 7:
                BtnRecursosTf.CssClass = "textbox";
                break;
            case 8:
                BtnGrupos.CssClass = "textbox";
                break;
            case 9:
                BtnDestinosTf.CssClass = "textbox";
                break;
            case 10:
                BtnEmplazamientos.CssClass = "textbox";
                break;
            case 11:
                BtnZonas.CssClass = "textbox";
                break;
            case 12:
                BtnTabCalidad.CssClass = "textbox";
                break;
            case 13:
                BtnRecursosRd.CssClass = "textbox";
                break;
            case 14:
                BtnDestinosRadio.CssClass = "textbox";
                break;
            case 15:
                BtnNucleos.CssClass = "textbox";
                break;
            case 16:
                BtnSectores.CssClass = "textbox";
                break;
            case 17:
                BtnAgrupaciones.CssClass = "textbox";
                break;
            case 18:
                BtnSectorizaciones.CssClass = "textbox";
                break;
            default:
                break;
        }
    }

    protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
        //Lee las opciones configuradas en el Web.xx.sitemap
        if (bSinTelefoniaATS)
        {
            // Si la opción SinTelefoniaATS está configurada, se eliminan las opciones encaminamientos --> "~/Configuracion/Encaminamientos.aspx"
            // y troncales -->"~/Configuracion/Troncales.aspx" del arbol de menú
            try
            {
                SiteMapNode mapNode = (SiteMapNode)e.Node.DataItem;

                if (null != mapNode)
                {
                    //Para identificar a estos elementos del menú, en los ficheros Web.xx.sitemap se ha añadido a la definición de ambos sitemapNode la propiedad identificador con el valor correspondiente

                    if (null != mapNode["identificador"])
                    {
                        string strId = mapNode["identificador"];

                        switch (strId)
                        {
                            case "Troncales":
                            case "Encaminamientos":
                                System.Web.UI.WebControls.TreeNode parent = e.Node.Parent;
                                if (parent != null)
                                {
                                    parent.ChildNodes.Remove(e.Node);
                                }
                                break;
                            case "InfOperadores":

                                if (!bAccesoInfOperadores)
                                {
                                    if (e.Node.Parent != null)
                                    {
                                        e.Node.Parent.ChildNodes.Remove(e.Node);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        else if (!bAccesoInfOperadores)
        {
            try
            {
                SiteMapNode mapNode = (SiteMapNode)e.Node.DataItem;

                if (null != mapNode && null!=mapNode["identificador"])
                {
                    string strId = mapNode["identificador"];

                    if (string.Compare(strId,"InfOperadores",true)==0)
                    {
                        System.Web.UI.WebControls.TreeNode parent = e.Node.Parent;
                        if (parent != null)
                        {
                            parent.ChildNodes.Remove(e.Node);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }


	protected void BtInicio_Click(object sender, EventArgs e)
	{
        string url = "~/Default.aspx";
        Response.Redirect(url, false);
	}
	protected void BtCluster_Click(object sender, EventArgs e)
	{
        Response.Redirect("~/Cluster/Default.aspx", false);
	}
	protected void BtMantenimiento_Click(object sender, EventArgs e)
	{
        System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        string urlMantto = config.AppSettings.Settings["UrlMantto"].Value;
        string urlRtn = config.AppSettings.Settings["UrlRetorno"].Value;

        Response.Redirect(urlMantto + "?RetUrl=" + urlRtn + "&user=" + Context.User.Identity.Name);
    }

	private bool GestionarUltimasModificaciones()
	{
		ServiciosCD40.ServiciosCD40 servicio = new ServiciosCD40.ServiciosCD40();
		if (servicio.HayModificacionesPendientes())
		{
			Panel1.Visible = true;

			return false;
		}


		FormsAuthentication.Initialize();
		HttpContext context1 = HttpContext.Current;
		HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
		cookie1.Path = FormsAuthentication.FormsCookiePath;
		cookie1.Expires = new DateTime(0x7cf, 10, 12);
		cookie1.Secure = FormsAuthentication.RequireSSL;
		context1.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
		context1.Response.Cookies.Add(cookie1);

        Response.Redirect("~/Login.aspx", false);
		return true;
	}

	protected void LinkButton1_Click(object sender, EventArgs e)
	{
		GestionarUltimasModificaciones();
	}

    protected void OnBtnCancelarModoContinuar(object sender, EventArgs e)
    {
        Panel1.Visible = false;
    }
	protected void OnBtnAceptarModoContinuar(object sender, EventArgs e)
	{
		if (RBContinuarActiva.Checked)
		{
            Response.Redirect("~/Configuracion/Sectorizaciones.aspx", false);
		}
		else if (RBContinuarModificada.Checked)
		{
			// recupera la última activa (CD40) como configuración de trabajo (CD40_Trans)
			ServiciosCD40.ServiciosCD40 servicio = new ServiciosCD40.ServiciosCD40();
			servicio.Rollback();

			FormsAuthentication.Initialize();
			HttpContext context1 = HttpContext.Current;
			HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
			cookie1.Path = FormsAuthentication.FormsCookiePath;
			cookie1.Expires = new DateTime(0x7cf, 10, 12);
			cookie1.Secure = FormsAuthentication.RequireSSL;
			context1.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
			context1.Response.Cookies.Add(cookie1);

            Response.Redirect("~/Login.aspx", false);
		}
		else
		{	// Se sale de la sesión de configuración pero deja las modificaciones pendientes
			FormsAuthentication.Initialize();
			HttpContext context1 = HttpContext.Current;
			HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
			cookie1.Path = FormsAuthentication.FormsCookiePath;
			cookie1.Expires = new DateTime(0x7cf, 10, 12);
			cookie1.Secure = FormsAuthentication.RequireSSL;
			context1.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
			context1.Response.Cookies.Add(cookie1);

            Response.Redirect("~/Login.aspx", false);
		}

	}

	//protected void BtnConfirmarMessageBox_OnClick(object sender, EventArgs e)
	//{
	//    ((PageBaseCD40.PageCD40)Page).FinalizaTransaccion();
	//    ((PageBaseCD40.PageCD40)Page).ComunicaSectorizacion();
	//}
	//protected void BtnCancelarMessageBox_OnClick(object sender, EventArgs e)
	//{
	//    ((PageBaseCD40.PageCD40)Page).CancelaTransaccion();
	//}

	//protected void BtnContinuarMessageBox_OnClick(object sender, EventArgs e)
	//{
	//    ((PageBaseCD40.PageCD40)Page).ContinuaTransaccion();
	//}
}
public class ReverseSort : IComparer
{
    public int Compare(object x, object y)
    {
        // reverse the arguments
        return Comparer.Default.Compare(y, x);
    }
}

