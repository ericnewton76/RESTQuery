@ECHO OFF
setlocal
set PROJECTNAME=RESTQuery

REM appveyor
if not "%APPVEYOR_BUILD_VERSION%" == "" set BUILD_VERSION=%APPVEYOR_BUILD_VERSION%
if "%APPVEYOR%" == "true" echo using nuget from Appveyor environment & set NUGET_EXE=nuget

REM checks
if "%BUILD_VERSION%" == "" echo Missing BUILD_VERSION & set FAIL=true & goto :FAIL

REM initialization
set PACKAGES_ROOT=%~dp0packages

if not "%NUGET_EXE%" == "" goto :CHECK_NUGET
where nuget 1>NUL 2>NUL
if %ERRORLEVEL% == 1 set NUGET_EXE=packages\NuGet.CommandLine.5.6.0\tools\NuGet.exe
if %ERRORLEVEL% == 0 set NUGET_EXE=nuget

:CHECK_NUGET
%NUGET_EXE% 1>NUL 2>NUL
if errorlevel 1 echo Missing nuget from path. set NUGET_EXE=[how to call nuget] & goto :FAIL

:NUGET_EXE

REM try install

:NUGET_PACKAGES_INSTALLED

:CLEAN_DIST
echo Cleaning dist\Release
rmdir /s /q dist\Release 1>NUL 2>NUL

mkdir dist 2>NUL
if not exist "dist\." echo Failed to create 'dist' folder &  goto :FAIL

echo.
if "%1" == "--skip-build" echo **SKIPPING BUILD & goto :SKIP_BUILD

echo.
msbuild src\%PROJECTNAME%\%PROJECTNAME%.csproj /p:OutputPath=..\..\dist\Release /p:Configuration=Release

shift

:SKIP_BUILD

REM Create Nuget Package
pushd dist
pwd

echo.
echo Cleaning out nupkg directory...
rmdir /s /q nupkg 1>NUL 2>NUL
REM xcopy will recreate nupkg

echo.
echo Copying assemblies to dist/nupkg
echo xcopy Release\%PROJECTNAME%.* nupkg\lib /s /y /i
xcopy Release\%PROJECTNAME%.* nupkg\lib /s /y /i

pushd nupkg
pwd

echo.
echo copy ..\..\%PROJECTNAME%.nuspec .
copy ..\..\%PROJECTNAME%.nuspec .
echo.

echo %NUGET_EXE% pack RESTQuery.nuspec -version %BUILD_VERSION%
%NUGET_EXE% pack RESTQuery.nuspec -version %BUILD_VERSION%
popd
popd

REM if not "%1" == "--no-deploy" %NUGET_EXE% push
shift

goto :END

:FAIL
echo An error occurred.
if %ERRORLEVEL% == 0 EXIT /B 1

:END

