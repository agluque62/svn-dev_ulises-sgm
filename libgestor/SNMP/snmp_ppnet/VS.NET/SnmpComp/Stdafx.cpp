// stdafx.cpp : source file that includes just the standard includes
// SnmpComp.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

#ifdef DOTNET_USES_SNMP_PP_DLL

#include "snmp_pp\mp_v3.h"
#include "snmp_pp\notifyqueue.h"
#include "snmp_pp\octet.h"
#include "snmp_pp\target.h"
#include "snmp_pp\uxsnmp.h"

#ifdef SNMP_PP_NAMESPACE
namespace Snmp_pp {
#endif

v3MP *v3MP::I = 0;

#define SNMP_TRAP_PORT 162    // standard port # for SNMP traps
int CNotifyEventQueue::m_listen_port = SNMP_TRAP_PORT;

enum OctetStr::OutputType OctetStr::hex_output_type = OctetStr::OutputHexAndClear;

unsigned long SnmpTarget::default_timeout = 100;
int SnmpTarget::default_retries = 1;

SnmpSynchronized Snmp::v3Lock;

#ifdef SNMP_PP_NAMESPACE
}; // end of namespace Snmp_pp
#endif

#endif
