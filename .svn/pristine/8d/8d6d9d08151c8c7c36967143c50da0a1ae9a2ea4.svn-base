using UnityEngine;
using UnityEditor;

/// <summary>
/// 修改材质的渲染层级
/// </summary>
public class MaterialRenderQueue : EditorWindow {

	[MenuItem("Tools/Material Queue")]
	static void ChangeMaterialQueue(){
		MaterialRenderQueue window = (MaterialRenderQueue)EditorWindow.GetWindow (typeof (MaterialRenderQueue));
		window.titleContent=new GUIContent("Queue Setting");
		window.ShowUtility ();
	}

	private Material m_mat;
	private int m_queue=3000;

	void OnGUI()
	{
		m_mat = (Material)EditorGUILayout.ObjectField("Material",m_mat,typeof(Material),false);
		m_queue = EditorGUILayout.IntField("Queue",m_queue);

		if(GUILayout.Button("OK",new GUILayoutOption[]{GUILayout.Width(120),GUILayout.Height(32)})){
			if(m_mat){
				m_mat.renderQueue = m_queue;
			}
		}
	}
}
