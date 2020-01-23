<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InformeSectorizacion.aspx.cs" Inherits="InformeSectorizacion" StylesheetTheme="TemaPaginasConfiguracion"  meta:resourcekey="PageResource1"%>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692FBEA5521E1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 0px; overflow: auto;">
      <table width="100%">
      <tr>
        <td align="center">
          <asp:Label ID="LbAplicacion" runat="server" SkinID="LabelCabecera"
                Font-Strikeout="False" Font-Underline="False" Style="z-index: 100;vertical-align:50%;padding-right:3em"
                meta:resourcekey="LB_APLICACION_RES">INFORME ULISES-5000</asp:Label>

          <asp:ImageButton ID="BtnPdf" Height="40px" ImageUrl="~/Images/pdf.png" runat="server" onclick="BtnPdf_Click" Style="z-index: 100;padding-right:1em" Visible="false"  meta:resourcekey="BtnPdfResource1" /> 

          <asp:ImageButton ID="BtnExcel" Height="40px" ImageUrl="~/Images/excel.png" runat="server" onclick="BtnExcel_Click" Visible="false" meta:resourcekey="BtnExcelResource1" />

        </td>
      </tr>
       <tr>
           <td align="center"  style="padding-left:50px">

         <CR:CrystalReportViewer ID="CRVInfSectorizacion" runat="server" 
            AutoDataBind="True" ReportSourceID="CRSourceInf" Visible="False" EnableDatabaseLogonPrompt="False"  ToolPanelView="None"  Width="1200px" Height="600px" GroupTreeImagesFolderUrl="" meta:resourcekey="CRViewerInfResource1" ToolbarImagesFolderUrl="" ToolPanelWidth="200px" 
            EnableDrillDown ="false" SeparatePages="True"/>
        <CR:CrystalReportSource ID="CRSourceInf" runat="server">
        </CR:CrystalReportSource> 

        </td>
 
       </tr>
      </table>
      </div>
    
    
    <div>

   
    </div>
    </form>
</body>
</html>
