<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" StylesheetTheme="TemaPaginasConfiguracion"
	CodeFileBaseClass="PageBaseCD40.PageCD40"	CodeFile="Inicio.aspx.cs" Inherits="Inicio" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--        <script language="javascript" type="text/javascript">
            if (window.name != "<%=GetWindowName()%>") {
                if ("" == "<%=GetWindowName()%>") {
                    window.name = "<%=SetWindowName()%>";
                }
                else {
                    window.name = "invalidAccess";
                    window.open("../BloqueoAplicacion.aspx", "_self");
                }
            }
        </script>
--%>
    	<asp:Label ID="LblPermisos" runat="server" ForeColor="Red" Style="z-index: 100; left: 255px;
		position: absolute; top: 153px" Text="No tiene permisos para ver esta página."
		Visible="False" Width="255px" meta:resourcekey="LblPermisosResource1"></asp:Label>
</asp:Content>
