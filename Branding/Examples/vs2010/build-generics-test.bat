@echo off

REM ********** Build test code with generics ***********
csc.exe /nologo /t:library /doc:generics-comments.xml generics-test.cs
MRefBuilder generics-test.dll /out:generics-reflection.org
