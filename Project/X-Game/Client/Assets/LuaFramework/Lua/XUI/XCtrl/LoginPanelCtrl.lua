require "Common/define"
local transform;
local gameObject;

local sproto = require "3rd/sproto/sproto"
local core = require "sproto.core"

LoginPanelCtrl = {};
local this = LoginPanelCtrl;

--构建函数--
function LoginPanelCtrl.New()
	logWarn("LoginPanelCtrl.New--->>");
	return this;
end

function LoginPanelCtrl.Awake(xpage)
    --logWarn('LoginPanelCtrl Awake--->>>'..'xpage name:'..xpage.m_pageName);
    xpage.m_pageType = EPageType.Normal;
    xpage.m_pageMode = EPageMode.DoNothing;
end

function LoginPanelCtrl.Start()
    logWarn('LoginPanelCtrl Start--->>>');
    local eventTriggerListener = EventTriggerListener.Get(LoginPanelView.loginBtn.gameObject);
	eventTriggerListener:AddClick(LoginPanelView.loginBtn,this.OnClick);
end

function LoginPanelCtrl.Rest()
    logWarn('LoginPanelCtrl Rest--->>>');
end

function LoginPanelCtrl.Hide()
    logWarn('LoginPanelCtrl Hide--->>>');
end

function LoginPanelCtrl.Destroy()
    logWarn('LoginPanelCtrl Destroy--->>>');
end

--单击事件--
function LoginPanelCtrl.OnClick(go)
    logWarn(LoginPanelView.userName.text.."|"..LoginPanelView.passWord.text);
    xpageMgr:ShowPage(true,"UI/UIPrefab/MainPanel");

    if TestProtoType == ProtocalType.BINARY then
        this.TestSendBinary();
    end
    if TestProtoType == ProtocalType.PB_LUA then
        this.TestSendPblua();
    end
    if TestProtoType == ProtocalType.PBC then
        this.TestSendPbc();
    end
    if TestProtoType == ProtocalType.SPROTO then
        this.TestSendSproto();
    end
    logWarn("OnClick---->>>"..go.name);
end

--测试发送SPROTO--
function LoginPanelCtrl.TestSendSproto()
    local sp = sproto.parse [[
    .Person {
        name 0 : string
        id 1 : integer
        email 2 : string

        .PhoneNumber {
            number 0 : string
            type 1 : integer
        }

        phone 3 : *PhoneNumber
    }

    .AddressBook {
        person 0 : *Person(id)
        others 1 : *Person
    }
    ]]

    local ab = {
        person = {
            [10000] = {
                name = "Alice",
                id = 10000,
                phone = {
                    { number = "123456789" , type = 1 },
                    { number = "87654321" , type = 2 },
                }
            },
            [20000] = {
                name = "Bob",
                id = 20000,
                phone = {
                    { number = "01234567890" , type = 3 },
                }
            }
        },
        others = {
            {
                name = "Carol",
                id = 30000,
                phone = {
                    { number = "9876543210" },
                }
            },
        }
    }
    local code = sp:encode("AddressBook", ab)
    ----------------------------------------------------------------
    local buffer = ByteBuffer.New();
    --buffer:WriteShort(Protocal.Message);
    --buffer:WriteByte(ProtocalType.SPROTO);
    buffer:WriteBuffer(code);
    networkMgr:SendMessage(Protocal.Message,buffer);
end

--测试发送PBC--
function LoginPanelCtrl.TestSendPbc()
    local path = Util.DataPath.."lua/3rd/pbc/addressbook.pb";

    local addr = io.open(path, "rb")
    local buffer = addr:read "*a"
    addr:close()
    protobuf.register(buffer)

    local addressbook = {
        name = "Alice",
        id = 12345,
        phone = {
            { number = "1301234567" },
            { number = "87654321", type = "WORK" },
        }
    }
    local code = protobuf.encode("tutorial.Person", addressbook)
    ----------------------------------------------------------------
    local buffer = ByteBuffer.New();
    buffer:WriteShort(Protocal.Message);
    buffer:WriteByte(ProtocalType.PBC);
    buffer:WriteBuffer(code);
    networkMgr:SendMessage(buffer);
end

--测试发送PBLUA--
function LoginPanelCtrl.TestSendPblua()
    local login = login_pb.LoginRequest();
    login.id = 2000;
    login.name = 'game';
    login.email = 'jarjin@163.com';
    local msg = login:SerializeToString();
    ----------------------------------------------------------------
    local buffer = ByteBuffer.New();
    buffer:WriteShort(Protocal.Message);
    buffer:WriteByte(ProtocalType.PB_LUA);
    buffer:WriteBuffer(msg);
    networkMgr:SendMessage(buffer);
end

--测试发送二进制--
function LoginPanelCtrl.TestSendBinary()
    local buffer = ByteBuffer.New();
    buffer:WriteShort(Protocal.Message);
    buffer:WriteByte(ProtocalType.BINARY);
    buffer:WriteString("ffff我的ffffQ靈uuu");
    buffer:WriteInt(200);
    networkMgr:SendMessage(buffer);
end
