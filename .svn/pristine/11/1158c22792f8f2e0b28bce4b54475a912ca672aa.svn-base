using System;

internal class MonoApplication : MonoBase
{
    internal Action FocusEvent;
    internal Action PauseEvent;
    internal Action QuitEvent;

    private void OnApplicationFocus()
    {
        if (FocusEvent != null)
        {
            FocusEvent();
        }
    }

    private void OnApplicationPause()
    {
        if (PauseEvent != null)
        {
            PauseEvent();
        }
    }

    private void OnApplicationQuit()
    {
        if (QuitEvent != null)
        {
            QuitEvent();
        }
    }
}

