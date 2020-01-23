//------------------------------------------------------------------------------
// Copyright (C) 2018, DF NUCLEO
//------------------------------------------------------------------------------
// Product:             Ulises
// Application:         
//------------------------------------------------------------------------------
//! @file
//! @author         Marian Vera
//! @date creation  01/03/2018
//! @date last      01/03/2018
//! @brief         Clase de Generación de fichero JSON con los datos de configuración para el proxy (centralita interna)
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using NLog;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;

namespace GeneraJSON_Dll
{
    public class CDatosParaProxy
    {
        //Array bidimensional que almacen la lista de telefonos de seguridad.
        //Cada elemento a su vez puede ser un rango
        public ArrayList telefonos_seguridad=null;

        //------------------------------------------------------------------------------
        //!@brief Constructor de la clase
        //------------------------------------------------------------------------------
        public CDatosParaProxy()
        {
          telefonos_seguridad = new ArrayList();
        }

        //------------------------------------------------------------------------------
        //!@brief Destructor de la clase
        //------------------------------------------------------------------------------    
        ~CDatosParaProxy()
        {
            if (telefonos_seguridad != null)
            {
                /*
                for (int i=0; i<telefonos_seguridad.Count;i++)
                {
                    if (telefonos_seguridad[i]!=null)
                    {
                        ((ArrayList)telefonos_seguridad[i]).Clear();
                    }
                }
                 */

                telefonos_seguridad.Clear();
            }

        }

        //------------------------------------------------------------------------------
        //!@brief Obtiene la lista de teléfonos de la base de datos
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string] pIdSistema Identificador del sistema
        //------------------------------------------------------------------------------
        private bool ObtenerTelefonosSeguridad(DbCommand cmdBD,string strSistema)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strIdRecurso = string.Empty;

            using (var cmd = cmdBD)
            {

                cmd.CommandType = CommandType.Text;

                /* SELECT  R.IdRecurso FROM recursostf RTLF, recursos R 
                    WHERE   RTLF.IdSistema=R.IdSistema AND RTLF.IdRecurso=R.IdRecurso AND RTLF.TipoRecurso=R.TipoRecurso AND
                            RTLF.IdSistema='departamento' and (R.Interface= 2 OR R.Interface=3) AND R.TipoRecurso=1  AND
                            EXISTS (SELECT 1 FROM equiposeu EU WHERE EU.IdSistema=R.IdSistema AND EU.idEquipos=R.idEquipos AND EU.interno=true)
                    ORDER BY R.IdRecurso;
                */
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
                        //Leemos el primer registro
                        objReader.Read();
                        if (objReader.HasRows)
                        {
                            strIdRecurso = objReader.GetString(0);
                            if (!String.IsNullOrEmpty(strIdRecurso))
                            {
                                telefonos_seguridad.Add(strIdRecurso);
                            }

                            while (objReader.Read())
                            {
                                strIdRecurso = objReader.GetString(0);
                                if (!String.IsNullOrEmpty(strIdRecurso))
                                {
                                    telefonos_seguridad.Add(strIdRecurso);
                                }
                            }
                        }
                    }
                    catch (Exception objEx)
                    {
                        StringBuilder strMsg = new StringBuilder();
                        strMsg.AppendFormat("Error en los telefonos de seguridad para el sistema={0}. Error: {1}", strSistema, objEx.Message.ToString());
                        Log(true, "ObtenerTelefonosSeguridad", strMsg.ToString());
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
                    ObtenerTelefonosSeguridad(cmdBD, pIdSistema);
                }

            }
            catch (Exception objEx)
            {
                StringBuilder strMsg = new StringBuilder();
                strMsg.AppendFormat("Error en ObtenerDatosCnfParaProxy para el sistema={0}. Error: {1}", pIdSistema, objEx.Message.ToString());
                Log(true, "GeneraDestinosAgendaSectorizados", strMsg.ToString());
                strMsg.Clear();
            }
            return (bCorrecto);
        }

        void Log(bool isError, string from, string msg)
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

    //------------------------------------------------------------------------------
    //!@brief Clase GeneraJSON
    //------------------------------------------------------------------------------
    class GeneraJSON
    {

        void Log(bool isError, string from, string msg)
        {
            StringBuilder strMsg = new StringBuilder();

            strMsg.AppendFormat("[GeneraJSON.{0}]: {1}", from, msg);

            if (isError)
                NLog.LogManager.GetLogger("GeneraJSON").Error(strMsg.ToString());
            else
                NLog.LogManager.GetLogger("GeneraJSON").Info(strMsg.ToString());

            strMsg.Clear();
        }

        //-----------------------------------------------------------------------------------------
        //!@brief Genera el fichero JSON con los datos de configuración para el proxy
        //!@param[MySqlConnection] objConexionBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string] strIdSistema Identificador del sistema
        //!@param[string] strNombreFichero Nombre del fichero JSON con ruta
        //-----------------------------------------------------------------------------------------
        public bool GeneraFicheroProxy(MySqlConnection objConexionBD, string strIdSistema, string strNombreFichero)
        {
            bool bCorrecto = false;

            if (!string.IsNullOrEmpty(strNombreFichero) && !string.IsNullOrEmpty(strIdSistema) )
            {
                try
                {
                    if (objConexionBD != null && objConexionBD.State == ConnectionState.Open)
                    {
                        string strDirFich = Path.GetDirectoryName(strNombreFichero);

                        //Comprobamos si el directorio existe
                        if (strDirFich.Length>0 && !File.Exists(strDirFich))
                        {
                            Log(false, "GeneraFicheroProxy", "No se ha podido generar el fichero. El directorio " + strDirFich+ "no existe");
                        }
                        else
                        {
                            using (MySqlCommand cmdBD = objConexionBD.CreateCommand())
                            {
                                StringBuilder strOutputJSON = new StringBuilder();
                                JavaScriptSerializer objSerializa = new JavaScriptSerializer();
                                CDatosParaProxy objDatos = new CDatosParaProxy();

                                objDatos.ObtenerDatosCnfParaProxy(cmdBD, strIdSistema);

                                strOutputJSON.Append(objSerializa.Serialize(objDatos));
                                File.WriteAllText(strNombreFichero, strOutputJSON.ToString());
                                bCorrecto = true;
                            }
                        }
                    }
                    else
                    {
                        Log(false, "GeneraFicheroProxy", "Conexión no establecida con la BD");
                    }
                }
                catch(Exception ex)
                {
                    Log(true, "GeneraFicheroProxy", string.Format("Error al generar el fichero proxy: sistema={0}, fichero={1}.Error:", strIdSistema, strNombreFichero) + ex.ToString());
                }
            }
            else
            {
                Log(false, "GeneraFicheroProxy", "El nombre del fichero,identificador del sistema no se ha informado");

            }

            return (bCorrecto);
        }
    }

    //------------------------------------------------------------------------------
    //!@brief Clase que implementa algunas funciones de envío por FTP de ficheros
    //------------------------------------------------------------------------------
    class ClienteFtp
    {
        private string m_strServer;
        private string m_strUser;
        private string m_strPwd;
        private string m_strURLServer;
        private FtpWebRequest objFtpRequest = null;
        const int TAM_BUFFER = 2048;

        //-----------------------------------------------------------------------------
        //!@brief constructor de la clase ClienteFtp
        //-----------------------------------------------------------------------------
        public ClienteFtp(string strServer, string strUser, string strPwd)
        {
            m_strServer=strServer;
            m_strUser=strUser;
            m_strPwd=strPwd;

            if (!string.IsNullOrEmpty(m_strServer))
                m_strURLServer=string.Format("ftp://{0}/",m_strServer);
        }

        //-----------------------------------------------------------------------------
        //!@brief Destructor de la clase ClienteFtp
        //-----------------------------------------------------------------------------
        ~ClienteFtp()
        {
            if (objFtpRequest != null)
            {
                objFtpRequest = null;
            }

        }

        //-----------------------------------------------------------------------------
        //!@brief SendFile Envío el fichero por FTP al servidor en modo binario
        //-----------------------------------------------------------------------------
        public bool SendFile(string strFicheroLocal, string StrFicheroRemoto)
        {
            bool bCorrecto=false;
            StringBuilder strMsgError = new StringBuilder();

            try
            {
                Stream ftpStream = null;

                // Se crea  el objeto para establecer la conexión FTP Request 
                objFtpRequest = (FtpWebRequest)FtpWebRequest.Create(m_strURLServer + "/" + StrFicheroRemoto);
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
                    using (FileStream objFS_local = new FileStream(strFicheroLocal, FileMode.Create))
                    {
                        // Buffer de envío de datos del fichero
                        byte[] byteBuffer = new byte[TAM_BUFFER];
                        int bytesSent = objFS_local.Read(byteBuffer, 0, TAM_BUFFER);
                        // Se envía el fichero poco a poco 
                        try
                        {
                            while (bytesSent != 0)
                            {
                                ftpStream.Write(byteBuffer, 0, bytesSent);
                                bytesSent = objFS_local.Read(byteBuffer, 0, TAM_BUFFER);
                            }

                            bCorrecto = true;
                        }
                        catch (Exception ex)
                        {
                            strMsgError.AppendFormat("Error al enviar el fichero {0} al servidor {1}, fichero remoto {2}.Error:{3}", strFicheroLocal, m_strURLServer,StrFicheroRemoto, ex.Message.ToString());
                            Log(false, "ClienteFtp.SendFile", strMsgError.ToString());
                        }
                        finally
                        {
                            // Se cierran los stream y see liberan los recursos
                            objFS_local.Close();
                            ftpStream.Close();
                            ftpStream.Dispose();
                            objFS_local.Dispose();
                            objFtpRequest = null;
                        }
                    }
                }
                catch (WebException exWeb)
                {
                    strMsgError.AppendFormat("Error establecer la conexión con el servidor {0}.Error:{3}", m_strURLServer, exWeb.Message.ToString());
                    Log(false, "ClienteFtp.SendFile", strMsgError.ToString());

                }
            }
            catch (Exception ex)
            {
                Log(false, "ClienteFtp.SendFile", ex.ToString());
            }

            return bCorrecto;
        }

        void Log(bool isError, string from, string msg)
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

}
