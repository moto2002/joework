using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class CCachedTextureManager
	{
		private const int c_cachedTextureMaxAmount = 100;

		private static string s_cachedTextureDirectory = CFileManager.CombinePath(CFileManager.GetCachePath(), "HttpImage");

		private static string s_cachedTextureInfoSetFileFullPath = CFileManager.CombinePath(CCachedTextureManager.s_cachedTextureDirectory, "httpimage.bytes");

		private static byte[] s_buffer = new byte[10000];

		private CCachedTextureInfoSet m_cachedTextureInfoSet;

		public CCachedTextureManager()
		{
			this.m_cachedTextureInfoSet = new CCachedTextureInfoSet();
			if (!CFileManager.IsDirectoryExist(CCachedTextureManager.s_cachedTextureDirectory))
			{
				CFileManager.CreateDirectory(CCachedTextureManager.s_cachedTextureDirectory);
			}
			if (CFileManager.IsFileExist(CCachedTextureManager.s_cachedTextureInfoSetFileFullPath))
			{
				byte[] data = CFileManager.ReadFile(CCachedTextureManager.s_cachedTextureInfoSetFileFullPath);
				int num = 0;
				this.m_cachedTextureInfoSet.Read(data, ref num);
			}
		}

		public Texture2D GetCachedTexture(string url, float validDays)
		{
			string md = CFileManager.GetMd5(url.ToLower());
			CCachedTextureInfo cachedTextureInfo = this.m_cachedTextureInfoSet.GetCachedTextureInfo(md);
			if (cachedTextureInfo == null || (DateTime.get_Now() - cachedTextureInfo.m_lastModifyTime).get_TotalDays() >= (double)validDays)
			{
				return null;
			}
			string text = CFileManager.CombinePath(CCachedTextureManager.s_cachedTextureDirectory, md + ".bytes");
			if (!CFileManager.IsFileExist(text))
			{
				return null;
			}
			byte[] array = CFileManager.ReadFile(text);
			if (array == null || array.Length <= 0)
			{
				return null;
			}
			Texture2D texture2D = null;
			if (cachedTextureInfo.m_isGif)
			{
				using (MemoryStream memoryStream = new MemoryStream(array))
				{
					texture2D = GifHelper.GifToTexture(memoryStream, 0);
				}
			}
			else
			{
				texture2D = new Texture2D(cachedTextureInfo.m_width, cachedTextureInfo.m_height, TextureFormat.ARGB32, false);
				texture2D.LoadImage(array);
			}
			return texture2D;
		}

		public void AddCachedTexture(string url, int width, int height, bool isGif, byte[] data)
		{
			string md = CFileManager.GetMd5(url.ToLower());
			if (this.m_cachedTextureInfoSet.m_cachedTextureInfoMap.ContainsKey(md))
			{
				CCachedTextureInfo cCachedTextureInfo = null;
				this.m_cachedTextureInfoSet.m_cachedTextureInfoMap.TryGetValue(md, ref cCachedTextureInfo);
				DebugHelper.Assert(this.m_cachedTextureInfoSet.m_cachedTextureInfos.Contains(cCachedTextureInfo), "zen me ke neng?");
				cCachedTextureInfo.m_width = width;
				cCachedTextureInfo.m_height = height;
				cCachedTextureInfo.m_lastModifyTime = DateTime.get_Now();
				cCachedTextureInfo.m_isGif = isGif;
			}
			else
			{
				if (this.m_cachedTextureInfoSet.m_cachedTextureInfos.get_Count() >= 100)
				{
					string text = this.m_cachedTextureInfoSet.RemoveEarliestTextureInfo();
					if (!string.IsNullOrEmpty(text))
					{
						string text2 = CFileManager.CombinePath(CCachedTextureManager.s_cachedTextureDirectory, text + ".bytes");
						if (CFileManager.IsFileExist(text2))
						{
							CFileManager.DeleteFile(text2);
						}
					}
				}
				CCachedTextureInfo cCachedTextureInfo2 = new CCachedTextureInfo();
				cCachedTextureInfo2.m_key = md;
				cCachedTextureInfo2.m_width = width;
				cCachedTextureInfo2.m_height = height;
				cCachedTextureInfo2.m_lastModifyTime = DateTime.get_Now();
				cCachedTextureInfo2.m_isGif = isGif;
				this.m_cachedTextureInfoSet.AddTextureInfo(md, cCachedTextureInfo2);
			}
			this.m_cachedTextureInfoSet.SortTextureInfo();
			int num = 0;
			this.m_cachedTextureInfoSet.Write(CCachedTextureManager.s_buffer, ref num);
			if (CFileManager.IsFileExist(CCachedTextureManager.s_cachedTextureInfoSetFileFullPath))
			{
				CFileManager.DeleteFile(CCachedTextureManager.s_cachedTextureInfoSetFileFullPath);
			}
			CFileManager.WriteFile(CCachedTextureManager.s_cachedTextureInfoSetFileFullPath, CCachedTextureManager.s_buffer, 0, num);
			string text3 = CFileManager.CombinePath(CCachedTextureManager.s_cachedTextureDirectory, md + ".bytes");
			if (CFileManager.IsFileExist(text3))
			{
				CFileManager.DeleteFile(text3);
			}
			CFileManager.WriteFile(text3, data);
		}
	}
}
