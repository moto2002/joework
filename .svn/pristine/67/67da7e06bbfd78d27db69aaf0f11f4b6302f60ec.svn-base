using Assets.Scripts.Common;
using Assets.Scripts.GameLogic;
using System;

namespace Assets.Scripts.GameSystem
{
	public class CBattleSelectTarget : Singleton<CBattleSelectTarget>
	{
		private int curMaxHp;

		private int curAd;

		private int curAp;

		private int curPhyDef;

		private int curMgDef;

		private bool IsOpenForm;

		public void OpenForm(PoolObjHandle<ActorRoot> _target)
		{
			if (!_target)
			{
				return;
			}
			this.curMaxHp = _target.get_handle().ValueComponent.mActorValue[5].totalValue;
			this.curAd = _target.get_handle().ValueComponent.mActorValue[1].totalValue;
			this.curAp = _target.get_handle().ValueComponent.mActorValue[2].totalValue;
			this.curPhyDef = _target.get_handle().ValueComponent.mActorValue[3].totalValue;
			this.curMgDef = _target.get_handle().ValueComponent.mActorValue[4].totalValue;
			this.IsOpenForm = true;
			Singleton<CBattleSystem>.GetInstance().FightForm.UpdateHpInfo();
			Singleton<CBattleSystem>.GetInstance().FightForm.UpdateEpInfo();
			Singleton<CBattleSystem>.GetInstance().FightForm.UpdateAdValueInfo();
			Singleton<CBattleSystem>.GetInstance().FightForm.UpdateApValueInfo();
			Singleton<CBattleSystem>.GetInstance().FightForm.UpdatePhyDefValueInfo();
			Singleton<CBattleSystem>.GetInstance().FightForm.UpdateMgcDefValueInfo();
		}

		public void CloseForm()
		{
			this.IsOpenForm = false;
			this.curMaxHp = 0;
			this.curAd = 0;
			this.curAp = 0;
			this.curPhyDef = 0;
			this.curMgDef = 0;
		}

		private bool IsChangeMaxHp(PoolObjHandle<ActorRoot> _target)
		{
			CrypticInt32 inData = _target.get_handle().ValueComponent.mActorValue[5].totalValue;
			if (this.curMaxHp != inData)
			{
				this.curMaxHp = inData;
				return true;
			}
			return false;
		}

		private bool IsChangeAd(PoolObjHandle<ActorRoot> _target)
		{
			CrypticInt32 inData = _target.get_handle().ValueComponent.mActorValue[1].totalValue;
			if (this.curAd != inData)
			{
				this.curAd = inData;
				return true;
			}
			return false;
		}

		private bool IsChangeAp(PoolObjHandle<ActorRoot> _target)
		{
			CrypticInt32 inData = _target.get_handle().ValueComponent.mActorValue[2].totalValue;
			if (this.curAp != inData)
			{
				this.curAp = inData;
				return true;
			}
			return false;
		}

		private bool IsChangePhyDef(PoolObjHandle<ActorRoot> _target)
		{
			CrypticInt32 inData = _target.get_handle().ValueComponent.mActorValue[3].totalValue;
			if (this.curPhyDef != inData)
			{
				this.curPhyDef = inData;
				return true;
			}
			return false;
		}

		private bool IsChangeMgDef(PoolObjHandle<ActorRoot> _target)
		{
			CrypticInt32 inData = _target.get_handle().ValueComponent.mActorValue[4].totalValue;
			if (this.curMgDef != inData)
			{
				this.curMgDef = inData;
				return true;
			}
			return false;
		}

		public void Update(PoolObjHandle<ActorRoot> _target)
		{
			if (!this.IsOpenForm || !_target)
			{
				return;
			}
			if (this.IsChangeMaxHp(_target))
			{
				Singleton<CBattleSystem>.GetInstance().FightForm.UpdateHpInfo();
			}
			if (this.IsChangeAd(_target))
			{
				Singleton<CBattleSystem>.GetInstance().FightForm.UpdateAdValueInfo();
			}
			if (this.IsChangeAp(_target))
			{
				Singleton<CBattleSystem>.GetInstance().FightForm.UpdateApValueInfo();
			}
			if (this.IsChangePhyDef(_target))
			{
				Singleton<CBattleSystem>.GetInstance().FightForm.UpdatePhyDefValueInfo();
			}
			if (this.IsChangeMgDef(_target))
			{
				Singleton<CBattleSystem>.GetInstance().FightForm.UpdateMgcDefValueInfo();
			}
		}
	}
}
