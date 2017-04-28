using UnityEngine;
using System;

namespace UnityEngine.UI
{	
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))]
	public class SceneLoading : MonoBehaviour {

		enum EffectState{ NONE,IN,STAY,OUT}

		public enum TransitionType{
			FadeIn,FadeOut,CircleIn,CircleOut
		}

		[HideInInspector]
		public TransitionType transitionIn = TransitionType.FadeIn ;
		[HideInInspector]
		public TransitionType transitionOut = TransitionType.FadeOut;

		public Shader circleHole;

		public Action onTransitionInStart,onTransitionInOver,onLoadSceneComplete,onTransitionOutStart,onTransitionOutOver;
		public Action<float> onLoadProgress;
		
		[HideInInspector]
		public float transitionSpeed = 2f;

		private string m_nextName;
		private int m_nextLv;
		private AsyncOperation m_async;
		private EffectState m_effectState = EffectState.NONE;

		private Image m_img;
		private Material m_material;

		// Use this for initialization
		void Awake () {
			DontDestroyOnLoad (gameObject);
			DontDestroyOnLoad (transform.parent.gameObject);

			m_img = GetComponent<Image>();
			m_material = new Material(circleHole);
			m_img.material = m_material;
		}



		public void StartTransitionIn(string sceneName){
			m_nextName = sceneName;
			m_effectState = EffectState.IN;
			transIn();
		}
		public void StartTransitionIn(int lv){
			m_nextLv = lv;
			m_effectState = EffectState.IN;
			transIn();
		}
		private void transIn()
		{
			if(transitionIn== TransitionType.CircleIn){
				Color c = m_img.color;
				c.a = 1f;
				m_img.color = c;
				m_material.SetFloat("_Radius",1f);
			}else if(transitionIn== TransitionType.FadeIn){
				Color c = m_img.color;
				c.a = 0f;
				m_img.color = c;
				m_material.SetFloat("_Radius",0f);
			}
			if(onTransitionInStart!=null){
				onTransitionInStart();
			}
		}



		
		public void StartLoadScene(){
			if(!string.IsNullOrEmpty(m_nextName)){
				m_async = Application.LoadLevelAsync (m_nextName);
			}else if(m_nextLv>-1){
				m_async = Application.LoadLevelAsync (m_nextLv);
			}
		}
		
		public void StartTransitionOut(){
			m_effectState = EffectState.OUT;
			GraphicRaycaster gr = transform.parent.GetComponent<GraphicRaycaster>();
			if(gr){
				gr.enabled = false;
			}
		}

		void Update()
		{
			if(m_effectState== EffectState.IN){
				if(transitionIn== TransitionType.FadeIn){
					Color c = m_img.color;
					c.a = Mathf.Lerp(c.a,1f,transitionSpeed*Time.deltaTime) ;
					if(c.a>=0.99f){
						c.a = 1f;
						m_effectState = EffectState.STAY;
						if(onTransitionInOver!=null){
							onTransitionInOver();
						}
					}
					m_img.color = c;
				}else if(transitionIn== TransitionType.CircleIn){
					float rad = m_material.GetFloat("_Radius");
					rad = Mathf.Lerp(rad,0f,transitionSpeed*Time.deltaTime*2f) ;
					if(rad<=0.01f){
						rad = 0f;
						m_effectState = EffectState.STAY;
						if(onTransitionInOver!=null){
							onTransitionInOver();
						}
					}
					m_material.SetFloat("_Radius",rad);
				}
			}
			else if(m_effectState== EffectState.STAY){
				if(m_async!=null){
					if(onLoadProgress!=null){
						onLoadProgress(m_async.progress);
					}
					if( m_async.progress==1f){
						m_effectState = EffectState.NONE;
						if(onLoadSceneComplete!=null){
							onLoadSceneComplete();
						}
					}
				}
			}
			else if(m_effectState== EffectState.OUT){
				if(transitionOut== TransitionType.FadeOut){
					Color c = m_img.color;
					c.a = Mathf.Lerp(c.a,0f,transitionSpeed*Time.deltaTime) ;
					if(c.a<=0.01f){
						c.a = 0f;
						DestroyImmediate(transform.parent.gameObject,true);
						return;
					}
					m_img.color = c;
				}else if(transitionOut== TransitionType.CircleOut){
					float rad = m_material.GetFloat("_Radius");
					rad = Mathf.Lerp(rad,1f,transitionSpeed*Time.deltaTime) ;
					if(rad>=0.99f){
						rad = 1f;
						if(onTransitionOutOver!=null){
							onTransitionOutOver();
						}
						DestroyImmediate(transform.parent.gameObject,true);
						return;
					}
					m_material.SetFloat("_Radius",rad);
				}
			}
		}
	}

}