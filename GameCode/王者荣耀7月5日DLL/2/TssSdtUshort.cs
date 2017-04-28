using System;

public class TssSdtUshort
{
	private ushort[] m_value;

	private ushort[] m_xor_key;

	private int m_index;

	public TssSdtUshort()
	{
		this.m_value = new ushort[3];
		this.m_xor_key = new ushort[3];
		this.m_index = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_value[i] = 0;
			this.m_xor_key[i] = 0;
		}
	}

	private ushort GetValue()
	{
		int index = this.m_index;
		ushort num = this.m_value[index];
		return num ^ this.m_xor_key[index];
	}

	private void SetValue(ushort v)
	{
		ushort ushortXORKey = TssSdtDataTypeFactory.GetUshortXORKey();
		int num = this.m_index + 1;
		if (num >= 3)
		{
			num = 0;
		}
		ushort num2 = v ^ ushortXORKey;
		this.m_value[num] = num2;
		this.m_xor_key[num] = ushortXORKey;
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

	public static bool operator ==(TssSdtUshort a, TssSdtUshort b)
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

	public static bool operator ==(TssSdtUshort a, ushort b)
	{
		if (object.Equals(a, null))
		{
			return 0 == b;
		}
		return a.GetValue() == b;
	}

	public static bool operator ==(ushort a, TssSdtUshort b)
	{
		if (object.Equals(b, null))
		{
			return a == 0;
		}
		return a == b.GetValue();
	}

	public static bool operator !=(TssSdtUshort a, TssSdtUshort b)
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

	public static bool operator !=(TssSdtUshort a, ushort b)
	{
		if (object.Equals(a, null))
		{
			return 0 != b;
		}
		return a.GetValue() != b;
	}

	public static bool operator !=(ushort a, TssSdtUshort b)
	{
		if (object.Equals(b, null))
		{
			return a != 0;
		}
		return a != b.GetValue();
	}

	public static bool operator <=(TssSdtUshort a, TssSdtUshort b)
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

	public static bool operator <=(TssSdtUshort a, ushort b)
	{
		if (object.Equals(a, null))
		{
			return 0 <= b;
		}
		return a.GetValue() <= b;
	}

	public static bool operator <=(ushort a, TssSdtUshort b)
	{
		if (object.Equals(b, null))
		{
			return a <= 0;
		}
		return a <= b.GetValue();
	}

	public static bool operator >=(TssSdtUshort a, TssSdtUshort b)
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

	public static bool operator >=(TssSdtUshort a, ushort b)
	{
		if (object.Equals(a, null))
		{
			return 0 >= b;
		}
		return a.GetValue() >= b;
	}

	public static bool operator >=(ushort a, TssSdtUshort b)
	{
		if (object.Equals(b, null))
		{
			return a >= 0;
		}
		return a >= b.GetValue();
	}

	public static bool operator >(TssSdtUshort a, TssSdtUshort b)
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

	public static bool operator >(TssSdtUshort a, ushort b)
	{
		if (object.Equals(a, null))
		{
			return 0 > b;
		}
		return a.GetValue() > b;
	}

	public static bool operator >(ushort a, TssSdtUshort b)
	{
		if (object.Equals(b, null))
		{
			return a > 0;
		}
		return a > b.GetValue();
	}

	public static bool operator <(TssSdtUshort a, TssSdtUshort b)
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

	public static bool operator <(TssSdtUshort a, ushort b)
	{
		if (object.Equals(a, null))
		{
			return 0 < b;
		}
		return a.GetValue() < b;
	}

	public static bool operator <(ushort a, TssSdtUshort b)
	{
		if (object.Equals(b, null))
		{
			return a < 0;
		}
		return a < b.GetValue();
	}

	public static TssSdtUshort operator ++(TssSdtUshort v)
	{
		TssSdtUshort tssSdtUshort = new TssSdtUshort();
		if (object.Equals(v, null))
		{
			ushort num = 0;
			num += 1;
			tssSdtUshort.SetValue(num);
		}
		else
		{
			ushort num2 = v.GetValue();
			num2 += 1;
			tssSdtUshort.SetValue(num2);
		}
		return tssSdtUshort;
	}

	public static TssSdtUshort operator --(TssSdtUshort v)
	{
		TssSdtUshort tssSdtUshort = new TssSdtUshort();
		if (object.Equals(v, null))
		{
			ushort num = 0;
			num -= 1;
			tssSdtUshort.SetValue(num);
		}
		else
		{
			ushort num2 = v.GetValue();
			num2 -= 1;
			tssSdtUshort.SetValue(num2);
		}
		return tssSdtUshort;
	}

	public static implicit operator ushort(TssSdtUshort v)
	{
		if (v == null)
		{
			return 0;
		}
		return v.GetValue();
	}

	public static implicit operator TssSdtUshort(ushort v)
	{
		TssSdtUshort tssSdtUshort = new TssSdtUshort();
		tssSdtUshort.SetValue(v);
		return tssSdtUshort;
	}
}
