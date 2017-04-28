using UnityEngine;
using System.Collections;
using TinyTeam.UI;
using UnityEngine.UI;
using System;
using ShiHuanJue.Event;

public class UILogin : TTUIPage
{
    private InputField InputField_userName;
    private InputField InputField_passWord;
    private Button Button_enterGame;

    public UILogin()
        : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UILogin";
    }

    public override void Awake(GameObject go)
    {
        this.InputField_userName = this.transform.Find("InputField_userName").GetComponent<InputField>();
        this.InputField_passWord = this.transform.Find("InputField_passWord").GetComponent<InputField>();
        this.Button_enterGame = this.transform.Find("Button_enterGame").GetComponent<Button>();
        this.InputField_userName.text = "joe";
        this.InputField_passWord.text = "123";
        this.Button_enterGame.onClick.AddListener(OnLoginGame);
    }

    private void OnLoginGame()
    {
        string userName = this.InputField_userName.text;
        string passWord = this.InputField_passWord.text;
        EventDispatcher.TriggerEvent<string, string>(EventsConst.LoginScene.LoginRequest,userName,passWord);
    }

    public override void Close()
    {
        this.Button_enterGame.onClick.RemoveListener(OnLoginGame);

        base.Close();
    }
}
