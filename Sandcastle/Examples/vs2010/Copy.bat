@echo off

REM *
REM * Test script for Copy and Clean output scripts.
REM *

REM ********** Set path for extensions (current vs2010 presentation project) *******************

set DXROOTEXT=%~dp0..\..
set VS2010P=%DXROOTEXT%\Presentation\vs2010

call "%VS2010P%\CleanOutput.bat"

call "%VS2010P%\CopyOutput.bat"
