using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// 把sourcePath下的资源打包到AssetBundleOutputPath下
/// </summary>
public class Builder : Editor
{
    public static string sourcePath = Application.dataPath + "/Resources";
    const string AssetBundlesOutputPath = "Assets/StreamingAssets";
    static string destinationPath;

    [MenuItem("Tools/AssetBundle/BuildForWindows64")]
    public static void BuildAssetBundle_Windows64()
    {
        BuildAssetBundle(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Tools/AssetBundle/BuildForOSX")]
    public static void BuildAssetBundle_OSX()
    {
        BuildAssetBundle(BuildTarget.StandaloneOSXUniversal);
    }

    [MenuItem("Tools/AssetBundle/BuildForAndroid")]
    public static void BuildAssetBundle_Android()
    {
        BuildAssetBundle(BuildTarget.Android);
    }

    [MenuItem("Tools/AssetBundle/BuildForIOS")]
    public static void BuildAssetBundle_IOS()
    {
        BuildAssetBundle(BuildTarget.iOS);
    }

    static void BuildAssetBundle(BuildTarget target)
    {
        destinationPath = Application.streamingAssetsPath;
        string platformName = Platform.GetPlatformFolder(target);
        destinationPath += "/" + platformName;
        //  Debug.Log(destinationPath);

        ClearAssetBundlesName();

        Pack(sourcePath);

        string outputPath = Path.Combine(AssetBundlesOutputPath, platformName);

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        BuildPipeline.BuildAssetBundles(outputPath, 0, target);

        AssetDatabase.Refresh();

        Debug.Log("打包完成.");

        BuildMD5.Build(platformName);

		EditorUtility.DisplayDialog("","完成","确定");

    }

    /// <summary>
    /// 因为只要设置了AssetBundleName的，都会进行打包，不论在什么目录下
    /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包
    /// </summary>
    static void ClearAssetBundlesName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        //Debug.Log (length);
        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }

        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
        length = AssetDatabase.GetAllAssetBundleNames().Length;
        //Debug.Log (length);
    }

    static void Pack(string source)
    {
        DirectoryInfo folder = new DirectoryInfo(source);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;

        for (int i = 0; i < length; i++)
        {
            if (!files[i].Name.EndsWith(".meta"))
            {
                if (files[i] is DirectoryInfo)
                {
                    Pack(files[i].FullName);
                }
                else
                {
                    //什么类型可以打包的，在这里判断一下
                    if (files[i].Name.EndsWith(".prefab") || files[i].Name.EndsWith(".unity") || files[i].Name.EndsWith(".mat") || files[i].Name.EndsWith(".ttf"))
                    {
                        file(files[i].FullName);
                    }
                    else if (files[i].Name.EndsWith(".json") || files[i].Name.EndsWith(".txt"))
                    {
                        //那么就设置相同的AssstBundleName,这样就会打到一个包里去
						string _source = Replace(files[i].FullName);
						string _assetPath = "Assets" + _source.Substring(Application.dataPath.Length);
						string _assetPath2 = _source.Substring(Application.dataPath.Length + 1);
						//Debug.Log (_assetPath);
						AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath);
						string assetName = _assetPath2.Substring(_assetPath2.IndexOf("/") + 1);
						assetName = assetName.Replace(Path.GetExtension(assetName), ".unity3d");
						assetName = assetName.Substring(0,assetName.LastIndexOf("/"));
						//Debug.Log (assetName);
						assetImporter.assetBundleName = assetName + "/huanjue_json.unity3d";
						//assetImporter.assetBundleName = assetName;

                    }
                    else//不可以打包的资源，直接copy过去
                    {
                        string _source = Replace(files[i].FullName);
                        string _assetPath2 = _source.Substring(Application.dataPath.Length + 1);
                        string assetName = _assetPath2.Substring(_assetPath2.IndexOf("/") + 1);
                        string path = destinationPath + "/" + assetName;

                        path = Path.GetDirectoryName(path);

                        if (Directory.Exists(path))
                        {

                        }
                        else
                        {
                            //没有这个路径就创建
                            Directory.CreateDirectory(path);
                            //Debug.Log(files[i].FullName);
                        }
                        //文件拷贝到路径下去
                        File.Copy(files[i].FullName, path + "/" + files[i].Name, true);
                    }
                }
            }
        }
    }

    static void file(string source)
    {
        string _source = Replace(source);
        string _assetPath = "Assets" + _source.Substring(Application.dataPath.Length);
        string _assetPath2 = _source.Substring(Application.dataPath.Length + 1);
        //Debug.Log (_assetPath);
        AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath);
        string assetName = _assetPath2.Substring(_assetPath2.IndexOf("/") + 1);
        assetName = assetName.Replace(Path.GetExtension(assetName), ".unity3d");
        //        Debug.Log (assetName);
        assetImporter.assetBundleName = assetName;
    }

    static string Replace(string s)
    {
        return s.Replace("\\", "/");
    }
}
