namespace Com.Duoyu001.Pool
{
    /* ==============================================================================
     * 功能描述：一个对象池接口  
     * 创 建 者：cjunhong
     * 邮    箱：john.cha@qq.com
     * Q      Q：327112182
     * ==============================================================================*/

    /// <summary>
    /// 一个对象池接口  
    /// </summary>
    /// <typeparam name="T">该对象池中存放的对象类型</typeparam>
    public interface IObjectPool<T>
    {
        /// <summary>
        /// 从对象池中取对象
        /// </summary>
        /// <returns></returns>
        T Take();

        /// <summary>
        /// 把对象存回对象池
        /// </summary>
        /// <param name="t"></param>
        void Restore(T t);

        /// <summary>
        /// 对象池增加对象
        /// </summary>
        void AddObject();

        /// <summary>
        /// 对象池中空闲(可用)对象的数目
        /// </summary>
        /// <returns></returns>
        int IdleNum();

        /// <summary>
        /// 对象池中空闲(可用)对象的数目
        /// </summary>
        /// <returns></returns>
        int ActiveNum();

        /// <summary>
        /// 清除对象池中所有对象
        /// </summary>
        void Clear();
    }
}
