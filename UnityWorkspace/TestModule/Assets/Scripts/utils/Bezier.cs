using UnityEngine;
using System.Collections;

/*
[System.Serializable]
public class Bezier : System.Object
{

    public Vector3 p0;

    public Vector3 p1;

    public Vector3 p2;

    public Vector3 p3;

    public float ti = 0f;

    private Vector3 b0 = Vector3.zero;

    private Vector3 b1 = Vector3.zero;

    private Vector3 b2 = Vector3.zero;

    private Vector3 b3 = Vector3.zero;

    private float Ax;

    private float Ay;

    private float Az;

    private float Bx;

    private float By;

    private float Bz;

    private float Cx;

    private float Cy;

    private float Cz;

    // Init function v0 = 1st point, v1 = handle of the 1st point , v2 = handle of the 2nd point, v3 = 2nd point

    // handle1 = v0 + v1

    // handle2 = v3 + v2

    public Bezier(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {

        this.p0 = v0;

        this.p1 = v1;

        this.p2 = v2;

        this.p3 = v3;

    }

    public void UpdateTargetPos(Vector3 v3)
    {
        this.p3 = v3;
    }
    public void UpdateTargetPos(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {

        this.p0 = v0;

        this.p1 = v1;

        this.p2 = v2;

        this.p3 = v3;
    }

    // 0.0 >= t <= 1.0
    public Vector3 GetPointAtTime(float t)
    {

        this.CheckConstant();

        float t2 = t * t;

        float t3 = t * t * t;

        float x = this.Ax * t3 + this.Bx * t2 + this.Cx * t + p0.x;

        float y = this.Ay * t3 + this.By * t2 + this.Cy * t + p0.y;

        float z = this.Az * t3 + this.Bz * t2 + this.Cz * t + p0.z;

        return new Vector3(x, y, z);

    }

    private void SetConstant()
    {

        this.Cx = 3f * ((this.p0.x + this.p1.x) - this.p0.x);

        this.Bx = 3f * ((this.p3.x + this.p2.x) - (this.p0.x + this.p1.x)) - this.Cx;

        this.Ax = this.p3.x - this.p0.x - this.Cx - this.Bx;

        this.Cy = 3f * ((this.p0.y + this.p1.y) - this.p0.y);

        this.By = 3f * ((this.p3.y + this.p2.y) - (this.p0.y + this.p1.y)) - this.Cy;

        this.Ay = this.p3.y - this.p0.y - this.Cy - this.By;

        this.Cz = 3f * ((this.p0.z + this.p1.z) - this.p0.z);

        this.Bz = 3f * ((this.p3.z + this.p2.z) - (this.p0.z + this.p1.z)) - this.Cz;

        this.Az = this.p3.z - this.p0.z - this.Cz - this.Bz;

    }

    // Check if p0, p1, p2 or p3 have changed
    private void CheckConstant()
    {

        if (this.p0 != this.b0 || this.p1 != this.b1 || this.p2 != this.b2 || this.p3 != this.b3)
        {

            this.SetConstant();

            this.b0 = this.p0;

            this.b1 = this.p1;

            this.b2 = this.p2;

            this.b3 = this.p3;

        }

    }

}
*/

public class Bezier
{
    private Vector3 p0 = Vector3.zero;
    private Vector3 p1 = Vector3.zero;
    private Vector3 p2 = Vector3.zero;


    public Bezier(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        this.p0 = v0;
        this.p1 = v1;
        this.p2 = v2;
    }

    public void UpdateTargetPos(Vector3 v2)
    {
        p2 = v2;
    }

    public Vector3 GetPointAtTime(float t)
    {
        float x = (1 - t) * (1 - t) * p0.x + 2 * t * (1 - t) * p1.x + t * t * p2.x;
        float y = (1 - t) * (1 - t) * p0.y + 2 * t * (1 - t) * p1.y + t * t * p2.y;
        float z = (1 - t) * (1 - t) * p0.z + 2 * t * (1 - t) * p1.z + t * t * p2.z;
        return new Vector3(x, y, z);
    }


}
