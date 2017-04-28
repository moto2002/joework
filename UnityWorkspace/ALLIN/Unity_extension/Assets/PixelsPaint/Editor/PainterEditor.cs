using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Painter))]
public class PainterEditor : Editor {

	public override void OnInspectorGUI(){

		Painter source = (Painter)target;
		EditorGUILayout.BeginVertical();

		source.isAutoInit = EditorGUILayout.Toggle (new GUIContent ("Is Auto Init"), source.isAutoInit);

		if(source.isAutoInit){
			source.initShowPic = EditorGUILayout.Toggle (new GUIContent ("Init Show Source"), source.initShowPic);
		}

		source.lerpDamp = EditorGUILayout.FloatField(new GUIContent("Brush Lerp"),source.lerpDamp);
		source.isEraser = EditorGUILayout.Toggle(new GUIContent("Is Eraser"),source.isEraser);

		source.brushType = (Painter.BrushType)EditorGUILayout.EnumPopup(new GUIContent("Brush Type"), source.brushType);

		if (source.brushType == Painter.BrushType.CustomBrush) {
			source.pen = (Texture2D)EditorGUILayout.ObjectField (new GUIContent ("Brush"), source.pen, typeof(Texture2D), true);
			source.penAlphaEnable = EditorGUILayout.Toggle(new GUIContent("Pen Alpha Support"),source.penAlphaEnable);
		} else {
			source.brushSize = EditorGUILayout.IntField(new GUIContent("Brush Size"),source.brushSize);
		}

		source.paintType = (Painter.PaintType)EditorGUILayout.EnumPopup(new GUIContent("Paint Type"), source.paintType);
		if (source.paintType == Painter.PaintType.Scribble) {
			source.source = (Texture2D)EditorGUILayout.ObjectField (new GUIContent ("Source Texture"), source.source, typeof(Texture2D), true);
		} else if (source.paintType == Painter.PaintType.DrawLine) {
			source.paintColor = EditorGUILayout.ColorField(new GUIContent("Paint Color"),source.paintColor);
		}else if (source.paintType == Painter.PaintType.DrawColorfulLine) {
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("paintColorful"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("colorChangeRate"), false);
			serializedObject.ApplyModifiedProperties();
		}

		EditorGUILayout.EndVertical();
	}
}

