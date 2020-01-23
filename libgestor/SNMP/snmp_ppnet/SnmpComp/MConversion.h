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
*	.NET conversion library for converting data types between managed and
*	unmanaged code.
*
*	Author:	Marek Malowidzki	2004
*/

#ifndef	__MCONVERSION_H
#define	__MCONVERSION_H

#include <string>
#include "MTypes.h"

using namespace std;
using namespace System;

namespace Mao {

namespace DotNet {

///////////////////////////////////////////////////////////////////////////////
// Data types conversion routines.
///////////////////////////////////////////////////////////////////////////////

string NetString2StdString(String^ s);
char* NetString2Chars(String^ s);
unsigned char* NetString2UChars(String^ s, int& length);
void NetString2Buf(String^ s, char* buf, int bufsize);

}	// end of namespace Memory

}	// end of namespace Mao

#endif
