using System;

public class TssSdtLong
{
	private long[] m_value;

	private long[] m_xor_key;

	private int m_index;

	public TssSdtLong()
	{
		this.m_value = new long[3];
		this.m_xor_key = new long[3];
		this.m_index = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_value[i] = 0L;
			this.m_xor_key[i] = 0L;
		}
	}

	private long GetValue()
	{
		int index = this.m_index;
		long num = this.m_value[index];
		return num ^ this.m_xor_key[index];
	}

	private void SetValue(long v)
	{
		long longXORKey = TssSdtDataTypeFactory.GetLongXORKey();
		int num = this.m_index + 1;
		if (num >= 3)
		{
			num = 0;
		}
		long num2 = v ^ longXORKey;
		this.m_value[num] = num2;
		this.m_xor_key[num] = longXORKey;
		this.m_index = num;
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return this.GetValue().GetHashCode();
	}

	public override string ToString()
	{
		return string.Format("{0}", this.GetValue());
	}

	public static bool operator ==(TssSdtLong a, TssSdtLong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() == 0L;
		}
		return a.GetValue() == b.GetValue();
	}

	public static bool operator ==(TssSdtLong a, long b)
	{
		if (object.Equals(a, null))
		{
			return 0L == b;
		}
		return a.GetValue() == b;
	}

	public static bool operator ==(long a, TssSdtLong b)
	{
		if (object.Equals(b, null))
		{
			return a == 0L;
		}
		return a == b.GetValue();
	}

	public static bool operator !=(TssSdtLong a, TssSdtLong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() != 0L;
		}
		return a.GetValue() != b.GetValue();
	}

	public static bool operator !=(TssSdtLong a, long b)
	{
		if (object.Equals(a, null))
		{
			return 0L != b;
		}
		return a.GetValue() != b;
	}

	public static bool operator !=(long a, TssSdtLong b)
	{
		if (object.Equals(b, null))
		{
			return a != 0L;
		}
		return a != b.GetValue();
	}

	public static bool operator <=(TssSdtLong a, TssSdtLong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() <= 0L;
		}
		return a.GetValue() <= b.GetValue();
	}

	public static bool operator <=(TssSdtLong a, long b)
	{
		if (object.Equals(a, null))
		{
			return 0L <= b;
		}
		return a.GetValue() <= b;
	}

	public static bool operator <=(long a, TssSdtLong b)
	{
		if (object.Equals(b, null))
		{
			return a <= 0L;
		}
		return a <= b.GetValue();
	}

	public static bool operator >=(TssSdtLong a, TssSdtLong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() >= 0L;
		}
		return a.GetValue() >= b.GetValue();
	}

	public static bool operator >=(TssSdtLong a, long b)
	{
		if (object.Equals(a, null))
		{
			return 0L >= b;
		}
		return a.GetValue() >= b;
	}

	public static bool operator >=(long a, TssSdtLong b)
	{
		if (object.Equals(b, null))
		{
			return a >= 0L;
		}
		return a >= b.GetValue();
	}

	public static bool operator >(TssSdtLong a, TssSdtLong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() > 0L;
		}
		return a.GetValue() > b.GetValue();
	}

	public static bool operator >(TssSdtLong a, long b)
	{
		if (object.Equals(a, null))
		{
			return 0L > b;
		}
		return a.GetValue() > b;
	}

	public static bool operator >(long a, TssSdtLong b)
	{
		if (object.Equals(b, null))
		{
			return a > 0L;
		}
		return a > b.GetValue();
	}

	public static bool operator <(TssSdtLong a, TssSdtLong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() < 0L;
		}
		return a.GetValue() < b.GetValue();
	}

	public static bool operator <(TssSdtLong a, long b)
	{
		if (object.Equals(a, null))
		{
			return 0L < b;
		}
		return a.GetValue() < b;
	}

	public static bool operator <(long a, TssSdtLong b)
	{
		if (object.Equals(b, null))
		{
			return a < 0L;
		}
		return a < b.GetValue();
	}

	public static TssSdtLong operator ++(TssSdtLong v)
	{
		TssSdtLong tssSdtLong = new TssSdtLong();
		if (object.Equals(v, null))
		{
			long num = 0L;
			num += 1L;
			tssSdtLong.SetValue(num);
		}
		else
		{
			long num2 = v.GetValue();
			num2 += 1L;
			tssSdtLong.SetValue(num2);
		}
		return tssSdtLong;
	}

	public static TssSdtLong operator --(TssSdtLong v)
	{
		TssSdtLong tssSdtLong = new TssSdtLong();
		if (object.Equals(v, null))
		{
			long num = 0L;
			num -= 1L;
			tssSdtLong.SetValue(num);
		}
		else
		{
			long num2 = v.GetValue();
			num2 -= 1L;
			tssSdtLong.SetValue(num2);
		}
		return tssSdtLong;
	}

	public static implicit operator long(TssSdtLong v)
	{
		if (v == null)
		{
			return 0L;
		}
		return v.GetValue();
	}

	public static implicit operator TssSdtLong(long v)
	{
		TssSdtLong tssSdtLong = new TssSdtLong();
		tssSdtLong.SetValue(v);
		return tssSdtLong;
	}
}
