using System;

public class TssSdtByte
{
	private byte[] m_value;

	private byte[] m_xor_key;

	private int m_index;

	public TssSdtByte()
	{
		this.m_value = new byte[3];
		this.m_xor_key = new byte[3];
		this.m_index = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_value[i] = 0;
			this.m_xor_key[i] = 0;
		}
	}

	private byte GetValue()
	{
		int index = this.m_index;
		byte b = this.m_value[index];
		return b ^ this.m_xor_key[index];
	}

	private void SetValue(byte v)
	{
		byte byteXORKey = TssSdtDataTypeFactory.GetByteXORKey();
		int num = this.m_index + 1;
		if (num >= 3)
		{
			num = 0;
		}
		byte b = v ^ byteXORKey;
		this.m_value[num] = b;
		this.m_xor_key[num] = byteXORKey;
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

	public static bool operator ==(TssSdtByte a, TssSdtByte b)
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

	public static bool operator ==(TssSdtByte a, byte b)
	{
		if (object.Equals(a, null))
		{
			return 0 == b;
		}
		return a.GetValue() == b;
	}

	public static bool operator ==(byte a, TssSdtByte b)
	{
		if (object.Equals(b, null))
		{
			return a == 0;
		}
		return a == b.GetValue();
	}

	public static bool operator !=(TssSdtByte a, TssSdtByte b)
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

	public static bool operator !=(TssSdtByte a, byte b)
	{
		if (object.Equals(a, null))
		{
			return 0 != b;
		}
		return a.GetValue() != b;
	}

	public static bool operator !=(byte a, TssSdtByte b)
	{
		if (object.Equals(b, null))
		{
			return a != 0;
		}
		return a != b.GetValue();
	}

	public static bool operator <=(TssSdtByte a, TssSdtByte b)
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

	public static bool operator <=(TssSdtByte a, byte b)
	{
		if (object.Equals(a, null))
		{
			return 0 <= b;
		}
		return a.GetValue() <= b;
	}

	public static bool operator <=(byte a, TssSdtByte b)
	{
		if (object.Equals(b, null))
		{
			return a <= 0;
		}
		return a <= b.GetValue();
	}

	public static bool operator >=(TssSdtByte a, TssSdtByte b)
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

	public static bool operator >=(TssSdtByte a, byte b)
	{
		if (object.Equals(a, null))
		{
			return 0 >= b;
		}
		return a.GetValue() >= b;
	}

	public static bool operator >=(byte a, TssSdtByte b)
	{
		if (object.Equals(b, null))
		{
			return a >= 0;
		}
		return a >= b.GetValue();
	}

	public static bool operator >(TssSdtByte a, TssSdtByte b)
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

	public static bool operator >(TssSdtByte a, byte b)
	{
		if (object.Equals(a, null))
		{
			return 0 > b;
		}
		return a.GetValue() > b;
	}

	public static bool operator >(byte a, TssSdtByte b)
	{
		if (object.Equals(b, null))
		{
			return a > 0;
		}
		return a > b.GetValue();
	}

	public static bool operator <(TssSdtByte a, TssSdtByte b)
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

	public static bool operator <(TssSdtByte a, byte b)
	{
		if (object.Equals(a, null))
		{
			return 0 < b;
		}
		return a.GetValue() < b;
	}

	public static bool operator <(byte a, TssSdtByte b)
	{
		if (object.Equals(b, null))
		{
			return a < 0;
		}
		return a < b.GetValue();
	}

	public static TssSdtByte operator ++(TssSdtByte v)
	{
		TssSdtByte tssSdtByte = new TssSdtByte();
		if (object.Equals(v, null))
		{
			byte b = 0;
			b += 1;
			tssSdtByte.SetValue(b);
		}
		else
		{
			byte b2 = v.GetValue();
			b2 += 1;
			tssSdtByte.SetValue(b2);
		}
		return tssSdtByte;
	}

	public static TssSdtByte operator --(TssSdtByte v)
	{
		TssSdtByte tssSdtByte = new TssSdtByte();
		if (object.Equals(v, null))
		{
			byte b = 0;
			b -= 1;
			tssSdtByte.SetValue(b);
		}
		else
		{
			byte b2 = v.GetValue();
			b2 -= 1;
			tssSdtByte.SetValue(b2);
		}
		return tssSdtByte;
	}

	public static implicit operator byte(TssSdtByte v)
	{
		if (v == null)
		{
			return 0;
		}
		return v.GetValue();
	}

	public static implicit operator TssSdtByte(byte v)
	{
		TssSdtByte tssSdtByte = new TssSdtByte();
		tssSdtByte.SetValue(v);
		return tssSdtByte;
	}
}
