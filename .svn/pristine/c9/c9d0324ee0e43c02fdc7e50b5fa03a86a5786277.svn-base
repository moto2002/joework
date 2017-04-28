using UnityEngine;
using System.Collections.Generic;
using Com.Duoyu001.Pool;
using Com.Duoyu001.Pool.U3D;

public class TestCastBullet : MonoBehaviour
{
    /// <summary>
    /// 开火口
    /// </summary>
    public Transform[] firePoint;
    /// <summary>
    /// 飞机移动速度
    /// </summary>
    public float velocity;
    /// <summary>
    /// 开火时间间隔
    /// </summary>
    public float castDuration;

    /// <summary>
    /// 子弹受力
    /// </summary>
    public float bulletForce;
    /// <summary>
    /// 子弹速度
    /// </summary>
    public float bulletVelocity;
    /// <summary>
    /// 子弹生命
    /// </summary>
    public float bulletLife;
    
    /// <summary>
    /// 子弹迸发特效的对象池
    /// </summary>
    public U3DAutoRestoreObjectPool burstEffectPool;

    /// <summary>
    /// 雷受力
    /// </summary>
    public float boomForce;
    /// <summary>
    /// 雷速度
    /// </summary>
    public float boomVelocity;
    /// <summary>
    /// 雷生命
    /// </summary>
    public float boomLife;


    /// <summary>
    /// 雷里每个单位的力大小
    /// </summary>
    public float boomUnitForce;
    /// <summary>
    /// 雷里每个单位的生命
    /// </summary>
    public float boomUnitInitLife;
    /// <summary>
    /// 雷里每个单位的速度
    /// </summary>
    public float boomUnitVelocity;
    /// <summary>
    /// 雷的单位数目
    /// </summary>
    public int boomUnitNum;
    /// <summary>
    /// 雷转圈数
    /// </summary>
    public float boomCircleNum;
    /// <summary>
    /// 雷的单位间隔
    /// </summary>
    public float boomDuration;

    //子弹特效的对象池
    private U3DAutoRestoreObjectPool pool;
    //飞机的Transform
    private Transform _CacheTransform;
    //下次可攻击事件
    private float nextCanCastTime;

    private List<TestBoom> booms = new List<TestBoom>();

    private void Awake()
    {
        pool = GetComponent<U3DAutoRestoreObjectPool>();
        _CacheTransform = gameObject.GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.J) && Time.time >= nextCanCastTime)
        {
            nextCanCastTime = Time.time + castDuration;
            foreach (var tran in firePoint)
            {
                IAutoRestoreObject<GameObject> restoreObj = null;
                GameObject go = null;

                //从迸发特效对象池中获取
                restoreObj = burstEffectPool.Take();
                //获取对象实体
                go = restoreObj.Get();
                go.transform.position = tran.position;
                TimeBaseChecker checker = go.GetComponent<TimeBaseChecker>();
                if (checker == null)
                {
                    checker = go.AddComponent<TimeBaseChecker>();
                }
                checker.Init(0.5f); //特效时间大概是0.3秒。我们就让他0.5秒回收吧。这个暂时不需要很精确。
                restoreObj.Restore = checker;   //将回收检查器用于对象

                //从子弹特效对象池中取
                restoreObj = pool.Take();
                //获取子弹GameObject
                go = restoreObj.Get();
                go.transform.position = tran.position;

                TestBullet bullet = go.GetComponent<TestBullet>();
                bullet.Init(bulletForce, bulletLife, bulletVelocity, delegate(GameObject obj) { });
                restoreObj.Restore = bullet;
            }
        }

        if (Input.GetKey(KeyCode.U) && Time.time >= nextCanCastTime)
        {
            nextCanCastTime = Time.time + castDuration;
            int[] angles = {-30, -20, -10, 0, 10, 20, 30};
            for (int i =0; i<angles.Length; i++)
            {
            		int angle = angles[i];
                IAutoRestoreObject<GameObject> restoreObj = null;
                GameObject go = null;

                //从迸发特效对象池中获取
                restoreObj = burstEffectPool.Take();
                //获取对象实体
                go = restoreObj.Get();
                go.transform.position = _CacheTransform.position;
                TimeBaseChecker checker = go.GetComponent<TimeBaseChecker>();
                if (checker == null)
                {
                    checker = go.AddComponent<TimeBaseChecker>();
                }
                checker.Init(0.5f); //特效时间大概是0.3秒。我们就让他0.5秒回收吧。这个暂时不需要很精确。
                restoreObj.Restore = checker;   //将回收检查器用于对象

                //从子弹特效对象池中取
                restoreObj = pool.Take();
                //获取子弹GameObject
                go = restoreObj.Get();
                go.transform.position = _CacheTransform.position;

                TestBullet bullet = go.GetComponent<TestBullet>();
                bullet.Init(bulletForce, bulletLife, bulletVelocity, delegate(GameObject obj) { }, angle);
                restoreObj.Restore = bullet;
            }
        }

        if (Input.GetKey(KeyCode.K))
        {
            //从子弹特效对象池中取
            IAutoRestoreObject<GameObject>  restoreObj = pool.Take();
            //获取子弹GameObject
            GameObject go = restoreObj.Get();
            go.transform.position = _CacheTransform.position;
            TestBullet bullet = go.GetComponent<TestBullet>();
            bullet.Init(boomForce, boomLife, boomVelocity, CreateBoom);
            restoreObj.Restore = bullet;
        }

        TryMovePlane();
        ProcessBooms();
    }

    /// <summary>
    /// 处理雷
    /// </summary>
    private void ProcessBooms()
    {
        for (int i = 0; i < booms.Count; i++ )
        {
            var boom = booms[i];
            if (boom.End)
            {
                booms.RemoveAt(i);
                i--;
            }
            else
            {
                boom.Update(Time.time);
            }
        }
    }

    private void CreateBoom(GameObject bullet)
    {
        booms.Add(new TestBoom(boomUnitNum, boomCircleNum, pool, boomDuration, bullet.gameObject.transform.position, boomUnitForce, boomUnitInitLife, boomUnitVelocity));
    }

    private void OnGUI()
    {
        int y = Screen.height - 45;
        int height = 20;
        GUI.Label(new Rect(10, y, 400, height),  "use `W`,`A`,`S`,`D` or `arrow` to move");
        y -= height;
        GUI.Label(new Rect(10, y, 400, height), "use 'J' to shoot");
        y -= height;
        GUI.Label(new Rect(10, y, 400, height), "use `U` to use shoot2");
        y -= height;
        GUI.Label(new Rect(10, y, 400, height), "use `K` to use boom");
        y -= height;
        GUI.Label(new Rect(10, y, 400, height), "---- Control ----");
    }

    /// <summary>
    /// 根据实际情况看是否需要让飞机移动
    /// </summary>
    private void TryMovePlane()
    {
        Vector3 moveVelocity = Vector3.zero;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveVelocity -= Vector3.right * velocity;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveVelocity += Vector3.right * velocity;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveVelocity += Vector3.up * velocity;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveVelocity -= Vector3.up * velocity;
        }
        if (moveVelocity != Vector3.zero)
        {
            _CacheTransform.position += moveVelocity * Time.deltaTime;
        }
    }
}
