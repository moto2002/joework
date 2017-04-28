using UnityEngine;
using System.Collections;

public class TypeConvert
{
    public static byte[] getBytes(byte[] b1, byte[] b2)
    {
        byte[] newb = new byte[b1.Length + b2.Length];
        b1.CopyTo(newb, 0);
        b2.CopyTo(newb,b1.Length);
        return newb;
    }

    public static byte[] getBytes(float s, bool asc)
    {
        int buf = (int)(s * 100);
        return getBytes(buf, asc);
    }

    public static byte[] getBytes(short s, bool asc)
    {
        byte[] buf = new byte[2];
        if (asc)
        {
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                buf[i] = (byte)(s & 0x00ff);
                s >>= 8;
            }
        }
        else
        {
            for (int i = 0; i < buf.Length; i++)
            {

                buf[i] = (byte)(s & 0x00ff);
                s >>= 8;
            }
        }
        return buf;
    }

    public static byte[] getBytes(int s, bool asc)
    {
        byte[] buf = new byte[4];
        if (asc)
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                buf[i] = (byte)(s & 0x000000ff);
                s >>= 8;
            }
        else
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(s & 0x000000ff);
                s >>= 8;
            }
        return buf;
    }

    public static byte[] getBytes(long s, bool asc)
    {
        byte[] buf = new byte[8];
        if (asc)
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                buf[i] = (byte)(s & 0x00000000000000ff);
                s >>= 8;
            }
        else
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(s & 0x00000000000000ff);
                s >>= 8;
            }
        return buf;
    }
    public static byte[] getBytes(string s)
    {
        return System.Text.Encoding.UTF8.GetBytes(s);
    }
    public static string getString(byte[] buf)
    {
        return System.Text.Encoding.UTF8.GetString(buf);
    }

    public static float getFloat(byte[] buf, bool asc)
    {
        int i = getInt(buf, asc);
        float s = (float)i;
        return s / 100;
    }

    public static short getShort(byte[] buf, bool asc)
    {
        if (buf == null)
        {
            //throw new IllegalArgumentException("byte array is null!");
        }
        if (buf.Length > 2)
        {
            //throw new IllegalArgumentException("byte array size > 2 !");
        }
        short r = 0;
        if (!asc)
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                r <<= 8;
                r |= (short)(buf[i] & 0x00ff);
            }
        else
            for (int i = 0; i < buf.Length; i++)
            {
                r <<= 8;
                r |= (short)(buf[i] & 0x00ff);
            }
        return r;
    }

    public static int getInt(byte[] buf, bool asc)
    {
        if (buf == null)
        {
            // throw new IllegalArgumentException("byte array is null!");
        }
        if (buf.Length > 4)
        {
            //throw new IllegalArgumentException("byte array size > 4 !");
        }
        int r = 0;
        if (!asc)
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                r <<= 8;
                r |= (buf[i] & 0x000000ff);
            }
        else
            for (int i = 0; i < buf.Length; i++)
            {
                r <<= 8;
                r |= (buf[i] & 0x000000ff);
            }
        return r;
    }

    public static long getLong(byte[] buf, bool asc)
    {
        if (buf == null)
        {
            //throw new IllegalArgumentException("byte array is null!");
        }
        if (buf.Length > 8)
        {
            //throw new IllegalArgumentException("byte array size > 8 !");
        }
        long r = 0;
        if (!asc)
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                r <<= 8;
                r |= (buf[i] & 0x00000000000000ff);
            }
        else
            for (int i = 0; i < buf.Length; i++)
            {
                r <<= 8;
                r |= (buf[i] & 0x00000000000000ff);
            }
        return r;
    }
}