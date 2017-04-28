using UnityEngine;
using System.Collections;
using System;

public class test : MonoBehaviour
{
    private Animator m_animator;
    private Transform m_transform;
    private float m_moveSpeed;
    void Start()
    {
        this.m_animator = this.GetComponent<Animator>();
        this.m_transform = this.transform;
        this.m_moveSpeed = 5f;


        CGameEventMgr.Inst().AddListener("JoystickMoveStart", On_JoystickMoveStart);
        CGameEventMgr.Inst().AddListener("JoystickMove", On_JoystickMove);
        CGameEventMgr.Inst().AddListener("JoystickMoveEnd", On_JoystickMoveEnd);
    }

    private void On_JoystickMoveEnd(CGameEvent _CGameEvent)
    {
        this.Idle();
    }

    private void On_JoystickMoveStart(CGameEvent _CGameEvent)
    {
        this.Move();
    }

    private void On_JoystickMove(CGameEvent _CGameEvent)
    {
        Vector3 temp = (Vector3)_CGameEvent.data;

        //位置的移动
        Vector3 move = temp * Time.deltaTime * this.m_moveSpeed;
        this.m_transform.localPosition += move;

        //从JoytackController移动方向 算出自身的角度
        float angle = Mathf.Atan2(temp.x, temp.z) * Mathf.Rad2Deg;
        this.m_transform.localRotation = Quaternion.Euler(Vector3.up * angle);
    }

    void Update()
    {
        
    }

    public void Move()
    {
        this.m_animator.Play("run");
    }

    public void Idle()
    {
        this.m_animator.Play("idle1");
    }
}
