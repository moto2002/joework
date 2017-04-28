using UnityEngine;
using System.Collections;

/// <summary>
/// 已知当前点为Target，目标点沿着Target的Y轴旋转30度，
/// 沿着Target的X轴延伸10米求目标点的3D坐标？
/// </summary>
public class test2 : MonoBehaviour
{
    public Transform Target;

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0, 30f, 0) * Target.rotation;
        Vector3 newPos = rotation * new Vector3(10f, 0, 0);

        Debug.DrawLine(newPos,Target.position, Color.red);
        Debug.Log("newpos" + newPos + "nowpos" + Target.position + "distance" + Vector3.Distance(newPos, Target.position));
    }
}
