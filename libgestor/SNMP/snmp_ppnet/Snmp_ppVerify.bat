@echo off
set SNMPPP_SOLUTION=%1
set SNMPPP_MODE=%2
set SNMPPP_AGENT=%3

IF NOT "%SNMPPP_AGENT%"=="" goto Verify

echo "Usage: %0 <SolutionDir> {Debug|Release} <SnmpAgent>"
echo "Example: %0 VS.NET Release 127.0.0.1"
goto End

:Verify
set SNMPPP_CD=%CD%
cd %SNMPPP_SOLUTION%

echo --------------------------
echo SnmpManager test
echo --------------------------
cd SnmpManager\bin\%SNMPPP_MODE%
SnmpManager walk %SNMPPP_AGENT% -o 1.3.6.1.2.1.1 -Dl0

echo --------------------------
echo SnmpManager/GetTable test
echo --------------------------
SnmpManager table %SNMPPP_AGENT% -o 1.3.6.1.2.1.2.2.1.2 -o 1.3.6.1.2.1.2.2.1.3 -o 1.3.6.1.2.1.2.2.1.4 -o 1.3.6.1.2.1.2.2.1.5 -o 1.3.6.1.2.1.2.2.1.6 -o 1.3.6.1.2.1.2.2.1.7 -o 1.3.6.1.2.1.2.2.1.8 -Dl0

echo --------------------------
echo SystemWalkExample test
echo --------------------------
cd ..\..\..\SystemWalkExample\bin\%SNMPPP_MODE%
SystemWalkExample %SNMPPP_AGENT% public

echo --------------------------
echo SystemWalkExampleCpp test
echo --------------------------
cd ..\..\..\SystemWalkExampleCpp\%SNMPPP_MODE%
SystemWalkExampleCpp %SNMPPP_AGENT% public

echo --------------------------
echo SystemWalkExampleVb test
echo --------------------------
cd ..\..\SystemWalkExampleVb\bin
SystemWalkExampleVb %SNMPPP_AGENT% public

cd %SNMPPP_CD%

set SNMPPP_SOLUTION=
set SNMPPP_MODE=
set SNMPPP_AGENT=
set SNMPPP_CD=

:End 
