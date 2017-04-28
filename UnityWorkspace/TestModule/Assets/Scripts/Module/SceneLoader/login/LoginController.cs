using UnityEngine;
using System.Collections;
using ShiHuanJue.Event;
using System;

public class LoginController
{
    public LoginController()
    {
        EventDispatcher.AddEventListener<string, string>(EventsConst.LoginScene.LoginRequest, OnLoginRequest);
        EventDispatcher.AddEventListener<bool>(EventsConst.LoginScene.LoginResponse, OnLoginResponse);
    }

    private void OnLoginResponse(bool obj)
    {
        if (obj)
        {
            SceneLoadManager.Instance.GoToMainScene();
        }
        else
        {
            Debug.LogError("登录失败");
        }
    }

    private void OnLoginRequest(string arg1, string arg2)
    {
        Debug.Log(arg1 + "|" + arg2);

        //发给服务器

        ServerSimluator.Instance.LoginResult(arg1, arg2);
    }

    public void Destroy()
    {
        EventDispatcher.RemoveEventListener<string, string>(EventsConst.LoginScene.LoginRequest, OnLoginRequest);
        EventDispatcher.RemoveEventListener<bool>(EventsConst.LoginScene.LoginResponse, OnLoginResponse);
    }
}
