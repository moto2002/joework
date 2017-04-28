using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtil {

	/// <summary>
	/// 数组随机顺序
	/// </summary>
	/// <returns>The sort.</returns>
	/// <param name="array">Array.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T[] RandomSort < T > (T[] array)
	{
		int len = array.Length;
		List<int> list = new List<int>();
		T[] ret=new T[len];
		System.Random rand = new System.Random();
		int i = 0;
		while (list.Count < len)
		{
			int iter = rand.Next(0, len);
			if (!list.Contains(iter))
			{
				list.Add(iter);
				ret[i] = array[iter];
				i++;
			}

		}
		return ret;
	} 


	/// <summary>
	/// 判断相交
	/// </summary>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static bool Intersect(ref Rect a,ref Rect b ) {
		FlipNegative( ref a );
		FlipNegative( ref b );
		bool c1 = a.xMin < b.xMax;
		bool c2 = a.xMax > b.xMin;
		bool c3 = a.yMin < b.yMax;
		bool c4 = a.yMax > b.yMin;
		return c1 && c2 && c3 && c4;
	}

	/// <summary>
	/// 反转
	/// </summary>
	/// <param name="r">The red component.</param>
	public static void FlipNegative(ref Rect r) {
		if( r.width < 0 ) 
			r.x -= ( r.width *= -1 );
		if( r.height < 0 )
			r.y -= ( r.height *= -1 );
	}
}
