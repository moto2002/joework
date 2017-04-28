using Assets.Scripts.UI;
using ResData;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class CampaignForm : ActivityForm
	{
		public enum DynamicAssets
		{
			ButtonBlueImage,
			ButtonYellowImage
		}

		public class ActivityMenuItem
		{
			public GameObject root;

			public Activity activity;

			public Text name;

			public Image flag;

			public Text flagText;

			public Image hotspot;

			public ActivityMenuItem(GameObject node, Activity actv)
			{
				this.root = node;
				this.activity = actv;
				this.name = Utility.GetComponetInChild<Text>(node, "Name");
				this.flag = Utility.GetComponetInChild<Image>(node, "Flag");
				this.flagText = Utility.GetComponetInChild<Text>(node, "Flag/Text");
				this.hotspot = Utility.GetComponetInChild<Image>(node, "Hotspot");
				this.activity.OnTimeStateChange += new Activity.ActivityEvent(this.OnStateChange);
				this.activity.OnMaskStateChange += new Activity.ActivityEvent(this.OnStateChange);
				this.Validate();
			}

			public void Clear()
			{
				this.activity.OnTimeStateChange -= new Activity.ActivityEvent(this.OnStateChange);
				this.activity.OnMaskStateChange -= new Activity.ActivityEvent(this.OnStateChange);
			}

			public void Validate()
			{
				this.name.text = this.activity.Name;
				RES_WEAL_COLORBAR_TYPE flagType = this.activity.FlagType;
				if (flagType != null)
				{
					this.flag.gameObject.CustomSetActive(true);
					string text = flagType.ToString();
					this.flag.SetSprite(CUIUtility.GetSpritePrefeb("UGUI/Sprite/Dynamic/Activity/" + text, false, false), false);
					this.flagText.text = Singleton<CTextManager>.GetInstance().GetText(text);
				}
				else
				{
					this.flag.gameObject.CustomSetActive(false);
				}
				this.hotspot.gameObject.CustomSetActive(this.activity.ReadyForDot);
			}

			private void OnStateChange(Activity actv)
			{
				DebugHelper.Assert(this.hotspot != null, "hotspot != null");
				if (this.hotspot != null)
				{
					this.hotspot.gameObject.SetActive(this.activity.ReadyForDot);
				}
			}
		}

		private class ActivityTabIndex
		{
			public int idx;

			public Activity m_Activity;
		}

		public static string s_formPath = CUIUtility.s_Form_Activity_Dir + "Form_Activity.prefab";

		private CUIFormScript _uiForm;

		private string[] m_strTitleList = new string[]
		{
			"精彩活动",
			"游戏公告"
		};

		private GameObject[] m_TitleListObj = new GameObject[2];

		private CUIListScript m_TitleMenuList;

		private CUIListScript _uiListMenu;

		private ListView<CampaignForm.ActivityMenuItem> _actvMenuList;

		private int _selectedIndex;

		private int _initTimer;

		private int _initStep;

		private GameObject _title;

		private Text _titleText;

		private Image _titleImage;

		private ScrollRect _viewScroll;

		private CampaignFormView _view;

		private static int G_DISPLAYCOLS = 5;

		private ListView<CampaignForm.ActivityTabIndex>[] m_AllSelectActivityMenuList = new ListView<CampaignForm.ActivityTabIndex>[CampaignForm.G_DISPLAYCOLS];

		private int m_nSelectActivityIndex;

		private int m_nUseActivityTabCount;

		private int[] m_ActivtyTypeToTabIdx = new int[CampaignForm.G_DISPLAYCOLS];

		public override CUIFormScript formScript
		{
			get
			{
				return this._uiForm;
			}
		}

		public CampaignForm(ActivitySys sys) : base(sys)
		{
			this._uiForm = null;
		}

		public Image GetDynamicImage(CampaignForm.DynamicAssets index)
		{
			return this._uiForm.GetWidget((int)index).GetComponent<Image>();
		}

		public GameObject GetIDIPRedObj()
		{
			if (this._uiForm == null)
			{
				return null;
			}
			int num = this.m_TitleListObj.Length;
			if (this.m_TitleListObj != null && this.m_nUseActivityTabCount + 1 == num)
			{
				return this.m_TitleListObj[this.m_nUseActivityTabCount];
			}
			return null;
		}

		private GameObject GetActivityRedObj()
		{
			if (this._uiForm == null)
			{
				return null;
			}
			if (this.m_TitleListObj != null && this.m_TitleListObj.Length >= 2)
			{
				return this.m_TitleListObj[0];
			}
			return null;
		}

		public override void Open()
		{
			this.ClearActiveData();
			if (null != this._uiForm)
			{
				return;
			}
			this._uiForm = Singleton<CUIManager>.GetInstance().OpenForm(CampaignForm.s_formPath, false, true);
			if (null == this._uiForm)
			{
				return;
			}
			this._initTimer = Singleton<CTimerManager>.GetInstance().AddTimer(80, 4, new CTimer.OnTimeUpHandler(this.onInitTimer));
			this._initStep = 0;
			this.PandroaUpdateBtn();
		}

		public void PandroaUpdateBtn()
		{
			if (this._uiForm)
			{
				MonoSingleton<PandroaSys>.GetInstance().ShowActiveActBoxBtn(this._uiForm);
			}
		}

		private void onInitTimer(int seq)
		{
			switch (++this._initStep)
			{
			case 1:
				this.InitSelectActivtyMenuData();
				this._title = Utility.FindChild(this._uiForm.gameObject, "Panel/Title");
				this._titleText = Utility.GetComponetInChild<Text>(this._title, "Text");
				this._titleImage = Utility.GetComponetInChild<Image>(this._title, "Image");
				this._uiListMenu = Utility.GetComponetInChild<CUIListScript>(this._uiForm.gameObject, "Panel/Panle_Activity/Menu/List");
				this._viewScroll = Utility.GetComponetInChild<ScrollRect>(this._uiForm.gameObject, "Panel/Panle_Activity/ScrollRect");
				this._view = new CampaignFormView(Utility.FindChild(this._uiForm.gameObject, "Panel/Panle_Activity/ScrollRect/Content"), this, null);
				break;
			case 2:
				this.m_TitleMenuList = Utility.GetComponetInChild<CUIListScript>(this._uiForm.gameObject, "Panel/TitleMenu/List");
				this.m_strTitleList = new string[this.m_nUseActivityTabCount + 1];
				for (int i = 0; i < this.m_nUseActivityTabCount; i++)
				{
					if (i < 3)
					{
						this.m_strTitleList[i] = Singleton<ActivitySys>.GetInstance().m_ActivtyTabName[i];
					}
					else
					{
						this.m_strTitleList[i] = Singleton<ActivitySys>.GetInstance().m_ActivtyTabName[0];
					}
				}
				this.m_strTitleList[this.m_nUseActivityTabCount] = "游戏公告";
				this.m_TitleMenuList.SetElementAmount(this.m_strTitleList.Length);
				this.m_TitleListObj = new GameObject[this.m_strTitleList.Length];
				for (int j = 0; j < this.m_strTitleList.Length; j++)
				{
					CUIListElementScript elemenet = this.m_TitleMenuList.GetElemenet(j);
					Text componetInChild = Utility.GetComponetInChild<Text>(elemenet.gameObject, "Text");
					if (componetInChild)
					{
						componetInChild.text = this.m_strTitleList[j];
					}
					this.m_TitleListObj[j] = elemenet.gameObject;
				}
				this.m_TitleMenuList.SelectElement(0, true);
				break;
			case 3:
				this.UpdateBuildMenulistTabIdx(0);
				break;
			case 4:
			{
				int num = -1;
				bool flag = true;
				while (++num < this._actvMenuList.get_Count())
				{
					if ((flag && this._actvMenuList.get_Item(num).activity.ReadyForGet) || (!flag && this._actvMenuList.get_Item(num).activity.ReadyForDot))
					{
						break;
					}
					if (flag && num + 1 == this._actvMenuList.get_Count())
					{
						num = -1;
						flag = false;
					}
				}
				if (num >= this._actvMenuList.get_Count())
				{
					num = 0;
				}
				this._uiListMenu.SelectElement(num, true);
				this.SelectMenuItem(num);
				Singleton<ActivitySys>.GetInstance().OnStateChange += new ActivitySys.StateChangeDelegate(this.OnValidateActivityRedSpot);
				Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Activity_Select, new CUIEventManager.OnUIEventHandler(this.OnSelectActivity));
				Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Activity_Select_TitleMenu, new CUIEventManager.OnUIEventHandler(this.OnSelectTitleMenu));
				this.UpdateTitelRedDot();
				Singleton<CTimerManager>.GetInstance().RemoveTimerSafely(ref this._initTimer);
				this._initStep = 0;
				Singleton<ActivitySys>.GetInstance().OnCampaignFormOpened();
				break;
			}
			}
		}

		private void OnValidateActivityRedSpot()
		{
			this.UpdateTitelRedDot();
		}

		private void UpdateTitelRedDot()
		{
			if (this._uiForm == null || this.m_TitleMenuList == null)
			{
				return;
			}
			for (int i = 0; i < this.m_nUseActivityTabCount; i++)
			{
				CUIListElementScript elemenet = this.m_TitleMenuList.GetElemenet(i);
				if (elemenet != null)
				{
					int num = 0;
					for (int j = 0; j < this.m_AllSelectActivityMenuList[i].get_Count(); j++)
					{
						Activity activity = this.m_AllSelectActivityMenuList[i].get_Item(j).m_Activity;
						if (activity != null && activity.ReadyForGet)
						{
							num++;
						}
					}
					if (num > 0)
					{
						CUICommonSystem.AddRedDot(elemenet.gameObject, enRedDotPos.enTopRight, num);
					}
					else
					{
						CUICommonSystem.DelRedDot(elemenet.gameObject);
					}
				}
			}
		}

		private void OnSelectTitleMenu(CUIEvent uiEvent)
		{
			if (this._uiForm == null)
			{
				return;
			}
			if (uiEvent.m_srcWidgetIndexInBelongedList < this.m_nUseActivityTabCount)
			{
				this.UpdateBuildMenulistTabIdx(uiEvent.m_srcWidgetIndexInBelongedList);
				Transform transform = this._uiForm.gameObject.transform.Find("Panel/Panle_Activity");
				if (transform)
				{
					transform.gameObject.CustomSetActive(true);
				}
				Transform transform2 = this._uiForm.gameObject.transform.Find("Panel/Panle_IDIP");
				if (transform2)
				{
					transform2.gameObject.CustomSetActive(false);
				}
			}
			else if (uiEvent.m_srcWidgetIndexInBelongedList == this.m_nUseActivityTabCount)
			{
				Transform transform3 = this._uiForm.gameObject.transform.Find("Panel/Panle_Activity");
				if (transform3)
				{
					transform3.gameObject.CustomSetActive(false);
				}
				Transform transform4 = this._uiForm.gameObject.transform.Find("Panel/Panle_IDIP");
				if (transform4)
				{
					transform4.gameObject.CustomSetActive(true);
				}
				MonoSingleton<IDIPSys>.GetInstance().OnOpenIDIPForm(this._uiForm);
			}
		}

		private void ClearActiveData()
		{
			for (int i = 0; i < this.m_AllSelectActivityMenuList.Length; i++)
			{
				this.m_AllSelectActivityMenuList[i] = new ListView<CampaignForm.ActivityTabIndex>();
			}
			this.m_nUseActivityTabCount = 0;
			this.m_nSelectActivityIndex = 0;
		}

		private void InitSelectActivtyMenuData()
		{
			for (int i = 0; i < this.m_AllSelectActivityMenuList.Length; i++)
			{
				this.m_AllSelectActivityMenuList[i] = new ListView<CampaignForm.ActivityTabIndex>();
			}
			this.m_nUseActivityTabCount = 0;
			this.m_nSelectActivityIndex = 0;
			ListView<Activity> activityList = base.Sys.GetActivityList((Activity actv) => actv.Entrance == 1);
			activityList.Sort(delegate(Activity l, Activity r)
			{
				bool readyForGet = l.ReadyForGet;
				bool readyForGet2 = r.ReadyForGet;
				if (readyForGet != readyForGet2)
				{
					return (!readyForGet) ? 1 : -1;
				}
				bool completed = l.Completed;
				bool completed2 = r.Completed;
				if (completed != completed2)
				{
					return (!completed) ? -1 : 1;
				}
				if (l.FlagType != r.FlagType)
				{
					return r.FlagType - l.FlagType;
				}
				return (int)(l.Sequence - r.Sequence);
			});
			for (int j = 0; j < activityList.get_Count(); j++)
			{
				Activity activity = activityList.get_Item(j);
				int num = activity.GetTabID();
				if (num < 0 || num >= CampaignForm.G_DISPLAYCOLS)
				{
					num = 0;
				}
				CampaignForm.ActivityTabIndex activityTabIndex = new CampaignForm.ActivityTabIndex();
				activityTabIndex.idx = num;
				activityTabIndex.m_Activity = activity;
				this.m_AllSelectActivityMenuList[num].Add(activityTabIndex);
			}
			int num2 = 0;
			for (int k = 0; k < this.m_AllSelectActivityMenuList.Length; k++)
			{
				if (this.m_AllSelectActivityMenuList[k].get_Count() > 0)
				{
					this.m_nUseActivityTabCount++;
					this.m_ActivtyTypeToTabIdx[num2] = this.m_AllSelectActivityMenuList[k].get_Item(0).idx;
					num2++;
				}
			}
		}

		private void UpdateBuildMenulistTabIdx(int idx)
		{
			if (this._actvMenuList != null)
			{
				for (int i = 0; i < this._actvMenuList.get_Count(); i++)
				{
					this._actvMenuList.get_Item(i).Clear();
				}
				this._actvMenuList = null;
			}
			this._actvMenuList = new ListView<CampaignForm.ActivityMenuItem>();
			this._selectedIndex = -1;
			if (idx >= this.m_ActivtyTypeToTabIdx.Length)
			{
				return;
			}
			this.m_nSelectActivityIndex = this.m_ActivtyTypeToTabIdx[idx];
			if (this.m_nSelectActivityIndex >= this.m_AllSelectActivityMenuList.Length)
			{
				return;
			}
			this._uiListMenu.SetElementAmount(this.m_AllSelectActivityMenuList[this.m_nSelectActivityIndex].get_Count());
			for (int j = 0; j < this.m_AllSelectActivityMenuList[this.m_nSelectActivityIndex].get_Count(); j++)
			{
				Activity activity = this.m_AllSelectActivityMenuList[this.m_nSelectActivityIndex].get_Item(j).m_Activity;
				CUIListElementScript elemenet = this._uiListMenu.GetElemenet(j);
				CampaignForm.ActivityMenuItem activityMenuItem = new CampaignForm.ActivityMenuItem(elemenet.gameObject, activity);
				this._actvMenuList.Add(activityMenuItem);
			}
			this.SelectMenuItem(0);
			this._uiListMenu.SelectElement(0, true);
		}

		public override void Update()
		{
			if (this._view != null && this._uiForm && this._uiForm.gameObject && this._initTimer == 0)
			{
				this._view.Update();
			}
		}

		private void SelectMenuItem(int index)
		{
			if (index < 0 || index >= this._actvMenuList.get_Count())
			{
				this._titleImage.gameObject.CustomSetActive(false);
				this._titleText.gameObject.CustomSetActive(true);
				this._titleText.text = Singleton<CTextManager>.GetInstance().GetText("activityEmptyTitle");
				this._view.SetActivity(null);
				return;
			}
			if (index != this._selectedIndex)
			{
				this._selectedIndex = index;
				CampaignForm.ActivityMenuItem activityMenuItem = this._actvMenuList.get_Item(this._selectedIndex);
				string title = activityMenuItem.activity.Title;
				if (string.IsNullOrEmpty(title))
				{
					this._title.CustomSetActive(false);
				}
				else
				{
					this._title.CustomSetActive(true);
					if (activityMenuItem.activity.IsImageTitle)
					{
						this._titleText.gameObject.CustomSetActive(false);
						this._titleImage.gameObject.CustomSetActive(true);
						this._titleImage.SetSprite(CUIUtility.GetSpritePrefeb(ActivitySys.SpriteRootDir + title, false, false), false);
						this._titleImage.SetNativeSize();
					}
					else
					{
						this._titleImage.gameObject.CustomSetActive(false);
						this._titleText.gameObject.CustomSetActive(true);
						this._titleText.text = title;
					}
				}
				this._view.SetActivity(activityMenuItem.activity);
				this._viewScroll.verticalNormalizedPosition = 1f;
				this.Update();
				activityMenuItem.activity.Visited = true;
			}
		}

		private void OnSelectActivity(CUIEvent uiEvent)
		{
			this.SelectMenuItem(uiEvent.m_srcWidgetIndexInBelongedList);
			CUICommonSystem.CloseUseableTips();
		}

		public override void Close()
		{
			if (this._actvMenuList != null)
			{
				for (int i = 0; i < this._actvMenuList.get_Count(); i++)
				{
					this._actvMenuList.get_Item(i).Clear();
				}
				this._actvMenuList = null;
			}
			if (this._view != null)
			{
				this._view.Clear();
				this._view = null;
			}
			if (null != this._uiForm)
			{
				Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Activity_Select, new CUIEventManager.OnUIEventHandler(this.OnSelectActivity));
				Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Activity_Select_TitleMenu, new CUIEventManager.OnUIEventHandler(this.OnSelectTitleMenu));
				CUIFormScript uiForm = this._uiForm;
				this._uiForm = null;
				this._uiListMenu = null;
				this.m_TitleMenuList = null;
				this.m_TitleListObj = null;
				this.ClearActiveData();
				Singleton<CUIManager>.GetInstance().CloseForm(uiForm);
				MonoSingleton<NobeSys>.GetInstance().ShowDelayNobeLoseTipsInfo();
				MonoSingleton<PandroaSys>.GetInstance().ShowPopNews();
			}
			MonoSingleton<IDIPSys>.GetInstance().OnCloseIDIPForm(null);
			Singleton<ActivitySys>.GetInstance().OnStateChange -= new ActivitySys.StateChangeDelegate(this.OnValidateActivityRedSpot);
			Singleton<CTimerManager>.GetInstance().RemoveTimerSafely(ref this._initTimer);
		}
	}
}
