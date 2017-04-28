using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class ExportSceneLightMap : Editor
{
	[MenuItem("Tools/Export Scene LightMap Data")]
	static void Export()
	{
        m_transList = new List<Transform>();
        GetSceneLightMapData();
	}

    static List<Transform> m_transList;
    static SceneLightmapData sceneLightMapData;
    static void GetSceneLightMapData()
	{
        sceneLightMapData = ScriptableObject.CreateInstance<SceneLightmapData>();

		string currSceneName =EditorApplication.currentScene;
		currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/")+1);
		currSceneName = currSceneName.Replace(".unity","");
		//Debug.Log(currSceneName);
		sceneLightMapData.m_sceneName = currSceneName;
		LightmapData[] currSceneLightMaps = LightmapSettings.lightmaps;
		int length = currSceneLightMaps.Length;
		if(length <=0)
		{
			EditorUtility.DisplayDialog("error","场景"+currSceneName+":没有LightmapData","ok");
			return;
		}
		//Debug.Log(length);
		sceneLightMapData.m_lightMapFarName = new string[length];
		sceneLightMapData.m_lightMapNearName = new string[length];
		for (int i = 0; i < length; i++) 
		{
			Texture2D currLightMapFar = currSceneLightMaps[i].lightmapFar;
			Texture2D currLightMapNear = currSceneLightMaps[i].lightmapNear;
			sceneLightMapData.m_lightMapFarName[i] = currLightMapFar.name;
			sceneLightMapData.m_lightMapNearName[i] = currLightMapNear.name;
		}

        sceneLightMapData.m_gameObjectList = new List<GameObjectData>();

        GameObject[] selectionGameObjects;
        if (Selection.activeGameObject)
        {
            selectionGameObjects = Selection.gameObjects;
            int selectedObjsLength = selectionGameObjects.Length;

            if (selectedObjsLength > 0)
            {
                if (selectedObjsLength > 1)
                {
                    EditorUtility.DisplayDialog("Error", "你选中了多个", "ok");
                    return;
                }
                else
                {
                    Transform _tempRoot = selectionGameObjects[0].transform;
                    sceneLightMapData.m_gameObjectRoot = _tempRoot.name;
                    m_transList.Add(_tempRoot);
                    CalcAllGameObject(_tempRoot);
                }

                AddChidGameObjectData();
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "请在Hierarchy中选中要导出的GameObject", "ok");
        }

        sceneLightMapData.ToString();

        string path = "Assets/Resources/SceneLightmapData.asset";           
		
		AssetDatabase.CreateAsset(sceneLightMapData, path);

        //编辑器环境下可以通过AssetDatabase.LoadAssetAtPath来加载
        Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(SceneLightmapData));                              

        string bundleName = path;
        bundleName = bundleName.Substring(bundleName.LastIndexOf("/") + 1);
        bundleName = bundleName.Replace(".asset", ".unity3d");

        #region Unity5.x打包方式
        //打包成.unity3d文件。
        AssetImporter assetImporter = AssetImporter.GetAtPath(path);
        assetImporter.assetBundleName = bundleName;
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/Windows", 0, BuildTarget.StandaloneWindows64);

        #endregion



        //删除面板上的那个临时对象               
        //AssetDatabase.DeleteAsset(p);   

        AssetDatabase.Refresh();

	}

    static void CalcAllGameObject(Transform trans)
    {
        if (trans.childCount > 0)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                m_transList.Add(trans.GetChild(i));
                CalcAllGameObject(trans.GetChild(i));
            }
        }
    }

    static void AddChidGameObjectData()
    {
        for (int i = 0; i < m_transList.Count; i++)
        {
            Renderer render = m_transList[i].GetComponent<Renderer>();
            if (render != null)
            {
                string goName = CalcGoChildName(m_transList[i].name, m_transList[i]);
                goName = goName.Substring(goName.IndexOf("/") + 1);
                //string goName = selectionGameObjects[i].name;
                Vector3 position = m_transList[i].position;
                Vector3 rotation = m_transList[i].rotation.eulerAngles;
                Vector3 scale = m_transList[i].lossyScale;
                int lightmapIndex = render.lightmapIndex;
                Vector4 lightmapScaleOffset = render.lightmapScaleOffset;
                int realtimeLightmapIndex = render.realtimeLightmapIndex;
                Vector4 realtimeLightmapScaleOffset = render.realtimeLightmapScaleOffset;

                GameObjectData goData = new GameObjectData();
                goData.m_name = goName;
                goData.m_position = position;
                goData.m_rotation = rotation;
                goData.m_scale = scale;
                goData.m_lightmapIndex = lightmapIndex;
                goData.m_lightmapScaleOffset = lightmapScaleOffset;
                goData.m_realtimeLightmapIndex = realtimeLightmapIndex;
                goData.m_realtimeLightmapScaleOffset = realtimeLightmapScaleOffset;
                goData.ToString();

                sceneLightMapData.m_gameObjectList.Add(goData);
            }
        }
    }

    static string CalcGoChildName(string name, Transform trans)
    {
        if (trans.parent == null)
        {
            return name;
        }
        else
        {
            trans = trans.parent;
            return CalcGoChildName(trans.name + "/" + name, trans);
        }
    }

}

