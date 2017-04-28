using System;

public class Singleton<T> where T : class, new()
{
	private static T m_instance;

	public static T instance
	{
		get
		{
			if (Singleton<T>.m_instance == null)
			{
				Singleton<T>.CreateInstance();
			}
			return Singleton<T>.m_instance;
		}
	}

	protected Singleton()
	{
	}

	public static void CreateInstance()
	{
		if (Singleton<T>.m_instance == null)
		{
			Singleton<T>.m_instance = Activator.CreateInstance<T>();
			(Singleton<T>.m_instance as Singleton<T>).Init();
		}
	}

	public static void DestroyInstance()
	{
		if (Singleton<T>.m_instance != null)
		{
			(Singleton<T>.m_instance as Singleton<T>).UnInit();
			Singleton<T>.m_instance = (T)((object)null);
		}
	}

	public static T GetInstance()
	{
		if (Singleton<T>.m_instance == null)
		{
			Singleton<T>.CreateInstance();
		}
		return Singleton<T>.m_instance;
	}

	public static bool HasInstance()
	{
		return Singleton<T>.m_instance != null;
	}

	public virtual void Init()
	{
	}

	public virtual void UnInit()
	{
	}
}
