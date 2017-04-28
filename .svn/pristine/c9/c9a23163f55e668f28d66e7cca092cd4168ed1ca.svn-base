using UnityEngine;
using System.Collections;

public class LoginScene : SceneBase
{
    LoginController m_loginController;
    public LoginScene()
    {
        this.m_needLoadQueue.Enqueue("uiprefab/uilogin");
    }


    public override void Init()
    {
        base.Init();
    }

    public override void SceneLoadedOK()
    {
        this.m_loginController = new LoginController();
        TTUIPageMgr.Instance.ShowPage<UILogin>();
        base.SceneLoadedOK();
    }

    public override void Destroy()
    {
        TTUIPageMgr.Instance.ClosePage<UILogin>();
        this.m_loginController.Destroy();
        this.m_loginController = null;
        base.Destroy();
    }
}
