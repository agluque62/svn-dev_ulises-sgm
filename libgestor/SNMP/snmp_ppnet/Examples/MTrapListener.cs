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
// The trap listener console utility for SNMP++.NET.
//
// Author:	Marek Malowidzki	2003, 2004
// Send comments, suggestions and bug reports to maom_onet@poczta.onet.pl.
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Reflection;
using System.Threading;
using Org.Snmp.Snmp_pp;

namespace Org.Snmp.Snmp_pp
{
	public sealed class MTrapListener
	{
		private static readonly int CB_DATA_ = new Random().Next();
		private static int		nextId_;
		[ThreadStatic]
		private static object	threadId_;

		private MTrapListener() {}

		private static string Now()
		{
			return DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
		}

		private static void GotTrap(Snmp snmp, Pdu pdu, SnmpTarget target, object cbData)
		{
			if ((int) cbData != CB_DATA_)
			{
				Console.Error.WriteLine("*** Invalid callback data!");
			}
			if (threadId_ == null)
			{
				threadId_ = Interlocked.Increment(ref nextId_);
			}

			// Use a lock if you want do not want messages from various threads
			// to be interleaved
			ManagerUtilities.PrintPdu(Console.Out,
				Now() + ": " + pdu.Type + " received from " + target,
				pdu, true, threadId_);
			Console.Out.WriteLine(
				"Enterprise: " + pdu.NotifyEnterprise
				+ "\nNotify OID      : " + pdu.NotifyId
				+ "\nV1 generic-trap : " + pdu.V1GenericTrap
				+ "\nV1 specific-trap: " + pdu.V1SpecificTrap
				+ "\nTimestamp        : " + pdu.NotifyTimestamp);
			if (pdu.Type == PduType.V1Trap)
			{
				Console.Out.WriteLine("v1TrapAddr: " + pdu.V1TrapAddress);
			}

			if (pdu.Type == PduType.Inform)
			{
				Vb vb = new Vb("1.3.6.1", new OctetStr("this is the response"));
				pdu = new Pdu(PduType.Response, vb);
				snmp.Invoke(pdu, target);
				ManagerUtilities.PrintPdu(Console.Out,
					"Response sent to " + target, pdu, true, threadId_);
			}

			// Long trap processing follows...
			Thread.Sleep(5000);
		}

		public static void Main(string[] args)
		{
#if	REQUIRES_CRT_INIT
			CRT.Auto.Initialize();
#endif

			string[] secNames = new string[100],
					 authPass = new string[100],
					 privPass = new string[100];
			AuthProtocol[] authProtos = new AuthProtocol[100];
			PrivProtocol[] privProtos = new PrivProtocol[100];
			int port  = 162, nSecNames = 0, naps = 0, npps = 0, napr = 0, nppr = 0;
			uint boot = 100;

			if (args.Length < 2)
			{
				Console.WriteLine("SNMP++.NET trap listener utility\n"
					+ "Author: Marek Malowidzki 2003,2004 (maom_onet@poczta.onet.pl)\n"
					+ "Based on SNMP++ package from Peter E. Mellquist (HP) and Jochen Katz\n"
					+ "SNMP++.NET " + Assembly.GetAssembly(typeof(Snmp))
					+ " built on " + Snmp.BuildTime
					+ "; SNMP++ v. " + Snmp.Snmp_ppVersion + "\n\n"
					+ "Usage: " + Environment.GetCommandLineArgs()[0] + " <options>\n"
					+ "where options are the following (specify at least one):\n"
					+ "-p<port>     - port number to use (default: 162)\n"
					+ "SNMPv3 options:\n"
					+ "-b<boot>     - boot counter value (default: 100)\n"
					+ "Subsequent SNMPv3 users' data:\n"
					+ "-sn<secName> - security name\n"
					+ "-A<authProto>- authentication protocol ("
					+ ManagerUtilities.EnumInfo(typeof(AuthProtocol), AuthProtocol.None) + ")\n"
					+ "-P<privProto>- privacy protocol ("
					+ ManagerUtilities.EnumInfo(typeof(PrivProtocol), PrivProtocol.None) + ")\n"
					+ "-Ua<authPass>- authentication password\n"
					+ "-Up<privPass>- privacy password\n");
				Environment.Exit(1);
			}

			string option, val;
			int index = 0;
			while (ManagerUtilities.GetOption(args, out option, out val, ref index))
			{
				char sub;
				switch (option)
				{
					case "p":
						port = int.Parse(val);
						break;
					case "b":
						boot = uint.Parse(val);
						break;
					case "s":
					switch (sub = ManagerUtilities.GetSubOption(ref val))
					{
						case 'n':
							secNames[nSecNames++] = val;
							break;
						default:
							throw new ArgumentException(
								sub + ": invalid sub-option");
					}
						break;
					case "A":
						authProtos[napr++] = (AuthProtocol) Enum.Parse(
							typeof(AuthProtocol), val, true);
						break;
					case "P":
						privProtos[nppr++] = (PrivProtocol) Enum.Parse(
							typeof(PrivProtocol), val, true);
						break;
					case "U":
					switch (sub = ManagerUtilities.GetSubOption(ref val))
					{
						case 'a':
							authPass[naps++] = val;
							break;
						case 'p':
							privPass[npps++] = val;
							break;
						default:
							throw new ApplicationException(
								sub + ": invalid sub-option");
					}
						break;
					default:
						throw new ApplicationException(
							"-" + option + ": invalid option");
				}
			}

			try
			{
				option = "v3MP initialization";
				V3MP.Init(new OctetStr("SNMP++.NET"), boot);
				USM usm = V3MP.Instance.Usm;
				for (int i = 0; i < nSecNames; i++)
				{
					usm.AddUsmUser(secNames[i],
						authProtos[i], privProtos[i], authPass[i], privPass[i]);
				}

				option = "Snmp initialization";
				using (Snmp snmp = new Snmp(true))
				{
					option = "traps configuration";
					Console.WriteLine("Registering trap listener for port " + port);
					snmp.NotifyListenPort = port;

					snmp.NotifyRegister(null, null, new NotifyCallback(GotTrap), CB_DATA_);

					option = "traps";
					Console.WriteLine("Processing traps...");
					Thread.Sleep(Timeout.Infinite);
				}
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
		}
	}
}
