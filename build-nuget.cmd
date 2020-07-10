@ECHO OFF
setlocal
set PROJECTNAME=RESTQuery

REM initialization
set PACKAGES_ROOT=%~dp0packages

if not "%NUGET_EXE%" == "" goto :CHECK_NUGET
where nuget 1>NUL 2>NUL
if %ERRORLEVEL% == 1 set NUGET_EXE=packages\NuGet.CommandLine.5.6.0\tools\NuGet.exe
if %ERRORLEVEL% == 0 set NUGET_EXE=nuget

:CHECK_NUGET
%NUGET_EXE% 1>NUL 2>NUL
if errorlevel 1 echo Missing nuget from path set NUGET_EXE & goto :END

:NUGET_EXE
REM appveyor
if not "%APPVEYOR_BUILD_VERSION%" == "" set BUILD_VERSION=%APPVEYOR_BUILD_VERSION%
if not "%APPVEYOR_BUILD_VERSION%" == "" echo using nuget from Appveyor environment & set NUGET_EXE=nuget

REM checks
if "%BUILD_VERSION%" == "" echo Missing BUILD_VERSION & set FAIL=true

REM try install
%NUGET_EXE% install
if errorlevel 0 if not errorlevel 1 goto :NUGET_PACKAGES_INSTALLED
if not "%NUGET_EXE%" == "nuget" if not exist "%NUGET_EXE%" echo Missing Nuget.Commandline.2.8.6 in packages, run %NUGET_EXE% restore & set FAIL=true
if "%FAIL%" == "true" goto :END

:NUGET_PACKAGES_INSTALLED

mkdir dist 2>NUL

if "%1" == "--after-build" goto :SKIP_BUILD

echo.
if "%1" == "--skip-build" goto :SKIP_BUILD
call .\build.cmd & if errorlevel 1 goto :END
shift

:SKIP_BUILD

REM Create Nuget Package
pushd dist
rmdir /s /q Release 1>NUL 2>NUL

echo.
echo Copying assemblies to Release\lib
xcopy %PROJECTNAME%\%PROJECTNAME%.* Release\lib /s /y /i

pushd Release
echo.
echo Copying %PROJECTNAME%.nuspec
copy ..\..\%PROJECTNAME%.nuspec .
echo.

echo %NUGET_EXE% pack RESTQuery.nuspec -version %BUILD_VERSION%
%NUGET_EXE% pack RESTQuery.nuspec -version %BUILD_VERSION%
popd
popd

REM if not "%1" == "--no-deploy" %NUGET_EXE% push
shift

:END

