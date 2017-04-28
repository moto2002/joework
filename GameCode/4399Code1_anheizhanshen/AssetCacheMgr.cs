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

    public static void GetInstance(string resourceName, Action<string, int, Object> loaded)
    {
        m_assetMgr.LoadInstance(resourceName, delegate (string pref, int guid, Object go) {
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

    public static void GetInstance(string resourceName, uint duration, Action<string, int, Object> loaded)
    {
        m_assetMgr.LoadInstance(resourceName, delegate (string pref, int guid, Object go) {
            if (loaded != null)
            {
                loaded(pref, guid, go);
            }
            TimerHeap.AddTimer(duration, 0, () => ReleaseInstance(go, true));
        });
    }

    public static void GetInstanceAutoRelease(string resourceName, Action<string, int, Object> loaded)
    {
        GetInstance(resourceName, delegate (string resource, int guid, Object go) {
            UnloadAssetbundle(resourceName);
            if (loaded != null)
            {
                loaded(resource, guid, go);
            }
        });
    }

    public static void GetInstances(string[] resourcesName, Action<Object[]> loaded)
    {
        Object[] objs;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new Object[resourcesName.Length];
            for (int i = 0; i < resourcesName.Length; i++)
            {
                int index = i;
                string resourceName = resourcesName[index];
                GetInstance(resourceName, delegate (string resource, int guid, Object obj) {
                    objs[index] = obj;
                    if ((index == (resourcesName.Length - 1)) && (loaded != null))
                    {
                        loaded(objs);
                    }
                });
            }
        }
    }

    public static void GetInstancesAutoRelease(string[] resourcesName, Action<Object[]> loaded)
    {
        GetInstances(resourcesName, delegate (Object[] gos) {
            UnloadAssetbundles(resourcesName);
            if (loaded != null)
            {
                loaded(gos);
            }
        });
    }

    public static Object GetLocalInstance(string resourceName)
    {
        return m_assetMgr.LoadLocalInstance(resourceName);
    }

    public static Object GetLocalResource(string resourceName)
    {
        return m_assetMgr.LoadLocalAsset(resourceName);
    }

    public static void GetNoCacheInstance(string resourceName, Action<string, int, Object> loaded)
    {
        m_assetMgr.LoadInstance(resourceName, loaded);
    }

    public static void GetNoCacheResource(string resourceName, Action<Object> loaded, Action<float> progress = null)
    {
        m_assetMgr.LoadAsset(resourceName, loaded, progress);
    }

    public static void GetResource(string resourceName, Action<Object> loaded, Action<float> progress = null)
    {
        m_assetMgr.LoadAsset(resourceName, delegate (Object obj) {
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

    public static void GetResourceAutoRelease(string resourceName, Action<Object> loaded)
    {
        m_assetMgr.LoadAsset(resourceName, delegate (Object obj) {
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
        });
    }

    public static void GetResources(string[] resourcesName, Action<Object[]> loaded, Action<float> progress = null)
    {
        Object[] objs;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new Object[resourcesName.Length];
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
                GetResource(resourceName, delegate (Object obj) {
                    objs[index] = obj;
                    if ((index == (resourcesName.Length - 1)) && (loaded != null))
                    {
                        loaded(objs);
                    }
                }, action);
            }
        }
    }

    public static void GetResourcesAutoRelease(string[] resourcesName, Action<Object[]> loaded, Action<float> progress = null)
    {
        GetResources(resourcesName, delegate (Object[] gos) {
            UnloadAssetbundles(resourcesName);
            if (loaded != null)
            {
                loaded(gos);
            }
        }, progress);
    }

    public static void GetSceneInstance(string resourceName, Action<string, int, Object> loaded, Action<float> progress)
    {
        m_assetMgr.LoadSceneInstance(resourceName, delegate (string pref, int guid, Object go) {
            if (guid != -1)
            {
                m_gameObjectNameMapping.Add(guid, resourceName);
            }
            if (loaded != null)
            {
                loaded(pref, guid, go);
            }
        }, progress);
    }

    public static void GetSceneResource(string resourceName, Action<Object> loaded)
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
            m_assetMgr.LoadSceneAsset(resourceName, delegate (Object obj) {
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

    public static void GetSecondtUIResources(string[] resourcesName, Action<Object[]> loaded)
    {
        Object[] objs;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new Object[resourcesName.Length];
            for (int i = 0; i < resourcesName.Length; i++)
            {
                int index = i;
                string resourceName = resourcesName[index];
                GetSecondUIResource(resourceName, delegate (Object obj) {
                    objs[index] = obj;
                    if ((index == (resourcesName.Length - 1)) && (loaded != null))
                    {
                        loaded(objs);
                    }
                }, null);
            }
        }
    }

    public static void GetSecondUIResource(string resourceName, Action<Object> loaded, Action<float> progress = null)
    {
        m_assetMgr.SecondLoadUIAsset(resourceName, delegate (Object obj) {
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

    public static void GetUIInstance(string resourceName, Action<string, int, Object> loaded, Action<float> progress = null)
    {
        m_assetMgr.LoadUIAsset(resourceName, delegate (Object o) {
            Object obj2 = null;
            int instanceID = -1;
            if (o != 0)
            {
                obj2 = Object.Instantiate(o);
                instanceID = obj2.GetInstanceID();
                m_gameObjectNameMapping.Add(instanceID, resourceName);
            }
            if (loaded != null)
            {
                loaded(resourceName, instanceID, obj2);
            }
        }, progress);
    }

    public static void GetUIResource(string resourceName, Action<Object> loaded, Action<float> progress = null)
    {
        m_assetMgr.LoadUIAsset(resourceName, delegate (Object obj) {
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

    public static void GetUIResources(string[] resourcesName, Action<Object[]> loaded)
    {
        Object[] objs;
        if ((resourcesName == null) || (resourcesName.Length == 0))
        {
            if (loaded != null)
            {
                loaded(null);
            }
        }
        else
        {
            objs = new Object[resourcesName.Length];
            for (int i = 0; i < resourcesName.Length; i++)
            {
                int index = i;
                string resourceName = resourcesName[index];
                GetUIResource(resourceName, delegate (Object obj) {
                    objs[index] = obj;
                    if ((index == (resourcesName.Length - 1)) && (loaded != null))
                    {
                        loaded(objs);
                    }
                }, null);
            }
        }
    }

    public static void ReleaseInstance(Object go, bool releaseAsset = true)
    {
        if (go != 0)
        {
            int instanceID = go.GetInstanceID();
            Object.Destroy(go);
            if (m_gameObjectNameMapping.ContainsKey(instanceID))
            {
                if (releaseAsset)
                {
                    m_assetMgr.Release(m_gameObjectNameMapping[instanceID]);
                }
                m_gameObjectNameMapping.Remove(instanceID);
            }
            else
            {
                LoggerHelper.Warning("go not in mapping: " + go.get_name(), true);
            }
        }
    }

    public static void ReleaseLocalInstance(Object go)
    {
        if (go != 0)
        {
            Object.Destroy(go);
        }
    }

    public static void ReleaseLocalResource(Object go)
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

    public static void ReleaseResource(Object obj, bool releaseAsset = true)
    {
        if (obj != null)
        {
            int instanceID = obj.GetInstanceID();
            if (m_resourceDic.ContainsKey(instanceID))
            {
                string prefab = m_resourceDic[instanceID];
                if (releaseAsset)
                {
                    m_assetMgr.Release(prefab);
                    m_resourceDic.Remove(instanceID);
                }
            }
        }
    }

    public static void ReleaseResourceImmediate(string resourceName)
    {
        m_assetMgr.Release(resourceName, true);
    }

    public static void ReleaseResourceImmediate(Object obj)
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

    public static Object SynGetInstance(string resourceName)
    {
        return m_assetMgr.SynLoadInstance(resourceName);
    }

    public static GameObject SynGetInstance(Object resource)
    {
        return (Object.Instantiate(resource) as GameObject);
    }

    public static Object SynGetResource(string resourceName)
    {
        return m_assetMgr.SynLoadAsset(resourceName);
    }

    public static void SynReleaseInstance(Object go)
    {
        Object.Destroy(go);
    }

    public static void UnloadAsset(Object go, bool releaseAsset = true)
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
                LoggerHelper.Warning("go not in mapping: " + go.get_name(), true);
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

    [StructLayout(LayoutKind.Sequential)]
    public struct SResourceRecord
    {
        public int nCreatedTimes;
        public int nDestroyTimes;
    }
}

