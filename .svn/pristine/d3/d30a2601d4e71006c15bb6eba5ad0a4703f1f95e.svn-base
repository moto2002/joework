using System;
using System.Collections.Generic;
using System.Text;

namespace SMGame
{
    /// <summary>
    /// 消息号
    /// </summary>
    public abstract class XNetProtocol
    {
        protected abstract XNetProtocol Clone();

        protected virtual void OnAnalyze(byte[] bytes)
        {

        }
        protected virtual void OnHandle()
        {

        }
        public virtual byte[] ToBytes()
        {
            return new byte[0];
        }
        public virtual void Release()
        {
        }

    }
}
