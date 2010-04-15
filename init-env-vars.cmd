:: 
:: This script will set environment variables for current user
:: that are required for running Sandcastle and SHFB.
:: 
:: 
@echo off

set sandcastle=%~dp0Sandcastle\
set shfb=%~dp0SHFB\SandcastleBuilder\SandcastleBuilderGUI\bin\Debug\

setx DXROOT %sandcastle%
setx SHFBROOT %shfb%

echo %%DXROOT%% variable points to
echo    %sandcastle%
echo %%SHFBROOT%% variable points to
echo    %shfb%
echo Note: SHFB uses the Debug build folder