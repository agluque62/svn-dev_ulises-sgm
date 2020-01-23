using System;
using System.Configuration;
using System.Data;
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
using SincronizaCD30;

// Clase para ordenar un ListBox
internal class myListItemComparer : IComparer
{
	int IComparer.Compare(object x, object y)
	{
		ListItem item_1 = x as ListItem;
		ListItem item_2 = y as ListItem;

		return item_1.Value.CompareTo(item_2.Value);
	}
}

public partial class Agrupacion : PageBaseCD40.PageCD40	//System.Web.UI.Page
{
    //private Mensajes.msgBox cMsg;
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
	static bool PermisoSegunPerfil;

    //Almacenan la lista de sectores original, antes de realizar una modificación:
    //  - ListaSectoresOriginal: lista de sectores donde el nombre de cada sector va entre comillas simples. Por ejemplo: 'GND','VIL'
    //  - ListaOriginal: lista de sectores separadas por comas. Por ejemplo: GND,VIL
    private static System.Text.StringBuilder ListaSectoresOriginal = new System.Text.StringBuilder();
    private static System.Text.StringBuilder ListaOriginal = new System.Text.StringBuilder();

	private static ServiciosCD40.ServiciosCD40 servicioParaSectorizacion = new ServiciosCD40.ServiciosCD40();
	private AsyncCallback CallbackCompletado;

	private static string IdNucleo;

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
				Response.Redirect("~/Configuracion/Inicio.aspx?Permiso=NO",false);
				return;
			}

			// BtEliminar.Visible = perfil == "3";
			PermisoSegunPerfil = (perfil == "3");
			//BtModificar.Visible = (Session["Wizard"] != null && (bool)Session["Wizard"]) && PermisoSegunPerfil;
			//BtEliminar.Visible = (Session["Wizard"] != null && (bool)Session["Wizard"]);
		}

		if (CallbackCompletado == null)
			CallbackCompletado = new AsyncCallback(OnCallBackCompleted);
		if (servicioParaSectorizacion == null)
			servicioParaSectorizacion = new ServiciosCD40.ServiciosCD40();

		// servicioParaSectorizacion.NoTransaction();		// Las actuaciones sobre la base de datos se ejecutarán al margen de TransactionTimeOut.

        if (!IsPostBack)
        {
            //logDebugView.Debug("Entrando en Agrupacion....");

            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            IndexListBox1 = -1;

            MuestraDatos(DameDatos());

            ActualizaWebPadre(true);
            CargarInforme();
        }
        else
        {
            //Si se ha recargado la página, las variables datos y la variable de session tienen valor nulo es porque 
            // si ha cambiado la sesión del servidor, bien por conmutación o reinicio
            //por lo que se va a la página de login
            if (datos == null || Session["idsistema"]==null)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "redirect", "<Script language = 'Javascript'> window.parent.location='../Login.aspx' ; </Script>", false);
            }
        }
        //else
        //    {
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
            //}
    }

	protected void CargarInforme()
	{
		LBImprimir.Attributes.Remove("onclick");
		string comando = "AbreVentana('../Informes/Report.aspx?Report=Agrupaciones.rpt');return false;";
		LBImprimir.Attributes.Add("onclick", comando);
	}

	protected void BtNuevo_Click(object sender, EventArgs e)
    {
		ListSectores.Items.Clear();
		TxtIdAgrupacion.Text = string.Empty;

        if (ListaSectoresOriginal.Length > 0)
            ListaSectoresOriginal.Clear();
        if (ListaOriginal.Length > 0)
            ListaOriginal.Clear();

		ModoEdicion(true);
		CargarSectoresSinAsignar(true);

        IndexListBox1 = ListBox1.SelectedIndex;
	}

    private void CargarSectoresSinAsignar(bool todos)
    {
        try
        {
            ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.Sectores t = new ServiciosCD40.Sectores();
            t.IdSistema = (string)Session["idsistema"];
            t.SectorSimple = true;

            DataSet d;
            if (todos)
                d = g.GetSectoresFueraDeAgrupacion((string)Session["idsistema"], null);
            else
                d = g.GetSectoresFueraDeAgrupacion((string)Session["idsistema"], TxtIdAgrupacion.Text);

            ListSectoresLibres.DataSource = d;
            ListSectoresLibres.DataTextField = "IdSector";
            ListSectoresLibres.DataBind();
        }
        catch (Exception e)
        {
            logDebugView.Error("(Agrupacion-CargarSectoresSinAsignar):", e);
        }
    }

    private void CargarSectores()
    {
        try
        {
            ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.SectoresAgrupacion t = new ServiciosCD40.SectoresAgrupacion();
            t.IdSistema = (string)Session["idsistema"];
            t.IdAgrupacion = TxtIdAgrupacion.Text;
            ServiciosCD40.Tablas[] d = g.ListSelectSQL(t);
            if (d != null && d.Length>0)
			{
                if (ListaSectoresOriginal.Length > 0)
                    ListaSectoresOriginal.Remove(0, ListaSectoresOriginal.Length);
                if (ListaOriginal.Length > 0)
                    ListaOriginal.Remove(0, ListaOriginal.Length);
 
                for (int i = 0; i < d.Length; i++)
                {
                    ListaSectoresOriginal.AppendFormat("'{0}',", ((ServiciosCD40.SectoresAgrupacion)d[i]).IdSector);
                    ListaOriginal.AppendFormat("{0},", ((ServiciosCD40.SectoresAgrupacion)d[i]).IdSector);
                    ListSectores.Items.Add(((ServiciosCD40.SectoresAgrupacion)d[i]).IdSector);
                }

                if (ListaSectoresOriginal.Length > 0)
                    ListaSectoresOriginal.Remove(ListaSectoresOriginal.Length - 1, 1);
                if (ListaOriginal.Length > 0)
                    ListaOriginal.Remove(ListaOriginal.Length - 1, 1);

				IdNucleo=((ServiciosCD40.SectoresAgrupacion)d[0]).IdNucleo;
			}
        }
        catch (Exception e)
        {
            logDebugView.Error("(Agrupacion-CargarSectores):", e);
        }
    }

    private void MostrarElemento()
    {
        ListSectores.Items.Clear();
        for (int i = 0; i < datos.Length; i++)
            if (String.Compare((((ServiciosCD40.Agrupaciones)datos[i]).IdAgrupacion), ListBox1.SelectedValue) == 0)
            {
                BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarAgrupacion"), ListBox1.SelectedValue);

                TxtIdAgrupacion.Text = ((ServiciosCD40.Agrupaciones)datos[i]).IdAgrupacion;
                CargarSectores();
                return;
            }
    }

    private void ModoEdicion(bool edicion)
    {
		RequiredFieldIdentificador.Visible = edicion;
		ListBox1.Enabled = !edicion;
        /**
         * AGL ID.83/84. Al entrar en modo 'edicion' si solo hay una agrupacion, la lista aparece vacía.
         * Quito el selector de la lista.
         * */
        //if (edicion)
         //   ListBox1.SelectedIndex = -1;
        /**
         * Fin de la Modificacion */

		TxtIdAgrupacion.ReadOnly = !edicion;
		TxtIdAgrupacion.Enabled = edicion;
		ListSectores.Enabled = edicion;
		IButAsignar.Visible = IButQuitar.Visible = edicion;
		ListSectoresLibres.Visible = edicion;
		BtAceptar.Visible = BtCancelar.Visible = edicion;
		BtNuevo.Visible = !edicion && PermisoSegunPerfil;
		BtModificar.Visible = BtEliminar.Visible = ListBox1.Items.Count > 0 && !edicion && PermisoSegunPerfil;
    }

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            ListaSectoresOriginal.Remove(0, ListaSectoresOriginal.Length);
            ListaOriginal.Remove(0, ListaOriginal.Length);
            BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil;
            MostrarElemento();
        }
    }

    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            // ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
            ServiciosCD40.Agrupaciones t = new ServiciosCD40.Agrupaciones();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;

            ServiciosCD40.Tablas[] d = servicioParaSectorizacion.ListSelectSQL(t);
            datos = d;
            return d;    
        }
        catch (Exception e)
        {
            logDebugView.Error("(Agrupacion-DameDatos):", e);
        }
        return null;
    }

    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
        ListBox1.Items.Clear();

        if (nu!=null)
            for (int i = 0; i < nu.Length; i++)
                ListBox1.Items.Add(((ServiciosCD40.Agrupaciones)nu[i]).IdAgrupacion);

		if (ListBox1.Items.Count > 0)
		{
			ActualizaWebPadre(true);

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
            }
            else
                ListBox1.SelectedIndex = -1;


			MostrarElemento();
			GeneraXmlParaInforme();
		}
		else
		{
			TxtIdAgrupacion.Text = string.Empty;
            /**
             * AGL ID.83/84. Al borrar un item y no quedar mas ITEMS, no se limpia la lista de sectores asignados
             * */
            ListSectores.Items.Clear();
            /**
             * Fin de la Modificacion */
		}

		ModoEdicion(false);
	}

	private void GeneraXmlParaInforme()
	{
		ServiciosCD40.SectoresAgrupacion t = new ServiciosCD40.SectoresAgrupacion();
		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement a = config.AppSettings.Settings["Sistema"];
		t.IdSistema = a.Value;

		ServiciosCD40.ServiciosCD40 s = new ServiciosCD40.ServiciosCD40();
		s.DataSetSelectSQL(t).WriteXml(Server.MapPath("~/Informes/Agrupaciones.xml"));
	}

	protected override void AceptarCambios()
	{
		//GuardarCambios();
		base.AceptarCambios();
	}

    protected override void CancelarCambios()
    {
        /**
         * ID.83/84. Al cancelar la edición no se muestran bien los datos.
         * */
        // Comentado por la Modificacion.
        // ModoEdicion(false);
        // Añadido por la Modificacion.

        NewItem = string.Empty;

        MuestraDatos(DameDatos());
        /**
         * Fin de la Modificacion */
	}


    private void GuardarCambios()
    {
		uint idSactaDominante = 0;

        try
        {
            ServiciosCD40.Sectores s = new ServiciosCD40.Sectores();
			System.Text.StringBuilder lista = new System.Text.StringBuilder();          //Lista Sectores entre comillas simples y separados por comas
			System.Text.StringBuilder listaSectores = new System.Text.StringBuilder();  //Lista de Sectores separados por comas
			string strNucleo = "";
			
            //Se ordena la lista de sectores
            OrdenaLista();

            //Se componen la lista de sectores entre comillas simples
            for (int i = 0; i < ListSectores.Items.Count; i++)
            {
                listaSectores.AppendFormat("'{0}',", ListSectores.Items[i].Text);
                lista.AppendFormat("{0},", ListSectores.Items[i].Text);
            }

            //Se elimina la última coma de las listas
            if (lista.Length > 0)
                lista = lista.Remove(lista.Length - 1, 1);

            if (listaSectores.Length > 0)
                listaSectores = listaSectores.Remove(listaSectores.Length - 1, 1);

            //Se comprueba si en el sistema existe otra agrupación con el mismo nombre o con el mismo conjunto de sectores
            if (bExisteAgrupacionConSectores(TxtIdAgrupacion.Enabled, listaSectores.ToString()))
                return;
            else
            {
                ServiciosCD40.Agrupaciones n = new ServiciosCD40.Agrupaciones();

                n.IdSistema = (string)Session["idsistema"];
                
                n.IdAgrupacion = TxtIdAgrupacion.Text;

                if (TxtIdAgrupacion.Enabled) //Nueva Agrupacion
                {
                    if (servicioParaSectorizacion.InsertSQL(n) < 0)
                    {
                        logDebugView.Warn("(Agrupacion-GuardarCambios): No se han podido insertar los datos (InsertSQL).");
                        cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorGuardarAgrupacion"), n.IdAgrupacion));
                        return;
                    }
                }
                else
                    if (servicioParaSectorizacion.UpdateSQL(n) < 0)
                    {
                        logDebugView.Warn("(Agrupacion-GuardarCambios): No se han podido actualizar los datos (UpdateSQL).");
                        cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorGuardarAgrupacion"), n.IdAgrupacion));
                    }

                ServiciosCD40.SectoresAgrupacion r = new ServiciosCD40.SectoresAgrupacion();
                r.IdSistema = (string)Session["idsistema"];
                r.IdAgrupacion = TxtIdAgrupacion.Text;

                NewItem = r.IdAgrupacion;

                servicioParaSectorizacion.DeleteSQL(r);

                List<string> lossectores = new List<string>();

                for (int i = 0; i < ListSectores.Items.Count; i++)
                {
                    ServiciosCD40.SectoresAgrupacion r1 = new ServiciosCD40.SectoresAgrupacion();
                    r1.IdSistema = (string)Session["idsistema"];
                    r1.IdAgrupacion = TxtIdAgrupacion.Text;
                    r1.IdSector = ListSectores.Items[i].Text;
                    lossectores.Add(r1.IdSector);
                    ServiciosCD40.Sectores s1 = new ServiciosCD40.Sectores();
                    s1.IdSistema = (string)Session["idsistema"];
                    s1.IdSector = ListSectores.Items[i].Text;
                    ServiciosCD40.Tablas[] tsector = servicioParaSectorizacion.ListSelectSQL(s1);
                    if (tsector != null)
                    {
                        for (int j = 0; j < tsector.Length; j++)
                            strNucleo = r1.IdNucleo = ((ServiciosCD40.Sectores)tsector[j]).IdNucleo;
                        servicioParaSectorizacion.InsertSQL(r1);
                    }

                    if (i == 0)
                        idSactaDominante = ((ServiciosCD40.Sectores)tsector[0]).NumSacta;
                }

                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                {
                    SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                    switch (sincro.AltaAgrupacion(TxtIdAgrupacion.Text, lossectores.ToArray()))
                    {
                        case 110:
                            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod110"), TxtIdAgrupacion.Text));
                            break;
                        case 111:
                            cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "Cod111"), TxtIdAgrupacion.Text));
                            break;
                        default:
                            break;
                    }
                }

                // Comprobar si la combinación de los sectores que componen la agrupación existe como sector
                ServiciosCD40.Sectores sct = new ServiciosCD40.Sectores();
                sct.IdSistema = (string)Session["idsistema"];
                sct.IdNucleo = strNucleo;
                sct.IdSector = servicioParaSectorizacion.GeneraAlgoritmo(sct.IdSistema, strNucleo, listaSectores.ToString());
                if (ListSectores.Items.Count > 0)
                {
                    //Si se está realizando una modificación, hay actualizar la anterior agrupación: SectoresOriginal y ListaOriginal contienen la lista de sectores antes de realizar 
                    // alguna modificación. Si se está dando de alta una nueva agrupacion, la lista estará vacía.
                    if (ListaSectoresOriginal.Length > 0 && ListaOriginal.Length > 0)
                    {
                        // Nombre de los sectores que componían la agrupación antes de ser modificada
                        string nomSectorOriginal = servicioParaSectorizacion.GeneraAlgoritmo(sct.IdSistema, strNucleo, ListaSectoresOriginal.ToString());
                        // Generar el sector con el nombre de la agrupación
                        GeneraSector(nomSectorOriginal, ListaSectoresOriginal, strNucleo, idSactaDominante);
                        // Modificar las sectorizaciones que componían los sectores originales (agrupación) con el del algoritmo
                        ActualizaSectoresSectorizacion(nomSectorOriginal, TxtIdAgrupacion.Text, strNucleo);
                        // Generar los SectoresSector que componen la agrupación...
                        GeneraSectoresSector(nomSectorOriginal, ListaOriginal.ToString(), strNucleo);
                        // Actualiza los Parámetros y los números de abonado de los sectores que componen una agrupación
                        //servicioParaSectorizacion.RegeneraParametrosAgrupacion(r.IdSistema, strNucleo, nomSectorOriginal);
                    }

                    // Generar el sector con el nombre de la agrupación
                    GeneraSector(TxtIdAgrupacion.Text, listaSectores, strNucleo, idSactaDominante);
                    // Generar los SectoresSector que componen la agrupación...
                    GeneraSectoresSector(TxtIdAgrupacion.Text, lista.ToString(), strNucleo);
                    // Cambia los identificadores de los sectores asignado a tops por el de la agrupación recién creada
                    ActualizaSectoresSectorizacion(TxtIdAgrupacion.Text, sct.IdSector, strNucleo);
                    // Actualiza los Parámetros y los números de abonado de los sectores que componen una agrupación
                    servicioParaSectorizacion.RegeneraParametrosAgrupacion(r.IdSistema, strNucleo, r.IdAgrupacion);

                    servicioParaSectorizacion.BeginRegeneraSectorizaciones((string)Session["idsistema"], true /* completa */, false /* radio */, false /* tf */, CallbackCompletado, null);
                }
            }

            lista.Clear();
            listaSectores.Clear();

            IndexListBox1 = ListBox1.SelectedIndex;

        }
        catch (Exception e)
        {
            logDebugView.Error("(Agrupacion-GuardarCambios):", e);
        }

        
        /**
         * AGL ID.83/84.  Al aceptar en modo edición no se muestran bien los datos.
         * */
        /* Comentado en el cambio
        ModoEdicion(false);*/
        MuestraDatos(DameDatos());
        /**
         * Fin de la Modificacion */
    }


    private bool bExisteAgrupacionConSectores(bool bNuevaAgrupacion,string strListaSectores)
    {
        bool bExiste=false;
        string idAgrupacion=String.Empty;
        
        //En el Alta, se comprueba si ya existe otra agrupación con el mismo identificador.
        //En el alta y en la modificación, se comprueba si en el sistema existe una agrupación con el conjunto de sectores configurado
        try
        {
            ServiciosCD40.Sectores s = new ServiciosCD40.Sectores();

            if (bNuevaAgrupacion) //Nueva Agrupacion
            {
                ServiciosCD40.Agrupaciones n = new ServiciosCD40.Agrupaciones();

                n.IdSistema = (string)Session["idsistema"];
                n.IdAgrupacion = TxtIdAgrupacion.Text;

                //Se comprueba que si existe otra agrupación en el sistema con el mismo nombre
                ServiciosCD40.Tablas[] d = servicioParaSectorizacion.ListSelectSQL(n);
                if (null != d && d.Length > 0)
                {
                    cMsg.alert(string.Format((string)GetGlobalResourceObject("Espaniol", "AvisoAgrupacionExiste"),TxtIdAgrupacion.Text));
                    bExiste=true ;
                }
            }
            else
            {   //Se obtiene el id de la agrupación que se está modificando
                idAgrupacion = TxtIdAgrupacion.Text;
            }

            if (!bExiste)
            {
                //Si la agrupación es nueva y no existe o se va a modificar, se comprueba si existe en el sistema
                //otra agrupación con el mismo conjunto de sectores 
       
                if (servicioParaSectorizacion.ExisteAgrupacionConSectores(strListaSectores,idAgrupacion))
                {
                    cMsg.alert(string.Format((string)GetGlobalResourceObject("Espaniol", "AvisoAgrupacionConMismosSectoresExiste"),strListaSectores));
                    bExiste = true;
                }
            }
        }
        catch (Exception objEx)
        {
            logDebugView.Error("Agrupacion (bExisteAgrupacionConSectores): error en la comprobación de la existencia de la agrupación de sectores.");
            logDebugView.Error("Error:", objEx);
        }

        return bExiste;
    }

	private void ActualizaSectoresSectorizacion(string idAgrupacion, string nombreGrupoSectores, string strNucleo)
	{
		servicioParaSectorizacion.ActualizaSectoresSectorizacion((string)Session["idsistema"], strNucleo, nombreGrupoSectores, idAgrupacion);
	}

	private void OrdenaLista()
	{
		// Ordenar la lista
		ListItem[] items = new ListItem[ListSectores.Items.Count];
		ListSectores.Items.CopyTo(items, 0);
		System.Array.Sort(items, new myListItemComparer());
		ListSectores.Items.Clear();
		ListSectores.Items.AddRange(items);
	}

	private void GeneraSectoresSector(string nomSector, string lista, string strNucleo)
	{
		//ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
		ServiciosCD40.SectoresSector s = new ServiciosCD40.SectoresSector();

		string[] sectores = lista.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

		s.IdSistema = (string)Session["IdSistema"];
		s.IdSector = nomSector;
		s.IdNucleo = strNucleo;

        /**
         * AGL ID.83/84. 
         * Antes de Insertar, los borro */
        servicioParaSectorizacion.DeleteSQL(s);

		for (int i = 0; i < sectores.Length; i++)
		{
			s.IdSectorOriginal = sectores[i];
			s.EsDominante = i == 0;

            servicioParaSectorizacion.InsertSQL(s);
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nomSector"></param>
    /// <param name="listaSectores"></param>
    /// <param name="idNucleo"></param>
    /// <param name="idSacta"></param>
	private void GeneraSector(string nomSector, System.Text.StringBuilder listaSectores, string idNucleo, uint idSacta)
	{
		try
		{
			//ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();
			ServiciosCD40.Sectores s = new ServiciosCD40.Sectores();

			s.IdSistema = (string)Session["IdSistema"];
			s.IdSector = nomSector;
			s.IdNucleo = idNucleo;

			// Comprobar que esta combinación de sectores no existe en la tabla Sectores
			// en cuyo caso no hay nada que generar
			if (servicioParaSectorizacion.ListSelectSQL(s).Length > 0)
				return;

			s.IdParejaUCS = s.IdNucleoParejaUCS = s.IdSistemaParejaUCS = null;
			s.SectorSimple = false;
			s.Tipo = "R";
			s.TipoPosicion = "C";

            s.PrioridadR2 = servicioParaSectorizacion.GetPrioridadSector(s.IdSistema, s.IdNucleo, listaSectores.ToString());

			s.TipoHMI = 0;
			s.NumSacta = idSacta;

            if (servicioParaSectorizacion.InsertSQL(s) < 0) logDebugView.Error("(Sectorizaciones-GeneraSector): No se ha podido guardar el sector.");
		}
		catch (Exception ex)
		{
			logDebugView.Error("(Agrupacion-GeneraSector): ", ex);
		}
	}

    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
        //cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "CancelarCambios"), "cancelparam");
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        //La agrupación debe tener como mínimo 2 sectores
        if (ListSectores.Items.Count <2)
            cMsg.alert((string)GetGlobalResourceObject("Espaniol", "AgrupacionNumMinSectores"));
        else 
            GuardarCambios();

    }

    protected void BtModificar_Click(object sender, EventArgs e)
    {
		ModoEdicion(true);
		TxtIdAgrupacion.Enabled = false;
		CargarSectoresSinAsignar(false);
        IndexListBox1 = ListBox1.SelectedIndex;
	}

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        if (ListBox1.SelectedValue != "")
        {
			//string texto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarAgrupacion"), ListBox1.SelectedValue);
            IndexListBox1 = ListBox1.SelectedIndex;
            Session["elemento"] = ListBox1.SelectedValue;
            EliminarElemento();
            //cMsg.confirm(texto, "eliminaelemento");
        }
    }

    private void EliminarElemento()
    {
        try
        {
            ServiciosCD40.Agrupaciones n = new ServiciosCD40.Agrupaciones();
            n.IdSistema = (string)Session["idsistema"];
            n.IdAgrupacion = ListBox1.SelectedValue;
			//ServiciosCD40.ServiciosCD40 g = new ServiciosCD40.ServiciosCD40();

			if (servicioParaSectorizacion.DeleteSQL(n) < 0)
			{
				logDebugView.Warn("(Nucleos-EliminarElemento): No se ha borrado el elemento");
				cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorEliminarAgrupacion"), n.IdAgrupacion));
			}
			else
			{
				Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
				KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
				if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
				{
					SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
					switch (sincro.BajaAgrupacion(n.IdAgrupacion))
					{
						case 112:
							string s = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado") + "\\n\\n"
								+ String.Format((string)GetGlobalResourceObject("Espaniol", "Cod112"), n.IdAgrupacion);
							cMsg.alert(s);
							break;
						case 113:
							string s1 = (string)GetGlobalResourceObject("Espaniol", "ElementoEliminado") + "\\n\\n"
								+ String.Format((string)GetGlobalResourceObject("Espaniol", "Cod113"), n.IdAgrupacion);
							cMsg.alert(s1);
							break;
						default:
							break;
					}
				}

                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));

                System.Text.StringBuilder strbListaSectores = new System.Text.StringBuilder();

                // Obtener los sectores que componen la agrupación para actualizar la lista
                if (ListSectores.Items.Count > 0)
                {
                    //Se compone la lista de sectores, con el nombre de cada sector entre comillas simples y la última coma
                    for (int i = 0; i < ListSectores.Items.Count; i++)
                    {
                        strbListaSectores.AppendFormat("'{0}',", ListSectores.Items[i].Text);
                    }
                    strbListaSectores = strbListaSectores.Remove(strbListaSectores.Length - 1, 1);

                    //La lista de sectores debe ir con cada item entre comillas simples para que la consulta no falle
                    ActualizaSectores(strbListaSectores);
                }

                if (ListaSectoresOriginal.Length > 0 && ListaOriginal.Length>0)
                {
                    // Nombre de los sectores que componían la agrupación antes de ser modificada
                    string nomSectorOriginal = servicioParaSectorizacion.GeneraAlgoritmo((string)Session["idsistema"], IdNucleo, ListaSectoresOriginal.ToString());
                    // Generar el sector con el nombre de la agrupación
                    GeneraSector(nomSectorOriginal, ListaSectoresOriginal, IdNucleo, 0);
                    // Modificar las sectorizaciones que componían los sectores originales (agrupación) con el del algoritmo
                    ActualizaSectoresSectorizacion(nomSectorOriginal, TxtIdAgrupacion.Text, IdNucleo);
                    // Generar los SectoresSector que componen la agrupación...
                    GeneraSectoresSector(nomSectorOriginal, ListaOriginal.ToString(), IdNucleo);
                    // Actualiza los Parámetros y los números de abonado de los sectores que componen una agrupación
                    //servicioParaSectorizacion.RegeneraParametrosAgrupacion((string)Session["idsistema"], IdNucleo, nomSectorOriginal);
                }

                // Se elimina de la tabla Sectores el sector correspondiente a la agrupación.
                ServiciosCD40.Sectores sc = new ServiciosCD40.Sectores();
                sc.IdSistema = (string)Session["idsistema"];
                sc.IdNucleo = IdNucleo;
                sc.IdSector = ListBox1.SelectedValue;
                servicioParaSectorizacion.DeleteSQL(sc);

                MuestraDatos(DameDatos());

				servicioParaSectorizacion.BeginRegeneraSectorizaciones((string)Session["idsistema"], true /* completa */, false /* radio */, false /* tf */, CallbackCompletado, null);

                strbListaSectores.Clear();
			}
		}
        catch (Exception e)
        {
            logDebugView.Error("(Agrupacion-EliminarElemento):", e);
        }
    }

	private void ActualizaSectores(System.Text.StringBuilder lista)
	{
		try
		{
			string nombreGrupoSectores = servicioParaSectorizacion.GeneraAlgoritmo((string)Session["idsistema"],IdNucleo, lista.ToString());

			servicioParaSectorizacion.ActualizaSectoresSectorizacion((string)Session["idsistema"], IdNucleo, ListBox1.SelectedValue, nombreGrupoSectores);
		}
		catch (Exception ex)
		{
			logDebugView.Error("(Agrupación-ActualizaSectores): ", ex);
		}
	}

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
		if (ListSectoresLibres.SelectedIndex >= 0)
		{
			for (int i = 0; i < ListSectoresLibres.Items.Count; i++)
				if (ListSectoresLibres.Items[i].Selected)
				{
					ListSectores.Items.Add(ListSectoresLibres.Items[i]);
					ListSectoresLibres.Items.Remove(ListSectoresLibres.Items[i]);
					i--;
				}
		}
    }

    protected void IButQuitar_Click(object sender, ImageClickEventArgs e)
    {
        if (ListSectores.SelectedIndex >= 0)
        {
            for (int i = 0; i < ListSectores.Items.Count;i++)
            {
                if (ListSectores.Items[i].Selected)
                {
                    ListSectoresLibres.Items.Add(ListSectores.Items[i]);
                    ListSectores.Items.Remove(ListSectores.Items[i]);
                    i--;
                }
            }
        }
    }

	// Respuesta a la regeneración de las sectorizaciones
	private void OnCallBackCompleted(IAsyncResult result)
	{
		try
		{
			int retorno = servicioParaSectorizacion.EndRegeneraSectorizaciones(result);
			Session.Add("Sectorizando", false);
		}
		catch (System.Web.Services.Protocols.SoapException soapException)
		{
			logDebugView.Error("(Agrupación-OnCallBackCompleted): ", soapException);
		}
	}
}
