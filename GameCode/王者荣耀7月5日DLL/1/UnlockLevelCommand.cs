using Assets.Scripts.Framework;
using CSProtocol;
using ResData;
using System;

[ArgumentDescription(0, typeof(int), "章节", new object[]
{

}), ArgumentDescription, ArgumentDescription(1, typeof(int), "序号", new object[]
{

}), CheatCommand("关卡/UnlockLevel", "解锁闯关关卡", 12)]
internal class UnlockLevelCommand : CheatCommandNetworking
{
	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		int Chapter = CheatCommandBase.SmartConvert<int>(InArguments[0]);
		int No = CheatCommandBase.SmartConvert<int>(InArguments[1]);
		ELevelTypeTag eLevelTypeTag = CheatCommandBase.SmartConvert<ELevelTypeTag>(InArguments[2]);
		CheatCmdRef.set_stUnlockLevel(new CSDT_CHEAT_UNLOCK_LEVEL());
		RES_LEVEL_DIFFICULTY_TYPE DiffType = (eLevelTypeTag != ELevelTypeTag.普通) ? 2 : 1;
		ResLevelCfgInfo resLevelCfgInfo = GameDataMgr.levelDatabin.FindIf((ResLevelCfgInfo x) => x.iChapterId == Chapter && (int)x.bLevelNo == No && x.bLevelDifficulty == DiffType);
		if (resLevelCfgInfo != null)
		{
			CheatCmdRef.get_stUnlockLevel().iLevelID = resLevelCfgInfo.iCfgID;
			return CheatCommandBase.Done;
		}
		return string.Format("未找到 {2} {0}-{1}对应地图配置", Chapter, No, eLevelTypeTag.ToString());
	}
}
