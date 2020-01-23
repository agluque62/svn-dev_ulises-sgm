
@echo off


@REM Obtener el path desde donde se está ejecutando el Ulises5000I_Install.bat
set mi_path=%~d0%~p0

cls
echo **************************************************************
echo **** Utilidad de instalación Servicios Web Ulises 5000-I  ****
echo **************************************************************
REM Iniciar servicio Web Deploy
echo *** Iniciando servicio de instalacion web **
net start "MsDepSvc"

:menu
cls
echo **************************************************************
echo **** Utilidad de instalación Servicios Web Ulises 5000-I  ****
echo **************************************************************
echo.
echo.
echo Seleccione los servicios web instalar
echo Configuracion		(C)
echo Base de Datos 		(B)
echo Software cluster 	(T)
echo Salir			(S)
echo.
choice /C CBTS /M "Pulse (C), (B), (T) o (S)" 
SET opcion=%errorlevel%

if errorlevel 4 goto end
if errorlevel 3 goto cluster
if errorlevel 2 goto bd
if errorlevel 1 goto configuracion
goto menu

:configuracion

echo **** Parando servicio IIS ******
iisreset /STOP

REM Crear directorio virtual "NucleoDF"
echo **************************************************************
echo **** Crear directorio virtual "NucleoDF"
echo **************************************************************
%windir%\System32\inetsrv\appcmd add app /site.name:"Default Web Site" /path:/NucleoDF /physicalPath:C:\inetpub\wwwroot\NucleoDF
%windir%\System32\inetsrv\appcmd set app /app.name: "Default Web Site/NucleoDF" /applicationPool:"ASP.NET v4.0"
%windir%\System32\inetsrv\appcmd set apppool "DefaultAppPool" /startMode:AlwaysRunning
%windir%\System32\inetsrv\appcmd set site "Default Web Site" /serverAutoStart:true

%windir%\System32\inetsrv\appcmd set app "Default Web Site/NucleoDF" /serviceAutoStartEnabled:true
%windir%\System32\inetsrv\appcmd set app "Default Web Site/NucleoDF/U5kCfg" /serviceAutoStartEnabled:true
%windir%\System32\inetsrv\appcmd set app "Default Web Site/NucleoDF/U5kCfg/InterfazSacta" /serviceAutoStartEnabled:true
%windir%\System32\inetsrv\appcmd set app "Default Web Site/NucleoDF/U5kCfg/InterfazSOAPConfiguracion" /serviceAutoStartEnabled:true
%windir%\System32\inetsrv\appcmd set app "Default Web Site/NucleoDF/U5kCfg/Servicios" /serviceAutoStartEnabled:true

cls
echo  ¿Desea hacer backup de los ficheros de configuración web.config?
choice /C SN /M "Pulse (S), (N)" 
SET backup=%errorlevel%

if errorlevel 2 goto seguir
echo **** Backup Web.config
md c:\tmpUlises\web
md c:\tmpUlises\servicios
md c:\tmpUlises\isc
md c:\tmpUlises\mantto
md c:\tmpUlises\sacta
rem md c:\tmpUlises\estadis
move /Y C:\inetpub\wwwroot\NucleoDF\U5kCfg\Web.config c:\tmpUlises\web
move /Y C:\inetpub\wwwroot\NucleoDF\U5kCfg\Servicios\Web.config c:\tmpUlises\servicios
move /Y C:\inetpub\wwwroot\NucleoDF\U5kCfg\InterfazSOAPConfiguracion\Web.config c:\tmpUlises\isc
move /Y C:\inetpub\wwwroot\NucleoDF\U5kCfg\GestorMantenimiento\Web.config c:\tmpUlises\mantto
move /Y C:\inetpub\wwwroot\NucleoDF\U5kCfg\InterfazSacta\Web.config c:\tmpUlises\sacta

:seguir
cls
echo **** Instalando Servicio Web SCSM-Ulises5000
cd %mi_path%
cd Web
call Web.deploy.cmd /y /m:localhost

pause

echo **** Instalando Servicio Web GestorMantenimiento
cd %mi_path%
cd GestorMantenimiento
call GestorMantenimiento.deploy.cmd /y /m:localhost

echo **** Instalando Servicio Web InterfazSacta
cd %mi_path%
cd InterfazSacta
call InterfazSacta.deploy.cmd /y /m:localhost

pause

echo **** Instalando Servicio Web InterfazSOAPConfiguracion
cd %mi_path%
cd InterfazSOAPConfiguracion
call InterfazSOAPConfiguracion.deploy.cmd /y /m:localhost

pause

echo **** Instalando Servicio Web Servicios
cd %mi_path%
cd Servicios
call Servicios.deploy.cmd /y /m:localhost

pause

if %backup% == 2 goto cambiarpool
echo **** Restore Web.config
move /Y c:\tmpUlises\web\Web.config C:\inetpub\wwwroot\NucleoDF\U5kCfg
move /Y c:\tmpUlises\servicios\Web.config C:\inetpub\wwwroot\NucleoDF\U5kCfg\Servicios
move /Y c:\tmpUlises\isc\Web.config C:\inetpub\wwwroot\NucleoDF\U5kCfg\InterfazSOAPConfiguracion
move /Y c:\tmpUlises\mantto\Web.config C:\inetpub\wwwroot\NucleoDF\U5kCfg\GestorMantenimiento
move /Y c:\tmpUlises\sacta\Web.config C:\inetpub\wwwroot\NucleoDF\U5kCfg\InterfazSacta
rmdir /Q /S c:\tmpUlises

:cambiarpool
REM Cambiar el application pool de NucleoDF ASP.NET v4.0
echo **************************************************************
echo **** Cambiar el application pool de NucleoDF ASP.NET v4.0
echo **************************************************************
%windir%\System32\inetsrv\appcmd set app /app.name: "Default Web Site/NucleoDF/U5kCfg" /applicationPool:"ASP.NET v4.0"
rem %windir%\System32\inetsrv\appcmd set app /app.name: "Default Web Site/NucleoDF/U5kCfg/Estadisticas" /applicationPool:"ASP.NET v4.0"
%windir%\System32\inetsrv\appcmd set app /app.name: "Default Web Site/NucleoDF/U5kCfg/GestorMantenimiento" /applicationPool:"ASP.NET v4.0"
%windir%\System32\inetsrv\appcmd set app /app.name: "Default Web Site/NucleoDF/U5kCfg/InterfazSacta" /applicationPool:"ASP.NET v4.0"
%windir%\System32\inetsrv\appcmd set app /app.name: "Default Web Site/NucleoDF/U5kCfg/InterfazSOAPConfiguracion" /applicationPool:"ASP.NET v4.0"
%windir%\System32\inetsrv\appcmd set app /app.name: "Default Web Site/NucleoDF/U5kCfg/Servicios" /applicationPool:"ASP.NET v4.0"

REM Instalar claves de registro
cls
echo **************************************************************
echo **** Instalar claves de registro
echo **************************************************************
cd \inetpub\wwwroot\NucleoDF\U5kCfg\Servicios
trans.reg

:fin
cls
echo **** Iniciando servicio IIS ******
iisreset /START


echo **** Fin de la Instalación Servicios Web Ulises 5000-I
echo Pulse una tecla para finalizar
pause
goto menu

:bd
REM Instalar base de datos Ulises
cls
echo **************************************************************
echo **** Crear base de datos Ulises
echo **************************************************************
cls
echo  ¿Desea crear o actualizar la base de datos Ulises?
choice /C SN /M "Pulse (S), (N)" 

if errorlevel 2 goto end
cd %mi_path%
cd "Actualizacion BD"
call "BDCD40 Installation.bat"
goto menu

:cluster
cd %mi_path%
call "InstallClusterServer_X86.bat"
goto menu

:end

cd %mi_path%
