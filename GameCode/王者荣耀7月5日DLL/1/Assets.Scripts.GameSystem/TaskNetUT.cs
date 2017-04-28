using Assets.Scripts.Framework;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using UnityEngine;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class TaskNetUT
	{
		[MessageHandler(1504)]
		public static void OnTaskUpdate(CSPkg msg)
		{
			Singleton<CTaskSys>.get_instance().On_TASKUPD_NTF(ref msg);
		}

		[MessageHandler(1501)]
		public static void OnDailyTaskRes(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CTaskSys>.get_instance().OnUSUALTASK_RES(ref msg);
		}

		[MessageHandler(1507)]
		public static void OnSCID_NEWTASKGET_NTF(CSPkg msg)
		{
			Singleton<CTaskSys>.get_instance().OnSCID_NEWTASKGET_NTF(ref msg);
		}

		public static void Send_Update_Task(uint taskid = 0u)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1500u);
			cSPkg.stPkgData.get_stUsualTaskReq().bRefreshMethod = ((taskid != 0u) ? 1 : 0);
			cSPkg.stPkgData.get_stUsualTaskReq().dwTaskID = taskid;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, taskid != 0u);
		}

		public static void Send_SubmitTask(uint taskid)
		{
			if (taskid == 0u)
			{
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1502u);
			cSPkg.stPkgData.get_stSubmitTaskReq().dwTaskID = taskid;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		[MessageHandler(1503)]
		public static void OnTASKSUBMIT_RES(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CTaskSys>.get_instance().OnTASKSUBMIT_RES(ref msg);
		}

		public static void Send_GetHuoyue_Reward(ushort id)
		{
			if (id == 0)
			{
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1601u);
			cSPkg.stPkgData.get_stHuoYueDuRewardReq().wHuoYueDuId = id;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		[MessageHandler(1602)]
		public static void OnHuoyue_Reward_RES(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CTaskSys>.get_instance().OnHuoyue_Reward_RES(ref msg);
		}

		[MessageHandler(1600)]
		public static void OnHuoyue_Info_NTF(CSPkg msg)
		{
			Singleton<CTaskSys>.get_instance().OnHuoyue_Info_NTF(ref msg);
		}

		[MessageHandler(1603)]
		public static void OnHuoyue_Error_NTF(CSPkg msg)
		{
			Singleton<CTaskSys>.get_instance().OnHuoyue_Error_NTF(ref msg);
		}

		[MessageHandler(2612)]
		public static void OnMonthWeekCardUseRsp(CSPkg pkg)
		{
			SCPKG_MONTH_WEEK_CARD_USE_RSP stMonthWeekCardUseRsp = pkg.stPkgData.get_stMonthWeekCardUseRsp();
			RES_PROP_VALFUNC_TYPE bCardType = stMonthWeekCardUseRsp.bCardType;
			Singleton<CTaskSys>.get_instance().SetCardExpireTime(bCardType, stMonthWeekCardUseRsp.dwExpireTime);
			Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText((bCardType != 8) ? "WeekCardTakeEffect" : "MonthCardTakeEffect"), false, 2f, null, new object[0]);
		}

		[MessageHandler(1508)]
		public static void OnSCID_DELTASK_NTF(CSPkg pkg)
		{
			SCPKG_DELTASK_NTF stDelTask = pkg.stPkgData.get_stDelTask();
			int num = 0;
			while ((long)num < (long)((ulong)stDelTask.dwTaskCnt))
			{
				SCDT_DELTASK sCDT_DELTASK = stDelTask.astNewTask[num];
				if (sCDT_DELTASK != null)
				{
					Singleton<CTaskSys>.get_instance().model.Remove(sCDT_DELTASK.dwTaskID);
					if (sCDT_DELTASK.dwTaskType == 0u)
					{
						if (sCDT_DELTASK.bTaskSubType == 3)
						{
							Singleton<CTaskSys>.get_instance().masterTaskID = 0u;
							Singleton<EventRouter>.get_instance().BroadCastEvent("TASK_Mentor_UPDATED");
						}
						else if (sCDT_DELTASK.bTaskSubType == 4)
						{
							Singleton<CTaskSys>.get_instance().apprenticeTaskID = 0u;
							Singleton<EventRouter>.get_instance().BroadCastEvent("TASK_Mentor_UPDATED");
						}
					}
					Singleton<EventRouter>.get_instance().BroadCastEvent("TaskUpdated");
					if (Singleton<CTaskSys>.get_instance().m_taskView != null)
					{
						Singleton<CTaskSys>.get_instance().m_taskView.Refresh();
					}
				}
				num++;
			}
		}

		public static void Send_Get_Level_Reward_Request(int level)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1614u);
			cSPkg.stPkgData.get_stLevelRewardReq().iLevel = level;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		[MessageHandler(1615)]
		public static void OnSCID_GETPVPLEVELREWARD_RSP(CSPkg pkg)
		{
			SCPKG_GETPVPLEVELREWARD_RSP stLevelRewardRsp = pkg.stPkgData.get_stLevelRewardRsp();
			if (stLevelRewardRsp.iErrorCode == 0)
			{
				Singleton<CTaskSys>.get_instance().model.SyncServerLevelRewardFlagData(stLevelRewardRsp.ullLevelRewardFlag);
				ListView<CUseable> useableListFromReward = CUseableManager.GetUseableListFromReward(stLevelRewardRsp.stRewardInfo);
				Singleton<CUIManager>.GetInstance().OpenAwardTip(LinqS.ToArray<CUseable>(useableListFromReward), null, false, enUIEventID.None, false, false, "Form_Award");
				Singleton<CTaskSys>.get_instance().m_taskView.Refresh();
			}
			else
			{
				Debug.LogError("---领取等级奖励 response error, errorcode:" + stLevelRewardRsp.iErrorCode);
			}
		}
	}
}
