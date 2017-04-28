rem @echo off
rem mode con:cols=120 lines=40

rem 重设当前的CMD路径 

rem 就是进入批处理所在目录了
cd /d %~dp0 

set currentPath=%cd%

echo.%currentPath%

REM _protoSrc 是你的proto文件目录的位置
set _protoSrc=proto

REM protoExe 是用于从proto生成cs的protogen.exe程序的位置
set protoExe=protogen

REM cs_out_file 存放生成的cs文件目录的位置
set cs_out_file=%currentPath%\cs\

for /R "%_protoSrc%" %%i in (*) do (
 if "%%~xi"  == ".proto" (

 rem echo.%protoExe% -i:%%i -o:%cs_out_file%%%~ni.cs -ns:MyProtoBuf
 %protoExe% -i:%%i -o:%cs_out_file%%%~ni.cs -ns:MyProtoBuf

  echo.%%~nxi to csharp ok !!!
 )
)

REM pause