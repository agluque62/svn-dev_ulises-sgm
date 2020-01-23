-- MySQL dump 10.11
--
-- Host: localhost    Database: cd40
-- ------------------------------------------------------
-- Server version	5.0.45-community-nt-log

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


USE CD40;
--
-- Dumping data for table `abonados`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `abonados` WRITE;
/*!40000 ALTER TABLE `abonados` DISABLE KEYS */;
/*!40000 ALTER TABLE `abonados` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `agenda`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `agenda` WRITE;
/*!40000 ALTER TABLE `agenda` DISABLE KEYS */;
/*!40000 ALTER TABLE `agenda` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `agrupaciones`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `agrupaciones` WRITE;
/*!40000 ALTER TABLE `agrupaciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `agrupaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `alarmas`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `alarmas` WRITE;
/*!40000 ALTER TABLE `alarmas` DISABLE KEYS */;
/*!40000 ALTER TABLE `alarmas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `altavoces`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `altavoces` WRITE;
/*!40000 ALTER TABLE `altavoces` DISABLE KEYS */;
/*!40000 ALTER TABLE `altavoces` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `controlbackup`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `controlbackup` WRITE;
/*!40000 ALTER TABLE `controlbackup` DISABLE KEYS */;
/*!40000 ALTER TABLE `controlbackup` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `destinos`
--
-- WHERE:  IdSistema='SICCIP'

--
-- Dumping data for table `sistema`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `sistema` WRITE;
/*!40000 ALTER TABLE `sistema` DISABLE KEYS */;
INSERT INTO `sistema` VALUES ('siccip',10,10,10,8,8,8,0,8,4,'224.100.20.1',1010,0,0);
/*!40000 ALTER TABLE `sistema` ENABLE KEYS */;
UNLOCK TABLES;


LOCK TABLES `destinos` WRITE;
/*!40000 ALTER TABLE `destinos` DISABLE KEYS */;
INSERT INTO `destinos` VALUES ('SICCIP','000001',0),('SICCIP','000003',0),('SICCIP','000004',0),('SICCIP','000005',0),('SICCIP','000018',0),('SICCIP','CADIZ',0),('SICCIP','CME1',2),('SICCIP','CME3',2),('SICCIP','COMTE',2),('SICCIP','COMTPUEN',2),('SICCIP','ISB1',0),('SICCIP','ISB1.1',0),('SICCIP','PUENTE',2),('SICCIP','RADIO1',2),('SICCIP','RADIO2',2),('SICCIP','UHF-1',0),('SICCIP','WB#1',0),('SICCIP','WB#2',0);
/*!40000 ALTER TABLE `destinos` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `InsertDestino` AFTER INSERT ON `destinos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('Destinos');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `UpdateDestino` AFTER UPDATE ON `destinos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('Destinos');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `DeleteDestino` AFTER DELETE ON `destinos` FOR EACH ROW BEGIN
	REPLACE INTO TablasModificadas (IdTabla) VALUES ('Destinos') ;
  IF old.TipoDestino=0 THEN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadio') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector') ;
	ELSE
		REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosTelefonia') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternos') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector') ;
	END IF;
END */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `destinosexternos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `destinosexternos` WRITE;
/*!40000 ALTER TABLE `destinosexternos` DISABLE KEYS */;
/*!40000 ALTER TABLE `destinosexternos` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `AltaDestinosExternos` BEFORE INSERT ON `destinosexternos` FOR EACH ROW BEGIN
  DECLARE numAbonados INT(1);
 	IF new.IdAbonado IS NOT null THEN
		SELECT COUNT(*) INTO numAbonados
				FROM Abonados
				WHERE IdSistema=new.IdSistema AND
							IdAbonado=new.IdAbonado;
		IF numAbonados=0 THEN
			INSERT INTO Abonados SET IdSistema=new.IdSistema,
																IdAbonado=new.IdAbonado;
		END IF;
	END IF ;
	INSERT INTO DestinosTelefonia (IdSistema,IdDestino,TipoDestino,IdPrefijo,IdGrupo) 
		VALUES (new.IdSistema,new.IdDestino,new.TipoDestino,new.IdPrefijo,null);
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternos');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `UpdateDestinosExternos` BEFORE UPDATE ON `destinosexternos` FOR EACH ROW BEGIN
  DECLARE numAbonados INT(1);
 	IF new.IdAbonado IS NOT null THEN
	 	SELECT COUNT(*) INTO numAbonados
				FROM Abonados
				WHERE IdSistema=new.IdSistema AND
							IdAbonado=new.IdAbonado;
		IF numAbonados=0 THEN
			INSERT INTO Abonados SET IdSistema=new.IdSistema,
															IdAbonado=new.IdAbonado;
		END IF;
	END IF ;
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternos');
END */;;

/*!50003 SET SESSION SQL_MODE="STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`%` */ /*!50003 TRIGGER `DeleteDestinosExternos` BEFORE DELETE ON `destinosexternos` FOR EACH ROW BEGIN
	IF old.IdPrefijo=1 THEN
		UPDATE RECURSOSLCEN SET IdDestino=NULL,TipoDestino=NULL,IdPrefijo=NULL
				WHERE IdSistema = old.IdSistema AND IdDestino=old.IdDestino AND TipoDestino=old.TipoDestino AND IdPrefijo=old.IdPrefijo;
	END IF;
	
	IF old.IdPrefijo>2 THEN
		UPDATE RECURSOSTF SET IdDestino=NULL,TipoDestino=NULL,IdPrefijo=NULL 
				WHERE IdSistema = old.IdSistema AND IdDestino=old.IdDestino AND TipoDestino=old.TipoDestino AND IdPrefijo=old.IdPrefijo;
	END IF;
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternos');
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector');
END */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `destinosexternossector`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `destinosexternossector` WRITE;
/*!40000 ALTER TABLE `destinosexternossector` DISABLE KEYS */;
/*!40000 ALTER TABLE `destinosexternossector` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `AltaDestinoExternoSector` AFTER INSERT ON `destinosexternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `UpdateDestinoExternoSector` AFTER UPDATE ON `destinosexternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `DeleteDestinoExternoSector` AFTER DELETE ON `destinosexternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector');
END */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `destinosinternos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `destinosinternos` WRITE;
/*!40000 ALTER TABLE `destinosinternos` DISABLE KEYS */;
INSERT INTO `destinosinternos` VALUES ('SICCIP','CME1',2,0,'CME1','SICCIP'),('SICCIP','CME1',2,2,'CME1','SICCIP'),('SICCIP','CME3',2,0,'CME3','SICCIP'),('SICCIP','CME3',2,2,'CME3','SICCIP'),('SICCIP','COMTE',2,0,'COMTE','SICCIP'),('SICCIP','COMTE',2,2,'COMTE','SICCIP'),('SICCIP','COMTPUEN',2,0,'COMTPUEN','SICCIP'),('SICCIP','COMTPUEN',2,2,'COMTPUEN','SICCIP'),('SICCIP','PUENTE',2,0,'PUENTE','SICCIP'),('SICCIP','PUENTE',2,2,'PUENTE','SICCIP'),('SICCIP','RADIO1',2,0,'RADIO1','SICCIP'),('SICCIP','RADIO1',2,2,'RADIO1','SICCIP'),('SICCIP','RADIO2',2,0,'RADIO2','SICCIP'),('SICCIP','RADIO2',2,2,'RADIO2','SICCIP');
/*!40000 ALTER TABLE `destinosinternos` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `AltaDestinoInterno` BEFORE INSERT ON `destinosinternos` FOR EACH ROW BEGIN
    INSERT INTO DestinosTelefonia SET IdSistema=new.IdSistema,
														 					IdDestino=new.IdDestino,
														 					TipoDestino=new.TipoDestino,
																			IdPrefijo=new.IdPrefijo;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `UpdateDestinoInterno` AFTER UPDATE ON `destinosinternos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `DeleteDestinoInterno` AFTER DELETE ON `destinosinternos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos');
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `destinosinternossector`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `destinosinternossector` WRITE;
/*!40000 ALTER TABLE `destinosinternossector` DISABLE KEYS */;
/*!40000 ALTER TABLE `destinosinternossector` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `AltaDestinoInternoSector` AFTER INSERT ON `destinosinternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `UpdateDestinoInternoSector` AFTER UPDATE ON `destinosinternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `DeleteDestinosInternosSector` BEFORE DELETE ON `destinosinternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `destinosradio`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `destinosradio` WRITE;
/*!40000 ALTER TABLE `destinosradio` DISABLE KEYS */;
INSERT INTO `destinosradio` VALUES ('SICCIP','000003',0,3,0),('SICCIP','000005',0,3,0);
/*!40000 ALTER TABLE `destinosradio` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `AltaDestinoRadio` BEFORE INSERT ON `destinosradio` FOR EACH ROW BEGIN
  		DECLARE numDestinos INT(1);
 	 		SELECT COUNT(*) INTO numDestinos
						FROM Destinos
						WHERE IdSistema=new.IdSistema AND
							IdDestino=new.IdDestino AND
							TipoDestino=new.TipoDestino;
			IF numDestinos=0 THEN
					INSERT INTO Destinos SET IdSistema=new.IdSistema,
																	 IdDestino=new.IdDestino,
																	 TipoDestino=new.TipoDestino;
			END IF;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadio');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `UpdateDestinoRadio` AFTER UPDATE ON `destinosradio` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadio');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `DESTRADIODELETE` BEFORE DELETE ON `destinosradio` FOR EACH ROW BEGIN
IF old.TipoDestino=0 THEN
	UPDATE RECURSOSRADIO SET IdDestino=NULL,TipoDestino=NULL 
  				WHERE IdSistema = old.IdSistema AND IdDestino=old.IdDestino AND TipoDestino=old.TipoDestino;
END IF ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadio');
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector') ;
END */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `destinosradiosector`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `destinosradiosector` WRITE;
/*!40000 ALTER TABLE `destinosradiosector` DISABLE KEYS */;
/*!40000 ALTER TABLE `destinosradiosector` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `AltaDestinoRadioSector` AFTER INSERT ON `destinosradiosector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `UpdateDestinoRadioSector` AFTER UPDATE ON `destinosradiosector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `DeleteDestinoRadioSector` AFTER DELETE ON `destinosradiosector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector');
END */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `destinostelefonia`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `destinostelefonia` WRITE;
/*!40000 ALTER TABLE `destinostelefonia` DISABLE KEYS */;
INSERT INTO `destinostelefonia` VALUES ('SICCIP','CME1',2,0,NULL),('SICCIP','CME1',2,2,NULL),('SICCIP','CME3',2,0,NULL),('SICCIP','CME3',2,2,NULL),('SICCIP','COMTE',2,0,NULL),('SICCIP','COMTE',2,2,NULL),('SICCIP','COMTPUEN',2,0,NULL),('SICCIP','COMTPUEN',2,2,NULL),('SICCIP','PUENTE',2,0,NULL),('SICCIP','PUENTE',2,2,NULL),('SICCIP','RADIO1',2,0,NULL),('SICCIP','RADIO1',2,2,NULL),('SICCIP','RADIO2',2,0,NULL),('SICCIP','RADIO2',2,2,NULL);
/*!40000 ALTER TABLE `destinostelefonia` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `AltaDestinosTelefonia` BEFORE INSERT ON `destinostelefonia` FOR EACH ROW BEGIN
 			DECLARE numDestinos INT(1);
 	 		SELECT COUNT(*) INTO numDestinos
						FROM Destinos
						WHERE IdSistema=new.IdSistema AND
							IdDestino=new.IdDestino AND
							TipoDestino=new.TipoDestino;
			IF numDestinos=0 THEN
					INSERT INTO Destinos SET IdSistema=new.IdSistema,
																	 IdDestino=new.IdDestino,
																	 TipoDestino=new.TipoDestino;
			END IF ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosTelefonia');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `UpdateDestinoTelefonia` AFTER UPDATE ON `destinostelefonia` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosTelefonia');
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `DeleteDestinoTelefonia` AFTER DELETE ON `destinostelefonia` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosTelefonia') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternos') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector') ;
END */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `emplazamientos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `emplazamientos` WRITE;
/*!40000 ALTER TABLE `emplazamientos` DISABLE KEYS */;
INSERT INTO `emplazamientos` VALUES ('SICCIP','DEMO-SICCIP');
/*!40000 ALTER TABLE `emplazamientos` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `PRDELETE` BEFORE DELETE ON `emplazamientos` FOR EACH ROW UPDATE RECURSOSRADIO SET IdEmplazamiento=NULL
			WHERE IdSistema = old.IdSistema AND IdEmplazamiento=old.IdEmplazamiento */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `encaminamientos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `encaminamientos` WRITE;
/*!40000 ALTER TABLE `encaminamientos` DISABLE KEYS */;
INSERT INTO `encaminamientos` VALUES ('SICCIP','SICCIP',1,0,'310099');
/*!40000 ALTER TABLE `encaminamientos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `equiposeu`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `equiposeu` WRITE;
/*!40000 ALTER TABLE `equiposeu` DISABLE KEYS */;
INSERT INTO `equiposeu` VALUES ('SICCIP','ALTAV 01','192.168.100.81','192.168.200.81',2),('SICCIP','ALTAV 02','192.168.100.82','192.168.200.82',2),('SICCIP','FAX','192.168.100.141','192.168.200.141',2),('SICCIP','KG 01','192.168.100.71','192.168.200.71',2),('SICCIP','KG 02','192.168.100.72','192.168.200.72',2),('SICCIP','KY 01','192.168.100.61','192.168.200.61',2),('SICCIP','KY 02','192.168.100.62','192.168.200.62',2),('SICCIP','LK14','192.168.100.121','192.168.200.121',2),('SICCIP','MHS 01','192.168.100.91','192.168.200.91',2),('SICCIP','MHS 02','192.168.100.101','192.168.200.101',2),('SICCIP','MHSKWR 01','192.168.100.111','192.168.200.111',2),('SICCIP','MHSKWR 02','192.168.100.112','192.168.200.112',2),('SICCIP','MOD 01','192.168.100.51','192.168.200.51',2),('SICCIP','MOD 02','192.168.100.52','192.168.200.52',2),('SICCIP','MOD 03','192.168.100.53','192.168.200.53',2),('SICCIP','RX01','192.168.100.11','192.168.200.11',2),('SICCIP','RX02','192.168.100.12','192.168.200.12',2),('SICCIP','SATDATA 01','192.168.100.41','192.168.200.41',2),('SICCIP','SATDATA 02','192.168.100.42','192.168.200.42',2),('SICCIP','TRX01','192.168.100.1','192.168.200.1',2),('SICCIP','TRX02','192.168.100.2','192.168.200.2',2),('SICCIP','TXARQ','192.168.100.131','192.168.200.131',2),('SICCIP','VHF 1','192.168.100.31','192.168.200.31',2),('SICCIP','VHF 2','192.168.100.32','192.168.200.32',2),('SICCIP','XCVR 01','192.168.100.21','192.168.200.21',2),('SICCIP','XCVR 02','192.168.100.22','192.168.200.22',2);
/*!40000 ALTER TABLE `equiposeu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `estadoaltavoces`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `estadoaltavoces` WRITE;
/*!40000 ALTER TABLE `estadoaltavoces` DISABLE KEYS */;
/*!40000 ALTER TABLE `estadoaltavoces` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `estadorecursos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `estadorecursos` WRITE;
/*!40000 ALTER TABLE `estadorecursos` DISABLE KEYS */;
/*!40000 ALTER TABLE `estadorecursos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `estadosrecursos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `estadosrecursos` WRITE;
/*!40000 ALTER TABLE `estadosrecursos` DISABLE KEYS */;
/*!40000 ALTER TABLE `estadosrecursos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `eventosradio`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `eventosradio` WRITE;
/*!40000 ALTER TABLE `eventosradio` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventosradio` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `eventostelefonia`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `eventostelefonia` WRITE;
/*!40000 ALTER TABLE `eventostelefonia` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventostelefonia` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `externos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `externos` WRITE;
/*!40000 ALTER TABLE `externos` DISABLE KEYS */;
/*!40000 ALTER TABLE `externos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `funciones`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `funciones` WRITE;
/*!40000 ALTER TABLE `funciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `funciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `grupostelefonia`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `grupostelefonia` WRITE;
/*!40000 ALTER TABLE `grupostelefonia` DISABLE KEYS */;
/*!40000 ALTER TABLE `grupostelefonia` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `historicoincidencias`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `historicoincidencias` WRITE;
/*!40000 ALTER TABLE `historicoincidencias` DISABLE KEYS */;
/*!40000 ALTER TABLE `historicoincidencias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `incidencias`
--
-- WHERE:  IdSistema='SICCIP'


--
-- Dumping data for table `indicadores`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `indicadores` WRITE;
/*!40000 ALTER TABLE `indicadores` DISABLE KEYS */;
/*!40000 ALTER TABLE `indicadores` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `internos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `internos` WRITE;
/*!40000 ALTER TABLE `internos` DISABLE KEYS */;
/*!40000 ALTER TABLE `internos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `niveles`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `niveles` WRITE;
/*!40000 ALTER TABLE `niveles` DISABLE KEYS */;
INSERT INTO `niveles` VALUES ('SICCIP','SICCIP','CME1',0,0,0,0),('SICCIP','SICCIP','CME3',0,0,0,0),('SICCIP','SICCIP','COMTE',0,0,0,0),('SICCIP','SICCIP','PUENTE',0,0,0,0),('SICCIP','SICCIP','RADIO1',0,0,0,0),('SICCIP','SICCIP','RADIO2',0,0,0,0);
/*!40000 ALTER TABLE `niveles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `nucleos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `nucleos` WRITE;
/*!40000 ALTER TABLE `nucleos` DISABLE KEYS */;
INSERT INTO `nucleos` VALUES ('SICCIP','SICCIP');
/*!40000 ALTER TABLE `nucleos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `operadores`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `operadores` WRITE;
/*!40000 ALTER TABLE `operadores` DISABLE KEYS */;
INSERT INTO `operadores` VALUES ('1','SICCIP','1',3,'','','','2001-01-01','');
/*!40000 ALTER TABLE `operadores` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `parametrosrecurso`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `parametrosrecurso` WRITE;
/*!40000 ALTER TABLE `parametrosrecurso` DISABLE KEYS */;
INSERT INTO `parametrosrecurso` VALUES ('SICCIP','1JV',1,0,0,0,0,0,20,0),('SICCIP','DTS - LSB',0,0,0,0,0,1,20,0),('SICCIP','DTS - USB',0,0,0,0,0,1,20,0),('SICCIP','HF #1-A1 USB',0,0,0,0,0,1,20,0),('SICCIP','HF #1-A2 LSB',0,0,0,0,0,1,20,0),('SICCIP','HF #2 - A1',0,0,0,0,0,1,20,0),('SICCIP','HF #2 - A2',0,0,0,0,0,1,20,0),('SICCIP','HF #4 - A1',0,0,0,0,0,1,20,0),('SICCIP','HF #4 - A2',0,0,0,0,0,1,20,0),('SICCIP','HF #5 - A1',0,0,0,0,0,1,20,0),('SICCIP','HF #6 - A1',0,0,0,0,0,1,20,0),('SICCIP','HF1-LNK11.LSB',0,0,0,0,0,1,20,0),('SICCIP','HF1-LNK11.USB',0,0,0,0,0,1,20,0),('SICCIP','HF2-LNK11.LSB',0,0,0,0,0,1,20,0),('SICCIP','HF2-LNK11.USB',0,0,0,0,0,1,20,0),('SICCIP','JA',1,0,0,0,0,0,20,0),('SICCIP','KG #1 - N',0,0,0,0,0,1,20,0),('SICCIP','KG #1 - R',0,0,0,0,0,1,20,0),('SICCIP','KW #1 - N',0,0,0,0,0,1,20,0),('SICCIP','KW #1 - R',0,0,0,0,0,1,20,0),('SICCIP','KY1-N NB',0,0,0,0,0,1,20,0),('SICCIP','KY1-N VINSON-2',0,0,0,0,0,1,20,0),('SICCIP','KY1-R',0,0,0,0,0,1,20,0),('SICCIP','KY2-N',0,0,0,0,0,1,20,0),('SICCIP','KY2-R',0,0,0,0,0,1,20,0),('SICCIP','MHS-1',0,0,0,0,0,1,20,0),('SICCIP','MHS-2',0,0,0,0,0,1,20,0),('SICCIP','MODEM #1-A1',0,0,0,0,0,1,20,0),('SICCIP','MODEM #1-D1',0,0,0,0,0,1,20,0),('SICCIP','MODEM #2-A1',0,0,0,0,0,1,20,0),('SICCIP','UHF #1-NB',0,0,0,0,0,1,20,0),('SICCIP','UHF #3-NB',0,0,0,0,0,1,20,0),('SICCIP','UHF #4-NB',0,0,0,0,0,1,20,0),('SICCIP','UHF-LNK11-1',0,0,0,0,0,1,20,0),('SICCIP','UHF-LNK11-2',0,0,0,0,0,1,20,0),('SICCIP','VINSON-1',0,0,0,0,0,1,20,0),('SICCIP','VINSON-3',0,0,0,0,0,1,20,0);
/*!40000 ALTER TABLE `parametrosrecurso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `parametrossector`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `parametrossector` WRITE;
/*!40000 ALTER TABLE `parametrossector` DISABLE KEYS */;
INSERT INTO `parametrossector` VALUES ('SICCIP','SICCIP','CME1',3,4,12,9,19,3,0,0,200,10),('SICCIP','SICCIP','CME3',3,4,12,9,19,3,0,0,200,10),('SICCIP','SICCIP','COMTE',3,4,12,9,19,3,0,0,200,10),('SICCIP','SICCIP','PUENTE',3,4,12,9,19,3,0,0,200,10),('SICCIP','SICCIP','RADIO1',3,4,12,9,19,3,0,0,200,10),('SICCIP','SICCIP','RADIO2',3,4,12,9,19,3,0,0,200,10);
/*!40000 ALTER TABLE `parametrossector` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `permisosredes`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `permisosredes` WRITE;
/*!40000 ALTER TABLE `permisosredes` DISABLE KEYS */;
/*!40000 ALTER TABLE `permisosredes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `prefijos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `prefijos` WRITE;
/*!40000 ALTER TABLE `prefijos` DISABLE KEYS */;
INSERT INTO `prefijos` VALUES ('siccip',0),('siccip',1),('siccip',2),('siccip',3),('siccip',4),('siccip',5),('siccip',6),('siccip',7),('siccip',8),('siccip',9),('siccip',32);
/*!40000 ALTER TABLE `prefijos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `radio`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `radio` WRITE;
/*!40000 ALTER TABLE `radio` DISABLE KEYS */;
/*!40000 ALTER TABLE `radio` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `rangos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `rangos` WRITE;
/*!40000 ALTER TABLE `rangos` DISABLE KEYS */;
INSERT INTO `rangos` VALUES ('SICCIP','SICCIP','O','310000',3,'','310099');
/*!40000 ALTER TABLE `rangos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `recursos`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `recursos` WRITE;
/*!40000 ALTER TABLE `recursos` DISABLE KEYS */;
INSERT INTO `recursos` VALUES ('SICCIP','1JV',1,NULL,'CGW-02',2,3,3,1,'',0),('SICCIP','DTS - LSB',0,NULL,'CGW-03',5,11,0,1,'',0),('SICCIP','DTS - USB',0,NULL,'CGW-03',5,11,0,0,'',0),('SICCIP','HF #1-A1 USB',0,NULL,'CGW-01',2,0,1,0,'',0),('SICCIP','HF #1-A2 LSB',0,NULL,'CGW-01',0,0,1,1,'',0),('SICCIP','HF #2 - A1',0,NULL,'CGW-01',2,0,1,2,'',0),('SICCIP','HF #2 - A2',0,NULL,'CGW-01',2,0,1,3,'',0),('SICCIP','HF #4 - A1',0,NULL,'CGW-01',2,0,2,2,'',0),('SICCIP','HF #4 - A2',0,NULL,'CGW-01',2,0,2,3,'',0),('SICCIP','HF #5 - A1',0,NULL,'CGW-02',2,0,3,2,'',0),('SICCIP','HF #6 - A1',0,NULL,'CGW-02',2,0,3,3,'',0),('SICCIP','HF1-LNK11.LSB',0,NULL,'CGW-03',5,11,1,1,'',0),('SICCIP','HF1-LNK11.USB',0,NULL,'CGW-03',5,11,1,0,'',0),('SICCIP','HF2-LNK11.LSB',0,NULL,'CGW-03',5,11,1,3,'',0),('SICCIP','HF2-LNK11.USB',0,NULL,'CGW-03',5,11,1,2,'',0),('SICCIP','JA',1,NULL,'CGW-02',2,3,3,0,'',0),('SICCIP','KG #1 - N',0,NULL,'CGW-02',5,11,0,3,'',0),('SICCIP','KG #1 - R',0,NULL,'CGW-02',5,11,1,0,'',0),('SICCIP','KW #1 - N',0,NULL,'CGW-02',5,11,1,3,'',0),('SICCIP','KW #1 - R',0,NULL,'CGW-02',5,11,1,1,'',0),('SICCIP','KY1-N NB',0,NULL,'CGW-01',2,0,2,0,'',0),('SICCIP','KY1-N VINSON-2',0,NULL,'CGW-02',5,11,0,1,'',0),('SICCIP','KY1-R',0,NULL,'CGW-01',2,0,3,1,'',0),('SICCIP','KY2-N',0,NULL,'CGW-01',2,0,3,3,'',0),('SICCIP','KY2-R',0,NULL,'CGW-01',2,0,3,2,'',0),('SICCIP','MHS-1',0,NULL,'CGW-02',5,11,2,0,'',0),('SICCIP','MHS-2',0,NULL,'CGW-02',5,11,2,1,'',0),('SICCIP','MODEM #1-A1',0,NULL,'CGW-01',2,0,3,0,'',0),('SICCIP','MODEM #1-D1',0,NULL,'CGW-02',5,11,1,2,'',0),('SICCIP','MODEM #2-A1',0,NULL,'CGW-01',2,0,0,1,'',0),('SICCIP','UHF #1-NB',0,NULL,'CGW-01',2,0,0,0,'',0),('SICCIP','UHF #3-NB',0,NULL,'CGW-01',2,0,0,2,'',0),('SICCIP','UHF #4-NB',0,NULL,'CGW-01',2,0,0,3,'',0),('SICCIP','UHF-LNK11-1',0,NULL,'CGW-03',5,11,0,2,'',0),('SICCIP','UHF-LNK11-2',0,NULL,'CGW-03',5,11,0,3,'',0),('SICCIP','VINSON-1',0,NULL,'CGW-02',5,11,0,0,'',0),('SICCIP','VINSON-3',0,NULL,'CGW-02',5,11,0,2,'',0);
/*!40000 ALTER TABLE `recursos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `recursoslcen`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `recursoslcen` WRITE;
/*!40000 ALTER TABLE `recursoslcen` DISABLE KEYS */;
/*!40000 ALTER TABLE `recursoslcen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `recursosradio`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `recursosradio` WRITE;
/*!40000 ALTER TABLE `recursosradio` DISABLE KEYS */;
INSERT INTO `recursosradio` VALUES ('SICCIP','DTS - LSB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','DTS - USB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF #1-A1 USB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF #1-A2 LSB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF #2 - A1',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF #2 - A2',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF #4 - A1',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF #4 - A2',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF #5 - A1',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF #6 - A1',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF1-LNK11.LSB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF1-LNK11.USB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF2-LNK11.LSB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','HF2-LNK11.USB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','KG #1 - N',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','KG #1 - R',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','KW #1 - N',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','KW #1 - R',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','KY1-N NB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','KY1-N VINSON-2',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','KY1-R',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','KY2-N',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','KY2-R',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','MHS-1',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','MHS-2',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','MODEM #1-A1',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','MODEM #1-D1',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','MODEM #2-A1',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','UHF #1-NB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','UHF #3-NB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','UHF #4-NB',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','UHF-LNK11-1',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','UHF-LNK11-2',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','VINSON-1',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10),('SICCIP','VINSON-3',0,0,NULL,'DEMO-SICCIP',0,'h','h',0,-30,0,-30,0,-33,0,-18,0,0,0,0,0,0,0,1,1,200,0,-33,120,1,200,10);
/*!40000 ALTER TABLE `recursosradio` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `recursostf`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `recursostf` WRITE;
/*!40000 ALTER TABLE `recursostf` DISABLE KEYS */;
INSERT INTO `recursostf` VALUES ('SICCIP','1JV',1,0,1,NULL,NULL,NULL,'0',NULL),('SICCIP','JA',1,0,1,NULL,NULL,NULL,'0',NULL);
/*!40000 ALTER TABLE `recursostf` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `redes`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `redes` WRITE;
/*!40000 ALTER TABLE `redes` DISABLE KEYS */;
INSERT INTO `redes` VALUES ('siccip','ATS',3);
/*!40000 ALTER TABLE `redes` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `REDDELETE` BEFORE DELETE ON `redes` FOR EACH ROW UPDATE RECURSOSTF SET IdRed=NULL 
	WHERE IdRed=old.IdRed AND IdSistema=old.IdSistema */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `registrobackup`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `registrobackup` WRITE;
/*!40000 ALTER TABLE `registrobackup` DISABLE KEYS */;
/*!40000 ALTER TABLE `registrobackup` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `registrotareas`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `registrotareas` WRITE;
/*!40000 ALTER TABLE `registrotareas` DISABLE KEYS */;
/*!40000 ALTER TABLE `registrotareas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `rutas`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `rutas` WRITE;
/*!40000 ALTER TABLE `rutas` DISABLE KEYS */;
/*!40000 ALTER TABLE `rutas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `sectores`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `sectores` WRITE;
/*!40000 ALTER TABLE `sectores` DISABLE KEYS */;
INSERT INTO `sectores` VALUES ('SICCIP','SICCIP','CME1',NULL,NULL,NULL,1,'V','C',4,0,1),('SICCIP','SICCIP','CME3',NULL,NULL,NULL,1,'V','C',4,0,2),('SICCIP','SICCIP','COMTE',NULL,NULL,NULL,1,'V','C',4,0,3),('SICCIP','SICCIP','COMTPUEN',NULL,NULL,NULL,0,'R','C',4,0,0),('SICCIP','SICCIP','PUENTE',NULL,NULL,NULL,1,'V','C',4,0,4),('SICCIP','SICCIP','RADIO1',NULL,NULL,NULL,1,'V','C',4,0,5),('SICCIP','SICCIP','RADIO2',NULL,NULL,NULL,1,'V','C',4,0,6);
/*!40000 ALTER TABLE `sectores` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `AltaSector` AFTER INSERT ON `sectores` FOR EACH ROW BEGIN
    INSERT INTO DestinosInternos SET IdSistema=new.IdSistema,
																			IdDestino=new.IdSector,
																			TipoDestino=2,
																			IdPrefijo=0,
																			IdSector=new.IdSector,
																			IdNucleo=new.IdNucleo;
    INSERT INTO DestinosInternos SET IdSistema=new.IdSistema,
																			IdDestino=new.IdSector,
																			TipoDestino=2,
																			IdPrefijo=2,
																			IdSector=new.IdSector,
																			IdNucleo=new.IdNucleo ;
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `ActualizaSector` AFTER UPDATE ON `sectores` FOR EACH ROW BEGIN
    UPDATE Destinos 
			SET IdDestino=new.IdSector
			WHERE IdSistema=new.IdSistema AND
													IdDestino=old.IdSector AND
													TipoDestino=2 ;
END */;;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `BajaSector` BEFORE DELETE ON `sectores` FOR EACH ROW BEGIN
    DELETE FROM Destinos WHERE IdSistema=old.IdSistema AND
																IdDestino=old.IdSector AND
																TipoDestino=2 ;
END */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `sectoresagrupacion`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `sectoresagrupacion` WRITE;
/*!40000 ALTER TABLE `sectoresagrupacion` DISABLE KEYS */;
/*!40000 ALTER TABLE `sectoresagrupacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `sectoressector`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `sectoressector` WRITE;
/*!40000 ALTER TABLE `sectoressector` DISABLE KEYS */;
INSERT INTO `sectoressector` VALUES ('SICCIP','SICCIP','CME1','CME1',1),('SICCIP','SICCIP','CME3','CME3',1),('SICCIP','SICCIP','COMTE','COMTE',1),('SICCIP','SICCIP','COMTPUEN','COMTE',1),('SICCIP','SICCIP','COMTPUEN','PUENTE',0),('SICCIP','SICCIP','PUENTE','PUENTE',1),('SICCIP','SICCIP','RADIO1','RADIO1',1);
/*!40000 ALTER TABLE `sectoressector` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `sectoressectorizacion`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `sectoressectorizacion` WRITE;
/*!40000 ALTER TABLE `sectoressectorizacion` DISABLE KEYS */;
INSERT INTO `sectoressectorizacion` VALUES ('SICCIP','02/02/2011 15:00:52','SICCIP','PUENTE','PICT100'),('SICCIP','PC1','SICCIP','PUENTE','PICT100'),('SICCIP','02/02/2011 15:00:52','SICCIP','CME1','TV 02'),('SICCIP','PC1','SICCIP','CME1','TV 02'),('SICCIP','02/02/2011 15:00:52','SICCIP','RADIO1','TV 03'),('SICCIP','PC1','SICCIP','RADIO1','TV 03');
/*!40000 ALTER TABLE `sectoressectorizacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `sectorizaciones`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `sectorizaciones` WRITE;
/*!40000 ALTER TABLE `sectorizaciones` DISABLE KEYS */;
INSERT INTO `sectorizaciones` VALUES ('SICCIP','02/02/2011 15:00:52',0,'2011-02-02 15:00:52'),('SICCIP','PC1',1,'2011-02-02 15:00:52');
/*!40000 ALTER TABLE `sectorizaciones` ENABLE KEYS */;
UNLOCK TABLES;

/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `AltaSistema` AFTER INSERT ON `sistema` FOR EACH ROW BEGIN
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
    INSERT INTO Prefijos VALUES (new.IdSistema,32);
		
		INSERT INTO Redes VALUES (new.IdSistema,"ATS",3) ;    
END */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `tablasmodificadas`
--
-- WHERE:  IdSistema='SICCIP'


--
-- Dumping data for table `tareas`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `tareas` WRITE;
/*!40000 ALTER TABLE `tareas` DISABLE KEYS */;
/*!40000 ALTER TABLE `tareas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `teclassector`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `teclassector` WRITE;
/*!40000 ALTER TABLE `teclassector` DISABLE KEYS */;
INSERT INTO `teclassector` VALUES ('SICCIP','SICCIP','CME1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),('SICCIP','SICCIP','CME3',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),('SICCIP','SICCIP','COMTE',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),('SICCIP','SICCIP','PUENTE',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),('SICCIP','SICCIP','RADIO1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0),('SICCIP','SICCIP','RADIO2',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
/*!40000 ALTER TABLE `teclassector` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tifx`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `tifx` WRITE;
/*!40000 ALTER TABLE `tifx` DISABLE KEYS */;
INSERT INTO `tifx` VALUES ('SICCIP','CGW-01','A',0,'',161,161,162,5060,0,'192.168.1.229','192.168.2.229'),('SICCIP','CGW-02','A',0,'',161,161,162,5060,0,'192.168.1.224','192.168.2.224'),('SICCIP','CGW-03','A',0,'',161,161,162,5060,0,'192.168.1.223','192.168.2.223');
/*!40000 ALTER TABLE `tifx` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `TIFXDELETE` BEFORE DELETE ON `tifx` FOR EACH ROW UPDATE RECURSOS SET IdTIFX=NULL 
		WHERE IdSistema = old.IdSistema AND IdTIFX= old.IdTIFX */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `top`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `top` WRITE;
/*!40000 ALTER TABLE `top` DISABLE KEYS */;
INSERT INTO `top` VALUES ('SICCIP','PICT100',4,'A','192.168.1.100','192.168.2.100'),('SICCIP','TV 01',1,'A','192.168.10.1','192.168.20.1'),('SICCIP','TV 02',2,'A','192.168.10.2','192.168.20.2'),('SICCIP','TV 03',3,'A','192.168.10.3','192.168.20.3');
/*!40000 ALTER TABLE `top` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `troncales`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `troncales` WRITE;
/*!40000 ALTER TABLE `troncales` DISABLE KEYS */;
/*!40000 ALTER TABLE `troncales` ENABLE KEYS */;
UNLOCK TABLES;

DELIMITER ;;
/*!50003 SET SESSION SQL_MODE="NO_AUTO_VALUE_ON_ZERO" */;;
/*!50003 CREATE */ /*!50017 DEFINER=`root`@`localhost` */ /*!50003 TRIGGER `TRONCALDELETE` BEFORE DELETE ON `troncales` FOR EACH ROW UPDATE RECURSOSTF SET IdTroncal=NULL 
		WHERE IdTroncal=old.IdTroncal AND IdSistema=old.IdSistema */;;

DELIMITER ;
/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;

--
-- Dumping data for table `troncalesruta`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `troncalesruta` WRITE;
/*!40000 ALTER TABLE `troncalesruta` DISABLE KEYS */;
/*!40000 ALTER TABLE `troncalesruta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `usuariosabonados`
--
-- WHERE:  IdSistema='SICCIP'

LOCK TABLES `usuariosabonados` WRITE;
/*!40000 ALTER TABLE `usuariosabonados` DISABLE KEYS */;
INSERT INTO `usuariosabonados` VALUES ('SICCIP',3,'SICCIP','CME1','310001'),('SICCIP',3,'SICCIP','CME3','310002'),('SICCIP',3,'SICCIP','COMTE','310003'),('SICCIP',3,'SICCIP','PUENTE','310004'),('SICCIP',3,'SICCIP','RADIO1','310005'),('SICCIP',3,'SICCIP','RADIO2','310006');
/*!40000 ALTER TABLE `usuariosabonados` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2011-02-04 12:04:41
