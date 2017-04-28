rem @echo off
rem mode con:cols=120 lines=40

rem 重设当前的CMD路径 

rem 就是进入批处理所在目录了
cd /d %~dp0 

set currentPath=%cd%

echo.%currentPath%

REM _protoSrc 是你的proto文件目录的位置
set _protoSrc=%currentPath%\proto

REM protoExe 是用于从proto生成java的protoc.exe程序的位置
set protoExe=%currentPath%\protoc.exe

REM java_out_file 存放生成的java文件目录的位置
set java_out_file=%currentPath%\java\

for /R "%_protoSrc%" %%i in (*) do (
 rem set filename=%%~nxi 
 if "%%~xi"  == ".proto" (

 %protoExe% --proto_path=%_protoSrc% --java_out=%java_out_file% %%i

 REM echo.%%~nxi to java ok !!!
 )
)

rem pause