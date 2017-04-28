import java.io.BufferedInputStream;
import java.io.DataInputStream;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.HashMap;

public class DBBase
{
	protected static final String BIN_FILE_PATH = "res/";
	
    private static DataInputStream s_stream;

    /**
     * 读取二进制文件
     */
    protected static void loadData(String fileName)
    {
        try
        {
        	s_stream = new DataInputStream(new BufferedInputStream(new FileInputStream(BIN_FILE_PATH + fileName)));
        } catch (IOException e)
        {   
            e.printStackTrace();
        }
    }

    /**
     * 关闭二进制文件
     */
    protected static void clearData()
    {
    	try
    	{
			s_stream.close();
			s_stream = null;
		} catch (IOException e) {
			e.printStackTrace();
		}
        System.gc();
    }

    /**
     * 跳过字节
     */
    protected static void skip(int step) throws IOException
    {
    	s_stream.skip(step);
    }
    
    /**
     * 读取1字节
     */
    protected static short readByte() throws IOException
	{
		return s_stream.readByte();
	}
    
    /**
     * 读取短整型
     */
    protected static short readShort() throws IOException
	{
		short value = (short) (s_stream.readByte() | (s_stream.readByte() << 8));
		return value;
	}
    
    /**
     * 读取整型
     */
    protected static int readInt() throws IOException
    {
    	int value = s_stream.readByte() | (s_stream.readByte() << 8) | (s_stream.readByte() << 16) | (s_stream.readByte() << 24);
		return value;
    }

    /**
     * 读取浮点数
     */
    protected static float readFloat() throws IOException
    {
    	String value = readString();
		return Float.parseFloat(value);
    }

    /**
     * 读取布尔值
     */
    protected static boolean readBoolean() throws IOException
    {
        return s_stream.readBoolean();
    }

    /**
     * 读取字符串
     */
    protected static String readString() throws IOException
	{
		int length = s_stream.readByte();
		char[] chars = new char[length];
		for (int i = 0; i < length; i++)
		{
			chars[i] = (char)s_stream.readByte();
		}
		return String.valueOf(chars);
	}

    /**
     * 读取int数组
     */
    protected static int[] readIntArray() throws IOException
    {
        short count = readShort();
        if (count > 0)
        {
	        int[] res = new int[count];
	        for (int i = 0; i < count; i++)
	        {
	            res[i] = readInt();
	        }
	        return res;
        }
        return null;
    }

    /**
     * 读取float数组
     */
    protected static float[] readFloatArray() throws IOException
    {
    	short count = readShort();
        if (count > 0)
        {
	        float[] res = new float[count];
	        for (int i = 0; i < count; i++)
	        {
	            res[i] = readFloat();
	        }
	        return res;
        }
        return null;
    }

    /**
     * 读取boolean数组
     */
    protected static boolean[] readBooleanArray() throws IOException
    {
    	short count = readShort();
        if (count > 0)
        {
	        boolean[] res = new boolean[count];
	        for (int i = 0; i < count; i++)
	        {
	            res[i] = readBoolean();
	        }
	        return res;
        }
        return null;
    }

    /**
     * 读取string数组
     */
    protected static String[] readStringArray() throws IOException
    {
    	short count = readShort();
        if (count > 0)
        {
	        String[] res = new String[count];
	        for (int i = 0; i < count; i++)
	        {
	            res[i] = readString();
	        }
	        return res;
        }
        return null;
    }

    /**
     * 读取int字典
     */
    protected static HashMap<String, Integer> readIntMap() throws IOException
    {
    	short count = readShort();
        if (count > 0)
        {
	        HashMap<String, Integer> res = new HashMap<String, Integer>(count, 1.0f);
	        for (int i = 0; i < count; i++)
	        {
	            res.put(readString(), readInt());
	        }
	        return res;
        }
        return null;
    }

    /**
     * 读取float字典
     */
    protected static HashMap<String, Float> readFloatMap() throws IOException
    {
    	short count = readShort();
        if (count > 0)
        {
	        HashMap<String, Float> res = new HashMap<String, Float>(count, 1.0f);
	        for (int i = 0; i < count; i++)
	        {
	            res.put(readString(), readFloat());
	        }
	        return res;
        }
        return null;
    }

    /**
     * 读取boolean字典
     */
    protected static HashMap<String, Boolean> readBooleanMap() throws IOException
    {
    	short count = readShort();
        if (count > 0)
        {
	        HashMap<String, Boolean> res = new HashMap<String, Boolean>(count, 1.0f);
	        for (int i = 0; i < count; i++)
	        {
	            res.put(readString(), readBoolean());
	        }
	        return res;
        }
        return null;
    }

    /**
     * 读取string字典
     */
    protected static HashMap<String, String> readStringMap() throws IOException
    {
    	short count = readShort();
        if (count > 0)
        {
	        HashMap<String, String> res = new HashMap<String, String>(count, 1.0f);
	        for (int i = 0; i < count; i++)
	        {
	            res.put(readString(), readString());
	        }
	        return res;
        }
        return null;
    }
    
    /**
     * 获取数组描述(int)
     */
    protected String listDesc(int[] attribute)
    {
    	if (attribute == null)
    		return "";
    	String res = "[";
    	for (int i = 0; i < attribute.length; i++)
    	{
    		res += attribute[i];
    		if (i < attribute.length - 1)
    			res += ",";
    	}
    	return res + "]";
    }
    
    /**
     * 获取数组描述(boolean)
     */
    protected String listDesc(boolean[] attribute)
    {
    	if (attribute == null)
    		return "";
    	String res = "[";
    	for (int i = 0; i < attribute.length; i++)
    	{
    		res += attribute[i];
    		if (i < attribute.length - 1)
    			res += ",";
    	}
    	return res + "]";
    }
    
    /**
     * 获取数组描述(float)
     */
    protected String listDesc(float[] attribute)
    {
    	if (attribute == null)
    		return "";
    	String res = "[";
    	for (int i = 0; i < attribute.length; i++)
    	{
    		res += attribute[i];
    		if (i < attribute.length - 1)
    			res += ",";
    	}
    	return res + "]";
    }
    
    /**
     * 获取数组描述(string)
     */
    protected String listDesc(String[] attribute)
    {
    	if (attribute == null)
    		return "";
    	String res = "[";
    	for (int i = 0; i < attribute.length; i++)
    	{
    		res += attribute[i];
    		if (i < attribute.length - 1)
    			res += ",";
    	}
    	return res + "]";
    }
    
    /**
     * 获取字典描述
     */
    protected String dictDesc(Object attribute)
    {
    	if (attribute == null)
    		return "";
    	return attribute.toString();
    }
}
