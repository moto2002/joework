namespace Com.Duoyu001.Pool
{
    /* ==============================================================================
     * 功能描述：对象工厂，用于创建、销毁、激活、反激活对象  
     * 创 建 者：cjunhong
     * 邮    箱：john.cha@qq.com
     * Q      Q：327112182
     * ==============================================================================*/
    public interface IObjectFactory<T>
    {
        /// <summary>
        /// 创建一个对象
        /// </summary>
        /// <returns></returns>
        T CreateObject(bool doActive);

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="t"></param>
        void DestroyObject(T t);

        /// <summary>
        /// 激活对象
        /// </summary>
        /// <param name="t"></param>
        void ActivateObject(T t);

        /// <summary>
        /// 反激活对象
        /// </summary>
        /// <param name="t"></param>
        void UnActivateObject(T t);
    }
}
