using UnityEngine;
using System.Collections;
using System.IO;

public class CfgCenter
{
    private static CfgCenter m_inst;
    public static CfgCenter Inst
    {
         get
        {
            if (m_inst == null)
            {
                m_inst = new CfgCenter();
                m_inst.Init();
            }
            return m_inst;
        }
    }

    public CfgGameListMgr m_cfgGameListMgr;

    string cfg_gameList = "GameList.xml";
    private void Init()
    {
        CopyFileFromStremingAssets2Local();
        string path = GameConst.Cfg_Path + "/" + cfg_gameList;
        m_cfgGameListMgr = XmlHelper.ParseXml<CfgGameListMgr>(path, typeof(CfgGameListMgr));
    }

    private void CopyFileFromStremingAssets2Local()
    {
        string path = GameConst.Cfg_Path + "/" + cfg_gameList;
        string from = Application.streamingAssetsPath + "/Cfg/"+cfg_gameList;

        if (File.Exists(path))
        {
            
        }
        else
        {
            CreateFile(path,"");
        }

        File.Copy(from, path, true);
        Debug.Log("GameList.xml copy ok");
    }
    public  void CreateFile(string path, string info)
    {
       // DeleteFile(path, filename);
        StreamWriter sw;
        FileInfo t = new FileInfo(path );
        if (!t.Exists)
        {
            sw = t.CreateText();
        }
        else
        {
            sw = t.AppendText();
        }
        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }

}
