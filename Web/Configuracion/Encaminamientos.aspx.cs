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

/// <summary>
/// 
/// </summary>
public class InfoRango
{
    public int tipo;//0: Operador  1: Privilegiado
    public string inicial;
    public string final;
    public string numero;
    public string idRed;
    
    public InfoRango()
    {
        tipo = 0;
        inicial = "";
        final = "";
        numero = "";
        idRed = "";
    }
}

/// <summary>
/// 
/// </summary>
public class InfoRuta
{
    public string idRuta;
    public int tipo;//0: Directa   1: Alternativa
    public List<string> troncales;

    public InfoRuta()
    {
        idRuta = "";
        tipo = 0;
        troncales = new List<string>();
    }
}

/// <summary>
/// 
/// </summary>
public class InfoEncaminamiento
{
    public string idEncaminamiento;
    public bool centralPropia;
    public bool throwswhitching;
	public string numTest;
    public List<InfoRango> rangos;
    public List<InfoRuta> rutas;

    public InfoEncaminamiento()
    {
		numTest = string.Empty;
        idEncaminamiento = "";
        centralPropia = false;
        throwswhitching = false;
        rangos = new List<InfoRango>();
        rutas = new List<InfoRuta>();
    }
}

/// <summary>
/// 
/// </summary>
public partial class Encaminamientos : PageBaseCD40.PageCD40//, System.Web.UI.Page
{
    /// <summary>
    /// 
    /// </summary>
	private static bool EncaminamientoDadoDeAlta = false;
    /// <summary>
    /// 
    /// </summary>
	private static int IndiceCentralPropia = -1;
    /// <summary>
    /// 
    /// </summary>
	private static bool Modificando=false;
    /// <summary>
    /// 
    /// </summary>
	private static bool HayCentralPropia = false;
    /// <summary>
    /// 
    /// </summary>
	private static bool HayRutaDirecta = false;
    /// <summary>
    /// 
    /// </summary>
    private static InfoEncaminamiento InfoEnc;
    /// <summary>
    /// 
    /// </summary>
	private static KeyValueConfigurationElement s;
    /// <summary>
    /// 
    /// </summary>
	private static DataSet DSRangos, DSRutas;
    /// <summary>
    /// 
    /// </summary>
	//private Mensajes.msgBox cMsg;
    /// <summary>
    /// 
    /// </summary>
    private static ServiciosCD40.Tablas[] datos;
    /// <summary>
    /// 
    /// </summary>
    private static ILog _logDebugView;
    /// <summary>
    /// 
    /// </summary>
	private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();
    /// <summary>
    /// 
    /// </summary>
	static bool PermisoSegunPerfil;
    /// <summary>
    /// 
    /// </summary>
	private static int NumPaginaActiva = 0;

    private static bool bEsCentralIP=false;

    private static bool bCentralConDestinoATS_LC = false;

    //202006 JOI Error #4066
//    private static bool bModificacionAsignaciónTroncales = false;
//    private static List<string> LTroncalesIniciales = new List<string>();
    
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

    private static int SelectedRangoIndex = -1;

    private const int INCIDENCIA_CNF = 114; //Identificador de incidencia de configuración

    private static bool bUpdateRango = false;

    //20200928 JOI #4599
    private static bool bSBCEnaire = false;
    private static string sSCV_DEP = "";
    private static string sSCV_DEP_P = "";

    private static Ulises5000Configuration.ToolsUlises5000Section UlisesToolsVersion;

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
        //20200928 JOI #4599
        Ulises5000Configuration.ToolsUlises5000Section ulisesVersion = Ulises5000Configuration.ToolsUlises5000Section.Instance;
        UlisesToolsVersion = ulisesVersion;
        bSBCEnaire = UlisesToolsVersion.Version == 3 ? true : false;

		if (!IsPostBack || RecargarDatosEnPagina)
        {
            IBCentralATS.CssClass = "buttonImageSelected";
            IBRangos.CssClass = "buttonImage";
            IBRutas.CssClass = "buttonImage";
            MultiView1.ActiveViewIndex = 0;
            Panel1.Width = 680;
            Panel1.Height = 355;
            IndexListBox1 = -1;

            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");
            
            BtNuevo.Visible = PermisoSegunPerfil;

			NumPaginaActiva = 0;

			RecargarDatosEnPagina = false;
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			s = config.AppSettings.Settings["Sistema"];

			CargarPrefijos();
			MuestraDatos(DameDatos());

			if (Session["Central"] != null)
            {
                ListBox1.SelectedIndex = ListBox1.Items.IndexOf(new ListItem((string)Session["Central"]));
                if (ListBox1.SelectedIndex != -1)
                {
                    MostrarElemento();
                    BtEliminar.Visible = BtModificar.Visible = PermisoSegunPerfil;
                }
				else
					ObtenerRangos();
            }

            bUpdateRango = false;
            //202006 JOI Error #4066
//            bModificacionAsignaciónTroncales = false;
//            LTroncalesIniciales.Clear();
            //202006 JOI Error #4066 FIN

		
		}
        else
        {
			//if (FinalTransaccion)
			//{
			//    FinalTransaccion = false;
			//    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "FinTransaccion"), "fintransaccion");
			//}
			//else
			{
				MultiView1.ActiveViewIndex = NumPaginaActiva;

                if (null == s || Session["idsistema"]==null)
                {
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                    s = config.AppSettings.Settings["Sistema"];

                    //Se recarga la variable de sesión para que las consultas se realicen correctamente y no devuelvan 
                    // datos duplicados o asociados a otros encaminamiento
                    Session["idsistema"] = s.Value;
                }

                if (datos== null)
                     DameDatos();

                //if (Request.Form["eliminaelemento"] == "1")   //El usuario elige eliminar el elemento 
                //{
                //    Request.Form["eliminaelemento"].Replace("1", "0");

                //    EliminarElemento();
                //}
                //if (Request.Form["cancelparam"] == "1")   //El usuario elige no guardar los cambios 
                //{
                //    Request.Form["cancelparam"].Replace("1", "0");

                //    CancelarCambios();
                //}
                //if (Request.Form["aceptparam"] == "1")   //El usuario elige guardar los cambios
                //{
                //    Request.Form["aceptparam"].Replace("1", "0");

                //    GuardarCambios();
                //}

			}
		}

		ActualizaWebPadre(HayCentralPropia);
	}


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.Encaminamientos t = new ServiciosCD40.Encaminamientos();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;

            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            datos = d;
            return d;
        }
        catch (Exception e)
        {
            logDebugView.Error("(Encaminamientos-DameDatos): ",e);
        }
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nu"></param>
    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
		HayCentralPropia = false;
		ListBox1.Items.Clear();
		LimpiaDatos();

        if (nu!=null)
			for (int i = 0; i < nu.Length; i++)
			{
				ListBox1.Items.Add(((ServiciosCD40.Encaminamientos)nu[i]).Central);
				
				// Comprobar si ya hay una central propia. Sólo puede haber una en el sistema
				HayCentralPropia |= ((ServiciosCD40.Encaminamientos)nu[i]).CentralPropia;
				if (((ServiciosCD40.Encaminamientos)nu[i]).CentralPropia)
					IndiceCentralPropia = i;
			}

		LBMiCentral.Visible = HayCentralPropia;

		if (ListBox1.Items.Count > 0)
		{
            if (ListBox1.Items.FindByText(NewItem) != null)
            {
                ListBox1.Items.FindByText(NewItem).Selected = true;
                IndexListBox1 = ListBox1.SelectedIndex;
                NewItem = string.Empty;
            }
            else
                ListBox1.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < ListBox1.Items.Count ? IndexListBox1 : 0;
            
            MostrarElemento();

			BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil;
		}
		else
		{
			BtModificar.Visible = BtEliminar.Visible = false;

			ObtenerRangos();
		}


		// ListTroncalesLibres.Items.Clear();
	}



    /// <summary>
    /// 
    /// </summary>
	private void ObtenerRangos()
	{
		try
		{
			GViewRangos.DataSource = DSRangos = ServicioCD40.RangosConIdRed(s.Value,TxtCentral.Text);
			GViewRangos.DataBind();
		}
		catch (Exception ex)
		{
			logDebugView.Error("(Rangos-DameDatos): ", ex);
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	private DataSet ObtenerRutas(string rutaSeleccionada)
	{
		ServiciosCD40.Rutas r = new ServiciosCD40.Rutas();
        r.IdSistema = (string)Session["idsistema"];
		r.Central = TxtCentral.Text;

		DDLRutas.Items.Clear();
		DDLRutas.DataSource = DSRutas = ServicioCD40.DataSetSelectSQL(r);
        if (null!=DSRutas && DSRutas.Tables.Count > 0)
        {
            DDLRutas.DataTextField = "IdRuta";
            DDLRutas.DataBind();
        }

		HayRutaDirecta = TieneRutaDirecta();

		ListTroncales.Items.Clear();

		if (null!=DSRutas && DSRutas.Tables.Count > 0 && DSRutas.Tables[0].Rows.Count > 0)
		{
            if (string.IsNullOrEmpty(rutaSeleccionada))
                DDLRutas.SelectedIndex = 0;
            else
                DDLRutas.SelectedValue = rutaSeleccionada;

			DListTipo.SelectedValue = (string)(DSRutas.Tables[0].Rows[DDLRutas.SelectedIndex].ItemArray[3]);
			CargarTroncalesRuta((string)(DSRutas.Tables[0].Rows[DDLRutas.SelectedIndex].ItemArray[1]));
			CargarTroncalesSinAsignar();
		}

		return DSRutas;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	private bool TieneRutaDirecta()
	{
		bool hay = false;

        if (null != DSRutas && DSRutas.Tables.Count > 0)
        {
            foreach (DataRow dr in DSRutas.Tables[0].Rows)
            {
                hay |= ((string)dr[3]) == "D";
                if (hay)
                    break;
            }
        }

		return hay;
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
            BtEliminar.Visible = PermisoSegunPerfil;
            //ActualizaArbol();

            MostrarElemento();
        }
		else
			ObtenerRangos();
	}

    /// <summary>
    /// 
    /// </summary>
    private void MostrarElemento()
    {
        if (null != datos)
        {
            for (int i = 0; i < datos.Length; i++)
            {
                if (String.Compare((((ServiciosCD40.Encaminamientos)datos[i]).Central), ListBox1.SelectedValue) == 0)
                {
                    string sTexto = string.Empty;

                    TxtCentral.Text = ((ServiciosCD40.Encaminamientos)datos[i]).Central;
                    CentralPropia.Checked = ((ServiciosCD40.Encaminamientos)datos[i]).CentralPropia;
                    Throwswitching.Checked = ((ServiciosCD40.Encaminamientos)datos[i]).Throwswitching;
                    LblNumTest.Visible = TBNumTest.Visible = CentralPropia.Checked;
                    TBNumTest.Text = ((ServiciosCD40.Encaminamientos)datos[i]).NumTest;
                    CBCentralIp.Checked = ((ServiciosCD40.Encaminamientos)datos[i]).CentralIp;

                    IBRutaAbajo.Visible = IBRutaArriba.Visible = LblRutas.Visible = LBRutas.Visible = IBRutas.Visible = !CentralPropia.Checked;

                    //Se almacena el valor original del campo CentralIP para saber si se ha modificado
                    bEsCentralIP = CBCentralIp.Checked;

                    //Si es una centra IP se hace visible la tabla que visualiza la tabla de direcciones del SCV IP y se habilitan los controles
                    //y validaciones de la tabla de direcciones
                    TblCentralIp.Visible = bEsCentralIP;

                    if (bEsCentralIP)
                    {
                        //Se recuperan los valores de las IP configuradas del SCV IP
                        MostrarIpsCentral(CentralPropia.Checked);
                    }
                    else
                    {
                        //Se limpian los campos de direcciones IP
                        TbIp1.Text = TbIp2.Text = TbIp3.Text = string.Empty;
                        TbSrvPresIp1.Text = TbSrvPresIp2.Text = TbSrvPresIp3.Text = string.Empty;
                    }

                    if (bEsCentralIP && ServicioCD40.ExisteDestinoATSenPanelLC_AsociadoCentralIP(((ServiciosCD40.Encaminamientos)datos[i]).IdSistema, TxtCentral.Text))
                    {
                        sTexto = string.Format((string)GetGlobalResourceObject("Espaniol", "AvisoBorrarCentralIP_DestinosATS_LC"), ((ServiciosCD40.Encaminamientos)datos[i]).Central);
                        bCentralConDestinoATS_LC = true;
                    }
                    else
                    {
                        sTexto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarEncaminamiento"), ListBox1.SelectedValue);
                        bCentralConDestinoATS_LC = false;
                    }

                    BtEliminar_ConfirmButtonExtender.ConfirmText = sTexto;
                    BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");


                    if (CentralPropia.Checked && NumPaginaActiva != 0)
                    {
                        IBCentralATS.CssClass = "buttonImageSelected";
                        IBRangos.CssClass = "buttonImage";
                        IBRutas.CssClass = "buttonImage";
                        MultiView1.ActiveViewIndex = NumPaginaActiva = 0;
                        Panel1.Width = 680;
                        //Panel1.Height = 355;
                    }
                    else if (bUpdateRango && NumPaginaActiva == 1)
                    {
                        bUpdateRango = false;
                        Panel1.Width = 560;
                    }

                    break;
                }
            }

            ListTroncalesLibres.Items.Clear();
            ActualizaArbol();
            ObtenerRangos();

            if (LBRutas.Items.Count > 0)
                LBRutas.Items.Clear();

            LBRutas.DataTextField = "IdRuta";
            LBRutas.DataSource = ObtenerRutas(null);
            LBRutas.DataBind();
        }
    }

    private void MostrarIpsCentral(bool bCentralPropia)
    {
        ServiciosCD40.EquiposEU e = new ServiciosCD40.EquiposEU();
        e.IdSistema = (string)Session["idsistema"];
        e.IdEquipos = TxtCentral.Text;

        //Se limpian los campos y se recuperan los valores
        TbIp1.Text = TbIp2.Text = TbIp3.Text=string.Empty;
        TbSrvPresIp1.Text = TbSrvPresIp2.Text = TbSrvPresIp3.Text = string.Empty;
        ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(e);
        if (d != null && d.Length > 0)
        {
            //20200928 JOI #4599
            sSCV_DEP = sSCV_DEP_P = string.Empty; ;
            // Se mueven los valores de Red1 a Red3 para su gestión en pagina, al guardarse se volveran a llevar a Red1.
            if (bCentralPropia && bSBCEnaire)
            {
                TbIp3.Text = ((ServiciosCD40.EquiposEU)d[0]).IpRed1;
                TbSrvPresIp3.Text = ((ServiciosCD40.EquiposEU)d[0]).SrvPresenciaIpRed1;
            }
            else
            {
                TbIp1.Text = ((ServiciosCD40.EquiposEU)d[0]).IpRed1;
                TbSrvPresIp1.Text = ((ServiciosCD40.EquiposEU)d[0]).SrvPresenciaIpRed1;
            }
            //Para la Central propia sólo se permite configurar el servidor proxy y de presencia principal
            //Si no es un SCV IP no propio, se pueden configurar también los servidores proxy y de presencia alternativos
            if (!bCentralPropia)
            {
                //Para la Central propia sólo se permite configurar el servidor proxy y de presencia principal
                TbIp2.Text = ((ServiciosCD40.EquiposEU)d[0]).IpRed2;
                TbIp3.Text = ((ServiciosCD40.EquiposEU)d[0]).IpRed3;
                TbSrvPresIp2.Text = ((ServiciosCD40.EquiposEU)d[0]).SrvPresenciaIpRed2;
                TbSrvPresIp3.Text = ((ServiciosCD40.EquiposEU)d[0]).SrvPresenciaIpRed3;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        string strNombreCentral = ListBox1.SelectedValue;
        string strSistema = (string)Session["idsistema"];

        if (!string.IsNullOrEmpty(strNombreCentral))
        {
            //Si se elimina el registro correspondietne al SCV propio que es SCV IP
            //Se verifica si el SCV está asignado como equipo externo a algún recurso de telefonía
            if (CentralPropia.Checked && CBCentralIp.Checked)
            {
                if (bCentralConRecursoTlfAsignado(strSistema,strNombreCentral))
                {
                    string strMsg = string.Empty;

                    if (null != GetLocalResourceObject("ErrorBorrarIdCentralPropiaConRecursosTf"))
                    {
                        strMsg = string.Format(GetLocalResourceObject("ErrorBorrarIdCentralPropiaConRecursosTf").ToString(), strNombreCentral);
                    }
                    else
                    {
                        strMsg = string.Format("En el sistema, existen recursos de telefonía que tienen asignado el SCV IP {0} como equipo externo. Antes eliminar el SCV debe liberarlo.", strNombreCentral);
                    }
                    cMsg.alert(strMsg);
                    return;
                }
            }
            
            IndexListBox1 = ListBox1.SelectedIndex;
            Session["elemento"] = strNombreCentral;
            EliminarElemento(strSistema, strNombreCentral);
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void EliminarElemento(string strIdSistema, string strNombreCentral)
    {
        try
        {
			ServiciosCD40.Encaminamientos n = new ServiciosCD40.Encaminamientos();
            n.IdSistema = strIdSistema;
            n.Central = strNombreCentral;

			if (ServicioCD40.DeleteSQL(n) < 0)
			{
				logDebugView.Warn("(Encaminamientos-EliminarElemento): No se ha podido eliminar el encaminamiento");
				return;
			}
			else
			{
                if (bCentralConDestinoATS_LC)
                {
                    //Si existen destinos ATS en el rango de la central IP y registra un evento informando de la situación

                    logDebugView.Info("(Encaminamientos-EliminarElemento): El usuario ha eliminado la central IP " + n.Central + ". En el sistema, existe algún sector con destinos ATS configurados en el panel de línea caliente que utilizan dicha central, que dejarán de funcionar");
                    
                    string []  arrAux= new string [1];
                    string sTexto = string.Empty;

                    sTexto =(string)GetGlobalResourceObject("Espaniol", "CentralIPEliminada_DestinosATS_LC");

                    if (null==sTexto)
                    {
                        sTexto = "El usuario ha eliminado la central IP {0} y en el sistema, existe algún sector con destinos ATS configurados en el panel de línea caliente que utilizan dicha central, que dejarán de funcionar";
                    }

                    sTexto = string.Format(sTexto, n.Central);
                    arrAux[0] = sTexto;
                    ServicioCD40.GeneraIncidenciaConfiguracion(n.IdSistema, INCIDENCIA_CNF, arrAux);

                }
                //MVO: si el encaminamiento está definido como una central IP, se borra el registro correspondiente de la tabla equiposeu 
                if (CBCentralIp.Checked)
                    BorraCentralComoEquipoExterno(n.IdSistema,n.Central);

				Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
				KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
				if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
				{
					SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
					switch (sincro.BajaEncaminamiento(n.Central))
					{
						case 127:
							cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod127"));
							break;
						case 128:
							cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod128"));
							break;
						default:
							break;
					}
				}
			}
            
            ListBox1.Items.Clear();

            MuestraDatos(DameDatos());
		
			cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
		}
        catch (Exception e)
        {
            logDebugView.Error("(Encaminamientos-EliminarElemento): ", e);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtNuevoRango_Click(object sender, EventArgs e)
    {
		Panel1.Width = 740;

        if (!string.IsNullOrEmpty(LError.Text))
            LError.Text = string.Empty;

		DListTipoRango.SelectedIndex = 0;
		TxtBInicial.Text = TxtBFinal.Text = "";
		TxtAbonado.Text = "";
		if (DListPrefijo.Items.Count > 0)
		{
			TxtAbonado.Enabled = true;
			DListPrefijo.Enabled = true;
			DListPrefijo.SelectedIndex = 0;
		}
		else
		{
			TxtAbonado.Enabled = false;
			DListPrefijo.Enabled = false;
		}

		PanelRangos.Visible = true;
        bUpdateRango = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtAceptar_Click(object sender, EventArgs e)
	{
        string strNombreCentral = string.Empty;
        string strSistema = (string)Session["idsistema"];
        LError.Text = string.Empty;

        //Se comprueba si el identificador de la central está informado
        if (!string.IsNullOrWhiteSpace(TxtCentral.Text))
        {
            strNombreCentral = TxtCentral.Text;

            //Si es un alta
            if (!Modificando && !EncaminamientoDadoDeAlta)
            {
                //Se comprueba que no exista otra central con el mismo nombre
                if (bExisteCentralConMismoNombre(strSistema, strNombreCentral))
                {
                    LError.Text = GetLocalResourceObject("REV_CentralExiste") != null ? GetLocalResourceObject("REV_CentralExiste").ToString() : "En el sistema, ya existe un SCV con el mismo identificador.";
                    return;
                }
            }

           //Se comprueba si al menos se ha configurado un rango
            if (DSRangos != null && DSRangos.Tables.Count > 0 && DSRangos.Tables[0].Rows.Count > 0)
            {
                //Si estamos modificando el registro correspondiente al SCV propio que es SCV IP
                //Y se ha quitado el check de central Propia o el check de SCV IP, se verifica si el SCV está asignado como equipo externo a algún recurso de telefonía
                if (IndexListBox1 == IndiceCentralPropia && IndexListBox1 != -1 &&
                   ((false == CentralPropia.Checked && CBCentralIp.Checked) || (bEsCentralIP && !CBCentralIp.Checked)))
                {
                    if (bCentralConRecursoTlfAsignado(strSistema,strNombreCentral))
                    {
                        string strMsg = string.Empty;

                        if (null != GetLocalResourceObject("ErrorEncaminamientoIdCentralPropiaConRecursosTf"))
                        {
                            strMsg = string.Format(GetLocalResourceObject("ErrorEncaminamientoIdCentralPropiaConRecursosTf").ToString(), strNombreCentral);
                        }
                        else
                        {
                            strMsg = string.Format("En el sistema, existen recursos de telefonía que tienen asignado el SCV IP {0} como equipo externo. Antes de quitar el indicador de Datos propios, debe liberar el equipo externo en los recursos.", strNombreCentral);
                        }
                        cMsg.alert(strMsg);
                        return;
                    }
                }

                if (!CentralPropia.Checked || (CentralPropia.Checked && NumTestValido(TBNumTest.Text)))
                {
                    if (CBCentralIp.Checked)
                    {
                        string strMsgError = string.Empty;
                        //Se comprueban si las direcciones IP de los servidores Proxy y de presencia son correctas
                        //20200928 JOI #4599
                        if (!bSBCEnaire)
                        {
                            if (!bDireccionesCentralIPValidas(strSistema, strNombreCentral, ref strMsgError))
                            {
                                LError.Text = strMsgError;
                                return;
                            }
                        }
                        else
                        {
                            if (!bDireccionesCentralIPValidasSBC(strSistema, strNombreCentral, ref strMsgError, CentralPropia.Checked))
                            {
                                LError.Text = strMsgError;
                                return;
                            }
                        }
                    }
                    else
                    {

                        //Si no es un SCV IP y no se trata de la central propia, debe tener configurado al menos una ruta
                        if (!CentralPropia.Checked  && (DSRutas == null || DSRutas.Tables.Count <= 0 || DSRutas.Tables[0].Rows.Count <= 0))
                        {
                            LError.Text = GetLocalResourceObject("REV_RutaCentral") != null ? GetLocalResourceObject("REV_RutaCentral").ToString() : "El SCV debe tener configurado una ruta.";
                            return;
                        }
                    }

                    //Si es un alta
                    if (!Modificando && !EncaminamientoDadoDeAlta)
                    {
                        if (bExisteEquipoExternoConNombreCentral(strSistema, strNombreCentral))
                        {
                            //No se puede dar de alta un encaminamiento con el mismo nombre que un equipo externo ya existente
                            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ErrorEncaminamientoIdCentral_EU_Existente"));
                        }
                        else
                        {
                            GuardarCambios();
                        }
                    }
                    else
                    {
                        GuardarCambios();
                    }
                }
            }
            else
            {
                LError.Text = GetLocalResourceObject("REV_RangoCentral") != null ? GetLocalResourceObject("REV_RangoCentral").ToString() : "El SCV debe tener configurado algún rango de abonados.";
                return;
            }
        }
        else
        {
            cMsg.alert((string)GetLocalResourceObject("RequiredFieldValidator1Resource1.ErrorMessage"));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
    }

    /// <summary>
    /// 
    /// </summary>
    private void GuardarCambios()
    {
        try
        {
            ServiciosCD40.Encaminamientos n = new ServiciosCD40.Encaminamientos();
            n.IdSistema = (string)Session["idsistema"];
            n.Central = TxtCentral.Text;
            n.CentralPropia = CentralPropia.Checked;
            n.Throwswitching = Throwswitching.Checked;
			n.NumTest = TBNumTest.Text;
            n.CentralIp = CBCentralIp.Checked;

            NewItem = TxtCentral.Text;

			if (!Modificando && !EncaminamientoDadoDeAlta)
			{
				if (ServicioCD40.InsertSQL(n) < 0)
					logDebugView.Warn("(Encaminamientos-GuardarCambios): No se ha podido insertar el encaminamiento");
				else
				{
                    // Guardar la central como equipo externo
                    CreaCentralComoEquipoExterno(TxtCentral.Text);

                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
					KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
					if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1) && (InfoEnc != null))
					{
						InfoEnc.idEncaminamiento = TxtCentral.Text;
						InfoEnc.centralPropia = CentralPropia.Checked;
						InfoEnc.throwswhitching = Throwswitching.Checked;
						InfoEnc.numTest = TBNumTest.Text;
					}
				}
			}
			else
			{
                if (bCentralConDestinoATS_LC && CBCentralIp.Checked != bEsCentralIP)
                {
                    //Si existen destinos ATS en el rango de la central IP y registra un evento informando de la situación
                    logDebugView.Info("(Encaminamientos-GuardarCambios): El usuario ha modificado la central " + n.Central + " como central no IP. Por lo que los destinos ATS configurados en el panel de línea caliente que utilizan dicha central dejarán de funcionar.");
                    
                    string[] arrAux = new string[1];
                    string sTexto = string.Empty;

                    sTexto = (string)GetGlobalResourceObject("Espaniol", "CentralIPModificada_DestinosATS_LC");

                    if (null == sTexto)
                    {
                        sTexto = "El usuario ha modificado la central {0} como central no IP. Por lo que los destinos ATS configurados en el panel de línea caliente que utilizan dicha central dejarán de funcionar";
                    }

                    sTexto = string.Format(sTexto, n.Central);

                    arrAux[0] = sTexto;
                    ServicioCD40.GeneraIncidenciaConfiguracion(n.IdSistema, INCIDENCIA_CNF, arrAux);
                }

                if (null != DSRutas && DSRutas.Tables.Count > 0)
                {
                    for (int orden = 0; orden < DSRutas.Tables[0].Rows.Count; orden++)
                    {
                        DataRow dr = DSRutas.Tables[0].Rows[orden];

                        if ((string)dr["IdRuta"] == LBRutas.Items[orden].Text)
                        {
                            ServiciosCD40.Rutas r = new ServiciosCD40.Rutas();
                            r.IdSistema = (string)dr["IdSistema"];
                            r.Central = (string)dr["Central"];
                            r.IdRuta = LBRutas.Items[orden].Text;
                            r.Tipo = (string)dr["Tipo"];
                            r.Orden = (int)dr["Orden"];

                            ServicioCD40.UpdateSQL(r);
                        }
                    }
                }
                if (ServicioCD40.UpdateSQL(n) < 0)
                    logDebugView.Warn("(Encaminamientos-GuardarCambios): No se ha podido actualizar el encaminamiento");
                else
                {
                    //MVO: Si el encaminamiento está definido como una central IP, se actualiza la configuración.
                    //En caso contrario, se verifica si anteriormente se había definido commo una central IP  y se elimina el registro de equipos externos.
                    if (CBCentralIp.Checked)
                        CreaCentralComoEquipoExterno(TxtCentral.Text);
                    else
                    {
                        BorraCentralComoEquipoExterno(n.IdSistema,TxtCentral.Text);
                    }

                }
            
            }
		
			GuardarRangos();
		}
        catch (Exception e)
        {
            logDebugView.Error("(Encaminamientos-GuardarCambios):",e);
        }
        
        if (!Modificando)
            GuardarEncaminamientoCD30();

		Modificando = false;

		HabilitaControles(false);


		PanelRangos.Visible = false;

		// ServicioCD40.Commit();

		EncaminamientoDadoDeAlta = false;

		ListBox1.Items.Clear();
        MuestraDatos(DameDatos());
    }

    private void CreaCentralComoEquipoExterno(string centralId)
    {
        if (CBCentralIp.Checked)
        {
            ServiciosCD40.EquiposEU eq = new ServiciosCD40.EquiposEU();
            eq.IdSistema = (string)Session["idsistema"];
            eq.IdEquipos = centralId;
  
            //Comprobamos si existe el equipo externo, si existe lo actualizamos

            ServiciosCD40.Tablas[] d= ServicioCD40.ListSelectSQL(eq);
                
            bool bExiste = false;

            if (d != null && d.Length > 0)
            {
                //Si existe se actualiza el registro 
                bExiste = true;
            }
            else
            {
                // Si no existe se inserta
                bExiste = false;
            }

            eq.Interno = CentralPropia.Checked;
            eq.TipoEquipo = 3;  // Telefonia
            // #4599
            if (!bSBCEnaire || !CentralPropia.Checked)
            {
                eq.IpRed1 = TbIp1.Text;
                eq.IpRed2 = TbIp2.Text;
                eq.IpRed3 = TbIp3.Text;
            }
            else
            {
                // Si es central propia y SBC se copia la IPRed3 a IpRed1
                eq.IpRed1 = TbIp3.Text;
                eq.IpRed2 = TbIp2.Text;
                eq.IpRed3 = TbIp1.Text;
            }
            // Para distinguir estos equipos de los realmente configurados como equipos externos
            // se crean con los valores de Min y Max a -1 y no mostrarse en la página de equipos externos.
            eq.Max = eq.Min = -1;

            //El puerto Sip ya no se configura
            //eq.SipPort = 5060;

            //En los campos IpRed1, IpRed2, IpRed3,SrvPresenciaIpRed1,SrvPresenciaIpRed2, SrvPresenciaIpRed3
            // se guarda la dirección IP y el puerto en formato XXX.XXX.XXX.XXX:YYYY
            // #4599
                
            if (!bSBCEnaire || !CentralPropia.Checked)
            {
                eq.SrvPresenciaIpRed1 = TbSrvPresIp1.Text;
                eq.SrvPresenciaIpRed2 = TbSrvPresIp2.Text;
                eq.SrvPresenciaIpRed3 = TbSrvPresIp3.Text;
            }
            else
            {
                // Si es central propia y SBC se copia la IPRed3 a IpRed1
                eq.SrvPresenciaIpRed1 = TbSrvPresIp3.Text;
                eq.SrvPresenciaIpRed2 = TbSrvPresIp2.Text;
                eq.SrvPresenciaIpRed3 = TbSrvPresIp1.Text;
            }

            if (!bExiste)
                ServicioCD40.InsertSQL(eq);
            else
                ServicioCD40.UpdateSQL(eq);
        }
    }

    private void BorraCentralComoEquipoExterno(string strIdSistema, string strIdCentral)
    {
        //Comprueba si existe un equipo externo asociado al encaminamiento,
        //Si existe lo elimina. Cuando el encaminamiento tiene activado el check de central IP
        // debe tener definido un equipo externo con las direcciones IP y el puerto de la central IP

        ServiciosCD40.EquiposEU eq = new ServiciosCD40.EquiposEU();
        eq.IdSistema = strIdSistema;
        eq.IdEquipos = strIdCentral;

        //Comprobamos si existe el equipo externo
        ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(eq);
        if (d != null && d.Length > 0 && ((ServiciosCD40.EquiposEU)d[0]).Max == -1 && ((ServiciosCD40.EquiposEU)d[0]).Min == -1)
        {
            //Si existe lo borramos, sólo si los valores Max y Min tienen el valor -1 que indica que está asociado a un encaminamiento
            if (ServicioCD40.DeleteSQL(eq)<0)
                logDebugView.Warn("(Encaminamientos-BorrarCentralEquipoExterno): No se ha podido borrar el equipo externo");
        }

    }

    private bool bExisteEquipoExternoConNombreCentral(string strIdSistema, string strIdCentral)
    {
        //Comprueba si existe un equipo externo con el mismo nombre que un encaminamiento     
        bool bExiste = false;
        ServiciosCD40.EquiposEU eq = new ServiciosCD40.EquiposEU();
        eq.IdSistema = strIdSistema;
        eq.IdEquipos = strIdCentral;

        //Comprobamos si existe el equipo externo
        ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(eq);
        if (d != null && d.Length > 0)
        {
            bExiste = true;
        }

        return bExiste;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool NumTestValido(string strNumText)
    {
        string strInicial = string.Empty;
        string strFinal = string.Empty;

        int iNumTest = 0;

        bool bOK = false;
        
        if (!string.IsNullOrWhiteSpace(strNumText) && Int32.TryParse(strNumText, out iNumTest))
        {
            //Se comprueba que el número de test se encuentra definido dentro de los rangos de la Red ATS de la Central
            if (GViewRangos.Rows.Count > 0)
            {
                foreach (TableRow rowRango in GViewRangos.Rows)
                {
                    if (string.IsNullOrEmpty(strInicial))
                        strInicial = rowRango.Cells[3].Text;

                    bOK |= Convert.ToInt32(rowRango.Cells[3].Text) <= iNumTest &&
                                Convert.ToInt32(rowRango.Cells[4].Text) >= iNumTest;
                    strFinal = rowRango.Cells[4].Text;
                }

                if (!bOK)
                    LblNumTestFueraRango.Text = string.Format(LblNumTestFueraRango.Text, strInicial, strFinal);
            }
            else
            {
                if (null != GetLocalResourceObject("LblRangoSCVPropioNoDefinido"))
                {
                    LblNumTestFueraRango.Text = GetLocalResourceObject("LblRangoSCVPropioNoDefinido").ToString();
                }
                else
                    LblNumTestFueraRango.Text="Debe configurar algún rango de números pertenecientes al plan de numeración ATS.";
            }
        }
        else
        {
            if (null != GetLocalResourceObject("LblNumTextNoOk"))
            {
                LblNumTestFueraRango.Text = GetLocalResourceObject("LblNumTextNoOk").ToString();
            }
            else
                LblNumTestFueraRango.Text="El número de Test está vacío o no es correcto";
        }


        if (!bOK)
            LblNumTestFueraRango.Visible = true;
        else
            LblNumTestFueraRango.Visible = false;

        return bOK;
    }

    /// <summary>
    /// 
    /// </summary>
	private void ActualizaArbol()
	{
		TreeView1.Nodes[0].ChildNodes.Clear();
		GetRutas(TreeView1.Nodes[0]);
	}

    /// <summary>
    /// 
    /// </summary>
    private void GuardarEncaminamientoCD30()
    { 
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1) && (InfoEnc != null))
        {
            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();            
            sincro.AltaRuta(InfoEnc.idEncaminamiento, false, 9, "");
            foreach (InfoRango r in InfoEnc.rangos)
            {
				if (InfoEnc.centralPropia)
				{
					sincro.AltaCentralPropia(InfoEnc.idEncaminamiento, InfoEnc.numTest, InfoEnc.throwswhitching);
					sincro.AltaRangoR2(r.tipo, r.inicial, r.final);
				}
				else
					sincro.AltaDestinoR2(InfoEnc.idEncaminamiento, r.tipo, r.inicial, r.final);
            }
            int numRuta=1;
            foreach (InfoRuta ru in InfoEnc.rutas)
            {
                if (ru.tipo == 0)//ruta directa
                {                    
                    sincro.AltaRuta(InfoEnc.idEncaminamiento, true, 0, (ru.troncales.Count>0)?ru.troncales[0]:"");
                }
                else
                {
                    foreach (string rec in ru.troncales)
                    {
                        sincro.AltaRuta(InfoEnc.idEncaminamiento, false, numRuta, rec);
                        ++numRuta;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private InfoEncaminamiento CargarEncaminamientoCD30()
    {
        InfoEncaminamiento inf = new InfoEncaminamiento();
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1) && (InfoEnc != null))
        {
            inf.idEncaminamiento = TxtCentral.Text;
            inf.centralPropia = CentralPropia.Checked;
            inf.throwswhitching = Throwswitching.Checked;


            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
            sincro.AltaRuta(InfoEnc.idEncaminamiento, false, 9, "");
            foreach (InfoRango r in InfoEnc.rangos)
            {
                if (InfoEnc.centralPropia)
                    sincro.AltaRangoR2(r.tipo, r.inicial, r.final);
                else
                    sincro.AltaDestinoR2(InfoEnc.idEncaminamiento, r.tipo, r.inicial, r.final);
            }
            int numRuta = 1;
            foreach (InfoRuta ru in InfoEnc.rutas)
            {
                if (ru.tipo == 0)//ruta directa
                {
                    sincro.AltaRuta(InfoEnc.idEncaminamiento, true, 0, (ru.troncales.Count > 0) ? ru.troncales[0] : "");
                }
                else
                {
                    foreach (string rec in ru.troncales)
                    {
                        sincro.AltaRuta(InfoEnc.idEncaminamiento, false, numRuta, rec);
                        ++numRuta;
                    }
                }
            }
        }
        return inf;
    }

    /// <summary>
    /// 
    /// </summary>
	private void GuardarRangos()
	{
		try
		{
			ServiciosCD40.Rangos t = new ServiciosCD40.Rangos();
			t.IdSistema = s.Value;
			t.Central = TxtCentral.Text;

			// Eliminar todos los rangos de este sistema para esta central.
			if (ServicioCD40.DeleteSQL(t) < 0) 
                logDebugView.Warn("(Encaminamientos-GuardarRangos): No se ha podido eliminar el rango de Usuarios Abonado de la central");

			for (int i = 0; i < DSRangos.Tables[0].Rows.Count; i++)
			{
				t.Tipo = (string)DSRangos.Tables[0].Rows[i].ItemArray[2];
				t.Inicial = (string)DSRangos.Tables[0].Rows[i].ItemArray[3];
				t.Final = (string)DSRangos.Tables[0].Rows[i].ItemArray[6];
                if (DListPrefijo.Enabled && DSRangos.Tables[0].Rows[i].ItemArray[5] != System.DBNull.Value)
                {
                    t.IdPrefijo = (uint)DSRangos.Tables[0].Rows[i].ItemArray[4];
                    t.IdAbonado = (string)DSRangos.Tables[0].Rows[i].ItemArray[5];
                }
                else
                {
                    t.IdPrefijo = 0;
                    t.IdAbonado = string.Empty;
                }

                if (ServicioCD40.InsertSQL(t) < 0)
                    logDebugView.Warn("(Encaminamientos-GuardarRangos): No se ha podido insertar el rango de Usuarios Abonado de la central");
			}
		}
		catch (Exception ex)
		{
			logDebugView.Error("(Encaminamientos-GuardarRangos): ", ex);
		}
	}

    /// <summary>
    /// 
    /// </summary>
	protected override void AceptarCambios()
	{
		//GuardarCambios();
		base.AceptarCambios();
	}

    /// <summary>
    /// 
    /// </summary>
    protected override void CancelarCambios()
    {
        LError.Text = string.Empty;
        if (bUpdateRango && NumPaginaActiva == 1)
        {
            bUpdateRango = false;
            Panel1.Width = 560;
        }

        //202006 JOI Error #4066
//        if (bModificacionAsignaciónTroncales)
//        {
//            bModificacionAsignaciónTroncales = false;
//            CancelarTroncales();
//        }

        //Se oculta el panel de rutas por si está visible y se pulsa cancelar
        PanelRutas.Visible = false;
        TreeView1.Visible = true;

        if (NumPaginaActiva == 2 && PanelRutas.Visible)
            Panel1.Width = 680;

        InfoEnc=null;

        Modificando = false;
        HabilitaControles(false);
        PanelRangos.Visible = false;

        ListBox1.Items.Clear();
        MuestraDatos(DameDatos());
    }

    /// <summary>
    /// 
    /// </summary>
	private void LimpiaDatos()
	{
		TxtCentral.Text = "";
		CentralPropia.Checked = false;
		Throwswitching.Checked = false;
		DDLRutas.Items.Clear();
        TreeView1.Nodes[0].ChildNodes.Clear();

        if (LBRutas.Items.Count > 0)
            LBRutas.Items.Clear();
        
        TbIp1.Text = TbIp2.Text = TbIp3.Text=string.Empty;
        CBCentralIp.Checked = false;
        TbSrvPresIp1.Text = TbSrvPresIp2.Text = TbSrvPresIp3.Text = string.Empty;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtNuevo_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = NumPaginaActiva = 0;
        IBCentralATS.CssClass = "buttonImageSelected";
        IBRangos.CssClass = "buttonImage";
        IBRutas.CssClass = "buttonImage";

        //IBCentralATS.ImageUrl = GetLocalResourceObject("IBCentralATSResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuEncaminamientosCentralATSSelected.JPG";
        //IBRangos.ImageUrl = GetLocalResourceObject("IBRangosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuEncaminamientosRangosUnSelected.JPG";
        //IBRutas.ImageUrl = GetLocalResourceObject("IBRutasResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuEncaminamientosRutasUnSelected.JPG";
        
        Modificando = false;
		EncaminamientoDadoDeAlta = false;
        HayRutaDirecta = false;
        bCentralConDestinoATS_LC = false;
        bEsCentralIP = false;

        BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");

		LimpiaDatos();
		HabilitaControles(true);

        bHabilitaControlesCentralIP(bEsCentralIP, CentralPropia.Checked);

		if (DSRangos != null)
			DSRangos.Clear();
		if (DSRutas != null)
			DSRutas.Clear();

		GViewRangos.DataBind();
		ListTroncales.Items.Clear();
        ListTroncalesLibres.Items.Clear();
        TblCentralIp.Visible = CBCentralIp.Checked;
        IndexListBox1 = ListBox1.SelectedIndex;

        //CargarTroncalesSinAsignar();
        //CargarTodosLosTroncales();

        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
        {
            InfoEnc = new InfoEncaminamiento();
        }
	}

    private void CargarTodosLosTroncales()
    {
        ServiciosCD40.Troncales t = new ServiciosCD40.Troncales();

        t.IdSistema = (string)Session["idsistema"];

        DataSet d = ServicioCD40.DataSetSelectSQL(t);
        if (d.Tables.Count > 0)
        {
            ListTroncalesLibres.DataSource = d;
            ListTroncalesLibres.DataTextField = "IdTroncal";
            ListTroncalesLibres.DataBind();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtModificar_Click(object sender, EventArgs e)
    {
		Modificando = true;

		HabilitaControles(true);

        bHabilitaControlesCentralIP(bEsCentralIP, CentralPropia.Checked);

		TxtCentral.Enabled = false;
		
		//CentralPropia.Visible = false;
		CargarTroncalesSinAsignar();
        IndexListBox1 = ListBox1.SelectedIndex;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="habilita"></param>
	private void HabilitaControles(bool habilita)
	{
		ListBox1.Enabled = !habilita;
	 	BtEliminar.Visible = BtModificar.Visible = !habilita && PermisoSegunPerfil && ListBox1.Items.Count > 0;
		BtAceptar.Visible = habilita;
		BtCancelar.Visible = habilita;

		TxtCentral.Enabled = habilita;
		CentralPropia.Enabled = habilita && (!HayCentralPropia || CentralPropia.Checked);
        CBCentralIp.Enabled = habilita;

		Throwswitching.Enabled = habilita;
		IBRutaArriba.Enabled = IBRutaAbajo.Enabled = habilita;

		TxtBInicial.Enabled = habilita;
		TxtAbonado.Enabled = habilita;
		TxtBFinal.Enabled = habilita;
        BtNuevoRango.Enabled = habilita;
		GViewRangos.Enabled = habilita;

		TxtIdRuta.Enabled = habilita;
        DListTipo.Enabled = habilita && PermisoSegunPerfil && DDLRutas.Items.Count > 0 && !HayRutaDirecta;

		ListTroncales.Enabled = ListTroncalesLibres.Enabled = habilita;
		BtnNuevaRuta.Enabled = BtnEliminarRuta.Enabled = habilita;
		IButAsignar.Enabled = IButQuitar.Enabled = habilita;
		IBRutas.Visible = !CentralPropia.Checked;

		BtNuevo.Visible = !habilita && PermisoSegunPerfil;
		LBMiCentral.Visible = !habilita;

		LblNumTest.Enabled = TBNumTest.Enabled = habilita;
		LblNumTest.Visible = TBNumTest.Visible = CentralPropia.Checked;

		RequiredFieldValidator1.Visible = RequiredFieldValidator2.Visible = RequiredFieldValidator3.Visible = RequiredFieldValidator4.Visible = habilita;
		ValidationSummary1.Visible = habilita;

        if (habilita==false)
        {
            bHabilitaControlesCentralIP(false, CentralPropia.Checked);
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void OnButtonTab_Click(object sender, EventArgs e)
	{
		Button ibSelected = (Button)sender;

		switch (ibSelected.ID)
		{
			case "IBCentralATS":
                IBCentralATS.CssClass = "buttonImageSelected";
                IBRangos.CssClass = "buttonImage";
                IBRutas.CssClass = "buttonImage";
				MultiView1.ActiveViewIndex = NumPaginaActiva = 0;
                Panel1.Width = 680;
                //Panel1.Height = 355;
				break;
			case "IBRangos":
                IBCentralATS.CssClass = "buttonImage";
                IBRangos.CssClass = "buttonImageSelected";
                IBRutas.CssClass = "buttonImage";

                if (!bUpdateRango)
                    Panel1.Width = 560;
                else
                    Panel1.Width = 740;

				//Panel1.Height = 355;
				MultiView1.ActiveViewIndex = NumPaginaActiva = 1;

                if (!BtNuevo.Visible && !Modificando)
                {
                    //Si es un alta
                    if (string.IsNullOrWhiteSpace(TxtCentral.Text))
                        BtNuevoRango.Enabled = false;
                    else
                        BtNuevoRango.Enabled = true;
                }
				break;
			case "IBRutas":
                IBCentralATS.CssClass = "buttonImage";
                IBRangos.CssClass = "buttonImage";
                IBRutas.CssClass = "buttonImageSelected";
				Panel1.Width = 680;
				//Panel1.Height = 355;
				MultiView1.ActiveViewIndex = NumPaginaActiva = 2;

                if (!BtNuevo.Visible && !Modificando && !EncaminamientoDadoDeAlta)
                {
                    //Si es un alta
                    if (string.IsNullOrWhiteSpace(TxtCentral.Text))
                        BtnNuevaRuta.Enabled = BtnEliminarRuta.Enabled = false;
                    else
                        BtnNuevaRuta.Enabled = BtnEliminarRuta.Enabled = true;
                }
				break;
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void GViewRangos_OnSelectedIndexChanging(object sender, GridViewPageEventArgs e)
	{
		GViewRangos.PageIndex = e.NewPageIndex;
		GViewRangos.DataSource = DSRangos;
		GViewRangos.DataBind();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void GViewRangos_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1) && (InfoEnc != null))
        {
            InfoEnc.rangos.RemoveAll(
                delegate(InfoRango i) 
                {
                    return ((i.inicial.CompareTo(DSRangos.Tables[0].Rows[GViewRangos.PageIndex * GViewRangos.PageSize + e.RowIndex][3]) == 0) 
                        && (i.inicial.CompareTo(DSRangos.Tables[0].Rows[GViewRangos.PageIndex * GViewRangos.PageSize + e.RowIndex][6]) == 0));
                });
        }

        if (null != DSRangos && DSRangos.Tables.Count > 0 && DSRangos.Tables[0].Rows.Count>0)
        {
            DSRangos.Tables[0].Rows[GViewRangos.PageIndex * GViewRangos.PageSize + e.RowIndex].Delete();
            DSRangos.AcceptChanges();
        }

        GViewRangos.DataSource = DSRangos;
		GViewRangos.DataBind();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GViewRangos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edicion")
        {
            Panel1.Width = 740;
            SelectedRangoIndex = Convert.ToInt32(e.CommandArgument);

            DropDownList d = (DropDownList)GViewRangos.Rows[SelectedRangoIndex].Cells[2].FindControl("DListTipoRango");
            DListTipoRango.SelectedValue = d.SelectedValue;

            TxtBInicial.Text = GViewRangos.Rows[SelectedRangoIndex].Cells[3].Text;
            TxtBFinal.Text = GViewRangos.Rows[SelectedRangoIndex].Cells[4].Text;
            TxtAbonado.Text = GViewRangos.Rows[SelectedRangoIndex].Cells[5].Text == "&nbsp;" ? string.Empty : GViewRangos.Rows[SelectedRangoIndex].Cells[5].Text;
            DListPrefijo.SelectedIndex = -1;
            if (TxtAbonado.Text.Length > 0)
                DListPrefijo.Items.FindByText(GViewRangos.Rows[SelectedRangoIndex].Cells[6].Text).Selected = true;

            PanelRangos.Visible = true;
            bUpdateRango = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnAceptarRango_Click(object sender, EventArgs e)
	{
         string strNombreCentral = TxtCentral.Text;
         string strSistema = (string)Session["idsistema"];
         string strMsgError=string.Empty;
         LError.Text = string.Empty;

         if (ExisteRango(strSistema, strNombreCentral,SelectedRangoIndex, ref strMsgError))
         {
             cMsg.alert(strMsgError);
             return;
         }

        if (SelectedRangoIndex != -1)
        {
            if (DSRangos.Tables[0].Rows.Count>SelectedRangoIndex)
                DSRangos.Tables[0].Rows[SelectedRangoIndex].Delete();
            SelectedRangoIndex = -1;
        }

        DataRow dr = DSRangos.Tables[0].NewRow();
        dr[0] = s.Value;
        dr[1] = TxtCentral.Text;
        dr[2] = DListTipoRango.SelectedValue;
        dr[3] = TxtBInicial.Text;
        dr[6] = TxtBFinal.Text;
        if (DListPrefijo.Enabled && TxtAbonado.Text.Length > 0)
        {
            dr[4] = DListPrefijo.SelectedValue;
            dr[5] = TxtAbonado.Text;
            dr[7] = DListPrefijo.SelectedItem.Text;
        }
        

        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1) && (InfoEnc != null))
        {
            InfoRango rang = new InfoRango();
            rang.inicial = TxtBInicial.Text;
            rang.final = TxtBFinal.Text;
            rang.numero = TxtAbonado.Text;
            rang.tipo = (DListTipoRango.SelectedValue == "O") ? 0 : 1;
            rang.idRed = DListPrefijo.Enabled ? DListPrefijo.SelectedItem.Text : string.Empty;
            InfoEnc.rangos.Add(rang);
        }

        DSRangos.Tables[0].Rows.Add(dr);

		DSRangos.AcceptChanges();

		GViewRangos.Visible = true;
		GViewRangos.DataSource = DSRangos;
		GViewRangos.DataBind();

		PanelRangos.Visible = false;
		Panel1.Width = 560;

        bUpdateRango = false;
	}

    /// <summary>
    /// 
    /// </summary>
	private void CargarPrefijos()
	{
		try
		{
			DListPrefijo.Items.Clear();
			ServiciosCD40.Redes t = new ServiciosCD40.Redes();
			t.IdSistema = (string)Session["idsistema"];
			DataSet d = ServicioCD40.DataSetSelectSQL(t);

            if (d.Tables.Count > 0)
            {
                DListPrefijo.DataTextField = "IdRed";
                DListPrefijo.DataValueField = "IdPrefijo";
                DListPrefijo.DataSource = d;
                DListPrefijo.DataBind();
                if (DListPrefijo.Items.FindByText("ATS") != null)
                    DListPrefijo.Items.Remove(DListPrefijo.Items.FindByText("ATS"));
            }

			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
			if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
			{
				if (DListPrefijo.Items.Count > 0)
				{
					for (int j = 0; j < DListPrefijo.Items.Count; j++)
					{
						if (DListPrefijo.Items[j].Value.CompareTo("4") != 0)
						{
							DListPrefijo.Items.Remove(DListPrefijo.Items[j--]);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			logDebugView.Error("(Encaminamientos-CargarPrefijos): ", ex);
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnCancelarRango_Click(object sender, EventArgs e)
	{
		PanelRangos.Visible = false;
		Panel1.Width = 560;
        bUpdateRango = false;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void GViewRangos_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		DropDownList d = (DropDownList)e.Row.FindControl("DListTipoRango");
		if (d != null) 
			d.SelectedValue = (String)DataBinder.Eval(e.Row.DataItem, "Tipo");
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void LBMiCentral_Click(object sender, EventArgs e)
	{
		ListBox1.SelectedIndex = IndiceCentralPropia;
		if (MultiView1.ActiveViewIndex == 2)
		{
            IBCentralATS.CssClass = "buttonImageSelected";
            IBRangos.CssClass = "buttonImage";
            IBRutas.CssClass = "buttonImage";
            Panel1.Width = 680;
			//Panel1.Height = 355;
			MultiView1.ActiveViewIndex = 0;
		}
		MostrarElemento();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idRuta"></param>
	private void CargarTroncalesRuta(string idRuta)
	{
		try
		{
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			s = config.AppSettings.Settings["Sistema"];
			ServiciosCD40.TroncalesRuta t = new ServiciosCD40.TroncalesRuta();
			t.IdSistema = s.Value;
			t.Central = TxtCentral.Text;
			t.IdRuta = idRuta;

			DataSet d = ServicioCD40.DataSetSelectSQL(t);
            if (d.Tables.Count > 0)
            {
                ListTroncales.DataSource = d;
                ListTroncales.DataTextField = "IdTroncal";
                ListTroncales.DataBind();
            }
            //202006 JOI Errores #4066
//            LTroncalesIniciales.Clear();
//            for (int i = 0; i < ListTroncales.Items.Count; i++)
//            {
//                LTroncalesIniciales.Add(ListTroncales.Items[i].Text);
//            }
		}
		catch (Exception ex)
		{
			logDebugView.Error("(Encaminamientos-CargarTroncalesRuta): ", ex);
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void DDLRutas_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		if (DSRutas!=null)
			DListTipo.SelectedValue=(string)(DSRutas.Tables[0].Rows[((DropDownList)sender).SelectedIndex].ItemArray[3]);

		CargarTroncalesRuta(((DropDownList)sender).SelectedItem.Text);
		CargarTroncalesSinAsignar();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnEliminarRuta_OnClick(object sender, EventArgs e)
	{
		ServiciosCD40.Rutas r = new ServiciosCD40.Rutas();
		r.IdSistema = s.Value;
		r.Central = TxtCentral.Text;
		r.IdRuta = DDLRutas.SelectedValue;

        DataSet dsRuta = ServicioCD40.DataSetSelectSQL(r);
        if (dsRuta != null && dsRuta.Tables.Count > 0 && dsRuta.Tables[0].Rows.Count>0)
        {
            r.Orden = (string)dsRuta.Tables[0].Rows[0]["Tipo"] == "D" ? Int32.MaxValue : (int)dsRuta.Tables[0].Rows[0]["Orden"];
            if (ServicioCD40.DeleteSQL(r) > 0)
            {
                //MVO.20170714: Al borrar la ruta, se actualiza el ListBox que muestra las rutas en la pestaña Central ATS
                if (LBRutas.Items.Count > 0)
                    LBRutas.Items.Clear();

                LBRutas.DataSource = ObtenerRutas(null);
                LBRutas.DataBind();
                ActualizaArbol();

                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1) && (InfoEnc != null))
                {
                    InfoEnc.rutas.RemoveAll(delegate(InfoRuta i) { return i.idRuta == r.IdRuta; });
                }
            }
        }
	}

    /// <summary>
    /// 
    /// </summary>
	private void CargarTroncalesSinAsignar()
	{
		try
		{
			DataSet d = ServicioCD40.TroncalesSinAsignarARutas(s.Value, TxtCentral.Text, DListTipo.SelectedValue == "D" ? "" : DDLRutas.SelectedItem.Text);
            if (null!=d && d.Tables.Count > 0)
            {
                ListTroncalesLibres.DataSource = d;
                ListTroncalesLibres.DataTextField = "IdTroncal";
                ListTroncalesLibres.DataBind();
            }
		}
		catch (Exception ex)
		{
			logDebugView.Error("(Encaminamientos-CargarTroncalesSinAsignar): ", ex);
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnNuevaRuta_Click(object sender, EventArgs e)
	{
		Panel1.Width = 700;

		TxtIdRuta.Text = "";

        RBDirecta.Enabled = !HayRutaDirecta;

        RBDirecta.Checked = !HayRutaDirecta;
        RBAlternativa.Checked = HayRutaDirecta;
		
		PanelRutas.Visible = true;
		TreeView1.Visible = false;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnCancelarRuta_Click(object sender, EventArgs e)
	{
		PanelRutas.Visible = false;
		TreeView1.Visible = true;
		Panel1.Width = 680;
	}
	
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnAceptarRuta_Click(object sender, EventArgs e)
	{
        String strIdSistema = s.Value;
        LError.Text = string.Empty;
        bool bNuevaCentral = false;


        if (!Modificando && !EncaminamientoDadoDeAlta) // Si se está dando de alta una nueva Central ATS y se quiere añadir una ruta
        {
            //Se comprueba que no exista otra central con el mismo nombre
            if (bExisteCentralConMismoNombre(strIdSistema, TxtCentral.Text))
            {
                LError.Text = GetLocalResourceObject("REV_CentralExiste") != null ? GetLocalResourceObject("REV_CentralExiste").ToString() : "En el sistema, ya existe un SCV con el mismo identificador.";
                return;
            }
            else if (bExisteEquipoExternoConNombreCentral(strIdSistema, TxtCentral.Text))
            {
                //No se puede dar de alta un encaminamiento con el mismo nombre que un equipo externo ya existente
                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ErrorEncaminamientoIdCentral_EU_Existente_AddRuta"));
                return;
            }

            bNuevaCentral = true;
        }

        //Se comprueba si al menos se ha configurado un rango
        if (DSRangos != null && DSRangos.Tables.Count > 0 && DSRangos.Tables[0].Rows.Count > 0)
        {
		    ServiciosCD40.Rutas r = new ServiciosCD40.Rutas();

		    PanelRutas.Visible = false;
		    TreeView1.Visible = true;
		
		    Panel1.Width = 680;

            int cuantasRutasAlternativas = 0;

            if (null != DSRutas && DSRutas.Tables.Count > 0)
            {
                foreach (DataRow dr in DSRutas.Tables[0].Rows)
                {
                    if ((string)dr["Tipo"] == "A")
                        cuantasRutasAlternativas++;
                }
            }

		    r.IdSistema = s.Value;
		    r.IdRuta = TxtIdRuta.Text;
		    r.Central = TxtCentral.Text;
		    r.Tipo = RBAlternativa.Checked ? "A" : "D";
		    r.Orden = r.Tipo == "D" ? 0 : cuantasRutasAlternativas + 1;

            if (bNuevaCentral)	// Nuevo encaminamiento
            {
                // Antes de dar de alta la ruta, es preciso tener dado de alta el encaminamiento.
                ServiciosCD40.Encaminamientos n = new ServiciosCD40.Encaminamientos();
                n.IdSistema = s.Value;
                n.Central = TxtCentral.Text;
                n.CentralPropia = CentralPropia.Checked;
                n.Throwswitching = Throwswitching.Checked;
                NewItem = TxtCentral.Text;

                if (ServicioCD40.InsertSQL(n) < 0)
                    logDebugView.Warn("(Encaminamientos-GuardarCambios): No se ha podido insertar el encaminamiento");
                else
                {
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                    KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                    if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1) && (InfoEnc != null))
                    {
                        InfoEnc.idEncaminamiento = TxtCentral.Text;
                        InfoEnc.centralPropia = CentralPropia.Checked;
                        InfoEnc.throwswhitching = Throwswitching.Checked;
                    }
                }

                EncaminamientoDadoDeAlta = true;
            }


            if (ServicioCD40.InsertSQL(r) < 0)
                logDebugView.Warn("(Encaminamientos-GuardarCambios): No se ha podido insertar la ruta");
            else
            { 
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1) && (InfoEnc != null))
                {
                    InfoRuta lr = new InfoRuta();
                    lr.idRuta = TxtIdRuta.Text;
                    lr.tipo = RBAlternativa.Checked ? 1 : 0;
                    InfoEnc.rutas.Add(lr);
                }
            }

		    ActualizaArbol();

		    if (RBDirecta.Checked)
			    HayRutaDirecta = true;

            //MVO.20170714: Al añadir una nueva ruta se recupera y actualiza el LisBox de la pestaña Central ATS
            if (LBRutas.Items.Count>0)
                LBRutas.Items.Clear();

            LBRutas.DataSource = ObtenerRutas(TxtIdRuta.Text);
            LBRutas.DataBind();
        }
        else
        {
            LError.Text = GetLocalResourceObject("REV_RangoCentral") != null ? GetLocalResourceObject("REV_RangoCentral").ToString() : "El SCV debe tener configurado algún rango de abonados.";
            return;
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void IButAsignar_Click(object sender, ImageClickEventArgs e)
	{
		if (ListTroncales.Items.Count >= 8)
			cMsg.alert((string)GetGlobalResourceObject("Espaniol", "TroncalesRutaSup"));
		else
			if (ListTroncalesLibres.SelectedIndex >= 0)
			{
				Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
				KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
				if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
				{
					if ((ListTroncales.Items.Count > 0) && (DListTipo.SelectedValue.CompareTo("D") == 0))
					{
						cMsg.alert((string)GetGlobalResourceObject("Espaniol", "NumTroncalesRutaDirecta"));
						return;
					}
				}
				for (int i = 0; i < ListTroncalesLibres.Items.Count; i++)
					if (ListTroncalesLibres.Items[i].Selected)
					{
						AsignaTroncal(ListTroncalesLibres.Items[i].Text);
						ListTroncales.Items.Add(ListTroncalesLibres.Items[i]);
						ListTroncalesLibres.Items.Remove(ListTroncalesLibres.Items[i]);
						i--;
                        //202006 JOI Error #4066
//                        bModificacionAsignaciónTroncales = true;
					}
			}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void IButQuitar_Click(object sender, ImageClickEventArgs e)
	{
		if (ListTroncales.SelectedIndex >= 0)
			for (int i = 0; i < ListTroncales.Items.Count; i++)
				if (ListTroncales.Items[i].Selected)
				{
					DesasignaTroncal(ListTroncales.Items[i].Text);

					ListTroncalesLibres.Items.Add(ListTroncales.Items[i]);
					ListTroncales.Items.Remove(ListTroncales.Items[i]);
					i--;
				}

	}

    /// <summary>
    /// 
    /// </summary>
    //202006 JOI Error #4066
//    private void CancelarTroncales()
//    {
//        // Limpia lista troncales con modificaciones.
//        for (int i = 0; i < ListTroncales.Items.Count; i++)
//        {
//            DesasignaTroncal(ListTroncales.Items[i].Text);
//            ListTroncalesLibres.Items.Add(ListTroncales.Items[i]);
//            ListTroncales.Items.Remove(ListTroncales.Items[i]);
//            i--;
//        }
//
//        //Recupera estado inicial de troncales asignados
//        for (int b = 0; b < LTroncalesIniciales.Count; b++)
//        {
//            AsignaTroncal(LTroncalesIniciales[b]);
//        }
//    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idTroncal"></param>
	private void DesasignaTroncal(string idTroncal)
	{
		ServiciosCD40.TroncalesRuta tr = new ServiciosCD40.TroncalesRuta();

		tr.IdSistema = s.Value;
		tr.Central = TxtCentral.Text;
		tr.IdRuta = DDLRutas.SelectedValue;
		tr.IdTroncal = idTroncal;

        if (ServicioCD40.DeleteSQL(tr) < 0)
            logDebugView.Warn("(Encaminamientos-GuardarCambios): No se ha podido desasignar el troncal de la ruta");
        else
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
            if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1) && (InfoEnc != null))
            {
                InfoRuta r = InfoEnc.rutas.Find(delegate(InfoRuta i) { return i.idRuta == tr.IdRuta; });
                r.troncales.Remove(idTroncal);
            }
            //202006 JOI Error #4066
//            bModificacionAsignaciónTroncales = true;
        }

		ActualizaArbol();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idTroncal"></param>
	private void AsignaTroncal(string idTroncal)
	{
		ServiciosCD40.TroncalesRuta tr = new ServiciosCD40.TroncalesRuta();

		tr.IdSistema = s.Value;
		tr.Central = TxtCentral.Text;
		tr.IdRuta = DDLRutas.SelectedValue;
		tr.IdTroncal = idTroncal;

        if (ServicioCD40.InsertSQL(tr) < 0)
            logDebugView.Warn("(Encaminamientos-GuardarCambios): No se ha podido asignar el troncal a la ruta");
        else
        { 
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
            if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1) && (InfoEnc != null))
            {
                InfoRuta r = InfoEnc.rutas.Find(delegate(InfoRuta i) { return i.idRuta == tr.IdRuta; });
                r.troncales.Add(idTroncal);
            }
            //202006 JOI Error #4066
//            bModificacionAsignaciónTroncales = true;
        }
	
		ActualizaArbol();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void DListTipo_SelectedIndexChange(object sender, EventArgs e)
	{
		ServiciosCD40.Rutas r = new ServiciosCD40.Rutas();

		r.IdSistema = s.Value;
		r.IdRuta = DDLRutas.SelectedValue;
		r.Central = TxtCentral.Text;
		r.Tipo = DListTipo.SelectedValue;

		ServicioCD40.UpdateSQL(r);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void IBRutaArriba_Click(object sender, ImageClickEventArgs e)
	{
		if (LBRutas.SelectedIndex > 1)	// Sólo a partir de la segunda ruta alternativa se puede subir de nivel.
		{
			int insertarEn=LBRutas.SelectedIndex;
			ListItem liSelected = LBRutas.Items[LBRutas.SelectedIndex - 1];
			LBRutas.Items.RemoveAt(LBRutas.SelectedIndex - 1);
			LBRutas.Items.Insert(insertarEn, liSelected);
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void IBRutaAbajo_Click(object sender, ImageClickEventArgs e)
	{
		if (LBRutas.SelectedIndex > 0 && LBRutas.SelectedIndex < LBRutas.Items.Count - 1)	// Sólo las rutas alternativas se puede bajar de nivel menos la última.
		{
			int insertarEn = LBRutas.SelectedIndex;
			ListItem liSelected = LBRutas.Items[LBRutas.SelectedIndex + 1];
			LBRutas.Items.RemoveAt(LBRutas.SelectedIndex + 1);
			LBRutas.Items.Insert(insertarEn, liSelected);
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CBCentralPropia_OnCheckedChanged(object sender, EventArgs e)
    {


        //Si se activa el check de central propia y la central tiene rutas definidas, se informa al usuario que debe eliminar las rutas antes de activar el check
        if (CentralPropia.Checked)
        {
            if (DSRutas != null && DSRutas.Tables.Count > 0 && DSRutas.Tables[0].Rows.Count > 0)
            {
                LError.Text = GetLocalResourceObject("REV_CentralPropiaConRutas") != null ? GetLocalResourceObject("REV_CentralPropiaConRutas").ToString() : "El SCV propio no puede tener rutas configuradas. Por favor, elimine las rutas.";
                CentralPropia.Checked = false;
            }
            else
            {
                LError.Text = string.Empty;
                //Se limpian las direcciones IP de los servidores alternativos 
                //Porque, para la Central propia sólo se permite configurar el servidor proxy y de presencia principal
                TbIp2.Text = TbIp3.Text = string.Empty;
                TbSrvPresIp2.Text = TbSrvPresIp3.Text = string.Empty;
                bHabilitaControlesCentralIP(CBCentralIp.Checked, CentralPropia.Checked);

                IBRutaAbajo.Visible = IBRutaArriba.Visible = LblRutas.Visible = LBRutas.Visible = IBRutas.Visible = false;
                LblNumTest.Visible = TBNumTest.Visible = true;
            }
        }
        else
        {
            IBRutaAbajo.Visible = IBRutaArriba.Visible = LblRutas.Visible = LBRutas.Visible = IBRutas.Visible = !CentralPropia.Checked;
            LblNumTest.Visible = TBNumTest.Visible = CentralPropia.Checked;
            LError.Text = string.Empty;

            bHabilitaControlesCentralIP(CBCentralIp.Checked, CentralPropia.Checked);
        }
    }

    protected void CBCentralIP_OnCheckedChanged(object sender, EventArgs e)
    {
        string strIdSistema = (string)Session["idsistema"];
        string sTexto = string.Empty;

        if (!string.IsNullOrEmpty(LError.Text))
            LError.Text = string.Empty;

        //Si es una central IP  se habilitan los controles y validaciones de la tabla de direcciones. Si no lo es, se deshabilitan y oculta
        //la tabla de direcciones de los Proxy y los servidores de presencia

        bHabilitaControlesCentralIP(CBCentralIp.Checked, CentralPropia.Checked);
        TblCentralIp.Visible = CBCentralIp.Checked;
        if (Modificando || EncaminamientoDadoDeAlta)
        {
            //Si la central era Central IP y es utilizada por Destinos ATS configurados en el panel de LC, se informa al usuario que los destinos dejarán de funcionar si se quita el check.
            if (bEsCentralIP && CBCentralIp.Checked != bEsCentralIP && bCentralConDestinoATS_LC)
            {
                sTexto = string.Format((string)GetGlobalResourceObject("Espaniol", "AvisoGuardarCentralIP_DestinosATS_LC"), TxtCentral.Text);
            }
            else
            {
                sTexto = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            }

            BtAceptar_ConfirmButtonExtender.ConfirmText = sTexto;
        }
    }

	//#region Reescribir función virtual heredada desde PageBaseCD40
	//protected override void FinTransaccion()
	//{
	//    CancelarCambios();
	//}
	//#endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void OnPopulateNode(object sender, TreeNodeEventArgs e)
	{
		switch (e.Node.Depth)
		{
			case 0:
				GetRutas(e.Node);
				break;
			case 1:
				GetTroncales(e.Node);
				break;
		}

	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="treeNode"></param>
	private void GetTroncales(TreeNode treeNode)
	{
		ServiciosCD40.TroncalesRuta tr = new ServiciosCD40.TroncalesRuta();
		tr.IdSistema = (string)Session["idsistema"];
		tr.Central = ListBox1.SelectedValue;
		tr.IdRuta = treeNode.Text;

		DataSet dsTroncalesRutas = ServicioCD40.DataSetSelectSQL(tr);

		if (dsTroncalesRutas != null && dsTroncalesRutas.Tables.Count > 0)
		{
			foreach (DataRow dr in dsTroncalesRutas.Tables[0].Rows)
			{
				TreeNode newNode = new TreeNode((string)dr["IdTroncal"]);

				newNode.SelectAction = TreeNodeSelectAction.Select;
				newNode.ImageUrl = "~/Images/troncal.png";
				//newNode.PopulateOnDemand = true;
				treeNode.ChildNodes.Add(newNode);
			}
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="treeNode"></param>
	private void GetRutas(TreeNode treeNode)
	{
		ServiciosCD40.Rutas r = new ServiciosCD40.Rutas();
		r.IdSistema = (string)Session["idsistema"];
        r.Central = TxtCentral.Text;    // ListBox1.SelectedValue;

		DataSet dsRutas = ServicioCD40.DataSetSelectSQL(r);

		if (dsRutas != null && dsRutas.Tables.Count > 0)
		{
			foreach (DataRow dr in dsRutas.Tables[0].Rows)
			{
				TreeNode newNode = new TreeNode((string)dr["IdRuta"]);

				newNode.SelectAction = TreeNodeSelectAction.SelectExpand;
				newNode.ImageUrl = (string)dr["Tipo"] == "D" ? "~/Images/rutadirecta.png" : "~/Images/rutaalternativa.png";

				newNode.PopulateOnDemand = true;
				treeNode.ChildNodes.Add(newNode);
			}
		}
	}

    //Habilita o deshabilita los controles y las validaciones asociadas a la tabla de direcciones IP
    //que se pueden configurar para un SCV IP
    private void bHabilitaControlesCentralIP(bool bHabilitar, bool bEsCentralpropia)
    {
        if (bHabilitar)
        {
            //Si se configura como SCV IP se habilitan los controles que permiten introducir las direcciones 
            //de los servidores de Proxy y de presencia
            if (!bSBCEnaire)
            {
                //Si se configura como SCV IP se habilitan los controles que permiten introducir las direcciones 
                //de los servidores de Proxy y de presencia
                TbIp1.Enabled = true;
                TbSrvPresIp1.Enabled = true;

                if (bEsCentralpropia)
                {
                    //Para la Central propia sólo se permite configurar el servidor proxy y de presencia principal
                    TbIp2.Enabled = TbIp3.Enabled = false;
                    TbSrvPresIp2.Enabled = TbSrvPresIp3.Enabled = false;
                }
                else
                {
                    //Si no es un SCV IP no propio, se pueden configurar también los servidores proxy y de presencia alternativos
                    TbIp2.Enabled = TbIp3.Enabled = true;
                    TbSrvPresIp2.Enabled = TbSrvPresIp3.Enabled = true;
                }


                //Se debe informar como minimo el Proxy 1
                RFV_TbIp1.Enabled = true;
            }
            else
            {
                if (bEsCentralpropia)
                {
                    //Para la Central propia en SBC solo se permite la IP del DEP y la de presencia propia 
                    TbIp3.Enabled = true;
                    TbSrvPresIp3.Enabled = true;
                    TbIp1.Enabled = TbIp2.Enabled = false;
                    TbSrvPresIp1.Enabled = TbSrvPresIp2.Enabled = false;
                }
                else
                {
                    //Si no es un SCV IP propio, se deben configurar también los servidores SBC Principal, SBC Alternativo SCV y SCV DEP
                    TbIp1.Enabled = TbIp2.Enabled = TbIp3.Enabled = true;
                    TbSrvPresIp1.Enabled = TbSrvPresIp2.Enabled = TbSrvPresIp3.Enabled = true;
                }
                //Se debe informar como minimo el Proxy 1
                RFV_TbIp1.Enabled = false;
            }
        }
        else
        {
            //Si no se configura como SCV IP se deshabilitan
            RFV_TbIp1.Enabled = false;

            //Los controles que permiten introducir de las direcciones  de los servidores de Proxy
            TbIp1.Enabled = TbIp2.Enabled = TbIp3.Enabled = false;
            //Los controles que permiten introducir de las direcciones  de los servidores de presencia
            TbSrvPresIp1.Enabled = TbSrvPresIp2.Enabled = TbSrvPresIp3.Enabled = false;
        }
    }

    //Devuelve true si la dirección IP que se le pasa como cadena tiene un formato correcto.
    //El formato puede ser a.b.c.d ó a.b.c.d:xxxx donde a.b.c.d corresponde a la dirección IP v4 y xxxx al puerto.El puerto puede tomar un valor entre 0 y 65535
    // En el parámetro de salida strDirconPuerto, se devuelve: si la ip se ha configurado con puerto, la dirección IP. Si no se ha configurado con puerto la dirección IP:5060
    private bool bEsDirIPvalida(string strDireccion,ref string strDirconPuerto)
    {
        bool bCorrecto = false;
        const int iPuertoDefecto = 5060;

        string STR_FORMATO_IP = "^(([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\\.){3}(([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))$";
        //string STR_FORMATO_IP_PUERTO = "^(([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\\.){3}(([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))\\:(([0-9]{1,4}))$";
        //string STR_FORMATO_IP_PUERTO = "^(([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\\.){3}(([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))\\:(([0-9]|[1-9][0-9]|[1-9][0-9][0-9]|[1-9][0-9][0-9][0-9]))$";
        string STR_FORMATO_IP_PUERTO = "^(([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\\.){3}(([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))\\:(([0-9]|[1-9][0-9]|[1-9][0-9][0-9]|[1-9][0-9][0-9][0-9]|6553[0-5]|655[0-2][0-9]|65[0-4][0-9]{2}|6[0-4][0-9]{3}|[0-5]))$";

        //Comprobamos que el formato sea el de una dirección IP V4 más el puerto separado por el carácter dos puntos.
        if (System.Text.RegularExpressions.Regex.IsMatch(strDireccion, @STR_FORMATO_IP))
        {
            bCorrecto = true;
            strDirconPuerto = string.Format("{0}:{1}", strDireccion, iPuertoDefecto);
        }
        else if (System.Text.RegularExpressions.Regex.IsMatch(strDireccion, STR_FORMATO_IP_PUERTO))
        {
            bCorrecto = true;
            strDirconPuerto = strDireccion;
        }

        return bCorrecto;
    }

    private bool bDireccionesCentralIPValidas(string strSistema,string strNombreCentral, ref string strMsg)
    {
        //Se comprueba si las direcciones IP configuradas cuando se configura un SCV IP son válidas
        //Como minimo se debe configurar la dirección Ip del proxy 1. Las dirección ip del servidor proxy i puede ser igual a la dirección IP del servidor de presencia i de la misma central.
        //En el resto de casos, a IP no se puede repetir.
        // Se debe invocar si CBCentralIp.Checked=true
        bool bCorrecto=true;
        string strIP=string.Empty;

        int i=0;
        
        const int iNumSrvProxy=3;
        const int iNumMaxSrv=iNumSrvProxy*2;

        string [] arrIpConPuerto=new string[iNumSrvProxy];
        string strAuxIpConPuerto=string.Empty;

        Dictionary<string, string> listaSCVIP = new Dictionary<string, string>(); //Contiene la lista de IP donde se almacena por cada IP:puerto la ip original

        for (i = 0; (i < iNumMaxSrv) && bCorrecto; i++)
        {
            switch(i)
            {
                case 0: //Srv. Proxy Principal
                    strIP=TbIp1.Text;

                    if (string.IsNullOrEmpty(strIP))
                    {
                        bCorrecto=false;
                        //Si no está configurada la dirección IP del GateWay 1 devolvemos error y no continuamos
                        if (GetLocalResourceObject("REV_TbIp1Requerido") != null)
                            strMsg = GetLocalResourceObject("REV_TbIp1Requerido").ToString();
                        else
                            strMsg="La dirección IP del Proxy principal no puede estar vacía.";
                    }
                    else if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                    {
                        bCorrecto = false;
                        if (GetLocalResourceObject("REV_TbProxyIpFormatoPrincipal") != null)
                            strMsg = GetLocalResourceObject("REV_TbProxyIpFormatoPrincipal").ToString();
                        else
                            strMsg = "Formato incorrecto de la dirección IP del Proxy principal.";
                    }
                    else
                    {
                        arrIpConPuerto[i] = strAuxIpConPuerto;

                        listaSCVIP.Add(strAuxIpConPuerto, strIP);
                    }
                    break;
                case 1: //Srv. Proxy alternativo 1
                    strIP=TbIp2.Text;

                    if (!string.IsNullOrEmpty(strIP))
                    {
                        strAuxIpConPuerto = string.Empty;
                        if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                        {
                            bCorrecto = false;
                            if (GetLocalResourceObject("REV_TbProxyIpFormatoAlternativo") != null)
                                strMsg = string.Format(GetLocalResourceObject("REV_TbProxyIpFormatoAlternativo").ToString());
                            else
                                strMsg = string.Format("Formato incorrecto de la dirección IP del Proxy alternativo {0}", i);
                        }
                        else
                        {
                            arrIpConPuerto[i] = strAuxIpConPuerto;

                            if (!listaSCVIP.ContainsKey(strAuxIpConPuerto))
                            {
                                listaSCVIP.Add(strAuxIpConPuerto, strIP);
                            }
                            else
                            {
                                bCorrecto = false;
                                //La dirección IP del servidor Proxy Alternativo 1 ya está asignada a otro servidor. Revise la configuración
                                if (GetLocalResourceObject("REV_IpSrvProxyAltEnOtraCentral") != null)
                                    strMsg = string.Format(GetLocalResourceObject("REV_IpSrvProxyAltEnOtraCentral").ToString(), 1);
                                else
                                    strMsg = string.Format("La dirección IP del servidor Proxy Alternativo {0} ya está configurada en otro servidor. Revise la configuración.", 1);
                            }
                        }
                    }
                    break;
                case 2: //Srv. Proxy alternativo 2
                    strIP=TbIp3.Text;

                    if (!string.IsNullOrEmpty(strIP))
                    {
                        strAuxIpConPuerto = string.Empty;

                        if (string.IsNullOrEmpty(TbIp2.Text))
                        {
                            bCorrecto = false;
                            //No se puede configurar el servidor proxy alternativo 2 sin haber configurado previamente el servidor proxy alternativo 1
                            if (GetLocalResourceObject("REV_TbSrvIpProxyAlternativo") != null)
                                strMsg = GetLocalResourceObject("REV_TbSrvIpProxyAlternativo").ToString();
                            else
                                strMsg = "Si desea configurar varios servidores proxy alternativos, debe configurar primero el servidor alternativo 1";
                        }
                        else
                        {
                            if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                            {
                                bCorrecto = false;
                                if (GetLocalResourceObject("REV_TbProxyIpFormatoAlternativo") != null)
                                    strMsg = string.Format(GetLocalResourceObject("REV_TbProxyIpFormatoAlternativo").ToString(), i);
                                else
                                    strMsg = string.Format("Formato incorrecto de la dirección IP del Proxy alternativo {0}", i);
                            }
                            else
                            {
                                arrIpConPuerto[i] = strAuxIpConPuerto;

                                if (!listaSCVIP.ContainsKey(strAuxIpConPuerto))
                                {
                                    listaSCVIP.Add(strAuxIpConPuerto, strIP);
                                }
                                else
                                {
                                    bCorrecto = false;
                                    if (GetLocalResourceObject("REV_IpSrvProxyAltEnOtraCentral") != null)
                                        strMsg = string.Format(GetLocalResourceObject("REV_IpSrvProxyAltEnOtraCentral").ToString(), 2);
                                    else
                                        strMsg = string.Format("La dirección IP del servidor Proxy Alternativo {0} ya está configurada en otro servidor. Revise la configuración.", 2);
                                }
                            }
                        }
                    }
                    break;
                case 3: //Dir Servidor de presencia principal
                    strIP=TbSrvPresIp1.Text;

                    if (!string.IsNullOrEmpty(strIP))
                    {
                        if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                        {
                            bCorrecto = false;
                            if (GetLocalResourceObject("REV_TbSrvPresIpFormatoPrincipal") != null)
                                strMsg = GetLocalResourceObject("REV_TbSrvPresIpFormatoPrincipal").ToString();
                            else
                                strMsg = "Formato incorrecto de la dirección IP del Servidor de Presencia principal.";
                        }
                        else
                        {
                            //Si la IP del proxy y el servidor de presencia principal son distintas la añadimos a la lista
                            //Hay que tener si la IP tiene o no puerto configurada y si es el puerto por defecto

                            if (string.Compare(arrIpConPuerto[i%iNumSrvProxy], strAuxIpConPuerto) != 0)
                            {
                                if (!listaSCVIP.ContainsKey(strAuxIpConPuerto))
                                    listaSCVIP.Add(strAuxIpConPuerto, strIP);
                                else
                                {
                                    bCorrecto = false;
                                    //La dirección IP del servidor de presencia principal ya está configurado
                                    if (GetLocalResourceObject("REV_IpSrvPresenciaPrincipalEnOtraCentral") != null)
                                        strMsg = GetLocalResourceObject("REV_IpSrvPresenciaPrincipalEnOtraCentral").ToString();
                                    else
                                        strMsg = "La dirección IP del servidor de Presencia principal ya está configurada.";
                                }
                            }
                        }
                    }
                    break;
                case 4: //Dir Servidor de presencia alternativo 1
                    strIP=TbSrvPresIp2.Text;
                    if (!string.IsNullOrEmpty(strIP))
                    {
                        if (string.IsNullOrEmpty(TbIp2.Text))
                        {
                            bCorrecto = false;
                            //Si se configura el servidor alternativo de presencia  y no se configura el Proxy alternativo correspondiente se informa del error
                            if (GetLocalResourceObject("REV_TbSrvPresIpAlternativoProxy") != null)
                                strIP = GetLocalResourceObject("REV_TbSrvPresIpAlternativoProxy").ToString();
                            else
                                strIP = "El sevidor de presencia alternativo {0}, se puede configurar si configura el Proxy alternativo {0} ";

                            strMsg = string.Format(strIP, 1);
                        }
                        else if (string.IsNullOrEmpty(TbSrvPresIp1.Text))
                        {
                            bCorrecto = false;
                            //No se puede configurar el servidor de presencia alternativo 1 si previamente no se configura el servidor de presencia principal
                            if (GetLocalResourceObject("REV_SrvIpPresenciaPrincipal") != null)
                                strMsg = GetLocalResourceObject("REV_SrvIpPresenciaPrincipal").ToString();
                            else
                                strMsg = "Si desea configurar sólo un servidor de presencia, se debe configurar el servidor de presencia principal";
                        }
                        else if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                        {
                            bCorrecto = false;
                            if (GetLocalResourceObject("REV_TbProxyIpFormatoAlternativo") != null)
                                strMsg = string.Format(GetLocalResourceObject("REV_TbSrvPresIpFormatoAlternativo").ToString(), 1);
                            else
                                strMsg = string.Format("Formato incorrecto de la dirección IP del Servidor de Presencia alternativo {0}.", 1);
                        }
                        else
                        {
                            //Si la IP del proxy y el servidor de presencia principal son distintas la añadimos a la lista
                            //Hay que tener si la IP tiene o no puerto configurada y si es el puerto por defecto

                            if (string.Compare(arrIpConPuerto[i%iNumSrvProxy], strAuxIpConPuerto) != 0)
                            {
                                if (!listaSCVIP.ContainsKey(strAuxIpConPuerto))
                                    listaSCVIP.Add(strAuxIpConPuerto, strIP);
                                else
                                {
                                    bCorrecto = false;
                                    //La dirección IP del servidor de presencia Alternativo 1 ya está asignada a otro servidor.
                                    if (GetLocalResourceObject("REV_IpSrvPresenciaAltEnOtraCentral") != null)
                                        strMsg = string.Format(GetLocalResourceObject("REV_IpSrvPresenciaAltEnOtraCentral").ToString(), 1);
                                    else
                                        strMsg = string.Format("La dirección IP del servidor de Presencia Alternativo {0} ya está configurada.", 1);
                                }
                            }
                        }
                    }
                    break;
                case 5: //Dir Servidor de presencia alternativo 2
                    strIP=TbSrvPresIp3.Text;
                    if (!string.IsNullOrEmpty(strIP))
                    {
                        if (string.IsNullOrEmpty(TbIp3.Text))
                        {
                            bCorrecto = false;
                            //Si se configura el servidor alternativo de presencia  y no se configura el Proxy alternativo correspondiente
                            // Se informa del error
                            if (GetLocalResourceObject("REV_TbSrvPresIpAlternativoProxy") != null)
                                strIP = GetLocalResourceObject("REV_TbSrvPresIpAlternativoProxy").ToString();
                            else
                                strIP = "El sevidor de presencia alternativo {0}, se puede configurar si configura el Proxy alternativo {0} ";

                            strMsg = string.Format(strIP, 2);
                        }
                        else if (string.IsNullOrEmpty(TbSrvPresIp1.Text))
                        {
                            bCorrecto = false;
                            //No se puede configurar el servidor de presencia alternativo 1 si previamente no se configura el servidor de presencia principal
                            if (GetLocalResourceObject("REV_SrvIpPresenciaPrincipal") != null)
                                strMsg = GetLocalResourceObject("REV_SrvIpPresenciaPrincipal").ToString();
                            else
                                strMsg = "Si desea configurar sólo un servidor de presencia, se debe configurar el servidor de presencia principal";
                        }
                        else
                        {
                            if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                            {
                                bCorrecto = false;
                                if (GetLocalResourceObject("REV_TbProxyIpFormatoAlternativo") != null)
                                    strMsg = string.Format(GetLocalResourceObject("REV_TbProxyIpFormatoAlternativo").ToString(), 2);
                                else
                                    strMsg = string.Format("Formato incorrecto de la dirección IP del Proxy alternativo {0}.", 2);
                            }
                            else
                            {
                                //Si la IP del proxy y el servidor de presencia principal son distintas la añadimos a la lista

                                if (string.Compare(arrIpConPuerto[i%iNumSrvProxy], strAuxIpConPuerto) != 0)
                                {
                                    if (!listaSCVIP.ContainsKey(strAuxIpConPuerto))
                                        listaSCVIP.Add(strAuxIpConPuerto, strIP);
                                    else
                                    {
                                        bCorrecto = false;
                                        //La dirección IP del servidor de presencia Alternativo 2 ya está asignada a otro servidor. Revise la configuración
                                        if (GetLocalResourceObject("REV_IpSrvPresenciaAltEnOtraCentral") != null)
                                            strMsg = string.Format(GetLocalResourceObject("REV_IpSrvPresenciaAltEnOtraCentral").ToString(), 2);
                                        else
                                            strMsg = string.Format("La dirección IP del servidor de Presencia Alternativo {0} ya está configurada en otro servidor. Revise la configuración.", 2);
                                    }
                                }
                            }
                        }
                        
                    }
                    break;
                default:
                    break;
            }
        } //for

        //Se comprueba si existe alguna central ATS con alguna de las direcciones IP configuradas
        if (bCorrecto && !DireccionesSCVIP_Libres(strSistema, strNombreCentral,ref listaSCVIP,ref strMsg))
        {
            bCorrecto=false;
        }

        listaSCVIP.Clear();

        return bCorrecto;
    }

    private bool bDireccionesCentralIPValidasSBC(string strSistema, string strNombreCentral, ref string strMsg, bool bCentralPropia)
    {
        //Se comprueba si las direcciones IP configuradas cuando se configura un SCV IP son válidas
        //Como minimo se debe configurar la dirección Ip del SBC principal o SBC Alternativo o la dirección del SCV Dependencia 
        //Las dirección ip del servidor proxy i puede ser igual a la dirección IP del servidor de presencia i de la misma central.
        //Las direcciones SBC Principal pueden repetirse al igual que las de SBC Alternativo.
        //Las direcciones SCV Departamento no pueden estar duplicadas ni estar en SBC´s
        //En el resto de casos, la IP no se puede repetir.
        // Se debe invocar si CBCentralIp.Checked=true
        bool bCorrecto = true;
        string strIP = string.Empty;

        int i = 0;

        const int iNumSrvProxy = 3;
        const int iNumMaxSrv = iNumSrvProxy * 2;

        string[] arrIpConPuerto = new string[iNumSrvProxy];
        string strAuxIpConPuerto = string.Empty;

        Dictionary<string, string> listaSCVIP = new Dictionary<string, string>(); //Contiene la lista de IP donde se almacena por cada IP:puerto la ip original
        if (string.IsNullOrEmpty(TbIp1.Text) && string.IsNullOrEmpty(TbIp2.Text) && string.IsNullOrEmpty(TbIp3.Text))
        {
            bCorrecto = false;
            if (GetLocalResourceObject("REV_TbIpsRequeridasSBC") != null)
                strMsg = GetLocalResourceObject("REV_TbIpsRequeridasSBC").ToString();
            else
                strMsg = "Debe introducir al menos una dirección IP de servidor proxy.";
        }
        else
        {
            for (i = 0; (i < iNumMaxSrv) && bCorrecto; i++)
            {
                switch (i)
                {
                    case 0: //Servidor SBC Principal
                        strIP = TbIp1.Text;
                        if (!string.IsNullOrEmpty(strIP))
                        {
                            strAuxIpConPuerto = string.Empty;
                            if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                            {
                                bCorrecto = false;
                                if (GetLocalResourceObject("REV_TbProxyIpFormatoPrincipalSBC") != null)
                                    strMsg = GetLocalResourceObject("REV_TbProxyIpFormatoPrincipalSBC").ToString();
                                else
                                    strMsg = "Formato incorrecto de la dirección IP del Proxy SBC PPAL.";
                            }
                            else
                            {
                                arrIpConPuerto[i] = strAuxIpConPuerto;
                                listaSCVIP.Add(strAuxIpConPuerto, strIP);
                            }
                        }
                        break;
                    case 1: //Servidor SBC Alternativo
                        strIP = TbIp2.Text;
                        if (!string.IsNullOrEmpty(strIP))
                        {
                            strAuxIpConPuerto = string.Empty;
                            if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                            {
                                bCorrecto = false;
                                if (GetLocalResourceObject("REV_TbProxyIpFormatoAlternativoSBC") != null)
                                    strMsg = string.Format(GetLocalResourceObject("REV_TbProxyIpFormatoAlternativoSBC").ToString(), i);
                                else
                                    strMsg = string.Format("Formato incorrecto de la dirección IP del Proxy SBC ALTER.");
                            }
                            else
                            {
                                arrIpConPuerto[i] = strAuxIpConPuerto;

                                if (!listaSCVIP.ContainsKey(strAuxIpConPuerto))
                                {
                                    listaSCVIP.Add(strAuxIpConPuerto, strIP);
                                }
                                else
                                {
                                    bCorrecto = false;
                                    if (GetLocalResourceObject("REV_IpSBCALTnOtroServidor") != null)
                                        strMsg = string.Format(GetLocalResourceObject("REV_IpSBCALTnOtroServidor").ToString());
                                    else
                                        strMsg = string.Format("La dirección IP del servidor SBC ALTER. ya está configurada en otro servidor. Revise la configuración.");
                                }
                            }

                        }
                        break;
                    case 2: //Servidor SCV Dependencia
                        strIP = TbIp3.Text;
                        strAuxIpConPuerto = string.Empty;
                        if (!string.IsNullOrEmpty(strIP))
                        {
                            if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                            {
                                bCorrecto = false;
                                if (GetLocalResourceObject("REV_TbIpFormatoSCVDep") != null)
                                    strMsg = string.Format(GetLocalResourceObject("REV_TbIpFormatoSCVDep").ToString());
                                else
                                    strMsg = string.Format("Formato incorrecto de la dirección IP del SCV DEP.");
                            }
                            else
                            {
                                arrIpConPuerto[i] = strAuxIpConPuerto;
                                sSCV_DEP = strAuxIpConPuerto;
                                if (!listaSCVIP.ContainsKey(strAuxIpConPuerto))
                                {
                                    listaSCVIP.Add(strAuxIpConPuerto, strIP);
                                }
                                else
                                {
                                    bCorrecto = false;
                                    if (GetLocalResourceObject("REV_IpSCVDepEnOtroServidor") != null)
                                        strMsg = string.Format(GetLocalResourceObject("REV_IpSCVDepEnOtroServidor").ToString());
                                    else
                                        strMsg = string.Format("La dirección IP del servidor SCV DEP ya está configurada en otro servidor. Revise la configuración.");
                                }
                            }
                        }
                        break;
                    case 3: //Dir Servidor de presencia SBC principal
                        strIP = TbSrvPresIp1.Text;
                        if (!string.IsNullOrEmpty(strIP))
                        {
                            if (string.IsNullOrEmpty(TbIp1.Text))
                            {
                                bCorrecto = false;
                                //Si se configura el servidor SBC alternativo de presencia  y no se configura el Proxy SBC alternativo correspondiente se informa del error
                                if (GetLocalResourceObject("REV_TbSrvPresIpSBCPrincipal") != null)
                                    strIP = GetLocalResourceObject("REV_TbSrvPresIpSBCPrincipal").ToString();
                                else
                                    strIP = "El servidor de presencia SBC principal, solo se puede configurar si configura el SBC PPAL.";

                                strMsg = strIP;
                            }
                            else
                            if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                            {
                                bCorrecto = false;
                                if (GetLocalResourceObject("REV_TbSrvPresIpFormatoSBCPrincipal") != null)
                                    strMsg = GetLocalResourceObject("REV_TbSrvPresIpFormatoSBCPrincipal").ToString();
                                else
                                    strMsg = "Formato incorrecto de la dirección IP del Servidor de Presencia SBC PPAL.";
                            }
                            else
                            {
                                //Si la IP del proxy y el servidor de presencia principal son distintas la añadimos a la lista
                                //Hay que tener si la IP tiene o no puerto configurada y si es el puerto por defecto
                                if (string.Compare(arrIpConPuerto[i % iNumSrvProxy], strAuxIpConPuerto) != 0)
                                {
                                    if (!listaSCVIP.ContainsKey(strAuxIpConPuerto))
                                        listaSCVIP.Add(strAuxIpConPuerto, strIP);
                                    else
                                    {
                                        bCorrecto = false;
                                        //La dirección IP del servidor de presencia principal ya está configurado
                                        if (GetLocalResourceObject("REV_IpSrvPresenciaSBCPrincipalEnOtraCentral") != null)
                                            strMsg = GetLocalResourceObject("REV_IpSrvPresenciaSBCPrincipalEnOtraCentral").ToString();
                                        else
                                            strMsg = "La dirección IP del servidor de Presencia SBC PPAL. ya está configurada.";
                                    }
                                }
                            }
                        }
                        break;
                    case 4: //Dir Servidor de presencia SBC alternativo
                        strIP = TbSrvPresIp2.Text;
                        if (!string.IsNullOrEmpty(strIP))
                        {
                            if (string.IsNullOrEmpty(TbIp2.Text))
                            {
                                bCorrecto = false;
                                //Si se configura el servidor SBC alternativo de presencia  y no se configura el Proxy SBC alternativo correspondiente se informa del error
                                if (GetLocalResourceObject("REV_TbSrvPresIpSBCAlternativo") != null)
                                    strIP = GetLocalResourceObject("REV_TbSrvPresIpSBCAlternativo").ToString();
                                else
                                    strIP = "El servidor de presencia SBC ALTER., solo se puede configurar si configura el SBC ALTER.";

                                strMsg = strIP;
                            }
                            else if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                            {
                                bCorrecto = false;
                                if (GetLocalResourceObject("REV_TbIpFormatoSBCAlternativo") != null)
                                    strMsg = string.Format(GetLocalResourceObject("REV_TbIpFormatoSBCAlternativo").ToString(), 1);
                                else
                                    strMsg = string.Format("Formato incorrecto de la dirección IP del Servidor de Presencia SBC ALTER.");
                            }
                            else
                            {
                                //Si la IP del proxy y el servidor de presencia principal son distintas la añadimos a la lista
                                //Hay que tener si la IP tiene o no puerto configurada y si es el puerto por defecto

                                if (string.Compare(arrIpConPuerto[i % iNumSrvProxy], strAuxIpConPuerto) != 0)
                                {
                                    if (!listaSCVIP.ContainsKey(strAuxIpConPuerto))
                                        listaSCVIP.Add(strAuxIpConPuerto, strIP);
                                    else
                                    {
                                        bCorrecto = false;
                                        //La dirección IP del servidor de presencia SBC Alternativo ya está asignada a otro servidor.
                                        if (GetLocalResourceObject("REV_IpSrvPresenciaSBCAltEnOtraCentral") != null)
                                            strMsg = string.Format(GetLocalResourceObject("REV_IpSrvPresenciaSBCAltEnOtraCentral").ToString(), 1);
                                        else
                                            strMsg = string.Format("La dirección IP del servidor de Presencia SBC ALTER. ya está configurada.");
                                    }
                                }
                            }
                        }
                        break;
                    case 5: //Dir Servidor de presencia SCV DEP
                        strIP = TbSrvPresIp3.Text;
                        if (!string.IsNullOrEmpty(strIP))
                        {
                            
                            if (string.IsNullOrEmpty(TbIp3.Text))
                            {
                                bCorrecto = false;
                                //Si se configura el servidor SCV DEP de presencia  y no se configura el SCV DEP correspondiente
                                // Se informa del error
                                if (GetLocalResourceObject("REV_TbSrvPresSCVDEPSinSCVDEP") != null)
                                    strIP = GetLocalResourceObject("REV_TbSrvPresSCVDEPSinSCVDEP").ToString();
                                else
                                    strIP = "El sevidor de presencia SCV DEP, solo se puede configurar si configura el SCV DEP";

                                strMsg = strIP;
                            }
                            else
                            {
                                if (!bEsDirIPvalida(strIP, ref strAuxIpConPuerto))
                                {
                                    bCorrecto = false;
                                    if (GetLocalResourceObject("REV_TbIPFormatoSCVDEP") != null)
                                        strMsg = string.Format(GetLocalResourceObject("REV_TbIPFormatoSCVDEP").ToString(), 2);
                                    else
                                        strMsg = string.Format("Formato incorrecto de la dirección IP del servidor de presencia del SCV DEP");
                                }
                                else
                                {
                                    //Si la IP del proxy y el servidor SVC DEP de presencia  son distintas la añadimos a la lista
                                    sSCV_DEP_P = strAuxIpConPuerto;
                                    if (string.Compare(arrIpConPuerto[i % iNumSrvProxy], strAuxIpConPuerto) != 0)
                                    {
                                        if (!listaSCVIP.ContainsKey(strAuxIpConPuerto))
                                        {
                                            listaSCVIP.Add(strAuxIpConPuerto, strIP);
                                        }
                                        else
                                        {
                                            bCorrecto = false;
                                            //La dirección IP del servidor de presencia SCV DEP  ya está asignada a otro servidor. Revise la configuración
                                            if (GetLocalResourceObject("REV_IpSrvPresenciaSCVDEPEnOtraCentral") != null)
                                                strMsg = string.Format(GetLocalResourceObject("REV_IpSrvPresenciaSCVDEPEnOtraCentral").ToString(), 2);
                                            else
                                                strMsg = string.Format("La dirección IP del servidor de Presencia SCV DEP ya está configurada en otro servidor. Revise la configuración.");
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            } //for
         }

        //Se comprueba si existe alguna central ATS con alguna de las direcciones IP configuradas
        if (bCorrecto && !DireccionesSCVIP_Libres(strSistema, strNombreCentral, ref listaSCVIP, ref strMsg))
        {
            bCorrecto = false;
        }

        listaSCVIP.Clear();

        return bCorrecto;
    }



	//private void GetCentrales(TreeNode treeNode)
	//{
	//    ServiciosCD40.Encaminamientos e = new ServiciosCD40.Encaminamientos();
	//    e.IdSistema = (string)Session["idsistema"];

	//    DataSet dsCentrales = ServicioCD40.DataSetSelectSQL(e);

	//    if (dsCentrales != null)
	//    {
	//        foreach (DataRow dr in dsCentrales.Tables[0].Rows)
	//        {
	//            TreeNode newNode = new TreeNode((string)dr["Central"]);

	//            newNode.SelectAction = TreeNodeSelectAction.SelectExpand;
	//            newNode.PopulateOnDemand = true;
	//            newNode.ImageUrl = "~/Images/central.png";
	//            treeNode.ChildNodes.Add(newNode);
	//        }
	//    }
	//}

    //Devuelve true si la Central tiene asociado algún recurso de telefonía como equipo externo
    //Esta función se debe llamar si la central es un SCV IP. 
    private bool bCentralConRecursoTlfAsignado(string strIdSistema,string strCentral)
    {
        bool bExiste = false;
        const uint TIPO_RECURSO_TELEFONIA=1;

        try
        {
            if (!string.IsNullOrEmpty(strCentral))
            {
                ServiciosCD40.Recursos objRec = new ServiciosCD40.Recursos();
                objRec.IdSistema = strIdSistema;
                objRec.IdEquipo = strCentral;

                ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(objRec);

                if (d != null && d.Length > 0)
                {
                    for (int i = 0; i < d.Length && !bExiste; i++)
                    {
                        if (((ServiciosCD40.Recursos)d[i]).TipoRecurso == TIPO_RECURSO_TELEFONIA)
                        {
                            bExiste = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error(string.Format("(Encaminamientos-bCentralConRecursoTlfAsignado): Error al obtener los recursos asignados al SCV {0}. Error: {1}", strCentral,ex.Message));

        }

        return (bExiste);
    }

    private bool DireccionesSCVIP_Libres(string strIdSistema, string strIdCentral,ref Dictionary<string, string> plistaIPCentral, ref string strMsg)
    {
        //Comprueba si las direcciones de los servidores proxy y de presencia  del SCV que se está configurando, 
        //no están asignadas a otro SCV IP
        // La lista de direcciones plistaIPCentral va siempre con el puerto. Si en la BD, la dirección no tiene puerto, se el añade el puerto por defecto para poder hacer la comparación

        bool bOk=true;
        string strAuxIpConPuerto=string.Empty;

        if (null != plistaIPCentral && plistaIPCentral.Count > 0)
        {
            ServiciosCD40.EquiposEU eq = new ServiciosCD40.EquiposEU();
            eq.IdSistema = strIdSistema;

            //Indicamos que queremos obtener los equipos externos asociados a centrales ATS IP
            eq.bObtenerEquiposCentralIp = true;

            //Comprobamos si las direcciones IP están asignadas a otros SCV IP
            //Para ello, se carga en un mapa la lista de direcciones IP configuradas en los SCV IPs del sistema.
            // si la IP no tiene el puerto se le añade el puerto por defecto para poder comparar
            ServiciosCD40.Tablas[] objListaEquiposCentralIP = ServicioCD40.ListSelectSQL(eq);
            if (objListaEquiposCentralIP != null && objListaEquiposCentralIP.Length > 0)
            {

                Dictionary<string, string> listaSrv = new Dictionary<string, string>();

                Dictionary<string, string> listaSrvSBC = new Dictionary<string, string>();

                for (int i=0; i<objListaEquiposCentralIP.Length; i++)
                {
                    //Se carga la listas con las direcciones IP para comprobar si las nuevas están o no asignadas.
                    ServiciosCD40.EquiposEU objEquipo= (ServiciosCD40.EquiposEU) objListaEquiposCentralIP[i];

                    //Si no se trata de la central que estamos configurando
                    if (string.Compare(objEquipo.IdEquipos,strIdCentral)!=0)
                    {
                        if (bSBCEnaire && objEquipo.Interno)
                        {
                            if (!string.IsNullOrEmpty(objEquipo.IpRed1))
                            {
                                if (bEsDirIPvalida(objEquipo.IpRed1, ref strAuxIpConPuerto))
                                {
                                    if (!listaSrvSBC.ContainsKey(strAuxIpConPuerto))
                                        listaSrvSBC.Add(strAuxIpConPuerto, objEquipo.IdEquipos);
                                }

                                if (string.Compare(objEquipo.IpRed1, objEquipo.SrvPresenciaIpRed1) != 0)
                                {
                                    if (bEsDirIPvalida(objEquipo.SrvPresenciaIpRed1, ref strAuxIpConPuerto))
                                    {
                                        if (!listaSrvSBC.ContainsKey(strAuxIpConPuerto))
                                            listaSrvSBC.Add(strAuxIpConPuerto, objEquipo.IdEquipos);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(objEquipo.IpRed1))
                            {
                                if (bEsDirIPvalida(objEquipo.IpRed1, ref strAuxIpConPuerto))
                                {
                                    if (!listaSrv.ContainsKey(strAuxIpConPuerto))
                                        listaSrv.Add(strAuxIpConPuerto, objEquipo.IdEquipos);
                                }
                                if (string.Compare(objEquipo.IpRed1,objEquipo.SrvPresenciaIpRed1)!=0)
                                {
                                    if (bEsDirIPvalida(objEquipo.SrvPresenciaIpRed1, ref strAuxIpConPuerto))
                                    {
                                        if (!listaSrv.ContainsKey(strAuxIpConPuerto))
                                            listaSrv.Add(strAuxIpConPuerto, objEquipo.IdEquipos);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(objEquipo.IpRed2))
                            {
                                if (bEsDirIPvalida(objEquipo.IpRed2, ref strAuxIpConPuerto))
                                {
                                    if (!listaSrv.ContainsKey(strAuxIpConPuerto))
                                        listaSrv.Add(strAuxIpConPuerto, objEquipo.IdEquipos);
                                }

                                if (string.Compare(objEquipo.IpRed2,objEquipo.SrvPresenciaIpRed2)!=0)
                                {
                                    if (bEsDirIPvalida(objEquipo.SrvPresenciaIpRed2, ref strAuxIpConPuerto))
                                    {
                                        if (!listaSrv.ContainsKey(strAuxIpConPuerto))
                                            listaSrv.Add(strAuxIpConPuerto, objEquipo.IdEquipos);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(objEquipo.IpRed3))
                            {
                                if (bEsDirIPvalida(objEquipo.IpRed3, ref strAuxIpConPuerto))
                                {
                                    if (!listaSrv.ContainsKey(strAuxIpConPuerto))
                                        listaSrv.Add(strAuxIpConPuerto, objEquipo.IdEquipos);
                                    if (bSBCEnaire)
                                    {
                                        if (!listaSrvSBC.ContainsKey(strAuxIpConPuerto))
                                            listaSrvSBC.Add(strAuxIpConPuerto, objEquipo.IdEquipos);
                                    }
                                }
                                if (string.Compare(objEquipo.IpRed3, objEquipo.SrvPresenciaIpRed3) != 0)
                                {
                                    if (bEsDirIPvalida(objEquipo.SrvPresenciaIpRed3, ref strAuxIpConPuerto))
                                    {
                                        if (!listaSrv.ContainsKey(strAuxIpConPuerto))
                                            listaSrv.Add(strAuxIpConPuerto, objEquipo.IdEquipos);
                                        if (bSBCEnaire)
                                        {
                                            if (!listaSrvSBC.ContainsKey(strAuxIpConPuerto))
                                                listaSrvSBC.Add(strAuxIpConPuerto, objEquipo.IdEquipos);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string [] arrListaIP=new string[plistaIPCentral.Count];
                string strAux = string.Empty;

                plistaIPCentral.Keys.CopyTo(arrListaIP,0);

                //Se comprueba si las direcciones IP configuradas no estan asignadas a otras centrales
                 for (int i=0; i<arrListaIP.Length && bOk;i++)
                 {
                     strAux = arrListaIP[i];
                     //20200928 #4599
                     if (bSBCEnaire)
                     {
                         if (!string.IsNullOrEmpty(sSCV_DEP) || !string.IsNullOrEmpty(sSCV_DEP_P))
                         {
                             if (!string.IsNullOrEmpty(sSCV_DEP))
                             {
                                 if (strAux == sSCV_DEP && bExisteSCVEnSBC(strAux, ref listaSrv, ref strMsg))
                                 {
                                     bOk = false;
                                     ErrorSCVExiste(plistaIPCentral[strAux], listaSrv[strAux], ref strMsg);
                                     continue;
                                 }
                                 if (strAux == sSCV_DEP && bExisteSCVEnSCV(strAux, ref listaSrvSBC, ref strMsg))
                                 {
                                     bOk = false;
                                     ErrorSCVExiste(plistaIPCentral[strAux], listaSrvSBC[strAux], ref strMsg);
                                     continue;
                                 }
                             }
                             if (!string.IsNullOrEmpty(sSCV_DEP_P))
                             {
                                 if ((strAux == sSCV_DEP_P) && bExisteSCVEnSBC(strAux, ref listaSrv, ref strMsg))
                                 {
                                     bOk = false;
                                     ErrorSCVExiste(plistaIPCentral[strAux], listaSrv[strAux], ref strMsg);
                                     continue;
                                 }
                                 if (strAux == sSCV_DEP && bExisteSCVEnSCV(strAux, ref listaSrvSBC, ref strMsg))
                                 {
                                     bOk = false;
                                     ErrorSCVExiste(plistaIPCentral[strAux], listaSrvSBC[strAux], ref strMsg);
                                     continue;
                                 }
                             }
                         }
                     }

                    //20200928 #4599
                    if (bSBCEnaire)
                    {
                        if (listaSrvSBC.ContainsKey(strAux)) 
                        {
                            bOk = false;
                            ErrorSCVExiste(plistaIPCentral[strAux], listaSrvSBC[strAux], ref strMsg);
                        }
                    }
                    else if (listaSrv.ContainsKey(strAux))
                    {
                            bOk = false;
                            ErrorSCVExiste(plistaIPCentral[strAux], listaSrv[strAux], ref strMsg);
                    }
                 }
                 //20200928 #4599
                 if (bSBCEnaire && bOk)
                 {
                     if ((!string.IsNullOrEmpty(sSCV_DEP) && (listaSrv.ContainsKey(sSCV_DEP))) || (!string.IsNullOrEmpty(sSCV_DEP_P) && (listaSrv.ContainsKey(sSCV_DEP_P))))
                     {
                         bOk=false;
                         ErrorSCVExiste(plistaIPCentral[strAux], listaSrv[strAux], ref strMsg);
                     }
                 }
                 listaSrv.Clear();
                 listaSrvSBC.Clear();
            }
        }
        return bOk;
    }
    //20200928 #4599
    bool bExisteSCVEnSBC(string Ip_SCV, ref Dictionary<string, string> listaSrv, ref string strMsg)
    {
        bool retorno = false;
        if (listaSrv.ContainsKey(Ip_SCV))
        {
            retorno = true;
        }
        return retorno;
    }

    bool bExisteSCVEnSCV(string Ip_SCV, ref Dictionary<string, string> listaSrvSBC, ref string strMsg)
    {
        bool retorno = false;
        if (listaSrvSBC.ContainsKey(Ip_SCV))
        {
            retorno = true;
        }
        return retorno;
    }
    //20200928 #4599
    void ErrorSCVExiste(string sIp, string sSCV, ref string strMsg)
    {
        if (GetLocalResourceObject("REV_ipSrvExistenteEnOtraCentral") != null)
            strMsg = string.Format(GetLocalResourceObject("REV_ipSrvExistenteEnOtraCentral").ToString(), sIp, sSCV);
        else
            strMsg = string.Format("La dirección IP {0} ya se encuentra configurada en el SCV IP {1}.", sIp, sSCV);
    }

    bool bExisteCentralConMismoNombre(string strIdSistema, string strIdCentral)
    {
        bool bExiste=false;
        ServiciosCD40.Encaminamientos objCentralATS = new ServiciosCD40.Encaminamientos();
        objCentralATS.IdSistema = strIdSistema;
        objCentralATS.Central=strIdCentral;


        //Comprobamos si las direcciones IP están asignadas a otros SCV IP
        //Para ello, se carga en un mapa la lista de direcciones IP configuradas en los SCV IPs del sistema.
        // si la IP no tiene el puerto se le añade el puerto por defecto para poder comparar
        ServiciosCD40.Tablas[] objListaCentrales = ServicioCD40.ListSelectSQL(objCentralATS);
        if (objListaCentrales != null && objListaCentrales.Length > 0)
        {
            bExiste = true;
        }

        return bExiste;
    }

    bool ExisteRango(string strSistema, string strCentral,int iSelectedRangoIndex, ref string strMsg)
    {
        bool bExiste = false;
        string strInicial = string.Empty;
        string strFinal = string.Empty;

        string strRegIni = string.Empty;
        string strRegFin = string.Empty;
        string strTipo = string.Empty;
        int i = 0;

        try
        {
            //Se obtienen los valores del nuevo registro
            strTipo = DListTipoRango.SelectedValue;
            strInicial = TxtBInicial.Text;
            strFinal = TxtBFinal.Text;

            //Se comprueba si el rango está asignado al encaminamiento que se está dando de alta o modificando
            if (DSRangos != null && DSRangos.Tables.Count > 0 && DSRangos.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < DSRangos.Tables[0].Rows.Count && !bExiste; i++)
                {
                    strRegIni = (string)(DSRangos.Tables[0].Rows[i]["Inicial"]);
                    strRegFin = (string)(DSRangos.Tables[0].Rows[i]["Final"]);

                    if (SelectedRangoIndex != -1 && iSelectedRangoIndex == i )
                    {
                        //Si estamos comparando el registro que estamos modificando, pasamos a la siguiente iteración
                        continue;
                    }
                    else
                    {
                        if ((string.Compare(strRegIni, strInicial) <= 0) && (string.Compare(strFinal, strRegFin) <= 0))
                        {
                            bExiste = true;

                            if (GetLocalResourceObject("REV_RangoExistente") != null)
                            {
                                strMsg = string.Format(GetLocalResourceObject("REV_RangoExistente").ToString(), strInicial, strFinal, strRegIni, strRegFin, strCentral);
                            }
                            else
                            {
                                strMsg = string.Format("El rango {0}-{1} está incluido dentro del rango {2}-{3} del SCV {4}", strInicial, strFinal, strRegIni, strRegFin, strCentral);
                            }
                        }
                        else if (((string.Compare(strInicial, strRegIni) <= 0) && (string.Compare(strFinal, strRegIni) >= 0)) ||
                                 ((string.Compare(strInicial, strRegIni) >= 0) && (string.Compare(strInicial, strRegFin) <= 0)))
                        {
                            bExiste = true;

                            if (GetLocalResourceObject("REV_RangoIncluido") != null)
                            {
                                strMsg = string.Format(GetLocalResourceObject("REV_RangoIncluido").ToString(), strInicial, strFinal, strRegIni, strRegFin, strCentral);
                            }
                            else
                                strMsg = string.Format("El rango {0}-{1} tiene números de abonado que están definidos dentro del rango {2}-{3} del SCV {4}.", strInicial, strFinal, strRegIni, strRegFin, strCentral);
                        }
                    }
                }
            }

            if (!bExiste)
            {
                //Se comprueba si el rango está asignado a otro encaminamiento
                DataSet objRangosBD = ServicioCD40.RangosConIdRed(strSistema, null);
                string strRegCentral = string.Empty;

                if (objRangosBD != null && objRangosBD.Tables.Count > 0 && objRangosBD.Tables[0].Rows.Count > 0)
                {

                    for (i = 0; i < objRangosBD.Tables[0].Rows.Count && !bExiste; i++)
                    {
                        strRegCentral = objRangosBD.Tables[0].Rows[i]["Central"].ToString();
                        //Si no es la central que estamos configurando
                        if (string.Compare(strCentral, strRegCentral) != 0)
                        {
                            strRegIni = (string)(objRangosBD.Tables[0].Rows[i]["Inicial"]);
                            strRegFin = (string)(objRangosBD.Tables[0].Rows[i]["Final"]);

                            if ((string.Compare(strRegIni, strInicial) <= 0) && (string.Compare(strFinal, strRegFin) <= 0))
                            {
                                bExiste = true;
                                if (GetLocalResourceObject("REV_RangoExistente") != null)
                                {
                                    strMsg = string.Format(GetLocalResourceObject("REV_RangoExistente").ToString(), strInicial, strFinal, strRegIni, strRegFin, strRegCentral);
                                }
                                else
                                    strMsg = string.Format("El rango {0}-{1} está incluido dentro del rango {2}-{3} configurado en el SCV {4}", strInicial, strFinal, strRegIni, strRegFin, strRegCentral);
                            }
                            else if (((string.Compare(strInicial, strRegIni) <= 0) && (string.Compare(strFinal, strRegIni) >= 0)) ||
                                     ((string.Compare(strInicial, strRegIni) >= 0) && (string.Compare(strInicial, strRegFin) <= 0)))
                            {
                                bExiste = true;
                                if (GetLocalResourceObject("REV_RangoIncluido") != null)
                                {
                                    strMsg = string.Format(GetLocalResourceObject("REV_RangoIncluido").ToString(), strInicial, strFinal, strRegIni, strRegFin, strRegCentral);
                                }
                                else
                                    strMsg = string.Format("El rango {0}-{1} tiene números de abonado que están definidos dentro del rango {2}-{3} del SCV {4}.", strInicial, strFinal, strRegIni, strRegFin, strRegCentral);
                            }
                        }
                    }

                    objRangosBD.Clear();
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Encaminamientos-ExisteRango): Error al comprobar si el nuevo rango a añadir existe. Error:" + ex.Message.ToString());
        }

        return bExiste;
    }
}
