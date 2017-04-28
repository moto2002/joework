using Assets.Scripts.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class CheckInWidget : ActivityWidget
	{
		private Text _awardTitle;

		private Text _awardDesc;

		private Text _progLabel;

		private Text _timeRemain;

		private GameObject _getBtn;

		private Text _getBtnText;

		private uint _remainSeconds;

		public CheckInWidget(GameObject node, ActivityView view) : base(node, view)
		{
			this._awardTitle = Utility.GetComponetInChild<Text>(node, "Content/AwardTitle");
			this._awardDesc = Utility.GetComponetInChild<Text>(node, "Content/AwardDesc");
			this._timeRemain = Utility.GetComponetInChild<Text>(node, "Content/TimeRemain");
			this._progLabel = Utility.GetComponetInChild<Text>(node, "Content/Progress");
			this._getBtn = Utility.FindChild(node, "GetAward");
			this._getBtnText = Utility.GetComponetInChild<Text>(this._getBtn, "Text");
			this.Validate();
			view.activity.OnMaskStateChange += new Activity.ActivityEvent(this.OnStateChanged);
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Wealfare_CheckInGet, new CUIEventManager.OnUIEventHandler(this.OnClickGet));
		}

		public override void Clear()
		{
			base.view.activity.OnMaskStateChange -= new Activity.ActivityEvent(this.OnStateChanged);
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Wealfare_CheckInGet, new CUIEventManager.OnUIEventHandler(this.OnClickGet));
		}

		public override void Validate()
		{
			ActivityPhase curPhase = base.view.activity.CurPhase;
			if (curPhase == null)
			{
				return;
			}
			if (curPhase.RewardDesc.get_Length() > 0)
			{
				this._awardDesc.text = curPhase.RewardDesc;
			}
			else
			{
				CUseable useable = curPhase.GetUseable(0);
				if (useable != null)
				{
					this._awardDesc.text = useable.m_name + ":" + curPhase.GetDropCount(0);
				}
			}
			if (curPhase.ReadyForGet)
			{
				this._getBtn.GetComponent<CUIEventScript>().enabled = true;
				this._getBtn.GetComponent<Button>().interactable = true;
				this._getBtnText.color = Color.white;
				this._awardTitle.text = Singleton<CTextManager>.GetInstance().GetText("awardToday");
				this._getBtnText.text = Singleton<CTextManager>.GetInstance().GetText("get");
				this._remainSeconds = 0u;
				this._timeRemain.text = Singleton<CTextManager>.GetInstance().GetText("timeCountDown").Replace("{0}", Utility.SecondsToTimeText(0u));
			}
			else
			{
				this._getBtn.GetComponent<CUIEventScript>().enabled = false;
				this._getBtn.GetComponent<Button>().interactable = false;
				this._getBtnText.color = Color.gray;
				if (base.view.activity.Completed)
				{
					this._awardTitle.text = Singleton<CTextManager>.GetInstance().GetText("awardToday");
					this._getBtnText.text = Singleton<CTextManager>.GetInstance().GetText("finished");
					this._remainSeconds = 0u;
					this._timeRemain.text = Singleton<CTextManager>.GetInstance().GetText("congraduFinish");
				}
				else
				{
					this._awardTitle.text = Singleton<CTextManager>.GetInstance().GetText("awardTomorrow");
					this._getBtnText.text = Singleton<CTextManager>.GetInstance().GetText("notInTime");
					DateTime dateTime = Utility.ToUtcTime2Local((long)CRoleInfo.GetCurrentUTCTime());
					DateTime dateTime2 = dateTime.AddDays(1.0);
					dateTime2 = new DateTime(dateTime2.get_Year(), dateTime2.get_Month(), dateTime2.get_Day(), 0, 0, 0);
					this._remainSeconds = (uint)(dateTime2 - dateTime).get_TotalSeconds();
					this._timeRemain.text = Singleton<CTextManager>.GetInstance().GetText("timeCountDown").Replace("{0}", Utility.SecondsToTimeText(this._remainSeconds));
				}
			}
			Text arg_2CD_0 = this._progLabel;
			string arg_2A5_0 = Singleton<CTextManager>.GetInstance().GetText("CheckInProgress");
			string arg_2A5_1 = "{0}";
			int current = base.view.activity.Current;
			arg_2CD_0.text = arg_2A5_0.Replace(arg_2A5_1, current.ToString()).Replace("{1}", base.view.activity.Target.ToString());
		}

		public override void Update()
		{
			if (this._remainSeconds > 0u)
			{
				this._remainSeconds -= 1u;
				this._timeRemain.text = Singleton<CTextManager>.GetInstance().GetText("timeCountDown").Replace("{0}", Utility.SecondsToTimeText(this._remainSeconds));
			}
		}

		private void OnClickGet(CUIEvent uiEvent)
		{
			ActivityPhase curPhase = base.view.activity.CurPhase;
			if (curPhase != null)
			{
				curPhase.DrawReward();
			}
		}

		private void OnStateChanged(Activity actv)
		{
			this.Validate();
		}
	}
}
