@ECHO OFF
:: Sincroniza la lista de tablas indicadas, de la base de datos destino con las de la base de datos origen

::Parametros
:: %1:  usuario de conexion a la BD
:: %2:  Contraseña del usuario de conexion a la BD
:: %3:  Nombre del host
:: %4:  nombre de la Base de datos origen
:: %5:  nombre de la Base de datos destino
:: %6:  lista de tablas a actualizar


SET BD_ORIGEN=%4

SET BD_DESTINO=%5

:: Obtenemos el nombre del fichero donde se guardarán los triggers originales de la BD destino
:: SET FICHERO_TRG_DESTBD_BCK=trigger_%BD_DESTINO%.sql
SET FICHERO_TRG_DESTBD_BCK= %~dp0trigger_new_cd40_trans_orig.sql
SET FICHERO_LOG=%~dp0Trans.log


:: Si los cambios se aplican desde:
::  la BD de configuracion a la BD de explotacion, la BD_ORIGEN=cd40_trans y la BD_DESTINO=cd40 no se deben importar los triggers.
::  la BD de explotación a la de configuración (restaura con la configuración original) se deben exportar los triggers a fichero 
::                                                       para volver a importarlos despues de haber restaurado los datos, para que no se pierdan los triggers de la BD de configuracion

IF /i "%BD_DESTINO:trans=%" == "%BD_DESTINO%" (SET RESTAURAR_CONFIGURACION=N) ELSE (SET RESTAURAR_CONFIGURACION=S)

IF "%RESTAURAR_CONFIGURACION%"=="S" (
  ECHO %DATE% %TIME%- Restaurar configuracion: %RESTAURAR_CONFIGURACION%  >>%FICHERO_LOG% 
  ECHO Se aplican los cambios en la BD destino %BD_DESTINO%  >>%FICHERO_LOG% 
  "C:\Program Files\MySQL\MySQL Server 5.6\bin\mysqldump.exe" --user=%1 --password=%2 --host=%3 --skip-triggers --opt --ignore-table=%4.historicoincidencias  --ignore-table=%4.operadores %4 %~6 | "C:\Program Files\MySQL\MySQL Server 5.6\bin\mysql.exe" --host=%3 --user=%1 --password=%2 -C %5  
  ECHO Se restauran los triggers %FICHERO_TRG_DESTBD_BCK% en la BD %BD_DESTINO%. Se ejecuta mysql con los parametros: --host=%3 --user=%1 --password=%2 %BD_DESTINO%    >>%FICHERO_LOG% 2>&1  
  "C:\Program Files\MySQL\MySQL Server 5.6\bin\mysql.exe" --host=%3 --user=%1 --password=%2 %BD_DESTINO% < %FICHERO_TRG_DESTBD_BCK%
) ELSE (
  :: Se aplica la sectorizacion, los cambios de la BD de configuracion (_trans) se aplican en la BD de explotación
  ECHO %DATE% %TIME% - Aplicar sectorizacion en la BD destino %BD_DESTINO%. Tablas:%6 >>%FICHERO_LOG% 
  "C:\Program Files\MySQL\MySQL Server 5.6\bin\mysqldump.exe" --user=%1 --password=%2 --host=%3 --skip-triggers --opt --ignore-table=%4.historicoincidencias %4 %~6 | "C:\Program Files\MySQL\MySQL Server 5.6\bin\mysql.exe" --host=%3 --user=%1 --password=%2 -C %5
)

ECHO %DATE% %TIME% FIN DEL PROCESO >>%FICHERO_LOG% 

exit 0





