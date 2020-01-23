<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="LoginCD40" StylesheetTheme="TemaPaginasConfiguracion" 
Theme="TemaPaginasConfiguracion" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<%--    <title>Control de acceso</title>
	 <style type="text/css">
		 #form1
		 {
			 height: 475px;
		 }
	</style>--%>
</head>
<body>
    <form id="form1" runat="server" class="estiloDiv gradpositivo white">
        <div runat="server">
    	<asp:Panel ID="Panel3" runat="server" BackColor="#F7F6F3" BorderColor="#FF3300" 
			 BorderStyle="Solid" BorderWidth="5px" 
			 Font-Names="Verdana" style="z-index: 3; left: 82px; top: 232px; position: absolute; height: 206px; width: 1039px" 
			 Visible="False" meta:resourcekey="Panel3Resource1">
			<asp:Label ID="Label3" runat="server" BackColor="#F7F6F3"
				style="position: absolute; z-index: 1; left: 214px; top: 86px; height: 64px; width: 691px;" 
				meta:resourcekey="Label3Resource1">
			</asp:Label>
			<asp:Panel ID="Panel2" runat="server" BackColor="#e00034" 
				style="z-index: 2; left: 11px; top: 6px; position: absolute; height: 47px; width: 1014px" 
				meta:resourcekey="Panel2Resource1">
				<asp:Label ID="Label2" runat="server" SkinId="LabelPanel"
					style="z-index: 1; left: 373px; top: 13px; position: absolute" 
					Text="Sesión terminada por inactividad" meta:resourcekey="Label2Resource1"></asp:Label>
			</asp:Panel>
			<asp:Table runat="server" Height="205px" Width="1040px" 
				meta:resourcekey="TableResource1">
				<asp:TableRow ID="TableRow1" runat="server" 
					meta:resourcekey="TableRow1Resource1">
					<asp:TableCell ID="TableCell1" runat="server" ColumnSpan="9" 
						meta:resourcekey="TableCell1Resource1"></asp:TableCell>
				</asp:TableRow>
				<asp:TableRow ID="TableRow2" runat="server" 
					meta:resourcekey="TableRow2Resource1">
					<asp:TableCell ID="TableCell2" runat="server" Width="104px" 
						meta:resourcekey="TableCell2Resource1"></asp:TableCell>
					<asp:TableCell ID="TableCell11" runat="server" Width="104px" 
						meta:resourcekey="TableCell11Resource1"></asp:TableCell>
					<asp:TableCell ID="TableCell3" runat="server" Width="104px" 
						meta:resourcekey="TableCell3Resource1"></asp:TableCell>
					<asp:TableCell ID="TableCell4" runat="server" Width="104px" 
						meta:resourcekey="TableCell4Resource1"></asp:TableCell>
					<asp:TableCell ID="TableCell5" runat="server" Width="104px" 
						meta:resourcekey="TableCell5Resource1"></asp:TableCell>
					<asp:TableCell ID="TableCell6" runat="server" Width="104px" 
						meta:resourcekey="TableCell6Resource1"></asp:TableCell>
					<asp:TableCell ID="TableCell7" runat="server" Width="104px" 
						meta:resourcekey="TableCell7Resource1"></asp:TableCell>
					<asp:TableCell ID="TableCell8" runat="server" Width="104px" 
						meta:resourcekey="TableCell8Resource1"></asp:TableCell>
					<asp:TableCell ID="TableCell9" runat="server" Width="104px" 
						meta:resourcekey="TableCell9Resource1"></asp:TableCell>
					<asp:TableCell ID="TableCell10" runat="server" HorizontalAlign="Left">
						<asp:LinkButton ID="LinkButton2" 
						runat="server" PostBackUrl="Login.aspx" meta:resourcekey="LinkButton2Resource1">
						</asp:LinkButton>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		 </asp:Panel>
		</div>
		 
        <div id="divCenter" style="position:absolute;left:50%; top:50%;  margin-left:-131px; margin-top:-70px;">
		 <asp:Login ID="Login1" runat="server" BackColor="Transparent"
			BorderColor="#E6E2D8" BorderPadding="4"
			 BorderStyle="Solid" BorderWidth="1px" DestinationPageUrl="~/Default.aspx" DisplayRememberMe="False"
			 FailureText="Usuario incorrecto.Por favor, inténtelo de nuevo." Font-Names="Verdana"
			 Font-Size="0.8em" ForeColor="#333333" Height="140px" LoginButtonText="Aceptar" 
			 OnAuthenticate="Login1_Authenticate" PasswordLabelText="Clave:" PasswordRequiredErrorMessage="Debe introducir una clave." 
			 RememberMeText="" TextBoxStyle-Font-Names="Verdana" LabelStyle-Font-Names="Verdana" TitleTextStyle-Font-Names="Verdana" 
			 TitleText="Login" UserNameLabelText="Usuario:" UserNameRequiredErrorMessage="Debe introducir un usuario." Width="262px" meta:resourcekey="Login1Resource1">
			 <TitleTextStyle BackColor="#e00034" Font-Bold="True" Font-Size="0.9em" ForeColor="#E5E5E5"  />
			 <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
			 <TextBoxStyle Font-Size="0.8em" BackColor="#FFCC99" CssClass="textbox" />
			 <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
				 Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
		 </asp:Login>
		</div>   
			
		<asp:Panel ID="Panel1" runat="server" Visible="False"
			 BackColor="#F7F6F3" BorderColor="#e00034"
			 style="position: relative; top: 247px; left: 310px; width: 676px; height: 169px; z-index: 103;" 
			BorderStyle="Solid" meta:resourcekey="Panel1Resource1" >
				 <asp:Label ID="Label1" runat="server" SkinId="LabelPanel"
					style="z-index: 1; left: 21px; top: 13px; position: absolute; height: 13px; width: 640px; text-align:center"
					 Text="Las últimas modificaciones no fueron activadas en el sistema. ¿Qué desea hacer?"  meta:resourcekey="Label1Resource1"></asp:Label>
			<asp:Panel ID="PnlCabecera" runat="server" 
				 style="position: relative; top: 5px; left: 5px; width: 670px; height: 29px" 
				  SkinId="PanelCabecera" meta:resourcekey="PnlCabeceraResource1">
			</asp:Panel>
    		<asp:Panel ID="PanelContenido" runat="server" 
				 
				style="position: relative; top: 5px; left: 5px; width: 670px; height: 131px; z-index: -1;" 
				meta:resourcekey="PanelContenidoResource1">
				 <asp:RadioButton ID="RBContinuarActiva" runat="server" 
					style="position: absolute; z-index: 1; left: 230px; top: 19px; width: auto; height: 24px;" 
					Text="Continuar con la última activa" Checked="True" GroupName="Modo" 
					 meta:resourcekey="RBContinuarActivaResource1" />
				 <asp:RadioButton ID="RBContinuarModificada" runat="server" 
					style="position: absolute; z-index: 2; left: 230px; top: 49px; height: 22px; width: auto;" 
					Text="Continuar con la últimas modificaciones" GroupName="Modo" 
					 meta:resourcekey="RBContinuarModificadaResource1" />
				 <asp:Button ID="BtnAceptarModoContinuar" runat="server" CssClass="botonCD40" 
					style="z-index: 2; left: 230px; top: 90px; position: absolute" Text="Aceptar" 
					 onclick="OnBtnAceptarModoContinuar" 
					 meta:resourcekey="BtnAceptarModoContinuarResource1" />
				 <asp:LinkButton ID="LinkButton1" runat="server" 
					 style="position: absolute; top: 90px; left: 463px; height: 20px;" 
					 onclick="LinkButton1_Click" meta:resourcekey="LinkButton1Resource1">Cancelar</asp:LinkButton>
			</asp:Panel>
		</asp:Panel>
    </form>
</body>
</html>
