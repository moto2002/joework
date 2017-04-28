using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform targeTransform;
    private Transform selfTransform;
    public float x, y, z;

    void Start()
    {
        selfTransform = this.transform;
        // selfTransform.LookAt(targeTransform);
    }

    void LateUpdate()
    {
        GetCameraPos();
    }

    private void GetCameraPos()
    {
        Vector3 newTagetVector3 = new Vector3(targeTransform.position.x + x, targeTransform.position.y + y,
            targeTransform.position.z + z);
        selfTransform.position = Vector3.Lerp(selfTransform.position, newTagetVector3, Time.deltaTime * 5);
    }
}
