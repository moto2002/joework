using UnityEngine;
using System.Collections;
using Geome;

public class Test : MonoBehaviour
{
    Circle circle1, circle2;
    public Transform c1;
    public Transform c2;

    void Start()
    {
        circle1 = new Circle(c1, 3f);
        circle2 = new Circle(c2, 1f);
    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        if (circle1 != null && circle2 != null)
        {
            if (Collision2DTools.Collision2D(circle1, circle2))
            {
                circle1.Draw(Color.red);
                circle2.Draw(Color.red);
            }
            else
            {
                circle1.Draw(Color.cyan);
                circle2.Draw(Color.cyan);
            }
       
        }

    }
}
