echo off
@cls
@echo **************************************************
@echo ***** INSTALANDO SERVICIO CLUSTER ****************
@echo **************************************************
@echo ¿Desea instalar los servicios de cluster?
@choice /C SN /M "Pulse (S), (N)" 

@if errorlevel 2 goto end
@set mi_path=%~d0%~p0
@cd %mi_path%

echo  ¿Desea hacer backup de los ficheros de configuración?
choice /C SN /M "Pulse (S), (N)" 
SET backup=%errorlevel%

if errorlevel 2 goto seguir
echo **** Backup config
md c:\tmpUlises\cluster
move /Y "%PROGRAMFILES%\DF Nucleo\UlisesV5000Cluster\*.config" c:\tmpUlises\cluster
pause

IF /i NOT EXIST "%PROGRAMFILES%\DF Nucleo\UlisesV5000Cluster" GOTO seguir

:: Se comprueba si el servicio existe
ECHO Se comprueba si el servicio ClusterSrv existe
sc query ClusterSrv 

IF %ERRORLEVEL% EQU 0 (
ECHO El servicio ClusterSrv existe. Se procede a su eliminacion
"%PROGRAMFILES%\DF Nucleo\UlisesV5000Cluster\ClusterSrv" -uninstall
ECHO.
ECHO El servicio ClusterSrv ha sido eliminado
)

:seguir
@xcopy /EIY ".\Cluster\Servidor" "%PROGRAMFILES%\DF Nucleo\UlisesV5000Cluster"
@cd "%PROGRAMFILES%\DF Nucleo\UlisesV5000Cluster"
ClusterSrv -install

IF %ERRORLEVEL% EQU 0 (
:: Se configura el servicio ClusterSrv con inicio automático diferido
ECHO - Se configura el servicio ClusterSrv con inicio automatico diferido 
sc config ClusterSrv  start= delayed-auto
)

pause

@cd %mi_path%

if %backup% == 2 goto registro

echo **** Restore config
move /Y "c:\tmpUlises\cluster\*.config" "%PROGRAMFILES%\DF Nucleo\UlisesV5000Cluster"
rmdir /Q /S c:\tmpUlises

:registro
REM Instalar claves de registro
::cls
ECHO.
echo **************************************************************
echo **** Instalar claves de registro
echo **************************************************************
clusterSrv_X86.reg

pause

:end
@echo **** Fin de la instalación de los servicios de cluster
@echo Pulse una tecla para finalizar
@pause
