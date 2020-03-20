<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="RecursosRadio.aspx.cs" Inherits="RecursosDeRadio" 
CodeFileBaseClass="PageBaseCD40.PageCD40"	Title="Gestión de Recursos Radio" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1"  %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>  

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

  <script type="text/javascript">
      function AbreVentana(ventana) {
          window.open(ventana);
      }
	</script>

	 <asp:Label ID="Label7" runat="server" Text="GESTIÓN DE RECURSOS RADIO" 
		  CssClass="labelPagina" meta:resourcekey="Label7Resource1" ></asp:Label>
	<asp:ListBox ID="ListBox1" runat="server" 
        OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" AutoPostBack="True"
        SkinID="MascaraListaElementosWide" Style="z-index: 100; left: 15px; width:255px; height:455px; position: absolute; top: 58px"  meta:resourcekey="ListBox1Resource1">
    </asp:ListBox>

    <asp:DropDownList ID="FiltroBusquedaRD" runat="server" AutoPostBack="True" Style="z-index: 100; left: 20px; width:172px; 
        position: absolute; top: 525px"
            OnSelectedIndexChanged="FiltroBusquedaRD_SelectedIndexChanged" SkinID="MascaraFiltroBusqueda" 
            meta:resourcekey="FiltroBusquedaRDResource">
                <asp:ListItem Value="0" meta:resourcekey="ListItemFiltroBusquedaRDResource0">Seleccione Filtro:</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="ListItemFiltroBusquedaRDResource1">Mostrar Todos</asp:ListItem>
                <asp:ListItem Value="2" meta:resourcekey="ListItemFiltroBusquedaRDResource2">Buscar por Nombre</asp:ListItem>
                <asp:ListItem Value="3" meta:resourcekey="ListItemFiltroBusquedaRDResource3">Buscar por Tipo</asp:ListItem>
                <asp:ListItem Value="4" meta:resourcekey="ListItemFiltroBusquedaRDResource4">Orden Alfabético (A-Z)</asp:ListItem>
                <asp:ListItem Value="5" meta:resourcekey="ListItemFiltroBusquedaRDResource5">Orden Alfabético (Z-A)</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="FiltroTipoRD" runat="server" AutoPostBack="True" visible="false" Style="z-index: 100; left: 20px; width:172px; 
        position: absolute; top: 550px"
            OnSelectedIndexChanged="FiltroTipoRD_SelectedIndexChanged" SkinID="MascaraFiltroTipo" 
            meta:resourcekey="FiltroTipoRDResource">
                <asp:ListItem Value="0" meta:resourcekey="ListItemFiltroTipoRDResource0">Tipo Interface:</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="ListItemFiltroTipoRDResource1">RX</asp:ListItem>
                <asp:ListItem Value="2" meta:resourcekey="ListItemFiltroTipoRDResource2">TX</asp:ListItem>
                <asp:ListItem Value="3" meta:resourcekey="ListItemFiltroTipoRDResource3">TX-RX</asp:ListItem>
                <asp:ListItem Value="4" meta:resourcekey="ListItemFiltroTipoRDResource4">HF-TX</asp:ListItem>
                <asp:ListItem Value="5" meta:resourcekey="ListItemFiltroTipoRDResource5">Audio M+N</asp:ListItem>
    </asp:DropDownList>
    <asp:TextBox ID="FiltroNombreRD" runat="server" visible="false"
        Style="z-index: 100; left: 20px; width:172px; position: absolute; top: 550px" meta:resourcekey="FiltroNombreRDResource">
        </asp:TextBox>
    <asp:Button ID="ButtonFiltroBuscarRD" runat="server" Style="z-index: 105; left: 55px; position: absolute;
        top: 575px" Text="Buscar" Width="100px" Visible="False" UseSubmitBehavior="true" 
		  OnClick="ButtonFiltroBuscarRD_Click" meta:resourcekey="ButtonFiltroBuscarRDResource" />


    <asp:LinkButton ID="BtNuevo" runat="server" OnClick="BtNuevo_Click" 
		  CausesValidation="False" Text="Nuevo" SkinID="MascaraBotonNuevo" 
		  meta:resourcekey="BtNuevoResource1"/>
    <asp:LinkButton ID="BtEliminar" runat="server" OnClick="BtEliminar_Click" 
		  CausesValidation="False" Text="Eliminar" SkinID="MascaraBotonEliminar" 
		  meta:resourcekey="BtEliminarResource1"/>
      <ajaxtoolkit:confirmbuttonextender ID="BtEliminar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
      </ajaxtoolkit:confirmbuttonextender>
    <asp:Label ID="LError" runat="server" ForeColor="Red" Style="left: 205px; position: absolute;
        top: 610px; z-index: 103;" Width="643px" Height="24px" 
		  meta:resourcekey="LErrorResource1"></asp:Label>
    <asp:LinkButton ID="BtModificar" runat="server" OnClick="BtModificar_Click" 
		  CausesValidation="False" Text="Modificar" SkinID="MascaraBotonModificar" 
		  meta:resourcekey="BtModificarResource1"/>
    <asp:Button ID="BtAceptar" runat="server" Style="z-index: 105; left: 365px; position: absolute;
        top: 533px" Text="Aceptar" Width="100px" OnClick="BtAceptar_Click" UseSubmitBehavior="true" 
		 Visible="False" meta:resourcekey="BtAceptarResource1"/>
      <ajaxtoolkit:confirmbuttonextender ID="BtAceptar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
      </ajaxtoolkit:confirmbuttonextender>
    <asp:LinkButton ID="BtCancelar" runat="server" OnClick="BtCancelar_Click" 
		  CausesValidation="False" Text="Cancelar" SkinID="MascaraBotonCancelar" 
		  meta:resourcekey="BtCancelarResource1"/>
      <ajaxtoolkit:confirmbuttonextender ID="BtCancelar_ConfirmButtonExtender" 
          runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
      </ajaxtoolkit:confirmbuttonextender>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores:"
        Style="z-index: 108; left: 32px; position: absolute; top: 561px" 
		  meta:resourcekey="ValidationSummary1Resource1" />

    <asp:Panel ID="Panel1" runat="server" Height="450px" Style="z-index: 109; left: 275px; position: absolute; top: 58px"
        Width="700px" BorderStyle="Inset"
        meta:resourcekey="Panel1Resource1">
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View1" runat="server">
                <div runat="server" style="position:absolute; left: 0px; top: 1px; width: 200px; height: 250px;">
                    <asp:Label ID="Label5" runat="server" Style="z-index: 100; left: 20px; position: absolute; top: 13px"
                        Text="Id. Recurso:" meta:resourcekey="Label5Resource1"></asp:Label>
                    <asp:TextBox ID="TxtIdRecurso" runat="server" Style="z-index: 101; left: 20px; position: absolute; top: 33px"
                        Width="155px" MaxLength="32" Enabled="False"
                        meta:resourcekey="TxtIdRecursoResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtIdRecurso"
                        ErrorMessage="El campo Id. Recurso no puede estar vacio." Style="z-index: 108; left: 190px; position: absolute; top: 33px"
                        meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtIdRecurso"
                        ErrorMessage="Carácter inválido" Style="z-index: 141; left: 182px; position: absolute; top: 33px"
                        ValidationExpression="^[\w-.]*$"
                        meta:resourcekey="RequiredFieldValidator2Resource1">*</asp:RegularExpressionValidator>
                    <!--
                    <asp:Label ID="LblConfiguracion" runat="server" Style="z-index: auto; left:20px; position:absolute; top:63px"
                        Text="Configuración" meta:resourcekey="LblConfiguracionResource1"></asp:Label>
                        <asp:RadioButton ID="RbConfiguracionBasica" runat="server" Style="z-index: auto; left:20px; position:absolute; top:83px" AutoPostBack="true" Enabled="false"
                            Text="Básica" meta:resourcekey="RbConfiguracionBasicaResource1" Checked="true" GroupName="Configuracion" OnCheckedChanged="RbConfiguracionBasica_CheckedChanged"></asp:RadioButton>
                        <asp:RadioButton ID="RbConfiguracionNM" runat="server" Style="z-index: auto; left:100px; position:absolute; top:83px" AutoPostBack="true" Enabled="false"
                            Text="N+M" meta:resourcekey="RbConfiguracionNMResource1" Checked="false" GroupName="Configuracion" OnCheckedChanged="RbConfiguracionNM_CheckedChanged"></asp:RadioButton>
                    -->
                    <asp:Label ID="Label6" runat="server" Style="z-index: 102; left: 20px; position: absolute; top: 63px"
                        Text="Tipo:" meta:resourcekey="Label2Resource1"></asp:Label>
                    <asp:DropDownList ID="DListTipo" runat="server" Style="z-index: 103; left: 20px; position: absolute; top: 83px"
                        Width="155px" Enabled="False" OnSelectedIndexChanged="DListTipo_SelectedIndexChanged" AutoPostBack="True"
                        meta:resourcekey="DListTipoResource1" class="select">
                        <asp:ListItem Value="0" meta:resourcekey="ListTipo_Audio_RX">Audio RX</asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="ListTipo_Audio_TX">Audio TX</asp:ListItem>
                        <asp:ListItem Value="2" meta:resourcekey="ListTipo_Audio_RXTX">Audio RX TX</asp:ListItem>
                        <asp:ListItem Value="3" meta:resourcekey="ListTipo_Audio_HF">Audio HF-TX</asp:ListItem>
                        <asp:ListItem Value="4" meta:resourcekey="ListTipo_Audio_NM">Audio M+N</asp:ListItem>
                    </asp:DropDownList>
					
                        <%-- <asp:ListItem meta:resourcekey="ListItemResource4" Value="7">Datos RX</asp:ListItem>
                        <asp:ListItem meta:resourcekey="ListItemResource5" Value="8">Datos TX</asp:ListItem>
                        <asp:ListItem meta:resourcekey="ListItemResource6" Value="9">Datos RX TX</asp:ListItem> --%>
					                    
                        <!--<asp:ListItem Value="5" style="display:none" meta:resourcekey="ListTipo_Audio_NM_Tx">Audio M+N Tx</asp:ListItem>
                        <asp:ListItem Value="6" meta:resourcekey="ListTipo_Audio_NM_RTx">Audio M+N RxTx</asp:ListItem>-->

                    <asp:Label ID="LDestino" runat="server" Style="left: 20px; position: absolute; top: 113px; z-index: 104;"
                        Text="Destino: " Width="53px" meta:resourcekey="LDestinoResource1"></asp:Label>
                    <asp:TextBox ID="TBDestino" runat="server" Style="z-index: 110; left: 20px; position: absolute; top: 133px"
                        Width="155px" Enabled="False" ReadOnly="True"
                        meta:resourcekey="TBDestinoResource1">Sin destino asociado</asp:TextBox>
                    <asp:DropDownList ID="DLDestino" runat="server" AppendDataBoundItems="True" Style="left: 17px; position: absolute; top: 183px; z-index: 105;"
                        Width="155px" Enabled="False" class="select"
                        Visible="False" meta:resourcekey="DLDestinoResource1">
                        <asp:ListItem Value="0" meta:resourcekey="ListItemResource7">&lt;Selecciona destino&gt;</asp:ListItem>
                    </asp:DropDownList>

                   

                      <asp:Label ID="Label42" runat="server" Style="z-index: 106; left: 20px; position: absolute; top: 163px"
                        Text="Offset de Frecuencias:" meta:resourcekey="Label8Resource1"></asp:Label>

                     <asp:DropDownList ID="DDLOffsetGeneral" runat="server" Enabled="False" Style="z-index: 107; left: 20px; position: absolute; top: 183px"
                        Width="155px"  class="select">
                        <asp:ListItem Value="0">Off</asp:ListItem>
                        <asp:ListItem Value="1">KHz 7 5</asp:ListItem>
                        <asp:ListItem Value="2">KHz 5 0</asp:ListItem>
                        <asp:ListItem Value="3">KHz 2 5</asp:ListItem>
                        <asp:ListItem Value="4">Hz 0 0</asp:ListItem>
                        <asp:ListItem Value="5">KHz minus 2 5</asp:ListItem>
                        <asp:ListItem Value="6">KHz minus 5 0</asp:ListItem>
                        <asp:ListItem Value="7">KHz minus 7 5</asp:ListItem>
                        <asp:ListItem Value="8">KHz 8</asp:ListItem>
                        <asp:ListItem Value="9">KHz 4</asp:ListItem>
                        <asp:ListItem Value="10">KHz minus 4</asp:ListItem>
                        <asp:ListItem Value="11">KHz minus 8</asp:ListItem>
                        <asp:ListItem Value="12">KHz 7 3</asp:ListItem>
                        <asp:ListItem Value="13">KHz minus 7 3</asp:ListItem>
                       </asp:DropDownList>

                     <asp:Label ID="Label8" runat="server" Style="z-index: 106; left: 20px; position: absolute; top: 213px"
                        Text="Emplazamiento:" meta:resourcekey="Label6Resource1"></asp:Label>
                    
                    <asp:DropDownList ID="DListEmplazamiento" runat="server" Enabled="False" Style="z-index: 107; left: 20px; position: absolute; top: 233px"
                        Width="155px"  class="select"
                        AppendDataBoundItems="True" meta:resourcekey="DListEmplazamientoResource1">
                        <asp:ListItem Value="0" meta:resourcekey="ListItemResource8">&lt;Selecciona emplazamiento&gt;</asp:ListItem>
                    </asp:DropDownList>


                     <asp:Label ID="LabelZona" runat="server" Style="z-index: 102; left: 20px; position: absolute; top: 263px; height: 18px; width: 155px;"
                    Text="Zonas:" meta:resourcekey="LabelZonasResource1"></asp:Label>
                    
                    <asp:DropDownList ID="DListZonas" runat="server" Style="z-index: 103; left: 20px; position: absolute; top: 283px"
                        Width="155px" class="select" AutoPostBack="False" Enabled="False"                        
                        AppendDataBoundItems="True" meta:resourcekey="DListZonasResource1">
                        <asp:ListItem Value="0" meta:resourcekey="ListItemResource34">&lt;Selecciona zona&gt;</asp:ListItem>
                    </asp:DropDownList>

                     

<%--                    <asp:Label ID="Label44" runat="server" Text="Tiempo CLD (seg):" Style="left:20px; top:313px; position:absolute; width:auto"
                        ForeColor="Black" meta:resourcekey="Label44Resource1"></asp:Label>
                     <asp:TextBox ID="TextBoxCLD" runat="server" Width="43px" Enabled="False" Style="top: 333px; left: 20px; position: absolute;"
                        meta:resourcekey="TxtTiempoCLDResource1"></asp:TextBox>--%>

                </div>
                <div runat="server" style="position:absolute; left: 201px; top: 10px; width: 500px; height: 280px;">
                    <ajaxToolkit:Accordion ID="Accordion1"
                        runat="server" SelectedIndex="0"
                        CssClass="accordion"
                        ContentCssClass="accordionContent"
                        HeaderCssClass="accordionHeader"
                        HeaderSelectedCssClass="accordionHeaderSelected" AutoSize="Fill" Width="490px" Height="250px">
                        <Panes>
                            <ajaxToolkit:AccordionPane ID="AccordioPane2" runat="server">
                                <Header>
                                    <asp:Label ID="LblHeader1" runat="server" meta:resourcekey="StrHeader1" ForeColor="White"/>
                                </Header>
                                <Content>
                                    <asp:Label ID="Label1" runat="server" Style="left: 15px; position: absolute; top: 35px; width:auto"  meta:resourcekey="StrDireccionIp">Dirección IP</asp:Label>
                                    <asp:TextBox ID="TBIpGestor" runat="server" Style="z-index: auto; left: 15px; position: absolute; top: 55px; width: 140px;" Enabled="false"></asp:TextBox>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                                <Header>
                                    <asp:Label ID="LblHeader2" runat="server" meta:resourcekey="StrHeader2" ForeColor="White"/>
                                </Header>
                                <Content>
                                    <asp:Label ID="Label2" runat="server" Style="left: 15px; position: absolute; top: 70px; width:auto" meta:resourcekey="StrOID">Valor OID</asp:Label>
                                    <asp:TextBox ID="TBOid" runat="server" Style="z-index: auto; left: 15px; position: absolute; top: 90px; width: 180px;" Enabled="false"></asp:TextBox>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                                <Header>
                                    <asp:Label ID="LblHeader3" runat="server" meta:resourcekey="StrHeader3" ForeColor="White"/>
                                </Header>
                                <Content>
                                    <asp:Button ID="BtnAnadirRango" runat="server" Text="Añadir" OnClick="BtnAnadirRango_Click" Style="position: absolute; left: 15px; top: 110px" Enabled="false" meta:resourcekey="StrAnadirRango" />
                                    <asp:TextBox ID="TBMin" runat="server" OnClick="this.value=''" Style="z-index: auto; left: 15px; position: absolute; top: 140px; width: 80px;" meta:resourcekey="StrRangoMinimo">Valor Mínimo</asp:TextBox>
                                    <asp:TextBox ID="TBMax" runat="server" OnClick="this.value=''" Style="z-index: auto; left: 15px; position: absolute; top: 160px; width: 80px;" meta:resourcekey="StrRangoMaximo">Valor Máximo</asp:TextBox>
                                    <asp:GridView ID="GVRangos" runat="server" Visible="True" Style="z-index: auto; left: 100px; position: absolute; top: 110px"
                                        AutoGenerateColumns="False" BackColor="#CCDDF9" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" CellSpacing="2" Height="70px" PageSize="2" AllowPaging="true"
                                        EmptyDataText="No data in the data source." Width="220px" OnRowDeleting="GVRangos_OnRowDeleting"
                                        OnPageIndexChanging="GVRangos_OnSelectedIndexChanging" SkinId="GridViewSkin">
                                        <Columns>
                                            <asp:CommandField meta:resourcekey="CommandFieldResource1" ShowDeleteButton="True" />
                                            <asp:BoundField DataField="Min" HeaderText="Frec. Mín." />
                                            <asp:BoundField DataField="Max" HeaderText="Frec. Máx." />
                                        </Columns>
                                        <HeaderStyle Height="10px" />
                                    </asp:GridView>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                        </Panes>
                    </ajaxToolkit:Accordion>

                    <ajaxToolkit:Accordion ID="Accordion2"
                        runat="server" SelectedIndex="0"
                        CssClass="accordion" Visible="false"
                        ContentCssClass="accordionContent"
                        HeaderCssClass="accordionHeader"
                        HeaderSelectedCssClass="accordionHeaderSelected" AutoSize="Fill" Width="490px" Height="435px" Enabled="false">
                        <Panes>
              
                    <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                                <Header>
                                    <asp:Label ID="Label9" runat="server" meta:resourcekey="StrHeader1" ForeColor="White"/>
                                </Header>
                                <Content>
                                    <asp:Label ID="Label11" runat="server" Style="left: 15px; position: absolute; top: 35px; width:auto"  meta:resourcekey="StrDireccionIp">Dirección IP</asp:Label>
                                    <asp:TextBox ID="TextBox1" runat="server" Style="z-index: auto; left: 15px; position: absolute; top: 55px; width: 140px;"></asp:TextBox>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                                <Header>
                                    <asp:Label ID="Label13" runat="server" meta:resourcekey="StrHeader2" ForeColor="White"/>
                                </Header>
                                <Content>
                                    <asp:Label ID="Label22" runat="server" Style="left: 15px; position: absolute; top: 70px; width:auto" meta:resourcekey="StrOID">Valor OID</asp:Label>
                                    <asp:TextBox ID="TextBox2" runat="server" Style="z-index: auto; left: 15px; position: absolute; top: 90px; width: 180px;"></asp:TextBox>
                                </Content>
                            </ajaxToolkit:AccordionPane> 

                            <ajaxToolkit:AccordionPane ID="AccordionPane6" runat="server">
                                <Header>
                                    <asp:Label ID="Label29" runat="server" meta:resourcekey="StrHeader4" ForeColor="White"/>
                                </Header>
                                <Content>
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <asp:Panel ID="PnlMarcaModelo" runat="server" GroupingText="Marca/Modelo" Width="220px" meta:resourcekey="PnlMarcaModeloResource">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label40" runat="server" Text="Marca" meta:resourcekey="lbMarcaResource" ></asp:Label>
                                                            </td>
                                                            <td>
															<asp:DropDownList ID="DDLMarca" runat="server" Width="130px" class="select">
                                                                    <asp:ListItem Value="1000">ROHDE 4200</asp:ListItem>
                                                                    <asp:ListItem Value="1001">JOTRON 7000</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                         </tr>
                                                     </table>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="PnlEquipo" runat="server" GroupingText="Equipo" Width="128px" meta:resourcekey="PnlEquipoResource">
                                                    <asp:RadioButton ID="RBReceptor" runat="server" Text="Receptor" GroupName="Equipo" Checked="true"
                                                         meta:resourcekey="RbReceptorResource"/><br />
                                                    <asp:RadioButton ID="RBTransmisor" runat="server" Text="Transmisor" GroupName="Equipo" 
                                                         meta:resourcekey="RBTransmisorResource"/><br />
                                                    <asp:RadioButton ID="RBTransceptor" runat="server" Text="Transceptor" GroupName="Equipo" Enabled="false" 
                                                         meta:resourcekey="RBTransceptorResource"/>
                                                </asp:Panel>
                                            </td>
                                            <!--
                                            <td>
                                                <asp:Panel ID="PnlCanal" runat="server" GroupingText="Canal" Width="170px"  meta:resourcekey="PnlCanalResource">
                                                    <asp:RadioButton ID="RBMono" runat="server" AutoPostBack="True" Text="Monocanal" GroupName="Canal"  Checked="true" meta:resourcekey="RBMonoResource"/><br />
                                                    <asp:RadioButton ID="RBMulti" runat="server" AutoPostBack="True" Text="Multicanal" GroupName="Canal" meta:resourcekey="RBMultiResource"/><br />
                                                    <asp:RadioButton ID="RBOtros" runat="server" Text="Otros" GroupName="Canal" Visible="false" meta:resourcekey="RBOtrosResource"/>
                                                </asp:Panel>
                                            </td>
                                            -->
                                            <td>
                                                <asp:Panel ID="PnlFrecuencia" runat="server" GroupingText="Frecuencia" Width="120px"  meta:resourcekey="PnlFrecuenciaResource">                                                    
                                                    <asp:RadioButton ID="RBVHF" runat="server" Text="VHF" GroupName="Frecuencia" Checked="true" AutoPostBack="true"
                                                        OnCheckedChanged="Checked_FrequencyType" meta:resourcekey="RBVHFResource"/><br />
                                                    <asp:RadioButton ID="RBUHF" runat="server" Text="UHF" AutoPostBack="true"
                                                        OnCheckedChanged="Checked_FrequencyType" GroupName="Frecuencia" meta:resourcekey="RBUHFResource"/><br />
                                                    <asp:RadioButton ID="RBHF" runat="server" Text="HF" GroupName="Frecuencia"  Visible="false" AutoPostBack="true"
                                                        OnCheckedChanged="Checked_FrequencyType" meta:resourcekey="RBHFResource"/>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Panel ID="PnlModo" runat="server" GroupingText="Modo" Width="100%"  meta:resourcekey="PnlModoResource">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="RBPrincipal" runat="server" Text="Principal (Monocanal)" GroupName="Modo" Checked="true" AutoPostBack="true" OnCheckedChanged="ConfiguraOpcionesM_N" meta:resourcekey="RBPrincialResource"/>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="RBReserva" runat="server" Text="Reserva (Multicanal)" GroupName="Modo" style="margin-left:30px" AutoPostBack="true" OnCheckedChanged="ConfiguraOpcionesM_N" meta:resourcekey="RBReservaResource"/>
                                                            </td>
                                                                <%--<asp:RadioButton ID="RBAmbos" runat="server" Text="Ambos" GroupName="Modo" Visible="false" meta:resourcekey="RBAmbosResource"/>--%>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Panel ID="PnlPrioridad" runat="server" GroupingText="Parametros M+N" Width="100%"  meta:resourcekey="PnlParametrosResource">
                                                    <table>
                                                        <tr id="TrPriority">
                                                            <td style="width:50px">
                                                                <asp:Label ID="Label34" runat="server"  meta:resourcekey="PrioridadResource"/>
                                                            </td>
                                                            <td style="width:100px">
                                                                <asp:TextBox ID="TBPriority" runat="server" Width="50px" Text="50"></asp:TextBox>
                                                            </td>
                                                            <td style="width:80px">
                                                                <asp:Label ID="Label35" runat="server" style="margin-left:20px" Visible="false" meta:resourcekey="PuertoResource"/>
                                                            </td>
                                                            <td style="width:100px">
                                                                <asp:TextBox ID="TbPuerto" runat="server" Width="50px"  Text="0" Visible="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="TrOffset">
<%--                                                            <td>
                                                                <asp:Label ID="Label36" runat="server" Text="Offset"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DDLOffset" runat="server" Width="70px" class="select">
                                                                    <asp:ListItem Value="0">Off</asp:ListItem>
                                                                    <asp:ListItem Value="1">KHz 7 5</asp:ListItem>
                                                                    <asp:ListItem Value="2">KHz 5 0</asp:ListItem>
                                                                    <asp:ListItem Value="3">KHz 2 5</asp:ListItem>
                                                                    <asp:ListItem Value="4">Hz 0 0</asp:ListItem>
                                                                    <asp:ListItem Value="5">KHz minus 2 5</asp:ListItem>
                                                                    <asp:ListItem Value="6">KHz minus 5 0</asp:ListItem>
                                                                    <asp:ListItem Value="7">KHz minus 7 5</asp:ListItem>
                                                                    <asp:ListItem Value="8">KHz 8</asp:ListItem>
                                                                    <asp:ListItem Value="9">KHz 4</asp:ListItem>
                                                                    <asp:ListItem Value="10">KHz minus 4</asp:ListItem>
                                                                    <asp:ListItem Value="11">KHz minus 8</asp:ListItem>
                                                                    <asp:ListItem Value="12">KHz 7 3</asp:ListItem>
                                                                    <asp:ListItem Value="13">KHz minus 7 3</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>--%>
                                                            <td>
                                                                <asp:Label ID="Label37" runat="server" meta:resourcekey="CanalizacionResource"/>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:DropDownList ID="DDLCanalizacion" runat="server" Width="90px" class="select">
                                                                    <asp:ListItem Value="0">Default</asp:ListItem>
                                                                    <asp:ListItem Value="1">KHz 8 33</asp:ListItem>
                                                                    <asp:ListItem Value="2">KHz 12 5</asp:ListItem>
                                                                    <asp:ListItem Value="3">KHz 25 00</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%--<asp:TextBox ID="TbCanalizacion" runat="server" Width="50px" style="margin-left:35px" Text="0"></asp:TextBox>--%>
                                                            </td>
                                                        <tr id="Modulacion">
                                                            <td>
                                                                <asp:Label ID="Label38" runat="server" meta:resourcekey="ModulacionResource"/>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DDLModulacion" runat="server" Width="80px" class="select">
                                                                    <asp:ListItem Value="0">AM</asp:ListItem>
                                                                    <asp:ListItem Value="1">Reserved</asp:ListItem>
                                                                    <asp:ListItem Value="2">ACARS</asp:ListItem>
                                                                    <asp:ListItem Value="3">VDL2</asp:ListItem>
                                                                    <asp:ListItem Value="4">AM CT</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%--<asp:TextBox ID="TbModulacion" runat="server" Width="50px" Text="0"></asp:TextBox>--%>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label39" runat="server" style="margin-left:20px" Visible="false" meta:resourcekey="PotencianResource"/>
                                                            </td>
                                                            <td>
                                                                <%-- 	    PowerLevelsDefault = 0,
                                                                            Low = 1,
                                                                            Normal = 2
                                                                        --%>
                                                                <asp:DropDownList ID="DDLPotencia" runat="server" Width="90px" Visible="false" class="select">
                                                                    <asp:ListItem Value="0">Default</asp:ListItem>
                                                                    <asp:ListItem Value="1">Low</asp:ListItem>
                                                                    <asp:ListItem Value="2">Normal</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%--<asp:TextBox ID="TbPotencia" runat="server" Width="50px" style="margin-left:35px" Text="0"></asp:TextBox>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                
                                                    
<%--                                                    <asp:RadioButton ID="RBAlta" runat="server"  GroupName="Prioridad" Checked="true" meta:resourcekey="RBAltaResource"/><br />
                                                    <asp:RadioButton ID="RBMedia" runat="server" GroupName="Prioridad" meta:resourcekey="RBMediaResource"/><br />
                                                    <asp:RadioButton ID="RBBaja" runat="server" GroupName="Prioridad" meta:resourcekey="RBBajaResource"/>--%>
                                                </asp:Panel>
                                            </td>                                        
                                        </tr>
                                    </table>
                                    <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="TBPriority"
                                            ErrorMessage="El valor debe estar comprendido entre 0 y 100" MaximumValue="100" Enabled="true"
                                            MinimumValue="0" Style="z-index: 175; left: 190px; position: absolute; top: 245px"
                                            Type="Integer" meta:resourcekey="RangeValidatorHFFrequency">*</asp:RangeValidator>
                                </Content>
                            </ajaxToolkit:AccordionPane>

                                                        <ajaxToolkit:AccordionPane ID="AccordionPane5" runat="server">
                                <Header>
                                    <asp:Label ID="Label28" runat="server" meta:resourcekey="StrHeader3" ForeColor="White"/>
                                </Header>
                                <Content>
<%--                                    <asp:Button ID="Button1" runat="server" Text="Añadir" OnClick="BtnAnadirRangoNM_Click" Style="position: absolute; left: 15px; top: 110px" meta:resourcekey="StrAnadirRango" />
                                    <asp:TextBox ID="TextBox3" runat="server" OnClick="this.value=''" Style="z-index: auto; left: 15px; position: absolute; top: 140px; width: 80px;" meta:resourcekey="StrRangoMinimo">Valor Mínimo</asp:TextBox>
                                    <asp:TextBox ID="TextBox4" runat="server" OnClick="this.value=''" Style="z-index: auto; left: 15px; position: absolute; top: 160px; width: 80px;" meta:resourcekey="StrRangoMaximo">Valor Máximo</asp:TextBox>
                                    <asp:GridView ID="GVRangos_NM" runat="server" Visible="True" Style="z-index: auto; left: 100px; position: absolute; top: 110px"
                                        AutoGenerateColumns="False" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None"
                                        BorderWidth="1px" CellPadding="3" CellSpacing="2" Height="70px" PageSize="2" AllowPaging="true"
                                        EmptyDataText="No data in the data source." Width="220px" OnRowDeleting="GVRangosNM_OnRowDeleting"
                                        OnPageIndexChanging="GVRangosNM_OnSelectedIndexChanging">
                                        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                                        <Columns>
                                            <asp:CommandField meta:resourcekey="CommandFieldResource1" ShowDeleteButton="True" />
                                            <asp:BoundField DataField="Min" HeaderText="Frec. Mín." />
                                            <asp:BoundField DataField="Max" HeaderText="Frec. Máx." />
                                        </Columns>
                                        <HeaderStyle BackColor="#e13423" Font-Bold="True" ForeColor="White" Height="10px" />
                                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>--%>
                                    <asp:Table runat="server" ID="TblTunedFreq" Style="position: absolute; top: 288px" Visible="true">
                                        <asp:TableRow ID="TableRow6" runat="server" >
                                            <asp:TableCell ID="TableCell17" runat="server">
                                                <asp:Label runat="server" ID="LblTunedFreq" meta:resourcekey="LblTunedFreqResource1" ></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell18" runat="server">
                                                <asp:TextBox ID="TBTunedFrequency" runat="server" Width="50px" ></asp:TextBox>
                                                <asp:Label ID="LblPoint" runat="server">.</asp:Label>
                                                <asp:TextBox ID="TBTunedDecimals" runat="server" Width="50px" ></asp:TextBox>
                                            </asp:TableCell>
                                            <asp:TableCell ID="TableCell19" runat="server">
                                                <asp:Label ID="Label31" runat="server">KHz</asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>

                                    <!-- ************************************************************  -->
                                    <!-- Rango de frecuencias válidas para la frecuencia a sintonizar  -->
                                    <!-- ************************************************************  -->
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TBTunedFrequency"
                                            ErrorMessage="El campo frecuencia no puede estar vacio." Enabled="false"
                                            Style="z-index: 120; left: 295px; position: absolute; top: 295px"
                                            meta:resourcekey="RangeValidatorFrequencyResource">*</asp:RequiredFieldValidator>
                                    

                                     <asp:RangeValidator ID="RangeValidatorHFFrequency" runat="server" ControlToValidate="TBTunedFrequency"
                                            ErrorMessage="Frecuencia HF: el valor debe estar comprendido entre 2.000 y 30.000 kHz" MaximumValue="30" Enabled="false"
                                            MinimumValue="2" Style="z-index: 175; left: 295px; position: absolute; top: 295px"
                                            Type="Double" meta:resourcekey="RangeValidatorHFFrequencyResource">*
                                     </asp:RangeValidator>
                                     <asp:RangeValidator ID="RangeValidatorVHFFrequency" runat="server" ControlToValidate="TBTunedFrequency"
                                            ErrorMessage="Frecuencia VHF: el valor debe estar comprendido entre 118.000 y 144.000 kHz" MaximumValue="144" Enabled="false"
                                            MinimumValue="118" Style="z-index: 175; left: 295px; position: absolute; top: 295px"
                                            Type="Double" meta:resourcekey="RangeValidatorVHFFrequencyResource">*
                                     </asp:RangeValidator>
                                     <asp:RangeValidator ID="RangeValidatorUHFFrequency" runat="server" ControlToValidate="TBTunedFrequency"
                                            ErrorMessage="Frecuencia UHF: el valor debe estar comprendido entre 225.000 y 400.000 kHz" MaximumValue="400" Enabled="false"
                                            MinimumValue="225" Style="z-index: 175; left: 295px; position: absolute; top: 295px" 
                                            Type="Double" meta:resourcekey="RangeValidatorUHFFrequencyResource">*
                                     </asp:RangeValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionTBTunedFrequency" runat="server" ControlToValidate="TBTunedFrequency"
                                            ErrorMessage="Caracter no válido en la frecuencia principal"  Enabled="true"
                                            Style="z-index: 175; left: 295px; position: absolute; top: 295px"
                                            ValidationExpression="^([0-9]{1,3})$"
                                            meta:resourcekey="RegularExpressionTBTunedFrequencyRes">*</asp:RegularExpressionValidator>

                                    <asp:RegularExpressionValidator ID="RangeValidatorDecimals" runat="server" ControlToValidate="TBTunedDecimals"
                                            ErrorMessage="El valor debe estar comprendido entre 000 y 999"  Enabled="true"
                                            Style="z-index: 175; left: 295px; position: absolute; top: 295px"
                                            ValidationExpression="^([0-9]{3})$"
                                            meta:resourcekey="RangeValidatorDecimalsResource">*</asp:RegularExpressionValidator>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                        </Panes>
                    </ajaxToolkit:Accordion>

                </div>

            </asp:View>
            <asp:View ID="View2" runat="server">
                <asp:Label ID="Label4" runat="server" Style="z-index: 104; left: 22px; position: absolute; top: 14px" Visible="false"
                    Text="Servidor SIP:" meta:resourcekey="Label4Resource1"></asp:Label>
                <asp:TextBox ID="TxtServidorSIP" runat="server" Style="z-index: 103; left: 22px; position: absolute; top: 34px" Visible="false"
                    Width="155px" Enabled="False" meta:resourcekey="TxtServidorSIPResource1"></asp:TextBox>
                
                <asp:Label ID="LblKAP" runat="server" Style="z-index: 168; left: 300px; position: absolute; top: 14px"
                    Text="Keep Alive Period (ms):" Width="197px"
                    meta:resourcekey="LblKAPResource1"></asp:Label>
                <asp:TextBox ID="TxtKAP" runat="server" Style="z-index: 170; left: 300px; position: absolute; top: 34px"
                    Width="43px" Enabled="False"
                    meta:resourcekey="TxtKAPResource1">200</asp:TextBox>

                <asp:Label ID="Label12" runat="server" Style="left: 22px; position: absolute; top: 63px; z-index: 125; height: 18px;"
                    Text="Tamaño paquete RTP (msg):" meta:resourcekey="Label12Resource1"></asp:Label>
                <asp:TextBox ID="TxtTamanioPaquete" runat="server" Style="left: 22px; position: absolute; top: 84px; z-index: 126;"
                    Width="35px" Enabled="False" meta:resourcekey="TxtTamanioPaqueteResource1">20</asp:TextBox>

                <asp:Label ID="LblKAM" runat="server" Height="21px" Style="z-index: 169; left: 300px; position: absolute; top: 64px"
                    Text="Keep Alive Multiplier:" Width="160px"
                    meta:resourcekey="LblKAMResource1"></asp:Label>
                <asp:TextBox ID="TxtKAM" runat="server" Style="z-index: 171; left: 300px; position: absolute; top: 84px"
                    Width="43px" Enabled="False"
                    meta:resourcekey="TxtKAMResource1">10</asp:TextBox>

                <asp:Label ID="Label23" runat="server" Style="z-index: 132; left: 22px; position: absolute; top: 114px" Visible="false"
                    Text="Modo Confirmación del PTT:" meta:resourcekey="Label23Resource1"></asp:Label>
                <asp:DropDownList ID="DListModoConfPTT" runat="server" Enabled="False" Style="z-index: 154; left: 22px; position: absolute; top: 134px"
                    Width="150px" meta:resourcekey="DListModoConfPTTResource1" class="select"  Visible="false">
                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource9">No Confirma</asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource10">Confirma a las sesiones RTP establecidas</asp:ListItem>
                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource11">Confirma s&#243;lo a la sesi&#243;n RTP de la que transmite el audio</asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="Label27" runat="server" Style="z-index: 159; left: 22px; position: absolute; top: 164px"
                    Text="Desactivación de SQ (paquetes):"  Visible="false"
                    meta:resourcekey="Label27Resource1"></asp:Label>
                <asp:TextBox ID="TxtDesactivacionSQ" runat="server" Style="z-index: 160; left: 22px; position: absolute; top: 184px"
                    Width="35px" Enabled="False"  Visible="false"
                    meta:resourcekey="TxtDesactivacionSQResource1"></asp:TextBox>
             

               


                <asp:CheckBox ID="CheckDiffServ" runat="server" Style="z-index: 115; left: 300px; position: absolute; top: 164px"
                    Text="Usar Protocolo DiffServ" Enabled="False" Visible="false"
                    meta:resourcekey="CheckDiffServResource1" />
                <asp:CheckBox ID="CheckSupresionSilencio" runat="server" Style="left: 300px; position: absolute; top: 194px; z-index: 106;"
                    Text="Uso supresión silencio" Enabled="False" Visible="false"
                    meta:resourcekey="CheckSupresionSilencioResource1" />

                <asp:Label ID="Label25" runat="server" Style="z-index: 155; left: 22px; position: absolute; top: 214px" Visible="false"
                    Text="Núm. Flujos de Audio:" meta:resourcekey="Label25Resource1"></asp:Label>
                <asp:DropDownList ID="DListFlujosAudio" runat="server" Enabled="False" Style="z-index: 156; left: 22px; position: absolute; top: 234px"
                    Width="51px" class="select" Visible="false"
                    meta:resourcekey="DListFlujosAudioResource1">
                    <asp:ListItem meta:resourcekey="ListItemResource12">1</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ListItemResource13">2</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ListItemResource14">3</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ListItemResource15">4</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ListItemResource16">5</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ListItemResource17">6</asp:ListItem>
                    <asp:ListItem meta:resourcekey="ListItemResource18">7</asp:ListItem>
                </asp:DropDownList>


                <asp:Label ID="Label30" runat="server" Style="z-index: 163; left: 22px; position: absolute; top: 264px" Visible="false"
                    Text="Timeout activación PTT (msg):" meta:resourcekey="Label30Resource1"></asp:Label>
                <asp:TextBox ID="TxtActivacionPTT" runat="server" Style="z-index: 124; left: 22px; position: absolute; top: 284px" Visible="false"
                    Width="46px" Enabled="False" meta:resourcekey="TxtActivacionPTTResource1"></asp:TextBox>

                <asp:Label ID="Label32" runat="server" Style="z-index: 118; left: 22px; position: absolute; top: 314px"
                    Text="Repetición estados SQ y BSS:" Visible="false"
                    meta:resourcekey="Label32Resource1"></asp:Label>
                <asp:TextBox ID="TxtRepSQyBSS" runat="server" Style="z-index: 119; left: 22px; position: absolute; top: 334px"
                    Width="43px" Enabled="False" Visible="false"
                    meta:resourcekey="TxtRepSQyBSSResource1"></asp:TextBox>

                <asp:Panel ID="PanelCodec" runat="server" GroupingText="  Codec preferido  " Height="99px"
                    Style="left: 300px; position: absolute; top: 237px; z-index: 102;" Width="162px" Visible="false"
                    ForeColor="RoyalBlue" meta:resourcekey="PanelCodecResource1">
                    <asp:RadioButton ID="RBCodecA" runat="server" Checked="True" GroupName="Codec" Text="G.711 ley A"
                        Enabled="False" Style="left: 31px; position: absolute; top: 25px; width:auto" 
                        ForeColor="Black" meta:resourcekey="RBCodecAResource1" />
                    <asp:RadioButton ID="RBCodecMu" runat="server" GroupName="Codec"
                        Text="G.711 ley µ" Enabled="False"
                        Style="left: 31px; position: absolute; top: 58px; width:auto" ForeColor="Black"
                        meta:resourcekey="RBCodecMuResource1" />
                    <br/><br/><br/>
                </asp:Panel>

                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="TxtRepSQyBSS"
                    ErrorMessage="El campo Repetición de estados SQ y BSS no puede estar vacio."
                    Style="z-index: 120; left: 199px; position: absolute; top: 290px"
                    meta:resourcekey="RequiredFieldValidator14Resource1">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator10" runat="server" ControlToValidate="TxtRepSQyBSS"
                    ErrorMessage="El campo Repetición de Estados SQ y BSS debe ser numérico." Operator="DataTypeCheck"
                    Style="z-index: 121; left: 199px; position: absolute; top: 290px" Type="Integer"
                    meta:resourcekey="CompareValidator10Resource1">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="TxtKAM"
                    ErrorMessage="Es necesario introducir un valor entre 2 y 50." Style="z-index: 174; left: 350px; position: absolute; top: 84px"
                    Width="4px"
                    meta:resourcekey="RequiredFieldValidator16Resource1">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="TxtKAM"
                    ErrorMessage="El valor debe estar comprendido entre 2 y 50" MaximumValue="50"
                    MinimumValue="2" Style="z-index: 175; left: 350px; position: absolute; top: 84px"
                    Type="Integer" meta:resourcekey="RangeValidator2Resource1">*</asp:RangeValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="TxtKAP"
                    ErrorMessage="Es necesario introducir un valor entre 20 y 1000." Style="z-index: 172; left: 350px; position: absolute; top: 34px"
                    meta:resourcekey="RequiredFieldValidator15Resource1">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="El valor debe estar comprendido entre 20 y 1000"
                    Height="20px" MaximumValue="1000" MinimumValue="20" Style="z-index: 173; left: 350px; position: absolute; top: 34px"
                    Width="12px" ControlToValidate="TxtKAP"
                    Type="Integer" meta:resourcekey="RangeValidator1Resource1">*</asp:RangeValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="TxtActivacionPTT"
                    ErrorMessage="El campo Timeout activación PTT no puede estar vacio." Style="z-index: 186; left: 299px; position: absolute; top: 249px"
                    meta:resourcekey="RequiredFieldValidator12Resource1">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator11" runat="server" ControlToValidate="TxtActivacionPTT"
                    ErrorMessage="El campo Timeout activación PTT debe ser numérico." Operator="DataTypeCheck"
                    Style="z-index: 197; left: 281px; position: absolute; top: 249px" Type="Integer"
                    meta:resourcekey="CompareValidator11Resource1">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="TxtDesactivacionSQ"
                    ErrorMessage="El campo Desactivación SQ no puede estar vacio." Style="z-index: 184; left: 299px; position: absolute; top: 175px"
                    meta:resourcekey="RequiredFieldValidator10Resource1">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator8" runat="server" ControlToValidate="TxtDesactivacionSQ"
                    ErrorMessage="El campo Desactivación de SQ debe ser numérico." Operator="DataTypeCheck"
                    Style="z-index: 194; left: 281px; position: absolute; top: 175px" Type="Integer"
                    meta:resourcekey="CompareValidator8Resource1">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtTamanioPaquete"
                    ErrorMessage="El campo Tamaño paquete RTP no puede estar vacio." Style="z-index: 179; left: 299px; position: absolute; top: 63px"
                    meta:resourcekey="RequiredFieldValidator3Resource1">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="TxtTamanioPaquete"
                    ErrorMessage="El campo Tamaño paquete RTP debe ser numérico." Operator="DataTypeCheck"
                    Style="z-index: 189; left: 281px; position: absolute; top: 63px" Type="Integer"
                    meta:resourcekey="CompareValidator1Resource1">*</asp:CompareValidator>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <asp:Panel ID="Panel32" runat="server" GroupingText="Tx" Style="z-index: 119; left: 15px; position: absolute; top: 15px; width:250px"
                    meta:resourcekey="Panel32Resource1" >
                    <br/><br/><br/><br/>
                    <asp:RadioButton ID="RBAGCTx" runat="server" GroupName="GananciaTx" Text="AGC" Style="left: 10px; top: 20px; position: absolute"
                        Enabled="False" ForeColor="Black" meta:resourcekey="RBAGCTxResource1"  OnCheckedChanged="TxParamAudio_Changed" AutoPostBack="True" />
                    <asp:RadioButton ID="RBGananciaTx" runat="server" GroupName="GananciaTx" Style="left: 10px; top: 40px; position: absolute"
                        Text="Ganancia" Checked="True" OnCheckedChanged="TxParamAudio_Changed"
                        Enabled="False" ForeColor="Black" meta:resourcekey="RBGananciaTxResource1" AutoPostBack="True" />
                    
                    <asp:Label ID="Label10" runat="server" Text="Nivel de salida (dBm)" Style="left: 15px; top: 65px; position: absolute"
                        ForeColor="Black" Width="160px" meta:resourcekey="Label10Resource1"></asp:Label>
                    <asp:TextBox ID="TxtGananciaTx" runat="server" Width="46px" Enabled="False" Style="left: 15px; top: 85px; position: absolute"
                        meta:resourcekey="TxtGananciaTxResource1">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
							  ControlToValidate="TxtGananciaTx" Style="left: 75px; top: 90px; position: absolute"
							  meta:resourcekey="RequiredFieldValidator5Resource1">*</asp:RequiredFieldValidator>
				    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="TxtGananciaTx"
					    Operator="DataTypeCheck" Type="Double" meta:resourcekey="CompareValidator3Resource1">*</asp:CompareValidator>
                    <asp:RangeValidator ID="RVTxtGananciaTx" runat="server" ControlToValidate="TxtGananciaTx"
					    ErrorMessage="El valor debe estar comprendido entre -13,4 y 1,2" MaximumValue="1,2"
					    MinimumValue="-13,4" Style="z-index: 207; left: 75px; position: absolute; top: 90px"
					    Type="Double" meta:resourcekey="RVTxtGananciaTxResource1">*</asp:RangeValidator>
                     <br />
                </asp:Panel>
                <asp:Panel ID="Panel33" runat="server" GroupingText="Rx"
                    Style="z-index: 100; left: 284px; width: 250px; position: absolute; top: 15px"
                    meta:resourcekey="Panel33Resource1">
                    <br/><br/><br/><br/>
                    <asp:RadioButton ID="RBAGCRx" runat="server" GroupName="GananciaRx" Text="AGC" Style="left: 10px; top: 20px; position: absolute"
                        Enabled="False" ForeColor="Black" meta:resourcekey="RBAGCRxResource1" OnCheckedChanged="RxParamAudio_Changed" AutoPostBack="True" />
                    <asp:RadioButton ID="RBGananciaRx" runat="server" GroupName="GananciaRx"
                        Text="Ganancia" Checked="True" Style="left: 10px; top: 40px; position: absolute"
                        Enabled="False" ForeColor="Black" meta:resourcekey="RBGananciaRxResource1" OnCheckedChanged="RxParamAudio_Changed" AutoPostBack="True" />

                    <asp:Label ID="Label3" runat="server" Text="Nivel de salida (dBm)" Style="left: 15px; top: 65px; position: absolute"
                        Width="155px" ForeColor="Black" meta:resourcekey="Label3Resource1"></asp:Label>
                    <asp:TextBox ID="TxtGananciaRx" runat="server" Width="48px" Enabled="False" Style="left: 15px; top: 85px; position: absolute"
                        meta:resourcekey="TxtGananciaRxResource1">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
							     ControlToValidate="TxtGananciaRx" Style="left: 75px; top: 90px; position: absolute"
							     meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
					<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="TxtGananciaRx"
							    Operator="DataTypeCheck" Type="Double" Width="17px" 
							     meta:resourcekey="CompareValidator2Resource1">*</asp:CompareValidator>
                    <asp:RangeValidator ID="RVTxtGananciaRx" runat="server" ControlToValidate="TxtGananciaRx"
					    ErrorMessage="El valor debe estar comprendido entre -24,3 y 1,10" MaximumValue="1,10"
					    MinimumValue="-24,3" Style="z-index: 207; left: 75px; position: absolute; top: 90px"
					    Type="Double" meta:resourcekey="RVTxtGananciaRxResource1">*</asp:RangeValidator>
                    <br />
                </asp:Panel>
            </asp:View>
            <asp:View ID="View4" runat="server">
                 <asp:Panel ID="Panel8" runat="server" GroupingText="Generales" Style="z-index: 102; left: 12px; position: absolute; top: 5px; height: 60px;"
                    Width="310px" ForeColor="RoyalBlue" Visible="false"
                    meta:resourcekey="Panel8Resource1">


<%--  
                      <table runat="server" ID="TblRecorders" style="position:absolute; top:25px;left:165px" visible="False">
                    <tr>
                        <td>
                            <asp:Label ID="Label43" runat="server" Style="z-index: 159"
                                Text="Grabador1:"  Visible="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="DDLRecorder1" runat="server" Visible="true" Style="z-index: 154;"
                                Width="120px"  AppendDataBoundItems="True"  class="select" Enabled="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label41" runat="server" Style="z-index: 159"
                                Text="Grabador2:"  Visible="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="DDLRecorder2" runat="server" Visible="true" Style="z-index: 154;"
                                Width="120px" AppendDataBoundItems="True"  class="select" Enabled="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>                   --%>
                     <br /><br /> <br /> <br /><br />
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="BSS" Style="z-index: 102; left: 325px; position: absolute; top: 5px; height: 385px;"
                    Width="285px" ForeColor="RoyalBlue" Visible="false"
                    meta:resourcekey="Panel3Resource1">
    
                  
                
                        <asp:CheckBox ID="CheckBSS" runat="server" Enabled="False" Text="BSS" Style="z-index: auto; left: 12px; position: absolute; top: 20px"
                         Width="52px" ForeColor="Black" OnCheckedChanged="Checked_BSS" meta:resourcekey="CheckBSSResource1" AutoPostBack="True" />    

                        <asp:Label ID="Label24" runat="server" Text="Método BSS Preferido:" ForeColor="Black" Style="z-index: auto; left: 12px; position: absolute; top: 45px"
                            meta:resourcekey="Label24Resource1"></asp:Label>
                    
                        <asp:DropDownList ID="DListMetodoBSS" runat="server" Enabled="False" Style="z-index: auto; left: 12px; position: absolute; top: 65px; "
                             AppendDataBoundItems="True" AutoPostBack="True" meta:resourcekey="DListMetodoBSSResource1" class="select" OnSelectedIndexChanged="DListMetodoBSS_SelectedIndexChanged">
                        </asp:DropDownList>


                        <asp:Label ID="LabelIDTablaCalidad" runat="server" Text="Tabla de Conversión a Índices RSSI:" ForeColor="Black" Style="z-index: auto; left: 12px; position: absolute; top: 110px"
                        meta:resourcekey="LabelIDTablaCalidadResource"></asp:Label>
                   
                       <asp:DropDownList ID="DListTablasCalidad" runat="server" Enabled="False" Style="left: 12px; position: absolute; top: 130px; width: 130px;"  class="select" 
                            OnSelectedIndexChanged="DListTablasCalidad_SelectedIndexChanged" AutoPostBack="True"                        
                            AppendDataBoundItems="True" meta:resourcekey="DListTablaResource1">                        
                        <asp:ListItem Value="0" meta:resourcekey="ListTablasResource">&lt;Selecciona Tabla&gt;</asp:ListItem>
                        </asp:DropDownList>

<%--                     <asp:TextBox ID="TextIDTablaCalidad" runat="server" Width="40px" Enabled="False" Style="left: 12px; top: 190px; position: absolute" 
                        meta:resourcekey="TxtIDtablaCalidadResource1" Visible="False"></asp:TextBox>--%>



                    


<%--                      <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="TextBoxCLD"
                        ErrorMessage="El campo Tiempo CLD debe estar comprendido entre 1 y 60 seg"
                        meta:resourcekey="RequiredFieldValidator18Resource1">*</asp:RequiredFieldValidator>
                      <asp:CompareValidator ID="CompareValidator15" runat="server" ControlToValidate="TextBoxCLD"
                        ErrorMessage="El campo GRS Delay debe ser numérico." Operator="DataTypeCheck"
                        Style="z-index: 207; left: 90px; position: absolute; top: 105px"
                        Type="Integer"  meta:resourcekey="CompareValidator15Resource1">*</asp:CompareValidator> 
                     <asp:RangeValidator ID="RangeTextBoxCLD" runat="server" ControlToValidate="TextBoxCLD"
					    ErrorMessage="El valor debe estar comprendido entre 1 y 60" MaximumValue="60"
					    MinimumValue="1" Style="z-index: 207; left: 75px; position: absolute; top: 105px"
					    Type="Double" meta:resourcekey="RVTxtCLDResource1">*</asp:RangeValidator>--%>
                    
                                         
                                       
                        <%--<asp:ListItem Value="0" meta:resourcekey="ListItemResource19">Ninguno</asp:ListItem>--%>
                      <%--   <asp:ListItem Value="1" meta:resourcekey="ListItemResource20">NUCLEO</asp:ListItem>--%>
                       <%--  <asp:ListItem Value="2" meta:resourcekey="ListItemResource21">C/N</asp:ListItem>--%>
                       <%--  <asp:ListItem Value="3" meta:resourcekey="ListItemResource22">PSD estandarizado</asp:ListItem>--%>
                    

                   


                    <asp:Table ID="TableCalidad" runat="server" Height="92px" Style="left: 15px; position: absolute; top:165px; z-index: 102;"
                    Width="40px" SkinID="TableSkin" Enabled="False" Visible="false" GridLines="Both" >
                    <asp:TableRow ID="TableVQTBSS" runat="server" SkinID="TableHeaderRow">
                        <asp:TableCell ID="TableVQTHeader1" runat="server" meta:resourcekey="VQTHead1Resource1"> NUCLEO </asp:TableCell>
                        <asp:TableCell ID="TableVQTHeader2" runat="server" meta:resourcekey="VQTHead2Resource1"> RSSI </asp:TableCell>                       
                    </asp:TableRow>
                    <asp:TableRow ID="TableVQTRow1" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                        <asp:TableCell ID="TableVQTRow1_1" runat="server" Width="25px" HorizontalAlign="Center" Text="0">                            
                        </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow1_2" runat="server" Width="25px">                           
                            <asp:Label ID="Label1_2" runat="server" Style="left:5px;" >
                             </asp:Label>
                        </asp:TableCell>                    
                    </asp:TableRow>
                    <asp:TableRow ID="TableVQTRow2" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                        <asp:TableCell ID="TableVQTRow2_1" runat="server" Width="25px" ForeColor="#000001" HorizontalAlign="Center" Text="10">
                        </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow2_2" runat="server" Width="25px">                           
                            <asp:Label ID="Label2_2" runat="server" Style="left:5px;" >
                             </asp:Label>
                        </asp:TableCell>    
                      </asp:TableRow>                               
                    <asp:TableRow ID="TableVQTRow3" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                        <asp:TableCell ID="TableVQTRow3_1" runat="server" Width="25px" ForeColor="#000001" HorizontalAlign="Center" Text="20">

                        </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow3_2" runat="server" Width="25px">                           
                            <asp:Label ID="Label3_2" runat="server" Style="left:5px;" >
                             </asp:Label>
                        </asp:TableCell>                    
                    </asp:TableRow> 
                    <asp:TableRow ID="TableVQTRow4" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                             <asp:TableCell ID="TableVQTRow4_1" runat="server" Width="25px" ForeColor="#000001" HorizontalAlign="Center" Text="30">
                        </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow4_2" runat="server" Width="25px">                           
                            <asp:Label ID="Label4_2" runat="server" Style="left:5px;" >
                             </asp:Label>
                        </asp:TableCell>                    
                    </asp:TableRow> 
                    <asp:TableRow ID="TableVQTRow5" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                        <asp:TableCell ID="TableVQTRow5_1" runat="server" Width="25px" ForeColor="#000001" HorizontalAlign="Center" Text="40">
                        </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow5_2" runat="server" Width="25px">                           
                            <asp:Label ID="Label5_2" runat="server" Style="left:5px;" >
                             </asp:Label>
                        </asp:TableCell>                    
                    </asp:TableRow> 
                    <asp:TableRow ID="TableVQTRow6" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                        <asp:TableCell ID="TableVQTRow6_1" runat="server" Width="25px" ForeColor="#000001" HorizontalAlign="Center" Text="50">
                        </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow6_2" runat="server" Width="25px">                           
                            <asp:Label ID="Label6_2" runat="server" Style="left:5px;" >
                             </asp:Label>
                        </asp:TableCell>                    
                    </asp:TableRow>                              
                </asp:Table>
                   <br /> <br /> <br /><br /><br /> <br /> <br /><br /><br /> <br /> <br /><br /> <br /> <br /><br /><br /> <br /> <br /><br /> <br />
                </asp:Panel>
                <asp:Panel ID="Panel4" runat="server" GroupingText="NTZ" Style="z-index: 104; left: 300px; position: absolute; top: 8px"
                    Width="280px" ForeColor="RoyalBlue" meta:resourcekey="Panel4Resource1" Visible="False" Height="80">
                    <asp:CheckBox ID="CheckNTZ" runat="server" Enabled="False" Text="NTZ" Style="z-index: auto; left: 12px; position: absolute; top: 18px"
                        ForeColor="Black" meta:resourcekey="CheckNTZResource1" />
                    <br /><br /><br /><br/>
                    <asp:Label ID="Label21" runat="server" Text="Tipo NTZ:" ForeColor="Black" Style="z-index: auto; left: 12px; position: absolute; top: 50px"
                        meta:resourcekey="Label21Resource1"></asp:Label>
                    <asp:DropDownList ID="DListTipoNTZ" runat="server" Enabled="False" Width="163px" Style="z-index: auto; left: 12px; position: absolute; top: 70px"
                        meta:resourcekey="DListTipoNTZResource1" class="select">
                        <asp:ListItem Value="0" meta:resourcekey="ListItemResource23">Cierre de Contactos</asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="ListItemResource24">Se&#241;al a 0V</asp:ListItem>
                        <asp:ListItem Value="2" meta:resourcekey="ListItemResource25">Se&#241;al a 5V</asp:ListItem>
                    </asp:DropDownList>
                </asp:Panel>
                <asp:Panel ID="Panel5" runat="server" GroupingText="Transmisión" Style="z-index: 102; left: 12px; position: absolute; top: 70px; height: 60px;"
                    Width="310px" ForeColor="RoyalBlue" meta:resourcekey="Panel5Resource1" ScrollBars="None">
                    <asp:Label ID="Label16" runat="server" Text="PTT:" ForeColor="Black" Style="left:15px; top:25px; position:absolute"
                        meta:resourcekey="Label16Resource1"></asp:Label>
                    <asp:DropDownList ID="DListPTT" runat="server" Enabled="False" Width="60px"
                        Style="left: 15px; top: 45px; position: absolute;" class="select"
                        meta:resourcekey="DListPTTResource1">
                        <asp:ListItem Value="h" meta:resourcekey="ListItemResource26">hw</asp:ListItem>
                        <%-- <asp:ListItem Value="s" meta:resourcekey="ListItemResource27">sw</asp:ListItem>
                        <asp:ListItem Value="m" meta:resourcekey="ListItemResource28">hw+sw</asp:ListItem> --%>
                    </asp:DropDownList>
                    <asp:Label ID="Label26" runat="server" Text="Tiempo Máximo en PTT (s)" Style="left:100px; top:25px; position:absolute; width:auto"
                        ForeColor="Black" meta:resourcekey="Label26Resource1"></asp:Label>
                    <asp:TextBox ID="TxtTiempoPTT" runat="server" Width="43px" Enabled="False" Style="top: 45px; left: 100px; position: absolute;"
                        meta:resourcekey="TxtTiempoPTTResource1"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="TxtTiempoPTT"
                        ErrorMessage="El campo Tiempo Máximo en PTT no puede estar vacio." Style="left: 125px;top:25px; position: relative"
                        meta:resourcekey="RequiredFieldValidator11Resource1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator9" runat="server" ControlToValidate="TxtTiempoPTT"
                        ErrorMessage="El campo Tiempo Máximo en PTT debe ser numérico." Operator="DataTypeCheck" Style="left: 125px;top:25px; position: relative"
                        Type="Integer" meta:resourcekey="CompareValidator9Resource1">*</asp:CompareValidator><br />

                    <asp:Label ID="Label20" runat="server" Text="Umbral tono PTT:" ForeColor="Black" Style="left:340px; top:20px; position:absolute"
                        meta:resourcekey="Label20Resource1" Visible="false"></asp:Label>
                    <asp:TextBox ID="TxtUmbralPTT" runat="server" Width="43px" Enabled="False" Style="left: 340px; top: 40px; position: absolute;"
                        meta:resourcekey="TxtUmbralPTTResource1" Visible="False"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="TxtUmbralPTT"
                        ErrorMessage="El campo Umbral tono PTT no puede estar vacio."
                        Style="left: 92px; position: relative"
                        meta:resourcekey="RequiredFieldValidator9Resource1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="TxtUmbralPTT"
                        ErrorMessage="El campo Umbral tono PTT debe ser numérico." Operator="DataTypeCheck"
                        Type="Integer" Style="left: 89px; position: relative"
                        meta:resourcekey="CompareValidator7Resource1">*</asp:CompareValidator>

                    <asp:Label ID="LabelGRS" runat="server" Text="GRS Delay (ms):" ForeColor="Black" Style="z-index: auto; left: 15px; position: absolute; top: 70px"
                        meta:resourcekey="Label34Resource1"></asp:Label>
                   
                       <asp:TextBox ID="TxtGRSDelay" runat="server" Width="40px" Enabled="False" Style="left: 15px; top: 90px; position: absolute" 
                        meta:resourcekey="TxtGRSDelayResource1"></asp:TextBox>
                     
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="TxtGRSDelay"
                        ErrorMessage="El campo GRS Delay  no puede estar vacio y debe tomar un valor entre 0 y 500 ms." Style=" left: 60px; position: absolute; top: 95px"
                        meta:resourcekey="RequiredFieldValidator17Resource1">*</asp:RequiredFieldValidator>

                     <asp:CompareValidator ID="CompareValidator14" runat="server" ControlToValidate="TxtGRSDelay"
                        ErrorMessage="El campo GRS Delay debe ser numérico." Operator="DataTypeCheck"
                          Style="z-index: 207; left: 60px; position: absolute; top: 95px"
                        Type="Integer" meta:resourcekey="CompareValidator14Resource1">*</asp:CompareValidator>

                    <asp:RangeValidator ID="RVTxtGRSDelay" runat="server" ControlToValidate="TxtGRSDelay"
					    ErrorMessage="El valor debe estar comprendido entre 0 y 500 ms" MaximumValue="500"
					    MinimumValue="-0" Style="z-index: 207; left: 60px; position: absolute; top: 95px"
					    Type="Double" meta:resourcekey="RVTxtGRSDelayResource1">*</asp:RangeValidator>

                    <br /><br /> <br /> <br /><br />
                </asp:Panel>
                <asp:Panel ID="Panel6" runat="server" GroupingText="Recepción" Height="60px" Style="z-index: 102; left: 12px; position: absolute; top: 257px"
                    Width="310px" ForeColor="RoyalBlue"
                    meta:resourcekey="Panel6Resource1">
                    <asp:Label ID="Label15" runat="server" Text="SQ:" ForeColor="Black" Style="left:15px; top:25px; position:absolute"
                        meta:resourcekey="Label15Resource1"></asp:Label>
                    <asp:DropDownList ID="DListSQ" runat="server" Enabled="False" Width="60px" class="select"
                        Style="left: 15px; top: 45px; position: absolute;" meta:resourcekey="DListSQResource1" OnSelectedIndexChanged="DListSQ_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="h" meta:resourcekey="ListItemResource29">hw</asp:ListItem>
                       <%--  <asp:ListItem Value="s" meta:resourcekey="ListItemResource30">sw</asp:ListItem> --%>
                        <asp:ListItem Value="v" meta:resourcekey="ListItemResource31">VAD</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label33" runat="server" Text="Umbral VAD:" ForeColor="Black" Style="left:100px; top:25px; position:absolute"
                        meta:resourcekey="Label33Resource1"></asp:Label>
                    <asp:TextBox ID="TxtUmbralVAD" runat="server" Width="43px" Enabled="False"
                        Style="top: 45px; left: 100px; position: absolute;" meta:resourcekey="TxtUmbralVADResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="TxtUmbralVAD"
                        ErrorMessage="El campo Umbral VAD no puede estar vacío, y debe estar comprendido entre -35 y -15 dBm"
                        Style="left: 150px; ;top:25px;position: relative"
                        meta:resourcekey="RequiredFieldValidator13Resource1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator12" runat="server" ControlToValidate="TxtUmbralVAD"
                        ErrorMessage="El campo Umbral VAD debe ser numérico." Operator="DataTypeCheck"
                        Type="Integer" Style="left: 150px; top:25px; position: relative"
                        meta:resourcekey="CompareValidator12Resource1">*</asp:CompareValidator><br />
                    <asp:RangeValidator ID="RVTxtUmbralVAD" runat="server" ControlToValidate="TxtUmbralVAD"
                        ErrorMessage="El umbral VAD debe estar comprendido entre -35 y -15 dBm" 
                        MinimumValue="-35" MaximumValue="-15" Style="left: 150px; top:7px; position: relative"
                        Type="Integer" meta:resourcekey="RVTxtUmbralVADResource1">*</asp:RangeValidator>

                    <asp:Label ID="Label19" runat="server" Text="Umbral tono SQ:" Style="left:340px; top:20px; position:absolute; width:auto"
                        ForeColor="Black" meta:resourcekey="Label19Resource1" Visible="False"></asp:Label>
                    <asp:TextBox ID="TxtUmbralSQ" runat="server" Width="43px" Enabled="False"
                        Style="left: 346px; top: 40px; position: absolute;" meta:resourcekey="TxtUmbralSQResource1" Visible="False"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TxtUmbralSQ"
                        ErrorMessage="El campo Umbral tono SQ no puede estar vacio."
                        Style="left: 4px; position: relative"
                        meta:resourcekey="RequiredFieldValidator8Resource1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="TxtUmbralSQ"
                        ErrorMessage="El campo Umbral tono SQ debe ser numérico." Operator="DataTypeCheck"
                        Type="Integer" Style="left: 2px; position: relative"
                        meta:resourcekey="CompareValidator6Resource1">*</asp:CompareValidator>

                     <asp:CheckBox ID="CheckGrabacionEd137" runat="server" Enabled="False" Text="Grabación ED137" Style="z-index: auto; left: 15px; position: absolute; top: 85px"
                        Width="160px" ForeColor="Black" meta:resourcekey="CheckGrabacionEd137Resource1" />                        
                    <br /><br /> <br /> <br /><br />
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" GroupingText="EM" Style="z-index: 103; left: 12px; position: absolute; top: 295px"
                    Width="280px" ForeColor="RoyalBlue" meta:resourcekey="Panel2Resource1" Visible="False">
                    <asp:CheckBox ID="CheckEM" runat="server" Enabled="False" Text="EM" Style="left: 15px; top: 25px; position: absolute" ForeColor="Black" meta:resourcekey="CheckEMResource1" />
                    <asp:Label ID="Label17" runat="server" Text="Umbral tono E:" ForeColor="Black" Style="left: 15px; top: 55px; width: 120px; position: absolute; height: 17px;" meta:resourcekey="Label17Resource1"> </asp:Label>
                    <asp:TextBox ID="TxtUmbralE" runat="server" Enabled="False" Style="left: 16px; top: 78px; position: absolute; width: 56px; height: 22px;" meta:resourcekey="TxtUmbralEResource1"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TxtUmbralE"
                        ErrorMessage="El campo Umbral tono E no puede estar vacio."
                        meta:resourcekey="RequiredFieldValidator6Resource1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="TxtUmbralE"
                        ErrorMessage="El campo Umbral tono E debe ser numérico." Operator="DataTypeCheck"
                        Type="Integer" meta:resourcekey="CompareValidator4Resource1">*</asp:CompareValidator><br />
                    <br />
                    <asp:Label ID="Label18" runat="server" Text="Umbral tono M:" ForeColor="Black" Style="left: 160px; top: 55px; width: 120px; position: absolute" 
                        meta:resourcekey="Label18Resource1"></asp:Label>
                    <asp:TextBox ID="TxtUmbralM" runat="server" Width="40px" Enabled="False" Style="left: 160px; top: 75px; position: absolute" 
                        meta:resourcekey="TxtUmbralMResource1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="TxtUmbralM"
                        ErrorMessage="El campo Umbral tono M no puede estar vacio."
                        meta:resourcekey="RequiredFieldValidator7Resource1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="TxtUmbralM"
                        ErrorMessage="El campo Umbral tono M debe ser numérico." Operator="DataTypeCheck"
                        Type="Integer" meta:resourcekey="CompareValidator5Resource1">*</asp:CompareValidator>
                    <br /><br/>
                </asp:Panel>
                <asp:Panel ID="Panel7" runat="server" GroupingText="Supervisión" Height="128px" Style="z-index: auto; left: 300px; position: absolute; top: 295px"
                    Width="280px" ForeColor="RoyalBlue" meta:resourcekey="GroupSupervision" Visible="False">
                    <asp:CheckBox ID="CheckTX" runat="server" Enabled="False" Style="z-index: auto; left: 17px; position: absolute; top: 22px"
                        Text="Supervisión portadora TX" ForeColor="Black"
                        Width="220px" meta:resourcekey="CheckTXResource1" Visible="False" />
                    <asp:CheckBox ID="CheckRX" runat="server" Enabled="False" Style="z-index: auto; left: 15px; position: absolute; top: 50px"
                        Text="Supervisión moduladora TX" ForeColor="Black"
                        Width="220px" meta:resourcekey="CheckRXResource1" />
                    <br /><br /><br/>
                 </asp:Panel>
                <asp:CheckBox ID="CheckCifrado" runat="server" Enabled="False" Text="Cifrado"
                    TextAlign="Left"
                    Style="left: 515px; position: absolute; top: 400px; width:auto"
                    meta:resourcekey="CheckCifradoResource1" Visible="False" />

                <asp:CheckBox ID="CheckBoxEventosPTT_SQ" runat="server" Visible="false" Enabled="false" Text="Eventos/Históricos PTT-SQ" Style="z-index: auto; left: 18px; position: absolute; top: 30px"
                        Width="250px" ForeColor="Black" meta:resourcekey="CheckEventosGrabacionResource1" /> 
            </asp:View>
            <asp:View ID="View5" runat="server">
                <asp:Table ID="TTifx" runat="server" Height="112px" Style="left: 46px; position: absolute; top: 96px; z-index: 102;"
                    Width="400px" BorderStyle="Double" GridLines="Both"
                    BorderColor="Black" SkinID="TableSkin">
                    <asp:TableRow ID="TableRow1" runat="server" BorderWidth="3px" BorderStyle="Solid" SkinID="TableHeaderRow">
                        <asp:TableCell ID="CellHead1" runat="server" meta:resourcekey="CellHead1Resource1">Slot 0</asp:TableCell>
                        <asp:TableCell ID="CellHead2" runat="server" meta:resourcekey="CellHead2Resource1">Slot 1</asp:TableCell>
                        <asp:TableCell ID="CellHead3" runat="server" meta:resourcekey="CellHead3Resource1">Slot 2</asp:TableCell>
                        <asp:TableCell ID="CellHead4" runat="server" meta:resourcekey="CellHead4Resource1">Slot 3</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow2" runat="server" SkinID="TableRow">
                        <asp:TableCell ID="TableCell1" runat="server" Width="100px">
                            <asp:TextBox ID="TextBox11" runat="server" Width="100px" class=""></asp:TextBox>
                            <asp:CheckBox ID="CheckBox11" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot" Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell2" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox12" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox12" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell3" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox13" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox13" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell4" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox14" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox14" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow3" runat="server" SkinID="TableRow">
                        <asp:TableCell ID="TableCell5" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox21" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox21" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell6" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox22" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox22" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell7" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox23" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox23" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell8" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox24" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox24" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow4" runat="server" SkinID="TableRow">
                        <asp:TableCell ID="TableCell9" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox31" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox31" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell10" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox32" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox32" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell11" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox33" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox33" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell12" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox34" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox34" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow5" runat="server" SkinID="TableRow">
                        <asp:TableCell ID="TableCell13" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox41" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox41" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell14" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox42" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox42" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell15" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox43" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox43" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell16" runat="server" Width="100px">
                            <asp:TextBox
                                ID="TextBox44" runat="server" Width="100px"></asp:TextBox>
                            <asp:CheckBox ID="CheckBox44" runat="server" AutoPostBack="True" OnCheckedChanged="Checked_CBSlot"
                                Enabled="False" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Label ID="Label14" runat="server" Style="left: 210px; position: absolute; top: 25px; z-index: 137;"
                    Text="Pasarela" meta:resourcekey="ListItemResource32"></asp:Label>
                <asp:DropDownList ID="DLTifx" runat="server" Style="left: 210px; position: absolute; top: 45px; z-index: 119;"
                    Width="180px" AutoPostBack="True" class="select"
                    OnSelectedIndexChanged="DDLTifx_SelectedIndexChanged" Enabled="False"
                    meta:resourcekey="DLTifxResource1">
                </asp:DropDownList>
                <asp:RadioButtonList ID="RBLTipoEquipo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RBLTipoEquipo_SelectedIndexChanged"
                    Style="z-index: 199; left: 23px; position: absolute; top: 17px;  height: 63px" Enabled="False"
                    BorderStyle="Groove" meta:resourcekey="RBLTipoEquipoResource1">
                    <asp:ListItem Selected="True" Value="0" meta:resourcekey="ListItemResource32">Pasarela</asp:ListItem>
                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource33">Equipo Externo</asp:ListItem>
                </asp:RadioButtonList>
                <asp:Label ID="LblEquipoExterno" runat="server" Style="z-index: 200; left: 210px; position: absolute; top: 25px"
                    Text="Equipo Externo"
                    meta:resourcekey="LblEquipoExternoResource1"></asp:Label>
                <asp:DropDownList ID="DDLEquipoExternos" runat="server" Style="z-index: 201; left: 210px; position: absolute; top: 45px; height:auto"
                    Width="180px" AppendDataBoundItems="True" class="select"
                    Enabled="False" AutoPostBack="True"
                    OnSelectedIndexChanged="DDLEquipoExternos_SelectedIndexChanged"
                    meta:resourcekey="DDLEquipoExternosResource1">
                </asp:DropDownList>

            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <asp:Button ID="IBGenerales" runat="server" SkinId="ButtonTab"
		OnClick="OnButton_Click" Style="z-index: 110; left: 276px; position: absolute;
		top: 41px" meta:resourcekey="IBGeneralesResource1" />
	<asp:Button ID="IBVoip" runat="server"  SkinId="ButtonTab"
		OnClick="OnButton_Click" Style="z-index: 111; left: 380px; position: absolute;
		top: 41px" meta:resourcekey="IBVoipResource1" />
	<asp:Button ID="IBAudio" runat="server"  SkinId="ButtonTab"
		OnClick="OnButton_Click" Style="z-index: 112; left: 484px; position: absolute;
		top: 41px" meta:resourcekey="IBAudioResource1" />
    <asp:Button ID="IBFuncionalidad" runat="server"  SkinId="ButtonTab"
		OnClick="OnButton_Click" Style="z-index: 113; left: 588px; position: absolute;
		top: 41px" meta:resourcekey="IBFuncionalidadResource1" />
	<asp:Button ID="IBAsignacion" runat="server"  SkinId="ButtonTab"
		OnClick="OnButton_Click" Style="z-index: 114; left: 692px; position: absolute;
		top: 41px" meta:resourcekey="IBAsignacionResource1" />
</asp:Content>

