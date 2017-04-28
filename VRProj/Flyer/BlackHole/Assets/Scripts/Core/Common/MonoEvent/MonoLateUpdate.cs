using System;

public class MonoLateUpdate : MonoBase
{
    public Action LateUpdateEvent;

    private void LateUpdate()
    {
        if (LateUpdateEvent != null)
        {
            LateUpdate();
        }
        else
        {
            DestroyWhenNullEvent();
        }
    }
}