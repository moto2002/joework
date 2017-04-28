
namespace Com.Duoyu001.Pool
{
    /* ==============================================================================
     * 功能描述：可主动回收的对象
     * 创 建 者：cjunhong
     * 邮    箱：john.cha@qq.com
     * Q      Q：327112182
     * ==============================================================================*/
    public class AutoRestoreObject<T> : IAutoRestoreObject<T>
    {
        
        private T t;

        public AutoRestoreObject(T t)
        {
            this.t = t;
        }

        public IAutoRestoreChecker Restore { get; set; }

        public T Get()
        {
            return t;
        }
    }
}
