using UnityEngine;
using System.Collections;
using System;

public class TestLoadLightmapAssetBundle : MonoBehaviour
{
    void Start()
    {
        GameObject goRoot = GameObject.Instantiate(Resources.Load("SceneRoot") as GameObject);
        goRoot.name = "SceneRoot";
    }

    IEnumerator loadTest(Action<SceneLightmapData> callback)
    {
        WWW www = new WWW("file://" + Application.streamingAssetsPath + "/Windows/scenelightmapdata.unity3d");
       // WWW www = new WWW("jar:file://" + Application.dataPath + "!/assets/"+ "Windows/scenelightmapdata.unity3d");
        yield return www;
        if (www.isDone)
        {
            SceneLightmapData sceneLightmapData2 = www.assetBundle.LoadAsset("scenelightmapdata") as SceneLightmapData;
            Debug.Log(sceneLightmapData2.m_sceneName);
            callback(sceneLightmapData2);
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 100), "加载灯光贴图"))
        {
            #region Unity5.x加载,ResourcesManager我已经处理了依赖的加载，可以看我之前的文章
           /* ResourcesManager.Instance.LoadAssetBundle("scenelightmapdata", (obj) =>
            {
                //设置场景的灯光信息
                SceneLightmapData scenemapData = obj as SceneLightmapData;
                int Count = scenemapData.m_lightMapNearName.Length;
                LightmapData[] tempMapDatas = new LightmapData[Count];
                for (int i = 0; i < Count; i++)
                {
                    LightmapData data = new LightmapData();
                    Texture2D near = Resources.Load(scenemapData.m_lightMapNearName[i]) as Texture2D;
                    Texture2D far = Resources.Load(scenemapData.m_lightMapFarName[i]) as Texture2D;
                    data.lightmapFar = far;
                    data.lightmapNear = near;
                    tempMapDatas[i] = data;
                }
                LightmapSettings.lightmaps = tempMapDatas;

                //设置每一个GameObject的lightmapIndex，lightmapScaleOffset
                Transform sceneRootTrans = GameObject.Find(scenemapData.m_gameObjectRoot).transform;
                for (int i = 0; i < scenemapData.m_gameObjectList.Count; i++)
                {
                    Transform tempTrans = sceneRootTrans.FindChild(scenemapData.m_gameObjectList[i].m_name);
                    Renderer tempRenderer = tempTrans.GetComponent<Renderer>();
                    tempRenderer.lightmapIndex = scenemapData.m_gameObjectList[i].m_lightmapIndex;
                    tempRenderer.lightmapScaleOffset = scenemapData.m_gameObjectList[i].m_lightmapScaleOffset;
                }

                //动态加载完之后需要调用一下StaticBatchingUtility.Combine(RootGameObject)来将整个场景静态化，使得batching能正常使用。
                StaticBatchingUtility.Combine(sceneRootTrans.gameObject);

            });
            */
            #endregion

            #region 加载

            StartCoroutine(loadTest((obj) => {

                //设置场景的灯光信息
                SceneLightmapData scenemapData = obj as SceneLightmapData;
                int Count = scenemapData.m_lightMapNearName.Length;
                LightmapData[] tempMapDatas = new LightmapData[Count];
                for (int i = 0; i < Count; i++)
                {
                    LightmapData data = new LightmapData();
                    Texture2D near = Resources.Load(scenemapData.m_lightMapNearName[i]) as Texture2D;
                    Texture2D far = Resources.Load(scenemapData.m_lightMapFarName[i]) as Texture2D;
                    data.lightmapFar = far;
                    data.lightmapNear = near;
                    tempMapDatas[i] = data;
                }
                LightmapSettings.lightmaps = tempMapDatas;

                //设置每一个GameObject的lightmapIndex，lightmapScaleOffset
                Transform sceneRootTrans = GameObject.Find(scenemapData.m_gameObjectRoot).transform;
                for (int i = 0; i < scenemapData.m_gameObjectList.Count; i++)
                {
                    Transform tempTrans = sceneRootTrans.FindChild(scenemapData.m_gameObjectList[i].m_name);
                    Renderer tempRenderer = tempTrans.GetComponent<Renderer>();
                    tempRenderer.lightmapIndex = scenemapData.m_gameObjectList[i].m_lightmapIndex;
                    tempRenderer.lightmapScaleOffset = scenemapData.m_gameObjectList[i].m_lightmapScaleOffset;
                }

                //动态加载完之后需要调用一下StaticBatchingUtility.Combine(RootGameObject)来将整个场景静态化，使得batching能正常使用。
                StaticBatchingUtility.Combine(sceneRootTrans.gameObject);

            }));
            #endregion
        }
    }

}
