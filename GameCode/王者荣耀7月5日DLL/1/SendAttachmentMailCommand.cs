using CSProtocol;
using System;

[ArgumentDescription(0, typeof(GMMailAccessCount), "个数", new object[]
{

}), ArgumentDescription(1, typeof(int), "Type1", new object[]
{

}), ArgumentDescription(2, typeof(int), "ID1", new object[]
{

}), ArgumentDescription(3, typeof(int), "Num1", new object[]
{

}), ArgumentDescription(4, typeof(int), "Type2", new object[]
{
	0,
	"二个|三个|四个"
}), ArgumentDescription(5, typeof(int), "ID2", new object[]
{
	0,
	"二个|三个|四个"
}), ArgumentDescription(12, typeof(int), "Num4", new object[]
{
	0,
	"四个"
}), ArgumentDescription(6, typeof(int), "Num2", new object[]
{
	0,
	"二个|三个|四个"
}), ArgumentDescription(11, typeof(int), "ID4", new object[]
{
	0,
	"四个"
}), ArgumentDescription(7, typeof(int), "Type3", new object[]
{
	0,
	"三个|四个"
}), ArgumentDescription(10, typeof(int), "Type4", new object[]
{
	0,
	"四个"
}), ArgumentDescription(8, typeof(int), "ID3", new object[]
{
	0,
	"三个|四个"
}), ArgumentDescription(9, typeof(int), "Num3", new object[]
{
	0,
	"三个|四个"
}), CheatCommand("通用/邮件/SendAttachmentMail", "道具邮件附件数量 个数：一个，二个，三个，四个", 13)]
internal class SendAttachmentMailCommand : CheatCommandNetworking
{
	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		int num = CheatCommandBase.StringToEnum(InArguments[0], typeof(GMMailAccessCount));
		CheatCmdRef.set_stSendMail(new CSDT_CHEAT_SENDMAIL());
		CheatCmdRef.get_stSendMail().bMailType = 1;
		CheatCmdRef.get_stSendMail().bMailsubType = 2;
		CheatCmdRef.get_stSendMail().bAccessCnt = (byte)num;
		CheatCmdRef.get_stSendMail().astAccess = new COMDT_MAILACCESS[num];
		for (int i = 0; i < num; i++)
		{
			CheatCmdRef.get_stSendMail().astAccess[i] = new COMDT_MAILACCESS();
			CheatCmdRef.get_stSendMail().astAccess[i].bGeted = 1;
			CheatCmdRef.get_stSendMail().astAccess[i].bAccessType = 1;
			CheatCmdRef.get_stSendMail().astAccess[i].stAccessInfo = new COMDT_MAILACCESS_INFO();
			CheatCmdRef.get_stSendMail().astAccess[i].stAccessInfo.set_stProp(new COMDT_MAILACCESS_PROP());
			CheatCmdRef.get_stSendMail().astAccess[i].stAccessInfo.get_stProp().wPropType = (ushort)CheatCommandBase.SmartConvert<int>(InArguments[i * 3 + 1]);
			CheatCmdRef.get_stSendMail().astAccess[i].stAccessInfo.get_stProp().dwPropID = (uint)CheatCommandBase.SmartConvert<int>(InArguments[i * 3 + 2]);
			CheatCmdRef.get_stSendMail().astAccess[i].stAccessInfo.get_stProp().iPropNum = CheatCommandBase.SmartConvert<int>(InArguments[i * 3 + 3]);
		}
		return CheatCommandBase.Done;
	}
}
