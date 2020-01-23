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

/*
*	Extract from thread utilities for Win32: critical section.
*
*	Author:	Marek Malowidzki	2000, 2003
*/

#ifndef	__MLOCK_H
#define	__MLOCK_H

#include <windows.h>
#using <mscorlib.dll>

namespace Mao {

namespace Threading {

///////////////////////////////////////////////////////////////////////////////
// Synchronization primitives.
///////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////
// MLock - base class for locks. Lock should be unlocked by the same thread,
// which has locked it. Locks should be accessed by MLockHandler objects only.
///////////////////////////////////////////////////////////////////////////////

class MLockHandler;

class MLock {

friend class MLockHandler;

public:
	virtual ~MLock() = 0;
protected:
	virtual void Lock() = 0;
	virtual void Unlock() = 0;
};

MLock::~MLock() {}

///////////////////////////////////////////////////////////////////////////////
// MLockHandler - locks the lock in constructor and unlocks it in destructor.
///////////////////////////////////////////////////////////////////////////////

class MLockHandler {

public:
	explicit MLockHandler(MLock &lock);
	~MLockHandler();
private:
	// copy constructor, operators =, new, delete
	// - illegal, not implemented
	MLockHandler(const MLockHandler&);
	MLockHandler& operator=(const MLockHandler&);
	void* operator new(size_t size);
	void operator delete(void *ptr);

	MLock &lock_;
};

inline MLockHandler::MLockHandler(MLock &lock) : lock_(lock) {
	lock_.Lock();
}

inline MLockHandler::~MLockHandler() {
	lock_.Unlock();
}

///////////////////////////////////////////////////////////////////////////////
// MCriticalSection - simplest and fastest thread synchronization for Win32.
// Nesting locking is supported.
///////////////////////////////////////////////////////////////////////////////

class MCriticalSection : public MLock {

public:
	MCriticalSection();
	virtual ~MCriticalSection();
private:
	// copy constructor and operator= - empty
	MCriticalSection(const MCriticalSection&);
	MCriticalSection& operator=(const MCriticalSection&);

	virtual void Lock();
	virtual void Unlock();

	CRITICAL_SECTION cs_;
};

inline MCriticalSection::MCriticalSection() {
	::InitializeCriticalSection(&cs_);
}

inline MCriticalSection::~MCriticalSection() {
	::DeleteCriticalSection(&cs_);
}

inline void MCriticalSection::Lock() {
	::EnterCriticalSection(&cs_);
}

inline void MCriticalSection::Unlock() {
	::LeaveCriticalSection(&cs_);
}

}	// end of namespace Thread

}	// end of namespace Mao

#endif
