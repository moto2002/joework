using UnityEngine;
using System.Collections;
using Geome;

public class RectCollisionRectTest : MonoBehaviour
{
    public Transform rectTrans1;
    public Transform rectTrans2;

    Rectangle rect1;
    Rectangle rect2;

    void Start()
    {
        this.rect1 = new Rectangle(this.rectTrans1, 2f, 4f);
        this.rect2 = new Rectangle(this.rectTrans2, 2f, 2f);
    }

    void Update()
    {
        this.rectTrans1.Rotate(new Vector3(0, 1f, 0));
        this.rectTrans2.Rotate(new Vector3(0, 1f, 0));
    }

    void OnDrawGizmos()
    {
        if (this.rect1 != null && this.rect2 != null)
        {

            if (Collision2DTools.Collision2D(this.rect1, this.rect2))
            {
                this.rect1.Draw(Color.red);
                this.rect2.Draw(Color.red);
            }
            else
            {
                this.rect1.Draw(Color.cyan);
                this.rect2.Draw(Color.cyan);
            }

        }

    }
}
