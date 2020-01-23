using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using NucleoGeneric;

namespace CamAltCtrl
{
    /// <summary>
    /// Clase de control del Automata de la Camara
    /// </summary>
    class CACAutomata
    {
        /// <summary>
        /// Rutina PostMessage de Windows.
        /// </summary>
        /// <param name="hWnd">Handle de Ventana</param>
        /// <param name="msg">Identificador de Mensaje</param>
        /// <param name="wParam">Parámetro WPARAM</param>
        /// <param name="lParam">Parámetro LPARAM</param>
        /// <returns>true, si correcto, false si incorrecto</returns>
        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        /// <summary>
        /// Rutina SendMessahe de Windows
        /// </summary>
        /// <param name="hWnd">Handle de Ventana</param>
        /// <param name="msg">identificador o codigo de mensaje</param>
        /// <param name="wParam">Parámetro WPARAM</param>
        /// <param name="lParam">Parámetro LPARAM</param>
        /// <returns>true si correcto.</returns>
        [DllImport("User32.Dll", EntryPoint = "SendMessageA")]
        static extern bool SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        /// <summary>
        /// Rutina ShowWindow del API de Windwos
        /// </summary>
        /// <param name="hWnd">Handle de Ventana</param>
        /// <param name="nCmdShow">Tipo de Modo de Visualizacion</param>
        /// <returns>true si correcto</returns>
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        /// <summary>
        /// Rutina 'SetWindowPos' del API de Windows
        /// </summary>
        /// <param name="hWnd">Handle de Ventana</param>
        /// <param name="nCmdShow">Tipo de modo de visualizacion</param>
        /// <param name="x">Posicion 'x' de la Ventana</param>
        /// <param name="y">Posición 'y' de la Ventana</param>
        /// <param name="cx">Ancho de la Ventana</param>
        /// <param name="cy">Alto de la Ventana</param>
        /// <param name="flags">Flags de presentacion</param>
        /// <returns>true si correcto</returns>
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, int nCmdShow, int x, int y, int cx, int cy, int flags);
        /// <summary>
        /// Rutina 'FindWindow' del API de Windows
        /// </summary>
        /// <param name="ClassName">Nombre de la Clase a la que pertenece la ventana</param>
        /// <param name="WindowName">Nombre de la Ventana</param>
        /// <returns>Handle de la Ventana o 'null' si no la encuentra</returns>
        [DllImportAttribute("User32.dll")]
        private static extern IntPtr FindWindow(String ClassName, String WindowName);
        /// <summary>
        /// Códigos de Mensajes.
        /// </summary>
        const uint WM_KEYDOWN = 0x100;
        const uint WM_SETFOCUS = 0x0007;
        const uint WM_ACTIVATE = 0x0006;

        /// <summary>
        /// Estados del Automata
        /// </summary>
        enum Estados { REPOSO, WAIT_EVENT1, WAIT_START, WAIT_EVENT2, WAIT_EVENT3, WAIT_EVENT4 }
        /// <summary>
        /// Objeto de control de los tiempos.
        /// </summary>
        System.Timers.Timer timer;
        /// <summary>
        /// Objeto de contro del Puerto Serie.
        /// </summary>
        SerialPort serial_port;
        /// <summary>
        /// Almacena la configuración locla.
        /// </summary>
        CACUserSettings local_settings = new CACUserSettings();
        /// <summary>
        /// Variable que contiene el 'estado' del autómata.
        /// </summary>
        Estados estado;
        /// <summary>
        /// Objeto para el control de la Aplicación que se Arranca o Para.
        /// </summary>
        Process app = new Process();
        /// <summary>
        /// Formato de estado de línea de control del puerto serie.
        /// </summary>
        struct HskLastState
        {
            public bool cd;
            public bool cts;
            public bool dsr;
        }
        /// <summary>
        /// Contiene el último estado de las líneas HSK del puerto serie.
        /// </summary>
        HskLastState last_state;
        /// <summary>
        /// Callback de Mensajes hacia la aplicacion principal.
        /// </summary>
        public event Mensaje Msg;
        /// <summary>
        /// Parte genérica del nombre de base de datos a asignar en cada prueba.
        /// </summary>
        string bdt_name_gen;
        /// <summary>
        /// Contador de ciclos en cada pureba.
        /// </summary>
        int cicle_count;

        /// <summary>
        /// Constructor del autómata:
        /// - Inicializa la variable estado.
        /// - Configura el temporizador.
        /// - Inicializa el puerto serie.
        /// </summary>
        public CACAutomata()
        {
            estado = Estados.REPOSO;
            SetTimer();

            ///
            serial_port = new SerialPort();
            serial_port.PinChanged += new SerialPinChangedEventHandler(serial_port_PinChanged);
        }

        /// <summary>
        /// Rutina para Arrancar o Parar el autómata.
        /// - Arrancar
        ///     - Programa y abre el Puerto serie.
        ///     - Lee el estado inicial de las líneas HSK
        ///     - Inicializa la parte general del nombre de la base de datos.
        ///     - Inicializa el contador de ciclos.
        ///     - Pone el automata a esperar el EVENTO1 (linea1-->ON)
        /// - Parar
        ///     - Resetea y cierra el puerto serie.
        ///     - Para el temporizador.
        ///     - Pone el estado a REPOSO.
        /// </summary>
        public void StartStop()
        {
            try
            {
                if (estado == Estados.REPOSO)
                {   // Arrancar el Automata.
                    estado = Estados.WAIT_EVENT1;
                    SetSerialPort();
                    serial_port.Open();

                    // Obtengo el estado actual de las líneas.    
                    last_state.cd = serial_port.CDHolding;
                    last_state.cts = serial_port.CtsHolding;
                    last_state.dsr = serial_port.DsrHolding;

                    //
                    bdt_name_gen = string.Format("CAC{0}", DateTime.Now.ToString("MMddhhmm"));
                    cicle_count = 1;

                    Msg("Automata Arrancado.");
                    Msg("Automata: WAIT_EVENT1");
                }
                else
                {   // Parar el Automata.
                    ResetSerialPort();
                    serial_port.Close();
                    timer.Stop();
                    estado = Estados.REPOSO;
                    Msg("Automata Detenido.");
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Obtiene si el automata está activo o inactivo.
        /// </summary>
        public bool IsActive
        {
            get { return estado == Estados.REPOSO ? false : true; }
        }

        /// <summary>
        /// Programación del Temporizador.
        ///  - Tiempo 1 segundo.
        /// </summary>
        private void SetTimer()
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(TimeEvent);
            timer.AutoReset = false;
        }

        /// <summary>
        /// Programación del Puerto Serie.
        ///  - Configura el nombre del puerto segun la configuracion local.
        ///  - Pone las líneas de salida al estado marcado por la configuración.
        ///  - Inicializa la estructura de ultimo estado conocido en líneas.
        /// </summary>
        private void SetSerialPort()
        {
            SerialPorts port = local_settings.puerto_serie;
            string strPort = port == SerialPorts.COM1 ? "COM1" :
                port == SerialPorts.COM2 ? "COM2" :
                port == SerialPorts.COM3 ? "COM3" :
                port == SerialPorts.COM4 ? "COM4" :
                port == SerialPorts.COM4 ? "COM5" :
                port == SerialPorts.COM4 ? "COM6" :
                port == SerialPorts.COM4 ? "COM7" :
                port == SerialPorts.COM4 ? "COM8" : "Error";

            serial_port.PortName = strPort;
            serial_port.Handshake = Handshake.None;
            serial_port.RtsEnable = local_settings.ps_salida==HskOut.RTS ? true : false;
            serial_port.DtrEnable = local_settings.ps_salida == HskOut.DTR ? true : false;

            last_state.cd = false;
            last_state.cts = false;
            last_state.dsr = false;
        }

        /// <summary>
        /// Pone a OFF todas las líneas de salida del puerto serie.
        /// </summary>
        private void ResetSerialPort()
        {
            serial_port.RtsEnable = false;
            serial_port.DtrEnable = false;
        }

        /// <summary>
        /// Rutina de tramatamiento de los cambios de líneas de control detectados en el puerto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Tipo de evento</param>
        /// Controla los eventos:
        ///  - Eventos no esperados: Genera excepción
        ///  - Eventos esperados: Los cataloga en función de la configuración (linea1, linea2) y:
        ///     - Si son coherentes con la cofiguración, se invoca el evento correspondiente.
        ///     - Si no son coherentes, se ignoran.
        void serial_port_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            try
            {
                switch (e.EventType)
                {
                    case SerialPinChange.Break:
                        throw new NotImplementedException("Puerto Serie. Evento BREAK");

                    case SerialPinChange.CDChanged:
                        if (last_state.cd == serial_port.CDHolding)
                            break;
                        last_state.cd = serial_port.CDHolding;

                        if (local_settings.ps_entrada_1 == HskIn.CD)
                        {
                            if (serial_port.CDHolding == true)
                                AutEvent_1();
                            else
                                AutEvent_2();
                        }
                        else if (local_settings.ps_entrada_2 == HskIn.CD)
                        {
                            if (serial_port.CDHolding == true)
                                AutEvent_3();
                            else
                                AutEvent_4();
                        }
                        else
                        {
                            // throw new NotImplementedException("Puerto Serie. Evento CD.");
                        }
                        break;

                    case SerialPinChange.CtsChanged:
                        if (last_state.cts == serial_port.CtsHolding)
                            break;
                        last_state.cts = serial_port.CtsHolding;
                        if (local_settings.ps_entrada_1 == HskIn.CTS)
                        {
                            if (serial_port.CtsHolding == true)
                                AutEvent_1();
                            else
                                AutEvent_2();
                        }
                        else if (local_settings.ps_entrada_2 == HskIn.CTS)
                        {
                            if (serial_port.CtsHolding == true)
                                AutEvent_3();
                            else
                                AutEvent_4();
                        }
                        else
                        {
                            // throw new NotImplementedException("Puerto Serie. Evento CTS.");
                        }
                        break;

                    case SerialPinChange.DsrChanged:
                        if (last_state.dsr == serial_port.DsrHolding)
                            break;
                        last_state.dsr = serial_port.DsrHolding;

                        if (local_settings.ps_entrada_1 == HskIn.DSR)
                        {
                            if (serial_port.DsrHolding == true)
                                AutEvent_1();
                            else
                                AutEvent_2();
                        }
                        else if (local_settings.ps_entrada_2 == HskIn.DSR)
                        {
                            if (serial_port.DsrHolding == true)
                                AutEvent_3();
                            else
                                AutEvent_4();
                        }
                        else
                        {
                            // throw new NotImplementedException("Puerto Serie. Evento DSR.");
                        }
                        break;

                    case SerialPinChange.Ring:
                        throw new NotImplementedException("Puerto Serie. Evento RING.");
                    default:
                        throw new NotImplementedException("Puerto Serie. Evento Desconocido");
                }
            }
            catch (Exception x)
            {
                Msg("Excepcion: " + x.Message);
                NucleoGeneric.NGDebug.TraceException("Excepcion en CACAutomata:serial_port_PinChanged. ", x);
            }
        }

        /// <summary>
        /// Rutina de tramatamiento de los cambios de líneas de control detectados en el puerto, para pruebas 
        /// de desarrollo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Evento Esperado</param>
        void serial_port_PinChanged_test(object sender, SerialPinChangedEventArgs e)
        {
            switch (e.EventType)
            {
                case SerialPinChange.Break:
                    Msg("Puerto Serie. Break " + (serial_port.BreakState == true ? "ON" : "OFF"));
                    break;

                case SerialPinChange.CDChanged:
                    if (last_state.cd != serial_port.CDHolding)
                        Msg("Puerto Serie. CD " + (serial_port.CDHolding == true ? "ON" : "OFF"));
                    last_state.cd = serial_port.CDHolding;
                    break;

                case SerialPinChange.CtsChanged:
                    if (last_state.cts != serial_port.CtsHolding)
                        Msg("Puerto Serie. CTS " + (serial_port.CtsHolding == true ? "ON" : "OFF"));
                    last_state.cts = serial_port.CtsHolding;
                    break;

                case SerialPinChange.DsrChanged:
                    if (last_state.dsr != serial_port.DsrHolding)
                        Msg("Puerto Serie. DSR " + (serial_port.DsrHolding == true ? "ON" : "OFF"));
                    last_state.dsr = serial_port.DsrHolding;
                    break;

                case SerialPinChange.Ring:
                    Msg("Puerto Serie. RING ");
                    break;
                default:
                    throw new NotImplementedException("Puerto Serie. Evento Desconocido");
            }
        }

        /// <summary>
        /// Evento de Temporización.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// Gestiona los Tick's de Temporización.
        ///     - Si está en estado 'WAIT_START' Arranca la aplicación configurada.
        void TimeEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();
                if (estado == Estados.WAIT_START)
                {
                    // Arrancar la Aplicacion...
                    Msg("Arrancando SIMGEM...");

                    start_test_simgem(1);

                    Msg("SIMGEM Arrancado");
                    estado = Estados.WAIT_EVENT2;
                    Msg("Automata = WAIT_EVENT2");
                }
                else
                    Msg("Timeout...");
            }
            catch (Exception x)
            {
                Msg("Excepcion: " + x.Message);
                NucleoGeneric.NGDebug.TraceException("Excepcion en CACAutomata::TimeEvent. ", x);
            }
        }

        /// <summary>
        /// Rutina correspondiente al EVENTO1 (linea1 --> ON):
        ///     - Si el automata está en WAIT_EVENT1
        ///         - Programa y arranca el Temporizador según la configuración.
        ///         - Pone al autómata en WAIT_START
        /// </summary>
        void AutEvent_1()
        {
            if (estado == Estados.WAIT_EVENT1)
            {
                timer.Interval = local_settings.TimeToStart * 1000;
                estado = Estados.WAIT_START;
                timer.Start();
                Msg("Automata = WAIT_START");
            }
            else
                Msg("Evento 1. Fuera de Automata");
        }

        /// <summary>
        /// Rutina correspondiente al EVENTO2 (linea1 --> OFF)
        ///     - Si el automata está en WAIT_EVENT2
        ///         - Detiene la Aplicación.
        ///         - Pone al autómata en WAIT_EVENT3
        /// </summary>
        void AutEvent_2()
        {
            if (estado == Estados.WAIT_EVENT2)
            {
                // Parar la Aplicacion.
                Msg("Deteniendo SIMGEM...");

                stop_test_simgem();

                Msg("SIMGEM Detenido");

                estado = Estados.WAIT_EVENT3;
                Msg("Automata = WAIT_EVENT3");
            }
            else
                Msg("Evento 2. Fuera de Automata");
        }

        /// <summary>
        /// Rutina correspondiente al EVENTO3 (linea2 --> ON)
        ///     - Si el automata está en WAIT_EVENT3
        ///         - Arranca la Aplicación.
        ///         - Pone al autómata en WAIT_EVENT4
        /// </summary>
        void AutEvent_3()
        {
            if (estado == Estados.WAIT_EVENT3)
            {
                // Arrancar la Aplicacion.
                Msg("Arrancando SIMGEM...");

                start_test_simgem(2);

                Msg("SIMGEM Arrancado");
                estado = Estados.WAIT_EVENT4;
                Msg("Automata = WAIT_EVENT4");
            }
            else
                Msg("Evento 3. Fuera de Automata");
        }

        /// <summary>
        /// Rutina correspondiente al EVENTO4 (linea2 --> OFF)
        ///     - Si el automata está en WAIT_EVENT4
        ///         - Detiene la Aplicación.
        ///         - Incrementa el contador de ciclos.
        ///         - Pone al autómata en WAIT_EVENT1
        /// </summary>
        void AutEvent_4()
        {
            if (estado == Estados.WAIT_EVENT4)
            {
                // Parar la Aplicacion.
                Msg("Deteniendo SIMGEM...");

                stop_test_simgem();

                Msg("SIMGEM Detenido");

                estado = Estados.WAIT_EVENT1;
                cicle_count++;

                Msg("Automata = WAIT_EVENT1");
            }
            else
                Msg("Evento 4. Fuera de Automata");
        }
   
        /// <summary>
        /// Rutina para el arranque de la Aplicación.
        ///     - Arranca la Aplicación a través del objeto 'process' 'app', según el Path de programa de la 
        ///     configuracion local. Espera a que arranque.
        ///     - Busca y muestra la ventana principal.
        ///     - Envía a la aplicación las teclas correspondientes para arrancar un 'cicle_test'.
        ///     - Cada paso se temporiza con el parámetro 'TimeToNextKey'.
        /// </summary>
        /// <param name="UnoDos">Arranque Primero(1) o segundo(2) del Ciclo.</param>
        void start_test_simgem(int UnoDos)
        {
            // Arramcar la Aplicacion.
            app.StartInfo.FileName = local_settings.Aplicacion;
            app.Start();
            app.WaitForInputIdle();

            Thread.Sleep(local_settings.TimeToNextKey);

            if (ShowWindow(app.MainWindowHandle, 1) == true)
            {
                System.Windows.Forms.SendKeys.SendWait("%ts");
                Thread.Sleep(local_settings.TimeToNextKey);
                // 
                string strName = "{RIGHT}" + string.Format("{0}_{1}_{2}", bdt_name_gen, cicle_count, UnoDos);
                System.Windows.Forms.SendKeys.SendWait(strName);
                Thread.Sleep(local_settings.TimeToNextKey);

                System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            }
            else
            {
                throw new ApplicationException("start_test_simgen: No puedo encontrar la ventana pricipal."); 
            }
        }

        /// <summary>
        /// Rutina para la parada del programa SIMGEN.
        ///     - Localiza la ventana Ppal por su nombre.
        ///     - Localiza el Dialogo por su nombre.
        ///     - Envia ESC al Dialogo.
        ///     - Envia ESC a la ventana.
        ///     - Espera que el proceso se cierre.
        /// </summary>
        void stop_test_simgem()
        {
            IntPtr hWndPpal = FindWindow(null, local_settings.AppName);
            IntPtr hWndDlg = FindWindow(null, local_settings.DlgName);

            NGDebug.Assert(hWndPpal != null, "stop_test_simgen: No puedo encontrar la ventana pricipal.");
            NGDebug.Assert(hWndDlg != null, "stop_test_simgen: No puedo encontrar el cuadro de dialogo.");

            Thread.Sleep(local_settings.TimeToNextKey);
            NGDebug.Assert(PostMessage(hWndDlg, WM_KEYDOWN, 0x1b, 0), "stop_test_simgen: No puedo enviar la tecla ESC al cuadro de dialogo");

            Thread.Sleep(local_settings.TimeToDlgExit);
            NGDebug.Assert(PostMessage(hWndPpal, WM_KEYDOWN, 0x1b, 0), "stop_test_simgen: No puedo enviar la tecla ESC a la ventana principal");

            Thread.Sleep(local_settings.TimeToNextKey);
            app.WaitForExit();
        }
    }
}
