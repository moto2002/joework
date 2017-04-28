using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using ResData;
using System;

namespace Assets.Scripts.GameLogic.Treasure
{
	internal abstract class NormalStrategy : BaseStrategy
	{
		public override bool isSupportDrop
		{
			get
			{
				return true;
			}
		}

		protected override bool hasRemain
		{
			get
			{
				return this.DropedCount < this.maxCount - 1;
			}
		}

		public override void NotifyDropEvent(PoolObjHandle<ActorRoot> actor)
		{
			DebugHelper.Assert(actor);
			if (actor.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster)
			{
				ResMonsterCfgInfo resMonsterCfgInfo = base.FindMonsterConfig(actor.get_handle().TheActorMeta.ConfigId);
				DebugHelper.Assert(resMonsterCfgInfo != null, "怪物数据档里面找不到id:{0}", new object[]
				{
					actor.get_handle().TheActorMeta.ConfigId
				});
				if (resMonsterCfgInfo != null)
				{
					RES_DROP_PROBABILITY_TYPE iDropProbability = resMonsterCfgInfo.iDropProbability;
					if (iDropProbability == 101)
					{
						this.FinishDrop();
					}
					else if (this.hasRemain && iDropProbability != null && FrameRandom.Random(100u) <= iDropProbability)
					{
						this.PlayDrop();
					}
				}
			}
		}
	}
}
