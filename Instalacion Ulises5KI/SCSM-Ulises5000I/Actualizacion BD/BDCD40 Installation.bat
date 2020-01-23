
@echo off
@SET PATH=%PATH%;%PROGRAMFILES%\MySQL\MySQL Server 5.6\bin

:menu
cls
echo **************************************************************
echo **** Utilidad de instalación base de datos Ulises 5000-I  ****
echo **************************************************************
echo.
echo.
echo Seleccione que desea instalar
echo.
echo Crear base de datos (1 SERVIDOR)(C)
echo Configurar Replicacion (CLUSTER)(R)
echo Actualizar datos		(A)
echo Crear usuario defecto		(U)
echo Crear datos de inicio		(D)
echo Salir				(S)
echo.
choice /C CRAUDS /M "Pulse (C), (R), (A), (U), (D) o (S)" 
SET opcion=%errorlevel%

if errorlevel 6 goto end
if errorlevel 5 goto datos
if errorlevel 4 goto usuario
if errorlevel 3 goto incidencias
if errorlevel 2 goto replica
if errorlevel 1 goto crear
goto menu

:crear
@echo Instalando base de datos para Ulises5000I-Configuración y Supervisión...
@mysql -uroot -pcd40 < ".\BDUlises.sql" > crear.log
pause

:incidencias
@choice /C sn /M "Desea importar datos de tablas afectadas?"
@if errorlevel 2 goto decideIncidencias
@if errorlevel 1 goto importar

: decideIncidencias
if %opcion% neq 1 goto menu

:importar
@echo Importando datos de tablas afectadas
@mysql -uroot -pcd40  < ".\ActualizaDatos-254.sql" >> crear.log
pause
if %opcion% neq 1 goto menu

:usuario
@choice /C sn /M "Desea crear usuario root@%?"
@if errorlevel 2 goto decideUsuario
@if errorlevel 1 goto crearusuario

:decideUsuario
if %opcion% neq 1 goto menu
goto datos

:crearusuario
@echo Creando usuario root@%
@mysql -uroot -pcd40 < ".\BDCD40_CrearUsuario.sql" >> crear.log
pause
if %opcion% neq 1 goto menu


:datos
@choice /C sn /M "Desea generar datos de inicio para las tablas de Sistema, Prefijos y Redes (Se eliminarán los registros creados)?"
@if errorlevel 2 goto menu

@mysql -uroot -pcd40 < ".\datos_de_inicio.sql" >> crear.log
pause
goto menu

:replica
rem call "replica.bat"
echo Configurando la replicación 
call "restaura_replica.bat"
goto menu

:end
@echo **** Fin de la Instalación Base de Datos
@echo Pulse una tecla.
@pause
