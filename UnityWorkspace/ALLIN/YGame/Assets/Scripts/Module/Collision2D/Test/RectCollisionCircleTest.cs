using UnityEngine;
using System.Collections;
using Geome;

public class RectCollisionCircleTest : MonoBehaviour
{

    public Transform rectTrans;
    public Transform circleTrans;

    Rectangle rect;
    Circle circle;

    void Start()
    {
        this.rect = new Rectangle(this.rectTrans, 2f, 4f);
        this.circle = new Circle(this.circleTrans,1f);
    }

    void Update()
    {
        this.rectTrans.Rotate(new Vector3(0,1f,0));
    }

    void OnDrawGizmos()
    {
        if (this.rect != null && this.circle != null)
        {
            if (Collision2DTools.Collision2D(rect, circle))
            {
                this.rect.Draw(Color.red);
                this.circle.Draw(Color.red);
            }
            else
            {
                this.rect.Draw(Color.cyan);
                this.circle.Draw(Color.cyan);
            }
        }
    }
}
