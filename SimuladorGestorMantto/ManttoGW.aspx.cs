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
using System.Timers;
using Ref_GestorMantenimiento;

public partial class ManttoGW : System.Web.UI.Page
{
	static Ref_GestorMantenimiento.EstadoClienteMantenimiento[] ListaPresencias;
	static Ref_GestorMantenimiento.GestorMantenimiento ServiceGestorMantto;
	static System.Collections.Generic.Dictionary<string, uint> ListaIncidencias;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			ServiceGestorMantto = new GestorMantenimiento();

			Ref_GestorMantenimiento.EstadoClienteMantenimiento[] listaTifx = ServiceGestorMantto.GetEstadoElementosSubsistema(Tipo_Elemento_HW.TEH_TIFX);

			LBPasarelas.Items.Clear();
			foreach (EstadoClienteMantenimiento estado in listaTifx)
			{
				LBPasarelas.Items.Add(estado.IdEh);
			}

			ListaPresencias = ServiceGestorMantto.GetEstadoElementosSubsistema(Tipo_Elemento_HW.TEH_TIFX);
			ListaIncidencias = new System.Collections.Generic.Dictionary<string, uint>();
			GeneraDiccionarioIncidencias(ServiceGestorMantto.GetTodasIncidencias());

			Timer t = new Timer(5000);
			t.Elapsed += new ElapsedEventHandler(PresenciaElementosHardware);
			t.Start();
			
			//GC.KeepAlive(t);
		}
	}

	private void GeneraDiccionarioIncidencias(DataSet dataSet)
	{
		foreach (System.Data.DataRow incidencia in dataSet.Tables[0].Rows)
		{
			ListaIncidencias.Add((string)incidencia["Descripcion"], (uint)incidencia["IdIncidencia"]);
		}
	}

	void PresenciaElementosHardware(object sender, ElapsedEventArgs e)
	{
		ListaPresencias = ServiceGestorMantto.GetEstadoElementosSubsistema(Tipo_Elemento_HW.TEH_TIFX);
	}

	protected void CBConexionRecurso_CheckedChanged(object sender, EventArgs e)
	{
		if (LBPasarelas.SelectedIndex < 0)
			return;

		uint idIncidencia = (((CheckBox)sender).Checked ? ListaIncidencias["Conexión Recurso"] : ListaIncidencias["Desconexión Recurso"]);

		ServiceGestorMantto.CreaEvento(idIncidencia, LBPasarelas.SelectedValue, Tipo_Elemento_HW.TEH_TIFX);
	}

	protected void CBErrorCargaConfiguracion_CheckedChanged(object sender, EventArgs e)
	{
		if (LBPasarelas.SelectedIndex < 0)
			return;

		uint idIncidencia = ListaIncidencias["Error carga configuración"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBPasarelas.SelectedValue, Tipo_Elemento_HW.TEH_TIFX);
	}

	protected void CBErrorProtocoloR2_CheckedChanged(object sender, EventArgs e)
	{
		if (LBPasarelas.SelectedIndex < 0)
			return;

		uint idIncidencia = ListaIncidencias["Error protocolo R2"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBPasarelas.SelectedValue, Tipo_Elemento_HW.TEH_TIFX);
	}

	protected void CBFalloLlamadaR2_CheckedChanged(object sender, EventArgs e)
	{
		if (LBPasarelas.SelectedIndex < 0)
			return;

		uint idIncidencia = ListaIncidencias["Fallo en llamada prueba R2"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBPasarelas.SelectedValue, Tipo_Elemento_HW.TEH_TIFX);
	}

	protected void CBLlamadaR2Ok_CheckedChanged(object sender, EventArgs e)
	{
		if (LBPasarelas.SelectedIndex < 0)
			return;

		uint idIncidencia = ListaIncidencias["Llamada R2 OK"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBPasarelas.SelectedValue, Tipo_Elemento_HW.TEH_TIFX);
	}

	protected void CBErrorProtocoloLCN_CheckedChanged(object sender, EventArgs e)
	{
		if (LBPasarelas.SelectedIndex < 0)
			return;

		uint idIncidencia = ListaIncidencias["Error protocolo LCN"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBPasarelas.SelectedValue, Tipo_Elemento_HW.TEH_TIFX);
	}

	protected void CBActivacionLCN_CheckedChanged(object sender, EventArgs e)
	{
		if (LBPasarelas.SelectedIndex < 0)
			return;

		uint idIncidencia = ListaIncidencias["Activación LCN"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBPasarelas.SelectedValue, Tipo_Elemento_HW.TEH_TIFX);
	}

	protected void CBLCNFueraServicio_CheckedChanged(object sender, EventArgs e)
	{
		if (LBPasarelas.SelectedIndex < 0)
			return;

		uint idIncidencia = ListaIncidencias["LCN fuera de servicio"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBPasarelas.SelectedValue, Tipo_Elemento_HW.TEH_TIFX);
	}

	protected void LBPasarelas_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ListaPresencias == null)
			return;
		/*
		string ehSeleccionado = ((ListBox)sender).SelectedValue;
		Ref_GestorMantenimiento.MantenimientoElementosHardware ehEncontrado =
					Array.Find(ListaPresencias, delegate(Ref_GestorMantenimiento.MantenimientoElementosHardware obj) { return (obj.IdEh == ehSeleccionado); });

		if (ehEncontrado.IdIncidencia == (int)ListaIncidencias["Caída GW"])
		{
			LblGWCaida.Visible = true;
			HabilitaControles(false);
		}
		else
		{
			HabilitaControles(true);
			CBActivacionLCN.Checked = false;
			CBConexionRecurso.Checked = false;
			CBErrorCargaConfiguracion.Checked = false;
			CBErrorProtocoloLCN.Checked = false;
			CBErrorProtocoloR2.Checked = false;
			CBFalloLlamadaR2.Checked = false;
			CBLCNFueraServicio.Checked = false;
			CBLlamadaR2Ok.Checked = false;
			LblGWCaida.Visible = false;
		}*/
	}

	private void HabilitaControles(bool habilita)
	{
		CBActivacionLCN.Enabled = habilita;
		CBConexionRecurso.Enabled = habilita;
		CBErrorCargaConfiguracion.Enabled = habilita;
		CBErrorProtocoloLCN.Enabled = habilita;
		CBErrorProtocoloR2.Enabled = habilita;
		CBFalloLlamadaR2.Enabled = habilita;
		CBLCNFueraServicio.Enabled = habilita;
		CBLlamadaR2Ok.Enabled = habilita;
	}
}
