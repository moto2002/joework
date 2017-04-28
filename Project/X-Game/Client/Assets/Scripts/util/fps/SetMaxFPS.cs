using UnityEngine;
using System.Collections;

//垂直同步设置
public enum VSyncCountSetting
{
    DontSync,
    EveryVBlank,
    EverSecondVBlank
}

public class SetMaxFPS : MonoBehaviour
{
    private VSyncCountSetting VSyncCount = VSyncCountSetting.DontSync;//用于快捷设置Unity Quanity设置中的垂直同步相关参数
    private int MaxFPSValue = 60;//帧率的值,关闭垂直同步,可以自己设定帧数
    private bool MaxNoLimit = false;//不设限制，保持可达到的最高帧率

    void Awake()
    {
        //设置垂直同步相关参数
        switch (VSyncCount)
        {
            //默认设置，不等待垂直同步，可以获取更高的帧率数
            case VSyncCountSetting.DontSync:
                QualitySettings.vSyncCount = 0;
        	break;

            //EveryVBlank，相当于帧率设为最大60
            case VSyncCountSetting.EveryVBlank:
            QualitySettings.vSyncCount = 1;
            break;
            //EverSecondVBlank情况，相当于帧率设为最大30
            case VSyncCountSetting.EverSecondVBlank:
            QualitySettings.vSyncCount = 2;
            break;

        }

        //设置没有帧率限制，火力全开！
        if (MaxNoLimit)
        {
            Application.targetFrameRate = -1;
        }
        //设置帧率的值
        else
        {
            Application.targetFrameRate = MaxFPSValue - 1;
        }   
    }
}
