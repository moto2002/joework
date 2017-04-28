using Assets.Scripts.UI;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class CFriendMentorTaskView
	{
		public enum EMentorTaskState
		{
			None,
			Empty,
			Tudi_Task,
			TudiTaskFinish_No_MasterTask,
			MasterTask,
			AllFinish
		}

		private enum enMentorTaskViewWidget
		{
			None = -1,
			TopNode,
			TopNodeHeaderTxt,
			TopNodeDescTxt,
			MiddleNode,
			MiddleNodeHeaderTxt,
			MiddleUnlockNode,
			MiddleDoingNode,
			BottomNode,
			BottomNodeHeaderTxt,
			BottomGetRewardBtn,
			BottomUnFinishInfo,
			BottomRewardContainer
		}

		public struct TaskViewConditionData
		{
			public bool bValid;

			public bool bFinish;

			public string condition;

			public string progress;

			public uint taskId;

			public int tag;

			public enUIEventID m_onClickEventID;

			public bool bShowGoToBtn;

			public void Clear()
			{
				this.bValid = false;
			}
		}

		public List<CFriendMentorTaskView.TaskViewConditionData> taskViewConditionDataList = new List<CFriendMentorTaskView.TaskViewConditionData>();

		public void Open()
		{
			Singleton<EventRouter>.get_instance().AddEventHandler("TaskUpdated", new Action(this.OnMentorTaskUpdate));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Task_Mentor_GetReward, new CUIEventManager.OnUIEventHandler(this.On_Task_Submit));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Task_Mentor_Close, new CUIEventManager.OnUIEventHandler(this.On_Task_Mentor_Close));
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CFriendContoller.MentorTaskFormPath, false, true);
			this.ShowMenu();
		}

		public void ShowMenu()
		{
			CFriendMentorTaskView.EMentorTaskState mentorTaskState = Singleton<CTaskSys>.get_instance().MentorTaskState;
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CFriendContoller.MentorTaskFormPath);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.FindChild("content/title").gameObject;
			GameObject gameObject2 = form.transform.FindChild("content/top").gameObject;
			GameObject gameObject3 = form.transform.FindChild("content/middle").gameObject;
			GameObject gameObject4 = form.transform.FindChild("content/bottom").gameObject;
			GameObject gameObject5 = form.transform.FindChild("content/info").gameObject;
			if (mentorTaskState == CFriendMentorTaskView.EMentorTaskState.Empty)
			{
				gameObject.CustomSetActive(false);
				gameObject2.CustomSetActive(false);
				gameObject3.CustomSetActive(false);
				gameObject4.CustomSetActive(false);
				gameObject5.CustomSetActive(true);
				gameObject5.transform.FindChild("txt").GetComponent<Text>().text = Singleton<CTextManager>.get_instance().GetText("MTV_Empty");
			}
			else if (mentorTaskState == CFriendMentorTaskView.EMentorTaskState.Tudi_Task)
			{
				gameObject.CustomSetActive(true);
				gameObject2.CustomSetActive(true);
				gameObject3.CustomSetActive(true);
				gameObject4.CustomSetActive(true);
				gameObject5.CustomSetActive(false);
				this.showTask(Singleton<CTaskSys>.get_instance().apprenticeTaskID);
			}
			else if (mentorTaskState == CFriendMentorTaskView.EMentorTaskState.TudiTaskFinish_No_MasterTask)
			{
				gameObject.CustomSetActive(false);
				gameObject2.CustomSetActive(false);
				gameObject3.CustomSetActive(false);
				gameObject4.CustomSetActive(false);
				gameObject5.CustomSetActive(true);
				gameObject5.transform.FindChild("txt").GetComponent<Text>().text = Singleton<CTextManager>.get_instance().GetText("MTV_MiddleEmpty");
			}
			else if (mentorTaskState == CFriendMentorTaskView.EMentorTaskState.MasterTask)
			{
				gameObject.CustomSetActive(true);
				gameObject2.CustomSetActive(true);
				gameObject3.CustomSetActive(true);
				gameObject4.CustomSetActive(true);
				gameObject5.CustomSetActive(false);
				this.showTask(Singleton<CTaskSys>.get_instance().masterTaskID);
			}
			else if (mentorTaskState == CFriendMentorTaskView.EMentorTaskState.AllFinish)
			{
				gameObject.CustomSetActive(false);
				gameObject2.CustomSetActive(false);
				gameObject3.CustomSetActive(false);
				gameObject4.CustomSetActive(false);
				gameObject5.CustomSetActive(true);
				gameObject5.transform.FindChild("txt").GetComponent<Text>().text = Singleton<CTextManager>.get_instance().GetText("MTV_AllFinish");
			}
		}

		private void On_Task_Mentor_Close(CUIEvent uievent)
		{
			this.Clear();
		}

		public void Clear()
		{
			Singleton<EventRouter>.get_instance().RemoveEventHandler("TaskUpdated", new Action(this.OnMentorTaskUpdate));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Task_Mentor_GetReward, new CUIEventManager.OnUIEventHandler(this.On_Task_Submit));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Task_Mentor_Close, new CUIEventManager.OnUIEventHandler(this.On_Task_Mentor_Close));
			if (Singleton<CFriendContoller>.get_instance().view != null)
			{
				Singleton<CFriendContoller>.get_instance().view.Refresh_Tab();
			}
		}

		public void showTask(uint taskid)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CFriendContoller.MentorTaskFormPath);
			if (form == null)
			{
				return;
			}
			CTask task = Singleton<CTaskSys>.get_instance().model.GetTask(taskid);
			if (task == null)
			{
				return;
			}
			Text componetInChild = Utility.GetComponetInChild<Text>(form.gameObject, "content/title/taskName");
			componetInChild.text = task.m_taskTitle;
			form.GetWidget(1).GetComponent<Text>().text = Singleton<CTextManager>.get_instance().GetText("MTV_TopNodeHeaderTxt");
			form.GetWidget(2).GetComponent<Text>().text = task.m_taskDesc;
			form.GetWidget(4).GetComponent<Text>().text = Singleton<CTextManager>.get_instance().GetText("MTV_MiddleNodeHeaderTxt");
			List<CFriendMentorTaskView.TaskViewConditionData> list = this.CalcParam(task, null, false);
			int num = Math.Min(3, list.get_Count());
			for (int i = 0; i < num; i++)
			{
				CFriendMentorTaskView.TaskViewConditionData taskViewConditionData = list.get_Item(i);
				GameObject widget = form.GetWidget(6);
				GameObject gameObject = widget.transform.FindChild(string.Format("cond_{0}", i)).gameObject;
				if (!(gameObject == null))
				{
					if (taskViewConditionData.bValid)
					{
						gameObject.CustomSetActive(true);
						gameObject.transform.FindChild("desc").GetComponent<Text>().text = taskViewConditionData.condition;
						gameObject.transform.FindChild("progress").GetComponent<Text>().text = taskViewConditionData.progress;
						CUIEventScript component = gameObject.transform.FindChild("btns/goto_btn").GetComponent<CUIEventScript>();
						if (component != null)
						{
							component.m_onClickEventID = taskViewConditionData.m_onClickEventID;
							component.m_onClickEventParams.tag = taskViewConditionData.tag;
							component.m_onClickEventParams.taskId = taskViewConditionData.taskId;
						}
						if (taskViewConditionData.bFinish)
						{
							component.gameObject.CustomSetActive(false);
						}
						else
						{
							component.gameObject.CustomSetActive(taskViewConditionData.bShowGoToBtn);
						}
						gameObject.transform.FindChild("btns/Text").gameObject.CustomSetActive(taskViewConditionData.bFinish);
					}
					else
					{
						gameObject.CustomSetActive(false);
					}
				}
			}
			CTaskView.CTaskUT.ShowTaskAward(form, task, form.GetWidget(11), 4);
			if (task.m_taskState == 1)
			{
				GameObject gameObject2 = form.GetWidget(9).gameObject;
				gameObject2.CustomSetActive(true);
				gameObject2.GetComponent<CUIEventScript>().m_onClickEventParams.tagUInt = task.m_baseId;
				form.GetWidget(10).gameObject.CustomSetActive(false);
			}
			else
			{
				form.GetWidget(9).gameObject.CustomSetActive(false);
				form.GetWidget(10).gameObject.CustomSetActive(false);
			}
		}

		private void OnMentorTaskUpdate()
		{
			this.ShowMenu();
		}

		public void ShowTask(uint taskID)
		{
		}

		private void On_Task_Submit(CUIEvent uiEvent)
		{
			uint tagUInt = uiEvent.m_eventParams.tagUInt;
			DebugHelper.Assert(tagUInt > 0u, "---ctask Submit task, taskid should > 0");
			if (tagUInt > 0u)
			{
				TaskNetUT.Send_SubmitTask(tagUInt);
			}
		}

		public List<CFriendMentorTaskView.TaskViewConditionData> CalcParam(CTask task, GameObject goto_obj, bool isMonthWeekCard = false)
		{
			if (task == null)
			{
				return null;
			}
			for (int i = 0; i < this.taskViewConditionDataList.get_Count(); i++)
			{
				this.taskViewConditionDataList.get_Item(i).Clear();
			}
			this.taskViewConditionDataList.Clear();
			if (isMonthWeekCard)
			{
				goto_obj.CustomSetActive(true);
				CFriendMentorTaskView.TaskViewConditionData taskViewConditionData = default(CFriendMentorTaskView.TaskViewConditionData);
				taskViewConditionData.bValid = true;
				taskViewConditionData.m_onClickEventID = enUIEventID.Mall_Open_Factory_Shop_Tab;
				this.taskViewConditionDataList.Add(taskViewConditionData);
				taskViewConditionData.bShowGoToBtn = true;
			}
			else
			{
				for (int j = 0; j < task.m_prerequisiteInfo.Length; j++)
				{
					CFriendMentorTaskView.TaskViewConditionData taskViewConditionData2 = default(CFriendMentorTaskView.TaskViewConditionData);
					taskViewConditionData2.bValid = false;
					taskViewConditionData2.bFinish = task.m_prerequisiteInfo[j].m_isReach;
					ResDT_PrerequisiteInTask resDT_PrerequisiteInTask = task.m_resTask.astPrerequisiteArray[j];
					taskViewConditionData2.condition = resDT_PrerequisiteInTask.szPrerequisiteDesc;
					RES_PERREQUISITE_TYPE conditionType = task.m_prerequisiteInfo[j].m_conditionType;
					if (task.m_prerequisiteInfo[j].m_valueTarget > 0)
					{
						taskViewConditionData2.bValid = true;
						if (conditionType != 3 && conditionType != 13)
						{
							taskViewConditionData2.progress = string.Concat(new object[]
							{
								task.m_prerequisiteInfo[j].m_value,
								"/",
								task.m_prerequisiteInfo[j].m_valueTarget,
								" "
							});
						}
						if (!task.m_prerequisiteInfo[j].m_isReach)
						{
							if (conditionType == 2)
							{
								int iParam = task.m_resTask.astPrerequisiteArray[j].astPrerequisiteParam[3].iParam;
								if ((float)iParam == Mathf.Pow(2f, 0f))
								{
									goto_obj.CustomSetActive(true);
									taskViewConditionData2.bShowGoToBtn = true;
									taskViewConditionData2.taskId = task.m_baseId;
									taskViewConditionData2.tag = task.m_resTask.astPrerequisiteArray[j].astPrerequisiteParam[4].iParam;
									taskViewConditionData2.m_onClickEventID = enUIEventID.Task_LinkPve;
								}
								else if ((float)iParam == Mathf.Pow(2f, 7f))
								{
									goto_obj.CustomSetActive(true);
									taskViewConditionData2.bShowGoToBtn = true;
									taskViewConditionData2.m_onClickEventID = enUIEventID.Burn_OpenForm;
								}
								else if ((float)iParam == Mathf.Pow(2f, 8f))
								{
									goto_obj.CustomSetActive(true);
									taskViewConditionData2.bShowGoToBtn = true;
									taskViewConditionData2.m_onClickEventID = enUIEventID.Arena_OpenForm;
								}
							}
							else if (conditionType == 1 || conditionType == 16)
							{
								goto_obj.CustomSetActive(true);
								taskViewConditionData2.bShowGoToBtn = true;
								taskViewConditionData2.m_onClickEventID = enUIEventID.Matching_OpenEntry;
								taskViewConditionData2.tag = 0;
							}
							else if (conditionType == 6)
							{
								goto_obj.CustomSetActive(false);
								taskViewConditionData2.bShowGoToBtn = false;
								taskViewConditionData2.m_onClickEventID = enUIEventID.Friend_OpenForm;
							}
							else if (conditionType == 13)
							{
								goto_obj.CustomSetActive(true);
								taskViewConditionData2.bShowGoToBtn = true;
								taskViewConditionData2.m_onClickEventID = enUIEventID.Matching_OpenEntry;
								taskViewConditionData2.tag = 0;
							}
							else if (conditionType == 17)
							{
								if ((long)task.m_resTask.astPrerequisiteArray[0].astPrerequisiteParam[1].iParam == 2L)
								{
									goto_obj.CustomSetActive(true);
									taskViewConditionData2.bShowGoToBtn = true;
									taskViewConditionData2.m_onClickEventID = enUIEventID.Matching_OpenEntry;
									taskViewConditionData2.tag = 3;
								}
								else
								{
									goto_obj.CustomSetActive(false);
									taskViewConditionData2.bShowGoToBtn = false;
								}
							}
							else if (conditionType == 20)
							{
								goto_obj.CustomSetActive(true);
								taskViewConditionData2.bShowGoToBtn = true;
								taskViewConditionData2.m_onClickEventID = enUIEventID.Arena_OpenForm;
							}
							else if (conditionType == 12)
							{
								goto_obj.CustomSetActive(true);
								taskViewConditionData2.bShowGoToBtn = true;
								taskViewConditionData2.m_onClickEventID = enUIEventID.Symbol_OpenForm;
							}
							else if (conditionType == 18)
							{
								goto_obj.CustomSetActive(true);
								taskViewConditionData2.bShowGoToBtn = true;
								taskViewConditionData2.m_onClickEventID = enUIEventID.Mall_Open_Factory_Shop_Tab;
							}
							else if (conditionType == 8)
							{
								goto_obj.CustomSetActive(true);
								taskViewConditionData2.bShowGoToBtn = true;
								taskViewConditionData2.m_onClickEventID = enUIEventID.Lottery_Open_Form;
							}
							else if (conditionType == 26)
							{
								goto_obj.CustomSetActive(true);
								taskViewConditionData2.bShowGoToBtn = true;
								if ((long)task.m_resTask.astPrerequisiteArray[0].astPrerequisiteParam[1].iParam == 1L)
								{
									taskViewConditionData2.m_onClickEventID = enUIEventID.Mall_GotoDianmondTreasureTab;
								}
								else if ((long)task.m_resTask.astPrerequisiteArray[0].astPrerequisiteParam[1].iParam == 2L)
								{
									taskViewConditionData2.m_onClickEventID = enUIEventID.Mall_GotoCouponsTreasureTab;
								}
							}
							else if (conditionType == 27)
							{
								goto_obj.CustomSetActive(true);
								taskViewConditionData2.bShowGoToBtn = true;
								taskViewConditionData2.m_onClickEventID = enUIEventID.Friend_OpenForm;
							}
							else
							{
								goto_obj.CustomSetActive(false);
								taskViewConditionData2.bShowGoToBtn = false;
							}
						}
						else
						{
							goto_obj.CustomSetActive(false);
							taskViewConditionData2.bShowGoToBtn = false;
						}
					}
					this.taskViewConditionDataList.Add(taskViewConditionData2);
				}
			}
			return this.taskViewConditionDataList;
		}
	}
}
