local transform;
local gameObject;

MainPanelCtrl = {};
local this = MainPanelCtrl;

--构建函数--
function MainPanelCtrl.New()
	logWarn("MainPanelCtrl.New--->>");
	return this;
end

function MainPanelCtrl.Awake(xpage)
    --logWarn('MainPanelCtrl Awake--->>>'..'xpage name:'..xpage.m_pageName);
    xpage.m_pageType = EPageType.Normal;
    xpage.m_pageMode = EPageMode.DoNothing;
end

function MainPanelCtrl.Start()
    logWarn('MainPanelCtrl Start--->>>');
    local eventTriggerListener = EventTriggerListener.Get(MainPanelView.packageBtn.gameObject);
	eventTriggerListener:AddClick(MainPanelView.packageBtn,this.OnClick);
end

function MainPanelCtrl.Rest()
    logWarn('MainPanelCtrl Rest--->>>');
end

function MainPanelCtrl.Hide()
    logWarn('MainPanelCtrl Hide--->>>');
end

function MainPanelCtrl.Destroy()
    logWarn('MainPanelCtrl Destroy--->>>');
end

--单击事件--
function MainPanelCtrl.OnClick(go)
	xpageMgr:ShowPage(true,"UI/UIPrefab/TopBar");
	xpageMgr:ShowPage(true,"UI/Prompt/PromptPanel");
end