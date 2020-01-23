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
// A simple library for SNMP managers.
//
// Author:	Marek Malowidzki	2004
// Send comments, suggestions and bug reports to maom_onet@poczta.onet.pl.
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using Org.Snmp.Snmp_pp;

namespace Org.Snmp.Snmp_pp
{
	public sealed class SnmpUtils
	{
		private SnmpUtils() {}

		public static IList Walk(Snmp snmp, SnmpTarget target, Pdu pdu, IList list)
		{
			if (!(pdu.Type == PduType.GetNext || pdu.Type == PduType.GetBulk))
			{
				throw new ArgumentException("Invalid Pdu type", "pdu");
			}

			if (pdu.Count != 1)
			{
				throw new ArgumentException("Expected a single Vb", "pdu");
			}

			try
			{
				Oid subtree = pdu[0].Oid;
				while (true)
				{
					pdu = snmp.Invoke(pdu, target);

					Vb lastVb = null;
					foreach (Vb vb in pdu)
					{
						Oid oid = vb.Oid;
						if (!oid.StartsWith(subtree)
							|| vb.Value.SmiSyntax == SmiSyntax.EndOfMibView)
						{
							return list;
						}

						lastVb = vb;
						list.Add(vb);
					}

					pdu = pdu.Clone(lastVb);
				}
			}
			catch (SnmpException e)
			{
				// SNMPv1 and NoSuchName?
				if (e.ErrorStatus != SnmpError.NoSuchName)
					throw;
			}

			return list;
		}

		public static IList Walk(Snmp snmp, SnmpTarget target, Pdu pdu)
		{
			return Walk(snmp, target, pdu, new ArrayList(128));
		}
	}
}
