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
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

public partial class GrupoFD : PageBaseCD40.PageCD40    // System.Web.UI.Page
{
    const string FREC_DESPLAZADA_MODO_ASAP = "0";
    const string FREC_DESPLAZADA_MODO_TIEMPO_FIJO = "1";

    const string DESTINORADIO_MODO_NORMAL = "0";
    const string DESTINORADIO_MODO_1_mas_1 = "1";
    const string DESTINORADIO_MODO_FD = "2";
    const string DESTINORADIO_MODO_EM = "3";

    const string TIPO_RECURSO_RX = "0";
    const string TIPO_RECURSO_TX = "1";
    const string TIPO_RECURSO_RXTX = "2";
    const string TIPO_RECURSO_AUDIO_HF_TX= "3";
    const string TIPO_RECURSO_M_N_RX = "4";
    const string TIPO_RECURSO_M_N_TX = "5";
    const string TIPO_RECURSO_M_N_RXTX = "6";
    const string TIPO_RECURSO_EE_RX = "7";
    const string TIPO_RECURSO_EE_TX = "8";
    const string TIPO_RECURSO_EE_RXTX = "9";

    
    const string DESTINORADIO_TIPOFRECUENCIA_VHF = "0";
    const string DESTINORADIO_TIPOFRECUENCIA_UHF = "1";
    const string DESTINORADIO_TIPOFRECUENCIA_HF = "2";

    const string MODO_TRANSMISION_CLIMAX = "C";
    const string MODO_TRANSMISION_ULTIMA_RECEPCION = "R";
    // De momento, no se permite configurar el modo de transmisión manual por lo que no aparece en la lista de valores permitidos
    const string MODO_TRANSMISION_MANUAL = "M";

    const string METODO_CLIMAX_RELATIVO = "0";
    const string METODO_CLIMAX_ABSOLUTO = "1";

    const string METODO_BSS_NINGUNO = "0";
    const string METODO_BSS_RSSI = "1";
    const string METODO_BSS_RSSI_NUCLEO = "2";
    const string FORMATO_LB_RECURSOS = "{0, -20} {1, -15} {2,-10}";
    //20201014  #4539 const string FORMATO_LB_RECURSOS_LIBRES = "{0, -20} {1, -15}";
    const string FORMATO_LB_RECURSOS_LIBRES = "{0, -20} {1, -20} {2,-15} ";

    private static ServiciosCD40.Tablas[] datos;
    
    private static ILog _logDebugView;
    private static KeyValueConfigurationElement s;
    static bool PermisoSegunPerfil;
    private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();
    private AsyncCallback CallbackCompletado;
    private Ulises5000Configuration.ToolsUlises5000Section UlisesToolsVersion;
    private static string Frecuencia = "";
    private static bool bCnfIdFormatoFrecuencia= true;
    //20200520 JOI ERROR #4425
    private static bool bRedundancia = false;

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

            //PermisoSegunPerfil = BtModificar.Visible = BtNuevo.Visible = perfil != "1" ;
            //Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            //Version = config.AppSettings.Settings["Version"];
            PermisoSegunPerfil = perfil != "1";
            BtModificar.Visible = BtNuevo.Visible = (!BtAceptar.Visible && PermisoSegunPerfil);

            Ulises5000Configuration.ToolsUlises5000Section UlisesTools = Ulises5000Configuration.ToolsUlises5000Section.Instance;

            UlisesToolsVersion = UlisesTools;
        }

        if (CallbackCompletado == null)
            CallbackCompletado = new AsyncCallback(OnCallBackCompleted);

        //Obtenemos el valor del parámetro DestinoRadioIdFormatoFrecuencia, para saber si el identificador del destino radio debe seguir
        //un formato determinado en función del tipo de frecuencia.
        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        s = config.AppSettings.Settings["DestinoRadioIdFormatoFrecuencia"];

        if (s != null && string.Compare(s.Value, "NO", true) == 0)
            bCnfIdFormatoFrecuencia = false;
        else
            bCnfIdFormatoFrecuencia = true;

        if (!IsPostBack)
        {
            IndexListBox1 = -1;

            ListEmplazamientos.Attributes.Add("disabled", "");
            ListTipos.Attributes.Add("disabled", "");
            ListEmplazamientosLibres.Attributes.Add("disabled", "");
            ListTiposLibres.Attributes.Add("disabled", "");

           
            // Mostrar Tipo destino radio HF sólo para NDjamena (Versión=2)
            if (UlisesToolsVersion.Tools["RadioHF"] == null)
                DListTipo.Items.RemoveAt(2);
            
            
            //BtAceptar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "AceptarCambios");            
            // cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AceptarCambios"), "aceptparam");
            BtCancelar_ConfirmButtonExtender.ConfirmText = (string)GetGlobalResourceObject("Espaniol", "CancelarCambios");            

            MuestraDatos(DameDatos());
            CargaEmplazamientos();
        }
        else
        {
            if (null == datos)
            {
                DameDatos();
            }

            if (Request.Form["SoloEliminaDeTFT"] == "1")
            {
                Request.Form["SoloEliminaDeTFT"].Replace("1", "0");

                EliminaDestinoDeTFT();
                EliminarElemento(true);
            }
        }
    }

    //Obtiene el texto correspondiente del fichero de recursos de la etiqueta que 
    // se le pasa como parámetro
    protected string ObtenerTextoRecursoLocal(string strEtiqueta,string strValorDefecto)
    {
        string strTexto = string.Empty;

        if (!string.IsNullOrEmpty(strEtiqueta))
        {
            if (GetLocalResourceObject(strEtiqueta) != null)
            {
                strTexto = GetLocalResourceObject(strEtiqueta).ToString();
            }
            else
                strTexto = strValorDefecto;
        }

        return strTexto;
    }


    protected void BtNuevo_Click(object sender, EventArgs e)
    {
        Panel1.Enabled = true;

        RequiredFieldIdentificador.Enabled = RequiredFieldIdentificador.Visible = true;
        //RequiredFieldValidator1.Enabled = RequiredFieldValidator1.Visible = true;
        errores.Visible = true;
        MostrarMenu();
        DListTipo.Enabled = true;   /* CheckExclusividad.Enabled = true; */

        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        BtModificar.Visible = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        ListBox1.Enabled = false;
        TxtIdEnlace.Enabled = true;
        TxtIdEnlace.ReadOnly = false;
        Label4.Visible = true;
        ListRecursos.Items.Clear();
        ListEmplazamientos.Items.Clear();
        Frecuencia = string.Empty;
        CheckBoxRedundancia.Checked = false;
        //20200520 JOI #4425
        bRedundancia = false;
        //VMG 18/02/2019
        LbEmplazamientoDefecto.Visible = false;
        DListEmplazamientoDefecto.Visible = false;
        LbTiempoVueltaADefecto.Visible = false;
        TxtTiempoVueltaADefecto.Visible = false;

        ListTipos.Items.Clear();


        DListEmplazamiento.SelectedIndex = 0;
       
      
        CargarRecursosSinAsignar(); 
        //CargarRecursosSinAsignarporEmplazamiento();

        ListRecursosLibres.Visible = true;
        
        LblFiltroEmplazamiento.Visible = DListEmplazamiento.Visible = DropDownFiltro.Visible = true;

        IButAsignar.Visible = true;
        IButQuitar.Visible = true;
        DListEmplazamiento.Enabled = true;

        //ListTiposLibres.Visible = (String.Compare("0", DropDownFiltro.SelectedValue) == 0);
        //ListEmplazamientosLibres.Visible = !ListTiposLibres.Visible;

        //MVO: Se visualizan/ocultan los campos de FD
        DListModoDestino.SelectedValue = DESTINORADIO_MODO_NORMAL;
        DListTipo.SelectedValue = DESTINORADIO_TIPOFRECUENCIA_VHF;
        //La frecuencia sintonizada sólo es configurable para la frecuencia  HF
        RFV_TBTunedFrequency.Enabled = RFV_TBTunedFrequency.Visible = false;
        TblTunedFreq.Visible = false;
        TBTunedFrequency.Text = string.Empty;

        if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_FD)
        {
            //Se hacen visibles las opciones de FD
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = true;
            Label9.Visible = DListMetodoClimax.Visible = TextVentanaBSS.Enabled = true;

            //El campo Tiempo CLD sólo es visible para las FD 
            Label44.Visible=TextBoxCLD.Visible = true;
            TextBoxCLD.Text = "1"; // Por defecto, se configura a 1 segundo
            //Se hace visible el campo Modo de Transmisión
            LbModoTransmision.Visible = DListModoTransmision.Visible = true;
            DListModoTransmision.SelectedValue = MODO_TRANSMISION_CLIMAX; //Por defecto, se selecciona Climax
            //20200611 JOI Error 3994
            //Si se activa por defecto Modo Transmisión CLIMAX se desactivan los asociados a Última Recepción
            LbEmplazamientoDefecto.Visible = false;
            DListEmplazamientoDefecto.Visible = false;
            LbTiempoVueltaADefecto.Visible = false;
            TxtTiempoVueltaADefecto.Visible = false;
        }
        //20200520 JOI Errores #4425
        else if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_NORMAL && bRedundancia)
        {
            //Se hacen visibles las opciones de MODO_NORMAL Y REDUNDANCIA
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = true;
            TextVentanaBSS.Enabled = true;
            Label9.Visible = DListMetodoClimax.Visible = false;
            Label44.Visible = TextBoxCLD.Visible = false;
            LbModoTransmision.Visible = DListModoTransmision.Visible = false;
        }
        else
        {
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible =  false;
            Label9.Visible = DListMetodoClimax.Visible = TextVentanaBSS.Enabled =  false;
            Label44.Visible = TextBoxCLD.Visible = false;
            LbModoTransmision.Visible = DListModoTransmision.Visible = false;

            //Se le asignan los valores por defecto a los campos Metodo climax, Tiempo CLD, Ventana BSS y Metodo BSS para el modo Normal
            AsignaValoresDefectoModoNormal();
        }

        IndexListBox1 = ListBox1.SelectedIndex;
    }

    private void CargarRecursosSinAsignar(bool porEmplazamiento=false)
    {
        try
        {
            //Indica si se visualiza la lista de recursos libres con tipos  (true) o con emplazamientos (false)
            bool visualizaRecLibresTipos = (String.Compare("0", DropDownFiltro.SelectedValue) == 0);
            string strSistema = (string)Session["idsistema"];
            StringBuilder strTexto = new StringBuilder();
            int i = 0;
            ServiciosCD40.Tablas[] d=null;

            ListRecursosLibres.Items.Clear();
            ListTiposLibres.Items.Clear();
            ListEmplazamientosLibres.Items.Clear();
            //ListTiposLibres.Visible = (String.Compare("0", DropDownFiltro.SelectedValue) == 0);
            //ListEmplazamientosLibres.Visible = !ListTiposLibres.Visible;

            Label4.Text = (string)GetLocalResourceObject("Label4Resource1.Text");

            if (!porEmplazamiento || string.IsNullOrEmpty(DListEmplazamiento.Text))
            {
                DListEmplazamiento.SelectedIndex = 0;
                d = ServicioCD40.RecursosSinAsignarAEnlaces1(strSistema, 0, null);
            }
            else
                d = ServicioCD40.RecursosSinAsignarAEnlaces1(strSistema, 0, DListEmplazamiento.Text);

            ServiciosCD40.RecursosRadio recRd = new ServiciosCD40.RecursosRadio();

            if (d != null)
            {
                for (i = 0; i < d.Length; i++)
                {
                    ListItem lbItem = new ListItem();
                    ListItem lbTipoItem = new ListItem();
                    string strTipo = string.Empty;
                    string strEmplazamiento = string.Empty;
                    int tipoRec = 0;
                    string strTipoValue = string.Empty;

                    strTexto.Clear();
                    ServiciosCD40.Recursos rec = (ServiciosCD40.Recursos)d[i];

                    // Si el destino es de tipo HF, sólo se pueden mostrar recursos de tipo 0 = AUDIO_RX
                    // o si el tipo no es HF, Si el tipo de recurso es TIPO_DESTINO_AUDIO_HF_TX=3  no se carga en la lista,
                    //independientemente de que esté o no definida la tool RadioHF
                    if ((DListTipo.SelectedValue == DESTINORADIO_TIPOFRECUENCIA_HF && rec.Tipo != 0) ||
                       (DListTipo.SelectedValue != DESTINORADIO_TIPOFRECUENCIA_HF && rec.Tipo == 3))
                    {
                        continue;
                    }
                    else
                    {
                        //Si el tipo de recurso es TIPO_DESTINO_AUDIO_HF_TX=3  no se carga en la lista, independientemente de que esté o no definida la tool RadioHF
                        // Mostrar Tipo recurso radio Audio HF-Tx sólo para NDjamena (Versión=2)
                        //if (((ServiciosCD40.Recursos)d[i]).Tipo == 3) /* Audio-HF-Tx */  //&& UlisesToolsVersion.Tools["RadioHF"] == null)
                        //    continue;
                        strTipoValue = rec.Tipo.ToString();

                        if (rec.Tipo >= 4 && rec.Tipo <= 6)
                            tipoRec = 4;
                        else if (rec.Tipo == 7)
                        {
                            tipoRec = (int)rec.Tipo;
                            ServiciosCD40.HFParams r = new ServiciosCD40.HFParams();

                            r.IdSistema = strSistema;
                            r.IdRecurso = ((ServiciosCD40.Recursos)d[i]).IdRecurso;
                            ServiciosCD40.Tablas[] h = ServicioCD40.ListSelectSQL(r);

                            if (h.Length > 0)
                            {
                                ServiciosCD40.HFParams recHfParam = ((ServiciosCD40.HFParams)h[0]);

                                //Si es audio EE, se obtiene el tipo Receptor, transmisor o transceptor
                                switch (recHfParam.TipoEquipo)
                                {
                                    case 0:
                                        //Audio EE RX
                                        tipoRec = 7;
                                        strTipoValue = "7";
                                        break;
                                    case 1:
                                        //Audio EE TX
                                        tipoRec = 8;
                                        strTipoValue = "8";
                                        break;
                                    case 2:
                                        //Audio EE TxRx
                                        tipoRec = 9;
                                        strTipoValue = "9";
                                        break;
                                }
                            }
                        }
                        else
                            tipoRec = (int)rec.Tipo;

                        var itemTipo= DListTipoRec.Items.FindByValue(tipoRec.ToString());
                        if (itemTipo != null)
                        {
                            strTipo = itemTipo.Text;
                        }

                        //Se obtiene el emplazamiento al que pertenece el recurso
                        recRd.IdSistema = strSistema;
                        recRd.IdRecurso = ((ServiciosCD40.Recursos)d[i]).IdRecurso;

                        ServiciosCD40.Tablas[] l2 = ServicioCD40.ListSelectSQL(recRd);
                        if (null != l2 && l2.Length > 0)
                        {
                            strEmplazamiento = ((ServiciosCD40.RecursosRadio)l2[0]).IdEmplazamiento;
                            ListEmplazamientosLibres.Items.Add(strEmplazamiento);
                        }

                        if (visualizaRecLibresTipos)
                        {
                            //Se visaliza el recurso libre con el tipo
                            //20201014  #4539 strTexto.AppendFormat(FORMATO_LB_RECURSOS_LIBRES, rec.IdRecurso, strTipo);
                            strTexto.AppendFormat(FORMATO_LB_RECURSOS_LIBRES, rec.IdRecurso, strTipo, strEmplazamiento);
                            lbItem.Attributes.Add("title", strEmplazamiento);
                        }
                        else
                        {
                            //Se visaliza el recurso libre con el emplazamiento
                            //20201014  #4539 strTexto.AppendFormat(FORMATO_LB_RECURSOS_LIBRES, rec.IdRecurso, strEmplazamiento);
                            strTexto.AppendFormat(FORMATO_LB_RECURSOS_LIBRES, rec.IdRecurso, strEmplazamiento, strTipo);
                            lbItem.Attributes.Add("title", strTipo);
                        }
                        strTexto.Replace(" ", "\u00A0");
                        lbItem.Text = strTexto.ToString();
                        lbItem.Value = rec.IdRecurso;

                        lbTipoItem.Text = strTipo;
                        lbTipoItem.Value = strTipoValue;

                        ListRecursosLibres.Items.Add(lbItem);
                        ListTiposLibres.Items.Add(lbTipoItem);
                    }
                }
            }
        }
        catch (Exception e)
        {
            logDebugView.Error(string.Format("(DestinosRadio-CargarRecursosSinAsignar): error al cargar los recursos de tipo {0}  ", DListTipo.SelectedValue), e);
        }
    }

    protected void DListEmplazamiento_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DListEmplazamiento.SelectedIndex == 0)
        {
            CargarRecursosSinAsignar();
        }
        else
        {
            CargarRecursosSinAsignar(porEmplazamiento:true);
        }
    }

    private void CargarRecursos()
    {
        try
        {
            ServiciosCD40.RecursosRadio t = new ServiciosCD40.RecursosRadio();
            t.IdSistema = (string)Session["idsistema"];
            t.IdDestino = TxtIdEnlace.Text;
            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            LblErrorMismatchFrequency.Visible = false;
            Frecuencia = string.Empty;

            int tipoRec = 0;
            string empDefecto = null;
            string tVueltaDefecto = "0";

            if (d != null)
            {
                for (int i = 0; i < d.Length; i++)
                {
                    ServiciosCD40.RecursosRadio rec = (ServiciosCD40.RecursosRadio)d[i];
                    ListItem lbItem = new ListItem();
                    string strTipo = String.Empty;

                    if (rec.Tipo >= 4 && rec.Tipo <= 6)
                        tipoRec = 4;
                    else
                        tipoRec = (int) rec.Tipo;

                    ListEmplazamientos.Items.Add(rec.IdEmplazamiento);

                    string tipoRecValue = rec.Tipo.ToString();
                    if (tipoRec == 4 || tipoRec==7) //Tipo M+N ó Audio EE
                    {
                        ServiciosCD40.HFParams r = new ServiciosCD40.HFParams();

                        r.IdSistema = (string)Session["idsistema"];
                        r.IdRecurso = ((ServiciosCD40.RecursosRadio)d[i]).IdRecurso;
                        ServiciosCD40.Tablas[] h = ServicioCD40.ListSelectSQL(r);

                        if (h.Length > 0)
                        {
                            ServiciosCD40.HFParams recHfParam=((ServiciosCD40.HFParams)h[0]);
                            if (tipoRec == 7)
                            {
                                //Si es audio EE, se obtiene el tipo Receptor, transmisor o transceptor
                                switch (recHfParam.TipoEquipo)
                                {
                                    case 0:
                                        //Audio EE RX
                                        tipoRecValue = TIPO_RECURSO_EE_RX;
                                        break;
                                    case 1:
                                        //Audio EE TX
                                        tipoRecValue = TIPO_RECURSO_EE_TX;
                                        tipoRec = 8;
                                        break;
                                    case 2:
                                        //Audio EE TxRx
                                        tipoRecValue = TIPO_RECURSO_EE_RXTX;
                                        tipoRec = 9;
                                        break;
                                }
                            }

                            if (Frecuencia == string.Empty)
                                Frecuencia = ((ServiciosCD40.HFParams)h[0]).Frecuencia;
                            else if (Frecuencia != ((ServiciosCD40.HFParams)h[0]).Frecuencia)
                            {
                                LblErrorMismatchFrequency.Visible = true;
                            }
                        }
                    }

                    var item = DListTipoRec.Items.FindByValue(tipoRec.ToString());
                    if (item != null)
                    {
                        ListTipos.Items.Add(item.Text);
                        ListTipos.Items[ListTipos.Items.Count - 1].Value = tipoRecValue;
                        strTipo = item.Text;
                    }

                    lbItem.Value = rec.IdRecurso;
                    StringBuilder strTexto = new StringBuilder();
                    strTexto.AppendFormat(FORMATO_LB_RECURSOS, rec.IdRecurso, strTipo, rec.IdEmplazamiento);

                    strTexto.Replace(" ", "\u00A0");
                    lbItem.Text = strTexto.ToString();
                    ListRecursos.Items.Add(lbItem);
                }
        //20200520 JOI Errores #4425
                if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_NORMAL && bRedundancia)
                {
                    //Se hacen visibles las opciones de MODO_NORMAL Y REDUNDANCIA
                    Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = true;
                    TextVentanaBSS.Enabled = true;
                    Label9.Visible = DListMetodoClimax.Visible = false;
                    Label44.Visible = TextBoxCLD.Visible = false;
                    LbModoTransmision.Visible = DListModoTransmision.Visible = false;
                }
                //Si el modo es FD y el modo de transmisión ultima recepción
                if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_FD && 
                    DListModoTransmision.SelectedValue == MODO_TRANSMISION_ULTIMA_RECEPCION)
                {
                    for (int r = 0; r < datos.Length; r++)
                    {
                        if (((ServiciosCD40.DestinosRadio)datos[r]).IdDestino == t.IdDestino)
                        {
                            //if (((ServiciosCD40.DestinosRadio)datos[r]).EmplazamientoDefecto != null)
                            if (!string.IsNullOrEmpty(((ServiciosCD40.DestinosRadio)datos[r]).EmplazamientoDefecto))
                            {
                                empDefecto = ((ServiciosCD40.DestinosRadio)datos[r]).EmplazamientoDefecto;
                                if (((ServiciosCD40.DestinosRadio)datos[r]).TiempoVueltaADefecto != null)
                                    tVueltaDefecto = ((ServiciosCD40.DestinosRadio)datos[r]).TiempoVueltaADefecto;
                            }
                            ///VMG 18/02/2019 Para el cálculo del método central.
                            /*if (((ServiciosCD40.DestinosRadio)datos[r]).MetodosBssOfrecidos == 3)
                                LBPorCentral.Visible = DLPorcentajeRSSI.Visible = true;
                            else
                                LBPorCentral.Visible = DLPorcentajeRSSI.Visible = false;
                             */
                        }
                    }
                    ListItem iNoneItem = new ListItem();
                    iNoneItem.Value = "0";
                    iNoneItem.Text = ObtenerTextoRecursoLocal("ListItemEmplDefNinguno.Text", "Ninguno");

                    DListEmplazamientoDefecto.Items.Clear();

                    DListEmplazamientoDefecto.Items.Add(iNoneItem);

                    for (int j = 0; j < ListEmplazamientos.Items.Count; j++)
                    {
                        if (DListEmplazamientoDefecto.Items.FindByText(ListEmplazamientos.Items[j].Text) == null)
                            DListEmplazamientoDefecto.Items.Add(ListEmplazamientos.Items[j].Text);
                    }

                    if (string.IsNullOrEmpty(empDefecto) || DListEmplazamientoDefecto.Items.FindByText(empDefecto) == null)
                    {
                        DListEmplazamientoDefecto.SelectedValue = "0";
                        TxtTiempoVueltaADefecto.Text = "0";
                    }
                    else
                    {
                        DListEmplazamientoDefecto.SelectedValue = empDefecto;
                        TxtTiempoVueltaADefecto.Text = tVueltaDefecto;
                    }
                    Label44.Visible = TextBoxCLD.Visible = false;
                    TextBoxCLD.Text = "0";
                }
                //20200611 JOI #3994
                //Si el modo es FD y el modo de transmisión es CLIMAX
                if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_FD &&
                    DListModoTransmision.SelectedValue == MODO_TRANSMISION_CLIMAX)
                {
                    Label44.Visible = TextBoxCLD.Visible = true;
                    LbModoTransmision.Visible = DListModoTransmision.Visible = true;
                    LbEmplazamientoDefecto.Visible = false;
                    DListEmplazamientoDefecto.Visible = false;
                    LbTiempoVueltaADefecto.Visible = false;
                    TxtTiempoVueltaADefecto.Visible = false;
                }
            }
        }
        catch (Exception e)
        {
            logDebugView.Error("(GrupoFD-CargarRecursos):", e);
        }
    }

    private void AsignaValoresDefectoModoNormal()
    {
        //Se le asignan los valores por defecto a los campos Metodo climax, Tiempo CLD, Ventana BSS y Metodo BSS para el modo Normal
        DListMetodoClimax.SelectedValue = METODO_CLIMAX_RELATIVO;
        TextBoxCLD.Text = "0";
        //20200728 #4546
        TextVentanaBSS.Text = "500";
        DDLMetodosBssOfrecidos.SelectedValue = METODO_BSS_RSSI;
        //20200728 #4546 FIN

        DListModoTransmision.SelectedValue = MODO_TRANSMISION_CLIMAX; //Por defecto, se selecciona Climax auque en el modo normal en BD se almacena NULL

    }

    private void MostrarElemento()
    {
        string sModoDest=string.Empty;
        string sTipo=string.Empty;
        MostrarMenu();
        TxtIdEnlace.ReadOnly = true;
        TxtIdEnlace.Enabled = false;
        DListTipo.Enabled = true;   // CheckExclusividad.Enabled = false;
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        BtModificar.Visible = BtEliminar.Visible = PermisoSegunPerfil && ListBox1.Items.Count > 0;
        ListRecursos.Items.Clear();
        ListEmplazamientos.Items.Clear();
        bool bRecargarRecursosLibres = false;

        LbEmplazamientoDefecto.Visible = DListEmplazamientoDefecto.Visible=false;
        LbTiempoVueltaADefecto.Visible = TxtTiempoVueltaADefecto.Visible=false;

        ListTipos.Items.Clear();

        if (null != datos)
        {
            for (int i = 0; i < datos.Length; i++)
            {
                if (String.Compare((((ServiciosCD40.DestinosRadio)datos[i]).IdDestino), ListBox1.SelectedValue) == 0)
                {
                    BtEliminar_ConfirmButtonExtender.ConfirmText = String.Format((string)GetGlobalResourceObject("Espaniol", "EliminarDestino"), ListBox1.SelectedValue);

                    TxtIdEnlace.Text = ((ServiciosCD40.DestinosRadio)datos[i]).IdDestino;
                    //CheckExclusividad.Checked = ((ServiciosCD40.DestinosRadio)datos[i]).ExclusividadTXRX;
                    // DListTipo.SelectedValue = ((ServiciosCD40.DestinosRadio)datos[i]).TipoFrec.ToString();
                    TblTunedFreq.Visible = ((ServiciosCD40.DestinosRadio)datos[i]).TipoFrec == Convert.ToUInt32(DESTINORADIO_TIPOFRECUENCIA_HF);
                    RFV_TBTunedFrequency.Enabled = RFV_TBTunedFrequency.Visible = TblTunedFreq.Visible;
                    DListPrioridadSIP.SelectedValue = (((ServiciosCD40.DestinosRadio)datos[i]).PrioridadSesionSip).ToString();

                    TextBoxCLD.Text = ((ServiciosCD40.DestinosRadio)datos[i]).CldSupervisionTime.ToString();
                    DDLMetodosBssOfrecidos.SelectedValue = ((ServiciosCD40.DestinosRadio)datos[i]).MetodosBssOfrecidos.ToString();
                    
                    //Se rellena el check 1+1
                    CheckBoxRedundancia.Checked= (!string.IsNullOrEmpty(((ServiciosCD40.DestinosRadio)datos[i]).ConRedundancia) && 
                                                  string.Compare(((ServiciosCD40.DestinosRadio)datos[i]).ConRedundancia,"1")==0) ? true : false;
                    //20200520 JOI #4425
                    bRedundancia = (CheckBoxRedundancia.Checked == true) ? true : false;

                    // El modo destino se recupera de los campos de la BD CnfModoDestino y CnfTipoFrecuencia.
                    //Por compatibilidad con la versión anterior, si estos campos no están informados se obtienen del campo TipoFrec
                    //El campo TipoFrec es el valor que se envía a través del Web Services configuración, por lo que seguirá manteniendo los mismos valores
                    if ( (!string.IsNullOrEmpty(((ServiciosCD40.DestinosRadio)datos[i]).CnfModoDestino)) && (!string.IsNullOrEmpty(((ServiciosCD40.DestinosRadio)datos[i]).CnfTipoFrecuencia)))
                    {
                        sModoDest = ((ServiciosCD40.DestinosRadio)datos[i]).CnfModoDestino;
                        sTipo = ((ServiciosCD40.DestinosRadio)datos[i]).CnfTipoFrecuencia;
                    }
                    else
                    {
                        //Se obtiene el tipo y el modo con el campo tipo frecuencia (TipoFrec)
                        if (((ServiciosCD40.DestinosRadio)datos[i]).TipoFrec > 3)
                        {
                            //Modo no normal
                            sModoDest = (((ServiciosCD40.DestinosRadio)datos[i]).TipoFrec - 3).ToString();
                        }
                        else
                        {
                            //Modo normal y el tipo de frecuencia puede tomar uno de los valores: 0 ->VHF, 1 -> UHF, 2-> HF
                            sModoDest = DESTINORADIO_MODO_NORMAL;
                            sTipo = (((ServiciosCD40.DestinosRadio)datos[i]).TipoFrec).ToString();
                        }
                    }

                    DListModoDestino.SelectedValue = sModoDest;
                    
                    if (!string.IsNullOrEmpty(sTipo))
                    {
                        //Si el tipo del destino a mostrar es distinto del anterior y algun tipo es HF, hay que actualizar la lista de recursos libres
                        if (string.Compare(DListTipo.SelectedValue, sTipo)!=0 && (string.Compare(DListTipo.SelectedValue,DESTINORADIO_TIPOFRECUENCIA_HF)==0 || string.Compare(sTipo,DESTINORADIO_TIPOFRECUENCIA_HF)==0))
                            bRecargarRecursosLibres = true;

                        DListTipo.SelectedValue = sTipo;
                    }

                    if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_FD)
                    {
                        CheckBox1Squelch.Checked = ((ServiciosCD40.DestinosRadio)datos[i]).AudioPrimerSqBss;
                        CheckedSincro.Checked = ((ServiciosCD40.DestinosRadio)datos[i]).SincronizaGrupoClimax;
                        TextVentanaBSS.Text = (((ServiciosCD40.DestinosRadio)datos[i]).VentanaSeleccionBss).ToString();
                        DListMetodoClimax.SelectedValue = (((ServiciosCD40.DestinosRadio)datos[i]).MetodoCalculoClimax).ToString();

                        //Se hacen visibles las opciones de FD
                        Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible=true;
                        Label9.Visible = DListMetodoClimax.Visible = TextVentanaBSS.Enabled = true;


                        //Se hace visible el campo Modo de Transmisión
                        LbModoTransmision.Visible = DListModoTransmision.Visible = true;
                        DListModoTransmision.SelectedValue=((ServiciosCD40.DestinosRadio)datos[i]).ModoTransmision;

                        //Solo mostramos si en FD y Modo Ultima Transmisión ;)
                        if (DListModoTransmision.SelectedValue == MODO_TRANSMISION_ULTIMA_RECEPCION)
                        {
                            LbEmplazamientoDefecto.Visible = true;
                            DListEmplazamientoDefecto.Visible = true;
                            LbTiempoVueltaADefecto.Visible = true;
                            TxtTiempoVueltaADefecto.Visible = true;
                            Label9.Visible = DListMetodoClimax.Visible = false;
                        }
                        //20200611 JOI error #3994
                        //Solo mostramos si en FD y Modo CLIMAX)
                        if (DListModoTransmision.SelectedValue == MODO_TRANSMISION_CLIMAX)
                        {
                            Label44.Visible = TextBoxCLD.Visible = true;
                            Label9.Visible = DListMetodoClimax.Visible = true;
                        }
                    }
                    //20200520 JOI Errores #4425
                    else if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_NORMAL && bRedundancia)
                    {
                        //Se hacen visibles las opciones de MODO_NORMAL Y REDUNDANCIA
                        Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = true;
                        TextVentanaBSS.Enabled = true;
                        Label9.Visible = DListMetodoClimax.Visible = false;
                        Label44.Visible = TextBoxCLD.Visible = false;
                        LbModoTransmision.Visible = DListModoTransmision.Visible = false;
                        TextVentanaBSS.Text = (((ServiciosCD40.DestinosRadio)datos[i]).VentanaSeleccionBss).ToString();
                    }
                    else
                    {
                        Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible =  false;
                        CheckBox1Squelch.Visible = Label9.Visible = DListMetodoClimax.Visible = TextVentanaBSS.Enabled =  false;
                        Label44.Visible = TextBoxCLD.Visible = false;
                        LbModoTransmision.Visible = DListModoTransmision.Visible = false;
                        AsignaValoresDefectoModoNormal();
                    }

                    //CheckFrecNoDesasignable.Checked = ((ServiciosCD40.DestinosRadio)datos[i]).ExclusividadTXRX;//CheckFrecNoDesasignable

                    if (TblTunedFreq.Visible)
                    {
                        TextBox tb = (TextBox)TblTunedFreq.FindControl("TbTunedFrequency");
                        if (tb != null)
                        {
                            tb.Text = ((ServiciosCD40.DestinosRadio)datos[i]).Frecuencia.ToString();
                        }
                    }
                    CargarRecursos();

                    if (bRecargarRecursosLibres)
                        CargarRecursosSinAsignar();

                    return;
                }

            }
        }
    }
    private void CargaEmplazamientos()
    {
        try
        {
            ServiciosCD40.Emplazamientos emp = new ServiciosCD40.Emplazamientos();

            emp.IdSistema = (string)Session["IdSistema"];
            DListEmplazamiento.DataSource = ServicioCD40.DataSetSelectSQL(emp);
            DListEmplazamiento.DataTextField = "IdEmplazamiento";
            DListEmplazamiento.DataBind();
        }
        catch (Exception ex)
        {
            logDebugView.Error("(GrupoFD-CargaEmplazamientos): ", ex);
        }
    }
   


    private void MostrarMenu()
    {
        LimpiarMenu();
        
        Label4.Visible = true;
        
        //TxtIdEnlace.Visible = true;
        //DListTipo.Visible = true;
        ListRecursosLibres.Visible = true;
        //ListTiposLibres.Visible = true;
        LblFiltroEmplazamiento.Visible = DListEmplazamiento.Visible = DropDownFiltro.Visible = true;

        //CheckExclusividad.Visible = true;
    }

    private void EsconderMenu()
    {
        Label4.Visible = false;
        //DListTipo.Visible = false;
        //IButAsignar.Visible = false;
        //IButQuitar.Visible = false;
        ListRecursosLibres.Visible = false;
        //ListTiposLibres.Visible = false;
        //Label4.Visible = false;
        //ListRecursosLibres.Visible = false;
        //ListEmplazamientosLibres.Visible = false;
        //DListTipoRec.Visible = false;


        LblFiltroEmplazamiento.Visible = DListEmplazamiento.Visible = DropDownFiltro.Visible = false;

        ListRecursos.Items.Clear();
        ListEmplazamientos.Items.Clear();
        ListRecursosLibres.Items.Clear();
        ListTiposLibres.Items.Clear();
        ListEmplazamientosLibres.Items.Clear();
        ListTipos.Items.Clear();
        //TxtIdEnlace.Visible = false;
        LimpiarMenu();
        BtAceptar.Visible = false;
        BtCancelar.Visible = false;
        BtModificar.Visible = false;
        RequiredFieldIdentificador.Enabled = RequiredFieldIdentificador.Visible = false;
        //RequiredFieldValidator1.Enabled = RequiredFieldValidator1.Visible = false;
        errores.Visible = false;
        //CheckExclusividad.Visible = false;
        TblTunedFreq.Visible = false;
        RFV_TBTunedFrequency.Enabled = RFV_TBTunedFrequency.Visible = false;
        valCustom.Enabled = valCustom.Visible=false;
    }

    private void LimpiarMenu()
    {
        TxtIdEnlace.Text = "";
        TextBoxCLD.Text = "1";
        DListModoDestino.SelectedIndex = 0;
        DDLMetodosBssOfrecidos.SelectedIndex = 0;
        DListPrioridadSIP.SelectedIndex = 0;
        DListMetodoClimax.SelectedIndex = 0;
        CheckedSincro.Checked = false;
        CheckBox1Squelch.Checked = true;
    }

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ListBox1.SelectedIndex >= 0)
        {
            BtEliminar.Visible = PermisoSegunPerfil;
            DListEmplazamiento.SelectedIndex = 0;
            MostrarElemento();
        }
    }

    private ServiciosCD40.Tablas[] DameDatos()
    {
        try
        {
            ServiciosCD40.DestinosRadio t = new ServiciosCD40.DestinosRadio();
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            s = config.AppSettings.Settings["Sistema"];

            t.IdSistema = s.Value;
            Session["idsistema"] = s.Value;

            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(t);
            datos = d;
            return d;
        }
        catch (Exception e)
        {
            logDebugView.Error("(GrupoFD-DameDatos):", e);
        }
        return null;
    }

    private void MuestraDatos(ServiciosCD40.Tablas[] nu)
    {
        // LError.Visible = false;

        ListBox1.Items.Clear();
        if (nu != null)
            for (int i = 0; i < nu.Length; i++)
            {
                ListBox1.Items.Add(((ServiciosCD40.DestinosRadio)nu[i]).IdDestino);
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

            DListEmplazamiento.SelectedIndex = 0;
            MostrarElemento();
            CargarRecursosSinAsignar();
        }
        else
        {
            BtModificar.Visible = BtEliminar.Visible = false;
        }
    }


    protected override void AceptarCambios()
    {
        base.AceptarCambios();
    }

    protected override void CancelarCambios()
    {
        EsconderMenu();

        Panel1.Enabled = false;
        DListEmplazamiento.Enabled = false;

        BtNuevo.Visible = PermisoSegunPerfil;
        ListBox1.Enabled = true;

        MuestraDatos(DameDatos());
      
    }

    private bool GuardarCambios()
    {
        try
        {
            bool destinoAnadido = false;
            String strMsgAux = String.Empty;
            String strIdDestino = String.Empty;
            String strValorTipoDestino = String.Empty;
            String strTipo = string.Empty;
            String strSistema = (string)Session["idsistema"];
            bool destinoConRedundancia = false;
            String modoDestino = DListModoDestino.SelectedValue;
            bool bDestinoFmtFrecuencia = false; //Indica si el identificador del destino debe seguir el formato de las frecuencias
            bool bDestinoFmtFrecuenciaHFMN = false; //Indica si el identificador del destino debe seguir el formato de las frecuencias
            Dictionary<string, CRecursoRedundancia> mapaRecursosRed = new Dictionary<string, CRecursoRedundancia>();

            //Se inicializa al valor configurado en el parámetro del fichero de configuración
            bDestinoFmtFrecuencia = bCnfIdFormatoFrecuencia;
            
            //Si el destino se configura con redundancia (1+1)
            destinoConRedundancia = (CheckBoxRedundancia.Checked == true) ? true : false;

            strValorTipoDestino = DListTipo.SelectedValue;
            strIdDestino = TxtIdEnlace.Text;

            //Si se está dando de alta un nuevo destino, se comprueba si el nombre del destino es único
            if (TxtIdEnlace.Enabled && ListBox1.Items.FindByText(TxtIdEnlace.Text) != null)
            {
                cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "Nombredestinoutilizado"), "aceptparam");
                return false;
            }

            //En el modo destino FD,el tipo de frecuencia no puede ser HF
            if (destinoConRedundancia && strValorTipoDestino == DESTINORADIO_TIPOFRECUENCIA_HF)
            {
                cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorAsignacionFrecuenciaHFConRedundancia"), "aceptparam");
                return false;
            }

            if (-1 != DListTipo.SelectedIndex)
            {
                strTipo = DListTipo.Items[DListTipo.SelectedIndex].Text;
            }

            //20200728 #4538
            if (destinoConRedundancia  && string.Compare(DDLMetodosBssOfrecidos.SelectedValue, METODO_BSS_NINGUNO) == 0)
            {
                cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorConfiguracion1_mas_1_BSS"), "aceptparam");
                return false;
            }

            #region Comprobación del formato del Id del destino

            if (false == bCnfIdFormatoFrecuencia)
            {
                //Si el destino radio es HF o tiene asignado algún recurso de tipo M+N debe cumplir el formato de las frecuencias
                // independientemente de que el parámetro bIdFormatoFrecuencia indique que el identificador no debe seguir el formato de las frecuencias
                if (string.Compare(strValorTipoDestino, DESTINORADIO_TIPOFRECUENCIA_HF) == 0)
                    bDestinoFmtFrecuenciaHFMN = true;
                else
                {
                    for (int i = 0; i < ListTipos.Items.Count && !bDestinoFmtFrecuencia; i++)
                    {
                        if ((String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_M_N_TX) == 0) ||
                            (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_M_N_RX) == 0) ||
                            (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_M_N_RXTX) == 0))
                        {
                            bDestinoFmtFrecuenciaHFMN = true;
                        }
                    }
                }
            }

            if (TxtIdEnlace.Enabled)
            {
                //Si se ha añadido un nuevo destino se verifica la máscara del identificador
                if (false == bDestinoFmtFrecuencia)
                {
                    if (bDestinoFmtFrecuenciaHFMN)//Caso particular de las M+N o HF. Solo mensaje
                    {
                        //20200608 JOI ERROR #4076 // Solo se comprobaba sí VHF
                        if (!Regex.IsMatch(strIdDestino, @"^(1)(1|2|3)([0-9]{1})\.([0-9]{2})(0|5)$") &&  //VHF
                            !Regex.IsMatch(strIdDestino, @"^(2|3|4)([0-9]{2})\.([0-9]{2})(0|5)$")    &&  //UHF
                            !Regex.IsMatch(strIdDestino, @"^(0)(0|1|2)([0-9])\.([0-9]{3})$"))            //HF
                        {
                            // El identificador no tiene el formato correcto    
                            strMsgAux = string.Format((string)GetGlobalResourceObject("Espaniol", "FormatoIdentificadoDestinoRadioIncorrectoHFMN"));
                            cMsg.confirm(strMsgAux, "aceptparam");
                            return false;
                        }
                    }
                    //Como máximo el identificador puede tener 10 caracteres
                    if (!Regex.IsMatch(TxtIdEnlace.Text, @"^[\w-._]{1,10}$"))
                    {
                        // El identificador no tiene el formato correcto 
                        strMsgAux = (string)GetLocalResourceObject("RequiredFieldValidator2Resource1.ErrorMessage");
                        cMsg.confirm(strMsgAux, "aceptparam");
                        return false;
                    }
                }
                else
                {
                    //Se comprueba que el identificador del destino radio tiene el siguiente patrón en función del tipo de destino
                    switch (strValorTipoDestino)
                    {
                        case DESTINORADIO_TIPOFRECUENCIA_VHF:
                            //si tipo=VHF  /^(1)(1|2|3)([0-9]{1})\.([0-9])([0-9])(0|5)$/;   /** 118.000 137.000 */
                            if (!Regex.IsMatch(strIdDestino, @"^(1)(1|2|3)([0-9]{1})\.([0-9]{2})(0|5)$"))
                            {
                                // El identificador no tiene el formato correcto    
                                strMsgAux = string.Format((string)GetGlobalResourceObject("Espaniol", "FormatoIdentificadoDestinoRadioIncorrecto"), strTipo, "118.000", "137.000");
                                cMsg.confirm(strMsgAux, "aceptparam");
                                return false;
                            }

                            break;
                        case DESTINORADIO_TIPOFRECUENCIA_UHF:
                            //Si tipo=UHF  /^(2|3|4)([0-9]{2})\.([0-9])([0-9])(0|5)$/;        /** 225.000 400.000 */
                            if (!Regex.IsMatch(strIdDestino, @"^(2|3|4)([0-9]{2})\.([0-9]{2})(0|5)$"))
                            {
                                // El identificador no tiene el formato correcto                       
                                strMsgAux = string.Format((string)GetGlobalResourceObject("Espaniol", "FormatoIdentificadoDestinoRadioIncorrecto"), strTipo, "225.000", "400.000");
                                cMsg.confirm(strMsgAux, "aceptparam");
                                return false;
                            }
                            break;
                        case DESTINORADIO_TIPOFRECUENCIA_HF:
                            //Si tipo=HF     el identificador debe tomar un valor en el rango [002.800, 022.000]
                            if (!Regex.IsMatch(strIdDestino, @"^(0)(0|1|2)([0-9])\.([0-9]{3})$"))
                            {
                                // El identificador no tiene el formato correcto
                                strMsgAux = string.Format((string)GetGlobalResourceObject("Espaniol", "FormatoIdentificadoDestinoRadioIncorrecto"), strTipo, "002.800", "022.000");
                                cMsg.confirm(strMsgAux, "aceptparam");
                                return false;
                            }
                            else
                            {
                                if (string.Compare(strIdDestino, "002.800") < 0 || string.Compare(strIdDestino, "022.000") > 0)
                                {
                                    // El identificador no tiene el formato correcto
                                    strMsgAux = string.Format((string)GetGlobalResourceObject("Espaniol", "FormatoIdentificadoDestinoRadioIncorrecto"), strTipo, "002.800", "022.000");
                                    cMsg.confirm(strMsgAux, "aceptparam");
                                    return false;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                //Si se está modificando, bCnfIdFormatoFrecuencia=false y el Id del destino no cumple el formato de las frecuencias, no se permite añadir recursos de tipo M+N
                if (!bCnfIdFormatoFrecuencia && bDestinoFmtFrecuencia)
                {
                    //Se comprueba que el identificador del destino radio tiene el siguiente patrón en función del tipo de destino
                    // 
                    if ((string.Compare(strValorTipoDestino, DESTINORADIO_TIPOFRECUENCIA_VHF) == 0 && !Regex.IsMatch(strIdDestino, @"^(1)(1|2|3)([0-9]{1})\.([0-9]{2})(0|5)$")) ||
                        (string.Compare(strValorTipoDestino, DESTINORADIO_TIPOFRECUENCIA_UHF) == 0 && !Regex.IsMatch(strIdDestino, @"^(2|3|4)([0-9]{2})\.([0-9]{2})(0|5)$")) ||
                        (string.Compare(strValorTipoDestino, DESTINORADIO_TIPOFRECUENCIA_HF) == 0 && (!Regex.IsMatch(strIdDestino, @"^(0)(0|1|2)([0-9])\.([0-9]{3})$") ||
                        (string.Compare(strIdDestino, "002.800") < 0 || string.Compare(strIdDestino, "022.000") > 0)))
                       )
                    {
                        //El identificador no cumple el formato de frecuencia, por lo que no se le pueden añadir recursos de tipo M+N
                        strMsgAux = string.Format((string)GetGlobalResourceObject("Espaniol", "DestinoRadioSinRecursosMN"), strIdDestino, strTipo);
                        cMsg.confirm(strMsgAux, "aceptparam");
                        return false;
                    }
                }
                //Modificando y es HF con M+N Aunque aquí nunca llega porque no se deja modificar el id. Lo dejamos por si acaso
                if (bDestinoFmtFrecuenciaHFMN && !Regex.IsMatch(strIdDestino, @"^(1)(1|2|3)([0-9]{1})\.([0-9]{2})(0|5)$"))
                {                 
                    // El identificador no tiene el formato correcto    
                    strMsgAux = string.Format((string)GetGlobalResourceObject("Espaniol", "FormatoIdentificadoDestinoRadioIncorrectoHFMN"));
                    cMsg.confirm(strMsgAux, "aceptparam");
                    return false;
                }
            }

            #endregion

            ServiciosCD40.DestinosRadio n = new ServiciosCD40.DestinosRadio();
            n.IdSistema = strSistema;
            if (TxtIdEnlace.Enabled) //Nuevo Enlace 
                n.IdDestino = TxtIdEnlace.Text;
            else
                n.IdDestino = ListBox1.SelectedValue;

            NewItem = n.IdDestino;
            n.PrioridadSesionSip = Convert.ToInt32(DListPrioridadSIP.SelectedValue);

            //Si el tipo no es FD, el modo de transmisión se almacena con valor null
            n.ModoTransmision = null;

            //Independientemente del modo, siempre se guardan los valores Sincr.Recepcion grupo=false y Audio en primer Squelch=true
            n.SincronizaGrupoClimax = false;
            n.AudioPrimerSqBss = true;

            //switch (DListModoDestino.SelectedValue)
            switch (modoDestino)
            {
                case DESTINORADIO_MODO_NORMAL:
                    n.TipoFrec = Convert.ToUInt32(DListTipo.SelectedValue);
                    break;

                case DESTINORADIO_MODO_1_mas_1:
                case DESTINORADIO_MODO_EM:
                    n.TipoFrec = Convert.ToUInt32(DListModoDestino.SelectedValue) + 3;
                    break;
                case DESTINORADIO_MODO_FD:
                    n.TipoFrec = Convert.ToUInt32(DListModoDestino.SelectedValue) + 3;
                    n.ModoTransmision = DListModoTransmision.SelectedValue;

                    //Si el modo es FD y el modo de transmisión es  ultima recepción, si se ha seleccionado algún emplazamiento por defecto
                    //se almacenan los valores EmplazamientoDefecto y tiempoVueltaaDefecto
                    if (n.ModoTransmision == MODO_TRANSMISION_ULTIMA_RECEPCION && string.Compare(DListEmplazamientoDefecto.SelectedValue,"0")!=0)
                    {
                        n.EmplazamientoDefecto = DListEmplazamientoDefecto.SelectedValue;

                        //Se comprueba que el valor TiempoVueltaADefecto es correcto
                        int iTiempoVueltaDefecto=0;

                        //El valor puede ser 0 o estar en el rango 10..120
                        if (Int32.TryParse(TxtTiempoVueltaADefecto.Text, out iTiempoVueltaDefecto) && 
                            (iTiempoVueltaDefecto==0 ||(iTiempoVueltaDefecto>=10 && iTiempoVueltaDefecto<=120)))
                        {
                            n.TiempoVueltaADefecto = TxtTiempoVueltaADefecto.Text;
                        }
                        else
                        {
                            //El valor TiempoVueltaADefecto no es correcto
                            strMsgAux = string.Format((string)GetGlobalResourceObject("Espaniol", "RangTiempoVueltaEmpDefecto"), strIdDestino, strTipo);
                            cMsg.confirm(strMsgAux, "aceptparam");
                            return false;
                        }
                    }
                    else
                    {
                        //En caso contrario, null
                        n.EmplazamientoDefecto = null;
                        n.TiempoVueltaADefecto = null;
                    }

                    break;
                default:
                    break;
                
            }

            n.TipoDestino = 0;//Destino Radio

 
            //Se almacena en BD el modo y el tipo frecuencia 
            n.CnfModoDestino = DListModoDestino.SelectedValue;
            n.CnfTipoFrecuencia = DListTipo.SelectedValue;

            //MVO: Si el destino es 1+1, está configurado con redundancia
            n.ConRedundancia = (destinoConRedundancia) ? "1" : "0"; 
            
            //n.ExclusividadTXRX = false; // CheckExclusividad.Checked;
            //n.ExclusividadTXRX = CheckFrecNoDesasignable.Checked;

            //Si el modo es FD, el método climax se almacena con los valores seleccionados por el usuario.
            //En cualquier otro caso, previamente se han asignado los valores por defecto: Ventana BSS = 500 Metodo Climax = Relativo Tiempo CLD=0 y Metodo BSS="0" (Ninguno)
            n.MetodoCalculoClimax = Convert.ToInt32(DListMetodoClimax.SelectedValue);
            n.VentanaSeleccionBss = Convert.ToInt32(TextVentanaBSS.Text);
            n.CldSupervisionTime = Convert.ToInt32(TextBoxCLD.Text);
            n.MetodosBssOfrecidos = Convert.ToInt32(DDLMetodosBssOfrecidos.SelectedValue);

            //En el tipo frecuencia HF, la frecuencia sintonizada es obligatoria
            if (DESTINORADIO_TIPOFRECUENCIA_HF == DListTipo.SelectedValue && TblTunedFreq.Visible)
            {
                if (!string.IsNullOrWhiteSpace(TBTunedFrequency.Text))
                {
                    //Se comprueba si la frecuencia sintonizada en Hz tiene el formato correcto
                    String strAux = String.Empty;
                    strAux = TBTunedFrequency.Text.Replace(".", String.Empty);
                    strAux = strAux.Replace(",", String.Empty);

                    try
                    {
                        n.Frecuencia = Convert.ToInt32(strAux);
                    }
                    catch (Exception)
                    {
                        //El formato es incorrecto
                        strMsgAux = string.Format((string)GetLocalResourceObject("FrecuenciaSintonizadaIncorrecta.Text"));
                        valCustom.ErrorMessage = strMsgAux;
                        valCustom.Enabled = true;
                        valCustom.Visible = true;
                        valCustom.IsValid = false;
                        return false;
                    }
                }
            }
            //20201009  #4608
            //Verificación BSS. Equipamiento si destino FS y 1+1 y FD BSS metodo igual BSS (no NUCLEO)
            if (!VerificaBSSDestinoRecurso(destinoConRedundancia))
                return false;

            //Se verifican las restricciones de los recursos en función del modo del destino
            // y de si el recurso está configurado como 1+1
            if (!destinoConRedundancia)
            {
                if (RestriccionesRecursosporModoDestino() == false)
                    return false;
            }
            else
            {
                if (RestriccionesRecursosporModoDestinoConRedundancia(modoDestino, strValorTipoDestino, ref mapaRecursosRed, ref strMsgAux) == false)
                {
                    cMsg.confirm(strMsgAux, "aceptparam");
                    return false;
                }
            }

            ServiciosCD40.RecursosRadio r = new ServiciosCD40.RecursosRadio();
            r.IdSistema = strSistema;
            r.TipoDestino = 0;//externo
            r.IdDestino = TxtIdEnlace.Text;

            ServiciosCD40.Tablas[] tabRecRadio = new ServiciosCD40.Tablas[ListRecursos.Items.Count];
            for (int i = 0; i < ListRecursos.Items.Count; i++)
            {
                ServiciosCD40.RecursosRadio r1 = new ServiciosCD40.RecursosRadio();
                r1.IdSistema = strSistema;
                r1.TipoDestino = 0;//externo
                r1.IdDestino = TxtIdEnlace.Text;
                r1.IdRecurso = ListRecursos.Items[i].Value;
                if (mapaRecursosRed.ContainsKey(r1.IdRecurso))
                {
                    r1.RedundanciaRol = mapaRecursosRed[r1.IdRecurso].Rol;
                    r1.RedundanciaIdPareja = mapaRecursosRed[r1.IdRecurso].IdPareja;
                }
                tabRecRadio[i] = (ServiciosCD40.Tablas)r1;
            }

            if (TxtIdEnlace.Enabled) //Nuevo Enlace
            {
                /*
                for (int i = 0; i < ListBox1.Items.Count; i++)
                {
                    if (String.Compare(ListBox1.Items[i].Text, TxtIdEnlace.Text) == 0)
                    {
                        cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "Nombredestinoutilizado"), "aceptparam");
                        return false;
                    }
                }
                 */

                destinoAnadido = ServicioCD40.AnadeDestinoRadio(n, r, tabRecRadio);
                if (destinoAnadido)
                {
                    if (ListRecursos.Items.Count > 0)
                    {
                        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                        {
                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();

                            foreach (ListItem idrec in ListRecursos.Items)
                            {
                                //Obtener el emplazamiento del recurso
                                string empl = "";
                                ServiciosCD40.RecursosRadio emp = new ServiciosCD40.RecursosRadio();

                                emp.IdSistema = strSistema;
                                emp.IdRecurso = idrec.Value;
                                ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(emp);
                                empl = ((ServiciosCD40.RecursosRadio)d[0]).IdEmplazamiento;
                                sincro.AltaFrecuencia(empl, n.IdDestino, 0, idrec.Text);
                            }
                        }
                    }

                    ActualizaWebPadre(true);
                }
            }
            else
            {
                IndexListBox1 = ListBox1.SelectedIndex;
                destinoAnadido = ServicioCD40.ModificaDestinoRadio(n, r, tabRecRadio);

                if (destinoAnadido)
                {
                    ServicioCD40.BeginRegeneraSectorizaciones(strSistema, true, true, true, CallbackCompletado, null); 
                    
                    if (ListRecursos.Items.Count > 0)
                    {
                        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                        if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                        {
                            SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                            sincro.BajaFrecuencia(n.IdDestino, false);
                            foreach (ListItem idrec in ListRecursos.Items)
                            {
                                //Obtener el emplazamiento del recurso
                                string empl = "";
                                ServiciosCD40.RecursosRadio emp = new ServiciosCD40.RecursosRadio();

                                emp.IdSistema = strSistema;
                                emp.IdRecurso = idrec.Value;
                                ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(emp);
                                empl = ((ServiciosCD40.RecursosRadio)d[0]).IdEmplazamiento;
                                sincro.AltaFrecuencia(empl, n.IdDestino, 0, idrec.Value);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            logDebugView.Error("(GrupoFD-GuardarCambios):", e);
        }

        EsconderMenu();
        Panel1.Enabled = false;
        DListEmplazamiento.Enabled = false;
        ListBox1.Enabled = true;
        BtNuevo.Visible = PermisoSegunPerfil;
        BtEliminar.Visible = false;

        MuestraDatos(DameDatos());

        return true;
    }

    protected void BtCancelar_Click(object sender, EventArgs e)
    {
        CancelarCambios();
    }

    protected void BtAceptar_Click(object sender, EventArgs e)
    {
        string strSistema = (string)Session["idsistema"];

        //Si se está insertando un nuevo destino radio
        if (TxtIdEnlace.Enabled && bIdentificadorAsignado(strSistema, TxtIdEnlace.Text))
        {
            //Existe otro destino radio o de telefonia con el mismo identificador
            cMsg.alert((string)GetLocalResourceObject("DestinoExiste.Text"));
            return;
        }
        else if (ListRecursos.Items.Count > 0)
        {
            if (GuardarCambios() == true)
                cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AceptarCambios"), "aceptparam");
        }

        else
            cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "AvisoDestinoSinRecurso"), "aceptparam");
    }

    protected void BtModificar_Click(object sender, EventArgs e)
    {
        IndexListBox1 = ListBox1.SelectedIndex;

        //RequiredFieldValidator1.Enabled = RequiredFieldValidator1.Visible = true;       

        DListEmplazamiento.Enabled = true;
        DListPrioridadSIP.Enabled = true;

        if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_FD)
        {
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = Label9.Visible = DListMetodoClimax.Visible = TextVentanaBSS.Enabled = true;
            Label44.Visible = TextBoxCLD.Visible = true;
            //Se hace visible el campo Modo de Transmisión
            LbModoTransmision.Visible = DListModoTransmision.Visible = true;
            //20200611 JOI #3994
            //Si el modo es FD y el modo de transmisión es CLIMAX
            if (DListModoTransmision.SelectedValue == MODO_TRANSMISION_CLIMAX)
            {
                Label44.Visible = TextBoxCLD.Visible = true;
                LbEmplazamientoDefecto.Visible = false;
                DListEmplazamientoDefecto.Visible = false;
                LbTiempoVueltaADefecto.Visible = false;
                TxtTiempoVueltaADefecto.Visible = false;
            }
            else
            {
                Label44.Visible = TextBoxCLD.Visible = false;
                LbEmplazamientoDefecto.Visible = true;
                DListEmplazamientoDefecto.Visible = true;
                LbTiempoVueltaADefecto.Visible = true;
                TxtTiempoVueltaADefecto.Visible = true;
                Label9.Visible = DListMetodoClimax.Visible = false;
            }
        }
        //20200520 JOI Errores #4425
        else if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_NORMAL && bRedundancia)
        {
            //Se hacen visibles las opciones de MODO_NORMAL Y REDUNDANCIA
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = true;
            TextVentanaBSS.Enabled = true;
            Label9.Visible = DListMetodoClimax.Visible = false;
            Label44.Visible = TextBoxCLD.Visible = false;
            LbModoTransmision.Visible = DListModoTransmision.Visible = false;
        }
        else
        {
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = Label9.Visible = DListMetodoClimax.Visible = TextVentanaBSS.Enabled = false;
            Label44.Visible = TextBoxCLD.Visible = false;
            LbModoTransmision.Visible = DListModoTransmision.Visible = false;
        }

        //MVO: el tipo de frecuencia no se puede modificar, porque el formato del identificador depende de la frecuencia y este no se puede cambiar.
        DListTipo.Enabled = false;

        Panel1.Enabled = true;
        errores.Visible = true;
       //DListTipo.Enabled = true;   // CheckExclusividad.Enabled = true;
        ListBox1.Enabled = false;
        BtNuevo.Visible = false;
        BtEliminar.Visible = false;
        BtModificar.Visible = false;
        BtAceptar.Visible = true;
        BtCancelar.Visible = true;
        ListRecursosLibres.Visible = true;

        //ListTiposLibres.Visible = (String.Compare("0", DropDownFiltro.SelectedValue) == 0);
        //ListEmplazamientosLibres.Visible = !ListTiposLibres.Visible;
      
        //TxtIdEnlace.ReadOnly = false;
        //TxtIdEnlace.Enabled = true;

        //DListTipoRec.Visible = true;
        ListRecursos.Enabled = true;
        Label4.Visible = true;

        LblFiltroEmplazamiento.Visible = DListEmplazamiento.Visible = DropDownFiltro.Visible = true;

        CargarRecursosSinAsignar();
        IButQuitar.Visible = true;
        IButAsignar.Visible = true;
    }

    protected void BtEliminar_Click(object sender, EventArgs e)
    {
        EsconderMenu();

        if (ListBox1.SelectedValue != "")
        {
            IndexListBox1 = ListBox1.SelectedIndex;
            Session["elemento"] = ListBox1.SelectedValue;
            EliminarElemento(false);
        }
    }

    private void EliminarElemento(bool forced)
    {
        string strIdSistema = (string)Session["idsistema"];

        if (!string.IsNullOrEmpty(strIdSistema))
        {
            // Liberar el destino del recurso
            ServiciosCD40.RecursosRadio rD = new ServiciosCD40.RecursosRadio();
            rD.IdSistema = strIdSistema;
            rD.IdDestino = ListBox1.SelectedValue;
            rD.TipoDestino = 0;

            ServiciosCD40.Destinos n = new ServiciosCD40.Destinos();
            n.IdSistema = strIdSistema;
            n.IdDestino = ListBox1.SelectedValue;
            n.TipoDestino = 0;

            if (forced || !DestinoAsignadoATft(ListBox1.SelectedValue))
            {
                if (ServicioCD40.EliminaDestino(n, rD))
                {
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                    KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];
                    if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                    {
                        SincronizaCD30.SincronizaCD30 sincro = new SincronizaCD30.SincronizaCD30();
                        sincro.BajaFrecuencia(n.IdDestino, true);
                    }

                    MuestraDatos(DameDatos());

                    // Regenerar todas las sectorizaciones excepto la activa porque puede que este destino 
                    // estuviera presente en alguna sectorización
                    ServicioCD40.BeginRegeneraSectorizaciones(strIdSistema, true, true, true, CallbackCompletado, null);
                }
            }
            else
            {
                cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "PanelesConDestinoAsignado"), "SoloEliminaDeTFT");
            }
        }
    }

    protected bool VerificaBSSDestinoRecurso(bool destinoConRedundancia)
    {
        int i = 0;
        //20201009  #4608 Se controla BSS para todos los destinos 
        if ((DListModoDestino.SelectedValue == DESTINORADIO_MODO_NORMAL && destinoConRedundancia) ||
            DListModoDestino.SelectedValue == DESTINORADIO_MODO_FD ||
             DListModoDestino.SelectedValue == DESTINORADIO_MODO_1_mas_1)
        {
            ServiciosCD40.RecursosRadio rec = new ServiciosCD40.RecursosRadio();
            rec.IdSistema = (string)Session["IdSistema"];
            string strMsgAux = string.Empty;
            for (i = 0; i < ListTipos.Items.Count; i++)
            {
                rec.IdRecurso = ListRecursos.Items[i].Value;
                ServiciosCD40.Tablas[] l_Bss = ServicioCD40.ListSelectSQL(rec);
                //MVO Si el recurso Radio es de tipo 0 (Audio RX ) ó 2 (Audio RX TX) y tiene configurada una pasarela se requiere tener configurada la propiedad BSS
                if ((((ServiciosCD40.RecursosRadio)l_Bss[0]).Tipo == 0 || ((ServiciosCD40.RecursosRadio)l_Bss[0]).Tipo == 2) && // Rx o RxTx
                      (!(String.IsNullOrEmpty(((ServiciosCD40.RecursosRadio)l_Bss[0]).idTIFX)) && ((ServiciosCD40.RecursosRadio)l_Bss[0]).BSS == false)
                    )
                {
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "NopropiedadBSSFS1_1") + "**" + rec.IdRecurso + "**", "aceptparam");
                    return false;
                }
                // Si el destino es FD y tiene asignado algún recurso radio de tipo RX o RXTX
                // que no está asignado a una pasarela el metodo BSS preferido no puede ser RSSI+NUCLEO
                if (string.Compare(DDLMetodosBssOfrecidos.SelectedValue, METODO_BSS_RSSI_NUCLEO) == 0 && DListModoDestino.SelectedValue == DESTINORADIO_MODO_FD) //20201009  #4608
                {
                    switch (ListTipos.Items[i].Value)
                    {
                        case TIPO_RECURSO_RX:
                        case TIPO_RECURSO_M_N_RX:
                        case TIPO_RECURSO_RXTX:
                        case TIPO_RECURSO_M_N_RXTX:
                        case TIPO_RECURSO_EE_RX:
                        case TIPO_RECURSO_EE_RXTX:
                            //Si el recurso no está asignado a una pasarela
                            if (String.IsNullOrEmpty(((ServiciosCD40.RecursosRadio)l_Bss[0]).idTIFX))
                            {
                                string strMsg = string.Empty;

                                if (GetGlobalResourceObject("Espaniol", "MsgDestinoFDConMetodoBSSNoPermitido") != null)
                                {
                                    strMsg = GetGlobalResourceObject("Espaniol", "MsgDestinoFDConMetodoBSSNoPermitido").ToString();
                                }
                                else
                                    strMsg = "En el Modo destino 'FD', no se puede seleccionar el método BSS 'RSSI y NUCLEO' si alguno de los recursos asignados es un circuito de recepción que no está asignado a una pasarela.";

                                cMsg.confirm(strMsg, "aceptparam");
                                return false;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        return true;
        //20201009 #4608
    }

    protected bool RestriccionesRecursosporModoDestino()
    {
        int numRx = 0;
        int numTx = 0;
        int numTxRx = 0;
        int numAudioRx=0;
        int i = 0;

        //20201030 #4543
        string strMsg = String.Empty;

        switch (DListModoDestino.SelectedValue)
        {
            case DESTINORADIO_MODO_NORMAL:

                for (i = 0; i < ListTipos.Items.Count; i++)
                {
                    if (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_RX) == 0) 
                    {
                        numRx++;
                        numAudioRx++;
                    }
                    else if ((String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_M_N_RX) == 0) ||
                             (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_EE_RX) == 0))
                    {
                        numRx++;
                    }
                    else if ((String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_TX) == 0) || 
                             (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_M_N_TX) == 0) ||
                             (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_EE_TX) == 0))
                        numTx++;
                    else if ((String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_RXTX) == 0) ||
                             (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_M_N_RXTX) == 0) ||
                             (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_EE_RXTX) == 0))
                    {
                        //numTxRx++;
                        numRx++;
                        numTx++;
                    }
                    else
                        break;
                }

                //MVO: se realizan las comprobaciones en función del tipo de frecuencia
                switch (DListTipo.SelectedValue)
                {
                    case DESTINORADIO_TIPOFRECUENCIA_HF:
                        //MVO: Si el tipo destino es HF, el destino sólo puede tener asociado un recurso de tipo Audio-RX
                        if ((numAudioRx != 1 || numRx != 1) || (numTx > 0))
                        {
                            cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorAsignacionRecursosDestinoNormalHF"), "aceptparam");
                            return false;
                        }

                        break;

                    case DESTINORADIO_TIPOFRECUENCIA_VHF:
                    case DESTINORADIO_TIPOFRECUENCIA_UHF:

                        //El destino puede tener asociado un circuito de recepción RX o un circuito RX y otro TX, o un circuito RX-TX 
                        if (numRx != 1 || numTx > 1)
                        {
                            cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorAsignacionRecursosDestinoNormal"), "aceptparam");
                            return false;
                        }
                        break;

                    default:
                        break;
                }
                break;

            case DESTINORADIO_MODO_1_mas_1:
                for (i = 0; i < ListTipos.Items.Count; i++)
                {
                    if ((String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_RX) == 0) || (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_M_N_RX) == 0)) numRx++;
                    else if ((String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_TX) == 0) || (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_M_N_TX) == 0)) numTx++;
                    else if ((String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_RXTX) == 0) || (String.Compare(ListTipos.Items[i].Value, TIPO_RECURSO_M_N_RXTX) == 0)) numTxRx++;
                    else
                        break;
                }
                
                if ((numRx != numTx))
                {
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorDiferentesNumTxyRx"), "aceptparam");
                    return false;
                }

                if (((numRx + numTx) / 2 + numTxRx) != 2)
                {
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorAsignacionRecursosDestino1_mas_1"), "aceptparam");
                    return false;
                }
                break;

            case DESTINORADIO_MODO_FD:
                numTx = 0;
                numRx = 0;
                //MVO: En el modo destino FD,el tipo de frecuencia no puede ser HF
                if (DESTINORADIO_TIPOFRECUENCIA_HF == DListTipo.SelectedValue)
                {
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorAsignacionFrecuenciaHFDestinoFD"), "aceptparam");
                    return false;
                }

                //MVO: Se comprueba que para un mismo emplazamiento no se pueden asignar más de un grupo de recursos (RX, TX), (M_N_TX,M_N_RX), TXRX o M_N_TXRX
                //     Y se obtiene el numTx y numRX de los recursos asignados
                if (!CheckNumRecursosEmplazamientoFD(ListEmplazamientos.Items, ListTipos.Items, ref numTx, ref numRx, ref strMsg))
                {
                    cMsg.confirm(strMsg, "aceptparam");
                    return false;
                }
                //Para FD, se deben asignar como mínimo más de 2 circuitos, de los cuales uno tiene que ser TX y otro RX. 
                if ((numRx < 1 || numTx < 1) || ((numRx + numTx) < 3))
                {
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "MinimoRecursosDestinoFD"), "aceptparam");
                    return false;
                }
                else if (numRx>5 || numTx>5)
                {
                    //El número máximo de circuitos de transmisión o Recepción es 5
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "MaximosRecursosDestinoFD"), "aceptparam");
                    return false;
                }
                //20200728 #4538
                if (bRedundancia && (string.Compare(DDLMetodosBssOfrecidos.SelectedValue, METODO_BSS_NINGUNO) == 0))
                {
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorConfiguracion1_mas_1_BSS"), "aceptparam");
                    return false;
                }
                break;

            default:
                cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorAsignacionRecursosDestino1_mas_1"), "aceptparam");
                return false;
             }
        return true;
    }

    private class CDatosRecurso
    {
        public string IdRecurso;
        public string IdTipo;
        //public int    Indice;
    }

    private class CEmplazamiento 
    {
        public CEmplazamiento()
        {
            ListRecRx = new List<CDatosRecurso>();
            ListRecTx = new List<CDatosRecurso>();
            ListRecRxTx = new List<CDatosRecurso>();
            iNumRx = 0;
            iNumTx = 0;
        }

        public List<CDatosRecurso> ListRecRx;
        public List<CDatosRecurso> ListRecTx;
        public List<CDatosRecurso> ListRecRxTx;
        public int iNumRx;
        public int iNumTx;
    };

    protected class CRecursoRedundancia
    {
        public string IdRecurso;
        public string Rol;
        public string IdPareja;
        public string IdEmplazamiento;
    }

    private bool ClasificaRecursosPorEmplazamiento(bool bModoFD, ref Dictionary<string, CRecursoRedundancia> dicRecursos,
                                                   ref int piNumTX, ref int piNumRX, ref String strMsg)
    {
        bool bCorrecto = true;
        Dictionary<string, CEmplazamiento> dicEmplazamientos = null;
        int i = 0;
        string strNombreEmp = String.Empty;
        int iNumEltosListaRecursos = 0;
        String strTipoRecurso = String.Empty;
        ListItemCollection objListaEmplazamientos = ListEmplazamientos.Items;
        ListItemCollection objListaTipos = ListTipos.Items;
        ListItemCollection listaRecursos = ListRecursos.Items;

        int iNumTxAux = 0;
        int iNumRxAux = 0;
        //20200618 JOI #4510
        bool bErrorCnfNM = false;
        //20201031 #4543
        bool bErrorSoloTX = false;

        //Se clasifican  los recursos asignados por emplazamiento y tipo (receptores, transmisores y transceptores)
        try
        {
            iNumEltosListaRecursos = listaRecursos.Count;

            if (iNumEltosListaRecursos > 0 )
            {
                dicEmplazamientos = new Dictionary<string, CEmplazamiento>();
                if (dicRecursos == null) 
                    dicRecursos = new Dictionary<string, CRecursoRedundancia>();
                else
                    dicRecursos.Clear();

                for (i = 0; i < iNumEltosListaRecursos; i++)
                {
                    strNombreEmp = objListaEmplazamientos[i].Text;

                    if (!dicEmplazamientos.ContainsKey(strNombreEmp))
                    {
                        CEmplazamiento objEmplazamiento = new CEmplazamiento();
                        dicEmplazamientos.Add(strNombreEmp, objEmplazamiento);
                    }

                    var rec = new CDatosRecurso();
                    rec.IdRecurso = listaRecursos[i].Value;
                    rec.IdTipo = objListaTipos[i].Value;
                    //rec.Indice = i;

                    //Obtenemos el número de recursos asignado a cada emplazamiento
                    strTipoRecurso = objListaTipos[i].Value;

                    switch (strTipoRecurso)
                    {
                        case TIPO_RECURSO_RX:
                        case TIPO_RECURSO_M_N_RX:
                        case TIPO_RECURSO_EE_RX:
                            dicEmplazamientos[strNombreEmp].ListRecRx.Add(rec);                           
                            break;

                        case TIPO_RECURSO_TX:
                        case TIPO_RECURSO_M_N_TX:
                        case TIPO_RECURSO_EE_TX:
                            dicEmplazamientos[strNombreEmp].ListRecTx.Add(rec);
                            break;

                        case TIPO_RECURSO_RXTX:
                        case TIPO_RECURSO_M_N_RXTX:
                        case TIPO_RECURSO_EE_RXTX:
                            dicEmplazamientos[strNombreEmp].ListRecRxTx.Add(rec);
                            break;

                        default:
                            break;
                    }
                }
                //20201022 #4510
                if (!bModoFD && dicEmplazamientos.Count() > 1)
                {
                    if (GetGlobalResourceObject("Espaniol", "ErrorConfiguracionFSEmplazamientos") != null)
                        strMsg = GetGlobalResourceObject("Espaniol", "ErrorConfiguracionFSEmplazamientos").ToString();
                    else
                        strMsg = "Si el destino se configura como Frecuencia Simple, no pueden asignarse equipos de distintos emplazamientos";
                    return false;
                }

                var listEmp = dicEmplazamientos.Values.ToList();
                var listEmpRed = listEmp.Where(x=> (x.ListRecRx.Count==2 || x.ListRecTx.Count==2 || x.ListRecRxTx.Count==2));

                if (!listEmpRed.Any())
                {
                    listEmpRed = listEmp.Where(x => (x.ListRecRx.Count > 2 || x.ListRecTx.Count > 2 || x.ListRecRxTx.Count > 2));

                    if (!listEmpRed.Any())
                    {
                        if (GetGlobalResourceObject("Espaniol", "ErrorConfiguracionDestinoConRedundancia") != null)
                            strMsg = GetGlobalResourceObject("Espaniol", "ErrorConfiguracionDestinoConRedundancia").ToString();
                        else
                            strMsg = "Si el destino se configura como 1+1, al menos se debe configurar un recurso con redundancia dentro del mismo emplazamiento";
                    }
                    else
                    {
                        if (GetGlobalResourceObject("Espaniol", "ErrorConfiguracionDestinoConRedundanciaEmplazamiento") != null)
                            strMsg = GetGlobalResourceObject("Espaniol", "ErrorConfiguracionDestinoConRedundanciaEmplazamiento").ToString();
                        else
                            strMsg = "Si el destino se configura como 1+1, dentro de un mismo emplazamiento sólo se pueden configurar con redundancia parejas de recursos Rx, Tx o RxTx";
                    }
                    
                    listEmp.Clear();
                    listEmpRed=null;

                    return false;
                }

                var listEmpRedOk = listEmpRed.Where(x=> ((x.ListRecRx.Count==2 || x.ListRecTx.Count==2) && x.ListRecRxTx.Count==0) || ((x.ListRecRx.Count==0 && x.ListRecTx.Count==0) && x.ListRecRxTx.Count==2));
                
                if (!listEmpRedOk.Any())
                {
                    if (GetGlobalResourceObject("Espaniol", "ErrorConfiguracionDestinoConRedundanciaEmplazamiento")!=null)
                        strMsg = GetGlobalResourceObject("Espaniol", "ErrorConfiguracionDestinoConRedundanciaEmplazamiento").ToString();
                    else
                        strMsg = "Si el destino se configura como 1+1, dentro de un mismo emplazamiento sólo se pueden configurar con redundancia parejas de recursos Rx y Tx, o RxTx";
                    return false;
                }

                //Se calcula el número de RX, TX y RXTX por emplazamiento 
                //comprueba que el destino 1+1 está correctamente configurado
                foreach (var emp in dicEmplazamientos)
                {
                    string idEmplazamiento = emp.Key;
                    CEmplazamiento objEmp = emp.Value;

                    //Calculamos el numero de RX y Tx para cada emplazamiento
                    int iRx=dicEmplazamientos[idEmplazamiento].ListRecRx.Count;
                    int iTx=dicEmplazamientos[idEmplazamiento].ListRecTx.Count;
                    int iRxTx = dicEmplazamientos[idEmplazamiento].ListRecRxTx.Count;

                    dicEmplazamientos[idEmplazamiento].iNumRx = (iRx>1)? iRx-1 : iRx ;
                    dicEmplazamientos[idEmplazamiento].iNumTx = (iTx>1)? iTx-1 : iTx ;
                    if (iRxTx>=1)
                    {
                        dicEmplazamientos[idEmplazamiento].iNumRx++;
                        dicEmplazamientos[idEmplazamiento].iNumTx++;
                    }
                    //20201030 #4543
                    if (iRx == 0 && iRxTx == 0)
                    {
                        bErrorSoloTX = true;
                    }

                    iNumRxAux +=dicEmplazamientos[idEmplazamiento].iNumRx;
                    iNumTxAux +=dicEmplazamientos[idEmplazamiento].iNumTx;
                }

                //20201030 #4543
                if (bErrorSoloTX)
                { 
                    strMsg = (string)GetGlobalResourceObject("Espaniol", "ErrorDestinoRadioEmplazamientoSoloTx");
                    return false;
                }

                //Validamos el número de circuitos de transmisión y recepción en función del modo
                if (bModoFD)
                {
                    var listKeyEmpNOk = dicEmplazamientos.Where(x=> (x.Value.iNumRx>1 || x.Value.iNumTx>1));
                    if (listKeyEmpNOk.Any())
                    {
                        strMsg = (string)GetGlobalResourceObject("Espaniol", "ErrorEmplazamientoSupNumTxRX_DestinoFD");
                        return false;
                    }
                }

                foreach (var emp in dicEmplazamientos)
                {
                    string  idEmplazamiento = emp.Key;
                    CEmplazamiento objEmp = emp.Value;
                    //20201031 #4543
                    bool bHayMmasN = false;
                    bool bHayunomasuno = false;

                    if (dicEmplazamientos[idEmplazamiento].ListRecRx.Count ==2 )
                    {
                        //Componemos la pareja de redundancia
                        CDatosRecurso datosRecP = objEmp.ListRecRx[0];
                        CDatosRecurso datosRecR = objEmp.ListRecRx[1];

                        //20200618 JOI #4510
                        if (datosRecP.IdTipo == TIPO_RECURSO_M_N_RX || datosRecR.IdTipo == TIPO_RECURSO_M_N_RX)
                        {
                            bCorrecto = false;
                            bErrorCnfNM = true;
                            break;
                        }

                        if (!dicRecursos.ContainsKey(datosRecP.IdRecurso))
                        {
                            CRecursoRedundancia objRecRed = new CRecursoRedundancia();
                            objRecRed.IdRecurso = datosRecP.IdRecurso;
                            objRecRed.Rol = "P";
                            objRecRed.IdPareja = string.Format("{0}#{1}", datosRecP.IdRecurso, datosRecR.IdRecurso);
                            objRecRed.IdEmplazamiento = idEmplazamiento;
                            dicRecursos.Add(datosRecP.IdRecurso, objRecRed);
                        }

                        if (!dicRecursos.ContainsKey(datosRecR.IdRecurso))
                        {
                            CRecursoRedundancia objRecRed = new CRecursoRedundancia();
                            objRecRed.IdRecurso = datosRecR.IdRecurso;
                            objRecRed.Rol = "R";
                            objRecRed.IdPareja = string.Format("{0}#{1}", datosRecP.IdRecurso, datosRecR.IdRecurso);
                            objRecRed.IdEmplazamiento = idEmplazamiento;
                            dicRecursos.Add(datosRecR.IdRecurso, objRecRed);
                        }
                        //20201031 #4543
                        bHayunomasuno = true;
                    }
                    else if (dicEmplazamientos[idEmplazamiento].ListRecRx.Count == 1 && objEmp.ListRecRx[0].IdTipo == TIPO_RECURSO_M_N_RX)
                    {
                        bHayMmasN = true;
                    }

                    if (dicEmplazamientos[idEmplazamiento].ListRecTx.Count == 2 )
                    {
                        //Componemos la pareja de redundancia
                        CDatosRecurso datosRecP = objEmp.ListRecTx[0];
                        CDatosRecurso datosRecR = objEmp.ListRecTx[1];

                        //20200618 JOI #4510
                        if (datosRecP.IdTipo == TIPO_RECURSO_M_N_TX || datosRecR.IdTipo == TIPO_RECURSO_M_N_TX)
                        {
                            bCorrecto = false;
                            bErrorCnfNM = true;
                            break;
                        }

                        if (!dicRecursos.ContainsKey(datosRecP.IdRecurso))
                        {
                            CRecursoRedundancia objRecRed = new CRecursoRedundancia();
                            objRecRed.IdRecurso = datosRecP.IdRecurso;
                            objRecRed.Rol = "P";
                            objRecRed.IdPareja = string.Format("{0}#{1}", datosRecP.IdRecurso, datosRecR.IdRecurso);
                            objRecRed.IdEmplazamiento = idEmplazamiento;
                            dicRecursos.Add(datosRecP.IdRecurso, objRecRed);
                        }

                        if (!dicRecursos.ContainsKey(datosRecR.IdRecurso))
                        {
                            CRecursoRedundancia objRecRed = new CRecursoRedundancia();
                            objRecRed.IdRecurso = datosRecR.IdRecurso;
                            objRecRed.Rol = "R";
                            objRecRed.IdPareja = string.Format("{0}#{1}", datosRecP.IdRecurso, datosRecR.IdRecurso);
                            objRecRed.IdEmplazamiento = idEmplazamiento;
                            dicRecursos.Add(datosRecR.IdRecurso, objRecRed);
                        }
                        //20201031 #4543
                        bHayunomasuno = true;
                    }
                    else if (dicEmplazamientos[idEmplazamiento].ListRecTx.Count == 1 && objEmp.ListRecTx[0].IdTipo == TIPO_RECURSO_M_N_TX)
                    {
                        bHayMmasN = true;
                    }

                    if (dicEmplazamientos[idEmplazamiento].ListRecRxTx.Count == 2)
                    {
                        //Componemos la pareja de redundancia
                        CDatosRecurso datosRecP = objEmp.ListRecRxTx[0];
                        CDatosRecurso datosRecR = objEmp.ListRecRxTx[1];

                        //20200618 JOI #4510
                        if (datosRecP.IdTipo == TIPO_RECURSO_M_N_RXTX || datosRecR.IdTipo == TIPO_RECURSO_M_N_RXTX)
                        {
                            bCorrecto = false;
                            bErrorCnfNM = true;
                            break;
                        }

                        if (!dicRecursos.ContainsKey(datosRecP.IdRecurso))
                        {
                            CRecursoRedundancia objRecRed = new CRecursoRedundancia();
                            objRecRed.IdRecurso = datosRecP.IdRecurso;
                            objRecRed.Rol = "P";
                            objRecRed.IdPareja = string.Format("{0}#{1}", datosRecP.IdRecurso, datosRecR.IdRecurso);
                            objRecRed.IdEmplazamiento = idEmplazamiento;
                            dicRecursos.Add(datosRecP.IdRecurso, objRecRed);
                        }

                        if (!dicRecursos.ContainsKey(datosRecR.IdRecurso))
                        {
                            CRecursoRedundancia objRecRed = new CRecursoRedundancia();
                            objRecRed.IdRecurso = datosRecR.IdRecurso;
                            objRecRed.Rol = "R";
                            objRecRed.IdPareja = string.Format("{0}#{1}", datosRecP.IdRecurso, datosRecR.IdRecurso);
                            objRecRed.IdEmplazamiento = idEmplazamiento;
                            dicRecursos.Add(datosRecR.IdRecurso, objRecRed);
                        }
                        //20201031 #4543
                        bHayunomasuno = true;
                    }
                    else if (dicEmplazamientos[idEmplazamiento].ListRecRxTx.Count == 1 && objEmp.ListRecRxTx[0].IdTipo == TIPO_RECURSO_M_N_RXTX)
                    {
                        bHayMmasN = true;
                    }
                    //20201031 #4543
                    if (bHayunomasuno && bHayMmasN)
                    {
                        strMsg = (string)GetGlobalResourceObject("Espaniol", "ErrorEmplazamientoUnoMasUnoMmasN");
                        return false;
                    }

                }

                piNumTX = iNumTxAux;
                piNumRX = iNumRxAux;

                if (!bCorrecto)
                {
                    dicRecursos.Clear();
                }
                dicEmplazamientos.Clear();
                if (bErrorCnfNM)
                {
                    bCorrecto = false;
                    if (GetGlobalResourceObject("Espaniol", "ErrorConfiguracionDestinoConRedundanciaEmplazamiento") != null)
                        strMsg = GetGlobalResourceObject("Espaniol", "ErrorConfiguracionDestinoConRedundanciaEmplazamiento").ToString();
                    else
                        strMsg = "Si el destino se configura como 1+1, dentro de un mismo emplazamiento sólo se pueden configurar con redundancia parejas de recursos Rx y Tx, o RxTx";
                }
            } //if

        }
        catch (Exception ex)
        {
            bCorrecto = false;
            logDebugView.Error("(GrupoFD-ClasificaRecursosPorEmplazamiento): se ha producido una excepción.Error: " + ex.ToString());
        }

        return bCorrecto;
    }

    protected bool RestriccionesRecursosporModoDestinoConRedundancia(string modoDestino, string tipoFrecuencia,
                                                                     ref Dictionary<string,CRecursoRedundancia> dicRecursos,  ref String strMsg) 
    {
        int numRx = 0;
        int numTx = 0;
        int i = 0;
        strMsg = string.Empty;

        //Si el destino se configura con reduncia, alguno de los recursos del destino se tiene configurar con un recurso redundante
        // Y dicho recurso debe pertenecer la mismo emplazamiento

        // Se agrupan los recursos por emplazamientos
        if (!ClasificaRecursosPorEmplazamiento(modoDestino == DESTINORADIO_MODO_FD, ref dicRecursos, ref numTx, ref numRx, ref strMsg))
        {
            //Si el modo es FD, para un mismo emplazamiento no se pueden asignar más de un grupo de recursos (RX, TX), (M_N_TX,M_N_RX), TXRX o M_N_TXRX
            cMsg.confirm(strMsg, "aceptparam");
            return false;
        }

        switch (modoDestino)
        {
            case DESTINORADIO_MODO_NORMAL:

                //MVO: se realizan las comprobaciones en función del tipo de frecuencia
                switch (tipoFrecuencia)
                {
                    case DESTINORADIO_TIPOFRECUENCIA_HF:
                        //Si el tipo destino es HF, el destino sólo puede tener asociado un recurso de tipo Audio-RX
                        if ((numRx != 1) || (numTx > 0))
                        {
                            cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorAsignacionRecursosDestinoNormalHF"), "aceptparam");
                            return false;
                        }
                        break;

                    case DESTINORADIO_TIPOFRECUENCIA_VHF:
                    case DESTINORADIO_TIPOFRECUENCIA_UHF:

                        //El destino puede tener asociado un circuito de recepción RX o un circuito RX y otro TX, o un circuito RX-TX 
                        if (numRx != 1 || numTx > 1)
                        {
                            cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorAsignacionRecursosDestinoNormalConRedundancia"), "aceptparam");
                            return false;
                        }
                        break;

                    default:
                        break;
                }
                break;

            case DESTINORADIO_MODO_FD:
                //Para FD, se deben asignar como mínimo más de 2 circuitos, de los cuales uno tiene que ser TX y otro RX. 
                if ((numRx < 1 || numTx < 1) || ((numRx + numTx) < 3))
                {
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "MinimoRecursosDestinoFDRedundancia"), "aceptparam");
                    return false;
                }
                else if (numRx > 5 || numTx > 5)
                {
                    //El número máximo de circuitos de transmisión o Recepción es 5
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "MaximosRecursosDestinoFDRedundancia"), "aceptparam");
                    return false;
                }
                break;

            default:
                cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorAsignacionRecursosDestino1_mas_1"), "aceptparam");
                return false;
        }

        return true;
    }

    private class CDato: Object { public int iNumRx=0; public int iNumTx=0;};

    //Se comprueba que para las Destinos con FD, para un mismo emplazamiento sólo se puede asociar como máximo un recurso radio RX y otro TX o un sólo recurso RXTX
    // Y se devuelve como parámetro el numTx y numRX de los recursos asignados
    private bool CheckNumRecursosEmplazamientoFD(ListItemCollection objListaEmplazamientos, ListItemCollection objListaTipos, ref int piNumTX, ref int piNumRX, ref String strMsg)
    {
        bool bCorrecto = true;
        Hashtable ListaEmplazamientos;
        int i=0;
        string strNombreEmp=String.Empty;
        int iNumEltosListaEmplazamientos = 0;
        int iNumEltosTipos = 0;
        String strTipoAudio= String.Empty;

        CDato objDatos = null;
        int iNumTxAux = 0;
        int iNumRxAux = 0;

        //Obtenemos los distintos emplazamientos de los recursos asignados
        try
        {
            iNumEltosListaEmplazamientos = objListaEmplazamientos.Count;
            iNumEltosTipos = objListaTipos.Count;

            if (iNumEltosListaEmplazamientos > 0 && iNumEltosTipos>0)
            {
                ListaEmplazamientos = new Hashtable();
                for (i = 0; i < iNumEltosListaEmplazamientos; i++)
                {
                    strNombreEmp = objListaEmplazamientos[i].Text;

                    if (!ListaEmplazamientos.ContainsKey(strNombreEmp))
                    {
                        if (null == objDatos)
                        {
                            objDatos = new CDato();
                        }

                        ListaEmplazamientos.Add(strNombreEmp,objDatos);
                        objDatos = null;
                    }

                    //Obtenemos el número de recursos TX y Rx asignado a cada emplazamiento
                    strTipoAudio = objListaTipos[i].Value;

                    switch (strTipoAudio)
                    {
                        case TIPO_RECURSO_RX:
                        case TIPO_RECURSO_M_N_RX:
                        case TIPO_RECURSO_EE_RX:
                            ((CDato)ListaEmplazamientos[strNombreEmp]).iNumRx++;
                            iNumRxAux++;
                            break;

                        case TIPO_RECURSO_TX:
                        case TIPO_RECURSO_M_N_TX:
                        case TIPO_RECURSO_EE_TX:
                            ((CDato)ListaEmplazamientos[strNombreEmp]).iNumTx++;
                            iNumTxAux++;
                            break;

                        case TIPO_RECURSO_RXTX:
                        case TIPO_RECURSO_M_N_RXTX:
                        case TIPO_RECURSO_EE_RXTX:
                            ((CDato)ListaEmplazamientos[strNombreEmp]).iNumRx++;
                            ((CDato)ListaEmplazamientos[strNombreEmp]).iNumTx++;
                            iNumTxAux++;
                            iNumRxAux++;

                            break;
                        default:
                            break;
                    }
                }

                piNumTX = iNumTxAux;
                piNumRX = iNumRxAux;

                //Recorremos el mapa para verificar si hay más de un RX o TX para el mismo emplazamiento
                foreach (DictionaryEntry upd in ListaEmplazamientos)
                {
                    objDatos = (CDato)ListaEmplazamientos[upd.Key];
                    
                    if (objDatos.iNumRx > 1 || objDatos.iNumTx > 1 )
                    {
                        bCorrecto = false;
                        strMsg = (string)GetGlobalResourceObject("Espaniol", "ErrorEmplazamientoSupNumTxRX_DestinoFD");
                        break;
                    }
                    //20201030 #4543
                    if (objDatos.iNumRx == 0)
                    {
                        bCorrecto = false;
                        strMsg = (string)GetGlobalResourceObject("Espaniol", "ErrorDestinoRadioEmplazamientoSoloTx");
                        break;
                    }

                }

                ListaEmplazamientos.Clear();

            } //if

        }
        catch (Exception ex)
        {
            bCorrecto = false;
            logDebugView.Error("(GrupoFD-CheckNumRecursosEmplazamientoFD): se ha producido una excepción.Error: "+ ex.ToString());
        }
        
        return bCorrecto;
    }

    private bool IgualEmplazamiento(string tipoRecurso, string site)
    {
        string tipoABuscar = string.Empty;

        switch (tipoRecurso)
        {
            case "4":   // M+N Rx
                tipoABuscar = "5";
                break;
            case "5":   // M+N Tx
                tipoABuscar = "4";
                break;
            case "0":   // Audio Rx
                tipoABuscar = "1";
                break;
            case "1":   // Audio Tx
                tipoABuscar = "0";
                break;
        }

        for (int j = 0; j < ListTipos.Items.Count; j++)
        {
            if ((String.Compare(ListTipos.Items[j].Value, tipoABuscar) == 0) && site == ListEmplazamientos.Items[j].Text)
            {
                //  Hemos encontrado un recurso complementario (Rx/Tx) al seleccionado del mismo emplazamiento
                return true;
            }
        }

        return false;
    }

    protected bool DiferentesEmplazamientosTxRx(int i, string Radio)
    {
        for (int j = 0; j < ListTipos.Items.Count; j++)
        {
            if ((String.Compare(ListTipos.Items[j].Text, Radio) == 0) || (String.Compare(ListTipos.Items[j].Text, "Audio RX TX") == 0))
            {
                //Coincide emplazamientos Tx y Rx
                if ((String.Compare(ListEmplazamientos.Items[i].Text, ListEmplazamientos.Items[j].Text) == 0) && (i!=j))
                {
                    cMsg.confirm((string)GetGlobalResourceObject("Espaniol", "ErrorAsignacionEmplazamientosRecursosDestinoFD"), "aceptparam");
                    return false;
                }
            }
        }

        return true;
    }


    protected void IButAsignar_Click(object sender, ImageClickEventArgs e)
    {

        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sincronizar = config.AppSettings.Settings["SincronizaCD30"];

        ServiciosCD40.RecursosRadio recRd = new ServiciosCD40.RecursosRadio();

        if (ListRecursosLibres.SelectedIndex >= 0)
            for (int i = 0; i < ListRecursosLibres.Items.Count; i++)
                if (ListRecursosLibres.Items[i].Selected)
                {
                    #region Sincronizar
                    if ((sincronizar != null) && (Int32.Parse(sincronizar.Value) == 1))
                    {//Comprobar que el recurso seleccionado no tiene el mismo emplazamiento que otro recurso ya asignado
                        List<ServiciosCD40.RecursosRadio> emplAsignados = new List<ServiciosCD40.RecursosRadio>();
                        for (int h = 0; h < ListRecursos.Items.Count; h++)
                        {
                            ServiciosCD40.RecursosRadio rec = new ServiciosCD40.RecursosRadio();
                            rec.IdSistema = (string)Session["IdSistema"];
                            rec.IdRecurso = ListRecursos.Items[h].Value;
                            ServiciosCD40.Tablas[] d = ServicioCD40.ListSelectSQL(rec);
                            rec.IdEmplazamiento = ((ServiciosCD40.RecursosRadio)d[0]).IdEmplazamiento;
                            emplAsignados.Add(rec);

                        }
                        //Obtener el emplazamiento del recurso seleccionado
                        string empl = string.Empty;

                        recRd.IdSistema = (string)Session["IdSistema"];
                        recRd.IdRecurso = ListRecursosLibres.Items[i].Value;
                        ServiciosCD40.Tablas[] l = ServicioCD40.ListSelectSQL(recRd);
                        empl = ((ServiciosCD40.RecursosRadio)l[0]).IdEmplazamiento;

                        bool asignar = true;
                        foreach (ServiciosCD40.RecursosRadio s in emplAsignados)
                        {
                            if (s.IdEmplazamiento.CompareTo(empl) == 0)
                            {
                                asignar = false;
                                cMsg.alert(String.Format((string)GetGlobalResourceObject("Espaniol", "MismoEmplazamiento"), recRd.IdRecurso, s.IdRecurso));
                                break;
                            }
                        }
                        if (asignar)
                        {
                            ListRecursos.Items.Add(ListRecursosLibres.Items[i]);

                            // MAF
                            //ServiciosCD40.RecursosRadio emp1 = new ServiciosCD40.RecursosRadio();
                            recRd.IdSistema = (string)Session["IdSistema"];
                            recRd.IdRecurso = ListRecursosLibres.Items[i].Value;
                            ServiciosCD40.Tablas[] l1 = ServicioCD40.ListSelectSQL(recRd);
                            ListEmplazamientos.Items.Add(((ServiciosCD40.RecursosRadio)l1[0]).IdEmplazamiento);

                            //MAF
                            int uintp = ((ServiciosCD40.RecursosRadio)l1[0]).Tipo > 3 && ((ServiciosCD40.RecursosRadio)l1[0]).Tipo < 7 ? 4 : (int)((ServiciosCD40.RecursosRadio)l1[0]).Tipo;
                            ListTipos.Items.Add(DListTipoRec.Items[uintp].Text);

                            ListTipos.Items[ListTipos.Items.Count - 1].Value = ((ServiciosCD40.RecursosRadio)l1[0]).Tipo.ToString();//MAF_MAF

                            // MAF
                            ListRecursosLibres.Items.Remove(ListRecursosLibres.Items[i]);
                            i--;
                        }
                    }
                    #endregion
                    else
                    {
                        ServiciosCD40.HFParams r = new ServiciosCD40.HFParams();

                        r.IdSistema = (string)Session["idsistema"];
                        r.IdRecurso = ListRecursosLibres.Items[i].Value;
                        ServiciosCD40.Tablas[] h = ServicioCD40.ListSelectSQL(r);

                        if (h.Length > 0)
                        {
                            //20201016 #4582 
                            if (TxtIdEnlace.Text != ((ServiciosCD40.HFParams)h[0]).Frecuencia)
                            {
                                string mensaje;
                                mensaje = string.Format((string)GetGlobalResourceObject("Espaniol", "ErrorFrecuenciaRecursoMN_Destino"), ((ServiciosCD40.HFParams)h[0]).Frecuencia, TxtIdEnlace.Text); 
                                cMsg.confirm(mensaje, "aceptparam");
                                return;
                            }

                            if (Frecuencia == string.Empty)
                                Frecuencia = ((ServiciosCD40.HFParams)h[0]).Frecuencia;
                            else if (Frecuencia != ((ServiciosCD40.HFParams)h[0]).Frecuencia)
                            {
                                LblErrorMismatchFrequency.Visible = true;
                            }
                        }

                        recRd.IdSistema = (string)Session["IdSistema"];
                        recRd.IdRecurso = ListRecursosLibres.Items[i].Value;
                        ServiciosCD40.Tablas[] idRecRd = ServicioCD40.ListSelectSQL(recRd);
                        ListEmplazamientos.Items.Add(((ServiciosCD40.RecursosRadio)idRecRd[0]).IdEmplazamiento);

                        if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_FD && 
                            DListModoTransmision.SelectedValue == MODO_TRANSMISION_ULTIMA_RECEPCION)
                        {
                            //VMG 
                            //18/02/2019 Metiendo los emplazamientos por defecto 
                            for (int j = 0; j < ListEmplazamientos.Items.Count; j++)
                            {
                                if (DListEmplazamientoDefecto.Items.FindByText(ListEmplazamientos.Items[j].Text) == null)
                                    DListEmplazamientoDefecto.Items.Add(ListEmplazamientos.Items[j].Text);
                            }
                        }

                        int uintp = ((ServiciosCD40.RecursosRadio)idRecRd[0]).Tipo > 3 && ((ServiciosCD40.RecursosRadio)idRecRd[0]).Tipo < 7 ? 4 : (int)((ServiciosCD40.RecursosRadio)idRecRd[0]).Tipo;
                        ListItem lbItem = new ListItem();
                        StringBuilder strTexto = new StringBuilder();
                        ListItem itemTipoLibre = ListTiposLibres.Items[i];
                        ListItem itemEmplazamientoLibre = ListEmplazamientosLibres.Items[i];

                        lbItem.Value = ListRecursosLibres.Items[i].Value;

                        strTexto.AppendFormat(FORMATO_LB_RECURSOS, recRd.IdRecurso, itemTipoLibre.Text, itemEmplazamientoLibre.Text);
                        strTexto.Replace(" ", "\u00A0");
                        lbItem.Text = strTexto.ToString();
                        ListRecursos.Items.Add(lbItem);

                        ListTipos.Items.Add(itemTipoLibre);
                        //ListTipos.Items.Add(DListTipoRec.Items[uintp].Text);
                        //ListTipos.Items[ListTipos.Items.Count-1].Value = ((ServiciosCD40.RecursosRadio)idRecRd[0]).Tipo.ToString();//MAF_MAF

                        if (ListEmplazamientosLibres.Items.Count>0)
                            ListEmplazamientosLibres.Items[i].Selected = true;

                        if (ListTiposLibres.Items.Count>0)
                            ListTiposLibres.Items[i].Selected = true;

                        int index = ListRecursosLibres.SelectedIndex;

                        ListRecursosLibres.Items.RemoveAt(index);

                        if (ListEmplazamientosLibres.Items.Count > 0 && index<ListEmplazamientosLibres.Items.Count)
                            ListEmplazamientosLibres.Items.RemoveAt(index);

                        if (ListTiposLibres.Items.Count > 0 && index < ListTiposLibres.Items.Count)
                            ListTiposLibres.Items.RemoveAt(index);
                        i--;
                    }
                }
    }


    /// <summary>
    /// Eliminar el recurso asignado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void IButQuitar_Click(object sender, ImageClickEventArgs e)
    {
        if (ListRecursos.SelectedIndex >= 0)
        {
            ServiciosCD40.RecursosRadio recRd = new ServiciosCD40.RecursosRadio();

            if(DListEmplazamiento.SelectedIndex != 0)
                CargarRecursosSinAsignar();

            LiberaRecursos();

            if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_FD &&
                DListModoTransmision.SelectedValue == MODO_TRANSMISION_ULTIMA_RECEPCION)
            {
                ListItem iNoneItem = new ListItem();
                iNoneItem.Value = "0";
                iNoneItem.Text = ObtenerTextoRecursoLocal("ListItemEmplDefNinguno.Text", "Ninguno");

                string strEmplDefecto = DListEmplazamientoDefecto.SelectedValue;

                //VMG 18/02/2019 
                //var selectedValue = DListEmplazamientoDefecto.SelectedValue;
                //Quitando de los emplazamientos por defecto 
                DListEmplazamientoDefecto.Items.Clear();//Borramos todos y añadimos de nuevo para no liarnos
                DListEmplazamientoDefecto.Items.Add(iNoneItem);
                for (int j = 0; j < ListEmplazamientos.Items.Count; j++)
                {
                    if (DListEmplazamientoDefecto.Items.FindByText(ListEmplazamientos.Items[j].Text) == null)
                        DListEmplazamientoDefecto.Items.Add(ListEmplazamientos.Items[j].Text);
                    //Si se elimina o no el recurso del emplazamiento por defecto, dejarlo o no en la lista
                    //if (ListEmplazamientos.Items[j].Value == selectedValue)
                    //    DListEmplazamientoDefecto.SelectedValue = selectedValue;
                    //else
                    //    DListEmplazamientoDefecto.SelectedIndex = 0;//Si es que no, poner ninguno y cero 
                }

                if (DListEmplazamientoDefecto.Items.FindByText(strEmplDefecto) != null)
                {
                    DListEmplazamientoDefecto.SelectedIndex = -1;
                    DListEmplazamientoDefecto.Items.FindByText(strEmplDefecto).Selected = true;
                }
                else
                {
                    if (DListEmplazamientoDefecto.SelectedIndex == 0)
                        TxtTiempoVueltaADefecto.Text = "0";
                }
            }

            LblErrorMismatchFrequency.Visible = false;
        }
    }

    private void LiberaRecursos()
    {
        int index;
        StringBuilder strTexto = new StringBuilder();
        //Indica si se visualiza la lista de recursos libres con tipos  (true) o con emplazamientos (false)
        bool visualizaRecLibresTipos = (String.Compare("0", DropDownFiltro.SelectedValue) == 0);

        for (int i = 0; i < ListRecursos.Items.Count; i++)
        {
            if (ListRecursos.Items[i].Selected)
            {
                ListItem lbItem = new ListItem();
                strTexto.Clear();

                if (visualizaRecLibresTipos)
                {
                    //Se visaliza el recurso libre con el tipo
                    //20201014  #4539 strTexto.AppendFormat(FORMATO_LB_RECURSOS_LIBRES, ListRecursos.Items[i].Value, ListTipos.Items[i].Text);
                    strTexto.AppendFormat(FORMATO_LB_RECURSOS_LIBRES, ListRecursos.Items[i].Value, ListTipos.Items[i].Text, ListEmplazamientos.Items[i].Text);
                    lbItem.Attributes.Add("title", ListEmplazamientos.Items[i].Text);
                }
                else
                {
                    //Se visaliza el recurso libre con el emplazamiento
                    //20201014  #4539 strTexto.AppendFormat(FORMATO_LB_RECURSOS_LIBRES, ListRecursos.Items[i].Value, ListEmplazamientos.Items[i].Text);
                    strTexto.AppendFormat(FORMATO_LB_RECURSOS_LIBRES, ListRecursos.Items[i].Value, ListEmplazamientos.Items[i].Text, ListTipos.Items[i].Text);
                    lbItem.Attributes.Add("title", ListTipos.Items[i].Text);
                }
                strTexto.Replace(" ", "\u00A0");
                lbItem.Text = strTexto.ToString();
                lbItem.Value = ListRecursos.Items[i].Value;

                ListRecursosLibres.Items.Add(lbItem);

                ListTiposLibres.Items.Add(ListTipos.Items[i]);
                ListEmplazamientosLibres.Items.Add(ListEmplazamientos.Items[i]);

                ListEmplazamientos.Items[i].Selected = true;
                ListTipos.Items[i].Selected = true;
                index = ListRecursos.SelectedIndex;
                ListRecursos.Items.RemoveAt(index);
                ListEmplazamientos.Items.RemoveAt(index);
                ListTipos.Items.RemoveAt(index);
                i--;
            }
        }

        if (ListRecursos.Items.Count > 0)
            ListRecursos.SelectedIndex = 0;
    }

    private bool DestinoAsignadoATft(string destino)
    {
        ServiciosCD40.DestinosRadioSector drs = new ServiciosCD40.DestinosRadioSector();
        drs.IdSistema = (string)Session["idsistema"];
        drs.IdDestino = destino;

        ServiciosCD40.Tablas[] lista = ServicioCD40.ListSelectSQL(drs);

        if (lista != null && lista.Length > 0)
            return true;

        return false;
    }

    private void EliminaDestinoDeTFT()
    {
        ServiciosCD40.DestinosRadioSector drs = new ServiciosCD40.DestinosRadioSector();
        drs.IdSistema = (string)Session["idsistema"];
        drs.IdDestino = ListBox1.SelectedValue;

        if (drs.IdSistema != null && drs.IdDestino != null)
        {
            // Eliminar de todos los TFT el destino
            ServicioCD40.DeleteSQL(drs);
        }
    }

    // Respuesta a la regeneración de las sectorizaciones
    private void OnCallBackCompleted(IAsyncResult result)
    {
        try
        {
            int retorno = ServicioCD40.EndRegeneraSectorizaciones(result);
            Session.Add("Sectorizando", false);
        }
        catch (System.Web.Services.Protocols.SoapException soapException)
        {
            logDebugView.Error("(GrupoFD-OnCallBackCompleted): ", soapException);
        }
    }

    protected void AsignarEmplazamiento(string Emplazamiento)
    {
        for (int j = 0; j < DListEmplazamiento.Items.Count; j++)
        {
            if (String.Compare(Emplazamiento, DListEmplazamiento.Items[j].Text) == 0)
            {
                DListEmplazamiento.SelectedIndex = j;

                ListRecursos.Items.Clear();
                ListEmplazamientos.Items.Clear();
                ListTipos.Items.Clear();
                CargarRecursos();
                CargarRecursosSinAsignar(porEmplazamiento:true);
            }
        }
    
    }

    protected void DListTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        CargarRecursosSinAsignar();

        if (DListTipo.SelectedValue == DESTINORADIO_TIPOFRECUENCIA_HF)
        {
            TblTunedFreq.Visible = true;
            RFV_TBTunedFrequency.Enabled = RFV_TBTunedFrequency.Visible = true;
            //Se limpia los recursos asignados para que cuando se cambie el tipo, si previamente se ha asignado algún recurso
            //la configuración no se quede inconsistente
            if (DListModoDestino.SelectedIndex==1)
                LblTunedFreq.Visible = TBTunedFrequency.Visible = Label10.Visible = false;
            else
                LblTunedFreq.Visible = TBTunedFrequency.Visible = Label10.Visible = true;
            if (ListRecursos.Items.Count > 0)
            {
                ListRecursos.Items.Clear();
                ListEmplazamientos.Items.Clear();
                ListTipos.Items.Clear();
            }
        }
        else
        {
            LblTunedFreq.Visible = TBTunedFrequency.Visible = Label10.Visible = false;
            TblTunedFreq.Visible = false;
            RFV_TBTunedFrequency.Enabled = RFV_TBTunedFrequency.Visible = false;
        }
    }

    protected void DFiltro_SelectedIndexChanged(object sender, EventArgs e)
    {
        DListEmplazamiento_SelectedIndexChanged(sender, e);
    }

    protected void DListModoDestino_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_FD)
        {
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible =  Label9.Visible = DListMetodoClimax.Visible = TextVentanaBSS.Enabled= true;
            Label44.Visible = TextBoxCLD.Visible = true;
            LbModoTransmision.Visible = DListModoTransmision.Visible = true;
            DListModoTransmision.SelectedValue = MODO_TRANSMISION_CLIMAX; //Por defecto, se selecciona Climax
            TextBoxCLD.Text = "1"; // Por defecto, se configura a 1 segundo
            LblTunedFreq.Visible = TBTunedFrequency.Visible = Label10.Visible = false;
        }
        //20200520 JOI Errores #4425
        else if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_NORMAL && bRedundancia)
        {
            //Se hacen visibles las opciones de MODO_NORMAL Y REDUNDANCIA
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = true;
            TextVentanaBSS.Enabled = true;
            Label9.Visible = DListMetodoClimax.Visible = false;
            Label44.Visible = TextBoxCLD.Visible = false;
            LbModoTransmision.Visible = DListModoTransmision.Visible = false;
        }
        else
        {
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = Label9.Visible = DListMetodoClimax.Visible = TextVentanaBSS.Enabled = false;
            Label44.Visible = TextBoxCLD.Visible = false;
            LbModoTransmision.Visible = DListModoTransmision.Visible = false;
            AsignaValoresDefectoModoNormal();
            if(DListTipo.SelectedIndex==2)
                LblTunedFreq.Visible = TBTunedFrequency.Visible = Label10.Visible = true;
            LbEmplazamientoDefecto.Visible = DListEmplazamientoDefecto.Visible = false;
            LbTiempoVueltaADefecto.Visible = TxtTiempoVueltaADefecto.Visible = false;
        }


       TblTunedFreq.Visible = DListTipo.SelectedValue == DESTINORADIO_TIPOFRECUENCIA_HF ? true : false;
       RFV_TBTunedFrequency.Enabled = RFV_TBTunedFrequency.Visible = TblTunedFreq.Visible;
    }

    protected void DListPrioridadSIP_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void DListMetodoClimax_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    //Devuelve true si ya existe algun destino en el sistema con el mismo identificador. En caso contrario, false.
    private bool bIdentificadorAsignado(string strIdSistema, string strIdentificador)
    {
        bool bExiste = false;
        int iExiste = -1;

        // Se comprueba que no existe otro destino radio o de telefonía con el mismo identificador 
        iExiste = ServicioCD40.CheckIdentificadorAsignado("D", strIdSistema, strIdentificador);

        if (iExiste > 0)
            bExiste = true;
        else if (iExiste < 0)
        {
            System.Text.StringBuilder strMsgError = new System.Text.StringBuilder();
            strMsgError.AppendFormat("(GrupoFD-bIdentificadorAsignado): el servicio servicioCD40.CheckIdentificadorAsignado('D', '{0}', '{1}') ha devuelto el codigo {2}", strIdSistema, strIdentificador, iExiste);
            logDebugView.Error(strMsgError.ToString());
            strMsgError.Clear();
        }

        return bExiste;
    }

    ///VMG 18/02/2019
    /// <summary>
    /// Mostrar u ocultar los campos para el modo Última recepción
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DListModoTransmision_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DListModoTransmision.SelectedValue == MODO_TRANSMISION_ULTIMA_RECEPCION)
        {
            ListItem iNoneItem = new ListItem();
            
            iNoneItem.Value = "0";
            iNoneItem.Text = ObtenerTextoRecursoLocal("ListItemEmplDefNinguno.Text", "Ninguno");

            LbEmplazamientoDefecto.Visible = true;
            DListEmplazamientoDefecto.Items.Clear();
            DListEmplazamientoDefecto.Items.Add(iNoneItem);
            DListEmplazamientoDefecto.Visible = true;
            LbTiempoVueltaADefecto.Visible = true;
            TxtTiempoVueltaADefecto.Text = "0";
            TxtTiempoVueltaADefecto.Visible = true;

            for (int j = 0; j < ListEmplazamientos.Items.Count; j++)
            {
                if (DListEmplazamientoDefecto.Items.FindByText(ListEmplazamientos.Items[j].Text) == null)
                    DListEmplazamientoDefecto.Items.Add(ListEmplazamientos.Items[j].Text);
            }
            DListEmplazamientoDefecto.SelectedValue = "0";
            //20200611 JOI #3994
            Label44.Visible = TextBoxCLD.Visible = false;
            Label9.Visible = DListMetodoClimax.Visible = false;
        }
        else //20200611 JOI #3994
        {
            Label9.Visible = DListMetodoClimax.Visible = true;
            Label44.Visible = TextBoxCLD.Visible = true;
            LbEmplazamientoDefecto.Visible = false;
            DListEmplazamientoDefecto.Visible = false;
            LbTiempoVueltaADefecto.Visible = false;
            TxtTiempoVueltaADefecto.Visible = false;
        }
    }

    ///VMG 18/02/2019                           
    /// <summary>
    /// Reset del tiempo de vuelta a defecto
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DListEmplazamientoDefecto_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DListEmplazamientoDefecto.SelectedValue == "0")
            TxtTiempoVueltaADefecto.Text = "0";
    }

    //20200520 JOI Errores #4425
    protected void CBRedundancia_CheckedChanged(object sender, EventArgs e)
    {
        bRedundancia = (CheckBoxRedundancia.Checked == true) ? true : false;
        //20200520 JOI Errores #4425
        if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_NORMAL && bRedundancia)
        {
            //Se hacen visibles las opciones de MODO_NORMAL Y REDUNDANCIA
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = true;
            TextVentanaBSS.Enabled = true;
            Label9.Visible = DListMetodoClimax.Visible = false;
            Label44.Visible = TextBoxCLD.Visible = false;
            LbModoTransmision.Visible = DListModoTransmision.Visible = false;
        }
        else if (DListModoDestino.SelectedValue == DESTINORADIO_MODO_NORMAL)
        {
            Label11.Visible = TextVentanaBSS.Visible = Label12.Visible = DDLMetodosBssOfrecidos.Visible = false;
            Label9.Visible = DListMetodoClimax.Visible = TextVentanaBSS.Enabled = false;
            Label44.Visible = TextBoxCLD.Visible = false;
            LbModoTransmision.Visible = DListModoTransmision.Visible = false;
            //Se le asignan los valores por defecto a los campos Metodo climax, Tiempo CLD, Ventana BSS y Metodo BSS para el modo Normal
            //AsignaValoresDefectoModoNormal();
        }
    }

}

 


