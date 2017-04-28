cls
rem 清屏

@echo off 
rem 关闭回显

title BatMain , Author:joeshifu

rem 设置控制台前景和背景颜色
color 0a

mode con:cols=120 lines=40

rem Unity.exe的路径
set unityExePath="D:\Program Files\Unity\Editor\Unity.exe"

goto  MENU

:MENU
rem 进入批处理所在目录
cd /d %~dp0 

rem echo.%~dp0
rem 当前盘符和路径

rem echo.%cd%
rem 当前目录

set currentpath=%cd%
rem echo.当前路径是:%currentpath%

cd..
set currentProjPath=%cd%
rem echo.上一级的目录是:%currentpath%

cd..
set parent=%cd%
rem echo.和代码工程同级的目录是:%parent%

rem 设置代码工程的目录 ProjectName 自定义 eg.ShiHuanJueLOL
set codeProjPath=%parent%\ShiHuanJueLOL

echo.
echo.=========================================================
echo.
echo.    0.退出
echo.    1.[Windows]:拷贝资源工程StreamingAssets/Windows文件夹下的资源到代码工程对应目录
echo.    2.[Android]:拷贝资源工程StreamingAssets/Android文件夹下的资源到代码工程对应目录
echo.    3.[ALL]:拷贝资源工程StreamingAssets下所有资源到代码工程
echo.    4.[Protobuf]:把.proto描述文件生成.cs文件，并拷贝到代码工程Scripts/Config/Protos(未完成)
echo.    5.打包Windows64位平台的AssetBundle
echo.
echo.=========================================================

set /p id=请输入:
if  "%id%" =="0" goto CMD0
if  "%id%" =="1" goto CMD1
if  "%id%" =="2" goto CMD2
if  "%id%" =="3" goto CMD3
if  "%id%" =="4" goto CMD4
if  "%id%" =="5" goto CMD5
pause

:CMD0
exit

:CMD1
rem 删除代码工程下的旧资源文件夹Windows，/s 递归删除子目录，/q 不提示
rd /s /q %codeProjPath%\Assets\StreamingAssets\Windows
md %codeProjPath%\Assets\StreamingAssets\Windows
rem del 只能删除文件
rem del /s /q %codeProjPath%\Assets\StreamingAssets\*

rem 进行拷贝操作 /y 覆盖 /e 递归，包括子目录和空文件
Xcopy "%currentProjPath%\Assets\StreamingAssets\Windows" "%codeProjPath%\Assets\StreamingAssets\Windows" /y /e 
goto  MENU

:CMD2
rd /s /q %codeProjPath%\Assets\StreamingAssets\Android
md %codeProjPath%\Assets\StreamingAssets\Android
Xcopy "%currentProjPath%\Assets\StreamingAssets\Android" "%codeProjPath%\Assets\StreamingAssets\Android" /y /e 
goto  MENU

:CMD3
Xcopy "%currentProjPath%\Assets\StreamingAssets" "%codeProjPath%\Assets\StreamingAssets" /y /e 
goto  MENU

:CMD4
rem 调用proto转cs的批处理
call %currentpath%\proto2cs\proto2cs.bat 
goto  MENU

:CMD5
echo.[%time%] 请稍等 
rem 具体参数参看unitymanual Command line arguments
%unityExePath% -quit -batchmode -executeMethod Builder.BuildAssetBundle_Windows64 
echo.[%time%] 完成 
goto  MENU
