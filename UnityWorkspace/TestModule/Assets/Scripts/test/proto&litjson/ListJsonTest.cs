
using UnityEngine;
using System.Collections;
using HuanJueGameData;
using LitJson;

public class ListJsonTest : MonoBehaviour
{
    public TextAsset ta;

    void Start()
    {
        CfgPlayerInfo cfginfo = JsonMapper.ToObject<CfgPlayerInfo>(ta.text);
        Debug.Log(cfginfo.PlayerInfo[0].name);
    }
}
