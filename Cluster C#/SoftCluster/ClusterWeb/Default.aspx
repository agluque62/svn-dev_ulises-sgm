<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
   EnableSessionState="False" %>

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
            
            ShowNodeInfo("Node1", node1Info,"Node2", node2Info);
            
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
         this.VirtualIp1 = result["VirtualIp1"];
         this.VirtualIp2 = result["VirtualIp2"];
         
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
         
      
      function ShowNodeInfo(node, nodeInfo,node2, nodeInfo2)
      {
         var node1N = document.getElementById("Node1Img");
         var node2N = document.getElementById("Node2Img");
         document.getElementById("Errores").innerHTML ="";
         if (nodeInfo.State == "Activo" && nodeInfo2.State == "No Activo")
         {
            node1N.src = "Images/PC_Green.gif";
            node1N.title ="Nodo 1 ACTIVO, Nodo 2 NO ACTIVO";
            
            node2N.src = "Images/PC_Blue.gif";
            node2N.title ="Nodo 1 ACTIVO, Nodo 2 NO ACTIVO. Click para conmutar";                      
         }
         else
             if (nodeInfo2.State == "Activo" && nodeInfo.State == "No Activo")
             {
                node1N.src = "Images/PC_Blue.gif";
                node1N.title ="Nodo1 NO ACTIVO, Nodo2 ACTIVO. Click para conmutar";
                node2N.src = "Images/PC_Green.gif";
                node2N.title ="Nodo1 NO ACTIVO, Nodo2 ACTIVO";
             }  
             else
                 if (nodeInfo.State == "Activo" && nodeInfo2.State == "No Funcional")
                 {
                    node1N.src = "Images/PC_Orange.gif";
                    node1N.title ="Nodo1 ACTIVO, Nodo2 PARADO. No es posible conmutar";
                    node2N.src = "Images/PC.gif";
                    node2N.title ="Nodo1 ACTIVO, Nodo2 PARADO. No es posible conmutar";
                 }  
                 else
                     if (nodeInfo2.State == "Activo" && nodeInfo.State == "No Funcional")
                     {
                        node1N.src = "Images/PC.gif";
                        node1N.title ="Nodo1 PARADO, Nodo2 ACTIVO. No es posible conmutar";
                        node2N.src = "Images/PC_Orange.gif";
                        node2N.title ="Nodo1 PARADO, Nodo2 ACTIVO. No es posible conmutar";
                     } 
                     else
                        if (nodeInfo2.State == "No Funcional" && nodeInfo.State == "No Funcional")
                        {
                            node1N.src = "Images/PC.gif";
                            node1N.title ="Nodo1 PARADO, Nodo2 PARADO. No es posible conmutar";
                            node2N.src = "Images/PC.gif";
                            node2N.title ="Nodo1 PARADO, Nodo2 PARADO. No es posible conmutar";
                        }
                        else 
                            if (nodeInfo2.State == "Activando Nodo" && nodeInfo.State == "No Funcional")
                            {
                                node1N.src = "Images/PC.gif";
                                node2N.src = "Images/PC_Red.gif";  
                                node2N.title = "ERROR EN EL CLUSTER. REINICIAR NODO 1"                             
                                node1N.title = "ERROR EN EL CLUSTER. REINICIAR NODO 1"   
                                document.getElementById("Errores").innerHTML ="ERROR EN EL CLUSTER. Si persiste reiniciar los nodos";                          
                            }
                            else 
                                if (nodeInfo.State == "Activando Nodo" && nodeInfo2.State == "No Funcional")
                                {
                                    node2N.src = "Images/PC.gif";
                                    node1N.src = "Images/PC_Red.gif";    
                                    node1N.title = "ERROR EN EL CLUSTER. REINICIAR NODO 2"                             
                                    node2N.title = "ERROR EN EL CLUSTER. REINICIAR NODO 2"    
                                    document.getElementById("Errores").innerHTML ="ERROR EN EL CLUSTER. Si persiste reiniciar los nodos";                                                                                           
                                }  
                                else
                                    if (nodeInfo2.State == "Activando Nodo" || nodeInfo.State == "Activando Nodo")
                                    {
                                        node1N.src = "Images/PC_Blue.gif";
                                        node2N.src = "Images/PC_Blue.gif";                          
                                    }  
                    

         document.getElementById(node + "Name").innerHTML = nodeInfo.Name; 
         document.getElementById(node + "State").innerHTML = "Estado: " + nodeInfo.State; 
         
         document.getElementById(node2 + "Name").innerHTML = nodeInfo2.Name; 
         document.getElementById(node2 + "State").innerHTML = "Estado: " + nodeInfo2.State; 

         
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
         
         
         if (nodeInfo2.State == "No Funcional")
         {
            document.getElementById(node2 + "Adapter1").innerHTML = ""; 
            document.getElementById(node2 + "Adapter2").innerHTML = "";
            document.getElementById(node2 + "Begin").innerHTML = "";
         }
         else
         {
            document.getElementById(node2 + "Adapter1").innerHTML = "Adaptador1: " + nodeInfo2.Adapter1; 
            document.getElementById(node2 + "Adapter2").innerHTML = "Adaptador2: " + nodeInfo2.Adapter2;
            document.getElementById(node2 + "Begin").innerHTML = "Desde: " + nodeInfo2.StateBegin;
         }  
                
         if (nodeInfo.State == "Activando Nodo" || nodeInfo2.State == "Activando Nodo")          
         {
            document.getElementById("VirtualIP1").innerHTML = "";
            document.getElementById("VirtualIP2").innerHTML = "";
         }
         else
             if (nodeInfo.State == "No Funcional" && nodeInfo2.State == "No Funcional")
             {
                document.getElementById("VirtualIP1").innerHTML = "";
                document.getElementById("VirtualIP2").innerHTML = "";         
             }
             else
             {
                if (nodeInfo.State == "Activo")
                {
                    document.getElementById("VirtualIP1").innerHTML = nodeInfo.VirtualIp1;
                    document.getElementById("VirtualIP2").innerHTML = nodeInfo.VirtualIp2;     
                }
                else
                    if (nodeInfo2.State == "Activo")
                    {
                        document.getElementById("VirtualIP1").innerHTML = nodeInfo2.VirtualIp1;
                        document.getElementById("VirtualIP2").innerHTML = nodeInfo2.VirtualIp2;   
                    }
            }
      }  
      
      function HandleClick(node)
      {         
         if ((node == "Node1") && (node1State == "No Activo"))
         {
             var conmutar = confirm("¿Esta seguro que quiere conmutar de nodo Activo?")
             if ( conmutar ) CallServer("ActivateNode1", "");
         }
         else if ((node == "Node2") && (node2State == "No Activo"))
         {
             var conmutar = confirm("¿Esta seguro que quiere conmutar de nodo Activo?")
             if ( conmutar ) CallServer("ActivateNode2", "");
         }
      }
       
   </script>

</head>
<body bgcolor="#FFFFFF" text="#000000" style="font-size: 11px; font-family: Arial; border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none;">
   <form id="frmCallback" name="frmCallback" runat="server">
       <div id="VirtualIP2" style="position: absolute; left: 240px; top: 335px; width: 259px;
         height: 14px; z-index: 105; font-weight: bold; font-size: 12px; text-align: center;" align="left">
       </div>
       <div id="VirtualIP1" style="position: absolute; left: 240px; top: 311px; width: 259px;
        height: 14px; z-index: 106; font-weight: bold; font-size: 12px; text-align: center;" align="left">
      </div>
      <div id="Node1Name" style="position: absolute; left: 244px; top: 102px; width: 114px;
         height: 14px; z-index: 100; font-weight: bold; font-size: 12px; text-align: center;" align="left">
         Nodo no operativo</div>
      <div id="Node1State" style="position: absolute; left: 87px; top: 152px; width: 150px;
         height: 14px; z-index: 101" align="left">
         Estado: No Funcional</div>
      <div id="Node1Begin" style="position: absolute; left: 87px; top: 172px; width: 150px;
         height: 14px; z-index: 102" align="left">
      </div>
      <div id="Node1Adapter1" style="position: absolute; left: 87px; top: 192px; width: 150px;
         height: 14px; z-index: 103" align="left">
      </div>
      <div id="Node1Adapter2" style="position: absolute; left: 87px; top: 212px; width: 150px;
         height: 14px; z-index: 104" align="left">
      </div>
      <div id="Node2Name" style="position: absolute; left: 383px; top: 102px; width: 114px;
         height: 14px; z-index: 107; font-weight: bold; font-size: 12px; text-align: center;" align="left">
         Nodo no operativo</div>
      <div id="Node2State" style="position: absolute; left: 506px; top: 151px; width: 150px;
         height: 14px; z-index: 108" align="left">
         Estado: No Funcional</div>
      <div id="Node2Begin" style="position: absolute; left: 506px; top: 171px; width: 150px;
         height: 14px; z-index: 109" align="left">
      </div>
      <div id="Node2Adapter1" style="position: absolute; left: 506px; top: 191px; width: 150px;
         height: 14px; z-index: 110" align="left">
      </div>
      <div id="Node2Adapter2" style="position: absolute; left: 506px; top: 211px; width: 150px;
         height: 14px; z-index: 111" align="left">
      </div>
      <div id="Trace" style="position: absolute; left: 618px; top: 314px; width: 150px;
         height: 14px; z-index: 112" align="left">
         <asp:HyperLink ID="HyperLink1" runat="server" EnableViewState="False" NavigateUrl="~/Trace.axd">TraceInfo</asp:HyperLink>
      </div>
       &nbsp;
       &nbsp;&nbsp;&nbsp;&nbsp;
       <div style="z-index: 113; left: 246px; width: 113px; border-top-style: none; border-right-style: none;
           border-left-style: none; position: absolute; top: 120px; height: 145px; border-bottom-style: none">
           <img id="Node1Img" onClick="HandleClick('Node1')" src="Images/PC.GIF" style="cursor:hand;z-index: 100; left: 0px; width: 108px;
               position: absolute; top: 0px; height: 142px" />
           &nbsp;
       <div style="z-index: 116; left: 139px; width: 113px; border-top-style: none; border-right-style: none;
           border-left-style: none; position: absolute; top: -1px; height: 145px; border-bottom-style: none">
           <img id="Node2Img" onClick="HandleClick('Node2')" src="Images/PC.GIF" style="cursor:hand;z-index: 100; left: 0px; width: 108px;
               position: absolute; top: 0px; height: 142px" />
           </div>
       </div>
       <hr style="z-index: 114; border-left-color: black; left: 0px; border-bottom-color: black;
           border-top-style: solid; border-top-color: black; border-right-style: solid;
           border-left-style: solid; position: absolute; top: 77px; border-right-color: black;
           border-bottom-style: solid; width: 884px;" />
       <hr style="z-index: 115; border-left-color: black; left: 1px; border-bottom-color: black;
           border-top-style: solid; border-top-color: black; border-right-style: solid;
           border-left-style: solid; position: absolute; top: 299px; border-right-color: black;
           border-bottom-style: solid; width: 886px;" />
       <div style="font-weight: bold; font-size: 40px; z-index: 116; left: 244px; width: 253px;
           position: absolute; top: 14px; height: 49px; text-align: center">
           CLUSTER</div>
       <div style="z-index: 117; left: 10px; width: 27px; position: absolute; top: 419px;
           height: 23px">
           <img src="Images/Green.GIF" style="z-index: 100; left: 1px; width: 18px; position: absolute;
               top: 3px; height: 19px" />
       </div>
       <div style="z-index: 118; left: 10px; width: 28px; position: absolute; top: 449px;
           height: 22px">
           <img src="Images/Blue.GIF" style="z-index: 100; left: 2px; width: 18px; top: 0px;
               height: 19px" /></div>
       <div style="z-index: 119; left: 10px; width: 27px; position: absolute; top: 480px;
           height: 16px">
           <img src="Images/Orange.GIF" style="z-index: 100; left: 0px; width: 18px; position: absolute;
               top: -2px; height: 19px" />
       </div>
       <div style="z-index: 120; left: 10px; width: 26px; position: absolute; top: 506px;
           height: 22px">
           <img src="Images/Gray.GIF" style="z-index: 100; left: 0px; width: 18px; position: absolute;
               top: 0px; height: 19px" />
       </div>
       <div style="z-index: 121; left: 44px; width: 223px; position: absolute; top: 423px;
           height: 17px">
           Nodo Activo y nodo remoto listo y a la espera
           <div style="z-index: 124; left: 0px; width: 223px; position: absolute; top: 29px;
               height: 17px">
               Nodo listo y a la espera y nodo remoto Activo</div>
       </div>
       <div style="z-index: 122; left: 44px; width: 223px; position: absolute; top: 480px;
           height: 17px">
           Nodo Activo y nodo remoto parado</div>
       <div style="z-index: 124; left: 45px; width: 223px; position: absolute; top: 507px;
           height: 17px">
           Nodo parado</div>
       <div id="Errores" style="font-weight: bold; z-index: 125; left: 120px; width: 488px;
           color: red; position: absolute; top: 363px; height: 16px; text-align: center">
       </div>
   </form>
</body>
</html>
