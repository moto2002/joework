local transform;
local gameObject;

MainPanelView = {};
local this = MainPanelView;


function MainPanelView.Start(obj)
	gameObject = obj;
	transform = obj.transform;
    logWarn('MainPanelView Start--->>>'..gameObject.name);

    this.packageBtn = transform:FindChild("Button").gameObject;
end

