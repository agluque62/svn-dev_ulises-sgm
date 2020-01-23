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
using log4net;
using log4net.Config;

public partial class Operadores : PageBaseCD40.PageCD40	// System.Web.UI.Page
{
	private Mensajes.msgBox cMsg;
	private static ServiciosCD40.Tablas[] datos;
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
	static ServiciosCD40.ServiciosCD40 Servicio = new ServiciosCD40.ServiciosCD40();
	private static bool Modificando;

	protected new void Page_Load(object sender, EventArgs e)
    {
		base.Page_Load(sender, e);

		cMsg = (Mensajes.msgBox)this.Master.FindControl("MsgBox1");

		if (Context.Request.IsAuthenticated)
		{
			// retrieve user's identity from httpcontext user 
			FormsIdentity ident = (FormsIdentity)Context.User.Identity;
			string perfil = ident.Ticket.UserData;
			if (perfil != "3")	// Sólo el perfil 3 (Técnico 3) tiene acceso a Operadores.
			{
                Response.Redirect("~/Configuracion/Inicio.aspx?Permiso=NO", false);
				return;
			}
		}

        if (!IsPostBack)
        {
            // Servicio.NoTransaction();
            BtnAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtnCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            //logDebugView.Debug("Entrando en Operadores....");
            IndexListBox1 = -1;

            MuestraDatos(DameDatos());
        }
        else
        {

            if (datos == null)
            {
                //Si se recarga la pagina y el vector datos es null lo recargamos
                //Dentro de la función DameDatos, se lee la variable de sesión Session["idsistema"] del ficheor Web.config
                DameDatos();
            }
        }
        //else
        //{
            //if (Request.Form["aceptparam"] == "1")//El usuario elige eliminar el elemento 
            //{
            //    Request.Form["aceptparam"].Replace("1", "0");

            //    AnadirOperador();
            //}
            //else 
            //if (Request.Form["modifparam"] == "1")//El usuario elige eliminar el elemento 
            //{
            //    Request.Form["modifparam"].Replace("1", "0");

            //    ModificarOperador();
            //}
			//else 
            //if (Request.Form["eliminaElemento"] == "1")//El usuario elige eliminar el elemento 
            //{
            //    Request.Form["eliminaElemento"].Replace("1", "0");

            //    EliminaOperador();
            //}
            //else if (Request.Form["cancelaElemento"] == "1")//El usuario elige eliminar el elemento 
            //{
            //    Request.Form["cancelaElemento"].Replace("1", "0");

            //    CancelarOperador();
            //}
			
		//}
    }

	private void CancelarOperador()
	{
		if (!Modificando)
		{
			if (LBOperadores.Items.Count > 0)
                LBOperadores.SelectedIndex = IndexListBox1 != -1 ? IndexListBox1 : 0;
            
            MostrarElemento();
		}

		TerminaActualizacion();
	}

	private void EliminaOperador()
	{
		ServiciosCD40.Operadores op = new ServiciosCD40.Operadores();
		op.IdSistema = (string)Session["idsistema"];
		op.IdOperador = TBUsuario.Text;

		if (Servicio.DeleteSQL(op) > 0)
		{
			cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
			TerminaActualizacion();

			Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
			KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
			if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
			{
				SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
				switch (sincro.BajaOperador(op.IdOperador))
				{
					case 137:
						cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod137"), op.IdOperador));
						break;
					case 138:
						cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod138"), op.IdOperador));
						break;
					default:
						break;
				}
			}
		}
	}

	private void AnadirOperador()
	{
		ServiciosCD40.Operadores op = new ServiciosCD40.Operadores();
		op.IdSistema = (string)Session["idsistema"];
		op.IdOperador = TBUsuario.Text;
		op.Clave = HFClave.Value;
		op.NivelAcceso = Convert.ToUInt32(DDLPerfil.SelectedValue);
		op.Nombre = TBNombre.Text;
		op.Apellidos = TBApellidos.Text;
		op.Telefono = TBTelefono.Text;
        op.FechaUltimoAcceso = DateTime.Today;
        NewItem = TBUsuario.Text;

		if (Servicio.InsertSQL(op) > 0)
		{
			cMsg.alert((string)GetGlobalResourceObject("Espaniol", "OperadorDadoDeAlta"));
			TerminaActualizacion();

			ActualizaWebPadre(true);

			Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
            if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
            {
                SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                switch (sincro.AltaOperador(op.IdOperador,(int)op.NivelAcceso,op.Clave,op.Nombre,op.Apellidos,op.Telefono))
                {
                    case 135:
						cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod135"), op.IdOperador));
                        break;
                    case 136:
						cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod136"), op.IdOperador));
                        break;
                    default:
                        break;
                }
            }
        }
	}

	private void ModificarOperador()
	{
		ServiciosCD40.Operadores op = new ServiciosCD40.Operadores();
		op.IdSistema = (string)Session["idsistema"];
		op.IdOperador = TBUsuario.Text;
		op.Clave = HFClave.Value;
		op.NivelAcceso = Convert.ToUInt32(DDLPerfil.SelectedValue);
		op.Nombre = TBNombre.Text;
		op.Apellidos = TBApellidos.Text;
		op.Telefono = TBTelefono.Text;
        op.FechaUltimoAcceso = DateTime.Today;

        IndexListBox1 = LBOperadores.SelectedIndex;

		if (Servicio.UpdateSQL(op) > 0)
		{
			cMsg.alert((string)GetGlobalResourceObject("Espaniol", "OperadorModificado"));

			TerminaActualizacion();

			Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
			KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
			if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
			{
				SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
				switch (sincro.ModificaOperador(op.IdOperador, (int)op.NivelAcceso, op.Clave, op.Nombre, op.Apellidos, op.Telefono))
				{
					case 139:
						cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod139"), op.IdOperador));
						break;
					case 140:
						cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod140"), op.IdOperador));
						break;
					default:
						break;
				}
			}
		}
	}

	private ServiciosCD40.Tablas[] DameDatos()
	{
		try
		{
			ServiciosCD40.Operadores t = new ServiciosCD40.Operadores();
			Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
			KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
			t.IdSistema = s.Value;
			Session["idsistema"] = s.Value;

			ServiciosCD40.Tablas[] d = Servicio.ListSelectSQL(t);
			datos = d;
			return d;
		}
		catch (Exception e)
		{
			logDebugView.Error("(Operadores-DameDatos):", e);
		}
		return null;
	}

	private void MuestraDatos(ServiciosCD40.Tablas[] nu)
	{
		LBOperadores.Items.Clear();
		if (nu != null)
			for (int i = 0; i < nu.Length; i++)
				LBOperadores.Items.Add(((ServiciosCD40.Operadores)nu[i]).IdOperador);

		if (LBOperadores.Items.Count > 0)
		{
            if (LBOperadores.Items.FindByText(NewItem) != null)
                LBOperadores.Items.FindByText(NewItem).Selected = true;
            else
                LBOperadores.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < LBOperadores.Items.Count ? IndexListBox1 : 0;

			BtnModificar.Visible = BtnEliminar.Visible = true;
			MostrarElemento();
            BtnEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarOperador"), LBOperadores.SelectedValue);
        }
		else
			BtnModificar.Visible = BtnEliminar.Visible = false;
	}

	private void MostrarElemento()
	{
		if (LBOperadores.Items.Count > 0 && null!=datos)
		{
			ActualizaWebPadre(true);

			int elemento = LBOperadores.SelectedIndex;
			TBUsuario.Text = ((ServiciosCD40.Operadores)datos[elemento]).IdOperador;
			HFClave.Value = ((ServiciosCD40.Operadores)datos[elemento]).Clave;
			DDLPerfil.SelectedValue = ((ServiciosCD40.Operadores)datos[elemento]).NivelAcceso.ToString();
			TBNombre.Text = ((ServiciosCD40.Operadores)datos[elemento]).Nombre;
			TBApellidos.Text = ((ServiciosCD40.Operadores)datos[elemento]).Apellidos;
			TBTelefono.Text = ((ServiciosCD40.Operadores)datos[elemento]).Telefono;
		}
	}

	protected void BtnModificar_Click(object sender, EventArgs e)
	{
		Modificando = true;
		Panel1.Enabled = true;
		BtnAceptar.Visible = BtnCancelar.Visible = true;
		BtnEliminar.Visible = BtnNuevo.Visible = BtnModificar.Visible = false;
		LBOperadores.Enabled = false;
		TBUsuario.Enabled = false;
		LkBCambiarClave.Visible = true;
    
        IndexListBox1 = LBOperadores.SelectedIndex;
    }

	private void ResetItems()
	{
		TBClave.Text=string.Empty;
		TBConfirmaClave.Text = string.Empty;
		TBNombre.Text = string.Empty;
		TBApellidos.Text = string.Empty;
		TBTelefono.Text = string.Empty;
		DDLPerfil.SelectedIndex = 0;
		if (!Modificando)
			TBUsuario.Text = string.Empty;
	}

	protected void BtnNuevo_Click(object sender, EventArgs e)
	{
		Modificando = false;
		Panel1.Enabled = true;
		Label8.Visible = TBConfirmaClave.Visible = true;
		Label2.Visible = TBClave.Visible = true;
		BtnAceptar.Visible = BtnCancelar.Visible = true;
		BtnEliminar.Visible = BtnNuevo.Visible = BtnModificar.Visible = false;
		LBOperadores.Enabled = false;
		TBUsuario.Enabled = true;
//		if (LBOperadores.Items.Count > 0)
//			LBOperadores.SelectedIndex = 0;

		ResetItems();
	}

	protected void BtnCancelar_Click(object sender, EventArgs e)
	{
        CancelarOperador();
		cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelaElemento");
	}

	private void TerminaActualizacion()
	{
		LBOperadores.Enabled = true;
		LblErrorClaveAntigua.Visible = false;
		LblErrorConfirmacion.Visible = false;
		LblMensajeConfirmacion.Visible = false;
		Modificando = false;
		Panel1.Enabled = false;
		Label8.Visible = TBConfirmaClave.Visible = false;
		Label2.Visible = TBClave.Visible = false;
		Label9.Visible = TBClaveAntigua.Visible = false;
		BtnEliminar.Visible = BtnEliminar.Visible = LBOperadores.Items.Count > 0;
		BtnNuevo.Visible = true;
		LBOperadores.Enabled = true;
		BtnAceptar.Visible = BtnCancelar.Visible = false;
		LkBCambiarClave.Visible = false;

		MuestraDatos(DameDatos());
	}

	protected void BtnAceptar_Click(object sender, EventArgs e)
	{
		if (Modificando && TBClaveAntigua.Visible && HFClave.Value != TBClaveAntigua.Text)
		{
			LblErrorClaveAntigua.Visible = true;
			LblMensajeConfirmacion.Text = (string)GetLocalResourceObject( "MsgClaveAntiguaVacia");
			LblMensajeConfirmacion.Visible = true;
		}
		else if (TBClave.Visible && TBClave.Text != TBConfirmaClave.Text)
		{
			LblErrorConfirmacion.Visible = true;
            LblMensajeConfirmacion.Text = (string)GetLocalResourceObject("MsgConfirmeClave");
			LblMensajeConfirmacion.Visible = true;
		}
		else
		{
			if (!Modificando || TBClave.Visible) 
				HFClave.Value = TBClave.Text;

            if (Modificando)
                ModificarOperador();
            else
                AnadirOperador();
			//cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AceptarCambios"), Modificando ? "modifparam" : "aceptparam");
		}
	}

	protected void LkBCambiarClave_Click(object sender, EventArgs e)
	{
		Label8.Visible = TBConfirmaClave.Visible = true;
		Label2.Visible = TBClave.Visible = true;
		Label9.Visible = TBClaveAntigua.Visible = true;
	}

	protected void LBOperadores_SelectedIndexChanged(object sender, EventArgs e)
	{
        if (LBOperadores.SelectedIndex >= 0)
        {
            BtnEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarOperador"), LBOperadores.SelectedValue);
            MostrarElemento();
        }
	}

	protected void BtnEliminar_Click(object sender, EventArgs e)
	{
        EliminaOperador();
        //string texto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarOperador"), LBOperadores.SelectedValue);

        //cMsg.confirm(texto, "eliminaElemento");
	}
}
