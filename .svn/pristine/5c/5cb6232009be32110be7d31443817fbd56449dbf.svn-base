using UnityEngine;
using System.Collections;


public class MyUnityTools
{
    #region 实例化GameObject
    public static GameObject LoadGameObject(GameObject prefab)
    {
        return LoadGameObject(prefab, null, Vector3.zero, Vector3.one);
    }
    public static GameObject LoadGameObject(GameObject prefab, GameObject parent, Vector3 pos, Vector3 scale)
    {
        if (prefab)
        {
            GameObject _obj = GameObject.Instantiate(prefab);
            if (parent != null)
                _obj.transform.parent = parent.transform;
            _obj.transform.localPosition = pos;
            _obj.transform.localScale = scale;
            return _obj;
        }
        else
            Debug.Log("Load-GameObject-Failed");
        return null;
    }
    #endregion

    #region 射线相关
    public static int GetLayerMaskValue(string s)
    {
        LayerMask mask;
        mask = 1 << LayerMask.NameToLayer(s);
        return mask.value;
    }
    public static RaycastHit SendRay(Ray ray, float maxDistance, string layName)
    {
        RaycastHit hit;
        int layMaskVal = GetLayerMaskValue(layName);
        if (Physics.Raycast(ray, out hit, maxDistance, layMaskVal))
        {
            return hit;
        }
        return hit;
    }
    #endregion

    /// <summary>
    /// 物体（包含所有子物体）碰撞器 是否可用
    /// </summary>
    /// <param name="goObj"></param>
    /// <param name="isEnable"></param>
    public static void EnabelBoxCollider(GameObject goObj, bool isEnable)
    {
        BoxCollider[] colliders = goObj.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = isEnable;
        }
    }

    /// <summary>
    /// 改变物体（包含所有子物体）的layer
    /// </summary>
    /// <param name="ts"></param>
    /// <param name="layerName"></param>
    public static void ChangeLayer(Transform ts, string layerName)
    {
        int layId = LayerMask.NameToLayer(layerName);
        ts.gameObject.layer = layId;
        Transform[] ts_children = ts.GetComponentsInChildren<Transform>();
        for (int i = 0; i < ts_children.Length; i++)
        {
            ts_children[i].gameObject.layer = layId;
        }        
    }

    /// <summary>
    /// 世界坐标转换到NGUI相机空间下的坐标
    /// </summary>
    /// <param name="wordPos"></param>
    /// <returns></returns>
    public static Vector3 WordPos2NGUIPos(Vector3 wordPos)
    {
        //Vector3 pos = Vector3.zero;
        //pos = Camera.main.WorldToScreenPoint(wordPos);
        //pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
        //pos.z = 0;
        //return pos;
        return Vector3.zero;
    }

    /// <summary>
    /// 查找子对象
    /// </summary>
    public static GameObject Child(Transform go, string subnode)
    {
        Transform tran = go.FindChild(subnode);
        if (tran == null) return null;
        return tran.gameObject;
    }

    /// <summary>
    /// 取平级对象
    /// </summary>
    public static GameObject Peer(Transform go, string subnode)
    {
        Transform tran = go.parent.FindChild(subnode);
        if (tran == null) return null;
        return tran.gameObject;
    }

    /// <summary>
    /// 清除所有子节点
    /// </summary>
    public static void ClearChild(Transform go)
    {
        if (go == null) return;
        for (int i = go.childCount - 1; i >= 0; i--)
        {
           GameObject.Destroy(go.GetChild(i).gameObject);
        }
    }

    #region 换装相关
    //-------------------------------------------------
    public static Transform FindChildRecursive(Transform t, string name)
    {
        if (t.name == name)
        {
            return t;
        }

        Transform r = t.Find(name);
        if (r != null)
            return r;
        foreach (Transform c in t)
        {
            r = FindChildRecursive(c, name);
            if (r != null)
                return r;
        }
        return null;
    }

    public static int BoneNameIndex = 1024;
    public static int GetIncreaseNum()
    {
        return BoneNameIndex++;
    }
    public static string GetIncreaseObjectName()
    {
        BoneNameIndex++;
        return "IncrObject_+" + BoneNameIndex;
    }
    public static string GetIncreaseObjectName(string name)
    {
        BoneNameIndex++;
        return name + BoneNameIndex;
    }
    public static void SetBones(GameObject newPart, GameObject root)
    {

        var smr = newPart.GetComponent<SkinnedMeshRenderer>();
        var myBones = new Transform[smr.bones.Length];
        for (var i = 0; i < smr.bones.Length; i++)
        {
            //Debug.Log("bone "+render.bones[i].name);
            myBones[i] = FindChildRecursive(root.transform, smr.bones[i].name);
        }
        smr.bones = myBones;
    }
    public static void SetBones(GameObject newPart, GameObject copyPart, GameObject root)
    {
        var smr = newPart.GetComponent<SkinnedMeshRenderer>();
        var copyRender = copyPart.GetComponent<SkinnedMeshRenderer>();
        var myBones = new Transform[copyRender.bones.Length];
        for (var i = 0; i < copyRender.bones.Length; i++)
        {
            myBones[i] = FindChildRecursive(root.transform, copyRender.bones[i].name);
        }
        smr.bones = myBones;
    }
    //-----------------------------------------------------------
    #endregion

    /// <summary>
    /// 是否是无线
    /// </summary>
    public static bool IsWifi
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }

    /// <summary>
    /// 网络是否可用
    /// </summary>
    public static bool NetAvailable
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    /// <summary>
    /// 应用程序内容路径
    /// </summary>
    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = "file://" + Application.dataPath + "/" + GameConst.AssetDirName + "/";
                break;
        }
        return path;
    }

    /// <summary>
    /// 取得数据存放目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            string game = GameConst.AppName.ToLower();
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + game + "/";
            }
            if (GameConst.DebugMode)
            {
                return Application.dataPath + "/" + GameConst.AssetDirName + "/";
            }
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                int i = Application.dataPath.LastIndexOf('/');
                return Application.dataPath.Substring(0, i + 1) + game + "/";
            }
            return "c:/" + game + "/";
        }
    }
    public static string LoadFileFromResources(string fileName)
    {
        TextAsset asset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
        string str = asset.text;
        Resources.UnloadAsset(asset);
        return str;
    }

}
