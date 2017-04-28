using UnityEngine;
using System.Collections;
/// <summary>
/// 
/// 其实可以用一个三角形，只是正对的目标点会出现一定偏差，其实影响不大
/// 
/// 为什么不直接算角度？然后判断距离，得出来的结果是扇形区域，更精确一点
/// </summary>
public class test6 : MonoBehaviour
{

    public Transform cube;
    float distance = 5f;

    void Start()
    {

    }


    void Update()
    {
        Quaternion r = this.transform.rotation;
        Vector3 f0 = transform.position + (r * Vector3.forward) * distance;
        Debug.DrawLine(transform.position,f0,Color.red);

        Quaternion r0 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 30f, transform.rotation.eulerAngles.z);
        Quaternion r1 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 30f, transform.rotation.eulerAngles.z);

        Vector3 f1 = transform.position + (r0 * Vector3.forward) * distance;
        Vector3 f2 = transform.position + (r1 * Vector3.forward) * distance;

        Debug.DrawLine(transform.position, f1, Color.red);
        Debug.DrawLine(transform.position, f2, Color.red);
        Debug.DrawLine(f1, f2, Color.red);

        //Vector3 point = cube.position;
        ////点是否在一个三角形内
        //if (isINTriangle(point, transform.position, f1, f2))
        //{
        //    Debug.Log("in");
        //}
        //else
        //{
        //    Debug.Log("not in");
        //}

        Vector3 selfVec = (f0 - this.transform.position).normalized;
        Vector3 targetVec = (cube.position - this.transform.position).normalized;
        Debug.DrawLine(cube.position,this.transform.position,Color.yellow);
        //左右30度
        if (Vector3.Dot(selfVec, targetVec) > Mathf.Cos(Mathf.PI / 6.0f))
        {
            if ((cube.position - this.transform.position).sqrMagnitude < 5f * 5f)
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


    private float triangleArea(float v0x, float v0y, float v1x, float v1y, float v2x, float v2y)
    {
        return Mathf.Abs((v0x * v1y + v1x * v2y + v2x * v0y
            - v1x * v0y - v2x * v1y - v0x * v2y) / 2f);
    }
    private bool isINTriangle(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        float x = point.x;
        float y = point.z;

        float v0x = v0.x;
        float v0y = v0.z;

        float v1x = v1.x;
        float v1y = v1.z;

        float v2x = v2.x;
        float v2y = v2.z;

        float t = triangleArea(v0x, v0y, v1x, v1y, v2x, v2y);
        float a = triangleArea(v0x, v0y, v1x, v1y, x, y) + triangleArea(v0x, v0y, x, y, v2x, v2y) + triangleArea(x, y, v1x, v1y, v2x, v2y);

        if (Mathf.Abs(t - a) <= 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
