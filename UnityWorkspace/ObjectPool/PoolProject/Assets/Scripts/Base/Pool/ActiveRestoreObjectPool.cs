using System.Collections.Generic;

namespace Com.Duoyu001.Pool
{
    /* ==============================================================================
     * 功能描述：主动回收对象池  
     * 创 建 者：cjunhong
     * 邮    箱：john.cha@qq.com
     * Q      Q：327112182
     * ==============================================================================*/
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActiveRestoreObjectPool<T> : IObjectPool<IAutoRestoreObject<T>>
    {
        private BaseObjectPool<T> baseObjectPool;
        private List<IAutoRestoreObject<T>> checkList = new List<IAutoRestoreObject<T>>();
        private IObjectFactory<T> factory;

        public ActiveRestoreObjectPool(IObjectFactory<T> factory, int maxIdleNum)
        {
            baseObjectPool = new BaseObjectPool<T>(factory, maxIdleNum);
            this.factory = factory;
        }

        public IAutoRestoreObject<T> Take()
        {
            T t = baseObjectPool.Take();
            AutoRestoreObject<T> autoRestoreObject = new AutoRestoreObject<T>(t);
            checkList.Add(autoRestoreObject);
            return autoRestoreObject;
        }

        /// <summary>
        /// 每次调用会把可回收的对象重新存入对象池
        /// </summary>
        public void CheckRestore()
        {
            for (int i = 0; i < checkList.Count; i++)
            {
                IAutoRestoreObject<T> check = checkList[i];
                IAutoRestoreChecker autoRestoreChecker = check.Restore;
                if (autoRestoreChecker != null && autoRestoreChecker.Restore)
                {
                    baseObjectPool.Restore(check.Get());
                    checkList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Restore(IAutoRestoreObject<T> t)
        {
            baseObjectPool.Restore(t.Get());
            checkList.Remove(t);
        }

        public void AddObject()
        {
            baseObjectPool.AddObject();
        }

        public int IdleNum()
        {
            return baseObjectPool.IdleNum();
        }

        public int ActiveNum()
        {
            return baseObjectPool.ActiveNum();
        }

        public void Clear()
        {
            baseObjectPool.Clear();
            foreach (IAutoRestoreObject<T> autoRestoreObject in checkList)
            {
                factory.DestroyObject(autoRestoreObject.Get());
            }
            checkList.Clear();
        }
    }
}
