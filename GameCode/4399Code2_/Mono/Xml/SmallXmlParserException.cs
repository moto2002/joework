namespace Mono.Xml
{
    using System;

    internal class SmallXmlParserException : SystemException
    {
        private int column;
        private int line;

        public SmallXmlParserException(string msg, int line, int column) : base(string.Format("{0}. At ({1},{2})", msg, line, column))
        {
            this.line = line;
            this.column = column;
        }

        public int Column
        {
            get
            {
                return this.column;
            }
        }

        public int Line
        {
            get
            {
                return this.line;
            }
        }
    }
}

