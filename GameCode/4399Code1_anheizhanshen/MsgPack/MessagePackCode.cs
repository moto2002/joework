namespace MsgPack
{
    using System;

    internal static class MessagePackCode
    {
        public const int Array16 = 220;
        public const int Array32 = 0xdd;
        public const int Bin16 = 0xc5;
        public const int Bin32 = 0xc6;
        public const int Bin8 = 0xc4;
        public const int Ext16 = 200;
        public const int Ext32 = 0xc9;
        public const int Ext8 = 0xc7;
        public const int FalseValue = 0xc2;
        public const int FixExt1 = 0xd4;
        public const int FixExt16 = 0xd8;
        public const int FixExt2 = 0xd5;
        public const int FixExt4 = 0xd6;
        public const int FixExt8 = 0xd7;
        public const int Map16 = 0xde;
        public const int Map32 = 0xdf;
        public const int MaximumFixedArray = 0x9f;
        public const int MaximumFixedMap = 0x8f;
        public const int MaximumFixedRaw = 0xbf;
        public const int MinimumFixedArray = 0x90;
        public const int MinimumFixedMap = 0x80;
        public const int MinimumFixedRaw = 160;
        public const int NilValue = 0xc0;
        public const int Raw16 = 0xda;
        public const int Raw32 = 0xdb;
        public const int Real32 = 0xca;
        public const int Real64 = 0xcb;
        public const int SignedInt16 = 0xd1;
        public const int SignedInt32 = 210;
        public const int SignedInt64 = 0xd3;
        public const int SignedInt8 = 0xd0;
        public const int Str8 = 0xd9;
        public const int TrueValue = 0xc3;
        public const int UnsignedInt16 = 0xcd;
        public const int UnsignedInt32 = 0xce;
        public const int UnsignedInt64 = 0xcf;
        public const int UnsignedInt8 = 0xcc;

        public static string ToString(int code)
        {
            if (code < 0x80)
            {
                return "PositiveFixNum";
            }
            if (code >= 0xe0)
            {
                return "NegativeFixNum";
            }
            switch (code)
            {
                case 0xc0:
                    return "Nil";

                case 0xc3:
                    return "True";

                case 0xca:
                    return "Real32";

                case 0xcb:
                    return "Real64";

                case 0xcc:
                    return "UnsignedInt8";

                case 0xcd:
                    return "UnsignedInt16";

                case 0xce:
                    return "UnsignedInt32";

                case 0xcf:
                    return "UnsignedInt64";

                case 0xd0:
                    return "SingnedInt8";

                case 0xd1:
                    return "SignedInt16";

                case 210:
                    return "SignedInt32";

                case 0xd3:
                    return "SignedInt64";

                case 0xda:
                    return "Raw16";

                case 0xdb:
                    return "Raw32";

                case 220:
                    return "Array16";

                case 0xdd:
                    return "Array32";

                case 0xde:
                    return "Map16";

                case 0xdf:
                    return "Map32";
            }
            switch ((code & 240))
            {
                case 160:
                case 0xb0:
                    return "FixedRaw";

                case 0x80:
                    return "FixedMap";

                case 0x90:
                    return "FixedArray";
            }
            return "Unknown";
        }
    }
}

