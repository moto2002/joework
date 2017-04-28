using UnityEngine;
using System.Collections;

public class boxcollisionbox : MonoBehaviour
{

    public Transform box1Trans;
    public Transform box2Trans;
    Rectangle rect1,rect2;

    void Start()
    {
        rect1 = new Rectangle(5f, 10f, new Vector2(box1Trans.position.x, box1Trans.position.z));
        rect2 = new Rectangle(5f, 5f, new Vector2(box2Trans.position.x, box2Trans.position.z));
    }

    void Update()
    {
   

        rect1.Refresh(new Vector2(box1Trans.position.x, box1Trans.position.z), box1Trans.rotation);
        rect2.Refresh(new Vector2(box2Trans.position.x, box2Trans.position.z), box2Trans.rotation);

        if (IntersectionDetection.Intersect(rect1, rect2))
        {
            Debug.Log("in");
        }
        else
        {
            Debug.Log("not in");
        }

        box1Trans.Rotate(new Vector3(0, 0.5f, 0));
        box2Trans.Rotate(new Vector3(0, 0.5f, 0));

    }
}
