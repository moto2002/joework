using UnityEngine;
using System.Collections;
using LuaFramework;

public class StartUpCommand : ControllerCommand
{

    public override void Execute(IMessage message)
    {
        if (!Util.CheckEnvironment()) 
            return;

        GameObject root = message.Body as GameObject;
        AppFacade.Instance.SetGameRoot(root);

        root.AddComponent<ShowFPS>(); //fps
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = AppConst.GameFrameRate;//关闭垂直同步后，可以可以根据情况，不同的场景，设置不同的帧率

        //-----------------初始化管理器-----------------------
        AppFacade.Instance.AddManager<NetworkManager>(ManagerName.Network);
        AppFacade.Instance.AddManager<PanelManager>(ManagerName.Panel);
        AppFacade.Instance.AddManager<SoundManager>(ManagerName.Sound);
        AppFacade.Instance.AddManager<TimerManager>(ManagerName.Timer);
        AppFacade.Instance.AddManager<LuaManager>(ManagerName.Lua);
        AppFacade.Instance.AddManager<ResourceManager>(ManagerName.Resource);
        AppFacade.Instance.AddManager<ThreadManager>(ManagerName.Thread);

        //AppFacade.Instance.AddManager<GameManager>(ManagerName.Game);

        AppFacade.Instance.SendMessageCommand(NotiConst.START_VERSION_MGR, root);
    }
}