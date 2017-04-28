using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PathCollect))]
public class PathCollectEditor : Editor {

	void OnSceneGUI(){
		PathCollect pathCollect = target as PathCollect;

		Tools.current = Tool.None;
		if(pathCollect.path!=null)
		{
			for(int i=0;i<pathCollect.path.Length;++i){
				if(pathCollect.isLocalPosition){
					Vector3 worldPos = Handles.DoPositionHandle(pathCollect.transform.TransformPoint(pathCollect.path[i]),Quaternion.identity);
					pathCollect.path[i] = pathCollect.transform.InverseTransformPoint(worldPos);
					Handles.Label(pathCollect.path[i],i+":"+pathCollect.path[i]);
				}else{
					pathCollect.path[i] = Handles.DoPositionHandle(pathCollect.path[i],Quaternion.identity);
					Handles.Label(pathCollect.path[i],i+":"+pathCollect.path[i]);
				}
			}
			SceneView.RepaintAll();
		}
	}
}
