using CSProtocol;
using System;

[ArgumentDescription(1, typeof(uint), "英雄ID", new object[]
{

}), ArgumentDescription(2, typeof(uint), "胜场数", new object[]
{

}), ArgumentDescription(3, typeof(uint), "总场数", new object[]
{

}), ArgumentDescription, ArgumentDescription, CheatCommand("通用/排行榜/SetMasterHero", "设置大神排行榜", 75)]
internal class SetMasterHero : CheatCommandNetworking
{
	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		CheatCmdRef.set_stSetMasterHero(new CSDT_CHEAT_SET_MASTERHERO());
		CheatCmdRef.get_stSetMasterHero().dwHeroID = CheatCommandBase.SmartConvert<uint>(InArguments[0]);
		CheatCmdRef.get_stSetMasterHero().dwWinCnt = CheatCommandBase.SmartConvert<uint>(InArguments[1]);
		CheatCmdRef.get_stSetMasterHero().dwGameCnt = CheatCommandBase.SmartConvert<uint>(InArguments[2]);
		CheatCmdRef.get_stSetMasterHero().ullAcntID = CheatCommandBase.SmartConvert<ulong>(InArguments[3]);
		CheatCmdRef.get_stSetMasterHero().iWorldLogicId = CheatCommandBase.SmartConvert<int>(InArguments[4]);
		return CheatCommandBase.Done;
	}
}
