using UnityEngine;
using System.Collections;

/// <summary>
/// 点在矩形里面
/// </summary>
public class PointInRectangle : MonoBehaviour
{
    public Transform pointTransform;
    public float length = 5f;
    public float width = 5f;

    private float half_length;
    private float half_width;

    void Start()
    {
        this.half_length = this.length / 2.0f;
        this.half_width = this.width / 2.0f;
    }

    void Update()
    {
        Quaternion r = transform.rotation;
        Vector3 left = (transform.position + (r * Vector3.left) * this.half_length);
        Debug.DrawLine(transform.position, left, Color.red);

        Vector3 right = (transform.position + (r * Vector3.right) * this.half_length);
        Debug.DrawLine(transform.position, right, Color.red);

        Vector3 leftEnd = (left + (r * Vector3.forward) * this.width);
        Debug.DrawLine(left, leftEnd, Color.red);

        Vector3 rightEnd = (right + (r * Vector3.forward) * this.width);
        Debug.DrawLine(right, rightEnd, Color.red);

        Debug.DrawLine(leftEnd, rightEnd, Color.red);

        Vector3 point = pointTransform.position;

        //是否在矩形内
        if (MyTool.isINRect(point, leftEnd, rightEnd, right, left))
        {
            Debug.Log("in");
        }
        else
        {
            Debug.Log("not in");
        }
    }
}
