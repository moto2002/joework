using Assets.Scripts.GameSystem;
using CSProtocol;
using System;

[ArgumentDescription, ArgumentDescription, ArgumentDescription, ArgumentDescription, CheatCommand("关卡/SetMobaInfo", "设置mobalevel", 79)]
internal class CheatCommandSetMobaLevel : CheatCommandNetworking
{
	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		CheatCmdRef.set_stSetMobaInfo(new CSDT_CHEAT_SET_MOBA_INFO());
		int num = CheatCommandBase.SmartConvert<int>(InArguments[0]);
		int num2 = CheatCommandBase.SmartConvert<int>(InArguments[1]);
		int num3 = CheatCommandBase.SmartConvert<int>(InArguments[2]);
		int num4 = CheatCommandBase.SmartConvert<int>(InArguments[3]);
		CheatCmdRef.get_stSetMobaInfo().stMobaInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().acntMobaInfo;
		if (num != -2)
		{
			if (num < 0 || num > 4)
			{
				return "错误的mobaLevel";
			}
			CheatCmdRef.get_stSetMobaInfo().stMobaInfo.iMobaLevel = num;
		}
		if (num2 != -2)
		{
			if (num2 < -1 || num2 > 7)
			{
				return "错误的heroType";
			}
			CheatCmdRef.get_stSetMobaInfo().stMobaInfo.iSelectedHeroType = num2;
		}
		if (num3 != -2)
		{
			if (num3 < -1 || num3 > 7)
			{
				return "错误的heroType";
			}
			CheatCmdRef.get_stSetMobaInfo().stMobaInfo.iRecommendHeroType = num3;
		}
		if (num4 != -2)
		{
			if (num4 < 0 || num4 > 2)
			{
				return "错误的mobaUsedType";
			}
			CheatCmdRef.get_stSetMobaInfo().stMobaInfo.bMobaUsedType = (byte)num4;
		}
		return CheatCommandBase.Done;
	}
}
