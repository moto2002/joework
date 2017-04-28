local transform;
local gameObject;

LoginPanelView = {};
local this = LoginPanelView;


function LoginPanelView.Start(obj)
	gameObject = obj;
	transform = obj.transform;
    logWarn('LoginPanelView Start--->>>'..gameObject.name);

 	local userNameGo = transform:FindChild("InputField_UserName").gameObject;
 	local passWordGo = transform:FindChild("InputField_PassWord").gameObject;
    this.userName =userNameGo:GetComponent("InputField");
    this.passWord =passWordGo:GetComponent("InputField");
    this.loginBtn = transform:FindChild("Button_Login").gameObject;
end

