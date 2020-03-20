<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="DestinosRadio.aspx.cs" Inherits="DestinosRadio" 
		CodeFileBaseClass="PageBaseCD40.PageCD40" Title="Gestión de Destinos de Radio" EnableEventValidation="false" StyleSheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
    <script type="text/javascript">
			function AbreVentana(ventana) {
			window.open(ventana);
		}
	</script>

    <asp:Label ID="Label1" runat="server" Style="z-index: 112; position: absolute; " 
			Text="GESTIÓN DE DESTINOS DE RADIO" CssClass="labelPagina" 
		  meta:resourcekey="Label1Resource1"></asp:Label>
	 <asp:ListBox ID="ListBox1" runat="server" 
		  OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" AutoPostBack="True"
					SkinID="MascaraListaElementos" meta:resourcekey="ListBox1Resource1">
    </asp:ListBox>

        <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Height="415px" Style="z-index: 105; left: 238px; position: absolute; top: 60px"
            Width="529px" Enabled="False"
            meta:resourcekey="Panel1Resource1">

            <asp:Label ID="Label5" runat="server" Style="z-index: 116; left: 14px; position: absolute; top: 22px"
                Text="Identificador:" Visible="False"
                meta:resourcekey="Label5Resource1"></asp:Label>
            <asp:TextBox ID="TxtIdEnlace" runat="server" Style="z-index: 117; left: 14px; position: absolute; top: 42px"
                Visible="False" MaxLength="32"
                meta:resourcekey="TxtIdEnlaceResource1"></asp:TextBox>

            <asp:Label ID="Label2" runat="server" Style="z-index: 101; left: 14px; position: absolute; top: 68px"
                Text="Tipo:" Visible="False" meta:resourcekey="Label2Resource1"></asp:Label>
            <asp:DropDownList ID="DListTipo" runat="server" Style="z-index: 105; left: 14px; position: absolute; top: 88px"
                Width="90px" Visible="False" OnSelectedIndexChanged="DListTipo_SelectedIndexChanged" AutoPostBack="true"
                meta:resourcekey="DListTipoResource1" class="select">
                <asp:ListItem Value="0" meta:resourcekey="ListItemResource1">VHF</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="ListItemResource2">UHF</asp:ListItem>
                <asp:ListItem Value="2">HF</asp:ListItem>
            </asp:DropDownList>

            <asp:Table runat="server" ID="TblTunedFreq" Style="left: 124px; position: absolute; top: 88px" Visible="false">
                <asp:TableRow runat="server" >
                    <asp:TableCell ID="TableCell1" runat="server">
                        <asp:Label runat="server" ID="LblTunedFreq" meta:resourcekey="LblTunedFreqResource1"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell2" runat="server">
                        <asp:TextBox ID="TBTunedFrequency" runat="server" Width="150px" ></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell3" runat="server">
                        <asp:Label ID="Label6" runat="server">Hz</asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <%--<ajaxToolkit:MaskedEditExtender ID="MaskedEditValidator1" runat="server" TargetControlID="TbTunedFrequency" MaskType="Number"
                Mask="99,999,999" InputDirection="RightToLeft" AcceptNegative="None" ClearMaskOnLostFocus="false" AutoComplete="true" AutoCompleteValue="0">
            </ajaxToolkit:MaskedEditExtender>
            <ajaxToolkit:MaskedEditValidator runat="server" Style="left: 480px; position: absolute; top: 90px" 
                ControlExtender="MaskedEditValidator1"
                ControlToValidate="TbTunedFrequency" 
                IsValidEmpty="False" 
                MinimumValue="1600000" 
                MaximumValue="30000000" 
                EmptyValueBlurredText="*" 
                InvalidValueBlurredMessage="I" 
                MaximumValueBlurredMessage="M" 
                MinimumValueBlurredText="m"
                Display="Static"
                meta:resourcekey="RangeMessageResource1"/>--%>

            <asp:CheckBox ID="CheckExclusividad" runat="server" Style="z-index: 122; left: 14px; position: absolute; top: 114px"
                Text="Exclusividad TX-RX" TextAlign="Left" Width="153px"
                Visible="False" meta:resourcekey="CheckExclusividadResource1" />

            <asp:Label ID="Label3" runat="server" Style="z-index: 110; left: 14px; position: absolute; top: 150px"
                Text="Recursos Asignados" Visible="False"
                meta:resourcekey="Label3Resource1"></asp:Label>
            <asp:ListBox ID="ListRecursos" runat="server" Height="210px" Style="z-index: 120; left: 14px; position: absolute; top: 170px"
                Visible="False" Width="188px"
                SelectionMode="Multiple" meta:resourcekey="ListRecursosResource1"></asp:ListBox>

            <asp:Label ID="Label4" runat="server" Style="z-index: 111; left: 290px; position: absolute; top: 150px"
                Text="Recursos Libres" Visible="False"
                meta:resourcekey="Label4Resource1"></asp:Label>
            <asp:ListBox ID="ListRecursosLibres" runat="server" Height="210px" Style="z-index: 121; left: 290px; position: absolute; top: 170px"
                Visible="False" Width="188px"
                SelectionMode="Multiple" meta:resourcekey="ListRecursosLibresResource1"></asp:ListBox>

            <asp:ImageButton ID="IButAsignar" runat="server" OnClick="ImageButton1_Click" Style="z-index: 102; left: 225px; position: absolute; top: 232px"
                CausesValidation="False"
                ImageUrl="~/Configuracion/Images/arrowIzq.gif" Visible="False"
                meta:resourcekey="IButAsignarResource1" />
                    &nbsp;
            <asp:ImageButton ID="IButQuitar" runat="server" Style="z-index: 103; left: 225px; position: absolute; top: 283px"
                CausesValidation="False"
                ImageUrl="~/Configuracion/Images/arrowDech.gif" OnClick="IButQuitar_Click"
                Visible="False" meta:resourcekey="IButQuitarResource1" />

            <asp:RequiredFieldValidator ID="RequiredFieldIdentificador" runat="server" ControlToValidate="TxtIdEnlace"
                ErrorMessage="Rellene el campo Identificador"
                Style="left: 172px; position: absolute; top: 40px" Enabled="False"
                meta:resourcekey="RequiredFieldIdentificadorResource1">*</asp:RequiredFieldValidator>
<%--            <asp:RangeValidator ID="RVFrequency" runat="server" Style="left: 480px; position: absolute; top: 90px" 
                MaximumValue="30000000" MinimumValue="1600000" ControlToValidate="TBTunedFrequency" 
                ErrorMessage="Out of range" Type="Integer"  meta:resourcekey="RangeMessageResource1">*</asp:RangeValidator>--%>
        </asp:Panel>

    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 113; left: 365px; position: absolute; top: 533px"
        Text="Aceptar" Width="80px" Visible="False" UseSubmitBehavior="true"
        OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
    <ajaxToolkit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender"
        runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
    </ajaxToolkit:ConfirmButtonExtender>

    <asp:ValidationSummary ID="errores" runat="server" HeaderText="Resumen de errores:" ForeColor="Red"
        Style="z-index: 114; left: 240px; position: absolute; top: 525px"
        Visible="False" meta:resourcekey="erroresResource1" />
    <asp:Label ID="LblErrorMismatchFrequency" runat="server" Style="z-index: 115; left: 240px; position: absolute; top: 525px; height: 12px; width: 535px;" ForeColor="Red"
        Visible="False" meta:resourcekey="LblMismatchFrequency">Los recursos asignados tienen configurada distinta frecuencia</asp:Label> 

    <asp:LinkButton ID="BtCancelar" runat="server" OnClick="BtCancelar_Click" 
		  CausesValidation="False" Text="Cancelar" SkinID="MascaraBotonCancelar" 
		  meta:resourcekey="BtCancelarResource1"/>
      <ajaxToolKit:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
      </ajaxToolKit:ConfirmButtonExtender>

    <asp:LinkButton ID="BtModificar" runat="server" OnClick="BtModificar_Click" 
		  CausesValidation="False" Text="Modificar" SkinID="MascaraBotonModificar" 
		  meta:resourcekey="BtModificarResource1"/>
    <asp:LinkButton ID="BtNuevo" runat="server"	OnClick="BtNuevo_Click" 
		  CausesValidation="False" Text="Nuevo" SkinID="MascaraBotonNuevo" 
		  meta:resourcekey="BtNuevoResource1"/>
    <asp:LinkButton ID="BtEliminar" runat="server" OnClick="BtEliminar_Click" 
		  CausesValidation="False" Text="Eliminar" SkinID="MascaraBotonEliminar" 
		  meta:resourcekey="BtEliminarResource1"/>
    <ajaxToolkit:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender"
        runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
    </ajaxToolkit:ConfirmButtonExtender>
</asp:Content>

