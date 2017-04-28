namespace ICSharpCode.SharpZipLib.BZip2
{
    using ICSharpCode.SharpZipLib.Core;
    using System;
    using System.IO;

    public static class BZip2
    {
        public static void Compress(Stream inStream, Stream outStream, bool isStreamOwner, int level)
        {
            if ((inStream == null) || (outStream == null))
            {
                throw new Exception("Null Stream");
            }
            try
            {
                using (BZip2OutputStream stream = new BZip2OutputStream(outStream, level))
                {
                    stream.IsStreamOwner = isStreamOwner;
                    StreamUtils.Copy(inStream, stream, new byte[0x1000]);
                }
            }
            finally
            {
                if (isStreamOwner)
                {
                    inStream.Close();
                }
            }
        }

        public static void Decompress(Stream inStream, Stream outStream, bool isStreamOwner)
        {
            if ((inStream == null) || (outStream == null))
            {
                throw new Exception("Null Stream");
            }
            try
            {
                using (BZip2InputStream stream = new BZip2InputStream(inStream))
                {
                    stream.IsStreamOwner = isStreamOwner;
                    StreamUtils.Copy(stream, outStream, new byte[0x1000]);
                }
            }
            finally
            {
                if (isStreamOwner)
                {
                    outStream.Close();
                }
            }
        }
    }
}

