@echo off
setlocal

REM ********** Set path for extensions (current vs2010 presentation project) *******************

set DXROOT=%DXROOT:~0,-1%
set DXROOTEXT=%~dp0..\..
set VS2010P=%DXROOTEXT%\Presentation\vs2010

REM ********** Set path for .net framework2.0, sandcastle,hhc,hxcomp****************************

set PATH=%windir%\Microsoft.NET\Framework\v2.0.50727;%DXROOT%\ProductionTools;%PATH%
set TOOLSPATH=%ProgramFiles%
if exist "%ProgramFiles% (x86)" set TOOLSPATH=%ProgramFiles(x86)%
set PATH=%TOOLSPATH%\HTML Help Workshop;%TOOLSPATH%\Microsoft Help 2.0 SDK;%PATH%


call "%VS2010P%\CleanOutput.bat"
call "%VS2010P%\CopyOutput.bat"

REM ********** Call BuildAssembler ****************************

set SandcastleConfig=%VS2010P%\configuration\sandcastle-javadoc.config

BuildAssembler /config:"%SandcastleConfig%" manifest-javadoc.xml

endlocal
