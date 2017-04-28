using System;

namespace NsTcpClient
{

    internal interface ICRC
    {

        long Value
        {
            get;
        }

        void Reset();

        void Crc(int bval);

        void Crc(byte[] buffer);

        void Crc(byte[] buf, int off, int len);

    }


    public sealed class CRC8 : ICRC
    {
        #region CRC 8 位校验表

        /// <summary> 
        /// CRC 8 位校验表 
        /// </summary> 
        readonly private static byte[] CRC8_Table = new byte[] 
		{ 
			0,94,188,226,97,63,221,131,194,156,126,32,163,253,31,65, 
			157,195,33,127,252,162,64,30, 95,1,227,189,62,96,130,220, 
			35,125,159,193,66,28,254,160,225,191,93,3,128,222,60,98, 
			190,224,2,92,223,129,99,61,124,34,192,158,29,67,161,255, 
			70,24,250,164,39,121,155,197,132,218,56,102,229,187,89,7,             
			219,133,103,57,186,228,6,88,25,71,165,251,120,38,196,154, 
			101,59,217,135,4,90,184,230,167,249,27,69,198,152,122,36,                         
			248,166,68,26,153,199,37,123,58,100,134,216,91,5,231,185,             
			140,210,48,110,237,179,81,15,78,16,242,172,47,113,147,205, 
			17,79,173,243,112,46,204,146,211,141,111,49,178,236,14,80, 
			175,241,19,77,206,144,114,44,109,51,209,143,12,82,176,238, 
			50,108,142,208,83,13,239,177,240,174,76,18,145,207,45,115, 
			202,148,118,40,171,245,23,73,8,86,180,234,105,55,213,139, 
			87,9,235,181,54,104,138,212,149,203, 41,119,244,170,72,22, 
			233,183,85,11,136,214,52,106,43,117,151,201,74,20,246,168, 
			116,42,200,150,21,75,169,247,182,232,10,84,215,137,107,53 
		};
        #endregion

        uint crc = 0;

        /// <summary> 
        ///返回 CRC8校验结果; 
        /// </summary> 
        public long Value
        {
            get
            {
                return crc;
            }
            set
            {
                crc = (uint)value;
            }
        }


        /// <summary> 
        /// CRC校验前设置校验值 
        /// </summary> 
        public void Reset()
        {
            crc = 0;
        }

        /// <summary> 
        /// 8 位 CRC 校验 产生校验码 需要被校验码和校验码 
        /// </summary> 
        /// <param name="CRC"></param> 
        /// <param name="OldCRC"> 初始为 0 ,以后为 返回值 ret </param> 
        /// <returns> 产生校验码时 ret　为校验码</returns> 

        public void Crc(byte CRC, byte OldCRC)
        {
            crc = CRC8_Table[OldCRC ^ CRC];
        }

        /// <summary> 
        /// 8 位 CRC 校验 产生校验码 只要被校验码 
        /// </summary> 
        /// <param name="bval"></param> 
        public void Crc(int bval)
        {
            crc = CRC8_Table[crc ^ bval];
        }

        /// <summary> 
        /// 8 位 CRC 校验 产生校验码 只要被校验的字节数组 
        /// </summary> 
        /// <param name="buffer"></param> 
        public void Crc(byte[] buffer)
        {

            Crc(buffer, 0, buffer.Length);
        }

        /// <summary> 
        /// 8 位 CRC 校验 产生校验码 要被校验的字节数组、起始结果位置和字节长度 
        /// </summary> 
        /// <param name="buf"></param> 
        /// <param name="off"></param> 
        /// <param name="len"></param> 
        public void Crc(byte[] buf, int off, int len)
        {
            if (buf == null)
            {
                throw new ArgumentNullException("buf");
            }

            if (off < 0 || len < 0 || off + len > buf.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = off; i < len; i++)
            {
                Crc(buf[i]);
            }
        }
    }

    public sealed class CRC16 : ICRC
    {
        #region CRC 16 位校验表

        /// <summary> 
        /// 16 位校验表 Upper 表 
        /// </summary> 
        readonly private static ushort[] uppercrctab = new ushort[] 
		{ 
			0x0000,0x1231,0x2462,0x3653,0x48c4,0x5af5,0x6ca6,0x7e97, 
			0x9188,0x83b9,0xb5ea,0xa7db,0xd94c,0xcb7d,0xfd2e,0xef1f 
		};

        /// <summary> 
        /// 16 位校验表 Lower 表 
        /// </summary> 
        readonly private static ushort[] lowercrctab = new ushort[] 
		{ 
			0x0000,0x1021,0x2042,0x3063,0x4084,0x50a5,0x60c6,0x70e7, 
			0x8108,0x9129,0xa14a,0xb16b,0xc18c,0xd1ad,0xe1ce,0xf1ef 
		};
        #endregion

        ushort crc = 0;

        /// <summary> 
        /// 校验后的结果 
        /// </summary> 
        public long Value
        {
            get
            {
                return crc;
            }
            set
            {
                crc = (ushort)value;
            }
        }

        /// <summary> 
        /// 设置crc 初始值 
        /// </summary> 
        public void Reset()
        {
            crc = 0;
        }

        /// <summary> 
        /// Crc16 
        /// </summary> 
        /// <param name="ucrc"></param> 
        /// <param name="buf"></param> 
        public void Crc(ushort ucrc, byte[] buf)
        {
            crc = ucrc;
            Crc(buf);
        }

        /// <summary> 
        /// Crc16 
        /// </summary> 
        /// <param name="bval"></param> 
        public void Crc(int bval)
        {
            ushort h = (ushort)((crc >> 12) & 0x0f);
            ushort l = (ushort)((crc >> 8) & 0x0f);
            ushort temp = crc;
            temp = (ushort)(((temp & 0x00ff) << 8) | bval);
            temp = (ushort)(temp ^ (uppercrctab[(h - 1) + 1] ^ lowercrctab[(l - 1) + 1]));
            crc = temp;
        }


        /// <summary> 
        /// Crc16 
        /// </summary> 
        /// <param name="buffer"></param> 
        public void Crc(byte[] buffer)
        {
            Crc(buffer, 0, buffer.Length);
        }

        /// <summary> 
        /// Crc16 
        /// </summary> 
        /// <param name="buf"></param> 
        /// <param name="off"></param> 
        /// <param name="len"></param> 
        public void Crc(byte[] buf, int off, int len)
        {
            if (buf == null)
            {
                throw new ArgumentNullException("buf");
            }

            if (off < 0 || len < 0 || off + len > buf.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = off; i < len; i++)
            {
                Crc(buf[i]);
            }
        }
    }


    public sealed class Crc32 : ICRC
    {
        readonly private static uint CrcSeed = 0xFFFFFFFF;

        readonly private static uint[] CrcTable = new uint[] { 
			0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA, 0x076DC419, 
			0x706AF48F, 0xE963A535, 0x9E6495A3, 0x0EDB8832, 0x79DCB8A4, 
			0xE0D5E91E, 0x97D2D988, 0x09B64C2B, 0x7EB17CBD, 0xE7B82D07, 
			0x90BF1D91, 0x1DB71064, 0x6AB020F2, 0xF3B97148, 0x84BE41DE, 
			0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7, 0x136C9856, 
			0x646BA8C0, 0xFD62F97A, 0x8A65C9EC, 0x14015C4F, 0x63066CD9, 
			0xFA0F3D63, 0x8D080DF5, 0x3B6E20C8, 0x4C69105E, 0xD56041E4, 
			0xA2677172, 0x3C03E4D1, 0x4B04D447, 0xD20D85FD, 0xA50AB56B, 
			0x35B5A8FA, 0x42B2986C, 0xDBBBC9D6, 0xACBCF940, 0x32D86CE3, 
			0x45DF5C75, 0xDCD60DCF, 0xABD13D59, 0x26D930AC, 0x51DE003A, 
			0xC8D75180, 0xBFD06116, 0x21B4F4B5, 0x56B3C423, 0xCFBA9599, 
			0xB8BDA50F, 0x2802B89E, 0x5F058808, 0xC60CD9B2, 0xB10BE924, 
			0x2F6F7C87, 0x58684C11, 0xC1611DAB, 0xB6662D3D, 0x76DC4190, 
			0x01DB7106, 0x98D220BC, 0xEFD5102A, 0x71B18589, 0x06B6B51F, 
			0x9FBFE4A5, 0xE8B8D433, 0x7807C9A2, 0x0F00F934, 0x9609A88E, 
			0xE10E9818, 0x7F6A0DBB, 0x086D3D2D, 0x91646C97, 0xE6635C01, 
			0x6B6B51F4, 0x1C6C6162, 0x856530D8, 0xF262004E, 0x6C0695ED, 
			0x1B01A57B, 0x8208F4C1, 0xF50FC457, 0x65B0D9C6, 0x12B7E950, 
			0x8BBEB8EA, 0xFCB9887C, 0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3, 
			0xFBD44C65, 0x4DB26158, 0x3AB551CE, 0xA3BC0074, 0xD4BB30E2, 
			0x4ADFA541, 0x3DD895D7, 0xA4D1C46D, 0xD3D6F4FB, 0x4369E96A, 
			0x346ED9FC, 0xAD678846, 0xDA60B8D0, 0x44042D73, 0x33031DE5, 
			0xAA0A4C5F, 0xDD0D7CC9, 0x5005713C, 0x270241AA, 0xBE0B1010, 
			0xC90C2086, 0x5768B525, 0x206F85B3, 0xB966D409, 0xCE61E49F, 
			0x5EDEF90E, 0x29D9C998, 0xB0D09822, 0xC7D7A8B4, 0x59B33D17, 
			0x2EB40D81, 0xB7BD5C3B, 0xC0BA6CAD, 0xEDB88320, 0x9ABFB3B6, 
			0x03B6E20C, 0x74B1D29A, 0xEAD54739, 0x9DD277AF, 0x04DB2615, 
			0x73DC1683, 0xE3630B12, 0x94643B84, 0x0D6D6A3E, 0x7A6A5AA8, 
			0xE40ECF0B, 0x9309FF9D, 0x0A00AE27, 0x7D079EB1, 0xF00F9344, 
			0x8708A3D2, 0x1E01F268, 0x6906C2FE, 0xF762575D, 0x806567CB, 
			0x196C3671, 0x6E6B06E7, 0xFED41B76, 0x89D32BE0, 0x10DA7A5A, 
			0x67DD4ACC, 0xF9B9DF6F, 0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5, 
			0xD6D6A3E8, 0xA1D1937E, 0x38D8C2C4, 0x4FDFF252, 0xD1BB67F1, 
			0xA6BC5767, 0x3FB506DD, 0x48B2364B, 0xD80D2BDA, 0xAF0A1B4C, 
			0x36034AF6, 0x41047A60, 0xDF60EFC3, 0xA867DF55, 0x316E8EEF, 
			0x4669BE79, 0xCB61B38C, 0xBC66831A, 0x256FD2A0, 0x5268E236, 
			0xCC0C7795, 0xBB0B4703, 0x220216B9, 0x5505262F, 0xC5BA3BBE, 
			0xB2BD0B28, 0x2BB45A92, 0x5CB36A04, 0xC2D7FFA7, 0xB5D0CF31, 
			0x2CD99E8B, 0x5BDEAE1D, 0x9B64C2B0, 0xEC63F226, 0x756AA39C, 
			0x026D930A, 0x9C0906A9, 0xEB0E363F, 0x72076785, 0x05005713, 
			0x95BF4A82, 0xE2B87A14, 0x7BB12BAE, 0x0CB61B38, 0x92D28E9B, 
			0xE5D5BE0D, 0x7CDCEFB7, 0x0BDBDF21, 0x86D3D2D4, 0xF1D4E242, 
			0x68DDB3F8, 0x1FDA836E, 0x81BE16CD, 0xF6B9265B, 0x6FB077E1, 
			0x18B74777, 0x88085AE6, 0xFF0F6A70, 0x66063BCA, 0x11010B5C, 
			0x8F659EFF, 0xF862AE69, 0x616BFFD3, 0x166CCF45, 0xA00AE278, 
			0xD70DD2EE, 0x4E048354, 0x3903B3C2, 0xA7672661, 0xD06016F7, 
			0x4969474D, 0x3E6E77DB, 0xAED16A4A, 0xD9D65ADC, 0x40DF0B66, 
			0x37D83BF0, 0xA9BCAE53, 0xDEBB9EC5, 0x47B2CF7F, 0x30B5FFE9, 
			0xBDBDF21C, 0xCABAC28A, 0x53B39330, 0x24B4A3A6, 0xBAD03605, 
			0xCDD70693, 0x54DE5729, 0x23D967BF, 0xB3667A2E, 0xC4614AB8, 
			0x5D681B02, 0x2A6F2B94, 0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B, 
			0x2D02EF8D 
		};

        internal static uint ComputeCrc32(uint oldCrc, byte bval)
        {
            return (uint)(Crc32.CrcTable[(oldCrc ^ bval) & 0xFF] ^ (oldCrc >> 8));
        }

        uint crc = 0;

        public long Value
        {
            get
            {
                return (long)crc;
            }
            set
            {
                crc = (uint)value;
            }
        }

        public void Reset()
        {
            crc = 0;
        }

        public void Crc(int bval)
        {
            crc ^= CrcSeed;
            crc = CrcTable[(crc ^ bval) & 0xFF] ^ (crc >> 8);
            crc ^= CrcSeed;
        }

        public void Crc(byte[] buffer)
        {
            Crc(buffer, 0, buffer.Length);
        }

        public void Crc(byte[] buf, int off, int len)
        {
            if (buf == null)
            {
                throw new ArgumentNullException("buf");
            }

            if (off < 0 || len < 0 || off + len > buf.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            crc ^= CrcSeed;

            while (--len >= 0)
            {
                crc = CrcTable[(crc ^ buf[off++]) & 0xFF] ^ (crc >> 8);
            }

            crc ^= CrcSeed;
        }
    }

}
