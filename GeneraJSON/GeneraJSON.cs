//------------------------------------------------------------------------------
// Copyright (C) 2018, DF NUCLEO
//------------------------------------------------------------------------------
// Product:             Ulises
// Application:         Web de configuración
//------------------------------------------------------------------------------
//! @file
//! @author         Marian Vera
//! @date creation  01/03/2018
//! @date last      01/03/2018
//! @brief         Clase de Generación de fichero JSON con los datos de configuración para el proxy (centralita interna)
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using NLog;

namespace GeneraJSON_Dll
{
    public static class CTraza
    {
        public static void EscribeLog(bool isError, string from, string msg)
        {
            StringBuilder strMsg = new StringBuilder();

            strMsg.AppendFormat("[GeneraJSON.{0}]: {1}", from, msg);

            if (isError)
                NLog.LogManager.GetLogger("GeneraJSON").Error(strMsg.ToString());
            else
                NLog.LogManager.GetLogger("GeneraJSON").Info(strMsg.ToString());

            strMsg.Clear();
        }
    }

    //----------------------------------------------------------------------------------------
    //!@brief Clase que define el objeto del fichero JSON para el proxy
    //----------------------------------------------------------------------------------------
    class CDatosParaProxy
    {
        //Almacena las reglas de encaminamiento de todos los sectores simples que forman parte de la sectorización
        public CEncaminaAbonados encamina_abonados = null;

        //Array que almacena en cada posición una lista de de telefonos de seguridad.
        //Cada elemento a su vez puede ser un rango o un número independiente
        public ArrayList telefonos_seguridad=null;
        

        //------------------------------------------------------------------------------
        //!@brief Constructor de la clase
        //------------------------------------------------------------------------------
        public CDatosParaProxy()
        {
          telefonos_seguridad = new ArrayList();
          encamina_abonados = new CEncaminaAbonados();
        }

        //------------------------------------------------------------------------------
        //!@brief Destructor de la clase
        //------------------------------------------------------------------------------    
        ~CDatosParaProxy()
        {
            if (telefonos_seguridad != null)
            {
                for (int i = 0; i < telefonos_seguridad.Count; i++)
                {
                    if (telefonos_seguridad[i]!=null)
                    {
                        ((ArrayList)telefonos_seguridad[i]).Clear();
                    }
                }
                telefonos_seguridad.Clear();
            }
        }

        //------------------------------------------------------------------------------
        //!@brief Obtiene la lista de teléfonos de la base de datos
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string] strSistema Identificador del sistema
        //------------------------------------------------------------------------------
        private bool ObtenerTelefonosSeguridad(DbCommand cmdBD,string strSistema)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strIdRecurso = string.Empty;

            using (var cmd = cmdBD)
            {

                cmd.CommandType = CommandType.Text;

                //Construimos la consulta que nos permite obtener los identificadores de los recursos con interfaz BC y BL
                //que están asignados a un equipo externo que corresponde al encaminamiento del SCV propio
                strSentencia.Append("SELECT  R.IdRecurso FROM recursostf RTLF, recursos R ");
                strSentencia.Append("WHERE   RTLF.IdSistema=R.IdSistema AND RTLF.IdRecurso=R.IdRecurso AND RTLF.TipoRecurso=R.TipoRecurso AND ");
                strSentencia.AppendFormat("RTLF.IdSistema='{0}' and (R.Interface={1} OR R.Interface={2}) AND R.TipoRecurso=1  AND ",strSistema,Constantes.INTERFAZ_REC_TLF_BC, Constantes.INTERFAZ_REC_TLF_BL);
                strSentencia.Append("EXISTS (SELECT 1 FROM equiposeu EU WHERE EU.IdSistema=R.IdSistema AND EU.idEquipos=R.idEquipos AND EU.interno=true) ");
                strSentencia.Append("ORDER BY R.IdRecurso");
                cmd.CommandText = strSentencia.ToString();

                using (DbDataReader objReader = cmd.ExecuteReader())
                {
                    try
                    {
                        if (objReader.HasRows)
                        {
                            while (objReader.Read())
                            {
                                strIdRecurso = objReader.GetString(0);
                                if (!String.IsNullOrEmpty(strIdRecurso))
                                {
                                    ArrayList EltoTelfSeg = new ArrayList();
                                    EltoTelfSeg.Add(strIdRecurso);
                                    telefonos_seguridad.Add(EltoTelfSeg);
                                }
                            }
                        }
                        bCorrecto = true;
                    }
                    catch (Exception objEx)
                    {
                        StringBuilder strMsg = new StringBuilder();
                        strMsg.AppendFormat("Error en los telefonos de seguridad para el sistema={0}. Error: {1}", strSistema, objEx.Message.ToString());
                        CTraza.EscribeLog(true, "ObtenerTelefonosSeguridad", strMsg.ToString());
                        strMsg.Clear();
                    }
                    finally
                    {
                        objReader.Close();
                    }
                }
            }

            return (bCorrecto);
        }

        //------------------------------------------------------------------------------
        //!@brief Obtiene la lista de sectores simples
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string] strSistema Identificador del sistema
        //!@param[string] arrSectores Lista con los sectores simples obtenidos
        //------------------------------------------------------------------------------
        private bool ObtenerListaSectores(DbCommand cmdBD, string strSistema, ref ArrayList arrSectores)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strIdSector = string.Empty;
            using (var cmd = cmdBD)
            {
                if (arrSectores == null)
                {
                    arrSectores = new ArrayList();
                }
                else
                {
                    if (arrSectores.Count>=0)
                        arrSectores.Clear();
                }

                cmd.CommandType = CommandType.Text;

                //Construimos la consulta que nos permite obtener la lista de sectores simples
                strSentencia.AppendFormat("SELECT IdSector FROM sectores where SectorSimple=true and IdSistema='{0}' order by IdSector", strSistema);
                cmd.CommandText = strSentencia.ToString();

                using (DbDataReader objReader = cmd.ExecuteReader())
                {
                    try
                    {
                        //Leemos el primer registro
                        objReader.Read();
                        if (objReader.HasRows)
                        {
                            strIdSector = objReader.GetString(0);
                         
                            if (!String.IsNullOrEmpty(strIdSector))
                            {
                                arrSectores.Add(strIdSector);
                            }

                            while (objReader.Read())
                            {
                                strIdSector = objReader.GetString(0);
                                if (!String.IsNullOrEmpty(strIdSector))
                                {
                                    arrSectores.Add(strIdSector);
                                }
                            }
                        }
                        bCorrecto = true;
                    }
                    catch (Exception objEx)
                    {
                        StringBuilder strMsg = new StringBuilder();
                        strMsg.AppendFormat("Error al obtener la lista de sectores simples para el sistema={0}. Error: {1}", strSistema, objEx.Message.ToString());
                        CTraza.EscribeLog(true, "ObtenerListaSectores", strMsg.ToString());
                        strMsg.Clear();
                    }
                    finally
                    {
                        objReader.Close();
                    }
                }
            }

            return (bCorrecto);
        }

        //------------------------------------------------------------------------------
        //!@brief Obtiene de la BD mysql los datos de configuración para el proxy
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string] pIdSistema Identificador del sistema
        //------------------------------------------------------------------------------
        public bool ObtenerDatosCnfParaProxy(DbCommand cmdBD, string pIdSistema)
        {
            bool bCorrecto = false;

            try
            {
                if (!string.IsNullOrEmpty(pIdSistema))
                {
                    bCorrecto=ObtenerTelefonosSeguridad(cmdBD, pIdSistema);

                    if (bCorrecto)
                    {
                        bCorrecto=encamina_abonados.ObtenerUsuariosSCV(cmdBD, pIdSistema);
                    }

                }

            }
            catch (Exception objEx)
            {
                StringBuilder strMsg = new StringBuilder();
                strMsg.AppendFormat("Error en ObtenerDatosCnfParaProxy para el sistema={0}. Error: {1}", pIdSistema, objEx.Message.ToString());
                CTraza.EscribeLog(true, "GeneraDestinosAgendaSectorizados", strMsg.ToString());
                strMsg.Clear();
            }
            return (bCorrecto);
        }


        //------------------------------------------------------------------------------
        //!@brief Obtiene de la base de datos la dirección IP del proxy del encaminamiento propio
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string] pIdSistema Identificador del sistema
        //!@param[string] pstrDirIP Dirección IP del proxy
        //------------------------------------------------------------------------------
        public bool ObtenerDirIpProxy(DbCommand cmdBD, string strSistema,ref string pstrDirIP)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strDirIP = string.Empty;

            pstrDirIP = string.Empty;

            using (var cmd = cmdBD)
            {

                try
                {
                    cmd.CommandType = CommandType.Text;

                    //Construimos la consulta que nos permite obtener la dirección IP del proxy
                    strSentencia.AppendFormat("SELECT  E.IpRed1 FROM equiposeu E WHERE IdSistema='{0}' AND E.interno=true AND E.min=-1 and E.max=-1", strSistema);
                    strSentencia.Append(" AND EXISTS (SELECT 1 FROM ENCAMINAMIENTOS C WHERE C.IdSistema=E.IdSistema AND C.Central=E.IdEquipos AND c.CentralPropia=true AND C.CentralIP=true)");
                    cmd.CommandText = strSentencia.ToString();

                    using (DbDataReader objReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            //Leemos el primer registro
                            objReader.Read();
                            if (objReader.HasRows)
                            {
                                strDirIP = objReader.GetString(0);
                                if (!String.IsNullOrEmpty(strDirIP))
                                {
                                    //Eliminamos los espacios en blanco por si los tuviera
                                    strDirIP.Trim();
                                    //El campo dirección IP puede venir con o sin el puerto, por lo que hay que eliminarlo.
                                    string[] arrAuxstr = strDirIP.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (arrAuxstr.Length > 0)
                                    {
                                        pstrDirIP = arrAuxstr[0];
                                        bCorrecto = true;
                                    }
                                }
                            }
                        }
                        catch (Exception objEx)
                        {
                            StringBuilder strMsg = new StringBuilder();
                            strMsg.AppendFormat("Error al obtener la dirección IP del proxy interno sistema={0}. Error: {1}", strSistema, objEx.Message.ToString());
                            CTraza.EscribeLog(true, "ObtenerDirIpProxy", strMsg.ToString());
                            strMsg.Clear();
                        }
                        finally
                        {
                            objReader.Close();
                        }
                    }
                }
                catch (Exception objEx)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error al obtener la dirección IP del proxy interno sistema={0}. Error: {1}", strSistema, objEx.Message.ToString());
                    CTraza.EscribeLog(true, "ObtenerDirIpProxy", strMsg.ToString());
                    strMsg.Clear();
                }
            }

            return (bCorrecto);
        }
    }

    //------------------------------------------------------------------------------
    //!@brief Clase CFicheroDatos
    //------------------------------------------------------------------------------
    public class CFicheroDatos
    {
        string strDirFtpProxy;

        public CFicheroDatos()
        {
            strDirFtpProxy = string.Empty;
        }

        public string GetDirFtpProxy() {return strDirFtpProxy;}

        //-----------------------------------------------------------------------------------------
        //!@brief Obtiene la dirección IP del servidor proxy interno configurado en la BD  
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string] strIdSistema Identificador del sistema
        //----------------------------------------------------------------------------------------
        private bool ObtenerDireccionIPProxy(DbCommand cmdBD, string strSistema)
        {
            bool bCorrecto = false;
            string strDirFtp=string.Empty;

            //Se obtiene la dirección IP del proxy interno, si no está configurado no se genera el fichero
            CDatosParaProxy objDatos = new CDatosParaProxy();

            //Se obtiene la dirección IP interna del proxy
            if (objDatos.ObtenerDirIpProxy(cmdBD, strSistema, ref strDirFtp))
            {
                strDirFtpProxy=strDirFtp;
                bCorrecto = true;
            }

            return bCorrecto;
        }
        //WMG 11/12/2018
        //-----------------------------------------------------------------------------------------
        //!@brief Genera el fichero JSON con los datos de configuración para el proxy. Al llamar a esta función
        //        la conexión con la BD debe estar previamente establecida
        //!@param[MySqlCommand] pcmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string] strIdSistema Identificador del sistema
        //!@param[string] strNombreFichero Nombre del fichero JSON con ruta
        //----------------------------------------------------------------------------------------
        private bool GeneraFicheroProxy( MySqlCommand pcmdBD, string strIdSistema, string strNombreFichero)
        {
            bool bCorrecto = false;
            MySqlCommand cmdBD = null;

            if (!string.IsNullOrEmpty(strNombreFichero) && !string.IsNullOrEmpty(strIdSistema) )
            {
                try
                {
                    if (pcmdBD != null)
                    {
                        string strDirFich = Path.GetDirectoryName(strNombreFichero);

                        //Comprobamos si el directorio existe
                        if (strDirFich.Length > 0 && !Directory.Exists(strDirFich))
                        {
                            CTraza.EscribeLog(false, "GeneraFicheroProxy", "No se ha podido generar el fichero. El directorio " + strDirFich + " no existe");
                        }
                        else
                        {
                            using (cmdBD = pcmdBD)
                            {
                                if (cmdBD.Connection.State == ConnectionState.Open)
                                {
                                    //Se obtiene la dirección IP del proxy interno, si no está configurado no se genera el fichero
                                    CDatosParaProxy objDatos = new CDatosParaProxy();

                                    StringBuilder strOutputJSON = new StringBuilder();
                                    JavaScriptSerializer objSerializa = new JavaScriptSerializer();
                                    //VMG 11/12/2018
                                    objDatos.ObtenerDatosCnfParaProxy(cmdBD, strIdSistema);

                                    strOutputJSON.Append(objSerializa.Serialize(objDatos));

                                    try
                                    {
                                        File.WriteAllText(strNombreFichero, strOutputJSON.ToString());
                                        bCorrecto = true;
                                    }
                                    catch (IOException exFich)
                                    {
                                        CTraza.EscribeLog(true, "GeneraFicheroProxy", string.Format("Error al guardar en disco el fichero json: sistema={0}, fichero={1}.Error:", strIdSistema, strNombreFichero) + exFich.Message.ToString());
                                    }
                                }
                                else
                                {
                                    CTraza.EscribeLog(false, "GeneraFicheroProxy", "La conexión con la BD no está abierta. BD: " + cmdBD.Connection.ConnectionString);
                                }
                            }
                        }
                    }
                    else
                    {
                        CTraza.EscribeLog(false, "GeneraFicheroProxy", "Conexión a BD no definida ");
                    }
                }
                catch(Exception ex)
                {
                    CTraza.EscribeLog(true, "GeneraFicheroProxy", string.Format("Error al generar el fichero json para el proxy: sistema={0}, fichero={1}.Error:", strIdSistema, strNombreFichero) + ex.ToString());

                    if (cmdBD != null && cmdBD.Connection.State == ConnectionState.Open)
                    {
                        cmdBD.Connection.Close();
                    }
                }
            }
            else
            {
                CTraza.EscribeLog(false, "GeneraFicheroProxy", "El nombre del fichero,identificador del sistema no se ha informado");
            }

            return (bCorrecto);
        }

        //----------------------------------------------------------------------------------------
        //!@brief Genera el fichero JSON con los datos de configuración para el proxy y lo envía por FTP.
        //        
        //!@param[MySqlConnection] objConexionBD Objeto de conexión a la BD de MySql
        //!@param[string] strIdSistema Identificador del sistema
        //!@param[string] strNombreFichero Nombre del fichero JSON con ruta
        //!@param[string] bObtenerIPProxy  Indica si se debe obtener la dirección IP del proxy local de la BD
        //-------------------------------------------------------------------------------------------------
        public bool GeneraFicheroProxy(MySqlConnection objConexionBD, string strIdSistema, string strNombreFichero,bool bObtenerIPProxy)
        {
            bool bCorrecto = false;
            MySqlCommand cmdBD = null;

            if (!string.IsNullOrEmpty(strNombreFichero) && !string.IsNullOrEmpty(strIdSistema))
            {
                try
                {
                    if (objConexionBD != null)
                    {
                        using (cmdBD = objConexionBD.CreateCommand())
                        {
                            if (cmdBD.Connection.State != ConnectionState.Open)
                            {
                                //Se abre la conexión con la BD
                                cmdBD.Connection.Open();
                            }

                            if (cmdBD.Connection.State == ConnectionState.Open)
                            {
                                //Se obtiene la dirección IP del proxy interno, si no está configurado no se genera el fichero

                                if (bObtenerIPProxy)
                                {
                                    ObtenerDireccionIPProxy(cmdBD, strIdSistema);
                                }

                                if (GeneraFicheroProxy(cmdBD, strIdSistema, strNombreFichero))
                                {
                                    bCorrecto = true;
                                }

                                //Se cierra la conexión con la BD
                                cmdBD.Connection.Close();
                            }
                            else
                            {
                                CTraza.EscribeLog(false, "GeneraFicheroProxy", "No se ha podido establecer la conexión a la BD " + objConexionBD.ConnectionString);
                            }
                       }
                    }
                    else
                    {
                        CTraza.EscribeLog(false, "GeneraFicheroProxy", "Conexión no establecida con la BD");
                    }
                }
                catch (Exception ex)
                {
                    CTraza.EscribeLog(true, "GeneraFicheroProxy", string.Format("Error al generar el fichero proxy: sistema={0}, fichero={1}.Error:", strIdSistema, strNombreFichero) + ex.ToString());

                    if (cmdBD != null && cmdBD.Connection.State == ConnectionState.Open)
                    {
                        cmdBD.Connection.Close();
                    }
                }
            }
            else
            {
                CTraza.EscribeLog(false, "GeneraFicheroProxy", "El nombre del fichero o el identificador del sistema no se ha informado");
            }

            return (bCorrecto);
        }
    }

    //------------------------------------------------------------------------------
    //!@brief Clase que implementa algunas funciones de envío de ficheros por FTP
    //------------------------------------------------------------------------------
    public class CClienteFtp
    {
        private string m_strServer;
        private string m_strUser;
        private string m_strPwd;
        private string m_strURLServer;
        const int TAM_BUFFER = 2048; //tamaño del buffer de envío de ficheros en bytes

        //-----------------------------------------------------------------------------
        //!@brief constructor de la clase CClienteFtp
        //-----------------------------------------------------------------------------
        public CClienteFtp(string strServer, string strUser, string strPwd)
        {
            m_strServer=strServer;
            m_strUser=strUser;
            m_strPwd=strPwd;

            if (!string.IsNullOrEmpty(m_strServer))
                m_strURLServer = string.Format("ftp://{0}/", m_strServer);
            else
                m_strURLServer = string.Empty;
        }

        //-----------------------------------------------------------------------------
        //!@brief Destructor de la clase ClienteFtp
        //-----------------------------------------------------------------------------
        ~CClienteFtp()
        {

        }

        //-----------------------------------------------------------------------------
        //!@brief SendFile Envío el fichero por FTP al servidor en modo binario
        //!@param[string] strFicheroLocal   Nombre con ruta del fichero local a enviar
        //!@param[string] StrFicheroRemoto  Nombre con ruta del fichero Remoto
        //-----------------------------------------------------------------------------
        protected bool SendFile(string strFicheroLocal, string StrFicheroRemoto)
        {
            bool bCorrecto=false;
            StringBuilder strMsgError = new StringBuilder();
            FtpWebRequest objFtpRequest = null;

            try
            {
                Stream ftpStream = null;

                // Se crea  el objeto para establecer la conexión FTP Request 
                objFtpRequest = (FtpWebRequest)FtpWebRequest.Create(m_strURLServer  + StrFicheroRemoto);
                // Se establece la conexión FTP con el usuario y el Password 
                objFtpRequest.Credentials = new NetworkCredential(m_strUser, m_strPwd);
                // Se envia el fichero en binario
                objFtpRequest.UseBinary = true;
                objFtpRequest.UsePassive = true;
                objFtpRequest.KeepAlive = true;
                // Indicamos el tipo de operación FTP
                objFtpRequest.Method = WebRequestMethods.Ftp.UploadFile;


                try
                {
                    // Se obtiene el stream de la petición de envío al servidor
                    ftpStream = objFtpRequest.GetRequestStream();

                    // Se lee el fichero que se va a enviar
                    using (FileStream objFileS_local = new FileStream(strFicheroLocal, FileMode.Open, FileAccess.Read))
                    {
                        // Buffer de envío de datos del fichero
                        byte[] byteBuffer = new byte[TAM_BUFFER];
                        int ibytesSent = objFileS_local.Read(byteBuffer, 0, TAM_BUFFER);
                        long bytesSentCounter = 0;

                        // Se envía el fichero poco a poco 
                        try
                        {
                            while (ibytesSent != 0)
                            {
                                ftpStream.Write(byteBuffer, 0, ibytesSent);
                                bytesSentCounter += ibytesSent;
                                ibytesSent = objFileS_local.Read(byteBuffer, 0, TAM_BUFFER);
                            }

                            objFileS_local.Close();
                            ftpStream.Close();

                            FtpWebResponse objFtpResponse = (FtpWebResponse)objFtpRequest.GetResponse();

                            //Comprobamos si el fichero se ha enviado correctamente
                            if (objFtpResponse.StatusCode == FtpStatusCode.ClosingData)
                                bCorrecto = true;
                            else
                            {
                                strMsgError.AppendFormat("El Fichero local ({0}) no enviado correctamente al servidor FTP (fichero remodo={1}). Mensaje:{2}-{3}", strFicheroLocal, StrFicheroRemoto, objFtpResponse.StatusCode,objFtpResponse.StatusDescription);
                            }

                            objFtpResponse.Close();
                        }
                        catch (Exception ex)
                        {
                            strMsgError.AppendFormat("Error al enviar el fichero {0} al servidor {1}, fichero remoto {2}.Error:{3}", strFicheroLocal, m_strURLServer,StrFicheroRemoto, ex.Message.ToString());
                            CTraza.EscribeLog(false, "ClienteFtp.SendFile", strMsgError.ToString());
                        }
                        finally
                        {
                            // Se cierran los stream y see liberan los recursos
                            ftpStream.Dispose();
                            objFileS_local.Dispose();
                            objFtpRequest = null;
                        }
                    }
                }
                catch (WebException exWeb)
                {
                    strMsgError.AppendFormat("Error establecer la conexión con el servidor {0}.Error:{1}", m_strURLServer, exWeb.Message.ToString());
                    CTraza.EscribeLog(false, "ClienteFtp.SendFile", strMsgError.ToString());
                }
            }
            catch (Exception ex)
            {
                CTraza.EscribeLog(false, "ClienteFtp.SendFile", ex.ToString());
            }

            return bCorrecto;
        }

        //-----------------------------------------------------------------------------
        //!@brief RenombraFichero Renombre el fichero especificado en el parámetro strFicheroRemotoConRuta 
        //        con el nombre indicado en el parámetro strNuevoNombre
        //-----------------------------------------------------------------------------
        protected bool RenombraFichero(string strFicheroRemotoConRuta, string strNuevoNombre)
        {
            bool bCorrecto = false;
            StringBuilder strMsgError = new StringBuilder();
            FtpWebRequest objFtpRequest = null;
            FtpWebResponse objFtpResponse = null;

            try
            {
                // Se crea  el objeto para establecer la conexión FTP Request 
                objFtpRequest = (FtpWebRequest)FtpWebRequest.Create(m_strURLServer+ strFicheroRemotoConRuta);
                // Se establece la conexión FTP con el usuario y el Password 
                objFtpRequest.Credentials = new NetworkCredential(m_strUser, m_strPwd);
                // Se envia el fichero en binario
                objFtpRequest.UseBinary = true;
                objFtpRequest.UsePassive = true;
                objFtpRequest.KeepAlive = true;
                // Indicamos el tipo de operación FTP
                objFtpRequest.Method = WebRequestMethods.Ftp.Rename;

                //Se indica el nuevo nombre del fichero
                objFtpRequest.RenameTo = strNuevoNombre;

                try
                {
                    // Se establece la comunicación con el servidor FTP
                    objFtpResponse = (FtpWebResponse)objFtpRequest.GetResponse();

                    if (objFtpResponse != null)
                    {
                        if (objFtpResponse.StatusCode == FtpStatusCode.FileActionOK)
                            bCorrecto = true;

                        objFtpResponse.Close();
                    }

                    objFtpRequest = null;
                }
                catch (WebException exWeb)
                {
                    strMsgError.AppendFormat("Error establecer la conexión con el servidor {0} para renombrar el fichero {1} a {2}.Error:{3}", m_strURLServer, strFicheroRemotoConRuta, strNuevoNombre, exWeb.Message.ToString());
                    CTraza.EscribeLog(false, "ClienteFtp.RenombraFichero", strMsgError.ToString());

                    if (objFtpResponse != null)
                    {
                        objFtpResponse.Close();
                        objFtpResponse = null;
                    }
                }
            }
            catch (Exception ex)
            {
                strMsgError.Clear();
                strMsgError.AppendFormat("Error al renombrar el fichero {0} a {1}.Error:{2}", strFicheroRemotoConRuta, strNuevoNombre, ex.Message.ToString());
                CTraza.EscribeLog(false, "ClienteFtp.RenombraFichero", strMsgError.ToString());
            }

            return bCorrecto;
        }

        //-----------------------------------------------------------------------------
        //!@brief FicheroExiste Comprueba si el fichero strFicheroRemotoConRuta 
        //        existe en el servidor Remoto 
        //-----------------------------------------------------------------------------
        protected bool FicheroExiste(string strFicheroRemotoConRuta,ref bool bExiste)
        {
            bool bCorrecto = false;
            StringBuilder strMsgError = new StringBuilder();
            FtpWebRequest objFtpRequest = null;
            FtpWebResponse objFtpResponse = null;

            try
            {
                // Se crea  el objeto para establecer la conexión FTP Request 
                objFtpRequest = (FtpWebRequest)FtpWebRequest.Create(m_strURLServer + strFicheroRemotoConRuta);
                // Se establece la conexión FTP con el usuario y el Password 
                objFtpRequest.Credentials = new NetworkCredential(m_strUser, m_strPwd);
                // Se envia el fichero en binario
                objFtpRequest.UseBinary = true;
                objFtpRequest.UsePassive = true;
                objFtpRequest.KeepAlive = true;
                // Indicamos el tipo de operación FTP
                objFtpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;

                try
                {
                    // Se establece la comunicación con el servidor FTP y se obtiene la respuesta
                    objFtpResponse = (FtpWebResponse)objFtpRequest.GetResponse();

                    if (objFtpResponse != null)
                    {
                        objFtpResponse.Close();
                    }

                    bExiste = true;
                    bCorrecto = true;
                }
                catch (WebException)
                {
                    bExiste = false;
                    bCorrecto = true;
                }
            }
            catch (Exception ex)
            {
                strMsgError.Clear();
                strMsgError.AppendFormat("Error al comprobar si el fichero {0} existe en el servidor FTP.Error:{1}", strFicheroRemotoConRuta, ex.Message.ToString());
                CTraza.EscribeLog(false, "ClienteFtp.FicheroExiste", strMsgError.ToString());

                bExiste = false;
            }
            finally
            {
                if (objFtpResponse != null)
                {
                    objFtpResponse.Close();
                    objFtpResponse = null;
                }
            }

            return bCorrecto;
        }


        //-----------------------------------------------------------------------------
        //!@brief FicheroExiste Comprueba si el fichero strFicheroRemotoConRuta 
        //        existe en el servidor Remoto 
        //-----------------------------------------------------------------------------
        protected bool BorrarFichero(string strFicheroRemotoConRuta, ref bool bEliminado)
        {
            bool bCorrecto = false;
            StringBuilder strMsgError = new StringBuilder();
            FtpWebRequest objFtpRequest = null;
            FtpWebResponse objFtpResponse = null;

            try
            {
                // Se crea  el objeto para establecer la conexión FTP Request 
                objFtpRequest = (FtpWebRequest)FtpWebRequest.Create(m_strURLServer + strFicheroRemotoConRuta);
                // Se establece la conexión FTP con el usuario y el Password 
                objFtpRequest.Credentials = new NetworkCredential(m_strUser, m_strPwd);
                // Se envia el fichero en binario
                objFtpRequest.UseBinary = true;
                objFtpRequest.UsePassive = true;
                objFtpRequest.KeepAlive = true;
                
                // Indicamos  que se quiere borrar el fichero
                objFtpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                try
                {
                    // Se establece la comunicación con el servidor FTP y se obtiene la respuesta
                    using (objFtpResponse = (FtpWebResponse)objFtpRequest.GetResponse())
                    {
                        if (objFtpResponse.StatusCode == FtpStatusCode.FileActionOK)
                        {
                            bEliminado = true;
                        }
                    }

                    bCorrecto = true;
                }
                catch (WebException)
                {
                    bEliminado = false;
                    bCorrecto = true;
                }
            }
            catch (Exception ex)
            {
                strMsgError.Clear();
                strMsgError.AppendFormat("Error al comprobar si el fichero {0} existe en el servidor FTP.Error:{1}", strFicheroRemotoConRuta, ex.Message.ToString());
                CTraza.EscribeLog(false, "ClienteFtp.FicheroExiste", strMsgError.ToString());

                bEliminado = false;
            }
            finally
            {
                if (objFtpResponse != null)
                {
                    objFtpResponse.Close();
                    objFtpResponse = null;
                }
            }

            return bCorrecto;
        }
        //-----------------------------------------------------------------------------
        //!@brief SendFile Envío el fichero por FTP al servidor en modo binario. Si el indicador bEnviarFicheroTempName es true
        //        el fichero se envía al servidor con un nombre temporal y posteriormente se renombra con el nombre original
        //-----------------------------------------------------------------------------
        public bool SendFile(string strFicheroLocal, string strFicheroRemoto,bool bEnviarFicheroTempName=false)
        {
            bool bCorrecto = false;

            try
            {
                if (false==bEnviarFicheroTempName)
                {
                    SendFile(strFicheroLocal, strFicheroRemoto);

                }
                else
                {
                    //Se compone el nombre temporal con el que se va a enviar el fichero
                    StringBuilder strNombreRemotoTemp = new StringBuilder();
                    bool bExisteFichOrig=false;

                    strNombreRemotoTemp.AppendFormat("{0}.tmp{1}", strFicheroRemoto, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                    if (SendFile(strFicheroLocal, strNombreRemotoTemp.ToString()))
                    {
                        //Se comprueba si el fichero con el nombre definitivo existe
                        bCorrecto=FicheroExiste(strFicheroRemoto,ref bExisteFichOrig);

                        if (bCorrecto)
                        {
                            if (bExisteFichOrig)
                            {
                                bool bEliminado=false;
                                //Elimina el fichero que tiene el mismo nombre
                                bCorrecto=BorrarFichero(strFicheroRemoto, ref  bEliminado);
                            }
                            
                            //Se renombra el fichero temporal con el nombre original

                            bCorrecto=RenombraFichero(strNombreRemotoTemp.ToString(), strFicheroRemoto);
                            strNombreRemotoTemp.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StringBuilder strMsgError = new StringBuilder();

                strMsgError.AppendFormat("Error al enviar el fichero {0} al servidor {1}, fichero remoto {2} (bEnviarFicheroTempName={3}).Error:{4}", strFicheroLocal, m_strURLServer,strFicheroRemoto,bEnviarFicheroTempName,ex.Message.ToString());
                CTraza.EscribeLog(false, "ClienteFtp.SendFile", strMsgError.ToString());
                strMsgError.Clear();
            }

            return bCorrecto;
        }

    }

}
