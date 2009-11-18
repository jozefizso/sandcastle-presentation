@echo off

REM ********** Set path for .net framework2.0, sandcastle,hhc,hxcomp****************************

set DXROOT=%~dp0..\..\
set PATH=%windir%\Microsoft.NET\Framework\v2.0.50727;%DXROOT%\ProductionTools;%PATH%
set TOOLSPATH=%ProgramFiles%
if exist "%ProgramFiles% (x86)" set TOOLSPATH=%ProgramFiles(x86)%
set PATH=%TOOLSPATH%\HTML Help Workshop;%TOOLSPATH%\Microsoft Help 2.0 SDK;%PATH%

if exist output rmdir output /s /q
if exist chm rmdir chm /s /q

REM ********** generate reflection data files for .net framework2.0****************************
::msbuild fxReflection.proj /Property:NetfxVer=2.0 /Property:PresentationStyle=%1

REM ********** Compile source files ****************************

::csc /t:library /doc:comments.xml test.cs
::if there are more than one file, please use [ csc /t:library /doc:comments.xml *.cs ]

if exist test.xml copy /y test.xml comments.xml

REM ********** Call MRefBuilder ****************************

MRefBuilder test.dll /out:reflection.org

REM ********** Apply Transforms ****************************

XslTransform /xsl:"%DXROOT%\ProductionTransforms\ApplyVSDocModel.xsl" reflection.org /xsl:"%DXROOT%\ProductionTransforms\AddFriendlyFilenames.xsl" /out:reflection.xml /arg:IncludeAllMembersTopic=true /arg:IncludeInheritedOverloadTopics=false


XslTransform /xsl:"%DXROOT%\ProductionTransforms\ReflectionToManifest.xsl"  reflection.xml /out:manifest.xml

call "%DXROOT%\Presentation\vs2010\copyOutput.bat"

REM ********** Call BuildAssembler ****************************
BuildAssembler /config:"%DXROOT%\Presentation\vs2010\configuration\sandcastle.config" manifest.xml

REM **************Generate an intermediate Toc file that simulates the Whidbey TOC format.

REM XslTransform /xsl:"%DXROOT%\ProductionTransforms\createvstoc.xsl" reflection.xml /out:toc.xml 
