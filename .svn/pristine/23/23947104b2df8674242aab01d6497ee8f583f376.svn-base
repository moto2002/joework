using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO
public class CPoolChildrenData
{
    public int preloadAmount;
}

public class CPoolManager : MonoBehaviour
{
    public static CPoolManager instance;

    private Dictionary<string, SpawnPool> spawnPoolDic = new Dictionary<string, SpawnPool>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CreatePool("bullets");
        CreatePool("effects");
        CreatePool("fishs");

        //DeletePool("effects");
        //DeletePool("bullets");

        // ClearPool();

        ///////////////////////////////////////////////

        CreatePoolChildrenGo("bullets", Resources.Load<Transform>("Cube"));
        CreatePoolChildrenGo("bullets", Resources.Load<Transform>("Sphere"));

        GameObject cube = SpawnPoolChildrenGo("bullets", "Cube");
        GameObject sphere = SpawnPoolChildrenGo("bullets", "Sphere");
        GameObject go = SpawnPoolChildrenGo("bullets", "Cube");

        DeSpawnPoolChildrenGo("bullets", cube.transform);
        DeSpawnPoolChildrenGo("bullets", sphere.transform);

    }

    void Update()
    {

    }

    private void OnDestroy()
    {

    }



    #region Do First : 池的类型的操作
    public void CreatePool(string _poolName)
    {
        GameObject go = new GameObject(_poolName);
        SpawnPool _pool = PoolManager.Pools.Create(_poolName, go);

        if (!spawnPoolDic.ContainsKey(_poolName))
        {
            spawnPoolDic.Add(_poolName, _pool);
        }
        else
        {
            spawnPoolDic[_poolName] = _pool;
        }
    }

    private SpawnPool GetPool(string _poolName)
    {
        SpawnPool _spawnPool;
        if (spawnPoolDic.TryGetValue(_poolName, out _spawnPool))
        {
            return _spawnPool;
        }
        else
        {
            Debug.LogError("no pool:" + _poolName);
            return null;
        }
    }

    public void DeletePool(string _poolName)
    {
        SpawnPool _spawnPool = GetPool(_poolName);
        if (_spawnPool != null)
        {
            _spawnPool.DespawnAll();
            GameObject.Destroy(_spawnPool.gameObject);
            spawnPoolDic.Remove(_poolName);
        }
    }

    public void ClearPool()
    {
        List<string> _spawnPoolNameList = new List<string>(spawnPoolDic.Keys);
        for (int i = 0; i < _spawnPoolNameList.Count; i++)
        {
            string _poolName = _spawnPoolNameList[i];
            DeletePool(_poolName);
        }
        spawnPoolDic.Clear();
    }
    #endregion

    #region Do Second : 池中的几种对象的操作
    public void CreatePoolChildrenGo(string _poolName, Transform _childenPrefab, CPoolChildrenData _poolChildrenData = null)
    {
        SpawnPool spawnPool = GetPool(_poolName);
        if (spawnPool == null)
            return;

        PrefabPool refabPool = new PrefabPool(_childenPrefab);
        if (!spawnPool._perPrefabPoolOptions.Contains(refabPool))
        {
            refabPool = new PrefabPool(_childenPrefab);

            //预先实例化个数
            refabPool.preloadAmount = 20;

            //开启限制
            refabPool.limitInstances = true;

            //关闭无限取Prefab
            refabPool.limitFIFO = false;//如果我们限制了缓存池里面只能有10个Prefab，如果不勾选它，那么你拿第11个的时候就会返回null。如果勾选它在取第11个的时候他会返回给你前10个里最不常用的那个。

            //限制池子里最大的Prefab数量
            refabPool.limitAmount = 50;

            //开启自动清理池子
            refabPool.cullDespawned = true;

            //最终保留
            refabPool.cullAbove = 20;

            //多久清理一次
            refabPool.cullDelay = 5;

            //每次清理几个
            refabPool.cullMaxPerPass = 5;

            //初始化内存池
            spawnPool._perPrefabPoolOptions.Add(refabPool);

            //spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[spawnPool.Count]);

            for (int i = 0; i < spawnPool._perPrefabPoolOptions.Count; i++)
            {
                spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[i]);
            }

        }
    }

    public GameObject SpawnPoolChildrenGo(string _poolName, string _childrenName)
    {
        SpawnPool spawnPool = GetPool(_poolName);
        if (spawnPool == null)
            return null;

        if (spawnPool.prefabs.ContainsKey(_childrenName))
        {
            Transform prefab = spawnPool.prefabs[_childrenName];
            Transform inst = spawnPool.Spawn(prefab);
            return inst.gameObject;
        }
        else
        {
            Debug.LogError("no pool children:" + _poolName + "_" + _childrenName);
            return null;
        }
    }

    public void DeSpawnPoolChildrenGo(string _poolName, Transform _instTransform, string _childrenName = "")
    {
        SpawnPool spawnPool = GetPool(_poolName);
        if (spawnPool == null)
            return;

        spawnPool.Despawn(_instTransform);

        /*
        if (spawnPool.prefabs.ContainsKey(_childrenName))
        {
            spawnPool.Despawn(_instTransform);
        }
        else
        {
            Debug.LogError("no pool children:" + _poolName + "_" + _childrenName);
            return;
        }
        */
    }
    #endregion

}
