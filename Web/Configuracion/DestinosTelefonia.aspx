<%@ Page Language="C#" MasterPageFile="~/CD40.master" EnableEventValidation="false" AutoEventWireup="true" StylesheetTheme="TemaPaginasConfiguracion"
	CodeFileBaseClass="PageBaseCD40.PageCD40" CodeFile="DestinosTelefonia.aspx.cs" Inherits="DestinosTelefonia" Title="Gestión destinos telefonía" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

  <script type="text/javascript">
      function AbreVentana(ventana) {
          window.open(ventana);
      }
	</script>

    <asp:Label ID="Label1" runat="server" Text="GESTIÓN DE DESTINOS DE TELEFONÍA" 
		  CssClass="labelPagina" meta:resourcekey="Label1Resource1"></asp:Label>
                
    <asp:ListBox ID="LBDestinos" runat="server" AutoPostBack="True" OnSelectedIndexChanged="LBDestinos_IndexChanged"
						SkinID="MascaraListaElementosWide" 	  meta:resourcekey="LBDestinosResource1">
    </asp:ListBox>

    <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Height="256px" Style="z-index: 105; left: 275px; position: absolute; top: 58px"
        Width="540px" Enabled="False"
        meta:resourcekey="Panel1Resource1">

        <asp:Label ID="Label2" runat="server" Style="left: 12px; position: absolute; top: 22px; z-index: 102; height: 19px; width: inherit;"
            Text="Destino:" meta:resourcekey="Label2Resource1"></asp:Label>
        <asp:TextBox ID="TBDestino" runat="server" ReadOnly="True"  MaxLength="32" Style="left: 12px; position: absolute; top: 42px; z-index: 106; height: 21px; width: 205px;"
            meta:resourcekey="TBDestinoResource1"></asp:TextBox>

        <asp:Label ID="Label3" runat="server" Style="left: 12px; position: absolute; top: 68px; z-index: 103;"
            Text="Prefijo:" meta:resourcekey="Label3Resource1"></asp:Label>
        <asp:DropDownList ID="DDLPrefijo" runat="server" AppendDataBoundItems="True" Style="left: 12px; position: absolute; top: 88px; z-index: 107;"
            Width="205px" class="select"
            AutoPostBack="True" OnSelectedIndexChanged="DDLPrefijo_SelectedIndexChanged"
            Enabled="False" meta:resourcekey="DDLPrefijoResource1">
            <asp:ListItem Value="-1" meta:resourcekey="ListItemResource1">&lt;Selecciona red&gt;</asp:ListItem>
            <asp:ListItem Value="1" meta:resourcekey="ListItemResource2">L&#237;nea Caliente Externa</asp:ListItem>
            <asp:ListItem Value="32" meta:resourcekey="ListItemResource3">Punto a punto</asp:ListItem>
        </asp:DropDownList>

        <asp:Label ID="Label5" runat="server" Style="left: 12px; position: absolute; top: 114px; z-index: 105;"
            Text="Número destino:" meta:resourcekey="Label5Resource1"></asp:Label>
        <asp:TextBox ID="TBAbonado" runat="server" Style="left: 12px; position: absolute; top: 134px; width: 205px; z-index: 109;"
            Enabled="False"
            meta:resourcekey="TBAbonadoResource1"></asp:TextBox>

        <asp:Label ID="LbGrupo" runat="server" Style="left: 12px; position: absolute; top: 160px; z-index: 104;"
            Text="Grupo:" meta:resourcekey="Label4Resource1"></asp:Label>
        <asp:TextBox ID="TBGrupo" runat="server" Style="left: 12px; position: absolute; top: 180px; z-index: 110;"
            Width="205px" Enabled="False"
            meta:resourcekey="TBGrupoResource1"></asp:TextBox>
        <asp:DropDownList ID="DDLGrupo" runat="server" Style="left: 12px; position: absolute; top: 205px; z-index: 108;"
            Width="205px" AppendDataBoundItems="True" Visible="False"
            OnSelectedIndexChanged="DDLGrupo_SelectedIndexChanged" AutoPostBack="True"
            meta:resourcekey="DDLGrupoResource1" class="select">
            <asp:ListItem Value="-1" meta:resourcekey="ListItemResource4">&lt;Selecciona grupo&gt;</asp:ListItem>
        </asp:DropDownList>

        <asp:Label ID="LblRecurso" runat="server" Style="left: 361px; position: absolute; top: 68px; z-index: 119;"
            Text="Recurso asignado:" Visible="False"
            meta:resourcekey="LblRecursoResource1"></asp:Label>
        <asp:TextBox ID="TBRecurso" runat="server" ReadOnly="True"
            Style="left: 361px; position: absolute; top: 88px; z-index: 120; height: 21px; width: 178px;"
            Visible="False" meta:resourcekey="TBRecursoResource1"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TBRecurso"
        ErrorMessage="El recurso es obligatorio para este tipo de destino." Style="left: 530px;
        position: absolute; top: 88px; z-index: 122;" Width="11px" Visible="False" 
		  meta:resourcekey="RequiredFieldValidator3Resource1">*</asp:RequiredFieldValidator>

        <asp:Label ID="LblRecursosLibres" runat="server" Style="left: 361px; position: absolute; top: 114px; z-index: 121;"
            Text="Recursos libres" Visible="False" 
            meta:resourcekey="LblRecursosLibresResource1"></asp:Label>
        <asp:DropDownList ID="DDLRecursos" runat="server" AppendDataBoundItems="True" Style="left: 361px; position: absolute; top: 134px; z-index: 118;"
            Visible="False"  class="select"
            Width="178px" OnSelectedIndexChanged="DDLRecursos_SelectedIndexChanged"
            Enabled="False" AutoPostBack="True" meta:resourcekey="DDLRecursosResource1">
            <asp:ListItem Value="-1" meta:resourcekey="ListItemResource5">&lt;Selecciona recurso&gt;</asp:ListItem>
        </asp:DropDownList>

        <asp:Button ID="BAnadir" runat="server" OnClick="BAnadir_Click" Style="left: 405px; position: absolute; top: 213px; z-index: 113;"
            Text="Aceptar"
            Visible="False" Width="100px" meta:resourcekey="BAnadirResource1" />
        <ajaxToolkit:ConfirmButtonExtender ID="BAnadir_ConfirmButtonExtender"
            runat="server" ConfirmText="" Enabled="True" TargetControlID="BAnadir">
        </ajaxToolkit:ConfirmButtonExtender>
    </asp:Panel>

    <asp:LinkButton ID="BCancelar" runat="server" OnClick="BCancelar_Click" 
		  CausesValidation="False" Text="Cancelar" SkinID="MascaraBotonCancelar" 
		  meta:resourcekey="BCancelarResource1"/>
     <ajaxToolKit:ConfirmButtonExtender ID="BCancelar_ConfirmButtonExtender" 
         runat="server" ConfirmText="" Enabled="True" TargetControlID="BCancelar">
     </ajaxToolKit:ConfirmButtonExtender>
    <asp:LinkButton ID="BModificar" runat="server" OnClick="BModificar_Click" 
		  CausesValidation="False" Text="Modificar" SkinID="MascaraBotonModificar" 
		  meta:resourcekey="BModificarResource1"/>
    <asp:LinkButton ID="LBImprimir" runat="server" SkinID="MascaraBotonImprimir" Visible="false"
		  meta:resourcekey="LBImprimirResource1">Imprimir</asp:LinkButton>
    <asp:LinkButton ID="BEliminar" runat="server" OnClick="BEliminar_Click" 
		  CausesValidation="False" Text="Eliminar" SkinID="MascaraBotonEliminar" 
		  meta:resourcekey="BEliminarResource1" />
     <ajaxToolKit:ConfirmButtonExtender ID="BEliminar_ConfirmButtonExtender" 
         runat="server" ConfirmText="" Enabled="True" TargetControlID="BEliminar">
     </ajaxToolKit:ConfirmButtonExtender>
    <asp:LinkButton ID="BNuevo" runat="server" OnClick="BNuevo_Click" 
		  CausesValidation="False" Text="Nuevo" SkinID="MascaraBotonNuevo" 
		  meta:resourcekey="BNuevoResource1"/>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TBDestino"
        ErrorMessage="El literal es obligatorio para dar de alta el destino." 
		  style="left: 470px; position: absolute; top: 110px; z-index: 116;" 
		  Visible="False" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDLPrefijo"
        ErrorMessage="El prefijo es obligatorio para dar de alta el destino." 
		  InitialValue="-1" Style="left: 470px;
        position: absolute; top: 155px; z-index: 117;" Visible="False" 
		  meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Sólo son válidos los números y los caracteres # * y ,(pausa)"
            ControlToValidate="TBAbonado" ValidationExpression="(#|\*|\,|[0-9])+"  Style="position: absolute;left: 470px; top: 200px; z-index: 117;" Visible="true" 
        meta:resourcekey="RegularExpressionValidator1Resource1" >*
    </asp:RegularExpressionValidator>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores"
        Style="z-index: 124; left: 270px; position: absolute; top: 330px" 
		  Visible="False" meta:resourcekey="ValidationSummary1Resource1" />
</asp:Content>

