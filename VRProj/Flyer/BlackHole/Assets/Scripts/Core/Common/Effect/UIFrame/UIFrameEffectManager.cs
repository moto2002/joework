using System;
using UnityEngine;
using System.Collections.Generic;
using Core.Extension.UIFrameEffect;

public class UIFrameEffectManager
{
    #region vars

    private Transform parent;
    private Counter counter;
    private string moduleName;

    /// <summary>
    /// 正在播放中的特效
    /// </summary>
    private List<IUIFrameEffect> showingList = new List<IUIFrameEffect>();

    /// <summary>
    /// 特效 ID 对应 特效实例列表
    /// </summary>
    private Dictionary<string, Queue<IUIFrameEffect>> effectDic = new Dictionary<string, Queue<IUIFrameEffect>>();

    /// <summary>
    /// 特效 ID 对应 图列表（实例特效时从这里取图集）
    /// </summary>
    private Dictionary<string, List<Sprite>> spriteDic = new Dictionary<string, List<Sprite>>();

    //private Dictionary<string, FrameEffectConfig> tableDataDic = new Dictionary<string, FrameEffectConfig>();

    //private TableCenter tableCenter;
    //private TableCenter TableCenter
    //{
    //    get
    //    {
    //        if (tableCenter == null)
    //        {
    //            tableCenter = TableCenter.Get(moduleName);
    //        }
    //        return tableCenter;
    //    }
    //}

    /// <summary>
    /// 特效配置信息
    /// </summary>
    //private Dictionary<string, FrameEffectConfig> TableDataDic
    //{
    //    get
    //    {
    //        if (tableDataDic == null || tableDataDic.Count < 1)
    //        {
    //            tableDataDic = TableCenter.GetTable<FrameEffectConfig>();
    //        }
    //        return tableDataDic;
    //    }
    //}

    #endregion

    private UIFrameEffectManager(Transform parent, string moduleName)
    {
        this.parent = parent;
        this.moduleName = moduleName;
        counter = new Counter();
    }



    /// <summary>
    /// 播放帧特效
    /// </summary>
    /// <param name="effectID">特效 ID</param>
    /// <param name="parent">特效所在的父物体</param>
    public IUIFrameEffect Play(string effectID, Transform parent, Vector3 offset = default(Vector3))
    {
        IUIFrameEffect effect = GetFrameEffect(effectID);
        if (effect == null)
        {
            Debug.LogError("Frame Effect \"" + effectID + "\" is not Exist!");
            return null;
        }
        counter.Add(effect);
        counter.Destroy += (arg) => { DestroyEffect(arg); };
        effect.SetDestroyEvent((arg) =>
        {
            if (showingList.Contains(arg))
                showingList.Remove(arg);
        });

        effect.SetEndEvent((arg) =>
        {
            arg.gameObject.transform.SetParent(this.parent.transform);
            if (showingList.Contains(arg))
                showingList.Remove(arg);
            if (effectDic.ContainsKey(arg.ToString()))
            {
                effectDic[arg.ToString()].Enqueue(arg);
            }
        });
        effect.gameObject.transform.SetParent(parent);
        effect.gameObject.transform.localEulerAngles = Vector3.zero;
        effect.gameObject.transform.localScale = Vector3.one;
        effect.gameObject.GetComponent<RectTransform>().anchoredPosition3D = offset;
        (effect as UIFrameEffect).Play();
        showingList.Add(effect);
        return effect;
    }

    /// <summary>
    /// 播放帧特效
    /// </summary>
    /// <param name="arg">帧特效参数</param>
    public IUIFrameEffect Play(UIFrameEffectArgs arg, Vector3 offset = new Vector3())
    {
        IUIFrameEffect effect = Play(arg.EffectID, arg.Parent, offset);
        if (effect == null) return null;
        (effect as UIFrameEffect).IndexEvent = arg.BackDic;
        return effect;
    }

    public void RemoveFrameEffect(string effectID)
    {
        if (!string.IsNullOrEmpty(effectID))
        {
            DestroyEffect(effectID);
        }
    }

    /// <summary>
    /// 清除所有帧特效数据
    /// <para>所有 GameObject 及 图集 列表</para>
    /// </summary>
    public void Clear()
    {
        //foreach (KeyValuePair<string, FrameEffectConfig> item in TableDataDic)
        //{
        //    DestroyEffect(item.Key);
        //}
    }

    #region Assist methodes

    //public void Init(string gameName, int count)
    //{
    //    foreach (KeyValuePair<string, FrameEffectConfig> item in TableDataDic)
    //    {
    //        if (item.Value.ModuleName.Equals(gameName))
    //        {
    //            if (!effectDic.ContainsKey(item.Value.Id))
    //            {
    //                effectDic.Add(item.Value.Id, new Queue<IFrameEffect>());
    //            }
    //            if (effectDic[item.Value.Id].Count < 1)
    //            {
    //                string name = gameName + "_" + item.Value.EffectName;
    //                for (int i = 0; i < count; i++)
    //                {
    //                    GameObject o = new GameObject(name + "_" + i);
    //                    FrameEffect fe = o.AddComponent<FrameEffect>();
    //                    fe.Setup(item.Value.Id, GetSpriteList(item.Value.Id));
    //                    effectDic[item.Value.Id].Enqueue(fe);
    //                    if (this.parent == null)
    //                        this.parent = new GameObject("FrameEffects");
    //                    o.transform.SetParent(this.parent.transform);
    //                }
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 根据 ID 获取对应 FrameEffect, 如果队列中没有特效则初始化 5 个
    /// <para>返回为空时，输出 Error 日志</para>
    /// </summary>
    private IUIFrameEffect GetFrameEffect(string effectID)
    {
        if (parent == null)
        {
            Debug.LogError("FrameEffectManager was destroyed !\n");
            return null;
        }
        //if (!TableDataDic.ContainsKey(effectID))
        //{
        //    Debug.LogError("FrameEffect which ID is " + effectID + " does not configged in Table !\n");
        //    return null;
        //}
        if (!effectDic.ContainsKey(effectID))
            effectDic.Add(effectID, new Queue<IUIFrameEffect>());
        if (effectDic[effectID].Count < 1)
        {
            //GameObject o = new GameObject(TableDataDic[effectID].ModuleName + "_" + TableDataDic[effectID].EffectName);
            //FrameEffect fe = o.AddComponent<FrameEffect>();
            //fe.Setup(effectID, GetSpriteList(effectID));
            //o.transform.SetParent(this.parent);
            //return fe as IFrameEffect;
        }
        return effectDic[effectID].Dequeue();
    }

    /// <summary>
    /// 根据 ID 获取对应的图集列表, 返回为空时，输出 Error 日志
    /// </summary>
    private List<Sprite> GetSpriteList(string effectID)
    {
        if (!spriteDic.ContainsKey(effectID))
        {
            //if (!TableDataDic.ContainsKey(effectID))
            //{
            //    return null;
            //}
            //else
            //{
            //    FrameEffectConfig effectConfig = TableDataDic[effectID];
            //    List<Sprite> spriteList = new List<Sprite>();
            //    AssetHelper assetHelper = AssetManager.Instance.GetHelper(effectConfig.ModuleName);
            //    if (effectConfig.Count > 0)
            //        for (int i = 0; i < effectConfig.Count; i++)
            //        {
            //            spriteList.Add(assetHelper.LoadFrame(effectConfig.Path, effectConfig.EffectName + "_" + i.ToString()));
            //        }
            //    spriteDic.Add(effectID, spriteList);
            //}
        }
        return spriteDic[effectID];
    }

    /// <summary>
    /// 清除帧特效数据
    /// </summary>
    private void DestroyEffect(string effectID)
    {
        if (spriteDic.ContainsKey(effectID))// 清除图集
        {
            spriteDic[effectID].Clear();
            spriteDic.Remove(effectID);
        }

        if (effectDic.ContainsKey(effectID))// 清除库存
        {
            int count = effectDic[effectID].Count;
            for (int i = 0; i < count; i++)
            {
                UnityEngine.Object.DestroyImmediate(effectDic[effectID].Dequeue().gameObject);
            }
            effectDic[effectID].Clear();
            effectDic.Remove(effectID);
        }
    }

    #endregion

    private class Counter
    {
        public event Action<string> Destroy;

        private List<FEItem> itemList = new List<FEItem>();

        public void Add(IUIFrameEffect effect)
        {
            FEItem fei = GetItem(effect.ToString());
            if (fei == null)
                fei = new FEItem(effect.ToString());
            if (!itemList.Contains(fei))
                itemList.Add(fei);
            fei.Add();
            fei.DestroyName += (arg) =>
            {
                if (Destroy != null)
                    Destroy(arg.ID);
                itemList.Remove(arg);
            };
            effect.SetDestroyEvent(SubItem);
            effect.SetEndEvent(SubItem);
        }

        private void SubItem(IUIFrameEffect arg)
        {
            FEItem fei2 = GetItem(arg.ToString());
            if (fei2 != null)
                fei2.Sub();
        }

        private FEItem GetItem(string id)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].ID.Equals(id))
                {
                    return itemList[i];
                }
            }
            return null;
        }

        private class FEItem
        {
            private int deathTime = 600;

            public string ID { get; private set; }

            public int Count { get; private set; }

            public TimerManager.ITimer Timer { get; private set; }

            public event Action<FEItem> DestroyName;

            public FEItem(string id)
            {
                ID = id;
                Count = 0;
            }

            public void Add()
            {
                Count++;
                if (Timer != null)
                {
                    //PublicWorld.TimerManager.RemoveTimer(Timer);
                    Timer = null;
                }
            }

            public void Sub()
            {
                Count--;
                if (Count == 0)
                {
                    //Timer = PublicWorld.TimerManager.AddTimer(deathTime, (arg) =>
                    //{
                    //    if (DestroyName != null)
                    //        DestroyName(this);
                    //});
                }
            }

            public override string ToString() { return ID; }
        }
    }
}

public interface IUIFrameEffect
{
    /// <summary>
    /// 特效ID
    /// </summary>
    string ID { get; }

    /// <summary>
    /// 当前播放序列号
    /// </summary>
    int CurIndex { get; }

    /// <summary>
    /// 特效 GameObject
    /// </summary>
    GameObject gameObject { get; }

    /// <summary>
    /// 返回指定序号图
    /// </summary>
    /// <param name="index">序号</param>
    Sprite this[int index] { get; }

    /// <summary>
    /// 停止播放
    /// </summary>
    void Stop();

    /// <summary>
    /// 设置起始播放帧
    /// </summary>
    /// <param name="index">起始帧序号</param>
    IUIFrameEffect SetIndex(int index);

    /// <summary>
    /// 播放速度
    /// </summary>
    /// <param name="speed">默认每秒12帧： 1/12 ~= 0.083f</param>
    IUIFrameEffect SetSpeed(float speed = 0.083f);

    /// <summary>
    /// 设置是否循环播放
    /// </summary>
    /// <param name="loop">默认只播放一次</param>
    IUIFrameEffect SetLoop(bool loop = true);

    /// <summary>
    /// 设置特效结束回调事件
    /// </summary>
    /// <param name="back">特效结束回调事件</param>
    IUIFrameEffect SetEndEvent(Action<IUIFrameEffect> back);

    /// <summary>
    /// 设置特效某帧播放回调事件
    /// </summary>
    /// <param name="index">帧序号</param>
    /// <param name="back">回调事件</param>
    IUIFrameEffect SetIndexEvent(int index, Action<IUIFrameEffect> back);

    /// <summary>
    /// 设置特效销毁回调事件
    /// </summary>
    /// <param name="back">销毁回调事件</param>
    IUIFrameEffect SetDestroyEvent(Action<IUIFrameEffect> back);

    /// <summary>
    /// 设置特效旋转速度
    /// </summary>
    /// <param name="rotateSpeed"></param>
    /// <returns></returns>
    IUIFrameEffect SetRotateSpeed(float rotateSpeed);

    /// <summary>
    /// 设置特效的旋转值
    /// </summary>
    /// <param name="rotate"></param>
    /// <returns></returns>
    IUIFrameEffect SetRotate(Vector3 rotate);
}