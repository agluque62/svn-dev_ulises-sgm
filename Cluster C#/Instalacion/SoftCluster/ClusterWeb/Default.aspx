<%@ page language="C#" autoeventwireup="true" inherits="_Default, App_Web_dw7q36xf" enablesessionstate="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   <title>SoftCluster State</title>

   <script type="text/javascript">
      var node1State = "No Funcional";
      var node2State = "No Funcional";
      
      setInterval("OnTimer()", 2000);
      
      function OnTimer()
      {
         CallServer("State", "");
      }
      
      function ReceiveServerData(arg)
      {
         var info = arg.split("@");
         
         if (info.length == 2)
         {
            var node1Info = new NodeInfo(info[0]);
            var node2Info = new NodeInfo(info[1]);
            
            if ((node1Info.State == "No Funcional") || (node2Info.State == "No Funcional"))
            {
               document.getElementById("Flecha").style.visibility = "hidden";
            }
            else
            {
               document.getElementById("Flecha").style.visibility = "visible";
            }
            
            ShowNodeInfo("Node1", node1Info);
            ShowNodeInfo("Node2", node2Info);
            
            node1State = node1Info.State;
            node2State = node2Info.State;
            
            delete node1Info;
            delete node2Info;
         }
      }
      
      function NodeInfo(info)
      {
         var result = {};
         var pairs = info.split("&");
         var keyValue;
         
         for (var i = 0; i < pairs.length; i++)
         {
            keyValue = pairs[i].split("=");
            result[keyValue[0]] = keyValue[1];
         }
         
         this.Name = result["Name"];
         this.State = result["State"];
         this.StateBegin = result["StateBegin"];
         this.Adapter1 = result["Adapter1"];
         this.Adapter2 = result["Adapter2"];
         
         if (!this.Name)
         {
            this.Name = "Nodo no operativo";
         }
         if (!this.State)
         {
            this.State = "No Funcional";
         }
         if (!this.StateBegin || (this.State == "No Funcional"))
         {
            this.StateBegin = "";
         }
         if (!this.Adapter1 || (this.State == "No Funcional"))
         {
            this.Adapter1 = "";
         }
         if (!this.Adapter2 || (this.State == "No Funcional"))
         {
            this.Adapter2 = "";
         }
      }
      
      function ShowNodeInfo(node, nodeInfo)
      {
         var nodeBT = document.getElementById(node);

         if (nodeInfo.State == "Activo")
         {
            nodeBT.style.backgroundImage = "url(Images/ActiveNode.bmp)";
            nodeBT.disabled = "disabled";
         }
         else
         {
            nodeBT.style.backgroundImage = "url(Images/InactiveNode.bmp)";
            if (nodeInfo.State == "No Activo")
            {
               nodeBT.disabled = undefined;
            }
            else
            {
               nodeBT.disabled = "disabled";
            }
         }

         document.getElementById(node + "Name").innerHTML = nodeInfo.Name; 
         document.getElementById(node + "State").innerHTML = "Estado: " + nodeInfo.State; 
         
         if (nodeInfo.State == "No Funcional")
         {
            document.getElementById(node + "Adapter1").innerHTML = ""; 
            document.getElementById(node + "Adapter2").innerHTML = "";
            document.getElementById(node + "Begin").innerHTML = "";
         }
         else
         {
            document.getElementById(node + "Adapter1").innerHTML = "Adaptador1: " + nodeInfo.Adapter1; 
            document.getElementById(node + "Adapter2").innerHTML = "Adaptador2: " + nodeInfo.Adapter2;
            document.getElementById(node + "Begin").innerHTML = "Desde: " + nodeInfo.StateBegin;
         }
      }     
      
      function HandleClick(node)
      {         
         if ((node == "Node1") && (node1State == "No Activo"))
         {
            CallServer("ActivateNode1", "");
         }
         else if ((node == "Node2") && (node2State == "No Activo"))
         {
            CallServer("ActivateNode2", "");
         }
      }
       
   </script>

</head>
<body bgcolor="#FFFFFF" text="#000000" style="font-size: 11px; font-family: Arial;">
   <form id="frmCallback" name="frmCallback" runat="server">
      <div id="wb_Node1" style="position: absolute; left: 60px; top: 30px; width: 60px;
         height: 79px; z-index: 1" align="center">
         <button id="Node1" type="button" onclick="HandleClick('Node1')" name="Node1" style="width: 60px;
            height: 79px; background-color: transparent; background-image: url(Images/InactiveNode.bmp);
            cursor: hand;" disabled="disabled">
         </button>
      </div>
      <div id="wb_Node2" style="position: absolute; left: 370px; top: 30px; width: 60px;
         height: 79px; z-index: 2" align="center">
         <button id="Node2" type="button" onclick="HandleClick('Node2')" name="Node2" style="width: 60px;
            height: 79px; background-color: transparent; background-image: url(Images/InactiveNode.bmp);
            cursor: hand;" disabled="disabled">
         </button>
      </div>
      <div id="wb_Image1" style="overflow: hidden; position: absolute; left: 125px; top: 60px;
         z-index: 3" align="left">
         <img src="Images/flecha.bmp" id="Flecha" alt="" align="top" border="0" style="width: 238px;
            height: 24px; visibility: hidden;" /></div>
      <div id="Node1Name" style="position: absolute; left: 60px; top: 120px; width: 150px;
         height: 14px; z-index: 4" align="left">
         Nodo no operativo</div>
      <div id="Node1State" style="position: absolute; left: 60px; top: 140px; width: 150px;
         height: 14px; z-index: 5" align="left">
         Estado: No Funcional</div>
      <div id="Node1Begin" style="position: absolute; left: 60px; top: 160px; width: 150px;
         height: 14px; z-index: 6" align="left">
      </div>
      <div id="Node1Adapter1" style="position: absolute; left: 60px; top: 180px; width: 150px;
         height: 14px; z-index: 7" align="left">
      </div>
      <div id="Node1Adapter2" style="position: absolute; left: 60px; top: 200px; width: 150px;
         height: 14px; z-index: 8" align="left">
      </div>
      <div id="Node2Name" style="position: absolute; left: 370px; top: 120px; width: 150px;
         height: 14px; z-index: 9" align="left">
         Nodo no operativo</div>
      <div id="Node2State" style="position: absolute; left: 370px; top: 140px; width: 150px;
         height: 14px; z-index: 10" align="left">
         Estado: No Funcional</div>
      <div id="Node2Begin" style="position: absolute; left: 370px; top: 160px; width: 150px;
         height: 14px; z-index: 11" align="left">
      </div>
      <div id="Node2Adapter1" style="position: absolute; left: 370px; top: 180px; width: 150px;
         height: 14px; z-index: 12" align="left">
      </div>
      <div id="Node2Adapter2" style="position: absolute; left: 370px; top: 200px; width: 150px;
         height: 14px; z-index: 13" align="left">
      </div>
      <div id="Trace" style="position: absolute; left: 60px; top: 250px; width: 150px;
         height: 14px; z-index: 14" align="left">
         <asp:HyperLink ID="HyperLink1" runat="server" EnableViewState="False" NavigateUrl="~/Trace.axd">TraceInfo</asp:HyperLink>
      </div>
   </form>
</body>
</html>
