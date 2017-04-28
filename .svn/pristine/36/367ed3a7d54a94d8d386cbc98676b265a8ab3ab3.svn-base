using CSProtocol;
using System;

[ArgumentDescription, ArgumentDescription(1, typeof(int), "月", new object[]
{

}), ArgumentDescription(2, typeof(int), "日", new object[]
{

}), ArgumentDescription(3, typeof(int), "时", new object[]
{

}), ArgumentDescription, ArgumentDescription, CheatCommand("工具/服务器/SetPvpBanTime", "设置禁止PVP截止时间", 91)]
internal class SetPvpBanTimeCommand : CheatCommandNetworking
{
	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		CheatCmdRef.set_stSetPvpBanEndTime(new CSDT_CHEAT_SET_OFFSET_SEC());
		CheatCmdRef.get_stSetPvpBanEndTime().iYear = CheatCommandBase.SmartConvert<int>(InArguments[0]);
		CheatCmdRef.get_stSetPvpBanEndTime().iMonth = CheatCommandBase.SmartConvert<int>(InArguments[1]);
		CheatCmdRef.get_stSetPvpBanEndTime().iDay = CheatCommandBase.SmartConvert<int>(InArguments[2]);
		CheatCmdRef.get_stSetPvpBanEndTime().iHour = CheatCommandBase.SmartConvert<int>(InArguments[3]);
		CheatCmdRef.get_stSetPvpBanEndTime().iMin = CheatCommandBase.SmartConvert<int>(InArguments[4]);
		CheatCmdRef.get_stSetPvpBanEndTime().iSec = CheatCommandBase.SmartConvert<int>(InArguments[5]);
		return CheatCommandBase.Done;
	}
}
