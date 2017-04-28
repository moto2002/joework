namespace Mogo.RPC
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AttachedInfo
    {
        public EntityDef entity { get; set; }

        public uint id { get; set; }

        public List<KeyValuePair<EntityDefProperties, object>> props { get; set; }

        public ushort typeId { get; set; }
    }
}

