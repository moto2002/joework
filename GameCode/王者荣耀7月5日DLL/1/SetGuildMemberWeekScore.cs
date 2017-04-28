using CSProtocol;
using System;

[ArgumentDescription(typeof(int), "个人周积分", new object[]
{

}), CheatCommand("通用/战队/SetGuildMemberWeekScore", "设置个人战队赛周积分(需要重登陆)", 63)]
internal class SetGuildMemberWeekScore : CommonValueChangeCommand
{
	protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
	{
		CheatCmdRef.set_stSetGuildMemberInfo(new CSDT_CHEAT_SET_GUILD_MEMBER_INFO());
		CheatCmdRef.get_stSetGuildMemberInfo().iGuildCoin = -1;
		CheatCmdRef.get_stSetGuildMemberInfo().iGuildMatchScore = -1;
		CheatCmdRef.get_stSetGuildMemberInfo().iGuildMatchWeekScore = InValue;
		CheatCmdRef.get_stSetGuildMemberInfo().iContinueWin = -1;
		CheatCmdRef.get_stSetGuildMemberInfo().iWeekMatchCnt = -1;
	}
}
