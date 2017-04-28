using UnityEngine;
using System.Collections;
using Mono.Xml;
using System.Security;

public class TestParseXml : MonoBehaviour 
{
	void Start () 
	{
		SecurityParser sp = new SecurityParser ();

		string s = Resources.Load ("test").ToString ();

		sp.LoadXml (s);

		SecurityElement se = sp.ToXml ();

		foreach (SecurityElement child in se.Children) 
		{
			//比对下是否使自己所需要得节点
			if(child.Tag == "table")
			{
				//获得节点得属性
				string wave = child.Attribute("wave");
				string level = child.Attribute("level");
				string name = child.Attribute("name");
				Debug.Log("wave:" + wave + " level:" + level + " name:" + name);
			}
		}
	}
	
	void Update () {
	
	}
}
