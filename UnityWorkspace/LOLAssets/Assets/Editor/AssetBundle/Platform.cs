using UnityEngine;
using System.Collections;
using UnityEditor;

public class Platform 
{
	public static string GetPlatformFolder(BuildTarget target)
	{
		switch (target)
		{
		case BuildTarget.Android:
			return "Android";
		case BuildTarget.iOS:
			return "IOS";
		case BuildTarget.WebPlayer:
			return "WebPlayer";
		case BuildTarget.StandaloneWindows:
		case BuildTarget.StandaloneWindows64:
			return "Windows";
		case BuildTarget.StandaloneOSXIntel:
		case BuildTarget.StandaloneOSXIntel64:
		case BuildTarget.StandaloneOSXUniversal:
			return "OSX";
		default:
			return null;
		}	
	}
}
