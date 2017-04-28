using UnityEngine;
using System.Collections;
using Geome;

public class SectorCollisionCircleTest : MonoBehaviour
{
    public Transform sectorTrans;
    public Transform circleTrans;

    Sector sector;
    Circle circle;

    void Start()
    {
        this.sector = new Sector(this.sectorTrans, 3f, 90f);
        this.circle = new Circle(this.circleTrans, 1f);
    }

    void Update()
    {
        this.sectorTrans.Rotate(new Vector3(0, 1f, 0));
    }

    void OnDrawGizmos()
    {
        if (this.sector != null && this.circle != null)
        {
            if (Collision2DTools.Collision2D(this.sector, this.circle))
            {
                this.sector.Draw(Color.red);
                this.circle.Draw(Color.red);
            }
            else
            {
                this.sector.Draw(Color.cyan);
                this.circle.Draw(Color.cyan);
            }
        }
    }
}
