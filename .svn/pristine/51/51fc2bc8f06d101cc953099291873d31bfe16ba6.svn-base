namespace Mogo.RPC
{
    using System;

    public class VLuaTable : VObject
    {
        private static VLuaTable m_instance = new VLuaTable();

        public VLuaTable() : base(typeof(LuaTable), VType.V_LUATABLE, 0)
        {
        }

        public VLuaTable(object vValue) : base(typeof(LuaTable), VType.V_LUATABLE, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            LuaTable table;
            return (Utils.ParseLuaTable(base.CutLengthHead(data, ref index), out table) ? table : null);
        }

        public override byte[] Encode(object vValue)
        {
            LuaTable table;
            Utils.PackLuaTable(vValue, out table);
            string str = Utils.PackLuaTable(table);
            return VString.Instance.Encode(str);
        }

        public static VLuaTable Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

