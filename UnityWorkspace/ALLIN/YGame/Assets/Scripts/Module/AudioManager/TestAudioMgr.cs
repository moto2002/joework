using UnityEngine;
using System.Collections;

public class TestAudioMgr : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.Init();

        //1.加载AudioClip,填充到Dictionary<string, AudioClip>，然后可以根据name来播放clip
        //2.外部加载得到AudioClip，直接进行Play
    }

    void Update()
    {

    }
}
