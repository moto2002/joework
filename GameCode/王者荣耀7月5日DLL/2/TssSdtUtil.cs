using System;

public class TssSdtUtil
{
	public static void Printf(string str)
	{
	}

	public static uint GetAvailPhys()
	{
		return 0u;
	}

	public static void PrintMem(uint mem)
	{
		if (mem < 1024u)
		{
			TssSdtUtil.Printf(string.Format("mem:{0}B", mem));
		}
		else if (mem < 1048576u)
		{
			TssSdtUtil.Printf(string.Format("mem:{0}K", mem / 1024.0));
		}
		else
		{
			TssSdtUtil.Printf(string.Format("mem:{0}M", mem / 1024.0 / 1024.0));
		}
	}
}
