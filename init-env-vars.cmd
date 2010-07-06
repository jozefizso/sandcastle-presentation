:: 
:: This script will set environment variables for current user
:: that are required for running Sandcastle and SHFB.
:: 
:: 
@echo off

set sandcastle=%~dp0Sandcastle\
set shfb=%~dp0SHFB\SandcastleBuilder\SandcastleBuilderGUI\bin\Debug\
set vs2010p=%~dp0Sandcastle\Presentation\vs2010

setx DXROOT %sandcastle%
setx SHFBROOT %shfb%
setx VS2010P %vs2010p%

echo %%DXROOT%% variable points to
echo    %sandcastle%
echo %%SHFBROOT%% variable points to
echo    %shfb%
echo %%VS2010P%% variable points to
echo    %vs2010p%
echo Note: SHFB uses the Debug build folder