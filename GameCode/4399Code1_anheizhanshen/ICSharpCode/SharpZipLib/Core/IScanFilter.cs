namespace ICSharpCode.SharpZipLib.Core
{
    using System;

    public interface IScanFilter
    {
        bool IsMatch(string name);
    }
}

