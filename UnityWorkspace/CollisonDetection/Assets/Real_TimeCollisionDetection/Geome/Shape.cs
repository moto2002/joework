using UnityEngine;
using System.Collections;

public enum ShapeType
{
	None,
	Circle,
	Rectangle,
	Triangle,
	Sector
}

public class Shape 
{
	public ShapeType m_shapeType = ShapeType.None;

	public Vector2 center;
	public Shape(Vector2 _center)
	{
		this.center = _center;
	}

}
