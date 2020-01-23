
@cls
@echo ********************************************************************
@echo ******* Comprobar que el archivo replication_server_step_2.sql
@echo ******* la línea: set @ip='<Dirección_IP_servidor_remoto>' contiene la dirección IP
@echo ******* del otro servidor
@echo ********************************************************************
@pause

@cls
@echo **************************************
@echo ******* REPLICACION SERVIDOR   *******
@echo *******        FASE 1          *******
@echo **************************************
"%PROGRAMFILES%\MySQL\MySQL Server 5.6\bin\mysql" -uroot -pcd40 -f -n < ".\replication_server_step_1.sql" > replica.log
"%PROGRAMFILES%\MySQL\MySQL Server 5.6\bin\mysql" -uroot -pcd40 -f -n < ".\replication_server_create_db.sql" >> replica.log

@pause
@cls
@echo *********************************************************************
@echo ***** A CONTINUACION SE VAN A CREAR LAS TABLAS DE ULISES      *******
@echo *********************************************************************
@pause
"%PROGRAMFILES%\MySQL\MySQL Server 5.6\bin\mysql" -uroot -pcd40 -f < ".\BDUlises.sql" >> replica.log
"%PROGRAMFILES%\MySQL\MySQL Server 5.6\bin\mysql" -uroot -pcd40 --database new_cd40 < ".\BDCD40_Incidencias.sql" >> replica.log
"%PROGRAMFILES%\MySQL\MySQL Server 5.6\bin\mysql" -uroot -pcd40 --database new_cd40_trans < ".\BDCD40_trans_Incidencias.sql" >> replica.log
"%PROGRAMFILES%\MySQL\MySQL Server 5.6\bin\mysql" -uroot -pcd40 -f < ".\datos_de_inicio.sql" >> replica.log
"%PROGRAMFILES%\MySQL\MySQL Server 5.6\bin\mysql" -uroot -pcd40 -f < ".\replication_server_step_2.sql" >> replica.log

@pause
@cls
@echo *********************************************************************
@echo ***** 			FIN REPLICACION	SERVIDOR FASE 1     *******
@echo *********************************************************************
@echo *********************************************************************
@echo *****    CAMBIA DE SERVIDOR Y PULSA (R) PARA REPLICAR         *******
@echo ***** NO PULSES UNA TECLA HASTA QUE NO FINALICE LA FASE 1     *******
@echo ***** 		DE REPLICACION EN DICHO SERVIDOR            *******
@echo *********************************************************************
@pause
@choice /C sn /M "¿Finalizo la FASE 1 de la replicacion en el otro servidor?"
@if errorlevel 2 goto FIN

@cls
@echo **************************************
@echo ******* REPLICACION SERVIDOR   *******
@echo *******        FASE 2          *******
@echo **************************************
"%PROGRAMFILES%\MySQL\MySQL Server 5.6\bin\mysql" -uroot -pcd40 -f < ".\replication_server_start_slave.sql" >> replica.log
@echo *********************************************************************
@echo ***** 			FIN REPLICACION	SERVIDOR FASE 2     *******
@echo *********************************************************************
@pause

:fin
