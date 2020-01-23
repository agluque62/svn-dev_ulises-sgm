<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="ManttoTop.aspx.cs" Inherits="ManttoTop" Title="Mantenimiento Terminales Operador" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<asp:Label ID="Label1" runat="server" Style="left: 66px; position: absolute; top: 52px; z-index: 100;"
		Text="Terminales de Operador" Font-Bold="True" Width="173px"></asp:Label>
	<asp:ListBox ID="LBTerminalesOperador" runat="server" Height="179px" Style="left: 67px;
		position: absolute; top: 82px; z-index: 101;" Width="132px" AutoPostBack="True" OnSelectedIndexChanged="LBTerminalesOperador_SelectedIndexChanged"></asp:ListBox>
	<asp:CheckBox ID="CBConexionJackEjecutivo" runat="server" AutoPostBack="True" OnCheckedChanged="CBConexionJackEjecutivo_CheckedChanged"
		Style="z-index: 102; left: 296px; position: absolute; top: 83px" Text="Conexión Jack Ejecutivo" />
	<asp:Label ID="LblGWCaida" runat="server" Font-Bold="True" ForeColor="Red" Style="z-index: 103;
		left: 296px; position: absolute; top: 278px" Text="Terminal de operador no presente en el sistema."
		Visible="False" Width="329px"></asp:Label>
	<asp:CheckBox ID="CBConexionJackAyudante" runat="server" AutoPostBack="True" OnCheckedChanged="CBConexionJackAyudante_CheckedChanged"
		Style="z-index: 104; left: 296px; position: absolute; top: 113px" Text="Conexión Jack Ayudante" />
	<asp:CheckBox ID="CBConexionAltavoz" runat="server" AutoPostBack="True" OnCheckedChanged="CBConexionAltavoz_CheckedChanged"
		Style="z-index: 105; left: 296px; position: absolute; top: 143px" Text="Conexión Altavoz" />
	<asp:CheckBox ID="CBPanelEnStandby" runat="server" AutoPostBack="True" OnCheckedChanged="CBPanelEnStandby_CheckedChanged"
		Style="z-index: 106; left: 296px; position: absolute; top: 173px" Text="Panel pasa a standby" />
	<asp:CheckBox ID="CBErrorCargaConfiguracion" runat="server" AutoPostBack="True" OnCheckedChanged="CBErrorCargaConfiguracion_CheckedChanged"
		Style="z-index: 108; left: 296px; position: absolute; top: 206px" Text="Error en carga de configuración" />
</asp:Content>

