using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingSceneTest : MonoBehaviour {

	public GameObject loadingPrefab;
	public Image iconPrefab;
	public float transitionSpeed = 2f;
	public SceneLoading.TransitionType transitionIn = SceneLoading.TransitionType.FadeIn ;
	public SceneLoading.TransitionType transitionOut = SceneLoading.TransitionType.FadeOut;

	private SceneLoading m_sceneLoading;
	private Image m_icon;

	// Use this for initialization
	IEnumerator Start () {
		DontDestroyOnLoad(gameObject);
		yield return new WaitForSeconds(2f);
		if(loadingPrefab){
			GameObject go =  (GameObject)Instantiate(loadingPrefab);

			m_sceneLoading = go.GetComponentInChildren<UnityEngine.UI.SceneLoading>() ;
			if(m_sceneLoading){
				m_sceneLoading.transitionIn = transitionIn;
				m_sceneLoading.transitionOut = transitionOut;
				m_sceneLoading.transitionSpeed = transitionSpeed;

				m_sceneLoading.onTransitionInOver = delegate() {
					StartCoroutine(ShowIcon());
				};
				m_sceneLoading.onLoadSceneComplete = delegate() {
					StartCoroutine(RemoveIcon());
				};
				m_sceneLoading.StartTransitionIn(1);
			}
		}
	}

	IEnumerator ShowIcon()
	{
		m_icon = (Image)Instantiate(iconPrefab);
		m_icon.transform.SetParent(m_sceneLoading.transform.parent);
		m_icon.transform.localScale = Vector3.zero;
		m_icon.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		while(m_icon.transform.localScale.x<0.99f){
			m_icon.transform.localScale = Vector3.Lerp(m_icon.transform.localScale,Vector3.one,50*Time.deltaTime);
			yield return null;
		}
		yield return new WaitForSeconds(0.25f);
		m_sceneLoading.StartLoadScene();
	}

	IEnumerator RemoveIcon()
	{
		while(m_icon.transform.localScale.x>0.01f){
			m_icon.transform.localScale = Vector3.Lerp(m_icon.transform.localScale,Vector3.zero,20*Time.deltaTime);
			yield return null;
		}
		m_sceneLoading.StartTransitionOut();
		DestroyImmediate(gameObject);
	}
	

}
