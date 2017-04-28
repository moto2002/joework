using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 常量
/// </summary>
public class Config
{

    public const string Common = "Common";

    public const string Pickupable = "Pickupable";

    /// <summary>
    /// 平台 TODO: USELESS
    /// </summary>
    public static string Platform
    {
        get
        {
            return
#if UNITY_STANDALONE_WIN
                    "StandaloneWindows";
#elif UNITY_IPHONE
                    "iOS";
#elif UNITY_ANDROID
					"Android";
#endif
        }
    }

    /// <summary>
    /// 资源持久化根路径: Application.persistentDataPath + "/" + Platform
    /// </summary>
    public static string PersistentPath
    {
        get
        {
            return Application.persistentDataPath + "/" + Platform;
        }
    }


    public const string LevelWasLoaded = "LEVELWASLOADED";

#if HTC

    public const string Connected = "HTCDEVICEHANDCONNECTED";
    public const string OutofRange = "HTCDEVICEOUTOFRANGE";
    public const string TriggerPressed = "HTCDEVICETRIGGERPRESSED";
    public const string Gripped = "HTCDEVICEGRIPPED";
    public const string PadPressed = "HTCDEVICEPADPRESSED";
    public const string MenuPressed = "HTCDEVICEMENUPRESSED";
    public const string PadTouched = "HTCDEVICEPADTOUCHED";
    public const string PadTouching = "HTCDEVICEPADTOUCHEDING";

#endif
}

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);
public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
public delegate void Callback<T, U, V, W>(T arg1, U arg2, V arg3, W arg4);

public interface IProtoBufable
{
    void Set(List<string> inValues);
}

public interface IDeviceState
{
    string Name { get; }
    bool Connected { get; }
    bool OutofRange { get; }
    Transform Root { get; }
    Transform Head { get; }
    Transform HTCHandModel { get; }
    Transform ExtentRoot { get; }

    Vector3 Velocity { get; }
    Vector3 AngularVelocity { get; }
}

public interface IDeviceTriggerState : IDeviceState
{
    bool TriggerPressDownNotUp { get; }
}

public interface IDeviceGripState : IDeviceState
{
    bool GripPressDownNotUp { get; }
}

public interface IDevicePadPressState : IDeviceState
{
    bool PadPressDownNotUp { get; }
}

public interface IDevicePadTouchState : IDeviceState
{
    bool PadTouchDownNotUp { get; }
}

public interface IDevicePadTouchingState : IDeviceState
{
    Vector2 PadPosition { get; }
}

public interface IDeviceMenuState : IDeviceState
{
    bool MenuPressDownNotUp { get; }
}

public interface ITextTyper
{
    Action Done { get; set; }
    ITextTyper Stop();
    ITextTyper Start(int countPerSecond);
}