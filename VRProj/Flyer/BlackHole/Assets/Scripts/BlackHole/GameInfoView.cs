using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VR;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngineInternal;

public class GameInfoView : MonoBehaviour
{
    public Action BackToGameListEvent;

    private GameObject m_uiRoot, m_startGameBtnGo, m_returnBtnGo,m_restBtnGo;

    private Text m_title,m_desc,m_type;

    private Image m_screenShot,m_icon;

    private List<TypeWordExample> m_typeWordEffectScript = new List<TypeWordExample>();

    private CfgGameList cfgGameList;

    public IntPtr m_hWnd;
    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(string strClassName, string strWindowName);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int nCmdShow);

    System.Diagnostics.Process HookProcess;

    bool isOpening = false;

    private void Awake()
    {
        Debug.Log("add GameInfoView .");
        m_uiRoot = GameObject.Find("UIRoot");
        m_startGameBtnGo = transform.FindChild("Start").gameObject;
        m_returnBtnGo = transform.FindChild("Return").gameObject;
        m_restBtnGo = transform.FindChild("Rest").gameObject;
        m_title = transform.FindChild("Title/Text").GetComponent<Text>();
        m_title.text = "";
        m_typeWordEffectScript.Add(m_title.gameObject.GetComponent<TypeWordExample>());
        m_desc = transform.FindChild("Desc").GetComponent<Text>();
        m_desc.text = "";
        m_typeWordEffectScript.Add(m_desc.gameObject.GetComponent<TypeWordExample>());
        m_screenShot = transform.FindChild("ScreenShot/Background").GetComponent<Image>();
        m_type = transform.FindChild("Icon/type").GetComponent<Text>();
        m_type.text = "";
        m_typeWordEffectScript.Add(m_type.gameObject.GetComponent<TypeWordExample>());
        m_icon = transform.FindChild("Game/icon").GetComponent<Image>();
        
        gameObject.SetActive(false);
     
        EventTriggerAssist.Get(m_startGameBtnGo).LeftClick += (e)=> 
        {
            Debug.Log("Start some Game");

            
            if(!isOpening)
            {
                isOpening = true;
                StartExternalApp();
            }
    
        };

        EventTriggerAssist.Get(m_returnBtnGo).LeftClick += (e)=> 
        {
            
            for (int i = 0; i < m_typeWordEffectScript.Count; i++)
            {
                m_typeWordEffectScript[i].StopTypeWordEffect();
            }

            if (BackToGameListEvent != null)
            {
                BackToGameListEvent();
            }
        };

        EventTriggerAssist.Get(m_restBtnGo).LeftClick += (e) =>
        {
            RestUIForwardCamera();
        };
    }

    private void StartExternalApp()
    {
        if (GlobalData.m_isFirstEntered)
        {
            PlayTimeMgr.Inst.StartRecord();
            GlobalData.m_isFirstEntered = false;
        }
           
        // StartHookWindows();
        RecordMgr.Inst.StartRecord(cfgGameList);
        string relativePath = cfgGameList._relativePath;
        string _appPath = GlobalData.AppContentPath() + relativePath;
        Debug.Log("_appPath:"+_appPath);
        VRSettings.enabled = false;
        Assist.StartExternalApp(_appPath, () =>
        {
            VRSettings.enabled = true;
            RecordMgr.Inst.StopRecord(cfgGameList);
            // HookProcess.Kill();
            ActiveMainWindow("BlackHole");
            TimerManager.Add(1, (x)=> { Camera.main.gameObject.GetComponent<RestVRState>().Rest(); });
            isOpening = false;
        });
    }
   
    private void ActiveMainWindow(string wndName)
    {

        IntPtr hWndPalaz = FindWindow(null, wndName);//就是窗体的的标题
       // Debug.Log(hWndPalaz);
        if (hWndPalaz != null)
        {
            //获得游戏本身句柄 
            m_hWnd = FindWindow("UnityWndClass", null);

            //激活，最大化
            ShowWindow(m_hWnd, 4);
        }
    }
   
    private void StartHookWindows()
    {
        string hookExe = @"Hook\HookWindowsDemo.exe";
        string m_ralativePath = GlobalData.AppContentPath() + hookExe;

        HookProcess = new System.Diagnostics.Process();
        HookProcess.StartInfo.UseShellExecute = false;
        HookProcess.StartInfo.FileName = m_ralativePath;
        HookProcess.StartInfo.CreateNoWindow = true;
        HookProcess.Start();
        // myProcess.WaitForExit();
    }

    //重置UI面朝Camera前方
    private void RestUIForwardCamera()
    {
        //Debug.Log("重置UI面朝Camera前方");
        InitUIRootPos();
    }
    private void InitUIRootPos()
    {
        m_uiRoot.transform.SetParent(Camera.main.transform);
        m_uiRoot.transform.localPosition = Vector3.zero;
        m_uiRoot.transform.localScale = Vector3.one;
        m_uiRoot.transform.localRotation = Quaternion.identity;
        m_uiRoot.transform.SetParent(null);
    }

    private void BackToGameList(PointerEventData obj)
    {
        throw new NotImplementedException();
    }

    private void StartGame(PointerEventData obj)
    {
        throw new NotImplementedException();
    }
  
    public void Show(string str)
    {
        print("Game info show...\n");
        gameObject.SetActive(true);

        int gameId;
        if (int.TryParse(str, out gameId))
        {
            cfgGameList = CfgCenter.Inst.m_cfgGameListMgr.FindById(gameId);

            m_title.text = cfgGameList._name;
            m_desc.text = cfgGameList._desc;
            string iconPath =GameConst.UI_Icon_Path + cfgGameList._icon + ".png";
            m_icon.sprite = Assist.LoadSprite(iconPath, (int)m_icon.sprite.rect.width, (int)m_icon.sprite.rect.height).toSprite();
            string screenShotPath = GameConst.UI_ScreenShot_Path + cfgGameList._screenShot + ".png";
            m_screenShot.sprite = Assist.LoadSprite(screenShotPath, (int)m_screenShot.sprite.rect.width, (int)m_screenShot.sprite.rect.height).toSprite();
            m_type.text = cfgGameList._type;
            m_type.text = m_type.text.Replace("\\n", "\n");

            for (int i = 0; i < m_typeWordEffectScript.Count; i++)
            {
                m_typeWordEffectScript[i].PlayTypeWordEffect();
            }
        }
    }

    public void Hide(Action done)
    {
        gameObject.SetActive(false);
        if (done != null)
        {
            done();
        }
    }
}
