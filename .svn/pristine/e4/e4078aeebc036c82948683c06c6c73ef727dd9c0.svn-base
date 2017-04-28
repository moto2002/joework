@echo off

set FILE_FORMAT=lua

setlocal enabledelayedexpansion

@echo on

for /f "delims=" %%i in ('"dir /a/b/on *.csv"') do (
set file=%%~fi
DataConverter.exe !file! %FILE_FORMAT%
)

@echo off

if not exist %FILE_FORMAT% md %FILE_FORMAT%

move *.lua %FILE_FORMAT%

pause