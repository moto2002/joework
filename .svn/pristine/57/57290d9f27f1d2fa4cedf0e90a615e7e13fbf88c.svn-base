using System;

namespace Core
{
    internal class MonoFixedUpdate : MonoBase
    {
        internal Action FixedUpdateEvent;

        private void FixedUpdate()
        {
            if (FixedUpdateEvent != null)
            {
                FixedUpdateEvent();
            }
            else
            {
                DestroyWhenNullEvent();
            }
        }
    }
}
