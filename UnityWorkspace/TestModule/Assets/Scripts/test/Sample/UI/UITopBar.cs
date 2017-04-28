using UnityEngine;
using System.Collections;
using TinyTeam.UI;
using UnityEngine.UI;

public class UITopBar : TTUIPage
{

    public UITopBar()
        : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/Topbar";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(() =>
        {
            TTUIPageMgr.Instance.HidePage();
            //TTUIPageMgr.Instance.ShowPage<UIMainPage>();
        });

        this.gameObject.transform.Find("btn_notice").GetComponent<Button>().onClick.AddListener(() =>
        {
            TTUIPageMgr.Instance.ShowPage<UINotice>();
            //TTUIPageMgr.Instance.ClosePage<UIMainPage>(true);
        });
    }


}
