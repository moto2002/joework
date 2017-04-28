using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public static class Assist
{
    /// <summary>
    /// 时间戳 (以秒为单位)
    /// </summary>
    public static long GetTimeStamp()
    {
        return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
    }

    /// <summary> 
    /// 平铺 
    /// </summary> 
    /// <param name="sp">起始坐标点</param> 
    /// <param name="objCounts">平铺数量</param> 
    /// <param name="columns">列数</param> 
    /// <param name="radiu">物体直径</param> 
    /// <param name="span">物体间距</param> 
    /// <param name="lines">行数(默认为0，若不为0则往Y轴正方向叠加)</param> 
    /// <returns>ArrayList Vector3坐标列表</returns>       
    public static ArrayList Tile(Vector3 sp, int objCounts, int columns, float radiu, float span, int lines = 0)
    {
        float px = 0;
        float pz = 0;
        ArrayList al = new ArrayList(objCounts);
        for (int i = 1; i <= objCounts; i++)
        {
            float xx = i % columns == 0 ? (columns - 1) * radiu + columns * span : (i % columns - 1) * radiu + span * (i % columns);
            float yy = radiu;
            float zz = i % columns == 0 ? (((int)i / columns - 1) * radiu + span * (int)((i / columns - 1) + radiu)) :
                    ((int)i / columns) * radiu + span * (int)(i / columns + radiu);
            if (i == 1)
            {
                px = xx;
                pz = zz;
            }
            if (lines != 0)
            {
                yy += (i - 1) / (columns * lines) * (radiu + span);
                zz -= (i - 1) / (columns * lines) * lines * (span + radiu);
            }
            al.Add(new Vector3(xx - px, yy, zz - pz) + sp);
        }
        return al;
    }

    // TODO: 
    public static float GetUITextWidth(Text text)
    {
        return text.preferredWidth;
    }

    #region 坐标转换

    /// <summary>
    /// 鼠标坐标转到指定物体的垂直面
    /// </summary>
    /// <param name="target">目标坐标</param>
    /// <param name="inPoint">鼠标坐标</param>
    /// <param name="offset">偏移量默认为Vector3.zero</param>
    /// <returns>转换之后的坐标</returns>
    public static Vector3 MousePointToScreenPoint(Vector3 target, Vector3 inPoint, Vector3 offset = new Vector3())
    {
        return mousePointToObjScreenPosition(target, inPoint, offset);
    }

    /// <summary>
    ///  获取 UI 位置
    /// <para>世界坐标系</para>
    /// </summary>
    /// <param name="target">UI 组件</param>
    /// <param name="vertex">是否取项点位置，默认取四边中点位置</param>
    /// <param name="camera">相机</param>
    /// <returns>UI世界坐标系中的四边中点位置</returns>
    public static Vector3[] GetUIWorldPosition(RectTransform target, bool vertex = false, Camera camera = null)
    {
        return UIExist(target) && CameraExist(camera) ?
               getUIWorldPosition(target, vertex) : null;
    }

    /// <summary>
    ///  获取 UI 位置
    /// <para>屏幕坐标系</para>
    /// </summary>
    /// <param name="target">UI 组件</param>
    /// <param name="vertex">是否取项点位置，默认取四边中点位置</param>
    /// <param name="camera">相机</param>
    /// <returns>UI世界坐标系中的四边中点位置</returns>
    public static Vector3[] GetUIScreenPosition(RectTransform target, bool vertex = false, Camera camera = null)
    {
        return UIExist(target) && CameraExist(camera) ?
               getUIScreenPosition(target, camera == null ? Camera.main : camera, vertex) : null;
    }

    /// <summary>
    /// 获取 UI 大小 (0: 宽、1: 高)
    /// <pare>世界坐标系</pare>
    /// </summary>
    /// <param name="target">UI 组件</param>
    /// <param name="camera">相机</param>
    /// <returns>0: 宽、1: 高</returns>
    public static float[] GetUIWorldSize(RectTransform target, Camera camera = null)
    {
        return UIExist(target) && CameraExist(camera) ?
               getUIWorldSize(target, camera) : new float[2] { 0, 0 };
    }

    /// <summary>
    /// 获取 UI 大小 (0: 宽、 1： 高)
    /// <para>屏幕坐标系</para>
    /// </summary>
    /// <param name="target">UI</param>
    /// <param name="camera">相机</param>
    /// <returns>0: 宽、 1： 高</returns>
    public static float[] GetUIScreenSize(RectTransform target, Camera camera = null)
    {
        return UIExist(target) && CameraExist(camera) ?
               getUIScreenSize(target, camera == null ? Camera.main : camera) : new float[2] { 0, 0 };
    }

    /// <summary>
    /// 获取 UI RECT
    /// <para>0： 距离屏幕左边距离</para>
    /// <para>1： 距离屏幕上边距离</para>
    /// <para>2： 宽</para>
    /// <para>3： 高</para>
    /// </summary>
    /// <param name="target">UI</param>
    /// <param name="camera">相机</param>
    /// <returns>UI RECT</returns>
    public static float[] GetUIScreenRect(RectTransform target, Camera camera = null)
    {
        if (UIExist(target) && CameraExist(camera))
        {
            Vector3[] sp = getUIScreenPosition(target, camera == null ? Camera.main : camera, false);
            float[] ws = getUIScreenSize(target, camera == null ? Camera.main : camera);
            return new float[4] { sp[0].x, sp[1].y, ws[0], ws[1] };
        }
        return new float[4] { 0, 0, 0, 0 };
    }

    #endregion

    #region assist

    private static bool CameraExist(Camera camera)
    {
        bool exist = camera != null || Camera.main != null;
        if (!exist)
            Debug.LogError("Camera is null or no main camera exist !\n");
        return exist;
    }

    private static bool UIExist(RectTransform target)
    {
        bool exist = target != null;
        if (!exist)
            Debug.LogError("UI is null !\n");
        return exist;
    }

    private static GameObject temp;
    private static Vector3[] getUIWorldPosition(RectTransform target, bool vertex = false)
    {
        temp = temp == null ? new GameObject("AssistTempObject") : temp;
        Vector3[] p = new Vector3[4];
        temp.transform.SetParent(target.transform);
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = new Vector3(target.rect.width * -target.pivot.x, vertex ? target.rect.height * -target.pivot.y : 0, 0);
        p[0] = new Vector3(temp.transform.position.x, temp.transform.position.y, 0);
        temp.transform.localPosition = new Vector3(vertex ? target.rect.width * -target.pivot.x : 0, target.rect.height * (1 - target.pivot.y), 0);
        p[1] = new Vector3(temp.transform.position.x, temp.transform.position.y, 0);
        temp.transform.localPosition = new Vector3(target.rect.width * (1 - target.pivot.x), vertex ? target.rect.height * (1 - target.pivot.y) : 0, 0);
        p[2] = new Vector3(temp.transform.position.x, temp.transform.position.y, 0);
        temp.transform.localPosition = new Vector3(vertex ? target.rect.width * (1 - target.pivot.x) : 0, target.rect.height * -target.pivot.y, 0);
        p[3] = new Vector3(temp.transform.position.x, temp.transform.position.y, 0);
        return p;
    }

    private static Vector3[] getUIScreenPosition(RectTransform target, Camera camera, bool vertex)
    {
        Vector3[] sp = getUIWorldPosition(target, vertex);
        sp[0] = camera.WorldToScreenPoint(sp[0]);
        sp[1] = camera.WorldToScreenPoint(sp[1]);
        sp[2] = camera.WorldToScreenPoint(sp[2]);
        sp[3] = camera.WorldToScreenPoint(sp[3]);
        return sp;
    }

    private static float[] getUIWorldSize(RectTransform target, Camera camera)
    {
        Vector3[] ps = getUIWorldPosition(target);
        return new float[2] { Mathf.Abs(ps[2].x - ps[0].x), Mathf.Abs(ps[1].y - ps[3].y) };
    }

    private static float[] getUIScreenSize(RectTransform target, Camera camera)
    {
        Vector3[] sp = getUIScreenPosition(target, camera, false);
        return new float[2] { Mathf.Abs(sp[2].x - sp[0].x), Mathf.Abs(sp[1].y - sp[3].y) };
    }

    private static Vector3 mousePointToObjScreenPosition(Vector3 fatherPosition, Vector3 inPoint, Vector3 inOffset)
    {
        Vector3 screenPoi = Camera.main.WorldToScreenPoint(fatherPosition);
        Vector3 offset = fatherPosition - Camera.main.ScreenToWorldPoint(new Vector3(inPoint.x, inPoint.y, screenPoi.z));
        Vector3 curScreenPoint = new Vector3(inPoint.x, inPoint.y, screenPoi.z);
        return Camera.main.ScreenToWorldPoint(curScreenPoint) + inOffset;
    }

    #endregion


    /// <summary>
    /// 打开外部的一个进行，eg：1.exe
    /// </summary>
    /// <param name="ralativePath">进程的路劲</param>
    /// <param name="callback">进程关闭的回调</param>
    public static void StartExternalApp(string ralativePath, Action callback)
    {
        string m_ralativePath = ralativePath;
        Action m_extendAppQuitCallback = callback;
        System.Diagnostics.Process myProcess;

        myProcess = new System.Diagnostics.Process();
        myProcess.StartInfo.UseShellExecute = false;
        myProcess.StartInfo.FileName = ralativePath;
        myProcess.StartInfo.CreateNoWindow = false;
        myProcess.Start();
        //myProcess.WaitForExit();

        TimerManager.ITimer timer = null;
        timer = TimerManager.Add(-1, (x)=> 
        {
            if (!myProcess.HasExited)//没有退出
            {
            }
            else
            {
                if (callback != null)
                {
                    callback();
                    TimerManager.Remove(timer);
                }
            }
        }, -1);
    }

    /// <summary>
    /// 加载 Texture2D
    /// </summary>
    /// <param name="fileFullPath"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Texture2D LoadSprite(string fileFullPath, int width, int height)
    {
        FileStream fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read);
        byte[] bs = fs.ToBytes();
        Texture2D t = new Texture2D(width, height);
        if (t.LoadImage(bs))
            return t;
        return null;
    }
}