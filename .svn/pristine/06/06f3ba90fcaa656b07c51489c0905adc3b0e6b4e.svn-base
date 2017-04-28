using System;

public class TssSdtUlong
{
	private ulong[] m_value;

	private ulong[] m_xor_key;

	private int m_index;

	public TssSdtUlong()
	{
		this.m_value = new ulong[3];
		this.m_xor_key = new ulong[3];
		this.m_index = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_value[i] = 0uL;
			this.m_xor_key[i] = 0uL;
		}
	}

	private ulong GetValue()
	{
		int index = this.m_index;
		ulong num = this.m_value[index];
		return num ^ this.m_xor_key[index];
	}

	private void SetValue(ulong v)
	{
		ulong ulongXORKey = TssSdtDataTypeFactory.GetUlongXORKey();
		int num = this.m_index + 1;
		if (num >= 3)
		{
			num = 0;
		}
		ulong num2 = v ^ ulongXORKey;
		this.m_value[num] = num2;
		this.m_xor_key[num] = ulongXORKey;
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

	public static bool operator ==(TssSdtUlong a, TssSdtUlong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() == 0uL;
		}
		return a.GetValue() == b.GetValue();
	}

	public static bool operator ==(TssSdtUlong a, ulong b)
	{
		if (object.Equals(a, null))
		{
			return 0uL == b;
		}
		return a.GetValue() == b;
	}

	public static bool operator ==(ulong a, TssSdtUlong b)
	{
		if (object.Equals(b, null))
		{
			return a == 0uL;
		}
		return a == b.GetValue();
	}

	public static bool operator !=(TssSdtUlong a, TssSdtUlong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() != 0uL;
		}
		return a.GetValue() != b.GetValue();
	}

	public static bool operator !=(TssSdtUlong a, ulong b)
	{
		if (object.Equals(a, null))
		{
			return 0uL != b;
		}
		return a.GetValue() != b;
	}

	public static bool operator !=(ulong a, TssSdtUlong b)
	{
		if (object.Equals(b, null))
		{
			return a != 0uL;
		}
		return a != b.GetValue();
	}

	public static bool operator <=(TssSdtUlong a, TssSdtUlong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() <= 0uL;
		}
		return a.GetValue() <= b.GetValue();
	}

	public static bool operator <=(TssSdtUlong a, ulong b)
	{
		if (object.Equals(a, null))
		{
			return 0uL <= b;
		}
		return a.GetValue() <= b;
	}

	public static bool operator <=(ulong a, TssSdtUlong b)
	{
		if (object.Equals(b, null))
		{
			return a <= 0uL;
		}
		return a <= b.GetValue();
	}

	public static bool operator >=(TssSdtUlong a, TssSdtUlong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() >= 0uL;
		}
		return a.GetValue() >= b.GetValue();
	}

	public static bool operator >=(TssSdtUlong a, ulong b)
	{
		if (object.Equals(a, null))
		{
			return 0uL >= b;
		}
		return a.GetValue() >= b;
	}

	public static bool operator >=(ulong a, TssSdtUlong b)
	{
		if (object.Equals(b, null))
		{
			return a >= 0uL;
		}
		return a >= b.GetValue();
	}

	public static bool operator >(TssSdtUlong a, TssSdtUlong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() > 0uL;
		}
		return a.GetValue() > b.GetValue();
	}

	public static bool operator >(TssSdtUlong a, ulong b)
	{
		if (object.Equals(a, null))
		{
			return 0uL > b;
		}
		return a.GetValue() > b;
	}

	public static bool operator >(ulong a, TssSdtUlong b)
	{
		if (object.Equals(b, null))
		{
			return a > 0uL;
		}
		return a > b.GetValue();
	}

	public static bool operator <(TssSdtUlong a, TssSdtUlong b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() < 0uL;
		}
		return a.GetValue() < b.GetValue();
	}

	public static bool operator <(TssSdtUlong a, ulong b)
	{
		if (object.Equals(a, null))
		{
			return 0uL < b;
		}
		return a.GetValue() < b;
	}

	public static bool operator <(ulong a, TssSdtUlong b)
	{
		if (object.Equals(b, null))
		{
			return a < 0uL;
		}
		return a < b.GetValue();
	}

	public static TssSdtUlong operator ++(TssSdtUlong v)
	{
		TssSdtUlong tssSdtUlong = new TssSdtUlong();
		if (object.Equals(v, null))
		{
			ulong num = 0uL;
			num += 1uL;
			tssSdtUlong.SetValue(num);
		}
		else
		{
			ulong num2 = v.GetValue();
			num2 += 1uL;
			tssSdtUlong.SetValue(num2);
		}
		return tssSdtUlong;
	}

	public static TssSdtUlong operator --(TssSdtUlong v)
	{
		TssSdtUlong tssSdtUlong = new TssSdtUlong();
		if (object.Equals(v, null))
		{
			ulong num = 0uL;
			num -= 1uL;
			tssSdtUlong.SetValue(num);
		}
		else
		{
			ulong num2 = v.GetValue();
			num2 -= 1uL;
			tssSdtUlong.SetValue(num2);
		}
		return tssSdtUlong;
	}

	public static implicit operator ulong(TssSdtUlong v)
	{
		if (v == null)
		{
			return 0uL;
		}
		return v.GetValue();
	}

	public static implicit operator TssSdtUlong(ulong v)
	{
		TssSdtUlong tssSdtUlong = new TssSdtUlong();
		tssSdtUlong.SetValue(v);
		return tssSdtUlong;
	}
}
