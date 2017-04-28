using UnityEngine;
using System.Collections;

/// <summary>
/// 这里我以角色左右个30度。 这样就可以根据两个模型的距离以及角度来判断了。
/// </summary>
public class test7 : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        Quaternion right = transform.rotation * Quaternion.AngleAxis(30, Vector3.up);
        Quaternion left = transform.rotation * Quaternion.AngleAxis(30, Vector3.down);

        Vector3 n = transform.position + (Vector3.forward * distance);
        Vector3 leftPoint = left * n;
        Vector3 rightPoint = right * n;

        Debug.DrawLine(transform.position, leftPoint, Color.red);
        Debug.DrawLine(transform.position, rightPoint, Color.red);
        Debug.DrawLine(rightPoint, leftPoint, Color.red);
    }
}
