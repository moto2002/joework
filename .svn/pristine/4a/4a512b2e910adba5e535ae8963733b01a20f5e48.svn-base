using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataPool
{
    private static DataPool m_instance;
    public static DataPool Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new DataPool();
            return m_instance;
        }
    }

    public PlayerData m_localPlayerData { get; set; }


    private Dictionary<int, PlayerData> userIdBindingPlayerData = new Dictionary<int, PlayerData>();

    public void AddPlayerData(int userId, PlayerData playerData)
    {
        if (this.userIdBindingPlayerData.ContainsKey(userId))
        {
            this.userIdBindingPlayerData[userId] = playerData;
        }
        else
        {
            this.userIdBindingPlayerData.Add(userId, playerData);
        }
    }

    public PlayerData GetPlayerData(int userId)
    {
        if (this.userIdBindingPlayerData.ContainsKey(userId))
        {
            return this.userIdBindingPlayerData[userId];
        }
        else
        {
            Debug.Log("no playerData :" + userId);
            return null;
        }
    }

    public void RemovePlayerData(int userId)
    {
        if (this.userIdBindingPlayerData.ContainsKey(userId))
        {
            this.userIdBindingPlayerData.Remove(userId);
        }
    }


}
