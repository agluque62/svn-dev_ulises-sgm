<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="Troncales.aspx.cs" Inherits="Troncales"
		CodeFileBaseClass="PageBaseCD40.PageCD40" Title="Gestión de Troncales" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <script type="text/javascript">
	        function AbreVentana(ventana) {
			window.open(ventana);
		}
		
	</script>

    <asp:Label ID="Label6" runat="server" Text="GESTIÓN DE TRONCALES" 
	CssClass="labelPagina" meta:resourcekey="Label6Resource1"></asp:Label>
    <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"
					SkinID="MascaraListaElementos" meta:resourcekey="ListBox1Resource1">
    </asp:ListBox>

    <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Height="396px" Style="z-index: 105; left: 238px; position: absolute; top: 60px"
        Width="280px" Enabled="False"
        meta:resourcekey="Panel1Resource1">

        <asp:Label ID="Label1" runat="server" Style="z-index: 107; left: 22px; position: absolute; top: 14px; height: 19px; width: 175px;"
            Text="Id de Troncal:" Visible="False"
            meta:resourcekey="Label1Resource1"></asp:Label>
        <asp:TextBox ID="TxtIdTroncal" runat="server" Style="z-index: 111; left: 22px; position: absolute; top: 34px"
            Visible="False" meta:resourcekey="TxtIdTroncalResource1"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldIdentificador" runat="server" ControlToValidate="TxtIdTroncal"
                ErrorMessage="Rellene el campo Identificador" Style="z-index: 125; left: 200px; position: absolute; top: 34px; width: 10px; height: 19px;"
                Visible="False" SetFocusOnError="True"
                meta:resourcekey="RequiredFieldIdentificadorResource1">*</asp:RequiredFieldValidator>

        <asp:Label ID="Label2" runat="server" Style="z-index: 108; left: 22px; position: absolute; top: 60px; height: 19px; width: 200px;"
            Text="Número de Test:" Visible="False"
            meta:resourcekey="Label2Resource1"></asp:Label>
        <asp:TextBox ID="TxtNumTest" runat="server" Style="z-index: 110; left: 22px; position: absolute; top: 80px"
            Visible="False" meta:resourcekey="TxtNumTestResource1"></asp:TextBox>

        <asp:Label ID="Label4" runat="server" Style="z-index: 114; left: 22px; position: absolute; top: 106px"
            Text="Label" Visible="False" 
            meta:resourcekey="Label4Resource1"></asp:Label>
        <asp:ListBox ID="ListBox2" runat="server" Style="z-index: 101; left: 22px; position: absolute; top: 146px; height: 245px;"
            Width="214px"
            AutoPostBack="True" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"
            Visible="False"
            ToolTip="Los recursos se muestran ordenados según el algoritmo de encaminamiento."
            meta:resourcekey="ListBox2Resource1"></asp:ListBox>

        <asp:Button ID="BtAceptar" runat="server" Style="z-index: 104; left: 65px; position: absolute; top: 150px"
            Text="Aceptar" Visible="False" Width="80px" UseSubmitBehavior="true"
            OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
        <ajaxToolkit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender"
            runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
        </ajaxToolkit:ConfirmButtonExtender>
    </asp:Panel>
        <asp:LinkButton ID="BtEliminar" runat="server" OnClick="BtEliminar_Click"
            CausesValidation="False" Text="Eliminar" SkinID="MascaraBotonEliminar"
            meta:resourcekey="BtEliminarResource1" />
        <ajaxToolkit:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender"
            runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
        </ajaxToolkit:ConfirmButtonExtender>
        <asp:LinkButton ID="BtNuevo" runat="server" OnClick="BtNuevo_Click"
            CausesValidation="False" Text="Nuevo" SkinID="MascaraBotonNuevo"
            meta:resourcekey="BtNuevoResource1" />
        <asp:LinkButton ID="BtCancelar" runat="server" OnClick="BtCancelar_Click"
            Text="Cancelar" SkinID="MascaraBotonCancelar" CausesValidation="False"
            meta:resourcekey="BtCancelarResource1" />
        <ajaxToolkit:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender"
            runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
        </ajaxToolkit:ConfirmButtonExtender>
        <asp:LinkButton ID="BtModificar" runat="server" OnClick="BtModificar_Click"
            CausesValidation="False" Text="Modificar" SkinID="MascaraBotonModificar"
            meta:resourcekey="BtModificarResource1" />

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores:"
                Style="z-index: 118; left: 250px; position: absolute; top: 490px" 
        meta:resourcekey="ValidationSummary1Resource1" />
</asp:Content>

