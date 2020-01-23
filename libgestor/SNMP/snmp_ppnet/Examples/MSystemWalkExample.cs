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
// A system walk SNMPv1 example for SNMP++.NET.
//
// Author:	Marek Malowidzki	2004
// Send comments, suggestions and bug reports to maom_onet@poczta.onet.pl.
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Reflection;
using Org.Snmp.Snmp_pp;

namespace Org.Snmp.Snmp_pp
{
	public sealed class MSystemWalkExample
	{
		private static readonly Hashtable SYSOID2NAME_;

		static MSystemWalkExample()
		{
#if	REQUIRES_CRT_INIT
			CRT.Auto.Initialize();
#endif

			SYSOID2NAME_ = new Hashtable();
			/*
			SYSOID2NAME_.Add("1.3.6.1.2.1.1",     "system");
			SYSOID2NAME_.Add("1.3.6.1.2.1.1.1.0", "sysDescr");
			SYSOID2NAME_.Add("1.3.6.1.2.1.1.2.0", "sysObjectID");
			SYSOID2NAME_.Add("1.3.6.1.2.1.1.3.0", "sysUpTime");
			SYSOID2NAME_.Add("1.3.6.1.2.1.1.4.0", "sysContact");
			SYSOID2NAME_.Add("1.3.6.1.2.1.1.5.0", "sysName");
			SYSOID2NAME_.Add("1.3.6.1.2.1.1.6.0", "sysLocation");
			SYSOID2NAME_.Add("1.3.6.1.2.1.1.7.0", "sysServices");
			SYSOID2NAME_.Add("1.3.6.1.2.1.1.8.0", "sysORLastChange");
			SYSOID2NAME_.Add("1.3.6.1.2.1.1.9.0", "sysORTable");
			*/
			SYSOID2NAME_.Add("1.1", "Estado_Top");
			SYSOID2NAME_.Add("1.1.2.0", "Estado_HMI");
			SYSOID2NAME_.Add("1.1.3.1.1.0", "Estado_Jacks_1");
			SYSOID2NAME_.Add("1.1.3.1.1.1", "Estado_Jacks_2");
			SYSOID2NAME_.Add("1.1.4.0", "Num_Altavoces");
			SYSOID2NAME_.Add("1.1.5.1.1", "Descriptor_Altavoces");
			SYSOID2NAME_.Add("1.1.5.1.2", "Estado_Altavoces");
			SYSOID2NAME_.Add("1.1.6.0", "Estado_Panel");

			foreach (object key in ((Hashtable) SYSOID2NAME_.Clone()).Keys)
			{
				object name = SYSOID2NAME_[key];
				Oid oid = new Oid(key.ToString());
				SYSOID2NAME_.Add(oid, name);
				SYSOID2NAME_.Add(name, oid);
			}
		}

		private MSystemWalkExample() {}

		public static void Main(string[] args) 
		{
			if (args.Length < 2)
			{
				Console.Error.WriteLine(
					"SNMP++.NET {0} built on {1}; SNMP++ v. {2}\n\n"
					+ "Usage: {3} <ip> <readCommunity> [<debugMark>]",
					Assembly.GetAssembly(typeof(Snmp)),
					Snmp.BuildTime,
					Snmp.Snmp_ppVersion,
					Environment.GetCommandLineArgs()[0]);
				Environment.Exit(1);
			}

			string ip = args[0], community = args[1];
			bool debug = args.Length > 2 && args[2].Length > 0;
			try
			{
				SnmpTarget.DefaultTimeout = 10000;
				SnmpTarget.DefaultRetries = 2;

				using (Snmp snmp = new Snmp(false))
				{
					UdpAddress udp = new UdpAddress(ip);
					SnmpVersion ver = SnmpVersion.SNMPv1;
					CTarget target = new CTarget(udp, ver, community, community);

					Oid systemOid = (Oid)SYSOID2NAME_["Estado_Top"];
//					Oid systemOid = (Oid) SYSOID2NAME_["system"];
					Vb vb = new Vb(systemOid);
					Pdu pdu = new Pdu(PduType.GetBulk/* GetNext*/, vb);
					//while (true)
					{
						if (debug)
						{
							Console.WriteLine("Sending : " + pdu + " to " + target);
						}

						Pdu resp = snmp.Invoke(pdu, target);

						if (debug)
						{
							Console.WriteLine("Received: " + resp);
						}

						vb = resp[0];
						Oid oid = vb.Oid;
						/*
						if (!oid.StartsWith(systemOid))
						{
							break;
						}
						*/
						SnmpSyntax val = vb.Value;
						Console.WriteLine("{0}({1}): {2} ({3})",
							oid, SYSOID2NAME_[oid], val, val.SmiSyntax);

						pdu = pdu.Clone(vb);
					}
				}
			}
			catch (SnmpException e) 
			{
				Console.WriteLine("SnmpException:"
					+ "\nstatus : " + e.ErrorStatus
					+ "\nindex  : " + e.ErrorIndex
					+ "\nmessage: " + e.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
