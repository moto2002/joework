using Mogo.RPC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

public class LuaTable : IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
    private Dictionary<string, object> m_dic = new Dictionary<string, object>();
    private Dictionary<string, bool> m_keyIsStringDic = new Dictionary<string, bool>();

    public void Add(int key, object value)
    {
        this.Add(key.ToString(), false, value);
    }

    public void Add(float key, object value)
    {
        this.Add(Convert.ToInt32(key).ToString(), false, value);
    }

    public void Add(string key, object value)
    {
        this.Add(key, true, value);
    }

    public void Add(string key, bool isString, object value)
    {
        this.m_dic.Add(key, value);
        this.m_keyIsStringDic.Add(key, isString);
    }

    public void Clear()
    {
        this.m_dic.Clear();
    }

    public bool ContainsKey(string key)
    {
        return this.m_dic.ContainsKey(key);
    }

    public bool ContainsValue(object value)
    {
        return this.m_dic.ContainsValue(value);
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return this.m_dic.GetEnumerator();
    }

    public LuaTable GetLuaTable(string key)
    {
        if (this.IsLuaTable(key))
        {
            return (this.m_dic[key] as LuaTable);
        }
        return null;
    }

    public bool IsKeyString(string key)
    {
        return this.m_keyIsStringDic.GetValueOrDefault<string, bool>(key, true);
    }

    public bool IsLuaTable(string key)
    {
        if (this.ContainsKey(key))
        {
            object obj2 = this.m_dic[key];
            if (obj2.GetType() == typeof(LuaTable))
            {
                return true;
            }
        }
        return false;
    }

    public bool Remove(string key)
    {
        return this.m_dic.Remove(key);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.m_dic.GetEnumerator();
    }

    public override string ToString()
    {
        return Utils.PackLuaTable(this);
    }

    public bool TryGetLuaTable(string key, out LuaTable value)
    {
        if (this.IsLuaTable(key))
        {
            value = this.m_dic[key] as LuaTable;
            return true;
        }
        value = null;
        return false;
    }

    public bool TryGetValue(string key, out object value)
    {
        return this.m_dic.TryGetValue(key, out value);
    }

    public int Count
    {
        get
        {
            return this.m_dic.Count;
        }
    }

    public object this[string key]
    {
        get
        {
            return this.m_dic[key];
        }
        set
        {
            this.m_dic[key] = value;
        }
    }

    public Dictionary<string, object>.KeyCollection Keys
    {
        get
        {
            return this.m_dic.Keys;
        }
    }

    public Dictionary<string, object>.ValueCollection Values
    {
        get
        {
            return this.m_dic.Values;
        }
    }
}

