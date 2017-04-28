using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameSystem
{
	public class GameFowCollector
	{
		private struct VirtualParticleAttachContext
		{
			public GameObject VirtualParticle;

			public PoolObjHandle<ActorRoot> AttachActor;

			public bool bUseShape;

			public VirtualParticleAttachContext(GameObject inParObj, PoolObjHandle<ActorRoot> inAttachActor, bool inUseShape)
			{
				this.VirtualParticle = inParObj;
				this.AttachActor = inAttachActor;
				this.bUseShape = inUseShape;
			}
		}

		public List<ACTOR_INFO> m_explorerPosList = new List<ACTOR_INFO>();

		private List<BULLET_INFO>[] m_explorerBulletListInternal;

		private List<GameFowCollector.VirtualParticleAttachContext> VirtualParentParticles_ = new List<GameFowCollector.VirtualParticleAttachContext>();

		private List<BULLET_INFO>[] m_explorerBulletList
		{
			get
			{
				if (this.m_explorerBulletListInternal == null)
				{
					this.m_explorerBulletListInternal = new List<BULLET_INFO>[2];
					int num = this.m_explorerBulletListInternal.Length;
					for (int i = 0; i < num; i++)
					{
						this.m_explorerBulletListInternal[i] = new List<BULLET_INFO>();
					}
				}
				return this.m_explorerBulletListInternal;
			}
		}

		public void AddVirtualParentParticle(GameObject inParObj, PoolObjHandle<ActorRoot> inAttachActor, bool inUseShape)
		{
			if (inParObj != null)
			{
				this.VirtualParentParticles_.Add(new GameFowCollector.VirtualParticleAttachContext(inParObj, inAttachActor, inUseShape));
			}
		}

		public void RemoveVirtualParentParticle(GameObject inParObj)
		{
			int count = this.VirtualParentParticles_.get_Count();
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					if (this.VirtualParentParticles_.get_Item(i).VirtualParticle == inParObj)
					{
						this.VirtualParentParticles_.RemoveAt(i);
						break;
					}
				}
			}
		}

		private void ClearVirtualParentParticles()
		{
			this.VirtualParentParticles_.Clear();
		}

		public void InitSurface()
		{
		}

		public void UninitSurface()
		{
			this.ClearExplorerPosList();
			this.ClearExplorerBulletList();
			this.ClearVirtualParentParticles();
		}

		public List<BULLET_INFO> GetExplorerBulletList(COM_PLAYERCAMP inCamp)
		{
			return this.m_explorerBulletList[HorizonMarkerByFow.TranslateCampToIndex(inCamp)];
		}

		public void ClearExplorerPosList()
		{
			int count = this.m_explorerPosList.get_Count();
			for (int i = 0; i < count; i++)
			{
				this.m_explorerPosList.get_Item(i).Release();
			}
			this.m_explorerPosList.Clear();
		}

		public void ClearExplorerBulletList()
		{
			int num = this.m_explorerBulletList.Length;
			for (int i = 0; i < num; i++)
			{
				List<BULLET_INFO> list = this.m_explorerBulletList[i];
				int count = list.get_Count();
				for (int j = 0; j < count; j++)
				{
					list.get_Item(j).Release();
				}
				list.Clear();
			}
		}

		public void CollectExplorer(bool bForce)
		{
			GameObjMgr instance = Singleton<GameObjMgr>.get_instance();
			GameFowManager instance2 = Singleton<GameFowManager>.get_instance();
			uint num = Singleton<FrameSynchr>.get_instance().CurFrameNum % instance2.InterpolateFrameInterval;
			uint num2 = Singleton<FrameSynchr>.get_instance().CurFrameNum % instance2.InterpolateFrameIntervalBullet;
			uint num3 = Singleton<FrameSynchr>.get_instance().CurFrameNum % instance2.InterpolateFrameIntervalHero;
			this.ClearExplorerPosList();
			int count = instance.GameActors.get_Count();
			for (int i = 0; i < count; i++)
			{
				PoolObjHandle<ActorRoot> poolObjHandle = instance.GameActors.get_Item(i);
				if (poolObjHandle)
				{
					ActorRoot handle = poolObjHandle.get_handle();
					ActorTypeDef actorType = handle.TheActorMeta.ActorType;
					if (actorType == ActorTypeDef.Actor_Type_Hero)
					{
						if (handle.ObjID % instance2.InterpolateFrameIntervalHero != num3 && !bForce)
						{
							goto IL_186;
						}
					}
					else if (handle.ObjID % instance2.InterpolateFrameInterval != num && !bForce)
					{
						goto IL_186;
					}
					if (actorType != ActorTypeDef.Actor_Type_Organ && (!handle.ActorControl.IsDeadState || handle.TheStaticData.TheBaseAttribute.DeadControl))
					{
						VInt3 vInt = new VInt3(handle.location.x, handle.location.z, 0);
						if (handle.HorizonMarker != null)
						{
							int[] exposedCamps = handle.HorizonMarker.GetExposedCamps();
							ACTOR_INFO aCTOR_INFO = ClassObjPool<ACTOR_INFO>.Get();
							aCTOR_INFO.camps = exposedCamps;
							aCTOR_INFO.location = handle.HorizonMarker.GetExposedPos();
							this.m_explorerPosList.Add(aCTOR_INFO);
						}
					}
				}
				IL_186:;
			}
			this.ClearExplorerBulletList();
			for (int j = 1; j < 3; j++)
			{
				List<PoolObjHandle<ActorRoot>> campBullet = Singleton<GameObjMgr>.get_instance().GetCampBullet(j);
				int count2 = campBullet.get_Count();
				for (int k = 0; k < count2; k++)
				{
					PoolObjHandle<ActorRoot> poolObjHandle2 = campBullet.get_Item(k);
					if (poolObjHandle2)
					{
						ActorRoot handle2 = poolObjHandle2.get_handle();
						BulletWrapper bulletWrapper = handle2.ActorControl as BulletWrapper;
						if (0 < bulletWrapper.SightRadius)
						{
							if (handle2.ObjID % instance2.InterpolateFrameIntervalBullet == num2 || bForce)
							{
								VInt3 location = new VInt3(handle2.location.x, handle2.location.z, 0);
								BULLET_INFO bULLET_INFO = ClassObjPool<BULLET_INFO>.Get();
								bULLET_INFO.radius = bulletWrapper.SightRange;
								bULLET_INFO.location = location;
								this.m_explorerBulletList[j - 1].Add(bULLET_INFO);
							}
						}
					}
				}
			}
		}

		public void UpdateFowVisibility(bool bForce)
		{
			GameObjMgr instance = Singleton<GameObjMgr>.get_instance();
			GameFowManager instance2 = Singleton<GameFowManager>.get_instance();
			Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
			COM_PLAYERCAMP playerCamp = hostPlayer.PlayerCamp;
			uint num = Singleton<FrameSynchr>.get_instance().CurFrameNum % instance2.InterpolateFrameInterval;
			uint num2 = Singleton<FrameSynchr>.get_instance().CurFrameNum % instance2.InterpolateFrameIntervalBullet;
			uint num3 = Singleton<FrameSynchr>.get_instance().CurFrameNum % instance2.InterpolateFrameIntervalHero;
			List<PoolObjHandle<ActorRoot>> gameActors = instance.GameActors;
			int count = gameActors.get_Count();
			for (int i = 0; i < count; i++)
			{
				if (gameActors.get_Item(i))
				{
					ActorRoot handle = gameActors.get_Item(i).get_handle();
					ActorTypeDef actorType = handle.TheActorMeta.ActorType;
					if (actorType == ActorTypeDef.Actor_Type_Hero)
					{
						if (handle.ObjID % instance2.InterpolateFrameIntervalHero != num3 && !bForce)
						{
							goto IL_212;
						}
					}
					else if (handle.ObjID % instance2.InterpolateFrameInterval != num && !bForce)
					{
						goto IL_212;
					}
					if (actorType != ActorTypeDef.Actor_Type_Organ && (!handle.ActorControl.IsDeadState || handle.TheStaticData.TheBaseAttribute.DeadControl))
					{
						VInt3 worldLoc = new VInt3(handle.location.x, handle.location.z, 0);
						if (actorType == ActorTypeDef.Actor_Type_Hero || actorType == ActorTypeDef.Actor_Type_Monster)
						{
							bool flag = instance2.QueryAttr(handle.location) == FieldObj.EViewBlockType.Grass;
							handle.HorizonMarker.SetTranslucentMark(HorizonConfig.HideMark.Jungle, flag, false);
							if (handle.HudControl != null && handle.HudControl.HasStatus(StatusHudType.InJungle) != flag)
							{
								if (flag)
								{
									handle.HudControl.ShowStatus(StatusHudType.InJungle);
								}
								else
								{
									handle.HudControl.HideStatus(StatusHudType.InJungle);
								}
							}
						}
						for (int j = 1; j < 3; j++)
						{
							COM_PLAYERCAMP cOM_PLAYERCAMP = j;
							if (cOM_PLAYERCAMP != handle.TheActorMeta.ActorCamp)
							{
								handle.HorizonMarker.SetHideMark(cOM_PLAYERCAMP, HorizonConfig.HideMark.Jungle, !instance2.IsSurfaceCellVisibleConsiderNeighbor(worldLoc, cOM_PLAYERCAMP));
							}
						}
					}
				}
				IL_212:;
			}
			Dictionary<int, ShenFuObjects>.Enumerator enumerator = Singleton<ShenFuSystem>.get_instance()._shenFuTriggerPool.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, ShenFuObjects> current = enumerator.get_Current();
				int key = current.get_Key();
				if ((long)key % (long)((ulong)instance2.InterpolateFrameInterval) == (long)((ulong)num))
				{
					KeyValuePair<int, ShenFuObjects> current2 = enumerator.get_Current();
					GameObject shenFu = current2.get_Value().ShenFu;
					GameFowCollector.SetObjVisibleByFow(shenFu, instance2, playerCamp);
				}
			}
			for (int k = 0; k < 3; k++)
			{
				if (k != playerCamp)
				{
					List<PoolObjHandle<ActorRoot>> campBullet = instance.GetCampBullet(k);
					int count2 = campBullet.get_Count();
					for (int l = 0; l < count2; l++)
					{
						PoolObjHandle<ActorRoot> poolObjHandle = campBullet.get_Item(l);
						if (poolObjHandle)
						{
							ActorRoot handle2 = poolObjHandle.get_handle();
							BulletWrapper bulletWrapper = handle2.ActorControl as BulletWrapper;
							if (bulletWrapper.m_bVisibleByFow)
							{
								if (handle2.ObjID % instance2.InterpolateFrameIntervalBullet == num2)
								{
									if (bulletWrapper.m_bVisibleByShape)
									{
										bulletWrapper.UpdateSubParObjVisibility(GameFowCollector.SetObjWithColVisibleByFow(poolObjHandle, instance2, playerCamp));
									}
									else
									{
										bulletWrapper.UpdateSubParObjVisibility(GameFowCollector.SetObjVisibleByFow(handle2.gameObject, instance2, playerCamp));
									}
								}
							}
						}
					}
				}
			}
			int count3 = this.VirtualParentParticles_.get_Count();
			for (int m = 0; m < count3; m++)
			{
				GameFowCollector.VirtualParticleAttachContext virtualParticleAttachContext = this.VirtualParentParticles_.get_Item(m);
				GameObject virtualParticle = this.VirtualParentParticles_.get_Item(m).VirtualParticle;
				if (!(virtualParticle == null))
				{
					int num4 = virtualParticle.GetInstanceID();
					if (num4 < 0)
					{
						num4 = -num4;
					}
					if ((long)num4 % (long)((ulong)instance2.InterpolateFrameIntervalBullet) == (long)((ulong)num))
					{
						if (virtualParticleAttachContext.bUseShape)
						{
							GameFowCollector.SetObjWithColVisibleByFowAttached(virtualParticle, instance2, playerCamp, this.VirtualParentParticles_.get_Item(m).AttachActor);
						}
						else
						{
							GameFowCollector.SetObjVisibleByFowAttached(virtualParticle, instance2, playerCamp, this.VirtualParentParticles_.get_Item(m).AttachActor);
						}
					}
				}
			}
		}

		public static bool SetObjVisibleByFow(GameObject obj, GameFowManager fowMgr, COM_PLAYERCAMP inHostCamp)
		{
			if (obj == null)
			{
				return false;
			}
			VInt3 worldLoc = (VInt3)obj.transform.position;
			worldLoc = new VInt3(worldLoc.x, worldLoc.z, 0);
			bool flag = fowMgr.IsSurfaceCellVisible(worldLoc, inHostCamp);
			if (flag)
			{
				MMGame_Math.SetLayer(obj, "Actor", "Particles", true);
			}
			else
			{
				MMGame_Math.SetLayer(obj, "Hide", true);
			}
			return flag;
		}

		public static bool SetObjVisibleByFowAttached(GameObject obj, GameFowManager fowMgr, COM_PLAYERCAMP inHostCamp, PoolObjHandle<ActorRoot> inAttachActor)
		{
			if (obj == null)
			{
				return false;
			}
			bool flag;
			if (inAttachActor)
			{
				VInt3 location = inAttachActor.get_handle().location;
				location = new VInt3(location.x, location.z, 0);
				flag = fowMgr.IsSurfaceCellVisible(location, inHostCamp);
				if (flag)
				{
					MMGame_Math.SetLayer(obj, "Actor", "Particles", true);
				}
				else
				{
					MMGame_Math.SetLayer(obj, "Hide", true);
				}
			}
			else
			{
				flag = GameFowCollector.SetObjVisibleByFow(obj, fowMgr, inHostCamp);
			}
			return flag;
		}

		public static bool SetObjWithColVisibleByFowAttached(GameObject inObj, GameFowManager fowMgr, COM_PLAYERCAMP inHostCamp, PoolObjHandle<ActorRoot> inAttachActor)
		{
			if (inObj == null)
			{
				return false;
			}
			if (!inAttachActor || inAttachActor.get_handle().shape == null)
			{
				return GameFowCollector.SetObjVisibleByFow(inObj, fowMgr, inHostCamp);
			}
			VCollisionShape shape = inAttachActor.get_handle().shape;
			VInt3 location = inAttachActor.get_handle().location;
			location = new VInt3(location.x, location.z, 0);
			bool flag = fowMgr.IsSurfaceCellVisible(location, inHostCamp);
			if (flag)
			{
				MMGame_Math.SetLayer(inObj, "Actor", "Particles", true);
			}
			else
			{
				flag = shape.AcceptFowVisibilityCheck(inHostCamp, fowMgr);
				if (flag)
				{
					MMGame_Math.SetLayer(inObj, "Actor", "Particles", true);
				}
				else
				{
					MMGame_Math.SetLayer(inObj, "Hide", true);
				}
			}
			return flag;
		}

		public static bool SetObjWithColVisibleByFow(PoolObjHandle<ActorRoot> inActor, GameFowManager fowMgr, COM_PLAYERCAMP inHostCamp)
		{
			if (!inActor)
			{
				return false;
			}
			ActorRoot handle = inActor.get_handle();
			VCollisionShape shape = handle.shape;
			if (shape == null)
			{
				return GameFowCollector.SetObjVisibleByFow(handle.gameObject, fowMgr, inHostCamp);
			}
			VInt3 location = handle.location;
			location = new VInt3(location.x, location.z, 0);
			bool flag = fowMgr.IsSurfaceCellVisible(location, inHostCamp);
			if (flag)
			{
				MMGame_Math.SetLayer(handle.gameObject, "Actor", "Particles", true);
			}
			else
			{
				flag = shape.AcceptFowVisibilityCheck(inHostCamp, fowMgr);
				if (flag)
				{
					MMGame_Math.SetLayer(handle.gameObject, "Actor", "Particles", true);
				}
				else
				{
					MMGame_Math.SetLayer(handle.gameObject, "Hide", true);
				}
			}
			return flag;
		}

		private static bool IsPointInCircularSector2(float cx, float cy, float ux, float uy, float squaredR, float cosTheta, float px, float py)
		{
			if (squaredR <= 0f)
			{
				return false;
			}
			float num = px - cx;
			float num2 = py - cy;
			float num3 = num * num + num2 * num2;
			if (num3 > squaredR)
			{
				return false;
			}
			float num4 = num * ux + num2 * uy;
			if (num4 >= 0f && cosTheta >= 0f)
			{
				return num4 * num4 > num3 * cosTheta * cosTheta;
			}
			if (num4 < 0f && cosTheta < 0f)
			{
				return num4 * num4 < num3 * cosTheta * cosTheta;
			}
			return num4 >= 0f;
		}

		public static bool VisitFowVisibilityCheck(VCollisionBox box, PoolObjHandle<ActorRoot> inActor, COM_PLAYERCAMP inHostCamp, GameFowManager fowMgr)
		{
			VInt2 zero = VInt2.zero;
			if (fowMgr.WorldPosToGrid(new VInt3(box.WorldPos.x, box.WorldPos.z, 0), out zero.x, out zero.y))
			{
				FieldObj pFieldObj = fowMgr.m_pFieldObj;
				int num = 0;
				int num2;
				pFieldObj.UnrealToGridX(box.WorldExtends.x, out num2);
				pFieldObj.UnrealToGridX(box.WorldExtends.z, out num);
				int num3 = zero.x - num2;
				num3 = Math.Max(0, num3);
				int num4 = zero.x + num2;
				num4 = Math.Min(num4, pFieldObj.NumX - 1);
				int num5 = zero.y - num;
				num5 = Math.Max(0, num5);
				int num6 = zero.y + num;
				num6 = Math.Min(num6, pFieldObj.NumY - 1);
				for (int i = num3; i <= num4; i++)
				{
					for (int j = num5; j <= num6; j++)
					{
						bool flag = Singleton<GameFowManager>.get_instance().IsVisible(i, j, inHostCamp);
						if (flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public static bool VisitFowVisibilityCheck(VCollisionSphere sphere, PoolObjHandle<ActorRoot> inActor, COM_PLAYERCAMP inHostCamp, GameFowManager fowMgr)
		{
			VInt2 zero = VInt2.zero;
			if (fowMgr.WorldPosToGrid(new VInt3(sphere.WorldPos.x, sphere.WorldPos.z, 0), out zero.x, out zero.y))
			{
				int num = 0;
				fowMgr.m_pFieldObj.UnrealToGridX(sphere.WorldRadius, out num);
				for (int i = -num; i <= num; i++)
				{
					for (int j = -num; j <= num; j++)
					{
						VInt2 vInt = new VInt2(i, j);
						VInt2 vInt2 = zero + vInt;
						if (fowMgr.IsInsideSurface(vInt2.x, vInt2.y))
						{
							if (vInt.get_sqrMagnitude() <= num * num)
							{
								bool flag = Singleton<GameFowManager>.get_instance().IsVisible(vInt2.x, vInt2.y, inHostCamp);
								if (flag)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public static bool VisitFowVisibilityCheck(VCollisionCylinderSector cylinder, PoolObjHandle<ActorRoot> inActor, COM_PLAYERCAMP inHostCamp, GameFowManager fowMgr)
		{
			VInt2 zero = VInt2.zero;
			if (fowMgr.WorldPosToGrid(new VInt3(cylinder.WorldPos.x, cylinder.WorldPos.z, 0), out zero.x, out zero.y))
			{
				float num = (float)cylinder.Radius;
				num *= 0.001f;
				num *= num;
				Vector3 vector = (Vector3)cylinder.WorldPos;
				float num2 = (float)cylinder.Degree;
				num2 *= 0.5f;
				num2 = Mathf.Cos(num2);
				Vector3 vector2 = (Vector3)cylinder.WorldPos;
				Vector3 vector3 = (Vector3)inActor.get_handle().forward;
				FieldObj pFieldObj = fowMgr.m_pFieldObj;
				int num3 = 0;
				pFieldObj.UnrealToGridX(cylinder.Radius, out num3);
				int num4 = zero.x - num3;
				num4 = Math.Max(0, num4);
				int num5 = zero.x + num3;
				num5 = Math.Min(num5, pFieldObj.NumX - 1);
				int num6 = zero.y - num3;
				num6 = Math.Max(0, num6);
				int num7 = zero.y + num3;
				num7 = Math.Min(num7, pFieldObj.NumY - 1);
				for (int i = num4; i <= num5; i++)
				{
					for (int j = num6; j <= num7; j++)
					{
						bool flag = Singleton<GameFowManager>.get_instance().IsVisible(i, j, inHostCamp);
						if (flag && GameFowCollector.IsPointInCircularSector2(vector.x, vector.z, vector3.x, vector3.z, num, num2, vector2.x, vector2.z))
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
