
echo off
@cls
@echo *********************************************************************
@echo ***** INSTALANDO SERVICIO SUPERVISION Y MANTENIMIENTO ***************
@echo *********************************************************************
@echo ¿Desea instalar los servicios de supervisión y mantenimiento?
@choice /C SN /M "Pulse (S), (N)" 

@if errorlevel 2 goto end
@set mi_path=%~d0%~p0
@cd %mi_path%

echo  ¿Desea hacer backup de los ficheros de configuración?
choice /C SN /M "Pulse (S), (N)" 
SET backup=%errorlevel%
if errorlevel 2 goto seguir
echo **** Backup config
md c:\tmpUlises\srvMantto
move /Y "%PROGRAMFILES(X86)%\DF Nucleo\UlisesV5000SSM\*.config" c:\tmpUlises\srvMantto
pause


:seguir
%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\installutil /u "%PROGRAMFILES(X86)%\DF Nucleo\UlisesV5000SSM\U5kManServer.exe"

xcopy /EIY .\u5kman-server "%PROGRAMFILES(X86)%\DF Nucleo\UlisesV5000SSM"
rem @cd "%PROGRAMFILES(X86)%\DF Nucleo\UlisesV5000SSM"
@cd %mi_path%


if %backup% == 2 goto registro

echo **** Restore config
move /Y c:\tmpUlises\srvMantto\*.config "%PROGRAMFILES(X86)%\DF Nucleo\UlisesV5000SSM"
rmdir /Q /S c:\tmpUlises

%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\installutil "%PROGRAMFILES(X86)%\DF Nucleo\UlisesV5000SSM\U5kManServer.exe"

:registro
echo **************************************************************
echo **** Instalar claves de registro
echo **************************************************************
u5kman.reg

:end
@echo **** Fin de la instalación de los servicios de supervisión y mantenimiento
@echo Pulse una tecla para finalizar
@pause
