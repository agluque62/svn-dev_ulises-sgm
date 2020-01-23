ALTER TABLE `new_cd40`.`hfparams` 
ADD COLUMN `PrioridadEquipo` INT(11) NULL DEFAULT '0' COMMENT '0: Baja\n1: Normal\n2: Alta' AFTER `TipoModo`;

ALTER TABLE `new_cd40_trans`.`hfparams` 
ADD COLUMN `PrioridadEquipo` INT(11) NULL DEFAULT '0' COMMENT '0: Baja\n1: Normal\n2: Alta' AFTER `TipoModo`;
