local transform;
local gameObject;

MessagePanelCtrl = {};
local this = MessagePanelCtrl;

--构建函数--
function MessagePanelCtrl.New()
	logWarn("MessagePanelCtrl.New--->>");
	return this;
end

function MessagePanelCtrl.Awake(xpage)
    --logWarn('MessagePanelCtrl Awake--->>>'..'xpage name:'..xpage.m_pageName);
    xpage.m_pageType = EPageType.PopUp;
    xpage.m_pageMode = EPageMode.HideOtherAndNeedBack;
end

function MessagePanelCtrl.Start()
    logWarn('MessagePanelCtrl Start--->>>');
    local eventTriggerListener = EventTriggerListener.Get(MessagePanelView.btnClose.gameObject);
	eventTriggerListener:AddClick(MessagePanelView.btnClose,this.OnClick);
end

function MessagePanelCtrl.Rest()
    logWarn('MessagePanelCtrl Rest--->>>');
end

function MessagePanelCtrl.Hide()
    logWarn('MessagePanelCtrl Hide--->>>');
end

function MessagePanelCtrl.Destroy()
    logWarn('MessagePanelCtrl Destroy--->>>');
end

--单击事件--
function MessagePanelCtrl.OnClick(go)
	xpageMgr:ShowPage(true,"UI/UIPrefab/TopBar");
	xpageMgr:ShowPage(true,"UI/UIPrefab/Notice");
end