@echo off

REM ********** Set path for .net framework2.0, sandcastle,hhc,hxcomp****************************

set DXROOT=%~dp0..\..\
set PATH=%windir%\Microsoft.NET\Framework\v2.0.50727;%DXROOT%\ProductionTools;%PATH%
set TOOLSPATH=%ProgramFiles%
if exist "%ProgramFiles% (x86)" set TOOLSPATH=%ProgramFiles(x86)%
set PATH=%TOOLSPATH%\HTML Help Workshop;%TOOLSPATH%\Microsoft Help 2.0 SDK;%PATH%


REM ********** Call BuildAssembler ****************************
echo BuildAssemler
BuildAssembler /config:"%DXROOT%\Presentation\vs2010\configuration\sandcastle.config" manifest.xml

:end