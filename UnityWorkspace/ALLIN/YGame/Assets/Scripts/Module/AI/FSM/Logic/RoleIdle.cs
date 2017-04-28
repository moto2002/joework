using UnityEngine;
using System.Collections;

namespace ShiHuanJue.FSM
{
    public class RoleIdle : IState
    {
        public void Enter(params System.Object[] args)
        {
            Debug.Log("RoleIdle Enter");
        }

        public void Exit(params System.Object[] args)
        {
            Debug.Log("RoleIdle Exit");
        }

        public void Process(params System.Object[] args)
        {
            Debug.Log("RoleIdle Process");
        }
    }
}