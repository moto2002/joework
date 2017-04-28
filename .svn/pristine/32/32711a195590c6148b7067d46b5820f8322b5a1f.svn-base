using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

//如果成员变量定义成和Xml字段一样，就不需要再加xmlAttitude了哦


[System.Serializable]
public class CfgGameList
{
    [XmlAttribute("id")]
    public int _id;

    [XmlAttribute("name")]
    public string _name;

    [XmlAttribute("icon")]
    public string _icon;

    [XmlAttribute("type")]
    public string _type;

    [XmlAttribute("desc")]
    public string _desc;

    [XmlAttribute("relativePath")]
    public string _relativePath;

    [XmlAttribute("screenShot")]
    public string _screenShot;
}

[XmlRoot("Root")]
public class CfgGameListMgr
{
    [XmlArray("List"), XmlArrayItem("GameList")]
    public List<CfgGameList> m_dataList;

    public CfgGameList FindById(int id)
    {
        return m_dataList.Find(x => x._id == id);
    }
}
