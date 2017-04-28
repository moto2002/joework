using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 路径获取
/// </summary>
public class PathCollect : MonoBehaviour {

	public Vector3[] path;
	public bool isClosed;
	public bool isLocalPosition;

	#if UNITY_EDITOR
	public Color gizmosColor = Color.red;

	void OnDrawGizmos()  
	{  
		if(path!=null)
		{
			Gizmos.color=gizmosColor;
			Matrix4x4 cacheMat = Gizmos.matrix;

			if(isLocalPosition){
				Gizmos.matrix = transform.localToWorldMatrix;
			}
			if(path.Length>1)
			{
				Vector3 prev=path[0];  
				for(int i=1;i<path.Length;++i){  
					Gizmos.DrawLine(path[i],prev);
					prev = path[i];
				}
				if(isClosed&&path.Length>2){
					Gizmos.DrawLine(path[path.Length-1],path[0]);
				}
			}
			for(int i=0;i<path.Length;++i){  
				Gizmos.DrawWireSphere(path[i],0.1f);
			}
			Gizmos.matrix = cacheMat;
		}

	}  

	#endif

}
