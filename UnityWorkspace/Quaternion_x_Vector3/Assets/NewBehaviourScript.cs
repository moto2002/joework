using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour
{

    public Transform target;

    private Vector3 defaultDir = Vector3.forward;
    private CharacterController ac;
    void Start()
    {
        ac = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 rot = target.rotation.eulerAngles;
        Vector3 dir = Quaternion.Euler(new Vector3(0, rot.y, 0)) * defaultDir;
        //this.transform.Translate(dir.normalized * 1 * Time.deltaTime);
        //ac.Move(dir.normalized*5*Time.deltaTime);
        ac.SimpleMove(dir.normalized * 5);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "forward"))
        {
            defaultDir = Vector3.forward;
        }

        if (GUI.Button(new Rect(10, 70, 100, 50), "left"))
        {
            defaultDir = Vector3.left;
        }

        if (GUI.Button(new Rect(10, 130, 100, 50), "right"))
        {
            defaultDir = Vector3.right;
        }

        if (GUI.Button(new Rect(10, 190, 100, 50), "back"))
        {
            defaultDir = Vector3.back;
        }

        if (GUI.Button(new Rect(10, 250, 100, 50), "stop"))
        {
            defaultDir = Vector3.zero;
        }
    }
}
