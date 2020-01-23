''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' SNMP++.NET v. 1.21 (2006-07-26 15:30:00)
'
' Copyright (c) 2003-2004 Military Communication Institute, Zegrze, Poland
' Author: Marek Malowidzki
'
' This software is based on SNMP++ from Jochen Katz, Frank Fock,
' which is in turn based on SNMP++2.6 from Hewlett Packard:
'
' Copyright (c) 2001-2003 Jochen Katz, Frank Fock
'
' Copyright (c) 1996
' Hewlett-Packard Company
'
' ATTENTION: USE OF THIS SOFTWARE IS SUBJECT TO THE FOLLOWING TERMS.
' Permission to use, copy, modify, distribute and/or sell this software 
' and/or its documentation is hereby granted without fee. User agrees 
' to display the above copyright notice and this license notice in all 
' copies of the software and any documentation of the software. User 
' agrees to assume all liability for the use of the software; 
' Hewlett-Packard, Jochen Katz and Military Communication Institute make
' no representations about the suitability of this software for any purpose.
' It is provided  "AS-IS" without warranty of any kind, either express
' or implied. User hereby grants a royalty-free license to any and all
' derivatives based upon this software code base. 
' 
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' _############################################################################
' _## 
' _##  SNMP++v3.2.9c
' _##  -----------------------------------------------
' _##  Copyright (c) 2001-2003 Jochen Katz, Frank Fock
' _##
' _##  This software is based on SNMP++2.6 from Hewlett Packard:
' _##  
' _##    Copyright (c) 1996
' _##    Hewlett-Packard Company
' _##  
' _##  ATTENTION: USE OF THIS SOFTWARE IS SUBJECT TO THE FOLLOWING TERMS.
' _##  Permission to use, copy, modify, distribute and/or sell this software 
' _##  and/or its documentation is hereby granted without fee. User agrees 
' _##  to display the above copyright notice and this license notice in all 
' _##  copies of the software and any documentation of the software. User 
' _##  agrees to assume all liability for the use of the software; 
' _##  Hewlett-Packard and Jochen Katz make no representations about the 
' _##  suitability of this software for any purpose. It is provided 
' _##  "AS-IS" without warranty of any kind, either express or implied. User 
' _##  hereby grants a royalty-free license to any and all derivatives based
' _##  upon this software code base. 
' _##  
' _##########################################################################*/
'===================================================================
'
' Copyright (c) 1999
' Hewlett-Packard Company
'
' ATTENTION: USE OF THIS SOFTWARE IS SUBJECT TO THE FOLLOWING TERMS.
' Permission to use, copy, modify, distribute and/or sell this software
' and/or its documentation is hereby granted without fee. User agrees
' to display the above copyright notice and this license notice in all
' copies of the software and any documentation of the software. User
' agrees to assume all liability for the use of the software; Hewlett-Packard
' makes no representations about the suitability of this software for any
' purpose. It is provided "AS-IS without warranty of any kind,either express
' or implied. User hereby grants a royalty-free license to any and all
' derivatives based upon this software code base.
'=====================================================================

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' A system walk SNMPv1 example for SNMP++.NET.
'
' This example has been rewritten from a C# version using an online help so it
' may be a bit lame.
' The namespace below is declared with a unique name,  otherwise it would hide
' the SNMP++.NET classes.
'
' Author:	Marek Malowidzki	2004
' Send comments, suggestions and bug reports to maom_onet@poczta.onet.pl.
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports System
Imports System.Collections
Imports System.Reflection
Imports Org.Snmp.Snmp_pp

Namespace Snmp_ppExample

    Public Class MSystemWalkExample

        Private Shared ReadOnly SYSOID2NAME_ As Hashtable

        Shared Sub New()

#If REQUIRES_CRT_INIT Then
            CRT.Auto.Initialize()
#End If

            SYSOID2NAME_ = New Hashtable
            SYSOID2NAME_.Add("1.3.6.1.2.1.1", "system")
            SYSOID2NAME_.Add("1.3.6.1.2.1.1.1.0", "sysDescr")
            SYSOID2NAME_.Add("1.3.6.1.2.1.1.2.0", "sysObjectID")
            SYSOID2NAME_.Add("1.3.6.1.2.1.1.3.0", "sysUpTime")
            SYSOID2NAME_.Add("1.3.6.1.2.1.1.4.0", "sysContact")
            SYSOID2NAME_.Add("1.3.6.1.2.1.1.5.0", "sysName")
            SYSOID2NAME_.Add("1.3.6.1.2.1.1.6.0", "sysLocation")
            SYSOID2NAME_.Add("1.3.6.1.2.1.1.7.0", "sysServices")
            SYSOID2NAME_.Add("1.3.6.1.2.1.1.8.0", "sysORLastChange")
            SYSOID2NAME_.Add("1.3.6.1.2.1.1.9.0", "sysORTable")

            Dim Key As Object
            For Each Key In DirectCast(SYSOID2NAME_.Clone(), Hashtable).Keys
                Dim Name As Object = SYSOID2NAME_(Key)
                Dim OidVar As Oid = New Oid(Key.ToString())
                SYSOID2NAME_.Add(OidVar, Name)
                SYSOID2NAME_.Add(Name, OidVar)
            Next Key
        End Sub

        Private Sub MSystemWalkExample()
        End Sub

        Public Shared Sub Main(ByVal CmdArgs() As String)

            If CmdArgs.Length < 2 Then
                Console.Error.WriteLine( _
                    "SNMP++.NET {0} built on {1}; SNMP++ v. {2}" + vbNewLine + vbNewLine _
                    + "Usage: {3} <ip> <readCommunity> [<debugMark>]", _
                    [Assembly].GetAssembly(GetType(Snmp)), _
                    Snmp.BuildTime, _
                    Snmp.Snmp_ppVersion, _
                    Environment.GetCommandLineArgs(0))
                Environment.Exit(1)
            End If

            Dim Ip As String = CmdArgs(0), Community As String = CmdArgs(1)
            Dim Debug As Boolean = CmdArgs.Length > 2 AndAlso CmdArgs(2).Length > 0
            Try
                SnmpTarget.DefaultTimeout = 10000
                SnmpTarget.DefaultRetries = 2

                Dim SnmpVar As Snmp = New Snmp(False)
                Try
                    Dim Udp As UdpAddress = New UdpAddress(Ip)
                    Dim Ver As SnmpVersion = SnmpVersion.SNMPv1
                    Dim Target As CTarget = New CTarget(Udp, Ver, Community, Community)
                    Dim SystemOid As Oid = DirectCast(SYSOID2NAME_("system"), Oid)
                    Dim VbVar As Vb = New Vb(SystemOid)
                    Dim PduVar As Pdu = New Pdu(PduType.GetNext, VbVar)
                    While True
                        If Debug Then
                            Console.WriteLine("Sending : " + PduVar.ToString() + " to " + Target.ToString())
                        End If

                        Dim PduRespVar As Pdu = SnmpVar.Invoke(PduVar, Target)

                        If Debug Then
                            Console.WriteLine("Received: " + PduRespVar.ToString())
                        End If

                        VbVar = PduRespVar(0)
                        Dim OidVar As Oid = VbVar.Oid
                        If Not OidVar.StartsWith(SystemOid) Then
                            Exit While
                        End If

                        Dim val As SnmpSyntax = VbVar.Value
                        Console.WriteLine("{0}({1}): {2} ({3})", _
                            OidVar, SYSOID2NAME_(OidVar), val, val.SmiSyntax)

                        PduVar = PduVar.Clone(VbVar)
                    End While
                Finally
                    SnmpVar.Dispose()
                End Try
            Catch e As SnmpException
                Console.WriteLine("SnmpException:" + vbNewLine _
                    + "status : " + e.ErrorStatus + vbNewLine _
                    + "index  : " + e.ErrorIndex + vbNewLine _
                    + "message: " + e.Message)
            Catch e As System.Exception
                Console.WriteLine(e)
            End Try
        End Sub

    End Class

End Namespace
