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
// A simple library for SNMP++.NET managers.
//
// Author:	Marek Malowidzki	2003, 2004
// Send comments, suggestions and bug reports to maom_onet@poczta.onet.pl.
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using Org.Snmp.Snmp_pp;

namespace Org.Snmp.Snmp_pp
{
	public sealed class ManagerUtilities
	{
		private ManagerUtilities() {}

		public static string EnumInfo(Type t, object defaultValue)
		{
			string[] names = Enum.GetNames(t);
			string defname = Enum.GetName(t, defaultValue);
			for (int i = 0; i < names.Length; i++)
			{
				if (names[i] == defname) 
				{
					names[i] += "(default)";
					break;
				}
			}
			return string.Join(",", names);
		}

		public static bool GetOption(string[] args,
			out string option, out string val, ref int index)
		{
			option = val = null;
			if (index >= args.Length)
			{
				return false;
			}

			option = args[index++];
			if (!(option.Length > 1 && option[0] == '-'))
			{
				throw new ArgumentException("expected option, got " + option, "option");
			}

			val = option.Substring(2);
			option = option.Substring(1, 1);

			if (val.Length == 0)
			{
				if (index >= args.Length)
				{
					throw new ArgumentException("no value for option -" + option, "option");
				}
				val = args[index++];
			}
			return true;
		}

		public static char GetSubOption(ref string s)
		{
			if (s.Length == 0)
			{
				throw new ArgumentException("sub-option expected", "s");
			}

			char c = s[0];
			s = s.Substring(1);
			return c;
		}

		public static Vb[] CreateVbs(PduType pduType, string[] oids, int oidsCount,
			string[] types, int typesCount, string[] values, int valuesCount)
		{
			if (typesCount != valuesCount)
			{
				throw new ArgumentException(
					"types and values must be specified the same number of times");
			}

			Vb[] vbs = new Vb[oidsCount];
			if (pduType != PduType.Set && typesCount == 0)
			{
				for (int i = 0; i < oidsCount; i++)
				{
					vbs[i] = new Vb(oids[i]);
				}
			}
			else
			{
				if (oidsCount != typesCount)
				{
					throw new ArgumentException(
						"OBJECT IDENTIFIERs and types must be specified"
						+ " the same number of times");
				}

				for (int i = 0; i < oidsCount; i++)
				{
					vbs[i] = Vb.Create(oids[i], types[i], values[i]);
				}
			}
			return vbs;
		}

		public static void PrintPdu(TextWriter os, string text,
			Pdu pdu, bool debug, object id)
		{
			lock (os)
			{
				os.WriteLine("[{0}]: {1}{2}", id, text, debug ? "\n" + pdu : "");

				// another way would be through Pdu.Vbs:
				// foreach (Vb vb in pdu.Vbs) {...}
				int i = 0;
				foreach (Vb vb in pdu)
				{
					SnmpSyntax val = vb.Value;
					SmiSyntax type = val != null ? val.SmiSyntax : SmiSyntax.Null;
					os.WriteLine("oid [{0}]: {1}\nval [{0}]: {2} ({3})", i, vb.Oid, val, type);
					i++;
				}
			}
		}
	}
}
