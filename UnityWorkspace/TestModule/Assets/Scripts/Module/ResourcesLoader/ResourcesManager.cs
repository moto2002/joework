using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ResourcesManager : Singleton<ResourcesManager>
{
    //资源的后缀名,eg:.unity3d
    string assetTail;

    //正在加载的资源
    Dictionary<string, bool> m_resLoading = new Dictionary<string, bool>();

    //已经加载的Assetbundle
    Dictionary<string, AssetBundle> m_assetBundleCache = new Dictionary<string, AssetBundle>();

    //标记已经加载的bundle中哪些是要常驻内存的，标记为true的bundle加载一次后就不会被清除
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

    /// <summary>
    /// 加载目标资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public void LoadAssetBundle(string name, Action<UnityEngine.Object> callback)
    {
        assetTail = AppConst.ExtName;
        name = name + assetTail;//eg:ui/panel.unity3d
        name = name.ToLower();

        Action<List<AssetBundle>> action = (depenceAssetBundles) =>
        {
            string realName = this.GetRuntimePlatform() + "/" + name;//eg:Windows/ui/panel.unity3d

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
                //有依赖，加载所有依赖资源 for (int i = 0; i < length; i++),这里从后往前加载，让最低层级的依赖资源先加载到内存，才能组合出上一层的资源
                for (int i = length - 1; i >= 0; i--)
                {
                    string dependenceAssetName = dependences[i];
                    dependenceAssetName = GetRuntimePlatform() + "/" + dependenceAssetName;//eg:Windows/altas/heroiconatlas.unity3d

                    //加载，加到assetpool
                    LoadResReturnAssetBundle(dependenceAssetName, (bundle) =>
                    {
                        int index = dependenceAssetName.LastIndexOf("/");
                        string assetName = dependenceAssetName.Substring(index + 1);
                        assetName = assetName.Replace(assetTail, "");
                        UnityEngine.Object obj = bundle.LoadAsset(assetName);
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

        LoadResReturnAssetBundle(manifestName, (bundle) =>
        {
            UnityEngine.Object obj = bundle.LoadAsset("AssetBundleManifest");
            AssetBundleManifest manif = obj as AssetBundleManifest;
            action(manif);
        });
    }

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
        string path = Util.AppContentPath() + name;
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

    /// <summary>
    /// 平台对应文件夹
    /// </summary>
    /// <returns></returns>
    private string GetRuntimePlatform()
    {
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

    #region 场景资源相关
    public void LoadSceneAssetBundle(string sceneName, Action callback)
    {
        assetTail = AppConst.ExtName;
        sceneName = sceneName + assetTail;
        sceneName = sceneName.ToLower();
        sceneName = this.GetRuntimePlatform() + "/" + sceneName;
        LoadResReturnWWW(sceneName, (www) =>
        {
            AssetBundle sceneAB = www.assetBundle;
            string levelName = sceneName.Substring(sceneName.LastIndexOf("/") + 1);
            levelName = levelName.Replace(assetTail, "");
            sceneAB.LoadAsset(levelName);
            StartCoroutine(LoadSceneLevel(levelName, callback));
        });
    }

    private IEnumerator LoadSceneLevel(string levelName, Action callback)
    {
        AsyncOperation async = Application.LoadLevelAsync(levelName);
        yield return async;
        Debug.Log("Loading complete");
        callback();
    }
    #endregion

}
