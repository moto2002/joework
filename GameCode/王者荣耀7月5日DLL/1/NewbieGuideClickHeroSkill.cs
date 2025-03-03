using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using System;
using UnityEngine;

public class NewbieGuideClickHeroSkill : NewbieGuideBaseScript
{
	protected override void Initialize()
	{
	}

	protected override void Update()
	{
		if (base.isInitialize)
		{
			base.Update();
			return;
		}
		CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
		if (form != null)
		{
			if (form.gameObject.transform.Find("PanelLeft/ListHostHeroInfoFull").gameObject.activeSelf)
			{
				MonoSingleton<NewbieGuideManager>.GetInstance().StopCurrentGuide();
				return;
			}
			GameObject gameObject = form.transform.FindChild("Other/SkillList").gameObject;
			if (gameObject != null)
			{
				CUIListScript component = gameObject.GetComponent<CUIListScript>();
				GameObject gameObject2 = component.GetElemenet(0).transform.FindChild("heroSkillItemCell").gameObject;
				if (gameObject2 != null)
				{
					base.AddHighLightGameObject(gameObject2, true, form, true);
					Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_Skill_Down, new CUIEventManager.OnUIEventHandler(this.onDownHandler));
					Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_Skill_Up, new CUIEventManager.OnUIEventHandler(this.onUpHandler));
					base.Initialize();
				}
			}
		}
	}

	private void onDownHandler(CUIEvent evt)
	{
		base.HideHighlighterAndText();
	}

	private void onUpHandler(CUIEvent evt)
	{
		this.ClickHandler(evt);
	}

	protected override bool IsDelegateClickEvent()
	{
		return true;
	}

	protected override void Clear()
	{
		Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.HeroSelect_Skill_Down, new CUIEventManager.OnUIEventHandler(this.onDownHandler));
		Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.HeroSelect_Skill_Up, new CUIEventManager.OnUIEventHandler(this.onUpHandler));
		base.Clear();
	}
}
