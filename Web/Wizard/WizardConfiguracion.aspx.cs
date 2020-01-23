using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Wizard_WizardConfiguracion : PageBaseCD40.PageCD40
{
    //Tabla con la lista de URL en la que están accesibles las páginas de cada paso del Wizard.
    //Al invocar a cada página desde el Wizard, es necesario pasarle el parámetro iframe=true, para que en la clase padre PageBaseCD40.PageCD40
    //se pueda identificar si la pantalla ha sido invocado desde el Wizard y así se pueda visualizar correctamente la cabecera (ocultar DivArbol y DivCabecera)
    // y  DivWizard (lista de botones de cada paso)

    static private string[] TablaUrl = { "../Configuracion/Operadores.aspx?iframe=true", "../Configuracion/Tifx.aspx?iframe=true" ,
									   "../Configuracion/EquiposExternos.aspx?iframe=true","../Configuracion/TOP.aspx?iframe=true","../Configuracion/Redes.aspx?iframe=true",
									   "../Configuracion/Troncales.aspx?iframe=true","../Configuracion/Encaminamientos.aspx?iframe=true",
									   "../Configuracion/RecursosTelefonia.aspx?iframe=true","../Configuracion/Grupos.aspx?iframe=true",
									   "../Configuracion/DestinosTelefonia.aspx?iframe=true","../Configuracion/Emplazamientos.aspx?iframe=true",
                                       "../Configuracion/Zonas.aspx?iframe=true","../Configuracion/TablasCalidad.aspx?iframe=true",
                                       "../Configuracion/RecursosRadio.aspx?iframe=true",
									   "../Configuracion/GrupoFD.aspx?iframe=true","../Configuracion/Nucleos.aspx?iframe=true","../Configuracion/Sector.aspx?iframe=true",
									   "../Configuracion/Agrupacion.aspx?iframe=true","../Configuracion/Sectorizaciones.aspx?iframe=true"};


    private static int NUM_BOTONES_WIZARD = 19;     //Número de pasos que tiene el wizard configurado
    private const int  NUM_MAX_BOTONES_WIZARD = 19; //Número máximo de pasos que tiene el wizard, que se corresponde con el número de ImagenesX X=1..19

    static private string[] TablaUrlConf = null;   //Almacena la lista de URL a visualizar en el Wizard configurado

    private const string URL_IMAGEN_ON = "~/Images/cab{0}on.png";  //Formato de la imagen de cada paso activo
    private const string URL_IMAGEN_OFF = "~/Images/cab{0}.png";   //Formato de la imagen de cada paso desactivado
    private const string BOTON_IMAGEN = "Image{0}";     //Formato del nombre del control de imagen definido en el fichero .aspx: Image1, Image2,...,Imagen19


	protected void Page_Load(object sender, EventArgs e)
	{
        //if (Context.Request.IsAuthenticated)
        //{
        //    if (((FormsIdentity)Context.User.Identity).Ticket.Name == "*CD40*")
        //    {
        //        Session.Add("PasoWizard", 0);
        //    }
        //    else
        //    {
        //        Session.Add("PasoWizard", 1);
        //    }
        //}

        bool bSinTelefoniaATS = false;

        if (Context.Request.IsAuthenticated)
        {
            string[] strPerfiles = { "Operador", "Técnico 1", "Técnico 2", "Técnico 3" };

            // retrieve user's identity from httpcontext user 
            FormsIdentity ident = (FormsIdentity)Context.User.Identity;
            string perfil = ident.Ticket.UserData;
            LabelPerfil.Text = strPerfiles[Convert.ToInt16(perfil)];
            LabelUsuario.Text = ident.Name;


            if (Session.IsNewSession)
            {
                //Si está autentificado y se ha creado una nueva sesión en el servidor (porque se ha conmutado), se redirecciona a la pagina de login

                string cookieHeader = Request.Headers["Cookie"];
                if ((null != cookieHeader) && (cookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                {
                    LinkLogOut_Click(null, null);
                    
                    return;
                }
            }


            if (false == IsPostBack)
            {
                //Si es la primera vez que se carga la página
                Session.Add("PasoWizard", 0);
                Session.Add("Wizard", true);

                LabelPerfil.Text = strPerfiles[Convert.ToInt16(perfil)];
                LabelUsuario.Text = ident.Name;
            }

            if ((null == TablaUrlConf) || false == IsPostBack)
            {
                bSinTelefoniaATS = Ulises5000Configuration.ToolsUlises5000Section.Instance.Tools["SinTelefoniaATS"] != null;

                //Se comprueba si por configuración hay que eliminar las opciones de telfonía ATS, es decir, las opciones Encaminamientos y Troncales.
                if (bSinTelefoniaATS || (string.Compare(perfil, "3") != 0))
                {
                    string strAux=string.Empty;
                    List<string> objLista = new List<string>();

                    //Si está definida, se eliminan las opciones "../Configuracion/Troncales.aspx","../Configuracion/Encaminamientos.aspx" del Wizard
                    // Y se reconstruye la tabla con las URL 
                    for (int i = 0; i < TablaUrl.Length; i++)
                    {
                        strAux = TablaUrl[i];

                        if (bSinTelefoniaATS && (strAux.Contains("Troncales.aspx") || strAux.Contains("Encaminamientos.aspx")))
                        {
                            continue;
                        }
                        else if (string.Compare(perfil, "3") != 0 && strAux.Contains("Operadores.aspx"))
                        {
                            continue;
                        }
                        else
                        {
                            objLista.Add(strAux);
                        }
                    }

                    TablaUrlConf = objLista.ToArray();
                    
                    NUM_BOTONES_WIZARD = TablaUrlConf.Length;


                    if ((null != TablaUrlConf) && (null != TablaUrlConf[0]))
                        IFrameWeb.Attributes["src"] = TablaUrlConf[0];
                    
                    objLista.Clear();
                }
                else
                {
                    TablaUrlConf = TablaUrl;
                    NUM_BOTONES_WIZARD = TablaUrlConf.Length;
                }

                //Se cargan los botones que muestran los pasos del Wizard con la imagen inicial
                CargaBotonesPagina();
            }
        }
        else
        {
            //Se redirige a la página de login
            Response.Redirect("~/Login.aspx", false);
        }
    }

    //public string GetWindowName()
    //{
    //    if (Session["WindowName"] == null)
    //        return string.Empty;

    //    return Session["WindowName"].ToString();
    //}

    private void CargaBotonesPagina()
    {
        int i = 0;
        string strNombreboton=string.Empty;
        Image objImagen=null;

        //Carga los botones de la página que están configurados.
        //Por defecto, todos los botones son visibles.

        i = NUM_BOTONES_WIZARD;

        while (i < NUM_MAX_BOTONES_WIZARD)
        {
            //Se hace no visible el resto de botones no configurados
            strNombreboton = string.Format(BOTON_IMAGEN, i + 1);
            objImagen = (Image)Page.FindControl(strNombreboton);
            if (objImagen != null)
                objImagen.Visible = false;
            
            i++;
        }
 
    }

    protected void LBFinWizard_OnCLick(object sender, EventArgs e)
    {
        Session.Remove("Wizard");
        Session.Remove("PasoWizard");

        Response.Redirect("../Default.aspx");
    }

    protected void LinkLogOut_Click(object sender, EventArgs e)
    {
        //Cierra la sesión y dejan las modificaciones pendientes 
        Session.Remove("Wizard");
        Session.Remove("PasoWizard");

        FormsAuthentication.Initialize();
        HttpContext context1 = HttpContext.Current;
        HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
        cookie1.Path = FormsAuthentication.FormsCookiePath;
        cookie1.Expires = new DateTime(0x7cf, 10, 12);
        cookie1.Secure = FormsAuthentication.RequireSSL;
        context1.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
        context1.Response.Cookies.Add(cookie1);

        Response.Redirect("../Login.aspx");
    }

    protected void BtnAnterior_Click(object sender, EventArgs e)
    {
        int iPasoWizard=0;
        Image objImagen = null;

        if (null != Session["PasoWizard"])
        {
            //Se obtiene el paso actual del Wizard
            iPasoWizard = (int)Session["PasoWizard"];
        }

        if (iPasoWizard >= 1)
        {
            string strNombreboton = string.Empty;
            string strImagenBoton = string.Empty;

            iPasoWizard = iPasoWizard - 1;

            //Se actualiza la imagen de todos los botones 
            //para que su estado sea consistente
            for (int i = 0; i < NUM_BOTONES_WIZARD; i++)
            {
                strNombreboton = string.Format(BOTON_IMAGEN, i + 1);

                if (i != iPasoWizard)
                {
                    //Se cambia la imagen del botón como desactivado
                    strImagenBoton = string.Format(URL_IMAGEN_OFF, i + 1);
                }
                else
                {
                    //Se cambia la imagen del nuevo paso a activa
                    strImagenBoton = string.Format(URL_IMAGEN_ON, i + 1);
                }

                objImagen = (Image)Page.FindControl(strNombreboton);
                if (objImagen != null)
                {
                    objImagen.ImageUrl = strImagenBoton;
                }
            }

            if (iPasoWizard > 0)
                BtnAnterior.Enabled = true;
            else
                BtnAnterior.Enabled = false;

            if (iPasoWizard < NUM_BOTONES_WIZARD)
                BtnSiguiente.Enabled = true;
            else
                BtnSiguiente.Enabled = false;

            if ((null != TablaUrlConf) && (null != TablaUrlConf[iPasoWizard]))
                IFrameWeb.Attributes["src"] = TablaUrlConf[iPasoWizard];

            //Una vez que se ha actualizado la pantalla, se actualiza la variable de sesión
            Session["PasoWizard"] = iPasoWizard;
        }

    }


    protected void BtnSiguiente_Click(object sender, EventArgs e)
    {
        int iPasoWizard = 0;
        Image objImagen = null;

        if (null != Session["PasoWizard"])
        {
            //Se obtiene el paso actual del Wizard
            iPasoWizard = (int)Session["PasoWizard"];
        }

        //Si no estoy en el último paso
        if (iPasoWizard < (NUM_BOTONES_WIZARD-1))
        {

            //Pasamos al siguiente paso
            iPasoWizard = iPasoWizard + 1;

            string strNombreboton = string.Empty;
            string strImagenBoton = string.Empty;

            //Se actualiza la imagen de todos los botones 
            //para que su estado sea consistente
            for (int i = 0; i < NUM_BOTONES_WIZARD; i++)
            {
                strNombreboton = string.Format(BOTON_IMAGEN, i + 1);

                if (i != iPasoWizard)
                {
                    //Se cambia la imagen del botón como desactivado
                    strImagenBoton = string.Format(URL_IMAGEN_OFF, i + 1);
                }
                else
                {
                    //Se cambia la imagen del nuevo paso a activa
                    strImagenBoton = string.Format(URL_IMAGEN_ON, i + 1);
                }

                objImagen = (Image)Page.FindControl(strNombreboton);
                if (objImagen != null)
                {
                    objImagen.ImageUrl = strImagenBoton;
                }
            }

            if (iPasoWizard > 0)
                BtnAnterior.Enabled = true;
            else
                BtnAnterior.Enabled = false;

            if (iPasoWizard < (NUM_BOTONES_WIZARD-1))
                BtnSiguiente.Enabled = true;
            else
                BtnSiguiente.Enabled = false;
            
            if ((null != TablaUrlConf) && (null != TablaUrlConf[iPasoWizard]))
                IFrameWeb.Attributes["src"] = TablaUrlConf[iPasoWizard];

            //Una vez que se ha actualizado la pantalla, se actualiza la variable de sesión
            Session["PasoWizard"] = iPasoWizard;
        }
    }

}
