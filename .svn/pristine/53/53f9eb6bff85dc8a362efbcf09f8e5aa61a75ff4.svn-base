namespace ICSharpCode.SharpZipLib.Core
{
    using System;
    using System.IO;

    public class PathFilter : IScanFilter
    {
        private NameFilter nameFilter_;

        public PathFilter(string filter)
        {
            this.nameFilter_ = new NameFilter(filter);
        }

        public virtual bool IsMatch(string name)
        {
            bool flag = false;
            if (name != null)
            {
                string str = (name.Length > 0) ? Path.GetFullPath(name) : "";
                flag = this.nameFilter_.IsMatch(str);
            }
            return flag;
        }
    }
}

