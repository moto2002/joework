using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using System;
using UnityEngine;

namespace AGE
{
	[EventCategory("Effect")]
	public class TriggerParticleTick : TickEvent
	{
		[ObjectTemplate(new Type[]
		{

		})]
		public int targetId;

		[ObjectTemplate(new Type[]
		{

		})]
		public int objectSpaceId = -1;

		[ObjectTemplate(new Type[]
		{

		})]
		public int VirtualAttachBulletId = -1;

		[AssetReference(AssetRefType.Particle)]
		public string resourceName = string.Empty;

		public float lifeTime = 2f;

		[SubObject]
		public string bindPointName = string.Empty;

		public Vector3 bindPosOffset = new Vector3(0f, 0f, 0f);

		public Quaternion bindRotOffset = new Quaternion(0f, 0f, 0f, 1f);

		public Vector3 scaling = new Vector3(1f, 1f, 1f);

		public bool bEnableOptCull = true;

		public bool bBulletPos;

		public bool bBulletDir;

		public bool bBullerPosDir;

		public bool enableLayer;

		public int layer;

		public bool enableTag;

		public string tag = string.Empty;

		public bool enableMaxLimit;

		public int MaxLimit = 10;

		public int LimitType = -1;

		public bool applyActionSpeedToParticle = true;

		public bool bUseSkin;

		public bool bUseSkinAdvance;

		private GameObject particleObject;

		public int extend = 10;

		public bool bUseAttachBulletShape;

		public override bool SupportEditMode()
		{
			return true;
		}

		public override BaseEvent Clone()
		{
			TriggerParticleTick triggerParticleTick = ClassObjPool<TriggerParticleTick>.Get();
			triggerParticleTick.CopyData(this);
			return triggerParticleTick;
		}

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			TriggerParticleTick triggerParticleTick = src as TriggerParticleTick;
			this.targetId = triggerParticleTick.targetId;
			this.objectSpaceId = triggerParticleTick.objectSpaceId;
			this.VirtualAttachBulletId = triggerParticleTick.VirtualAttachBulletId;
			this.resourceName = triggerParticleTick.resourceName;
			this.lifeTime = triggerParticleTick.lifeTime;
			this.bindPointName = triggerParticleTick.bindPointName;
			this.bindPosOffset = triggerParticleTick.bindPosOffset;
			this.bindRotOffset = triggerParticleTick.bindRotOffset;
			this.scaling = triggerParticleTick.scaling;
			this.bEnableOptCull = triggerParticleTick.bEnableOptCull;
			this.bBulletPos = triggerParticleTick.bBulletPos;
			this.bBulletDir = triggerParticleTick.bBulletDir;
			this.bBullerPosDir = triggerParticleTick.bBullerPosDir;
			this.enableLayer = triggerParticleTick.enableLayer;
			this.layer = triggerParticleTick.layer;
			this.enableTag = triggerParticleTick.enableTag;
			this.tag = triggerParticleTick.tag;
			this.enableMaxLimit = triggerParticleTick.enableMaxLimit;
			this.MaxLimit = triggerParticleTick.MaxLimit;
			this.LimitType = triggerParticleTick.LimitType;
			this.applyActionSpeedToParticle = triggerParticleTick.applyActionSpeedToParticle;
			this.particleObject = triggerParticleTick.particleObject;
			this.extend = triggerParticleTick.extend;
			this.bUseSkin = triggerParticleTick.bUseSkin;
			this.bUseSkinAdvance = triggerParticleTick.bUseSkinAdvance;
			this.bUseAttachBulletShape = triggerParticleTick.bUseAttachBulletShape;
		}

		public override void OnUse()
		{
			base.OnUse();
			this.targetId = 0;
			this.objectSpaceId = -1;
			this.VirtualAttachBulletId = -1;
			this.lifeTime = 5f;
			this.resourceName = string.Empty;
			this.bindPointName = string.Empty;
			this.bindPosOffset = new Vector3(0f, 0f, 0f);
			this.bindRotOffset = new Quaternion(0f, 0f, 0f, 1f);
			this.scaling = new Vector3(1f, 1f, 1f);
			this.bEnableOptCull = true;
			this.bBulletPos = false;
			this.bBulletDir = false;
			this.bBullerPosDir = false;
			this.enableLayer = false;
			this.layer = 0;
			this.enableTag = false;
			this.tag = string.Empty;
			this.enableMaxLimit = false;
			this.MaxLimit = 10;
			this.LimitType = -1;
			this.applyActionSpeedToParticle = true;
			this.particleObject = null;
			this.extend = 10;
			this.bUseSkinAdvance = false;
			this.bUseAttachBulletShape = false;
		}

		public override void Process(Action _action, Track _track)
		{
			if (MonoSingleton<Reconnection>.GetInstance().isProcessingRelayRecover)
			{
				return;
			}
			SkillUseContext skillUseContext = null;
			Vector3 vector = this.bindPosOffset;
			Quaternion quaternion = this.bindRotOffset;
			GameObject gameObject = _action.GetGameObject(this.targetId);
			GameObject gameObject2 = _action.GetGameObject(this.objectSpaceId);
			Transform transform = null;
			Transform transform2 = null;
			if (this.bindPointName.get_Length() == 0)
			{
				if (gameObject != null)
				{
					transform = gameObject.transform;
				}
				else if (gameObject2 != null)
				{
					transform2 = gameObject2.transform;
				}
			}
			else
			{
				Transform transform3 = null;
				if (gameObject != null)
				{
					GameObject gameObject3 = SubObject.FindSubObject(gameObject, this.bindPointName);
					if (gameObject3 != null)
					{
						transform3 = gameObject3.transform;
					}
					if (transform3 != null)
					{
						transform = transform3;
					}
					else if (gameObject != null)
					{
						transform = gameObject.transform;
					}
				}
				else if (gameObject2 != null)
				{
					GameObject gameObject3 = SubObject.FindSubObject(gameObject2, this.bindPointName);
					if (gameObject3 != null)
					{
						transform3 = gameObject3.transform;
					}
					if (transform3 != null)
					{
						transform2 = transform3;
					}
					else if (gameObject != null)
					{
						transform2 = gameObject2.transform;
					}
				}
			}
			if (this.bBulletPos)
			{
				VInt3 zero = VInt3.zero;
				_action.refParams.GetRefParam("_BulletPos", ref zero);
				vector = (Vector3)zero;
				quaternion = Quaternion.identity;
				if (this.bBulletDir)
				{
					VInt3 zero2 = VInt3.zero;
					if (_action.refParams.GetRefParam("_BulletDir", ref zero2))
					{
						quaternion = Quaternion.LookRotation((Vector3)zero2);
					}
				}
			}
			else if (transform != null)
			{
				vector = transform.localToWorldMatrix.MultiplyPoint(this.bindPosOffset);
				quaternion = transform.rotation * this.bindRotOffset;
			}
			else if (transform2 != null)
			{
				if (gameObject2 != null)
				{
					PoolObjHandle<ActorRoot> actorHandle = _action.GetActorHandle(this.objectSpaceId);
					if (actorHandle)
					{
						vector = (Vector3)IntMath.Transform((VInt3)this.bindPosOffset, actorHandle.get_handle().forward, actorHandle.get_handle().location);
						quaternion = Quaternion.LookRotation((Vector3)actorHandle.get_handle().forward) * this.bindRotOffset;
					}
				}
				else
				{
					vector = transform2.localToWorldMatrix.MultiplyPoint(this.bindPosOffset);
					quaternion = transform2.rotation * this.bindRotOffset;
				}
				if (this.bBulletDir)
				{
					VInt3 zero3 = VInt3.zero;
					if (_action.refParams.GetRefParam("_BulletDir", ref zero3))
					{
						quaternion = Quaternion.LookRotation((Vector3)zero3);
						quaternion *= this.bindRotOffset;
					}
				}
				else if (this.bBullerPosDir)
				{
					skillUseContext = _action.refParams.GetRefParamObject<SkillUseContext>("SkillContext");
					if (skillUseContext != null)
					{
						PoolObjHandle<ActorRoot> originator = skillUseContext.Originator;
						if (originator)
						{
							Vector3 a = transform2.position;
							if (gameObject2 != null)
							{
								PoolObjHandle<ActorRoot> actorHandle2 = _action.GetActorHandle(this.objectSpaceId);
								if (actorHandle2)
								{
									a = (Vector3)actorHandle2.get_handle().location;
								}
							}
							Vector3 forward = a - (Vector3)originator.get_handle().location;
							quaternion = Quaternion.LookRotation(forward);
							quaternion *= this.bindRotOffset;
						}
					}
				}
			}
			if (this.bEnableOptCull && transform2 && transform2.gameObject.layer == LayerMask.NameToLayer("Hide") && !FogOfWar.enable)
			{
				return;
			}
			if (this.bEnableOptCull && MonoSingleton<GlobalConfig>.get_instance().bEnableParticleCullOptimize && !MonoSingleton<CameraSystem>.get_instance().CheckVisiblity(new Bounds(vector, new Vector3((float)this.extend, (float)this.extend, (float)this.extend))))
			{
				return;
			}
			bool flag = false;
			string prefabName;
			if (this.bUseSkin)
			{
				prefabName = SkinResourceHelper.GetResourceName(_action, this.resourceName, this.bUseSkinAdvance);
			}
			else
			{
				prefabName = this.resourceName;
			}
			if (skillUseContext == null)
			{
				skillUseContext = _action.refParams.GetRefParamObject<SkillUseContext>("SkillContext");
			}
			bool flag2 = true;
			int particleLOD = GameSettings.ParticleLOD;
			if (GameSettings.DynamicParticleLOD)
			{
				if (skillUseContext != null && skillUseContext.Originator && skillUseContext.Originator.get_handle().TheActorMeta.PlayerId == Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer().PlayerId)
				{
					flag2 = false;
				}
				if (!flag2 && particleLOD > 1)
				{
					GameSettings.ParticleLOD = 1;
				}
				MonoSingleton<SceneMgr>.GetInstance().m_dynamicLOD = flag2;
			}
			this.particleObject = MonoSingleton<SceneMgr>.GetInstance().GetPooledGameObjLOD(prefabName, true, SceneObjType.ActionRes, vector, quaternion, out flag);
			if (GameSettings.DynamicParticleLOD)
			{
				MonoSingleton<SceneMgr>.GetInstance().m_dynamicLOD = false;
			}
			if (this.particleObject == null)
			{
				if (GameSettings.DynamicParticleLOD)
				{
					MonoSingleton<SceneMgr>.GetInstance().m_dynamicLOD = flag2;
				}
				this.particleObject = MonoSingleton<SceneMgr>.GetInstance().GetPooledGameObjLOD(this.resourceName, true, SceneObjType.ActionRes, vector, quaternion, out flag);
				if (GameSettings.DynamicParticleLOD)
				{
					MonoSingleton<SceneMgr>.GetInstance().m_dynamicLOD = false;
				}
				if (this.particleObject == null)
				{
					if (GameSettings.DynamicParticleLOD)
					{
						GameSettings.ParticleLOD = particleLOD;
					}
					return;
				}
			}
			if (GameSettings.DynamicParticleLOD)
			{
				GameSettings.ParticleLOD = particleLOD;
			}
			if (!this.particleObject)
			{
				return;
			}
			ParticleHelper.IncParticleActiveNumber();
			if (transform != null)
			{
				PoolObjHandle<ActorRoot> poolObjHandle = (!(transform.gameObject == gameObject)) ? ActorHelper.GetActorRoot(transform.gameObject) : _action.GetActorHandle(this.targetId);
				if (poolObjHandle && poolObjHandle.get_handle().ActorMesh)
				{
					this.particleObject.transform.parent = poolObjHandle.get_handle().ActorMesh.transform;
				}
				else
				{
					this.particleObject.transform.parent = transform.parent;
					if (poolObjHandle && poolObjHandle.get_handle().TheActorMeta.ActorCamp != Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().PlayerCamp && FogOfWar.enable)
					{
						if (poolObjHandle.get_handle().HorizonMarker != null)
						{
							poolObjHandle.get_handle().HorizonMarker.AddSubParObj(this.particleObject);
						}
						else
						{
							BulletWrapper bulletWrapper = poolObjHandle.get_handle().ActorControl as BulletWrapper;
							if (bulletWrapper != null)
							{
								bulletWrapper.AddSubParObj(this.particleObject);
							}
						}
					}
				}
			}
			string text = "Particles";
			if (transform && transform.gameObject.layer == LayerMask.NameToLayer("Hide"))
			{
				text = "Hide";
			}
			if (transform == null && transform2 != null && FogOfWar.enable)
			{
				PoolObjHandle<ActorRoot> actorRoot = ActorHelper.GetActorRoot(transform2.gameObject);
				if (actorRoot && actorRoot.get_handle().TheActorMeta.ActorCamp != Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().PlayerCamp)
				{
					if (transform2.gameObject.layer == LayerMask.NameToLayer("Hide"))
					{
						text = "Hide";
					}
					PoolObjHandle<ActorRoot> actorHandle3 = _action.GetActorHandle(this.VirtualAttachBulletId);
					Singleton<GameFowManager>.get_instance().m_collector.AddVirtualParentParticle(this.particleObject, actorHandle3, this.bUseAttachBulletShape);
				}
			}
			MMGame_Math.SetLayer(this.particleObject, text, false);
			if (flag)
			{
				ParticleHelper.Init(this.particleObject, this.scaling);
			}
			Singleton<CGameObjectPool>.GetInstance().RecycleGameObjectDelay(this.particleObject, Mathf.Max(_action.length, (int)(this.lifeTime * 1000f)), new CGameObjectPool.OnDelayRecycleDelegate(TriggerParticleTick.OnRecycleTickObj));
			if (this.applyActionSpeedToParticle && this.particleObject != null)
			{
				_action.AddTempObject(Action.PlaySpeedAffectedType.ePSAT_Fx, this.particleObject);
			}
		}

		public static void OnRecycleTickObj(GameObject obj)
		{
			ParticleHelper.DecParticleActiveNumber();
			if (FogOfWar.enable)
			{
				Singleton<GameFowManager>.get_instance().m_collector.RemoveVirtualParentParticle(obj);
			}
		}
	}
}
