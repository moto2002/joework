using Assets.Scripts.Framework;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class CEquipSystem : Singleton<CEquipSystem>
	{
		private struct stEquipRankInfo
		{
			public int equipRankItemCnt;

			public CSDT_RANKING_LIST_ITEM_INFO[] rankDetail;
		}

		public const int c_recommendEquipMaxCount = 6;

		public const int c_equipLevelMaxCount = 3;

		private const int c_equipJudgeMaxCount = 12;

		private const int c_judgeMarksCount = 5;

		private const int c_maxEquipCntPerLevel = 12;

		public const int c_2ndEquipMaxCount = 3;

		public const int c_3rdEquipMaxCountPer2ndEquip = 2;

		public const int c_backEquipMaxCount = 20;

		private static int s_equipUsageAmount = Enum.GetValues(typeof(enEquipUsage)).get_Length();

		private List<ushort>[][] m_equipList;

		private static Dictionary<ushort, CEquipInfo> s_equipInfoDictionary;

		private Dictionary<long, ResRecommendEquipInBattle> m_defaultRecommendEquipsDictionary;

		private stEquipTree m_equipTree;

		private uint m_backEquipCount;

		private ushort[] m_backEquipIDs = new ushort[20];

		private uint m_editHeroID;

		private uint m_defaultCombinationID = 1u;

		private ushort[] m_editCustomRecommendEquips = new ushort[6];

		private CEquipRelationPath m_equipRelationPath;

		public static string s_CustomRecommendEquipPath = "UGUI/Form/System/CustomRecommendEquip/Form_CustomRecommendEquip.prefab";

		private static string s_EquipListNodePrefabPath = "UGUI/Form/System/CustomRecommendEquip/Panel_EquipList.prefab";

		private static string s_EquipTreePath = "UGUI/Form/System/CustomRecommendEquip/Form_EquipTree.prefab";

		private static string s_ChooseHeroPath = "UGUI/Form/System/CustomRecommendEquip/Form_ChooseHero.prefab";

		private static string s_GodEquipPath = "UGUI/Form/System/CustomRecommendEquip/Form_GodEquip.prefab";

		private static string s_EquipJudgePath = "UGUI/Form/System/CustomRecommendEquip/Form_EquipJudge.prefab";

		private float m_uiEquipItemHeight;

		private float m_uiEquipItemContentDefaultHeight;

		private float m_uiCustomEquipContentHeight;

		private float m_uiEquipItemContentHightDiff = 70f;

		private CUIFormScript m_customEquipForm;

		private CUIFormScript m_equipJudgeForm;

		private enEquipUsage m_curEquipUsage = enEquipUsage.PhyAttack;

		private CEquipInfo m_selEquipInfo;

		private Transform m_selEquipItemObj;

		private bool bEditEquip;

		private ListView<ResHeroCfgInfo> m_heroList = new ListView<ResHeroCfgInfo>();

		private enHeroJobType m_curHeroJob;

		private uint m_reqRankHeroId;

		private Dictionary<uint, CEquipSystem.stEquipRankInfo> m_equipRankItemDetail = new Dictionary<uint, CEquipSystem.stEquipRankInfo>();

		private float c_moveAnimaTime = 0.1f;

		private bool m_useGodEquip;

		private bool m_revertDefaultEquip;

		private bool m_bOwnHeroFlag = true;

		private Transform m_selectedEquipItemInEquipTree;

		private CEquipInfo m_selectedEquipInfoInEquipTree;

		private enGodEquipTab m_godEquipCurTab;

		private static int m_judgeGodeIndex = -1;

		public override void Init()
		{
			this.m_equipList = new List<ushort>[CEquipSystem.s_equipUsageAmount][];
			for (int i = 0; i < CEquipSystem.s_equipUsageAmount; i++)
			{
				this.m_equipList[i] = new List<ushort>[3];
				for (int j = 0; j < 3; j++)
				{
					this.m_equipList[i][j] = new List<ushort>();
				}
			}
			this.m_equipTree = new stEquipTree(3, 2, 20);
			this.m_equipRelationPath = new CEquipRelationPath();
			if (CEquipSystem.s_equipInfoDictionary == null)
			{
				CEquipSystem.s_equipInfoDictionary = new Dictionary<ushort, CEquipInfo>();
			}
			this.m_defaultRecommendEquipsDictionary = new Dictionary<long, ResRecommendEquipInBattle>();
			GameDataMgr.m_equipInBattleDatabin.Accept(new Action<ResEquipInBattle>(this.EquipInBattleInVisitor));
			this.InitializeBackEquipListForEachEquip();
			GameDataMgr.m_recommendEquipInBattleDatabin.Accept(new Action<ResRecommendEquipInBattle>(this.DefaultRecommendEquipsInVisitor));
			this.InitUIEventListener();
		}

		private void InitSysGodEquipTab()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CEquipSystem.s_GodEquipPath);
			if (form == null)
			{
				return;
			}
			GameObject widget = form.GetWidget(0);
			string[] array = new string[]
			{
				Singleton<CTextManager>.GetInstance().GetText("CustomEquip_Tab_RecommendSystem"),
				Singleton<CTextManager>.GetInstance().GetText("CustomEquip_Tab_RecommendGod")
			};
			CUIListScript component = widget.GetComponent<CUIListScript>();
			component.SetElementAmount(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				Text component2 = component.GetElemenet(i).transform.Find("Text").GetComponent<Text>();
				component2.text = array[i];
			}
			component.SelectElement(0, true);
		}

		public override void UnInit()
		{
			this.UinitUIEventListener();
		}

		public static CEquipInfo GetEquipInfo(ushort equipID)
		{
			CEquipInfo result = null;
			if (CEquipSystem.s_equipInfoDictionary.TryGetValue(equipID, ref result))
			{
				return result;
			}
			return null;
		}

		public Dictionary<ushort, CEquipInfo> GetEquipInfoDictionary()
		{
			return CEquipSystem.s_equipInfoDictionary;
		}

		public List<ushort>[][] GetEquipList()
		{
			return this.m_equipList;
		}

		private void EquipInBattleInVisitor(ResEquipInBattle resEquipInBattle)
		{
			if (CEquipSystem.s_equipInfoDictionary.ContainsKey(resEquipInBattle.wID))
			{
				CEquipSystem.s_equipInfoDictionary.Remove(resEquipInBattle.wID);
			}
			CEquipSystem.s_equipInfoDictionary.Add(resEquipInBattle.wID, new CEquipInfo(resEquipInBattle.wID));
			if (resEquipInBattle.bUsage >= 0 && (int)resEquipInBattle.bUsage < CEquipSystem.s_equipUsageAmount && resEquipInBattle.bLevel >= 1 && resEquipInBattle.bLevel <= 3 && resEquipInBattle.bInvalid == 0)
			{
				this.m_equipList[(int)resEquipInBattle.bUsage][(int)(resEquipInBattle.bLevel - 1)].Add(resEquipInBattle.wID);
			}
		}

		private void InitializeBackEquipListForEachEquip()
		{
			Dictionary<ushort, CEquipInfo>.Enumerator enumerator = CEquipSystem.s_equipInfoDictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<ushort, CEquipInfo> current = enumerator.get_Current();
				CEquipInfo value = current.get_Value();
				if (value.m_resEquipInBattle != null && value.m_resEquipInBattle.bInvalid == 0)
				{
					for (int i = 0; i < value.m_resEquipInBattle.PreEquipID.Length; i++)
					{
						if (value.m_resEquipInBattle.PreEquipID[i] > 0)
						{
							CEquipInfo cEquipInfo = null;
							if (CEquipSystem.s_equipInfoDictionary.TryGetValue(value.m_resEquipInBattle.PreEquipID[i], ref cEquipInfo) && cEquipInfo.m_resEquipInBattle.bInvalid == 0)
							{
								cEquipInfo.AddBackEquipID(value.m_equipID);
							}
						}
					}
				}
			}
		}

		private void DefaultRecommendEquipsInVisitor(ResRecommendEquipInBattle resRecommendEquipInBattle)
		{
			long doubleKey = GameDataMgr.GetDoubleKey((uint)resRecommendEquipInBattle.wHeroID, (uint)resRecommendEquipInBattle.wCombinationID);
			if (this.m_defaultRecommendEquipsDictionary.ContainsKey(doubleKey))
			{
				this.m_defaultRecommendEquipsDictionary.Remove(doubleKey);
			}
			if (resRecommendEquipInBattle.RecommendEquipID.Length == 6)
			{
				ResRecommendEquipInBattle resRecommendEquipInBattle2 = null;
				for (int i = 0; i < 6; i++)
				{
					CEquipInfo cEquipInfo = null;
					if (resRecommendEquipInBattle.RecommendEquipID[i] == 0 || (CEquipSystem.s_equipInfoDictionary.TryGetValue(resRecommendEquipInBattle.RecommendEquipID[i], ref cEquipInfo) && cEquipInfo.m_resEquipInBattle.bInvalid <= 0))
					{
						resRecommendEquipInBattle2 = resRecommendEquipInBattle;
					}
					else
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Gao Mao a! tuijian zhuangbei limian tian le ge bu ke yong de zhuagnbei!!! HeroID = ",
							resRecommendEquipInBattle.wHeroID,
							", CombineID = ",
							resRecommendEquipInBattle.wCombinationID,
							", equipID = ",
							resRecommendEquipInBattle.RecommendEquipID[i]
						}));
					}
				}
				this.m_defaultRecommendEquipsDictionary.Add(doubleKey, resRecommendEquipInBattle2);
			}
			else
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Gao Mao a! tuijian zhuangbei de shuliang dou meiyou tian dui!!! HeroID = ",
					resRecommendEquipInBattle.wHeroID,
					", CombineID = ",
					resRecommendEquipInBattle.wCombinationID
				}));
			}
		}

		private void ClearEquipList()
		{
			for (int i = 0; i < CEquipSystem.s_equipUsageAmount; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					this.m_equipList[i][j].Clear();
				}
			}
		}

		private void GetEquipTree(ushort equipID, ref stEquipTree equipTree)
		{
			equipTree.Create(equipID, CEquipSystem.s_equipInfoDictionary);
		}

		private void InitializeEditCustomRecommendEquips(uint heroID, ref bool useCustomRecommendEquips)
		{
			for (int i = 0; i < 6; i++)
			{
				this.m_editCustomRecommendEquips[i] = 0;
			}
			this.m_editHeroID = heroID;
			if (heroID == 0u)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null && masterRoleInfo.m_customRecommendEquipDictionary != null)
			{
				ushort[] array = null;
				useCustomRecommendEquips = false;
				if (masterRoleInfo.m_customRecommendEquipDictionary.TryGetValue(heroID, ref array))
				{
					useCustomRecommendEquips = true;
				}
				else
				{
					useCustomRecommendEquips = false;
					ResRecommendEquipInBattle defaultRecommendEquipInfo = this.GetDefaultRecommendEquipInfo(heroID, 1u);
					if (defaultRecommendEquipInfo != null)
					{
						array = defaultRecommendEquipInfo.RecommendEquipID;
					}
				}
				if (array == null)
				{
					return;
				}
				for (int j = 0; j < 6; j++)
				{
					if (array[j] == 0)
					{
						this.m_editCustomRecommendEquips[j] = 0;
					}
					else
					{
						CEquipInfo cEquipInfo = null;
						if (CEquipSystem.s_equipInfoDictionary.TryGetValue(array[j], ref cEquipInfo) && cEquipInfo.m_resEquipInBattle.bInvalid <= 0)
						{
							this.m_editCustomRecommendEquips[j] = array[j];
						}
						else
						{
							this.m_editCustomRecommendEquips[j] = 0;
							if (useCustomRecommendEquips)
							{
							}
						}
					}
				}
			}
		}

		public ResRecommendEquipInBattle GetDefaultRecommendEquipInfo(uint heroID, uint combinationID)
		{
			ResRecommendEquipInBattle result = null;
			long doubleKey = GameDataMgr.GetDoubleKey(heroID, combinationID);
			if (this.m_defaultRecommendEquipsDictionary.TryGetValue(doubleKey, ref result))
			{
				return result;
			}
			return null;
		}

		private void RevertEditCustomRecommendEquipToDefault()
		{
			ResRecommendEquipInBattle defaultRecommendEquipInfo = this.GetDefaultRecommendEquipInfo(this.m_editHeroID, this.m_defaultCombinationID);
			ushort[] array = null;
			if (defaultRecommendEquipInfo != null)
			{
				array = defaultRecommendEquipInfo.RecommendEquipID;
			}
			if (array != null)
			{
				for (int i = 0; i < 6; i++)
				{
					this.m_editCustomRecommendEquips[i] = array[i];
				}
			}
			else
			{
				for (int j = 0; j < 6; j++)
				{
					this.m_editCustomRecommendEquips[j] = 0;
				}
			}
		}

		private void EditCustomRecommendEquipByGodEquip(int cnt, ref uint[] equipList)
		{
			if (equipList == null || cnt > equipList.Length)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				this.m_editCustomRecommendEquips[i] = 0;
			}
			int num = 0;
			while (num < cnt && num < 6)
			{
				this.m_editCustomRecommendEquips[num] = (ushort)equipList[num];
				num++;
			}
		}

		private void SaveEditCustomRecommendEquip()
		{
			if (this.m_editHeroID == 0u)
			{
				return;
			}
			this.bEditEquip = false;
			if (this.IsEditCustomRecommendEquipUseDefaultSetting())
			{
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5203u);
				cSPkg.stPkgData.get_stRecoverSystemEquipChgReq().dwHeroId = this.m_editHeroID;
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
			}
			else
			{
				CSPkg cSPkg2 = NetworkModule.CreateDefaultCSPKG(5201u);
				cSPkg2.stPkgData.get_stSelfDefineHeroEquipChgReq().stHeroEquipChgInfo.dwHeroId = this.m_editHeroID;
				for (int i = 0; i < 6; i++)
				{
					cSPkg2.stPkgData.get_stSelfDefineHeroEquipChgReq().stHeroEquipChgInfo.HeroEquipList[i] = (uint)this.m_editCustomRecommendEquips[i];
				}
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg2, true);
			}
		}

		private bool IsEditCustomRecommendEquipUseDefaultSetting()
		{
			if (this.m_editHeroID == 0u)
			{
				return false;
			}
			ResRecommendEquipInBattle defaultRecommendEquipInfo = this.GetDefaultRecommendEquipInfo(this.m_editHeroID, 1u);
			ushort[] array = null;
			if (defaultRecommendEquipInfo != null)
			{
				array = defaultRecommendEquipInfo.RecommendEquipID;
			}
			if (array != null)
			{
				for (int i = 0; i < 6; i++)
				{
					if (this.m_editCustomRecommendEquips[i] != array[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		[MessageHandler(5202)]
		public static void RecieveSCSelfDefineHeroEquipChgRsp(CSPkg csPkg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (csPkg.stPkgData.get_stSelfDefineHeroEquipChgRsp().bResult == 0)
			{
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null)
				{
					uint dwHeroId = csPkg.stPkgData.get_stSelfDefineHeroEquipChgRsp().stHeroEquipChgInfo.dwHeroId;
					masterRoleInfo.m_customRecommendEquipsLastChangedHeroID = dwHeroId;
					ushort[] array = null;
					if (masterRoleInfo.m_customRecommendEquipDictionary.TryGetValue(dwHeroId, ref array))
					{
						for (int i = 0; i < 6; i++)
						{
							array[i] = (ushort)csPkg.stPkgData.get_stSelfDefineHeroEquipChgRsp().stHeroEquipChgInfo.HeroEquipList[i];
						}
					}
					else
					{
						array = new ushort[6];
						for (int j = 0; j < 6; j++)
						{
							array[j] = (ushort)csPkg.stPkgData.get_stSelfDefineHeroEquipChgRsp().stHeroEquipChgInfo.HeroEquipList[j];
						}
						masterRoleInfo.m_customRecommendEquipDictionary.Add(dwHeroId, array);
					}
					Singleton<CEquipSystem>.GetInstance().RefreshEquipCustomPanel(true);
					Singleton<CEquipSystem>.GetInstance().OpenTipsOnSvrRsp();
				}
			}
		}

		[MessageHandler(5204)]
		public static void RecieveSCRecoverSystemEquipRsp(CSPkg csPkg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (csPkg.stPkgData.get_stRecoverSystemEquipChgRsp().bResult == 0)
			{
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null)
				{
					uint dwHeroId = csPkg.stPkgData.get_stRecoverSystemEquipChgRsp().dwHeroId;
					masterRoleInfo.m_customRecommendEquipsLastChangedHeroID = dwHeroId;
					masterRoleInfo.m_customRecommendEquipDictionary.Remove(dwHeroId);
					Singleton<CEquipSystem>.GetInstance().RefreshEquipCustomPanel(true);
					Singleton<CEquipSystem>.GetInstance().OpenTipsOnSvrRsp();
				}
			}
		}

		private void InitUIEventListener()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_OpenForm, new CUIEventManager.OnUIEventHandler(this.OnCustomEquipOpen));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_FormClose, new CUIEventManager.OnUIEventHandler(this.OnCustomEquipClose));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_UsageListSelect, new CUIEventManager.OnUIEventHandler(this.OnCustomEquipListSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_ItemClick, new CUIEventManager.OnUIEventHandler(this.OnEuipItemClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_EditItemClick, new CUIEventManager.OnUIEventHandler(this.OnCustomEditItemClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_ViewEquipTree, new CUIEventManager.OnUIEventHandler(this.OnViewEquipTree));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_ChangeHero, new CUIEventManager.OnUIEventHandler(this.OnChangeHero));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_ModifyEquip, new CUIEventManager.OnUIEventHandler(this.OnModifyEquip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_ConfirmModify, new CUIEventManager.OnUIEventHandler(this.OnConfirmModifyEquip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_CancleModify, new CUIEventManager.OnUIEventHandler(this.OnCancleModifyEquip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_AddEquip, new CUIEventManager.OnUIEventHandler(this.OnAddEquip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_DeleteEquip, new CUIEventManager.OnUIEventHandler(this.OnDeleteEquip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_ShowConfirmRevertDefaultTip, new CUIEventManager.OnUIEventHandler(this.OnShowConfirmRevertDefaultTip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_RevertDefault, new CUIEventManager.OnUIEventHandler(this.OnRevertDefaultEquip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_Expand, new CUIEventManager.OnUIEventHandler(this.OnExpandCustomEquip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_PackUp, new CUIEventManager.OnUIEventHandler(this.OnPackUpCustomEquip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_HeroTypeListSelect, new CUIEventManager.OnUIEventHandler(this.OnHeroTypeListSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_HeroListItemEnable, new CUIEventManager.OnUIEventHandler(this.OnHeroListElementEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_HeroListItemClick, new CUIEventManager.OnUIEventHandler(this.OnHeroListElementClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_OwnFlagChange, new CUIEventManager.OnUIEventHandler(this.OnHeroOwnFlagChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_ViewGodEquip, new CUIEventManager.OnUIEventHandler(this.OnViewGodEquip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_GodEquipItemEnable, new CUIEventManager.OnUIEventHandler(this.OnGodEquipItemEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_GodEquipUseBtnClick, new CUIEventManager.OnUIEventHandler(this.OnGodEquipUseBtnClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_GodEquipReqTimeOut, new CUIEventManager.OnUIEventHandler(this.OnReqGodEquipTimeOut));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_BackEquipListElementEnable, new CUIEventManager.OnUIEventHandler(this.OnBackEquipListElementEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_CircleTimerUp, new CUIEventManager.OnUIEventHandler(this.OnCircleTimerUp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_OnEquipTreeClosed, new CUIEventManager.OnUIEventHandler(this.OnEquipTreeClosed));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_OnEquipTreeItemSelected, new CUIEventManager.OnUIEventHandler(this.OnEquipTreeItemSelected));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_OnBackEquipListSelectChanged, new CUIEventManager.OnUIEventHandler(this.OnBackEquipListSelectChanged));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_OnGodEquipTabChanged, new CUIEventManager.OnUIEventHandler(this.OnGodEquipTabChanged));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_GodEquipSysUseBtnClick, new CUIEventManager.OnUIEventHandler(this.OnGodEquipSysUseBtnClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_IWantJudgeBtnClick, new CUIEventManager.OnUIEventHandler(this.OnIWantJudgeBtnClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_JudgeMarkSubmitBtnClick, new CUIEventManager.OnUIEventHandler(this.OnJudgeSubmitBtnClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.CustomEquip_OpenJudgeRule, new CUIEventManager.OnUIEventHandler(this.OnJudgeRuleBtnClick));
			Singleton<EventRouter>.GetInstance().AddEventHandler<SCPKG_GET_RANKING_LIST_RSP>(EventID.CUSTOM_EQUIP_RANK_LIST_GET, new Action<SCPKG_GET_RANKING_LIST_RSP>(this.OnGetCustomEquipRankList));
		}

		private void UinitUIEventListener()
		{
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_OpenForm, new CUIEventManager.OnUIEventHandler(this.OnCustomEquipOpen));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_FormClose, new CUIEventManager.OnUIEventHandler(this.OnCustomEquipClose));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_UsageListSelect, new CUIEventManager.OnUIEventHandler(this.OnCustomEquipListSelect));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_ItemClick, new CUIEventManager.OnUIEventHandler(this.OnEuipItemClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_EditItemClick, new CUIEventManager.OnUIEventHandler(this.OnCustomEditItemClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_ViewEquipTree, new CUIEventManager.OnUIEventHandler(this.OnViewEquipTree));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_ChangeHero, new CUIEventManager.OnUIEventHandler(this.OnChangeHero));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_ModifyEquip, new CUIEventManager.OnUIEventHandler(this.OnModifyEquip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_ConfirmModify, new CUIEventManager.OnUIEventHandler(this.OnConfirmModifyEquip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_CancleModify, new CUIEventManager.OnUIEventHandler(this.OnCancleModifyEquip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_AddEquip, new CUIEventManager.OnUIEventHandler(this.OnAddEquip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_DeleteEquip, new CUIEventManager.OnUIEventHandler(this.OnDeleteEquip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_ShowConfirmRevertDefaultTip, new CUIEventManager.OnUIEventHandler(this.OnShowConfirmRevertDefaultTip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_RevertDefault, new CUIEventManager.OnUIEventHandler(this.OnRevertDefaultEquip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_Expand, new CUIEventManager.OnUIEventHandler(this.OnExpandCustomEquip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_PackUp, new CUIEventManager.OnUIEventHandler(this.OnPackUpCustomEquip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_ViewGodEquip, new CUIEventManager.OnUIEventHandler(this.OnViewGodEquip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_HeroTypeListSelect, new CUIEventManager.OnUIEventHandler(this.OnHeroTypeListSelect));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_HeroListItemEnable, new CUIEventManager.OnUIEventHandler(this.OnHeroListElementEnable));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_HeroListItemClick, new CUIEventManager.OnUIEventHandler(this.OnHeroListElementClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_OwnFlagChange, new CUIEventManager.OnUIEventHandler(this.OnHeroOwnFlagChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_GodEquipItemEnable, new CUIEventManager.OnUIEventHandler(this.OnGodEquipItemEnable));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_GodEquipUseBtnClick, new CUIEventManager.OnUIEventHandler(this.OnGodEquipUseBtnClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_GodEquipReqTimeOut, new CUIEventManager.OnUIEventHandler(this.OnReqGodEquipTimeOut));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_BackEquipListElementEnable, new CUIEventManager.OnUIEventHandler(this.OnBackEquipListElementEnable));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_CircleTimerUp, new CUIEventManager.OnUIEventHandler(this.OnCircleTimerUp));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_OnEquipTreeClosed, new CUIEventManager.OnUIEventHandler(this.OnEquipTreeClosed));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_OnEquipTreeItemSelected, new CUIEventManager.OnUIEventHandler(this.OnEquipTreeItemSelected));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_OnBackEquipListSelectChanged, new CUIEventManager.OnUIEventHandler(this.OnBackEquipListSelectChanged));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_OnGodEquipTabChanged, new CUIEventManager.OnUIEventHandler(this.OnGodEquipTabChanged));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_GodEquipSysUseBtnClick, new CUIEventManager.OnUIEventHandler(this.OnGodEquipSysUseBtnClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_IWantJudgeBtnClick, new CUIEventManager.OnUIEventHandler(this.OnIWantJudgeBtnClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_JudgeMarkSubmitBtnClick, new CUIEventManager.OnUIEventHandler(this.OnJudgeSubmitBtnClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.CustomEquip_OpenJudgeRule, new CUIEventManager.OnUIEventHandler(this.OnJudgeRuleBtnClick));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<SCPKG_GET_RANKING_LIST_RSP>(EventID.CUSTOM_EQUIP_RANK_LIST_GET, new Action<SCPKG_GET_RANKING_LIST_RSP>(this.OnGetCustomEquipRankList));
		}

		private GameObject GetEquipListNodeWidget(enEquipListNodeWidget widgetId)
		{
			if (this.m_customEquipForm != null)
			{
				GameObject widget = this.m_customEquipForm.GetWidget(5);
				if (widget != null)
				{
					Transform transform = widget.transform.Find("Panel_EquipList");
					if (transform != null)
					{
						CUIComponent component = transform.GetComponent<CUIComponent>();
						if (component != null)
						{
							return component.GetWidget((int)widgetId);
						}
					}
				}
			}
			return null;
		}

		private void InitEquipPathLine()
		{
			if (this.m_customEquipForm != null)
			{
				this.m_equipRelationPath.Clear();
				GameObject equipListNodeWidget = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipVerticalLinesPanel);
				if (null == equipListNodeWidget)
				{
					return;
				}
				Transform transform = equipListNodeWidget.transform;
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < 11; j++)
					{
						Transform transform2 = transform.Find(string.Format("imgLine_{0}_{1}", i, j));
						this.m_equipRelationPath.InitializeVerticalLine(i, j, j + 1, transform2.gameObject);
					}
				}
				GameObject equipListNodeWidget2 = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipPanel_Level1);
				if (equipListNodeWidget2 != null)
				{
					this.InitEquipItemHorizontalLine(equipListNodeWidget2.transform, 1);
				}
				GameObject equipListNodeWidget3 = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipPanel_Level2);
				if (equipListNodeWidget3 != null)
				{
					this.InitEquipItemHorizontalLine(equipListNodeWidget3.transform, 2);
				}
				GameObject equipListNodeWidget4 = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipPanel_Level3);
				if (equipListNodeWidget4 != null)
				{
					this.InitEquipItemHorizontalLine(equipListNodeWidget4.transform, 3);
				}
			}
		}

		private void InitEquipItemHorizontalLine(Transform equipPanel, int level)
		{
			if (null == equipPanel)
			{
				return;
			}
			for (int i = 0; i < 12; i++)
			{
				Transform transform = equipPanel.Find(string.Format("equipItem{0}", i));
				Transform transform2 = transform.Find("imgLineFront");
				if (level > 1)
				{
					int index = (level <= 2) ? 0 : 1;
					this.m_equipRelationPath.InitializeHorizontalLine(index, i, CEquipLineSet.enHorizontalLineType.Right, transform2.gameObject);
				}
				Transform transform3 = transform.Find("imgLineBack");
				if (level < 3)
				{
					int index = (level >= 2) ? 1 : 0;
					this.m_equipRelationPath.InitializeHorizontalLine(index, i, CEquipLineSet.enHorizontalLineType.Left, transform3.gameObject);
				}
			}
		}

		private void OnCustomEquipOpen(CUIEvent uiEvent)
		{
			this.m_customEquipForm = Singleton<CUIManager>.GetInstance().OpenForm(CEquipSystem.s_CustomRecommendEquipPath, false, true);
			this.m_curEquipUsage = enEquipUsage.PhyAttack;
			this.bEditEquip = false;
			this.m_useGodEquip = false;
			this.m_revertDefaultEquip = false;
			if (this.m_customEquipForm != null)
			{
				if (this.IsEquipListNodeExsit())
				{
					this.OnEquipListNodeLoaded();
				}
				else
				{
					GameObject widget = this.m_customEquipForm.GetWidget(6);
					if (widget != null)
					{
						widget.CustomSetActive(true);
						CUITimerScript component = widget.GetComponent<CUITimerScript>();
						if (component != null)
						{
							component.StartTimer();
						}
					}
				}
				if (this.m_uiCustomEquipContentHeight == 0f)
				{
					GameObject widget2 = this.m_customEquipForm.GetWidget(12);
					if (widget2 != null)
					{
						this.m_uiCustomEquipContentHeight = (widget2.transform as RectTransform).rect.height;
					}
				}
				GameObject widget3 = this.m_customEquipForm.GetWidget(0);
				CTextManager instance = Singleton<CTextManager>.GetInstance();
				string[] titleList = new string[]
				{
					instance.GetText("Equip_Usage_PhyAttack"),
					instance.GetText("Equip_Usage_MagicAttack"),
					instance.GetText("Equip_Usage_Defence"),
					instance.GetText("Equip_Usage_Move"),
					instance.GetText("Equip_Usage_Jungle")
				};
				CUICommonSystem.InitMenuPanel(widget3, titleList, this.m_curEquipUsage - enEquipUsage.PhyAttack, true);
				this.RefreshEquipCustomPanel(true);
				GameObject widget4 = this.m_customEquipForm.GetWidget(13);
				if (widget4 != null)
				{
					widget4.CustomSetActive(!CSysDynamicBlock.bLobbyEntryBlocked);
				}
				MonoSingleton<NewbieGuideManager>.GetInstance().CheckTriggerTime(NewbieGuideTriggerTimeType.onEnterCustomRecommendEquip, new uint[0]);
			}
			CMiShuSystem.SendUIClickToServer(enUIClickReprotID.rp_BeizhanBtn);
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagFormCustomEquipShow(false);
		}

		private void OnEquipListNodeLoaded()
		{
			if (null == this.m_customEquipForm)
			{
				return;
			}
			GameObject widget = this.m_customEquipForm.GetWidget(6);
			if (widget != null)
			{
				widget.CustomSetActive(false);
			}
			if (this.m_uiEquipItemHeight == 0f)
			{
				GameObject equipListNodeWidget = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipItem);
				if (equipListNodeWidget != null)
				{
					this.m_uiEquipItemHeight = (equipListNodeWidget.transform as RectTransform).rect.height;
				}
			}
			if (this.m_uiEquipItemContentDefaultHeight == 0f)
			{
				GameObject equipListNodeWidget2 = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipItemContent);
				if (equipListNodeWidget2 != null)
				{
					this.m_uiEquipItemContentDefaultHeight = (equipListNodeWidget2.transform as RectTransform).rect.height;
				}
			}
			if (this.m_uiCustomEquipContentHeight == 0f)
			{
				GameObject widget2 = this.m_customEquipForm.GetWidget(12);
				if (widget2 != null)
				{
					this.m_uiCustomEquipContentHeight = (widget2.transform as RectTransform).rect.height;
				}
			}
			this.InitEquipPathLine();
			this.RefreshEquipListPanel(true);
		}

		private bool IsEquipListNodeExsit()
		{
			if (this.m_customEquipForm != null)
			{
				GameObject widget = this.m_customEquipForm.GetWidget(5);
				if (widget != null)
				{
					return widget.transform.Find("Panel_EquipList") != null;
				}
			}
			return false;
		}

		private void OnCircleTimerUp(CUIEvent uiEvent)
		{
			this.LoadEquipListNode();
			this.OnEquipListNodeLoaded();
		}

		private void OnEquipTreeClosed(CUIEvent uiEvent)
		{
			if (this.m_selectedEquipItemInEquipTree != null)
			{
				this.SetItemSelectedInEquipTree(this.m_selectedEquipItemInEquipTree, false);
				this.m_selectedEquipItemInEquipTree = null;
			}
			this.m_selectedEquipInfoInEquipTree = null;
		}

		private void OnEquipTreeItemSelected(CUIEvent uiEvent)
		{
			this.SelectItemInEquipTree(uiEvent.m_srcFormScript, uiEvent.m_srcWidget.transform, uiEvent.m_eventParams.battleEquipPar.equipInfo);
		}

		private void OnBackEquipListSelectChanged(CUIEvent uiEvent)
		{
			if (this.m_selectedEquipInfoInEquipTree == null || this.m_selectedEquipInfoInEquipTree.m_backEquipIDs == null)
			{
				return;
			}
			int selectedIndex = (uiEvent.m_srcWidgetScript as CUIListScript).GetSelectedIndex();
			if (selectedIndex < 0 || selectedIndex >= this.m_selectedEquipInfoInEquipTree.m_backEquipIDs.get_Count())
			{
				return;
			}
			CEquipInfo equipInfo = CEquipSystem.GetEquipInfo(this.m_selectedEquipInfoInEquipTree.m_backEquipIDs.get_Item(selectedIndex));
			if (equipInfo == null)
			{
				DebugHelper.Assert(equipInfo != null, "GetEquipInfo is null equipId = " + this.m_selectedEquipInfoInEquipTree.m_backEquipIDs.get_Item(selectedIndex));
				return;
			}
			this.RefreshEquipTreeForm(uiEvent.m_srcFormScript, equipInfo);
		}

		private void OnGodEquipTabChanged(CUIEvent uiEvent)
		{
			CUIListScript component = uiEvent.m_srcWidget.GetComponent<CUIListScript>();
			if (component == null)
			{
				DebugHelper.Assert(false, "CEquipSystem.OnGodEquipTabChanged(): lst is null!!!");
				return;
			}
			CUIListElementScript selectedElement = component.GetSelectedElement();
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CEquipSystem.s_GodEquipPath);
			if (form == null)
			{
				return;
			}
			GameObject widget = form.GetWidget(1);
			GameObject widget2 = form.GetWidget(2);
			this.m_godEquipCurTab = (enGodEquipTab)component.GetSelectedIndex();
			enGodEquipTab godEquipCurTab = this.m_godEquipCurTab;
			if (godEquipCurTab != enGodEquipTab.EN_System)
			{
				if (godEquipCurTab == enGodEquipTab.EN_God)
				{
					widget.CustomSetActive(true);
					widget2.CustomSetActive(false);
				}
			}
			else
			{
				widget.CustomSetActive(false);
				widget2.CustomSetActive(true);
			}
		}

		private void SelectItemInEquipTree(CUIFormScript equipTreeFormScript, Transform selectedItem, CEquipInfo equipInfo)
		{
			if (selectedItem == this.m_selectedEquipItemInEquipTree && equipInfo == this.m_selectedEquipInfoInEquipTree)
			{
				return;
			}
			if (this.m_selectedEquipItemInEquipTree != null)
			{
				this.SetItemSelectedInEquipTree(this.m_selectedEquipItemInEquipTree, false);
			}
			this.m_selectedEquipItemInEquipTree = selectedItem;
			this.m_selectedEquipInfoInEquipTree = equipInfo;
			if (this.m_selectedEquipItemInEquipTree != null)
			{
				this.SetItemSelectedInEquipTree(this.m_selectedEquipItemInEquipTree, true);
			}
			this.RefreshRightPanelInEquipTreeForm(equipTreeFormScript, equipInfo);
			GameObject widget = equipTreeFormScript.GetWidget(1);
			if (widget != null)
			{
				this.RefreshEquipBackList(widget.transform, equipInfo);
			}
		}

		private void SetItemSelectedInEquipTree(Transform equipItem, bool selected)
		{
			if (equipItem != null)
			{
				Transform transform = equipItem.Find("selectImg");
				if (transform != null)
				{
					transform.gameObject.CustomSetActive(selected);
				}
			}
		}

		private void LoadEquipListNode()
		{
			if (this.m_customEquipForm != null)
			{
				GameObject widget = this.m_customEquipForm.GetWidget(5);
				if (widget != null)
				{
					CUICommonSystem.LoadUIPrefab(CEquipSystem.s_EquipListNodePrefabPath, "Panel_EquipList", widget, this.m_customEquipForm);
				}
			}
		}

		private void OnCustomEquipClose(CUIEvent uiEvent)
		{
			this.ClearCurSelectEquipItem();
			this.m_equipRelationPath.Clear();
			this.m_customEquipForm = null;
			this.bEditEquip = false;
			this.m_useGodEquip = false;
			this.m_revertDefaultEquip = false;
		}

		private void ClearCurSelectEquipItem()
		{
			this.m_equipRelationPath.Reset();
			if (this.m_selEquipItemObj != null)
			{
				this.SetEquipItemSelectFlag(this.m_selEquipItemObj, false);
				this.m_selEquipItemObj = null;
			}
			this.m_selEquipInfo = null;
		}

		private void SetEquipItemSelectFlag(Transform equipItemObj, bool bSelect)
		{
			if (equipItemObj != null)
			{
				Transform transform = equipItemObj.Find("selectImg");
				if (transform != null)
				{
					transform.gameObject.CustomSetActive(bSelect);
				}
			}
		}

		private void OnEuipItemClick(CUIEvent uiEvent)
		{
			if (null == this.m_customEquipForm)
			{
				return;
			}
			this.ClearCurSelectEquipItem();
			this.m_selEquipInfo = uiEvent.m_eventParams.battleEquipPar.equipInfo;
			this.m_selEquipItemObj = uiEvent.m_srcWidget.transform;
			if (this.m_selEquipInfo != null && this.m_selEquipItemObj != null)
			{
				this.SetEquipItemSelectFlag(this.m_selEquipItemObj, true);
				this.RefreshRightPanel(this.m_selEquipInfo);
				this.m_equipRelationPath.Display(this.m_selEquipInfo.m_equipID, this.m_equipList[(int)this.m_curEquipUsage], CEquipSystem.s_equipInfoDictionary);
			}
		}

		private void OnCustomEquipListSelect(CUIEvent uiEvent)
		{
			this.ClearCurSelectEquipItem();
			this.CloseRightPanel();
			CUIListScript cUIListScript = (CUIListScript)uiEvent.m_srcWidgetScript;
			if (cUIListScript != null)
			{
				this.m_curEquipUsage = cUIListScript.GetSelectedIndex() + enEquipUsage.PhyAttack;
				this.RefreshEquipListPanel(true);
			}
		}

		private void OnCustomEditItemClick(CUIEvent uiEvent)
		{
			CEquipInfo equipInfo = uiEvent.m_eventParams.battleEquipPar.equipInfo;
			Transform transform = uiEvent.m_srcWidget.transform;
			if (equipInfo == null || null == this.m_customEquipForm)
			{
				return;
			}
			this.ClearCurSelectEquipItem();
			if (equipInfo.m_resEquipInBattle.bUsage != (byte)this.m_curEquipUsage)
			{
				GameObject widget = this.m_customEquipForm.GetWidget(0);
				if (widget != null)
				{
					CUIListScript component = widget.GetComponent<CUIListScript>();
					if (component != null)
					{
						component.SelectElement((int)(equipInfo.m_resEquipInBattle.bUsage - 1), true);
					}
				}
			}
			this.m_equipRelationPath.Display(equipInfo.m_equipID, this.m_equipList[(int)this.m_curEquipUsage], CEquipSystem.s_equipInfoDictionary);
			this.m_selEquipItemObj = transform;
			this.SetEquipItemSelectFlag(this.m_selEquipItemObj, true);
			this.RefreshRightPanel(equipInfo);
		}

		private void RefreshEquipCustomPanel(bool bRefreshHero)
		{
			if (null == this.m_customEquipForm)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "RefreshEquipCustomPanel role is null");
			if (masterRoleInfo == null)
			{
				return;
			}
			if (bRefreshHero)
			{
				this.RefreshCustomEquipHero();
			}
			this.RefreshCustomEquips(true);
			GameObject widget = this.m_customEquipForm.GetWidget(8);
			GameObject widget2 = this.m_customEquipForm.GetWidget(9);
			GameObject widget3 = this.m_customEquipForm.GetWidget(10);
			if (widget != null)
			{
				widget.CustomSetActive(!this.bEditEquip);
			}
			if (widget2 != null)
			{
				widget2.CustomSetActive(this.bEditEquip);
			}
			if (widget3 != null)
			{
				widget3.CustomSetActive(this.bEditEquip);
			}
		}

		private void RefreshCustomEquipHero()
		{
			if (null == this.m_customEquipForm)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "RefreshEquipCustomPanel role is null");
			if (masterRoleInfo == null)
			{
				return;
			}
			GameObject widget = this.m_customEquipForm.GetWidget(7);
			ResHeroCfgInfo dataByKey;
			if (masterRoleInfo.m_customRecommendEquipsLastChangedHeroID == 0u)
			{
				dataByKey = GameDataMgr.heroDatabin.GetDataByKey(masterRoleInfo.GetFirstHeroId());
			}
			else
			{
				dataByKey = GameDataMgr.heroDatabin.GetDataByKey(masterRoleInfo.m_customRecommendEquipsLastChangedHeroID);
			}
			if (dataByKey == null)
			{
				return;
			}
			masterRoleInfo.m_customRecommendEquipsLastChangedHeroID = dataByKey.dwCfgID;
			CUICommonSystem.SetHeroItemImage(this.m_customEquipForm, widget.gameObject, dataByKey.szImagePath, enHeroHeadType.enIcon, false, false);
		}

		private void RefreshCustomEquips(bool bInitEditEquips)
		{
			if (null == this.m_customEquipForm)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return;
			}
			bool flag = false;
			if (bInitEditEquips)
			{
				this.InitializeEditCustomRecommendEquips(masterRoleInfo.m_customRecommendEquipsLastChangedHeroID, ref flag);
			}
			GameObject widget = this.m_customEquipForm.GetWidget(1);
			for (int i = 0; i < this.m_editCustomRecommendEquips.Length; i++)
			{
				Transform transform = widget.transform.Find("equipItem" + i);
				if (transform != null)
				{
					Transform transform2 = transform.Find("addButton");
					Transform transform3 = transform.Find("deleteButton");
					Transform transform4 = transform.Find("imgIcon");
					CEquipInfo equipInfo = CEquipSystem.GetEquipInfo(this.m_editCustomRecommendEquips[i]);
					Transform transform5 = transform.FindChild("imgActiveEquip");
					Transform transform6 = transform.FindChild("imgEyeEquip");
					if (equipInfo != null)
					{
						transform4.gameObject.CustomSetActive(true);
						this.RefreshEquipItemSimpleInfo(transform, equipInfo);
						CUIMiniEventScript component = transform.GetComponent<CUIMiniEventScript>();
						if (component != null)
						{
							component.m_onClickEventID = enUIEventID.CustomEquip_EditItemClick;
							component.m_onClickEventParams.battleEquipPar.equipInfo = equipInfo;
						}
						transform3.gameObject.CustomSetActive(this.bEditEquip);
						transform2.gameObject.CustomSetActive(false);
						if (this.bEditEquip)
						{
							CUIMiniEventScript component2 = transform3.GetComponent<CUIMiniEventScript>();
							if (component2 != null)
							{
								component2.m_onClickEventID = enUIEventID.CustomEquip_DeleteEquip;
								component2.m_onClickEventParams.tag = i;
							}
						}
						if (transform6 && transform5)
						{
							if (equipInfo.m_resEquipInBattle.dwActiveSkillID > 0u)
							{
								if (equipInfo.m_resEquipInBattle.bUsage == 6)
								{
									transform6.gameObject.CustomSetActive(true);
									transform5.gameObject.CustomSetActive(false);
								}
								else
								{
									transform5.gameObject.CustomSetActive(true);
									transform6.gameObject.CustomSetActive(false);
								}
							}
							else
							{
								transform5.gameObject.CustomSetActive(false);
								transform6.gameObject.CustomSetActive(false);
							}
						}
					}
					else
					{
						transform4.gameObject.CustomSetActive(false);
						transform3.gameObject.CustomSetActive(false);
						transform2.gameObject.CustomSetActive(this.bEditEquip);
						this.SetEquipItemSelectFlag(transform, false);
						if (this.bEditEquip)
						{
							CUIMiniEventScript component3 = transform2.GetComponent<CUIMiniEventScript>();
							if (component3 != null)
							{
								component3.m_onClickEventID = enUIEventID.CustomEquip_AddEquip;
								component3.m_onClickEventParams.tag = i;
							}
						}
						if (transform6 && transform5)
						{
							transform5.gameObject.CustomSetActive(false);
							transform6.gameObject.CustomSetActive(false);
						}
					}
				}
			}
		}

		private void RefreshEquipListPanel(bool isSwichUsage)
		{
			if (null == this.m_customEquipForm)
			{
				return;
			}
			GameObject equipListNodeWidget = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipPanel_Level1);
			if (equipListNodeWidget != null)
			{
				this.RefreshEquipLevelPanel(equipListNodeWidget.transform, 1);
			}
			GameObject equipListNodeWidget2 = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipPanel_Level2);
			if (equipListNodeWidget2 != null)
			{
				this.RefreshEquipLevelPanel(equipListNodeWidget2.transform, 2);
			}
			GameObject equipListNodeWidget3 = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipPanel_Level3);
			if (equipListNodeWidget3 != null)
			{
				this.RefreshEquipLevelPanel(equipListNodeWidget3.transform, 3);
			}
			if (isSwichUsage)
			{
				int num = 0;
				List<ushort>[] array = this.m_equipList[(int)this.m_curEquipUsage];
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].get_Count() > num)
					{
						num = array[i].get_Count();
					}
				}
				float num2 = this.m_uiEquipItemContentDefaultHeight - (float)(12 - num) * this.m_uiEquipItemHeight;
				GameObject equipListNodeWidget4 = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipItemContent);
				if (equipListNodeWidget4 != null)
				{
					RectTransform rectTransform = equipListNodeWidget4.transform as RectTransform;
					if (this.IsCustomEquipPanelExpanded())
					{
						rectTransform.offsetMin = new Vector2(0f, -num2 - this.m_uiEquipItemContentHightDiff);
					}
					else
					{
						rectTransform.offsetMin = new Vector2(0f, -num2);
					}
					rectTransform.offsetMax = new Vector2(0f, 0f);
				}
			}
		}

		private void RefreshEquipLevelPanel(Transform equipPanel, int level)
		{
			if (null == equipPanel)
			{
				return;
			}
			List<ushort> list = this.m_equipList[(int)this.m_curEquipUsage][level - 1];
			int count = list.get_Count();
			int i = 0;
			while (i < 12 && i < count)
			{
				Transform transform = equipPanel.Find(string.Format("equipItem{0}", i));
				this.RefreshEquipItem(transform, list.get_Item(i));
				CanvasGroup component = transform.GetComponent<CanvasGroup>();
				if (component != null)
				{
					component.alpha = 1f;
					component.blocksRaycasts = true;
				}
				i++;
			}
			while (i < 12)
			{
				Transform transform2 = equipPanel.Find(string.Format("equipItem{0}", i));
				CanvasGroup component2 = transform2.GetComponent<CanvasGroup>();
				if (component2 != null)
				{
					component2.alpha = 0f;
					component2.blocksRaycasts = false;
				}
				i++;
			}
		}

		private void RefreshEquipItem(Transform equipItem, ushort equipID)
		{
			if (null == equipItem || null == this.m_customEquipForm || equipID == 0)
			{
				return;
			}
			CEquipInfo equipInfo = CEquipSystem.GetEquipInfo(equipID);
			if (equipInfo == null)
			{
				DebugHelper.Assert(equipInfo != null, "GetEquipInfo is null equipId = " + equipID);
				return;
			}
			this.RefreshEquipItemSimpleInfo(equipItem, equipInfo);
			ResEquipInBattle resEquipInBattle = equipInfo.m_resEquipInBattle;
			if (resEquipInBattle != null)
			{
				Image component = equipItem.Find("imgIcon").GetComponent<Image>();
				component.SetSprite(equipInfo.m_equipIconPath, this.m_customEquipForm, true, false, false, false);
				Text component2 = equipItem.Find("nameText").GetComponent<Text>();
				component2.text = equipInfo.m_equipName;
				Text component3 = equipItem.Find("priceText").GetComponent<Text>();
				component3.text = resEquipInBattle.dwBuyPrice.ToString();
				CUIMiniEventScript component4 = equipItem.GetComponent<CUIMiniEventScript>();
				if (component4 != null)
				{
					component4.m_onClickEventID = enUIEventID.CustomEquip_ItemClick;
					component4.m_onClickEventParams.battleEquipPar.equipInfo = equipInfo;
				}
				Transform transform = equipItem.FindChild("imgActiveEquip");
				Transform transform2 = equipItem.FindChild("imgEyeEquip");
				if (transform2 && transform)
				{
					if (equipInfo.m_resEquipInBattle.dwActiveSkillID > 0u)
					{
						if (equipInfo.m_resEquipInBattle.bUsage == 6)
						{
							transform2.gameObject.CustomSetActive(true);
							transform.gameObject.CustomSetActive(false);
						}
						else
						{
							transform.gameObject.CustomSetActive(true);
							transform2.gameObject.CustomSetActive(false);
						}
					}
					else
					{
						transform.gameObject.CustomSetActive(false);
						transform2.gameObject.CustomSetActive(false);
					}
				}
			}
		}

		private void RefreshRightPanel(CEquipInfo equipInfo)
		{
			if (null == this.m_customEquipForm || equipInfo == null)
			{
				return;
			}
			GameObject widget = this.m_customEquipForm.GetWidget(2);
			if (widget != null)
			{
				widget.CustomSetActive(true);
				Text component = widget.transform.Find("equipNameText").GetComponent<Text>();
				component.text = equipInfo.m_equipName;
				Text component2 = widget.transform.Find("Panel_euipProperty/equipPropertyDescText").GetComponent<Text>();
				component2.text = equipInfo.m_equipPropertyDesc;
				RectTransform rectTransform = component2.transform as RectTransform;
				rectTransform.anchoredPosition = new Vector2(0f, 0f);
			}
			GameObject widget2 = this.m_customEquipForm.GetWidget(3);
			widget2.CustomSetActive(false);
			Text component3 = widget2.transform.Find("buyPriceText").GetComponent<Text>();
			component3.text = equipInfo.m_resEquipInBattle.dwBuyPrice.ToString();
			GameObject widget3 = this.m_customEquipForm.GetWidget(4);
			widget3.CustomSetActive(true);
			CUIEventScript component4 = widget3.GetComponent<CUIEventScript>();
			if (component4 != null)
			{
				stUIEventParams eventParams = default(stUIEventParams);
				eventParams.battleEquipPar.equipInfo = equipInfo;
				component4.SetUIEvent(enUIEventType.Click, enUIEventID.CustomEquip_ViewEquipTree, eventParams);
			}
		}

		public void CloseRightPanel()
		{
			if (null == this.m_customEquipForm)
			{
				return;
			}
			GameObject widget = this.m_customEquipForm.GetWidget(2);
			widget.CustomSetActive(false);
			GameObject widget2 = this.m_customEquipForm.GetWidget(3);
			widget2.CustomSetActive(false);
			GameObject widget3 = this.m_customEquipForm.GetWidget(4);
			widget3.CustomSetActive(false);
		}

		private void OnViewEquipTree(CUIEvent uiEvent)
		{
			CEquipInfo equipInfo = uiEvent.m_eventParams.battleEquipPar.equipInfo;
			if (equipInfo == null)
			{
				return;
			}
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CEquipSystem.s_EquipTreePath, false, true);
			if (cUIFormScript != null)
			{
				this.RefreshEquipTreeForm(cUIFormScript, equipInfo);
			}
		}

		private void OnBackEquipListElementEnable(CUIEvent uiEvent)
		{
			if (this.m_selectedEquipInfoInEquipTree == null)
			{
				return;
			}
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			if (srcWidgetIndexInBelongedList < 0 || srcWidgetIndexInBelongedList >= this.m_selectedEquipInfoInEquipTree.m_backEquipIDs.get_Count())
			{
				return;
			}
			if (uiEvent.m_srcWidget != null)
			{
				CEquipInfo equipInfo = CEquipSystem.GetEquipInfo(this.m_selectedEquipInfoInEquipTree.m_backEquipIDs.get_Item(srcWidgetIndexInBelongedList));
				if (equipInfo == null)
				{
					DebugHelper.Assert(equipInfo != null, "GetEquipInfo is null equipId = " + this.m_selectedEquipInfoInEquipTree.m_backEquipIDs.get_Item(srcWidgetIndexInBelongedList));
					return;
				}
				this.RefreshEquipItemSimpleInfo(uiEvent.m_srcWidget.transform, equipInfo);
			}
		}

		private void RefreshEquipTreeForm(CUIFormScript equipTreeForm, CEquipInfo equipInfo)
		{
			if (null == equipTreeForm || equipInfo == null)
			{
				return;
			}
			this.GetEquipTree(equipInfo.m_equipID, ref this.m_equipTree);
			GameObject widget = equipTreeForm.GetWidget(0);
			if (widget != null)
			{
				this.RefreshEquipTreePanel(widget.transform, ref this.m_equipTree, equipInfo);
				Transform selectedItem = widget.transform.Find("rootEquipItem");
				this.SelectItemInEquipTree(equipTreeForm, selectedItem, equipInfo);
			}
		}

		private void RefreshEquipTreePanel(Transform equipTreePanel, ref stEquipTree equipTree, CEquipInfo equipInfo)
		{
			if (null == equipTreePanel || equipInfo == null)
			{
				return;
			}
			Transform equipItem = equipTreePanel.transform.Find("rootEquipItem");
			this.RefreshEquipTreeItem(equipItem, equipInfo.m_equipID);
			Transform lineGroupPanel = equipTreePanel.transform.Find("lineGroupPanel");
			this.RefreshLineGroupPanel(lineGroupPanel, 3, (int)equipTree.m_2ndEquipCount);
			Transform transform = equipTreePanel.transform.Find("preEquipGroupPanel");
			if (null == transform)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				ushort num = equipTree.m_2ndEquipIDs[i];
				Transform transform2 = transform.Find("preEquipGroup" + i);
				if (transform2 != null)
				{
					transform2.gameObject.CustomSetActive(num > 0);
					if (num > 0)
					{
						Transform equipItem2 = transform2.Find("2ndEquipItem");
						this.RefreshEquipTreeItem(equipItem2, num);
						lineGroupPanel = transform2.transform.Find("lineGroupPanel");
						this.RefreshLineGroupPanel(lineGroupPanel, 2, (int)equipTree.m_3rdEquipCounts[i]);
						for (int j = 0; j < 2; j++)
						{
							ushort num2 = equipTree.m_3rdEquipIDs[i][j];
							Transform transform3 = transform2.Find("preEquipPanel/3rdEquipItem" + j);
							transform3.gameObject.CustomSetActive(num2 > 0);
							this.RefreshEquipTreeItem(transform3, num2);
						}
					}
				}
			}
		}

		private void RefreshLineGroupPanel(Transform lineGroupPanel, int maxLineCnt, int curLineCnt)
		{
			if (null == lineGroupPanel)
			{
				return;
			}
			for (int i = 0; i < maxLineCnt; i++)
			{
				Transform transform = lineGroupPanel.Find("linePanel" + i);
				if (transform != null)
				{
					transform.gameObject.CustomSetActive(i + 1 == curLineCnt);
				}
			}
		}

		private void RefreshEquipBackList(Transform equipBackList, CEquipInfo equipInfo)
		{
			if (null == equipBackList)
			{
				return;
			}
			CUIListScript component = equipBackList.GetComponent<CUIListScript>();
			if (component != null)
			{
				component.SetElementAmount((equipInfo.m_backEquipIDs != null) ? equipInfo.m_backEquipIDs.get_Count() : 0);
				component.SelectElement(-1, false);
			}
		}

		private void RefreshEquipTreeItem(Transform equipItem, ushort equipID)
		{
			if (null == equipItem || null == this.m_customEquipForm || equipID == 0)
			{
				return;
			}
			CEquipInfo equipInfo = CEquipSystem.GetEquipInfo(equipID);
			if (equipInfo == null)
			{
				DebugHelper.Assert(equipInfo != null, "GetEquipInfo is null equipId = " + equipID);
				return;
			}
			this.RefreshEquipItemSimpleInfo(equipItem, equipInfo);
			ResEquipInBattle resEquipInBattle = equipInfo.m_resEquipInBattle;
			if (resEquipInBattle != null)
			{
				CUIMiniEventScript component = equipItem.GetComponent<CUIMiniEventScript>();
				if (component != null)
				{
					component.m_onClickEventID = enUIEventID.CustomEquip_OnEquipTreeItemSelected;
					component.m_onClickEventParams.battleEquipPar.equipInfo = equipInfo;
				}
			}
		}

		private void RefreshEquipItemSimpleInfo(Transform equipItem, CEquipInfo equipInfo)
		{
			if (null == equipItem || equipInfo == null)
			{
				return;
			}
			ResEquipInBattle resEquipInBattle = equipInfo.m_resEquipInBattle;
			if (resEquipInBattle != null)
			{
				Image component = equipItem.Find("imgIcon").GetComponent<Image>();
				component.SetSprite(equipInfo.m_equipIconPath, this.m_customEquipForm, true, false, false, false);
			}
		}

		private void RefreshRightPanelInEquipTreeForm(CUIFormScript equipTreeFormScript, CEquipInfo equipInfo)
		{
			GameObject widget = equipTreeFormScript.GetWidget(2);
			if (widget != null)
			{
				Text component = widget.transform.Find("equipNameText").GetComponent<Text>();
				component.text = equipInfo.m_equipName;
				Text component2 = widget.transform.Find("equipPropertyDescText").GetComponent<Text>();
				component2.text = equipInfo.m_equipPropertyDesc;
			}
			GameObject widget2 = equipTreeFormScript.GetWidget(3);
			if (widget2 != null)
			{
				widget2.GetComponent<Text>().text = equipInfo.m_resEquipInBattle.dwBuyPrice.ToString();
			}
		}

		private void OnModifyEquip(CUIEvent uiEvent)
		{
			this.bEditEquip = true;
			this.RefreshEquipCustomPanel(false);
		}

		private void OnCancleModifyEquip(CUIEvent uiEvent)
		{
			this.bEditEquip = false;
			this.RefreshEquipCustomPanel(false);
		}

		private void OnConfirmModifyEquip(CUIEvent uiEvent)
		{
			this.SaveEditCustomRecommendEquip();
		}

		private void OnAddEquip(CUIEvent uiEvent)
		{
			if (this.m_selEquipInfo == null)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("CustomEquip_ChooseEquipTip", true, 1.5f, null, new object[0]);
				return;
			}
			int tag = uiEvent.m_eventParams.tag;
			if (tag >= 0 && tag < this.m_editCustomRecommendEquips.Length)
			{
				this.m_editCustomRecommendEquips[tag] = this.m_selEquipInfo.m_equipID;
			}
			this.RefreshCustomEquips(false);
		}

		private void OnDeleteEquip(CUIEvent uiEvent)
		{
			int tag = uiEvent.m_eventParams.tag;
			if (tag >= 0 && tag < this.m_editCustomRecommendEquips.Length)
			{
				this.m_editCustomRecommendEquips[tag] = 0;
			}
			this.RefreshCustomEquips(false);
		}

		private void OnShowConfirmRevertDefaultTip(CUIEvent uiEvent)
		{
			string text = Singleton<CTextManager>.GetInstance().GetText("CustomEquip_ConfirmRevertDefaultTip");
			Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(text, enUIEventID.CustomEquip_RevertDefault, enUIEventID.None, false);
		}

		private void OnRevertDefaultEquip(CUIEvent uiEvent)
		{
			this.m_revertDefaultEquip = true;
			this.RevertEditCustomRecommendEquipToDefault();
			this.SaveEditCustomRecommendEquip();
		}

		private void ResetHeroList(enHeroJobType jobType, bool bOwn)
		{
			this.m_heroList.Clear();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "CEquipSystem ResetHeroList role is null");
			if (masterRoleInfo != null)
			{
				ListView<ResHeroCfgInfo> allHeroList = CHeroDataFactory.GetAllHeroList();
				for (int i = 0; i < allHeroList.get_Count(); i++)
				{
					if ((jobType == enHeroJobType.All || allHeroList.get_Item(i).bMainJob == (byte)jobType || allHeroList.get_Item(i).bMinorJob == (byte)jobType) && (!bOwn || masterRoleInfo.IsHaveHero(allHeroList.get_Item(i).dwCfgID, false)))
					{
						this.m_heroList.Add(allHeroList.get_Item(i));
					}
				}
			}
		}

		private void RefreshHeroListPanel()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CEquipSystem.s_ChooseHeroPath);
			if (null == form)
			{
				return;
			}
			GameObject widget = form.GetWidget(1);
			if (widget != null)
			{
				this.m_bOwnHeroFlag = widget.GetComponent<Toggle>().isOn;
			}
			this.ResetHeroList(this.m_curHeroJob, this.m_bOwnHeroFlag);
			GameObject widget2 = form.GetWidget(2);
			if (widget2 != null)
			{
				CUIListScript component = widget2.GetComponent<CUIListScript>();
				component.SetElementAmount(this.m_heroList.get_Count());
			}
		}

		private void OnChangeHero(CUIEvent uiEvent)
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CEquipSystem.s_ChooseHeroPath, false, true);
			if (null == cUIFormScript)
			{
				return;
			}
			this.m_curHeroJob = enHeroJobType.All;
			GameObject widget = cUIFormScript.GetWidget(0);
			if (widget != null)
			{
				GameObject widget2 = cUIFormScript.GetWidget(1);
				if (widget2 != null)
				{
					widget2.GetComponent<Toggle>().isOn = this.m_bOwnHeroFlag;
				}
				string text = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_All");
				string text2 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Tank");
				string text3 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Soldier");
				string text4 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Assassin");
				string text5 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Master");
				string text6 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Archer");
				string text7 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Aid");
				string[] titleList = new string[]
				{
					text,
					text2,
					text3,
					text4,
					text5,
					text6,
					text7
				};
				CUICommonSystem.InitMenuPanel(widget, titleList, (int)this.m_curHeroJob, true);
			}
		}

		private void OnHeroOwnFlagChange(CUIEvent uiEvent)
		{
			this.RefreshHeroListPanel();
		}

		private void OnHeroTypeListSelect(CUIEvent uiEvent)
		{
			CUIListScript cUIListScript = (CUIListScript)uiEvent.m_srcWidgetScript;
			if (cUIListScript != null)
			{
				this.m_curHeroJob = (enHeroJobType)cUIListScript.GetSelectedIndex();
				this.RefreshHeroListPanel();
			}
		}

		private void OnHeroListElementEnable(CUIEvent uiEvent)
		{
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			if (srcWidgetIndexInBelongedList < 0 || srcWidgetIndexInBelongedList >= this.m_heroList.get_Count())
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "CEquipSystem OnHeroListElementEnable role is null");
			if (masterRoleInfo == null || masterRoleInfo.m_customRecommendEquipDictionary == null)
			{
				return;
			}
			ResHeroCfgInfo resHeroCfgInfo = this.m_heroList.get_Item(srcWidgetIndexInBelongedList);
			if (resHeroCfgInfo != null && uiEvent.m_srcWidget != null)
			{
				Transform transform = uiEvent.m_srcWidget.transform.Find("heroItemCell");
				if (transform != null)
				{
					CUICommonSystem.SetHeroItemImage(uiEvent.m_srcFormScript, transform.gameObject, resHeroCfgInfo.szImagePath, enHeroHeadType.enIcon, !masterRoleInfo.IsHaveHero(resHeroCfgInfo.dwCfgID, false), false);
					CUIEventScript component = transform.GetComponent<CUIEventScript>();
					if (component != null)
					{
						component.SetUIEvent(enUIEventType.Click, enUIEventID.CustomEquip_HeroListItemClick, new stUIEventParams
						{
							heroId = resHeroCfgInfo.dwCfgID
						});
					}
					Transform transform2 = transform.Find("equipedPanel");
					if (transform2 != null)
					{
						int num = 0;
						bool flag = this.IsHeroCustomEquip(resHeroCfgInfo.dwCfgID, ref num);
						transform2.gameObject.CustomSetActive(flag);
						if (flag)
						{
							Text component2 = transform2.Find("Text").GetComponent<Text>();
							if (num < 6)
							{
								component2.text = Singleton<CTextManager>.GetInstance().GetText("CustomEquip_EquipCnt", new string[]
								{
									num.ToString(),
									6.ToString()
								});
							}
							else
							{
								component2.text = Singleton<CTextManager>.GetInstance().GetText("CustomEquip_EquipComplete");
							}
						}
					}
					Transform transform3 = transform.Find("TxtFree");
					if (transform3 != null)
					{
						transform3.gameObject.CustomSetActive(masterRoleInfo.IsFreeHero(resHeroCfgInfo.dwCfgID));
					}
				}
			}
		}

		private bool IsHeroCustomEquip(uint heroId, ref int cnt)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "CEquipSystem IsHeroCustomEquip role is null");
			if (masterRoleInfo == null || masterRoleInfo.m_customRecommendEquipDictionary == null)
			{
				return false;
			}
			ushort[] array = null;
			cnt = 0;
			if (masterRoleInfo.m_customRecommendEquipDictionary.TryGetValue(heroId, ref array))
			{
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						CEquipInfo equipInfo = CEquipSystem.GetEquipInfo(array[i]);
						if (equipInfo != null && equipInfo.m_resEquipInBattle.bInvalid == 0)
						{
							cnt++;
						}
					}
				}
				return true;
			}
			return false;
		}

		private void OnHeroListElementClick(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().CloseForm(CEquipSystem.s_ChooseHeroPath);
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				this.ClearCurSelectEquipItem();
				this.CloseRightPanel();
				masterRoleInfo.m_customRecommendEquipsLastChangedHeroID = uiEvent.m_eventParams.heroId;
				this.RefreshEquipCustomPanel(true);
			}
		}

		private void OnExpandCustomEquip(CUIEvent uiEvent)
		{
			if (null == this.m_customEquipForm)
			{
				return;
			}
			GameObject widget = this.m_customEquipForm.GetWidget(12);
			GameObject widget2 = this.m_customEquipForm.GetWidget(11);
			GameObject widget3 = this.m_customEquipForm.GetWidget(14);
			if (widget != null && widget2 != null && widget3 != null)
			{
				widget2.CustomSetActive(true);
				widget3.CustomSetActive(false);
				LeanTween.cancel(widget);
				RectTransform customContentRectTransform = widget.transform as RectTransform;
				Vector2 vector = new Vector2(customContentRectTransform.anchoredPosition.x, customContentRectTransform.anchoredPosition.y + this.m_uiCustomEquipContentHeight);
				LeanTween.value(widget, delegate(Vector2 pos)
				{
					customContentRectTransform.anchoredPosition = pos;
				}, customContentRectTransform.anchoredPosition, vector, this.c_moveAnimaTime);
			}
			GameObject equipListNodeWidget = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipItemContent);
			if (equipListNodeWidget != null)
			{
				RectTransform rectTransform = equipListNodeWidget.transform as RectTransform;
				float x = rectTransform.offsetMin.x;
				float y = rectTransform.offsetMin.y - this.m_uiEquipItemContentHightDiff;
				rectTransform.offsetMin = new Vector2(x, y);
			}
		}

		private void OnPackUpCustomEquip(CUIEvent uiEvent)
		{
			if (null == this.m_customEquipForm)
			{
				return;
			}
			GameObject widget = this.m_customEquipForm.GetWidget(12);
			GameObject equipCustomPanel = this.m_customEquipForm.GetWidget(11);
			GameObject equipExpandBtn = this.m_customEquipForm.GetWidget(14);
			if (widget != null && equipCustomPanel != null && equipExpandBtn != null)
			{
				LeanTween.cancel(widget);
				RectTransform customContentRectTransform = widget.transform as RectTransform;
				Vector2 vector = new Vector2(customContentRectTransform.anchoredPosition.x, customContentRectTransform.anchoredPosition.y - this.m_uiCustomEquipContentHeight);
				LeanTween.value(widget, delegate(Vector2 pos)
				{
					customContentRectTransform.anchoredPosition = pos;
				}, customContentRectTransform.anchoredPosition, vector, this.c_moveAnimaTime).setOnComplete(delegate
				{
					equipCustomPanel.CustomSetActive(false);
					equipExpandBtn.CustomSetActive(true);
				});
			}
			GameObject equipListNodeWidget = this.GetEquipListNodeWidget(enEquipListNodeWidget.EquipItemContent);
			if (equipListNodeWidget != null)
			{
				RectTransform rectTransform = equipListNodeWidget.transform as RectTransform;
				float x = rectTransform.offsetMin.x;
				float y = rectTransform.offsetMin.y + this.m_uiEquipItemContentHightDiff;
				rectTransform.offsetMin = new Vector2(x, y);
			}
		}

		private bool IsCustomEquipPanelExpanded()
		{
			if (this.m_customEquipForm != null)
			{
				GameObject widget = this.m_customEquipForm.GetWidget(11);
				if (widget != null)
				{
					return widget.activeSelf;
				}
			}
			return false;
		}

		private void OnViewGodEquip(CUIEvent uiEvent)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "OnViewGodEquip role is null");
			if (masterRoleInfo == null)
			{
				return;
			}
			this.m_reqRankHeroId = masterRoleInfo.m_customRecommendEquipsLastChangedHeroID;
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2602u);
			cSPkg.stPkgData.get_stGetRankingListReq().bNumberType = 22;
			cSPkg.stPkgData.get_stGetRankingListReq().iSubType = (int)masterRoleInfo.m_customRecommendEquipsLastChangedHeroID;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			Singleton<CUIManager>.GetInstance().OpenForm(CEquipSystem.s_GodEquipPath, false, true);
			this.InitSysGodEquipTab();
			Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(Singleton<CTextManager>.GetInstance().GetText("CustomEquip_ReqGodEquipTip"), 10, enUIEventID.CustomEquip_GodEquipReqTimeOut);
		}

		private void OnGetCustomEquipRankList(SCPKG_GET_RANKING_LIST_RSP rankListRsp)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			CSDT_RANKING_LIST_SUCC stOfSucc = rankListRsp.stRankingListDetail.get_stOfSucc();
			if (stOfSucc.bNumberType != 22)
			{
				return;
			}
			CEquipSystem.stEquipRankInfo stEquipRankInfo = default(CEquipSystem.stEquipRankInfo);
			stEquipRankInfo.equipRankItemCnt = (int)stOfSucc.dwItemNum;
			stEquipRankInfo.rankDetail = stOfSucc.astItemDetail;
			if (this.m_equipRankItemDetail.ContainsKey(this.m_reqRankHeroId))
			{
				this.m_equipRankItemDetail.set_Item(this.m_reqRankHeroId, stEquipRankInfo);
			}
			else
			{
				this.m_equipRankItemDetail.Add(this.m_reqRankHeroId, stEquipRankInfo);
			}
			this.RefreshGodEquipForm(this.m_reqRankHeroId);
		}

		private void OnReqGodEquipTimeOut(CUIEvent uiEvent)
		{
			this.RefreshGodEquipForm(this.m_reqRankHeroId);
		}

		private void RefreshGodEquipForm(uint heroId)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CEquipSystem.s_GodEquipPath);
			if (null == form)
			{
				return;
			}
			Transform transform = form.transform.Find("Panel/godEquipPanel/godEquipList");
			if (null == transform)
			{
				return;
			}
			int num = 0;
			CEquipSystem.stEquipRankInfo stEquipRankInfo;
			if (this.m_equipRankItemDetail.TryGetValue(heroId, ref stEquipRankInfo))
			{
				num = stEquipRankInfo.equipRankItemCnt;
			}
			CUIListScript component = transform.GetComponent<CUIListScript>();
			if (component != null)
			{
				component.SetElementAmount(num);
			}
			if (num == 0)
			{
				Transform transform2 = form.transform.Find("Panel/godEquipPanel/info_node");
				if (transform2 != null)
				{
					transform2.gameObject.CustomSetActive(true);
				}
			}
			GameObject widget = form.GetWidget(2);
			if (widget == null)
			{
				return;
			}
			Transform transform3 = widget.transform;
			if (transform3 == null)
			{
				return;
			}
			Transform transform4 = transform3.Find("godEquipList");
			if (transform4 == null)
			{
				return;
			}
			component = transform4.GetComponent<CUIListScript>();
			if (component != null && component.GetElementAmount() == 0)
			{
				ResRecommendEquipInBattle[] array = new ResRecommendEquipInBattle[3];
				int num2 = 0;
				for (uint num3 = 1u; num3 < 4u; num3 += 1u)
				{
					long doubleKey = GameDataMgr.GetDoubleKey(this.m_reqRankHeroId, num3);
					if (this.m_defaultRecommendEquipsDictionary.TryGetValue(doubleKey, ref array[(int)((UIntPtr)(num3 - 1u))]))
					{
						num2++;
					}
				}
				component.SetElementAmount(num2);
				int num4 = 0;
				for (int i = 0; i < 3; i++)
				{
					if (array[i] != null)
					{
						CUIListElementScript elemenet = component.GetElemenet(num4);
						if (elemenet != null)
						{
							Transform transform5 = elemenet.gameObject.transform;
							Transform transform6 = transform5.Find("TitleText");
							Transform transform7 = transform5.Find("tipsText");
							Transform transform8 = transform5.Find("useButton");
							if (transform8 != null)
							{
								CUIEventScript component2 = transform8.GetComponent<CUIEventScript>();
								if (component2 != null)
								{
									component2.SetUIEvent(enUIEventType.Click, enUIEventID.CustomEquip_GodEquipSysUseBtnClick, new stUIEventParams
									{
										tag = i + 1
									});
								}
							}
							CUICommonSystem.SetTextContent(transform6.gameObject, array[i].szCombinationName);
							CUICommonSystem.SetTextContent(transform7.gameObject, array[i].szCombinationDesc);
							for (int j = 0; j < 6; j++)
							{
								Transform transform9 = transform5.Find("equipItem" + j);
								if (transform9 != null)
								{
									Transform transform10 = transform9.Find("imgIcon");
									if (j >= array[i].RecommendEquipID.Length)
									{
										transform10.gameObject.CustomSetActive(false);
									}
									else
									{
										ushort equipID = array[i].RecommendEquipID[j];
										CEquipInfo equipInfo = CEquipSystem.GetEquipInfo(equipID);
										if (equipInfo != null)
										{
											transform10.gameObject.CustomSetActive(true);
											transform10.GetComponent<Image>().SetSprite(equipInfo.m_equipIconPath, form, true, false, false, false);
											string text = Singleton<CTextManager>.GetInstance().GetText("Equip_CommonTipsFormat", new string[]
											{
												equipInfo.m_equipName,
												equipInfo.m_equipPropertyDesc
											});
											CUICommonSystem.SetCommonTipsEvent(form, transform10.gameObject, text, enUseableTipsPos.enTop);
										}
										else
										{
											transform10.gameObject.CustomSetActive(false);
										}
									}
								}
							}
							num4++;
						}
					}
				}
			}
		}

		private void OnGodEquipItemEnable(CUIEvent uiEvent)
		{
			CEquipSystem.stEquipRankInfo stEquipRankInfo;
			if (!this.m_equipRankItemDetail.TryGetValue(this.m_reqRankHeroId, ref stEquipRankInfo))
			{
				return;
			}
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			if (srcWidgetIndexInBelongedList < 0 || srcWidgetIndexInBelongedList >= stEquipRankInfo.equipRankItemCnt || stEquipRankInfo.rankDetail == null)
			{
				return;
			}
			CSDT_RANKING_LIST_ITEM_INFO cSDT_RANKING_LIST_ITEM_INFO = stEquipRankInfo.rankDetail[srcWidgetIndexInBelongedList];
			if (cSDT_RANKING_LIST_ITEM_INFO == null || null == uiEvent.m_srcWidget)
			{
				return;
			}
			Transform transform = uiEvent.m_srcWidget.transform;
			Transform transform2 = transform.Find("Bg");
			Transform transform3 = transform.Find("BgNo1");
			Transform transform4 = transform.Find("123No");
			Transform transform5 = transform.Find("NumText");
			Transform transform6 = transform.Find("winCntText");
			Transform transform7 = transform.Find("playerJudgeText");
			Transform transform8 = transform.Find("judgeMarkText");
			transform3.gameObject.CustomSetActive(false);
			transform2.gameObject.CustomSetActive(srcWidgetIndexInBelongedList >= 0);
			transform4.GetChild(0).gameObject.CustomSetActive(0 == srcWidgetIndexInBelongedList);
			transform4.GetChild(1).gameObject.CustomSetActive(1 == srcWidgetIndexInBelongedList);
			transform4.GetChild(2).gameObject.CustomSetActive(2 == srcWidgetIndexInBelongedList);
			transform5.gameObject.CustomSetActive(srcWidgetIndexInBelongedList > 2);
			transform5.GetComponent<Text>().text = cSDT_RANKING_LIST_ITEM_INFO.dwRankNo.ToString();
			uint num = (cSDT_RANKING_LIST_ITEM_INFO.dwRankScore <= 99999999u) ? cSDT_RANKING_LIST_ITEM_INFO.dwRankScore : 99999999u;
			float num2 = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().dwWinRate / 100f;
			float num3 = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().dwEvalScore / 100f;
			byte[] szEvalID = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().szEvalID;
			int bEvalNum = (int)cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().bEvalNum;
			string text = string.Empty;
			for (int i = 0; i < bEvalNum; i++)
			{
				ResEquipEval dataByKey = GameDataMgr.m_recommendEquipJudge.GetDataByKey((uint)szEvalID[i]);
				text = text + dataByKey.szDesc + " ";
			}
			transform6.GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("CustomEquip_WinCntText", new string[]
			{
				num.ToString(),
				num2 + "%",
				num3.ToString()
			});
			transform7.GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("JudgeEquip_PlayerJudge", new string[]
			{
				text
			});
			transform8.GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("JudgeEquip_PlayerJudgeMark", new string[]
			{
				string.Empty,
				num3.ToString()
			});
			int j;
			for (j = 0; j < (int)cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().bEquipNum; j++)
			{
				ushort equipID = (ushort)cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().EquipID[j];
				Transform transform9 = transform.Find("equipItem" + j);
				if (transform9 != null)
				{
					Transform transform10 = transform9.Find("imgIcon");
					CEquipInfo equipInfo = CEquipSystem.GetEquipInfo(equipID);
					if (equipInfo != null)
					{
						transform10.gameObject.CustomSetActive(true);
						transform10.GetComponent<Image>().SetSprite(equipInfo.m_equipIconPath, this.m_customEquipForm, true, false, false, false);
						string text2 = Singleton<CTextManager>.GetInstance().GetText("Equip_CommonTipsFormat", new string[]
						{
							equipInfo.m_equipName,
							equipInfo.m_equipPropertyDesc
						});
						CUICommonSystem.SetCommonTipsEvent(this.m_customEquipForm, transform10.gameObject, text2, enUseableTipsPos.enTop);
					}
					else
					{
						transform10.gameObject.CustomSetActive(false);
					}
				}
			}
			while (j < 6)
			{
				Transform transform11 = transform.Find("equipItem" + j);
				if (transform11 != null)
				{
					Transform transform12 = transform11.Find("imgIcon");
					transform12.gameObject.CustomSetActive(false);
				}
				j++;
			}
			Transform transform13 = transform.Find("useButton");
			Transform transform14 = transform.Find("judgeButton");
			if (transform13 != null)
			{
				CUIEventScript component = transform13.GetComponent<CUIEventScript>();
				if (component != null)
				{
					component.SetUIEvent(enUIEventType.Click, enUIEventID.CustomEquip_GodEquipUseBtnClick, new stUIEventParams
					{
						tag = srcWidgetIndexInBelongedList
					});
				}
			}
			if (transform14 != null)
			{
				CUIEventScript component2 = transform14.GetComponent<CUIEventScript>();
				if (component2 != null)
				{
					component2.SetUIEvent(enUIEventType.Click, enUIEventID.CustomEquip_IWantJudgeBtnClick, new stUIEventParams
					{
						tag = srcWidgetIndexInBelongedList,
						heroId = this.m_reqRankHeroId
					});
				}
			}
		}

		private void OnGodEquipUseBtnClick(CUIEvent uiEvent)
		{
			CEquipSystem.stEquipRankInfo stEquipRankInfo;
			if (!this.m_equipRankItemDetail.TryGetValue(this.m_reqRankHeroId, ref stEquipRankInfo))
			{
				return;
			}
			if (uiEvent.m_eventParams.tag >= 0 && uiEvent.m_eventParams.tag < stEquipRankInfo.equipRankItemCnt && stEquipRankInfo.rankDetail != null)
			{
				CSDT_RANKING_LIST_ITEM_INFO cSDT_RANKING_LIST_ITEM_INFO = stEquipRankInfo.rankDetail[uiEvent.m_eventParams.tag];
				this.EditCustomRecommendEquipByGodEquip((int)cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().bEquipNum, ref cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().EquipID);
				this.m_useGodEquip = true;
				this.SaveEditCustomRecommendEquip();
				Singleton<CUIManager>.GetInstance().CloseForm(CEquipSystem.s_GodEquipPath);
			}
		}

		private void OnGodEquipSysUseBtnClick(CUIEvent uiEvent)
		{
			this.m_defaultCombinationID = (uint)uiEvent.m_eventParams.tag;
			ResRecommendEquipInBattle defaultRecommendEquipInfo = this.GetDefaultRecommendEquipInfo(this.m_editHeroID, this.m_defaultCombinationID);
			if (defaultRecommendEquipInfo != null)
			{
				for (int i = 0; i < 6; i++)
				{
					this.m_editCustomRecommendEquips[i] = defaultRecommendEquipInfo.RecommendEquipID[i];
				}
			}
			this.SaveEditCustomRecommendEquip();
			Singleton<CUIManager>.GetInstance().CloseForm(CEquipSystem.s_GodEquipPath);
		}

		private void OnIWantJudgeBtnClick(CUIEvent uiEvent)
		{
			CEquipSystem.m_judgeGodeIndex = uiEvent.m_eventParams.tag;
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5329u);
			CEquipSystem.stEquipRankInfo stEquipRankInfo;
			if (CEquipSystem.m_judgeGodeIndex >= 0 && this.m_equipRankItemDetail.TryGetValue(this.m_reqRankHeroId, ref stEquipRankInfo))
			{
				CSDT_RANKING_LIST_ITEM_INFO cSDT_RANKING_LIST_ITEM_INFO = stEquipRankInfo.rankDetail[CEquipSystem.m_judgeGodeIndex];
				cSPkg.stPkgData.get_stGetEquipEvalReq().dwHeroId = this.m_reqRankHeroId;
				uint[] equipID = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().EquipID;
				for (int i = 0; i < (int)cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().bEquipNum; i++)
				{
					cSPkg.stPkgData.get_stGetEquipEvalReq().HeroEquipList[i] = equipID[i];
				}
			}
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(5330)]
		public static void RecieveSCGetEquipEvalRsp(CSPkg csPkg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			CSDT_HERO_EQUIPEVAL stHeroEquipEval = csPkg.stPkgData.get_stGetEquipEvalRsp().stHeroEquipEval;
			COMDT_EQUIPEVAL_PERACNT stEvalInfo = stHeroEquipEval.stEvalInfo;
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CEquipSystem.s_EquipJudgePath, false, true);
			GameObject widget = cUIFormScript.GetWidget(0);
			GameObject widget2 = cUIFormScript.GetWidget(3);
			GameObject widget3 = cUIFormScript.GetWidget(4);
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CEquipSystem.s_GodEquipPath);
			if (form != null)
			{
				try
				{
					GameObject widget4 = form.GetWidget(1);
					Transform transform = widget4.transform.Find("godEquipList/ScrollRect/Content/ListElement_" + CEquipSystem.m_judgeGodeIndex).transform;
					Transform transform2 = transform.Find("judgeMarkText");
					Transform transform3 = transform.Find("playerJudgeText");
					widget2.GetComponent<Text>().text = transform2.GetComponent<Text>().text;
					widget3.GetComponent<Text>().text = transform3.GetComponent<Text>().text;
				}
				catch (Exception var_11_EE)
				{
				}
			}
			if (widget != null)
			{
				for (int i = 0; i < 6; i++)
				{
					Transform transform4 = widget.transform.Find("equipItem" + i);
					if (transform4 != null)
					{
						Transform transform5 = transform4.Find("imgIcon");
						if (stHeroEquipEval.HeroEquipList[i] == 0u)
						{
							transform5.gameObject.CustomSetActive(false);
						}
						else
						{
							ushort equipID = (ushort)stHeroEquipEval.HeroEquipList[i];
							CEquipInfo equipInfo = CEquipSystem.GetEquipInfo(equipID);
							if (equipInfo != null)
							{
								transform5.gameObject.CustomSetActive(true);
								transform5.GetComponent<Image>().SetSprite(equipInfo.m_equipIconPath, cUIFormScript, true, false, false, false);
								string text = Singleton<CTextManager>.GetInstance().GetText("Equip_CommonTipsFormat", new string[]
								{
									equipInfo.m_equipName,
									equipInfo.m_equipPropertyDesc
								});
								CUICommonSystem.SetCommonTipsEvent(cUIFormScript, transform5.gameObject, text, enUseableTipsPos.enTop);
							}
							else
							{
								transform5.gameObject.CustomSetActive(false);
							}
						}
					}
				}
			}
			GameObject widget5 = cUIFormScript.GetWidget(1);
			if (widget5 != null)
			{
				CUIToggleListScript component = widget5.transform.GetComponent<CUIToggleListScript>();
				int num = Math.Min(GameDataMgr.m_recommendEquipJudge.Count(), 12);
				component.SetElementAmount(num);
				for (int j = 0; j < num; j++)
				{
					CUIToggleListElementScript cUIToggleListElementScript = (CUIToggleListElementScript)component.GetElemenet(j);
					ResEquipEval dataByIndex = GameDataMgr.m_recommendEquipJudge.GetDataByIndex(j);
					if (cUIToggleListElementScript != null)
					{
						Transform transform6 = cUIToggleListElementScript.gameObject.transform;
						Transform transform7 = transform6.Find("JudgeNameTxt");
						CUICommonSystem.SetTextContent(transform7.gameObject, dataByIndex.szDesc);
					}
					for (int k = 0; k < (int)stEvalInfo.bEvalNum; k++)
					{
						if (dataByIndex.bEvalID == stEvalInfo.szEvalID[k])
						{
							component.SetMultiSelected(j, true);
							break;
						}
					}
				}
			}
			GameObject widget6 = cUIFormScript.GetWidget(2);
			CUIListScript component2 = widget6.GetComponent<CUIListScript>();
			component2.SetElementAmount(5);
			for (int l = 0; l < 5; l++)
			{
				Text component3 = component2.GetElemenet(l).transform.Find("Text").GetComponent<Text>();
				component3.text = Singleton<CTextManager>.GetInstance().GetText("CustomEquip_JudgeMarks", new string[]
				{
					(l + 1).ToString()
				});
			}
			component2.SelectElement((int)(stHeroEquipEval.stEvalInfo.dwScore - 1u), true);
		}

		private void OnJudgeRuleBtnClick(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().OpenInfoForm(20);
		}

		private void OnJudgeSubmitBtnClick(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CEquipSystem.s_EquipJudgePath);
			if (form == null)
			{
				return;
			}
			GameObject widget = form.GetWidget(1);
			CUIToggleListScript component = widget.GetComponent<CUIToggleListScript>();
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5327u);
			bool[] multiSelected = component.GetMultiSelected();
			int num = 0;
			for (int i = 0; i < multiSelected.Length; i++)
			{
				if (multiSelected[i])
				{
					num++;
				}
			}
			uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(291u).dwConfValue;
			if ((long)num > (long)((ulong)dwConfValue))
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("CustomEquip_JudgeTooMany", new string[]
				{
					"1",
					5.ToString()
				}), false, 1.5f, null, new object[0]);
				return;
			}
			CEquipSystem.stEquipRankInfo stEquipRankInfo;
			bool flag = this.m_equipRankItemDetail.TryGetValue(this.m_reqRankHeroId, ref stEquipRankInfo);
			if (flag)
			{
				CSDT_RANKING_LIST_ITEM_INFO cSDT_RANKING_LIST_ITEM_INFO = stEquipRankInfo.rankDetail[CEquipSystem.m_judgeGodeIndex];
				uint[] equipID = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().EquipID;
				for (int j = 0; j < (int)cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stCustomEquip().bEquipNum; j++)
				{
					cSPkg.stPkgData.get_stEquipEvalReq().stHeroEquipEval.HeroEquipList[j] = equipID[j];
				}
			}
			cSPkg.stPkgData.get_stEquipEvalReq().stHeroEquipEval.dwHeroId = this.m_editHeroID;
			GameObject widget2 = form.GetWidget(2);
			CUIListScript component2 = widget2.GetComponent<CUIListScript>();
			if (component2.GetSelectedIndex() < 0)
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("CustomEquip_JudgeNoMarks"), false, 1.5f, null, new object[0]);
				return;
			}
			int num2 = 0;
			cSPkg.stPkgData.get_stEquipEvalReq().stHeroEquipEval.stEvalInfo.bEvalNum = (byte)num;
			for (int k = 0; k < multiSelected.Length; k++)
			{
				ResEquipEval dataByIndex = GameDataMgr.m_recommendEquipJudge.GetDataByIndex(k);
				if (multiSelected[k])
				{
					cSPkg.stPkgData.get_stEquipEvalReq().stHeroEquipEval.stEvalInfo.szEvalID[num2++] = dataByIndex.bEvalID;
				}
			}
			cSPkg.stPkgData.get_stEquipEvalReq().stHeroEquipEval.stEvalInfo.dwScore = (uint)(component2.GetSelectedIndex() + 1);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(5328)]
		public static void RecieveSCEquipEvalRsp(CSPkg csPkg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CEquipSystem.s_EquipJudgePath);
			if (form != null)
			{
				form.Close();
			}
			if (csPkg.stPkgData.get_stEquipEvalRsp().bResult == 0)
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Appoint_Or_Leader_Success"), false, 1.5f, null, new object[0]);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Utility.ProtErrCodeToStr(5328, (int)csPkg.stPkgData.get_stEquipEvalRsp().bResult), false, 1.5f, null, new object[0]);
			}
		}

		public void OpenTipsOnSvrRsp()
		{
			if (this.m_useGodEquip)
			{
				this.m_useGodEquip = false;
				Singleton<CUIManager>.GetInstance().OpenTips("CustomEquip_UseGodEquipTip", true, 1.5f, null, new object[0]);
			}
			else if (this.m_revertDefaultEquip)
			{
				this.m_revertDefaultEquip = false;
				Singleton<CUIManager>.GetInstance().OpenTips("CustomEquip_RevertDefaultTip", true, 1.5f, null, new object[0]);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips("EquipChange_Suc", true, 1.5f, null, new object[0]);
			}
		}
	}
}
