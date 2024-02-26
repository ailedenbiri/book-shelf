using System;
using System.Collections.Generic;
using System.Text;
namespace Exoa.Utils
{
    public static class TextExtenseions
    {
        private const string PasswordChars = "23456789abcdefghjkmnpqrstuvwxyABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string PasswordCharsForID = "23456789abcdefghjkmnpqrstuvwxy";
        private const string UnreservedCharacters = "-._~";
        private const string UnreservedURICharacters = "-._~:/?#[]@!$&'()*+,;=";
        public static string GenerateGUID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static int String2Int(string text)
        {
            if (text.IsNullOrEmpty())
            {
                return 1;
            }
            int num = 0;
            for (int i = 0; i < text.Length; i++)
            {
                num += (i + 1) * (int)text[i];
            }
            return num;
        }
        public static string[] Bigram(string text)
        {
            if (text.IsNullOrEmpty())
            {
                return new string[0];
            }
            if (text.Length <= 2)
            {
                return new string[]
                {
                    text
                };
            }
            List<string> list = new List<string>();
            for (int i = 0; i < text.Length - 1; i++)
            {
                list.Add(text.Substring(i, System.Math.Min(text.Length - i, 2)));
            }
            return list.ToArray();
        }
        public static string[] Trigram(string text)
        {
            if (text.IsNullOrEmpty())
            {
                return new string[0];
            }
            if (text.Length <= 3)
            {
                return new string[]
                {
                    text
                };
            }
            List<string> list = new List<string>();
            for (int i = 0; i < text.Length - 2; i++)
            {
                list.Add(text.Substring(i, System.Math.Min(text.Length - i, 3)));
            }
            return list.ToArray();
        }
        public static string UnescapeDataString(string stringToUnescape)
        {
            return Uri.UnescapeDataString(stringToUnescape);
        }
        public static string UnescapeHTMLString(string stringToEscape)
        {
            return stringToEscape.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&#39;", "'").Replace("&#039;", "'").Replace("&amp;", "&");
        }
        public static string EscapeHTMLString(string stringToEscape)
        {
            return stringToEscape.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&#39;").Replace("'", "&#039;");
        }
    }
}
