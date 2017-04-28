using Com.Duoyu001.Pool;
using Com.Duoyu001.Pool.U3D;
using UnityEngine;

public class TestBoom
{
    private U3DAutoRestoreObjectPool pool;
    private float duration;
    private Vector3 position;
    private float angle;
    private float boomUnitForce;
    private float boomUnitInitLife;
    private float boomUnitVelocity;

    private int num;
    private float nextCanCastTime;
    private float currentAngle;

    public TestBoom(int num, float circleNum, U3DAutoRestoreObjectPool pool, float duration, Vector3 position, float boomUnitForce, float boomUnitInitLife, float boomUnitVelocity)
    {
        this.num = num;
        this.pool = pool;
        this.duration = duration;
        this.position = position;
        
        this.boomUnitForce = boomUnitForce;
        this.boomUnitInitLife = boomUnitInitLife;
        this.boomUnitVelocity = boomUnitVelocity;
        angle = 360*circleNum/num;
    }

    public void Update(float time)
    {
        if (num == 0)
        {
            return;
        }

        if (time >= nextCanCastTime)
        {
            nextCanCastTime = time + duration;
            num--;

            IAutoRestoreObject<GameObject> restoreObject = pool.Take();
            GameObject go = restoreObject.Get();
            go.transform.position = position;
            TestBullet bullet = go.GetComponent<TestBullet>();
            bullet.Init(boomUnitForce, boomUnitInitLife, boomUnitVelocity, delegate(GameObject gameObject) { }, currentAngle);
            restoreObject.Restore = bullet;
            currentAngle += angle;
        }
    }

    public bool End
    {
        get { return num == 0; }
    }
}