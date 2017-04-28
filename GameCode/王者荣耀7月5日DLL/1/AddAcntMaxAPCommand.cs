using CSProtocol;
using System;

[ArgumentDescription(typeof(int), "体力值", new object[]
{

}), CheatCommand("英雄/属性修改/数值/AddAcntMaxAP", "设置最大体力值", 5)]
internal class AddAcntMaxAPCommand : CommonValueChangeCommand
{
	protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
	{
		CheatCmdRef.set_stAddAcntMaxAP(new CSDT_CHEAT_COMVAL());
		CheatCmdRef.get_stAddAcntMaxAP().iValue = InValue;
	}
}
