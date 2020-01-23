-- MySQL dump 10.13  Distrib 5.6.26, for Win64 (x86_64)
--
-- Host: localhost    Database: new_cd40_trans
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
-- Current Database: `new_cd40_trans`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `new_cd40_trans` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `new_cd40_trans`;

--
-- Table structure for table `prefijos`
--

DROP TABLE IF EXISTS `prefijos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `prefijos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdPrefijo`),
  KEY `Prefijos_FKIndex1` (`IdSistema`),
  CONSTRAINT `fk_prefijos_sistema` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prefijos`
--

LOCK TABLES `prefijos` WRITE;
/*!40000 ALTER TABLE `prefijos` DISABLE KEYS */;
INSERT INTO `prefijos` VALUES ('departamento',0),('departamento',1),('departamento',2),('departamento',3),('departamento',4),('departamento',5),('departamento',6),('departamento',7),('departamento',8),('departamento',9),('departamento',10),('departamento',11),('departamento',12),('departamento',13),('departamento',14),('departamento',15),('departamento',16),('departamento',17),('departamento',18),('departamento',19),('departamento',20),('departamento',21),('departamento',22),('departamento',23),('departamento',24),('departamento',25),('departamento',26),('departamento',27),('departamento',28),('departamento',29),('departamento',30),('departamento',31),('departamento',32);
/*!40000 ALTER TABLE `prefijos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `redes`
--

DROP TABLE IF EXISTS `redes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `redes` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRed` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdRed`),
  KEY `REDES_FKIndex1` (`IdSistema`),
  KEY `REDES_FKIndex2` (`IdSistema`,`IdPrefijo`),
  CONSTRAINT `redes_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `redes_ibfk_2` FOREIGN KEY (`IdSistema`, `IdPrefijo`) REFERENCES `prefijos` (`IdSistema`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `redes`
--

LOCK TABLES `redes` WRITE;
/*!40000 ALTER TABLE `redes` DISABLE KEYS */;
INSERT INTO `redes` VALUES ('departamento','ATS',3);
/*!40000 ALTER TABLE `redes` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `REDDELETE` BEFORE DELETE ON `redes` FOR EACH ROW BEGIN
UPDATE RECURSOSTF SET IdRed=NULL 
	WHERE IdRed=old.IdRed AND IdSistema=old.IdSistema;
DELETE FROM destinostelefonia WHERE
  IdSistema=old.IdSistema AND
  TipoDestino=1 AND
  IdPrefijo=old.IdPrefijo;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `sistema`
--

DROP TABLE IF EXISTS `sistema`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sistema` (
  `IdSistema` varchar(32) NOT NULL,
  `TiempoPTT` int(10) unsigned DEFAULT NULL,
  `TiempoSinJack1` int(10) unsigned DEFAULT NULL,
  `TiempoSinJack2` int(10) unsigned DEFAULT NULL,
  `TamLiteralEnlExt` int(10) unsigned DEFAULT '32',
  `TamLiteralDA` int(10) unsigned DEFAULT '32',
  `TamLiteralIA` int(10) unsigned DEFAULT '32',
  `TamLiteralAG` int(10) unsigned DEFAULT '32',
  `TamLiteralEmplazamiento` int(10) unsigned DEFAULT '32',
  `VersionIP` int(1) unsigned DEFAULT NULL,
  `GrupoMulticastConfiguracion` varchar(15) DEFAULT NULL,
  `PuertoMulticastConfiguracion` int(5) unsigned DEFAULT '1000',
  `EstadoScvA` int(1) unsigned DEFAULT '0',
  `EstadoScvB` int(1) unsigned DEFAULT '0',
  `NumLlamadasEnIDA` int(10) unsigned DEFAULT '10',
  `KeepAliveMultiplier` int(10) unsigned DEFAULT '10',
  `KeepAlivePeriod` int(10) unsigned DEFAULT '200',
  `NumPagDestinosInt` int(10) unsigned DEFAULT NULL,
  `NumDestinosInternosPag` int(10) unsigned DEFAULT NULL,
  `NumPagFreq` int(10) unsigned DEFAULT NULL,
  `NumFreqPagina` int(10) unsigned DEFAULT NULL,
  `NumLlamadasEntrantesIDA` int(10) unsigned DEFAULT '5',
  `NumEnlacesAI` int(10) unsigned DEFAULT '18',
  PRIMARY KEY (`IdSistema`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sistema`
--

LOCK TABLES `sistema` WRITE;
/*!40000 ALTER TABLE `sistema` DISABLE KEYS */;
INSERT INTO `sistema` VALUES ('departamento',0,0,0,32,32,32,32,32,0,'224.100.10.1',1000,0,0,4,10,200,3,15,9,12,3,9);
/*!40000 ALTER TABLE `sistema` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `AltaSistema` AFTER INSERT ON sistema FOR EACH ROW


BEGIN
    INSERT INTO Prefijos VALUES (new.IdSistema,0);
    INSERT INTO Prefijos VALUES (new.IdSistema,1);
    INSERT INTO Prefijos VALUES (new.IdSistema,2);
    INSERT INTO Prefijos VALUES (new.IdSistema,3);
    INSERT INTO Prefijos VALUES (new.IdSistema,4);
    INSERT INTO Prefijos VALUES (new.IdSistema,5);
    INSERT INTO Prefijos VALUES (new.IdSistema,6);
    INSERT INTO Prefijos VALUES (new.IdSistema,7);
    INSERT INTO Prefijos VALUES (new.IdSistema,8);
    INSERT INTO Prefijos VALUES (new.IdSistema,9);
    INSERT INTO Prefijos VALUES (new.IdSistema,10);
    INSERT INTO Prefijos VALUES (new.IdSistema,11);
    INSERT INTO Prefijos VALUES (new.IdSistema,12);
    INSERT INTO Prefijos VALUES (new.IdSistema,13);
    INSERT INTO Prefijos VALUES (new.IdSistema,14);
    INSERT INTO Prefijos VALUES (new.IdSistema,15);
    INSERT INTO Prefijos VALUES (new.IdSistema,16);
    INSERT INTO Prefijos VALUES (new.IdSistema,17);
    INSERT INTO Prefijos VALUES (new.IdSistema,18);
    INSERT INTO Prefijos VALUES (new.IdSistema,19);
    INSERT INTO Prefijos VALUES (new.IdSistema,20);
    INSERT INTO Prefijos VALUES (new.IdSistema,21);
    INSERT INTO Prefijos VALUES (new.IdSistema,22);
    INSERT INTO Prefijos VALUES (new.IdSistema,23);
    INSERT INTO Prefijos VALUES (new.IdSistema,24);
    INSERT INTO Prefijos VALUES (new.IdSistema,25);
    INSERT INTO Prefijos VALUES (new.IdSistema,26);
    INSERT INTO Prefijos VALUES (new.IdSistema,27);
    INSERT INTO Prefijos VALUES (new.IdSistema,28);
    INSERT INTO Prefijos VALUES (new.IdSistema,29);
    INSERT INTO Prefijos VALUES (new.IdSistema,30);
    INSERT INTO Prefijos VALUES (new.IdSistema,31);
    INSERT INTO Prefijos VALUES (new.IdSistema,32);
		
	INSERT INTO Redes VALUES (new.IdSistema,"ATS",3) ;    

	UPDATE ParametrosSector SET NumLlamadasEntrantesIDA=new.NumLlamadasEntrantesIDA,
                                NumLlamadasEnIDA=new.NumLlamadasEnIda,
                                NumFreqPagina=new.NumFreqPagina,
                                NumPagFreq=new.NumPagFreq,
                                NumDestinosInternosPag=new.NumDestinosInternosPag,
                                NumPagDestinosInt=new.NumPagDestinosInt,
								KeepAliveMultiplier=new.KeepAliveMultiplier,
								KeepAlivePeriod=new.KeepAlivePeriod,
								NumEnlacesAI=new.NumEnlacesAI
								 WHERE IdSistema=new.IdSistema;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `sistema_AUPD` AFTER UPDATE ON sistema FOR EACH ROW


UPDATE ParametrosSector SET NumLlamadasEntrantesIDA=new.NumLlamadasEntrantesIDA,
                                NumLlamadasEnIDA=new.NumLlamadasEnIda,
                                NumFreqPagina=new.NumFreqPagina,
                                NumPagFreq=new.NumPagFreq,
                                NumDestinosInternosPag=new.NumDestinosInternosPag,
                                NumPagDestinosInt=new.NumPagDestinosInt,
								KeepAliveMultiplier=new.KeepAliveMultiplier,
								KeepAlivePeriod=new.KeepAlivePeriod,
								NumEnlacesAI=new.NumEnlacesAI
								 WHERE IdSistema=new.IdSistema */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Current Database: `new_cd40`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `new_cd40` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `new_cd40`;

--
-- Table structure for table `prefijos`
--

DROP TABLE IF EXISTS `prefijos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `prefijos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdPrefijo`),
  KEY `Prefijos_FKIndex1` (`IdSistema`),
  CONSTRAINT `fk_prefijos_sistema` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prefijos`
--

LOCK TABLES `prefijos` WRITE;
/*!40000 ALTER TABLE `prefijos` DISABLE KEYS */;
INSERT INTO `prefijos` VALUES ('departamento',0),('departamento',1),('departamento',2),('departamento',3),('departamento',4),('departamento',5),('departamento',6),('departamento',7),('departamento',8),('departamento',9),('departamento',10),('departamento',11),('departamento',12),('departamento',13),('departamento',14),('departamento',15),('departamento',16),('departamento',17),('departamento',18),('departamento',19),('departamento',20),('departamento',21),('departamento',22),('departamento',23),('departamento',24),('departamento',25),('departamento',26),('departamento',27),('departamento',28),('departamento',29),('departamento',30),('departamento',31),('departamento',32);
/*!40000 ALTER TABLE `prefijos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `redes`
--

DROP TABLE IF EXISTS `redes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `redes` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRed` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdRed`),
  KEY `REDES_FKIndex1` (`IdSistema`),
  KEY `REDES_FKIndex2` (`IdSistema`,`IdPrefijo`),
  CONSTRAINT `redes_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `redes_ibfk_2` FOREIGN KEY (`IdSistema`, `IdPrefijo`) REFERENCES `prefijos` (`IdSistema`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `redes`
--

LOCK TABLES `redes` WRITE;
/*!40000 ALTER TABLE `redes` DISABLE KEYS */;
INSERT INTO `redes` VALUES ('departamento','ATS',3);
/*!40000 ALTER TABLE `redes` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `REDDELETE` BEFORE DELETE ON `redes` FOR EACH ROW BEGIN
UPDATE RECURSOSTF SET IdRed=NULL 
	WHERE IdRed=old.IdRed AND IdSistema=old.IdSistema;
DELETE FROM destinostelefonia WHERE
  IdSistema=old.IdSistema AND
  TipoDestino=1 AND
  IdPrefijo=old.IdPrefijo;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `sistema`
--

DROP TABLE IF EXISTS `sistema`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sistema` (
  `IdSistema` varchar(32) NOT NULL,
  `TiempoPTT` int(10) unsigned DEFAULT NULL,
  `TiempoSinJack1` int(10) unsigned DEFAULT NULL,
  `TiempoSinJack2` int(10) unsigned DEFAULT NULL,
  `TamLiteralEnlExt` int(10) unsigned DEFAULT '32',
  `TamLiteralDA` int(10) unsigned DEFAULT '32',
  `TamLiteralIA` int(10) unsigned DEFAULT '32',
  `TamLiteralAG` int(10) unsigned DEFAULT '32',
  `TamLiteralEmplazamiento` int(10) unsigned DEFAULT '32',
  `VersionIP` int(1) unsigned DEFAULT NULL,
  `GrupoMulticastConfiguracion` varchar(15) DEFAULT NULL,
  `PuertoMulticastConfiguracion` int(5) unsigned DEFAULT '1000',
  `EstadoScvA` int(1) unsigned DEFAULT '0',
  `EstadoScvB` int(1) unsigned DEFAULT '0',
  `NumLlamadasEnIDA` int(10) unsigned DEFAULT '10',
  `KeepAliveMultiplier` int(10) unsigned DEFAULT '10',
  `KeepAlivePeriod` int(10) unsigned DEFAULT '200',
  `NumPagDestinosInt` int(10) unsigned DEFAULT NULL,
  `NumDestinosInternosPag` int(10) unsigned DEFAULT NULL,
  `NumPagFreq` int(10) unsigned DEFAULT NULL,
  `NumFreqPagina` int(10) unsigned DEFAULT NULL,
  `NumLlamadasEntrantesIDA` int(10) unsigned DEFAULT '5',
  `NumEnlacesAI` int(10) unsigned DEFAULT '18',
  PRIMARY KEY (`IdSistema`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sistema`
--

LOCK TABLES `sistema` WRITE;
/*!40000 ALTER TABLE `sistema` DISABLE KEYS */;
INSERT INTO `sistema` VALUES ('departamento',0,0,0,32,32,32,32,32,0,'224.100.10.1',1000,0,0,4,10,200,3,15,9,12,3,9);
/*!40000 ALTER TABLE `sistema` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `AltaSistema` AFTER INSERT ON sistema FOR EACH ROW


BEGIN
    INSERT INTO Prefijos VALUES (new.IdSistema,0);
    INSERT INTO Prefijos VALUES (new.IdSistema,1);
    INSERT INTO Prefijos VALUES (new.IdSistema,2);
    INSERT INTO Prefijos VALUES (new.IdSistema,3);
    INSERT INTO Prefijos VALUES (new.IdSistema,4);
    INSERT INTO Prefijos VALUES (new.IdSistema,5);
    INSERT INTO Prefijos VALUES (new.IdSistema,6);
    INSERT INTO Prefijos VALUES (new.IdSistema,7);
    INSERT INTO Prefijos VALUES (new.IdSistema,8);
    INSERT INTO Prefijos VALUES (new.IdSistema,9);
    INSERT INTO Prefijos VALUES (new.IdSistema,10);
    INSERT INTO Prefijos VALUES (new.IdSistema,11);
    INSERT INTO Prefijos VALUES (new.IdSistema,12);
    INSERT INTO Prefijos VALUES (new.IdSistema,13);
    INSERT INTO Prefijos VALUES (new.IdSistema,14);
    INSERT INTO Prefijos VALUES (new.IdSistema,15);
    INSERT INTO Prefijos VALUES (new.IdSistema,16);
    INSERT INTO Prefijos VALUES (new.IdSistema,17);
    INSERT INTO Prefijos VALUES (new.IdSistema,18);
    INSERT INTO Prefijos VALUES (new.IdSistema,19);
    INSERT INTO Prefijos VALUES (new.IdSistema,20);
    INSERT INTO Prefijos VALUES (new.IdSistema,21);
    INSERT INTO Prefijos VALUES (new.IdSistema,22);
    INSERT INTO Prefijos VALUES (new.IdSistema,23);
    INSERT INTO Prefijos VALUES (new.IdSistema,24);
    INSERT INTO Prefijos VALUES (new.IdSistema,25);
    INSERT INTO Prefijos VALUES (new.IdSistema,26);
    INSERT INTO Prefijos VALUES (new.IdSistema,27);
    INSERT INTO Prefijos VALUES (new.IdSistema,28);
    INSERT INTO Prefijos VALUES (new.IdSistema,29);
    INSERT INTO Prefijos VALUES (new.IdSistema,30);
    INSERT INTO Prefijos VALUES (new.IdSistema,31);
    INSERT INTO Prefijos VALUES (new.IdSistema,32);
		
	INSERT INTO Redes VALUES (new.IdSistema,"ATS",3) ;    

	UPDATE ParametrosSector SET NumLlamadasEntrantesIDA=new.NumLlamadasEntrantesIDA,
                                NumLlamadasEnIDA=new.NumLlamadasEnIda,
                                NumFreqPagina=new.NumFreqPagina,
                                NumPagFreq=new.NumPagFreq,
                                NumDestinosInternosPag=new.NumDestinosInternosPag,
                                NumPagDestinosInt=new.NumPagDestinosInt,
								KeepAliveMultiplier=new.KeepAliveMultiplier,
								KeepAlivePeriod=new.KeepAlivePeriod,
								NumEnlacesAI=new.NumEnlacesAI
								 WHERE IdSistema=new.IdSistema;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `sistema_AUPD` AFTER UPDATE ON sistema FOR EACH ROW


UPDATE ParametrosSector SET NumLlamadasEntrantesIDA=new.NumLlamadasEntrantesIDA,
                                NumLlamadasEnIDA=new.NumLlamadasEnIda,
                                NumFreqPagina=new.NumFreqPagina,
                                NumPagFreq=new.NumPagFreq,
                                NumDestinosInternosPag=new.NumDestinosInternosPag,
                                NumPagDestinosInt=new.NumPagDestinosInt,
								KeepAliveMultiplier=new.KeepAliveMultiplier,
								KeepAlivePeriod=new.KeepAlivePeriod,
								NumEnlacesAI=new.NumEnlacesAI
								 WHERE IdSistema=new.IdSistema */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-10-20 14:22:36
