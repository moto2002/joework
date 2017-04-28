using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using System;
using System.IO;

public class MyWWWNative : IWWW
{
    float timestart;
    float timeout;
	Action<Exception, string> curEvent;
	WWW curWWW;

    public void DoUrl(string url, Action<Exception, string> onFinish, float timeout, string values = null, bool bValuesPost = false)
    {
        if (onFinish == null)
        {
            Debug.LogError("must have onFinish");
            return;
        }

        //lock (_LockObj)
        {
            if (curEvent != null)
            {
                Debug.LogError("call Dourl one by one.");
                return;
            }
            curEvent = onFinish;
        }

        timestart = DateTime.Now.Ticks;
        timeout = this.timeout;

        if (values != null)
        {
			#region value!=null
			//          if (bValuesPost)
				//            {
				//                WWWForm post = new WWWForm();
				//                post.AddField("values", values);
				//                curWWW = new WWW(url, post);
				//                return;
				//            }
				//            else
				//            {
				//                url += "&values=" + Uri.EscapeDataString(values);
				//            }
			#endregion
        }

        // Uri u = new Uri(url);
        // Debug.Log("Uri=" + u);
        Debug.Log("url=" + url);
        curWWW = new WWW(url);
    }

    public void Update()
    {
        if (curWWW != null)
        {
            if (curWWW.isDone)
            {
                if (curEvent != null)
                {
                    //string data = curWWW.text;
                    string data = "ok";
                    Debug.Log(curWWW.url); 

                    #region self

                     Debug.Log(curWWW.bytes.Length + "/" + curWWW.size + "/" + curWWW.bytesDownloaded);




					//由于依赖关系都存在于manifest中，所以在加载资源之前，要先加载manifest文件。
					//实际上在打包的时候，会有一个总的manifest文件，叫：文件名字.manifest，然后每一个小的资源分别有一个自己的manifest文件。
				   //在我们加载的时候，需要先把总的AssetBundle加载进来。
					//所以之前下载是包括总的信息那个资源

					//根据我们需要加载的资源名称，获得所有依赖资源
//					AssetBundle ab =curWWW.assetBundle;
//					AssetBundleManifest abmanifest = (AssetBundleManifest)ab.LoadAsset("AssetBundleManifest");
//					ab.Unload(false);

					//realName是想加载的AssetBundle的名字，需要带扩展名
//					string realname ="scene.unity3d";
//					string[] dependsAssetNames = abmanifest.GetAllDependencies(realname);
//					int counts =dependsAssetNames.Length;
//                    Debug.Log("count:"+counts);
//					for (int i = 0; i < counts; i++) {
//						Debug.Log( dependsAssetNames[i]);
//					}

//					AssetBundle[] abs = new AssetBundle[counts];
//
//					string path = Application.streamingAssetsPath+"/";
//					for (int index = 0; index < counts; index++) {
//						string denpendsUrl = path + dependsAssetNames[index];
//						//然后开始下载
//						//然后把www.assetbundle放到abs中
//					}

					//通过了这一步，所有的依赖资源都加载完了，可以加载目标资源了：
//				   WWW www = WWW.LoadFromCacheOrDownload(url, mainfest.GetAssetBundleHash(realName+".ab"), 0);
//					yield return www;
//					if (!string.IsNullOrEmpty(www.error))
//					{
//						Debug.Log(www.error);
//					}
//					else
//					{
//						AssetBundle ab = www.assetBundle;
//						GameObject gobj = ab.LoadAsset(realName) as GameObject;
//						if (gobj != null)
//							Instantiate(gobj);
//						ab.Unload(false);
//					}
//					foreach (AssetBundle ab in abs)
//					{
//						ab.Unload(false);
//					}
//					//到这一步，所有的资源都加载完毕了。注意的是，记得Unload，不然下次就加不进来了。
//					//或者不Unload的话，就做一个字典记录所有加载过的AssetBundle，还有它们的引用计数器。那样就可以先判断是否存在在加载。

                    #endregion
                  
   


                    Exception err = curWWW.error == null ? null : new Exception(curWWW.error);
                    curWWW = null;
                    var _event = curEvent;
                    curEvent = null;
                    _event(err, data);


                }
            }
            else
            {
                Debug.Log(curWWW.progress);

                //连接超时
                if (timeout > 0 && (DateTime.Now.Ticks - timestart) / 10000000 > timeout)
                {
                    string data = null;
                    Exception err = new TimeoutEx("RequestTimeOut");
                    curWWW = null;
                    var _event = curEvent;
                    curEvent = null;
                    _event(err, data);
                }
            }
        }
    }

    public bool InUse
    {
        get { return (curEvent != null); }
    }
}
