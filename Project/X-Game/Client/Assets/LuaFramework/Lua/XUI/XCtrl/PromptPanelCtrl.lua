local transform;
local gameObject;

PromptPanelCtrl = {};
local this = PromptPanelCtrl;

--构建函数--
function PromptPanelCtrl.New()
	logWarn("PromptPanelCtrl.New--->>");
	return this;
end

function PromptPanelCtrl.Awake(xpage)
    --logWarn('PromptPanelCtrl Awake--->>>'..'xpage name:'..xpage.m_pageName);
    xpage.m_pageType = EPageType.PopUp;
    xpage.m_pageMode = EPageMode.HideOtherAndNeedBack;
end

function PromptPanelCtrl.Start()
    logWarn('PromptPanelCtrl Start--->>>');
    resMgr:LoadPrefab('UI/Prompt/PromptItem.unity3d', { 'PromptItem' }, this.InitPanel);
end

function PromptPanelCtrl.Rest()
    logWarn('PromptPanelCtrl Rest--->>>');
end

function PromptPanelCtrl.Hide()
    logWarn('PromptPanelCtrl Hide--->>>');
end

function PromptPanelCtrl.Destroy()
    logWarn('PromptPanelCtrl Destroy--->>>');
end

-------------------------------------------------
--初始化面板--
function PromptPanelCtrl.InitPanel(objs)
    local count = 100; 
    local parent = PromptPanelView.gridParent;
    for i = 1, count do
        local go = newObject(objs[0]);
        go.name = 'Item'..tostring(i);
        go.transform:SetParent(parent);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;

        local eventTriggerListener = EventTriggerListener.Get(go);
        eventTriggerListener:AddClick(go,this.OnItemClick);

        local label = go.transform:FindChild('Text');
        label:GetComponent('Text').text = tostring(i);
    end
end

--滚动项单击--
function PromptPanelCtrl.OnItemClick(go)
    log(go.name);
    xpageMgr:ShowPage(true,"UI/UIPrefab/Notice");
end