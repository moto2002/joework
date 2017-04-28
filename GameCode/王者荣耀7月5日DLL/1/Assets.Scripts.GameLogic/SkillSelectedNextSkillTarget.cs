using System;

namespace Assets.Scripts.GameLogic
{
	[SkillBaseSelectTarget]
	public class SkillSelectedNextSkillTarget : SkillBaseSelectTarget
	{
		public override ActorRoot SelectTarget(SkillSlot UseSlot)
		{
			return this.GetNextSkillTarget(UseSlot);
		}

		public override VInt3 SelectTargetDir(SkillSlot UseSlot)
		{
			ActorRoot nextSkillTarget = this.GetNextSkillTarget(UseSlot);
			if (nextSkillTarget != null)
			{
				VInt3 vInt = nextSkillTarget.location - UseSlot.Actor.get_handle().location;
				vInt.y = 0;
				return vInt.NormalizeTo(1000);
			}
			return UseSlot.Actor.get_handle().forward;
		}

		private ActorRoot GetNextSkillTarget(SkillSlot UseSlot)
		{
			for (int i = 0; i < UseSlot.NextSkillTargetIDs.get_Count(); i++)
			{
				ActorRoot actorRoot = Singleton<GameObjMgr>.GetInstance().GetActor(UseSlot.NextSkillTargetIDs.get_Item(i));
				if (actorRoot != null)
				{
					if (((ulong)UseSlot.SkillObj.cfgData.dwSkillTargetFilter & (ulong)(1L << (int)(actorRoot.TheActorMeta.ActorType & (ActorTypeDef)31))) <= 0uL && actorRoot.HorizonMarker.IsVisibleFor(UseSlot.Actor.get_handle().TheActorMeta.ActorCamp) && UseSlot.Actor.get_handle().CanAttack(actorRoot))
					{
						if (DistanceSearchCondition.Fit(actorRoot, UseSlot.Actor.get_handle(), UseSlot.SkillObj.GetMaxSearchDistance(UseSlot.GetSkillLevel())))
						{
							return actorRoot;
						}
					}
				}
			}
			return null;
		}
	}
}
