using UnityEngine;
using System.Collections;
using ShiHuanJue.Event;
using System;

public class MainController
{
    public MainController()
    {
        EventDispatcher.AddEventListener<PlayerData>(EventsConst.MainScene.PlayerDataResponse, OnPlayerDataResponse);
    }

    private void OnPlayerDataResponse(PlayerData obj)
    {
        DataPool.Instance.m_localPlayerData = obj;
        DataPool.Instance.AddPlayerData(obj.userId,obj);
    }

    public void Destroy()
    {
        EventDispatcher.RemoveEventListener<PlayerData>(EventsConst.MainScene.PlayerDataResponse, OnPlayerDataResponse);
    }
}
