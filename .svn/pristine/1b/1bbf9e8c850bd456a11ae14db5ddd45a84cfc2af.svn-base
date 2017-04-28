using System.IO;
using UnityEngine;
using System;
using System.Security.Cryptography;
using com.shihuanjue.game.Log;

public static class Utils
{

	public static string GetDirectoryName(string fileName)
	{
		return fileName.Substring(0, fileName.LastIndexOf('/'));
	}
	
	public static byte[] LoadByteFile(string fileName)
	{
		if (File.Exists(fileName))
		{
			return File.ReadAllBytes(fileName);
		}
		return null;
	}
	
	public static byte[] LoadByteResource(string fileName)
	{
		TextAsset asset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
		byte[] buffer = asset.bytes;
		Resources.UnloadAsset(asset);
		return buffer;
	}
	
	
	public static string LoadFile(string fileName)
	{
		if (File.Exists(fileName))
		{
			using (StreamReader reader = File.OpenText(fileName))
			{
				return reader.ReadToEnd();
			}
		}
		return string.Empty;
	}
	
	public static string LoadFileResource(string fileName)
	{
		UnityEngine.Object obj2 = Resources.Load(fileName);
		if (obj2 != null)
		{
			string str = obj2.ToString();
			Resources.UnloadAsset(obj2);
			return str;
		}
		return string.Empty;
	}
	
	

	public static string BuildFileMd5(string filename)
	{
		string str = null;
		try
		{
			using (FileStream stream = File.OpenRead(filename))
			{
				str = FormatMD5(MD5.Create().ComputeHash(stream));
			}
		}
		catch (Exception exception)
		{
			LogHelper.Except(exception, null);
		}
		return str;
	}

	public static string FormatMD5(byte[] data)
	{
		return BitConverter.ToString(data).Replace("-", "").ToLower();
	}

	

	public static bool ParseQuaternion(string _inputString, out Quaternion result)
	{
		string str = _inputString.Trim();
		result = new Quaternion();
		if (str.Length < 9)
		{
			return false;
		}
		try
		{
			string[] strArray = str.Split(new char[] { ',' });
			if (strArray.Length != 4)
			{
				return false;
			}
			result.x = float.Parse(strArray[0]);
			result.y = float.Parse(strArray[1]);
			result.z = float.Parse(strArray[2]);
			result.w = float.Parse(strArray[3]);
			return true;
		}
		catch (Exception exception)
		{
			LogHelper.Error("Parse Quaternion error: " + str + exception.ToString(), true);
			return false;
		}
	}
	
	public static bool ParseVector3(string _inputString, out Vector3 result)
	{
		string str = _inputString.Trim();
		result = new Vector3();
		if (str.Length < 7)
		{
			return false;
		}
		try
		{
			string[] strArray = str.Split(new char[] { ',' });
			if (strArray.Length != 3)
			{
				return false;
			}
			result.x = float.Parse(strArray[0]);
			result.y = float.Parse(strArray[1]);
			result.z = float.Parse(strArray[2]);
			return true;
		}
		catch (Exception exception)
		{
            LogHelper.Error("Parse Vector3 error: " + str + exception.ToString(), true);
			return false;
		}
	}
	

	

	
	


}