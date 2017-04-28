namespace Mogo.Util
{
    using System;

    internal class TimerData : AbsTimerData
    {
        private System.Action m_action;

        public override void DoAction()
        {
            this.m_action();
        }

        public override Delegate Action
        {
            get
            {
                return this.m_action;
            }
            set
            {
                this.m_action = value as System.Action;
            }
        }
    }
}

