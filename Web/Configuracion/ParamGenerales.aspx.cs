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
using System.Resources;
using log4net;
using log4net.Config;

public partial class ParamGenerales : PageBaseCD40.PageCD40	// System.Web.UI.Page
{
    /// <summary>
    /// 
    /// </summary>
    private static ServiciosCD40.Sistema info;
    /// <summary>
    /// 
    /// </summary>
	private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();
    /// <summary>
    /// 
    /// </summary>
	private static int UltimaPagina;
    /// <summary>
    /// 
    /// </summary>
	private static DataSet ListaTareas;
    /// <summary>
    /// 
    /// </summary>
	private static ServiciosCD40.DatosControlBackup DatosBackup;

    /// <summary>
    /// AGL. 2012.06.19 ID.114
    /// Solo accede a modificar parámetros el 'Tecnico 3'
    /// </summary>
    static bool PermisoSegunPerfil;

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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected new void Page_Load(object sender, EventArgs e)
    {
		base.Page_Load(sender, e);

		//cMsg = (Mensajes.msgBox)this.Master.FindControl("MsgBox1");

        /**
         * AGL. 2012.06.19 ID.114
         * Solo accede a modificar parámetros el 'Tecnico 3'.
         * */
        if (Context.Request.IsAuthenticated)
        {
            if (((FormsIdentity)Context.User.Identity).Ticket.Name != "*CD40*")
            {
                Response.Redirect("~/Configuracion/Inicio.aspx", false);
            }

            // retrieve user's identity from httpcontext user 
            FormsIdentity ident = (FormsIdentity)Context.User.Identity;
            string perfil = ident.Ticket.UserData;
            PermisoSegunPerfil = perfil == "3";
        }
        /**/
        
        if (!IsPostBack)
		{
			MuestraDatos(DameDatos());
			LeeTareas();
			UltimaPagina = 0;
			CargarInforme();

			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
			KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
			LblSistema.Text = s.Value;

            IBSistema.CssClass = "buttonImageSelected";
            IBHistoricos.CssClass = "buttonImage";
            IBTareas.CssClass = "buttonImage";
            //IBSistema.ImageUrl = GetLocalResourceObject("IBSistemaResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuSistemaSistemaSelected.JPG";
            //IBHistoricos.ImageUrl = GetLocalResourceObject("IBHistoricosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSistemaHistoricosUnSelected.JPG";
            //IBTareas.ImageUrl = GetLocalResourceObject("IBTareasResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSistemaTareasUnSelected.JPG";

            
            DListVersionIP.Enabled = false;
            /**/
            
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");
        }
		else
		{
			MultiView1.ActiveViewIndex = UltimaPagina;
		}
	}

	//protected void ActualizaWebPadre()
	//{
	//    string webPadreScript = "<script language='JavaScript'>" +
	//    "HabilitaBotonSiguiente()</script>";

	//    if (!ClientScript.IsClientScriptBlockRegistered("WebPadreScript"))
	//        ClientScript.RegisterStartupScript(this.GetType(), "WebPadreScript", webPadreScript, false);
	//}

    /// <summary>
    /// 
    /// </summary>
	protected void CargarInforme()
	{
		LBImprimir.Attributes.Remove("onclick");
		string comando = "AbreVentana('../Informes/Report.aspx?Report=Sistema.rpt');return false;";
		LBImprimir.Attributes.Add("onclick", comando);
	}
    /// <summary>
    /// 
    /// </summary>
    /// 
	private void LeeTareas()
	{
        /*
		ServiciosCD40.Tareas t = new ServiciosCD40.Tareas();
		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
		t.IdSistema = s.Value;
		Session["idsistema"] = t.IdSistema;

		ListaTareas = ServicioCD40.DataSetSelectSQL(t);
        if (ListaTareas.Tables.Count > 0)
        {
            GVTareas.DataSource = ListaTareas;
            GVTareas.DataBind();
        }

		ChequeaProgramadas();
         */
	}
    /// <summary>
    /// 
    /// </summary>
	private void ChequeaProgramadas()
	{
        //Ref_Estadisticas.Estadisticas servicioTareas = new Ref_Estadisticas.Estadisticas();

        //string[] listaTareasProgramadas = servicioTareas.ListaTareasProgramadas();
        //if (listaTareasProgramadas != null)
        //{
        //    foreach (GridViewRow r in GVTareas.Rows)
        //    {
        //        int indice = Array.FindIndex(listaTareasProgramadas, delegate(string obj) { return ((obj != null) && (obj.Substring(0, obj.Length - 4) == r.Cells[2].Text)); });
        //        if (indice >= 0)
        //            ((CheckBox)r.FindControl("CBProgramada")).Checked = true;
        //    }
        //}
	}
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private ServiciosCD40.Sistema DameDatos()
    {
        try
        {
            ServiciosCD40.Sistema t = new ServiciosCD40.Sistema();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = t.IdSistema;

			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);

			if (d.Length > 0)
			{
				DatosBackup = ServicioCD40.RecuperarDatosControlBackup(t.IdSistema);
				return (ServiciosCD40.Sistema)d[0];
			}
        }
        catch (Exception e)
        {
            logDebugView.Error("(ParametrosGenerales-DameDatos)",e);
        }
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    private void MuestraDatos(ServiciosCD40.Sistema s)
    {
		info = s;

        if (s != null)
        {
			ActualizaWebPadre(true);
            /**
            * AGL. 2012.06.19 ID.114
            * Solo accede a modificar parámetros el 'Tecnico 3'.
            * */
            BtModificar.Visible = PermisoSegunPerfil; 
            BtNuevo.Visible = false;

            //TxtPTT.Text = s.TiempoPtt.ToString();
            //TxtSinJack1.Text = s.TiempoSinJack1.ToString();
            //TxtSinJack2.Text = s.TiempoSinJack2.ToString();

            //TxtTamLitEnlExt.Text = s.TamLiteralEnlExt.ToString();
            //TxtTamLitDA.Text = s.TamLiteralDA.ToString();
            //TxtTamLitEmpl.Text = s.TamLiteralEmplazamiento.ToString();
            //TxtTamLitIA.Text = s.TamLiteralIA.ToString();
            TxtGrupoMulti.Text = s.GrupoMulticastConfiguracion;
            TxtPortMulti.Text = s.PuertoMulticastConfiguracion.ToString();

            TxtLlamEntIDA.Text = s.NumLlamadasEntrantesIda.ToString();
            TxtLlamIDA.Text = s.NumLlamadasEnIda.ToString();
            TxtFrecPag.Text = s.NumFrecPagina.ToString();
            TxtPagFrec.Text = s.NumPagFrec.ToString();
            TxtEnlIntPag.Text = s.NumDestinosInternosPag.ToString();
            TxtPagEnlInt.Text = s.NumPagDestinosInt.ToString();
            TxtKAP.Text = s.KeepAlivePeriod.ToString();
            TxtKAM.Text = s.KeepAliveMultiplier.ToString();
            TxtNumAI.Text = s.NumEnlacesAI.ToString();

            //DListVersionIP.SelectedValue = s.VersionIP.ToString();
            info = s;

			// Datos para backup
            if (DatosBackup.NomRecursoHistoricos != null)
            {
                TBRecursoHistoricos.Text = DatosBackup.NomRecursoHistoricos;
                TBRecursoIndicadores.Text = DatosBackup.NomRecursoIndicadores;
                RBLGestion.SelectedValue = DatosBackup.Profundidad[3].ToString();
                RBLHardware.SelectedValue = DatosBackup.Profundidad[0].ToString();
                RBLRadio.SelectedValue = DatosBackup.Profundidad[2].ToString();
                RBLTelefonia.SelectedValue = DatosBackup.Profundidad[1].ToString();
                RadioButtonList1.SelectedValue = DatosBackup.Profundidad[4].ToString();
                RadioButtonList2.SelectedValue = DatosBackup.Profundidad[5].ToString();
                RadioButtonList3.SelectedValue = DatosBackup.Profundidad[6].ToString();
                RadioButtonList4.SelectedValue = DatosBackup.Profundidad[7].ToString();
            }

			GeneraXmlParaInforme();
		}
		else
		{
			BtModificar.Visible = false;
			BtNuevo.Visible = true;
		}
    }
    /// <summary>
    /// 
    /// </summary>
	private void GeneraXmlParaInforme()
	{
		ServiciosCD40.Sistema t = new ServiciosCD40.Sistema();
		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
		t.IdSistema = s.Value;

		DataSet d = ServicioCD40.DataSetSelectSQL(t);
		d.WriteXml(Server.MapPath("~/Informes/Sistema.xml"));
	}
    /// <summary>
    /// Boton Modificar...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        TxtPTT.ReadOnly = false;
        TxtSinJack1.ReadOnly = false;
        TxtSinJack2.ReadOnly = false;

		Panel2.Enabled = true;
		Panel3.Enabled = true;

        TxtTamLitEnlExt.ReadOnly = false;
        TxtTamLitDA.ReadOnly = false;
        TxtTamLitEmpl.ReadOnly = false;
        TxtTamLitIA.ReadOnly = false;
        BtModificar.Visible = false;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        TxtPortMulti.ReadOnly = false;
        TxtGrupoMulti.ReadOnly = false;

        TxtLlamEntIDA.ReadOnly = false;
        TxtLlamIDA.ReadOnly = false;
        TxtFrecPag.ReadOnly = false;
        TxtPagFrec.ReadOnly = false;
        TxtEnlIntPag.ReadOnly = false;
        TxtPagEnlInt.ReadOnly = false;
        TxtKAP.ReadOnly = false;
        TxtKAM.ReadOnly = false;
        TxtNumAI.ReadOnly = false;

        /**
         * AGL. 2012.06.19 ID.114
         * 
         * */
        DListVersionIP.Enabled = true;
        /**/
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtNuevo_Click(object sender, EventArgs e)
	{
		TxtPTT.ReadOnly = false;
		TxtSinJack1.ReadOnly = false;
		TxtSinJack2.ReadOnly = false;

		Panel2.Enabled = true;
		Panel3.Enabled = true;

		TxtTamLitEnlExt.ReadOnly = false;
		TxtTamLitDA.ReadOnly = false;
		TxtTamLitEmpl.ReadOnly = false;
		TxtTamLitIA.ReadOnly = false;
		BtNuevo.Visible = false;
		BtAceptar.Visible = true;
		BtCancelar.Visible = true;
		TxtPortMulti.ReadOnly = false;
		TxtGrupoMulti.ReadOnly = false;

        TxtLlamEntIDA.ReadOnly = false;
        TxtLlamIDA.ReadOnly = false;
        TxtFrecPag.ReadOnly = false;
        TxtPagFrec.ReadOnly = false;
        TxtEnlIntPag.ReadOnly = false;
        TxtPagEnlInt.ReadOnly = false;
        TxtKAP.ReadOnly = false;
        TxtKAM.ReadOnly = false;
        TxtNumAI.ReadOnly = true;
    }
    /// <summary>
    /// 
    /// </summary>
    private void EstadoNormal()
    {
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
		//BtModificar.Visible = (Session["Wizard"] == null || !(bool)Session["Wizard"]);
		//BtNuevo.Visible = !BtModificar.Visible;

		Panel2.Enabled = false;
		Panel3.Enabled = false;
		
		TxtPTT.ReadOnly = true;
        TxtSinJack1.ReadOnly = true;
        TxtSinJack2.ReadOnly = true;
        TxtTamLitEnlExt.ReadOnly = true;
        TxtTamLitDA.ReadOnly = true;
        TxtTamLitEmpl.ReadOnly = true;
        TxtTamLitIA.ReadOnly = true;
        TxtPortMulti.ReadOnly = true;
        TxtGrupoMulti.ReadOnly = true;

        TxtLlamEntIDA.ReadOnly = true;
        TxtLlamIDA.ReadOnly = true;
        TxtFrecPag.ReadOnly = true;
        TxtPagFrec.ReadOnly = true;
        TxtEnlIntPag.ReadOnly = true;
        TxtPagEnlInt.ReadOnly = true;
        TxtKAP.ReadOnly = true;
        TxtKAM.ReadOnly = true;
        TxtNumAI.ReadOnly = true;

        /**
         * AGL. 2012.06.19 ID.114
         * 
         * */
        DListVersionIP.Enabled = false;
        /**/
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
		MuestraDatos(DameDatos());

		EstadoNormal();
        if (info!=null)
        {
            //TxtPTT.Text = info.TiempoPtt.ToString();
            //TxtSinJack1.Text = info.TiempoSinJack1.ToString();
            //TxtSinJack2.Text = info.TiempoSinJack2.ToString();

            //TxtTamLitEnlExt.Text = info.TamLiteralEnlExt.ToString();
            //TxtTamLitDA.Text = info.TamLiteralDA.ToString();
            //TxtTamLitEmpl.Text = info.TamLiteralEmplazamiento.ToString();
            //TxtTamLitIA.Text = info.TamLiteralIA.ToString();
            //DListVersionIP.SelectedValue = info.VersionIP.ToString();
            TxtGrupoMulti.Text = info.GrupoMulticastConfiguracion;
            TxtPortMulti.Text = info.PuertoMulticastConfiguracion.ToString();

            TxtLlamEntIDA.Text = info.NumLlamadasEntrantesIda.ToString();
            TxtLlamIDA.Text = info.NumLlamadasEnIda.ToString();
            TxtFrecPag.Text = info.NumFrecPagina.ToString();
            TxtPagFrec.Text = info.NumPagFrec.ToString();
            TxtEnlIntPag.Text = info.NumDestinosInternosPag.ToString();
            TxtPagEnlInt.Text = info.NumPagDestinosInt.ToString();
            TxtKAP.Text = info.KeepAlivePeriod.ToString();
            TxtKAM.Text = info.KeepAliveMultiplier.ToString();
            TxtNumAI.Text = info.NumEnlacesAI.ToString();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        GuardarCambios();
    }
    /// <summary>
    /// 
    /// </summary>
    private void GuardarCambios()
    {
        try
        {
            bool nuevo = false;
            if (info == null)
            {
                info = new ServiciosCD40.Sistema();
                nuevo = true;
	            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		        KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
			    info.IdSistema = s.Value;
				Session["idsistema"] = info.IdSistema;
            }
            //info.TiempoPtt = UInt16.Parse(TxtPTT.Text);
            //info.TiempoSinJack1 = UInt16.Parse(TxtSinJack1.Text);
            //info.TiempoSinJack2 = UInt16.Parse(TxtSinJack2.Text);

            //info.TamLiteralEnlExt = UInt16.Parse(TxtTamLitEnlExt.Text);
            //info.TamLiteralDA = UInt16.Parse(TxtTamLitDA.Text);
            //info.TamLiteralEmplazamiento = UInt16.Parse(TxtTamLitEmpl.Text);
            //info.TamLiteralIA = UInt16.Parse(TxtTamLitIA.Text);
            info.GrupoMulticastConfiguracion = TxtGrupoMulti.Text;
            info.PuertoMulticastConfiguracion = UInt16.Parse(TxtPortMulti.Text);
            //info.VersionIP = UInt16.Parse(DListVersionIP.SelectedValue);
            if (TxtKAP.Text.Length > 0)
                info.KeepAlivePeriod = Convert.ToUInt32(TxtKAP.Text);
            if (TxtKAM.Text.Length > 0)
                info.KeepAliveMultiplier = Convert.ToUInt32(TxtKAM.Text);
            if (TxtEnlIntPag.Text.Length > 0)
                info.NumDestinosInternosPag = UInt16.Parse(TxtEnlIntPag.Text);
            if (TxtFrecPag.Text.Length > 0)
                info.NumFrecPagina = UInt16.Parse(TxtFrecPag.Text);
            if (TxtLlamIDA.Text.Length > 0)
                info.NumLlamadasEnIda = UInt16.Parse(TxtLlamIDA.Text);
            if (TxtLlamEntIDA.Text.Length > 0)
                info.NumLlamadasEntrantesIda = UInt16.Parse(TxtLlamEntIDA.Text);
            if (TxtPagEnlInt.Text.Length > 0)
                info.NumPagDestinosInt = UInt16.Parse(TxtPagEnlInt.Text);
            if (TxtPagFrec.Text.Length > 0)
                info.NumPagFrec = UInt16.Parse(TxtPagFrec.Text);
            if (TxtNumAI.Text.Length > 0)
                info.NumEnlacesAI = UInt16.Parse(TxtNumAI.Text);

            if (nuevo)
            {
				if (ServicioCD40.InsertSQL(info) < 0)
                    logDebugView.Warn("(ParametrosGenerales-GuardarCambios): No se ha realizado el insert");
			}
            else
				if (ServicioCD40.UpdateSQL(info) < 0)
                    logDebugView.Warn("(ParametrosGenerales-GuardarCambios): No se ha realizado el update");

			ActualizaWebPadre(true);
			MuestraDatos(DameDatos());
		}
        catch (Exception e)
        {
            logDebugView.Error("(ParametrosGenerales-GuardarCambios): ",e);
        }

		EstadoNormal();

		// Parámetros para control de backup
		ServiciosCD40.DatosControlBackup datosBackup = new ServiciosCD40.DatosControlBackup();
		datosBackup.Profundidad = new uint[8];

		datosBackup.NomRecursoHistoricos = TBRecursoHistoricos.Text;
		datosBackup.NomRecursoIndicadores = TBRecursoIndicadores.Text;
		datosBackup.Profundidad[0] = Convert.ToUInt32(RBLHardware.SelectedValue);
		datosBackup.Profundidad[1] = Convert.ToUInt32(RBLTelefonia.SelectedValue);
		datosBackup.Profundidad[2] = Convert.ToUInt32(RBLRadio.SelectedValue);
		datosBackup.Profundidad[3] = Convert.ToUInt32(RBLGestion.SelectedValue);
		datosBackup.Profundidad[4] = Convert.ToUInt32(RadioButtonList1.SelectedValue);
		datosBackup.Profundidad[5] = Convert.ToUInt32(RadioButtonList2.SelectedValue);
		datosBackup.Profundidad[6] = Convert.ToUInt32(RadioButtonList3.SelectedValue);
		datosBackup.Profundidad[7] = Convert.ToUInt32(RadioButtonList4.SelectedValue);

		ServicioCD40.GeneraDatosControlBackup((string)Session["idsistema"], datosBackup);

		// Crea las tareas para realizar los backups según la profundidad configurada.
		// La creación de tareas en Windows 7 da problemas de excepción con la librería
        // Revisar.
        // CreaTareasBackup(datosBackup);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="datosBackup"></param>
	private void CreaTareasBackup(ServiciosCD40.DatosControlBackup datosBackup)
	{
        //string[] nombresTareasBackup ={"Backup_Historicos_Hardware","Backup_Historicos_Telefonia","Backup_Historicos_Radio","Backup_Historicos_Gestion",
        //            "Backup_Indicadores_Hardware","Backup_Indicadores_Telefonia","Backup_Indicadores_Radio","Backup_Indicadores_Gestion"};
        //string[] argumentosTareasBackup ={"-n{0} -iHardware -bBackup_Historicos -s{1}",
        //                                    "-n{0} -iTelefonia -bBackup_Historicos -s{1}",
        //                                    "-n{0} -iRadio -bBackup_Historicos -s{1}",
        //                                    "-n{0} -iGestion -bBackup_Historicos -s{1}",
        //                                    "-n{0} -iHardware -bBackup_Indicadores -s{1}",
        //                                    "-n{0} -iTelefonia -bBackupIndicadores -s{1}",
        //                                    "-n{0} -iRadio -bBackupIndicadores -s{1}",
        //                                    "-n{0} -iGestion -bBackupIndicadores -s{1}"};

        //Ref_Estadisticas.Estadisticas serviceEstadisticas = new Ref_Estadisticas.Estadisticas();
        //Ref_Estadisticas.Tareas t = new Ref_Estadisticas.Tareas();

        //for (int i = 0; i < 8; i++)
        //{
        //    t.IdSistema = (string)Session["idsistema"];
        //    t.IdTarea = nombresTareasBackup[i];
        //    t.Argumentos = string.Format(argumentosTareasBackup[i], nombresTareasBackup[i], t.IdSistema);
        //    t.Hora = (new TimeSpan(0, 0, 0)).ToString();
        //    t.Periodicidad = Ref_Estadisticas._Enum_Periodicidad.D;
        //    t.Comentario = "Tarea backup";
        //    t.Programa = "Indicadores.exe";
        //    t.Intervalo = 1;

        //    serviceEstadisticas.EliminarTarea(t);
        //    serviceEstadisticas.CrearTarea(t);
        //}
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
			case "IBSistema":
                IBSistema.CssClass = "buttonImageSelected";
                IBHistoricos.CssClass = "buttonImage";
                IBTareas.CssClass = "buttonImage";
                //IBSistema.ImageUrl = GetLocalResourceObject("IBSistemaResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuSistemaSistemaSelected.JPG";
                //IBHistoricos.ImageUrl = GetLocalResourceObject("IBHistoricosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSistemaHistoricosUnSelected.JPG";
                //IBTareas.ImageUrl = GetLocalResourceObject("IBTareasResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSistemaTareasUnSelected.JPG";
				MultiView1.ActiveViewIndex = UltimaPagina = 0;
				Panel1.Width = 800;
				Panel1.Height = 320;
				break;
			case "IBHistoricos":
                IBSistema.CssClass = "buttonImage";
                IBHistoricos.CssClass = "buttonImageSelected";
                IBTareas.CssClass = "buttonImage";
                //IBSistema.ImageUrl = GetLocalResourceObject("IBSistemaResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSistemaSistemaSelected.JPG";
                //IBHistoricos.ImageUrl = GetLocalResourceObject("IBHistoricosResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuSistemaHistoricosUnSelected.JPG";
                //IBTareas.ImageUrl = GetLocalResourceObject("IBTareasResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSistemaTareasUnSelected.JPG";
				MultiView1.ActiveViewIndex = UltimaPagina = 1;
				Panel1.Width = 660;
				Panel1.Height = 465;
				break;
			case "IBTareas":
                IBSistema.CssClass = "buttonImage";
                IBHistoricos.CssClass = "buttonImage";
                IBTareas.CssClass = "buttonImageSelected";
                //IBSistema.ImageUrl = GetLocalResourceObject("IBSistemaResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSistemaSistemaSelected.JPG";
                //IBHistoricos.ImageUrl = GetLocalResourceObject("IBHistoricosResource1.ImageUrl").ToString(); //"~/Configuracion/Images/MenuSistemaHistoricosUnSelected.JPG";
                //IBTareas.ImageUrl = GetLocalResourceObject("IBTareasResource1.ImageUrlSelected").ToString(); //"~/Configuracion/Images/MenuSistemaTareasUnSelected.JPG";
				MultiView1.ActiveViewIndex = UltimaPagina = 2;
				Panel1.Height = 270;
				Panel1.Width = 920;
				MuestraTarea(0);
				break;
		}
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="indice"></param>
	private void MuestraTarea(int indice)
	{
		if (ListaTareas.Tables[0].Rows.Count > 0)
		{
			TBNombreTarea.Text = (string)ListaTareas.Tables[0].Rows[indice].ItemArray[1];
			TBArgumentos.Text = (string)ListaTareas.Tables[0].Rows[indice].ItemArray[3];
			TBHora.Text = ((TimeSpan)ListaTareas.Tables[0].Rows[indice].ItemArray[4]).ToString().Remove(5, 3);
			DDLPeriodicidad.SelectedValue = (string)ListaTareas.Tables[0].Rows[indice].ItemArray[5];
			TBComentario.Text = (ListaTareas.Tables[0].Rows[indice].ItemArray[6] != System.DBNull.Value) ? ListaTareas.Tables[0].Rows[indice].ItemArray[6].ToString() : "";
			TBPrograma.Text = (string)ListaTareas.Tables[0].Rows[indice].ItemArray[2];
		}
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnNuevaTarea_Click(object sender, EventArgs e)
	{
        /*
		TBPrograma.Visible = false;
		File1.Visible = true;
		TBArgumentos.ReadOnly = TBComentario.ReadOnly = TBHora.ReadOnly = TBNombreTarea.ReadOnly = false;
		DDLPeriodicidad.Enabled = true;
		//Label2.Visible = Label3.Visible = Label4.Visible = Label5.Visible = Label6.Visible = Label7.Visible = true;
		BtnAnadirTarea.Visible = BtnCancelTarea.Visible = true;
		GVTareas.Visible = BtnNuevaTarea.Visible = false;

		TBNombreTarea.Text = TBArgumentos.Text = TBComentario.Text = "";
		DDLPeriodicidad.SelectedValue = (string)ServiciosCD40._Enum_Periodicidad.D.ToString();
		TBHora.Text = DateTime.Now.ToString("HH:mm");
         */
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnCancelTarea_Click(object sender, EventArgs e)
	{
		TBPrograma.Visible = true;
		File1.Visible = false;
		TBArgumentos.ReadOnly = TBComentario.ReadOnly = TBHora.ReadOnly = TBNombreTarea.ReadOnly = true;
		DDLPeriodicidad.Enabled = false;
		//Label2.Visible = Label3.Visible = Label4.Visible = Label5.Visible = Label6.Visible = Label7.Visible = true;
		BtnAnadirTarea.Visible = BtnCancelTarea.Visible = false;
		GVTareas.Visible = BtnNuevaTarea.Visible = true;
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void BtnAnadirTarea_Click(object sender, EventArgs e)
	{
		TBPrograma.Visible = true;
		File1.Visible = false;
		TBArgumentos.ReadOnly = TBComentario.ReadOnly = TBHora.ReadOnly = TBNombreTarea.ReadOnly = true;
		DDLPeriodicidad.Enabled = false;
		//Label2.Visible = Label3.Visible = Label4.Visible = Label5.Visible = Label6.Visible = Label7.Visible = true;
		BtnAnadirTarea.Visible = BtnCancelTarea.Visible = false;
		GVTareas.Visible = BtnNuevaTarea.Visible = true;

		/*
        ServiciosCD40.Tareas t = new ServiciosCD40.Tareas();

		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
		t.IdSistema = s.Value;
		t.IdTarea = TBNombreTarea.Text;

		// Sólo con IE se recupera el path completo. 
		// De todas formas hay que tener en cuenta que este código se ejecuta en el servidor... 
		// y el path puede que sea el del lado cliente. La aplicación de la tarea seguramente esté alojada
		// del lado del servidor, con lo que esta parte de la aplicación sólo debería ejecutarse en la 
		// misma máquina que el servidor.
		t.Programa = File1.Value;		

		switch (DDLPeriodicidad.SelectedValue)
		{
			case "S":
				t.Periodicidad = ServiciosCD40._Enum_Periodicidad.S;
				break;
			case "D":
				t.Periodicidad = ServiciosCD40._Enum_Periodicidad.D;
				break;
			case "M":
				t.Periodicidad = ServiciosCD40._Enum_Periodicidad.M;
				break;
			default:
				break;
		}
		
		t.Hora = TBHora.Text;
		t.Argumentos = TBArgumentos.Text;
		t.Comentario = TBComentario.Text;

		if (ServicioCD40.InsertSQL(t) < 0)
			logDebugView.Warn("(ParametrosGenerales-BtnAnadirTarea_Click): No se ha realizado el insert de la tarea");
		else
			LeeTareas();
         */
	}

	//protected void BtnEliminaTarea_Click(object sender, EventArgs e)
	//{
	//    ServiciosCD40.Tareas t = new ServiciosCD40.Tareas();

	//    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
	//    KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
	//    t.IdSistema = s.Value;
	//    t.IdTarea = TBNombreTarea.Text;

	//    if (ServicioCD40.DeleteSQL(t) < 0)
	//        logDebugView.Warn("(ParametrosGenerales-BtnEliminaTarea_Click): No se ha realizado el delete de la tarea");
	//}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void GVTareas_OnSelectedIndexChanged(object sender, GridViewSelectEventArgs e)
	{
		MuestraTarea(e.NewSelectedIndex);
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void GVTareas_OnDeleting(object sender, GridViewDeleteEventArgs e)
	{
        /*
		ServiciosCD40.Tareas t = new ServiciosCD40.Tareas();
		int indice = e.RowIndex;

		if (GVTareas.Rows[indice].FindControl("CBProgramada") != null &&
			((CheckBox)GVTareas.Rows[indice].FindControl("CBProgramada")).Checked)
		{
			cMsg.alert((string)GetGlobalResourceObject("Espaniol","BorrarTarea"));
			return;
		}

		Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
		t.IdSistema = s.Value;
		t.IdTarea = (string)ListaTareas.Tables[0].Rows[indice].ItemArray[1];

		if (ServicioCD40.DeleteSQL(t) < 0)
			logDebugView.Warn("(ParametrosGenerales-GVTareas_OnDeleting): No se ha realizado el delete de la tarea");
		else
			LeeTareas();
         */
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	protected void GVTareas_OnRowCommand(object sender, GridViewCommandEventArgs e)
	{
    //    if(e.CommandName=="BtnCommandProgramar")
    //    {
    //        Ref_Estadisticas.Estadisticas serviceEstadisticas = new Ref_Estadisticas.Estadisticas();
    //        Ref_Estadisticas.Tareas t = new Ref_Estadisticas.Tareas();

    //        // Extraer la fila del GridView del argumento del programa
    //        int index = Convert.ToInt32(e.CommandArgument);
    //        bool programada = ((CheckBox)GVTareas.Rows[index].FindControl("CBProgramada")).Checked = !((CheckBox)GVTareas.Rows[index].FindControl("CBProgramada")).Checked;


    //        t.IdSistema = (string)ListaTareas.Tables[0].Rows[index].ItemArray[0];
    //        t.IdTarea = (string)ListaTareas.Tables[0].Rows[index].ItemArray[1];
    //        t.Argumentos = (string)ListaTareas.Tables[0].Rows[index].ItemArray[3];
    //        t.Hora = ((TimeSpan)ListaTareas.Tables[0].Rows[index].ItemArray[4]).ToString().Remove(5, 3);
    //        switch ((string)ListaTareas.Tables[0].Rows[index].ItemArray[5])
    //        {
    //            case "S":
    //                t.Periodicidad = Ref_Estadisticas._Enum_Periodicidad.S;
    //                break;
    //            case "D":
    //                t.Periodicidad = Ref_Estadisticas._Enum_Periodicidad.D;
    //                break;
    //            case "M":
    //                t.Periodicidad = Ref_Estadisticas._Enum_Periodicidad.M;
    //                break;
    //            default:
    //                break;
    //        }
    //        t.Comentario = ListaTareas.Tables[0].Rows[index].ItemArray[7] != System.DBNull.Value ? (string)ListaTareas.Tables[0].Rows[index].ItemArray[7] : "";
    //        t.Programa = (string)ListaTareas.Tables[0].Rows[index].ItemArray[2];

    //        if (programada)
    //        {
    //            serviceEstadisticas.CrearTarea(t);
    //        }
    //        else
    //        {
    //            serviceEstadisticas.EliminarTarea(t);
    //        }

    //    }
    }
}
