using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//状态管理类，对状态进行管理
public class FSM_Manager
{
    private List<FSM_State> states;//存储所有状态的的List

    private FSM_State currentState;  //当前状态
    public FSM_State CurrentState
    {
        set { currentState = value; }
        get { return currentState; }
    }

    private StateID currentStateID;//当前状态ID
    public StateID CurrentStateID
    {
        set { currentStateID = value; }
        get { return currentStateID; }
    }

    private static FSM_Manager instance;
    public static FSM_Manager GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new FSM_Manager(); 
              /*  GameObject n = new GameObject();
                n.name = "FSM_Manager";
                instance = n.AddComponent<FSM_Manager>() as FSM_Manager;*/
            }

            return instance;
        }

    }

    public FSM_Manager()
    {
        states = new List<FSM_State>();   //初始化
    }

    public void AddFsmState(FSM_State s)//添加状态
    {
        if (s == null)
        {
            Debug.LogError(" Null reference is not allowed");
        }
        if (states.Count == 0)
        {
            states.Add(s);                   //设置默认状态;
            currentState = s;
            currentStateID = s.ID;
            return;
        }
        foreach (FSM_State state in states)
        {
            if (state == s)
            {
                Debug.LogError(s.ID.ToString() + "has already been added");
                return;
            }
        }
        states.Add(s);
    }

    public void DelateFsmState(StateID id)//移除状态
    {
        if (id == StateID.NullStateId)
        {
            Debug.LogError("NullStateID is not allowed for a real state");
            return;
        }

        foreach (FSM_State state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
               ". It was not on the list of states");
    }

    public void ChangeFsmState(Transition tr)//状态更改,performTransition
    {
        if (tr == Transition.NullTransition)
        {
            Debug.LogError("NullTransition is not allowed for a real transition");
            return;
        }
        StateID id = CurrentState.GetOutPutState(tr);   //当前状态会进入的新的状态
        currentStateID = id;
        foreach (FSM_State state in states)          //通过添加的所有状态，进行搜索来获取要进入的状态实例
        {
            if (currentStateID == state.ID)
            {
                CurrentState.DoBeforeMoveing();     //退出状态前，留下点什么，比如挥舞下手臂
                currentState = state;
                CurrentState.DoBeforeEnter();     //进入状态
                break;
            }
        }
    }


}
