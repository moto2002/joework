using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/*无论是NPC还是Player都有很多状态，在状态切换的时候为了使得程序显得清晰明了，采用了状态机制
首先实际状态都是继承自某个状态抽象类的，这个抽象类定义了进入，退出一个状态虚方法
定义了检测环境是否发生状态改变，以及在该状态下执行的动作的抽象方法
 该实例主要涉及到一个NPC在指定的位置进行巡逻，当看到Player的时候切换状态进入追逐Player状态*/

public enum Transition    //如果进入一个新的状态，需要一些触发，比如NPC看到了Player，由巡逻状态进入跟踪状态
{
    NullTransition,
    SeePlayerTrans,
    LosePlayerTrans,
    testSeeOthers,
    testLoseOthers

}

public enum StateID     //每个状态都应该有一个ID,作为识别改状态的标志
{
    NullStateId,
    ChaseingplayerId,
    FollowPathId,
    testAttackOthers,
    testAgainMove

}

public abstract class FSM_State    //定一个抽象类
{
    private Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();    
    //在某一状态下，事件引起了触发进入另一个状态
    // 于是我们定义了一个字典，存储的便是触发的类型，以及对应要进入的状态

    private StateID id;            //定一个状态ID作为变量来标识
    public StateID ID
    {
        set { id = value; }
        get { return id; }
    }


    public void AddTransition(Transition tr, StateID id)  //向字典里添加
    {
        if (tr == Transition.NullTransition)
        {
            Debug.LogError("Null Trans is not allower to adding into");
            return;
        }

        if (ID == StateID.NullStateId)
        {
            Debug.LogError("Null State id not ~~~");
            return;

        }
        if (map.ContainsKey(tr))              //NPC  任何时候都只能出于一种状态，所以一旦定义了一个触发的枚举类型，对应的只能是接下来的一种状态
        {
            Debug.LogError(id.ToString() + "is already added to");
            return;
        }

        map.Add(tr, id);
    }



    public void DeleateTransition(Transition tr) //删除字典里存储的一个状态
    {
        if (tr == Transition.NullTransition)
        {
            Debug.LogError("TransNull is not allowed to delate");
            return;
        }
        if (map.ContainsKey(tr))
        {

            map.Remove(tr);
            return;
        }
        Debug.LogError(tr.ToString() + "are not exist");
    }

    public StateID GetOutPutState(Transition tr)  //由状态的触发枚举类型返回一个对应的状态类型
    {
        if (map.ContainsKey(tr))
        {
            // Debug.Log("Translate " + tr + "State" + map[tr]);
            return map[tr];
        }
        return StateID.NullStateId;
    }

    public virtual void DoBeforeEnter() { }
    public virtual void DoBeforeMoveing() { }

    public abstract void Reason(GameObject player, GameObject NPC); //什么原因导致转换状态
    public abstract void Act(GameObject player, GameObject NPC);//在该状态下该做什么事


}