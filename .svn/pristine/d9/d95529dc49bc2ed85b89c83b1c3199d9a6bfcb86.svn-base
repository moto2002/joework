using CSProtocol;
using System;

[ArgumentDescription(0, typeof(int), "ID", new object[]
{

}), ArgumentDescription(1, typeof(int), "等级", new object[]
{

}), CheatCommand("英雄/属性/SetHeroLvl", "设置英雄等级", 10)]
internal class SetHeroLvlCommand : CheatCommandNetworking
{
	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		int dwHeroID = CheatCommandBase.SmartConvert<int>(InArguments[0]);
		int iValue = CheatCommandBase.SmartConvert<int>(InArguments[1]);
		CheatCmdRef.set_stSetHeroLvl(new CSDT_CHEAT_HEROVAL());
		CheatCmdRef.get_stSetHeroLvl().dwHeroID = (uint)dwHeroID;
		CheatCmdRef.get_stSetHeroLvl().iValue = iValue;
		return CheatCommandBase.Done;
	}
}
