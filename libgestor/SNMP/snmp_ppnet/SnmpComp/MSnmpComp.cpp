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
// This is the implementation of a SNMP++.NET component for the SNMP++ library.
// Compatible SNMP++ Releases: 3.2.14 and later.
//
// Most .NET classes are just thin wrappers around the unmanaged implementation.
// Some .NET conventions are applied, for example:
// - most "smaller" managed classes are immutable - and cannot be changed after
//   creation (except when ENABLE_DISPOSE_MANAGER is defined, see below).  They
//   do not implement IDisposable even if they hold pointers to unmanaged memory. 
// - "heavyweight" managed classes implement IDisposable to mark that they keep
//   native resources (memory, sockets). They should be manually Dispose()d.
// - wrapped C++ implementations are available through "private public" const T&
//   WrappedRef() member functions.
// - unmanaged memory is always copied when managed objects are created (so, no
//   pointer sharing and no reference counting is done). This is less efficient
//   but also safer. SNMP++ does not use reference counting internally, so care
//   would have to be taken (e.g., destroying Pdu means destroying all its Vb's,
//   etc.).
// - "pure" virtual classes turned to interfaces (none in the current impl.).
// - getters (get_xxx()) are converted to properties (__property get_Xxx()); the
//   same for setters (set_xxx()).
// - method names are converted from "C style" (e.g., aa_bb_cc) to ".NET style"
//   (e.g., AaBbCc).
// - operators== are replaced with Equals() methods; in most cases, GetHashCode()
//   methods are also overridden, to enable using SNMP++.NET objects as keys in
//   hashing tables. Comparison operators are replaced with IComparable.CompareTo().
// - errors are always returned as exceptions. As an example, when unmanaged new
//   returns NULL, UnmanagedOutOfMemoryException is thrown (OutOfMemoryException
//   is reserved for CLR).  SNMP++ errors are returned as SnmpClassExceptions or
//   SnmpExceptions; the former are for ::Snmp class errors while the latter are
//   for SNMP protocol errors.
// - the implementation assumes that the SNMP++ code  *does not use exceptions*
//   and that the unmanaged new operator returns NULL on memory allocation error
//   (instead of throwing bad_alloc()). If this is not true, then auto_ptr's must
//   be added in a number of places.
// - most classes contain additional programmer-friendly methods. These include:
//   - String*s used in most places where SNMP++ expects ::OctetStr
//   - ToString() returning friendly info (these calls may be costly for complex
//     classes, e.g., Pdu)
// - Vb creation has been (I think) greatly simplified with Vb::Create().
// Note that this is a C++ module rather than a header file. The reason is that
// this is a .NET assembly code (not indended to be included).
//
// MT safety: depends on the class:
// - the "small" classes are invariant and thus inherently MT-safe
// - the Snmp class depends SNMP++ MT-safety
// - the Pdu class is MT-unsafe (locking may be applied in a future release)
// - memory managers, iterators are bound to the creating thread
//
// Memory/resources management:
// - use Snmp within using(), as it keeps a socket
// - rely on automatic garbage collection
// - if it is necessary to decrease memory usage, run GC either periodically or
//   on the basis of MemoryManager.Count property value (the number of allocated
//   SNMP++.NET objects) or when UnmanagedOutOfMemoryException is thrown
// - finally, enable the memory manager and use it in client code. Note that in
//   this case, immutable objects may be destroyed!
//
// Changes:
// - 2006-05-19:
//   Added Pdu::V1GenericTrap and Pdu::V1SpecificTrap.
// - 2006-04-18:
//   Converted to Managed C++ for Visual Studio 2005 and .NET 2.0.
// - 2005-06-27:
//   Corrected Vb::Create(): SnmpSyntax name was incorrectly mapped to the
//   type. The reason was that one cannot map an int to a bool in a managed
//   code; instead of !String::Compare(..), String::Compare(..) != 0 must
//   be used (see the static constructor for the SnmpSyntax class).
//   Snmp::NotifyListenPort is now a static property.
//   GetHashValue() methods renamed to GetHashCode().
// - 2005-02-03:
//   Added calls to GC::KeepAlive() in multiple places (either directly or
//   through GCReturn* templates) to protect from a possible scenario when
//   a managed object is collected and finalized while underlying unmanaged
//   object is still in use (e.g., is passed to an unmanaged function). See
//   http://blogs.msdn.com/cbrumme/archive/2003/04/19/51365.aspx
//   http://www.gotdotnet.com/team/fxcop/Docs/Rules/SecurityRules/IntptrAndFinalize.html
//   Removed MT-unsafe calls to get_printable*() functions in  ::SnmpSyntax
//   and derived classes. get_printable() is now called on a temporary copy
//   (see SafePrintable* templates) or has completely been replaced with own
//   implementations.
//   IpAddress.FriendlyName now uses the .NET Dns class (due to doubts about
//   MT-safety of the unmanaged implementation).
//   Pdu::EndInvoke() does not create a WaitHandle any more even if it has
//   to wait for completion - see CallbackData::WaitForCompletion().
//   Added constructors UdpAddress(IPEndPoint*) and GenAddress(IPEndPoint*).
//   Added conversion operator (implicit) (IPaddress) IpAddress.
//   Added property Snmp.BuildTime (the build date & time of the assembly).
//   Added Oid.MaxLength field (the maximum OID length in SNMP).
// - 2004-12-27:
//   Changes in Dispose(bool), which are aiming at providing some protection
//   against problems with automatic DLL termination - see the DELETE_NATIVE
//   macro and the CRT namespace.
// - 2004-12-07:
//   A number of CLS compliant constructors, methods and properties added
//   in many classes using unsigned types; new additions use Int64 (C++'s
//   long long) type with values matching UInt32. Parameterless methods and
//   properties have AsInt64 suffix. Note that Counter64 also uses Int64.
//   Return type for UdpAddress.Port() and IpxSockAddress.Socket() is now
//   int (instead of unsigned short), to make them CLS-compliant.
//   Properties Pdu.MaxSizeScopedPdu, Pdu.NonRepeaters and Pdu.MaxRepetitions
//   were changed from unsigned int to int.
//   Fixed a bug in TimeTicks (::SnmpUInt32 instead of ::TimeTicks was created
//   as the underlying native SNMP++ object).
// - 2004-10-29:
//   C++'s volatile seems to be a bit slippery as it does not offer the same
//   functionality as .NET's volatile. Thus, 'volatile' has been dropped for
//   all unmanaged fields. Instead, explicit .NET locks are used. See:
//   http://blogs.msdn.com/brada/archive/2004/05/12/130935.aspx
//   Corrected a bug in one of internal OctetStr constructors (a reference to
//   a released memory; yes, this is unmanaged C++ ;-)).
//   Fixed UdpAddress::Ip.
// - 2004-09-30:
//   An important change in the interface: all Count/Length properties return
//   int instead of unsigned int, similarly, indexers take int-type parameter
//   rather than unsigned int.  This change makes the API more compliant with
//   standard libraries (e.g., string.Length, Hashtable.Count, string[], etc.).
//   Similar change has been made in Oid.Compare(), Oid.Trim(), Oid.TrimLeft()
//   and Snmp.PendingInvokes. Note that SNMP++ is not too consistent here, as
//   lengths for OctetStr and Oid are unsigned long, int otherwise.
//   Added two convenient Pdu constructors that operate on Oid's rather than
//   Vb's and should be more efficient in case of empty Vb's.
//   Added Clone() methods for cloning targets.
//   Added two static properties for debugging configuration: Snmp.DebugLogFile
//   and Snmp.DebugLogLevel.
// - 2004-09-09:
//   Added Snmp.StartAsyncDispatcher() and an additional argument to all Snmp
//   class constructors. The method and the argument allow Snmp to auto-start
//   a background dispatching thread for the asynchronous interface.
//   Added OctetStr.OutputType enum and OctetStr.OutputType property (which
//   follows a new SNMP++ 3.2.14 feature).
//   Fixed error reporting in asynchronous calls; now an exception is thrown
//   when error-status is not Success.
// - 2004-09-08:
//   Improved memory management: no more potentially dangerous bare unmanaged
//   pointers in constructors; auto_ptr<T> used in all such places.
//   Address.CompareTo() now compares addresses on a byte-by-byte manner.
//   Added RowStatus enumeration; operations on SNMP RowStatus still rely on
//   SnmpInt32 but now this enum can be used, e.g., as in this example (C#):
//   SnmpInt32 rowStatus = new SnmpInt32((int) RowStatus.CreateAndGo);
//   Also added Manager.GetHeapsMemory() (moved code from SnmpManager).
//   WaitHandle is now Close()d.
// - 2004-08-30: 
//   Added code to improve reliability, e.g., checks if SNMP++'s += operators
//   succeeded.
//   Corrected OBJECT_CREATED/OBJECT_DISPOSED macros, which now use a pointer
//   to correctly track the number of created objects.
//   Snmp.DefaultBulkMaxRepetitions property removed from API (was unused).
//   UTarget.EngineId property was finally made MT-safe.
//   Oid.Trim(int) changed to Oid.Trim(unsigned int) (as is in SNMP++).
//   Oid.TrimLeft() method added to ease extracting row indexes.
//   Code review and a number of small fixes in comments.
// - 2004-08-10: the Pdu class will be left as it is, no more significant API
//   changes. Currently it is not synchronized (it is MT-unsafe);  may become
//   synchronized in a future release.
// - 2004-08-01: finally converted to .NET 1.1 and VS.NET 2003. Problems with
//   mixed-mode assemblies due to bug in .NET 1.0 and 1.1.
// - 2004-07-01: added automatic random generator initialization in the Snmp's
//   class static constructor and additional Srand() static methods for manual
//   re-initialization.
// - 2004-05-11: Pdu.Clone(), the first step to make the Pdu class immutable
// - 2004-05-06: finally compiles with VS.NET 2003 after IPrivateDisposable has
//   been changed from a managed interface to an equivalent abstract class.
//
// TODO (provided this will be indeed useful):
// - documentation.
//
// Copyright (c) Military Communication Institute
// Author:	Marek Malowidzki	2003, 2004
// Send comments, suggestions and bug reports to maom_onet@poczta.onet.pl.
////////////////////////////////////////////////////////////////////////////////

#ifndef	_SNMPv3
#error	please define _SNMPv3
#endif

#ifndef	SNMP_PP_NAMESPACE
#error	please define SNMP_PP_NAMESPACE for all SNMP++ modules
#endif

#include <assert.h>
#include <memory>
#include "snmp_pp/snmp_pp.h"
#include "snmp_pp/notifyqueue.h"
#include "snmp_pp/log.h"	// SNMP++ v. 3.2.17
#include "MConversion.h"
#include "MTypes.h"
#include "MLock.h"
#include "auto_array_ptr.h"

#using "System.dll"

using namespace std;
using namespace System;
using namespace System::Collections;
using namespace System::Net;
using namespace System::Reflection;
using namespace System::Runtime::InteropServices;
using namespace System::Runtime::Serialization;
using namespace System::Text;
using namespace System::Threading;
using namespace Mao::DotNet;
using namespace Mao::Threading;
using namespace Snmp_pp;

// Warning: defining ENABLE_DISPOSE_MANAGER enables the "real" memory manager;
// immutable classes will not be immutable any more since the real manager, if
// installed, disposes all classes created within its scope.
#define	OBJECT_CREATED_NO_ADD(p)	MemoryManager::ObjectCreatedNoAdd(this, p)
#define	OBJECT_CREATED(p)			MemoryManager::ObjectCreated(this, p)
#define	OBJECT_DELETED(p)			MemoryManager::ObjectDeleted(this, p)

// If you wish this warning in the Debug mode, uncomment it below.
//#ifdef	_DEBUG
//#define	DEBUG_WARN_CRT()		System::Diagnostics::Debug::Fail("DLL already terminated!")
//#else
#define	DEBUG_WARN_CRT()			(void) 0
//#endif

#define	DELETE_NATIVE(m_p)			{\
										void *p = m_p;\
										if (CRT::g_dllUp)\
											delete m_p;\
										else\
											DEBUG_WARN_CRT();\
										m_p = 0;\
										\
										OBJECT_DELETED(p);\
									}
namespace Org {

namespace Snmp {

namespace Snmp_pp {

namespace CRT {
	extern volatile bool g_dllUp;

#ifndef	REQUIRES_CRT_INIT
	volatile bool g_dllUp = true;
#endif
}

namespace {

////////////////////////////////////////////////////////////////////////////////
// GCReturn*() templates should be used to assure a managed object will not be
// GC-collected and finalized during a call to an unmanaged code.  This may be
// surprising, but is possible - see
// http://blogs.msdn.com/cbrumme/archive/2003/04/19/51365.aspx
////////////////////////////////////////////////////////////////////////////////

template<typename RETT>
inline RETT GCReturnT(Object^ _this, const RETT& ret) {

	GC::KeepAlive(_this);
	return ret;
}

template<typename RETT>
inline RETT GCReturn2T(Object^ _this, Object^ _that, const RETT& ret) {

	GC::KeepAlive(_this);
	GC::KeepAlive(_that);
	return ret;
}

////////////////////////////////////////////////////////////////////////////////
// SafePrintable* templates should be used to perform calls to MT-unsafe (as of
// SNMP++ 1.15 version) ::SnmpSyntax::get_printable() member functions in an MT-
// safe manner.
////////////////////////////////////////////////////////////////////////////////

template<typename UT1, typename UT2>
inline String^ SafePrintable2T(const UT2& ut) {

	UT1 copy(ut);
	if (copy.valid()) {
		const char* ptr = copy.get_printable();
		if (ptr)
			return gcnew String(ptr);
	}

	throw gcnew UnmanagedOutOfMemoryException();
}

template<typename UT>
inline String^ SafePrintableT(const UT& ut) {
	return SafePrintable2T<UT, UT>(ut);
}

template<typename UT>
String^ SafePrintableHexT(const UT& ut) {

	UT copy(ut);
	if (copy.valid()) {
		const char* ptr = copy.get_printable_hex();
		if (ptr)
			return gcnew String(ptr);
	}

	throw gcnew UnmanagedOutOfMemoryException();
}

////////////////////////////////////////////////////////////////////////////////
// General-purpose utility classes and functions, not related to SNMP++: memory
// management, enumerators, collections, etc.
////////////////////////////////////////////////////////////////////////////////

template<typename MT, typename UT>
IList^ SnmpCollection2List(const SnmpCollection<UT>& collection) {

	ArrayList^ list = gcnew ArrayList(collection.size());
	for (int i = 0, count = collection.size(); i < count; i++) {
		auto_ptr<UT> aptr(UnmanagedNew<UT>());
		collection.get_element(*aptr, i);
		if (!aptr->valid()) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		list->Add(ManagedNew<MT, UT>(aptr));
	}
	return list;
}

template<typename MT, typename UT>
IList^ SnmpCollection2List(const SnmpCollection<UT>& collection, MT^ (fun)(const UT&)) {

	ArrayList^ list = gcnew ArrayList(collection.size());
	for (int i = 0, count = collection.size(); i < count; i++) {
		UT* ptr = 0;
		collection.get_element(ptr, i);
		list->Add(fun(*ptr));
	}
	return list;
}

template<typename MT, typename UT>
void List2SnmpCollection(IList^ list, SnmpCollection<UT>& collection) {

	collection.clear();

	if (list) {
		IEnumerator^ en = static_cast<IEnumerable^>(list)->GetEnumerator();
		int len = 0;
		while (en->MoveNext()) {
			MT^ o = dynamic_cast<MT^>(en->Current);
			collection += dynamic_cast<const UT&>(o->WrappedRef());
			GC::KeepAlive(o);
			if (collection.size() != ++len) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
		}
	}
}

unsigned int ArgToULong(long long i, const char* name) {

	if (i < 0 || i > UInt32::MaxValue) {
		throw gcnew ArgumentException("Value out of range", gcnew String(name));
	}
	return (unsigned int) i;
}

}	// end of the anonymous namespace

////////////////////////////////////////////////////////////////////////////////
// IPrivateDisposable - a "private IDisposable".
// Note: originally it was an interface but after changes in VS 2003, which does
// not allow any more an interface method to be non-public in classes (error is:
// "in a managed type you cannot reduce the accessibility to a virtual method"),
// I have changed the interface to a class.
////////////////////////////////////////////////////////////////////////////////

public ref class IPrivateDisposable abstract {

internal:
	virtual void InternalDispose() = 0;
};

////////////////////////////////////////////////////////////////////////////////
// IMemoryManager - an interface for a memory manager.
////////////////////////////////////////////////////////////////////////////////

public interface class IMemoryManager : public IDisposable {

	// Removes object from the memory manager. The object remains immutable
	// and may be safely used in any thread; will be garbage-collected later.
	bool Remove(Object^ o);

	property int Count {
		int get();
	}
};

////////////////////////////////////////////////////////////////////////////////
// RealMemoryManager - enables manual memory management for SNMP++.NET classes.
// Managers form a stack to enable nested using() statements involving them.
////////////////////////////////////////////////////////////////////////////////

private ref class RealMemoryManager sealed : public IMemoryManager {

private:
	ref class Entry sealed {

	public:
		array<IPrivateDisposable^>^	ds;
		int					count;
		Entry^				next;

		Entry(Entry^ n) : ds(gcnew array<IPrivateDisposable^>(128)), next(n) {}
	};
public:
	RealMemoryManager() {

		m_entries = gcnew Entry(nullptr);
		if (!m_mgrs) {
			m_mgrs = gcnew array<RealMemoryManager^>(128);
		}
		m_mgrs[m_nmgrs] = this;
		m_nmgrs++;
	}

	~RealMemoryManager() {

		if (m_nmgrs == 0 || m_mgrs[m_nmgrs - 1] != this) {
			// A serous application logic bug - Dispose() has been misused.
			// Clear the stack to enable garbage collection; this may later
			// cause further exceptions.
			Array::Clear(m_mgrs, 0, m_nmgrs);
			m_nmgrs = 0;
			throw gcnew InvalidOperationException(
				"Dispose() must be called once, in the reversed manager creation order");
		}

		m_mgrs[--m_nmgrs] = nullptr;
		while (m_entries) {
			for (int i = 0; i < m_entries->count; i++) {
				try {
					for ( ; i < m_entries->count; i++) {
						m_entries->ds[i]->InternalDispose();
					}
				}
				catch (Object^ exc)
				{
#ifdef	_DEBUG
					System::Diagnostics::Debug::Assert(false,
						"An exception thrown by Dispose()", exc->ToString());
					throw;
#endif
				}
			}
			m_entries = m_entries->next;
		}
		m_count = 0;
	}

	virtual bool Remove(Object^ o) {

		if (o) {
			Entry^ e = m_entries;
			while (e) {
				// first, search the recently added objects
				for (int i = e->count - 1; i >= 0; i--) {
					if (o == e->ds[i]) {
						e->ds[i] = e->ds[--e->count];
						m_count--;
						return true;
					}
				}
				e = e->next;
			}
		}
		return false;
	}

	property int Count {
		virtual int get() {
			return m_count;
		}
	}
internal:
	static void TryAdd(IPrivateDisposable^ o) {

		if (m_nmgrs) {
			m_mgrs[m_nmgrs - 1]->Add(o);
		}
	}
private:
	void Add(IPrivateDisposable^ o) {

		if (m_entries->count == m_entries->ds->Length) {
			m_entries = gcnew Entry(m_entries);
		}
		m_entries->ds[m_entries->count++] = o;
		m_count++;
	}

	Entry^ m_entries;
	int m_count;

	[ThreadStatic]
	static array<RealMemoryManager^>^ m_mgrs;
	[ThreadStatic]
	static unsigned int m_nmgrs;
};

////////////////////////////////////////////////////////////////////////////////
// DummyMemoryManager - does not do anything.
////////////////////////////////////////////////////////////////////////////////

#ifndef	ENABLE_DISPOSE_MANAGER

private ref class DummyMemoryManager sealed : public IMemoryManager {

public:
	~DummyMemoryManager() {}

	virtual bool Remove(Object^ o) {
		return true;
	}

	virtual property int Count {
		int get() {
			return 0;
		}
	}
};

#endif

////////////////////////////////////////////////////////////////////////////////
// MemoryManager - an entry point to memory management. Tracks the number of
// valid SNMP++.NET objects created in this and all other threads.
////////////////////////////////////////////////////////////////////////////////

public ref class MemoryManager sealed {

public:
	// Installs the memory manager which will collect all SNMP++.NET objects
	// created until Destroy() is called. Destroy() will destroy the objects
	// and de-install the manager. Managers should only be used within using():
	// every manager must be Dispose()d exactly once.
	static IMemoryManager^ GetMemoryManager() {
#ifdef	ENABLE_DISPOSE_MANAGER
		return gcnew RealMemoryManager();
#else
		return gcnew DummyMemoryManager();
#endif
	}

#define	LENGTH(array)	(sizeof(array)/sizeof(array[0]))

	static long long GetHeapsMemory() {

		HANDLE heaps[256];
		DWORD nheaps = ::GetProcessHeaps(LENGTH(heaps), heaps);
		if (nheaps > LENGTH(heaps)) {
			throw gcnew ApplicationException("too many heaps: " + nheaps);
		}

		PROCESS_HEAP_ENTRY phe;
		long long memory = 0;
		for (DWORD i = 0; i < nheaps; i++) {
			int error;
			if (!::HeapLock(heaps[i])) {
				error = ::GetLastError();
				return error < 0 ? error : -error;
			}

			phe.lpData = 0;
			while (::HeapWalk(heaps[i], &phe)) {
				if ((phe.wFlags & PROCESS_HEAP_ENTRY_BUSY) != 0) {
					memory += phe.cbData + phe.cbOverhead;
				}
			}
			error = ::GetLastError();

			::HeapUnlock(heaps[i]);

			if (error != ERROR_NO_MORE_ITEMS)
				return error < 0 ? error : -error;
		}
		return memory;
	}

	property static int Count {
		int get() {
			return m_count;
		}
	}

	property static int ThreadCount {
		int get() {
			return m_threadCount;
		}
	}
internal:
	static void ObjectCreatedNoAdd(IPrivateDisposable^ , const void* p) {

		// It is assumed that empty pointer is an error
		assert(p);

		Interlocked::Increment(m_count);
		m_threadCount++;
	}

	static void ObjectCreated(IPrivateDisposable^ o, const void* p) {

		ObjectCreatedNoAdd(o, p);

#ifdef	ENABLE_DISPOSE_MANAGER
		RealMemoryManager::TryAdd(o);
#endif
	}

	static void ObjectDeleted(IPrivateDisposable^, const void* p) {

		// Update object count only if ObjectCreated() was previously
		// called for this object
		if (p) {
			int count = Interlocked::Decrement(m_count);
			assert(count >= 0);
#ifdef	_DEBUG
			if (count == 0) {
				System::IO::TextWriter^ out = Console::Out;
				if (out)
					try {
						out->WriteLine("All native SNMP++ objects deleted!");
						out->Flush();
					}
					catch (ObjectDisposedException^) {}
			}
#endif
			m_threadCount--;
		}
	}
private:
	MemoryManager() {}

	static int m_count;
	[ThreadStatic]
	static int m_threadCount = 0;
};

////////////////////////////////////////////////////////////////////////////////
// Enumerator - a general-purpose enumerator used in classes that also provide
// indexers (usually, get_Item()).
////////////////////////////////////////////////////////////////////////////////

// Return the number of items in a collection.
private delegate int Delegate_Count();
// Return i-th element or throw IndexOutOfRangeException if i invalid.
private delegate Object^ Delegate_Get(int i);

private ref class Enumerator sealed : public IEnumerator {

public:
	Enumerator(Delegate_Count^ count, Delegate_Get^ get)
		: m_count(count), m_get(get), m_index(-1) {}

	virtual bool MoveNext() {
		return ++m_index < m_count();
	}

	virtual void Reset() {
		m_index = -1;
	}

	property Object^ Current {

		virtual Object^ get() {
			try {
				if (m_index >= 0) {
					return m_get(m_index);
				}
			}
			catch (IndexOutOfRangeException^) {}

			throw gcnew InvalidOperationException();
		}
	}
private:
	Delegate_Count^	const m_count;
	Delegate_Get^	const m_get;
	int				m_index;
};

////////////////////////////////////////////////////////////////////////////////
// Enumerations.
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// IpVersion.
////////////////////////////////////////////////////////////////////////////////

public enum class IpVersion {
	IPv4 = ::Address::version_ipv4,
	IPv6 = ::Address::version_ipv6
};

////////////////////////////////////////////////////////////////////////////////
// SnmpVersion.
////////////////////////////////////////////////////////////////////////////////

public enum class SnmpVersion {
	SNMPv1	= version1,
	SNMPv2c	= version2c,
	SNMPv3	= version3
};

////////////////////////////////////////////////////////////////////////////////
// SecurityModel.
////////////////////////////////////////////////////////////////////////////////

public enum class SecurityModel {
	Any     = SNMP_SECURITY_MODEL_ANY,
	SNMPv1  = SNMP_SECURITY_MODEL_V1,
	SNMPv2c = SNMP_SECURITY_MODEL_V2,
	USM	    = SNMP_SECURITY_MODEL_USM
};

////////////////////////////////////////////////////////////////////////////////
// SecurityLevel.
////////////////////////////////////////////////////////////////////////////////

public enum class SecurityLevel {
	NoAuthNoPriv = SNMP_SECURITY_LEVEL_NOAUTH_NOPRIV,
	AuthNoPriv   = SNMP_SECURITY_LEVEL_AUTH_NOPRIV,
	AuthPriv     = SNMP_SECURITY_LEVEL_AUTH_PRIV
};

////////////////////////////////////////////////////////////////////////////////
// AuthProtocol.
////////////////////////////////////////////////////////////////////////////////

public enum class AuthProtocol {
	None = SNMP_AUTHPROTOCOL_NONE,
	MD5  = SNMP_AUTHPROTOCOL_HMACMD5,
	SHA  = SNMP_AUTHPROTOCOL_HMACSHA
};

////////////////////////////////////////////////////////////////////////////////
// PrivProtocol.
////////////////////////////////////////////////////////////////////////////////

public enum class PrivProtocol {
	None   = SNMP_PRIVPROTOCOL_NONE,
	DES    = SNMP_PRIVPROTOCOL_DES,
	IDEA   = SNMP_PRIVPROTOCOL_IDEA,
	AES128 = SNMP_PRIVPROTOCOL_AES128,
	AES192 = SNMP_PRIVPROTOCOL_AES192,
	AES256 = SNMP_PRIVPROTOCOL_AES256,
	DES3   = SNMP_PRIVPROTOCOL_3DESEDE
};

////////////////////////////////////////////////////////////////////////////////
// SmiSyntax.
////////////////////////////////////////////////////////////////////////////////

public enum class SmiSyntax {
	Int            = sNMP_SYNTAX_INT,
	Bits           = sNMP_SYNTAX_BITS,
	OctetString    = sNMP_SYNTAX_OCTETS,
	Null           = sNMP_SYNTAX_NULL,
	Oid            = sNMP_SYNTAX_OID,
	Int32          = sNMP_SYNTAX_INT32,
	IpAddress      = sNMP_SYNTAX_IPADDR,
	Counter32      = sNMP_SYNTAX_CNTR32,
	Gauge32        = sNMP_SYNTAX_GAUGE32,
	TimeTicks      = sNMP_SYNTAX_TIMETICKS,
	Opaque         = sNMP_SYNTAX_OPAQUE,
	Counter64      = sNMP_SYNTAX_CNTR64,
	UInt32         = sNMP_SYNTAX_UINT32,
	NoSuchInstance = sNMP_SYNTAX_NOSUCHINSTANCE,
	NoSuchObject   = sNMP_SYNTAX_NOSUCHOBJECT,
	EndOfMibView   = sNMP_SYNTAX_ENDOFMIBVIEW
};

////////////////////////////////////////////////////////////////////////////////
// SnmpError.
////////////////////////////////////////////////////////////////////////////////

public enum class SnmpError {
	Success             = SNMP_ERROR_SUCCESS,
	TooBig              = SNMP_ERROR_TOO_BIG,
	NoSuchName          = SNMP_ERROR_NO_SUCH_NAME,
	BadValue            = SNMP_ERROR_BAD_VALUE,
	ReadOnly            = SNMP_ERROR_READ_ONLY,
	VbError             = SNMP_ERROR_GENERAL_VB_ERR,
	NoAccess            = SNMP_ERROR_NO_ACCESS,
	WrongType           = SNMP_ERROR_WRONG_TYPE,
	WrongLength         = SNMP_ERROR_WRONG_LENGTH,
	WrongEncoding       = SNMP_ERROR_WRONG_ENCODING,
	WrongValue          = SNMP_ERROR_WRONG_VALUE,
	NoCreation          = SNMP_ERROR_NO_CREATION,
	InconsistentValue   = SNMP_ERROR_INCONSIST_VAL,
	ResourceUnavailable = SNMP_ERROR_RESOURCE_UNAVAIL,
	CommitFailed        = SNMP_ERROR_COMITFAIL,
	UndoFailed          = SNMP_ERROR_UNDO_FAIL,
	AuthorizationError  = SNMP_ERROR_AUTH_ERR,
	NotWritable         = SNMP_ERROR_NOT_WRITEABLE,	// seems that "writable" is correct ;-)
	InconsistentName    = SNMP_ERROR_INCONSIS_NAME
};

////////////////////////////////////////////////////////////////////////////////
// ExceptionStatus.
////////////////////////////////////////////////////////////////////////////////

public enum class ExceptionStatus {
	NoSuchInstance = sNMP_SYNTAX_NOSUCHINSTANCE,
	NoSuchObject   = sNMP_SYNTAX_NOSUCHOBJECT,
	EndOfMibView   = sNMP_SYNTAX_ENDOFMIBVIEW
};

////////////////////////////////////////////////////////////////////////////////
// Snmp_ppStatus.
////////////////////////////////////////////////////////////////////////////////

public enum class Snmp_ppStatus {
	Success             = SNMP_CLASS_SUCCESS,
	Error               = SNMP_CLASS_ERROR,
	ResourceUnavailable = SNMP_CLASS_RESOURCE_UNAVAIL,
	InternalError       = SNMP_CLASS_INTERNAL_ERROR,
	Unsupported         = SNMP_CLASS_UNSUPPORTED,
	Timeout             = SNMP_CLASS_TIMEOUT,
	AsyncResponse       = SNMP_CLASS_ASYNC_RESPONSE,
	Notification        = SNMP_CLASS_NOTIFICATION,
	SessionDestroyed    = SNMP_CLASS_SESSION_DESTROYED,
	Invalid             = SNMP_CLASS_INVALID,
	InvalidPdu          = SNMP_CLASS_INVALID_PDU,
	InvalidTarget       = SNMP_CLASS_INVALID_TARGET,
	InvalidCallback     = SNMP_CLASS_INVALID_CALLBACK,
	InvalidReqId        = SNMP_CLASS_INVALID_REQID,
	InvalidNotifyId     = SNMP_CLASS_INVALID_NOTIFYID,
	InvalidOperation    = SNMP_CLASS_INVALID_OPERATION,
	InvalidOid          = SNMP_CLASS_INVALID_OID,
	InvalidAddress      = SNMP_CLASS_INVALID_ADDRESS,
	ErrStatusSet		= SNMP_CLASS_ERR_STATUS_SET,
	TlUnsupported       = SNMP_CLASS_TL_UNSUPPORTED,
	TlInUse             = SNMP_CLASS_TL_IN_USE,
	TlFailed            = SNMP_CLASS_TL_FAILED,
	Shutdown            = SNMP_CLASS_SHUTDOWN,
	BadVersion          = SNMP_CLASS_BADVERSION,
	Asn1Error           = SNMP_CLASS_ASN1ERROR
};

////////////////////////////////////////////////////////////////////////////////
// AddressType.
////////////////////////////////////////////////////////////////////////////////

public enum class AddressType {
	IP      = ::Address::type_ip,
	IPX     = ::Address::type_ipx,
	UDP     = ::Address::type_udp,
	IPXSOCK = ::Address::type_ipxsock,
	MAC     = ::Address::type_mac,
	Invalid = ::Address::type_invalid
};

////////////////////////////////////////////////////////////////////////////////
// PduType.
////////////////////////////////////////////////////////////////////////////////

public enum class PduType {
	Get      = sNMP_PDU_GET,
	GetNext  = sNMP_PDU_GETNEXT,
	Response = sNMP_PDU_RESPONSE,
	Set      = sNMP_PDU_SET,
	V1Trap   = sNMP_PDU_V1TRAP,
	GetBulk  = sNMP_PDU_GETBULK,
	Inform   = sNMP_PDU_INFORM,
	Trap     = sNMP_PDU_TRAP,
	Report   = sNMP_PDU_REPORT
};

////////////////////////////////////////////////////////////////////////////////
// RowStatus.
////////////////////////////////////////////////////////////////////////////////

public enum class RowStatus {
	Active			= 1,
    NotInService	= 2,
    NotReady		= 3,
    CreateAndGo		= 4,
    CreateAndWait	= 5,
    Destroy			= 6	
};

////////////////////////////////////////////////////////////////////////////////
// Exception classes.
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// SnmpClassException - this class is used to notify managed code about errors
// in the ::Snmp class during operations and object construction (instead of
// int-based status codes).
////////////////////////////////////////////////////////////////////////////////

[Serializable]
public ref class SnmpClassException sealed : public ApplicationException {

public:
	SnmpClassException(Snmp_ppStatus status, String^ message)
		: ApplicationException(message), m_status(status) {}

	SnmpClassException(Snmp_ppStatus status, Exception^ exception)
		: ApplicationException(gcnew String(::Snmp::error_msg((int) status)), exception),
			m_status(status) {}

	SnmpClassException(Snmp_ppStatus status)
		: ApplicationException(gcnew String(::Snmp::error_msg((int) status))), m_status(status) {}

	SnmpClassException(String^ message)
		: ApplicationException(message), m_status(Snmp_ppStatus::Success) {}

	SnmpClassException() : m_status(Snmp_ppStatus::Success) {}

	property Snmp_ppStatus Status {
		Snmp_ppStatus get() {
			return m_status;
		}
	}
private:
	const Snmp_ppStatus m_status;
};

////////////////////////////////////////////////////////////////////////////////
// SnmpException - this class is used to notify managed code about SNMP protocol
// errors.
////////////////////////////////////////////////////////////////////////////////

[Serializable]
public ref class SnmpException sealed : public ApplicationException {

public:
	SnmpException(SnmpError status, int index, String^ message)
		: ApplicationException(message), m_error(status), m_index(index) {}

	SnmpException(SnmpError status, int index)
		: ApplicationException(
			gcnew String(::Snmp::error_msg((int) status))), m_error(status), m_index(index) {}

	SnmpException(String^ message, Exception^ exception)
		: ApplicationException(message, exception), m_error(SnmpError::Success), m_index(-1) {}

	SnmpException(String^ message)
		: ApplicationException(message), m_error(SnmpError::Success), m_index(-1) {}

	SnmpException() : m_error(SnmpError::Success), m_index(-1) {}

	virtual String^ ToString() override {
		return String::Concat("Error: ", Message,
			"; status: ", m_error.ToString(),
			"; index: ", m_index.ToString());
	}

	property SnmpError ErrorStatus {
		SnmpError get() {
			return m_error;
		}
	}

	property int ErrorIndex {
		int get() {
			return m_index;
		}
	}
private:
	const SnmpError m_error;
	const int		m_index;
};

////////////////////////////////////////////////////////////////////////////////
// V3MpInitializationError - thrown when the V3MP initialization fails.
////////////////////////////////////////////////////////////////////////////////

[Serializable]
public ref class V3MpInitializationError sealed : public ApplicationException {

public:
	V3MpInitializationError(String^ message) : ApplicationException(message) {}

	V3MpInitializationError(int status) : m_status(status) {}

	V3MpInitializationError() {}

	property int InitStatus {
		int get() {
			return m_status;
		}
	}
private:
	int m_status;
};

////////////////////////////////////////////////////////////////////////////////
// SMI syntax classes.
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// SnmpSyntax.
////////////////////////////////////////////////////////////////////////////////

public ref class SnmpSyntax abstract : public IPrivateDisposable, public IComparable {

public:
	!SnmpSyntax() {
		InternalDispose(false);
	}

	virtual int CompareTo(Object^ o) = 0;

	property ::Org::Snmp::Snmp_pp::SmiSyntax SmiSyntax {
		::Org::Snmp::Snmp_pp::SmiSyntax get() {
			return (::Org::Snmp::Snmp_pp::SmiSyntax)
				GCReturnT<int>(this, m_value->get_syntax());
		}
	}

	static ::Org::Snmp::Snmp_pp::SmiSyntax GetSyntax(String^ syntax) {

		int index = Array::BinarySearch(m_names,
			syntax->ToLower(System::Globalization::CultureInfo::InvariantCulture));
		if (index >= 0) {
			return m_syntax[index];
		}
		else {
			throw gcnew SnmpClassException(Snmp_ppStatus::Invalid, syntax);
		}
	}

	property static array<String^>^ SupportedSyntaxNames {
		array<String^>^ get() {
			return dynamic_cast<array<String^>^>(m_names->Clone());
		}
	}
internal:
	SnmpSyntax(auto_ptr<::SnmpSyntax>& value) : m_value(value.release()) {
		OBJECT_CREATED(m_value);
		GC::KeepAlive(this);
	}

	virtual void InternalDispose() override {
		InternalDispose(true);
		GC::SuppressFinalize(this);
	}

	// This member function should be called when the underlying unmanaged
	// object has been changed, in order to update cached hash value. Note
	// that the design assumes that SnmpSyntax objects are immutable; thus,
	// this function may *only* be called on a newly created managed object,
	// before its managed reference is returned to the client code.
	virtual void UpdateHash() {}

	const ::SnmpSyntax& WrappedRef() {
		return *m_value;
	}
private protected:
	virtual void InternalDispose(bool /* disposing */) {
		DELETE_NATIVE(m_value);
	}
private:
	static array<String^>^ GetUniqueLowerNames(array<String^>^ n) {

		for (int i = 0; i < n->Length; i++) {
			n[i] = n[i]->ToLower(System::Globalization::CultureInfo::InvariantCulture);
		}
		Array::Sort(n);

		String^ last = n[0];
		int ndiff = 1;
		for (int i = 1; i < n->Length; i++) {
			if (!last->Equals(n[i])) {
				n[ndiff++] = n[i];
				last = n[i];
			}
		}

		array<String^>^ un = gcnew array<String^>(ndiff);
		Array::Copy(n, 0, un, 0, ndiff);
		return un;
	}

	static Array^ Glue(Array^ a1, Array^ a2) {

		Array^ a = Array::CreateInstance(
			a1->GetType()->GetElementType(), a1->Length + a2->Length);
		Array::Copy(a1, 0, a, 0, a1->Length);
		Array::Copy(a2, 0, a, a1->Length, a2->Length);
		return a;
	}

	static SnmpSyntax() {

		array<String^>^ enums = Enum::GetNames(::Org::Snmp::Snmp_pp::SmiSyntax::typeid);
		array<String^>^ names = {
			"i", "integer", "integer32",
			"b", "bits",
			"s", "octetstring",
			"n", "null",
			"o", "oid",
			"ipaddress",
			"c", "counter", "counter32",
			"g", "gauge", "gauge32",
			"t", "timeticks",
			"opaque",
			"counter64",
			"u", "unsigned", "unsigned32"};
		array<::Org::Snmp::Snmp_pp::SmiSyntax>^ evals
			= dynamic_cast<array<::Org::Snmp::Snmp_pp::SmiSyntax>^>(
			Enum::GetValues(::Org::Snmp::Snmp_pp::SmiSyntax::typeid));
		array<::Org::Snmp::Snmp_pp::SmiSyntax>^ _nvals = {
			::Org::Snmp::Snmp_pp::SmiSyntax::Int, ::Org::Snmp::Snmp_pp::SmiSyntax::Int, ::Org::Snmp::Snmp_pp::SmiSyntax::Int,
			::Org::Snmp::Snmp_pp::SmiSyntax::Bits, ::Org::Snmp::Snmp_pp::SmiSyntax::Bits,
			::Org::Snmp::Snmp_pp::SmiSyntax::OctetString, ::Org::Snmp::Snmp_pp::SmiSyntax::OctetString,
			::Org::Snmp::Snmp_pp::SmiSyntax::Null, ::Org::Snmp::Snmp_pp::SmiSyntax::Null,
			::Org::Snmp::Snmp_pp::SmiSyntax::Oid, ::Org::Snmp::Snmp_pp::SmiSyntax::Oid,
			::Org::Snmp::Snmp_pp::SmiSyntax::IpAddress,
			::Org::Snmp::Snmp_pp::SmiSyntax::Counter32, ::Org::Snmp::Snmp_pp::SmiSyntax::Counter32, ::Org::Snmp::Snmp_pp::SmiSyntax::Counter32,
			::Org::Snmp::Snmp_pp::SmiSyntax::Gauge32, ::Org::Snmp::Snmp_pp::SmiSyntax::Gauge32, ::Org::Snmp::Snmp_pp::SmiSyntax::Gauge32,
			::Org::Snmp::Snmp_pp::SmiSyntax::TimeTicks, ::Org::Snmp::Snmp_pp::SmiSyntax::TimeTicks,
			::Org::Snmp::Snmp_pp::SmiSyntax::Opaque,
			::Org::Snmp::Snmp_pp::SmiSyntax::Counter64,
			::Org::Snmp::Snmp_pp::SmiSyntax::UInt32, ::Org::Snmp::Snmp_pp::SmiSyntax::UInt32, ::Org::Snmp::Snmp_pp::SmiSyntax::UInt32};
		// I am not sure why (is this a managed C++ bug?) the array created above
		// has an invalid element type.  Will be ok when created dynamically.
		array<::Org::Snmp::Snmp_pp::SmiSyntax>^ nvals
			= dynamic_cast<array<::Org::Snmp::Snmp_pp::SmiSyntax>^>(
			Array::CreateInstance(::Org::Snmp::Snmp_pp::SmiSyntax::typeid, _nvals->Length));
		for (int i = 0; i < _nvals->Length; i++) {
			nvals[i] = (::Org::Snmp::Snmp_pp::SmiSyntax) _nvals[i];
		}

		array<String^>^ n = dynamic_cast<array<String^>^>(Glue(enums, names));
		array<String^>^ ncopy = dynamic_cast<array<String^>^>(n->Clone());
		m_names = GetUniqueLowerNames(n);

		nvals[0] = ::Org::Snmp::Snmp_pp::SmiSyntax::Int;
		array<::Org::Snmp::Snmp_pp::SmiSyntax>^ s
			= dynamic_cast<array<::Org::Snmp::Snmp_pp::SmiSyntax>^>(Glue(evals,  nvals));
		m_syntax = gcnew array<::Org::Snmp::Snmp_pp::SmiSyntax>(m_names->Length);

		for (int i = 0; i < m_names->Length; i++) {
			int npos;
			for (npos = 0; String::Compare(m_names[i], ncopy[npos], true,
				System::Globalization::CultureInfo::InvariantCulture) != 0; npos++)
				;
			m_syntax[i] = s[npos];
		}
	}

	static array<::Org::Snmp::Snmp_pp::SmiSyntax>^	m_syntax;
	static array<String^>^	m_names;
	::SnmpSyntax*	m_value;
};

////////////////////////////////////////////////////////////////////////////////
// SnmpInt32.
////////////////////////////////////////////////////////////////////////////////

public ref class SnmpInt32 sealed : public SnmpSyntax {

public:
	SnmpInt32(int i)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(UnmanagedNew2T<::SnmpInt32, int>(i))) {}

	virtual int GetHashCode() override {
		return GCReturnT<long>(this,
			(long) dynamic_cast<const ::SnmpInt32&>(WrappedRef()));
	}

	virtual bool Equals(Object^ o) override {

		if (!o || GetType() != o->GetType()) {
			return false;
		}
		SnmpInt32^ pint = dynamic_cast<SnmpInt32^>(o);
		return GCReturn2T<bool>(this, o,
				(long) dynamic_cast<const ::SnmpInt32&>(WrappedRef())
			==  (long) dynamic_cast<const ::SnmpInt32&>(pint->WrappedRef()));
	}

	virtual String^ ToString() override {
		return Value.ToString();
	}

	virtual int CompareTo(Object^ o) override {

		if (!o) {
			return 1;
		}
		if (GetType() != o->GetType()) {
			throw gcnew ArgumentException("Invalid argument type", "o");
		}
		SnmpInt32^ pint = dynamic_cast<SnmpInt32^>(o);
		long me = (long) dynamic_cast<const ::SnmpInt32&>(WrappedRef()),
			 he = (long) dynamic_cast<const ::SnmpInt32&>(pint->WrappedRef());
		return GCReturn2T<int>(this, o, me < he ? -1 : (me != he));
	}

	property int Value {
		int get() {
			return GCReturnT<int>(this,
				(long) dynamic_cast<const ::SnmpInt32&>(WrappedRef()));
		}
	}

	static operator int (SnmpInt32^ pint) {
		return pint->Value;
	}
internal:
	SnmpInt32(auto_ptr<::SnmpInt32>& aint)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(aint)) {}

	SnmpInt32()
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(UnmanagedNew<::SnmpInt32>())) {}
};

////////////////////////////////////////////////////////////////////////////////
// SnmpUint32.
////////////////////////////////////////////////////////////////////////////////

public ref class SnmpUInt32 : public SnmpSyntax {

public:
	[CLSCompliant(false)]
	SnmpUInt32(unsigned int i)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(
			UnmanagedNew2T<::SnmpUInt32, unsigned long>((unsigned long) i))) {}

	SnmpUInt32(long long i)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(
			UnmanagedNew2T<::SnmpUInt32, unsigned long>(ArgToULong(i, "i")))) {}

	virtual int GetHashCode() override {
		return GCReturnT<int>(this,
			(unsigned long) dynamic_cast<const ::SnmpUInt32&>(WrappedRef()));
	}

	virtual bool Equals(Object^ o) override {

		if (!o || GetType() != o->GetType()) {
			return false;
		}
		SnmpUInt32^ puint = dynamic_cast<SnmpUInt32^>(o);
		return GCReturn2T<bool>(this, o,
				(unsigned long) dynamic_cast<const ::SnmpUInt32&>(WrappedRef())
			==  (unsigned long) dynamic_cast<const ::SnmpUInt32&>(puint->WrappedRef()));
	}

	virtual String^ ToString() override {
		return Value.ToString();
	}

	virtual int CompareTo(Object^ o) override {

		if (!o) {
			return 1;
		}
		if (GetType() != o->GetType()) {
			throw gcnew ArgumentException("Invalid argument type", "o");
		}
		SnmpUInt32^ puint = dynamic_cast<SnmpUInt32^>(o);
		unsigned long me = (unsigned long) dynamic_cast<const ::SnmpUInt32&>(WrappedRef()),
					  he = (unsigned long) dynamic_cast<const ::SnmpUInt32&>(puint->WrappedRef());
		return GCReturn2T<int>(this, o, me < he ? -1 : (me != he));
	}

	[CLSCompliant(false)]
	property unsigned int Value {
		unsigned int get() {
			return GCReturnT<unsigned long>(this,
				(unsigned long) dynamic_cast<const ::SnmpUInt32&>(WrappedRef()));
		}
	}

	property long long ValueAsInt64 {
		long long get() {
			return Value;
		}
	}

	[CLSCompliant(false)]
	static operator unsigned int (SnmpUInt32^ obj) {
		return obj->Value;
	}

	static operator long long (SnmpUInt32^ obj) {
		return obj->Value;
	}
internal:
	SnmpUInt32(auto_ptr<::SnmpUInt32>& auint)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(auint)) {}
};

////////////////////////////////////////////////////////////////////////////////
// Counter32.
////////////////////////////////////////////////////////////////////////////////

public ref class Counter32 sealed : public SnmpUInt32 {

public:
	[CLSCompliant(false)]
	Counter32(unsigned int value)
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(
			UnmanagedNew2T<::Counter32, unsigned long>((unsigned long) value))) {}

	Counter32(long long value)
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(
			UnmanagedNew2T<::Counter32, unsigned long>(ArgToULong(value, "value")))) {}

	Counter32(SnmpUInt32^ value)
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(
			UnmanagedNew2T<::Counter32, unsigned long>((unsigned long)
				dynamic_cast<const ::SnmpUInt32&>(value->WrappedRef())))) {
		GC::KeepAlive(value);
	}
internal:
	Counter32()
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(UnmanagedNew<::Counter32>())) {}
};

////////////////////////////////////////////////////////////////////////////////
// TimeTicks.
////////////////////////////////////////////////////////////////////////////////

public ref class TimeTicks sealed : public SnmpUInt32 {

public:
	[CLSCompliant(false)]
	TimeTicks(unsigned int value)
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(
			UnmanagedNew2T<::TimeTicks, unsigned long>((unsigned long) value))) {}

	TimeTicks(long long value)
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(
			UnmanagedNew2T<::TimeTicks, unsigned long>(ArgToULong(value, "value")))) {}

	TimeTicks(SnmpUInt32^ value)
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(
			UnmanagedNew2T<::TimeTicks, unsigned long>((unsigned long)
				dynamic_cast<const ::SnmpUInt32&>(value->WrappedRef())))) {
		GC::KeepAlive(value);
	}

	virtual String^ ToString() override {
		return GCReturnT<String^>(this,
			SafePrintableT<::TimeTicks>(
				dynamic_cast<const ::TimeTicks&>(WrappedRef())));
	}
internal:
	TimeTicks()
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(UnmanagedNew<::TimeTicks>())) {}

	TimeTicks(auto_ptr<::TimeTicks>& aticks)
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(aticks)) {}
};

////////////////////////////////////////////////////////////////////////////////
// Gauge32.
////////////////////////////////////////////////////////////////////////////////

public ref class Gauge32 sealed : public SnmpUInt32 {

public:
	[CLSCompliant(false)]
	Gauge32(unsigned int value)
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(
			UnmanagedNew2T<::Gauge32, unsigned long>((unsigned long) value))) {}

	Gauge32(long long value)
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(
			UnmanagedNew2T<::Gauge32, unsigned long>(ArgToULong(value, "value")))) {}

	Gauge32(SnmpUInt32^ value)
		: SnmpUInt32(auto_ptr<::SnmpUInt32>(
			UnmanagedNew2T<::Gauge32, unsigned long>((unsigned long)
				dynamic_cast<const ::SnmpUInt32&>(value->WrappedRef())))) {
		GC::KeepAlive(value);
	}
internal:
	Gauge32() : SnmpUInt32(auto_ptr<::SnmpUInt32>(UnmanagedNew<::Gauge32>())) {}
};

////////////////////////////////////////////////////////////////////////////////
// Counter64.
// CLSCompliant type used here is long long (Int64), which is more convenient
// than separate values for 'high' and 'low' parts of an unsigned 64-bit word.
// Additionally, the only operations that make sense for counters are subtraction
// and comparison.
////////////////////////////////////////////////////////////////////////////////

public ref class Counter64 sealed : public SnmpSyntax {

public:
	[CLSCompliant(false)]
	Counter64(UInt64 value)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(CreateCounter64(value))) {}

	Counter64(long long value)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(CreateCounter64((UInt64) value))) {}

	virtual int GetHashCode() override {
		const ::Counter64& cnt64 = dynamic_cast<const ::Counter64&>(WrappedRef());
		return GCReturnT<int>(this, cnt64.high() ^ cnt64.low());
	}

	virtual bool Equals(Object^ o) override {

		if (!o || GetType() != o->GetType()) {
			return false;
		}
		Counter64^ pcnt64 = dynamic_cast<Counter64^>(o);
		return GCReturn2T<bool>(this, o,
				dynamic_cast<const ::Counter64&>(WrappedRef())
			==  dynamic_cast<const ::Counter64&>(pcnt64->WrappedRef()));
	}

	virtual String^ ToString() override {
		return Value.ToString();
	}

	virtual int CompareTo(Object^ o) override {

		if (!o) {
			return 1;
		}
		if (GetType() != o->GetType()) {
			throw gcnew ArgumentException("Invalid argument type", "o");
		}
		Counter64^ pcnt64 = dynamic_cast<Counter64^>(o);
		const ::Counter64& me = dynamic_cast<const ::Counter64&>(WrappedRef());
		const ::Counter64& he = dynamic_cast<const ::Counter64&>(pcnt64->WrappedRef());
		unsigned long meHigh = me.high(), meLow = me.low(),
					  heHigh = he.high(), heLow = he.low();
		return GCReturn2T<int>(this, o,
			meHigh != heHigh
				? (meHigh < heHigh ? -1 : 1)
				: (meLow  < heLow  ? -1 : (meLow != heLow)));
	}

	[CLSCompliant(false)]
	property UInt64 Value {

		UInt64 get() {
			const ::Counter64& cnt64 = dynamic_cast<const ::Counter64&>(WrappedRef());
			UInt64 low = cnt64.low(), high = cnt64.high();
			return GCReturnT<UInt64>(this, low | (high << 32));
		}
	}

	property long long ValueAsInt64 {
		long long get() {
			return (long long) Value;
		}
	}

	[CLSCompliant(false)]
	static operator UInt64 (Counter64^ cnt64) {
		return cnt64->Value;
	}

	static operator long long (Counter64^ cnt64) {
		return (long long) cnt64->Value;
	}
internal:
	Counter64() : SnmpSyntax(auto_ptr<::SnmpSyntax>(UnmanagedNew<::Counter64>())) {}
private:
	static ::Counter64* CreateCounter64(UInt64 value) {

		::Counter64* pcnt64 = UnmanagedNew<::Counter64>();
		pcnt64->set_low((unsigned long) value);
		pcnt64->set_high((unsigned long) (value >> 32));
		return pcnt64;
	}
};

////////////////////////////////////////////////////////////////////////////////
// Oid.
////////////////////////////////////////////////////////////////////////////////

public ref class Oid sealed : public SnmpSyntax, public IEnumerable {

public:
	static const int MaxLength = 128;

	Oid(String^ oid)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(CreateOid(oid))) {}

	[CLSCompliant(false)]
	Oid(array<unsigned int>^ oid)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(CreateOid(oid))) {}

	Oid(array<long long>^ oid)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(CreateOid(oid))) {}

	// Should we cache the hash value?  For long OIDs, it could be a good idea.
	// Note that although sub-ids belong to <0, 2147483647> range, they usually
	// are small (and fit in a single byte).
	virtual int GetHashCode() override {

		const ::Oid& oid = dynamic_cast<const ::Oid&>(WrappedRef());
		int hash = 0;
		for (unsigned int i = 0; i < oid.len(); i++) {
			hash = ((hash << 8) | ((unsigned int) hash >> 24)) ^ oid[i];
		}
		return GCReturnT<int>(this, hash);
	}

	virtual bool Equals(Object^ o) override {

		if (!o || GetType() != o->GetType()) {
			return false;
		}
		Oid^ oid = dynamic_cast<Oid^>(o);
		return GCReturn2T<bool>(this, o,
				dynamic_cast<const ::Oid&>(WrappedRef())
			==  dynamic_cast<const ::Oid&>(oid->WrappedRef()));
	}

	// ToString() must be overridden due to the following reasons:
	// - ::Oid::get_printable() is MT-unsafe
	// - it unnecessarily allocates and keeps memory
	virtual String^ ToString() override {

		const ::Oid& oid = dynamic_cast<const ::Oid&>(WrappedRef());
		if (oid.len() > 0) {
			StringBuilder^ sb = gcnew StringBuilder(2048);
			sb->Append((unsigned int) oid[0]);
			for (unsigned int i = 1; i < oid.len(); i++) {
				sb->Append(".");
				sb->Append((unsigned int) oid[i]);
			}
			return GCReturnT<String^>(this, sb->ToString());
		}
		else
			return "";
	}

	virtual int CompareTo(Object^ o) override {

		if (!o) {
			return 1;
		}
		if (GetType() != o->GetType()) {
			throw gcnew ArgumentException("Invalid argument type", "o");
		}
		return Compare(dynamic_cast<Oid^>(o));
	}

	virtual IEnumerator^ GetEnumerator() {
		return gcnew Enumerator(
			gcnew Delegate_Count(this, &::Org::Snmp::Snmp_pp::Oid::Length::get),
			gcnew Delegate_Get(this, &::Org::Snmp::Snmp_pp::Oid::Get));
	}

	Oid^ Append(Oid^ oid) {

		const ::Oid& thisRef = dynamic_cast<const ::Oid&>(WrappedRef());
		const ::Oid& thatRef = dynamic_cast<const ::Oid&>(oid->WrappedRef());
		auto_ptr<::Oid> aptr(UnmanagedNew<::Oid>(thisRef));
		::Oid& ref = *aptr;
		ref += thatRef;
		if (ref.len() != thisRef.len() + thatRef.len()) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		return GCReturn2T<Oid^>(this, oid, ManagedNew<Oid, ::Oid>(aptr));
	}

	Oid^ Append(int oid) {

		const ::Oid& thisRef = dynamic_cast<const ::Oid&>(WrappedRef());
		auto_ptr<::Oid> aptr(UnmanagedNew<::Oid>(thisRef));
		::Oid& ref = *aptr;
		ref += oid;
		if (ref.len() != thisRef.len() + 1) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		return GCReturnT<Oid^>(this, ManagedNew<Oid, ::Oid>(aptr));
	}

	Oid^ Trim(int n) {

		const ::Oid& oid = dynamic_cast<const ::Oid&>(WrappedRef());
		if (n < 0 || (unsigned int) n > oid.len()) {
			throw gcnew ArgumentException(
				"Invalid number of subidentifiers to trim", "n");
		}
		if (n == 0)
		{
			return this;
		}

		auto_ptr<::Oid> aptr(UnmanagedNew<::Oid>(oid));
		aptr->trim(n);
		return GCReturnT<Oid^>(this, ManagedNew<Oid, ::Oid>(aptr));
	}

	Oid^ Trim() {
		return Trim(1);
	}

	Oid^ TrimLeft(int n) {

		const ::Oid& oid = dynamic_cast<const ::Oid&>(WrappedRef());
		if (n < 0 || (unsigned int) n > oid.len()) {
			throw gcnew ArgumentException(
				"Invalid number of subidentifiers to trim", "n");
		}
		if (n == 0)
		{
			return this;
		}

		int oidLen = oid.len() - n;
		if (oidLen > MaxLength) {	// should not happen at all
			throw gcnew IndexOutOfRangeException();
		}

		unsigned long raw[MaxLength] = {0};
		::Oid* poid = new ::Oid(raw, oidLen);
		if (!poid) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		if (!poid->valid()) {
			delete poid;
			throw gcnew UnmanagedOutOfMemoryException();
		}
		for (unsigned int i = 0; i < poid->len(); i++)
			(*poid)[i] = oid[n + i];

		auto_ptr<::Oid> aptr(poid);
		return GCReturnT<Oid^>(this, ManagedNew<Oid, ::Oid>(aptr));
	}

	Oid^ TrimLeft() {
		return TrimLeft(1);
	}

	// Compare at most  'n' sub-identifiers starting from the direction
	// determined by 'fromLeft'. Return the position at which the first
	// difference occurs, increased by one, with the minus sign if 'oid'
	// is greater than this OID. 0 means equal values.
	int Compare(Oid^ oid, int n, bool fromLeft) {

		const ::Oid& o1 = dynamic_cast<const ::Oid&>(WrappedRef()),
					 o2 = dynamic_cast<const ::Oid&>(oid->WrappedRef());
		const unsigned int len1 = o1.len(), len2 = o2.len();
		const int m = (int) (len1 < len2 ? len1 : len2);
		const int max = m < n ? m : n;
		int i;

		if (fromLeft) {
			for (i = 0; i < max && o1[i] == o2[i]; i++)
				;
			if (i < max) {
				return GCReturn2T<int>(this, oid,
					o1[i] > o2[i] ? i + 1 : -(i + 1));
			}
		}
		else {
			for (i = 0; i < max && o1[len1 - i - 1] == o2[len2 - i - 1]; i++)
				;
			if (i < max) {
				return GCReturn2T<int>(this, oid,
					o1[len1 - i - 1] > o2[len2 - i - 1] ? i + 1 : -(i + 1));
			}
		}
		return GCReturn2T<int>(this, oid,
			max == n ? 0 : (++i, (len1 > len2 ? i : (len1 != len2 ? -i : 0))));
	}

	// As above but the whole OIDs are compared.
	int Compare(Oid^ oid, bool fromLeft) {
		return Compare(oid, System::Int32::MaxValue, fromLeft);
	}

	// As above but the the direction is left-to-right.
	int Compare(Oid^ oid) {
		return Compare(oid, System::Int32::MaxValue, true);
	}

	bool StartsWith(Oid^ oid) {
		return Compare(oid, oid->Length, true) == 0;
	}

	bool EndsWith(Oid^ oid) {
		return Compare(oid, oid->Length, false) == 0;
	}

	[CLSCompliant(false)]
	property unsigned int default[int] {

		unsigned int get(int position) {
			const ::Oid& oid = dynamic_cast<const ::Oid&>(WrappedRef());
			if (position < 0 || (unsigned int) position >= oid.len()) {
				throw gcnew IndexOutOfRangeException();
			}
			return GCReturnT<unsigned int>(this, oid[position]);
		}
	}

	property long long ItemAsInt64[int] {
		long long get(int position) {
			return this[position];
		}
	}

	property int Length {
		int get() {
			return GCReturnT<int>(this,
				dynamic_cast<const ::Oid&>(WrappedRef()).len());
		}
	}

	[CLSCompliant(false)]
	property array<unsigned int>^ Oids {

		array<unsigned int>^ get() {
			const ::Oid& roid = dynamic_cast<const ::Oid&>(WrappedRef());
			array<unsigned int>^ subids = gcnew array<System::UInt32>(roid.len());
			for (unsigned int i = 0; i < roid.len(); i++) {
				subids[i] = roid[i];
			}
			return GCReturnT<array<unsigned int>^>(this, subids);
		}
	}

	property array<long long>^ OidsAsInt64 {

		array<long long>^ get() {
			const ::Oid& roid = dynamic_cast<const ::Oid&>(WrappedRef());
			array<long long>^ subids = gcnew array<System::Int64>(roid.len());
			for (unsigned int i = 0; i < roid.len(); i++) {
				subids[i] = roid[i];
			}
			return GCReturnT<array<long long>^>(this, subids);
		}
	}

	[CLSCompliant(false)]
	static operator array<unsigned int>^ (Oid^ oid) {
		return oid->Oids;
	}

	static operator array<long long>^ (Oid^ oid) {
		return oid->OidsAsInt64;
	}

	static operator String^ (Oid^ oid) {
		return oid->ToString();
	}
internal:
	Oid() : SnmpSyntax(auto_ptr<::SnmpSyntax>(UnmanagedNew<::Oid>())) {}

	Oid(auto_ptr<::Oid>& aoid)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(aoid)) {}
private:
	Object^ Get(int pos) {
		return this[pos];
	}

	static ::Oid* CreateOid(String^ oid) {

		auto_array_ptr<char> aptr(NetString2Chars(oid));
		::Oid* oidp = UnmanagedNew2T<::Oid, char*>(const_cast<char*>(aptr.get()));
		if (!oidp->valid()) {
			delete oidp;
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidOid, oid);
		}
		return oidp;
	}

	static ::Oid* CreateOid(unsigned long* o, int len) {

		::Oid* oidp = new ::Oid(o, len);
		if (!oidp) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		if (!oidp->valid()) {
			delete oidp;
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidOid);
		}
		return oidp;
	}

	static ::Oid* CreateOid(array<unsigned int>^ oid) {

		unsigned long o[MaxLength];
		int len = oid->Length;
		if (len > sizeof(o)/sizeof(o[0])) {
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidOid);
		}

		// no Marshal.Copy() for UInt32
		for (int i = 0; i < len; i++) {
			o[i] = oid[i];
		}

		return CreateOid(o, len);
	}

	static ::Oid* CreateOid(array<long long>^ oid) {

		unsigned long o[MaxLength];
		int len = oid->Length;
		if (len > sizeof(o)/sizeof(o[0])) {
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidOid);
		}

		for (int i = 0; i < len; i++) {
			o[i] = ArgToULong(oid[i], "oid");
		}

		return CreateOid(o, len);
	}
};

////////////////////////////////////////////////////////////////////////////////
// OctetStr.
////////////////////////////////////////////////////////////////////////////////

public ref class OctetStr : public SnmpSyntax, public IEnumerable {

public:
	enum class OutputType
	{
		OutputHexAndClear = ::OctetStr::OutputHexAndClear,
		OutputHex         = ::OctetStr::OutputHex,
		OutputClear       = ::OctetStr::OutputClear
	};

	OctetStr(String^ str)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(CreateOctetStr(str))),
		m_hash(str->GetHashCode()) {}

	OctetStr(array<System::Byte>^ bytes)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(CreateOctetStr(bytes))),
		m_hash(ComputeHashCode()) {}

	virtual int GetHashCode() override {
		return m_hash;
	}

	virtual bool Equals(Object^ o) override {

		if (!o || GetType() != o->GetType()) {
			return false;
		}
		OctetStr^ pos = dynamic_cast<OctetStr^>(o);
		return GCReturn2T<bool>(this, o,
				dynamic_cast<const ::OctetStr&>(WrappedRef())
			==  dynamic_cast<const ::OctetStr&>(pos->WrappedRef()));
	}

	virtual int CompareTo(Object^ o) override {

		if (!o) {
			return 1;
		}
		if (GetType() != o->GetType()) {
			throw gcnew ArgumentException("Invalid argument type", "o");
		}
		OctetStr^ pos = dynamic_cast<OctetStr^>(o);
		return GCReturn2T<int>(this, o,
			dynamic_cast<const ::OctetStr&>(WrappedRef()).nCompare(
				System::UInt32::MaxValue,
				dynamic_cast<const ::OctetStr&>(pos->WrappedRef())));
	}

	// ToString() must be overridden due to the following reasons:
	// - ::OctetStr::get_printable() is MT-unsafe
	// - it unnecessarily allocates and keeps memory
	// - it does not correctly handle null characters.
	virtual String^ ToString() override {

		const ::OctetStr& os = dynamic_cast<const ::OctetStr&>(WrappedRef());
		wchar_t* buf = new wchar_t[os.len()];
		if (!buf) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		auto_array_ptr<wchar_t> aptr(buf);

		for (unsigned int i = 0; i < os.len(); i++) {
			buf[i] = os[i];
		}
		return GCReturnT<String^>(this, gcnew String(buf, 0, os.len()));
	}

	// get_printable_hex() must be re-implemented (see comments for ToString()).
	String^ ToHexString() {

		return GCReturnT<String^>(this,
			SafePrintableHexT<::OctetStr>(
				dynamic_cast<const ::OctetStr&>(WrappedRef())));
	}

	property System::Byte default[int] {

		System::Byte get(int position) {
			const ::OctetStr& os = dynamic_cast<const ::OctetStr&>(WrappedRef());
			if (position < 0 || (unsigned int) position >= os.len()) {
				throw gcnew IndexOutOfRangeException();
			}
			return GCReturnT<System::Byte>(this, os[position]);
		}
	}

	virtual IEnumerator^ GetEnumerator() {
		return gcnew Enumerator(
			gcnew Delegate_Count(this, &::Org::Snmp::Snmp_pp::OctetStr::Length::get),
			gcnew Delegate_Get(this, &::Org::Snmp::Snmp_pp::OctetStr::Get));
	}

	OctetStr^ Append(String^ str) {

		const ::OctetStr& thisRef = dynamic_cast<const ::OctetStr&>(WrappedRef());
		auto_array_ptr<char> aptr(NetString2Chars(str));
		auto_ptr<::OctetStr> aos(UnmanagedNew<::OctetStr>(thisRef));
		::OctetStr& ref = *aos.get();
		ref += aptr.get();
		if (ref.len() != thisRef.len() + ::strlen(aptr.get())) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		return GCReturnT<OctetStr^>(this,
			ManagedNew<OctetStr, ::OctetStr>(aos));
	}

	property int Length {
		int get() {
			return GCReturnT<int>(this,
				dynamic_cast<const ::OctetStr&>(WrappedRef()).len());
		}
	}

	property array<System::Byte>^ Bytes {

		array<System::Byte>^ get() {
			const ::OctetStr& os = dynamic_cast<const ::OctetStr&>(WrappedRef());
			array<System::Byte>^ bytes = gcnew array<System::Byte>(os.len());
			for (unsigned int i = 0; i < os.len(); i++) {
				bytes[i] = os[i];
			}
			return GCReturnT<array<System::Byte>^>(this, bytes);
		}
	}

	static operator array<System::Byte>^ (OctetStr^ pos) {
		return pos->Bytes;
	}

	static operator String^ (OctetStr^ pos) {
		return pos->ToString();
	}

	static OctetStr^ FromHexString(OctetStr^ str) {

		auto_ptr<::OctetStr> aptr(UnmanagedNew<::OctetStr>());
		::OctetStr& ref = *aptr;
		ref = ::OctetStr::from_hex_string(
					dynamic_cast<const ::OctetStr&>(str->WrappedRef()));
		if (!ref.valid()) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		return GCReturnT<OctetStr^>(str, ManagedNew<OctetStr, ::OctetStr>(aptr));
	}

	static property OutputType PrintOutputType {
		void set(OutputType ot) {
			::OctetStr::set_hex_output_type((::OctetStr::OutputType) ot);
		}
	}
internal:
	OctetStr()
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(UnmanagedNew<::OctetStr>())) {}

	OctetStr(auto_ptr<::OctetStr>& s)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(s)) {
		m_hash = ComputeHashCode();
	}

	virtual void UpdateHash() override {
		m_hash = ComputeHashCode();
	}
private:
	Object^ Get(int pos) {
		return this[pos];
	}

	int ComputeHashCode() {

		const ::OctetStr& os = dynamic_cast<const ::OctetStr&>(WrappedRef());
		int hash = 0;
		for (unsigned int i = 0; i < os.len(); i++) {
			hash = ((hash << 8) | ((unsigned int) hash >> 24)) ^ os[i];
		}
		return GCReturnT<int>(this, hash);
	}

	static ::OctetStr* CreateOctetStr(const unsigned char* buf, int length) {

		::OctetStr* pos = new ::OctetStr(buf, length);
		if (!pos) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		return pos;
	}

	static ::OctetStr* CreateOctetStr(String^ str) {

		int length = 0;
		auto_array_ptr<unsigned char> aptr(NetString2UChars(str, length));
		return CreateOctetStr(aptr.get(), length);
	}

	static ::OctetStr* CreateOctetStr(array<System::Byte>^ bytes) {

		unsigned char* buf = new unsigned char[bytes->Length];
		if (!buf) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		auto_array_ptr<unsigned char> aptr(buf);

		for (int i = 0; i < bytes->Length; i++) {
			buf[i] = bytes[i];
		}
		return CreateOctetStr(buf, bytes->Length);
	}

	int m_hash;
};

////////////////////////////////////////////////////////////////////////////////
// OpaqueStr.
////////////////////////////////////////////////////////////////////////////////

public ref class OpaqueStr sealed : public OctetStr {

public:
	OpaqueStr(String^ str) : OctetStr(str) {}

	OpaqueStr(OctetStr^ str)
		: OctetStr(auto_ptr<::OctetStr>(CreateOpaqueStr(str))) {}
internal:
	OpaqueStr()
		: OctetStr(auto_ptr<::OctetStr>(UnmanagedNew<::OpaqueStr>())) {}
private:
	static ::OctetStr* CreateOpaqueStr(OctetStr^ str) {
		return GCReturnT<::OctetStr*>(str,
			UnmanagedNew<::OctetStr>(
				dynamic_cast<const ::OctetStr&>(str->WrappedRef())));
	}
};

////////////////////////////////////////////////////////////////////////////////
// SnmpExceptionStatus.
////////////////////////////////////////////////////////////////////////////////

class UnmanagedSnmpExceptionStatus : public ::SnmpInt32 {

public:
	explicit UnmanagedSnmpExceptionStatus(ExceptionStatus status)
		: ::SnmpInt32((int) status) {}

	virtual SmiUINT32 get_syntax() const {
		return (long) *this;
	}
};

public ref class SnmpExceptionStatus sealed : public SnmpSyntax {

public:
	SnmpExceptionStatus(ExceptionStatus status)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(
			UnmanagedNew2T<UnmanagedSnmpExceptionStatus, ExceptionStatus>(status))) {}

	virtual int GetHashCode() override {
		return GCReturnT<int>(this,
			(long) dynamic_cast<const UnmanagedSnmpExceptionStatus&>(WrappedRef()));
	}

	virtual bool Equals(Object^ o) override {

		if (!o || GetType() != o->GetType()) {
			return false;
		}
		SnmpExceptionStatus^ pes = dynamic_cast<SnmpExceptionStatus^>(o);
		return GCReturn2T<bool>(this, o,
				(long) dynamic_cast<const UnmanagedSnmpExceptionStatus&>(WrappedRef())
			==  (long) dynamic_cast<const UnmanagedSnmpExceptionStatus&>(pes->WrappedRef()));
	}

	virtual int CompareTo(Object^ o) override {

		if (!o) {
			return 1;
		}
		if (GetType() != o->GetType()) {
			throw gcnew ArgumentException("Invalid argument type", "o");
		}
		SnmpExceptionStatus^ pes = dynamic_cast<SnmpExceptionStatus^>(o);
		long me = (long) dynamic_cast<const UnmanagedSnmpExceptionStatus&>(WrappedRef()),
			 he = (long) dynamic_cast<const UnmanagedSnmpExceptionStatus&>(pes->WrappedRef());
		return GCReturn2T<int>(this, o, me < he ? -1 : (me != he));
	}

	// also available through get_Syntax() 
	property ExceptionStatus Value {
		ExceptionStatus get() {
			return (ExceptionStatus) GCReturnT<long>(this,
				(long) dynamic_cast<const UnmanagedSnmpExceptionStatus&>(WrappedRef()));
		}
	}

	static operator ExceptionStatus (SnmpExceptionStatus^ status) {
		return GCReturnT<ExceptionStatus>(status, status->Value);
	}
};

////////////////////////////////////////////////////////////////////////////////
// Address functions & classes.
////////////////////////////////////////////////////////////////////////////////

template<typename UT>
UT* CreateAddress(String^ s) {

	auto_array_ptr<char> aptr(NetString2Chars(s));
	UT* addr = UnmanagedNew2T<UT, char*>(aptr.get());
	if (!addr->valid()) {
		delete addr;
		throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress, s);
	}
	return addr;
}

ref class GenAddress;

template<typename UT>
UT* CreateAddress(GenAddress^ addr) {

	UT* a = UnmanagedNew2T<UT, ::GenAddress>(
		dynamic_cast<const ::GenAddress&>(addr->WrappedRef()));
	if (!a->valid()) {
		delete a;
		throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress, addr->ToString());
	}
	return GCReturnT<UT*>(addr, a);
}

////////////////////////////////////////////////////////////////////////////////
// Address.
////////////////////////////////////////////////////////////////////////////////

public ref class Address abstract : public SnmpSyntax, public IEnumerable {

public:
	virtual int GetHashCode() override {
		return GCReturnT<int>(this, GetHashCode(WrappedRef()));
	}

	virtual bool Equals(Object^ o) override {

		if (!o || GetType() != o->GetType()) {
			return false;
		}
		// We use the standard operator== and assume that it does not use
		// "mutable" fields (e.g., friendly name).
		::Org::Snmp::Snmp_pp::Address^ paddr = dynamic_cast<::Org::Snmp::Snmp_pp::Address^>(o);
		return GCReturn2T<bool>(this, o, WrappedRef() == paddr->WrappedRef());
	}

	virtual String^ ToString() override {
		return GCReturnT<String^>(this,
			SafePrintable2T<::GenAddress, ::Address>(WrappedRef()));
	}

	// Addresses are compared byte-by-byte.
	virtual int CompareTo(Object^ o) override {

		if (!o) {
			return 1;
		}
		if (GetType() != o->GetType()) {
			throw gcnew ArgumentException("Invalid argument type", "o");
		}
		const ::Address & thisAddr = WrappedRef(),
						& thatAddr = dynamic_cast<::Org::Snmp::Snmp_pp::Address^>(o)->WrappedRef();
		int len = thisAddr.get_length();
		if (len > thatAddr.get_length())
			len = thatAddr.get_length();
		for (int i = 0; i < len; i++) {
			int diff = (int) thisAddr[i] - (int) thatAddr[i];
			if (diff)
				return diff;
		}
		return GCReturn2T<int>(this, o,
			thisAddr.get_length() - thatAddr.get_length());
	}

	// This function has been removed from API. Use GetHashCode().
	/*
	int HashFunction() {
		return GCReturnT<int>(this, WrappedRef().hashFunction());
	}
	*/

	virtual IEnumerator^ GetEnumerator() {
		return gcnew Enumerator(
			gcnew Delegate_Count(this, &::Org::Snmp::Snmp_pp::Address::Length::get),
			gcnew Delegate_Get(this, &::Org::Snmp::Snmp_pp::Address::Get));
	}

	property ::Org::Snmp::Snmp_pp::AddressType AddressType {
		::Org::Snmp::Snmp_pp::AddressType get() {
			return (::Org::Snmp::Snmp_pp::AddressType)
				GCReturnT<int>(this, WrappedRef().get_type());
		}
	}

	property System::Byte default[int] {

		System::Byte get(int position) {
			if (position < 0 || position >= WrappedRef().get_length()) {
				throw gcnew IndexOutOfRangeException();
			}
			return GCReturnT<System::Byte>(this, WrappedRef()[position]);
		}
	}

	property int Length {
		int get() {
			return GCReturnT<int>(this, WrappedRef().get_length());
		}
	}

	property array<System::Byte>^ Bytes {

		array<System::Byte>^ get() {
			int len = WrappedRef().get_length();
			array<System::Byte>^ bytes = gcnew array<System::Byte>(len);
			for (int i = 0; i < len; i++) {
				bytes[i] = WrappedRef()[i];
			}
			return GCReturnT<array<System::Byte>^>(this, bytes);
		}
	}
internal:
	Address(auto_ptr<::Address>& addr)
		: SnmpSyntax(auto_ptr<::SnmpSyntax>(addr)) {}

	const ::Address& WrappedRef() {
		return dynamic_cast<const ::Address&>(SnmpSyntax::WrappedRef());
	}

	static int GetHashCode(const ::Address& addr) {

		int hash = 0, len = addr.get_length();
		for (int i = 0; i < len; i++) {
			hash = ((hash << 8) | ((unsigned int) hash >> 24)) ^ addr[i];
		}
		return hash;
	}
private:
	Object^ Get(int pos) {
		return this[pos];
	}
};

////////////////////////////////////////////////////////////////////////////////
// IpAddress.
////////////////////////////////////////////////////////////////////////////////

ref class GenAddress;

public ref class IpAddress : public Address {

public:
	IpAddress(IPAddress^ ip)
		: Address(auto_ptr<::Address>(CreateAddress<::IpAddress>(ip->ToString()))) {}

	IpAddress(String^ ip)
		: Address(auto_ptr<::Address>(CreateAddress<::IpAddress>(ip))) {}

	IpAddress(GenAddress^ address)
		: Address(auto_ptr<::Address>(CreateAddress<::IpAddress>(address))) {}

	IpAddress^ MapToIPv6() {

		const ::IpAddress& ip = dynamic_cast<const ::IpAddress&>(WrappedRef());
		if (ip.get_ip_version() == ::Address::version_ipv6) {
			return this;
		}

		auto_ptr<::IpAddress> aptr(UnmanagedClone<::IpAddress>(ip));
		aptr->map_to_ipv6();
		return GCReturnT<IpAddress^>(this,
			ManagedNew<IpAddress, ::IpAddress>(aptr));
	}

	property String^ FriendlyName {

		String^ get() {
			try {
				return Dns::GetHostByAddress(ToString())->HostName;
			}
			catch (SystemException^ exc) {
				throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress, exc);
			}
		}
	}

	property ::Org::Snmp::Snmp_pp::IpVersion IpVersion {
		::Org::Snmp::Snmp_pp::IpVersion get() {
			return (::Org::Snmp::Snmp_pp::IpVersion) GCReturnT<int>(this,
				dynamic_cast<const ::IpAddress&>(WrappedRef()).get_ip_version());
		}
	}

	static operator IPAddress^ (IpAddress^ address) {
		return IPAddress::Parse(address->ToString());
	}
internal:
	IpAddress(auto_ptr<::IpAddress>& ip)
		: Address(auto_ptr<::Address>(ip)) {}

	IpAddress()
		: Address(auto_ptr<::Address>(UnmanagedNew<::IpAddress>())) {}
private:
	static IpAddress() {

		// initialize WinSock: may be needed by resolver
		::Snmp::socket_startup();
	}
};

////////////////////////////////////////////////////////////////////////////////
// UdpAddress.
////////////////////////////////////////////////////////////////////////////////

public ref class UdpAddress sealed : public IpAddress {

public:
	[CLSCompliant(false)]
	UdpAddress(IPAddress^ ip, unsigned short port)
		: IpAddress(auto_ptr<::IpAddress>(CreateUdpAddress(ip->ToString(), port))) {}

	UdpAddress(IPAddress^ ip, int port)
		: IpAddress(auto_ptr<::IpAddress>(CreateUdpAddress(ip->ToString(), port))) {}

	UdpAddress(IPAddress^ ip)
		: IpAddress(auto_ptr<::IpAddress>(CreateUdpAddress(ip->ToString(), 161))) {}

	UdpAddress(IPEndPoint^ endpoint)
		: IpAddress(auto_ptr<::IpAddress>(
			CreateUdpAddress(endpoint->Address->ToString(), endpoint->Port))) {}

	[CLSCompliant(false)]
	UdpAddress(IpAddress^ ip, unsigned short port)
		: IpAddress(auto_ptr<::IpAddress>(CreateUdpAddress(ip, port))) {}

	UdpAddress(IpAddress^ ip, int port)
		: IpAddress(auto_ptr<::IpAddress>(CreateUdpAddress(ip, port))) {}

	UdpAddress(IpAddress^ ip)
		: IpAddress(auto_ptr<::IpAddress>(CreateUdpAddress(ip, 161))) {}

	[CLSCompliant(false)]
	UdpAddress(String^ ip, unsigned short port)
		: IpAddress(auto_ptr<::IpAddress>(CreateUdpAddress(ip, port))) {}

	UdpAddress(String^ ip, int port)
		: IpAddress(auto_ptr<::IpAddress>(CreateUdpAddress(ip, port))) {}

	UdpAddress(String^ ip)
		: IpAddress(auto_ptr<::IpAddress>(CreateUdpAddress(ip, 161))) {}

	UdpAddress(GenAddress^ address)
		: IpAddress(auto_ptr<::IpAddress>(CreateAddress<::UdpAddress>(address))) {}

	UdpAddress()
		: IpAddress(auto_ptr<::IpAddress>(UnmanagedNew<::UdpAddress>())) {}

	UdpAddress^ MapToIPv6() {

		const ::UdpAddress& udp = dynamic_cast<const ::UdpAddress&>(WrappedRef());
		if (udp.get_ip_version() == ::Address::version_ipv6) {
			return this;
		}

		auto_ptr<::UdpAddress> aptr(UnmanagedClone<::UdpAddress>(udp));
		aptr->map_to_ipv6();
		if (!aptr->valid()) {
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress);
		}
		return GCReturnT<UdpAddress^>(this,
			ManagedNew<UdpAddress, ::UdpAddress>(aptr));
	}

	property String^ Ip {

		String^ get() {
			String^ ipWithPort = ToString();
			array<String^>^ portMarkers
				= {"/", IpVersion == ::Org::Snmp::Snmp_pp::IpVersion::IPv4 ? ":" : "]:"};
			for (int i = 0; i < portMarkers->Length; i++) {
				int index = ipWithPort->LastIndexOf(portMarkers[i]);
				if (index >= 0)
					return ipWithPort->Substring(0, index);
			}
			return ipWithPort;
		}
	}

	property int Port {
		int get() {
			return GCReturnT<int>(this,
				dynamic_cast<const ::UdpAddress&>(WrappedRef()).get_port());
		}
	}
internal:
	UdpAddress(auto_ptr<::UdpAddress>& udp)
		: IpAddress(auto_ptr<::IpAddress>(udp)) {}
private:
	static UdpAddress() {

		// initialize WinSock: may be needed by resolver
		::Snmp::socket_startup();
	}

	static ::UdpAddress* CreateUdpAddress(IpAddress^ ip, int port) {

		if (port < 0 || port > 0xffff) {
			throw gcnew ArgumentException("Port value out of range", "port");
		}

		::UdpAddress* addr = UnmanagedNew2T<::UdpAddress, ::IpAddress>(
			dynamic_cast<const ::IpAddress&>(ip->WrappedRef()));
		GC::KeepAlive(ip);

		addr->set_port((unsigned short) port);
		if (!addr->valid()) {
			delete addr;
			String^ msg = String::Concat(ip->ToString(),
							":", ((System::Int32) port).ToString());
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress, msg);
		}
		return addr;
	}

	static ::UdpAddress* CreateUdpAddress(String^ ip, int port) {

		if (port < 0 || port > 0xffff) {
			throw gcnew ArgumentException("Port value out of range", "port");
		}

		auto_array_ptr<char> aptr(NetString2Chars(ip));
		::UdpAddress* addr = UnmanagedNew2T<::UdpAddress, char*>(aptr.get());
		addr->set_port((unsigned short) port);
		if (!addr->valid()) {
			delete addr;
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress,
				String::Concat(ip, ":", ((System::Int32) port).ToString()));
		}
		return addr;
	}
};

////////////////////////////////////////////////////////////////////////////////
// GenAddress.
////////////////////////////////////////////////////////////////////////////////

public ref class GenAddress sealed : public Address {

public:
	GenAddress(String^ address, ::Org::Snmp::Snmp_pp::AddressType type)
		: Address(auto_ptr<::Address>(CreateGenAddress(address, type))) {}

	GenAddress(String^ address)
		: Address(auto_ptr<::Address>(CreateGenAddress(address, ::Org::Snmp::Snmp_pp::AddressType::Invalid))) {}

	GenAddress(::Org::Snmp::Snmp_pp::Address^ address)
		: Address(auto_ptr<::Address>(
			UnmanagedNew2T<::GenAddress, ::Address>(address->WrappedRef()))) {
		GC::KeepAlive(address);
	}

	GenAddress(IPEndPoint^ endpoint)
		: Address(auto_ptr<::Address>(
			CreateGenAddress(endpoint->ToString(), ::Org::Snmp::Snmp_pp::AddressType::UDP))) {}

	static operator IpAddress^ (GenAddress^ address) {

		const ::GenAddress& ga = dynamic_cast<const ::GenAddress&>(address->WrappedRef());
		if (ga.get_type() == ::Address::type_ip) {
			::IpAddress *ipaddr = dynamic_cast<::IpAddress*>(ga.cast_ipaddress().clone());
			if (ipaddr) {
				auto_ptr<::IpAddress> aptr(ipaddr);
				return GCReturnT<IpAddress^>(address,
					ManagedNew<IpAddress, ::IpAddress>(aptr));
			}
		}
		// should we throw UnmanagedOutOfMemoryException() if clone() fails?
		throw gcnew InvalidCastException(
			String::Concat(address->ToString(), ": cannot cast to IpAddress"));
	}

	static operator UdpAddress^ (GenAddress^ address) {

		const ::GenAddress& ga = dynamic_cast<const ::GenAddress&>(address->WrappedRef());
		if (ga.get_type() == ::Address::type_udp) {
			::UdpAddress *udpaddr = dynamic_cast<::UdpAddress*>(ga.cast_udpaddress().clone());
			if (udpaddr) {
				auto_ptr<::UdpAddress> aptr(udpaddr);
				return GCReturnT<UdpAddress^>(address,
					ManagedNew<UdpAddress, ::UdpAddress>(aptr));
			}
		}
		// should we throw UnmanagedOutOfMemoryException() if clone() fails?
		throw gcnew InvalidCastException(
			String::Concat(address->ToString(), ": cannot cast to UdpAddress"));
	}
internal:
	GenAddress(auto_ptr<::GenAddress>& addr)
		: Address(auto_ptr<::Address>(addr)) {}
private:
	static GenAddress() {

		// initialize WinSock: may be needed by resolver
		::Snmp::socket_startup();
	}

	static ::GenAddress* CreateGenAddress(String^ addr, ::Org::Snmp::Snmp_pp::AddressType type) {

		auto_array_ptr<char> aptr(NetString2Chars(addr));
		::GenAddress* paddr = new ::GenAddress(aptr.get(), (::Address::addr_type) type);
		if (!paddr) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		if (!paddr->valid()) {
			delete paddr;
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress, addr);
		}
		return paddr;
	}
};

////////////////////////////////////////////////////////////////////////////////
// MacAddress - 802.3 MAC Address Class.
////////////////////////////////////////////////////////////////////////////////

#ifdef _MAC_ADDRESS

public ref class MacAddress sealed : public Address {

public:
	MacAddress(String^ mac)
		: Address(auto_ptr<::Address>(CreateAddress<::MacAddress>(mac))) {}

	MacAddress(GenAddress^ address)
		: Address(auto_ptr<::Address>(CreateAddress<::MacAddress>(address))) {}
internal:
	MacAddress(auto_ptr<::MacAddress>& mac)
		: Address(auto_ptr<::Address>(mac)) {}
private:
	static MacAddress() {

		// initialize WinSock: may be needed by resolver
		::Snmp::socket_startup();
	}
};

#endif

////////////////////////////////////////////////////////////////////////////////
// IpxAddress.
////////////////////////////////////////////////////////////////////////////////

#ifdef _IPX_ADDRESS

public ref class IpxAddress : public Address {

public:
	IpxAddress(String^ ipx)
		: Address(auto_ptr<::Address>(CreateAddress<::IpxAddress>(ipx))) {}

	IpxAddress(GenAddress^ address)
		: Address(auto_ptr<::Address>(CreateAddress<::IpxAddress>(address))) {}

#ifdef _MAC_ADDRESS
	property MacAddress^ HostId {

		MacAddress^ get() {
			auto_ptr<::MacAddress> aptr(UnmanagedNew<::MacAddress>());
			if (!dynamic_cast<const ::IpxAddress&>(WrappedRef()).get_hostid(*aptr)) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			return GCReturnT<MacAddress^>(this,
				ManagedNew<MacAddress, ::MacAddress>(aptr));
		}
	}
#endif
internal:
	IpxAddress(auto_ptr<::IpxAddress>& ipx)
		: Address(auto_ptr<::Address>(ipx)) {}
private:
	static IpxAddress() {

		// initialize WinSock: may be needed by resolver
		::Snmp::socket_startup();
	}
};

#endif

////////////////////////////////////////////////////////////////////////////////
// IpxSockAddress.
////////////////////////////////////////////////////////////////////////////////

#ifdef _IPX_ADDRESS

public ref class IpxSockAddress sealed : public IpxAddress {

public:
	[CLSCompliant(false)]
	IpxSockAddress(IpxAddress^ address, unsigned short socket)
		: IpxAddress(auto_ptr<::IpxAddress>(CreateIpxSockAddress(address, socket))) {}

	IpxSockAddress(IpxAddress^ address, int socket)
		: IpxAddress(auto_ptr<::IpxAddress>(CreateIpxSockAddress(address, socket))) {}

	[CLSCompliant(false)]
	IpxSockAddress(String^ ipx, unsigned short socket)
		: IpxAddress(auto_ptr<::IpxAddress>(CreateIpxSockAddress(ipx, socket))) {}

	IpxSockAddress(String^ ipx, int socket)
		: IpxAddress(auto_ptr<::IpxAddress>(CreateIpxSockAddress(ipx, socket))) {}

	IpxSockAddress(String^ ipx)
		: IpxAddress(auto_ptr<::IpxAddress>(CreateIpxSockAddress(ipx, 0))) {}

	IpxSockAddress(GenAddress^ address)
		: IpxAddress(auto_ptr<::IpxAddress>(CreateAddress<::IpxSockAddress>(address))) {}

	IpxSockAddress()
		: IpxAddress(auto_ptr<::IpxAddress>(UnmanagedNew<::IpxSockAddress>())) {}

	property int Socket {
		int get() {
			return GCReturnT<int>(this,
				dynamic_cast<const ::IpxSockAddress&>(WrappedRef()).get_socket());
		}
	}
internal:
	IpxSockAddress(auto_ptr<::IpxSockAddress>& ipx)
		: IpxAddress(auto_ptr<::IpxAddress>(ipx)) {}
private:
	static IpxSockAddress() {

		// initialize WinSock: may be needed by resolver
		::Snmp::socket_startup();
	}

	static ::IpxSockAddress* CreateIpxSockAddress(IpxAddress^ ip, int socket) {

		if (socket < 0 || socket > 0xffff) {
			throw gcnew ArgumentException("Socket value out of range", "socket");
		}

		::IpxSockAddress* addr = UnmanagedNew2T<::IpxSockAddress, ::IpxAddress>(
			dynamic_cast<const ::IpxAddress&>(ip->WrappedRef()));
		GC::KeepAlive(ip);

		addr->set_socket((unsigned short) socket);
		if (!addr->valid()) {
			delete addr;
			String^ msg = String::Concat(ip->ToString(),
							":", ((System::Int32) socket).ToString());
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress, msg);
		}
		return addr;
	}

	static ::IpxSockAddress* CreateIpxSockAddress(String^ ipx, int socket) {

		if (socket < 0 || socket > 0xffff) {
			throw gcnew ArgumentException("Socket value out of range", "socket");
		}

		auto_array_ptr<char> aptr(NetString2Chars(ipx));
		::IpxSockAddress* addr = UnmanagedNew2T<::IpxSockAddress, char*>(aptr.get());
		addr->set_socket((unsigned short) socket);
		if (!addr->valid()) {
			delete addr;
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress,
				String::Concat(ipx, ((System::Int32) socket).ToString()));
		}
		return addr;
	}
};

#endif

////////////////////////////////////////////////////////////////////////////////
// And a helper unmanaged-to-managed bridge.
////////////////////////////////////////////////////////////////////////////////

static ::Org::Snmp::Snmp_pp::Address^ GetManagedAddress(const ::GenAddress& addr) {

	switch (addr.get_type()) {
		case ::Address::type_udp:
			return ManagedFromUnmanagedClone<UdpAddress, ::UdpAddress, ::Address>(addr);
		case ::Address::type_ip:
			return ManagedFromUnmanagedClone<IpAddress, ::IpAddress, ::Address>(addr);
		case ::Address::type_mac:
			return ManagedFromUnmanagedClone<MacAddress, ::MacAddress, ::Address>(addr);
		case ::Address::type_ipx:
			return ManagedFromUnmanagedClone<IpxAddress, ::IpxAddress, ::Address>(addr);
		case ::Address::type_ipxsock:
			return ManagedFromUnmanagedClone<IpxSockAddress, ::IpxSockAddress, ::Address>(addr);
		case ::Address::type_invalid:
		default:
		{
			String^ stype = ((System::Int32) addr.get_type()).ToString();
			throw gcnew ArgumentException(
				String::Concat("Invalid address type: ", stype), "addr");
		}
	}
}

////////////////////////////////////////////////////////////////////////////////
// Target classes.
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// SnmpTarget.
////////////////////////////////////////////////////////////////////////////////

public ref class SnmpTarget abstract : public IPrivateDisposable {

public:
	!SnmpTarget() {
		InternalDispose(false);
	}

	virtual int GetHashCode() override {
		return GCReturnT<int>(this,
			::Org::Snmp::Snmp_pp::Address::GetHashCode(m_target->get_address())
			^ ((m_target->get_retry() << 16) | m_target->get_timeout()));
	}

	virtual bool Equals(Object^ o) override {

		if (!o || GetType() != o->GetType()) {
			return false;
		}
		SnmpTarget^ t = dynamic_cast<SnmpTarget^>(o);
		return GCReturn2T<bool>(this, o, *m_target == t->WrappedRef());
	}

	virtual SnmpTarget^ Clone(::Org::Snmp::Snmp_pp::Address^ address) = 0;

	property int Retry {
		int get() {
			return GCReturnT<int>(this, m_target->get_retry());
		}
	}

	property int Timeout {
		int get() {
			return GCReturnT<int>(this, m_target->get_timeout() * 10);
		}
	}

	property GenAddress^ Address {

		GenAddress^ get() {
			auto_ptr<::GenAddress> aptr(UnmanagedNew<::GenAddress>());
			if (!m_target->get_address(*aptr)) {
				throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress);
			}
			return GCReturnT<GenAddress^>(this,
				ManagedNew<GenAddress, ::GenAddress>(aptr));
		}
	}

	property SnmpVersion Version {
		SnmpVersion get() {
			return (SnmpVersion) GCReturnT<int>(this, m_target->get_version());
		}
	}

	property static int DefaultRetries {
		void set(int value) {
			::SnmpTarget::set_default_retries(value);
		}
	}

	property static int DefaultTimeout {
		void set(int timeout) {
			::SnmpTarget::set_default_timeout((timeout + 9)/10);
		}
	}
protected:
	SnmpTarget(::Org::Snmp::Snmp_pp::Address^ address, int timeout, int retries)
		: m_target(Create(address, timeout, retries)) {
		OBJECT_CREATED(m_target);
		GC::KeepAlive(this);
	}

	SnmpTarget(::Org::Snmp::Snmp_pp::Address^ address) : m_target(Create(address, -1, -1)) {
		OBJECT_CREATED(m_target);
		GC::KeepAlive(this);
	}
internal:
	SnmpTarget(auto_ptr<::SnmpTarget>& target) : m_target(target.release()) {
		OBJECT_CREATED(m_target);
		GC::KeepAlive(this);
	}

	virtual void InternalDispose() override {
		InternalDispose(true);
		GC::SuppressFinalize(this);
	}

	const ::SnmpTarget& WrappedRef() {
		return *m_target;
	}
private protected:
	virtual void InternalDispose(bool /* disposing */) {
		DELETE_NATIVE(m_target);
	}
private:
	static ::SnmpTarget* Create(::Org::Snmp::Snmp_pp::Address^ addr, int timeout, int retries) {

		::SnmpTarget* target = UnmanagedNew2T<::SnmpTarget, ::Address>(addr->WrappedRef());
		GC::KeepAlive(addr);

		if (timeout >= 0) {
			target->set_timeout((timeout + 9)/10);
		}
		if (retries >= 0) {
			target->set_retry(retries);
		}
		if (!target->valid()) {
			delete target;
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidTarget, addr->ToString());
		}
		return target;
	}

	::SnmpTarget* m_target;
};

////////////////////////////////////////////////////////////////////////////////
// CTarget.
////////////////////////////////////////////////////////////////////////////////

public ref class CTarget sealed : public SnmpTarget {

public:
	CTarget(::Org::Snmp::Snmp_pp::Address^ address, SnmpVersion snmpVersion,
		String^ readCommunity, String^ writeCommunity, int retries, int timeout)
		: SnmpTarget(auto_ptr<::SnmpTarget>(
			CreateCTarget(address->WrappedRef(), snmpVersion,
				readCommunity, writeCommunity, retries, timeout))) {

		GC::KeepAlive(address);
		m_hash = ComputeHash();
	}

	CTarget(::Org::Snmp::Snmp_pp::Address^ address, SnmpVersion snmpVersion,
		String^ readCommunity, String^ writeCommunity)
		: SnmpTarget(auto_ptr<::SnmpTarget>(
			CreateCTarget(address->WrappedRef(), snmpVersion,
				readCommunity, writeCommunity, -1, -1))) {

		GC::KeepAlive(address);
		m_hash = ComputeHash();
	}

	CTarget(::Org::Snmp::Snmp_pp::Address^ address, SnmpVersion snmpVersion)
		: SnmpTarget(auto_ptr<::SnmpTarget>(
			CreateCTarget(address->WrappedRef(), snmpVersion, nullptr, nullptr, -1, -1))) {

		GC::KeepAlive(address);
		m_hash = ComputeHash();
	}

	CTarget(::Org::Snmp::Snmp_pp::Address^ address)
		: SnmpTarget(auto_ptr<::SnmpTarget>(
			CreateCTarget(address->WrappedRef(), SnmpVersion::SNMPv1, nullptr, nullptr, -1, -1))) {

		GC::KeepAlive(address);
		m_hash = ComputeHash();
	}

	virtual int GetHashCode() override {
		return m_hash;
	}

	virtual bool Equals(Object^ o) override {

		if (!o || GetType() != o->GetType()) {
			return false;
		}
		CTarget^ t = dynamic_cast<CTarget^>(o);;
		return GCReturn2T<bool>(this, o,
				dynamic_cast<const ::CTarget&>(WrappedRef())
			==  dynamic_cast<const ::CTarget&>(t->WrappedRef()));
	}

	SnmpTarget^ Clone(::Org::Snmp::Snmp_pp::Address^ address, SnmpVersion version) {

		auto_ptr<::CTarget> aptr(UnmanagedNew<::CTarget>(
			dynamic_cast<const ::CTarget&>(WrappedRef())));
		aptr->set_version((snmp_version) version);
		if (!aptr->valid()) {
			throw gcnew UnmanagedOutOfMemoryException();
		}

		const ::Address& addr = address->WrappedRef();
		if (!aptr->set_address(addr)) {
			throw gcnew ArgumentException(
				String::Concat("Invalid address: ", address->ToString()), "address");
		}

		return GCReturn2T<SnmpTarget^>(this, address,
			ManagedNew<CTarget, ::CTarget>(aptr));
	}

	virtual SnmpTarget^ Clone(::Org::Snmp::Snmp_pp::Address^ address) override {
		return Clone(address, Version);
	}

	virtual String^ ToString() override {

		const ::CTarget& t = dynamic_cast<const ::CTarget&>(WrappedRef());
		String^ saddr = SafePrintable2T<::GenAddress, ::Address>(t.get_address());
		return GCReturnT<String^>(this,
			String::Concat(saddr,
				",", ((SnmpVersion) t.get_version()).ToString(),
				",", t.get_readcommunity(),
				",", t.get_writecommunity()));
	}

	property String^ ReadCommunity {
		String^ get() {
			return GCReturnT<String^>(this,
				gcnew String(dynamic_cast<const ::CTarget&>(
					WrappedRef()).get_readcommunity()));
		}
	}

	property String^ WriteCommunity {
		String^ get() {
			return GCReturnT<String^>(this,
				gcnew String(dynamic_cast<const ::CTarget&>(
					WrappedRef()).get_writecommunity()));
		}
	}
internal:
	CTarget(auto_ptr<::CTarget>& target)
		: SnmpTarget(auto_ptr<::SnmpTarget>(target.release())) {
		m_hash = ComputeHash();
	}
private:
	static ::CTarget* CreateCTarget(const ::Address& address,
		SnmpVersion snmpVersion, String^ readCommunity, String^ writeCommunity,
		int retries, int timeout) {

		char* rc = NetString2Chars(readCommunity);
		auto_array_ptr<char> rcptr(rc);
		char* wc = NetString2Chars(writeCommunity);
		auto_array_ptr<char> wcptr(wc);

		::CTarget* target = rc && wc
			? new ::CTarget(address, rc, wc) : new ::CTarget(address);
		if (!target) {
			throw gcnew UnmanagedOutOfMemoryException();
		}

		target->set_version((snmp_version) snmpVersion);
		if (retries >= 0) {
			target->set_retry(retries);
		}
		if (timeout >= 0) {
			target->set_timeout((timeout + 9)/10);
		}
		if (!target->valid()) {
			delete target;
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidTarget);
		}
		return target;
	}

	int ComputeHash() {

		const ::CTarget& t = dynamic_cast<const ::CTarget&>(WrappedRef());
		return GCReturnT<int>(this,
			SnmpTarget::GetHashCode()
			^ (gcnew String(t.get_readcommunity ()))->GetHashCode()
			^ (gcnew String(t.get_writecommunity()))->GetHashCode());
	}

	int m_hash;
};

////////////////////////////////////////////////////////////////////////////////
// UTarget.
////////////////////////////////////////////////////////////////////////////////

public ref class UTarget sealed : public SnmpTarget {

public:
	UTarget(::Org::Snmp::Snmp_pp::Address^ address, String^ secName, SecurityModel secModel, String^ engineId,
		int retries, int timeout)
		: SnmpTarget(auto_ptr<::SnmpTarget>(
			CreateUTarget(address->WrappedRef(),
				secName, secModel, engineId, retries, timeout))) {

		GC::KeepAlive(address);
		m_hash = ComputeHash();
	}

	UTarget(::Org::Snmp::Snmp_pp::Address^ address, String^ secName, SecurityModel secModel, String^ engineId)
		: SnmpTarget(auto_ptr<::SnmpTarget>(
			CreateUTarget(address->WrappedRef(),
				secName, secModel, engineId, -1, -1))) {

		GC::KeepAlive(address);
		m_hash = ComputeHash();
	}

	UTarget(::Org::Snmp::Snmp_pp::Address^ address, String^ secName, SecurityModel secModel)
		: SnmpTarget(auto_ptr<::SnmpTarget>(
			CreateUTarget(address->WrappedRef(), secName, secModel, nullptr, -1, -1))) {

		GC::KeepAlive(address);
		m_hash = ComputeHash();
	}

	virtual int GetHashCode() override {
		return m_hash;
	}

	virtual bool Equals(Object^ o) override {

		if (!o || GetType() != o->GetType()) {
			return false;
		}
		// ::UTarget::operator== skips engineId during comparison and this is OK
		UTarget^ t = dynamic_cast<UTarget^>(o);
		return GCReturn2T<bool>(this, o,
				dynamic_cast<const ::UTarget&>(WrappedRef())
			==  dynamic_cast<const ::UTarget&>(t->WrappedRef()));
	}

	virtual SnmpTarget^ Clone(::Org::Snmp::Snmp_pp::Address^ address) override {

		auto_ptr<::UTarget> aptr(UnmanagedNew<::UTarget>(
			dynamic_cast<const ::UTarget&>(WrappedRef())));
		if (!aptr->valid()) {
			throw gcnew UnmanagedOutOfMemoryException();
		}

		const ::Address& addr = address->WrappedRef();
		if (!aptr->set_address(addr)) {
			throw gcnew ArgumentException(
				String::Concat("Invalid address: ", address->ToString()), "address");
		}

		return GCReturn2T<SnmpTarget^>(this, address,
			ManagedNew<UTarget, ::UTarget>(aptr));
	}

	virtual String^ ToString() override {

		const ::UTarget& t = dynamic_cast<const ::UTarget&>(WrappedRef());
		String^ sga = SafePrintable2T<::GenAddress, ::Address>(t.get_address());
		String^ sos = SafePrintableT<::OctetStr>(t.get_security_name());
		return GCReturnT<String^>(this,
			String::Concat(sga,
				",", ((SnmpVersion) t.get_version()).ToString(),
				",", EngineId->ToString(),
				",", ((::Org::Snmp::Snmp_pp::SecurityModel) t.get_security_model()).ToString(),
				",", sos));
	}

	property OctetStr^ SecurityName {

		OctetStr^ get() {
			auto_ptr<::OctetStr> aptr(UnmanagedNew<::OctetStr>());
			dynamic_cast<const ::UTarget&>(WrappedRef()).get_security_name(*aptr);
			if (!aptr->valid()) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			return GCReturnT<OctetStr^>(this,
				ManagedNew<OctetStr, ::OctetStr>(aptr));
		}
	}

	property ::Org::Snmp::Snmp_pp::SecurityModel SecurityModel {
		::Org::Snmp::Snmp_pp::SecurityModel get() {
			return (::Org::Snmp::Snmp_pp::SecurityModel)
				GCReturnT<int>(this,
					dynamic_cast<const ::UTarget&>(WrappedRef()).get_security_model());
		}
	}

	property OctetStr^ EngineId {

		OctetStr^ get() {
			// Note: ::Snmp invocation functions modify the engine id.
			// To avoid problems, we pass a copy, and the value is not
			// updated. Thus, we have to check the cache first.
			auto_ptr<::OctetStr> aptr(UnmanagedNew<::OctetStr>());
			::OctetStr& ref = *aptr;
			dynamic_cast<const ::UTarget&>(WrappedRef()).get_engine_id(ref);
			if (!ref.valid()) {
				throw gcnew UnmanagedOutOfMemoryException();
			}

			if (ref.len() == 0)
			{
				::GenAddress ga(WrappedRef().get_address());
			    v3MP::I->get_from_engine_id_table(ref, (char*) ga.get_printable());
			}

			return GCReturnT<OctetStr^>(this,
				ManagedNew<OctetStr, ::OctetStr>(aptr));
		}
	}
internal:
	UTarget(auto_ptr<::UTarget>& target)
		: SnmpTarget(auto_ptr<::SnmpTarget>(target.release())) {
		m_hash = ComputeHash();
	}
private:
	static ::UTarget* CreateUTarget(const ::Address& address, String^ secName,
		::Org::Snmp::Snmp_pp::SecurityModel secModel, String^ engineId, int retries, int timeout) {

		char* sname = NetString2Chars(secName);
		auto_array_ptr<char> anmptr(sname);

		::UTarget* target = new ::UTarget(address, sname, (int) secModel);
		if (!target) {
			throw gcnew UnmanagedOutOfMemoryException();
		}

		if (engineId) {
			char* engid = NetString2Chars(engineId);
			auto_array_ptr<char> aeiptr(engid);
			target->set_engine_id(engid);
		}
		if (retries >= 0) {
			target->set_retry(retries);
		}
		if (timeout >= 0) {
			target->set_timeout((timeout + 9)/10);
		}
		if (!target->valid()) {
			delete target;
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidTarget);
		}
		return target;
	}

	int ComputeHash() {

		// Engine_id is not taken into account as it may be discovered
		// and changed behind the scenes.
		const ::UTarget& t = dynamic_cast<const ::UTarget&>(WrappedRef());
		String^ sos = SafePrintableT<::OctetStr>(t.get_security_name());
		return GCReturnT<int>(this,
			SnmpTarget::GetHashCode()
			^ ((t.get_version() << 16) | t.get_security_model())
			^ sos->GetHashCode());
	}

	int m_hash;
};

////////////////////////////////////////////////////////////////////////////////
// And a helper unmanaged-to-managed bridge.
////////////////////////////////////////////////////////////////////////////////

static SnmpTarget^ GetManagedTarget(const ::SnmpTarget& target) {

	switch (target.get_type()) {
		case ::SnmpTarget::type_ctarget:
			return ManagedFromUnmanagedClone<CTarget, ::CTarget, ::SnmpTarget>(target);
		case ::SnmpTarget::type_utarget:
			return ManagedFromUnmanagedClone<UTarget, ::UTarget, ::SnmpTarget>(target);
		case ::SnmpTarget::type_base:
		default:
		{
			String^ stype = ((System::Int32) target.get_type()).ToString();
			throw gcnew ArgumentException(
				String::Concat("Invalid target type: ", stype), "target");
		}
	}
}

////////////////////////////////////////////////////////////////////////////////
// PDUs and VBs.
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// Vb.
////////////////////////////////////////////////////////////////////////////////

public ref class Vb sealed : public IPrivateDisposable {

public:
	Vb(Oid^ oid, SnmpSyntax^ value)
		: m_vb(CreateVb(dynamic_cast<const ::Oid&>(oid->WrappedRef()), value)) {

		GC::KeepAlive(oid);
		OBJECT_CREATED(m_vb);
		GC::KeepAlive(this);
	}

	Vb(Oid^ oid)
		: m_vb(CreateVb(dynamic_cast<const ::Oid&>(oid->WrappedRef()), nullptr)) {

		GC::KeepAlive(oid);
		OBJECT_CREATED(m_vb);
		GC::KeepAlive(this);
	}

	Vb(String^ oid, SnmpSyntax^ value)
		: m_vb(CreateVb(oid, value)) {

		OBJECT_CREATED(m_vb);
		GC::KeepAlive(this);
	}

	Vb(String^ oid)
		: m_vb(CreateVb(oid, nullptr)) {

		OBJECT_CREATED(m_vb);
		GC::KeepAlive(this);
	}

	!Vb() {
		InternalDispose(false);
	}

	virtual String^ ToString() override {
		return GCReturnT<String^>(this, ToString(*m_vb));
	}

	property ::Org::Snmp::Snmp_pp::Oid^ Oid {
		::Org::Snmp::Snmp_pp::Oid^ get() {
			auto_ptr<::Oid> aptr(UnmanagedNew<::Oid>(m_vb->get_oid()));
			return GCReturnT<::Org::Snmp::Snmp_pp::Oid^>(this,
				ManagedNew<::Org::Snmp::Snmp_pp::Oid, ::Oid>(aptr));
		}
	}

	// Note that both NULL and SnmpExceptionStatus may be returned
	property SnmpSyntax^ Value {
		SnmpSyntax^ get() {
			return GCReturnT<SnmpSyntax^>(this, CreateValue(*m_vb));
		}
	}

	static Vb^ Create(String^ oid, String^ syntax, String^ value) {
		return Create(oid, SnmpSyntax::GetSyntax(syntax), value);
	}

	static Vb^ Create(String^ oid, SmiSyntax syntax, String^ value) {
		bool b = false;
		return gcnew Vb(gcnew ::Org::Snmp::Snmp_pp::Oid(oid), Create(syntax, value, b));
	}
internal:
	Vb(auto_ptr<::Vb>& vb) : m_vb(vb.release()) {
		OBJECT_CREATED(m_vb);
		GC::KeepAlive(this);
	}

	virtual void InternalDispose() override {
		InternalDispose(true);
		GC::SuppressFinalize(this);
	}

	const ::Vb& WrappedRef() {
		return *m_vb;
	}

	static String^ ToString(const ::Vb& vb) {

		String^ soid = SafePrintableT<::Oid>(vb.get_oid());
		SnmpSyntax^ val = CreateValue(vb);
		return val ? String::Concat(soid, ",", val->ToString()) : soid;
	}
private:
	virtual void InternalDispose(bool /* disposing */) sealed {
		DELETE_NATIVE(m_vb);
	}

	static SnmpSyntax^ Create(SmiSyntax syntax, String^ v, bool& filled) {

		filled = false;
		switch (syntax) {
			case SmiSyntax::Null:
				return nullptr;
			case SmiSyntax::Int32:
				return v ? gcnew SnmpInt32(System::Int32::Parse(v)) : gcnew SnmpInt32();
			case SmiSyntax::TimeTicks:
				return v ? gcnew TimeTicks(System::UInt32::Parse(v)) : gcnew TimeTicks();
			case SmiSyntax::Counter32:
				return v ? gcnew Counter32(System::UInt32::Parse(v)) : gcnew Counter32();
			case SmiSyntax::Gauge32:	// case SmiSyntax::Uint32:
				return v ? gcnew Gauge32(System::UInt32::Parse(v)) : gcnew Gauge32();
			case SmiSyntax::Counter64:
				return v ? gcnew Counter64(UInt64::Parse(v)) : gcnew Counter64();
			case SmiSyntax::Bits:
			case SmiSyntax::OctetString:
				return v ? gcnew OctetStr(v) : gcnew OctetStr();
			case SmiSyntax::Opaque:
				return v ? gcnew OpaqueStr(v) : gcnew OpaqueStr();
			case SmiSyntax::IpAddress:
				return v ? gcnew IpAddress(v) : gcnew IpAddress();
			case SmiSyntax::Oid:
				return v ? gcnew ::Org::Snmp::Snmp_pp::Oid(v) : gcnew ::Org::Snmp::Snmp_pp::Oid();
			case SmiSyntax::NoSuchInstance:
			case SmiSyntax::NoSuchObject:
			case SmiSyntax::EndOfMibView:
				filled = true;
				return gcnew SnmpExceptionStatus((ExceptionStatus) syntax);
			default:
				throw gcnew SnmpClassException(Snmp_ppStatus::Invalid,
					String::Concat("invalid syntax for a Vb value: ",
						((System::Int32) syntax).ToString()));
		}
	}

	static SnmpSyntax^ CreateValue(const ::Vb& vb) {

		bool filled = false;
		SnmpSyntax^ pval = Create((SmiSyntax) vb.get_syntax(), nullptr, filled);
		if (pval && !filled) {
			int status = vb.get_value(const_cast<::SnmpSyntax&>(pval->WrappedRef()));
			if (status != SNMP_CLASS_SUCCESS) {
				throw gcnew SnmpClassException(
					(Snmp_ppStatus) status, "cannot extract value from Vb");
			}
			// The unmanaged value has been changed and the managed wrapper is
			// not aware of this; if the wrapper caches the hash value, it must
			// be immediately updated.
			pval->UpdateHash();
		}
		return pval;
	}

	static ::Vb* CreateVb(const ::Oid& oid, SnmpSyntax^ value) {

		::Vb* pvb = UnmanagedNew2T<::Vb, ::Oid>(oid);
		if (value) {
			pvb->set_value(value->WrappedRef());
			GC::KeepAlive(value);
		}
		if (!pvb->valid()) {
			delete pvb;
			throw gcnew SnmpClassException(Snmp_ppStatus::Invalid, "invalid data for Vb");
		}
		return pvb;
	}

	static ::Vb* CreateVb(String^ oidstr, SnmpSyntax^ value) {

		auto_array_ptr<char> aptr(NetString2Chars(oidstr));
		::Oid oid(aptr.get());
		if (!oid.valid()) {
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidOid, oidstr);
		}

		return CreateVb(oid, value);
	}

	::Vb* m_vb;
};

////////////////////////////////////////////////////////////////////////////////
// Pdu.
//
// Note that unlike most classes, this class' objects are NOT immutable and,
// in general, SHOULD NOT be shared among various threads (unless access is
// read-only in all threads). If you need to do that, consider Clone().
////////////////////////////////////////////////////////////////////////////////

public ref class Pdu sealed
	: public IDisposable, public IEnumerable, public IPrivateDisposable {

public:
	Pdu(PduType type, Vb^ vb)
		: m_maxReps(DEF_MAX_REPS), m_pdu(Init(type, vb)) {
		OBJECT_CREATED(m_pdu);
		GC::KeepAlive(this);
	}

	Pdu(PduType type, array<Vb^>^ vbArray)
		: m_maxReps(DEF_MAX_REPS), m_pdu(Init(type, vbArray)) {
		OBJECT_CREATED(m_pdu);
		GC::KeepAlive(this);
	}

	Pdu(PduType type, Oid^ oid)
		: m_maxReps(DEF_MAX_REPS), m_pdu(Init(type, oid)) {
		OBJECT_CREATED(m_pdu);
		GC::KeepAlive(this);
	}

	Pdu(PduType type, array<Oid^>^ oidArray)
		: m_maxReps(DEF_MAX_REPS), m_pdu(Init(type, oidArray)) {
		OBJECT_CREATED(m_pdu);
		GC::KeepAlive(this);
	}

	~Pdu() {
		InternalDispose(true);
	}

	!Pdu() {
		InternalDispose(false);
	}

	virtual void InternalDispose() override {
		InternalDispose(true);
		GC::SuppressFinalize(this);
	}

	virtual String^ ToString() override {

		return GCReturnT<String^>(this,
			String::Concat(((PduType) m_pdu->get_type()).ToString(),
				",messageId:", ((System::UInt32) m_pdu->get_message_id()).ToString(),
				",reqid:", ((System::UInt32) m_pdu->get_request_id()).ToString(),
				",estat:", ((System::Int32) m_pdu->get_error_status()).ToString(),
				",eidx:", ((System::Int32) m_pdu->get_error_index()).ToString(),
				",", VbsToString(*m_pdu)));
	}

	Pdu^ Clone(PduType pduType, array<Vb^>^ vbArray) {

		auto_ptr<::Pdu> aptr(UnmanagedNew<::Pdu>(WrappedRef()));
		GC::KeepAlive(this);

		::Pdu& pdu = *aptr;
		pdu.set_type((int) pduType);
		pdu.set_vblist((::Vb*) -1, 0);
		Init(pdu, vbArray);

		return ManagedNew<Pdu, ::Pdu>(aptr);
	}

	Pdu^ Clone(PduType pduType, Vb^ vb) {

		array<Vb^>^ vbs = gcnew array<Vb^>(1);
		vbs[0] = vb;
		return Clone(pduType, vbs);
	}

	Pdu^ Clone(array<Vb^>^ vbArray) {
		return Clone(Type, vbArray);
	}

	Pdu^ Clone(Vb^ vb) {
		return Clone(Type, vb);
	}

	Pdu^ Clone() {

		auto_ptr<::Pdu> aptr(UnmanagedNew<::Pdu>(WrappedRef()));
		if (!aptr->valid()) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		return GCReturnT<Pdu^>(this, ManagedNew<Pdu, ::Pdu>(aptr));
	}

	virtual IEnumerator^ GetEnumerator() {
		return gcnew Enumerator(
			gcnew Delegate_Count(this, &::Org::Snmp::Snmp_pp::Pdu::Count::get),
			gcnew Delegate_Get(this, &::Org::Snmp::Snmp_pp::Pdu::Get));
	}

	property Vb^ default[int] {

		Vb^ get(int position) {
			if (position < 0 || position >= m_pdu->get_vb_count()) {
				throw gcnew IndexOutOfRangeException();
			}

			auto_ptr<::Vb> aptr(UnmanagedNew<::Vb>());
			bool ok = m_pdu->get_vb(*aptr, position);
			if (!ok) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			return GCReturnT<Vb^>(this, ManagedNew<Vb, ::Vb>(aptr));
		}
	}

	property int Count {
		int get() {
			return GCReturnT<int>(this, m_pdu->get_vb_count());
		}
	}

	property array<Vb^>^ Vbs {

		array<Vb^>^ get() {
			// 'this' kept alive by calls below
			int len = m_pdu->get_vb_count();
			array<Vb^>^ vbs = gcnew array<Vb^>(len);
			for (int i = 0; i < len; i++) {
				vbs[i] = this[i];
			}
			return vbs;
		}
	}

	property PduType Type {
		PduType get() {
			return (PduType) GCReturnT<int>(this, m_pdu->get_type());
		}
	}

	property SnmpError ErrorStatus {

		SnmpError get() {
			return (SnmpError) GCReturnT<int>(this, m_pdu->get_error_status());
		}

		void set(SnmpError value) {
			m_pdu->set_error_status((int) value);
			GC::KeepAlive(this);
		}
	}

	void ClearErrorStatus() {
		m_pdu->clear_error_status();
		GC::KeepAlive(this);
	}

	property int ErrorIndex {

		int get() {
			return GCReturnT<int>(this, m_pdu->get_error_index());
		}

		void set(int index) {
			m_pdu->set_error_index(index);
			GC::KeepAlive(this);
		}
	}

	void ClearErrorIndex() {
		m_pdu->clear_error_index();
		GC::KeepAlive(this);
	}

	[CLSCompliant(false)]
	property unsigned int RequestId {

		unsigned int get() {
			return GCReturnT<unsigned int>(this, m_pdu->get_request_id());
		}

		void set(unsigned int requestId) {
			m_pdu->set_request_id(requestId);
			GC::KeepAlive(this);
		}
	}

	property long long RequestIdAsInt64 {
		long long get() {
			return GCReturnT<long long>(this, m_pdu->get_request_id());
		}

		void set(long long requestId) {
			m_pdu->set_request_id(ArgToULong(requestId, "requestId"));
			GC::KeepAlive(this);
		}
	}

	property TimeTicks^ NotifyTimestamp {

		TimeTicks^ get() {
			auto_ptr<::TimeTicks> aptr(UnmanagedNew<::TimeTicks>());
			m_pdu->get_notify_timestamp(*aptr);
			return GCReturnT<TimeTicks^>(this,
				ManagedNew<TimeTicks, ::TimeTicks>(aptr));
		}

		void set(TimeTicks^ ticks) {
			m_pdu->set_notify_timestamp(
				dynamic_cast<const ::TimeTicks&>(ticks->WrappedRef()));
			GC::KeepAlive(this);
		}
	}

	property Oid^ NotifyId {

		Oid^ get() {
			auto_ptr<::Oid> aptr(UnmanagedNew<::Oid>());
			if (!m_pdu->get_notify_id(*aptr)) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			return GCReturnT<Oid^>(this, ManagedNew<Oid, ::Oid>(aptr));
		}

		void set(Oid^ oid) {
			if (!m_pdu->set_notify_id(dynamic_cast<const ::Oid&>(oid->WrappedRef()))) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			GC::KeepAlive(this);
		}
	}

	property Oid^ NotifyEnterprise {

		Oid^ get() {
			auto_ptr<::Oid> aptr(UnmanagedNew<::Oid>());
			if (!m_pdu->get_notify_enterprise(*aptr)) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			return GCReturnT<Oid^>(this, ManagedNew<Oid, ::Oid>(aptr));
		}

		void set(Oid^ enterprise) {

			if (!m_pdu->set_notify_enterprise(
				dynamic_cast<const ::Oid&>(enterprise->WrappedRef()))) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			GC::KeepAlive(this);
			GC::KeepAlive(enterprise);
		}
	}

	property SecurityLevel SecurityLevel {

		::Org::Snmp::Snmp_pp::SecurityLevel get() {
			return (::Org::Snmp::Snmp_pp::SecurityLevel)
				GCReturnT<int>(this, m_pdu->get_security_level());
		}

		void set(::Org::Snmp::Snmp_pp::SecurityLevel secLevel) {
			m_pdu->set_security_level((int) secLevel);
			GC::KeepAlive(this);
		}
	}

	property OctetStr^ ContextName {

		OctetStr^ get() {
			auto_ptr<::OctetStr> aptr(UnmanagedNew<::OctetStr>());
			if (!m_pdu->get_context_name(*aptr)) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			return GCReturnT<OctetStr^>(this,
				ManagedNew<OctetStr, ::OctetStr>(aptr));
		}

		void set(OctetStr^ os) {

			if (!m_pdu->set_context_name(
				dynamic_cast<const ::OctetStr&>(os->WrappedRef()))) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			GC::KeepAlive(this);
			GC::KeepAlive(os);
		}
	}

	property OctetStr^ ContextEngineId {

		OctetStr^ get() {
			auto_ptr<::OctetStr> aptr(UnmanagedNew<::OctetStr>());
			if (!m_pdu->get_context_engine_id(*aptr)) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			return GCReturnT<OctetStr^>(this,
				ManagedNew<OctetStr, ::OctetStr>(aptr));
		}

		void set(OctetStr^ os) {
			if (!m_pdu->set_context_engine_id(
				dynamic_cast<const ::OctetStr&>(os->WrappedRef()))) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			GC::KeepAlive(this);
			GC::KeepAlive(os);
		}
	}

	[CLSCompliant(false)]
	property unsigned int MessageId {

		unsigned int get() {
			return GCReturnT<unsigned int>(this, m_pdu->get_message_id());
		}

		void set(unsigned int messageId) {
			m_pdu->set_message_id(messageId);
			GC::KeepAlive(this);
		}
	}

	property long long MessageIdAsInt64 {

		long long get() {
			return GCReturnT<long long>(this, m_pdu->get_message_id());
		}

		void set(long long messageId) {
			m_pdu->set_message_id(ArgToULong(messageId, "messageId"));
			GC::KeepAlive(this);
		}
	}

	property int MaxSizeScopedPdu {

		int get() {
			return GCReturnT<int>(this, m_pdu->get_maxsize_scopedpdu());
		}

		void set(int size) {

			if (size < 0) {
				throw gcnew ArgumentException(
					"Invalid maximum scoped PDU size", "size");
			}
			m_pdu->set_maxsize_scopedpdu(size);
			GC::KeepAlive(this);
		}
	}

	property int V1GenericTrap {

		// RFC-2576, section 3.2
		int get() {
			Oid^ oid = NotifyId;
			Oid^ pfx = gcnew Oid("1.3.6.1.6.3.1.1.5");
			int pfxlen = pfx->Length, value;
			if (oid->Length == pfxlen + 1
				&& oid->Compare(pfx, pfxlen, true) == 0
				&& (value = oid[pfxlen] - 1) < 6)
				return value;
			else
				return 6;
		}
	}

	property int V1SpecificTrap {

		// RFC-2576, section 3.2
		int get() {
			if (V1GenericTrap < 6)
				return 0;
			else {
				Oid^ oid = NotifyId;
				int len = oid->Length;
				return len > 0 ? oid[len - 1] : 0;
			}
		}
	}

	property GenAddress^ V1TrapAddress {

		GenAddress^ get() {
			auto_ptr<::GenAddress> aptr(UnmanagedNew<::GenAddress>());
			if (m_pdu->get_v1_trap_address(*aptr) == FALSE) {
				throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress);
			}
			return GCReturnT<GenAddress^>(this,
				ManagedNew<GenAddress, ::GenAddress>(aptr));
		}

		void set(GenAddress^ address) {

			if (m_pdu->set_v1_trap_address(address->WrappedRef()) == FALSE) {
				throw gcnew SnmpClassException(Snmp_ppStatus::InvalidAddress);
			}
			GC::KeepAlive(this);
			GC::KeepAlive(address);
		}
	}

	property int NonRepeaters {
		int get() {
			return m_nonReps;
		}

		void set(int nonReps) {

			if (nonReps < 0) {
				throw gcnew ArgumentException(
					"Invalid value for non-repeaters", "nonReps");
			}
			m_nonReps = nonReps;
		}
	}

	property int MaxRepetitions {
		int get() {
			return m_maxReps;
		}

		void set(int maxReps) {

			if (maxReps < 0) {
				throw gcnew ArgumentException(
					"Invalid value for max-repetitions", "maxReps");
			}
			m_maxReps = maxReps;
		}
	}
internal:
	Pdu(auto_ptr<::Pdu>& apdu)
		: m_pdu(apdu.release()), m_maxReps(DEF_MAX_REPS) {
		OBJECT_CREATED(m_pdu);
		GC::KeepAlive(this);
	}

	const ::Pdu& WrappedRef() {
		return *m_pdu;
	}
private:
	virtual void InternalDispose(bool /* disposing */) sealed {
		DELETE_NATIVE(m_pdu);
	}

	// We provide here two separate implementations for Vb's and Oid's.
	// It should work faster than in a case of creating Vb's from Oid's
	// to operate on Vb's . We could apply templates but it would look
	// complex, so we simply repeat code.
	static void Init(::Pdu& pdu, array<Vb^>^ vbs) {

		for (int i = 0, len = vbs->Length; i < len; i++) {
			Vb^ vb = vbs[i];
			pdu += vb->WrappedRef();
			GC::KeepAlive(vb);
		}
	}

	static void Init(::Pdu& pdu, array<Oid^>^ oids) {

		for (int i = 0, len = oids->Length; i < len; i++) {
			Oid^ oid = oids[i];
			pdu += ::Vb(dynamic_cast<const ::Oid&>(oid->WrappedRef()));
			GC::KeepAlive(oid);
		}
	}

	static void CheckPdu(const ::Pdu& pdu, int vbCount) {

		if (pdu.get_vb_count() != vbCount) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		if (!pdu.valid()) {
			throw gcnew SnmpClassException(Snmp_ppStatus::InvalidPdu, "invalid Pdu");
		}
	}

	static ::Pdu* Init(PduType pduType, array<Vb^>^ vbs) {

		auto_ptr<::Pdu> aptr(UnmanagedNew<::Pdu>());
		::Pdu& pdu = *aptr;
		pdu.set_type((int) pduType);
		Init(pdu, vbs);
		CheckPdu(pdu, vbs->Length);

		return aptr.release();
	}

	static ::Pdu* Init(PduType pduType, Vb^ vb) {

		array<Vb^>^ vbs = gcnew array<Vb^>(1);
		vbs[0] = vb;
		return Init(pduType, vbs);
	}

	static ::Pdu* Init(PduType pduType, array<Oid^>^ oids) {

		auto_ptr<::Pdu> aptr(UnmanagedNew<::Pdu>());
		::Pdu& pdu = *aptr;
		pdu.set_type((int) pduType);
		Init(pdu, oids);
		CheckPdu(pdu, oids->Length);

		return aptr.release();
	}

	static ::Pdu* Init(PduType pduType, Oid^ oid) {

		array<Oid^>^ oids = gcnew array<Oid^>(1);
		oids[0] = oid;
		return Init(pduType, oids);
	}

	static String^ VbsToString(const ::Pdu& pdu) {

		String^ s = "";
		for (int i = 0; i < pdu.get_vb_count(); i++) {
			::Vb vb;
			if (pdu.get_vb(vb, i)) {
				s = String::Concat(s, ",", Vb::ToString(vb));
			}
			else {
				throw gcnew UnmanagedOutOfMemoryException();
			}
		}
		return s;
	}

	Object^ Get(int pos) {
		return this[pos];
	}

	static const unsigned int DEF_MAX_REPS	= 10;
	unsigned int	m_nonReps,
					m_maxReps;
	::Pdu*			m_pdu;
};

////////////////////////////////////////////////////////////////////////////////
// SNMP class and related helper classes.
////////////////////////////////////////////////////////////////////////////////

ref class Snmp;

public delegate void NotifyCallback(
	Snmp^ snmp, Pdu^ pdu, SnmpTarget^ target, Object^ data);

////////////////////////////////////////////////////////////////////////////////
// CallbackData - callback data.
////////////////////////////////////////////////////////////////////////////////

private ref class CallbackData sealed : public IAsyncResult {

public:
	CallbackData(Snmp^ snmp, AsyncCallback^ callback, Object^ data)
		: m_snmp(snmp), m_callback(callback), m_data(data) {}

	// IAsyncResult
	property Object^ AsyncState {
		virtual Object^ get() {
			return m_data;
		}
	}

	property WaitHandle^ AsyncWaitHandle {

		virtual WaitHandle^ get() {
			Monitor::Enter(this);
			try {
				if (!m_handle) {
					m_handle = gcnew ManualResetEvent(m_completed);
				}
				return m_handle;
			}
			__finally {
				Monitor::Exit(this);
			}
		}
	}

	property bool CompletedSynchronously {
		virtual bool get() {
			return false;
		}
	}

	property bool IsCompleted {

		virtual bool get() {
			bool completed;

			Monitor::Enter(this);
			completed = m_completed;
			Monitor::Exit(this);

			return completed;
		}
	}

	// Own interface
	void WaitForCompletion() {

		Monitor::Enter(this);
		try {
			if (!m_completed) {
				Monitor::Wait(this);
			}
		}
		__finally {
			Monitor::Exit(this);
		}
	}

	void Done(Pdu^ pdu) {
		Done(static_cast<Object^>(pdu));
	}

	void Done(Exception^ e) {
		Done(static_cast<Object^>(e));
	}

	property Snmp^ Snmp {
		::Org::Snmp::Snmp_pp::Snmp^ get() {
			return m_snmp;
		}
	}

	// The property returns the result of an asynchronous operation
	// (either returns a Pdu or throws a stored exception); it also
	// invalidates this callback data object
	property Pdu^ Pdu {
		::Org::Snmp::Snmp_pp::Pdu^ get();
	}

	property unsigned int ReqId {
		unsigned int get() {
			unsigned int reqId;

			Monitor::Enter(this);
			reqId = m_reqId;
			Monitor::Exit(this);

			return reqId;
		}

		void set(unsigned int reqId) {

			Monitor::Enter(this);
			m_reqId = reqId;
			Monitor::Exit(this);
		}
	}
private:
	void Done(Object^ result) {

		Monitor::Enter(this);
		try {
			if (m_completed) {
				throw gcnew InvalidOperationException("Done() called twice");
			}

			m_completed = true;
			if (m_handle) {
				m_handle->Set();
			}
			m_result = result;
			Monitor::Pulse(this);
		}
		__finally {
			Monitor::Exit(this);
		}

		if (m_callback) {
			ThreadPool::QueueUserWorkItem(gcnew WaitCallback(this,
				&::Org::Snmp::Snmp_pp::CallbackData::Callback), this);
		}
	}

	void Callback(Object^ o);

	::Org::Snmp::Snmp_pp::Snmp^	m_snmp;
	AsyncCallback^		m_callback;
	Object^				m_data;
	Object^				m_result;
	unsigned int		m_reqId;
	ManualResetEvent^	m_handle;
	bool				m_completed;
};

////////////////////////////////////////////////////////////////////////////////
// CallbackRegistry - keeps registered callbacks in a hashtable.
////////////////////////////////////////////////////////////////////////////////

private ref class CallbackRegistry sealed {

public:
	void InstallCallback(CallbackData^ cbData) {
		m_hash->Add(cbData->GetHashCode(), cbData);
	}

	CallbackData^ RemoveCallback(int hashCode) {

		CallbackData^ cbData
			= dynamic_cast<CallbackData^>(m_hash[hashCode]);
		if (cbData) {
			m_hash->Remove(hashCode);
		}
		return cbData;
	}

	CallbackData^ GetCallback(int hashCode) {
		return dynamic_cast<CallbackData^>(m_hash[hashCode]);
	}

	static property CallbackRegistry^ Instance {
		CallbackRegistry^ get() {
			return m_instance;
		}
	}
private:
	CallbackRegistry() : m_hash(Hashtable::Synchronized(gcnew Hashtable())) {}

	Hashtable^ m_hash;
	static CallbackRegistry^ m_instance = gcnew CallbackRegistry();
};

static void UnmanagedAsyncCallback(int reason, ::Snmp* session,
	::Pdu& pdu, ::SnmpTarget& target, void* data);

static void UnmanagedNotifyCallback(int reason, ::Snmp* session,
	::Pdu& pdu, ::SnmpTarget& target, void* data);

////////////////////////////////////////////////////////////////////////////////
// This is a thin wrapper for the standard SNMP++ SNMP class.
// 
// Note: the first successfully created SNMP object will seal the v3MP and no
// further changes will be possible.  Also, the last argument to constructors
// specifies whether a dispatcher thread should be automatically started (see
// StartAsyncDispatcher()).
////////////////////////////////////////////////////////////////////////////////

public ref class Snmp sealed : public IPrivateDisposable {

public:
	Snmp(UdpAddress^ addressV4, UdpAddress^ addressV6, bool startAsyncDispatcher)
		: m_snmp(CreateSnmp(addressV4, addressV6)) {

		Init(startAsyncDispatcher);
		OBJECT_CREATED_NO_ADD(m_snmp);
		GC::KeepAlive(this);
	}

	Snmp(UdpAddress^ address, bool startAsyncDispatcher)
		: m_snmp(CreateSnmp(address, nullptr)) {

		Init(startAsyncDispatcher);
		OBJECT_CREATED_NO_ADD(m_snmp);
		GC::KeepAlive(this);
	}

	Snmp(int port, bool startAsyncDispatcher)
		: m_snmp(CreateSnmp(port)) {

		Init(startAsyncDispatcher);
		OBJECT_CREATED_NO_ADD(m_snmp);
		GC::KeepAlive(this);
	}

	Snmp(bool startAsyncDispatcher)
		: m_snmp(CreateSnmp(0)) {

		Init(startAsyncDispatcher);
		OBJECT_CREATED_NO_ADD(m_snmp);
		GC::KeepAlive(this);
	}

	~Snmp() {
		InternalDispose(true);
	}

	!Snmp() {
		InternalDispose(false);
	}

	virtual void InternalDispose() override {
		InternalDispose(true);
		GC::SuppressFinalize(this);
	}

	////////////////////////////////////////////////////////////////////////////
	// Synchronous interface.
	////////////////////////////////////////////////////////////////////////////

	// We do not use pointers to member functions due to VC++.NET 2002 problem:
	// typedef int (::Snmp::*SnmpAsyncOpPointer)
	//		(::Pdu&, ::SnmpTarget&, const snmp_callback, const void*);
	// SnmpAsyncOpPointer op = &::Snmp::get;
	// error C2440: 'initializing' : cannot convert from 'overloaded-function'
	// to 'Snmp_pp::Snmp::SnmpAsyncOpPointer'

	Pdu^ Invoke(Pdu^ pdu, SnmpTarget^ target) {

		auto_ptr<::Pdu> aptr(UnmanagedNew<::Pdu>(pdu->WrappedRef()));
		::Pdu& refPdu = *aptr;
		if (!refPdu.valid()) {
			throw gcnew UnmanagedOutOfMemoryException();
		}

		auto_ptr<::SnmpTarget> atgt;
		::SnmpTarget& tg = CopyTargetIfNeeded(target->WrappedRef(), atgt);
		bool returnPdu = false;
		int status;
		switch (pdu->Type) {
			case PduType::Get:
				status = m_snmp->get(refPdu, tg);
				returnPdu = true;
				break;
			case PduType::GetNext:
				status = m_snmp->get_next(refPdu, tg);
				returnPdu = true;
				break;
			case PduType::GetBulk:
			{
				int nonRepeaters   = pdu->NonRepeaters   & 0x7fffffff,
					maxRepetitions = pdu->MaxRepetitions & 0x7fffffff;
				status = m_snmp->get_bulk(refPdu, tg,
							nonRepeaters, maxRepetitions);
				returnPdu = true;
				break;
			}
			case PduType::Set:
				status = m_snmp->set(refPdu, tg);
				break;
			case PduType::Trap:
				status = m_snmp->trap(refPdu, tg);
				break;
			case PduType::Report:
				status = m_snmp->report(refPdu, tg);
				break;
			case PduType::Inform:
				status = m_snmp->inform(refPdu, tg);
				break;
			case PduType::Response:
				status = m_snmp->response(refPdu, tg);
				break;
			default:
				status = SNMP_CLASS_ERROR;
				break;
		};

		if (status != SNMP_CLASS_SUCCESS) {
			ThrowException(status, aptr->get_error_index());
		}

		GC::KeepAlive(this);
		GC::KeepAlive(pdu);
		GC::KeepAlive(target);

		return returnPdu ? ManagedNew<Pdu, ::Pdu>(aptr) : nullptr;
	}

	IList^ BroadcastDiscovery(int timeoutSec, UdpAddress^ address,
		SnmpVersion version, String^ community) {

		char* comm = NetString2Chars(community);
		auto_array_ptr<char> aptr(comm);

		::OctetStr ostr(comm ? comm : ""), *postr = comm ? &ostr : 0;
		::UdpAddressCollection addrs;
		int status = m_snmp->broadcast_discovery(addrs, timeoutSec,
			dynamic_cast<const ::UdpAddress&>(address->WrappedRef()),
			(snmp_version) version, postr);
		if (status != SNMP_CLASS_SUCCESS) {
			ThrowException(status);
		}

		GC::KeepAlive(this);
		GC::KeepAlive(address);

		return SnmpCollection2List<UdpAddress, ::UdpAddress>(addrs);
	}

	////////////////////////////////////////////////////////////////////////////
	// Asynchronous interface - follows the standard .NET asynchronous pattern.
	////////////////////////////////////////////////////////////////////////////

	IAsyncResult^ BeginInvoke(Pdu^ pdu, SnmpTarget^ target,
		AsyncCallback^ callback, Object^ data) {

		auto_ptr<::Pdu> aptr(UnmanagedNew<::Pdu>(pdu->WrappedRef()));
		::Pdu& refPdu = *aptr;
		if (!refPdu.valid()) {
			throw gcnew UnmanagedOutOfMemoryException();
		}

		// The callback must be installed BEFORE Snmp async function returns
		// - it's theoretically possible that the callback will be processed
		// immediately by another thread.
		CallbackData^ cbData = gcnew CallbackData(this, callback, data);
		CallbackRegistry::Instance->InstallCallback(cbData);
		Interlocked::Increment(m_pendingInvokes);
		int hashCode = cbData->GetHashCode();

		auto_ptr<::SnmpTarget> atgt;
		::SnmpTarget& tg = CopyTargetIfNeeded(target->WrappedRef(), atgt);
		void* pHashCode = reinterpret_cast<void*>(hashCode);
		int status;
		switch (pdu->Type) {
			case PduType::Get:
				status = m_snmp->get(refPdu, tg,
							UnmanagedAsyncCallback, pHashCode);
				break;
			case PduType::GetNext:
				status = m_snmp->get_next(refPdu, tg,
							UnmanagedAsyncCallback, pHashCode);
				break;
			case PduType::GetBulk:
			{
				int nonRepeaters   = pdu->NonRepeaters   & 0x7fffffff,
					maxRepetitions = pdu->MaxRepetitions & 0x7fffffff;
				status = m_snmp->get_bulk(refPdu, tg,
							nonRepeaters, maxRepetitions,
							UnmanagedAsyncCallback, pHashCode);
				break;
			}
			case PduType::Set:
				status = m_snmp->set(refPdu, tg,
							UnmanagedAsyncCallback, pHashCode);
				break;
			case PduType::Inform:
				status = m_snmp->inform(refPdu, tg,
							UnmanagedAsyncCallback, pHashCode);
				break;
			default:
				status = SNMP_CLASS_ERROR;
				break;
		};

		if (status != SNMP_CLASS_SUCCESS) {
			CallbackRegistry::Instance->RemoveCallback(hashCode);
			Interlocked::Decrement(m_pendingInvokes);
			ThrowException(status);
		}

		cbData->ReqId = aptr->get_request_id();

		GC::KeepAlive(this);
		GC::KeepAlive(pdu);
		GC::KeepAlive(target);

		return cbData;
	}

	Pdu^ EndInvoke(IAsyncResult^ ar) {

		CallbackData^ cbData = dynamic_cast<CallbackData^>(ar);
		if (cbData->Snmp != this) {
			throw gcnew ArgumentException(
				"Invalid IAsyncResult passed as argument", "ar");
		}

		cbData->WaitForCompletion();
		return cbData->Pdu;
	}

	bool CancelInvoke(IAsyncResult^ ar) {

		CallbackData^ cbData = dynamic_cast<CallbackData^>(ar);
		bool res = m_snmp->cancel(cbData->ReqId);
		if (res) {
			CallbackRegistry::Instance->RemoveCallback(ar->GetHashCode());
		}
		return GCReturnT<bool>(this, res);
	}

	// This method starts a thread that will act as an asynchronous dispatcher
	// for this Snmp object. The thread is required for the asynchronous stuff
	// to work, which includes BeginInvoke() and notifications. The thread may
	// be stopped either by calling StopAsyncDispatcher() or Dispose().
	//
	// Note: SNMP++ does not have its own listening/dispatching thread;  if we
	// want to receive asynchronous responses or notifications, we must provide
	// our own one. The dispatching thread will only be used for listening and
	// dispatching responses to thread pool threads; the responses will be
	// processed on these threads.
	bool StartAsyncDispatcher() {

		Monitor::Enter(m_notifyLock);
		try {
			if (!m_dispatcher)
			{
				Dispatcher^ disp = gcnew Dispatcher(this);
				ThreadStart^ ts = gcnew ThreadStart(disp, &Dispatcher::Dispatch);
				m_dispatcher = gcnew Thread(ts);
				m_dispatcher->Start();
				return true;
			}
			else
				return false;
		}
		__finally {
			Monitor::Exit(m_notifyLock);
		}
	}

	bool StopAsyncDispatcher() {

		Monitor::Enter(m_notifyLock);
		try {
			if (m_dispatcher)
			{
				m_dispatcher->Interrupt();
				m_dispatcher->Join();
				m_dispatcher = nullptr;
				return true;
			}
			else
				return false;
		}
		__finally {
			Monitor::Exit(m_notifyLock);
		}
	}

	property int PendingInvokes {
		int get() {
			return m_pendingInvokes;
		}
	}

	////////////////////////////////////////////////////////////////////////////
	// Notifications (traps).
	////////////////////////////////////////////////////////////////////////////

	property int NotifyListenPort {
		int get() {
			return m_snmp->notify_get_listen_port();
		}

		void set(int port) {
			m_snmp->notify_set_listen_port(port);
		}
	}

	void NotifyRegister(IList^ trapIds, IList^ targets,
		NotifyCallback^ callback, Object^ data) {

		OidCollection trapCol;
		TargetCollection targetCol;

		List2SnmpCollection<Oid, ::Oid>(trapIds, trapCol);
		List2SnmpCollection<SnmpTarget, ::SnmpTarget>(targets, targetCol);

		CallbackData^ cbData = gcnew CallbackData(this, nullptr, nullptr);
		CallbackRegistry::Instance->InstallCallback(cbData);
		int hashCode = cbData->GetHashCode(), status;

		Monitor::Enter(m_notifyLock);
		try {
			status = m_snmp->notify_register(trapCol, targetCol,
				Snmp_pp::UnmanagedNotifyCallback, reinterpret_cast<void*>(hashCode));
			if (status == SNMP_CLASS_SUCCESS) {
				m_notifyCallback = callback;
				m_notifyData = data;
			}
		}
		__finally {
			Monitor::Exit(m_notifyLock);
			GC::KeepAlive(this);
		}

		if (status != SNMP_CLASS_SUCCESS) {
			CallbackRegistry::Instance->RemoveCallback(hashCode);
			ThrowException(status);
		}
	}

	void NotifyUnregister() {

		int hashCode;

		Monitor::Enter(m_notifyLock);
		try {
			m_notifyCallback = nullptr;
			m_notifyData = nullptr;

			hashCode = reinterpret_cast<int>(m_snmp->get_notify_callback_data());
			m_snmp->notify_unregister();
		}
		__finally {
			Monitor::Exit(m_notifyLock);
			GC::KeepAlive(this);
		}

		CallbackRegistry::Instance->RemoveCallback(hashCode);
	}

	void GetNotifyData([Out] IList^& trapIds, [Out] IList^& targets, [Out] IList^& listenAddresses,
		[Out] NotifyCallback^& callback, [Out] Object^& data) {

		OidCollection trCol;
		TargetCollection tgCol;
		AddressCollection adCol;
		NotifyCallback^ c;
		Object^ d;

		int status;

		Monitor::Enter(m_notifyLock);
		try {
			status = m_snmp->get_notify_filter(trCol, tgCol, adCol);
			c = m_notifyCallback;
			d = m_notifyData;
		}
		__finally {
			Monitor::Exit(m_notifyLock);
			GC::KeepAlive(this);
		}

		if (status != SNMP_CLASS_SUCCESS) {
			ThrowException(status);
		}

		IList^ tp = SnmpCollection2List<Oid, ::Oid>(trCol);
		IList^ tg = SnmpCollection2List<SnmpTarget, ::SnmpTarget>(tgCol, GetManagedTarget);
		IList^ la = SnmpCollection2List<Address, ::GenAddress>(adCol, GetManagedAddress);

		trapIds = tp;
		targets = tg;
		listenAddresses = la;
		callback = c;
		data = d;
	}

	void GetNotifyData([Out] NotifyCallback^& callback, [Out] Object^& data) {

		Monitor::Enter(m_notifyLock);
		try {
			callback = m_notifyCallback;
			data = m_notifyData;
		}
		__finally {
			Monitor::Exit(m_notifyLock);
		}
	}

	void ProcessEvent(int blockTime) {
		m_snmp->eventListHolder->SNMPProcessEvents(blockTime);
		// GC::KeepAlive(this);			// not necessary here
	}

	////////////////////////////////////////////////////////////////////////////
	// Other stuff.
	////////////////////////////////////////////////////////////////////////////

	property IpAddress^ ListenAddress {

		IpAddress^ get() {
			auto_ptr<::IpAddress> aptr(UnmanagedNew<::IpAddress>(m_snmp->get_listen_address()));
			return GCReturnT<IpAddress^>(this, ManagedNew<IpAddress, ::IpAddress>(aptr));
		}
	}

	static String^ ErrorMsg(int c) {
		return gcnew String(::Snmp::error_msg(c));
	}

	static String^ ErrorMsg(Oid^ oid) {
		return GCReturnT<String^>(oid,
			gcnew String(::Snmp::error_msg(dynamic_cast<const ::Oid&>(oid->WrappedRef()))));
	}

	static void Srand(int seed) {
		::srand(seed);
	}

	static void Srand() {
		Srand((int) time(0));
	}

	static property String^ DebugLogFile {

		void set(String^ fileName) {
			static char LogFileName[256];
			static MCriticalSection cs;

			MLockHandler lock(cs);
			if (fileName) {
				NetString2Buf(fileName, LogFileName, sizeof(LogFileName));
				// ::debug_set_logfile(LogFileName);						// SNMP++ v. 3.2.16
				::Snmp_pp::DefaultLog::init(new AgentLogImpl(LogFileName));	// SNMP++ v. 3.2.17
			}
			else {
				// ::debug_set_logfile(0);									// SNMP++ v. 3.2.16
				::Snmp_pp::DefaultLog::init(0);								// SNMP++ v. 3.2.17
			}
		}
	}

	property static int DebugLogLevel {
		void set(int level) {
			::debug_set_level(level);
		}
	}

	property static String^ Snmp_ppVersion {
		String^ get() {
			return SNMP_PP_VERSION_STRING;
		}
	}

	property static String^ BuildTime {
		String^ get() {
			return String::Concat(__DATE__, " ", __TIME__);
		}
	}
internal:
	void Notification(Pdu^ pdu, SnmpTarget^ target) {

		NotifyCallback^ callback;
		Object^ data;

		GetNotifyData(callback, data);

		if (callback) {
			NotifyData^ notify = gcnew NotifyData(this, pdu, target, callback, data);
			notify->Queue();
		}
	}

	void CallbackFinished() {
		Interlocked::Decrement(m_pendingInvokes);
	}

	const ::Snmp& WrappedRef() {
		return *m_snmp;
	}

	static void ThrowException(int status, int index) {

		if (Enum::IsDefined(Snmp_ppStatus::typeid, status)) {
			throw gcnew SnmpClassException((Snmp_ppStatus) status);
		}
		if (Enum::IsDefined(SnmpError::typeid, status)) {
			throw gcnew SnmpException((SnmpError) status, index);
		}
		throw gcnew ApplicationException(
			String::Concat(((System::Int32) status).ToString(), ": undefined status"));
	}

	static void ThrowException(int status) {
		ThrowException(status, -1);
	}
private:
	virtual void InternalDispose(bool disposing) sealed {

		if (disposing) {
			StopAsyncDispatcher();
		}

		DELETE_NATIVE(m_snmp);
	}

	ref class NotifyData sealed {

	public:
		NotifyData(Snmp^ snmp, Pdu^ pdu, SnmpTarget^ target,
			NotifyCallback^ callback, Object^ data) {

			m_snmp     = snmp;
			m_pdu      = pdu;
			m_target   = target;
			m_callback = callback;
			m_data     = data;
		}

		void Queue() {
			ThreadPool::QueueUserWorkItem(gcnew WaitCallback(this, &NotifyData::Callback));
		}
	private:
		void Callback(Object^) {
			m_callback(m_snmp, m_pdu, m_target, m_data);
		}

		Snmp^			m_snmp;
		Pdu^			m_pdu;
		SnmpTarget^		m_target;
		NotifyCallback^	m_callback;
		Object^			m_data;
	};

	ref class Dispatcher sealed
	{
	public:
		Dispatcher(Snmp^ snmp) : m_snmp(snmp) {}

		void Dispatch()
		{
			try
			{
				while (true)
				{
					while (m_snmp->PendingInvokes > 0)
					{
						m_snmp->ProcessEvent(1000);
					}

					// check interruption status, then wait for an event
					System::Threading::Thread::Sleep(0);
					m_snmp->ProcessEvent(100);
				}
			}
			catch (ThreadInterruptedException^) {}
		}
	private:
		Snmp^ m_snmp;
	};

	static Snmp() {

		// initialize WinSock
		::Snmp::socket_startup();
		// initialize random generator
		::srand((unsigned int) time(0));
	}

	void Init(bool startAsyncDispatcher) {

		m_notifyLock = gcnew Object();
		if (startAsyncDispatcher) {
			StartAsyncDispatcher();
		}
	}

	static ::SnmpTarget& CopyTargetIfNeeded(
		const ::SnmpTarget& target, auto_ptr<::SnmpTarget>& atgt) {

		// Note that engine_id in a v3 target may be modified when
		// the engine_id is empty - in this case, a copy is needed
		// (see also UTarget::EngineId)
		switch (target.get_type()) {
			case ::SnmpTarget::type_ctarget:
				return const_cast<::SnmpTarget&>(target);
			case ::SnmpTarget::type_utarget:
			{
				const ::UTarget& utarget = dynamic_cast<const ::UTarget&>(target);
				if (utarget.get_engine_id().len() != 0)
					return const_cast<::SnmpTarget&>(target);
			}
			// fall through - a copy needed
			default:
			{
				::SnmpTarget* tclone = target.clone();
				atgt.reset(tclone);
				if (!tclone || !tclone->valid()) {
					throw gcnew UnmanagedOutOfMemoryException();
				}
				return *tclone;
			}
		}
	}

	static void CheckIfCreated(::Snmp* snmp, int status) {

		if (!snmp) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		if (status != SNMP_CLASS_SUCCESS) {
			delete snmp;
			throw gcnew SnmpClassException((Snmp_ppStatus) status);
		}
	}

	static ::Snmp* CreateSnmp(UdpAddress^ addrV4, UdpAddress^ addrV6);

	static ::Snmp* CreateSnmp(int port);

	::Snmp*					m_snmp;
	int						m_pendingInvokes;
	NotifyCallback^			m_notifyCallback;
	Object^					m_notifyData;
	Object^					m_notifyLock;
	Thread^					m_dispatcher;
};

Pdu^ CallbackData::Pdu::get() {

	Object^ result;

	Monitor::Enter(this);
	try {
		result = m_result;
		m_result = nullptr;
	}
	__finally {
		Monitor::Exit(this);
	}

	if (!m_callback) {
		m_snmp->CallbackFinished();
	}

	Snmp_pp::Pdu^ pdu = dynamic_cast<Snmp_pp::Pdu^>(result);
	if (pdu) {
		return pdu;
	}
	else
	if (result) {
		throw dynamic_cast<Exception^>(result);
	}
	else {
		return nullptr;
	}
}

void CallbackData::Callback(Object^ o) {

	try {
		m_callback(dynamic_cast<IAsyncResult^>(o));
	}
	__finally {
		m_snmp->CallbackFinished();
	}
}

////////////////////////////////////////////////////////////////////////////////
// USM.
////////////////////////////////////////////////////////////////////////////////

public ref class USM sealed {

public:
	void AddUsmUser(OctetStr^ securityName,
		AuthProtocol authProtocol, PrivProtocol privProtocol,
		OctetStr^ authPassword, OctetStr^ privPassword);
	void AddUsmUser(String^ securityName,
		AuthProtocol authProtocol, PrivProtocol privProtocol,
		String^ authPassword, String^ privPassword);
internal:
	USM() {}
};

////////////////////////////////////////////////////////////////////////////////
// V3MP - a thin but a bit safer wrapper for v3MP. In this implementation, we
// have made the following assumptions:
// - V3MP may be freely re-initialized with Init() until the first call, which
//   retrieves the reference (get_Instance()) and before the first SNMP object
//   is created.
// - explicit "dummy" creation for SNMPv3 (which is default) is not necessary.
////////////////////////////////////////////////////////////////////////////////

public ref class V3MP sealed {

public:
	[CLSCompliant(false)]
	static void Init(OctetStr^ engineId, unsigned int engineBoots) {

		Monitor::Enter(m_lock);
		try {
			if (m_pv3mp) {
				throw gcnew V3MpInitializationError(
					String::Concat("V3MP has already been initialized",
						" - explicitly by V3MP.Init()",
						" or implicitly by the Snmp class"));
			}

			const ::OctetStr& refid
				= dynamic_cast<const ::OctetStr&>(engineId->WrappedRef());
			m_pv3mp = Init(refid, engineBoots);
			GC::KeepAlive(engineId);
		}
		__finally {
			Monitor::Exit(m_lock);
		}
	}

	static void Init(OctetStr^ engineId, long long engineBoots) {
		Init(engineId, ArgToULong(engineBoots, "engineBoots"));
	}

	property static V3MP^ Instance {
		V3MP^ get() {
			Init();
			return m_v3mp;
		}
	}

	property OctetStr^ LocalEngineId {

		OctetStr^ get() {
			auto_ptr<::OctetStr> aptr(UnmanagedNew<::OctetStr>());
			const_cast<::v3MP*>(m_pv3mp)->get_local_engine_id(*aptr);
			if (!aptr->valid()) {
				throw gcnew UnmanagedOutOfMemoryException();
			}
			return ManagedNew<OctetStr, ::OctetStr>(aptr);
		}
	}

	property USM^ Usm {
		USM^ get() {
			return m_usm;
		}
	}
internal:
	static ::v3MP& WrappedRef() {
		return const_cast<::v3MP&>(*Init());
	}
private:
	V3MP() {}

	static ::v3MP* Init(const ::OctetStr& os, unsigned int engineBoots) {

		int status;
		::v3MP* pmp = new ::v3MP(os, engineBoots, status);
		if (!pmp) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		if (status != SNMPv3_MP_OK) {
			delete pmp;
			throw gcnew V3MpInitializationError(status);
		}
		return pmp;
	}

	static ::v3MP* Init() {

		Monitor::Enter(m_lock);
		try {
			if (!m_pv3mp) {
				m_pv3mp = Init(::OctetStr("SNMP++.NET"), 0);
			}
			return m_pv3mp;
		}
		__finally {
			Monitor::Exit(m_lock);
		}
	}

	static V3MP^ m_v3mp = gcnew V3MP();
	static USM^  m_usm  = gcnew USM();

	static v3MP*    m_pv3mp = 0;
	static Object^	m_lock = gcnew Object();
};

::Snmp* Snmp::CreateSnmp(UdpAddress^ addrV4, UdpAddress^ addrV6) {

	int status;
	::Snmp *snmp = (addrV6 != nullptr
		? new ::Snmp(status, dynamic_cast<const ::UdpAddress&>(addrV4->WrappedRef()),
							 dynamic_cast<const ::UdpAddress&>(addrV6->WrappedRef()))
		: new ::Snmp(status, dynamic_cast<const ::UdpAddress&>(addrV4->WrappedRef())));
	GC::KeepAlive(addrV4);
	GC::KeepAlive(addrV6);
	CheckIfCreated(snmp, status);

	// seal the V3MP
	V3MP::Instance;
	return snmp;
}

::Snmp* Snmp::CreateSnmp(int port) {

	int status;
	::Snmp *snmp = new ::Snmp(status, port);
	CheckIfCreated(snmp, status);

	// seal the V3MP
	V3MP::Instance;
	return snmp;
}

void USM::AddUsmUser(::Org::Snmp::Snmp_pp::OctetStr^ securityName,
	AuthProtocol authProtocol, PrivProtocol privProtocol,
	::Org::Snmp::Snmp_pp::OctetStr^ authPassword, ::Org::Snmp::Snmp_pp::OctetStr^ privPassword) {

	const ::OctetStr& secName  = dynamic_cast<const ::OctetStr&>(securityName->WrappedRef());
	const ::OctetStr& authPass = dynamic_cast<const ::OctetStr&>(authPassword->WrappedRef());
	const ::OctetStr& privPass = dynamic_cast<const ::OctetStr&>(privPassword->WrappedRef());

	if (V3MP::WrappedRef().get_usm()->add_usm_user(secName,
		(int) authProtocol, (int) privProtocol, authPass, privPass) != SNMPv3_USM_OK) {
		throw gcnew UnmanagedOutOfMemoryException();
	}

	GC::KeepAlive(securityName);
	GC::KeepAlive(authPassword);
	GC::KeepAlive(privPassword);
}

void USM::AddUsmUser(String^ securityName,
	AuthProtocol authProtocol, PrivProtocol privProtocol,
	String^ authPassword, String^ privPassword) {

	auto_array_ptr<char>	sn(NetString2Chars(securityName)),
							ap(NetString2Chars(authPassword)),
							pp(NetString2Chars(privPassword));
	::OctetStr  snos(sn.get()),
				apos(ap.get()),
				ppos(pp.get());
	if (!snos.valid() || !apos.valid() || !ppos.valid()) {
		throw gcnew UnmanagedOutOfMemoryException();
	}
	if (V3MP::WrappedRef().get_usm()->add_usm_user(snos,
		(int) authProtocol, (int) privProtocol, apos, ppos) != SNMPv3_USM_OK) {
		throw gcnew UnmanagedOutOfMemoryException();
	}
}

////////////////////////////////////////////////////////////////////////////////
// And finally, the callbacks.
////////////////////////////////////////////////////////////////////////////////

void UnmanagedAsyncCallback(int reason, ::Snmp* session,
	::Pdu& pdu, ::SnmpTarget& target, void* data) {

	int hashCode = reinterpret_cast<int>(data);
	CallbackData^ cbData = CallbackRegistry::Instance->RemoveCallback(hashCode);
	if (!cbData) {
		return;
	}

	try {
		// cbData keeps alive its Snmp instance
		if (session != &cbData->Snmp->WrappedRef()) {
			throw gcnew ArgumentException(
				"Invalid session object", "session");
		}

		if (reason != SNMP_CLASS_ASYNC_RESPONSE) {
			Snmp::ThrowException(reason);
		}

		if (pdu.get_error_status() == (int) SnmpError::Success) {
			auto_ptr<::Pdu> apdu(UnmanagedNew<::Pdu>(pdu));
			Pdu^ ppdu = ManagedNew<Pdu, ::Pdu>(apdu);
			cbData->Done(ppdu);
		}
		else {
			SnmpException^ e = gcnew SnmpException(
				(SnmpError) pdu.get_error_status(), pdu.get_error_index());
			cbData->Done(e);
		}
	}
	catch (Exception^ e) {
		cbData->Done(e);
	}
}

void UnmanagedNotifyCallback(int reason, ::Snmp* session,
	::Pdu& pdu, ::SnmpTarget& target, void* data) {

	int hashCode = reinterpret_cast<int>(data);
	CallbackData^ cbData = CallbackRegistry::Instance->GetCallback(hashCode);
	if (!cbData) {
		return;
	}

	Snmp^ snmp = cbData->Snmp;
	try {
		if (session != &snmp->WrappedRef()) {
			throw gcnew ArgumentException(
				"Invalid session object", "session");
		}

		if (reason != SNMP_CLASS_NOTIFICATION) {
			Snmp::ThrowException(reason);
		}

		auto_ptr<::Pdu> apdu(UnmanagedNew<::Pdu>(pdu));
		Pdu^ ppdu = ManagedNew<Pdu, ::Pdu>(apdu);
		SnmpTarget^ ptgt = GetManagedTarget(target);
		snmp->Notification(ppdu, ptgt);
	}
	catch (Object^ exc) {
#ifdef	_DEBUG
		System::Diagnostics::Debug::Assert(
			false, "Unexpected exception", exc->ToString());
		throw;
#else
		Console::Error->WriteLine(String::Concat("*** ", exc));
#endif
	}
}

}	// end of namespace Snmp_pp

}	// end of namespace Snmp

}	// end of namespace Org
