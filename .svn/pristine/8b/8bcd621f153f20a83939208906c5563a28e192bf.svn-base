//
//	UniWebViewMessage.cs
//  Created by Wang Wei(@onevcat) on 2013-10-20.
//
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This structure represent a message from webview.
/// </summary>
/// <description>
/// All url with a scheme of "uniwebview" in the webpage clicked will raise
/// the OnReceivedMessage with a UniWebViewMessage object. You can listen to 
/// that event and get path and param from webpage.
/// </description>
public struct UniWebViewMessage {
	/// <summary>
	/// Gets the raw message.
	/// </summary>
	/// <value>
	/// The raw message received from UniWebView. It should be a url containing information 
	/// delivered from webview.
	/// </value>
	public string rawMessage {get; private set;}

	/// <summary>
	/// The url scheme of this UniWebViewMessage
	/// </summary>
	/// <value>The scheme.</value>
	/// <description>
	/// Every message UniWebView can get is a url in a registered scheme.
	/// The default scheme is "uniwebview". You can register your own scheme by
	/// using the RegisterScheme method on UniWebView object.
	/// </description>
	public string scheme {get; private set;}

	/// <summary>
	/// The path of this UniWebViewMessage
	/// </summary>
	/// <value>The path.</value>
	/// <description>
	/// A url "uniwebview://yourPath?param1=value1&param2=value2", path = yourPath
	/// </description>
	public string path {get; private set;}

	/// <summary>
	/// The arguments of this UniWebViewMessage
	/// </summary>
	/// <value>The arguments.</value>
	/// <description>
	/// A url "uniwebview://yourPath?param1=value1&param2=value2", args[param1] = value1, args[param2] = value2
	/// </description>
	public Dictionary<string, string> args{get; private set;}

	/// <summary>
	/// Initializes a new instance of the <see cref="UniWebViewMessage"/> struct.
	/// </summary>
	/// <param name="rawMessage">Raw message which will be parsed to a UniWebViewMessage.</param>
	public UniWebViewMessage(string rawMessage) : this() {
		this.rawMessage = rawMessage;
		string[] schemeSplit = rawMessage.Split(new string[] {"://"}, System.StringSplitOptions.None);
		if (schemeSplit.Length >= 2) {
			this.scheme = schemeSplit[0];
			string pathAndArgsString = "";
			int index = 1;
			while (index < schemeSplit.Length) {
				pathAndArgsString = string.Concat(pathAndArgsString, schemeSplit[index]);
				index++;
			}

			string[] split = pathAndArgsString.Split("?"[0]);
			
			this.path = split[0].TrimEnd('/');
			this.args = new Dictionary<string, string>();
			if (split.Length > 1) {
				foreach (string pair in split[1].Split("&"[0])) {
					string[] elems = pair.Split("="[0]);
					if (elems.Length > 1) {
						args[elems[0]] = WWW.UnEscapeURL(elems[1]);
					}
				}
			}
		} else {
			Debug.LogError("Bad url scheme. Can not be parsed to UniWebViewMessage: " + rawMessage);
		}
	}
}