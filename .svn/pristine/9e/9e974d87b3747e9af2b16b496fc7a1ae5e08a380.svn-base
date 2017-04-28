@echo off

set FILE_FORMAT=plist

setlocal enabledelayedexpansion

@echo on

for /f "delims=" %%i in ('"dir /a/b/on *.csv"') do (
set file=%%~fi
DataConverter.exe !file! %FILE_FORMAT% 1
)

@echo off

if not exist %FILE_FORMAT% md %FILE_FORMAT%

move *.plist %FILE_FORMAT%

pause