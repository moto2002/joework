namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.Text;

    public sealed class ZipConstants
    {
        public const int ArchiveExtraDataSignature = 0x7064b50;
        [Obsolete("Use CentralHeaderDigitalSignaure instead")]
        public const int CENDIGITALSIG = 0x5054b50;
        [Obsolete("Use CentralHeaderBaseSize instead")]
        public const int CENHDR = 0x2e;
        [Obsolete("Use CentralHeaderSignature instead")]
        public const int CENSIG = 0x2014b50;
        [Obsolete("Use Zip64CentralFileHeaderSignature instead")]
        public const int CENSIG64 = 0x6064b50;
        public const int CentralHeaderBaseSize = 0x2e;
        public const int CentralHeaderDigitalSignature = 0x5054b50;
        public const int CentralHeaderSignature = 0x2014b50;
        [Obsolete("Use CryptoHeaderSize instead")]
        public const int CRYPTO_HEADER_SIZE = 12;
        public const int CryptoHeaderSize = 12;
        public const int DataDescriptorSignature = 0x8074b50;
        public const int DataDescriptorSize = 0x10;
        private static int defaultCodePage = Encoding.UTF8.CodePage;
        [Obsolete("Use EndOfCentralRecordBaseSize instead")]
        public const int ENDHDR = 0x16;
        public const int EndOfCentralDirectorySignature = 0x6054b50;
        public const int EndOfCentralRecordBaseSize = 0x16;
        [Obsolete("Use EndOfCentralDirectorySignature instead")]
        public const int ENDSIG = 0x6054b50;
        [Obsolete("Use DataDescriptorSize instead")]
        public const int EXTHDR = 0x10;
        [Obsolete("Use DataDescriptorSignature instead")]
        public const int EXTSIG = 0x8074b50;
        public const int LocalHeaderBaseSize = 30;
        public const int LocalHeaderSignature = 0x4034b50;
        [Obsolete("Use LocalHeaderBaseSize instead")]
        public const int LOCHDR = 30;
        [Obsolete("Use LocalHeaderSignature instead")]
        public const int LOCSIG = 0x4034b50;
        [Obsolete("Use SpanningSignature instead")]
        public const int SPANNINGSIG = 0x8074b50;
        public const int SpanningSignature = 0x8074b50;
        public const int SpanningTempSignature = 0x30304b50;
        [Obsolete("Use SpanningTempSignature instead")]
        public const int SPANTEMPSIG = 0x30304b50;
        public const int VERSION_AES = 0x33;
        [Obsolete("Use VersionMadeBy instead")]
        public const int VERSION_MADE_BY = 0x33;
        [Obsolete("Use VersionStrongEncryption instead")]
        public const int VERSION_STRONG_ENCRYPTION = 50;
        public const int VersionMadeBy = 0x33;
        public const int VersionStrongEncryption = 50;
        public const int VersionZip64 = 0x2d;
        public const int Zip64CentralDirLocatorSignature = 0x7064b50;
        public const int Zip64CentralFileHeaderSignature = 0x6064b50;
        public const int Zip64DataDescriptorSize = 0x18;

        private ZipConstants()
        {
        }

        public static byte[] ConvertToArray(string str)
        {
            if (str == null)
            {
                return new byte[0];
            }
            return Encoding.GetEncoding(DefaultCodePage).GetBytes(str);
        }

        public static byte[] ConvertToArray(int flags, string str)
        {
            if (str == null)
            {
                return new byte[0];
            }
            if ((flags & 0x800) != 0)
            {
                return Encoding.UTF8.GetBytes(str);
            }
            return ConvertToArray(str);
        }

        public static string ConvertToString(byte[] data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            return ConvertToString(data, data.Length);
        }

        public static string ConvertToString(byte[] data, int count)
        {
            if (data == null)
            {
                return string.Empty;
            }
            return Encoding.GetEncoding(DefaultCodePage).GetString(data, 0, count);
        }

        public static string ConvertToStringExt(int flags, byte[] data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            if ((flags & 0x800) != 0)
            {
                return Encoding.UTF8.GetString(data, 0, data.Length);
            }
            return ConvertToString(data, data.Length);
        }

        public static string ConvertToStringExt(int flags, byte[] data, int count)
        {
            if (data == null)
            {
                return string.Empty;
            }
            if ((flags & 0x800) != 0)
            {
                return Encoding.UTF8.GetString(data, 0, count);
            }
            return ConvertToString(data, count);
        }

        public static int DefaultCodePage
        {
            get
            {
                return defaultCodePage;
            }
            set
            {
                defaultCodePage = value;
            }
        }
    }
}

