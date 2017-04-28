using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

//如果成员变量定义成和Xml字段一样，就不需要再加xmlAttitude了哦

namespace XGame.Config
{
	[System.Serializable]
	public class PersonData
	{
		[XmlElement("idCard")]
		public int _idCard;
		
		[XmlElement("name")]
		public string _name;
		
		[XmlElement("tel")]
		public int _tel;
	}
	
	[XmlRoot("PeopleInfo")]
	public class PersonMgr
	{
		[XmlArray("PeopleNodes"),XmlArrayItem("PeopleNode")]
		public List<PersonData> personDataList;
		
		public PersonData FindById (int id)
		{
			return personDataList.Find (x => x._idCard == id);
		}
	}
}
