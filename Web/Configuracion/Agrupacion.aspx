<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="Agrupacion.aspx.cs" Inherits="Agrupacion" 
	CodeFileBaseClass="PageBaseCD40.PageCD40"	Title="Gestión de Agrupaciones" EnableEventValidation="false" StyleSheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <script type="text/javascript">

			function AbreVentana(ventana) {
			window.open(ventana);
		}
	</script>

	<asp:Label ID="Label1" runat="server" Text="GESTIÓN DE AGRUPACIONES" 
		CssClass="labelPagina" meta:resourcekey="Label1Resource1"></asp:Label>	
	<asp:ListBox ID="ListBox1" runat="server" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" 
			AutoPostBack="True" SkinID="MascaraListaElementos" 
		  meta:resourcekey="ListBox1Resource1">
   </asp:ListBox>

    <asp:ImageButton ID="IButAsignar" runat="server" OnClick="ImageButton1_Click" Style="z-index: 102;
        left: 468px; position: absolute; top: 232px;  -webkit-transform: rotate(90deg); -moz-transform: rotate(90deg); -o-transform: rotate(90deg); transform: rotate(90deg);"
         CausesValidation="False" ImageUrl="~/Configuracion/Images/arrow.png" Visible="False" 
		  meta:resourcekey="IButAsignarResource1" />
    &nbsp;
    <asp:ImageButton ID="IButQuitar" runat="server" Style="z-index: 103; left: 468px;
        position: absolute; top: 283px; -webkit-transform: rotate(-90deg); -moz-transform: rotate(-90deg); -o-transform: rotate(-90deg); transform: rotate(-90deg); " 
         CausesValidation="False" 
		  ImageUrl="~/Configuracion/Images/arrow.png" OnClick="IButQuitar_Click" 
		  Visible="False" meta:resourcekey="IButQuitarResource1" />
    <asp:RequiredFieldValidator ID="RequiredFieldIdentificador" runat="server" ControlToValidate="TxtIdAgrupacion"
        ErrorMessage="Rellene el campo Identificador" Style="z-index: 104; left: 535px;
        position: absolute; top: 90px" Visible="False" 
		  meta:resourcekey="RequiredFieldIdentificadorResource1">*</asp:RequiredFieldValidator>
    &nbsp; &nbsp;
    <asp:LinkButton ID="BtNuevo" runat="server" OnClick="BtNuevo_Click" 
		  CausesValidation="False" Text="Nuevo" SkinID="MascaraBotonNuevo" 
		  meta:resourcekey="BtNuevoResource1" />
    <asp:LinkButton ID="BtEliminar" runat="server"  OnClick="BtEliminar_Click" 
		  CausesValidation="False" Text="Eliminar" SkinID="MascaraBotonEliminar" 
		  meta:resourcekey="BtEliminarResource1" />
      <ajaxToolKit:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
      </ajaxToolKit:ConfirmButtonExtender>
    <asp:LinkButton ID="BtModificar" runat="server" OnClick="BtModificar_Click" 
		  CausesValidation="False" Text="Modificar" SkinId="MascaraBotonModificar" 
		  meta:resourcekey="BtModificarResource1" />
    &nbsp; &nbsp;&nbsp; &nbsp;
    <asp:Label ID="Label3" runat="server" Style="z-index: 110; left: 264px; position: absolute;
        top: 126px" Text="Sectores Asignados" meta:resourcekey="Label3Resource1" ></asp:Label>
    <asp:Label ID="Label4" runat="server" Style="z-index: 111; left: 518px; position: absolute;
        top: 126px" Text="Sectores Disponibles" Visible="False" 
		  meta:resourcekey="Label4Resource1"></asp:Label>

    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 113; left: 397px; position: absolute;
        top: 535px" Text="Aceptar" Width="80px" Visible="False" UseSubmitBehavior="true"
		  OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
      <ajaxToolKit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
      </ajaxToolKit:ConfirmButtonExtender>
    <asp:ValidationSummary ID="errores" runat="server" HeaderText="Resumen de Errores:"
        Style="z-index: 114; left: 264px; position: absolute; top: 455px" 
		  Visible="False" meta:resourcekey="erroresResource1" />
    <asp:LinkButton ID="BtCancelar" runat="server" Text="Cancelar" SkinID="MascaraBotonCancelar" 
		  OnClick="BtCancelar_Click" CausesValidation="False" 
		  meta:resourcekey="BtCancelarResource1" />
      <ajaxToolKit:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
      </ajaxToolKit:ConfirmButtonExtender>
    <asp:Label ID="Label5" runat="server" Style="z-index: 116; left: 264px; position: absolute;
        top: 63px" Text="Identificador:" meta:resourcekey="Label5Resource1" ></asp:Label>
    <asp:TextBox ID="TxtIdAgrupacion" runat="server" Style="z-index: 117; left: 264px; position: absolute;
        top: 87px" MaxLength="32" Width="262px" ReadOnly="True" 
		  meta:resourcekey="TxtIdAgrupacionResource1"></asp:TextBox>

    
    <asp:ListBox ID="ListSectores" runat="server" Height="308px" Enabled="False" Style="z-index: 120;
        left: 264px; position: absolute; top: 158px" Width="188px" 
		  SelectionMode="Multiple" meta:resourcekey="ListSectoresResource1"></asp:ListBox>
    <asp:ListBox ID="ListSectoresLibres" runat="server" Height="308px" Style="z-index: 121;
        left: 517px; position: absolute; top: 158px" Visible="False" Width="188px" 
		  SelectionMode="Multiple" meta:resourcekey="ListSectoresLibresResource1"></asp:ListBox>
</asp:Content>

