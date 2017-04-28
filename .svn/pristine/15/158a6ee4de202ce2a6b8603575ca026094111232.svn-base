require "Logic/LuaClass"
require "Common/define"
require "Common/functions"
require "Cfg/CfgDataCenter"

XGame = {};
local this = XGame;

local game; 
local transform;
local gameObject;
local WWW = UnityEngine.WWW;

--初始化完成，--
function XGame.OnInitOK()
	this.ConnectServer();--发送链接服务器信息
    this.InitViewPanels();

    local cfg_player_data = TemplatePlayerGet('Leonard','classHuman');
    logWarn('----------------:'..cfg_player_data.job)
    local temp_v = cfg_player_data.addPointOrder;
    for i,v in ipairs(temp_v) do
    	--print('\n'..i,v)
    end
end

--------------------------------------------------------------
function XGame.InitViewPanels()
	for i = 1, #XPanelNames do
		require ("XUI/XView/"..tostring(XPanelNames[i]).."View")
        require ("XUI/XCtrl/"..tostring(XPanelNames[i]).."Ctrl")
	end
end

function XGame.ConnectServer()
	AppConst.SocketPort = 10060;
    AppConst.SocketAddress = "127.0.0.1";
    networkMgr:SendConnect();
    logWarn('ConnectServer--->>>'..'port:'..AppConst.SocketPort..'ip:'..AppConst.SocketAddress);
end