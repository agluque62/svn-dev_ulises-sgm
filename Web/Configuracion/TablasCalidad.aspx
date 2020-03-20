<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="TablasCalidad.aspx.cs" StylesheetTheme="TemaPaginasConfiguracion"
	CodeFileBaseClass="PageBaseCD40.PageCD40" Inherits="TablasCalidad" Title="Gestión Tablas de Conversión a Índices RSSI" EnableEventValidation="false" meta:resourcekey="PageResource1"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
			function AbreVentana(ventana) {
			window.open(ventana);
		}
		
	</script>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumén de errores:"
		 Style="z-index: 113; left: 254px; position: absolute; top: 444px; right: 369px;" 
		 Visible="False" meta:resourcekey="ValidationSummary1Resource1" />

    <asp:Label ID="Label4" runat="server" Text="GESTIÓN TABLAS DE CONVERSIÓN A ÍNDICES RSSI"
            CssClass="labelPagina" meta:resourcekey="Label4Resource1"></asp:Label>

    <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"
        SkinID="MascaraListaElementos" meta:resourcekey="ListBox1Resource1">
    </asp:ListBox>

    <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" Height="400px" Style="z-index: 105; left: 238px; position: absolute; top: 60px"
        Width="410px" Enabled="False"
        meta:resourcekey="Panel1Resource1">


               <asp:Table ID="TableCalidad" runat="server" Height="112px" Style="left: 270px; position: absolute; top:54px; z-index: 102;"
                    Width="60px" SkinID="TableSkin" TabIndex="1" Visible="False">
                    <asp:TableRow ID="TableVQTBSS" runat="server" SkinID="TableHeaderRow" >
                        <asp:TableCell ID="TableVQTHeader1" runat="server" meta:resourcekey="VQTHead1Resource1"> NUCLEO </asp:TableCell>
                        <asp:TableCell ID="TableVQTHeader2" runat="server" meta:resourcekey="VQTHead2Resource1"> RSSI </asp:TableCell>                       
                    </asp:TableRow>
                    <asp:TableRow ID="TableVQTRow1" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                        <asp:TableCell ID="TableVQTRow1_1" runat="server" Width="25px" HorizontalAlign="Center" Text="0">                           
                        </asp:TableCell>
                       
                        <asp:TableCell ID="TableVQTRow1_2" runat="server" Width="25px">                           
                            <asp:DropDownList ID="DropDownListVQTRow1_2" runat="server" Style="left:5px;" Enabled="True" 
                            meta:resourcekey="DListTipoResource1" class="select">
                             <asp:ListItem Value="0" >0</asp:ListItem>
                             <asp:ListItem Value="1" >1</asp:ListItem>
                             <asp:ListItem Value="2" >2</asp:ListItem>
                             <asp:ListItem Value="3" >3</asp:ListItem>
                             <asp:ListItem Value="4" >4</asp:ListItem>
                             <asp:ListItem Value="5" >5</asp:ListItem>
                             <asp:ListItem Value="6" >6</asp:ListItem>
                             <asp:ListItem Value="7" >7</asp:ListItem>
                             <asp:ListItem Value="8" >8</asp:ListItem>
                             <asp:ListItem Value="9" >9</asp:ListItem>
                             <asp:ListItem Value="10" >10</asp:ListItem>
                             <asp:ListItem Value="11" >11</asp:ListItem>
                             <asp:ListItem Value="12" >12</asp:ListItem>
                             <asp:ListItem Value="13" >13</asp:ListItem>
                             <asp:ListItem Value="14" >14</asp:ListItem>
                             <asp:ListItem Value="15" >15</asp:ListItem>
                           </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableVQTRow2" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                        <asp:TableCell ID="TableVQTRow2_1" runat="server" Width="25px" HorizontalAlign="Center" Text="10">                                                   
                        </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow2_2" runat="server" Width="25px">                           
                            <asp:DropDownList ID="DropDownListVQTRow2_2" runat="server" Enabled="True"
                            meta:resourcekey="DListTipoResource1" class="select">
                          <asp:ListItem Value="0" >0</asp:ListItem>
                             <asp:ListItem Value="1" >1</asp:ListItem>
                             <asp:ListItem Value="2" >2</asp:ListItem>
                             <asp:ListItem Value="3" >3</asp:ListItem>
                             <asp:ListItem Value="4" >4</asp:ListItem>                             
                             <asp:ListItem Value="5" >5</asp:ListItem>
                             <asp:ListItem Value="6" >6</asp:ListItem>
                             <asp:ListItem Value="7" >7</asp:ListItem>
                             <asp:ListItem Value="8" >8</asp:ListItem>
                             <asp:ListItem Value="9" >9</asp:ListItem>     
                             <asp:ListItem Value="10" >10</asp:ListItem>
                             <asp:ListItem Value="11" >11</asp:ListItem>
                             <asp:ListItem Value="12" >12</asp:ListItem>
                             <asp:ListItem Value="13" >13</asp:ListItem>
                             <asp:ListItem Value="14" >14</asp:ListItem>     
                             <asp:ListItem Value="15" >15</asp:ListItem>                             
                            </asp:DropDownList>
                        </asp:TableCell>    
                      </asp:TableRow>                               
                    <asp:TableRow ID="TableVQTRow3" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                        <asp:TableCell ID="TableVQTRow3_1" runat="server" Width="25px" HorizontalAlign="Center" Text="20">                        
                        </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow3_2" runat="server" Width="25px">                           
                            <asp:DropDownList ID="DropDownListVQTRow3_2" runat="server" Enabled="True" 
                            meta:resourcekey="DListTipoResource1" class="select">
                           <asp:ListItem Value="0" >0</asp:ListItem>
                             <asp:ListItem Value="1" >1</asp:ListItem>
                             <asp:ListItem Value="2" >2</asp:ListItem>
                             <asp:ListItem Value="3" >3</asp:ListItem>
                             <asp:ListItem Value="4" >4</asp:ListItem>                             
                             <asp:ListItem Value="5" >5</asp:ListItem>
                             <asp:ListItem Value="6" >6</asp:ListItem>
                             <asp:ListItem Value="7" >7</asp:ListItem>
                             <asp:ListItem Value="8" >8</asp:ListItem>
                             <asp:ListItem Value="9" >9</asp:ListItem>     
                             <asp:ListItem Value="10" >10</asp:ListItem>
                             <asp:ListItem Value="11" >11</asp:ListItem>
                             <asp:ListItem Value="12" >12</asp:ListItem>
                             <asp:ListItem Value="13" >13</asp:ListItem>
                             <asp:ListItem Value="14" >14</asp:ListItem>     
                             <asp:ListItem Value="15" >15</asp:ListItem>                         
                            </asp:DropDownList>
                        </asp:TableCell>                    
                    </asp:TableRow> 
                    <asp:TableRow ID="TableVQTRow4" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                             <asp:TableCell ID="TableVQTRow4_1" runat="server" Width="25px" HorizontalAlign="Center" Text="30">                                 
                             </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow4_2" runat="server" Width="25px">                           
                            <asp:DropDownList ID="DropDownListVQTRow4_2" runat="server" Enabled="True" 
                            meta:resourcekey="DListTipoResource1" class="select">
                            <asp:ListItem Value="0" >0</asp:ListItem>
                             <asp:ListItem Value="1" >1</asp:ListItem>
                             <asp:ListItem Value="2" >2</asp:ListItem>
                             <asp:ListItem Value="3" >3</asp:ListItem>
                             <asp:ListItem Value="4" >4</asp:ListItem>                             
                             <asp:ListItem Value="5" >5</asp:ListItem>
                             <asp:ListItem Value="6" >6</asp:ListItem>
                             <asp:ListItem Value="7" >7</asp:ListItem>
                             <asp:ListItem Value="8" >8</asp:ListItem>
                             <asp:ListItem Value="9" >9</asp:ListItem>     
                             <asp:ListItem Value="10" >10</asp:ListItem>
                             <asp:ListItem Value="11" >11</asp:ListItem>
                             <asp:ListItem Value="12" >12</asp:ListItem>
                             <asp:ListItem Value="13" >13</asp:ListItem>
                             <asp:ListItem Value="14" >14</asp:ListItem>     
                             <asp:ListItem Value="15" >15</asp:ListItem>                           
                            </asp:DropDownList>
                        </asp:TableCell>                    
                    </asp:TableRow> 
                    <asp:TableRow ID="TableVQTRow5" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                        <asp:TableCell ID="TableVQTRow5_1" runat="server" Width="25px" HorizontalAlign="Center" Text="40">                        
                        </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow5_2" runat="server" Width="25px">                           
                            <asp:DropDownList ID="DropDownListVQTRow5_2" runat="server" Enabled="True" 
                            meta:resourcekey="DListTipoResource1" class="select">
                            <asp:ListItem Value="0" >0</asp:ListItem>
                             <asp:ListItem Value="1" >1</asp:ListItem>
                             <asp:ListItem Value="2" >2</asp:ListItem>
                             <asp:ListItem Value="3" >3</asp:ListItem>
                             <asp:ListItem Value="4" >4</asp:ListItem>                             
                             <asp:ListItem Value="5" >5</asp:ListItem>
                             <asp:ListItem Value="6" >6</asp:ListItem>
                             <asp:ListItem Value="7" >7</asp:ListItem>
                             <asp:ListItem Value="8" >8</asp:ListItem>
                             <asp:ListItem Value="9" >9</asp:ListItem>     
                             <asp:ListItem Value="10" >10</asp:ListItem>
                             <asp:ListItem Value="11" >11</asp:ListItem>
                             <asp:ListItem Value="12" >12</asp:ListItem>
                             <asp:ListItem Value="13" >13</asp:ListItem>
                             <asp:ListItem Value="14" >14</asp:ListItem>     
                             <asp:ListItem Value="15" >15</asp:ListItem>                         
                            </asp:DropDownList>
                        </asp:TableCell>                    
                    </asp:TableRow> 
                    <asp:TableRow ID="TableVQTRow6" runat="server" SkinID="TableRow" HorizontalAlign="Center">
                        <asp:TableCell ID="TableVQTRow6_1" runat="server" Width="25px" HorizontalAlign="Center" Text="50">                        
                        </asp:TableCell>
                        <asp:TableCell ID="TableVQTRow6_2" runat="server" Width="25px">                           
                            <asp:DropDownList ID="DropDownListVQTRow6_2" runat="server" Enabled="True" 
                            meta:resourcekey="DListTipoResource1" class="select">
                            <asp:ListItem Value="0" >0</asp:ListItem>
                             <asp:ListItem Value="1" >1</asp:ListItem>
                             <asp:ListItem Value="2" >2</asp:ListItem>
                             <asp:ListItem Value="3" >3</asp:ListItem>
                             <asp:ListItem Value="4" >4</asp:ListItem>                             
                             <asp:ListItem Value="5" >5</asp:ListItem>
                             <asp:ListItem Value="6" >6</asp:ListItem>
                             <asp:ListItem Value="7" >7</asp:ListItem>
                             <asp:ListItem Value="8" >8</asp:ListItem>
                             <asp:ListItem Value="9" >9</asp:ListItem>     
                             <asp:ListItem Value="10" >10</asp:ListItem>
                             <asp:ListItem Value="11" >11</asp:ListItem>
                             <asp:ListItem Value="12" >12</asp:ListItem>
                             <asp:ListItem Value="13" >13</asp:ListItem>
                             <asp:ListItem Value="14" >14</asp:ListItem>     
                             <asp:ListItem Value="15" >15</asp:ListItem>                           
                            </asp:DropDownList>
                        </asp:TableCell>                    
                    </asp:TableRow>                               
                </asp:Table>

        <asp:Label ID="Label3" runat="server" Style="z-index: 110; left: 22px; position: absolute; top: 14px; width:auto"
            Text="Label" Visible="False" meta:resourcekey="Label3Resource1"></asp:Label>
       <asp:ListBox ID="LBoxRecursos" runat="server" Height="248px" Style="z-index: 101; left: 22px; position: absolute; top: 54px"
            Width="214px" AutoPostBack="True"
            Visible="False" meta:resourcekey="LBoxDestinosResource1"></asp:ListBox>

        <asp:Label ID="Label2" runat="server" Style="z-index: 109; left: 12px; position: absolute; top: 22px"
            Text="Id. Tabla:" Visible="False"
            meta:resourcekey="Label2Resource1"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" MaxLength="32" Style="z-index: 104; left: 12px; position: absolute; top: 42px"
            Visible="False" Width="173px"
            meta:resourcekey="TextBox1Resource1" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>

        <asp:Button ID="BtAceptar" runat="server" Style="z-index: 105; left: 165px; position: absolute; top: 331px"
            Text="Aceptar" Visible="False" Width="80px" UseSubmitBehavior="true"
            OnClick="BtAceptar_Click" meta:resourcekey="BtAceptarResource1" />
        <ajaxToolkit:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender"
            runat="server" ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
        </ajaxToolkit:ConfirmButtonExtender>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
            ErrorMessage="Es necesario dar un nombre a la tabla de conversión." SetFocusOnError="True"
            Style="z-index: 111; left: 200px; position: absolute; top: 45px"
            Visible="False" meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
    </asp:Panel>


    <asp:LinkButton ID="BtNuevo" runat="server" OnClick="BtNuevo_Click" CausesValidation="False" 
			SkinID="MascaraBotonNuevo" Text="Nuevo" meta:resourcekey="BtNuevoResource1"/>

    <asp:LinkButton ID="BtModificar" runat="server" OnClick="BtModificar_Click" 
	CausesValidation="False" Text="Modificar" SkinID="MascaraBotonModificar" 
	meta:resourcekey="BtModificarResource1"/>

    <asp:LinkButton ID="BtEliminar" runat="server" Text="Eliminar" OnClick="BtEliminar_Click" CausesValidation="False" 
			SkinID="MascaraBotonEliminar" meta:resourcekey="BtEliminarResource1"/>
    <ajaxToolKit:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender" 
        runat="server" ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
    </ajaxToolKit:ConfirmButtonExtender>
  
    <asp:LinkButton ID="BtCancelar" runat="server" Text="Cancelar" OnClick="BtCancelar_Click" CausesValidation="False" 
			SkinID="MascaraBotonCancelar" meta:resourcekey="BtCancelarResource1" />
    <ajaxToolKit:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender" 
        runat="server" ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
    </ajaxToolKit:ConfirmButtonExtender>
    </asp:Content>