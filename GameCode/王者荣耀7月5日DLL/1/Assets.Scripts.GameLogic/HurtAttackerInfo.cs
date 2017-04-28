using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.GameLogic
{
	public struct HurtAttackerInfo
	{
		public int iActorLvl;

		public int iActorATT;

		public int iActorINT;

		public int iActorMaxHp;

		public int iDEFStrike;

		public int iRESStrike;

		public int iDEFStrikeRate;

		public int iRESStrikeRate;

		public int iFinalHurt;

		public int iCritStrikeRate;

		public int iCritStrikeValue;

		public int iReduceCritStrikeRate;

		public int iReduceCritStrikeValue;

		public int iCritStrikeEff;

		public int iPhysicsHemophagiaRate;

		public int iMagicHemophagiaRate;

		public int iPhysicsHemophagia;

		public int iMagicHemophagia;

		public int iHurtOutputRate;

		public ActorTypeDef actorType;

		public void Init(PoolObjHandle<ActorRoot> _atker, PoolObjHandle<ActorRoot> _target)
		{
			if (_atker)
			{
				this.iActorLvl = _atker.get_handle().ValueComponent.mActorValue.actorLvl;
				this.iActorATT = _atker.get_handle().ValueComponent.mActorValue[1].totalValue;
				this.iActorINT = _atker.get_handle().ValueComponent.mActorValue[2].totalValue;
				this.iActorMaxHp = _atker.get_handle().ValueComponent.mActorValue[5].totalValue;
				this.iDEFStrike = _atker.get_handle().ValueComponent.mActorValue[7].totalValue;
				this.iRESStrike = _atker.get_handle().ValueComponent.mActorValue[8].totalValue;
				this.iDEFStrikeRate = _atker.get_handle().ValueComponent.mActorValue[34].totalValue;
				this.iRESStrikeRate = _atker.get_handle().ValueComponent.mActorValue[35].totalValue;
				this.iFinalHurt = _atker.get_handle().ValueComponent.mActorValue[13].totalValue;
				this.iCritStrikeRate = _atker.get_handle().ValueComponent.mActorValue[6].totalValue;
				this.iCritStrikeValue = _atker.get_handle().ValueComponent.mActorValue[24].totalValue;
				this.iCritStrikeEff = _atker.get_handle().ValueComponent.mActorValue[12].totalValue;
				this.iMagicHemophagia = _atker.get_handle().ValueComponent.mActorValue[27].totalValue;
				this.iPhysicsHemophagia = _atker.get_handle().ValueComponent.mActorValue[26].totalValue;
				this.iMagicHemophagiaRate = _atker.get_handle().ValueComponent.mActorValue[10].totalValue;
				this.iPhysicsHemophagiaRate = _atker.get_handle().ValueComponent.mActorValue[9].totalValue;
				this.iHurtOutputRate = _atker.get_handle().ValueComponent.mActorValue[31].totalValue;
				this.actorType = _atker.get_handle().TheActorMeta.ActorType;
			}
			else if (_target)
			{
				this.iReduceCritStrikeRate = _target.get_handle().ValueComponent.mActorValue[11].totalValue;
				this.iReduceCritStrikeValue = _target.get_handle().ValueComponent.mActorValue[25].totalValue;
			}
		}
	}
}
