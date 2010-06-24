:: 
:: This scripts will install XML schema files that
:: are required for IntelliSense inside MAML Conceptual documents.
:: 
:: This scripts requires administrator priviledges.
:: 
@echo off

call "%VS100COMNTOOLS%\..\..\VC\vcvarsall.bat"

msbuild scripts\vs-intellisense.proj /target:InstallConceptualSchema