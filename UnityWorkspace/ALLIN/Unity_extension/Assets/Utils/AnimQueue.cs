using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 控制动画播放顺序
/// </summary>
public class AnimQueue : MonoBehaviour {

	public bool autoPlay=true;//是否自动播放
	public float delay=0f; //整体的delay
	public AnimVO[] anims; //动画集

	[System.Serializable]
	public class AnimVO{
		public Animator animator;
		public string animName;//AnimatiorController里面的Clip名称
		public float delay ;//delay播放
	}

	void Start(){
		if(autoPlay){
			Invoke("StartAnimQueue",delay);
		}
	}

	IEnumerator PlayAnim( AnimVO vo){
		yield return new WaitForSeconds(vo.delay);
		//全部只播放layer=0的动画
		vo.animator.Play(vo.animName,0,0f);
	}

	/// <summary>
	/// 开始播放动画
	/// </summary>
	public void StartAnimQueue () {
		foreach(AnimVO vo in anims){
			if(vo.animator!=null ){
				StartCoroutine(PlayAnim(vo));
			}
		}
	}
}
