using UnityEngine;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;

using UnityEditor.XCodeEditor;

public class XcodeProjectMod : Editor
{
#if UNITY_IOS || UNITY_EDITOR
    [PostProcessBuild]
    static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject)
    {
        if (target == BuildTarget.iOS)
        {

            ModifyProj(pathToBuildProject);
            SetPlist(pathToBuildProject);
        }
    }

    public static void ModifyProj(string pathToBuildProject)
    {
        string _projPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
        PBXProject _pbxProj = new PBXProject();
        _pbxProj.ReadFromString(File.ReadAllText(_projPath));

        //获取当前项目名字  
        string _targetGuid = _pbxProj.TargetGuidByName("Unity-iPhone");//PBXProject.GetUnityTargetName()

        //*******************************添加framework*******************************//
        _pbxProj.AddFrameworkToProject(_targetGuid, "StoreKit.framework", true);
        _pbxProj.AddFrameworkToProject(_targetGuid, "Security.framework", false);
        _pbxProj.AddFrameworkToProject(_targetGuid, "JavaScriptCore.framework", false);
        _pbxProj.AddFrameworkToProject(_targetGuid, "libc++.1.tbd", false);
        _pbxProj.AddFrameworkToProject(_targetGuid, "libz.1.tbd", false);


        //*******************************添加tbd*******************************//
        _pbxProj.AddFileToBuild(_targetGuid, _pbxProj.AddFile("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk));
        _pbxProj.AddFileToBuild(_targetGuid, _pbxProj.AddFile("usr/lib/libc++.tbd", "Frameworks/libc++.tbd", PBXSourceTree.Sdk));

        #region 第三方的
        //------
        //文件夹下所有文件
        CopyAndReplaceDirectory("Assets/Lib/mylib.framework", Path.Combine(pathToBuildProject, "Frameworks/mylib.framework"));
        _pbxProj.AddFileToBuild(_targetGuid, _pbxProj.AddFile("Frameworks/mylib.framework", "Frameworks/mylib.framework", PBXSourceTree.Source));

        //单文件
        var fileName = "my_file.xml";
        var filePath = Path.Combine("Assets/Lib", fileName);
        File.Copy(filePath, Path.Combine(pathToBuildProject, fileName));
        _pbxProj.AddFileToBuild(_targetGuid, _pbxProj.AddFile(fileName, fileName, PBXSourceTree.Source));
        //------
        #endregion



        //*******************************设置buildsetting*******************************//
        _pbxProj.SetBuildProperty(_targetGuid, "ENABLE_BITCODE", "NO");
        //_pbxProj.AddBuildProperty(_targetGuid,"");
        //_pbxProj.UpdateBuildProperty();

        //_pbxProj.SetBuildProperty(_targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
        //_pbxProj.AddBuildProperty(_targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");

        // 设置签名  
        //_pbxProj.SetBuildProperty (_targetGuid, "CODE_SIGN_IDENTITY", "iPhone Distribution: _______________");  
        //_pbxProj.SetBuildProperty (_targetGuid, "PROVISIONING_PROFILE", "********-****-****-****-************");   

        //******************************编辑代码****************************************************************
        //修改代码
        //EditorCode(_projPath);


        File.WriteAllText(_projPath, _pbxProj.WriteToString());

        // 保存工程  
        //_pbxProj.WriteToFile(_projPath);
    }



    /// <summary>
    /// 修改plist  
    /// </summary>
    /// <param name="pathToBuildProject"></param>
    static void SetPlist(string pathToBuildProject)
    {
        string _plistPath = pathToBuildProject + "/Info.plist";
        PlistDocument _plist = new PlistDocument();

        _plist.ReadFromString(File.ReadAllText(_plistPath));

        PlistElementDict _rootDic = _plist.root;
        _rootDic.SetString("View controller-based status bar appearance", "NO");
        _rootDic.SetString("NSContactsUsageDescription", "是否允许此游戏使用麦克风？");



        File.WriteAllText(_plistPath, _plist.WriteToString());

        //保存plist  
        //_plist.WriteToFile(_plistPath);
    }



    static void CopyAndReplaceDirectory(string srcPath, string dstPath)
    {
        if (Directory.Exists(dstPath))
            Directory.Delete(dstPath);
        if (File.Exists(dstPath))
            File.Delete(dstPath);

        Directory.CreateDirectory(dstPath);

        foreach (var file in Directory.GetFiles(srcPath))
            File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));

        foreach (var dir in Directory.GetDirectories(srcPath))
            CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
    }


    /// <summary>
    /// 编辑代码文件
    /// </summary>
    /// <param name="filePath"></param>
    private static void EditorCode(string filePath)
    {
        //读取UnityAppController.mm文件
        XClass UnityAppController = new XClass(filePath + "/Classes/UnityAppController.mm");

        //在指定代码后面增加一行代码
        //此方法在某一时刻才生效（2.0 < iOS Version < 9.0）
        UnityAppController.WriteBelow("@synthesize interfaceOrientation	= _curOrientation;",
            "- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url{[[IapppayKit sharedInstance] handleOpenUrl:url];return YES;}");



        //此方法在某一时刻才生效（iOS Version > 9.0）
        UnityAppController.WriteBelow("@synthesize renderDelegate			= _renderDelegate;",
            "- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<NSString*, id> *)options{[[IapppayKit sharedInstance] handleOpenUrl:url];return YES;}");


        UnityAppController.WriteBelow("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);",
            "[[IapppayKit sharedInstance] handleOpenUrl:url];");

        //声明头文件
        UnityAppController.WriteBelow("#import <OpenGLES/ES2/glext.h>",
            "#import <IapppayKit/IapppayKit.h>");

        //在指定代码中替换一行
        //UnityAppController.Replace(@"return YES;",codeAdd2);

    }
#endif
}