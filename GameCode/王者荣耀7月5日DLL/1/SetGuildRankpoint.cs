using CSProtocol;
using System;

[ArgumentDescription(typeof(int), "战队竞技点(活跃点)", new object[]
{

}), CheatCommand("通用/战队/SetGuildRankpoint", "设置战队竞技点(活跃点)", 40)]
internal class SetGuildRankpoint : CommonValueChangeCommand
{
	protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
	{
		CheatCmdRef.set_stSetGuildInfo(new CSDT_CHEAT_SET_GUILD_INFO());
		CheatCmdRef.get_stSetGuildInfo().iActive = InValue;
		CheatCmdRef.get_stSetGuildInfo().iGuildMatchScore = -1;
		CheatCmdRef.get_stSetGuildInfo().iGuildMatchWeekScore = -1;
		CheatCmdRef.get_stSetGuildInfo().iGuildMatchWeekRankNo = -1;
		CheatCmdRef.get_stSetGuildInfo().iGuildMatchSeasonRankNo = -1;
	}
}
