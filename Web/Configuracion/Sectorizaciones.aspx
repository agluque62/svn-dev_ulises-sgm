<%@ Page Language="C#" MasterPageFile="~/CD40.master" AutoEventWireup="true" CodeFile="Sectorizaciones.aspx.cs" CodeFileBaseClass=" PageBaseCD40.PageCD40"
		Inherits="Sectorizaciones" Title="Gestión de Sectorizaciones" EnableEventValidation="false" StylesheetTheme="TemaPaginasConfiguracion" meta:resourcekey="PageResource1"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


  <script type="text/javascript">

      setInterval("OnTimer()", 5000);
      
      function OnTimer()
      {
          CallServer("State", "");
          hideNloader();
      }
            
      function ClientCallbackError(result, context)
      {
         alert(result);
      }

      function RemoveListBox(strElemento) {
          var objListBox = document.getElementById('<% = ListBox1.ClientID %>');
          var bContinuar = new Boolean(true);
          if (objListBox != null) {
              for (var i = 0; i < objListBox.options.length && bContinuar; i++) {
                  if (objListBox.options[i].value == strElemento) {
                      objListBox.remove(i);
                      bContinuar = false;
                      var numEltos = objListBox.options.length;
                      if (numEltos > 0) {
                          objListBox.options[0].selected = true;
                          objListBox.dispatchEvent(new Event('change'));
                      }
                      else {
                          //Si el listbox no tiene elmentos, se hace no visible el panel
                          var objTUCS = document.getElementById('<% = TUCS.ClientID %>');
                          if (objTUCS != null) {
                              objTUCS.style.display = 'none';
                          }
                      }
                  }
              }
          }
      }

      function GetSectorizacionSelect() {
          var objListBox = document.getElementById('<% = ListBox1.ClientID %>');
          var strValor = null;

          if (objListBox != null && objListBox.selectedIndex!= -1)
              strValor = objListBox.options[objListBox.selectedIndex].value;

          return strValor;
      }


      function DisableButton() {
          if (document.activeElement.id == "<%=BtActivar.ClientID %>") {
              //Si se ha pulsado el botón activar, se desactiva el botón y se muestra el loader
              //para que el usuario no pueda volver al pulsar el botón hasta que finalice la operación
              document.getElementById("<%=BtActivar.ClientID %>").disabled = true;
              document.getElementById("initloader").style.display = "block";
          }
      }

      window.onbeforeunload = DisableButton;


      function ReceiveServerData(arg)
      {
        // Recibe una cadena de la función PageCD40.GetCallbackResult de la clase base la siguiente información:
        // "bsectorizando;false;(string)GetGlobalResourceObject("Espaniol", "FinTransaccion");EstadoEnlaceSacta;versionActual;bActualizar"
        // Donde: 
        //  - bSectorizando: puede tomar el valor true o false (Application["Sectorizando"]=="True")
        //  - EstadoEnlaceSacta: valor entero con el valor devuelto por el servicio Web Servicios.GetEstadoSacta().
          //                     lblEstadoSacta.style.color = 'red' si toma el valor 0 (Servicio no arrancado) o el valor 16 (servicio arrancado pero sin conexion con SACTA).
          //                     lblEstadoSacta.style.color = 'green' si toma cualquier otro valor
        //  - VersionActual: string con la fecha/hora de activación de la sectorización activa en formato DD/MM/YYYY HH24:MI:SS 
        //                  (ServicioParaTransaccion.GetVersionConfiguracion((string)Session["idsistema"])))
        //  - actualizar: valor booleano que indica a la pantalla si debe actualizar la versión sectorización mostrada (llama a la función del backend Actualiza)

        var strEstado=arg.split(';');
        var info = strEstado[0];
        var bActivar = null;
        var bSectorizando = new Boolean(false);
        
        if (info.length > 0)
        {
            bActivar = document.getElementById('<%=BtActivar.ClientID%>');
            if (bActivar != null)
            {
                if (info == "True")
                {
                    bSectorizando = true;
                    bActivar.disabled = true;
                }
                else
                {
                    bSectorizando = false;
                    bActivar.disabled = false;
                    
                }
            }
            var bExplorar = document.getElementById('<%=BtExplorar.ClientID%>');
            if (bExplorar != null)
            {
                if (info == "True")
                    bExplorar.disabled = true;
                //else
                //    bExplorar.disabled = false;
            }
        }

       //Se comprueba si se está visualizando la sectorización activa
       var infoVisualizandoActiva = strEstado[1];

        //Se obtiene el estado del enlace SACTA
        info = strEstado[3];
        if (info.length > 0)
        {
            var lblEstadoSacta = document.getElementById('<%=LblEnlaceSacta.ClientID%>');
            if (lblEstadoSacta != null)
            {
                if (info == "0" || info == "16")    // Servicio no arrancado (info==0) o servicio arrancado pero sin conexion con SACTA (info == 16)
                {
                    if (lblEstadoSacta.style.color == 'green') {
                        //Si se ha desactivado SACTA, se comprueba si aparece la sectorización SACTA y si está se elimina de la lista.
                        RemoveListBox('SACTA');
                    }
                    lblEstadoSacta.style.color = 'red';
                    if (bSectorizando == false) {
                        if (infoVisualizandoActiva != null && infoVisualizandoActiva == "True")
                            bActivar.disabled = true;
                    }
                }
                
                else {

                    if (lblEstadoSacta.style.color == 'red')
                    {
                        //Si el servicio SACTA se ha activado y la sectorización SACTA
                        //no está en la lista, se añade al listBox1
                        var objListBox = document.getElementById('<% = ListBox1.ClientID %>');

                        if (objListBox != null) {
                            var isSACTA = false;
                            var i = 0;

                            for (i = 0; i < objListBox.options.length && isSACTA == false; i++) {
                                if (objListBox.options[i].value.toUpperCase().match("SACTA")) {
                                    isSACTA = true;
                                }
                            }

                            if (!isSACTA) {
                                var opt = document.createElement("option");
                                opt.text = "SACTA";
                                opt.value = "SACTA";
                                objListBox.options.add(opt, 0);
                                //Se indica al backend que el listbox ha cambiado
                                objListBox.dispatchEvent(new Event('change'));
                            }
                        }
                    }

                    lblEstadoSacta.style.color = 'green';

                    if (bSectorizando == false && bActivar != null) {
                        var lbIdSectorizacionActiva = document.getElementById('<%=LIdSectorizacion.ClientID%>');

                        if (lbIdSectorizacionActiva != null && infoVisualizandoActiva != null) {
                            if (lbIdSectorizacionActiva.innerHTML == "SACTA" && infoVisualizandoActiva != "True" && GetSectorizacionSelect() == "SACTA") {
                                bActivar.disabled = false;
                            }
                            else {
                                bActivar.disabled = true;
                            }
                        }
                        else {
                            bActivar.disabled = true;
                        }
                    }
                }
            }
        }

        
        info = strEstado[4];
        if (info.length > 0)
        {
            var fecha = info.split(' ');
            if (fecha[0].length > 0)
            {
                var lblFecha = document.getElementById('<%=LFechaSectorizacion.ClientID%>');
                if (lblFecha != null)
                {
                    lblFecha.textContent = fecha[0];
                }
            }
            if (fecha[1].length > 0) {
                var lblHora = document.getElementById('<%=LHoraSectorizacion.ClientID%>');
                if (lblHora != null) {
                    lblHora.textContent = fecha[1];
                }
            }
        }
        info = strEstado[5];
        if (info.length > 0) {
            if (info == "True") {
                __doPostBack("Actualiza");
            }
        }
    }

    function AbreVentana(ventana) {
        window.open(ventana);
    }

    function displayNloader() {
        document.getElementById("Nloader").style.display = "block";
    }

    function hideNloader() {
        document.getElementById("Nloader").style.display = "none";
    }
  </script>


<%--            <asp:TableCell ID="Cell7" runat="server" Visible="False">
                <asp:Button ID="TBox7" Width="100px" BackColor="transparent" runat="server" BorderStyle="None" OnClick="Top_OnClick"
                    style="position:absolute; overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle;"></asp:Button>
					<asp:ImageButton ID="IButton7" runat="server" ImageUrl="~/Configuracion/Images/TextoUCS.jpg"/>                    
            </asp:TableCell> 
--%>
	 <asp:Label ID="LblEnlaceSacta" runat="server" ForeColor="#666666" 
		 style="position: absolute; z-index: 1; left: 661px; top: 10px; width: 104px; height: 18px" 
		 Text="Enlace SACTA" meta:resourcekey="LblEnlaceSactaResource1"></asp:Label>
	 <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
	</asp:ScriptManagerProxy>

    <asp:Label ID="LblIdSectorizacion" runat="server"
        Style="z-index: 127; left: 214px; top: 62px; position: absolute; width: auto"
        Text="Id. sectorización" Visible="False"
        meta:resourcekey="LblIdSectorizacionResource1"></asp:Label>
    <asp:TextBox ID="TBNuevo" runat="server" MaxLength="32" Style="left: 214px; position: absolute; top: 85px; z-index: 126;"
        Visible="False"
        Width="173px" meta:resourcekey="TBNuevoResource1"></asp:TextBox>

     <asp:Panel ID="PanelNoPermissions" runat="server" ForeColor="#00C000" 
		 Style="z-index: -1; left: 80px; position: absolute; top: 100px; height: 60px; width: 697px;" 
		 meta:resourcekey="PanelNoPermissions" Visible="false">
         <asp:Label ID="LblPermisos" runat="server" ForeColor="Red" Style="z-index: 100; left: 255px; width:auto; position : absolute; top: 153px" 
            Text="No tiene permisos para ver esta sección."
            meta:resourcekey="LblPermisosResource1">
         </asp:Label>
     </asp:Panel>

     <div   id="initloader"  style="position:absolute;margin-top:2px;background-color:white;z-index:1000; display:none" >
         <div id="Desactivador"  style="position: absolute;margin-top: 0px;background-color:transparent;width:900px; height:650px" > </div>
         <div id="loader" style="display:block;position:absolute;top:50%;left:50%;width:150px;height:150px;margin:500px 0 0 470px;background-image: url('../Cluster/Images/progress.gif');background-repeat: no-repeat;background-size: 60% 60%">
        </div>  
     </div>
    
    <div id="Nloader" class="nucleo_loader_back" style="display:none; position: absolute;margin-top: 0px;width:920px; height:750px; background-color: white; z-index: 500; opacity: 0.5;" >
        <div class="preloader-1"></div>   
        <div class="preloader-1-text">Un momento por favor...</div>
          <div style="position:absolute;left:400px">
          <span class="preloader-1-line1 line-1"></span>
          <span class="preloader-1-line2 line-2"></span>
          <span class="preloader-1-line3 line-3"></span>
          <span class="preloader-1-line4 line-4"></span>
          <span class="preloader-1-line5 line-5"></span>
          <span class="preloader-1-line6 line-6"></span>
          <span class="preloader-1-line7 line-7"></span>
          <span class="preloader-1-line8 line-8"></span>
          <span class="preloader-1-line9 line-9"></span>
        </div> 
    </div>

    <asp:Panel ID="PanelActiva" runat="server" ForeColor="#97D700"
		 GroupingText="Sectorización activa" 
		 
		 Style="z-index: -1; left: 204px; position: absolute; top: 408px; height: 60px; width: 657px;" 
		 meta:resourcekey="PanelActivaResource1">
		 &nbsp;&nbsp;&nbsp;&nbsp;
		<asp:Label ID="LFecha" runat="server" Style="z-index: 100; left: 14px; position: absolute;
			top: 26px" Text="Fecha:" Width="47px" ForeColor="#4A7729" Font-Bold="true" 
			 meta:resourcekey="LFechaResource1"></asp:Label>
		<asp:Label ID="LHora" runat="server" Style="z-index: 106; left: 206px; position: absolute;
			top: 26px" Text="Hora:" ForeColor="#4A7729" Font-Bold="true" meta:resourcekey="LHoraResource1"></asp:Label>
		<asp:Label ID="LId" runat="server" Style="z-index: 102; left: 377px; position: absolute;
			top: 26px" Text="Nombre:" ForeColor="#4A7729" Font-Bold="true" meta:resourcekey="LIdResource1"></asp:Label>
		<asp:Label ID="LFechaSectorizacion" runat="server" Style="z-index: 101; left: 64px; position: absolute;
			top: 26px" Width="106px" ForeColor="Black" 
			 meta:resourcekey="LFechaSectorizacionResource1"></asp:Label>
		<asp:Label ID="LHoraSectorizacion" runat="server" Style="z-index: 104; left: 258px; position: absolute;
			top: 26px" Width="103px" ForeColor="Black" 
			 meta:resourcekey="LHoraSectorizacionResource1"></asp:Label>
		<asp:Label ID="LIdSectorizacion" runat="server" Style="z-index: 107; left: 446px; position: absolute;
			top: 26px" Width="156px" ForeColor="Black" 
			 meta:resourcekey="LIdSectorizacionResource1"></asp:Label>
		<asp:Button ID="BVerSectorizacionActiva" runat="server" Style="z-index: 105; left: 587px; position: absolute;
			top: 20px" Text="Ver" Width="56px" OnClick="BVerSectorizacionActiva_Click" 
				Enabled="False" meta:resourcekey="BVerSectorizacionActivaResource1" />
	</asp:Panel>

    <asp:Label ID="Label6" runat="server" Text="GESTIÓN DE SECTORIZACIONES" 
		 CssClass="labelPagina" meta:resourcekey="Label6Resource1"></asp:Label>
    <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"
			SkinID="MascaraListaElementos" meta:resourcekey="ListBox1Resource1"></asp:ListBox>


    <asp:Button ID="BtActivar" runat="server" OnClick="BtActivar_Click" Style="left: 208px;
        position: absolute; top: 498px; z-index: 101;" Text="Activar" BackColor="#b6c8a9"
		    Width="100px" Enabled="False" Height="24px" 
		    meta:resourcekey="BtActivarResource1" />

    <asp:LinkButton ID="BtEliminar"
        runat="server" 
        style="position: absolute; left: 682px; top: 570px; right: 63px; height: 19px; width: 62px;"
        OnClick="BtEliminar_Click" 
        
		 CausesValidation="False" Text="Eliminar" SkinID="MascaraBotonEliminar" 
		 meta:resourcekey="BtEliminarResource1"/>
    <asp:ConfirmButtonExtender ID="BtEliminar_ConfirmButtonExtender" runat="server" 
        ConfirmText="" Enabled="True" TargetControlID="BtEliminar">
    </asp:ConfirmButtonExtender>
    <asp:LinkButton ID="BtNuevo" runat="server"  OnClick="BtNuevo_Click" 
         style="position: absolute; left: 236px; top: 570px; "
		 CausesValidation="False" Text="Nuevo" SkinID="MascaraBotonNuevo" 
		 meta:resourcekey="BtNuevoResource1"/>
    <asp:Button ID="BtAceptar" runat="server" Style="left: 250px; position: absolute;
        top: 150px; z-index: 105; vertical-align: text-top;" Text="Aceptar" 
		 Visible="False" Width="90px" OnClick="BtAceptar_Click" Height="25px" 
		 meta:resourcekey="BtAceptarResource1"  UseSubmitBehavior="true"/>
    <asp:ConfirmButtonExtender ID="BtAceptar_ConfirmButtonExtender" runat="server" 
        ConfirmText="" Enabled="True" TargetControlID="BtAceptar">
    </asp:ConfirmButtonExtender>
    <asp:LinkButton ID="BtCancelar" runat="server" OnClick="BtCancelar_Click" 
         style="position: absolute; left: 682px; top: 570px; right: 63px;"
		 CausesValidation="False" Text="Cancelar" SkinID="MascaraBotonCancelar" 
		 meta:resourcekey="BtCancelarResource1" />   
    <asp:ConfirmButtonExtender ID="BtCancelar_ConfirmButtonExtender" runat="server" 
        ConfirmText="" Enabled="True" TargetControlID="BtCancelar">
    </asp:ConfirmButtonExtender>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Resumen de errores"
         Style="z-index: 137; left: 250px; position: absolute; top: 345px" 
         Visible="true" meta:resourcekey="erroresResource1" />

    
    <div runat="server" id="DvMarcoTops"
        style="position: absolute; top: 58px; width: 650px; left: 205px; height: 345px; border: inset 2px black; overflow: auto; margin-bottom: 9px; background-color: white">
        <asp:Table ID="TUCS" runat="server" BorderStyle="None"
            BackColor="Transparent" GridLines="Both" meta:resourcekey="TUCSResource1">
            <asp:TableRow ID="Row1" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell1" runat="server" Visible="false" ToolTip="">
                    <asp:Button ID="TBox1" Width="100px" BackColor="transparent" runat="server"
                        BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton1" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell2" runat="server" Visible="false">
                    <asp:Button ID="TBox2" Width="100px" BackColor="transparent" runat="server"
                        BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton2" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell3" runat="server" Visible="false">
                    <asp:Button ID="TBox3" Width="100px" BackColor="transparent" runat="server"
                        BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton3" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell4" runat="server" Visible="false">
                    <asp:Button ID="TBox4" Width="100px" BackColor="transparent" runat="server"
                        BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton4" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell5" runat="server" Visible="false">
                    <asp:Button ID="TBox5" Width="100px" BackColor="transparent" runat="server"
                        BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton5" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell6" runat="server" Visible="false">
                    <asp:Button ID="TBox6" Width="100px" BackColor="transparent" runat="server"
                        BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton6" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="Cell7" runat="server" Visible="false">
                <asp:Button ID="TBox7" Width="100px" BackColor="transparent" runat="server" BorderStyle="None" OnClick="Top_OnClick"
                    style="position:absolute; overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle;"></asp:Button>
					<asp:ImageButton ID="IButton7" runat="server" ImageUrl="~/Configuracion/Images/TextoUCS.jpg"/>                    
            </asp:TableCell> 
                --%>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow1" runat="server" VerticalAlign="Top"
                BorderWidth="0px">
                <asp:TableCell ID="UserCell1" runat="server" Visible="false">
                    <asp:Button ID="TextBox1" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick" 
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell2" runat="server" Visible="false">
                    <asp:Button ID="TextBox2" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell3" runat="server" Visible="false">
                    <asp:Button ID="TextBox3" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell4" runat="server" Visible="false">
                    <asp:Button ID="TextBox4" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell5" runat="server" Visible="false">
                    <asp:Button ID="TextBox5" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell6" runat="server" Visible="false">
                    <asp:Button ID="TextBox6" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="UserCell7" runat="server" Visible="false">
                <asp:Button ID="TextBox7" Width="100px" BackColor="Transparent" runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                    style="overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle; position: absolute"></asp:Button>
                <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick"/>
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios1" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios1" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios1" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: auto" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios2" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios2" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios3" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios3" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios4" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios4" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios5" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios5" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios6" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios6" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden" />


                    </div>
                </asp:TableCell>
                <%--				<asp:TableCell ID="CellUsuarios7" runat="server" Visible="false">
					<div style = "height:75px; overflow:hidden; width:100px">
						<asp:ListBox ID="BUsuarios7"  Height="100px" Width="100px" runat="server" BorderStyle="None" BackColor="Coral" Visible="false"
								Style="height:75px; width:100px; font-family: Tahoma;" SelectionMode="Multiple"></asp:ListBox>
					</div>
				</asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row2" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell7" runat="server" Visible="false">
                    <asp:Button ID="TBox7" Width="100px" BackColor="transparent" runat="server"
                        BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton7" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell8" runat="server" Visible="false">
                    <asp:Button ID="TBox8" Width="100px" BackColor="transparent" runat="server"
                        BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton8" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell9" runat="server" Visible="false">
                    <asp:Button ID="TBox9" Width="100px" BackColor="transparent" runat="server"
                        BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton9" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell10" runat="server" Visible="false">
                    <asp:Button ID="TBox10" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton10" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell11" runat="server" Visible="false">
                    <asp:Button ID="TBox11" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton11" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell12" runat="server" Visible="false">
                    <asp:Button ID="TBox12" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton12" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="Cell13" runat="server" Visible="false">
                <asp:Button ID="TBox13" Width="100px" BackColor="transparent" runat="server" BorderStyle="None" OnClick="Top_OnClick"
                    style="position:absolute; overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle;"></asp:Button>
					<asp:ImageButton ID="IButton14" runat="server" ImageUrl="~/Configuracion/Images/TextoUCS.jpg"/>                    
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow2" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell7" runat="server" Visible="false">
                    <asp:Button ID="TextBox7" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell8" runat="server" Visible="false">
                    <asp:Button ID="TextBox8" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell9" runat="server" Visible="false">
                    <asp:Button ID="TextBox9" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell10" runat="server" Visible="false">
                    <asp:Button ID="TextBox10" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell11" runat="server" Visible="false">
                    <asp:Button ID="TextBox11" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton11" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"
                        OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell12" runat="server" Visible="false">
                    <asp:Button ID="TextBox12" Width="100px" BackColor="Transparent"
                        runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton12" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="UserCell13" runat="server" Visible="false">
                <asp:Button ID="TextBox13" Width="100px" BackColor="Transparent" runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                    style="overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle; position: absolute"></asp:Button>
                <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick"/>
            </asp:TableCell>            
                --%>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios2" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios7" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios7" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios8" runat="server" Visible="false"
                    VerticalAlign="Top">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios8" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios9" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios9" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios10" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios10" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios11" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios11" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios12" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios12" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <%--				<asp:TableCell ID="CellUsuarios13" runat="server" Visible="false">
					<asp:ListBox ID="BUsuarios13"  Height="100px" Width="100px" runat="server" BorderStyle="None" BackColor="Coral" Visible="false"
								Style="font-family: Tahoma;" SelectionMode="Multiple"></asp:ListBox>
				</asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row3" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell13" runat="server" Visible="false">
                    <asp:Button ID="TBox13" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton13" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell14" runat="server" Visible="false">
                    <asp:Button ID="TBox14" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton14" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell15" runat="server" Visible="false">
                    <asp:Button ID="TBox15" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton15" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell16" runat="server" Visible="false">
                    <asp:Button ID="TBox16" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton16" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell17" runat="server" Visible="false">
                    <asp:Button ID="TBox17" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton17" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell18" runat="server" Visible="false">
                    <asp:Button ID="TBox18" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton18" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="Cell19" runat="server" Visible="false">
                <asp:Button ID="TBox19" Width="100px" BackColor="transparent" runat="server" BorderStyle="None" OnClick="Top_OnClick"
                    style="position:absolute; overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle;"></asp:Button>
					<asp:ImageButton ID="IButton19" runat="server" ImageUrl="~/Configuracion/Images/TextoUCS.jpg"/>                    
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow3" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell13" runat="server" Visible="false">
                    <asp:Button ID="TextBox13" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton13" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell14" runat="server" Visible="false">
                    <asp:Button ID="TextBox14" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton14" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell15" runat="server" Visible="false">
                    <asp:Button ID="TextBox15" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton15" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell16" runat="server" Visible="false">
                    <asp:Button ID="TextBox16" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton16" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell17" runat="server" Visible="false">
                    <asp:Button ID="TextBox17" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton17" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell18" runat="server" Visible="false">
                    <asp:Button ID="TextBox18" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton18" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="UserCell21" runat="server" Visible="false">
                <asp:Button ID="TextBox21" BackColor="Transparent" runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                    style="overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30; width: 101px; text-align: center; vertical-align:middle; position: absolute" ></asp:Button>
                <asp:ImageButton ID="ImageButton21" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick"/>
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios3" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios13" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios13" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios14" runat="server" Visible="false"
                    VerticalAlign="Top">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios14" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios15" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios15" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small;overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios16" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios16" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small;overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios17" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios17" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small;overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios18" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios18" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <%--				<asp:TableCell ID="CellUsuarios21" runat="server" Visible="false">
					<asp:ListBox ID="BUsuarios21"  Height="100px" Width="100px" runat="server" BorderStyle="None" BackColor="Coral" Visible="false"
								Style="font-family: Tahoma;" SelectionMode="Multiple"></asp:ListBox>
				</asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row4" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell19" runat="server" Visible="false">
                    <asp:Button ID="TBox19" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton19" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell20" runat="server" Visible="false">
                    <asp:Button ID="TBox20" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton20" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell21" runat="server" Visible="false">
                    <asp:Button ID="TBox21" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton21" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell22" runat="server" Visible="false">
                    <asp:Button ID="TBox22" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton22" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell23" runat="server" Visible="false">
                    <asp:Button ID="TBox23" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton23" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell24" runat="server" Visible="false">
                    <asp:Button ID="TBox24" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton24" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="Cell28" runat="server" Visible="false">
                <asp:Button ID="TBox28" Width="100px" BackColor="transparent" runat="server" BorderStyle="None" OnClick="Top_OnClick"
                    style="position:absolute; overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle;"></asp:Button>
					<asp:ImageButton ID="IButton28" runat="server" ImageUrl="~/Configuracion/Images/TextoUCS.jpg"/>                    
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow4" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell19" runat="server" Visible="false">
                    <asp:Button ID="TextBox19" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton19" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell20" runat="server" Visible="false">
                    <asp:Button ID="TextBox20" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton20" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell21" runat="server" Visible="false">
                    <asp:Button ID="TextBox21" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton21" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell22" runat="server" Visible="false">
                    <asp:Button ID="TextBox22" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton22" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell23" runat="server" Visible="false">
                    <asp:Button ID="TextBox23" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton23" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell24" runat="server" Visible="false">
                    <asp:Button ID="TextBox24" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton24" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="UserCell28" runat="server" Visible="false">
                <asp:Button ID="TextBox28" BackColor="Transparent" runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                    style="overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30; width: 101px; text-align: center; vertical-align:middle; position: absolute" ></asp:Button>
                <asp:ImageButton ID="ImageButton28" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg"  OnClick="UCS_OnClick"/>
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios4" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios19" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios19" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios20" runat="server" Visible="false"
                    VerticalAlign="Top">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios20" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios21" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios21" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios22" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios22" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios23" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios23" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios24" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios24" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <%--				<asp:TableCell ID="CellUsuarios28" runat="server" Visible="false">
					<asp:ListBox ID="BUsuarios28"  Height="100px" Width="100px" runat="server" BorderStyle="None" BackColor="Coral" Visible="false"
								Style="font-family: Tahoma;" SelectionMode="Multiple"></asp:ListBox>
				</asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row5" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell25" runat="server" Visible="false">
                    <asp:Button ID="TBox25" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton25" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell26" runat="server" Visible="false">
                    <asp:Button ID="TBox26" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton26" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell27" runat="server" Visible="false">
                    <asp:Button ID="TBox27" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton27" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell28" runat="server" Visible="false">
                    <asp:Button ID="TBox28" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton28" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell29" runat="server" Visible="false">
                    <asp:Button ID="TBox29" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton29" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell30" runat="server" Visible="false">
                    <asp:Button ID="TBox30" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton30" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="Cell35" runat="server" Visible="false">
                <asp:Button ID="TBox35" Width="100px" BackColor="transparent" runat="server" BorderStyle="None" OnClick="Top_OnClick"
                    style="position:absolute; overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle;"></asp:Button>
					<asp:ImageButton ID="IButton35" runat="server" ImageUrl="~/Configuracion/Images/TextoUCS.jpg"/>                    
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow5" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell25" runat="server" Visible="false">
                    <asp:Button ID="TextBox25" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton25" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell26" runat="server" Visible="false">
                    <asp:Button ID="TextBox26" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton26" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell27" runat="server" Visible="false">
                    <asp:Button ID="TextBox27" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton27" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell28" runat="server" Visible="false">
                    <asp:Button ID="TextBox28" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton28" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell29" runat="server" Visible="false">
                    <asp:Button ID="TextBox29" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton29" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell30" runat="server" Visible="false">
                    <asp:Button ID="TextBox30" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton30" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="UserCell35" runat="server" Visible="false">
                <asp:Button ID="TextBox35" BackColor="Transparent" runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                    style="overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30; width: 101px; text-align: center; vertical-align:middle; position: absolute" ></asp:Button>
                <asp:ImageButton ID="ImageButton35" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick"/>
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios5" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios25" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios25" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios26" runat="server" Visible="false"
                    VerticalAlign="Top">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios26" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios27" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios27" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios28" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios28" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios29" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios29" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios30" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios30" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <%--				<asp:TableCell ID="CellUsuarios35" runat="server" Visible="false">
					<asp:ListBox ID="BUsuarios35"  Height="100px" Width="100px" runat="server" BorderStyle="None" BackColor="Coral" Visible="false"
								Style="font-family: Tahoma;" SelectionMode="Multiple"></asp:ListBox>
				</asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row6" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell31" runat="server" Visible="false">
                    <asp:Button ID="TBox31" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton31" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell32" runat="server" Visible="false">
                    <asp:Button ID="TBox32" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton32" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell33" runat="server" Visible="false">
                    <asp:Button ID="TBox33" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton33" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell34" runat="server" Visible="false">
                    <asp:Button ID="TBox34" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton34" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell35" runat="server" Visible="false">
                    <asp:Button ID="TBox35" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton35" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell36" runat="server" Visible="false">
                    <asp:Button ID="TBox36" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton36" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="Cell42" runat="server" Visible="false">
                <asp:Button ID="TBox42" Width="100px" BackColor="transparent" runat="server" BorderStyle="None" OnClick="Top_OnClick"
                    style="position:absolute; overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle;"></asp:Button>
					<asp:ImageButton ID="IButton42" runat="server" ImageUrl="~/Configuracion/Images/TextoUCS.jpg"/>                    
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow6" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell31" runat="server" Visible="false">
                    <asp:Button ID="TextBox31" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton31" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell32" runat="server" Visible="false">
                    <asp:Button ID="TextBox32" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton32" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell33" runat="server" Visible="false">
                    <asp:Button ID="TextBox33" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton33" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell34" runat="server" Visible="false">
                    <asp:Button ID="TextBox34" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton34" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell35" runat="server" Visible="false">
                    <asp:Button ID="TextBox35" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton35" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell36" runat="server" Visible="false">
                    <asp:Button ID="TextBox36" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton36" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="UserCell42" runat="server" Visible="false">
                <asp:Button ID="TextBox42" BackColor="Transparent" runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                    style="overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30; width: 101px; text-align: center; vertical-align:middle; position: absolute" ></asp:Button>
                <asp:ImageButton ID="ImageButton42" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick"/>
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios6" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios31" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios31" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios32" runat="server" Visible="false"
                    VerticalAlign="Top">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios32" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios33" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios33" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios34" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios34" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios35" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios35" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios36" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios36" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <%--				<asp:TableCell ID="CellUsuarios42" runat="server" Visible="false">
					<asp:ListBox ID="BUsuarios42"  Height="100px" Width="100px" runat="server" BorderStyle="None" BackColor="Coral" Visible="false"
								Style="font-family: Tahoma;" SelectionMode="Multiple"></asp:ListBox>
				</asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row7" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell37" runat="server" Visible="false">
                    <asp:Button ID="TBox37" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton37" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell38" runat="server" Visible="false">
                    <asp:Button ID="TBox38" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton38" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell39" runat="server" Visible="false">
                    <asp:Button ID="TBox39" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton39" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell40" runat="server" Visible="false">
                    <asp:Button ID="TBox40" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton40" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell41" runat="server" Visible="false">
                    <asp:Button ID="TBox41" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton41" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell42" runat="server" Visible="false">
                    <asp:Button ID="TBox42" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton42" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="Cell49" runat="server" Visible="false">
                <asp:Button ID="TBox49" Width="100px" BackColor="transparent" runat="server" BorderStyle="None" OnClick="Top_OnClick"
                    style="position:absolute; overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle;"></asp:Button>
					<asp:ImageButton ID="IButton49" runat="server" ImageUrl="~/Configuracion/Images/TextoUCS.jpg"/>                    
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow7" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell37" runat="server" Visible="false">
                    <asp:Button ID="TextBox37" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton37" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell38" runat="server" Visible="false">
                    <asp:Button ID="TextBox38" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton38" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell39" runat="server" Visible="false">
                    <asp:Button ID="TextBox39" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton39" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell40" runat="server" Visible="false">
                    <asp:Button ID="TextBox40" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton40" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell41" runat="server" Visible="false">
                    <asp:Button ID="TextBox41" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton41" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell42" runat="server" Visible="false">
                    <asp:Button ID="TextBox42" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton42" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="UserCell49" runat="server" Visible="false">
                <asp:Button ID="TextBox49" BackColor="Transparent" runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                    style="overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30; width: 101px; text-align: center; vertical-align:middle; position: absolute" ></asp:Button>
                <asp:ImageButton ID="ImageButton49" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick"/>
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios7" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios37" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios37" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios38" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios38" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios39" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios39" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios40" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios40" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios41" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios41" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios42" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios42" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <%--				<asp:TableCell ID="CellUsuarios43" runat="server" Visible="false">
					<asp:ListBox ID="BUsuarios43"  Height="100px" Width="100px" runat="server" BorderStyle="None" BackColor="Coral" Visible="false"
								Style="font-family: Tahoma;" SelectionMode="Multiple"></asp:ListBox>
				</asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row8" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell43" runat="server" Visible="false">
                    <asp:Button ID="TBox43" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton43" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell44" runat="server" Visible="false">
                    <asp:Button ID="TBox44" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton44" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell45" runat="server" Visible="false">
                    <asp:Button ID="TBox45" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton45" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell46" runat="server" Visible="false">
                    <asp:Button ID="TBox46" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton46" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell47" runat="server" Visible="false">
                    <asp:Button ID="TBox47" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton47" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell48" runat="server" Visible="false">
                    <asp:Button ID="TBox48" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton48" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="Cell49" runat="server" Visible="false">
                <asp:Button ID="TBox49" Width="100px" BackColor="transparent" runat="server" BorderStyle="None" OnClick="Top_OnClick"
                    style="position:absolute; overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align:middle;"></asp:Button>
					<asp:ImageButton ID="IButton49" runat="server" ImageUrl="~/Configuracion/Images/TextoUCS.jpg"/>                    
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow8" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell43" runat="server" Visible="false">
                    <asp:Button ID="TextBox43" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton43" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell44" runat="server" Visible="false">
                    <asp:Button ID="TextBox44" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton44" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell45" runat="server" Visible="false">
                    <asp:Button ID="TextBox45" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton45" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell46" runat="server" Visible="false">
                    <asp:Button ID="TextBox46" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton46" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell47" runat="server" Visible="false">
                    <asp:Button ID="TextBox47" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton47" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell48" runat="server" Visible="false">
                    <asp:Button ID="TextBox48" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton48" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <%--            <asp:TableCell ID="UserCell49" runat="server" Visible="false">
                <asp:Button ID="TextBox49" BackColor="Transparent" runat="server" BorderStyle="None" OnClick="UCS_OnClick"
                    style="overflow:hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30; width: 101px; text-align: center; vertical-align:middle; position: absolute" ></asp:Button>
                <asp:ImageButton ID="ImageButton49" runat="server" ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick"/>
            </asp:TableCell>
                --%>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios8" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios43" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios43" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios44" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios44" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios45" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios45" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios46" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios46" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios47" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios47" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios48" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios48" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row9" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell49" runat="server" Visible="false">
                    <asp:Button ID="TBox49" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton49" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell50" runat="server" Visible="false">
                    <asp:Button ID="TBox50" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton50" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell51" runat="server" Visible="false">
                    <asp:Button ID="TBox51" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton51" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell52" runat="server" Visible="false">
                    <asp:Button ID="TBox52" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton52" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell53" runat="server" Visible="false">
                    <asp:Button ID="TBox53" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton53" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell54" runat="server" Visible="false">
                    <asp:Button ID="TBox54" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton54" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow9" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell49" runat="server" Visible="false">
                    <asp:Button ID="TextBox49" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton49" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell50" runat="server" Visible="false">
                    <asp:Button ID="TextBox50" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton50" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell51" runat="server" Visible="false">
                    <asp:Button ID="TextBox51" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton51" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell52" runat="server" Visible="false">
                    <asp:Button ID="TextBox52" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton52" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell53" runat="server" Visible="false">
                    <asp:Button ID="TextBox53" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton53" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell54" runat="server" Visible="false">
                    <asp:Button ID="TextBox54" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton54" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios9" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios49" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios49" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios50" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios50" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios51" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios51" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios52" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios52" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios53" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios53" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios54" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios54" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row10" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell55" runat="server" Visible="false">
                    <asp:Button ID="TBox55" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton55" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell56" runat="server" Visible="false">
                    <asp:Button ID="TBox56" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton56" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell57" runat="server" Visible="false">
                    <asp:Button ID="TBox57" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton57" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell58" runat="server" Visible="false">
                    <asp:Button ID="TBox58" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton58" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell59" runat="server" Visible="false">
                    <asp:Button ID="TBox59" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton59" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell60" runat="server" Visible="false">
                    <asp:Button ID="TBox60" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton60" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow10" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell55" runat="server" Visible="false">
                    <asp:Button ID="TextBox55" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton55" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell56" runat="server" Visible="false">
                    <asp:Button ID="TextBox56" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton56" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell57" runat="server" Visible="false">
                    <asp:Button ID="TextBox57" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton57" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell58" runat="server" Visible="false">
                    <asp:Button ID="TextBox58" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton58" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell59" runat="server" Visible="false">
                    <asp:Button ID="TextBox59" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton59" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell60" runat="server" Visible="false">
                    <asp:Button ID="TextBox60" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton60" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios10" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios55" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios55" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios56" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios56" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios57" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios57" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios58" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios58" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios59" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios59" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios60" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios60" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row11" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell61" runat="server" Visible="false">
                    <asp:Button ID="TBox61" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton61" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell62" runat="server" Visible="false">
                    <asp:Button ID="TBox62" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton62" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell63" runat="server" Visible="false">
                    <asp:Button ID="TBox63" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton63" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell64" runat="server" Visible="false">
                    <asp:Button ID="TBox64" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton64" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell65" runat="server" Visible="false">
                    <asp:Button ID="TBox65" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton65" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell66" runat="server" Visible="false">
                    <asp:Button ID="TBox66" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton66" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow11" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell61" runat="server" Visible="false">
                    <asp:Button ID="TextBox61" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton61" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell62" runat="server" Visible="false">
                    <asp:Button ID="TextBox62" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton62" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell63" runat="server" Visible="false">
                    <asp:Button ID="TextBox63" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton63" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell64" runat="server" Visible="false">
                    <asp:Button ID="TextBox64" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton64" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell65" runat="server" Visible="false">
                    <asp:Button ID="TextBox65" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton65" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell66" runat="server" Visible="false">
                    <asp:Button ID="TextBox66" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton66" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios11" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios61" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios61" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios62" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios62" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios63" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios63" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios64" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios64" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios65" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios65" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios66" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios66" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>
   
            <asp:TableRow ID="Row12" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell67" runat="server" Visible="false">
                    <asp:Button ID="TBox67" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton67" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell68" runat="server" Visible="false">
                    <asp:Button ID="TBox68" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton68" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell69" runat="server" Visible="false">
                    <asp:Button ID="TBox69" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton69" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell70" runat="server" Visible="false">
                    <asp:Button ID="TBox70" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton70" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell71" runat="server" Visible="false">
                    <asp:Button ID="TBox71" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton71" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell72" runat="server" Visible="false">
                    <asp:Button ID="TBox72" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton72" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow12" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell67" runat="server" Visible="false">
                    <asp:Button ID="TextBox67" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton67" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell68" runat="server" Visible="false">
                    <asp:Button ID="TextBox68" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton68" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell69" runat="server" Visible="false">
                    <asp:Button ID="TextBox69" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton69" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell70" runat="server" Visible="false">
                    <asp:Button ID="TextBox70" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton70" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell71" runat="server" Visible="false">
                    <asp:Button ID="TextBox71" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton71" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell72" runat="server" Visible="false">
                    <asp:Button ID="TextBox72" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton72" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios12" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios67" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios67" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios68" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios68" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios69" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios69" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios70" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios70" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios71" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios71" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios72" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios72" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row13" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell73" runat="server" Visible="false">
                    <asp:Button ID="TBox73" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton73" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell74" runat="server" Visible="false">
                    <asp:Button ID="TBox74" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton74" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell75" runat="server" Visible="false">
                    <asp:Button ID="TBox75" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton75" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell76" runat="server" Visible="false">
                    <asp:Button ID="TBox76" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton76" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell77" runat="server" Visible="false">
                    <asp:Button ID="TBox77" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton77" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell78" runat="server" Visible="false">
                    <asp:Button ID="TBox78" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton78" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow13" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell73" runat="server" Visible="false">
                    <asp:Button ID="TextBox73" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton73" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell74" runat="server" Visible="false">
                    <asp:Button ID="TextBox74" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton74" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell75" runat="server" Visible="false">
                    <asp:Button ID="TextBox75" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton75" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell76" runat="server" Visible="false">
                    <asp:Button ID="TextBox76" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton76" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell77" runat="server" Visible="false">
                    <asp:Button ID="TextBox77" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton77" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell78" runat="server" Visible="false">
                    <asp:Button ID="TextBox78" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton78" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios13" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios73" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios73" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios74" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios74" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios75" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios75" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios76" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios76" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios77" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios77" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios78" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios78" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row14" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell79" runat="server" Visible="false">
                    <asp:Button ID="TBox79" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton79" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell80" runat="server" Visible="false">
                    <asp:Button ID="TBox80" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton80" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell81" runat="server" Visible="false">
                    <asp:Button ID="TBox81" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton81" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell82" runat="server" Visible="false">
                    <asp:Button ID="TBox82" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton82" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell83" runat="server" Visible="false">
                    <asp:Button ID="TBox83" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton83" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell84" runat="server" Visible="false">
                    <asp:Button ID="TBox84" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton84" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow14" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell79" runat="server" Visible="false">
                    <asp:Button ID="TextBox79" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton79" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell80" runat="server" Visible="false">
                    <asp:Button ID="TextBox80" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton80" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell81" runat="server" Visible="false">
                    <asp:Button ID="TextBox81" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton81" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell82" runat="server" Visible="false">
                    <asp:Button ID="TextBox82" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton82" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell83" runat="server" Visible="false">
                    <asp:Button ID="TextBox83" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton83" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell84" runat="server" Visible="false">
                    <asp:Button ID="TextBox84" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton84" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios14" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios79" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios79" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios80" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios80" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios81" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios81" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios82" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios82" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios83" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios83" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios84" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios84" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row15" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell85" runat="server" Visible="false">
                    <asp:Button ID="TBox85" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton85" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell86" runat="server" Visible="false">
                    <asp:Button ID="TBox86" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton86" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell87" runat="server" Visible="false">
                    <asp:Button ID="TBox87" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton87" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell88" runat="server" Visible="false">
                    <asp:Button ID="TBox88" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton88" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell89" runat="server" Visible="false">
                    <asp:Button ID="TBox89" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton89" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell90" runat="server" Visible="false">
                    <asp:Button ID="TBox90" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton90" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow15" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell85" runat="server" Visible="false">
                    <asp:Button ID="TextBox85" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton85" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell86" runat="server" Visible="false">
                    <asp:Button ID="TextBox86" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton86" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell87" runat="server" Visible="false">
                    <asp:Button ID="TextBox87" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton87" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell88" runat="server" Visible="false">
                    <asp:Button ID="TextBox88" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton88" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell89" runat="server" Visible="false">
                    <asp:Button ID="TextBox89" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton89" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell90" runat="server" Visible="false">
                    <asp:Button ID="TextBox90" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton90" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios15" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios85" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios85" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios86" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios86" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios87" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios87" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios88" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios88" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios89" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios89" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios90" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios90" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row16" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell91" runat="server" Visible="false">
                    <asp:Button ID="TBox91" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton91" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell92" runat="server" Visible="false">
                    <asp:Button ID="TBox92" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton92" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell93" runat="server" Visible="false">
                    <asp:Button ID="TBox93" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton93" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell94" runat="server" Visible="false">
                    <asp:Button ID="TBox94" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton94" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell95" runat="server" Visible="false">
                    <asp:Button ID="TBox95" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton95" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell96" runat="server" Visible="false">
                    <asp:Button ID="TBox96" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton96" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow16" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell91" runat="server" Visible="false">
                    <asp:Button ID="TextBox91" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton91" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell92" runat="server" Visible="false">
                    <asp:Button ID="TextBox92" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton92" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell93" runat="server" Visible="false">
                    <asp:Button ID="TextBox93" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton93" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell94" runat="server" Visible="false">
                    <asp:Button ID="TextBox94" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton94" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell95" runat="server" Visible="false">
                    <asp:Button ID="TextBox95" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton95" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell96" runat="server" Visible="false">
                    <asp:Button ID="TextBox96" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton96" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios16" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios91" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios91" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios92" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios92" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios93" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios93" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios94" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios94" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios95" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios95" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios96" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios96" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>

            <asp:TableRow ID="Row17" runat="server" BackColor="Transparent">
                <asp:TableCell ID="Cell97" runat="server" Visible="false">
                    <asp:Button ID="TBox97" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton97" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell98" runat="server" Visible="false">
                    <asp:Button ID="TBox98" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton98" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell99" runat="server" Visible="false">
                    <asp:Button ID="TBox99" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton99" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell100" runat="server" Visible="false">
                    <asp:Button ID="TBox100" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton100" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell101" runat="server" Visible="false">
                    <asp:Button ID="TBox101" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton101" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
                <asp:TableCell ID="Cell102" runat="server" Visible="false">
                    <asp:Button ID="TBox102" Width="100px" BackColor="transparent"
                        runat="server" BorderStyle="None" OnClick="Top_OnClick"
                        Style="position: absolute; overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; text-align: center; vertical-align: middle;"></asp:Button>

                    <asp:ImageButton ID="IButton102" runat="server"
                        ImageUrl="~/Configuracion/Images/TextoUCS.jpg" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="UserTRow17" runat="server" VerticalAlign="Top">
                <asp:TableCell ID="UserCell97" runat="server" Visible="false">
                    <asp:Button ID="TextBox97" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton97" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell98" runat="server" Visible="false">
                    <asp:Button ID="TextBox98" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton98" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell99" runat="server" Visible="false">
                    <asp:Button ID="TextBox99" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton99" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell100" runat="server" Visible="false">
                    <asp:Button ID="TextBox100" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton100" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell101" runat="server" Visible="false">
                    <asp:Button ID="TextBox101" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton101" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
                <asp:TableCell ID="UserCell102" runat="server" Visible="false">
                    <asp:Button ID="TextBox102" BackColor="Transparent" runat="server"
                        BorderStyle="None" OnClick="UCS_OnClick"
                        Style="overflow: hidden; font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 101px; text-align: center; vertical-align: middle; position: absolute; text-overflow: ellipsis"></asp:Button>

                    <asp:ImageButton ID="ImageButton102" runat="server"
                        ImageUrl="~/Configuracion/Images/UCS.jpg" OnClick="UCS_OnClick" />

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="RowUsuarios17" runat="server" BackColor="Coral">
                <asp:TableCell ID="CellUsuarios97" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios97" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="false"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios98" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios98" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios99" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios99" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios100" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios100" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />


                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios101" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios101" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
                <asp:TableCell ID="CellUsuarios102" runat="server" Visible="false">
                    <div style="height: 75px; overflow: hidden; width: 100px">
                        <asp:ListBox ID="BUsuarios102" runat="server" BorderStyle="None"
                            BackColor="Coral" Visible="true"
                            Style="height: 75px; width: 100px; font-family: Tahoma; font-size:x-small; overflow: hidden"
                            SelectionMode="Multiple" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="10px" runat="server"></asp:TableRow>
        </asp:Table>
    </div>

    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TBNuevo"
        ErrorMessage="Rellene el campo Identificador" Style="left: 410px; position: absolute;
        top: 90px; z-index: 112; height: 19px;" Visible="False"  SetFocusOnError="True" 
		 meta:resourcekey="RequiredFieldValidator1Resource1">*</asp:RequiredFieldValidator>
      
      
    <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Groove"
            BorderWidth="1px" Height="151px" Style="left: 320px; position: absolute;
            top: 195px; z-index: 113;" Visible="False" Width="150px" 
        	meta:resourcekey="Panel1Resource1">
        <asp:Button ID="BtLiberar" runat="server" Style="left: 17px; position: absolute;
            top: 65px" Text="Liberar" Width="120px" OnClick="BtLiberar_Click" 
			  meta:resourcekey="BtLiberarResource1"  Enabled="true"/>
        <asp:Button ID="BtCancel" runat="server" Style="left: 17px; position: absolute;
            top: 121px" Text="Cancelar" Width="120px" OnClick="BtCancelPanel_Click" 
			  meta:resourcekey="BtCancelResource1"  Enabled="true"/>
        <asp:Button ID="BtAsignar" runat="server" Style="left: 17px; position: absolute;
            top: 9px" Text="Asignar" Width="120px" OnClick="BtAsignar_Click" 
			  meta:resourcekey="BtAsignarResource1"  Enabled="true"/>
        <asp:Button ID="BtExplorar" runat="server" Style="left: 17px; position: absolute;
            top: 93px" Text="Ver Panel" Width="120px" OnClick="VerPanel_Click" 
			  meta:resourcekey="BtExplorarResource1" Enabled="true" />
        <asp:Button ID="BtCambiar" runat="server" Style="left: 17px; position: absolute;
            top: 37px" Text="Cambiar de Top" Width="120px" OnClick="Cambiar_Click" 
			  meta:resourcekey="BtCambiarResource1"  Enabled="true"/>
    </asp:Panel>

    <asp:Panel ID="PanelTop" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Groove"
        BorderWidth="1px" Height="95px" Style="left: 319px; position: absolute; z-index:114;
        top: 228px;" Visible="False" Width="150px" 
		 meta:resourcekey="PanelTopResource1">
        <asp:Button ID="BtnIntercambiarTop" runat="server" Style="left: 17px; position: absolute;
            top: 9px" Text="Intercambiar Top" Width="120px" 
			  OnClick="BtnIntercambiarTop_Click" 
			  meta:resourcekey="BtnIntercambiarTopResource1" />
        <asp:Button ID="BtnLiberarTop" runat="server" Style="left: 17px; position: absolute;
            top: 37px" Text="Liberar Top" Width="120px" OnClick="BtnLiberarTop_Click" 
			  meta:resourcekey="BtnLiberarTopResource1" />
        <asp:Button ID="Button2" runat="server" Style="left: 17px; position: absolute;
            top: 65px" Text="Cancelar" Width="120px" OnClick="BtnCancelarTop_Click" 
			  meta:resourcekey="Button2Resource2" />
    </asp:Panel>
    
    <asp:Panel ID="PanelSectores" runat="server" BackColor="White" BorderColor="Black"
        BorderStyle="Solid" BorderWidth="1px" ForeColor="Transparent"
        Style="left: 257px; position: absolute; top: 150px; z-index: 115; height: 288px;" Visible="False"
        Width="246px" meta:resourcekey="PanelSectoresResource1">
        <asp:ListBox ID="LBoxSectores" runat="server" 
			  Style="left: 18px; position: absolute; top: 13px; height: 173px; width: 203px;" 
			  meta:resourcekey="LBoxSectoresResource1"></asp:ListBox>
        <asp:Button ID="BtAsignaSector" runat="server" OnClick="BtAsignaSector_Click" 
			  Style="left: 81px; position: absolute; top: 214px" Text="Asignar" 
			  Width="80px" meta:resourcekey="BtAsignaSectorResource1" />
		  <asp:Button ID="BtCerrar" runat="server" OnClick="BtCerrar_Click" 
			  Style="left: 81px; position: absolute; top: 250px" Text="Cerrar" Width="80px" 
			  meta:resourcekey="Button2Resource2" />
    </asp:Panel>
    <asp:Panel ID="PanelNucleo" runat="server" BackColor="White" BorderColor="Black"
        BorderStyle="Solid" BorderWidth="1px" Height="85px" Style="left: 267px;
        position: absolute; top: 229px; z-index: 116;" Visible="False" 
		 Width="227px" meta:resourcekey="PanelNucleoResource1">
        <asp:Label ID="Label1" runat="server" Style="left: 9px; position: absolute;
            top: 17px" Text="Núcleo:" meta:resourcekey="Label1Resource1"></asp:Label>
        <asp:DropDownList ID="DListNucleo" runat="server" Style="left: 70px;
            position: absolute; top: 16px" Width="147px"  class="select"
			  meta:resourcekey="DListNucleoResource1">
        </asp:DropDownList>
        <asp:Button ID="BtAceptarNucleo" runat="server" OnClick="BtAceptarNucleo_Click" 
			  Style="left: 78px; position: absolute; top: 52px" Text="Aceptar" Width="80px" 
			  meta:resourcekey="BtAceptarNucleoResource1" />
    </asp:Panel>
&nbsp;
	<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >--%>
		<div>
			<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" X="290" Y="114"
				BackgroundCssClass="modalBackground" Drag="False" DropShadow="True" 
				 Enabled="True" PopupControlID="PnlParametrosSector" 
				PopupDragHandleControlID="PersonCaption" TargetControlID="BtnOculto">
			</asp:ModalPopupExtender>
		
			<asp:Panel ID="PnlParametrosSector" runat="server" CssClass="modalBox" 
				Width="765px" Height="700px"
						 Style="cursor: default; display:none;" 
				meta:resourcekey="PnlParametrosSectorResource1" >
				<asp:Panel ID="PersonCaption" runat="server" CssClass="caption" 
					Style="margin-bottom: 5px; cursor: default;" 
					meta:resourcekey="PersonCaptionResource1">
							Sector
				</asp:Panel>
                <asp:Panel ID="UP2" runat="server" Width="755px"
                    meta:resourcekey="UP2Resource1">
                    <asp:Table ID="TPanelSector" runat="server" Width="750px"
                        meta:resourcekey="TPanelSectorResource1">
                        <asp:TableRow ID="TableRow8" runat="server">
                            <asp:TableCell ID="TableCell46" runat="server"></asp:TableCell>
                            <asp:TableCell ID="TableCell47" runat="server" HorizontalAlign="Center">
                                <asp:Label ID="LblSector"
                                    runat="server" Font-Bold="True"
                                    ForeColor="LightSkyBlue"
                                    Visible="False" Width="151px" />
                            </asp:TableCell><asp:TableCell ID="TableCell48" runat="server" HorizontalAlign="Right">
                                <asp:ImageButton
                                    ID="BtnVolverASectorizacion" runat="server" Visible="False"
                                    OnClick="BtnVolverASectorizacion_OnClick"
                                    ImageUrl="~/App_Themes/TemaPaginasConfiguracion/Cerrar.png" />
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="TableRow9" runat="server">
                            <asp:TableCell ID="TableCell49"
                                runat="server" ColumnSpan="4">
                                <asp:UpdatePanel ID="UPParametros" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="IBPropiedadesGenerales" runat="server"
                                            ImageUrl="~/Configuracion/Images/MenuSectorPropGeneralesSelected.JPG"
                                            OnClick="OnButtonImageMenu_Click" Visible="False" />

                                        <asp:ImageButton ID="IBUtilidades" runat="server"
                                            ImageUrl="~/Configuracion/Images/MenuSectorUtilidadesUnSelected.JPG"
                                            OnClick="OnButtonImageMenu_Click" Visible="False" />

                                        <asp:ImageButton ID="IBParametros" runat="server"
                                            ImageUrl="~/Configuracion/Images/MenuSectorParametrosUnSelected.JPG"
                                            OnClick="OnButtonImageMenu_Click" Visible="False" />

                                        <asp:ImageButton ID="IBNivelesIntrusion" runat="server"
                                            ImageUrl="~/Configuracion/Images/MenuSectorNivelesIntrusionUnSelected.JPG"
                                            OnClick="OnButtonImageMenu_Click" Visible="False" />

                                        <asp:ImageButton ID="IBPermisosRedes" runat="server"
                                            ImageUrl="~/Configuracion/Images/MenuSectorPermisosRedesUnSelected.JPG"
                                            OnClick="OnButtonImageMenu_Click" Visible="False" />

                                        <asp:Panel ID="PanelParametrosSector" runat="server" Width="750px"
                                            BorderStyle="Inset">
                                            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                                <asp:View ID="View1" runat="server">
                                                    <asp:Table ID="TView1" runat="server">
                                                        <asp:TableRow
                                                            ID="TableRow14" runat="server">
                                                            <asp:TableCell
                                                                ID="TableCell50" runat="server">
                                                                <asp:Label
                                                                    ID="Label5" runat="server" Text="Identificador:"
                                                                    meta:resourcekey="Label5Resource1"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell65" runat="server">
                                                                <asp:TextBox ID="TxtIdSector"
                                                                    runat="server" Enabled="False"></asp:TextBox>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell66" runat="server" RowSpan="6">
                                                                <asp:Panel
                                                                    ID="PanelAbonado" runat="server" BorderStyle="Inset" Height="139px"
                                                                    Width="268px">
                                                                    <asp:Label ID="Label18" runat="server" Text="Abonados a los que atiende:" meta:resourcekey="Label18Resource1"></asp:Label>

                                                                    <asp:GridView ID="GVAbonados" runat="server"
                                                                        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="2"
                                                                        AllowPaging="True" Height="18px" Width="257px" OnPageIndexChanging="GVAbonados_OnSelectedIndexChanging">
                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                        <AlternatingRowStyle BackColor="White" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="IdRed" HeaderText="Red"
                                                                                meta:resourcekey="BoundFieldResource1" />
                                                                            <asp:BoundField DataField="IdAbonado" HeaderText="N&#250;mero Abonado"
                                                                                meta:resourcekey="BoundFieldResource2" />
                                                                            <asp:BoundField DataField="IdPrefijo" Visible="False"
                                                                                meta:resourcekey="BoundFieldResource3" />
                                                                        </Columns>

                                                                        <EditRowStyle BackColor="#2461BF" />

                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="False" ForeColor="White" Height="15px" />

                                                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />

                                                                        <RowStyle BackColor="#EFF3FB" />

                                                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    </asp:GridView>

                                                                </asp:Panel>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow ID="TableRow19" runat="server">
                                                            <asp:TableCell
                                                                ID="TableCell74" runat="server">
                                                                <asp:Label
                                                                    ID="Label19" runat="server" Text="Tipo:" meta:resourcekey="Label19Resource1"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell75" runat="server">
                                                                <asp:DropDownList  class="select"
                                                                    ID="DListTipoSector" runat="server" Enabled="False" AutoPostBack="True">
                                                                    <asp:ListItem Value="R"
                                                                        meta:resourcekey="ListItemResource1">Real</asp:ListItem>
                                                                    <asp:ListItem Value="V" meta:resourcekey="ListItemResource2">Virtual</asp:ListItem>
                                                                    <asp:ListItem Value="M" meta:resourcekey="ListItemResource3">Mantenimiento</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell76" runat="server"></asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow ID="TableRow16" runat="server">
                                                            <asp:TableCell
                                                                ID="TableCell63" runat="server">
                                                                <asp:Label
                                                                    ID="Label16" runat="server" Text="Núcleo:" meta:resourcekey="Label16Resource1"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell81" runat="server">
                                                                <asp:DropDownList  class="select"
                                                                    ID="DListNucleoParametros" runat="server" Width="110px" Enabled="False">
                                                                </asp:DropDownList>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow ID="TableRow17" runat="server">
                                                            <asp:TableCell
                                                                ID="TableCell68" runat="server">
                                                                <asp:Label
                                                                    ID="Label2" runat="server" Text="Tipo de Posición:"
                                                                    meta:resourcekey="Label2Resource1"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell69" runat="server">
                                                                <asp:DropDownList class="select"
                                                                    ID="DListTipoPosicion" runat="server" Width="110px" Enabled="False">
                                                                    <asp:ListItem Value="C"
                                                                        meta:resourcekey="ListItemResource4">Controlador</asp:ListItem>
                                                                    <asp:ListItem Value="P" meta:resourcekey="ListItemResource5">Planificador</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow ID="TableRow18" runat="server">
                                                            <asp:TableCell
                                                                ID="TableCell71" runat="server">
                                                                <asp:Label
                                                                    ID="Label13" runat="server" Text="Prioridad R2:" meta:resourcekey="Label13Resource1"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell72" runat="server">
                                                                <asp:DropDownList class="select"
                                                                    ID="DDLPrioridadR2" runat="server" Enabled="False">
                                                                    <asp:ListItem Value="4"
                                                                        meta:resourcekey="ListItemResource6">No Urgente</asp:ListItem>
                                                                    <asp:ListItem Value="3" meta:resourcekey="ListItemResource7">Normal</asp:ListItem>
                                                                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource8">Urgente</asp:ListItem>
                                                                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource9">Emergencia</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow ID="TableRow15" runat="server">
                                                            <asp:TableCell
                                                                ID="TableCell51" runat="server">
                                                                <asp:CheckBox
                                                                    ID="CheckIntrusion" runat="server" Checked="True" Text="Intrusión"
                                                                    meta:resourcekey="CheckIntrusionResource1" Enabled="False" />
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell61" runat="server">
                                                                <asp:CheckBox ID="CheckIntruido"
                                                                    runat="server" Checked="True" Text="Intruido" Enabled="False"
                                                                    meta:resourcekey="CheckIntruidoResource1" />
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>

                                                </asp:View>
                                                <asp:View ID="View2" runat="server">
                                                    <div runat="server" style="height: 200px; width: auto; overflow: auto">
                                                        <asp:Table runat="server">
                                                            <asp:TableRow
                                                                runat="server">
                                                                <asp:TableCell
                                                                    ID="TableCell67" runat="server" Width="250px">
                                                                    <asp:CheckBox ID="CheckConferencia"
                                                                        runat="server" Text="Conferencia" Enabled="False"
                                                                        meta:resourcekey="CheckConferenciaResource1" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell70" runat="server">
                                                                    <asp:CheckBox ID="CheckEscucha"
                                                                        runat="server" Text="Escucha" Enabled="False"
                                                                        meta:resourcekey="CheckEscuchaResource1" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow20" runat="server">
                                                                <asp:TableCell ID="TableCell78"
                                                                    runat="server">
                                                                    <asp:CheckBox
                                                                        ID="CheckRedireccion" runat="server" Text="Redirección" Enabled="False"
                                                                        meta:resourcekey="CheckRedireccionResource1" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell79" runat="server">
                                                                    <asp:CheckBox ID="CheckRepUltLlamada"
                                                                        runat="server" Checked="True" Text="Repetición última llamada" Enabled="False"
                                                                        meta:resourcekey="CheckRepUltLlamadaResource1" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow21" runat="server">
                                                                <asp:TableCell ID="TableCell83"
                                                                    runat="server">
                                                                    <asp:CheckBox
                                                                        ID="CheckTecla55" runat="server" Text="Tecla 55+1" Enabled="False"
                                                                        meta:resourcekey="CheckTecla55Resource1" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell84" runat="server">
                                                                    <asp:CheckBox ID="CheckMonitoring"
                                                                        runat="server" Checked="True" Text="Monitoring" Enabled="False"
                                                                        meta:resourcekey="CheckMonitoringResource1" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow22" runat="server">
                                                                <asp:TableCell ID="TableCell87"
                                                                    runat="server">
                                                                    <asp:CheckBox
                                                                        ID="CheckIntRDTF" runat="server" Text="Integr. Radio-Telefonía"
                                                                        Enabled="False" meta:resourcekey="CheckIntRDTFResource1" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell88" runat="server">
                                                                    <asp:CheckBox ID="CheckCoorRD"
                                                                        runat="server" Text="Coordinador Radio" Enabled="False"
                                                                        meta:resourcekey="CheckCoorRDResource1" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow23" runat="server">
                                                                <asp:TableCell ID="TableCell91"
                                                                    runat="server">
                                                                    <asp:CheckBox
                                                                        ID="CheckLTT" runat="server" Checked="True" Text="LTT" Enabled="False"
                                                                        meta:resourcekey="CheckLTTResource1" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell92" runat="server">
                                                                    <asp:CheckBox ID="CheckRediCA"
                                                                        runat="server" Text="Inhabilitación Redirección C/A" Enabled="False"
                                                                        meta:resourcekey="CheckRediCAResource1" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow runat="server">
                                                                <asp:TableCell ID="TableCell73" runat="server">
                                                                    <asp:CheckBox ID="CheckRetener"
                                                                        runat="server" Checked="True" Text="Retener" Enabled="False"
                                                                        meta:resourcekey="CheckRetenerResource1" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell77" runat="server">
                                                                    <asp:CheckBox ID="CheckCaptura"
                                                                        runat="server" Text="Captura" Enabled="False"
                                                                        meta:resourcekey="CheckCapturaResource1" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow runat="server">
                                                                <asp:TableCell ID="TableCell80" runat="server">
                                                                    <asp:CheckBox ID="CheckReAutomatica"
                                                                        runat="server" Text="Rellamada automática" Enabled="False"
                                                                        meta:resourcekey="CheckReAutomaticaResource1" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell82" runat="server">
                                                                    <asp:CheckBox ID="CheckTeclaPrio"
                                                                        runat="server" Checked="True" Text="Tecla de prioridad" Enabled="False"
                                                                        meta:resourcekey="CheckTeclaPrioResource1" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow runat="server">
                                                                <asp:TableCell ID="TableCell85" runat="server">
                                                                    <asp:CheckBox ID="CheckCoordTF"
                                                                        runat="server" Text="Coordinador Telefonía" Enabled="False"
                                                                        meta:resourcekey="CheckCoordTFResource1" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell86" runat="server">
                                                                    <asp:CheckBox ID="CheckLlamadaSelect"
                                                                        runat="server" Text="Llamada Selectiva" Enabled="False"
                                                                        meta:resourcekey="CheckLlamadaSelectResource1" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow runat="server">
                                                                <asp:TableCell ID="TableCell89" runat="server">
                                                                    <asp:CheckBox ID="CheckTransDirect"
                                                                        runat="server" Text="Transferencia Directa" Enabled="False"
                                                                        meta:resourcekey="CheckTransDirectResource1" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell90" runat="server">
                                                                    <asp:CheckBox ID="CheckBSS"
                                                                        runat="server" Checked="True" Text="Grupo BSS" Enabled="False"
                                                                        meta:resourcekey="CheckBssResource1" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow runat="server">
                                                                <asp:TableCell ID="TableCell93" runat="server">
                                                                    <asp:CheckBox ID="CheckSayAgain"
                                                                        runat="server" Checked="True" Text="Say Again" Enabled="False"
                                                                        meta:resourcekey="CheckSayAgainResource1" />
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell94" runat="server">
                                                                    <asp:CheckBox ID="CheckTransPre"
                                                                        runat="server" Checked="True" Text="Transferencia con consulta previa"
                                                                        meta:resourcekey="CheckTransPreResource1" Enabled="False" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>

                                                    </div>

                                                </asp:View>
                                                <asp:View ID="View3" runat="server">
                                                    <div id="Div1" runat="server" style="height: 200px; width: auto; overflow: auto">
                                                        <asp:Table runat="server">
                                                            <asp:TableRow
                                                                runat="server">
                                                                <asp:TableCell
                                                                    ID="TableCell95" runat="server">
                                                                    <asp:Label
                                                                        ID="LblKAP" runat="server" Text="Keep Alive Period (ms):"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell96" runat="server">
                                                                    <asp:TextBox ID="TxtKAP"
                                                                        runat="server" Enabled="False">200</asp:TextBox>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow24" runat="server">
                                                                <asp:TableCell ID="TableCell97"
                                                                    runat="server">
                                                                    <asp:Label
                                                                        ID="LblKAM" runat="server" Text="Keep Alive Multiplier:"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell98" runat="server">
                                                                    <asp:TextBox ID="TxtKAM"
                                                                        runat="server" Enabled="False">10</asp:TextBox>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow25" runat="server">
                                                                <asp:TableCell ID="TableCell99"
                                                                    runat="server">
                                                                    <asp:Label
                                                                        ID="Label8" runat="server" Text="Número llamadas entrantes en cola IDA:"
                                                                        meta:resourcekey="Label8Resource1"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell100" runat="server">
                                                                    <asp:TextBox ID="TxtLlamEntIDA"
                                                                        runat="server" Enabled="False">3</asp:TextBox>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow26" runat="server">
                                                                <asp:TableCell ID="TableCell101"
                                                                    runat="server">
                                                                    <asp:Label
                                                                        ID="Label9" runat="server" Text="Número llamadas IDA:" Width="154px"
                                                                        meta:resourcekey="Label9Resource1"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell102" runat="server">
                                                                    <asp:TextBox ID="TxtLlamIDA"
                                                                        runat="server" Enabled="False">4</asp:TextBox>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow27" runat="server">
                                                                <asp:TableCell ID="TableCell103"
                                                                    runat="server">
                                                                    <asp:Label
                                                                        ID="Label10" runat="server" Text="Número de Frecuencias por Página:"
                                                                        meta:resourcekey="Label10Resource1"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell104" runat="server">
                                                                    <asp:TextBox ID="TxtFrecPag"
                                                                        runat="server" Enabled="False">12</asp:TextBox>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow28" runat="server">
                                                                <asp:TableCell ID="TableCell105"
                                                                    runat="server">
                                                                    <asp:Label
                                                                        ID="Label11" runat="server" Text="Núm. de Pág. de Frec.:"
                                                                        meta:resourcekey="Label11Resource1"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell106" runat="server">
                                                                    <asp:TextBox ID="TxtPagFrec"
                                                                        runat="server" Enabled="False">9</asp:TextBox>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow29" runat="server">
                                                                <asp:TableCell ID="TableCell107"
                                                                    runat="server">
                                                                    <asp:Label
                                                                        ID="Label12" runat="server" Text="Número de Enlaces Internos por Página:"
                                                                        meta:resourcekey="Label12Resource1"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell108" runat="server">
                                                                    <asp:TextBox ID="TxtEnlIntPag"
                                                                        runat="server" Enabled="False">19</asp:TextBox>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="TableRow30" runat="server">
                                                                <asp:TableCell ID="TableCell109"
                                                                    runat="server">
                                                                    <asp:Label
                                                                        ID="Label14" runat="server" Text="Núm. Pág. Enlaces Int.:"
                                                                        meta:resourcekey="Label14Resource1"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell110" runat="server">
                                                                    <asp:TextBox ID="TxtPagEnlInt"
                                                                        runat="server" Enabled="False">3</asp:TextBox>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>

                                                    </div>

                                                </asp:View>
                                                <asp:View ID="View4" runat="server">
                                                    <asp:Table runat="server">
                                                        <asp:TableRow ID="TableRow31" runat="server" Height="75px">
                                                            <asp:TableCell ID="TableCell111"
                                                                runat="server" Width="50px">
                                                                <asp:Label
                                                                    ID="Label21" runat="server" Text="CICL:"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell112" runat="server" Width="200px">
                                                                <asp:DropDownList class="select"
                                                                    ID="DDLCicl" runat="server" Enabled="False">
                                                                    <asp:ListItem
                                                                        Value="0" meta:resourcekey="ListItemResource10">No Urgente</asp:ListItem>
                                                                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource11">Normal</asp:ListItem>
                                                                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource12">Urgente</asp:ListItem>
                                                                    <asp:ListItem Value="3" meta:resourcekey="ListItemResource13">Emergencia</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell113" runat="server" Width="50px">
                                                                <asp:Label ID="Label4"
                                                                    runat="server" Text="CPIPCL:"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell114" runat="server" Width="200px">
                                                                <asp:DropDownList class="select"
                                                                    ID="DDLCpipcl" runat="server" Enabled="False">
                                                                    <asp:ListItem Value="0"
                                                                        meta:resourcekey="ListItemResource14">No Urgente</asp:ListItem>
                                                                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource15">Normal</asp:ListItem>
                                                                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource16">Urgente</asp:ListItem>
                                                                    <asp:ListItem Value="3" meta:resourcekey="ListItemResource17">Emergencia</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow ID="TableRow33" runat="server" Height="75px">
                                                            <asp:TableCell
                                                                ID="TableCell115" runat="server">
                                                                <asp:Label
                                                                    ID="Label3" runat="server" Text="CIPL:"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell116" runat="server">
                                                                <asp:DropDownList ID="DDLCipl" class="select"
                                                                    runat="server" Enabled="False">
                                                                    <asp:ListItem
                                                                        Value="0" meta:resourcekey="ListItemResource18">No Urgente</asp:ListItem>
                                                                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource19">Normal</asp:ListItem>
                                                                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource20">Urgente</asp:ListItem>
                                                                    <asp:ListItem Value="3" meta:resourcekey="ListItemResource21">Emergencia</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell117" runat="server">
                                                                <asp:Label ID="Label7"
                                                                    runat="server" Text="CPICL:"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell ID="TableCell118" runat="server">
                                                                <asp:DropDownList ID="DDLCpicl" class="select"
                                                                    runat="server" Enabled="False">
                                                                    <asp:ListItem
                                                                        Value="0" meta:resourcekey="ListItemResource22">No Urgente</asp:ListItem>
                                                                    <asp:ListItem Value="1" meta:resourcekey="ListItemResource23">Normal</asp:ListItem>
                                                                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource24">Urgente</asp:ListItem>
                                                                    <asp:ListItem Value="3" meta:resourcekey="ListItemResource25">Emergencia</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>

                                                </asp:View>
                                                <asp:View ID="View5" runat="server">
                                                    <div id="DivPermisos" runat="server" style="height: auto; width: auto;">
                                                        <asp:Table ID="TblPermisos" runat="server">
                                                            <asp:TableRow ID="RowPermisos1" runat="server">
                                                                <asp:TableCell ID="CellPermisos1" runat="server" Width="250px"></asp:TableCell>
                                                                <asp:TableCell ID="CellPermisos2" runat="server" Width="250px">
                                                                    <asp:GridView ID="GVPermisosRedes" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                                                                        GridLines="None" PageSize="5"
                                                                        AllowPaging="True"
                                                                        Height="159px" Width="257px" Style="left: 150px"
                                                                        meta:resourcekey="GVPermisosRedesResource1">
                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <AlternatingRowStyle BackColor="White" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="IdRed" HeaderText="Red"
                                                                                meta:resourcekey="BoundFieldResource7" />
                                                                            <asp:CheckBoxField DataField="Llamar" HeaderText="Llamar"
                                                                                meta:resourcekey="BoundFieldResource8" />
                                                                            <asp:CheckBoxField DataField="Recibir" HeaderText="Recibir"
                                                                                meta:resourcekey="BoundFieldResource9" />
                                                                        </Columns>
                                                                        <EditRowStyle BackColor="#2461BF" />
                                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="False" ForeColor="White"
                                                                            Height="15px" />
                                                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                                        <RowStyle BackColor="#EFF3FB" />
                                                                    </asp:GridView>
                                                                </asp:TableCell>
                                                                <asp:TableCell ID="TableCell43" runat="server"></asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </div>
                                                </asp:View>
                                            </asp:MultiView>

                                        </asp:Panel>


                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                            <asp:TableCell
                                ID="TableCell64" runat="server" ColumnSpan="4" HorizontalAlign="Center">
                                <asp:UpdatePanel
                                    ID="UPPanelesTeclas" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="LBPanelTel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="LBPanelRadio" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="LBPanelLC" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div runat="server" style="width: 760px; height: auto;">
                                            <asp:Table ID="TEnlacesLC" runat="server"
                                                Visible="False">
                                                <asp:TableRow
                                                    ID="FilaLC1" runat="server">
                                                    <asp:TableCell ID="TableCellLC21" Visible="false" CssClass="textbox"
                                                        runat="server" Width="80px" Height="30px">
                                                        <asp:TextBox ID="TextBoxLC21"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC21" runat="server" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC22" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC22"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC22" runat="server" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC23" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC23"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC23" runat="server" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC24" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC24"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC24" runat="server" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC25" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC25"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC25" runat="server" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC26" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC26"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC26" runat="server" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC27" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC27"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC27" runat="server" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC28" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC28"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC28" runat="server" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC29" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC29"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC29" runat="server" />--%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRowLC2" runat="server">
                                                    <asp:TableCell ID="TableCellLC11" CssClass="textbox" 
                                                        runat="server" Width="80px" Height="30px" Visible="false">
                                                        <asp:TextBox ID="TextBoxLC11"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC11" runat="server"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC12" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC12"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC12" runat="server" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC13" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC13"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC13" runat="server"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC14" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC14"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC14" runat="server"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC15" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC15"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC15" runat="server"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC16" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC16"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC16" runat="server"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC17" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC17"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC17" runat="server"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC18" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC18"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC18" runat="server" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="TableCellLC19" runat="server" Width="80px" Height="30px" Visible="false" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxLC19"
                                                            BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            SkinID="LiteralTeclaPanelLC" MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonLC19" runat="server" />--%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>


                                            <asp:Table ID="TEnlacesInternos"
                                                runat="server" BackColor="Transparent" Visible="False" Width="320px">
                                                <asp:TableRow ID="Fila1"
                                                    runat="server">
                                                    <asp:TableCell
                                                        ID="C11" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB11" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
<%--                                                        <asp:ImageButton ID="IB11" runat="server" CausesValidation="False"
                                                            ImageUrl="~/Configuracion/Images/BotonEnlaceInterno.jpg" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C12" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB12" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
<%--                                                        <asp:ImageButton ID="IB12" runat="server" CausesValidation="False"
                                                            ImageUrl="~/Configuracion/Images/BotonEnlaceInterno.jpg" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C13" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB13" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
<%--                                                        <asp:ImageButton ID="IB13" runat="server" CausesValidation="False"
                                                            ImageUrl="~/Configuracion/Images/BotonEnlaceInterno.jpg" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C14" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB14" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle;"></asp:TextBox>
<%--                                                        <asp:ImageButton ID="IB14" runat="server" CausesValidation="False"
                                                            ImageUrl="~/Configuracion/Images/BotonEnlaceInterno.jpg" />--%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="Fila2" runat="server">
                                                    <asp:TableCell ID="C21" runat="server" CssClass="textbox"
                                                        Width="80px" Height="30px" Visible="False">
                                                        <asp:TextBox
                                                            ID="TB21" runat="server" BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB21" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C22" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB22" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB22" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C23" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB23" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB23" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C24" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB24" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB24" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="Fila3" runat="server">
                                                    <asp:TableCell ID="C31" runat="server" CssClass="textbox"
                                                        Width="80px" Height="30px" Visible="False">
                                                        <asp:TextBox
                                                            ID="TB31" runat="server" BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB31" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C32" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB32" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB32" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C33" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB33" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB33" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C34" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB34" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB34" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="Fila4" runat="server">
                                                    <asp:TableCell ID="C41" runat="server" CssClass="textbox"
                                                        Width="80px" Height="30px" Visible="False">
                                                        <asp:TextBox
                                                            ID="TB41" runat="server" BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB41" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C42" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB42" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB42" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C43" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB43" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB43" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C44" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB44" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB44" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="TableRow32" runat="server">
                                                    <asp:TableCell ID="C51" runat="server" CssClass="textbox"
                                                        Width="80px" Height="30px" Visible="False">
                                                        <asp:TextBox
                                                            ID="TB51" runat="server" BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB41" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C52" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB52" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB42" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C53" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB53" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB43" runat="server" CausesValidation="False"/>--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="C54" runat="server" Width="80px" Height="30px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TB54" runat="server"
                                                            BackColor="Transparent" BorderStyle="None"
                                                            MaxLength="32" ReadOnly="True" Rows="3" Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 80px; text-align: center; vertical-align: middle"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="IB44" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>


                                            <asp:Table ID="TEnlacesRadio"
                                                runat="server" BackColor="Transparent" Visible="False" Width="418px">
                                                <asp:TableRow ID="FilaRadio1"
                                                    runat="server">
                                                    <asp:TableCell
                                                        ID="CeldaRadio11" runat="server" Width="103px" Height="60px" Visible="False" CssClass="textbox">
                                                        <asp:TextBox ID="TextBoxRadio11"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio11" runat="server"
                                                            ImageUrl="~/Configuracion/Images/BotonEnlaceExterno.jpg"
                                                            CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio12" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio12" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio12" runat="server"
                                                            ImageUrl="~/Configuracion/Images/BotonEnlaceExterno.jpg"
                                                            CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio13" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio13" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio13" runat="server"
                                                            ImageUrl="~/Configuracion/Images/BotonEnlaceExterno.jpg"
                                                            CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio14" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio14" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio14" runat="server"
                                                            ImageUrl="~/Configuracion/Images/BotonEnlaceExterno.jpg"
                                                            CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio15" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio15" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio15" runat="server"
                                                            ImageUrl="~/Configuracion/Images/BotonEnlaceExterno.jpg"
                                                            CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="FilaRadio2" runat="server">
                                                    <asp:TableCell ID="CeldaRadio21" CssClass="textbox"
                                                        runat="server" Width="103px" Height="60px" Visible="False">
                                                        <asp:TextBox ID="TextBoxRadio21"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio21" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio22" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio22" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio22" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio23" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio23" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio23" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio24" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio24" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio24" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio25" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio25" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio25" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow ID="FilaRadio3" runat="server">
                                                    <asp:TableCell ID="CeldaRadio31" CssClass="textbox"
                                                        runat="server" Width="103px" Height="60px" Visible="False">
                                                        <asp:TextBox ID="TextBoxRadio31"
                                                            BackColor="Transparent" runat="server" ReadOnly="True" BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio31" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio32" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio32" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio32" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio33" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio33" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio33" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio34" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio34" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio34" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                    <asp:TableCell ID="CeldaRadio35" runat="server" Width="103px" Height="60px"
                                                        Visible="False" CssClass="textbox">
                                                        <asp:TextBox
                                                            ID="TextBoxRadio35" BackColor="Transparent" runat="server" ReadOnly="True"
                                                            BorderStyle="None" Rows="3"
                                                            Style="font-weight: bold; font-size: x-small; font-family: Tahoma; height: 30px; width: 103px; text-align: center; vertical-align: middle"
                                                            MaxLength="32"></asp:TextBox>
                                                        <%--<asp:ImageButton ID="ImageButtonRadio35" runat="server" CausesValidation="False" />--%>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>

                                        </div>

                                        <asp:Table runat="server">
                                            <asp:TableRow
                                                ID="TableRow11" runat="server" HorizontalAlign="Center">
                                                <asp:TableCell ID="TableCell53"
                                                    runat="server" ColumnSpan="3">
                                                    <asp:Label
                                                        ID="LblPanel" runat="server" ForeColor="LightSkyBlue" Width="152px" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow ID="TableRow12" runat="server">
                                                <asp:TableCell
                                                    ID="TableCell55" runat="server" HorizontalAlign="Center">
                                                    <asp:ImageButton ID="BtnPaginaMenos"
                                                        runat="server" ImageUrl="~/Configuracion/Images/arrowIzq.gif"
                                                        Visible="False" OnClick="BtnPaginaMenos_Click" />
                                                </asp:TableCell>
                                                <asp:TableCell ID="TableCell56" runat="server" HorizontalAlign="Center">
                                                    <asp:Label ID="LblPagina"
                                                        runat="server" ForeColor="LightSkyBlue" Width="91px" />
                                                </asp:TableCell>
                                                <asp:TableCell ID="TableCell57" runat="server" HorizontalAlign="Center">
                                                    <asp:ImageButton ID="BtnPaginaMas"
                                                        runat="server" ImageUrl="~/Configuracion/Images/arrowDech.gif"
                                                        Visible="False" OnClick="BtnPaginaMas_Click" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>


                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:TableCell></asp:TableRow><asp:TableRow ID="TableRow10" runat="server">
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow13" runat="server">
                            <asp:TableCell
                                ID="TableCell58" runat="server" HorizontalAlign="Center">
                                <asp:LinkButton ID="LBPanelTel"
                                    runat="server" OnClick="LBPanelTel_Click" Visible="False"
                                    meta:resourcekey="LBPanelTelResource1">Panel Telefónico</asp:LinkButton>
                            </asp:TableCell><asp:TableCell ID="TableCell59" runat="server" HorizontalAlign="Center">
                                <asp:LinkButton ID="LBPanelRadio"
                                    runat="server" OnClick="LBPanelRadio_Click" Visible="False"
                                    meta:resourcekey="LBPanelRadioResource1">Panel Radio</asp:LinkButton>
                            </asp:TableCell><asp:TableCell ID="TableCell60" runat="server" HorizontalAlign="Center">
                                <asp:LinkButton ID="LBPanelLC"
                                    runat="server" OnClick="LBPanelLC_Click" Visible="False"
                                    meta:resourcekey="LBPanelLCResource1">Panel Línea Caliente</asp:LinkButton>
                            </asp:TableCell></asp:TableRow></asp:Table></asp:Panel></asp:Panel><asp:Button ID="BtnOculto" runat="server" Text="Button" Style="display: none;" />
        </div>
	<%--</asp:UpdatePanel>--%>
</asp:Content>