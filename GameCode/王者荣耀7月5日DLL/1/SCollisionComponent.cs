using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using System;
using UnityEngine;

public class SCollisionComponent : MonoBehaviour
{
	public CollisionShapeType shapeType = CollisionShapeType.Sphere;

	[HideInInspector, SerializeField]
	public VInt3 Pos = VInt3.zero;

	[HideInInspector, SerializeField]
	public VInt3 Size = new VInt3(500, 500, 500);

	[HideInInspector, SerializeField]
	public VInt3 Size2 = new VInt3(0, 0, 0);

	public VCollisionShape CreateShape()
	{
		DebugHelper.Assert(!Singleton<BattleLogic>.get_instance().isFighting || Singleton<GameLogic>.get_instance().bInLogicTick || Singleton<FrameSynchr>.get_instance().isCmdExecuting);
		VCollisionShape result = null;
		switch (this.shapeType)
		{
		case CollisionShapeType.Box:
			result = new VCollisionBox
			{
				Size = this.Size,
				Pos = this.Pos
			};
			break;
		case CollisionShapeType.Sphere:
			result = new VCollisionSphere
			{
				Pos = this.Pos,
				Radius = this.Size.x
			};
			break;
		case CollisionShapeType.CylinderSector:
		{
			VCollisionCylinderSector vCollisionCylinderSector = new VCollisionCylinderSector();
			vCollisionCylinderSector.Pos = this.Pos;
			vCollisionCylinderSector.Radius = this.Size.x;
			vCollisionCylinderSector.Height = this.Size.y;
			vCollisionCylinderSector.Degree = this.Size.z;
			vCollisionCylinderSector.Rotation = this.Size2.x;
			break;
		}
		}
		return result;
	}
}
