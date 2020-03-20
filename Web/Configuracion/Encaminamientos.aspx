<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="Encaminamientos.aspx.cs" Inherits="Encaminamientos" 
	 CodeFileBaseClass=" PageBaseCD40.PageCD40" Title="Gestión de Encaminamientos" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	 <script type="text/javascript">
	     function AbreVentana(ventana) {
	         window.open(ventana);
	     }

		//esta funcion solo deja ingresar numeros enteros
		function entero(e) {
			var caracter
			caracter = e.keyCode
			status = caracter

			if (caracter > 47 && caracter < 58) {
				return true
			}
			return false

		}

	</script>
    <asp:Label ID="Label2" runat="server" Text="GESTIÓN DE ENCAMINAMIENTOS" 
		 CssClass="labelPagina" meta:resourcekey="Label2Resource1" ></asp:Label>        

	<asp:LinkButton ID="LBMiCentral" runat="server" Style="z-index: 110; left: 625px; position: absolute;
		top: 60px" Visible="False" OnClick="LBMiCentral_Click" SkinId="LinkButtonCabecera"
		 meta:resourcekey="LBMiCentralResource1">Ir al SCV del Sistema</asp:LinkButton>

    <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"
				SkinId="MascaraListaElementos" meta:resourcekey="ListBox1Resource1">
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
	 <asp:LinkButton ID="BtModificar" runat="server" OnClick="BtModificar_Click" 
		 CausesValidation="False" Text="Modificar" SkinID="MascaraBotonModificar" 
		 meta:resourcekey="BtModificarResource1"/>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores"
        Style="z-index: 104; left: 220px; position: absolute; top: 516px" 
		 Visible="False" Width="450px" meta:resourcekey="ValidationSummary1Resource1" />

	<asp:Button ID="IBCentralATS" runat="server" SkinId="ButtonTab" CausesValidation="false"
		Style="z-index: 105; left: 221px; position: absolute; top: 70px" 
		  OnClick="OnButtonTab_Click" meta:resourcekey="IBCentralATSResource1" />
	<asp:Button ID="IBRangos" runat="server" SkinId="ButtonTab" CausesValidation="false"
		Style="z-index: 106; left: 325px; position: absolute; top: 70px" 
		 OnClick="OnButtonTab_Click" meta:resourcekey="IBRangosResource1" />
	<asp:Button ID="IBRutas" runat="server" SkinId="ButtonTab" CausesValidation="false"
		Style="z-index: 107; left: 429px; position: absolute; top: 70px" 
		 OnClick="OnButtonTab_Click" meta:resourcekey="IBRutasResource1" />
	
	<asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Height="235px" Style="z-index: 108;
		left: 211px; position: absolute; top: 86px" Width="533px" DefaultButton="BtAceptar"
		 meta:resourcekey="Panel1Resource1">
		<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">

			<asp:View ID="ViewCentralATS" runat="server">
				 <asp:Label ID="Label1" runat="server" Style="z-index: 100; left: 21px; position: absolute;
					  top: 18px; height: 19px; width: 150px;" Text="Id de SCV:" 
                     meta:resourcekey="Label1Resource1" ></asp:Label>
				 <asp:TextBox ID="TxtCentral" runat="server" Style="z-index: 101; left: 21px; position: absolute;
					  top: 38px" CausesValidation="True" Enabled="False" 
					 meta:resourcekey="TxtCentralResource1"></asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtCentral"
					  ErrorMessage="El identificador de SCV no  puede ser vacío." Style="z-index: 102;
					  left: 200px; position: absolute; top: 38px" 
					 meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
				 
                <asp:CheckBox ID="Throwswitching" runat="server" Style="z-index: 103; left: 20px; position: absolute;
					  top: 110px" Text="Throughswitching" TextAlign="Left" Enabled="False" visible="false"
					 meta:resourcekey="ThrowswitchingResource1"/>
				 <asp:CheckBox ID="CentralPropia" runat="server" Style="z-index: 104; left: 20px; position: absolute;
					  top: 84px" Text="Datos Propios" TextAlign="Left" Enabled="False" 
					 AutoPostBack="True" oncheckedchanged="CBCentralPropia_OnCheckedChanged" 
					 meta:resourcekey="CentralPropiaResource1"/>
				<asp:CheckBox ID="CBCentralIp" runat="server" Style="z-index: 105; left: 20px; position: absolute;
					  top: 134px" Text="SCV IP" TextAlign="Left" Enabled="False" 
					AutoPostBack="True" oncheckedchanged="CBCentralIP_OnCheckedChanged" 
					meta:resourcekey="CentralIPResource1"/>
            
            <table id="TblCentralIp" runat="server" style="position:absolute; left:20px;top:220px;" visible="false">
              <tr align="center">
                  <td>
                  </td>
                  <td>
                        <asp:Label ID="LblIp1" runat="server" meta:resourcekey="LblIp1Resource"/>
                  </td>
                  <td>
                        <asp:Label ID="LblIp2" runat="server" meta:resourcekey="LblIp2Resource"/>
                  </td>
                  <td>
                        <asp:Label ID="LblIp3" runat="server" meta:resourcekey="LblIp3Resource"/>
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:Label ID="LbProxy" runat="server" meta:resourcekey="LbProxyResource"/>
                  </td>
                  <td>
                      <asp:TextBox ID="TbIp1" runat="server"  Enabled="false"   Width="150px" meta:resourcekey="TbIp1Resource"/>
                  </td>
                  <td>
                      <asp:TextBox ID="TbIp2" runat="server" Enabled="false"   Width="150px" meta:resourcekey="TbIp2Resource"/>
                  </td>
                  <td>
                      <asp:TextBox ID="TbIp3" runat="server" Enabled="false"   Width="150px"   meta:resourcekey="TbIp3Resource"/>
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:Label ID="LbPresencia" runat="server" meta:resourcekey="LbSrvPresenciaResource"/>
                  </td>
                  <td>
                      <asp:TextBox ID="TbSrvPresIp1" runat="server" Enabled="false"  Width="150px" meta:resourcekey="TbSrvPresIp1Resource" />
                  </td>
                  <td>
                      <asp:TextBox ID="TbSrvPresIp2" runat="server" Enabled="false"  Width="150px" meta:resourcekey="TbSrvPresIp2Resource"/>
                  </td>
                  <td>
                      <asp:TextBox ID="TbSrvPresIp3" runat="server" Enabled="false"  Width="150px" meta:resourcekey="TbSrvPresIp3Resource"/>
                  </td>
              </tr>                    
            </table>
				<asp:Label ID="LblRutas" runat="server" Height="92px" Style="z-index: 105; left: 354px;
					position: absolute; top: 18px" Text="Rutas" Width="85px" 
					 meta:resourcekey="LblRutasResource1"></asp:Label>
				<asp:ListBox ID="LBRutas" runat="server" Height="143px" Style="z-index: 106; left: 354px;
					position: absolute; top: 38px" Width="135px" meta:resourcekey="LBRutasResource1"></asp:ListBox>
				<asp:ImageButton ID="IBRutaArriba" runat="server" Style="z-index: 107; left: 497px; -webkit-transform: rotate(180deg); -moz-transform: rotate(180deg); -o-transform: rotate(180deg); transform: rotate(180deg);
					position: absolute; top: 58px" ImageUrl="~/Configuracion/Images/arrow.png" 
					 Enabled="False" OnClick="IBRutaArriba_Click" 
					 meta:resourcekey="IBRutaArribaResource1" />
				<asp:ImageButton ID="IBRutaAbajo" runat="server" Style="z-index: 109; left: 497px; 
					position: absolute; top: 128px" ImageUrl="~/Configuracion/Images/arrow.png" 
					 Enabled="False" OnClick="IBRutaAbajo_Click" 
					 meta:resourcekey="IBRutaAbajoResource1" />
				<asp:Label ID="LblNumTest" runat="server" 
					style="z-index: 1; left: 149px; top: 84px; position: absolute" 
					Text="Número Test:" Visible="False" meta:resourcekey="LblNumTestResource1"></asp:Label>
				<asp:TextBox ID="TBNumTest" runat="server" 
					style="z-index: 1; left: 270px; top: 83px; position: absolute; width: 84px" 
					 MaxLength="6"  Visible="False" Enabled="False" onkeypress="return entero(event)" 
					 meta:resourcekey="TBNumTestResource1" ></asp:TextBox>

		        <asp:RequiredFieldValidator ID="RFV_TbIp1" runat="server" ControlToValidate="TbIp1"
			        ErrorMessage="La dirección IP del servidor Proxy 1 no puede estar vacía." Style="z-index: 143;
			        left: 180px; position: absolute; top: 225px" Enabled="false" Visible="false"
			        meta:resourcekey="RFV_TbIp1Resource1">*</asp:RequiredFieldValidator>

			</asp:View>
		
			<asp:View ID="ViewRangos" runat="server">                
                <asp:GridView Style="Z-INDEX: 99; LEFT: 19px; POSITION: absolute; TOP: 20px"
                    ID="GViewRangos" runat="server" SkinId="GridViewSkin"
                    CellPadding="4" PageSize="7"
                    AutoGenerateColumns="False" AllowPaging="True" Height="175px"
                    OnRowCommand="GViewRangos_RowCommand"
                    OnRowDeleting="GViewRangos_RowDeleting" 
                    OnPageIndexChanging="GViewRangos_OnSelectedIndexChanging"
                    OnRowDataBound="GViewRangos_RowDataBound" Width="400px"
                    Enabled="False"
                    meta:resourcekey="GViewRangosResource1">
                    <Columns>
                        <asp:CommandField meta:resourcekey="CommandFieldResource1"
                            ShowDeleteButton="True" />
                        <asp:ButtonField Text="Editar" ButtonType="Link" CausesValidation="true" CommandName="Edicion" meta:resourcekey="ButtonEditarResource1"/>
                        <asp:TemplateField HeaderText="Tipo de Rango"
                            meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <asp:DropDownList ID="DListTipoRango" runat="server" DataValueField="Tipo" class="select"
                                    Enabled="False" meta:resourcekey="DListTipoRangoResource1" Width="100px">
                                    <asp:ListItem meta:resourcekey="ListItemResource1" Value="O">Operador</asp:ListItem>
                                    <asp:ListItem meta:resourcekey="ListItemResource2" Value="P">Privilegiado</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Inicial" HeaderText="Inicial"
                            meta:resourcekey="BoundFieldResource1">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Final" HeaderText="Final"
                            meta:resourcekey="BoundFieldResource2">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IdAbonado" HeaderText="Abonado"
                            meta:resourcekey="BoundFieldResource3">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IdRed" HeaderText="Red"
                            meta:resourcekey="BoundFieldResource4" />
                        <asp:BoundField DataField="IdPrefijo" HeaderText="Prefijo Red"
                            meta:resourcekey="BoundFieldResource5" Visible="False">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                <asp:Panel ID="PanelRangos" runat="server" Height="320px" Style="z-index: 101; left: 503px;
					position: absolute; top: 19px" Width="215px" BorderStyle="Inset" Visible="False" 
					meta:resourcekey="PanelRangosResource1">
					<asp:Label style="Z-INDEX: 107; LEFT: 5px; POSITION: absolute; TOP: 12px" 
						id="Label4" runat="server" Text="Tipo Rango:" 
						meta:resourcekey="Label4Resource1"></asp:Label>
					<asp:DropDownList style="Z-INDEX: 110; LEFT: 5px; POSITION: absolute; TOP: 32px" 
						id="DListTipoRango" runat="server" Width="120px"  class="select"
						meta:resourcekey="DListTipoRangoResource2">
					  <asp:ListItem Value="O" meta:resourcekey="ListItemResource3">Operador</asp:ListItem>
					  <asp:ListItem Value="P" meta:resourcekey="ListItemResource4">Privilegiado</asp:ListItem>
					</asp:DropDownList>

					<asp:Label style="Z-INDEX: 108; LEFT: 5px; POSITION: absolute; TOP: 58px" 
						id="Label5" runat="server" Text="Inicial:" meta:resourcekey="Label5Resource1"></asp:Label>
					<asp:TextBox style="Z-INDEX: 100; LEFT: 5px; POSITION: absolute; TOP: 78px" 
						id="TxtBInicial" runat="server" 
						MaxLength="6" Width="124px" TabIndex="1" meta:resourcekey="TxtBInicialResource1"></asp:TextBox>
					<asp:RequiredFieldValidator style="Z-INDEX: 101; LEFT: 180px; POSITION: absolute; TOP: 80px" 
						id="RequiredFieldValidator2" runat="server" 
						ErrorMessage="El valor Inicial es obligatorio" ControlToValidate="TxtBInicial" 
						meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RequiredFieldValidator>

					<asp:Label style="Z-INDEX: 109; LEFT: 5px; POSITION: absolute; TOP: 104px" 
						id="Label8" runat="server" Text="Final:" meta:resourcekey="Label8Resource1"></asp:Label>
					<asp:TextBox style="Z-INDEX: 106; LEFT: 5px; POSITION: absolute; TOP: 124px" 
						id="TxtBFinal" runat="server" MaxLength="6" Width="124px" TabIndex="2" 
						meta:resourcekey="TxtBFinalResource1"></asp:TextBox>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtBFinal"
						ErrorMessage="El valor Final es obligario." Style="z-index: 102; left: 180px;
						position: absolute; top: 130px" Width="4px" 
						meta:resourcekey="RequiredFieldValidator3Resource1">*</asp:RequiredFieldValidator>

					<asp:Label style="Z-INDEX: 104; LEFT: 5px; POSITION: absolute; TOP: 150px" 
						id="Label6" runat="server" Text="Abonado:" meta:resourcekey="Label6Resource1"></asp:Label>
					<asp:TextBox style="Z-INDEX: 103; LEFT: 5px; POSITION: absolute; TOP: 170px" 
						id="TxtAbonado" runat="server" MaxLength="15" Width="124px" TabIndex="3" 
						meta:resourcekey="TxtAbonadoResource1"></asp:TextBox>

					<asp:Label style="Z-INDEX: 105; LEFT: 5px; POSITION: absolute; TOP: 196px" 
						id="Label7" runat="server" Text="Red:" meta:resourcekey="Label7Resource1"></asp:Label>
					<asp:DropDownList style="Z-INDEX: 111; LEFT: 5px; POSITION: absolute; TOP: 216px" 
						id="DListPrefijo" runat="server" Width="98px" TabIndex="4"  class="select"
						meta:resourcekey="DListPrefijoResource1">
					</asp:DropDownList>

					<asp:Button ID="BtnAceptarRango" runat="server" Style="z-index: 112; left: 35px;
						position: absolute; top: 266px" Text="Aceptar" OnClick="BtnAceptarRango_Click" 
						TabIndex="5" meta:resourcekey="BtnAceptarRangoResource1" />
					<asp:Button ID="BtnCancelarRango" runat="server" Style="z-index: 113; left: 128px;
						position: absolute; top: 266px" Text="Cancelar" OnClick="BtnCancelarRango_Click" 
						CausesValidation="False" TabIndex="6" 
						meta:resourcekey="BtnCancelarRangoResource1" />
					<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="TxtBInicial"
						ControlToValidate="TxtBFinal" ErrorMessage="El valor Inicial debe ser menor que el Final"
						Operator="GreaterThanEqual" Style="z-index: 115; left: 135px; position: absolute; top: 80px"
						Width="13px" meta:resourcekey="CompareValidator1Resource1">*</asp:CompareValidator>
				</asp:Panel>
				<asp:Button style="Z-INDEX: 102; LEFT: 189px; POSITION: absolute; TOP: 327px" 
					id="BtNuevoRango" onclick="BtNuevoRango_Click" 
						runat="server" Width="120px" Text="Nuevo rango" UseSubmitBehavior="False" 
					Enabled="False" meta:resourcekey="Button1Resource1"></asp:Button>
    		</asp:View>
					
			<asp:View ID="ViewRutas" runat="server">
				<asp:Label ID="LabelRutas" runat="server" Style="z-index: 101; left: 12px; position: absolute;
					top: 10px" Text="Rutas" Width="48px" meta:resourcekey="LabelRutasResource1"></asp:Label>
				<asp:DropDownList ID="DDLRutas" runat="server" Style="z-index: 100; left: 12px;
					position: absolute; top: 30px" Width="167px" AutoPostBack="True" 
					OnSelectedIndexChanged="DDLRutas_OnSelectedIndexChanged"  class="select"
					meta:resourcekey="DDLRutasResource1">
					</asp:DropDownList>

				<asp:Label style="Z-INDEX: 102; LEFT: 12px; POSITION: absolute; TOP: 56px" 
					id="LabelTipo" runat="server" Text="Tipo" meta:resourcekey="LabelTipoResource1"></asp:Label>
				<asp:DropDownList style="Z-INDEX: 105; LEFT: 12px; POSITION: absolute; TOP: 76px" 
					id="DListTipo" runat="server"  class="select"
					Width="90px" Enabled="False" AutoPostBack="True" 
					OnSelectedIndexChanged="DListTipo_SelectedIndexChange" 
					meta:resourcekey="DListTipoResource1">
					<asp:ListItem Value="D" meta:resourcekey="ListItemResource5">Directa</asp:ListItem>
					<asp:ListItem Value="A" meta:resourcekey="ListItemResource6">Alternativa</asp:ListItem>
				</asp:DropDownList>

				<asp:Label style="Z-INDEX: 108; LEFT: 14px; POSITION: absolute; TOP: 102px" 
					id="LblTroncalesAsignados" runat="server" 
					Text="Troncales Asignados" Enabled="False" 
					meta:resourcekey="LblTroncalesAsignadosResource1"></asp:Label>
				<asp:ListBox style="Z-INDEX: 111; LEFT: 14px; POSITION: absolute; TOP: 122px" 
					id="ListTroncales" runat="server" 
					Width="188px" Height="210px" SelectionMode="Multiple" Enabled="False" 
					meta:resourcekey="ListTroncalesResource1">
					</asp:ListBox>

				<asp:Label style="Z-INDEX: 109; LEFT: 259px; POSITION: absolute; TOP: 102px; width:auto" 
					id="LblTroncalesLibres" runat="server" 
					Text="Troncales Libres" meta:resourcekey="LblTroncalesLibresResource1"></asp:Label>
				<asp:ListBox style="Z-INDEX: 112; LEFT: 259px; POSITION: absolute; TOP: 122px" 
					id="ListTroncalesLibres" runat="server" 
					Width="188px" Height="210px" SelectionMode="Multiple" Enabled="False" 
					meta:resourcekey="ListTroncalesLibresResource1">
					</asp:ListBox>			

				<asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ErrorMessage="Es obligatorio seleccionar una ruta"
					style="z-index: 103; left: 210px; position: absolute; top: 33px" 
					ControlToValidate="DDLRutas" ValidationGroup="ValidatorRutas" 
					meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
				<asp:ImageButton style="Z-INDEX: 103; LEFT: 214px; POSITION: absolute; TOP: 178px; -webkit-transform: rotate(90deg); -moz-transform: rotate(90deg); -o-transform: rotate(90deg); transform: rotate(90deg); " 
                    id="IButAsignar" runat="server" ImageUrl="~/Configuracion/Images/arrow.png" Enabled="False" 
					OnClick="IButAsignar_Click" ValidationGroup="ValidatorRutas" 
					meta:resourcekey="IButAsignarResource1">
					</asp:ImageButton>
				<asp:ImageButton style="Z-INDEX: 104; LEFT: 214px; POSITION: absolute; TOP: 235px; -webkit-transform: rotate(-90deg); -moz-transform: rotate(-90deg); -o-transform: rotate(-90deg); transform: rotate(-90deg); " 
                    id="IButQuitar" runat="server" ImageUrl="~/Configuracion/Images/arrow.png" Enabled="False" 
					OnClick="IButQuitar_Click" ValidationGroup="ValidatorRutas" 
					meta:resourcekey="IButQuitarResource1">
					</asp:ImageButton>
				<asp:Button style="Z-INDEX: 106; LEFT: 357px; POSITION: absolute; TOP: 10px; width: 140px;" 
					id="BtnNuevaRuta" runat="server" Text="Nueva ruta" CausesValidation="False" Enabled="False" 
					OnClick="BtnNuevaRuta_Click" meta:resourcekey="BtnNuevaRutaResource1">
					</asp:Button>
				<asp:Button style="Z-INDEX: 107; LEFT: 357px; POSITION: absolute; TOP: 46px; width: 140px;" 
					id="BtnEliminarRuta" runat="server" Text="Eliminar ruta" Enabled="False" OnClick="BtnEliminarRuta_OnClick" 
					meta:resourcekey="BtnEliminarRutaResource1">
					</asp:Button>

					<asp:Panel ID="PanelRutas" runat="server" Height="221px" Style="z-index: 114; left: 456px;
						position: absolute; top: 106px" Width="220px" BorderStyle="Inset" Visible="False" 
					meta:resourcekey="PanelRutasResource1">
						<asp:Label style="Z-INDEX: 100; LEFT: 13px; POSITION: absolute; TOP: 13px" 
							id="LblIdentificador" runat="server" Text="Identificador" 
							meta:resourcekey="LblIdentificadorResource1"></asp:Label>
						<asp:TextBox style="Z-INDEX: 101; LEFT: 13px; POSITION: absolute; TOP: 40px" 
							id="TxtIdRuta" runat="server" MaxLength="32" Enabled="False" 
							meta:resourcekey="TxtIdRutaResource1"></asp:TextBox>
						<asp:RequiredFieldValidator style="Z-INDEX: 102; LEFT: 190px; POSITION: absolute; TOP: 42px" id="RequiredFieldIdentificador" 
							runat="server" ErrorMessage="El Identificador de la ruta es obligatorio" 
							ControlToValidate="TxtIdRuta" ValidationGroup="ValidatorRuta" 
							meta:resourcekey="RequiredFieldIdentificadorResource1">*</asp:RequiredFieldValidator>
						<asp:Button ID="BtnAceptarRuta" runat="server" Style="z-index: 103; left: 34px; position: absolute;
							top: 182px" Text="Aceptar" OnClick="BtnAceptarRuta_Click" ValidationGroup="ValidatorRuta" 
							meta:resourcekey="BtnAceptarRutaResource1" />
						<asp:Button ID="BtnCancelarRuta" runat="server" Style="z-index: 104; left: 122px;
							position: absolute; top: 182px" Text="Cancelar" OnClick="BtnCancelarRuta_Click" 
							CausesValidation="False" meta:resourcekey="BtnCancelarRutaResource1" />
						<asp:Panel ID="PanelTipoRuta" runat="server" 
							style="z-index: 106; left: 13px; position: absolute; top: 77px" BackColor="Transparent" 
							GroupingText="Tipo de Ruta" Height="85px" Width="192px" 
							meta:resourcekey="PanelTipoRutaResource1">
							<asp:RadioButton ID="RBDirecta" runat="server" Style="z-index: 100; left: 15px;
								position: absolute; top: 28px" Width="123px" GroupName="TipoRuta" Text="Directa" 
								meta:resourcekey="RBDirectaResource1" />
							<asp:RadioButton ID="RBAlternativa" runat="server" Style="z-index: 102; left: 15px;
								position: absolute; top: 59px" Checked="True" GroupName="TipoRuta" Text="Alternativa" 
								meta:resourcekey="RBAlternativaResource1" />
							<br />
							<br />
							<br />
						</asp:Panel>
				</asp:Panel>
				 <asp:TreeView ID="TreeView1" runat="server" SkinID="TreeViewCD40" 
				 style="position: absolute; z-index: 111; left: 478px; top: 119px; height: 210px; width: 180px" 
				 Width="131px" ExpandDepth="0" ontreenodepopulate="OnPopulateNode" BorderStyle="Solid" 
					meta:resourcekey="TreeView1Resource1">
				<LevelStyles>
					 <asp:TreeNodeStyle ForeColor="DarkBlue" ImageUrl="~/Images/central.png" 
						 Font-Underline="False" />
					 <asp:TreeNodeStyle ForeColor="#0066FF" Font-Underline="False" />
				</LevelStyles>
				 <Nodes>
					 <asp:TreeNode SelectAction="SelectExpand" 
						 Selected="True" Text="Rutas" Value="Rutas" meta:resourcekey="TreeNodeResource1"></asp:TreeNode>
				 </Nodes>
			 </asp:TreeView>
		</asp:View>
		</asp:MultiView>

        <asp:Label ID="LblNumTestFueraRango" runat="server" 
            Text="El número de test debe estar en el rango {0} - {1}" ForeColor="Red" 
            style="z-index: 1; left: 25px; top: 497px; position: absolute" Visible="False" 
            EnableViewState="False" meta:resourcekey="LblNumTestFueraRangoResource1"></asp:Label>

        <asp:Label ID="LError" runat="server" ForeColor="Black" Style="left: 25px; position: absolute; text-align:start;
            top: 497px; z-index: 102;" Width="500px" Height="45px" 
            meta:resourcekey="LErrorResource1"></asp:Label>

	   &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
	   <asp:Button ID="BtAceptar" runat="server" Style="z-index: 103; left: 178px; position: absolute;
	      top: 369px" Text="Aceptar" Visible="False" Width="80px" UseSubmitBehavior="true" 
			OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
	    <ajaxToolKit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender" 
            runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
        </ajaxToolKit:ConfirmButtonExtender>
	</asp:Panel>
</asp:Content>

