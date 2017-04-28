using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 简单的声音管理
/// </summary>
public class AudioManager:MonoBehaviour {
	private static AudioManager m_instance;
	public static AudioManager Instance{
		get{
			if(m_instance==null){
				GameObject go = new GameObject("[Music]");
				return go.AddComponent<AudioManager>();
			}
			return m_instance;
		}
	}

	private AudioSource m_musicAS;

	void Awake(){
		if(m_instance!=null){
			Destroy(gameObject);
			return;
		}
		m_instance = this;
		m_musicAS = GetComponent<AudioSource>();
		if(m_musicAS==null){
			m_musicAS = gameObject.AddComponent<AudioSource>();
			m_musicAS.loop=true;
			m_musicAS.playOnAwake = false;
		}
		DontDestroyOnLoad(gameObject);
	}


	//存储loop的music声音
	private Dictionary<string,AudioSource> m_musicAudio = new Dictionary<string, AudioSource>();
	private Dictionary<string,AudioSource> m_musicPathAudio = new Dictionary<string, AudioSource>();
	private List<string> m_removeGuids = new List<string>();

	#region Sound

	public float volume{
		get{return AudioListener.volume; }
		set{
			AudioListener.volume=value;
		}
	}

	/// <summary>
	/// 只播放一次的声音
	/// </summary>
	/// <param name="resourcePath">Resource path.</param>
	/// <param name="volume">Volume.</param>
	/// <param name="delay">delay.</param>
	public void PlaySoundEffect( string resourcePath , float volume = 1f , float delay=0f){
		if(AudioListener.volume<0.01f) return ;
		AudioClip clip = Resources.Load<AudioClip>(resourcePath);
		if(clip){
			if(delay>0){
				StartCoroutine(DelayPlaySoundEffect(clip,volume,delay));
			}else{
				PlaySoundEffect(clip,volume);
			}
		}
		else
		{
			Debug.LogWarning("Sound not found: "+resourcePath);
		}
	}
	IEnumerator DelayPlaySoundEffect(AudioClip clip , float volume, float delay ){
		yield return new WaitForSeconds(delay);
		AudioSource.PlayClipAtPoint(clip,Camera.main.transform.position);
	}
	/// <summary>
	/// 只播放一次的声音
	/// </summary>
	/// <param name="clip">Clip.</param>
	/// <param name="volume">Volume.</param>
	/// <param name="delay">delay.</param>
	public void PlaySoundEffect( AudioClip clip , float volume = 1f, float delay=0f){
		if(AudioListener.volume<0.01f) return ;
		if(delay>0){
			StartCoroutine(DelayPlaySoundEffect(clip,volume,delay));
		}else{
			AudioSource.PlayClipAtPoint(clip,Camera.main.transform.position);
		}
	}
	#endregion


	void FixedUpdate(){
		if(m_musicAudio.Count>0){
			m_removeGuids.Clear();
			foreach(string guid in m_musicAudio.Keys){
				AudioSource source = m_musicAudio[guid];
				if(!source.loop && !source.isPlaying){
					m_removeGuids.Add(guid);
				}
			}
			for(int i=0;i<m_removeGuids.Count;++i){
				StopMusicByGUID(m_removeGuids[i]);
			}
		}
	}


	#region Music
	/// <summary>
	/// 循环播放音效
	/// </summary>
	/// <returns>此音效的id.</returns>
	/// <param name="resourcePath">Resource path.</param>
	/// <param name="volume">Volume.</param>
	/// <param name="dontDestroy">dontDestroy.</param>
	public string PlayMusic(string resourcePath , float volume = 1f ,bool isLoop = true,bool dontDestroy=false){
		if(m_musicPathAudio.ContainsKey(resourcePath)){
			AudioSource source = m_musicPathAudio[resourcePath];
			if(!source.isPlaying){
				source.loop = isLoop;
				source.Play();
			}
			foreach(string key in m_musicAudio.Keys){
				if(m_musicAudio[key]==source){
					return key;
				}
			}
		}
		else
		{
			AudioClip clip = Resources.Load<AudioClip>(resourcePath);
			if(clip){
				string id = PlayMusic(clip,volume,isLoop,dontDestroy);
				AudioSource source = m_musicAudio[id];
				m_musicPathAudio[resourcePath] = source;
				return id;
			}
		}
		return null;
	}
	/// <summary>
	/// 循环播放音效
	/// </summary>
	/// <returns>此音效的id.</returns>
	/// <param name="clip">Clip.</param>
	/// <param name="volume">Volume.</param>
	/// <param name="dontDestroy">dontDestroy.</param>
	public string PlayMusic(AudioClip clip, float volume = 1f,bool isLoop = true,bool dontDestroy=false){
		string guid = Guid.NewGuid().ToString();
		GameObject go = new GameObject("Music_"+guid);
		if(dontDestroy){
			GameObject.DontDestroyOnLoad(go);
		}
		go.transform.position = Camera.main.transform.position;
		AudioSource audioSource = go.AddComponent<AudioSource>();
		audioSource.loop=isLoop;
		audioSource.volume = volume;
		audioSource.clip = clip;
		audioSource.Play();
		m_musicAudio[guid] = audioSource;
		return guid;
	}
	public void StopMusicByGUID( string guid){
		if(m_musicAudio.ContainsKey(guid)){
			AudioSource source = m_musicAudio[guid];
			source.Stop();
			string id=null;
			foreach(String path in m_musicPathAudio.Keys){
				if(m_musicPathAudio[path]== source){
					id = path;
					break;
				}
			}
			if(!string.IsNullOrEmpty(id)){
				m_musicPathAudio.Remove(id);
			}
			m_musicAudio.Remove(guid);
			GameObject.Destroy(source.gameObject);
		}
	}
	public void StopMusicByPath( string path){
		if(m_musicPathAudio.ContainsKey(path)){
			AudioSource source = m_musicPathAudio[path];
			source.Stop();
			string id=null;
			foreach(String guid in m_musicAudio.Keys){
				if(m_musicAudio[guid]== source){
					id = guid;
					break;
				}
			}
			if(!string.IsNullOrEmpty(id)){
				m_musicAudio.Remove(id);
			}
			m_musicPathAudio.Remove(path);
			GameObject.Destroy(source.gameObject);
		}
	}
	public AudioSource GetMusicByGUID( string guid ){
		if(m_musicAudio.ContainsKey(guid)){
			return m_musicAudio[guid];
		}
		return null;
	}
	public AudioSource GetMusicByPath( string path){
		if(m_musicPathAudio.ContainsKey(path)){
			return m_musicPathAudio[path];
		}
		return null;
	}

	public void StopAllMusic(){
		foreach(var item in m_musicAudio){
			AudioSource source = item.Value;
			source.Stop();
			GameObject.Destroy(source.gameObject);
		}
		m_musicAudio.Clear();
		m_musicPathAudio.Clear();
		StopBgMusic();
	}

	/// <summary>
	/// 播放背景音乐
	/// </summary>
	/// <param name="clip">Clip.</param>
	/// <param name="volume">Volume.</param>
	/// <param name="isContinue">是否继续，false将重新开始播放.</param>
	public void PlayBgMusic( AudioClip clip , bool isContinue=true){
		if(isContinue && clip==m_musicAS.clip && m_musicAS.isPlaying) return;

		m_musicAS.clip = clip;
		m_musicAS.Play();
	}
	/// <summary>
	/// 停止背景音乐的播放
	/// </summary>
	public void StopBgMusic(){
		m_musicAS.Stop();
	}

	/// <summary>
	/// 背景音乐的音量
	/// </summary>
	/// <value>The background music volume.</value>
	public float bgMusicVolume{
		get{ return m_musicAS.volume ; }
		set{
			m_musicAS.volume = value;
		}
	}
	#endregion

}
