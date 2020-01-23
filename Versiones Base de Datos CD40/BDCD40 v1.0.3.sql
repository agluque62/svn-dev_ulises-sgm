-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.0.45-community-nt-log


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


--
-- Create schema cd40
--

CREATE DATABASE IF NOT EXISTS cd40;
USE cd40;

--
-- Definition of table `externos`
--

DROP TABLE IF EXISTS `externos`;
CREATE TABLE `externos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `PosHMI` int(10) unsigned default NULL,
  `Prioridad` int(10) unsigned default NULL,
  `OrigenR2` varchar(32) default NULL,
  `PrioridadSIP` int(10) unsigned default NULL,
  `TipoAcceso` varchar(2) NOT NULL,
  `Literal` varchar(32) default NULL,
  PRIMARY KEY  (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`IdDestino`,`IdPrefijo`),
  KEY `Table_41_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`),
  CONSTRAINT `externos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) REFERENCES `sectoressectorizacion` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `externos`
--

/*!40000 ALTER TABLE `externos` DISABLE KEYS */;
INSERT INTO `externos` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`IdDestino`,`IdPrefijo`,`PosHMI`,`Prioridad`,`OrigenR2`,`PrioridadSIP`,`TipoAcceso`,`Literal`) VALUES 
 ('departamento','19/07/2012 09:58:33','DESA','111','315004',3,9,1,'314011',4,'DA','315004'),
 ('departamento','19/07/2012 09:58:33','DESA','111','316001',3,4,1,'314011',4,'DA','316001'),
 ('departamento','19/07/2012 09:58:33','DESA','111','BL_1',32,1,1,'111',4,'DA','BL_1'),
 ('departamento','19/07/2012 09:58:33','DESA','111','EN BLANCO',3,7,1,'314011',4,'DA','EN BLANCO'),
 ('departamento','19/07/2012 09:58:33','DESA','111','nada',4,1,0,'111',0,'AG','nada'),
 ('departamento','19/07/2012 09:58:33','DESA','314008','315004',3,1,1,'314008',2,'DA','315004'),
 ('departamento','19/07/2012 09:58:33','DESA','314008','LC_21',1,10,1,'314008',4,'IA','LC_21'),
 ('departamento','19/07/2012 09:58:33','DESA','314082','315004',3,5,1,'314082',4,'DA','315004'),
 ('departamento','19/07/2012 09:58:33','DESA','314082','316001',3,7,1,'314082',4,'DA','316001'),
 ('departamento','19/07/2012 09:58:33','DESA','314082','BC_1',32,13,1,'314082',4,'DA','BC_1'),
 ('departamento','19/07/2012 09:58:33','DESA','314082','BL_1',32,4,1,'314082',4,'DA','BL_1'),
 ('departamento','19/07/2012 09:58:33','DESA','314082','BL_2',32,8,1,'314082',4,'DA','BL_2'),
 ('departamento','19/07/2012 09:58:33','DESA','314082','LC_22',1,11,1,'314082',4,'IA','LC_22'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','315004',3,9,1,'314020',4,'DA','315004'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','316001',3,12,1,'314020',4,'DA','316001'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','BC_1',32,2,1,'JUCAR',4,'DA','BC_1'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','BL_1',32,3,1,'JUCAR',4,'DA','BL_1'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','BL_2',32,6,1,'JUCAR',4,'DA','BL_2'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','micasa',4,2,0,'JUCAR',0,'AG','micasa'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','movil',4,3,0,'JUCAR',0,'AG','movil'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','otro',4,4,0,'JUCAR',0,'AG','otro'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_41','BL_1',32,5,1,'POS_41',4,'DA','BL_1'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_41','DEST_LC_2',1,10,1,'POS_41',4,'IA','DEST_LC_2'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_69','DEST_LC_1',1,3,1,'POS_69',4,'IA','DEST_LC_1'),
 ('departamento','S1','DESA','111','315004',3,9,1,'314011',4,'DA','315004'),
 ('departamento','S1','DESA','111','316001',3,4,1,'314011',4,'DA','316001'),
 ('departamento','S1','DESA','111','BL_1',32,1,1,'111',4,'DA','BL_1'),
 ('departamento','S1','DESA','111','EN BLANCO',3,7,1,'314011',4,'DA','EN BLANCO'),
 ('departamento','S1','DESA','111','nada',4,1,0,'111',0,'AG','nada'),
 ('departamento','S1','DESA','314008','315004',3,1,1,'314008',2,'DA','315004'),
 ('departamento','S1','DESA','314008','LC_21',1,10,1,'314008',4,'IA','LC_21'),
 ('departamento','S1','DESA','314082','315004',3,5,1,'314082',4,'DA','315004'),
 ('departamento','S1','DESA','314082','316001',3,7,1,'314082',4,'DA','316001'),
 ('departamento','S1','DESA','314082','BC_1',32,13,1,'314082',4,'DA','BC_1'),
 ('departamento','S1','DESA','314082','BL_1',32,4,1,'314082',4,'DA','BL_1'),
 ('departamento','S1','DESA','314082','BL_2',32,8,1,'314082',4,'DA','BL_2'),
 ('departamento','S1','DESA','314082','LC_22',1,11,1,'314082',4,'IA','LC_22'),
 ('departamento','S1','DESA','JUCAR','315004',3,9,1,'314020',4,'DA','315004'),
 ('departamento','S1','DESA','JUCAR','316001',3,12,1,'314020',4,'DA','316001'),
 ('departamento','S1','DESA','JUCAR','BC_1',32,2,1,'JUCAR',4,'DA','BC_1'),
 ('departamento','S1','DESA','JUCAR','BL_1',32,3,1,'JUCAR',4,'DA','BL_1'),
 ('departamento','S1','DESA','JUCAR','BL_2',32,6,1,'JUCAR',4,'DA','BL_2'),
 ('departamento','S1','DESA','JUCAR','micasa',4,2,0,'JUCAR',0,'AG','micasa'),
 ('departamento','S1','DESA','JUCAR','movil',4,3,0,'JUCAR',0,'AG','movil'),
 ('departamento','S1','DESA','JUCAR','otro',4,4,0,'JUCAR',0,'AG','otro'),
 ('departamento','S1','DESA','POS_41','BL_1',32,5,1,'POS_41',4,'DA','BL_1'),
 ('departamento','S1','DESA','POS_41','DEST_LC_2',1,10,1,'POS_41',4,'IA','DEST_LC_2'),
 ('departamento','S1','DESA','POS_69','DEST_LC_1',1,3,1,'POS_69',4,'IA','DEST_LC_1');
/*!40000 ALTER TABLE `externos` ENABLE KEYS */;


--
-- Definition of table `incidencias`
--

DROP TABLE IF EXISTS `incidencias`;
CREATE TABLE `incidencias` (
  `IdIncidencia` int(10) unsigned NOT NULL,
  `IdIncidenciaCorrectora` int(10) unsigned default NULL,
  `Incidencia` varchar(180) default NULL,
  `Descripcion` varchar(180) default NULL,
  `GeneraError` tinyint(1) default NULL,
  `OID` varchar(32) default NULL,
  PRIMARY KEY  (`IdIncidencia`),
  KEY `Incidencias_FKIndex1` (`IdIncidenciaCorrectora`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `incidencias`
--

/*!40000 ALTER TABLE `incidencias` DISABLE KEYS */;
INSERT INTO `incidencias` (`IdIncidencia`,`IdIncidenciaCorrectora`,`Incidencia`,`Descripcion`,`GeneraError`,`OID`) VALUES 
 (96,NULL,'Cambio de día.','Cambio de día',NULL,NULL),
 (105,NULL,'Carga de sectorización.','Carga de sectorización {0}',NULL,NULL),
 (106,NULL,'Error carga sectorización.','Error carga sectorización {0}',1,NULL),
 (108,NULL,'Rechazo sectorización. No están todos los sectores reales.','Rechazo sectorización {0}. No están todos los sectores reales',1,NULL),
 (109,NULL,'Sectorización automática implantada.','Sectorización automática implantada',NULL,NULL),
 (110,NULL,'Sectorización automática rechazada.','Sectorización automática rechazada',1,NULL),
 (111,NULL,'Sector asignado a posición.','Sector {0} asignado a posición {1}',NULL,NULL),
 (112,NULL,'Sector desasignado de la posición.','Sector {0} desasignado de la posición {1}',NULL,NULL),
 (201,NULL,'Servidor 1 activo.','Servidor 1 activo',NULL,NULL),
 (202,201,'Servidor 1 caído.','Servidor 1 caído',1,NULL),
 (203,NULL,'Servidor 2 activo.','Servidor 2 activo',NULL,NULL),
 (204,203,'Servidor 2 caído.','Servidor 2 caído',1,NULL),
 (1001,NULL,'Entrada TOP.','Entrada TOP',NULL,'1.1.1000.0'),
 (1002,1001,'Caída TOP.','Caída TOP',1,'1.1.1000.0'),
 (1003,NULL,'Conexión jacks ejecutivo.','Conexión jacks ejecutivo. Posición: {0}',NULL,'1.1.1000.1.3.0'),
 (1004,1003,'Desconexión jacks ejecutivo.','Desconexión jacks ejecutivo. Posición: {0}',1,'1.1.1000.1.3.0'),
 (1005,NULL,'Conexión jacks ayudante.','Conexión jacks ayudante. Posición: {0}',NULL,'1.1.1000.1.3.1'),
 (1006,1005,'Desconexión jacks ayudante.','Desconexión jacks ayudante. Posición: {0} ',1,'1.1.1000.1.3.1'),
 (1007,NULL,'Conexión altavoz.','Conexión altavoz',NULL,'1.1.1000.1.2'),
 (1008,1007,'Desconexión altavoz.','Desconexión altavoz',1,'1.1.1000.1.2'),
 (1009,NULL,'Panel pasa a operación.','Panel pasa a operación',NULL,'1.1.6.0'),
 (1010,1009,'Panel pasa a standby.','Panel pasa a standby',1,'1.1.6.0'),
 (1011,NULL,'Página de frecuencias seleccionada.','Página de frecuencias seleccionada: {0}. Posición: {1}. Sector: {2}',NULL,NULL),
 (1012,NULL,'Selección recurso radio.','Selección recurso radio: {0}. Frecuencia: {1}. Posicion: {2}. Sector: {3}',NULL,NULL),
 (1013,NULL,'Estado selección.','Estado selección: {0}. Frecuencia: {1}. Posicion {2}. Sector: {3}',NULL,NULL),
 (1014,NULL,'Estado PTT.','Estado PTT: {0}. Posicion:{1}. Sector{2}',NULL,NULL),
 (1015,NULL,'Facilidad seleccionada.','Facilidad seleccionada: {0}. Posición: {1}. Sector: {2}',NULL,NULL),
 (1016,NULL,'Llamada entrante Posicion.','Llamada entrante Posicion: {0}. Acceso: {1}. Destino: {2}. Prioridad: {3}',NULL,NULL),
 (1017,NULL,'Llamada saliente Posicion.','Llamada saliente Posicion: {0}. Acceso: {1}. Destino: {2}. Prioridad: {3}',NULL,NULL),
 (1018,NULL,'Estado Recepción.','Estado Recepción: {0}. Posicion:{1}. Sector{2}',0,NULL),
 (1019,NULL,'Fin llamada saliente Posicion.','Fin llamada saliente Posicion: {0}. Acceso: {1}. Destino: {2}. Prioridad: {3}',NULL,NULL),
 (2001,NULL,'Entrada GW.','Entrada GW',NULL,'1.1.100.2.0'),
 (2002,2001,'Caída GW.','Caída GW.',1,'1.1.100.2.0'),
 (2003,NULL,'Conexión Recurso Radio.','Conexión Recurso Radio.',NULL,'1.1.200.3.1.17'),
 (2004,2003,'Desconexión Recurso Radio.','Desconexión Recurso Radio.',1,'1.1.200.3.1.17'),
 (2005,NULL,'Conexión Recurso Telefonía.','Conexión Recurso Telefonía.',NULL,'1.1.400.3.1.14'),
 (2006,2005,'Desconexión Recurso Telefonía.','Desconexión Recurso Telefonía.',1,'1.1.400.3.1.14'),
 (2009,NULL,'Conexión Recurso R2.','Conexión Recurso R2.',NULL,'1.1.500.3.1.17'),
 (2010,NULL,'Desconexión Recurso R2.','Desconexión Recurso R2.',NULL,'1.1.500.3.1.17'),
 (2012,NULL,'Error protocolo LCN.','Error protocolo LCN.',NULL,'1.1.300.3.1.17'),
 (2013,NULL,'Conexión Recurso LCN.','Conexión Recurso LCN.',NULL,'1.1.300.3.1.17'),
 (2014,2013,'Desconexión Recurso LCN.','Desconexión Recurso LCN.',1,'1.1.300.3.1.17'),
 (2020,NULL,'Llamada entrante R2.','Llamada entrante R2. Recurso {0}. Línea{1}.  Red {2}. Troncal {3}. Tipo de Interfaz {4}. Prioridad {5}. Ruta {6}. Acceso {7}',NULL,'1.1.500.3.1.17'),
 (2021,NULL,'Fin llamada entrante R2.','Fin llamada entrante R2. Recurso {0}. Línea{1}.  Red {2}. Troncal {3}. Tipo de Interfaz {4}. Prioridad {5}. Ruta {6}. Acceso {7}',NULL,'1.1.500.3.1.17'),
 (2022,NULL,'Llamada saliente R2.','Llamada saliente R2. Recurso {0}. Línea{1}.  Red {2}. Troncal {3}. Tipo de Interfaz {4}. Prioridad {5}. Ruta {6}. Acceso {7}',NULL,'1.1.500.3.1.17'),
 (2023,NULL,'Fin llamada saliente R2.','Fin llamada saliente R2. Recurso {0}. Línea{1}.  Red {2}. Troncal {3}. Tipo de Interfaz {4}. Prioridad {5}. Ruta {6}. Acceso {7}',NULL,'1.1.500.3.1.17'),
 (2024,NULL,'Llamada prueba R2.','Llamada prueba R2',NULL,'1.1.500.3.1.17'),
 (2025,NULL,'Error protocolo R2.','Error protocolo R2',1,'1.1.500.3.1.17'),
 (2030,NULL,'Llamada entrante LCN.','Llamada entrante LCN: Recurso: {0}. Línea: {1}',NULL,'1.1.300.3.1.17'),
 (2031,NULL,'Fin llamada entrante LCN.','Fin llamada entrante LCN: Recurso: {0}. Línea: {1}',NULL,'1.1.300.3.1.17'),
 (2032,NULL,'Llamada saliente LCN.','Llamada saliente LCN: Recurso: {0}. Línea: {1}.',NULL,'1.1.300.3.1.17'),
 (2033,NULL,'Fin llamada saliente LCN.','Fin llamada saliente LCN: Recurso: {0}. Línea: {1} Estado: {2}',NULL,'1.1.300.3.1.17'),
 (2040,NULL,'Llamada entrante telefonía.','Llamada entrante telefonía: Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2041,NULL,'Fin llamada entrante telefonía.','Fin llamada entrante telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2042,NULL,'Llamada saliente telefonía.','Llamada saliente telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2043,NULL,'Fin llamada saliente telefonía.','Fin llamada saliente telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2050,NULL,'PTT On.','PTT On:  Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (2051,NULL,'PTT Off.','PTT Off:   Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (2052,NULL,'SQ On.','SQ On:   Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (2053,NULL,'SQ Off.','SQ Off:   Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (3001,NULL,'Entrada Equipo Externo.','Entrada Equipo Externo',NULL,NULL),
 (3002,3001,'Caída Equipo Externo.','Caída Equipo Externo',1,NULL);
/*!40000 ALTER TABLE `incidencias` ENABLE KEYS */;


--
-- Definition of table `internos`
--

DROP TABLE IF EXISTS `internos`;
CREATE TABLE `internos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `PosHMI` int(10) unsigned default NULL,
  `Prioridad` int(10) unsigned default NULL,
  `OrigenR2` varchar(32) default NULL,
  `PrioridadSIP` int(10) unsigned default NULL,
  `TipoAcceso` varchar(2) NOT NULL,
  `Literal` varchar(32) default NULL,
  PRIMARY KEY  (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`IdDestino`,`IdPrefijo`),
  KEY `Table_40_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`),
  CONSTRAINT `internos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) REFERENCES `sectoressectorizacion` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `internos`
--

/*!40000 ALTER TABLE `internos` DISABLE KEYS */;
INSERT INTO `internos` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`IdDestino`,`IdPrefijo`,`PosHMI`,`Prioridad`,`OrigenR2`,`PrioridadSIP`,`TipoAcceso`,`Literal`) VALUES 
 ('departamento','19/07/2012 09:58:33','DESA','111','JUCAR',0,10,1,'111',4,'IA','JUCAR'),
 ('departamento','19/07/2012 09:58:33','DESA','111','JUCAR',2,2,1,'111',4,'DA','JUCAR'),
 ('departamento','19/07/2012 09:58:33','DESA','111','POS_69',0,2,1,'111',4,'IA','POS_69'),
 ('departamento','19/07/2012 09:58:33','DESA','111','POS_69',2,13,1,'111',4,'DA','POS_69'),
 ('departamento','19/07/2012 09:58:33','DESA','314001','314082',2,14,1,'314001',1,'DA','314082'),
 ('departamento','19/07/2012 09:58:33','DESA','314001','JUCAR',2,1,1,'314001',4,'DA','JUCAR'),
 ('departamento','19/07/2012 09:58:33','DESA','314001','POS_69',2,3,1,'314001',3,'DA','POS_69'),
 ('departamento','19/07/2012 09:58:33','DESA','314008','POS_69',2,2,1,'314008',4,'DA','POS_69'),
 ('departamento','19/07/2012 09:58:33','DESA','314051','314082',2,2,1,'314051',4,'DA','314082'),
 ('departamento','19/07/2012 09:58:33','DESA','314051','POS_41',2,1,1,'314051',4,'DA','POS_41'),
 ('departamento','19/07/2012 09:58:33','DESA','314082','314001',2,14,1,'314082',1,'DA','314001'),
 ('departamento','19/07/2012 09:58:33','DESA','314082','314051',2,1,1,'314082',4,'DA','314051'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','111',0,10,1,'JUCAR',4,'IA','111'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','111',2,4,1,'JUCAR',4,'DA','111'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','314001',2,1,1,'JUCAR',4,'DA','314001'),
 ('departamento','19/07/2012 09:58:33','DESA','JUCAR','POS_41',2,5,1,'JUCAR',4,'DA','POS_41'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_41','314051',2,1,1,'POS_41',4,'DA','314051'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_41','JUCAR',2,4,1,'POS_41',4,'DA','JUCAR'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_41','POS_69',0,1,1,'POS_41',4,'IA','POS_69'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_41','POS_69',2,3,1,'POS_41',4,'DA','POS_69'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_69','111',0,2,1,'POS_69',4,'IA','111'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_69','111',2,13,1,'POS_69',4,'DA','111'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_69','314001',2,3,1,'POS_69',3,'DA','314001'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_69','314008',2,2,1,'POS_69',4,'DA','314008'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_69','POS_41',0,1,1,'POS_69',4,'IA','POS_41'),
 ('departamento','19/07/2012 09:58:33','DESA','POS_69','POS_41',2,1,1,'POS_69',4,'DA','POS_41'),
 ('departamento','S1','DESA','111','JUCAR',0,10,1,'111',4,'IA','JUCAR'),
 ('departamento','S1','DESA','111','JUCAR',2,2,1,'111',4,'DA','JUCAR'),
 ('departamento','S1','DESA','111','POS_69',0,2,1,'111',4,'IA','POS_69'),
 ('departamento','S1','DESA','111','POS_69',2,13,1,'111',4,'DA','POS_69'),
 ('departamento','S1','DESA','314001','314082',2,14,1,'314001',1,'DA','314082'),
 ('departamento','S1','DESA','314001','JUCAR',2,1,1,'314001',4,'DA','JUCAR'),
 ('departamento','S1','DESA','314001','POS_69',2,3,1,'314001',3,'DA','POS_69'),
 ('departamento','S1','DESA','314008','POS_69',2,2,1,'314008',4,'DA','POS_69'),
 ('departamento','S1','DESA','314051','314082',2,2,1,'314051',4,'DA','314082'),
 ('departamento','S1','DESA','314051','POS_41',2,1,1,'314051',4,'DA','POS_41'),
 ('departamento','S1','DESA','314082','314001',2,14,1,'314082',1,'DA','314001'),
 ('departamento','S1','DESA','314082','314051',2,1,1,'314082',4,'DA','314051'),
 ('departamento','S1','DESA','JUCAR','111',0,10,1,'JUCAR',4,'IA','111'),
 ('departamento','S1','DESA','JUCAR','111',2,4,1,'JUCAR',4,'DA','111'),
 ('departamento','S1','DESA','JUCAR','314001',2,1,1,'JUCAR',4,'DA','314001'),
 ('departamento','S1','DESA','JUCAR','POS_41',2,5,1,'JUCAR',4,'DA','POS_41'),
 ('departamento','S1','DESA','POS_41','314051',2,1,1,'POS_41',4,'DA','314051'),
 ('departamento','S1','DESA','POS_41','JUCAR',2,4,1,'POS_41',4,'DA','JUCAR'),
 ('departamento','S1','DESA','POS_41','POS_69',0,1,1,'POS_41',4,'IA','POS_69'),
 ('departamento','S1','DESA','POS_41','POS_69',2,3,1,'POS_41',4,'DA','POS_69'),
 ('departamento','S1','DESA','POS_69','111',0,2,1,'POS_69',4,'IA','111'),
 ('departamento','S1','DESA','POS_69','111',2,13,1,'POS_69',4,'DA','111'),
 ('departamento','S1','DESA','POS_69','314001',2,3,1,'POS_69',3,'DA','314001'),
 ('departamento','S1','DESA','POS_69','314008',2,2,1,'POS_69',4,'DA','314008'),
 ('departamento','S1','DESA','POS_69','POS_41',0,1,1,'POS_69',4,'IA','POS_41'),
 ('departamento','S1','DESA','POS_69','POS_41',2,1,1,'POS_69',4,'DA','POS_41');
/*!40000 ALTER TABLE `internos` ENABLE KEYS */;


--
-- Definition of procedure `GetHistoricos`
--

DROP PROCEDURE IF EXISTS `GetHistoricos`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetHistoricos`(in id_sistema varchar(32),in num_scv int, in lista_incidencias text,in tipo_hw int, in id_hw varchar(32), in desde datetime, in hasta datetime)
BEGIN
	declare i int;
	declare pos int;
	declare cadena text;
	set i=LOCATE(",",lista_incidencias);
	set pos=1;
	create temporary table if not exists t SELECT hi.*, i.Descripcion AS Descriptor FROM HistoricoIncidencias hi, incidencias i where false;
	IF (lista_incidencias<>"") THEN												
		IF (id_hw="") THEN
			IF (tipo_hw=-1) THEN																		
				WHILE i>0 DO
					set cadena=substring(lista_incidencias,pos,i-pos);
					set pos=i+1;
					set i=LOCATE(",",lista_incidencias,pos);
		
					INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
							hi.IdIncidencia=cadena AND 
							hi.FechaHora >= desde AND
							hi.FechaHora < hasta AND 
							i.IdIncidencia=hi.IdIncidencia;
					END WHILE;
			ELSE
				WHILE i>0 DO
					set cadena=substring(lista_incidencias,pos,i-pos);
					set pos=i+1;
					set i=LOCATE(",",lista_incidencias,pos);
		
					INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
							hi.TipoHw = tipo_hw AND
							hi.IdIncidencia=cadena AND 
							hi.FechaHora >= desde AND
							hi.FechaHora < hasta AND 
							i.IdIncidencia=hi.IdIncidencia;
					END WHILE;
			END IF;
		ELSE
			IF (tipo_hw=-1) THEN																		
				WHILE i>0 DO
					set cadena=substring(lista_incidencias,pos,i-pos);
					set pos=i+1;
					set i=LOCATE(",",lista_incidencias,pos);
		
					INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
							hi.IdHw=id_hw AND 
							hi.IdIncidencia=cadena AND 
							hi.FechaHora >= desde AND
							hi.FechaHora < hasta AND 
							i.IdIncidencia=hi.IdIncidencia;
					END WHILE;
			ELSE
				WHILE i>0 DO
					set cadena=substring(lista_incidencias,pos,i-pos);
					set pos=i+1;
					set i=LOCATE(",",lista_incidencias,pos);
		
					INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
							hi.IdHw=id_hw AND 
							hi.TipoHw=tipo_hw AND 
							hi.IdIncidencia=cadena AND 
							hi.FechaHora >= desde AND
							hi.FechaHora < hasta AND 
							i.IdIncidencia=hi.IdIncidencia;
					END WHILE;
			END IF;
		END IF;
	ELSE
		IF (id_hw="") THEN
			IF (tipo_hw=-1) THEN																		
				INSERT INTO t 
					SELECT hi.*,i.Descripcion FROM HistoricoIncidencias hi, incidencias i WHERE hi.IdSistema=id_sistema AND 
  							hi.FechaHora >= desde AND
	  						hi.FechaHora < hasta AND 
								i.IdIncidencia=hi.IdIncidencia;
			ELSE
				INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias  hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
									hi.TipoHw=tipo_hw AND 
    							hi.FechaHora >= desde AND
    							hi.FechaHora < hasta AND 
									i.IdIncidencia=hi.IdIncidencia;
			END IF;
		ELSE
			IF (tipo_hw=-1) THEN																		
				INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
							hi.IdHw=id_hw AND 
							hi.FechaHora >= desde AND
							hi.FechaHora < hasta AND 
							i.IdIncidencia=hi.IdIncidencia;
			ELSE
				INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
							hi.TipoHw=tipo_hw AND
							hi.IdHw=id_hw AND 
							hi.FechaHora >= desde AND
							hi.FechaHora < hasta AND 
							i.IdIncidencia=hi.IdIncidencia;
			END IF;
		END IF;
	END IF;
	IF (num_scv=0) THEN
		SELECT * FROM t				
			ORDER BY -FechaHora;
	ELSE
		SELECT * FROM t				
			WHERE Scv=Num_scv
			ORDER BY -FechaHora;
	END IF;
		
	DROP TABLE t;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;



/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
