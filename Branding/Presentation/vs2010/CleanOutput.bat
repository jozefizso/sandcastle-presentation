@echo off

if "%1"=="/all" (
	echo Cleaning up all output directories.
	
	if exist Output rmdir Output /s /q
	if exist chm rmdir chm /s /q
	if exist IntelliSense rmdir IntelliSense /s /q
) else (
	echo Cleaning up HTML and CHM output directories.
	
	if exist Output\html del Output\html\* /q
	if exist chm rmdir chm /s /q
	
)


