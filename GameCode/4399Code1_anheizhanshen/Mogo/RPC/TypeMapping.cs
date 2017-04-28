namespace Mogo.RPC
{
    using Mogo.Util;
    using System;

    public class TypeMapping
    {
        private TypeMapping()
        {
        }

        public static VObject GetVObject(string type)
        {
            switch (type)
            {
                case "STRING":
                    return VString.Instance;

                case "INT8":
                    return VInt8.Instance;

                case "UINT8":
                    return VUInt8.Instance;

                case "INT16":
                    return VInt16.Instance;

                case "UINT16":
                    return VUInt16.Instance;

                case "INT32":
                    return VInt32.Instance;

                case "UINT32":
                    return VUInt32.Instance;

                case "INT64":
                    return VInt64.Instance;

                case "UINT64":
                    return VUInt64.Instance;

                case "FLOAT":
                    return VFloat.Instance;

                case "FLOAT64":
                    return VDouble.Instance;

                case "BOOL":
                    return VBoolean.Instance;

                case "BLOB":
                    return VBLOB.Instance;

                case "LUA_TABLE":
                    return VLuaTable.Instance;
            }
            LoggerHelper.Warning(string.Format("Can not find type: {0}.", type), true);
            return VEmpty.Instance;
        }
    }
}

