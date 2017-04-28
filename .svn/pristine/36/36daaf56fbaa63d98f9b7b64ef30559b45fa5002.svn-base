using CSProtocol;
using System;

[ArgumentDescription(typeof(int), "战队币", new object[]
{

}), CheatCommand("通用/战队/SetGuildMemberConstruct", "设置个人战队币", 63)]
internal class SetGuildMemberCoin : CommonValueChangeCommand
{
	protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
	{
		CheatCmdRef.set_stSetGuildMemberInfo(new CSDT_CHEAT_SET_GUILD_MEMBER_INFO());
		CheatCmdRef.get_stSetGuildMemberInfo().iGuildCoin = InValue;
		CheatCmdRef.get_stSetGuildMemberInfo().iGuildMatchScore = -1;
		CheatCmdRef.get_stSetGuildMemberInfo().iGuildMatchWeekScore = -1;
		CheatCmdRef.get_stSetGuildMemberInfo().iContinueWin = -1;
		CheatCmdRef.get_stSetGuildMemberInfo().iWeekMatchCnt = -1;
	}
}
