<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="RecursosTelefonia.aspx.cs" Inherits="RecursosDeTelefonia" 
	CodeFileBaseClass="PageBaseCD40.PageCD40"	Title="Gestión de Recursos" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

  <script type="text/javascript">
      function AbreVentana(ventana) {
          window.open(ventana);
      }
	</script>

    <!--
     * Función para mostrar los errores de los campos de texto.
     *  Localizada en los distintos idiomas.
     *
     * Params: el elemento, y los valores min y max a comprobar
     *
     * VMG: 13-11-2018
     *-->
    <script type="text/javascript">
        var isErrorTmMaxEntry = isErrorTmSupervisionOptions = false;
        var isErrorTmDeteccionCallerId = isErrorTmDetFinLlamada = false;
        var isErrorPeriodoSpvRing = isErrorFiltroSpvRing = isErrorTReleaseBL = false;

        function checkRangeRT(element, minValue, maxValue) {
            var resource = returnResources();
            var errorMessage;
            var errorElement = 0;

            if (!element.value)
                errorElement = 1;
            else if (element.value < minValue || element.value > maxValue)
                errorElement = 2;
            else if (isNaN(element.value))
                errorElement = 3;

            if (errorElement != 0) {
                switch (element.id) {
                    case 'ContentPlaceHolder1_TxtTmLlamadaEntrante':
                        isErrorTmMaxEntry = true;
                        if (errorElement == 1)
                            errorMessage = resource.ErrorTmLlamadaEntranteValue;
                        else if (errorElement == 2)
                            errorMessage = resource.ErrorTmLlamadaEntranteRange;
                        else
                            errorMessage = resource.ErrorTmLlamadaEntranteNaN;
                        break;
                    case 'ContentPlaceHolder1_TxtTmSupervisionOptions':
                        isErrorTmSupervisionOptions = true;
                        if (errorElement == 1)
                            errorMessage = resource.ErrorTmSupervisionOptionsValue;
                        else if (errorElement == 2)
                            errorMessage = resource.ErrorTmSupervisionOptionsRange;
                        else
                            errorMessage = resource.ErrorTmSupervisionOptionsNaN;
                        break;
                    case 'ContentPlaceHolder1_TxtTmDeteccionCallerId':
                        isErrorTmDeteccionCallerId = true;
                        if (errorElement == 1)
                            errorMessage = resource.ErrorTmDeteccionCallerIdValue;
                        else if (errorElement == 2)
                            errorMessage = resource.ErrorTmDeteccionCallerIdRange;
                        else
                            errorMessage = resource.ErrorTmDeteccionCallerIdNaN;
                        break;
                    case 'ContentPlaceHolder1_TxtTmDetFinLlamada':
                        isErrorTmDetFinLlamada = true;
                        if (errorElement == 1)
                            errorMessage = resource.ErrorTmDetFinLlamadaValue;
                        else if (errorElement == 2)
                            errorMessage = resource.ErrorTmDetFinLlamadaRange;
                        else
                            errorMessage = resource.ErrorTmDetFinLlamadaNaN;
                        break;
                    case 'ContentPlaceHolder1_TxtPeriodoSpvRing':
                        isErrorPeriodoSpvRing = true;
                        if (errorElement == 1)
                            errorMessage = resource.ErrorPeriodoSpvRingValue;
                        else if (errorElement == 2)
                            errorMessage = resource.ErrorPeriodoSpvRingRange;
                        else
                            errorMessage = resource.ErrorPeriodoSpvRingNaN;
                        break;
                    case 'ContentPlaceHolder1_TxtFiltroSpvRing':
                        isErrorFiltroSpvRing = true;
                        if (errorElement == 1)
                            errorMessage = resource.ErrorFiltroSpvRingValue;
                        else if (errorElement == 2)
                            errorMessage = resource.ErrorFiltroSpvRingRange;
                        else
                            errorMessage = resource.ErrorFiltroSpvRingNaN;
                        break;
                    case 'ContentPlaceHolder1_TxtTReleaseBL':
                        isErrorTReleaseBL = true;
                        if (errorElement == 1)
                            errorMessage = resource.ErrorTReleaseBLValue;
                        else if (errorElement == 2)
                            errorMessage = resource.ErrorTReleaseBLRange;
                        else
                            errorMessage = resource.ErrorTReleaseBLNaN;
                        break;
                }
                alertify.error(errorMessage);
                element.value = "";
            }
            else {
                switch (element.id) {
                    case 'ContentPlaceHolder1_TxtTmLlamadaEntrante':
                        isErrorTmMaxEntry = false;
                        break;
                    case 'ContentPlaceHolder1_TxtTmSupervisionOptions':
                        isErrorTmSupervisionOptions = false;
                        break;
                    case 'ContentPlaceHolder1_TxtTmDeteccionCallerId':
                        isErrorTmDeteccionCallerId = false;
                        break;
                    case 'ContentPlaceHolder1_TxtTmDetFinLlamada':
                        isErrorTmDetFinLlamada = false;
                        break;
                    case 'ContentPlaceHolder1_TxtPeriodoSpvRing':
                        isErrorPeriodoSpvRing = false;
                        break;
                    case 'ContentPlaceHolder1_TxtFiltroSpvRing':
                        isErrorFiltroSpvRing = false;
                        break;
                    case 'ContentPlaceHolder1_TxtTReleaseBL':
                        isErrorTReleaseBL = false;
                        break;
                }
            }
            if (isErrorTmMaxEntry || isErrorTmSupervisionOptions || isErrorTmDeteccionCallerId || isErrorTmDetFinLlamada
                || isErrorPeriodoSpvRing || isErrorFiltroSpvRing || isErrorTReleaseBL)
                $('#ContentPlaceHolder1_BtAceptar').prop("disabled", true);
            else
                $('#ContentPlaceHolder1_BtAceptar').prop("disabled", false);
        }

        function returnResources() {
            var resources;
            switch (navigator.language) {
                case "es":
                    resources = {
                        ErrorTmLlamadaEntranteRange: '<%= Resources.Espaniol.ErrorTmLlamadaEntranteRange %>',
                        ErrorTmLlamadaEntranteValue: '<%= Resources.Espaniol.ErrorTmLlamadaEntranteValue %>',
                        ErrorTmLlamadaEntranteNaN: '<%= Resources.Espaniol.ErrorTmLlamadaEntranteNaN %>',
                        ErrorTmSupervisionOptionsRange: '<%= Resources.Espaniol.ErrorTmSupervisionOptionsRange %>',
                        ErrorTmSupervisionOptionsValue: '<%= Resources.Espaniol.ErrorTmSupervisionOptionsValue %>',
                        ErrorTmSupervisionOptionsNaN: '<%= Resources.Espaniol.ErrorTmSupervisionOptionsNaN %>',
                        ErrorTmDeteccionCallerIdRange: '<%= Resources.Espaniol.ErrorTmDeteccionCallerIdRange %>',
                        ErrorTmDeteccionCallerIdValue: '<%= Resources.Espaniol.ErrorTmDeteccionCallerIdValue %>',
                        ErrorTmDeteccionCallerIdNaN: '<%= Resources.Espaniol.ErrorTmDeteccionCallerIdNaN %>',
                        ErrorTmDetFinLlamadaRange: '<%= Resources.Espaniol.ErrorTmDetFinLlamadaRange %>',
                        ErrorTmDetFinLlamadaValue: '<%= Resources.Espaniol.ErrorTmDetFinLlamadaValue %>',
                        ErrorTmDetFinLlamadaNaN: '<%= Resources.Espaniol.ErrorTmDetFinLlamadaNaN %>',
                        ErrorPeriodoSpvRingRange: '<%= Resources.Espaniol.ErrorPeriodoSpvRingRange %>',
                        ErrorPeriodoSpvRingValue: '<%= Resources.Espaniol.ErrorPeriodoSpvRingValue %>',
                        ErrorPeriodoSpvRingNaN: '<%= Resources.Espaniol.ErrorPeriodoSpvRingNaN %>',
                        ErrorFiltroSpvRingRange: '<%= Resources.Espaniol.ErrorFiltroSpvRingRange %>',
                        ErrorFiltroSpvRingValue: '<%= Resources.Espaniol.ErrorFiltroSpvRingValue %>',
                        ErrorFiltroSpvRingNaN: '<%= Resources.Espaniol.ErrorFiltroSpvRingNaN %>',
                        ErrorTReleaseBLRange: '<%= Resources.Espaniol.ErrorTReleaseBLRange %>',
                        ErrorTReleaseBLValue: '<%= Resources.Espaniol.ErrorTReleaseBLValue %>',
                        ErrorTReleaseBLNaN: '<%= Resources.Espaniol.ErrorTReleaseBLNaN %>'
                    }
                    break;
                case "en-us":
                case "en-gb":
                case "en-US":
                case "en-GB":
                case "en":
                    resources = {
                        ErrorTmLlamadaEntranteRange: '<%= Resources.English.ErrorTmLlamadaEntranteRange %>',
                        ErrorTmLlamadaEntranteValue: '<%= Resources.English.ErrorTmLlamadaEntranteValue %>',
                        ErrorTmLlamadaEntranteNaN: '<%= Resources.English.ErrorTmLlamadaEntranteNaN %>',
                        ErrorTmSupervisionOptionsRange: '<%= Resources.English.ErrorTmSupervisionOptionsRange %>',
                        ErrorTmSupervisionOptionsValue: '<%= Resources.English.ErrorTmSupervisionOptionsValue %>',
                        ErrorTmSupervisionOptionsNan: '<%= Resources.English.ErrorTmSupervisionOptionsNaN %>',
                        ErrorTmDeteccionCallerIdRange: '<%= Resources.English.ErrorTmDeteccionCallerIdRange %>',
                        ErrorTmDeteccionCallerIdValue: '<%= Resources.English.ErrorTmDeteccionCallerIdValue %>',
                        ErrorTmDeteccionCallerIdNaN: '<%= Resources.English.ErrorTmDeteccionCallerIdNaN %>',
                        ErrorTmDetFinLlamadaRange: '<%= Resources.English.ErrorTmDetFinLlamadaRange %>',
                        ErrorTmDetFinLlamadaValue: '<%= Resources.English.ErrorTmDetFinLlamadaValue %>',
                        ErrorTmDetFinLlamadaNaN: '<%= Resources.English.ErrorTmDetFinLlamadaNaN %>',
                        ErrorPeriodoSpvRingRange: '<%= Resources.English.ErrorPeriodoSpvRingRange %>',
                        ErrorPeriodoSpvRingValue: '<%= Resources.English.ErrorPeriodoSpvRingValue %>',
                        ErrorPeriodoSpvRingNaN: '<%= Resources.English.ErrorPeriodoSpvRingNaN %>',
                        ErrorFiltroSpvRingRange: '<%= Resources.English.ErrorFiltroSpvRingRange %>',
                        ErrorFiltroSpvRingValue: '<%= Resources.English.ErrorFiltroSpvRingValue %>',
                        ErrorFiltroSpvRingNaN: '<%= Resources.English.ErrorFiltroSpvRingNaN %>',
                        ErrorTReleaseBLRange: '<%= Resources.English.ErrorTReleaseBLRange %>',
                        ErrorTReleaseBLValue: '<%= Resources.English.ErrorTReleaseBLValue %>',
                        ErrorTReleaseBLNaN: '<%= Resources.English.ErrorTReleaseBLNaN %>'
                    }
                    break;
                case "fr-FR":
                case "fr-fr":
                case "fr":
                    break;
            }
            return resources;
        }
    </script>
    
	 <asp:ListBox ID="ListBox1" runat="server" 
		  OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" MouseHover="ListBox1_MouseHover" AutoPostBack="True"
			SkinID="MascaraListaElementosWide" meta:resourcekey="ListBox1Resource1">
    </asp:ListBox>

    <asp:DropDownList ID="FiltroBusquedaTF" runat="server" AutoPostBack="True" Style="z-index: 100; left: 50px; width:172px; 
        position: absolute; top: 475px"
            OnSelectedIndexChanged="FiltroBusquedaTF_SelectedIndexChanged" SkinID="MascaraFiltroBusqueda" 
            meta:resourcekey="FiltroBusquedaTFResource">
                <asp:ListItem Value="0" meta:resourcekey="ListItemFiltroBusquedaTFResource0">Seleccione Filtro:</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="ListItemFiltroBusquedaTFResource1">Mostrar Todos</asp:ListItem>
                <asp:ListItem Value="2" meta:resourcekey="ListItemFiltroBusquedaTFResource2">Buscar por Nombre</asp:ListItem>
                <asp:ListItem Value="3" meta:resourcekey="ListItemFiltroBusquedaTFResource3">Buscar por Tipo</asp:ListItem>
                <asp:ListItem Value="4" meta:resourcekey="ListItemFiltroBusquedaTFResource4">Orden Alfabético (A-Z)</asp:ListItem>
                <asp:ListItem Value="5" meta:resourcekey="ListItemFiltroBusquedaTFResource5">Orden Alfabético (Z-A)</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="FiltroTipoTF" runat="server" AutoPostBack="True" visible="false" Style="z-index: 100; left: 50px; width:172px; 
        position: absolute; top: 500px"
            OnSelectedIndexChanged="FiltroTipoTF_SelectedIndexChanged" SkinID="MascaraFiltroTipo" 
            meta:resourcekey="FiltroTipoTFResource">
                <asp:ListItem Value="0" meta:resourcekey="ListItemFiltroTipoTFResource0">Tipo Interface:</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="ListItemFiltroTipoTFResource1">AB</asp:ListItem>
                <asp:ListItem Value="2" meta:resourcekey="ListItemFiltroTipoTFResource2">ATS-N5</asp:ListItem>
                <asp:ListItem Value="3" meta:resourcekey="ListItemFiltroTipoTFResource3">ATS-QSIG</asp:ListItem>
                <asp:ListItem Value="4" meta:resourcekey="ListItemFiltroTipoTFResource4">ATS-R2</asp:ListItem>
                <asp:ListItem Value="5" meta:resourcekey="ListItemFiltroTipoTFResource5">BC</asp:ListItem>
                <asp:ListItem Value="6" meta:resourcekey="ListItemFiltroTipoTFResource6">BL</asp:ListItem>
                <asp:ListItem Value="7" meta:resourcekey="ListItemFiltroTipoTFResource7">E&M Marcación</asp:ListItem>
                <asp:ListItem Value="8" meta:resourcekey="ListItemFiltroTipoTFResource8">E&M PP</asp:ListItem>
                <asp:ListItem Value="9" meta:resourcekey="ListItemFiltroTipoTFResource9">ISDN 2B+D</asp:ListItem>
                <asp:ListItem Value="10" meta:resourcekey="ListItemFiltroTipoTFResource10">ISDN 30B+D</asp:ListItem>
                <asp:ListItem Value="11" meta:resourcekey="ListItemFiltroTipoTFResource11">LCEN</asp:ListItem>
    </asp:DropDownList>
    <asp:TextBox ID="FiltroNombreTF" runat="server" visible="false"
        Style="z-index: 100; left: 50px; width:172px; position: absolute; top: 500px" meta:resourcekey="FiltroNombreTFResource">
        </asp:TextBox>
    <asp:Button ID="ButtonFiltroBuscarTF" runat="server" Style="z-index: 105; left: 55px; position: absolute;
        top: 535px" Text="Buscar" Width="100px" Visible="False" UseSubmitBehavior="true" 
		  OnClick="ButtonFiltroBuscarTF_Click" meta:resourcekey="ButtonFiltroBuscarTFResource" />

    <asp:Label ID="Label7" runat="server" Text="GESTIÓN DE RECURSOS  TELEFONÍA" 
		  CssClass="labelPagina" meta:resourcekey="Label7Resource1"></asp:Label>

    <asp:Label ID="LError" runat="server" ForeColor="Red" Style="left: 265px; position: absolute; text-align:center;
        top: 497px; z-index: 102;" Width="500px" Height="45px" 
		  meta:resourcekey="LErrorResource1"></asp:Label>
 
    <asp:LinkButton ID="BtNuevo" runat="server" OnClick="BtNuevo_Click" 
		  CausesValidation="False" Text="Nuevo" SkinID="MascaraBotonNuevo" 
		  meta:resourcekey="BtNuevoResource1"/>
    <asp:LinkButton ID="BtModificar" runat="server" OnClick="BtModificar_Click" 
		  CausesValidation="False" Text="Modificar" SkinID="MascaraBotonModificar" 
		  meta:resourcekey="BtModificarResource1"/>
    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 105; left: 366px; position: absolute;
        top: 465px" Text="Aceptar" Width="100px" Visible="False" UseSubmitBehavior="true" 
		  OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
      <ajaxToolKit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
      </ajaxToolKit:ConfirmButtonExtender>
    <asp:LinkButton ID="BtCancelar" runat="server" OnClick="BtCancelar_Click" 
		  CausesValidation="False" Text="Cancelar" SkinID="MascaraBotonCancelar" 
		  meta:resourcekey="BtCancelarResource1"/>
      <ajaxToolKit:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
      </ajaxToolKit:ConfirmButtonExtender>
    <asp:LinkButton ID="BtEliminar" runat="server" OnClick="BtEliminar_Click" 
		  CausesValidation="False" Text="Eliminar" SkinID="MascaraBotonEliminar" 
		  meta:resourcekey="BtEliminarResource1"/>
      <ajaxToolKit:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
      </ajaxToolKit:ConfirmButtonExtender>

    <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Style="z-index: 108; left: 275px; position: absolute; top: 58px;"
         Height="199px" Width="540px" meta:resourcekey="Panel1Resource1">
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="ViewParamGenerales" runat="server">
                <asp:Label ID="Label5" runat="server" Style="z-index: 100; left: 14px; position: absolute; top: 12px"
                    Text="Id. Recurso:" meta:resourcekey="Label5Resource1"></asp:Label>
                <asp:TextBox ID="TxtIdRecurso" runat="server" Style="z-index: 101; left: 14px; position: absolute; top: 32px; width: 250px;" MaxLength="32"
                    Enabled="False" meta:resourcekey="TxtIdRecursoResource1"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtIdRecurso"
                    ErrorMessage="El campo Id. Recurso no puede estar vacio." Style="z-index: 102; left: 175px; position: absolute; top: 32px"
                    meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="TxtIdRecurso"
                    ErrorMessage="Carácter inválido" Style="z-index: 141; left: 145px; position: absolute; top: 32px"
                    ValidationExpression="^[\w-.&=+$,;?/%_!~*'()]*$"
                    meta:resourcekey="RequiredFieldValidator7Resource1">*</asp:RegularExpressionValidator>
                <!--
                <asp:Label ID="Label2" runat="server" Style="z-index: 103; left: 14px; position: absolute; top: 58px"
                    Text="Tipo:" meta:resourcekey="Label2Resource1"></asp:Label>
                <asp:DropDownList ID="DListTipo" runat="server" Style="z-index: 104; left: 14px; position: absolute; top: 78px"
                    Width="156px" Enabled="False" class="select"
                    meta:resourcekey="DListTipoResource1">
                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource3">Audio RX</asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource4">Audio TX</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource5" Selected="True">Audio RX TX</asp:ListItem>
                </asp:DropDownList>
                -->
                <asp:Label ID="Label6" runat="server" Style="z-index: 105; left: 14px; position: absolute; top: 68px"
                    Text="Interface:" meta:resourcekey="Label6Resource1"></asp:Label>
                <asp:DropDownList ID="DListInterface" runat="server" Style="z-index: 106; left: 14px; position: absolute; top: 88px"
                    Width="156px" AutoPostBack="True" class="select"
                    OnSelectedIndexChanged="DListInterface_SelectedIndexChanged" Enabled="False"
                    meta:resourcekey="DListInterfaceResource1" >
                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource6">LCEN</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource7">BC</asp:ListItem>
                    <asp:ListItem Value="3" meta:resourcekey="ListItemResource8">BL</asp:ListItem>
                    <asp:ListItem Value="4" meta:resourcekey="ListItemResource9">AB</asp:ListItem>
                    <asp:ListItem Value="5" meta:resourcekey="ListItemResource10">ATS-R2</asp:ListItem>
                    <asp:ListItem Value="6" meta:resourcekey="ListItemResource11">ATS-N5</asp:ListItem>
                    <asp:ListItem Value="7" meta:resourcekey="ListItemResource12">ATS-QSIG</asp:ListItem>
                    <asp:ListItem Value="8" meta:resourcekey="ListItemResource13">ISDN 2B+D</asp:ListItem>
                    <asp:ListItem Value="9" meta:resourcekey="ListItemResource14">ISDN 30B+D</asp:ListItem>
                    <asp:ListItem Value="13" meta:resourcekey="ListItemResource21">E&M PP</asp:ListItem>
                    <asp:ListItem Value="14" meta:resourcekey="ListItemResource22">E&M Marcación</asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="LBSupervisionOptions" runat="server" Style="z-index: 201; left: 300px; position: absolute; top: 80px" 
                    Visible="False" meta:resourcekey="LBSupervisionOptionsResource"></asp:Label>
                <asp:DropDownList ID="DDLSupervisionOptions" runat="server" OnSelectedIndexChanged="DDLElement_SelectedIndexChanged" 
                        AutoPostBack="True" Style="z-index: 124; left: 475px; position: absolute; top: 92px" 
                        Visible="true" Width="45px" AppendDataBoundItems="True" class="select" Enabled="False" 
                        meta:resourcekey="DDLSupervisionOptionsResource">
                            <asp:ListItem Value="1" meta:resourcekey="DDLYesItemResource">Si</asp:ListItem>
                            <asp:ListItem Value="0" meta:resourcekey="DDLNoItemResource">No</asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="LDestino" runat="server" Style="left: 14px; position: absolute; top: 128px; z-index: 111;"
                    Text="Destino: " Width="53px" Visible="False" meta:resourcekey="LDestinoResource1"></asp:Label>
                
                <asp:TextBox ID="TBDestino" runat="server" Enabled="False" ReadOnly="True" Style="z-index: 116; left: 14px; position: absolute; top: 148px"
                    Visible="False" Width="156px"
                    meta:resourcekey="TBDestinoResource1"></asp:TextBox>
                <asp:Label ID="LBTmSupervisionOptions" runat="server" Style="z-index: 201; left: 300px; position: absolute; top: 130px"
                    Visible="False" meta:resourcekey="LBTmSupervisionOptionsResource"></asp:Label>
                <asp:TextBox ID="TxtTmSupervisionOptions" runat="server" 
                    Enabled="False" Style="z-index: 124; left: 475px; position: absolute; top: 142px"
                    Width="45px" Visible="False" meta:resourcekey="TxtTmSupervisionOptionsResource">130</asp:TextBox>

                <asp:Label ID="LLado" runat="server" Style="left: 333px; position: absolute; top: 12px; z-index: 107;"
                    Text="Lado" Visible="False" meta:resourcekey="LLadoResource1"></asp:Label>
                <asp:DropDownList ID="DDLLado" runat="server" Style="left: 333px; position: absolute; top: 32px; z-index: 108;"
                    Visible="False" Width="56px" Enabled="False" class="select"
                    meta:resourcekey="DDLLadoResource1">
                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource15">A</asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource16">B</asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="LblModo" runat="server" Text="Modo: "
                    Style="position: absolute; top: 58px; left: 333px;" Visible="False"
                    meta:resourcekey="LblModoResource1"></asp:Label>
                <asp:DropDownList ID="DDLModo" runat="server" Enabled="False" class="select"
                    Style="position: absolute; top: 78px; left: 333px;" Visible="False"
                    meta:resourcekey="DDLModoResource1">
                    <asp:ListItem Value="Usuario" Text="Usuario"
                        meta:resourcekey="ListItemResource1"></asp:ListItem>
                    <asp:ListItem Value="Red" Text="Red" meta:resourcekey="ListItemResource2"></asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="LTroncal" runat="server" Style="left: 333px; position: absolute; top: 106px; z-index: 109;"
                    Text="Troncal:" Visible="False" meta:resourcekey="LTroncalResource1"></asp:Label>
                <asp:DropDownList ID="DDLTroncal" runat="server" AppendDataBoundItems="True" Style="left: 333px; position: absolute; top: 126px; z-index: 110;"
                    Visible="False" Width="190px" class="select"
                    Enabled="False" meta:resourcekey="DDLTroncalResource1">
                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource17">&lt;Selecciona Troncal&gt;</asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="LRed" runat="server" Style="left: 300px; position: absolute; top: 106px; z-index: 113;"
                    Text="Red:" Visible="False" Width="1px" meta:resourcekey="LRedResource1"></asp:Label>
                <asp:DropDownList ID="DDLRed" runat="server" AppendDataBoundItems="True" Style="left: 300px; position: absolute; top: 126px; z-index: 114;"
                    Visible="False" Width="220px" class="select"
                    Enabled="False" OnSelectedIndexChanged="DDLRed_SelectedIndexChanged" AutoPostBack="True" meta:resourcekey="DDLRedResource1">
                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource18">&lt;Selecciona red&gt;</asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="LblTipoEM" runat="server" Style="left: 333px; position: absolute; top: 152px; z-index: 111; width:auto"
                    Text="Tipo E&M: " Visible="False" meta:resourcekey="LTipoEM"></asp:Label>
                <asp:DropDownList ID="DDLTipoEM" runat="server" Enabled="False" ReadOnly="True" Style="z-index: 116; left: 333px; position: absolute; top: 172px"
                    Visible="False" class="select"
                    meta:resourcekey="DDLLadoResource1">
                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource23">I</asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource24">II</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource25">III</asp:ListItem>
                    <asp:ListItem Value="3" meta:resourcekey="ListItemResource26">IV</asp:ListItem>
                    <asp:ListItem Value="4" meta:resourcekey="ListItemResource27">V</asp:ListItem>
                </asp:DropDownList>
            </asp:View>

            <asp:View ID="ViewParamVoip" runat="server">
                <asp:Label ID="Label4" runat="server" Style="z-index: 106; left: 14px; position: absolute; top: 12px" Visible="false"
                    Text="Servidor SIP:" meta:resourcekey="Label4Resource1"></asp:Label>
                <asp:TextBox ID="TxtServidorSIP" runat="server" Style="z-index: 105; left: 14px; position: absolute; top: 32px" Visible="false"
                    Width="155px" Enabled="False" meta:resourcekey="TxtServidorSIPResource1"></asp:TextBox>

                <asp:Label ID="Label12" runat="server" Style="left: 14px; position: absolute; top: 58px"
                    Text="Tamaño paquete RTP (msg.)" meta:resourcekey="Label12Resource1"></asp:Label>
                <asp:TextBox ID="TBTamanoPaquete" runat="server" Style="left: 14px; position: absolute; top: 78px"
                    Width="35px" Enabled="False" meta:resourcekey="TBTamanoPaqueteResource1">20</asp:TextBox>

                <asp:CheckBox ID="CheckGrabacionEd137" runat="server" Style="z-index: 115; left: 14px; position: absolute; top: 106px"
                    Text="Grabación ED-137" Enabled="False" Checked="True"
                    meta:resourcekey="CheckGrabacionEd137Resource1" />
                <asp:CheckBox ID="CheckDiffServ" runat="server" Style="z-index: 118; left: 14px; position: absolute; top: 126px"
                     Enabled="False" Visible="false"/>
                <asp:CheckBox ID="CBSupersionSilencio" runat="server" Style="left: 14px; position: absolute; top: 146px"
                    Text="Usar supresión silencio" Enabled="False" Visible="false"
                    meta:resourcekey="CBSupersionSilencioResource1" />

                <asp:Panel ID="Panel2" runat="server" GroupingText="Codec preferido" Height="72px"
                    Style="left: 14px; position: absolute; top: 171px" Width="172px" Enabled="False" Visible="false"
                    ForeColor="RoyalBlue" meta:resourcekey="Panel2Resource1">
                    <asp:RadioButton ID="RBCodecA" runat="server" Checked="True" GroupName="Codec"
                        Text="G.711 ley A" ForeColor="Black" meta:resourcekey="RBCodecAResource1" />
                    <br />
                    <asp:RadioButton ID="RBCodecMu" runat="server" GroupName="Codec"
                        Text="G.711 ley µ" ForeColor="Black" meta:resourcekey="RBCodecMuResource1" />
                </asp:Panel>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TBTamanoPaquete"
                    ErrorMessage="El campo Tamaño paquete RTP no puede estar vacio." Style="z-index: 179; left: 250px; position: absolute; top: 57px"
                    meta:resourcekey="RequiredFieldValidator3Resource1">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="TBTamanoPaquete"
                    ErrorMessage="El campo Tamaño paquete RTP debe ser numérico." Operator="DataTypeCheck"
                    Style="z-index: 189; left: 268px; position: absolute; top: 57px" Type="Integer"
                    meta:resourcekey="CompareValidator1Resource1">*</asp:CompareValidator>
            </asp:View>

            <asp:View ID="ViewParamAudio" runat="server">
                <asp:Panel ID="PanelParametrosTx" runat="server" GroupingText="Tx  " 
                    Style="z-index:auto; left: 3px; position: absolute; top: 9px; height:168px;" Width="257px" Enabled="False"
                    ForeColor="RoyalBlue" meta:resourcekey="PanelParametrosTxResource1">
                    <asp:RadioButton ID="RBAGCTx" runat="server" GroupName="GananciaTx" Text="AGC"
                        ForeColor="Black" meta:resourcekey="RBAGCTxResource1"  OnCheckedChanged="TxParamAudio_Changed" AutoPostBack="True"  /><br />
                    <asp:RadioButton ID="RBGananciaTx" runat="server" GroupName="GananciaTx"
                        Text="Ganancia" Checked="True" ForeColor="Black"
                        meta:resourcekey="RBGananciaTxResource1"  OnCheckedChanged="TxParamAudio_Changed" AutoPostBack="True" />
                    <br /><br /><br /><br /><br />
                    <asp:Label ID="Label3" runat="server" Text="Nivel de salida (dBm):" Style="z-index:auto; left: 20px; position: absolute; top: 95px" Width="277px"
                        ForeColor="Black" meta:resourcekey="Label3Resource1" ></asp:Label>
                    <asp:TextBox ID="TBGananciaTx" runat="server"  Style="z-index:auto; left: 20px; position: absolute; top: 115px; width:auto" 
                        meta:resourcekey="TBGananciaTxResource1">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                        ControlToValidate="TBGananciaTx" Style="z-index:205; left: 200px; position: absolute; top: 118px; width:auto" 
                        meta:resourcekey="RequiredFieldValidator5Resource1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="TBGananciaTx"
                        Operator="DataTypeCheck" Type="Double" Style="z-index:206; left: 200px; position: absolute; top: 118px; width:auto" 
                        meta:resourcekey="CompareValidator3Resource1">*</asp:CompareValidator>
                    <asp:RangeValidator ID="RVTBGananciaTx" runat="server" ControlToValidate="TBGananciaTx"
					    ErrorMessage="TX-Ganancia AD: el valor debe estar comprendido entre -13,4 y 1,2" MaximumValue="1,2"
					    MinimumValue="-13,4" Style="z-index: 207; left: 200px; position: absolute; top: 118px"
					    Type="Double" meta:resourcekey="RVTBGananciaTxResource1">*</asp:RangeValidator>
                     <br />
                </asp:Panel>

                <asp:Panel ID="PanelParametrosRx" runat="server" GroupingText="Rx  " ForeColor="RoyalBlue"
                    Height="68px" Style="left: 261px; position: absolute; top: 9px" Width="257px"
                    Enabled="False" meta:resourcekey="PanelParametrosRxResource1">
                    <asp:RadioButton ID="RBAGCRx" runat="server" GroupName="GananciaRx" Text="AGC"
                        ForeColor="Black" meta:resourcekey="RBAGCRxResource1" OnCheckedChanged="RxParamAudio_Changed" AutoPostBack="True"  /><br />
                    <asp:RadioButton ID="RBGananciaRx" runat="server" GroupName="GananciaRx"
                        Text="Ganancia" Checked="True" ForeColor="Black"
                        meta:resourcekey="RBGananciaRxResource1" OnCheckedChanged="RxParamAudio_Changed" AutoPostBack="True"  />
                    <br /> <br /> <br /> <br /> <br />
                    <asp:Label ID="Label10" runat="server" Text="Nivel de salida (dBm):" Style="left: 20px; position: absolute; top: 95px"
                        ForeColor="Black" meta:resourcekey="Label10Resource1"></asp:Label>
                    <asp:TextBox ID="TBGananciaRx" runat="server" Width="47px" Style="left: 20px; position: absolute; top: 115px; width:auto" 
                        meta:resourcekey="TBGananciaRxResource1">0</asp:TextBox>&nbsp;
					  <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                          ControlToValidate="TBGananciaRx" Style="z-index: 208; left: 200px; position: absolute; top: 118px"
                          meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="TBGananciaRx"
                        Operator="DataTypeCheck" Type="Double" Width="17px"  Style="z-index: 209; left: 200px; position: absolute; top: 120px"
                        meta:resourcekey="CompareValidator2Resource1">*</asp:CompareValidator>
                    <asp:RangeValidator ID="RVTBGananciaRx" runat="server" ControlToValidate="TBGananciaRx"
					    ErrorMessage="RX-Ganancia DA: el valor debe estar comprendido entre -24,3 y 1,10" MaximumValue="1,10"
					    MinimumValue="-24,3" Style="z-index: 210; left: 200px; position: absolute; top: 120px"
					    Type="Double" meta:resourcekey="RVTBGananciaRxResource1">*</asp:RangeValidator>
                     <br />
                </asp:Panel>
            </asp:View>

            <asp:View ID="ViewFuncionalidadLCE" runat="server">
                <asp:Label ID="Label19" runat="server" Style="z-index: 175; left: 15px; position: absolute; top: 20px"
                    Text="Umbral tono Rx:" Visible="false" meta:resourcekey="Label19Resource1"></asp:Label>
                <asp:TextBox ID="TxtUmbralSQ" runat="server" Enabled="False" Style="z-index: 177; left: 140px; position: absolute; top: 20px"
                    Width="47px" Visible="false" 
                    meta:resourcekey="TxtUmbralSQResource1">-50</asp:TextBox>
                <asp:CompareValidator ID="CompareValidator21" runat="server" ControlToValidate="TxtUmbralSQ"
                    ErrorMessage="El campo Umbral tono SQ debe ser numérico." Operator="DataTypeCheck"
                    Style="z-index: 219; left: 188px; position: absolute; top: 21px" Type="Integer"
                    meta:resourcekey="CompareValidator21Resource1">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtUmbralSQ"
                    ErrorMessage="El campo Umbral tono SQ no puede estar vacio." Style="z-index: 138; left: 207px; position: absolute; top: 21px"
                    meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>
                <asp:Label ID="Label20" runat="server" Style="z-index: 176; left: 15px; position: absolute; top: 50px"
                    Text="Nivel tono Tx:"  Visible="false" meta:resourcekey="Label20Resource1"></asp:Label>
                <asp:TextBox ID="TxtUmbralPTT" runat="server" Enabled="False" Style="z-index: 178; left: 140px; position: absolute; top: 50px"
                    Width="47px" Visible="false" 
                    meta:resourcekey="TxtUmbralPTTResource1">-10</asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TxtUmbralPTT"
                    ErrorMessage="El campo Umbral Tono PTT no puede estar vacio." Style="z-index: 139; left: 456px; position: absolute; top: 15px"
                    meta:resourcekey="RequiredFieldValidator6Resource1">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator22" runat="server" ControlToValidate="TxtUmbralPTT"
                    ErrorMessage="El campo Umbral tono PTT debe ser numérico." Operator="DataTypeCheck"
                    Style="z-index: 221; left: 437px; position: absolute; top: 21px" Type="Integer"
                    meta:resourcekey="CompareValidator22Resource1">*</asp:CompareValidator>

                <asp:TextBox ID="TxtLongRafagas" runat="server" Enabled="False" Style="z-index: 121; left: 356px; position: absolute; top: 425px"
                    Visible="False" Width="45px"
                    meta:resourcekey="TxtLongRafagasResource1"></asp:TextBox>
                
                
                
                
                
                
                
                <asp:Label ID="Label16" runat="server" Style="z-index: 120; left: 75px; position: absolute; top: 398px"
                    Text="Timeout de Refresco de estados:" Visible="False" Width="202px"
                    meta:resourcekey="Label16Resource1"></asp:Label>
                <asp:TextBox ID="TxtRefresco" runat="server" Enabled="False" Style="z-index: 167; left: 255px; position: absolute; top: 360px"
                    Visible="False" Width="47px"
                    meta:resourcekey="TxtRefrescoResource1"></asp:TextBox>
                &nbsp; &nbsp;
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="TxtT1"
                     ErrorMessage="El campo T1 no puede estar vacio." Style="z-index: 143; left: 104px; position: absolute; top: 68px"
                     meta:resourcekey="RequiredFieldValidator9Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="TxtT2"
                    ErrorMessage="El campo T2 no puede estar vacio." Style="z-index: 144; left: 104px; position: absolute; top: 103px"
                    meta:resourcekey="RequiredFieldValidator10Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="TxtT3"
                    ErrorMessage="El campo T3 no puede estar vacio." Style="z-index: 145; left: 104px; position: absolute; top: 138px"
                    meta:resourcekey="RequiredFieldValidator11Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="TxtT4"
                    ErrorMessage="El campo T4 no puede estar vacio." Style="z-index: 146; left: 104px; position: absolute; top: 173px"
                    meta:resourcekey="RequiredFieldValidator12Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="TxtT5"
                    ErrorMessage="El campo T5 no puede estar vacio." Style="z-index: 147; left: 104px; position: absolute; top: 208px"
                    meta:resourcekey="RequiredFieldValidator13Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="TxtT1Max"
                    ErrorMessage="El campo T1Max no puede estar vacio." Style="z-index: 148; left: 258px; position: absolute; top: 68px"
                    meta:resourcekey="RequiredFieldValidator14Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="TxtT2Max"
                    ErrorMessage="El campo T2Max no puede estar vacio." Style="z-index: 149; left: 258px; position: absolute; top: 105px"
                    meta:resourcekey="RequiredFieldValidator15Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="TxtT4Max"
                    ErrorMessage="El campo T4Max no puede estar vacio." Style="z-index: 150; left: 258px; position: absolute; top: 142px"
                    meta:resourcekey="RequiredFieldValidator16Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="TxtT5Max"
                    ErrorMessage="El campo T5Max no puede estar vacio." Style="z-index: 151; left: 258px; position: absolute; top: 173px"
                    meta:resourcekey="RequiredFieldValidator17Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="TxtT6"
                    ErrorMessage="El campo T6 no puede estar vacio." Style="z-index: 152; left: 395px; position: absolute; top: 68px"
                    meta:resourcekey="RequiredFieldValidator18Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="TxtT8"
                    ErrorMessage="El campo T8 no puede estar vacio." Style="z-index: 153; left: 395px; position: absolute; top: 103px"
                    meta:resourcekey="RequiredFieldValidator19Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="TxtT9"
                    ErrorMessage="El campo T9 no puede estar vacio." Style="z-index: 154; left: 395px; position: absolute; top: 138px"
                    meta:resourcekey="RequiredFieldValidator20Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="TxtT10"
                    ErrorMessage="El campo T10 no puede estar vacio." Style="z-index: 155; left: 395px; position: absolute; top: 173px"
                    meta:resourcekey="RequiredFieldValidator21Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="TxtT11"
                    ErrorMessage="El campo T11 no puede estar vacio." Style="z-index: 156; left: 395px; position: absolute; top: 208px"
                    meta:resourcekey="RequiredFieldValidator22Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="TxtT12"
                    ErrorMessage="El campo T12 no puede estar vacio." Style="z-index: 157; left: 395px; position: absolute; top: 246px"
                    meta:resourcekey="RequiredFieldValidator23Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="TxtT6Max"
                    ErrorMessage="El campo T6Max no puede estar vacio." Style="z-index: 158; left: 541px; position: absolute; top: 68px"
                    meta:resourcekey="RequiredFieldValidator24Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="TxtT8Max"
                    ErrorMessage="El campo T8Max no puede estar vacio." Style="z-index: 159; left: 541px; position: absolute; top: 103px"
                    meta:resourcekey="RequiredFieldValidator25Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="TxtT9Max"
                    ErrorMessage="El campo T9Max no puede estar vacio." Style="z-index: 160; left: 541px; position: absolute; top: 138px"
                    meta:resourcekey="RequiredFieldValidator26Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="TxtT10Max"
                    ErrorMessage="El campo T10Max no puede estar vacio." Style="z-index: 161; left: 541px; position: absolute; top: 173px"
                    meta:resourcekey="RequiredFieldValidator27Resource1">*</asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="TxtT11Max"
                    ErrorMessage="El campo T11Max no puede estar vacio." Style="z-index: 162; left: 541px; position: absolute; top: 208px"
                    meta:resourcekey="RequiredFieldValidator28Resource1">*</asp:RequiredFieldValidator>
                
                
                
                
                &nbsp;
				 <asp:Label ID="Label1" runat="server" Style="z-index: 179; left: 61px; position: absolute; top: 426px"
                     Text="Longitud de ráfagas en transacciones de estado:" Visible="False"
                     meta:resourcekey="Label1Resource1"></asp:Label>
                <asp:Label ID="Label15" runat="server" Style="z-index: 180; left: 115px; position: absolute; top: 360px"
                    Text="Refresco de Estados:" Visible="False"
                    meta:resourcekey="Label15Resource1"></asp:Label>
                <asp:TextBox ID="TxtTimeoutRef" runat="server" Enabled="False" Style="z-index: 181; left: 278px; position: absolute; top: 397px"
                    Visible="False" Width="47px"
                    meta:resourcekey="TxtTimeoutRefResource1"></asp:TextBox>
                
                <!--VMG Ts-->
                <asp:Label ID="Label17" runat="server" Style="z-index: 182; left: 295px; position: absolute; top: 20px"
                    Text="T1:" meta:resourcekey="Label17Resource1"></asp:Label>
                <asp:TextBox ID="TxtT1" runat="server" Enabled="False" Style="z-index: 166; left: 330px; position: absolute; top: 20px"
                    Width="45px" meta:resourcekey="TxtT1Resource1">20</asp:TextBox>
                <asp:Label ID="Label21" runat="server" Style="z-index: 184; left: 295px; position: absolute; top: 50px"
                    Text="T2:" meta:resourcekey="Label21Resource1"></asp:Label>
                <asp:TextBox ID="TxtT2" runat="server" Enabled="False" Style="z-index: 165; left: 330px; position: absolute; top: 50px"
                    Width="45px" meta:resourcekey="TxtT2Resource1">20</asp:TextBox>
                <asp:Label ID="Label23" runat="server" Style="z-index: 201; left: 295px; position: absolute; top: 80px"
                    Text="T3:" meta:resourcekey="Label23Resource1"></asp:Label>
                <asp:TextBox ID="TxtT3" runat="server" Enabled="False" Style="z-index: 137; left: 330px; position: absolute; top: 80px"
                    Width="45px" meta:resourcekey="TxtT3Resource1">40</asp:TextBox>
                <asp:Label ID="Label24" runat="server" Style="z-index: 186; left: 295px; position: absolute; top: 110px"
                    Text="T4:" meta:resourcekey="Label24Resource1"></asp:Label>
                <asp:TextBox ID="TxtT5" runat="server" Enabled="False" Style="z-index: 135; left: 330px; position: absolute; top: 110px"
                    Width="45px" meta:resourcekey="TxtT5Resource1">60</asp:TextBox>
                <asp:Label ID="Label26" runat="server" Style="z-index: 188; left: 295px; position: absolute; top: 140px"
                    Text="T5:" meta:resourcekey="Label26Resource1"></asp:Label>
                <asp:TextBox ID="TxtT4" runat="server" Enabled="False" Style="z-index: 136; left: 330px; position: absolute; top: 140px"
                    Width="45px" meta:resourcekey="TxtT4Resource1">300</asp:TextBox>
                <asp:Label ID="Label28" runat="server" Style="z-index: 188; left: 295px; position: absolute; top: 170px"
                    Text="T6:" meta:resourcekey="Label28Resource1"></asp:Label>
                <asp:TextBox ID="TxtT6" runat="server" Enabled="False" Style="z-index: 136; left: 330px; position: absolute; top: 170px"
                    Width="45px" meta:resourcekey="TxtT6Resource1">5000</asp:TextBox>
                <asp:Label ID="Label30" runat="server" Style="z-index: 188; left: 295px; position: absolute; top: 200px"
                    Text="T8:" meta:resourcekey="Label30Resource1"></asp:Label>
                <asp:TextBox ID="TxtT8" runat="server" Enabled="False" Style="z-index: 136; left: 330px; position: absolute; top: 200px"
                    Width="45px" meta:resourcekey="TxtT8Resource1">80</asp:TextBox>
                <asp:Label ID="Label32" runat="server" Style="z-index: 188; left: 295px; position: absolute; top: 230px"
                    Text="T9:" meta:resourcekey="Label32Resource1"></asp:Label>
                <asp:TextBox ID="TxtT9" runat="server" Enabled="False" Style="z-index: 122; left: 330px; position: absolute; top: 230px"
                    Width="45px" meta:resourcekey="TxtT9Resource1">40</asp:TextBox>
                <asp:Label ID="Label34" runat="server" Style="z-index: 188; left: 295px; position: absolute; top: 260px"
                    Text="T10:" meta:resourcekey="Label34Resource1"></asp:Label>
                <asp:TextBox ID="TxtT10" runat="server" Enabled="False" Style="z-index: 122; left: 330px; position: absolute; top: 260px"
                    Width="45px" meta:resourcekey="TxtT10Resource1">20</asp:TextBox>
                <asp:Label ID="Label36" runat="server" Style="z-index: 198; left: 295px; position: absolute; top: 290px"
                    Text="T11:" meta:resourcekey="Label36Resource1"></asp:Label>
                <asp:TextBox ID="TxtT11" runat="server" Enabled="False" Style="z-index: 122; left: 330px; position: absolute; top: 290px"
                    Width="45px" meta:resourcekey="TxtT11Resource1">20</asp:TextBox>
                <asp:Label ID="Label38" runat="server" Style="z-index: 199; left: 295px; position: absolute; top: 320px"
                    Text="T12:" meta:resourcekey="Label38Resource1"></asp:Label>
                <asp:TextBox ID="TxtT12" runat="server" Enabled="False" Style="z-index: 122; left: 330px; position: absolute; top: 320px"
                    Width="45px" meta:resourcekey="TxtT10Resource1">200</asp:TextBox>
                
                <!--VMG TMaxs-->
                <asp:Label ID="Label18" runat="server" Style="z-index: 183; left: 415px; position: absolute; top: 20px"
                    Text="T1Máx:" meta:resourcekey="Label18Resource1"></asp:Label> 
                <asp:TextBox ID="TxtT1Max" runat="server" Enabled="False" Style="z-index: 164; left: 480px; position: absolute; top: 20px"
                    Width="45px" meta:resourcekey="TxtT1MaxResource1">100</asp:TextBox>
                <asp:Label ID="Label22" runat="server" Style="z-index: 185; left: 415px; position: absolute; top: 50px"
                    Text="T2Máx:" meta:resourcekey="Label22Resource1"></asp:Label>
                <asp:TextBox ID="TxtT2Max" runat="server" Enabled="False" Style="z-index: 163; left: 480px; position: absolute; top: 50px"
                    Width="45px" meta:resourcekey="TxtT2MaxResource1">1000</asp:TextBox>
                <asp:Label ID="Label25" runat="server" Style="z-index: 187; left: 415px; position: absolute; top: 110px"
                    Text="T4Máx:" meta:resourcekey="Label25Resource1"></asp:Label>
                <asp:TextBox ID="TxtT4Max" runat="server" Enabled="False" Style="z-index: 134; left: 480px; position: absolute; top: 110px"
                    Width="45px" meta:resourcekey="TxtT4MaxResource1">1000</asp:TextBox>
                <asp:Label ID="Label27" runat="server" Style="z-index: 189; left: 415px; position: absolute; top: 140px"
                    Text="T5Máx:" meta:resourcekey="Label27Resource1"></asp:Label>
                <asp:TextBox ID="TxtT5Max" runat="server" Enabled="False" Style="z-index: 133; left: 480px; position: absolute; top: 140px"
                    Width="45px" meta:resourcekey="TxtT5MaxResource1">100</asp:TextBox>
                <asp:Label ID="Label29" runat="server" Style="z-index: 191; left: 415px; position: absolute; top: 170px"
                    Text="T6Máx:" meta:resourcekey="Label29Resource1"></asp:Label>
                <asp:TextBox ID="TxtT6Max" runat="server" Enabled="False" Style="z-index: 127; left: 480px; position: absolute; top: 170px"
                    Width="45px" meta:resourcekey="TxtT6MaxResource1">6000</asp:TextBox>
                <asp:Label ID="Label31" runat="server" Style="z-index: 193; left: 415px; position: absolute; top: 200px"
                    Text="T8Máx:" meta:resourcekey="Label31Resource1"></asp:Label>
                <asp:TextBox ID="TxtT8Max" runat="server" Enabled="False" Style="z-index: 126; left: 480px; position: absolute; top: 200px"
                    Width="45px" meta:resourcekey="TxtT8MaxResource1">150</asp:TextBox>
                <asp:Label ID="Label33" runat="server" Style="z-index: 195; left: 415px; position: absolute; top: 230px"
                    Text="T9Máx:" meta:resourcekey="Label33Resource1"></asp:Label>
                <asp:TextBox ID="TxtT9Max" runat="server" Enabled="False" Style="z-index: 125; left: 480px; position: absolute; top: 230px"
                    Width="45px" meta:resourcekey="TxtT9MaxResource1">60</asp:TextBox>
                <asp:Label ID="Label35" runat="server" Style="z-index: 197; left: 415px; position: absolute; top: 260px"
                    Text="T10Máx:" meta:resourcekey="Label35Resource1"></asp:Label>
                <asp:TextBox ID="TxtT10Max" runat="server" Enabled="False" Style="z-index: 124; left: 480px; position: absolute; top: 260px"
                    Width="45px" meta:resourcekey="TxtT10MaxResource1">130</asp:TextBox>
                <asp:Label ID="Label37" runat="server" Style="z-index: 200; left: 415px; position: absolute; top: 290px"
                    Text="T11Máx:" meta:resourcekey="Label37Resource1"></asp:Label>
                <asp:TextBox ID="TxtT11Max" runat="server" Enabled="False" Style="z-index: 123; left: 480px; position: absolute; top: 290px"
                    Width="45px" meta:resourcekey="TxtT11MaxResource1">130</asp:TextBox>
                
                <!--VMG New Telefonia-->
                <asp:Label ID="LBTReleaseBL" runat="server" Style="z-index: 201; left: 15px; position: absolute; top: 80px"
                    Visible="False" meta:resourcekey="LBTReleaseBLResource"></asp:Label>
                <asp:TextBox ID="TxtTReleaseBL" runat="server" onblur="checkRangeRT(this,1,10);" 
                    Enabled="False" Style="z-index: 124; left: 240px; position: absolute; top: 93px"
                    Width="45px" Visible="False" meta:resourcekey="TxtTReleaseBLResource">130</asp:TextBox>
                                
                <asp:Label ID="LBTmDetFinLlamada" runat="server" Style="z-index: 201; left: 15px; position: absolute; top: 80px"
                    Visible="False" meta:resourcekey="LBTmDetFinLlamadaResource"></asp:Label>
                <asp:TextBox ID="TxtTmDetFinLlamada" runat="server" onblur="checkRangeRT(this,1,8);" 
                    Enabled="False" Style="z-index: 124; left: 180px; position: absolute; top: 90px"
                    Width="45px" Visible="False" meta:resourcekey="TxtTmDetFinLlamadaResource">0</asp:TextBox>

                <asp:Label ID="LBTmLlamadaEntrante" runat="server" Style="z-index: 201; left: 15px; position: absolute; top: 130px"
                    Visible="False" meta:resourcekey="LBTmLlamadaEntranteResource"></asp:Label>
                <asp:TextBox ID="TxtTmLlamadaEntrante" runat="server" onblur="checkRangeRT(this,1,60);" 
                    Enabled="False" Style="z-index: 124; left: 180px; position: absolute; top: 143px"
                    Width="45px" Visible="False" meta:resourcekey="TxtTmLlamadaEntranteResource">0</asp:TextBox>

                <asp:Label ID="LBDeteccionDtmf" runat="server" Style="z-index: 201; left: 15px; position: absolute; top: 180px"
                    Visible="False" meta:resourcekey="LBDeteccionDtmfResource"></asp:Label>
                <asp:DropDownList ID="DDLDeteccionDtmf" runat="server" Style="z-index: 124; left: 180px; position: absolute; top: 175px" 
                        Visible="true" Width="45px" AppendDataBoundItems="True" class="select" Enabled="False" 
                        meta:resourcekey="DDLDeteccionDtmfResource">
                            <asp:ListItem Value="1" meta:resourcekey="DDLYesItemResource">Si</asp:ListItem>
                            <asp:ListItem Value="0" meta:resourcekey="DDLNoItemResource">No</asp:ListItem>
                </asp:DropDownList>
                
                <asp:Label ID="LBPeriodoSpvRing" runat="server" Style="z-index: 201; left: 15px; position: absolute; top: 180px"
                    Visible="False" meta:resourcekey="LBPeriodoSpvRingResource"></asp:Label>
                <asp:TextBox ID="TxtPeriodoSpvRing" runat="server" onblur="checkRangeRT(this,50,400);" 
                    Enabled="False" Style="z-index: 124; left: 180px; position: absolute; top: 193px"
                    Width="45px" Visible="False" meta:resourcekey="TxPeriodoSpvRingResource">0</asp:TextBox>
                                
                <asp:Label ID="LBFiltroSpvRing" runat="server" Style="z-index: 201; left: 15px; position: absolute; top: 230px"
                    Visible="False" meta:resourcekey="LBFiltroSpvRingResource"></asp:Label>
                <asp:TextBox ID="TxtFiltroSpvRing" runat="server" onblur="checkRangeRT(this,1,6);" 
                    Enabled="False" Style="z-index: 124; left: 180px; position: absolute; top: 243px"
                    Width="45px" Visible="False" meta:resourcekey="TxtFiltroSpvRingResource">0</asp:TextBox>                

                <asp:Label ID="LBDeteccionInversionPol" runat="server" Style="z-index: 201; left: 270px; position: absolute; top: 30px"
                    Visible="False" meta:resourcekey="LBDeteccionInversionPolResource"></asp:Label>
                <asp:DropDownList ID="DDLDeteccionInversionPol" runat="server" Style="z-index: 124; left: 450px; position: absolute; top: 43px" 
                    Visible="true" Width="45px" AppendDataBoundItems="True" class="select" Enabled="False">
                        <asp:ListItem Value="1" meta:resourcekey="DDLYesItemResource">Si</asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="DDLNoItemResource">No</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="LBDeteccionCallerId" runat="server" Style="z-index: 201; left: 270px; position: absolute; top: 80px"
                    Visible="False" meta:resourcekey="LBDeteccionCallerIdResource"></asp:Label>
                <asp:DropDownList ID="DDLDeteccionCallerId" runat="server" Style="z-index: 124; left: 450px; position: absolute; top: 93px" 
                        OnSelectedIndexChanged="DDLElement_SelectedIndexChanged" AutoPostBack="True" Visible="true" 
                        Width="45px" AppendDataBoundItems="True" class="select" Enabled="False">
                            <asp:ListItem Value="1" meta:resourcekey="DDLYesItemResource">Si</asp:ListItem>
                            <asp:ListItem Value="0" meta:resourcekey="DDLNoItemResource">No</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="LBTmDeteccionCallerId" runat="server" Style="z-index: 201; left: 270px; position: absolute; top: 130px"
                    Visible="False" meta:resourcekey="LBTmDeteccionCallerIdResource"></asp:Label>
                <asp:TextBox ID="TxtTmDeteccionCallerId" runat="server" onblur="checkRangeRT(this,1001,5000);" 
                    Enabled="False" Style="z-index: 124; left: 450px; position: absolute; top: 143px"
                    Width="45px" Visible="False" meta:resourcekey="TxtDeteccionCallerIdResource">0</asp:TextBox>

                <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="TxtT1"
                    ErrorMessage="El campo T1 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 204; left: 85px; position: absolute; top: 68px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator4Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="TxtT2"
                    ErrorMessage="El campo T2 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 205; left: 85px; position: absolute; top: 103px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator5Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="TxtT3"
                    ErrorMessage="El campo T3 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 206; left: 85px; position: absolute; top: 138px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator6Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="TxtT4"
                    ErrorMessage="El campo T4 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 207; left: 85px; position: absolute; top: 173px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator7Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator8" runat="server" ControlToValidate="TxtT5"
                    ErrorMessage="El campo T5 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 208; left: 85px; position: absolute; top: 208px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator8Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator9" runat="server" ControlToValidate="TxtT1Max"
                    ErrorMessage="El campo T1Máx debe ser numérico." Operator="DataTypeCheck" Style="z-index: 209; left: 240px; position: absolute; top: 68px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator9Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator10" runat="server" ControlToValidate="TxtT2Max"
                    ErrorMessage="El campo T2Máx debe ser numérico." Operator="DataTypeCheck" Style="z-index: 210; left: 240px; position: absolute; top: 105px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator10Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator11" runat="server" ControlToValidate="TxtT4Max"
                    ErrorMessage="El campo T4Máx debe ser numérico." Operator="DataTypeCheck" Style="z-index: 211; left: 240px; position: absolute; top: 142px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator11Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator12" runat="server" ControlToValidate="TxtT5Max"
                    ErrorMessage="El campo T5Máx debe ser numérico." Operator="DataTypeCheck" Style="z-index: 212; left: 240px; position: absolute; top: 179px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator12Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator13" runat="server" ControlToValidate="TxtT6"
                    ErrorMessage="El campo T6 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 213; left: 378px; position: absolute; top: 68px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator13Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator26" runat="server" ControlToValidate="TxtT6Max"
                    ErrorMessage="El campo T6Max debe ser numérico." Operator="DataTypeCheck" Style="z-index: 214; left: 524px; position: absolute; top: 68px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator26Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator27" runat="server" ControlToValidate="TxtT8Max"
                    ErrorMessage="El campo T8Max debe ser numérico." Operator="DataTypeCheck" Style="z-index: 215; left: 524px; position: absolute; top: 103px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator27Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator28" runat="server" ControlToValidate="TxtT9Max"
                    ErrorMessage="El campo T9Max debe ser numérico." Operator="DataTypeCheck" Style="z-index: 216; left: 524px; position: absolute; top: 138px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator28Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator19" runat="server" ControlToValidate="TxtT10Max"
                    ErrorMessage="El campo T10Max debe ser numérico." Operator="DataTypeCheck" Style="z-index: 217; left: 524px; position: absolute; top: 173px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator19Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator20" runat="server" ControlToValidate="TxtT11Max"
                    ErrorMessage="El campo T11Max debe ser numérico." Operator="DataTypeCheck" Style="z-index: 218; left: 524px; position: absolute; top: 208px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator20Resource1">*</asp:CompareValidator>
                &nbsp; &nbsp;
				 <asp:CompareValidator ID="CompareValidator14" runat="server" ControlToValidate="TxtT8"
                     ErrorMessage="El campo T8 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 225; left: 378px; position: absolute; top: 103px"
                     Type="Integer"
                     meta:resourcekey="CompareValidator14Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator15" runat="server" ControlToValidate="TxtT9"
                    ErrorMessage="El campo T9 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 226; left: 378px; position: absolute; top: 138px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator15Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator16" runat="server" ControlToValidate="TxtT10"
                    ErrorMessage="El campo T10 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 227; left: 378px; position: absolute; top: 173px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator16Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator17" runat="server" ControlToValidate="TxtT11"
                    ErrorMessage="El campo T11 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 228; left: 378px; position: absolute; top: 208px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator17Resource1">*</asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator18" runat="server" ControlToValidate="TxtT12"
                    ErrorMessage="El campo T12 debe ser numérico." Operator="DataTypeCheck" Style="z-index: 229; left: 378px; position: absolute; top: 246px"
                    Type="Integer"
                    meta:resourcekey="CompareValidator18Resource1">*</asp:CompareValidator>
            </asp:View>

            <asp:View ID="ViewAsignacionHw" runat="server">
                <asp:Label ID="Label14" runat="server" Style="left: 210px; position: absolute; top: 25px"
                    Text="TifX" meta:resourcekey="Label14Resource1"></asp:Label>
                <asp:DropDownList ID="DListTifx" runat="server" Style="left: 210px; position: absolute; top: 45px"
                    Width="180px" AutoPostBack="True" class="select"
                    OnSelectedIndexChanged="DDLTifx_SelectedIndexChanged" Enabled="False"
                    meta:resourcekey="DListTifxResource1">
                </asp:DropDownList>
                <asp:RadioButtonList ID="RBLTipoEquipo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RBLTipoEquipo_SelectedIndexChanged"
                    Style="z-index: 191; left: 17px; position: absolute; top: 23px; height: 63px" Enabled="False" BorderStyle="Groove"
                    meta:resourcekey="RBLTipoEquipoResource1">
                    <asp:ListItem Selected="True" Value="0" meta:resourcekey="ListItemResource19">Pasarela</asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource20">Equipo Externo</asp:ListItem>
                </asp:RadioButtonList>
                <asp:Label ID="LblEquipoExterno" runat="server" Style="z-index: 192; left: 210px; position: absolute; top: 25px; height: 19px; width: auto;"
                    Text="Equipo Externo" Visible="False"
                    meta:resourcekey="LblEquipoExternoResource1"></asp:Label>
                <asp:DropDownList ID="DListEquipoExternos" runat="server" Style="z-index: 194; left: 210px; position: absolute; top: 45px; height:auto"
                    Visible="False" Width="180px" class="select"
                    AppendDataBoundItems="True" Enabled="False" AutoPostBack="True"
                    OnSelectedIndexChanged="DDLEquipoExternos_SelectedIndexChanged"
                    meta:resourcekey="DListEquipoExternosResource1">
                </asp:DropDownList>

                <asp:Table ID="TTifx" runat="server" Height="112px" Style="left: 46px; position: absolute; top: 96px; z-index: 102;"
                    Width="400px" BorderStyle="Double" GridLines="Both"
                    BorderColor="Black" SkinID="TableSkin">
                    <asp:TableRow ID="TableRow1" runat="server" BorderWidth="3px" BorderStyle="Solid" SkinID="TableHeaderRow">
                        <asp:TableCell ID="TableCell1" runat="server"
                            HorizontalAlign="Center" meta:resourcekey="TableCell1Resource1">Slot 0</asp:TableCell>
                        <asp:TableCell ID="TableCell2" runat="server"
                            HorizontalAlign="Center" meta:resourcekey="TableCell2Resource1">Slot 1</asp:TableCell>
                        <asp:TableCell ID="TableCell3" runat="server"
                            HorizontalAlign="Center" meta:resourcekey="TableCell3Resource1">Slot 2</asp:TableCell>
                        <asp:TableCell ID="TableCell4" runat="server"
                            HorizontalAlign="Center" meta:resourcekey="TableCell4Resource1">Slot 3</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow2" runat="server" SkinID="TableRow">
                        <asp:TableCell ID="TableCell5" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox11" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox11" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell6" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox12" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox12" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell7" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox13" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox13" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell8" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox14" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox14" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow3" runat="server" SkinID="TableRow">
                        <asp:TableCell ID="TableCell9" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox21" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox21" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell10" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox22" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox22" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell11" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox23" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox23" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell12" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox24" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox24" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow4" runat="server" SkinID="TableRow">
                        <asp:TableCell ID="TableCell13" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox31" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox31" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell14" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox32" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox32" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell15" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox33" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox33" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell16" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox34" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox34" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow5" runat="server" SkinID="TableRow">
                        <asp:TableCell ID="TableCell17" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox41" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox41" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell18" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox42" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox42" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell19" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox43" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox43" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell20" runat="server" Width="100px"
                            HorizontalAlign="Center">
                            <asp:TextBox
                                ID="TextBox44" runat="server" Visible="False" ReadOnly="True" Width="100px"></asp:TextBox><asp:CheckBox ID="CheckBox44" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" />

                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>

        </asp:MultiView>
    </asp:Panel>
    <asp:Button ID="IBGenerales" runat="server" SkinId="ButtonTab" CausesValidation="true"
		OnClick="OnButton_Click" Style="z-index: 109; left: 276px; position: absolute;
		top: 42px" meta:resourcekey="IBGeneralesResource1" />
	<asp:Button ID="IBVoip" runat="server" SkinId="ButtonTab" CausesValidation="true"
		OnClick="OnButton_Click" Style="z-index: 110; left: 380px; position: absolute;
		top: 42px" meta:resourcekey="IBVoipResource1" />
	<asp:Button ID="IBAudio" runat="server" SkinId="ButtonTab" CausesValidation="true"
		OnClick="OnButton_Click" Style="z-index: 111; left: 484px; position: absolute;
		top: 42px" meta:resourcekey="IBAudioResource1" />
	<asp:Button ID="IBFuncionalidad" runat="server" SkinId="ButtonTab" CausesValidation="true"
		OnClick="OnButton_Click" Style="z-index: 112; left: 588px; position: absolute;
		top: 42px" meta:resourcekey="IBFuncionalidadResource1" />
	<asp:Button ID="IBAsignacion" runat="server" SkinId="ButtonTab" CausesValidation="true"
		OnClick="OnButton_Click" Style="z-index: 113; left: 692px; position: absolute;
		top: 42px" meta:resourcekey="IBAsignacionResource1" />
	
	<asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores:"
		Style="z-index: 115; left: 286px; position: absolute; top: 549px" 
		  meta:resourcekey="ValidationSummary1Resource1" />
</asp:Content>

