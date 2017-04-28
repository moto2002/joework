using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;

public class CAddSkillSys : Singleton<CAddSkillSys>
{
	public const string ADD_SKILL_FORM_PATH = "UGUI/Form/System/AddedSkill/Form_AddedSkill.prefab";

	public override void Init()
	{
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.AddedSkill_OpenForm, new CUIEventManager.OnUIEventHandler(this.OnOpenForm));
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.AddedSkill_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseForm));
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.AddedSkill_GetDetail, new CUIEventManager.OnUIEventHandler(this.OnGetDetail));
		base.Init();
	}

	public override void UnInit()
	{
		Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.AddedSkill_OpenForm, new CUIEventManager.OnUIEventHandler(this.OnOpenForm));
		Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.AddedSkill_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseForm));
		Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.AddedSkill_GetDetail, new CUIEventManager.OnUIEventHandler(this.OnGetDetail));
		base.UnInit();
	}

	private void OnOpenForm(CUIEvent cuiEvent)
	{
		if (Singleton<CFunctionUnlockSys>.get_instance().FucIsUnlock(22))
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.get_instance().OpenForm("UGUI/Form/System/AddedSkill/Form_AddedSkill.prefab", false, true);
			if (cUIFormScript != null)
			{
				CAddSkillView.OpenForm(cUIFormScript.gameObject);
			}
		}
		else
		{
			ResSpecialFucUnlock dataByKey = GameDataMgr.specialFunUnlockDatabin.GetDataByKey(22u);
			Singleton<CUIManager>.get_instance().OpenTips(Utility.UTF8Convert(dataByKey.szLockedTip), false, 1.5f, null, new object[0]);
		}
		CMiShuSystem.SendUIClickToServer(enUIClickReprotID.rp_AddSkillBtn);
	}

	private void OnCloseForm(CUIEvent cuiEvent)
	{
		Singleton<CUIManager>.get_instance().CloseForm("UGUI/Form/System/AddedSkill/Form_AddedSkill.prefab");
		Singleton<CResourceManager>.get_instance().UnloadUnusedAssets();
	}

	private void OnGetDetail(CUIEvent cuiEvent)
	{
		CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm("UGUI/Form/System/AddedSkill/Form_AddedSkill.prefab");
		if (form != null && !form.IsHided())
		{
			CAddSkillView.OnRefresh(form.gameObject, (ushort)cuiEvent.m_eventParams.tag);
		}
	}

	public static bool IsSelSkillAvailable()
	{
		CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
		return Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode() && Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(22) && masterRoleInfo != null;
	}

	public static ListView<ResSkillUnlock> GetSelSkillAvailable(ResDT_UnUseSkill unUseSkillInfo)
	{
		ListView<ResSkillUnlock> listView = new ListView<ResSkillUnlock>();
		CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
		if (unUseSkillInfo == null)
		{
			return listView;
		}
		for (int i = 0; i < GameDataMgr.addedSkiilDatabin.count; i++)
		{
			ResSkillUnlock dataByIndex = GameDataMgr.addedSkiilDatabin.GetDataByIndex(i);
			if (masterRoleInfo != null && masterRoleInfo.PvpLevel >= (uint)dataByIndex.wAcntLevel)
			{
				bool flag = true;
				if (unUseSkillInfo != null)
				{
					for (int j = 0; j < unUseSkillInfo.UnUseSkillList.Length; j++)
					{
						if (unUseSkillInfo.UnUseSkillList[j] == dataByIndex.dwUnlockSkillID)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					listView.Add(dataByIndex);
				}
			}
		}
		return listView;
	}

	public static bool IsSelSkillAvailable(ResDT_UnUseSkill unUseSkillInfo, uint selSkillId)
	{
		CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
		ResSkillUnlock resSkillUnlock = null;
		if (unUseSkillInfo == null)
		{
			return false;
		}
		for (int i = 0; i < GameDataMgr.addedSkiilDatabin.count; i++)
		{
			ResSkillUnlock dataByIndex = GameDataMgr.addedSkiilDatabin.GetDataByIndex(i);
			if (dataByIndex.dwUnlockSkillID == selSkillId)
			{
				resSkillUnlock = dataByIndex;
				break;
			}
		}
		if (resSkillUnlock != null && masterRoleInfo != null && masterRoleInfo.PvpLevel >= (uint)resSkillUnlock.wAcntLevel)
		{
			if (unUseSkillInfo != null)
			{
				for (int j = 0; j < unUseSkillInfo.UnUseSkillList.Length; j++)
				{
					if (unUseSkillInfo.UnUseSkillList[j] == resSkillUnlock.dwUnlockSkillID)
					{
						return false;
					}
				}
			}
			return true;
		}
		return false;
	}

	public static uint GetSelfSelSkill(ResDT_UnUseSkill unUseSkillInfo, uint heroId)
	{
		if (!CAddSkillSys.IsSelSkillAvailable())
		{
			return 0u;
		}
		if (unUseSkillInfo == null)
		{
			return 0u;
		}
		CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
		if (masterRoleInfo == null)
		{
			return 0u;
		}
		uint num = 0u;
		CHeroInfo heroInfo = masterRoleInfo.GetHeroInfo(heroId, true);
		if (heroInfo != null)
		{
			num = heroInfo.skillInfo.SelSkillID;
		}
		else if (masterRoleInfo.IsFreeHero(heroId))
		{
			COMDT_FREEHERO_INFO freeHeroSymbol = masterRoleInfo.GetFreeHeroSymbol(heroId);
			if (freeHeroSymbol != null)
			{
				num = freeHeroSymbol.dwSkillID;
			}
		}
		if (!CAddSkillSys.IsSelSkillAvailable(unUseSkillInfo, num))
		{
			num = GameDataMgr.addedSkiilDatabin.GetAnyData().dwUnlockSkillID;
		}
		if (!CAddSkillSys.IsSelSkillAvailable(unUseSkillInfo, num))
		{
			num = GameDataMgr.globalInfoDatabin.GetDataByKey(154u).dwConfValue;
		}
		return num;
	}
}
