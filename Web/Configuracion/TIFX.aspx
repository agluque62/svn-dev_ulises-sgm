<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="TIFX.aspx.cs" Inherits="TIFX_Cfg"
	CodeFileBaseClass="PageBaseCD40.PageCD40" Title="Gestión de pasarelas" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <script type="text/javascript">
			function AbreVentana(ventana) {
			window.open(ventana);
		}
	</script>

 <asp:Label ID="Label6" runat="server" Text="GESTIÓN DE PASARELAS" 
		  CssClass="labelPagina" meta:resourcekey="Label6Resource1" ></asp:Label>        
	<asp:ListBox ID="ListBox1" runat="server" 
		  OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" AutoPostBack="True"
		SkinID="MascaraListaElementos" meta:resourcekey="ListBox1Resource1">
  </asp:ListBox>

    <asp:LinkButton ID="BtNuevo" runat="server" OnClick="BtNuevo_Click" 
		  CausesValidation="False" Text="Nuevo" SkinID="MascaraBotonNuevo" 
		  meta:resourcekey="BtNuevoResource1" />
    <asp:LinkButton ID="BtEliminar" runat="server" OnClick="BtEliminar_Click" 
		  CausesValidation="False" Text="Eliminar" SkinID="MascaraBotonEliminar" 
		  meta:resourcekey="BtEliminarResource1"/>
      <ajaxToolKit:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
      </ajaxToolKit:ConfirmButtonExtender>
    <asp:LinkButton ID="BtModificar" runat="server" OnClick="BtModificar_Click" 
		  CausesValidation="False" Text="Modificar" SkinID="MascaraBotonModificar" 
		  meta:resourcekey="BtModificarResource1"/>
    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 114; left: 392px; position: absolute;
        top: 480px" Text="Aceptar" Width="100px" Visible="False" UseSubmitBehavior="true" 
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
    <asp:LinkButton ID="LBImprimir" runat="server" SkinID="MascaraBotonImprimir"  Visible="false"
		  meta:resourcekey="LBImprimirResource1">Imprimir</asp:LinkButton>
    
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores"
        Style="z-index: 139; left: 230px; position: absolute; top: 550px" 
		Visible="False" meta:resourcekey="ValidationSummary1Resource1" />
    
    <asp:Panel ID="Panel2" runat="server" BorderStyle="Inset" Height="390px" Style="z-index: 99;
		left: 226px; position: absolute; top: 75px" Width="460px" 
		  meta:resourcekey="Panel2Resource1">
			<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
				<asp:View ID="ViewGenerales" runat="server">
					<asp:Label ID="Label5" runat="server" Style="z-index: 116; left: 14px; position: absolute;
						top: 10px" Text="Identificador:" meta:resourcekey="Label5Resource1"></asp:Label>
					<asp:TextBox ID="TxtIdTIFX" runat="server" Style="z-index: 117; left: 14px; position: absolute;
						top: 30px; text-transform:uppercase;" MaxLength="32" ReadOnly="True" meta:resourcekey="TxtIdTIFXResource1"></asp:TextBox>

					<asp:Label ID="Label2" runat="server" Style="z-index: 101; left: 214px; position: absolute;
						top: 65px" Text="Arranque:" Visible="false" meta:resourcekey="Label2Resource1"></asp:Label>
					<asp:DropDownList ID="DListArranque" runat="server" Style="z-index: 108; left: 214px;
						position: absolute; top: 85px" Width="103px" Visible="false" meta:resourcekey="DListArranqueResource1" class="select">
						<asp:ListItem Value="A" meta:resourcekey="ListItemResource1">Autom&#225;tico</asp:ListItem>
						<asp:ListItem Value="M" meta:resourcekey="ListItemResource2">Manual</asp:ListItem>
					</asp:DropDownList>

					<asp:Label ID="Label10" runat="server" Style="z-index: 102; left: 14px; position: absolute;
						top: 65px" Text="Modo de Sincronización:" meta:resourcekey="Label10Resource1"></asp:Label>
					<asp:DropDownList ID="DListModoSincro" runat="server" Style="z-index: 109; left: 14px;
						position: absolute; top: 85px" Width="165px" AutoPostBack="True"  class="select" Enabled="false"
						OnTextChanged="DListModoSincro_TextChanged" 
						meta:resourcekey="DListModoSincroResource1">
						<asp:ListItem Value="0" Selected="True" meta:resourcekey="ListItemResource3">Ninguna</asp:ListItem>
						<asp:ListItem Value="1" meta:resourcekey="ListItemResource4">IEE1588</asp:ListItem>
						<asp:ListItem Value="2" meta:resourcekey="ListItemResource5">NTP</asp:ListItem>
						<asp:ListItem Value="3" meta:resourcekey="ListItemResource6">Protocolo NUCLEO</asp:ListItem>
					</asp:DropDownList>

					<asp:Label ID="Label15" runat="server" Style="z-index: 129; left: 14px; position: absolute;
						top: 125px" Text="Servidor NTP" Visible="False" meta:resourcekey="Label15Resource1"></asp:Label>
					<asp:TextBox ID="TxtMaster" runat="server" ReadOnly="True" Style="z-index: 137; left: 14px;
						position: absolute; top: 145px" Visible="False" Width="165px" MaxLength="32" 
						meta:resourcekey="TxtMasterResource1"></asp:TextBox>

					<asp:Label ID="Label3" runat="server" Style="z-index: 110; left: 14px; position: absolute;
						top: 180px" Text="IP-CPU-0:" meta:resourcekey="Label3Resource1"></asp:Label>
					<asp:TextBox ID="TxtIP1" runat="server" Style="z-index: 105; left: 14px; position: absolute;
						top: 200px" Width="165px" ReadOnly="True" meta:resourcekey="TxtIP1Resource1"></asp:TextBox>

					<asp:Label ID="Label4" runat="server" Style="z-index: 107; left: 14px; position: absolute;
						top: 235px" Text="IP-CPU-1:" meta:resourcekey="Label4Resource1"></asp:Label>
					<asp:TextBox ID="TxtIP2" runat="server" Style="z-index: 106; left: 14px; position: absolute;
						top: 255px" Width="165px" ReadOnly="True" meta:resourcekey="TxtIP2Resource1"></asp:TextBox>
					
                    <asp:Label ID="Label7" runat="server" Style="z-index: 109; left: 14px; position: absolute;
						top: 290px" Text="IP de Comunicaciones:" meta:resourcekey="Label7Resource1"></asp:Label>
					<asp:TextBox ID="TxtIpVirtual" runat="server" Style="z-index: 111; left: 14px; position: absolute;
						top: 310px" Width="165px" ReadOnly="True" meta:resourcekey="TxtIpVirtualResource1"></asp:TextBox>

					<asp:Label ID="LblIp1Existente" runat="server" Visible="False"  ForeColor="Black"
						Text="La dirección IP-CPU-0 ya existe en el sistema." 
								Style="position:absolute; top: 450px; left: 50px; width: auto; height: 44px;" 
						meta:resourcekey="LblIp1ExistenteResource1"></asp:Label>
					<asp:Label ID="LblIp2Existente" runat="server" Visible="False"  ForeColor="Black"
						Text="La dirección IP-CPU-1 ya existe en el sistema."
								Style="position:absolute; top: 450px; left: 50px; width: auto; height: 44px;" 
						meta:resourcekey="LblIp2ExistenteResource1"></asp:Label>
					<asp:Label ID="LblIpVirtualExistente" runat="server" Visible="False"  ForeColor="Black"
						Text="La dirección IP de comunicaciones ya existe en el sistema."
								Style="position:absolute; top: 450px; left: 50px; width: auto; height: 44px;" 
						meta:resourcekey="LblIpVirtualExistenteResource1"></asp:Label>

                    <asp:RequiredFieldValidator ID="RequiredFieldTxtIP1" runat="server" ControlToValidate="TxtIP1"
						ErrorMessage="La dirección IP de la CPU-0 no puede estar vacía" Style="z-index: 105;
						left: 195px; position: absolute; top: 200px" 
						meta:resourcekey="RequiredFieldTxtIP1Resource1">*</asp:RequiredFieldValidator>

                    <asp:RequiredFieldValidator ID="RequiredFieldTxtIP2" runat="server" ControlToValidate="TxtIP2"
						ErrorMessage="La dirección IP de la CPU-1 no puede estar vacía" Style="z-index: 105;
						left: 195px; position: absolute; top: 255px" 
						meta:resourcekey="RequiredFieldTxtIP2Resource1">*</asp:RequiredFieldValidator>

                    <asp:RequiredFieldValidator ID="RequiredFieldTxtIpVirtual" runat="server" ControlToValidate="TxtIpVirtual"
						ErrorMessage="La dirección IP de Comunicaciones no puede estar vacía" Style="z-index: 142;
						left: 195px; position: absolute; top: 310px" 
						meta:resourcekey="RequiredFieldTxtIpVirtualResource1">*</asp:RequiredFieldValidator>

					<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtIP2"
						ErrorMessage="Formato incorrecto de la IP de la CPU-1." Style="z-index: 141;
						left: 195px; position: absolute; top: 255px" 
						ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}" 
						meta:resourcekey="RegularExpressionValidator2Resource1">*</asp:RegularExpressionValidator>
					<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtIP1"
						ErrorMessage="Formato incorrecto de la IP de la CPU-0." Style="z-index: 118;
						left: 195px; position: absolute; top: 200px" 
						ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}" 
						meta:resourcekey="RegularExpressionValidator1Resource1">*</asp:RegularExpressionValidator>
					<asp:RequiredFieldValidator ID="RequiredFieldIdTIFX" runat="server" ControlToValidate="TxtIdTIFX"
						ErrorMessage="El identificador de la pasarela no puede estar vacio." Style="z-index: 140;
						left: 195px; position: absolute; top: 30px" 
						meta:resourcekey="RequiredFieldIdTIFXResource1">*</asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="TxtIpVirtual"
						ErrorMessage="Formato incorrecto de la dirección IP de comunicaciones." Style="z-index: 141;
						left: 195px; position: absolute; top: 310px" 
						ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}" 
						meta:resourcekey="RegularExpressionValidator3Resource1">*</asp:RegularExpressionValidator>

                    <asp:RegularExpressionValidator ID="RegExpValidatorIPServidorNTP" runat="server" ControlToValidate="TxtMaster"
						ErrorMessage="Formato incorrecto de la dirección IP del servidor NTP." Style="z-index: 141;
						left: 195px; position: absolute; top: 145px" 
						ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}" 
						meta:resourcekey="RegExpValidatorIPServidorNTPRes1">*</asp:RegularExpressionValidator>

                    <asp:Label ID="Label1" runat="server" Style="z-index: 125; left: 250px; position: absolute;
						top: 125px" Text="Tiempo Supervisión:" meta:resourcekey="Label1Resource1"></asp:Label>
					<asp:TextBox ID="TxtTimeSupervision" runat="server" ReadOnly="True" Style="z-index: 126;
						left: 250px; position: absolute; top: 145px" Width="57px" Text="90" 
						meta:resourcekey="TxtTimeSupervisionResource1"></asp:TextBox>

					<asp:Label ID="Label14" runat="server" Style="z-index: 128; left: 250px; position: absolute;
						top: 180px" Text="Puerto SIP Local:" meta:resourcekey="Label14Resource1"></asp:Label>
					<asp:TextBox ID="TxtPortSIPLocal" runat="server" ReadOnly="True" Style="z-index: 127;
						left: 250px; position: absolute; top: 200px" Width="57px" Text="5060" 
						meta:resourcekey="TxtPortSIPLocalResource1"></asp:TextBox>

                    <asp:Label ID="LBSupervisionLanGW" runat="server" Style="z-index: 128; left: 250px; position: absolute;
						top: 235px" Text="Supervisi&oacute;n LAN GW:" meta:resourcekey="LBSupervisionLanGWResource"></asp:Label>
                    <asp:DropDownList ID="DDLSupervisionLanGW" OnSelectedIndexChanged="DDLSupervisionLanGW_SelectedIndexChanged" 
                        AutoPostBack="True" runat="server" Style="z-index: 105; left: 250px; position: absolute; top: 255px" 
                        Visible="true" Width="57px" AppendDataBoundItems="True" class="select" Enabled="False" 
                        meta:resourcekey="DDLSupervisionLanGWResource">
                            <asp:ListItem Value="1" meta:resourcekey="DDLYesItemResource">Si</asp:ListItem>
                            <asp:ListItem Value="0" meta:resourcekey="DDLNoItemResource">No</asp:ListItem>
                    </asp:DropDownList>

                    <asp:Label ID="LBTmMaxSupervLanGW" runat="server" Style="z-index: 128; left: 250px; position: absolute;
						top: 275px" meta:resourcekey="LBTmMaxSupervLanGWResource"></asp:Label>
					<asp:TextBox ID="TxtTmMaxSupervLanGW" runat="server" onblur="checkRangeTifx(this,1,5);" ReadOnly="False" Style="z-index: 127;
						left: 250px; position: absolute; top: 310px" Width="57px" Text="1" 
						meta:resourcekey="TxtTmMaxSupervLanGWResource"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV_TxtTimeSupervision" runat="server" ControlToValidate="TxtTimeSupervision"
                        ErrorMessage="El tiempo de supervision de las sesiones SIP no puede estar vacío." Style="z-index: 140;
                        left: 350px; position: absolute; top: 260px" 
                        meta:resourcekey="RFV_TxtTimeSupervisionResource1">*</asp:RequiredFieldValidator>  
            
                    <asp:RangeValidator ID="RV_TxtTimeSupervision" runat="server" ControlToValidate="TxtTimeSupervision"
                        ErrorMessage="El tiempo de supervision de las sesiones SIP deber ser numérico y debe tomar un valor en el intervalo [90,1800] segundos."
                        MinimumValue="90" MaximumValue="1800" Style="z-index: 131; left: 350px; position: absolute;
                        top: 260px" Type="Integer" meta:resourcekey="RV_TxtTimeSupervisionResource1">*</asp:RangeValidator>
				</asp:View>
				<asp:View ID="ViewSIP" runat="server">
                    <table runat="server" id="TblRecorders" style="position: absolute; top: 50px; left: 17px" visible="True">
                        <tr>
                            <td>
                                <asp:Label ID="Label43" runat="server" Style="z-index: 159"
                                    Text="Grabador 1:" Visible="true" meta:resourcekey="Label43Resource1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="DDLRecorder1" runat="server" Visible="true" Style="z-index: 154;"
                                    Width="200px"  Height="22px" AppendDataBoundItems="True" class="select" Enabled="false">
                                </asp:DropDownList>
                                 <br />
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
                                    Text="Grabador 2:" Visible="true" meta:resourcekey="Label41Resource1"></asp:Label>
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

                    <asp:RangeValidator ID="RangeValidatorSIPLocal" runat="server" ControlToValidate="TxtPortSIPLocal"
						ErrorMessage="El campo puerto local SIP deber ser numérico y mayor de 1024."
						MaximumValue="65535" MinimumValue="1025" Style="z-index: 131; left: 264px; position: absolute;
						top: 181px" Type="Integer" meta:resourcekey="RangeValidatorSIPLocalResource1">*</asp:RangeValidator>
					<asp:RequiredFieldValidator ID="RequiredFieldSIP" runat="server" ControlToValidate="TxtPortSIPLocal"
						ErrorMessage="El puerto local para SIP no puede estar vacio." Style="z-index: 143;
						left: 253px; position: absolute; top: 45px" 
						meta:resourcekey="RequiredFieldSIPResource1">*</asp:RequiredFieldValidator>
					<asp:RangeValidator ID="RangeValidatorTimeSupervision" runat="server" ControlToValidate="TxtTimeSupervision"
						ErrorMessage="El campo Tiempo Supervisión deber ser numérico." MinimumValue="0"
						Style="z-index: 135; left: 80px; position: absolute; top: 32px" Type="Integer" 
						MaximumValue="100000" meta:resourcekey="RangeValidatorTimeSupervisionResource1">*</asp:RangeValidator>
					<asp:RequiredFieldValidator ID="RequiredFieldTimeSupervision" runat="server" ControlToValidate="TxtTimeSupervision"
						ErrorMessage="El campo Tiempo Supervisión no puede estar vacio." Style="z-index: 136;
						left: 80px; position: absolute; top: 32px" 
						meta:resourcekey="RequiredFieldTimeSupervisionResource1">*</asp:RequiredFieldValidator>

				</asp:View>
				<asp:View ID="ViewSNMP" runat="server">
					 <asp:Label ID="Label11" runat="server" Style="z-index: 119; left: 14px; position: absolute;
						  top: 12px; " Text="Puerto Local SNMP:" meta:resourcekey="Label11Resource1"></asp:Label>
					<asp:TextBox ID="TxtSNMPPortLocal" runat="server" Style="z-index: 122; 
							top: 32px; left: 14px; position: absolute" Width="57px" ReadOnly="True" Text="161" 
						 meta:resourcekey="TxtSNMPPortLocalResource1"></asp:TextBox>

					<asp:Label ID="Label12" runat="server" Style="z-index: 120; left: 14px; position: absolute;
						top: 122px" Text="Puerto Remoto SNMP:" Visible="False" meta:resourcekey="Label12Resource1"></asp:Label>
					<asp:TextBox ID="TxtSNMPPortRemoto" runat="server" Style="z-index: 123; 
						top: 142px; left: 14px; position: absolute; " Width="76px" ReadOnly="True" Text="161"  Visible="False"
						 meta:resourcekey="TxtSNMPPortRemotoResource1"></asp:TextBox>
					
					 <asp:Label ID="Label13" runat="server" Style="z-index: 121; left: 14px; position: absolute;
							top: 67px" Text="Puerto Traps SNMP:" meta:resourcekey="Label13Resource1"></asp:Label>
					 <asp:TextBox ID="TxtSNMPTraps" runat="server" Style="z-index: 124; left: 14px; position: absolute;
							top: 87px" Width="57px" ReadOnly="True" Text="162" 
						 meta:resourcekey="TxtSNMPTrapsResource1"></asp:TextBox>

					<asp:RangeValidator ID="RangeValidatorSNMPRemoto" runat="server" ControlToValidate="TxtSNMPPortRemoto"
						ErrorMessage="El campo puerto remoto SNMP deber ser numérico y mayor de 1024."
						MaximumValue="65535" MinimumValue="100" Style="z-index: 132; left: 292px; position: absolute;
						top: 46px" Type="Integer" meta:resourcekey="RangeValidatorSNMPRemotoResource1">*</asp:RangeValidator>
					<asp:RequiredFieldValidator ID="RequiredFieldSNMPRemoto" runat="server" ControlToValidate="TxtSNMPPortRemoto"
						ErrorMessage="El puerto remoto para SNMP no puede estar vacio." Style="z-index: 144;
						left: 291px; position: absolute; top: 46px" 
						 meta:resourcekey="RequiredFieldSNMPRemotoResource1">*</asp:RequiredFieldValidator>
					<asp:RangeValidator ID="RangeValidatorLocalSNMP" runat="server" ControlToValidate="TxtSNMPPortLocal"
						ErrorMessage="El campo puerto local SNMP deber ser numérico  y mayor de 1024."
						MaximumValue="65535" MinimumValue="100" Style="z-index: 130; left: 80px; position: absolute;
						top: 32px" Type="Integer" meta:resourcekey="RangeValidatorLocalSNMPResource1">*</asp:RangeValidator>
					<asp:RequiredFieldValidator ID="RequiredFieldPortLocalSNMP" runat="server" ControlToValidate="TxtSNMPPortLocal"
						ErrorMessage="El puerto local para SNMP no puede estar vacio." Style="z-index: 142;
						left: 80px; position: absolute; top: 32px" 
						 meta:resourcekey="RequiredFieldPortLocalSNMPResource1">*</asp:RequiredFieldValidator>
					 <asp:RangeValidator ID="RangeValidatorTraps" runat="server" ControlToValidate="TxtSNMPTraps"
						  ErrorMessage="El campo Traps SNMP deber ser numérico." Style="z-index: 133; left: 80px;
							position: absolute; top: 87px" Type="Integer" MinimumValue="100" MaximumValue="65535" 
						 meta:resourcekey="RangeValidatorTrapsResource1">*</asp:RangeValidator>
					 <asp:RequiredFieldValidator ID="RequiredFieldTraps" runat="server" ControlToValidate="TxtSNMPTraps"
							ErrorMessage="El campo Traps SNMP no puede estar vacio." Style="z-index: 134;
							left: 80px; position: absolute; top: 87px" 
						 meta:resourcekey="RequiredFieldTrapsResource1">*</asp:RequiredFieldValidator>
				</asp:View>
				<asp:View ID="ViewRecursos" runat="server">
				 <asp:Table ID="TTifx" runat="server" Height="112px" Style="left: 63px; position: absolute;
				  top: 16px; z-index: 151;" Width="400px" BorderStyle="Double" GridLines="Both" 
						BorderColor="Black" meta:resourcekey="TTifxResource1">
				  <asp:TableRow ID="TableRow1" runat="server" BorderColor="Black" BorderWidth="3px" 
						 BorderStyle="Solid" meta:resourcekey="TableRow1Resource1" >
						<asp:TableCell ID="CellHead1" runat="server" BackColor="#E00034" 
							HorizontalAlign="Center" Font-Bold="True" BorderColor="Black"  ForeColor="White"
							meta:resourcekey="CellHead1Resource1">Slot 0</asp:TableCell>
						<asp:TableCell ID="CellHead2" runat="server" BackColor="#E00034" 
							HorizontalAlign="Center" Font-Bold="True" BorderColor="Black"  ForeColor="White"
							meta:resourcekey="CellHead2Resource1">Slot 1</asp:TableCell>
						<asp:TableCell ID="CellHead3" runat="server" BackColor="#E00034" 
							HorizontalAlign="Center" Font-Bold="True" BorderColor="Black" ForeColor="White" 
							meta:resourcekey="CellHead3Resource1">Slot 2</asp:TableCell>
						<asp:TableCell ID="CellHead4" runat="server" BackColor="#E00034"  ForeColor="White"
							HorizontalAlign="Center" Font-Bold="True" BorderColor="Black" 
							meta:resourcekey="CellHead4Resource1">Slot 3</asp:TableCell>
				  </asp:TableRow>
				  <asp:TableRow ID="TableRow2" runat="server" >
						<asp:TableCell ID="TableCell1" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox11" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							ForeColor="White" ></asp:TextBox>
<asp:CheckBox ID="CheckBox11" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell2" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox12" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox12" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell3" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox13" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox13" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell4" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox14" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox14" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
				  </asp:TableRow>
				  <asp:TableRow ID="TableRow3" runat="server" >
						<asp:TableCell ID="TableCell5" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox21" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox21" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell6" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox22" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox22" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell7" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox23" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox23" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell8" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox24" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox24" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
				  </asp:TableRow>
				  <asp:TableRow ID="TableRow4" runat="server" >
						<asp:TableCell ID="TableCell9" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox31" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox31" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell10" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox32" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox32" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell11" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox33" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox33" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell12" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox34" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox34" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
				  </asp:TableRow>
				  <asp:TableRow ID="TableRow5" runat="server" >
						<asp:TableCell ID="TableCell13" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox41" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox41" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell14" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox42" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox42" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell15" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox43" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox43" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
						<asp:TableCell ID="TableCell16" runat="server" Width="100px" 
							HorizontalAlign="Center" ><asp:TextBox 
							ID="TextBox44" runat="server" Visible="False" ReadOnly="True" Width="100px" 
							></asp:TextBox>
<asp:CheckBox ID="CheckBox44" runat="server" AutoPostBack="True" Enabled="False" 
							/>
</asp:TableCell>
				  </asp:TableRow>
			 </asp:Table>
				</asp:View>

			
			</asp:MultiView>
		
		</asp:Panel>

    <asp:Label ID="LError" runat="server" ForeColor="Red" Style="left: 215px; position: absolute;
        top: 390px; z-index: 103;" Width="643px" Height="24px" Visible="false"
		  meta:resourcekey="LErrorResource1"></asp:Label>

	<asp:Button ID="IBPropiedadesGenerales" runat="server" Style="z-index: 146; left: 230px;
		position: absolute; top: 58px" SkinID="ButtonTab"
		  ImageUrl="~/Configuracion/Images/MenuTifxPropGeneralesSelected.JPG" 
		  OnClick="OnButton_Click" 
		  meta:resourcekey="IBPropiedadesGeneralesResource1" />
	<asp:Button ID="IBProtocoloSIP" runat="server" ImageUrl="~/Configuracion/Images/MenuTifxProtocoloSIPUnSelected.JPG"
		Style="z-index: 147; left: 338px; position: absolute; top: 58px"  SkinID="ButtonTab"
		  OnClick="OnButton_Click" meta:resourcekey="IBProtocoloSIPResource1" />
	<asp:Button ID="IBProtocoloSNMP" runat="server" ImageUrl="~/Configuracion/Images/MenuTifxProtocoloSNMPUnSelected.JPG"
		Style="z-index: 148; left: 446px; position: absolute; top: 58px"  SkinID="ButtonTab"
		  OnClick="OnButton_Click" meta:resourcekey="IBProtocoloSNMPResource1" />
	<asp:Button ID="IBRecursos" runat="server" ImageUrl="~/Configuracion/Images/MenuTifxRecursosUnSelected.JPG"
		Style="z-index: 149; left: 556px; position: absolute; top: 58px"  SkinID="ButtonTab"
		  OnClick="OnButton_Click" meta:resourcekey="IBRecursosResource1" />

</asp:Content>


