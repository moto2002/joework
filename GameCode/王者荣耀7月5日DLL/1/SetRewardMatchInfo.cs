using CSProtocol;
using System;

[ArgumentDescription, ArgumentDescription, ArgumentDescription(1, typeof(uint), "地图ID", new object[]
{

}), ArgumentDescription, ArgumentDescription, CheatCommand("通用/赏金赛/SetRewardMatchPoint", "变更赏金赛信息", 73)]
internal class SetRewardMatchInfo : CheatCommandNetworking
{
	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		CheatCmdRef.set_stChgRewardMatchInfo(new CSDT_CHEAT_CHG_REWARDMATCH_INFO());
		CheatCmdRef.get_stChgRewardMatchInfo().dwMapId = CheatCommandBase.SmartConvert<uint>(InArguments[0]);
		CheatCmdRef.get_stChgRewardMatchInfo().iWinCnt = CheatCommandBase.SmartConvert<int>(InArguments[1]);
		CheatCmdRef.get_stChgRewardMatchInfo().iLossCnt = CheatCommandBase.SmartConvert<int>(InArguments[2]);
		CheatCmdRef.get_stChgRewardMatchInfo().iMatchCnt = CheatCommandBase.SmartConvert<int>(InArguments[3]);
		CheatCmdRef.get_stChgRewardMatchInfo().iPerfectCnt = CheatCommandBase.SmartConvert<int>(InArguments[4]);
		return CheatCommandBase.Done;
	}
}
