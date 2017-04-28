using Com.Duoyu001.Pool;
using UnityEngine;
using System.Collections;

public class TestBullet : MonoBehaviour, IAutoRestoreChecker
{
    public delegate void OnLifeEnd(GameObject go);

    private Vector3 force;
    private Vector3 velocity;
    private float life;
    private OnLifeEnd onLifeEnd;

    public void Init(float force, float initLife, float initVelocity, OnLifeEnd onLifeEnd, float angle = 0)
    {
        angle = Mathf.Deg2Rad * (90+angle);
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.up * angle);
        Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        this.force = direction.normalized * force;
        this.life = initLife;
        this.velocity = direction.normalized * initVelocity;
        this.onLifeEnd = onLifeEnd;
    }

    public void Update()
    {
        velocity += force * Time.deltaTime;
        gameObject.transform.position += velocity * Time.deltaTime;
        life -= Time.deltaTime;
        if (life <= 0)
        {
            if (onLifeEnd != null)
            {
                onLifeEnd(gameObject);
                onLifeEnd = null;
            }
        }
    }

    public bool Restore {
        get
        {
            return life <= 0;
        }
    }
}
