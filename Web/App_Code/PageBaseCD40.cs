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

/// <summary>
/// Summary description for PageBaseCD40
/// </summary>
namespace PageBaseCD40
{
	public class PageCD40 : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
	{
		string _Command;
		protected static bool Sectorizando = false;
		protected static bool RecargarDatosEnPagina = false;
        protected static int IndexListBox1 = -1;

		protected Mensajes.msgBox cMsg;
        private static ServiciosCD40.ServiciosCD40 ServicioParaTransaccion = new ServiciosCD40.ServiciosCD40();
		protected static int EstadoEnlaceSacta = 0;
		private static string UltimaVersionSectorizacion = string.Empty;
        protected static string NewItem = string.Empty;

        protected static bool bSactaActivoEnCnf; //Indica si por configuración, se ha activado la funcionalidad SACTA en la Web de Configuración
        protected static bool bVisualizandoActiva; //Almacena si el usuario está visualizando la sectorización activa

		//private static bool ContinuarConLaTransaccion = false;

		private AsyncCallback CallbackCompletado;


		//static int TiempoPendienteTransaccion = 0;

		public PageCD40()
		{
		}

        //public string GetWindowName()
        //{
        //    if ((bool)Session["Wizard"] == true)
        //        return "Wizard";

        //    if (Session["WindowName"] == null)
        //        return string.Empty;

        //    return Session["WindowName"].ToString();
        //}

        //public string SetWindowName()
        //{
        //    if (Session["WindowName"] == null)
        //        Session["WindowName"] = Guid.NewGuid().ToString().Replace("-", "");

        //    return Session["WindowName"].ToString();
        //}

		protected override void OnInit(System.EventArgs e)
		{
			BuildPage(this.Form);
			//BuildPage(GenerateHtmlForm());
			base.OnInit(e);
		}

		protected void ActualizaWebPadre(bool habilita)
		{
			string webPadreScript = "<script language='JavaScript'>" +
				"HabilitaBotonSiguiente(" + (habilita ? "false" : "true") + ")</script>";

			if (!ClientScript.IsClientScriptBlockRegistered("WebPadreScript"))
				ClientScript.RegisterStartupScript(this.GetType(), "WebPadreScript", webPadreScript, false);
		}

		protected void BuildPage(HtmlForm form)
		{
			////////////////////////////////////////////////////////

			// Build the page and include the generated form

		
			this.Controls.AddAt(0, new LiteralControl(@"
<script id='Base' type='text/javascript'>
		
	function HabilitaBotonSiguiente(argumento) {
		var btnSiguiente = parent.document.getElementById('BtnSiguiente');
		if (btnSiguiente) {
			btnSiguiente.disabled=argumento;
		}
	}

</script>
"));
			

	
    //setInterval('OnTimerTransaction()', 5000);

	//function OnTimerTransaction()
	//{
	//    CallServerTransaction('State', '');
	//}
        
	//function ClientCallbackError(result, context)
	//{
	//    alert(result);
	//}

	//function GetTransactionStatus(arg)
	//{
	//    var strEstado=arg.split(';');
	//    if (strEstado[1]=='True')
	//        {
	//        if (confirm(strEstado[2], 'fintransaccion'))
	//            __doPostBack('confirma');
	//        else
	//            __doPostBack('cancela');
	//        }
	//}
			
			//this.Controls.Add(form);

//            this.Controls.Add(new LiteralControl(@"
//                </body>
//            </html>
//        "));
		}

		private HtmlForm GenerateHtmlForm()
		{
			HtmlForm form = new HtmlForm();
			form.ID = "FormBase"; form.Name = "FormBase";
			AddControlsFromDerivedPage(form);
			return this.Form;
		}

		private void AddControlsFromDerivedPage(HtmlForm form)
		{
			int count = this.Controls.Count;
			for (int i = 0; i < count; ++i)
			{
				System.Web.UI.Control ctrl = this.Controls[i];
				form.Controls.Add(ctrl);
				this.Controls.Remove(ctrl);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			cMsg = (Mensajes.msgBox)this.Master.FindControl("MsgBox1");
			if (!ClientScript.IsClientScriptBlockRegistered("TransaccionEnCurso"))
			{
				string cbReference = ClientScript.GetCallbackEventReference(this, "arg", "GetTransactionStatus", "context", "ClientCallbackError", true);
				string callbackScript = "function CallServerTransaction(arg, context) {" + cbReference + "; }";
				ClientScript.RegisterStartupScript(this.GetType(), "TransaccionEnCurso", callbackScript, true);
				// RegisterClientScriptBlock(this.GetType(), "TransaccionEnCurso", callbackScript, true);
				//this.RegisterClientScriptBlock("TransaccionEnCurso", callbackScript);
			}

			if (CallbackCompletado == null)
				CallbackCompletado = new AsyncCallback(OnCallBackCompleted);


            if (Context.Session != null)
            {
                if (Session.IsNewSession)
                {
                    //Si se ha creado una nueva sesión, cuando la sesión ya estaba iniciado porque se ha conmutado de servidor
                    string cookieHeader = Request.Headers["Cookie"];
                    if ((null != cookieHeader) && (cookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                    {

                        Session.Abandon();

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);

                        /*
                        string striframe = Request.QueryString["iframe"];


                        if (!string.IsNullOrEmpty(striframe) && striframe.Equals("true"))
                        {
                            //Si estamos dentro del frame del wizard
                            //Se abandona la sesión y se va a la página de login
                            Session.Abandon();

                            ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);

                        }
                        else
                        {
                            //Si la página está dentro de una pantalla de la Web de configuración
                            //Se va a la página de login
                            //Response.Redirect("~/Login.aspx", true);
                            Session.Abandon();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);
                        }
                         
                         */ 

                        return;

                    }
                }
            }


			if (!IsPostBack)
			{
                //Se lee el parámetro iframe para conocer si la pantalla se está invocando desde el Wizard (iframe=true) o desde las pantallas de configuración 
                // para poder activar la cabecera y el árbol con la lista de opciones correspondiente
                // Wizard --> DivWizard (lista de botones de cada paso)
                // Configuración --> DivArbol (árbol del menú), DivCabecera --> cabecera de la pantalla.

                string striframe = Request.QueryString["iframe"];
                if (!string.IsNullOrEmpty(striframe) && striframe.Equals("true"))
                {
                    // Se está invocando desde el Wizard
                    //Se ocultan la cabecera y el árbol de la pagina de configuración
                    Control division = this.Master.FindControl("DivArbol");
                    if (division != null && division.Visible)
                        division.Visible = false;
                    division = this.Master.FindControl("DivCabecera");
                    if (division != null && division.Visible)
                        division.Visible = false;

                    //Se hace visible el arbol de menú del Wizard. El div de la cabecera del Wizard no es necesario
                    //porque no está definido en la pagina base CD40.master, sino está en la misma página del wizard
                    division = this.Master.FindControl("DivWizard");
                    if (division != null && !division.Visible)
                        division.Visible = true;
                }
                else
                {
                    // Se está invocando desde la pagina de configuracion
                    //Se oculta el árbol del Wizard
                    Control division = this.Master.FindControl("DivWizard");
                    if (division != null && division.Visible)
                        division.Visible = false;

                    //Se hace visible la cabecera y el arbol de menú de configuración
                    division = this.Master.FindControl("DivArbol");
                    if (division != null && !division.Visible)
                        division.Visible = true;
                    division = this.Master.FindControl("DivCabecera");
                    if (division != null && !division.Visible)
                        division.Visible = true;

                }

                bSactaActivoEnCnf = bSistemaConSactaHabilitadoPorCnf();
                bVisualizandoActiva = false;
			}
		}

        private void ShowMessageBox()
		{
			AjaxControlToolkit.ModalPopupExtender c = (AjaxControlToolkit.ModalPopupExtender)Master.FindControl("ModalPopupExtenderMessageBox");
			if (c != null)
				c.Show();
		}

		protected virtual void CancelarCambios() { }

		protected virtual void AceptarCambios() 
		{
			//ComunicaSectorizacion();
		}
        /*
		public void ComunicaSectorizacion()
		{
			ServiciosCD40.Sectorizaciones s = new ServiciosCD40.Sectorizaciones();

			s.IdSistema = (string)Session["idsistema"];
			s.Activa = true;

			ServiciosCD40.Tablas[] listaSectorizaciones = ServicioParaTransaccion.ListSelectSQL(s);
			if (listaSectorizaciones.Length > 0)
			{
				s.IdSectorizacion = ((ServiciosCD40.Sectorizaciones)listaSectorizaciones[0]).IdSectorizacion;
				DateTime fechaActiva = new DateTime();
				fechaActiva = DateTime.Now;
				ServicioParaTransaccion.ComunicaSectorizacionActiva(s.IdSistema, s.IdSectorizacion, ref fechaActiva);
			}
		}
        */

        private bool bSistemaConSactaHabilitadoPorCnf()
        {
            const string CONF_KEY_CON_SACTA = "SistemaConSACTA";
            bool bConSacta = false;

            //Se obtiene el parametro que indica si se debe o no visualizar la posición Sacta
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement objConf = null;

            //Se obtiene el parametro que indica si se debe o no visualizar la posición Sacta
            objConf = config.AppSettings.Settings[CONF_KEY_CON_SACTA];

            if ((objConf != null) && (!string.IsNullOrEmpty(objConf.Value) && string.Compare(objConf.Value, "SI", true) == 0))
            {
                //El sistema está configurado con el sistema SACTA
                bConSacta = true;
            }

            return bConSacta;
        }

		#region ICallbackEventHandler Members
		// Respuesta a la regeneración de las sectorizaciones
		private void OnCallBackCompleted(IAsyncResult result)
		{
			try
			{
				int retorno = ServicioParaTransaccion.EndRegeneraSectorizaciones(result);
				Session.Add("Sectorizando", false);
			}
			catch (System.Web.Services.Protocols.SoapException )
			{
				//Session.Add("Sectorizando", false);
			}
		}

		public string GetCallbackResult()
		{
			string sectorizando = "False";
            string versionActual = string.Empty;
            bool actualizar = false;

			if (Application["Sectorizando"] != null)
				sectorizando = ((bool)Application["Sectorizando"]).ToString();

			Sectorizando = sectorizando == "True";
			//TiempoPendienteTransaccion = ServicioParaTransaccion.TiempoPendienteDeTransaccion();		// -1= Transacción timeouted

            try
            {
                //Si por configuración, está habilitada la funcionalidad Sacta, se obtiene el estado del servicio Sacta
                //En caso contrario, no se invoca al Web Service para no sobrecargar la página
                if (bSactaActivoEnCnf)
                {
                    EstadoEnlaceSacta = (int)ServicioParaTransaccion.GetEstadoSacta();
                }
                else
                    EstadoEnlaceSacta=0; //Se indica que está deshabilitado


                versionActual = ServicioParaTransaccion.GetVersionConfiguracion((string)Session["idsistema"]);

                if (!string.IsNullOrEmpty(versionActual) && UltimaVersionSectorizacion != versionActual)
                {
                    UltimaVersionSectorizacion = versionActual;
                    actualizar = true;
                }
            }
            catch (System.Web.Services.Protocols.SoapException )
            {

            }

			return string.Format("{0};{1};{2};{3};{4};{5}", sectorizando,
                                                bVisualizandoActiva,
												(string)GetGlobalResourceObject("Espaniol", "FinTransaccion"),
												EstadoEnlaceSacta, versionActual,
												actualizar);
		}

		public void RaiseCallbackEvent(string eventArgument)
		{
			_Command = eventArgument;
		}
		#endregion

	}
}
