using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VR;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngineInternal;

public class JourneyListView : MonoBehaviour
{
    private List<GameItem> itemList = new List<GameItem>();

    public Action<string> ShowJourneyInfo;

    public IntPtr m_hWnd;
    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(string strClassName, string strWindowName);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int nCmdShow);
    protected void Awake()
    {
        Debug.Log("add GameListView .");
    }

    private void Start()
    {
        int gameCount = 1;// CfgCenter.Inst.m_cfgGameListMgr.m_dataList.Count; //游戏列表个数

        for (int index = 0; index < gameCount; index++)
        {
            int id = index + 1; //id看需求
            //CfgGameList cfgGameList = CfgCenter.Inst.m_cfgGameListMgr.FindById(id);

            GameItem item = new GameItem(transform.FindChild(index.ToString()).gameObject);
            //item.SetData(cfgGameList);
            item.SetData();
            item.SetClicked(item.Self, () =>
             {
                 if (ShowJourneyInfo != null)
                 {
                     ShowJourneyInfo(item.id.ToString());
                 }

                 BtnClicked();

             });
            itemList.Add(item);
        }
    }

    private void BtnClicked()
    {
        if (!isOpening)
        {
            isOpening = true;
            StartExternalApp();
        }

    }
    bool isOpening = false;
    private void StartExternalApp()
    {
        if (GlobalData.m_isFirstEntered)
        {
            PlayTimeMgr.Inst.StartRecord();
            GlobalData.m_isFirstEntered = false;
        }
        string relativePath = @"大雄宝殿/Buddha.exe";
        string _appPath = GlobalData.AppContentPath() + relativePath;
        Debug.Log("_appPath:" + _appPath);
        VRSettings.enabled = false;
        Assist.StartExternalApp(_appPath, () =>
        {
            VRSettings.enabled = true;
            ActiveMainWindow("BlackHole");
            TimerManager.Add(1, (x) => { Camera.main.gameObject.GetComponent<RestVRState>().Rest(); });
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

    public void ShowGameIcons(Action done)
    {
        this.gameObject.SetActive(true);

        ArrayList al = Assist.Tile(new Vector3(-216, -230, 0), itemList.Count, 3, 250, 80);
        for (int j = 0; j < itemList.Count; j++)
        {
            itemList[j].Self.SetActive(false);
        }
        itemList[0].Self.SetActive(true); // 
        // TODO: 动画
        for (int i = 0; i < itemList.Count; i++)
        {
            RectTransform tempRectTrans = itemList[i].Self.GetComponent<RectTransform>();
            Vector3 startPos = new Vector3(((Vector3)al[i]).x, (((Vector3)al[i]).z - 222) * -1 - 300, 0);
            tempRectTrans.anchoredPosition3D = startPos;
            GameObject next = null;
            if (i <= itemList.Count - 2)
                next = itemList[i + 1].Self;
            tempRectTrans.Moveto(new Vector3(startPos.x, startPos.y + 300, startPos.z), 0.3f).setDelay(0.1f * (i + 1)).setEase(LeanTweenType.easeInCubic).onComplete = () =>
            {
                if (next != null)
                    next.SetActive(true);
            };
        }
    }

    public void HideGameIcons(Action done)
    {
        this.gameObject.SetActive(false);

        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].Self.SetActive(false);
        }
        if (done != null)
        {
            done();
        }
    }

    private class GameItem
    {
        public int id;
        public GameObject Self;
        public Image icon;
        public Text name;

        public GameItem(GameObject target)
        {
            Self = target;
            icon = Self.transform.FindChild("icon").GetComponent<Image>();
            name = Self.transform.FindChild("NameBackground/Text").GetComponent<Text>();
        }

        public void SetData(CfgGameList cfgGameList)
        {
            id = cfgGameList._id;
            name.text = cfgGameList._name;
            //Self.name =cfgGameList._id.ToString();
            string iconPath = GameConst.UI_Icon_Path + cfgGameList._icon + ".png";
            icon.sprite = Assist.LoadSprite(iconPath, (int)icon.sprite.rect.width, (int)icon.sprite.rect.height).toSprite();
            Self.SetActive(false);
        }

        public void SetData( )
        {
            id = 1;
            name.text = "大雄宝殿";
            string iconPath = GameConst.UI_Icon_Path +  "Icon_DaXiongBaoDian.png";
            icon.sprite = Assist.LoadSprite(iconPath, (int)icon.sprite.rect.width, (int)icon.sprite.rect.height).toSprite();
            Self.SetActive(false);
        }

        public void SetClicked(GameObject go, Action callback)
        {
            EventTriggerAssist.Get(go).LeftClick += (f) =>
            {
                if (callback != null)
                {
                    callback();
                }
            };
        }
    }
}
