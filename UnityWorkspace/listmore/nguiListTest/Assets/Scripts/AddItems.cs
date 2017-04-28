using UnityEngine;
using System.Collections;

public class AddItems : MonoBehaviour
{
    static int index;

    GameObject item;

    void Start()
    {
        item = Resources.Load("Hero") as GameObject;
        UIGridDyn grid = GameObject.Find("UI Root/Camera/Panel/Grid").gameObject.GetComponent<UIGridDyn>();
        grid.srcObj = item;
        index = 0;
    }

    void OnClick()
    {
        UIGridDyn grid = GameObject.Find("UI Root/Camera/Panel/Grid").gameObject.GetComponent<UIGridDyn>();
        for (int i = 0; i < 10; i++)
        {
            grid.AddItem(index.ToString(), new FormationItem(i, i));
            index++;

        }
    }

    public class FormationItem : UIGridDyn.IItemObj
    {
        public LGButton btnDetail;
        public UITexture headIcon;
        public UISprite armyTypeIcon;
        public GameObject fight;
        public UILabel level;
        public UILabel name;
        public UILabel att;
        public UILabel hp;
        //public HeroData myHero;
        public int hid;
        public int itemid;
        public FormationItem(int _hid, int _tagid)
        {
            hid = _hid;
            itemid = _tagid;
        }

        public void OnItemShow(string key, GameObject head)
        {
            btnDetail = head.transform.FindChild("btnDetail").GetComponent<LGButton>();
            btnDetail.Team = "detail";
            btnDetail.Tag = hid.ToString();
            headIcon = head.transform.FindChild("Head").GetComponent<UITexture>();
            armyTypeIcon = head.transform.FindChild("ArmyType").GetComponent<UISprite>();
            fight = head.transform.FindChild("Fight").gameObject;
            level = head.transform.FindChild("Level").GetComponent<UILabel>();
            name = head.transform.FindChild("Name").GetComponent<UILabel>();
            att = head.transform.FindChild("Att").GetComponent<UILabel>();
            hp = head.transform.FindChild("HP").GetComponent<UILabel>();
            //head.AddComponent<DragDropItem>();

            armyTypeIcon.spriteName = "Archer";
            level.text = key;
            name.text = "HERO";
            att.text = "1024";
            hp.text = "2048";
            fight.SetActive(true);
        }

        public void OnItemHide(string key, GameObject obj)
        {

        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 50), "Count=" + index);
    }
}
