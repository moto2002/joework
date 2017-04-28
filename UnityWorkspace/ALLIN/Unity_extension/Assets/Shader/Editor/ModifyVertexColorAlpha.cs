using UnityEngine;
using UnityEditor;
/// <summary>
/// 处理顶点颜色
/// </summary>
public class ModifyVertexColorAlpha {

	[MenuItem("Tools/Modify Vertex Color")]
	static void ModifyVertexColor(){
		foreach(Object o in Selection.GetFiltered(typeof(GameObject),SelectionMode.DeepAssets)){
			if(!(o is GameObject))
				continue;

			GameObject go = o as GameObject;
			MeshFilter meshFilter = go.GetComponent<MeshFilter>();
			if(meshFilter&& meshFilter.sharedMesh){
				Mesh mesh = meshFilter.sharedMesh;
				float min=float.MaxValue;
				float max = float.MinValue;
				//找最y最大和最小值
				foreach(Vector3 v in mesh.vertices){
					if(v.y>max){
						max = v.y;
					}
					if(v.y<min){
						min = v.y;
					}
				}
				float cha=max-min;
				Color[] cols = new Color[mesh.vertexCount];
				for(int i=0;i<mesh.vertexCount;++i){
					Color c = Color.white;
					c.a=(mesh.vertices[i].y-min)/cha;
					cols[i] = c;
				}
				mesh.colors = cols;
                meshFilter.sharedMesh = mesh;
                string path = AssetDatabase.GetAssetPath(go);
             
                path = path.Substring(0, path.LastIndexOf(".")) + "_VertexColor.asset";
                AssetDatabase.CreateAsset(Object.Instantiate(mesh), path);
                AssetDatabase.SaveAssets();
			}
		}
	}

	[MenuItem("Tools/Show Vertex Color")]
	static void ShowVertexColor(){
		foreach(Object o in Selection.GetFiltered(typeof(GameObject),SelectionMode.DeepAssets)){
			if(!(o is GameObject))
				continue;
			GameObject go = o as GameObject;
			MeshFilter meshFilter = go.GetComponent<MeshFilter>();
			if(meshFilter&& meshFilter.sharedMesh){
				Mesh mesh = meshFilter.sharedMesh;
				for(int i=0;i<mesh.vertexCount;++i){
					Debug.Log(mesh.colors[i]);
				}
			}
		}
	}


}
