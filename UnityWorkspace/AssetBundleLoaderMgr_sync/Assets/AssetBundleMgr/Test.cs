using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    public GameObject canvas;
    void Start()
    {
       //1.无依赖。
        /*
        string path = Application.streamingAssetsPath;
        AssetBundle ab = AssetBundle.LoadFromFile(path + "/ui/button.unity3d");
        Object obj = ab.LoadAsset("button");
        GameObject inst = GameObject.Instantiate(obj) as GameObject;
        inst.transform.SetParent(canvas.transform);
        inst.transform.localPosition = Vector3.zero;
        inst.transform.localScale = Vector3.one;
        */

        //2.加载TexturePacker打出的图集 里面的所有sprite
       /*
        * string path = Application.streamingAssetsPath;
        AssetBundle ab = AssetBundle.LoadFromFile(path + "/fish_common2.unity3d");
        Sprite[] result = ab.LoadAllAssets<Sprite>();
        int spriteArryLength = result.Length;
        Debug.Log("length:" + spriteArryLength);
        for (int i = 0; i < spriteArryLength; i++)
        {
            Debug.Log(result[i].name);
        }
        */

        //3.处理依赖, (如：依赖图集)
        
        AssetBundleLoaderMgr.instance.LoadAssetBundleManifest();
        Object obj= AssetBundleLoaderMgr.instance.LoadAssetBundleFromFile("image.unity3d");
        GameObject inst = GameObject.Instantiate(obj) as GameObject;
        inst.transform.SetParent(canvas.transform);
        inst.transform.localPosition = Vector3.zero;
        inst.transform.localScale = Vector3.one;
    }

    void Update()
    {

    }
}
