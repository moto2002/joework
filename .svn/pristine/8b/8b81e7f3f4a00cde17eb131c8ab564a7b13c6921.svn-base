using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.DataCenter;
using CSProtocol;
using ResData;
using System;
using UnityEngine;

namespace AGE
{
	[EventCategory("MMGame/Skill")]
	public class CallMonsterTick : TickEvent
	{
		[ObjectTemplate(new Type[]
		{

		})]
		public int TargetId = -1;

		public int WayPointId = -1;

		public int ConfigID;

		public ECampType CampType;

		public bool Invincible;

		public bool Moveable;

		public int LifeTime;

		public bool bCopyedHeroInfo;

		private GameObject wayPoint;

		private static readonly int MaxLevel = 6;

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			CallMonsterTick callMonsterTick = src as CallMonsterTick;
			this.TargetId = callMonsterTick.TargetId;
			this.WayPointId = callMonsterTick.WayPointId;
			this.ConfigID = callMonsterTick.ConfigID;
			this.LifeTime = callMonsterTick.LifeTime;
			this.CampType = callMonsterTick.CampType;
			this.Invincible = callMonsterTick.Invincible;
			this.Moveable = callMonsterTick.Moveable;
			this.bCopyedHeroInfo = callMonsterTick.bCopyedHeroInfo;
		}

		public override BaseEvent Clone()
		{
			CallMonsterTick callMonsterTick = ClassObjPool<CallMonsterTick>.Get();
			callMonsterTick.CopyData(this);
			return callMonsterTick;
		}

		public override void OnUse()
		{
			base.OnUse();
		}

		private COM_PLAYERCAMP SelectCamp(ref PoolObjHandle<ActorRoot> InActor)
		{
			if (this.CampType == ECampType.ECampType_Self)
			{
				return InActor.get_handle().TheActorMeta.ActorCamp;
			}
			if (this.CampType != ECampType.ECampType_Hostility)
			{
				return 0;
			}
			COM_PLAYERCAMP actorCamp = InActor.get_handle().TheActorMeta.ActorCamp;
			if (actorCamp == 1)
			{
				return 2;
			}
			if (actorCamp == 2)
			{
				return 1;
			}
			return 0;
		}

		private int GetAddValue(ref PoolObjHandle<ActorRoot> OrignalHost, ref PoolObjHandle<ActorRoot> Monster, ref ResCallMonster CallMonsterCfg, RES_FUNCEFT_TYPE type)
		{
			int num = 0;
			byte bAddType = CallMonsterCfg.bAddType;
			if ((CallMonsterCfg.bAddType & 1) != 0)
			{
				num += OrignalHost.get_handle().ValueComponent.mActorValue[1].totalValue;
			}
			if ((CallMonsterCfg.bAddType & 2) != 0)
			{
				num += OrignalHost.get_handle().ValueComponent.mActorValue[2].totalValue;
			}
			if ((CallMonsterCfg.bAddType & 4) != 0)
			{
				num += OrignalHost.get_handle().ValueComponent.mActorValue[type].totalValue;
			}
			return num;
		}

		private void ApplyMonsterAdditive(ref PoolObjHandle<ActorRoot> OrignalHost, ref PoolObjHandle<ActorRoot> Monster, ref ResCallMonster CallMonsterCfg)
		{
			this.ApplyProperty(ref Monster, 1, (int)CallMonsterCfg.dwAddAttack, this.GetAddValue(ref OrignalHost, ref Monster, ref CallMonsterCfg, 1));
			this.ApplyProperty(ref Monster, 2, (int)CallMonsterCfg.dwAddMagic, this.GetAddValue(ref OrignalHost, ref Monster, ref CallMonsterCfg, 2));
			this.ApplyProperty(ref Monster, 3, (int)CallMonsterCfg.dwAddArmor, this.GetAddValue(ref OrignalHost, ref Monster, ref CallMonsterCfg, 3));
			this.ApplyProperty(ref Monster, 4, (int)CallMonsterCfg.dwAddResistant, this.GetAddValue(ref OrignalHost, ref Monster, ref CallMonsterCfg, 4));
			this.ApplyProperty(ref Monster, 5, (int)CallMonsterCfg.dwAddHp, this.GetAddValue(ref OrignalHost, ref Monster, ref CallMonsterCfg, 5));
			if ((CallMonsterCfg.bAddType & 4) != 0)
			{
				Monster.get_handle().ValueComponent.actorHp = Monster.get_handle().ValueComponent.actorHpTotal * OrignalHost.get_handle().ValueComponent.actorHp / OrignalHost.get_handle().ValueComponent.actorHpTotal;
				Monster.get_handle().ValueComponent.mActorValue[32].addValue = OrignalHost.get_handle().ValueComponent.actorEpTotal;
				Monster.get_handle().ValueComponent.actorEp = OrignalHost.get_handle().ValueComponent.actorEp;
			}
			else
			{
				Monster.get_handle().ValueComponent.actorHp = Monster.get_handle().ValueComponent.actorHpTotal;
			}
			this.ApplyProperty(ref Monster, 15, (int)CallMonsterCfg.dwAddSpeed, this.GetAddValue(ref OrignalHost, ref Monster, ref CallMonsterCfg, 15));
		}

		private void ApplyProperty(ref PoolObjHandle<ActorRoot> Monster, RES_FUNCEFT_TYPE InType, int InValue, int InBase)
		{
			int num = (int)((double)InBase * (double)InValue / 10000.0);
			Monster.get_handle().ValueComponent.mActorValue[InType].addValue = Monster.get_handle().ValueComponent.mActorValue[InType].addValue + num;
		}

		private int SelectLevel(ref PoolObjHandle<ActorRoot> HostActor, ref ResCallMonster CallMonsterCfg, ref SkillUseContext SkillContext)
		{
			if (CallMonsterCfg.bDependencyType == 1)
			{
				return HostActor.get_handle().ValueComponent.actorSoulLevel;
			}
			if (CallMonsterCfg.bDependencyType == 2)
			{
				return HostActor.get_handle().SkillControl.SkillSlotArray[(int)SkillContext.SlotType].GetSkillLevel();
			}
			return 0;
		}

		private void SpawnMonster(Action _action, ref PoolObjHandle<ActorRoot> tarActor)
		{
			SkillUseContext refParamObject = _action.refParams.GetRefParamObject<SkillUseContext>("SkillContext");
			if (refParamObject == null || !refParamObject.Originator || refParamObject.Originator.get_handle().ActorControl == null)
			{
				DebugHelper.Assert(false, "Failed find orignal actor of this skill. action:{0}", new object[]
				{
					_action.name
				});
				return;
			}
			if (refParamObject.Originator.get_handle().ActorControl.IsDeadState)
			{
				return;
			}
			DebugHelper.Assert(refParamObject.Originator.get_handle().ValueComponent != null, "ValueComponent is null");
			ResCallMonster dataByKey = GameDataMgr.callMonsterDatabin.GetDataByKey((long)this.ConfigID);
			DebugHelper.Assert(dataByKey != null, "Failed find call monster config id:{0} action:{1}", new object[]
			{
				this.ConfigID,
				_action.name
			});
			if (dataByKey == null)
			{
				return;
			}
			int num = Math.Min(CallMonsterTick.MaxLevel, this.SelectLevel(ref refParamObject.Originator, ref dataByKey, ref refParamObject));
			ResMonsterCfgInfo dataCfgInfo = MonsterDataHelper.GetDataCfgInfo((int)dataByKey.dwMonsterID, num);
			DebugHelper.Assert(dataCfgInfo != null, "Failed find monster id={0} diff={1} action:{2}", new object[]
			{
				dataByKey.dwMonsterID,
				num,
				_action.name
			});
			if (dataCfgInfo == null)
			{
				return;
			}
			string text = StringHelper.UTF8BytesToString(ref dataCfgInfo.szCharacterInfo) + ".asset";
			CActorInfo exists = Singleton<CResourceManager>.GetInstance().GetResource(text, typeof(CActorInfo), 0, false, false).m_content as CActorInfo;
			if (exists)
			{
				ActorMeta actorMeta = default(ActorMeta);
				ActorMeta actorMeta2 = actorMeta;
				actorMeta2.ConfigId = (int)dataByKey.dwMonsterID;
				actorMeta2.ActorType = ActorTypeDef.Actor_Type_Monster;
				actorMeta2.ActorCamp = this.SelectCamp(ref refParamObject.Originator);
				actorMeta2.EnCId = (int)dataByKey.dwMonsterID;
				actorMeta2.Difficuty = (byte)num;
				actorMeta2.SkinID = refParamObject.Originator.get_handle().TheActorMeta.SkinID;
				actorMeta = actorMeta2;
				VInt3 location = tarActor.get_handle().location;
				VInt3 forward = tarActor.get_handle().forward;
				if (!PathfindingUtility.IsValidTarget(refParamObject.Originator.get_handle(), location))
				{
					location = refParamObject.Originator.get_handle().location;
					forward = refParamObject.Originator.get_handle().forward;
				}
				PoolObjHandle<ActorRoot> poolObjHandle = Singleton<GameObjMgr>.GetInstance().SpawnActorEx(null, ref actorMeta, location, forward, false, true);
				if (poolObjHandle)
				{
					poolObjHandle.get_handle().InitActor();
					this.ApplyMonsterAdditive(ref refParamObject.Originator, ref poolObjHandle, ref dataByKey);
					MonsterWrapper monsterWrapper = poolObjHandle.get_handle().ActorControl as MonsterWrapper;
					if (monsterWrapper != null)
					{
						monsterWrapper.SetHostActorInfo(ref refParamObject.Originator, refParamObject.SlotType, this.bCopyedHeroInfo);
						if (this.wayPoint != null)
						{
							monsterWrapper.AttackAlongRoute(this.wayPoint.GetComponent<WaypointsHolder>());
						}
						if (this.LifeTime > 0)
						{
							monsterWrapper.LifeTime = this.LifeTime;
						}
					}
					poolObjHandle.get_handle().PrepareFight();
					Singleton<GameObjMgr>.get_instance().AddActor(poolObjHandle);
					poolObjHandle.get_handle().StartFight();
					poolObjHandle.get_handle().ObjLinker.Invincible = this.Invincible;
					poolObjHandle.get_handle().ObjLinker.CanMovable = this.Moveable;
					poolObjHandle.get_handle().Visible = refParamObject.Originator.get_handle().Visible;
					poolObjHandle.get_handle().ValueComponent.actorSoulLevel = refParamObject.Originator.get_handle().ValueComponent.actorSoulLevel;
					refParamObject.Originator.get_handle().ValueComponent.AddSoulExp(0, false, AddSoulType.Other);
				}
			}
		}

		public override void Process(Action _action, Track _track)
		{
			PoolObjHandle<ActorRoot> actorHandle = _action.GetActorHandle(this.TargetId);
			if (!actorHandle)
			{
				if (ActionManager.Instance.isPrintLog)
				{
				}
				return;
			}
			this.wayPoint = _action.GetGameObject(this.WayPointId);
			this.SpawnMonster(_action, ref actorHandle);
		}
	}
}
