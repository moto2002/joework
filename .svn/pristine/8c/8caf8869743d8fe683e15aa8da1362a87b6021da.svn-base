local transform;
local gameObject;

NoticeCtrl = {};
local this = NoticeCtrl;

--构建函数--
function NoticeCtrl.New()
	logWarn("NoticeCtrl.New--->>");
	return this;
end

function NoticeCtrl.Awake(xpage)
    --logWarn('NoticeCtrl Awake--->>>'..'xpage name:'..xpage.m_pageName);
   	xpage.m_pageType = EPageType.PopUp;
    xpage.m_pageMode = EPageMode.HideOtherAndNeedBack;
end

function NoticeCtrl.Start()
    logWarn('NoticeCtrl Start--->>>');
    local eventTriggerListener = EventTriggerListener.Get(NoticeView.confimBtn.gameObject);
	eventTriggerListener:AddClick(NoticeView.confimBtn,this.OnClick);
end

function NoticeCtrl.Rest()
    logWarn('NoticeCtrl Rest--->>>');
end

function NoticeCtrl.Hide()
    logWarn('NoticeCtrl Hide--->>>');
end

function NoticeCtrl.Destroy()
    logWarn('NoticeCtrl Destroy--->>>');
end

--单击事件--
function NoticeCtrl.OnClick(go)
	--xpageMgr:ShowPage(true,"UI/Prompt/PromptPanel");
end