using UnityEngine;
using System.Collections;

public class BoxCircleIntersect : MonoBehaviour 
{
	public Transform boxTrans;
	public Transform circleTrans;

	Rectangle rect;
	Circle circle;

	void Start () 
	{
		rect = new Rectangle(10f,5f,new Vector2(boxTrans.position.x,boxTrans.position.z));
		circle = new Circle(3f,new Vector2(circleTrans.position.x ,circleTrans.position.z));

		GameObject range = GameObject.Instantiate( Resources.Load("range") as GameObject);
		range.transform.parent = circleTrans;
		range.transform.localPosition = Vector3.zero;
		range.transform.localScale = Vector3.one * circle.radius * 2f;
	}

	void Update () 
	{

		//刷新中心点
		rect.Refresh(new Vector2(boxTrans.position.x,boxTrans.position.z),boxTrans.rotation);

        Matrix4x4 matrix = boxTrans.worldToLocalMatrix;
        Vector3 newv = matrix.MultiplyPoint3x4(circleTrans.position);
        circle.Refresh(new Vector2(newv.x, newv.z));

//
//		#region 画出矩形范围
//		Quaternion r = boxTrans.rotation;
//		Vector3 leftbottom = new Vector3(boxTrans.position.x,boxTrans.position.y,boxTrans.position.z - (rect.half_width));
//		Vector3 left = ( leftbottom + (r * Vector3.left) * rect.half_length);
//		Debug.DrawLine(leftbottom, left, Color.red);
//		
//		Vector3 rightbottom = new Vector3(boxTrans.position.x,boxTrans.position.y,boxTrans.position.z - (rect.half_width));
//		Vector3 right = ( rightbottom+ (r * Vector3.right) * rect.half_length);
//		Debug.DrawLine(rightbottom, right, Color.red);
//		
//		Vector3 leftEnd = (left + (r * Vector3.forward) * rect.width);
//		Debug.DrawLine(left, leftEnd, Color.red);
//		
//		Vector3 rightEnd = (right + (r * Vector3.forward) * rect.width);
//		Debug.DrawLine(right, rightEnd, Color.red);
//		
//		Debug.DrawLine(leftEnd, rightEnd, Color.red);
//		#endregion

		#region 画出圆形正前方的半径
		Quaternion r_circle = boxTrans.rotation;
		Vector3 circleTop = (circleTrans.position + (r_circle * Vector3.forward) * circle.radius);
		Debug.DrawLine(circleTop, circleTrans.position, Color.yellow);
		#endregion

		if(this.CalcBoxCircleIntersect(rect.center,rect.GetH(),circle.center,circle.radius))
		{
			Debug.Log("in");
		}
		else
		{
			Debug.Log(" not in ");
		}
	}

	/// <summary>
	/// Calculates the box circle intersect.
	/// </summary>
	/// <returns><c>true</c>, if box circle intersect was calculated, <c>false</c> otherwise.</returns>
	/// <param name="c">矩形的中心点</param>
	/// <param name="h">矩形的半长向量</param>
	/// <param name="p">圆的中心点</param>
	/// <param name="r">圆的半径.</param>
	bool CalcBoxCircleIntersect(Vector2 c, Vector2 h, Vector2 p, float r)
	{
		p = new Vector2(Mathf.Abs(p.x),Mathf.Abs(p.y));
		c = new Vector2(Mathf.Abs(c.x),Mathf.Abs(c.y));
		Vector2 v = (p - c);    // 第1步：转换至第1象限

		Vector2 _u = v-h;
		Vector2 u = new Vector2(Mathf.Max(_u.x, 0),Mathf.Max(_u.y,0)); // 第2步：求圆心至矩形的最短距离矢量

		//return Vector2.Dot(u, u) <= r * r; // 第3步：长度平方与半径平方比较

        //return u.sqrMagnitude <= r * r; // 第3步：长度平方与半径平方比较

        return u.x * u.x + u.y * u.y <= r * r;
	}
}



