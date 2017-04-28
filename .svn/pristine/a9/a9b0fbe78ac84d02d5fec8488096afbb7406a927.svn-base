using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 可序列化的key,value容器
/// </summary>
[Serializable]
public class Map<TKey, TValue> : ScriptableObject where TKey: IEquatable<TKey>{

	public TValue this[TKey index]{
		get { 
			if(ContainsKey(index)){
				return GetMap(index);
			}
			return default (TValue);
		}
		set { Add(index,value); }
	}

	[SerializeField]
	private List<TKey> m_keys = new List<TKey>();
	public List<TKey> Keys
	{
		get{return m_keys;}
		set{m_keys=value;}
	}

	[SerializeField]
	private List<TValue> m_values = new List<TValue>();
	public List<TValue> Values
	{
		get{return m_values;}
		set{m_values=value;}
	}

	public void Add(TKey key, TValue data)
	{
		if(!ContainsKey(key))
		{
			m_keys.Add(key);
			m_values.Add(data);
		}
		else
			SetMap(key, data);
	}

	public void Remove(TKey key)
	{        
		m_values.Remove(GetMap(key));
		m_keys.Remove(key);        
	}

	public bool ContainsKey(TKey key)
	{
		int len = m_keys.Count;
		for(int i =0; i<len;i++)
		{
			if(m_keys[i].Equals(key))
				return true;
		}
		return false;
	}

	public bool ContainsValue(TValue data)
	{
		int len = m_values.Count;
		for(int i =0; i<len;i++)
		{
			if(m_values[i].Equals(data))
				return true;
		}
		return false;
	}

	public void Clear()
	{
		if(m_keys.Count>0)
			m_keys.Clear();
		if(m_values.Count>0)
			m_values.Clear();
	}    

	private void SetMap(TKey key, TValue data)
	{
		int keyIndex = 0;
		int count = m_keys.Count;
		for(int i =0; i<count;i++)
		{
			if(m_keys[i].Equals(key))
				keyIndex = i;
		}
		m_values[keyIndex]=data;
	}

	private TKey GetKey(TKey key)
	{
		int count = m_keys.Count;
		for(int i =0; i<count;i++)
		{
			if(m_keys[i].Equals(key))
				return m_keys[i];
		}
		return default(TKey);

	}        

	private TValue GetMap(TKey key)
	{
		int keyIndex = 0;
		int count = m_keys.Count;
		for(int i =0; i<count;i++)
		{
			if(m_keys[i].Equals(key))
				keyIndex = i;
		}

		return m_values[keyIndex];

	}    

	/// <summary>
	/// 转换成Dictionary
	/// </summary>
	/// <returns>The to dictionary.</returns>
	public Dictionary<TKey, TValue> ConvertToDictionary()
	{
		Dictionary<TKey,TValue> dictionaryData = new Dictionary<TKey, TValue>();
		try{
			for(int i =0; i<m_keys.Count;i++)
			{
				dictionaryData.Add(m_keys[i], m_values[i]);
			}
		}
		catch(Exception)
		{
			Debug.LogError("KeysList.Count is not equal to ValuesList.Count. It shouldn't happen!");
		}

		return dictionaryData;
	}

	/// <summary>
	/// Dictionary转Map
	/// </summary>
	/// <returns>The to map.</returns>
	/// <param name="dic">Dic.</param>
	public static Map<TKey,TValue> ConvertToMap( Dictionary<TKey,TValue> dic){
		Map<TKey,TValue> map = new Map<TKey, TValue>();
		foreach(TKey key in dic.Keys){
			map[key] = dic[key];
		}
		return map;
	}

}