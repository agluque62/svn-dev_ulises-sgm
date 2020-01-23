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

#include "MConversion.h"

using namespace System::Runtime::InteropServices;
using namespace Org::Snmp::Snmp_pp;

string Mao::DotNet::NetString2StdString(String^ s) {

	if (!s) {
		throw gcnew ArgumentNullException("s");
	}

	const char* chars = (const char*)
		Marshal::StringToHGlobalAnsi(s).ToPointer();
	int len = ::strlen(chars);
	string stds = chars;
	Marshal::FreeHGlobal(IntPtr((void*) chars));

	// 'len' does not need to be equal to s->Length
	// as 's' may contain NULL (0) characters
	if (stds.length() != len) {
		throw gcnew UnmanagedOutOfMemoryException();
	}
	return stds;
}

char* Mao::DotNet::NetString2Chars(String^ s) {

	if (!s) {
		throw gcnew ArgumentNullException("s");
	}

	// use auto_ptr<> as StringToHGlobalAnsi() may throw an exception
	int length = s->Length;
	auto_ptr<char> scopy(new char[length + 1]);
	if (!scopy.get()) {
		throw gcnew UnmanagedOutOfMemoryException();
	}

	const char* chars = (const char*)
		Marshal::StringToHGlobalAnsi(s).ToPointer();
	::strcpy(scopy.get(), chars);
	Marshal::FreeHGlobal(IntPtr((void*) chars));

	return scopy.release();
}

unsigned char* Mao::DotNet::NetString2UChars(String^ s, int& length) {

	if (!s) {
		throw gcnew ArgumentNullException("s");
	}

	// use auto_ptr<> as StringToHGlobalAnsi() may throw an exception
	length = s->Length;
	auto_ptr<unsigned char> scopy(new unsigned char[length]);
	if (!scopy.get()) {
		length = 0;
		throw gcnew UnmanagedOutOfMemoryException();
	}

	const char* chars = (const char*)
		Marshal::StringToHGlobalAnsi(s).ToPointer();
	::memcpy(scopy.get(), chars, length);
	Marshal::FreeHGlobal(IntPtr((void*) chars));

	return scopy.release();
}

void Mao::DotNet::NetString2Buf(String^ s, char* buf, int bufsize) {

	if (!s) {
		throw gcnew ArgumentNullException("s");
	}

	if (s->Length >= bufsize) {
		throw gcnew UnmanagedOutOfMemoryException(
			"Too long string to hold in the buffer");
	}

	const char* chars = (const char*)
		Marshal::StringToHGlobalAnsi(s).ToPointer();
	::strcpy(buf, chars);
	Marshal::FreeHGlobal(IntPtr((void*) chars));
}
