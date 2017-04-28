using UnityEngine;
using System.Collections;
using Mono.Xml;
using System.Security;

public class TestParseXml1 : MonoBehaviour {

	void Start () {

		string s  = Resources.Load ("Tank_Fire").ToString();

		load_xml (s);

        this.save_xml();
	}
	
	void Update () {
	
	}

	public void load_xml(byte[] pBuffer)
	{
		string xml = System.Text.Encoding.UTF8.GetString(pBuffer);
		load_xml (xml);
	}

	public void load_xml(string s)
	{
		string xml = s;
		SecurityParser sp = new SecurityParser();
		sp.LoadXml(xml);
		
		SecurityElement se = sp.ToXml();

		foreach (SecurityElement child in se.Children) 
		{
			if (child.Tag == "Node") 
			{
				string m_name = child.Attribute("Class");
				string agentType = child.Attribute("AgentType");
				string versionStr = child.Attribute("Version");
				
				Debug.Log ("m_name:"+m_name + "/" + "agentType:" + agentType + "/" + "version:" + versionStr);
			}
			else
			{
				Debug.Log("fdsdf");
			}
		}

	}

    void save_xml()
    {
        XMLParser.SaveText(Application.streamingAssetsPath + "/" + "testt.xml", "hello,save xml");
    }
}
