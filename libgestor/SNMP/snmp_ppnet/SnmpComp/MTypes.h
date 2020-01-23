////////////////////////////////////////////////////////////////////////////////
//
// SNMP++.NET v. 1.21 (2006-07-26 15:30:00)
//
// Copyright (c) 2003-2004 Military Communication Institute, Zegrze, Poland
// Author: Marek Malowidzki
//
// ATTENTION: USE OF THIS SOFTWARE IS SUBJECT TO THE FOLLOWING TERMS.
// Permission to use, copy, modify, distribute and/or sell this software 
// and/or its documentation is hereby granted without fee. User agrees 
// to display the above copyright notice and this license notice in all 
// copies of the software and any documentation of the software. User 
// agrees to assume all liability for the use of the software; 
// Military Communication Institute make
// no representations about the suitability of this software for any purpose.
// It is provided  "AS-IS" without warranty of any kind, either express
// or implied. User hereby grants a royalty-free license to any and all
// derivatives based upon this software code base. 
// 
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
// Type conversion templates and memory management.
//
// Author:	Marek Malowidzki	2003,2004
////////////////////////////////////////////////////////////////////////////////

#pragma once

#include <memory>
#using <mscorlib.dll>

using namespace std;
using namespace System;

namespace Org {

namespace Snmp {

namespace Snmp_pp {

////////////////////////////////////////////////////////////////////////////////
// UnmanagedOutOfMemoryException - thrown when the unmanaged heap is exhausted.
////////////////////////////////////////////////////////////////////////////////

[Serializable]
public ref class UnmanagedOutOfMemoryException sealed : public ApplicationException {

public:
	UnmanagedOutOfMemoryException(String^ message, Exception^ exception)
		: ApplicationException(message, exception) {}

	UnmanagedOutOfMemoryException(String^ message)
		: ApplicationException(message) {}

	UnmanagedOutOfMemoryException() {}
};

////////////////////////////////////////////////////////////////////////////////
// Object allocation templates.
////////////////////////////////////////////////////////////////////////////////

// I would prefer to have a single name 'UnmanagedNew' for all templates but it
// causes various versions of VC++ to complain.
template <typename T, typename TT>
inline T* UnmanagedNew2T(const TT& t) {

		T *newT = new T(t);
		if (!newT) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		return newT;
}

template <typename T>
inline T* UnmanagedNew(const T& t) {

		T *newT = new T(t);
		if (!newT) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		return newT;
}

template <typename T>
inline T* UnmanagedNew() {

		T *newT = new T();
		if (!newT) {
			throw gcnew UnmanagedOutOfMemoryException();
		}
		return newT;
}

template <typename MT, typename UT>
inline MT^ ManagedNew(auto_ptr<UT>& aptr) {

		MT^ dotNetT = gcnew MT(aptr);
		// assert that the ownership of the pointer
		// has been transferred to the managed object
		assert(!aptr.get());
		return dotNetT;
}

template <typename T>
inline T* UnmanagedClone(const T& t) {

	T *newT = dynamic_cast<T*>(t.clone());
	if (!newT) {
		throw gcnew UnmanagedOutOfMemoryException();
	}
	return newT;
}

template <typename MT, typename UT, typename UBaseT>
inline MT^ ManagedFromUnmanagedClone(const UBaseT& t) {

	auto_ptr<UT> aptr(static_cast<UT*>(UnmanagedClone<UBaseT>(t)));
	return ManagedNew<MT, UT>(aptr);
}

}	// end of namespace Org

}	// end of namespace Snmp

}	// end of namespace Snmp_pp
