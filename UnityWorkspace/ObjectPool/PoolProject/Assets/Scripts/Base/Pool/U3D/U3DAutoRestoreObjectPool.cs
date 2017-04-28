using UnityEngine;
using Com.Duoyu001.Pool;
namespace Com.Duoyu001.Pool.U3D
{
    /* ==============================================================================
     * 功能描述：对象池组件，可以挂在U3D对象上  
     * 创 建 者：cjunhong
     * 邮    箱：john.cha@qq.com
     * Q      Q：327112182
     * ==============================================================================*/
    public class U3DAutoRestoreObjectPool : MonoBehaviour, IActiveRestoreObjectPool<GameObject>
    {
        private ActiveRestoreObjectPool<GameObject> pool;

        public GameObject prefab;
        public Transform parent;
        public int maxNum;
        public int initNum;

        private static int idGenerate;
        private int _id = -1;
        private int id
        {
            get
            {
                if (_id == -1)
                {
                    _id = idGenerate;
                    idGenerate++;
                }
                return _id;
            }
        }

        void Awake()
        {
            pool = new ActiveRestoreObjectPool<GameObject>(new PrefabFactory(prefab, parent), maxNum);
            if (initNum > maxNum)
            {
                initNum = maxNum;
            }
            for (int i = 0; i < initNum; i++)
            {
                pool.AddObject();
            }
        }

        void OnGUI()
        {
            int y = 10+40*id;
            GUI.Label(new Rect(10, y, 300, 20),  "Idle num in pools#"+gameObject.name+"  : " + pool.IdleNum());
            GUI.Label(new Rect(10, y+20, 300, 20), "Active num in pools#"+gameObject.name+" : " + pool.ActiveNum());
        }

        public void Update()
        {
            pool.CheckRestore();
        }

        public IAutoRestoreObject<GameObject> Take()
        {
            return pool.Take();
        }

        public void Restore(IAutoRestoreObject<GameObject> t)
        {
            pool.Restore(t);
        }

        public void AddObject()
        {
            pool.AddObject();
        }

        public int IdleNum()
        {
            return pool.IdleNum();
        }

        public int ActiveNum()
        {
            return pool.ActiveNum();
        }

        public void Clear()
        {
            pool.Clear();
        }

        public void CheckRestore()
        {
            pool.CheckRestore();
        }
    }
}
