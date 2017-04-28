namespace MsgPack
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Text.RegularExpressions;

    internal static class Validation
    {
        private static readonly Regex _unicodeTR15Annex7IdentifierPattern = new Regex(@"[\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}][\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*", RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        private static bool IsPrintable(UnicodeCategory category)
        {
            switch (category)
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.OtherLetter:
                case UnicodeCategory.NonSpacingMark:
                case UnicodeCategory.EnclosingMark:
                case UnicodeCategory.DecimalDigitNumber:
                case UnicodeCategory.LetterNumber:
                case UnicodeCategory.OtherNumber:
                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.DashPunctuation:
                case UnicodeCategory.OpenPunctuation:
                case UnicodeCategory.ClosePunctuation:
                case UnicodeCategory.InitialQuotePunctuation:
                case UnicodeCategory.FinalQuotePunctuation:
                case UnicodeCategory.OtherPunctuation:
                case UnicodeCategory.MathSymbol:
                case UnicodeCategory.CurrencySymbol:
                case UnicodeCategory.OtherSymbol:
                    return false;
            }
            return true;
        }

        public static void ValidateBuffer<T>(T[] byteArray, int offset, long length, string nameOfByteArray, string nameOfLength, bool validateBufferSize)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException(nameOfByteArray);
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", string.Format(CultureInfo.CurrentCulture, "'{0}' is negative.", new object[] { "offset" }));
            }
            if (length < 0L)
            {
                throw new ArgumentOutOfRangeException("nameOfLength", string.Format(CultureInfo.CurrentCulture, "'{0}' is negative.", new object[] { nameOfLength }));
            }
            if (validateBufferSize && (byteArray.Length < (offset + length)))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "'{0}' is too small for specified '{1}' and '{2}'. Length of '{0}' is {3}, '{1}' is {4}, '{2}' is {5}.", new object[] { nameOfByteArray, "offset", nameOfLength, byteArray.Length, offset, length }));
            }
        }

        public static void ValidateIsNotNullNorEmpty(string value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            if (value.Length == 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "'{0}' cannot be empty.", new object[] { parameterName }), parameterName);
            }
        }

        public static void ValidateMethodName(string methodName, string parameterName)
        {
            ValidateIsNotNullNorEmpty(methodName, parameterName);
            MatchCollection matchs = _unicodeTR15Annex7IdentifierPattern.Matches(methodName);
            if ((((matchs.Count != 1) || !matchs[0].Success) || (matchs[0].Index != 0)) || (matchs[0].Length != methodName.Length))
            {
                int index = 0;
                int num2 = 0;
                for (int i = 0; i < matchs.Count; i++)
                {
                    if (matchs[i].Index == num2)
                    {
                        num2 += matchs[i].Length;
                    }
                    else
                    {
                        index = num2;
                        break;
                    }
                }
                Contract.Assert(index >= 0);
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(methodName, index);
                if (IsPrintable(unicodeCategory))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, @"Char at {0}('{1}'\u{2}[{3}] is not used for method name.", new object[] { index, methodName[index], (ushort) methodName[index], unicodeCategory }));
                }
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, @"Char at {0}(\u{1}[{2}] is not used for method name.", new object[] { index, (ushort) methodName[index], unicodeCategory }));
            }
        }
    }
}

