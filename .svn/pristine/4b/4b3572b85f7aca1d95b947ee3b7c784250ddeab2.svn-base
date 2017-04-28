namespace Mono.Xml
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Text;

    internal class MiniParser
    {
        protected int col;
        protected static string[] errors = new string[] { "Expected element", "Invalid character in tag", "No '='", "Invalid character entity", "Invalid attr value", "Empty tag", "No end tag", "Bad entity ref" };
        private static readonly int INPUT_RANGE = 13;
        protected int line;
        protected bool splitCData = false;
        private static readonly ushort[] tbl = new ushort[] { 
            0x901, 0xa900, 0xf880, 0x2880, 0x1880, 0x3880, 0x4880, 0x5880, 0x6880, 0x8880, 0x9880, 0xb880, 0x7880, 0x881, 0x2902, 0x1885, 
            0x3903, 0x4881, 0x5881, 0x6881, 0x8910, 0x9881, 0xa881, 0xb881, 0x7881, 0xfa04, 0x1100, 0xa902, 0xfb02, 0x881, 0x2881, 0x3881, 
            0x4881, 0x5881, 0x6881, 0x8881, 0x9881, 0xb881, 0x7881, 0x3905, 0x903, 0x2903, 0x1903, 0x4903, 0x5903, 0x6903, 0x8903, 0x9903, 
            0xa903, 0xb903, 0x7903, 0xf903, 0xfb04, 0x2206, 0x1207, 0xa208, 0x881, 0x3881, 0x4881, 0x5881, 0x6881, 0x8881, 0x9881, 0xb881, 
            0x7881, 0x1900, 0x903, 0x2903, 0x3903, 0x4903, 0x5903, 0x6903, 0x8903, 0x9903, 0xa903, 0xb903, 0x7903, 0xf903, 0x1900, 0x881, 
            0x2881, 0x3881, 0x4881, 0x5881, 0x6881, 0x8881, 0x9881, 0xa881, 0xb881, 0x7881, 0xf881, 0xa01, 0x5d0a, 0x2c0a, 0x1c0a, 0x3c0a, 
            0x4c0a, 0x6c0a, 0x8c0a, 0x9c0a, 0xab07, 0xbc0a, 0x7c0a, 0xfc0a, 0xfb09, 0x2006, 0x1007, 0xa908, 0x881, 0x3881, 0x4881, 0x5881, 
            0x6881, 0x8881, 0x9881, 0xb881, 0x7881, 0xfb09, 0x430b, 0xa90c, 0x882, 0x2882, 0x1882, 0x3882, 0x5882, 0x6882, 0x8882, 0x9882, 
            0xb882, 0x7882, 0x90d, 0x5d0a, 0x2c0a, 0x1c0a, 0x3c0a, 0x4c0a, 0x6c0a, 0x8c0a, 0x9c0a, 0xac0a, 0xbc0a, 0x7c0a, 0xfc0a, 0x690e, 
            0x790f, 0xa90b, 0x884, 0x2884, 0x1884, 0x3884, 0x4884, 0x5884, 0x8884, 0x9884, 0xb884, 0xf884, 0x430b, 0xa90c, 0x882, 0x2882, 
            0x1882, 0x3882, 0x5882, 0x6882, 0x8882, 0x9882, 0xb882, 0x7882, 0xf882, 0x2502, 0x8910, 0x886, 0x1886, 0x3886, 0x4886, 0x5886, 
            0x6886, 0x9886, 0xa886, 0xb886, 0x7886, 0xf886, 0x6411, 0x5d0e, 0xb0e, 0x2b0e, 0x1b0e, 0x3b0e, 0x4b0e, 0x8b0e, 0x9b0e, 0xab0e, 
            0xbb0e, 0x7b0e, 0xfb0e, 0x7411, 0x5d0f, 0xb0f, 0x2b0f, 0x1b0f, 0x3b0f, 0x4b0f, 0x6b0f, 0x8b0f, 0x9b0f, 0xab0f, 0xbb0f, 0xfb0f, 
            0x9612, 0x1900, 0x613, 0x2613, 0x3613, 0x4613, 0x5613, 0x6613, 0x8613, 0xa613, 0xb613, 0x7613, 0xf613, 0x2006, 0x1007, 0xa911, 
            0xfb09, 0x881, 0x3881, 0x4881, 0x5881, 0x6881, 0x8881, 0x9881, 0xb881, 0x7881, 0xb70a, 0xc12, 0x2c12, 0x1c12, 0x3c12, 0x4c12, 
            0x5c12, 0x6c12, 0x8c12, 0x9c12, 0xac12, 0x7c12, 0xfc12, 0xc13, 0x2c13, 0x1c13, 0x3c13, 0x4c13, 0x5c13, 0x6c13, 0x8c13, 0x9c13, 
            0xac13, 0xbc13, 0x7c13, 0xfc13, 0xffff, 0xffff
         };
        protected int[] twoCharBuff = new int[2];

        public MiniParser()
        {
            this.Reset();
        }

        protected void FatalErr(string descr)
        {
            throw new XMLError(descr, this.line, this.col);
        }

        public void Parse(IReader reader, IHandler handler)
        {
            int num8;
            bool flag4;
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (handler == null)
            {
                handler = new HandlerAdapter();
            }
            AttrListImpl attrs = new AttrListImpl();
            string name = null;
            Stack stack = new Stack();
            string str2 = null;
            this.line = 1;
            this.col = 0;
            int num = 0;
            int state = 0;
            StringBuilder sb = new StringBuilder();
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            int num3 = 0;
            handler.OnStartParsing(this);
            goto Label_0972;
        Label_01ED:
            handler.OnStartElement(str2, attrs);
            if (num != 0x2f)
            {
                stack.Push(str2);
            }
            else
            {
                handler.OnEndElement(str2);
            }
            attrs.Clear();
            goto Label_0972;
        Label_04F4:
            sb.Append((char) num);
            goto Label_0972;
        Label_094D:
            this.col = num8;
        Label_0972:
            flag4 = true;
            this.col++;
            num = reader.Read();
            if (num == -1)
            {
                if (state != 0)
                {
                    this.FatalErr("Unexpected EOF");
                }
                handler.OnEndParsing(this);
            }
            else
            {
                int charCode = "<>/?=&'\"![ ]\t\r\n".IndexOf((char) num) & 15;
                switch (charCode)
                {
                    case 13:
                        goto Label_0972;

                    case 12:
                        charCode = 10;
                        break;

                    case 14:
                        this.col = 0;
                        this.line++;
                        charCode = 10;
                        break;
                }
                int num5 = Xlat(charCode, state);
                state = num5 & 0xff;
                if ((num != 10) || ((state != 14) && (state != 15)))
                {
                    int num6;
                    num5 = num5 >> 8;
                    if (state >= 0x80)
                    {
                        if (state == 0xff)
                        {
                            this.FatalErr("State dispatch error.");
                        }
                        else
                        {
                            this.FatalErr(errors[state ^ 0x80]);
                        }
                    }
                    switch (num5)
                    {
                        case 0:
                            goto Label_01ED;

                        case 1:
                        {
                            str2 = sb.ToString();
                            sb = new StringBuilder();
                            string str3 = null;
                            if ((stack.Count == 0) || (str2 != (str3 = stack.Pop() as string)))
                            {
                                if (str3 != null)
                                {
                                    this.FatalErr(string.Format("Expected end tag '{0}' but found '{1}'", str2, str3));
                                }
                                else
                                {
                                    this.FatalErr("Tag stack underflow");
                                }
                            }
                            handler.OnEndElement(str2);
                            goto Label_0972;
                        }
                        case 2:
                            str2 = sb.ToString();
                            sb = new StringBuilder();
                            switch (num)
                            {
                                case 0x2f:
                                case 0x3e:
                                    goto Label_01ED;
                            }
                            goto Label_0972;

                        case 3:
                            name = sb.ToString();
                            sb = new StringBuilder();
                            goto Label_0972;

                        case 4:
                            if (name == null)
                            {
                                this.FatalErr("Internal error.");
                            }
                            attrs.Add(name, sb.ToString());
                            sb = new StringBuilder();
                            name = null;
                            goto Label_0972;

                        case 5:
                            handler.OnChars(sb.ToString());
                            sb = new StringBuilder();
                            goto Label_0972;

                        case 6:
                        {
                            string str4 = "CDATA[";
                            flag2 = false;
                            flag3 = false;
                            if (num != 0x2d)
                            {
                                if (num != 0x5b)
                                {
                                    flag3 = true;
                                    num3 = 0;
                                }
                                else
                                {
                                    for (num6 = 0; num6 < str4.Length; num6++)
                                    {
                                        if (reader.Read() != str4[num6])
                                        {
                                            this.col += num6 + 1;
                                            break;
                                        }
                                    }
                                    this.col += str4.Length;
                                    flag = true;
                                }
                            }
                            else
                            {
                                if (reader.Read() != 0x2d)
                                {
                                    this.FatalErr("Invalid comment");
                                }
                                this.col++;
                                flag2 = true;
                                this.twoCharBuff[0] = -1;
                                this.twoCharBuff[1] = -1;
                            }
                            goto Label_0972;
                        }
                        case 7:
                        {
                            int num7 = 0;
                            num = 0x5d;
                            while (num == 0x5d)
                            {
                                num = reader.Read();
                                num7++;
                            }
                            if (num != 0x3e)
                            {
                                for (num6 = 0; num6 < num7; num6++)
                                {
                                    sb.Append(']');
                                }
                                sb.Append((char) num);
                                state = 0x12;
                            }
                            else
                            {
                                for (num6 = 0; num6 < (num7 - 2); num6++)
                                {
                                    sb.Append(']');
                                }
                                flag = false;
                            }
                            this.col += num7;
                            goto Label_0972;
                        }
                        case 8:
                            this.FatalErr(string.Format("Error {0}", state));
                            goto Label_0972;

                        case 9:
                            goto Label_0972;

                        case 10:
                            sb = new StringBuilder();
                            if (num == 60)
                            {
                                goto Label_0972;
                            }
                            goto Label_04F4;

                        case 11:
                            goto Label_04F4;

                        case 12:
                            if (flag2)
                            {
                                if (((num == 0x3e) && (this.twoCharBuff[0] == 0x2d)) && (this.twoCharBuff[1] == 0x2d))
                                {
                                    flag2 = false;
                                    state = 0;
                                }
                                else
                                {
                                    this.twoCharBuff[0] = this.twoCharBuff[1];
                                    this.twoCharBuff[1] = num;
                                }
                            }
                            else if (flag3)
                            {
                                switch (num)
                                {
                                    case 60:
                                    case 0x3e:
                                        num3 ^= 1;
                                        break;
                                }
                                if ((num == 0x3e) && (num3 != 0))
                                {
                                    flag3 = false;
                                    state = 0;
                                }
                            }
                            else
                            {
                                if ((this.splitCData && (sb.Length > 0)) && flag)
                                {
                                    handler.OnChars(sb.ToString());
                                    sb = new StringBuilder();
                                }
                                flag = false;
                                sb.Append((char) num);
                            }
                            goto Label_0972;

                        case 13:
                        {
                            num = reader.Read();
                            num8 = this.col + 1;
                            if (num != 0x23)
                            {
                                string str5 = "aglmopqstu";
                                string str6 = "&'\"><";
                                int num13 = 0;
                                int num14 = 15;
                                int num15 = 0;
                                int length = sb.Length;
                            Label_0897:
                                flag4 = true;
                                if (num13 != 15)
                                {
                                    num13 = str5.IndexOf((char) num) & 15;
                                }
                                if (num13 == 15)
                                {
                                    this.FatalErr(errors[7]);
                                }
                                sb.Append((char) num);
                                int num17 = "Ｕ㾏侏ཟｸ⊙ｏ"[num13];
                                int num18 = (num17 >> 4) & 15;
                                int num19 = num17 & 15;
                                int num20 = num17 >> 12;
                                int num21 = (num17 >> 8) & 15;
                                num = reader.Read();
                                num8++;
                                num13 = 15;
                                if ((num18 != 15) && (num == str5[num18]))
                                {
                                    if (num20 < 14)
                                    {
                                        num14 = num20;
                                    }
                                    num15 = 12;
                                }
                                else if ((num19 != 15) && (num == str5[num19]))
                                {
                                    if (num21 < 14)
                                    {
                                        num14 = num21;
                                    }
                                    num15 = 8;
                                }
                                else if (num == 0x3b)
                                {
                                    if (((num14 != 15) && (num15 != 0)) && (((num17 >> num15) & 15) == 14))
                                    {
                                        int len = (num8 - this.col) - 1;
                                        if (((len > 0) && (len < 5)) && (((StrEquals("amp", sb, length, len) || StrEquals("apos", sb, length, len)) || (StrEquals("quot", sb, length, len) || StrEquals("lt", sb, length, len))) || StrEquals("gt", sb, length, len)))
                                        {
                                            sb.Length = length;
                                            sb.Append(str6[num14]);
                                        }
                                        else
                                        {
                                            this.FatalErr(errors[7]);
                                        }
                                        goto Label_094D;
                                    }
                                    goto Label_0897;
                                }
                                num13 = 0;
                                goto Label_0897;
                            }
                            int num9 = 10;
                            int num10 = 0;
                            int num11 = 0;
                            num = reader.Read();
                            num8++;
                            if (num == 120)
                            {
                                num = reader.Read();
                                num8++;
                                num9 = 0x10;
                            }
                            NumberStyles style = (num9 == 0x10) ? NumberStyles.HexNumber : NumberStyles.Integer;
                            while (true)
                            {
                                flag4 = true;
                                int num12 = -1;
                                if (char.IsNumber((char) num) || ("abcdef".IndexOf(char.ToLower((char) num)) != -1))
                                {
                                    try
                                    {
                                        num12 = int.Parse(new string((char) num, 1), style);
                                    }
                                    catch (FormatException)
                                    {
                                        num12 = -1;
                                    }
                                }
                                if (num12 == -1)
                                {
                                    if ((num == 0x3b) && (num11 > 0))
                                    {
                                        sb.Append((char) num10);
                                    }
                                    else
                                    {
                                        this.FatalErr("Bad char ref");
                                    }
                                    goto Label_094D;
                                }
                                num10 *= num9;
                                num10 += num12;
                                num11++;
                                num = reader.Read();
                                num8++;
                            }
                        }
                    }
                    this.FatalErr(string.Format("Unexpected action code - {0}.", num5));
                }
                goto Label_0972;
            }
        }

        public void Reset()
        {
            this.line = 0;
            this.col = 0;
        }

        protected static bool StrEquals(string str, StringBuilder sb, int sbStart, int len)
        {
            if (len != str.Length)
            {
                return false;
            }
            for (int i = 0; i < len; i++)
            {
                if (str[i] != sb[sbStart + i])
                {
                    return false;
                }
            }
            return true;
        }

        protected static int Xlat(int charCode, int state)
        {
            int index = state * INPUT_RANGE;
            int num2 = Math.Min(tbl.Length - index, INPUT_RANGE);
            while (--num2 >= 0)
            {
                ushort num3 = tbl[index];
                if (charCode == (num3 >> 12))
                {
                    return (num3 & 0xfff);
                }
                index++;
            }
            return 0xfff;
        }

        private enum ActionCode : byte
        {
            ACC_CDATA = 12,
            ACC_CHARS_STATE_CHANGE = 11,
            END_CDATA = 7,
            END_ELEM = 1,
            END_NAME = 2,
            ERROR = 8,
            FLUSH_CHARS_STATE_CHANGE = 10,
            PROC_CHAR_REF = 13,
            SEND_CHARS = 5,
            SET_ATTR_NAME = 3,
            SET_ATTR_VAL = 4,
            START_CDATA = 6,
            START_ELEM = 0,
            STATE_CHANGE = 9,
            UNKNOWN = 15
        }

        public class AttrListImpl : MiniParser.IMutableAttrList, MiniParser.IAttrList
        {
            protected ArrayList names;
            protected ArrayList values;

            public AttrListImpl() : this(0)
            {
            }

            public AttrListImpl(MiniParser.IAttrList attrs) : this((attrs != null) ? attrs.Length : 0)
            {
                if (attrs != null)
                {
                    this.CopyFrom(attrs);
                }
            }

            public AttrListImpl(int initialCapacity)
            {
                if (initialCapacity <= 0)
                {
                    this.names = new ArrayList();
                    this.values = new ArrayList();
                }
                else
                {
                    this.names = new ArrayList(initialCapacity);
                    this.values = new ArrayList(initialCapacity);
                }
            }

            public void Add(string name, string value)
            {
                this.names.Add(name);
                this.values.Add(value);
            }

            public void ChangeValue(string name, string newValue)
            {
                int index = this.names.IndexOf(name);
                if ((index >= 0) && (index < this.Length))
                {
                    this.values[index] = newValue;
                }
            }

            public void Clear()
            {
                this.names.Clear();
                this.values.Clear();
            }

            public void CopyFrom(MiniParser.IAttrList attrs)
            {
                if ((attrs != null) && (this == attrs))
                {
                    this.Clear();
                    int length = attrs.Length;
                    for (int i = 0; i < length; i++)
                    {
                        this.Add(attrs.GetName(i), attrs.GetValue(i));
                    }
                }
            }

            public string GetName(int i)
            {
                string str = null;
                if ((i >= 0) && (i < this.Length))
                {
                    str = this.names[i] as string;
                }
                return str;
            }

            public string GetValue(int i)
            {
                string str = null;
                if ((i >= 0) && (i < this.Length))
                {
                    str = this.values[i] as string;
                }
                return str;
            }

            public string GetValue(string name)
            {
                return this.GetValue(this.names.IndexOf(name));
            }

            public void Remove(int i)
            {
                if (i >= 0)
                {
                    this.names.RemoveAt(i);
                    this.values.RemoveAt(i);
                }
            }

            public void Remove(string name)
            {
                this.Remove(this.names.IndexOf(name));
            }

            public bool IsEmpty
            {
                get
                {
                    return (this.Length != 0);
                }
            }

            public int Length
            {
                get
                {
                    return this.names.Count;
                }
            }

            public string[] Names
            {
                get
                {
                    return (this.names.ToArray(typeof(string)) as string[]);
                }
            }

            public string[] Values
            {
                get
                {
                    return (this.values.ToArray(typeof(string)) as string[]);
                }
            }
        }

        private enum CharKind : byte
        {
            AMP = 5,
            BANG = 8,
            CHARS = 15,
            CR = 13,
            DQUOTE = 7,
            EOL = 14,
            EQ = 4,
            LEFT_BR = 0,
            LEFT_SQBR = 9,
            PI_MARK = 3,
            RIGHT_BR = 1,
            RIGHT_SQBR = 11,
            SLASH = 2,
            SPACE = 10,
            SQUOTE = 6,
            TAB = 12,
            UNKNOWN = 0x1f
        }

        public class HandlerAdapter : MiniParser.IHandler
        {
            public void OnChars(string ch)
            {
            }

            public void OnEndElement(string name)
            {
            }

            public void OnEndParsing(MiniParser parser)
            {
            }

            public void OnStartElement(string name, MiniParser.IAttrList attrs)
            {
            }

            public void OnStartParsing(MiniParser parser)
            {
            }
        }

        public interface IAttrList
        {
            void ChangeValue(string name, string newValue);
            string GetName(int i);
            string GetValue(int i);
            string GetValue(string name);

            bool IsEmpty { get; }

            int Length { get; }

            string[] Names { get; }

            string[] Values { get; }
        }

        public interface IHandler
        {
            void OnChars(string ch);
            void OnEndElement(string name);
            void OnEndParsing(MiniParser parser);
            void OnStartElement(string name, MiniParser.IAttrList attrs);
            void OnStartParsing(MiniParser parser);
        }

        public interface IMutableAttrList : MiniParser.IAttrList
        {
            void Add(string name, string value);
            void Clear();
            void CopyFrom(MiniParser.IAttrList attrs);
            void Remove(int i);
            void Remove(string name);
        }

        public interface IReader
        {
            int Read();
        }

        public class XMLError : Exception
        {
            protected int column;
            protected string descr;
            protected int line;

            public XMLError() : this("Unknown")
            {
            }

            public XMLError(string descr) : this(descr, -1, -1)
            {
            }

            public XMLError(string descr, int line, int column) : base(descr)
            {
                this.descr = descr;
                this.line = line;
                this.column = column;
            }

            public override string ToString()
            {
                return string.Format("{0} @ (line = {1}, col = {2})", this.descr, this.line, this.column);
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
}

