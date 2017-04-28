using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


class UIGridDyn : UIWidgetContainer
{
    public int cellheight = 80;//单位高度
    public int cellwidth = 320;
    public int cutLineHeight = 10;
    public int showline = 10;//一次显示几个单位
    public int colcount = 1;//一行几个单位
    public GameObject srcObj; //复制用单位
    public GameObject srcCutLine; //复制用单位

    int startline = 0;//第一个显示的单位

    GameObject rect;
    UIPanel mPanel;
    int CurY = 0;//分配项位置
    public class ItemInfo
    {
        public int YInGrid = 0;
        public int XInGrid = 0;
        public GameObject itemGameObject = null;
        public string itemKey;
        public IItemObj obj;
    }
    public interface IItemObj
    {
        void OnItemShow(string key, GameObject obj);
        void OnItemHide(string key, GameObject obj);
    }
    public class Line : List<ItemInfo>
    {
        public bool isShow()
        {
            if (this.Count == 0)
                return false;
            return (this[0].itemGameObject != null);
        }
        public bool iscutline = false;
        public void Show(GameObject srcObj, Transform parent, Action<string, GameObject> callback)
        {
            foreach (var c in this)
            {
                var myItem = GameObject.Instantiate(srcObj) as GameObject;

                myItem.name = c.itemKey;
                myItem.transform.parent = parent;
                myItem.transform.localScale = new Vector3(1, 1, 1);
                if (callback != null)
                {
                    callback(c.itemKey, myItem);
                }
                if (c.obj != null)
                {
                    c.obj.OnItemShow(c.itemKey, myItem);
                }

                myItem.transform.localPosition = new Vector3(c.XInGrid, -c.YInGrid, myItem.transform.position.z);

                c.itemGameObject = myItem;
            }
        }
        public void Hide(Action<string, GameObject> callback)
        {
            foreach (var c in this)
            {
                if (callback != null)
                {
                    callback(c.itemKey, c.itemGameObject);

                }
                if (c.obj != null)
                {
                    c.obj.OnItemHide(c.itemKey, c.itemGameObject);
                }
                GameObject.Destroy(c.itemGameObject);
                c.itemGameObject = null;
            }

        }
    }
    List<Line> infos = new List<Line>();
    public void AddItem(string key, IItemObj obj = null)
    {
        ItemInfo info = new ItemInfo();
        info.itemKey = key;
        info.YInGrid = CurY;
        info.obj = obj;

        if (infos.Count == 0||infos[infos.Count-1].iscutline) infos.Add(new Line());
        if (infos[infos.Count - 1].Count < colcount)
        {
            info.XInGrid = infos[infos.Count - 1].Count * cellwidth;
            infos[infos.Count - 1].Add(info);
        }
        else
        {
            infos.Add(new Line());
            info.XInGrid = 0;
            infos[infos.Count - 1].Add(info);

        }
        if (infos[infos.Count - 1].Count == colcount)
        {
            CurY += cellheight;
        }
        rect.GetComponent<UIWidget>().height = (int)CurY;
    }
    public void AddCutLine(string key,IItemObj obj =null)
    {
        if(infos.Count!=0)
        {
            if(infos[infos.Count-1].Count<colcount)
            {
                CurY += cellheight;
            }
        }
        ItemInfo info = new ItemInfo();
        info.itemKey = key;
        info.XInGrid = cellwidth *(colcount-1) /2;
        info.YInGrid = CurY - cellheight/2+cutLineHeight/2;
        info.obj = obj;

        infos.Add(new Line());
        infos[infos.Count - 1].iscutline = true;
        infos[infos.Count - 1].Add(info);

        CurY += cutLineHeight;
    }
    public void RemoveItem(string key)
    {
        foreach(var l in infos)
        {
            bool br = false;
            foreach(var c in l)
            {
                bool bShow = l.isShow();
                if(c.itemKey==key)
                {
                    br = true;
                    if (bShow)//如果原来可见要处理一下
                    {
                        l.Hide(onItemHide);
                    }
                    l.Remove(c);
                    if(bShow)//如果原来可见要处理一下
                    {
                        
                        if (l.Count > 0)
                        {
                            l.Show(l.iscutline?srcCutLine: srcObj, this.transform, onItemShow);
                        }
                    }
                    if(l.Count==0)//如果行被删除空了要移除一行
                    {
                        infos.Remove(l);
                    }
                    break;
                }
            }
            if (br) break;
        }
    }
    public void Resort()
    {
        List<KeyValuePair< IItemObj,string>> items = new List<KeyValuePair<IItemObj,string>>();
        foreach (var line in infos)
        {
            if (line.isShow())
            {
                line.Hide(onItemHide);
                foreach(var l in line)
                {
                    items.Add(new KeyValuePair<IItemObj, string>(l.obj, l.itemKey));
                }
            }
        }
        infos.Clear();
        CurY = 0;
        startline = 0;
        foreach(var i in items)
        {
            AddItem(i.Value, i.Key);
        }
    }
    public void ResetItem()
    {
        foreach (var line in infos)
        {
            if (line.isShow())
            {
                line.Hide(onItemHide);
                line.Show(line.iscutline ? srcCutLine : srcObj, this.transform, onItemShow);
            }
        }
    }
    void Start()
    {
        if (mPanel == null)
        {
            rect = new GameObject();
            //rect.AddComponent<BoxCollider>();
            UIWidget w = rect.AddComponent<UIWidget>();
            //w.autoResizeBoxCollider = true;
            w.pivot = UIWidget.Pivot.Top;
            rect.transform.parent = this.transform;
            rect.name = "holdposition";
            rect.transform.localScale = new Vector3(1, 1, 1);
            rect.transform.localPosition = new Vector3(0, cellheight / 2, 0);
            mPanel = NGUITools.FindInParents<UIPanel>(gameObject);

            mPanel.ConstrainTargetToBounds(this.transform, true);
        }
    }

    void Update()
    {
        
        for (int i = startline; i < startline + showline; i++)
        {
            bool bLast = false;
            if (infos.Count - 1 == i || i == startline + showline - 1)
                bLast = true;

            if (i == infos.Count)
                break;

            if (infos[i].isShow() == false)
            {//Show It
                if (infos[i].iscutline==false&&srcObj != null)
                {
                    infos[i].Show(srcObj, this.transform, onItemShow);
                }
                else if (infos[i].iscutline && srcCutLine != null)
                {
                    infos[i].Show(srcCutLine, this.transform, onItemShow);
                }
                else
                {
                    Debug.LogError("(UIGridDyn): The srcObj is null.");
                }
            }

            if (infos[i].isShow() == true)
            {
                var widget =infos[i][0].itemGameObject.GetComponent<UIWidget>();
                bool bHide=true;
                if(widget ==null)
                {
                    var trans = infos[i][0].itemGameObject.transform;

                    float x0 = -0.5f * (float)cellwidth;
                    float y0 = -0.5f * (float)cellheight;
                    float x1 = x0 + cellwidth;
                    float y1 = y0 + cellheight;

                    //Transform wt = cachedTransform;

                    var pos0 = trans.TransformPoint(x0, y0, 0f);
                    var pos1 = trans.TransformPoint(x0, y1, 0f);
                    var pos2 = trans.TransformPoint(x1, y1, 0f);
                    var pos3 = trans.TransformPoint(x1, y0, 0f);
                    bHide = !mPanel.IsVisible(pos0, pos1, pos2, pos3);
                }
                else
                {
                    bHide = !mPanel.IsVisible(widget);
                }

                if (bHide)
                {//Hide It.

                    if (i == startline && i + showline < infos.Count)//第一个被隐藏
                    {
                        infos[i].Hide(onItemHide);
                        startline++;

                    }
                    if (bLast && startline > 0)//最后一个被隐藏
                    {
                        infos[i].Hide(onItemHide);
                        startline--;
                        if (startline < 0) startline = 0;
                    }
                }

            }
        }

    }
    public event Action<string, GameObject> onItemShow;
    public event Action<string, GameObject> onItemHide;
}
