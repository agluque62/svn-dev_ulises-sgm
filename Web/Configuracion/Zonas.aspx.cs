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

public partial class Zonas : PageBaseCD40.PageCD40		//System.Web.UI.Page
{
	static bool PermisoSegunPerfil;

    static bool Modificar = false;

    
    private Mensajes.msgBox cMsg;
    private static ILog _logDebugView;
	private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();
    
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

            PermisoSegunPerfil = BtModificar.Visible = BtNuevo.Visible = perfil != "1";
			//BtEliminar.Visible = ListBox1.Items.Count > 0 && PermisoSegunPerfil;
		}

        if (!IsPostBack)
        {
            BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");

            MuestraDatos(DameDatos());
            CargarInforme();
        }
        else
        {
            if (null == Session["idsistema"])
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
                Session["idsistema"] = s.Value;
            }
        }
    }

	protected void CargarInforme()
	{
		LBImprimir.Attributes.Remove("onclick");
        string comando = "AbreVentana('../Informes/Report.aspx?Report=Emplazamientos.rpt');return false;";
        //JC string comando = "AbreVentana('../Informes/Report.aspx?Report=Zonas.rpt');return false;";
		LBImprimir.Attributes.Add("onclick", comando);
	}

    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.Zonas t = new ServiciosCD40.Zonas();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;

            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);

            return d;
        }
        catch (Exception e)
        {
            logDebugView.Error("(Zonas-DameDatos):",e);
        }
        return null;
    }

    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
        ListBox1.Items.Clear();
        Modificar = false;

        if (nu != null)
        {
            for (int i = 0; i < nu.Length; i++)
            {
                ListBox1.Items.Add(((ServiciosCD40.Zonas)nu[i]).Nombre);
                ListBox1.Items[i].Value = ((ServiciosCD40.Zonas)nu[i]).IdZonas.ToString();
            }
        }

        if (ListBox1.Items.Count > 0)
        {

            ActualizaWebPadre(true);

            if (ListBox1.Items.FindByText(NewItem) != null)
            {
                ListBox1.Items.FindByText(NewItem).Selected = true;
                IndexListBox1 = ListBox1.SelectedIndex;
                NewItem = string.Empty;
            }
            else
                ListBox1.SelectedIndex = IndexListBox1 != -1 && IndexListBox1 < ListBox1.Items.Count ? IndexListBox1 : 0;

            MostrarRecursos(); //MAF_zonas

            //GeneraXmlParaInforme();
        }
        else//MAF
        {
            BtModificar.Visible = BtEliminar.Visible = false;
        
        }

	}


    //MAF_zonas
    
   private void MostrarRecursos()
    {
        try
        {
            BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarZona"), ListBox1.SelectedItem.Text);
            Label3.Text = (string)GetLocalResourceObject("RecursosZonas") + " " + ListBox1.SelectedItem.Text;
            Label3.Visible = true;
            LBoxRecursos.Visible = true;

            ServiciosCD40.RecursosRadio t = new ServiciosCD40.RecursosRadio();
          
            t.IdSistema = (string)Session["idsistema"];
            t.Zonas_IdZonas = Convert.ToInt32(ListBox1.SelectedValue);
           
			ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            LBoxRecursos.Items.Clear();
                        
            for (int i = 0; i < d.Length; i++)            
                LBoxRecursos.Items.Add(((ServiciosCD40.RecursosRadio)d[i]).IdRecurso);

            BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
            // Oculta el botón BtEliminar si no hay Destinos asociados a la Zona
            BtEliminar.Visible = (BtEliminar.Visible  && (LBoxRecursos.Items.Count > 0 ? false : true));           
     
         
        }
        catch (Exception e)
        {
            logDebugView.Error("(Zonas-MostrarRecursos):", e);
        }
    }
    
    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            MostrarRecursos();//MAF_zonas
        }
    }

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        if (ListBox1.SelectedItem.Text != "" && Session["idsistema"]!=null)
        {
            LBoxRecursos.Visible = false;
            Label3.Visible = false;

            IndexListBox1 = ListBox1.SelectedIndex;
            Session["elemento"] = ListBox1.SelectedItem.Text;
            EliminarElemento();

           // string texto = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarZona"), ListBox1.SelectedItem.Text);
           // cMsg.confirm(texto, "eliminaelemento");
            
        }
    }

    private void EliminarElemento()
    {
        try
        {
           
            ServiciosCD40.Zonas n = new ServiciosCD40.Zonas();
            
            n.IdSistema = (string)Session["idsistema"];
            n.Nombre = (string)Session["elemento"];
            n.IdZonas = Convert.ToInt32(ListBox1.SelectedValue);
            
			
            if (ServicioCD40.DeleteSQL(n) < 0)
            {
                logDebugView.Warn("(Zonas-EliminarElemento): no se ha borrado la zona");                
                cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorEliminarZonas"), n.Nombre));
            }
            else
            {
            
                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
            }
                        
            ListBox1.Items.Clear();
            MuestraDatos(DameDatos());
        }
        catch (Exception e)
        {
            logDebugView.Error("(Zonas-EliminarElemento):", e);
        }
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        string StrSistema = (string)Session["idsistema"];
        int iElementoSeleccionado=ListBox1.SelectedIndex;

        if (!Modificar || (Modificar && iElementoSeleccionado != -1 && String.Compare(TextBox1.Text, ListBox1.Items[iElementoSeleccionado].Text) != 0))
        {
            //Se comprueba que el identificador de la zona sea único (Emplazamientos,zonas)
            //Si se está insertando una nueva zona o si se está modificando el nombre 
            if (TextBox1.Enabled && bIdentificadorAsignado(StrSistema, TextBox1.Text))
            {
                //Existe otro zona o emplazamiento con el mismo identificador
                string strMsg = string.Empty;

                if (null != GetGlobalResourceObject("Espaniol", "ErrorGuardarZona"))
                    strMsg = string.Format(GetGlobalResourceObject("Espaniol", "ErrorGuardarZona").ToString(), TextBox1.Text);
                else
                    strMsg = string.Format("El identificador de la zona {0} ya existe", TextBox1.Text);

                cMsg.alert(strMsg);
                return;
            }
        }

        GuardarCambios(StrSistema);
    }

    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
    }

    private void GuardarCambios(string strSistema)
    {
        try
        {
            
            ServiciosCD40.Zonas n = new ServiciosCD40.Zonas();
            n.IdSistema = strSistema;
            n.Nombre = TextBox1.Text;

            if (!Modificar)
            {
                NewItem = TextBox1.Text;
                    
                if (ServicioCD40.InsertSQL(n) < 0)
                {
                    logDebugView.Warn("(Zonas-GuardarElemento): no se ha guardado el zonas.");
                    cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorGuardarZona"), n.Nombre)); // cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorGuardarZonas"), n.idZonas));
                }
                else
                {
                    ActualizaWebPadre(true);
                }
            }
            else
            {
                n.IdZonas = Convert.ToInt32(ListBox1.SelectedValue);
                    
                if (ServicioCD40.UpdateSQL(n) < 0)
                    logDebugView.Warn("(Zonas-GuardarCambios): No se ha podido actualizar la zona.");

            }


            BtAceptar.Visible = false;
            BtCancelar.Visible = false;
            Label2.Visible = false;
            TextBox1.Visible = false;
            ListBox1.Enabled = true;
			BtNuevo.Visible = PermisoSegunPerfil;
			           
            //BtEliminar.Visible = ListBox1.Items.Count > 0 && PermisoSegunPerfil;
            ListBox1.Items.Clear();
            MuestraDatos(DameDatos());
            ValidationSummary1.Visible = false;
            RequiredFieldValidator1.Visible = false;

            Panel1.Enabled = false;

        }
        catch (Exception e)
        {
            logDebugView.Error("(Zonas-GuardarCambios):", e);
        }
    }

	protected override void AceptarCambios()
	{
		base.AceptarCambios();
	}

    protected override void CancelarCambios()
    {
		BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        Label2.Visible = false;
        TextBox1.Visible = false;
        ListBox1.Enabled = true;
		BtNuevo.Visible = PermisoSegunPerfil;
		//BtEliminar.Visible = ListBox1.Items.Count > 0 && PermisoSegunPerfil;
        ValidationSummary1.Visible = false;
        RequiredFieldValidator1.Visible = false;
        Panel1.Enabled = false;

		MuestraDatos(DameDatos());
    }

    protected void BtNuevo_Click(object sender, EventArgs e)
    {
		//
        Label3.Visible = false;
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
        {
            if (ListBox1.Items.Count >= 4)
            {
                cMsg.alert((string)GetGlobalResourceObject("Espaniol", "MaxZonas"));
                return;
            }
        }

        Panel1.Enabled = true;

        Label3.Visible = false;
        LBoxRecursos.Visible = false;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        BtModificar.Visible = false;
        TextBox1.Text = "";
        Label2.Visible = true;
        TextBox1.Visible = true;
       
        ListBox1.Enabled = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        ValidationSummary1.Visible = true;
        RequiredFieldValidator1.Visible = true;

        Modificar = false;
        IndexListBox1 = ListBox1.SelectedIndex;
    }

    protected void BtModificar_Click(object sender, EventArgs e)
    {
        Panel1.Enabled = true;
        
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        BtModificar.Visible = false;
              

        Label3.Visible = false;
        LBoxRecursos.Visible = false;
        BtAceptar.Visible = true;
        
        BtCancelar.Visible = true;
        BtModificar.Visible = false;
        TextBox1.Text = ListBox1.SelectedItem.Text;
        Label2.Visible = true;
        TextBox1.Visible = true;
        ListBox1.Enabled = false;
        
        BtEliminar.Visible = false;
        ValidationSummary1.Visible = true;
        RequiredFieldValidator1.Visible = true;

        Modificar = true;
        IndexListBox1 = ListBox1.SelectedIndex;

    }

    //Devuelve true si ya existe una zona o emplazamiento en el sistema con el mismo identificador. En caso contrario, false.
    private bool bIdentificadorAsignado(string strIdSistema, string strIdentificador)
    {
        bool bExiste = false;
        int iExiste = -1;

        // Se comprueba que no existe una zona o emplazamiento con el mismo identificador 
        iExiste = ServicioCD40.CheckIdentificadorAsignado("NA", strIdSistema, strIdentificador);

        if (iExiste > 0)
            bExiste = true;
        else if (iExiste < 0)
        {
            System.Text.StringBuilder strMsgError = new System.Text.StringBuilder();
            strMsgError.AppendFormat("(Zonas-bIdentificadorAsignado): el servicio servicioCD40.CheckIdentificadorAsignado('NA', '{0}', '{1}') ha devuelto el codigo {2}", strIdSistema, strIdentificador, iExiste);
            logDebugView.Error(strMsgError.ToString());
            strMsgError.Clear();
        }

        return bExiste;
    }
}
