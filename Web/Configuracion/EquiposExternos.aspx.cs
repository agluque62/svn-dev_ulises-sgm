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
using log4net;
using log4net.Config;

public partial class Configuracion_EquiposExternos : PageBaseCD40.PageCD40	// System.Web.UI.Page
{
    private const int MAX_GRABADORES = 2;
    private const int MAX_EQUIPOS_TLF_INTERNA = 1;
    private const int RTSP_PORT = 554;
    private const int SIP_PORT = 5060;

    const string TIPO_EQUIPO_RADIO = "2";
    const string TIPO_EQUIPO_TLF = "3";
    const string TIPO_EQUIPO_GRABADOR = "5";

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
	private static ServiciosCD40.Tablas[] datos;
	private static ServiciosCD40.ServiciosCD40 ServicioAccesoABaseDeDatos = new ServiciosCD40.ServiciosCD40();
	static bool PermisoSegunPerfil;
    private static int NumGrabadores = 0;
    private static bool bNumGrabadoresExcedido = false;

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

			PermisoSegunPerfil = perfil == "3";
		}

		if (!IsPostBack)
		{
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

			BtNuevo.Visible = PermisoSegunPerfil;
            IndexListBox1 = -1;

			MuestraDatos(DameDatos());

			ActualizaWebPadre(true);
		}
		else
        {
                //Si se ha recargado la página, las variables datos y la variable de session tienen valor nulo es porque 
                // si ha cambiado la sesión del servidor, bien por conmutación o reinicio
                //por lo que se va a la página de login
                if (datos == null || Session["idsistema"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);
                }
                
                LblIp1Existente.Visible = LblIp2Existente.Visible = false;

                //if (Request.Form["eliminaelemento"] == "1")//El usuario elige eliminar el elemento 
                //{
                //    Request.Form["eliminaelemento"].Replace("1", "0");

                //    EliminarElemento();
                //}
                //if (Request.Form["cancelparam"] == "1")    //El usuario elige no guardar los cambios 
                //{
                //    Request.Form["cancelparam"].Replace("1", "0");

                //    CancelarCambios();
                //}
                //if (Request.Form["aceptparam"] == "1")     //El usuario elige guardar los cambios
                //{
                //    Request.Form["aceptparam"].Replace("1", "0");

                //    GuardarCambios();
                //}
			}
	}

	protected void BtNuevo_Click(object sender, EventArgs e)
	{
		MostrarMenu();
		BtAceptar.Visible = true;
		BtCancelar.Visible = true;
		BtModificar.Visible = false;
		BtNuevo.Visible = false;
		BtEliminar.Visible = false;
        TBSipPort.Text = "5060";
        DDLTipoEquipo.SelectedValue = TIPO_EQUIPO_RADIO; //  "2"-Radio
        ChangeLblPort(DDLTipoEquipo.SelectedValue);      //Se actualiza la etiqueta del Puerto acorde al tipo de equipo

		ListBox1.Enabled = false;
		TBIdEquipo.Enabled = true;
		TBIdEquipo.ReadOnly = false;
		TxtIP1.ReadOnly = false;
		TxtIP2.ReadOnly = false;
        TBSipPort.ReadOnly = false;
		DDLTipoEquipo.Enabled = true;
        IndexListBox1 = ListBox1.SelectedIndex;

		MostrarValidacion();
	}

    private void MostrarElemento()
    {
        MostrarMenu();
        BtModificar.Visible = PermisoSegunPerfil;
        TBIdEquipo.ReadOnly = true;
        TxtIP1.ReadOnly = true;
        TxtIP2.ReadOnly = true;
        TBIdEquipo.Enabled = true;
        TBSipPort.ReadOnly = true;
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        DDLTipoEquipo.Enabled = false;

        if (null != datos)
        {
            for (int i = 0; i < datos.Length; i++)
            {
                if (String.Compare((((ServiciosCD40.EquiposEU)datos[i]).IdEquipos), ListBox1.SelectedValue) == 0)
                {
                    BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarEquipoEU"), ListBox1.SelectedValue);

                    TBIdEquipo.Text = ((ServiciosCD40.EquiposEU)datos[i]).IdEquipos;
                    TxtIP1.Text = ((ServiciosCD40.EquiposEU)datos[i]).IpRed1;
                    TxtIP2.Text = ((ServiciosCD40.EquiposEU)datos[i]).IpRed2;

                    DDLTipoEquipo.SelectedValue = ((ServiciosCD40.EquiposEU)datos[i]).TipoEquipo.ToString();

                    //Se actualiza la etiqueta del puerto y el valor del puerto con el valor por defecto en función del tipo de equipo 
                    ChangeLblPort(DDLTipoEquipo.SelectedValue);

                    //Se actualiza el puerto con el valor del puerto configurado en la BD
                    TBSipPort.Text = ((ServiciosCD40.EquiposEU)datos[i]).SipPort.ToString();
                }
            }
        }
    }

	private void MostrarMenu()
	{
		LimpiarMenu();
		Label3.Visible = true;
		Label4.Visible = true;
		Label5.Visible = true;
        LblSipPort.Visible = true;
        TBSipPort.Visible = true;
		TBIdEquipo.Visible = true;
		TxtIP1.Visible = true;
		TxtIP2.Visible = true;
		DDLTipoEquipo.Visible = true;
		LblTipoEquipo.Visible = true;
        
        TBSipPort.Enabled = true;
	}

	private void EsconderMenu()
	{
		Label3.Visible = false;
		Label4.Visible = false;
		Label5.Visible = false;

		TxtIP1.Visible = false;
		TxtIP2.Visible = false;

		TBIdEquipo.Visible = false;
        LblSipPort.Visible = false;
        TBSipPort.Visible = false;

		LimpiarMenu();
		BtAceptar.Visible = false;
		BtCancelar.Visible = false;
		BtModificar.Visible = false;
		DDLTipoEquipo.Visible = false;
		LblTipoEquipo.Visible = false;
	}

	private void LimpiarMenu()
	{
		TBIdEquipo.Text = "";
		TxtIP1.Text = "";
		TxtIP2.Text = "";
	}

	protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ListBox1.SelectedIndex >= 0)
		{
            BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarEquipoEU"), ListBox1.SelectedValue);
            //BtEliminar.Visible = PermisoSegunPerfil;
			MostrarElemento();
		}
	}

	private ServiciosCD40.Tablas[] DameDatos()
	{
		try
		{
			ServiciosCD40.EquiposEU t = new ServiciosCD40.EquiposEU();
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
			t.IdSistema = s.Value;
			Session["idsistema"] = s.Value;

			ServiciosCD40.Tablas[] d = ServicioAccesoABaseDeDatos.ListSelectSQL(t);
			datos = d;
			return d;
		}
		catch (Exception ex)
		{
			logDebugView.Error("(EQUIPOSEU-DameDatos): ", ex);
		}
		return null;
	}

	private void MuestraDatos(ServiciosCD40.Tablas[] nu)
	{
		ListBox1.Items.Clear();
        NumGrabadores = 0;
        bNumGrabadoresExcedido = false;

        if (nu != null)
        {
            for (int i = 0; i < nu.Length; i++)
            {
                // Contabilizar el tipo de equipo Grabador porque sólo se permiten hasta dos grabadores
                if (((ServiciosCD40.EquiposEU)nu[i]).TipoEquipo == 5) /*Tipo grabador */
                    NumGrabadores++;

                // Los equipos que están definidos como centrales de encaminamiento AGVN desde la página
                // de encaminamientos no se muestran como equipos externos
                if (((ServiciosCD40.EquiposEU)nu[i]).Min != -1 && ((ServiciosCD40.EquiposEU)nu[i]).Max != -1)
                    ListBox1.Items.Add(((ServiciosCD40.EquiposEU)nu[i]).IdEquipos);
            }
        }

		if (ListBox1.Items.Count > 0)
		{
			ActualizaWebPadre(true);

			BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil;

            if (ListBox1.Items.FindByText(NewItem) != null)
            {
                ListBox1.Items.FindByText(NewItem).Selected = true;
                IndexListBox1 = ListBox1.SelectedIndex;
                NewItem = string.Empty;
            }
            else
                ListBox1.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < ListBox1.Items.Count ? IndexListBox1 : 0;

            MostrarElemento();
		}
		else
		{
			BtModificar.Visible = BtEliminar.Visible = false;
		}
	}

	//protected override void AceptarCambios()
	//{
	//    base.AceptarCambios();
	//}

	protected override void CancelarCambios()
	{
		//EsconderMenu();

		BtNuevo.Visible = PermisoSegunPerfil;
		BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
		ListBox1.Enabled = true;

        EsconderValidacion();
	}

    private void GuardarCambios()
    {
        try
        {
            string strSistema=(string)Session["idsistema"];

            //MVO: el número de grabadores se comprueba al guardar los cambios, tanto si es un alta como una modificación
            if (DDLTipoEquipo.SelectedValue == TIPO_EQUIPO_GRABADOR /* 5- Grabador*/ &&
                (NumGrabadores > MAX_GRABADORES || bNumGrabadoresExcedido))
            {
                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "MaximoNumeroGrabadores"));
                return;
            }

			ServiciosCD40.EquiposEU n = new ServiciosCD40.EquiposEU();
			n.IdSistema = strSistema;
            if (!TBIdEquipo.ReadOnly) //Nuevo Equipo Externo
            {
                //Se comprueba que no exista otro equipo EU con el mismos identificador o Central ATS en la tabla de encaminamientos
                //Porque en la tabla equiposEu, se almacenan los equipos externos y los identificadores de la central ATS
                //que tienen configurada una Central IP
                if (!bExisteEquipoExterno(n.IdSistema, TBIdEquipo.Text))
                    n.IdEquipos = TBIdEquipo.Text;
                else
                {
                    //Ya existe otro equipo externo o Central ATS con el mismo identificador 
                    cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ErrorEU_Existente"));
                    return;
                }
            }
            else
                n.IdEquipos = ListBox1.SelectedValue;

            //Si el equipo externo es de tipo grabador, se comprueba que no exista otro equipo externo grabador con la misma dirección IP
            if (DDLTipoEquipo.SelectedValue == TIPO_EQUIPO_GRABADOR)
            {
              if (ServicioAccesoABaseDeDatos.ExisteGrabadorMismaIP(strSistema, TxtIP1.Text, n.IdEquipos))
              {
                  cMsg.alert(string.Format((string)GetGlobalResourceObject("Espaniol", "AvisoConfGrabadorMismaIp"),TxtIP1.Text));
                  return;
              }
                else if ((string.Compare(TxtIP1.Text, TxtIP2.Text) != 0) && ServicioAccesoABaseDeDatos.ExisteGrabadorMismaIP(strSistema, TxtIP2.Text, n.IdEquipos))
              {
                cMsg.alert(string.Format((string)GetGlobalResourceObject("Espaniol", "AvisoConfGrabadorMismaIp"), TxtIP2.Text));
                return;
              }
            }

            NewItem = n.IdEquipos;

			n.IpRed1 = TxtIP1.Text;
			n.IpRed2 = TxtIP2.Text;
			n.TipoEquipo = Convert.ToUInt32(DDLTipoEquipo.SelectedValue);
            n.Interno = false;   // Siempre es false, excepto para el caso del equipo externo asociado a la Central ATS propia
            n.SipPort = DDLTipoEquipo.SelectedValue == TIPO_EQUIPO_GRABADOR /* Grabador */ ? RTSP_PORT : Convert.ToInt32(TBSipPort.Text);

			// Comprobar que ninguna de las IPs existen ya dadas de alta en el sistema.
            /* SE ELIMINA ESTA RESTRICCIÓN PARA PODER CONFIGURAR EQUIPOS VINCULADOS A UN PROXY */
            /*
			if (ServicioAccesoABaseDeDatos.ExisteIP(n.IdSistema, n.IpRed1, n.IdEquipos))
			{
				LblIp1Existente.Visible = true;
				return;
			}
			else if (ServicioAccesoABaseDeDatos.ExisteIP(n.IdSistema, n.IpRed2, n.IdEquipos))
			{
				LblIp2Existente.Visible = true;
				return;
			}
			*/
            if (!TBIdEquipo.ReadOnly) //Equipo nuevo
            {
                if (ServicioAccesoABaseDeDatos.InsertSQL(n) < 0) logDebugView.Warn("(EQUIPOSEU-GuardarCambios): No se ha podido guardar la EQUIPOSEU");
            }
            else
            {
                if (ServicioAccesoABaseDeDatos.UpdateSQL(n) < 0) logDebugView.Warn("(EQUIPOSEU-GuardarCambios): No se ha podido actualizar la EQUIPOSEU");
            }
		}
		catch (Exception ex)
		{
			logDebugView.Error("(EQUIPOSEU-GuardarCambios): ", ex);
		}

		EsconderMenu();

		ListBox1.Enabled = true;
		BtNuevo.Visible = PermisoSegunPerfil;
		BtEliminar.Visible = BtModificar.Visible = false;
		EsconderValidacion();
		MuestraDatos(DameDatos());
	}

	protected void BtCancelar_Click(object sender, EventArgs e)
	{
        CancelarCambios();
        MuestraDatos(DameDatos());
		//cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");
	}

	protected void BtAceptar_Click(object sender, EventArgs e)
	{
        GuardarCambios();
		//cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AceptarCambios"), "aceptparam");
	}

	protected void BtModificar_Click(object sender, EventArgs e)
	{
		BtModificar.Visible = false;
		BtAceptar.Visible = true;
		BtCancelar.Visible = true;
        BtEliminar.Visible = false;

		TxtIP1.ReadOnly = false;
		TxtIP2.ReadOnly = false;
        TBSipPort.ReadOnly = false;

        //MVO 26/06/2017: No se permite modificar el tipo de equipo si se cumple alguna de las siguientes condiciones:
            // Si el equipo externo es de tipo grabador, y está configurado en alguna pasarela o terminal de operador como grabador 1 o grabador 2
            // Si el equipo es de tipo telefonía o radio, y está asignado a un recurso de telefónía o radio respectivamente.

        if (bExistenRecursosAsignados(ListBox1.SelectedValue, DDLTipoEquipo.SelectedValue))
        {
            DDLTipoEquipo.Enabled = false;
        }
        else
		    DDLTipoEquipo.Enabled = true;

        IndexListBox1 = ListBox1.SelectedIndex;
        
        MostrarValidacion();
	}

    private bool bExistenRecursosAsignados(string strNombreEquipoExt, string strTipoEquipoExt)
    {
        //MVO 26/06/2017: se verifica si el equipo externo está asignado a algún recurso (radio o telefonía) o pasarela y terminal de operador (grabador), en función del tipo
        //                Si el equipo es de tipo grabador, el grabador puede estar configurado en alguna pasarela o terminal de operador como grabador 1 o grabador 2
        //                Si el equipo es de tipo telefonía o radio, el equipo puede estar asignado a un recurso de telefónía o radio respectivamente.
        
        ServiciosCD40.Tablas[] t = null;
        bool bExiste = false;

        if (!string.IsNullOrEmpty(strNombreEquipoExt) && !string.IsNullOrEmpty(strTipoEquipoExt))
        {
            switch (strTipoEquipoExt)
            {
                case TIPO_EQUIPO_GRABADOR:

                    // Busca alguna pasarela que tenga este grabador configurado como Grabador1 o Grabador2
                    ServiciosCD40.TifX r = new ServiciosCD40.TifX();
                    r.IdSistema = (string)Session["idsistema"];
                    r.Grabador1 = strNombreEquipoExt;

                    t = ServicioAccesoABaseDeDatos.ListSelectSQL(r);
                    if (t != null && t.Length > 0)
                    {
                        // No se pueden borrar el equipo que está asignado a una pasarela
                        bExiste = true;
                    }
                    else
                    {

                        // Busca alguna top (terminal de operador) que tenga este grabador configurado como Grabador1 o Grabador2
                        ServiciosCD40.Top tops = new ServiciosCD40.Top();
                        tops.IdSistema = (string)Session["idsistema"];
                        tops.Grabador1 = strNombreEquipoExt;

                        t = ServicioAccesoABaseDeDatos.ListSelectSQL(tops);
                        if (t != null && t.Length > 0)
                        {
                            // No se pueden borrar el equipo que está asignado a un terminal de operador
                            bExiste = true;
                        }
                    }

                    break;
                case TIPO_EQUIPO_RADIO:
                case TIPO_EQUIPO_TLF:

                    //Se comprueba si el equipo radio está asociado a un recurso radio o de telefonía                                
                    ServiciosCD40.Recursos objRec = new ServiciosCD40.Recursos();
                    objRec.IdSistema = (string)Session["idsistema"];
                    objRec.IdEquipo = strNombreEquipoExt;

                    t = ServicioAccesoABaseDeDatos.ListSelectSQL(objRec);
                    if (t != null && t.Length > 0)
                    {
                        // No se pueden borrar equipos externos de tipo radio o telefonía que están asignados a recursos
                        bExiste = true;
                    }

                    break;
                default:
                    bExiste = false;
                    break;
            }
        }

        return bExiste;

    }

	protected void BtEliminar_Click(object sender, EventArgs e)
	{
		if (ListBox1.SelectedValue != "")
		{
            //MVO 26/06/2017: antes de borrar el equipo externo, se verifica si hay algún recurso que lo tenga asignado, en función del tipo de equipo
            bool bRefExternas = false;

            bRefExternas = bExistenRecursosAsignados(ListBox1.SelectedValue, DDLTipoEquipo.SelectedValue);

            if (bRefExternas)
            {
                // No se pueden borrar equipos de tipo radio o telefonía que están asignados a recursos o de tipo Grabador si están asociados a pasarelas o terminales de operador.
                string strMsgAux = string.Format((string)GetGlobalResourceObject("Espaniol", "EquipoAsignadoARecurso"), ListBox1.SelectedValue);
                cMsg.alert(strMsgAux);
                return;
            }
            else
            {
                EsconderMenu();
                Session["elemento"] = ListBox1.SelectedValue;
                IndexListBox1 = ListBox1.SelectedIndex;
                EliminarElemento();
            }

		}
	}

	private void EliminarElemento()
	{
		try
		{
			ServiciosCD40.EquiposEU n = new ServiciosCD40.EquiposEU();
			n.IdSistema = (string)Session["idsistema"];
			n.IdEquipos = (string)Session["elemento"];

            if (ServicioAccesoABaseDeDatos.DeleteSQL(n) < 0)
                logDebugView.Warn("(EQUIPOSEU-EliminarElemento): No se ha podido eliminar la EQUIPOSEU.");            
            else
			  cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
		}
		catch (Exception ex)
		{
			logDebugView.Error("Error al eliminar equipo externo(EQUIPOSEU-EliminarElemento): ", ex);
		}
		MuestraDatos(DameDatos());
	}

	private void EsconderValidacion()
	{
		RequiredFieldIdTIFX.Visible = false;
        RequiredFieldIP1.Visible = false;
		RequiredFieldIP2.Visible = false;
		RegularExpressionValidator1.Visible = false;
        RegularExpressionValidator2.Visible = false;
		ValidationSummary1.Visible = false;
	}

	private void MostrarValidacion()
	{
		RequiredFieldIdTIFX.Visible = true;
        RequiredFieldIP1.Visible = true;
        RequiredFieldIP2.Visible = true;
		RegularExpressionValidator1.Visible = true;
        RegularExpressionValidator2.Visible = true;
		ValidationSummary1.Visible = true;
	}

    private void ChangeLblPort(string value)
    {
        // Habilitar o no el puerto SIP
        RangeValidatorSIPLocal.Enabled = TBSipPort.Enabled = DDLTipoEquipo.SelectedValue != TIPO_EQUIPO_GRABADOR; // 5- Grabador
        
        // Cambiar SIP por RTSP o vicecersa según el tipo de equipo
        string[] lbl = LblSipPort.Text.Split(' ');
        string newLbl = string.Empty;
        for (int i=0;i<lbl.Length;i++)
        {
            if (value == TIPO_EQUIPO_GRABADOR)
            {
                lbl[i] = lbl[i] == "SIP" ? "RTSP" : lbl[i];
                TBSipPort.Text = RTSP_PORT.ToString();
            }
            else
            {
                lbl[i] = lbl[i] == "RTSP" ? "SIP" : lbl[i];
                TBSipPort.Text = SIP_PORT.ToString();
            }

            newLbl += lbl[i] + " ";
        }

        LblSipPort.Text = newLbl == string.Empty ? LblSipPort.Text : newLbl;
    }

    protected void DDLTipoEquipo_SelectedIndexChanged(object sender, EventArgs e)
    {       

        //Si el tipo seleccionado es grabador, se comprueba si se ha llegado al máximo
        if (((DropDownList)sender).SelectedValue == TIPO_EQUIPO_GRABADOR)
        {
            if (NumGrabadores >= MAX_GRABADORES)
                bNumGrabadoresExcedido = true;
            else
                bNumGrabadoresExcedido = false;
        }

        // Cambiar SIP por RTSP o vicecersa según el tipo de equipo
        ChangeLblPort(((DropDownList)sender).SelectedValue);

    }

    private bool bExisteEquipoExterno(string strIdSistema, string strIdEquipo)
    {
        //Comprueba si en la BD existe un equipo externo con el mismo identificador o con el nombre de una Central ATS o un top o una pasarela
        //Porque en este grupo los identificadores deben ser únicos
        //Porque en la tabla equiposEu, se almacenan los equipos externos y los identificadores de la central ATS
        //que tienen configurada una Central IP

        bool bExiste = false;
        int iExiste = -1;

        iExiste = ServicioAccesoABaseDeDatos.CheckIdentificadorAsignado("EF", strIdSistema, strIdEquipo);

        if (iExiste > 0)
            bExiste = true;
        else if (iExiste < 0)
        {
            System.Text.StringBuilder strMsgError = new System.Text.StringBuilder();
            strMsgError.AppendFormat("(EquiposExternos-bIdentificadorAsignado): el servicio servicioCD40.CheckIdentificadorAsignado('EF', '{0}', '{1}') ha devuelto el codigo {2}", strIdSistema, strIdEquipo, iExiste);
            logDebugView.Error(strMsgError.ToString());
            strMsgError.Clear();
        }

        return bExiste;
    }
}
