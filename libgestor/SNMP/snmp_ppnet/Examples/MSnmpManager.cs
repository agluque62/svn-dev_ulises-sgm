////////////////////////////////////////////////////////////////////////////////
//
// SNMP++.NET v. 1.21 (2006-07-26 15:30:00)
//
// Copyright (c) 2003-2004 Military Communication Institute, Zegrze, Poland
// Author: Marek Malowidzki
//
// This software is based on SNMP++ from Jochen Katz, Frank Fock,
// which is in turn based on SNMP++2.6 from Hewlett Packard:
//
// Copyright (c) 2001-2003 Jochen Katz, Frank Fock
//
// Copyright (c) 1996
// Hewlett-Packard Company
//
// ATTENTION: USE OF THIS SOFTWARE IS SUBJECT TO THE FOLLOWING TERMS.
// Permission to use, copy, modify, distribute and/or sell this software 
// and/or its documentation is hereby granted without fee. User agrees 
// to display the above copyright notice and this license notice in all 
// copies of the software and any documentation of the software. User 
// agrees to assume all liability for the use of the software; 
// Hewlett-Packard, Jochen Katz and Military Communication Institute make
// no representations about the suitability of this software for any purpose.
// It is provided  "AS-IS" without warranty of any kind, either express
// or implied. User hereby grants a royalty-free license to any and all
// derivatives based upon this software code base. 
// 
////////////////////////////////////////////////////////////////////////////////
/*_############################################################################
  _## 
  _##  SNMP++v3.2.9c
  _##  -----------------------------------------------
  _##  Copyright (c) 2001-2003 Jochen Katz, Frank Fock
  _##
  _##  This software is based on SNMP++2.6 from Hewlett Packard:
  _##  
  _##    Copyright (c) 1996
  _##    Hewlett-Packard Company
  _##  
  _##  ATTENTION: USE OF THIS SOFTWARE IS SUBJECT TO THE FOLLOWING TERMS.
  _##  Permission to use, copy, modify, distribute and/or sell this software 
  _##  and/or its documentation is hereby granted without fee. User agrees 
  _##  to display the above copyright notice and this license notice in all 
  _##  copies of the software and any documentation of the software. User 
  _##  agrees to assume all liability for the use of the software; 
  _##  Hewlett-Packard and Jochen Katz make no representations about the 
  _##  suitability of this software for any purpose. It is provided 
  _##  "AS-IS" without warranty of any kind, either express or implied. User 
  _##  hereby grants a royalty-free license to any and all derivatives based
  _##  upon this software code base. 
  _##  
  _##########################################################################*/
/*===================================================================

  Copyright (c) 1999
  Hewlett-Packard Company

  ATTENTION: USE OF THIS SOFTWARE IS SUBJECT TO THE FOLLOWING TERMS.
  Permission to use, copy, modify, distribute and/or sell this software
  and/or its documentation is hereby granted without fee. User agrees
  to display the above copyright notice and this license notice in all
  copies of the software and any documentation of the software. User
  agrees to assume all liability for the use of the software; Hewlett-Packard
  makes no representations about the suitability of this software for any
  purpose. It is provided "AS-IS without warranty of any kind,either express
  or implied. User hereby grants a royalty-free license to any and all
  derivatives based upon this software code base.
=====================================================================*/

////////////////////////////////////////////////////////////////////////////////
// The command line utility for SNMP++.NET.
//
// Author:	Marek Malowidzki	2003, 2004
// Send comments, suggestions and bug reports to maom_onet@poczta.onet.pl.
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Org.Snmp.Snmp_pp;

namespace Org.Snmp.Snmp_pp
{
	////////////////////////////////////////////////////////////////////////////
	// Barrier - a simple barrier for threads synchronization.
	////////////////////////////////////////////////////////////////////////////
	
	internal sealed class Barrier
	{
		private readonly object	lock_ = new object();
		private readonly uint	n_;
		private uint			c_;

		public Barrier(uint n) { n_ = n; }

		public uint Size  { get { return n_; } }

		public uint Count { get { return c_; } }

		public void Enter()
		{
			lock (lock_)
			{
				if (++c_ == n_)
				{
					c_ = 0;
					Monitor.PulseAll(lock_);
				}
				else
				{
					Monitor.Wait(lock_);
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////
	// MemoryStats - this simple class drops memory usage statistics to files.
	////////////////////////////////////////////////////////////////////////////

	internal sealed class MemoryStats
	{
		private readonly string	filePfx_;
		private volatile int	nStats_;

		public MemoryStats(string filePfx) { filePfx_ = filePfx; }

		public int StatsCount { get { return nStats_; } }

		public void Collect()
		{
			DateTime start = DateTime.Now;
			try
			{
				using (StreamWriter mn = new StreamWriter(filePfx_ + ".managed"),
									hm = new StreamWriter(filePfx_ + ".heaps"))
				{
					mn.AutoFlush = true;
					hm.AutoFlush = true;

					while (true)
					{
						long managedMemory = GC.GetTotalMemory(false),
							 heapsMemory   = MemoryManager.GetHeapsMemory();

						TimeSpan tspan = DateTime.Now.Subtract(start);
						double tmsec = tspan.TotalMilliseconds,
								sec  = tmsec / 1000.0;
						int msec = (int) ((long) tmsec % 1000);

						mn.WriteLine("{0}\t{1}", sec, managedMemory);
						if (heapsMemory < 0)
							hm.Write("# ");		// write error code as a comment
						hm.WriteLine("{0}\t{1}", sec, heapsMemory);
						nStats_++;

						Thread.Sleep(1000 - msec);
					}
				}
			}
			catch (ThreadInterruptedException) {}
		}

		public void WaitForNextStat()
		{
			int nstat = nStats_;
			do
			{
				Thread.Sleep(100);
			}
			while (nstat == nStats_);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////
	// Manager -  this is the class that demonstrates the use of the SNMP++.NET
	// API. It may look excessively complex as it tries to present most options
	// and scenarios but the real SNMP code is really straightforward.
	////////////////////////////////////////////////////////////////////////////

	public sealed class Manager
	{
		private readonly int		id_;
		private readonly Snmp		snmp_;
		private readonly SnmpTarget target_;
		private readonly Pdu		pdu_;
		private readonly OperType	operType_;
		private readonly TableReader.GetTableOptions tableOptions_;
		private readonly uint		nRepeats_;
		private readonly bool		asyncSync_,
									debug_;
		private readonly Barrier	barrier_;
		private volatile int		nCalls_,
									nRepd_;
		private Exception			exc_;

		private enum OperType {Simple, Walk, Table};

		private Manager(int id,
			Snmp snmp, SnmpTarget target, Pdu pdu, OperType operType,
			ref TableReader.GetTableOptions tableOptions, Barrier barrier,
			uint nRepeats, bool asyncSync, bool debug)
		{
			id_        = id;
			snmp_      = snmp;
			target_    = target;
			pdu_       = pdu;
			operType_  = operType;
			tableOptions_ = tableOptions;
			barrier_   = barrier;
			nRepeats_  = nRepeats;
			asyncSync_ = asyncSync;
			debug_     = debug;
		}

		private Manager Clone(int id)
		{
			TableReader.GetTableOptions to = tableOptions_;
			return new Manager(id, snmp_, target_, pdu_,
						operType_, ref to,
						barrier_, nRepeats_, asyncSync_, debug_);
		}

		private void DoSnmp()
		{
			barrier_.Enter();
			try
			{
				TableReader.GetTableOptions to = tableOptions_;
				int count = DoSnmp(snmp_, target_, pdu_,
								operType_, ref to,
								nRepeats_, asyncSync_, debug_);
				lock (barrier_)
				{
					nCalls_ += count;
				}
			}
			catch (Exception e)
			{
				lock (barrier_)
				{
					exc_ = e;
				}
				Console.Error.WriteLine(e);
			}
			finally
			{
				barrier_.Enter();
			}
		}

		private void AsyncDoSnmp()
		{
			if (operType_ != OperType.Table)
			{
				AsyncDoSnmp(snmp_, target_, pdu_,
					new AsyncCallback(ResponseCallback), this);
			}
			else
			{
				Oid[] columnOids = GetColumnOids(pdu_);
				Pdu pdu = pdu_.Clone(new Vb[0]);	// not really needed; for test purposes
				TableReader.BeginGetTable(snmp_, pdu, target_,
					columnOids, tableOptions_,
					new AsyncCallback(TableResponseCallback), columnOids);
			}
		}

		private void ResponseCallback(IAsyncResult ar)
		{
			if (ar.AsyncState != this)
			{
				throw new ArgumentException(
					"Fatal: invalid data passed to callback");
			}

			using (MemoryManager.GetMemoryManager())
			{
				Pdu pdu;
				try
				{
					pdu = snmp_.EndInvoke(ar);
				}
				catch (Exception e)
				{
					Console.Error.WriteLine(e);
					barrier_.Enter();
					return;
				}

				bool show = (nRepd_ % 1000) == 0;
				if (show || debug_)
				{
					ManagerUtilities.PrintPdu(Console.Out,
						"Callback PDU:", pdu, true, id_.ToString());
				}

				Pdu nextPdu = pdu_;
				if (operType_ == OperType.Walk)
				{
					Vb nextVb = pdu[0];
					Oid nextOid = nextVb.Oid;
					Oid rootOid = pdu_[0].Oid;
					if (nextOid.StartsWith(rootOid))
					{
						if (show)
						{
							SnmpSyntax val = nextVb.Value;
							SmiSyntax type = val != null ? val.SmiSyntax : SmiSyntax.Null;
							Console.WriteLine("[{0}]: {1},{2},{3}", id_, nextOid, val, type);
						}

						nRepd_--;
						nextPdu = pdu_.Clone(new Vb(nextOid));
					}
				}

				nCalls_++;
				nRepd_++;
				if (nRepd_ < nRepeats_)
				{
					AsyncDoSnmp(snmp_, target_, nextPdu,
						new AsyncCallback(ResponseCallback), this);
				}
				else
				{
					barrier_.Enter();
				}
			}
		}

		private void TableResponseCallback(IAsyncResult ar)
		{
			Vb[][] vbTable;
			Oid[] columnOids = (Oid[]) ar.AsyncState;
			try
			{
				int nRequests = 0;
				vbTable = TableReader.EndGetTable(ar, ref nRequests);
				nCalls_ += nRequests;
				TableReader.PrintTable(Console.Out, columnOids, vbTable);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
			}
			finally
			{
				barrier_.Enter();
			}
		}

		private Barrier Barrier
		{
			get { return barrier_; }
		}

		private int Calls
		{
			get
			{
				if (exc_ != null)
				{
					throw exc_;
				}
				return nCalls_;
			}
		}

		private static void PrintPdu(TextWriter os, string text, Pdu pdu, bool debug)
		{
			ManagerUtilities.PrintPdu(os, text, pdu, debug, Thread.CurrentThread.Name);
		}

		private static Pdu Invoke(Snmp snmp, Pdu pdu, SnmpTarget target, bool asyncSync)
		{
			if (asyncSync)
			{
				IAsyncResult ar = snmp.BeginInvoke(pdu, target, null, null);
				return snmp.EndInvoke(ar);
			}
			else
			{
				return snmp.Invoke(pdu, target);
			}
		}
				
		private static void Walk(Snmp snmp, Pdu pdu, SnmpTarget target,
			bool asyncSync, bool show, bool debug, ref int ncalls)
		{
			string thName = Thread.CurrentThread.Name;
			Oid rootOid = pdu[0].Oid;
			while (true)
			{
				if (debug)
				{
					PrintPdu(Console.Out, "Sending PDU to target " + target, pdu, debug);
				}

				Pdu resp = Invoke(snmp, pdu, target, asyncSync);
				ncalls++;

				Vb nextVb = resp[0];
				Oid nextOid = nextVb.Oid;
				if (!nextOid.StartsWith(rootOid))
				{
					break;
				}

				if (debug)
				{
					PrintPdu(Console.Out, "Received PDU:", resp, debug);
				}
				else
				if (show)
				{
					SnmpSyntax val = nextVb.Value;
					SmiSyntax type = val != null ? val.SmiSyntax : SmiSyntax.Null;
					Console.WriteLine("[{0}]: {1},{2},{3}", thName, nextOid, val, type);
				}
				
				pdu = pdu.Clone(new Vb(nextOid));
			}
		}

		private static Oid[] GetColumnOids(Pdu pdu)
		{
			Vb[] vbs = pdu.Vbs;
			Oid[] columnOids = new Oid[vbs.Length];
			for (int i = 0; i < columnOids.Length; i++)
			{
				columnOids[i] = vbs[i].Oid;
			}
			return columnOids;
		}

		private static void Table(Snmp snmp, Pdu pdu, SnmpTarget target,
			ref TableReader.GetTableOptions tableOptions, bool show, bool debug,
			ref int ncalls)
		{
			Oid[] columnOids = GetColumnOids(pdu);
			pdu = pdu.Clone(new Vb[0]);	// not really needed; for test purposes
			Vb[][] vbTable = TableReader.GetTable(snmp, pdu, target,
								columnOids, tableOptions, ref ncalls);
			if (show || debug)
			{
				TableReader.PrintTable(Console.Out, columnOids, vbTable);
			}
		}

		private static Pdu DoSnmp(Snmp snmp, SnmpTarget target, Pdu pdu,
			OperType operType, ref TableReader.GetTableOptions tableOptions,
			bool asyncSync, bool show, bool debug, ref int ncalls)
		{
			switch (operType)
			{
				case OperType.Walk:
					Walk(snmp, pdu, target, asyncSync, show, debug, ref ncalls);
					pdu = null;
					break;
				case OperType.Table:
					Table(snmp, pdu, target, ref tableOptions, show, debug, ref ncalls);
					pdu = null;
					break;
				default:
					pdu = Invoke(snmp, pdu, target, asyncSync);
					ncalls++;
					break;
			}
			return pdu;
		}
		
		private static int DoSnmp(Snmp snmp, SnmpTarget target, Pdu pdu,
			OperType operType, ref TableReader.GetTableOptions tableOptions,
			uint repeat, bool asyncSync, bool debug)
		{
			int ncalls = 0;
			for (uint i = 0; i < repeat; i++)
			{
				using (IMemoryManager mgr = MemoryManager.GetMemoryManager())
				{
					bool show = (i % 1000) == 0;
					if (show || debug)
					{
						PrintPdu(Console.Out, "Sending PDU to target " + target, pdu, debug);
					}

					Pdu resPdu = DoSnmp(snmp, target, pdu,
									operType, ref tableOptions,
									asyncSync, show, debug, ref ncalls);

					if ((show || debug) && resPdu != null)
					{
						PrintPdu(Console.Out, "Received PDU:", resPdu, debug);
					}
					if (debug)
					{
						Console.WriteLine("Removing " + mgr.Count + " objects");
					}
				}
			}
			return ncalls;
		}

		private static void DoSnmp(Object o)
		{
			((Manager) o).DoSnmp();
		}
		
		private static void AsyncDoSnmp(Snmp snmp, SnmpTarget target, Pdu pdu,
			AsyncCallback callback, Object data)
		{
			snmp.BeginInvoke(pdu, target, callback, data);
		}

		private static void SetupThreads(Manager mgr, uint nThreads)
		{
			// The thread pool is much more convenient as it allows the state
			// to be passed as an arg. We use the pool for 5 threads or less.
			if (nThreads <= 5)
			{
				WaitCallback wc = new WaitCallback(DoSnmp);
				for (int i = 0; i < nThreads; i++)
				{
					ThreadPool.QueueUserWorkItem(wc, mgr);
				}
			}
			else
			{
				ThreadStart ts = new ThreadStart(mgr.DoSnmp);
				for (int i = 0; i < nThreads; i++)
				{
					Thread t = new Thread(ts);
					t.Name = (i + 1).ToString();
					t.Start();
				}
			}
		}

		private static int Process(Manager mgr, ref string option)
		{
			// We use a single Manager object in all threads; the current
			// thread is also processing the job
			option = "<thread creation>";
			uint nThreads = mgr.Barrier.Size;
			SetupThreads(mgr, nThreads - 1);

			option = "<op>";
			mgr.DoSnmp();

			// All threads have finished processing (and entered the barrier)
			return mgr.Calls;
		}

		private static int AsyncProcess(Manager mgr, ref string option)
		{
			// Note that each "thread" needs to maintain its own number
			// of repetitions - we need a single object per each "thread"
			option = "<thread creation>";
			Barrier barrier = mgr.Barrier;
			uint nThreads = barrier.Size - 1;
			Manager[] mgrs = new Manager[nThreads];
			for (int i = 0; i < nThreads; i++)
			{
				mgrs[i] = mgr.Clone(i);
				mgrs[i].AsyncDoSnmp();
			}

			// Wait for all threads to finish processing (and enter the barrier)
			barrier.Enter();

			int ncalls = 0;
			for (int i = 0; i < nThreads; i++)
			{
				ncalls += mgrs[i].Calls;
			}
			return ncalls;
		}
		
		private static void GetOperType(string op,
			out PduType pduType, out OperType operType)
		{
			operType = OperType.Simple;
			switch (op)
			{
				case "walk":
					pduType = PduType.GetNext;
					operType = OperType.Walk;
					break;
				case "table":
					pduType = PduType.GetBulk;
					operType = OperType.Table;
					break;
				default:
					pduType = (PduType) Enum.Parse(typeof(PduType), op, true);
					break;
			}
		}

		private static bool ParseBoolWithDefault(string val)
		{
			return val.Length == 0 ? true : bool.Parse(val);
		}

		private delegate int Delegate(Manager mgr, ref string option);

		public static void Main(string[] args) 
		{
#if REQUIRES_CRT_INIT
			CRT.Auto.Initialize();
#endif

			if (args.Length < 2)
			{
				Console.WriteLine("SNMP++.NET command line utility\n"
					+ "Author: Marek Malowidzki 2003,2004 (maom_onet@poczta.onet.pl)\n"
					+ "Based on SNMP++ package from Peter E. Mellquist (HP) and Jochen Katz\n"
					+ "SNMP++.NET " + Assembly.GetAssembly(typeof(Snmp))
					+ " built on " + Snmp.BuildTime
					+ "; SNMP++ v. " + Snmp.Snmp_ppVersion + "\n\n"
					+ "Usage: " + Environment.GetCommandLineArgs()[0] + " <op> <agent> [<options>]\n"
					+ "where:\n"
					+ "op           - SNMP operation to perform (get,getnext,getbulk,set,walk,table)\n"
					+ "agent        - IP address or DNS name of an agent\n"
					+ "options are the following:\n"
					+ "-v<version>  - SNMP version to use (v1(default),v2c,v3)\n"
					+ "-p<port>     - port number to use\n"
					+ "-r<retries>  - number of retries (default: 2)\n"
					+ "-t<timeout>  - timeout in milliseconds (default: 1000)\n"
					+ "-d<bool>     - print debug messages\n"
					+ "-o<oid>      - subsequent OID\n"
					+ "-T<type>     - subsequent SNMP type name:\n"
						+ string.Join(",", SnmpSyntax.SupportedSyntaxNames)
					+ " (o stands for oid and s for string)\n"
					+ "-V<value>    - subsequent value\n"
					+ "Debug options:\n"
					+ "-Df<file>    - log file name\n"
					+ "-Dl<level>   - log level\n"
					+ "SNMPv1/v2c options:\n"
					+ "-c<comm>     - read community name (default: public)\n"
					+ "-C<comm>     - write community name (default: public)\n"
					+ "SNMPv3 options:\n"
					+ "-b<boot>     - boot counter value (default: 100)\n"
					+ "-sn<secName> - security name\n"
					+ "-sl<secLevel>- security level ("
						+ ManagerUtilities.EnumInfo(typeof(SecurityLevel), SecurityLevel.AuthPriv) + ")\n"
					+ "-sm<secModel>- security model ("
						+ ManagerUtilities.EnumInfo(typeof(SecurityModel), SecurityModel.USM) + ")\n"
					+ "-xn<ctxName> - context name\n"
					+ "-xe<ctxEngId>- context engine ID (default: discover)\n"
					+ "-A<authProto>- authentication protocol ("
						+ ManagerUtilities.EnumInfo(typeof(AuthProtocol), AuthProtocol.None) + ")\n"
					+ "-P<privProto>- privacy protocol ("
						+ ManagerUtilities.EnumInfo(typeof(PrivProtocol), PrivProtocol.None) + ")\n"
					+ "-Ua<authPass>- authentication password\n"
					+ "-Up<privPass>- privacy password\n"
					+ "Table operation options:\n"
					+ "-Os<rowOid>  - row index to start from\n"
					+ "-Oe<rowOid>  - row index to finish at\n"
					+ "-On<count>   - max number of rows to retrieve\n"
					+ "-Or<count>   - rows per query or 0 for heuristics\n"
					+ "Testing options:\n"
					+ "-Xa<bool>    - use asynchronous interface\n"
					+ "-Xm<filePfx> - collect memory usage in <filePfx>.<ext> files, where ext denotes memory type\n"
					+ "-Xn<repeat>  - repeat number of times (default: 1)\n"
					+ "-Xp<priority>- set process priority ("
						+ ManagerUtilities.EnumInfo(typeof(ProcessPriorityClass), ProcessPriorityClass.Normal) + ")\n"
					+ "-Xs<bool>    - use asychronous interface for synchronous calls\n"
					+ "-Xt<threads> - run multiple threads (default: 1)");
				Environment.Exit(1);
			}

			string option = "<ip>";
			Thread statsCollector = null;
			MemoryStats stats = null;
			int retCode = 1;
			try
			{
				UdpAddress udp = new UdpAddress(args[1]);
				SnmpVersion ver = SnmpVersion.SNMPv1;
				SnmpTarget.DefaultRetries = 2;
				SnmpTarget.DefaultTimeout = 1000;
				uint nRepeats = 1, nThreads = 1;
				int debugLevel = int.MinValue;
				string statsFile = null, debugFile = null;
				TableReader.GetTableOptions tableOptions = new TableReader.GetTableOptions();
				bool async = false, asyncSync = false, debug = false;

				string  readCommunity  = "public",
						writeCommunity = "public";
				AuthProtocol authProto = AuthProtocol.None;
				PrivProtocol privProto = PrivProtocol.None;
				string  authPass = "",
						privPass = "",
						secName  = "",
						ctxName  = "",
						ctxEngId = "";
				SecurityLevel secLevel = SecurityLevel.AuthPriv;
				SecurityModel secModel = SecurityModel.USM;
				uint boot = 100;

				string[] oids  = new string[128],
						 types = new string[128],
						 vals  = new string[128];
				int noids = 0, ntypes = 0, nvals = 0;

				string val;
				int index = 2;
				while (ManagerUtilities.GetOption(args, out option, out val, ref index))
				{
					char sub;
					switch (option)
					{
						case "v":
							ver = (SnmpVersion)	Enum.Parse(
								typeof(SnmpVersion), "SNMPv" + val, true);
							break;
						case "p":
							udp = new UdpAddress(udp.Ip, int.Parse(val));
							break;
						case "r":
							SnmpTarget.DefaultRetries = int.Parse(val);
							break;
						case "t":
							SnmpTarget.DefaultTimeout = int.Parse(val);
							break;
						case "d":
							debug = bool.Parse(val);
							break;
						case "o":
							oids[noids++] = val;
							break;
						case "T":
							types[ntypes++] = val;
							break;
						case "V":
							vals[nvals++] = val;
							break;
						case "c":
							readCommunity = val;
							break;
						case "C":
							writeCommunity = val;
							break;
						case "b":
							boot = uint.Parse(val);
							break;
						case "s":
							switch (sub = ManagerUtilities.GetSubOption(ref val))
							{
								case 'n':
									secName = val;
									break;
								case 'l':
									secLevel = (SecurityLevel) Enum.Parse(
										typeof(SecurityLevel), val, true);
									break;
								case 'm':
									secModel = (SecurityModel) Enum.Parse(
										typeof(SecurityModel), val, true);
									break;
								default:
									throw new ArgumentException(
										sub + ": invalid sub-option");
							}
							break;
						case "x":
							switch (sub = ManagerUtilities.GetSubOption(ref val))
							{
								case 'n':
									ctxName = val;
									break;
								case 'e':
									ctxEngId = val;
									break;
								default:
									throw new ArgumentException(
										sub + ": invalid sub-option");
							}
							break;
						case "A":
							authProto = (AuthProtocol) Enum.Parse(
								typeof(AuthProtocol), val, true);
							break;
						case "P":
							privProto = (PrivProtocol) Enum.Parse(
								typeof(PrivProtocol), val, true);
							break;
						case "D":
							switch (sub = ManagerUtilities.GetSubOption(ref val))
							{
								case 'f':
									debugFile = val;
									break;
								case 'l':
									debugLevel = int.Parse(val);
									break;
								default:
									throw new ArgumentException(
										sub + ": invalid sub-option");
							}
							break;
						case "U":
							switch (sub = ManagerUtilities.GetSubOption(ref val))
							{
								case 'a':
									authPass = val;
									break;
								case 'p':
									privPass = val;
									break;
								default:
									throw new ArgumentException(
										sub + ": invalid sub-option");
							}
							break;
						case "O":
						{
							switch (sub = ManagerUtilities.GetSubOption(ref val))
							{
								case 's':
									tableOptions.startRowIndex = new Oid(val);
									break;
								case 'e':
									tableOptions.endRowIndex = new Oid(val);
									break;
								case 'n':
									tableOptions.maxRows = int.Parse(val);
									break;
								case 'r':
									tableOptions.rowsPerQuery = int.Parse(val);
									break;
								default:
									throw new ArgumentException(
										sub + ": invalid sub-option");
							}
							break;
						}
						case "X":
							switch (sub = ManagerUtilities.GetSubOption(ref val))
							{
								case 'a':
									async = ParseBoolWithDefault(val);
									break;
								case 'm':
									statsFile = val;
									break;
								case 'n':
									nRepeats = uint.Parse(val);
									break;
								case 'p':
									System.Diagnostics.Process.GetCurrentProcess().PriorityClass
										= (ProcessPriorityClass) Enum.Parse(
											typeof(ProcessPriorityClass), val, true);
									break;
								case 's':
									asyncSync = ParseBoolWithDefault(val);
									break;
								case 't':
									if ((nThreads = uint.Parse(val)) <= 0)
									{
										throw new ArgumentException(
											val + ": invalid threads number");
									}
									break;
								default:
									throw new ArgumentException(
										sub + ": invalid sub-option");
							}
							break;
						default:
							throw new ArgumentException(
								"-" + option + ": invalid option");
					}
				}

				if (noids == 0)
				{
					option = "<OIDs>";
					throw new ArgumentException(
						"No OIDs specified, use -o option");
				}

				bool asyncMode = async || asyncSync;

				// Debug options
				if (debugFile != null)
				{
					Snmp.DebugLogFile = debugFile;
				}
				if (debugLevel != int.MinValue)
				{
					Snmp.DebugLogLevel = debugLevel;
				}

				// Operation type processing
				option = "<Pdu creation>";
				PduType pduType;
				OperType operType;
				GetOperType(args[0].ToLower(), out pduType, out operType);

				// Adjusting settings for a table operation
				if (operType == OperType.Table)
				{
					TableReader.UseAsyncInvoke = asyncMode;
				}

				// Pdu creation
				Pdu pdu;
				using (IMemoryManager mgr = MemoryManager.GetMemoryManager())
				{
					Vb[] vbs = ManagerUtilities.CreateVbs(pduType, oids, noids, types, ntypes, vals, nvals);
					pdu = new Pdu(pduType, vbs);
					mgr.Remove(pdu);	// remove Pdu from the memory manager
				}

				// SnmpTarget creation
				SnmpTarget target;
				if (ver == SnmpVersion.SNMPv3)
				{
					option = "<SNMPv3 initialization>";
					V3MP.Init(new OctetStr("SNMP++.NET"), boot);
					USM usm = V3MP.Instance.Usm;
					usm.AddUsmUser(secName, authProto, privProto, authPass, privPass);

					target = new UTarget(udp, secName, secModel);

					pdu.SecurityLevel = secLevel;
					pdu.ContextName = new OctetStr(ctxName);
					pdu.ContextEngineId = new OctetStr(ctxEngId);
				}
				else
				{
					option = "<SNMPv1/v2c initialization>";
					target = new CTarget(udp, ver, readCommunity, writeCommunity);
				}
				udp = null;

				// Memory usage statistics initialization
				option = "<Statistics collector initialization>";
				if (statsFile != null)
				{
					stats = new MemoryStats(statsFile);
					statsCollector = new Thread(new ThreadStart(stats.Collect));
					statsCollector.Priority = ThreadPriority.BelowNormal;
					statsCollector.Start();
				}

				// Snmp session initialization & further processing
				option = "<SNMP session initialization>";
				using (Snmp snmp = new Snmp(asyncMode))
				{
					Thread.CurrentThread.Name = "0";

					Barrier barrier;
					Delegate fun;
					if (async)
					{
						fun = new Delegate(AsyncProcess);
						barrier = new Barrier(nThreads + 1);
					}
					else
					{
						fun = new Delegate(Process);
						barrier = new Barrier(nThreads);
					}

					Manager mgr = new Manager(0, snmp, target, pdu, operType,
											ref tableOptions, barrier,
											nRepeats, asyncSync, debug);

					DateTime start = DateTime.Now;
					int ncalls = fun(mgr, ref option);
					double msec = DateTime.Now.Subtract(start).TotalMilliseconds;

					// clear references on stack
					pdu = null; target = null; mgr = null;
					Console.WriteLine("{0} {1} SNMP request(s) in {2} msec. ({3} req./sec.)",
						ncalls, asyncMode ? "asynchronous" : "synchronous",
						(int) msec, msec > 0 ? (1000 * (long) ncalls / msec) : 0);
				}
				retCode = 0;
			}
			catch (SnmpClassException e)
			{
				Console.Error.WriteLine("*** SNMP class error while processing {0}:\n"
					+ "SnmpClass status code: {1}\n{2}", option, e.Status, e);
			}
			catch (SnmpException e)
			{
				Console.Error.WriteLine("*** SNMP protocol error while processing {0}:\n"
					+ "SNMP error status: {1}\nSNMP error index: {2}\n{3}",
					option, e.ErrorStatus, e.ErrorIndex, e);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("*** Error while processing {0}:\n{1}", option, e);
			}

			Console.WriteLine("Remaining native SNMP++ objects before GC: "
				+ MemoryManager.Count);

			if (statsCollector != null)
			{
				stats.WaitForNextStat();
			}

			GC.Collect();
			GC.Collect();
			GC.WaitForPendingFinalizers();

			if (statsCollector != null)
			{
				stats.WaitForNextStat();

				statsCollector.Interrupt();
				statsCollector.Join();
			}

			// Despite our honest intentions, there may still be uncollected
			// SNMP++.NET objects, especially in the Release mode
			Console.WriteLine("Remaining native SNMP++ objects after GC: "
				+ MemoryManager.Count);

			Environment.Exit(retCode);
		}
	}
}
