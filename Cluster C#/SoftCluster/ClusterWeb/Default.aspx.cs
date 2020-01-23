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
using NLog;

using Utilities;
using ClusterLib;

public partial class _Default : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
{
   string _Command;
   static Logger _Logger = LogManager.GetCurrentClassLogger();

   protected void Page_Load(object sender, EventArgs e)
   {
       string cbReference = ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
       string callbackScript = "function CallServer(arg, context) {" + cbReference + "; }";
       ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
   }

   #region ICallbackEventHandler Members

   public string GetCallbackResult()
   {
      string str = "";

      _Logger.Debug("Received ask for {0}", _Command);

      if (_Command == "State")
      {
         string[] stateStr = new string[] { "No Funcional", "Activando Nodo", "Activo", "No Activo" };

         Application.Lock();
         NodeInfo node1 = (NodeInfo)Application["ClusterWeb_Node1"];
         NodeInfo node2 = (NodeInfo)Application["ClusterWeb_Node2"];
         Application.UnLock();

         str = string.Format("Name={0}&State={1}&StateBegin={2}&Adapter1={3}&Adapter2={4}&VirtualIp1={5}&VirtualIp2={6}",
            node1.Name, stateStr[(int)node1.State], node1.StateBegin, node1.AdapterIp1, node1.AdapterIp2, node1.VirtualIp1, node1.VirtualIp2);
         str += "@";
         str += string.Format("Name={0}&State={1}&StateBegin={2}&Adapter1={3}&Adapter2={4}&VirtualIp1={5}&VirtualIp2={6}",
            node2.Name, stateStr[(int)node2.State], node2.StateBegin, node2.AdapterIp1, node2.AdapterIp2, node2.VirtualIp1, node2.VirtualIp2);

         _Logger.Debug("Sending state: {0}", str);
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

      return str;
   }

   public void RaiseCallbackEvent(string eventArgument)
   {
      _Command = eventArgument;
   }

   #endregion
}
