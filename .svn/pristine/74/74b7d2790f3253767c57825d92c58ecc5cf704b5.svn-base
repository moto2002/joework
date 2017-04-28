using UnityEngine;
using System.Collections;

/// <summary>
/// 已知一个目标点，让模型朝着这个目标点移动。
/// </summary>
public class test3 : MonoBehaviour
{

    public Transform Target;

    void LateUpdate()
    {
        Target.transform.LookAt(new Vector3(100f,200f,300f));
        Target.Translate(Vector3.forward);

        //TargetCube与Target的方向，然后沿y轴旋转30度，然后以1次平移1分米平移
        /*
        Vector3 vecn = (TargetCube.position - Target.position).normalized;
        vecn = Quaternion.Euler(0f, 30f, 0f) * vecn;
        Target.Translate(vecn * 0.1f);
         */
    }




}
