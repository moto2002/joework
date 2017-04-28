using System;
using UnityEngine;

public class PoolItem : IPoolable
{
    /// <summary>
    /// GameObject
    /// </summary>
    public GameObject GameObject { get; private set; }
    /// <summary>
    /// 是否已经被销毁
    /// </summary>
    public bool Destroyed { get; private set; }
    /// <summary>
    /// 被销毁事件
    /// </summary>
    public event Action<IPoolable> DestroyedEvent;

    /// <summary>
    /// 构造函数
    /// </summary>
    public PoolItem(GameObject body)
    {
        Destroyed = false;
        GameObject = body;
    }

    /// <summary>
    /// 复制一个新的实例对象
    /// </summary>
    public virtual IPoolable Copy()
    {
        GameObject o = UnityEngine.Object.Instantiate(GameObject);
        o.name = GameObject.name;
        o.transform.SetParent(GameObject.transform.parent);
        o.transform.localPosition = GameObject.transform.localPosition;
        o.transform.localScale = GameObject.transform.localScale;
        if (o.GetComponent<RectTransform>())
            o.GetComponent<RectTransform>().anchoredPosition3D = GameObject.GetComponent<RectTransform>().anchoredPosition3D;
        if (o.GetComponent<ParticleSystem>())
            o.GetComponent<ParticleSystem>().Stop();
        return new PoolItem(o);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void Dispose()
    {
        if (!Destroyed)
        {
            Destroyed = true;
            if (DestroyedEvent != null) DestroyedEvent(this);
            UnityEngine.Object.DestroyImmediate(GameObject);
        }
    }
}
