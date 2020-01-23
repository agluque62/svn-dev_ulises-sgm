////////////////////////////////////////////////////////////////////////////////
//
// SNMP++.NET v. 1.11 (2004-10-29 15:00:00)
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

#using <mscorlib.dll>
#using <System.dll>
#include <tchar.h>

using namespace System;
using namespace System::Collections;
using namespace System::Net;
using namespace System::Reflection;
using namespace Org::Snmp::Snmp_pp;

namespace Org {

namespace Snmp {
		
namespace Snmp_pp {

public __gc __sealed class MSystemWalkExample {

public:
	static void Walk(String* ip, String* community, bool debug) {

		SnmpTarget::DefaultTimeout = 10000;
		SnmpTarget::DefaultRetries = 2;

		Snmp* snmp = 0;
		try {
			snmp = new Snmp(false);

			UdpAddress* udp = new UdpAddress(ip);
			SnmpVersion ver = SnmpVersion::SNMPv1;
			CTarget* target = new CTarget(udp, ver, community, community);
			Oid* systemOid = dynamic_cast<Oid*>(SYSOID2NAME_->Item[S"system"]);
			Vb* vb = new Vb(systemOid);
			Pdu* pdu = new Pdu(PduType::GetNext, vb);
			while (true) {
				if (debug) {
					Console::WriteLine(S"Sending : {0} to {1}", pdu, target);
				}

				Pdu* resp = snmp->Invoke(pdu, target);

				if (debug) {
					Console::WriteLine(S"Received: {0}", resp);
				}

				vb = resp->Item[0];
				Oid* oid = vb->Oid;
				if (!oid->StartsWith(systemOid)) {
					break;
				}

				SnmpSyntax* val = vb->Value;
				Console::WriteLine(S"{0}({1}): {2} ({3})",
					oid, SYSOID2NAME_->Item[oid], val, __box(val->SmiSyntax));

				pdu = pdu->Clone(new Vb(oid));
			}
		}
		catch (SnmpException* e) {
			Console::WriteLine(
				S"SnmpException:\nstatus : {0}\nindex  : {1}\nmessage: {2}",
				__box(e->ErrorStatus), __box(e->ErrorIndex), e->Message);
		}
		catch (Exception* e) {
			Console::WriteLine(e);
		}
		__finally {
			if (snmp)
				snmp->Dispose();
		}
	}
private:
	MSystemWalkExample() {}

	static MSystemWalkExample() {

		SYSOID2NAME_->Add(S"1.3.6.1.2.1.1",     S"system");
		SYSOID2NAME_->Add(S"1.3.6.1.2.1.1.1.0", S"sysDescr");
		SYSOID2NAME_->Add(S"1.3.6.1.2.1.1.2.0", S"sysObjectID");
		SYSOID2NAME_->Add(S"1.3.6.1.2.1.1.3.0", S"sysUpTime");
		SYSOID2NAME_->Add(S"1.3.6.1.2.1.1.4.0", S"sysContact");
		SYSOID2NAME_->Add(S"1.3.6.1.2.1.1.5.0", S"sysName");
		SYSOID2NAME_->Add(S"1.3.6.1.2.1.1.6.0", S"sysLocation");
		SYSOID2NAME_->Add(S"1.3.6.1.2.1.1.7.0", S"sysServices");
		SYSOID2NAME_->Add(S"1.3.6.1.2.1.1.8.0", S"sysORLastChange");
		SYSOID2NAME_->Add(S"1.3.6.1.2.1.1.9.0", S"sysORTable");

		IEnumerator* en = dynamic_cast<Hashtable*>(SYSOID2NAME_->Clone())->Keys->GetEnumerator();
		while (en->MoveNext()) {
			Object* key = en->Current;
			Object* name = SYSOID2NAME_->Item[key];
			Oid* oid = new Oid(key->ToString());
			SYSOID2NAME_->Add(oid, name);
			SYSOID2NAME_->Add(name, oid);
		}
	}

	static Hashtable* SYSOID2NAME_ = new Hashtable();
};

}	// end of namespace Snmp_pp

}	// end of namespace Snmp

}	// end of namespace Org

int _tmain(int argc, _TCHAR* argv[]) {

		// C++ behaves in a different way than C# or VB.NET, as _tmain() is
		// a global function and not a method of a class. Thus, _tmain() starts
		// before the static constructor and we need to initialize the DLL here.
#ifdef	REQUIRES_CRT_INIT
		CRT::Auto::Initialize();
#endif

	if (argc < 3) {
		Console::Error->WriteLine(
			String::Concat(S"SNMP++.NET {0} built on {1}; SNMP++ v. {2}\n\n",
				S"Usage: ", (String*) argv[0]," <ip> <readCommunity> [<debugMark>]"),
				Assembly::GetAssembly(__typeof(Snmp)),
				Snmp::BuildTime,
				Snmp::Snmp_ppVersion);
		return 1;
	}

	String* ip = (String*) argv[1];
	String* community = (String*) argv[2];
	bool debug = argc > 3;
	Console::WriteLine(ip);
	Console::WriteLine(community);
	MSystemWalkExample::Walk(ip, community, debug);
	return 0;
}
