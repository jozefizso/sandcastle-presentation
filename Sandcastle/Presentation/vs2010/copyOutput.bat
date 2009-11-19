@echo off

REM *
REM * CopyOutput.bat will copy resource files from template folder vs2010
REM * to output directory located at the build script.
REM *

setlocal
set VS2010P=%~dp0

echo Copying resources to output.

if not exist Output mkdir Output

if not exist Output\html mkdir Output\html
if not exist Output\icons mkdir Output\icons
if not exist Output\scripts mkdir Output\scripts
if not exist Output\styles mkdir Output\styles
if not exist Output\media mkdir Output\media

REM Mirror (sync) the contents of the resource folders to the Output

robocopy "%VS2010P%\icons" "Output\icons" /MIR /NJH /NJS /NP /NFL
robocopy "%VS2010P%\scripts" "Output\scripts" /MIR /NJH /NJS /NP /NFL
robocopy "%VS2010P%\styles" "Output\styles" /MIR /NJH /NJS /NP /NFL

REM if not exist Intellisense mkdir Intellisense

endlocal
