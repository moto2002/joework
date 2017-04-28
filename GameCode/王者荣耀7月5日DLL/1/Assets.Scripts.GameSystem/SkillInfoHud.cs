using Assets.Scripts.Common;
using Assets.Scripts.GameLogic;
using Assets.Scripts.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class SkillInfoHud
	{
		public const int MAX_SKILL_HUD_COUNT = 5;

		private GameObject[] _skillItems;

		private Image[] _skillIcons;

		private GameObject[] _skillLevelRoots;

		private Text[] _skillLevels;

		private Image[] _skillCdBgs;

		private PoolObjHandle<ActorRoot> _curActor;

		public SkillInfoHud(GameObject root)
		{
			this._skillItems = new GameObject[5];
			this._skillIcons = new Image[5];
			this._skillLevelRoots = new GameObject[5];
			this._skillLevels = new Text[5];
			this._skillCdBgs = new Image[5];
			for (int i = 0; i < 5; i++)
			{
				GameObject gameObject = Utility.FindChild(root, "Skill_" + (i + 1));
				this._skillItems[i] = gameObject;
				this._skillIcons[i] = Utility.GetComponetInChild<Image>(gameObject, "Icon");
				this._skillLevelRoots[i] = Utility.FindChild(gameObject, "Level");
				this._skillLevels[i] = Utility.GetComponetInChild<Text>(gameObject, "Level/Text");
				this._skillCdBgs[i] = Utility.GetComponetInChild<Image>(gameObject, "CdBg");
			}
			this._curActor = new PoolObjHandle<ActorRoot>(null);
		}

		public void SwitchActor(ref PoolObjHandle<ActorRoot> actor)
		{
			if (actor == this._curActor)
			{
				return;
			}
			this._curActor = actor;
			SkillSlot[] skillSlotArray = actor.get_handle().SkillControl.SkillSlotArray;
			int num = 0;
			while (num < this._skillItems.Length && num + 1 < skillSlotArray.Length)
			{
				SkillSlotType skillSlotType = num + SkillSlotType.SLOT_SKILL_1;
				SkillSlot skillSlot = skillSlotArray[num + 1];
				GameObject gameObject = this._skillItems[num];
				if (gameObject)
				{
					if (skillSlot != null)
					{
						gameObject.CustomSetActive(true);
						this._skillIcons[num].SetSprite(CUIUtility.s_Sprite_Dynamic_Skill_Dir + skillSlot.SkillObj.IconName, Singleton<CBattleSystem>.GetInstance().WatchFormScript, true, false, false, false);
						this._skillLevelRoots[num].CustomSetActive(skillSlotType >= SkillSlotType.SLOT_SKILL_1 && skillSlotType <= SkillSlotType.SLOT_SKILL_3);
						this.ValidateLevel(skillSlotType);
						this.ValidateCD(skillSlotType);
					}
					else
					{
						gameObject.CustomSetActive(false);
					}
				}
				num++;
			}
		}

		public void ValidateLevel(SkillSlotType slot)
		{
			if (!this._curActor)
			{
				return;
			}
			int num = slot - SkillSlotType.SLOT_SKILL_1;
			if (num >= 0 && num < this._skillLevels.Length)
			{
				SkillSlot skillSlot = this._curActor.get_handle().SkillControl.SkillSlotArray[(int)slot];
				this._skillLevels[num].text = skillSlot.GetSkillLevel().ToString();
			}
		}

		public void ValidateCD(SkillSlotType slot)
		{
			if (!this._curActor)
			{
				return;
			}
			int num = slot - SkillSlotType.SLOT_SKILL_1;
			if (num >= 0 && num < this._skillCdBgs.Length)
			{
				SkillSlot skillSlot = this._curActor.get_handle().SkillControl.SkillSlotArray[(int)slot];
				this._skillCdBgs[num].fillAmount = (float)skillSlot.CurSkillCD / (float)skillSlot.GetSkillCDMax();
			}
		}
	}
}
