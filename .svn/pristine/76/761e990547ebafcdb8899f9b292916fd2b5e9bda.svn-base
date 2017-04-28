using UnityEngine;
using System.Collections;
using ShiHuanJue.FSM;

public class TestFSM : MonoBehaviour
{
    void Start()
    {
        FSMRole roleAI = new FSMRole();
        Debug.Log(roleAI.m_currentState);

        roleAI.ChangeStatus(MotionState.IDLE);
        Debug.Log(roleAI.m_currentState);

        roleAI.ChangeStatus(MotionState.WALKING);
        Debug.Log(roleAI.m_currentState);

        roleAI.ChangeStatus(MotionState.IDLE);
        Debug.Log(roleAI.m_currentState);
    }

    void Update()
    {

    }
}
