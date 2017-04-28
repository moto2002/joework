namespace MsgPack.Serialization
{
    using MsgPack.Serialization.Reflection;
    using System;
    using System.Text;

    internal static class IdentifierUtility
    {
        public static string BuildMethodName(string operation, Type targetType, string targetMemberName)
        {
            return string.Join("_", new string[] { operation, EscapeTypeName(targetType), targetMemberName });
        }

        public static string EscapeTypeName(Type type)
        {
            string fullName = type.GetFullName();
            StringBuilder builder = new StringBuilder(fullName.Length);
            bool flag = false;
            foreach (char ch in fullName)
            {
                switch (ch)
                {
                    case ' ':
                    {
                        flag = false;
                        continue;
                    }
                    case '&':
                    {
                        if (flag)
                        {
                            builder.Append('_');
                        }
                        flag = false;
                        builder.Append("Reference");
                        continue;
                    }
                    case '*':
                    {
                        if (!flag)
                        {
                            builder.Append("Pointer");
                        }
                        continue;
                    }
                    case ',':
                    {
                        flag = false;
                        builder.Append('_');
                        continue;
                    }
                    case '.':
                    case '`':
                    {
                        flag = false;
                        builder.Append('_');
                        continue;
                    }
                    case '[':
                    {
                        flag = true;
                        continue;
                    }
                    case ']':
                    {
                        if (!flag)
                        {
                            break;
                        }
                        builder.Append("Array");
                        flag = false;
                        continue;
                    }
                    default:
                        goto Label_0123;
                }
                builder.Append('_');
                continue;
            Label_0123:
                if (flag)
                {
                    builder.Append('_');
                }
                flag = false;
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}

