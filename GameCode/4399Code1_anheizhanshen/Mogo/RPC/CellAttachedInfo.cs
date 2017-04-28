namespace Mogo.RPC
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CellAttachedInfo : AttachedInfo
    {
        public uint checkFlag { get; set; }

        public Vector3 position
        {
            get
            {
                return new Vector3(this.x * 0.01f, 0f, this.y * 0.01f);
            }
        }

        public short x { get; set; }

        public short y { get; set; }
    }
}

