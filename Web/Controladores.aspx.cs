using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;

using TListaDeSectores = System.Collections.Generic.List<string>;
using TListaDeTops = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>;
using TListaDeSectorizaciones = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>>;


// Clase para ordenar un ListBox
class myListItemComparer : System.Collections.IComparer
{
	int System.Collections.IComparer.Compare(object x, object y)
	{
		ListItem item_1 = x as ListItem;
		ListItem item_2 = y as ListItem;

		return item_1.Value.CompareTo(item_2.Value);
	}
}


public partial class Controladores : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
{
	string _Command;
	static ServiciosCD40.ServiciosCD40 ServiceServiciosCD40;// = new ServiciosCD40.ServiciosCD40();
	static int NumTops = 0;
	static TListaDeSectorizaciones ListaDeSectorizaciones = new TListaDeSectorizaciones();
	static string IdSectorizacionActiva;
	static string TopSeleccionada = string.Empty;
    private static AsyncCallback CallbackCompletado;

    protected void Page_Load(object sender, EventArgs e)
	{
        //base.Page_Load(sender, e);

        //cMsg = (Mensajes.msgBox)this.Master.FindControl("MsgBox1");
        
        if (Context.Request.IsAuthenticated)
		{
			// retrieve user's identity from httpcontext user 
			FormsIdentity ident = (FormsIdentity)Context.User.Identity;
			string perfil = ident.Ticket.UserData;
			if (perfil != "0")
			{
				Response.Redirect("~/Configuracion/Inicio.aspx?Permiso=NO");
				return;
			}
		}
		else if (!IsPostBack)
		{
			Response.Redirect("~/Login.aspx");
			return;
		}

        if (CallbackCompletado == null)
            CallbackCompletado = new AsyncCallback(OnCallBackCompleted);

        if (ServiceServiciosCD40 == null)
            ServiceServiciosCD40 = new ServiciosCD40.ServiciosCD40();
    
		string cbReference = ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context", "ClientCallbackError", true);
		string callbackScript = "function CallServer(arg, context) {" + cbReference + "; }";
		ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

		if (!IsPostBack)
		{
			Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
			KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];

			RecuperaIdSectorizacionActiva(s.Value);
			RecuperaTops(s.Value);
			RecuperaSectoresActiva(s.Value);
			RecuperaSectorizaciones(s.Value);
			RecuperaSectoresSectorizaciones(s.Value);
			MuestraEstadoSectorizacion();
		}
		else
			EliminaFilas();
	}

	private void MuestraEstadoSectorizacion()
	{
		if (LblIdSectorizacion.Text == IdSectorizacionActiva)
		{
			LblEstado.ForeColor = System.Drawing.Color.Green;
			LblEstado.Text = "ACTIVA";
		}
		else
		{
			LblEstado.ForeColor = System.Drawing.Color.White;
			LblEstado.Text = "NO ACTIVA";
		}
	}

	private void RecuperaSectoresSectorizaciones(string idSistema)
	{
		foreach (KeyValuePair<string,Dictionary<string, System.Collections.Generic.List<string>>> s in ListaDeSectorizaciones)
		{
			System.Data.DataSet ds = ServiceServiciosCD40.ControladoresRecuperaSectoresSectorizacion(idSistema, s.Key);
			
			if (ds != null && ds.Tables.Count>0)
			{
				foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
				{
					if (!s.Value.ContainsKey((string)dr["IdTop"]))
						s.Value.Add((string)dr["IdTop"], new List<string>());

					if (dr["IdSector"]!= System.DBNull.Value)
					{
						System.Data.DataSet  t = ServiceServiciosCD40.ControladoresRecuperaListaSectores(idSistema, (string)dr["IdNucleo"], (string)dr["IdSector"]);
                        for (int i = 0; t.Tables.Count > 0 && i < t.Tables[0].Rows.Count; i++)
                            s.Value[(string)dr["IdTop"]].Add((string)t.Tables[0].Rows[i]["IdSectorOriginal"]);
					}
				}
			}
		}
	}

	private void RecuperaSectorizaciones(string idSistema)
	{
		System.Data.DataSet ds = ServiceServiciosCD40.ControladoresRecuperaSectorizaciones(idSistema);

        if (ds != null && ds.Tables.Count > 0)
		{
			LBSectorizaciones.Items.Clear();
            LBEliminar.Items.Clear();
            ListaDeSectorizaciones.Clear();

			foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
			{ 
				// Evitar que aparezca la sectorización activa y SACTA
				if (((string)dr["IdSectorizacion"] != ((DateTime)dr["FechaActivacion"]).ToString("dd/MM/yyyy HH:mm:ss")) &&
                    ((string)dr["IdSectorizacion"] != "SACTA") &&
                    ((string)dr["IdSectorizacion"] != "SCV") &&
                    ((string)dr["IdSectorizacion"] != "TEMPORARY_CONTROLLER_SCTZ"))
				{
					LBSectorizaciones.Items.Add((string)dr["IdSectorizacion"]);
                    LBEliminar.Items.Add((string)dr["IdSectorizacion"]);
					TListaDeTops l = new Dictionary<string, TListaDeSectores>();
					ListaDeSectorizaciones[(string)dr["IdSectorizacion"]] = l;
				}
			}
		}
	}

	private void RecuperaIdSectorizacionActiva(string idSistema)
	{
		System.Data.DataSet ds = ServiceServiciosCD40.ControladoresRecuperaIdSectorizacionActiva(idSistema);
		if (ds != null && ds.Tables.Count>0 &&  ds.Tables[0].Rows.Count>0 )
		{
			IdSectorizacionActiva = LblIdSectorizacion.Text = (string)ds.Tables[0].Rows[0]["IdSectorizacion"];
		}
	}

	private void RecuperaSectoresActiva(string idSistema)
	{
		System.Data.DataSet ds = ServiceServiciosCD40.ControladoresRecuperaSectoresActiva(idSistema);
		MuestraSectores(ds);
	}

	private void RecuperaSectoresSectorizacion(string idSectorizacion)
	{
		Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];

		System.Data.DataSet ds = ServiceServiciosCD40.ControladoresRecuperaSectoresSectorizacion(s.Value, idSectorizacion);

		MuestraSectores(ds);
		MuestraEstadoSectorizacion();
	}

	private void RecuperaListaSectores(string idSistema, string idNucleo, string idSector, int indiceTerminal)
	{
		Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement mostrarMantto = config.AppSettings.Settings["MostrarMantenimiento"];
		KeyValueConfigurationElement mostrarVirtuales = config.AppSettings.Settings["MostrarVirtuales"];

		System.Data.DataSet listaSectores = ServiceServiciosCD40.ControladoresRecuperaListaSectores(idSistema, idNucleo, idSector);
		CheckBoxList cblSectores = (CheckBoxList)Table1.Rows[indiceTerminal].Cells[4].FindControl("CBLSectores" + (indiceTerminal + 1));
		for (int i = 0; listaSectores.Tables.Count > 0 && i <  listaSectores.Tables[0].Rows.Count; i++)
		{
			if (cblSectores != null)
			{
                ListItem item = new ListItem((string)listaSectores.Tables[0].Rows[i]["IdSectorOriginal"]);
                item.Value=(string)listaSectores.Tables[0].Rows[i]["Tipo"];
                //item.Attributes.Add("style", "color : red;");
                cblSectores.Items.Add(item);
			}
		}

        ReordenaCheckBoxList(cblSectores, idNucleo);
	}

	private void RecuperaTops(string idSistema)
	{
		System.Data.DataSet ds = ServiceServiciosCD40.ControladoresRecuperaTops(idSistema);
        if (ds != null && ds.Tables.Count > 0)
		{
			NumTops = ds.Tables[0].Rows.Count;

			int i = 0;
			foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
			{
				CheckBox cb = (CheckBox)Table1.Rows[i++].Cells[1].FindControl("CBTop" + i);
				if (cb != null)
				{
					cb.Text = (string)dr["IdTop"];
				}
				//Table1.Rows[i++].Cells[1].Text = (string)dr["IdTop"];			// Identificador del terminal de voz

			}
			int cuantos = Table1.Rows.Count - i;
			for (int numFilas=0; numFilas < cuantos; numFilas++)
			{
				Table1.Rows.RemoveAt(i);
			}
		}
	}

	private void MuestraSectores(System.Data.DataSet ds)
	{
        if (ds != null && ds.Tables.Count > 0)
		{
			ResetTablaSectorizacion();

			foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
			{
				int i = 0;
				foreach (TableRow tr in Table1.Rows)
				{
					CheckBox cb = (CheckBox)tr.Cells[1].FindControl("CBTop" + (i + 1));
					if (cb != null)
					{
						if (cb.Text != (string)dr["IdTop"])
							i++;
						else
							break;
					}
				}

//				while (((CheckBox)Table1.Rows[i].Cells[1].FindControl("CheckBox" + (i + 1))).Text != (string)dr["IdTop"])
//					i++;

				if (dr["IdSector"] != System.DBNull.Value && dr["IdNucleo"] != System.DBNull.Value)
				{
					Table1.Rows[i].Cells[3].Text = (string)dr["IdSector"];			// Nombre de la agrupación
					Table1.Rows[i].Cells[2].Text = (string)dr["IdNucleo"];			// Núcleo
                    if ((string)dr["IdSector"] != "**FS**")
					    RecuperaListaSectores((string)dr["IdSistema"], (string)dr["IdNucleo"], (string)dr["IdSector"], i);
				}
			}
		}
	}

	private void EliminaFilas()
	{
		int numRows = Table1.Rows.Count;

		for (int numFilas = NumTops; numFilas < numRows; numFilas++)
		{
			Table1.Rows.RemoveAt(NumTops);
		}
	}

	private void ResetTablaSectorizacion()
	{
		int i = 0;
		foreach (TableRow tr in Table1.Rows)
		{
			tr.Cells[3].Text = string.Empty;			// Nombre de la agrupación
			tr.Cells[2].Text = string.Empty;			// Núcleo
			CheckBoxList cblSectores = (CheckBoxList)tr.Cells[4].FindControl("CBLSectores" + (i++ + 1));
			cblSectores.Items.Clear();					// La lista de sectores 
		}
	}

	public void OnClick_BtnCargarSectorizacion(object sender, EventArgs e)
	{
        Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
        DateTime fechaActivacion = DateTime.Now;
        
        if (LblIdSectorizacion.Text == string.Empty)
		{
            LblIdSectorizacion.Text = "SCV";

            ServiceServiciosCD40.ControladoresRenombraSectorizacion(s.Value, LblIdSectorizacion.Text);

            ServiceServiciosCD40.ComunicaSectorizacionActiva(config.AppSettings.Settings["ListenIP"].Value, s.Value, LblIdSectorizacion.Text, ref fechaActivacion);

            //RecuperaSectorizaciones(s.Value);
            //RecuperaSectoresSectorizaciones(s.Value);
            RecuperaIdSectorizacionActiva(s.Value);
            MuestraEstadoSectorizacion();
        }
		else
		{
            ServiceServiciosCD40.ComunicaSectorizacionActiva(config.AppSettings.Settings["ListenIP"].Value, s.Value, LblIdSectorizacion.Text, ref fechaActivacion);

            RecuperaIdSectorizacionActiva(s.Value);
            //RecuperaSectoresSectorizaciones(s.Value);
            MuestraEstadoSectorizacion();
        }
	}

	public void OnSelectedIndexChange_LBSectorizaciones(object sender, EventArgs e)
	{
		if (LBSectorizaciones.SelectedItem != null)
		{
			LblIdSectorizacion.Text = LBSectorizaciones.SelectedItem.Text;
			RecuperaSectoresSectorizacion(LblIdSectorizacion.Text);
		}
	}

    public void OnSelectedIndexChange_LBEliminar(object sender, EventArgs e)
    {
        if (LBEliminar.SelectedItem != null)
        {
            ServiciosCD40.Sectorizaciones n = new ServiciosCD40.Sectorizaciones();
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"]; 
            n.IdSistema = s.Value;
            n.IdSectorizacion = LBEliminar.SelectedValue;

            if (ServiceServiciosCD40.DeleteSQL(n) >= 0)
            {
                RecuperaSectorizaciones(n.IdSistema);
                RecuperaSectoresSectorizaciones(n.IdSistema);
            }
        }
    }

	public void RBCheckedChanged(object sender, EventArgs e)
	{
		RadioButton rbTop = (RadioButton)sender;
		int numTop = Convert.ToInt32(rbTop.ID.Remove(0, 11));	// Obtener número de fila a partir del id del radiobuttonXX
		CheckBoxList lbTop = (CheckBoxList)Table1.Rows[numTop - 1].Cells[4].FindControl("CBLSectores" + numTop);
		for (int i = 0; i < NumTops; i++)
		{
			if (i != numTop-1)
			{
				CheckBoxList lbSectores = (CheckBoxList)Table1.Rows[i].Cells[4].FindControl("CBLSectores" + (i + 1));
				if (lbSectores != null)
				{
					// Añadir a la top destino
					foreach (ListItem sector in lbSectores.Items)
					{
						if (sector.Selected)
						{
                            if ((sector.Value == "M" && lbTop.Items.Count > 0) ||
                                (lbTop.Items.FindByValue("M") != null))
                            {
                                rbTop.Checked = false;

                                ModalPopupExtender2.Show();

                                return;
                            }

							lbTop.Items.Add(sector);
							sector.Selected = false;
						}
					}

					foreach (ListItem sector in lbTop.Items)
					{
						lbSectores.Items.Remove(lbSectores.Items.FindByText(sector.Text));
					}
					// Actualizar el núcleo el top destino
					if (Table1.Rows[numTop - 1].Cells[2].Text == string.Empty)
						Table1.Rows[numTop - 1].Cells[2].Text = Table1.Rows[i].Cells[2].Text;
					// Actualizar el núcleo el top origen
					if (lbSectores.Items.Count == 0)
						Table1.Rows[i].Cells[2].Text = string.Empty;
				}
			}
		}

        // ReordenaCheckBoxList(lbTop, Table1.Rows[numTop - 1].Cells[2].Text);
		rbTop.Checked = false;

		LblIdSectorizacion.Text = IdentificaSectorizacion();

		if (LblIdSectorizacion.Text == string.Empty)
		{
			RegeneraSectorizacion();	// IdSectorizacion = TEMPORARY_CONTROLLER_SCTZ
            RecuperaSectoresSectorizacion("TEMPORARY_CONTROLLER_SCTZ");
        }
		else
			RecuperaSectoresSectorizacion(LblIdSectorizacion.Text);
	}

	private string IdentificaSectorizacion()
	{
		bool seguir=true;
		string idSectorizacion=string.Empty;

		foreach (KeyValuePair<string, Dictionary<string, System.Collections.Generic.List<string>>> s in ListaDeSectorizaciones)
		{
			seguir = true;
			idSectorizacion=s.Key;

			for (int i = 0; i < NumTops; i++)
			{
				CheckBoxList cbl = (CheckBoxList)Table1.Rows[i].Cells[4].FindControl("CBLSectores" + (i + 1));
				CheckBox cb = (CheckBox)Table1.Rows[i].Cells[1].FindControl("CBTop" + (i + 1));

				if (cb!= null && (s.Value[cb.Text].Count != cbl.Items.Count && s.Value[cb.Text].Count>0 && s.Value[cb.Text][0]!="**FS**"))
					seguir = false;
				else if (cbl.Items.Count > 0)
				{
					foreach (ListItem item in cbl.Items)
					{
						seguir = null != s.Value[cb.Text].Find(delegate(string obj) { return (obj == item.Text); });
						if (!seguir)
							break;
					}
				}
				if (!seguir)
					break;
			}
			if (seguir)
				break;
		}

		return seguir ? idSectorizacion : string.Empty;
	}

	private void ReordenaCheckBoxList(CheckBoxList lbSectores, string idNucleo)
	{
        System.Text.StringBuilder listaUsuarios = new System.Text.StringBuilder();
        foreach (ListItem s in lbSectores.Items)
            listaUsuarios.AppendFormat("'{0}',", s.Text);

        if (listaUsuarios.Length == 0)
            return;

        listaUsuarios = listaUsuarios.Remove(listaUsuarios.Length - 1, 1);
        lbSectores.Items.Clear();

        // Ordenar la lista por el IDSacta
        Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        KeyValueConfigurationElement sistema = config.AppSettings.Settings["Sistema"];

        System.Data.DataSet d = ServiceServiciosCD40.SectoresNumSactaSorted(sistema.Value, idNucleo, listaUsuarios.ToString());

        if (d != null && d.Tables.Count > 0)
        {
            foreach (System.Data.DataRow ds in d.Tables[0].Rows)
            {
                ListItem i = new ListItem((string)ds[0], (string)ds[1]);
                i.Attributes.Add("style", (string)ds[1] == "M" ? "color:Blue;" : ((string)ds[1] == "R" ? "color:LightGreen;" : "color:orange;"));
                lbSectores.Items.Add(i);
            }
        }
	}

	private void RegeneraSectorizacion()
	{
		int i = 1;
		Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];

		string[] listaNucleos = new string[NumTops];
		string[] listaTops=new string[NumTops];
		string[][] listaSectores = new string[NumTops][];

		foreach (TableRow tr in Table1.Rows)
		{
			int sctr = 0;
			CheckBoxList lista = ((CheckBoxList)tr.Cells[4].FindControl("CBLSectores" + i));
			int cuantos = lista.Items.Count;
			string[] listaSector = new string[cuantos];
			foreach (ListItem ls in lista.Items)
			{
				listaSector[sctr++] = ls.Text;
			}

			listaNucleos[i - 1] = tr.Cells[2].Text;	// Nucleo
			CheckBox cb = (CheckBox)tr.Cells[1].FindControl("CBTop" + i);
			if (cb != null)
				listaTops[i - 1] = cb.Text;
			listaSectores[i - 1] = listaSector;
			i++;
		}

        //Application["Sectorizando"] = true;
        //ServiceServiciosCD40.BeginControladoresRegeneraSectorizacion(s.Value, listaTops, listaSectores, listaNucleos, CallbackCompletado, null);
        ServiceServiciosCD40.ControladoresRegeneraSectorizacion(s.Value, listaTops, listaSectores, listaNucleos);
    }

	protected void BtnAceptarGuardar_Click(object sender, EventArgs e)
	{
		Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
		DateTime fechaActivacion = DateTime.Now;
		LblIdSectorizacion.Text = TBNomSectorizacion.Text;

		ServiceServiciosCD40.ControladoresRenombraSectorizacion(s.Value, LblIdSectorizacion.Text);

        ServiceServiciosCD40.ComunicaSectorizacionActiva(config.AppSettings.Settings["ListenIP"].Value, s.Value, LblIdSectorizacion.Text, ref fechaActivacion);

        RecuperaSectorizaciones(s.Value);
        RecuperaSectoresSectorizaciones(s.Value);
        RecuperaIdSectorizacionActiva(s.Value);
        MuestraEstadoSectorizacion();

        Panel1.Visible = false;
    }

	protected void BtnGuardarSectorizacion_Click(object sender, EventArgs e)
	{
		Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
		KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];

		LblIdSectorizacion.Text = TBNomSectorizacion.Text;
		ServiceServiciosCD40.ControladoresRenombraSectorizacion(s.Value, LblIdSectorizacion.Text);

        RecuperaSectorizaciones(s.Value);
        RecuperaSectoresSectorizaciones(s.Value);

        Panel1.Visible = false;
    }

	protected void BtnGuardar_Click(object sender, EventArgs e)
	{
		//ModalPopupExtender1.Show();
        Panel1.Visible = true;
		BtnGuardarSectorizacion.Visible = true;
		BtnAceptarGuardar.Visible = false;
	}

	protected void CBTodos_CheckedChanged(object sender, EventArgs e)
	{
		CheckBox cbTodos = (CheckBox)sender;
		int numTop = Convert.ToInt32(cbTodos.ID.Remove(0, 7));	// Obtener número de fila a partir del id del checkbox TodosXX
		CheckBoxList lbTop = (CheckBoxList)Table1.Rows[numTop - 1].Cells[4].FindControl("CBLSectores" + numTop);
		foreach (ListItem i in lbTop.Items)
		{
			i.Selected = cbTodos.Checked;
		}
	}

	protected void CBTop_CheckedChanged(object sender, EventArgs e)
	{
		CheckBox cb = (CheckBox)sender;
        if (TopSeleccionada == string.Empty || TopSeleccionada == cb.Text)
        {
            TopSeleccionada = cb.Checked ? cb.Text : string.Empty;
            RecuperaSectoresSectorizacion(LblIdSectorizacion.Text == string.Empty ? "TEMPORARY_CONTROLLER_SCTZ" : LblIdSectorizacion.Text);
        }
        else
        {
            // Intercambiar TOP
            IntercambiaTops(cb.Text);

            int i = 1;
            TopSeleccionada = string.Empty;

            foreach (TableRow tr in Table1.Rows)
            {
                CheckBox cbox = (CheckBox)tr.Cells[1].FindControl("CBTop" + i);
                if (cbox != null)
                {
                    cbox.Checked = false;
                }

                i++;
            }

            LblIdSectorizacion.Text = IdentificaSectorizacion();

            if (LblIdSectorizacion.Text == string.Empty)
            {
                RegeneraSectorizacion();	// IdSectorizacion = TEMPORARY_CONTROLLER_SCTZ
                RecuperaSectoresSectorizacion("TEMPORARY_CONTROLLER_SCTZ");
            }
            else
                RecuperaSectoresSectorizacion(LblIdSectorizacion.Text);
        }
	}

	private void IntercambiaTops(string top)
	{
		int i = 1;
		CheckBoxList cbl1 = null;
		CheckBoxList cbl2 = null;
        int iTopSeleccionada = 0;
        int iTop = 0;

		// Buscar los sectores de la TopSeleccionada
		foreach (TableRow tr in Table1.Rows)
		{
			CheckBox cb1 = (CheckBox)tr.Cells[1].FindControl("CBTop" + i);

            if (cb1 != null && cb1.Text == TopSeleccionada)
            {
                cbl1 = (CheckBoxList)tr.Cells[4].FindControl("CBLSectores" + i);
                iTopSeleccionada = i;
            }
            else if (cb1 != null && cb1.Text == top)
            {
                cbl2 = (CheckBoxList)tr.Cells[4].FindControl("CBLSectores" + i);
                iTop = i;
            }

			i++;
		}

		ListItem[] s1 = new ListItem[cbl1.Items.Count];
		ListItem[] s2 = new ListItem[cbl2.Items.Count];

		cbl1.Items.CopyTo(s1, 0);
		cbl2.Items.CopyTo(s2, 0);

		cbl1.Items.Clear();
		cbl1.Items.AddRange(s2);
		cbl2.Items.Clear();
		cbl2.Items.AddRange(s1);

        // Intercambiar nombre agrupación y núcleo
        //if (iTopSeleccionada != 0 && iTop != 0)
        //{
        //    string aux = Table1.Rows[iTopSeleccionada].Cells[2].Text;
        //    Table1.Rows[iTopSeleccionada].Cells[2].Text = Table1.Rows[iTop].Cells[2].Text;
        //    Table1.Rows[iTop].Cells[2].Text = aux;

        //    aux = Table1.Rows[iTopSeleccionada].Cells[3].Text;
        //    Table1.Rows[iTopSeleccionada].Cells[3].Text = Table1.Rows[iTop].Cells[3].Text;
        //    Table1.Rows[iTop].Cells[3].Text = aux;
        //}

	}

    protected void BtnCancelarMantto_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();

        RecuperaSectoresSectorizacion(LblIdSectorizacion.Text);
    }

	protected void LnkBCancelar_OnClick(object sender, EventArgs e)
	{
		//ModalPopupExtender1.Hide();
        Panel1.Visible = false;
	}

    private void OnCallBackCompleted(IAsyncResult result)
    {
        try
        {
            ServiceServiciosCD40.EndControladoresRegeneraSectorizacion(result);
            Application["Sectorizando"] = false;
        }
        catch (System.Web.Services.Protocols.SoapException )
        {
        }
    }
		
    public string GetCallbackResult()
	{
        //string sectorizando = "False";

        //if (Application["Sectorizando"] != null)
        //    sectorizando = ((bool)Application["Sectorizando"]).ToString();

        //Sectorizando = sectorizando == "True";
        ////TiempoPendienteTransaccion = ServicioParaTransaccion.TiempoPendienteDeTransaccion();		// -1= Transacción timeouted

        int EstadoEnlaceSacta = (int)ServiceServiciosCD40.GetEstadoSacta();

        //string versionActual = ServicioParaTransaccion.GetVersionConfiguracion((string)Session["idsistema"]);
        //bool actualizar=false;
        //if (UltimaVersionSectorizacion != versionActual)
        //{
        //    UltimaVersionSectorizacion = versionActual;
        //    actualizar = true;
        //}

        return string.Format("{0}", EstadoEnlaceSacta);
        //                                    false,
        //                                    (string)GetGlobalResourceObject("Espaniol", "FinTransaccion"),
        //                                    EstadoEnlaceSacta, versionActual,
        //                                    actualizar);
	}

	public void RaiseCallbackEvent(string eventArgument)
	{
		_Command = eventArgument;
	}
}
