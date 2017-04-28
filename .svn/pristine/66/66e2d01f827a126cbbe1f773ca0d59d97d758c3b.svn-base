using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 矩形
/// </summary>
public class Rectangle : Shape
{
    public float length;
    public float width;

    private float half_length;
    private float half_width;

    private Vector2[] vertexs;
    public Vector2 GetVertex(int index)
    {
        return this.vertexs[index];
    }
    private void SetVertex(int index, Vector2 _vertex)
    {
        this.vertexs[index] = _vertex;
    }

    public Rectangle(float _length, float _width, Vector2 _center)
        : base(_center)
    {
        this.m_shapeType = ShapeType.Rectangle;

        this.length = _length;
        this.width = _width;
        this.center = _center;

        this.half_length = this.length / 2.0f;
        this.half_width = this.width / 2.0f;

        this.vertexs = new Vector2[5];
    }

    //半长向量
    private Vector2 h;
    public Vector2 GetH()
    {
        //Vector2 x_ = new Vector2(this.center.x + (this.half_length), this.center.y);
       // Vector2 y_ = new Vector2(this.center.x, this.center.y + (this.half_width));

        //Vector2
      //  this.h = (x_ - this.center) + (y_ - this.center);

        this.h = new Vector2(this.vertexs[1].x, this.vertexs[1].y) - this.center;
        return this.h;
    }

    public void Refresh(Vector2 _center, Quaternion _rotate)
    {
        this.center = _center;

        Vector3 _temp = new Vector3(_center.x, 0, _center.y);
        Vector3 bottom = (_temp + (_rotate * Vector3.back) * this.half_width);
        Vector3 left = (bottom + (_rotate * Vector3.left) * this.half_length);
        Vector3 right = (bottom + (_rotate * Vector3.right) * this.half_length);
        Vector3 leftEnd = (left + (_rotate * Vector3.forward) * this.width);
        Vector3 rightEnd = (right + (_rotate * Vector3.forward) * this.width);

        this.SetVertex(0, this.center);
        //顺时针
        this.SetVertex(1, new Vector2(rightEnd.x, rightEnd.z));
        this.SetVertex(2, new Vector2(right.x, right.z));
        this.SetVertex(3, new Vector2(left.x, left.z));
        this.SetVertex(4, new Vector2(leftEnd.x, leftEnd.z));

        //画出线
        Debug.DrawLine(left, right, Color.red);
        Debug.DrawLine(left, leftEnd, Color.red);
        Debug.DrawLine(right, rightEnd, Color.red);
        Debug.DrawLine(leftEnd, rightEnd, Color.red);
    }

 
    /// <summary>
    /// 获取法线,用来做投影的轴
    /// </summary>
    public List<Vector2> GetNormals()
    {
        List<Vector2> normals = new List<Vector2>();
        for (int i = 1; i < this.vertexs.Length - 1; i++)
        {
            Vector2 currentNormal = new Vector2(this.GetVertex(i + 1).x - this.GetVertex(i).x, this.GetVertex(i + 1).y - this.GetVertex(i).y);
            currentNormal = currentNormal.normalized;
            normals.Add(currentNormal);
        }

        return normals;
    }


    public List<Vector2> PrepareVector()
    {
        List<Vector2> vecs_box = new List<Vector2>();
        for (int i = 0; i < 5; i++)
        {
            Vector2 corner_box = this.GetVertex(i);
            vecs_box.Add(new Vector2(corner_box.x - 0, corner_box.y - 0));
        }
        return vecs_box;
    }

}
