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
// This code is heavily based on MaoSNMP (which is the part of the Custom
// Screens Builder), which required semi-automatic translation from Java.
//
// Author:	Marek Malowidzki	2003, 2004
// Send comments, suggestions and bug reports to maom_onet@poczta.onet.pl.
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;
using Org.Snmp.Snmp_pp;

namespace Org.Snmp.Snmp_pp
{
	/// <summary>
	/// This class provides useful high-level SNMP tables retrieval mechanisms.
	/// </summary>
	/// <remarks>This class is a collection of static methods. Thus, instances
	/// cannot be created.</remarks>
	public sealed class TableReader
	{
		#region Fields and Constants

		private const int	MAX_DATAGRAM_SIZE_	= 1500,
							MAX_HEADER_V1_SIZE_	= 58,	// 20 (IP) + 8 (UDP) + 30+ (SNMPv1/v2c)
							MAX_HEADER_V3_SIZE_	= 140,	// 20 (IP) + 8 (UDP) + 112+ (SNMPv3/authNoPriv)
							AVG_VARBIND_SIZE_	= 30;
		private static bool	useAsyncInvoke_;

		#endregion

		#region Types

		/// <summary>
		/// Provides options for table retrieval operations.
		/// </summary>
		public struct GetTableOptions
		{
			/// <summary>
			/// The row index to start from or <b>null</b>.
			/// </summary>
			public Oid startRowIndex;

			/// <summary>
			/// The row index to finish at or <b>null</b>.
			/// </summary>
			public Oid endRowIndex;

			/// <summary>
			/// Maximum number of rows to retrieve. Specify 0 to read the whole
			/// table.
			/// </summary>
			public int maxRows;

			/// <summary>
			/// Number of rows to read in a single query. Only valid if the request
			/// used will be GetBulk. Specify 0 to enable a simple heuristic algorithm
			/// that dynamically computes this value so that the response fits a single
			/// Ethernet packet.
			/// </summary>
			public int rowsPerQuery;
		}

		private sealed class GetTableState : IAsyncResult
		{
			#region Types

			public delegate Pdu SnmpInvoke(IAsyncResult ar);

			#endregion

			#region Fields

			// constant fields
			public readonly	Snmp		snmp;
			public readonly	SnmpTarget	target;
			public readonly	Oid			endRowIndex;
			public readonly	int			maxRows,
										rowsPerQuery;
			public readonly	AsyncCallback callback;
			public readonly object		callbackData;
			public readonly SnmpInvoke	invoke;

			// state
			public readonly	Oid[]		columnOids;
			public readonly Hashtable	rows;
			public Pdu					pdu;
			public Vb[]					vbs;
			public int					nRequests;

			// wait state & result
			private volatile bool		completed_;
			private ManualResetEvent	handle_;
			private Exception			exc_;

			#endregion

			#region Constructors

			public GetTableState(Snmp snmp, Pdu pdu, SnmpTarget target,
				Oid[] columnOids, Oid endRowIndex,
				int maxRows, int rowsPerQuery,
				Vb[] vbs,
				AsyncCallback callback, object callbackData, bool sync)
			{
				this.snmp         = snmp;
				this.target       = target;
				this.columnOids   = columnOids;
				this.endRowIndex  = endRowIndex;
				this.maxRows      = maxRows;
				this.rowsPerQuery = rowsPerQuery;
				this.rows         = new Hashtable();
				this.callback     = callback;
				this.callbackData = callbackData;
				this.invoke       = sync
									? new SnmpInvoke(SyncInvoke)
									: new SnmpInvoke(AsyncInvoke);
				this.pdu          = pdu;
				this.vbs          = vbs;
			}

			#endregion

			#region Properties

			public object SyncRoot { get { return rows; } }

			#endregion

			#region Public Methods

			// This method internally uses either synchronous or asynchronous
			// interface of the Snmp class, depending on useAsyncInvoke_, but
			// is synchronous itself.
			public Pdu SyncInvoke(IAsyncResult ar)
			{
				if (useAsyncInvoke_)
				{
					// use asynchronous invocation
					ar = snmp.BeginInvoke(pdu, target, null, null);
					return snmp.EndInvoke(ar);
				}
				else
				{
					// the old-good synchronous call
					return snmp.Invoke(pdu, target);
				}
			}

			public Pdu AsyncInvoke(IAsyncResult ar)
			{
				return snmp.EndInvoke(ar);
			}

			public void Done(Exception exc)
			{
				lock (SyncRoot)
				{
					pdu = null;
					vbs = null;

					if ((exc_ = exc) != null)
					{
						rows.Clear();
					}

					completed_ = true;
					if (handle_ != null)
					{
						handle_.Set();
					}
				}

				if (callback != null)
				{
					try
					{
						callback(this);
					}
					catch (Exception e)
					{
						Console.Error.WriteLine(e);
					}
				}
			}

			public Vb[][] GetResult(ref int nRequests)
			{
				if (!completed_)
				{
					WaitHandle handle = AsyncWaitHandle;
					handle.WaitOne();
					handle.Close();
				}

				lock (SyncRoot)
				{
					nRequests += this.nRequests;
					this.nRequests = 0;
					if (exc_ == null)
					{
						Vb[][] vbTable = ExtractRows(rows);
						rows.Clear();
						return vbTable;
					}
					else
					{
						throw exc_;
					}
				}
			}

			#endregion

			#region IAsyncResult Members

			public object AsyncState { get { return callbackData; } }

			public bool CompletedSynchronously { get { return false; } }

			public WaitHandle AsyncWaitHandle
			{
				get
				{
					lock (SyncRoot)
					{
						if (handle_ == null)
							handle_ = new ManualResetEvent(completed_);
						return handle_;
					}
				}
			}

			public bool IsCompleted { get { return completed_; } }

			#endregion
		}

		#endregion

		#region Constructors

		// non instantiable
		private TableReader() {}

		#endregion

		#region Private Methods

		private static int GetMaxRepetitions(PduType type, SnmpVersion version,
			int vbCount, int rowCount, int maxRows, int rowsPerQuery)
		{
			if (type != PduType.GetBulk)
				return 0;

			// This is a simple heuristic algorithm that allows to limit
			// the size of a GetBulk response (to fit in a MTU packet).
			if (rowsPerQuery <= 0)
			{
				int header = version == SnmpVersion.SNMPv3
					? MAX_HEADER_V3_SIZE_ : MAX_HEADER_V1_SIZE_;
				rowsPerQuery = ((MAX_DATAGRAM_SIZE_ - header)
					/ vbCount) / AVG_VARBIND_SIZE_;
			}

			int toRead = maxRows - rowCount;
			if (toRead < rowsPerQuery && toRead > 0)
				rowsPerQuery = toRead;
			return rowsPerQuery;
		}

		private static int GetMaxRepetitions(GetTableState state)
		{
			Pdu pdu = state.pdu;
			return GetMaxRepetitions(pdu.Type, state.target.Version,
				pdu.Count, state.rows.Count, state.maxRows, state.rowsPerQuery);
		}

		private static Vb[] CreateNullVbs(Oid[] columnOids, Oid startRowIndex)
		{
			Vb[] vbs = new Vb[columnOids.Length];
			for (int i = 0; i < vbs.Length; i++)
			{
				Oid oid = columnOids[i];
				if (startRowIndex != null)
					oid = oid.Append(startRowIndex);
				vbs[i] = new Vb(oid);
			}
			return vbs;
		}

		private static Vb[] CreateNullVbs(Oid[] oids, Vb[] arrayToReuse)
		{
			if(arrayToReuse == null || arrayToReuse.Length != oids.Length)
				arrayToReuse = new Vb[oids.Length];
			for (int i = 0; i < arrayToReuse.Length; i++)
				arrayToReuse[i] = new Vb(oids[i]);
			return arrayToReuse;
		}

		private static int GetNonNullCount(object[] oa)
		{
			int nNonNulls = 0;
			for (int i = 0; i < oa.Length; i++)
			{
				if (oa[i] != null)
					nNonNulls++;
			}
			return nNonNulls;
		}

		private static GetTableState PrepareGetTable(
			Snmp snmp, Pdu pdu, SnmpTarget target,
			Oid[] columnOids, Oid startRowIndex, Oid endRowIndex,
			int maxRows, int rowsPerQuery, AsyncCallback callback, object callbackData, bool sync)
		{
			if (!(pdu.Type == PduType.GetNext || pdu.Type == PduType.GetBulk))
				throw new ArgumentException("Invalid pdu type: " + pdu.Type, "pdu");

			if (columnOids.Length == 0)
				throw new ArgumentException("Empty column oid table", "columnOids");

			if (maxRows <= 0)
				maxRows = int.MaxValue;

			columnOids = (Oid[]) columnOids.Clone();
			Vb[] vbs = CreateNullVbs(columnOids, startRowIndex);
			pdu = pdu.Clone(vbs);
			pdu.NonRepeaters = 0;

			return new GetTableState(snmp, pdu, target, columnOids,
						endRowIndex, maxRows, rowsPerQuery, vbs,
						callback, callbackData, sync);
		}

		// Processes the next response to GET NEXT/GET BULK request. Map 'rows'
		// contains already received rows  (possibly only partially filled with
		// variable bindings). The method returns new OIDs which are to be used
		// in the subsequent requests. If this is the last response, this array
		// will be empty. Note that 'columnOids' is also modified and nulls are
		// set for columns that have been finished.
		private static Oid[] ProcessResponse(
			Oid[] columnOids, Vb[] vbs, Oid endRowIndex, Hashtable rows)
		{
			int nNonNulls = GetNonNullCount(columnOids);
			Oid[] oids = new Oid[columnOids.Length];
			Oid rowIndex = null;
			Vb[] row = null;

			for (int i = 0, o = 0;
				i < vbs.Length && nNonNulls > 0;
				i++, o = ++o % columnOids.Length) 
			{
				while (columnOids[o] == null)
					o = ++o % columnOids.Length;
				Oid columnOid = columnOids[o];

				Vb vb = vbs[i];
				Oid oid = vb.Oid;
				SnmpSyntax val = vb.Value;

				// Check whether this Vb should be included in the results
				// and whether this column has been finished. 'val' should
				// never be NULL but some faulty agents send NULLs.
				bool endOfColumn
					= !oid.StartsWith(columnOid)
					|| (val != null && val.SmiSyntax == SmiSyntax.EndOfMibView);
				bool includeVb = !endOfColumn;

				Oid rowIdx = null;
				if (!endOfColumn)
				{
					rowIdx = oid.TrimLeft(columnOid.Length);
					if (endRowIndex != null)
					{
						int res = rowIdx.CompareTo(endRowIndex);
						endOfColumn = res >= 0;
						includeVb   = res <= 0;
					}
				}

				// if a valid value has been returned, store it in the hash
				// table and store its OID for a subsequent request
				if (includeVb) 
				{
					oids[o] = oid;
					if (rowIndex == null || !rowIdx.Equals(rowIndex))
					{
						rowIndex = rowIdx;
						row = (Vb[]) rows[rowIndex];
						if (row == null) 
						{
							row = new Vb[columnOids.Length];
							rows.Add(rowIndex, row);
						}
					}
					row[o] = vb;
				}

				// if the column has been finished, remove it from further
				// processing
				if (endOfColumn)
				{
					if (val != null && val.SmiSyntax == SmiSyntax.EndOfMibView) 
					{
						columnOids[o] = null;
						nNonNulls--;
					}
					oids[o] = null;
				}
			}

			// set finished columns of 'columnOids' to null
			// and remove nulls from 'oids'
			int noids = 0;
			for (int i = 0; i < oids.Length; i++) 
			{
				if (oids[i] == null)
					columnOids[i] = null;
				else
					oids[noids++] = oids[i];
			}

			// shrink 'oids', if necessary
			if(noids < oids.Length) 
			{
				Oid[] o = new Oid[noids];
				Array.Copy(oids, 0, o, 0, noids);
				oids = o;
			}
			return oids;
		}

		private static Oid[] ProcessSnmpv1EndOfMIB(SnmpVersion version,
			Oid[] columnOids, Pdu request, SnmpException e)
		{
			int index = 0;
			if (version == SnmpVersion.SNMPv1
				&& e.ErrorStatus == SnmpError.NoSuchName
				&& (index = e.ErrorIndex) > 0
				&& index <= request.Count)
			{
				index--;
				for (int i = 0, j = 0; i < columnOids.Length; i++)
				{
					if (columnOids[i] != null && j++ == index)
					{
						columnOids[i] = null;
						break;
					}
				}

				Vb[]  vbs  = request.Vbs;
				Oid[] oids = new Oid[vbs.Length - 1];
				for (int i = 0; i < index; i++)
				{
					oids[i] = vbs[i].Oid;
				}
				for (int i = index; i < oids.Length; i++)
				{
					oids[i] = vbs[i + 1].Oid;
				}
				return oids;
			}
			else
			{
				return null;
			}
		}

		private static Vb[][] ExtractRows(Hashtable rows)
		{
			Oid[] keys = new Oid[rows.Count];
			rows.Keys.CopyTo(keys, 0);
			Array.Sort(keys);

			Vb[][] vbRows = new Vb[keys.Length][];
			for (int i = 0; i < vbRows.Length; i++)
				vbRows[i] = (Vb[]) rows[keys[i]];
			return vbRows;
		}

		private static bool ProcessGetTable(GetTableState state,
			IAsyncResult ar, ref int nRequests)
		{
			Pdu pdu = state.pdu;
			Oid[] oids;
			try
			{
				Pdu resp = state.invoke(ar);
				oids = ProcessResponse(
					state.columnOids, resp.Vbs, state.endRowIndex, state.rows);
			}
			catch (SnmpException e)
			{
				oids = ProcessSnmpv1EndOfMIB(
					state.target.Version, state.columnOids, pdu, e);
				if (oids == null)
				{
					throw;
				}
			}
			nRequests++;

			if (oids.Length == 0 || state.rows.Count >= state.maxRows)
			{
				return true;
			}
			else
			{
				state.vbs = CreateNullVbs(oids, state.vbs);
				state.pdu = pdu.Clone(state.vbs);
				return false;
			}
		}

		private static void GetTableCallback(IAsyncResult ar)
		{
			GetTableState state = (GetTableState) ar.AsyncState;
			Exception exc = null;
			try
			{
				bool done;
				lock (state.SyncRoot)
				{
					done = ProcessGetTable(state, ar, ref state.nRequests);
				}
				if (!done)
				{
					state.pdu.MaxRepetitions = GetMaxRepetitions(state);
					state.snmp.BeginInvoke(state.pdu, state.target,
						new AsyncCallback(GetTableCallback), state);
					return;
				}
			}
			catch (Exception e)
			{
				exc = e;
			}

			state.Done(exc);
		}

		#endregion
        
		#region Public Methods

		/// <summary>
		/// Retrieves an SNMP table from an agent.
		/// </summary>
		/// <param name="snmp">The SNMP session object to use.</param>
		/// <param name="pdu">The PDU object used to convey the request type
		/// and some security parameters (variable bindings are ignored; may
		/// be an empty array).</param>
		/// <param name="target">The SNMP target.</param>
		/// <param name="columnOids">The OIDs of table columns to retrieve. The
		/// columns should share table indexes  (e.g., be a part of the same or
		/// AUGMENTed table).</param>
		/// <param name="startRowIndex">The row index to start from or <b>null</b>.</param>
		/// <param name="endRowIndex">The row index to finish at or <b>null</b>.</param>
		/// <param name="maxRows">Maximum number of rows to retrieve. Specify 0
		/// to read the whole table.</param>
		/// <param name="rowsPerQuery">Number of rows to read in a single query.
		/// Only valid if the request used will be GetBulk. Specify 0 to enable
		/// a simple heuristic algorithm that dynamically computes this value
		/// so that the response fits a single Ethernet packet.</param>
		/// <param name="nRequests">Will be incremented every time a response
		/// is received.</param>
		/// <returns>The retrieved rows. Due to performance reasons, the rows
		/// are returned as a jagged array  but the array is guaranteed to be
		/// rectangular.  It contains either valid variable bindings or <b>null</b>
		/// values for table "holes".</returns>
		/// <remarks>This method returns table rows for the specified set of
		/// columns. A typical example includes the retrieval of a complete
		/// table but a number of parameters makes this method more flexible.
		/// For more information on internal behavior, see the
		/// <see cref="TableReader.UseAsyncInvoke"/> property.</remarks>
		public static Vb[][] GetTable(Snmp snmp, Pdu pdu, SnmpTarget target,
			Oid[] columnOids, Oid startRowIndex, Oid endRowIndex,
			int maxRows, int rowsPerQuery, ref int nRequests)
		{
			GetTableState state = PrepareGetTable(snmp, pdu, target, columnOids,
							startRowIndex, endRowIndex, maxRows, rowsPerQuery,
							null, null, true);
			do
			{
				state.pdu.MaxRepetitions = GetMaxRepetitions(state);
			}
			while (!ProcessGetTable(state, null, ref nRequests));

			return ExtractRows(state.rows);
		}

		/// <summary>
		/// Retrieves an SNMP table from an agent.
		/// </summary>
		/// <param name="snmp">The SNMP session object to use.</param>
		/// <param name="pdu">The PDU object used to convey the request type
		/// and some security parameters (variable bindings are ignored; may
		/// be an empty array).</param>
		/// <param name="target">The SNMP target.</param>
		/// <param name="columnOids">The OIDs of table columns to retrieve. The
		/// columns should share table indexes  (e.g., be a part of the same or
		/// AUGMENTed table).</param>
		/// <param name="options">The options for the operation.</param>
		/// <param name="nRequests">Will be incremented every time a response
		/// is received.</param>
		/// <returns>The retrieved rows. Due to performance reasons, the rows
		/// are returned as a jagged array  but the array is guaranteed to be
		/// rectangular.  It contains either valid variable bindings or <b>null</b>
		/// values for table "holes".</returns>
		/// <remarks>This method returns table rows for the specified set of
		/// columns. A typical example includes the retrieval of a complete
		/// table but a number of parameters makes this method more flexible.
		/// For more information on internal behavior, see the
		/// <see cref="TableReader.UseAsyncInvoke"/> property.</remarks>
		public static Vb[][] GetTable(Snmp snmp, Pdu pdu, SnmpTarget target,
			Oid[] columnOids, GetTableOptions options, ref int nRequests)
		{
			return GetTable(snmp, pdu, target, columnOids,
				options.startRowIndex, options.endRowIndex,
				options.maxRows, options.rowsPerQuery, ref nRequests);
		}

		/// <summary>
		/// Retrieves an SNMP table from an agent.
		/// </summary>
		/// <param name="snmp">The SNMP session object to use.</param>
		/// <param name="pdu">The PDU object used to convey the request type
		/// and some security parameters (variable bindings are ignored; may
		/// be an empty array).</param>
		/// <param name="target">The SNMP target.</param>
		/// <param name="columnOids">The OIDs of table columns to retrieve. The
		/// columns should share table indexes  (e.g., be a part of the same or
		/// AUGMENTed table).</param>
		/// <param name="startRowIndex">The row index to start from or <b>null</b>.</param>
		/// <param name="maxRows">Maximum number of rows to retrieve. Specify 0
		/// to read the whole table.</param>
		/// <returns>The retrieved rows. Due to performance reasons, the rows
		/// are returned as a jagged array  but the array is guaranteed to be
		/// rectangular.  It contains either valid variable bindings or <b>null</b>
		/// values for table "holes".</returns>
		/// <remarks>This method returns table rows for the specified set of
		/// columns. A typical example includes the retrieval of a complete
		/// table but a number of parameters makes this method more flexible.
		/// For more information on internal behavior, see the
		/// <see cref="TableReader.UseAsyncInvoke"/> property.</remarks>
		public static Vb[][] GetTable(Snmp snmp, Pdu pdu, SnmpTarget target,
			Oid[] columnOids, Oid startRowIndex, int maxRows)
		{
			int nRequests = 0;
			return GetTable(snmp, pdu, target, columnOids,
				startRowIndex, null, maxRows, 0, ref nRequests);
		}

		/// <summary>
		/// Retrieves an SNMP table from an agent.
		/// </summary>
		/// <param name="snmp">The SNMP session object to use.</param>
		/// <param name="pdu">The PDU object used to convey the request type
		/// and some security parameters (variable bindings are ignored; may
		/// be an empty array).</param>
		/// <param name="target">The SNMP target.</param>
		/// <param name="columnOids">The OIDs of table columns to retrieve. The
		/// columns should share table indexes  (e.g., be a part of the same or
		/// AUGMENTed table).</param>
		/// <returns>The retrieved rows. Due to performance reasons, the rows
		/// are returned as a jagged array  but the array is guaranteed to be
		/// rectangular.  It contains either valid variable bindings or <b>null</b>
		/// values for table "holes".</returns>
		/// <remarks>This method returns table rows for the specified set of
		/// columns. A typical example includes the retrieval of a complete
		/// table. For more information on internal behavior, see the
		/// <see cref="TableReader.UseAsyncInvoke"/> property.</remarks>
		public static Vb[][] GetTable(Snmp snmp, Pdu pdu, SnmpTarget target,
			Oid[] columnOids)
		{
			return GetTable(snmp, pdu, target, columnOids,
				null, 0);
		}

		/// <summary>
		/// Starts an asynchronous SNMP table retrieval operation.
		/// </summary>
		/// <param name="snmp">The SNMP session object to use.</param>
		/// <param name="pdu">The PDU object used to convey the request type
		/// and some security parameters (variable bindings are ignored; may
		/// be an empty array).</param>
		/// <param name="target">The SNMP target.</param>
		/// <param name="columnOids">The OIDs of table columns to retrieve. The
		/// columns should share table indexes  (e.g., be a part of the same or
		/// AUGMENTed table).</param>
		/// <param name="startRowIndex">The row index to start from or <b>null</b>.</param>
		/// <param name="endRowIndex">The row index to finish at or <b>null</b>.</param>
		/// <param name="maxRows">Maximum number of rows to retrieve. Specify 0
		/// to read the whole table.</param>
		/// <param name="rowsPerQuery">Number of rows to read in a single query.
		/// Only valid if the request used will be GetBulk. Specify 0 to enable
		/// a simple heuristic algorithm that dynamically computes this value
		/// so that the response fits a single Ethernet packet.</param>
		/// <param name="callback">Callback method to invoke when a table is read
		/// or <b>null</b>.</param>
		/// <param name="callbackData">User state.</param>
		/// <returns><see cref="IAsyncResult"/> object.</returns>
		/// <remarks>
		/// This method starts asynchronous table retrieval for the specified set
		/// of columns. A typical example includes the retrieval of a whole table
		/// but a number of parameters makes this method more flexible.  In order
		/// to complete the operation and free related resources, <see cref="EndGetTable"/>
		/// must be later called,  usually, in the callback method specified
		/// by <b>callback</b> parameter.
		/// </remarks>
		public static IAsyncResult BeginGetTable(Snmp snmp, Pdu pdu, SnmpTarget target,
			Oid[] columnOids, Oid startRowIndex, Oid endRowIndex,
			int maxRows, int rowsPerQuery,
			AsyncCallback callback, object callbackData)
		{
			GetTableState state = PrepareGetTable(snmp, pdu, target, columnOids,
							startRowIndex, endRowIndex, maxRows, rowsPerQuery,
							callback, callbackData, false);
			state.pdu.MaxRepetitions = GetMaxRepetitions(state);
			snmp.BeginInvoke(state.pdu, state.target,
				new AsyncCallback(GetTableCallback), state);
			return state;
		}

		/// <summary>
		/// Starts an asynchronous SNMP table retrieval operation.
		/// </summary>
		/// <param name="snmp">The SNMP session object to use.</param>
		/// <param name="pdu">The PDU object used to convey the request type
		/// and some security parameters (variable bindings are ignored; may
		/// be an empty array).</param>
		/// <param name="target">The SNMP target.</param>
		/// <param name="columnOids">The OIDs of table columns to retrieve. The
		/// columns should share table indexes  (e.g., be a part of the same or
		/// AUGMENTed table).</param>
		/// <param name="options">The options for the operation.</param>
		/// <param name="callback">Callback method to invoke when a table is read
		/// or <b>null</b>.</param>
		/// <param name="callbackData">User state.</param>
		/// <returns><see cref="IAsyncResult"/> object.</returns>
		/// <remarks>
		/// This method starts asynchronous table retrieval for the specified set
		/// of columns. A typical example includes the retrieval of a whole table
		/// but a number of parameters makes this method more flexible.  In order
		/// to complete the operation and free related resources, <see cref="EndGetTable"/>
		/// must be later called,  usually, in the callback method specified
		/// by <b>callback</b> parameter.
		/// </remarks>
		public static IAsyncResult BeginGetTable(Snmp snmp, Pdu pdu, SnmpTarget target,
			Oid[] columnOids, GetTableOptions options,
			AsyncCallback callback, object callbackData)
		{
			return BeginGetTable(snmp, pdu, target, columnOids,
				options.startRowIndex, options.endRowIndex,
				options.maxRows, options.rowsPerQuery, callback, callbackData);
		}

		/// <summary>
		/// Starts an asynchronous SNMP table retrieval operation.
		/// </summary>
		/// <param name="snmp">The SNMP session object to use.</param>
		/// <param name="pdu">The PDU object used to convey the request type
		/// and some security parameters (variable bindings are ignored; may
		/// be an empty array).</param>
		/// <param name="target">The SNMP target.</param>
		/// <param name="columnOids">The OIDs of table columns to retrieve. The
		/// columns should share table indexes  (e.g., be a part of the same or
		/// AUGMENTed table).</param>
		/// <param name="startRowIndex">The row index to start from or <b>null</b>.</param>
		/// <param name="maxRows">Maximum number of rows to retrieve. Specify 0
		/// to read the whole table.</param>
		/// <param name="callback">Callback method to invoke when a table is read
		/// or <b>null</b>.</param>
		/// <param name="callbackData">User state.</param>
		/// <returns><see cref="IAsyncResult"/> object.</returns>
		/// <remarks>
		/// This method starts asynchronous table retrieval for the specified set
		/// of columns. A typical example includes the retrieval of a whole table
		/// but a number of parameters makes this method more flexible.  In order
		/// to complete the operation and free related resources, <see cref="EndGetTable"/>
		/// must be later called,  usually, in the callback method specified
		/// by <b>callback</b> parameter.
		/// </remarks>
		public static IAsyncResult BeginGetTable(Snmp snmp, Pdu pdu, SnmpTarget target,
			Oid[] columnOids, Oid startRowIndex, int maxRows,
			AsyncCallback callback, object callbackData)
		{
			return BeginGetTable(snmp, pdu, target, columnOids,
				startRowIndex, null, maxRows, 0, callback, callbackData);
		}

		/// <summary>
		/// Starts an asynchronous SNMP table retrieval operation.
		/// </summary>
		/// <param name="snmp">The SNMP session object to use.</param>
		/// <param name="pdu">The PDU object used to convey the request type
		/// and some security parameters (variable bindings are ignored; may
		/// be an empty array).</param>
		/// <param name="target">The SNMP target.</param>
		/// <param name="columnOids">The OIDs of table columns to retrieve. The
		/// columns should share table indexes  (e.g., be a part of the same or
		/// AUGMENTed table).</param>
		/// <param name="callback">Callback method to invoke when a table is read
		/// or <b>null</b>.</param>
		/// <param name="callbackData">User state.</param>
		/// <returns><see cref="IAsyncResult"/> object.</returns>
		/// <remarks>
		/// This method starts asynchronous table retrieval for the specified set
		/// of columns. A typical example includes the retrieval of a whole table.
		/// In order to complete the operation and free related resources,
		/// <see cref="EndGetTable"/> must be later called, usually, in the callback
		/// method specified by <b>callback</b> parameter.
		/// </remarks>
		public static IAsyncResult BeginGetTable(Snmp snmp, Pdu pdu, SnmpTarget target,
			Oid[] columnOids,
			AsyncCallback callback, object callbackData)
		{
			return BeginGetTable(snmp, pdu, target, columnOids,
				null, 0, callback, callbackData);
		}

		/// <summary>
		/// Completes an asynchronous SNMP table retrieval operation.
		/// </summary>
		/// <param name="ar"><see cref="IAsyncResult"/> object returned by
		/// <see cref="BeginGetTable"/>.</param>
		/// <param name="nRequests">This parameter will be incremented by the
		/// number of received responses.</param>
		/// <returns>The retrieved rows. Due to performance reasons, the rows
		/// are returned as a jagged array  but the array is guaranteed to be
		/// rectangular.  It contains either valid variable bindings or <b>null</b>
		/// values for table "holes".</returns>
		/// <remarks>
		/// The method waits for the asynchronous <see cref="BeginGetTable"/> operation
		/// to complete and returns retrieved rows or throws an exception on
		/// error.
		/// </remarks>
		public static Vb[][] EndGetTable(IAsyncResult ar, ref int nRequests)
		{
			return ((GetTableState) ar).GetResult(ref nRequests);
		}

		/// <summary>
		/// Completes an asynchronous SNMP table retrieval operation.
		/// </summary>
		/// <param name="ar"><see cref="IAsyncResult"/> object returned by
		/// <see cref="BeginGetTable"/>.</param>
		/// <returns>The retrieved rows. Due to performance reasons, the rows
		/// are returned as a jagged array  but the array is guaranteed to be
		/// rectangular.  It contains either valid variable bindings or <b>null</b>
		/// values for table "holes".</returns>
		/// <remarks>
		/// The method waits for the asynchronous <see cref="BeginGetTable"/> operation
		/// to complete and returns retrieved rows or throws an exception on
		/// error.
		/// </remarks>
		public static Vb[][] EndGetTable(IAsyncResult ar)
		{
			int nRequests = 0;
			return EndGetTable(ar, ref nRequests);
		}

		/// <summary>
		/// Returns the row index for the supplied row of variable bindings that
		/// corresponds to the specified columns.
		/// </summary>
		/// <param name="vbRow">The variable bindings. At least one of them must
		/// be non-null, which is guaranteed by the <see cref="GetTable"/> method.
		/// </param>
		/// <param name="columnOids">The set of columns.</param>
		/// <returns>This row's index or <b>null</b>.</returns>
		public static Oid GetRowIndex(Vb[] vbRow, Oid[] columnOids)
		{
			for (int i = 0; i < vbRow.Length; i++)
			{
				if (vbRow[i] != null)
				{
					return vbRow[i].Oid.TrimLeft(columnOids[i].Length);
				}
			}
			return null;
		}

		/// <summary>
		/// Prints the SNMP table returned by <see cref="GetTable"/>.
		/// </summary>
		/// <param name="os">The writer to use.</param>
		/// <param name="columnOids">The column OIDs.</param>
		/// <param name="vbTable">The table of variable bindings.</param>
		public static void PrintTable(TextWriter os, Oid[] columnOids, Vb[][] vbTable)
		{
			foreach (Vb[] vbRow in vbTable)
			{
				if (vbRow.Length > 0)
				{
					Oid rowIndex = GetRowIndex(vbRow, columnOids);
					os.WriteLine("Row index: " + rowIndex);

					for (int j = 0; j < vbRow.Length; j++)
					{
						if (vbRow[j] != null)
						{
							Oid oid = vbRow[j].Oid;
							SnmpSyntax val = vbRow[j].Value;
							os.WriteLine("{0},{1} ({2})", oid, val, val.SmiSyntax);
						}
						else
						{
							Oid oid = columnOids[j].Append(rowIndex);
							os.WriteLine("{0}, ({1})", oid, SmiSyntax.Null);
						}
					}
					os.WriteLine();
				}
			}
		}

		/// <summary>
		/// Sets interface type to use by synchronous <see cref="GetTable"/>
		/// operations.
		/// </summary>
		/// <remarks>
		/// While <see cref="GetTable"/> offers a synchronous interface, it
		/// may internally employ either synchronous or asynchronous SNMP++
		/// calls. The property allows to select the call type. Setting the
		/// value to <b>true</b> enables asynchronous calls.  The property
		/// should be set once, at application startup; it is not synchronized
		/// and changes may not be immediately visible in other threads.</remarks>
		public static bool UseAsyncInvoke
		{
			get { return useAsyncInvoke_; }
			set { useAsyncInvoke_ = value; }
		}

		#endregion
	}
}
