using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScene : SceneBase
{
    MainController m_mainController;
    public MainScene()
    {
        this.m_needLoadQueue.Enqueue("uiprefab/notice");
        this.m_needLoadQueue.Enqueue("uiprefab/topbar");
        this.m_needLoadQueue.Enqueue("uiprefab/uibattle");
        this.m_needLoadQueue.Enqueue("uiprefab/uimain");
        this.m_needLoadQueue.Enqueue("uiprefab/uiroot");
        this.m_needLoadQueue.Enqueue("uiprefab/uiskill");
    }

    public override void Init()
    {
        base.Init();
    }

    public override void SceneLoadedOK()
    {
        this.m_mainController = new MainController();
        TTUIPageMgr.Instance.ShowPage<UITopBar>();
        TTUIPageMgr.Instance.ShowPage<UIMainPage>();

        base.SceneLoadedOK();
    }
    public override void Destroy()
    {
        TTUIPageMgr.Instance.ClosePage<UITopBar>();
        TTUIPageMgr.Instance.ClosePage<UIMainPage>();
        TTUIPageMgr.Instance.ClosePage<UIBattle>();
        TTUIPageMgr.Instance.ClosePage<UISkillPage>();
        TTUIPageMgr.Instance.ClosePage<UINotice>();

        this.m_mainController.Destroy();
        this.m_mainController = null;
        base.Destroy();
    }
}
