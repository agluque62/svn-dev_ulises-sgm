using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.Security;
using System.Web.Configuration;
using log4net;
using log4net.Config;

public partial class InformeSectorizaciones : PageBaseCD40.PageCD40
{
    private static ServiciosCD40.ServiciosCD40 ServicioCD40 = new ServiciosCD40.ServiciosCD40();
    private static ILog _logDebugView;
    private static bool bPaginaCargada=false;

    public static ILog logDebugView
    {
        get
        {
            if (_logDebugView == null)
            {
                log4net.Config.XmlConfigurator.Configure();
                _logDebugView = LogManager.GetLogger("CONFIGURACION");
            }
            return _logDebugView;
        }
    }

    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (Context.Request.IsAuthenticated)
        {
          // retrieve user's identity from httpcontext user 
          FormsIdentity ident = (FormsIdentity)Context.User.Identity;
          string perfil = ident.Ticket.UserData;
          //A esta pantalla tienen accesos todos los usuarios menos los operadores
          if (string.Compare(perfil, "0") == 0)
          {
              Response.Redirect("~/Login.aspx", false);
            return;
          }

        }
        else
        {
            Response.Redirect("~/Login.aspx", false);
        }

        if (!IsPostBack)
        {
            //Se obtienen los datos para cargar el listBox de sectorizaciones
            ObtenerListaSectorizaciones();
            Configura_BtLanzaInforme();
            bPaginaCargada = true;
        }
        else
        {
            if (false == bPaginaCargada)
            {
                if (DListSectorizaciones.Items.Count <= 0)
                {
                    DListSectorizaciones.Items.Clear();
                }
                ObtenerListaSectorizaciones();
            }
        }


    }

    protected bool ObtenerListaSectorizaciones()
    {
        bool bCorrecto = false;

        try
        {
            ServiciosCD40.Tablas[] objListaSectorizaciones = null;
            ServiciosCD40.Sectorizaciones objSectorizacion = new ServiciosCD40.Sectorizaciones();

            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement s;

            String strSectorizacionAux=string.Empty;

            s = config.AppSettings.Settings["Sistema"];

            if (s != null)
            {
                objSectorizacion.IdSistema = s.Value;
            }

            if (DListSectorizaciones.Items.Count>0)
                DListSectorizaciones.Items.Clear();

            LbValorSectorizacionActiva.Text = string.Empty;

            objListaSectorizaciones = ServicioCD40.ListSelectSQL(objSectorizacion);

            DListSectorizaciones.DataTextField = "IdSectorizacion";

            if (objListaSectorizaciones!=null && objListaSectorizaciones.Length>0)
            {
                for (int i = 0; i < objListaSectorizaciones.Length; i++)
                {
                    strSectorizacionAux=((ServiciosCD40.Sectorizaciones)objListaSectorizaciones[i]).IdSectorizacion ;

                    // Evitar que aparezca la sectorización activa y SACTA
                    if ((((ServiciosCD40.Sectorizaciones)objListaSectorizaciones[i]).IdSectorizacion !=
                        ((ServiciosCD40.Sectorizaciones)objListaSectorizaciones[i]).FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss")) &&
                        string.Compare(strSectorizacionAux, "SCV")!=0 &&
                        string.Compare(strSectorizacionAux, "SACTA")!=0)
                    {
                        DListSectorizaciones.Items.Add(strSectorizacionAux);

                        if (((ServiciosCD40.Sectorizaciones)objListaSectorizaciones[i]).Activa == true)
                            LbValorSectorizacionActiva.Text = strSectorizacionAux;
                    }
                    else if (string.Compare(strSectorizacionAux, "SACTA")==0 && ((ServiciosCD40.Sectorizaciones)objListaSectorizaciones[i]).Activa == true)
                    {
                        //Si SACTA es la sectorización activa
                        LbValorSectorizacionActiva.Text = strSectorizacionAux;
                        DListSectorizaciones.Items.Add(strSectorizacionAux);
                    }
                }

                if (!string.IsNullOrEmpty(LbValorSectorizacionActiva.Text) && DListSectorizaciones.Items.Count>0)
                {
                    DListSectorizaciones.SelectedValue = LbValorSectorizacionActiva.Text;
                }
            }

            bCorrecto=true;
        }
        catch (Exception ex)
        {
            logDebugView.Error("(InformeSectorizaciones-ObtenerListaSectorizaciones): ", ex);
        }

        return (bCorrecto);
    }
    protected void Configura_BtLanzaInforme()
    {
        string strComando=string.Empty;
        string strIdSectorizacion = string.Empty;

        strIdSectorizacion = DListSectorizaciones.SelectedValue;

        LinkLanzaInforme.Attributes.Remove("onclick");

        if (!string.IsNullOrEmpty(strIdSectorizacion))
        {
            strComando = string.Format("AbreVentana('../Informes/InformeSectorizacion.aspx?SECTORIZACION={0}');return false;", strIdSectorizacion);
        }
        else
        {
            strComando = "AbreVentana('../Informes/InformeSectorizacion.aspx?SECTORIZACION=REP_SECTORIZATIONS');return false;";
        }

        LinkLanzaInforme.Attributes.Add("onclick", strComando);
    }

    protected void CRViewerInforme_Init(object sender, EventArgs e)
    {
    }

    protected void DListSectorizaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Configura_BtLanzaInforme();
    }


    protected void BtLanzaInforme_Click(object sender, EventArgs e)
    {
        //EjecutaReport();
    }

}
