<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="Emplazamientos.aspx.cs" StylesheetTheme="TemaPaginasConfiguracion"
	CodeFileBaseClass="PageBaseCD40.PageCD40" Inherits="Emplazamientos" Title="Gestión de Emplazamientos" EnableEventValidation="false" meta:resourcekey="PageResource1"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
			function AbreVentana(ventana) {
			window.open(ventana);
		}
		
	</script>

    <asp:Label ID="Label4" runat="server" Text="GESTIÓN DE EMPLAZAMIENTOS"
            CssClass="labelPagina" meta:resourcekey="Label4Resource1"></asp:Label>

    <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"
        SkinID="MascaraListaElementos" meta:resourcekey="ListBox1Resource1">
    </asp:ListBox>

    <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Height="396px" Style="z-index: 105; left: 238px; position: absolute; top: 60px"
        Width="350px" Enabled="False"
        meta:resourcekey="Panel1Resource1">

        <asp:Label ID="Label3" runat="server" Style="z-index: 110; left: 22px; position: absolute; top: 14px; width:auto"
            Text="Label" Visible="False" meta:resourcekey="Label3Resource1"></asp:Label>
        <asp:ListBox ID="LBoxRecursos" runat="server" Height="248px" Style="z-index: 101; left: 22px; position: absolute; top: 54px"
            Width="214px" AutoPostBack="True"
            Visible="False" meta:resourcekey="LBoxRecursosResource1"></asp:ListBox>

        <asp:Label ID="Label2" runat="server" Style="z-index: 109; left: 12px; position: absolute; top: 22px"
            Text="Emplazamiento:" Visible="False"
            meta:resourcekey="Label2Resource1"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" MaxLength="32" Style="z-index: 104; left: 12px; position: absolute; top: 42px"
            Visible="False" Width="173px"
            meta:resourcekey="TextBox1Resource1"></asp:TextBox>

        <asp:Button ID="BtAceptar" runat="server" Style="z-index: 105; left: 133px; position: absolute; top: 230px"
            Text="Aceptar" Visible="False" Width="80px" UseSubmitBehavior="true"
            OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
        <ajaxToolkit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender"
            runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
        </ajaxToolkit:ConfirmButtonExtender>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
            ErrorMessage="Es necesario dar un nombre al emplazamiento." SetFocusOnError="True"
            Style="z-index: 111; left: 200px; position: absolute; top: 47px"
            Visible="False" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
    </asp:Panel>

    <asp:LinkButton ID="BtEliminar" runat="server" Text="Eliminar" OnClick="BtEliminar_Click" CausesValidation="False" 
			SkinID="MascaraBotonEliminar" meta:resourcekey="BtEliminarResource1"/>
    <ajaxToolKit:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender" 
        runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
    </ajaxToolKit:ConfirmButtonExtender>
    <asp:LinkButton ID="BtNuevo" runat="server" OnClick="BtNuevo_Click" CausesValidation="False" 
			SkinID="MascaraBotonNuevo" Text="Nuevo" meta:resourcekey="BtNuevoResource1" />
    <asp:LinkButton ID="BtCancelar" runat="server" Text="Cancelar" OnClick="BtCancelar_Click" CausesValidation="False" 
			SkinID="MascaraBotonCancelar" meta:resourcekey="BtCancelarResource1" />
    <ajaxToolKit:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender" 
        runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
    </ajaxToolKit:ConfirmButtonExtender>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumén de errores:"
		Style="z-index: 118; left: 250px; position: absolute; top: 490px"  
		Visible="False" meta:resourcekey="ValidationSummary1Resource1" />

    </asp:Content>

