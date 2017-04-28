using System;

public class TssSdtFloat
{
	private float[] m_value;

	private float[] m_xor_key;

	private int m_index;

	public TssSdtFloat()
	{
		this.m_value = new float[3];
		this.m_xor_key = new float[3];
		this.m_index = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_value[i] = 0f;
			this.m_xor_key[i] = 0f;
		}
	}

	private float GetValue()
	{
		int index = this.m_index;
		float num = this.m_value[index];
		return num - this.m_xor_key[index];
	}

	private void SetValue(float v)
	{
		int num = this.m_index + 1;
		if (num >= 3)
		{
			num = 0;
		}
		float num2 = TssSdtDataTypeFactory.GetFloatXORKey();
		if (v > 3.40282347E+38f)
		{
			num2 = 0f;
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

	public static bool operator ==(TssSdtFloat a, TssSdtFloat b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() == 0f;
		}
		return a.GetValue() == b.GetValue();
	}

	public static bool operator ==(TssSdtFloat a, float b)
	{
		if (object.Equals(a, null))
		{
			return 0f == b;
		}
		return a.GetValue() == b;
	}

	public static bool operator ==(float a, TssSdtFloat b)
	{
		if (object.Equals(b, null))
		{
			return a == 0f;
		}
		return a == b.GetValue();
	}

	public static bool operator !=(TssSdtFloat a, TssSdtFloat b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() != 0f;
		}
		return a.GetValue() != b.GetValue();
	}

	public static bool operator !=(TssSdtFloat a, float b)
	{
		if (object.Equals(a, null))
		{
			return 0f != b;
		}
		return a.GetValue() != b;
	}

	public static bool operator !=(float a, TssSdtFloat b)
	{
		if (object.Equals(b, null))
		{
			return a != 0f;
		}
		return a != b.GetValue();
	}

	public static bool operator <=(TssSdtFloat a, TssSdtFloat b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() <= 0f;
		}
		return a.GetValue() <= b.GetValue();
	}

	public static bool operator <=(TssSdtFloat a, float b)
	{
		if (object.Equals(a, null))
		{
			return 0f <= b;
		}
		return a.GetValue() <= b;
	}

	public static bool operator <=(float a, TssSdtFloat b)
	{
		if (object.Equals(b, null))
		{
			return a <= 0f;
		}
		return a <= b.GetValue();
	}

	public static bool operator >=(TssSdtFloat a, TssSdtFloat b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return true;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() >= 0f;
		}
		return a.GetValue() >= b.GetValue();
	}

	public static bool operator >=(TssSdtFloat a, float b)
	{
		if (object.Equals(a, null))
		{
			return 0f >= b;
		}
		return a.GetValue() >= b;
	}

	public static bool operator >=(float a, TssSdtFloat b)
	{
		if (object.Equals(b, null))
		{
			return a >= 0f;
		}
		return a >= b.GetValue();
	}

	public static bool operator >(TssSdtFloat a, TssSdtFloat b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() > 0f;
		}
		return a.GetValue() > b.GetValue();
	}

	public static bool operator >(TssSdtFloat a, float b)
	{
		if (object.Equals(a, null))
		{
			return 0f > b;
		}
		return a.GetValue() > b;
	}

	public static bool operator >(float a, TssSdtFloat b)
	{
		if (object.Equals(b, null))
		{
			return a > 0f;
		}
		return a > b.GetValue();
	}

	public static bool operator <(TssSdtFloat a, TssSdtFloat b)
	{
		if (object.Equals(a, null) && object.Equals(b, null))
		{
			return false;
		}
		if (object.Equals(b, null))
		{
			return a.GetValue() < 0f;
		}
		return a.GetValue() < b.GetValue();
	}

	public static bool operator <(TssSdtFloat a, float b)
	{
		if (object.Equals(a, null))
		{
			return 0f < b;
		}
		return a.GetValue() < b;
	}

	public static bool operator <(float a, TssSdtFloat b)
	{
		if (object.Equals(b, null))
		{
			return a < 0f;
		}
		return a < b.GetValue();
	}

	public static TssSdtFloat operator ++(TssSdtFloat v)
	{
		TssSdtFloat tssSdtFloat = new TssSdtFloat();
		if (object.Equals(v, null))
		{
			float num = 0f;
			num += 1f;
			tssSdtFloat.SetValue(num);
		}
		else
		{
			float num2 = v.GetValue();
			num2 += 1f;
			tssSdtFloat.SetValue(num2);
		}
		return tssSdtFloat;
	}

	public static TssSdtFloat operator --(TssSdtFloat v)
	{
		TssSdtFloat tssSdtFloat = new TssSdtFloat();
		if (object.Equals(v, null))
		{
			float num = 0f;
			num -= 1f;
			tssSdtFloat.SetValue(num);
		}
		else
		{
			float num2 = v.GetValue();
			num2 -= 1f;
			tssSdtFloat.SetValue(num2);
		}
		return tssSdtFloat;
	}

	public static implicit operator float(TssSdtFloat v)
	{
		if (v == null)
		{
			return 0f;
		}
		return v.GetValue();
	}

	public static implicit operator TssSdtFloat(float v)
	{
		TssSdtFloat tssSdtFloat = new TssSdtFloat();
		tssSdtFloat.SetValue(v);
		return tssSdtFloat;
	}
}
