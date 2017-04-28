local transform;
local gameObject;
local currPageName;

TopBarCtrl = {};
local this = TopBarCtrl;

--构建函数--
function TopBarCtrl.New()
	logWarn("TopBarCtrl.New--->>");
	return this;
end

function TopBarCtrl.Awake(xpage)
    xpage.m_pageType = EPageType.Fixed;
    xpage.m_pageMode = EPageMode.DoNothing;
    currPageName = xpage.m_pageName;
end

function TopBarCtrl.Start()
    --logWarn('TopBarCtrl Start--->>>');
    local eventTriggerListener = EventTriggerListener.Get(TopBarView.backBtn.gameObject);
	eventTriggerListener:AddClick(TopBarView.backBtn,this.OnBackBtnClick);
end

function TopBarCtrl.Rest()
    --logWarn('TopBarCtrl Rest--->>>');
end

function TopBarCtrl.Hide()
    --logWarn('TopBarCtrl Hide--->>>');
end

function TopBarCtrl.Destroy()
    --logWarn('TopBarCtrl Destroy--->>>');
end
----------------------------------------------------
function TopBarCtrl.OnBackBtnClick(go)
    local success = xpageMgr:HideCurrPage();
    --if(success==false) then
    --   xpageMgr:HidePage("TopBar");
    --end

    if(success==true and xpageMgr:GetNeedBackCount()==0) then
        xpageMgr:HidePage(currPageName);
    end
end