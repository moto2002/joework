namespace Com.Duoyu001.Pool
{
    /* ==============================================================================
     * 功能描述：一个能主动回收的对象池 
     * 创 建 者：cjunhong
     * 邮    箱：john.cha@qq.com
     * Q      Q：327112182
     * ==============================================================================*/
    public interface IActiveRestoreObjectPool<T> : IObjectPool<IAutoRestoreObject<T>>
    {
        /// <summary>
        /// 此方法必须能识别哪些对象可以回收，而且回收到池中
        /// </summary>
        void CheckRestore();
    }
}
