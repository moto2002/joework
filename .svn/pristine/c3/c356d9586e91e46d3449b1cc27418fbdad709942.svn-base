using System;

public class TssSdtDataTypeFactory
{
	private static byte m_byte_xor_key;

	private static short m_short_xor_key;

	private static ushort m_ushort_xor_key;

	private static int m_int_xor_key;

	private static uint m_uint_xor_key;

	private static long m_long_xor_key;

	private static ulong m_ulong_xor_key;

	private static float m_float_xor_key;

	private static double m_double_xor_key;

	public static byte GetByteXORKey()
	{
		if (TssSdtDataTypeFactory.m_byte_xor_key == 0)
		{
			Random random = new Random();
			TssSdtDataTypeFactory.m_byte_xor_key = (byte)random.Next(0, 255);
		}
		TssSdtDataTypeFactory.m_byte_xor_key += 1;
		if (TssSdtDataTypeFactory.m_byte_xor_key > 255)
		{
			TssSdtDataTypeFactory.m_byte_xor_key = 0;
		}
		return TssSdtDataTypeFactory.m_byte_xor_key;
	}

	public static short GetShortXORKey()
	{
		if (TssSdtDataTypeFactory.m_short_xor_key == 0)
		{
			Random random = new Random();
			TssSdtDataTypeFactory.m_short_xor_key = (short)random.Next(0, 255);
		}
		TssSdtDataTypeFactory.m_short_xor_key += 1;
		if (TssSdtDataTypeFactory.m_short_xor_key > 255)
		{
			TssSdtDataTypeFactory.m_short_xor_key = 0;
		}
		return TssSdtDataTypeFactory.m_short_xor_key;
	}

	public static ushort GetUshortXORKey()
	{
		if (TssSdtDataTypeFactory.m_ushort_xor_key == 0)
		{
			Random random = new Random();
			TssSdtDataTypeFactory.m_ushort_xor_key = (ushort)random.Next(0, 255);
		}
		TssSdtDataTypeFactory.m_ushort_xor_key += 1;
		if (TssSdtDataTypeFactory.m_ushort_xor_key > 255)
		{
			TssSdtDataTypeFactory.m_ushort_xor_key = 0;
		}
		return TssSdtDataTypeFactory.m_ushort_xor_key;
	}

	public static int GetIntXORKey()
	{
		if (TssSdtDataTypeFactory.m_int_xor_key == 0)
		{
			Random random = new Random();
			TssSdtDataTypeFactory.m_int_xor_key = random.Next(0, 65535);
		}
		TssSdtDataTypeFactory.m_int_xor_key++;
		if (TssSdtDataTypeFactory.m_int_xor_key > 65535)
		{
			TssSdtDataTypeFactory.m_int_xor_key = 0;
		}
		return TssSdtDataTypeFactory.m_int_xor_key;
	}

	public static uint GetUintXORKey()
	{
		if (TssSdtDataTypeFactory.m_uint_xor_key == 0u)
		{
			Random random = new Random();
			TssSdtDataTypeFactory.m_uint_xor_key = (uint)random.Next(0, 65535);
		}
		TssSdtDataTypeFactory.m_uint_xor_key += 1u;
		if (TssSdtDataTypeFactory.m_uint_xor_key > 65535u)
		{
			TssSdtDataTypeFactory.m_uint_xor_key = 0u;
		}
		return TssSdtDataTypeFactory.m_uint_xor_key;
	}

	public static long GetLongXORKey()
	{
		if (TssSdtDataTypeFactory.m_long_xor_key == 0L)
		{
			Random random = new Random();
			TssSdtDataTypeFactory.m_long_xor_key = (long)random.Next(0, 65535);
		}
		TssSdtDataTypeFactory.m_long_xor_key += 1L;
		if (TssSdtDataTypeFactory.m_long_xor_key > 65535L)
		{
			TssSdtDataTypeFactory.m_long_xor_key = 0L;
		}
		return TssSdtDataTypeFactory.m_long_xor_key;
	}

	public static ulong GetUlongXORKey()
	{
		if (TssSdtDataTypeFactory.m_ulong_xor_key == 0uL)
		{
			Random random = new Random();
			TssSdtDataTypeFactory.m_ulong_xor_key = (ulong)((long)random.Next(0, 65535));
		}
		TssSdtDataTypeFactory.m_ulong_xor_key += 1uL;
		if (TssSdtDataTypeFactory.m_ulong_xor_key > 65535uL)
		{
			TssSdtDataTypeFactory.m_ulong_xor_key = 0uL;
		}
		return TssSdtDataTypeFactory.m_ulong_xor_key;
	}

	public static float GetFloatXORKey()
	{
		if (TssSdtDataTypeFactory.m_float_xor_key == 0f)
		{
			Random random = new Random();
			TssSdtDataTypeFactory.m_float_xor_key = (float)random.Next(0, 255);
		}
		TssSdtDataTypeFactory.m_float_xor_key += 1f;
		if (TssSdtDataTypeFactory.m_float_xor_key > 255f)
		{
			TssSdtDataTypeFactory.m_float_xor_key = 0f;
		}
		return TssSdtDataTypeFactory.m_float_xor_key;
	}

	public static double GetDoubleXORKey()
	{
		if (TssSdtDataTypeFactory.m_double_xor_key == 0.0)
		{
			Random random = new Random();
			TssSdtDataTypeFactory.m_double_xor_key = (double)random.Next(0, 255);
		}
		TssSdtDataTypeFactory.m_double_xor_key += 1.0;
		if (TssSdtDataTypeFactory.m_double_xor_key > 255.0)
		{
			TssSdtDataTypeFactory.m_double_xor_key = 0.0;
		}
		return TssSdtDataTypeFactory.m_double_xor_key;
	}
}
