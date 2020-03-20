<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_DefaultCluster"
    StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Cluster State</title>
    <link rel="shortcut icon" href="~/favicon.ico">
   <link rel="stylesheet" href="style/loader.css" />

</head>
<body bgcolor="#FFFFFF" text="#000000" style="font-size: 11px; font-family: Arial; border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none;">
   <script type="text/javascript">
//       if (window.name != "<%=GetWindowName()%>") {
//           window.name = "invalidAccess";
//           window.open("../BloqueoAplicacion.aspx", "_self");
//       }

       var node1State = "No Funcional";
       var node2State = "No Funcional";
       var serverStatus = false;
       var bConmutando = false;

       setInterval("OnTimer()", 5000);

       function OnTimer() {
           serverStatus = false;
           CallServer("State", "");
           setTimeout("RefreshServer()", 3000);

       }


       function ReceiveServerData(arg) {
           var info = arg.split("@");

           if (info.length == 2) {
               var node1Info = new NodeInfo(info[0]);
               var node2Info = new NodeInfo(info[1]);

               ShowNodeInfo("Node1", node1Info, "Node2", node2Info);

               node1State = node1Info.State;
               node2State = node2Info.State;

               delete node1Info;
               delete node2Info;

               serverStatus = true;
           }
       }

       function RefreshServer() {
           if (serverStatus == false && bConmutando == false) {
               window.location.reload();
           }

       }

       function RefreshPage() {
           //Se oculta el div del loader si está activo
           if (document.getElementById("initloader").style.display != "none") {
               document.getElementById("initloader").style.display = "none";
           }
           bConmutando = false;

           //Se fuerza la recarga la página del servidor y no de la cache
           //después de conmutar
           window.location.reload(true);
           
       }

       function NodeInfo(info) {
           var result = {};
           var pairs = info.split("&");
           var keyValue;

           for (var i = 0; i < pairs.length; i++) {
               keyValue = pairs[i].split("=");
               result[keyValue[0]] = keyValue[1];
           }

           this.Name = result["Name"];
           this.State = result["State"];
           this.StateBegin = result["StateBegin"];
           this.Adapter1 = result["Adapter1"];
           this.Adapter2 = result["Adapter2"];
           this.VirtualIp1 = result["VirtualIp1"];
           this.VirtualIp2 = result["VirtualIp2"];
           this.ReplicationState = result["ReplicationState"];
           this.DataReplicationState = result["DataReplicationState"];

           if (!this.Name) {
               this.Name = Strings.No_Operativo;    // "Nodo no operativo";
           }
           if (!this.State) {
               this.State = Strings.No_Funcional;   //"No Funcional";
           }
           if (!this.StateBegin || (this.State == "No Funcional")) {
               this.StateBegin = "";
           }
           if (!this.Adapter1 || (this.State == "No Funcional")) {
               this.Adapter1 = "";
           }
           if (!this.Adapter2 || (this.State == "No Funcional")) {
               this.Adapter2 = "";
           }
       }


       function ShowNodeInfo(node, nodeInfo, node2, nodeInfo2) {
           var strEstadoNodo1 = Strings.No_Funcional;
           var strEstadoNodo2 = Strings.No_Funcional;

           var node1N = document.getElementById("Node1Img");
           var node2N = document.getElementById("Node2Img");
           document.getElementById("Errores").innerHTML = ""

           document.getElementById("LblReplicationServer1").innerHTML = "OFF"
           document.getElementById("LblReplicationServer2").innerHTML = "OFF"
           document.getElementById("ReplicationImg").src = (nodeInfo.DataReplicationState != "True" || nodeInfo2.DataReplicationState != "True") ? "Images/DataReplicationOff.gif" : "Images/DataReplicationOk.gif";

           if (nodeInfo.State == "Activo" && nodeInfo2.State == "No Activo") {
               node1N.src = "Images/PC_Green.gif";
               node1N.title = Strings.Nodo1_A_Nodo2_NA;

               node2N.src = "Images/PC_Blue.gif";
               node2N.title = Strings.Nodo1_A_Nodo2_NA_Click;

               strEstadoNodo1 = Strings.Activo;
               strEstadoNodo2 = Strings.No_Activo;
           }
           else
               if (nodeInfo2.State == "Activo" && nodeInfo.State == "No Activo") {
               node1N.src = "Images/PC_Blue.gif";
               node1N.title = Strings.Nodo1_NA_Nodo2_A_Click;
               node2N.src = "Images/PC_Green.gif";
               node2N.title = Strings.Nodo1_NA_Nodo2_A;

               strEstadoNodo1 = Strings.No_Activo;
               strEstadoNodo2 = Strings.Activo;
           }
           else
               if (nodeInfo.State == "Activo" && nodeInfo2.State == "No Funcional") {
               node1N.src = "Images/PC_Orange.gif";
               node1N.title = Strings.Nodo1_A_Nodo2_P;
               node2N.src = "Images/PC.gif";
               node2N.title = Strings.Nodo1_A_Nodo2_P;

               strEstadoNodo1 = Strings.Activo;
               strEstadoNodo2 = Strings.No_Funcional;
           }
           else
               if (nodeInfo2.State == "Activo" && nodeInfo.State == "No Funcional") {
               node1N.src = "Images/PC.gif";
               node1N.title = Strings.Nodo1_P_Nodo2_A;
               node2N.src = "Images/PC_Orange.gif";
               node2N.title = Strings.Nodo1_P_Nodo2_A;

               strEstadoNodo1 = Strings.No_Funcional;
               strEstadoNodo2 = Strings.Activo;
           }
           else
               if (nodeInfo2.State == "No Funcional" && nodeInfo.State == "No Funcional") {
               node1N.src = "Images/PC.gif";
               node1N.title = Strings.Nodo1_P_Nodo2_P;
               node2N.src = "Images/PC.gif";
               node2N.title = Strings.Nodo1_P_Nodo2_P;

               strEstadoNodo1 = Strings.No_Funcional;
               strEstadoNodo2 = Strings.No_Funcional;
           }
           else
               if (nodeInfo2.State == "Activando Nodo" && nodeInfo.State == "No Funcional") {
               node1N.src = "Images/PC.gif";
               node2N.src = "Images/PC_Red.gif";
               node2N.title = Strings.Error_Nodo_1;
               node1N.title = Strings.Error_Nodo_1
               document.getElementById("Errores").innerHTML = Strings.Error_Cluster;
               strEstadoNodo1 = Strings.No_Funcional;
               strEstadoNodo2 = Strings.Activando_Nodo;
           }
           else
               if (nodeInfo.State == "Activando Nodo" && nodeInfo2.State == "No Funcional") {
               node2N.src = "Images/PC.gif";
               node1N.src = "Images/PC_Red.gif";
               node1N.title = Strings.Error_Nodo_2;
               node2N.title = Strings.Error_Nodo_2;
               document.getElementById("Errores").innerHTML = Strings.Error_Cluster;
               strEstadoNodo1 = Strings.Activando_Nodo;
               strEstadoNodo2 = Strings.No_Funcional;
           }
           else
               if (nodeInfo2.State == "Activando Nodo" || nodeInfo.State == "Activando Nodo") {
               node1N.src = "Images/PC_Blue.gif";
               node2N.src = "Images/PC_Blue.gif";
               strEstadoNodo1 = Strings.Activando_Nodo;
               strEstadoNodo2 = Strings.Activando_Nodo;
           }


           document.getElementById(node + "Name").innerHTML = nodeInfo.Name;
           document.getElementById(node + "State").innerHTML = Strings.Estado + ": " + strEstadoNodo1;

           document.getElementById(node2 + "Name").innerHTML = nodeInfo2.Name;
           document.getElementById(node2 + "State").innerHTML = Strings.Estado + ": " + strEstadoNodo2;

           if (nodeInfo.State == "No Funcional") {
               document.getElementById(node + "Adapter1").innerHTML = "";
               document.getElementById(node + "Adapter2").innerHTML = "";
               document.getElementById(node + "Begin").innerHTML = "";
           }
           else {
               document.getElementById(node + "Adapter1").innerHTML = Strings.Adaptador + "1: " + nodeInfo.Adapter1;
               document.getElementById(node + "Adapter2").innerHTML = Strings.Adaptador + "2: " + nodeInfo.Adapter2;
               document.getElementById(node + "Begin").innerHTML = Strings.Desde + ": " + nodeInfo.StateBegin;
           }


           if (nodeInfo2.State == "No Funcional") {
               document.getElementById(node2 + "Adapter1").innerHTML = "";
               document.getElementById(node2 + "Adapter2").innerHTML = "";
               document.getElementById(node2 + "Begin").innerHTML = "";
           }
           else {
               document.getElementById(node2 + "Adapter1").innerHTML = Strings.Adaptador + "1: " + nodeInfo2.Adapter1;
               document.getElementById(node2 + "Adapter2").innerHTML = Strings.Adaptador + "2: " + nodeInfo2.Adapter2;
               document.getElementById(node2 + "Begin").innerHTML = Strings.Desde + ": " + nodeInfo2.StateBegin;
           }

           if (nodeInfo.State == "Activando Nodo" || nodeInfo2.State == "Activando Nodo") {
               document.getElementById("VirtualIP1").innerHTML = "";
               document.getElementById("VirtualIP2").innerHTML = "";
           }
           else
               if (nodeInfo.State == "No Funcional" && nodeInfo2.State == "No Funcional") {
               document.getElementById("VirtualIP1").innerHTML = "";
               document.getElementById("VirtualIP2").innerHTML = "";
           }
           else {
                   document.getElementById("LblReplicationServer1").innerHTML = (nodeInfo.State != "No Funcional" && nodeInfo.ReplicationState == "2" ) ? "ON" : "OFF"
                   document.getElementById("LblReplicationServer2").innerHTML = (nodeInfo2.State != "No Funcional" && nodeInfo2.ReplicationState == "2") ? "ON" : "OFF"
                if (nodeInfo.State == "Activo") {
                   document.getElementById("VirtualIP1").innerHTML = nodeInfo.VirtualIp1;
                   document.getElementById("VirtualIP2").innerHTML = nodeInfo.VirtualIp2;
                   }
               else
                   if (nodeInfo2.State == "Activo") {
                       document.getElementById("VirtualIP1").innerHTML = nodeInfo2.VirtualIp1;
                       document.getElementById("VirtualIP2").innerHTML = nodeInfo2.VirtualIp2;
                   }
           }
       }

       function HandleClick(node) {
           if ((node == "Node1") && (node1State == "No Activo")) {
               var conmutar = confirm(Strings.Seguro_Conmutar)
               if (conmutar) {
                   bConmutando = true;
                   //Se activa el loader ...
                   document.getElementById("initloader").style.display = "block";
                   CallServer("ActivateNode1", "");
                   setTimeout("RefreshPage()", 5000);
               }
           }
          else if ((node == "Node2") && (node2State == "No Activo")) {
               var conmutar = confirm(Strings.Seguro_Conmutar)
               if (conmutar) {
                   bConmutando = true;
                   //Se activa el loader ...
                   document.getElementById("initloader").style.display = "block";
                   CallServer("ActivateNode2", "");
                   setTimeout("RefreshPage()", 5000);
               }
           }
       }

   </script>
   <form id="frmCallback" name="frmCallback" runat="server" class="estiloDiv gradpositivo white">
		 <asp:ScriptManager ID="ScriptManager1" EnableScriptLocalization="true" runat="server">
				  <Scripts>
						<asp:ScriptReference Path="scripts/strings.js" ResourceUICultures="en,es,fr" />
				  </Scripts>
		  </asp:ScriptManager>           
        <div style="z-index: 102; left: 8px; width: 1265px; position: absolute; top: 8px;background-color:transparent;
            height: 100px;" class="estiloDiv red" >
            <img src="../Images/LogoNucleo20x190.png" style="z-index: 109; left: 21px; width: 190px; position: absolute;top: 11px; height: 50px; border-right-color: black; right: 1053px;" />            
            <asp:Label ID="Label1" runat="server" Style="z-index: 100; left: 245px; position: absolute; top: 17px; height: 28px; width: 368px; right: 392px;" 
                SkinID="LabelCabecera" meta:resourcekey="Label1Resource1"></asp:Label>

<%--            <asp:Button ID="BtMantenimiento" runat="server" BackColor="LightBlue" BorderColor="Black"
                BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Font-Overline="False"
                Font-Underline="False" ForeColor="Blue" OnClick="BtMantenimiento_Click"
                Style="left: 477px; position: absolute; top: 76px; z-index: 101;" 
					  Text="Mantenimiento" Width="110px" 
					  meta:resourcekey="BtMantenimientoResource1" />
            <asp:Button ID="BtConfiguracion" runat="server" BackColor="LightBlue" BorderColor="Black"
                BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" ForeColor="Blue" OnClick="BtConfiguracion_Click"
                Style="left: 361px; position: absolute; top: 76px; z-index: 102;" 
					  Text="Configuracion" Width="110px" 
					  meta:resourcekey="BtConfiguracionResource1" />
            <asp:Button ID="BtInicio" runat="server" BackColor="LightBlue" BorderColor="Black"
                BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" ForeColor="Blue" OnClick="BtInicio_Click"
                Style="left: 245px; position: absolute; top: 76px; z-index: 103;" 
					  Text="Inicio" Width="110px" meta:resourcekey="BtInicioResource1" />--%>

            <asp:LinkButton ID="BtCfg" runat="server" OnClick="BtConfiguracion_Click"
                Style="position: absolute; top: 65px; z-index: 101; left: 477px; height: 25px; text-align:center; width:116px"
                Text="Configuracion" CausesValidation="False" SkinID="LinkButtonCabecera"  
                meta:resourcekey="BtConfiguracionResource1" UseSubmitBehavior="false" />
            <asp:LinkButton ID="BtMantenimiento" runat="server" OnClick="BtMantenimiento_Click"
                Style="left: 361px; position: absolute; top: 65px; z-index: 102; height: 25px; text-align:center; width:116px"
                Text="Mantenimiento" CausesValidation="False" SkinID="LinkButtonCabecera"
                meta:resourcekey="BtMantenimientoResource1" UseSubmitBehavior="false" />
            <asp:LinkButton ID="BtInicio" runat="server" OnClick="BtInicio_Click"
                Style="left: 245px; position: absolute; top: 65px; z-index: 103; text-align:center; width:116px"
                Text="Inicio" CausesValidation="False" SkinID="LinkButtonCabecera"
                meta:resourcekey="BtInicioResource1" UseSubmitBehavior="false" />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        </div>

       <div id="VirtualIP2" style="position: absolute; left: 335px; top: 383px; width: 259px; display:none;
         height: 14px; z-index: 105; font-weight: bold; font-size: 12px; text-align: center;" align="left">
       </div>
    
       <div id="VirtualIP1" style="position: absolute; left: 335px; top: 362px; width: 259px;
        height: 14px; z-index: 106; font-weight: bold; font-size: 12px; text-align: center;" align="left">
      </div>
      <div id="Node1Name" style="position: absolute; left: 754px; top: 185px; width: 114px;
         height: 14px; z-index: 100; font-weight: bold; font-size: 12px; text-align: center;" align="left">
         <asp:Label ID="Lbl1" runat="server" Text="Nodo no operativo" 
				meta:resourcekey="Lbl1Resource1" />
      </div>
      <div id="Node1State" style="position: absolute; left: 85px; top: 154px; width: 150px;
         height: 14px; z-index: 101" align="left">
         <asp:Label ID="Lbl3" runat="server" Text="Estado: No Funcional" 
				meta:resourcekey="Lbl3Resource1" />
      </div>
      <div id="Node1Begin" style="position: absolute; left: 85px; top: 174px; width: 160px;
         height: 14px; z-index: 102" align="left">
      </div>
      <div id="Node1Adapter1" style="position: absolute; left: 85px; top: 194px; width: 150px;
         height: 14px; z-index: 103" align="left">
      </div>
      <div id="Node1Adapter2" style="position: absolute; left: 85px; top: 214px; width: 150px; display:none;
         height: 14px; z-index: 104" align="left">
      </div>
      <div id="Node2Name" style="position: absolute; left: 754px; top: 293px; width: 114px;
         height: 14px; z-index: 107; font-weight: bold; font-size: 12px; text-align: center;" align="left">
         <asp:Label ID="Lbl2" runat="server" Text="Nodo no operativo" 
				meta:resourcekey="Lbl2Resource1" />
      </div>
      <div id="Node2State" style="position: absolute; left: 85px; top: 262px; width: 150px;
         height: 14px; z-index: 108">
         <asp:Label ID="Label4" runat="server" Text="Estado: No Funcional" 
				meta:resourcekey="Label4Resource1" />
      </div>
      <div id="Node2Begin" style="position: absolute; left: 85px; top: 282px; width: 160px;
         height: 14px; z-index: 109">
      </div>
      <div id="Node2Adapter1" style="position: absolute; left: 85px; top: 302px; width: 150px;
         height: 14px; z-index: 110" align="left">
      </div>
      <div id="Node2Adapter2" style="position: absolute; left: 85px; top: 322px; width: 150px; display:none;
         height: 14px; z-index: 111" align="left">
      </div>
       &nbsp;
       &nbsp;
       &nbsp;&nbsp;&nbsp;&nbsp;

       <div style="border-style: none; z-index: 113; left: 247px; width: 448px; position: absolute; top: 161px; height: 63px; ">
           <img id="Node1Img" onClick="HandleClick('Node1')" src="Images/PC.GIF" style="cursor:hand;z-index: 100; left: 0px; width: 447px;
               position: absolute; top: 2px; height: 62px"/>
           &nbsp;
       </div>
       <div style="border-style: none; z-index: 213; left: 454px; width: 44px; position: absolute; top: 233px; height: 31px; ">
           <img id="ReplicationImg" src="Images/DataReplicationOff.gif" style="z-index: 100; left: 1px; width: 40px; position: absolute;
               top: 3px; height: 25px; "/>
       </div>
       <div style="border-style: none; z-index: 116; left: 247px; width: 448px; position: absolute; top: 268px; height: 63px; ">
           <img id="Node2Img" onClick="HandleClick('Node2')" src="Images/PC.GIF" style="cursor:hand;z-index: 100; left: 0px; width: 447px;
               position: absolute; top: 2px; height: 62px" />
           </div>
       
       <div id="initloader" runat="server">
            <div id="Desactivador" >
            </div>
            <div class="loader"></div>
            <p id="TextLoader"> </p>
       </div>

       <hr style="border-style: solid; border-color: black; z-index: 115; left: 284px; position: absolute; top: 343px; width: 366px; height: 1px;" />

       <div style="z-index: 117; left: 15px; width: 27px; position: absolute; top: 458px;
           height: 23px">
           <img src="Images/Green.GIF" style="z-index: 100; left: 1px; width: 18px; position: absolute;
               top: 3px; height: 19px" />
       </div>
       <div style="z-index: 118; left: 15px; width: 28px; position: absolute; top: 488px;
           height: 22px">
           <img src="Images/Blue.GIF" style="z-index: 100; left: 2px; width: 18px; top: 0px;
               height: 19px" /></div>
       <div style="z-index: 119; left: 15px; width: 27px; position: absolute; top: 519px;
           height: 16px">
           <img src="Images/Orange.GIF" style="z-index: 100; left: 0px; width: 18px; position: absolute;
               top: -2px; height: 19px" />
       </div>
       <div style="z-index: 120; left: 15px; width: 26px; position: absolute; top: 545px;
           height: 22px">
           <img src="Images/Gray.GIF" style="z-index: 100; left: 0px; width: 18px; position: absolute;
               top: 0px; height: 19px" />
       </div>
       <div style="z-index: 120; left: 15px; width: 26px; position: absolute; top: 573px;
           height: 22px">
           <img src="Images/Red.GIF" style="z-index: 100; left: 0px; width: 18px; position: absolute;
               top: 0px; height: 19px" />
       </div>
       <div style="z-index: 121; left: 44px; width: auto; position: absolute; top: 462px;
           height: 17px">
	         <asp:Label ID="Label5" runat="server" 
					Text="Nodo Activo y nodo remoto listo y a la espera" 
					meta:resourcekey="Label5Resource1" />
           
           <div style="z-index: 124; left: 0px; width: 500px; position: absolute; top: 29px;
               height: 17px">
	         <asp:Label ID="Label6" runat="server" 
					  Text="Nodo listo y a la espera y nodo remoto Activo" 
					  meta:resourcekey="Label6Resource1" />
           </div>
       </div>
       <div style="z-index: 122; left: 44px; width: auto; position: absolute; top: 519px;
           height: 17px">
	         <asp:Label ID="Label7" runat="server" Text="Nodo Activo y nodo remoto parado" 
					meta:resourcekey="Label7Resource1" />
       </div>
       <div style="z-index: 124; left: 45px; width: auto; position: absolute; top: 546px;
           height: 17px">
	         <asp:Label ID="Label8" runat="server" Text="Nodo parado" 
					meta:resourcekey="Label8Resource1" />
       </div>
       <div style="z-index: 124; left: 45px; width: auto; position: absolute; top: 573px;
           height: 17px">
	         <asp:Label ID="Label3" runat="server" Text="Activando Nodo" 
					meta:resourcekey="Label3Resource1" />
       </div>
       <div id="Errores" style="font-weight: bold; z-index: 125; left: 223px; width: 488px;
           color: red; position: absolute; top: 425px; height: 16px; text-align: center">
       </div>
        <div style="z-index: 301; left: 8px; width: 1264px; position: absolute; top: 855px; height: 37px; text-align:center; color: black; font-family:Arial; font-size:small" >
            <br\ />
            &#169 Núcleo. 2020. v2.6.0. All rights reserved.
        </div>	
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" style="position:absolute; top: 177px; left: 889px;" Text="Servicio replicación:" meta:resourcekey="LbServicioRepResource1"></asp:Label>
                    </td>
                </tr>
                    <td>
                        <asp:Label ID="LblReplicationServer1" runat="server" style="position:absolute; top: 203px; left: 889px;"></asp:Label>
                    </td>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" style="position:absolute; top: 279px; left: 889px;" Text="Servicio replicación:"  meta:resourcekey="LbServicioRepResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LblReplicationServer2" runat="server" style="position:absolute; top: 309px; left: 889px;"></asp:Label>
                    </td>
                </tr>
            </table>
       </form>
    </body>
</html>