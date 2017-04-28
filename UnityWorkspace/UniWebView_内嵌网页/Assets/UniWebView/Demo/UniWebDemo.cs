
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a demo script to show how to use UniWebView.
/// You can follow the step 1 to 10 and get started with the basic use of UniWebView.
/// </summary>
public class UniWebDemo : MonoBehaviour
{
    public GameObject panel;
    public Canvas mCanvas;
    public RectTransform mContent;

    //Just let it compile on platforms beside of iOS and Android
    //If you are just targeting for iOS and Android, you can ignore this
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8

    //1. First of all, we need a reference to hold an instance of UniWebView
    private UniWebView _webView;

    private string _errorMessage;
    private Vector3 _moveVector;

    void Start()
    {
    }


    void Update()
    {

    }

    public void Open()
    {
        //2. You can add a UniWebView either in Unity Editor or by code.
        //Here we check if there is already a UniWebView component. If not, add one.
        _webView = GetComponent<UniWebView>();
        if (_webView == null)
        {
            _webView = gameObject.AddComponent<UniWebView>();
            _webView.OnReceivedMessage += OnReceivedMessage;
            _webView.OnLoadComplete += OnLoadComplete;
            _webView.OnWebViewShouldClose += OnWebViewShouldClose;
            _webView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;

            _webView.InsetsForScreenOreitation += InsetsForScreenOreitation;
        }

        //3. You can set the insets of this webview by assigning an insets value simply
        //   like this:
        /*
                int bottomInset = (int)(UniWebViewHelper.screenHeight * 0.5f);
                _webView.insets = new UniWebViewEdgeInsets(5,5,bottomInset,5);
        */

        // Or you can also use the `InsetsForScreenOreitation` delegate to specify different
        // insets for portrait or landscape screen. If your webpage should resize on both portrait
        // and landscape, please use the delegate way. See the `InsetsForScreenOreitation` method
        // in this file for more.

        // Now, set the url you want to load.
        _webView.url = "http://chat56op.live800.com/live800/chatClient/chatbox.jsp?companyID=758310&configID=101237&jid=3350361496";// "http://www.baidu.com";//"http://uniwebview.onevcat.com/demo/index1-1.html";
                                                                                                                                    //You can read a local html file, by putting the file into /Assets/StreamingAssets folder
                                                                                                                                    //And use the url like these
                                                                                                                                    //If you are using "Split Application Binary" for Android, see the FAQ section of manual for more.
                                                                                                                                    /*
                                                                                                                                    #if UNITY_EDITOR
                                                                                                                                    _webView.url = Application.streamingAssetsPath + "/index.html";
                                                                                                                                    #elif UNITY_IOS
                                                                                                                                    _webView.url = Application.streamingAssetsPath + "/index.html";
                                                                                                                                    #elif UNITY_ANDROID
                                                                                                                                    _webView.url = "file:///android_asset/index.html";
                                                                                                                                    #elif UNITY_WP8
                                                                                                                                    _webView.url = "Data/StreamingAssets/index.html";
                                                                                                                                    #endif
                                                                                                                                    */

        // You can set the spinner visibility and text of the webview.
        // This line can change the text of spinner to "Wait..." (default is  "Loading...")
        //_webView.SetSpinnerLabelText("Wait...");
        // This line will tell UniWebView to not show the spinner as well as the text when loading.
        //_webView.SetShowSpinnerWhenLoading(false);

        //4.Now, you can load the webview and waiting for OnLoadComplete event now.
        _webView.Load();

        _errorMessage = null;

        //You can also load some HTML string instead from a url or local file.
        //When loading from the HTML string, the _webView.url will take no effect.
        //_webView.LoadHTMLString("<body>I am a html string</body>",null);

        //If you want the webview show immediately, instead of the OnLoadComplete event, call Show()
        //A blank webview will appear first, then load the web page content in it
        //_webView.Show();
    }

    public void Back()
    {
        _webView.GoBack();
    }

    public void ToolBar()
    {
        if (_webView.toolBarShow)
        {
            _webView.HideToolBar(true);
        }
        else
        {
            _webView.ShowToolBar(true);
        }
    }

    public void Close()
    {
        panel.SetActive(false);

        //8. When you done your work with the webview,
        //you can hide it, destory it and do some clean work.
        _webView.Hide();
        Destroy(_webView);
        _webView.OnReceivedMessage -= OnReceivedMessage;
        _webView.OnLoadComplete -= OnLoadComplete;
        _webView.OnWebViewShouldClose -= OnWebViewShouldClose;
        _webView.OnEvalJavaScriptFinished -= OnEvalJavaScriptFinished;
        _webView.InsetsForScreenOreitation -= InsetsForScreenOreitation;
        _webView = null;

      
    }


    #region 加载完成
    //5. When the webView complete loading the url sucessfully, you can show it.
    //   You can also set the autoShowWhenLoadComplete of UniWebView to show it automatically when it loads finished.
    void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
    {
        if (success)
        {
            panel.SetActive(true);
            webView.Show();

           
        }
        else
        {
            Debug.Log("Something wrong in webview loading: " + errorMessage);
            _errorMessage = errorMessage;
        }
    }
    #endregion

    #region 接收信息
    //6. The webview can talk to Unity by a url with scheme of "uniwebview". See the webpage for more
    //   Every time a url with this scheme clicked, OnReceivedMessage of webview event get raised.
    void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
    {
        Debug.Log("Received a message from native");
        Debug.Log(message.rawMessage);

        if (string.Equals(message.path, "close"))
        {
            Close();
        }
    }
    #endregion


    //9. By using EvaluatingJavaScript method, you can talk to webview from Unity.
    //It can evel a javascript or run a js method in the web page.
    //(In the demo, it will be called when the cube hits the sphere)
    public void ShowAlertInWebview(float time, bool first)
    {
        _moveVector = Vector3.zero;
        if (first)
        {
            //Eval the js and wait for the OnEvalJavaScriptFinished event to be raised.
            //The sample(float time) is written in the js in webpage, in which we pop
            //up an alert and return a demo string.
            //When the js excute finished, OnEvalJavaScriptFinished will be raised.
            _webView.EvaluatingJavaScript("sample(" + time + ")");
        }
    }

    //In this demo, we set the text to the return value from js.
    void OnEvalJavaScriptFinished(UniWebView webView, string result)
    {
        Debug.Log("js result: " + result);
    }

    //10. If the user close the webview by tap back button (Android) or toolbar Done button (iOS),
    //    we should set your reference to null to release it.
    //    Then we can return true here to tell the webview to dismiss.
    bool OnWebViewShouldClose(UniWebView webView)
    {
        if (webView == _webView)
        {
            _webView = null;
            return true;
        }
        return false;
    }

    UniWebViewEdgeInsets InsetsForScreenOreitation(UniWebView webView, UniWebViewOrientation orientation)
    {
        Vector3[] fourCornersArray = new Vector3[4];
        mContent.GetWorldCorners(fourCornersArray);
        Camera cameraTmp = null;
        if (mCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
        }
        else
        {
            cameraTmp = mCanvas.worldCamera;
        }
        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(cameraTmp, fourCornersArray[0]);
        //Vector2 pos1 = cameraTmp.WorldToScreenPoint(fourCornersArray[1]);
        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(cameraTmp, fourCornersArray[2]);
        //  Vector2 pos3 = cameraTmp.WorldToScreenPoint(fourCornersArray[3]);

        float _top = Screen.height - topRight.y;
        float _left = bottomLeft.x;
        float _bottom = bottomLeft.y;
        float _right = Screen.width - topRight.x;

        if (orientation == UniWebViewOrientation.Portrait)  //竖屏
        {


            int offset = 0;

            return new UniWebViewEdgeInsets(UniWebViewHelper.ConvertPixelToPoint(_top,false)+ offset, UniWebViewHelper.ConvertPixelToPoint( _left,true)+ offset, UniWebViewHelper.ConvertPixelToPoint(_bottom,false)+ offset, UniWebViewHelper.ConvertPixelToPoint(_right,true)+ offset);

        }
        else//横屏
        {
            int offset = 0;

            return new UniWebViewEdgeInsets(UniWebViewHelper.ConvertPixelToPoint(_top, false) + offset, UniWebViewHelper.ConvertPixelToPoint(_left, true) + offset, UniWebViewHelper.ConvertPixelToPoint(_bottom, false) + offset, UniWebViewHelper.ConvertPixelToPoint(_right, true) + offset);
        }
    }
#else //End of #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
	void Start() {
		Debug.LogWarning("UniWebView only works on iOS/Android/WP8. Please switch to these platforms in Build Settings.");
	}
#endif
}
