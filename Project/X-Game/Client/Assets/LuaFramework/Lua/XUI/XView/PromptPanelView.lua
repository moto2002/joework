local transform;
local gameObject;

PromptPanelView = {};
local this = PromptPanelView;


function PromptPanelView.Start(obj)
	gameObject = obj;
	transform = obj.transform;
    logWarn('PromptPanelView Start--->>>'..gameObject.name);

   	this.btnOpen = transform:FindChild("Open").gameObject;
	this.gridParent = transform:FindChild('ScrollView/Grid');
end

