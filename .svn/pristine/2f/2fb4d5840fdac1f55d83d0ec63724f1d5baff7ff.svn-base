using UnityEngine;
using System.Collections;

/// <summary>
/// 点在扇形里面
/// </summary>
public class PointInSector : MonoBehaviour
{
    public Transform pointTransform;
    public float distance = 5f;
    public float angle = 30f;
    void Update()
    {
        Quaternion r = this.transform.rotation;
        Vector3 f0 = transform.position + (r * Vector3.forward) * distance;
        Debug.DrawLine(transform.position, f0, Color.red);//正前方

        Quaternion r0 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - angle, transform.rotation.eulerAngles.z);
        Vector3 f1 = transform.position + (r0 * Vector3.forward) * distance;
        Debug.DrawLine(transform.position, f1, Color.red);//左30度方向

        Quaternion r1 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + angle, transform.rotation.eulerAngles.z);
        Vector3 f2 = transform.position + (r1 * Vector3.forward) * distance;//右30度方向
        Debug.DrawLine(transform.position, f2, Color.red);

        Debug.DrawLine(f1, f2, Color.red);//连接成一个三角形

        Vector3 point = pointTransform.position;

        Vector3 selfVec = (f0 - this.transform.position).normalized;
        Vector3 targetVec = (pointTransform.position - this.transform.position).normalized;
        Debug.DrawLine(pointTransform.position, this.transform.position, Color.yellow);

        //在左右30度以内
        if (Vector3.Dot(selfVec, targetVec) > Mathf.Cos(Mathf.PI / 6.0f))
        {
            //且在周围distance范围以内，等价于在这个扇形区域！
            if ((pointTransform.position - this.transform.position).sqrMagnitude < distance * distance)
            {
                Debug.Log("in");
            }
            else
            {
                Debug.Log("not in");
            }
        }
        else
        {
            Debug.Log("out of angle");
        }
    }
}
