using System;

namespace ShiHuanJue.FSM
{
    public interface IState
    {
        // 进入该状态
        void Enter(params Object[] args);

        // 离开状态
        void Exit(params Object[] args);

        // 状态处理
        void Process(params Object[] args);
    }
}