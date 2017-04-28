using Assets.Scripts.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class BannerWidget : ActivityWidget
	{
		public const float SCROLL_TIME_SPAN = 3.5f;

		private CUIStepListScript _stepList;

		private int _curStepIndex;

		private bool _leftToRight;

		private float _lastScrollTime;

		private ListView<UrlAction> _urlaList;

		private Text _timeCD;

		public BannerWidget(GameObject node, ActivityView view) : base(node, view)
		{
			this._stepList = Utility.GetComponetInChild<CUIStepListScript>(node, "StepList");
			this._stepList.SetDontUpdate(true);
			this._timeCD = Utility.GetComponetInChild<Text>(node, "TimeCD");
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Activity_BannerClick, new CUIEventManager.OnUIEventHandler(this.OnClick));
			this.Validate();
		}

		public override void Clear()
		{
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Activity_BannerClick, new CUIEventManager.OnUIEventHandler(this.OnClick));
		}

		public override void Update()
		{
			this.updateAutoScroll();
			this.updateOverTime();
		}

		public override void Validate()
		{
			this._urlaList = UrlAction.ParseFromText(base.view.activity.Content, null);
			if (this._urlaList.get_Count() > 0)
			{
				this._stepList.SetElementAmount(this._urlaList.get_Count());
				for (int i = 0; i < this._urlaList.get_Count(); i++)
				{
					UrlAction urlAction = this._urlaList.get_Item(i);
					CUIListElementScript elemenet = this._stepList.GetElemenet(i);
					if (null != elemenet)
					{
						CUIHttpImageScript component = elemenet.GetComponent<CUIHttpImageScript>();
						if (null != component)
						{
							component.SetImageUrl(urlAction.target);
						}
					}
				}
				this._curStepIndex = 0;
				this._leftToRight = true;
				this._stepList.MoveElementInScrollArea(this._curStepIndex, true);
				this._lastScrollTime = Time.time;
				this.updateOverTime();
			}
		}

		private void OnClick(CUIEvent uiEvent)
		{
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			if (this._urlaList != null && srcWidgetIndexInBelongedList < this._urlaList.get_Count())
			{
				UrlAction urlAction = this._urlaList.get_Item(srcWidgetIndexInBelongedList);
				if (urlAction.Execute())
				{
					base.view.form.Close();
				}
			}
		}

		private void updateAutoScroll()
		{
			int count = this._urlaList.get_Count();
			if (count > 1 && this._curStepIndex < count && Time.time > this._lastScrollTime + (float)this._urlaList.get_Item(this._curStepIndex).showTime * 0.001f)
			{
				this._lastScrollTime = Time.time;
				if ((this._curStepIndex + 1 == count && this._leftToRight) || (this._curStepIndex == 0 && !this._leftToRight))
				{
					this._leftToRight = !this._leftToRight;
				}
				this._curStepIndex += ((!this._leftToRight) ? -1 : 1);
				this._stepList.MoveElementInScrollArea(this._curStepIndex, false);
			}
		}

		private void updateOverTime()
		{
			UrlAction urlAction = this._urlaList.get_Item(this._curStepIndex);
			if (urlAction.overTime > 0uL)
			{
				DateTime dateTime = Utility.ULongToDateTime(urlAction.overTime);
				DateTime dateTime2 = Utility.ToUtcTime2Local((long)CRoleInfo.GetCurrentUTCTime());
				TimeSpan timeSpan = dateTime - dateTime2;
				string text = Singleton<CTextManager>.GetInstance().GetText("TIME_SPAN_FORMAT");
				text = text.Replace("{0}", timeSpan.get_Days().ToString());
				text = text.Replace("{1}", timeSpan.get_Hours().ToString());
				text = text.Replace("{2}", timeSpan.get_Minutes().ToString());
				text = text.Replace("{3}", timeSpan.get_Seconds().ToString());
				this._timeCD.gameObject.CustomSetActive(dateTime >= dateTime2);
				this._timeCD.text = text;
			}
			else
			{
				this._timeCD.gameObject.CustomSetActive(false);
			}
		}
	}
}
