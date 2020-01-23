using System;
using System.Net;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using log4net;
using log4net.Config;
using Utilities;
using ClusterLib;

public partial class _DefaultCluster : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
{
   string _Command;
   //static Logger _Logger = LogManager.GetCurrentClassLogger();
   private static ILog _logDebugView;
   public static ILog logDebugView
   {
       get
       {
           if (_logDebugView == null)
           {
               log4net.Config.XmlConfigurator.Configure();
               _logDebugView = LogManager.GetLogger("CLUSTER");
           }
           return _logDebugView;
       }
   }

   protected void Page_Load(object sender, EventArgs e)
   {
       //Si no está autentificado se redirige a la pagina de login
       if (Context.Request.IsAuthenticated)
       {
           // retrieve user's identity from httpcontext user 
           FormsIdentity ident = (FormsIdentity)Context.User.Identity;
           string perfil = ident.Ticket.UserData;
           //A esta pantalla sólo tiene acceso los usuarios con perfil 3
           if (perfil != "3")
           {
               Response.Redirect("~/Login.aspx", false);
               return;
           }

           RegisterScript();
       }
       else
       {
           Response.Redirect("~/Login.aspx", false);
       }
   }

   public string GetWindowName()
   {
       if (Session["WindowName"] != null)
           return Session["WindowName"].ToString();

       Session["WindowName"] = Guid.NewGuid().ToString().Replace("-", "");
       return Session["WindowName"].ToString();
   }

   protected void BtInicio_Click(object sender, EventArgs e)
   {
       Response.Redirect("~/Default.aspx", false);
   }

   protected void BtConfiguracion_Click(object sender, EventArgs e)
   {
       Response.Redirect("~/Configuracion/Inicio.aspx", false);
   }

   protected void BtMantenimiento_Click(object sender, EventArgs e)
   {
       System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
       string urlMantto = config.AppSettings.Settings["UrlMantto"].Value;
       string urlRtn = config.AppSettings.Settings["UrlRetorno"].Value;

       Response.Redirect(urlMantto + "?RetUrl=" + urlRtn + "&user=" + Context.User.Identity.Name);
   }

   private void RegisterScript()
   {
       string cbReference = ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
       string callbackScript = "function CallServer(arg, context) {" + cbReference + "; }";
       ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
   }

#region ICallbackEventHandler Members

   public string GetCallbackResult()
   {
      string str = "";
      string s = String.Format("Received ask for {0}", _Command);
      logDebugView.Debug(s);
      try
      {
          if (_Command == "State")
          {
              string[] stateStr = new string[] { "No Funcional", "Activando Nodo", "Activo", "No Activo" };

              Application.Lock();
              NodeInfo node1 = (NodeInfo)Application["ClusterWeb_Node1"];
              NodeInfo node2 = (NodeInfo)Application["ClusterWeb_Node2"];
              Application.UnLock();

              if (node1 != null && node2 != null)
              {
                  bool dataReplicationState = (bool)Application["Data_Replication_State"] && (node1.ReplicationServiceState == "2" && node2.ReplicationServiceState == "2");
                  str = string.Format("Name={0}&State={1}&StateBegin={2}&Adapter1={3}&Adapter2={4}&VirtualIp1={5}&VirtualIp2={6}&ReplicationState={7}&DataReplicationState={8}",
                     node1.Name, stateStr[(int)node1.State], node1.StateBegin, node1.AdapterIp1, node1.AdapterIp2, node1.VirtualIp1, node1.VirtualIp2, node1.ReplicationServiceState, dataReplicationState);
                  str += "@";
                  str += string.Format("Name={0}&State={1}&StateBegin={2}&Adapter1={3}&Adapter2={4}&VirtualIp1={5}&VirtualIp2={6}&ReplicationState={7}&DataReplicationState={8}",
                     node2.Name, stateStr[(int)node2.State], node2.StateBegin, node2.AdapterIp1, node2.AdapterIp2, node2.VirtualIp1, node2.VirtualIp2, node2.ReplicationServiceState, dataReplicationState);
                  string s1 = String.Format("Sending state: {0}", str);
                  logDebugView.Debug(s1);
              }
          }
          else if (_Command == "ActivateNode1")
          {
              Application.Lock();
              UdpSocket comm = (UdpSocket)Application["ClusterWeb_Comm"];
              IPEndPoint ep = (IPEndPoint)Application["ClusterWeb_EP1"];
              byte[] msg = (byte[])Application["ClusterWeb_ActivateMsg"];
              Application.UnLock();

              comm.Send(ep, msg);
          }
          else if (_Command == "ActivateNode2")
          {
              Application.Lock();
              UdpSocket comm = (UdpSocket)Application["ClusterWeb_Comm"];
              IPEndPoint ep = (IPEndPoint)Application["ClusterWeb_EP2"];
              byte[] msg = (byte[])Application["ClusterWeb_ActivateMsg"];
              Application.UnLock();

              comm.Send(ep, msg);
          }
      }
      catch (Exception ex)
      {
          logDebugView.Error("(Cluster-Default.aspx)GetCallbackResult", ex);
      }

      return str;
   }

   public void RaiseCallbackEvent(string eventArgument)
   {
      _Command = eventArgument;
   }

   #endregion
}
