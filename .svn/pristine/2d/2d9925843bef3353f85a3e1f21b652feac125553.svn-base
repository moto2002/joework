using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class RestVRState : MonoBehaviour
{

    private bool Once = true;
    public Transform TrCamera;
    // Use this for initialization
    void Start()
    {
        InputTracking.Recenter();
    }

    // Update is called once per frame
    void Update()
    {

        if (Once)
        {
            if (null == TrCamera)
            {
                return;
            }
            if (VRDevice.isPresent)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x - TrCamera.localEulerAngles.x, transform.eulerAngles.y - TrCamera.localEulerAngles.y, transform.eulerAngles.z - TrCamera.localEulerAngles.z);
                Once = false;
            }
        }

    }

    public void Rest()
    {
        Debug.Log("rest");
        InputTracking.Recenter();
        Once = false;
    }
}