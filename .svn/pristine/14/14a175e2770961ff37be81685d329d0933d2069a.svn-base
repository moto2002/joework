using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

/// <summary>
/// 日期
/// </summary>
public class MyDateTime
{
    public int _year;
    public int _month;
    public int _day;
    public int _hour;
    public int _minute;
    public int _seconds;

    public int Year
    {
        get
        {
            return _year;
        }
    }

    public int Month
    {
        get
        {
            return _month;
        }
    }

    public int Day
    {
        get
        {
            return _day;
        }
    }

    public MyDateTime(int seconds)
    {
        int minute = 0;
        int hour = 0;
        int day = 0;
        if (seconds >= 60)
        {
            minute = seconds / 60;
            seconds = seconds % 60;
            if (minute >= 60)
            {
                hour = minute / 60;
                minute = minute % 60;
                if (hour >= 24)
                {
                    day = (int)((hour - 0.01) / 24f) + 1;
                    hour = hour % 24;
                }
            }
        }

        _year = 1970;
        for (int i = 1970; i < 9999; i++)
        {
            int yDay = GetDayByYear(i);
            if (day - yDay > 0)
            {
                day = day - yDay;
            }
            else
            {
                _year = i;
                break;
            }
        }

        for (int i = 1; i < 13; i++)
        {
            int mDay = GetMonthDayByYear(_year, i);
            if (day - mDay > 0)
            {
                day = day - mDay;
            }
            else
            {
                _month = i;
                break;
            }
        }

        _day = day;
        _hour = hour;
        _minute = minute;
        _seconds = seconds;
    }

    public static int GetMonthDayByYear(int year, int month)
    {
        int sum = 0;
        int leap = 0;
        switch (month)
        {
            case 1:
                sum = 31;
                break;
            case 2:
                sum = 28;
                break;
            case 3:
                sum = 31;
                break;
            case 4:
                sum = 30;
                break;
            case 5:
                sum = 31;
                break;
            case 6:
                sum = 30;
                break;
            case 7:
                sum = 31;
                break;
            case 8:
                sum = 31;
                break;
            case 9:
                sum = 30;
                break;
            case 10:
                sum = 31;
                break;
            case 11:
                sum = 30;
                break;
            case 12:
                sum = 31;
                break;
        }
        if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0))//判断是不是闰年
        {
            if (month == 2)
            {
                sum++;
            }
        }
        return sum;
    }

    private int GetDayByYear(int year)
    {
        int sum;
        int leap;
        sum = 365;
        if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0))//判断是不是闰年
            leap = 1;
        else
            leap = 0;
        if (leap == 1)//如果是闰年总天数应该加一天
            sum++;
        return sum;
    }
}

#region 统一查找方式

/// <summary>
/// 游戏对象搜索接口
/// </summary>
public interface IGameObjectFinder
{
    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="root">搜索的开始位置/根节点</param>
    /// <param name="findResult">搜索存放的结果</param>
    void Find(Transform root, List<Transform> findResult);
}

/// <summary>
/// 迭代搜索判断
/// </summary>
public interface IGameObjectFinderForIteration
{
    /// <summary>
    /// 指定节点是否合法
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    bool isVaild(Transform node);
}

/// <summary>
/// 根据组件搜索
/// </summary>
/// <typeparam name="T"></typeparam>
public class GameObjectFinderByComponent<T> : IGameObjectFinder where T : Component
{
    public void Find(Transform root, List<Transform> findResult)
    {
        foreach (var componentsInChild in root.GetComponentsInChildren<T>())
        {
            findResult.Add(componentsInChild.transform);
        }
    }
}

/// <summary>
/// 迭代遍历搜索
/// </summary>
public class GameObjectFinderByIteration : IGameObjectFinder
{
    private IGameObjectFinderForIteration finderForIteration;
    public GameObjectFinderByIteration(IGameObjectFinderForIteration finderForIteration)
    {
        this.finderForIteration = finderForIteration;
    }

    public void Find(Transform root, List<Transform> findResult)
    {
        for (int i = 0, childCount = root.childCount; i < childCount; i++)
        {
            Transform t = root.GetChild(i);
            if (finderForIteration.isVaild(t))
            {
                findResult.Add(t);
            }
            Find(t, findResult);
        }
    }
}

/// <summary>
/// 迭代遍历按名字搜索
/// </summary>
public class FinderForIterationByName : IGameObjectFinderForIteration
{
    protected readonly string NAME;
    public FinderForIterationByName(string name)
    {
        NAME = name;
    }

    public bool isVaild(Transform getChild)
    {
        return getChild.gameObject.name.Equals(NAME);
    }
}

#endregion

public class MyTools
{
    public static GameObject LoadGameObject(string Path)
    {
        if (Path == null || Path.Length == 0)
            return null;
        GameObject _objClass = (GameObject)Resources.Load(Path);
        if (_objClass)
        {
            GameObject _obj = (GameObject)GameObject.Instantiate(_objClass);
            return _obj;
        }
        else
            Debug.Log("Load-Failed " + Path);
        return null;
    }
    public static GameObject LoadGameObject(string Path, GameObject parent, Vector3 _Pos)
    {
        if (Path == null || Path.Length == 0)
            return null;
        GameObject _objClass = (GameObject)Resources.Load(Path);
        if (_objClass)
        {
            GameObject _obj = (GameObject)GameObject.Instantiate(_objClass);
            _obj.SetActive(false);
            if (parent != null)
                _obj.transform.parent = parent.transform;
            _obj.transform.localPosition = _Pos;
            _obj.transform.localScale = Vector3.one;
            _obj.SetActive(true);
            return _obj;
        }
        else
            Debug.Log("Load-Failed " + Path);
        return null;
    }
    public static T LoadGameObjectAndGetCompent<T>(string Path, GameObject parent, Vector3 _Pos) where T : Component
    {
        GameObject go = LoadGameObject(Path, parent, _Pos);
        if (go != null)
            return go.GetComponent<T>();
        else
            return null;
    }

    public static string Int2IP(uint ipCode)
    {
        byte a = (byte)((ipCode & 0xFF000000) >> 24);
        byte b = (byte)((ipCode & 0x00FF0000) >> 16);
        byte c = (byte)((ipCode & 0x0000FF00) >> 8);
        byte d = (byte)(ipCode & 0x000000FF);
        string ipStr = string.Format("{0}.{1}.{2}.{3}", d, c, b, a);
        return ipStr;
    }
    public static uint IP2Int(string ipStr)
    {
        string[] ip = ipStr.Split('.');
        uint ipCode = 0xFFFFFF00 | byte.Parse(ip[3]);
        ipCode = ipCode & 0xFFFF00FF | (uint.Parse(ip[2]) << 0x8);
        ipCode = ipCode & 0xFF00FFFF | (uint.Parse(ip[1]) << 0xF);
        ipCode = ipCode & 0x00FFFFFF | (uint.Parse(ip[0]) << 0x18);
        return ipCode;
    }
    public static Vector3 ParserVector3(string _PutIn)
    {
        Vector3 NewVec = new Vector3(0.0f, 0.0f, 0.0f);
        char[] delimiterChars = { ' ', ',', ':', '-', '\t' };
        string[] words = _PutIn.Split(delimiterChars);
        int index = 0;
        foreach (string s in words)
        {
            if (index == 0)
                NewVec.x = float.Parse(s);
            else if (index == 1)
                NewVec.y = float.Parse(s);
            else if (index == 2)
                NewVec.z = float.Parse(s);
            index++;
        }
        return NewVec;
    }
    public static ArrayList ParserIntString(string _PutIn)
    {
        ArrayList newArrayList = new ArrayList();
        char[] delimiterChars = { ' ', ',', ':', '-', '\t' };
        string[] words = _PutIn.Split(delimiterChars);
        foreach (string s in words)
        {
            newArrayList.Add(int.Parse(s));
        }
        return newArrayList;
    }
    public static ArrayList ParserFloatString(string _PutIn)
    {
        ArrayList newArrayList = new ArrayList();
        char[] delimiterChars = { ' ', ',', ':', '-', '\t' };
        string[] words = _PutIn.Split(delimiterChars);
        foreach (string s in words)
        {
            newArrayList.Add(float.Parse(s));
        }
        return newArrayList;
    }
    public static Vector2 ParserVector2(string _PutIn)
    {
        Vector2 NewVec = new Vector2(0.0f, 0.0f);
        char[] delimiterChars = { ' ', ',', ':', '-', '\t' };
        string[] words = _PutIn.Split(delimiterChars);
        int ii = 0;
        foreach (string s in words)
        {
            if (ii == 0)
                NewVec.x = float.Parse(s);
            else if (ii == 1)
                NewVec.y = float.Parse(s);
            ii++;
        }
        return NewVec;
    }
    public static Color ParserColor(string _PutIn)
    {
        Color NewVec = new Color();
        char[] delimiterChars = { ' ', ',', ':', '-', '\t' };
        string[] words = _PutIn.Split(delimiterChars);
        int ii = 0;
        foreach (string s in words)
        {
            if (ii == 0)
                NewVec.a = float.Parse(s);
            else if (ii == 1)
                NewVec.r = float.Parse(s);
            else if (ii == 2)
                NewVec.g = float.Parse(s);
            else if (ii == 3)
                NewVec.b = float.Parse(s);
            ii++;
        }
        return NewVec;
    }
  
    /// <summary>
    /// 是否是闰年
    /// </summary>
    /// <param name="iYear"></param>
    /// <returns></returns>
    public static bool IsLeapYear(ushort iYear)
    {
        return 0 == (iYear % 4) && (iYear % 100) != 0 || 0 == (iYear % 400);
    }
    public static Vector3 D3DXVec3Cross(Vector3 pV1, Vector3 pV2)
    {
        Vector3 v;
        v.x = pV1.y * pV2.z - pV1.z * pV2.y;
        v.y = pV1.z * pV2.x - pV1.x * pV2.z;
        v.z = pV1.x * pV2.y - pV1.y * pV2.x;
        return v;
    }

    public static Vector2 DirRihgt = new Vector2(1.0f, 0.0f).normalized; 
    public const int DirR = 1;
    public static Vector2 DirRightTop = new Vector2(1.0f, 1.0f).normalized; 
    public const int DirRT = 2;
    public static Vector2 DirTop = new Vector2(0.0f, 1.0f).normalized; 
    public const int DirT = 3;
    public static Vector2 DirLeftTop = new Vector2(-1.0f, 1.0f).normalized; 
    public const int DirLT = 4;
    public static Vector2 DirLeft = new Vector2(-1.0f, 0.0f).normalized; 
    public const int DirL = 5;
    public static Vector2 DirLeftBottom = new Vector2(-1.0f, -1.0f).normalized; 
    public const int DirLB = 6;
    public static Vector2 DirBottom = new Vector2(0.0f, -1.0f).normalized; 
    public const int DirB = 7;
    public static Vector2 DirRihgtBottom = new Vector2(1.0f, -1.0f).normalized; 
    public const int DirRB = 8;

    public static Vector3 TDirRihgt = new Vector3(1.0f, 0.0f).normalized;
    public static Vector3 TDirRightTop = new Vector3(1.0f, 1.0f).normalized;
    public static Vector3 TDirTop = new Vector3(0.0f, 1.0f).normalized;
    public static Vector3 TDirLeftTop = new Vector3(-1.0f, 1.0f).normalized;
    public static Vector3 TDirLeft = new Vector3(-1.0f, 0.0f).normalized;
    public static Vector3 TDirLeftBottom = new Vector3(-1.0f, -1.0f).normalized;
    public static Vector3 TDirBottom = new Vector3(0.0f, -1.0f).normalized;
    public static Vector3 TDirRihgtBottom = new Vector3(1.0f, -1.0f).normalized;

    public static Vector2 Get4XDir(Vector2 Dir, out int IntDir)
    {
        Vector2 newdir = new Vector2(0.0f, 0.0f);
        float thr = 3.14f / 4f;
        if (Dir.x > 0.0f)
        {
            if (Dir.y >= -Mathf.Sin(thr) && Dir.y <= Mathf.Sin(thr))
            {
                newdir = DirRihgt;
                IntDir = DirR;
            }
            else
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirTop;
                    IntDir = DirT;
                }
                else
                {
                    newdir = DirBottom;
                    IntDir = DirB;
                }
            }
        }
        else if (Dir.x < 0.0f)
        {
            if (Dir.y >= -Mathf.Sin(thr) && Dir.y <= Mathf.Sin(thr))
            {
                newdir = DirLeft;
                IntDir = DirL;
            }
            else
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirTop;
                    IntDir = DirT;
                }
                else
                {
                    newdir = DirBottom;
                    IntDir = DirB;
                }
            }
        }
        else
        {
            if (Dir.y > 0.0f)
            {
                newdir = DirTop;
                IntDir = DirT;
            }
            else
            {
                newdir = DirBottom;
                IntDir = DirB;
            }
        }
        return newdir;
    }
    public static Vector2 Get8XDir(Vector2 Dir, out int IntDir)
    {
        Vector2 newdir = new Vector2(0.0f, 0.0f);
        float thr = 3.14f / 8f;
        if (Dir.x > 0.0f)
        {
            if (Dir.y >= -Mathf.Sin(thr) && Dir.y <= Mathf.Sin(thr))
            {
                newdir = DirRihgt;
                IntDir = DirR;
            }
            else if (Dir.y >= -Mathf.Sin(thr * 3) && Dir.y <= Mathf.Sin(thr * 3))
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirRightTop;
                    IntDir = DirRT;
                }
                else
                {
                    newdir = DirRihgtBottom;
                    IntDir = DirRB;
                }
            }
            else
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirTop;
                    IntDir = DirT;
                }
                else
                {
                    newdir = DirBottom;
                    IntDir = DirB;
                }
            }
        }
        else if (Dir.x < 0.0f)
        {
            if (Dir.y >= -Mathf.Sin(thr) && Dir.y <= Mathf.Sin(thr))
            {
                newdir = DirLeft;
                IntDir = DirL;
            }
            else if (Dir.y >= -Mathf.Sin(thr * 3) && Dir.y <= Mathf.Sin(thr * 3))
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirLeftTop;
                    IntDir = DirLT;
                }
                else
                {
                    newdir = DirLeftBottom;
                    IntDir = DirLB;
                }
                newdir = newdir.normalized;
            }
            else
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirTop;
                    IntDir = DirLT;
                }
                else
                {
                    newdir = DirBottom;
                    IntDir = DirB;
                }
            }
        }
        else
        {
            if (Dir.y > 0.0f)
            {
                newdir = DirTop;
                IntDir = DirT;
            }
            else
            {
                newdir = DirBottom;
                IntDir = DirB;
            }
        }
        return newdir;
    }

//----------------------------------------------------------------------

    public static void ClearTransFormChild(Transform ts)
    {
        for (int i = 0; i < ts.childCount; i++)
        {
            GameObject.Destroy(ts.GetChild(i).gameObject);
        }
    }

    public static void ChangeMaskLayer(Transform ts, int masklayer)
    {
        ts.gameObject.layer = masklayer;
        for (int i = 0; i < ts.childCount; i++)
        {
            Transform go = ts.GetChild(i);
            ChangeMaskLayer(go.transform, masklayer);
        }
    }

    public static void EnabelBoxCollider(GameObject goObj)
    {
        BoxCollider[] colliders = goObj.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
    }

    public static void DisEnabelBoxCollider(GameObject goObj)
    {
        BoxCollider[] colliders = goObj.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    public static string SecondsToString(int seconds)
    {
        MyDateTime myDate = new MyDateTime(seconds + 8 * 3600);
        return myDate.Year + "年" + myDate.Month + "月" + myDate.Day + "日" + " " + myDate._hour + ":" + myDate._minute + ":" + myDate._seconds;
    }

    public static int GetHour(int seconds)
    {
        MyDateTime myDate = new MyDateTime(seconds + 8 * 3600);
        return myDate._hour;
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="path">文件创建目录</param>
    /// <param name="filename">文件的名称</param>
    /// <param name="info">写入的内容</param>
    public static void CreateFile(string path, string filename, string info)
    {
        DeleteFile(path, filename);
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "/" + filename);
        if (!t.Exists)
        {
            sw = t.CreateText();
        }
        else
        {
            sw = t.AppendText();
        }
        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }
  
    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="name">名称</param>
    public static ArrayList LoadFile(string path, string name)
    {
        //使用流的形式读取
        ArrayList arrlist = new ArrayList();
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "/" + name);
        }
        catch (Exception e)
        {
            string logE = e.ToString();
            return arrlist;
        }
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            arrlist.Add(line);
        }
        sr.Close();
        sr.Dispose();
        return arrlist;
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="path">删除文件的路径</param>
    /// <param name="name">删除文件的名称</param>
    public static void DeleteFile(string path, string name)
    {
        File.Delete(path + "/" + name);
    }

    public static string[] LoadData(string loadUrl)
    {
        TextAsset binAsset = Resources.Load(loadUrl, typeof(TextAsset)) as TextAsset;
        if (binAsset != null)
        {
            string[] lineArray = binAsset.text.Split("\n"[0]);
            return lineArray;
        }
        return null;
    }

    /// <summary>
    /// 查找指定根节点下符合指定的查找器的Transform并保持到findResult中
    /// </summary>
    /// <param name="root"></param>
    /// <param name="findResult"></param>
    /// <param name="finder"></param>
    public static void Find(Transform root, List<Transform> findResult, IGameObjectFinder finder)
    {
        if (root == null)
        {
            throw new Exception("root can not be null, it defines the starting point of the find path");
        }

        if (findResult == null)
        {
            throw new Exception("findResult can not be null, it used to collect the find result");
        }

        if (finder == null)
        {
            throw new Exception("finder can not be null, it defines how to find transform");
        }
        finder.Find(root, findResult);
    }

    //FindTest,统一搜索借口，方便维护，做新手引导比较方便
    //List<Transform> result;
    //MyTool.Find(transform,result,new GameObjectFinderByComponent<Rigidbody>());
    //MyTool.Find(transform,result,new GameObjectFinderByIteration(new FinderForIterationByName("abc")));

    public static Vector3 WordPos2NGUIPos(Vector3 wordPos)
    {
        //Vector3 pos = Vector3.zero;
        //pos = Camera.main.WorldToScreenPoint(wordPos);
        //pos = UICamera.mainCamera.ScreenToWorldPoint(pos);
        //pos.z = 0;
        //return pos;
        return Vector3.zero;
    }

	public static int GetLayerMaskValue(string s)
	{
		LayerMask mask;
		mask = 1 << LayerMask.NameToLayer(s);
		return mask.value;
	}

    //是否在三角形内
    private static float triangleArea(float v0x, float v0y, float v1x, float v1y, float v2x, float v2y)
    {
        return Mathf.Abs((v0x * v1y + v1x * v2y + v2x * v0y
            - v1x * v0y - v2x * v1y - v0x * v2y) / 2f);
    }
    public static bool isINTriangle(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        float x = point.x;
        float y = point.z;

        float v0x = v0.x;
        float v0y = v0.z;

        float v1x = v1.x;
        float v1y = v1.z;

        float v2x = v2.x;
        float v2y = v2.z;

        float t = triangleArea(v0x, v0y, v1x, v1y, v2x, v2y);
        float a = triangleArea(v0x, v0y, v1x, v1y, x, y) + triangleArea(v0x, v0y, x, y, v2x, v2y) + triangleArea(x, y, v1x, v1y, v2x, v2y);

        if (Mathf.Abs(t - a) <= 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //是否在矩形内
    private static float Multiply(float p1x, float p1y, float p2x, float p2y, float p0x, float p0y)
    {
        return ((p1x - p0x) * (p2y - p0y) - (p2x - p0x) * (p1y - p0y));
    }
    public static bool isINRect(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        float x = point.x;
        float y = point.z;

        float v0x = v0.x;
        float v0y = v0.z;

        float v1x = v1.x;
        float v1y = v1.z;

        float v2x = v2.x;
        float v2y = v2.z;

        float v3x = v3.x;
        float v3y = v3.z;

        if (Multiply(x, y, v0x, v0y, v1x, v1y) * Multiply(x, y, v3x, v3y, v2x, v2y) <= 0 && Multiply(x, y, v3x, v3y, v0x, v0y) * Multiply(x, y, v2x, v2y, v1x, v1y) <= 0)
            return true;
        else
            return false;

    }

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
    public static T FindChildrecursive<T>(Transform t) where T : MonoBehaviour
    {
        if (t.GetComponent<T>() != null)
        {
            return t.GetComponent<T>();
        }
        T r = null;
        foreach (Transform c in t)
        {
            r = FindChildrecursive<T>(c);
            if (r != null)
            {
                return r;
            }
        }
        return null;
    }
    public static T FindType<T>(GameObject g) where T : Component
    {
        Transform tall = g.transform;
        foreach (Transform t in tall)
        {
            if (t.GetComponent<T>() != null)
            {
                return t.GetComponent<T>();
            }
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
            myBones[i] = MyTools.FindChildRecursive(root.transform, smr.bones[i].name);
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
            myBones[i] = MyTools.FindChildRecursive(root.transform, copyRender.bones[i].name);
        }
        smr.bones = myBones;
    }



}









