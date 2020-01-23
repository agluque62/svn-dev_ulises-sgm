@echo off

rmdir /S /Q doc

cd VS.NET
rmdir /S /Q libdes\Debug
rmdir /S /Q libdes\Release
rmdir /S /Q snmp++\Debug
rmdir /S /Q snmp++\Release
rmdir /S /Q SnmpComp\DebugLib
rmdir /S /Q SnmpComp\DebugMemLib
rmdir /S /Q SnmpComp\ReleaseLib
rmdir /S /Q SnmpComp\ReleaseMemLib
rmdir /S /Q SnmpManager\bin
rmdir /S /Q SnmpManager\obj
rmdir /S /Q SystemWalkExample\bin
rmdir /S /Q SystemWalkExample\obj
rmdir /S /Q SystemWalkExampleCpp\Debug
rmdir /S /Q SystemWalkExampleCpp\Release
rmdir /S /Q SystemWalkExampleVb\bin
rmdir /S /Q SystemWalkExampleVb\obj
rmdir /S /Q TableReader\bin
rmdir /S /Q TableReader\obj
rmdir /S /Q temp
rmdir /S /Q TrapListener\bin
rmdir /S /Q TrapListener\obj
del /Q VS.NET.ncb

cd ..\VSOLD.NET
rmdir /S /Q SnmpComp\Debug
rmdir /S /Q SnmpComp\DebugMem
rmdir /S /Q SnmpComp\Release
rmdir /S /Q SnmpComp\ReleaseMem
rmdir /S /Q SnmpManager\bin
rmdir /S /Q SnmpManager\obj
rmdir /S /Q SnmpUtils\bin
rmdir /S /Q SnmpUtils\obj
rmdir /S /Q SystemWalkExample\bin
rmdir /S /Q SystemWalkExample\obj
rmdir /S /Q SystemWalkExampleCpp\Debug
rmdir /S /Q SystemWalkExampleCpp\Release
rmdir /S /Q SystemWalkExampleVb\bin
rmdir /S /Q SystemWalkExampleVb\obj
rmdir /S /Q TableReader\bin
rmdir /S /Q TableReader\obj
rmdir /S /Q temp
rmdir /S /Q TrapListener\bin
rmdir /S /Q TrapListener\obj
del /Q VS.NET.ncb

cd ..