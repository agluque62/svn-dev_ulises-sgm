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

public partial class TablasCalidad : PageBaseCD40.PageCD40		//System.Web.UI.Page
{
	static bool PermisoSegunPerfil;

    static bool Modificar = false;

    static int[] Tabla_idbss = new int[16];
    

    
    private Mensajes.msgBox cObjMsg;
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

        cObjMsg = (Mensajes.msgBox)this.Master.FindControl("MsgBox1");

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
		}
    }


    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        { 
            ServiciosCD40.Tabla_bss t = new ServiciosCD40.Tabla_bss();
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
                ListBox1.Items.Add(((ServiciosCD40.Tabla_bss)nu[i]).Name);
                ListBox1.Items[i].Value = ((ServiciosCD40.Tabla_bss)nu[i]).IdTabla_bss.ToString();              
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

            MostrarRecursos();
            MostrarTablaCalidad();
            
            TableCalidad.Visible = BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
            BtEliminar.Visible = (BtEliminar.Visible && (LBoxRecursos.Items.Count > 0 ? false : true));
        }
        else//MAF
        {
            TableCalidad.Visible = BtModificar.Visible = BtEliminar.Visible = false;        
        }
	}
	    
   private void MostrarRecursos()
   {
       try
       {
           BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarTablaCalidad"), ListBox1.SelectedItem.Text);
           Label3.Text = (string)GetLocalResourceObject("RecursosTablaCalidad") + " " + ListBox1.SelectedItem.Text;
           Label3.Visible = true;
           LBoxRecursos.Visible = true;
           LBoxRecursos.Items.Clear();

          
           //ServiciosCD40.Tabla_bss_recurso t = new ServiciosCD40.Tabla_bss_recurso();
           //ServiciosCD40.RecursosRadio r = new ServiciosCD40.RecursosRadio();


           string[] listaRecursos = ServicioCD40.GetRecursosRadioFromTablaBss(Convert.ToInt32(ListBox1.SelectedValue));

           if (listaRecursos != null)
           {
               for (int i = 0; i < listaRecursos.Length; i++)
                   LBoxRecursos.Items.Add(listaRecursos[i]);
           }

       }
       catch (Exception e)
       {
           logDebugView.Error("(TablasCalidad-MostrarRecursos):", e);
       } 
   }

   private void MostrarTablaCalidad()
   {
       try
       {

           // Leo idRecursos asociado al idTabla en Tabla_bss_idtabla_bss[]
           ServiciosCD40.ValoresTabla t = new ServiciosCD40.ValoresTabla();
           t.Tabla_bss_idtabla_bss = Convert.ToInt32(ListBox1.SelectedValue);
            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
          

           for (int i = 0; i < d.Length; i++)
           {

               Tabla_idbss[i] = ((ServiciosCD40.ValoresTabla)d[i]).IdValores_Tabla;
               switch (i)
               {
                   
                   case 0:                                          
                        DropDownListVQTRow1_2.SelectedValue = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow1_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                   break;
                   case 1:
                      
                        DropDownListVQTRow2_2.SelectedValue = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow2_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                   break;
                   case 2:

                        DropDownListVQTRow3_2.SelectedValue = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow3_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                   break;
                   case 3:

                        DropDownListVQTRow4_2.SelectedValue = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow4_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                   break;
                   case 4:

                        DropDownListVQTRow5_2.SelectedValue = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow5_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                   break;
                   case 5:

                        DropDownListVQTRow6_2.SelectedValue = ((ServiciosCD40.ValoresTabla)d[i]).Valor_rssi.ToString();
                        //TableVQTRow6_1.Text = ((ServiciosCD40.ValoresTabla)d[i]).Valor_Prop.ToString();
                   break;                   
               }
           }
       }
       catch (Exception e)
       {
           logDebugView.Error("(TablasCalidad-MostrarTablaCalidad:", e);
       }
   }

   protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            MostrarRecursos();
            MostrarTablaCalidad();
            
            BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
            BtEliminar.Visible = (BtEliminar.Visible && (LBoxRecursos.Items.Count > 0 ? false : true));
        }
    }

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        if (ListBox1.SelectedItem.Text != "")
        {
            LBoxRecursos.Visible = false;
            Label3.Visible = false;

            IndexListBox1 = ListBox1.SelectedIndex;
            Session["elemento"] = ListBox1.SelectedItem.Text;
            EliminarElemento();
        }
    }

    private void EliminarElemento()
    {
        try
        {
           
           ServiciosCD40.Tabla_bss n = new ServiciosCD40.Tabla_bss();
            
           
            n.IdTabla_bss = Convert.ToInt32(ListBox1.SelectedValue);
            
			
            if (ServicioCD40.DeleteSQL(n) < 0)
            {
                logDebugView.Warn("(TablasCalidad-EliminarElemento): no se ha borrado la Tabla de Calidad de Audio");
                cObjMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorEliminarTablas"), n.Name));
            }
            else
            {

                cObjMsg.alert((string)GetGlobalResourceObject("Espaniol", "ElementoEliminado"));
            }
                        
            ListBox1.Items.Clear();
            MuestraDatos(DameDatos());
        }
        catch (Exception e)
        {
            logDebugView.Error("(TablasCalidad-EliminarElemento):", e);
        }
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        GuardarCambios();
     

    }

    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
        
    }

    private void GuardarCambioTablaCalidad()
    {
        try
        {

           // Leo idRecursos asociado al idTabla en Tabla_bss_idtabla_bss[]
           ServiciosCD40.ValoresTabla t = new ServiciosCD40.ValoresTabla();
           ServiciosCD40.Tabla_bss n = new ServiciosCD40.Tabla_bss();


           n.Name = TextBox1.Text;
           ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(n); // id_tabla =  Tabla_bss(name)           
           t.Tabla_bss_idtabla_bss=((ServiciosCD40.Tabla_bss)d[0]).IdTabla_bss;

            for (int i = 0; i <= 5; i++)
            {
               
                t.Valor_Prop = i;

                switch (i)
                {
                    case 0:
                        t.Valor_rssi = Convert.ToInt32(DropDownListVQTRow1_2.SelectedValue);                         
                        break;
                    case 1:
                        t.Valor_rssi = Convert.ToInt32(DropDownListVQTRow2_2.SelectedValue);                        
                        break;
                    case 2:
                        t.Valor_rssi = Convert.ToInt32(DropDownListVQTRow3_2.SelectedValue);                        
                        break;
                    case 3:
                        t.Valor_rssi = Convert.ToInt32(DropDownListVQTRow4_2.SelectedValue);                        
                        break;
                    case 4:
                        t.Valor_rssi = Convert.ToInt32(DropDownListVQTRow5_2.SelectedValue);                        
                        break;
                    case 5:
                        t.Valor_rssi = Convert.ToInt32(DropDownListVQTRow6_2.SelectedValue);                        
                        break;                   
                }

                if (!Modificar)
                {
                    if (ServicioCD40.InsertSQL(t) < 0)
                    {
                        logDebugView.Warn("(TablasCalidad-GuardarCambioTablaCalidad): no se ha guardado la tabla de calidad de audio.");
                        cObjMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorGuardarTablaCalidad"), TextBox1.Text)); 
                    }
                    else
                    {
                        ActualizaWebPadre(true);
                    }
                }
                else
                {

                    t.IdValores_Tabla = Tabla_idbss[i];
                 
                    if (ServicioCD40.UpdateSQL(t) < 0)
                        logDebugView.Warn("(TablasCalidad-GuardarCambioTablaCalidad): No se ha podido actualizar lla tabla de conversión a índices RSSI (tabla Calidad).");
                }


            }

            //BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
            //BtEliminar.Visible = (BtEliminar.Visible && (LBoxRecursos.Items.Count > 0 ? true : false));

        }
        catch (Exception e)
        {
            logDebugView.Error("(TablasCalidad-MostrarTablaCalidad:", e);
        }
    }

    private void GuardarCambiosRecursos()
    {
        try
        {

            ServiciosCD40.Tabla_bss n = new ServiciosCD40.Tabla_bss();          

            n.Name = TextBox1.Text;


            if (!Modificar)// Nueva tabla
            {
                NewItem = TextBox1.Text;
                if (ServicioCD40.InsertSQL(n) < 0)
                {
                    logDebugView.Warn("(TablasCalidad-GuardarCambiosRecursos): no se ha guardado la tabla de conversión a índices RSSI (tabla Calidad).");
                    cObjMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorGuardarTablaCalidad"), n.Name)); 
                }
                else
                {
                    ActualizaWebPadre(true);
                }
            }
            else // Modificar Tabla
            {
                n.IdTabla_bss = Convert.ToInt32(ListBox1.SelectedValue);

                if (ServicioCD40.UpdateSQL(n) < 0)
                    logDebugView.Warn("(TablasCalidad-GuardarCambiosRecursos): No se ha podido actualizar la tabla de conversión a índices RSSI (tabla Calidad).");
            }

           
        }
        catch (Exception e)
        {
            logDebugView.Error("(TablasCalidad-GuardarCambiosRecursos):", e);
        }
    }
    
    private void GuardarCambios()    
    {

        //MVO:  El nombre de la tabla debe ser único, tanto si se modifica con como si es una tabla nueva
        if (!NombreTablaCalidadRepetida())
        {
            GuardarCambiosRecursos();
            GuardarCambioTablaCalidad();

            BtAceptar.Visible = false;
            BtCancelar.Visible = false;
            Label2.Visible = false;
            TextBox1.Visible = false;
            TableCalidad.Enabled = false;

            ListBox1.Enabled = true;
            BtNuevo.Visible = PermisoSegunPerfil;

            ListBox1.Items.Clear();
            MuestraDatos(DameDatos());
            ValidationSummary1.Visible = false;
            RequiredFieldValidator1.Visible = false;

            Panel1.Enabled = false;
        }
        else    
        {
            cObjMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "ErrorTablaCalidadExiste"), TextBox1.Text));
            return;
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
        
        TableCalidad.Enabled = false;
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
        IndexListBox1 = ListBox1.SelectedIndex;

        Label3.Visible = false;
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
        {
            if (ListBox1.Items.Count >= 4)
            {
                cObjMsg.alert((string)GetGlobalResourceObject("Espaniol", "MaxZonas"));
                return;
            }
        }

        Panel1.Enabled = true;

        Label3.Visible = false;
        LBoxRecursos.Visible = false;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        BtModificar.Visible = false;
        BtEliminar.Visible = false;
        TextBox1.Text = "";
        Label2.Visible = true;
        TextBox1.Visible = true;        
        TableCalidad.Enabled = true;
        TableCalidad.Visible = true;
        ListBox1.Enabled = false;
        BtNuevo.Visible = false;      
        ValidationSummary1.Visible = true;
        RequiredFieldValidator1.Visible = true;

        Modificar = false;
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
        TableCalidad.Enabled = true;

        IndexListBox1 = ListBox1.SelectedIndex;
        ListBox1.Enabled = false;
        
       // BtEliminar.Visible = false;
        ValidationSummary1.Visible = true;
        RequiredFieldValidator1.Visible = true;
              
        Modificar = true;

    }

    private bool NombreTablaCalidadRepetida()
    {
        //Si se está dando de alta un nuevo recurso o se está modificando hay que comprobar que no existe 
        //en la lista.

        if (!Modificar) //Nueva tabla 
        {
            for (int i = 0; i < ListBox1.Items.Count; i++)
            {
                if (String.Compare((TextBox1.Text), ListBox1.Items[i].Text, false) == 0)
                {
                    return true;
                }

            }
        }
        else
        {
            //Se está modificando
            for (int i = 0; i < ListBox1.Items.Count; i++)
            {
                if ((String.Compare((TextBox1.Text), ListBox1.Items[i].Text, true) == 0) &&(i != ListBox1.SelectedIndex))
                {
                    //Si se está modificando no debe ser el mismo
                    return true;
                }
            }

        }
        return false;
    }
    
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }
}
