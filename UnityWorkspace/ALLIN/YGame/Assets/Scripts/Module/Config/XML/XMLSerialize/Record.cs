using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

//如果成员变量定义成和Xml字段一样，就不需要再加xmlAttitude了哦

[System.Serializable]
public class RecordItem
{
    [XmlAttribute("m_startTime")]
    public string m_startTime;

    [XmlAttribute("m_stopTime")]
    public string m_stopTime;

    [XmlAttribute("m_duringTime")]
    public string m_duringTime;
}

[System.Serializable]
public class GameItem
{
    [XmlAttribute("m_gameId")]
    public int m_gameId;

    [XmlAttribute("m_gameName")]
    public string m_gameName;

    [XmlAttribute("m_count")]
    public int m_count;

    [XmlArray("Records"), XmlArrayItem("Record")]
    public List<RecordItem> m_record;
}


[System.Serializable]
public class PlayGameRecord
{
    [XmlAttribute("m_date")]
    public string m_date;

    [XmlArray("GameItems"), XmlArrayItem("GameItem")]
    public List<GameItem> m_gameItems;

    public GameItem FindById(int id)
    {
        return m_gameItems.Find(x => x.m_gameId == id);
    }
}



[XmlRoot("Root")]
public class PlayGameRecordMgr
{
    [XmlArray("List"), XmlArrayItem("GameList")]
    public List<PlayGameRecord> m_dataList;

    public PlayGameRecord FindById(string id)
    {
        return m_dataList.Find(x => x.m_date == id);
    }
}