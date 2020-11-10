@echo off


@REM Obtener el path desde donde se está ejecutando el Ulises5000I_Install.bat
set mi_path=%~d0%~p0
echo *******************************************************************************
echo ****     Utilidad de copia Web.config version Desarrollo / Distribucion    **** 
echo ****                     Ulises  V5000-I                                   ****
echo **** Para realizar la copia de los ficheros Web.config es necesario cerrar ****
echo **** proyecto de CD40 en Visual Studio                                     ****
echo *******************************************************************************

echo Seleccione la opcion a instalar
echo Desarrollo		(D)
echo Distribucion 		(P)
echo Salir			(S)
echo.
choice /C DPS /M "Pulse (D), (P) o (S)"
SET opcion=%errorlevel%
if errorlevel 3 goto end
if errorlevel 2 goto distribucion
if errorlevel 1 goto desarrollo

goto menu

:desarrollo
echo  Desea copiar Webs.config para desarrollo?
choice /C SN /M "Pulse (S), (N)" 
if errorlevel 2 goto end

copy /Y .\Development\Web\Web.config ..\Web
copy /Y .\Development\Servicios\Web.config ..\Servicios
copy /Y .\Development\InterfazSOAPConfiguracion\Web.config ..\InterfazSOAPConfiguracion
copy /Y .\Development\InterfazSacta\Web.config ..\InterfazSacta
echo **** Fin de la copia de ficheros Web Ulises 5000-I para desarrollo
echo Pulse una tecla para finalizar
pause
goto end

:distribucion
echo  Desea copiar Webs.config para distribucion?
choice /C SN /M "Pulse (S), (N)" 
if errorlevel 2 goto end

copy /Y .\Deploy\Web\Web.config ..\Web
copy /Y .\Deploy\Servicios\Web.config ..\Servicios
copy /Y .\Deploy\InterfazSOAPConfiguracion\Web.config ..\InterfazSOAPConfiguracion
copy /Y .\Deploy\InterfazSacta\Web.config ..\InterfazSacta
echo **** Fin de la copia de ficheros Web Ulises 5000-I para distribucion
echo Pulse una tecla para finalizar
pause
goto end

:end
