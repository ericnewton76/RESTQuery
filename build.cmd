@ECHO OFF
setlocal
set PROJECT_NAME=RESTQuery

msbuild src\%PROJECT_NAME%\%PROJECT_NAME%.csproj /p:OutputPath=..\..\dist\%PROJECT_NAME% /p:Configuration=Release
