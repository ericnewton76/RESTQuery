@ECHO OFF
setlocal
set PROJECT_NAME=RESTQuery

echo Cleaning dist\Release
rmdir /s /q dist\Release 1>NUL 2>NUL

echo.
msbuild src\%PROJECT_NAME%\%PROJECT_NAME%.csproj /p:OutputPath=..\..\dist\Release /p:Configuration=Release
