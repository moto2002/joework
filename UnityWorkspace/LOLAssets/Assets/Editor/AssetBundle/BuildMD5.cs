using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;
using System;

public class BuildMD5 
{
 	public static void Build(string folderName)
	{
        string versionurl = Application.streamingAssetsPath + "/";
        string url = versionurl + folderName;
		string version = ForEach (url);
        string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string data = String.Format("{0}|{1}\n","Date",msg); 
        data += version.Trim();
        StreamWriter stream = File.CreateText(versionurl + "/Version.txt");
		stream.Write (data);
		stream.Close ();
		AssetDatabase.Refresh ();
		Debug.Log ("版本生成完成.");
	}

	private static string ForEach(string url)
	{
		string version = "";
		DirectoryInfo folder = new DirectoryInfo(url);
		FileSystemInfo[] files = folder.GetFileSystemInfos ();
		int length = files.Length;
		for (int i = 0; i < length; i++)
			if (files[i] is DirectoryInfo)
				version += ForEach (files[i].FullName);
		else
			version += Info (files[i] as FileInfo);
		return version;
	}
	
	private static string Info(FileInfo file)
	{
		if (file.Name.EndsWith(".meta"))
			return "";
        return name(file) + "|" + version(file) + "|" + size(file) + "\n";
	}
	
	private static string name(FileInfo file)
	{
		return file.FullName.Substring (Application.streamingAssetsPath.Length + 1).Replace ("\\", "/");
	}
	
	private static string size(FileInfo file)
	{
		return file.Length.ToString ();
	}

	private static string version(FileInfo file)
	{
		FileStream stream = new FileStream (file.FullName, FileMode.Open);
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ();
		byte[] bytes = md5.ComputeHash (stream);
		stream.Close ();
		StringBuilder builder = new StringBuilder ();
		for (int i = 0; i < bytes.Length; i++)
			builder.Append(bytes[i].ToString("x2"));
		return builder.ToString ();
	}
}
