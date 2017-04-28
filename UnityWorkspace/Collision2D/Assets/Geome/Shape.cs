using UnityEngine;
using System.Collections;

namespace Geome
{
    public class Shape 
    {
        public Transform Trans { get; set; }

        public Shape(Transform trans)
        {
            this.Trans = trans;
        }

        public virtual void Draw(Color color)
        {

        }
    }
}


