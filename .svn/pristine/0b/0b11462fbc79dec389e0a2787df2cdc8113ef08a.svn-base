using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
public class CfgPlayTime
{

    [XmlAttribute("id")]
    public int _id;

    [XmlAttribute("m_date")]
    public string m_date;

    [XmlAttribute("m_startTime")]
    public string m_startTime;

    [XmlAttribute("m_stopTime")]
    public string m_stopTime;

    [XmlAttribute("m_duringTime")]
    public string m_duringTime;

    [XmlAttribute("m_cost")]
    public string m_cost;

}

[XmlRoot("Root")]
public class CfgPlayTimeMgr
{
    [XmlArray("Times"), XmlArrayItem("Time")]
    public List<CfgPlayTime> m_dataList;

    public CfgPlayTime FindById(int id)
    {
        return m_dataList.Find(x => x._id == id);
    }

    public List<CfgPlayTime> FindByDate(string date)
    {
        return m_dataList.FindAll(x => x.m_date == date);
    }
}