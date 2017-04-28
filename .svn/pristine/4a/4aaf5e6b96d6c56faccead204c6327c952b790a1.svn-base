using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 只能是public的字段
/// public的属性也不行，因为读出来的数据是null
/// </summary>
[System.Serializable]
public class SceneLightmapData : ScriptableObject
{
    public string m_sceneName;

    public string[] m_lightMapFarName;

    public string[] m_lightMapNearName;

    public string m_gameObjectRoot;

    public List<GameObjectData> m_gameObjectList;

    public void ToString()
    {
        Debug.Log("场景名:" + m_sceneName);
        for (int i = 0; i < m_lightMapFarName.Length; i++)
        {
            Debug.Log("lightmapFar" + i + ":" + m_lightMapFarName[i]);
        }
        for (int j = 0; j < m_lightMapNearName.Length; j++)
        {
            Debug.Log("lightmapNear" + j + ":" + m_lightMapNearName[j]);
        }
    }
}