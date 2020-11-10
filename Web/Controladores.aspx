<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Controladores.aspx.cs" Inherits="Controladores" 
		Title="Sectorizaciones" EnableEventValidation="false" meta:resourcekey="PageResource1"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <link rel="shortcut icon" href="~/favicon.ico">
    <title></title>
    </head>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">--%>
<body>
    <form id="form1" runat="server" style="background-color: #666699">

    <script type="text/javascript">
        setInterval("OnTimer()", 1000);

        function OnTimer() {
            CallServer("State", "");
        }

        function ClientCallbackError(result, context) {
            alert(result);
        }

        function ReceiveServerData(arg) {
            var info = arg;
            if (info.length > 0) {
                var lblEstadoSacta = document.getElementById('<%=LBLEnlaceSacta.ClientID%>');
                if (lblEstadoSacta != null) {
                    if (info == "0") {
                        lblEstadoSacta.style.color = 'red';
                        lblEstadoSacta.innerText = 'SACTA OFF';
                    }
                    else if (info == "1") {
                        lblEstadoSacta.style.color = 'green';
                        lblEstadoSacta.innerText = 'SACTA ON';
                    }
                }
            }
        }
	</script>

 		<ajaxToolKit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
			CombineScripts="True">
		</ajaxToolKit:ToolkitScriptManager>
    
	 <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>

	<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
		<ContentTemplate>
			<ajaxToolKit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
					TargetControlId="Button1" PopupControlID="Panel3" 
					BackgroundCssClass="modalBackground" DropShadow="true">
			</ajaxToolKit:ModalPopupExtender>
			 <asp:Panel ID="Panel3" runat="server" CssClass="modalBox" Height="174px" Width="396px"
				BackColor="#666699" BorderColor="Black" BorderStyle="Solid">
		 <asp:Label ID="Label6" runat="server" Width="370px"
			 Text="Los sectores de mantenimiento no se pueden agrupar con otros sectores." 
			 BorderStyle="Solid" Style="position:absolute; top: 5px; left: 11px; text-align:center;" Enabled="true"
			 BackColor="#9999FF" Height="42px"></asp:Label>
		 <asp:Button ID="BtnAceptar" runat="server" Text="Aceptar" onclick="BtnCancelarMantto_Click"
			 Style="position:absolute; top: 106px; left: 152px; height: 32px; width: 90px;"/>
    	</asp:Panel>
			<asp:Button ID="Button1" runat="server" Text="Button" Style="display:none;" />
		</ContentTemplate>
	</asp:UpdatePanel>

<%--	<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
		<ContentTemplate>
			<ajaxToolKit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
					TargetControlId="BtnOculto" PopupControlID="Panel1" 
					BackgroundCssClass="modalBackground" DropShadow="true">
			</ajaxToolKit:ModalPopupExtender>--%>
			 <asp:Panel ID="Panel1" runat="server" CssClass="modalBox" Height="174px" Width="416px"
				BackColor="#666699" BorderColor="Black" BorderStyle="Solid" Visible="false" Style="position:absolute; top:300px; left: 400px">
		 <asp:Label ID="Label4" runat="server" Width="370px" 
			 Text="Sectorización sin grabar. Se debe grabar con un identificador" 
			 BorderStyle="Solid" Style="position:absolute; top: 5px; left: 11px; width:auto" Enabled="false"
			 BackColor="#9999FF" Height="21px"></asp:Label>
		 <asp:Label ID="Label5" runat="server" Text="Identificador sectorización:" 
			 Style="position:absolute; top: 53px; left: 14px;"></asp:Label>
		 <asp:TextBox ID="TBNomSectorizacion" runat="server" 
			 Style="position:absolute; top: 52px; left: 195px; height: 22px; width: 201px;"></asp:TextBox>
		 <asp:Button ID="BtnAceptarGuardar" runat="server" Text="Grabar" 
			 Style="position:absolute; top: 106px; left: 92px; height: 32px; width: 90px;" 
			 onclick="BtnAceptarGuardar_Click"/>
		 <asp:Button ID="BtnGuardarSectorizacion" runat="server" Text="Grabar" Visible="false"
			 Style="position:absolute; top: 106px; left: 92px; height: 32px; width: 90px;"
			 onclick="BtnGuardarSectorizacion_Click"/>
		 <asp:LinkButton ID="LnkBCancelar" runat="server" ForeColor="White"
			 Style="position:absolute; top: 116px; left: 325px;" OnClick="LnkBCancelar_OnClick">Cancelar</asp:LinkButton>
	</asp:Panel>
<%--			<asp:Button ID="BtnOculto" runat="server" Text="Button" Style="display:none;" />
		</ContentTemplate>
	</asp:UpdatePanel>--%>
			
		<div style="border: thin solid #C0C0C0; background-color: #CCCCFF; height: 80px; ">
		<asp:Table runat="server" Width="1180px">
			<asp:TableRow ID="TableRow21" runat="server">
				<asp:TableCell ID="TableCell97" runat="server" RowSpan="2" Width="265px">
					 <asp:Image ID="Image1" runat="server" Width="188px" Height="50px"
						 ImageUrl="~/Images/LogoNucleo20x190.png" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell98" runat="server" VerticalAlign="Top" HorizontalAlign="Left" Width="160px">
    				 <asp:Label ID="Label1" runat="server" BorderStyle="Inset" Font-Bold="True" 
						 style="height: 19px" 
						 Text="SECTORIZACIÓN:">
					</asp:Label>
				</asp:TableCell>
				<asp:TableCell ID="TableCell111" runat="server" VerticalAlign="Top" Width="160px">
					<asp:Label ID="LblIdSectorizacion" runat="server" 
						 Font-Bold="True" ForeColor="White"></asp:Label>
				</asp:TableCell>
				<asp:TableCell ID="TableCell99" runat="server" VerticalAlign="Top" Width="160px">
    				 <asp:Label ID="Label3" runat="server" BorderStyle="Inset" Font-Bold="True" 
						 style="height: 19px;" 
						 Text="ENLACE SACTA:"></asp:Label>
				</asp:TableCell>
				<asp:TableCell ID="TableCell199" runat="server" VerticalAlign="Top" Width="160px" >
					<asp:Label ID="LBLEnlaceSacta" runat="server"></asp:Label>
				</asp:TableCell>
				<asp:TableCell ID="TableCell100" runat="server" HorizontalAlign="Right">
					<asp:LinkButton ID="LinkButton1" runat="server" 
						 Height="21px" Width="134px" PostBackUrl="~/Login.aspx">Cerrar Sesión</asp:LinkButton>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow22" runat="server">
				<asp:TableCell ID="TableCell102" runat="server" VerticalAlign="Top" HorizontalAlign="Left">
    				 <asp:Label ID="Label2" runat="server" BorderStyle="Inset" Font-Bold="True" 
						 style="height: 19px;" 
						 Text="ESTADO:">
					</asp:Label>
				</asp:TableCell>
				<asp:TableCell runat="server" VerticalAlign="Top">
					<asp:Label ID="LblEstado" runat="server" Font-Bold="True" ForeColor="White">
					</asp:Label>
				</asp:TableCell>
				<asp:TableCell ID="TableCell103" runat="server">
				</asp:TableCell>
				<asp:TableCell ID="TableCell104" runat="server">
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
		</div>
		<div runat="server" style="border: thin #000000; background-color: #666699" >
			<asp:Table ID="TableCabecera" runat="server" BorderColor="Black" BorderStyle="Solid" 
				 Height="20px" style="margin-top: 3px" Width="1186px" ForeColor="#FFFFCC">
				<asp:TableHeaderRow runat="server">
					<asp:TableHeaderCell ID="TableHeaderCell1" runat="server" Font-Size="Large" ForeColor="#66FF66" Width="25px"></asp:TableHeaderCell>
					<asp:TableHeaderCell ID="TableHeaderCell2" runat="server" Font-Size="Large" ForeColor="#66FF66" Width="105px" HorizontalAlign="Left">Posiciones</asp:TableHeaderCell>
					<asp:TableHeaderCell runat="server" Font-Size="Large" ForeColor="#66FF66" Width="105px" HorizontalAlign="Left">Núcleo</asp:TableHeaderCell>
					<asp:TableHeaderCell runat="server" Font-Size="Large" ForeColor="#66FF66" Width="105px" HorizontalAlign="Left">Agrupación</asp:TableHeaderCell>
					<asp:TableHeaderCell runat="server" Font-Size="Large" ForeColor="#66FF66" HorizontalAlign="Left">Sectores asignados</asp:TableHeaderCell>
				</asp:TableHeaderRow>
			</asp:Table>
		</div>
		<div style="border: thin #000000; background-color: #666699; height: 475px; overflow:auto; ">
    	<asp:Table ID="Table1" runat="server" BorderColor="Silver" BorderStyle="Double" 
				style="margin-top: 3px" Width="1225px" GridLines="Both">
			<asp:TableRow runat="server" Height="30px">
				<asp:TableCell ID="TableCell1" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton1" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/>
				</asp:TableCell>
				<asp:TableCell ID="TableCell101" runat="server" Font-Size="Medium" ForeColor="Black" Width="115px">
					<asp:CheckBox runat="server" ID="CBTop1" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true"/>
				</asp:TableCell>
				<asp:TableCell ID="TableCell26" runat="server" Font-Size="Medium" ForeColor="White" Width="115px"></asp:TableCell>
				<asp:TableCell ID="TableCell27" runat="server" Font-Size="Medium" ForeColor="White" Width="115px"></asp:TableCell>
				<asp:TableCell ID="TableCell28" runat="server" Font-Size="Medium" ForeColor="White">
					<div runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores1" runat="server" Width="750px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					</div>
				</asp:TableCell>
				<asp:TableCell ID="TableCell129" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos1" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged"/>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow runat="server" Height="30px">
				<asp:TableCell ID="TableCell105" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton2" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell2" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop2" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell29" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell30" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell31" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div1" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores2" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					</div>
				</asp:TableCell>
				<asp:TableCell ID="TableCell130" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos2" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow runat="server" Height="30px">
				<asp:TableCell ID="TableCell106" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton3" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell3" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop3" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell32" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell33" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell34" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div2" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores3" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell131" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos3" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow runat="server" Height="30px">
				<asp:TableCell ID="TableCell107" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton4" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell4" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop4" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell35" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell36" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell37" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div3" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores4" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell132" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos4" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow1" runat="server" Height="30px">
				<asp:TableCell ID="TableCell108" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton5" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell5" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop5" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell6" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell7" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell8" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div4" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores5" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell133" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos5" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow2" runat="server" Height="30px">
				<asp:TableCell ID="TableCell109" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton6" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell9" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop6" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell10" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell11" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell12" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div5" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores6" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell134" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos6" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow3" runat="server" Height="30px">
				<asp:TableCell ID="TableCell110" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton7" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell13" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop7" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell14" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell15" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell16" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div6" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores7" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell135" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos7" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow4" runat="server" Height="30px">
				<asp:TableCell ID="TableCell112" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton8" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell17" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop8" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell18" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell19" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell20" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div7" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores8" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell136" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos8" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow5" runat="server" Height="30px">
				<asp:TableCell ID="TableCell113" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton9" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell21" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop9" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell22" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell23" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell24" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div8" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores9" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell137" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos9" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow7" runat="server" Height="30px">
				<asp:TableCell ID="TableCell114" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton10" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell41" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop10" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell42" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell43" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell44" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div10" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores10" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell138" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos10" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow8" runat="server" Height="30px">
				<asp:TableCell ID="TableCell115" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton11" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell45" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop11" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell46" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell47" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell48" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div11" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores11" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell139" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos11" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow9" runat="server" Height="30px">
				<asp:TableCell ID="TableCell116" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton12" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell49" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop12" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell50" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell51" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell52" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div12" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores12" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell140" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos12" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow10" runat="server" Height="30px">
				<asp:TableCell ID="TableCell117" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton13" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell53" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop13" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell54" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell55" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell56" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div13" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores13" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell141" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos13" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow11" runat="server" Height="30px">
				<asp:TableCell ID="TableCell118" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton14" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell57" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop14" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell58" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell59" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell60" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div14" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores14" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell142" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos14" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow12" runat="server" Height="30px">
				<asp:TableCell ID="TableCell119" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton15" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell61" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop15" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell62" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell63" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell64" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div15" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores15" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell143" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos15" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow13" runat="server" Height="30px">
				<asp:TableCell ID="TableCell120" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton16" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell65" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop16" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell66" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell67" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell68" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div16" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores16" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell144" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos16" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow14" runat="server" Height="30px">
				<asp:TableCell ID="TableCell121" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton17" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell69" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop17" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell70" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell71" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell72" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div17" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores17" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell145" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos17" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow15" runat="server" Height="30px">
				<asp:TableCell ID="TableCell122" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton18" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell73" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop18" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell74" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell75" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell76" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div18" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores18" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell146" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos18" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow16" runat="server" Height="30px">
				<asp:TableCell ID="TableCell123" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton19" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell77" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop19" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell78" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell79" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell80" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div19" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores19" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell147" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos19" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow17" runat="server" Height="30px">
				<asp:TableCell ID="TableCell124" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton20" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell81" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop20" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell82" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell83" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell84" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div20" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores20" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell148" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos20" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow18" runat="server" Height="30px">
				<asp:TableCell ID="TableCell125" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton21" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell85" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop21" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell86" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell87" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell88" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div21" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores21" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell149" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos21" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow19" runat="server" Height="30px">
				<asp:TableCell ID="TableCell126" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton22" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell89" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop22" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell90" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell91" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell92" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div22" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores22" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell150" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos22" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow20" runat="server" Height="30px">
				<asp:TableCell ID="TableCell127" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton23" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell93" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop23" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell94" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell95" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell96" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div23" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores23" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell151" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos23" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="TableRow6" runat="server" Height="30px">
				<asp:TableCell ID="TableCell128" runat="server" Font-Size="Medium" ForeColor="Black" Width="25px">
					<asp:RadioButton ID="RadioButton24" runat="server" GroupName="Selected" OnCheckedChanged="RBCheckedChanged" AutoPostBack="true"/></asp:TableCell>
				<asp:TableCell ID="TableCell25" runat="server" Font-Size="Medium" ForeColor="Black">
					<asp:CheckBox runat="server" ID="CBTop24" OnCheckedChanged="CBTop_CheckedChanged" AutoPostBack="true" />
				</asp:TableCell>
				<asp:TableCell ID="TableCell38" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell39" runat="server" Font-Size="Medium" ForeColor="White"></asp:TableCell>
				<asp:TableCell ID="TableCell40" runat="server" Font-Size="Medium" ForeColor="White">
					<div id="Div9" runat="server" style="overflow:auto">
						<asp:CheckBoxList ID="CBLSectores24" runat="server"  Width="800px" RepeatDirection="Horizontal"></asp:CheckBoxList>
					

					

					

					</div>
				

</asp:TableCell>
				<asp:TableCell ID="TableCell152" runat="server" Font-Size="Medium" ForeColor="White" Width="60px">
					<asp:CheckBox ID="CBTodos24" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="CBTodos_CheckedChanged" />
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
	 </div>
		<div runat="server" 
			  style="border: thin outset #333399; background-color: #666699">
		 <asp:Table runat="server" Width="1170px">
			<asp:TableRow runat="server">
				<asp:TableCell runat="server"></asp:TableCell>
			</asp:TableRow>
			<asp:TableRow runat="server">
				<asp:TableCell runat="server" Width="700px" VerticalAlign="Top">
				     <asp:LinkButton ID="BtnCargaSectorizacion" runat="server" style="height: 33px; width: 166px" Text="Cargar sectorización" OnClick="OnClick_BtnCargarSectorizacion"
				      ForeColor="LightGreen"/>
				</asp:TableCell><asp:TableCell runat="server" VerticalAlign="Top">
					 <asp:LinkButton ID="LBtnSectorizaciones" runat="server" style="height: 23px; width: 101px" Text="Sectorizaciones" 
					  ForeColor="LightGreen"/>		
				
</asp:TableCell><asp:TableCell runat="server" VerticalAlign="Top">
					<ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server"
						 TargetControlID="PnlContent" ExpandControlID="LBtnSectorizaciones" CollapseControlID="LBtnSectorizaciones"
						 Collapsed="True" SuppressPostBack="true" ExpandDirection="Vertical">
					</ajaxToolkit:CollapsiblePanelExtender>
					
<asp:Panel ID="PnlContent" runat="server" BorderStyle="Solid" 
						CssClass="collapsePanel" Style="height:200px; width:200px;">
						<asp:ListBox ID="LBSectorizaciones" runat="server" Height="200px" Width="200px" BackColor="#FFCC99" OnSelectedIndexChanged="OnSelectedIndexChange_LBSectorizaciones" AutoPostBack="true"></asp:ListBox>
				

				</asp:Panel>
				
</asp:TableCell><asp:TableCell ID="TableCell156" runat="server" />

                <asp:TableCell ID="TableCell154" runat="server" VerticalAlign="Top">
					<ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server"
						 TargetControlID="Panel2" ExpandControlID="LBtnEliminar" CollapseControlID="LBtnEliminar"
						 Collapsed="True" SuppressPostBack="true" ExpandDirection="Vertical">
					</ajaxToolkit:CollapsiblePanelExtender>
					
<asp:Panel ID="Panel2" runat="server" BorderStyle="Solid" 
						CssClass="collapsePanel" Style="height:200px; width:200px;">
						<asp:ListBox ID="LBEliminar" runat="server" Height="200px" Width="200px" BackColor="#FFCC99" OnSelectedIndexChanged="OnSelectedIndexChange_LBEliminar" AutoPostBack="true"></asp:ListBox>
				

				</asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell153" runat="server" VerticalAlign="Top">
					                 <asp:LinkButton ID="LBtnEliminar" runat="server" style="height: 23px; width: 101px" Text="Eliminar" 
					                  ForeColor="LightGreen"/>		
                				
                </asp:TableCell><asp:TableCell ID="TableCell155" runat="server" />

                <asp:TableCell runat="server" VerticalAlign="Top" HorizontalAlign="Right">
			    	<asp:LinkButton ID="LBtnGuardar" runat="server" onclick="BtnGuardar_Click" ForeColor="LightGreen">Guardar</asp:LinkButton>
				
</asp:TableCell></asp:TableRow></asp:Table></div></form></body></html>