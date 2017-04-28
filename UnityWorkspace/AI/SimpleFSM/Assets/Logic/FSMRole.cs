using System;
using System.Collections;

namespace ShiHuanJue.FSM
{
    static public class  RoleStateSet 
    {
        static public RoleIdle stateIdle = new RoleIdle();
        static public RoleWalking stateWalking = new RoleWalking();
        static public RoleDead stateDead = new RoleDead();
        static public RoleAttacking stateAttackint = new RoleAttacking();
    }

    public class FSMRole : FSMParent
    {
        public FSMRole()
        {
            theFSM.Add(MotionState.IDLE, RoleStateSet.stateIdle);
            theFSM.Add(MotionState.WALKING, RoleStateSet.stateWalking);
            theFSM.Add(MotionState.DEAD, RoleStateSet.stateDead);
            theFSM.Add(MotionState.ATTACKING, RoleStateSet.stateAttackint);
        }

        public override void ChangeStatus(string newState, params Object[] args)
        {
            base.ChangeStatus(newState,args);
        }
    }
}