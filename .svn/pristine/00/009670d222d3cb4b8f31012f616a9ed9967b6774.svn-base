using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AppFacade : Facade
{
    private static AppFacade _instance;

    public AppFacade()
        : base()
    {
    }

    public static AppFacade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AppFacade();
            }
            return _instance;
        }
    }

    override protected void InitFramework()
    {
        base.InitFramework();
        RegisterCommand(NotiConst.START_UP, typeof(StartUpCommand));
        RegisterCommand(NotiConst.DISPATCH_MESSAGE, typeof(SocketCommand));
        RegisterCommand(NotiConst.START_VERSION_MGR, typeof(StartVersionMgrCommand));
        
    }

    /// <summary>
    /// 启动框架
    /// </summary>
    public void StartUp(GameObject root)
    {
        SendMessageCommand(NotiConst.START_UP, root);
        RemoveMultiCommand(NotiConst.START_UP);
    }

    public void Destroy()
    {
        RemoveCommand(NotiConst.DISPATCH_MESSAGE);
        RemoveCommand(NotiConst.START_VERSION_MGR);      
    }
}

