using CSProtocol;
using System;

[ArgumentDescription(2, typeof(int), "数量", new object[]
{

}), ArgumentDescription(0, typeof(EPropTypeType), "物品类型", new object[]
{

}), ArgumentDescription(1, typeof(int), "ID", new object[]
{

}), CheatCommand("英雄/属性/AddItem", "添加物品", 7)]
internal class AddItemCommand : CheatCommandNetworking
{
	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		EPropTypeType ePropTypeType = CheatCommandBase.SmartConvert<EPropTypeType>(InArguments[0]);
		int dwItemID = CheatCommandBase.SmartConvert<int>(InArguments[1]);
		int num = CheatCommandBase.SmartConvert<int>(InArguments[2]);
		CheatCmdRef.set_stAddItem(new CSDT_CHEAT_ITEMINFO());
		CheatCmdRef.get_stAddItem().wItemType = (ushort)ePropTypeType;
		CheatCmdRef.get_stAddItem().dwItemID = (uint)dwItemID;
		CheatCmdRef.get_stAddItem().wItemCnt = (ushort)num;
		return CheatCommandBase.Done;
	}
}
