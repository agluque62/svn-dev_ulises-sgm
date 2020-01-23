<%@ Page Language="C#" CodeFile="TFTRadio.aspx.cs" MasterPageFile="~/CD40.master" Inherits="TFTRadio"
	CodeFileBaseClass="PageBaseCD40.PageCD40" Title="Panel Radio" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--     <script type="text/javascript">
        if (window.name != "<%=GetWindowName()%>") {
            window.name = "invalidAccess";
            window.open("../BloqueoAplicacion.aspx", "_self");
        }
    </script>
--%>   
	<asp:Label ID="Label6" runat="server" Text="RADIO" CssClass="labelPagina" 
		meta:resourcekey="Label6Resource1"/>
	<asp:ImageButton ID="IButPagAbajo" runat="server"  Style="z-index: 100; -webkit-transform: rotate(90deg); -moz-transform: rotate(90deg); -o-transform: rotate(90deg); transform: rotate(90deg);
    left: 165px; position: absolute; top: 73px" CausesValidation="False" 
		ImageUrl="~/Configuracion/Images/arrow.png" OnClick="IButPagAbajo_Click" 
		meta:resourcekey="IButPagAbajoResource1" />
	<asp:ImageButton ID="IButPagArriba" runat="server" Style="z-index: 101; left: 321px; -webkit-transform: rotate(-90deg); -moz-transform: rotate(-90deg); -o-transform: rotate(-90deg); transform: rotate(-90deg);
    position: absolute; top: 73px" CausesValidation="False" 
		ImageUrl="~/Configuracion/Images/arrow.png" OnClick="IButPagArriba_Click" 
		meta:resourcekey="IButPagArribaResource1" />

	<asp:Label ID="LabelPag" runat="server" Font-Bold="True" Style="z-index: 102; left: 222px;
    position: absolute; top: 79px" Text="PAGINA " 
		meta:resourcekey="LabelPagResource1"></asp:Label>

    <asp:Table ID="TEnlacesRadio" runat="server" Style="left: 43px; position: absolute; top: 127px; z-index: 104;"
        BackColor="Transparent" BorderStyle="Solid" BorderColor="#eeb44f"
        meta:resourcekey="TEnlacesRadioResource1">
        <asp:TableRow ID="TableRow1" runat="server" BackColor="Transparent">
            <asp:TableCell ID="TableCell1" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div1" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox runat="server" ID="TextBox1" ReadOnly="True"
                        SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div2" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button1" runat="server" SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell2" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div3" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox2" runat="server" ReadOnly="True"
                        SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div4" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button2" runat="server" SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell3" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div5" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox3" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div6" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button3" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell4" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div7" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox4" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div9" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button4" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell5" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div8" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox5" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div11" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button5" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow2" runat="server" BackColor="Transparent">
            <asp:TableCell ID="TableCell6" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div10" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox6" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div13" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button6" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell7" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div12" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox7" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div15" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button7" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell8" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div14" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox8" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div17" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button8" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell9" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div16" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox9" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div19" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button9" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell10" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div18" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox10" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div21" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button10" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow3" runat="server" BackColor="Transparent">
            <asp:TableCell ID="TableCell11" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div20" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox11" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div23" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button11" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell12" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div22" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox12" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div25" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button12" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell13" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div24" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox13" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div27" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button13" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell14" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div26" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox14" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div29" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button14" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
            <asp:TableCell ID="TableCell15" runat="server" Visible="false" Width="103px"
                BorderStyle="Solid" BorderColor="#eeb44f" BorderWidth="1px">
                <div id="Div28" runat="server" style="height: 30px; width: 103px">
                    <asp:TextBox ID="TextBox15" runat="server" ReadOnly="True" Rows="3"
                        MaxLength="32" SkinID="LiteralTeclaPanelRadio"></asp:TextBox>
                </div>
                <div id="Div31" runat="server" style="height: 50px; width: 103px">
                    <asp:Button ID="Button15" runat="server"
                        SkinId="BotonPanelRadio" Text="Tx Rx"
                        OnClick="CeldaEnlaceRadio_OnClick" CausesValidation="false" />
                </div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:ListBox ID="LBoxDestinos" runat="server" Height="335px" Style="z-index: 105; left: 620px;
        position: absolute; top: 128px" Width="195px" Rows="100" 
		meta:resourcekey="LBoxDestinosResource1"></asp:ListBox>
   <asp:Label ID="Label1" runat="server" Style="z-index: 106; left: 620px; position: absolute;
        top: 100px" Text="Destinos disponibles" 
		meta:resourcekey="Label1Resource1"></asp:Label>
   <asp:LinkButton ID="BtLC" runat="server" Style="z-index: 108; left: 317px; position: absolute;
        top: 415px; height: 19px; width: auto;" Text="Panel Linea Caliente" SkinId="LinkButtonCabecera"
		OnClick="BtLC_Click" meta:resourcekey="BtLCResource1" />
   <asp:LinkButton ID="BtTelefonia" runat="server" Style="z-index: 107; left: 102px; position: absolute;
        top: 415px; height: 19px; width: auto;" Text="Panel Telefonía" 
		SkinId="LinkButtonCabecera" OnClick="BtTelefonia_Click" 
		meta:resourcekey="BtTelefoniaResource1" />

    <asp:LinkButton ID="BtSector" runat="server" Style="z-index: 103; left: 180px; position: absolute;
        top: 490px; width: 180px; height: 35px; background: url('../Configuracion/Images/arrowback.gif') no-repeat left top; 
        padding-left:18px;margin-left: 10px;padding-right:10px;" SkinId="LinkButtonCabecera" 
        Visible="True" 
		OnClick="BtSector_Click" CausesValidation="False"  
		  meta:resourcekey="BtVolverSectorRes1" >
        <asp:Label ID="EnlaceVolverSector" runat="server" Style="display:table;margin:10px auto; vertical-align: middle;"
                Text=" Volver al Sector" Visible="True" meta:resourcekey="EnlaceVolverSectorResource1"></asp:Label>
     </asp:LinkButton>

   <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Groove"
        BorderWidth="1px" Height="126px" Style="z-index: 109; left: 211px; position: absolute;
        top: 209px" Visible="False" Width="93px" 
		meta:resourcekey="Panel1Resource1">
        <asp:Button ID="BtLiberar" runat="server" Style="z-index: 100; left: 7px; position: absolute;
            top: 37px" Text="Liberar" Width="80px" OnClick="BtLiberar_Click" 
			  meta:resourcekey="BtLiberarResource1" />
        <asp:Button ID="BModificar" runat="server" Style="z-index: 104; left: 7px; position: absolute;
            top: 65px" Text="Modificar" Width="80px" OnClick="BtModificar_Click" 
			  meta:resourcekey="BModificarResource1" />
        <asp:Button ID="BtCancelar" runat="server" Style="z-index: 101; left: 7px; position: absolute;
            top: 93px" Text="Cancelar" Width="80px" OnClick="BtCancelar_Click" 
			  meta:resourcekey="BtCancelarResource1" />
        <asp:Button ID="BtAsignar" runat="server" Style="z-index: 103; left: 7px; position: absolute;
            top: 9px" Text="Asignar" Width="80px" OnClick="BtAsignar_Click" 
			  meta:resourcekey="BtAsignarResource1" />
	</asp:Panel>
    
   <asp:Panel ID="Panel2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Groove"
        BorderWidth="1px" Height="492px" Style="z-index: 111; left: 92px; position: absolute;
        top: 60px" Visible="False" Width="623px" 
		meta:resourcekey="Panel2Resource1">
        
       <asp:Label ID="Label2" runat="server" Style="z-index: 102; left: 250px; position: absolute; top: 13px"
           Text="Literal:" meta:resourcekey="Label2Resource1"></asp:Label>
       <asp:TextBox ID="TBoxLiteral" runat="server" MaxLength="10" Style="z-index: 101; left: 250px; position: absolute; top: 33px"
           Width="144px"  meta:resourcekey="TBoxLiteralResource1"></asp:TextBox>
               
       <asp:RadioButtonList ID="RBLSpreadLiteral" runat="server" Style="z-index: 101;
			left: 250px; position: relative; top: 65px; font-size: 6px;" 
			meta:resourcekey="RBLSpreadLiteralResource">
			<asp:ListItem Value="0" Selected="True" meta:resourcekey="RBSpreadResource"></asp:ListItem>
			<asp:ListItem Value="1" meta:resourcekey="RBSingleResource"></asp:ListItem>
		</asp:RadioButtonList>
      
       <asp:Label ID="LbDestino" runat="server" Style="z-index: 102; left: 20px; position: absolute; top: 13px"
           Text="Destino:" meta:resourcekey="LbDestinoResource1"></asp:Label>
       <asp:TextBox ID="TBoxDestino" runat="server" MaxLength="32" Style="z-index: 101; left: 20px; position: absolute; top: 33px;"
           Width="144px"  meta:resourcekey="TBoxDestinoResource1" enabled="false" ></asp:TextBox>

       <asp:Label ID="Label4" runat="server" Style="z-index: 105; left: 20px; position: absolute;
            top: 73px" Text="Tipo PTT:" meta:resourcekey="Label4Resource1"></asp:Label>
        <asp:DropDownList ID="DListPrioridadSIP" runat="server" Style="z-index: 103; left: 20px;
            position: absolute; top: 93px" Width="107px"  class="select"
			  meta:resourcekey="DListPrioridadSIPResource1">
				<asp:ListItem Value="1" meta:resourcekey="ListItemResource2">Normal</asp:ListItem>
				<asp:ListItem Value="3" meta:resourcekey="ListItemResource3">Prioritario</asp:ListItem>
				<asp:ListItem Value="4" meta:resourcekey="ListItemResource4">Emergencia</asp:ListItem>
        </asp:DropDownList>

		<asp:Label ID="Label5" runat="server" Style="z-index: 106; left: 20px; position: absolute;
            top: 133px" Text="Prioridad de descarte en Sectorización:" meta:resourcekey="Label5Resource1"></asp:Label>&nbsp;&nbsp; 
		  <asp:DropDownList ID="DListPrioridadTecla" runat="server" Style="z-index: 104; left: 20px;
            position: absolute; top: 153px" Width="190px"  class="select"
			  meta:resourcekey="DListPrioridadTeclaResource1">
            <asp:ListItem Value="1" meta:resourcekey="ListItemResource5">No descartable</asp:ListItem>
            <asp:ListItem Value="2" meta:resourcekey="ListItemResource6">Alta</asp:ListItem>
            <asp:ListItem Value="3" meta:resourcekey="ListItemResource7">Media</asp:ListItem>
            <asp:ListItem Value="4" meta:resourcekey="ListItemResource8">Baja</asp:ListItem>
		  </asp:DropDownList>

		  <asp:Label ID="Label3" runat="server" Style="z-index: 109; left: 20px; position: absolute;
            top: 193px" Text="Modo Operación:" Visible="false" meta:resourcekey="Label3Resource1"></asp:Label>
		  <asp:DropDownList ID="DListModoOpe" runat="server" Style="z-index: 110; left: 20px;
            position: absolute; top: 213px" Visible="false" meta:resourcekey="DListModoOpeResource1" class="select">
                <asp:ListItem Value="R" meta:resourcekey="ListItemResource9">Reposo</asp:ListItem>
				<asp:ListItem Value="M" meta:resourcekey="ListItemResource10">Monitor</asp:ListItem>
				<asp:ListItem Value="T" meta:resourcekey="ListItemResource11">Trafico</asp:ListItem>
				<asp:ListItem Value="A" meta:resourcekey="ListItemResource12">CrossCoupled_A</asp:ListItem>
				<asp:ListItem Value="B" meta:resourcekey="ListItemResource13">CrossCoupled_B</asp:ListItem>
				<asp:ListItem Value="C" meta:resourcekey="ListItemResource14">CrossCoupled_C</asp:ListItem>
				<asp:ListItem Value="D" meta:resourcekey="ListItemResource15">CrossCoupled_D</asp:ListItem>
		  </asp:DropDownList>

		  <asp:Label ID="Label7" runat="server" Style="z-index: 108; left: 20px; position: absolute;
            top: 253px" Text="Cascos:" Visible="false" meta:resourcekey="Label7Resource1"></asp:Label>
		  <asp:DropDownList ID="DListCascos" runat="server" Style="z-index: 111; left: 20px;
            position: absolute; top: 273px" Width="124px"  class="select" Visible="false"
			  meta:resourcekey="DListCascosResource1">
            <asp:ListItem Value="I" meta:resourcekey="ListItemResource16">No Posible</asp:ListItem>
				<asp:ListItem Value="A" meta:resourcekey="ListItemResource17">Posible</asp:ListItem>
				<asp:ListItem Value="S" meta:resourcekey="ListItemResource18">Seleccionado</asp:ListItem>
		  </asp:DropDownList>

		  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TBoxLiteral"
            ErrorMessage="*" 
			  Style="z-index: 107; left: 400px; position: absolute; top: 36px" 
			  meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>

        <asp:CheckBox ID="CBSupervisionPortadora" runat="server" 
            meta:resourcekey="CBSupervisarPortadoraResource1" 
            Text="Supervisión portadora:" Style="z-index: 125;
            left: 20px; position: absolute; top: 303px" TextAlign="Left"/>

       <asp:GridView ID="GViewEstado" runat="server" Style="z-index: 111; left: 306px; position: absolute; top: 24px"
           AutoGenerateColumns="False" ForeColor="#333333" BackColor="#DEBA84" 
           BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="4"
           CellSpacing="3" Width="248px" AllowPaging="False"
           OnPageIndexChanging="OnGVEstado_Changing" PageSize="10" EmptyDataText="No data in the data source."
           meta:resourcekey="GViewEstadoResource1">
           <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
           <HeaderStyle Height="25px" />
           <RowStyle Height="20px" />
           <AlternatingRowStyle BackColor="White" ForeColor="#284775" Height="20px"/>
           <Columns>
               <asp:TemplateField HeaderText="Seleccionado"
                   meta:resourcekey="TemplateFieldResource1">
                   <ItemTemplate>
                        <asp:CheckBox ID="DListEstados" runat="server" Checked="false" />
<%--                       <asp:DropDownList ID="DListEstados" runat="server" Enabled="false"
                           meta:resourcekey="DListEstadosResource1" Width="100px" class="select">
                           <asp:ListItem meta:resourcekey="ListItemResource19" Value="A">Activo</asp:ListItem>
                           <asp:ListItem meta:resourcekey="ListItemResource20" Value="S">Seleccionado</asp:ListItem>
                           <asp:ListItem meta:resourcekey="ListItemResource21" Value="I">Inactivo</asp:ListItem>
                       </asp:DropDownList>--%>
                   </ItemTemplate>
                   <ItemStyle VerticalAlign="Top" Width="40%" />
               </asp:TemplateField>
               <asp:BoundField DataField="IdEmplazamiento" HeaderText="Emplazamiento"
                   meta:resourcekey="BoundFieldResource2">
                   <ItemStyle HorizontalAlign="Center" Width="30%" />
               </asp:BoundField>
               <asp:BoundField DataField="IdRecurso" HeaderText="Recurso"
                   meta:resourcekey="BoundFieldResource1">
                   <ItemStyle HorizontalAlign="Center" Width="30%" />
               </asp:BoundField>
           </Columns>
           <EditRowStyle BackColor="#999999" />
           <HeaderStyle BackColor="#e13423" Font-Bold="True" ForeColor="White" Width="100px" />
           <PagerStyle BackColor="#8C4510" HorizontalAlign="Center" />
           <RowStyle BackColor="#FFF7E7" ForeColor="#333333" />
           <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
       </asp:GridView>

       <asp:Button ID="BtAceptar" runat="server" Style="z-index: 100; left: 233px; position: absolute; top: 440px"
           Text="Aceptar" Width="80px" OnClick="BtAceptar_Click"
           meta:resourcekey="BtAceptarResource1" UseSubmitBehavior="true" />
       <asp:Button ID="BCancelar"
           runat="server" Style="z-index: 115; left: 336px; position: absolute; top: 440px"
           Text="Cancelar" Width="80px" OnClick="BCancelar_Click"
           CausesValidation="False" meta:resourcekey="BCancelarResource1" />


<%--       <asp:Table ID="Table1" runat="server" Height="85px" Style="z-index: 112; left: 356px;
            position: absolute; top: 13px" Width="210px"  Visible="false"
			  meta:resourcekey="Table1Resource1">
            <asp:TableRow runat="server" >
                <asp:TableCell runat="server" meta:resourcekey="TableCellResource1">Altavoz 1:</asp:TableCell>
					 <asp:TableCell runat="server" HorizontalAlign="Left">
						 <asp:DropDownList ID="DropDownList1" runat="server" class="select">
							<asp:ListItem Value="A" meta:resourcekey="ListItemResource22">Activo</asp:ListItem>
							<asp:ListItem Value="S" meta:resourcekey="ListItemResource23">Seleccionado</asp:ListItem>
							<asp:ListItem Value="I" meta:resourcekey="ListItemResource24">Inactivo</asp:ListItem>
						</asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow runat="server" 
					meta:resourcekey="TableRowResource2">
                <asp:TableCell runat="server" meta:resourcekey="TableCellResource3">Altavoz 2:</asp:TableCell>
					 <asp:TableCell runat="server" >
						<asp:DropDownList ID="DropDownList2" runat="server"  class="select">
							<asp:ListItem Value="A" meta:resourcekey="ListItemResource25">Activo</asp:ListItem>
							<asp:ListItem Value="S" meta:resourcekey="ListItemResource26">Seleccionado</asp:ListItem>
							<asp:ListItem Value="I" meta:resourcekey="ListItemResource27">Inactivo</asp:ListItem>
						</asp:DropDownList>
					 </asp:TableCell>
				</asp:TableRow>
				<asp:TableRow runat="server" >
                <asp:TableCell runat="server" meta:resourcekey="TableCellResource5">Altavoz 3:</asp:TableCell>
					 <asp:TableCell runat="server" >
						<asp:DropDownList ID="DropDownList3" runat="server"  class="select">
							<asp:ListItem Value="A" meta:resourcekey="ListItemResource28">Activo</asp:ListItem>
							<asp:ListItem Value="S" meta:resourcekey="ListItemResource29">Seleccionado</asp:ListItem>
							<asp:ListItem Value="I" meta:resourcekey="ListItemResource30">Inactivo</asp:ListItem>
					</asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow runat="server" >
                <asp:TableCell runat="server" meta:resourcekey="TableCellResource7">Altavoz 4:</asp:TableCell>
					 <asp:TableCell runat="server" >
						<asp:DropDownList ID="DropDownList4" runat="server" class="select" >
							<asp:ListItem Value="A" meta:resourcekey="ListItemResource31">Activo</asp:ListItem>
							<asp:ListItem Value="S" meta:resourcekey="ListItemResource32">Seleccionado</asp:ListItem>
							<asp:ListItem Value="I" meta:resourcekey="ListItemResource33">Inactivo</asp:ListItem>
						</asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow runat="server" >
                <asp:TableCell runat="server" meta:resourcekey="TableCellResource9">Altavoz 5:</asp:TableCell>
					 <asp:TableCell runat="server" >
						<asp:DropDownList ID="DropDownList5" runat="server" class="select">
							<asp:ListItem Value="A" meta:resourcekey="ListItemResource34">Activo</asp:ListItem>
							<asp:ListItem Value="S" meta:resourcekey="ListItemResource35">Seleccionado</asp:ListItem>
							<asp:ListItem Value="I" meta:resourcekey="ListItemResource36">Inactivo</asp:ListItem>
						</asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow runat="server" >
                <asp:TableCell runat="server" meta:resourcekey="TableCellResource11">Altavoz 6:</asp:TableCell>
					 <asp:TableCell runat="server">
						<asp:DropDownList ID="DropDownList6" runat="server" class="select">
							<asp:ListItem Value="A" meta:resourcekey="ListItemResource37">Activo</asp:ListItem>
							<asp:ListItem Value="S" meta:resourcekey="ListItemResource38">Seleccionado</asp:ListItem>
							<asp:ListItem Value="I" meta:resourcekey="ListItemResource39">Inactivo</asp:ListItem>
						</asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow runat="server" >
                <asp:TableCell runat="server" meta:resourcekey="TableCellResource13">Altavoz 7:</asp:TableCell>
					 <asp:TableCell runat="server">
						<asp:DropDownList ID="DropDownList7" runat="server" class="select">
							<asp:ListItem Value="A" meta:resourcekey="ListItemResource40">Activo</asp:ListItem>
							<asp:ListItem Value="S" meta:resourcekey="ListItemResource41">Seleccionado</asp:ListItem>
							<asp:ListItem Value="I" meta:resourcekey="ListItemResource42">Inactivo</asp:ListItem>
						</asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>--%>
    </asp:Panel>

    <asp:Panel ID="PanelNoPermissions" runat="server" ForeColor="#00C000" 
		Style="z-index: -1; left: 100px; position: absolute; top: 420px; height: 60px; width: 797px;" 
		meta:resourcekey="PanelNoPermissions" Visible="false">
        <asp:Label ID="LblPermisos" runat="server" ForeColor="Red" Style="z-index: 100; left: 10px; width:auto; position : absolute; top: 15px" 
        Text="No tiene permisos para ver esta sección."
        meta:resourcekey="LblPermisosResource1">
        </asp:Label>
    </asp:Panel>

</asp:Content>