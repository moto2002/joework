import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.UnsupportedEncodingException;
import java.nio.ByteBuffer;

public class ConvertTools
{
	/*
	 * 对象到字节数组的转换
	 */
	public static byte[] Object2Bytes(Object obj)
	{
		byte[] bytes = null;
		ByteArrayOutputStream bos = new ByteArrayOutputStream();
		try
		{
			ObjectOutputStream oos = new ObjectOutputStream(bos);
			oos.writeObject(obj);
			oos.flush();
			bytes = bos.toByteArray();
			oos.close();
			bos.close();
		} catch (IOException ex)
		{
			ex.printStackTrace();
		}
		return bytes;
	}

	public static int SizeOf(Object obj)
	{
		return Object2Bytes(obj).length;
	}

	public static Object Bytes2Object(byte[] bytes)
	{
		Object obj = null;
		try
		{
			ByteArrayInputStream bis = new ByteArrayInputStream(bytes);
			ObjectInputStream ois = new ObjectInputStream(bis);
			obj = ois.readObject();
			ois.close();
			bis.close();
		} catch (IOException ex)
		{
			ex.printStackTrace();
		} catch (ClassNotFoundException ex)
		{
			ex.printStackTrace();
		}
		return obj;

	}

	public static Object ByteBuffer2Object(ByteBuffer byteBuffer)
			throws ClassNotFoundException, IOException
	{
		InputStream input = new ByteArrayInputStream(byteBuffer.array());
		ObjectInputStream oi = new ObjectInputStream(input);
		Object obj = oi.readObject();
		input.close();
		oi.close();
		byteBuffer.clear();
		return obj;
	}

	public static ByteBuffer Object2ByteBuffer(Object obj) throws IOException
	{
		byte[] bytes = ConvertTools.Object2Bytes(obj);
		ByteBuffer buff = ByteBuffer.wrap(bytes);
		return buff;
	}

	public static byte[] Short2Bytes(short number)
	{
		int temp = number;
		byte[] b = new byte[2];
		for (int i = 0; i < b.length; i++)
		{
			b[i] = new Integer(temp & 0xff).byteValue();// 将最低位保存在最低位
			temp = temp >> 8;// 向右移8位
		}
		return b;
	}

	public static short Bytes2Short(byte[] b)
	{
		short s = 0;
		short s0 = (short) (b[0] & 0xff);// 最低位
		short s1 = (short) (b[1] & 0xff);
		s1 <<= 8;
		s = (short) (s0 | s1);
		return s;
	}

	public static byte[] Int2Bytes(int number)
	{
		int temp = number;
		byte[] b = new byte[4];
		for (int i = 0; i < b.length; i++)
		{
			b[i] = new Integer(temp & 0xff).byteValue();// 将最低位保存在最低位
			temp = temp >> 8;// 向右移8位
		}
		return b;
	}

	public static int Bytes2Int(byte[] b)
	{
		int s = 0;
		int s0 = b[0] & 0xff;// 最低位
		int s1 = b[1] & 0xff;
		int s2 = b[2] & 0xff;
		int s3 = b[3] & 0xff;
		s3 <<= 24;
		s2 <<= 16;
		s1 <<= 8;
		s = s0 | s1 | s2 | s3;
		return s;
	}

	public static byte[] Long2Bytes(long number)
	{
		long temp = number;
		byte[] b = new byte[8];
		for (int i = 0; i < b.length; i++)
		{
			b[i] = new Long(temp & 0xff).byteValue();// 将最低位保存在最低位 temp = temp
														// >> 8;// 向右移8位
		}
		return b;
	}

	public static long Bytes2Long(byte[] b)
	{
		long s = 0;
		long s0 = b[0] & 0xff;// 最低位
		long s1 = b[1] & 0xff;
		long s2 = b[2] & 0xff;
		long s3 = b[3] & 0xff;
		long s4 = b[4] & 0xff;// 最低位
		long s5 = b[5] & 0xff;
		long s6 = b[6] & 0xff;
		long s7 = b[7] & 0xff;

		// s0不变
		s1 <<= 8;
		s2 <<= 16;
		s3 <<= 24;
		s4 <<= 8 * 4;
		s5 <<= 8 * 5;
		s6 <<= 8 * 6;
		s7 <<= 8 * 7;
		s = s0 | s1 | s2 | s3 | s4 | s5 | s6 | s7;
		return s;
	}

	public static byte[] Double2Bytes(double num)
	{
		byte[] b = new byte[8];
		long l = Double.doubleToLongBits(num);
		for (int i = 0; i < 8; i++)
		{
			b[i] = new Long(l).byteValue();
			l = l >> 8;
		}
		return b;
	}

	public static double Bytes2Double(byte[] b)
	{
		long m;
		m = b[0];
		m &= 0xff;
		m |= ((long) b[1] << 8);
		m &= 0xffff;
		m |= ((long) b[2] << 16);
		m &= 0xffffff;
		m |= ((long) b[3] << 24);
		m &= 0xffffffffl;
		m |= ((long) b[4] << 32);
		m &= 0xffffffffffl;
		m |= ((long) b[5] << 40);
		m &= 0xffffffffffffl;
		m |= ((long) b[6] << 48);
		m &= 0xffffffffffffffl;
		m |= ((long) b[7] << 56);
		return Double.longBitsToDouble(m);
	}

	public static byte[] Float2Bytes(float f)
	{
		int temp = Float.floatToIntBits(f);
		byte[] b = Int2Bytes(temp);
		return b;
	}

	public static float Bytes2Float(byte[] b)
	{
		// 4 bytes
		int accum = 0;
		for (int shiftBy = 0; shiftBy < 4; shiftBy++)
		{
			accum |= (b[shiftBy] & 0xff) << shiftBy * 8;
		}
		return Float.intBitsToFloat(accum);
	}

	public static byte[] Char2Bytes(char c)
	{
		byte[] b = new byte[2];
		b[0] = (byte) ((c & 0xFF00) >> 8);
		b[1] = (byte) (c & 0xFF);
		return b;
	}

	public static char Bytes2Char(byte[] b)
	{
		char c = (char) (((b[0] & 0xFF) << 8) | (b[1] & 0xFF));
		return c;
	}

	public static byte[] String2Bytes(String str)			
	{
		byte[] b = null;
		try
		{
			b = str.getBytes("GBK");
			
		} catch (UnsupportedEncodingException e)
		{
			e.printStackTrace();
		}
		return b;
	}

	public static String Bytes2String(byte[] str)
	{
		String keyword = null;
		try
		{
			keyword = new String(str, "GBK");
		} catch (UnsupportedEncodingException e)
		{
			e.printStackTrace();
		}
		return keyword;
	}

}
