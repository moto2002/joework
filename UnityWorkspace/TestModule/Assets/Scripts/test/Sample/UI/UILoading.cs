using UnityEngine;
using System.Collections;
using TinyTeam.UI;
using UnityEngine.UI;
using ShiHuanJue.Event;

public class UILoading : TTUIPage
{
    public UILoading()
        : base(UIType.PopUp, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/UILoading";
    }

    Slider progress;
    Image background;
    Text tip;

    public override void Awake(GameObject go)
    {
        this.progress = this.transform.Find("Progress").GetComponent<Slider>();
        this.background = this.transform.Find("Image").GetComponent<Image>();
        this.tip = this.transform.Find("Text").GetComponent<Text>();
    }

    public override void Refresh()
    {
        base.Refresh();
        this.OnUpdateProgress(0);
        EventDispatcher.AddEventListener<float>(EventsConst.UILoading.UpdateProgress, OnUpdateProgress);
    }

    public override void Hide()
    {
        base.Hide();
        EventDispatcher.RemoveEventListener<float>(EventsConst.UILoading.UpdateProgress, OnUpdateProgress);
    }

    private void OnUpdateProgress(float progress)
    {
        this.progress.value = progress;
    }





}
