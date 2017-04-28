namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib.Core;
    using System;
    using System.IO;
    using System.Text;

    public class WindowsNameTransform : INameTransform
    {
        private string _baseDirectory;
        private char _replacementChar;
        private bool _trimIncomingPaths;
        private static readonly char[] InvalidEntryChars;
        private const int MaxPath = 260;

        static WindowsNameTransform()
        {
            char[] invalidPathChars = Path.GetInvalidPathChars();
            int num = invalidPathChars.Length + 3;
            InvalidEntryChars = new char[num];
            Array.Copy(invalidPathChars, 0, InvalidEntryChars, 0, invalidPathChars.Length);
            InvalidEntryChars[num - 1] = '*';
            InvalidEntryChars[num - 2] = '?';
            InvalidEntryChars[num - 3] = ':';
        }

        public WindowsNameTransform()
        {
            this._replacementChar = '_';
        }

        public WindowsNameTransform(string baseDirectory)
        {
            this._replacementChar = '_';
            if (baseDirectory == null)
            {
                throw new ArgumentNullException("baseDirectory", "Directory name is invalid");
            }
            this.BaseDirectory = baseDirectory;
        }

        public static bool IsValidName(string name)
        {
            return (((name != null) && (name.Length <= 260)) && (string.Compare(name, MakeValidName(name, '_')) == 0));
        }

        public static string MakeValidName(string name, char replacement)
        {
            int num;
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            name = WindowsPathUtils.DropPathRoot(name.Replace("/", @"\"));
            while ((name.Length > 0) && (name[0] == '\\'))
            {
                name = name.Remove(0, 1);
            }
            while ((name.Length > 0) && (name[name.Length - 1] == '\\'))
            {
                name = name.Remove(name.Length - 1, 1);
            }
            for (num = name.IndexOf(@"\\"); num >= 0; num = name.IndexOf(@"\\"))
            {
                name = name.Remove(num, 1);
            }
            num = name.IndexOfAny(InvalidEntryChars);
            if (num >= 0)
            {
                StringBuilder builder = new StringBuilder(name);
                while (num >= 0)
                {
                    builder[num] = replacement;
                    if (num >= name.Length)
                    {
                        num = -1;
                    }
                    else
                    {
                        num = name.IndexOfAny(InvalidEntryChars, num + 1);
                    }
                }
                name = builder.ToString();
            }
            if (name.Length > 260)
            {
                throw new PathTooLongException();
            }
            return name;
        }

        public string TransformDirectory(string name)
        {
            name = this.TransformFile(name);
            if (name.Length <= 0)
            {
                throw new ZipException("Cannot have an empty directory name");
            }
            while (name.EndsWith(@"\"))
            {
                name = name.Remove(name.Length - 1, 1);
            }
            return name;
        }

        public string TransformFile(string name)
        {
            if (name != null)
            {
                name = MakeValidName(name, this._replacementChar);
                if (this._trimIncomingPaths)
                {
                    name = Path.GetFileName(name);
                }
                if (this._baseDirectory != null)
                {
                    name = Path.Combine(this._baseDirectory, name);
                }
                return name;
            }
            name = string.Empty;
            return name;
        }

        public string BaseDirectory
        {
            get
            {
                return this._baseDirectory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._baseDirectory = Path.GetFullPath(value);
            }
        }

        public char Replacement
        {
            get
            {
                return this._replacementChar;
            }
            set
            {
                for (int i = 0; i < InvalidEntryChars.Length; i++)
                {
                    if (InvalidEntryChars[i] == value)
                    {
                        throw new ArgumentException("invalid path character");
                    }
                }
                if ((value == '\\') || (value == '/'))
                {
                    throw new ArgumentException("invalid replacement character");
                }
                this._replacementChar = value;
            }
        }

        public bool TrimIncomingPaths
        {
            get
            {
                return this._trimIncomingPaths;
            }
            set
            {
                this._trimIncomingPaths = value;
            }
        }
    }
}

