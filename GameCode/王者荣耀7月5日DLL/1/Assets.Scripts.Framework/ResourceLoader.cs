using System;
using UnityEngine;

namespace Assets.Scripts.Framework
{
	public class ResourceLoader : Singleton<ResourceLoader>
	{
		public delegate void LoadCompletedDelegate();

		public delegate void BinLoadCompletedDelegate(ref byte[] rawData);

		public void LoadScene(string name, ResourceLoader.LoadCompletedDelegate finishDelegate)
		{
			Application.LoadLevel(name);
			if (finishDelegate != null)
			{
				finishDelegate();
			}
		}

		public void LoadDatabin(string name, ResourceLoader.BinLoadCompletedDelegate finishDelegate)
		{
			CBinaryObject cBinaryObject = Singleton<CResourceManager>.GetInstance().GetResource(name, typeof(TextAsset), 1, false, false).m_content as CBinaryObject;
			DebugHelper.Assert(cBinaryObject != null, "load databin fail {0}", new object[]
			{
				name
			});
			byte[] data = cBinaryObject.m_data;
			if (finishDelegate != null)
			{
				finishDelegate(ref data);
			}
			Singleton<CResourceManager>.GetInstance().RemoveCachedResource(name);
		}

		public static string GetDatabinPath(string name)
		{
			return string.Format("jar:file://{0}!/assets/databin/{1}", Application.dataPath, name);
		}
	}
}
