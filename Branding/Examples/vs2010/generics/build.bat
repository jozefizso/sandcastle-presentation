@echo off
setlocal

if {%1}=={} (
	echo Please enter the number of the file to build.
	goto end
)

REM ********** Build file *******************

csc /t:library /out:tmp.dll /doc:generics%1-doc.xml generics%1.cs
del tmp.dll

:end
endlocal
