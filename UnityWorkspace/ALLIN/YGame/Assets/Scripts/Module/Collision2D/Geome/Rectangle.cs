using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Geome
{
    public class Rectangle : Shape
    {
        public float Length { get; set; }
        public float Width { get; set; }

        public Rectangle(Transform trans, float length, float width)
            : base(trans)
        {
            this.Length = length;
            this.Width = width;
        }

        public Vector3[] GetVerts()
        {
            Vector3 dir = this.Trans.forward.normalized;
            Vector3 dirNormal = new Vector3(-dir.z, 0, dir.x);

            Vector3 left_bottom = this.Trans.position + dirNormal * this.Length * 0.5f;
            Vector3 right_bottom = this.Trans.position - dirNormal * this.Length * 0.5f;
            Vector3 right_top = right_bottom + dir * this.Width;
            Vector3 left_top = left_bottom + dir * this.Width;

            //顺时针存储
            // Vector3[] verts = new Vector3[4] { left_bottom, right_bottom, right_top, left_top };
            Vector3[] verts = new Vector3[4] { right_top, right_bottom, left_bottom, left_top };
            return verts;

        }

        /// <summary>
        /// 获取法线
        /// </summary>
        public List<Vector2> GetNormals()
        {
            List<Vector2> normals = new List<Vector2>();
            Vector3[] verts = this.GetVerts();
            for (int i = 0; i < verts.Length - 1; i++)
            {
                Vector2 currentNormal = new Vector2(verts[i + 1].x - verts[i].x, verts[i + 1].z - verts[i].z);
                currentNormal = currentNormal.normalized;
                normals.Add(currentNormal);
            }

            return normals;
        }


        public List<Vector2> PrepareVector()
        {
            List<Vector2> vecs_box = new List<Vector2>();
            Vector3[] verts = this.GetVerts();
            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 corner_box = verts[i];
                vecs_box.Add(new Vector2(corner_box.x - 0, corner_box.z - 0));
            }
            return vecs_box;
        }


        public override void Draw(Color color)
        {
            UnityEditor.Handles.DrawSolidRectangleWithOutline(this.GetVerts(), color, color);
        }
    }
}

