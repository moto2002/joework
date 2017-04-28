using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// 输入控制.
/// </summary>
public class InputUtil {

	//手动控制
	public static bool isOnUI = false;

	/// <summary>
	/// 判断鼠标是否在UI上面，ui包括ugui，一些3dui(通过Collider来阻止事件的ui)
	/// </summary>
	/// <returns><c>true</c>, if mouse on U was checked, <c>false</c> otherwise.</returns>
	public static bool CheckMouseOnUI(){
		if(isOnUI) return true;
		return CheckMouseOnUGUI();
		return false;
	}


	/// <summary>
	/// 判断是否在ugui上面
	/// </summary>
	/// <returns><c>true</c>, if mouse on UGU was checked, <c>false</c> otherwise.</returns>
	static bool CheckMouseOnUGUI(){
		if(EventSystem.current){
			if(Input.touchSupported && Input.touchCount>0){
				return EventSystem.current.IsPointerOverGameObject (Input.GetTouch(0).fingerId);
			}else{
				return EventSystem.current.IsPointerOverGameObject ();
			}
		}else{
			// GameObject.Instantiate(Resources.Load("UI/EventSystem"));
		}
		return false;
	}
}
