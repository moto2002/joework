import java.io.BufferedInputStream;
import java.io.DataInputStream;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;

public class DBBinary
{
	static final byte DB_BIN_MODE_ARRAY = 0;
	static final byte DB_BIN_MODE_INT_DICT = 1;
	static final byte DB_BIN_MODE_STR_DICT = 2;
	
	static final byte DB_COMPLEX_TYPE_NONE	= 0;
	static final byte DB_COMPLEX_TYPE_LIST	= 1;
	static final byte DB_COMPLEX_TYPE_DICT	= 2;
	
	static final byte DB_BIN_DATATYPE_INT		= 1;
	static final byte DB_BIN_DATATYPE_BOOL		= 2;
	static final byte DB_BIN_DATATYPE_FLOAT		= 3;
	static final byte DB_BIN_DATATYPE_STRING	= 4;
	
	public static HashMap<String, Object> s_unionTable;
	
	protected static short readShort(DataInputStream stream) throws IOException
	{
		short value = (short) (stream.readByte() | (stream.readByte() << 8));
		return value;
	}
	
	protected static int readInt(DataInputStream stream) throws IOException
	{
		int value = stream.readByte() | (stream.readByte() << 8) | (stream.readByte() << 16) | (stream.readByte() << 24);
		return value;
	}
	
	protected static String readString(DataInputStream stream) throws IOException
	{
		int length = stream.readByte();
		char[] chars = new char[length];
		for (int i = 0; i < length; i++)
		{
			chars[i] = (char)stream.readByte();
		}
		return String.valueOf(chars);
	}
	
	protected static float readFloat(DataInputStream stream) throws IOException
	{
		String strValue = readString(stream);
		return Float.parseFloat(strValue);
	}
	
	protected static Object readAttribute(DataInputStream stream, byte type) throws IOException
	{
		Object value = null;
		switch (type)
		{
		case DB_BIN_DATATYPE_INT:
			value = readInt(stream);
			break;
		case DB_BIN_DATATYPE_BOOL:
			value = stream.readBoolean();
			break;
		case DB_BIN_DATATYPE_FLOAT:
			value = readFloat(stream);
			break;
		case DB_BIN_DATATYPE_STRING:
			value = readString(stream);
			break;
		}
		return value;
	}
	
	protected static Object readList(DataInputStream stream, byte type) throws IOException
	{
		short count = readShort(stream);
		Object[] list = new Object[count];
		for (int i = 0; i < count; i++)
		{
			list[i] = readAttribute(stream, type);
		}
		return list;
	}
	
	protected static Object readDict(DataInputStream stream, byte type) throws IOException
	{
		short count = readShort(stream);
		HashMap<String, Object> dict = new HashMap<String, Object>(count, 1.0f);
		for (int i = 0; i < count; i++)
		{
			String key = readString(stream);
			Object value = readAttribute(stream, type);
			dict.put(key, value);
		}
		return dict;
	}
	
	static String[] readUnionKeys(DataInputStream stream) throws IOException
	{
		byte unionCount = stream.readByte();
		String[] keys = new String[unionCount];
		for (int i = 0; i < unionCount; i++)
		{
			keys[i] = readString(stream);
		}
		return keys;
	}
	
	public static Object loadData(String fileName)
	{
		try
		{
			DataInputStream stream = new DataInputStream(new BufferedInputStream(new FileInputStream(fileName)));
			Object dataBase = parseDB(stream);
			return dataBase;
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		return null;
	}
	
	public static Object parseDB(DataInputStream stream)
	{
		Object dataBase = null;
		
		try
		{
			byte mode = stream.readByte();
			short dataCount = readShort(stream);
			short valueCount = readShort(stream);
			String[] unionKeys = readUnionKeys(stream);
			HashMap<String, Object> unionTable = new HashMap<String, Object>();
			
			if (mode == DB_BIN_MODE_ARRAY)
			{
				ArrayList<Object> temp = new ArrayList<Object>(dataCount);
				for (int i = 0; i < dataCount; i++)
				{
					HashMap<String, Object> data = readData(stream, valueCount);
					temp.add(data);
					setUnionKeys(unionTable, data, unionKeys);
				}
				dataBase = temp;
			}
			else if (mode == DB_BIN_MODE_INT_DICT)
			{
				HashMap<Integer, Object> temp = new HashMap<Integer, Object>(dataCount, 1.0f);
				for (int i = 0; i < dataCount; i++)
				{
					int index = readInt(stream);
					HashMap<String, Object> data = readData(stream, valueCount);
					temp.put(index, data);
					setUnionKeys(unionTable, data, unionKeys);
				}
				dataBase = temp;
			}
			else if (mode == DB_BIN_MODE_STR_DICT)
			{
				HashMap<String, Object> temp = new HashMap<String, Object>(dataCount, 1.0f);
				for (int i = 0; i < dataCount; i++)
				{
					String key = readString(stream);
					HashMap<String, Object> data = readData(stream, valueCount);
					temp.put(key, data);
					setUnionKeys(unionTable, data, unionKeys);
				}
				dataBase = temp;
			}
			
			s_unionTable = unionTable;
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		return dataBase;
	}
	
	protected static HashMap<String, Object> readData(DataInputStream stream, int count)
	{
		HashMap<String, Object> data = new HashMap<String, Object>();

		try
		{
			for (int i = 0; i < count; i++)
			{
				String name = readString(stream);
				byte type = stream.readByte();
				byte complex = stream.readByte();
				Object value = null;
				switch (complex)
				{
				case DB_COMPLEX_TYPE_NONE:
					value = readAttribute(stream, type);
					break;
				case DB_COMPLEX_TYPE_LIST:
					value = readList(stream, type);
					break;
				case DB_COMPLEX_TYPE_DICT:
					value = readDict(stream, type);
					break;
				}
				data.put(name, value);
			}
		} catch (IOException e) {
			e.printStackTrace();
		}

		return data;
	}
	
	protected static void printAll(Object db, String title)
	{
		String className = db.getClass().toString();
		System.out.println(title);
		if (className.contains("HashMap"))
		{
			HashMap<Object, Object> map = (HashMap<Object, Object>)db;
			Object[] keys = map.keySet().toArray();
			for (int i = 0; i < keys.length; i++)
			{
				Object key = keys[i];
				System.out.println(key.toString() + ": " + map.get(key).toString());
			}
		}
		else
		{
			ArrayList<Object> list = (ArrayList<Object>)db;
			for (int i = 0; i < list.size(); i++)
			{
				System.out.println(list.get(i).toString());
			}
		}
	}
	
	public static void setUnionKeys(HashMap<String, Object> unionTable, HashMap<String, Object> data, String[] unionKeys)
	{
		if (unionKeys.length > 0)
		{
			String unionKey = "";
			for (int i = 0; i < unionKeys.length; i++)
			{
				unionKey += data.get(unionKeys[i]) + (i < unionKeys.length - 1 ? "_" : "");
			}
			unionTable.put(unionKey, data);
		}
	}
}
