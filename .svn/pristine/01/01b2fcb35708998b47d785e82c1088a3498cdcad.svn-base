using UnityEngine;
using System.Collections;


/// <summary>
/// 假设主角看到的是向左30度，向右30度在这个区域,距离5米
/// </summary>
public class test4 : MonoBehaviour
{

    float distance = 5f;

    void Start()
    {

    }

    void Update()
    {
        Quaternion r = transform.rotation;
        Vector3 f0 = (transform.position + (r * Vector3.forward) * distance);
        Debug.DrawLine(transform.position, f0, Color.red);

        Quaternion r0 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 30f, transform.rotation.eulerAngles.z);
        Quaternion r1 = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 30f, transform.rotation.eulerAngles.z);

        Vector3 f1 = (transform.position + (r0 * Vector3.forward) * distance);
        Vector3 f2 = (transform.position + (r1 * Vector3.forward) * distance);

        Debug.DrawLine(transform.position, f1, Color.red);
        Debug.DrawLine(transform.position, f2, Color.red);

        Debug.DrawLine(f0, f1, Color.blue);
        Debug.DrawLine(f0, f2, Color.blue);
    }
}
