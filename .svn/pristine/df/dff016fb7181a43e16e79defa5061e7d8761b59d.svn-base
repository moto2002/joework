using UnityEngine;
using UnityEditor;

/// <summary>
/// 扩展Transform面板，显示世界坐标，更改原点
/// author:zhouzhanglin
/// </summary>
[CustomEditor(typeof(Transform))]
public class TransformInspector : Editor {

	static bool pivotMode = false;
	static Tool tool;

	SerializedProperty mPos;
	SerializedProperty mScale;

	public void OnEnable(){
		mPos = serializedObject.FindProperty("m_LocalPosition");
		mScale = serializedObject.FindProperty("m_LocalScale");		
	}

	public override void OnInspectorGUI()
	{
		pivot = (target as Transform).position;
		EditorGUIUtility.labelWidth = 15f;
		DrawPosition();
		DrawRotation();
		DrawScale();
		DrawGlobalPosition();

		if((target as Transform).childCount>0){
			bool mode = GUILayout.Toggle(pivotMode,"Change Children Pivot","Button");
			if(mode!=pivotMode){
				if(mode){
					tool = Tools.current;
					Tools.current = Tool.None;
					pivot = (target as Transform).position;
				}else{
					Tools.current = tool;
				}
			}
			pivotMode = mode;
		}
		else
		{
			pivotMode = false;
		}
		serializedObject.ApplyModifiedProperties();
	}

	void DrawPosition ()
	{
		GUILayout.BeginHorizontal();
		{
			bool reset = GUILayout.Button("P", GUILayout.Width(20f));
			EditorGUILayout.LabelField("Position",GUILayout.Width(50f));
			mPos.vector3Value = ConvertPosition (mPos.vector3Value);
			EditorGUILayout.PropertyField(mPos.FindPropertyRelative("x"));
			EditorGUILayout.PropertyField(mPos.FindPropertyRelative("y"));
			EditorGUILayout.PropertyField(mPos.FindPropertyRelative("z"));
			if (reset) mPos.vector3Value = Vector3.zero;
		}
		GUILayout.EndHorizontal();
		Undo.RecordObject(target,"move");
	}

	Vector3 ConvertPosition(Vector3 pPoint)
	{
		pPoint.x = ConvertValue (pPoint.x);
		pPoint.y = ConvertValue (pPoint.y);
		pPoint.z = ConvertValue (pPoint.z);
		return pPoint;
	}

	float ConvertValue(float pValue)
	{
		return (float)System.Convert.ToDouble(pValue.ToString("f2"));
	}

	void DrawGlobalPosition(){
		GUILayout.BeginHorizontal();
		{
			Transform tran = target as Transform;
			EditorGUILayout.LabelField("World Pos",GUILayout.Width(75f));
			tran.position = EditorGUILayout.Vector3Field("", tran.position);
			// EditorGUILayout.LabelField("World Pos",GUILayout.Width(74f));
			// EditorGUILayout.LabelField("X  "+tran.position.x , GUILayout.Width(70f),GUILayout.ExpandWidth(true));
			// EditorGUILayout.LabelField("Y  "+tran.position.y , GUILayout.Width(70f),GUILayout.ExpandWidth(true));
			// EditorGUILayout.LabelField("Z  "+tran.position.z , GUILayout.Width(70f),GUILayout.ExpandWidth(true));
		}
		GUILayout.EndHorizontal();
		Undo.RecordObject(target,"move");
	}

	void DrawScale ()
	{
		GUILayout.BeginHorizontal();
		{
			bool reset = GUILayout.Button("P", GUILayout.Width(20f));
			EditorGUILayout.LabelField("Scale",GUILayout.Width(50f));
			EditorGUILayout.PropertyField(mScale.FindPropertyRelative("x"));
			EditorGUILayout.PropertyField(mScale.FindPropertyRelative("y"));
			EditorGUILayout.PropertyField(mScale.FindPropertyRelative("z"));
			if (reset) mScale.vector3Value = Vector3.one;
		}
		GUILayout.EndHorizontal();
		Undo.RecordObject(target,"scale");
	}

	void DrawRotation ()
	{
		GUILayout.BeginHorizontal();
		{
			bool reset = GUILayout.Button("P", GUILayout.Width(20f));
			EditorGUILayout.LabelField("Rotation",GUILayout.Width(50f));
			Vector3 ls = (serializedObject.targetObject as Transform).localEulerAngles;
			FloatField("X",ref ls.x);
			FloatField("Y",ref ls.y);
			FloatField("Z",ref ls.z);
			if (reset)
				(serializedObject.targetObject as Transform).localEulerAngles = Vector3.zero;
			else
				(serializedObject.targetObject as Transform).localEulerAngles = ls;
		}
		GUILayout.EndHorizontal();
		Undo.RecordObject(target,"rotate");
	}

	void FloatField(string name,ref float f)
	{
		f = EditorGUILayout.FloatField(name,f);
	}



	private Vector3 pivot;
	void OnSceneGUI(){
		if(pivotMode){
			Transform trans = target as Transform;
			Handles.color = Color.red;
			Handles.Label(pivot,"Change Children Pivot");
			Vector3 tempPivot = Handles.DoPositionHandle(pivot,Quaternion.identity);
			Vector3 movePivot = tempPivot-pivot;
			for(int i=0 ;i<trans.childCount;++i){
				Transform child = trans.GetChild(i);
				child.position-= new Vector3(movePivot.x,movePivot.y,0f);
				Undo.RecordObject(child,"move");
			}
			trans.position+= new Vector3(movePivot.x,movePivot.y,0f);
			pivot = tempPivot;
			Undo.RecordObject(target,"move");
			Undo.RecordObject(target,"rotate");
			SceneView.RepaintAll();
		}
        
	}

}
