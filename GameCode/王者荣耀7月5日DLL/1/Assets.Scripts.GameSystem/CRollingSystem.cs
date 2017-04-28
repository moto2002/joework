using Assets.Scripts.GameLogic;
using Assets.Scripts.UI;
using CSProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class CRollingSystem : Singleton<CRollingSystem>
	{
		private const int PriorityMin = 1;

		private const int PriorityMax = 1000;

		private const int PriorityIDIPRatio = 100;

		private const int PriorityHornRatio = 10;

		private const int PriorityEventRatio = 1;

		private const int ResetRollingInfoPriorityPeriod = 300;

		private const int RollingCDMilliSeconds = 20000;

		private List<RollingInfo> m_rollingInfos = new List<RollingInfo>();

		private bool m_isInRollingCD;

		public override void Init()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.UIComponent_AutoScroller_Scroll_Finish, new CUIEventManager.OnUIEventHandler(this.OnAutoScrollerScrollFinish));
		}

		private void OnAutoScrollerScrollFinish(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form != null && uiEvent.m_srcWidget == form.GetWidget(4))
			{
				this.HandleOnShowRollingInfo();
				this.ResetRollingInfoPriority();
				this.m_isInRollingCD = true;
				Singleton<CTimerManager>.GetInstance().AddTimer(20000, 1, new CTimer.OnTimeUpHandler(this.OnRollingCDOver));
			}
		}

		private void OnRollingCDOver(int timerSequence)
		{
			this.m_isInRollingCD = false;
			if (Singleton<LobbyLogic>.GetInstance().CanShowRolling)
			{
				this.StartRolling();
			}
		}

		private void ResetRollingInfoPriority()
		{
			for (int i = 0; i < this.m_rollingInfos.get_Count(); i++)
			{
				if (this.IsNeedResetResetPriority(this.m_rollingInfos.get_Item(i)))
				{
					this.m_rollingInfos.get_Item(i).priority = 1000;
					this.m_rollingInfos.get_Item(i).resetPriorityTime = CRoleInfo.GetCurrentUTCTime();
				}
			}
		}

		private bool IsNeedResetResetPriority(RollingInfo rollingInfo)
		{
			return rollingInfo.repeatCount > 0 && CRoleInfo.GetCurrentUTCTime() - rollingInfo.resetPriorityTime > 300;
		}

		private void HandleOnShowRollingInfo()
		{
			int onShowRollingInfoIndex = this.GetOnShowRollingInfoIndex();
			if (0 <= onShowRollingInfoIndex && onShowRollingInfoIndex < this.m_rollingInfos.get_Count())
			{
				if (this.m_rollingInfos.get_Item(onShowRollingInfoIndex).repeatCount > 1)
				{
					RollingInfo expr_42 = this.m_rollingInfos.get_Item(onShowRollingInfoIndex);
					expr_42.repeatCount -= 1;
					this.m_rollingInfos.get_Item(onShowRollingInfoIndex).priority = 1;
					this.m_rollingInfos.get_Item(onShowRollingInfoIndex).isShowing = false;
				}
				else
				{
					this.m_rollingInfos.RemoveAt(onShowRollingInfoIndex);
				}
			}
		}

		public void StartRolling()
		{
			if (this.m_isInRollingCD)
			{
				return;
			}
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			CUIAutoScroller component = form.GetWidget(4).GetComponent<CUIAutoScroller>();
			this.StartRolling(component);
		}

		public void StopRolling()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			CUIAutoScroller component = form.GetWidget(4).GetComponent<CUIAutoScroller>();
			component.StopAutoScroll();
		}

		private void StartRolling(CUIAutoScroller autoScroller)
		{
			if (autoScroller == null || autoScroller.IsScrollRunning() || !Singleton<LobbyLogic>.GetInstance().CanShowRolling || this.m_isInRollingCD)
			{
				return;
			}
			RectTransform rectTransform = autoScroller.transform as RectTransform;
			if (rectTransform != null)
			{
				if (Singleton<CLoudSpeakerSys>.get_instance().IsLoudSpeakerShowing())
				{
					rectTransform.anchorMin = new Vector2(0.661f, 0.5f);
				}
				else
				{
					rectTransform.anchorMin = new Vector2(0.3f, 0.5f);
				}
			}
			int nextShowRollingInfoIndex = this.GetNextShowRollingInfoIndex();
			if (0 <= nextShowRollingInfoIndex && nextShowRollingInfoIndex < this.m_rollingInfos.get_Count())
			{
				autoScroller.SetText(this.m_rollingInfos.get_Item(nextShowRollingInfoIndex).content);
				autoScroller.StartAutoScroll(false);
				this.m_rollingInfos.get_Item(nextShowRollingInfoIndex).isShowing = true;
			}
		}

		private static int CalculatePriority(COM_ROLLINGMSG_TYPE type, byte innerPriority)
		{
			int result = 1;
			if (type == null)
			{
				result = (int)(100 + innerPriority);
			}
			else if (type == 1)
			{
				result = (int)(10 + innerPriority);
			}
			else if (type == 2)
			{
				result = (int)(1 + innerPriority);
			}
			else
			{
				DebugHelper.Assert(false, "CalculatePriority.CalculatePriority(): invalid rolling msg type {0}", new object[]
				{
					type
				});
			}
			return result;
		}

		private static byte CalculateRepeatCount(uint startTime, uint endTime, ushort repeatPeriod)
		{
			byte result = 0;
			if (endTime > startTime && repeatPeriod != 0)
			{
				result = (byte)((endTime - startTime) / (uint)repeatPeriod);
			}
			return result;
		}

		private int GetOnShowRollingInfoIndex()
		{
			for (int i = 0; i < this.m_rollingInfos.get_Count(); i++)
			{
				if (this.m_rollingInfos.get_Item(i).isShowing)
				{
					return i;
				}
			}
			return -1;
		}

		private int GetNextShowRollingInfoIndex()
		{
			if (this.m_rollingInfos.get_Count() == 0)
			{
				return -1;
			}
			int num = 0;
			for (int i = 0; i < this.m_rollingInfos.get_Count(); i++)
			{
				if (this.m_rollingInfos.get_Item(i).priority > num)
				{
					num = i;
				}
			}
			return num;
		}

		[MessageHandler(1450)]
		public static void ReceiveRollingMsgNtf(CSPkg msg)
		{
			SCPKG_ROLLINGMSG_NTF stRollingMsgNtf = msg.stPkgData.get_stRollingMsgNtf();
			int num = Mathf.Min(stRollingMsgNtf.astRollingMsg.Length, (int)stRollingMsgNtf.bMsgCnt);
			for (int i = 0; i < num; i++)
			{
				RollingInfo rollingInfo = new RollingInfo();
				CSDT_ROLLING_MSG cSDT_ROLLING_MSG = stRollingMsgNtf.astRollingMsg[i];
				rollingInfo.resetPriorityTime = CRoleInfo.GetCurrentUTCTime();
				rollingInfo.priority = CRollingSystem.CalculatePriority(cSDT_ROLLING_MSG.bType, cSDT_ROLLING_MSG.bPriority);
				rollingInfo.repeatCount = CRollingSystem.CalculateRepeatCount(cSDT_ROLLING_MSG.dwStartTime, cSDT_ROLLING_MSG.dwEndTime, cSDT_ROLLING_MSG.wPeriod);
				rollingInfo.content = Utility.UTF8Convert(cSDT_ROLLING_MSG.szContent, (int)cSDT_ROLLING_MSG.wContentLen);
				if (cSDT_ROLLING_MSG.bType == 0)
				{
					rollingInfo.content = "<color=#b5e9ff>" + rollingInfo.content + "</color>";
				}
				if (cSDT_ROLLING_MSG.bIsChat == 0)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<string>(EventID.ROLLING_SYSTEM_CHAT_INFO_RECEIVED, rollingInfo.content);
				}
				Singleton<CRollingSystem>.GetInstance().m_rollingInfos.Add(rollingInfo);
			}
			Singleton<CRollingSystem>.GetInstance().StartRolling();
		}
	}
}
