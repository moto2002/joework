using UnityEngine;
using System.Collections;
using System;

public class SceneLoadManager
{
    private static SceneLoadManager m_instance;
    public static SceneLoadManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new SceneLoadManager();
            return m_instance;
        }
    }

    public SceneBase m_currentScene = new SceneBase();

	public UnityEngine.Object GetCurrentSceneCacheObj(string cacheName)
	{
		return this.m_currentScene.GetCacheObj(cacheName);
	}

    public void SceneSwitch(SceneBase toScene)
    {
        this.m_currentScene.Destroy();
        this.m_currentScene = toScene;
        toScene.Init();
    }


}
