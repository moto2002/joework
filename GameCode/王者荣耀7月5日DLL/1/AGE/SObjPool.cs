using System;

namespace AGE
{
	public class SObjPool<T> where T : new()
	{
		public static ListView<T> list = new ListView<T>();

		private static int allocNum = 0;

		public static T New()
		{
			T result;
			if (SObjPool<T>.list.get_Count() > 0)
			{
				int num = SObjPool<T>.list.get_Count() - 1;
				result = SObjPool<T>.list.get_Item(num);
				SObjPool<T>.list.RemoveAt(num);
			}
			else
			{
				SObjPool<T>.allocNum++;
				result = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			}
			return result;
		}

		public static void Delete(T v)
		{
			SObjPool<T>.list.Add(v);
		}

		public static void Alloc(int num)
		{
			int num2 = num - SObjPool<T>.list.get_Count();
			for (int i = 0; i < num2; i++)
			{
				SObjPool<T>.allocNum++;
				SObjPool<T>.list.Add((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			}
		}
	}
}
