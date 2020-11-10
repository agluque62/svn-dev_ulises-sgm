ALTER TABLE `new_cd40`.`sectores` 
ADD COLUMN `SeleccionadoFS` TINYINT(1) NULL DEFAULT 0 COMMENT 'Modificación para marca de sector seleccionado modelo Fuera Sectorización' AFTER `NumSacta`;
USE `new_cd40_trans`;
DROP procedure IF EXISTS `GestionaFS`;

DELIMITER $$
USE `new_cd40`$$
CREATE PROCEDURE `GestionaFS` (in id_sistema char(32), in id_nucleo char(32), in id_sector char(32), in SeleccionadoFS bool)
BEGIN
IF(SeleccionadoFS = true) THEN
	UPDATE Sectores 
		SET SeleccionadoFS=false;
		REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('Sectores');
                END IF;
END$$

DELIMITER ;


ALTER TABLE `new_cd40_trans`.`sectores` 
ADD COLUMN `SeleccionadoFS` TINYINT(1) NULL DEFAULT 0 COMMENT 'Modificación para marca de sector seleccionado modelo Fuera Sectorización' AFTER `NumSacta`;

USE `new_cd40_trans`;
DROP procedure IF EXISTS `GestionaFS`;

DELIMITER $$
USE `new_cd40_trans`$$
CREATE PROCEDURE `GestionaFS` (in id_sistema char(32), in id_nucleo char(32), in id_sector char(32), in SeleccionadoFS bool)
BEGIN
IF(SeleccionadoFS = true) THEN
	UPDATE Sectores 
		SET SeleccionadoFS=false;
		REPLACE INTO TablasModificadas (IdTabla)
				VALUES ('Sectores');
                END IF;
END$$

DELIMITER ;