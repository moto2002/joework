using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
///贴图类 
/// </summary>
public class ImageInfo
{
    public string name;
    public Rect rect;
    public Vector2 vec2;

    public ImageInfo(string[] str)
    {
        if (str != null)
        {
            this.name = str[0];
            float x, y, w, h;
            x = float.Parse(str[1]);
            y = float.Parse(str[2]);
            w = float.Parse(str[3]);
            h = float.Parse(str[4]);
            this.rect = new Rect(x, y, w, h);
            float pivotX, pivotY;
            pivotX = float.Parse(str[5]);
            pivotY = float.Parse(str[6]);
            this.vec2 = new Vector2(pivotX, pivotY);
        }
    }
}

/// <summary>
/// 图集管理类
/// </summary>
public class AtlasMgr
{
    public static Dictionary<string, List<ImageInfo>> imgDic = new Dictionary<string, List<ImageInfo>>();
}


public class Test : MonoBehaviour 
{
    string url = "test";
    public TextAsset mTA;
	public Texture2D mTex;
	public GameObject UIRoot;
	List<Image> images = new List<Image>();

	void Start () 
    {
        this.LoadData(url);

        for (int i = 0; i < AtlasMgr.imgDic[url].Count; i++)
        {
            ImageInfo _temp = AtlasMgr.imgDic[url][i];
            Debug.Log(_temp.name + "|"+_temp.rect + "|" + _temp.vec2);
        }

		int count = this.UIRoot.transform.childCount;
		for (int j = 0; j < count; j++)
		{
			this.images.Add( this.UIRoot.transform.Find("Image" + j).GetComponent<Image>());
		}
	}

	/// <summary>
	/// 加载图集的配置信息
	/// </summary>
	/// <param name="loadUrl">Load URL.</param>
    private void LoadData(string loadUrl)
    {
        if (!AtlasMgr.imgDic.ContainsKey(loadUrl))
        {
            AtlasMgr.imgDic.Add(loadUrl, new List<ImageInfo>());
        }

        TextAsset binAsset = mTA;// Resources.Load(loadUrl, typeof(TextAsset)) as TextAsset;
		string[] lineArray = binAsset.text.Split(new char[]{'\n'});

        for (int i = 0; i < lineArray.Length; i++)
        {
            lineArray[i] = lineArray[i].Replace("\r", "");
			string[] strArray = lineArray[i].Split(new char[] {';'});
            ImageInfo imgInfo = new ImageInfo(strArray);
            AtlasMgr.imgDic[loadUrl].Add(imgInfo);
        }
    }

	void OnGUI()
	{
		if(GUILayout.Button("换图"))
		{
			for (int i = 0; i < this.images.Count; i++) 
			{
				this.images[i].sprite = Sprite.Create(this.mTex,AtlasMgr.imgDic["test"][i].rect,AtlasMgr.imgDic["test"][i].vec2);
			}
		}
	}
}
