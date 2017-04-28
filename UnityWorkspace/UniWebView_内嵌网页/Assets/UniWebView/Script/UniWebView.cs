#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
//
//	UniWebView.cs
//  Created by Wang Wei(@onevcat) on 2013-10-20.
//
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The main class of UniWebView.
/// </summary>
/// <description>
/// Each gameObject with this script represent a webview object
/// in system. Be careful: when this script's Awake() get called, it will change the name of
/// the gameObject to make it unique in the game. So make sure this script is appeneded to a
/// gameObject that you don't care its name.
/// </description>
public class UniWebView : MonoBehaviour {
	#region Events and Delegate
	//Delegate and event
	public delegate void LoadCompleteDelegate(UniWebView webView, bool success, string errorMessage);
	/// <summary>
	/// Occurs when a UniWebView finished loading a webpage.
	/// </summary>
	/// <description>
	/// If loading finished successfully, success will be true, otherwise false and with an errorMessage.
	/// </description>
	public event LoadCompleteDelegate OnLoadComplete;

	public delegate void LoadBeginDelegate(UniWebView webView, string loadingUrl);
	/// <summary>
	/// Occurs when a UniWebView began to load a webpage.
	/// </summary>
	/// <description>
	/// You can do something with the UniWebView passed by parameter to get some info
	/// or do your things when a url begins to load
	/// Sometimes, the webpage contains some other parts besides of the main frame. Whenever these parts
	/// begin to load, this event will be fired. You can get the url from parameter to know what url is
	/// about to be loaded.
	/// It is useful when you want to get some parameters from url when user clicked a link.
	/// </description>
	public event LoadBeginDelegate OnLoadBegin;

	public delegate void ReceivedMessageDelegate(UniWebView webView, UniWebViewMessage message);
	/// <summary>
	/// Occurs when a UniWebView received message.
	/// </summary>
	/// <description>
	/// If a url with format of "uniwebview://yourPath?param1=value1&param2=value2" clicked,
	/// this event will get raised with a <see cref="UniWebViewMessage"/> object.
	/// </description>
	public event ReceivedMessageDelegate OnReceivedMessage;

	public delegate void EvalJavaScriptFinishedDelegate(UniWebView webView, string result);
	/// <summary>
	/// Occurs when a UniWebView finishes eval a javascript and returned something.
	/// </summary>
	/// <description>
	/// You can use EvaluatingJavaScript method to make the webview to eval a js.
	/// The string-returned version of EvaluatingJavaScript is removed. You should
	/// always listen this event to get the result of js.
	/// </description>
	public event EvalJavaScriptFinishedDelegate OnEvalJavaScriptFinished;

	public delegate bool WebViewShouldCloseDelegate(UniWebView webView);
	/// <summary>
	/// Occurs when on web view will be closed by native. Ask if this webview should be closed or not.
	/// </summary>
	/// The users can close the webView by tapping back button (Android) or done button (iOS).
	/// When the webview will be closed, this event will be raised.
	/// If you return false, the webview will not be closed. If you did not implement it, webview will be closed.
	/// </description>
	public event WebViewShouldCloseDelegate OnWebViewShouldClose;

	public delegate void ReceivedKeyCodeDelegate(UniWebView webView, int keyCode);
	/// <summary>
	/// Occurs when users clicks or taps any key while webview is actived. This event only fired on Android.
	/// </summary>
	/// <description>
	/// On Android, the key down event can not be passed back to Unity due to some issue in Unity 4.3's Android Player.
	/// As result, you are not able to get the key input on Android by just using Unity's Input.GetKeyDown or GetKey method.
	/// If you want to know the user input while the webview on, you can subscribe this event.
	/// This event will be fired with a int number indicating the key code tapped as parameter.
	/// You can refer to Android's documentation to find out which key is tapped (http://developer.android.com/reference/android/view/KeyEvent.html)
	/// This event will be never raised on iOS, because iOS forbids the tracking of user's keyboard or device button events.
	/// </description>
	public event ReceivedKeyCodeDelegate OnReceivedKeyCode;

	public delegate UniWebViewEdgeInsets InsetsForScreenOreitationDelegate(UniWebView webView, UniWebViewOrientation orientation);
	/// <summary>
	/// Called when the webview need to know the insets for calculating webview's frame.
	/// </summary>
    /// <description>
	/// If you need to show web content in both portrait and landscpace mode, you might want to
	/// specify different insets for the screen orientation separately.
	/// This event will be called when the Show() method gets called and the screen orientation
	/// changes. You can implement this method and check current orientation, then return correct insets.
	/// If this event is not implemented, the old insets value will be used to resize the webview size.
	/// If you do not use auto-rotating in your webview, you can ignore this method and just set the <see cref="insets"/>
	/// property directly.
    /// </description>
	public event InsetsForScreenOreitationDelegate InsetsForScreenOreitation;

	#endregion

	[SerializeField]
	private UniWebViewEdgeInsets _insets = new UniWebViewEdgeInsets(0,0,0,0);

	/// <summary>
	/// Gets or sets the insets of a UniWebView object.
	/// </summary>
	/// <value>The insets in point from top, left, bottom and right edge from the screen.</value>
	/// <description>
	/// Default value is UniWebViewEdgeInsets(0,0,0,0), which means a full screen webpage.
	/// If you want use different insets in portrait and landscape screen, use <see cref="InsetsForScreenOreitation"/>
	/// </description>
	public UniWebViewEdgeInsets insets {
		get {
			return _insets;
		}
		set {
			if (_insets != value) {
				ForceUpdateInsetsInternal(value);
			}
		}
	}

	private void ForceUpdateInsetsInternal(UniWebViewEdgeInsets insets) {
		_insets = insets;
		UniWebViewPlugin.ChangeSize(gameObject.name,
		                            this.insets.top,
		                            this.insets.left,
		                            this.insets.bottom,
		                            this.insets.right);
		#if UNITY_EDITOR
		CreateTexture(this.insets.left,
		              this.insets.bottom,
		              Screen.width - this.insets.left - this.insets.right,
		              Screen.height - this.insets.top - this.insets.bottom
                      );
        #endif
    }

    /// <summary>
    /// The url this UniWebView should load. You should set it before loading webpage.
	/// Do not use this value when you want to get the url of current page. It is only for loading the first page.
	/// The value will not change as the users navigating between the pages.
	/// Use <seealso cref="currentUrl"/> if you want to know current url.
	/// </summary>
	public string url;

	/// <summary>
	/// If true, load the set url when in script's Start() method.
	/// Otherwise, you should call Load() method yourself.
	/// </summary>
	public bool loadOnStart;

	/// <summary>
	/// If true, show the webview automatically when it finished loading.
	/// Otherwise, you should listen the OnLoadComplete event and call Show() method your self.
	/// </summary>
	public bool autoShowWhenLoadComplete;

	/// <summary>
	/// Gets the current URL of the web page.
	/// </summary>
	/// <value>The current URL of this webview.</value>
	/// <description>
	/// This value indicates the main frame url of the webpage.
	/// It will be updated only when the webpage finishs or fails loading.
	/// </description>
	public string currentUrl {
		get {
			return UniWebViewPlugin.GetCurrentUrl(gameObject.name);
		}
	}

	private bool _backButtonEnable = true;
	private bool _bouncesEnable;
	private bool _zoomEnable;
	private string _currentGUID;
	private int _lastScreenHeight;
	private bool _immersiveMode = true;

	/// <summary>
	/// Gets or sets a value indicating whether the back button of this <see cref="UniWebView"/> is enabled.
	/// </summary>
	/// <description>
	/// It is only for Android and Windows Phone 8. If set true, users can use the back button of these devices to goBack or close the web view
	/// if there is nothing to goBack. Otherwise, the back button will do nothing when the webview is shown.
	/// When set true, Unity will not receive the Input.KeyDown or other similar event in Android. You could use <seealso cref="OnReceivedKeyCode"/> to watch the key event instead.
	/// This value means nothing for iOS, since there is no back button for iOS devices.
	/// </description>
	/// <value><c>true</c> if back button enabled; otherwise, <c>false</c>. Default is true</value>.
	public bool backButtonEnable {
		get {
			return _backButtonEnable;
		}
		set {
			if (_backButtonEnable != value) {
				_backButtonEnable = value;
				#if (UNITY_ANDROID || UNITY_WP8) && !UNITY_EDITOR
				UniWebViewPlugin.SetBackButtonEnable(gameObject.name, _backButtonEnable);
				#endif
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="UniWebView"/> can bounces or not.
	/// </summary>
	/// <description>
	/// The default iOS webview has a bounces effect when drag out of edge.
	/// The default Android webview has a color indicator when drag beyond the edge.
	/// UniWebView disabled these bounces effect by default. If you want the bounces, set this property to true.
	/// This property does noting in editor or Windows Phone 8.
	/// </description>
	/// <value><c>true</c> if bounces enable; otherwise, <c>false</c>.</value>
	public bool bouncesEnable {
		get {
			return _bouncesEnable;
		}
		set {
			if (_bouncesEnable != value) {
				_bouncesEnable = value;
				#if !UNITY_EDITOR
				UniWebViewPlugin.SetBounces(gameObject.name, _bouncesEnable);
				#endif
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="UniWebView"/> can be zoomed or not.
	/// </summary>
	/// <description>
	/// If true, users can zoom in or zoom out the webpage by a pinch gesture.
	/// This propery will valid immediately on Android. But on iOS, it will not valid until the next page loaded.
	/// You can set this property before page loading, or use Reload() to refresh current page to make it valid.
	/// This property does noting in Windows Phone 8, since there is no way to control the default behavior for it.
	/// If you need to disable users zoom or pinch guesture, you need to add a viewport tag with "user-scalable=no" to your page.
	/// </description>
	/// <value><c>true</c> if zoom enabled; otherwise, <c>false</c>.</value> Default is false.
	public bool zoomEnable {
		get {
			return _zoomEnable;
		}
		set {
			if (_zoomEnable != value) {
				_zoomEnable = value;
				#if !UNITY_EDITOR
				UniWebViewPlugin.SetZoomEnable(gameObject.name, _zoomEnable);
				#endif
			}
		}
	}

	/// <summary>
	/// Get the user agent of this webview.
	/// </summary>
	/// <value>The string of user agent using by the webview.</value>
	/// <description>
	/// It is a read-only property of webview. Once it is set when the webview gets created, the user agent can not be changed again.
	/// If you want to use a customized user agent, you should call <see cref="SetUserAgent"/> method before create a webview component.
	/// </description>
	public string userAgent {
		get {
			return UniWebViewPlugin.GetUserAgent(gameObject.name);
		}
	}

	/// <summary>
	/// Get or set the alpha of current webview.
	/// </summary>
	/// <value>The alpha.</value>
	/// <description>
	/// This value indicates the alpha of webview.
	/// The value should be between 0.0~1.0, which 1.0 means opaque and 0.0 is transparent.
	/// This property will work on iOS, Android and WP8. In editor it will be always 1.0 and opaque.
	/// </description>
	public float alpha {
		get {
			return UniWebViewPlugin.GetAlpha(gameObject.name);
		}
		set {
			UniWebViewPlugin.SetAlpha(gameObject.name, Mathf.Clamp01(value));
		}
	}

	/// <summary>
	/// Get or set a value indicating whether this <see cref="UniWebView"/> is in immersive mode.
	/// </summary>
	/// <value><c>true</c> if immersive mode; otherwise, <c>false</c>.</value>
	/// <description>
	///
	/// </description>
	public bool immersiveMode {
		get {
			return _immersiveMode;
		}
		set {
			#if UNITY_ANDROID && !UNITY_EDITOR
			_immersiveMode = value;
			UniWebViewPlugin.SetImmersiveModeEnabled(gameObject.name, _immersiveMode);
			#endif
		}
	}



    /// <summary>
    /// Set user agent string for webview. This method has no effect on the webviews which are already initiated.
    /// The user agent of a UniWebView component can not be changed once it is created.
    /// If you want to change the user agent for the webview, you have to call it before creating the UniWebView instance.
    /// </summary>
    /// <param name="value">The value user agent should be. Set it to null will reset the user agent to the default one.</param>
    public static void SetUserAgent(string value) {
		UniWebViewPlugin.SetUserAgent(value);
	}

	/// <summary>
	/// Reset the user agent of webview. This method has no effect on the webviews which are already initiated.
	/// The user agent of a UniWebView component can not be changed once it is created.
	/// If you want to set user agent, use the <see cref="SetUserAgent"/> method.
	/// </summary>
	public static void ResetUserAgent() {
		UniWebViewPlugin.SetUserAgent(null);
	}

	/// <summary>
	/// Load the url set in <seealso cref="url"/> property of this UniWebView.
	/// </summary>
	public void Load() {
		string loadUrl = String.IsNullOrEmpty(url) ? "about:blank" : url.Trim();
		UniWebViewPlugin.Load(gameObject.name, loadUrl);
	}

	/// <summary>
	/// A alias method to load a specified url.
	/// </summary>
	/// <param name="aUrl">A url to set and load</param>
	/// <description>
	/// It will set the url of this UniWebView and then call Load it.
	/// </description>
	public void Load(string aUrl) {
		url = aUrl;
		Load();
	}

	/// <summary>
	/// Load a HTML string.
	/// </summary>
	/// <param name="htmlString">The content HTML string for the web page.</param>
	/// <param name="baseUrl">The base URL in which the webview should to refer other resources</param>
	/// <description>
	/// If you want to specify a local baseUrl, you need to encode it to proper format.
	/// See the "Can I load some html string by using UniWebView?" section in FAQ page (http://uniwebview.onevcat.com/faqs.html) for more.
	/// </description>
	public void LoadHTMLString(string htmlString, string baseUrl) {
		UniWebViewPlugin.LoadHTMLString(gameObject.name, htmlString, baseUrl);
	}

	/// <summary>
	/// Reload current page.
	/// </summary>
	public void Reload() {
		UniWebViewPlugin.Reload(gameObject.name);
	}

	/// <summary>
	/// Stop loading the current request. It will do nothing if the webpage is not loading.
	/// </summary>
	public void Stop() {
		UniWebViewPlugin.Stop(gameObject.name);
	}

	/// <summary>
	/// Show this UniWebView on screen.
	/// </summary>
	/// <description>
	/// Usually, it should be called when you get the LoadCompleteDelegate raised with a success flag true.
	/// The webview will not be visible until this method is called.
	/// </description>
	public void Show() {
		_lastScreenHeight = UniWebViewHelper.screenHeight;
        ResizeInternal();

		UniWebViewPlugin.Show(gameObject.name);
		#if UNITY_EDITOR
		_webViewId = UniWebViewPlugin.GetId(gameObject.name);
		_hidden = false;
		#endif
	}

	/// <summary>
	/// Send a piece of javascript to the web page and evaluate (execute) it.
	/// </summary>
	/// <param name="javaScript">A single javascript method call to be sent to and executed in web page</param>
	/// <description>
	/// Although you can write complex javascript code and evaluate it at once, a suggest way is calling this method with a single js method name.
	/// The webview will try evaluate (execute) the javascript. When it finished, OnEvalJavaScriptFinished will be raised with the result.
	/// You can add your js function to the html page by referring to the js file in your html page, or add it by using AddJavaScript(string javaScript) method.
	/// </description>
	public void EvaluatingJavaScript(string javaScript) {
		UniWebViewPlugin.EvaluatingJavaScript(gameObject.name, javaScript);
	}

	/// <summary>
	/// Add some javascript to the web page.
	/// </summary>
	/// <param name="javaScript">Some javascript code you want to add to the page.</param>
	/// <description>
	/// This method will execute the input javascript code without raising an
	/// OnEvalJavaScriptFinished event. You can use this method to add some customized js
	/// function to the web page, then use EvaluatingJavaScript(string javaScript) to execute it.
	/// This method will add js in a async way in Android, so you should call it earlier than EvaluatingJavaScript
	/// </description>
	public void AddJavaScript(string javaScript) {
		UniWebViewPlugin.AddJavaScript(gameObject.name, javaScript);
	}

	/// <summary>
	/// Hide this UniWebView.
	/// </summary>
	/// <description>
	/// Calling this method on a UniWebView will hide it.
	/// </description>
	public void Hide() {
		#if UNITY_EDITOR
		_hidden = true;
		#endif
		UniWebViewPlugin.Dismiss(gameObject.name);
	}

	/// <summary>
	/// Clean the cache of this UniWebView.
	/// </summary>
	public void CleanCache() {
		UniWebViewPlugin.CleanCache(gameObject.name);
	}

	/// <summary>
	/// Clean the cookie using in the app.
	/// </summary>
	/// <param name="key">The key under which you want to clean the cache.</param>
	/// <description>
	/// Try to clean cookies under the specified key using in the app.
	/// If you leave the key as null or send an empty string as key, all cache will be cleared.
	/// This method will clear the cookies in memory and try to
	/// sync the change to disk. The memory opreation will return
	/// right away, but the disk operation is async and could take some time.
	/// Caution, in Android, there is no way to remove a specified cookie.
	/// So this method will call setCookie method with the key to set
	/// it to an empty value instead. Please refer to Android
	/// documentation on CookieManager for more information.
	/// The key parameter will be ignored at all on Windows Phone 8. All cookie will be cleared.
	/// </description>
	public void CleanCookie(string key = null) {
		UniWebViewPlugin.CleanCookie(gameObject.name, key);
	}

	/// <summary>
	/// Set the background of webview to transparent.
	/// </summary>
	/// <description>
	/// In iOS, there is a grey background in webview. If you don't want it, just call this method to set it transparent.
	/// There is no way to set Windows Phone 8 background to transparent, so this method will do noting on Windows Phone.
	/// </description>
	[Obsolete("SetTransparentBackground is deprecated, please use SetBackgroundColor instead.")]
	public void SetTransparentBackground(bool transparent = true) {
		UniWebViewPlugin.TransparentBackground(gameObject.name, transparent);
	}

	/// <summary>
	/// Set the background color of webview.
	/// </summary>
	/// <description>
	/// Set the background color of the webview. In iOS, it will only take in action when the web page has no background color from css.
	/// There is no way to set Windows Phone 8, so this method will do noting on Windows Phone.
	/// And in OSX Editor, it is limited and can be only used to set white or clear background.
	/// </description>
	public void SetBackgroundColor(Color color) {
		UniWebViewPlugin.SetBackgroundColor(gameObject.name, color.r, color.g, color.b, color.a);
	}

	/// <summary>
	/// If the tool bar is showing or not.
	/// </summary>
	/// <description>
	/// This parameter is only available in iOS. In other platform, it will be always false.
	/// </description>
	public bool toolBarShow = false;

	/// <summary>
	/// Show the tool bar. The tool bar contains three buttons: go back, go forward and close webview.
	/// </summary>
	/// <param name="animate">If set to <c>true</c>, show it with an animation.</param>
	/// <description>
	/// The tool bar is only available in iOS. In Android and Windows Phone, you can use the back button of device to go back.
	/// </description>
	public void ShowToolBar(bool animate) {
		#if UNITY_IOS && !UNITY_EDITOR
		toolBarShow = true;
		UniWebViewPlugin.ShowToolBar(gameObject.name,animate);
		#endif
	}

	/// <summary>
	/// Hide the tool bar. The tool bar contains three buttons: go back, go forward and close webview.
	/// </summary>
	/// <param name="animate">If set to <c>true</c>, show it with an animation.</param>
	/// <description>
	/// The tool bar is only available in iOS. For Android and Windows Phone, you can use the back button of device to go back.
	/// </description>
	public void HideToolBar(bool animate) {
		#if UNITY_IOS && !UNITY_EDITOR
		toolBarShow = false;
		UniWebViewPlugin.HideToolBar(gameObject.name,animate);
		#endif
	}

	/// <summary>
	/// Set if a default spinner should show when loading the webpage.
	/// </summary>
	/// <description>
	/// The default value is true, which means a spinner will show when the webview is on, and it is loading some thing.
	/// The spinner contains a label and you can set a message to it. <see cref=""/>
	/// You can set it false if you do not want a spinner show when loading.
	/// A progress bar will be used in Windows Phone instead of a spinner.
	/// </description>
	/// <param name="show">If set to <c>true</c> show.</param>
	public void SetShowSpinnerWhenLoading(bool show) {
		UniWebViewPlugin.SetSpinnerShowWhenLoading(gameObject.name, show);
	}

	/// <summary>
	/// Set the label text for the spinner showing when webview loading.
	/// The default value is "Loading..."
	/// </summary>
	/// <param name="text">Text.</param>
	/// <description>
	/// There is no text for Windows Phone spinner, so it will do noting for it.
	/// </description>
	public void SetSpinnerLabelText(string text) {
		UniWebViewPlugin.SetSpinnerText(gameObject.name, text);
	}

	/// <summary>
	/// Set to use wide view port support or not.
	/// </summary>
	/// <param name="use">If set to <c>true</c> use view port tag in the html to determine the layout.</param>
	/// <description>
	/// This method only works (and be necessary) for Android. If you are using viewport tag in you page, you may
	/// want to enable it before you loading and showing your page.
	/// </description>
	public void SetUseWideViewPort(bool use) {
		#if UNITY_ANDROID && !UNITY_EDITOR
		UniWebViewPlugin.SetUseWideViewPort(gameObject.name, use);
		#endif
	}

	/// <summary>
	/// Determines whether the webview can go back.
	/// </summary>
	/// <returns><c>true</c> if this instance can go back, which means there is at least one page in the navigation stack below; 
	/// otherwise, <c>false</c>.</returns>
	public bool CanGoBack() {
		return UniWebViewPlugin.CanGoBack(gameObject.name);
	}

	/// <summary>
	/// Determines whether this webview can go forward.
	/// </summary>
	/// <returns><c>true</c> if this instance can go forward, which means the user did at least once back; 
	/// otherwise, <c>false</c>.</returns>
	public bool CanGoForward() {
		return UniWebViewPlugin.CanGoForward(gameObject.name);
	}

	/// <summary>
	/// Go to the previous page if there is any one.
	/// </summary>
	public void GoBack() {
		UniWebViewPlugin.GoBack(gameObject.name);
	}

	/// <summary>
	/// Go to the next page if there is any one.
	/// </summary>
	public void GoForward() {
		UniWebViewPlugin.GoForward(gameObject.name);
	}

	/// <summary>
	/// Adds the URL scheme. After be added, all link of this scheme will send a message when clicked.
	/// </summary>
	/// <param name="scheme">Scheme.</param>
	/// <description>
	/// The scheme should not contain "://". For example, if you want to receive a url like "xyz://mydomian.com", you can pass "xyz" here.
	/// </description>
	public void AddUrlScheme(string scheme) {
		UniWebViewPlugin.AddUrlScheme(gameObject.name, scheme);
	}

	/// <summary>
	/// Removes the URL scheme. After be removed, this kind of url will be handled by the webview.
	/// </summary>
	/// <param name="scheme">Scheme.</param>
	public void RemoveUrlScheme(string scheme) {
		UniWebViewPlugin.RemoveUrlScheme(gameObject.name, scheme);
	}

	private bool OrientationChanged() {
		int newHeight = UniWebViewHelper.screenHeight;
		if (_lastScreenHeight != newHeight) {
			_lastScreenHeight = newHeight;
			return true;
		} else {
			return false;
		}
	}

	private void ResizeInternal() {
		int newHeight = UniWebViewHelper.screenHeight;
		int newWidth = UniWebViewHelper.screenWidth;

		UniWebViewEdgeInsets newInset = this.insets;
		if (InsetsForScreenOreitation != null) {
			UniWebViewOrientation orientation =
				newHeight >= newWidth ? UniWebViewOrientation.Portrait : UniWebViewOrientation.LandScape;
            newInset = InsetsForScreenOreitation(this, orientation);
        }

        ForceUpdateInsetsInternal(newInset);
    }

    #region Messages from native
	private void LoadComplete(string message) {
		bool loadSuc = string.Equals(message, "");
		bool hasCompleteListener = (OnLoadComplete != null);

		if (loadSuc) {
			if (hasCompleteListener) {
				OnLoadComplete(this, true, null);
			}
			if (autoShowWhenLoadComplete) {
				Show();
			}
		} else {
			Debug.LogWarning("Web page load failed: " + gameObject.name + "; url: " + url + "; error:" + message);
			if (hasCompleteListener) {
				OnLoadComplete(this, false, message);
			}
		}
	}

	private void LoadBegin(string url) {
		Debug.Log("Begin to load: " + url);
		if (OnLoadBegin != null) {
			OnLoadBegin(this, url);
		}
	}

	private void ReceivedMessage(string rawMessage) {
		UniWebViewMessage message = new UniWebViewMessage(rawMessage);
		if (OnReceivedMessage != null) {
			OnReceivedMessage(this,message);
		}
	}

	private void WebViewDone(string message) {
		bool destroy = true;
		if (OnWebViewShouldClose != null) {
			destroy = OnWebViewShouldClose(this);
		}
		if (destroy) {
			Hide();
			Destroy(this);
		}
	}

	private void WebViewKeyDown(string message) {
		int keyCode = Convert.ToInt32(message);
		if (OnReceivedKeyCode != null) {
			OnReceivedKeyCode(this, keyCode);
		}
	}

	private void EvalJavaScriptFinished(string result) {
		if (OnEvalJavaScriptFinished != null) {
			OnEvalJavaScriptFinished(this, result);
		}
	}

	private IEnumerator LoadFromJarPackage(string jarFilePath) {
		WWW stream = new WWW(jarFilePath);
		yield return stream;
		if (stream.error != null) {
			if (OnLoadComplete != null) {
				OnLoadComplete(this,false,stream.error);
			}
			yield break;
		} else {
			LoadHTMLString(stream.text, "");
		}
	}

	#endregion

	#region Life Cycle
	void Awake() {
		_currentGUID = System.Guid.NewGuid().ToString();
		gameObject.name = gameObject.name + _currentGUID;
		UniWebViewPlugin.Init(gameObject.name,
		                      this.insets.top,
		                      this.insets.left,
		                      this.insets.bottom,
		                      this.insets.right);
		_lastScreenHeight = UniWebViewHelper.screenHeight;

		#if UNITY_EDITOR
		CreateTexture(this.insets.left,
	    	          this.insets.bottom,
	        	      Screen.width - this.insets.left - this.insets.right,
	            	  Screen.height - this.insets.top - this.insets.bottom
	              	);
		#endif
	}

	void Start() {
		if (loadOnStart) {
			Load();
		}
    }

	private void OnDestroy() {
		#if UNITY_EDITOR
		Clean();
		#endif
		UniWebViewPlugin.Destroy(gameObject.name);
		gameObject.name = gameObject.name.Replace(_currentGUID, "");
	}

	#endregion

	private void Update() {
		#if UNITY_EDITOR
		if (Application.platform == RuntimePlatform.OSXEditor) {
			_inputString += Input.inputString;
		}
		#endif

		//Handle screen auto orientation.
		if (OrientationChanged()) {
            ResizeInternal();
        }
    }





#if UNITY_ANDROID
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
#endif

    #region UnityEditor Debug
#if UNITY_EDITOR
    private Rect _webViewRect;
	private Texture2D _texture;
	private string _inputString;
	private int _webViewId;
	private bool _hidden;

	private void CreateTexture(int x, int y, int width, int height) {
		if (Application.platform == RuntimePlatform.OSXEditor) {
			int w = 1;
			int h = 1;
			while (w < width) { w <<= 1; }
			while (h < height) { h <<= 1; }
			_webViewRect = new Rect(x, y, width, height);
			_texture = new Texture2D(w, h, TextureFormat.ARGB32, false);
		}
	}

	private void Clean() {
		if (Application.platform == RuntimePlatform.OSXEditor) {
			Destroy(_texture);
			_webViewId = 0;
			_texture = null;
		}
	}

    private void OnGUI()
    {
        if (Application.platform == RuntimePlatform.OSXEditor) {
			if (_webViewId != 0 && !_hidden) {
				Vector3 pos = Input.mousePosition;
				bool down = Input.GetMouseButton(0);
				bool press = Input.GetMouseButtonDown(0);
				bool release = Input.GetMouseButtonUp(0);
				float deltaY = Input.GetAxis("Mouse ScrollWheel");
				bool keyPress = false;
				string keyChars = "";
				short keyCode = 0;
				if (_inputString.Length > 0) {
					keyPress = true;
					keyChars = _inputString.Substring(0, 1);
					keyCode = (short)_inputString[0];
					_inputString = _inputString.Substring(1);
				}

				var id = _texture.GetNativeTexturePtr().ToInt32();
				UniWebViewPlugin.InputEvent(gameObject.name,
				                            (int)(pos.x - _webViewRect.x), (int)(pos.y - _webViewRect.y), deltaY,
				                            down, press, release, keyPress, keyCode, keyChars,
				                            id);
				GL.IssuePluginEvent(_webViewId);
				Matrix4x4 m = GUI.matrix;
				GUI.matrix = Matrix4x4.TRS(new Vector3(0, Screen.height, 0),
				                           Quaternion.identity, new Vector3(1, -1, 1));
				GUI.DrawTexture(_webViewRect, _texture);
				GUI.matrix = m;
			}
		}
	}
	#endif
	#endregion
}
#endif //UNITY_IOS || UNITY_ANDROID
