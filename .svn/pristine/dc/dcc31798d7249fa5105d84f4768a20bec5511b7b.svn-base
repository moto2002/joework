using CSProtocol;
using System;

[ArgumentDescription, ArgumentDescription(1, typeof(int), "月", new object[]
{

}), ArgumentDescription(2, typeof(int), "日", new object[]
{

}), ArgumentDescription(3, typeof(int), "时", new object[]
{

}), ArgumentDescription, ArgumentDescription, CheatCommand("工具/服务器/SetServerTime", "设置服务器时间", 31)]
internal class SetServerTimeCommand : CheatCommandNetworking
{
	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		CheatCmdRef.set_stSetOffsetSec(new CSDT_CHEAT_SET_OFFSET_SEC());
		CheatCmdRef.get_stSetOffsetSec().iYear = CheatCommandBase.SmartConvert<int>(InArguments[0]);
		CheatCmdRef.get_stSetOffsetSec().iMonth = CheatCommandBase.SmartConvert<int>(InArguments[1]);
		CheatCmdRef.get_stSetOffsetSec().iDay = CheatCommandBase.SmartConvert<int>(InArguments[2]);
		CheatCmdRef.get_stSetOffsetSec().iHour = CheatCommandBase.SmartConvert<int>(InArguments[3]);
		CheatCmdRef.get_stSetOffsetSec().iMin = CheatCommandBase.SmartConvert<int>(InArguments[4]);
		CheatCmdRef.get_stSetOffsetSec().iSec = CheatCommandBase.SmartConvert<int>(InArguments[5]);
		return CheatCommandBase.Done;
	}
}
