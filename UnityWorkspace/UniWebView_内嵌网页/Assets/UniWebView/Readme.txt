For a beautiful version, please visit the officail website of UniWebView: http://uniwebview.onevcat.com

## UniWebView - An easier solution for integrating WebView to your mobile games

![UniWebView](http://uniwebview.onevcat.com/uniwebview-banner.png)

## What is UniWebView

**UniWebView** is a Unity3D package to help you using WebView easily on iOS and Android platform. You can set up a web view and embed web content in your game with less than 10 lines of code. There is also a clean and simple interface for you to interact between the game and webview. 

Main features:

* A native webview ([UIWebView](http://developer.apple.com/library/ios/#documentation/uikit/reference/UIWebView_Class/Reference/Reference.html) for iOS, [WebView](http://developer.android.com/reference/android/webkit/WebView.html) for Android). Compatible with html5, css3 and javascript.
* Send a message from webpage to Unity, using a specified url scheme. You can control the game flow and run your script code by clicking a url in the web page. By this strategy, you can implement a dynamic workflow to change your game logic on air.
* Excute and eval javascript defined in Unity game or webpage. 
* The size and apperence of webview is customizable.
* Play youtube video and other media.
* All source code of C# script and native plugin is included for a reference, with detailed documentation.
* Easy debug for Unity Mac Editor. There is no need to build and run again and again in your device. You can preview and interact with the webview just in editor. (Supporting for Windows Editor is on the way)
* UniWebView supports Unity 4.1.4 and above.

## Getting Start

`UniWebView` is very easy to be integrated to your project. It just take you 3 steps to make it working. Just follow these:

1. Download and import the Unity package of `UniWebView` into your project. (If you already have an `AndroidManifest.xml`, you should uncheck the tick before this file when importing and follow [this brief merge instruction](https://github.com/onevcat/UniWebView/tree/master#merge-androidmanifestxml). I will try to keep the back compatibility as possile as I can, but sometimes there is no other ways to fix some bugs without modification of this file. **If you are updating this plugin from an eariler version, you should check the [change log](http://uniwebview.onevcat.com/ChangeLog.txt) to decide if you need reimport or merge the menifest file again.**). After importing, restart Unity to load the Edtior plugin.
2. Drag and drop the `UniWebViewObject` prefab from `UniWebView/Prefab` folder to your game scene.
3. Change the `Url` in the Inspector to a url you what to load (or you can just play without changing anything, Unity3D's homepage will be get loaded) and Play your scene, your page should show automatically after loading.

Yes, now webpage is living in your game. The Prefab supplies a simple way to start using `UniWebView`. You can play with it in Unity Mac Editor, iOS devices or Android. The `Insets` could control the size of webview and `Load On Start` and `Auto Show When Load Complete` do exactly what they say. When you are ready, let's see how to **interact with the webview**.

## Communication between webview and Unity game

The UniWebView would become USELESS if it is can just present a webpage. By using the package, you can easily talking to the webview from your game or receiving the message it sends out. You can follow the structures below to get some detail information.

### WebView to Unity

The suggested method is **using a url scheme to send message to Unity**. `UniWebView` will listen to a url starting with `uniwebview://`. When the url is loaded (regularly it is a click on the url link in your webpage by user), `UniWebView` will parse it to a [`UniWebViewMessage`](http://uniwebview.onevcat.com/reference/struct_uni_web_view_message.html) and then raise the `OnReceivedMessage` event. A `UniWebViewMessage` contains a `path` string to represent the path in the url, and a `args` dictionary to represent the parameters in the url. For example when the url 

A click on link with `uniwebview://move?direction=up&distance=1` will be parsed as

```
path = "move"
args = {
    direction = "up",
    distance = "1"
}
```

in a `UniWebViewMessage`, and you can implement your logic in a listener for `OnReceivedMessage`. There is a demo with step-by-step tutorial in the packeage to show how to use it. Check it whenever you want.

### Unity to WebView

You can run any javascript with the web page from Unity game. The javascript can be either a block of code embeded in the webpage, or a string of js code wrote in your game script. By using [`EvaluatingJavaScript`](http://uniwebview.onevcat.com/reference/class_uni_web_view.html#a7cb377ca74716229fb995630db52d175), you can call and run javascript, so you can talk to your page to do something you want. As you may noticed, this method has a return value, you can return something you want by using it and get respond from the webpage.

There is some limitation here in evaluating a javascript. First is in the Mac Editor, the alert in the page would not pop up (But the js code will be excuted as expected). The second, there is a limitation on Android and the webview can not return value after evaluating the js. If you do need a return value for it, you can open a url start with "uniwebview://" and listen to the message event as a work around.

## Other Features

Besides of the basic web page and comunication, there are some other feature UniWebView for you.

* Background transparent - In iOS, there is a gray background by default in webview. You can use `SetTransparentBackground` to set the back ground transparent.
* Clean cache - The web view will keep the url request by default, which may cause an old page showed even if you update your web page. Use `CleanCache` to solve this problem.
* Go back and go forward - Just behaves as a browser, you can control the webpage navigation in your game, by `GoBack` and `GoForward` method.
* Play youtube video with the webview - Just load the url and it can play.
* Close by back button or native tool bar - There is a built-in toolbar to control the navigation and dismiss on iOS. Also use back button to go back and close webview on Android.
* Multiple webviews. If you want more than one webview in your game, just instantiate another gameObject and add the script to it.

### Merge AndroidManifest.xml

If you already have an AndroidManifest.xml file in your project, you should not import the UniWebView's and add or update something to your old AndroidManifest.xml yourself. Don't worry about it, it's fairly simple. After you importing the package to your project without AndroidManifest.xml, you could follow these steps to make it work:

1. Open your AndroidManifest.xml file with any text editor you like. The file should be loacated in Assets/Plugins/Android.
2. Search for `android.intent.action.MAIN` in your AndroidManifest.xml. Regularly, there should be one and only one in the file.
3. Merge and add as following.

(1) The result of Step 2 should be located in a pair of `<activity>` and `</activity>`. Insert the two entries below above the `</activity>` tag. If they are already there and having the same value, you can leave them there and go to (2). If the value is not proper, you should change them to the same as below.

```xml
<meta-data android:name="android.app.lib_name" android:value="unity" />
<meta-data android:name="unityplayer.ForwardNativeEventsToDalvik" android:value="true" />
```
(2) (This is an optional step, if there is some text input field and you want do some keyboard input in the web page, you have to do this step. Otherwise, you can skip it) In the <activity...> tag, change the value of `android:name` to `"com.onevcat.uniwebview.AndroidPlugin"`. This will use a subclass of Unity's activity to start the game, by doing so we can avoid some issues for web view in Android. If you are using a main activity other than `com.unity3d.player.UnityPlayerNativeActivity`, `com.unity3d.player.UnityPlayerActivity` or `com.unity3d.player.UnityPlayerProxyActivity`, you have to modify the source code shipped with UniWebView to your own customization, which is far beyond the start manual. You can refer to [this post](https://docs.mobage.com/display/WWNATIVE/Customizing+the+Default+Unity+Activity+Android) to know more about activity customization.

(3) In the <activity...> tag, add `android:hardwareAccelerated="true"` into it. This will enable the html5 feature for Android.

After these three steps, the activity would be something like this:

```xml
<activity android:name="com.onevcat.uniwebview.AndroidPlugin"
    android:label="@string/app_name"
    android:hardwareAccelerated="true"
    android:configChanges="fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen">
    <intent-filter>
        <action android:name="android.intent.action.MAIN" />
        <category android:name="android.intent.category.LAUNCHER" />
    </intent-filter>
    <meta-data android:name="android.app.lib_name" android:value="unity" />
    <meta-data android:name="unityplayer.ForwardNativeEventsToDalvik" android:value="true" />
</activity>
```

(4) Then you need add an new activity for possible added custom view for webview. Add the following lines below the `</activity>` tag in (1)

```xml
<activity android:name="com.onevcat.uniwebview.UniWebViewCustomViewActivity"
    android:label="@string/app_name"
    android:hardwareAccelerated="true"
    android:theme="@android:style/Theme.Black.NoTitleBar.Fullscreen"
    android:configChanges="fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen">
    <meta-data android:name="android.app.lib_name" android:value="unity" />
    <meta-data android:name="unityplayer.ForwardNativeEventsToDalvik" android:value="true" />
</activity>
```

(5) Add the permision for accessing to Internet, if there is not one yet. Add the next line just before the end of manifest, the line above the `</manifest>` tag.

```xml
<uses-permission android:name="android.permission.INTERNET" />
```

Now you should have done it. You can [go back to the Getting Start](https://github.com/onevcat/UniWebView/tree/master#getting-start) or just continue to play with the plugin as you like.

## Demo Example

There is a step-by-step demo to help you get started with UniWebView. You can open the DemoScene under UniWebView/Demo folder to play with it. In that demo, you can get a basic idea how UniWebView works between native webview and your Unity game. You can follow the UniWebDemo.cs to setup your webview by code. And of course don't forget we have a `UniWebViewObject` prefab in UniWebView/Prefab folder for you to quickly start without any other effort.

## FAQ

You can find the frequently asked questions here. If you want to know something not mentioned here, please send me a mail or post a new thread in forum.

#### Does the package contains all source code or a dll? Can I modify it?

All source code of the Unity side C# and native .m/.java files are included in the package. You can modify these code to fit your project better, but you should observe the [EULA of Unity Asset Store](http://unity3d.com/company/legal/as_terms) you agreed when you register your Unity Asset Store account. You have no rights to reproduce, duplicate, copy, sell, trade or resell this package.

#### Can I use it to play some youtube video?

Yes. UniWebView support playing youtube on web page. More than that, UniWebView is compatible with html 5 and javascript. Other videoes embedded by html 5 or webpages containing html 5 elements will work correctly in UniWebView. There is bug cause it does not work in version 1.0.1, if there are problems, please update the plugin.

#### Can I use the webview as a texture in the game?

No, UniWebView is not designed to use as a texture. It is a view added above Unity's view, without interrupt Unity game. You can set the size of webview so you can decide if the Unity game scene could be seen or not.

#### I can not input text in some Android device, what happened?

Please check if you set AndroidManifest.xml correctly. UniWebView need to start from a activity subclassed from UnityPlayerActivity and run as the main activity to solve a Unity issue which cause to input response. Follow the `Merge AndroidManifest.xml` section to config it properly. If you can not get it works, feel free to contact me.

#### I am already using another plugin as the main activity in AndroidManifest.xml, what can I do?

You have to modify either of that plugin or UniWebView to make they live together happily. Fortunately, UniWebView is shipped with source code contained, so you can modify it for your own use as you like. Please refer to [this tutorial](http://uniwebview.onevcat.com/recompile.html) about how to do it, and if you encountered any problems during it, please feel free to let me know.

#### Can I load local html files by using UniWebView?

Yes, UniWebView can load local html file. Put your files in the StreamingAssets folder (`Assets/StreamingAssets`), and use set the url property of UniWebView as below:
 
* For Mac Editor and iOS: `_webView.url = Application.streamingAssetsPath + "/yourWebPage.html";`
* For Android: `_webView.url = "file:///android_asset/yourWebPage.html";`

If you are using "Split Application Binary" for Android build (obb files), you should not put your local html files under `StreamingAssets` folder. Instead, you can put them to `Assets/Plugins/Android/asset/` and then you can use the same url (like "file:///android_asset/yourWebPage.html") to load it.

#### Can I load some html string by using UniWebView?

Yes, you can load a html string by calling `LoadHTMLString` on the webview object. When you are using this method to load a webview, the url property will not take effect. You can also set a base url for the resources and search path for the webview. Please see the [script reference](http://uniwebview.onevcat.com/reference) and demo code for more.


## Script Reference & Support Forum

You can find the [script reference here](http://uniwebview.onevcat.com/reference). There is also a [support forum](https://groups.google.com/forum/#!forum/uni_webview) for you to ask anything about `UniWebView`. You can also [submit an issue](https://github.com/onevcat/UniWebView/issues) if you encountered anything wrong. Once confirmed, I will fix them as soon as possible. Hope `UniWebView` can accelerate your development progress. [Get it](https://www.assetstore.unity3d.com/#/content/12476) now, enjoy it and have a good day :)

You can see all [change log here](http://uniwebview.onevcat.com/ChangeLog.txt).