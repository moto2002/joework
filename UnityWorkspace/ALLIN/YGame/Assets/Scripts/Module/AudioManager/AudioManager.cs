using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//你需要音频组件：AudioSource和AudioListener，问题是音频片段是通过AudioClip展现，每个AudioSource在某一刻只能有单个AudioClip在播放，
//有时这已经足够，但有时我们需要让游戏对象能播放多种不同的声音，或许是在同一刻（想象一下，角色说话的同时传出脚步声）
//我们有两种类型：一个是transform(emitter),声音将在emitter处播放，并且跟随emitter的移动（如开过的汽车）
//另外一个是Vector3，不同点是如果你提供了一个点(Vector3)，声音将在此播放，
public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<string, AudioClip> m_acDic = new Dictionary<string, AudioClip>();
    private AudioClip GetAudioClip(string acName)
    {
        AudioClip ac;
        if (m_acDic.TryGetValue(acName, out ac))
        {
            return ac;
        }
        return null;
    }
    private void AddAudioClip(string acName, AudioClip ac)
    {
        if (!m_acDic.ContainsKey(acName))
        {
            m_acDic.Add(acName, ac);
        }
    }

    public void Init()
    {
        m_bgmAudio = this.gameObject.AddComponent<AudioSource>();
    }

    #region Play AudioClip Attach Emitter
    public AudioSource Play(string acName, Transform emitter)
    {
        return Play(acName, emitter, 1f, 1f);
    }
    public AudioSource Play(string acName, Transform emitter, float volume)
    {
        return Play(acName, emitter, volume, 1f);
    }
    public AudioSource Play(string acName, Transform emitter, float volume, float pitch)
    {
        AudioClip clip = GetAudioClip(acName);
        if (clip != null)
        {
            return Play(clip, emitter, volume, pitch);
        }
        return null;
    }
    public AudioSource Play(AudioClip clip, Transform emitter)
    {
        return Play(clip, emitter, 1f, 1f);
    }
    public AudioSource Play(AudioClip clip, Transform emitter, float volume)
    {
        return Play(clip, emitter, volume, 1f);
    }

    /// <summary>
    /// Plays a sound by creating an empty game object with an AudioSource
    /// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
    /// </summary>
    public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch)
    {
        //Create an empty game object attach emitter
        GameObject go = new GameObject("Audio: " + clip.name);
        go.transform.parent = emitter;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(go, clip.length);
        return source;
    }
    #endregion

    #region Play AudioClip At Point
    public AudioSource Play(string acName, Vector3 point)
    {
        return Play(acName, point, 1f, 1f);
    }
    public AudioSource Play(string acName, Vector3 point, float volume)
    {
        return Play(acName, point, volume, 1f);
    }
    public AudioSource Play(string acName, Vector3 point, float volume, float pitch)
    {
        AudioClip clip = GetAudioClip(acName);
        if (clip != null)
        {
            return Play(clip,point,volume,pitch);
        }
        return null;
    }
    public AudioSource Play(AudioClip clip, Vector3 point)
    {
        return Play(clip, point, 1f, 1f);
    }
    public AudioSource Play(AudioClip clip, Vector3 point, float volume)
    {
        return Play(clip, point, volume, 1f);
    }

    /// <summary>
    /// Plays a sound at the given point in space by creating an empty game object with an AudioSource
    /// in that place and destroys it after it finished playing.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="point"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    public AudioSource Play(AudioClip clip, Vector3 point, float volume, float pitch)
    {
        //Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);
        go.transform.position = point;

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(go, clip.length);
        return source;
    }
    #endregion

    #region Play BGM
    private string m_prevBGMName = "";//上一次播放的BGM名字
    private AudioSource m_bgmAudio;
    public void PlayBGM(string clipName)
    {
        if (!string.IsNullOrEmpty(m_prevBGMName) && !string.IsNullOrEmpty(clipName))
        {
            if (m_prevBGMName != clipName)
            {
                StopBGM();
            }
            else
            {
                //same
                return;
            }
        }
        m_bgmAudio.loop = true;
        m_bgmAudio.clip = GetAudioClip(clipName);
        if (m_bgmAudio.clip == null)
        {
            //m_bgmAudio.clip = Resources.Load("Audio/" + clipName) as AudioClip;
            Debug.LogError("No AudioClip:"+clipName);
            return;
        }
        if (m_bgmAudio.clip != null && m_bgmAudio.isPlaying == false)
        {
            m_bgmAudio.Play();
        }
        m_prevBGMName = clipName;
    }
    public void StopBGM()
    {
        if (m_bgmAudio != null && m_bgmAudio.isPlaying)
        {
            m_bgmAudio.Stop();
            m_bgmAudio.clip = null;
        }
    }
    public void ReSumeBGM()
    {
        if (m_bgmAudio.clip != null)
        {
            m_bgmAudio.Play();
        }
    }
    public void PauseBGM()
    {
        if (m_bgmAudio != null && m_bgmAudio.clip != null)
        {
            m_bgmAudio.Pause();
        }
    }
    #endregion

}