using CSProtocol;
using System;

[ArgumentDescription(typeof(int), "数量", new object[]
{

}), CheatCommand("英雄/属性修改/经验等级/AddExpPool", "加经验池", 15)]
internal class AddExpPoolCommand : CommonValueChangeCommand
{
	protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
	{
		CheatCmdRef.set_stUpdAcntInfo(new CSDT_CHEAT_UPDACNTINFO());
		CheatCmdRef.get_stUpdAcntInfo().iUpdType = 15;
		CheatCmdRef.get_stUpdAcntInfo().iUpdValue = InValue;
	}
}
