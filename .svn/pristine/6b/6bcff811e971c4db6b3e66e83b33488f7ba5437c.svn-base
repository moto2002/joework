using System;
using UnityEngine;

public class TestXmlSerialize : MonoBehaviour
{
    void Start()
    {
        //write

        /*
        GameItem item = new GameItem();
        item.m_gameId = 1;
        item.m_gameName = "游戏a";
        item.m_record = new System.Collections.Generic.List<RecordItem>();
        RecordItem ri1 = new RecordItem();
        ri1.m_startTime = "08:00";
        ri1.m_stopTime = "08:45";
        ri1.m_duringTime = "45min";
        RecordItem ri2 = new RecordItem();
        ri2.m_startTime = "09:00";
        ri2.m_stopTime = "09:45";
        ri2.m_duringTime = "45min";
        item.m_record.Add(ri1);
        item.m_record.Add(ri2);
        item.m_count = item.m_record.Count;

        PlayGameRecord pgr = new PlayGameRecord();
        pgr.m_date = "2016-09-22";
        pgr.m_gameItems = new System.Collections.Generic.List<GameItem>();
        pgr.m_gameItems.Add(item);

        PlayGameRecordMgr pgrMgr = new PlayGameRecordMgr();
        pgrMgr.m_dataList = new System.Collections.Generic.List<PlayGameRecord>();
        pgrMgr.m_dataList.Add(pgr);

        XmlHelper.SaveXml("record.xml", pgrMgr, typeof(PlayGameRecordMgr));
        Debug.Log("ok");
        */


        //read
        string path = Application.dataPath + "/Resources/Cfg/record.xml";
        PlayGameRecordMgr pgrMgr = XmlHelper.ParseXml<PlayGameRecordMgr>(path, typeof(PlayGameRecordMgr));

        string date = DateTime.Now.ToString("yyyy-MM-dd");
        PlayGameRecord recordItem = pgrMgr.FindById("2016-09-22");
        Debug.Log("count:" + recordItem.m_gameItems.Count);
        GameItem item = recordItem.FindById(1);//假设游戏id =1 

        Debug.Log(item.m_gameName + "_" + item.m_count);




    }

    void Update()
    {

    }
}
