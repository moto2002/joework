using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;
using System;

namespace LuaFramework
{
    public class PanelManager : Manager
    {
        private Transform parent;

        Transform Parent
        {
            get
            {
                if (parent == null)
                {
                    GameObject go = GameObject.FindWithTag("GuiCamera");
                    if (go != null) parent = go.transform;
                }
                return parent;
            }
        }


#if ASYNC_MODE
        /// <summary>
        /// 创建面板，请求资源管理器
        /// </summary>
        /// <param name="type"></param>
        public void CreatePanel(string name, LuaFunction func = null)
        {

            //string abName = name.ToLower() + AppConst.ExtName;
            //string assetName = name + "Panel";

            //name = name.ToLower();
            string abName = name + AppConst.ExtName;
            name = name.Substring(name.LastIndexOf("/") + 1);
            string assetName = name;
            //Debug.LogError(abName+"{}"+assetName);

            ResManager.LoadPrefab(abName, assetName, delegate(UnityEngine.Object[] objs)
            {
                if (objs.Length == 0) return;
                // Get the asset.
                GameObject prefab = objs[0] as GameObject;

                if (Parent.FindChild(name) != null || prefab == null)
                {
                    return;
                }
                GameObject go = Instantiate(prefab) as GameObject;
                go.name = assetName;
                go.layer = LayerMask.NameToLayer("Default");
                go.transform.SetParent(Parent);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.AddComponent<LuaBehaviour>();

                if (func != null) func.Call(go);
                Debug.LogWarning("CreatePanel::>> " + name + " " + prefab);
            });
        }


        public void CreatePanel(string resPath, Transform parent, Action<GameObject> callback)
        {
            string panelName = resPath.Substring(resPath.LastIndexOf("/") + 1);
            ResManager.LoadPrefabFromResources(resPath,(prefab)=>{
            
                GameObject panelInst = GameObject.Instantiate(prefab) as GameObject;
                panelInst.name = panelName;
                panelInst.layer = LayerMask.NameToLayer("UI");
                panelInst.transform.SetParent(parent);
                panelInst.transform.localScale = Vector3.one;
                panelInst.transform.localPosition = Vector3.zero;

                if (callback != null)
                    callback(panelInst);
            });
  
        }
#else
        /// <summary>
        /// 创建面板，请求资源管理器
        /// </summary>
        /// <param name="type"></param>
        public void CreatePanel(string name, LuaFunction func = null) {
            string assetName = name + "Panel";
            GameObject prefab = ResManager.LoadAsset<GameObject>(name, assetName);
            if (Parent.FindChild(name) != null || prefab == null) {
                return;
            }
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = assetName;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.SetParent(Parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.AddComponent<LuaBehaviour>();

            if (func != null) func.Call(go);
            Debug.LogWarning("CreatePanel::>> " + name + " " + prefab);
        }
#endif
    }
}