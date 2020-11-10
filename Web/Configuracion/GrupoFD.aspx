<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="GrupoFD.aspx.cs" Inherits="GrupoFD" 
		CodeFileBaseClass="PageBaseCD40.PageCD40" Title="Gestión de Destinos Radio" EnableEventValidation="false" StyleSheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
    <script type="text/javascript"> 
			function AbreVentana(ventana) {
			window.open(ventana);
		}
	</script>

    <asp:Label ID="Label1" runat="server" Style="z-index: 112; position: absolute; " 
			Text="GESTIÓN DESTINOS RADIO" CssClass="labelPagina" 
		  meta:resourcekey="Label1Resource1"></asp:Label>
	 <asp:ListBox ID="ListBox1" runat="server"
         OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" AutoPostBack="True"
         SkinID="MascaraListaElementos"  Style="z-index: 100; left: 21px; width:172px; height:460px; position: absolute; top: 58px" meta:resourcekey="ListBox1Resource1">
    </asp:ListBox>

        <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Height="455px" Style="z-index: 105; left: 200px; position: absolute; top: 60px"
            Width="800px" Enabled="False"
            meta:resourcekey="Panel1Resource1">

            <asp:Label ID="Label5" runat="server" meta:resourcekey="Label5Resource1" Style="z-index: 116; left: 15px; position: absolute; top: 22px; right: 631px;width:165px;" Text="Id. Destino:" Visible="True"></asp:Label>
            <asp:TextBox ID="TxtIdEnlace" runat="server" Style="z-index: 117; left: 14px; position: absolute; top: 42px; height: 22px; width: 140px;"
                Visible="True" MaxLength="32"
                meta:resourcekey="TxtIdDestino"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldIdentificador" runat="server" ControlToValidate="TxtIdEnlace"
                ErrorMessage="Rellene el campo Identificador"
                Style="left: 172px; position: absolute; top: 42px" Enabled="False"
                meta:resourcekey="RequiredFieldIdentificadorResource1">*</asp:RequiredFieldValidator>
            
           <%--<asp:CheckBox ID="CheckFrecNoDesasignable" runat="server" Style="z-index: 122; left: 167px; position: absolute; top: 97px"
                Text="Frec. no desasignable" TextAlign="Left" Width="153px"
                Visible="False" meta:resourcekey="CheckFrecNoDesasignableResource1" />--%>

             <%-- <asp:CheckExclusividad ID="CheckBox1" runat="server" Style="z-index: 122; left: 167px; position: absolute; top: 107px"
                Text="Exclusividad TX-RX" TextAlign="Left" Width="153px"
                Visible="False" meta:resourcekey="CheckExclusividadResource1" />--%>

            <asp:Label ID="Label6" runat="server" Style="z-index: 111; left: 10px; position: absolute; top: 190px"
                Text="Emplazamientos" Visible="false" meta:resourcekey="Label6Resource1"></asp:Label>
            
            <asp:ListBox ID="ListEmplazamientos" runat="server" meta:resourcekey="ListRecursosResource1"
                Enabled="False" Style="overflow-x:auto; z-index: 120; left: 11px; position: absolute; top: 210px; width: 120px; height: 195px;" class="listbox" EnableViewState="true" SelectionMode="Multiple" Rows="10">
            </asp:ListBox>

            <asp:Label ID="Label8" runat="server" Style="z-index: 110; left: 140px; position: absolute; top: 190px;  height: 17px;"
                Text="Tipo:" Visible="false" meta:resourcekey="Label8Resource1"></asp:Label>
            <asp:ListBox ID="ListTipos" runat="server" Enabled="false"
                meta:resourcekey="LisTipoResource1" Style="overflow-x:auto; z-index: 120; left: 140px; position: absolute; top: 210px;width: 120px; height: 195px;" SelectionMode="Multiple"></asp:ListBox>

            <asp:Label ID="Label3" runat="server" Style="z-index: 110; left: 11px; position: absolute; top: 190px;height: 17px;"
                Text="Rec. Asignados"
                meta:resourcekey="Label3Resource1"></asp:Label>

            <asp:ListBox ID="ListRecursos" runat="server"
                meta:resourcekey="ListRecursosResource1"  Style="overflow-x:auto; z-index: 120; left: 11px; position: absolute; top: 210px; width: 400px; height: 195px;font-family: Courier New;font-size:13px;" Visible="true" SelectionMode="Multiple"></asp:ListBox>
           <asp:Label ID="Label4" runat="server" Style="z-index: 111; left: 480px; position: absolute; top: 190px"
                Text="Recursos Libres " Visible="False"
                meta:resourcekey="Label4Resource1"></asp:Label>
            <asp:ListBox ID="ListRecursosLibres" runat="server" Style="overflow-x:auto; z-index: 121; left: 480px; position: absolute; top: 210px; width: 305px; height: 190px; font-family: Courier New;font-size:13px;"
                Visible="False" SelectionMode="Multiple"  meta:resourcekey="ListRecursosLibresResource1"></asp:ListBox>

            <asp:Label ID="LblFiltroEmplazamiento" runat="server" Style="z-index: 111; left: 275px; position: absolute; top: 420px"
                Text="Filtrar por emplazamiento" Visible="False"
                meta:resourcekey="FiltrarPorEmplazamientoResource"></asp:Label>

            <asp:DropDownList ID="DListEmplazamiento" runat="server" Enabled="False" Style="z-index: 107; left: 510px; position: absolute; top: 420px; height: 20px; width: 120px; "  class="select" OnSelectedIndexChanged="DListEmplazamiento_SelectedIndexChanged" AutoPostBack="true"
                        AppendDataBoundItems="True" meta:resourcekey="DListEmplazamientoResource1" Visible="False">
                        <asp:ListItem Value="0" meta:resourcekey="ListEmplazamientoResource" Text="Todos">&lt;Selecciona&gt;</asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="DropDownFiltro" runat="server" Style="z-index: 107; left: 650px; position: absolute; top:420px; height: 20px; width: 120px;"  class="select" OnSelectedIndexChanged="DFiltro_SelectedIndexChanged" AutoPostBack="true"
                        AppendDataBoundItems="True" Visible="False">
                <asp:ListItem Value="0" meta:resourcekey="DropDownFiltroItemResource1">Tipo</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="DropDownFiltroItemResource2">Emplazamiento</asp:ListItem>
            </asp:DropDownList>
            
             <asp:ListBox ID="ListTiposLibres" runat="server" Style="z-index: 121; left: 650px; position: absolute; top: 210px; width: 120px; height: 190px; font-family: Courier New;font-size:13px;"
                Visible="False" SelectionMode="Multiple"  meta:resourcekey="ListTiposLibresResource1"></asp:ListBox>

            <asp:ListBox ID="ListEmplazamientosLibres" runat="server" Style="overflow-x:auto; z-index: 121; left: 650px; position: absolute; top: 210px; width: 120px; height: 190px; font-family: Courier New;font-size:13px;"
                Visible="False" SelectionMode="Multiple" meta:resourcekey="ListTiposLibresResource1"></asp:ListBox>
  
            <asp:ImageButton ID="IButAsignar" runat="server" OnClick="IButAsignar_Click" Style="z-index: 102; left: 427px; position: absolute; top: 260px; -webkit-transform: rotate(90deg); -moz-transform: rotate(90deg); -o-transform: rotate(90deg); transform: rotate(90deg);"
                CausesValidation="False"
                ImageUrl="~/Configuracion/Images/arrow.png" Visible="True"
                meta:resourcekey="IButAsignarResource1" />
                    &nbsp;
            <asp:ImageButton ID="IButQuitar" runat="server" Style="z-index: 103; left: 427px; position: absolute; top: 320px;-webkit-transform: rotate(-90deg); -moz-transform: rotate(-90deg); -o-transform: rotate(-90deg); transform: rotate(-90deg); "
                CausesValidation="False"
                ImageUrl="~/Configuracion/Images/arrow.png" OnClick="IButQuitar_Click"
                Visible="True" meta:resourcekey="IButQuitarResource1" />

<%--            <asp:RangeValidator ID="RVFrequency" runat="server" Style="left: 480px; position: absolute; top: 90px" 
                MaximumValue="30000000" MinimumValue="1600000" ControlToValidate="TBTunedFrequency" 
                ErrorMessage="Out of range" Type="Integer"  meta:resourcekey="RangeMessageResource1">*</asp:RangeValidator>--%>
                                       
            <%--<asp:Button ID="BtAceptar" runat="server" meta:resourcekey="BtAceptarResource1" OnClick="BtAceptar_Click" Style="z-index: 113; left: 394px; position: absolute; top: 390px; width: 75px;" Text="Aceptar" UseSubmitBehavior="true" Visible="False" />--%>
           <%-- <ajaxToolkit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender" runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
            </ajaxToolkit:ConfirmButtonExtender> --%>

             <asp:Label ID="Label2" runat="server" Style="z-index: 101; left: 14px; position: absolute; top: 75px"
                Text="Tipo:" Visible="True" meta:resourcekey="Label2Resource1"></asp:Label>
            <asp:DropDownList ID="DListTipo" runat="server" Style="z-index: 105; left: 14px; height: 22px; position: absolute; top: 95px"
                Width="90px" Visible="True" OnSelectedIndexChanged="DListTipo_SelectedIndexChanged"
                meta:resourcekey="DListTipoResource1" class="select" AutoPostBack="True">
                <asp:ListItem Value="0" meta:resourcekey="ListItemResource1">VHF</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="ListItemResource2">UHF</asp:ListItem>
                <asp:ListItem Value="2" meta:resourcekey="ListItemResource3">HF</asp:ListItem>
            </asp:DropDownList>



             <asp:Table runat="server" ID="TblTunedFreq" Style="left: 14px; position: absolute; top: 125px; right: 460px;" Visible="false">
                <asp:TableRow ID="TableRow1" runat="server" >
                    <asp:TableCell ID="TableCell1" runat="server"  Style="width:40%;">
                        <asp:Label runat="server" ID="LblTunedFreq" meta:resourcekey="LblTunedFreqResource1"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell2" runat="server" Style="width:40%;" >
                        <asp:TextBox ID="TBTunedFrequency" runat="server" Width="100px" Style="left:50px" ></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell ID="TableCell3" runat="server">
                        <asp:Label ID="Label10" runat="server">Hz</asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <asp:Label ID="LbModoTransmision" runat="server" Style="z-index: 101; left: 14px; position: absolute; top: 128px"
                Text="Modo de Transmisión:" Visible="True" meta:resourcekey="LbModoTransmisionResource"></asp:Label>
            <asp:DropDownList ID="DListModoTransmision" runat="server" Style="z-index: 105; left: 14px; height: 22px; position: absolute; top: 147px"
                Width="145px" Visible="True" OnSelectedIndexChanged="DListModoTransmision_SelectedIndexChanged" AutoPostBack="True"
                meta:resourcekey="DListModoTransmisionRes" class="select">
                <asp:ListItem Value="C" meta:resourcekey="ListItemModoTransmisionClimaxRes">Climax</asp:ListItem>
                <asp:ListItem Value="R" meta:resourcekey="ListItemModoTransmisionUltRecepcionRes">Última Recepción</asp:ListItem>
                <%--<asp:ListItem Value="M" meta:resourcekey="ListItemModoTransmisionManualRes">Manual</asp:ListItem>  --%> 
            </asp:DropDownList>

            <asp:Label ID="LbEmplazamientoDefecto" runat="server" Style="z-index: 101; left: 190px; position: absolute; top: 128px"
                Text="Emplazamiento por Defecto:" Visible="False" meta:resourcekey="LbEmplazamientoDefectoResource"></asp:Label>
            <asp:DropDownList ID="DListEmplazamientoDefecto" runat="server" Style="z-index: 105; left: 190px; height: 22px; position: absolute; top: 147px"
                Width="200px" Visible="False" OnSelectedIndexChanged="DListEmplazamientoDefecto_SelectedIndexChanged" 
                meta:resourcekey="DListEmplazamientoDefectoRes" class="select" AutoPostBack="True">
                <asp:ListItem Value="0" meta:resourcekey="ListItemEmplDefNinguno">Ninguno</asp:ListItem>
            </asp:DropDownList>

            <asp:Label ID="LbTiempoVueltaADefecto" runat="server" Text="Tiempo Vuelta a Defecto" Style="left:430px; top:128px; position:absolute; width:auto"
                Visible="False" meta:resourcekey="LbTiempoVueltaADefectoResource"></asp:Label>

            <asp:TextBox ID="TxtTiempoVueltaADefecto" runat="server" Width="88px" Style="z-index: auto; top: 147px; left: 430px; position: absolute;"
                Visible="False" meta:resourcekey="TxtTiempoVueltaADefectoResource">0</asp:TextBox>

             <asp:RequiredFieldValidator ID="RFV_TBTunedFrequency" runat="server" ControlToValidate="TBTunedFrequency"
                ErrorMessage="La Frecuencia Sintonizada es requerida"
                Style="left: 330px; position: absolute; top: 132px" Enabled="False"
                meta:resourcekey="RFV_TBTunedFrequencyResource1">*</asp:RequiredFieldValidator>
            
            <asp:CustomValidator 
                 ID="valCustom" Style="left: 330px; position: absolute; top: 132px" Enabled="False"
                  runat="server">*</asp:CustomValidator>


             <asp:Label ID="Label41" runat="server" Style="z-index: 102; left:190px; position: absolute; top: 22px; height: 18px; width: 150px;"
             Text="Modo Destino:" meta:resourcekey="Label41Resource1"></asp:Label>
             <asp:DropDownList ID="DListModoDestino" runat="server" Style="z-index: 103; left: 190px; position: absolute; top: 42px; height: 22px; width: 88px;" 
                 OnSelectedIndexChanged="DListModoDestino_SelectedIndexChanged" AutoPostBack="True"
                 meta:resourcekey="DListModoDestinoResource1" class="select" ToolTip="Normal, FD">
                        <asp:ListItem Value="0"  meta:resourcekey="DListModoDestinoItemRes1">Normal</asp:ListItem>
                        <%--<asp:ListItem Value="1">1+1</asp:ListItem> --%>
                        <asp:ListItem Value="2" meta:resourcekey="DListModoDestinoItemRes2">FD</asp:ListItem>
                        <%--<asp:ListItem Value="3">EM</asp:ListItem>  --%> 
             </asp:DropDownList>

              <asp:Label ID="Label7" runat="server" Style="z-index: 102; left: 370px; position: absolute; top: 22px; height: 18px; width: 155px;"
             Text="Prioridad Sesión SIP:" meta:resourcekey="Label7Resource1"></asp:Label>
             <asp:DropDownList ID="DListPrioridadSIP" runat="server" Style="z-index: 103; left: 370px; position: absolute; top: 42px; height: 22px; width: 105px;" 
                        OnSelectedIndexChanged="DListPrioridadSIP_SelectedIndexChanged" AutoPostBack="True"
                        meta:resourcekey="DListGrupoResource1" class="select" ToolTip="Normal, Emergencia">
                        <asp:ListItem Value="2" meta:resourcekey="DListPrioridadSIPItemRes1">Normal</asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="DListPrioridadSIPItemRes2">Emergencia</asp:ListItem>                        
             </asp:DropDownList>

            <asp:Label ID="LbRedundancia" runat="server" Style="z-index: 102; left: 543px; position: absolute; top: 22px; height: 18px; width: 155px;"
             Text="1+1" meta:resourcekey="CheckedRedundanciaResource1"></asp:Label>
             <asp:CheckBox ID="CheckBoxRedundancia" runat="server" Style="z-index: 103; left: 543px; position: absolute; top: 42px; width: 20px;"
                 OnCheckedChanged="CBRedundancia_CheckedChanged" AutoPostBack="true"
                    Visible="True"/>

             <asp:Label ID="Label9" runat="server" Style="z-index: 102; left: 598px; position: absolute; top: 22px; height: 18px; width: 155px;"
             Text="Método Climax:" meta:resourcekey="Label10Resource"></asp:Label>
             <asp:DropDownList ID="DListMetodoClimax" runat="server" Style="z-index: 103; left: 600px; position: absolute; top: 42px; height: 22px; width: 88px;" OnSelectedIndexChanged="DListMetodoClimax_SelectedIndexChanged"
                        meta:resourcekey="DListMetodoClimaxResource1" class="select" ToolTip="Relativo, Absoluto">
                        <asp:ListItem Value="0"  meta:resourcekey="DListMetodoClimaxItemRes1">Relativo</asp:ListItem>
                        <asp:ListItem Value="1"  meta:resourcekey="DListMetodoClimaxItemRes2">Absoluto</asp:ListItem>                        
             </asp:DropDownList>

            <asp:CheckBox ID="CheckedSincro" runat="server" Enabled="False" Text="Sinc. Recepción en Grupo" Style="z-index: 103; left: 359px; position: absolute; top: 127px; width: 176px; margin-right: 9px;"
                  meta:resourcekey="CheckedSincroResource1" Visible="False"/>
            <asp:CheckBox ID="CheckBox1Squelch" runat="server" Enabled="False" Text="Audio en Primer Squelch" Style="z-index: 103; left: 548px; position: absolute; top: 127px; width: 176px; margin-right: 9px;" 
                   meta:resourcekey="CheckBox1SquelchResource1" Visible="False" />


            <asp:Label ID="Label44" runat="server" Text="Periodo Cálculo CLD (s):" Style="left:190px; top:75px; position:absolute; width:auto"
                meta:resourcekey="Label44Resource1"></asp:Label>
                <asp:TextBox ID="TextBoxCLD" runat="server" Width="88px" Style="z-index: auto; top: 94px; left: 190px; position: absolute;"
                meta:resourcekey="TxtTiempoCLDResource1">1</asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="TextBoxCLD" Style="z-index: auto; top: 98px; left: 280px; position: absolute;"
            ErrorMessage="El periodo de cálculo CLD debe estar comprendido entre 0 y 60 segundos."
            meta:resourcekey="RequiredFieldValidator18Resource1">*</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator15" runat="server" ControlToValidate="TextBoxCLD"
            ErrorMessage="El campo GRS Delay debe ser numérico." Operator="DataTypeCheck"
            Style="z-index: 207; left: 269px; position: absolute; top: 94px"
            Type="Integer"  meta:resourcekey="CompareValidator15Resource1">*</asp:CompareValidator> 
            <asp:RangeValidator ID="RangeTextBoxCLD" runat="server" ControlToValidate="TextBoxCLD"
			ErrorMessage="El periodo de cálculo CLD debe estar comprendido entre 0 y 60 segundos." MaximumValue="60"
			MinimumValue="0" Style="z-index: 207; left: 280px; position: absolute; top: 98px; width: 8px;"
			Type="Double" meta:resourcekey="RVTxtCLDResource1">*</asp:RangeValidator>


            <asp:Label ID="Label11" runat="server" meta:resourcekey="Label11Resource1" Style="z-index: 116; left: 370px; position: absolute; top: 75px; right: 208px; height: 19px;" Text="Ventana BSS (ms):"></asp:Label>
            <asp:TextBox ID="TextVentanaBSS" runat="server" Style="z-index: 117; left: 370px; position: absolute; top: 95px; height: 22px; width: 87px;" MaxLength="32" meta:resourcekey="TxtVentanaBSS" Text="50"></asp:TextBox>

              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
							     ControlToValidate="TextVentanaBSS" Style="z-index: 117; left: 470px; position: absolute; top: 98px"
							     meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
					<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="TextVentanaBSS"
							    Operator="DataTypeCheck" Type="Integer" Width="17px"  Style="z-index: 117; left: 480px; position: absolute; top: 95px"
							     meta:resourcekey="CompareValidator2Resource1">*</asp:CompareValidator>
                    <asp:RangeValidator ID="RVTexVentanaBSS" runat="server" ControlToValidate="TextVentanaBSS"
					    ErrorMessage="La ventana de selección BSS debe estar comprendido entre 200 y 2000 ms" MaximumValue="2000"
					    MinimumValue="200" Style="z-index: 117; left: 490px; position: absolute; top: 95px"
					    Type="Double" meta:resourcekey="RVTxtVentanaBSS">*</asp:RangeValidator>

             <asp:Label ID="Label12" runat="server" Style="z-index: 102; left: 543px; position: absolute; top: 75px; width: 180px;"
                Text="Métodos BSS:" meta:resourcekey="Label12Resource"></asp:Label>
             <asp:DropDownList ID="DDLMetodosBssOfrecidos" runat="server" Style="z-index: 103; left: 543px; position: absolute; top: 95px; width: 145px;" 
                        class="select" meta:resourcekey="DDLMetodosBsssResource1">
                        <asp:ListItem Value="0"  meta:resourcekey="DDLMetodosBssItemRes1">Ninguno</asp:ListItem>
                        <asp:ListItem Value="1"  meta:resourcekey="DDLMetodosBssItemRes2">RSSI</asp:ListItem>
                        <asp:ListItem Value="2"  meta:resourcekey="DDLMetodosBssItemRes3">RSSI y NUCLEO</asp:ListItem>
             </asp:DropDownList>


            <asp:DropDownList ID="DListTipoRec" runat="server" AutoPostBack="True" class="select" Enabled="False" meta:resourcekey="DListTipoResource1" Style="z-index: 103; left: 232px; position: absolute; top: 386px; height: 19px; width: 99px;" Visible="False">
                <asp:ListItem Value="0" meta:resourcekey="ListTipoRec_Audio_RX">Audio RX</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="ListTipoRec_Audio_TX">Audio TX</asp:ListItem>
                <asp:ListItem Value="2" meta:resourcekey="ListTipoRec_Audio_RXTX">Audio RX TX</asp:ListItem>
                <asp:ListItem Value="3" meta:resourcekey="ListTipoRec_Audio_HF">Audio HF-TX</asp:ListItem>
                <asp:ListItem Value="4" meta:resourcekey="ListTipoRec_Audio_MN">Audio M+N</asp:ListItem>
                <%--//20200525 JOI  #4470 Version 2.6.0 no se implementa--%>
            <%--<asp:ListItem Value="7" meta:resourcekey="ListTipoRec_Audio_EE_RX">Audio EE RX</asp:ListItem>
                <asp:ListItem Value="8" meta:resourcekey="ListTipoRec_Audio_EE_TX">Audio EE TX</asp:ListItem>
                <asp:ListItem Value="9" meta:resourcekey="ListTipoRec_Audio_EE_RXTX">Audio EE RX TX</asp:ListItem>--%>
            </asp:DropDownList>
        </asp:Panel>

    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 113; left: 366px; position: absolute; top: 533px"
        Text="Aceptar" Width="80px" Visible="False" UseSubmitBehavior="true"
        OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
<%--    <ajaxToolkit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender"
        runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
    </ajaxToolkit:ConfirmButtonExtender>--%>

    <asp:ValidationSummary ID="errores" runat="server" HeaderText="Resumen de Errores:" ForeColor="Red"
        Style="z-index: 114; left: 240px; position: absolute; top: 580px"
        Visible="False" meta:resourcekey="erroresResource1" />
    <asp:Label ID="LblErrorMismatchFrequency" runat="server" Style="z-index: 115; left: 240px; position: absolute; top: 580px; height: 12px; width: 535px;" ForeColor="Red"
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


