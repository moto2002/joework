using System;

public class MonoUpdate : MonoBase
{
    internal Action UpdateEvent;

    private void Update()
    {
        if (UpdateEvent != null)
        {
            UpdateEvent();
        }
        else
        {
            DestroyWhenNullEvent();
        }
    }
}