<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="Nucleos.aspx.cs" Inherits="Nucleos" 
CodeFileBaseClass="PageBaseCD40.PageCD40" Title="Gestión de Núcleos" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
<%--    <script type="text/javascript">
        if ("<%=GetWindowName()%>" != "Wizard") {
            if (window.name != "<%=GetWindowName()%>") {
                window.name = "invalidAccess";
                window.open("../BloqueoAplicacion.aspx", "_self");
            }
        }
	</script>
--%>
    <asp:Label ID="Label4" runat="server" Text="GESTIÓN DE NUCLEOS" 
		 CssClass="labelPagina" meta:resourcekey="Label4Resource1"></asp:Label>        

    <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" 
		 OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" 
		 SkinID="MascaraListaElementos" meta:resourcekey="ListBox1Resource1"></asp:ListBox>
    <asp:ListBox ID="ListBox2" runat="server" Style="z-index: 101; left: 212px;
        position: absolute; top: 249px; height: 215px;" Width="246px" 
		 AutoPostBack="True" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" 
		 Visible="False" meta:resourcekey="ListBox2Resource1">
        </asp:ListBox>
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
    <asp:TextBox ID="TextBox1" runat="server" MaxLength="32" Style="z-index: 104; left: 212px;
        position: absolute; top: 113px" Visible="False" Width="173px" 
		 meta:resourcekey="TextBox1Resource1"></asp:TextBox>
    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 105; left: 258px; position: absolute;
        top: 170px" Text="Aceptar" Visible="False" Width="80px" UseSubmitBehavior="true"  
		 OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
    <ajaxToolKit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender" 
        runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
    </ajaxToolKit:ConfirmButtonExtender>
    <asp:RequiredFieldValidator ID="RequiredFieldNucleo" runat="server" ControlToValidate="TextBox1"
        ErrorMessage="Introduzca un nombre para el nuevo núcleo." SetFocusOnError="True"
        Style="z-index: 108; left: 402px; position: absolute; top: 113px" 
		 Visible="False" meta:resourcekey="RequiredFieldNucleoResource1"></asp:RequiredFieldValidator>
    <asp:Label ID="Label1" runat="server" Style="z-index: 109; left: 212px; position: absolute;
        top: 84px" Text="Identificador:" Visible="False" 
		 meta:resourcekey="Label1Resource1"></asp:Label>
    <asp:Label ID="Label3" runat="server" Style="z-index: 112; left: 212px; position: absolute;
        top: 228px; height: 19px;" Text="Label" Visible="False" 
		 meta:resourcekey="Label3Resource1"></asp:Label>

</asp:Content>

