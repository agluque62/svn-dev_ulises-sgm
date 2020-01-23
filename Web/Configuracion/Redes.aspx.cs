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

public partial class Redes : PageBaseCD40.PageCD40	//	System.Web.UI.Page
{
    private const int MAX_REDES = 20;
    /// <summary>
    /// 
    /// </summary>
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
	private static int NumPaginaActiva = 0;

    private static uint iPrefijoRedElto = 0;

    //Almacena la lista de Recursos asociados a la red
    private static DataSet DSListaRecursos = null;


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
                Response.Redirect("~/Configuracion/Inicio.aspx?Permiso=NO", false);
				return;
			}

			PermisoSegunPerfil = perfil != "1";
		}

		if (!IsPostBack)
		{
            IndexListBox1 = -1;
            NumPaginaActiva = 0;
			BtNuevo.Visible = PermisoSegunPerfil;
			MuestraDatos(DameDatos());
			CargarInforme();

			ActualizaWebPadre(true);

            IBDatosGenerales.CssClass = "buttonImageSelected";
            IBRecursos.CssClass = "buttonImage";
            //IBDatosGenerales.ImageUrl = GetLocalResourceObject("IBDatosGeneralesResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuRedesDatosGeneralesSelected.JPG";
            //IBRecursos.ImageUrl = GetLocalResourceObject("IBRecursosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuRedesRecursosUnSelected.JPG";

            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            /**
             * AGL. 2012.06.19 ID.114
             * Solo accede a modificar parámetros el 'Tecnico 3'.
             * */
            DropDownList1.Enabled = PermisoSegunPerfil;
            /**/
		}
		else
		{
			MultiView1.ActiveViewIndex = NumPaginaActiva;
            //Si se ha recargado la página, las variables datos y la variable de session tienen valor nulo es porque 
            // si ha cambiado la sesión del servidor, bien por conmutación o reinicio
            //por lo que se va a la página de login
            if (datos == null || Session["idsistema"] == null)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);
            }
		}
    }
    /// <summary>
    /// 
    /// </summary>
	protected void CargarInforme()
	{
		LBImprimir.Attributes.Remove("onclick");
		string comando = "AbreVentana('../Informes/Report.aspx?Report=Redes.rpt');return false;";
		LBImprimir.Attributes.Add("onclick", comando);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.Redes t = new ServiciosCD40.Redes();
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
            logDebugView.Error("(Redes-DameDatos): ",ex);
        }
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    private void CargarPrefijosRed()
    {
        try
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            Session["idsistema"] = s.Value;

			DataSet d = ServicioCD40.PrefijosSinAsignarARedes(s.Value);
			DropDownList1.DataSource = d;
			DropDownList1.DataTextField = "IdPrefijo";
			DropDownList1.DataBind();
		}
        catch (Exception ex)
        {
            logDebugView.Error("(Redes-CargarPrefijosRed): ", ex);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nu"></param>
    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
		ListBox1.Items.Clear();

        if (nu!=null)
            for (int i = 0; i < nu.Length;i++)
                ListBox1.Items.Add(((ServiciosCD40.Redes)nu[i]).IdRed);

		if (ListBox1.Items.Count > 0)
		{
			ActualizaWebPadre(true);

            if (ListBox1.Items.FindByText(NewItem) != null)
            {
                ListBox1.Items.FindByText(NewItem).Selected = true;
                IndexListBox1 = ListBox1.SelectedIndex;
            }
            else
                ListBox1.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < ListBox1.Items.Count ? IndexListBox1 : 0;

			MostrarElemento();
			GeneraXmlParaInforme();
		}
		else
		{
			BtEliminar.Visible = BtModificar.Visible = false;
		}

        NewItem = string.Empty;
    }
    /// <summary>
    /// 
    /// </summary>
	private void GeneraXmlParaInforme()
	{
		ServiciosCD40.Redes t = new ServiciosCD40.Redes();
		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
		t.IdSistema = s.Value;

		DataSet d = ServicioCD40.DataSetSelectSQL(t);
		d.WriteXml(Server.MapPath("~/Informes/Redes.xml"));
		ServicioCD40.RecursosDeUnaRed((string)Session["idsistema"], null).WriteXml(Server.MapPath("~/Informes/RecursosEnRedes.xml"));
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
            //BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarRed"), ListBox1.SelectedValue);
            //BtEliminar.Visible = PermisoSegunPerfil;	
            MostrarElemento();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void MostrarElemento()
    {
        MostrarMenu();
        TxtIdRed.ReadOnly = true;
        string strIdSistema = (string)Session["idsistema"];

        if (null != datos)
        {
            for (int i = 0; i < datos.Length; i++)
            {
                if (String.Compare((((ServiciosCD40.Redes)datos[i]).IdRed), ListBox1.SelectedValue) == 0)
                {
                    //BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarRed"), ListBox1.SelectedValue);

                    TxtIdRed.Text = ((ServiciosCD40.Redes)datos[i]).IdRed;
                    DropDownList1.Items.Clear();
                    DropDownList1.Items.Add(((ServiciosCD40.Redes)datos[i]).Prefijo.ToString());
                    DropDownList1.Text = ((ServiciosCD40.Redes)datos[i]).Prefijo.ToString();

                    CargaGridViewRecursos(strIdSistema,((ServiciosCD40.Redes)datos[i]).IdRed);

                    //Si el prefijo es 3 (RED ATS) no se permite eliminar ni modificar
                    iPrefijoRedElto = ((ServiciosCD40.Redes)datos[i]).Prefijo;

                    if (iPrefijoRedElto != 3)
                    {
                        BtEliminar.Visible = true;
                        BtModificar.Visible = true;
                    }
                    else
                    {
                        BtEliminar.Visible = false;
                        BtModificar.Visible = false;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idRed"></param>
    private void CargaGridViewRecursos(string strIdSistema,string idRed)
    {
        if (DSListaRecursos != null)
            DSListaRecursos.Clear();

        DSListaRecursos = ServicioCD40.RecursosDeUnaRed(strIdSistema, idRed);

        if (DSListaRecursos != null)
        {
            GVRecursos.DataSource = DSListaRecursos;
            GVRecursos.DataBind();
        }

        if (GVRecursos.Rows.Count > 0)
            BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "NoConRecursosEliminarRed"), ListBox1.SelectedValue);
        else
            BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarRed"), ListBox1.SelectedValue);
    }

    /// <summary>
    /// 
    /// </summary>
    private void MostrarMenu()
    {
        TxtIdRed.Text = "";        
        TxtIdRed.Visible = true;
        Label1.Visible = true;
        Label2.Visible = true;
        DropDownList1.Visible = true;
		BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
    }
    /// <summary>
    /// 
    /// </summary>
    private void EsconderMenu()
    {
        TxtIdRed.Text = "";
        TxtIdRed.Visible = false;
        Label1.Visible = false;
        Label2.Visible = false;
        DropDownList1.Visible = false;
		BtModificar.Visible = BtEliminar.Visible = false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        string strIdSistema = (string)Session["idsistema"];
        string strIdRed = ListBox1.SelectedValue;

        if (!string.IsNullOrEmpty(strIdRed) && !string.IsNullOrEmpty(strIdSistema))
        {
            uint idPrefijoDat= Convert.ToUInt32(DropDownList1.SelectedValue);

            if (ServicioCD40.Red_ConDestinosTlf(strIdSistema, idPrefijoDat))
            {
                string strMsg = string.Empty;

                if (null != GetLocalResourceObject("MsgBorrar_RedConDestinosTelefonia"))
                {
                    strMsg = string.Format(GetLocalResourceObject("MsgBorrar_RedConDestinosTelefonia").ToString(), strIdRed);
                }
                else
                    strMsg = string.Format("En el sistema existen destinos de telefonía que utilizan la red {0}. Antes de borrar la red, debe eliminar estos destinos", strIdRed);

                cMsg.alert(strMsg);
                return;

            }
            else
            {
                //Si la red no tiene recursos asociados, se elimina
                if (GVRecursos.Rows.Count == 0)
                {
                    Session["prefijo"] = DropDownList1.SelectedValue;
                    IndexListBox1 = ListBox1.SelectedIndex;
                    EliminarElemento(strIdSistema, ListBox1.SelectedValue);
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void EliminarElemento(string strIdSistema,string strIdRed)
    {
        try
        {
			ServiciosCD40.Redes n = new ServiciosCD40.Redes();
            n.IdSistema = strIdSistema;
            n.IdRed = strIdRed;
			if (ServicioCD40.DeleteSQL(n) < 0)
                logDebugView.Warn("(Redes-EliminarElemento): No se ha podido eliminar la red " + strIdRed);
            else
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    switch (sincro.BajaRed(Convert.ToInt32(Session["prefijo"]), n.IdRed))
                    {
                        case 127:
                            string msg = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado") + "\\n\\n" +
                                (string)GetGlobalResourceObject("Espaniol", "Cod127");
                            cMsg.alert(msg);
                            break;
                        case 128:
                            string msg1 = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado") + "\\n\\n" +
                                (string)GetGlobalResourceObject("Espaniol", "Cod128");
                            cMsg.alert(msg1);
                            break;
                        default:
                            break;
                    }
                }
                else
                    cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
            }            
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Redes-EliminarElemento): ", ex);
        }
        ListBox1.Items.Clear();
        MuestraDatos(DameDatos());
		BtNuevo.Visible = PermisoSegunPerfil;
		BtEliminar.Visible = BtModificar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        GuardarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AceptarCambios"), "aceptparam");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
        // cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");
    }
    /// <summary>
    /// 
    /// </summary>
    private void GuardarCambios()
    {
        string strIdSistema = (string)Session["idsistema"];


        if (!string.IsNullOrEmpty(strIdSistema))
        {

            try
            {
                ServiciosCD40.Redes n = new ServiciosCD40.Redes();
                n.IdSistema = strIdSistema;
                n.IdRed = TxtIdRed.Text;
                n.Prefijo = UInt16.Parse(DropDownList1.Text);

                NewItem = TxtIdRed.Text;

                //Si se va a insertar un nuevo registro
                if (!TxtIdRed.ReadOnly)
                {
                    if (ServicioCD40.InsertSQL(n) < 0)
                        logDebugView.Warn("(Redes-GuardarCambios): No se ha podido guardar la red.");
                    else
                    {
                        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                        {
                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                            switch (sincro.AltaRed((int)n.Prefijo, n.IdRed))
                            {
                                case 132:
                                    cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod132"), n.IdRed));
                                    break;
                                case 133:
                                    cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod133"), n.IdRed));
                                    break;
                                case 134:
                                    cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod134"), n.IdRed));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    //Si es una actualización, comprobamos si se ha cambiado el prefijo

                    if (n.Prefijo != iPrefijoRedElto)
                    {
                        //Se verifica que existe algún destino de telefonía con el prefijo asignado
                        //Si existe, se indica que se debe eliminar el destino para poder cambiar
                        //el prefijo de la red
                        if (ServicioCD40.Red_ConDestinosTlf(n.IdSistema, iPrefijoRedElto))
                        {
                            string strMsg = string.Empty;

                            if (null != GetLocalResourceObject("MsgMod_PrefRedConDestinosTelefonia"))
                            {
                                strMsg = string.Format(GetLocalResourceObject("MsgMod_PrefRedConDestinosTelefonia").ToString(), n.IdRed);
                            }
                            else
                                strMsg = string.Format("En el sistema existen destinos de telefonía que utilizan la red {0}. Antes de modificar el prefijo, debe eliminar estos destinos", n.IdRed);

                            cMsg.alert(strMsg);
                            return;
                        }
                    }
                    IndexListBox1 = ListBox1.SelectedIndex;

                    if (ServicioCD40.UpdateSQL(n) < 0)
                        logDebugView.Warn("(Redes-GuardarCambios): No se ha podido actualizar la red.");
                }
            }
            catch (Exception ex)
            {
                logDebugView.Error("(Redes-GuardarCambios): ", ex);
            }

            BtAceptar.Visible = false;
            BtCancelar.Visible = false;
            //TxtIdRed.Visible = false;
            ListBox1.Enabled = true;
            BtNuevo.Visible = PermisoSegunPerfil;
            BtEliminar.Visible = BtModificar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
            //Label1.Visible = false;
            //Label2.Visible = false;
            //DropDownList1.Visible = false;
            ListBox1.Enabled = true;
            ListBox1.Items.Clear();
            MuestraDatos(DameDatos());
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
		BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        //TxtIdRed.Visible = false;
        ListBox1.Enabled = true;
        BtNuevo.Visible = PermisoSegunPerfil;
        BtEliminar.Visible = BtModificar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
        //Label1.Visible = false;
        //Label2.Visible = false;
        //DropDownList1.Visible = false;
        if (ListBox1.Items.Count > 0)
            ListBox1.SelectedIndex = IndexListBox1 != -1 ? IndexListBox1 : 0;

		MuestraDatos(DameDatos());
		ListBox1.Enabled = true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtNuevo_Click(object sender, EventArgs e)
    {
        if (ListBox1.Items.Count < MAX_REDES)
        {
            CargarPrefijosRed();
            if (DropDownList1.Items.Count > 0)
            {
                BtModificar.Visible = false;
                BtAceptar.Visible = true;
                BtCancelar.Visible = true;
                TxtIdRed.ReadOnly = false;
                TxtIdRed.Visible = true;
                ListBox1.Enabled = false;
                BtNuevo.Visible = false;
                BtEliminar.Visible = false;
                Label1.Visible = true;
                Label2.Visible = true;
                TxtIdRed.Text = "";
                DropDownList1.Visible = true;
                IndexListBox1 = ListBox1.SelectedIndex;
                
                if (null != GVRecursos)
                {
                    GVRecursos.DataSource = null;
                    GVRecursos.DataBind();
                }
                MultiView1.ActiveViewIndex = NumPaginaActiva = 0;
            }
            else
            {
                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "NoMasRedes"));
                if (ListBox1.Items.Count > 0)
                    ListBox1.SelectedIndex = 0;

                MostrarElemento();
            }
        }
        else
        {
            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "NoMasRedes"));
        }
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtModificar_Click(object sender, EventArgs e)
    {
        string pr = DropDownList1.Text;
		BtNuevo.Visible = false;
		BtEliminar.Visible = false;
        BtModificar.Visible = false;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        TxtIdRed.ReadOnly = true;
        CargarPrefijosRed();
		DropDownList1.Items.Add(pr);
        DropDownList1.Text = pr;
        IndexListBox1 = ListBox1.SelectedIndex;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void OnButton_Click(object sender, EventArgs e)
	{
		Button ibSelected = (Button)sender;

		switch (ibSelected.ID)
		{
			case "IBDatosGenerales":
                IBDatosGenerales.CssClass = "buttonImageSelected";
                IBRecursos.CssClass = "buttonImage";
                //IBDatosGenerales.ImageUrl = GetLocalResourceObject("IBDatosGeneralesResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuRedesDatosGeneralesSelected.JPG";
                //IBRecursos.ImageUrl = GetLocalResourceObject("IBRecursosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuRedesRecursosUnSelected.JPG";
				MultiView1.ActiveViewIndex = NumPaginaActiva = 0;
				break;
			case "IBRecursos":
                IBDatosGenerales.CssClass = "buttonImage";
                IBRecursos.CssClass = "buttonImageSelected";
                //IBDatosGenerales.ImageUrl = GetLocalResourceObject("IBDatosGeneralesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuRedesDatosGeneralesSelected.JPG";
                //IBRecursos.ImageUrl = GetLocalResourceObject("IBRecursosResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuRedesRecursosUnSelected.JPG";
				MultiView1.ActiveViewIndex = NumPaginaActiva = 1;
				break;
		}
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void GViewRecursos_OnSelectedIndexChanging(object sender, GridViewPageEventArgs e)
	{
		GVRecursos.PageIndex = e.NewPageIndex;
        GVRecursos.DataSource = DSListaRecursos;
		GVRecursos.DataBind();
	}

    protected void GViewRecursos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Se muestra el texto del tipo de equipo
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Se procesa el texto de cada registro para componer el valor del rango
            Label LbValor = (Label)e.Row.FindControl("LbTipo");
            Label LbValorTipo = (Label)e.Row.FindControl("LbValorTipo");

            if (LbValorTipo != null && LbValor!=null)
            {
                if (DDLTipoEquipoRecurso.Items.FindByValue(LbValor.Text) != null)
                {
                    LbValorTipo.Text = DDLTipoEquipoRecurso.Items.FindByValue(LbValor.Text).Text;
                }
            }
        }
    }

}
