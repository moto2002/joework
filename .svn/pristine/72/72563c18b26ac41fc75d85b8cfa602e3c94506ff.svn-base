using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class RecordMgr
{
    private DateTime m_start, m_stop;
    string m_date;
    int m_gameId;
    string m_gameName;
    RecordItem tempRecordItem;

    private static RecordMgr m_inst;
    public static RecordMgr Inst
    {
        get
        {
            if (m_inst == null)
            {
                m_inst = new RecordMgr();
            }
            return m_inst;
        }
    }


    public RecordMgr()
    {
     
        LoadCacheRecord();
    }

    public void LoadCacheRecord()
    {
        string path = GameConst.Record_Cache_Path + GameConst.Record_Name;
        if (File.Exists(path))
        {
            //Debug.Log("exist");
            PlayGameRecordMgr pgrMgr = null;
            try
            {
                pgrMgr = XmlHelper.ParseXml<PlayGameRecordMgr>(path, typeof(PlayGameRecordMgr),true);
            }
            catch (Exception e)
            {
                pgrMgr = null;
            }

            if (pgrMgr != null)
                GlobalData.m_pgrMgr = pgrMgr;
            else
            {
                GlobalData.m_pgrMgr = null;
                Debug.Log("load record.xml failed");
            }
        }
        else
        {
            GlobalData.m_pgrMgr = null;
            //Debug.Log("not exist");
        }
    }



    public void StartRecord(CfgGameList m_cfgGameList)
    {
        tempRecordItem = new  RecordItem();

        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("hh:mm:ss");
        string gameName = m_cfgGameList._name;
        m_start = DateTime.Now;

        m_date = date;
        m_gameId = m_cfgGameList._id;
        m_gameName = gameName;
        tempRecordItem.m_startTime = time;
    }

    public void StopRecord(CfgGameList m_cfgGameList)
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("hh:mm:ss");
        string gameName = m_cfgGameList._name;
        m_stop = DateTime.Now;
        TimeSpan ts = m_stop - m_start;
        string str =GlobalData.GetTimeSpan(ts);

        tempRecordItem.m_stopTime = time;
        tempRecordItem.m_duringTime = str;

        ExcuteRecord();

    }

    private void ExcuteRecord()
    {
        if (GlobalData.m_pgrMgr != null)
        {
            PlayGameRecord pgr =  GlobalData.m_pgrMgr.FindById(m_date);
            if (pgr != null)
            {
                GameItem gameItem = pgr.FindById(m_gameId);
                if (gameItem != null)
                {
                    gameItem.m_record.Add(tempRecordItem);
                    gameItem.m_count = gameItem.m_record.Count;
                }
                else
                {
                    gameItem = new GameItem();
                    gameItem.m_gameId = m_gameId;
                    gameItem.m_gameName = m_gameName;
                    gameItem.m_record = new List<RecordItem>();
                    gameItem.m_record.Add(tempRecordItem);
                    gameItem.m_count = gameItem.m_record.Count;
                    pgr.m_gameItems.Add(gameItem);
                }
            }
            else
            {
                pgr = new PlayGameRecord();
                pgr.m_date = m_date;
                pgr.m_gameItems = new List<GameItem>();
                GameItem gameItem = new GameItem();
                gameItem.m_gameId = m_gameId;
                gameItem.m_gameName = m_gameName;
                gameItem.m_record = new List<RecordItem>();
                gameItem.m_record.Add(tempRecordItem);
                gameItem.m_count = gameItem.m_record.Count;
                pgr.m_gameItems.Add(gameItem);
                GlobalData.m_pgrMgr.m_dataList.Add(pgr);
            }
        }
        else
        {
            GlobalData.m_pgrMgr = new PlayGameRecordMgr();
            GlobalData.m_pgrMgr.m_dataList = new List<PlayGameRecord>();
            PlayGameRecord pgr = new PlayGameRecord();
            pgr.m_date = m_date;
            pgr.m_gameItems = new List<GameItem>();
            GameItem gameItem = new GameItem();
            gameItem.m_gameId = m_gameId;
            gameItem.m_gameName = m_gameName;
            gameItem.m_record = new List<RecordItem>();
            gameItem.m_record.Add(tempRecordItem);
            gameItem.m_count = gameItem.m_record.Count;
            pgr.m_gameItems.Add(gameItem);
            GlobalData.m_pgrMgr.m_dataList.Add(pgr);

        }

        XmlHelper.SaveXml("record.xml", GlobalData.m_pgrMgr, typeof(PlayGameRecordMgr),true);
        Debug.Log("record ok");
    }

    
}
