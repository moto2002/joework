


public class test
{
	public static void main(String[] args)
	{
		Person p = new Person();
		p.SetName("Joe");
		p.SetAge(22);
		
		System.out.println(p.GetName() + ":" +p.GetAge());
		
		byte[] b =ConvertTools.Object2Bytes(p);
		
		System.out.println(new String(b));
		System.out.println(b.length);
		Person tp = (Person)ConvertTools.Bytes2Object(b);
		System.out.println(tp.GetAge() + "{}" + tp.GetName());
		System.out.println(ConvertTools.SizeOf(tp));
		System.out.println("--------------------");		
		
		int i = 5;
		byte[] intb = ConvertTools.Int2Bytes(i);
		System.out.println(ConvertTools.Bytes2Int(intb));
		System.out.println("--------------------");		
		
		String ss ="hello joe";
		byte[] sb = ConvertTools.String2Bytes(ss);
		System.out.println(ConvertTools.Bytes2String(sb));
		System.out.println("--------------------");		
		
		float f = 8.0f;
		byte[] fb = ConvertTools.Float2Bytes(f);
		System.out.println(ConvertTools.Bytes2Float(fb));
		System.out.println("--------------------");		
		
		short st = 1;
		byte[] sstb = ConvertTools.Short2Bytes(st);
		System.out.println(sstb.length);
		System.out.println(ConvertTools.Bytes2Short(sstb));
		System.out.println("--------------------");		
		
		
		
		
		
	}

}
