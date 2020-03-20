using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using log4net;
using log4net.Config;

public partial class TOP :	PageBaseCD40.PageCD40	// System.Web.UI.Page
{
    private AsyncCallback CallbackCompletado;
    /// <summary>
    /// 
    /// </summary>
    private Mensajes.msgBox cMsg;
    /// <summary>
    /// 
    /// </summary>
    private static ILog _logDebugView;
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
    private static ServiciosCD40.Tablas[] datos;
    /// <summary>
    /// 
    /// </summary>
	static bool PermisoSegunPerfil;
    /// <summary>
    /// 
    /// </summary>
	private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();
    /// <summary>
    /// 
    /// </summary>
	private static bool Modificando = false;

    /// <summary>
    /// 
    /// </summary>
    private Ulises5000Configuration.ToolsUlises5000Section UlisesToolsVersion;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
				Response.Redirect("~/Configuracion/Inicio.aspx?Permiso=NO");
				return;
			}

			PermisoSegunPerfil = perfil == "3";
        
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            Ulises5000Configuration.ToolsUlises5000Section UlisesTools = Ulises5000Configuration.ToolsUlises5000Section.Instance;

            UlisesToolsVersion = UlisesTools;
        }

		//ServicioCD40.NoTransaction();	// Las actuaciones sobre la base de datos se realizarán inmediatamente, independientemente de TransactionTimeout

        if (CallbackCompletado == null)
            CallbackCompletado = new AsyncCallback(OnCallBackCompleted);

		if (!IsPostBack)
		{
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            IndexListBox1 = -1;

            //Mostrar grabación ED137 sólo para Nouakchott (Version==1)
            if (UlisesToolsVersion.Tools["GrabacionRecursoRadio"] == null)
                TblRecorders.Visible = false;

            //Se lee la variable de sesión idsistema
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            Session["idsistema"] = s.Value;

            CargaDDLGrabadores();

            BtNuevo.Visible = PermisoSegunPerfil;
			MuestraDatos(DameDatos());
		}
		else
		{
            LblIp1Existente.Visible = LblIp2Existente.Visible = false;

                //if (Request.Form["eliminaelemento"] == "1")	//El usuario elige eliminar el elemento 
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

            if (Session["idsistema"] == null)
            {
                //Si la variable de sesión idsistema es nula, se vuelve a recuperar
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
                Session["idsistema"] = s.Value;
            }

            if (datos == null)
                DameDatos();
       }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtNuevo_Click(object sender, EventArgs e)
    {
        IndexListBox1 = ListBox1.SelectedIndex;
        MostrarMenu();
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        BtModificar.Visible = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        ListBox1.Enabled = false;
        TxtIdTop.Enabled = true;
        TxtIdTop.ReadOnly = false;
        TxtIP1.ReadOnly = false;
        TxtIP2.ReadOnly = false;
        DDLRecorder2.Enabled = true;
        DDLRecorder1.Enabled = true;

        DListPosicion.Enabled = PermisoSegunPerfil;
        DListArranque.Enabled = PermisoSegunPerfil;
        
        MostrarGestionErrores();
	}

    /// <summary>
    /// 
    /// </summary>
    private void MostrarElemento()
    {
        MostrarMenu();
        
        BtModificar.Visible = PermisoSegunPerfil;
        
        TxtIdTop.ReadOnly = true;
        TxtIP1.ReadOnly = true;
        TxtIP2.ReadOnly = true;
        TxtIdTop.Enabled = false;
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;

        //MVO: Se deshabilitan los grabadores
        DDLRecorder1.Enabled = false;
        DDLRecorder2.Enabled = false;

        if (datos != null)
        {
            for (int i = 0; i < datos.Length; i++)
            {
                if (String.Compare((((ServiciosCD40.Top)datos[i]).IdTop), ListBox1.SelectedValue) == 0)
                {
                    BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarTop"), ListBox1.SelectedValue);

                    TxtIdTop.Text = ((ServiciosCD40.Top)datos[i]).IdTop;
                    TxtIP1.Text = ((ServiciosCD40.Top)datos[i]).IpRed1;
                    TxtIP2.Text = ((ServiciosCD40.Top)datos[i]).IpRed2;

                    try
                    {
                        if (((ServiciosCD40.Top)datos[i]).Grabador1 == null || ((ServiciosCD40.Top)datos[i]).Grabador1 == string.Empty)
                            DDLRecorder1.SelectedIndex = 0;
                        else
                            DDLRecorder1.SelectedValue = ((ServiciosCD40.Top)datos[i]).Grabador1;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        DDLRecorder1.SelectedIndex = 0;
                    }

                    try
                    {
                        if (((ServiciosCD40.Top)datos[i]).Grabador2 == null || ((ServiciosCD40.Top)datos[i]).Grabador2 == string.Empty)
                            DDLRecorder2.SelectedIndex = 0;
                        else
                            DDLRecorder2.SelectedValue = ((ServiciosCD40.Top)datos[i]).Grabador2;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        DDLRecorder2.SelectedIndex = 0;
                    }

                    ActualizaPosiciones((int)((ServiciosCD40.Top)datos[i]).PosicionSacta);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void MostrarMenu()
    {
        LimpiarMenu();
        Label1.Visible = true;
        
        Label3.Visible = true;
        
        Label5.Visible = true;
        TxtIdTop.Visible = true;
        DListPosicion.Visible = true;
        TxtIP1.Visible = true;
        
        //MVO 2017/07/05 el modo arranque y la IP de la red 2 se hace no visible 
        //DListArranque.Visible = true;
        //Label2.Visible = true;
        //TxtIP2.Visible = true;
        //Label4.Visible = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void EsconderMenu()
    {
        OcultarGestionErrores();
        Label1.Visible = false;        
        Label3.Visible = false;
        
        Label5.Visible = false;
        DListPosicion.Visible = false;        
        TxtIP1.Visible = false;        
        
        //MVO 2017/07/05 el modo arranque se hace no visible 
        //DListArranque.Visible = false;
        //Label2.Visible = false;
        //Label4.Visible = false;
        //TxtIP2.Visible = false;

        TxtIdTop.Visible = false;
        LimpiarMenu();
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        BtModificar.Visible = false;

        DListPosicion.Enabled = false;
        DListArranque.Enabled = false;
    }

    /// <summary>
    /// 
    /// </summary>
    private void LimpiarMenu()
    {
        TxtIdTop.Text = "";
        TxtIP1.Text = "";
        TxtIP2.Text = "";
        ActualizaPosiciones(-1);
        DDLRecorder1.SelectedIndex = DDLRecorder2.SelectedIndex = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarTop"), ListBox1.SelectedValue);
            //BtEliminar.Enabled = PermisoSegunPerfil;
            MostrarElemento();
        } 
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.Top t = new ServiciosCD40.Top();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;

			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            datos = d;
            return d;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TOP-DameDatos): ",ex);
        }
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nu"></param>
    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
        ListBox1.Items.Clear();
        if (nu!=null)
            for (int j = 0; j < nu.Length; j++)
                ListBox1.Items.Add(((ServiciosCD40.Top)nu[j]).IdTop);

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
			LimpiarMenu();
			BtModificar.Visible = BtEliminar.Visible = false;
		}

        //	ActualizaPosiciones(-1);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
	private void ActualizaPosiciones(int pos)
    {
        List<int> posiciones = new List<int>();
        for (int j = 1; j < 256; j++)
            posiciones.Add(j);

        if (datos != null)
        {
            for (int j = 0; j < datos.Length; j++)
                posiciones.Remove((int)((ServiciosCD40.Top)datos[j]).PosicionSacta);
        }

        DListPosicion.Items.Clear();
        foreach (int n in posiciones)
            DListPosicion.Items.Add(n.ToString());
        if (pos >= 1)
        {
            DListPosicion.Items.Add(pos.ToString());
            DListPosicion.SelectedIndex = DListPosicion.Items.IndexOf(new ListItem(pos.ToString()));
        }
    }

    /// <summary>
    /// 
    /// </summary>
	protected override void AceptarCambios()
	{
		base.AceptarCambios();
	}

    /// <summary>
    /// 
    /// </summary>
    protected override void CancelarCambios()
    {
		//ServicioCD40.Rollback(NombreSavePoint);
		
		EsconderMenu();
		Modificando = false;

		BtNuevo.Visible = PermisoSegunPerfil;
		BtEliminar.Visible = BtModificar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
        ListBox1.Enabled = true;


		MuestraDatos(DameDatos());
    }

    /// <summary>
    /// 
    /// </summary>
    private void GuardarCambios(string strSistema)
    {
        try
        {
            LblIp1Existente.Visible = LblIp2Existente.Visible = false;

            ServiciosCD40.Top n = new ServiciosCD40.Top();
            n.IdSistema = strSistema;
            if (!Modificando) //TOP nueva
                n.IdTop = TxtIdTop.Text;
            else
                n.IdTop = ListBox1.SelectedValue;

            NewItem = n.IdTop;

            n.PosicionSacta = UInt16.Parse(DListPosicion.SelectedValue);
            n.IpRed1 = TxtIP1.Text;

            //Si la direccion IP2 no se informa (está oculta), se configura con el valor de la IP1, ya que la IP2 era requerida
            if (string.IsNullOrEmpty(TxtIP2.Text) || false==TxtIP2.Visible)
            {
                n.IpRed2 = TxtIP1.Text;
                //TxtIP2.Text=TxtIP1.Text;
            }
            else
                n.IpRed2 = TxtIP2.Text;
            
            n.ModoArranque = DListArranque.SelectedValue;

			// Comprobar que ninguna de las IPs existen en el sistema.
			if (ServicioCD40.ExisteIP(n.IdSistema, n.IpRed1, n.IdTop))
			{
				LblIp1Existente.Visible = true;
				return;
			}
            else if (false==n.IpRed1.Equals(n.IpRed2))
            {
                //Si las 2 direcciones IP no son iguales, se comprueba si la segunda dirección IP no está asignada a otro elemento
                if (ServicioCD40.ExisteIP(n.IdSistema, n.IpRed2, n.IdTop))
                {
                    LblIp2Existente.Visible = true;
                    return;
                }
            }
           

            n.Grabador1 = DDLRecorder1.SelectedIndex != 0 ? DDLRecorder1.SelectedValue : null;
            n.Grabador2 = DDLRecorder2.SelectedIndex != 0 ? DDLRecorder2.SelectedValue : null;

            if (!Modificando) //TOP nueva
            {
                if (ServicioCD40.InsertSQL(n) < 0) logDebugView.Warn("(TOP-GuardarCambios): No se ha podido guardar la TOP.");

                ActualizaWebPadre(true);
            }
            else
            {
                if (ServicioCD40.UpdateSQL(n) < 0) logDebugView.Warn("(TOP-GuardarCambios): No se ha podido actualizar la TOP.");
                IndexListBox1 = ListBox1.SelectedIndex;
            }

			Modificando = false;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(TOP-GuardarCambios): ",ex);
        }

        EsconderMenu();
        
        ListBox1.Enabled = true;
        BtNuevo.Visible = PermisoSegunPerfil;
		BtEliminar.Visible = BtModificar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
        
        MuestraDatos(DameDatos());
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        //Si se está dando de alta un nuevo top
        // se verifica que el identificador sea único en el sistema
        string StrSistema = (string)Session["idsistema"];

        if (!Modificando && TxtIdTop.Enabled && bIdentificadorAsignado(StrSistema, TxtIdTop.Text))
        {
            //MVO: Si se está insertando un top y existe un elemento físico con el mismo identificador en el sistema, se informa al usuario para que indique otro identificador
            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorIdentificadorEltoFisicoExiste"), TxtIdTop.Text));
        }
        else
        {
            //Se comprueba si se ha seleccionado correctamente los grabadores
            if ((0 != DDLRecorder2.SelectedIndex) && (0 == DDLRecorder1.SelectedIndex))
            {
                //No se permite configurar el grabador 2, si previamente no se ha configurado el grabador1
                cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AvisoConfGrabadorEnTop"), "aceptparam");
            }
            else
            {
                //Se comprueba que los grabadores tienen que ser distintos
                if ((0 != DDLRecorder1.SelectedIndex) && (0 != DDLRecorder2.SelectedIndex) && (DDLRecorder1.SelectedValue == DDLRecorder2.SelectedValue))
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AvisoConfGrabadorDistintos"), "aceptparam");
                else
                    GuardarCambios(StrSistema);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtModificar_Click(object sender, EventArgs e)
    {
        IndexListBox1 = ListBox1.SelectedIndex;
        BtModificar.Visible = false;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        BtEliminar.Visible = false;
        TxtIP1.ReadOnly = false;
        TxtIP2.ReadOnly = false;
		Modificando = true;

        DListPosicion.Enabled = PermisoSegunPerfil;
        DListArranque.Enabled = PermisoSegunPerfil;
        IndexListBox1 = ListBox1.SelectedIndex;
        DDLRecorder2.Enabled = true;
        DDLRecorder1.Enabled = true;

        MostrarGestionErrores();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        //	EsconderMenu();
        string strSistema=string.Empty;
        string strIdTop = string.Empty;
        bool bTerminalEnSectorizacion = false;
        
        if (ListBox1.SelectedValue != "")
        {
            IndexListBox1 = ListBox1.SelectedIndex;
            Session["elemento"] = ListBox1.SelectedValue;
            strIdTop = ListBox1.SelectedValue;
            strSistema = (string)Session["idsistema"];

            if (ServicioCD40.TerminalOperadorAsignadoEnSectorizacion(strSistema, strIdTop))
            {
                bTerminalEnSectorizacion = true;
            }

            EliminarElemento();

            if (bTerminalEnSectorizacion)
            {
                //Se actualiza la sectorización por si el terminal estuviera asignado en alguna sectorizacion
                Session.Add("Sectorizando", true);
                ServicioCD40.BeginRegeneraSectorizaciones((string)Session["idsistema"], true, false, false, CallbackCompletado, null);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void EliminarElemento()
    {
        try
        {
			ServiciosCD40.Top n = new ServiciosCD40.Top();
            n.IdSistema = (string)Session["idsistema"];
            n.IdTop = (string)Session["elemento"];
			if (ServicioCD40.DeleteSQL(n) < 0) logDebugView.Warn("(TOP-EliminarElemento): No se ha podido eliminar la TOP");
            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
        }
        catch (Exception ex)
        {
            logDebugView.Error(string.Format("TOP-EliminarElemento. Error al eliminar el terminal idTop={0} sistema={0} ", (string)Session["idsistema"], (string)Session["elemento"]), ex);
        }
        MuestraDatos(DameDatos());
    }

    /// <summary>
    /// 
    /// </summary>
    private void MostrarGestionErrores()
    {
        RequiredFieldValidator1.Visible = true;
        RequiredFieldValidator2.Visible = true;
        //MVO 20170706: Se elimina la validación del campo IP2, al hacerse no visible
        //RequiredFieldValidator3.Visible = true;
        RegularExpressionValidator1.Visible = true;
        RegularExpressionValidator2.Visible = true;
        ValidationSummary1.Visible = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OcultarGestionErrores()
    {
        RequiredFieldValidator1.Visible = false;
        RequiredFieldValidator2.Visible = false;
        //MVO 20170706: Se elimina la validación del campo IP2 , al hacerse no visible
        //RequiredFieldValidator3.Visible = false;
        RegularExpressionValidator1.Visible = false;
        RegularExpressionValidator2.Visible = false;
        ValidationSummary1.Visible = false;
    }

    private void CargaDDLGrabadores()
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
        Session["idsistema"] = s.Value;

        ServiciosCD40.EquiposEU tifx = new ServiciosCD40.EquiposEU();
        string strFirstItemRecorder = string.Empty;

        strFirstItemRecorder = GetLocalResourceObject("DDLRecorderItem1").ToString();

        tifx.IdSistema = s.Value;
        tifx.TipoEquipo = (uint)ServiciosCD40.Tipo_Elemento_HW.TEH_GRABADOR;

        DDLRecorder1.Items.Clear();
        //Se añade el item 0 que indica que no se selecciona ningún grabador.
        //DDLRecorder1.Items.Add("< Ninguno >");
        DDLRecorder1.Items.Add(strFirstItemRecorder);
        DDLRecorder1.SelectedIndex = 0;
        DDLRecorder1.DataSource = ServicioCD40.DataSetSelectSQL(tifx);
        DDLRecorder1.DataTextField = "IdEquipos";
        DDLRecorder1.DataValueField = "IdEquipos";
        DDLRecorder1.DataBind();

        DDLRecorder2.Items.Clear();
        //Se añade en el item 0, DDLRecorder2.Items.Add("< Ninguno >");
        DDLRecorder2.Items.Add(strFirstItemRecorder);

        DDLRecorder2.SelectedIndex = 0;
        DDLRecorder2.DataSource = ServicioCD40.DataSetSelectSQL(tifx);
        DDLRecorder2.DataTextField = "IdEquipos";
        DDLRecorder2.DataValueField = "IdEquipos";
        DDLRecorder2.DataBind();
    }

    // Respuesta a la regeneración de las sectorizaciones
    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    private void OnCallBackCompleted(IAsyncResult result)
    {
        try
        {
            int retorno = ServicioCD40.EndRegeneraSectorizaciones(result);
            Session.Add("Sectorizando", false);
        }
        catch (System.Web.Services.Protocols.SoapException soapException)
        {
            logDebugView.Error("(TOP-OnCallBackCompleted): excepción al regenerarlaSectorizacion", soapException);
        }
    }

    //Devuelve true si ya existe algun elemento físico en el sistema con el mismo identificador. En caso contrario, false.
    private bool bIdentificadorAsignado(string strIdSistema, string strIdentificador)
    {
        bool bExiste = false;
        int iExiste = -1;

        //Si se está dando de alta un puesto de operador hay que comprobar 
        // que no existe un elemento físico (Pasarela, top, equipo externo o encaminamiento) con el mismo identificador 
        iExiste = ServicioCD40.CheckIdentificadorAsignado("EF", strIdSistema, strIdentificador);

        if (iExiste > 0)
            bExiste = true;
        else if (iExiste < 0)
        {
            System.Text.StringBuilder strMsgError = new System.Text.StringBuilder();
            strMsgError.AppendFormat("(Top-bIdentificadorAsignado): el servicio servicioCD40.CheckIdentificadorAsignado('EF', '{0}', '{1}') ha devuelto el codigo {2}", strIdSistema, strIdentificador, iExiste);
            logDebugView.Error(strMsgError.ToString());
            strMsgError.Clear();
        }

        return bExiste;
    }
}
