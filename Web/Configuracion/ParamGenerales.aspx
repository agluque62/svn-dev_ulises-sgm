<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="ParamGenerales.aspx.cs" Inherits="ParamGenerales" StylesheetTheme="TemaPaginasConfiguracion"
CodeFileBaseClass="PageBaseCD40.PageCD40" Title="Parámetros Generales" EnableEventValidation="false" Culture="auto" meta:resourcekey="PageResource1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script type="text/javascript">

        function AbreVentana(ventana) {
    		window.open(ventana);
    	}
	</script>
<script runat="server">
    protected void WarnChange(object sender, System.EventArgs e)
    {
        WarnText.Text = "* ¡Antes de cambiar el número de frecuencias o accesos telefónicos por página debe borrar las asignaciones en todos los sectores!";
    }
    
</script>

	 <asp:Label ID="Label9" runat="server" Text="PARÁMETROS GENERALES DEL SISTEMA" 
		CssClass="labelPagina" meta:resourcekey="Label9Resource1"></asp:Label>
	<asp:Label ID="LblSistema" runat="server" 
		style="z-index: 1; left: 362px; top: 15px; position: absolute" Font-Bold="True" 
		Font-Names="Tahoma" meta:resourcekey="LblSistemaResource1" ></asp:Label>
        
    <asp:LinkButton ID="BtNuevo" runat="server" OnClick="BtNuevo_Click" 
		CausesValidation="False" SkinID="MascaraBotonNuevo" Text="Nuevo" 
		style="top: 545px; right: 945px; left: 37px; position:absolute" 
		meta:resourcekey="BtNuevoResource1" />
	 <asp:LinkButton ID="BtModificar" runat="server" OnClick="Button1_Click" 
		Text="Modificar" SkinID="MascaraBotonModificarSistema"
		 CausesValidation="False" meta:resourcekey="BtModificarResource1"/>
	 <asp:Button ID="BtAceptar" runat="server" Style="z-index: 117; left: 391px; position: absolute;
		  top: 550px" Text="Aceptar" Visible="False" Width="100px" UseSubmitBehavior="true"  
		 OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
	 <ajaxToolKit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender" 
        runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
    </ajaxToolKit:ConfirmButtonExtender>
	 <asp:LinkButton ID="BtCancelar" runat="server" OnClick="BtCancelar_Click" 
		Text="Cancelar" SkinID="MascaraBotonCancelarSistema"
		 CausesValidation="False" meta:resourcekey="BtCancelarResource1" />
    <ajaxToolKit:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender" 
        runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
    </ajaxToolKit:ConfirmButtonExtender>

		 
<%--	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
--%>
		<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
		</asp:ScriptManagerProxy>	
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
  			<asp:Button ID="IBSistema" runat="server" Style="z-index: 101; left: 84px;
				position: absolute; top: 54px" SkinId="ButtonTab"
				OnClick="OnButton_Click" meta:resourcekey="IBSistemaResource1" />
			<asp:Button ID="IBHistoricos" runat="server" SkinId="ButtonTab" Visible="false"
				Style="z-index: 102; left: 188px; position: absolute; top: 54px" 
				OnClick="OnButton_Click" meta:resourcekey="IBHistoricosResource1"  />
			<asp:Button ID="IBTareas" runat="server" SkinId="ButtonTab" Visible="false"
				Style="z-index: 103; left: 293px; position: absolute; top: 54px" 
				OnClick="OnButton_Click" meta:resourcekey="IBTareasResource1" />
			<asp:Panel ID="Panel1" runat="server" Height="320px" Style="z-index: 142; left: 82px;
				position: absolute; top: 70px" Width="800px" BorderStyle="Inset" 
				meta:resourcekey="Panel1Resource1">
				<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
					<asp:View ID="View1" runat="server">
						 <asp:Label ID="Label22" runat="server" Style="z-index: 100; left: 22px; position: absolute;
							  top: 131px" Text="Tiempo en PTT: " meta:resourcekey="Label2Resource1" Visible="false"></asp:Label>    
						 <asp:TextBox ID="TxtPTT" runat="server" Style="z-index: 102; left: 177px; position: absolute;
							  top: 130px" Width="49px" meta:resourcekey="TxtPTTResource1" ReadOnly="True" Visible="false"></asp:TextBox>
						 <asp:Label ID="Label23" runat="server" Style="z-index: 103; left: 22px; position: absolute;
							  top: 25px" Text="Tiempo sin Jacks 1: " meta:resourcekey="Label3Resource1" Visible="false"></asp:Label>
						 <asp:Label ID="Label24" runat="server" Style="z-index: 104; left: 22px; position: absolute;
							  top: 78px" Text="Tiempo sin Jacks 2: " meta:resourcekey="Label4Resource1" Visible="false"></asp:Label>
						 <asp:TextBox ID="TxtSinJack1" runat="server" Style="z-index: 105; left: 177px; position: absolute;
							  top: 24px" Width="47px" meta:resourcekey="TxtSinJack1Resource1" ReadOnly="True" Visible="False"></asp:TextBox>
						 <asp:TextBox ID="TxtSinJack2" runat="server" Style="z-index: 106; left: 177px; position: absolute;
							  top: 77px" Width="47px" meta:resourcekey="TxtSinJack2Resource1" ReadOnly="True" Visible="False"></asp:TextBox>
						 <asp:Label ID="Label11" runat="server" Style="z-index: 107; left: 325px; position: absolute;
							  top: 131px" Text="Tamaño literales Enlaces Externos:" Width="221px" 
							 meta:resourcekey="Label11Resource1" Visible="False"></asp:Label>
						 <asp:Label ID="Label12" runat="server" Style="z-index: 108; left: 325px; position: absolute;
							  top: 25px" Text="Tamaño literales Acceso Directo:" Width="221px" 
							 meta:resourcekey="Label12Resource1" Visible="False"></asp:Label>
						 <asp:Label ID="Label13" runat="server" Style="z-index: 109; left: 325px; position: absolute;
							  top: 78px" Text="Tamaño literales Acceso Indirecto:" Width="221px" 
							 meta:resourcekey="Label13Resource1" Visible="False"></asp:Label>
						 <asp:Label ID="Label14" runat="server" Style="z-index: 110; left: 325px; position: absolute;
							  top: 184px" Text="Tamaño literales de Emplazamientos:" Width="222px" 
							 meta:resourcekey="Label14Resource1" Visible="False"></asp:Label>
						 <asp:TextBox ID="TxtTamLitEnlExt" runat="server" Style="z-index: 111; left: 558px; position: absolute;
							  top: 130px" Width="44px" ReadOnly="True" meta:resourcekey="TxtTamLitEnlExtResource1" Visible="False"></asp:TextBox>
						 <asp:TextBox ID="TxtTamLitDA" runat="server" Style="z-index: 112; left: 556px; position: absolute;
							  top: 24px" Width="44px" ReadOnly="True" meta:resourcekey="TxtTamLitDAResource1" Visible="False"></asp:TextBox>
						 <asp:TextBox ID="TxtTamLitEmpl" runat="server" Style="z-index: 113; left: 558px; position: absolute;
							  top: 183px" Width="44px" ReadOnly="True" meta:resourcekey="TxtTamLitEmplResource1" Visible="False"></asp:TextBox>
						 <asp:TextBox ID="TxtTamLitIA" runat="server" Style="z-index: 114; left: 556px; position: absolute;
							  top: 77px" Width="44px" ReadOnly="True" meta:resourcekey="TxtTamLitIAResource1" Visible="False"></asp:TextBox>
						 <asp:Label ID="Label15" runat="server" Style="z-index: 115; left: 22px; position: absolute;
							  top: 184px" Text="Version IP:" meta:resourcekey="Label15Resource1" Visible="False"></asp:Label>
						 &nbsp;
						 &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						 <asp:DropDownList ID="DListVersionIP" runat="server" Style="z-index: 119; left: 177px;
							  position: absolute; top: 183px" Width="55px" meta:resourcekey="DListVersionIPResource1" Visible="False" class="select">
							  <asp:ListItem Value="4" meta:resourcekey="ListItemResource1">IP v4</asp:ListItem>
							  <asp:ListItem Value="6" meta:resourcekey="ListItemResource2">IP v6</asp:ListItem>
						 </asp:DropDownList>
						 &nbsp;
						 <%--<asp:RequiredFieldValidator ID="ReqFieldJack1" runat="server" ControlToValidate="TxtSinJack1"
							  ErrorMessage="Tiempo sin Jacks 1 no puede estar vacio" InitialValue=" " SetFocusOnError="True"
							  Style="z-index: 121; left: 236px; position: absolute; top: 26px"
							  Width="13px" Display="Dynamic" meta:resourcekey="ReqFieldJack1Resource1" Visible="False">*</asp:RequiredFieldValidator>
						 <asp:CompareValidator ID="CompValidatorJack1" runat="server" ControlToValidate="TxtSinJack1"
							  ErrorMessage="Tiempo sin Jacks 1 debe contener un valor numérico" Operator="DataTypeCheck"
							  SetFocusOnError="True" Style="z-index: 122; left: 235px; position: absolute;
							  top: 27px" Type="Integer" meta:resourcekey="CompValidatorJack1Resource1" Visible="False">*</asp:CompareValidator>
						 <asp:RequiredFieldValidator ID="ReqFieldJack2" runat="server" ControlToValidate="TxtSinJack2"
							  ErrorMessage="Tiempo sin Jacks 2 no puede estar vacio" Style="z-index: 123; left: 234px; position: absolute;
							  top: 78px" SetFocusOnError="True" meta:resourcekey="ReqFieldJack2Resource1">*</asp:RequiredFieldValidator>
						 <asp:CompareValidator ID="CompValidatorJack2" runat="server" ControlToValidate="TxtSinJack2"
							  ErrorMessage="Tiempo sin Jacks 2 debe contener un valor numérico" Operator="DataTypeCheck"
							  SetFocusOnError="True" Style="z-index: 124; left: 234px; position: absolute;
							  top: 78px" Type="Integer" meta:resourcekey="CompValidatorJack2Resource1" Visible="False">*</asp:CompareValidator>
						 <asp:RequiredFieldValidator ID="RequiredFieldPTT" runat="server" ControlToValidate="TxtPTT"
							  ErrorMessage="Tiempo en PTT no puede estar vacio" Height="16px" Style="z-index: 125;
							  left: 235px; position: absolute; top: 122px" Width="12px" SetFocusOnError="True" 
							 meta:resourcekey="RequiredFieldPTTResource1" Visible="False">*</asp:RequiredFieldValidator>
						 <asp:CompareValidator ID="CompValidatorPTT" runat="server" ControlToValidate="TxtPTT"
							  ErrorMessage="Tiempo en PTT debe contener un valor numérico" Height="16px" Operator="DataTypeCheck"
							  SetFocusOnError="True" Style="z-index: 126; left: 236px; position: absolute;
							  top: 133px" Type="Integer" Width="12px" meta:resourcekey="CompValidatorPTTResource1" Visible="False">*</asp:CompareValidator>--%>
						 <%--<asp:RequiredFieldValidator ID="RequiredFieldAD" runat="server" ControlToValidate="TxtTamLitDA"
							  ErrorMessage="Tamaño literales de Acceso Directo no puede ser vacio" SetFocusOnError="True"
							  Style="z-index: 127; left: 609px; position: absolute; top: 24px" 
							 meta:resourcekey="RequiredFieldADResource1" Visible="False">*</asp:RequiredFieldValidator>
						 <asp:CompareValidator ID="CompValidatorAD" runat="server" ControlToValidate="TxtTamLitDA"
							  ErrorMessage="Tamaño literales de Acceso Directo debe contener un valor numérico"
							  Operator="DataTypeCheck" SetFocusOnError="True" Style="z-index: 128; left: 609px;
							  position: absolute; top: 25px" Type="Integer" 
							 meta:resourcekey="CompValidatorADResource1" Visible="False">*</asp:CompareValidator>
						 <asp:RequiredFieldValidator ID="RequiredFieldAI" runat="server" ControlToValidate="TxtTamLitIA"
							  ErrorMessage="Tamaño literales de Acceso Indirecto no puede ser vacio" SetFocusOnError="True"
							  Style="z-index: 129; left: 610px; position: absolute; top: 80px" 
							 meta:resourcekey="RequiredFieldAIResource1" Visible="False">*</asp:RequiredFieldValidator>
						 <asp:CompareValidator ID="CompValidatorAI" runat="server" ControlToValidate="TxtTamLitIA"
							  ErrorMessage="Tamaño literales de Acceso Indirecto debe contener un valor numérico"
							  Operator="DataTypeCheck" SetFocusOnError="True" Style="z-index: 130; left: 610px;
							  position: absolute; top: 80px" Type="Integer" 
							 meta:resourcekey="CompValidatorAIResource1" Visible="False">*</asp:CompareValidator>
						 <asp:RequiredFieldValidator ID="RequiredFieldEE" runat="server" ControlToValidate="TxtTamLitEnlExt"
							  ErrorMessage="Tamaño literales de Enlaces Externos no puede ser vacio" SetFocusOnError="True"
							  Style="z-index: 131; left: 612px; position: absolute; top: 122px" 
							 meta:resourcekey="RequiredFieldEEResource1" Visible="False">*</asp:RequiredFieldValidator>
						 <asp:CompareValidator ID="CompValidatorEE" runat="server" ControlToValidate="TxtTamLitEnlExt"
							  ErrorMessage="Tamaño literales de Enlaces Externos debe contener un valor numérico"
							  Operator="DataTypeCheck" SetFocusOnError="True" Style="z-index: 132; left: 611px;
							  position: absolute; top: 131px" Type="Integer" 
							 meta:resourcekey="CompValidatorEEResource1" Visible="False">*</asp:CompareValidator>        
						 <asp:RequiredFieldValidator ID="RequiredFieldEmp" runat="server" ControlToValidate="TxtTamLitEmpl"
							  ErrorMessage="Tamaño literales de Emplazamientos no puede ser vacio" SetFocusOnError="True"
							  Style="z-index: 133; left: 611px; position: absolute; top: 186px" 
							 meta:resourcekey="RequiredFieldEmpResource1" Visible="False">*</asp:RequiredFieldValidator>
						 <asp:CompareValidator ID="CompValidatorEmp" runat="server" ControlToValidate="TxtTamLitEmpl"
							  ErrorMessage="Tamaño literales de Emplazamientos debe contener un valor numérico"
							  Operator="DataTypeCheck" SetFocusOnError="True" Style="z-index: 134; left: 611px;
							  position: absolute; top: 186px" Type="Integer" 
							 meta:resourcekey="CompValidatorEmpResource1" Visible="False">*</asp:CompareValidator>                        --%>

				<asp:TextBox ID="TxtKAP" runat="server" Style="z-index: 201; left: 22px;
					position: absolute; top: 46px" Width="33px" ReadOnly="True" 
					meta:resourcekey="TxtKAPResource1">200</asp:TextBox>
				<asp:Label ID="LblKAP" runat="server" Style="z-index: 199; left: 22px; position: absolute;
					top: 26px" Text="Keep Alive Period (ms):" Width="180px" 
					meta:resourcekey="LblKAPResource1"></asp:Label>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="TxtKAP"
					ErrorMessage="Es necesario introducir un valor entre 20 y 1000." Style="z-index: 203;
					left: 80px; position: absolute; top: 50px" 
					meta:resourcekey="RequiredFieldValidator12Resource1">*</asp:RequiredFieldValidator>
				<asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="El valor debe estar comprendido entre 20 y 1000"
					Height="20px" MaximumValue="1000" MinimumValue="20" Style="z-index: 204; left: 220px;
					position: absolute; top: 28px" Width="12px" ControlToValidate="TxtKAP" Type="Integer" 
					meta:resourcekey="RangeValidator1Resource1">*</asp:RangeValidator>

				<asp:Label ID="LblKAM" runat="server" Height="21px" Style="z-index: 200; left: 400px;
					position: absolute; top: 26px" Text="Keep Alive Multiplier:" Width="170px" 
					meta:resourcekey="LblKAMResource1"></asp:Label>
				<asp:TextBox ID="TxtKAM" runat="server" Style="z-index: 202;
					left: 400px; position: absolute; top: 46px" Width="33px" ReadOnly="True" 
					meta:resourcekey="TxtKAMResource1">10</asp:TextBox>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="TxtKAM"
					ErrorMessage="Es necesario introducir un valor entre 2 y 50." Style="z-index: 205;
					left: 475px; position: absolute; top: 50px" Width="4px" 
					meta:resourcekey="RequiredFieldValidator13Resource1">*</asp:RequiredFieldValidator>
				<asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="TxtKAM"
					ErrorMessage="El valor debe estar comprendido entre 2 y 50" MaximumValue="50"
					MinimumValue="2" Style="z-index: 207; left: 475px; position: absolute; top: 50px"
					Type="Integer" meta:resourcekey="RangeValidator2Resource1">*</asp:RangeValidator>
				 
				 <asp:Label ID="Label21" runat="server" Style="z-index: 169; left: 22px; position: absolute;
					  top: 70px" Text="Número llamadas entrantes en cola IDA:" 
					meta:resourcekey="Label13Resource1"></asp:Label>
				 <asp:TextBox ID="TxtLlamEntIDA" runat="server" Style="z-index: 170; left: 22px; position: absolute;
					  top: 90px" Width="39px" ReadOnly="True"  meta:resourcekey="TxtLlamEntIDAResource1">3</asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TxtLlamEntIDA"
					  ErrorMessage="El campo número de llamadas entrantes en cola IDA no puede estar vacio."
					  Style="z-index: 147; left: 80px; position: absolute; top: 95px" 
					meta:resourcekey="RequiredFieldValidator6Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="TxtLlamEntIDA"
					  ErrorMessage="El campo número de llamadas entrantes en cola IDA debe ser numérico."
					  Operator="DataTypeCheck" Style="z-index: 189; left: 80px; position: absolute;
					  top: 95px" Type="Integer"  meta:resourcekey="CompareValidator2Resource1">*</asp:CompareValidator>
                 <asp:RangeValidator ID="RangeValidatorTxtLlamEntIDA" runat="server" MinimumValue="1" MaximumValue="4" 
                        ControlToValidate="TxtLlamEntIDA"
                        ErrorMessage="El campo número de llamadas entrantes en cola IDA debe estar comprendido entre 1 y 4." 
                        Style="z-index: 189; left: 80px; position: absolute;top: 95px"
					meta:resourcekey="RangeValidatorTxtLlamEntIDAResource1">*</asp:RangeValidator>
					  
				 <asp:Label ID="Label25" runat="server" Style="z-index: 171; left: 400px; position: absolute;
					  top: 70px" Text="Número llamadas IDA:" meta:resourcekey="Label2Resource1"></asp:Label>
				 <asp:TextBox ID="TxtLlamIDA" runat="server" Style="z-index: 172; left: 400px; position: absolute;
					  top: 90px" Width="39px" ReadOnly="True" meta:resourcekey="TxtLlamIDAResource1">4</asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="TxtLlamIDA"
					  ErrorMessage="El campo número de llamadas IDA no puede estar vacio." Style="z-index: 150;
					  left: 450px; position: absolute; top: 95px" 
					meta:resourcekey="RequiredFieldValidator9Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="TxtLlamIDA"
					  ErrorMessage="El campo número de llamadas IDA debe ser numérico." Operator="DataTypeCheck"
					  Style="z-index: 192; left: 450px; position: absolute; top: 95px" Type="Integer" 
					meta:resourcekey="CompareValidator4Resource1">*</asp:CompareValidator>
                 <asp:RangeValidator ID="RangeValidatorTxtLlamIDA" runat="server" MinimumValue="1" MaximumValue="4" 
                        ControlToValidate="TxtLlamIDA"
                        ErrorMessage="El campo número de llamadas IDA debe estar comprendido entre 1 y 4." 
                        Style="z-index: 189; left: 450px; position: absolute;top: 95px"
					meta:resourcekey="RangeValidatorTxtLlamIDAResource1">*</asp:RangeValidator>

				 <asp:Label ID="Label26" runat="server" Style="z-index: 173; left: 22px; position: absolute;
					  top: 114px" Text="Número de frecuencias por página:" 
					meta:resourcekey="Label12Resource1"></asp:Label>
				 <asp:TextBox ID="TxtFrecPag" runat="server" Style="z-index: 175; left: 22px; position: absolute;
					  top: 134px" Width="39px" ReadOnly="True" OnLoad="WarnChange"  AutoPostBack="true" 
                      meta:resourcekey="TxtFrecPagResource1">15</asp:TextBox>
				 <asp:label ID="WarnText" runat="server" Style="z-index: 173; left: 50px; position: absolute; 
                      top: 325px" ReadOnly="True" Visible="true" color="#f00000" meta:resourcekey="WarnResource">
					  </asp:label>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="TxtFrecPag"
					  ErrorMessage="El campo número de frecuencias por página no puede estar vacio."
					  Style="z-index: 148; left: 80px; position: absolute; top: 140px" 
					meta:resourcekey="RequiredFieldValidator7Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="TxtFrecPag"
					  ErrorMessage="El campo número de frecuencias por página debe ser numérico." Operator="DataTypeCheck"
					  Style="z-index: 190; left: 369px; position: absolute; top: 116px" Type="Integer" 
					meta:resourcekey="CompareValidator3Resource1">*</asp:CompareValidator>
				<asp:RangeValidator ID="RangeValidator3" runat="server" MinimumValue="4" 
					MaximumValue="20" Type="Integer" ControlToValidate="TxtFrecPag"
											ErrorMessage="El campo número de frecuencias por página debe estar comprendido entre 4 y 20." 
											Style="z-index: 148; left: 369px; position: absolute; top: 116px" 
					meta:resourcekey="RangeValidator3Resource1">*</asp:RangeValidator>
				 <asp:Label ID="Label27" runat="server" Style="z-index: 174; left: 400px; position: absolute;
					  top: 114px" Text="Número de páginas de frecuencias:"  
					meta:resourcekey="Label11Resource1"></asp:Label>
				 <asp:TextBox ID="TxtPagFrec" runat="server" Style="z-index: 176; left: 400px; position: absolute;
					  top: 134px" Width="22px" ReadOnly="True" meta:resourcekey="TxtPagFrecResource1">9</asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="TxtPagFrec"
					  ErrorMessage="El campo número de páginas de frecuencias no puede estar vacio."
					  Style="z-index: 151; left: 450px; position: absolute; top: 140px" 
					meta:resourcekey="RequiredFieldValidator10Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="TxtPagFrec"
					  ErrorMessage="El campo número de páginas de frecuencias debe ser numérico." Operator="DataTypeCheck"
					  Style="z-index: 193; left: 450px; position: absolute; top: 140px" Type="Integer" 
					meta:resourcekey="CompareValidator5Resource1">*</asp:CompareValidator>

				 <asp:Label ID="Label28" runat="server" Style="z-index: 177; left: 22px; position: absolute;
					  top: 158px" Text="Número de enlaces telefónicos por página:" 
					meta:resourcekey="Label4Resource1"></asp:Label>
				 <asp:TextBox ID="TxtEnlIntPag" runat="server" Style="z-index: 179; left: 22px; position: absolute;
					  top: 178px" Width="39px" ReadOnly="True" meta:resourcekey="TxtEnlIntPagResource1">15</asp:TextBox>
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TxtEnlIntPag"
					  ErrorMessage="El campo número de enlaces telefónicos por página no puede estar vacio."
					  Style="z-index: 149; left: 80px; position: absolute; top: 180px" 
					meta:resourcekey="RequiredFieldValidator8Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="TxtEnlIntPag"
					  ErrorMessage="El campo número de enlaces telefónicos por página debe ser numérico."
					  Operator="DataTypeCheck" Style="z-index: 191; left: 80px; position: absolute;
					  top: 180px" Type="Integer" meta:resourcekey="CompareValidator6Resource1">*</asp:CompareValidator>
				<asp:RangeValidator ID="RangeValidator4" runat="server" MinimumValue="8" 
					MaximumValue="24" Type="Integer" ControlToValidate="TxtEnlIntPag"
											ErrorMessage="El campo número de enlaces telefónicos por página debe estar comprendido entre 8 y 24." 
											Style="z-index: 149; left: 80px; position: absolute; top: 180px" 
					meta:resourcekey="RangeValidator4Resource1">*</asp:RangeValidator>

				 <asp:Label ID="Label29" runat="server" Style="z-index: 178; left: 400px; position: absolute;
					  top: 158px" Text="Número páginas enlaces telefónicos:" 
					meta:resourcekey="Label3Resource1"></asp:Label>
				 <asp:TextBox ID="TxtPagEnlInt" runat="server" Style="z-index: 180; left: 400px; position: absolute;
					  top: 178px" Width="22px" ReadOnly="True" meta:resourcekey="TxtPagEnlIntResource1">3</asp:TextBox>        
				 <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="TxtPagEnlInt"
					  ErrorMessage="El campo número de páginas de enlaces telefónicos no puede estar vacio."
					  Style="z-index: 152; left: 450px; position: absolute; top: 180px" 
					meta:resourcekey="RequiredFieldValidator11Resource1">*</asp:RequiredFieldValidator>
				 <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="TxtPagEnlInt"
					  ErrorMessage="El campo número de páginas de enlaces telefónicos debe ser numérico."
					  Operator="DataTypeCheck" Style="z-index: 194; left: 450px; position: absolute;
					  top: 180px" Type="Integer" meta:resourcekey="CompareValidator6Resource1">*</asp:CompareValidator>

				 <asp:Label ID="Label30" runat="server" Style="z-index: 178; left: 22px; position: absolute;
					  top: 203px" Text="Número enlaces acceso instantáneo:" 
					meta:resourcekey="Label30Resource1"></asp:Label>
				 <asp:TextBox ID="TxtNumAI" runat="server" Style="z-index: 180; left: 22px; position: absolute;
					  top: 223px" Width="22px" ReadOnly="True" meta:resourcekey="TxtNumAIResource1">18</asp:TextBox>

				<asp:Label ID="Label31" runat="server" Style="z-index: 135; left: 22px; position: absolute;
					top: 248px" Text="Puerto Multicast:" meta:resourcekey="Label31Resource1"></asp:Label>
				<asp:Label ID="Label35" runat="server" Style="z-index: 136; left: 400px; position: absolute;
					top: 248px" Text="Grupo Multicast:" meta:resourcekey="Label35Resource1"></asp:Label>
				<asp:TextBox ID="TxtGrupoMulti" runat="server" ReadOnly="True" Style="z-index: 137;
					left: 400px; position: absolute; top: 268px" Width="100px" meta:resourcekey="TxtGrupoMultiResource1"></asp:TextBox>
				<asp:TextBox ID="TxtPortMulti" runat="server" MaxLength="5" ReadOnly="True" Style="z-index: 138;
					left: 22px; position: absolute; top: 268px" Width="49px" 
					meta:resourcekey="TxtPortMultiResource1"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVTxtGrupoMulti" runat="server" ControlToValidate="TxtGrupoMulti"
                      ErrorMessage="La IP del Grupo Multicast no puede estar vacía."
                      Style="z-index: 152; left: 520px; position: absolute; top: 275px" 
                      meta:resourcekey="RFVTxtGrupoMultiResource1">*</asp:RequiredFieldValidator>
				<asp:RegularExpressionValidator ID="RegularExpressionValidator11" 
					runat="server" ControlToValidate="TxtGrupoMulti"
					ErrorMessage="La IP del Grupo Multicast no tiene el formato correcto." Style="z-index: 139;
					left: 520px; position: absolute; top: 275px" 
					ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}" 
					meta:resourcekey="RegularExpressionValidator11Resource1">*</asp:RegularExpressionValidator>
				<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="TxtPortMulti"
					ErrorMessage="El campo puerto Multicast debe ser numérico." Operator="DataTypeCheck"
					Style="z-index: 140; left: 100px; position: absolute; top: 275px"  Type="Integer" 
					meta:resourcekey="CompareValidator1Resource1">*</asp:CompareValidator>
                 <asp:RequiredFieldValidator ID="RFVTxtPortMulti" runat="server" ControlToValidate="TxtPortMulti"
                      ErrorMessage="El campo puerto Multicast no puede estar vacío."
                      Style="z-index: 152; left: 100px; position: absolute; top: 275px" 
                      meta:resourcekey="RFVTxtPortMultiResource1">*</asp:RequiredFieldValidator>

                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores"
                    Style="z-index: 137; left: 50px; position: absolute; top: 360px" 
		            Visible="true" meta:resourcekey="erroresResource1" />
					</asp:View>					
					<asp:View ID="View2" runat="server">
						<asp:Panel ID="Panel2" runat="server" Style="z-index: 100;
							left: 13px; position: absolute; top: 10px; height: 220px; width: 631px;" 
							Height="150px" BorderStyle="Inset" Enabled="False" meta:resourcekey="Panel2Resource1">
							<asp:Label ID="LblHw" runat="server" Style="z-index: 100; left: 19px; position: absolute;
								top: 28px" Text="Hardware" Font-Bold="True" meta:resourcekey="LblHwResource1"></asp:Label>
							<asp:RadioButtonList ID="RBLHardware" runat="server" Height="80px" Style="z-index: 101;
								left: 19px; position: absolute; top: 54px" Width="116px" 
								meta:resourcekey="RBLHardwareResource1">
								<asp:ListItem Value="7" Selected="True" meta:resourcekey="ListItemResource3">1 Semana</asp:ListItem>
								<asp:ListItem Value="14" meta:resourcekey="ListItemResource4">2 Semanas</asp:ListItem>
								<asp:ListItem Value="21" meta:resourcekey="ListItemResource5">3 Semanas</asp:ListItem>
								<asp:ListItem Value="30" meta:resourcekey="ListItemResource6">1 Mes</asp:ListItem>
								<asp:ListItem Value="60" meta:resourcekey="ListItemResource7">2 Meses</asp:ListItem>
								<asp:ListItem Value="90" meta:resourcekey="ListItemResource8">3 Meses</asp:ListItem>
							</asp:RadioButtonList>
							<asp:Label ID="LblTf" runat="server" Style="z-index: 102; left: 165px; position: absolute;
								top: 28px" Text="Telefonía" Font-Bold="True" meta:resourcekey="LblTfResource1"></asp:Label>
							<asp:RadioButtonList ID="RBLTelefonia" runat="server" Height="125px" Style="z-index: 108;
								left: 165px; position: absolute; top: 54px" Width="127px" BorderColor="White" 
								meta:resourcekey="RBLTelefoniaResource1">
								<asp:ListItem Value="7" Selected="True" meta:resourcekey="ListItemResource9">1 Semana</asp:ListItem>
								<asp:ListItem Value="14" meta:resourcekey="ListItemResource10">2 Semanas</asp:ListItem>
								<asp:ListItem Value="21" meta:resourcekey="ListItemResource11">3 Semanas</asp:ListItem>
								<asp:ListItem Value="30" meta:resourcekey="ListItemResource12">1 Mes</asp:ListItem>
								<asp:ListItem Value="60" meta:resourcekey="ListItemResource13">2 Meses</asp:ListItem>
								<asp:ListItem Value="90" meta:resourcekey="ListItemResource14">3 Meses</asp:ListItem>
							</asp:RadioButtonList>
							<asp:Label ID="LblRd" runat="server" Style="z-index: 104; left: 322px; position: absolute;
								top: 28px" Text="Radio" Font-Bold="True" meta:resourcekey="LblRdResource1"></asp:Label>
							<asp:RadioButtonList ID="RBLRadio" runat="server" Height="125px" Style="z-index: 105;
								left: 322px; position: absolute; top: 54px" Width="127px" 
								meta:resourcekey="RBLRadioResource1">
								<asp:ListItem Value="7" Selected="True" meta:resourcekey="ListItemResource15">1 Semana</asp:ListItem>
								<asp:ListItem Value="14" meta:resourcekey="ListItemResource16">2 Semanas</asp:ListItem>
								<asp:ListItem Value="21" meta:resourcekey="ListItemResource17">3 Semanas</asp:ListItem>
								<asp:ListItem Value="30" meta:resourcekey="ListItemResource18">1 Mes</asp:ListItem>
								<asp:ListItem Value="60" meta:resourcekey="ListItemResource19">2 Meses</asp:ListItem>
								<asp:ListItem Value="90" meta:resourcekey="ListItemResource20">3 Meses</asp:ListItem>
							</asp:RadioButtonList>
							<asp:Label ID="LblGestion" runat="server" Style="z-index: 106; left: 481px; position: absolute;
								top: 28px" Text="Gestión" Font-Bold="True" meta:resourcekey="LblGestionResource1"></asp:Label>
							<asp:RadioButtonList ID="RBLGestion" runat="server" Height="125px" Style="z-index: 107;
								left: 481px; position: absolute; top: 54px" Width="127px" 
								meta:resourcekey="RBLGestionResource1">
								<asp:ListItem Value="7" Selected="True" meta:resourcekey="ListItemResource21">1 Semana</asp:ListItem>
								<asp:ListItem Value="14" meta:resourcekey="ListItemResource22">2 Semanas</asp:ListItem>
								<asp:ListItem Value="21" meta:resourcekey="ListItemResource23">3 Semanas</asp:ListItem>
								<asp:ListItem Value="30" meta:resourcekey="ListItemResource24">1 Mes</asp:ListItem>
								<asp:ListItem Value="60" meta:resourcekey="ListItemResource25">2 Meses</asp:ListItem>
								<asp:ListItem Value="90" meta:resourcekey="ListItemResource26">3 Meses</asp:ListItem>
							</asp:RadioButtonList>
							<asp:Label ID="Label17" runat="server" ForeColor="#6699FF" 
								Text="Profundidad históricos" meta:resourcekey="Label17Resource1"></asp:Label>
							<asp:Label ID="Label19" runat="server" 
								style="z-index: 1; left: 218px; top: 2px; position: absolute; width:auto" 
								Text="Recurso para backup:" meta:resourcekey="Label19Resource1"></asp:Label>
							<asp:TextBox ID="TBRecursoHistoricos" runat="server" 
								style="z-index: 1; left: 395px; top: 1px; position: absolute; width: 215px" 
								meta:resourcekey="TBRecursoHistoricosResource1"></asp:TextBox>
						</asp:Panel>
						<asp:Panel ID="Panel3" runat="server" Style="z-index: 100;
							left: 13px; position: absolute; top: 247px; height: 203px;" Width="631px" 
							BorderStyle="Inset" Enabled="False" meta:resourcekey="Panel3Resource1">
							<asp:Label ID="Label1" runat="server" Style="z-index: 100; left: 19px; position: absolute;
								top: 34px" Text="Hardware" Font-Bold="True" meta:resourcekey="Label1Resource1"></asp:Label>
							<asp:RadioButtonList ID="RadioButtonList1" runat="server" Height="80px" Style="z-index: 101;
								left: 19px; position: absolute; top: 62px" Width="116px" 
								meta:resourcekey="RadioButtonList1Resource1">
								<asp:ListItem Value="7" Selected="True" meta:resourcekey="ListItemResource27">1 Semana</asp:ListItem>
								<asp:ListItem Value="14" meta:resourcekey="ListItemResource28">2 Semanas</asp:ListItem>
								<asp:ListItem Value="21" meta:resourcekey="ListItemResource29">3 Semanas</asp:ListItem>
								<asp:ListItem Value="30" meta:resourcekey="ListItemResource30">1 Mes</asp:ListItem>
								<asp:ListItem Value="60" meta:resourcekey="ListItemResource31">2 Meses</asp:ListItem>
								<asp:ListItem Value="90" meta:resourcekey="ListItemResource32">3 Meses</asp:ListItem>
							</asp:RadioButtonList>
							<asp:Label ID="Label8" runat="server" Style="z-index: 102; left: 165px; position: absolute;
								top: 34px" Text="Telefonía" Font-Bold="True" meta:resourcekey="Label8Resource1"></asp:Label>
							<asp:RadioButtonList ID="RadioButtonList2" runat="server" Height="125px" Style="z-index: 108;
								left: 165px; position: absolute; top: 62px" Width="127px" BorderColor="White" 
								meta:resourcekey="RadioButtonList2Resource1">
								<asp:ListItem Value="7" Selected="True" meta:resourcekey="ListItemResource33">1 Semana</asp:ListItem>
								<asp:ListItem Value="14" meta:resourcekey="ListItemResource34">2 Semanas</asp:ListItem>
								<asp:ListItem Value="21" meta:resourcekey="ListItemResource35">3 Semanas</asp:ListItem>
								<asp:ListItem Value="30" meta:resourcekey="ListItemResource36">1 Mes</asp:ListItem>
								<asp:ListItem Value="60" meta:resourcekey="ListItemResource37">2 Meses</asp:ListItem>
								<asp:ListItem Value="90" meta:resourcekey="ListItemResource38">3 Meses</asp:ListItem>
							</asp:RadioButtonList>
							<asp:Label ID="Label10" runat="server" Style="z-index: 104; left: 322px; position: absolute;
								top: 34px" Text="Radio" Font-Bold="True" meta:resourcekey="Label10Resource1"></asp:Label>
							<asp:RadioButtonList ID="RadioButtonList3" runat="server" Height="125px" Style="z-index: 105;
								left: 322px; position: absolute; top: 62px" Width="127px" 
								meta:resourcekey="RadioButtonList3Resource1">
								<asp:ListItem Value="7" Selected="True" meta:resourcekey="ListItemResource39">1 Semana</asp:ListItem>
								<asp:ListItem Value="14" meta:resourcekey="ListItemResource40">2 Semanas</asp:ListItem>
								<asp:ListItem Value="21" meta:resourcekey="ListItemResource41">3 Semanas</asp:ListItem>
								<asp:ListItem Value="30" meta:resourcekey="ListItemResource42">1 Mes</asp:ListItem>
								<asp:ListItem Value="60" meta:resourcekey="ListItemResource43">2 Meses</asp:ListItem>
								<asp:ListItem Value="90" meta:resourcekey="ListItemResource44">3 Meses</asp:ListItem>
							</asp:RadioButtonList>
							<asp:Label ID="Label16" runat="server" Style="z-index: 106; left: 481px; position: absolute;
								top: 34px" Text="Gestión" Font-Bold="True" meta:resourcekey="Label16Resource1"></asp:Label>
							<asp:RadioButtonList ID="RadioButtonList4" runat="server" Height="125px" Style="z-index: 107;
								left: 481px; position: absolute; top: 62px" Width="127px" 
								meta:resourcekey="RadioButtonList4Resource1">
								<asp:ListItem Value="7" Selected="True" meta:resourcekey="ListItemResource45">1 Semana</asp:ListItem>
								<asp:ListItem Value="14" meta:resourcekey="ListItemResource46">2 Semanas</asp:ListItem>
								<asp:ListItem Value="21" meta:resourcekey="ListItemResource47">3 Semanas</asp:ListItem>
								<asp:ListItem Value="30" meta:resourcekey="ListItemResource48">1 Mes</asp:ListItem>
								<asp:ListItem Value="60" meta:resourcekey="ListItemResource49">2 Meses</asp:ListItem>
								<asp:ListItem Value="90" meta:resourcekey="ListItemResource50">3 Meses</asp:ListItem>
							</asp:RadioButtonList>
								<asp:Label ID="Label18" runat="server" ForeColor="#6699FF" 
									Text="Profundidad indicadores" meta:resourcekey="Label18Resource1"></asp:Label>
							<asp:Label ID="Label20" runat="server" 
								style="z-index: 1; left: 218px; top: 4px; position: absolute; width: auto;" 
								Text="Recurso para backup:" meta:resourcekey="Label20Resource1"></asp:Label>
							<asp:TextBox ID="TBRecursoIndicadores" runat="server" 
								style="position: absolute; top: 1px; left: 395px; z-index: 1" width="215px" 
								meta:resourcekey="TBRecursoIndicadoresResource1"></asp:TextBox>
						</asp:Panel>
    				</asp:View>
					<asp:View ID="View3" runat="server">
						&nbsp;
						<asp:Label ID="Label2" runat="server" Style="z-index: 200; left: 48px; position: absolute;
							top: 26px; height: 19px;" Text="Nombre" meta:resourcekey="Label2Resource2"></asp:Label>
						<asp:Label ID="Label3" runat="server" Style="z-index: 201; left: 48px; position: absolute;
							top: 54px; width: 71px; right: 541px;" Text="Programa" height="19px" 
							meta:resourcekey="Label3Resource2"></asp:Label>
						<asp:Label ID="Label4" runat="server" Style="z-index: 202; left: 48px; position: absolute;
							top: 82px; height: 19px;" Text="Argumentos" meta:resourcekey="Label4Resource2"></asp:Label>
						<asp:Label ID="Label5" runat="server" Style="z-index: 203; left: 48px; position: absolute;
							top: 110px" Text="Hora" height="19px" meta:resourcekey="Label5Resource1"></asp:Label>
						<asp:Label ID="Label6" runat="server" Style="z-index: 204; left: 48px; position: absolute;
							top: 138px" Text="Periodicidad" height="19px" meta:resourcekey="Label6Resource1"></asp:Label>
						<asp:Label ID="Label7" runat="server" Style="z-index: 205; left: 48px; position: absolute;
							top: 166px" Text="Comentario" height="19px" meta:resourcekey="Label7Resource1"></asp:Label>
						<asp:TextBox ID="TBNombreTarea" runat="server" Style="z-index: 106; left: 150px; position: absolute;
							top: 25px" ReadOnly="True" meta:resourcekey="TBNombreTareaResource1"></asp:TextBox>
						&nbsp;
						<asp:TextBox ID="TBArgumentos" runat="server" Style="z-index: 107; left: 150px; position: absolute;
							top: 81px" Width="279px" ReadOnly="True" meta:resourcekey="TBArgumentosResource1"></asp:TextBox>
						<asp:TextBox ID="TBHora" runat="server" Style="z-index: 108; left: 150px; position: absolute;
							top: 109px" Width="38px" ReadOnly="True" meta:resourcekey="TBHoraResource1"></asp:TextBox>
						<asp:TextBox ID="TBComentario" runat="server" Height="34px" Style="z-index: 109; left: 150px;
							position: absolute; top: 166px" Width="279px" ReadOnly="True" 
							meta:resourcekey="TBComentarioResource1"></asp:TextBox>
						<asp:DropDownList ID="DDLPeriodicidad" runat="server" Style="z-index: 110; left: 150px;
							position: absolute; top: 137px" Width="122px" Enabled="False"  class="select"
							meta:resourcekey="DDLPeriodicidadResource1">
							<asp:ListItem Value="D" meta:resourcekey="ListItemResource51">Diaria</asp:ListItem>
							<asp:ListItem Value="S" meta:resourcekey="ListItemResource52">Semanal</asp:ListItem>
							<asp:ListItem Value="M" meta:resourcekey="ListItemResource53">Mensual</asp:ListItem>
						</asp:DropDownList>
						&nbsp;
						<asp:Button ID="BtnAnadirTarea" runat="server" Style="z-index: 111; left: 549px; position: absolute;
							top: 163px" Text="Añadir" Visible="False" Width="69px" 
							OnClick="BtnAnadirTarea_Click" meta:resourcekey="BtnAnadirTareaResource1" />
						&nbsp;
						<asp:Button ID="BtnNuevaTarea" runat="server" Height="24px" Style="z-index: 112; left: 362px;
							position: absolute; top: 24px" Text="Nueva" Width="70px" 
							OnClick="BtnNuevaTarea_Click" meta:resourcekey="BtnNuevaTareaResource1" />
						<asp:Button ID="BtnCancelTarea" runat="server" Height="24px" Style="z-index: 112; left: 640px; position: absolute;
							top: 163px" Text="Cancelar" Width="70px"  Visible="false"
							OnClick="BtnCancelTarea_Click" meta:resourcekey="BtnCancelTareaResource1" />
						&nbsp;
						<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TBHora"
							ErrorMessage="Hora no válida" Style="z-index: 113; left: 190px; position: absolute;
							top: 110px" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$" 
							Visible="False" meta:resourcekey="RegularExpressionValidator1Resource1"></asp:RegularExpressionValidator>
						<input id="File1" style="z-index: 100; left: 150px; position: absolute; top: 53px; width: 308px;"
							type="file" runat="server" lang="es-es" visible="False"/>
						<asp:GridView ID="GVTareas" runat="server" AutoGenerateColumns="False" CellPadding="4"
							ForeColor="#333333" GridLines="None" Height="195px" Style="z-index: 114; left: 451px;
							position: absolute; top: 24px" Width="384px" 
							OnSelectedIndexChanging="GVTareas_OnSelectedIndexChanged" 
							OnRowDeleting="GVTareas_OnDeleting" OnRowCommand="GVTareas_OnRowCommand" 
							meta:resourcekey="GVTareasResource1">
							<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
							<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
							<Columns>
								<asp:CommandField ShowSelectButton="True" 
									meta:resourcekey="CommandFieldResource1" />
								<asp:TemplateField HeaderText="Programada" 
									meta:resourcekey="TemplateFieldResource1">
								 <ItemTemplate>
									 <asp:CheckBox ID="CBProgramada" runat="server" AutoPostBack="True" 
										 Enabled="False" meta:resourcekey="CBProgramadaResource1" />
								 </ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>
								<asp:BoundField HeaderText="Tarea" ReadOnly="True" DataField="IdTarea" 
									meta:resourcekey="BoundFieldResource1" />
								<asp:CommandField CancelText="Eliminar" ShowCancelButton="False"
									ShowDeleteButton="True" SelectText="" meta:resourcekey="CommandFieldResource2" />
								<asp:ButtonField CommandName="BtnCommandProgramar" Text="Programar" 
									meta:resourcekey="ButtonFieldResource1" />
							</Columns>
							<EditRowStyle BackColor="#999999" />
							<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
							<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
							<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
							<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
						</asp:GridView>
						&nbsp;&nbsp;&nbsp;
						<asp:TextBox ID="TBPrograma" runat="server" ReadOnly="True" Style="z-index: 116;
							left: 150px; position: absolute; top: 53px" Width="279px" 
							meta:resourcekey="TBProgramaResource1"></asp:TextBox>
					</asp:View>
					</asp:MultiView>
				</asp:Panel>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>

