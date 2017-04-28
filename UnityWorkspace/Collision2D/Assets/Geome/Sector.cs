using UnityEngine;
using System.Collections;

namespace Geome
{
    public class Sector :Shape
    {
        public float Radius { get; set; }
        public float Angle { get; set; }

        public Sector(Transform trans,float radius,float angle)
            : base(trans)
        {
            this.Radius = radius;
            this.Angle = angle;
        }


        public override void Draw(Color color)
        {
            UnityEditor.Handles.color = color;
            Vector3 leftLine = Quaternion.Euler(0, this.Angle * -0.5f, 0) * this.Trans.forward;
            UnityEditor.Handles.DrawSolidArc(this.Trans.position, Vector3.up, leftLine, this.Angle, this.Radius);
        }
    }
}

