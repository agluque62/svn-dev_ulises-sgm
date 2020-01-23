<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="Redes.aspx.cs" Inherits="Redes" 
	CodeFileBaseClass="PageBaseCD40.PageCD40"	Title="Gestión de Redes" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <script type="text/javascript">

			function AbreVentana(ventana) {
			window.open(ventana);
		}
		
	</script>

    <asp:Label ID="Label3" runat="server" Text="GESTIÓN DE REDES" 
		 CssClass="labelPagina" meta:resourcekey="Label3Resource1"></asp:Label>
    <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"
				SkinID="MascaraListaElementos" meta:resourcekey="ListBox1Resource1">
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
    
    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 103; left: 500px; position: absolute;
        top: 370px" Text="Aceptar" Visible="False" Width="80px" UseSubmitBehavior="true" 
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
    
    <asp:LinkButton ID="BtModificar" runat="server" OnClick="BtModificar_Click" 
		 CausesValidation="False" Text="Modificar" SkinID="MascaraBotonModificar" 
		 meta:resourcekey="BtModificarResource1"/>  
        
	<asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Height="260px" Style="z-index: 107;
		left: 265px; position: absolute; top: 82px" Width="580px"  meta:resourcekey="Panel1Resource1">
		<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
			<asp:View ID="View1" runat="server">
				 <asp:Label ID="Label1" runat="server" Style="z-index: 102; left: 22px; position: absolute;
					  top: 14px" Text="Red:" meta:resourcekey="Label1Resource1"></asp:Label>
				 <asp:TextBox ID="TxtIdRed" runat="server" MaxLength="32" Style="z-index: 100; left: 22px;
					  position: absolute; top: 34px" Width="190px" meta:resourcekey="TxtIdRedResource1"></asp:TextBox>

				 <asp:Label ID="Label2" runat="server" Style="z-index: 104; left: 22px; position: absolute;
					  top: 60px" Text="Prefijo:" meta:resourcekey="Label2Resource1"></asp:Label>
				 <asp:DropDownList ID="DropDownList1" runat="server" Style="z-index: 101; left: 22px;
					  position: absolute; top: 80px" meta:resourcekey="DropDownList1Resource1" class="select">
				 </asp:DropDownList>

				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtIdRed"
					ErrorMessage="El campo Id. Recurso no puede estar vacio." Style="z-index: 103; left: 220px; position: absolute;
					top: 38px" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
			</asp:View>
			<asp:View ID="View2" runat="server">
                 <!-- El list box DDLTipoEquipoRecurso se utiliza para mostrar el texto del tipo de equipo configurado en cada recurso-->
                 <asp:DropDownList ID="DDLTipoEquipoRecurso" runat="server"  Enabled="false" BackColor="#FFF7E7" Visible="false" >
                    <asp:ListItem Value="P" meta:resourcekey="ListTipo_P">Pasarela</asp:ListItem>
                    <asp:ListItem Value="E" meta:resourcekey="ListTipo_E">Equipo Externo</asp:ListItem>
                    <asp:ListItem Value="O" meta:resourcekey="ListTipo_O">Otros</asp:ListItem>
                </asp:DropDownList>

				<asp:GridView ID="GVRecursos" runat="server" Style="z-index: 128; left: 15px; position: absolute;
                            top: 15px;" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"  Visible="true"
                            GridLines="None" PageSize="5" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None"
                            BorderWidth="2px" AllowPaging="True" 
					OnPageIndexChanging="GViewRecursos_OnSelectedIndexChanging"
                    OnRowDatabound="GViewRecursos_RowDataBound"  
					meta:resourcekey="GVRecursosResource1">
                    <FooterStyle   BackColor="#F7DFB5"/>
					<Columns>
                        <asp:BoundField DataField="IdRecurso" HeaderText="Recurso"    ItemStyle-Width="80px"  meta:resourcekey="BoundFieldResource1" />
                        <asp:TemplateField HeaderText="Tipo" meta:resourcekey="BoundFieldTipo">
                            <ItemTemplate>
                                <asp:Label ID="LbTipo" runat="server" Text='<%# Bind("Tipo") %>'  Visible="false"></asp:Label>
                                <asp:Label ID="LbValorTipo" Width="125px"  runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
						<asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-Width="80px"  meta:resourcekey="BoundFieldResource2" />
						<asp:BoundField DataField="SlotPasarela" HeaderText="Interface" 
							meta:resourcekey="BoundFieldResource3">
						<ItemStyle HorizontalAlign="Right" />
						</asp:BoundField>
						<asp:BoundField DataField="NumDispositivoSlot" HeaderText="Slot"   
							meta:resourcekey="BoundFieldResource4">
						<ItemStyle HorizontalAlign="Right" />
						</asp:BoundField>
					</Columns>
					<EditRowStyle BackColor="#8C4510" />
                    <HeaderStyle BackColor="#e13423" Font-Bold="True" ForeColor="White" Height="10px"/>
                    <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                    <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
				</asp:GridView>
			</asp:View>
		</asp:MultiView></asp:Panel>

	<asp:ValidationSummary ID="ValidationSummary1" runat="server" 
		Style="z-index: 115; left: 286px; position: absolute; top: 549px" 
		  meta:resourcekey="ValidationSummary1Resource1" />
		
	<asp:Button ID="IBDatosGenerales" runat="server" SkinID="ButtonTab"
		OnClick="OnButton_Click" Style="z-index: 108; left: 266px; position: absolute;
		top: 66px " meta:resourcekey="IBDatosGeneralesResource1" />
	<asp:Button ID="IBRecursos" runat="server" SkinId="ButtonTab"
		OnClick="OnButton_Click" Style="z-index: 110; left: 372px; position: absolute;
		top: 66px" meta:resourcekey="IBRecursosResource1" />

    <asp:LinkButton ID="LBImprimir" runat="server" SkinID="MascaraBotonImprimir" Visible="false"
		 meta:resourcekey="LBImprimirResource1">Imprimir</asp:LinkButton>
            
</asp:Content>

