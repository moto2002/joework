using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace XGame.Utils
{
	public class XmlHelper : MonoBehaviour 
	{
		public static string dataPath = Application.streamingAssetsPath;
		
		static string UTF8ByteArrayToString(byte[] characters)   
		{        
			UTF8Encoding encoding = new UTF8Encoding();   
			string constructedString = encoding.GetString(characters);   
			return (constructedString);   
		}   
		
		static byte[] StringToUTF8ByteArray(string pXmlString)   
		{   
			UTF8Encoding encoding = new UTF8Encoding();   
			byte[] byteArray = encoding.GetBytes(pXmlString);   
			return byteArray;   
		}  
		
		static string SerializeObject(object pObject,System.Type type)   
		{  
			string XmlizedString = null;   
			MemoryStream memoryStream = new MemoryStream();   
			XmlSerializer xs = new XmlSerializer(type);   
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);   
			xs.Serialize(xmlTextWriter, pObject);   
			memoryStream = (MemoryStream)xmlTextWriter.BaseStream;   
			XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());   
			return XmlizedString;   
		}  
		
		
		static object DeserializeObject(string pXmlizedString,System.Type type)   
		{   
			XmlSerializer xs = new XmlSerializer(type);   
			MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));   
			return xs.Deserialize(memoryStream);   
		}   
		
		public static T ParseXml<T>(string filePath,System.Type type)
		{
			object instance = null;
			FileInfo t = new FileInfo (dataPath + "/" + filePath);
			if(t.Exists)   
			{   
				StreamReader r = t.OpenText();   
				string _info = r.ReadToEnd();   
				r.Close();   
				string _data=_info;   
				if(_data.ToString() != "")   
				{   
					instance = DeserializeObject(_data,type);                
				}   
			}
			return (T)instance;
		}
		
		public static void SaveXml(string filePath,object obj,System.Type type)
		{
			string _data = SerializeObject(obj,type);   
			StreamWriter writer;   
			FileInfo t = new FileInfo (Application.dataPath + "/" + filePath);
			if(t.Exists)   
			{   
				t.Delete();    
			}      
			writer = t.CreateText();   
			writer.Write(_data);   
			writer.Close();  
		}
	}

}

