using Assets.Scripts.Framework;
using ResData;
using System;
using System.Collections.Generic;

public class CTextManager : Singleton<CTextManager>
{
	private Dictionary<int, string> m_textMap;

	public override void Init()
	{
	}

	public void LoadLocalText()
	{
		DatabinTable<ResText, ushort> databinTable = new DatabinTable<ResText, ushort>("Databin/Client/Text/Text.bytes", "wID");
		this.LoadText(databinTable);
		databinTable.Unload();
	}

	public void LoadText(DatabinTable<ResText, ushort> textDataBin)
	{
		if (textDataBin == null)
		{
			return;
		}
		this.m_textMap = new Dictionary<int, string>();
		Dictionary<long, object>.Enumerator enumerator = textDataBin.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<long, object> current = enumerator.get_Current();
			ResText resText = (ResText)current.get_Value();
			this.m_textMap.Add(StringExtension.JavaHashCode(StringHelper.UTF8BytesToString(ref resText.szKey)), StringHelper.UTF8BytesToString(ref resText.szValue));
		}
	}

	public bool IsTextLoaded()
	{
		return this.m_textMap != null;
	}

	public string GetText(string key)
	{
		string text = string.Empty;
		this.m_textMap.TryGetValue(StringExtension.JavaHashCode(key), ref text);
		if (string.IsNullOrEmpty(text))
		{
			text = key;
		}
		return text;
	}

	public string GetText(string key, params string[] args)
	{
		string text = Singleton<CTextManager>.GetInstance().GetText(key);
		if (text == null)
		{
			text = "text with tag [" + key + "] was not found!";
		}
		else
		{
			text = string.Format(text, args);
		}
		return text;
	}
}
