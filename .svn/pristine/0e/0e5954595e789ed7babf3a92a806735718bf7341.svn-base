
using UnityEngine;
using System.Collections;
using HuanJueGameData;
using LitJson;

public class ListJsonTest : MonoBehaviour
{
    public TextAsset ta;

    void Start()
    {
        CfgHeroInfo cfginfo = JsonMapper.ToObject<CfgHeroInfo>(ta.text);
        Debug.Log(cfginfo.HeroInfo[0].name +"_"+cfginfo.HeroInfo[0].scale);
    }
}
