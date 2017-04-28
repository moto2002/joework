using System.Collections.Generic;

public class DictionaryBase<Tkey, Tvalue>
{
    public int Count
    {
        get
        {
            return dic == null ? 0 : dic.Count;
        }
    }

    protected Dictionary<Tkey, Tvalue> dic = new Dictionary<Tkey, Tvalue>();

    private Tvalue GetValue(Tkey key)
    {
        return dic.ContainsKey(key) ?
            dic[key] : default(Tvalue);
    }

    public void Add(Tkey key, Tvalue value)
    {
        if (dic.ContainsKey(key))
        {
            dic[key] = value;
            return;
        }
        dic.Add(key, value);
    }

    public Tvalue this[Tkey key]
    {
        get { return GetValue(key); }
        set { Add(key, value); }
    }

    public virtual bool ContainKey(Tkey key)
    {
        return dic.ContainsKey(key);
    }

    public void Remove(Tkey key)
    {
        if (dic.ContainsKey(key))
        {
            OnRemove(dic[key]);
            dic.Remove(key);
        }
    }

    public void Clear()
    {
        foreach (KeyValuePair<Tkey, Tvalue> item in dic)
        {
            OnRemove(item.Value);
        }
        dic.Clear();
    }

    protected virtual void OnRemove(Tvalue value)
    {

    }

    protected virtual void OnCleared()
    {

    }
}
