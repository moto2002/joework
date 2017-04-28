using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleScene : SceneBase
{
    public BattleScene()
    {
        //this.m_sceneName = "scene/battlescene";

        this.m_needLoadQueue.Enqueue("scene/scene2");

    }
    public override void Init()
    {
        base.Init();
    }
    public override void SceneLoadedOK()
    {
        GameObject.Instantiate(this.GetCacheObj("scene2") as GameObject);

        Debug.Log("成功进入BattleScene");

        base.SceneLoadedOK();
    }
    public override void Destroy()
    {
        base.Destroy();
    }
}
