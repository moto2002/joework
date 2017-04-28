using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using System;

namespace Assets.Scripts.GameLogic
{
	public class BaseAttackMode : LogicComponent
	{
		protected uint commonAttackEnemyHeroTargetID;

		protected CommonAttackButtonType commonAttackButtonType = CommonAttackButtonType.CommonAttackButton;

		protected bool TargetType(uint selectID, ActorTypeDef ActorType)
		{
			bool result = false;
			if (selectID > 0u)
			{
				PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.GetInstance().GetActor(selectID);
				result = (actor && actor.get_handle().TheActorMeta.ActorType == ActorType);
			}
			return result;
		}

		protected bool IsValidTargetID(uint selectID)
		{
			bool flag = false;
			if (selectID <= 0u)
			{
				return flag;
			}
			PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.GetInstance().GetActor(selectID);
			flag = (actor && !actor.get_handle().ObjLinker.Invincible && !actor.get_handle().ActorControl.IsDeadState && !this.actor.IsSelfCamp(actor) && actor.get_handle().HorizonMarker.IsVisibleFor(this.actor.TheActorMeta.ActorCamp) && actor.get_handle().AttackOrderReady);
			if (!flag)
			{
				return flag;
			}
			Skill nextSkill = this.actor.ActorControl.GetNextSkill(SkillSlotType.SLOT_SKILL_0);
			if (nextSkill != null)
			{
				long num = (long)nextSkill.GetMaxSearchDistance(0);
				if (!actor || actor.get_handle().shape == null || actor.get_handle().ActorAgent == null || nextSkill.cfgData == null)
				{
					return false;
				}
				num += (long)actor.get_handle().shape.AvgCollisionRadius;
				num *= num;
				if ((this.actor.ActorControl.actorLocation - actor.get_handle().location).get_sqrMagnitudeLong2D() > num)
				{
					return false;
				}
			}
			return flag;
		}

		public virtual uint CommonAttackSearchEnemy(int srchR)
		{
			return 0u;
		}

		public virtual uint SelectSkillTarget(SkillSlot _slot)
		{
			return 0u;
		}

		public virtual VInt3 SelectSkillDirection(SkillSlot _slot)
		{
			return VInt3.one;
		}

		public virtual bool SelectSkillPos(SkillSlot _slot, out VInt3 _position)
		{
			_position = VInt3.zero;
			return false;
		}

		public virtual bool CancelCommonAttackMode()
		{
			return false;
		}

		public virtual void OnDead()
		{
		}

		public void SetEnemyHeroAttackTargetID(uint uiTargetId)
		{
			this.commonAttackEnemyHeroTargetID = uiTargetId;
		}

		public uint GetEnemyHeroAttackTargetID()
		{
			return this.commonAttackEnemyHeroTargetID;
		}

		public void SetCommonButtonType(sbyte type)
		{
			this.commonAttackButtonType = (CommonAttackButtonType)type;
		}

		protected virtual uint NormalModeCommonAttackSearchTarget(int srchR, SelectEnemyType type, ref bool bSearched)
		{
			return 0u;
		}

		protected virtual uint LastHitModeCommonAttackSearchTarget(int srchR, SelectEnemyType type, ref bool bSearched)
		{
			return 0u;
		}

		protected virtual uint LastHitAttackSearchTarget(int srchR, SelectEnemyType type, ref bool bSearched)
		{
			return 0u;
		}

		protected uint ExecuteSearchTraget(int srchR, ref bool bSearched)
		{
			Player ownerPlayer = ActorHelper.GetOwnerPlayer(ref this.actorPtr);
			SelectEnemyType type = SelectEnemyType.SelectLowHp;
			LastHitMode lastHitMode = LastHitMode.None;
			if (ownerPlayer != null)
			{
				type = ownerPlayer.AttackTargetMode;
				lastHitMode = ownerPlayer.useLastHitMode;
			}
			uint result;
			if (lastHitMode == LastHitMode.None)
			{
				result = this.NormalModeCommonAttackSearchTarget(srchR, type, ref bSearched);
			}
			else if (this.commonAttackButtonType == CommonAttackButtonType.CommonAttackButton)
			{
				result = this.LastHitModeCommonAttackSearchTarget(srchR, type, ref bSearched);
			}
			else
			{
				result = this.LastHitAttackSearchTarget(srchR, type, ref bSearched);
			}
			return result;
		}
	}
}
