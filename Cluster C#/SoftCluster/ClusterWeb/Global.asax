<%@ Application Language="C#" %>
<%@ Import Namespace="NLog" %>
<%@ Import Namespace="Utilities" %>
<%@ Import Namespace="ClusterLib" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Timers" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="System.Runtime.Serialization.Formatters.Binary" %>

<script runat="server">

   static UdpSocket _Comm;
   static IPEndPoint _EndPoint1;
   static IPEndPoint _EndPoint2;
   static System.Timers.Timer _PeriodicTasks;
   static DateTime _LastReceived;
   static byte[] _ActivateMsg;
   static byte[] _AskInfoMsg;
   static bool _Disposed;
   static Logger _Logger = LogManager.GetLogger("Global.asax");
   
   void Application_Start(object sender, EventArgs e) 
   {
      try
      {
         Configuration config = WebConfigurationManager.OpenWebConfiguration("/ClusterWeb");
         KeyValueConfigurationElement ip = config.AppSettings.Settings["ListenIp"];
         KeyValueConfigurationElement port = config.AppSettings.Settings["ListenPort"];
         KeyValueConfigurationElement epIp1 = config.AppSettings.Settings["ClusterSrv1Ip"];
         KeyValueConfigurationElement epPort1 = config.AppSettings.Settings["ClusterSrv1Port"];
         KeyValueConfigurationElement epIp2 = config.AppSettings.Settings["ClusterSrv2Ip"];
         KeyValueConfigurationElement epPort2 = config.AppSettings.Settings["ClusterSrv2Port"];

         _Comm = new UdpSocket(ip.Value, Int32.Parse(port.Value));
         _Comm.NewDataEvent += OnNewData;

         _EndPoint1 = new IPEndPoint(IPAddress.Parse(epIp1.Value), Int32.Parse(epPort1.Value));
         _EndPoint2 = new IPEndPoint(IPAddress.Parse(epIp2.Value), Int32.Parse(epPort2.Value));

         MsgType type = MsgType.Activate;
         MemoryStream ms = new MemoryStream();
         BinaryFormatter bf = new BinaryFormatter();
         bf.Serialize(ms, type);
         _ActivateMsg = ms.ToArray();

         type = MsgType.GetState;
         ms.Position = 0;
         bf.Serialize(ms, type);
         _AskInfoMsg = ms.ToArray();

         Application["ClusterWeb_Comm"] = _Comm;
         Application["ClusterWeb_EP1"] = _EndPoint1;
         Application["ClusterWeb_EP2"] = _EndPoint2;
         Application["ClusterWeb_ActivateMsg"] = _ActivateMsg;
         Application["ClusterWeb_Node1"] = new NodeInfo();
         Application["ClusterWeb_Node2"] = new NodeInfo();

         _PeriodicTasks = new System.Timers.Timer(1000);
         _PeriodicTasks.AutoReset = false;
         _PeriodicTasks.Elapsed += PeriodicTasks;

         _Comm.BeginReceive();
         _PeriodicTasks.Enabled = true;
      }
      catch (Exception ex)
      {
         _Logger.FatalException("Error on start application", ex);
      }
   }

   void Application_End(object sender, EventArgs e) 
   {
      _Disposed = true;
      _PeriodicTasks.Enabled = false;
      _PeriodicTasks.Close();
      _Comm.Dispose();

      _PeriodicTasks = null;
      _Comm = null;
   }
     
   void Application_Error(object sender, EventArgs e) 
   { 
     // Code that runs when an unhandled error occurs

   }

   void OnNewData(object sender, DataGram dg)
   {
      try
      {
         BinaryFormatter bf = new BinaryFormatter();
         MemoryStream ms = new MemoryStream(dg.Data);
         ClusterState state = bf.Deserialize(ms) as ClusterState;

         if (state != null)
         {
            Application.Lock();

            if (dg.Client.ToString() == _EndPoint1.ToString())
            {
               Application["ClusterWeb_Node1"] = state.LocalNode;
               Application["ClusterWeb_Node2"] = state.RemoteNode;
            }
            else
            {
               Application["ClusterWeb_Node1"] = state.RemoteNode;
               Application["ClusterWeb_Node2"] = state.LocalNode;
            }
            _LastReceived = DateTime.Now;
            Application.UnLock();
         }
      }
      catch (Exception ex)
      {
         if (_Disposed)
         {
            _Logger.ErrorException("Error on data received from " + dg.Client, ex);
         }
      }
   }

   void PeriodicTasks(object sender, ElapsedEventArgs e)
   {
      try
      {
         Application.Lock();
         if ((DateTime.Now - _LastReceived).TotalMilliseconds > 5000)
         {
            Application["ClusterWeb_Node1"] = new NodeInfo();
            Application["ClusterWeb_Node2"] = new NodeInfo();
         }
         Application.UnLock();
            
         _Comm.Send(_EndPoint1, _AskInfoMsg);
         _Comm.Send(_EndPoint2, _AskInfoMsg);
      }
      catch (Exception ex)
      {
         if (!_Disposed)
         {
            _Logger.ErrorException("Error asking state to nodes", ex);
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

</script>
