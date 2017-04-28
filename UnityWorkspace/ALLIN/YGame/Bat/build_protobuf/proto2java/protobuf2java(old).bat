@echo off
echo ** setting runtime variable

REM _protoSrc 是你的proto文件目录的位置
set _protoSrc=F:\JoeWorkspace\Project\X-Game\Cfg_Build\build_protobuf\proto2java\proto

REM protoExe 是用于从proto生成java的protoc.exe程序的位置
set protoExe=F:\JoeWorkspace\Project\X-Game\Cfg_Build\build_protobuf\proto2java\protoc.exe

REM java_out_file 存放生成的Java文件目录的位置
set java_out_file=F:\JoeWorkspace\Project\X-Game\Cfg_Build\build_protobuf\proto2java\java\

for /R "%_protoSrc%" %%i in (*) do ( 
	rem set filename=%%~nxi 
	if "%%~xi"  == ".proto" (
		%protoExe% --proto_path=%_protoSrc% --java_out=%java_out_file% %%i
	)
)

REM pause