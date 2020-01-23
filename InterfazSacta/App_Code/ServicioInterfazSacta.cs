using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;


using System.Collections;
using System.Text;
using Sacta;


[WebService(Namespace = "http://CD40.es/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ServicioInterfazSacta : System.Web.Services.WebService
{
	static SactaModule sModule = null;
	static string IdSistema;
	static byte EstadoSacta = (byte)0;
	private MySql.Data.MySqlClient.MySqlConnection MySqlConnectionToCd40;

	public ServicioInterfazSacta()
	{
		string cadenaConexion;
		System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
		if (webConfiguracion.ConnectionStrings.ConnectionStrings.Count > 0)
		{
			cadenaConexion = webConfiguracion.ConnectionStrings.ConnectionStrings["ConexionBaseDatosCD40"].ToString();

			MySqlConnectionToCd40 = new MySql.Data.MySqlClient.MySqlConnection(cadenaConexion);
		}

		//Uncomment the following line if using designed components 
		//InitializeComponent(); 


		//if (sModule == null)
		//{
		//    System.Diagnostics.Debug.Assert(false);
		//    System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
		//    System.Configuration.KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
		//    IdSistema = s.Value;
		//    sModule = new Sacta.SactaModule(IdSistema);
		//}
	}

	void sModule_SactaActivityChanged(object sender, System.Collections.Generic.Dictionary<string, object> msg)
	{
		EstadoSacta = (byte)msg["SactaActivity"];
	}

	//[WebMethod]
	//public void ComunicaSectorizacion(DateTime fechaActivacion)
	//{
	//    ServiciosCD40 serviciosCD40 = new ServiciosCD40();

	//    System.Diagnostics.Debug.Assert(false);
	//    serviciosCD40.ComunicaSectorizacionActiva(IdSistema, "SACTA", ref fechaActivacion);
		
	//}
	[WebMethod]
	public byte GetEstadoSacta()
	{
        if (sModule != null)
        {
            EstadoSacta = (byte)((byte)(sModule.State << 4) | (EstadoSacta));

            return EstadoSacta;
        }

        return 0;
	}

	[WebMethod]
	public void StartSacta()
	{
		if (sModule == null)
		{
			//System.Diagnostics.Debug.Assert(false);
			System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
			System.Configuration.KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
			IdSistema = s.Value;
			sModule = new Sacta.SactaModule(IdSistema, MySqlConnectionToCd40);
			sModule.Start();
			sModule.SactaActivityChanged += new Utilities.GenericEventHandler<System.Collections.Generic.Dictionary<string, object>>(sModule_SactaActivityChanged);
		}
		else
			sModule.Start();
	}

	[WebMethod]
	public void EndSacta()
	{
		if (sModule == null)
		{
			//System.Diagnostics.Debug.Assert(false);
			System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
			System.Configuration.KeyValueConfigurationElement s = config.AppSettings.Settings["Sistema"];
			IdSistema = s.Value;
			sModule = new Sacta.SactaModule(IdSistema, MySqlConnectionToCd40);
			sModule.Stop();
		}
		else
			sModule.Stop();

		EstadoSacta = (byte)0;

		sModule = null;
	}

    [WebMethod]
    public string SactaConfGet()
    {
        return SactaModule.SactaCfgGet();
    }

    [WebMethod]
    public bool SactaConfSet(string jcfg)
    {
        return SactaModule.SactaCfgSet(jcfg);
    }

}
