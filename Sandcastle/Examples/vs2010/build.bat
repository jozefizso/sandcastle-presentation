@echo off

REM ********** Set path for .net framework2.0, sandcastle,hhc,hxcomp****************************

set DXROOT=%~dp0..\..\
set PATH=%windir%\Microsoft.NET\Framework\v2.0.50727;%DXROOT%\ProductionTools;%PATH%
set TOOLSPATH=%ProgramFiles%
if exist "%ProgramFiles% (x86)" set TOOLSPATH=%ProgramFiles(x86)%
set PATH=%TOOLSPATH%\HTML Help Workshop;%TOOLSPATH%\Microsoft Help 2.0 SDK;%PATH%

if exist output rmdir output /s /q
if exist chm rmdir chm /s /q

REM ********** Call MRefBuilder ****************************

MRefBuilder test.dll /out:reflection.org

REM ********** Apply Transforms ****************************

:: create model and file names
XslTransform /xsl:"%DXROOT%\ProductionTransforms\ApplyVSDocModel.xsl" reflection.org /xsl:"%DXROOT%\ProductionTransforms\AddFriendlyFilenames.xsl" /out:reflection.xml /arg:IncludeAllMembersTopic=true /arg:IncludeInheritedOverloadTopics=false
:: create manifest
XslTransform /xsl:"%DXROOT%\ProductionTransforms\ReflectionToManifest.xsl"  reflection.xml /out:manifest.xml

call "%DXROOT%\Presentation\vs2010\copyOutput.bat"

REM ********** Call BuildAssembler ****************************
BuildAssembler /config:"%DXROOT%\Presentation\vs2010\configuration\sandcastle.config" manifest.xml

REM **************Generate an intermediate Toc file that simulates the Whidbey TOC format.

REM XslTransform /xsl:"%DXROOT%\ProductionTransforms\createvstoc.xsl" reflection.xml /out:toc.xml 
