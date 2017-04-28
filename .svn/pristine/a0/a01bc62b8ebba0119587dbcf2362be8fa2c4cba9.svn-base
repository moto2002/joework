namespace ICSharpCode.SharpZipLib.Core
{
    using System;

    public abstract class WindowsPathUtils
    {
        internal WindowsPathUtils()
        {
        }

        public static string DropPathRoot(string path)
        {
            string str = path;
            if ((path == null) || (path.Length <= 0))
            {
                return str;
            }
            if ((path[0] != '\\') && (path[0] != '/'))
            {
                if ((path.Length <= 1) || (path[1] != ':'))
                {
                    return str;
                }
                int count = 2;
                if ((path.Length > 2) && ((path[2] == '\\') || (path[2] == '/')))
                {
                    count = 3;
                }
                return str.Remove(0, count);
            }
            if ((path.Length <= 1) || ((path[1] != '\\') && (path[1] != '/')))
            {
                return str;
            }
            int startIndex = 2;
            int num2 = 2;
        Label_0084:;
            if ((startIndex <= path.Length) && (((path[startIndex] != '\\') && (path[startIndex] != '/')) || (--num2 > 0)))
            {
                startIndex++;
                goto Label_0084;
            }
            startIndex++;
            if (startIndex < path.Length)
            {
                return path.Substring(startIndex);
            }
            return "";
        }
    }
}

