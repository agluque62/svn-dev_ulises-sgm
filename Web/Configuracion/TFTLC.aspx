<%@ Page Language="C#" CodeFile="TFTLC.aspx.cs" MasterPageFile="~/CD40.master" Inherits="TFTLC" 
	CodeFileBaseClass="PageBaseCD40.PageCD40"	Title="Panel Linea Caliente" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--     <script type="text/javascript">
        if (window.name != "<%=GetWindowName()%>") {
            window.name = "invalidAccess";
            window.open("../BloqueoAplicacion.aspx", "_self");
        }
    </script>
--%>     
    <asp:Label ID="Label6" runat="server" Text="LÍNEA CALIENTE" 
		 CssClass="labelPagina" meta:resourcekey="Label6Resource1"></asp:Label>        
    <asp:ListBox ID="LBoxDestinos" runat="server" Height="331px" Style="z-index: 101; left: 259px;
        position: absolute; top: 305px" Width="195px" Rows="100" 
		 meta:resourcekey="LBoxDestinosResource1"></asp:ListBox>
    <asp:Label ID="Label1" runat="server" Style="z-index: 102; left: 265px; position: absolute;
        top: 278px" Text="Destinos disponibles" meta:resourcekey="Label1Resource1"></asp:Label>    
 
   <asp:Table ID="TEnlacesLC" runat="server" Height="80px" Style="left: 41px;
        position: absolute; top: 74px; z-index: 99;" BackColor="Transparent" BorderStyle="Solid" BorderColor="#eeb44f">
       <asp:TableRow ID="TableRow1" runat="server" BackColor="Transparent">
           <asp:TableCell ID="TableCell10" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button10" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell11" runat="server">
               <asp:Button ID="Button11" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell12" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button12" runat="server"
                   SkinId="BotonPanelLc" 
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell13" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button13" runat="server"
                   SkinId="BotonPanelLc" 
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell14" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button14" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell15" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button15" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell16" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button16" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell17" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button17" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell18" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button18" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
       </asp:TableRow>
       <asp:TableRow ID="TableRow2" runat="server" BackColor="Transparent" Visible="true">
           <asp:TableCell ID="TableCell1" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button1" runat="server"
                   SkinId="BotonPanelLc" 
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell2" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button2" runat="server"
                   SkinId="BotonPanelLc" 
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell3" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button3" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell4" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button4" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell5" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button5" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell6" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button6" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell7" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button7" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell8" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button8" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
           <asp:TableCell ID="TableCell9" runat="server" BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
               <asp:Button ID="Button9" runat="server"
                   SkinId="BotonPanelLc"
                   OnClick="CeldaEnlaceLineaCaliente_OnClick" />

           </asp:TableCell>
       </asp:TableRow>
   </asp:Table> 

   <asp:Panel ID="PanelNoPermissions" runat="server" ForeColor="#00C000" 
		 Style="z-index: -1; left: 100px; position: absolute; top: 60px; height: 60px; width: 797px;" 
		 meta:resourcekey="PanelNoPermissions" Visible="false">
         <asp:Label ID="LblPermisos" runat="server" ForeColor="Red" Style="z-index: 100; left: 10px; width:auto; position : absolute; top: 153px" 
            Text="No tiene permisos para ver esta sección."
            meta:resourcekey="LblPermisosResource1">
         </asp:Label>
     </asp:Panel>

    <asp:LinkButton ID="BtTelefonia" runat="server" SkinId="LinkButtonCabecera" Style="z-index: 104; left: 679px; position: absolute;
        top: 305px; height: 19px; width: 61px;" Text="Panel Telefonía" 
		 OnClick="BtTelefonia_Click" meta:resourcekey="BtTelefoniaResource1" />
    <asp:LinkButton ID="BtRadio" runat="server" SkinId="LinkButtonCabecera" Style="z-index: 105; left: 591px; position: absolute;
        top: 305px; height: 19px; width: 45px;" Text="Panel Radio" 
		 OnClick="BtRadio_Click" meta:resourcekey="BtRadioResource1" />

    <asp:LinkButton ID="BtSector" runat="server" Style="z-index: 103; left: 580px; position: absolute;
        top: 430px; width: 180px; height: 35px; background: url('../Configuracion/Images/arrowback.gif') no-repeat left top; 
        padding-left:18px;margin-left: 10px;padding-right:10px;" SkinId="LinkButtonCabecera" 
        Visible="True" 
		OnClick="BtSector_Click" CausesValidation="False"  
		  meta:resourcekey="BtVolverSectorRes1" >
        <asp:Label ID="EnlaceVolverSector" runat="server" Style="display:table;margin:10px auto; vertical-align: middle;"
                Text=" Volver al Sector" Visible="True" meta:resourcekey="EnlaceVolverSectorResource1"></asp:Label>
     </asp:LinkButton>
        
    <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Groove"
        BorderWidth="1px" Height="126px" Style="z-index: 106; left: 332px; position: absolute;
        top: 112px" Visible="False" Width="93px" 
		 meta:resourcekey="Panel1Resource1">
        <asp:Button ID="BtLiberar" runat="server" Style="z-index: 100; left: 7px; position: absolute;
            top: 37px" Text="Liberar" Width="80px" OnClick="BtLiberar_Click" 
			  meta:resourcekey="BtLiberarResource1" />
        <asp:Button ID="BtCancelar" runat="server" Style="z-index: 101; left: 7px; position: absolute;
            top: 93px" Text="Cancelar" Width="80px" OnClick="BtCancelar_Click" 
			  meta:resourcekey="BtCancelarResource1" />
        <asp:Button ID="BtAsignar" runat="server" Style="z-index: 103; left: 7px; position: absolute;
            top: 9px" Text="Asignar" Width="80px" OnClick="BtAsignar_Click" 
			  meta:resourcekey="BtAsignarResource1" />
        <asp:Button ID="BModificar" runat="server" Style="z-index: 104; left: 7px; position: absolute;
            top: 65px" Text="Modificar" Width="80px" OnClick="BtModificar_Click" 
			  meta:resourcekey="BModificarResource1" />
    </asp:Panel>
    
    <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Groove"
        BorderWidth="1px" Height="260px" Style="z-index: 107; left: 225px; position: absolute;
        top: 72px" Visible="False" Width="350px" 
		 meta:resourcekey="Panel2Resource1">

        <asp:Label ID="Label2" runat="server" Style="z-index: 102; left: 25px; position: absolute;
            top: 23px" Text="Literal:" meta:resourcekey="Label2Resource1"></asp:Label>
        <asp:TextBox ID="TBoxLiteral" runat="server" MaxLength="32" Style="z-index: 101; left: 25px;
            position: absolute; top: 43px" Width="144px" enabled="false"
			  meta:resourcekey="TBoxLiteralResource1">
        </asp:TextBox>

        <asp:Label ID="LbDestinoLCEN" runat="server"  Style="z-index: 107; left: 25px; position: absolute;top: 143px"
             Text="Destino LCEN:" meta:resourcekey="LbDestinoLCENResource1"></asp:Label>
        
        <asp:DropDownList ID="DListDestinosLCEN" runat="server" Style="z-index: 105; left: 25px;
            position: absolute; top: 165px" Width="190px"  class="select"
                    AppendDataBoundItems="True" Enabled="True" 
                    meta:resourcekey="DListDestinosLCENResource1">
        </asp:DropDownList>

        <asp:Label ID="Label4" runat="server" Style="z-index: 107; left: 25px; position: absolute;
            top: 83px" Text="Prioridad SIP:" meta:resourcekey="Label4Resource1" Visible="false"></asp:Label>
        <asp:DropDownList ID="DListPrioridadSIP" runat="server" Style="z-index: 105; left: 25px;
            position: absolute; top: 103px" Width="115px"  class="select"
			  meta:resourcekey="DListPrioridadSIPResource1" Visible="false">
            <asp:ListItem Value="4" meta:resourcekey="ListItemResource1">No urgente</asp:ListItem>
			  <asp:ListItem Value="3" meta:resourcekey="ListItemResource2">Normal</asp:ListItem>
			  <asp:ListItem Value="2" meta:resourcekey="ListItemResource3">Urgente</asp:ListItem>
			  <asp:ListItem Value="1" meta:resourcekey="ListItemResource4">Emergencia</asp:ListItem>
        </asp:DropDownList>

        <asp:Label ID="Label5" runat="server" Style="z-index: 108; left: 25px; position: absolute;
            top: 83px" Text="Prioridad de descarte en Sectorización:" meta:resourcekey="Label5Resource1"></asp:Label>
        <asp:DropDownList ID="DListPrioridadTecla" runat="server" Style="z-index: 106; left: 25px;
            position: absolute; top: 103px" Width="275px"  class="select"
			  meta:resourcekey="DListPrioridadTeclaResource1">
            <asp:ListItem Value="1" meta:resourcekey="ListItemResource5">No descartable</asp:ListItem>
            <asp:ListItem Value="2" meta:resourcekey="ListItemResource6">Alta</asp:ListItem>
            <asp:ListItem Value="3" meta:resourcekey="ListItemResource7">Media</asp:ListItem>
            <asp:ListItem Value="4" meta:resourcekey="ListItemResource8">Baja</asp:ListItem>
        </asp:DropDownList>

        <asp:Button ID="BtAceptar" runat="server" Style="z-index: 100; left: 75px; position: absolute;
            top: 215px" Text="Aceptar" Width="80px" OnClick="BtAceptar_Click" 
			  meta:resourcekey="BtAceptarResource1" UseSubmitBehavior="true" />
        <asp:Button ID="BCancelar" runat="server" Style="z-index: 115; left: 191px; position: absolute;
			 top: 215px" Text="Cancelar" Width="80px" OnClick="BCancelar_Click" 
			  meta:resourcekey="BCancelarResource1" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TBoxLiteral"
            ErrorMessage="*" 
			  Style="z-index: 110; left: 180px; position: absolute; top: 45px" 
			  meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
    </asp:Panel>

</asp:Content>
