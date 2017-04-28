using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameListView : MonoBehaviour
{
    private List<GameItem> itemList = new List<GameItem>();

    public Action<string> ShowGameInfo;

    protected void Awake()
    {
        Debug.Log("add GameListView .");
    }

    private void Start()
    {
        int gameCount = CfgCenter.Inst.m_cfgGameListMgr.m_dataList.Count; //游戏列表个数

        for (int index = 0; index < gameCount; index++)
        {
            int id = index + 1; //id看需求
            CfgGameList cfgGameList = CfgCenter.Inst.m_cfgGameListMgr.FindById(id);

            GameItem item = new GameItem(transform.FindChild(index.ToString()).gameObject);
            item.SetData(cfgGameList);
            item.SetClicked(item.Self, () =>
             {
                 if (ShowGameInfo != null)
                 {
                     ShowGameInfo(item.id.ToString());
                 }
             });
            itemList.Add(item);
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
