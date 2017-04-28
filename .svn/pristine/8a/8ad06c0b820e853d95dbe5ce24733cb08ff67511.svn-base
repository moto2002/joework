using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ResourcesManager : Singleton<ResourcesManager>
{
    //bundle后缀
    string assetTail = ".unity3d";

    //正在加载的资源
    Dictionary<string, bool> m_resLoading = new Dictionary<string, bool>();

    //已经加载的Assetbundle
    Dictionary<string, AssetBundle> m_assetBundleCache = new Dictionary<string, AssetBundle>();

    //常驻内存的bundle，加载一次后就不会被清除
    Dictionary<string, bool> m_residentList = new Dictionary<string, bool>();


    /// <summary>
    /// 清除AssetBundle缓存，常驻内存的不清除
    /// </summary>
    public void ClearCache()
    {
        List<string> keys = new List<string>(m_assetBundleCache.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (m_residentList.ContainsKey(keys[i]))
            {
                continue;
            }
            m_assetBundleCache[keys[i]].Unload(false);
            m_assetBundleCache.Remove(keys[i]);
        }
    }

    #region LoadAssetBundle

    /// <summary>
    /// 加载目标资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public void LoadAssetBundle(string name, Action<UnityEngine.Object> callback)
    {

        name = name + assetTail;//eg:ui/panel.unity3d
        name = name.ToLower();

        Action<List<AssetBundle>> action = (depenceAssetBundles) =>
        {

            string realName = this.GetRuntimePlatform() + "/" + name;//eg:Windows/ui/panel.unity3d

            //            LoadResReturnWWW(realName, (www) =>
            //            {
            //                int index = realName.LastIndexOf("/");
            //                string assetName = realName.Substring(index + 1);
            //                assetName = assetName.Replace(assetTail, "");
            //                AssetBundle assetBundle = www.assetBundle;
            //                UnityEngine.Object obj = assetBundle.LoadAsset(assetName);//LoadAsset(name）,这个name没有后缀,eg:panel
            //
            //                //卸载资源内存
            //                assetBundle.Unload(false);
            //                for (int i = 0; i < depenceAssetBundles.Count; i++)
            //                {
            //                    depenceAssetBundles[i].Unload(false);
            //                }
            //
            //                //加载目标资源完成的回调
            //                callback(obj);
            //            });

            LoadResReturnAssetBundle(realName, (bundle) =>
            {
                int index = realName.LastIndexOf("/");
                string assetName = realName.Substring(index + 1);
                assetName = assetName.Replace(assetTail, "");
                UnityEngine.Object obj = bundle.LoadAsset(assetName);//LoadAsset(name）,这个name没有后缀,eg:panel

                //加载目标资源完成的回调
                callback(obj);
            });

        };

        LoadDependenceAssets(name, action);
    }

    /// <summary>
    /// 加载目标资源的依赖资源
    /// </summary>
    /// <param name="targetAssetName"></param>
    /// <param name="action"></param>
    private void LoadDependenceAssets(string targetAssetName, Action<List<AssetBundle>> action)
    {
        Debug.Log("要加载的目标资源:" + targetAssetName);//ui/panel.unity3d
        Action<AssetBundleManifest> dependenceAction = (manifest) =>
        {
            List<AssetBundle> depenceAssetBundles = new List<AssetBundle>();//用来存放加载出来的依赖资源的AssetBundle

            string[] dependences = manifest.GetAllDependencies(targetAssetName);
            Debug.Log("依赖文件个数：" + dependences.Length);
            int length = dependences.Length;
            int finishedCount = 0;
            if (length == 0)
            {
                //没有依赖
                action(depenceAssetBundles);
            }
            else
            {
                //有依赖，加载所有依赖资源
                for (int i = 0; i < length; i++)
                {
                    string dependenceAssetName = dependences[i];
                    dependenceAssetName = GetRuntimePlatform() + "/" + dependenceAssetName;//eg:Windows/altas/heroiconatlas.unity3d

                    //                    //加载，加到assetpool
                    //                    LoadResReturnWWW(dependenceAssetName, (www) =>
                    //                    {
                    //                        int index = dependenceAssetName.LastIndexOf("/");
                    //                        string assetName = dependenceAssetName.Substring(index + 1);
                    //                        assetName = assetName.Replace(assetTail, "");
                    //                        AssetBundle assetBundle = www.assetBundle;
                    //                        UnityEngine.Object obj = assetBundle.LoadAsset(assetName);
                    //                        //assetBundle.Unload(false);
                    //                        depenceAssetBundles.Add(assetBundle);
                    //
                    //                        finishedCount++;
                    //
                    //                        if (finishedCount == length)
                    //                        {
                    //                            //依赖都加载完了
                    //                            action(depenceAssetBundles);
                    //                        }
                    //                    });

                    //加载，加到assetpool
                    LoadResReturnAssetBundle(dependenceAssetName, (bundle) =>
                                     {
                                         int index = dependenceAssetName.LastIndexOf("/");
                                         string assetName = dependenceAssetName.Substring(index + 1);
                                         assetName = assetName.Replace(assetTail, "");

                                         UnityEngine.Object obj = bundle.LoadAsset(assetName);
                                         //assetBundle.Unload(false);
                                         depenceAssetBundles.Add(bundle);

                                         finishedCount++;

                                         if (finishedCount == length)
                                         {
                                             //依赖都加载完了
                                             action(depenceAssetBundles);
                                         }
                                     });
                }
            }
        };
        LoadAssetBundleManifest(dependenceAction);
    }

    /// <summary>
    /// 加载AssetBundleManifest
    /// </summary>
    /// <param name="action"></param>
    private void LoadAssetBundleManifest(Action<AssetBundleManifest> action)
    {
        string manifestName = this.GetRuntimePlatform();
        manifestName = manifestName + "/" + manifestName;//eg:Windows/Windows
                                                         //        LoadResReturnWWW(manifestName, (www) =>
                                                         //        {
                                                         //            AssetBundle assetBundle = www.assetBundle;
                                                         //            UnityEngine.Object obj = assetBundle.LoadAsset("AssetBundleManifest");
                                                         //            assetBundle.Unload(false);
                                                         //            AssetBundleManifest manif = obj as AssetBundleManifest;
                                                         //            action(manif);
                                                         //        });

        LoadResReturnAssetBundle(manifestName, (bundle) =>
        {
            UnityEngine.Object obj = bundle.LoadAsset("AssetBundleManifest");
            AssetBundleManifest manif = obj as AssetBundleManifest;
            action(manif);
        });
    }
    #endregion

    private string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = "file://" + Application.streamingAssetsPath + "/";
                break;
        }

        return path;
    }


    #region ExcuteLoader
    private void LoadResReturnAssetBundle(string name, Action<AssetBundle> callback)
    {
        if (m_assetBundleCache.ContainsKey(name))
        {
            callback(m_assetBundleCache[name]);
            return;
        }

        LoadResReturnWWW(name, (www) =>
        {
            AssetBundle ab = www.assetBundle;
            m_assetBundleCache.Add(name, ab);
            if (callback != null)
            {
                callback(ab);
            }
        });
    }

    public void LoadResReturnWWW(string name, Action<WWW> callback)
    {
        string path = AppContentPath() + name;
        Debug.Log("加载：" + path);
        StartCoroutine(LoaderRes(path, callback));
    }

    IEnumerator LoaderRes(string path, Action<WWW> callback)
    {
        while (m_resLoading.ContainsKey(path))
        {
            yield return null;
        }

        m_resLoading.Add(path, true);

        WWW www = new WWW(path);
        yield return www;

        if (!String.IsNullOrEmpty(www.error))
            Debug.LogError(www.error);

        if (www.isDone && String.IsNullOrEmpty(www.error))
        {
            callback(www);
            m_resLoading.Remove(path);
        }
    }
    #endregion

    #region Util
    /// <summary>
    /// 平台对应文件夹
    /// </summary>
    /// <returns></returns>
    private string GetRuntimePlatform()
    {
        //string platform = "";
        //if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    platform = "Windows";
        //}
        //else if (Application.platform == RuntimePlatform.Android)
        //{
        //    platform = "Android";
        //}
        //else if (Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    platform = "IOS";
        //}
        //else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
        //{
        //    platform = "OSX";
        //}
        //return platform;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        return "Windows";
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
         return "OSX";
#elif UNITY_IOS
         return "IOS";
#elif UNITY_ANDROID
         return "Android";
#endif

    }
    #endregion
}
