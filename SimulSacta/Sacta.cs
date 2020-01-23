using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Diagnostics;

using NLog;
using SimulSACTA.Properties;
using Utilities;

namespace SimulSACTA
{
    class Sacta : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] SectorUcs
        {
            get { return _SectorUcs; }
            set { _SectorUcs = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Sacta()
        {
            Settings stts = Settings.Default;

            _Comm = new UdpSocket[2];

            _Comm[0] = new UdpSocket(stts.SactaIpA, stts.ListenPortA);
            _Comm[1] = new UdpSocket(stts.SactaIpB, stts.ListenPortB);

            if (stts.EnableMulticast == true)
            {
                _Comm[0].Base.JoinMulticastGroup(IPAddress.Parse(stts.SactaMcastA), IPAddress.Parse(stts.SactaIpA));
                _Comm[1].Base.JoinMulticastGroup(IPAddress.Parse(stts.SactaMcastB), IPAddress.Parse(stts.SactaIpA));
            }

            _Comm[0].NewDataEvent += OnNewData;
            _Comm[1].NewDataEvent += OnNewData;

            _EndPoint = new IPEndPoint[2];
            _EndPoint[0] = new IPEndPoint(IPAddress.Parse(stts.ScvIpA), stts.ScvPortA);
            _EndPoint[1] = new IPEndPoint(IPAddress.Parse(stts.ScvIpB), stts.ScvPortB);

            _LastScvReceived = new DateTime[2];

            _PeriodicTasks = new Timer(_PeriodicTasksInterval);
            _PeriodicTasks.AutoReset = false;
            _PeriodicTasks.Elapsed += PeriodicTasks;
        }
        /// <summary>
        /// 
        /// </summary>
        ~Sacta()
        {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            _Comm[0].BeginReceive();
            _Comm[1].BeginReceive();
            _PeriodicTasks.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectorUcs"></param>
        public void RunSectorization(string[] sectorUcs)
        {
            SendManualSectorization(sectorUcs);
        }

        #region IDisposable Members
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private
        /// <summary>
        /// 
        /// </summary>
        const uint _PeriodicTasksInterval = 1000;
        /// <summary>
        /// 
        /// </summary>
        UdpSocket[] _Comm;
        IPEndPoint[] _EndPoint;
        DateTime[] _LastScvReceived;
        DateTime _LastPresenceSended;
        Timer _PeriodicTasks;
        int _ActivityState;
        uint _ActivityTimeOut = Settings.Default.ActivityTimeOut;
        string[] _SectorUcs;
        ushort _SeqNum;
        // ushort[] _SeqNum = new ushort[] { 0, 0 };
        uint _SectVersion=0;
        bool _SendInit;
        bool _Disposed;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bDispose"></param>
        void Dispose(bool bDispose)
        {
            if (!_Disposed)
            {
                _Disposed = true;

                if (bDispose)
                {
                    _PeriodicTasks.Enabled = false;
                    _PeriodicTasks.Close();
                    _PeriodicTasks = null;

                    _Comm[0].Dispose();
                    _Comm[1].Dispose();
                    _Comm = null;

                    _EndPoint = null;
                    _LastScvReceived = null;
                    _SectorUcs = null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dg"></param>
        void OnNewData(object sender, DataGram dg)
        {
            try
            {
                MemoryStream ms = new MemoryStream(dg.Data);
                CustomBinaryFormatter bf = new CustomBinaryFormatter();
                SactaMsg msg = bf.Deserialize<SactaMsg>(ms);

                if (IsValid(msg))
                {
                    int net = (sender == _Comm[0] ? 0 : 1);
                    _LastScvReceived[net] = DateTime.Now;

                    switch (msg.Type)
                    {
                        case SactaMsg.MsgType.Presence:
                            _ActivityTimeOut = (uint)(((SactaMsg.PresenceInfo)(msg.Info)).ActivityTimeOutSg * 1000);
                            if (_ActivityTimeOut < 5) _ActivityTimeOut = Settings.Default.ActivityTimeOut;
                            break;
                        case SactaMsg.MsgType.SectAsk:
                            SendSectorization(net, _SectorUcs, msg.UserOrg);
                            MainForm.LogMethod("INFO", net==0 ? "SACTA1" : "SACTA2", "Recibida Peticion de Sectorizacion");
                            break;
                        case SactaMsg.MsgType.SectAnwer:
                            SactaMsg.SectAnswerInfo info = (SactaMsg.SectAnswerInfo)(msg.Info);
                            MainForm.LogMethod("INFO", net == 0 ? "SACTA1" : "SACTA2", String.Format("Sectorizacion V-{0}: {1}", info.Version, (info.Result == 1 ? "Implantada" : "Rechazada")));
                            break;
                        case SactaMsg.MsgType.Init:
                            MainForm.LogMethod("INFO", net == 0 ? "SACTA1" : "SACTA2", "Recibido MSG INIT");
                            _SeqNum = 0;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!_Disposed)
                {
                    MainForm.LogMethod("ERROR", "OnNewData", String.Format("Excepción al Procesar Datos Recibidos: {0}",ex.Message));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool IsValid(SactaMsg msg)
        {
            Settings stts = Settings.Default;

            return ((msg.DomainOrg == stts.ScvDomain) && (msg.DomainDst == stts.SactaDomain) &&
               (msg.CenterOrg == stts.ScvCenter) && (msg.CenterDst == stts.SactaCenter) &&
               (msg.UserDst == stts.SactaGroupUser) && stts.ScvUsers.Contains(msg.UserOrg.ToString()));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PeriodicTasks(object sender, ElapsedEventArgs e)
        {
            try
            {
                int activityState = ((uint)((DateTime.Now - _LastScvReceived[0]).TotalMilliseconds) < _ActivityTimeOut ? 1 : 0);
                activityState |= ((uint)((DateTime.Now - _LastScvReceived[1]).TotalMilliseconds) < _ActivityTimeOut ? 2 : 0);

                if (activityState != _ActivityState)
                {
                    _ActivityState = activityState;
                    MainForm.LogMethod("INFO", "PeriodicTask", String.Format("Estado de Actividad de SCV Cambiado a {0}", _ActivityState));
                }

                if ((uint)((DateTime.Now - _LastPresenceSended).TotalMilliseconds) > Settings.Default.PresenceInterval)
                {
                    if (!_SendInit)
                    {
                        /* Secuencia a CERO */
                        _SeqNum = 0;

                        SendInit(0);
                        SendInit(1);
                        _SendInit = true;
                    }

                    SendPresence(0);
                    SendPresence(1);
                    /* Incrementar Secuencia */
                    _SeqNum++;
                }
            }
            catch (Exception ex)
            {
                if (!_Disposed)
                {
                    MainForm.LogMethod("ERROR", "PeriodicTask", String.Format("Excepción en Tareas Periodicas de SACTA: {0}", ex.Message));
                }
            }
            finally
            {
                if (!_Disposed)
                {
                    _PeriodicTasks.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        void SendInit(int sactanet)
        {

            Debug.Assert(sactanet == 0 || sactanet == 1);

            CustomBinaryFormatter bf = new CustomBinaryFormatter();
            MemoryStream ms = new MemoryStream();
            SactaMsg msg = new SactaMsg(SactaMsg.MsgType.Init, SactaMsg.InitId);

            bf.Serialize(ms, msg);
            byte[] data = ms.ToArray();

            foreach (string sUser in Settings.Default.ScvUsers)
            {
                byte[] user = BitConverter.GetBytes(UInt16.Parse(sUser));
                Array.Reverse(user);
                Array.Copy(user, 0, data, 6, 2);

                _Comm[sactanet].Send(_EndPoint[sactanet], data);
                MainForm.LogMethod("INFO", sactanet==0 ? "SACTA1" : "SACTA2", String.Format("Enviado MSG Init a SCV {0}", sUser));

                //_Comm[1].Send(_EndPoint[1], data);
                //MainForm.LogMethod("INFO", "SACTA2", String.Format("Enviado MSG Init a SCV {0}", sUser));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sactanet"></param>
        /// <param name="sectorUcs"></param>
        /// <param name="ask"></param>
        void SendSectorization(int sactanet, string[] sectorUcs, ushort UserOrg)
        {
            Debug.Assert(sactanet == 0 || sactanet == 1);

            CustomBinaryFormatter bf = new CustomBinaryFormatter();
            MemoryStream ms = new MemoryStream();
            SactaMsg msg = new SactaMsg(SactaMsg.MsgType.Sectorization, _SeqNum, _SectVersion, sectorUcs);
            bf.Serialize(ms, msg);
            byte[] data = ms.ToArray();

            byte[] user = BitConverter.GetBytes(UserOrg);
            Array.Reverse(user);
            Array.Copy(user, 0, data, 6, 2);

            _Comm[sactanet].Send(_EndPoint[sactanet], data);
            MainForm.LogMethod("INFO", sactanet == 0 ? "SACTA1" : "SACTA2", String.Format("Enviando Sectorizacion V-{2} a SCV {0} ({1})", UserOrg, strSect(sectorUcs), _SectVersion));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectorUcs"></param>
        /// <param name="ask"></param>
        void SendManualSectorization(string[] sectorUcs)
        {
            _SectVersion++;
            foreach (string sUser in Settings.Default.ScvUsers)
            {
                SendSectorization(0, sectorUcs, UInt16.Parse(sUser));
                SendSectorization(1, sectorUcs, UInt16.Parse(sUser));
                _SeqNum++;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        void SendPresence(int sactanet)
        {
            Debug.Assert(sactanet == 0 || sactanet == 1);

            CustomBinaryFormatter bf = new CustomBinaryFormatter();
            MemoryStream ms = new MemoryStream();
            SactaMsg msg = new SactaMsg(SactaMsg.MsgType.Presence, _SeqNum);

            bf.Serialize(ms, msg);
            byte[] data = ms.ToArray();

            foreach (string sUser in Settings.Default.ScvUsers)
            {
                byte[] user = BitConverter.GetBytes(UInt16.Parse(sUser));
                Array.Reverse(user);
                Array.Copy(user, 0, data, 6, 2);

                _Comm[sactanet].Send(_EndPoint[sactanet], data);
            }

            _LastPresenceSended = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectorUcs"></param>
        /// <returns></returns>
        string strSect(string[] sectorUcs) 
        {
            StringBuilder sb = new StringBuilder();
            for (int i=0; i<sectorUcs.Length; i+=2) 
            {
                sb.Append(sectorUcs[i].ToString());
                sb.Append("->");
                sb.Append(sectorUcs[i+1].ToString());
                sb.Append(",");
            }
            return sb.ToString();
        }

        #endregion

        /** 20170214. AGL. Para permitir las 'escuchas mcast' */
        class UdpSocket2
        {
        }

    }
}
