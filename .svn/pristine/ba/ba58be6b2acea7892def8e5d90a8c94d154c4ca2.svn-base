using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class Packager
{
    public static string platform = string.Empty;
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();
    static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();

    [MenuItem("AssetBundle/Build iPhone Resource", false, 100)]
    public static void BuildiPhoneResource()
    {
        BuildTarget target;
#if UNITY_5
        target = BuildTarget.iOS;
#else
		target = BuildTarget.iPhone;
#endif
        BuildAssetResource(target);
    }

    [MenuItem("AssetBundle/Build Android Resource", false, 101)]
    public static void BuildAndroidResource()
    {
        BuildAssetResource(BuildTarget.Android);
    }

    [MenuItem("AssetBundle/Build Windows Resource", false, 102)]
    public static void BuildWindowsResource()
    {
        BuildAssetResource(BuildTarget.StandaloneWindows);
    }


    /// <summary>
    /// 生成绑定素材
    /// </summary>
    public static void BuildAssetResource(BuildTarget target)
    {
        if (Directory.Exists(Util.DataPath))
        {
            Directory.Delete(Util.DataPath, true);
        }
        string streamPath = Application.streamingAssetsPath;
        if (Directory.Exists(streamPath))
        {
            Directory.Delete(streamPath, true);
        }
        Directory.CreateDirectory(streamPath);
        AssetDatabase.Refresh();

        maps.Clear();

        HandleGameAssetBundle();

        string resPath = "Assets/StreamingAssets";

        BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle;
        BuildPipeline.BuildAssetBundles(resPath, maps.ToArray(), options, target);
        BuildFileIndex();

        //string streamDir = Application.dataPath + "/Lua/";
        //if (Directory.Exists(streamDir))
        //    Directory.Delete(streamDir, true);
        AssetDatabase.Refresh();
    }




    //------------------------------------------------------------------------------------
    public static string sourcePath = Application.dataPath + "/Resources";

    /// <summary>
    ///处理自己的游戏资源 
    /// </summary>
    static void HandleGameAssetBundle()
    {
        Pack(sourcePath);
    }

    static void Pack(string source)
    {
        DirectoryInfo folder = new DirectoryInfo(source);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;
        for (int i = 0; i < length; i++)
        {
            if (files[i] is DirectoryInfo)
            {
                Pack(files[i].FullName);
            }
            else
            {
                if (!files[i].Name.EndsWith(".meta"))
                {
                    file(files[i].FullName);
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

        string assetName = _assetPath2.Substring(_assetPath2.IndexOf("/") + 1);
        assetName = assetName.Replace(Path.GetExtension(assetName), AppConst.ExtName);
        //Debug.Log (assetName);
        //在代码中给资源设置AssetBundleName
        //AssetImporter assetImporter = AssetImporter.GetAtPath (_assetPath);
        //assetImporter.assetBundleName = assetName;

        AssetBundleBuild build = new AssetBundleBuild();
        //assetName = assetName.Substring(0,assetName.LastIndexOf("Panel"));
        build.assetBundleName = assetName.ToLower();//assetName.ToLower()+ AppConst.ExtName;
        build.assetNames = new string[] { _assetPath };
        maps.Add(build);
    }

    static string Replace(string s)
    {
        return s.Replace("\\", "/");
    }
    //------------------------------------------------------------------------------------

  

    static int[] String2IntArry(string str)
    {
        string[] strs = str.Split('.');
        List<int> ints = new List<int>();
        for (int i = 0; i < strs.Length; i++)
        {
            ints.Add(int.Parse(strs[i]));
        }
        return ints.ToArray();
    }

    static string IntArray2String(int[] ints)
    {
        string str = string.Format("{0}.{1}.{2}.{3}", ints[0], ints[1], ints[2], ints[3]);
        return str;
    }

    static void BuildFileIndex()
    {
        string resPath = Application.dataPath+ "/StreamingAssets/";

        ///----------------------创建文件列表-----------------------
        string newFilePath = resPath + "/files.txt";
        if (File.Exists(newFilePath))
            File.Delete(newFilePath);

        paths.Clear();
        files.Clear();
        Recursive(resPath);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);

        //加上版本号
        TextAsset verTxt = (TextAsset)Resources.Load("config");
        int[] versionNums = String2IntArry(verTxt.text);
        versionNums[3]++;
        string ver = IntArray2String(versionNums);
        sw.WriteLine(ver);
        //这里还要更新config
        string configFilePath = Application.dataPath + "/Resources/config.txt";
        if (File.Exists(configFilePath))
            File.Delete(configFilePath);
        FileStream config_fs = new FileStream(configFilePath, FileMode.CreateNew);
        StreamWriter config_sw = new StreamWriter(config_fs);
        config_sw.WriteLine(ver);
        config_sw.Close();
        config_fs.Close();


        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            string ext = Path.GetExtension(file);
            if (file.EndsWith(".meta") || file.Contains(".DS_Store"))
                continue;

            string md5 = Util.md5file(file);
            string value = file.Replace(resPath, string.Empty);


            FileStream _fs = new FileStream(file, FileMode.Open);
            int size = (int)_fs.Length;
            _fs.Close();
            sw.WriteLine(value + "|" + md5 + "|" + size);
        }
        sw.Close();
        fs.Close();
    }



    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta"))
                continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }

    
  

    
}