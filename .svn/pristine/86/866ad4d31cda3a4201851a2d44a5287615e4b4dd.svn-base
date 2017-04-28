using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
public class ResultData
{
    public CfgPlayTime cfgPlayTime;
    public TimeSpan ts;
}
public class PlayTimeMgr
{
    private static PlayTimeMgr m_inst;
    private CfgPlayTime tempCfgPlayTime;
    private DateTime m_start, m_stop;

    public static PlayTimeMgr Inst
    {
        get
        {
            if (m_inst == null)
            {
                m_inst = new PlayTimeMgr();

            }
            return m_inst;
        }
    }

    public PlayTimeMgr()
    {
        LoadCacheRecord();
    }

    private void LoadCacheRecord()
    {
        string path = GameConst.Record_Cache_Path + GameConst.Play_Time;
        if (File.Exists(path))
        {
            //Debug.Log("exist");
            CfgPlayTimeMgr ptMgr = null;
            try
            {
                ptMgr = XmlHelper.ParseXml<CfgPlayTimeMgr>(path, typeof(CfgPlayTimeMgr));
            }
            catch (Exception e)
            {
                ptMgr = null;
            }

            if (ptMgr != null)
                GlobalData.m_ptMgr = ptMgr;
            else
            {
                GlobalData.m_ptMgr = null;
                Debug.Log("load playtime.xml failed");
            }
        }
        else
        {
            GlobalData.m_ptMgr = null;
            //Debug.Log("not exist");
        }
    }


    public void StartRecord()
    {
        tempCfgPlayTime  = new CfgPlayTime();
        tempCfgPlayTime._id = 0;
        tempCfgPlayTime.m_startTime = DateTime.Now.ToString("hh:mm:ss");
        m_start = DateTime.Now;
    }


    public ResultData StopRecord()
    {
        if (GlobalData.m_isFirstEntered)
        {
            return null;
        }

        tempCfgPlayTime.m_stopTime = DateTime.Now.ToString("hh:mm:ss");
        m_stop = DateTime.Now;
        TimeSpan ts = m_stop - m_start;
        string str = GlobalData.GetTimeSpan(ts);
        tempCfgPlayTime.m_duringTime = str;

        ResultData rd = new ResultData();
        rd.cfgPlayTime = tempCfgPlayTime;
        rd.ts = ts;

        return rd;

       // ExcuteRecord();
    }

    public void ExcuteRecord()
    {
        if (GlobalData.m_ptMgr != null)
        {
            int count = GlobalData.m_ptMgr.m_dataList.Count;
            int index = count + 1;
            tempCfgPlayTime._id = index;
            tempCfgPlayTime.m_cost = GlobalData.m_cost;
            tempCfgPlayTime.m_date = DateTime.Now.ToString("yyyy-MM-dd");
            GlobalData.m_ptMgr.m_dataList.Add(tempCfgPlayTime);
        }
        else
        {
            GlobalData.m_ptMgr = new CfgPlayTimeMgr();
            GlobalData.m_ptMgr.m_dataList = new List<CfgPlayTime>();
            tempCfgPlayTime._id = 1;
            tempCfgPlayTime.m_cost = GlobalData.m_cost;
            tempCfgPlayTime.m_date = DateTime.Now.ToString("yyyy-MM-dd");
            GlobalData.m_ptMgr.m_dataList.Add(tempCfgPlayTime);
        }

        XmlHelper.SaveXml("playtime.xml", GlobalData.m_ptMgr, typeof(CfgPlayTimeMgr));
        Debug.Log("playtime.xml ok");
    }
}
