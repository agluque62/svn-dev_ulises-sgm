-- MySQL dump 10.13  Distrib 5.6.26, for Win64 (x86_64)
--
-- Host: localhost    Database: new_cd40
-- ------------------------------------------------------
-- Server version	5.6.26-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `new_cd40`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `new_cd40` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `new_cd40`;

--
-- Table structure for table `incidencias`
--

DROP TABLE IF EXISTS `incidencias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `incidencias` (
  `IdIncidencia` int(10) unsigned NOT NULL,
  `IdIncidenciaCorrectora` int(10) unsigned DEFAULT NULL,
  `Incidencia` varchar(180) DEFAULT NULL,
  `Descripcion` varchar(180) DEFAULT NULL,
  `GeneraError` tinyint(1) DEFAULT NULL,
  `OID` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdIncidencia`),
  KEY `Incidencias_FKIndex1` (`IdIncidenciaCorrectora`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incidencias`
--

LOCK TABLES `incidencias` WRITE;
/*!40000 ALTER TABLE `incidencias` DISABLE KEYS */;
INSERT INTO `incidencias` (`IdIncidencia`, `IdIncidenciaCorrectora`, `Incidencia`, `Descripcion`, `GeneraError`, `OID`) VALUES 
(1,NULL,'Equipo HF conectado','Equipo HF {0} conectado',0,NULL),
(2,NULL,'Error en equipo HF','Error en equipo HF {0}',0,NULL),
(3,NULL,'Equipo HF desconectado','Equipo HF {0} desconectado',0,NULL),
(4,NULL,'Equipo HF asignado','Equipo HF asignado. Usuario: {0}. Equipo: {1}, Frecuencia: {2}.',0,NULL),
(5,NULL,'Equipo HF liberado','Equipo HF liberado. Equipo: {0}, Frecuencia: {1}.',0,NULL),
(6,NULL,'Error HF general','Error HF general',0,NULL),
(7,NULL,'Error HF asignación','Error HF asignación. Usuario: {0}, Equipo: {1}, Frecuencia: {2}.',0,NULL),
(8,NULL,'Error HF desasignación','Error HF desasignación: Equipo: {0}, Frecuencia: {1}.',0,NULL),
(9,NULL,'Error HF asignación múltiple','Error HF asignación múltiple. Usuario {0}.',0,NULL),
(10,NULL,'Error HF preparación SELCAL','Error HF preparación SELCAL. Usuario: {0}, Equipo: {1}, OnOff: {2}.',0,NULL),
(50,NULL,'INFO Servicio ','Informacion de Servicio {0}: {1} {2} {3} {4}',0,NULL),
(51,NULL,'ERROR Servicio','Error en Servicio {0}: {1}',1,NULL),
(96,NULL,'Cambio de día.','Cambio de día',0,NULL),
(101,NULL,'Selección SCV.','SCV {0} seleccionado.',0,NULL),
(105,NULL,'Carga de sectorización.','Carga de sectorización {0}',0,'.1.1.600.1'),
(106,NULL,'Error carga sectorización.','Error carga sectorización {0}',0,'.1.1.600.1'),
(108,NULL,'Rechazo sectorización. No están todos los sectores reales.','Rechazo sectorización {0}. No están todos los sectores reales',0,'.1.1.600.1'),
(109,NULL,'Sectorización automática implantada.','Sectorización automática implantada',0,'.1.1.600.1'),
(110,NULL,'Sectorización automática rechazada.','Sectorización automática rechazada',0,'.1.1.600.1'),
(111,NULL,'Sector asignado a posición.','Sector {0} asignado a posición {1}',0,'.1.1.600.1'),
(112,NULL,'Sector desasignado de la posición.','Sector {0} desasignado de la posición {1}',0,'.1.1.600.1'),
(113,NULL,'Rechazo sectorización. 1 + 1 no activo.','Rechazo sectorización. 1 + 1 no activo.',0,'.1.1.600.1'),
(114,NULL,'Configuración','{0}',0,NULL),
(201,NULL,'Servidor activo.','{0}',0,NULL),
(202,201,'Servidor reserva.','{0}',0,NULL),
(203,NULL,'Servidor caído.','{0}',0,NULL),
(300,NULL,'Gestion NBX','{0} {1} {2} {3}',0,NULL),
(301,NULL,'Alarma NBX','{0} {1} {2} {3}',1,NULL),
(1001,NULL,'Entrada TOP.','Entrada TOP',1,'1.1.1000.0'),
(1002,1001,'Caída TOP.','Caída TOP',1,'1.1.1000.0'),
(1003,NULL,'Conexión jacks ejecutivo.','Conexión jacks ejecutivo. Posición: {0}',0,'1.1.1000.1.3.0'),
(1004,1003,'Desconexión jacks ejecutivo.','Desconexión jacks ejecutivo. Posición: {0}',0,'1.1.1000.1.3.0'),
(1005,NULL,'Conexión jacks ayudante.','Conexión jacks ayudante. Posición: {0}',0,'1.1.1000.1.3.1'),
(1006,1005,'Desconexión jacks ayudante.','Desconexión jacks ayudante. Posición: {0} ',0,'1.1.1000.1.3.1'),
(1007,NULL,'Conexión altavoz.','Conexión altavoz {0}. Posición: {1} ',1,'1.1.1000.1.2'),
(1008,1007,'Desconexión altavoz.','Desconexión altavoz {0}. Posición: {1} ',1,'1.1.1000.1.2'),
(1009,NULL,'Panel pasa a operación.','Panel pasa a operación. Posición: {0} ',1,'1.1.1000.1.4'),
(1010,1009,'Panel pasa a standby.','Panel pasa a standby. Posición: {0} ',1,'1.1.1000.1.4'),
(1011,NULL,'Página de frecuencias seleccionada.','Página de frecuencias seleccionada: {0}. Posición: {1}.',0,'1.1.1000.6'),
(1014,NULL,'Estado PTT.','Estado PTT: {0}. Posicion:{1}. Sector{2}',0,'1.1.1000.2'),
(1015,NULL,'Facilidad seleccionada.','Facilidad seleccionada: {0}. Posición: {1}.',0,'1.1.1000.8'),
(1016,NULL,'Llamada entrante Posicion.','Llamada entrante. Acceso: {0}. Posicion: {1}.',0,'1.1.1000.9'),
(1017,NULL,'Llamada saliente Posicion.','Llamada saliente. Acceso: {0}. Posicion: {1}.',0,'1.1.1000.7'),
(1019,NULL,'Fin llamada Posicion.','Fin llamada. Acceso: {0}.Posicion {1}.',0,'1.1.1000.10'),
(1020,NULL,'Llamada telefonía establecida','Llamada establecida. Acceso: {0}. Posicion: {1}.',0,'1.1.1000.11'),
(1021,NULL,'Función Briefing',' Sesión Briefing en Puesto: {0} {1}',NULL,'1.1.1000.13'),
(1022,NULL,'Reproducción GLP','Grabación Local en Puesto {0} {1}',NULL,'1.1.1000.12'),
(1023,NULL,'Cable de Grabación Conectado','Cable de Grabación Conectado',0,NULL),
(1024,NULL,'Cable de Grabación Desconectado','Cable de Grabación Desconectado',1,NULL),
(1025,NULL,'Evento LAN','Estado LAN. Lan1: {0}, Lan2: {1}',1,NULL),
(2001, NULL, 'Modulo GW en Servicio', '{0} En servicio', 1, '1.1.100.2.0'),
(2002, 2001, 'Modulo GW Fuera de Servicio', '{0} Fuera de Servicio', 1, '1.1.100.2.0'),
(2003, NULL, 'Recurso Radio en Servicio', '{0}. Recurso Radio en Servicio', 1, '1.1.200.3.1.17'),
(2004, 2003, 'Recurso Radio Fuera de Servicio', '{0}. Recurso Radio Fuera de Servicio', 1, '1.1.200.3.1.17'),
(2005, NULL, 'Recurso Telefonía en Servicio', '{0}. Recurso Telefonía en Servicio', 1, '1.1.400.3.1.14'),
(2006, 2005, 'Recurso Telefonía Fuera de Servicio', '{0}. Recurso Telefonía Fuera de Servicio', 1, '1.1.400.3.1.14'),
(2007, NULL, 'Tarjeta interfaz en Servicio', 'Tarjeta interfaz: Slot {0}. en Servicio', 1, '1.1.100.31.1.2'),
(2008, 2007, 'Tarjeta interfaz Fuera de Servicio', 'Tarjeta interfaz: Slot {0}. Fuera de Servicio', 1, '1.1.100.31.1.2'),
(2009, NULL, 'Recurso R2 en Servicio', '{0}. Recurso R2 en Servicio', 1, '1.1.500.3.1.17'),
(2010, NULL, 'Recurso R2 Fuera de Servicio', '{0}. Recurso R2 Fuera de Servicio', 1, '1.1.500.3.1.17'),
(2012, NULL, 'Error protocolo LCN.', '{0}. Error protocolo LCN.', 0, '1.1.300.3.1.17'),
(2013, NULL, 'Recurso LCN en Servicio', '{0}. Recurso LCN en Servicio', 1, '1.1.300.3.1.17'),
(2014, 2013, 'Recurso LCN Fuera de Servicio', '{0}. Recurso LCN Fuera de Servicio', 1, '1.1.300.3.1.17'),
(2015, NULL, 'Recurso N5 en Servicio', '{0}. Recurso N5 en Servicio', 1, NULL),
(2016, NULL, 'Recurso N5 Fuera de Servicio', '{0}. Recurso N5 Fuera de Servicio', 1, NULL),
(2017, NULL, 'Recurso QSIG en Servicio', '{0}. Recurso QSIG en Servicio', 0, NULL),
(2018, NULL, 'Recurso QSIG Fuera de Servicio', '{0}. Recurso QSIG fuera de Servicio', 0, NULL),
(2020,NULL,'Llamada entrante R2.','Llamada entrante R2. Recurso {0}. Troncal {1}. Origen {2}. Prioridad {3}. Destino {4}',0,'1.1.500.3.1.17'),
(2021,NULL,'Fin llamada entrante R2.','Fin llamada entrante R2. Recurso {0}. Troncal {1}.',0,'1.1.500.3.1.17'),
(2022,NULL,'Llamada saliente R2.','Llamada saliente R2. Recurso {0}. Troncal {1}. Origen {2}. Prioridad {3}. Destino {4}',0,'1.1.500.3.1.17'),
(2023,NULL,'Fin llamada saliente R2.','Fin llamada saliente R2. Recurso {0}. Troncal {1}.',0,'1.1.500.3.1.17'),
(2024,NULL,'Llamada prueba R2.','Llamada prueba R2. Pasarela: {0}. Recurso: {1}',0,'1.1.500.3.1.17'),
(2025,NULL,'Error protocolo R2.','Error protocolo R2. Pasarela: {0}. Recurso: {1}',0,'1.1.500.3.1.17'),
(2030,NULL,'Llamada entrante LCN.','Llamada entrante LCN: Recurso: {0}',0,'1.1.300.3.1.17'),
(2031,NULL,'Fin llamada entrante LCN.','Fin llamada entrante LCN: Recurso: {0}.',0,'1.1.300.3.1.17'),
(2032,NULL,'Llamada saliente LCN.','Llamada saliente LCN: Recurso: {0}.',0,'1.1.300.3.1.17'),
(2033,NULL,'Fin llamada saliente LCN.','Fin llamada saliente LCN: Recurso: {0}.',0,'1.1.300.3.1.17'),
(2040,NULL,'Llamada entrante telefonía.','Llamada entrante telefonía: Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',0,'1.1.400.3.1.14'),
(2041,NULL,'Fin llamada entrante telefonía.','Fin llamada entrante telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',0,'1.1.400.3.1.14'),
(2042,NULL,'Llamada saliente telefonía.','Llamada saliente telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',0,'1.1.400.3.1.14'),
(2043,NULL,'Fin llamada saliente telefonía.','Fin llamada saliente telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',0,'1.1.400.3.1.14'),
(2050,NULL,'PTT On.','PTT On:  Recurso {0}. Frecuencia: {1}',0,'1.1.200.3.1.17'),
(2051,NULL,'PTT Off.','PTT Off:  Recurso {0}. Frecuencia: {1}',0,'1.1.200.3.1.17'),
(2052,NULL,'SQ On.','SQ On: Recurso {0}. Frecuencia: {1}',0,'1.1.200.3.1.17'),
(2053,NULL,'SQ Off.','SQ Off: Recurso {0}. Frecuencia: {1}',0,'1.1.200.3.1.17'),
(2100,NULL,'Selección Principal/Reserva','Pasarela {0} pasa a {1}',0,'1.1.100.21.0'),
(2200,NULL,'Evento Pasarela','{0}: {1} {2} {3}',1,NULL),
(2300,NULL,'Operacion Local en Pasarela','{0}: {1} {2} {3}',0,NULL),
(3001,NULL,'Entrada Equipo Externo.','Entrada Equipo Externo',1,NULL),
(3002,3001,'Caída Equipo Externo.','Caída Equipo Externo',1,NULL),
(3003,NULL,'Abonado PBX Activo', 'Abonado PBX Activo', 0,NULL),
(3004,NULL,'Abonado PBX Inactivo', 'Abonado PBX Inactivo', 0,NULL),
(3050,NULL,'MN. Equipo Disponible','Equipo Disponible. {1}',0,NULL),
(3051,NULL,'MN. Equipo en Fallo','Equipo en Fallo. {1}',1,NULL),
(3052,NULL,'MN. Error en Comunicacion con Equipo','Error en Comunicacion con equipo. {1}',1,NULL),
(3060,NULL,'MN. Tx de Frecuencia en Equipo PPAL','Tx de Frecuencia en Equipo PPAL. {1}',0,NULL),
(3061,NULL,'MN. Tx de Frecuencia en Equipo RSVA','Tx de Frecuencia en Equipo RSVA. {1}',0,NULL),
(3062,NULL,'MN. Tx de Frecuencia No Disponible','Tx de Frecuencia No Disponible. {1}',1,NULL),
(3063,NULL,'MN. Tx de Frecuencia No Disponible. Baja Prioridad','Tx de Frecuencia No Disponible por Prioridad. {1}',1,NULL),
(3064,NULL,'MN. Rx de Frecuencia en Equipo PPAL','Rx de Frecuencia en Equipo PPAL. {1}',0,NULL),
(3065,NULL,'MN. Rx de Frecuencia en Equipo RSVA','Rx de Frecuencia en Equipo RSVA. {1}',0,NULL),
(3066,NULL,'MN. Rx de Frecuencia No Disponible','Rx de Frecuencia No Disponible. {1}',1,NULL),
(3067,NULL,'MN. Rx de Frecuencia No Disponible. Baja Prioridad','Rx de Frecuencia No Disponible por Prioridad. {1}',1,NULL),
(3070,NULL,'MN. Operacion Manual','Operacion Manual. {1}',0,NULL),
(3071,NULL,'MN. Error en Operacion Manual','Error en Operacion Manual. {1}',1,NULL),
(3080,NULL,'MN. Informacion','{1}',0,NULL),(3081,NULL,'MN. Error Generico','Error. {1}',1,NULL),
(3082,NULL,'MN. Error de Configuracion','Error de Configuracion: {1}',1,NULL),
(5000,NULL,'Contador de tiempo operativo','{0}',0,NULL),
(5001,NULL,'Elemento entra en estado operativo',NULL,0,NULL),
(5002,NULL,'Elemento sale de estado operativo',NULL,0,NULL),
(5003,NULL,'Contador de tiempo operativo','{0}',0,NULL),
(5004,NULL,'Elemento entra en estado de fallo',NULL,0,NULL),
(5005,NULL,'Elemento sale de estado de fallo',NULL,0,NULL);
/*!40000 ALTER TABLE `incidencias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `incidencias_frances`
--

DROP TABLE IF EXISTS `incidencias_frances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `incidencias_frances` (
  `IdIncidencia` int(10) unsigned NOT NULL,
  `IdIncidenciaCorrectora` int(10) unsigned DEFAULT NULL,
  `Incidencia` varchar(180) DEFAULT NULL,
  `Descripcion` varchar(180) DEFAULT NULL,
  `GeneraError` tinyint(1) DEFAULT NULL,
  `OID` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdIncidencia`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incidencias_frances`
--

LOCK TABLES `incidencias_frances` WRITE;
/*!40000 ALTER TABLE `incidencias_frances` DISABLE KEYS */;
INSERT INTO `incidencias_frances` (`IdIncidencia`, `IdIncidenciaCorrectora`, `Incidencia`, `Descripcion`, `GeneraError`, `OID`) VALUES 
  (1,NULL,'Équipement HF branché','Équipement HF {0} branché',NULL,NULL),
  (2,NULL,'Erreur à l\\\'équipement HF','Erreur à l\\\'équipement HF {0}',NULL,NULL),
  (3,NULL,'Équipement HF débranché','Équipement HF {0} débranché.',NULL,NULL),
  (4,NULL,'Équipement HF livré','Équipement HF livré. Operateur: {0}. Équipement: {1}, Fréquence: {2}.',NULL,NULL),
  (5,NULL,'Équipement HF non livré','Équipement HF non livré.  Équipement: {1}, Fréquence: {2}.',NULL,NULL),
  (6,NULL,'Erreur HF général','Erreur HF général',NULL,NULL),
  (7,NULL,'Erreur HF de livraison','Erreur HF de livraison. Operateur: {0}, Équipement: {1}, Fréquence: {2}.',NULL,NULL),
  (8,NULL,'Erreur HF de non livraison','Erreur HF de non livraison: Équipement: {1}, Fréquence: {2}.',NULL,NULL),
  (9,NULL,'Erreur HF de livraison multiple','Erreur HF de livraison multiple. Operateur {0}.',NULL,NULL),
  (10,NULL,'Erreur HF préparation SELCAL','Erreur HF préparation SELCAL. Operateur: {0}, Équipement: {1}, OnOff: {2}.',NULL,NULL),
  (50,NULL,'Information de service','Information de service {0}: {1} {2} {3} {4}',0,NULL),
  (51,NULL,'Erreur dans le service','Erreur dans le serviceo {0}: {1} {2} {3} {4}',1,NULL),
  (96,NULL,'Changement de la journée.','Changement de la journée',NULL,NULL),
  (101,NULL,'Sélection SCV.','SCV sélectionné {0}.',NULL,NULL),
  (105,NULL,'Chargement sectorisation.','Chargement de la sectorisation {0}',NULL,'.1.1.600.1'),
  (106,NULL,'Erreur de chargement sectorisation.','Erreur de chargement sectorisation {0}',0,'.1.1.600.1'),
  (108,NULL,'Sectorisation rejetée ». Tous les secteurs ne sont pas réels.','Rejeter la sectorisation {0}. Ils ne sont pas tous les secteurs réels',0,'.1.1.600.1'),
  (109,NULL,'Sectorisation implantée automatique.','Sectorisation automatique implantée',NULL,'.1.1.600.1'),
  (110,NULL,'Sectorisation automatique rejetée.','Sectorisation automatique rejetée',0,'.1.1.600.1'),
  (111,NULL,'Secteur livré à la position.','Secteur {0} livré à la position {1}',NULL,'.1.1.600.1'),
  (112,NULL,'Secteur non livré à la position.','Secteur {0} désallouée de la position {1}',NULL,'.1.1.600.1'),
  (113,NULL,'Sectorisation rejetée. 1 + 1 n\\\'est pas active.','Rejeter la sectorisation. 1 + 1 n\\\'est pas active.',NULL,'.1.1.600.1'),
  (114,NULL,'Configuration','{0}',0,NULL),
  (201,NULL,'Serveur actif','{0}',0,NULL),
  (202,201,'Serveur en réserve','{0}',0,NULL),
  (203,NULL,'Serveur tombé','{0}',0,NULL),
  (300,NULL,'Gestion de NBX','{0} {1} {2} {3}',0,NULL),
  (301,NULL,'Alarme NBX','{0} {1} {2} {3}',1,NULL),
  (1001,NULL,'Entrée TOP.','Entrée TOP',1,'1.1.1000.0'),
  (1002,1001,'TOP coupée.','Top coupée',1,'1.1.1000.0'),
  (1003,NULL,'Liaison jacks exécutif.','Liaison jacks exécutif Post: {0}',0,'1.1.1000.1.3.0'),
  (1004,1003,'Débranchement jacks exécutif.','Débranchement jacks exécutif. Post: {0}',0,'1.1.1000.1.3.0'),
  (1005,NULL,'Liaison jacks assistant','Liaison jacks assistant. Post: {0}',0,'1.1.1000.1.3.1'),
  (1006,1005,'Débranchement jacks assistant.','Débranchement jacks assistant. Post: {0}',0,'1.1.1000.1.3.1'),
  (1007,NULL,'Liaison haut-parleur','Liaison haut-parleur {0}. Post: {1}',1,'1.1.1000.1.2'),
  (1008,1007,'Débranchement haut-parleur.','Débranchement haut-parleur {0}. Post: {1}',1,'1.1.1000.1.2 '),
  (1009,NULL,'Panneau en marche','Panneau en marche. Post: {0}',1,'1.1.1000.1.4 '),
  (1010,1009,'Panneau passe à stand-by.','Panneau passe à stand-by. Post: {0}',1,'1.1.1000.1.4 '),
  (1011,NULL,'Page de fréquences est sélectionnée.','Page {0} de fréquences est sélectionnée. Post:. {1}',NULL,'1.1.1000.6'),
  (1014,NULL,'État de PTT.','Etat de PTT: {0}. Post: {1}. Secteur {2}',NULL,'1.1.1000.2'),
  (1015,NULL,'Facilité sélectionnée','Facilité sélectionnée: {0}. Post:. {1}',NULL,'1.1.1000.8'),
  (1016,NULL,'Appel entrant.','Appel entrant. Accès: {0}. Post:. {1}',NULL,'1.1.1000.9'),
  (1017,NULL,'Appel sortant Post.','Appel sortant. Accès: {0}. Post:. {1}',NULL,'1.1.1000.7'),
  (1019,NULL,'Fin d\\\'appel Post.','Fin d\\\'appel. Accès: {0}. Post:. {1}',NULL,'1.1.1000.10 '),
  (1020,NULL,'Appel établi','Appel établi. Accès: {0}. Post:. {1}',NULL,'1.1.1000.11'),
  (1021,NULL,'Séance Breifing','Séance Breifing à le poste: {0}. {1}',NULL,'1.1.1000.13'),
  (1022,NULL,'Fonction de reproduction.','Fonction de reproduction post: {0}. {1}',NULL,'1.1.1000.12'),
  (1023,NULL,'Câble d\'enregistrement connecté','Câble d\'enregistrement connecté',0,NULL),
  (1024,NULL,'Câble d\'enregistrement déconnecté','Câble d\'enregistrement déconnecté',1,NULL),
  (1025,NULL,'Événement réseau local','État du réseau local. LAN1: {0}, LAN2: {1}',1,NULL),
  (2001, NULL, 'Modulo passerelle en service', '{0} En service', 1, '1.1.100.2.0'),
  (2002, 2001, 'Modulo passerelle hors service', '{0} Hors service', 1, '1.1.100.2.0'),
  (2003, NULL, 'Ressource radio en service', '{0}. Ressource radio en service', NULL, '1.1.200.3.1.17'),
  (2004, 2003, 'Ressource radio hors service', '{0}. Ressource radio hors service', 0, '1.1.200.3.1.17'),
  (2005, NULL, 'Ressource téléphonie en service', '{0}. Ressource téléphonie en service', NULL, '1.1.400.3.1.14'),
  (2006, 2005, 'Ressources téléphonie hors service', '{0}. Ressource téléphonie hors service', 0, '1.1.400.3.1.14'),
  (2007, NULL, 'Carte d\\\'interface en service', 'Carte d\\\'interface: {0} en service', 1, '1.1.100.31.1.2'),
  (2008, 2007, 'Carte d\\\'interface hors service', 'Carte d\\\'interface: {0} hors service', 1, '1.1.100.31.1.2'),
  (2009, NULL, 'Ressource R2 en service', '{0}. Ressource R2 en service', NULL, '1.1.500.3.1.17'),
  (2010, NULL, 'Ressource R2 hors service', '{0}. Ressource R2 hors service', NULL, '1.1.500.3.1.17'),
  (2012, NULL, 'Erreur de protocole LCN.', '{0}. Erreur de protocole LCN.', NULL, '1.1.300.3.1.17'),
  (2013, NULL, 'Ressource LCN en service', '{0}. Ressource LCN en service', NULL, '1.1.300.3.1.17'),
  (2014, 2013, 'Ressource LCN hors service', '{0}. Ressource LCN hors service', 0, '1.1.300.3.1.17'),
  (2015, NULL, 'Ressource N5 en service', '{0}. Ressource N5 en service', 0, NULL),
  (2016, NULL, 'Ressource N5 hors service', '{0}. Ressource N5 hors service', 0, NULL),
  (2017, NULL, 'Ressource QSIG en service', '{0}. Ressource QSIG en service', 0, NULL),
  (2018, NULL, 'Débranchement ressource QSIG', '{0}. Débranchement ressource QSIG', 0, NULL),
  (2020,NULL,'Appel R2 entrant.','Appel R2 entrant. Ressource {0}. Tronc {1}. {2} Origine. Priorité {3}. Destination {4} ',NULL,'1.1.500.3.1.17 '),
  (2021,NULL,'Fin appel R2.','Fin appel R2. Ressource {0}. Tronc {1}.',NULL,'1.1.500.3.1.17 '),
  (2022,NULL,'Appel R2 sortant.','Appel R2 sortant. Ressource {0}. Tronc {1}. {2} Origine. Priorité {3}. Destination {4}',NULL,'1.1.500.3.1.17 '),
  (2023,NULL,'Fin appel R2.','Fin appel R2. Ressource {0}. Tronc {1}.',NULL,'1.1.500.3.1.17 '),
  (2024,NULL,'Appel de test R2.','Appel de test R2. Passerelle: {0}. Ressource: {1}',NULL,'1.1.500.3.1.17 '),
  (2025,NULL,'Erreur de protocole R2.','Erreur de protocole R2. Passerelle: {0}. Ressource: {1}',0,'1.1.500.3.1.17 '),
  (2030,NULL,'Appel LCN entrant.','Appel LCN entrant: Ressource: {0}',NULL,'1.1.300.3.1.17'),
  (2031,NULL,'Fin d\\\'appel entrant LCN.','Fin d\\\'appel entrant LCN: Ressource: {0}.',NULL,'1.1.300.3.1.17'),
  (2032,NULL,'Appel LCN sortant.','Appel LCN sortant: Ressource:. {0}',NULL,'1.1.300.3.1.17'),
  (2033,NULL,'Fin appel sortant LCN.','Fin d\\\'appel sortant LCN: Ressource:. {0}',NULL,'1.1.300.3.1.17'),
  (2040,NULL,'Appel téléphonique entrant.','Appel téléphonique entrant: Ressource {0}. Ligne {1}.  Réseau {2}. Type d\\\'interface {3}. Accès {4}.',NULL,'1.1.400.3.1.14'),
  (2041,NULL,'Fin appel téléphonique entrant.','Fin appel téléphonique entrant: Ressource {0}. Ligne {1}.  Réseau {2}. Type d\\\'interface {3}. Accès {4}.',NULL,'1.1.400.3.1.14'),
  (2042,NULL,'Appel téléphonique sortant.','Appel téléphonique sortant: Ressource {0}. Ligne {1}.  Réseau {2}. Type d\\\'interface {3}. Accès {4}.',NULL,'1.1.400.3.1.14'),
  (2043,NULL,'Fin appel téléphonique sortant.','Fin appel téléphonique sortant: Ressource {0}. Ligne {1}.  Réseau {2}. Type d\\\'interface {3}. Accès {4}.',NULL,'1.1.400.3.1.14'),
  (2050,NULL,'PTT On.','PTT On: Passerelle: {0}. Ressource {1}. Fréquence: {2}',NULL,'1.1.200.3.1.17'),
  (2051,NULL,'PTT Off.','PTT Off: Passerelle: {0}. Ressource {1}. Fréquence: {2}',NULL,'1.1.200.3.1.17'),
  (2052,NULL,'SQ On.','SQ On: Passerelle: {0}. Ressource {1}. Fréquence: {2}',NULL,'1.1.200.3.1.17'),
  (2053,NULL,'SQ Off.','SQ Off: Passerelle: {0}. Ressource {1}. Fréquence: {2} ',NULL,'1.1.200.3.1.17'),
  (2100,NULL,'Sélection Principal/Réserve','Passerelle {0} passe a  l\\\'état de {1}',NULL,'1.1.100.21.0'),
  (2200,NULL,'GW Event','{0}: {1} {2} {3}',0,NULL),
  (2300,NULL,'GW Operation','{0}: {1} {2} {3}',0,NULL),
  (3001,NULL,'Liaison équipement externe.','Liaison équipement externe',1,NULL),
  (3002,3001,'Débranchement équipement externe.','Débranchement équipement externe',1,NULL),
  (3003,NULL,'Abonné PBX Active', 'Abonné PBX Active', 0,NULL),
  (3004,NULL,'Abonné PBX Non active', 'Abonné PBX Non active', 0,NULL),
  (3050,NULL,'MN. Unité disponible','Unité disponible. {1}',0,NULL),
  (3051,NULL,'MN. Unité en échec','Unité en échec. {1}',1,NULL),
  (3052,NULL,'MN. Erreur de communication dans l\'unité','Erreur de communication dans l\'unité. {1}',1,NULL),
  (3060,NULL,'MN. Fréquence TX dans l\'unité principale','Fréquence TX dans l\'unité principale. {1}',0,NULL),
  (3061,NULL,'MN. Fréquence TX en unité de réserve','Fréquence TX en unité de réserve. {1}',0,NULL),
  (3062,NULL,'MN. Fréquence TX non disponible','Fréquence TX non disponible. {1}',1,NULL),
  (3063,NULL,'MN. Fréquence TX non disponible. Priorité basse','Fréquence TX non disponible. Priorité basse. {1}',1,NULL),
  (3064,NULL,'MN. Fréquence RX dans l\'unité principale','Fréquence RX dans l\'unité principaleL. {1}',0,NULL),
  (3065,NULL,'MN. Fréquence RX en unité de réserve','Fréquence RX en unité de réserve. {1}',0,NULL),
  (3066,NULL,'MN. Fréquence RX non disponible','Fréquence R X non disponible. {1}',1,NULL),
  (3067,NULL,'MN. Fréquence RX non disponible. Priorité basse','Fréquence RX non disponible. Priorité basse. {1}',1,NULL),
  (3070,NULL,'MN. Opération manuelle','Opération manuellel. {1}',0,NULL),
  (3071,NULL,'MN. Erreur d\'opération manuelle','Erreur d\'opération manuellel. {1}',1,NULL),
  (3080,NULL,'MN. Information','{1}',0,NULL),
  (3081,NULL,'MN. Erreur générique','Erreur. {1}',1,NULL),
  (3082,NULL,'MN. Erreur de configuration','Erreur de configuration: {1}',1,NULL),
  (5000,NULL,'Contador de tiempo operativo','{0}',0,NULL),
  (5001,NULL,'Elemento entra en estado operativo',NULL,0,NULL),
  (5002,NULL,'Elemento sale de estado operativo',NULL,0,NULL),
  (5003,NULL,'Contador de tiempo operativo','{0}',0,NULL),
  (5004,NULL,'Elemento entra en estado de fallo',NULL,0,NULL),
  (5005,NULL,'Elemento sale de estado de fallo',NULL,0,NULL);
/*!40000 ALTER TABLE `incidencias_frances` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `incidencias_ingles`
--

DROP TABLE IF EXISTS `incidencias_ingles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `incidencias_ingles` (
  `IdIncidencia` int(10) unsigned NOT NULL,
  `IdIncidenciaCorrectora` int(10) unsigned DEFAULT NULL,
  `Incidencia` varchar(180) DEFAULT NULL,
  `Descripcion` varchar(180) DEFAULT NULL,
  `GeneraError` tinyint(1) DEFAULT NULL,
  `OID` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdIncidencia`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incidencias_ingles`
--

LOCK TABLES `incidencias_ingles` WRITE;
/*!40000 ALTER TABLE `incidencias_ingles` DISABLE KEYS */;
INSERT INTO `incidencias_ingles` (`IdIncidencia`, `IdIncidenciaCorrectora`, `Incidencia`, `Descripcion`, `GeneraError`, `OID`) VALUES
  (1,NULL,'HF Equipment on','HF Equipment {0} on',NULL,NULL),
  (2,NULL,'Error in HF equipment','Error in HF equipment {0}',NULL,NULL),
  (3,NULL,'HF equipment off','HF equipment {0} off',NULL,NULL),
  (4,NULL,'HF equipment assigned','HF equipment assigned. User: {0}. Equipment: {1}, Frequency: {2}.',NULL,NULL),
  (5,NULL,'HF equipment unassigned','HF equipment unassigned.  Equipment: {1}, Frequency: {2}.',NULL,NULL),
  (6,NULL,'HF equipment error','HF equipment error',NULL,NULL),
  (7,NULL,'HF assigning error','HF assigning error. User: {0}, Equipment: {1}, Frequency: {2}.',NULL,NULL),
  (8,NULL,'HF unassigning error','HF unassigning error: Equipment: {1}, Frequency: {2}.',NULL,NULL),
  (9,NULL,'HF multiple assigning error','HF multiple assigning error. User {0}.',NULL,NULL),
  (10,NULL,'SELCAL preparation error','SELCAL preparation error. User: {0}, Equipment: {1}, OnOff: {2}.',NULL,NULL),
  (50,NULL,'Service information','Service information {0}: {1} {2} {3} {4}',0,NULL),
  (51,NULL,'Service ERROR','Error on Service {0}: {1} {2} {3} {4}',1,NULL),
  (96,NULL,'Change of day.','Change of day',NULL,NULL),
  (101,NULL,'VCS Selection.','Selectioned VCS {0}.',NULL,NULL),
  (105,NULL,'Sectorization load','Sectorization load {0}',NULL,'.1.1.600.1'),
  (106,NULL,'Error in sectorization load.','Error sectorization load {0}',1,'.1.1.600.1'),
  (108,NULL,'Rejection in sectorization. All real sectors are not in sectorization.','Reject in sectorization {0}. All real sectors are not in sectorization',1,'.1.1.600.1'),
  (109,NULL,'Loaded automatic sectorization.','Loaded automatic sectorization',NULL,'.1.1.600.1'),
  (110,NULL,'Rejected automatic sectorization.','Rejected automatic sectorization',1,'.1.1.600.1'),
  (111,NULL,'Position assigned to the sector.','Position {1} assigned to the sector {0}',NULL,'.1.1.600.1'),
  (112,NULL,'Position unassigned to the sector.','Position {1} unassigned to the sector {0}',NULL,'.1.1.600.1'),
  (113,NULL,'Rejection in sectorización. 1 + 1 is not active.','Rejection in sectorization. 1 + 1 is not active.',NULL,'.1.1.600.1'),
  (114,NULL,'Configuration','{0}',0,NULL),
  (201,NULL,'Active Server.','{0}',0,NULL),
  (202,201,'Standby server.','{0}',0,NULL),
  (203,NULL,'Fallen Server.','{0}',0,NULL),
  (300,NULL,'NBX Management','{0} {1} {2} {3}',0,NULL),
  (301,NULL,'NBX Alarm','{0} {1} {2} {3}',1,NULL),
  (1001,NULL,'Entering OT.','Entering OT. Position: {0}',NULL,'1.1.1000.0'),
  (1002,1001,'OT down.','OT down. Position: {0}',1,'1.1.1000.0'),
  (1003,NULL,'Executive jacks on.','Executive jacks on. Position: {0}',NULL,'1.1.1000.1.3.0'),
  (1004,1003,'Executive jacks off.','Executive jacks off. Position: {0}',1,'1.1.1000.1.3.0'),
  (1005,NULL,'Assistant jacks on.','Assistant jacks on. Position: {0}',NULL,'1.1.1000.1.3.1'),
  (1006,1005,'Assistant jacks off.','Assistant jacks off. Position: {0}',1,'1.1.1000.1.3.1'),
  (1007,NULL,'Speaker on.','Speaker on {0}. Position: {1}',1,'1.1.1000.1.2'),
  (1008,1007,'Speaker off','Speaker off. Position: {0}',0,'1.1.1000.1.2'),
  (1009,NULL,'Panel to operation','Panel to operation. Position: {0}',-1,'1.1.1000.1.4'),
  (1010,1009,'Panel to standby.','Panel to standby. Position: {0}',1,'1.1.1000.1.4'),
  (1011,NULL,'Frequency page selected','Frequency page selected: {0}. Position: {1}. Sector: {2}',NULL,'1.1.1000.6'),
  (1014,NULL,'PTT status.','PTT status: {0}. Position:{1}. PTT Type: {2}',NULL,'1.1.1000.2'),
  (1015,NULL,'Selected function','Selected function: {0}. Position: {1}. Sector: {2}',NULL,'1.1.1000.8'),
  (1016,NULL,'Incomming call Position.','Incomming calll. Access: {0}. Position: {1}.',NULL,'1.1.1000.9'),
  (1017,NULL,'Outgoing call Position.','Outgoing calll. Access: {0}. Position: {1}.',NULL,'1.1.1000.7'),
  (1019,NULL,'Ending  call Position.','Ending call. Access: {0}. Position: {1}.',NULL,'1.1.1000.10'),
  (1020,NULL,'Established telephone call','Established call.  Access: {0}. Position: {1}.',NULL,NULL),
  (1021,NULL,'Briefing function',' Briefing session in : {0} {1}',NULL,'1.1.1000.13'),
  (1022,NULL,'Playing function','Local playing breafing in {0} {1}',NULL,'1.1.1000.12'),
  (1023,NULL,'Recording cable connected','Recording cable connected',0,NULL),
  (1024,NULL,'Recording cable disconnected','Recording cable disconnected',1,NULL),
  (1025,NULL,'LAN Event','Status: LAN1 {0}, LAN2 {1}',1,NULL),
  (2001, NULL, 'Gateway module in service', '{0}. In service', 1, '1.1.100.2.0'),
  (2002, 2001, 'Gateway module out of service', '{0}. Out of service', 1, '1.1.100.2.0'),
  (2003, NULL, 'Radio resource on.', '{0}. Radio resource on.', 1, '1.1.200.3.1.17'),
  (2004, 2003, 'Radio resource off.', '{0}. Radio resource off.', 1, '1.1.200.3.1.17'),
  (2005, NULL, 'Telephone resource on.', '{0}. Telephone resource on.', 1, '1.1.400.3.1.14'),
  (2006, 2005, 'Telephone resource off.', '{0}. Telephone resource off.', 1, '1.1.400.3.1.14'),
  (2007, NULL, 'Interface on', 'Interface on: {0}', 1, '1.1.100.31.1.2'),
  (2008, 2007, 'Interface off', 'Interface off: {0}', 1, '1.1.100.31.1.2'),
  (2009, NULL, 'R2 resource on.', '{0}. R2 resource on.', 1, '1.1.500.3.1.17'),
  (2010, NULL, 'R2 resource off.', '{0}. R2 resource off.', 1, '1.1.500.3.1.17'),
  (2012, NULL, 'Protocol LCN error.', '{0}. Protocol LCN error', 0, '1.1.300.3.1.17'),
  (2013, NULL, 'LCN resource on.', '{0}. LCN resource on.', 1, '1.1.300.3.1.17'),
  (2014, 2013, 'LCN resource off.', '{0}. LCN resource off.', 1, '1.1.300.3.1.17'),
  (2015, NULL, 'N5 Resource on', '{0}. N5 Resource on', 1, NULL),
  (2016, NULL, 'N5. Resource off', '{0}. N5 Resource off', 1, NULL),
  (2017, NULL, 'QSIG Resource on', '{0}. QSIG Resource on', 0, NULL),
  (2018, NULL, 'QSIG Resource off', '{0}. QSIG Resource off', 0, NULL),
  (2020,NULL,'Incomming call R2.','Incomming call R2. Resource {0}. Trunk {1}. Source {2}. Priority {3}. Target {4}',NULL,'1.1.500.3.1.17'),
  (2021,NULL,'End of incomming call R2.','End of incomming call R2. Resource {0}. Trunk {1}. Source {2}. Priority {3}. Target {4}',NULL,'1.1.500.3.1.17'),
  (2022,NULL,'Outgoing call R2.','Outgoing call R2.  Resource {0}. Trunk {1}. Source {2}. Priority {3}. Target {4}',NULL,'1.1.500.3.1.17'),
  (2023,NULL,'End of outgoing call R2.','End of outgoing call R2. Resource {0}. Trunk {1}. Source {2}. Priority {3}. Target {4}',NULL,'1.1.500.3.1.17'),
  (2024,NULL,'Test call  R2.','Test call R2',NULL,'1.1.500.3.1.17'),
  (2025,NULL,'R2 protocol error.','R2 protocol error.',1,'1.1.500.3.1.17'),
  (2030,NULL,'Incomming call LCN.','Incomming call LCN: Resource: {0}. Line: {1}',NULL,'1.1.300.3.1.17'),
  (2031,NULL,'End of incomming call LCN.','End of incomming call LCN: Resource: {0}. Line: {1}',NULL,'1.1.300.3.1.17'),
  (2032,NULL,'Outgoing call LCN.','Outgoing call LCN: Resource: {0}. Line: {1}.',NULL,'1.1.300.3.1.17'),
  (2033,NULL,'End of outgoing call LCN.','End of outgoing call LCN: Resource: {0}. Line: {1}.',NULL,'1.1.300.3.1.17'),
  (2040,NULL,'Telephony incomming call.','Telephony incomming call.: Resource {0}. Line {1}. Network {2}. Interfaz {3}. Access {4}.',NULL,'1.1.400.3.1.14'),
  (2041,NULL,'End of telephony incomming call.','End of telephony incomming call: Resource {0}. Line {1}. Network {2}. Interfaz {3}. Access {4}.',NULL,'1.1.400.3.1.14'),
  (2042,NULL,'Telephony outgoing call.','Telephony outgoing call:  Resource {0}. Line {1}. Network {2}. Interfaz {3}. Access {4}.',NULL,'1.1.400.3.1.14'),
  (2043,NULL,'End of telephony outgoing call.','End of telephony outgoing call: Resource {0}. Line {1}. Network {2}. Interfaz {3}. Access {4}.',NULL,'1.1.400.3.1.14'),
  (2050,NULL,'PTT On.','PTT On: Resource {0}. Frequency: {1}',NULL,'1.1.200.3.1.17'),
  (2051,NULL,'PTT Off.','PTT Off: Resource {0}. Frequency: {1}',NULL,'1.1.200.3.1.17'),
  (2052,NULL,'SQ On.','SQ On: Resource {0}. Frequency: {1}',NULL,'1.1.200.3.1.17'),
  (2053,NULL,'SQ Off.','SQ Off: Resource {0}. Frequency: {1}',NULL,'1.1.200.3.1.17'),
  (2100,NULL,'Selection Main/Standby','The gateway {0} switchs to {1}.',NULL,'1.1.100.21.0'),
  (2200,NULL,'GW Event','{0}: {1} {2} {3}',0,NULL),
  (2300,NULL,'GW Manual Operation','{0}: {1} {2} {3}',0,NULL),
  (3001,NULL,'Entering external equipment','Entering external equipment',NULL,NULL),
  (3002,3001,'External equipment down','External equipment down',1,NULL),
  (3003,NULL,'Pbx Subscriber ON', 'Pbx Subscriber ON', 0,NULL),
  (3004,NULL,'Pbx Subscriber OFF', 'Pbx Subscriber OFF', 0,NULL),
  (3050,NULL,'MN. Equipment available ','Equipment available. {1}',0,NULL),
  (3051,NULL,'MN. Equipment in failure','Equipment in failure. {1}',1,NULL),
  (3052,NULL,'MN. Error in Communication with Equipment','Error in Communication with Equipment. {1}',1,NULL),
  (3060,NULL,'MN. TX frequency in main equipment','Tx  frequency in main equipment. {1}',0,NULL),
  (3061,NULL,'MN. TX frequency in reserve equipment','Tx  frequency in reserve equipment. {1}',0,NULL),
  (3062,NULL,'MN. TX frequency not available','Tx de Frecuencia No Disponible. {1}',1,NULL),
  (3063,NULL,'MN. TX frequency not available. Low priority','Tx frequency not available by priority. {1}',1,NULL),
  (3064,NULL,'MN. RX frequency in main equipment','Rx  frequency in main equipment. {1}',0,NULL),
  (3065,NULL,'MN. RX frequency in reserve equipment','Rx  frequency in reserve equipment. {1}',0,NULL),
  (3066,NULL,'MN. RX  frequency not available','Rx  frequency not available. {1}',1,NULL),
  (3067,NULL,'MN. RX frequency not available. Low priority','Rx  frequency not available by priority. {1}',1,NULL),
  (3070,NULL,'MN. Manual Operation','Manual Operation. {1}',0,NULL),
  (3071,NULL,'MN. Error in manual operation','Error in manual operation. {1}',1,NULL),
  (3080,NULL,'MN. Information','{1}',0,NULL),
  (3081,NULL,'MN. Generic error','Error. {1}',1,NULL),
  (3082,NULL,'MN. Configuration error','Configuration error: {1}',1,NULL),
  (5000,NULL,'Contador de tiempo operativo','{0}',0,NULL),
  (5001,NULL,'Elemento entra en estado operativo',NULL,0,NULL),
  (5002,NULL,'Elemento sale de estado operativo',NULL,0,NULL),
  (5003,NULL,'Contador de tiempo operativo','{0}',0,NULL),
  (5004,NULL,'Elemento entra en estado de fallo',NULL,0,NULL),
  (5005,NULL,'Elemento sale de estado de fallo',NULL,0,NULL);
/*!40000 ALTER TABLE `incidencias_ingles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `metodos_bss`
--

DROP TABLE IF EXISTS `metodos_bss`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `metodos_bss` (
  `idmetodos_bss` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idmetodos_bss`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `metodos_bss`
--

LOCK TABLES `metodos_bss` WRITE;
/*!40000 ALTER TABLE `metodos_bss` DISABLE KEYS */;
INSERT INTO `metodos_bss` VALUES (0,'NUCLEO'),(1,'RSSI');
/*!40000 ALTER TABLE `metodos_bss` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Current Database: `new_cd40_trans`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `new_cd40_trans` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `new_cd40_trans`;

--
-- Table structure for table `incidencias`
--

DROP TABLE IF EXISTS `incidencias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `incidencias` (
  `IdIncidencia` int(10) unsigned NOT NULL,
  `IdIncidenciaCorrectora` int(10) unsigned DEFAULT NULL,
  `Incidencia` varchar(180) DEFAULT NULL,
  `Descripcion` varchar(180) DEFAULT NULL,
  `GeneraError` tinyint(1) DEFAULT NULL,
  `OID` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdIncidencia`),
  KEY `Incidencias_FKIndex1` (`IdIncidenciaCorrectora`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incidencias`
--

LOCK TABLES `incidencias` WRITE;
/*!40000 ALTER TABLE `incidencias` DISABLE KEYS */;
INSERT INTO `incidencias` (`IdIncidencia`, `IdIncidenciaCorrectora`, `Incidencia`, `Descripcion`, `GeneraError`, `OID`) VALUES 
(1,NULL,'Equipo HF conectado','Equipo HF {0} conectado',0,NULL),
(2,NULL,'Error en equipo HF','Error en equipo HF {0}',0,NULL),
(3,NULL,'Equipo HF desconectado','Equipo HF {0} desconectado',0,NULL),
(4,NULL,'Equipo HF asignado','Equipo HF asignado. Usuario: {0}. Equipo: {1}, Frecuencia: {2}.',0,NULL),
(5,NULL,'Equipo HF liberado','Equipo HF liberado. Equipo: {0}, Frecuencia: {1}.',0,NULL),
(6,NULL,'Error HF general','Error HF general',0,NULL),
(7,NULL,'Error HF asignación','Error HF asignación. Usuario: {0}, Equipo: {1}, Frecuencia: {2}.',0,NULL),
(8,NULL,'Error HF desasignación','Error HF desasignación: Equipo: {0}, Frecuencia: {1}.',0,NULL),
(9,NULL,'Error HF asignación múltiple','Error HF asignación múltiple. Usuario {0}.',0,NULL),
(10,NULL,'Error HF preparación SELCAL','Error HF preparación SELCAL. Usuario: {0}, Equipo: {1}, OnOff: {2}.',0,NULL),
(50,NULL,'INFO Servicio ','Informacion de Servicio {0}: {1} {2} {3} {4}',0,NULL),
(51,NULL,'ERROR Servicio','Error en Servicio {0}: {1}',1,NULL),
(96,NULL,'Cambio de día.','Cambio de día',0,NULL),
(101,NULL,'Selección SCV.','SCV {0} seleccionado.',0,NULL),
(105,NULL,'Carga de sectorización.','Carga de sectorización {0}',0,'.1.1.600.1'),
(106,NULL,'Error carga sectorización.','Error carga sectorización {0}',0,'.1.1.600.1'),
(108,NULL,'Rechazo sectorización. No están todos los sectores reales.','Rechazo sectorización {0}. No están todos los sectores reales',0,'.1.1.600.1'),
(109,NULL,'Sectorización automática implantada.','Sectorización automática implantada',0,'.1.1.600.1'),
(110,NULL,'Sectorización automática rechazada.','Sectorización automática rechazada',0,'.1.1.600.1'),
(111,NULL,'Sector asignado a posición.','Sector {0} asignado a posición {1}',0,'.1.1.600.1'),
(112,NULL,'Sector desasignado de la posición.','Sector {0} desasignado de la posición {1}',0,'.1.1.600.1'),
(113,NULL,'Rechazo sectorización. 1 + 1 no activo.','Rechazo sectorización. 1 + 1 no activo.',0,'.1.1.600.1'),
(114,NULL,'Configuración','{0}',0,NULL),
(201,NULL,'Servidor activo.','{0}',0,NULL),
(202,201,'Servidor reserva.','{0}',0,NULL),
(203,NULL,'Servidor caído.','{0}',0,NULL),
(300,NULL,'Gestion NBX','{0} {1} {2} {3}',0,NULL),
(301,NULL,'Alarma NBX','{0} {1} {2} {3}',1,NULL),
(1001,NULL,'Entrada TOP.','Entrada TOP',1,'1.1.1000.0'),
(1002,1001,'Caída TOP.','Caída TOP',1,'1.1.1000.0'),
(1003,NULL,'Conexión jacks ejecutivo.','Conexión jacks ejecutivo. Posición: {0}',0,'1.1.1000.1.3.0'),
(1004,1003,'Desconexión jacks ejecutivo.','Desconexión jacks ejecutivo. Posición: {0}',0,'1.1.1000.1.3.0'),
(1005,NULL,'Conexión jacks ayudante.','Conexión jacks ayudante. Posición: {0}',0,'1.1.1000.1.3.1'),
(1006,1005,'Desconexión jacks ayudante.','Desconexión jacks ayudante. Posición: {0} ',0,'1.1.1000.1.3.1'),
(1007,NULL,'Conexión altavoz.','Conexión altavoz {0}. Posición: {1} ',1,'1.1.1000.1.2'),
(1008,1007,'Desconexión altavoz.','Desconexión altavoz {0}. Posición: {1} ',1,'1.1.1000.1.2'),
(1009,NULL,'Panel pasa a operación.','Panel pasa a operación. Posición: {0} ',1,'1.1.1000.1.4'),
(1010,1009,'Panel pasa a standby.','Panel pasa a standby. Posición: {0} ',1,'1.1.1000.1.4'),
(1011,NULL,'Página de frecuencias seleccionada.','Página de frecuencias seleccionada: {0}. Posición: {1}.',0,'1.1.1000.6'),
(1014,NULL,'Estado PTT.','Estado PTT: {0}. Posicion:{1}. Sector{2}',0,'1.1.1000.2'),
(1015,NULL,'Facilidad seleccionada.','Facilidad seleccionada: {0}. Posición: {1}.',0,'1.1.1000.8'),
(1016,NULL,'Llamada entrante Posicion.','Llamada entrante. Acceso: {0}. Posicion: {1}.',0,'1.1.1000.9'),
(1017,NULL,'Llamada saliente Posicion.','Llamada saliente. Acceso: {0}. Posicion: {1}.',0,'1.1.1000.7'),
(1019,NULL,'Fin llamada Posicion.','Fin llamada. Acceso: {0}.Posicion {1}.',0,'1.1.1000.10'),
(1020,NULL,'Llamada telefonía establecida','Llamada establecida. Acceso: {0}. Posicion: {1}.',0,'1.1.1000.11'),
(1021,NULL,'Función Briefing',' Sesión Briefing en Puesto: {0} {1}',NULL,'1.1.1000.13'),
(1022,NULL,'Reproducción GLP','Grabación Local en Puesto {0} {1}',NULL,'1.1.1000.12'),
(1023,NULL,'Cable de Grabación Conectado','Cable de Grabación Conectado',0,NULL),
(1024,NULL,'Cable de Grabación Desconectado','Cable de Grabación Desconectado',1,NULL),
(1025,NULL,'Evento LAN','Estado LAN. Lan1: {0}, Lan2: {1}',1,NULL),
(2001, NULL, 'Modulo GW en Servicio', '{0} En servicio', 1, '1.1.100.2.0'),
(2002, 2001, 'Modulo GW Fuera de Servicio', '{0} Fuera de Servicio', 1, '1.1.100.2.0'),
(2003, NULL, 'Recurso Radio en Servicio', '{0}. Recurso Radio en Servicio', 1, '1.1.200.3.1.17'),
(2004, 2003, 'Recurso Radio Fuera de Servicio', '{0}. Recurso Radio Fuera de Servicio', 1, '1.1.200.3.1.17'),
(2005, NULL, 'Recurso Telefonía en Servicio', '{0}. Recurso Telefonía en Servicio', 1, '1.1.400.3.1.14'),
(2006, 2005, 'Recurso Telefonía Fuera de Servicio', '{0}. Recurso Telefonía Fuera de Servicio', 1, '1.1.400.3.1.14'),
(2007, NULL, 'Tarjeta interfaz en Servicio', 'Tarjeta interfaz: Slot {0}. en Servicio', 1, '1.1.100.31.1.2'),
(2008, 2007, 'Tarjeta interfaz Fuera de Servicio', 'Tarjeta interfaz: Slot {0}. Fuera de Servicio', 1, '1.1.100.31.1.2'),
(2009, NULL, 'Recurso R2 en Servicio', '{0}. Recurso R2 en Servicio', 1, '1.1.500.3.1.17'),
(2010, NULL, 'Recurso R2 Fuera de Servicio', '{0}. Recurso R2 Fuera de Servicio', 1, '1.1.500.3.1.17'),
(2012, NULL, 'Error protocolo LCN.', '{0}. Error protocolo LCN.', 0, '1.1.300.3.1.17'),
(2013, NULL, 'Recurso LCN en Servicio', '{0}. Recurso LCN en Servicio', 1, '1.1.300.3.1.17'),
(2014, 2013, 'Recurso LCN Fuera de Servicio', '{0}. Recurso LCN Fuera de Servicio', 1, '1.1.300.3.1.17'),
(2015, NULL, 'Recurso N5 en Servicio', '{0}. Recurso N5 en Servicio', 1, NULL),
(2016, NULL, 'Recurso N5 Fuera de Servicio', '{0}. Recurso N5 Fuera de Servicio', 1, NULL),
(2017, NULL, 'Recurso QSIG en Servicio', '{0}. Recurso QSIG en Servicio', 0, NULL),
(2018, NULL, 'Recurso QSIG Fuera de Servicio', '{0}. Recurso QSIG fuera de Servicio', 0, NULL),
(2020,NULL,'Llamada entrante R2.','Llamada entrante R2. Recurso {0}. Troncal {1}. Origen {2}. Prioridad {3}. Destino {4}',0,'1.1.500.3.1.17'),
(2021,NULL,'Fin llamada entrante R2.','Fin llamada entrante R2. Recurso {0}. Troncal {1}.',0,'1.1.500.3.1.17'),
(2022,NULL,'Llamada saliente R2.','Llamada saliente R2. Recurso {0}. Troncal {1}. Origen {2}. Prioridad {3}. Destino {4}',0,'1.1.500.3.1.17'),
(2023,NULL,'Fin llamada saliente R2.','Fin llamada saliente R2. Recurso {0}. Troncal {1}.',0,'1.1.500.3.1.17'),
(2024,NULL,'Llamada prueba R2.','Llamada prueba R2. Pasarela: {0}. Recurso: {1}',0,'1.1.500.3.1.17'),
(2025,NULL,'Error protocolo R2.','Error protocolo R2. Pasarela: {0}. Recurso: {1}',0,'1.1.500.3.1.17'),
(2030,NULL,'Llamada entrante LCN.','Llamada entrante LCN: Recurso: {0}',0,'1.1.300.3.1.17'),
(2031,NULL,'Fin llamada entrante LCN.','Fin llamada entrante LCN: Recurso: {0}.',0,'1.1.300.3.1.17'),
(2032,NULL,'Llamada saliente LCN.','Llamada saliente LCN: Recurso: {0}.',0,'1.1.300.3.1.17'),
(2033,NULL,'Fin llamada saliente LCN.','Fin llamada saliente LCN: Recurso: {0}.',0,'1.1.300.3.1.17'),
(2040,NULL,'Llamada entrante telefonía.','Llamada entrante telefonía: Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',0,'1.1.400.3.1.14'),
(2041,NULL,'Fin llamada entrante telefonía.','Fin llamada entrante telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',0,'1.1.400.3.1.14'),
(2042,NULL,'Llamada saliente telefonía.','Llamada saliente telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',0,'1.1.400.3.1.14'),
(2043,NULL,'Fin llamada saliente telefonía.','Fin llamada saliente telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',0,'1.1.400.3.1.14'),
(2050,NULL,'PTT On.','PTT On:  Recurso {0}. Frecuencia: {1}',0,'1.1.200.3.1.17'),
(2051,NULL,'PTT Off.','PTT Off:  Recurso {0}. Frecuencia: {1}',0,'1.1.200.3.1.17'),
(2052,NULL,'SQ On.','SQ On: Recurso {0}. Frecuencia: {1}',0,'1.1.200.3.1.17'),
(2053,NULL,'SQ Off.','SQ Off: Recurso {0}. Frecuencia: {1}',0,'1.1.200.3.1.17'),
(2100,NULL,'Selección Principal/Reserva','Pasarela {0} pasa a {1}',0,'1.1.100.21.0'),
(2200,NULL,'Evento Pasarela','{0}: {1} {2} {3}',1,NULL),
(2300,NULL,'Operacion Local en Pasarela','{0}: {1} {2} {3}',0,NULL),
(3001,NULL,'Entrada Equipo Externo.','Entrada Equipo Externo',1,NULL),
(3002,3001,'Caída Equipo Externo.','Caída Equipo Externo',1,NULL),
(3003,NULL,'Abonado PBX Activo', 'Abonado PBX Activo', 0,NULL),
(3004,NULL,'Abonado PBX Inactivo', 'Abonado PBX Inactivo', 0,NULL),
(3050,NULL,'MN. Equipo Disponible','Equipo Disponible. {1}',0,NULL),
(3051,NULL,'MN. Equipo en Fallo','Equipo en Fallo. {1}',1,NULL),
(3052,NULL,'MN. Error en Comunicacion con Equipo','Error en Comunicacion con equipo. {1}',1,NULL),
(3060,NULL,'MN. Tx de Frecuencia en Equipo PPAL','Tx de Frecuencia en Equipo PPAL. {1}',0,NULL),
(3061,NULL,'MN. Tx de Frecuencia en Equipo RSVA','Tx de Frecuencia en Equipo RSVA. {1}',0,NULL),
(3062,NULL,'MN. Tx de Frecuencia No Disponible','Tx de Frecuencia No Disponible. {1}',1,NULL),
(3063,NULL,'MN. Tx de Frecuencia No Disponible. Baja Prioridad','Tx de Frecuencia No Disponible por Prioridad. {1}',1,NULL),
(3064,NULL,'MN. Rx de Frecuencia en Equipo PPAL','Rx de Frecuencia en Equipo PPAL. {1}',0,NULL),
(3065,NULL,'MN. Rx de Frecuencia en Equipo RSVA','Rx de Frecuencia en Equipo RSVA. {1}',0,NULL),
(3066,NULL,'MN. Rx de Frecuencia No Disponible','Rx de Frecuencia No Disponible. {1}',1,NULL),
(3067,NULL,'MN. Rx de Frecuencia No Disponible. Baja Prioridad','Rx de Frecuencia No Disponible por Prioridad. {1}',1,NULL),
(3070,NULL,'MN. Operacion Manual','Operacion Manual. {1}',0,NULL),
(3071,NULL,'MN. Error en Operacion Manual','Error en Operacion Manual. {1}',1,NULL),
(3080,NULL,'MN. Informacion','{1}',0,NULL),
(3081,NULL,'MN. Error Generico','Error. {1}',1,NULL),
(3082,NULL,'MN. Error de Configuracion','Error de Configuracion: {1}',1,NULL),
(5000,NULL,'Contador de tiempo operativo','{0}',0,NULL),
(5001,NULL,'Elemento entra en estado operativo',NULL,0,NULL),
(5002,NULL,'Elemento sale de estado operativo',NULL,0,NULL),
(5003,NULL,'Contador de tiempo operativo','{0}',0,NULL),
(5004,NULL,'Elemento entra en estado de fallo',NULL,0,NULL),
(5005,NULL,'Elemento sale de estado de fallo',NULL,0,NULL);
/*!40000 ALTER TABLE `incidencias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `incidencias_frances`
--

DROP TABLE IF EXISTS `incidencias_frances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `incidencias_frances` (
  `IdIncidencia` int(10) unsigned NOT NULL,
  `IdIncidenciaCorrectora` int(10) unsigned DEFAULT NULL,
  `Incidencia` varchar(180) DEFAULT NULL,
  `Descripcion` varchar(180) DEFAULT NULL,
  `GeneraError` tinyint(1) DEFAULT NULL,
  `OID` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdIncidencia`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incidencias_frances`
--

LOCK TABLES `incidencias_frances` WRITE;
/*!40000 ALTER TABLE `incidencias_frances` DISABLE KEYS */;
INSERT INTO `incidencias_frances` VALUES (1,NULL,'Équipement HF branché','Équipement HF {0} branché',NULL,NULL),
(2,NULL,'Erreur à l\\\'équipement HF','Erreur à l\\\'équipement HF {0}',NULL,NULL),
(3,NULL,'Équipement HF débranché','Équipement HF {0} débranché.',NULL,NULL),
(4,NULL,'Équipement HF livré','Équipement HF livré. Operateur: {0}. Équipement: {1}, Fréquence: {2}.',NULL,NULL),
(5,NULL,'Équipement HF non livré','Équipement HF non livré.  Équipement: {1}, Fréquence: {2}.',NULL,NULL),
(6,NULL,'Erreur HF général','Erreur HF général',NULL,NULL),
(7,NULL,'Erreur HF de livraison','Erreur HF de livraison. Operateur: {0}, Équipement: {1}, Fréquence: {2}.',NULL,NULL),
(8,NULL,'Erreur HF de non livraison','Erreur HF de non livraison: Équipement: {1}, Fréquence: {2}.',NULL,NULL),
(9,NULL,'Erreur HF de livraison multiple','Erreur HF de livraison multiple. Operateur {0}.',NULL,NULL),
(10,NULL,'Erreur HF préparation SELCAL','Erreur HF préparation SELCAL. Operateur: {0}, Équipement: {1}, OnOff: {2}.',NULL,NULL),
(50,NULL,'Information de service','Information de service {0}: {1} {2} {3} {4}',0,NULL),
(51,NULL,'Erreur dans le service','Erreur dans le serviceo {0}: {1} {2} {3} {4}',1,NULL),
(96,NULL,'Changement de la journée.','Changement de la journée',NULL,NULL),
(101,NULL,'Sélection SCV.','SCV sélectionné {0}.',NULL,NULL),
(105,NULL,'Chargement sectorisation.','Chargement de la sectorisation {0}',NULL,'.1.1.600.1'),
(106,NULL,'Erreur de chargement sectorisation.','Erreur de chargement sectorisation {0}',0,'.1.1.600.1'),
(108,NULL,'Sectorisation rejetée ». Tous les secteurs ne sont pas réels.','Rejeter la sectorisation {0}. Ils ne sont pas tous les secteurs réels',0,'.1.1.600.1'),
(109,NULL,'Sectorisation implantée automatique.','Sectorisation automatique implantée',NULL,'.1.1.600.1'),
(110,NULL,'Sectorisation automatique rejetée.','Sectorisation automatique rejetée',0,'.1.1.600.1'),
(111,NULL,'Secteur livré à la position.','Secteur {0} livré à la position {1}',NULL,'.1.1.600.1'),
(112,NULL,'Secteur non livré à la position.','Secteur {0} désallouée de la position {1}',NULL,'.1.1.600.1'),
(113,NULL,'Sectorisation rejetée. 1 + 1 n\\\'est pas active.','Rejeter la sectorisation. 1 + 1 n\\\'est pas active.',NULL,'.1.1.600.1'),
(114,NULL,'Configuration','{0}',0,NULL),
(201,NULL,'Serveur actif','{0}',0,NULL),
(202,201,'Serveur en réserve','{0}',0,NULL),
(203,NULL,'Serveur tombé','{0}',0,NULL),
(300,NULL,'Gestion de NBX','{0} {1} {2} {3}',0,NULL),
(301,NULL,'Alarme NBX','{0} {1} {2} {3}',1,NULL),
(1001,NULL,'Entrée TOP.','Entrée TOP',1,'1.1.1000.0'),
(1002,1001,'TOP coupée.','Top coupée',1,'1.1.1000.0'),
(1003,NULL,'Liaison jacks exécutif.','Liaison jacks exécutif Post: {0}',0,'1.1.1000.1.3.0'),
(1004,1003,'Débranchement jacks exécutif.','Débranchement jacks exécutif. Post: {0}',0,'1.1.1000.1.3.0'),
(1005,NULL,'Liaison jacks assistant','Liaison jacks assistant. Post: {0}',0,'1.1.1000.1.3.1'),
(1006,1005,'Débranchement jacks assistant.','Débranchement jacks assistant. Post: {0}',0,'1.1.1000.1.3.1'),
(1007,NULL,'Liaison haut-parleur','Liaison haut-parleur {0}. Post: {1}',1,'1.1.1000.1.2'),
(1008,1007,'Débranchement haut-parleur.','Débranchement haut-parleur {0}. Post: {1}',1,'1.1.1000.1.2 '),
(1009,NULL,'Panneau en marche','Panneau en marche. Post: {0}',1,'1.1.1000.1.4 '),
(1010,1009,'Panneau passe à stand-by.','Panneau passe à stand-by. Post: {0}',1,'1.1.1000.1.4 '),
(1011,NULL,'Page de fréquences est sélectionnée.','Page {0} de fréquences est sélectionnée. Post:. {1}',NULL,'1.1.1000.6'),
(1014,NULL,'État de PTT.','Etat de PTT: {0}. Post: {1}. Secteur {2}',NULL,'1.1.1000.2'),
(1015,NULL,'Facilité sélectionnée','Facilité sélectionnée: {0}. Post:. {1}',NULL,'1.1.1000.8'),
(1016,NULL,'Appel entrant.','Appel entrant. Accès: {0}. Post:. {1}',NULL,'1.1.1000.9'),
(1017,NULL,'Appel sortant Post.','Appel sortant. Accès: {0}. Post:. {1}',NULL,'1.1.1000.7'),
(1019,NULL,'Fin d\\\'appel Post.','Fin d\\\'appel. Accès: {0}. Post:. {1}',NULL,'1.1.1000.10 '),
(1020,NULL,'Appel établi','Appel établi. Accès: {0}. Post:. {1}',NULL,'1.1.1000.11'),
(1021,NULL,'Séance Breifing','Séance Breifing à le poste: {0}. {1}',NULL,'1.1.1000.13'),
(1022,NULL,'Fonction de reproduction.','Fonction de reproduction post: {0}. {1}',NULL,'1.1.1000.12'),
(1023,NULL,'Câble d\'enregistrement connecté','Câble d\'enregistrement connecté',0,NULL),
(1024,NULL,'Câble d\'enregistrement déconnecté','Câble d\'enregistrement déconnecté',1,NULL),
(1025,NULL,'Événement réseau local','État du réseau local. LAN1: {0}, LAN2: {1}',1,NULL),
(2001, NULL, 'Modulo passerelle en service', '{0} En service', 1, '1.1.100.2.0'),
(2002, 2001, 'Modulo passerelle hors service', '{0} Hors service', 1, '1.1.100.2.0'),
(2003, NULL, 'Ressource radio en service', '{0}. Ressource radio en service', NULL, '1.1.200.3.1.17'),
(2004, 2003, 'Ressource radio hors service', '{0}. Ressource radio hors service', 0, '1.1.200.3.1.17'),
(2005, NULL, 'Ressource téléphonie en service', '{0}. Ressource téléphonie en service', NULL, '1.1.400.3.1.14'),
(2006, 2005, 'Ressources téléphonie hors service', '{0}. Ressource téléphonie hors service', 0, '1.1.400.3.1.14'),
(2007, NULL, 'Carte d\\\'interface en service', 'Carte d\\\'interface: {0} en service', 1, '1.1.100.31.1.2'),
(2008, 2007, 'Carte d\\\'interface hors service', 'Carte d\\\'interface: {0} hors service', 1, '1.1.100.31.1.2'),
(2009, NULL, 'Ressource R2 en service', '{0}. Ressource R2 en service', NULL, '1.1.500.3.1.17'),
(2010, NULL, 'Ressource R2 hors service', '{0}. Ressource R2 hors service', NULL, '1.1.500.3.1.17'),
(2012, NULL, 'Erreur de protocole LCN.', '{0}. Erreur de protocole LCN.', NULL, '1.1.300.3.1.17'),
(2013, NULL, 'Ressource LCN en service', '{0}. Ressource LCN en service', NULL, '1.1.300.3.1.17'),
(2014, 2013, 'Ressource LCN hors service', '{0}. Ressource LCN hors service', 0, '1.1.300.3.1.17'),
(2015, NULL, 'Ressource N5 en service', '{0}. Ressource N5 en service', 0, NULL),
(2016, NULL, 'Ressource N5 hors service', '{0}. Ressource N5 hors service', 0, NULL),
(2017, NULL, 'Ressource QSIG en service', '{0}. Ressource QSIG en service', 0, NULL),
(2018, NULL, 'Débranchement ressource QSIG', '{0}. Débranchement ressource QSIG', 0, NULL),
(2020,NULL,'Appel R2 entrant.','Appel R2 entrant. Ressource {0}. Tronc {1}. {2} Origine. Priorité {3}. Destination {4} ',NULL,'1.1.500.3.1.17 '),
(2021,NULL,'Fin appel R2.','Fin appel R2. Ressource {0}. Tronc {1}.',NULL,'1.1.500.3.1.17 '),
(2022,NULL,'Appel R2 sortant.','Appel R2 sortant. Ressource {0}. Tronc {1}. {2} Origine. Priorité {3}. Destination {4}',NULL,'1.1.500.3.1.17 '),
(2023,NULL,'Fin appel R2.','Fin appel R2. Ressource {0}. Tronc {1}.',NULL,'1.1.500.3.1.17 '),
(2024,NULL,'Appel de test R2.','Appel de test R2. Passerelle: {0}. Ressource: {1}',NULL,'1.1.500.3.1.17 '),
(2025,NULL,'Erreur de protocole R2.','Erreur de protocole R2. Passerelle: {0}. Ressource: {1}',0,'1.1.500.3.1.17 '),
(2030,NULL,'Appel LCN entrant.','Appel LCN entrant: Ressource: {0}',NULL,'1.1.300.3.1.17'),
(2031,NULL,'Fin d\\\'appel entrant LCN.','Fin d\\\'appel entrant LCN: Ressource: {0}.',NULL,'1.1.300.3.1.17'),
(2032,NULL,'Appel LCN sortant.','Appel LCN sortant: Ressource:. {0}',NULL,'1.1.300.3.1.17'),
(2033,NULL,'Fin appel sortant LCN.','Fin d\\\'appel sortant LCN: Ressource:. {0}',NULL,'1.1.300.3.1.17'),
(2040,NULL,'Appel téléphonique entrant.','Appel téléphonique entrant: Ressource {0}. Ligne {1}.  Réseau {2}. Type d\\\'interface {3}. Accès {4}.',NULL,'1.1.400.3.1.14'),
(2041,NULL,'Fin appel téléphonique entrant.','Fin appel téléphonique entrant: Ressource {0}. Ligne {1}.  Réseau {2}. Type d\\\'interface {3}. Accès {4}.',NULL,'1.1.400.3.1.14'),
(2042,NULL,'Appel téléphonique sortant.','Appel téléphonique sortant: Ressource {0}. Ligne {1}.  Réseau {2}. Type d\\\'interface {3}. Accès {4}.',NULL,'1.1.400.3.1.14'),
(2043,NULL,'Fin appel téléphonique sortant.','Fin appel téléphonique sortant: Ressource {0}. Ligne {1}.  Réseau {2}. Type d\\\'interface {3}. Accès {4}.',NULL,'1.1.400.3.1.14'),
(2050,NULL,'PTT On.','PTT On: Passerelle: {0}. Ressource {1}. Fréquence: {2}',NULL,'1.1.200.3.1.17'),
(2051,NULL,'PTT Off.','PTT Off: Passerelle: {0}. Ressource {1}. Fréquence: {2}',NULL,'1.1.200.3.1.17'),
(2052,NULL,'SQ On.','SQ On: Passerelle: {0}. Ressource {1}. Fréquence: {2}',NULL,'1.1.200.3.1.17'),
(2053,NULL,'SQ Off.','SQ Off: Passerelle: {0}. Ressource {1}. Fréquence: {2} ',NULL,'1.1.200.3.1.17'),
(2100,NULL,'Sélection Principal/Réserve','Passerelle {0} passe a  l\\\'état de {1}',NULL,'1.1.100.21.0'),
(2200,NULL,'GW Event','{0}: {1} {2} {3}',0,NULL),
(2300,NULL,'GW Operation','{0}: {1} {2} {3}',0,NULL),
(3001,NULL,'Liaison équipement externe.','Liaison équipement externe',1,NULL),
(3002,3001,'Débranchement équipement externe.','Débranchement équipement externe',1,NULL),
(3003,NULL,'Abonné PBX Active', 'Abonné PBX Active', 0,NULL),
(3004,NULL,'Abonné PBX Non active', 'Abonné PBX Non active', 0,NULL),
(3050,NULL,'MN. Unité disponible','Unité disponible. {1}',0,NULL),
(3051,NULL,'MN. Unité en échec','Unité en échec. {1}',1,NULL),
(3052,NULL,'MN. Erreur de communication dans l\'unité','Erreur de communication dans l\'unité. {1}',1,NULL),
(3060,NULL,'MN. Fréquence TX dans l\'unité principale','Fréquence TX dans l\'unité principale. {1}',0,NULL),
(3061,NULL,'MN. Fréquence TX en unité de réserve','Fréquence TX en unité de réserve. {1}',0,NULL),
(3062,NULL,'MN. Fréquence TX non disponible','Fréquence TX non disponible. {1}',1,NULL),
(3063,NULL,'MN. Fréquence TX non disponible. Priorité basse','Fréquence TX non disponible. Priorité basse. {1}',1,NULL),
(3064,NULL,'MN. Fréquence RX dans l\'unité principale','Fréquence RX dans l\'unité principaleL. {1}',0,NULL),
(3065,NULL,'MN. Fréquence RX en unité de réserve','Fréquence RX en unité de réserve. {1}',0,NULL),
(3066,NULL,'MN. Fréquence RX non disponible','Fréquence R X non disponible. {1}',1,NULL),
(3067,NULL,'MN. Fréquence RX non disponible. Priorité basse','Fréquence RX non disponible. Priorité basse. {1}',1,NULL),
(3070,NULL,'MN. Opération manuelle','Opération manuellel. {1}',0,NULL),
(3071,NULL,'MN. Erreur d\'opération manuelle','Erreur d\'opération manuellel. {1}',1,NULL),
(3080,NULL,'MN. Information','{1}',0,NULL),
(3081,NULL,'MN. Erreur générique','Erreur. {1}',1,NULL),
(3082,NULL,'MN. Erreur de configuration','Erreur de configuration: {1}',1,NULL),
(5000,NULL,'Contador de tiempo operativo','{0}',0,NULL),
(5001,NULL,'Elemento entra en estado operativo',NULL,0,NULL),
(5002,NULL,'Elemento sale de estado operativo',NULL,0,NULL),
(5003,NULL,'Contador de tiempo operativo','{0}',0,NULL),
(5004,NULL,'Elemento entra en estado de fallo',NULL,0,NULL),
(5005,NULL,'Elemento sale de estado de fallo',NULL,0,NULL);
/*!40000 ALTER TABLE `incidencias_frances` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `incidencias_ingles`
--

DROP TABLE IF EXISTS `incidencias_ingles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `incidencias_ingles` (
  `IdIncidencia` int(10) unsigned NOT NULL,
  `IdIncidenciaCorrectora` int(10) unsigned DEFAULT NULL,
  `Incidencia` varchar(180) DEFAULT NULL,
  `Descripcion` varchar(180) DEFAULT NULL,
  `GeneraError` tinyint(1) DEFAULT NULL,
  `OID` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdIncidencia`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incidencias_ingles`
--

LOCK TABLES `incidencias_ingles` WRITE;
/*!40000 ALTER TABLE `incidencias_ingles` DISABLE KEYS */;
INSERT INTO `incidencias_ingles` (`IdIncidencia`, `IdIncidenciaCorrectora`, `Incidencia`, `Descripcion`, `GeneraError`, `OID`) VALUES 
(1,NULL,'HF Equipment on','HF Equipment {0} on',NULL,NULL),
(2,NULL,'Error in HF equipment','Error in HF equipment {0}',NULL,NULL),
(3,NULL,'HF equipment off','HF equipment {0} off',NULL,NULL),
(4,NULL,'HF equipment assigned','HF equipment assigned. User: {0}. Equipment: {1}, Frequency: {2}.',NULL,NULL),
(5,NULL,'HF equipment unassigned','HF equipment unassigned.  Equipment: {1}, Frequency: {2}.',NULL,NULL),
(6,NULL,'HF equipment error','HF equipment error',NULL,NULL),
(7,NULL,'HF assigning error','HF assigning error. User: {0}, Equipment: {1}, Frequency: {2}.',NULL,NULL),
(8,NULL,'HF unassigning error','HF unassigning error: Equipment: {1}, Frequency: {2}.',NULL,NULL),
(9,NULL,'HF multiple assigning error','HF multiple assigning error. User {0}.',NULL,NULL),
(10,NULL,'SELCAL preparation error','SELCAL preparation error. User: {0}, Equipment: {1}, OnOff: {2}.',NULL,NULL),
(50,NULL,'Service information','Service information {0}: {1} {2} {3} {4}',0,NULL),
(51,NULL,'Service ERROR','Error on Service {0}: {1} {2} {3} {4}',1,NULL),
(96,NULL,'Change of day.','Change of day',NULL,NULL),
(101,NULL,'VCS Selection.','Selectioned VCS {0}.',NULL,NULL),
(105,NULL,'Sectorization load','Sectorization load {0}',NULL,'.1.1.600.1'),
(106,NULL,'Error in sectorization load.','Error sectorization load {0}',1,'.1.1.600.1'),
(108,NULL,'Rejection in sectorization. All real sectors are not in sectorization.','Reject in sectorization {0}. All real sectors are not in sectorization',1,'.1.1.600.1'),
(109,NULL,'Loaded automatic sectorization.','Loaded automatic sectorization',NULL,'.1.1.600.1'),
(110,NULL,'Rejected automatic sectorization.','Rejected automatic sectorization',1,'.1.1.600.1'),
(111,NULL,'Position assigned to the sector.','Position {1} assigned to the sector {0}',NULL,'.1.1.600.1'),
(112,NULL,'Position unassigned to the sector.','Position {1} unassigned to the sector {0}',NULL,'.1.1.600.1'),
(113,NULL,'Rejection in sectorización. 1 + 1 is not active.','Rejection in sectorization. 1 + 1 is not active.',NULL,'.1.1.600.1'),
(114,NULL,'Configuration','{0}',0,NULL),
(201,NULL,'Active Server.','{0}',0,NULL),
(202,201,'Standby server.','{0}',0,NULL),
(203,NULL,'Fallen Server.','{0}',0,NULL),
(300,NULL,'NBX Management','{0} {1} {2} {3}',0,NULL),
(301,NULL,'NBX Alarm','{0} {1} {2} {3}',1,NULL),
(1001,NULL,'Entering OT.','Entering OT. Position: {0}',NULL,'1.1.1000.0'),
(1002,1001,'OT down.','OT down. Position: {0}',1,'1.1.1000.0'),
(1003,NULL,'Executive jacks on.','Executive jacks on. Position: {0}',NULL,'1.1.1000.1.3.0'),
(1004,1003,'Executive jacks off.','Executive jacks off. Position: {0}',1,'1.1.1000.1.3.0'),
(1005,NULL,'Assistant jacks on.','Assistant jacks on. Position: {0}',NULL,'1.1.1000.1.3.1'),
(1006,1005,'Assistant jacks off.','Assistant jacks off. Position: {0}',1,'1.1.1000.1.3.1'),
(1007,NULL,'Speaker on.','Speaker on {0}. Position: {1}',1,'1.1.1000.1.2'),
(1008,1007,'Speaker off','Speaker off. Position: {0}',0,'1.1.1000.1.2'),
(1009,NULL,'Panel to operation','Panel to operation. Position: {0}',-1,'1.1.1000.1.4'),
(1010,1009,'Panel to standby.','Panel to standby. Position: {0}',1,'1.1.1000.1.4'),
(1011,NULL,'Frequency page selected','Frequency page selected: {0}. Position: {1}. Sector: {2}',NULL,'1.1.1000.6'),
(1014,NULL,'PTT status.','PTT status: {0}. Position:{1}. PTT Type: {2}',NULL,'1.1.1000.2'),
(1015,NULL,'Selected function','Selected function: {0}. Position: {1}. Sector: {2}',NULL,'1.1.1000.8'),
(1016,NULL,'Incomming call Position.','Incomming calll. Access: {0}. Position: {1}.',NULL,'1.1.1000.9'),
(1017,NULL,'Outgoing call Position.','Outgoing calll. Access: {0}. Position: {1}.',NULL,'1.1.1000.7'),
(1019,NULL,'Ending  call Position.','Ending call. Access: {0}. Position: {1}.',NULL,'1.1.1000.10'),
(1020,NULL,'Established telephone call','Established call.  Access: {0}. Position: {1}.',NULL,NULL),
(1021,NULL,'Briefing function',' Briefing session in : {0} {1}',NULL,'1.1.1000.13'),
(1022,NULL,'Playing function','Local playing breafing in {0} {1}',NULL,'1.1.1000.12'),
(1023,NULL,'Recording cable connected','Recording cable connected',0,NULL),
(1024,NULL,'Recording cable disconnected','Recording cable disconnected',1,NULL),
(1025,NULL,'LAN Event','Status: LAN1 {0}, LAN2 {1}',1,NULL),
(2001, NULL, 'Gateway module in service', '{0}. In service', 1, '1.1.100.2.0'),
(2002, 2001, 'Gateway module out of service', '{0}. Out of service', 1, '1.1.100.2.0'),
(2003, NULL, 'Radio resource on.', '{0}. Radio resource on.', 1, '1.1.200.3.1.17'),
(2004, 2003, 'Radio resource off.', '{0}. Radio resource off.', 1, '1.1.200.3.1.17'),
(2005, NULL, 'Telephone resource on.', '{0}. Telephone resource on.', 1, '1.1.400.3.1.14'),
(2006, 2005, 'Telephone resource off.', '{0}. Telephone resource off.', 1, '1.1.400.3.1.14'),
(2007, NULL, 'Interface on', 'Interface on: {0}', 1, '1.1.100.31.1.2'),
(2008, 2007, 'Interface off', 'Interface off: {0}', 1, '1.1.100.31.1.2'),
(2009, NULL, 'R2 resource on.', '{0}. R2 resource on.', 1, '1.1.500.3.1.17'),
(2010, NULL, 'R2 resource off.', '{0}. R2 resource off.', 1, '1.1.500.3.1.17'),
(2012, NULL, 'Protocol LCN error.', '{0}. Protocol LCN error', 0, '1.1.300.3.1.17'),
(2013, NULL, 'LCN resource on.', '{0}. LCN resource on.', 1, '1.1.300.3.1.17'),
(2014, 2013, 'LCN resource off.', '{0}. LCN resource off.', 1, '1.1.300.3.1.17'),
(2015, NULL, 'N5 Resource on', '{0}. N5 Resource on', 1, NULL),
(2016, NULL, 'N5. Resource off', '{0}. N5 Resource off', 1, NULL),
(2017, NULL, 'QSIG Resource on', '{0}. QSIG Resource on', 0, NULL),
(2018, NULL, 'QSIG Resource off', '{0}. QSIG Resource off', 0, NULL),
(2020,NULL,'Incomming call R2.','Incomming call R2. Resource {0}. Trunk {1}. Source {2}. Priority {3}. Target {4}',NULL,'1.1.500.3.1.17'),
(2021,NULL,'End of incomming call R2.','End of incomming call R2. Resource {0}. Trunk {1}. Source {2}. Priority {3}. Target {4}',NULL,'1.1.500.3.1.17'),
(2022,NULL,'Outgoing call R2.','Outgoing call R2.  Resource {0}. Trunk {1}. Source {2}. Priority {3}. Target {4}',NULL,'1.1.500.3.1.17'),
(2023,NULL,'End of outgoing call R2.','End of outgoing call R2. Resource {0}. Trunk {1}. Source {2}. Priority {3}. Target {4}',NULL,'1.1.500.3.1.17'),
(2024,NULL,'Test call  R2.','Test call R2',NULL,'1.1.500.3.1.17'),
(2025,NULL,'R2 protocol error.','R2 protocol error.',1,'1.1.500.3.1.17'),
(2030,NULL,'Incomming call LCN.','Incomming call LCN: Resource: {0}. Line: {1}',NULL,'1.1.300.3.1.17'),
(2031,NULL,'End of incomming call LCN.','End of incomming call LCN: Resource: {0}. Line: {1}',NULL,'1.1.300.3.1.17'),
(2032,NULL,'Outgoing call LCN.','Outgoing call LCN: Resource: {0}. Line: {1}.',NULL,'1.1.300.3.1.17'),
(2033,NULL,'End of outgoing call LCN.','End of outgoing call LCN: Resource: {0}. Line: {1}.',NULL,'1.1.300.3.1.17'),
(2040,NULL,'Telephony incomming call.','Telephony incomming call.: Resource {0}. Line {1}. Network {2}. Interfaz {3}. Access {4}.',NULL,'1.1.400.3.1.14'),
(2041,NULL,'End of telephony incomming call.','End of telephony incomming call: Resource {0}. Line {1}. Network {2}. Interfaz {3}. Access {4}.',NULL,'1.1.400.3.1.14'),
(2042,NULL,'Telephony outgoing call.','Telephony outgoing call:  Resource {0}. Line {1}. Network {2}. Interfaz {3}. Access {4}.',NULL,'1.1.400.3.1.14'),
(2043,NULL,'End of telephony outgoing call.','End of telephony outgoing call: Resource {0}. Line {1}. Network {2}. Interfaz {3}. Access {4}.',NULL,'1.1.400.3.1.14'),
(2050,NULL,'PTT On.','PTT On: Resource {0}. Frequency: {1}',NULL,'1.1.200.3.1.17'),
(2051,NULL,'PTT Off.','PTT Off: Resource {0}. Frequency: {1}',NULL,'1.1.200.3.1.17'),
(2052,NULL,'SQ On.','SQ On: Resource {0}. Frequency: {1}',NULL,'1.1.200.3.1.17'),
(2053,NULL,'SQ Off.','SQ Off: Resource {0}. Frequency: {1}',NULL,'1.1.200.3.1.17'),
(2100,NULL,'Selection Main/Standby','The gateway {0} switchs to {1}.',NULL,'1.1.100.21.0'),
(2200,NULL,'GW Event','{0}: {1} {2} {3}',0,NULL),
(2300,NULL,'GW Manual Operation','{0}: {1} {2} {3}',0,NULL),
(3001,NULL,'Entering external equipment','Entering external equipment',NULL,NULL),
(3002,3001,'External equipment down','External equipment down',1,NULL),
(3003,NULL,'Pbx Subscriber ON', 'Pbx Subscriber ON', 0,NULL),
(3004,NULL,'Pbx Subscriber OFF', 'Pbx Subscriber OFF', 0,NULL),
(3050,NULL,'MN. Equipment available ','Equipment available. {1}',0,NULL),
(3051,NULL,'MN. Equipment in failure','Equipment in failure. {1}',1,NULL),
(3052,NULL,'MN. Error in Communication with Equipment','Error in Communication with Equipment. {1}',1,NULL),
(3060,NULL,'MN. TX frequency in main equipment','Tx  frequency in main equipment. {1}',0,NULL),
(3061,NULL,'MN. TX frequency in reserve equipment','Tx  frequency in reserve equipment. {1}',0,NULL),
(3062,NULL,'MN. TX frequency not available','Tx de Frecuencia No Disponible. {1}',1,NULL),
(3063,NULL,'MN. TX frequency not available. Low priority','Tx frequency not available by priority. {1}',1,NULL),
(3064,NULL,'MN. RX frequency in main equipment','Rx  frequency in main equipment. {1}',0,NULL),
(3065,NULL,'MN. RX frequency in reserve equipment','Rx  frequency in reserve equipment. {1}',0,NULL),
(3066,NULL,'MN. RX  frequency not available','Rx  frequency not available. {1}',1,NULL),
(3067,NULL,'MN. RX frequency not available. Low priority','Rx  frequency not available by priority. {1}',1,NULL),
(3070,NULL,'MN. Manual Operation','Manual Operation. {1}',0,NULL),
(3071,NULL,'MN. Error in manual operation','Error in manual operation. {1}',1,NULL),
(3080,NULL,'MN. Information','{1}',0,NULL),
(3081,NULL,'MN. Generic error','Error. {1}',1,NULL),
(3082,NULL,'MN. Configuration error','Configuration error: {1}',1,NULL),
(5000,NULL,'Contador de tiempo operativo','{0}',0,NULL),
(5001,NULL,'Elemento entra en estado operativo',NULL,0,NULL),
(5002,NULL,'Elemento sale de estado operativo',NULL,0,NULL),
(5003,NULL,'Contador de tiempo operativo','{0}',0,NULL),
(5004,NULL,'Elemento entra en estado de fallo',NULL,0,NULL),
(5005,NULL,'Elemento sale de estado de fallo',NULL,0,NULL);
/*!40000 ALTER TABLE `incidencias_ingles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `metodos_bss`
--

DROP TABLE IF EXISTS `metodos_bss`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `metodos_bss` (
  `idmetodos_bss` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idmetodos_bss`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `metodos_bss`
--

LOCK TABLES `metodos_bss` WRITE;
/*!40000 ALTER TABLE `metodos_bss` DISABLE KEYS */;
INSERT INTO `metodos_bss` VALUES (0,'NUCLEO'),(1,'RSSI');
/*!40000 ALTER TABLE `metodos_bss` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-03-21 11:28:02
