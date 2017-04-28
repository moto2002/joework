namespace Mogo.Util
{
    using System;

    internal class TimerData<T> : AbsTimerData
    {
        private Action<T> m_action;
        private T m_arg1;

        public override void DoAction()
        {
            this.m_action(this.m_arg1);
        }

        public override Delegate Action
        {
            get
            {
                return this.m_action;
            }
            set
            {
                this.m_action = value as Action<T>;
            }
        }

        public T Arg1
        {
            get
            {
                return this.m_arg1;
            }
            set
            {
                this.m_arg1 = value;
            }
        }
    }
}

