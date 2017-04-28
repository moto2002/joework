using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ShiHuanJue.Event;

public class SceneBase
{
    public string m_sceneName;
    public Queue<string> m_needLoadQueue;
    private int m_maxCount;
    private int m_loadedCount;
    public Dictionary<string, UnityEngine.Object> m_cacheObj;
    public UnityEngine.Object GetCacheObj(string name)
    {
        if (name.Contains("/"))
        {
            name = name.Substring(name.LastIndexOf("/") + 1).ToLower();
        }

        if (m_cacheObj.ContainsKey(name))
            return m_cacheObj[name];
        else
        {
            Debug.Log(name + ":not in cachePool");
            return null;
        }
    }

    public SceneBase()
    {
        this.m_needLoadQueue = new Queue<string>();
        this.m_cacheObj = new Dictionary<string, UnityEngine.Object>();
    }

    public virtual void Init()
    {
        this.m_maxCount = this.m_needLoadQueue.Count;
        this.m_loadedCount = 0;

        this.LoadSceneAssetBundle(() =>
        {
            Debug.Log("scene assetbundle 加载完毕");

            this.LoadSceneNeedGameObject(this.SceneLoadedOK);
        });

    }

    private void LoadSceneAssetBundle(Action callback)
    {
        if (!string.IsNullOrEmpty(this.m_sceneName))
        {
            ResourcesManager.Instance.LoadSceneAssetBundle(this.m_sceneName, () =>
            {
                callback();
            });
        }
        else
        {
            callback();
        }
    }

    //去加载场景需要的资源
    private void LoadSceneNeedGameObject(Action callback)
    {
        if (this.m_needLoadQueue.Count > 0)
        {
            string curr = this.m_needLoadQueue.Dequeue();
            ResourcesManager.Instance.LoadAssetBundle(curr, (obj) =>
            {
                int index = curr.LastIndexOf("/");
                string assetName = curr.Substring(index + 1);
                this.m_cacheObj.Add(assetName, obj);

                this.m_loadedCount++;
                float progress = (float)this.m_loadedCount / (float)this.m_maxCount;
                Debug.Log("资源加载进度:" + this.m_loadedCount + "/" + this.m_maxCount + "百分比:" + progress);

                LoadSceneNeedGameObject(callback);
            });
        }
        else
        {
            Debug.Log("场景资源加载完毕");

            //TimerManager.Instance.StartTimer(1, null, () => { callback(); });
            callback();
        }
    }

    public virtual void SceneLoadedOK()
    {

    }

    public virtual void Destroy()
    {
        this.m_sceneName = null;
        this.m_needLoadQueue.Clear();
        this.m_cacheObj.Clear();
    }

}
