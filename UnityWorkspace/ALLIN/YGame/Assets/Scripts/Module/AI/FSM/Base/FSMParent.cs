using System;
using System.Collections;
using System.Collections.Generic;

namespace ShiHuanJue.FSM
{
    public abstract class FSMParent
    {
        #region 公共变量

        public string m_currentState = string.Empty;

        #endregion

        #region 私有变量

        protected Dictionary<string, IState> theFSM = new Dictionary<string, IState>();

        #endregion

        public FSMParent()
        {
        }

        public virtual void ChangeStatus(string newState, params Object[] args)
        {
            if (this.m_currentState == newState)
            {
                UnityEngine.Debug.Log("oldState = newState");
                return;
            }

            if (!this.theFSM.ContainsKey(newState))
            {
                UnityEngine.Debug.Log("newState is not in stateList");
                return;
            }

            if(this.theFSM.ContainsKey(this.m_currentState))
                this.theFSM[this.m_currentState].Exit(args);
            this.theFSM[newState].Enter(args);
            this.m_currentState = newState;//更改当前状态
            this.theFSM[newState].Process(args);
        }
    }
}