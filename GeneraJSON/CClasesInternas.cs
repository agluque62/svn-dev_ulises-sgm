//------------------------------------------------------------------------------
// Copyright (C) 2018, DF NUCLEO
//------------------------------------------------------------------------------
// Product:             Ulises
// Application:         
//------------------------------------------------------------------------------
//! @file
//! @author         Marian Vera
//! @date creation  08/03/2018
//! @date last      08/03/2018
//! @brief          Implementa las clases internas para para generar el objeto JSON
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace GeneraJSON_Dll
{
    //----------------------------------------------------------------------------------------------
    //!@brief Contiene las clases que permiten definir las clases internas del objeto del fichero JSON  a enviar al proxy
    //--------------------------------------------------------------------------------------------
    
    //----------------------------------------------------------------------------------------------
    //!@brief Clase para cada recurso alternativo disponibles para cada prefijo
    //----------------------------------------------------------------------------------------------
    internal class CRecRedAlt
    {
        public string idrec; //Identificador del recurso de red
        public string GwId;  //Identificador de la pasarela asociada al recurso
        public string ip;    //Dirección IP de la pasarela asociada al recurso

        //-----------------------------------------------------------------------------
        //!@brief Constructor de la clase CRecRedAlt
        //-----------------------------------------------------------------------------
        public CRecRedAlt()
        {
            idrec = string.Empty;
            GwId = string.Empty;
            ip = string.Empty;
        }
    }

    //----------------------------------------------------------------------------------------------
    //!@brief Clase de definición de cada red asociada al sector
    //----------------------------------------------------------------------------------------------
    internal class CRecRedSector
    {
        public string idred; //Prefijo de la red
        public string idrec; //Identificador del recurso de red
        public string GwId;  //Identificador de la pasarela asociada al recurso
        public string ip;    //Dirección IP de la pasarela asociada al recurso

        //------------------------------------------------------------------------------
        //!@brief Constructor de la clase CRecRedSector
        //-----------------------------------------------------------------------------
        public CRecRedSector()
        {
            idred = string.Empty;
            idrec = string.Empty;
            GwId = string.Empty;
            ip = string.Empty;
        }
    }

    //---------------------------------------------------------------------------------------------
    //!@brief Clase del objeto redes_alternativas
    //---------------------------------------------------------------------------------------------
    internal class CRedesAlt
    {
        //Prefijo de la red 
        public string prefijo;

        //Lista de todos los recursos asociados a cada red
        public List<CRecRedAlt> recursos = null;

        //------------------------------------------------------------------------------
        //!@brief Constructor de la clase CRedesAlt
        //-----------------------------------------------------------------------------
        public CRedesAlt()
        {
            recursos = new List<CRecRedAlt>();
            prefijo = string.Empty;
        }

        //------------------------------------------------------------------------------
        //!@brief Destructor de la clase CRedesAlt
        //------------------------------------------------------------------------------    
        ~CRedesAlt()
        {
            if (recursos != null && recursos.Count > 0)
            {
                recursos.Clear();
            }
        }
    }

    //--------------------------------------------------------------------------------------------
    //!@brief Clase que implementa cada elemento del objeto usuarios_scv
    //--------------------------------------------------------------------------------------------
    internal class CSector
    {
        //Identificador del Sector
        public string idSector;

        //Lista de números ATS del Sector
        public ArrayList sector = null;

        //Una lista blanca de abonados o rango de abonados en cada red, a los cuales se podrá efectuar llamadas
        //Cada elemento del array es una ArrayList.
        public ArrayList lista_blanca_salientes = null;

        //Una lista blanca de abonados o rango de abonados en cada red  de los cuales podrá recibir llamadas
        //Cada elemento del array es una ArrayList.
        public ArrayList lista_blanca_entrantes = null;

        // Para cada red configurada en el sector, se indica el recurso predeterminado para encaminar sus llamadas salientes 
        //y dirigir hacia él las llamadas entrantes.
        public List<CRecRedSector> redes = null;

        //-----------------------------------------------------------------------------
        //!@brief Constructor de la clase CSector
        //-----------------------------------------------------------------------------
        public CSector()
        {
            idSector = string.Empty;
            sector = new ArrayList();
            lista_blanca_salientes = new ArrayList();
            lista_blanca_entrantes = new ArrayList();
            redes = new List<CRecRedSector>();
        }

        //------------------------------------------------------------------------------
        //!@brief Destructor de la clase CSector
        //------------------------------------------------------------------------------    
        ~CSector()
        {
            //Se libera la lista de números de abonados
            if (sector != null && sector.Count > 0)
            {
                sector.Clear();
            }

            //Se libera la lista blanca de abonados a los que puede llamar
            if (lista_blanca_salientes != null && lista_blanca_salientes.Count > 0)
            {
                //Liberamos cada elemento de la lista
                for (int i = 0; i < lista_blanca_salientes.Count; i++)
                {
                    if (lista_blanca_salientes[i] != null)
                    {
                        ((ArrayList)lista_blanca_salientes[i]).Clear();
                    }
                }

                lista_blanca_salientes.Clear();
            }

            //Se libera la lista blanca de abonados de los que puede recibir llamadas
            if (lista_blanca_entrantes != null && lista_blanca_entrantes.Count > 0)
            {
                //Liberamos cada elemento de la lista blanca de llamadas entrantes
                for (int i = 0; i < lista_blanca_entrantes.Count; i++)
                {
                    if (lista_blanca_entrantes[i] != null)
                    {
                        ((ArrayList)lista_blanca_entrantes[i]).Clear();
                    }

                    lista_blanca_entrantes.Clear();
                }

                if (redes != null && redes.Count > 0)
                {
                    redes.Clear();
                }
            }
        }
    }

    //------------------------------------------------------------------------------------------
    //!@brief Clase que implementa el objeto encamina_abonados
    //------------------------------------------------------------------------------------------
    internal class CEncaminaAbonados
    {
        //Lista con la información de cada sector
        public List<CSector> usuarios_scv = null;

        //Lista de redes alternativas
        public List<CRedesAlt> redes_alternativas = null;

        //------------------------------------------------------------------------------
        //!@brief Constructor de la clase
        //-----------------------------------------------------------------------------
        public CEncaminaAbonados()
        {
            usuarios_scv = new List<CSector>();
            redes_alternativas = new List<CRedesAlt>();
        }

        //------------------------------------------------------------------------------
        //!@brief Destructor de la clase
        //------------------------------------------------------------------------------    
        ~CEncaminaAbonados()
        {
            if (usuarios_scv != null)
            {
                if (usuarios_scv.Count > 0)
                {
                    usuarios_scv.Clear();
                }
            }

            if (redes_alternativas != null)
            {
                if (redes_alternativas.Count > 0)
                {
                    redes_alternativas.Clear();
                }
            }
        }

        //------------------------------------------------------------------------------
        //!@brief Obtiene la lista de sectores simples
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string]    strSistema Identificador del sistema
        //!@param[string]    arrSectores Lista con los sectores simples obtenidos
        //------------------------------------------------------------------------------
        private bool ObtenerListaSectores(DbCommand cmdBD, string strSistema, ref Hashtable arrSectores)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strIdSector = string.Empty;
            string strIdNucleo = string.Empty;

            if (arrSectores != null)
            {
                if (arrSectores.Count > 0)
                    arrSectores.Clear();

                using (var cmd = cmdBD)
                {

                    cmd.CommandType = CommandType.Text;

                    //Construimos la consulta que nos permite obtener la lista de sectores simples
                    strSentencia.AppendFormat("SELECT IdSector,IdNucleo FROM sectores where SectorSimple=true and IdSistema='{0}' order by IdSistema,IdNucleo,IdSector", strSistema);
                    cmd.CommandText = strSentencia.ToString();

                    using (DbDataReader objReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            if (objReader.HasRows)
                            {
                                while (objReader.Read())
                                {
                                    //Se obtiene el identificador del Sector
                                    strIdSector = objReader.GetString(0);
                                    //Se obtiene el identificador del nucleo
                                    strIdNucleo = objReader.GetString(1);
                                    if (!String.IsNullOrEmpty(strIdSector) && !arrSectores.ContainsKey(strIdSector))
                                    {
                                        arrSectores.Add(strIdSector, strIdNucleo);
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
            }

            return (bCorrecto);
        }

        //------------------------------------------------------------------------------------
        //!@brief Obtiene la lista de recursos configurados en todas las redes del sistema con 
        //        prefijo >=4
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string]    strSistema Identificador del sistema
        //-------------------------------------------------------------------------------------
        public bool ObtenerRedesAlternativas(DbCommand cmdBD, string strSistema)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strIdRed = string.Empty;
            string strIdPrefijo = string.Empty;

            Hashtable objMapaRedes = new Hashtable();

            using (var cmd = cmdBD)
            {
                redes_alternativas.Clear();

                cmd.CommandType = CommandType.Text;

                //Construimos la consulta que nos permite obtener la lista de prefijos de redes con identificador mayor o igual que 4
                strSentencia.AppendFormat(" SELECT IdRed,IdPrefijo FROM redes where IdSistema='{0}' AND IdPrefijo>=4 ORDER BY IdPrefijo", strSistema);
                cmd.CommandText = strSentencia.ToString();

                try
                {
                    using (DbDataReader objReader = cmd.ExecuteReader())
                    {
                        if (objReader.HasRows && objReader.FieldCount >= 2)
                        {
                            while (objReader.Read())
                            {
                                strIdRed = objReader.GetString(0);
                                strIdPrefijo = objReader.GetString(1);
                                if (!objMapaRedes.ContainsKey(strIdPrefijo))
                                {
                                    objMapaRedes.Add(strIdPrefijo, strIdRed);
                                }
                            }
                        }

                        objReader.Close();
                    }
                }
                catch (Exception objEx)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error al obtener la lista de redes alternativas para el sistema={0}. Error: {1}", strSistema, objEx.Message.ToString());
                    CTraza.EscribeLog(true, "ObtenerRedesAlternativas", strMsg.ToString());
                    strMsg.Clear();
                }

                //Para cada red, obtenemos la lista de recursos asociados

                if (objMapaRedes.Count > 0)
                {
                    strIdRed = string.Empty;
                    strIdPrefijo = string.Empty;

                    foreach (System.Collections.DictionaryEntry upd in objMapaRedes)
                    {

                        strIdRed = (string)objMapaRedes[upd.Key];
                        strIdPrefijo = upd.Key.ToString();

                        //Se obtienen los recursos asociados a la red

                        CRedesAlt objRedAlt = new CRedesAlt();

                        //El prefijo se rellena con ceros por la izquierda
                        objRedAlt.prefijo = strIdPrefijo.PadLeft(2, '0');

                        strSentencia.Clear();
                        strSentencia.Append("SELECT R.IdRecurso, R.IdTifx, P.IpRed1 FROM recursosTf RTF, recursos R, tifx P WHERE  RTF.IdSistema=R.IdSistema AND RTF.IdRecurso=R.IdRecurso AND ");
                        strSentencia.AppendFormat("RTF.TipoRecurso=R.TipoRecurso AND R.IdSistema=P.IdSistema AND R.IdTifx=P.IdTifx AND RTF.IdSistema='{0}'  AND RTF.IdRed='{1}' ", strSistema, strIdRed);

                        cmd.CommandText = strSentencia.ToString();

                        try
                        {
                            using (DbDataReader objReader = cmd.ExecuteReader())
                            {
                                if (objReader.HasRows && objReader.FieldCount >= 3)
                                {
                                    //Añadimos el primer recurso
                                    CRecRedAlt objRec = null;

                                    while (objReader.Read())
                                    {
                                        //Añadimos el resto de recursos
                                        objRec = new CRecRedAlt();

                                        objRec.idrec = objReader.GetString(0);
                                        objRec.GwId = objReader.GetString(1);
                                        objRec.ip = objReader.GetString(2);

                                        objRedAlt.recursos.Add(objRec);
                                    }
                                }

                                objReader.Close();
                            }
                        }
                        catch (Exception objEx)
                        {
                            StringBuilder strMsg = new StringBuilder();
                            strMsg.AppendFormat("Error al obtener  los recursos de la red alternativa {0} para el sistema={1}. Error: {2}", strIdRed, strSistema, objEx.Message.ToString());
                            CTraza.EscribeLog(true, "ObtenerRedesAlternativas", strMsg.ToString());
                            strMsg.Clear();
                        }

                        redes_alternativas.Add(objRedAlt);

                    } //for each

                    objMapaRedes.Clear();
                }

                bCorrecto = true;
            }
            return (bCorrecto);
        }

        //-------------------------------------------------------------------------------------
        //!@brief Obtiene la lista de números de abonados de tipo ATS configurados en el sector
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string]    strSistema Identificador del sistema
        //!@param[string]    strNucleo Identificador del Nucleo
        //!@param[string]    strSector Identificador del Sector
        //!@param[CSector]   objSector objeto de la clase Sector donde se rellenará el objeto 
        //                   sector con la lista de abonados de la red ATS
        //--------------------------------------------------------------------------------------
        private bool ObtenerListaNumAbonadosAtsSector(DbCommand cmdBD, string strSistema, string strNucleo, string strSector, ref CSector objSector)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strIdAbonado = string.Empty;

            try
            {
                if (objSector != null && objSector.sector != null)
                {
                    using (var cmd = cmdBD)
                    {
                        if (objSector.sector.Count > 0)
                            objSector.sector.Clear();

                        cmd.CommandType = CommandType.Text;

                        //Construimos la consulta para obtener los usuarios Abonados al sector
                        strSentencia.AppendFormat("SELECT IdAbonado FROM  usuariosabonados WHERE IdSistema='{0}' AND IdNucleo='{1}' AND IdSector='{2}' AND IdPrefijo={3} order by IdAbonado", strSistema, strNucleo, strSector, Constantes.PREFIJO_RED_TLF_ATS);
                        cmd.CommandText = strSentencia.ToString();

                        using (DbDataReader objReader = cmd.ExecuteReader())
                        {
                            try
                            {
                                if (objReader.HasRows && objReader.FieldCount > 0)
                                {
                                    while (objReader.Read())
                                    {
                                        strIdAbonado = objReader.GetString(0);
                                        if (!String.IsNullOrEmpty(strIdAbonado))
                                        {
                                            objSector.sector.Add(strIdAbonado);
                                        }
                                    }
                                }
                                bCorrecto = true;
                            }
                            catch (Exception objEx)
                            {
                                StringBuilder strMsg = new StringBuilder();
                                strMsg.AppendFormat("Error al obtener la lista de abonados ATS para el sistema={0}, Nucleo={1}, Sector={2}. Error: {3}", strSistema, strNucleo, strSector, objEx.Message.ToString());
                                CTraza.EscribeLog(true, "ObtenerListaNumAbonadosAtsSector", strMsg.ToString());
                                strMsg.Clear();
                            }
                            finally
                            {
                                objReader.Close();
                                strSentencia.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                bCorrecto = false;
                StringBuilder strMsg = new StringBuilder();
                strMsg.AppendFormat("Error al obtener la lista de abonados ATS para el sistema={0}, Nucleo={1}, Sector={2}. Sentencia: {3} \n", strSistema, strNucleo, strSector, strSentencia);
                strMsg.AppendFormat("Error: {0}", objEx.Message.ToString());
                CTraza.EscribeLog(true, "ObtenerListaNumAbonadosAtsSector", strMsg.ToString());
                strMsg.Clear();
                strSentencia.Clear();
            }

            return (bCorrecto);
        }

        //------------------------------------------------------------------------------
        //!@brief Obtiene la lista blanca de abonados o rango de abonados a los que el sector puede efectuar llamadas
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string]    strSistema Identificador del sistema
        //!@param[string]    strNucleo Identificador del Nucleo
        //!@param[string]    strSector Identificador del Sector
        //!@param[CSector]   objSector objeto de la clase Sector donde se rellenará el objeto 
        //                   sector lista_blanca_salientes
        //--------------------------------------------------------------------------------------
        private bool ObtenerListaLlamadasSalientesSector(DbCommand cmdBD, string strSistema, string strNucleo, string strSector, ref CSector objSector)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strIdAbonado = string.Empty;
            string strPrefijo = string.Empty;
            string strTipo = string.Empty;
            string strNumAbonadoInicial = string.Empty;
            string strNumAbonadoFinal = string.Empty;
            //Posicion de cada campo en base 0 de la select de la sentencia construida
            const int POS_C_TIPO = 0;
            const int POS_C_IDPREFIJO = 2;
            const int POS_C_NUM_ABONADO_INICIAL = 3;
            const int POS_C_NUM_ABONADO_FINAL = 4;
            const int NUM_COLUMNAS_CONSULTA = 5;

            ArrayList objEltoLista = null;

            try
            {
                if (objSector != null && objSector.lista_blanca_salientes != null)
                {
                    using (var cmd = cmdBD)
                    {
                        if (objSector.lista_blanca_salientes.Count > 0)
                            objSector.lista_blanca_salientes.Clear();

                        cmd.CommandType = CommandType.Text;

                        //Construimos la consulta para obtener la lista de abonados  para las que el sector tiene permitido llamar (PermisosRedes.Llamar=true)
                        // Para que el sector pueda realizar llamadas para una red determinada debe tener configurado el check PermisosRedes.Llamar=true para la red. Este check, permite
                        // activar o desactivar la lista blanca de llamadas salientes configurado en la BD.

                        //Para la red, el sector 
                        strSentencia.Append("SELECT LL.Tipo,LL.IdRed,R.IdPrefijo,LL.NumAbonadoInicial,LL.NumAbonadoFinal FROM  llamadas_salientes_sector LL, redes R ");
                        strSentencia.Append("WHERE LL.IdSistema=R.IdSistema AND LL.IdRed=R.IdRed AND ");
                        strSentencia.Append("EXISTS (SELECT 1 FROM permisosRedes PR WHERE PR.IdSistema=LL.IdSistema AND PR.IdNucleo=LL.IdNucleo AND PR.IdSector=LL.IdSector AND PR.IdRed=LL.IdRed AND PR.Llamar=true) ");
                        strSentencia.AppendFormat("AND LL.IdSistema='{0}' AND LL.IdNucleo='{1}' AND LL.IdSector='{2}' AND R.IdPrefijo>{3} ORDER BY LL.IdRed,R.idPrefijo", strSistema, strNucleo, strSector, Constantes.PREFIJO_RED_TLF_ATS);
                        cmd.CommandText = strSentencia.ToString();

                        using (DbDataReader objReader = cmd.ExecuteReader())
                        {
                            try
                            {
                                if (objReader.HasRows && objReader.FieldCount >= NUM_COLUMNAS_CONSULTA)
                                {
                                    while (objReader.Read())
                                    {
                                        //Obtenemos los datos de cada registro
                                        // "lista_blanca_salientes": [ ["04111111111","04111111115"], ["04222222222"],["05333333333","05333333336"] ]

                                        strTipo = objReader.GetString(POS_C_TIPO);

                                        if (!String.IsNullOrEmpty(strTipo))
                                        {
                                            switch (strTipo.ToUpper())
                                            {
                                                case "A": //Abonado
                                                case "T": //Todos los abonados de la red

                                                    objEltoLista = new ArrayList();

                                                    strPrefijo = objReader.GetString(POS_C_IDPREFIJO);

                                                    if (string.Compare(strTipo, "A", true) == 0)
                                                    {
                                                        strIdAbonado = objReader.GetString(POS_C_NUM_ABONADO_INICIAL);
                                                    }
                                                    else
                                                    {
                                                        //Se pone el Asterisco aunque en Bd debería venir también el *
                                                        strIdAbonado = "*";
                                                    }

                                                    //El prefijo se rellena con ceros por la izquierda
                                                    strNumAbonadoInicial = string.Format("{0}{1}", strPrefijo.PadLeft(2, '0'), strIdAbonado);
                                                    objEltoLista.Add(strNumAbonadoInicial);

                                                    objSector.lista_blanca_salientes.Add(objEltoLista);

                                                    strNumAbonadoInicial = string.Empty;

                                                    break;
                                                case "R": //Rango de Abonados

                                                    objEltoLista = new ArrayList();

                                                    strPrefijo = objReader.GetString(POS_C_IDPREFIJO);
                                                    strIdAbonado = objReader.GetString(POS_C_NUM_ABONADO_INICIAL);

                                                    //El prefijo se rellena con ceros por la izquierda
                                                    strNumAbonadoInicial = string.Format("{0}{1}", strPrefijo.PadLeft(2, '0'), strIdAbonado);

                                                    strIdAbonado = objReader.GetString(POS_C_NUM_ABONADO_FINAL);

                                                    if (!string.IsNullOrEmpty(strIdAbonado))
                                                    {
                                                        strNumAbonadoFinal = string.Format("{0}{1}", strPrefijo.PadLeft(2, '0'), strIdAbonado); ;
                                                    }

                                                    //Se añade el abondado inicial y final del rango
                                                    objEltoLista.Add(strNumAbonadoInicial);
                                                    objEltoLista.Add(strNumAbonadoFinal);

                                                    //Se añade el rango a la lista de llamadas
                                                    objSector.lista_blanca_salientes.Add(objEltoLista);

                                                    strNumAbonadoInicial = string.Empty;
                                                    strNumAbonadoFinal = string.Empty;
                                                    break;

                                                default:  //Valor no permitido
                                                    break;
                                            }

                                            strPrefijo = string.Empty;
                                            strTipo = string.Empty;
                                        }
                                    }
                                }
                                bCorrecto = true;
                            }
                            catch (Exception objEx)
                            {
                                StringBuilder strMsg = new StringBuilder();
                                strMsg.AppendFormat("Error al obtener la lista de llamadas salientes para el sistema={0}, Nucleo={1}, Sector={2}. Error: {3}", strSistema, strNucleo, strSector, objEx.Message.ToString());
                                CTraza.EscribeLog(true, "ObtenerListaLlamadasSalientesSector", strMsg.ToString());
                                strMsg.Clear();
                            }
                            finally
                            {
                                objReader.Close();
                                strSentencia.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                bCorrecto = false;
                StringBuilder strMsg = new StringBuilder();
                strMsg.AppendFormat("Error al obtener la lista de llamadas salientes para el sistema={0}, Nucleo={1}, Sector={2}. Sentencia: {3} \n", strSistema, strNucleo, strSector, strSentencia);
                strMsg.AppendFormat("Error: {0}", objEx.Message.ToString());
                CTraza.EscribeLog(true, "ObtenerListaLlamadasSalientesSector", strMsg.ToString());
                strMsg.Clear();
                strSentencia.Clear();
            }

            return (bCorrecto);
        }

        //------------------------------------------------------------------------------
        //!@brief Obtiene la lista blanca de abonados o rango de abonados de los que el sector puede recibir llamadas
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string]    strSistema Identificador del sistema
        //!@param[string]    strNucleo Identificador del Nucleo
        //!@param[string]    strSector Identificador del Sector
        //!@param[CSector]   objSector objeto de la clase Sector donde se rellenará el objeto 
        //                   sector lista_blanca_entrantes
        //--------------------------------------------------------------------------------------
        private bool ObtenerListaLlamadasEntrantesSector(DbCommand cmdBD, string strSistema, string strNucleo, string strSector, ref CSector objSector)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strIdAbonado = string.Empty;
            string strPrefijo = string.Empty;
            string strTipo = string.Empty;
            string strNumAbonadoInicial = string.Empty;
            string strNumAbonadoFinal = string.Empty;
            //Posicion de cada campo en base 0 de la select de la sentencia construida
            const int POS_C_TIPO = 0;
            const int POS_C_IDPREFIJO = 2;
            const int POS_C_NUM_ABONADO_INICIAL = 3;
            const int POS_C_NUM_ABONADO_FINAL = 4;
            const int NUM_COLUMNAS_CONSULTA = 5;

            ArrayList objEltoLista = null;

            try
            {

                if (objSector != null && objSector.lista_blanca_entrantes != null)
                {
                    using (var cmd = cmdBD)
                    {
                        if (objSector.lista_blanca_entrantes.Count > 0)
                            objSector.lista_blanca_entrantes.Clear();

                        cmd.CommandType = CommandType.Text;

                        //Construimos la consulta para obtener la lista de abonados de los que el sector tiene permitido recibir llamadas (PermisosRedes.Recibir=true)
                        // Para que el sector pueda recibir llamadas para una red determinada debe tener configurado el check PermisosRedes.Recibir=true. Este check, permite
                        // activar o desactivar la lista de abonados configurados.

                        //Para la red, el sector 
                        strSentencia.Append("SELECT LL.Tipo,LL.IdRed,R.IdPrefijo,LL.NumAbonadoInicial,LL.NumAbonadoFinal FROM  llamadas_entrantes_sector LL, redes R ");
                        strSentencia.Append("WHERE LL.IdSistema=R.IdSistema AND LL.IdRed=R.IdRed AND ");
                        strSentencia.Append("EXISTS (SELECT 1 FROM permisosRedes PR WHERE PR.IdSistema=LL.IdSistema AND PR.IdNucleo=LL.IdNucleo AND PR.IdSector=LL.IdSector AND PR.IdRed=LL.IdRed AND PR.Recibir=true) ");
                        strSentencia.AppendFormat("AND LL.IdSistema='{0}' AND LL.IdNucleo='{1}' AND LL.IdSector='{2}' AND R.IdPrefijo>{3} ORDER BY LL.IdRed,R.idPrefijo", strSistema, strNucleo, strSector, Constantes.PREFIJO_RED_TLF_ATS);

                        cmd.CommandText = strSentencia.ToString();

                        using (DbDataReader objReader = cmd.ExecuteReader())
                        {
                            try
                            {
                                if (objReader.HasRows && objReader.FieldCount >= NUM_COLUMNAS_CONSULTA)
                                {
                                    while (objReader.Read())
                                    {
                                        //Obtenemos los datos de cada registro
                                        // "lista_blanca_entrantes": [ ["04111111111","04111111115"], ["04222222222"],["05333333333","05333333336"] ]

                                        strTipo = objReader.GetString(POS_C_TIPO);

                                        if (!String.IsNullOrEmpty(strTipo))
                                        {
                                            switch (strTipo.ToUpper())
                                            {
                                                case "A": //Abonado
                                                case "T": //Todos los abonados de la red

                                                    objEltoLista = new ArrayList();

                                                    strPrefijo = objReader.GetString(POS_C_IDPREFIJO);

                                                    if (string.Compare(strTipo, "A", true) == 0)
                                                    {
                                                        strIdAbonado = objReader.GetString(POS_C_NUM_ABONADO_INICIAL);
                                                    }
                                                    else
                                                    {
                                                        //Se pone el Asterisco aunque en Bd debería venir también el *
                                                        strIdAbonado = "*";
                                                    }

                                                    //El prefijo se rellena con ceros por la izquierda
                                                    strNumAbonadoInicial = string.Format("{0}{1}", strPrefijo.PadLeft(2, '0'), strIdAbonado);
                                                    objEltoLista.Add(strNumAbonadoInicial);

                                                    objSector.lista_blanca_entrantes.Add(objEltoLista);

                                                    strNumAbonadoInicial = string.Empty;

                                                    break;
                                                case "R": //Rango de Abonados

                                                    objEltoLista = new ArrayList();

                                                    strPrefijo = objReader.GetString(POS_C_IDPREFIJO);
                                                    strIdAbonado = objReader.GetString(POS_C_NUM_ABONADO_INICIAL);

                                                    //El prefijo se rellena con ceros por la izquierda
                                                    strNumAbonadoInicial = string.Format("{0}{1}", strPrefijo.PadLeft(2, '0'), strIdAbonado);

                                                    strIdAbonado = objReader.GetString(POS_C_NUM_ABONADO_FINAL);

                                                    if (!string.IsNullOrEmpty(strIdAbonado))
                                                    {
                                                        strNumAbonadoFinal = string.Format("{0}{1}", strPrefijo.PadLeft(2, '0'), strIdAbonado); ;
                                                    }

                                                    //Se añade el abondado inicial y final del rango
                                                    objEltoLista.Add(strNumAbonadoInicial);
                                                    objEltoLista.Add(strNumAbonadoFinal);

                                                    //Se añade el rango a la lista de llamadas
                                                    objSector.lista_blanca_entrantes.Add(objEltoLista);

                                                    strNumAbonadoInicial = string.Empty;
                                                    strNumAbonadoFinal = string.Empty;
                                                    break;

                                                default:  //Valor no permitido
                                                    break;
                                            }

                                            strPrefijo = string.Empty;
                                            strTipo = string.Empty;
                                        }
                                    }
                                }

                                bCorrecto = true;
                            }
                            catch (Exception objEx)
                            {
                                StringBuilder strMsg = new StringBuilder();
                                strMsg.AppendFormat("Error al obtener la lista de llamadas entrantes para el sistema={0}, Nucleo={1}, Sector={2}. Error: {3}", strSistema, strNucleo, strSector, objEx.Message.ToString());
                                CTraza.EscribeLog(true, "ObtenerListaLlamadasEntrantesSector", strMsg.ToString());
                                strMsg.Clear();
                            }
                            finally
                            {
                                objReader.Close();
                                strSentencia.Clear();
                            }
                        }
                    }

                }
            }
            catch (Exception objEx)
            {
                bCorrecto = false;
                StringBuilder strMsg = new StringBuilder();
                strMsg.AppendFormat("Error al obtener la lista de llamadas entrantes para el sistema={0}, Nucleo={1}, Sector={2}. Sentencia: {3} \n", strSistema, strNucleo, strSector, strSentencia);
                strMsg.AppendFormat("Error: {0}", objEx.Message.ToString());
                CTraza.EscribeLog(true, "ObtenerListaLlamadasEntrantesSector", strMsg.ToString());
                strMsg.Clear();
                strSentencia.Clear();
            }

            return (bCorrecto);
        }

        //------------------------------------------------------------------------------
        //!@brief Obtiene las interfaces predeterminadas FXO en cada Red PSTN para encaminar las llamadas
        //        salientes del sector
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string]    strSistema Identificador del sistema
        //!@param[string]    strNucleo Identificador del Nucleo
        //!@param[string]    strSector Identificador del Sector
        //!@param[CSector]   objSector objeto de la clase Sector donde se rellenará el objeto 
        //                   sector.redes
        //--------------------------------------------------------------------------------------

        private bool ObtenerRedesSector(DbCommand cmdBD, string strSistema, string strIdNucleo, string strIdSector, ref CSector objSector)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strIdPrefijoRed = string.Empty;
            string strIdRecursoRed = string.Empty;
            string strIdGwId = string.Empty;
            string strIp = string.Empty;
            //Posicion de cada campo en base 0 de la select de la sentencia construida
            const int POS_C_IDPREFIJO = 0;      //Prefijo de la Red
            const int POS_C_IDRECURSOPREF = 1;  //Identificador del recurso de telefonía
            const int POS_C_IDTIFX = 2;         //Identificador de la pasarela
            const int POS_C_IP = 3;             //Dirección Ip de la pasarela
            const int NUM_COLUMNAS_CONSULTA = 4;

            CRecRedSector objRecurso = null;

            try
            {

                if (objSector != null && objSector.redes != null)
                {
                    using (var cmd = cmdBD)
                    {
                        if (objSector.redes.Count >0)
                            objSector.redes.Clear();

                        cmd.CommandType = CommandType.Text;

                        // Sólo se obtienen las redes con prefijo>3 (ATS)  que tienen asignado una interfaz predeterminada FXO
                        //Construimos la consulta para obtener las interfaces para cada red del sector

                        strSentencia.Append("SELECT  R.IdPrefijo, PR.IdRecursoPref,REC.IdTifx, P.IpRed1 FROM permisosredes PR ");
                        strSentencia.Append("JOIN redes R ON  PR.IdSistema = R.IdSistema AND PR.IdRed = R.IdRed ");
                        strSentencia.Append("JOIN recursosTf RTF  ON RTF.IdSistema=PR.IdSistema AND RTF.IdRecurso=PR.IdRecursoPref AND RTF.TipoRecurso=PR.TipoRecursoPref ");
                        strSentencia.Append("JOIN recursos REC ON REC.IdSistema=RTF.IdSistema AND REC.IdRecurso=RTF.IdRecurso AND REC.TipoRecurso=RTF.TipoRecurso ");
                        strSentencia.Append("JOIN tifx P ON REC.IdSistema=REC.IdSistema AND REC.IdTifx=P.IdTifx ");
                        strSentencia.AppendFormat("AND PR.IdSistema='{0}' AND  PR.IdNucleo='{1}' AND PR.IdSector='{2}' AND R.IdPrefijo>{3} ORDER BY R.IdPrefijo", strSistema, strIdNucleo, strIdSector, Constantes.PREFIJO_RED_TLF_ATS);
                        cmd.CommandText = strSentencia.ToString();

                        //Obtenemos los datos de cada registro, para componer el objeto con la estructura del ejemplo
                        // "redes": [{"idred":"04", "idrec":"rec1", "GwId":"TIFX1", "ip":"192.168.1.30"},
                        //           {"idred":"05", "idrec":"rec2", "GwId":"TIFX2", "ip":"192.168.1.31"},
                        //           {"idred":"06", "idrec":"rec3", "GwId":"TIFX3", "ip":"192.168.1.33"}]

                        using (DbDataReader objReader = cmd.ExecuteReader())
                        {
                            try
                            {
                                if (objReader.HasRows && objReader.FieldCount >= NUM_COLUMNAS_CONSULTA)
                                {
                                    while (objReader.Read())
                                    {

                                        strIdPrefijoRed = objReader.GetString(POS_C_IDPREFIJO);
                                        //Obtenemos el identificador predeterminado de cada red
                                        strIdRecursoRed = objReader.GetString(POS_C_IDRECURSOPREF);

                                        if (!String.IsNullOrEmpty(strIdPrefijoRed) && !String.IsNullOrEmpty(strIdRecursoRed))
                                        {
                                            objRecurso = new CRecRedSector();

                                            objRecurso.idred = strIdPrefijoRed.PadLeft(2, '0');
                                            objRecurso.idrec = strIdRecursoRed;

                                            strIdGwId = objReader.GetString(POS_C_IDTIFX);
                                            objRecurso.GwId = strIdGwId;

                                            strIp = objReader.GetString(POS_C_IP);
                                            objRecurso.ip = strIp;

                                            objSector.redes.Add(objRecurso);
                                        }
                                    }
                                }

                                bCorrecto = true;
                            }
                            catch (Exception objEx)
                            {
                                StringBuilder strMsg = new StringBuilder();
                                strMsg.AppendFormat("Error al obtener la lista de interfaces FXO para cada Red PSTN para el sistema={0}, Nucleo={1}, Sector={2}. Error: {3}", strSistema, strIdNucleo, strIdSector, objEx.Message.ToString());
                                CTraza.EscribeLog(true, "ObtenerRedesSector", strMsg.ToString());
                                strMsg.Clear();
                            }
                            finally
                            {
                                objReader.Close();
                                strSentencia.Clear();
                            }
                        }
                    }

                }
            }
            catch (Exception objEx)
            {
                bCorrecto = false;
                StringBuilder strMsg = new StringBuilder();
                strMsg.AppendFormat("Error al obtener la lista de llamadas entrantes para el sistema={0}, Nucleo={1}, Sector={2}. Sentencia: {3} \n", strSistema, strIdSector, strIdSector, strSentencia);
                strMsg.AppendFormat("Error: {0}", objEx.Message.ToString());
                CTraza.EscribeLog(true, "ObtenerRedesSector", strMsg.ToString());
                strMsg.Clear();
                strSentencia.Clear();
            }

            return (bCorrecto);
        }

        //-------------------------------------------------------------------------------------------------
        //!@brief            Obtiene los datos de los sectores simples y rellena el objeto miembro usuarios_scv
        //!@param[DbCommand] cmdBD Comando para ejecutar sentencias en la BD de MySql
        //!@param[string]    strSistema Identificador del sistema
        //----------------------------------------------------------------------------------------------
        public bool ObtenerUsuariosSCV(DbCommand cmdBD, string strSistema)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            string strIdSector = string.Empty;
            string strIdNucleo = string.Empty;

            Hashtable objArrSectores = new Hashtable(); //Almacena la lista de sectores simples configurados en la BD para el sistema y el Nucleo al que pertenece
            //La clave del mapa es el identificador del sector

            CSector objSector = null;

            using (var cmd = cmdBD)
            {
                if (usuarios_scv.Count > 0)
                {
                    usuarios_scv.Clear();
                }

                bCorrecto = ObtenerListaSectores(cmdBD, strSistema, ref objArrSectores);

                if (bCorrecto && objArrSectores.Count > 0)
                {
                    foreach (DictionaryEntry upd in objArrSectores)
                    {
                        //Para cada sector

                        strIdSector = upd.Key.ToString();
                        strIdNucleo = (string)objArrSectores[upd.Key];

                        objSector = new CSector();

                        //Se informa el Identificador del sector
                        objSector.idSector = strIdSector;

                        //Se obtiene la lista de abonados ATS
                        ObtenerListaNumAbonadosAtsSector(cmdBD, strSistema, strIdNucleo, strIdSector, ref objSector);
                        //Se recupera la lista blanca de llamadas salientes
                        ObtenerListaLlamadasSalientesSector(cmdBD, strSistema, strIdNucleo, strIdSector, ref objSector);

                        //Se recupera la lista blanca de llamadas entrantes
                        ObtenerListaLlamadasEntrantesSector(cmdBD, strSistema, strIdNucleo, strIdSector, ref objSector);

                        //Obtener las redes configuradas en el sector
                        ObtenerRedesSector(cmdBD, strSistema, strIdNucleo, strIdSector, ref objSector);

                        //Añadimos el sector a la lista de usuarios SCV
                        usuarios_scv.Add(objSector);

                    } //for each

                    objArrSectores.Clear();
                }

                //Se obtienen todos los recursos de cada una de las redes
                ObtenerRedesAlternativas(cmdBD, strSistema);

                bCorrecto = true;
            }
            return (bCorrecto);
        }
    }
}
