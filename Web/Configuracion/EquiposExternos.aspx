<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="EquiposExternos.aspx.cs" StylesheetTheme="TemaPaginasConfiguracion"
CodeFileBaseClass="PageBaseCD40.PageCD40" Inherits="Configuracion_EquiposExternos" Title="Gestión de Equipos Externos" EnableEventValidation="false" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--	 <script type="text/javascript">
	     if ("<%=GetWindowName()%>" != "Wizard") {
	         if (window.name != "<%=GetWindowName()%>") {
	             window.name = "invalidAccess";
	             window.open("../BloqueoAplicacion.aspx", "_self");
	         }
	     }
	</script>
--%>	
	<asp:Label ID="Label6" runat="server" Text="GESTIÓN DE EQUIPOS EXTERNOS" 
		CssClass="labelPagina" meta:resourcekey="Label6Resource1"/>
	<asp:ListBox ID="ListBox1" runat="server" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"
		SkinID="MascaraListaElementos" AutoPostBack="True" 
		meta:resourcekey="ListBox1Resource1"></asp:ListBox>

    <asp:Panel ID="Panel1" runat="server" Height="320px" Style="z-index: 109; left: 205px; position: absolute; top: 60px"
        Width="305px" BorderStyle="Inset">

        <asp:Label ID="Label5" runat="server" Style="z-index: 111; left: 14px; position: absolute; top: 22px"
            Text="Identificador:" Visible="False" meta:resourcekey="Label5Resource1"></asp:Label>
        <asp:TextBox ID="TBIdEquipo" runat="server" MaxLength="32" ReadOnly="True" Style="z-index: 112; left: 14px; position: absolute; top: 42px"
            Visible="False" meta:resourcekey="TBIdEquipoResource1"></asp:TextBox>

        <asp:Label ID="LblTipoEquipo" runat="server" Style="z-index: 119; left: 14px; position: absolute; top: 87px; width:auto"
            Text="Tipo:" Visible="False" meta:resourcekey="LblTipoEquipoResource1"></asp:Label>
        <asp:DropDownList ID="DDLTipoEquipo" runat="server" Enabled="False" Style="z-index: 117; left: 14px; position: absolute; top: 107px" AutoPostBack="true"
            Visible="False" Width="118px" Height="22px" meta:resourcekey="DDLTipoEquipoResource1" class="select" OnTextChanged="DDLTipoEquipo_SelectedIndexChanged">
            <asp:ListItem Value="2" meta:resourcekey="ListItemResource1">Radio</asp:ListItem>
            <asp:ListItem Value="3" meta:resourcekey="ListItemResource2">Telefon&#237;a</asp:ListItem>
            <asp:ListItem Value="5" meta:resourcekey="ListItemResource3">Grabador</asp:ListItem>
        </asp:DropDownList>

        <asp:Label ID="Label3" runat="server" Style="z-index: 105; left: 14px; position: absolute; top: 152px; width:auto"
            Text="IP de la Red 1:" Visible="False" meta:resourcekey="Label3Resource1"></asp:Label>
        <asp:TextBox ID="TxtIP1" runat="server" ReadOnly="True" Style="z-index: 102; left: 14px; position: absolute; top: 172px"
            Visible="False" Width="131px" meta:resourcekey="TxtIP1Resource1"></asp:TextBox>

        <asp:Label ID="Label4" runat="server" Style="z-index: 104; left: 14px; position: absolute; top: 207px"
            Text="IP de la Red 2:" Visible="False" meta:resourcekey="Label4Resource1"></asp:Label>
        <asp:TextBox ID="TxtIP2" runat="server" ReadOnly="True" Style="z-index: 103; left: 14px; position: absolute; top: 227px"
            Visible="False" Width="131px"
            meta:resourcekey="TxtIP2Resource1"></asp:TextBox>

        <asp:Label ID="LblSipPort" runat="server" Style="z-index: 106; left: 14px; position: absolute; top: 257px"
            Text="Puerto SIP:" Visible="False" meta:resourcekey="LblSipPortResource1"></asp:Label>
        <asp:TextBox ID="TBSipPort" runat="server" ReadOnly="True" Style="z-index: 103; left: 14px; position: absolute; top: 277px"
            Visible="False" Width="51px"></asp:TextBox>

        <asp:Button ID="BtAceptar" runat="server" OnClick="BtAceptar_Click" Style="z-index: 109; left: 168px; position: absolute; top: 277px"
            Text="Aceptar" Visible="False"
            UseSubmitBehavior="true" Width="100px" meta:resourcekey="BtAceptarResource1" />
        <ajaxToolkit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender"
            runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
        </ajaxToolkit:ConfirmButtonExtender>

        <asp:Label ID="LblIp1Existente" runat="server" Visible="False"
            Text="La dirección IP Red 1 ya existe en el sistema." ForeColor="Red"
            Style="position: absolute; top: 172px; left: 193px; width: auto;"
            meta:resourcekey="LblIp1ExistenteResource1"></asp:Label>
        <asp:Label ID="LblIp2Existente" runat="server" Visible="False"
            Text="La dirección IP Red 2 ya existe en el sistema." ForeColor="Red"
            Style="position: absolute; top: 227px; left: 193px; width: auto;"
            meta:resourcekey="LblIp2ExistenteResource1"></asp:Label>

        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtIP1"
            ErrorMessage="Formato incorrecto de la IP de la Red 1." Style="z-index: 113; left: 162px; position: absolute; top: 175px"
            ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"
            Visible="False" meta:resourcekey="RegularExpressionValidator1Resource1">*</asp:RegularExpressionValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtIP2"
            ErrorMessage="Formato incorrecto de la IP de la Red 2." Style="z-index: 113; left: 162px; position: absolute; top: 230px"
            ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"
            Visible="False" meta:resourcekey="RegularExpressionValidator2Resource1">*</asp:RegularExpressionValidator>
		<asp:RangeValidator ID="RangeValidatorSIPLocal" runat="server" ControlToValidate="TBSipPort"
			ErrorMessage="El campo puerto SIP deber ser numérico y mayor de 1024."
			MaximumValue="65535" MinimumValue="1025" Style="z-index: 131; left: 123px; position: absolute;
			top: 277px" Type="Integer" meta:resourcekey="RangeValidatorSIPLocalResource1">*</asp:RangeValidator>
		<asp:RequiredFieldValidator ID="RequiredFieldSIP" runat="server" ControlToValidate="TBSipPort"
			ErrorMessage="El puerto SIP no puede estar vacio." Style="z-index: 143;
			left: 80px; position: absolute; top: 280px" 
			meta:resourcekey="RequiredFieldSIPResource1">*</asp:RequiredFieldValidator>
    </asp:Panel>

        <asp:RequiredFieldValidator ID="RequiredFieldIP1" runat="server" ControlToValidate="TxtIP1"
            ErrorMessage="El campo IP de la Red 1 no puede estar vacio." Style="z-index: 122; left: 370px; position: absolute; top: 238px"            
            meta:resourcekey="RequiredFieldIP1Resource1">*</asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldIP2" runat="server" ControlToValidate="TxtIP2"
            ErrorMessage="El campo IP de la Red 2 no puede estar vacio." Style="z-index: 121; left: 370px; position: absolute; top: 294px"
            Visible="False"
            meta:resourcekey="RequiredFieldIP2Resource1">*</asp:RequiredFieldValidator>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores"
            Style="z-index: 115; left: 233px; position: absolute; top: 391px"
            Visible="False" meta:resourcekey="ValidationSummary1Resource1" />
        <asp:RequiredFieldValidator ID="RequiredFieldIdTIFX" runat="server" ControlToValidate="TBIdEquipo"
            ErrorMessage="El identificador del equipo externo no puede estar vacío." Style="z-index: 116; left: 410px; position: absolute; top: 105px; width:auto"
            Visible="False"
            meta:resourcekey="RequiredFieldIdTIFXResource1">*</asp:RequiredFieldValidator>

    <asp:LinkButton ID="BtNuevo" runat="server" CausesValidation="False" OnClick="BtNuevo_Click"
		SkinID="MascaraBotonNuevo" Text="Nuevo" meta:resourcekey="BtNuevoResource1" />
	<asp:LinkButton ID="BtEliminar" runat="server" CausesValidation="False" OnClick="BtEliminar_Click" 
		SkinID="MascaraBotonEliminar" Text="Eliminar" 
		meta:resourcekey="BtEliminarResource1" />
	 <ajaxToolKit:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender" 
         runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
     </ajaxToolKit:ConfirmButtonExtender>
	<asp:LinkButton ID="BtModificar" runat="server" CausesValidation="False" OnClick="BtModificar_Click"
		SkinID="MascaraBotonModificar" Text="Modificar" 
		meta:resourcekey="BtModificarResource1" />
	<asp:linkButton ID="BtCancelar" runat="server" CausesValidation="False" OnClick="BtCancelar_Click"
		SkinID="MascaraBotonCancelar" Text="Cancelar" 
		meta:resourcekey="BtCancelarResource1" />
	 <ajaxToolKit:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender" 
         runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
     </ajaxToolKit:ConfirmButtonExtender>
</asp:Content>

