using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using System;
using UnityEngine;

[CheatCommandEntry("性能")]
internal class CheatCommandReplayEntry
{
	public static bool heroPerformanceTest;

	public static bool commitFOWMaterial = true;

	[CheatCommandEntryMethod("单英雄测试 开", true, false)]
	public static string SingleHeroTestOn()
	{
		CheatCommandReplayEntry.heroPerformanceTest = true;
		return CheatCommandBase.Done;
	}

	[CheatCommandEntryMethod("单英雄测试 关", true, false)]
	public static string SingleHeroTestOff()
	{
		CheatCommandReplayEntry.heroPerformanceTest = false;
		return CheatCommandBase.Done;
	}

	[CheatCommandEntryMethod("PROFILE!", true, false)]
	public static string Profile()
	{
		MonoSingleton<ConsoleWindow>.get_instance().set_isVisible(false);
		MonoSingleton<SProfiler>.GetInstance().ToggleVisible();
		return CheatCommandBase.Done;
	}

	[CheatCommandEntryMethod("PROFILE 300->5000!", true, false)]
	public static string Profile5000Auto()
	{
		FrameCommand<SwitchActorAutoAICommand> frameCommand = FrameCommandFactory.CreateFrameCommand<SwitchActorAutoAICommand>();
		frameCommand.cmdData.IsAutoAI = 1;
		frameCommand.Send();
		MonoSingleton<ConsoleWindow>.get_instance().set_isVisible(false);
		MonoSingleton<SProfiler>.GetInstance().StartProfileNFrames(5000, 300u);
		return CheatCommandBase.Done;
	}

	[CheatCommandEntryMethod("PROFILE 5000!", true, false)]
	public static string Profile5000()
	{
		MonoSingleton<ConsoleWindow>.get_instance().set_isVisible(false);
		MonoSingleton<SProfiler>.GetInstance().StartProfileNFrames(5000, 0u);
		return CheatCommandBase.Done;
	}

	[CheatCommandEntryMethod("PROFILE 10000!", true, false)]
	public static string Profile10000()
	{
		MonoSingleton<ConsoleWindow>.get_instance().set_isVisible(false);
		MonoSingleton<SProfiler>.GetInstance().StartProfileNFrames(10000, 0u);
		return CheatCommandBase.Done;
	}

	[CheatCommandEntryMethod(" 锁帧模式", true, false)]
	public static string LockFPS()
	{
		GameFramework instance = MonoSingleton<GameFramework>.GetInstance();
		instance.LockFPS_SGame = !instance.LockFPS_SGame;
		return (!instance.LockFPS_SGame) ? "UNITY" : "SGAME";
	}

	[CheatCommandEntryMethod("Battle Form模式", true, false)]
	public static string BattleFormCanvasMode()
	{
		Canvas component = Singleton<CBattleSystem>.GetInstance().FormScript.gameObject.GetComponent<Canvas>();
		if (component.renderMode == RenderMode.ScreenSpaceCamera)
		{
			component.renderMode = RenderMode.WorldSpace;
		}
		else if (component.renderMode == RenderMode.WorldSpace)
		{
			component.renderMode = RenderMode.ScreenSpaceCamera;
		}
		return component.renderMode.ToString();
	}

	[CheatCommandEntryMethod("小地图开关", true, false)]
	public static string MapPanelSwitch()
	{
		GameObject gameObject = Singleton<CBattleSystem>.GetInstance().FormScript.transform.Find("MapPanel").gameObject;
		gameObject.SetActive(!gameObject.activeSelf);
		return CheatCommandBase.Done;
	}

	[CheatCommandEntryMethod("迷雾材质提交开关", true, false)]
	public static string CommitFOWMaterialSwitch()
	{
		CheatCommandReplayEntry.commitFOWMaterial = !CheatCommandReplayEntry.commitFOWMaterial;
		return CheatCommandReplayEntry.commitFOWMaterial.ToString();
	}

	[CheatCommandEntryMethod(" 关闭战斗UI", true, false)]
	public static string CloseFormBattle()
	{
		CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(FightForm.s_battleUIForm);
		if (form != null)
		{
			form.gameObject.CustomSetActive(false);
			return "已关闭";
		}
		return "不在战斗中";
	}

	[CheatCommandEntryMethod(" 战斗UI停止绘制", true, false)]
	public static string StopFormBattleDraw()
	{
		CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(FightForm.s_battleUIForm);
		if (form != null)
		{
			form.gameObject.CustomSetActive(true);
			form.Hide(enFormHideFlag.HideByCustom, true);
			return "已停止绘制";
		}
		return "不在战斗中";
	}

	[CheatCommandEntryMethod(" 战斗UI恢复显示和绘制", true, false)]
	public static string ShowFormBattle()
	{
		CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(FightForm.s_battleUIForm);
		if (form != null)
		{
			form.gameObject.CustomSetActive(true);
			form.Appear(enFormHideFlag.HideByCustom, true);
			return "已恢复";
		}
		return "不在战斗中";
	}
}
