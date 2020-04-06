<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" 
 StylesheetTheme="TemaPaginasConfiguracion" Theme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
      <link rel="shortcut icon" href="~/favicon.ico">
<%--    <title>Gestión y Mantenimiento ULISES-5000</title>
    <style type="text/css">
        #divForm
        {
            width: 282px;
        }
        #form1
        {
            height: 438px;
        }
    </style>
--%>
</head>

<body >
<%--        <script language="javascript" type="text/javascript">
            if (window.name != "<%=GetWindowName()%>") {
                if ("" == "<%=GetWindowName()%>") {
                    window.name = "<%=SetWindowName()%>";
                }
                else {
                    window.name = "invalidAccess";
                    window.open("BloqueoAplicacion.aspx", "_self");
                }
            }
        </script>
--%>    
<%--<div class='img' id='Div1'>
 <div class='mask'></div>
 <img src="Images/Logotipo_Duro_Nucleo.gif" alt="" />
</div>
<div class='img' id='img-2'>
 <div class='mask'></div>
 <img src="Images/Logotipo_Duro_Nucleo.gif" alt="" />
</div>
<div class='img' id='img-3'>
 <div class='mask' id='mask-1'></div>
 <div class='mask' id='mask-2'></div>
 <img src="Images/Logotipo_Duro_Nucleo.gif" alt=""/>
</div>
<div class='img' id='img-4'>
 <div class='mask'></div>
 <img src="Images/Logotipo_Duro_Nucleo.gif" alt="" />
</div>
<div class='img' id='img-5'>
 <div class='mask'></div>
 <img src="Images/Logotipo_Duro_Nucleo.gif" alt="" />
</div>
<div class='img' id='img-6'>
 <div class='mask'></div>
 <img src="Images/Logotipo_Duro_Nucleo.gif" alt="" />
</div>
        --%>
    <form id="form1" runat="server" class="estiloDiv gradpositivo white">
        <div id="divForm" runat="server" style="position:relative; left:50px; top:50px; width:1000px; height:800px; ">
            <asp:Table ID="Table1" runat="server" Width="1000px" Height="800px">
                <asp:TableRow ID="row1" runat="server">
                    <asp:TableCell ID="cell11" runat="server"></asp:TableCell>
                    <asp:TableCell ID="cell12" runat="server" Width="282px" HorizontalAlign="Center">
                        <asp:LinkButton ID="LBPpalMantenimientoCluster" runat="server" 
			                  OnClick="LBPpalMantenimientoCluster_Click" 
			                  Width="258px" CssClass="reflection"
			                  meta:resourcekey="LBPpalMantenimientoClusterResource1">Mantenimiento Cluster
			            </asp:LinkButton>
                    </asp:TableCell>
                    <asp:TableCell ID="cell13" runat="server"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="row2" runat="server" Height="122px">
                    <asp:TableCell ID="cell21" runat="server" VerticalAlign="Middle"  HorizontalAlign="Left" >
                        <asp:LinkButton ID="LBPpalConfiguracion" runat="server" 
			                  OnClick="LBPpalConfiguracion_Click" 
			                  Width="158px" CssClass="reflection"
			                  meta:resourcekey="LBPpalConfiguracionResource1">Configuración
			            </asp:LinkButton>
                    </asp:TableCell>
                    <asp:TableCell ID="cell22" runat="server" VerticalAlign="Middle" Width="282px" HorizontalAlign="Center">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/LogoNucleo290x77.png"
			                  style="height: inherit; width:inherit " meta:resourcekey="Image1Resource1"/>
                    </asp:TableCell>
                    <asp:TableCell ID="cell23" runat="server" VerticalAlign="Middle" HorizontalAlign="Right">
                        <asp:LinkButton ID="LBPpalMantenimiento" runat="server"
                            OnClick="LBPpalMantenimiento_Click"
                            Width="158px" CssClass="reflection"
                            meta:resourcekey="LBPpalMantenimientoResource1">Mantenimiento
                        </asp:LinkButton>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="row3" runat="server">
                    <asp:TableCell ID="cell32" runat="server">
                    </asp:TableCell>
                    <asp:TableCell ID="cell31" runat="server" Width="282px" HorizontalAlign="Center">
                        <asp:LinkButton ID="LBWizardConfiguracion" runat="server"
                            Enabled="true" 
                            OnClick="LBWizard_Click"
                            Width="258px" CssClass="reflection"
                            meta:resourcekey="LBWizardConfiguracionResource1">Asistente de configuración
                        </asp:LinkButton>
                    </asp:TableCell>
<%--                    <asp:TableCell ID="cell33" runat="server" Width="282px" HorizontalAlign="Left">
                        <asp:LinkButton ID="LBEstadisticas" runat="server"
                            OnClick="LBEstadisticas_Click"
                            Width="258px" CssClass="reflection"
                            meta:resourcekey="LBEstadisticasResource1">Estadisticas
                        </asp:LinkButton>
                    </asp:TableCell>--%>
                </asp:TableRow>
            </asp:Table>
    </div>
        <div style="z-index: 301; left: 15px; width: 1260px; position: absolute; top: 875px; height: 37px; text-align:center; color: black; font-family:Arial; font-size:small" >
            <br\ />
            &#169 Núcleo. 2020. v2.6.0. All rights reserved.
        </div>
    </form>
</body>
</html>
