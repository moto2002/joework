using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	public class SkillDetectionControl : Singleton<SkillDetectionControl>
	{
		private DictionaryView<uint, SkillBaseDetection> _registedRule = new DictionaryView<uint, SkillBaseDetection>();

		public override void Init()
		{
			ClassEnumerator classEnumerator = new ClassEnumerator(typeof(SkillBaseDetectionAttribute), typeof(SkillBaseDetection), typeof(SkillBaseDetectionAttribute).get_Assembly(), true, false, false);
			using (ListView<Type>.Enumerator enumerator = classEnumerator.get_results().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type current = enumerator.get_Current();
					SkillBaseDetection skillBaseDetection = (SkillBaseDetection)Activator.CreateInstance(current);
					Attribute customAttribute = Attribute.GetCustomAttribute(current, typeof(SkillBaseDetectionAttribute));
					this._registedRule.Add((customAttribute as SkillBaseDetectionAttribute).UseRule, skillBaseDetection);
				}
			}
		}

		public bool Detection(SkillUseRule ruleType, SkillSlot slot)
		{
			SkillBaseDetection skillBaseDetection;
			return !this._registedRule.TryGetValue(ruleType, ref skillBaseDetection) || skillBaseDetection.Detection(slot);
		}
	}
}
