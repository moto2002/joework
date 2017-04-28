using UnityEngine;
using System.Collections;
using System;

public class CharacterMove : MonoBehaviour
{
    private Vector3 m_dir = Vector3.zero;
    private float m_moveSpeed = 5f;
    private bool m_canMove = false;
    private Transform m_target;
    private Action m_callback;
    private float dis = 2f;
    private CharacterController characterController;
    private Transform head;
    private bool isCanMove = true;
    void Start()
    {
        head = Camera.main.transform;
        characterController = this.gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isCanMove)
        {
            DoMove();
        }

        CheckNearTarget();
    }

    private void DoMove()
    {
        Vector3 dir = m_dir;
        float speed = m_moveSpeed;
        Vector3 rotate = head.rotation.eulerAngles;
        dir = Quaternion.Euler(0, rotate.y, 0) * dir;
        characterController.SimpleMove(dir * speed);//自动应用重力
    }

    private void CheckNearTarget()
    {
        if (m_target == null)
            return;

        float dis = (this.transform.position - this.m_target.position).sqrMagnitude;
        if (dis <= this.dis)
        {
            if (m_callback != null)
            {
                m_callback();
                m_callback = null;
                m_target = null;
            }
        }
    }

    #region api
    /// <summary>
    /// 更新移动方向
    /// </summary>
    /// <param name="_dir"></param>
    public void UpdateMoveDir(Vector2 _dir)
    {
        //float y = this.transform.position.y;
        Vector3 dir = new Vector3(_dir.x, 0, _dir.y);
        m_dir = dir;
    }

    /// <summary>
    /// 设置靠近某目标的回调
    /// </summary>
    /// <param name="target"></param>
    /// <param name="callback"></param>
    public void SetCallback(Transform target, Action callback)
    {
        m_target = target;
        m_callback = callback;
    }

    public void SetCanMove(bool _canMove)
    {
        isCanMove = _canMove;
    }

    #endregion

}
