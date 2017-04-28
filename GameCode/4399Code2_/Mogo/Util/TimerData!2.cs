namespace Mogo.Util
{
    using System;

    internal class TimerData<T, U> : AbsTimerData
    {
        private Action<T, U> m_action;
        private T m_arg1;
        private U m_arg2;

        public override void DoAction()
        {
            this.m_action(this.m_arg1, this.m_arg2);
        }

        public override Delegate Action
        {
            get
            {
                return this.m_action;
            }
            set
            {
                this.m_action = value as Action<T, U>;
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

        public U Arg2
        {
            get
            {
                return this.m_arg2;
            }
            set
            {
                this.m_arg2 = value;
            }
        }
    }
}

