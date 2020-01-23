<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="Grupos.aspx.cs" Inherits="Grupos" 
CodeFileBaseClass="PageBaseCD40.PageCD40"	Title="Gestión de Grupos de Telefonía" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <script type="text/javascript">
			function AbreVentana(ventana) {
			window.open(ventana);
		}
	</script>
    <asp:Label ID="Label4" runat="server" Text="GESTIÓN DE GRUPOS DE TELEFONÍA" 
		  CssClass="labelPagina" meta:resourcekey="Label4Resource1"></asp:Label>        
    <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" 
		  OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" 
		  SkinID="MascaraListaElementos" meta:resourcekey="ListBox1Resource1"></asp:ListBox>

    <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Height="245px" Style="z-index: 105; left: 238px; position: absolute; top: 60px"
    Width="329px" Enabled="False"
    meta:resourcekey="Panel1Resource1">

    <asp:Label ID="Label1" runat="server" Style="z-index: 109; left: 14px; position: absolute;
        top: 15px; height: auto;" Text="Identificador:" Visible="False" 
		  meta:resourcekey="Label1Resource1"></asp:Label>
    <asp:TextBox ID="TextBox1" runat="server" MaxLength="32" Style="z-index: 104; left: 14px;
        position: absolute; top: 34px" Visible="False" Width="173px" 
		  meta:resourcekey="TextBox1Resource1"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldNucleo" runat="server" ControlToValidate="TextBox1"
        ErrorMessage="Introduzca un nombre para el nuevo grupo." SetFocusOnError="True"
        Style="z-index: 108; left: 214px; position: absolute; top: 34px" 
		 Visible="False" meta:resourcekey="RequiredFieldNucleoResource1"></asp:RequiredFieldValidator>

    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 105; left: 95px; position: absolute;
        top: 67px" Text="Aceptar" Visible="False" Width="80px" UseSubmitBehavior="true"  
		 OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />

    <asp:Label ID="Label3" runat="server" Style="z-index: 112; left: 14px; position: absolute;
        top: 22px; height: 18px;" Text="Label" Visible="False" 
		  meta:resourcekey="Label3Resource1"></asp:Label>
    <asp:ListBox ID="ListBox2" runat="server" Height="188px" Style="z-index: 101; left: 14px;
        position: absolute; top: 42px" Width="214px" 
		 OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" Visible="False" 
		  meta:resourcekey="ListBox2Resource1">
        </asp:ListBox>

        </asp:Panel>


      <ajaxToolKit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
      </ajaxToolKit:ConfirmButtonExtender>
        
    <asp:LinkButton ID="BtEliminar" runat="server" OnClick="BtEliminar_Click" 
		  CausesValidation="False" Text="Eliminar" SkinID="MascaraBotonEliminar" 
		  meta:resourcekey="BtEliminarResource1"/>
      <ajaxToolKit:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
      </ajaxToolKit:ConfirmButtonExtender>
    <asp:LinkButton ID="BtNuevo" runat="server" OnClick="BtNuevo_Click" 
		  CausesValidation="False" Text="Nuevo" SkinID="MascaraBotonNuevo" 
		  meta:resourcekey="BtNuevoResource1"/>
    <asp:LinkButton ID="BtCancelar" runat="server" OnClick="BtCancelar_Click" 
		  CausesValidation="False" Text="Cancelar" SkinID="MascaraBotonCancelar" 
		  meta:resourcekey="BtCancelarResource1"/>
      <ajaxToolKit:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
      </ajaxToolKit:ConfirmButtonExtender>
    <asp:LinkButton ID="LBImprimir" runat="server" SkinID="MascaraBotonImprimir" Visible="false"
		  meta:resourcekey="LBImprimirResource1">Imprimir</asp:LinkButton>
</asp:Content>

