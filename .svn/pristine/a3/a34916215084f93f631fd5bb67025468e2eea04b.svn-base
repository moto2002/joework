using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 缓存池
/// <para>调用 SetTempleAndCount 方法完成初始化</para>
/// <para>若模板对象的GameObject为空或被销毁，则不能产生新的实例</para>
/// </summary>
/// <typeparam name="Tvalue">IPoolable 类型</typeparam>
public class SinglePool<Tvalue> where Tvalue : IPoolable
{
    // 是否已经初始化
    private bool inited = false;
    // 模板
    private Tvalue temple;
    // 正在使用的列表
    private List<Tvalue> aliveList = new List<Tvalue>();
    // 缓存池
    private Queue<Tvalue> pool = new Queue<Tvalue>();
    /// <summary>
    /// 正在使用的数量
    /// </summary>
    public int AliveCount { get { return aliveList.Count; } }
    /// <summary>
    /// 缓存池中对象的数量
    /// </summary>
    public int PoolCount { get { return pool.Count; } }

    /// <summary>
    /// 设置模板及初始数量
    /// </summary>
    /// <param name="target">模板</param>
    /// <param name="cacheCount">缓存数量</param>
    public void SetTempleAndCount(Tvalue target, int cacheCount = 1)
    {
        if (inited || target == null || target.GameObject == null) return;
        inited = true;
        temple = target;
        temple.DestroyedEvent += Remove;
        if (cacheCount > 0)
        {
            for (int i = 0; i < cacheCount; i++)
            {
                Tvalue t = (Tvalue)target.Copy();
                t.DestroyedEvent += Remove;
                pool.Enqueue(t);
            }
        }
    }

    /// <summary>
    /// 取正在使用的对象
    /// </summary>
    /// <param name="index">序号</param>
    /// <returns>正在使用的对象</returns>
    public Tvalue GetAlive(int index)
    {
        return index > -1 && aliveList.Count > 0 && index < aliveList.Count ?
               aliveList[index] : default(Tvalue);
    }

    /// <summary>
    /// 从缓存池中取出或用模板复制出一个对象
    /// </summary>
    public Tvalue Get()
    {
        if (!inited) return default(Tvalue);
        Tvalue back = pool.Count > 0 ? pool.Dequeue() :
                      temple != null && temple.GameObject != null ? (Tvalue)temple.Copy() : default(Tvalue);
        if (pool.Count < 1 && temple != null && temple.GameObject != null)
            back.DestroyedEvent += Remove;
        return back;
    }

    /// <summary>
    /// 将对象还回缓存池
    /// </summary>
    public void Set(Tvalue target)
    {
        if (!inited) return;
        if (aliveList.Contains(target)) aliveList.Remove(target);
        if (target != null && target.GameObject != null && !pool.Contains(target))
        {
            target.GameObject.transform.SetParent(temple.GameObject.transform.parent);
            target.GameObject.transform.localPosition = Vector3.zero;
            if (target.GameObject.GetComponent<RectTransform>())
                target.GameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            pool.Enqueue(target);
        }
    }

    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < aliveList.Count; i++)
        {
            aliveList[i].DestroyedEvent -= Remove;
            aliveList[i].Dispose();
        }
        aliveList.Clear();
        int pc = pool.Count;
        for (int i = 0; i < pc; i++)
        {
            Tvalue t = pool.Dequeue();
            t.DestroyedEvent -= Remove;
            t.Dispose();
        }
        pool.Clear();
        if (temple.GameObject != null)
        {
            temple.DestroyedEvent -= Remove;
            temple.Dispose();
        }
        inited = false;
    }

    /// <summary>
    /// 在对象销毁前从缓存池中移除
    /// </summary>
    /// <param name="target">将要销毁的对象</param>
    private void Remove(IPoolable target)
    {
        if (!inited || target == null || target.GameObject == null) return;
        if (temple != null && temple.GameObject != null && temple.GameObject.Equals(target.GameObject))
        {
            temple.DestroyedEvent -= Remove;
            temple = default(Tvalue);
            return;
        }
        bool removed = false;
        List<Tvalue> nt = new List<Tvalue>();
        int pc = pool.Count;
        for (int i = 0; i < pc; i++)
        {
            Tvalue t = pool.Dequeue();
            if (t.GameObject.Equals(target.GameObject))
            {
                removed = true;
                target.DestroyedEvent -= Remove;
            }
            else
                nt.Add(t);
        }
        for (int i = 0; i < nt.Count; i++)
        {
            pool.Enqueue(nt[i]);
        }
        if (removed) return;

        int alc = aliveList.Count;
        for (int i = 0; i < alc; i++)
        {
            if (aliveList[i].GameObject.Equals(target.GameObject))
            {
                aliveList.Remove(aliveList[i]);
                target.DestroyedEvent -= Remove;
                break;
            }
        }
    }
}

/// <summary>
/// 缓存对象接口
/// <para>注： DestroyedEvent 必须在物体被销毁前被触发，即：OnDestroy()</para>
/// </summary>
public interface IPoolable : IDisposable
{
    /// <summary>
    /// 是否已经被销毁
    /// </summary>
    bool Destroyed { get; }

    /// <summary>
    /// GameObject
    /// </summary>
    GameObject GameObject { get; }

    /// <summary>
    /// 被销毁事件
    /// </summary>
    event Action<IPoolable> DestroyedEvent;

    /// <summary>
    /// 复制
    /// </summary>
    /// <returns>IPoolable 接口对象</returns>
    IPoolable Copy();
}