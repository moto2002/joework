using UnityEngine;
using System.Collections;

public class Matrix2D {

	public float a=1f,b=0f,c=0f,d=1f,tx=0f,ty=0f;

	public Matrix2D Rotate( float angle){
		float c = Mathf.Cos(angle);
		float s = Mathf.Sin(angle);

		float a1 = a;
		float c1 = c;
		float tx1 = tx;

		a = a1 * c - b * s;
		b = a1 * s + b * c;
		c = c1 * c - d * s;
		d = c1 * s+ d * c;
		tx = tx1 * c - ty * s;
		ty = tx1 * s + ty * c;

		return this;
	}

	public Matrix2D Scale(float x,float y){
		a = a*x;
		d = d * y;
		tx = tx* x;
		ty = ty * y;
		return this;
	}

	public Matrix2D Translate(float txx,float tyy)
	{
		tx += txx;
		ty += tyy;
		return this;
	}

	public Matrix2D Identity(){
		a = 1f;
		d = 1f;
		b = 0f;
		c = 0f;
		tx = 0f;
		ty = 0f;
		return this;
	}

}
