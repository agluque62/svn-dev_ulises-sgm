<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="InformeSectorizaciones.aspx.cs" Inherits="InformeSectorizaciones" 
	CodeFileBaseClass="PageBaseCD40.PageCD40"	Title="Informe de Sectorizaciones" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1"%>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">

       function AbreVentana(ventana) {
           window.open(ventana);
       }

    </script>

    <asp:Label ID="LbTitulo" runat="server" Style="z-index: 112; position: absolute; margin-left: auto; margin-right: auto; text-align: center;" 
        Text="INFORME DE SECTORIZACIONES" CssClass="labelPagina" 
        meta:resourcekey="LbTituloResource"></asp:Label>
    
    <asp:Panel ID="PanelInfSectorizacion" BorderStyle="Inset" Height="335px" Style="z-index: 105; left: 140px; position: absolute; top: 100px"
            Width="730px" runat="server">

         <asp:Label ID="LbSectorizacionActiva" runat="server" Style="z-index: 102; left: 50px; position: absolute; top: 67px; height: 18px; width: 261px;" 
                 Text="Sectorización Activa:" meta:resourcekey="LbSectorizacionActivaResource1"></asp:Label>

             <asp:Label ID="LbValorSectorizacionActiva" runat="server" Style="z-index: 102; left: 320px; position: absolute; top: 67px; height: 18px; width: 350px;" meta:resourcekey="LbSectorizacionActivaResource1" Font-Bold="True"></asp:Label>

         <asp:Label ID="LbSectorizacion" runat="server" Style="z-index: 102; left: 50px; position: absolute; top: 125px; height: 18px; width: 255px;" 
                 Text="Sectorización:" meta:resourcekey="LbSectorizacionResource1"></asp:Label>
          <asp:DropDownList ID="DListSectorizaciones" runat="server" Style="z-index: 103; left: 320px; position: absolute; top: 125px; height: 22px; width: 300px;" 
                     meta:resourcekey="DListSectorizacionesResource" class="select" OnSelectedIndexChanged="DListSectorizaciones_SelectedIndexChanged" AutoPostBack="true">
          </asp:DropDownList>

         <asp:LinkButton ID="LinkLanzaInforme" runat="server" Style="z-index: 103; left: 215px; position: absolute;
            top: 220px; width: 280px; height: 50px; background: url('../Images/reportinf.png') no-repeat left top; 
            padding-left:18px;margin-left: 10px;padding-right:10px;" SkinId="LinkButtonCabecera" 
            OnClick="BtLanzaInforme_Click" CausesValidation="False"
            meta:resourcekey="LinkInformeResource" >

            <asp:Label ID="LbEjecutarInforme" runat="server" Style="display:table;margin:10px auto; vertical-align: middle;"
                    Text=" Visualizar el Informe " Visible="true" meta:resourcekey="LbEjecutarInformeResource"></asp:Label>
        </asp:LinkButton>

    </asp:Panel>


</asp:Content>

