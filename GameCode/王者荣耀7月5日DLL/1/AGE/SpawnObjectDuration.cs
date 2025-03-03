using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.DataCenter;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using CSProtocol;
using System;
using UnityEngine;

namespace AGE
{
	[EventCategory("MMGame/Skill")]
	public class SpawnObjectDuration : DurationEvent
	{
		[ObjectTemplate(true)]
		public int targetId = -1;

		[ObjectTemplate(new Type[]
		{

		})]
		public int parentId = -1;

		[ObjectTemplate(new Type[]
		{

		})]
		public int objectSpaceId = -1;

		[AssetReference(AssetRefType.Particle)]
		public string prefabName = string.Empty;

		public bool bMoveCollision;

		public bool recreateExisting = true;

		public bool bTargetPosition;

		public VInt3 targetPosition = VInt3.zero;

		public bool modifyTranslation = true;

		public bool superTranslation;

		public VInt3 translation = VInt3.zero;

		public bool modifyDirection;

		public VInt3 direction = VInt3.forward;

		public bool modifyScaling;

		public Vector3 scaling = Vector3.one;

		public bool enableLayer;

		public int layer;

		public bool enableTag;

		public string tag = string.Empty;

		public bool applyActionSpeedToAnimation = true;

		public bool applyActionSpeedToParticle = true;

		public bool bUseSkin;

		public bool bUseSkinAdvance;

		private ActorRootSlot actorSlot;

		private GameObject m_particleObj;

		private PoolObjHandle<ActorRoot> actorRoot;

		public int sightRadius;

		public bool bEyeObj;

		public int EyeLifeTime;

		public bool bVisibleByFow = true;

		public bool bCheckVisibleByShape;

		[AssetReference(AssetRefType.MonsterConfigId)]
		public int EyeCfgIdByMonster;

		public bool bInvisibleBullet;

		public bool bForbidBulletInObstacle;

		public override bool SupportEditMode()
		{
			return true;
		}

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			SpawnObjectDuration spawnObjectDuration = src as SpawnObjectDuration;
			this.targetId = spawnObjectDuration.targetId;
			this.parentId = spawnObjectDuration.parentId;
			this.objectSpaceId = spawnObjectDuration.objectSpaceId;
			this.prefabName = spawnObjectDuration.prefabName;
			this.bMoveCollision = spawnObjectDuration.bMoveCollision;
			this.recreateExisting = spawnObjectDuration.recreateExisting;
			this.modifyTranslation = spawnObjectDuration.modifyTranslation;
			this.superTranslation = spawnObjectDuration.superTranslation;
			this.translation = spawnObjectDuration.translation;
			this.bTargetPosition = spawnObjectDuration.bTargetPosition;
			this.targetPosition = spawnObjectDuration.targetPosition;
			this.modifyDirection = spawnObjectDuration.modifyDirection;
			this.direction = spawnObjectDuration.direction;
			this.modifyScaling = spawnObjectDuration.modifyScaling;
			this.scaling = spawnObjectDuration.scaling;
			this.enableLayer = spawnObjectDuration.enableLayer;
			this.layer = spawnObjectDuration.layer;
			this.enableTag = spawnObjectDuration.enableTag;
			this.tag = spawnObjectDuration.tag;
			this.applyActionSpeedToAnimation = spawnObjectDuration.applyActionSpeedToAnimation;
			this.applyActionSpeedToParticle = spawnObjectDuration.applyActionSpeedToParticle;
			this.bUseSkin = spawnObjectDuration.bUseSkin;
			this.bUseSkinAdvance = spawnObjectDuration.bUseSkinAdvance;
			this.sightRadius = spawnObjectDuration.sightRadius;
			this.bVisibleByFow = spawnObjectDuration.bVisibleByFow;
			this.bCheckVisibleByShape = spawnObjectDuration.bCheckVisibleByShape;
			this.bEyeObj = spawnObjectDuration.bEyeObj;
			this.EyeLifeTime = spawnObjectDuration.EyeLifeTime;
			this.EyeCfgIdByMonster = spawnObjectDuration.EyeCfgIdByMonster;
			this.bInvisibleBullet = spawnObjectDuration.bInvisibleBullet;
			this.bForbidBulletInObstacle = spawnObjectDuration.bForbidBulletInObstacle;
		}

		public override BaseEvent Clone()
		{
			SpawnObjectDuration spawnObjectDuration = ClassObjPool<SpawnObjectDuration>.Get();
			spawnObjectDuration.CopyData(this);
			return spawnObjectDuration;
		}

		public override void OnUse()
		{
			base.OnUse();
			this.actorSlot = null;
			this.m_particleObj = null;
			this.actorRoot.Release();
			this.sightRadius = 0;
			this.bVisibleByFow = true;
			this.bCheckVisibleByShape = false;
			this.bEyeObj = false;
			this.EyeLifeTime = 0;
			this.EyeCfgIdByMonster = 0;
			this.bInvisibleBullet = false;
			this.bForbidBulletInObstacle = false;
		}

		private void SetParent(ref PoolObjHandle<ActorRoot> parentActor, ref PoolObjHandle<ActorRoot> newActor, VInt3 trans)
		{
			if (parentActor && newActor)
			{
				this.actorSlot = parentActor.get_handle().CreateActorRootSlot(newActor, parentActor.get_handle().location, trans);
			}
		}

		private void CreateEye()
		{
			if (this.actorRoot && this.bEyeObj)
			{
				this.actorRoot.get_handle().ObjLinker.CanMovable = false;
				Singleton<GameObjMgr>.get_instance().AddActor(this.actorRoot);
				EyeWrapper eyeWrapper = this.actorRoot.get_handle().ActorControl as EyeWrapper;
				if (eyeWrapper != null)
				{
					eyeWrapper.LifeTime = this.EyeLifeTime;
				}
				if (this.actorRoot.get_handle().HorizonMarker != null)
				{
					this.actorRoot.get_handle().HorizonMarker.SightRadius = this.sightRadius;
				}
				if (this.actorRoot.get_handle().SMNode != null)
				{
					VCollisionSphere vCollisionSphere = new VCollisionSphere();
					vCollisionSphere.Born(this.actorRoot);
					vCollisionSphere.Pos = VInt3.zero;
					vCollisionSphere.Radius = 500;
					vCollisionSphere.dirty = true;
					vCollisionSphere.ConditionalUpdateShape();
					this.actorRoot.get_handle().SMNode.Attach();
				}
			}
		}

		private void CreateBullet()
		{
			if (this.actorRoot)
			{
				Singleton<GameObjMgr>.get_instance().AddBullet(ref this.actorRoot);
				BulletWrapper bulletWrapper = this.actorRoot.get_handle().ActorControl as BulletWrapper;
				if (bulletWrapper != null)
				{
					if (this.bMoveCollision)
					{
						bulletWrapper.SetMoveCollision(this.bMoveCollision);
					}
					if (FogOfWar.enable)
					{
						bulletWrapper.SightRadius = this.sightRadius;
						COM_PLAYERCAMP playerCamp = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().PlayerCamp;
						if (bulletWrapper.actor.TheActorMeta.ActorCamp != playerCamp)
						{
							bulletWrapper.m_bVisibleByFow = this.bVisibleByFow;
							bulletWrapper.m_bVisibleByShape = this.bCheckVisibleByShape;
							if (this.bVisibleByFow)
							{
								if (this.bCheckVisibleByShape)
								{
									GameFowCollector.SetObjWithColVisibleByFow(this.actorRoot, Singleton<GameFowManager>.get_instance(), playerCamp);
								}
								else
								{
									GameFowCollector.SetObjVisibleByFow(this.actorRoot.get_handle().gameObject, Singleton<GameFowManager>.get_instance(), playerCamp);
								}
							}
						}
					}
				}
			}
		}

		private void RemoveBullet()
		{
			if (this.actorRoot)
			{
				Singleton<GameObjMgr>.get_instance().RmvBullet(ref this.actorRoot);
			}
		}

		private void EnterSpawnBullet(Action _action, Track _track)
		{
			string resourceName;
			if (this.bUseSkin)
			{
				resourceName = SkinResourceHelper.GetResourceName(_action, this.prefabName, this.bUseSkinAdvance);
			}
			else
			{
				resourceName = this.prefabName;
			}
			VInt3 vInt = VInt3.zero;
			VInt3 forward = VInt3.forward;
			SkillUseContext refParamObject = _action.refParams.GetRefParamObject<SkillUseContext>("SkillContext");
			COM_PLAYERCAMP camp = (refParamObject == null || !refParamObject.Originator) ? 0 : refParamObject.Originator.get_handle().TheActorMeta.ActorCamp;
			GameObject gameObject = _action.GetGameObject(this.parentId);
			PoolObjHandle<ActorRoot> actorHandle = _action.GetActorHandle(this.parentId);
			PoolObjHandle<ActorRoot> actorHandle2 = _action.GetActorHandle(this.objectSpaceId);
			if (actorHandle2)
			{
				ActorRoot handle = actorHandle2.get_handle();
				if (this.superTranslation)
				{
					VInt3 zero = VInt3.zero;
					_action.refParams.GetRefParam("_BulletPos", ref zero);
					vInt = IntMath.Transform(zero, handle.forward, handle.location);
				}
				else if (this.modifyTranslation)
				{
					vInt = IntMath.Transform(this.translation, handle.forward, handle.location);
				}
				if (this.modifyDirection)
				{
					forward = actorHandle2.get_handle().forward;
				}
			}
			else if (this.bTargetPosition)
			{
				vInt = this.translation + this.targetPosition;
				if (this.modifyDirection && refParamObject != null && refParamObject.Originator)
				{
					forward = refParamObject.Originator.get_handle().forward;
				}
			}
			else
			{
				if (this.modifyTranslation)
				{
					vInt = this.translation;
				}
				if (this.modifyDirection && this.direction.x != 0 && this.direction.y != 0)
				{
					forward = this.direction;
					forward.NormalizeTo(1000);
				}
			}
			if (this.targetId >= 0)
			{
				_action.ExpandGameObject(this.targetId);
				GameObject gameObject2 = _action.GetGameObject(this.targetId);
				if (this.recreateExisting && gameObject2 != null)
				{
					if (this.applyActionSpeedToAnimation)
					{
						_action.RemoveTempObject(Action.PlaySpeedAffectedType.ePSAT_Anim, gameObject2);
					}
					if (this.applyActionSpeedToParticle)
					{
						_action.RemoveTempObject(Action.PlaySpeedAffectedType.ePSAT_Fx, gameObject2);
					}
					ActorHelper.DetachActorRoot(gameObject2);
					ActionManager.DestroyGameObject(gameObject2);
					_action.SetGameObject(this.targetId, null);
				}
				bool flag = true;
				if (!(gameObject2 == null))
				{
					return;
				}
				if (this.bForbidBulletInObstacle && !PathfindingUtility.IsValidTarget(refParamObject.Originator.get_handle(), vInt))
				{
					bool flag2 = false;
					VInt3 vInt2 = PathfindingUtility.FindValidTarget(refParamObject.Originator.get_handle(), vInt, refParamObject.Originator.get_handle().location, 10000, out flag2);
					if (flag2)
					{
						VInt vInt3 = 0;
						PathfindingUtility.GetGroundY(vInt2, out vInt3);
						vInt2.y = vInt3.i;
						vInt = vInt2;
					}
					else
					{
						vInt = refParamObject.Originator.get_handle().location;
					}
				}
				GameObject gameObject3 = MonoSingleton<SceneMgr>.GetInstance().Spawn("TempObject", SceneObjType.Bullet, vInt, forward);
				if (!gameObject3)
				{
					throw new Exception("Age:SpawnObjectDuration Spawn Exception");
				}
				gameObject3.transform.localScale = Vector3.one;
				bool flag3 = true;
				int particleLOD = GameSettings.ParticleLOD;
				if (GameSettings.DynamicParticleLOD)
				{
					if (refParamObject != null && refParamObject.Originator && refParamObject.Originator.get_handle().TheActorMeta.PlayerId == Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer().PlayerId)
					{
						flag3 = false;
					}
					if (!flag3 && particleLOD > 1)
					{
						GameSettings.ParticleLOD = 1;
					}
					MonoSingleton<SceneMgr>.GetInstance().m_dynamicLOD = flag3;
				}
				this.m_particleObj = MonoSingleton<SceneMgr>.GetInstance().GetPooledGameObjLOD(resourceName, true, SceneObjType.ActionRes, gameObject3.transform.position, gameObject3.transform.rotation, out flag);
				if (GameSettings.DynamicParticleLOD)
				{
					MonoSingleton<SceneMgr>.GetInstance().m_dynamicLOD = false;
				}
				if (this.m_particleObj == null)
				{
					if (GameSettings.DynamicParticleLOD)
					{
						MonoSingleton<SceneMgr>.GetInstance().m_dynamicLOD = flag3;
					}
					this.m_particleObj = MonoSingleton<SceneMgr>.GetInstance().GetPooledGameObjLOD(this.prefabName, true, SceneObjType.ActionRes, gameObject3.transform.position, gameObject3.transform.rotation, out flag);
					if (GameSettings.DynamicParticleLOD)
					{
						MonoSingleton<SceneMgr>.GetInstance().m_dynamicLOD = false;
					}
				}
				if (GameSettings.DynamicParticleLOD)
				{
					GameSettings.ParticleLOD = particleLOD;
				}
				if (this.m_particleObj != null)
				{
					this.m_particleObj.transform.SetParent(gameObject3.transform);
					this.m_particleObj.transform.localPosition = Vector3.zero;
					this.m_particleObj.transform.localRotation = Quaternion.identity;
				}
				this.actorRoot = ActorHelper.AttachActorRoot(gameObject3, ActorTypeDef.Actor_Type_Bullet, camp, null);
				_action.SetGameObject(this.targetId, gameObject3);
				this.actorRoot.get_handle().location = vInt;
				this.actorRoot.get_handle().forward = forward;
				VCollisionShape.InitActorCollision(this.actorRoot, this.m_particleObj, _action.actionName);
				if (this.actorRoot.get_handle().shape != null)
				{
					this.actorRoot.get_handle().shape.ConditionalUpdateShape();
				}
				if (this.bInvisibleBullet && this.actorRoot.get_handle().ActorControl != null)
				{
					BulletWrapper bulletWrapper = this.actorRoot.get_handle().ActorControl as BulletWrapper;
					if (bulletWrapper != null)
					{
						bulletWrapper.InitForInvisibleBullet();
					}
				}
				this.actorRoot.get_handle().InitActor();
				if (refParamObject != null)
				{
					refParamObject.EffectPos = this.actorRoot.get_handle().location;
					if (this.actorRoot.get_handle().TheActorMeta.ActorType != ActorTypeDef.Actor_Type_EYE)
					{
						this.CreateBullet();
					}
				}
				if (this.applyActionSpeedToAnimation)
				{
					_action.AddTempObject(Action.PlaySpeedAffectedType.ePSAT_Anim, gameObject3);
				}
				if (this.applyActionSpeedToParticle)
				{
					_action.AddTempObject(Action.PlaySpeedAffectedType.ePSAT_Fx, gameObject3);
				}
				this.actorRoot.get_handle().StartFight();
				if (this.enableLayer || this.enableTag)
				{
					if (this.enableLayer)
					{
						gameObject3.layer = this.layer;
					}
					if (this.enableTag)
					{
						gameObject3.tag = this.tag;
					}
					Transform[] componentsInChildren = gameObject3.GetComponentsInChildren<Transform>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						if (this.enableLayer)
						{
							componentsInChildren[i].gameObject.layer = this.layer;
						}
						if (this.enableTag)
						{
							componentsInChildren[i].gameObject.tag = this.tag;
						}
					}
				}
				if (flag)
				{
					ParticleHelper.Init(gameObject3, this.scaling);
				}
				PoolObjHandle<ActorRoot> actorHandle3 = _action.GetActorHandle(this.targetId);
				this.SetParent(ref actorHandle, ref actorHandle3, this.translation);
				if (this.modifyScaling)
				{
					gameObject3.transform.localScale = this.scaling;
				}
			}
			else
			{
				GameObject gameObject4;
				if (this.modifyDirection)
				{
					gameObject4 = MonoSingleton<SceneMgr>.GetInstance().InstantiateLOD(this.prefabName, true, SceneObjType.ActionRes, (Vector3)vInt, Quaternion.LookRotation((Vector3)forward));
				}
				else
				{
					gameObject4 = MonoSingleton<SceneMgr>.GetInstance().InstantiateLOD(this.prefabName, true, SceneObjType.ActionRes, (Vector3)vInt);
				}
				if (gameObject4 == null)
				{
					return;
				}
				if (this.applyActionSpeedToAnimation)
				{
					_action.AddTempObject(Action.PlaySpeedAffectedType.ePSAT_Anim, gameObject4);
				}
				if (this.applyActionSpeedToParticle)
				{
					_action.AddTempObject(Action.PlaySpeedAffectedType.ePSAT_Fx, gameObject4);
				}
				if (this.enableLayer)
				{
					gameObject4.layer = this.layer;
					Transform[] componentsInChildren2 = gameObject4.GetComponentsInChildren<Transform>();
					for (int j = 0; j < componentsInChildren2.Length; j++)
					{
						componentsInChildren2[j].gameObject.layer = this.layer;
					}
				}
				if (this.enableTag)
				{
					gameObject4.tag = this.tag;
					Transform[] componentsInChildren3 = gameObject4.GetComponentsInChildren<Transform>();
					for (int k = 0; k < componentsInChildren3.Length; k++)
					{
						componentsInChildren3[k].gameObject.tag = this.tag;
					}
				}
				if (gameObject4.GetComponent<ParticleSystem>() && this.modifyScaling)
				{
					ParticleSystem[] componentsInChildren4 = gameObject4.GetComponentsInChildren<ParticleSystem>();
					for (int l = 0; l < componentsInChildren4.Length; l++)
					{
						componentsInChildren4[l].startSize *= this.scaling.x;
						componentsInChildren4[l].startLifetime *= this.scaling.y;
						componentsInChildren4[l].startSpeed *= this.scaling.z;
						componentsInChildren4[l].transform.localScale *= this.scaling.x;
					}
				}
				PoolObjHandle<ActorRoot> poolObjHandle = ActorHelper.GetActorRoot(gameObject4);
				this.SetParent(ref actorHandle, ref poolObjHandle, this.translation);
				if (this.modifyScaling)
				{
					gameObject4.transform.localScale = this.scaling;
				}
			}
		}

		private void EnterSpawnEye(Action _action, Track _track)
		{
			if (this.bUseSkin)
			{
				string resourceName = SkinResourceHelper.GetResourceName(_action, this.prefabName, this.bUseSkinAdvance);
			}
			else
			{
				string resourceName = this.prefabName;
			}
			VInt3 vInt = VInt3.zero;
			VInt3 forward = VInt3.forward;
			SkillUseContext refParamObject = _action.refParams.GetRefParamObject<SkillUseContext>("SkillContext");
			COM_PLAYERCAMP cOM_PLAYERCAMP = (refParamObject == null || !refParamObject.Originator) ? 0 : refParamObject.Originator.get_handle().TheActorMeta.ActorCamp;
			GameObject gameObject = _action.GetGameObject(this.parentId);
			PoolObjHandle<ActorRoot> actorHandle = _action.GetActorHandle(this.parentId);
			PoolObjHandle<ActorRoot> actorHandle2 = _action.GetActorHandle(this.objectSpaceId);
			if (actorHandle2)
			{
				ActorRoot handle = actorHandle2.get_handle();
				if (this.superTranslation)
				{
					VInt3 zero = VInt3.zero;
					_action.refParams.GetRefParam("_BulletPos", ref zero);
					vInt = IntMath.Transform(zero, handle.forward, handle.location);
				}
				else if (this.modifyTranslation)
				{
					vInt = IntMath.Transform(this.translation, handle.forward, handle.location);
				}
				if (this.modifyDirection)
				{
					forward = actorHandle2.get_handle().forward;
				}
			}
			else if (this.bTargetPosition)
			{
				vInt = this.translation + this.targetPosition;
				if (this.modifyDirection && refParamObject != null && refParamObject.Originator)
				{
					forward = refParamObject.Originator.get_handle().forward;
				}
			}
			else
			{
				if (this.modifyTranslation)
				{
					vInt = this.translation;
				}
				if (this.modifyDirection && this.direction.x != 0 && this.direction.y != 0)
				{
					forward = this.direction;
					forward.NormalizeTo(1000);
				}
			}
			if (this.targetId >= 0)
			{
				_action.ExpandGameObject(this.targetId);
				GameObject gameObject2 = _action.GetGameObject(this.targetId);
				if (this.recreateExisting && gameObject2 != null)
				{
					if (this.applyActionSpeedToAnimation)
					{
						_action.RemoveTempObject(Action.PlaySpeedAffectedType.ePSAT_Anim, gameObject2);
					}
					if (this.applyActionSpeedToParticle)
					{
						_action.RemoveTempObject(Action.PlaySpeedAffectedType.ePSAT_Fx, gameObject2);
					}
					ActorHelper.DetachActorRoot(gameObject2);
					ActionManager.DestroyGameObject(gameObject2);
					_action.SetGameObject(this.targetId, null);
				}
				GameObject gameObject3 = null;
				if (!(gameObject2 == null))
				{
					return;
				}
				ActorStaticData actorStaticData = default(ActorStaticData);
				IGameActorDataProvider actorDataProvider = Singleton<ActorDataCenter>.get_instance().GetActorDataProvider(GameActorDataProviderType.StaticBattleDataProvider);
				ActorMeta actorMeta = default(ActorMeta);
				ActorMeta actorMeta2 = actorMeta;
				actorMeta2.ActorType = ActorTypeDef.Actor_Type_EYE;
				actorMeta2.ActorCamp = cOM_PLAYERCAMP;
				actorMeta2.ConfigId = this.EyeCfgIdByMonster;
				actorMeta2.EnCId = this.EyeCfgIdByMonster;
				actorMeta = actorMeta2;
				actorDataProvider.GetActorStaticData(ref actorMeta, ref actorStaticData);
				CActorInfo exists = null;
				if (!string.IsNullOrEmpty(actorStaticData.TheResInfo.ResPath))
				{
					CActorInfo actorInfo = CActorInfo.GetActorInfo(actorStaticData.TheResInfo.ResPath, 0);
					if (actorInfo != null)
					{
						exists = (CActorInfo)Object.Instantiate(actorInfo);
					}
				}
				PoolObjHandle<ActorRoot> poolObjHandle = default(PoolObjHandle<ActorRoot>);
				if (exists)
				{
					if (refParamObject.Originator && !PathfindingUtility.IsValidTarget(refParamObject.Originator.get_handle(), vInt) && !Singleton<GameFowManager>.get_instance().m_pFieldObj.FindNearestNotBrickFromWorldLocNonFow(ref vInt, refParamObject.Originator.get_handle()))
					{
						vInt = refParamObject.Originator.get_handle().location;
					}
					poolObjHandle = Singleton<GameObjMgr>.get_instance().SpawnActorEx(null, ref actorMeta, vInt, forward, false, true);
					if (poolObjHandle)
					{
						this.actorRoot = poolObjHandle;
						gameObject3 = poolObjHandle.get_handle().gameObject;
						poolObjHandle.get_handle().InitActor();
						this.CreateEye();
						poolObjHandle.get_handle().PrepareFight();
						poolObjHandle.get_handle().StartFight();
					}
				}
				if (!poolObjHandle)
				{
					return;
				}
				if (!gameObject3)
				{
					throw new Exception("Age:SpawnObjectDuration Spawn Exception");
				}
				gameObject3.transform.localScale = Vector3.one;
				if (GameSettings.DynamicParticleLOD)
				{
					bool flag = true;
					int particleLOD = GameSettings.ParticleLOD;
					if (refParamObject != null && refParamObject.Originator && refParamObject.Originator.get_handle().TheActorMeta.PlayerId == Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer().PlayerId)
					{
						flag = false;
					}
					if (!flag && particleLOD > 1)
					{
						GameSettings.ParticleLOD = 1;
					}
					MonoSingleton<SceneMgr>.GetInstance().m_dynamicLOD = flag;
				}
				this.actorRoot.get_handle().location = vInt;
				this.actorRoot.get_handle().forward = forward;
				if (this.actorRoot.get_handle().shape != null)
				{
					this.actorRoot.get_handle().shape.ConditionalUpdateShape();
				}
				if (this.actorRoot.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_EYE)
				{
					this.actorRoot.get_handle().TheActorMeta.ConfigId = this.EyeCfgIdByMonster;
				}
				if (refParamObject != null)
				{
					refParamObject.EffectPos = this.actorRoot.get_handle().location;
					if (this.actorRoot.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_EYE)
					{
						DebugHelper.Assert(this.actorRoot.get_handle().TheActorMeta.ActorCamp == cOM_PLAYERCAMP);
						this.actorRoot.get_handle().TheActorMeta.ActorCamp = cOM_PLAYERCAMP;
					}
				}
				SpawnEyeEventParam spawnEyeEventParam = new SpawnEyeEventParam(refParamObject.Originator, vInt);
				Singleton<GameSkillEventSys>.GetInstance().SendEvent<SpawnEyeEventParam>(GameSkillEventDef.Event_SpawnEye, refParamObject.Originator, ref spawnEyeEventParam, GameSkillEventChannel.Channel_HostCtrlActor);
			}
		}

		public override void Enter(Action _action, Track _track)
		{
			base.Enter(_action, _track);
			if (this.bEyeObj)
			{
				this.EnterSpawnEye(_action, _track);
			}
			else
			{
				this.EnterSpawnBullet(_action, _track);
			}
		}

		public override void Leave(Action _action, Track _track)
		{
			base.Leave(_action, _track);
			if (!this.bEyeObj)
			{
				if (this.m_particleObj)
				{
					this.m_particleObj.transform.parent = null;
					ActionManager.DestroyGameObject(this.m_particleObj);
				}
				GameObject gameObject = _action.GetGameObject(this.targetId);
				if (this.targetId >= 0 && gameObject != null)
				{
					if (this.applyActionSpeedToAnimation)
					{
						_action.RemoveTempObject(Action.PlaySpeedAffectedType.ePSAT_Anim, gameObject);
					}
					if (this.applyActionSpeedToParticle)
					{
						_action.RemoveTempObject(Action.PlaySpeedAffectedType.ePSAT_Fx, gameObject);
					}
					if (this.bInvisibleBullet && this.actorRoot.get_handle().ActorControl != null)
					{
						BulletWrapper bulletWrapper = this.actorRoot.get_handle().ActorControl as BulletWrapper;
						if (bulletWrapper != null)
						{
							bulletWrapper.UninitForInvisibleBullet();
						}
					}
					this.RemoveBullet();
					ActorHelper.DetachActorRoot(gameObject);
					ActionManager.DestroyGameObjectFromAction(_action, gameObject);
				}
			}
			if (this.actorSlot != null)
			{
				PoolObjHandle<ActorRoot> actorHandle = _action.GetActorHandle(this.parentId);
				if (actorHandle)
				{
					actorHandle.get_handle().RemoveActorRootSlot(this.actorSlot);
				}
				this.actorSlot = null;
			}
		}
	}
}
