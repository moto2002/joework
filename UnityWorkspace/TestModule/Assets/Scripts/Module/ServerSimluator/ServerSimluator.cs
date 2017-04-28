using UnityEngine;
using System.Collections;
using ShiHuanJue.Event;
using System.Collections.Generic;

public class ServerSimluator
{
    private static ServerSimluator m_instance;
    public static ServerSimluator Instance
    {
        get {
            if (m_instance == null)
                m_instance = new ServerSimluator();
            return m_instance;
        }
    }

    public void LoginResult(string userName,string pwd)
    {
        if (userName == "joe" && pwd == "123")
            EventDispatcher.TriggerEvent<bool>(EventsConst.LoginScene.LoginResponse, true);
        else
            EventDispatcher.TriggerEvent<bool>(EventsConst.LoginScene.LoginResponse, false);
    }

    public void PlayerDataResult()
    {
        PlayerData playerData = new PlayerData();
        playerData.userId = 10001;
        playerData.userName = "乔师傅";
        playerData.experience = 100;
        playerData.level = 1;
        playerData.gold = 1000;
        playerData.diamond = 500;

        playerData.haveHeros.Add(1001);
        playerData.haveHeros.Add(1002);
        playerData.haveHeros.Add(1003);
        playerData.haveHeros.Add(1004);
        playerData.haveHeros.Add(1005);

        EventDispatcher.TriggerEvent<PlayerData>(EventsConst.MainScene.PlayerDataResponse, playerData);

    }
}
