using Mogo.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AssetCacheMgr
{
    private static ILoadAsset m_assetMgr;
    private static Dictionary<int, string> m_gameObjectNameMapping = new Dictionary<int, string>();
    private static Dictionary<int, string> m_resourceDic = new Dictionary<int, string>();

    public void ClearLoadAssetTasks()
    {
    }

    public static void ForceClear(HashSet<string> CityResources)
    {
        m_gameObjectNameMapping.Clear();
        m_assetMgr.ForceClear();
        m_resourceDic.Clear();
    }

    public static void GetInstance(string resourceName, Action<string, int, UnityEngine.Object> loaded)
    {
        m_assetMgr.LoadInstance(resourceName, delegate (string pref, int guid, UnityEngine.Object go) {
            if (guid != -1)
            {
                m_gameObjectNameMapping.Add(guid, resourceName);
            }
            if (loaded != null)
            {
                loaded(pref, guid, go);
            }
        });
    }

    public static void GetInstance(string resourceName, uint duration)
    {
        GetInstance(resourceName, duration, null);
    }

    public static void GetInstance(string resourceName, uint duration, Action<string, int, UnityEngine.Object> loaded)
    {
        m_assetMgr.LoadInstance(resourceName, delegate (string pref, int guid, UnityEngine.Object go) {
            if (loaded != null)
            {
                loaded(pref, guid, go);
            }
            TimerHeap.AddTimer(duration, 0, () => ReleaseInstance(go, true));
        });
    }

    public static void GetInstanceAutoRelease(string resourceName, Action<string, int, UnityEngine.Object> loaded)
    {
        GetInstance(resourceName, delegate (string resource, int guid, UnityEngine.Object go) {
            UnloadAssetbundle(resourceName);
            if (loaded != null)
            {
                loaded(resource, guid, go);
            }
        });
    }

    public static void GetInstances(string[] resourcesName, Action<UnityEngine.Object[]> loaded)
    {
        UnityEngine.Object[] objs;
        int count;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new UnityEngine.Object[resourcesName.Length];
            count = 0;
            for (int i = 0; i < resourcesName.Length; i++)
            {
                int index = i;
                string resourceName = resourcesName[index];
                GetInstance(resourceName, delegate (string resource, int guid, UnityEngine.Object obj) {
                    objs[index] = obj;
                    count++;
                    if ((count == resourcesName.Length) && (loaded != null))
                    {
                        loaded(objs);
                    }
                });
            }
        }
    }

    public static void GetInstancesAutoRelease(string[] resourcesName, Action<UnityEngine.Object[]> loaded)
    {
        GetInstances(resourcesName, delegate (UnityEngine.Object[] gos) {
            UnloadAssetbundles(resourcesName);
            if (loaded != null)
            {
                loaded(gos);
            }
        });
    }

    public static UnityEngine.Object GetLocalInstance(string resourceName)
    {
        return m_assetMgr.LoadLocalInstance(resourceName);
    }

    public static UnityEngine.Object GetLocalResource(string resourceName)
    {
        return m_assetMgr.LoadLocalAsset(resourceName);
    }

    public static void GetNoCacheInstance(string resourceName, Action<string, int, UnityEngine.Object> loaded, Action<float> progress = null)
    {
        m_assetMgr.LoadInstance(resourceName, loaded, progress);
    }

    public static void GetNoCacheInstanceAutoRelease(string resourceName, Action<string, int, UnityEngine.Object> loaded)
    {
        GetNoCacheInstance(resourceName, delegate (string resource, int guid, UnityEngine.Object go) {
            UnloadAssetbundle(resourceName);
            if (loaded != null)
            {
                loaded(resource, guid, go);
            }
        }, null);
    }

    public static void GetNoCacheResource(string resourceName, Action<UnityEngine.Object> loaded, Action<float> progress = null, byte priority = 2)
    {
        m_assetMgr.LoadAsset(resourceName, priority, loaded, progress);
    }

    public static void GetNoCacheResourceAutoRelease(string resourceName, Action<UnityEngine.Object> loaded, Action<float> progress = null, byte priority = 2)
    {
        m_assetMgr.LoadAsset(resourceName, priority, delegate (UnityEngine.Object obj) {
            UnloadAssetbundle(resourceName);
            if (loaded != null)
            {
                loaded(obj);
            }
        }, progress);
    }

    public static void GetNoCacheResources(string[] resourcesName, Action<UnityEngine.Object[]> loaded, Action<float> progress = null, byte priority = 2)
    {
        UnityEngine.Object[] objs;
        int count;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new UnityEngine.Object[resourcesName.Length];
            count = 0;
            for (int i = 0; i < resourcesName.Length; i++)
            {
                Action<float> action2 = null;
                int index = i;
                string resourceName = resourcesName[index];
                Action<float> action = null;
                if (progress != null)
                {
                    if (action2 == null)
                    {
                        action2 = pg => progress((pg + index) / ((float) resourcesName.Length));
                    }
                    action = action2;
                }
                GetNoCacheResource(resourceName, delegate (UnityEngine.Object obj) {
                    objs[index] = obj;
                    count++;
                    if ((count == resourcesName.Length) && (loaded != null))
                    {
                        loaded(objs);
                    }
                }, action, priority);
            }
        }
    }

    public static void GetNoCacheResourcesAutoRelease(string[] resourcesName, Action<UnityEngine.Object[]> loaded, Action<float> progress = null, byte priority = 2)
    {
        GetNoCacheResources(resourcesName, delegate (UnityEngine.Object[] gos) {
            UnloadAssetbundles(resourcesName);
            if (loaded != null)
            {
                loaded(gos);
            }
        }, progress, priority);
    }

    public static void GetResource(string resourceName, Action<UnityEngine.Object> loaded, Action<float> progress = null, byte priority = 2)
    {
        m_assetMgr.LoadAsset(resourceName, priority, delegate (UnityEngine.Object obj) {
            if (obj != 0)
            {
                int key = obj.GetInstanceID();
                if (!m_resourceDic.ContainsKey(key))
                {
                    m_resourceDic.Add(key, resourceName);
                }
            }
            if (loaded != null)
            {
                loaded(obj);
            }
        }, progress);
    }

    public static void GetResourceAutoRelease(string resourceName, Action<UnityEngine.Object> loaded, Action<float> progress = null, byte priority = 2)
    {
        m_assetMgr.LoadAsset(resourceName, priority, delegate (UnityEngine.Object obj) {
            UnloadAssetbundle(resourceName);
            if (obj != 0)
            {
                int key = obj.GetInstanceID();
                if (!m_resourceDic.ContainsKey(key))
                {
                    m_resourceDic.Add(key, resourceName);
                }
            }
            if (loaded != null)
            {
                loaded(obj);
            }
        }, progress);
    }

    public static void GetResources(string[] resourcesName, Action<UnityEngine.Object[]> loaded, Action<float> progress = null, byte priority = 2)
    {
        UnityEngine.Object[] objs;
        int count;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new UnityEngine.Object[resourcesName.Length];
            count = 0;
            for (int i = 0; i < resourcesName.Length; i++)
            {
                Action<float> action2 = null;
                int index = i;
                string resourceName = resourcesName[index];
                Action<float> action = null;
                if (progress != null)
                {
                    if (action2 == null)
                    {
                        action2 = pg => progress((pg + index) / ((float) resourcesName.Length));
                    }
                    action = action2;
                }
                GetResource(resourceName, delegate (UnityEngine.Object obj) {
                    objs[index] = obj;
                    count++;
                    if ((count == resourcesName.Length) && (loaded != null))
                    {
                        loaded(objs);
                    }
                }, action, priority);
            }
        }
    }

    public static void GetResourcesAutoRelease(string[] resourcesName, Action<UnityEngine.Object[]> loaded, Action<float> progress = null, byte priority = 2)
    {
        GetResources(resourcesName, delegate (UnityEngine.Object[] gos) {
            UnloadAssetbundles(resourcesName);
            if (loaded != null)
            {
                loaded(gos);
            }
        }, progress, priority);
    }

    public static void GetSceneResource(string resourceName, Action<UnityEngine.Object> loaded)
    {
        if (string.IsNullOrEmpty(resourceName))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            m_assetMgr.LoadSceneAsset(resourceName, delegate (UnityEngine.Object obj) {
                if (obj != 0)
                {
                    int key = obj.GetInstanceID();
                    if (!m_resourceDic.ContainsKey(key))
                    {
                        m_resourceDic.Add(key, resourceName);
                    }
                }
                if (loaded != null)
                {
                    loaded(obj);
                }
            });
        }
    }

    public static void GetSecondResource(string resourceName, Action<UnityEngine.Object> loaded, Action<float> progress = null)
    {
        m_assetMgr.SecondLoadAsset(resourceName, delegate (UnityEngine.Object obj) {
            if (obj != 0)
            {
                int key = obj.GetInstanceID();
                if (!m_resourceDic.ContainsKey(key))
                {
                    m_resourceDic.Add(key, resourceName);
                }
            }
            if (loaded != null)
            {
                loaded(obj);
            }
        }, progress);
    }

    public static void GetSecondResources(string[] resourcesName, Action<UnityEngine.Object[]> loaded, Action<float> progress = null)
    {
        UnityEngine.Object[] objs;
        int count;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new UnityEngine.Object[resourcesName.Length];
            count = 0;
            for (int i = 0; i < resourcesName.Length; i++)
            {
                Action<float> action2 = null;
                int index = i;
                string resourceName = resourcesName[index];
                Action<float> action = null;
                if (progress != null)
                {
                    if (action2 == null)
                    {
                        action2 = pg => progress((pg + index) / ((float) resourcesName.Length));
                    }
                    action = action2;
                }
                GetSecondResource(resourceName, delegate (UnityEngine.Object obj) {
                    objs[index] = obj;
                    count++;
                    if ((count == resourcesName.Length) && (loaded != null))
                    {
                        loaded(objs);
                    }
                }, action);
            }
        }
    }

    public static void GetSecondUIResource(string resourceName, Action<UnityEngine.Object> loaded, Action<float> progress = null)
    {
        m_assetMgr.SecondLoadUIAsset(resourceName, delegate (UnityEngine.Object obj) {
            if (obj != 0)
            {
                int key = obj.GetInstanceID();
                if (!m_resourceDic.ContainsKey(key))
                {
                    m_resourceDic.Add(key, resourceName);
                }
            }
            if (loaded != null)
            {
                loaded(obj);
            }
        }, progress);
    }

    public static void GetSecondUIResources(string[] resourcesName, Action<UnityEngine.Object[]> loaded, Action<float> progress = null)
    {
        UnityEngine.Object[] objs;
        int count;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new UnityEngine.Object[resourcesName.Length];
            count = 0;
            for (int i = 0; i < resourcesName.Length; i++)
            {
                Action<float> action2 = null;
                int index = i;
                string resourceName = resourcesName[index];
                Action<float> action = null;
                if (progress != null)
                {
                    if (action2 == null)
                    {
                        action2 = pg => progress((pg + index) / ((float) resourcesName.Length));
                    }
                    action = action2;
                }
                GetSecondUIResource(resourceName, delegate (UnityEngine.Object obj) {
                    objs[index] = obj;
                    count++;
                    if ((count == resourcesName.Length) && (loaded != null))
                    {
                        loaded(objs);
                    }
                }, action);
            }
        }
    }

    public static void GetUIInstance(string resourceName, Action<string, int, UnityEngine.Object> loaded, Action<float> progress = null)
    {
        m_assetMgr.LoadUIAsset(resourceName, delegate (UnityEngine.Object o) {
            UnityEngine.Object obj2 = null;
            int instanceID = -1;
            if (o != 0)
            {
                obj2 = UnityEngine.Object.Instantiate(o);
                instanceID = obj2.GetInstanceID();
                m_gameObjectNameMapping.Add(instanceID, resourceName);
            }
            if (loaded != null)
            {
                loaded(resourceName, instanceID, obj2);
            }
        }, progress);
    }

    public static void GetUIInstanceNoCache(string resourceName, Action<string, int, UnityEngine.Object> loaded, Action<float> progress = null)
    {
        m_assetMgr.LoadUIAsset(resourceName, delegate (UnityEngine.Object o) {
            UnityEngine.Object obj2 = null;
            int num = -1;
            if (o != 0)
            {
                obj2 = UnityEngine.Object.Instantiate(o);
            }
            if (loaded != null)
            {
                loaded(resourceName, num, obj2);
            }
        }, progress);
    }

    public static void GetUIInstances(string[] resourcesName, Action<UnityEngine.Object[]> loaded)
    {
        UnityEngine.Object[] objs;
        int count;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new UnityEngine.Object[resourcesName.Length];
            count = 0;
            for (int i = 0; i < resourcesName.Length; i++)
            {
                int index = i;
                string resourceName = resourcesName[index];
                GetUIInstance(resourceName, delegate (string resource, int guid, UnityEngine.Object obj) {
                    objs[index] = obj;
                    count++;
                    if ((count == resourcesName.Length) && (loaded != null))
                    {
                        loaded(objs);
                    }
                }, null);
            }
        }
    }

    public static void GetUIResource(string resourceName, Action<UnityEngine.Object> loaded, Action<float> progress = null, byte priority = 2)
    {
        m_assetMgr.LoadUIAsset(resourceName, priority, delegate (UnityEngine.Object obj) {
            if (obj != 0)
            {
                int key = obj.GetInstanceID();
                if (!m_resourceDic.ContainsKey(key))
                {
                    m_resourceDic.Add(key, resourceName);
                }
            }
            if (loaded != null)
            {
                loaded(obj);
            }
        }, progress);
    }

    public static void GetUIResources(string[] resourcesName, Action<UnityEngine.Object[]> loaded, Action<float> progress = null, byte priority = 2)
    {
        UnityEngine.Object[] objs;
        int count;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new UnityEngine.Object[resourcesName.Length];
            count = 0;
            for (int i = 0; i < resourcesName.Length; i++)
            {
                Action<float> action2 = null;
                int index = i;
                string resourceName = resourcesName[index];
                Action<float> action = null;
                if (progress != null)
                {
                    if (action2 == null)
                    {
                        action2 = pg => progress((pg + index) / ((float) resourcesName.Length));
                    }
                    action = action2;
                }
                GetUIResource(resourceName, delegate (UnityEngine.Object obj) {
                    objs[index] = obj;
                    count++;
                    if ((count == resourcesName.Length) && (loaded != null))
                    {
                        loaded(objs);
                    }
                }, action, priority);
            }
        }
    }

    public static void ReleaseInstance(UnityEngine.Object go, bool releaseAsset = true)
    {
        if (go != 0)
        {
            int instanceID = go.GetInstanceID();
            UnityEngine.Object.Destroy(go);
            if (m_gameObjectNameMapping.ContainsKey(instanceID))
            {
                if (releaseAsset)
                {
                    m_assetMgr.Release(m_gameObjectNameMapping[instanceID]);
                }
                m_gameObjectNameMapping.Remove(instanceID);
            }
        }
    }

    public static void ReleaseLocalInstance(UnityEngine.Object go)
    {
        if (go != 0)
        {
            UnityEngine.Object.Destroy(go);
        }
    }

    public static void ReleaseLocalResource(UnityEngine.Object go)
    {
        Resources.UnloadAsset(go);
    }

    public static void ReleaseResource(string resourceName, bool releaseAsset = true)
    {
        if (releaseAsset)
        {
            m_assetMgr.Release(resourceName);
        }
    }

    public static void ReleaseResource(UnityEngine.Object obj, bool releaseAsset = true)
    {
        if (obj != null)
        {
            int instanceID = obj.GetInstanceID();
            if (m_resourceDic.ContainsKey(instanceID))
            {
                string prefab = m_resourceDic[instanceID];
                if (releaseAsset && m_assetMgr.Release(prefab))
                {
                    m_resourceDic.Remove(instanceID);
                }
            }
        }
    }

    public static void ReleaseResourceImmediate(string resourceName)
    {
        m_assetMgr.Release(resourceName, true);
    }

    public static void ReleaseResourceImmediate(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            int instanceID = obj.GetInstanceID();
            if (m_resourceDic.ContainsKey(instanceID))
            {
                string prefab = m_resourceDic[instanceID];
                m_assetMgr.Release(prefab, true);
                m_resourceDic.Remove(instanceID);
            }
        }
    }

    public static void ReleaseResourcesImmediate(string[] resourcesName)
    {
        if (resourcesName != null)
        {
            foreach (string str in resourcesName)
            {
                ReleaseResourceImmediate(str);
            }
            GC.Collect();
        }
    }

    public static void ReleasesResource(string[] resourcesName, bool releaseAsset = true)
    {
        if (resourcesName != null)
        {
            foreach (string str in resourcesName)
            {
                ReleaseResource(str, releaseAsset);
            }
        }
    }

    public static UnityEngine.Object SynGetInstance(string resourceName)
    {
        return m_assetMgr.SynLoadInstance(resourceName);
    }

    public static GameObject SynGetInstance(UnityEngine.Object resource)
    {
        return (UnityEngine.Object.Instantiate(resource) as GameObject);
    }

    public static UnityEngine.Object SynGetResource(string resourceName)
    {
        return m_assetMgr.SynLoadAsset(resourceName);
    }

    public static void SynReleaseInstance(UnityEngine.Object go)
    {
        UnityEngine.Object.Destroy(go);
    }

    public static void UnloadAsset(UnityEngine.Object go, bool releaseAsset = true)
    {
        if (go != 0)
        {
            int instanceID = go.GetInstanceID();
            if (m_gameObjectNameMapping.ContainsKey(instanceID))
            {
                if (releaseAsset)
                {
                    m_assetMgr.UnloadAsset(m_gameObjectNameMapping[instanceID]);
                }
            }
            else
            {
                LoggerHelper.Warning("go not in mapping: " + go.name, true);
            }
        }
    }

    public static void UnloadAssetbundle(string resourceName)
    {
        m_assetMgr.Release(resourceName, false);
    }

    public static void UnloadAssetbundles(string[] resourcesName)
    {
        if (resourcesName != null)
        {
            foreach (string str in resourcesName)
            {
                m_assetMgr.Release(str, false);
            }
            GC.Collect();
        }
    }

    public static ILoadAsset AssetMgr
    {
        get
        {
            return m_assetMgr;
        }
        set
        {
            m_assetMgr = value;
        }
    }

    public static Dictionary<int, string> GameObjectNameMapping
    {
        get
        {
            return m_gameObjectNameMapping;
        }
    }

    public static Dictionary<int, string> ResourceDic
    {
        get
        {
            return m_resourceDic;
        }
    }
}

