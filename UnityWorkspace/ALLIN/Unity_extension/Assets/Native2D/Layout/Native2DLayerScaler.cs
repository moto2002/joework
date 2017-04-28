using UnityEngine;
using System.Collections;
/// <summary>
/// Native2D 游戏Sprite的根目录
/// </summary>
[ExecuteInEditMode]
public class Native2DLayerScaler: MonoBehaviour {

	public UnityEngine.UI.CanvasScaler referenceCanvas;

	void Start(){
		if(referenceCanvas){
			transform.localScale = referenceCanvas.transform.localScale*100f;
		}
	}

	void LateUpdate(){
		if(Application.platform== RuntimePlatform.OSXEditor||Application.platform== RuntimePlatform.WindowsEditor){
			if(referenceCanvas){
				transform.localScale = referenceCanvas.transform.localScale*100f;
			}
		}
	}
}
