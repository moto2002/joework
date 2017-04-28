namespace ICSharpCode.SharpZipLib.Core
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Text.RegularExpressions;

    public class NameFilter : IScanFilter
    {
        private ArrayList exclusions_;
        private string filter_;
        private ArrayList inclusions_;

        public NameFilter(string filter)
        {
            this.filter_ = filter;
            this.inclusions_ = new ArrayList();
            this.exclusions_ = new ArrayList();
            this.Compile();
        }

        private void Compile()
        {
            if (this.filter_ != null)
            {
                string[] strArray = SplitQuoted(this.filter_);
                for (int i = 0; i < strArray.Length; i++)
                {
                    if ((strArray[i] != null) && (strArray[i].Length > 0))
                    {
                        string str;
                        bool flag = strArray[i][0] != '-';
                        if (strArray[i][0] == '+')
                        {
                            str = strArray[i].Substring(1, strArray[i].Length - 1);
                        }
                        else if (strArray[i][0] == '-')
                        {
                            str = strArray[i].Substring(1, strArray[i].Length - 1);
                        }
                        else
                        {
                            str = strArray[i];
                        }
                        if (flag)
                        {
                            this.inclusions_.Add(new Regex(str, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase));
                        }
                        else
                        {
                            this.exclusions_.Add(new Regex(str, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase));
                        }
                    }
                }
            }
        }

        public bool IsExcluded(string name)
        {
            foreach (Regex regex in this.exclusions_)
            {
                if (regex.IsMatch(name))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsIncluded(string name)
        {
            if (this.inclusions_.Count == 0)
            {
                return true;
            }
            foreach (Regex regex in this.inclusions_)
            {
                if (regex.IsMatch(name))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsMatch(string name)
        {
            return (this.IsIncluded(name) && !this.IsExcluded(name));
        }

        public static bool IsValidExpression(string expression)
        {
            bool flag = true;
            try
            {
                Regex regex = new Regex(expression, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }
            catch (ArgumentException)
            {
                flag = false;
            }
            return flag;
        }

        public static bool IsValidFilterExpression(string toTest)
        {
            if (toTest == null)
            {
                throw new ArgumentNullException("toTest");
            }
            bool flag = true;
            try
            {
                string[] strArray = SplitQuoted(toTest);
                for (int i = 0; i < strArray.Length; i++)
                {
                    if ((strArray[i] != null) && (strArray[i].Length > 0))
                    {
                        string str;
                        if (strArray[i][0] == '+')
                        {
                            str = strArray[i].Substring(1, strArray[i].Length - 1);
                        }
                        else if (strArray[i][0] == '-')
                        {
                            str = strArray[i].Substring(1, strArray[i].Length - 1);
                        }
                        else
                        {
                            str = strArray[i];
                        }
                        Regex regex = new Regex(str, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    }
                }
            }
            catch (ArgumentException)
            {
                flag = false;
            }
            return flag;
        }

        public static string[] SplitQuoted(string original)
        {
            char ch = '\\';
            char[] array = new char[] { ';' };
            ArrayList list = new ArrayList();
            if ((original != null) && (original.Length > 0))
            {
                int num = -1;
                StringBuilder builder = new StringBuilder();
                while (num < original.Length)
                {
                    num++;
                    if (num >= original.Length)
                    {
                        list.Add(builder.ToString());
                    }
                    else if (original[num] == ch)
                    {
                        num++;
                        if (num >= original.Length)
                        {
                            throw new ArgumentException("Missing terminating escape character", "original");
                        }
                        if (Array.IndexOf<char>(array, original[num]) < 0)
                        {
                            builder.Append(ch);
                        }
                        builder.Append(original[num]);
                    }
                    else if (Array.IndexOf<char>(array, original[num]) >= 0)
                    {
                        list.Add(builder.ToString());
                        builder.Length = 0;
                    }
                    else
                    {
                        builder.Append(original[num]);
                    }
                }
            }
            return (string[]) list.ToArray(typeof(string));
        }

        public override string ToString()
        {
            return this.filter_;
        }
    }
}

