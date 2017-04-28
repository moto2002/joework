using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DragAndDrop3D))]
public class DragAndDrop3DEditor : Editor {

	public override void OnInspectorGUI(){

		DragAndDrop3D source = (DragAndDrop3D)target;
		EditorGUILayout.BeginVertical();

		source.dragTarget = (Transform)EditorGUILayout.ObjectField(new GUIContent("Drag Target", "拖动的对象，默认为自己."), source.dragTarget, typeof(Transform),true);

		source.isDragDisableCollider = EditorGUILayout.Toggle(new GUIContent("Is Drag Disable Collider", "Drag时是否禁用此对象的collider组件."), source.isDragDisableCollider);

		source.rayCastCamera = (Camera)EditorGUILayout.ObjectField(new GUIContent("Raycast Camera", "如果为null，则使用mainCamera."), source.rayCastCamera,typeof(Camera),true);
		

		source.raycastDistance = EditorGUILayout.FloatField(new GUIContent("Raycast Distance", "射线的检测距离，只用于射线检测时."), source.raycastDistance);

		source.rayCastMasksLength = EditorGUILayout.IntField(new GUIContent("Raycast Masks Length", "Drag Object射线检测的Layer"), source.rayCastMasksLength);

		if( source.rayCastMasksLength>0 && (source.rayCastMasks==null || source.rayCastMasksLength!=source.rayCastMasks.Length)){
			source.rayCastMasks = new LayerMask[source.rayCastMasksLength];
		}
		for(int i = 0; i<source.rayCastMasksLength;i++){
			source.rayCastMasks[i] = EditorGUILayout.LayerField(new GUIContent("        Layer "+i, ""), source.rayCastMasks[i]);
		}

		source.isDragOriginPoint = EditorGUILayout.Toggle(new GUIContent("Is Drag Origin Point", "在拖动时是否固定在拖动物的原点."), source.isDragOriginPoint);

		if (source.isDragOriginPoint) {
			source.dragOffset = EditorGUILayout.Vector3Field(new GUIContent("Drag Offset", "拖动时的偏移值."), source.dragOffset);
		}

		source.dragLayer = EditorGUILayout.LayerField(new GUIContent("Drag Layer", ""), source.dragLayer);

		source.dragMoveDamp = EditorGUILayout.FloatField(new GUIContent("Drag Move Damp", "拖动时的缓动参数."), source.dragMoveDamp);
		
		source.mousePickLayer = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Mouse Pick Layer", "移动时在哪个面上移动，如果为null，则在拖动物的Z轴面移动."), source.mousePickLayer,typeof(GameObject),true);

		if(source.mousePickLayer){
			source.pickLayerIsPlane = EditorGUILayout.Toggle(new GUIContent("Pick Layer is Plane","Pick Layer是否是Plane,Plane为无限区域"),source.pickLayerIsPlane);
		}

		source.dropPosByMouse = EditorGUILayout.Toggle(new GUIContent("Drop Pos By Mouse","Drop时参考位置，true为参考鼠标位置，false为参考引用对象位置"),source.dropPosByMouse);

		if(!source.dropPosByMouse){
			source.dropRefPos = (Transform)EditorGUILayout.ObjectField(new GUIContent("Drop Ref Pos", "Drop引用位置，为空时是自己."), source.dropRefPos, typeof(Transform),true);
		}

		source.dropLayerMaskLength = EditorGUILayout.IntField(new GUIContent("Drop LayerMask Length", "drop容器所在的层."), source.dropLayerMaskLength);

		if( source.dropLayerMaskLength>0 && (source.dropLayerMasks==null || source.dropLayerMaskLength!=source.dropLayerMasks.Length)){
			source.dropLayerMasks = new LayerMask[source.dropLayerMaskLength];
		}
		for(int i = 0; i<source.dropLayerMaskLength;i++){
			source.dropLayerMasks[i] = EditorGUILayout.LayerField(new GUIContent("        Layer "+i, ""), source.dropLayerMasks[i]);
		}

		source.dropMedthod = EditorGUILayout.TextField(new GUIContent("Drop Medthod", "drop发生时发送的事件，drop和当前拖动对象都会发送."), source.dropMedthod);

		source.isDropFailBack = EditorGUILayout.Toggle(new GUIContent("Is Drop Fail Back", "如果没有检测到可drop的容器，是否返回原来的位置."), source.isDropFailBack);

		if (source.isDropFailBack) {
			source.dragBackEffect = (DragAndDrop3D.DragBackEffect)EditorGUILayout.EnumPopup(new GUIContent("Drag Back Effect", "返回原来位置时的效果."), source.dragBackEffect);

			if(source.dragBackEffect== DragAndDrop3D.DragBackEffect.TweenPosition||source.dragBackEffect== DragAndDrop3D.DragBackEffect.TweenScale){
				source.backEffectSpeed = EditorGUILayout.FloatField(new GUIContent("Back Effect Speed", "返回原来位置时的速度.对TweenPosition和TweenScale有用."), source.backEffectSpeed);
			}
		}

		EditorGUILayout.EndVertical();
	}
}
