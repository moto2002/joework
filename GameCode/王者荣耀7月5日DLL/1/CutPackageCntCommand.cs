using CSProtocol;
using System;

[ArgumentDescription(typeof(int), "数量", new object[]
{

}), CheatCommand("英雄/属性修改/其它/CutPackageCnt", "缩减背包容量", 47)]
internal class CutPackageCntCommand : CommonValueChangeCommand
{
	protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
	{
		CheatCmdRef.set_stCutPackageCnt(new CSDT_CHEAT_COMVAL());
		CheatCmdRef.get_stCutPackageCnt().iValue = InValue;
	}
}
