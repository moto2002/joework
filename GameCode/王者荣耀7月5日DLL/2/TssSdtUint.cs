using System;

public class TssSdtUint
{
	private uint[] m_value;

	private uint[] m_xor_key;

	private int m_index;

	public TssSdtUint()
	{
		this.m_value = new uint[3];
		this.m_xor_key = new uint[3];
		this.m_index = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_value[i] = 0u;
			this.m_xor_key[i] = 0u;
		}
	}

	private uint GetValue()
	{
		int index = this.m_index;
		uint num = this.m_value[index];
		return num ^ this.m_xor_key[index];
	}

	private void SetValue(uint v)
	{
		uint uintXORKey = TssSdtDataTypeFactory.GetUintXORKey();
		int num = this.m_index + 1;
		if (num >= 3)
		{
			num = 0;
		}
		uint num2 = v ^ uintXORKey;
		this.m_value[num] = num2;
		this.m_xor_key[num] = uintXORKey;
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

	public static bool operator ==(TssSdtUint a, TssSdtUint b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() == 0u;
		}
		return a.GetValue() == b.GetValue();
	}

	public static bool operator ==(TssSdtUint a, uint b)
	{
		if (object.Equals(a, null))
		{
			return 0u == b;
		}
		return a.GetValue() == b;
	}

	public static bool operator ==(uint a, TssSdtUint b)
	{
		if (object.Equals(b, null))
		{
			return a == 0u;
		}
		return a == b.GetValue();
	}

	public static bool operator !=(TssSdtUint a, TssSdtUint b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() != 0u;
		}
		return a.GetValue() != b.GetValue();
	}

	public static bool operator !=(TssSdtUint a, uint b)
	{
		if (object.Equals(a, null))
		{
			return 0u != b;
		}
		return a.GetValue() != b;
	}

	public static bool operator !=(uint a, TssSdtUint b)
	{
		if (object.Equals(b, null))
		{
			return a != 0u;
		}
		return a != b.GetValue();
	}

	public static bool operator <=(TssSdtUint a, TssSdtUint b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() <= 0u;
		}
		return a.GetValue() <= b.GetValue();
	}

	public static bool operator <=(TssSdtUint a, uint b)
	{
		if (object.Equals(a, null))
		{
			return 0u <= b;
		}
		return a.GetValue() <= b;
	}

	public static bool operator <=(uint a, TssSdtUint b)
	{
		if (object.Equals(b, null))
		{
			return a <= 0u;
		}
		return a <= b.GetValue();
	}

	public static bool operator >=(TssSdtUint a, TssSdtUint b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() >= 0u;
		}
		return a.GetValue() >= b.GetValue();
	}

	public static bool operator >=(TssSdtUint a, uint b)
	{
		if (object.Equals(a, null))
		{
			return 0u >= b;
		}
		return a.GetValue() >= b;
	}

	public static bool operator >=(uint a, TssSdtUint b)
	{
		if (object.Equals(b, null))
		{
			return a >= 0u;
		}
		return a >= b.GetValue();
	}

	public static bool operator >(TssSdtUint a, TssSdtUint b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() > 0u;
		}
		return a.GetValue() > b.GetValue();
	}

	public static bool operator >(TssSdtUint a, uint b)
	{
		if (object.Equals(a, null))
		{
			return 0u > b;
		}
		return a.GetValue() > b;
	}

	public static bool operator >(uint a, TssSdtUint b)
	{
		if (object.Equals(b, null))
		{
			return a > 0u;
		}
		return a > b.GetValue();
	}

	public static bool operator <(TssSdtUint a, TssSdtUint b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() < 0u;
		}
		return a.GetValue() < b.GetValue();
	}

	public static bool operator <(TssSdtUint a, uint b)
	{
		if (object.Equals(a, null))
		{
			return 0u < b;
		}
		return a.GetValue() < b;
	}

	public static bool operator <(uint a, TssSdtUint b)
	{
		if (object.Equals(b, null))
		{
			return a < 0u;
		}
		return a < b.GetValue();
	}

	public static TssSdtUint operator ++(TssSdtUint v)
	{
		TssSdtUint tssSdtUint = new TssSdtUint();
		if (object.Equals(v, null))
		{
			uint num = 0u;
			num += 1u;
			tssSdtUint.SetValue(num);
		}
		else
		{
			uint num2 = v.GetValue();
			num2 += 1u;
			tssSdtUint.SetValue(num2);
		}
		return tssSdtUint;
	}

	public static TssSdtUint operator --(TssSdtUint v)
	{
		TssSdtUint tssSdtUint = new TssSdtUint();
		if (object.Equals(v, null))
		{
			uint num = 0u;
			num -= 1u;
			tssSdtUint.SetValue(num);
		}
		else
		{
			uint num2 = v.GetValue();
			num2 -= 1u;
			tssSdtUint.SetValue(num2);
		}
		return tssSdtUint;
	}

	public static implicit operator uint(TssSdtUint v)
	{
		if (v == null)
		{
			return 0u;
		}
		return v.GetValue();
	}

	public static implicit operator TssSdtUint(uint v)
	{
		TssSdtUint tssSdtUint = new TssSdtUint();
		tssSdtUint.SetValue(v);
		return tssSdtUint;
	}
}
