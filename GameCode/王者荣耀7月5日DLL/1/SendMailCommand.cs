using Assets.Scripts.GameSystem;
using CSProtocol;
using System;

[ArgumentDescription(0, typeof(GMMailAccessType), "属性类别", new object[]
{

}), ArgumentDescription(1, typeof(int), "数值", new object[]
{

}), ArgumentDescription(2, typeof(string), "好友名称", new object[]
{
	0,
	"心"
}), CheatCommand("通用/邮件/SendMail", "送心送钱邮件", 13)]
internal class SendMailCommand : CheatCommandNetworking
{
	private COMDT_FRIEND_INFO CachedFriendInfo;

	public override bool CheckArguments(string[] InArguments, out string OutMessage)
	{
		this.CachedFriendInfo = null;
		if (!base.CheckArguments(InArguments, ref OutMessage))
		{
			return false;
		}
		if (CheatCommandBase.StringToEnum(InArguments[0], typeof(GMMailAccessType)) == 2)
		{
			if (Singleton<CFriendContoller>.get_instance() != null && Singleton<CFriendContoller>.get_instance().model != null)
			{
				this.CachedFriendInfo = Singleton<CFriendContoller>.get_instance().model.getFriendByName(InArguments[2], CFriendModel.FriendType.GameFriend);
			}
			if (this.CachedFriendInfo == null)
			{
				OutMessage = string.Format("当前命令无法在当前状态下被使用或者无法找名称为\"{0}\"的好友", InArguments[2]);
				return false;
			}
		}
		return true;
	}

	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		GMMailAccessType gMMailAccessType = (GMMailAccessType)CheatCommandBase.StringToEnum(InArguments[0], typeof(GMMailAccessType));
		CheatCmdRef.set_stSendMail(new CSDT_CHEAT_SENDMAIL());
		if (gMMailAccessType == GMMailAccessType.金币)
		{
			CheatCmdRef.get_stSendMail().bMailType = 1;
			CheatCmdRef.get_stSendMail().bMailsubType = 2;
			CheatCmdRef.get_stSendMail().bAccessCnt = 1;
			CheatCmdRef.get_stSendMail().astAccess = new COMDT_MAILACCESS[1];
			CheatCmdRef.get_stSendMail().astAccess[0] = new COMDT_MAILACCESS();
			CheatCmdRef.get_stSendMail().astAccess[0].bGeted = 1;
			CheatCmdRef.get_stSendMail().astAccess[0].bAccessType = 4;
			CheatCmdRef.get_stSendMail().astAccess[0].stAccessInfo = new COMDT_MAILACCESS_INFO();
			CheatCmdRef.get_stSendMail().astAccess[0].stAccessInfo.set_stRongYu(new COMDT_MAILACCESS_RONGYU());
			CheatCmdRef.get_stSendMail().astAccess[0].stAccessInfo.get_stRongYu().dwRongYuPoint = (uint)CheatCommandBase.SmartConvert<int>(InArguments[1]);
		}
		else if (gMMailAccessType == GMMailAccessType.钻石)
		{
			CheatCmdRef.get_stSendMail().bMailType = 1;
			CheatCmdRef.get_stSendMail().bMailsubType = 2;
			CheatCmdRef.get_stSendMail().bAccessCnt = 1;
			CheatCmdRef.get_stSendMail().astAccess = new COMDT_MAILACCESS[1];
			CheatCmdRef.get_stSendMail().astAccess[0] = new COMDT_MAILACCESS();
			CheatCmdRef.get_stSendMail().astAccess[0].bGeted = 1;
			CheatCmdRef.get_stSendMail().astAccess[0].bAccessType = 2;
			CheatCmdRef.get_stSendMail().astAccess[0].stAccessInfo = new COMDT_MAILACCESS_INFO();
			CheatCmdRef.get_stSendMail().astAccess[0].stAccessInfo.set_stMoney(new COMDT_MAILACCESS_MONEY());
			CheatCmdRef.get_stSendMail().astAccess[0].stAccessInfo.get_stMoney().bType = 7;
			CheatCmdRef.get_stSendMail().astAccess[0].stAccessInfo.get_stMoney().dwMoney = (uint)CheatCommandBase.SmartConvert<int>(InArguments[1]);
		}
		else if (gMMailAccessType == GMMailAccessType.心)
		{
			DebugHelper.Assert(this.CachedFriendInfo != null);
			CheatCmdRef.get_stSendMail().szTo = this.CachedFriendInfo.szUserName;
			CheatCmdRef.get_stSendMail().ullToUid = this.CachedFriendInfo.stUin.ullUid;
			CheatCmdRef.get_stSendMail().dwToLogicWorld = this.CachedFriendInfo.stUin.dwLogicWorldId;
			CheatCmdRef.get_stSendMail().bMailType = 2;
			CheatCmdRef.get_stSendMail().bMailsubType = 2;
			CheatCmdRef.get_stSendMail().bAccessCnt = 1;
			CheatCmdRef.get_stSendMail().astAccess = new COMDT_MAILACCESS[1];
			CheatCmdRef.get_stSendMail().astAccess[0] = new COMDT_MAILACCESS();
			CheatCmdRef.get_stSendMail().astAccess[0].bGeted = 1;
			CheatCmdRef.get_stSendMail().astAccess[0].bAccessType = 3;
			CheatCmdRef.get_stSendMail().astAccess[0].stAccessInfo = new COMDT_MAILACCESS_INFO();
			CheatCmdRef.get_stSendMail().astAccess[0].stAccessInfo.set_stHeart(new COMDT_MAILACCESS_HEART());
			CheatCmdRef.get_stSendMail().astAccess[0].stAccessInfo.get_stHeart().dwHeart = (uint)CheatCommandBase.SmartConvert<int>(InArguments[1]);
		}
		return CheatCommandBase.Done;
	}
}
