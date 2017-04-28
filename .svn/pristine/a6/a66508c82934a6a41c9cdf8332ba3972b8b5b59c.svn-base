using UnityEngine;

namespace Com.Duoyu001.Pool.U3D
{
    /* ==============================================================================
     * 功能描述：U3D预设-对象池工厂  
     * 创 建 者：cjunhong
     * 邮    箱：john.cha@qq.com
     * Q      Q：327112182
     * ==============================================================================*/
    public class PrefabFactory : IObjectFactory<GameObject>
    {
        private GameObject prefab;
        private Transform parent;

        public PrefabFactory(GameObject prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
        }

        public GameObject CreateObject(bool doActive)
        {
            GameObject go =  GameObject.Instantiate(prefab) as GameObject;
            go.transform.parent = parent;
            go.transform.localPosition = Vector3.zero;
            if (doActive)
            {
                ActivateObject(go);
            }
            else
            {
                go.SetActive(false);
            }
            return go;
        }

        public void DestroyObject(GameObject t)
        {
            GameObject.Destroy(t);
        }

        public void ActivateObject(GameObject t)
        {
            t.SetActive(true);
        }

        public void UnActivateObject(GameObject t)
        {
            t.SetActive(false);
        }
    }
}
