using System;

public class TssSdtShort
{
	private short[] m_value;

	private short[] m_xor_key;

	private int m_index;

	public TssSdtShort()
	{
		this.m_value = new short[3];
		this.m_xor_key = new short[3];
		this.m_index = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_value[i] = 0;
			this.m_xor_key[i] = 0;
		}
	}

	private short GetValue()
	{
		int index = this.m_index;
		short num = this.m_value[index];
		return num ^ this.m_xor_key[index];
	}

	private void SetValue(short v)
	{
		short shortXORKey = TssSdtDataTypeFactory.GetShortXORKey();
		int num = this.m_index + 1;
		if (num >= 3)
		{
			num = 0;
		}
		short num2 = v ^ shortXORKey;
		this.m_value[num] = num2;
		this.m_xor_key[num] = shortXORKey;
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

	public static bool operator ==(TssSdtShort a, TssSdtShort b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() == 0;
		}
		return a.GetValue() == b.GetValue();
	}

	public static bool operator ==(TssSdtShort a, short b)
	{
		if (object.Equals(a, null))
		{
			return 0 == b;
		}
		return a.GetValue() == b;
	}

	public static bool operator ==(short a, TssSdtShort b)
	{
		if (object.Equals(b, null))
		{
			return a == 0;
		}
		return a == b.GetValue();
	}

	public static bool operator !=(TssSdtShort a, TssSdtShort b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() != 0;
		}
		return a.GetValue() != b.GetValue();
	}

	public static bool operator !=(TssSdtShort a, short b)
	{
		if (object.Equals(a, null))
		{
			return 0 != b;
		}
		return a.GetValue() != b;
	}

	public static bool operator !=(short a, TssSdtShort b)
	{
		if (object.Equals(b, null))
		{
			return a != 0;
		}
		return a != b.GetValue();
	}

	public static bool operator <=(TssSdtShort a, TssSdtShort b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() <= 0;
		}
		return a.GetValue() <= b.GetValue();
	}

	public static bool operator <=(TssSdtShort a, short b)
	{
		if (object.Equals(a, null))
		{
			return 0 <= b;
		}
		return a.GetValue() <= b;
	}

	public static bool operator <=(short a, TssSdtShort b)
	{
		if (object.Equals(b, null))
		{
			return a <= 0;
		}
		return a <= b.GetValue();
	}

	public static bool operator >=(TssSdtShort a, TssSdtShort b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() >= 0;
		}
		return a.GetValue() >= b.GetValue();
	}

	public static bool operator >=(TssSdtShort a, short b)
	{
		if (object.Equals(a, null))
		{
			return 0 >= b;
		}
		return a.GetValue() >= b;
	}

	public static bool operator >=(short a, TssSdtShort b)
	{
		if (object.Equals(b, null))
		{
			return a >= 0;
		}
		return a >= b.GetValue();
	}

	public static bool operator >(TssSdtShort a, TssSdtShort b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() > 0;
		}
		return a.GetValue() > b.GetValue();
	}

	public static bool operator >(TssSdtShort a, short b)
	{
		if (object.Equals(a, null))
		{
			return 0 > b;
		}
		return a.GetValue() > b;
	}

	public static bool operator >(short a, TssSdtShort b)
	{
		if (object.Equals(b, null))
		{
			return a > 0;
		}
		return a > b.GetValue();
	}

	public static bool operator <(TssSdtShort a, TssSdtShort b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() < 0;
		}
		return a.GetValue() < b.GetValue();
	}

	public static bool operator <(TssSdtShort a, short b)
	{
		if (object.Equals(a, null))
		{
			return 0 < b;
		}
		return a.GetValue() < b;
	}

	public static bool operator <(short a, TssSdtShort b)
	{
		if (object.Equals(b, null))
		{
			return a < 0;
		}
		return a < b.GetValue();
	}

	public static TssSdtShort operator ++(TssSdtShort v)
	{
		TssSdtShort tssSdtShort = new TssSdtShort();
		if (object.Equals(v, null))
		{
			short num = 0;
			num += 1;
			tssSdtShort.SetValue(num);
		}
		else
		{
			short num2 = v.GetValue();
			num2 += 1;
			tssSdtShort.SetValue(num2);
		}
		return tssSdtShort;
	}

	public static TssSdtShort operator --(TssSdtShort v)
	{
		TssSdtShort tssSdtShort = new TssSdtShort();
		if (object.Equals(v, null))
		{
			short num = 0;
			num -= 1;
			tssSdtShort.SetValue(num);
		}
		else
		{
			short num2 = v.GetValue();
			num2 -= 1;
			tssSdtShort.SetValue(num2);
		}
		return tssSdtShort;
	}

	public static implicit operator short(TssSdtShort v)
	{
		if (v == null)
		{
			return 0;
		}
		return v.GetValue();
	}

	public static implicit operator TssSdtShort(short v)
	{
		TssSdtShort tssSdtShort = new TssSdtShort();
		tssSdtShort.SetValue(v);
		return tssSdtShort;
	}
}
