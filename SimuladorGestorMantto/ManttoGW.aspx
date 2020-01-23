<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="ManttoGW.aspx.cs" Inherits="ManttoGW" Title="Mantenimiento Pasarelas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<asp:Label ID="Label1" runat="server" Font-Bold="True" Style="left: 72px; position: absolute;
		top: 54px" Text="Pasarelas" Width="74px"></asp:Label>
	<asp:ListBox ID="LBPasarelas" runat="server" Height="225px" Style="left: 71px; position: absolute;
		top: 82px" Width="136px" AutoPostBack="True" OnSelectedIndexChanged="LBPasarelas_SelectedIndexChanged"></asp:ListBox>
	<asp:CheckBox ID="CBConexionRecurso" runat="server" Style="left: 262px; position: absolute;
		top: 67px" Text="Conexión Recurso" AutoPostBack="True" OnCheckedChanged="CBConexionRecurso_CheckedChanged" />
	<asp:CheckBox ID="CBErrorCargaConfiguracion" runat="server" Style="left: 262px; position: absolute;
		top: 98px" Text="Error carga configuración" AutoPostBack="True" OnCheckedChanged="CBErrorCargaConfiguracion_CheckedChanged" />
	<asp:CheckBox ID="CBErrorProtocoloR2" runat="server" Style="left: 262px; position: absolute;
		top: 129px" Text="Error protocolo R2" AutoPostBack="True" OnCheckedChanged="CBErrorProtocoloR2_CheckedChanged" />
	<asp:CheckBox ID="CBFalloLlamadaR2" runat="server" Style="left: 262px; position: absolute;
		top: 160px" Text="Fallo en llamada R2" AutoPostBack="True" OnCheckedChanged="CBFalloLlamadaR2_CheckedChanged" />
	<asp:CheckBox ID="CBLlamadaR2Ok" runat="server" Style="left: 262px; position: absolute;
		top: 191px" Text="Llamada R2 OK" AutoPostBack="True" OnCheckedChanged="CBLlamadaR2Ok_CheckedChanged" />
	<asp:CheckBox ID="CBErrorProtocoloLCN" runat="server" Style="left: 262px; position: absolute;
		top: 222px" Text="Error protocolo LCN" Width="158px" AutoPostBack="True" OnCheckedChanged="CBErrorProtocoloLCN_CheckedChanged" />
	<asp:CheckBox ID="CBActivacionLCN" runat="server" Style="left: 262px; position: absolute;
		top: 253px" Text="Activacion LCN" AutoPostBack="True" OnCheckedChanged="CBActivacionLCN_CheckedChanged" />
	<asp:CheckBox ID="CBLCNFueraServicio" runat="server" Style="left: 262px; position: absolute;
		top: 288px" Text="LCN Fuera de servicio" AutoPostBack="True" OnCheckedChanged="CBLCNFueraServicio_CheckedChanged" />
	<asp:Label ID="LblGWCaida" runat="server" Font-Bold="True" ForeColor="Red" Style="left: 267px;
		position: absolute; top: 328px" Text="Pasarela no presente en el sistema." Visible="False"
		Width="251px"></asp:Label>
</asp:Content>

