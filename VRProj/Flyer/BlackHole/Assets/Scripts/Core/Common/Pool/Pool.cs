using System.Collections.Generic;

/// <summary>
/// 缓存池 key: 缓存类型， value: 缓存对象
/// </summary>
/// <typeparam name="Tkey">描述对象类型</typeparam>
/// <typeparam name="Tvalue">缓存池 IPoolable </typeparam>
public class Pool<Tkey, Tvalue> where Tvalue : IPoolable
{
    /// <summary>
    /// 正在使用的数量
    /// </summary>
    public int AliveCount { get { return aliveList.Count; } }
    // 正在使用中的列表
    private List<Tvalue> aliveList = new List<Tvalue>();
    // 缓存初始数量，不包含该Key时，初始数量为 1
    private Dictionary<Tkey, int> cacheCountDic = new Dictionary<Tkey, int>();
    // 缓存池
    private Dictionary<Tkey, SinglePool<Tvalue>> pool = new Dictionary<Tkey, SinglePool<Tvalue>>();

    /// <summary>
    /// 设置缓存初始数量
    /// <para>默认初始数量为 1</para>
    /// <para>缓存池初始后设置该值无效</para>
    /// </summary>
    /// <param name="key">缓存类型</param>
    /// <param name="count">数量</param>
    public void SetCacheCount(Tkey key, int count)
    {
        if (cacheCountDic.ContainsKey(key))
            cacheCountDic[key] = count;
        else
            cacheCountDic.Add(key, count);
    }

    /// <summary>
    /// 取出正在使用中的对象
    /// </summary>
    /// <param name="index">序号</param>
    /// <returns></returns>
    public Tvalue GetAlive(int index)
    {
        return index > -1 && index < aliveList.Count ? aliveList[index] : default(Tvalue);
    }

    /// <summary>
    /// 取指定类型缓存池中现有的数量
    /// </summary>
    /// <param name="key">类型</param>
    /// <returns>缓存池中现有的数量</returns>
    public int GetPoolCount(Tkey key)
    {
        return pool.ContainsKey(key) ? pool[key].PoolCount : 0;
    }

    /// <summary>
    /// 缓存对象
    /// </summary>
    /// <param name="key">类型</param>
    /// <returns>缓存对象</returns>
    public Tvalue this[Tkey key]
    {
        get
        {
            if (pool.ContainsKey(key))
            {
                Tvalue t = pool[key].Get();
                t.DestroyedEvent += AliveDestroyed;
                aliveList.Add(t);
                return t;
            }
            return default(Tvalue);
        }
        set
        {
            if (aliveList.Contains(value))
                aliveList.Remove(value);
            if (value == null) return;

            if (pool.ContainsKey(key))
            {
                value.DestroyedEvent -= AliveDestroyed;
                pool[key].Set(value);
            }
            else
            {
                SinglePool<Tvalue> p = new SinglePool<Tvalue>();
                p.SetTempleAndCount(value, cacheCountDic.ContainsKey(key) ? cacheCountDic[key] : 1);
                pool.Add(key, p);
            }
        }
    }

    /// <summary>
    /// 移除一类缓存物体
    /// </summary>
    public void Remove(Tkey key)
    {
        if (pool.ContainsKey(key))
            pool[key].Clear();
        pool.Remove(key);
    }

    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        foreach (KeyValuePair<Tkey, SinglePool<Tvalue>> item in pool)
        {
            item.Value.Clear();
        }
        aliveList.Clear();
        pool.Clear();
        cacheCountDic.Clear();
    }

    private void AliveDestroyed(IPoolable target)
    {
        for (int i = 0; i < aliveList.Count; i++)
        {
            if ((aliveList[i] as IPoolable).Equals(target))
            {
                aliveList[i].DestroyedEvent -= AliveDestroyed;
                aliveList.Remove(aliveList[i]);
                break;
            }
        }
    }
}