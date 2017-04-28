using UnityEngine;
using System.Collections;

namespace Geome
{
    public class Circle : Shape
    {
        public float Radius { get; set; }   

        public Circle(Transform trans,float radius)
            : base(trans)
        {
            this.Radius = radius;
        }

        public override void Draw(Color color)
        {
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawSolidArc(this.Trans.position, Vector3.up, this.Trans.forward, 360f, this.Radius);
        }
    }
}

