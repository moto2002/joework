using System;

public class TssSdtDouble
{
	private double[] m_value;

	private double[] m_xor_key;

	private int m_index;

	public TssSdtDouble()
	{
		this.m_value = new double[3];
		this.m_xor_key = new double[3];
		this.m_index = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_value[i] = 0.0;
			this.m_xor_key[i] = 0.0;
		}
	}

	private double GetValue()
	{
		int index = this.m_index;
		double num = this.m_value[index];
		return num - this.m_xor_key[index];
	}

	private void SetValue(double v)
	{
		int num = this.m_index + 1;
		if (num >= 3)
		{
			num = 0;
		}
		double num2 = TssSdtDataTypeFactory.GetDoubleXORKey();
		if (v > 1.7976931348623157E+308)
		{
			num2 = 0.0;
		}
		this.m_xor_key[num] = num2;
		this.m_value[num] = v + this.m_xor_key[num];
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

	public static bool operator ==(TssSdtDouble a, TssSdtDouble b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() == 0.0;
		}
		return a.GetValue() == b.GetValue();
	}

	public static bool operator ==(TssSdtDouble a, double b)
	{
		if (object.Equals(a, null))
		{
			return 0.0 == b;
		}
		return a.GetValue() == b;
	}

	public static bool operator ==(double a, TssSdtDouble b)
	{
		if (object.Equals(b, null))
		{
			return a == 0.0;
		}
		return a == b.GetValue();
	}

	public static bool operator !=(TssSdtDouble a, TssSdtDouble b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() != 0.0;
		}
		return a.GetValue() != b.GetValue();
	}

	public static bool operator !=(TssSdtDouble a, double b)
	{
		if (object.Equals(a, null))
		{
			return 0.0 != b;
		}
		return a.GetValue() != b;
	}

	public static bool operator !=(double a, TssSdtDouble b)
	{
		if (object.Equals(b, null))
		{
			return a != 0.0;
		}
		return a != b.GetValue();
	}

	public static bool operator <=(TssSdtDouble a, TssSdtDouble b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() <= 0.0;
		}
		return a.GetValue() <= b.GetValue();
	}

	public static bool operator <=(TssSdtDouble a, double b)
	{
		if (object.Equals(a, null))
		{
			return 0.0 <= b;
		}
		return a.GetValue() <= b;
	}

	public static bool operator <=(double a, TssSdtDouble b)
	{
		if (object.Equals(b, null))
		{
			return a <= 0.0;
		}
		return a <= b.GetValue();
	}

	public static bool operator >=(TssSdtDouble a, TssSdtDouble b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() >= 0.0;
		}
		return a.GetValue() >= b.GetValue();
	}

	public static bool operator >=(TssSdtDouble a, double b)
	{
		if (object.Equals(a, null))
		{
			return 0.0 >= b;
		}
		return a.GetValue() >= b;
	}

	public static bool operator >=(double a, TssSdtDouble b)
	{
		if (object.Equals(b, null))
		{
			return a >= 0.0;
		}
		return a >= b.GetValue();
	}

	public static bool operator >(TssSdtDouble a, TssSdtDouble b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() > 0.0;
		}
		return a.GetValue() > b.GetValue();
	}

	public static bool operator >(TssSdtDouble a, double b)
	{
		if (object.Equals(a, null))
		{
			return 0.0 > b;
		}
		return a.GetValue() > b;
	}

	public static bool operator >(double a, TssSdtDouble b)
	{
		if (object.Equals(b, null))
		{
			return a > 0.0;
		}
		return a > b.GetValue();
	}

	public static bool operator <(TssSdtDouble a, TssSdtDouble b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() < 0.0;
		}
		return a.GetValue() < b.GetValue();
	}

	public static bool operator <(TssSdtDouble a, double b)
	{
		if (object.Equals(a, null))
		{
			return 0.0 < b;
		}
		return a.GetValue() < b;
	}

	public static bool operator <(double a, TssSdtDouble b)
	{
		if (object.Equals(b, null))
		{
			return a < 0.0;
		}
		return a < b.GetValue();
	}

	public static TssSdtDouble operator ++(TssSdtDouble v)
	{
		TssSdtDouble tssSdtDouble = new TssSdtDouble();
		if (object.Equals(v, null))
		{
			double num = 0.0;
			num += 1.0;
			tssSdtDouble.SetValue(num);
		}
		else
		{
			double num2 = v.GetValue();
			num2 += 1.0;
			tssSdtDouble.SetValue(num2);
		}
		return tssSdtDouble;
	}

	public static TssSdtDouble operator --(TssSdtDouble v)
	{
		TssSdtDouble tssSdtDouble = new TssSdtDouble();
		if (object.Equals(v, null))
		{
			double num = 0.0;
			num -= 1.0;
			tssSdtDouble.SetValue(num);
		}
		else
		{
			double num2 = v.GetValue();
			num2 -= 1.0;
			tssSdtDouble.SetValue(num2);
		}
		return tssSdtDouble;
	}

	public static implicit operator double(TssSdtDouble v)
	{
		if (v == null)
		{
			return 0.0;
		}
		return v.GetValue();
	}

	public static implicit operator TssSdtDouble(double v)
	{
		TssSdtDouble tssSdtDouble = new TssSdtDouble();
		tssSdtDouble.SetValue(v);
		return tssSdtDouble;
	}
}
