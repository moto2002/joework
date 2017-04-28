using System;

public class TssSdtInt
{
	private int[] m_value;

	private int[] m_xor_key;

	private int m_index;

	public TssSdtInt()
	{
		this.m_value = new int[3];
		this.m_xor_key = new int[3];
		this.m_index = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_value[i] = 0;
			this.m_xor_key[i] = 0;
		}
	}

	private int GetValue()
	{
		int index = this.m_index;
		int num = this.m_value[index];
		return num ^ this.m_xor_key[index];
	}

	private void SetValue(int v)
	{
		int intXORKey = TssSdtDataTypeFactory.GetIntXORKey();
		int num = this.m_index + 1;
		if (num >= 3)
		{
			num = 0;
		}
		int num2 = v ^ intXORKey;
		this.m_value[num] = num2;
		this.m_xor_key[num] = intXORKey;
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

	public static bool operator ==(TssSdtInt a, TssSdtInt b)
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

	public static bool operator ==(TssSdtInt a, int b)
	{
		if (object.Equals(a, null))
		{
			return 0 == b;
		}
		return a.GetValue() == b;
	}

	public static bool operator ==(int a, TssSdtInt b)
	{
		if (object.Equals(b, null))
		{
			return a == 0;
		}
		return a == b.GetValue();
	}

	public static bool operator !=(TssSdtInt a, TssSdtInt b)
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

	public static bool operator !=(TssSdtInt a, int b)
	{
		if (object.Equals(a, null))
		{
			return 0 != b;
		}
		return a.GetValue() != b;
	}

	public static bool operator !=(int a, TssSdtInt b)
	{
		if (object.Equals(b, null))
		{
			return a != 0;
		}
		return a != b.GetValue();
	}

	public static bool operator <=(TssSdtInt a, TssSdtInt b)
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

	public static bool operator <=(TssSdtInt a, int b)
	{
		if (object.Equals(a, null))
		{
			return 0 <= b;
		}
		return a.GetValue() <= b;
	}

	public static bool operator <=(int a, TssSdtInt b)
	{
		if (object.Equals(b, null))
		{
			return a <= 0;
		}
		return a <= b.GetValue();
	}

	public static bool operator >=(TssSdtInt a, TssSdtInt b)
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

	public static bool operator >=(TssSdtInt a, int b)
	{
		if (object.Equals(a, null))
		{
			return 0 >= b;
		}
		return a.GetValue() >= b;
	}

	public static bool operator >=(int a, TssSdtInt b)
	{
		if (object.Equals(b, null))
		{
			return a >= 0;
		}
		return a >= b.GetValue();
	}

	public static bool operator >(TssSdtInt a, TssSdtInt b)
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

	public static bool operator >(TssSdtInt a, int b)
	{
		if (object.Equals(a, null))
		{
			return 0 > b;
		}
		return a.GetValue() > b;
	}

	public static bool operator >(int a, TssSdtInt b)
	{
		if (object.Equals(b, null))
		{
			return a > 0;
		}
		return a > b.GetValue();
	}

	public static bool operator <(TssSdtInt a, TssSdtInt b)
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

	public static bool operator <(TssSdtInt a, int b)
	{
		if (object.Equals(a, null))
		{
			return 0 < b;
		}
		return a.GetValue() < b;
	}

	public static bool operator <(int a, TssSdtInt b)
	{
		if (object.Equals(b, null))
		{
			return a < 0;
		}
		return a < b.GetValue();
	}

	public static TssSdtInt operator ++(TssSdtInt v)
	{
		TssSdtInt tssSdtInt = new TssSdtInt();
		if (object.Equals(v, null))
		{
			int num = 0;
			num++;
			tssSdtInt.SetValue(num);
		}
		else
		{
			int num2 = v.GetValue();
			num2++;
			tssSdtInt.SetValue(num2);
		}
		return tssSdtInt;
	}

	public static TssSdtInt operator --(TssSdtInt v)
	{
		TssSdtInt tssSdtInt = new TssSdtInt();
		if (object.Equals(v, null))
		{
			int num = 0;
			num--;
			tssSdtInt.SetValue(num);
		}
		else
		{
			int num2 = v.GetValue();
			num2--;
			tssSdtInt.SetValue(num2);
		}
		return tssSdtInt;
	}

	public static implicit operator int(TssSdtInt v)
	{
		if (v == null)
		{
			return 0;
		}
		return v.GetValue();
	}

	public static implicit operator TssSdtInt(int v)
	{
		TssSdtInt tssSdtInt = new TssSdtInt();
		tssSdtInt.SetValue(v);
		return tssSdtInt;
	}
}
