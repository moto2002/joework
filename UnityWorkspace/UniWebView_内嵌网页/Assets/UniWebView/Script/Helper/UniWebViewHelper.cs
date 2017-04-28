using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Supply some helper utility method for UniWebView
/// </summary>
public class UniWebViewHelper{
	/// <summary>
	/// Get the height of the screen.
	/// </summary>
	/// <value>
	/// The height of screen.
	/// </value>
	/// <description>
	/// In iOS devices, it will always return the screen height in "point", 
	/// instead of "pixel". It would be useful to use this value to calculate webview size.
	/// On other platforms, it will just return Unity's Screen.height.
	/// For example, a portrait iPhone 5 will return 568 and a landscape one 320. You should 
	/// always use this value to do screen-size-based insets calculation.
	/// </description>
	public static int screenHeight {
		get {
#if UNITY_IOS && !UNITY_EDITOR
			return UniWebViewPlugin.ScreenHeight();
#else
			return Screen.height;
#endif
		}
	}

	/// <summary>
	/// Get the height of the screen.
	/// </summary>
	/// <value>
	/// The height of screen.
	/// </value>
	/// <description>
	/// In iOS devices, it will always return the screen width in "point", 
	/// instead of "pixel". It would be useful to use this value to calculate webview size.
	/// On other platforms, it will just return Unity's Screen.height.
	/// For example, a portrait iPhone 5 will return 320 and a landscape one 568. You should 
	/// always use this value to do screen-size-based insets calculation.
	/// </description>
	public static int screenWidth {
		get {
#if UNITY_IOS && !UNITY_EDITOR
			return UniWebViewPlugin.ScreenWidth();
#else
			return Screen.width;
#endif
		}
	}

    public static int ConvertPixelToPoint(float pixel, bool width)
    {
#if UNITY_IOS && !UNITY_EDITOR
        float scale = 0;
        if (width)
        {
            scale = 1f * screenWidth / Screen.width;
        }
        else
        {
            scale = 1f * screenHeight / Screen.height;
        }

        return (int)(pixel * scale);
#endif

        return (int)pixel;
    }


}
