using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	public class SkillSelectControl : Singleton<SkillSelectControl>
	{
		private DictionaryView<uint, SkillBaseSelectTarget> registedRule = new DictionaryView<uint, SkillBaseSelectTarget>();

		private PoolObjHandle<ActorRoot> m_SkillTargetObj;

		public bool IsLowerHpMode()
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			return hostPlayer == null || hostPlayer.AttackTargetMode == SelectEnemyType.SelectLowHp;
		}

		public override void Init()
		{
			ClassEnumerator classEnumerator = new ClassEnumerator(typeof(SkillBaseSelectTargetAttribute), typeof(SkillBaseSelectTarget), typeof(SkillBaseSelectTargetAttribute).get_Assembly(), true, false, false);
			using (ListView<Type>.Enumerator enumerator = classEnumerator.get_results().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type current = enumerator.get_Current();
					SkillBaseSelectTarget skillBaseSelectTarget = (SkillBaseSelectTarget)Activator.CreateInstance(current);
					Attribute customAttribute = Attribute.GetCustomAttribute(current, typeof(SkillBaseSelectTargetAttribute));
					this.registedRule.Add((customAttribute as SkillBaseSelectTargetAttribute).TargetRule, skillBaseSelectTarget);
				}
			}
		}

		public ActorRoot SelectTarget(SkillTargetRule ruleType, SkillSlot slot)
		{
			if (this.m_SkillTargetObj)
			{
				return this.m_SkillTargetObj;
			}
			SkillBaseSelectTarget skillBaseSelectTarget;
			if (this.registedRule.TryGetValue(ruleType, ref skillBaseSelectTarget))
			{
				return skillBaseSelectTarget.SelectTarget(slot);
			}
			return null;
		}

		public VInt3 SelectTargetDir(SkillTargetRule ruleType, SkillSlot slot)
		{
			SkillBaseSelectTarget skillBaseSelectTarget;
			if (this.registedRule.TryGetValue(ruleType, ref skillBaseSelectTarget))
			{
				return skillBaseSelectTarget.SelectTargetDir(slot);
			}
			return slot.Actor.get_handle().forward;
		}

		public VInt3 SelectTargetPos(SkillTargetRule ruleType, SkillSlot slot, out bool bTarget)
		{
			bTarget = false;
			SkillBaseSelectTarget skillBaseSelectTarget;
			if (!this.registedRule.TryGetValue(ruleType, ref skillBaseSelectTarget))
			{
				return slot.Actor.get_handle().location;
			}
			ActorRoot actorRoot = skillBaseSelectTarget.SelectTarget(slot);
			if (actorRoot != null)
			{
				bTarget = true;
				return actorRoot.location;
			}
			return slot.Actor.get_handle().location;
		}
	}
}
