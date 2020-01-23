-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.5.15


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
-- Temporary table structure for view `sectoresentopsparainformexml`
--
DROP TABLE IF EXISTS `sectoresentopsparainformexml`;
DROP VIEW IF EXISTS `sectoresentopsparainformexml`;
CREATE TABLE `sectoresentopsparainformexml` (
  `Idsistema` varchar(32),
  `IdSectorizacion` varchar(32),
  `IdToP` varchar(32),
  `IdSector` varchar(32),
  `IdNucleo` varchar(32),
  `IdSectorOriginal` varchar(32)
);

--
-- Temporary table structure for view `viewdestinostelefonia`
--
DROP TABLE IF EXISTS `viewdestinostelefonia`;
DROP VIEW IF EXISTS `viewdestinostelefonia`;
CREATE TABLE `viewdestinostelefonia` (
  `IdSistema` varchar(32),
  `IdDestino` varchar(32),
  `TipoDestino` int(11) unsigned,
  `IdGrupo` varchar(32),
  `IdPrefijo` int(11) unsigned,
  `IdAbonado` varchar(32),
  `IdRed` varchar(32)
);

--
-- Temporary table structure for view `viewincidenciasmasalarma`
--
DROP TABLE IF EXISTS `viewincidenciasmasalarma`;
DROP VIEW IF EXISTS `viewincidenciasmasalarma`;
CREATE TABLE `viewincidenciasmasalarma` (
  `IdSistema` varchar(32),
  `IdIncidencia` int(10) unsigned,
  `Incidencia` varchar(180),
  `alarma` int(4)
);

--
-- Temporary table structure for view `viewincidenciasmasalarma_ingles`
--
DROP TABLE IF EXISTS `viewincidenciasmasalarma_ingles`;
DROP VIEW IF EXISTS `viewincidenciasmasalarma_ingles`;
CREATE TABLE `viewincidenciasmasalarma_ingles` (
  `IdSistema` varchar(32),
  `IdIncidencia` int(10) unsigned,
  `Incidencia` varchar(180),
  `alarma` int(4)
);

--
-- Temporary table structure for view `viewrecursosimplicadosrutas`
--
DROP TABLE IF EXISTS `viewrecursosimplicadosrutas`;
DROP VIEW IF EXISTS `viewrecursosimplicadosrutas`;
CREATE TABLE `viewrecursosimplicadosrutas` (
  `idsistema` varchar(32),
  `idrecurso` varchar(32),
  `iddestino` varchar(32),
  `idred` varchar(32),
  `idtroncal` varchar(32),
  `idruta` varchar(32),
  `tipo` varchar(1)
);

--
-- Temporary table structure for view `viewsectoresenagrupacion`
--
DROP TABLE IF EXISTS `viewsectoresenagrupacion`;
DROP VIEW IF EXISTS `viewsectoresenagrupacion`;
CREATE TABLE `viewsectoresenagrupacion` (
  `contador` bigint(21),
  `IdAgrupacion` varchar(32)
);

--
-- Temporary table structure for view `viewsectoresentops`
--
DROP TABLE IF EXISTS `viewsectoresentops`;
DROP VIEW IF EXISTS `viewsectoresentops`;
CREATE TABLE `viewsectoresentops` (
  `IdSistema` varchar(32),
  `idsectorizacion` varchar(32),
  `IdTOP` varchar(32),
  `idsector` varchar(32),
  `idnucleo` varchar(32)
);
--
-- Create schema cd40_trans
--

CREATE DATABASE IF NOT EXISTS cd40_trans;
USE cd40_trans;

--
-- Temporary table structure for view `sectoresentopsparainformexml`
--
DROP TABLE IF EXISTS `sectoresentopsparainformexml`;
DROP VIEW IF EXISTS `sectoresentopsparainformexml`;
CREATE TABLE `sectoresentopsparainformexml` (
  `Idsistema` varchar(32),
  `IdSectorizacion` varchar(32),
  `IdToP` varchar(32),
  `IdSector` varchar(32),
  `IdNucleo` varchar(32),
  `IdSectorOriginal` varchar(32)
);

--
-- Temporary table structure for view `viewdestinostelefonia`
--
DROP TABLE IF EXISTS `viewdestinostelefonia`;
DROP VIEW IF EXISTS `viewdestinostelefonia`;
CREATE TABLE `viewdestinostelefonia` (
  `IdSistema` varchar(32),
  `IdDestino` varchar(32),
  `TipoDestino` int(11) unsigned,
  `IdGrupo` varchar(32),
  `IdPrefijo` int(11) unsigned,
  `IdAbonado` varchar(32),
  `IdRed` varchar(32)
);

--
-- Temporary table structure for view `viewincidenciasmasalarma`
--
DROP TABLE IF EXISTS `viewincidenciasmasalarma`;
DROP VIEW IF EXISTS `viewincidenciasmasalarma`;
CREATE TABLE `viewincidenciasmasalarma` (
  `IdSistema` varchar(32),
  `IdIncidencia` int(10) unsigned,
  `Incidencia` varchar(180),
  `alarma` int(4)
);

--
-- Temporary table structure for view `viewincidenciasmasalarma_ingles`
--
DROP TABLE IF EXISTS `viewincidenciasmasalarma_ingles`;
DROP VIEW IF EXISTS `viewincidenciasmasalarma_ingles`;
CREATE TABLE `viewincidenciasmasalarma_ingles` (
  `IdSistema` varchar(32),
  `IdIncidencia` int(10) unsigned,
  `Incidencia` varchar(180),
  `alarma` int(4)
);

--
-- Temporary table structure for view `viewrecursosimplicadosrutas`
--
DROP TABLE IF EXISTS `viewrecursosimplicadosrutas`;
DROP VIEW IF EXISTS `viewrecursosimplicadosrutas`;
CREATE TABLE `viewrecursosimplicadosrutas` (
  `idsistema` varchar(32),
  `idrecurso` varchar(32),
  `iddestino` varchar(32),
  `idred` varchar(32),
  `idtroncal` varchar(32),
  `idruta` varchar(32),
  `tipo` varchar(1)
);

--
-- Temporary table structure for view `viewsectoresenagrupacion`
--
DROP TABLE IF EXISTS `viewsectoresenagrupacion`;
DROP VIEW IF EXISTS `viewsectoresenagrupacion`;
CREATE TABLE `viewsectoresenagrupacion` (
  `contador` bigint(21),
  `IdAgrupacion` varchar(32)
);

--
-- Temporary table structure for view `viewsectoresentops`
--
DROP TABLE IF EXISTS `viewsectoresentops`;
DROP VIEW IF EXISTS `viewsectoresentops`;
CREATE TABLE `viewsectoresentops` (
  `IdSistema` varchar(32),
  `idsectorizacion` varchar(32),
  `IdTOP` varchar(32),
  `idsector` varchar(32),
  `idnucleo` varchar(32)
);
--
-- Create schema cd40
--

CREATE DATABASE IF NOT EXISTS cd40;
USE cd40;

--
-- Definition of table `abonados`
--

DROP TABLE IF EXISTS `abonados`;
CREATE TABLE `abonados` (
  `IdSistema` varchar(32) NOT NULL,
  `IdAbonado` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdAbonado`),
  KEY `ABONADOS_FKIndex1` (`IdSistema`),
  CONSTRAINT `abonados_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `agenda`
--

DROP TABLE IF EXISTS `agenda`;
CREATE TABLE `agenda` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `Prefijo` int(2) unsigned NOT NULL DEFAULT '0',
  `Numero` varchar(20) NOT NULL DEFAULT '',
  `Nombre` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`,`Prefijo`,`Numero`),
  CONSTRAINT `FK_SECTORES` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `agrupaciones`
--

DROP TABLE IF EXISTS `agrupaciones`;
CREATE TABLE `agrupaciones` (
  `IdSistema` varchar(32) NOT NULL,
  `IdAgrupacion` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdAgrupacion`),
  KEY `Agrupaciones_FKIndex1` (`IdSistema`),
  CONSTRAINT `agrupaciones_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `alarmas`
--

DROP TABLE IF EXISTS `alarmas`;
CREATE TABLE `alarmas` (
  `IdSistema` varchar(32) NOT NULL,
  `IdIncidencia` int(10) unsigned NOT NULL,
  `Alarma` tinyint(1) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdIncidencia`),
  KEY `Alarmas_FKIndex1` (`IdSistema`),
  KEY `Alarmas_FKIndex2` (`IdIncidencia`),
  CONSTRAINT `alarmas_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `alarmas_ibfk_2` FOREIGN KEY (`IdIncidencia`) REFERENCES `incidencias` (`IdIncidencia`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `altavoces`
--

DROP TABLE IF EXISTS `altavoces`;
CREATE TABLE `altavoces` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `NumAltavoz` int(10) unsigned NOT NULL,
  `Estado` varchar(1) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdNucleo`,`IdSector`,`PosHMI`,`NumAltavoz`),
  KEY `Altavoces_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdNucleo`,`IdSector`,`PosHMI`),
  CONSTRAINT `altavoces_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdNucleo`, `IdSector`, `PosHMI`) REFERENCES `destinosradiosector` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdNucleo`, `IdSector`, `PosHMI`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `controlbackup`
--

DROP TABLE IF EXISTS `controlbackup`;
CREATE TABLE `controlbackup` (
  `IdSistema` varchar(32) NOT NULL,
  `TipoBackup` int(11) NOT NULL,
  `Profundidad` int(11) unsigned NOT NULL,
  `Recurso` varchar(128) NOT NULL,
  PRIMARY KEY (`IdSistema`,`TipoBackup`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `destinos`
--

DROP TABLE IF EXISTS `destinos`;
CREATE TABLE `destinos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`),
  KEY `Destinos_FKIndex1` (`IdSistema`),
  CONSTRAINT `destinos_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of trigger `InsertDestino`
--

DROP TRIGGER /*!50030 IF EXISTS */ `InsertDestino`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `InsertDestino` AFTER INSERT ON `destinos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('Destinos');
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestino`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestino`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestino` AFTER UPDATE ON `destinos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('Destinos');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestino`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestino`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestino` BEFORE DELETE ON `destinos` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of table `destinosexternos`
--

DROP TABLE IF EXISTS `destinosexternos`;
CREATE TABLE `destinosexternos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `IdAbonado` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_44_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  CONSTRAINT `Table_44_FKIndex1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinostelefonia` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of trigger `AltaDestinosExternos`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinosExternos`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinosExternos` BEFORE INSERT ON `destinosexternos` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinosExternos`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinosExternos`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinosExternos` AFTER UPDATE ON `destinosexternos` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinosExternos`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinosExternos`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinosExternos` BEFORE DELETE ON `destinosexternos` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of table `destinosexternossector`
--

DROP TABLE IF EXISTS `destinosexternossector`;
CREATE TABLE `destinosexternossector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `PosHMI` int(10) unsigned DEFAULT NULL,
  `Prioridad` int(10) unsigned DEFAULT NULL,
  `OrigenR2` varchar(32) DEFAULT NULL,
  `PrioridadSIP` int(10) unsigned DEFAULT NULL,
  `TipoAcceso` varchar(2) NOT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_45_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_45_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `destinosexternossector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinosexternos` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `destinosexternossector_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `AltaDestinoExternoSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinoExternoSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinoExternoSector` AFTER INSERT ON `destinosexternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector');
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoExternoSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoExternoSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoExternoSector` AFTER UPDATE ON `destinosexternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinoExternoSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinoExternoSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinoExternoSector` BEFORE DELETE ON `destinosexternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector');
END $$

DELIMITER ;

--
-- Definition of table `destinosinternos`
--

DROP TABLE IF EXISTS `destinosinternos`;
CREATE TABLE `destinosinternos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_42_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_42_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `destinosinternos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinostelefonia` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `destinosinternos_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `AltaDestinoInterno`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinoInterno`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinoInterno` BEFORE INSERT ON `destinosinternos` FOR EACH ROW BEGIN
    INSERT INTO DestinosTelefonia SET IdSistema=new.IdSistema,
														 					IdDestino=new.IdDestino,
														 					TipoDestino=new.TipoDestino,
																			IdPrefijo=new.IdPrefijo;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos');
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoInterno`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoInterno`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoInterno` AFTER UPDATE ON `destinosinternos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinoInterno`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinoInterno`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinoInterno` BEFORE DELETE ON `destinosinternos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos');
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END $$

DELIMITER ;

--
-- Definition of table `destinosinternossector`
--

DROP TABLE IF EXISTS `destinosinternossector`;
CREATE TABLE `destinosinternossector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `Prioridad` int(1) unsigned DEFAULT NULL,
  `OrigenR2` varchar(32) DEFAULT NULL,
  `PrioridadSIP` int(1) unsigned DEFAULT NULL,
  `TipoAcceso` varchar(2) NOT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdNucleo`,`IdSector`,`IdPrefijo`),
  KEY `Table_43_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_43_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `destinosinternossector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinosinternos` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `destinosinternossector_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `AltaDestinoInternoSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinoInternoSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinoInternoSector` AFTER INSERT ON `destinosinternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoInternoSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoInternoSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoInternoSector` AFTER UPDATE ON `destinosinternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinosInternosSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinosInternosSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinosInternosSector` BEFORE DELETE ON `destinosinternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END $$

DELIMITER ;

--
-- Definition of table `destinosradio`
--

DROP TABLE IF EXISTS `destinosradio`;
CREATE TABLE `destinosradio` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `TipoFrec` int(1) unsigned DEFAULT NULL,
  `ExclusividadTXRX` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`),
  KEY `DestinosRadio_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`),
  CONSTRAINT `destinosradio_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`) REFERENCES `destinos` (`IdSistema`, `IdDestino`, `TipoDestino`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `AltaDestinoRadio`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinoRadio`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinoRadio` BEFORE INSERT ON `destinosradio` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoRadio`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoRadio`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoRadio` AFTER UPDATE ON `destinosradio` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadio');
END $$

DELIMITER ;

--
-- Definition of trigger `DESTRADIODELETE`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DESTRADIODELETE`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DESTRADIODELETE` BEFORE DELETE ON `destinosradio` FOR EACH ROW BEGIN
IF old.TipoDestino=0 THEN
	UPDATE RECURSOSRADIO SET IdDestino=NULL,TipoDestino=NULL 
  				WHERE IdSistema = old.IdSistema AND IdDestino=old.IdDestino AND TipoDestino=old.TipoDestino;
END IF ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadio');
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector') ;
END $$

DELIMITER ;

--
-- Definition of table `destinosradiosector`
--

DROP TABLE IF EXISTS `destinosradiosector`;
CREATE TABLE `destinosradiosector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `Prioridad` int(1) unsigned DEFAULT NULL,
  `PrioridadSIP` int(1) unsigned DEFAULT NULL,
  `ModoOperacion` varchar(1) DEFAULT NULL,
  `Cascos` varchar(1) DEFAULT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  `SupervisionPortadora` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdNucleo`,`IdSector`,`PosHMI`),
  KEY `DestinosRadioSector_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`),
  KEY `DestinosRadioSector_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `destinosradiosector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`) REFERENCES `destinosradio` (`IdSistema`, `IdDestino`, `TipoDestino`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `destinosradiosector_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of trigger `AltaDestinoRadioSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinoRadioSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinoRadioSector` AFTER INSERT ON `destinosradiosector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector');
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoRadioSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoRadioSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoRadioSector` AFTER UPDATE ON `destinosradiosector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinoRadioSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinoRadioSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinoRadioSector` BEFORE DELETE ON `destinosradiosector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector');
END $$

DELIMITER ;

--
-- Definition of table `destinostelefonia`
--

DROP TABLE IF EXISTS `destinostelefonia`;
CREATE TABLE `destinostelefonia` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `IdGrupo` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `DestinosTelefonia_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`),
  KEY `DestinosTelefonia_FKIndex2` (`IdSistema`,`IdGrupo`),
  KEY `DestinosTelefonia_FKIndex3` (`IdSistema`,`IdPrefijo`),
  CONSTRAINT `destinostelefonia_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`) REFERENCES `destinos` (`IdSistema`, `IdDestino`, `TipoDestino`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `destinostelefonia_ibfk_2` FOREIGN KEY (`IdSistema`, `IdGrupo`) REFERENCES `grupostelefonia` (`IdSistema`, `IdGrupo`) ON DELETE NO ACTION ON UPDATE CASCADE,
  CONSTRAINT `destinostelefonia_ibfk_3` FOREIGN KEY (`IdSistema`, `IdPrefijo`) REFERENCES `prefijos` (`IdSistema`, `IdPrefijo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `AltaDestinosTelefonia`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinosTelefonia`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinosTelefonia` BEFORE INSERT ON `destinostelefonia` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoTelefonia`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoTelefonia`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoTelefonia` AFTER UPDATE ON `destinostelefonia` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosTelefonia');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinoTelefonia`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinoTelefonia`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinoTelefonia` BEFORE DELETE ON `destinostelefonia` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosTelefonia') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternos') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector') ;
END $$

DELIMITER ;

--
-- Definition of table `emplazamientos`
--

DROP TABLE IF EXISTS `emplazamientos`;
CREATE TABLE `emplazamientos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdEmplazamiento` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdEmplazamiento`),
  KEY `Emplazamientos_FKIndex1` (`IdSistema`),
  CONSTRAINT `emplazamientos_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `PRDELETE`
--

DROP TRIGGER /*!50030 IF EXISTS */ `PRDELETE`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `PRDELETE` BEFORE DELETE ON `emplazamientos` FOR EACH ROW UPDATE RECURSOSRADIO SET IdEmplazamiento=NULL
			WHERE IdSistema = old.IdSistema AND IdEmplazamiento=old.IdEmplazamiento $$

DELIMITER ;

--
-- Definition of table `encaminamientos`
--

DROP TABLE IF EXISTS `encaminamientos`;
CREATE TABLE `encaminamientos` (
  `IdSistema` varchar(32) NOT NULL,
  `Central` varchar(32) NOT NULL,
  `CentralPropia` tinyint(1) DEFAULT '0',
  `Throwswitching` tinyint(1) DEFAULT '0',
  `NumTest` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`Central`),
  KEY `Encaminamientos_FKIndex1` (`IdSistema`),
  CONSTRAINT `encaminamientos_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `equiposeu`
--

DROP TABLE IF EXISTS `equiposeu`;
CREATE TABLE `equiposeu` (
  `IdSistema` varchar(32) NOT NULL,
  `idEquipos` varchar(32) NOT NULL,
  `IpRed1` varchar(60) DEFAULT NULL,
  `IpRed2` varchar(60) DEFAULT NULL,
  `TipoEquipo` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`idEquipos`),
  KEY `EquiposEU_FKIndex1` (`IdSistema`),
  CONSTRAINT `equiposeu_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `estadoaltavoces`
--

DROP TABLE IF EXISTS `estadoaltavoces`;
CREATE TABLE `estadoaltavoces` (
  `IdDestino` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `IdSistema` varchar(32) NOT NULL,
  `NumAltavoz` int(10) unsigned NOT NULL,
  `Estado` varchar(1) DEFAULT NULL,
  PRIMARY KEY (`IdDestino`,`PosHMI`,`IdSector`,`IdNucleo`,`IdSectorizacion`,`IdSistema`,`NumAltavoz`),
  KEY `Altavoz_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`PosHMI`,`IdDestino`),
  CONSTRAINT `estadoaltavoces_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`, `PosHMI`, `IdDestino`) REFERENCES `radio` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`, `PosHMI`, `IdDestino`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `estadorecursos`
--

DROP TABLE IF EXISTS `estadorecursos`;
CREATE TABLE `estadorecursos` (
  `IdDestino` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `Estado` varchar(1) DEFAULT NULL,
  PRIMARY KEY (`IdDestino`,`PosHMI`,`IdSector`,`IdNucleo`,`IdSectorizacion`,`IdSistema`,`IdRecurso`),
  KEY `Recurso_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`PosHMI`,`IdDestino`),
  CONSTRAINT `estadorecursos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`, `PosHMI`, `IdDestino`) REFERENCES `radio` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`, `PosHMI`, `IdDestino`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `estadosrecursos`
--

DROP TABLE IF EXISTS `estadosrecursos`;
CREATE TABLE `estadosrecursos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `Estado` varchar(1) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdSector`,`IdNucleo`,`PosHMI`,`IdRecurso`,`TipoRecurso`,`IdDestino`,`TipoDestino`),
  KEY `Table_44_FKIndex1` (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `Table_44_FKIndex2` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdNucleo`,`IdSector`,`PosHMI`),
  CONSTRAINT `estadosrecursos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRecurso`, `TipoRecurso`) REFERENCES `recursosradio` (`IdSistema`, `IdRecurso`, `TipoRecurso`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `estadosrecursos_ibfk_2` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdNucleo`, `IdSector`, `PosHMI`) REFERENCES `destinosradiosector` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdNucleo`, `IdSector`, `PosHMI`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `eventosradio`
--

DROP TABLE IF EXISTS `eventosradio`;
CREATE TABLE `eventosradio` (
  `IdSistema` varchar(32) NOT NULL,
  `Scv` int(1) DEFAULT '0',
  `TipoHw` int(1) NOT NULL,
  `IdHw` varchar(32) NOT NULL,
  `Usuario` varchar(32) NOT NULL,
  `UsuarioLogico` varchar(32) DEFAULT NULL COMMENT 'Si el TipoHw es una TOP, UsuarioLogico será null.',
  `IdIncidencia` int(10) unsigned DEFAULT NULL,
  `Evento` varchar(255) NOT NULL,
  `FechaHora` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `Duracion` time NOT NULL,
  PRIMARY KEY (`IdSistema`,`TipoHw`,`IdHw`,`FechaHora`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `eventostelefonia`
--

DROP TABLE IF EXISTS `eventostelefonia`;
CREATE TABLE `eventostelefonia` (
  `IdSistema` varchar(32) NOT NULL,
  `TipoHw` int(1) NOT NULL,
  `IdHw` varchar(32) NOT NULL,
  `UsuarioFisico` varchar(32) NOT NULL,
  `UsuarioLogico` varchar(32) DEFAULT NULL,
  `Evento` varchar(255) NOT NULL,
  `Acceso` varchar(10) DEFAULT NULL,
  `TipoInterfaz` varchar(5) DEFAULT NULL,
  `Red` varchar(32) DEFAULT NULL,
  `Troncal` varchar(32) DEFAULT NULL,
  `Servicio` varchar(32) DEFAULT NULL,
  `Destino` varchar(32) DEFAULT NULL,
  `Prioridad` varchar(5) DEFAULT NULL,
  `Ruta` varchar(32) DEFAULT NULL,
  `FechaHora` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `Duracion` time NOT NULL,
  PRIMARY KEY (`IdSistema`,`TipoHw`,`IdHw`,`FechaHora`),
  CONSTRAINT `FK_EventoTelefonia_Sistema` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


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
  `TipoAcceso` varchar(2) NOT NULL,
  `PosHMI` int(10) unsigned DEFAULT NULL,
  `Prioridad` int(10) unsigned DEFAULT NULL,
  `OrigenR2` varchar(32) DEFAULT NULL,
  `PrioridadSIP` int(10) unsigned DEFAULT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`IdDestino`,`IdPrefijo`,`TipoAcceso`),
  KEY `Table_41_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`),
  CONSTRAINT `externos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) REFERENCES `sectoressectorizacion` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `funciones`
--

DROP TABLE IF EXISTS `funciones`;
CREATE TABLE `funciones` (
  `IdSistema` varchar(32) NOT NULL,
  `Clave` int(11) NOT NULL,
  `Tipo` int(11) NOT NULL COMMENT '0: Radio; 1: Telefonía',
  `Funcion` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`Clave`),
  CONSTRAINT `FK_Sistema_Funciones` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `grupostelefonia`
--

DROP TABLE IF EXISTS `grupostelefonia`;
CREATE TABLE `grupostelefonia` (
  `IdSistema` varchar(32) NOT NULL,
  `IdGrupo` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdGrupo`),
  KEY `GruposTelefonia_FKIndex1` (`IdSistema`),
  CONSTRAINT `grupostelefonia_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `historicoincidencias`
--

DROP TABLE IF EXISTS `historicoincidencias`;
CREATE TABLE `historicoincidencias` (
  `IdSistema` varchar(32) NOT NULL,
  `Scv` int(1) NOT NULL DEFAULT '0',
  `IdHw` varchar(32) NOT NULL,
  `TipoHw` int(1) unsigned NOT NULL,
  `IdIncidencia` int(10) unsigned NOT NULL,
  `FechaHora` datetime NOT NULL,
  `Reconocida` datetime DEFAULT NULL,
  `Descripcion` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdHw`,`TipoHw`,`IdIncidencia`,`FechaHora`),
  KEY `HistoricoIncidencias_FKIndex1` (`IdIncidencia`),
  KEY `HistoricoIncidencias_FKIndex2` (`IdSistema`),
  CONSTRAINT `historicoincidencias_ibfk_1` FOREIGN KEY (`IdIncidencia`) REFERENCES `incidencias` (`IdIncidencia`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `historicoincidencias_ibfk_2` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `incidencias`
--

DROP TABLE IF EXISTS `incidencias`;
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

--
-- Dumping data for table `incidencias`
--

/*!40000 ALTER TABLE `incidencias` DISABLE KEYS */;
INSERT INTO `incidencias` (`IdIncidencia`,`IdIncidenciaCorrectora`,`Incidencia`,`Descripcion`,`GeneraError`,`OID`) VALUES 
 (96,NULL,'Cambio de día.','Cambio de día',NULL,NULL),
 (101,NULL,'Selección SCV','SCV {0} seleccionado.',NULL,NULL),
 (105,NULL,'Carga de sectorización.','Carga de sectorización {0}',NULL,NULL),
 (106,NULL,'Error carga sectorización.','Error carga sectorización {0}',0,NULL),
 (108,NULL,'Rechazo sectorización. No están todos los sectores reales.','Rechazo sectorización {0}. No están todos los sectores reales',0,NULL),
 (109,NULL,'Sectorización automática implantada.','Sectorización automática implantada',NULL,NULL),
 (110,NULL,'Sectorización automática rechazada.','Sectorización automática rechazada',0,NULL),
 (111,NULL,'Sector asignado a posición.','Sector {0} asignado a posición {1}',NULL,NULL),
 (112,NULL,'Sector desasignado de la posición.','Sector {0} desasignado de la posición {1}',NULL,NULL),
 (201,NULL,'Servidor 1 activo.','Servidor 1 activo',NULL,NULL),
 (202,201,'Servidor 1 caído.','Servidor 1 caído',0,NULL),
 (203,NULL,'Servidor 2 activo.','Servidor 2 activo',NULL,NULL),
 (204,203,'Servidor 2 caído.','Servidor 2 caído',0,NULL),
 (1001,NULL,'Entrada TOP.','Entrada TOP',1,'1.1.1000.0'),
 (1002,1001,'Caída TOP.','Caída TOP',1,'1.1.1000.0'),
 (1003,NULL,'Conexión jacks ejecutivo.','Conexión jacks ejecutivo. Posición: {0}',0,'1.1.1000.1.3.0'),
 (1004,1003,'Desconexión jacks ejecutivo.','Desconexión jacks ejecutivo. Posición: {0}',0,'1.1.1000.1.3.0'),
 (1005,NULL,'Conexión jacks ayudante.','Conexión jacks ayudante. Posición: {0}',0,'1.1.1000.1.3.1'),
 (1006,1005,'Desconexión jacks ayudante.','Desconexión jacks ayudante. Posición: {0} ',0,'1.1.1000.1.3.1'),
 (1007,NULL,'Conexión altavoz.','Conexión altavoz. Posición: {0} ',1,'1.1.1000.1.2'),
 (1008,1007,'Desconexión altavoz.','Desconexión altavoz. Posición: {0} ',1,'1.1.1000.1.2'),
 (1009,NULL,'Panel pasa a operación.','Panel pasa a operación. Posición: {0} ',1,'1.1.6.0'),
 (1010,1009,'Panel pasa a standby.','Panel pasa a standby. Posición: {0} ',1,'1.1.6.0'),
 (1011,NULL,'Página de frecuencias seleccionada.','Página de frecuencias seleccionada: {0}. Posición: {1}. Sector: {2}',NULL,NULL),
 (1012,NULL,'Selección recurso radio.','Selección recurso radio: {0}. Frecuencia: {1}. Posicion: {2}. Sector: {3}',NULL,NULL),
 (1013,NULL,'Estado selección.','Estado selección: {0}. Frecuencia: {1}. Posicion {2}. Sector: {3}',NULL,NULL),
 (1014,NULL,'Estado PTT.','Estado PTT: {0}. Posicion:{1}. Sector{2}',NULL,'1.1.1000.2'),
 (1015,NULL,'Facilidad seleccionada.','Facilidad seleccionada: {0}. Posición: {1}. Sector: {2}',NULL,NULL),
 (1016,NULL,'Llamada entrante Posicion.','Llamada entrante Posicion: {0}. Acceso: {1}. Destino: {2}. Prioridad: {3}',NULL,NULL),
 (1017,NULL,'Llamada saliente Posicion.','Llamada saliente Posicion: {0}. Acceso: {1}. Destino: {2}. Prioridad: {3}',NULL,NULL),
 (1018,NULL,'Estado Recepción.','Estado Recepción: {0}. Posicion:{1}. Sector{2}',0,NULL),
 (1019,NULL,'Fin llamada saliente Posicion.','Fin llamada saliente Posicion: {0}. Acceso: {1}. Destino: {2}. Prioridad: {3}',NULL,NULL),
 (2001,NULL,'Entrada GW.','Entrada GW',1,'1.1.100.2.0'),
 (2002,2001,'Caída GW.','Caída GW.',1,'1.1.100.2.0'),
 (2003,NULL,'Conexión Recurso Radio.','Conexión Recurso Radio. Pasarela: {0}. Recurso: {1}',NULL,'1.1.200.3.1.17'),
 (2004,2003,'Desconexión Recurso Radio.','Desconexión Recurso Radio.  Pasarela: {0}. Recurso: {1}',0,'1.1.200.3.1.17'),
 (2005,NULL,'Conexión Recurso Telefonía.','Conexión Recurso Telefonía.  Pasarela: {0}. Recurso: {1}',NULL,'1.1.400.3.1.14'),
 (2006,2005,'Desconexión Recurso Telefonía.','Desconexión Recurso Telefonía. Pasarela: {0}. Recurso: {1}',0,'1.1.400.3.1.14'),
 (2007,NULL,'Conexión tarjeta de interfaz','Conexión tarjeta de interfaz: Slot {0}',1,'1.1.100.31.1.2'),
 (2008,2007,'Desconexión tarjeta de interfaz','Desconexión tarjeta de interfaz: Slot {0}',1,'1.1.100.31.1.2'),
 (2009,NULL,'Conexión Recurso R2.','Conexión Recurso R2.  Pasarela: {0}. Recurso: {1}',NULL,'1.1.500.3.1.17'),
 (2010,NULL,'Desconexión Recurso R2.','Desconexión Recurso R2.  Pasarela: {0}. Recurso: {1}',NULL,'1.1.500.3.1.17'),
 (2012,NULL,'Error protocolo LCN.','Error protocolo LCN.  Pasarela: {0}. Recurso: {1}',NULL,'1.1.300.3.1.17'),
 (2013,NULL,'Conexión Recurso LCN.','Conexión Recurso LCN.  Pasarela: {0}. Recurso: {1}',NULL,'1.1.300.3.1.17'),
 (2014,2013,'Desconexión Recurso LCN.','Desconexión Recurso LCN.  Pasarela: {0}. Recurso: {1}',0,'1.1.300.3.1.17'),
 (2020,NULL,'Llamada entrante R2.','Llamada entrante R2. Recurso {0}. Troncal {1}. Origen {2}. Prioridad {3}. Destino {4}',NULL,'1.1.500.3.1.17'),
 (2021,NULL,'Fin llamada entrante R2.','Fin llamada entrante R2. Recurso {0}. Troncal {1}.',NULL,'1.1.500.3.1.17'),
 (2022,NULL,'Llamada saliente R2.','Llamada saliente R2. Recurso {0}. Troncal {1}. Origen {2}. Prioridad {3}. Destino {4}',NULL,'1.1.500.3.1.17'),
 (2023,NULL,'Fin llamada saliente R2.','Fin llamada saliente R2. Recurso {0}. Troncal {1}.',NULL,'1.1.500.3.1.17'),
 (2024,NULL,'Llamada prueba R2.','Llamada prueba R2. Pasarela: {0}. Recurso: {1}',NULL,'1.1.500.3.1.17'),
 (2025,NULL,'Error protocolo R2.','Error protocolo R2. Pasarela: {0}. Recurso: {1}',0,'1.1.500.3.1.17'),
 (2030,NULL,'Llamada entrante LCN.','Llamada entrante LCN: Recurso: {0}.',NULL,'1.1.300.3.1.17'),
 (2031,NULL,'Fin llamada entrante LCN.','Fin llamada entrante LCN: Recurso: {0}.',NULL,'1.1.300.3.1.17'),
 (2032,NULL,'Llamada saliente LCN.','Llamada saliente LCN: Recurso: {0}.',NULL,'1.1.300.3.1.17'),
 (2033,NULL,'Fin llamada saliente LCN.','Fin llamada saliente LCN: Recurso: {0}.',NULL,'1.1.300.3.1.17'),
 (2040,NULL,'Llamada entrante telefonía.','Llamada entrante telefonía: Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2041,NULL,'Fin llamada entrante telefonía.','Fin llamada entrante telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2042,NULL,'Llamada saliente telefonía.','Llamada saliente telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2043,NULL,'Fin llamada saliente telefonía.','Fin llamada saliente telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2050,NULL,'PTT On.','PTT On:  Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (2051,NULL,'PTT Off.','PTT Off:   Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (2052,NULL,'SQ On.','SQ On:   Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (2053,NULL,'SQ Off.','SQ Off:   Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (3001,NULL,'Entrada Equipo Externo.','Entrada Equipo Externo',1,NULL),
 (3002,3001,'Caída Equipo Externo.','Caída Equipo Externo',1,NULL);
/*!40000 ALTER TABLE `incidencias` ENABLE KEYS */;

--
-- Definition of table `incidencias_ingles`
--

DROP TABLE IF EXISTS `incidencias_ingles`;
CREATE TABLE `incidencias_ingles` (
  `IdIncidencia` int(10) unsigned NOT NULL,
  `IdIncidenciaCorrectora` int(10) unsigned DEFAULT NULL,
  `Incidencia` varchar(180) DEFAULT NULL,
  `Descripcion` varchar(180) DEFAULT NULL,
  `GeneraError` tinyint(1) DEFAULT NULL,
  `OID` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdIncidencia`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `incidencias_ingles`
--

/*!40000 ALTER TABLE `incidencias_ingles` DISABLE KEYS */;
INSERT INTO `incidencias_ingles` (`IdIncidencia`,`IdIncidenciaCorrectora`,`Incidencia`,`Descripcion`,`GeneraError`,`OID`) VALUES 
 (96,NULL,'Change of day.','Change of day',NULL,NULL),
 (105,NULL,'Sectorization load','Sectorization load {0}',NULL,NULL),
 (106,NULL,'Error in sectorization load.','Error sectorization load {0}',1,NULL),
 (108,NULL,'Rejection in sectorización. All real sectors are not in sectorization.','Reject in sectorization {0}. All real sectors are not in sectorization',1,NULL),
 (109,NULL,'Loaded automatic sectorization.','Loaded automatic sectorization',NULL,NULL),
 (110,NULL,'Rejected automatic sectorization.','Rejected automatic sectorization',1,NULL),
 (111,NULL,'Position assigned to the sector.','Position {1} assigned to the sector {0}',NULL,NULL),
 (112,NULL,'Position unassigned to the sector.','Position {1} unassigned to the sector {0}',NULL,NULL),
 (201,NULL,'Server 1 active.','Server 1 active',NULL,NULL),
 (202,201,'Server 1 down.','Server 1 down.',1,NULL),
 (203,NULL,'Server 2 active','Server 2 active',NULL,NULL),
 (204,203,'Server 2 down','Server 2 down',1,NULL),
 (1001,NULL,'Entering TOP.','Entering TOP. Position: {0}',NULL,'1.1.1000.0'),
 (1002,1001,'TOP down.','TOP down. Position: {0}',1,'1.1.1000.0'),
 (1003,NULL,'Executive jacks on.','Executive jacks on. Position: {0}',NULL,'1.1.1000.1.3.0'),
 (1004,1003,'Executive jacks off.','Executive jacks off. Position: {0}',1,'1.1.1000.1.3.0'),
 (1005,NULL,'Assistant jacks on.','Assistant jacks on. Position: {0}',NULL,'1.1.1000.1.3.1'),
 (1006,1005,'Assistant jacks off.','Assistant jacks off. Position: {0}',1,'1.1.1000.1.3.1'),
 (1007,NULL,'Speaker on.','Speaker on. Position: {0}',-4,'1.1.1000.1.2'),
 (1008,1007,'Speaker off','Speaker off. Position: {0}',0,'1.1.1000.1.2'),
 (1009,NULL,'Panel to operation','Panel to operation. Position: {0}',-1,'1.1.1000.1.4'),
 (1010,1009,'Panel to standby.','Panel to standby. Position: {0}',1,'1.1.1000.1.4'),
 (1011,NULL,'Frequency page selected','Frequency page selected: {0}. Position: {1}. Sector: {2}',NULL,NULL),
 (1012,NULL,'Radio resource selection.','Radio resource selection: {0}. Frequency: {1}. Position: {2}. Sector: {3}',NULL,NULL),
 (1013,NULL,'Selection status','Selection status: {0}. Frequency: {1}. Position {2}. Sector: {3}',NULL,NULL),
 (1014,NULL,'PTT status.','PTT status: {0}. Position:{1}. PTT Type: {2}',NULL,'1.1.1000.2'),
 (1015,NULL,'Selected function','Selected function: {0}. Position: {1}. Sector: {2}',NULL,NULL),
 (1016,NULL,'Incomming call Position.','Incomming call Position: {0}. Access: {1}. Destination: {2}. Priority: {3}',NULL,NULL),
 (1017,NULL,'Outgoing call Position.','Outgoing call Position: {0}. Access: {1}. Destination: {2}. Priority: {3}',NULL,NULL),
 (1018,NULL,'Reception status.','Reception status: {0}. Position:{1}. Sector{2}',0,NULL),
 (1019,NULL,'End of outgoing call Position.','End of outgoing call Position: {0}. Access: {1}. Destination: {2}. Priority: {3}',NULL,NULL),
 (2001,NULL,'Entering GW.','Entering GW.',NULL,'1.1.100.2.0'),
 (2002,2001,'GW down.','GW down.',1,'1.1.100.2.0'),
 (2003,NULL,'Radio resource on.','Radio resource on.',NULL,'1.1.200.3.1.17'),
 (2004,2003,'Radio resource off.','Radio resource off.',1,'1.1.200.3.1.17'),
 (2005,NULL,'Telephone resource on.','Telephone resource on.',NULL,'1.1.400.3.1.14'),
 (2006,2005,'Telephone resource off.','Telephone resource off.',1,'1.1.400.3.1.14'),
 (2009,NULL,'R2 resource on.','R2 resource on.',NULL,'1.1.500.3.1.17'),
 (2010,NULL,'R2 resource off.','R2 resource off.',NULL,'1.1.500.3.1.17'),
 (2012,NULL,'Protocol LCN error.','Protocol LCN error',NULL,'1.1.300.3.1.17'),
 (2013,NULL,'LCN resource on.','LCN resource on.',NULL,'1.1.300.3.1.17'),
 (2014,2013,'LCN resource off.','LCN resource off.',1,'1.1.300.3.1.17'),
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
 (2050,NULL,'PTT On.','PTT On:  Gateway: {0}. Resource {1}. Frequency: {2}',NULL,'1.1.200.3.1.17'),
 (2051,NULL,'PTT Off.','PTT Off:  Gateway: {0}. Resource {1}. Frequency: {2}',NULL,'1.1.200.3.1.17'),
 (2052,NULL,'SQ On.','SQ On:  Gateway: {0}. Resource {1}. Frequency: {2}',NULL,'1.1.200.3.1.17'),
 (2053,NULL,'SQ Off.','SQ Off:  Gateway: {0}. Resource {1}. Frequency: {2}',NULL,'1.1.200.3.1.17'),
 (3001,NULL,'Entering external equipment','Entering external equipment',NULL,NULL),
 (3002,3001,'External equipment down','External equipment down',1,NULL);
/*!40000 ALTER TABLE `incidencias_ingles` ENABLE KEYS */;


--
-- Definition of table `indicadores`
--

DROP TABLE IF EXISTS `indicadores`;
CREATE TABLE `indicadores` (
  `IdSistema` varchar(32) NOT NULL,
  `FamiliaIndicador` int(11) NOT NULL,
  `Indicador` int(11) NOT NULL,
  `ElementoFisico` varchar(32) NOT NULL,
  `ElementoLogico` varchar(32) DEFAULT NULL,
  `Destino` varchar(32) DEFAULT NULL,
  `Red` varchar(32) DEFAULT NULL,
  `Troncal` varchar(32) DEFAULT NULL,
  `TipoInterfaz` varchar(5) DEFAULT NULL,
  `Prioridad` varchar(5) DEFAULT NULL,
  `Ruta` varchar(32) DEFAULT NULL,
  `Acceso` varchar(1) DEFAULT NULL,
  `FechaHora` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `Duracion` time NOT NULL,
  PRIMARY KEY (`IdSistema`,`FamiliaIndicador`,`Indicador`,`ElementoFisico`,`FechaHora`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

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
  `TipoAcceso` varchar(2) NOT NULL,
  `PosHMI` int(10) unsigned DEFAULT NULL,
  `Prioridad` int(10) unsigned DEFAULT NULL,
  `OrigenR2` varchar(32) DEFAULT NULL,
  `PrioridadSIP` int(10) unsigned DEFAULT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`IdDestino`,`IdPrefijo`,`TipoAcceso`),
  KEY `Table_40_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`),
  CONSTRAINT `internos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) REFERENCES `sectoressectorizacion` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `niveles`
--

DROP TABLE IF EXISTS `niveles`;
CREATE TABLE `niveles` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `CICL` int(10) unsigned DEFAULT '0',
  `CIPL` int(10) unsigned DEFAULT '0',
  `CPICL` int(10) unsigned DEFAULT '0',
  `CPIPL` int(10) unsigned DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `Niveles_FKIndex1` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `niveles_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `nucleos`
--

DROP TABLE IF EXISTS `nucleos`;
CREATE TABLE `nucleos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdNucleo`),
  KEY `Nucleos_FKIndex1` (`IdSistema`),
  CONSTRAINT `nucleos_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `operadores`
--

DROP TABLE IF EXISTS `operadores`;
CREATE TABLE `operadores` (
  `IdOperador` varchar(32) NOT NULL,
  `IdSistema` varchar(32) NOT NULL,
  `Clave` varchar(10) DEFAULT NULL,
  `NivelAcceso` int(1) unsigned DEFAULT NULL,
  `Nombre` varchar(32) DEFAULT NULL,
  `Apellidos` varchar(32) DEFAULT NULL,
  `Telefono` varchar(32) DEFAULT NULL,
  `FechaUltAcceso` date DEFAULT NULL,
  `Comentarios` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`IdOperador`,`IdSistema`),
  KEY `Operadores_FKIndex1` (`IdSistema`),
  CONSTRAINT `operadores_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `parametrosrecurso`
--

DROP TABLE IF EXISTS `parametrosrecurso`;
CREATE TABLE `parametrosrecurso` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL,
  `GananciaAGCTX` int(1) unsigned DEFAULT '0',
  `GananciaAGCTXdBm` int(11) DEFAULT '0',
  `GananciaAGCRX` int(1) unsigned DEFAULT '0',
  `GananciaAGCRXdBm` int(11) DEFAULT '0',
  `SupresionSilencio` tinyint(1) DEFAULT '1',
  `TamRTP` int(10) unsigned DEFAULT '20',
  `Codec` int(1) unsigned DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `ParametrosRecurso_FKIndex1` (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  CONSTRAINT `parametrosrecurso_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRecurso`, `TipoRecurso`) REFERENCES `recursos` (`IdSistema`, `IdRecurso`, `TipoRecurso`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `parametrossector`
--

DROP TABLE IF EXISTS `parametrossector`;
CREATE TABLE `parametrossector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `NumLlamadasEntrantesIDA` int(10) unsigned DEFAULT '5',
  `NumLlamadasEnIDA` int(10) unsigned DEFAULT '10',
  `NumFreqPagina` int(10) unsigned DEFAULT NULL,
  `NumPagFreq` int(10) unsigned DEFAULT NULL,
  `NumDestinosInternosPag` int(10) unsigned DEFAULT NULL,
  `NumPagDestinosInt` int(10) unsigned DEFAULT NULL,
  `Intrusion` tinyint(1) DEFAULT '1',
  `Intruido` tinyint(1) DEFAULT '1',
  `KeepAlivePeriod` int(10) unsigned DEFAULT '200',
  `KeepAliveMultiplier` int(10) unsigned DEFAULT '10',
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `ParametrosSector_FKIndex1` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `parametrossector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `permisosredes`
--

DROP TABLE IF EXISTS `permisosredes`;
CREATE TABLE `permisosredes` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRed` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `Llamar` tinyint(1) DEFAULT NULL,
  `Recibir` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdRed`,`IdSector`,`IdNucleo`),
  KEY `PermisosRedes_FKIndex1` (`IdSistema`,`IdRed`),
  KEY `PermisosRedes_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `permisosredes_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRed`) REFERENCES `redes` (`IdSistema`, `IdRed`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `permisosredes_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `prefijos`
--

DROP TABLE IF EXISTS `prefijos`;
CREATE TABLE `prefijos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdPrefijo`),
  KEY `Prefijos_FKIndex1` (`IdSistema`),
  CONSTRAINT `fk_prefijos_sistema` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `radio`
--

DROP TABLE IF EXISTS `radio`;
CREATE TABLE `radio` (
  `IdSistema` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `Prioridad` int(1) unsigned DEFAULT NULL,
  `PrioridadSIP` int(1) unsigned DEFAULT NULL,
  `ModoOperacion` varchar(1) DEFAULT NULL,
  `Cascos` varchar(1) DEFAULT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  `SupervisionPortadora` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`PosHMI`,`IdDestino`),
  KEY `Table_42_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`),
  CONSTRAINT `radio_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) REFERENCES `sectoressectorizacion` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `rangos`
--

DROP TABLE IF EXISTS `rangos`;
CREATE TABLE `rangos` (
  `IdSistema` varchar(32) NOT NULL,
  `Central` varchar(32) NOT NULL,
  `Tipo` varchar(1) NOT NULL,
  `Inicial` varchar(6) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `IdAbonado` varchar(32) NOT NULL,
  `Final` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`Tipo`,`Central`,`Inicial`),
  KEY `Rangos_FKIndex2` (`IdSistema`,`IdPrefijo`),
  KEY `Rangos_FKIndex4` (`IdSistema`,`Central`),
  CONSTRAINT `FK_Encaminamientos` FOREIGN KEY (`IdSistema`, `Central`) REFERENCES `encaminamientos` (`IdSistema`, `Central`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `recursos`
--

DROP TABLE IF EXISTS `recursos`;
CREATE TABLE `recursos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL DEFAULT '1',
  `idEquipos` varchar(32) DEFAULT NULL,
  `IdTIFX` varchar(32) DEFAULT NULL,
  `Tipo` int(1) unsigned NOT NULL DEFAULT '0',
  `Interface` int(2) unsigned DEFAULT '0',
  `SlotPasarela` int(1) DEFAULT '-1',
  `NumDispositivoSlot` int(1) DEFAULT '-1',
  `ServidorSIP` varchar(32) DEFAULT '0.0.0.0:5060',
  `Diffserv` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RECURSOS_FKIndex1` (`IdSistema`),
  KEY `RECURSOS_FKIndex2` (`IdSistema`,`IdTIFX`),
  KEY `RECURSOS_FKIndex3` (`idEquipos`,`IdSistema`),
  CONSTRAINT `recursos_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `recursos_ibfk_2` FOREIGN KEY (`IdSistema`, `IdTIFX`) REFERENCES `tifx` (`IdSistema`, `IdTIFX`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `recursoslcen`
--

DROP TABLE IF EXISTS `recursoslcen`;
CREATE TABLE `recursoslcen` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned DEFAULT NULL,
  `IdDestino` varchar(32) DEFAULT NULL,
  `TipoDestino` int(10) unsigned DEFAULT NULL,
  `T1` int(10) unsigned DEFAULT '20',
  `T1Max` int(10) unsigned DEFAULT '100',
  `T2` int(10) unsigned DEFAULT '20',
  `T2Max` int(10) unsigned DEFAULT '100',
  `T3` int(10) unsigned DEFAULT '40',
  `T4` int(10) unsigned DEFAULT '40',
  `T4Max` int(10) unsigned DEFAULT '160',
  `T5` int(10) unsigned DEFAULT '60',
  `T5Max` int(10) unsigned DEFAULT '100',
  `T6` int(10) unsigned DEFAULT '5000',
  `T6Max` int(10) unsigned DEFAULT '6000',
  `T8` int(10) unsigned DEFAULT '80',
  `T8Max` int(10) unsigned DEFAULT '150',
  `T9` int(10) unsigned DEFAULT '40',
  `T9Max` int(10) unsigned DEFAULT '60',
  `T10` int(10) unsigned DEFAULT '20',
  `T10Max` int(10) unsigned DEFAULT '130',
  `T11` int(10) unsigned DEFAULT '20',
  `T11Max` int(10) unsigned DEFAULT '130',
  `T12` int(10) unsigned DEFAULT '200',
  `FrqTonoSQ` int(10) unsigned DEFAULT '2280',
  `UmbralTonoSQ` int(11) DEFAULT '-35',
  `FrqTonoPTT` int(10) unsigned DEFAULT '2280',
  `UmbralTonoPTT` int(11) DEFAULT '-10',
  `RefrescoEstados` int(10) unsigned DEFAULT '200',
  `Timeout` int(10) unsigned DEFAULT '600',
  `LongRafagas` int(10) unsigned DEFAULT '6',
  PRIMARY KEY (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosLCEN_FKIndex1` (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosLCEN_FKIndex2` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  CONSTRAINT `recursoslcen_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRecurso`, `TipoRecurso`) REFERENCES `recursos` (`IdSistema`, `IdRecurso`, `TipoRecurso`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `recursoslcen_ibfk_2` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinosexternos` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `recursosradio`
--

DROP TABLE IF EXISTS `recursosradio`;
CREATE TABLE `recursosradio` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL,
  `TipoDestino` int(10) unsigned DEFAULT NULL,
  `IdDestino` varchar(32) DEFAULT NULL,
  `IdEmplazamiento` varchar(32) NOT NULL,
  `EM` tinyint(1) DEFAULT NULL,
  `SQ` varchar(1) DEFAULT NULL,
  `PTT` varchar(1) DEFAULT NULL,
  `FrqTonoE` int(10) unsigned DEFAULT NULL,
  `UmbralTonoE` int(11) DEFAULT NULL,
  `FrqTonoM` int(10) unsigned DEFAULT NULL,
  `UmbralTonoM` int(11) DEFAULT NULL,
  `FrqTonoSQ` int(10) unsigned DEFAULT NULL,
  `UmbralTonoSQ` int(11) DEFAULT NULL,
  `FrqTonoPTT` int(10) unsigned DEFAULT NULL,
  `UmbralTonoPTT` int(11) DEFAULT NULL,
  `BSS` tinyint(1) DEFAULT NULL,
  `NTZ` tinyint(1) DEFAULT NULL,
  `TipoNTZ` int(1) unsigned DEFAULT '0',
  `Cifrado` tinyint(1) DEFAULT NULL,
  `SupervPortadoraTX` tinyint(1) DEFAULT NULL,
  `SupervModuladoraTX` tinyint(1) DEFAULT NULL,
  `ModoConfPTT` int(1) unsigned DEFAULT NULL,
  `RepSQyBSS` int(10) unsigned DEFAULT '1',
  `DesactivacionSQ` int(10) unsigned DEFAULT '1',
  `TimeoutPTT` int(10) unsigned DEFAULT '200',
  `MetodoBSS` int(1) unsigned DEFAULT NULL,
  `UmbralVAD` int(11) DEFAULT '-33',
  `TiempoPTT` int(10) unsigned DEFAULT '120',
  `NumFlujosAudio` int(1) unsigned DEFAULT '1',
  `KeepAlivePeriod` int(10) unsigned DEFAULT '200',
  `KeepAliveMultiplier` int(10) unsigned DEFAULT '10',
  PRIMARY KEY (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosRadio_FKIndex1` (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosRadio_FKIndex2` (`IdSistema`,`IdEmplazamiento`),
  KEY `RecursosRadio_FKIndex3` (`IdSistema`,`IdDestino`,`TipoDestino`),
  CONSTRAINT `recursosradio_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRecurso`, `TipoRecurso`) REFERENCES `recursos` (`IdSistema`, `IdRecurso`, `TipoRecurso`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `recursosradio_ibfk_2` FOREIGN KEY (`IdSistema`, `IdEmplazamiento`) REFERENCES `emplazamientos` (`IdSistema`, `IdEmplazamiento`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `recursosradio_ibfk_3` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`) REFERENCES `destinosradio` (`IdSistema`, `IdDestino`, `TipoDestino`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `recursostf`
--

DROP TABLE IF EXISTS `recursostf`;
CREATE TABLE `recursostf` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned DEFAULT NULL,
  `TipoDestino` int(10) unsigned DEFAULT NULL,
  `IdDestino` varchar(32) DEFAULT NULL,
  `IdTroncal` varchar(32) DEFAULT NULL,
  `IdRed` varchar(32) DEFAULT NULL,
  `Lado` varchar(1) NOT NULL DEFAULT 'A',
  `Modo` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosTF_FKIndex1` (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosTF_FKIndex2` (`IdSistema`,`IdRed`),
  KEY `RecursosTF_FKIndex3` (`IdSistema`,`IdTroncal`),
  KEY `RecursosTF_FKIndex4` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  CONSTRAINT `recursostf_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRecurso`, `TipoRecurso`) REFERENCES `recursos` (`IdSistema`, `IdRecurso`, `TipoRecurso`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `recursostf_ibfk_2` FOREIGN KEY (`IdSistema`, `IdRed`) REFERENCES `redes` (`IdSistema`, `IdRed`) ON DELETE NO ACTION ON UPDATE CASCADE,
  CONSTRAINT `recursostf_ibfk_3` FOREIGN KEY (`IdSistema`, `IdTroncal`) REFERENCES `troncales` (`IdSistema`, `IdTroncal`) ON DELETE NO ACTION ON UPDATE CASCADE,
  CONSTRAINT `recursostf_ibfk_4` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinosexternos` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `redes`
--

DROP TABLE IF EXISTS `redes`;
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

--
-- Definition of trigger `REDDELETE`
--

DROP TRIGGER /*!50030 IF EXISTS */ `REDDELETE`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `REDDELETE` BEFORE DELETE ON `redes` FOR EACH ROW UPDATE RECURSOSTF SET IdRed=NULL 
	WHERE IdRed=old.IdRed AND IdSistema=old.IdSistema $$

DELIMITER ;

--
-- Definition of table `registrobackup`
--

DROP TABLE IF EXISTS `registrobackup`;
CREATE TABLE `registrobackup` (
  `IdSistema` varchar(32) NOT NULL,
  `TipoBackup` int(11) NOT NULL,
  `FechaInicio` datetime NOT NULL,
  `FechaFin` datetime NOT NULL,
  `RecursoDestino` varchar(128) NOT NULL,
  PRIMARY KEY (`IdSistema`,`TipoBackup`,`FechaInicio`),
  CONSTRAINT `FK_RegistroBackup_Sistema` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `registrotareas`
--

DROP TABLE IF EXISTS `registrotareas`;
CREATE TABLE `registrotareas` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTarea` varchar(32) NOT NULL,
  `Ejecutada` datetime NOT NULL,
  `Resultado` int(11) NOT NULL,
  `Comentario` varchar(125) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdTarea`,`Ejecutada`),
  CONSTRAINT `Tareas` FOREIGN KEY (`IdSistema`, `IdTarea`) REFERENCES `tareas` (`IdSistema`, `IdTarea`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `rutas`
--

DROP TABLE IF EXISTS `rutas`;
CREATE TABLE `rutas` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRuta` varchar(32) NOT NULL,
  `Central` varchar(32) NOT NULL,
  `Tipo` varchar(1) DEFAULT NULL,
  `Orden` int(11) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdRuta`,`Central`),
  KEY `Rutas_FKIndex1` (`IdSistema`,`Central`),
  CONSTRAINT `rutas_ibfk_1` FOREIGN KEY (`IdSistema`, `Central`) REFERENCES `encaminamientos` (`IdSistema`, `Central`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `sectores`
--

DROP TABLE IF EXISTS `sectores`;
CREATE TABLE `sectores` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdSistemaParejaUCS` varchar(32) DEFAULT NULL,
  `IdNucleoParejaUCS` varchar(32) DEFAULT NULL,
  `IdParejaUCS` varchar(32) DEFAULT NULL,
  `SectorSimple` tinyint(1) DEFAULT '1',
  `Tipo` varchar(1) DEFAULT 'R',
  `TipoPosicion` varchar(1) DEFAULT 'C',
  `PrioridadR2` int(1) unsigned DEFAULT '4',
  `TipoHMI` int(10) unsigned DEFAULT NULL,
  `NumSacta` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `Sectores_FKIndex1` (`IdSistema`,`IdNucleo`),
  KEY `Sectores_FKIndex3` (`IdSistemaParejaUCS`,`IdNucleoParejaUCS`,`IdParejaUCS`),
  CONSTRAINT `sectores_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`) REFERENCES `nucleos` (`IdSistema`, `IdNucleo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `sectores_ibfk_3` FOREIGN KEY (`IdSistemaParejaUCS`, `IdNucleoParejaUCS`, `IdParejaUCS`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of trigger `AltaSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaSector` AFTER INSERT ON `sectores` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of trigger `ActualizaSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `ActualizaSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `ActualizaSector` AFTER UPDATE ON `sectores` FOR EACH ROW BEGIN
    UPDATE Destinos 
			SET IdDestino=new.IdSector
			WHERE IdSistema=new.IdSistema AND
													IdDestino=old.IdSector AND
													TipoDestino=2 ;
END $$

DELIMITER ;

--
-- Definition of trigger `BajaSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `BajaSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `BajaSector` BEFORE DELETE ON `sectores` FOR EACH ROW BEGIN
    DELETE FROM Destinos WHERE IdSistema=old.IdSistema AND
																IdDestino=old.IdSector AND
																TipoDestino=2 ;
END $$

DELIMITER ;

--
-- Definition of table `sectoresagrupacion`
--

DROP TABLE IF EXISTS `sectoresagrupacion`;
CREATE TABLE `sectoresagrupacion` (
  `IdSistema` varchar(32) NOT NULL,
  `IdAgrupacion` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdAgrupacion`,`IdSector`,`IdNucleo`),
  KEY `Sectores_has_Agrupaciones_FKIndex1` (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `Sectores_has_Agrupaciones_FKIndex2` (`IdSistema`,`IdAgrupacion`),
  CONSTRAINT `sectoresagrupacion_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `sectoresagrupacion_ibfk_2` FOREIGN KEY (`IdSistema`, `IdAgrupacion`) REFERENCES `agrupaciones` (`IdSistema`, `IdAgrupacion`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `sectoressector`
--

DROP TABLE IF EXISTS `sectoressector`;
CREATE TABLE `sectoressector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdSectorOriginal` varchar(32) NOT NULL,
  `EsDominante` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`,`IdSectorOriginal`),
  KEY `SectoresSector_FKIndex1` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `sectoressector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `sectoressectorizacion`
--

DROP TABLE IF EXISTS `sectoressectorizacion`;
CREATE TABLE `sectoressectorizacion` (
  `IdSistema` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdTOP` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`IdTOP`),
  KEY `SectoresSectorizacion_FKIndex1` (`IdSistema`,`IdSectorizacion`),
  KEY `SectoresSectorizacion_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `SectoresSectorizacion_FKIndex3` (`IdSistema`,`IdTOP`),
  CONSTRAINT `sectoressectorizacion_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`) REFERENCES `sectorizaciones` (`IdSistema`, `IdSectorizacion`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `sectoressectorizacion_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `sectoressectorizacion_ibfk_3` FOREIGN KEY (`IdSistema`, `IdTOP`) REFERENCES `top` (`IdSistema`, `IdTOP`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `sectorizaciones`
--

DROP TABLE IF EXISTS `sectorizaciones`;
CREATE TABLE `sectorizaciones` (
  `IdSistema` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `Activa` tinyint(1) DEFAULT NULL,
  `FechaActivacion` datetime DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdSectorizacion`),
  KEY `Sectorizaciones_FKIndex1` (`IdSistema`),
  CONSTRAINT `sectorizaciones_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `sistema`
--

DROP TABLE IF EXISTS `sistema`;
CREATE TABLE `sistema` (
  `IdSistema` varchar(32) NOT NULL,
  `TiempoPTT` int(10) unsigned DEFAULT NULL,
  `TiempoSinJack1` int(10) unsigned DEFAULT NULL,
  `TiempoSinJack2` int(10) unsigned DEFAULT NULL,
  `TamLiteralEnlExt` int(10) unsigned DEFAULT NULL,
  `TamLiteralDA` int(10) unsigned DEFAULT NULL,
  `TamLiteralIA` int(10) unsigned DEFAULT NULL,
  `TamLiteralAG` int(10) unsigned DEFAULT NULL,
  `TamLiteralEmplazamiento` int(10) unsigned DEFAULT NULL,
  `VersionIP` int(1) unsigned DEFAULT NULL,
  `GrupoMulticastConfiguracion` varchar(15) DEFAULT NULL,
  `PuertoMulticastConfiguracion` int(5) unsigned DEFAULT '1000',
  `EstadoScvA` int(1) unsigned DEFAULT '0',
  `EstadoScvB` int(1) unsigned DEFAULT '0',
  PRIMARY KEY (`IdSistema`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `AltaSistema`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaSistema`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaSistema` AFTER INSERT ON `sistema` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of table `tablasmodificadas`
--

DROP TABLE IF EXISTS `tablasmodificadas`;
CREATE TABLE `tablasmodificadas` (
  `IdTabla` varchar(32) NOT NULL,
  PRIMARY KEY (`IdTabla`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `tareas`
--

DROP TABLE IF EXISTS `tareas`;
CREATE TABLE `tareas` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTarea` varchar(32) NOT NULL,
  `Programa` varchar(255) NOT NULL,
  `Argumentos` varchar(255) DEFAULT NULL,
  `Hora` time NOT NULL,
  `Periodicidad` char(1) NOT NULL,
  `Intervalo` int(3) DEFAULT '1',
  `Comentario` varchar(125) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdTarea`),
  CONSTRAINT `Sistema` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `teclassector`
--

DROP TABLE IF EXISTS `teclassector`;
CREATE TABLE `teclassector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `TransConConsultaPrev` tinyint(1) DEFAULT '1',
  `TransDirecta` tinyint(1) DEFAULT '0',
  `Conferencia` tinyint(1) DEFAULT '0',
  `Escucha` tinyint(1) DEFAULT '0',
  `Retener` tinyint(1) DEFAULT '1',
  `Captura` tinyint(1) DEFAULT '0',
  `Redireccion` tinyint(1) DEFAULT '0',
  `RepeticionUltLlamada` tinyint(1) DEFAULT '1',
  `RellamadaAut` tinyint(1) DEFAULT '0',
  `TeclaPrioridad` tinyint(1) DEFAULT '1',
  `Tecla55mas1` tinyint(1) DEFAULT '0',
  `Monitoring` tinyint(1) DEFAULT '1',
  `CoordinadorTF` tinyint(1) DEFAULT '0',
  `CoordinadorRD` tinyint(1) DEFAULT '0',
  `IntegracionRDTF` tinyint(1) DEFAULT '0',
  `LlamadaSelectiva` tinyint(1) DEFAULT '0',
  `GrupoBSS` tinyint(1) DEFAULT '1',
  `LTT` tinyint(1) DEFAULT '1',
  `SayAgain` tinyint(1) DEFAULT '1',
  `InhabilitacionRedirec` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `TeclasSector_FKIndex1` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `teclassector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `tifx`
--

DROP TABLE IF EXISTS `tifx`;
CREATE TABLE `tifx` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTIFX` varchar(32) NOT NULL,
  `ModoArranque` varchar(1) DEFAULT NULL,
  `ModoSincronizacion` int(1) unsigned DEFAULT NULL,
  `Master` varchar(32) DEFAULT '0.0.0.0:0',
  `SNMPPortLocal` int(10) unsigned DEFAULT '161',
  `SNMPPortRemoto` int(10) unsigned DEFAULT '161',
  `SNMPTraps` int(10) unsigned DEFAULT '162',
  `SIPPortLocal` int(10) unsigned DEFAULT '5060',
  `TimeSupervision` int(10) unsigned DEFAULT '0',
  `IpRed1` varchar(60) DEFAULT NULL,
  `IpRed2` varchar(60) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdTIFX`),
  KEY `TIFX_FKIndex1` (`IdSistema`),
  CONSTRAINT `tifx_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `TIFXDELETE`
--

DROP TRIGGER /*!50030 IF EXISTS */ `TIFXDELETE`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `TIFXDELETE` BEFORE DELETE ON `tifx` FOR EACH ROW UPDATE RECURSOS SET IdTIFX=NULL 
		WHERE IdSistema = old.IdSistema AND IdTIFX= old.IdTIFX $$

DELIMITER ;

--
-- Definition of table `top`
--

DROP TABLE IF EXISTS `top`;
CREATE TABLE `top` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTOP` varchar(32) NOT NULL,
  `PosicionSacta` int(10) unsigned DEFAULT NULL,
  `ModoArranque` varchar(1) DEFAULT NULL,
  `IpRed1` varchar(60) DEFAULT NULL,
  `IpRed2` varchar(60) DEFAULT NULL,
  `ConexionJacksEjecutivo` tinyint(1) DEFAULT '1',
  `ConexionJacksAyudante` tinyint(1) DEFAULT '1',
  `NumAltavoces` smallint(1) DEFAULT '8',
  PRIMARY KEY (`IdSistema`,`IdTOP`),
  KEY `TOP_FKIndex1` (`IdSistema`),
  CONSTRAINT `top_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `troncales`
--

DROP TABLE IF EXISTS `troncales`;
CREATE TABLE `troncales` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTroncal` varchar(32) NOT NULL,
  `NumTest` varchar(16) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdTroncal`),
  KEY `Troncales_FKIndex1` (`IdSistema`),
  CONSTRAINT `troncales_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `TRONCALDELETE`
--

DROP TRIGGER /*!50030 IF EXISTS */ `TRONCALDELETE`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `TRONCALDELETE` BEFORE DELETE ON `troncales` FOR EACH ROW UPDATE RECURSOSTF SET IdTroncal=NULL 
		WHERE IdTroncal=old.IdTroncal AND IdSistema=old.IdSistema $$

DELIMITER ;

--
-- Definition of table `troncalesruta`
--

DROP TABLE IF EXISTS `troncalesruta`;
CREATE TABLE `troncalesruta` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTroncal` varchar(32) NOT NULL,
  `Central` varchar(32) NOT NULL,
  `IdRuta` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdTroncal`,`Central`,`IdRuta`),
  KEY `TroncalesRuta_FKIndex1` (`IdSistema`,`IdTroncal`),
  KEY `TroncalesRuta_FKIndex2` (`IdSistema`,`IdRuta`,`Central`),
  CONSTRAINT `troncalesruta_ibfk_1` FOREIGN KEY (`IdSistema`, `IdTroncal`) REFERENCES `troncales` (`IdSistema`, `IdTroncal`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `troncalesruta_ibfk_2` FOREIGN KEY (`IdSistema`, `IdRuta`, `Central`) REFERENCES `rutas` (`IdSistema`, `IdRuta`, `Central`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `usuariosabonados`
--

DROP TABLE IF EXISTS `usuariosabonados`;
CREATE TABLE `usuariosabonados` (
  `IdSistema` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdAbonado` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdPrefijo`,`IdAbonado`,`IdNucleo`,`IdSector`),
  KEY `UsuariosAbonados_FKIndex2` (`IdSistema`,`IdPrefijo`),
  KEY `UsuariosAbonados_FKIndex3` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `usuariosabonados_ibfk_2` FOREIGN KEY (`IdSistema`, `IdPrefijo`) REFERENCES `prefijos` (`IdSistema`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `usuariosabonados_ibfk_3` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Create schema cd40_trans
--

CREATE DATABASE IF NOT EXISTS cd40_trans;
USE cd40_trans;

--
-- Definition of table `abonados`
--

DROP TABLE IF EXISTS `abonados`;
CREATE TABLE `abonados` (
  `IdSistema` varchar(32) NOT NULL,
  `IdAbonado` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdAbonado`),
  KEY `ABONADOS_FKIndex1` (`IdSistema`),
  CONSTRAINT `abonados_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `agenda`
--

DROP TABLE IF EXISTS `agenda`;
CREATE TABLE `agenda` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `Prefijo` int(2) unsigned NOT NULL DEFAULT '0',
  `Numero` varchar(20) NOT NULL DEFAULT '',
  `Nombre` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`,`Prefijo`,`Numero`),
  CONSTRAINT `FK_SECTORES` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `agrupaciones`
--

DROP TABLE IF EXISTS `agrupaciones`;
CREATE TABLE `agrupaciones` (
  `IdSistema` varchar(32) NOT NULL,
  `IdAgrupacion` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdAgrupacion`),
  KEY `Agrupaciones_FKIndex1` (`IdSistema`),
  CONSTRAINT `agrupaciones_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `alarmas`
--

DROP TABLE IF EXISTS `alarmas`;
CREATE TABLE `alarmas` (
  `IdSistema` varchar(32) NOT NULL,
  `IdIncidencia` int(10) unsigned NOT NULL,
  `Alarma` tinyint(1) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdIncidencia`),
  KEY `Alarmas_FKIndex1` (`IdSistema`),
  KEY `Alarmas_FKIndex2` (`IdIncidencia`),
  CONSTRAINT `alarmas_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `alarmas_ibfk_2` FOREIGN KEY (`IdIncidencia`) REFERENCES `incidencias` (`IdIncidencia`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `altavoces`
--

DROP TABLE IF EXISTS `altavoces`;
CREATE TABLE `altavoces` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `NumAltavoz` int(10) unsigned NOT NULL,
  `Estado` varchar(1) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdNucleo`,`IdSector`,`PosHMI`,`NumAltavoz`),
  KEY `Altavoces_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdNucleo`,`IdSector`,`PosHMI`),
  CONSTRAINT `altavoces_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdNucleo`, `IdSector`, `PosHMI`) REFERENCES `destinosradiosector` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdNucleo`, `IdSector`, `PosHMI`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `controlbackup`
--

DROP TABLE IF EXISTS `controlbackup`;
CREATE TABLE `controlbackup` (
  `IdSistema` varchar(32) NOT NULL,
  `TipoBackup` int(11) NOT NULL,
  `Profundidad` int(11) unsigned NOT NULL,
  `Recurso` varchar(128) NOT NULL,
  PRIMARY KEY (`IdSistema`,`TipoBackup`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `destinos`
--

DROP TABLE IF EXISTS `destinos`;
CREATE TABLE `destinos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`),
  KEY `Destinos_FKIndex1` (`IdSistema`),
  CONSTRAINT `destinos_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `InsertDestino`
--

DROP TRIGGER /*!50030 IF EXISTS */ `InsertDestino`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `InsertDestino` AFTER INSERT ON `destinos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('Destinos');
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestino`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestino`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestino` AFTER UPDATE ON `destinos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('Destinos');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestino`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestino`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestino` BEFORE DELETE ON `destinos` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of table `destinosexternos`
--

DROP TABLE IF EXISTS `destinosexternos`;
CREATE TABLE `destinosexternos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `IdAbonado` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_44_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  CONSTRAINT `Table_44_FKIndex1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinostelefonia` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of trigger `AltaDestinosExternos`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinosExternos`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinosExternos` BEFORE INSERT ON `destinosexternos` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinosExternos`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinosExternos`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinosExternos` AFTER UPDATE ON `destinosexternos` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinosExternos`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinosExternos`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinosExternos` BEFORE DELETE ON `destinosexternos` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of table `destinosexternossector`
--

DROP TABLE IF EXISTS `destinosexternossector`;
CREATE TABLE `destinosexternossector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `PosHMI` int(10) unsigned DEFAULT NULL,
  `Prioridad` int(10) unsigned DEFAULT NULL,
  `OrigenR2` varchar(32) DEFAULT NULL,
  `PrioridadSIP` int(10) unsigned DEFAULT NULL,
  `TipoAcceso` varchar(2) NOT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_45_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_45_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `destinosexternossector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinosexternos` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `destinosexternossector_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `AltaDestinoExternoSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinoExternoSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinoExternoSector` AFTER INSERT ON `destinosexternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector');
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoExternoSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoExternoSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoExternoSector` AFTER UPDATE ON `destinosexternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinoExternoSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinoExternoSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinoExternoSector` BEFORE DELETE ON `destinosexternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector');
END $$

DELIMITER ;

--
-- Definition of table `destinosinternos`
--

DROP TABLE IF EXISTS `destinosinternos`;
CREATE TABLE `destinosinternos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_42_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_42_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `destinosinternos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinostelefonia` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `destinosinternos_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of trigger `AltaDestinoInterno`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinoInterno`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinoInterno` BEFORE INSERT ON `destinosinternos` FOR EACH ROW BEGIN
    INSERT INTO DestinosTelefonia SET IdSistema=new.IdSistema,
														 					IdDestino=new.IdDestino,
														 					TipoDestino=new.TipoDestino,
																			IdPrefijo=new.IdPrefijo;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos');
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoInterno`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoInterno`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoInterno` AFTER UPDATE ON `destinosinternos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinoInterno`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinoInterno`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinoInterno` BEFORE DELETE ON `destinosinternos` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos');
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END $$

DELIMITER ;

--
-- Definition of table `destinosinternossector`
--

DROP TABLE IF EXISTS `destinosinternossector`;
CREATE TABLE `destinosinternossector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `Prioridad` int(1) unsigned DEFAULT NULL,
  `OrigenR2` varchar(32) DEFAULT NULL,
  `PrioridadSIP` int(1) unsigned DEFAULT NULL,
  `TipoAcceso` varchar(2) NOT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdNucleo`,`IdSector`,`IdPrefijo`),
  KEY `Table_43_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `Table_43_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `destinosinternossector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinosinternos` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `destinosinternossector_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of trigger `AltaDestinoInternoSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinoInternoSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinoInternoSector` AFTER INSERT ON `destinosinternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoInternoSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoInternoSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoInternoSector` AFTER UPDATE ON `destinosinternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinosInternosSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinosInternosSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinosInternosSector` BEFORE DELETE ON `destinosinternossector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector');
END $$

DELIMITER ;

--
-- Definition of table `destinosradio`
--

DROP TABLE IF EXISTS `destinosradio`;
CREATE TABLE `destinosradio` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `TipoFrec` int(1) unsigned DEFAULT NULL,
  `ExclusividadTXRX` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`),
  KEY `DestinosRadio_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`),
  CONSTRAINT `destinosradio_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`) REFERENCES `destinos` (`IdSistema`, `IdDestino`, `TipoDestino`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of trigger `AltaDestinoRadio`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinoRadio`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinoRadio` BEFORE INSERT ON `destinosradio` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoRadio`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoRadio`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoRadio` AFTER UPDATE ON `destinosradio` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadio');
END $$

DELIMITER ;

--
-- Definition of trigger `DESTRADIODELETE`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DESTRADIODELETE`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DESTRADIODELETE` BEFORE DELETE ON `destinosradio` FOR EACH ROW BEGIN
IF old.TipoDestino=0 THEN
	UPDATE RECURSOSRADIO SET IdDestino=NULL,TipoDestino=NULL 
  				WHERE IdSistema = old.IdSistema AND IdDestino=old.IdDestino AND TipoDestino=old.TipoDestino;
END IF ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadio');
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector') ;
END $$

DELIMITER ;

--
-- Definition of table `destinosradiosector`
--

DROP TABLE IF EXISTS `destinosradiosector`;
CREATE TABLE `destinosradiosector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `Prioridad` int(1) unsigned DEFAULT NULL,
  `PrioridadSIP` int(1) unsigned DEFAULT NULL,
  `ModoOperacion` varchar(1) DEFAULT NULL,
  `Cascos` varchar(1) DEFAULT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  `SupervisionPortadora` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdNucleo`,`IdSector`,`PosHMI`),
  KEY `DestinosRadioSector_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`),
  KEY `DestinosRadioSector_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `destinosradiosector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`) REFERENCES `destinosradio` (`IdSistema`, `IdDestino`, `TipoDestino`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `destinosradiosector_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `AltaDestinoRadioSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinoRadioSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinoRadioSector` AFTER INSERT ON `destinosradiosector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector');
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoRadioSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoRadioSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoRadioSector` AFTER UPDATE ON `destinosradiosector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinoRadioSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinoRadioSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinoRadioSector` BEFORE DELETE ON `destinosradiosector` FOR EACH ROW BEGIN
  REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosRadioSector');
END $$

DELIMITER ;

--
-- Definition of table `destinostelefonia`
--

DROP TABLE IF EXISTS `destinostelefonia`;
CREATE TABLE `destinostelefonia` (
  `IdSistema` varchar(32) NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `IdGrupo` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  KEY `DestinosTelefonia_FKIndex1` (`IdSistema`,`IdDestino`,`TipoDestino`),
  KEY `DestinosTelefonia_FKIndex2` (`IdSistema`,`IdGrupo`),
  KEY `DestinosTelefonia_FKIndex3` (`IdSistema`,`IdPrefijo`),
  CONSTRAINT `destinostelefonia_ibfk_1` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`) REFERENCES `destinos` (`IdSistema`, `IdDestino`, `TipoDestino`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `destinostelefonia_ibfk_2` FOREIGN KEY (`IdSistema`, `IdGrupo`) REFERENCES `grupostelefonia` (`IdSistema`, `IdGrupo`) ON DELETE NO ACTION ON UPDATE CASCADE,
  CONSTRAINT `destinostelefonia_ibfk_3` FOREIGN KEY (`IdSistema`, `IdPrefijo`) REFERENCES `prefijos` (`IdSistema`, `IdPrefijo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of trigger `AltaDestinosTelefonia`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaDestinosTelefonia`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaDestinosTelefonia` BEFORE INSERT ON `destinostelefonia` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of trigger `UpdateDestinoTelefonia`
--

DROP TRIGGER /*!50030 IF EXISTS */ `UpdateDestinoTelefonia`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `UpdateDestinoTelefonia` AFTER UPDATE ON `destinostelefonia` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosTelefonia');
END $$

DELIMITER ;

--
-- Definition of trigger `DeleteDestinoTelefonia`
--

DROP TRIGGER /*!50030 IF EXISTS */ `DeleteDestinoTelefonia`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `DeleteDestinoTelefonia` BEFORE DELETE ON `destinostelefonia` FOR EACH ROW BEGIN
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosTelefonia') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternos') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternos') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosInternosSector') ;
    REPLACE INTO TablasModificadas (IdTabla) VALUES ('DestinosExternosSector') ;
END $$

DELIMITER ;

--
-- Definition of table `emplazamientos`
--

DROP TABLE IF EXISTS `emplazamientos`;
CREATE TABLE `emplazamientos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdEmplazamiento` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdEmplazamiento`),
  KEY `Emplazamientos_FKIndex1` (`IdSistema`),
  CONSTRAINT `emplazamientos_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `PRDELETE`
--

DROP TRIGGER /*!50030 IF EXISTS */ `PRDELETE`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `PRDELETE` BEFORE DELETE ON `emplazamientos` FOR EACH ROW UPDATE RECURSOSRADIO SET IdEmplazamiento=NULL
			WHERE IdSistema = old.IdSistema AND IdEmplazamiento=old.IdEmplazamiento $$

DELIMITER ;

--
-- Definition of table `encaminamientos`
--

DROP TABLE IF EXISTS `encaminamientos`;
CREATE TABLE `encaminamientos` (
  `IdSistema` varchar(32) NOT NULL,
  `Central` varchar(32) NOT NULL,
  `CentralPropia` tinyint(1) DEFAULT '0',
  `Throwswitching` tinyint(1) DEFAULT '0',
  `NumTest` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`Central`),
  KEY `Encaminamientos_FKIndex1` (`IdSistema`),
  CONSTRAINT `encaminamientos_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `equiposeu`
--

DROP TABLE IF EXISTS `equiposeu`;
CREATE TABLE `equiposeu` (
  `IdSistema` varchar(32) NOT NULL,
  `idEquipos` varchar(32) NOT NULL,
  `IpRed1` varchar(60) DEFAULT NULL,
  `IpRed2` varchar(60) DEFAULT NULL,
  `TipoEquipo` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`idEquipos`),
  KEY `EquiposEU_FKIndex1` (`IdSistema`),
  CONSTRAINT `equiposeu_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `estadoaltavoces`
--

DROP TABLE IF EXISTS `estadoaltavoces`;
CREATE TABLE `estadoaltavoces` (
  `IdDestino` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `IdSistema` varchar(32) NOT NULL,
  `NumAltavoz` int(10) unsigned NOT NULL,
  `Estado` varchar(1) DEFAULT NULL,
  PRIMARY KEY (`IdDestino`,`PosHMI`,`IdSector`,`IdNucleo`,`IdSectorizacion`,`IdSistema`,`NumAltavoz`),
  KEY `Altavoz_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`PosHMI`,`IdDestino`),
  CONSTRAINT `estadoaltavoces_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`, `PosHMI`, `IdDestino`) REFERENCES `radio` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`, `PosHMI`, `IdDestino`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `estadorecursos`
--

DROP TABLE IF EXISTS `estadorecursos`;
CREATE TABLE `estadorecursos` (
  `IdDestino` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `Estado` varchar(1) DEFAULT NULL,
  PRIMARY KEY (`IdDestino`,`PosHMI`,`IdSector`,`IdNucleo`,`IdSectorizacion`,`IdSistema`,`IdRecurso`),
  KEY `Recurso_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`PosHMI`,`IdDestino`),
  CONSTRAINT `estadorecursos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`, `PosHMI`, `IdDestino`) REFERENCES `radio` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`, `PosHMI`, `IdDestino`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `estadosrecursos`
--

DROP TABLE IF EXISTS `estadosrecursos`;
CREATE TABLE `estadosrecursos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `TipoDestino` int(10) unsigned NOT NULL,
  `Estado` varchar(1) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdSector`,`IdNucleo`,`PosHMI`,`IdRecurso`,`TipoRecurso`,`IdDestino`,`TipoDestino`),
  KEY `Table_44_FKIndex1` (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `Table_44_FKIndex2` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdNucleo`,`IdSector`,`PosHMI`),
  CONSTRAINT `estadosrecursos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRecurso`, `TipoRecurso`) REFERENCES `recursosradio` (`IdSistema`, `IdRecurso`, `TipoRecurso`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `estadosrecursos_ibfk_2` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdNucleo`, `IdSector`, `PosHMI`) REFERENCES `destinosradiosector` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdNucleo`, `IdSector`, `PosHMI`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `eventosradio`
--

DROP TABLE IF EXISTS `eventosradio`;
CREATE TABLE `eventosradio` (
  `IdSistema` varchar(32) NOT NULL,
  `Scv` int(1) DEFAULT '0',
  `TipoHw` int(1) NOT NULL,
  `IdHw` varchar(32) NOT NULL,
  `Usuario` varchar(32) NOT NULL,
  `UsuarioLogico` varchar(32) DEFAULT NULL COMMENT 'Si el TipoHw es una TOP, UsuarioLogico será null.',
  `IdIncidencia` int(10) unsigned DEFAULT NULL,
  `Evento` varchar(255) NOT NULL,
  `FechaHora` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `Duracion` time NOT NULL,
  PRIMARY KEY (`IdSistema`,`TipoHw`,`IdHw`,`FechaHora`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `funciones`
--

DROP TABLE IF EXISTS `funciones`;
CREATE TABLE `funciones` (
  `IdSistema` varchar(32) NOT NULL,
  `Clave` int(11) NOT NULL,
  `Tipo` int(11) NOT NULL COMMENT '0: Radio; 1: Telefonía',
  `Funcion` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`Clave`),
  CONSTRAINT `FK_Sistema_Funciones` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `grupostelefonia`
--

DROP TABLE IF EXISTS `grupostelefonia`;
CREATE TABLE `grupostelefonia` (
  `IdSistema` varchar(32) NOT NULL,
  `IdGrupo` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdGrupo`),
  KEY `GruposTelefonia_FKIndex1` (`IdSistema`),
  CONSTRAINT `grupostelefonia_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `historicoincidencias`
--

DROP TABLE IF EXISTS `historicoincidencias`;
CREATE TABLE `historicoincidencias` (
  `IdSistema` varchar(32) NOT NULL,
  `Scv` int(1) NOT NULL DEFAULT '0',
  `IdHw` varchar(32) NOT NULL,
  `TipoHw` int(1) unsigned NOT NULL,
  `IdIncidencia` int(10) unsigned NOT NULL,
  `FechaHora` datetime NOT NULL,
  `Reconocida` datetime DEFAULT NULL,
  `Descripcion` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdHw`,`TipoHw`,`IdIncidencia`,`FechaHora`),
  KEY `HistoricoIncidencias_FKIndex1` (`IdIncidencia`),
  KEY `HistoricoIncidencias_FKIndex2` (`IdSistema`),
  CONSTRAINT `historicoincidencias_ibfk_1` FOREIGN KEY (`IdIncidencia`) REFERENCES `incidencias` (`IdIncidencia`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `historicoincidencias_ibfk_2` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `incidencias`
--

DROP TABLE IF EXISTS `incidencias`;
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

--
-- Dumping data for table `incidencias`
--

/*!40000 ALTER TABLE `incidencias` DISABLE KEYS */;
INSERT INTO `incidencias` (`IdIncidencia`,`IdIncidenciaCorrectora`,`Incidencia`,`Descripcion`,`GeneraError`,`OID`) VALUES 
 (96,NULL,'Cambio de día.','Cambio de día',NULL,NULL),
 (101,NULL,'Selección SCV','SCV {0} seleccionado.',NULL,NULL),
 (105,NULL,'Carga de sectorización.','Carga de sectorización {0}',NULL,NULL),
 (106,NULL,'Error carga sectorización.','Error carga sectorización {0}',0,NULL),
 (108,NULL,'Rechazo sectorización. No están todos los sectores reales.','Rechazo sectorización {0}. No están todos los sectores reales',0,NULL),
 (109,NULL,'Sectorización automática implantada.','Sectorización automática implantada',NULL,NULL),
 (110,NULL,'Sectorización automática rechazada.','Sectorización automática rechazada',0,NULL),
 (111,NULL,'Sector asignado a posición.','Sector {0} asignado a posición {1}',NULL,NULL),
 (112,NULL,'Sector desasignado de la posición.','Sector {0} desasignado de la posición {1}',NULL,NULL),
 (201,NULL,'Servidor 1 activo.','Servidor 1 activo',NULL,NULL),
 (202,201,'Servidor 1 caído.','Servidor 1 caído',0,NULL),
 (203,NULL,'Servidor 2 activo.','Servidor 2 activo',NULL,NULL),
 (204,203,'Servidor 2 caído.','Servidor 2 caído',0,NULL),
 (1001,NULL,'Entrada TOP.','Entrada TOP',1,'1.1.1000.0'),
 (1002,1001,'Caída TOP.','Caída TOP',1,'1.1.1000.0'),
 (1003,NULL,'Conexión jacks ejecutivo.','Conexión jacks ejecutivo. Posición: {0}',0,'1.1.1000.1.3.0'),
 (1004,1003,'Desconexión jacks ejecutivo.','Desconexión jacks ejecutivo. Posición: {0}',0,'1.1.1000.1.3.0'),
 (1005,NULL,'Conexión jacks ayudante.','Conexión jacks ayudante. Posición: {0}',0,'1.1.1000.1.3.1'),
 (1006,1005,'Desconexión jacks ayudante.','Desconexión jacks ayudante. Posición: {0} ',0,'1.1.1000.1.3.1'),
 (1007,NULL,'Conexión altavoz.','Conexión altavoz. Posición: {0} ',1,'1.1.1000.1.2'),
 (1008,1007,'Desconexión altavoz.','Desconexión altavoz. Posición: {0} ',1,'1.1.1000.1.2'),
 (1009,NULL,'Panel pasa a operación.','Panel pasa a operación. Posición: {0} ',1,'1.1.6.0'),
 (1010,1009,'Panel pasa a standby.','Panel pasa a standby. Posición: {0} ',1,'1.1.6.0'),
 (1011,NULL,'Página de frecuencias seleccionada.','Página de frecuencias seleccionada: {0}. Posición: {1}. Sector: {2}',NULL,NULL),
 (1012,NULL,'Selección recurso radio.','Selección recurso radio: {0}. Frecuencia: {1}. Posicion: {2}. Sector: {3}',NULL,NULL),
 (1013,NULL,'Estado selección.','Estado selección: {0}. Frecuencia: {1}. Posicion {2}. Sector: {3}',NULL,NULL),
 (1014,NULL,'Estado PTT.','Estado PTT: {0}. Posicion:{1}. Sector{2}',NULL,'1.1.1000.2'),
 (1015,NULL,'Facilidad seleccionada.','Facilidad seleccionada: {0}. Posición: {1}. Sector: {2}',NULL,NULL),
 (1016,NULL,'Llamada entrante Posicion.','Llamada entrante Posicion: {0}. Acceso: {1}. Destino: {2}. Prioridad: {3}',NULL,NULL),
 (1017,NULL,'Llamada saliente Posicion.','Llamada saliente Posicion: {0}. Acceso: {1}. Destino: {2}. Prioridad: {3}',NULL,NULL),
 (1018,NULL,'Estado Recepción.','Estado Recepción: {0}. Posicion:{1}. Sector{2}',0,NULL),
 (1019,NULL,'Fin llamada saliente Posicion.','Fin llamada saliente Posicion: {0}. Acceso: {1}. Destino: {2}. Prioridad: {3}',NULL,NULL),
 (2001,NULL,'Entrada GW.','Entrada GW',1,'1.1.100.2.0'),
 (2002,2001,'Caída GW.','Caída GW.',1,'1.1.100.2.0'),
 (2003,NULL,'Conexión Recurso Radio.','Conexión Recurso Radio. Pasarela: {0}. Recurso: {1}',NULL,'1.1.200.3.1.17'),
 (2004,2003,'Desconexión Recurso Radio.','Desconexión Recurso Radio.  Pasarela: {0}. Recurso: {1}',0,'1.1.200.3.1.17'),
 (2005,NULL,'Conexión Recurso Telefonía.','Conexión Recurso Telefonía.  Pasarela: {0}. Recurso: {1}',NULL,'1.1.400.3.1.14'),
 (2006,2005,'Desconexión Recurso Telefonía.','Desconexión Recurso Telefonía. Pasarela: {0}. Recurso: {1}',0,'1.1.400.3.1.14'),
 (2007,NULL,'Conexión tarjeta de interfaz','Conexión tarjeta de interfaz: Slot {0}',1,'1.1.100.31.1.2'),
 (2008,2007,'Desconexión tarjeta de interfaz','Desconexión tarjeta de interfaz: Slot {0}',1,'1.1.100.31.1.2'),
 (2009,NULL,'Conexión Recurso R2.','Conexión Recurso R2.  Pasarela: {0}. Recurso: {1}',NULL,'1.1.500.3.1.17'),
 (2010,NULL,'Desconexión Recurso R2.','Desconexión Recurso R2.  Pasarela: {0}. Recurso: {1}',NULL,'1.1.500.3.1.17'),
 (2012,NULL,'Error protocolo LCN.','Error protocolo LCN.  Pasarela: {0}. Recurso: {1}',NULL,'1.1.300.3.1.17'),
 (2013,NULL,'Conexión Recurso LCN.','Conexión Recurso LCN.  Pasarela: {0}. Recurso: {1}',NULL,'1.1.300.3.1.17'),
 (2014,2013,'Desconexión Recurso LCN.','Desconexión Recurso LCN.  Pasarela: {0}. Recurso: {1}',0,'1.1.300.3.1.17'),
 (2020,NULL,'Llamada entrante R2.','Llamada entrante R2. Recurso {0}. Troncal {1}. Origen {2}. Prioridad {3}. Destino {4}',NULL,'1.1.500.3.1.17'),
 (2021,NULL,'Fin llamada entrante R2.','Fin llamada entrante R2. Recurso {0}. Troncal {1}.',NULL,'1.1.500.3.1.17'),
 (2022,NULL,'Llamada saliente R2.','Llamada saliente R2. Recurso {0}. Troncal {1}. Origen {2}. Prioridad {3}. Destino {4}',NULL,'1.1.500.3.1.17'),
 (2023,NULL,'Fin llamada saliente R2.','Fin llamada saliente R2. Recurso {0}. Troncal {1}.',NULL,'1.1.500.3.1.17'),
 (2024,NULL,'Llamada prueba R2.','Llamada prueba R2. Pasarela: {0}. Recurso: {1}',NULL,'1.1.500.3.1.17'),
 (2025,NULL,'Error protocolo R2.','Error protocolo R2. Pasarela: {0}. Recurso: {1}',0,'1.1.500.3.1.17'),
 (2030,NULL,'Llamada entrante LCN.','Llamada entrante LCN: Recurso: {0}.',NULL,'1.1.300.3.1.17'),
 (2031,NULL,'Fin llamada entrante LCN.','Fin llamada entrante LCN: Recurso: {0}.',NULL,'1.1.300.3.1.17'),
 (2032,NULL,'Llamada saliente LCN.','Llamada saliente LCN: Recurso: {0}.',NULL,'1.1.300.3.1.17'),
 (2033,NULL,'Fin llamada saliente LCN.','Fin llamada saliente LCN: Recurso: {0}.',NULL,'1.1.300.3.1.17'),
 (2040,NULL,'Llamada entrante telefonía.','Llamada entrante telefonía: Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2041,NULL,'Fin llamada entrante telefonía.','Fin llamada entrante telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2042,NULL,'Llamada saliente telefonía.','Llamada saliente telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2043,NULL,'Fin llamada saliente telefonía.','Fin llamada saliente telefonía:  Recurso {0}. Línea {1}.  Red {2}. Tipo interfaz {3}. Acceso {4}.',NULL,'1.1.400.3.1.14'),
 (2050,NULL,'PTT On.','PTT On:  Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (2051,NULL,'PTT Off.','PTT Off:   Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (2052,NULL,'SQ On.','SQ On:   Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (2053,NULL,'SQ Off.','SQ Off:   Pasarela: {0}. Recurso {1}. Frecuencia: {2}',NULL,'1.1.200.3.1.17'),
 (3001,NULL,'Entrada Equipo Externo.','Entrada Equipo Externo',1,NULL),
 (3002,3001,'Caída Equipo Externo.','Caída Equipo Externo',1,NULL);
/*!40000 ALTER TABLE `incidencias` ENABLE KEYS */;

--
-- Definition of table `incidencias_ingles`
--

DROP TABLE IF EXISTS `incidencias_ingles`;
CREATE TABLE `incidencias_ingles` (
  `IdIncidencia` int(10) unsigned NOT NULL,
  `IdIncidenciaCorrectora` int(10) unsigned DEFAULT NULL,
  `Incidencia` varchar(180) DEFAULT NULL,
  `Descripcion` varchar(180) DEFAULT NULL,
  `GeneraError` tinyint(1) DEFAULT NULL,
  `OID` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdIncidencia`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `incidencias_ingles`
--

/*!40000 ALTER TABLE `incidencias_ingles` DISABLE KEYS */;
INSERT INTO `incidencias_ingles` (`IdIncidencia`,`IdIncidenciaCorrectora`,`Incidencia`,`Descripcion`,`GeneraError`,`OID`) VALUES 
 (96,NULL,'Change of day.','Change of day',NULL,NULL),
 (105,NULL,'Sectorization load','Sectorization load {0}',NULL,NULL),
 (106,NULL,'Error in sectorization load.','Error sectorization load {0}',1,NULL),
 (108,NULL,'Rejection in sectorización. All real sectors are not in sectorization.','Reject in sectorization {0}. All real sectors are not in sectorization',1,NULL),
 (109,NULL,'Loaded automatic sectorization.','Loaded automatic sectorization',NULL,NULL),
 (110,NULL,'Rejected automatic sectorization.','Rejected automatic sectorization',1,NULL),
 (111,NULL,'Position assigned to the sector.','Position {1} assigned to the sector {0}',NULL,NULL),
 (112,NULL,'Position unassigned to the sector.','Position {1} unassigned to the sector {0}',NULL,NULL),
 (201,NULL,'Server 1 active.','Server 1 active',NULL,NULL),
 (202,201,'Server 1 down.','Server 1 down.',1,NULL),
 (203,NULL,'Server 2 active','Server 2 active',NULL,NULL),
 (204,203,'Server 2 down','Server 2 down',1,NULL),
 (1001,NULL,'Entering TOP.','Entering TOP. Position: {0}',NULL,'1.1.1000.0'),
 (1002,1001,'TOP down.','TOP down. Position: {0}',1,'1.1.1000.0'),
 (1003,NULL,'Executive jacks on.','Executive jacks on. Position: {0}',NULL,'1.1.1000.1.3.0'),
 (1004,1003,'Executive jacks off.','Executive jacks off. Position: {0}',1,'1.1.1000.1.3.0'),
 (1005,NULL,'Assistant jacks on.','Assistant jacks on. Position: {0}',NULL,'1.1.1000.1.3.1'),
 (1006,1005,'Assistant jacks off.','Assistant jacks off. Position: {0}',1,'1.1.1000.1.3.1'),
 (1007,NULL,'Speaker on.','Speaker on. Position: {0}',-4,'1.1.1000.1.2'),
 (1008,1007,'Speaker off','Speaker off. Position: {0}',0,'1.1.1000.1.2'),
 (1009,NULL,'Panel to operation','Panel to operation. Position: {0}',-1,'1.1.1000.1.4'),
 (1010,1009,'Panel to standby.','Panel to standby. Position: {0}',1,'1.1.1000.1.4'),
 (1011,NULL,'Frequency page selected','Frequency page selected: {0}. Position: {1}. Sector: {2}',NULL,NULL),
 (1012,NULL,'Radio resource selection.','Radio resource selection: {0}. Frequency: {1}. Position: {2}. Sector: {3}',NULL,NULL),
 (1013,NULL,'Selection status','Selection status: {0}. Frequency: {1}. Position {2}. Sector: {3}',NULL,NULL),
 (1014,NULL,'PTT status.','PTT status: {0}. Position:{1}. PTT Type: {2}',NULL,'1.1.1000.2'),
 (1015,NULL,'Selected function','Selected function: {0}. Position: {1}. Sector: {2}',NULL,NULL),
 (1016,NULL,'Incomming call Position.','Incomming call Position: {0}. Access: {1}. Destination: {2}. Priority: {3}',NULL,NULL),
 (1017,NULL,'Outgoing call Position.','Outgoing call Position: {0}. Access: {1}. Destination: {2}. Priority: {3}',NULL,NULL),
 (1018,NULL,'Reception status.','Reception status: {0}. Position:{1}. Sector{2}',0,NULL),
 (1019,NULL,'End of outgoing call Position.','End of outgoing call Position: {0}. Access: {1}. Destination: {2}. Priority: {3}',NULL,NULL),
 (2001,NULL,'Entering GW.','Entering GW.',NULL,'1.1.100.2.0'),
 (2002,2001,'GW down.','GW down.',1,'1.1.100.2.0'),
 (2003,NULL,'Radio resource on.','Radio resource on.',NULL,'1.1.200.3.1.17'),
 (2004,2003,'Radio resource off.','Radio resource off.',1,'1.1.200.3.1.17'),
 (2005,NULL,'Telephone resource on.','Telephone resource on.',NULL,'1.1.400.3.1.14'),
 (2006,2005,'Telephone resource off.','Telephone resource off.',1,'1.1.400.3.1.14'),
 (2009,NULL,'R2 resource on.','R2 resource on.',NULL,'1.1.500.3.1.17'),
 (2010,NULL,'R2 resource off.','R2 resource off.',NULL,'1.1.500.3.1.17'),
 (2012,NULL,'Protocol LCN error.','Protocol LCN error',NULL,'1.1.300.3.1.17'),
 (2013,NULL,'LCN resource on.','LCN resource on.',NULL,'1.1.300.3.1.17'),
 (2014,2013,'LCN resource off.','LCN resource off.',1,'1.1.300.3.1.17'),
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
 (2050,NULL,'PTT On.','PTT On:  Gateway: {0}. Resource {1}. Frequency: {2}',NULL,'1.1.200.3.1.17'),
 (2051,NULL,'PTT Off.','PTT Off:  Gateway: {0}. Resource {1}. Frequency: {2}',NULL,'1.1.200.3.1.17'),
 (2052,NULL,'SQ On.','SQ On:  Gateway: {0}. Resource {1}. Frequency: {2}',NULL,'1.1.200.3.1.17'),
 (2053,NULL,'SQ Off.','SQ Off:  Gateway: {0}. Resource {1}. Frequency: {2}',NULL,'1.1.200.3.1.17'),
 (3001,NULL,'Entering external equipment','Entering external equipment',NULL,NULL),
 (3002,3001,'External equipment down','External equipment down',1,NULL);
/*!40000 ALTER TABLE `incidencias_ingles` ENABLE KEYS */;


--
-- Definition of table `indicadores`
--

DROP TABLE IF EXISTS `indicadores`;
CREATE TABLE `indicadores` (
  `IdSistema` varchar(32) NOT NULL,
  `FamiliaIndicador` int(11) NOT NULL,
  `Indicador` int(11) NOT NULL,
  `ElementoFisico` varchar(32) NOT NULL,
  `ElementoLogico` varchar(32) DEFAULT NULL,
  `Destino` varchar(32) DEFAULT NULL,
  `Red` varchar(32) DEFAULT NULL,
  `Troncal` varchar(32) DEFAULT NULL,
  `TipoInterfaz` varchar(5) DEFAULT NULL,
  `Prioridad` varchar(5) DEFAULT NULL,
  `Ruta` varchar(32) DEFAULT NULL,
  `Acceso` varchar(1) DEFAULT NULL,
  `FechaHora` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `Duracion` time NOT NULL,
  PRIMARY KEY (`IdSistema`,`FamiliaIndicador`,`Indicador`,`ElementoFisico`,`FechaHora`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

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
  `TipoAcceso` varchar(2) NOT NULL,
  `PosHMI` int(10) unsigned DEFAULT NULL,
  `Prioridad` int(10) unsigned DEFAULT NULL,
  `OrigenR2` varchar(32) DEFAULT NULL,
  `PrioridadSIP` int(10) unsigned DEFAULT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`IdDestino`,`IdPrefijo`,`TipoAcceso`),
  KEY `Table_40_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`),
  CONSTRAINT `internos_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) REFERENCES `sectoressectorizacion` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `niveles`
--

DROP TABLE IF EXISTS `niveles`;
CREATE TABLE `niveles` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `CICL` int(10) unsigned DEFAULT '0',
  `CIPL` int(10) unsigned DEFAULT '0',
  `CPICL` int(10) unsigned DEFAULT '0',
  `CPIPL` int(10) unsigned DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `Niveles_FKIndex1` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `niveles_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `nucleos`
--

DROP TABLE IF EXISTS `nucleos`;
CREATE TABLE `nucleos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdNucleo`),
  KEY `Nucleos_FKIndex1` (`IdSistema`),
  CONSTRAINT `nucleos_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `operadores`
--

DROP TABLE IF EXISTS `operadores`;
CREATE TABLE `operadores` (
  `IdOperador` varchar(32) NOT NULL,
  `IdSistema` varchar(32) NOT NULL,
  `Clave` varchar(10) DEFAULT NULL,
  `NivelAcceso` int(1) unsigned DEFAULT NULL,
  `Nombre` varchar(32) DEFAULT NULL,
  `Apellidos` varchar(32) DEFAULT NULL,
  `Telefono` varchar(32) DEFAULT NULL,
  `FechaUltAcceso` date DEFAULT NULL,
  `Comentarios` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`IdOperador`,`IdSistema`),
  KEY `Operadores_FKIndex1` (`IdSistema`),
  CONSTRAINT `operadores_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `parametrosrecurso`
--

DROP TABLE IF EXISTS `parametrosrecurso`;
CREATE TABLE `parametrosrecurso` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL,
  `GananciaAGCTX` int(1) unsigned DEFAULT '0',
  `GananciaAGCTXdBm` int(11) DEFAULT '0',
  `GananciaAGCRX` int(1) unsigned DEFAULT '0',
  `GananciaAGCRXdBm` int(11) DEFAULT '0',
  `SupresionSilencio` tinyint(1) DEFAULT '1',
  `TamRTP` int(10) unsigned DEFAULT '20',
  `Codec` int(1) unsigned DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `ParametrosRecurso_FKIndex1` (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  CONSTRAINT `parametrosrecurso_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRecurso`, `TipoRecurso`) REFERENCES `recursos` (`IdSistema`, `IdRecurso`, `TipoRecurso`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `parametrossector`
--

DROP TABLE IF EXISTS `parametrossector`;
CREATE TABLE `parametrossector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `NumLlamadasEntrantesIDA` int(10) unsigned DEFAULT '5',
  `NumLlamadasEnIDA` int(10) unsigned DEFAULT '10',
  `NumFreqPagina` int(10) unsigned DEFAULT NULL,
  `NumPagFreq` int(10) unsigned DEFAULT NULL,
  `NumDestinosInternosPag` int(10) unsigned DEFAULT NULL,
  `NumPagDestinosInt` int(10) unsigned DEFAULT NULL,
  `Intrusion` tinyint(1) DEFAULT '1',
  `Intruido` tinyint(1) DEFAULT '1',
  `KeepAlivePeriod` int(10) unsigned DEFAULT '200',
  `KeepAliveMultiplier` int(10) unsigned DEFAULT '10',
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `ParametrosSector_FKIndex1` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `parametrossector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `permisosredes`
--

DROP TABLE IF EXISTS `permisosredes`;
CREATE TABLE `permisosredes` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRed` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `Llamar` tinyint(1) DEFAULT NULL,
  `Recibir` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdRed`,`IdSector`,`IdNucleo`),
  KEY `PermisosRedes_FKIndex1` (`IdSistema`,`IdRed`),
  KEY `PermisosRedes_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `permisosredes_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRed`) REFERENCES `redes` (`IdSistema`, `IdRed`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `permisosredes_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `prefijos`
--

DROP TABLE IF EXISTS `prefijos`;
CREATE TABLE `prefijos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdPrefijo`),
  KEY `Prefijos_FKIndex1` (`IdSistema`),
  CONSTRAINT `fk_prefijos_sistema` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `radio`
--

DROP TABLE IF EXISTS `radio`;
CREATE TABLE `radio` (
  `IdSistema` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `PosHMI` int(10) unsigned NOT NULL,
  `IdDestino` varchar(32) NOT NULL,
  `Prioridad` int(1) unsigned DEFAULT NULL,
  `PrioridadSIP` int(1) unsigned DEFAULT NULL,
  `ModoOperacion` varchar(1) DEFAULT NULL,
  `Cascos` varchar(1) DEFAULT NULL,
  `Literal` varchar(32) DEFAULT NULL,
  `SupervisionPortadora` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`PosHMI`,`IdDestino`),
  KEY `Table_42_FKIndex1` (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`),
  CONSTRAINT `radio_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) REFERENCES `sectoressectorizacion` (`IdSistema`, `IdSectorizacion`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `rangos`
--

DROP TABLE IF EXISTS `rangos`;
CREATE TABLE `rangos` (
  `IdSistema` varchar(32) NOT NULL,
  `Central` varchar(32) NOT NULL,
  `Tipo` varchar(1) NOT NULL,
  `Inicial` varchar(6) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `IdAbonado` varchar(32) NOT NULL,
  `Final` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`Tipo`,`Central`,`Inicial`),
  KEY `Rangos_FKIndex2` (`IdSistema`,`IdPrefijo`),
  KEY `Rangos_FKIndex4` (`IdSistema`,`Central`),
  CONSTRAINT `FK_Encaminamientos` FOREIGN KEY (`IdSistema`, `Central`) REFERENCES `encaminamientos` (`IdSistema`, `Central`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `recursos`
--

DROP TABLE IF EXISTS `recursos`;
CREATE TABLE `recursos` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL DEFAULT '1',
  `idEquipos` varchar(32) DEFAULT NULL,
  `IdTIFX` varchar(32) DEFAULT NULL,
  `Tipo` int(1) unsigned NOT NULL DEFAULT '0',
  `Interface` int(2) unsigned DEFAULT '0',
  `SlotPasarela` int(1) DEFAULT '-1',
  `NumDispositivoSlot` int(1) DEFAULT '-1',
  `ServidorSIP` varchar(32) DEFAULT '0.0.0.0:5060',
  `Diffserv` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RECURSOS_FKIndex1` (`IdSistema`),
  KEY `RECURSOS_FKIndex2` (`IdSistema`,`IdTIFX`),
  KEY `RECURSOS_FKIndex3` (`idEquipos`,`IdSistema`),
  CONSTRAINT `recursos_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `recursos_ibfk_2` FOREIGN KEY (`IdSistema`, `IdTIFX`) REFERENCES `tifx` (`IdSistema`, `IdTIFX`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `recursoslcen`
--

DROP TABLE IF EXISTS `recursoslcen`;
CREATE TABLE `recursoslcen` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned DEFAULT NULL,
  `IdDestino` varchar(32) DEFAULT NULL,
  `TipoDestino` int(10) unsigned DEFAULT NULL,
  `T1` int(10) unsigned DEFAULT '20',
  `T1Max` int(10) unsigned DEFAULT '100',
  `T2` int(10) unsigned DEFAULT '20',
  `T2Max` int(10) unsigned DEFAULT '100',
  `T3` int(10) unsigned DEFAULT '40',
  `T4` int(10) unsigned DEFAULT '40',
  `T4Max` int(10) unsigned DEFAULT '160',
  `T5` int(10) unsigned DEFAULT '60',
  `T5Max` int(10) unsigned DEFAULT '100',
  `T6` int(10) unsigned DEFAULT '5000',
  `T6Max` int(10) unsigned DEFAULT '6000',
  `T8` int(10) unsigned DEFAULT '80',
  `T8Max` int(10) unsigned DEFAULT '150',
  `T9` int(10) unsigned DEFAULT '40',
  `T9Max` int(10) unsigned DEFAULT '60',
  `T10` int(10) unsigned DEFAULT '20',
  `T10Max` int(10) unsigned DEFAULT '130',
  `T11` int(10) unsigned DEFAULT '20',
  `T11Max` int(10) unsigned DEFAULT '130',
  `T12` int(10) unsigned DEFAULT '200',
  `FrqTonoSQ` int(10) unsigned DEFAULT '2280',
  `UmbralTonoSQ` int(11) DEFAULT '-35',
  `FrqTonoPTT` int(10) unsigned DEFAULT '2280',
  `UmbralTonoPTT` int(11) DEFAULT '-10',
  `RefrescoEstados` int(10) unsigned DEFAULT '200',
  `Timeout` int(10) unsigned DEFAULT '600',
  `LongRafagas` int(10) unsigned DEFAULT '6',
  PRIMARY KEY (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosLCEN_FKIndex1` (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosLCEN_FKIndex2` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  CONSTRAINT `recursoslcen_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRecurso`, `TipoRecurso`) REFERENCES `recursos` (`IdSistema`, `IdRecurso`, `TipoRecurso`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `recursoslcen_ibfk_2` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinosexternos` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `recursosradio`
--

DROP TABLE IF EXISTS `recursosradio`;
CREATE TABLE `recursosradio` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL,
  `TipoDestino` int(10) unsigned DEFAULT NULL,
  `IdDestino` varchar(32) DEFAULT NULL,
  `IdEmplazamiento` varchar(32) NOT NULL,
  `EM` tinyint(1) DEFAULT NULL,
  `SQ` varchar(1) DEFAULT NULL,
  `PTT` varchar(1) DEFAULT NULL,
  `FrqTonoE` int(10) unsigned DEFAULT NULL,
  `UmbralTonoE` int(11) DEFAULT NULL,
  `FrqTonoM` int(10) unsigned DEFAULT NULL,
  `UmbralTonoM` int(11) DEFAULT NULL,
  `FrqTonoSQ` int(10) unsigned DEFAULT NULL,
  `UmbralTonoSQ` int(11) DEFAULT NULL,
  `FrqTonoPTT` int(10) unsigned DEFAULT NULL,
  `UmbralTonoPTT` int(11) DEFAULT NULL,
  `BSS` tinyint(1) DEFAULT NULL,
  `NTZ` tinyint(1) DEFAULT NULL,
  `TipoNTZ` int(1) unsigned DEFAULT '0',
  `Cifrado` tinyint(1) DEFAULT NULL,
  `SupervPortadoraTX` tinyint(1) DEFAULT NULL,
  `SupervModuladoraTX` tinyint(1) DEFAULT NULL,
  `ModoConfPTT` int(1) unsigned DEFAULT NULL,
  `RepSQyBSS` int(10) unsigned DEFAULT '1',
  `DesactivacionSQ` int(10) unsigned DEFAULT '1',
  `TimeoutPTT` int(10) unsigned DEFAULT '200',
  `MetodoBSS` int(1) unsigned DEFAULT NULL,
  `UmbralVAD` int(11) DEFAULT '-33',
  `TiempoPTT` int(10) unsigned DEFAULT '120',
  `NumFlujosAudio` int(1) unsigned DEFAULT '1',
  `KeepAlivePeriod` int(10) unsigned DEFAULT '200',
  `KeepAliveMultiplier` int(10) unsigned DEFAULT '10',
  PRIMARY KEY (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosRadio_FKIndex1` (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosRadio_FKIndex2` (`IdSistema`,`IdEmplazamiento`),
  KEY `RecursosRadio_FKIndex3` (`IdSistema`,`IdDestino`,`TipoDestino`),
  CONSTRAINT `recursosradio_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRecurso`, `TipoRecurso`) REFERENCES `recursos` (`IdSistema`, `IdRecurso`, `TipoRecurso`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `recursosradio_ibfk_2` FOREIGN KEY (`IdSistema`, `IdEmplazamiento`) REFERENCES `emplazamientos` (`IdSistema`, `IdEmplazamiento`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `recursosradio_ibfk_3` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`) REFERENCES `destinosradio` (`IdSistema`, `IdDestino`, `TipoDestino`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `recursostf`
--

DROP TABLE IF EXISTS `recursostf`;
CREATE TABLE `recursostf` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRecurso` varchar(32) NOT NULL,
  `TipoRecurso` int(1) unsigned NOT NULL,
  `IdPrefijo` int(10) unsigned DEFAULT NULL,
  `TipoDestino` int(10) unsigned DEFAULT NULL,
  `IdDestino` varchar(32) DEFAULT NULL,
  `IdTroncal` varchar(32) DEFAULT NULL,
  `IdRed` varchar(32) DEFAULT NULL,
  `Lado` varchar(1) NOT NULL DEFAULT 'A',
  `Modo` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosTF_FKIndex1` (`IdSistema`,`IdRecurso`,`TipoRecurso`),
  KEY `RecursosTF_FKIndex2` (`IdSistema`,`IdRed`),
  KEY `RecursosTF_FKIndex3` (`IdSistema`,`IdTroncal`),
  KEY `RecursosTF_FKIndex4` (`IdSistema`,`IdDestino`,`TipoDestino`,`IdPrefijo`),
  CONSTRAINT `recursostf_ibfk_1` FOREIGN KEY (`IdSistema`, `IdRecurso`, `TipoRecurso`) REFERENCES `recursos` (`IdSistema`, `IdRecurso`, `TipoRecurso`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `recursostf_ibfk_2` FOREIGN KEY (`IdSistema`, `IdRed`) REFERENCES `redes` (`IdSistema`, `IdRed`) ON DELETE NO ACTION ON UPDATE CASCADE,
  CONSTRAINT `recursostf_ibfk_3` FOREIGN KEY (`IdSistema`, `IdTroncal`) REFERENCES `troncales` (`IdSistema`, `IdTroncal`) ON DELETE NO ACTION ON UPDATE CASCADE,
  CONSTRAINT `recursostf_ibfk_4` FOREIGN KEY (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) REFERENCES `destinosexternos` (`IdSistema`, `IdDestino`, `TipoDestino`, `IdPrefijo`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `redes`
--

DROP TABLE IF EXISTS `redes`;
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


--
-- Definition of trigger `REDDELETE`
--

DROP TRIGGER /*!50030 IF EXISTS */ `REDDELETE`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `REDDELETE` BEFORE DELETE ON `redes` FOR EACH ROW UPDATE RECURSOSTF SET IdRed=NULL 
	WHERE IdRed=old.IdRed AND IdSistema=old.IdSistema $$

DELIMITER ;

--
-- Definition of table `registrobackup`
--

DROP TABLE IF EXISTS `registrobackup`;
CREATE TABLE `registrobackup` (
  `IdSistema` varchar(32) NOT NULL,
  `TipoBackup` int(11) NOT NULL,
  `FechaInicio` datetime NOT NULL,
  `FechaFin` datetime NOT NULL,
  `RecursoDestino` varchar(128) NOT NULL,
  PRIMARY KEY (`IdSistema`,`TipoBackup`,`FechaInicio`),
  CONSTRAINT `FK_RegistroBackup_Sistema` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `registrotareas`
--

DROP TABLE IF EXISTS `registrotareas`;
CREATE TABLE `registrotareas` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTarea` varchar(32) NOT NULL,
  `Ejecutada` datetime NOT NULL,
  `Resultado` int(11) NOT NULL,
  `Comentario` varchar(125) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdTarea`,`Ejecutada`),
  CONSTRAINT `Tareas` FOREIGN KEY (`IdSistema`, `IdTarea`) REFERENCES `tareas` (`IdSistema`, `IdTarea`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `rutas`
--

DROP TABLE IF EXISTS `rutas`;
CREATE TABLE `rutas` (
  `IdSistema` varchar(32) NOT NULL,
  `IdRuta` varchar(32) NOT NULL,
  `Central` varchar(32) NOT NULL,
  `Tipo` varchar(1) DEFAULT NULL,
  `Orden` int(11) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdRuta`,`Central`),
  KEY `Rutas_FKIndex1` (`IdSistema`,`Central`),
  CONSTRAINT `rutas_ibfk_1` FOREIGN KEY (`IdSistema`, `Central`) REFERENCES `encaminamientos` (`IdSistema`, `Central`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `sectores`
--

DROP TABLE IF EXISTS `sectores`;
CREATE TABLE `sectores` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdSistemaParejaUCS` varchar(32) DEFAULT NULL,
  `IdNucleoParejaUCS` varchar(32) DEFAULT NULL,
  `IdParejaUCS` varchar(32) DEFAULT NULL,
  `SectorSimple` tinyint(1) DEFAULT '1',
  `Tipo` varchar(1) DEFAULT 'R',
  `TipoPosicion` varchar(1) DEFAULT 'C',
  `PrioridadR2` int(1) unsigned DEFAULT '4',
  `TipoHMI` int(10) unsigned DEFAULT NULL,
  `NumSacta` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `Sectores_FKIndex1` (`IdSistema`,`IdNucleo`),
  KEY `Sectores_FKIndex3` (`IdSistemaParejaUCS`,`IdNucleoParejaUCS`,`IdParejaUCS`),
  CONSTRAINT `sectores_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`) REFERENCES `nucleos` (`IdSistema`, `IdNucleo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `sectores_ibfk_3` FOREIGN KEY (`IdSistemaParejaUCS`, `IdNucleoParejaUCS`, `IdParejaUCS`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `AltaSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaSector` AFTER INSERT ON `sectores` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of trigger `ActualizaSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `ActualizaSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `ActualizaSector` AFTER UPDATE ON `sectores` FOR EACH ROW BEGIN
    UPDATE Destinos 
			SET IdDestino=new.IdSector
			WHERE IdSistema=new.IdSistema AND
													IdDestino=old.IdSector AND
													TipoDestino=2 ;
END $$

DELIMITER ;

--
-- Definition of trigger `BajaSector`
--

DROP TRIGGER /*!50030 IF EXISTS */ `BajaSector`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `BajaSector` BEFORE DELETE ON `sectores` FOR EACH ROW BEGIN
    DELETE FROM Destinos WHERE IdSistema=old.IdSistema AND
																IdDestino=old.IdSector AND
																TipoDestino=2 ;
END $$

DELIMITER ;

--
-- Definition of table `sectoresagrupacion`
--

DROP TABLE IF EXISTS `sectoresagrupacion`;
CREATE TABLE `sectoresagrupacion` (
  `IdSistema` varchar(32) NOT NULL,
  `IdAgrupacion` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdAgrupacion`,`IdSector`,`IdNucleo`),
  KEY `Sectores_has_Agrupaciones_FKIndex1` (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `Sectores_has_Agrupaciones_FKIndex2` (`IdSistema`,`IdAgrupacion`),
  CONSTRAINT `sectoresagrupacion_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `sectoresagrupacion_ibfk_2` FOREIGN KEY (`IdSistema`, `IdAgrupacion`) REFERENCES `agrupaciones` (`IdSistema`, `IdAgrupacion`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `sectoressector`
--

DROP TABLE IF EXISTS `sectoressector`;
CREATE TABLE `sectoressector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdSectorOriginal` varchar(32) NOT NULL,
  `EsDominante` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`,`IdSectorOriginal`),
  KEY `SectoresSector_FKIndex1` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `sectoressector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `sectoressectorizacion`
--

DROP TABLE IF EXISTS `sectoressectorizacion`;
CREATE TABLE `sectoressectorizacion` (
  `IdSistema` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdTOP` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdSectorizacion`,`IdNucleo`,`IdSector`,`IdTOP`),
  KEY `SectoresSectorizacion_FKIndex1` (`IdSistema`,`IdSectorizacion`),
  KEY `SectoresSectorizacion_FKIndex2` (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `SectoresSectorizacion_FKIndex3` (`IdSistema`,`IdTOP`),
  CONSTRAINT `sectoressectorizacion_ibfk_1` FOREIGN KEY (`IdSistema`, `IdSectorizacion`) REFERENCES `sectorizaciones` (`IdSistema`, `IdSectorizacion`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `sectoressectorizacion_ibfk_2` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `sectoressectorizacion_ibfk_3` FOREIGN KEY (`IdSistema`, `IdTOP`) REFERENCES `top` (`IdSistema`, `IdTOP`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `sectorizaciones`
--

DROP TABLE IF EXISTS `sectorizaciones`;
CREATE TABLE `sectorizaciones` (
  `IdSistema` varchar(32) NOT NULL,
  `IdSectorizacion` varchar(32) NOT NULL,
  `Activa` tinyint(1) DEFAULT NULL,
  `FechaActivacion` datetime DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdSectorizacion`),
  KEY `Sectorizaciones_FKIndex1` (`IdSistema`),
  CONSTRAINT `sectorizaciones_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `sistema`
--

DROP TABLE IF EXISTS `sistema`;
CREATE TABLE `sistema` (
  `IdSistema` varchar(32) NOT NULL,
  `TiempoPTT` int(10) unsigned DEFAULT NULL,
  `TiempoSinJack1` int(10) unsigned DEFAULT NULL,
  `TiempoSinJack2` int(10) unsigned DEFAULT NULL,
  `TamLiteralEnlExt` int(10) unsigned DEFAULT NULL,
  `TamLiteralDA` int(10) unsigned DEFAULT NULL,
  `TamLiteralIA` int(10) unsigned DEFAULT NULL,
  `TamLiteralAG` int(10) unsigned DEFAULT NULL,
  `TamLiteralEmplazamiento` int(10) unsigned DEFAULT NULL,
  `VersionIP` int(1) unsigned DEFAULT NULL,
  `GrupoMulticastConfiguracion` varchar(15) DEFAULT NULL,
  `PuertoMulticastConfiguracion` int(5) unsigned DEFAULT '1000',
  `EstadoScvA` int(1) unsigned DEFAULT '0',
  `EstadoScvB` int(1) unsigned DEFAULT '0',
  PRIMARY KEY (`IdSistema`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `AltaSistema`
--

DROP TRIGGER /*!50030 IF EXISTS */ `AltaSistema`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `AltaSistema` AFTER INSERT ON `sistema` FOR EACH ROW BEGIN
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
END $$

DELIMITER ;

--
-- Definition of table `tablasmodificadas`
--

DROP TABLE IF EXISTS `tablasmodificadas`;
CREATE TABLE `tablasmodificadas` (
  `IdTabla` varchar(32) NOT NULL,
  PRIMARY KEY (`IdTabla`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `tareas`
--

DROP TABLE IF EXISTS `tareas`;
CREATE TABLE `tareas` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTarea` varchar(32) NOT NULL,
  `Programa` varchar(255) NOT NULL,
  `Argumentos` varchar(255) DEFAULT NULL,
  `Hora` time NOT NULL,
  `Periodicidad` char(1) NOT NULL,
  `Intervalo` int(3) DEFAULT '1',
  `Comentario` varchar(125) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdTarea`),
  CONSTRAINT `Sistema` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `teclassector`
--

DROP TABLE IF EXISTS `teclassector`;
CREATE TABLE `teclassector` (
  `IdSistema` varchar(32) NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `TransConConsultaPrev` tinyint(1) DEFAULT '1',
  `TransDirecta` tinyint(1) DEFAULT '0',
  `Conferencia` tinyint(1) DEFAULT '0',
  `Escucha` tinyint(1) DEFAULT '0',
  `Retener` tinyint(1) DEFAULT '1',
  `Captura` tinyint(1) DEFAULT '0',
  `Redireccion` tinyint(1) DEFAULT '0',
  `RepeticionUltLlamada` tinyint(1) DEFAULT '1',
  `RellamadaAut` tinyint(1) DEFAULT '0',
  `TeclaPrioridad` tinyint(1) DEFAULT '1',
  `Tecla55mas1` tinyint(1) DEFAULT '0',
  `Monitoring` tinyint(1) DEFAULT '1',
  `CoordinadorTF` tinyint(1) DEFAULT '0',
  `CoordinadorRD` tinyint(1) DEFAULT '0',
  `IntegracionRDTF` tinyint(1) DEFAULT '0',
  `LlamadaSelectiva` tinyint(1) DEFAULT '0',
  `GrupoBSS` tinyint(1) DEFAULT '1',
  `LTT` tinyint(1) DEFAULT '1',
  `SayAgain` tinyint(1) DEFAULT '1',
  `InhabilitacionRedirec` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`IdSistema`,`IdNucleo`,`IdSector`),
  KEY `TeclasSector_FKIndex1` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `teclassector_ibfk_1` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `tifx`
--

DROP TABLE IF EXISTS `tifx`;
CREATE TABLE `tifx` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTIFX` varchar(32) NOT NULL,
  `ModoArranque` varchar(1) DEFAULT NULL,
  `ModoSincronizacion` int(1) unsigned DEFAULT NULL,
  `Master` varchar(32) DEFAULT '0.0.0.0:0',
  `SNMPPortLocal` int(10) unsigned DEFAULT '161',
  `SNMPPortRemoto` int(10) unsigned DEFAULT '161',
  `SNMPTraps` int(10) unsigned DEFAULT '162',
  `SIPPortLocal` int(10) unsigned DEFAULT '5060',
  `TimeSupervision` int(10) unsigned DEFAULT '0',
  `IpRed1` varchar(60) DEFAULT NULL,
  `IpRed2` varchar(60) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdTIFX`),
  KEY `TIFX_FKIndex1` (`IdSistema`),
  CONSTRAINT `tifx_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of trigger `TIFXDELETE`
--

DROP TRIGGER /*!50030 IF EXISTS */ `TIFXDELETE`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `TIFXDELETE` BEFORE DELETE ON `tifx` FOR EACH ROW UPDATE RECURSOS SET IdTIFX=NULL 
		WHERE IdSistema = old.IdSistema AND IdTIFX= old.IdTIFX $$

DELIMITER ;

--
-- Definition of table `top`
--

DROP TABLE IF EXISTS `top`;
CREATE TABLE `top` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTOP` varchar(32) NOT NULL,
  `PosicionSacta` int(10) unsigned DEFAULT NULL,
  `ModoArranque` varchar(1) DEFAULT NULL,
  `IpRed1` varchar(60) DEFAULT NULL,
  `IpRed2` varchar(60) DEFAULT NULL,
  `ConexionJacksEjecutivo` tinyint(1) DEFAULT '1',
  `ConexionJacksAyudante` tinyint(1) DEFAULT '1',
  `NumAltavoces` smallint(1) DEFAULT '8',
  PRIMARY KEY (`IdSistema`,`IdTOP`),
  KEY `TOP_FKIndex1` (`IdSistema`),
  CONSTRAINT `top_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Definition of table `troncales`
--

DROP TABLE IF EXISTS `troncales`;
CREATE TABLE `troncales` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTroncal` varchar(32) NOT NULL,
  `NumTest` varchar(16) DEFAULT NULL,
  PRIMARY KEY (`IdSistema`,`IdTroncal`),
  KEY `Troncales_FKIndex1` (`IdSistema`),
  CONSTRAINT `troncales_ibfk_1` FOREIGN KEY (`IdSistema`) REFERENCES `sistema` (`IdSistema`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `TRONCALDELETE`
--

DROP TRIGGER /*!50030 IF EXISTS */ `TRONCALDELETE`;

DELIMITER $$

CREATE DEFINER = `root`@`localhost` TRIGGER `TRONCALDELETE` BEFORE DELETE ON `troncales` FOR EACH ROW UPDATE RECURSOSTF SET IdTroncal=NULL 
		WHERE IdTroncal=old.IdTroncal AND IdSistema=old.IdSistema $$

DELIMITER ;

--
-- Definition of table `troncalesruta`
--

DROP TABLE IF EXISTS `troncalesruta`;
CREATE TABLE `troncalesruta` (
  `IdSistema` varchar(32) NOT NULL,
  `IdTroncal` varchar(32) NOT NULL,
  `Central` varchar(32) NOT NULL,
  `IdRuta` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdTroncal`,`Central`,`IdRuta`),
  KEY `TroncalesRuta_FKIndex1` (`IdSistema`,`IdTroncal`),
  KEY `TroncalesRuta_FKIndex2` (`IdSistema`,`IdRuta`,`Central`),
  CONSTRAINT `troncalesruta_ibfk_1` FOREIGN KEY (`IdSistema`, `IdTroncal`) REFERENCES `troncales` (`IdSistema`, `IdTroncal`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `troncalesruta_ibfk_2` FOREIGN KEY (`IdSistema`, `IdRuta`, `Central`) REFERENCES `rutas` (`IdSistema`, `IdRuta`, `Central`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `usuariosabonados`
--

DROP TABLE IF EXISTS `usuariosabonados`;
CREATE TABLE `usuariosabonados` (
  `IdSistema` varchar(32) NOT NULL,
  `IdPrefijo` int(10) unsigned NOT NULL,
  `IdNucleo` varchar(32) NOT NULL,
  `IdSector` varchar(32) NOT NULL,
  `IdAbonado` varchar(32) NOT NULL,
  PRIMARY KEY (`IdSistema`,`IdPrefijo`,`IdAbonado`,`IdNucleo`,`IdSector`),
  KEY `UsuariosAbonados_FKIndex2` (`IdSistema`,`IdPrefijo`),
  KEY `UsuariosAbonados_FKIndex3` (`IdSistema`,`IdNucleo`,`IdSector`),
  CONSTRAINT `usuariosabonados_ibfk_2` FOREIGN KEY (`IdSistema`, `IdPrefijo`) REFERENCES `prefijos` (`IdSistema`, `IdPrefijo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `usuariosabonados_ibfk_3` FOREIGN KEY (`IdSistema`, `IdNucleo`, `IdSector`) REFERENCES `sectores` (`IdSistema`, `IdNucleo`, `IdSector`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Create schema cd40
--

CREATE DATABASE IF NOT EXISTS cd40;
USE cd40;

--
-- Definition of procedure `ActualizaNombreSector`
--

DROP PROCEDURE IF EXISTS `ActualizaNombreSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ActualizaNombreSector`(in id_sistema char(32), in id_nucleo char(32), in id_sector char(32), in id_nuevo_nombre_sector char(32))
BEGIN
	UPDATE Sectores 
		SET IdSector=id_nuevo_nombre_sector
		WHERE IdSistema=id_sistema AND
					IdNucleo=id_nucleo AND
					IdSector=id_sector AND
					SectorSimple=FALSE;

		REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('Sectores');
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ActualizaSectoresSectorizacion`
--

DROP PROCEDURE IF EXISTS `ActualizaSectoresSectorizacion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ActualizaSectoresSectorizacion`(in id_sistema varchar(32), in id_nucleo varchar(32), in id_grupo varchar(32), in id_agrupacion varchar(32))
BEGIN
	DECLARE id char(32);
	
	SELECT DATE_FORMAT(FechaActivacion,"%d/%m/%Y %H:%i:%s") INTO id FROM sectorizaciones WHERE IdSistema=id_sistema AND Activa=true;
	
	UPDATE SectoresSectorizacion
		SET IdSector=id_agrupacion
		WHERE IdSistema=id_sistema AND
					IdNucleo=id_nucleo AND
					IdSector=id_grupo AND
					IdSectorizacion<>id;

	REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('SectoresSectorizacion');
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AgrupacionDeLosSectores`
--

DROP PROCEDURE IF EXISTS `AgrupacionDeLosSectores`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `AgrupacionDeLosSectores`(in cuantos int, in lista_usuarios char(55))
BEGIN
	DECLARE lista CHAR(55);
	SET @lista=lista_usuarios;
	SELECT sa.IdAgrupacion,@lista,lista FROM SectoresAgrupacion sa, viewSectoresEnAgrupacion cuenta
	WHERE cuenta.contador=cuantos AND
				sa.IdAgrupacion=cuenta.IdAgrupacion	AND
				sa.IdSector in (@lista);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AsignacionRecursosDeUnaRed`
--

DROP PROCEDURE IF EXISTS `AsignacionRecursosDeUnaRed`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AsignacionRecursosDeUnaRed`(in id_sistema char(32), in id_red char(32))
BEGIN
	IF (id_red is not null) THEN
		SELECT rTf.IdRed, r.IdRecurso, r.IdTifx, r.SlotPasarela, r.NumDispositivoSlot
		FROM RecursosTf rTf, Recursos r
		WHERE rTf.IdSistema=id_sistema AND
					rTf.IdRed=id_red AND
					r.IdSistema=rTf.IdSistema AND
					r.IdRecurso=rTf.IdRecurso;
	ELSE
		(SELECT rd.IdRed, r.IdRecurso, r.IdTifx, r.SlotPasarela, r.NumDispositivoSlot
		FROM RecursosTf rTf, Recursos r, Redes rd
		WHERE rd.IdSistema=id_sistema AND
					rTf.IdSistema=rd.IdSistema AND
					rd.IdRed=rTf.IdRed AND
					r.IdSistema=rTf.IdSistema AND
					r.IdRecurso=rTf.IdRecurso)
	UNION
		(SELECT rd.IdRed, null,null,null,null
		FROM Redes rd
		WHERE rd.IdSistema=id_sistema AND
					rd.IdRed not in (SELECT DISTINCT(IdRed) FROM RecursosTf WHERE idRed is not null));
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AsignacionUsuariosATops`
--

DROP PROCEDURE IF EXISTS `AsignacionUsuariosATops`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AsignacionUsuariosATops`(in id_Sistema char(32))
BEGIN
	SELECT c.IdSectorOriginal AS IdSector,a.IdTop FROM SectoresSectorizacion a, Sectorizaciones b, SectoresSector c
	WHERE b.IdSistema=id_Sistema AND
				b.Activa=true AND
				a.IdSistema=id_Sistema AND
				a.IdSectorizacion=b.IdSectorizacion AND
				c.IdSistema=id_Sistema AND
				c.IdNucleo=a.IdNucleo AND
				c.IdSector=a.IdSector; 
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CreaPosicionesActiva`
--

DROP PROCEDURE IF EXISTS `CreaPosicionesActiva`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreaPosicionesActiva`(in id_sistema char(32), in id_sectorizacion char(32), in id_activa char(32))
BEGIN
		INSERT into radio
			SELECT IdSistema,id_activa as IdSectorizacion,IdNucleo,IdSector,PosHMI,IdDestino,Prioridad,PrioridadSIP,ModoOperacion,Cascos,Literal,SupervisionPortadora FROM Radio
				WHERE IdSectorizacion=id_sectorizacion AND
							IdSistema=id_sistema;
		REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('radio');
	
		INSERT into Internos
			SELECT IdSistema,id_activa as IdSectorizacion,IdNucleo,IdSector,IdDestino,IdPrefijo,TipoAcceso,PosHMI,Prioridad,OrigenR2,PrioridadSIP,Literal FROM Internos
				WHERE IdSectorizacion=id_sectorizacion AND
							IdSistema=id_sistema;
		REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('Internos');
		
		INSERT into Externos
			SELECT IdSistema,id_activa as IdSectorizacion,IdNucleo,IdSector,IdDestino,IdPrefijo,TipoAcceso,PosHMI,Prioridad,OrigenR2,PrioridadSIP,Literal FROM Externos
				WHERE IdSectorizacion=id_sectorizacion AND
							IdSistema=id_sistema;
		REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('Externos');
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CrearFicheroBackup`
--

DROP PROCEDURE IF EXISTS `CrearFicheroBackup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CrearFicheroBackup`(in id_sistema char(32), in tabla text, in fichero text,in fDesde datetime, in fHasta datetime)
BEGIN

DECLARE lista CHAR(55);



set @SQL = concat("SELECT *",
			"INTO OUTFILE '", fichero, "'", 
      " FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' LINES TERMINATED BY '\\n'",
      " FROM ", tabla,
      " WHERE IdSistema='", id_sistema,
			"' AND DATE_FORMAT(FechaHora,'%Y/%m/%d')>='",DATE_FORMAT(fDesde,'%Y/%m/%d'), 
			"' AND DATE_FORMAT(FechaHora,'%Y/%m/%d')< '",DATE_FORMAT(fHasta,'%Y/%m/%d'),
			"';"); 


PREPARE stmt FROM @SQL; 
EXECUTE stmt; 

set @SQL_DELETE = concat("DELETE FROM ", tabla,
      " WHERE IdSistema='", id_sistema,
			"' AND DATE_FORMAT(FechaHora,'%Y/%m/%d')>='",DATE_FORMAT(fDesde,'%Y/%m/%d'), 
			"' AND DATE_FORMAT(FechaHora,'%Y/%m/%d')< '",DATE_FORMAT(fHasta,'%Y/%m/%d'),
			"';"); 


PREPARE stmt_delete FROM @SQL_DELETE; 
EXECUTE stmt_delete; 
	




END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CreaSectoresActiva`
--

DROP PROCEDURE IF EXISTS `CreaSectoresActiva`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreaSectoresActiva`(in id_sistema char(32), in id_sectorizacion char(32), in id_activa char(32))
BEGIN
	INSERT into SectoresSectorizacion
	SELECT IdSistema,id_activa as IdSectorizacion,IdNucleo,IdSector,IdTop FROM SectoresSectorizacion
		WHERE IdSectorizacion=id_sectorizacion AND
					IdSistema=id_sistema;
					
	REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('SectoresSectorizacion');
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CreaSectoresActivaConexionSacta`
--

DROP PROCEDURE IF EXISTS `CreaSectoresActivaConexionSacta`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreaSectoresActivaConexionSacta`(in id_sistema char(32), in id_sectorizacion char(32), in id_activa char(32))
BEGIN
	INSERT into SectoresSectorizacion
		SELECT ss.IdSistema,id_activa as IdSectorizacion,ss.IdNucleo,ss.IdSector,ss.IdTOP FROM SectoresSectorizacion ss, Sectores s
		WHERE ss.IdSistema=id_sistema AND
					(ss.IdSectorizacion='SACTA' OR
						(ss.IdSectorizacion=id_sectorizacion AND 
							s.Tipo='M' AND	
							ss.IdTOP NOT IN (SELECT idtop 
																	FROM sectoressectorizacion 
																	WHERE idsistema=id_sistema AND idsectorizacion='SACTA'))) AND
					ss.IdSector=s.IdSector;

	REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('SectoresSectorizacion');
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CuantasTeclasConPrioridadUno`
--

DROP PROCEDURE IF EXISTS `CuantasTeclasConPrioridadUno`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CuantasTeclasConPrioridadUno`(in id_sistema varchar(32), in id_nucleo varchar(32), out cuantas int)
BEGIN
  DECLARE howManyInt int default 0;
  DECLARE howManyExt int default 0;
  SET cuantas=0;
	
  SELECT COUNT(*) into howManyInt FROM DestinosInternosSector
    WHERE IdSistema=id_sistema AND 
          IdNucleo=id_nucleo AND
          IdPrefijo=0 AND
          Prioridad=1;

  SELECT COUNT(*) into howManyExt FROM DestinosExternosSector
    WHERE IdSistema=id_sistema AND 
          IdNucleo=id_nucleo AND
          IdPrefijo=1 AND
          Prioridad=1;

  SET cuantas=howManyInt + howManyExt;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosExternosAsignadosASector`
--

DROP PROCEDURE IF EXISTS `DestinosExternosAsignadosASector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosExternosAsignadosASector`(in id_sistema char(32), in id_sector char(32), in telefonia BOOL)
BEGIN
	IF telefonia=true THEN
		IF id_sector IS NOT NULL THEN
			SELECT * FROM DestinosExternosSector
			WHERE IdSistema=id_sistema AND
						IdSector=id_sector AND
						IdPrefijo<>1;
		ELSE
			SELECT * FROM DestinosExternosSector
			WHERE IdSistema=id_sistema AND
						IdPrefijo<>1;
		END IF;
	ELSE
		IF id_sector IS NOT NULL THEN
			SELECT * FROM DestinosExternosSector
			WHERE IdSistema=id_sistema AND
						IdSector=id_sector AND
						IdPrefijo=1;
		ELSE
			SELECT * FROM DestinosExternosSector
			WHERE IdSistema=id_sistema AND
						IdPrefijo=1;
		END IF;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosInstantaneosSinAsignarAEnlaces`
--

DROP PROCEDURE IF EXISTS `DestinosInstantaneosSinAsignarAEnlaces`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosInstantaneosSinAsignarAEnlaces`(in id_sistema char(32))
BEGIN
	SELECT * FROM Destinos
	WHERE IdDestino not in (SELECT IdDestino FROM EnlacesLCEN 
														WHERE IdSistema=id_sistema AND IdPrefijo in ("0","1")) AND
				IdSistema=id_sistema AND
				IdPrefijo in ("0","1");
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosInternosAsignadosASector`
--

DROP PROCEDURE IF EXISTS `DestinosInternosAsignadosASector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosInternosAsignadosASector`(in id_sistema char(32), in id_sector char(32), in telefonia BOOL)
BEGIN
	IF telefonia=true THEN
		IF id_sector IS NOT NULL THEN
			SELECT * FROM DestinosInternosSector
			WHERE IdSistema=id_sistema AND
						IdSector=id_sector AND
						IdPrefijo=2;
		ELSE
			SELECT * FROM DestinosInternosSector
			WHERE IdSistema=id_sistema AND
						IdPrefijo=2;
		END IF;
	ELSE
		IF id_sector IS NOT NULL THEN
			SELECT * FROM DestinosInternosSector
			WHERE IdSistema=id_sistema AND
						IdSector=id_sector AND
						IdPrefijo=0;
		ELSE
			SELECT * FROM DestinosInternosSector
			WHERE IdSistema=id_sistema AND
						IdPrefijo=0;
		END IF;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosInternosSinAsignarAEnlaces`
--

DROP PROCEDURE IF EXISTS `DestinosInternosSinAsignarAEnlaces`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosInternosSinAsignarAEnlaces`(in id_sistema char(32))
BEGIN
	SELECT * FROM Destinos
	WHERE IdDestino not in (SELECT IdDestino FROM EnlacesInternos 
														WHERE IdSistema=id_sistema AND IdPrefijo <> "1") AND
				IdSistema=id_sistema AND
				IdPrefijo <> "1";
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosLineaCalienteSinAsignarAlSector`
--

DROP PROCEDURE IF EXISTS `DestinosLineaCalienteSinAsignarAlSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosLineaCalienteSinAsignarAlSector`(in id_sistema char(32), in id_usuario char(32))
BEGIN
		(select a.* from destinosTelefonia a, destinosInternos b, Sectores c
			where a.IdSistema=id_sistema AND
					a.IdPrefijo=0 AND 
					a.IdSistema=b.IdSistema AND
					a.IdPrefijo=b.IdPrefijo AND
					a.IdDestino=b.IdDestino AND
					b.IdSector<>id_usuario AND
					c.IdSector=b.IdSector AND
					c.SectorSimple=true AND
					b.IdDestino not in (SELECT IdDestino FROM DestinosInternosSector
															WHERE IdSistema=id_sistema AND
																		IdSector=id_usuario AND
																		IdPrefijo=0))
			UNION
			(select a.* from destinosTelefonia a
			where a.IdSistema=id_sistema AND
					a.IdPrefijo=1 AND 
					a.IdDestino not in (SELECT IdDestino FROM DestinosExternosSector
															WHERE IdSistema=id_sistema AND
																		
																		IdPrefijo=1));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosPorGruposTelefonia`
--

DROP PROCEDURE IF EXISTS `DestinosPorGruposTelefonia`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosPorGruposTelefonia`(in id_sistema varchar(32))
BEGIN
	SELECT g.idgrupo,d.IdDestino
	FROM GruposTelefonia g, destinostelefonia d
	WHERE g.IdSistema=id_sistema AND
				d.IdSistema = g.IdSistema AND
				d.IdGrupo=g.IdGrupo
UNION
	SELECT idgrupo,null 
		FROM grupostelefonia 
		WHERE IdSistema=id_sistema AND 
					IdGrupo NOT IN (SELECT IdGrupo FROM destinostelefonia WHERE idSistema=id_sistema AND IdGrupo is not null);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosRadioAsignadosASector`
--

DROP PROCEDURE IF EXISTS `DestinosRadioAsignadosASector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `DestinosRadioAsignadosASector`(in id_sistema char(32), in id_usuario char(32), in pagina int, in frecPorPagina int)
BEGIN
	SELECT * FROM DestinosRadioSector
				WHERE Idsistema=id_sistema AND
							IdSector=id_usuario AND
							(((PosHMI-1) div frecPorPagina) = pagina);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosRadioSectorizados`
--

DROP PROCEDURE IF EXISTS `DestinosRadioSectorizados`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `DestinosRadioSectorizados`(in id_sistema char(32), in id_sector char(32), in id_sectorizacion char(32))
BEGIN
		SELECT * FROM radio
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdSector=id_sector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosRadioSectorizadosParaXML`
--

DROP PROCEDURE IF EXISTS `DestinosRadioSectorizadosParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosRadioSectorizadosParaXML`(in id_sistema char(32), in id_sectorizacion char(32))
BEGIN
	IF (id_sectorizacion IS NOT NULL) THEN
		SELECT * FROM radio
			WHERE IdSistema=id_sistema AND
						IdSectorizacion=id_sectorizacion;
	ELSE
		SELECT * FROM DestinosRadioSector
			WHERE IdSistema=id_sistema;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosRadioSinAsignarALaPaginaDelSector`
--

DROP PROCEDURE IF EXISTS `DestinosRadioSinAsignarALaPaginaDelSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `DestinosRadioSinAsignarALaPaginaDelSector`(in id_sistema char(32), in id_usuario char(32), in pagina int, in frecPorPagina int)
BEGIN
	SELECT IdDestino FROM DestinosRadio DR
	WHERE idSistema=id_sistema AND
				0 < (SELECT COUNT(*) FROM RecursosRadio RR WHERE RR.IdDestino=DR.IdDestino) AND
				IdDestino not in (select IdDestino from DestinosRadioSector
														where Idsistema=id_sistema AND
																	IdSector=id_usuario AND
																	(((PosHMI-1) div frecPorPagina) = pagina));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosTelefoniaSectorizados`
--

DROP PROCEDURE IF EXISTS `DestinosTelefoniaSectorizados`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `DestinosTelefoniaSectorizados`(in id_sistema char(32), in id_sector char(32), in id_sectorizacion char(32), in internos BOOL)
BEGIN
	IF internos=true THEN
		SELECT * FROM internos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdSector=id_sector;
	ELSE
			SELECT * FROM externos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdSector=id_sector;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosTelefoniaSectorizadosParaXML`
--

DROP PROCEDURE IF EXISTS `DestinosTelefoniaSectorizadosParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosTelefoniaSectorizadosParaXML`(in id_sistema char(32), in id_sectorizacion char(32), in lc BOOL)
BEGIN
	IF (id_sectorizacion IS NOT NULL) THEN
		IF lc=true THEN
			(SELECT * FROM internos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdPrefijo=0)
			UNION
			(SELECT * FROM externos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdPrefijo=1);
		ELSE
			(SELECT * FROM internos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdPrefijo=2)
			UNION
			(SELECT * FROM externos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdPrefijo<>1);
		END IF;			
	ELSE	
		IF lc=true THEN
			(SELECT * FROM DestinosInternosSector
					WHERE IdSistema=id_sistema AND
								IdPrefijo=0)
			UNION
			(SELECT d.IdSistema,d.IdDestino,d.TipoDestino,d.IdNucleo,d.IdSector,d.IdPrefijo,d.PosHMI,d.Prioridad,d.OrigenR2,d.PrioridadSIP,d.TipoAcceso,d.Literal
					FROM DestinosExternosSector d
					WHERE IdSistema=id_sistema AND
								IdPrefijo=1);
		ELSE
			(SELECT * FROM DestinosInternosSector
					WHERE IdSistema=id_sistema AND
								IdPrefijo=2)
			UNION
			(SELECT d.IdSistema,d.IdDestino,d.TipoDestino,d.IdNucleo,d.IdSector,d.IdPrefijo,d.PosHMI,d.Prioridad,d.OrigenR2,d.PrioridadSIP,d.TipoAcceso,d.Literal
					FROM DestinosExternosSector d
					WHERE IdSistema=id_sistema AND
								IdPrefijo<>1);
		END IF;			
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosTelefoniaSinAsignarAlSector`
--

DROP PROCEDURE IF EXISTS `DestinosTelefoniaSinAsignarAlSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `DestinosTelefoniaSinAsignarAlSector`(in id_sistema char(32), in id_usuario char(32))
BEGIN
		(select a.* from destinosTelefonia a, destinosInternos b
			where a.IdSistema=id_sistema AND
				a.IdPrefijo=2 AND 
				a.IdSistema=b.IdSistema AND
				a.IdPrefijo=b.IdPrefijo AND
				a.IdDestino=b.IdDestino AND
				b.IdSector<>id_usuario AND
				b.IdDestino in (SELECT s.IdSector FROM Sectores s
														WHERE s.IdSistema=id_sistema AND
																	s.SectorSimple) AND
				b.IdDestino not in (SELECT IdDestino FROM DestinosInternosSector
														WHERE IdSistema=id_sistema AND
																	IdSector=id_usuario AND
																	IdPrefijo=2)
				ORDER BY a.IdDestino)
UNION
(select a.* from destinosTelefonia a
	where a.IdSistema=id_sistema AND
				a.TipoDestino=1 AND 
				a.IdPrefijo<>1 AND
				a.IdDestino not in (SELECT IdDestino FROM DestinosExternosSector
														WHERE IdSistema=id_sistema AND
																	IdSector=id_usuario)
				ORDER BY a.IdDestino);

END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `EliminaActiva`
--

DROP PROCEDURE IF EXISTS `EliminaActiva`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `EliminaActiva`(in id_sistema char(32))
BEGIN
	DECLARE id char(32);
	
	SELECT FechaActivacion INTO id FROM sectorizaciones WHERE IdSistema=id_sistema AND Activa=true;
	
	DELETE FROM Sectorizaciones
		WHERE NOT Activa AND
					IdSistema=id_sistema AND
					FechaActivacion=id;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ExisteIP`
--

DROP PROCEDURE IF EXISTS `ExisteIP`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ExisteIP`(in id_sistema varchar(32), in ip varchar(15), in id_hw varchar(32))
BEGIN
	select 	(select count(*) from Top where IdSistema=id_sistema AND IdTOP!=id_hw AND (IpRed1=ip OR IpRed2=ip)) +
					(select count(*) from Tifx where IdSistema=id_sistema AND IdTifx!=id_hw AND (IpRed1=ip OR IpRed2=ip)) +
					(select count(*) from EquiposEu where IdSistema=id_sistema AND idEquipos!=id_hw AND (IpRed1=ip OR IpRed2=ip));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `FinalizaTransaccion`
--

DROP PROCEDURE IF EXISTS `FinalizaTransaccion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `FinalizaTransaccion`()
BEGIN
		start transaction;
		
		insert ignore into cd40.sistema select * from cd40_trans.sistema;
		insert ignore into cd40.abonados select * from cd40_trans.abonados;
		
		commit;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GeneraRegistroBackup`
--

DROP PROCEDURE IF EXISTS `GeneraRegistroBackup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GeneraRegistroBackup`(in id_sistema char(32), in tipo_backup int, in fDesde datetime, in fHasta datetime, in recurso text)
BEGIN
	INSERT INTO registrobackup (
	   IdSistema
	  ,TipoBackup
	  ,FechaInicio
	  ,FechaFin
	  ,RecursoDestino
	) VALUES (
	   id_sistema 															
	  ,tipo_backup 															
	  ,DATE_FORMAT(fDesde,'%Y/%m/%d %H:%i:%s') 	
	  ,DATE_FORMAT(fHasta,'%Y/%m/%d %H:%i:%s') 	
	  ,recurso 																	
	);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetEventosRadio`
--

DROP PROCEDURE IF EXISTS `GetEventosRadio`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetEventosRadio`(in id_sistema varchar(32), in desde date, in hasta date)
BEGIN
	SELECT * FROM EventosRadio	
	WHERE IdSistema=id_sistema AND
				DATE_FORMAT(FechaHora,"%Y/%m/%d") >= desde AND
				DATE_FORMAT(FechaHora,"%Y/%m/%d") < hasta
	ORDER BY FechaHora;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetEventosTelefonia`
--

DROP PROCEDURE IF EXISTS `GetEventosTelefonia`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetEventosTelefonia`(in id_sistema varchar(32), in desde date, in hasta date)
BEGIN
	SELECT * FROM EventosTelefonia	
	WHERE IdSistema=id_sistema AND
				DATE_FORMAT(FechaHora,"%Y/%m/%d") >= desde AND
				DATE_FORMAT(FechaHora,"%Y/%m/%d") < hasta
	ORDER BY FechaHora;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetFunciones`
--

DROP PROCEDURE IF EXISTS `GetFunciones`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetFunciones`(in id_sistema varchar(32), in _tipo int)
BEGIN
	SELECT * FROM funciones
	WHERE IdSistema=id_sistema AND
				Tipo=_tipo;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

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

--
-- Definition of procedure `GetUnHistorico`
--

DROP PROCEDURE IF EXISTS `GetUnHistorico`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUnHistorico`(in id_sistema varchar(32),in lista_incidencias text,in tipo_hw int, in id_hw varchar(32), in desde date, in hasta date)
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
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
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
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
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
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
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
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
					END WHILE;
			END IF;
		END IF;
	ELSE
		IF (id_hw="") THEN
			IF (tipo_hw=-1) THEN																		
				INSERT INTO t 
					SELECT hi.*,i.Descripcion FROM HistoricoIncidencias hi, incidencias i WHERE hi.IdSistema=id_sistema AND 
								DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
								DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
								i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
			ELSE
				INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias  hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
									hi.TipoHw=tipo_hw AND 
									DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
									DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
								i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
			END IF;
		ELSE
			IF (tipo_hw=-1) THEN																		
				INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
							hi.IdHw=id_hw AND 
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
			ELSE
				INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
							hi.TipoHw=tipo_hw AND
							hi.IdHw=id_hw AND 
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
			END IF;
		END IF;
	END IF;

	SELECT * FROM t				
		ORDER BY -FechaHora LIMIT 1;
		
	DROP TABLE t;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `IdManttoLibre`
--

DROP PROCEDURE IF EXISTS `IdManttoLibre`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `IdManttoLibre`(in id_Sistema varchar(32), out id_Sacta int)
BEGIN
	DECLARE encontrado bool default false;
	DECLARE nSacta int default 10001;
	DECLARE id int;
	
	set id_Sacta=0;
	
	WHILE ((not encontrado) AND (nSacta < 20000)) DO
		SELECT COUNT(*) INTO id FROM sectores WHERE IdSistema=id_sistema AND NumSacta=nSacta;
		IF (id=0) THEN
			SET encontrado=true;
			SET id_Sacta=nSacta;
		ELSE
			SET nSacta=nSacta+1;
		END IF;
	END WHILE;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `IniciaTransaccion`
--

DROP PROCEDURE IF EXISTS `IniciaTransaccion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `IniciaTransaccion`()
BEGIN
		create table if not exists cd40_trans.sistema like cd40.sistema;
		create table if not exists cd40_trans.abonados like cd40.abonados;
		
		start transaction;
		
		delete from cd40_trans.sistema;
		delete from cd40_trans.abonados;
		
		insert into cd40_trans.sistema select * from cd40.sistema;
		insert into cd40_trans.abonados select * from cd40.abonados;
		
		commit;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ListaRecursosDestino`
--

DROP PROCEDURE IF EXISTS `ListaRecursosDestino`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ListaRecursosDestino`(in id_sistema varchar(32), in id_destino varchar(32), in id_prefijo int)
BEGIN
	IF (id_prefijo=1) THEN
		SELECT rLC.*,r.Interface FROM recursoslcen rLC, recursos r
		WHERE rLC.IdSistema=id_sistema AND
					rLC.IdDestino=id_destino AND
					rLC.IdPrefijo=id_prefijo AND
					r.IdSistema=rLC.IdSistema AND
					r.IdRecurso=rLC.IdRecurso AND
					r.TipoRecurso=rLC.TipoRecurso;
	ELSE
		SELECT rTf.*,r.Interface FROM recursostf rTf, recursos r
		WHERE rTf.IdSistema=id_sistema AND
					rTf.IdDestino=id_destino AND
					rTf.IdPrefijo=id_prefijo AND
					r.IdSistema=rTf.IdSistema AND
					r.IdRecurso=rTf.IdRecurso AND
					r.TipoRecurso=rTf.TipoRecurso;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ListaRutas`
--

DROP PROCEDURE IF EXISTS `ListaRutas`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ListaRutas`(in id_Sistema char(32), in id_Central char(32))
BEGIN
	select IdRuta as IdRuta,Tipo as Tipo from Rutas 
		where IdSistema=id_Sistema AND
				Central=id_Central
		ORDER BY Orden;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `LoginTop`
--

DROP PROCEDURE IF EXISTS `LoginTop`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `LoginTop`(in id_Sistema char(32),in id_Hw char(32), out id_Usuario char(32), out modo_arranque char(1))
BEGIN
	SELECT b.idsector,c.ModoArranque  INTO id_Usuario, modo_arranque
	FROM sectorizaciones a, SectoresSectorizacion b, top c
	WHERE a.idSistema=id_Sistema AND
				c.IdSistema=id_Sistema AND
				a.Activa=true AND
				b.idSistema=id_Sistema AND
				a.idSectorizacion=b.idSectorizacion AND
				b.idTOP=id_Hw AND
				b.IdTop=c.IdTOP;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NivelesIntrusion`
--

DROP PROCEDURE IF EXISTS `NivelesIntrusion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `NivelesIntrusion`(in id_sistema char(32), in id_usuario char(32))
BEGIN
	SELECT n.* FROM Niveles n, SectoresSector ss
	WHERE ss.IdSistema=id_sistema AND
				ss.IdSector=id_usuario AND
				n.IdSistema=id_sistema AND
				n.IdSector=ss.IdSectorOriginal AND
				ss.EsDominante;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NivelesIntrusionParaXML`
--

DROP PROCEDURE IF EXISTS `NivelesIntrusionParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `NivelesIntrusionParaXML`(in id_sistema varchar(32), in id_sectorizacion varchar(32))
BEGIN
	IF (id_sectorizacion IS NOT NULL) THEN
		SELECT c.IdSector, n.* FROM Niveles n, SectoresSector ss, sectoressectorizacion c
			WHERE ss.IdSistema=id_sistema AND
				c.IdSectorizacion=id_sectorizacion AND
				c.IdSistema=ss.IdSistema AND
				n.IdSistema=ss.IdSistema AND
				n.IdSector=ss.IdSectorOriginal AND
				c.IdSector=ss.IdSector AND
				ss.EsDominante;
	ELSE
		SELECT n.* FROM Niveles n, Sectores s
			WHERE	s.IdSistema=id_sistema AND
						s.SectorSimple AND
						n.IdSistema=s.IdSistema AND
						n.IdSector=s.IdSector AND
						n.IdNucleo=s.IdNucleo;
	END IF;	
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NumerosAbonadoExternos`
--

DROP PROCEDURE IF EXISTS `NumerosAbonadoExternos`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `NumerosAbonadoExternos`(in idSistema char(32), in idUsuario char(32))
BEGIN
	SELECT a.*
		FROM UsuariosAbonados a, Sectorizaciones b
		WHERE b.idSistema=idSistema AND
					b.Activa=true AND
					a.idSectorizacion=b.idSectorizacion AND
					a.idSistema=b.idSistema AND
					a.idSector=idUsuario AND
					a.idPrefijos<>1;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NumerosAbonadoInternos`
--

DROP PROCEDURE IF EXISTS `NumerosAbonadoInternos`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `NumerosAbonadoInternos`(in id_Sistema char(32),in id_Usuario char (32))
BEGIN
	SELECT *
		FROM UsuariosAbonados
		WHERE IdSistema=id_Sistema AND
					IdSector=id_Usuario;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NumerosAbonadosSectorAgrupado`
--

DROP PROCEDURE IF EXISTS `NumerosAbonadosSectorAgrupado`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `NumerosAbonadosSectorAgrupado`(in id_sistema char(32), in id_sector_agrupado char(32))
BEGIN
	SELECT UA.* FROM UsuariosAbonados ua, SectoresAgrupacion ss
	WHERE ss.IdSistema=id_sistema AND
				ua.IdSistema=ss.IdSistema AND
				ss.IdAgrupacion=id_sector_agrupado AND
				ss.IdSector=ua.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NumerosAbonadosSectorNoAgrupado`
--

DROP PROCEDURE IF EXISTS `NumerosAbonadosSectorNoAgrupado`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `NumerosAbonadosSectorNoAgrupado`(in id_sistema char(32), in id_nucleo char(32), in id_sector_agrupado char(32))
BEGIN
	SELECT UA.* FROM UsuariosAbonados ua, SectoresSector ss
	WHERE ss.IdSistema=id_sistema AND	ss.IdNucleo=id_nucleo AND
				ua.IdSistema=ss.IdSistema AND ua.IdNucleo=ss.IdNucleo AND
				ss.IdSector=id_sector_agrupado AND
				ss.IdSectorOriginal=ua.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ParametrosSectorAgrupado`
--

DROP PROCEDURE IF EXISTS `ParametrosSectorAgrupado`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `ParametrosSectorAgrupado`(in id_sistema char(32), in id_sector_agrupado char(32))
BEGIN
		SELECT ps.* FROM SectoresAgrupacion ss, ParametrosSector ps
		WHERE ss.IdSistema=id_sistema AND
					ps.IdSistema=ss.IdSistema AND
					ss.IdAgrupacion=id_sector_agrupado AND
					ss.IdSector=ps.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ParametrosSectores`
--

DROP PROCEDURE IF EXISTS `ParametrosSectores`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ParametrosSectores`(in id_sistema varchar(32), in lista_nucleos varchar(255), in lista_sectores varchar(255))
BEGIN
	declare i int;
	declare pos int;
	declare nucleo text;
	declare j int;
	declare posj int;
	declare sector text;
	set i=LOCATE(",",lista_nucleos);
	set pos=1;

	create temporary table if not exists ps SELECT * FROM ParametrosSector where false;

	WHILE i>0 DO
		set j=LOCATE(",",lista_sectores);
		set posj=1;

		set nucleo=substring(lista_nucleos,pos,i-pos);
		set pos=i+1;
		set i=LOCATE(",",lista_nucleos,pos);

		WHILE j>0 DO
			set sector=substring(lista_sectores,posj,j-posj);
			set posj=j+1;
			set j=LOCATE(",",lista_sectores,posj);

			INSERT INTO ps
			SELECT * FROM ParametrosSector
				WHERE IdSistema=id_sistema AND
							IdNucleo=nucleo AND
							IdSector=sector;
					
			END WHILE;
		END WHILE;
		
		SELECT * FROM ps;
		DROP TABLE ps;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ParametrosSectoresParaXML`
--

DROP PROCEDURE IF EXISTS `ParametrosSectoresParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ParametrosSectoresParaXML`(in id_sistema varchar(32), in id_sectorizacion varchar(32))
BEGIN
	IF (id_sectorizacion IS NOT NULL) THEN
		SELECT c.IdSector, n.* FROM ParametrosSector n, SectoresSector ss, sectoressectorizacion c
		WHERE ss.IdSistema=id_sistema AND
				c.IdSectorizacion=id_sectorizacion AND
				c.IdSistema=ss.IdSistema AND
				n.IdSistema=ss.IdSistema AND
				n.IdSector=ss.IdSectorOriginal AND
				c.IdSector=ss.IdSector AND
				ss.EsDominante;
	ELSE
		SELECT n.* FROM ParametrosSector n, Sectores s
			WHERE	s.IdSistema=id_sistema AND
						s.SectorSimple AND
						n.IdSistema=s.IdSistema AND
						n.IdSector=s.IdSector AND
						n.IdNucleo=s.IdNucleo;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ParametrosSectorNoAgrupado`
--

DROP PROCEDURE IF EXISTS `ParametrosSectorNoAgrupado`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `ParametrosSectorNoAgrupado`(in id_sistema char(32), in id_sector_agrupado char(32))
BEGIN
	SELECT ps.* FROM SectoresSector ss, ParametrosSector ps
		WHERE ss.IdSistema=id_sistema AND
					ps.IdSistema=ss.IdSistema AND
					ss.IdSector=id_sector_agrupado AND
					ss.IdSectorOriginal=ps.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PermisosRedes`
--

DROP PROCEDURE IF EXISTS `PermisosRedes`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `PermisosRedes`(in idSistema char(32), in idUsuario char(32))
BEGIN
		SELECT a.*
		FROM PermisosRedes a, Sectorizaciones b
		WHERE b.idSistema=idSistema AND
					b.Activa=true AND
					a.idSectorizacion=b.idSectorizacion AND
					a.idSistema=b.idSistema AND
					a.idSector=idUsuario;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PlanDireccionamientoIP`
--

DROP PROCEDURE IF EXISTS `PlanDireccionamientoIP`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `PlanDireccionamientoIP`(in id_Sistema char(32))
BEGIN
	(SELECT IdTop as Id,0 as TipoEH, IpRed1, IpRed2
		FROM TOP
		WHERE IdSistema=id_Sistema)
	union
	(SELECT IdTifX as Id,1 as TipoEH , IpRed1, IpRed2
		FROM TifX
		WHERE IdSistema=id_Sistema)
	union
	(SELECT IdEquipos as Id,2 as TipoEH , IpRed1, IpRed2	
		FROM EquiposEU
		WHERE IdSistema=id_Sistema);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PlanDireccionamientoSIP`
--

DROP PROCEDURE IF EXISTS `PlanDireccionamientoSIP`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `PlanDireccionamientoSIP`(in id_Sistema char(32))
BEGIN

	SELECT a.IdPrefijo,a.IdAbonado,ss.IdSectorOriginal as IdSector
	FROM UsuariosAbonados a, SectoresSector ss
	WHERE ss.IdSector in (SELECT c.IdSector FROM Sectorizaciones b, SectoresSectorizacion c
													WHERE b.IdSistema=id_sistema AND
																b.Activa=true AND
																b.IdSectorizacion=c.IdSectorizacion) AND
				ss.EsDominante AND
				a.IdSector=ss.IdSector
	ORDER BY ss.IdSectorOriginal,a.IdPrefijo,a.IdAbonado;


END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PosicionOcupadaEnSector`
--

DROP PROCEDURE IF EXISTS `PosicionOcupadaEnSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `PosicionOcupadaEnSector`(in id_sistema char(32), in id_nucleo char(32), in id_sector char(32), in tipo_acceso char(32), in posicion int, out existe boolean)
BEGIN
	DECLARE existeInterno,existeExterno int;
	
	SELECT count(*) INTO existeInterno
		FROM DestinosInternosSector dis
		WHERE dis.IdSistema=id_sistema AND
					dis.IdNucleo=id_nucleo AND
					dis.IdSector=id_sector AND
					dis.TipoAcceso=tipo_acceso AND
					dis.PosHMI=posicion;
				
	SELECT count(*) INTO existeExterno
		FROM DestinosExternosSector dis
		WHERE dis.IdSistema=id_sistema AND
					dis.IdNucleo=id_nucleo AND
					dis.IdSector=id_sector AND
					dis.TipoAcceso=tipo_acceso AND
					dis.PosHMI=posicion;				
					
	SET existe=((existeInterno>0) OR (existeExterno>0));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PrefijosSinAsignarARedes`
--

DROP PROCEDURE IF EXISTS `PrefijosSinAsignarARedes`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `PrefijosSinAsignarARedes`(in id_Sistema char(32))
BEGIN
	select IdPrefijo from Prefijos
	where IdSistema=id_Sistema AND
				IdPrefijo > 3 AND IdPrefijo < 10 AND
				IdPrefijo not in (select IdPrefijo from Redes where IdSistema=id_Sistema);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PrimeraPosicionLibre`
--

DROP PROCEDURE IF EXISTS `PrimeraPosicionLibre`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `PrimeraPosicionLibre`(in id_sistema char(32), in id_nucleo char(32), in id_sector char(32), in tipo_acceso char(2), out hueco int)
BEGIN
	DECLARE i int;
	DECLARE huecoInterno, huecoExterno int;
	DECLARE encontrado boolean;
	SET i=0;
	SET encontrado=false;
	SET huecoInterno=0;
	SET huecoExterno=0;
	WHILE (NOT encontrado) AND (i<56) DO
		SET i=i+1;
		SELECT COUNT(*) INTO huecoInterno
			FROM DestinosInternosSector d
			WHERE d.IdSistema=id_sistema AND
						d.IdNucleo=id_nucleo AND
						d.IdSector=id_sector AND
						d.TipoAcceso=tipo_acceso AND
						d.PosHMI=i;
						
		SELECT COUNT(*) INTO huecoExterno
			FROM DestinosExternosSector d
			WHERE d.IdSistema=id_sistema AND
						d.IdNucleo=id_nucleo AND
						d.IdSector=id_sector AND
						d.TipoAcceso=tipo_acceso AND
						d.PosHMI=i;
						
		SET encontrado=((huecoInterno=0) AND (huecoExterno=0));
	END WHILE;

	SET hueco=i;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RangosConIdRed`
--

DROP PROCEDURE IF EXISTS `RangosConIdRed`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RangosConIdRed`(in id_sistema varchar(32), in id_central varchar(32))
BEGIN
	IF (id_central is not null) THEN
		SELECT r.*, red.IdRed
		FROM 	Rangos r, Redes red
		WHERE	r.IdSistema=id_sistema AND
					r.Central=id_central AND
					red.IdSistema=r.IdSistema AND
					r.IdPrefijo=red.IdPrefijo;
	ELSE
		SELECT r.*, red.IdRed
		FROM 	Rangos r, Redes red
		WHERE	r.IdSistema=id_sistema AND
					red.IdSistema=r.IdSistema AND
					r.IdPrefijo=red.IdPrefijo;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosLCENSinAsignarAEnlaces`
--

DROP PROCEDURE IF EXISTS `RecursosLCENSinAsignarAEnlaces`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `RecursosLCENSinAsignarAEnlaces`(in id_sistema char(32))
BEGIN
		SELECT * FROM RecursosLCEN
		WHERE IdSistema=id_sistema AND
					IdDestino is null;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosLcParaInformeXML`
--

DROP PROCEDURE IF EXISTS `RecursosLcParaInformeXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RecursosLcParaInformeXML`(in id_sistema varchar(32))
BEGIN
	SELECT r.*, rt.*,pr.*
	FROM recursos r, recursoslcen rt, parametrosRecurso pr
	WHERE r.IdSistema=id_sistema AND
				rt.IdSistema = r.IdSistema AND rt.IdRecurso = r.IdRecurso AND rt.TipoRecurso = r.TipoRecurso AND
				pr.IdSistema = r.IdSistema AND pr.IdRecurso = r.IdRecurso AND pr.TipoRecurso = r.TipoRecurso;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosPorEmplazamientoParaXML`
--

DROP PROCEDURE IF EXISTS `RecursosPorEmplazamientoParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RecursosPorEmplazamientoParaXML`(in id_sistema varchar(32))
BEGIN
		(SELECT t.IdEmplazamiento,rTf.IdRecurso,r.IdTifx, r.SlotPasarela, r.NumDispositivoSlot
			FROM emplazamientos t, recursosradio rTf, recursos r
			WHERE t.IdSistema=id_sistema AND
				rTf.IdSistema=t.IdSistema AND
				rTf.IdEmplazamiento=t.IdEmplazamiento AND
				r.IdSistema=rTf.IdSistema AND
				r.IdRecurso=rTf.IdRecurso)
	UNION
		(SELECT t.IdEmplazamiento,null,null,null,null
			FROM emplazamientos t
			WHERE t.IdSistema=id_sistema AND
					t.IdEmplazamiento NOT IN (SELECT DISTINCT(IdEmplazamiento) FROM recursosradio WHERE IdSistema=id_sistema AND IdEmplazamiento IS NOT NULL));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosPorTroncalParaXML`
--

DROP PROCEDURE IF EXISTS `RecursosPorTroncalParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RecursosPorTroncalParaXML`(in id_sistema varchar(32))
BEGIN
		(SELECT t.IdTroncal,t.NumTest,rTf.IdRecurso,r.IdTifx, r.SlotPasarela, r.NumDispositivoSlot
			FROM troncales t, recursostf rTf, recursos r
			WHERE t.IdSistema=id_sistema AND
				rTf.IdSistema=t.IdSistema AND
				rTf.IdTroncal=t.IdTroncal AND
				r.IdSistema=rTf.IdSistema AND
				r.IdRecurso=rTf.IdRecurso)
	UNION
		(SELECT t.IdTroncal,t.NumTest,null,null,null,null
			FROM troncales t
			WHERE t.IdSistema=id_sistema AND
					t.IdTroncal NOT IN (SELECT DISTINCT(IdTroncal) FROM recursostf WHERE IdSistema=id_sistema AND IdTroncal IS NOT NULL));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosRadioSinAsignarAEnlaces`
--

DROP PROCEDURE IF EXISTS `RecursosRadioSinAsignarAEnlaces`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `RecursosRadioSinAsignarAEnlaces`(in id_sistema char(32))
BEGIN
	SELECT * FROM RecursosRadio
		WHERE IdSistema=id_sistema AND
					IdDestino is null;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosRdParaInformeXML`
--

DROP PROCEDURE IF EXISTS `RecursosRdParaInformeXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RecursosRdParaInformeXML`(in id_sistema varchar(32))
BEGIN
	SELECT r.*, rt.*,pr.*
	FROM recursos r, recursosradio rt, parametrosRecurso pr
	WHERE r.IdSistema=id_sistema AND
				rt.IdSistema = r.IdSistema AND rt.IdRecurso = r.IdRecurso AND rt.TipoRecurso = r.TipoRecurso AND
				pr.IdSistema = r.IdSistema AND pr.IdRecurso = r.IdRecurso AND pr.TipoRecurso = r.TipoRecurso;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosSinAsignar`
--

DROP PROCEDURE IF EXISTS `RecursosSinAsignar`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `RecursosSinAsignar`(in id char(32),in id_Sistema char(32))
BEGIN
	select * from recursos
	where IdSistema=id_Sistema && @id is null;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosTfParaInformeXML`
--

DROP PROCEDURE IF EXISTS `RecursosTfParaInformeXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RecursosTfParaInformeXML`(in id_sistema varchar(32))
BEGIN
	SELECT r.*, pr.*, rt.IdPrefijo,rt.TipoDestino,rt.IdDestino,rt.IdTroncal,rt.IdRed,rt.Lado,rt.Modo
	FROM recursos r, recursostf rt, parametrosRecurso pr
	WHERE r.IdSistema=id_sistema AND
				rt.IdSistema = r.IdSistema AND rt.IdRecurso = r.IdRecurso AND rt.TipoRecurso = r.TipoRecurso AND
				pr.IdSistema = r.IdSistema AND pr.IdRecurso = r.IdRecurso AND pr.TipoRecurso = r.TipoRecurso;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosTFSinAsignarAEnlaces`
--

DROP PROCEDURE IF EXISTS `RecursosTFSinAsignarAEnlaces`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `RecursosTFSinAsignarAEnlaces`(in id_sistema char(32))
BEGIN
		SELECT rtf.* FROM RecursosTF rtf, recursos r
		WHERE rtf.IdSistema=id_sistema AND
					IdDestino is null AND
					r.IdSistema=id_sistema AND
					r.IdRecurso=rtf.IdRecurso AND
					r.TipoRecurso=rtf.TipoRecurso AND
					r.Interface in (2,3);			
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RedesUsuariosAbonados`
--

DROP PROCEDURE IF EXISTS `RedesUsuariosAbonados`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `RedesUsuariosAbonados`(in id_Sistema char(32), in id_Nucleo char(32), in id_Sector char(32))
BEGIN
	SELECT red.IdRed, red.IdPrefijo, abonado.IdAbonado 
	FROM Redes red, UsuariosAbonados abonado
	WHERE abonado.IdSistema=id_Sistema AND
				abonado.IdNucleo=id_Nucleo AND
				abonado.IdSector=id_Sector AND
				red.IdSistema=abonado.IdSistema AND
				red.IdPrefijo=abonado.IdPrefijo;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RedesUsuariosAbonadosParaXML`
--

DROP PROCEDURE IF EXISTS `RedesUsuariosAbonadosParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RedesUsuariosAbonadosParaXML`(in id_sistema varchar(32), in id_sectorizacion varchar(32))
BEGIN
			SELECT abonado.IdNucleo, c.IdSector,c.IdSectorOriginal, red.IdRed, red.IdPrefijo, abonado.IdAbonado 
			FROM Redes red, UsuariosAbonados abonado, sectoressector c
			WHERE abonado.IdSistema=id_sistema AND
						abonado.IdSistema=c.IdSistema AND
						abonado.IdNucleo=c.IdNucleo AND
						abonado.IdSector=c.IdSectorOriginal AND
						red.IdSistema=abonado.IdSistema AND
						red.IdPrefijo=abonado.IdPrefijo;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectorConNumeroAbonado`
--

DROP PROCEDURE IF EXISTS `SectorConNumeroAbonado`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `SectorConNumeroAbonado`(in id_sistema char(32), in id_nucleo char(32), in id_prefijo int, in id_abonado char(32))
BEGIN
	SELECT s.IdSector FROM UsuariosAbonados ua, Sectores s
		WHERE ua.IdSistema=id_sistema AND
					ua.IdNucleo=id_nucleo AND
					ua.IdPrefijo=id_prefijo AND
					ua.IdAbonado=id_abonado AND
					s.IdSistema=ua.IdSistema AND
					s.IdNucleo=ua.IdNucleo AND
					s.SectorSimple AND
					ua.IdSector=s.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresCompuestosPorSectorOriginal`
--

DROP PROCEDURE IF EXISTS `SectoresCompuestosPorSectorOriginal`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `SectoresCompuestosPorSectorOriginal`(in id_sistema char(32), in id_sector_original char(32))
BEGIN
	SELECT s.IdSector from SectoresSector ss, Sectores s
	where ss.IdSistema=id_sistema AND
				ss.IdSectorOriginal=id_sector_original AND
				ss.IdSectorOriginal!=ss.IdSector AND
				s.IdSector=ss.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresFueraDeAgrupacion`
--

DROP PROCEDURE IF EXISTS `SectoresFueraDeAgrupacion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `SectoresFueraDeAgrupacion`(in id_sistema varchar(32), in id_agrupacion varchar(32))
BEGIN
	IF id_agrupacion IS NOT NULL THEN
		SELECT s.IdSector FROM Sectores s
			WHERE s.IdSistema=id_sistema AND
						s.SectorSimple=true AND
						s.IdSector NOT IN (SELECT IdSector FROM	SectoresAgrupacion	
																	WHERE IdSistema=id_sistema AND
																				IdAgrupacion=id_agrupacion);
	ELSE
		SELECT s.IdSector FROM Sectores s
			WHERE s.IdSistema=id_sistema AND
						s.SectorSimple=true;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresManttoEnActiva`
--

DROP PROCEDURE IF EXISTS `SectoresManttoEnActiva`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `SectoresManttoEnActiva`(in id_sistema char(32))
BEGIN
	SELECT  s.NumSacta, t.PosicionSacta
	FROM Sectores s, sectorizaciones sct, sectoressectorizacion ss, top t
	WHERE sct.IdSistema=id_sistema AND
				sct.Activa AND
				ss.IdSistema = sct.IdSistema AND ss.IdSectorizacion = sct.IdSectorizacion AND
				ss.IdSistema = s.IdSistema AND ss.IdNucleo = s.IdNucleo AND ss.IdSector = s.IdSector AND s.SectorSimple AND s.Tipo='M' AND
				ss.IdTOP=t.IdTOP;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresManttoEnTop`
--

DROP PROCEDURE IF EXISTS `SectoresManttoEnTop`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SectoresManttoEnTop`(in id_sistema varchar(32),in id_sectorizacion varchar(32), in id_top varchar(32))
BEGIN
	select s.* from sectores s, sectoressectorizacion ss, sectoressector sct
	where ss.IdSistema=id_sistema AND
				ss.IdSistema=sct.IdSistema AND
				s.IdSistema=sct.IdSistema AND
				ss.IdSectorizacion=id_sectorizacion AND
				ss.IdTOP=id_top AND
				ss.IdSector=sct.IdSector AND
				sct.IdSectorOriginal=s.IdSector AND
				s.SectorSimple AND
				s.Tipo='M';
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresNumSactaSorted`
--

DROP PROCEDURE IF EXISTS `SectoresNumSactaSorted`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SectoresNumSactaSorted`(in id_sistema char(32), in id_nucleo char(32), in lista_sectores text)
BEGIN
	DECLARE lista text;
	SET @lista=lista_sectores;
	SELECT IdSector from Sectores
	where IdSistema=id_sistema AND IdNucleo=id_nucleo AND
		IdSector in (@lista)
	ORDER BY NumSacta;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresReales`
--

DROP PROCEDURE IF EXISTS `SectoresReales`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SectoresReales`(in id_sistema char(32), in id_sectorizacion char(32), out cuantos int)
BEGIN
	SELECT count(*) into cuantos FROM Sectores 
	WHERE IdSistema=id_sistema AND
				SectorSimple AND
				Tipo='R' AND
				IdNucleo IN (SELECT DISTINCT IdNucleo FROM SectoresSectorizacion WHERE IdSistema=id_sistema AND IdSectorizacion=id_sectorizacion) AND 
				IdSector NOT IN (SELECT IdSectorOriginal FROM SectoresSector ss,SectoresSectorizacion sz
													WHERE sz.IdSectorizacion=id_sectorizacion AND
																sz.IdSistema=id_sistema AND
																ss.IdSistema=sz.IdSistema AND
																ss.IdSector=sz.IdSector);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresSinAsignarASectorizacion`
--

DROP PROCEDURE IF EXISTS `SectoresSinAsignarASectorizacion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SectoresSinAsignarASectorizacion`(in id_sistema char(32),in id_sectorizacion char(32), in id_nucleo char(32))
BEGIN
	select a.IdSector, a.Tipo from Sectores a
	where a.IdSistema=id_sistema AND
				a.IdNucleo=id_nucleo AND
				a.SectorSimple=true AND
				a.IdSector not in (select ss.IdSectorOriginal FROM sectoresSectorizacion ssz, sectoresSector ss
																		WHERE ssz.IdSistema=id_sistema AND
																					ssz.IdNucleo=id_nucleo AND
																					ssz.IdSectorizacion=id_sectorizacion AND
																					ss.IdSistema=ssz.IdSistema AND ss.IdNucleo=ssz.IdNucleo AND 
																					ss.IdSector=ssz.IdSector);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SlotsLibresEnTifx`
--

DROP PROCEDURE IF EXISTS `SlotsLibresEnTifx`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `SlotsLibresEnTifx`(in id_sistema char(32), in cuantos int)
BEGIN
	CASE cuantos
		WHEN 8 THEN
				SELECT l.Id FROM (
									select b.idtifx as Id,16-sum(a.2bd) as libres 
																								from (
																											(select idtifx as id,count(*)*2 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=8 
																																group by idtifx) UNION
																										 	(select idtifx as id,count(*)*8 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=7 
																																group by idtifx) UNION
																											(select idtifx as id,count(*)*16 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=9
																																group by idtifx) UNION
																											(select idtifx as id,count(*) as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface in (0,1,2,3,4,5,6)
																																group by idtifx) UNION
																											(select idtifx as id,0 as 2BD from TIFX 
																													where IdSistema=id_sistema AND
																																IdTifx NOT IN (SELECT DISTINCT(IdTifx) FROM Recursos WHERE IdSistema=id_sistema))
																											) a, tifx b
													where b.IdSistema=id_sistema AND
							  								a.id=b.IdTIFX
													group by a.id
								) l
				WHERE l.libres>=2;
			WHEN 7 THEN
				SELECT l.Id FROM (
									select b.idtifx as Id,16-sum(a.2bd) as libres 
																								from (
																											(select idtifx as id,((count(*)DIV 2)*4) + ((count(*)%2)*4)as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=8 
																																group by idtifx) UNION
																										 	(select idtifx as id,count(*)*8 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=7 
																																group by idtifx) UNION
																											(select idtifx as id,count(*)*16 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=9
																																group by idtifx) UNION
																											(select idtifx as id,count(*) as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface in (0,1,2,3,4,5,6)
																																group by idtifx) UNION
																											(select idtifx as id,0 as 2BD from TIFX 
																													where IdSistema=id_sistema AND
																																IdTifx NOT IN (SELECT DISTINCT(IdTifx) FROM Recursos WHERE IdSistema=id_sistema))
																											) a, tifx b
													where b.IdSistema=id_sistema AND
							  								a.id=b.IdTIFX
													group by a.id
								) l
				WHERE l.libres>=8;
			WHEN 9 THEN
				SELECT l.Id FROM (
									select b.idtifx as Id,16-sum(a.2bd) as libres 
																								from (
																											(select idtifx as id,((count(*)DIV 2)*4) + ((count(*)%2)*4)as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=8 
																																group by idtifx) UNION
																										 	(select idtifx as id,count(*)*8 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=7 
																																group by idtifx) UNION
																											(select idtifx as id,count(*)*16 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=9
																																group by idtifx) UNION
																											(select idtifx as id,count(*) as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface in (0,1,2,3,4,5,6)
																																group by idtifx) UNION
																											(select idtifx as id,0 as 2BD from TIFX 
																													where IdSistema=id_sistema AND
																																IdTifx NOT IN (SELECT DISTINCT(IdTifx) FROM Recursos WHERE IdSistema=id_sistema))
																											) a, tifx b
													where b.IdSistema=id_sistema AND
							  								a.id=b.IdTIFX
													group by a.id
								) l
				WHERE l.libres>=16;
			ELSE
				SELECT l.Id FROM (
									select b.idtifx as Id,16-sum(a.2bd) as libres 
																								from (
																											(select idtifx as id,((count(*)DIV 2)*4) + ((count(*)%2)*4)as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=8 
																																group by idtifx) UNION
																										 	(select idtifx as id,count(*)*8 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=7 
																																group by idtifx) UNION
																											(select idtifx as id,count(*)*16 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=9
																																group by idtifx) UNION
																											(select idtifx as id,count(*) as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface in (0,1,2,3,4,5,6)
																																group by idtifx) UNION
																											(select idtifx as id,0 as 2BD from TIFX 
																													where IdSistema=id_sistema AND
																																IdTifx NOT IN (SELECT DISTINCT(IdTifx) FROM Recursos WHERE IdSistema=id_sistema))
																											) a, tifx b
													where b.IdSistema=id_sistema AND
							  								a.id=b.IdTIFX
													group by a.id
								) l
				WHERE l.libres>=1;
	END CASE;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `TeclasSector`
--

DROP PROCEDURE IF EXISTS `TeclasSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `TeclasSector`(in id_sistema char(32), in id_sector char(32))
BEGIN
	SELECT ts.* FROM TeclasSector ts, SectoresSector ss
	WHERE ss.IdSistema=id_sistema AND
				ss.IdSector=id_sector AND
				ts.IdSistema=id_sistema AND
				ts.IdSector=ss.IdSectorOriginal AND
				ss.EsDominante;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `TeclasSectorParaXML`
--

DROP PROCEDURE IF EXISTS `TeclasSectorParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `TeclasSectorParaXML`(in id_sistema varchar(32), in id_sectorizacion varchar(32))
BEGIN
	IF (id_sectorizacion IS NOT NULL) THEN
		SELECT c.IdSector,ts.* FROM TeclasSector ts, SectoresSector ss, sectoressectorizacion c
			WHERE ss.IdSistema=id_sistema AND
						c.IdSectorizacion=id_sectorizacion AND
						c.IdSistema=ss.IdSistema AND
						ts.IdSistema=ss.IdSistema AND
						ts.IdNucleo=ss.IdNucleo AND
						ts.IdSector=ss.IdSectorOriginal AND
						c.IdSector=ss.IdSector AND
						ss.EsDominante;
	ELSE
		SELECT ts.* FROM TeclasSector ts, Sectores s
			WHERE	s.IdSistema=id_sistema AND
						s.SectorSimple AND
						ts.IdSistema=s.IdSistema AND
						ts.IdSector=s.IdSector AND
						ts.IdNucleo=s.IdNucleo;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `TroncalesSinAsignarARutas`
--

DROP PROCEDURE IF EXISTS `TroncalesSinAsignarARutas`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `TroncalesSinAsignarARutas`(in id_Sistema char(32),in id_Central char(32), in id_Ruta char(32))
BEGIN
	IF (id_Ruta="") THEN
			
			SELECT * 
				FROM Troncales 
				WHERE IdSistema=id_Sistema AND 
							IdTroncal NOT IN (SELECT IdTroncal 
																	from TroncalesRuta Tr,Rutas r
																	where tr.IdSistema=id_Sistema AND
																				r.IdSistema=Tr.IdSistema AND
																				r.IdRuta=Tr.IdRuta AND
																				(r.Tipo='D' OR tr.Central=id_Central));
	ELSE
			
			SELECT * 
				FROM Troncales 
				WHERE IdSistema=id_Sistema AND 
							IdTroncal NOT IN (SELECT IdTroncal 
																	from TroncalesRuta
																	where IdSistema=id_Sistema AND
																				Central=id_Central);
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `UsuariosImplicadosEnRecurso`
--

DROP PROCEDURE IF EXISTS `UsuariosImplicadosEnRecurso`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `UsuariosImplicadosEnRecurso`(in id_sistema char(32), in id_destino char(32))
BEGIN
	SELECT c.IdSectorOriginal FROM Externos a, SectoresSector c
	WHERE (a.IdSectorizacion = (SELECT IdSectorizacion FROM Sectorizaciones 
																		WHERE IdSistema=id_sistema AND
																					Activa=true)) AND
				a.IdSistema=id_sistema AND
				a.IdDestino=id_destino AND
				c.IdSistema=id_sistema AND
				c.IdSector=a.IdSector AND
				c.EsDominante;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `VersionConfiguracion`
--

DROP PROCEDURE IF EXISTS `VersionConfiguracion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `VersionConfiguracion`(
  IN   `id_sistema`  varchar(32),
  OUT  `versionCfg`  text
)
BEGIN
	SELECT DATE_FORMAT(FechaActivacion,"%d/%m/%Y %H:%i:%s") INTO versionCfg FROM Sectorizaciones 
			WHERE Activa=true AND
						IdSistema=id_sistema;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;
--
-- Create schema cd40_trans
--

CREATE DATABASE IF NOT EXISTS cd40_trans;
USE cd40_trans;

--
-- Definition of procedure `ActualizaNombreSector`
--

DROP PROCEDURE IF EXISTS `ActualizaNombreSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ActualizaNombreSector`(in id_sistema char(32), in id_nucleo char(32), in id_sector char(32), in id_nuevo_nombre_sector char(32))
BEGIN
	UPDATE Sectores 
		SET IdSector=id_nuevo_nombre_sector
		WHERE IdSistema=id_sistema AND
					IdNucleo=id_nucleo AND
					IdSector=id_sector AND
					SectorSimple=FALSE;

		REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('Sectores');
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ActualizaSectoresSectorizacion`
--

DROP PROCEDURE IF EXISTS `ActualizaSectoresSectorizacion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ActualizaSectoresSectorizacion`(in id_sistema varchar(32), in id_nucleo varchar(32), in id_grupo varchar(32), in id_agrupacion varchar(32))
BEGIN
	DECLARE id char(32);
	
	SELECT DATE_FORMAT(FechaActivacion,"%d/%m/%Y %H:%i:%s") INTO id FROM sectorizaciones WHERE IdSistema=id_sistema AND Activa=true;
	
	UPDATE SectoresSectorizacion
		SET IdSector=id_agrupacion
		WHERE IdSistema=id_sistema AND
					IdNucleo=id_nucleo AND
					IdSector=id_grupo AND
					IdSectorizacion<>id;

	REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('SectoresSectorizacion');
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AgrupacionDeLosSectores`
--

DROP PROCEDURE IF EXISTS `AgrupacionDeLosSectores`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `AgrupacionDeLosSectores`(in cuantos int, in lista_usuarios char(55))
BEGIN
	DECLARE lista CHAR(55);
	SET @lista=lista_usuarios;
	SELECT sa.IdAgrupacion,@lista,lista FROM SectoresAgrupacion sa, viewSectoresEnAgrupacion cuenta
	WHERE cuenta.contador=cuantos AND
				sa.IdAgrupacion=cuenta.IdAgrupacion	AND
				sa.IdSector in (@lista);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AsignacionRecursosDeUnaRed`
--

DROP PROCEDURE IF EXISTS `AsignacionRecursosDeUnaRed`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AsignacionRecursosDeUnaRed`(in id_sistema char(32), in id_red char(32))
BEGIN
	IF (id_red is not null) THEN
		SELECT rTf.IdRed, r.IdRecurso, r.IdTifx, r.SlotPasarela, r.NumDispositivoSlot
		FROM RecursosTf rTf, Recursos r
		WHERE rTf.IdSistema=id_sistema AND
					rTf.IdRed=id_red AND
					r.IdSistema=rTf.IdSistema AND
					r.IdRecurso=rTf.IdRecurso;
	ELSE
		(SELECT rd.IdRed, r.IdRecurso, r.IdTifx, r.SlotPasarela, r.NumDispositivoSlot
		FROM RecursosTf rTf, Recursos r, Redes rd
		WHERE rd.IdSistema=id_sistema AND
					rTf.IdSistema=rd.IdSistema AND
					rd.IdRed=rTf.IdRed AND
					r.IdSistema=rTf.IdSistema AND
					r.IdRecurso=rTf.IdRecurso)
	UNION
		(SELECT rd.IdRed, null,null,null,null
		FROM Redes rd
		WHERE rd.IdSistema=id_sistema AND
					rd.IdRed not in (SELECT DISTINCT(IdRed) FROM RecursosTf WHERE idRed is not null));
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `AsignacionUsuariosATops`
--

DROP PROCEDURE IF EXISTS `AsignacionUsuariosATops`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AsignacionUsuariosATops`(in id_Sistema char(32))
BEGIN
	SELECT c.IdSectorOriginal AS IdSector,a.IdTop FROM SectoresSectorizacion a, Sectorizaciones b, SectoresSector c
	WHERE b.IdSistema=id_Sistema AND
				b.Activa=true AND
				a.IdSistema=id_Sistema AND
				a.IdSectorizacion=b.IdSectorizacion AND
				c.IdSistema=id_Sistema AND
				c.IdNucleo=a.IdNucleo AND
				c.IdSector=a.IdSector; 
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CreaPosicionesActiva`
--

DROP PROCEDURE IF EXISTS `CreaPosicionesActiva`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreaPosicionesActiva`(in id_sistema char(32), in id_sectorizacion char(32), in id_activa char(32))
BEGIN
		INSERT into radio
			SELECT IdSistema,id_activa as IdSectorizacion,IdNucleo,IdSector,PosHMI,IdDestino,Prioridad,PrioridadSIP,ModoOperacion,Cascos,Literal,SupervisionPortadora FROM Radio
				WHERE IdSectorizacion=id_sectorizacion AND
							IdSistema=id_sistema;
		REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('radio');
	
		INSERT into Internos
			SELECT IdSistema,id_activa as IdSectorizacion,IdNucleo,IdSector,IdDestino,IdPrefijo,TipoAcceso,PosHMI,Prioridad,OrigenR2,PrioridadSIP,Literal FROM Internos
				WHERE IdSectorizacion=id_sectorizacion AND
							IdSistema=id_sistema;
		REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('Internos');
		
		INSERT into Externos
			SELECT IdSistema,id_activa as IdSectorizacion,IdNucleo,IdSector,IdDestino,IdPrefijo,TipoAcceso,PosHMI,Prioridad,OrigenR2,PrioridadSIP,Literal FROM Externos
				WHERE IdSectorizacion=id_sectorizacion AND
							IdSistema=id_sistema;
		REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('Externos');
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CrearFicheroBackup`
--

DROP PROCEDURE IF EXISTS `CrearFicheroBackup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CrearFicheroBackup`(in id_sistema char(32), in tabla text, in fichero text,in fDesde datetime, in fHasta datetime)
BEGIN

DECLARE lista CHAR(55);



set @SQL = concat("SELECT *",
			"INTO OUTFILE '", fichero, "'", 
      " FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' LINES TERMINATED BY '\\n'",
      " FROM ", tabla,
      " WHERE IdSistema='", id_sistema,
			"' AND DATE_FORMAT(FechaHora,'%Y/%m/%d')>='",DATE_FORMAT(fDesde,'%Y/%m/%d'), 
			"' AND DATE_FORMAT(FechaHora,'%Y/%m/%d')< '",DATE_FORMAT(fHasta,'%Y/%m/%d'),
			"';"); 


PREPARE stmt FROM @SQL; 
EXECUTE stmt; 

set @SQL_DELETE = concat("DELETE FROM ", tabla,
      " WHERE IdSistema='", id_sistema,
			"' AND DATE_FORMAT(FechaHora,'%Y/%m/%d')>='",DATE_FORMAT(fDesde,'%Y/%m/%d'), 
			"' AND DATE_FORMAT(FechaHora,'%Y/%m/%d')< '",DATE_FORMAT(fHasta,'%Y/%m/%d'),
			"';"); 


PREPARE stmt_delete FROM @SQL_DELETE; 
EXECUTE stmt_delete; 
	




END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CreaSectoresActiva`
--

DROP PROCEDURE IF EXISTS `CreaSectoresActiva`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreaSectoresActiva`(in id_sistema char(32), in id_sectorizacion char(32), in id_activa char(32))
BEGIN
	INSERT into SectoresSectorizacion
	SELECT IdSistema,id_activa as IdSectorizacion,IdNucleo,IdSector,IdTop FROM SectoresSectorizacion
		WHERE IdSectorizacion=id_sectorizacion AND
					IdSistema=id_sistema;
					
	REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('SectoresSectorizacion');
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CreaSectoresActivaConexionSacta`
--

DROP PROCEDURE IF EXISTS `CreaSectoresActivaConexionSacta`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreaSectoresActivaConexionSacta`(in id_sistema char(32), in id_sectorizacion char(32), in id_activa char(32))
BEGIN
	INSERT into SectoresSectorizacion
		SELECT ss.IdSistema,id_activa as IdSectorizacion,ss.IdNucleo,ss.IdSector,ss.IdTOP FROM SectoresSectorizacion ss, Sectores s
		WHERE ss.IdSistema=id_sistema AND
					(ss.IdSectorizacion='SACTA' OR
						(ss.IdSectorizacion=id_sectorizacion AND 
							s.Tipo='M' AND	
							ss.IdTOP NOT IN (SELECT idtop 
																	FROM sectoressectorizacion 
																	WHERE idsistema=id_sistema AND idsectorizacion='SACTA'))) AND
					ss.IdSector=s.IdSector;

	REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('SectoresSectorizacion');
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `CuantasTeclasConPrioridadUno`
--

DROP PROCEDURE IF EXISTS `CuantasTeclasConPrioridadUno`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `CuantasTeclasConPrioridadUno`(in id_sistema varchar(32), in id_nucleo varchar(32), out cuantas int)
BEGIN
  DECLARE howManyInt int default 0;
  DECLARE howManyExt int default 0;
  SET cuantas=0;
	
  SELECT COUNT(*) into howManyInt FROM DestinosInternosSector
    WHERE IdSistema=id_sistema AND 
          IdNucleo=id_nucleo AND
          IdPrefijo=0 AND
          Prioridad=1;

  SELECT COUNT(*) into howManyExt FROM DestinosExternosSector
    WHERE IdSistema=id_sistema AND 
          IdNucleo=id_nucleo AND
          IdPrefijo=1 AND
          Prioridad=1;

  SET cuantas=howManyInt + howManyExt;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosExternosAsignadosASector`
--

DROP PROCEDURE IF EXISTS `DestinosExternosAsignadosASector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosExternosAsignadosASector`(in id_sistema char(32), in id_sector char(32), in telefonia BOOL)
BEGIN
	IF telefonia=true THEN
		IF id_sector IS NOT NULL THEN
			SELECT * FROM DestinosExternosSector
			WHERE IdSistema=id_sistema AND
						IdSector=id_sector AND
						IdPrefijo<>1;
		ELSE
			SELECT * FROM DestinosExternosSector
			WHERE IdSistema=id_sistema AND
						IdPrefijo<>1;
		END IF;
	ELSE
		IF id_sector IS NOT NULL THEN
			SELECT * FROM DestinosExternosSector
			WHERE IdSistema=id_sistema AND
						IdSector=id_sector AND
						IdPrefijo=1;
		ELSE
			SELECT * FROM DestinosExternosSector
			WHERE IdSistema=id_sistema AND
						IdPrefijo=1;
		END IF;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosInstantaneosSinAsignarAEnlaces`
--

DROP PROCEDURE IF EXISTS `DestinosInstantaneosSinAsignarAEnlaces`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosInstantaneosSinAsignarAEnlaces`(in id_sistema char(32))
BEGIN
	SELECT * FROM Destinos
	WHERE IdDestino not in (SELECT IdDestino FROM EnlacesLCEN 
														WHERE IdSistema=id_sistema AND IdPrefijo in ("0","1")) AND
				IdSistema=id_sistema AND
				IdPrefijo in ("0","1");
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosInternosAsignadosASector`
--

DROP PROCEDURE IF EXISTS `DestinosInternosAsignadosASector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosInternosAsignadosASector`(in id_sistema char(32), in id_sector char(32), in telefonia BOOL)
BEGIN
	IF telefonia=true THEN
		IF id_sector IS NOT NULL THEN
			SELECT * FROM DestinosInternosSector
			WHERE IdSistema=id_sistema AND
						IdSector=id_sector AND
						IdPrefijo=2;
		ELSE
			SELECT * FROM DestinosInternosSector
			WHERE IdSistema=id_sistema AND
						IdPrefijo=2;
		END IF;
	ELSE
		IF id_sector IS NOT NULL THEN
			SELECT * FROM DestinosInternosSector
			WHERE IdSistema=id_sistema AND
						IdSector=id_sector AND
						IdPrefijo=0;
		ELSE
			SELECT * FROM DestinosInternosSector
			WHERE IdSistema=id_sistema AND
						IdPrefijo=0;
		END IF;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosInternosSinAsignarAEnlaces`
--

DROP PROCEDURE IF EXISTS `DestinosInternosSinAsignarAEnlaces`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosInternosSinAsignarAEnlaces`(in id_sistema char(32))
BEGIN
	SELECT * FROM Destinos
	WHERE IdDestino not in (SELECT IdDestino FROM EnlacesInternos 
														WHERE IdSistema=id_sistema AND IdPrefijo <> "1") AND
				IdSistema=id_sistema AND
				IdPrefijo <> "1";
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosLineaCalienteSinAsignarAlSector`
--

DROP PROCEDURE IF EXISTS `DestinosLineaCalienteSinAsignarAlSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosLineaCalienteSinAsignarAlSector`(in id_sistema char(32), in id_usuario char(32))
BEGIN
		(select a.* from destinosTelefonia a, destinosInternos b, Sectores c
			where a.IdSistema=id_sistema AND
					a.IdPrefijo=0 AND 
					a.IdSistema=b.IdSistema AND
					a.IdPrefijo=b.IdPrefijo AND
					a.IdDestino=b.IdDestino AND
					b.IdSector<>id_usuario AND
					c.IdSector=b.IdSector AND
					c.SectorSimple=true AND
					b.IdDestino not in (SELECT IdDestino FROM DestinosInternosSector
															WHERE IdSistema=id_sistema AND
																		IdSector=id_usuario AND
																		IdPrefijo=0))
			UNION
			(select a.* from destinosTelefonia a
			where a.IdSistema=id_sistema AND
					a.IdPrefijo=1 AND 
					a.IdDestino not in (SELECT IdDestino FROM DestinosExternosSector
															WHERE IdSistema=id_sistema AND
																		
																		IdPrefijo=1));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosPorGruposTelefonia`
--

DROP PROCEDURE IF EXISTS `DestinosPorGruposTelefonia`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosPorGruposTelefonia`(in id_sistema varchar(32))
BEGIN
	SELECT g.idgrupo,d.IdDestino
	FROM GruposTelefonia g, destinostelefonia d
	WHERE g.IdSistema=id_sistema AND
				d.IdSistema = g.IdSistema AND
				d.IdGrupo=g.IdGrupo
UNION
	SELECT idgrupo,null 
		FROM grupostelefonia 
		WHERE IdSistema=id_sistema AND 
					IdGrupo NOT IN (SELECT IdGrupo FROM destinostelefonia WHERE idSistema=id_sistema AND IdGrupo is not null);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosRadioAsignadosASector`
--

DROP PROCEDURE IF EXISTS `DestinosRadioAsignadosASector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `DestinosRadioAsignadosASector`(in id_sistema char(32), in id_usuario char(32), in pagina int, in frecPorPagina int)
BEGIN
	SELECT * FROM DestinosRadioSector
				WHERE Idsistema=id_sistema AND
							IdSector=id_usuario AND
							(((PosHMI-1) div frecPorPagina) = pagina);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosRadioSectorizados`
--

DROP PROCEDURE IF EXISTS `DestinosRadioSectorizados`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `DestinosRadioSectorizados`(in id_sistema char(32), in id_sector char(32), in id_sectorizacion char(32))
BEGIN
		SELECT * FROM radio
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdSector=id_sector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosRadioSectorizadosParaXML`
--

DROP PROCEDURE IF EXISTS `DestinosRadioSectorizadosParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosRadioSectorizadosParaXML`(in id_sistema char(32), in id_sectorizacion char(32))
BEGIN
	IF (id_sectorizacion IS NOT NULL) THEN
		SELECT * FROM radio
			WHERE IdSistema=id_sistema AND
						IdSectorizacion=id_sectorizacion;
	ELSE
		SELECT * FROM DestinosRadioSector
			WHERE IdSistema=id_sistema;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosRadioSinAsignarALaPaginaDelSector`
--

DROP PROCEDURE IF EXISTS `DestinosRadioSinAsignarALaPaginaDelSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `DestinosRadioSinAsignarALaPaginaDelSector`(in id_sistema char(32), in id_usuario char(32), in pagina int, in frecPorPagina int)
BEGIN
	SELECT IdDestino FROM DestinosRadio DR
	WHERE idSistema=id_sistema AND
				0 < (SELECT COUNT(*) FROM RecursosRadio RR WHERE RR.IdDestino=DR.IdDestino) AND
				IdDestino not in (select IdDestino from DestinosRadioSector
														where Idsistema=id_sistema AND
																	IdSector=id_usuario AND
																	(((PosHMI-1) div frecPorPagina) = pagina));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosTelefoniaSectorizados`
--

DROP PROCEDURE IF EXISTS `DestinosTelefoniaSectorizados`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `DestinosTelefoniaSectorizados`(in id_sistema char(32), in id_sector char(32), in id_sectorizacion char(32), in internos BOOL)
BEGIN
	IF internos=true THEN
		SELECT * FROM internos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdSector=id_sector;
	ELSE
			SELECT * FROM externos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdSector=id_sector;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosTelefoniaSectorizadosParaXML`
--

DROP PROCEDURE IF EXISTS `DestinosTelefoniaSectorizadosParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DestinosTelefoniaSectorizadosParaXML`(in id_sistema char(32), in id_sectorizacion char(32), in lc BOOL)
BEGIN
	IF (id_sectorizacion IS NOT NULL) THEN
		IF lc=true THEN
			(SELECT * FROM internos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdPrefijo=0)
			UNION
			(SELECT * FROM externos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdPrefijo=1);
		ELSE
			(SELECT * FROM internos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdPrefijo=2)
			UNION
			(SELECT * FROM externos
					WHERE IdSistema=id_sistema AND
								IdSectorizacion=id_sectorizacion AND
								IdPrefijo<>1);
		END IF;			
	ELSE	
		IF lc=true THEN
			(SELECT * FROM DestinosInternosSector
					WHERE IdSistema=id_sistema AND
								IdPrefijo=0)
			UNION
			(SELECT d.IdSistema,d.IdDestino,d.TipoDestino,d.IdNucleo,d.IdSector,d.IdPrefijo,d.PosHMI,d.Prioridad,d.OrigenR2,d.PrioridadSIP,d.TipoAcceso,d.Literal
					FROM DestinosExternosSector d
					WHERE IdSistema=id_sistema AND
								IdPrefijo=1);
		ELSE
			(SELECT * FROM DestinosInternosSector
					WHERE IdSistema=id_sistema AND
								IdPrefijo=2)
			UNION
			(SELECT d.IdSistema,d.IdDestino,d.TipoDestino,d.IdNucleo,d.IdSector,d.IdPrefijo,d.PosHMI,d.Prioridad,d.OrigenR2,d.PrioridadSIP,d.TipoAcceso,d.Literal
					FROM DestinosExternosSector d
					WHERE IdSistema=id_sistema AND
								IdPrefijo<>1);
		END IF;			
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `DestinosTelefoniaSinAsignarAlSector`
--

DROP PROCEDURE IF EXISTS `DestinosTelefoniaSinAsignarAlSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `DestinosTelefoniaSinAsignarAlSector`(in id_sistema char(32), in id_usuario char(32))
BEGIN
		(select a.* from destinosTelefonia a, destinosInternos b
			where a.IdSistema=id_sistema AND
				a.IdPrefijo=2 AND 
				a.IdSistema=b.IdSistema AND
				a.IdPrefijo=b.IdPrefijo AND
				a.IdDestino=b.IdDestino AND
				b.IdSector<>id_usuario AND
				b.IdDestino in (SELECT s.IdSector FROM Sectores s
														WHERE s.IdSistema=id_sistema AND
																	s.SectorSimple) AND
				b.IdDestino not in (SELECT IdDestino FROM DestinosInternosSector
														WHERE IdSistema=id_sistema AND
																	IdSector=id_usuario AND
																	IdPrefijo=2)
				ORDER BY a.IdDestino)
UNION
(select a.* from destinosTelefonia a
	where a.IdSistema=id_sistema AND
				a.TipoDestino=1 AND 
				a.IdPrefijo<>1 AND
				a.IdDestino not in (SELECT IdDestino FROM DestinosExternosSector
														WHERE IdSistema=id_sistema AND
																	IdSector=id_usuario)
				ORDER BY a.IdDestino);

END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `EliminaActiva`
--

DROP PROCEDURE IF EXISTS `EliminaActiva`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `EliminaActiva`(in id_sistema char(32))
BEGIN
	DECLARE id char(32);
	
	SELECT FechaActivacion INTO id FROM sectorizaciones WHERE IdSistema=id_sistema AND Activa=true;
	
	DELETE FROM Sectorizaciones
		WHERE NOT Activa AND
					IdSistema=id_sistema AND
					FechaActivacion=id;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ExisteIP`
--

DROP PROCEDURE IF EXISTS `ExisteIP`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ExisteIP`(in id_sistema varchar(32), in ip varchar(15), in id_hw varchar(32))
BEGIN
	select 	(select count(*) from Top where IdSistema=id_sistema AND IdTOP!=id_hw AND (IpRed1=ip OR IpRed2=ip)) +
					(select count(*) from Tifx where IdSistema=id_sistema AND IdTifx!=id_hw AND (IpRed1=ip OR IpRed2=ip)) +
					(select count(*) from EquiposEu where IdSistema=id_sistema AND idEquipos!=id_hw AND (IpRed1=ip OR IpRed2=ip));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `FinalizaTransaccion`
--

DROP PROCEDURE IF EXISTS `FinalizaTransaccion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `FinalizaTransaccion`()
BEGIN
		start transaction;
		
		insert ignore into cd40.sistema select * from cd40_trans.sistema;
		insert ignore into cd40.abonados select * from cd40_trans.abonados;
		
		commit;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GeneraRegistroBackup`
--

DROP PROCEDURE IF EXISTS `GeneraRegistroBackup`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GeneraRegistroBackup`(in id_sistema char(32), in tipo_backup int, in fDesde datetime, in fHasta datetime, in recurso text)
BEGIN
	INSERT INTO registrobackup (
	   IdSistema
	  ,TipoBackup
	  ,FechaInicio
	  ,FechaFin
	  ,RecursoDestino
	) VALUES (
	   id_sistema 															
	  ,tipo_backup 															
	  ,DATE_FORMAT(fDesde,'%Y/%m/%d %H:%i:%s') 	
	  ,DATE_FORMAT(fHasta,'%Y/%m/%d %H:%i:%s') 	
	  ,recurso 																	
	);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetEventosRadio`
--

DROP PROCEDURE IF EXISTS `GetEventosRadio`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetEventosRadio`(in id_sistema varchar(32), in desde date, in hasta date)
BEGIN
	SELECT * FROM EventosRadio	
	WHERE IdSistema=id_sistema AND
				DATE_FORMAT(FechaHora,"%Y/%m/%d") >= desde AND
				DATE_FORMAT(FechaHora,"%Y/%m/%d") < hasta
	ORDER BY FechaHora;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetEventosTelefonia`
--

DROP PROCEDURE IF EXISTS `GetEventosTelefonia`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetEventosTelefonia`(in id_sistema varchar(32), in desde date, in hasta date)
BEGIN
	SELECT * FROM EventosTelefonia	
	WHERE IdSistema=id_sistema AND
				DATE_FORMAT(FechaHora,"%Y/%m/%d") >= desde AND
				DATE_FORMAT(FechaHora,"%Y/%m/%d") < hasta
	ORDER BY FechaHora;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `GetFunciones`
--

DROP PROCEDURE IF EXISTS `GetFunciones`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetFunciones`(in id_sistema varchar(32), in _tipo int)
BEGIN
	SELECT * FROM funciones
	WHERE IdSistema=id_sistema AND
				Tipo=_tipo;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

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

--
-- Definition of procedure `GetUnHistorico`
--

DROP PROCEDURE IF EXISTS `GetUnHistorico`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUnHistorico`(in id_sistema varchar(32),in lista_incidencias text,in tipo_hw int, in id_hw varchar(32), in desde date, in hasta date)
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
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
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
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
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
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
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
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
					END WHILE;
			END IF;
		END IF;
	ELSE
		IF (id_hw="") THEN
			IF (tipo_hw=-1) THEN																		
				INSERT INTO t 
					SELECT hi.*,i.Descripcion FROM HistoricoIncidencias hi, incidencias i WHERE hi.IdSistema=id_sistema AND 
								DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
								DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
								i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
			ELSE
				INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias  hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
									hi.TipoHw=tipo_hw AND 
									DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
									DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
								i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
			END IF;
		ELSE
			IF (tipo_hw=-1) THEN																		
				INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
							hi.IdHw=id_hw AND 
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
			ELSE
				INSERT INTO t 
					SELECT hi.*, i.Descripcion FROM HistoricoIncidencias hi, incidencias i
						WHERE hi.IdSistema=id_sistema AND
							hi.TipoHw=tipo_hw AND
							hi.IdHw=id_hw AND 
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") >= desde AND
							DATE_FORMAT(hi.FechaHora,"%Y/%m/%d") < hasta AND
							i.IdIncidencia=hi.IdIncidencia
						LIMIT 1;
			END IF;
		END IF;
	END IF;

	SELECT * FROM t				
		ORDER BY -FechaHora LIMIT 1;
		
	DROP TABLE t;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `IdManttoLibre`
--

DROP PROCEDURE IF EXISTS `IdManttoLibre`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `IdManttoLibre`(in id_Sistema varchar(32), out id_Sacta int)
BEGIN
	DECLARE encontrado bool default false;
	DECLARE nSacta int default 10001;
	DECLARE id int;
	
	set id_Sacta=0;
	
	WHILE ((not encontrado) AND (nSacta < 20000)) DO
		SELECT COUNT(*) INTO id FROM sectores WHERE IdSistema=id_sistema AND NumSacta=nSacta;
		IF (id=0) THEN
			SET encontrado=true;
			SET id_Sacta=nSacta;
		ELSE
			SET nSacta=nSacta+1;
		END IF;
	END WHILE;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `IniciaTransaccion`
--

DROP PROCEDURE IF EXISTS `IniciaTransaccion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `IniciaTransaccion`()
BEGIN
		create table if not exists cd40_trans.sistema like cd40.sistema;
		create table if not exists cd40_trans.abonados like cd40.abonados;
		
		start transaction;
		
		delete from cd40_trans.sistema;
		delete from cd40_trans.abonados;
		
		insert into cd40_trans.sistema select * from cd40.sistema;
		insert into cd40_trans.abonados select * from cd40.abonados;
		
		commit;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ListaRecursosDestino`
--

DROP PROCEDURE IF EXISTS `ListaRecursosDestino`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ListaRecursosDestino`(in id_sistema varchar(32), in id_destino varchar(32), in id_prefijo int)
BEGIN
	IF (id_prefijo=1) THEN
		SELECT rLC.*,r.Interface FROM recursoslcen rLC, recursos r
		WHERE rLC.IdSistema=id_sistema AND
					rLC.IdDestino=id_destino AND
					rLC.IdPrefijo=id_prefijo AND
					r.IdSistema=rLC.IdSistema AND
					r.IdRecurso=rLC.IdRecurso AND
					r.TipoRecurso=rLC.TipoRecurso;
	ELSE
		SELECT rTf.*,r.Interface FROM recursostf rTf, recursos r
		WHERE rTf.IdSistema=id_sistema AND
					rTf.IdDestino=id_destino AND
					rTf.IdPrefijo=id_prefijo AND
					r.IdSistema=rTf.IdSistema AND
					r.IdRecurso=rTf.IdRecurso AND
					r.TipoRecurso=rTf.TipoRecurso;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ListaRutas`
--

DROP PROCEDURE IF EXISTS `ListaRutas`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ListaRutas`(in id_Sistema char(32), in id_Central char(32))
BEGIN
	select IdRuta as IdRuta,Tipo as Tipo from Rutas 
		where IdSistema=id_Sistema AND
				Central=id_Central
		ORDER BY Orden;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `LoginTop`
--

DROP PROCEDURE IF EXISTS `LoginTop`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `LoginTop`(in id_Sistema char(32),in id_Hw char(32), out id_Usuario char(32), out modo_arranque char(1))
BEGIN
	SELECT b.idsector,c.ModoArranque  INTO id_Usuario, modo_arranque
	FROM sectorizaciones a, SectoresSectorizacion b, top c
	WHERE a.idSistema=id_Sistema AND
				c.IdSistema=id_Sistema AND
				a.Activa=true AND
				b.idSistema=id_Sistema AND
				a.idSectorizacion=b.idSectorizacion AND
				b.idTOP=id_Hw AND
				b.IdTop=c.IdTOP;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NivelesIntrusion`
--

DROP PROCEDURE IF EXISTS `NivelesIntrusion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `NivelesIntrusion`(in id_sistema char(32), in id_usuario char(32))
BEGIN
	SELECT n.* FROM Niveles n, SectoresSector ss
	WHERE ss.IdSistema=id_sistema AND
				ss.IdSector=id_usuario AND
				n.IdSistema=id_sistema AND
				n.IdSector=ss.IdSectorOriginal AND
				ss.EsDominante;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NivelesIntrusionParaXML`
--

DROP PROCEDURE IF EXISTS `NivelesIntrusionParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `NivelesIntrusionParaXML`(in id_sistema varchar(32), in id_sectorizacion varchar(32))
BEGIN
	IF (id_sectorizacion IS NOT NULL) THEN
		SELECT c.IdSector, n.* FROM Niveles n, SectoresSector ss, sectoressectorizacion c
			WHERE ss.IdSistema=id_sistema AND
				c.IdSectorizacion=id_sectorizacion AND
				c.IdSistema=ss.IdSistema AND
				n.IdSistema=ss.IdSistema AND
				n.IdSector=ss.IdSectorOriginal AND
				c.IdSector=ss.IdSector AND
				ss.EsDominante;
	ELSE
		SELECT n.* FROM Niveles n, Sectores s
			WHERE	s.IdSistema=id_sistema AND
						s.SectorSimple AND
						n.IdSistema=s.IdSistema AND
						n.IdSector=s.IdSector AND
						n.IdNucleo=s.IdNucleo;
	END IF;	
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NumerosAbonadoExternos`
--

DROP PROCEDURE IF EXISTS `NumerosAbonadoExternos`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `NumerosAbonadoExternos`(in idSistema char(32), in idUsuario char(32))
BEGIN
	SELECT a.*
		FROM UsuariosAbonados a, Sectorizaciones b
		WHERE b.idSistema=idSistema AND
					b.Activa=true AND
					a.idSectorizacion=b.idSectorizacion AND
					a.idSistema=b.idSistema AND
					a.idSector=idUsuario AND
					a.idPrefijos<>1;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NumerosAbonadoInternos`
--

DROP PROCEDURE IF EXISTS `NumerosAbonadoInternos`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `NumerosAbonadoInternos`(in id_Sistema char(32),in id_Usuario char (32))
BEGIN
	SELECT *
		FROM UsuariosAbonados
		WHERE IdSistema=id_Sistema AND
					IdSector=id_Usuario;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NumerosAbonadosSectorAgrupado`
--

DROP PROCEDURE IF EXISTS `NumerosAbonadosSectorAgrupado`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `NumerosAbonadosSectorAgrupado`(in id_sistema char(32), in id_sector_agrupado char(32))
BEGIN
	SELECT UA.* FROM UsuariosAbonados ua, SectoresAgrupacion ss
	WHERE ss.IdSistema=id_sistema AND
				ua.IdSistema=ss.IdSistema AND
				ss.IdAgrupacion=id_sector_agrupado AND
				ss.IdSector=ua.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `NumerosAbonadosSectorNoAgrupado`
--

DROP PROCEDURE IF EXISTS `NumerosAbonadosSectorNoAgrupado`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `NumerosAbonadosSectorNoAgrupado`(in id_sistema char(32), in id_nucleo char(32), in id_sector_agrupado char(32))
BEGIN
	SELECT UA.* FROM UsuariosAbonados ua, SectoresSector ss
	WHERE ss.IdSistema=id_sistema AND	ss.IdNucleo=id_nucleo AND
				ua.IdSistema=ss.IdSistema AND ua.IdNucleo=ss.IdNucleo AND
				ss.IdSector=id_sector_agrupado AND
				ss.IdSectorOriginal=ua.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ParametrosSectorAgrupado`
--

DROP PROCEDURE IF EXISTS `ParametrosSectorAgrupado`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `ParametrosSectorAgrupado`(in id_sistema char(32), in id_sector_agrupado char(32))
BEGIN
		SELECT ps.* FROM SectoresAgrupacion ss, ParametrosSector ps
		WHERE ss.IdSistema=id_sistema AND
					ps.IdSistema=ss.IdSistema AND
					ss.IdAgrupacion=id_sector_agrupado AND
					ss.IdSector=ps.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ParametrosSectores`
--

DROP PROCEDURE IF EXISTS `ParametrosSectores`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ParametrosSectores`(in id_sistema varchar(32), in lista_nucleos varchar(255), in lista_sectores varchar(255))
BEGIN
	declare i int;
	declare pos int;
	declare nucleo text;
	declare j int;
	declare posj int;
	declare sector text;
	set i=LOCATE(",",lista_nucleos);
	set pos=1;

	create temporary table if not exists ps SELECT * FROM ParametrosSector where false;

	WHILE i>0 DO
		set j=LOCATE(",",lista_sectores);
		set posj=1;

		set nucleo=substring(lista_nucleos,pos,i-pos);
		set pos=i+1;
		set i=LOCATE(",",lista_nucleos,pos);

		WHILE j>0 DO
			set sector=substring(lista_sectores,posj,j-posj);
			set posj=j+1;
			set j=LOCATE(",",lista_sectores,posj);

			INSERT INTO ps
			SELECT * FROM ParametrosSector
				WHERE IdSistema=id_sistema AND
							IdNucleo=nucleo AND
							IdSector=sector;
					
			END WHILE;
		END WHILE;
		
		SELECT * FROM ps;
		DROP TABLE ps;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ParametrosSectoresParaXML`
--

DROP PROCEDURE IF EXISTS `ParametrosSectoresParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `ParametrosSectoresParaXML`(in id_sistema varchar(32), in id_sectorizacion varchar(32))
BEGIN
	IF (id_sectorizacion IS NOT NULL) THEN
		SELECT c.IdSector, n.* FROM ParametrosSector n, SectoresSector ss, sectoressectorizacion c
		WHERE ss.IdSistema=id_sistema AND
				c.IdSectorizacion=id_sectorizacion AND
				c.IdSistema=ss.IdSistema AND
				n.IdSistema=ss.IdSistema AND
				n.IdSector=ss.IdSectorOriginal AND
				c.IdSector=ss.IdSector AND
				ss.EsDominante;
	ELSE
		SELECT n.* FROM ParametrosSector n, Sectores s
			WHERE	s.IdSistema=id_sistema AND
						s.SectorSimple AND
						n.IdSistema=s.IdSistema AND
						n.IdSector=s.IdSector AND
						n.IdNucleo=s.IdNucleo;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `ParametrosSectorNoAgrupado`
--

DROP PROCEDURE IF EXISTS `ParametrosSectorNoAgrupado`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `ParametrosSectorNoAgrupado`(in id_sistema char(32), in id_sector_agrupado char(32))
BEGIN
	SELECT ps.* FROM SectoresSector ss, ParametrosSector ps
		WHERE ss.IdSistema=id_sistema AND
					ps.IdSistema=ss.IdSistema AND
					ss.IdSector=id_sector_agrupado AND
					ss.IdSectorOriginal=ps.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PermisosRedes`
--

DROP PROCEDURE IF EXISTS `PermisosRedes`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `PermisosRedes`(in idSistema char(32), in idUsuario char(32))
BEGIN
		SELECT a.*
		FROM PermisosRedes a, Sectorizaciones b
		WHERE b.idSistema=idSistema AND
					b.Activa=true AND
					a.idSectorizacion=b.idSectorizacion AND
					a.idSistema=b.idSistema AND
					a.idSector=idUsuario;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PlanDireccionamientoIP`
--

DROP PROCEDURE IF EXISTS `PlanDireccionamientoIP`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `PlanDireccionamientoIP`(in id_Sistema char(32))
BEGIN
	(SELECT IdTop as Id,0 as TipoEH, IpRed1, IpRed2
		FROM TOP
		WHERE IdSistema=id_Sistema)
	union
	(SELECT IdTifX as Id,1 as TipoEH , IpRed1, IpRed2
		FROM TifX
		WHERE IdSistema=id_Sistema)
	union
	(SELECT IdEquipos as Id,2 as TipoEH , IpRed1, IpRed2	
		FROM EquiposEU
		WHERE IdSistema=id_Sistema);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PlanDireccionamientoSIP`
--

DROP PROCEDURE IF EXISTS `PlanDireccionamientoSIP`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `PlanDireccionamientoSIP`(in id_Sistema char(32))
BEGIN

	SELECT a.IdPrefijo,a.IdAbonado,ss.IdSectorOriginal as IdSector
	FROM UsuariosAbonados a, SectoresSector ss
	WHERE ss.IdSector in (SELECT c.IdSector FROM Sectorizaciones b, SectoresSectorizacion c
													WHERE b.IdSistema=id_sistema AND
																b.Activa=true AND
																b.IdSectorizacion=c.IdSectorizacion) AND
				ss.EsDominante AND
				a.IdSector=ss.IdSector
	ORDER BY ss.IdSectorOriginal,a.IdPrefijo,a.IdAbonado;


END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PosicionOcupadaEnSector`
--

DROP PROCEDURE IF EXISTS `PosicionOcupadaEnSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `PosicionOcupadaEnSector`(in id_sistema char(32), in id_nucleo char(32), in id_sector char(32), in tipo_acceso char(32), in posicion int, out existe boolean)
BEGIN
	DECLARE existeInterno,existeExterno int;
	
	SELECT count(*) INTO existeInterno
		FROM DestinosInternosSector dis
		WHERE dis.IdSistema=id_sistema AND
					dis.IdNucleo=id_nucleo AND
					dis.IdSector=id_sector AND
					dis.TipoAcceso=tipo_acceso AND
					dis.PosHMI=posicion;
				
	SELECT count(*) INTO existeExterno
		FROM DestinosExternosSector dis
		WHERE dis.IdSistema=id_sistema AND
					dis.IdNucleo=id_nucleo AND
					dis.IdSector=id_sector AND
					dis.TipoAcceso=tipo_acceso AND
					dis.PosHMI=posicion;				
					
	SET existe=((existeInterno>0) OR (existeExterno>0));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PrefijosSinAsignarARedes`
--

DROP PROCEDURE IF EXISTS `PrefijosSinAsignarARedes`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `PrefijosSinAsignarARedes`(in id_Sistema char(32))
BEGIN
	select IdPrefijo from Prefijos
	where IdSistema=id_Sistema AND
				IdPrefijo > 3 AND IdPrefijo < 10 AND
				IdPrefijo not in (select IdPrefijo from Redes where IdSistema=id_Sistema);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `PrimeraPosicionLibre`
--

DROP PROCEDURE IF EXISTS `PrimeraPosicionLibre`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `PrimeraPosicionLibre`(in id_sistema char(32), in id_nucleo char(32), in id_sector char(32), in tipo_acceso char(2), out hueco int)
BEGIN
	DECLARE i int;
	DECLARE huecoInterno, huecoExterno int;
	DECLARE encontrado boolean;
	SET i=0;
	SET encontrado=false;
	SET huecoInterno=0;
	SET huecoExterno=0;
	WHILE (NOT encontrado) AND (i<56) DO
		SET i=i+1;
		SELECT COUNT(*) INTO huecoInterno
			FROM DestinosInternosSector d
			WHERE d.IdSistema=id_sistema AND
						d.IdNucleo=id_nucleo AND
						d.IdSector=id_sector AND
						d.TipoAcceso=tipo_acceso AND
						d.PosHMI=i;
						
		SELECT COUNT(*) INTO huecoExterno
			FROM DestinosExternosSector d
			WHERE d.IdSistema=id_sistema AND
						d.IdNucleo=id_nucleo AND
						d.IdSector=id_sector AND
						d.TipoAcceso=tipo_acceso AND
						d.PosHMI=i;
						
		SET encontrado=((huecoInterno=0) AND (huecoExterno=0));
	END WHILE;

	SET hueco=i;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RangosConIdRed`
--

DROP PROCEDURE IF EXISTS `RangosConIdRed`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RangosConIdRed`(in id_sistema varchar(32), in id_central varchar(32))
BEGIN
	IF (id_central is not null) THEN
		SELECT r.*, red.IdRed
		FROM 	Rangos r, Redes red
		WHERE	r.IdSistema=id_sistema AND
					r.Central=id_central AND
					red.IdSistema=r.IdSistema AND
					r.IdPrefijo=red.IdPrefijo;
	ELSE
		SELECT r.*, red.IdRed
		FROM 	Rangos r, Redes red
		WHERE	r.IdSistema=id_sistema AND
					red.IdSistema=r.IdSistema AND
					r.IdPrefijo=red.IdPrefijo;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosLCENSinAsignarAEnlaces`
--

DROP PROCEDURE IF EXISTS `RecursosLCENSinAsignarAEnlaces`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `RecursosLCENSinAsignarAEnlaces`(in id_sistema char(32))
BEGIN
		SELECT * FROM RecursosLCEN
		WHERE IdSistema=id_sistema AND
					IdDestino is null;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosLcParaInformeXML`
--

DROP PROCEDURE IF EXISTS `RecursosLcParaInformeXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RecursosLcParaInformeXML`(in id_sistema varchar(32))
BEGIN
	SELECT r.*, rt.*,pr.*
	FROM recursos r, recursoslcen rt, parametrosRecurso pr
	WHERE r.IdSistema=id_sistema AND
				rt.IdSistema = r.IdSistema AND rt.IdRecurso = r.IdRecurso AND rt.TipoRecurso = r.TipoRecurso AND
				pr.IdSistema = r.IdSistema AND pr.IdRecurso = r.IdRecurso AND pr.TipoRecurso = r.TipoRecurso;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosPorEmplazamientoParaXML`
--

DROP PROCEDURE IF EXISTS `RecursosPorEmplazamientoParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RecursosPorEmplazamientoParaXML`(in id_sistema varchar(32))
BEGIN
		(SELECT t.IdEmplazamiento,rTf.IdRecurso,r.IdTifx, r.SlotPasarela, r.NumDispositivoSlot
			FROM emplazamientos t, recursosradio rTf, recursos r
			WHERE t.IdSistema=id_sistema AND
				rTf.IdSistema=t.IdSistema AND
				rTf.IdEmplazamiento=t.IdEmplazamiento AND
				r.IdSistema=rTf.IdSistema AND
				r.IdRecurso=rTf.IdRecurso)
	UNION
		(SELECT t.IdEmplazamiento,null,null,null,null
			FROM emplazamientos t
			WHERE t.IdSistema=id_sistema AND
					t.IdEmplazamiento NOT IN (SELECT DISTINCT(IdEmplazamiento) FROM recursosradio WHERE IdSistema=id_sistema AND IdEmplazamiento IS NOT NULL));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosPorTroncalParaXML`
--

DROP PROCEDURE IF EXISTS `RecursosPorTroncalParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RecursosPorTroncalParaXML`(in id_sistema varchar(32))
BEGIN
		(SELECT t.IdTroncal,t.NumTest,rTf.IdRecurso,r.IdTifx, r.SlotPasarela, r.NumDispositivoSlot
			FROM troncales t, recursostf rTf, recursos r
			WHERE t.IdSistema=id_sistema AND
				rTf.IdSistema=t.IdSistema AND
				rTf.IdTroncal=t.IdTroncal AND
				r.IdSistema=rTf.IdSistema AND
				r.IdRecurso=rTf.IdRecurso)
	UNION
		(SELECT t.IdTroncal,t.NumTest,null,null,null,null
			FROM troncales t
			WHERE t.IdSistema=id_sistema AND
					t.IdTroncal NOT IN (SELECT DISTINCT(IdTroncal) FROM recursostf WHERE IdSistema=id_sistema AND IdTroncal IS NOT NULL));
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosRadioSinAsignarAEnlaces`
--

DROP PROCEDURE IF EXISTS `RecursosRadioSinAsignarAEnlaces`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `RecursosRadioSinAsignarAEnlaces`(in id_sistema char(32))
BEGIN
	SELECT * FROM RecursosRadio
		WHERE IdSistema=id_sistema AND
					IdDestino is null;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosRdParaInformeXML`
--

DROP PROCEDURE IF EXISTS `RecursosRdParaInformeXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RecursosRdParaInformeXML`(in id_sistema varchar(32))
BEGIN
	SELECT r.*, rt.*,pr.*
	FROM recursos r, recursosradio rt, parametrosRecurso pr
	WHERE r.IdSistema=id_sistema AND
				rt.IdSistema = r.IdSistema AND rt.IdRecurso = r.IdRecurso AND rt.TipoRecurso = r.TipoRecurso AND
				pr.IdSistema = r.IdSistema AND pr.IdRecurso = r.IdRecurso AND pr.TipoRecurso = r.TipoRecurso;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosSinAsignar`
--

DROP PROCEDURE IF EXISTS `RecursosSinAsignar`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `RecursosSinAsignar`(in id char(32),in id_Sistema char(32))
BEGIN
	select * from recursos
	where IdSistema=id_Sistema && @id is null;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosTfParaInformeXML`
--

DROP PROCEDURE IF EXISTS `RecursosTfParaInformeXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RecursosTfParaInformeXML`(in id_sistema varchar(32))
BEGIN
	SELECT r.*, pr.*, rt.IdPrefijo,rt.TipoDestino,rt.IdDestino,rt.IdTroncal,rt.IdRed,rt.Lado,rt.Modo
	FROM recursos r, recursostf rt, parametrosRecurso pr
	WHERE r.IdSistema=id_sistema AND
				rt.IdSistema = r.IdSistema AND rt.IdRecurso = r.IdRecurso AND rt.TipoRecurso = r.TipoRecurso AND
				pr.IdSistema = r.IdSistema AND pr.IdRecurso = r.IdRecurso AND pr.TipoRecurso = r.TipoRecurso;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RecursosTFSinAsignarAEnlaces`
--

DROP PROCEDURE IF EXISTS `RecursosTFSinAsignarAEnlaces`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `RecursosTFSinAsignarAEnlaces`(in id_sistema char(32))
BEGIN
		SELECT rtf.* FROM RecursosTF rtf, recursos r
		WHERE rtf.IdSistema=id_sistema AND
					IdDestino is null AND
					r.IdSistema=id_sistema AND
					r.IdRecurso=rtf.IdRecurso AND
					r.TipoRecurso=rtf.TipoRecurso AND
					r.Interface in (2,3);			
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RedesUsuariosAbonados`
--

DROP PROCEDURE IF EXISTS `RedesUsuariosAbonados`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `RedesUsuariosAbonados`(in id_Sistema char(32), in id_Nucleo char(32), in id_Sector char(32))
BEGIN
	SELECT red.IdRed, red.IdPrefijo, abonado.IdAbonado 
	FROM Redes red, UsuariosAbonados abonado
	WHERE abonado.IdSistema=id_Sistema AND
				abonado.IdNucleo=id_Nucleo AND
				abonado.IdSector=id_Sector AND
				red.IdSistema=abonado.IdSistema AND
				red.IdPrefijo=abonado.IdPrefijo;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `RedesUsuariosAbonadosParaXML`
--

DROP PROCEDURE IF EXISTS `RedesUsuariosAbonadosParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RedesUsuariosAbonadosParaXML`(in id_sistema varchar(32), in id_sectorizacion varchar(32))
BEGIN
			SELECT abonado.IdNucleo, c.IdSector,c.IdSectorOriginal, red.IdRed, red.IdPrefijo, abonado.IdAbonado 
			FROM Redes red, UsuariosAbonados abonado, sectoressector c
			WHERE abonado.IdSistema=id_sistema AND
						abonado.IdSistema=c.IdSistema AND
						abonado.IdNucleo=c.IdNucleo AND
						abonado.IdSector=c.IdSectorOriginal AND
						red.IdSistema=abonado.IdSistema AND
						red.IdPrefijo=abonado.IdPrefijo;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectorConNumeroAbonado`
--

DROP PROCEDURE IF EXISTS `SectorConNumeroAbonado`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `SectorConNumeroAbonado`(in id_sistema char(32), in id_nucleo char(32), in id_prefijo int, in id_abonado char(32))
BEGIN
	SELECT s.IdSector FROM UsuariosAbonados ua, Sectores s
		WHERE ua.IdSistema=id_sistema AND
					ua.IdNucleo=id_nucleo AND
					ua.IdPrefijo=id_prefijo AND
					ua.IdAbonado=id_abonado AND
					s.IdSistema=ua.IdSistema AND
					s.IdNucleo=ua.IdNucleo AND
					s.SectorSimple AND
					ua.IdSector=s.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresCompuestosPorSectorOriginal`
--

DROP PROCEDURE IF EXISTS `SectoresCompuestosPorSectorOriginal`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `SectoresCompuestosPorSectorOriginal`(in id_sistema char(32), in id_sector_original char(32))
BEGIN
	SELECT s.IdSector from SectoresSector ss, Sectores s
	where ss.IdSistema=id_sistema AND
				ss.IdSectorOriginal=id_sector_original AND
				ss.IdSectorOriginal!=ss.IdSector AND
				s.IdSector=ss.IdSector;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresFueraDeAgrupacion`
--

DROP PROCEDURE IF EXISTS `SectoresFueraDeAgrupacion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `SectoresFueraDeAgrupacion`(in id_sistema varchar(32), in id_agrupacion varchar(32))
BEGIN
	IF id_agrupacion IS NOT NULL THEN
		SELECT s.IdSector FROM Sectores s
			WHERE s.IdSistema=id_sistema AND
						s.SectorSimple=true AND
						s.IdSector NOT IN (SELECT IdSector FROM	SectoresAgrupacion	
																	WHERE IdSistema=id_sistema AND
																				IdAgrupacion=id_agrupacion);
	ELSE
		SELECT s.IdSector FROM Sectores s
			WHERE s.IdSistema=id_sistema AND
						s.SectorSimple=true;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresManttoEnActiva`
--

DROP PROCEDURE IF EXISTS `SectoresManttoEnActiva`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `SectoresManttoEnActiva`(in id_sistema char(32))
BEGIN
	SELECT  s.NumSacta, t.PosicionSacta
	FROM Sectores s, sectorizaciones sct, sectoressectorizacion ss, top t
	WHERE sct.IdSistema=id_sistema AND
				sct.Activa AND
				ss.IdSistema = sct.IdSistema AND ss.IdSectorizacion = sct.IdSectorizacion AND
				ss.IdSistema = s.IdSistema AND ss.IdNucleo = s.IdNucleo AND ss.IdSector = s.IdSector AND s.SectorSimple AND s.Tipo='M' AND
				ss.IdTOP=t.IdTOP;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresManttoEnTop`
--

DROP PROCEDURE IF EXISTS `SectoresManttoEnTop`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SectoresManttoEnTop`(in id_sistema varchar(32),in id_sectorizacion varchar(32), in id_top varchar(32))
BEGIN
	select s.* from sectores s, sectoressectorizacion ss, sectoressector sct
	where ss.IdSistema=id_sistema AND
				ss.IdSistema=sct.IdSistema AND
				s.IdSistema=sct.IdSistema AND
				ss.IdSectorizacion=id_sectorizacion AND
				ss.IdTOP=id_top AND
				ss.IdSector=sct.IdSector AND
				sct.IdSectorOriginal=s.IdSector AND
				s.SectorSimple AND
				s.Tipo='M';
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresNumSactaSorted`
--

DROP PROCEDURE IF EXISTS `SectoresNumSactaSorted`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SectoresNumSactaSorted`(in id_sistema char(32), in id_nucleo char(32), in lista_sectores text)
BEGIN
	DECLARE lista text;
	SET @lista=lista_sectores;
	SELECT IdSector from Sectores
	where IdSistema=id_sistema AND IdNucleo=id_nucleo AND
		IdSector in (@lista)
	ORDER BY NumSacta;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresReales`
--

DROP PROCEDURE IF EXISTS `SectoresReales`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SectoresReales`(in id_sistema char(32), in id_sectorizacion char(32), out cuantos int)
BEGIN
	SELECT count(*) into cuantos FROM Sectores 
	WHERE IdSistema=id_sistema AND
				SectorSimple AND
				Tipo='R' AND
				IdNucleo IN (SELECT DISTINCT IdNucleo FROM SectoresSectorizacion WHERE IdSistema=id_sistema AND IdSectorizacion=id_sectorizacion) AND 
				IdSector NOT IN (SELECT IdSectorOriginal FROM SectoresSector ss,SectoresSectorizacion sz
													WHERE sz.IdSectorizacion=id_sectorizacion AND
																sz.IdSistema=id_sistema AND
																ss.IdSistema=sz.IdSistema AND
																ss.IdSector=sz.IdSector);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SectoresSinAsignarASectorizacion`
--

DROP PROCEDURE IF EXISTS `SectoresSinAsignarASectorizacion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SectoresSinAsignarASectorizacion`(in id_sistema char(32),in id_sectorizacion char(32), in id_nucleo char(32))
BEGIN
	select a.IdSector, a.Tipo from Sectores a
	where a.IdSistema=id_sistema AND
				a.IdNucleo=id_nucleo AND
				a.SectorSimple=true AND
				a.IdSector not in (select ss.IdSectorOriginal FROM sectoresSectorizacion ssz, sectoresSector ss
																		WHERE ssz.IdSistema=id_sistema AND
																					ssz.IdNucleo=id_nucleo AND
																					ssz.IdSectorizacion=id_sectorizacion AND
																					ss.IdSistema=ssz.IdSistema AND ss.IdNucleo=ssz.IdNucleo AND 
																					ss.IdSector=ssz.IdSector);
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `SlotsLibresEnTifx`
--

DROP PROCEDURE IF EXISTS `SlotsLibresEnTifx`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `SlotsLibresEnTifx`(in id_sistema char(32), in cuantos int)
BEGIN
	CASE cuantos
		WHEN 8 THEN
				SELECT l.Id FROM (
									select b.idtifx as Id,16-sum(a.2bd) as libres 
																								from (
																											(select idtifx as id,count(*)*2 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=8 
																																group by idtifx) UNION
																										 	(select idtifx as id,count(*)*8 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=7 
																																group by idtifx) UNION
																											(select idtifx as id,count(*)*16 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=9
																																group by idtifx) UNION
																											(select idtifx as id,count(*) as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface in (0,1,2,3,4,5,6)
																																group by idtifx) UNION
																											(select idtifx as id,0 as 2BD from TIFX 
																													where IdSistema=id_sistema AND
																																IdTifx NOT IN (SELECT DISTINCT(IdTifx) FROM Recursos WHERE IdSistema=id_sistema))
																											) a, tifx b
													where b.IdSistema=id_sistema AND
							  								a.id=b.IdTIFX
													group by a.id
								) l
				WHERE l.libres>=2;
			WHEN 7 THEN
				SELECT l.Id FROM (
									select b.idtifx as Id,16-sum(a.2bd) as libres 
																								from (
																											(select idtifx as id,((count(*)DIV 2)*4) + ((count(*)%2)*4)as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=8 
																																group by idtifx) UNION
																										 	(select idtifx as id,count(*)*8 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=7 
																																group by idtifx) UNION
																											(select idtifx as id,count(*)*16 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=9
																																group by idtifx) UNION
																											(select idtifx as id,count(*) as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface in (0,1,2,3,4,5,6)
																																group by idtifx) UNION
																											(select idtifx as id,0 as 2BD from TIFX 
																													where IdSistema=id_sistema AND
																																IdTifx NOT IN (SELECT DISTINCT(IdTifx) FROM Recursos WHERE IdSistema=id_sistema))
																											) a, tifx b
													where b.IdSistema=id_sistema AND
							  								a.id=b.IdTIFX
													group by a.id
								) l
				WHERE l.libres>=8;
			WHEN 9 THEN
				SELECT l.Id FROM (
									select b.idtifx as Id,16-sum(a.2bd) as libres 
																								from (
																											(select idtifx as id,((count(*)DIV 2)*4) + ((count(*)%2)*4)as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=8 
																																group by idtifx) UNION
																										 	(select idtifx as id,count(*)*8 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=7 
																																group by idtifx) UNION
																											(select idtifx as id,count(*)*16 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=9
																																group by idtifx) UNION
																											(select idtifx as id,count(*) as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface in (0,1,2,3,4,5,6)
																																group by idtifx) UNION
																											(select idtifx as id,0 as 2BD from TIFX 
																													where IdSistema=id_sistema AND
																																IdTifx NOT IN (SELECT DISTINCT(IdTifx) FROM Recursos WHERE IdSistema=id_sistema))
																											) a, tifx b
													where b.IdSistema=id_sistema AND
							  								a.id=b.IdTIFX
													group by a.id
								) l
				WHERE l.libres>=16;
			ELSE
				SELECT l.Id FROM (
									select b.idtifx as Id,16-sum(a.2bd) as libres 
																								from (
																											(select idtifx as id,((count(*)DIV 2)*4) + ((count(*)%2)*4)as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=8 
																																group by idtifx) UNION
																										 	(select idtifx as id,count(*)*8 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=7 
																																group by idtifx) UNION
																											(select idtifx as id,count(*)*16 as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface=9
																																group by idtifx) UNION
																											(select idtifx as id,count(*) as 2BD from recursos 
																													where IdSistema=id_sistema AND
																																interface in (0,1,2,3,4,5,6)
																																group by idtifx) UNION
																											(select idtifx as id,0 as 2BD from TIFX 
																													where IdSistema=id_sistema AND
																																IdTifx NOT IN (SELECT DISTINCT(IdTifx) FROM Recursos WHERE IdSistema=id_sistema))
																											) a, tifx b
													where b.IdSistema=id_sistema AND
							  								a.id=b.IdTIFX
													group by a.id
								) l
				WHERE l.libres>=1;
	END CASE;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `TeclasSector`
--

DROP PROCEDURE IF EXISTS `TeclasSector`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `TeclasSector`(in id_sistema char(32), in id_sector char(32))
BEGIN
	SELECT ts.* FROM TeclasSector ts, SectoresSector ss
	WHERE ss.IdSistema=id_sistema AND
				ss.IdSector=id_sector AND
				ts.IdSistema=id_sistema AND
				ts.IdSector=ss.IdSectorOriginal AND
				ss.EsDominante;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `TeclasSectorParaXML`
--

DROP PROCEDURE IF EXISTS `TeclasSectorParaXML`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `TeclasSectorParaXML`(in id_sistema varchar(32), in id_sectorizacion varchar(32))
BEGIN
	IF (id_sectorizacion IS NOT NULL) THEN
		SELECT c.IdSector,ts.* FROM TeclasSector ts, SectoresSector ss, sectoressectorizacion c
			WHERE ss.IdSistema=id_sistema AND
						c.IdSectorizacion=id_sectorizacion AND
						c.IdSistema=ss.IdSistema AND
						ts.IdSistema=ss.IdSistema AND
						ts.IdNucleo=ss.IdNucleo AND
						ts.IdSector=ss.IdSectorOriginal AND
						c.IdSector=ss.IdSector AND
						ss.EsDominante;
	ELSE
		SELECT ts.* FROM TeclasSector ts, Sectores s
			WHERE	s.IdSistema=id_sistema AND
						s.SectorSimple AND
						ts.IdSistema=s.IdSistema AND
						ts.IdSector=s.IdSector AND
						ts.IdNucleo=s.IdNucleo;
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `TroncalesSinAsignarARutas`
--

DROP PROCEDURE IF EXISTS `TroncalesSinAsignarARutas`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `TroncalesSinAsignarARutas`(in id_Sistema char(32),in id_Central char(32), in id_Ruta char(32))
BEGIN
	IF (id_Ruta="") THEN
			
			SELECT * 
				FROM Troncales 
				WHERE IdSistema=id_Sistema AND 
							IdTroncal NOT IN (SELECT IdTroncal 
																	from TroncalesRuta Tr,Rutas r
																	where tr.IdSistema=id_Sistema AND
																				r.IdSistema=Tr.IdSistema AND
																				r.IdRuta=Tr.IdRuta AND
																				(r.Tipo='D' OR tr.Central=id_Central));
	ELSE
			
			SELECT * 
				FROM Troncales 
				WHERE IdSistema=id_Sistema AND 
							IdTroncal NOT IN (SELECT IdTroncal 
																	from TroncalesRuta
																	where IdSistema=id_Sistema AND
																				Central=id_Central);
	END IF;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `UsuariosImplicadosEnRecurso`
--

DROP PROCEDURE IF EXISTS `UsuariosImplicadosEnRecurso`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`%` PROCEDURE `UsuariosImplicadosEnRecurso`(in id_sistema char(32), in id_destino char(32))
BEGIN
	SELECT c.IdSectorOriginal FROM Externos a, SectoresSector c
	WHERE (a.IdSectorizacion = (SELECT IdSectorizacion FROM Sectorizaciones 
																		WHERE IdSistema=id_sistema AND
																					Activa=true)) AND
				a.IdSistema=id_sistema AND
				a.IdDestino=id_destino AND
				c.IdSistema=id_sistema AND
				c.IdSector=a.IdSector AND
				c.EsDominante;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;

--
-- Definition of procedure `VersionConfiguracion`
--

DROP PROCEDURE IF EXISTS `VersionConfiguracion`;

DELIMITER $$

/*!50003 SET @TEMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER' */ $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `VersionConfiguracion`(
  IN   `id_sistema`  varchar(32),
  OUT  `versionCfg`  text
)
BEGIN
	SELECT DATE_FORMAT(FechaActivacion,"%d/%m/%Y %H:%i:%s") INTO versionCfg FROM Sectorizaciones 
			WHERE Activa=true AND
						IdSistema=id_sistema;
END $$
/*!50003 SET SESSION SQL_MODE=@TEMP_SQL_MODE */  $$

DELIMITER ;
--
-- Create schema cd40
--

CREATE DATABASE IF NOT EXISTS cd40;
USE cd40;

--
-- Definition of view `sectoresentopsparainformexml`
--

DROP TABLE IF EXISTS `sectoresentopsparainformexml`;
DROP VIEW IF EXISTS `sectoresentopsparainformexml`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `sectoresentopsparainformexml` AS (select `a`.`IdSistema` AS `Idsistema`,`a`.`IdSectorizacion` AS `IdSectorizacion`,`a`.`IdTOP` AS `IdToP`,`a`.`IdSector` AS `IdSector`,`c`.`IdNucleo` AS `IdNucleo`,`c`.`IdSectorOriginal` AS `IdSectorOriginal` from (`sectoressectorizacion` `a` join `sectoressector` `c`) where ((`a`.`IdSistema` = `c`.`IdSistema`) and (`c`.`IdNucleo` = `a`.`IdNucleo`) and (`c`.`IdSector` = `a`.`IdSector`)) order by `c`.`IdSectorOriginal`) union (select `b`.`IdSistema` AS `IdSistema`,`b`.`IdSectorizacion` AS `IdSectorizacion`,`a`.`IdTOP` AS `IdTOP`,NULL AS `NULL`,NULL AS `NULL`,NULL AS `NULL` from (`top` `a` join `sectorizaciones` `b`) where ((`a`.`IdSistema` = `b`.`IdSistema`) and (not((`a`.`IdSistema`,`b`.`IdSectorizacion`,`a`.`IdTOP`) in (select `sectoressectorizacion`.`IdSistema` AS `IdSistema`,`sectoressectorizacion`.`IdSectorizacion` AS `idsectorizacion`,`sectoressectorizacion`.`IdTOP` AS `IdTOP` from `sectoressectorizacion`)))));

--
-- Definition of view `viewdestinostelefonia`
--

DROP TABLE IF EXISTS `viewdestinostelefonia`;
DROP VIEW IF EXISTS `viewdestinostelefonia`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `viewdestinostelefonia` AS (select `a`.`IdSistema` AS `IdSistema`,`a`.`IdDestino` AS `IdDestino`,`a`.`TipoDestino` AS `TipoDestino`,`a`.`IdGrupo` AS `IdGrupo`,`a`.`IdPrefijo` AS `IdPrefijo`,`b`.`IdAbonado` AS `IdAbonado`,NULL AS `IdRed` from (`destinostelefonia` `a` join `destinosexternos` `b`) where ((`a`.`IdSistema` = `b`.`IdSistema`) and (`a`.`IdDestino` = `b`.`IdDestino`) and (`a`.`TipoDestino` = `b`.`TipoDestino`) and (`a`.`IdPrefijo` in (0,1,2,32)) and (`a`.`IdPrefijo` = `b`.`IdPrefijo`))) union (select `a`.`IdSistema` AS `IdSistema`,`a`.`IdDestino` AS `IdDestino`,`a`.`TipoDestino` AS `TipoDestino`,`a`.`IdGrupo` AS `IdGrupo`,`a`.`IdPrefijo` AS `IdPrefijo`,`b`.`IdAbonado` AS `IdAbonado`,`c`.`IdRed` AS `IdRed` from ((`destinostelefonia` `a` join `destinosexternos` `b`) join `redes` `c`) where ((`a`.`IdSistema` = `b`.`IdSistema`) and (`a`.`IdDestino` = `b`.`IdDestino`) and (`a`.`TipoDestino` = `b`.`TipoDestino`) and (`a`.`IdPrefijo` = `b`.`IdPrefijo`) and (`c`.`IdSistema` = `b`.`IdSistema`) and (`c`.`IdPrefijo` = `b`.`IdPrefijo`)));

--
-- Definition of view `viewincidenciasmasalarma`
--

DROP TABLE IF EXISTS `viewincidenciasmasalarma`;
DROP VIEW IF EXISTS `viewincidenciasmasalarma`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `viewincidenciasmasalarma` AS select `s`.`IdSistema` AS `IdSistema`,`i`.`IdIncidencia` AS `IdIncidencia`,`i`.`Incidencia` AS `Incidencia`,(select `alarmas`.`Alarma` AS `Alarma` from `alarmas` where ((`alarmas`.`IdSistema` = `s`.`IdSistema`) and (`alarmas`.`IdIncidencia` = `i`.`IdIncidencia`))) AS `alarma` from (`incidencias` `i` join `sistema` `s`) order by `s`.`IdSistema`,`i`.`Incidencia`;

--
-- Definition of view `viewincidenciasmasalarma_ingles`
--

DROP TABLE IF EXISTS `viewincidenciasmasalarma_ingles`;
DROP VIEW IF EXISTS `viewincidenciasmasalarma_ingles`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `viewincidenciasmasalarma_ingles` AS select `s`.`IdSistema` AS `IdSistema`,`i`.`IdIncidencia` AS `IdIncidencia`,`i`.`Incidencia` AS `Incidencia`,(select `alarmas`.`Alarma` AS `Alarma` from `alarmas` where ((`alarmas`.`IdSistema` = `s`.`IdSistema`) and (`alarmas`.`IdIncidencia` = `i`.`IdIncidencia`))) AS `alarma` from (`incidencias_ingles` `i` join `sistema` `s`) order by `s`.`IdSistema`,`i`.`Incidencia`;

--
-- Definition of view `viewrecursosimplicadosrutas`
--

DROP TABLE IF EXISTS `viewrecursosimplicadosrutas`;
DROP VIEW IF EXISTS `viewrecursosimplicadosrutas`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `viewrecursosimplicadosrutas` AS (select `rt`.`IdSistema` AS `idsistema`,`rt`.`IdRecurso` AS `idrecurso`,`rt`.`IdDestino` AS `iddestino`,`rt`.`IdRed` AS `idred`,`rt`.`IdTroncal` AS `idtroncal`,`r`.`IdRuta` AS `idruta`,`r`.`Tipo` AS `tipo` from ((`recursostf` `rt` join `troncalesruta` `tr`) join `rutas` `r`) where ((`tr`.`IdSistema` = `rt`.`IdSistema`) and (`r`.`IdSistema` = `tr`.`IdSistema`) and (`rt`.`IdTroncal` is not null) and (`rt`.`IdTroncal` = `tr`.`IdTroncal`) and (`tr`.`IdRuta` = `r`.`IdRuta`)) order by `rt`.`IdSistema`,`rt`.`IdRecurso`) union (select `rt`.`IdSistema` AS `idsistema`,`rt`.`IdRecurso` AS `idrecurso`,`rt`.`IdDestino` AS `IdDestino`,`rt`.`IdRed` AS `idred`,NULL AS `idtroncal`,NULL AS `idruta`,NULL AS `tipo` from `recursostf` `rt` where isnull(`rt`.`IdTroncal`) order by `rt`.`IdSistema`,`rt`.`IdRecurso`);

--
-- Definition of view `viewsectoresenagrupacion`
--

DROP TABLE IF EXISTS `viewsectoresenagrupacion`;
DROP VIEW IF EXISTS `viewsectoresenagrupacion`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `viewsectoresenagrupacion` AS select count(0) AS `contador`,`sectoresagrupacion`.`IdAgrupacion` AS `IdAgrupacion` from `sectoresagrupacion` group by `sectoresagrupacion`.`IdAgrupacion`;

--
-- Definition of view `viewsectoresentops`
--

DROP TABLE IF EXISTS `viewsectoresentops`;
DROP VIEW IF EXISTS `viewsectoresentops`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `viewsectoresentops` AS (select `sectoressectorizacion`.`IdSistema` AS `IdSistema`,`sectoressectorizacion`.`IdSectorizacion` AS `idsectorizacion`,`sectoressectorizacion`.`IdTOP` AS `IdTOP`,`sectoressectorizacion`.`IdSector` AS `idsector`,`sectoressectorizacion`.`IdNucleo` AS `idnucleo` from `sectoressectorizacion` where (`sectoressectorizacion`.`IdSistema`,`sectoressectorizacion`.`IdSectorizacion`,`sectoressectorizacion`.`IdTOP`) in (select `a`.`IdSistema` AS `IdSistema`,`b`.`IdSectorizacion` AS `idsectorizacion`,`a`.`IdTOP` AS `IdTOP` from (`top` `a` join `sectorizaciones` `b`) where (`a`.`IdSistema` = `b`.`IdSistema`))) union (select `a`.`IdSistema` AS `IdSistema`,`b`.`IdSectorizacion` AS `idsectorizacion`,`a`.`IdTOP` AS `IdTOP`,NULL AS `NULL`,NULL AS `NULL` from (`top` `a` join `sectorizaciones` `b`) where ((`a`.`IdSistema` = `b`.`IdSistema`) and (not((`a`.`IdSistema`,`b`.`IdSectorizacion`,`a`.`IdTOP`) in (select `sectoressectorizacion`.`IdSistema` AS `IdSistema`,`sectoressectorizacion`.`IdSectorizacion` AS `idsectorizacion`,`sectoressectorizacion`.`IdTOP` AS `IdTOP` from `sectoressectorizacion`)))));
--
-- Create schema cd40_trans
--

CREATE DATABASE IF NOT EXISTS cd40_trans;
USE cd40_trans;

--
-- Definition of view `sectoresentopsparainformexml`
--

DROP TABLE IF EXISTS `sectoresentopsparainformexml`;
DROP VIEW IF EXISTS `sectoresentopsparainformexml`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `sectoresentopsparainformexml` AS (select `a`.`IdSistema` AS `Idsistema`,`a`.`IdSectorizacion` AS `IdSectorizacion`,`a`.`IdTOP` AS `IdToP`,`a`.`IdSector` AS `IdSector`,`c`.`IdNucleo` AS `IdNucleo`,`c`.`IdSectorOriginal` AS `IdSectorOriginal` from (`sectoressectorizacion` `a` join `sectoressector` `c`) where ((`a`.`IdSistema` = `c`.`IdSistema`) and (`c`.`IdNucleo` = `a`.`IdNucleo`) and (`c`.`IdSector` = `a`.`IdSector`)) order by `c`.`IdSectorOriginal`) union (select `b`.`IdSistema` AS `IdSistema`,`b`.`IdSectorizacion` AS `IdSectorizacion`,`a`.`IdTOP` AS `IdTOP`,NULL AS `NULL`,NULL AS `NULL`,NULL AS `NULL` from (`top` `a` join `sectorizaciones` `b`) where ((`a`.`IdSistema` = `b`.`IdSistema`) and (not((`a`.`IdSistema`,`b`.`IdSectorizacion`,`a`.`IdTOP`) in (select `sectoressectorizacion`.`IdSistema` AS `IdSistema`,`sectoressectorizacion`.`IdSectorizacion` AS `idsectorizacion`,`sectoressectorizacion`.`IdTOP` AS `IdTOP` from `sectoressectorizacion`)))));

--
-- Definition of view `viewdestinostelefonia`
--

DROP TABLE IF EXISTS `viewdestinostelefonia`;
DROP VIEW IF EXISTS `viewdestinostelefonia`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `viewdestinostelefonia` AS (select `a`.`IdSistema` AS `IdSistema`,`a`.`IdDestino` AS `IdDestino`,`a`.`TipoDestino` AS `TipoDestino`,`a`.`IdGrupo` AS `IdGrupo`,`a`.`IdPrefijo` AS `IdPrefijo`,`b`.`IdAbonado` AS `IdAbonado`,NULL AS `IdRed` from (`destinostelefonia` `a` join `destinosexternos` `b`) where ((`a`.`IdSistema` = `b`.`IdSistema`) and (`a`.`IdDestino` = `b`.`IdDestino`) and (`a`.`TipoDestino` = `b`.`TipoDestino`) and (`a`.`IdPrefijo` in (0,1,2,32)) and (`a`.`IdPrefijo` = `b`.`IdPrefijo`))) union (select `a`.`IdSistema` AS `IdSistema`,`a`.`IdDestino` AS `IdDestino`,`a`.`TipoDestino` AS `TipoDestino`,`a`.`IdGrupo` AS `IdGrupo`,`a`.`IdPrefijo` AS `IdPrefijo`,`b`.`IdAbonado` AS `IdAbonado`,`c`.`IdRed` AS `IdRed` from ((`destinostelefonia` `a` join `destinosexternos` `b`) join `redes` `c`) where ((`a`.`IdSistema` = `b`.`IdSistema`) and (`a`.`IdDestino` = `b`.`IdDestino`) and (`a`.`TipoDestino` = `b`.`TipoDestino`) and (`a`.`IdPrefijo` = `b`.`IdPrefijo`) and (`c`.`IdSistema` = `b`.`IdSistema`) and (`c`.`IdPrefijo` = `b`.`IdPrefijo`)));

--
-- Definition of view `viewincidenciasmasalarma`
--

DROP TABLE IF EXISTS `viewincidenciasmasalarma`;
DROP VIEW IF EXISTS `viewincidenciasmasalarma`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `viewincidenciasmasalarma` AS select `s`.`IdSistema` AS `IdSistema`,`i`.`IdIncidencia` AS `IdIncidencia`,`i`.`Incidencia` AS `Incidencia`,(select `alarmas`.`Alarma` AS `Alarma` from `alarmas` where ((`alarmas`.`IdSistema` = `s`.`IdSistema`) and (`alarmas`.`IdIncidencia` = `i`.`IdIncidencia`))) AS `alarma` from (`incidencias` `i` join `sistema` `s`) order by `s`.`IdSistema`,`i`.`Incidencia`;

--
-- Definition of view `viewincidenciasmasalarma_ingles`
--

DROP TABLE IF EXISTS `viewincidenciasmasalarma_ingles`;
DROP VIEW IF EXISTS `viewincidenciasmasalarma_ingles`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `viewincidenciasmasalarma_ingles` AS select `s`.`IdSistema` AS `IdSistema`,`i`.`IdIncidencia` AS `IdIncidencia`,`i`.`Incidencia` AS `Incidencia`,(select `alarmas`.`Alarma` AS `Alarma` from `alarmas` where ((`alarmas`.`IdSistema` = `s`.`IdSistema`) and (`alarmas`.`IdIncidencia` = `i`.`IdIncidencia`))) AS `alarma` from (`incidencias_ingles` `i` join `sistema` `s`) order by `s`.`IdSistema`,`i`.`Incidencia`;

--
-- Definition of view `viewrecursosimplicadosrutas`
--

DROP TABLE IF EXISTS `viewrecursosimplicadosrutas`;
DROP VIEW IF EXISTS `viewrecursosimplicadosrutas`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `viewrecursosimplicadosrutas` AS (select `rt`.`IdSistema` AS `idsistema`,`rt`.`IdRecurso` AS `idrecurso`,`rt`.`IdDestino` AS `iddestino`,`rt`.`IdRed` AS `idred`,`rt`.`IdTroncal` AS `idtroncal`,`r`.`IdRuta` AS `idruta`,`r`.`Tipo` AS `tipo` from ((`recursostf` `rt` join `troncalesruta` `tr`) join `rutas` `r`) where ((`tr`.`IdSistema` = `rt`.`IdSistema`) and (`r`.`IdSistema` = `tr`.`IdSistema`) and (`rt`.`IdTroncal` is not null) and (`rt`.`IdTroncal` = `tr`.`IdTroncal`) and (`tr`.`IdRuta` = `r`.`IdRuta`)) order by `rt`.`IdSistema`,`rt`.`IdRecurso`) union (select `rt`.`IdSistema` AS `idsistema`,`rt`.`IdRecurso` AS `idrecurso`,`rt`.`IdDestino` AS `IdDestino`,`rt`.`IdRed` AS `idred`,NULL AS `idtroncal`,NULL AS `idruta`,NULL AS `tipo` from `recursostf` `rt` where isnull(`rt`.`IdTroncal`) order by `rt`.`IdSistema`,`rt`.`IdRecurso`);

--
-- Definition of view `viewsectoresenagrupacion`
--

DROP TABLE IF EXISTS `viewsectoresenagrupacion`;
DROP VIEW IF EXISTS `viewsectoresenagrupacion`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `viewsectoresenagrupacion` AS select count(0) AS `contador`,`sectoresagrupacion`.`IdAgrupacion` AS `IdAgrupacion` from `sectoresagrupacion` group by `sectoresagrupacion`.`IdAgrupacion`;

--
-- Definition of view `viewsectoresentops`
--

DROP TABLE IF EXISTS `viewsectoresentops`;
DROP VIEW IF EXISTS `viewsectoresentops`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `viewsectoresentops` AS (select `sectoressectorizacion`.`IdSistema` AS `IdSistema`,`sectoressectorizacion`.`IdSectorizacion` AS `idsectorizacion`,`sectoressectorizacion`.`IdTOP` AS `IdTOP`,`sectoressectorizacion`.`IdSector` AS `idsector`,`sectoressectorizacion`.`IdNucleo` AS `idnucleo` from `sectoressectorizacion` where (`sectoressectorizacion`.`IdSistema`,`sectoressectorizacion`.`IdSectorizacion`,`sectoressectorizacion`.`IdTOP`) in (select `a`.`IdSistema` AS `IdSistema`,`b`.`IdSectorizacion` AS `idsectorizacion`,`a`.`IdTOP` AS `IdTOP` from (`top` `a` join `sectorizaciones` `b`) where (`a`.`IdSistema` = `b`.`IdSistema`))) union (select `a`.`IdSistema` AS `IdSistema`,`b`.`IdSectorizacion` AS `idsectorizacion`,`a`.`IdTOP` AS `IdTOP`,NULL AS `NULL`,NULL AS `NULL` from (`top` `a` join `sectorizaciones` `b`) where ((`a`.`IdSistema` = `b`.`IdSistema`) and (not((`a`.`IdSistema`,`b`.`IdSectorizacion`,`a`.`IdTOP`) in (select `sectoressectorizacion`.`IdSistema` AS `IdSistema`,`sectoressectorizacion`.`IdSectorizacion` AS `idsectorizacion`,`sectoressectorizacion`.`IdTOP` AS `IdTOP` from `sectoressectorizacion`)))));



/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
