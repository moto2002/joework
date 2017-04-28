using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 创建cubemap
/// </summary>
public class CreateCubemap : ScriptableWizard {

	//位置
	public Transform renderFromPosition;
	//cubemap
	public Cubemap cubemap;

	[MenuItem ("Tools/Create Cube map")]
	static void CreateWizard () {
		ScriptableWizard.DisplayWizard<CreateCubemap>("Create Cubemap", "Create");
	}

	void OnWizardCreate(){
		//创建一个临时相机
		var go = new GameObject( "CubemapCamera", typeof(Camera) );

		go.transform.position = renderFromPosition.position;
		go.transform.rotation = Quaternion.identity;

		go.GetComponent<Camera>().RenderToCubemap( cubemap );

		DestroyImmediate( go );
	}

	void OnWizardUpdate(){
		helpString = "Select transform to render from and cubemap to render into";
		isValid = (renderFromPosition != null) && (cubemap != null);
	}
}
