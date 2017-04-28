using UnityEngine;
using System.Collections;

/// <summary>
/// 已知3D模型Target，Y轴旋转30度后向前平移。
/// </summary>
public class test1 : MonoBehaviour
{
    public Transform Target;

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Quaternion rotation = Quaternion.Euler(0, 30f, 0) * Target.rotation;
            Vector3 newPos = rotation * Vector3.forward;
            Target.Translate(newPos.x,newPos.y,newPos.z);
        }
    }
}
