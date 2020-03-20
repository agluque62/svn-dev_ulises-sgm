<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="TOP.aspx.cs" Inherits="TOP" StylesheetTheme="TemaPaginasConfiguracion"
		CodeFileBaseClass="PageBaseCD40.PageCD40" Title="Gestión de Terminales de Operador" EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	  <script type="text/javascript">

			function AbreVentana(ventana) {
			window.open(ventana);
		}
	</script>

    <asp:Label ID="Label6" runat="server" Text="GESTIÓN DE TERMINALES DE OPERADOR" 
		  CssClass="labelPagina" meta:resourcekey="Label6Resource1"/>
    <asp:ListBox ID="ListBox1" runat="server" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" 
			SkinID="MascaraListaElementos" AutoPostBack="True" 
		  meta:resourcekey="ListBox1Resource1"></asp:ListBox>
    
    <asp:Panel ID="Panel1" runat="server" Height="400px" Style="z-index: 109; left: 205px; position: absolute; top: 60px"
        Width="550px" BorderStyle="Inset">

    <asp:Label ID="Label5" runat="server" Style="z-index: 113; left: 22px; position: absolute;
        top: 14px" Text="Identificador:" Visible="False" 
		  meta:resourcekey="Label5Resource1"></asp:Label>
    <asp:TextBox ID="TxtIdTop" runat="server" Style="z-index: 114; left: 22px; position: absolute;
        top: 34px" Visible="False" MaxLength="32" 
		  meta:resourcekey="TxtIdTopResource1"></asp:TextBox>

    <asp:Label ID="Label1" runat="server" Style="z-index: 101; left: 22px; position: absolute;
        top: 64px" Text="Posición SACTA:" Visible="False" 
		  meta:resourcekey="Label1Resource1"></asp:Label>
    <asp:DropDownList ID="DListPosicion" runat="server" Style="z-index: 122; left: 22px;
        position: absolute; top: 84px" Visible="False" Width="57px" Enabled="false" class="select"
		  meta:resourcekey="DListPosicionResource1">
    </asp:DropDownList>

    <asp:Label ID="Label2" runat="server" Style="z-index: 102; left: 220px; position: absolute;
        top: 64px" Text="Modo de Arranque:" Visible="false" 
		  meta:resourcekey="Label2Resource1"></asp:Label>
    <asp:DropDownList ID="DListArranque" runat="server" Style="z-index: 106; left: 220px;
        position: absolute; top: 84px" Width="103px" Visible="false"  class="select"
		  meta:resourcekey="DListArranqueResource1" Enabled="false">
        <asp:ListItem Value="A" meta:resourcekey="ListItemResource1">Autom&#225;tico</asp:ListItem>
        <asp:ListItem Value="M" meta:resourcekey="ListItemResource2">Manual</asp:ListItem>
    </asp:DropDownList>

    <asp:Label ID="Label3" runat="server" Style="z-index: 107; left: 22px; position: absolute;
        top: 134px" Text="IP de la Red:" Visible="false" 
		  meta:resourcekey="Label3Resource1"></asp:Label>
    <asp:TextBox ID="TxtIP1" runat="server" Style="z-index: 103; left: 22px; position: absolute;
        top: 154px" Visible="false" meta:resourcekey="TxtIP1Resource1"></asp:TextBox>

    <asp:Label ID="Label4" runat="server" Style="z-index: 105; left: 220px; position: absolute;
        top: 134px" Text="IP de la Red 2:" Visible="false" 
		  meta:resourcekey="Label4Resource1"></asp:Label>
    <asp:TextBox ID="TxtIP2" runat="server" Style="z-index: 104; left: 220px; position: absolute;
        top: 154px" Visible="false" meta:resourcekey="TxtIP2Resource1"></asp:TextBox>

    <table runat="server" id="TblRecorders" style="position: absolute; top: 215px; left: 20px" visible="True">
        <tr>
            <td>
                <asp:Label ID="Label43" runat="server" Style="z-index: 159"
                    Text="Grabador 1:" Visible="true" meta:resourcekey="DDLRecorder1Resource1"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="DDLRecorder1" runat="server" Visible="true" Style="z-index: 154;"
                    Width="200px" Height="22px" AppendDataBoundItems="True" class="select" Enabled="false">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label41" runat="server" Style="z-index: 159"
                    Text="Grabador 2:" Visible="true" meta:resourcekey="DDLRecorder2Resource1"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="DDLRecorder2" runat="server" Visible="true" Style="z-index: 154;"
                    Width="200px" Height="22px" AppendDataBoundItems="True" class="select" Enabled="false">
                </asp:DropDownList>
            </td>
        </tr>
    </table>


    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 111; left: 110px; position: absolute;
        top: 365px" Text="Aceptar" Width="80px" Visible="False" UseSubmitBehavior="true"  
		  OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
      <ajaxToolKit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
      </ajaxToolKit:ConfirmButtonExtender>
	<asp:Label ID="LblIp1Existente" runat="server" Visible="False" 
		Text="La dirección IP Red ya existe en el sistema." ForeColor="Red"
				Style="position:absolute; top: 192px; left: 232px; width: auto; height: 44px;" 
		  meta:resourcekey="LblIp1ExistenteResource1"></asp:Label>
	<asp:Label ID="LblIp2Existente" runat="server" Visible="False" 
		Text="La dirección IP Red 2 ya existe en el sistema." ForeColor="Red"
				Style="position:absolute; top: 242px; left: 232px; width: auto; height: 44px;" 
		  meta:resourcekey="LblIp2ExistenteResource1"></asp:Label>

    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtIP1"
        ErrorMessage="La IP de la Red no tiene el formato correcto" SetFocusOnError="True"
        Style="z-index: 116; left: 205px; position: absolute; top: 154px" 
		  ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}" Height="22px" 
		  Visible="False" Width="14px" 
		  meta:resourcekey="RegularExpressionValidator1Resource1">*</asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtIP2"
        ErrorMessage="La IP de la Red 2 no tiene el formato correcto" Height="22px" SetFocusOnError="True"
        Style="z-index: 117; left: 521px; position: absolute; top: 185px" ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"
        Visible="False" Width="16px" 
		  meta:resourcekey="RegularExpressionValidator2Resource1">*</asp:RegularExpressionValidator>
        </asp:Panel>


        <asp:LinkButton ID="BtNuevo" runat="server" OnClick="BtNuevo_Click" 
		  CausesValidation="False" Text="Nuevo" SkinID="MascaraBotonNuevo" 
		  meta:resourcekey="BtNuevoResource1"/>
    <asp:LinkButton ID="BtEliminar" runat="server" OnClick="BtEliminar_Click" 
		  CausesValidation="False" Text="Eliminar" SkinID="MascaraBotonEliminar" 
		  meta:resourcekey="BtEliminarResource1"/>
      <ajaxToolKit:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
      </ajaxToolKit:ConfirmButtonExtender>
    <asp:LinkButton ID="BtModificar" runat="server" OnClick="BtModificar_Click" 
		  CausesValidation="False" Text="Modificar" SkinID="MascaraBotonModificar" 
		  meta:resourcekey="BtModificarResource1"/>
    <asp:LinkButton ID="BtCancelar" runat="server" OnClick="BtCancelar_Click" 
		  CausesValidation="False" Text="Cancelar" SkinID="MascaraBotonCancelar" 
		  meta:resourcekey="BtCancelarResource1"/>
      <ajaxToolKit:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
      </ajaxToolKit:ConfirmButtonExtender>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores:"
        Style="z-index: 118; left: 250px; position: absolute; top: 490px" 
		  meta:resourcekey="ValidationSummary1Resource1" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtIdTop"
        ErrorMessage="El campo Identificador no puede estar vacio." SetFocusOnError="True"
        Style="z-index: 119; left: 410px; position: absolute; top: 100px" 
		  Visible="False" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtIP1"
        ErrorMessage="El campo IP de la Red no puede estar vacio." Height="22px" SetFocusOnError="True"
        Style="z-index: 120; left: 410px; position: absolute; top: 220px" Visible="False"
        Width="14px" meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtIP2"
        ErrorMessage="El campo IP de la Red 2 no puede estar vacio." Height="22px" SetFocusOnError="True"
        Style="z-index: 121; left: 410px; position: absolute; top: 220px" Visible="False"
        Width="16px" meta:resourcekey="RequiredFieldValidator3Resource1">*</asp:RequiredFieldValidator>
</asp:Content>

