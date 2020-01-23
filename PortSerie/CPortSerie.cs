
#region Bloque Using
using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;
#endregion

namespace NucleoSw
{
/// <summary>
/// Clase de Control de Puertos Serie
/// </summary>
/// 
public class CPortSerie
{
	#region Constantes

	private const uint PURGE_TXABORT = 0x0001;  // Kill the pending/current writes to the comm port.
	private const uint PURGE_RXABORT = 0x0002;  // Kill the pending/current reads to the comm port.
	private const uint PURGE_TXCLEAR = 0x0004;  // Kill the transmit queue if there.
	private const uint PURGE_RXCLEAR = 0x0008;  // Kill the typeahead buffer if there.

	private const uint SETXOFF  = 1;	// Simulate XOFF received
	private const uint SETXON   = 2;	// Simulate XON received
	private const uint SETRTS	= 3;	// Set RTS high
	private const uint CLRRTS	= 4;	// Set RTS low
	private const uint SETDTR	= 5;	// Set DTR high
	private const uint CLRDTR	= 6;	// Set DTR low
	private const uint SETBREAK	= 8;	// Set the device break line.
	private const uint CLRBREAK	= 9;	// Clear the device break line.

	private const uint MS_CTS_ON  = 0x0010;
	private const uint MS_DSR_ON  = 0x0020;
	private const uint MS_RING_ON = 0x0040;
	private const uint MS_RLSD_ON = 0x0080;

	private const uint FILE_FLAG_OVERLAPPED  = 0x40000000;
	private const uint FILE_ATTRIBUTE_NORMAL = 0x00000000;

	private const uint OPEN_EXISTING = 3;

	private const int  INVALID_HANDLE_VALUE = -1;

	private const uint GENERIC_READ = 0x80000000;
	private const uint GENERIC_WRITE = 0x40000000;

	private const uint ERROR_SUCCESS = 0;
	private const uint ERROR_OPERATION_ABORTED = 995;
	private const uint ERROR_IO_PENDING = 997;

	#endregion Constantes

	#region Bloque Atributos

	private IntPtr m_hFile = IntPtr.Zero;
	private string m_strPort;
	private string m_strSetup;
	private bool   m_bIsOpen;
	private DCB    m_dcb;

	#endregion

	#region Bloque Clase DCB

	public struct DCB 
	{
	#region Atributos DCB

	public int DCBlength;
	public uint BaudRate;
	public uint Flags;
	public ushort wReserved;
	public ushort XonLim;
	public ushort XoffLim;
	public byte ByteSize;
	public byte Parity;
	public byte StopBits;
	public sbyte XonChar;
	public sbyte XoffChar;
	public sbyte ErrorChar;
	public sbyte EofChar;
	public sbyte EvtChar;
	public ushort wReserved1;

	#endregion

	#region Properties DCB

	//---------------------------------------------------------------------
	//
	public uint fBinary 
		{ 
		get 
			{ return Flags&0x0001; } 
		set 
			{ Flags = Flags & ~1U | value; } 
		}
		
	//---------------------------------------------------------------------
	//
	public uint fParity 
		{ 
		get 
			{ return (Flags>>1)&1; }
		set 
			{ Flags = Flags & ~(1U << 1) | (value << 1); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fOutxCtsFlow 
		{ 
		get 
			{ return (Flags>>2)&1; }
		set 
			{ Flags = Flags & ~(1U << 2) | (value << 2); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fOutxDsrFlow 
		{ 
		get 
			{ return (Flags>>3)&1; }
		set 
			{ Flags = Flags & ~(1U << 3) | (value << 3); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fDtrControl 
		{ 
		get 
			{ return (Flags>>4)&3; }
		set 
			{ Flags = Flags & ~(3U << 4) | (value << 4); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fDsrSensitivity 
		{ 
		get 
			{ return (Flags>>6)&1; }
		set 
			{ Flags = Flags & ~(1U << 6) | (value << 6); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fTXContinueOnXoff 
		{ 
		get 
			{ return (Flags>>7)&1; }
		set 
			{ Flags = Flags & ~(1U << 7) | (value << 7); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fOutX 
		{ 
		get 
			{ return (Flags>>8)&1; }
		set 
			{ Flags = Flags & ~(1U << 8) | (value << 8); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fInX 
		{ 
		get 
			{ return (Flags>>9)&1; }
		set 
			{ Flags = Flags & ~(1U << 9) | (value << 9); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fErrorChar 
		{ 
		get 
			{ return (Flags>>10)&1; }
		set 
			{ Flags = Flags & ~(1U << 10) | (value << 10); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fNull 
		{ 
		get 
			{ return (Flags>>11)&1; }
		set 
			{ Flags = Flags & ~(1U << 11) | (value << 11); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fRtsControl 
		{ 
		get 
			{ return (Flags>>12)&3; }
		set 
			{ Flags = Flags & ~(3U << 12) | (value << 12); } 
		}

	//---------------------------------------------------------------------
	//
	public uint fAbortOnError 
		{ 
		get 
			{ return (Flags>>13)&1; }
		set 
			{ Flags = Flags & ~(1U << 13) | (value << 13); } 
		}
	#endregion Properties DCB

	#region Methods DCB

	//---------------------------------------------------------------------
	//
	public override string ToString() 
		{
		return "DCBlength: " + DCBlength + "\r\n" +
				"BaudRate: " + BaudRate + "\r\n" +
				"fBinary: " + fBinary + "\r\n" +
				"fParity: " + fParity + "\r\n" +
				"fOutxCtsFlow: " + fOutxCtsFlow + "\r\n" +
				"fOutxDsrFlow: " + fOutxDsrFlow + "\r\n" +
				"fDtrControl: " + fDtrControl + "\r\n" +
				"fDsrSensitivity: " + fDsrSensitivity + "\r\n" +
				"fTXContinueOnXoff: " + fTXContinueOnXoff + "\r\n" +
				"fOutX: " + fOutX + "\r\n" +
				"fInX: " + fInX + "\r\n" +
				"fErrorChar: " + fErrorChar + "\r\n" +
				"fNull: " + fNull + "\r\n" +
				"fRtsControl: " + fRtsControl + "\r\n" +
				"fAbortOnError: " + fAbortOnError + "\r\n" +
				"XonLim: " + XonLim + "\r\n" +
				"XoffLim: " + XoffLim + "\r\n" +
				"ByteSize: " + ByteSize + "\r\n" +
				"Parity: " + Parity + "\r\n" +
				"StopBits: " + StopBits + "\r\n" +
				"XonChar: " + XonChar + "\r\n" +
				"XoffChar: " + XoffChar + "\r\n" +
				"EofChar: " + EofChar + "\r\n" +
				"EvtChar: " + EvtChar + "\r\n";
		}

	#endregion Methods DCB

	}

	#endregion

	#region Bloque SerialTimeouts

	public struct SerialTimeouts 
	{
	#region Attributes SerialTimeouts

	public int ReadIntervalTimeout;
	public int ReadTotalTimeoutMultiplier;
	public int ReadTotalTimeoutConstant;
	public int WriteTotalTimeoutMultiplier;
	public int WriteTotalTimeoutConstant;

	#endregion Attributes SerialTimeouts

	#region Constructors

	public SerialTimeouts(int r1, int r2, int r3, int w1, int w2) 
		{
		ReadIntervalTimeout = r1;
		ReadTotalTimeoutMultiplier = r2;
		ReadTotalTimeoutConstant = r3;
		WriteTotalTimeoutMultiplier = w1;
		WriteTotalTimeoutConstant = w2;
		}

	#endregion Constructors

	#region Methods

	public override string ToString() 
		{
		return "ReadIntervalTimeout: " + ReadIntervalTimeout + "\r\n" +
				"ReadTotalTimeoutMultiplier: " + ReadTotalTimeoutMultiplier + "\r\n" +
				"ReadTotalTimeoutConstant: " + ReadTotalTimeoutConstant + "\r\n" +
				"WriteTotalTimeoutMultiplier: " + WriteTotalTimeoutMultiplier + "\r\n" +
				"WriteTotalTimeoutConstant: " + WriteTotalTimeoutConstant + "\r\n";
		}

	#endregion Methods
	}

	#endregion

	#region Constructores
	//---------------------------------------------------------------------
	//
	public CPortSerie()
		{
		m_bIsOpen=false;
		}
	#endregion

	#region Propiedades (HSK)

	//---------------------------------------------------------------------
	// Linea de HSK DTR
	public uint Dtr
	{
	get
		{
		if (!m_bIsOpen) return 0;
		if (!GetCommState(m_hFile, ref m_dcb))
			throw new CPortSerieException(m_strPort, "get Dtr (GetCommState)", GetLastError());
		return m_dcb.fDtrControl;
		}
	set
		{
		if (!m_bIsOpen) return;
		if (!GetCommState(m_hFile, ref m_dcb))
			throw new CPortSerieException(m_strPort, "set Dtr (SetCommState)", GetLastError());
		m_dcb.fDtrControl = value;
		if (!SetCommState(m_hFile, ref m_dcb))
			throw new CPortSerieException(m_strPort, "set Dtr (GetCommState)", GetLastError());
		}
	}

	//---------------------------------------------------------------------
	// Linea de HSK RTS
	public uint Rts
	{
	get
		{
		if (!m_bIsOpen) return 0;
		if (!GetCommState(m_hFile, ref m_dcb))
			throw new CPortSerieException(m_strPort, "get Rts (GetCommState)", GetLastError());
		return m_dcb.fRtsControl;
		}
	set
		{
		if (!m_bIsOpen) return;
		if (!GetCommState(m_hFile, ref m_dcb))
			throw new CPortSerieException(m_strPort, "set Rts (GetCommState)", GetLastError());
		m_dcb.fRtsControl = value;
		if (!SetCommState(m_hFile, ref m_dcb))
			throw new CPortSerieException(m_strPort, "set Rts (GetCommState)", GetLastError());
		}
	}

	//---------------------------------------------------------------------
	// Linea de HSK DSR
	public uint Dsr
	{
	get
		{
		if (!m_bIsOpen) return 0;

		uint dwStatus;
		if (!GetCommModemStatus(m_hFile, out dwStatus))
			throw new CPortSerieException(m_strPort, "get Dsr (GetCommState)", GetLastError());
		if ((dwStatus & MS_DSR_ON)>0)
			return 1;
		else
			return 0;
		}
	set
		{
		}
	}

	//---------------------------------------------------------------------
	// Linea de HSK CTS
	public uint Cts
	{
		get
		{
			if (!m_bIsOpen) return 0;

			uint dwStatus;
			if (!GetCommModemStatus(m_hFile, out dwStatus))
				throw new CPortSerieException(m_strPort, "get Cts (GetCommState)", GetLastError());
			if ((dwStatus & MS_CTS_ON)>0)
				return 1;
			else
				return 0;
		}
		set
		{
		}
	}

	//---------------------------------------------------------------------
	// Linea de HSK DSR
	public uint Cd
	{
		get
		{
			if (!m_bIsOpen) return 0;

			uint dwStatus;
			if (!GetCommModemStatus(m_hFile, out dwStatus))
				throw new CPortSerieException(m_strPort, "get Cd (GetCommState)", GetLastError());
			if ((dwStatus & MS_RLSD_ON)>0)
				return 1;
			else
				return 0;
		}
		set
		{
		}
	}
	
	//---------------------------------------------------------------------
	// Puerto Abierto
	public bool IsOpen
	{
		get {return m_bIsOpen;}
	}
	#endregion

	#region Metodos Publicos

	//---------------------------------------------------------------------
	//
	public void Open(string strPort, string strSetup)
	{
	if (m_bIsOpen || m_hFile != IntPtr.Zero) 
		throw new CPortSerieException(m_strPort, "Open <Puerto Ya Abierto", 0);

	m_strPort = strPort;
	m_strSetup = strSetup;
		
	m_hFile = CreateFile(strPort, (uint)((GENERIC_READ)|(GENERIC_WRITE)), 0, 0, 
						OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL /* |FILE_FLAG_OVERLAPPED*/, 0);
	if (m_hFile.ToInt32() == INVALID_HANDLE_VALUE) 
		{
		m_hFile = IntPtr.Zero;
		throw new CPortSerieException(m_strPort, "Open (CreateFile)", GetLastError());
		}

		// Configurar el Puerto con el Setup.
	if (!GetCommState(m_hFile, ref m_dcb) )
		throw new CPortSerieException(m_strPort, "Open (GetCommState)", GetLastError());
	if (!BuildCommDCB(strSetup, ref m_dcb) )
		throw new CPortSerieException(m_strPort, "Open (BuildCommDCB)", GetLastError());

	m_dcb.fDtrControl = 0;
	m_dcb.fRtsControl = 0;

	if (!SetCommState(m_hFile, ref m_dcb))
		throw new CPortSerieException(m_strPort, "Open (SetCommState)", GetLastError());

//	SetTimeouts(100, 0, 500, 200, 100);
	SetTimeouts(10, 0, 50, 200, 100);

	m_bIsOpen = true;
	}

	public void Open(string strPort)
	{
	Open(strPort, "9600,n,8,1");
	}

	public void Close()
	{
	if (!m_bIsOpen || m_hFile == IntPtr.Zero) 
		throw new CPortSerieException(m_strPort, "Close (Puerto No Abierto)", 0);

	CloseHandle(m_hFile);
	m_hFile = IntPtr.Zero;
	m_bIsOpen = false;
	}

	public int  GetChar()
	{
	if (!m_bIsOpen || m_hFile == IntPtr.Zero) 
		throw new CPortSerieException(m_strPort, "GetChar (Puerto No Abierto)", 0);
	unsafe
		{
		byte leido;
		uint leidos;

		if (!ReadFile(m_hFile, &leido, 1, out leidos, null))
			throw new CPortSerieException(m_strPort, "GetChar (ReadFile)", GetLastError());

		return leidos==0 ? -1 : leido;
		}
	}

	public bool Receive(byte [] bfRec, out int nRec)
	{
	int leido;
	int size = bfRec.GetLength(0);
	
	nRec = 0;
	do
		{
		leido = GetChar();
		if (leido != -1 && nRec < size)
			bfRec[nRec++] = (byte )leido;
		} while (leido != -1);
	return nRec > 0 ? true : false;
	}

	public bool Send(byte [] bfSend)
	{
	if (!m_bIsOpen || m_hFile == IntPtr.Zero) 
		throw new CPortSerieException(m_strPort, "Close (Puerto No Abierto)", 0);
		
	uint size = (uint )bfSend.GetLength(0);
	uint escritos;
	GCHandle gchBuffer = GCHandle.Alloc(bfSend, GCHandleType.Pinned);
	unsafe 
		{
		byte* data = (byte*)((int)gchBuffer.AddrOfPinnedObject());
		if (!WriteFile(m_hFile, data, size, out escritos, null))
			throw new CPortSerieException(m_strPort, "Send (WriteFile)", GetLastError());
		}
	return escritos==size ? true : false;
	}


	public void Send(byte cmd)
	{
		byte[] trama = new byte[] { cmd };
		Send(trama);
	}

	#endregion

	#region Metodos Internos

	public void SetTimeouts(int ReadIntervalTimeout,
							int ReadTotalTimeoutMultiplier,
							int ReadTotalTimeoutConstant, 
							int WriteTotalTimeoutMultiplier,
							int WriteTotalTimeoutConstant) 
	{
	SerialTimeouts Timeouts = new SerialTimeouts(ReadIntervalTimeout,
												ReadTotalTimeoutMultiplier,
												ReadTotalTimeoutConstant, 
												WriteTotalTimeoutMultiplier,
												WriteTotalTimeoutConstant);
	unsafe {
		if (!SetCommTimeouts(m_hFile, ref Timeouts))
			throw new CPortSerieException(m_strPort, "SetTimeouts (SetCommTimeouts)", GetLastError());
		}
	}


	#endregion

	#region Imports

	[DllImport("kernel32.dll", EntryPoint="CreateFileW",  SetLastError=true,
			 CharSet=CharSet.Unicode, ExactSpelling=true)]
	static extern IntPtr CreateFile(string filename, uint access, uint sharemode, uint security_attributes, uint creation, uint flags, uint template);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern bool CloseHandle(IntPtr handle);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern unsafe bool ReadFile(IntPtr hFile, byte* lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, NativeOverlapped* lpOverlapped);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern unsafe bool WriteFile(IntPtr hFile, byte* lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, NativeOverlapped* lpOverlapped);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern bool SetCommTimeouts(IntPtr hFile, ref SerialTimeouts lpCommTimeouts);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern bool SetCommState(IntPtr hFile, ref DCB lpDCB);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern bool GetCommState(IntPtr hFile, ref DCB lpDCB);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern bool BuildCommDCB(string def, ref DCB lpDCB);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern int GetLastError();

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern bool FlushFileBuffers(IntPtr hFile);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern bool PurgeComm(IntPtr hFile, uint dwFlags);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern bool EscapeCommFunction(IntPtr hFile, uint dwFunc);

	[DllImport("kernel32.dll", SetLastError=true)]
	static extern bool GetCommModemStatus(IntPtr hFile, out uint modemStat);

	#endregion Imports

	}

/// <summary>
/// Gestion de Excepciones del Puerto Serie.
/// </summary>
public class CPortSerieException : ApplicationException
	{
	#region Datos Internos

	string m_strPort;
	long   m_code;

	#endregion

	#region Constructores

	public CPortSerieException()
	{
	}

	public CPortSerieException(string msg) : base(msg)
	{
	}

	public CPortSerieException(string msg, long error) : base (msg)
	{
	m_code = error;
	}

	public CPortSerieException(string strPort, string msg, long error) : base (msg)
	{
	m_code = error;
	m_strPort = strPort;
	}

	#endregion

	#region Metodos Publicos
	public override string ToString()
	{
	return "Excepcion en Puerto " + m_strPort + " " + Message + "Error: " + m_code.ToString();
	}

	#endregion
	}
}

