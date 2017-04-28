using CSProtocol;
using System;

[ArgumentDescription(typeof(int), "活跃值ID", new object[]
{

}), CheatCommand("工具/游戏/活跃度/Reset_Huoyue", "重置活跃值", 57)]
internal class ResetHuoYueCommand : CommonValueChangeCommand
{
	protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
	{
		CheatCmdRef.set_stHuoYueDuOpt(new CSDT_CHEAT_HUOYUEDU());
		CheatCmdRef.get_stHuoYueDuOpt().iValue = InValue;
		CheatCmdRef.get_stHuoYueDuOpt().bOpt = 3;
	}
}
