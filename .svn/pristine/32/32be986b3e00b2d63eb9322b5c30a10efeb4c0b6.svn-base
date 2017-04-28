namespace TinyTeam.UI
{
    using System;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Each Page Mean one UI 'window'
    /// 3 steps:
    /// instance ui > refresh ui by data > show
    /// 
    /// </summary>
    /// 

    #region enum define
    public enum UIType
    {
        Normal,
        Fixed,
        PopUp,
        None,      //独立的窗口
    }

    public enum UIMode
    {
        DoNothing,
        HideOther,     // 闭其他界面
        NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
        NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
    }

    public enum UICollider
    {
        None,      // 显示该界面不包含碰撞背景
        Normal,    // 碰撞透明背景
        WithBg,    // 碰撞非透明背景
    }
    #endregion


    public abstract class TTUIPage
    {
        public string name = string.Empty;

        //this page's id
        public int id = -1;

        //this page's type
        public UIType type = UIType.Normal;

        //how to show this page.
        public UIMode mode = UIMode.DoNothing;

        //the background collider mode
        public UICollider collider = UICollider.None;

        //path to load ui
        public string uiPath = string.Empty;

        //this ui's gameobject
        public GameObject gameObject;
        public Transform transform;

        //record this ui load mode.async or sync.
        public bool isAsyncUI = false;

        //this page active flag
        protected bool isActived = false;

        //refresh page 's data.
        public object m_data = null;

        //delegate load ui function.
        public static Func<string, Object> delegateSyncLoadUI = null;
        public static Action<string, Action<Object>> delegateAsyncLoadUI = null;

        #region virtual api

        ///When Instance UI Ony Once.
        public virtual void Awake(GameObject go) { }

        ///Show UI Refresh Eachtime.
        public virtual void Refresh() { }

        ///Active this UI
        public virtual void Active()
        {
            this.gameObject.SetActive(true);
            isActived = true;
        }

        /// <summary>
        /// Only Deactive UI
        /// </summary>
        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
            isActived = false;
            //set this page's data null when hide.
            this.m_data = null;
        }

        /// <summary>
        /// destroy ui
        /// </summary>
        public virtual void Close()
        {
            if (this.gameObject != null)
                GameObject.Destroy(this.gameObject);

            this.gameObject = null;
            this.m_data = null;
        }

        #endregion

        #region internal api

        public TTUIPage(UIType type, UIMode mod, UICollider col)
        {
            this.type = type;
            this.mode = mod;
            this.collider = col;
            this.name = this.GetType().ToString();

            //bind special delegate .
            TTUIBind.Bind();
        }

        /// <summary>
        /// Sync Show UI Logic
        /// </summary>
        public void Show()
        {
            //去缓存池中去取。之前已经预加载好了的。

            //1:instance UI
            if (this.gameObject == null && string.IsNullOrEmpty(uiPath) == false)
            {
                GameObject go = null;
                if (delegateSyncLoadUI != null)
                {
                    Object o = delegateSyncLoadUI(uiPath);
                    go = o != null ? GameObject.Instantiate(o) as GameObject : null;
                }
                else
                {
                    go = GameObject.Instantiate(Resources.Load(uiPath)) as GameObject;
                }

                //protected.
                if (go == null)
                {
                    Debug.LogError("[UI] Cant sync load your ui prefab.");
                    return;
                }

                AnchorUIGameObject(go);

                //after instance should awake init.
                Awake(go);

                //mark this ui sync ui
                isAsyncUI = false;
            }

            //:animation or init when active.
            Active();

            //:refresh ui component.
            Refresh();

            //:popup this node to top if need back.
            TTUIPageMgr.Instance.PopNode(this);
        }

        /// <summary>
        /// Async Show UI Logic
        /// </summary>
        public void Show(Action callback)
        {
            ResourcesManager.Instance.StartCoroutine(AsyncShow(callback));
        }

        IEnumerator AsyncShow(Action callback)
        {
            //1:Instance UI
            //FIX:support this is manager multi gameObject,instance by your self.
            if (this.gameObject == null && string.IsNullOrEmpty(uiPath) == false)
            {
                GameObject go = null;
                bool _loading = true;

                delegateAsyncLoadUI(uiPath, (o) =>
                {
                    go = o != null ? GameObject.Instantiate(o) as GameObject : null;
                    AnchorUIGameObject(go);
                    Awake(go);
                    isAsyncUI = true;
                    _loading = false;

                    //:animation active.
                    Active();

                    //:refresh ui component.
                    Refresh();

                    //:popup this node to top if need back.
                    TTUIPageMgr.Instance.PopNode(this);

                    if (callback != null) callback();
                });

                float _t0 = Time.realtimeSinceStartup;
                while (_loading)
                {
                    if (Time.realtimeSinceStartup - _t0 >= 10.0f)
                    {
                        Debug.LogError("[UI] WTF async load your ui prefab timeout!");
                        yield break;
                    }
                    yield return null;
                }
            }
            else
            {
                //:animation active.
                Active();

                //:refresh ui component.
                Refresh();

                //:popup this node to top if need back.
                TTUIPageMgr.Instance.PopNode(this);

                if (callback != null) callback();
            }
        }

        public bool CheckIfNeedBack()
        {
            if (type == UIType.Fixed || type == UIType.PopUp || type == UIType.None)
                return false;
            else if (mode == UIMode.NoNeedBack || mode == UIMode.DoNothing)
                return false;
            return true;
        }

        protected void AnchorUIGameObject(GameObject ui)
        {
            if (TTUIPageMgr.Instance.m_uiRoot == null || ui == null) return;

            this.gameObject = ui;
            this.transform = ui.transform;

            //check if this is ugui or (ngui)?
            Vector3 anchorPos = Vector3.zero;
            Vector2 sizeDel = Vector2.zero;
            Vector3 scale = Vector3.one;
            if (ui.GetComponent<RectTransform>() != null)
            {
                anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
                sizeDel = ui.GetComponent<RectTransform>().sizeDelta;
                scale = ui.GetComponent<RectTransform>().localScale;
            }
            else
            {
                anchorPos = ui.transform.localPosition;
                scale = ui.transform.localScale;
            }

            //Debug.Log("anchorPos:" + anchorPos + "|sizeDel:" + sizeDel);

            if (type == UIType.Fixed)
            {
                ui.transform.SetParent(TTUIPageMgr.Instance.m_uiRoot.fixedRoot);
            }
            else if (type == UIType.Normal)
            {
                ui.transform.SetParent(TTUIPageMgr.Instance.m_uiRoot.normalRoot);
            }
            else if (type == UIType.PopUp)
            {
                ui.transform.SetParent(TTUIPageMgr.Instance.m_uiRoot.popupRoot);
            }


            if (ui.GetComponent<RectTransform>() != null)
            {
                ui.GetComponent<RectTransform>().anchoredPosition = anchorPos;
                ui.GetComponent<RectTransform>().sizeDelta = sizeDel;
                ui.GetComponent<RectTransform>().localScale = scale;
            }
            else
            {
                ui.transform.localPosition = anchorPos;
                ui.transform.localScale = scale;
            }
        }

        public override string ToString()
        {
            return ">Name:" + name + ",ID:" + id + ",Type:" + type.ToString() + ",ShowMode:" + mode.ToString() + ",Collider:" + collider.ToString();
        }

        public bool isActive()
        {
            //fix,if this page is not only one gameObject
            //so,should check isActived too.
            bool ret = gameObject != null && gameObject.activeSelf;
            return ret || isActived;
        }

        #endregion

    }//TTUIPage
}//namespace