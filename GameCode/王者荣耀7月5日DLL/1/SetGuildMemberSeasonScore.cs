using CSProtocol;
using System;

[ArgumentDescription(typeof(int), "个人赛季积分", new object[]
{

}), CheatCommand("通用/战队/SetGuildMemberSeasonScore", "设置个人战队赛赛季积分(需要重登陆)", 63)]
internal class SetGuildMemberSeasonScore : CommonValueChangeCommand
{
	protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
	{
		CheatCmdRef.set_stSetGuildMemberInfo(new CSDT_CHEAT_SET_GUILD_MEMBER_INFO());
		CheatCmdRef.get_stSetGuildMemberInfo().iGuildCoin = -1;
		CheatCmdRef.get_stSetGuildMemberInfo().iGuildMatchScore = InValue;
		CheatCmdRef.get_stSetGuildMemberInfo().iGuildMatchWeekScore = -1;
		CheatCmdRef.get_stSetGuildMemberInfo().iContinueWin = -1;
		CheatCmdRef.get_stSetGuildMemberInfo().iWeekMatchCnt = -1;
	}
}
