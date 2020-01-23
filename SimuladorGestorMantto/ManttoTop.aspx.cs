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

public partial class ManttoTop : System.Web.UI.Page
{
	static Ref_GestorMantenimiento.EstadoClienteMantenimiento[] ListaPresencias;
	static Ref_GestorMantenimiento.GestorMantenimiento ServiceGestorMantto;
	static System.Collections.Generic.Dictionary<string, uint> ListaIncidencias;
	
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			ServiceGestorMantto = new GestorMantenimiento();

			Ref_GestorMantenimiento.EstadoClienteMantenimiento[] listaTops = ServiceGestorMantto.GetEstadoElementosSubsistema(Tipo_Elemento_HW.TEH_TOP);

			LBTerminalesOperador.Items.Clear();
			foreach (EstadoClienteMantenimiento estado in listaTops)
			{
				LBTerminalesOperador.Items.Add(estado.IdEh);
			}

			ListaPresencias = ServiceGestorMantto.GetEstadoElementosSubsistema(Tipo_Elemento_HW.TEH_TOP);
			ListaIncidencias = new System.Collections.Generic.Dictionary<string, uint>();
			GeneraDiccionarioIncidencias(ServiceGestorMantto.GetTodasIncidencias());

			Timer t = new Timer(5000);
			t.Elapsed += new ElapsedEventHandler(PresenciaElementosHardware);
			t.Start();
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
		ListaPresencias = ServiceGestorMantto.GetEstadoElementosSubsistema(Tipo_Elemento_HW.TEH_TOP);
	}

	protected void LBTerminalesOperador_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (ListaPresencias == null)
			return;
		/*
		string ehSeleccionado = ((ListBox)sender).SelectedValue;
		Ref_GestorMantenimiento.MantenimientoElementosHardware ehEncontrado =
					Array.Find(ListaPresencias, delegate(Ref_GestorMantenimiento.EstadoClienteMantenimiento obj) { return (obj.IdEh == ehSeleccionado); });

		if (ehEncontrado.IdIncidencia == (int)ListaIncidencias["Ca�da TOP"])
		{
			LblGWCaida.Visible = true;
			HabilitaControles(false);
		}
		else
		{
			HabilitaControles(true);
			CBConexionAltavoz.Checked = false;
			CBConexionJackAyudante.Checked = false;
			CBConexionJackEjecutivo.Checked = false;
			CBErrorCargaConfiguracion.Checked = false;
			CBPanelEnStandby.Checked = false;
			LblGWCaida.Visible = false;
		}*/
	}

	private void HabilitaControles(bool habilita)
	{
		CBConexionAltavoz.Enabled = habilita;
		CBConexionJackAyudante.Enabled = habilita;
		CBConexionJackEjecutivo.Enabled = habilita;
		CBErrorCargaConfiguracion.Enabled = habilita;
		CBPanelEnStandby.Enabled = habilita;
	}

	protected void CBConexionJackEjecutivo_CheckedChanged(object sender, EventArgs e)
	{
		if (LBTerminalesOperador.SelectedIndex < 0)
			return;

		uint idIncidencia = ((CheckBox)sender).Checked ? ListaIncidencias["Conexi�n jacks ejecutivo"] : ListaIncidencias["Desconexi�n jacks ejecutivo"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBTerminalesOperador.SelectedValue, Tipo_Elemento_HW.TEH_TOP);
	}
	
	protected void CBConexionJackAyudante_CheckedChanged(object sender, EventArgs e)
	{
		if (LBTerminalesOperador.SelectedIndex < 0)
			return;

		uint idIncidencia = ((CheckBox)sender).Checked ? ListaIncidencias["Conexi�n jacks ayudante"] : ListaIncidencias["Desconexi�n jacks ayudante"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBTerminalesOperador.SelectedValue, Tipo_Elemento_HW.TEH_TOP);
	}
	
	protected void CBConexionAltavoz_CheckedChanged(object sender, EventArgs e)
	{
		if (LBTerminalesOperador.SelectedIndex < 0)
			return;

		uint idIncidencia = ((CheckBox)sender).Checked ? ListaIncidencias["Conexi�n altavoz"] : ListaIncidencias["Desconexi�n altavoz"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBTerminalesOperador.SelectedValue, Tipo_Elemento_HW.TEH_TOP);
	}

	protected void CBPanelEnStandby_CheckedChanged(object sender, EventArgs e)
	{
		if (LBTerminalesOperador.SelectedIndex < 0)
			return;

		uint idIncidencia = ((CheckBox)sender).Checked ? ListaIncidencias["Panel pasa a standby"] : ListaIncidencias["Panel pasa a operaci�n"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBTerminalesOperador.SelectedValue, Tipo_Elemento_HW.TEH_TOP);
	}

	protected void CBErrorCargaConfiguracion_CheckedChanged(object sender, EventArgs e)
	{
		if (LBTerminalesOperador.SelectedIndex < 0)
			return;
		uint idIncidencia = ListaIncidencias["Error carga configuraci�n"];

		ServiceGestorMantto.CreaEvento(idIncidencia, LBTerminalesOperador.SelectedValue, Tipo_Elemento_HW.TEH_TOP);
	}
}
