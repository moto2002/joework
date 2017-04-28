using UnityEngine;

public class BesizerUtil {
	
	public static Vector2 GetBezierPoint(ref Vector2 anchor1, ref Vector2 control1, ref Vector2 anchor2, ref Vector2 control2, float t)
	{
		float cx = 3 * (control1.x - anchor1.x);
		float bx = 3 * (control2.x - control1.x) - cx;
		float ax = anchor2.x - anchor1.x - cx - bx;
		float cy = 3 * (control1.y - anchor1.y);
		float by = 3 * (control2.y - control1.y) - cy;
		float ay = anchor2.y - anchor1.y - cy - by;
		
		return new Vector2((ax * (t * t * t)) + (bx * (t * t)) + (cx * t) + anchor1.x,
		                   (ay * (t * t * t)) + (by * (t * t)) + (cy * t) + anchor1.y);
	}
	
	public static Vector3 GetBezierPoint3D(ref Vector3 anchor1, ref Vector3 control1, ref Vector3 anchor2, ref Vector3 control2, float t)
	{
		float cx = 3 * (control1.x - anchor1.x);
		float bx = 3 * (control2.x - control1.x) - cx;
		float ax = anchor2.x - anchor1.x - cx - bx;
		float cy = 3 * (control1.y - anchor1.y);
		float by = 3 * (control2.y - control1.y) - cy;
		float ay = anchor2.y - anchor1.y - cy - by;
		float cz = 3 * (control1.z - anchor1.z);
		float bz = 3 * (control2.z - control1.z) - cz;
		float az = anchor2.z - anchor1.z - cz - bz;
		
		return new Vector3((ax * (t * t * t)) + (bx * (t * t)) + (cx * t) + anchor1.x,
		                   (ay * (t * t * t)) + (by * (t * t)) + (cy * t) + anchor1.y,
		                   (az * (t * t * t)) + (bz * (t * t)) + (cz * t) + anchor1.z);
	}
	
}
