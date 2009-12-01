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

REM ********** Compile and Call MRefBuilder ****************************

csc /t:library /out:test.dll /doc:comments.xml test.cs
MRefBuilder test.dll /out:reflection.org

REM ********** Apply Transforms ****************************

set ApplyVSDocModel=%DXROOT%\ProductionTransforms\ApplyVSDocModel.xsl
set AddFiendlyFilenames=%DXROOT%\ProductionTransforms\AddFriendlyFilenames.xsl
set ReflectionToManifest=%DXROOT%\ProductionTransforms\ReflectionToManifest.xsl
set SandcastleConfig=%DXROOTEXT%\Presentation\vs2010\configuration\sandcastle.config

REM create model and file names
XslTransform /xsl:"%ApplyVSDocModel%" reflection.org /xsl:"%AddFiendlyFilenames%" /out:reflection.xml /arg:IncludeAllMembersTopic=true /arg:IncludeInheritedOverloadTopics=false
REM create manifest
XslTransform /xsl:"%ReflectionToManifest%"  reflection.xml /out:manifest.xml

call "%VS2010P%\CopyOutput.bat"

REM ********** Call BuildAssembler ****************************
BuildAssembler /config:"%SandcastleConfig%" manifest.xml

REM **************Generate an intermediate Toc file that simulates the Whidbey TOC format.

REM XslTransform /xsl:"%DXROOT%\ProductionTransforms\createvstoc.xsl" reflection.xml /out:toc.xml 

endlocal
