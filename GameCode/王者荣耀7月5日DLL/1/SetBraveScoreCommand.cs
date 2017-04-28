using CSProtocol;
using System;

[ArgumentDescription(typeof(int), "数值", new object[]
{

}), CheatCommand("英雄/英雄排位赛/SetBraveScore", "设置勇者积分", 90)]
internal class SetBraveScoreCommand : CommonValueChangeCommand
{
	protected override void FillMessageField(ref CSDT_CHEATCMD_DETAIL CheatCmdRef, int InValue)
	{
		CheatCmdRef.set_stSetAddStarScore(new CSDT_CHEAT_COMVAL());
		CheatCmdRef.get_stSetAddStarScore().iValue = InValue;
	}
}
