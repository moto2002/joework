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
	public class TriggerParticle : DurationEvent
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

		public bool bUseSkin;

		public bool bUseSkinAdvance;

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

		public bool applyActionSpeedToParticle = true;

		protected GameObject particleObject;

		public int extend = 10;

		public bool bOnlyFollowPos;

		public int iDelayDisappearTime;

		private Vector3 offsetPosition;

		private Transform followTransform;

		private Transform particleTransform;

		public bool bUseAttachBulletShape;

		public override bool SupportEditMode()
		{
			return true;
		}

		public override BaseEvent Clone()
		{
			TriggerParticle triggerParticle = ClassObjPool<TriggerParticle>.Get();
			triggerParticle.CopyData(this);
			return triggerParticle;
		}

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			TriggerParticle triggerParticle = src as TriggerParticle;
			this.targetId = triggerParticle.targetId;
			this.objectSpaceId = triggerParticle.objectSpaceId;
			this.VirtualAttachBulletId = triggerParticle.VirtualAttachBulletId;
			this.resourceName = triggerParticle.resourceName;
			this.bindPointName = triggerParticle.bindPointName;
			this.bindPosOffset = triggerParticle.bindPosOffset;
			this.bindRotOffset = triggerParticle.bindRotOffset;
			this.scaling = triggerParticle.scaling;
			this.bEnableOptCull = triggerParticle.bEnableOptCull;
			this.bBulletPos = triggerParticle.bBulletPos;
			this.bBulletDir = triggerParticle.bBulletDir;
			this.bBullerPosDir = triggerParticle.bBullerPosDir;
			this.enableLayer = triggerParticle.enableLayer;
			this.layer = triggerParticle.layer;
			this.enableTag = triggerParticle.enableTag;
			this.tag = triggerParticle.tag;
			this.applyActionSpeedToParticle = triggerParticle.applyActionSpeedToParticle;
			this.particleObject = triggerParticle.particleObject;
			this.extend = triggerParticle.extend;
			this.bOnlyFollowPos = triggerParticle.bOnlyFollowPos;
			this.bUseSkin = triggerParticle.bUseSkin;
			this.bUseSkinAdvance = triggerParticle.bUseSkinAdvance;
			this.iDelayDisappearTime = triggerParticle.iDelayDisappearTime;
			this.bUseAttachBulletShape = triggerParticle.bUseAttachBulletShape;
		}

		public override void OnUse()
		{
			base.OnUse();
			this.targetId = 0;
			this.objectSpaceId = -1;
			this.VirtualAttachBulletId = -1;
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
			this.applyActionSpeedToParticle = true;
			this.particleObject = null;
			this.extend = 10;
			this.offsetPosition = Vector3.zero;
			this.followTransform = null;
			this.particleTransform = null;
			this.bOnlyFollowPos = false;
			this.bUseSkin = false;
			this.bUseSkinAdvance = false;
			this.iDelayDisappearTime = 0;
			this.bUseAttachBulletShape = false;
		}

		public override void Enter(Action _action, Track _track)
		{
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
					PoolObjHandle<ActorRoot> actorHandle = _action.GetActorHandle(this.targetId);
					this.followTransform = transform;
				}
				else if (gameObject2 != null)
				{
					transform2 = gameObject2.transform;
				}
			}
			else if (gameObject != null)
			{
				GameObject gameObject3 = SubObject.FindSubObject(gameObject, this.bindPointName);
				if (gameObject3 != null)
				{
					transform = gameObject3.transform;
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
					transform2 = gameObject3.transform;
				}
				else if (gameObject != null)
				{
					transform2 = gameObject2.transform;
				}
			}
			if (this.bEnableOptCull && transform2 && transform2.gameObject.layer == LayerMask.NameToLayer("Hide") && !FogOfWar.enable)
			{
				return;
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
					PoolObjHandle<ActorRoot> actorHandle2 = _action.GetActorHandle(this.objectSpaceId);
					if (actorHandle2)
					{
						vector = (Vector3)IntMath.Transform((VInt3)this.bindPosOffset, actorHandle2.get_handle().forward, actorHandle2.get_handle().location);
						quaternion = Quaternion.LookRotation((Vector3)actorHandle2.get_handle().forward) * this.bindRotOffset;
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
								PoolObjHandle<ActorRoot> actorHandle3 = _action.GetActorHandle(this.objectSpaceId);
								if (actorHandle3)
								{
									a = (Vector3)actorHandle3.get_handle().location;
								}
							}
							Vector3 forward = a - (Vector3)originator.get_handle().location;
							quaternion = Quaternion.LookRotation(forward);
							quaternion *= this.bindRotOffset;
						}
					}
				}
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
			if (this.particleObject != null)
			{
				this.particleTransform = this.particleObject.transform;
			}
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
				this.particleTransform = this.particleObject.transform;
			}
			if (GameSettings.DynamicParticleLOD)
			{
				GameSettings.ParticleLOD = particleLOD;
			}
			ParticleHelper.IncParticleActiveNumber();
			if (transform != null)
			{
				if (!this.bOnlyFollowPos)
				{
					PoolObjHandle<ActorRoot> arg_5A7_0 = (!(transform.gameObject == gameObject)) ? ActorHelper.GetActorRoot(transform.gameObject) : _action.GetActorHandle(this.targetId);
					this.particleTransform.parent = transform;
				}
				else
				{
					this.offsetPosition = vector - transform.position;
				}
			}
			if (flag)
			{
				if (this.enableLayer || this.enableTag)
				{
					Transform[] componentsInChildren = this.particleObject.GetComponentsInChildren<Transform>();
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
				ParticleSystem[] componentsInChildren2 = this.particleObject.GetComponentsInChildren<ParticleSystem>();
				if (componentsInChildren2 != null)
				{
					for (int j = 0; j < componentsInChildren2.Length; j++)
					{
						componentsInChildren2[j].startSize *= this.scaling.x;
						componentsInChildren2[j].startLifetime *= this.scaling.y;
						componentsInChildren2[j].startSpeed *= this.scaling.z;
						componentsInChildren2[j].transform.localScale *= this.scaling.x;
					}
					ParticleSystemPoolComponent cachedComponent = Singleton<CGameObjectPool>.GetInstance().GetCachedComponent<ParticleSystemPoolComponent>(this.particleObject, true);
					ParticleSystemPoolComponent.ParticleSystemCache[] array = new ParticleSystemPoolComponent.ParticleSystemCache[componentsInChildren2.Length];
					for (int k = 0; k < array.Length; k++)
					{
						array[k].par = componentsInChildren2[k];
						array[k].emmitState = componentsInChildren2[k].enableEmission;
					}
					cachedComponent.cache = array;
				}
			}
			else
			{
				ParticleSystemPoolComponent cachedComponent2 = Singleton<CGameObjectPool>.GetInstance().GetCachedComponent<ParticleSystemPoolComponent>(this.particleObject, false);
				if (null != cachedComponent2)
				{
					ParticleSystemPoolComponent.ParticleSystemCache[] cache = cachedComponent2.cache;
					if (cache != null)
					{
						for (int l = 0; l < cache.Length; l++)
						{
							if (cache[l].par.enableEmission != cache[l].emmitState)
							{
								cache[l].par.enableEmission = cache[l].emmitState;
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
					PoolObjHandle<ActorRoot> actorHandle4 = _action.GetActorHandle(this.VirtualAttachBulletId);
					Singleton<GameFowManager>.get_instance().m_collector.AddVirtualParentParticle(this.particleObject, actorHandle4, this.bUseAttachBulletShape);
				}
			}
			MMGame_Math.SetLayer(this.particleObject, text, false);
			if (this.applyActionSpeedToParticle)
			{
				_action.AddTempObject(Action.PlaySpeedAffectedType.ePSAT_Fx, this.particleObject);
			}
		}

		public override void Process(Action _action, Track _track, int _localTime)
		{
			if (this.bOnlyFollowPos && this.particleTransform != null && this.followTransform != null && this.particleObject != null)
			{
				this.particleTransform.position = this.followTransform.position + this.offsetPosition;
			}
			base.Process(_action, _track, _localTime);
		}

		private static void OnDelayRecycleParticleCallback(GameObject recycleObj)
		{
			recycleObj.transform.position = new Vector3(10000f, 10000f, 10000f);
			if (FogOfWar.enable)
			{
				Singleton<GameFowManager>.get_instance().m_collector.RemoveVirtualParentParticle(recycleObj);
			}
		}

		public override void Leave(Action _action, Track _track)
		{
			if (this.particleObject != null)
			{
				if (this.iDelayDisappearTime > 0)
				{
					ParticleSystemPoolComponent cachedComponent = Singleton<CGameObjectPool>.GetInstance().GetCachedComponent<ParticleSystemPoolComponent>(this.particleObject, false);
					if (null != cachedComponent)
					{
						ParticleSystemPoolComponent.ParticleSystemCache[] cache = cachedComponent.cache;
						if (cache != null)
						{
							for (int i = 0; i < cache.Length; i++)
							{
								cache[i].par.enableEmission = false;
							}
						}
						MonoSingleton<SceneMgr>.GetInstance().AddToRoot(this.particleObject, SceneObjType.ActionRes);
						Singleton<CGameObjectPool>.GetInstance().RecycleGameObjectDelay(this.particleObject, this.iDelayDisappearTime, new CGameObjectPool.OnDelayRecycleDelegate(TriggerParticle.OnDelayRecycleParticleCallback));
					}
				}
				else
				{
					if (FogOfWar.enable)
					{
						Singleton<GameFowManager>.get_instance().m_collector.RemoveVirtualParentParticle(this.particleObject);
					}
					this.particleTransform.position = new Vector3(10000f, 10000f, 10000f);
					Singleton<CGameObjectPool>.GetInstance().RecycleGameObject(this.particleObject);
				}
				ParticleHelper.DecParticleActiveNumber();
				if (this.applyActionSpeedToParticle)
				{
					_action.RemoveTempObject(Action.PlaySpeedAffectedType.ePSAT_Fx, this.particleObject);
				}
			}
		}
	}
}
