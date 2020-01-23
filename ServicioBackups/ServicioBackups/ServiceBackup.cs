using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Runtime.InteropServices;

namespace ServicioBackups
{
	public partial class ServiceBackup : ServiceBase
	{
		[DllImport("Kernel32.dll")]
		public static extern bool Beep(UInt32 frequency, UInt32 duration);
		
		Timer PeriodoComprobacionTareas;
		ReferenciaEstadisticas.EstadisticasSoapClient ServicioEstadisticas;
		string Sistema;
		string PeriodoDias;

		public ServiceBackup()
		{
			InitializeComponent();
		}


		protected override void OnStart(string[] args)
		{
			Sistema = string.Empty;
			PeriodoDias = string.Empty;

			string[] argumentos = Environment.GetCommandLineArgs();
			if (argumentos.Length != 3) return;

			for (int i = 1; i < argumentos.Length; i++)
			{
				switch (argumentos[i].Substring(0, 2).ToLower())
				{
					case "-s":
						Sistema = argumentos[i].Substring(2);
						break;
					case "-p":
						PeriodoDias = argumentos[i].Substring(2);
						break;
					default:
						break;
				}
			}

			if (Sistema != string.Empty && PeriodoDias != string.Empty)
			{
				ServicioEstadisticas = new ReferenciaEstadisticas.EstadisticasSoapClient();

				// Iniciar el timer con la periodicidad establecida.
				PeriodoComprobacionTareas = new Timer(Convert.ToInt32(PeriodoDias) * 24 * 3600 * 1000);
				PeriodoComprobacionTareas.Enabled = true;
				PeriodoComprobacionTareas.Elapsed += new ElapsedEventHandler(PeriodoComprobacionTareas_Elapsed);
			}
			else
			{
				Beep(1000, 500);
			}

			// refEstadisticas.RevisarTareasPendientes(args[0]);
		}

		void PeriodoComprobacionTareas_Elapsed(object sender, ElapsedEventArgs e)
		{
			ServicioEstadisticas.RevisarTareasPendientes(Sistema);
		}

		protected override void OnStop()
		{
			// parar timer
			if (PeriodoComprobacionTareas != null)
				PeriodoComprobacionTareas.Enabled = false;
		}
	}
}
