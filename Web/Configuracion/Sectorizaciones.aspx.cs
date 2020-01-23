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
using ServiciosCD40;
using SincronizaCD30;
using log4net;
using log4net.Config;
using System.Text;


//public partial class Sectorizaciones : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
public partial class Sectorizaciones : PageBaseCD40.PageCD40//, System.Web.UI.ICallbackEventHandler
{
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
    private static int NumUCSs = 42;
    private static DataSet datos;
	private static int NumTopSeleccionada;
	private static int TopIntercambio;
	private static List<string> UsuarioSeleccionado=new List<string>();
	private static DataSet DSNumerosAbonados;
	private static DataSet DataSetSectoresLibres;

	//private const int NumPaginas = 3;
	//private const int NumPosicionesPag = 16;
	private static int NumPagActual = 1;
	private static ServiciosCD40.Tablas[] DatosInternos;
	private static ServiciosCD40.Tablas[] DatosExternos;
	private static ServiciosCD40.Tablas[] DatosRadio;
	private static ServiciosCD40.ParametrosSector ParametrosSector = new ServiciosCD40.ParametrosSector();
//	private static ServiciosCD40.Sistema ParametrosSistema = new ServiciosCD40.Sistema();
	private static List<string>[] ListaUsuariosEnTop = new List<string>[49];
	private static bool VisualizandoActiva = false;
	static bool PermisoSegunPerfil;
	static string IdSectorizacionActiva;
    static bool bServicioSactaAct; //Indica  si el servicio SACTA está activo

    private static ServiciosCD40.ServiciosCD40 serviceServiciosCD40;
//	private static ServiciosMantenimiento.GestorMantenimiento ServicioMantenimiento;

    private static AsyncCallback CallbackCompletado;//, CambioSectorizacionCallbackCompletado;

    const string ID_SECTORIZACION_SACTA="SACTA"; //Identificador de la Sectorización recibida del Sistema SACTA
    const string ID_SECTORIZACION_SCV = "SCV";   //Identificador interno de la Sectorización activa
    private static System.Collections.Hashtable objListaTipoSector = new System.Collections.Hashtable(); //Almacena el tipo de cada sector de la sectorización: R (Real),V (Virtual) o M (Mantenimiento)
    private const string ID_SECTOR_VIRTUAL = "V";
    private const string ID_SECTOR_REAL = "R";
    private const string ID_SECTOR_MANTENIMIENTO = "M";


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

            // Los usuarios con perfil técnico 1, técnico 2 o técnico 3 pueden crear/modificar y cargar sectorizaciones
            PermisoSegunPerfil = ((perfil == "3") || (perfil == "2") || (string.Compare(perfil,"1")==0));

		}

        SetFocus(ListBox1);

		if (CallbackCompletado == null)
			CallbackCompletado = new AsyncCallback(OnCallBackCompleted);

		if (serviceServiciosCD40 == null)
			serviceServiciosCD40 = new ServiciosCD40.ServiciosCD40();
        //if (ServicioMantenimiento == null)
        //    ServicioMantenimiento = new ServiciosMantenimiento.GestorMantenimiento();
        
		string cbReference = ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context", "ClientCallbackError", true);
		string callbackScript = "function CallServer(arg, context) {" + cbReference + "; }";

        ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

        if (!IsPostBack)
        {
            //Si es la primera vez que se accede a la página, se verifica si la aplicacion está configurada con la funcionalidad Sacta
            bSactaActivoEnCnf = bSistemaConSactaHabilitadoPorCnf();
            bServicioSactaAct = false;
        }

        
        //Si por configuración, está habilitada la funcionalidad Sacta, se obtiene el estado del servicio Sacta
        //En caso contrario, no se invoca al Web Service para no sobrecargar la página
        if (bSactaActivoEnCnf)
        {
            if (EstaServicioSactaActivo())
            {
                bServicioSactaAct = true;
            }
            else
            {
                //Si se ha desactivado
                bServicioSactaAct = false;
            }
        }

        if (PermisoSegunPerfil)
        {
            if (bServicioSactaAct == false)
            {
                //BtActivar.Enabled = (Application["Sectorizando"] != null ? !(bool)Application["Sectorizando"] : true);

                //Si el proceso de sectorización está activo, se deshabilitan el botón de Activar sectorización
                if ((Application["Sectorizando"] != null && (bool)Application["Sectorizando"] == true) || (Session["Sectorizando"] != null && (bool)Session["Sectorizando"] == true))
                {
                    BtActivar.Enabled = false;
                }
                else
                {
                    BtActivar.Enabled = true;
                }
            }
            else
            {
                //Si está activo SACTA, se está visualizando la sectorización activa y es la sectorización sacta, se habilita el botón btActivar
                if (!VisualizandoActiva && string.Compare(LIdSectorizacion.Text, ID_SECTORIZACION_SACTA) == 0 && string.Compare(ListBox1.SelectedValue, ID_SECTORIZACION_SACTA) == 0)
                    BtActivar.Enabled = true;
                else
                    BtActivar.Enabled = false;
            }
        }
        else
            BtActivar.Enabled = false;

        LblEnlaceSacta.ForeColor = (bServicioSactaAct == false) ? System.Drawing.Color.Red : System.Drawing.Color.Green;

        if (!IsPostBack)
        {
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            BtActivar.Visible = BtNuevo.Visible = PermisoSegunPerfil;
            BtLiberar.Enabled = BtAsignar.Enabled = BtCambiar.Enabled = PermisoSegunPerfil;
            CreaListaUsuariosEnTop();

            //Al acceder a la página por primera vez se visualiza la sectorización activa
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            Session["idsistema"] = s.Value;

            ActualizaActiva();
            NewItem = LIdSectorizacion.Text;
            MuestraDatos(DameDatos());
        }
        else
        {
            //Si se ha recargado la página, las variables datos y la variable de session tienen valor nulo es porque 
            // si ha cambiado la sesión del servidor, bien por conmutación o reinicio
            //por lo que se va a la página de login
            if (Session["idsistema"] == null)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);
            }

            bVisualizandoActiva = VisualizandoActiva;

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
			if (Request.Form["SectoresManttoSolos"] == "1")     //Error al generar una sectorizacion con sectores de mantto.
			{
				Request.Form["SectoresManttoSolos"].Replace("1", "0");
			}

			
			//if (Request.Params["__EVENTTARGET"] == "Actualiza")
			{
				ActualizaActiva();
			}

			//if (PnlParametrosSector.Visible)
			//    ModalPopupExtender1.Show();
		}
        
    }

    /// <summary>
    /// 
    /// </summary>
	private void CreaListaUsuariosEnTop()
	{
		UsuarioSeleccionado.Clear();
		TopIntercambio = 0;

		for (int i = 0; i < NumUCSs; i++)
			ListaUsuariosEnTop[i] = new List<string>();
	}

    /// <summary>
    /// 
    /// </summary>
	private void ResetListaUsuariosEnTops()
	{
		//int numRow = 1;
		//foreach (TableRow tr in TUCS.Rows)
		//{
		//    if (tr.ID == "TableRow" + numRow)
		//    {
		//        foreach (TableCell tc in tr.Cells)
		//        {
		//            tc.Visible = true;
		//        }
		//    }
		//}

		for (int i = 0; i < NumUCSs; i++)
		{
			Button tbox = (Button)TUCS.FindControl("TextBox" +(i + 1));
			tbox.Text = string.Empty;
	
            if (null!=ListaUsuariosEnTop[i])
			    ListaUsuariosEnTop[i].Clear();

			ListBox lBoxUsuarios = (ListBox)TUCS.FindControl("BUsuarios" + (i + 1));
			if (lBoxUsuarios != null)
				lBoxUsuarios.Items.Clear();
		}

        if (objListaTipoSector.Count> 0)
            objListaTipoSector.Clear(); 
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.Sectorizaciones t = new ServiciosCD40.Sectorizaciones();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;
			//ParametrosSistema.IdSistema = s.Value;
			//ServiciosCD40.Tablas[] sistema = g.ListSelectSQL(ParametrosSistema);
			//if (sistema.Length > 0)
			//    ParametrosSistema = (ServiciosCD40.Sistema)sistema[0];

            ServiciosCD40.Tablas[] d = g.ListSelectSQL(t);
            return d;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-DameDatos): ",ex);
        }
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nu"></param>
    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
        string strIdSectorizacion = string.Empty;
        string strFechaActivacion = string.Empty;

        if (nu != null)
        {
            for (int i = 0; i < nu.Length; i++)
            {
                strIdSectorizacion = ((ServiciosCD40.Sectorizaciones)nu[i]).IdSectorizacion;
                strFechaActivacion = ((ServiciosCD40.Sectorizaciones)nu[i]).FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss");

                // Evitar que aparezca la sectorización activa ID_SECTORIZACION_SCV
                //La sectorización SACTA sólo podrá aparecer si el servicio SACTA está activo
                if (string.Compare(strIdSectorizacion, strFechaActivacion) != 0 &&
                    string.Compare(strIdSectorizacion, ID_SECTORIZACION_SCV) != 0 &&
                    string.Compare(strIdSectorizacion, ID_SECTORIZACION_SACTA) != 0)
                {
                    ListBox1.Items.Add(((ServiciosCD40.Sectorizaciones)nu[i]).IdSectorizacion);
                }

                if (bServicioSactaAct==true && string.Compare(strIdSectorizacion, ID_SECTORIZACION_SACTA) == 0 && ListBox1.Items.FindByText(strIdSectorizacion) == null)
                {
                    //Si el servicio SACTA está activo y la sectorización SACTA no está en la lista, se inserta al principio de la lista
                    ListBox1.Items.Insert(0,((ServiciosCD40.Sectorizaciones)nu[i]).IdSectorizacion);
                }

                if (((ServiciosCD40.Sectorizaciones)nu[i]).Activa)
                {
                    BVerSectorizacionActiva.Enabled = true;
                    LFechaSectorizacion.Text = ((ServiciosCD40.Sectorizaciones)nu[i]).FechaActivacion.ToString("dd/MM/yyyy");
                    LHoraSectorizacion.Text = ((ServiciosCD40.Sectorizaciones)nu[i]).FechaActivacion.ToString("HH:mm:ss");
                    LIdSectorizacion.Text = strIdSectorizacion;
                }
            }
        }

        if (ListBox1.Items.Count > 0)
        {
            BtActivar.Visible = PermisoSegunPerfil;

            if (ListBox1.Items.FindByText(NewItem) != null)
            {
                ListBox1.Items.FindByText(NewItem).Selected = true;
                IndexListBox1 = ListBox1.SelectedIndex;
                NewItem = string.Empty;
            }
            else
            {
                ListBox1.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < ListBox1.Items.Count ? IndexListBox1 : 0;
            }

            Session["elemento"] = ListBox1.SelectedValue;
            CargarSectorizacion((string)Session["idsistema"],ListBox1.SelectedValue, false);

            //MVO: el botón será visible si la sectorización seleccionada no es la activa
            BtEliminar.Visible = PermisoSegunPerfil && (ListBox1.SelectedValue != LIdSectorizacion.Text);
        }
        else
        {
            BtEliminar.Visible = BtActivar.Visible = false;
        }
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
            //BtEliminar.Enabled = PermisoSegunPerfil;

            BtActivar.Visible = PermisoSegunPerfil && (Application["Sectorizando"] != null ? !(bool)Application["Sectorizando"] : true);

            //Si el servicio SACTA está activo y se selecciona un elemento, se deshabilita el botón Activar
            if (BtActivar.Visible && bServicioSactaAct && string.Compare(ListBox1.SelectedValue, ID_SECTORIZACION_SACTA) != 0)
            {
                BtActivar.Enabled = false;
            }
            else
                BtActivar.Enabled = BtActivar.Visible;

            Session["elemento"] = ListBox1.SelectedValue;
            CargarSectorizacion((string)Session["idsistema"], ListBox1.SelectedValue, false);

            BtEliminar.Visible = PermisoSegunPerfil && ListBox1.SelectedValue != LIdSectorizacion.Text;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idSectorizacion"></param>
    /// <param name="activa"></param>
    private void CargarSectorizacion(string strIdSistema,string idSectorizacion, bool activa)
    {
		CambiaAspectoTablaUcs(false);
        string strNombreSector = string.Empty;
        string strTipoSector = string.Empty;


        //Se comprueba que se ha informado el identificador de la sectorización, para evitar que se muestren los tops de todas las sectorizaciones configuradas
        // en la base de datos cuando se supone que se debe visualizar sólo la sectorización que se ha seleccionado
        if (!string.IsNullOrEmpty(idSectorizacion) && !string.IsNullOrEmpty(strIdSistema))
        {

            try
            {
                TUCS.Visible = true;
                ResetListaUsuariosEnTops();

                BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarSectorizacion"), ListBox1.SelectedValue);

                //ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
                DataSet d = serviceServiciosCD40.AsignacionDeUsuariosATops(strIdSistema, idSectorizacion);
                datos = d;

                if (null != d && d.Tables.Count > 0)
                {
                    int index = 1;
                    foreach (System.Data.DataRow ds in d.Tables[0].Rows)
                    {
                        if (index <= NumUCSs)
                        {
                            if (ds["IdNucleo"] != DBNull.Value)
                                ViewState["idnucleo"] = (string)ds["IdNucleo"];
                            if (ds["IdSistema"] != DBNull.Value)
                                ViewState["IdSistema"] = (string)ds["IdSistema"];

                            if (ds["IdTop"] != DBNull.Value)
                            {
                                Button tboxTop = (Button)TUCS.FindControl("TBox" + index);
                                tboxTop.Text = (string)ds["IdTop"];
                                ImageButton ibut = (ImageButton)TUCS.FindControl("IButton" + index);
                                ibut.ImageUrl = "~/Configuracion/Images/TextoUCS.jpg";
                            }

                            if (ds["IdSector"] != DBNull.Value && (string)ds["IdSector"] != "**FS**")
                            {
                                ImageButton ibut = (ImageButton)TUCS.FindControl("ImageButton" + index.ToString());
                                Button tbox = (Button)TUCS.FindControl("TextBox" + index.ToString());

                                strNombreSector = ds["IdSector"].ToString();
                                strTipoSector= string.Empty;

                                tbox.ToolTip = tbox.Text = (string)ds["IdSector"];
                                ibut.ImageUrl = "~/Configuracion/Images/UCSAs.jpg";
                                TableCell celda = (TableCell)TUCS.FindControl("TableCell" + index.ToString());
                                celda.ToolTip = ListaUsuarios((string)ds["IdNucleo"], (string)ds["IdSector"], index - 1);
                                ListBox lBoxUsuarios = (ListBox)TUCS.FindControl("BUsuarios" + index);
                                TableCell tc = (TableCell)TUCS.FindControl("CellUsuarios" + index);
                                if (tc != null)
                                {
                                    tc.Visible = true;
                                    //tc.Text = celda.ToolTip;
                                }
                                if (lBoxUsuarios != null)
                                {
                                    lBoxUsuarios.Visible = true;
                                    for (int i = 0; i < ListaUsuariosEnTop[index - 1].Count; i++)
                                        lBoxUsuarios.Items.Add(ListaUsuariosEnTop[index - 1][i]);
                                }

                                //Obtenemos el tipo del sector
                                if (ds.Table.Columns.Contains("Tipo") && ds["Tipo"] != DBNull.Value)
                                {
                                    strTipoSector=ds["Tipo"].ToString();

                                    if (strTipoSector.Length>0 && !objListaTipoSector.ContainsKey(strNombreSector))
                                        objListaTipoSector.Add(strNombreSector, strTipoSector);
                                }
                            }
                            else
                            {
                                ImageButton ibut = (ImageButton)TUCS.FindControl("ImageButton" + index);
                                Button tbox = (Button)TUCS.FindControl("TextBox" + index);
                                tbox.ToolTip = tbox.Text = "";
                                TableCell celda = (TableCell)TUCS.FindControl("TableCell" + index.ToString());
                                celda.ToolTip = "";
                                ListBox lBoxUsuarios = (ListBox)TUCS.FindControl("BUsuarios" + index);
                                ibut.ImageUrl = "~/Configuracion/Images/UCS.jpg";
                                TableCell tc = (TableCell)TUCS.FindControl("CellUsuarios" + index);
                                if (tc != null)
                                    tc.Visible = true;
                                if (lBoxUsuarios != null)
                                    lBoxUsuarios.Items.Clear();
                            }
                            TableCell cellidtop = (TableCell)TUCS.FindControl("Cell" + index.ToString());
                            cellidtop.Visible = true;
                            TableCell cellucs = (TableCell)TUCS.FindControl("Tablecell" + index.ToString());
                            cellucs.Visible = true;
                            index++;
                        }
                    }

                    for (int j = index; j <= NumUCSs; j++)
                    {
                        TableCell cellidtop = (TableCell)TUCS.FindControl("Cell" + j.ToString());
                        cellidtop.Visible = false;
                        TableCell cellucs = (TableCell)TUCS.FindControl("Tablecell" + j.ToString());
                        cellucs.Visible = false;
                    }

                }

            }
            catch (Exception ex)
            {
                logDebugView.Error("(Sectorizaciones-CargarSectorizacion): ", ex);
            }
        }
        else
            logDebugView.Debug("(Sectorizaciones-CargarSectorizacion): Se ha invodado a la función con la sectorización o el sistema sin informar");      
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="nucleo"></param>
    /// <param name="sector"></param>
    /// <param name="indiceTop"></param>
    /// <returns></returns>
	private string ListaUsuarios(string nucleo, string sector,int indiceTop)
	{
        try
        {
            System.Text.StringBuilder strSectores = new System.Text.StringBuilder();
            //ServiciosCD40.ServiciosCD40 serviceServiciosCD40 = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.SectoresSector ss = new ServiciosCD40.SectoresSector();
            ss.IdSistema = (string)Session["idsistema"];
            ss.IdNucleo = nucleo;
            ss.IdSector = sector;

			ServiciosCD40.Tablas[] listaSectores = serviceServiciosCD40.ListSelectSQL(ss);

            if (listaSectores != null)
            {
                for (int i = 0; i < listaSectores.Length; i++)
                {
                    ListaUsuariosEnTop[indiceTop].Add(((ServiciosCD40.SectoresSector)listaSectores[i]).IdSectorOriginal);
                    //                strSectores.Append(((ServiciosCD40.SectoresSector)listaSectores[i]).IdSectorOriginal);
                    //                strSectores.Append("\r\n");
                }
            }

            OrdenaListaUsuariosPorIdSacta(indiceTop, null);

            for (int i = 0; i < ListaUsuariosEnTop[indiceTop].Count; i++)
            {
                strSectores.Append(ListaUsuariosEnTop[indiceTop][i]);
                strSectores.Append("\r\n");
            }

			return strSectores.ToString();
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-ListaUsuarios): ", ex);
        }
        return "";
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        if (ListBox1.SelectedValue != "")
        {
            IndexListBox1 = ListBox1.SelectedIndex;
            Session["elemento"] = ListBox1.SelectedValue;
            EliminarElemento();
			//cMsg.confirm(string.Format((string)GetGlobalResourceObject("Espaniol", "EliminarSectorizacion"),ListBox1.SelectedValue), "eliminaelemento");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void EliminarElemento()
    {
        try
        {
            ServiciosCD40.Sectorizaciones n = new ServiciosCD40.Sectorizaciones();
            n.IdSistema = (string)Session["idsistema"];
            n.IdSectorizacion = (string)Session["elemento"];

            //Si se informa el sistema y la sectorización se realiza el borrado
            if (!string.IsNullOrEmpty(n.IdSistema) && !string.IsNullOrEmpty(n.IdSectorizacion))
            {
                ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();

                if (g.DeleteSQL(n) < 0)
                {
                    logDebugView.Warn("(Sectorizaciones-EliminarElemento): No se ha podido eliminar la sectorizacion.");
                    cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorEliminarSectorizacion"), n.IdSectorizacion));
                }
                else
                {
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                    KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                    if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                    {
                        SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                        switch (sincro.BajaSectorizacion(n.IdSectorizacion))
                        {
                            case 106:
                                string s = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado") + "\\n\\n"
                                    + String.Format((string)GetGlobalResourceObject("Espaniol", "Cod106"), n.IdSectorizacion);
                                cMsg.alert(s);
                                break;
                            case 107:
                                string s1 = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado") + "\\n\\n"
                                    + String.Format((string)GetGlobalResourceObject("Espaniol", "Cod107"), n.IdSectorizacion);
                                cMsg.alert(s1);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                        cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-EliminarElemento): ",ex);
        }
        ListBox1.Items.Clear();
        MuestraDatos(DameDatos());

        if (ListBox1.Items.Count == 0)
            TUCS.Visible = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        //Se comprueba que la sectorización no tenga las palabras reservadas SACTA Y SCV
        if (string.Compare(TBNuevo.Text, ID_SECTORIZACION_SACTA) != 0 && string.Compare(TBNuevo.Text, ID_SECTORIZACION_SCV) != 0)
        {
            GuardarCambios();
        }
        else
        {
            //Identificador no permitido. El identificador no puede tener el valor SACTA o SCV
            cMsg.alert((string)GetLocalResourceObject("MsgIdentificadorNoValido"));
        }
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
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");
    }

    /// <summary>
    /// 
    /// </summary>
    private void GuardarCambios()
    {
        ServiciosCD40.Sectorizaciones n = new ServiciosCD40.Sectorizaciones();
        n.IdSistema = (string)Session["idsistema"];
        n.IdSectorizacion = TBNuevo.Text;

        //La fecha de activación se inicializa a 01/01/1970
        n.FechaActivacion = new DateTime(1970, 1, 1, 0, 0, 0);

        NewItem = TBNuevo.Text;

        ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
        if (g.InsertSQL(n) < 0)
        {
            logDebugView.Warn("(Sectorizaciones-GuardarCambios): No se ha podido guardar la sectorizacion.");
            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorGuardarSectorizacion"), n.IdSectorizacion));
		}
        else
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
            if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
            {
                SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                switch (sincro.AltaSectorizacion(n.IdSectorizacion))
                {
                    case 104:
                        cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod104"), n.IdSectorizacion));
                        break;
                    case 105:
                        cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod105"), n.IdSectorizacion));
                        break;
                    default:
                        break;
                }
            }
        }
		// g.Commit();		// Esta página web no activa TimeoutTransaction, con lo cual es necesario hacer un commit al Aceptar.

        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
		LblIdSectorizacion.Visible = false;
		TBNuevo.Visible = false;
        RequiredFieldValidator1.Visible = false;
        ListBox1.Enabled = true;
        BtNuevo.Visible = PermisoSegunPerfil;
        //MVO: el botón eliminar será visible si la sectorización no está activa
        BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0 && ListBox1.SelectedValue != LIdSectorizacion.Text;
        BtActivar.Visible = PermisoSegunPerfil && ListBox1.SelectedIndex >= 0;

        DvMarcoTops.Visible = true;
		PanelActiva.Visible = true;
		ListBox1.Items.Clear();
        MuestraDatos(DameDatos());
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
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
		LblIdSectorizacion.Visible = false;
		TBNuevo.Visible = false;
        RequiredFieldValidator1.Visible = false;
        ListBox1.Enabled = true;
        BtNuevo.Visible = PermisoSegunPerfil;
        BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0 && ListBox1.SelectedValue != LIdSectorizacion.Text;
        BtActivar.Visible = PermisoSegunPerfil && ListBox1.SelectedIndex >= 0;
        DvMarcoTops.Visible = true;
		PanelActiva.Visible = true;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtNuevo_Click(object sender, EventArgs e)
    {
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
		LblIdSectorizacion.Visible = true;
        TBNuevo.Text = "";
        RequiredFieldValidator1.Visible = true;
        TBNuevo.Visible = true;
        ListBox1.Enabled = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        BtActivar.Visible = false;
		DvMarcoTops.Visible = false;
		PanelActiva.Visible = false;
        IndexListBox1 = ListBox1.SelectedIndex;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtActivar_Click(object sender, EventArgs e)
    {
        string listenIp = string.Empty;
        string strIdSectorizacion = string.Empty;

        try
        {
            //Si no se ha seleccionado ninguna sectorización, no hacemos nada 

            strIdSectorizacion = (string)Session["elemento"];

            if (string.IsNullOrEmpty(strIdSectorizacion))
                return;

            DateTime fechaActiva = new DateTime();

            // Comunicar al servicio de mantenimiento del cambio de sectorización
            //bool comunicaAMantenimiento = serviceServiciosCD40.FindTableInTablasModificadas("TIFX") ||
            //    serviceServiciosCD40.FindTableInTablasModificadas("TOP") ||
            //    serviceServiciosCD40.FindTableInTablasModificadas("RECURSOS");

            //Si algún panel del top está visible, se oculta
            if (Panel1.Visible)
                Panel1.Visible = false;

            if (PanelTop.Visible)
                PanelTop.Visible = false;

            if (PanelSectores.Visible)
                PanelSectores.Visible = false;

            System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            listenIp = webConfiguracion.AppSettings.Settings["OrigenMulticastIp"].Value;

            logDebugView.Debug(string.Format("Sectorizaciones-BActivar_Click: Se invoca al servicio serviceServiciosCD40.ComunicaSectorizacionActiva({0},{1},{2},{3})", listenIp, (string)Session["idsistema"], (string)Session["elemento"], fechaActiva.ToString("dd/MM/yyyy HH:mm:ss")));

			if (serviceServiciosCD40.ComunicaSectorizacionActiva(listenIp, (string)Session["idsistema"], (string)Session["elemento"], ref fechaActiva))
			{
				LIdSectorizacion.Text = (string)Session["elemento"];
				LFechaSectorizacion.Text = fechaActiva.ToString("dd/MM/yyyy");
				LHoraSectorizacion.Text = fechaActiva.ToString("HH:mm:ss");
				BVerSectorizacionActiva.Enabled = true;

                //if (comunicaAMantenimiento)
                //    ServicioMantenimiento.CambioDeSectorizacion();

                logDebugView.Debug("Sectorizaciones-BActivar_Click: serviceServiciosCD40.ComunicaSectorizacionActiva implantada. Fecha activacion=" + fechaActiva.ToString("dd/MM/yyyy HH:mm:ss"));
                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "SectorizacionImplantada"));

                //MVO: Si la sectorización se ha habilitado correctamente, no se permite eliminar la sectorización
                if (ListBox1.SelectedIndex >= 0)
                {
                    bool bEliminarVisible = true;
                    bEliminarVisible = PermisoSegunPerfil && ListBox1.SelectedValue != LIdSectorizacion.Text;

                    if (BtEliminar.Visible != bEliminarVisible)
                        BtEliminar.Visible = bEliminarVisible;
                }
			}
			else
			{
                logDebugView.Debug("Sectorizaciones-BActivar_Click: serviceServiciosCD40.ComunicaSectorizacionActiva devuelve false");
				cMsg.alert((string)GetGlobalResourceObject("Espaniol", "NoTodosSectoresReales"));
			}


			//ResetTransaction();
        }
        catch (Exception ex)
        {
            logDebugView.Error(string.Format("(Sectorizaciones-BActivar_Click): Error al activar la sectorizacion listenIp={0} sistema={1} sectorizacion={2} . Error: ",listenIp,(string)Session["idsistema"], (string)Session["elemento"]), ex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnIntercambiarTop_Click(object sender, EventArgs e)
	{
		PanelTop.Visible = false;

		ListBox lBoxUsuarios = (ListBox)TUCS.FindControl("BUsuarios" + NumTopSeleccionada);
		if (lBoxUsuarios != null)	// Se cogen todos los usuarios de la top
		{
			int index = 1;

			Panel1.Visible = false;

			// Guardar usuarios y top.
			TopIntercambio = NumTopSeleccionada;
			foreach (ListItem item in lBoxUsuarios.Items)
			{
				UsuarioSeleccionado.Add(item.Text);
			}

			int numRow = 1;
			foreach (TableRow tr in TUCS.Rows)
			{
				if (tr.ID == "Row" + numRow)
				{
					foreach (TableCell tc in tr.Cells)
					{
						if (tc.Visible)
						{
							ImageButton ibut = (ImageButton)TUCS.FindControl("IButton" + index);
							ibut.ImageUrl = "~/Configuracion/Images/TOP.gif";
						}
						index++;
					}
					numRow++;
				}
			}
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnLiberarTop_Click(object sender, EventArgs e)
	{
        try
        {
            string strIdSistema = (string)Session["idsistema"];

            PanelTop.Visible = false;
			Button itop = (Button)TUCS.FindControl("TBox" + NumTopSeleccionada);

			// Generar evento en históricos
			foreach (string s in ListaUsuariosEnTop[NumTopSeleccionada - 1])
			{
				serviceServiciosCD40.GeneraHistoricoSectorAsignado((string)Session["IdSistema"], false, s, itop != null ? itop.Text : "");
			}


            // Liberar todos los sectores de la top
            LiberarTop(itop.Text);

			Top t = new Top();
			t.IdSistema = (string)Session["IdSistema"];
			t.IdTop = itop.Text;
			ServiciosCD40.Tablas[] tops = serviceServiciosCD40.ListSelectSQL(t);
			if (tops.Length == 0)
				return;
			uint numTopSeleccionada = ((Top)tops[0]).PosicionSacta;

            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
            if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
            {
                SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
				switch (sincro.LiberarUCS(ListBox1.SelectedValue, (int)numTopSeleccionada))
                {
                    case 119:
                        cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod119"));                        
                        break;
                    case 120:
                        cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod120"));                        
                        break;
                    default:
                        break;
                }
            }

			CargarSectorizacion(strIdSistema,(string)Session["elemento"], false);
			
			// Generar la sectorización en el servidor
			Application["Sectorizando"] = true;

			//Session.Add("Sectorizando", true);
			BtActivar.Enabled = false;
			serviceServiciosCD40.BeginGenerarSectorizacion((string)Session["IdSistema"], (string)Session["elemento"], false, CallbackCompletado, null);
            //VMG 04/03/2019
            Page.ClientScript.RegisterStartupScript(this.GetType(), "displayNloader", "displayNloader();", true);
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-BtnLiberarTop_Click): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnCancelarTop_Click(object sender, EventArgs e)
	{
		TopIntercambio = 0;
		PanelTop.Visible = false;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void Top_OnClick(object sender, EventArgs e)
	{

        if (VisualizandoActiva || !PermisoSegunPerfil)	// Visualizando sectorización activa no se permiten cambios en las Tops.
        {
            PanelNoPermissions.Visible = !PermisoSegunPerfil;
            DvMarcoTops.Visible = PermisoSegunPerfil;
            return;
        }
        else if (bServicioSactaAct && string.Compare(ListBox1.SelectedValue, ID_SECTORIZACION_SACTA) == 0)
        {
            //Si el enlace SACTA está activo y se está visualizando la sectorización SACTA sólo se permite liberar el TOP si el sector es de mantenimiento
            NumTopSeleccionada = Convert.ToInt32(((TableCell)((Control)sender).Parent).ID.Replace("Cell", ""));

            if (NumTopSeleccionada > 0)
            {
                ListBox lBoxUsuariosAux = (ListBox)TUCS.FindControl("BUsuarios" + NumTopSeleccionada);
                Button tboxTop = (Button)TUCS.FindControl("TextBox" + NumTopSeleccionada);
                string strNombreTop = ((Button)sender).Text;
                string strNombreSector = string.Empty;

                if (tboxTop!=null)
                    strNombreSector=tboxTop.Text;

                if (lBoxUsuariosAux != null && lBoxUsuariosAux.Items.Count == 1 && !string.IsNullOrEmpty(strNombreSector))
                {
                    //Si el sector es de mantenimiento, se permite liberar el top
                    if (!string.IsNullOrEmpty(strNombreSector) && objListaTipoSector.Contains(strNombreSector) &&
                        (string)(objListaTipoSector[strNombreSector]) == ID_SECTOR_MANTENIMIENTO)
                    {
                        VisualizarPanelTop(true, strNombreTop);
                    }
                    else
                        PanelTop.Visible = false;
                }
                else
                {
                    PanelTop.Visible = false;
                }
            }
        }
        else
        {
            try
            {
                NumTopSeleccionada = Convert.ToInt32(((TableCell)((Control)sender).Parent).ID.Replace("Cell", ""));

                if (TopIntercambio == 0)	// No se ha pulsado para intercambiar un usuario o sector
                {
                    // Liberar todos los sectores de la top
                    ViewState["IdSector"] = ((Button)TUCS.FindControl("TextBox" + NumTopSeleccionada)).Text;
                    //PanelTop.Visible = true;

                    string strNombreTop = ((Button)sender).Text;
                    VisualizarPanelTop(false, strNombreTop);
                }
                else
                {
                    if (TopIntercambio != NumTopSeleccionada)
                    {
                        Button itop = (Button)TUCS.FindControl("TBox" + NumTopSeleccionada);
                        Button itopIntercambio = (Button)TUCS.FindControl("TBox" + TopIntercambio);

                        //if (serviceServiciosCD40.SectoresManttoEnTop((string)Session["IdSistema"], (string)Session["elemento"], itop.Text))
                        //{
                        //    TopIntercambio = 0;
                        //    UsuarioSeleccionado.Clear();

                        //    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "SectoresManttoSolos"), "SectoresManttoSolos");
                        //    return;
                        //}


                        Top t = new Top();
                        t.IdSistema = (string)Session["IdSistema"];
                        t.IdTop = itop.Text;
                        ServiciosCD40.Tablas[] tops = serviceServiciosCD40.ListSelectSQL(t);
                        if (tops.Length == 0)
                            return;
                        uint numTopSeleccionada = ((Top)tops[0]).PosicionSacta;
                        t.IdTop = itopIntercambio.Text;
                        tops = serviceServiciosCD40.ListSelectSQL(t);
                        if (tops.Length == 0)
                            return;
                        uint numTopIntercambio = ((Top)tops[0]).PosicionSacta;

                        List<string> listaAux = new List<string>();

                        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                        {
                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                            switch (sincro.IntercambiarUCS(ListBox1.SelectedValue, ListaUsuariosEnTop[TopIntercambio - 1].ToArray(), (int)numTopIntercambio,
                                    ListaUsuariosEnTop[NumTopSeleccionada - 1].ToArray(), (int)numTopSeleccionada))
                            {
                                case 121:
                                    cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod121"));
                                    break;
                                case 122:
                                    cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod122"));
                                    break;
                                default:
                                    break;
                            }
                        }

                        listaAux = ListaUsuariosEnTop[NumTopSeleccionada - 1];
                        ListaUsuariosEnTop[NumTopSeleccionada - 1] = ListaUsuariosEnTop[TopIntercambio - 1];
                        ListaUsuariosEnTop[TopIntercambio - 1] = listaAux;

                        EliminaSectorizacionEnTop(NumTopSeleccionada);
                        EliminaSectorizacionEnTop(TopIntercambio);

                        GenerarTop(NumTopSeleccionada, false, false);
                        GenerarTop(TopIntercambio, true, false);

                        // Generar la sectorización en el servidor
                        //ServiciosCD40.ServiciosCD40 serviceServiciosCD40 = new ServiciosCD40.ServiciosCD40();
                        Application["Sectorizando"] = true;
                        //Session.Add("Sectorizando", true);
                        BtActivar.Enabled = false;
                        serviceServiciosCD40.BeginGenerarSectorizacion((string)Session["IdSistema"], (string)Session["elemento"], false, CallbackCompletado, null);
                        //VMG 04/03/2019
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "displayNloader", "displayNloader();", true);
                    }
                    TopIntercambio = 0;
                    UsuarioSeleccionado.Clear();
                }
            }
            catch (Exception ex)
            {
                logDebugView.Error("(Sectorizaciones-Top_OnClick): ", ex);
            }
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numTop"></param>
	private void EliminaSectorizacionEnTop(int numTop)
	{
        try
        {
            ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.SectoresSectorizacion ss = new ServiciosCD40.SectoresSectorizacion();

            Button itop = (Button)TUCS.FindControl("TBox" + numTop);

            ss.IdSistema = (string)Session["IdSistema"];
            ss.IdSectorizacion = ListBox1.SelectedValue;
            ss.IdTop = itop.Text;

            if (!string.IsNullOrEmpty(ss.IdSistema) && !string.IsNullOrEmpty(ss.IdSectorizacion) && !string.IsNullOrEmpty(ss.IdTop))
            {
                // Eliminar los sectores asignados a esta top en esta sectorización sólo si se informan el idSistema, la sectorización y el idTop
                // para evitar que se puedan eliminar todas las sectorizaciones si estos valores no se informan antes de llamar a la función
                if (g.DeleteSQL(ss) < 0) logDebugView.Error("(Sectorizaciones-EliminaSectorizacionEnTop): no se ha podido eliminar SectoresSectorizacion");
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-EliminaSectorizacionEnTop): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UCS_OnClick(object sender,  EventArgs e)
    {
        try
        {
            if (!PermisoSegunPerfil)
            {
                BtLiberar.Enabled = BtAsignar.Enabled = BtCambiar.Enabled = false;
                BtExplorar.Enabled = true;
            }
            
            string strNombreSector = string.Empty;
            string strSectorizacion = string.Empty;
            bool bNointercambiarTop = true;

            NumTopSeleccionada = Convert.ToInt32(((TableCell)((Control)sender).Parent).ID.Replace("TableCell", ""));

            strNombreSector = ((Button)TUCS.FindControl("TextBox" + NumTopSeleccionada)).Text;
            ListBox lBoxUsuariosAux = (ListBox)TUCS.FindControl("BUsuarios" + NumTopSeleccionada);
            ImageButton ibut = (ImageButton)TUCS.FindControl("ImageButton" + NumTopSeleccionada);
            Button itop = (Button)TUCS.FindControl("TBox" + NumTopSeleccionada);
            strSectorizacion = ListBox1.SelectedValue;

            if (VisualizandoActiva) //Si se está visualizando la sectorización activa
            {
                ViewState["IdSector"] = strNombreSector;
                ViewState["IdTop"] = itop.Text;

                //El botón ver panel se activa, si el puesto tiene sectores asignados
                if (lBoxUsuariosAux != null && lBoxUsuariosAux.Items.Count > 0)
                {
                    BtExplorar.Enabled = true;
                }
                else
                {
                    BtExplorar.Enabled = false;
                }
                BtLiberar.Enabled = BtAsignar.Enabled = BtCambiar.Enabled = false;

                BtCancel.Enabled = true;
                Panel1.Visible = true;
                
            }
            else if (bServicioSactaAct && string.Compare(strSectorizacion, ID_SECTORIZACION_SACTA) == 0)
            {
                if (!string.IsNullOrEmpty(strNombreSector) && objListaTipoSector.Contains(strNombreSector) &&
                     (string)(objListaTipoSector[strNombreSector]) == ID_SECTOR_MANTENIMIENTO)
                {
                    ViewState["IdTop"] = itop.Text;
                    //Si  está modificando la sectorización SACTA, el servicio SACTA está activo y el sector es de tipo mantenimiento. Se permite liberar y asignar sectores de mantenimiento.
                    if (lBoxUsuariosAux != null && lBoxUsuariosAux.Items.Count > 0)
                    {
                        BtLiberar.Enabled = true && PermisoSegunPerfil;
                        BtAsignar.Enabled = false;
                        BtExplorar.Enabled = true;
                    }
                    else
                    {
                        BtLiberar.Enabled = false;
                        BtAsignar.Enabled = true && PermisoSegunPerfil;
                        BtExplorar.Enabled = false;
                    }
                }
                else
                {
                    BtLiberar.Enabled = false;
                    if (lBoxUsuariosAux != null && lBoxUsuariosAux.Items.Count > 0)
                    {
                        BtExplorar.Enabled = true;
                        BtAsignar.Enabled = false;
                    }
                    else
                    {
                        BtExplorar.Enabled = false;
                        BtAsignar.Enabled = true;
                    }
                }

                BtCambiar.Enabled = false;


                ViewState["IdSector"] = strNombreSector;
                if (datos != null && datos.Tables.Count > 0 && datos.Tables[0].Rows.Count > 0)
                {
                foreach (System.Data.DataRow ds in datos.Tables[0].Rows)
                    if ((ds["IdSector"] != DBNull.Value) && ((string)ds["IdSector"] == (string)ViewState["IdSector"]))
                        ViewState["idnucleo"] = (string)ds["IdNucleo"];
                }

                BtCancel.Enabled = true;
                Panel1.Visible = true;
                
            }
            else if (TopIntercambio == 0 && UsuarioSeleccionado.Count == 0)	// No se ha pulsado para intercambiar un usuario o sector
            {
                //Button itop = (Button)TUCS.FindControl("TBox" + NumTopSeleccionada);
                ViewState["IdTop"] = itop.Text;

                if (ibut.ImageUrl == "~/Configuracion/Images/UCSAs.jpg")
                {
                    BtLiberar.Enabled = PermisoSegunPerfil;
                    BtExplorar.Enabled = true;
                    ViewState["IdSector"] = ((Button)TUCS.FindControl("TextBox" + NumTopSeleccionada)).Text;
                    if (datos != null && datos.Tables.Count > 0 && datos.Tables[0].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow ds in datos.Tables[0].Rows)
                            if ((ds["IdSector"] != DBNull.Value) && ((string)ds["IdSector"] == (string)ViewState["IdSector"]))
                                ViewState["idnucleo"] = (string)ds["IdNucleo"];
                    }
                }
                else
                {
                    ViewState["IdSector"] = "";
                    ViewState["idnucleo"] = "";
                    if (datos != null && datos.Tables.Count > 0 && datos.Tables[0].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow ds in datos.Tables[0].Rows)
                            if (ds["IdSector"] != DBNull.Value)
                            {
                                ViewState["idnucleo"] = (string)ds["IdNucleo"];
                                break;
                            }
                    }

                    BtLiberar.Enabled = false;
                    BtExplorar.Enabled = false;
                }

                if (ListBox1.SelectedIndex >= 0)
                    BtAsignar.Enabled = PermisoSegunPerfil;
                else
                    BtAsignar.Enabled = false;

//				if (BtAsignar.Enabled || BtLiberar.Enabled)
				{
                    //InhabilitarElementos();
					if ((string)ViewState["idnucleo"] != "")
					{
						ListBox lBoxUsuarios = (ListBox)TUCS.FindControl("BUsuarios" + NumTopSeleccionada);
						BtCambiar.Enabled = BtLiberar.Enabled = lBoxUsuarios.SelectedIndex >= 0 && PermisoSegunPerfil;
						
						Panel1.Visible = true;
                        
					}
					else
					{
						ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
						ServiciosCD40.Nucleos t = new ServiciosCD40.Nucleos();
						Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
						KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
						t.IdSistema = (string)Session["idsistema"];
						ServiciosCD40.Tablas[] d = g.ListSelectSQL(t);
						if (d.Length > 0)
						{
                            DListNucleo.Items.Clear();
							for (int i = 0; i < d.Length; i++)
								DListNucleo.Items.Add(((ServiciosCD40.Nucleos)d[i]).IdNucleo);

							PanelNucleo.Visible = true;
						}
						else
							cMsg.alert((string)GetGlobalResourceObject("Espaniol", "NoHayNucleo"));
					}
                }
            }
            else
            {
                bNointercambiarTop = false;
                // Actualizar la top desde donde se mueven los sectores
				Button itopSel = (Button)TUCS.FindControl("TBox" + NumTopSeleccionada);
				itop = (Button)TUCS.FindControl("TBox" + TopIntercambio);
                //ViewState["IdTop"] = itop.Text;

				Button itopOrigen = (Button)TUCS.FindControl("TBox" + NumTopSeleccionada);
				if (//serviceServiciosCD40.SectoresManttoEnTop((string)Session["IdSistema"], (string)Session["elemento"], itop.Text) ||
						(serviceServiciosCD40.SectoresManttoEnTop((string)Session["IdSistema"], (string)Session["elemento"], itop.Text) &&
                        ListaUsuariosEnTop[NumTopSeleccionada - 1].Count > 0))
				{
					TopIntercambio = 0;
					UsuarioSeleccionado.Clear();
					CambiaAspectoTablaUcs(false); 
					cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "SectoresManttoSolos"), "SectoresManttoSolos");
					return;
				}
                // Si se pretende asignar un sector en una top que tiene asignado un sector de mantenimiento, este debe eliminarse de la top
                if (serviceServiciosCD40.SectoresManttoEnTop((string)Session["IdSistema"], (string)Session["elemento"], itopSel.Text))
                {
                    LiberarTop(itopSel.Text);
                }

                foreach (string s in UsuarioSeleccionado)
                    ListaUsuariosEnTop[TopIntercambio - 1].Remove(s);
                GenerarTop(TopIntercambio, false,false);

                // Actualizar la top final
                //itop = (Button)TUCS.FindControl("TBox" + NumTopSeleccionada);
                ViewState["IdTop"] = itop.Text;
                foreach (string s in UsuarioSeleccionado)
                    ListaUsuariosEnTop[NumTopSeleccionada - 1].Add(s);
                
				GenerarTop(NumTopSeleccionada, true,false);


				Top t = new Top();
				t.IdSistema = (string)Session["IdSistema"];
				t.IdTop = itopSel.Text;
				ServiciosCD40.Tablas[] tops = serviceServiciosCD40.ListSelectSQL(t);
				if (tops.Length == 0)
					return;
				uint numTopSeleccionada = ((Top)tops[0]).PosicionSacta;
				t.IdTop = itop.Text;
				tops = serviceServiciosCD40.ListSelectSQL(t);
				if (tops.Length == 0)
					return;
				uint numTopIntercambio = ((Top)tops[0]).PosicionSacta;
               
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    switch (sincro.MoverSectores(ListBox1.SelectedValue, ListaUsuariosEnTop[TopIntercambio - 1].ToArray(), (int)numTopIntercambio, ListaUsuariosEnTop[NumTopSeleccionada - 1].ToArray(), (int)numTopSeleccionada))
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

                // Generar la sectorización en el servidor
                //ServiciosCD40.ServiciosCD40 serviceServiciosCD40 = new ServiciosCD40.ServiciosCD40();
				Application["Sectorizando"] = true;
				//Session.Add("Sectorizando", true);
				BtActivar.Enabled = false;
				serviceServiciosCD40.BeginGenerarSectorizacion((string)Session["IdSistema"], (string)Session["elemento"], false, CallbackCompletado, null);
                //VMG 04/03/2019
                Page.ClientScript.RegisterStartupScript(this.GetType(), "displayNloader", "displayNloader();", true);
                TopIntercambio = 0;
                UsuarioSeleccionado.Clear();
            }

            if (bNointercambiarTop)
            {
                if (BtLiberar.Enabled)
                    ObtenerTooltip_PanelUCS_Liberar(strNombreSector);
                else
                    BtLiberar.ToolTip = string.Empty;

                if (BtExplorar.Enabled)
                    ObtenerTooltip_PanelUCS_Ver(itop.Text);
                else
                    BtExplorar.ToolTip = string.Empty;

                if (BtAsignar.Enabled)
                    ObtenerTooltip_PanelUCS_Asignar(itop.Text);
                else
                    BtAsignar.ToolTip = string.Empty;
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-UCS_OnClick): ", ex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void VerPanel_Click(object sender, EventArgs e)
    {
        Session["NombreSector"] = (string)ViewState["IdSector"];
        Session["idnucleo"] = (string)ViewState["idnucleo"];
		Session["elemento"] = VisualizandoActiva ? IdSectorizacionActiva : Session["elemento"];
		
		NumPagActual = 1;
		ActualizarPosicionesPanel();		
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void Cambiar_Click(object sender, EventArgs e)
	{
		ListBox lBoxUsuarios = (ListBox)TUCS.FindControl("BUsuarios" + NumTopSeleccionada);
		if (lBoxUsuarios != null && lBoxUsuarios.SelectedIndex != -1)
		{
			int index = 1;

			Panel1.Visible = false;

			// Guardar usuario seleccionado y top.
			TopIntercambio = NumTopSeleccionada;
			foreach (ListItem item in lBoxUsuarios.Items)
			{
				if (item.Selected)
					UsuarioSeleccionado.Add(item.Text);
			}

			int numRow = 1;
			foreach (TableRow tr in TUCS.Rows)
			{
				if (tr.ID == "Row" + numRow)
				{
					foreach (TableCell tc in tr.Cells)
					{
						if (tc.Visible)
						{
							ImageButton ibut = (ImageButton)TUCS.FindControl("ImageButton" + index);
							ibut.ImageUrl = "~/Configuracion/Images/UCS.gif";
						}
						index++;
					}
					numRow++;
				}
			}
			// Intercambiar un usuario de esta top a otra.
			//string idUsuario = lBoxUsuarios.SelectedValue;
			//ListaUsuariosEnTop[NumTopSeleccionada - 1].Remove(idUsuario);
			//GenerarTop();
		}
	}

    /// <summary>
    /// 
    /// </summary>
	private void CargaDestinosInternos()
	{
        try
        {
            ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.Tablas[] d = g.DestinosTelefoniaSectorizados((string)Session["idsistema"], (string)Session["NombreSector"], (string)Session["elemento"], true);
            DatosInternos = d;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-CargaDestinosInternos): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
	private void CargaDestinosExternos()
	{
        try
        {
            ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.Tablas[] d = g.DestinosTelefoniaSectorizados((string)Session["idsistema"], (string)Session["NombreSector"], (string)Session["elemento"], false);
            DatosExternos = d;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-CargaDestinosExternos): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
	private void CargaDestinosRadio()
	{
        try
        {
            ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.Tablas[] d = g.DestinosRadioSectorizados((string)Session["idsistema"], (string)Session["NombreSector"], (string)Session["elemento"]);
            DatosRadio = d;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-CargaDestinosRadio): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
	private void ActualizarPosicionesPanel()
	{
		NumPagActual = 1;
		LblPagina.Text = GetLocalResourceObject("LblPaginaResource1.Text").ToString() + " " + NumPagActual;
		LblPanel.Visible = LblPagina.Visible = true;
		PanelActiva.Visible = false;
		
		ObtenerDatosSector();
		ObtenerDatosGeneralesSector();
		ObtenerUtilidadesSector();
		ObtenerNivelesSector();
        ObtenerPermisosRedes((string)Session["idsistema"], (string)Session["NombreSector"]);

		CargaDestinosInternos();
		CargaDestinosExternos();
		CargaDestinosRadio();
		LimpiarPanel();
		MostrarInternos();
		MostrarExternos();
		MostrarRadio();

		ListBox1.Visible = BtNuevo.Visible = BtActivar.Visible = BtEliminar.Visible = false;
		TUCS.Enabled = true;
		ListBox1.Enabled = true;
		BtNuevo.Visible = PermisoSegunPerfil;
        //MVO: el botón eliminar sólo es visible si la sectorización no está activa
        BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0 && ListBox1.SelectedValue != LIdSectorizacion.Text;

        BtActivar.Enabled = PermisoSegunPerfil  && (Application["Sectorizando"] != null ? !(bool)Application["Sectorizando"] : true);
        BtActivar.Enabled &= (bSactaActivoEnCnf && (EstaServicioSactaActivo() == false));

		TEnlacesInternos.Visible = true;
		LblPanel.Text = GetLocalResourceObject("LBPanelTelResource1.Text").ToString();	// "Panel Telefonía";

		BtnVolverASectorizacion.Visible = BtnPaginaMenos.Visible = BtnPaginaMas.Visible = true;
		LBPanelTel.Visible = LBPanelRadio.Visible = LBPanelLC.Visible = true;
		LblSector.Visible = true;

		TUCS.Visible = false;
		Panel1.Visible = false;
        MultiView1.ActiveViewIndex = 0;

		ModalPopupExtender1.Show();		//PnlParametrosSector.Visible = true;

		IBParametros.Visible = IBPermisosRedes.Visible = IBPropiedadesGenerales.Visible = IBUtilidades.Visible = IBNivelesIntrusion.Visible = true;
		IBPropiedadesGenerales.ImageUrl = GetLocalResourceObject("IBPropiedadesGeneralesResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuTifxPropGeneralesSelected.JPG";
		IBUtilidades.ImageUrl = GetLocalResourceObject("IBUtilidadesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorUtilidadesUnSelected.JPG";
		IBParametros.ImageUrl = GetLocalResourceObject("IBParametrosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorParametrosUnSelected.JPG";
		IBNivelesIntrusion.ImageUrl = GetLocalResourceObject("IBNivelesIntrusionResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorNivelesIntrusionUnSelected.JPG";
		IBPermisosRedes.ImageUrl = GetLocalResourceObject("IBPermisosRedesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorPermisosRedesUnSelected.JPG";
	}

    /// <summary>
    /// 
    /// </summary>
	private void MostrarRadio()
	{
		if (DatosRadio != null)
		{
			int i;
			for (i = 0; i < (int)(ParametrosSector.NumFrecPagina); i++)
			{
				uint fila = (uint)((i % ParametrosSector.NumFrecPagina) / TEnlacesRadio.Rows[0].Cells.Count);
				uint col = (uint)((i % ParametrosSector.NumFrecPagina) % TEnlacesRadio.Rows[0].Cells.Count);

				TableCell tCell = (TableCell)TEnlacesRadio.FindControl("CeldaRadio" + (fila + 1) + (col + 1));
				if (tCell != null)
				{
					tCell.Visible = true;
				}
			}

			for (i = 0; i < DatosRadio.Length; i++)
			{
				int posini = (int)(((NumPagActual - 1) * (int)(ParametrosSector.NumFrecPagina)) + 1);
				int posfin = (int)(posini + ParametrosSector.NumFrecPagina);
				uint pos = ((ServiciosCD40.Radio)DatosRadio[i]).PosHMI;
				uint fila = (uint)(((pos - 1) % ParametrosSector.NumFrecPagina) / TEnlacesRadio.Rows[0].Cells.Count);
				uint col = (uint)(((pos - 1) % ParametrosSector.NumFrecPagina) % TEnlacesRadio.Rows[0].Cells.Count);

				if (pos >= posini && pos < posfin)
				{
					ImageButton ibut = (ImageButton)TEnlacesRadio.FindControl("ImageButtonRadio" + (fila + 1) + (col + 1));
					TextBox tbox = (TextBox)TEnlacesRadio.FindControl("TextBoxRadio" + (fila + 1) + (col + 1));
					if (ibut != null)
						//ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceExternoAs.jpg";
                        ibut.ImageUrl = "";
					if (tbox != null)
						tbox.Text = ((ServiciosCD40.Radio)DatosRadio[i]).Literal;
				}
			}
		}
	}

    /// <summary>
    /// 
    /// </summary>
	private void ObtenerDatosSector()
	{
        try
        {
            LblSector.Text = (string)Session["NombreSector"];

            //ServiciosCD40.ServiciosCD40 serviceServiciosCD40 = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.ParametrosSector s = new ServiciosCD40.ParametrosSector();
            s.IdSistema = (string)Session["idsistema"];
            s.IdSector = (string)Session["NombreSector"];
            s.IdNucleo = (string)Session["idnucleo"];

            ServiciosCD40.Tablas[] sector = serviceServiciosCD40.ListSelectSQL(s);
            if (sector.Length > 0)
            {
                ParametrosSector.IdSistema = ((ServiciosCD40.ParametrosSector)sector[0]).IdSistema;
                ParametrosSector.IdNucleo = ((ServiciosCD40.ParametrosSector)sector[0]).IdNucleo;
                ParametrosSector.IdSector = ((ServiciosCD40.ParametrosSector)sector[0]).IdSector;
                ParametrosSector.NumDestinosInternosPag = ((ServiciosCD40.ParametrosSector)sector[0]).NumDestinosInternosPag;
                ParametrosSector.NumEnlacesInternosPag = ((ServiciosCD40.ParametrosSector)sector[0]).NumEnlacesInternosPag;
                ParametrosSector.NumEnlacesAI = ((ServiciosCD40.ParametrosSector)sector[0]).NumEnlacesAI;
                ParametrosSector.NumFrecPagina = ((ServiciosCD40.ParametrosSector)sector[0]).NumFrecPagina;
                ParametrosSector.NumLlamadasEnIda = ((ServiciosCD40.ParametrosSector)sector[0]).NumLlamadasEnIda;
                ParametrosSector.NumLlamadasEntrantesIda = ((ServiciosCD40.ParametrosSector)sector[0]).NumLlamadasEntrantesIda;
                ParametrosSector.NumPagDestinosInt = ((ServiciosCD40.ParametrosSector)sector[0]).NumPagDestinosInt;
                ParametrosSector.NumPagEnlacesInt = ((ServiciosCD40.ParametrosSector)sector[0]).NumPagEnlacesInt;
                ParametrosSector.NumPagFrec = ((ServiciosCD40.ParametrosSector)sector[0]).NumPagFrec;

				TxtKAP.Text = ((ServiciosCD40.ParametrosSector)sector[0]).KeepAlivePeriod.ToString();
				TxtKAM.Text = ((ServiciosCD40.ParametrosSector)sector[0]).KeepAliveMultiplier.ToString();
				TxtEnlIntPag.Text = ((ServiciosCD40.ParametrosSector)sector[0]).NumEnlacesInternosPag.ToString();
				TxtFrecPag.Text = ((ServiciosCD40.ParametrosSector)sector[0]).NumFrecPagina.ToString();
				TxtLlamEntIDA.Text = ((ServiciosCD40.ParametrosSector)sector[0]).NumLlamadasEntrantesIda.ToString();
				TxtLlamIDA.Text = ((ServiciosCD40.ParametrosSector)sector[0]).NumLlamadasEnIda.ToString();
				TxtPagEnlInt.Text = ((ServiciosCD40.ParametrosSector)sector[0]).NumPagEnlacesInt.ToString();
				TxtPagFrec.Text = ((ServiciosCD40.ParametrosSector)sector[0]).NumPagFrec.ToString();

				// Se muestran en panel de parámetros generales.
				CheckIntruido.Checked = ((ServiciosCD40.ParametrosSector)sector[0]).Intruido;
				CheckIntrusion.Checked = ((ServiciosCD40.ParametrosSector)sector[0]).Intrusion;
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-ObtenerDatosSector): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	private DataSet ObtenerDatosGeneralesSector()
	{
		ServiciosCD40.Sectores s = new ServiciosCD40.Sectores();
		s.IdSistema = (string)Session["idsistema"];
		s.IdSector = (string)Session["NombreSector"];
		s.IdNucleo = (string)Session["idnucleo"];

		ServiciosCD40.Tablas[] sector = serviceServiciosCD40.ListSelectSQL(s);
		if (sector.Length > 0)
		{
			TxtIdSector.Text = s.IdSector;
			
			DDLPrioridadR2.SelectedValue = ((ServiciosCD40.Sectores)sector[0]).PrioridadR2.ToString();
			
			DListNucleoParametros.Items.Add(s.IdNucleo);
			DListNucleoParametros.SelectedIndex = 0;

			DListTipoSector.SelectedValue = ((ServiciosCD40.Sectores)sector[0]).Tipo;
			DListTipoPosicion.SelectedValue = ((ServiciosCD40.Sectores)sector[0]).TipoPosicion;

			DataSet d = serviceServiciosCD40.RedesUsuariosAbonados(s.IdSistema, s.IdNucleo, s.IdSector);
			GVAbonados.DataSource = DSNumerosAbonados = d;
			GVAbonados.DataBind();

			return d;
		}

		return null;
	}

    /// <summary>
    /// 
    /// </summary>
	private void ObtenerUtilidadesSector()
	{
		try
		{
			// Obtiene los datos del sector dominante que compone la agrupación
            ServiciosCD40.TeclasSector t = new ServiciosCD40.TeclasSector();
            t.IdSistema = (string)Session["idsistema"];
            t.IdSector = (string)Session["NombreSector"];
            t.IdNucleo = (string)ViewState["idnucleo"];
            ServiciosCD40.Tablas[] datosSect = serviceServiciosCD40.ListSelectSQL(t);
            
            //DataSet datos = serviceServiciosCD40.TeclasSector((string)Session["idsistema"], (string)Session["NombreSector"]);

            if (datosSect !=null && datosSect.Length > 0)
			{
				//DataRow teclaRow = datos.Tables[0].Rows[0];
                CheckTransPre.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).TransConConsultaPrev;
                CheckTransDirect.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).TransDirecta;
                CheckConferencia.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).Conferencia;
                CheckEscucha.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).Escucha;
                CheckRetener.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).Retener;
                CheckCaptura.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).Captura;
                CheckRedireccion.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).Redireccion;
                CheckRepUltLlamada.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).RepeticionUltLlamada;
                CheckReAutomatica.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).RellamadaAut;
                CheckTeclaPrio.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).TeclaPrioridad;
                CheckTecla55.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).Tecla55mas1;
                CheckMonitoring.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).Monitoring;
                CheckCoordTF.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).CoordinadorTF;
                CheckCoorRD.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).CoordinadorRD;
                CheckIntRDTF.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).IntegracionRDTF;
                CheckLlamadaSelect.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).LlamadaSelectiva;
                CheckBSS.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).GrupoBSS;
                CheckLTT.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).LTT;
                CheckSayAgain.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).SayAgain;
                CheckRediCA.Checked = ((ServiciosCD40.TeclasSector)datosSect[0]).InhabilitacionRedirec;
			}
		}
		catch (Exception ex)
		{
			logDebugView.Error("(Sectorizaciones-ObtenerUtilidadesSector): ", ex);
		}
	}

    /// <summary>
    /// 
    /// </summary>
	private void ObtenerNivelesSector()
	{
		// Obtiene los datos del sector dominante que compone la agrupación
        DataSet datosSect = serviceServiciosCD40.NivelesIntrusion((string)Session["idsistema"], (string)Session["NombreSector"]);

        if (datosSect != null && datosSect.Tables.Count>0 && datosSect.Tables[0].Rows.Count > 0)
		{
            DataRow rowNiveles = datosSect.Tables[0].Rows[0];
			if (rowNiveles["CICL"] != System.DBNull.Value)
				DDLCicl.SelectedValue = Convert.ToString(rowNiveles["CICL"]);
			if (rowNiveles["CIPL"] != System.DBNull.Value)
				DDLCipl.SelectedValue = Convert.ToString(rowNiveles["CIPL"]);
			if (rowNiveles["CPICL"] != System.DBNull.Value)
				DDLCpicl.SelectedValue = Convert.ToString(rowNiveles["CPICL"]);
			if (rowNiveles["CPIPL"] != System.DBNull.Value)
				DDLCpipcl.SelectedValue = Convert.ToString(rowNiveles["CPIPL"]);
		}
	}

    private void ObtenerPermisosRedes(string idSistema, string idSector)
    {
        DataSet DSPermisosRedes = serviceServiciosCD40.PermisoRedesSector(idSistema, idSector);

        GVPermisosRedes.Visible = true;
        if (DSPermisosRedes.Tables.Count > 0)
        {
            GVPermisosRedes.DataSource = DSPermisosRedes;
            GVPermisosRedes.DataBind();
        }
    }

    /// <summary>
    /// 
    /// </summary>
	private void MostrarInternos()
	{
        try
        {
            if (DatosInternos != null)
            {
                for (int i = 0; i < DatosInternos.Length; i++)
                {
                    if (((ServiciosCD40.Internos)DatosInternos[i]).TipoAcceso == "DA")
                    {
                        int posini = (int)(((NumPagActual - 1) * (int)(ParametrosSector.NumDestinosInternosPag)) + 1);
                        int posfin = (int)(posini + ParametrosSector.NumDestinosInternosPag);
                        uint pos = ((ServiciosCD40.Internos)DatosInternos[i]).PosHMI;
                        uint fila = (uint)(((pos - 1) % ParametrosSector.NumDestinosInternosPag) / TEnlacesInternos.Rows[0].Cells.Count);
                        uint col = (uint)(((pos - 1) % ParametrosSector.NumDestinosInternosPag) % TEnlacesInternos.Rows[0].Cells.Count);

                        if (pos >= posini && pos < posfin)
                        {
                            ImageButton ibut = (ImageButton)TEnlacesInternos.FindControl("IB" + (fila + 1) + (col + 1));
                            TextBox tbox = (TextBox)TEnlacesInternos.FindControl("TB" + (fila + 1) + (col + 1));
                            if (ibut != null)
                                //ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceInternoAs.jpg";
                                ibut.ImageUrl = "";
                            if (tbox != null)
                                tbox.Text = ((ServiciosCD40.Internos)DatosInternos[i]).Literal;
                        }
                    }
                    else if (((ServiciosCD40.Internos)DatosInternos[i]).TipoAcceso == "IA")
                    {
                        int posini = 1;
                        int posfin = 18;
                        uint pos = ((ServiciosCD40.Internos)DatosInternos[i]).PosHMI;
                        uint fila = (uint)((pos - 1)  / TEnlacesLC.Rows[0].Cells.Count);
                        uint col = (uint)((pos - 1)  % TEnlacesLC.Rows[0].Cells.Count);

                        if ((pos >= posini) && (pos <= posfin))
                        {
                            //ImageButton ibut = (ImageButton)TEnlacesLC.FindControl("ImageButtonLC" + (fila + 1) + (col + 1));
                            TextBox tbox = (TextBox)TEnlacesLC.FindControl("TextBoxLC" + (fila + 1) + (col + 1));
                            //if (ibut != null)
                            //{
                                //ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceLCAs.jpg";
                             //   ibut.ImageUrl = "";
                            //}
                               
                            if (tbox != null)
                                tbox.Text = ((ServiciosCD40.Internos)DatosInternos[i]).Literal;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-MostrarInternos): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
	private void MostrarExternos()
	{
        try
        {
            if (DatosExternos != null)
            {
                for (int i = 0; i < DatosExternos.Length; i++)
                {
                    if (((ServiciosCD40.Externos)DatosExternos[i]).TipoAcceso == "DA")
                    {
                        int posini = (int)(((NumPagActual - 1) * (int)(ParametrosSector.NumDestinosInternosPag)) + 1);
                        int posfin = (int)(posini + ParametrosSector.NumDestinosInternosPag);
                        uint pos = ((ServiciosCD40.Externos)DatosExternos[i]).PosHMI;
                        uint fila = (uint)(((pos - 1) % ParametrosSector.NumDestinosInternosPag) / TEnlacesInternos.Rows[0].Cells.Count);
                        uint col = (uint)(((pos - 1) % ParametrosSector.NumDestinosInternosPag) % TEnlacesInternos.Rows[0].Cells.Count);
                        if (pos >= posini && pos < posfin)
                        {
                            ImageButton ibut = (ImageButton)TEnlacesInternos.FindControl("IB" + (fila + 1) + (col + 1));
                            TextBox tbox = (TextBox)TEnlacesInternos.FindControl("TB" + (fila + 1) + (col + 1));
                            if (ibut != null)
                                //ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceInternoAs.jpg";
                                ibut.ImageUrl = "";
                            if (tbox != null)
                                tbox.Text = ((ServiciosCD40.Externos)DatosExternos[i]).Literal;
                        }
                    }
                    else if (((ServiciosCD40.Externos)DatosExternos[i]).TipoAcceso == "IA" && ((ServiciosCD40.Externos)DatosExternos[i]).IdPrefijo == 3)
                    {
                        //En el panel de LC sólo se muestran los destinos ATS (IdPrefijo==3)
                        //Porque en la tabla externos puede haber destinos de telefonía LCEN asociados a destino ATS.
                        //Estos destinos LCEN  se configuran en la misma posición que el destino ATS.
                        int posini = 1;
                        int posfin = 18;
                        uint pos = ((ServiciosCD40.Externos)DatosExternos[i]).PosHMI;
                        uint fila = (uint)((pos - 1) / TEnlacesLC.Rows[0].Cells.Count);
                        uint col = (uint)((pos - 1) % TEnlacesLC.Rows[0].Cells.Count);

                        if (pos >= posini && pos <= posfin)
                        {
                            //ImageButton ibut = (ImageButton)TEnlacesLC.FindControl("ImageButtonLC" + (fila + 1) + (col + 1));
                            TextBox tbox = (TextBox)TEnlacesLC.FindControl("TextBoxLC" + (fila + 1) + (col + 1));
                            //if (ibut != null)
                                //ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceLCAs.jpg";
                              //  ibut.ImageUrl = "";
                            if (tbox != null)
                                tbox.Text = ((ServiciosCD40.Externos)DatosExternos[i]).Literal;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-MostrarExternos): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
	private void LimpiarPanel()
	{
        try
        {
			//int fila, col;
			for (int i = 0; i < (int)(ParametrosSector.NumDestinosInternosPag); i++)
			{
				uint fila = (uint)((i % ParametrosSector.NumDestinosInternosPag) / TEnlacesInternos.Rows[0].Cells.Count);
				uint col = (uint)((i % ParametrosSector.NumDestinosInternosPag) % TEnlacesInternos.Rows[0].Cells.Count);
				TableCell tCell = (TableCell)TEnlacesInternos.FindControl("C" + (fila + 1) + (col + 1));
				if (tCell != null)
				{
					tCell.Visible = true;
                    //ImageButton ibut = (ImageButton)TEnlacesInternos.FindControl("IB" + (fila + 1) + (col + 1));
                    TextBox tbox = (TextBox)TEnlacesInternos.FindControl("TB" + (fila + 1) + (col + 1));
                    //if (ibut != null) ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceInterno.jpg";
                    if (tbox != null) tbox.Text = "";
				}
            }

            for (int fila = ParametrosSector.NumEnlacesAI > 9 ? 0 : 1; fila < TEnlacesLC.Rows.Count; fila++)
            {
                for (int col = 0; col < TEnlacesLC.Rows[fila].Cells.Count; col++)
                {
                    TEnlacesLC.Rows[fila].Cells[col].Visible = true;
                    //ImageButton ibut = (ImageButton)TEnlacesLC.FindControl("ImageButtonLC" + (fila + 1) + (col + 1));
                    TextBox tbox = (TextBox)TEnlacesLC.FindControl("TextBoxLC" + (fila + 1) + (col + 1));
                    //if (ibut != null) ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceLC.jpg";
                    if (tbox != null) tbox.Text = "";
                }
            }

			for (int i = 0; i < (int)(ParametrosSector.NumFrecPagina); i++)
			{
				uint fila = (uint)((i % ParametrosSector.NumFrecPagina) / TEnlacesRadio.Rows[0].Cells.Count);
				uint col = (uint)((i % ParametrosSector.NumFrecPagina) % TEnlacesRadio.Rows[0].Cells.Count);

				TableCell tCell = (TableCell)TEnlacesRadio.FindControl("CeldaRadio" + (fila + 1) + (col + 1));
				if (tCell != null)
				{
					tCell.Visible = true;
                    //ImageButton ibut = (ImageButton)TEnlacesRadio.FindControl("ImageButtonRadio" + (fila + 1) + (col + 1));
                    TextBox tbox = (TextBox)TEnlacesRadio.FindControl("TextBoxRadio" + (fila + 1) + (col + 1));
                    //if (ibut != null) ibut.ImageUrl = "~/Configuracion/Images/BotonEnlaceExterno.jpg";
                    if (tbox != null) tbox.Text = "";
				}
			}

        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-LimpiarPanel): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtLiberar_Click(object sender, EventArgs e)
    {
        try
        {
            ListBox lBoxUsuarios = (ListBox)TUCS.FindControl("BUsuarios" + NumTopSeleccionada);
            if (lBoxUsuarios != null)
            {
                foreach (ListItem item in lBoxUsuarios.Items)
                {
					if (item.Selected)
					{
						ListaUsuariosEnTop[NumTopSeleccionada - 1].Remove(item.Text);

						// Crear evento en históricos
						Button itop = (Button)TUCS.FindControl("TBox" + NumTopSeleccionada);
						serviceServiciosCD40.GeneraHistoricoSectorAsignado((string)Session["IdSistema"], false, item.Text, itop != null ? itop.Text : "");
					}
                }

				Top t = new Top();
				t.IdSistema = (string)Session["IdSistema"];
				t.IdTop = (string)ViewState["IdTop"];
				ServiciosCD40.Tablas[] tops = serviceServiciosCD40.ListSelectSQL(t);
				if (tops.Length == 0)
					return;

				t.PosicionSacta = ((Top)tops[0]).PosicionSacta;
				
				Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    //sincro.LiberarUCS(ListBox1.SelectedValue, NumTopSeleccionada);
                    switch ((ListaUsuariosEnTop[NumTopSeleccionada - 1].Count>0)?
							 sincro.AsignarSectoresUCS(ListBox1.SelectedValue, ListaUsuariosEnTop[NumTopSeleccionada - 1].ToArray(), (int)t.PosicionSacta) : 123)
                    {
                        case 123:
                            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod127"));
                            break;
                        default:
                            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod128"));
                            break;
                    }
                }

                //string idUsuario = lBoxUsuarios.SelectedValue;
                //ListaUsuariosEnTop[NumTopSeleccionada - 1].Remove(idUsuario);
                GenerarTop(NumTopSeleccionada, true,true);
                // Generar la sectorización en el servidor
                //ServiciosCD40.ServiciosCD40 serviceServiciosCD40 = new ServiciosCD40.ServiciosCD40();
				Application["Sectorizando"] = true;
				//Session.Add("Sectorizando", true);
				BtActivar.Enabled = false;
				serviceServiciosCD40.BeginGenerarSectorizacion((string)Session["IdSistema"], (string)Session["elemento"], false, CallbackCompletado, null);
                //VMG 04/03/2019
                Page.ClientScript.RegisterStartupScript(this.GetType(), "displayNloader", "displayNloader();", true);
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-BtLiberar_Click): ", ex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idtop"></param>
    private void LiberarTop(string idtop)
    {
        try
        {
            ServiciosCD40.SectoresSectorizacion s = new ServiciosCD40.SectoresSectorizacion();
            s.IdSistema = (string)Session["idsistema"];
            s.IdSectorizacion = ListBox1.SelectedValue;
            s.IdTop = idtop;

            if (!string.IsNullOrEmpty(s.IdSistema) && !string.IsNullOrEmpty(s.IdSectorizacion) && !string.IsNullOrEmpty(s.IdTop))
            {
                // Eliminar los sectores asignados a esta top en esta sectorización sólo si se informan el idSistema, la sectorización y el idTop
                // para evitar que se puedan eliminar todas las sectorizaciones si estos valores no se informan antes de llamar a la función

                if (serviceServiciosCD40.DeleteSQL(s) < 0) logDebugView.Warn("(Sectorizaciones-LiberarTop): No se ha podido eliminar SectoresSectorizacion");
                else
                    ListaUsuariosEnTop[NumTopSeleccionada - 1].Clear();
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-LiberarTop): ", ex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtCancelPanel_Click(object sender, EventArgs e)
    {
		Panel1.Visible = false;
        HabilitarElementos();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtAsignar_Click(object sender, EventArgs e)
    {
        string strSistema = (string)Session["idsistema"];
        CargarSectoresPanel(strSistema,(string)ViewState["idnucleo"],ListBox1.SelectedValue);
		if (LBoxSectores.Items.Count > 0)
			PanelSectores.Visible = true;
		else
			cMsg.alert((string)GetGlobalResourceObject("Espaniol","NoSectores"));
    }

    /// <summary>
    /// 
    /// </summary>
    private void InhabilitarElementos()
    {
        TUCS.Enabled = false;
        ListBox1.Enabled = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        BtActivar.Enabled = false;

		TUCS.Visible = false;
		Panel1.Visible = false;
	}

    /// <summary>
    /// 
    /// </summary>
    private void HabilitarElementos()
    {
        TUCS.Enabled = true;
        ListBox1.Enabled = true;
        BtNuevo.Visible = PermisoSegunPerfil;
    }

    /// <summary>
    /// Asignar un sector a una top
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtAsignaSector_Click(object sender, EventArgs e)
    {
        try
        {
            if (LBoxSectores.SelectedIndex >= 0)
            {
                // Los sectores de mantenimiento no se pueden agrupar con ningun otro sector
				if ((DataSetSectoresLibres.Tables[0].Rows[LBoxSectores.SelectedIndex][1].ToString() == "M" && ListaUsuariosEnTop[NumTopSeleccionada - 1].Count > 0))
				{
					TopIntercambio = 0;
					UsuarioSeleccionado.Clear();
					CambiaAspectoTablaUcs(false);
					PanelSectores.Visible = false;
					Panel1.Visible = false;
					cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "SectoresManttoSolos"), "SectoresManttoSolos");
					return;
				}
                // Si se pretende asignar un sector en una top que tiene asignado un sector de mantenimiento, este debe eliminarse de la top
                if (serviceServiciosCD40.SectoresManttoEnTop((string)Session["IdSistema"], (string)Session["elemento"], (string)ViewState["IdTop"]) &&
                    (DataSetSectoresLibres.Tables[0].Rows[LBoxSectores.SelectedIndex][1].ToString() != "M"))
                {
                    LiberarTop((string)ViewState["IdTop"]);
                }

				Top t = new Top();
				t.IdSistema = (string)Session["IdSistema"];
				t.IdTop = (string)ViewState["IdTop"];
				ServiciosCD40.Tablas[] tops = serviceServiciosCD40.ListSelectSQL(t);
				if (tops.Length==0)
					return;

				t.PosicionSacta = ((Top)tops[0]).PosicionSacta;

				ListaUsuariosEnTop[NumTopSeleccionada - 1].Add(LBoxSectores.SelectedValue);

                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    //sincro.LiberarUCS(ListBox1.SelectedValue, NumTopSeleccionada);
					// cd40.top.PosicionSacta <=> cd30.UCS.NumUCS
					switch (sincro.AsignarSectoresUCS(ListBox1.SelectedValue, ListaUsuariosEnTop[NumTopSeleccionada - 1].ToArray(), (int)t.PosicionSacta))
                    {
                        case 123:
                            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod127"));
                            break;
                        default:
                            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "Cod128"));
                            break;
                    }
                }

                GenerarTop(NumTopSeleccionada, true,true);

				serviceServiciosCD40.GeneraHistoricoSectorAsignado((string)Session["IdSistema"], true, LBoxSectores.SelectedValue, (string)ViewState["IdTop"]);
				Application["Sectorizando"] = true;
				//Session.Add("Sectorizando", true);
				BtActivar.Enabled = false;

				serviceServiciosCD40.BeginGenerarSectorizacion((string)Session["IdSistema"], (string)Session["elemento"], false, CallbackCompletado, null);
                //VMG 04/03/2019
                Page.ClientScript.RegisterStartupScript(this.GetType(), "displayNloader", "displayNloader();", true);
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-Button1_Click): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numTop"></param>
    /// <param name="actualizarSectorizacion"></param>
	private void GenerarTop(int numTop, bool actualizarSectorizacion, bool bEliminarSectorAgrupado=false)
	{
        string strIdSistema = (string)Session["idsistema"];
        string strIdSectorizacion = string.Empty;

        try
        {
            bool bBorrarSectoresAgrupados = false;

            //ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.SectoresSectorizacion ss = new ServiciosCD40.SectoresSectorizacion();

            Button itop = (Button)TUCS.FindControl("TBox" + numTop);
            ViewState["IdTop"] = itop.Text;

            string strNuevoNomSector = DameNombreSector(numTop);
            string strIdNucleo = (string)ViewState["idnucleo"];
            string strNombreSectorTop = ((Button)TUCS.FindControl("TextBox" + numTop)).Text;

            //MVO: Se comprueba si el sector no es simple (agrupado) y se encuentra asignado a otras sectorizaciones
            //Se obtiene el Id del sistema
            ss.IdSistema = strIdSistema;

            if (bEliminarSectorAgrupado && !string.IsNullOrEmpty(strNombreSectorTop))
            {
                //Se obtiene el Id del sector (Nombre) del TOP
                ss.IdSector = strNombreSectorTop;
                ss.IdNucleo = strIdNucleo;

                ServiciosCD40.Tablas[] d = serviceServiciosCD40.ListSelectSQL(ss);

                if (null != d && d.Length > 0)
                {
                    //Si sólo se obtiene un elemento, el sector solo está asociado a la sectorización que estamos modificando, que es la que se quiere eliminar
                    //Si sólo está asignado a una sectorización que corresponde con la sectorización activa, se obtendrán dos registros y no se debe eliminar
                    if (1 == d.Length && ((ServiciosCD40.SectoresSectorizacion)d[0]).IdSectorizacion == ListBox1.SelectedValue)
                    {
                        bBorrarSectoresAgrupados = true;
                    }
                }
            }

            ss.IdSector = null;
            ss.IdNucleo = null;

            strIdSectorizacion = ListBox1.SelectedValue;

            ss.IdSectorizacion = strIdSectorizacion;
            ss.IdTop = itop.Text;

            if (!string.IsNullOrEmpty(ss.IdSistema) && !string.IsNullOrEmpty(ss.IdSectorizacion) && !string.IsNullOrEmpty(ss.IdTop))
            {
                // Eliminar los sectores asignados a este top en esta sectorización
                serviceServiciosCD40.DeleteSQL(ss);
            }

            //A continuación, si el sector sólo está asociado a una única sectorización y es agrupado (sectores.SectorSimple==false),
            // se elimina el sector para no dejar basura en la tabla sectores y sectoressector
            if (bBorrarSectoresAgrupados)
            {
                ServiciosCD40.Sectores objSectores = new ServiciosCD40.Sectores();

                if (null != objSectores)
                {
                    objSectores.IdSistema = ss.IdSistema;
                    objSectores.IdNucleo = strIdNucleo;
                    objSectores.IdSector = strNombreSectorTop;

                    ServiciosCD40.Tablas[] d = serviceServiciosCD40.ListSelectSQL(objSectores);

                    if (null != d && d.Length > 0)
                    {
                        //Si el sector no es simple, se elimina el sector agrupado
                        if (((ServiciosCD40.Sectores)d[0]).SectorSimple == false)
                        {
                            serviceServiciosCD40.DeleteSQL(objSectores);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(strNuevoNomSector))
            {
                // Crear el nuevo sector
                GeneraSector(strNuevoNomSector, numTop);

                // Insertar el nuevo sector en la sectorización
                ss.IdNucleo = (string)ViewState["idnucleo"];
                ss.IdSector = strNuevoNomSector;

				if (serviceServiciosCD40.InsertSQL(ss) < 0) logDebugView.Warn("(Sectorizaciones-GenerarTop): No se ha podido guardar el sector.");

                // Crear el nuevo SectoresSector
                GeneraSectoresSector(strNuevoNomSector, numTop);
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-GenerarTop): ", ex);
        }

        PanelSectores.Visible = false;
        Panel1.Visible = false;

		if (actualizarSectorizacion)
		{
			HabilitarElementos();
            CargarSectorizacion(strIdSistema, strIdSectorizacion, VisualizandoActiva);
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nomSector"></param>
    /// <param name="numTop"></param>
	private void GeneraSectoresSector(string nomSector, int numTop)
	{
        try
        {
            //ServiciosCD40.ServiciosCD40 serviceServiciosCD40 = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.SectoresSector ss = new ServiciosCD40.SectoresSector();

            foreach (string s in ListaUsuariosEnTop[numTop - 1])
            {
                ss.IdSistema = (string)Session["IdSistema"];
                ss.IdNucleo = (string)ViewState["idnucleo"];
                ss.IdSector = nomSector;
                ss.IdSectorOriginal = s;

				// Comprobar que esta combinación de sectores no existe en la tabla Sectores
				// en cuyo caso no hay nada que generar
				if (serviceServiciosCD40.ListSelectSQL(ss).Length > 0)
					continue;

                ss.EsDominante = ListaUsuariosEnTop[numTop - 1][0] == s;	// si es el primer usuario, es dominante

                if (serviceServiciosCD40.InsertSQL(ss) < 0) logDebugView.Error("(Sectorizaciones-GeneraSectoresSector): No se ha podido guardar SectoresSector");
            }
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-GeneraSectoresSector): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nomSector"></param>
    /// <param name="numTop"></param>
	private void GeneraSector(string nomSector, int numTop)
	{
        try
        {
			//ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.Sectores s = new ServiciosCD40.Sectores();

            s.IdSistema = (string)Session["IdSistema"];
            s.IdSector = nomSector;
            s.IdNucleo = (string)ViewState["idnucleo"];

			// Comprobar que esta combinación de sectores no existe en la tabla Sectores
			// en cuyo caso no hay nada que generar
            //serviceServiciosCD40.DeleteSQL(s);

            if (serviceServiciosCD40.ListSelectSQL(s).Length > 0)
            {
                return;
            }

            s.IdParejaUCS = s.IdNucleoParejaUCS = s.IdSistemaParejaUCS = null;
            s.SectorSimple = false;
            s.Tipo = "R";
            s.TipoPosicion = "C";

            // Obtener literales de los usuarios
            System.Text.StringBuilder lista = new System.Text.StringBuilder();
            foreach (string user in ListaUsuariosEnTop[numTop - 1])
            {
                lista.AppendFormat("'{0}',", user);
            }
            lista = lista.Remove(lista.Length - 1, 1);
            s.PrioridadR2 = serviceServiciosCD40.GetPrioridadSector(s.IdSistema, s.IdNucleo, lista.ToString());

            s.TipoHMI = 0;
            s.NumSacta = 0;

            // Eliminar el destino antes de crear el sector.
            // Si el destino exisitiese (¿?) no se crea el sector y da error.
            ServiciosCD40.Destinos d = new ServiciosCD40.Destinos();
            d.IdSistema = s.IdSistema;
            d.IdDestino = s.IdSector;
            d.TipoDestino = 2;
            serviceServiciosCD40.DeleteSQL(d);
            
            if (serviceServiciosCD40.InsertSQL(s) < 0) logDebugView.Error("(Sectorizaciones-GeneraSector): No se ha podido guardar el sector.");
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-GeneraSector): ", ex);
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numTop"></param>
    /// <returns></returns>
	private string DameNombreSector(int numTop)
	{
        try
        {
            if (ListaUsuariosEnTop[numTop - 1].Count == 0)
                return "";

            // Ordenar la lista por el IDSacta
			//ListaUsuariosEnTop[numTop - 1].Sort();

            System.Text.StringBuilder lista = new System.Text.StringBuilder();
            foreach (string s in ListaUsuariosEnTop[numTop - 1])
                lista.AppendFormat("'{0}',", s);
            lista = lista.Remove(lista.Length - 1, 1);

            lista = OrdenaListaUsuariosPorIdSacta(numTop - 1, lista);

            // Buscar una agrupación
            ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            string nomAgrupacion = g.GetAgrupacion((string)Session["IdSistema"], (string)ViewState["idnucleo"],ListaUsuariosEnTop[numTop - 1].Count, lista.ToString());
            // Si no la encuentra, aplicar algoritmo
			//if (nomAgrupacion == null)
			//    nomAgrupacion = GeneraAlgoritmo(numTop);

			return nomAgrupacion;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-DameNombreSector): ", ex);
        }
        return "";
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numTop"></param>
    /// <param name="lista"></param>
    /// <returns></returns>
    System.Text.StringBuilder OrdenaListaUsuariosPorIdSacta(int numTop, System.Text.StringBuilder lista)
    {
        System.Text.StringBuilder listaUsuarios = new System.Text.StringBuilder();
        if (lista == null)
        {
            foreach (string s in ListaUsuariosEnTop[numTop])
                listaUsuarios.AppendFormat("'{0}',", s);
            
            if (listaUsuarios.Length>0)
                listaUsuarios = listaUsuarios.Remove(listaUsuarios.Length - 1, 1);
        }
        else
            listaUsuarios = lista;

        ListaUsuariosEnTop[numTop].Clear();

        // Ordenar la lista por el IDSacta
        DataSet d = serviceServiciosCD40.SectoresNumSactaSorted((string)Session["IdSistema"], (string)ViewState["idnucleo"], listaUsuarios.ToString());

        if (d != null && d.Tables.Count > 0)
        {
            foreach (System.Data.DataRow ds in d.Tables[0].Rows)
            {
                ListaUsuariosEnTop[numTop].Add((string)ds[0]);
            }
        }

        return listaUsuarios;
    }

	/*
	private string GeneraAlgoritmo(int numTop)
	{
		if (ListaUsuariosEnTop.Length==0)
			return "";

		System.Text.StringBuilder nombreAlgoritmo=new System.Text.StringBuilder();
		System.Text.StringBuilder lista = new System.Text.StringBuilder();
		foreach (string s in ListaUsuariosEnTop[numTop - 1])
			lista.AppendFormat("{0},", s);
        lista = lista.Remove(lista.Length - 1, 1);

		return serviceServiciosCD40.GeneraAlgoritmo((string)Session["idsistema"], lista.ToString());
	}
	*/

    /// <summary>
    /// 
    /// </summary>
    /// <param name="elnucleo"></param>
	private void CargarSectoresPanel(string strSistema, string elnucleo,string strIdSectorizacion)
    {
        if (DataSetSectoresLibres != null && DataSetSectoresLibres.Tables.Count > 0)
            DataSetSectoresLibres.Clear();

        if (LBoxSectores.Items.Count > 0)
            LBoxSectores.Items.Clear();

        try
        {
            //Si la sectorización no es SACTA, el procedimiento devuelve todos los sectores que están sin asignar en la sectorización
            //Si la sectorización es SACTA, el procedimiento sólo devolverá los sectores de mantenimiento que están sin asignar
            DataSetSectoresLibres = serviceServiciosCD40.SectoresSinAsignarASectorizacion(strSistema, elnucleo, strIdSectorizacion);
            LBoxSectores.DataTextField = "IdSector";
            LBoxSectores.DataSource = DataSetSectoresLibres;
            LBoxSectores.DataBind();

        }
        catch (Exception ex)
        {
            logDebugView.Error("(Sectorizaciones-CargarSectoresPanel): ", ex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtCerrar_Click(object sender, EventArgs e)
    {
        PanelSectores.Visible = false;
        Panel1.Visible = false;
        HabilitarElementos();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtAceptarNucleo_Click(object sender, EventArgs e)
    {
        ViewState["idnucleo"] = DListNucleo.SelectedValue;
        PanelNucleo.Visible = false;
        Panel1.Visible = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void LBPanelTel_Click(object sender, EventArgs e)
	{
		NumPagActual = 1;
		LblPagina.Text = GetLocalResourceObject("LblPaginaResource1.Text").ToString() + " " + NumPagActual;
		LblPanel.Visible = LblPagina.Visible = true;
		LblPanel.Text = GetLocalResourceObject("LBPanelTelResource1.Text").ToString();

		TEnlacesInternos.Visible = true;
		TEnlacesLC.Visible = TEnlacesRadio.Visible = false;

		BtnPaginaMas.Visible = BtnPaginaMenos.Visible = true;

		MostrarExternos();
		MostrarInternos();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void LBPanelRadio_Click(object sender, EventArgs e)
	{
		NumPagActual = 1;
		LblPagina.Text = GetLocalResourceObject("LblPaginaResource1.Text").ToString() + " " + NumPagActual;
		LblPanel.Visible = LblPagina.Visible = true;
		LblPanel.Text = GetLocalResourceObject("LBPanelRadioResource1.Text").ToString();

		TEnlacesRadio.Visible = true;
		TEnlacesLC.Visible = TEnlacesInternos.Visible = false;

		BtnPaginaMas.Visible = BtnPaginaMenos.Visible = true;

		MostrarRadio();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void LBPanelLC_Click(object sender, EventArgs e)
	{
		NumPagActual = 1;
		LblPagina.Text = GetLocalResourceObject("LblPaginaResource1.Text").ToString() + " " + NumPagActual;
		LblPanel.Visible = LblPagina.Visible = true;
		LblPanel.Text = GetLocalResourceObject("LBPanelLCResource1.Text").ToString();

		TEnlacesLC.Visible = true;
		TEnlacesRadio.Visible = TEnlacesInternos.Visible = false;

		BtnPaginaMas.Visible = BtnPaginaMenos.Visible = false;

		MostrarExternos();
		MostrarInternos();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnVolverASectorizacion_OnClick(object sender, EventArgs e)
	{
        string strIdSistema = (string)Session["idsistema"];
        string strIdSectorizacion = string.Empty;
        bool bVisualizandoActAux = bVisualizandoActiva;

        if (!bVisualizandoActAux)
            strIdSectorizacion = ListBox1.SelectedValue;
        else
            strIdSectorizacion = IdSectorizacionActiva;

        CargarSectorizacion(strIdSistema, strIdSectorizacion, bVisualizandoActAux);

        if (bVisualizandoActAux)
        {
            CambiaAspectoTablaUcs(true);
        }

        ListBox1.Visible = BtNuevo.Visible = BtEliminar.Visible = BtActivar.Visible = true;
		BtNuevo.Visible = PermisoSegunPerfil;
        //BtEliminar.Enabled = PermisoSegunPerfil && ListBox1.Items.Count > 0;
        
        //MVO: el botón eliminar será visible para la sectorización no activa
        BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0 && ListBox1.SelectedValue != LIdSectorizacion.Text;

        BtActivar.Enabled = PermisoSegunPerfil && ListBox1.Items.Count > 0 && !bVisualizandoActAux;
        BtActivar.Enabled &= (!bServicioSactaAct || (bServicioSactaAct && (string.Compare(ListBox1.SelectedValue, ID_SECTORIZACION_SACTA) == 0)));

		PanelActiva.Visible = true;
		ModalPopupExtender1.Hide();		//PnlParametrosSector.Visible = false;
		IBParametros.Visible = IBPermisosRedes.Visible = IBPropiedadesGenerales.Visible = IBUtilidades.Visible = IBNivelesIntrusion.Visible = false;


		BtnPaginaMenos.Visible = false;
		BtnPaginaMas.Visible = false;
		LblSector.Visible = false;

		TEnlacesRadio.Visible = TEnlacesLC.Visible = TEnlacesInternos.Visible = false;
		LBPanelLC.Visible = LBPanelRadio.Visible = LBPanelTel.Visible = false;

		BtnVolverASectorizacion.Visible = BtnPaginaMenos.Visible = BtnPaginaMas.Visible = false;
		LBPanelTel.Visible = LBPanelRadio.Visible = LBPanelLC.Visible = false;
		LblPagina.Visible = LblPanel.Visible = LblPanel.Visible = false;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnPaginaMenos_Click(object sender, ImageClickEventArgs e)
	{
		NumPagActual = (NumPagActual == 1) ? 1 : NumPagActual - 1;
		LblPagina.Text = GetLocalResourceObject("LblPaginaResource1.Text").ToString() + " " + NumPagActual;
		LimpiarPanel();

		if (TEnlacesRadio.Visible)
			MostrarRadio();
		else
		{
			MostrarExternos();
			MostrarInternos();
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnPaginaMas_Click(object sender, ImageClickEventArgs e)
	{
		if (TEnlacesRadio.Visible)
			NumPagActual = (NumPagActual == ParametrosSector.NumPagFrec) ? (int)ParametrosSector.NumPagFrec : NumPagActual + 1;
		else
			NumPagActual = (NumPagActual == ParametrosSector.NumPagDestinosInt) ? (int)ParametrosSector.NumPagDestinosInt : NumPagActual + 1;

		LblPagina.Text = GetLocalResourceObject("LblPaginaResource1.Text").ToString() + " " + NumPagActual;
		LimpiarPanel();

		if (TEnlacesRadio.Visible)
			MostrarRadio();
		else
		{
			MostrarExternos();
			MostrarInternos();
		}
	}

    /// <summary>
    /// Respuesta a la regeneración de las sectorizaciones
    /// </summary>
    /// <param name="result"></param>
	private void OnCallBackCompleted(IAsyncResult result)
	{
		try
		{
			serviceServiciosCD40.EndGenerarSectorizacion(result);
			Application["Sectorizando"] = false;
			//Session.Add("Sectorizando", false);

            if (bSactaActivoEnCnf)
                bServicioSactaAct=EstaServicioSactaActivo();

            BtActivar.Enabled = PermisoSegunPerfil && (!bServicioSactaAct || (bServicioSactaAct && string.Compare(ListBox1.SelectedValue, ID_SECTORIZACION_SACTA) == 0));

		}
		catch (System.Web.Services.Protocols.SoapException soapException)
		{
            logDebugView.Error("(Sectorizaciones-OnCallBackCompleted): ", soapException);
		}
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "hideNloader", "hideNloader();", true);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BVerSectorizacionActiva_Click(object sender, EventArgs e)
	{
        string strIdSistema = (string)Session["idsistema"];

		ServiciosCD40.Sectorizaciones s = new ServiciosCD40.Sectorizaciones();
		s.IdSistema = (string)Session["idsistema"];
		s.IdSectorizacion = LIdSectorizacion.Text;

		DataSet dsSectorizacion = serviceServiciosCD40.DataSetSelectSQL(s);
        if (dsSectorizacion != null && dsSectorizacion.Tables.Count > 0 && dsSectorizacion.Tables[0].Rows.Count > 0)
        {
			Session["elemento"] = ((DateTime)dsSectorizacion.Tables[0].Rows[0]["FechaActivacion"]).ToString("dd/MM/yyyy HH:mm:ss");
			IdSectorizacionActiva = ((DateTime)dsSectorizacion.Tables[0].Rows[0]["FechaActivacion"]).ToString("dd/MM/yyyy HH:mm:ss");

            string strIdSectorizacion=(string)dsSectorizacion.Tables[0].Rows[0]["IdSectorizacion"];

            ListBox1.SelectedIndex = -1;
            BtActivar.Enabled = false;

            //Si la sectorizacion no es sacta
            if (string.Compare(strIdSectorizacion, ID_SECTORIZACION_SACTA) == 0)
            {
                if (bServicioSactaAct && ListBox1.Items.FindByText(strIdSectorizacion) == null)
                {
                    //Si el servicio SACTA está activo y la sectorización SACTA no está en la lista,
                    //se inserta al principio de la lista
                    ListBox1.Items.Insert(0, ID_SECTORIZACION_SACTA);
                }
            }

            CargarSectorizacion(strIdSistema, IdSectorizacionActiva, true);
            CambiaAspectoTablaUcs(true);
        }

        //MVO: al visualizar la sectorización activa, se hace no visible el botón eliminar
        BtEliminar.Visible = false;
    }

    /// <summary>
    /// 
    /// </summary>
	private void ActualizaActiva()
	{
        //if (VisualizandoActiva)
        //{
            string strIdSistema = (string)Session["idsistema"];

            if (!string.IsNullOrEmpty(strIdSistema))
            {
                ServiciosCD40.Sectorizaciones s = new ServiciosCD40.Sectorizaciones();
                s.IdSistema = strIdSistema;
                s.Activa = true;

                DataSet dsSectorizacion = serviceServiciosCD40.DataSetSelectSQL(s);
                if (dsSectorizacion != null && dsSectorizacion.Tables.Count > 0 && dsSectorizacion.Tables[0].Rows.Count > 0)
                {
                    string strNombreSectorizacionActNew = (string)dsSectorizacion.Tables[0].Rows[0]["IdSectorizacion"];
                    string stridSectorizacionConFechaActiva = ((DateTime)dsSectorizacion.Tables[0].Rows[0]["FechaActivacion"]).ToString("dd/MM/yyyy HH:mm:ss");
                    bool bActualizarActiva = false;

                    string strFechaSectorizacionAct=string.Format("{0} {1}", LFechaSectorizacion.Text,LHoraSectorizacion.Text);

                    if (string.Compare(strNombreSectorizacionActNew, LIdSectorizacion.Text) == 0 && string.Compare(stridSectorizacionConFechaActiva, strFechaSectorizacionAct) == 0)
                        bActualizarActiva = false;
                    else
                        bActualizarActiva = true;

                    if (bActualizarActiva)
                    {
                        LFechaSectorizacion.Text = ((DateTime)dsSectorizacion.Tables[0].Rows[0]["FechaActivacion"]).ToString("dd/MM/yyyy");
                        LHoraSectorizacion.Text = ((DateTime)dsSectorizacion.Tables[0].Rows[0]["FechaActivacion"]).ToString("HH:mm:ss");
                        LIdSectorizacion.Text = (string)dsSectorizacion.Tables[0].Rows[0]["IdSectorizacion"];

                        if (VisualizandoActiva)
                        {
                            CargarSectorizacion(strIdSistema, stridSectorizacionConFechaActiva, true);
                            CambiaAspectoTablaUcs(true);
                        }
                        else if (bServicioSactaAct && string.Compare(strNombreSectorizacionActNew, ID_SECTORIZACION_SACTA) == 0)
                        {
                            if (string.Compare(strNombreSectorizacionActNew, ListBox1.SelectedValue) == 0)
                            {
                                //Si se está visualizando la sectorización SACTA y se recibe una modificación de SACTA hay que actualizarla para que se visualicen los cambios.
                                CargarSectorizacion(strIdSistema, ID_SECTORIZACION_SACTA, false);
                            }

                            if (ListBox1.Items.Count > 0 && ListBox1.Items.FindByText(ID_SECTORIZACION_SACTA) == null)
                            {
                                //Si no se está visualizando la sectorización Activa, el servicio SACTA está activo y 
                                //la sectorización SACTA no está en la lista, se inserta al principio de la lista
                                ListBox1.Items.Insert(0, ID_SECTORIZACION_SACTA);
                            }
                        }
                    }
                    else
                    {
                        if (BtActivar.Visible && BtActivar.Enabled && !bServicioSactaAct && string.Compare(LIdSectorizacion.Text, ID_SECTORIZACION_SACTA) == 0)
                        {
                            if (VisualizandoActiva)
                                BtActivar.Enabled = false;

                            if (ListBox1.Items.Count > 0 && ListBox1.Items.FindByText(ID_SECTORIZACION_SACTA) != null)
                            {
                                //Si el servicio SACTA no está activo y en la lista está la sectorización SACTA se elimina.
                                ListBox1.Items.Remove(ListBox1.Items.FindByText(ID_SECTORIZACION_SACTA));
                            }

                        }
                        else if (bServicioSactaAct && ListBox1.Items.FindByText(ID_SECTORIZACION_SACTA) == null)
                        {
                            //Si el servicio SACTA está activo y no está en la lista, se inserta al principio de la lista
                            ListBox1.Items.Insert(0,ID_SECTORIZACION_SACTA);
                        }
                    }
                }
            }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="activa"></param>
	private void CambiaAspectoTablaUcs(bool activa)
	{
		VisualizandoActiva = activa;

        bVisualizandoActiva= VisualizandoActiva;

		for (int i = 0; i < TUCS.Rows.Count; i++)
		{
			switch (i % 4)
			{
				case 0:	// Texto Ucs
					for (int j = 0; j < TUCS.Rows[i].Cells.Count; j++)
					{
						ImageButton ibut = (ImageButton)TUCS.FindControl("IButton" + ((j + 1) + ((i / 4) * TUCS.Rows[i].Cells.Count)));
						if (ibut != null)
							ibut.ImageUrl = activa ? "~/Configuracion/Images/TextoUCSActiva.jpg" : "~/Configuracion/Images/TextoUCS.jpg";
					}
					break;
				case 1:
					for (int j = 0; j < TUCS.Rows[i].Cells.Count; j++)
					{
						ImageButton ibut = (ImageButton)TUCS.FindControl("ImageButton" + ((j + 1) + ((i / 4) * TUCS.Rows[i].Cells.Count)));
						if (ibut != null)
							ibut.ImageUrl = activa ? "~/Configuracion/Images/UCSActiva.jpg" : "~/Configuracion/Images/UCS.jpg";
					}
					break;
				case 2:
					TUCS.Rows[i].BackColor = activa ? System.Drawing.Color.Green : System.Drawing.Color.Coral;
					for (int j = 0; j < TUCS.Rows[i].Cells.Count; j++)
					{
						ListBox lb = (ListBox)TUCS.FindControl("BUsuarios" + ((j + 1) + ((i / 4) * TUCS.Rows[i].Cells.Count)));
                        if (lb != null)
                        {
                            if (activa)
                                lb.CssClass = "listboxActive";
                            else 
                                lb.CssClass = "listbox";

                            lb.BackColor = activa ? System.Drawing.Color.Green : System.Drawing.Color.Coral;
                        }
					}
					break;
				default:
					break;
			}
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void GVAbonados_OnSelectedIndexChanging(object sender, GridViewPageEventArgs e)
	{
		GVAbonados.PageIndex = e.NewPageIndex;
		GVAbonados.DataSource = DSNumerosAbonados;
		GVAbonados.DataBind();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void OnButtonImageMenu_Click(object sender, ImageClickEventArgs e)
	{
		ImageButton ibSelected = (ImageButton)sender;

		switch (ibSelected.ID)
		{
			case "IBPropiedadesGenerales":
				IBPropiedadesGenerales.ImageUrl = GetLocalResourceObject("IBPropiedadesGeneralesResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuTifxPropGeneralesSelected.JPG";
				IBUtilidades.ImageUrl = GetLocalResourceObject("IBUtilidadesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorUtilidadesUnSelected.JPG";
				IBParametros.ImageUrl = GetLocalResourceObject("IBParametrosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorParametrosUnSelected.JPG";
				IBNivelesIntrusion.ImageUrl = GetLocalResourceObject("IBNivelesIntrusionResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorNivelesIntrusionUnSelected.JPG";
				IBPermisosRedes.ImageUrl = GetLocalResourceObject("IBPermisosRedesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorPermisosRedesUnSelected.JPG";
				MultiView1.ActiveViewIndex = 0;
				PnlParametrosSector.Width = 610;
				break;
			case "IBUtilidades":
				IBPropiedadesGenerales.ImageUrl = GetLocalResourceObject("IBPropiedadesGeneralesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuTifxPropGeneralesSelected.JPG";
				IBUtilidades.ImageUrl = GetLocalResourceObject("IBUtilidadesResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuSectorUtilidadesUnSelected.JPG";
				IBParametros.ImageUrl = GetLocalResourceObject("IBParametrosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorParametrosUnSelected.JPG";
				IBNivelesIntrusion.ImageUrl = GetLocalResourceObject("IBNivelesIntrusionResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorNivelesIntrusionUnSelected.JPG";
				IBPermisosRedes.ImageUrl = GetLocalResourceObject("IBPermisosRedesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorPermisosRedesUnSelected.JPG";
				MultiView1.ActiveViewIndex = 1;
				PnlParametrosSector.Width = 760;
				break;
			case "IBParametros":
				IBPropiedadesGenerales.ImageUrl = GetLocalResourceObject("IBPropiedadesGeneralesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuTifxPropGeneralesSelected.JPG";
				IBUtilidades.ImageUrl = GetLocalResourceObject("IBUtilidadesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorUtilidadesUnSelected.JPG";
				IBParametros.ImageUrl = GetLocalResourceObject("IBParametrosResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuSectorParametrosUnSelected.JPG";
				IBNivelesIntrusion.ImageUrl = GetLocalResourceObject("IBNivelesIntrusionResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorNivelesIntrusionUnSelected.JPG";
				IBPermisosRedes.ImageUrl = GetLocalResourceObject("IBPermisosRedesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorPermisosRedesUnSelected.JPG";
				MultiView1.ActiveViewIndex = 2;
				PnlParametrosSector.Width = 760;
				break;
			case "IBNivelesIntrusion":
				IBPropiedadesGenerales.ImageUrl = GetLocalResourceObject("IBPropiedadesGeneralesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuTifxPropGeneralesSelected.JPG";
				IBUtilidades.ImageUrl = GetLocalResourceObject("IBUtilidadesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorUtilidadesUnSelected.JPG";
				IBParametros.ImageUrl = GetLocalResourceObject("IBParametrosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorParametrosUnSelected.JPG";
				IBNivelesIntrusion.ImageUrl = GetLocalResourceObject("IBNivelesIntrusionResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuSectorNivelesIntrusionUnSelected.JPG";
				IBPermisosRedes.ImageUrl = GetLocalResourceObject("IBPermisosRedesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorPermisosRedesUnSelected.JPG";
				MultiView1.ActiveViewIndex = 3;
				PnlParametrosSector.Width = 760;
				break;
			case "IBPermisosRedes":
				IBPropiedadesGenerales.ImageUrl = GetLocalResourceObject("IBPropiedadesGeneralesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuTifxPropGeneralesSelected.JPG";
				IBUtilidades.ImageUrl = GetLocalResourceObject("IBUtilidadesResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorUtilidadesUnSelected.JPG";
				IBParametros.ImageUrl = GetLocalResourceObject("IBParametrosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorParametrosUnSelected.JPG";
				IBNivelesIntrusion.ImageUrl = GetLocalResourceObject("IBNivelesIntrusionResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSectorNivelesIntrusionUnSelected.JPG";
				IBPermisosRedes.ImageUrl = GetLocalResourceObject("IBPermisosRedesResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuSectorPermisosRedesUnSelected.JPG";
				MultiView1.ActiveViewIndex = 4;
				PnlParametrosSector.Width = 760;
				break;
		}

		//ModalPopupExtender1.Show();
	}

    private bool EstaServicioSactaActivo()
    {
        bool bActivo=false;
        try
        {
            if ((0x0F & serviceServiciosCD40.GetEstadoSacta()) == 0)
            {
                bActivo = false;
            }
            else
            {
                bActivo = true;
            }
        }
        catch(Exception Ex)
        {
            logDebugView.Error("(Sectorizaciones-EstaServicioSactaActivo): Error al comprobar si el servicio Safta está activo. Error: "+ Ex.Message.ToString());
        }

        return bActivo;
    }

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
            bConSacta=true;
        }

        return bConSacta;
    }

    private void VisualizarPanelTop(bool bSectSACTA,string strTop)
    {
        PanelTop.Visible = true;
        BtnLiberarTop.Enabled = true;

        if (GetLocalResourceObject("PanelTop_liberarTop") != null)
            BtnLiberarTop.ToolTip = string.Format(GetLocalResourceObject("PanelTop_liberarTop").ToString(), strTop);
        else
            BtnLiberarTop.ToolTip = string.Format("Liberar el top {0} ", strTop);

        if (bSectSACTA)
        {
            BtnIntercambiarTop.Enabled = false;
            BtnIntercambiarTop.ToolTip = string.Empty;
        }
        else
        {
            BtnIntercambiarTop.Enabled = true;

            if (GetLocalResourceObject("PanelTop_InterCambiarTop") != null)
                BtnIntercambiarTop.ToolTip = string.Format(GetLocalResourceObject("PanelTop_InterCambiarTop").ToString(), strTop);
            else
                BtnIntercambiarTop.ToolTip = string.Format("Intercambiar el top {0} ", strTop);
            
        }
    }

    private void ObtenerTooltip_PanelUCS_Ver(string strNombreSector)
    {
        if (GetLocalResourceObject("PanelUCS_Ver") != null)
            BtExplorar.ToolTip = string.Format(GetLocalResourceObject("PanelUCS_Ver").ToString(), strNombreSector);
        else
            BtExplorar.ToolTip = string.Format("Ver panel del sector: {0} ", strNombreSector);
    }

    private void ObtenerTooltip_PanelUCS_Liberar(string strNombreSector)
    {
        if (GetLocalResourceObject("PanelUCS_Liberar") != null)
            BtLiberar.ToolTip = string.Format(GetLocalResourceObject("PanelUCS_Liberar").ToString(), strNombreSector);
        else
            BtLiberar.ToolTip = string.Format("Liberar en el sector: {0} ", strNombreSector);
    }

    private void ObtenerTooltip_PanelUCS_Asignar(string strNombre)
    {
        if (GetLocalResourceObject("PanelUCS_Asignar") != null)
            BtAsignar.ToolTip = string.Format(GetLocalResourceObject("PanelUCS_Asignar").ToString(), strNombre);
        else
            BtAsignar.ToolTip = string.Format("Asignar al puesto de operador {0} ", strNombre);
    }

	//#region ICallbackEventHandler Members
	//public new string GetCallbackResult()
	//{
	//    string str = "";

	//    if (Application["Sectorizando"] != null)
	//        str = ((bool)Application["Sectorizando"]).ToString();

	//    str = "False";

	//    return string.Format("{0};{1}", base.GetCallbackResult(), str);

	//}

	//public new void RaiseCallbackEvent(string eventArgument)
	//{
	//    //base.RaiseCallbackEvent(eventArgument);

	//    _Command = eventArgument;
	//}
	//#endregion
}
