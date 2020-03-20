<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="Sector.aspx.cs" Inherits="Sector" 
	CodeFileBaseClass="PageBaseCD40.PageCD40"	Title="Gestión de Sectores" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">
			function AbreVentana(ventana) {
			window.open(ventana);
		}
	</script>

    <asp:Label ID="Label15" runat="server" Text="GESTIÓN DE SECTORES" 
		  CssClass="labelPagina" meta:resourcekey="Label15Resource1"></asp:Label>

    <asp:ListBox ID="ListBox1" runat="server" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" 
			SkinID="MascaraListaElementos" AutoPostBack="True" 
		  meta:resourcekey="ListBox1Resource1"></asp:ListBox>
    
	<asp:Button ID="IBPropiedadesGenerales" runat="server" Style="z-index: 101; left: 230px;
		position: absolute; top: 42px" 
		  OnClick="OnButton_Click" SkinID="ButtonTab"
		  meta:resourcekey="IBPropiedadesGeneralesResource1" />
	<asp:Button ID="IBUtilidades" runat="server"  SkinID="ButtonTab"
		Style="z-index: 102; left: 340px; position: absolute; top: 42px" 
		  OnClick="OnButton_Click" meta:resourcekey="IBUtilidadesResource1" />
<%--	<asp:Button ID="IBParametros" runat="server"  SkinID="ButtonTab"
		Style="z-index: 103; left: 450px; position: absolute; top: 42px" 
		  OnClick="OnButton_Click" meta:resourcekey="IBParametrosResource1" />--%>

	<asp:Button ID="IBPermisosRedes" runat="server"  SkinID="ButtonTab"
		Style="z-index: 104; left: 450px; position: absolute; top: 42px" 
		  OnClick="OnButton_Click" meta:resourcekey="IBPermisosRedesResource1" />
	<asp:Button ID="IBAgenda" runat="server"  SkinID="ButtonTab"
		Style="z-index: 105; left: 560px; position: absolute; top: 42px" 
		  OnClick="OnButton_Click" meta:resourcekey="IBAgendaResource1" />

	<asp:Button ID="IBNivelesIntrusion" runat="server"  SkinID="ButtonTab"
		Style="z-index: 106; left: 670px; position: absolute; top: 42px" 
		  OnClick="OnButton_Click" Enabled="false" Visible="false"
		  meta:resourcekey="IBNivelesIntrusionResource1" /> 
	
	<asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Height="350px" Style="z-index: 99;
		left: 226px; position: absolute; top: 59px" Width="761px" 
		  meta:resourcekey="Panel1Resource1">
		<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
			<asp:View ID="ViewGenerales" runat="server">
				 <asp:Label ID="Label5" runat="server" Style="z-index: 100; left: 16px; position: absolute;
					  top: 9px" Text="Identificador:" meta:resourcekey="Label5Resource1"></asp:Label>
				 <asp:TextBox ID="TxtIdSector" runat="server" Style="z-index: 101; left: 16px; position: absolute;
					  top: 29px" MaxLength="32" Enabled="False" meta:resourcekey="TxtIdSectorResource1"></asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldSector" runat="server" ControlToValidate="TxtIdSector"
					  ErrorMessage="El campo identificador del sector no puede estar vacio." Style="z-index: 102;
					  left: 200px; position: absolute; top: 33px" 
					 meta:resourcekey="RequiredFieldSectorResource1">*</asp:RequiredFieldValidator>
				<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtIdSector"
					ErrorMessage="Carácter inválido" Style="z-index: 141;
					left: 290px; position: absolute; top: 10px" 
					ValidationExpression="^[\w-]*$" 
					meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RegularExpressionValidator>

				 <asp:Label ID="Label6" runat="server" Style="z-index: 112; left: 16px; position: absolute;
					  top: 64px" Text="Tipo:" meta:resourcekey="Label6Resource1"></asp:Label>
				 <asp:DropDownList ID="DListTipoSector" runat="server" Style="z-index: 113; left: 16px;
					  position: absolute; top: 84px"  Enabled="False" AutoPostBack="True"  class="select"
					 OnSelectedIndexChanged="DListTipoSector_SelectedIndexChanged" 
					 meta:resourcekey="DListTipoSectorResource1">
					  <asp:ListItem Value="R" meta:resourcekey="ListItemTipoSectorReal">Real</asp:ListItem>
					  <asp:ListItem Value="V" meta:resourcekey="ListItemTipoSectorVirtual">Virtual</asp:ListItem>
					  <asp:ListItem Value="M" meta:resourcekey="ListItemTipoSectorMto">Mantenimiento</asp:ListItem>
				 </asp:DropDownList>

				 <asp:Label ID="Label2" runat="server" Style="z-index: 105; left: 16px; position: absolute;
					  top: 119px" Text="Tipo de Posición:" meta:resourcekey="Label2Resource1"></asp:Label>
				 <asp:DropDownList ID="DListTipoPosicion" runat="server" Style="z-index: 114; left: 16px;
					  position: absolute; top: 139px" Width="110px" Enabled="False"  class="select"
					 meta:resourcekey="DListTipoPosicionResource1">
					  <asp:ListItem Value="C" meta:resourcekey="ListItemResource8">Controlador</asp:ListItem>
				 </asp:DropDownList>
                
                <!-- Se elimina el tipo planificador, ya que la aplicación no lo soporta
                    <asp:ListItem Value="P" meta:resourcekey="ListItemResource9">Planificador</asp:ListItem>
                    -->

				 <asp:Label ID="Label17" runat="server" Style="z-index: 108; left: 16px; position: absolute;
					  top: 174px" Text="Id. Sacta:" meta:resourcekey="Label17Resource1"></asp:Label>
			    <asp:DropDownList ID="DListIdSacta" runat="server"  class="select"
					 Style="left: 16px; position: absolute;top: 194px; z-index: 109;" 
					 Enabled="False" meta:resourcekey="DListIdSactaResource1">
			    </asp:DropDownList>

                <asp:Label ID="Label13" runat="server" Style="z-index: 106; left: 16px; position: absolute; top: 229px"
                    Text="Prioridad R2:" meta:resourcekey="Label13Resource1"></asp:Label>
                <asp:DropDownList ID="DDLPrioridadR2" runat="server" Style="z-index: 107; left: 16px; position: absolute; top: 249px"
                    Enabled="False" class="select"
                    meta:resourcekey="DDLPrioridadR2Resource1">
                    <asp:ListItem Value="4" meta:resourcekey="ListItemResource1">No Urgente</asp:ListItem>
                    <asp:ListItem Value="3" meta:resourcekey="ListItemResource2">Normal</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource3">Urgente</asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource4">Emergencia</asp:ListItem>
                </asp:DropDownList>

				 <asp:Label ID="Label16" runat="server" Style="z-index: 110; left: 16px; position: absolute;
					  top: 284px" Text="Núcleo:" meta:resourcekey="Label16Resource1"></asp:Label>
				 <asp:DropDownList ID="DListNucleo" runat="server" Style="z-index: 111; left: 16px;
					  position: absolute; top: 304px" Width="110px" Enabled="False"  class="select"
					 meta:resourcekey="DListNucleoResource1">
				 </asp:DropDownList>

                <asp:Panel ID="Panel2" runat="server" BorderStyle="Inset" Height="100px" Style="z-index: 115; left: 335px; position: absolute; top: 240px"
                    Width="412px" Enabled="False">
                    <asp:Label ID="LblPanelCaracteristicas" runat="server" Style="z-index: 110; left: 22px; position: absolute;
					  top: 5px" Text="Características" meta:resourcekey="LblPanelCaracteristicasResource1"></asp:Label>
                    <asp:CheckBox ID="CheckIntrusion" runat="server" Checked="True" Style="z-index: 103; left: 22px; position: absolute; top: 40px"
                        Text="   Intrusión" OnCheckedChanged="CheckIntrusion_CheckedChanged" AutoPostBack="true"
                        meta:resourcekey="CheckIntrusionResource1" />
                    <asp:CheckBox ID="CheckIntruido" runat="server" Checked="True" Style="z-index: 104; left: 22px; position: absolute; top: 70px"
                        Text="   Intruido"
                        meta:resourcekey="CheckIntruidoResource1" />
                    <asp:CheckBox ID="CheckGrabacion" runat="server" Checked="True" Style="z-index: 105; left: 152px; position: absolute; top: 40px"
                        Text="   Grabación ED-137"
                        meta:resourcekey="CheckGrabacionResource1" />
                </asp:Panel>

                <asp:Panel ID="PanelAbonado" runat="server" BorderStyle="Inset" Height="222px" Style="z-index: 115;
							left: 335px; position: absolute; top: 5px" Width="412px" Enabled="False" 
					 meta:resourcekey="PanelAbonadoResource1">
					 <asp:Button ID="BtAceptarAbonado" runat="server" CausesValidation="False" OnClick="BtAceptarAbonado_Click"
						  Style="z-index: 115; left: 265px; position: absolute; top: 195px" Text="Aceptar"
						  Visible="False" Width="62px" meta:resourcekey="BtAceptarAbonadoResource1" />
					 <asp:Label ID="Label18" runat="server" Style="z-index: 116; left: 7px; position: absolute;
						  top: 3px" Text="Abonados a los que atiende:" meta:resourcekey="Label18Resource1"></asp:Label>
					 <asp:Label ID="Label19" runat="server" Style="z-index: 117; left: 273px; position: absolute;
						  top: 25px" Text="Abonado:" Visible="False" meta:resourcekey="Label19Resource1"></asp:Label>
					 <asp:TextBox ID="TxtAbonado" runat="server" MaxLength="32" Style="z-index: 118; left: 273px;
						  position: absolute; top: 45px" Visible="False" Width="108px" 
						 meta:resourcekey="TxtAbonadoResource1"></asp:TextBox>
					 <asp:Button ID="BtCancelarAbonado" runat="server" CausesValidation="False" OnClick="BtCancelarAbonado_Click"
						  Style="z-index: 119; left: 330px; position: absolute; top: 195px" Text="Cancelar"
						  Visible="False" Width="72px" meta:resourcekey="BtCancelarAbonadoResource1" />
					 <asp:Label ID="Label20" runat="server" Style="z-index: 120; left: 273px; position: absolute;
						  top: 82px" Text="Red:" Visible="False" meta:resourcekey="Label20Resource1"></asp:Label>
					 <asp:DropDownList ID="DListPrefijos" runat="server" Style="z-index: 121; left: 273px;
						  position: absolute; top: 105px" Visible="False" Enabled="False"  class="select"
						 OnSelectedIndexChanged="DListPrefijos_SelectedIndexChange" AutoPostBack="True" 
						 meta:resourcekey="DListPrefijosResource1">
					 </asp:DropDownList>
					 <asp:Button ID="BtNuevoAbonado" runat="server" CausesValidation="False" OnClick="BtNuevoAbonado_Click"
						  Style="z-index: 123; left: 273px; position: absolute; top: 161px" Text="Abonado Nuevo"
						  Visible="False" Width="109px" meta:resourcekey="BtNuevoAbonadoResource1" />
					<asp:GridView ID="GVAbonados" runat="server" Style="z-index: 126; left: 5px; position: absolute;
						top: 23px" AutoGenerateColumns="False" CellPadding="4" SkinId="GridViewSkin" 
						 GridLines="None" PageSize="5"  BorderStyle="None"
                         BorderWidth="1px" EmptyDataText="No data in the data source."
						OnRowDeleting="GVAbonados_OnRowDeleting" AllowPaging="True" 
						OnPageIndexChanging="GVAbonados_OnSelectedIndexChanging" Height="159px" Width="257px" 
						 meta:resourcekey="GVAbonadosResource1">
						<Columns>
							<asp:CommandField meta:resourcekey="CommandFieldResource1" 
								ShowDeleteButton="True" />
							<asp:BoundField DataField="IdRed" HeaderText="Red" 
								meta:resourcekey="BoundFieldResource1" />
							<asp:BoundField DataField="IdAbonado" HeaderText="Número Abonado" 
								meta:resourcekey="BoundFieldResource2" />
							<asp:BoundField DataField="IdPrefijo" meta:resourcekey="BoundFieldResource3" 
								Visible="False" />
						</Columns>
                        <HeaderStyle Height="10px"/>
                        <PagerStyle  HorizontalAlign="Center" />
					</asp:GridView>
					<asp:LinkButton ID="LkBRangos" runat="server" Style="z-index: 117; left: 273px;
						position: absolute; top: 134px" OnClick="LkBRangos_OnClick" Visible="False" SkinId="LinkButtonCabecera" 
						 meta:resourcekey="LkBRangosResource1">Ver Rangos</asp:LinkButton>

                    <asp:GridView ID="GVRangos" runat="server" Visible="False" Style="z-index: 117; left: 5px; position: absolute;
						top: 35px" AutoGenerateColumns="False" SkinId="GridViewSkin" BorderStyle="None" 
						BorderWidth="1px" CellPadding="3" CellSpacing="2" Height="159px" PageSize="5" 
						 Width="257px" meta:resourcekey="GVRangosResource1">
						<Columns>
							<asp:BoundField DataField="Tipo" HeaderText="Tipo" 
								meta:resourcekey="BoundFieldResource4" />
							<asp:BoundField DataField="Inicial" HeaderText="Inicial" 
								meta:resourcekey="BoundFieldResource5" />
							<asp:BoundField DataField="Final" HeaderText="Final" 
								meta:resourcekey="BoundFieldResource6" />
						</Columns>
						<HeaderStyle Height="10px"/>
					</asp:GridView>
				</asp:Panel>
		</asp:View>
			<asp:View ID="ViewUtilidades" runat="server">
                <asp:Panel ID="PanelComunes" runat="server" Style="position:absolute; left:15px">
				     <asp:CheckBox ID="CheckEscucha" runat="server" Style="z-index: 129; position: absolute;
					      top: 44px" Text="Escucha"  Enabled="False"  Width="300px" meta:resourcekey="CheckEscuchaResource1"/>
				     <asp:CheckBox ID="CheckRetener" runat="server" Style="z-index: 130; position: absolute;
					      top: 74px" Checked="True" Text="Retener"  Enabled="False"  Width="300px"
					     meta:resourcekey="CheckRetenerResource1"/>
				     <asp:CheckBox ID="CheckTransDirect" runat="server" Style="z-index: 153; position: absolute;
					      top: 104px" Text="Transferencia Directa" Width="300px"  Enabled="False" 
					     meta:resourcekey="CheckTransDirectResource1" />
				     <asp:CheckBox ID="CheckTeclaPrio" runat="server" Checked="True" Style="z-index: 135; position: absolute; top: 134px" 
                         Text="Tecla de prioridad"  Enabled="False"  Width="300px"
					     Visible="false" meta:resourcekey="CheckTeclaPrioResource1"/>
                </asp:Panel>
                <asp:Panel ID="PanelTwr" runat="server" Style="position:absolute; left:185px">
				     <asp:CheckBox ID="CheckLTT" runat="server" Checked="True" Style="z-index: 155; position: absolute; top: 44px" 
                         Text="LTT" Width="300px"  Enabled="False" Visible="false"
					     meta:resourcekey="CheckLTTResource1"/>
                    <asp:CheckBox ID="CheckCaptura" runat="server" Style="z-index: 131; position: absolute; left: 190px; 
					      top: 44px" Text="Captura"  Enabled="False"  Width="300px"
					     meta:resourcekey="CheckCapturaResource1"/>
                </asp:Panel>
                <asp:Panel ID="PanelAsecna" runat="server" Style="position:absolute; left:185px">
				     <asp:CheckBox ID="CheckConferencia" runat="server" Style="z-index: 128;
					      position: absolute; top: 44px" Text="Conferencia"  Enabled="False" Width="300px"
					     meta:resourcekey="CheckConferenciaResource1"/>
				     <asp:CheckBox ID="CheckRecording" runat="server" Checked="True" Style="z-index: 158;
					      position: absolute; top: 74px" Text="Grabación Local en Puesto" 
					     Width="300px"  Enabled="False" meta:resourcekey="CheckRecordingResource1"/>
                </asp:Panel>
                <asp:Panel ID="PanelNoImplementadas" runat="server" Style="position:absolute; left:375px">
				     <asp:CheckBox ID="CheckRedireccion" runat="server" Style="z-index: 132; position: absolute;
					      top: 74px" Text="Redirección"  Enabled="False"  Width="300px"
					     meta:resourcekey="CheckRedireccionResource1"/>
				     <asp:CheckBox ID="CheckRepUltLlamada" runat="server" Checked="True" Style="z-index: 133; 
					      position: absolute; top: 104px" Text="Repetición de la última llamada"   Width="300px"
					     Enabled="False" meta:resourcekey="CheckRepUltLlamadaResource1"/>
				     <asp:CheckBox ID="CheckReAutomatica" runat="server" Style="z-index: 134; position: absolute;
					      top: 134px" Text="Rellamada automática"  Enabled="False"  Width="300px"
					     meta:resourcekey="CheckReAutomaticaResource1"/>
				     <asp:CheckBox ID="CheckTecla55" runat="server" Style="z-index: 136; position: absolute;
					      top: 164px" Text="Tecla 55+1"  Enabled="False"  Width="300px"
					     meta:resourcekey="CheckTecla55Resource1"/>
				     <asp:CheckBox ID="CheckMonitoring" runat="server" Checked="True" Style="z-index: 137; 
					      position: absolute; top: 194px" Text="Monitoring" Enabled="False"  Width="300px"
					     meta:resourcekey="CheckMonitoringResource1"/>
				     <asp:CheckBox ID="CheckCoordTF" runat="server" Style="z-index: 138; position: absolute;
					      top: 224px" Text="Coordinador Telefonía"  Enabled="False"  Width="300px"
					     meta:resourcekey="CheckCoordTFResource1"/>
				     <asp:CheckBox ID="CheckLlamadaSelect" runat="server" Style="z-index: 139; position: absolute; left:200px;
					      top: 44px" Text="Llamada Selectiva"  Enabled="False"  Width="300px"
					     meta:resourcekey="CheckLlamadaSelectResource1"/>
				     <asp:CheckBox ID="CheckIntRDTF" runat="server" Style="z-index: 140;position: absolute;left:200px;
					      top: 74px" Text="Integración de Radio y Telefonía"  Enabled="False"  Width="300px"
					     meta:resourcekey="CheckIntRDTFResource1"/>
				     <asp:CheckBox ID="CheckCoorRD" runat="server" Style="z-index: 141;position: absolute;left:200px;
					      top: 104px" Text="Coordinador Radio"  Enabled="False"  Width="300px"
					     meta:resourcekey="CheckCoorRDResource1"/>
				     <asp:CheckBox ID="CheckBSS" runat="server" Checked="True" Style="z-index: 154;left:200px;
					      position: absolute; top: 134px" Text="Grupo BSS" Width="300px"  Enabled="False" 
					     meta:resourcekey="CheckBSSResource1"/>
				     <asp:CheckBox ID="CheckRediCA" runat="server" Style="z-index: 156; left: 405px; position: absolute;left:200px;
					      top: 164px" Text="   Inhabilitación Redirección C/A" Width="300px"  Enabled="False" 
					     meta:resourcekey="CheckRediCAResource1"/>
				     <asp:CheckBox ID="CheckSayAgain" runat="server" Checked="True" Style="z-index: 157;left:200px;
					      position: absolute; top: 194px" Text="Say Again" Width="300px"  Enabled="False" 
					     meta:resourcekey="CheckSayAgainResource1"/>
				     <asp:CheckBox ID="CheckTransPre" runat="server" Checked="True" Style="z-index: 158;left:200px;
					      position: absolute; top: 224px" Text="Transferencia con consulta previa habilitada" 
					     Width="300px"  Enabled="False" meta:resourcekey="CheckTransPreResource1"/>
                 </asp:Panel>
			</asp:View>
<%--			<asp:View ID="ViewParametros" runat="server">
				<asp:TextBox ID="TxtKAP" runat="server" Style="z-index: 201; left: 312px;
					position: absolute; top: 23px" Width="33px" Enabled="False" 
					meta:resourcekey="TxtKAPResource1">200</asp:TextBox>
				<asp:Label ID="LblKAP" runat="server" Style="z-index: 199; left: 52px; position: absolute;
					top: 26px" Text="Keep Alive Period (msg):" Width="180px" 
					meta:resourcekey="LblKAPResource1"></asp:Label>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="TxtKAP"
					ErrorMessage="Es necesario introducir un valor entre 20 y 1000." Style="z-index: 203;
					left: 370px; position: absolute; top: 24px" 
					meta:resourcekey="RequiredFieldValidator12Resource1">*</asp:RequiredFieldValidator>
				<asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="El valor debe estar comprendido entre 20 y 1000"
					Height="20px" MaximumValue="1000" MinimumValue="20" Style="z-index: 204; left: 370px;
					position: absolute; top: 24px" Width="12px" ControlToValidate="TxtKAP" Type="Integer" 
					meta:resourcekey="RangeValidator1Resource1">*</asp:RangeValidator>

				<asp:Label ID="LblKAM" runat="server" Height="21px" Style="z-index: 200; left: 390px;
					position: absolute; top: 26px" Text="Keep Alive Multiplier:" Width="154px" 
					meta:resourcekey="LblKAMResource1"></asp:Label>
				<asp:TextBox ID="TxtKAM" runat="server" Height="15px" Style="z-index: 202;
					left: 542px; position: absolute; top: 23px" Width="33px" Enabled="False" 
					meta:resourcekey="TxtKAMResource1">10</asp:TextBox>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="TxtKAM"
					ErrorMessage="Es necesario introducir un valor entre 2 y 50." Style="z-index: 205;
					left: 598px; position: absolute; top: 25px" Width="4px" 
					meta:resourcekey="RequiredFieldValidator13Resource1">*</asp:RequiredFieldValidator>
				<asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="TxtKAM"
					ErrorMessage="El valor debe estar comprendido entre 2 y 50" MaximumValue="50"
					MinimumValue="2" Style="z-index: 207; left: 598px; position: absolute; top: 25px"
					Type="Integer" meta:resourcekey="RangeValidator2Resource1">*</asp:RangeValidator>
				 
				 <asp:Label ID="Label8" runat="server" Style="z-index: 169; left: 52px; position: absolute;
					  top: 70px" Text="Número llamadas entrantes en cola IDA:" 
					meta:resourcekey="Label8Resource1"></asp:Label>
				 <asp:TextBox ID="TxtLlamEntIDA" runat="server" Style="z-index: 170; left: 312px; position: absolute;
					  top: 68px" Width="39px" Enabled="False" meta:resourcekey="TxtLlamEntIDAResource1">3</asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TxtLlamEntIDA"
					  ErrorMessage="El campo número de llamadas entrantes en cola IDA no puede estar vacio."
					  Style="z-index: 147; left: 370px; position: absolute; top: 73px" 
					meta:resourcekey="RequiredFieldValidator6Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="TxtLlamEntIDA"
					  ErrorMessage="El campo número de llamadas entrantes en cola IDA debe ser numérico."
					  Operator="DataTypeCheck" Style="z-index: 189; left: 370px; position: absolute;
					  top: 73px" Type="Integer" meta:resourcekey="CompareValidator1Resource1">*</asp:CompareValidator>
					  
				 <asp:Label ID="Label9" runat="server" Style="z-index: 171; left: 390px; position: absolute;
					  top: 71px" Text="Número llamadas IDA:" Width="154px" 
					meta:resourcekey="Label9Resource1"></asp:Label>
				 <asp:TextBox ID="TxtLlamIDA" runat="server" Style="z-index: 172; left: 542px; position: absolute;
					  top: 67px" Width="32px" Enabled="False" meta:resourcekey="TxtLlamIDAResource1">4</asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="TxtLlamIDA"
					  ErrorMessage="El campo número de llamadas IDA no puede estar vacio." Style="z-index: 150;
					  left: 596px; position: absolute; top: 73px" 
					meta:resourcekey="RequiredFieldValidator9Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="TxtLlamIDA"
					  ErrorMessage="El campo número de llamadas IDA debe ser numérico." Operator="DataTypeCheck"
					  Style="z-index: 192; left: 596px; position: absolute; top: 73px" Type="Integer" 
					meta:resourcekey="CompareValidator4Resource1">*</asp:CompareValidator>

				 <asp:Label ID="Label10" runat="server" Style="z-index: 173; left: 52px; position: absolute;
					  top: 114px" Text="Número de Frecuencias por Página:" 
					meta:resourcekey="Label10Resource1"></asp:Label>
				 <asp:TextBox ID="TxtFrecPag" runat="server" Style="z-index: 175; left: 312px; position: absolute;
					  top: 113px" Width="39px" Enabled="False" meta:resourcekey="TxtFrecPagResource1">15</asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="TxtFrecPag"
					  ErrorMessage="El campo número de frecuencias por página no puede estar vacio."
					  Style="z-index: 148; left: 369px; position: absolute; top: 116px" 
					meta:resourcekey="RequiredFieldValidator7Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="TxtFrecPag"
					  ErrorMessage="El campo número de frecuencias por página debe ser numérico." Operator="DataTypeCheck"
					  Style="z-index: 190; left: 369px; position: absolute; top: 116px" Type="Integer" 
					meta:resourcekey="CompareValidator2Resource1">*</asp:CompareValidator>
				<asp:RangeValidator ID="RangeValidator3" runat="server" MinimumValue="1" 
					MaximumValue="15" ControlToValidate="TxtFrecPag"
											ErrorMessage="El campo número de frecuencias por página debe estar comprendido entre 1 y 15." 
											Style="z-index: 148; left: 369px; position: absolute; top: 116px" 
					meta:resourcekey="RangeValidator3Resource1">*</asp:RangeValidator>
				 <asp:Label ID="Label11" runat="server" Style="z-index: 174; left: 390px; position: absolute;
					  top: 114px" Text="Núm. de Pág. de Frec.:" Width="154px" 
					meta:resourcekey="Label11Resource1"></asp:Label>
				 <asp:TextBox ID="TxtPagFrec" runat="server" Style="z-index: 176; left: 542px; position: absolute;
					  top: 112px" Width="32px" Enabled="False" meta:resourcekey="TxtPagFrecResource1">9</asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="TxtPagFrec"
					  ErrorMessage="El campo número de páginas de frecuencias no puede estar vacio."
					  Style="z-index: 151; left: 596px; position: absolute; top: 113px" 
					meta:resourcekey="RequiredFieldValidator10Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="TxtPagFrec"
					  ErrorMessage="El campo número de páginas de frecuencias debe ser numérico." Operator="DataTypeCheck"
					  Style="z-index: 193; left: 596px; position: absolute; top: 113px" Type="Integer" 
					meta:resourcekey="CompareValidator5Resource1">*</asp:CompareValidator>

				 <asp:Label ID="Label12" runat="server" Style="z-index: 177; left: 52px; position: absolute;
					  top: 158px" Text="Número de Enlaces Internos por Página:" 
					meta:resourcekey="Label12Resource1"></asp:Label>
				 <asp:TextBox ID="TxtEnlIntPag" runat="server" Style="z-index: 179; left: 312px; position: absolute;
					  top: 158px" Width="39px" Enabled="False" meta:resourcekey="TxtEnlIntPagResource1">15</asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TxtEnlIntPag"
					  ErrorMessage="El campo número de enlaces internos por página no puede estar vacio."
					  Style="z-index: 149; left: 369px; position: absolute; top: 161px" 
					meta:resourcekey="RequiredFieldValidator8Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="TxtEnlIntPag"
					  ErrorMessage="El campo número de enlaces internos por página debe ser numérico."
					  Operator="DataTypeCheck" Style="z-index: 191; left: 369px; position: absolute;
					  top: 161px" Type="Integer" meta:resourcekey="CompareValidator3Resource1">*</asp:CompareValidator>
				<asp:RangeValidator ID="RangeValidator4" runat="server" MinimumValue="1" 
					MaximumValue="20" ControlToValidate="TxtEnlIntPag"
											ErrorMessage="El campo número de enlaces internos por página debe estar comprendido entre 1 y 20." 
											Style="z-index: 149; left: 369px; position: absolute; top: 161px" 
					meta:resourcekey="RangeValidator4Resource1">*</asp:RangeValidator>

				 <asp:Label ID="Label14" runat="server" Style="z-index: 178; left: 390px; position: absolute;
					  top: 158px" Text="Núm. Pág. Enlaces Int.:" Width="154px" 
					meta:resourcekey="Label14Resource1"></asp:Label>
				 <asp:TextBox ID="TxtPagEnlInt" runat="server" Style="z-index: 180; left: 542px; position: absolute;
					  top: 158px" Width="32px" Enabled="False" meta:resourcekey="TxtPagEnlIntResource1">3</asp:TextBox>        
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="TxtPagEnlInt"
					  ErrorMessage="El campo número de páginas de enlaces internos no puede estar vacio."
					  Style="z-index: 152; left: 596px; position: absolute; top: 162px" 
					meta:resourcekey="RequiredFieldValidator11Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="TxtPagEnlInt"
					  ErrorMessage="El campo número de páginas de enlaces internos debe ser numérico."
					  Operator="DataTypeCheck" Style="z-index: 194; left: 596px; position: absolute;
					  top: 162px" Type="Integer" meta:resourcekey="CompareValidator6Resource1">*</asp:CompareValidator>
			</asp:View>--%>

            <asp:View ID="ViewPermisosRedes" runat="server">
					<asp:GridView ID="GVPermisosRedes" runat="server" Style="z-index: 126; left: 55px; position: absolute;
						top: 23px" AutoGenerateColumns="False" CellPadding="4" SkinId="GridViewSkin" 
						 GridLines="None" PageSize="5"  BorderStyle="None"
                         BorderWidth="1px" EmptyDataText="No data." 
                        OnPageIndexChanging="GridView_OnSelectedIndexChange"
						AllowPaging="True" 
						Height="159px" Width="257px" 
						meta:resourcekey="GVPermisosRedesResource1">
						<Columns>
							<asp:BoundField DataField="IdRed" HeaderText="Red"
								meta:resourcekey="BoundFieldResource7" />
							<asp:CheckBoxField DataField="Llamar" HeaderText="Llamar" ItemStyle-HorizontalAlign="Center"
								meta:resourcekey="BoundFieldResource8" />
							<asp:CheckBoxField DataField="Recibir" HeaderText="Recibir" ItemStyle-HorizontalAlign="Center"
								meta:resourcekey="BoundFieldResource9" />
						</Columns>
						<HeaderStyle  Height="10px"/>
                        <PagerStyle HorizontalAlign="Center" />
					</asp:GridView>
            </asp:View>

			<asp:View ID="ViewAgenda" runat="server">
					<asp:GridView ID="GVAgenda" runat="server" Style="z-index: 126; left: 55px; position: absolute;
						top: 23px" AutoGenerateColumns="False" CellPadding="4" SkinId="GridViewSkin" 
						 GridLines="None" PageSize="5" BorderStyle="None"
                         BorderWidth="1px" EmptyDataText="No data in the data source."
						OnRowDeleting="GVagenda_OnRowDeleting" AllowPaging="True" 
						OnPageIndexChanging="GVAgenda_OnSelectedIndexChanging" Height="159px" Width="257px" 
						meta:resourcekey="GVAgendaResource1">
						<Columns>
							<asp:CommandField meta:resourcekey="CommandFieldResource2" 
								ShowDeleteButton="True" Visible="False" />
							<asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left"
								meta:resourcekey="BoundFieldResource7" />
							<asp:BoundField DataField="Prefijo" HeaderText="Prefijo" ItemStyle-HorizontalAlign="Left"
								meta:resourcekey="BoundFieldResource10" />
							<asp:BoundField DataField="Numero" HeaderText="Número" ItemStyle-HorizontalAlign="Left" 
								meta:resourcekey="BoundFieldResource11" />
						</Columns>
						<HeaderStyle Height="10px"/>
                        <PagerStyle HorizontalAlign="Center" />
					</asp:GridView>


				<asp:Label ID="LlbAgenda" runat="server" 
					style="position: absolute; top: 5px; left: 55px; z-index: 2; height: 16px; width: auto" 
					Text="Destinos Agenda" meta:resourcekey="LlbAgendaResource1"></asp:Label>
				<asp:Button ID="BtnAnadirAgenda" runat="server" Text="Añadir" Enabled="False"
					style="position: absolute; top: 187px; left: 384px" onclick="OnBtnAnadirAgenda_Click" 
						meta:resourcekey="BtnAnadirAgendaResource1" />
				<asp:Label ID="LblNombre" runat="server" 
					style="position: absolute; top: 132px; left: 478px; z-index: 2; height: 16px; width: 50px" 
					Text="Nombre" Visible="False" meta:resourcekey="LblNombreResource1"></asp:Label>
				<asp:TextBox ID="TBNombreAgenda" runat="server" 
					style="z-index: 1; left: 537px; top: 132px; position: absolute" Visible="False" 
						meta:resourcekey="TBNombreAgendaResource1"></asp:TextBox>
				<asp:Label ID="LblPrefijo" runat="server" 
					style="position: absolute; top: 164px; left: 478px; z-index: 2; height: 17px; width: 50px" 
					Text="Prefijo" Visible="False" meta:resourcekey="LblPrefijoResource1"></asp:Label>
				<asp:DropDownList ID="DDLPrefijoAgenda" runat="server"  class="select"
					style="z-index: 1; left: 537px; top: 163px; width: 100px; position: absolute" 
						Visible="False" meta:resourcekey="DDLPrefijoAgendaResource1">
				</asp:DropDownList>
				<asp:Label ID="LblNumero" runat="server" 
					style="position: absolute; top: 191px; left: 478px; z-index: 2; height: 17px; width: 50px" 
					Text="Número" Visible="False" meta:resourcekey="LblNumeroResource1"></asp:Label>
				<asp:TextBox ID="TBNumeroAgenda" runat="server" 
					style="z-index: 1; left: 537px; top: 192px; position: absolute" Visible="False" 
						meta:resourcekey="TBNumeroAgendaResource1"></asp:TextBox>
				<asp:CheckBox ID="CbCompartir" runat="server" Checked="false"
					style="position: absolute; top: 218px; left: 478px; z-index: 2; height: 17px; width: auto" 
					Text="¿Compartir con todos los sectores?" Visible="False" meta:resourcekey="LblCompartirResource1"></asp:CheckBox>
			</asp:View>

			<asp:View ID="ViewNivelesItrusion" runat="server">
				 <asp:Label ID="Label1" runat="server" Style="z-index: 100; left: 43px; position: absolute;
					  top: 24px" Text="CICL:" meta:resourcekey="Label1Resource1"></asp:Label>
					<asp:DropDownList ID="DDLCicl" runat="server" Style="z-index: 117; left: 113px;
						position: absolute; top: 21px" Enabled="False" meta:resourcekey="DDLCiclResource1" class="select">
						<asp:ListItem Value="0" meta:resourcekey="ListItemResource10">No Urgente</asp:ListItem>
						<asp:ListItem Value="1" meta:resourcekey="ListItemResource11">Normal</asp:ListItem>
						<asp:ListItem Value="2" meta:resourcekey="ListItemResource12">Urgente</asp:ListItem>
						<asp:ListItem Value="3" meta:resourcekey="ListItemResource13">Emergencia</asp:ListItem>
					</asp:DropDownList>

				 <asp:Label ID="Label4" runat="server" Style="z-index: 104; left: 43px; position: absolute;
					  top: 194px" Text="CPIPCL:" meta:resourcekey="Label4Resource1"></asp:Label>
					<asp:DropDownList ID="DDLCpipcl" runat="server" Style="z-index: 117; left: 113px;
						position: absolute; top: 193px" Enabled="False" meta:resourcekey="DDLCpipclResource1" class="select">
						<asp:ListItem Value="0" meta:resourcekey="ListItemResource14">No Urgente</asp:ListItem>
						<asp:ListItem Value="1" meta:resourcekey="ListItemResource15">Normal</asp:ListItem>
						<asp:ListItem Value="2" meta:resourcekey="ListItemResource16">Urgente</asp:ListItem>
						<asp:ListItem Value="3" meta:resourcekey="ListItemResource17">Emergencia</asp:ListItem>
					</asp:DropDownList>

				 <asp:Label ID="Label3" runat="server" Style="z-index: 108; left: 43px; position: absolute;
					  top: 135px" Text="CIPL:" meta:resourcekey="Label3Resource1"></asp:Label>
					<asp:DropDownList ID="DDLCipl" runat="server" Style="z-index: 117; left: 113px;
						position: absolute; top: 135px" Enabled="False" meta:resourcekey="DDLCiplResource1" class="select">
						<asp:ListItem Value="0" meta:resourcekey="ListItemResource18">No Urgente</asp:ListItem>
						<asp:ListItem Value="1" meta:resourcekey="ListItemResource19">Normal</asp:ListItem>
						<asp:ListItem Value="2" meta:resourcekey="ListItemResource20">Urgente</asp:ListItem>
						<asp:ListItem Value="3" meta:resourcekey="ListItemResource21">Emergencia</asp:ListItem>
					</asp:DropDownList>

				 <asp:Label ID="Label7" runat="server" Style="z-index: 112; left: 43px; position: absolute;
					  top: 79px" Text="CPICL:" meta:resourcekey="Label7Resource1"></asp:Label>
 					<asp:DropDownList ID="DDLCpicl" runat="server" Style="z-index: 117; left: 113px;
						position: absolute; top: 78px" Enabled="False" meta:resourcekey="DDLCpiclResource1" class="select">
						<asp:ListItem Value="0" meta:resourcekey="ListItemResource22">No Urgente</asp:ListItem>
						<asp:ListItem Value="1" meta:resourcekey="ListItemResource23">Normal</asp:ListItem>
						<asp:ListItem Value="2" meta:resourcekey="ListItemResource24">Urgente</asp:ListItem>
						<asp:ListItem Value="3" meta:resourcekey="ListItemResource25">Emergencia</asp:ListItem>
					</asp:DropDownList>
			</asp:View>
		
        </asp:MultiView>
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

   
    <asp:LinkButton ID="BtLC" runat="server" Style="z-index: 103; left: 725px; position: absolute;
        top: 430px; width: auto;" Text="Panel Línea Caliente" Visible="False" 
		 OnClick="BtLC_Click" CausesValidation="False" SkinId="LinkButtonCabecera" 
		  meta:resourcekey="BtLCResource1" />
    <asp:LinkButton ID="BtTelefonia" runat="server" Style="z-index: 105; left: 494px; position: absolute;
        top: 430px; width: auto; margin-right: 0px;" 
		 Text="Panel Telefonía" Visible="False" 
		 OnClick="BtTelefonia_Click" CausesValidation="False" SkinId="LinkButtonCabecera" 
		  meta:resourcekey="BtTelefoniaResource1" />
    <asp:LinkButton ID="BtRadio" runat="server" OnClick="Button2_Click" Style="z-index: 120;
        left: 289px; position: absolute; top: 430px; width: auto;" 
		 Text="Panel Radio" Visible="False" CausesValidation="False" SkinId="LinkButtonCabecera" 
		  meta:resourcekey="BtRadioResource1" />

    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 108; left: 501px; position: absolute;
        top: 441px" Text="Aceptar" Width="80px" Visible="False" UseSubmitBehavior="true"  
		 OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
      <ajaxToolKit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
      </ajaxToolKit:ConfirmButtonExtender>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores:"
        Style="z-index: 182; left: 294px; position: absolute; top: 474px" 
		 meta:resourcekey="ValidationSummary1Resource1" />
</asp:Content>

