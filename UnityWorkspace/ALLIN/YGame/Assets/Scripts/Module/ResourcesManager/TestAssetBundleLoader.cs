using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestAssetBundleLoader : MonoBehaviour
{
    Dictionary<string, GameObject> GameObjectPool = new Dictionary<string, GameObject>();

    void Start()
    {
        //要加载的资源的队列
        Queue<string> needLoadQueue = new Queue<string>();
        needLoadQueue.Enqueue("ui/plane");
        needLoadQueue.Enqueue("ui/cube");
        needLoadQueue.Enqueue("ui/sphere");
        needLoadQueue.Enqueue("ui/plane");
        Load(needLoadQueue);
    }

    void Load(Queue<string> needLoadQueue)
    {
        if (needLoadQueue.Count > 0)
        {
            string needLoadAssetName = needLoadQueue.Dequeue();
            ResourcesManager.Instance.LoadAssetBundle(needLoadAssetName, (obj) =>
            {
                GameObject go = GameObject.Instantiate(obj) as GameObject;
                int index = needLoadAssetName.LastIndexOf("/");
                string assetName = needLoadAssetName.Substring(index + 1);

                //加载出来的GameObject放到GameObjectPool存储
                if(!GameObjectPool.ContainsKey(assetName))
                   GameObjectPool.Add(assetName, go);
                else
                    Debug.LogError("already exist");
                Load(needLoadQueue);
            });
        }
        else
        {
            Debug.Log("all finished");
        }
    }
}
