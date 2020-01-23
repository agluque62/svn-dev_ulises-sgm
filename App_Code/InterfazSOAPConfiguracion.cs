using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;
using System.Data;
using CD40.BD;
using CD40.BD.Entidades;
//using SICCIP.Entidades;
using ConfiguracionElementosHw;
using System.Xml.Serialization;

[WebService(Namespace = "http://CD40.es/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

#region XML-Includes
[XmlInclude(typeof(Tablas))]
[XmlInclude(typeof(Agrupaciones))]
[XmlInclude(typeof(SectoresAgrupacion))]
[XmlInclude(typeof(GruposTelefonia))]
[XmlInclude(typeof(TifX))]
[XmlInclude(typeof(Top))]
[XmlInclude(typeof(Sistema))]
[XmlInclude(typeof(CD40.BD.Entidades.Operadores))]
[XmlInclude(typeof(Emplazamientos))]
[XmlInclude(typeof(Nucleos))]
[XmlInclude(typeof(Prefijos))]
[XmlInclude(typeof(Sectorizaciones))]
[XmlInclude(typeof(Redes))]
[XmlInclude(typeof(PermisosRedes))]
[XmlInclude(typeof(Troncales))]
[XmlInclude(typeof(SectoresSectorizacion))]
[XmlInclude(typeof(Encaminamientos))]
[XmlInclude(typeof(Sectores))]
[XmlInclude(typeof(Rangos))]
[XmlInclude(typeof(Rutas))]
[XmlInclude(typeof(ParametrosRecursoRadio))]
[XmlInclude(typeof(ParametrosRecursoTelefonia))]
[XmlInclude(typeof(ParametrosRecursoLCEN))]
[XmlInclude(typeof(Recursos))]
[XmlInclude(typeof(RecursosLCEN))]
[XmlInclude(typeof(RecursosTF))]
[XmlInclude(typeof(RecursosRadio))]
//[XmlInclude(typeof(SICCIP.Entidades.RecursosDatos))]
[XmlInclude(typeof(UsuariosAbonados))]
[XmlInclude(typeof(Destinos))]
[XmlInclude(typeof(DestinosExternos))]
[XmlInclude(typeof(Externos))]
[XmlInclude(typeof(DestinosExternosSector))]
[XmlInclude(typeof(DestinosInternos))]
[XmlInclude(typeof(Internos))]
[XmlInclude(typeof(DestinosInternosSector))]
[XmlInclude(typeof(DestinosRadio))]
[XmlInclude(typeof(DestinosRadioSector))]
[XmlInclude(typeof(EstadosRecursos))]
[XmlInclude(typeof(Radio))]
[XmlInclude(typeof(DestinosTelefonia))]
[XmlInclude(typeof(Niveles))]
[XmlInclude(typeof(Altavoces))]
[XmlInclude(typeof(GruposTelefonia))]
[XmlInclude(typeof(SectoresSector))]
[XmlInclude(typeof(TroncalesRuta))]
[XmlInclude(typeof(TeclasSector))]
[XmlInclude(typeof(ParametrosSector))]

[XmlInclude(typeof(RangosSCV))]
[XmlInclude(typeof(RecursosSCV))]
[XmlInclude(typeof(SectoresSCV))]
[XmlInclude(typeof(ParametrosRDSCV))]
[XmlInclude(typeof(ParametrosLCENSCV))]
[XmlInclude(typeof(ParametrosTFSCV))]
[XmlInclude(typeof(ParametrosSectorSCV))]
[XmlInclude(typeof(PermisosRedesSCV))]
[XmlInclude(typeof(TeclasSectorSCV))]
[XmlInclude(typeof(NivelesSCV))]

[XmlInclude(typeof(Tipos.ExportaTipoEnumerados))]
#endregion

public class InterfazSOAPConfiguracion : System.Web.Services.WebService
{
	private GestorBaseDatos GestorBDCD40;
	private MySql.Data.MySqlClient.MySqlConnection MySqlConnectionToCd40;
	ConfiguracionSistema CfgSistema;

    ConfiguracionElementosHw.CfgUsuario ConfiguracionUsuario;
    CfgPasarela ConfiguracionPasarela;
    CfgEnlaceExterno []ConfiguracionEnlaceExterno;
    CfgEnlaceInterno []ConfiguracionEnlaceInterno;

	static object Sync = new object();

    public InterfazSOAPConfiguracion () 
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
		string cadenaConexion;
		System.Configuration.Configuration webConfiguracion = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
		if (webConfiguracion.ConnectionStrings.ConnectionStrings.Count > 0)
		{
			cadenaConexion = webConfiguracion.ConnectionStrings.ConnectionStrings["ConexionBaseDatosCD40"].ToString();

			MySqlConnectionToCd40 = new MySql.Data.MySqlClient.MySqlConnection(cadenaConexion);

			GestorBDCD40 = new GestorBaseDatos(MySqlConnectionToCd40);
		}

		ConfiguracionUsuario = new ConfiguracionElementosHw.CfgUsuario();
        ConfiguracionPasarela = new CfgPasarela();

        ConfiguracionUsuario.ListaAbonados = new List<NumerosAbonado>();
        //ConfiguracionUsuario.ListaNombreUsuario = new List<string>();

        CfgSistema = new ConfiguracionSistema();
        CfgSistema.ParametrosGenerales = new ParametrosGeneralesSistema();
    }

    #region Métodos
    public DireccionamientoIP[] ListaPlanDireccionamientoIP(string id_sistema)
    {
        //Procedimientos pr = new Procedimientos();
        DataSet dsPlan = Procedimientos.PlanDireccionamientoIP(GestorBDCD40.ConexionMySql, id_sistema);
        DireccionamientoIP[] plan = new DireccionamientoIP[dsPlan.Tables[0].Rows.Count];
        int numFila = 0;

        if (dsPlan != null)
        {
            foreach (System.Data.DataRow dr in dsPlan.Tables[0].Rows)
            {
                DireccionamientoIP p = new DireccionamientoIP();

                p.IdHost = (string)dr["Id"];
                p.TipoHost = (Tipos.Tipo_Elemento_HW)Convert.ToUInt32(dr["TipoEH"]);
                if (dr["IpRed1"] != System.DBNull.Value)
                    p.IpRed1 = (string)dr["IpRed1"];
                if (dr["IpRed2"] != System.DBNull.Value)
                    p.IpRed2 = (string)dr["IpRed2"];

                plan[numFila++] = p;
            }
        }

        return plan;
    }

    private void DameParametrosGenerales(string id_sistema)
    {
        Sistema s = new Sistema();
        s.IdSistema = id_sistema;

		List<Tablas> listaSistemas = GestorBDCD40.ListSelectSQL(s, null);
		if (listaSistemas.Count > 0)
		{
			s = (Sistema)listaSistemas[0];

			CfgSistema.ParametrosGenerales.TamLiteralEnlAG = s.TamLiteralAG;
			CfgSistema.ParametrosGenerales.TamLiteralEnlDA = s.TamLiteralDA;
			CfgSistema.ParametrosGenerales.TamLiteralEmplazamiento = s.TamLiteralEmplazamiento;
			CfgSistema.ParametrosGenerales.TamLiteralEnlExt = s.TamLiteralEnlExt;
			CfgSistema.ParametrosGenerales.TamLiteralEnlIA = s.TamLiteralIA;
			CfgSistema.ParametrosGenerales.TiempoMaximoPTT = s.TiempoPtt;
			CfgSistema.ParametrosGenerales.TiempoSinJack1 = s.TiempoSinJack1;
			CfgSistema.ParametrosGenerales.TiempoSinJack2 = s.TiempoSinJack2;
		}
        /*
        IFormatter format = new SoapFormatter();
        Stream canal = new FileStream("Listado.xml", FileMode.Create, FileAccess.Write, FileShare.None);
        format.Serialize(canal, s);
        canal.Close();
        */
    }

    private void DamePlanNumeracionATS(string id_sistema)
    {
        //Procedimientos pr = new Procedimientos();
        Utilidades ut = new Utilidades(GestorBDCD40.ConexionMySql);

        ushort j = 0;
        Encaminamientos e=new Encaminamientos();
        e.IdSistema=id_sistema;


		List<Tablas> listaEncaminamientos = GestorBDCD40.ListSelectSQL(e, null);
        int tamListaEncaminamientos = listaEncaminamientos.Count;

        CfgSistema.PlanNumeracionATS = new NumeracionATS[tamListaEncaminamientos];
        for (ushort i = 0; i < tamListaEncaminamientos; i++)
        {
            // Datos de la central
            CfgSistema.PlanNumeracionATS[i] = new NumeracionATS();
            CfgSistema.PlanNumeracionATS[i].CentralPropia = ((Encaminamientos)listaEncaminamientos[i]).CentralPropia;
            CfgSistema.PlanNumeracionATS[i].Throwswitching = ((Encaminamientos)listaEncaminamientos[i]).Throwswitching;
			CfgSistema.PlanNumeracionATS[i].NumTest = CfgSistema.PlanNumeracionATS[i].CentralPropia ? ((Encaminamientos)listaEncaminamientos[i]).NumTest : string.Empty;
            
            // Datos de los rangos
            Rangos r=new Rangos();
            r.IdSistema=id_sistema;
            r.Central = ((Encaminamientos)listaEncaminamientos[i]).Central;
                        // Rangos de operador
            r.Tipo = "O";
			List<Tablas> listaRangos = GestorBDCD40.ListSelectSQL(r, null);
            int tamListaRangos = listaRangos.Count;
            CfgSistema.PlanNumeracionATS[i].RangosOperador = new RangosSCV[tamListaRangos];
            for (ushort num = 0; num < tamListaRangos; num++)
            {
                CfgSistema.PlanNumeracionATS[i].RangosOperador[num] = (RangosSCV)listaRangos[num];
            }
                        // Rangos de privilegiado 
            r.Tipo = "P";
			listaRangos = GestorBDCD40.ListSelectSQL(r, null);
            tamListaRangos = listaRangos.Count;
            CfgSistema.PlanNumeracionATS[i].RangosPrivilegiados = new RangosSCV[tamListaRangos];
            for (ushort num = 0; num < tamListaRangos; num++)
            {
                CfgSistema.PlanNumeracionATS[i].RangosPrivilegiados[num] = (RangosSCV)listaRangos[num];
            }

            // Datos de las rutas
			DataSet dsRutas = Procedimientos.ListaRutas(GestorBDCD40.ConexionMySql, id_sistema, ((Encaminamientos)listaEncaminamientos[i]).Central);

            CfgSistema.PlanNumeracionATS[i].ListaRutas = new PlanRutas[dsRutas.Tables[0].Rows.Count];
            j = 0;
            foreach (System.Data.DataRow drRutas in dsRutas.Tables[0].Rows)
            {
                CfgSistema.PlanNumeracionATS[i].ListaRutas[j] = new PlanRutas();

                CfgSistema.PlanNumeracionATS[i].ListaRutas[j].TipoRuta = (string)drRutas["Tipo"];
                CfgSistema.PlanNumeracionATS[i].ListaRutas[j].ListaTroncales = ut.ListaDeTroncalesEnUnaRuta(id_sistema, ((Encaminamientos)listaEncaminamientos[i]).Central, (string)drRutas["IdRuta"]);
                j++;
            }
        }
    }

    private void DamePlanDireccionamientoIP(string id_sistema)
    {
        CfgSistema.PlanDireccionamientoIP = ListaPlanDireccionamientoIP(id_sistema);
    }

    private void DamePlanTroncales(string id_sistema)
    {
        Troncales t = new Troncales();
        t.IdSistema = id_sistema;

		List<Tablas> listaTroncales = GestorBDCD40.ListSelectSQL(t, null);
        int tamListaTroncales = listaTroncales.Count;
        CfgSistema.PlanTroncales = new ListaTroncales[tamListaTroncales];
        for (ushort i = 0; i < tamListaTroncales; i++)
        {
            CfgSistema.PlanTroncales[i] = new ListaTroncales();
            CfgSistema.PlanTroncales[i].IdTroncal = ((Troncales)listaTroncales[i]).IdTroncal;
            CfgSistema.PlanTroncales[i].NumeroTest = ((Troncales)listaTroncales[i]).NumTest;

            RecursosTF r = new RecursosTF();
            r.IdSistema = id_sistema;
            r.IdTroncal = ((Troncales)listaTroncales[i]).IdTroncal;

			List<Tablas> listaRecursos = GestorBDCD40.ListSelectSQL(r, null);
			int tamListaRecursos = listaRecursos.Count;
			int posA = 0;
			int posB = tamListaRecursos - 1;

			CfgSistema.PlanTroncales[i].ListaRecursos = new PlanRecursos[tamListaRecursos];
            for (ushort j = 0; j < tamListaRecursos; j++)
            {
				int posGlobal;
				if (((RecursosTF)listaRecursos[j]).Lado == "0")
					posGlobal = posA++;
				else
					posGlobal = posB--;

				CfgSistema.PlanTroncales[i].ListaRecursos[posGlobal] = new PlanRecursos();
				CfgSistema.PlanTroncales[i].ListaRecursos[posGlobal].IdRecurso = ((RecursosTF)listaRecursos[j]).IdRecurso;

                Recursos rTroncal = new Recursos();
                rTroncal.IdSistema = r.IdSistema;
				rTroncal.IdRecurso = CfgSistema.PlanTroncales[i].ListaRecursos[posGlobal].IdRecurso;
                rTroncal.TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_TELEFONIA;
				List<Tablas> listaRecursosTroncal = GestorBDCD40.ListSelectSQL(rTroncal, null);
                if (listaRecursosTroncal.Count > 0)
					CfgSistema.PlanTroncales[i].ListaRecursos[posGlobal].Tipo = ((Recursos)listaRecursosTroncal[0]).Interface;
            }
        }
    }

    private void DamePlanRedes(string id_sistema)
    {
        Redes rd = new Redes();
        rd.IdSistema = id_sistema;

		List<Tablas> listaRedes = GestorBDCD40.ListSelectSQL(rd, null);
        int tamListaRedes = listaRedes.Count;
        CfgSistema.PlanRedes = new ListaRedes[tamListaRedes];
        for (ushort i = 0; i < tamListaRedes; i++)
        {
            CfgSistema.PlanRedes[i] = new ListaRedes();
            CfgSistema.PlanRedes[i].IdRed = ((Redes)listaRedes[i]).IdRed;
            CfgSistema.PlanRedes[i].Prefijo = Convert.ToUInt32(((Redes)listaRedes[i]).Prefijo);

            RecursosTF r = new RecursosTF();
            r.IdSistema = id_sistema;
            r.IdRed = ((Redes)listaRedes[i]).IdRed;

			List<Tablas> listaRecursos = GestorBDCD40.ListSelectSQL(r, null);
            int tamListaRecursos = listaRecursos.Count;
            CfgSistema.PlanRedes[i].ListaRecursos = new PlanRecursos[tamListaRecursos];
            for (ushort j = 0; j < tamListaRecursos; j++)
            {
                CfgSistema.PlanRedes[i].ListaRecursos[j] = new PlanRecursos();
                CfgSistema.PlanRedes[i].ListaRecursos[j].IdRecurso = ((RecursosTF)listaRecursos[j]).IdRecurso;

                Recursos rTroncal = new Recursos();
                rTroncal.IdSistema = r.IdSistema;
                rTroncal.IdRecurso = ((RecursosTF)listaRecursos[j]).IdRecurso;
				List<Tablas> listaRecursosTroncal = GestorBDCD40.ListSelectSQL(rTroncal, null);
                if (listaRecursosTroncal.Count > 0)
                    CfgSistema.PlanRedes[i].ListaRecursos[j].Tipo = ((Recursos)listaRecursosTroncal[0]).Interface;
            }
        }
    }

    private void DamePlanAsignacionUsuarios(string id_sistema)
    {
        uint i = 0;
        //Procedimientos pr = new Procedimientos();
		DataSet ds = Procedimientos.AsignacionUsuariosATops(GestorBDCD40.ConexionMySql, id_sistema);

        CfgSistema.PlanAsignacionUsuarios = new AsignacionUsuariosTV[ds.Tables[0].Rows.Count];
        foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
        {
            CfgSistema.PlanAsignacionUsuarios[i] = new AsignacionUsuariosTV();
            CfgSistema.PlanAsignacionUsuarios[i].IdUsuario = (string)dr["IdSector"];
            CfgSistema.PlanAsignacionUsuarios[i++].IdHost = (string)dr["IdTop"];
        }
    }

    private void DamePlanAsignacionRecursos(string id_sistema)
    {
        Recursos r = new Recursos();
        r.IdSistema = id_sistema;
		r.TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_DONT_CARE;

		List<Tablas> listaRecursos = GestorBDCD40.ListSelectSQL(r, null);
        int tamListaRecursos = listaRecursos.Count;
        CfgSistema.PlanAsignacionRecursos = new AsignacionRecursosGW[tamListaRecursos];

        for (ushort i = 0; i < tamListaRecursos; i++)
        {
            CfgSistema.PlanAsignacionRecursos[i] = new AsignacionRecursosGW();
            CfgSistema.PlanAsignacionRecursos[i].IdRecurso = ((Recursos)listaRecursos[i]).IdRecurso;
			CfgSistema.PlanAsignacionRecursos[i].IdHost = ((Recursos)listaRecursos[i]).IdTifX != null ? ((Recursos)listaRecursos[i]).IdTifX : ((Recursos)listaRecursos[i]).IdEquipo;
        }
    }

    private void DamePlanDireccionamientoSIP(string id_sistema)
    {
        //Procedimientos pr = new Procedimientos();
		DataSet dsPlan = Procedimientos.PlanDireccionamientoSIP(GestorBDCD40.ConexionMySql, id_sistema);
        List<DireccionamientoSIP> plan = new List<DireccionamientoSIP>();

        int numFila = 0;
        int numAbonado=0;
        string ultimoUsuario = "";

        if (dsPlan != null)
        {
            foreach (System.Data.DataRow dr in dsPlan.Tables[0].Rows)
            {
                if (ultimoUsuario != (string)dr["IdSector"])
                {
                    ultimoUsuario = (string)dr["IdSector"];
                    numAbonado = 0;

                    DireccionamientoSIP unDir = new DireccionamientoSIP();
                    unDir.IdUsuario = (string)dr["IdSector"];
                    unDir.NumerosAbonadoQueAtiende = new List<DireccionamientoSIP.StrNumeroAbonado>();

                    plan.Add(unDir);
                    numFila++;
                }

                DireccionamientoSIP.StrNumeroAbonado abonado = new DireccionamientoSIP.StrNumeroAbonado();

                abonado.Prefijo = Convert.ToUInt32(dr["IdPrefijo"]);
                abonado.NumeroAbonado = (string)dr["IdAbonado"];
                plan[numFila-1].NumerosAbonadoQueAtiende.Add(abonado);

                numAbonado++;
            }
        }

        CfgSistema.PlanDireccionamientoSIP = plan;
    }

	private List<ParametrosRDSCV.RecursosExternos> GetListaEnlaces(ParametrosRecursosRadioKASiccip r) 
	{
		List<ParametrosRDSCV.RecursosExternos> listaEnlaces = new List<ParametrosRDSCV.RecursosExternos>();
		DataSet ds = GestorBDCD40.GetDataSet(r.GetUriRecursoDestino(), null);

		if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
		{
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				if ((string)dr["IdRecurso2"] != ((RecursosRadio)r).IdRecurso)
				{
					ParametrosRDSCV.RecursosExternos rec = new ParametrosRDSCV.RecursosExternos();

					rec.Nombre = (string)dr["IdRecurso2"];

					listaEnlaces.Add(rec);
				}
			}
		}

		return listaEnlaces.Count == 0 ? null : listaEnlaces;
	}


    public List<Tablas> ListaRecursosPasarela(string id_sistema, string id_hw)
    {
        Recursos r = new Recursos();
        r.IdTifX = id_hw;
        r.IdSistema = id_sistema;

		List<Tablas> listaRecursos = GestorBDCD40.ListSelectSQL(r, null);
        return listaRecursos;
    }

	private string GetEstadoDelRecurso(string idSectorizacion, Recursos r, Radio rRadio)
	{
		EstadoRecursos eRecurso = new EstadoRecursos();
		eRecurso.IdSistema = rRadio.IdSistema;
		eRecurso.IdSector = rRadio.IdSector;
		eRecurso.IdNucleo = rRadio.IdNucleo;
		eRecurso.IdSectorizacion = idSectorizacion;
		eRecurso.IdDestino = rRadio.IdDestino;
		eRecurso.IdRecurso = r.IdRecurso;

		List<Tablas> estado = GestorBDCD40.ListSelectSQL(eRecurso, null);
		if (estado.Count > 0)
		{
			return ((EstadoRecursos)estado[0]).Estado;
		}

		return "I";
	}

    #endregion

    #region WebServices
    [WebMethod(Description = "Pasándole el identificador de sistema, devuelve la versión de la configuración activa registrada en la base de datos")]
    public void GetVersionConfiguracion_XML(string id_sistema)
    {
		//Procedimientos pr = new Procedimientos();

		GetVersion v;
		v.IdSistema = id_sistema;
		v.Version = Procedimientos.VersionSectorizacion(GestorBDCD40.ConexionMySql, id_sistema);

		System.Xml.Serialization.XmlSerializer xmlWriter = new System.Xml.Serialization.XmlSerializer(typeof(GetVersion));

		StreamWriter xmlFile = new StreamWriter( Server.MapPath("XML_GetVersionConfiguracion.xml"));
		xmlWriter.Serialize(xmlFile, v);
		xmlFile.Close();
	}

	[WebMethod(Description = "Pasándole el identificador de sistema, devuelve la versión de la configuración activa registrada en la base de datos")]
	public string GetVersionConfiguracion(string id_sistema)
	{
		//Procedimientos pr = new Procedimientos();

		return Procedimientos.VersionSectorizacion(GestorBDCD40.ConexionMySql, id_sistema);

		//lock (Sync)
		//{
		//    object data;

		//    try
		//    {
		//        System.Xml.Serialization.XmlSerializer xmlReader = new System.Xml.Serialization.XmlSerializer(typeof(GetVersion));
		//        StreamReader xmlFile = new StreamReader(Server.MapPath("XML_GetVersionConfiguracion.xml"));
		//        data = xmlReader.Deserialize(xmlFile.BaseStream);
		//        xmlFile.Close();
		//    } 
		//    catch (System.IO.FileNotFoundException e)
		//    {
		//        System.Diagnostics.Debug.Assert(false, e.Message);
		//        return null;
		//    }

		//    return ((GetVersion)data).Version;
		//}
	}

	[WebMethod(Description = "Pasándole el identificador de sistema, devuelve el Scv [1|2] activo. 0 si error")]
	public string GetScvActivo(string idSistema)
	{
		lock (Sync)
		{
			Sistema s = new Sistema();
			s.IdSistema = idSistema;

			List<Tablas> sistema = GestorBDCD40.ListSelectSQL(s, null);
			if (sistema.Count > 0)
			{
				if (((Sistema)sistema[0]).EstadoScv1 == 1 && ((Sistema)sistema[0]).EstadoScv2 != 1)
					return "1";
				else if (((Sistema)sistema[0]).EstadoScv1 != 1 && ((Sistema)sistema[0]).EstadoScv2 == 1)
					return "2";
				else
					return "0";
			}
			return "0";
		}
	}

    [WebMethod(Description = "Pasándole el identificador de usuario y el sistema al que pertenece, devuelve la lista de enlaces externos del usuario.")]
    public CfgEnlaceExterno[] GetListaEnlacesExternos(string id_sistema, string id_usuario)
    {
		lock (Sync)
		{
			Utilidades ut = new Utilidades(GestorBDCD40.ConexionMySql);
			string strUltimoEnlace = "";
			int numEnlaceDistinto = 0;
			// Obtener identificador de la sectorización activa
			Sectorizaciones s = new Sectorizaciones();
			s.IdSistema = id_sistema;
			s.Activa = true;
			List<Tablas> sectorizacion = GestorBDCD40.ListSelectSQL(s, null);
			if (sectorizacion.Count <= 0)
				return null;

			//DestinosRadioSector eExternoSector = new DestinosRadioSector();
			Radio eExternoSector = new Radio();
			eExternoSector.IdSistema = id_sistema;
			eExternoSector.IdSector = id_usuario;
			// El identificador de la sectorizacion donde se tienen que obtener los datos
			// coincide con la fecha de activación de la sectorización activa
			eExternoSector.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss");
			//eExternoSector.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).IdSectorizacion;

			// Cuenta los enlaces radio distintos de un sector que tengan configurado un recurso
			int numEnlacesExternos = ut.SelectCountDistinctEnlaceSQL(eExternoSector);
			if (numEnlacesExternos > 0)
			{
				ConfiguracionEnlaceExterno = new CfgEnlaceExterno[numEnlacesExternos];

				List<Tablas> listaEnlacesExternos = GestorBDCD40.ListSelectSQL(eExternoSector, null);
				int numTotalEnlaces = listaEnlacesExternos.Count;

				for (int i = 0; i < numTotalEnlaces; i++)
				{
					if (strUltimoEnlace != ((Radio)listaEnlacesExternos[i]).IdDestino)
					{
						// Sólo si el destino externo tiene recursos, se considerará bueno el enlace
						// en caso contrario, se ignora el enlace
						RecursosRadio rRadio = new RecursosRadio();
						rRadio.IdSistema = id_sistema;
						rRadio.IdDestino = ((Radio)listaEnlacesExternos[i]).IdDestino;
						List<Tablas> listaRecursosEnlace = GestorBDCD40.ListSelectSQL(rRadio, null);
						if (listaRecursosEnlace.Count > 0)
						{
							// Nuevo enlace del sector
							strUltimoEnlace = ((Radio)listaEnlacesExternos[i]).IdDestino;

							ConfiguracionEnlaceExterno[numEnlaceDistinto] = new CfgEnlaceExterno();
							ConfiguracionEnlaceExterno[numEnlaceDistinto].ListaPosicionesEnHmi = new List<uint>();
							ConfiguracionEnlaceExterno[numEnlaceDistinto].ListaRecursos = new List<CfgRecursoEnlaceExterno>();
							ConfiguracionEnlaceExterno[numEnlaceDistinto].DestinoAudio = new List<string>();

							ConfiguracionEnlaceExterno[numEnlaceDistinto].Literal = ((Radio)listaEnlacesExternos[i]).Literal;
							ConfiguracionEnlaceExterno[numEnlaceDistinto].Prioridad = ((Radio)listaEnlacesExternos[i]).PrioridadSIP;
							ConfiguracionEnlaceExterno[numEnlaceDistinto].EstadoAsignacion = ((Radio)listaEnlacesExternos[i]).ModoOperacion;
							ConfiguracionEnlaceExterno[numEnlaceDistinto].ListaPosicionesEnHmi.Add(((Radio)listaEnlacesExternos[i]).PosHMI);
							ConfiguracionEnlaceExterno[numEnlaceDistinto].DestinoAudio.Add(((Radio)listaEnlacesExternos[i]).Cascos);
							EstadoAltavoces altavoz = new EstadoAltavoces();
							altavoz.IdSistema = id_sistema;
							altavoz.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).IdSectorizacion;
							altavoz.IdSector = id_usuario;
							altavoz.IdNucleo = ((Radio)listaEnlacesExternos[i]).IdNucleo;
							altavoz.IdDestino = ((Radio)listaEnlacesExternos[i]).IdDestino;
							List<Tablas> listaAltavoces = GestorBDCD40.ListSelectSQL(altavoz, null);
							for (int k = 0; k < listaAltavoces.Count; k++)
							{
								ConfiguracionEnlaceExterno[numEnlaceDistinto].DestinoAudio.Add(((EstadoAltavoces)listaAltavoces[k]).Estado);
							}

							DestinosRadio eExterno = new DestinosRadio();
							eExterno.IdSistema = id_sistema;
							eExterno.IdDestino = ((Radio)listaEnlacesExternos[i]).IdDestino;

							List<Tablas> listaEnlaces = GestorBDCD40.ListSelectSQL(eExterno, null);
							if (listaEnlaces.Count > 0)
							{
								ConfiguracionEnlaceExterno[numEnlaceDistinto].TipoFrecuencia = ((DestinosRadio)listaEnlaces[0]).TipoFrec;
								ConfiguracionEnlaceExterno[numEnlaceDistinto].ExclusividadTxRx = ((DestinosRadio)listaEnlaces[0]).ExclusividadTXRX;
							}

							for (int j = 0; j < listaRecursosEnlace.Count; j++)
							{
								CfgRecursoEnlaceExterno cfgRecurso = new CfgRecursoEnlaceExterno();
								cfgRecurso.IdRecurso = ((RecursosRadio)listaRecursosEnlace[j]).IdRecurso;
								Recursos r = new Recursos();
								r.IdSistema = id_sistema;
								r.IdRecurso = cfgRecurso.IdRecurso;
								List<Tablas> tRecurso = GestorBDCD40.ListSelectSQL(r, null);
								if (tRecurso.Count > 0)
									cfgRecurso.Tipo = ((Recursos)tRecurso[0]).Tipo;
								cfgRecurso.ModoConfPTT = ((RecursosRadio)listaRecursosEnlace[j]).ModoConfPTT;
								cfgRecurso.NumFlujosAudio = ((RecursosRadio)listaRecursosEnlace[j]).NumFlujosAudio;
								cfgRecurso.IdEmplazamiento = ((RecursosRadio)listaRecursosEnlace[j]).IdEmplazamiento;

								cfgRecurso.Estado = GetEstadoDelRecurso(((Sectorizaciones)sectorizacion[0]).IdSectorizacion, r, (Radio)listaEnlacesExternos[i]);// "S"; 

								ConfiguracionEnlaceExterno[numEnlaceDistinto].ListaRecursos.Add(cfgRecurso);
							}

							numEnlaceDistinto++;
						}
					}
					else
						ConfiguracionEnlaceExterno[numEnlaceDistinto - 1].ListaPosicionesEnHmi.Add(((Radio)listaEnlacesExternos[i]).PosHMI);


				}
				return ConfiguracionEnlaceExterno;
			}


			return null;
		}
    }

    [WebMethod(Description = "Pasándole el identificador de usuario y el sistema al que pertenece, devuelve la lista de enlaces internos del usuario.")]
    public CfgEnlaceInterno[] GetListaEnlacesInternos(string id_sistema, string id_usuario)
    {
        /*
        EnlacesInternosSector eInterno = new EnlacesInternosSector();
        int numEnlacesInternos = GestorBDCD40.SelectCountSQL(eInterno, "IdSistema='" + id_sistema + "' AND IdSector='" + id_usuario + "'");
        EnlacesLCENSector eLcen = new EnlacesLCENSector();
        numEnlacesInternos += GestorBDCD40.SelectCountSQL(eLcen, "IdSistema='" + id_sistema + "' AND IdSector='" + id_usuario + "'");
        */
		// Obtener identificador de la sectorización activa
		lock (Sync)
		{
			Sectorizaciones s = new Sectorizaciones();
			s.IdSistema = id_sistema;
			s.Activa = true;
			List<Tablas> sectorizacion = GestorBDCD40.ListSelectSQL(s, null);
			if (sectorizacion.Count <= 0)
				return null;

			// Enlaces a usuarios internos
			//DestinosInternosSector eInterno = new DestinosInternosSector();
			Internos eInterno = new Internos();
			eInterno.IdSistema = id_sistema;
			eInterno.IdSector = id_usuario;
			// El identificador de la sectorizacion donde se tienen que obtener los datos
			// coincide con la fecha de activación de la sectorización activa
			eInterno.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss");
			//eInterno.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).IdSectorizacion;

			List<Tablas> listaEnlacesInternos = GestorBDCD40.ListSelectSQL(eInterno, null);

			// Enlaces a usuarios externos
			//DestinosExternosSector eExterno = new DestinosExternosSector();
			Externos eExterno = new Externos();
			eExterno.IdSistema = id_sistema;
			eExterno.IdSector = id_usuario;
			// El identificador de la sectorizacion donde se tienen que obtener los datos
			// coincide con la fecha de activación de la sectorización activa
			eExterno.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss");
			//eExterno.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).IdSectorizacion;
			List<Tablas> listaEnlacesExternos = GestorBDCD40.ListSelectSQL(eExterno, null);

			int numEnlacesSector = listaEnlacesInternos.Count + listaEnlacesExternos.Count;

			if (numEnlacesSector > 0)
			{
				ConfiguracionEnlaceInterno = new CfgEnlaceInterno[numEnlacesSector];

				#region Enlaces a usuarios internos
				for (int i = 0; i < listaEnlacesInternos.Count; i++)
				{
					ConfiguracionEnlaceInterno[i] = new CfgEnlaceInterno();
					ConfiguracionEnlaceInterno[i].Literal = ((Internos)listaEnlacesInternos[i]).Literal;
					ConfiguracionEnlaceInterno[i].PosicionHMI = ((Internos)listaEnlacesInternos[i]).PosHMI;
					ConfiguracionEnlaceInterno[i].TipoEnlaceInterno = ((Internos)listaEnlacesInternos[i]).TipoAcceso;
					ConfiguracionEnlaceInterno[i].Prioridad = ((Internos)listaEnlacesInternos[i]).PrioridadSIP;
					ConfiguracionEnlaceInterno[i].OrigenR2 = ((Internos)listaEnlacesInternos[i]).OrigenR2;

					DestinosTelefonia interno = new DestinosTelefonia();
					interno.IdSistema = id_sistema;
					interno.IdDestino = ((Internos)listaEnlacesInternos[i]).IdDestino;
					List<Tablas> listaInternos = GestorBDCD40.ListSelectSQL(interno, null);
					if (listaInternos.Count > 0)
					{
						ConfiguracionEnlaceInterno[i].Dependencia = ((DestinosTelefonia)listaInternos[0]).IdGrupo;
					}


					#region Recurso para el destino
					ConfiguracionEnlaceInterno[i].ListaRecursos = new List<CfgRecursoEnlaceInternoConInterface>();

					CfgRecursoEnlaceInternoConInterface cfgRecurso = new CfgRecursoEnlaceInternoConInterface();

					cfgRecurso.NombreRecurso = ((Internos)listaEnlacesInternos[i]).IdDestino;
					cfgRecurso.Prefijo = ((Internos)listaEnlacesInternos[i]).IdPrefijo;

					ConfiguracionEnlaceInterno[i].ListaRecursos.Add(cfgRecurso);

					/*
					DestinosInternos dInterno = new DestinosInternos();
					dInterno.IdSistema = id_sistema;
					dInterno.IdDestino = ((DestinosInternosSector)listaEnlacesInternos[i]).IdDestino;
					dInterno.IdPrefijo = ((DestinosInternosSector)listaEnlacesInternos[i]).IdPrefijo;
					Tablas[] destino = GestorBDCD40.ListSelectSQL(dInterno);
					if (destino.Length > 0)
					{
						cfgRecurso.NombreRecurso = ((DestinosInternos)destino[0]).IdSector;
						cfgRecurso.Prefijo = ((DestinosInternosSector)listaEnlacesInternos[i]).IdPrefijo;
						ConfiguracionEnlaceInterno[i].ListaRecursos.Add(cfgRecurso);
					}*/
					#endregion
				}
				#endregion

				#region Enlaces a usuarios externos
				for (int i = listaEnlacesInternos.Count, j = 0; i < listaEnlacesExternos.Count + listaEnlacesInternos.Count; i++, j++)
				{
					ConfiguracionEnlaceInterno[i] = new CfgEnlaceInterno();
					ConfiguracionEnlaceInterno[i].Literal = ((Externos)listaEnlacesExternos[j]).Literal;
					ConfiguracionEnlaceInterno[i].TipoEnlaceInterno = ((Externos)listaEnlacesExternos[j]).TipoAcceso;
					ConfiguracionEnlaceInterno[i].PosicionHMI = ((Externos)listaEnlacesExternos[j]).PosHMI;
					ConfiguracionEnlaceInterno[i].Prioridad = ((Externos)listaEnlacesExternos[j]).PrioridadSIP;
					ConfiguracionEnlaceInterno[i].OrigenR2 = ((Externos)listaEnlacesExternos[j]).OrigenR2;

					DestinosTelefonia interno = new DestinosTelefonia();
					interno.IdSistema = id_sistema;
					interno.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
					List<Tablas> listaInternos = GestorBDCD40.ListSelectSQL(interno, null);
					if (listaInternos.Count > 0)
					{
						ConfiguracionEnlaceInterno[i].Dependencia = ((DestinosTelefonia)listaInternos[0]).IdGrupo;
					}


					#region Lista de recursos para el destino
					ConfiguracionEnlaceInterno[i].ListaRecursos = new List<CfgRecursoEnlaceInternoConInterface>();

					if (((Externos)listaEnlacesExternos[j]).IdPrefijo == 32 ||
						((Externos)listaEnlacesExternos[j]).IdPrefijo == 1)  // PP o LCEN
					{
						DataSet listaRecurso = Procedimientos.ListaRecursosDestino(GestorBDCD40.ConexionMySql, id_sistema, ((Externos)listaEnlacesExternos[j]).IdDestino, (int)((Externos)listaEnlacesExternos[j]).IdPrefijo);
						if (listaRecurso != null)
						{
							for (int numRecurso = 0; numRecurso < listaRecurso.Tables[0].Rows.Count; numRecurso++)
							{
								CfgRecursoEnlaceInternoConInterface cfgRecursoInterno = new CfgRecursoEnlaceInternoConInterface();

								cfgRecursoInterno.NombreRecurso = (string)listaRecurso.Tables[0].Rows[numRecurso]["IdRecurso"];
								cfgRecursoInterno.Prefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;
								cfgRecursoInterno.Interface = (Tipos.TipoInterface)(uint)listaRecurso.Tables[0].Rows[numRecurso]["Interface"];

								// En principio, los destinos PP o LCEN no deben tener número de abonado, pero... los PP Abonado de Namibia??
								DestinosExternos dExterno = new DestinosExternos();
								dExterno.IdSistema = id_sistema;
								dExterno.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
								dExterno.IdPrefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

								List<Tablas> destino = GestorBDCD40.ListSelectSQL(dExterno, null);
								if (destino.Count > 0)
									cfgRecursoInterno.NumeroAbonado = ((DestinosExternos)destino[0]).IdAbonado;

								ConfiguracionEnlaceInterno[i].ListaRecursos.Add(cfgRecursoInterno);
							}
						}

						/*
											RecursosTF rTf = new RecursosTF();
											rTf.IdSistema = id_sistema;
											rTf.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
											rTf.IdPrefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

											List<Tablas> listaRecurso = GestorBDCD40.ListSelectSQL(rTf);
											for (int numRecurso=0;numRecurso<listaRecurso.Count;numRecurso++)
											{
												CfgRecursoEnlaceInternoConInterface cfgRecursoInterno = new CfgRecursoEnlaceInternoConInterface();

												cfgRecursoInterno.NombreRecurso = ((RecursosTF)listaRecurso[numRecurso]).IdRecurso;
												cfgRecursoInterno.Prefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;
												cfgRecursoInterno.Interface = ((Recursos)listaRecurso[numRecurso]).Interface;

												// En principio, los destinos PP o LCEN no deben tener número de abonado, pero... los PP Abonado de Namibia??
												DestinosExternos dExterno = new DestinosExternos();
												dExterno.IdSistema = id_sistema;
												dExterno.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
												dExterno.IdPrefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

												List<Tablas> destino = GestorBDCD40.ListSelectSQL(dExterno);
												if (destino.Count > 0)
													cfgRecursoInterno.NumeroAbonado = ((DestinosExternos)destino[0]).IdAbonado;

												ConfiguracionEnlaceInterno[i].ListaRecursos.Add(cfgRecursoInterno);
											}
						 */
					}
					/*				else if (((Externos)listaEnlacesExternos[j]).IdPrefijo == 1)  // LCEN
									{
										RecursosLCEN rTf = new RecursosLCEN();
										rTf.IdSistema = id_sistema;
										rTf.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
										rTf.IdPrefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

										List<Tablas> listaRecurso = GestorBDCD40.ListSelectSQL(rTf);
										for (int numRecurso = 0; numRecurso < listaRecurso.Count; numRecurso++)
										{
											CfgRecursoEnlaceInternoConInterface cfgRecursoInterno = new CfgRecursoEnlaceInternoConInterface();

											cfgRecursoInterno.NombreRecurso = ((RecursosLCEN)listaRecurso[numRecurso]).IdRecurso;
											cfgRecursoInterno.Prefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;
											cfgRecursoInterno.Interface = ((Recursos)listaRecurso[numRecurso]).Interface;

											// En principio, los destinos PP o LCEN no deben tener número de abonado, pero... los PP Abonado de Namibia??
											DestinosExternos dExterno = new DestinosExternos();
											dExterno.IdSistema = id_sistema;
											dExterno.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
											dExterno.IdPrefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

											List<Tablas> destino = GestorBDCD40.ListSelectSQL(dExterno);
											if (destino.Count > 0)
												cfgRecursoInterno.NumeroAbonado = ((DestinosExternos)destino[0]).IdAbonado;

											ConfiguracionEnlaceInterno[i].ListaRecursos.Add(cfgRecursoInterno);
										}
									} */
					else       // RED
					{
						// Los destinos de redes, no llevan recursos, pero sí pueden llevar número de abonado
						CfgRecursoEnlaceInternoConInterface cfgRecursoInterno = new CfgRecursoEnlaceInternoConInterface();

						cfgRecursoInterno.Prefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;


						if (((Externos)listaEnlacesExternos[j]).TipoAcceso == "DA")
						{
							// Ver si el destino tiene un número de abonado asociado.
							DestinosExternos dExterno = new DestinosExternos();
							dExterno.IdSistema = id_sistema;
							dExterno.IdDestino = ((Externos)listaEnlacesExternos[j]).IdDestino;
							dExterno.IdPrefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;

							List<Tablas> destino = GestorBDCD40.ListSelectSQL(dExterno, null);
							if (destino.Count > 0)
								cfgRecursoInterno.NumeroAbonado = ((DestinosExternos)destino[0]).IdAbonado;

							ConfiguracionEnlaceInterno[i].ListaRecursos.Add(cfgRecursoInterno);
						}
						else
						{
							// AG
							Agenda ag = new Agenda();
							ag.IdSistema = id_sistema;
							ag.IdSector = id_usuario;
							ag.Prefijo = ((Externos)listaEnlacesExternos[j]).IdPrefijo;
							ag.Nombre = ((Externos)listaEnlacesExternos[j]).IdDestino;
							List<Tablas> destino = GestorBDCD40.ListSelectSQL(ag, null);
							if (destino.Count > 0)
								cfgRecursoInterno.NumeroAbonado = ((Agenda)destino[0]).Numero;

							ConfiguracionEnlaceInterno[i].ListaRecursos.Add(cfgRecursoInterno);
						}
					}

					#endregion
				}
				#endregion

				return ConfiguracionEnlaceInterno;
			}

			return null;
		}
    }

    [WebMethod(Description = "Pasándole el identificador hardware del terminal de voz y el sistema al que pertenece, " +
                            "devuelve el identificador de usuario alojado en el mismo y el modo de arranque ('A' o 'M').")]
    public LoginTerminalVoz LoginTop(string id_sistema, string id_hw) 
    {
		lock (Sync)
		{
			LoginTerminalVoz loginTop = new LoginTerminalVoz();
			//Procedimientos pr = new Procedimientos();
			string[] parametros = Procedimientos.LoginTop(GestorBDCD40.ConexionMySql, id_sistema, id_hw);
			loginTop.IdUsuario = parametros[0];
			loginTop.ModoLogin = parametros[1];

			return loginTop;
		}
    }

    [WebMethod (Description="Pasándole el identificador de usuario y el sistema al que pertenece, devuelve la configuración del usuario.")]
	public ConfiguracionElementosHw.CfgUsuario GetCfgUsuario(string id_sistema, string id_usuario)
    {
		lock (Sync)
		{
			Utilidades ut = new Utilidades(GestorBDCD40.ConexionMySql);
			//Procedimientos pr = new Procedimientos();
			#region Lista Número de Abonados
			DataSet listaNumerosInternos = Procedimientos.NumerosAbonadoInternos(GestorBDCD40.ConexionMySql, id_sistema, id_usuario);
			foreach (System.Data.DataRow ds in listaNumerosInternos.Tables[0].Rows)
			{
				NumerosAbonado abExternos = new NumerosAbonado();
				abExternos.Numero = (string)ds["IdAbonado"];
				abExternos.Prefijo = Convert.ToUInt32(ds["IdPrefijo"]);

				ConfiguracionUsuario.ListaAbonados.Add(abExternos);
			}

			#endregion

			#region Numero enlaces
			// Obtener identificador de la sectorización activa
			Sectorizaciones sc = new Sectorizaciones();
			sc.IdSistema = id_sistema;
			sc.Activa = true;
			List<Tablas> sectorizacion = GestorBDCD40.ListSelectSQL(sc, null);
			if (sectorizacion.Count <= 0)
				return null;

			Internos di = new Internos();
			Externos de = new Externos();
			Radio dr = new Radio();

			dr.IdSistema = de.IdSistema = di.IdSistema = id_sistema;
			dr.IdSector = de.IdSector = di.IdSector = id_usuario;
			dr.IdSectorizacion = de.IdSectorizacion = di.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss");
			//dr.IdSectorizacion = ((Sectorizaciones)sectorizacion[0]).IdSectorizacion;

			/*
			 * MODIFICIÓN HECHA A RAIZ DE CAMBIAR ACCESOABASEDEDATOS POR GESTORBASEDEDATOS *
			ConfiguracionUsuario.NumeroEnlacesInternos = GestorBDCD40.SelectCountSQL(di, "idSector='" + id_usuario + "' AND idSistema='" + id_sistema + "' AND IdSectorizacion='" + dr.IdSectorizacion + "'");
			ConfiguracionUsuario.NumeroEnlacesInternos += GestorBDCD40.SelectCountSQL(de, "idSector='" + id_usuario + "' AND idSistema='" + id_sistema + "' AND IdSectorizacion='" + dr.IdSectorizacion + "'");
			ConfiguracionUsuario.NumeroEnlacesExternos = ut.SelectCountDistinctEnlaceSQL(dr);

			 */
			ConfiguracionUsuario.NumeroEnlacesInternos = GestorBDCD40.DataSetSelectSQL(di, null).Tables[0].Rows.Count;	// (di, "idSector='" + id_usuario + "' AND idSistema='" + id_sistema + "' AND IdSectorizacion='" + dr.IdSectorizacion + "'");
			ConfiguracionUsuario.NumeroEnlacesInternos += GestorBDCD40.DataSetSelectSQL(de, null).Tables[0].Rows.Count;	//SelectCountSQL(de, "idSector='" + id_usuario + "' AND idSistema='" + id_sistema + "' AND IdSectorizacion='" + dr.IdSectorizacion + "'");
			ConfiguracionUsuario.NumeroEnlacesExternos = ut.SelectCountDistinctEnlaceSQL(dr);

			#endregion

			#region Sector
			Sectores s = new Sectores();
			s.IdSistema = id_sistema;
			s.IdSector = id_usuario;
			s.SectorSimple = true;

			List<Tablas> listaSelect = GestorBDCD40.ListSelectSQL(s, null);
			if (listaSelect.Count > 0)
				ConfiguracionUsuario.Sector = (SectoresSCV)(listaSelect[0]);
			#endregion

			#region ParametrosSector
			ParametrosSector pSector = new ParametrosSector();
			pSector.IdSistema = id_sistema;
			pSector.IdSector = id_usuario;
			listaSelect = GestorBDCD40.ListSelectSQL(pSector, null);

			if (listaSelect.Count > 0)
			{
				ConfiguracionUsuario.ParametrosDelSector = (ParametrosSectorSCVKeepAlive)listaSelect[0];
			}

			#endregion

			#region PermisosRedes
			PermisosRedes pRedes = new PermisosRedes();
			pRedes.IdSistema = id_sistema;
			pRedes.IdSector = id_usuario;

			List<Tablas> listaPermisos = GestorBDCD40.ListSelectSQL(pRedes, null);
			int tamListaPermisos = listaPermisos.Count;
			ConfiguracionUsuario.PermisosRedDelSector = new PermisosRedesSCV[tamListaPermisos];
			for (ushort num = 0; num < tamListaPermisos; num++)
			{
				ConfiguracionUsuario.PermisosRedDelSector[num] = (PermisosRedesSCV)listaPermisos[num];
			}
			#endregion

			#region TeclasSector
			TeclasSector teclasSector = new TeclasSector();

			DataSet teclas = Procedimientos.TeclasSector(GestorBDCD40.ConexionMySql, id_sistema, id_usuario);
			//listaSelect=GestorBDCD40.ListSelectSQL(tSector);
			if (teclas.Tables[0].Rows.Count > 0)
			{
				DataRow teclaRow = teclas.Tables[0].Rows[0];

				teclasSector.IdSector = (string)teclaRow["IdSector"];
				teclasSector.IdSistema = (string)teclaRow["IdSistema"];
				teclasSector.IdNucleo = (string)teclaRow["IdNucleo"];
				if (teclaRow["TransConConsultaPrev"] != System.DBNull.Value)
					teclasSector.TransConConsultaPrev = (sbyte)teclaRow["TransConConsultaPrev"] != 0;
				if (teclaRow["TransDirecta"] != System.DBNull.Value)
					teclasSector.TransDirecta = (sbyte)teclaRow["TransDirecta"] != 0;
				if (teclaRow["Conferencia"] != System.DBNull.Value)
					teclasSector.Conferencia = (sbyte)teclaRow["Conferencia"] != 0;
				if (teclaRow["Escucha"] != System.DBNull.Value)
					teclasSector.Escucha = (sbyte)teclaRow["Escucha"] != 0;
				if (teclaRow["Retener"] != System.DBNull.Value)
					teclasSector.Retener = (sbyte)teclaRow["Retener"] != 0;
				if (teclaRow["Captura"] != System.DBNull.Value)
					teclasSector.Captura = (sbyte)teclaRow["Captura"] != 0;
				if (teclaRow["Redireccion"] != System.DBNull.Value)
					teclasSector.Redireccion = (sbyte)teclaRow["Redireccion"] != 0;
				if (teclaRow["RepeticionUltLlamada"] != System.DBNull.Value)
					teclasSector.RepeticionUltLlamada = (sbyte)teclaRow["RepeticionUltLlamada"] != 0;
				if (teclaRow["RellamadaAut"] != System.DBNull.Value)
					teclasSector.RellamadaAut = (sbyte)teclaRow["RellamadaAut"] != 0;
				if (teclaRow["TeclaPrioridad"] != System.DBNull.Value)
					teclasSector.TeclaPrioridad = (sbyte)teclaRow["TeclaPrioridad"] != 0;
				if (teclaRow["Tecla55mas1"] != System.DBNull.Value)
					teclasSector.Tecla55mas1 = (sbyte)teclaRow["Tecla55mas1"] != 0;
				if (teclaRow["Monitoring"] != System.DBNull.Value)
					teclasSector.Monitoring = (sbyte)teclaRow["Monitoring"] != 0;
				if (teclaRow["CoordinadorTF"] != System.DBNull.Value)
					teclasSector.CoordinadorTF = (sbyte)teclaRow["CoordinadorTF"] != 0;
				if (teclaRow["CoordinadorRD"] != System.DBNull.Value)
					teclasSector.CoordinadorRD = (sbyte)teclaRow["CoordinadorRD"] != 0;
				if (teclaRow["IntegracionRDTF"] != System.DBNull.Value)
					teclasSector.IntegracionRDTF = (sbyte)teclaRow["IntegracionRDTF"] != 0;
				if (teclaRow["LlamadaSelectiva"] != System.DBNull.Value)
					teclasSector.LlamadaSelectiva = (sbyte)teclaRow["LlamadaSelectiva"] != 0;
				if (teclaRow["GrupoBSS"] != System.DBNull.Value)
					teclasSector.GrupoBSS = (sbyte)teclaRow["GrupoBSS"] != 0;
				if (teclaRow["LTT"] != System.DBNull.Value)
					teclasSector.LTT = (sbyte)teclaRow["LTT"] != 0;
				if (teclaRow["SayAgain"] != System.DBNull.Value)
					teclasSector.SayAgain = (sbyte)teclaRow["SayAgain"] != 0;
				if (teclaRow["InhabilitacionRedirec"] != System.DBNull.Value)
					teclasSector.InhabilitacionRedirec = (sbyte)teclaRow["InhabilitacionRedirec"] != 0;
			}
			ConfiguracionUsuario.TeclasDelSector = teclasSector;
			#endregion

			#region Niveles
			Niveles n = new Niveles();

			DataSet dsNiveles = Procedimientos.NivelesIntrusion(GestorBDCD40.ConexionMySql, id_sistema, id_usuario);

			if (dsNiveles.Tables[0].Rows.Count > 0)
			{
				DataRow rowNiveles = dsNiveles.Tables[0].Rows[0];
				if (rowNiveles["IdSector"] != System.DBNull.Value)
					n.IdSector = (string)rowNiveles["IdSector"];
				if (rowNiveles["IdNucleo"] != System.DBNull.Value)
					n.IdNucleo = (string)rowNiveles["IdNucleo"];
				if (rowNiveles["IdSistema"] != System.DBNull.Value)
					n.IdSistema = (string)rowNiveles["IdSistema"];
				if (rowNiveles["CICL"] != System.DBNull.Value)
					n.CICL = (uint)rowNiveles["CICL"];
				if (rowNiveles["CIPL"] != System.DBNull.Value)
					n.CIPL = (uint)rowNiveles["CIPL"];
				if (rowNiveles["CPICL"] != System.DBNull.Value)
					n.CPICL = (uint)rowNiveles["CPICL"];
				if (rowNiveles["CPIPL"] != System.DBNull.Value)
					n.CPIPL = (uint)rowNiveles["CPIPL"];

				ConfiguracionUsuario.NivelesDelSector = n;
			}
			#endregion

			// Nombre de la agruapacion
			ConfiguracionUsuario.Nombre = id_usuario;

			//if (s.SectorSimple)
			//    ConfiguracionUsuario.IdIdentificador = id_usuario;
			//else
			//{
			SectoresSector ss = new SectoresSector();
			ss.IdSistema = id_sistema;
			ss.IdSector = id_usuario;
			ss.EsDominante = true;

			List<Tablas> sDominante = GestorBDCD40.ListSelectSQL(ss, null);
			if (sDominante.Count > 0)
				// Nombre del usuario dominante
				ConfiguracionUsuario.IdIdentificador = ((SectoresSector)sDominante[0]).IdSectorOriginal;

			//}
			return ConfiguracionUsuario;
		}
    }

    [WebMethod (Description="Pasándole el identificador hardware de la pasarela y el sistema al que pertenece, " + 
                            "devuelve la configuración de la pasarela.")]
    public CfgPasarela GetCfgPasarela(string id_sistema, string id_hw)
    {
		lock (Sync)
		{
			Recursos r = new Recursos();
			r.IdTifX = id_hw;
			r.IdSistema = id_sistema;

			ConfiguracionPasarela.Nombre = id_hw;
			//ConfiguracionPasarela.NumRecursos = Convert.ToInt32(GestorBDCD40.SelectCountSQL(r, "idTIFX='" + id_hw + "' AND idSistema='" + id_sistema + "'"));
			ConfiguracionPasarela.NumRecursos = GestorBDCD40.DataSetSelectSQL(r, null).Tables[0].Rows.Count;
			
			TifX tifX = new TifX();
			tifX.IdSistema = id_sistema;
			tifX.IdTifx = id_hw;

			System.Data.DataSet ds = GestorBDCD40.DataSetSelectSQL(tifX, null);

			if (ds.Tables[0].Rows.Count > 0)
			{
				ConfiguracionPasarela.ModoSincronizacion = Convert.ToInt32(ds.Tables[0].Rows[0]["ModoSincronizacion"]);
				ConfiguracionPasarela.MasterSincronizacion = Convert.ToString(ds.Tables[0].Rows[0]["Master"]);
				ConfiguracionPasarela.PuertoLocalSNMP = Convert.ToUInt32(ds.Tables[0].Rows[0]["SNMPPortLocal"]);
				ConfiguracionPasarela.PuertoRemotoSNMP = Convert.ToUInt32(ds.Tables[0].Rows[0]["SNMPPortRemoto"]);
				ConfiguracionPasarela.PuertoRemotoTrapsSNMP = Convert.ToUInt32(ds.Tables[0].Rows[0]["SNMPTraps"]);
				ConfiguracionPasarela.PuertoLocalSIP = Convert.ToUInt32(ds.Tables[0].Rows[0]["SIPPortLocal"]);
				ConfiguracionPasarela.PeriodoSupervisionSIP = Convert.ToUInt32(ds.Tables[0].Rows[0]["TimeSupervision"]);

				List<Tablas> listaRecursos = ListaRecursosPasarela(id_sistema, id_hw);
				int tamListaRecursos = listaRecursos.Count;
				ConfiguracionPasarela.ListaRecursos = new RecursosSCV[tamListaRecursos];
				for (ushort num = 0; num < tamListaRecursos; num++)
				{
					ConfiguracionPasarela.ListaRecursos[num] = (RecursosSCV)listaRecursos[num];
				}
			}

			return ConfiguracionPasarela;
		}
    }

	[WebMethod(Description = "Pasándole un identificador de sistema, un identificador de recurso y" +
							" el tipo de interfaz (TI_Radio=0, TI_LCEN=1, TI_BC=2, TI_BL=3, TI_AB=4, TI_ATS_R2=5, TI_ATS_N5=6, TI_ATS_QSIG=7, TI_ISDN_2BD=8, TI_ISDN_30BD=9, TI_I_O=10, TI_DATOS=11)" +
							", retorna los parámetros correspondientes al recurso.")]
	public Tablas GetParametrosRecursoById(string idSistema, string idRecurso, int tipo)
	{
		lock (Sync)
		{
			Recursos r = new Recursos();
			r.IdSistema = idSistema;
			r.IdRecurso = idRecurso;
			r.Interface = (Tipos.TipoInterface)tipo;

			return GetParametrosRecurso(r);
		}
	}

    [WebMethod(Description = "Pasándole un recurso, retorna los parámetros correspondientes.")]
    public Tablas GetParametrosRecurso(Recursos r)
    {
		lock (Sync)
		{
			//Procedimientos pr = new Procedimientos();
			switch ((Tipos.TipoInterface)r.Interface)
			{
					/*
				case Tipos.TipoInterface.TI_DATOS:
					RecursosDatos rDatos = new RecursosDatos();
					rDatos.IdSistema = r.IdSistema;
					rDatos.IdRecurso = r.IdRecurso;
					//rDatos.TipoRecurso = (uint)Tipos.Tipo_Recurso.TR_DATOS;

					List<Tablas> tDatos = GestorBDCD40.ListSelectSQL(rDatos, null);
					if (tDatos.Count > 0)
					{
						DataSet ds = GestorBDCD40.GetDataSet(((ParametrosRecursosRadioKASiccip)tDatos[0]).GetUriRecursoDestino(), null);

						if (ds.Tables[0].Rows.Count > 0)
						{
							System.Data.DataRow drEnlace = ds.Tables[0].Rows[0];
							if (r.IdRecurso == (string)drEnlace["IdRecurso1"])
								((ParametrosRecursosRadioKASiccip)tDatos[0]).UriRecursoDestino = (string)drEnlace["IdRecurso2"] + "@" + (string)drEnlace["IdElementoHw2"];
							else if (r.IdRecurso == (string)drEnlace["IdRecurso2"])
								((ParametrosRecursosRadioKASiccip)tDatos[0]).UriRecursoDestino = (string)drEnlace["IdRecurso1"] + "@" + (string)drEnlace["IdElementoHw1"];

							return (ParametrosRecursosRadioKASiccip)tDatos[0];
						}
					}
					break;
					 */
				case Tipos.TipoInterface.TI_DATOS:
				case Tipos.TipoInterface.TI_Radio:
					RecursosRadio pRadio = new RecursosRadio();
					pRadio.IdSistema = r.IdSistema;
					pRadio.IdRecurso = r.IdRecurso;

					List<Tablas> tRadio = GestorBDCD40.ListSelectSQL(pRadio, null);
					if (tRadio.Count > 0)
					{
						((ParametrosRecursosRadioKASiccip)tRadio[0]).ListaEnlacesRecursosExternos = GetListaEnlaces((ParametrosRecursosRadioKASiccip)tRadio[0]);
						/*	DE MOMENTO NO SE VA A UTILIZAR
						DataSet ds = GestorBDCD40.GetDataSet(((ParametrosRecursosRadioKASiccip)tRadio[0]).GetUriRecursoDestino(), null);
						if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
						{
							System.Data.DataRow drEnlace = ds.Tables[0].Rows[0];
							if (r.IdRecurso == (string)drEnlace["IdRecurso1"])
								((ParametrosRecursosRadioKASiccip)tRadio[0]).UriRecursoDestino = (string)drEnlace["IdRecurso2"] + "@" + (string)drEnlace["IdElementoHw2"];
							else if (r.IdRecurso == (string)drEnlace["IdRecurso2"])
								((ParametrosRecursosRadioKASiccip)tRadio[0]).UriRecursoDestino = (string)drEnlace["IdRecurso1"] + "@" + (string)drEnlace["IdElementoHw1"];
						}
						*/



						return (ParametrosRecursosRadioKASiccip)tRadio[0];
					}
					break;
				case Tipos.TipoInterface.TI_LCEN:
					RecursosLCEN pLCEN = new RecursosLCEN();
					pLCEN.IdSistema = r.IdSistema;
					pLCEN.IdRecurso = r.IdRecurso;

					List<Tablas> tLCEN = GestorBDCD40.ListSelectSQL(pLCEN, null);
					if (tLCEN.Count > 0)
					{
						string idDestino = ((RecursosLCEN)tLCEN[0]).IdDestino;
						((ParametrosRecursoLCEN)tLCEN[0]).ListaEnlacesInternos = Procedimientos.ListaUsuariosImplicadosEnRecurso(GestorBDCD40.ConexionMySql, r.IdSistema, idDestino);
						return (ParametrosRecursoLCEN)tLCEN[0];
					}
					break;
				case Tipos.TipoInterface.TI_BC:
				case Tipos.TipoInterface.TI_BL:
				case Tipos.TipoInterface.TI_AB:
				case Tipos.TipoInterface.TI_ATS_R2:
				case Tipos.TipoInterface.TI_ATS_N5:
				case Tipos.TipoInterface.TI_ATS_QSIG:
				case Tipos.TipoInterface.TI_ISDN_2BD:
				case Tipos.TipoInterface.TI_ISDN_30BD:
					RecursosTF pTf = new RecursosTF();
					pTf.IdSistema = r.IdSistema;
					pTf.IdRecurso = r.IdRecurso;

					List<Tablas> tTf = GestorBDCD40.ListSelectSQL(pTf, null);
					if (tTf.Count > 0)
					{
						string idDestino = ((RecursosTF)tTf[0]).IdDestino;
						((ParametrosRecursoTelefonia)tTf[0]).ListaEnlacesInternos = Procedimientos.ListaUsuariosImplicadosEnRecurso(GestorBDCD40.ConexionMySql, r.IdSistema, idDestino);
						return (ParametrosRecursoTelefonia)tTf[0];
					}
					break;
				default:
					return null;
			}

			return null;
		}
    }

    [WebMethod(Description = "Pasándole el identificador del sistema devuelve el conjunto de " +
                            "parámetros que conforman la configuración común de los distintos elementos del SCV.")]
    public ConfiguracionSistema GetConfigSistema(string id_sistema)
    {
		lock (Sync)
		{
			DameParametrosGenerales(id_sistema);
			DamePlanNumeracionATS(id_sistema);
			DamePlanDireccionamientoIP(id_sistema);
			DamePlanTroncales(id_sistema);
			DamePlanRedes(id_sistema);
			DamePlanAsignacionUsuarios(id_sistema);
			DamePlanAsignacionRecursos(id_sistema);
			DamePlanDireccionamientoSIP(id_sistema);

			return CfgSistema;
		}
    }

    [WebMethod(Description = "Pasándole el identificador del sistema devuelve los parámetros generales del sistema.")]
    public ParametrosGeneralesSistema GetParametrosGenerales(string id_sistema)
    {
		lock (Sync)
		{
			DameParametrosGenerales(id_sistema);

			return CfgSistema.ParametrosGenerales;
		}
    }

    [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de numeración ATS.")]
    public NumeracionATS[] GetPlanNumeracionATS(string id_sistema)
    {
		lock (Sync)
		{
			DamePlanNumeracionATS(id_sistema);

			return CfgSistema.PlanNumeracionATS;
		}
    }

    [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de direccionamiento IP.")]
    public DireccionamientoIP[] GetPlanDireccionamientoIP(string id_sistema)
    {
		lock (Sync)
		{
			DamePlanDireccionamientoIP(id_sistema);

			return CfgSistema.PlanDireccionamientoIP;
		}
    }

    [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de troncales.")]
    public ListaTroncales[] GetPlanTroncales(string id_sistema)
    {
		lock (Sync)
		{
			DamePlanTroncales(id_sistema);

			return CfgSistema.PlanTroncales;
		}
    }

    [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de redes.")]
    public ListaRedes[] GetPlanRedes(string id_sistema)
    {
		lock (Sync)
		{
			DamePlanRedes(id_sistema);

			return CfgSistema.PlanRedes;
		}
    }

    [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de asignación de usuarios a terminales de voz.")]
    public AsignacionUsuariosTV[] GetPlanAsignacionUsuarios(string id_sistema)
    {
		lock (Sync)
		{
			DamePlanAsignacionUsuarios(id_sistema);

			return CfgSistema.PlanAsignacionUsuarios;
		}
    }

    [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de asignación de recursos a pasarelas.")]
    public AsignacionRecursosGW[] GetPlanAsignacionRecursos(string id_sistema)
    {
		lock (Sync)
		{
			DamePlanAsignacionRecursos(id_sistema);

			return CfgSistema.PlanAsignacionRecursos;
		}
    }

    [WebMethod(Description = "Pasándole el identificador del sistema devuelve el plan de direccionamiento SIP.")]
    public System.Collections.Generic.List<DireccionamientoSIP> GetPlanDireccionamientoSIP(string id_sistema)
    {
		lock (Sync)
		{
			DamePlanDireccionamientoSIP(id_sistema);

			return CfgSistema.PlanDireccionamientoSIP;
		}
    }

    [WebMethod(Description = "Pasándole el identificador del sistema, proporciona la dirección multicast y el puerto " +
                            "para notificación de una activación de configuración.")]
    public ParametrosMulticast GetParametrosMulticast(string id_sistema)
    {
		lock (Sync)
		{
			ParametrosMulticast pMulticast = new ParametrosMulticast();
			Sistema sis = new Sistema();
			sis.IdSistema = id_sistema;

			List<Tablas> sSistema = GestorBDCD40.ListSelectSQL(sis, null);
			if (sSistema.Count > 0)
			{
				pMulticast.GrupoMulticastConfiguracion = ((Sistema)sSistema[0]).GrupoMulticastConfiguracion;
				pMulticast.PuertoMulticastConfiguracion = ((Sistema)sSistema[0]).PuertoMulticastConfiguracion;

				return pMulticast;
			}

			return null;
		}
    }

    #endregion
}
