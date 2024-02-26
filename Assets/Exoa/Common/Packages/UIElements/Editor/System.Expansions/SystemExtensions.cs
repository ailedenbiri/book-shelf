using Exoa.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
public static class SystemExtensions
{
    public static bool EqualTo(this float a, float b, float range = 0f)
    {
        return System.Math.Abs(a - b) <= range;
    }
    public static bool EqualTo(this double a, double b, double range = 0.0)
    {
        return System.Math.Abs(a - b) <= range;
    }
    public static bool EqualTo(this decimal a, decimal b, [DecimalConstant(0, 0, 0u, 0u, 0u)] decimal range = default(decimal))
    {
        return System.Math.Abs(a - b) <= range;
    }
    public static short Limit(this short value, short min, short max)
    {
        if (value < min)
        {
            value = min;
        }
        else
        {
            if (value > max)
            {
                value = max;
            }
        }
        return value;
    }
    public static short LimitLow(this short value, short min)
    {
        if (value < min)
        {
            value = min;
        }
        return value;
    }
    public static short LimitHigh(this short value, short max)
    {
        if (value > max)
        {
            value = max;
        }
        return value;
    }
    public static int Limit(this int value, int min, int max)
    {
        if (value < min)
        {
            value = min;
        }
        else
        {
            if (value > max)
            {
                value = max;
            }
        }
        return value;
    }
    public static int LimitLow(this int value, int min)
    {
        if (value < min)
        {
            value = min;
        }
        return value;
    }
    public static int LimitHigh(this int value, int max)
    {
        if (value > max)
        {
            value = max;
        }
        return value;
    }
    public static float Limit(this float value, float min, float max)
    {
        if (value < min)
        {
            value = min;
        }
        else
        {
            if (value > max)
            {
                value = max;
            }
        }
        return value;
    }
    public static float LimitLow(this float value, float min)
    {
        if (value < min)
        {
            value = min;
        }
        return value;
    }
    public static float LimitHigh(this float value, float max)
    {
        if (value > max)
        {
            value = max;
        }
        return value;
    }
    public static long Limit(this long value, long min, long max)
    {
        if (value < min)
        {
            value = min;
        }
        else
        {
            if (value > max)
            {
                value = max;
            }
        }
        return value;
    }
    public static long LimitLow(this long value, long min)
    {
        if (value < min)
        {
            value = min;
        }
        return value;
    }
    public static long LimitHigh(this long value, long max)
    {
        if (value > max)
        {
            value = max;
        }
        return value;
    }
    public static double Limit(this double value, double min, double max)
    {
        if (value < min)
        {
            value = min;
        }
        else
        {
            if (value > max)
            {
                value = max;
            }
        }
        return value;
    }
    public static double LimitLow(this double value, double min)
    {
        if (value < min)
        {
            value = min;
        }
        return value;
    }
    public static double LimitHigh(this double value, double max)
    {
        if (value > max)
        {
            value = max;
        }
        return value;
    }
    public static decimal Limit(this decimal value, decimal min, decimal max)
    {
        if (value < min)
        {
            value = min;
        }
        else
        {
            if (value > max)
            {
                value = max;
            }
        }
        return value;
    }
    public static decimal LimitLow(this decimal value, decimal min)
    {
        if (value < min)
        {
            value = min;
        }
        return value;
    }
    public static decimal LimitHigh(this decimal value, decimal max)
    {
        if (value > max)
        {
            value = max;
        }
        return value;
    }
    public static string ToTitleCase(this string value)
    {
        if (value.IsNullOrEmpty())
        {
            return "";
        }
        return char.ToUpper(value[0]).ToString() + value.Substring(1).ToLower();
    }
    public static string Replaces(this string value, string newValue, params string[] oldValues)
    {
        if (newValue == null || oldValues == null)
        {
            return value;
        }
        for (int i = 0; i < oldValues.Length; i++)
        {
            string text = oldValues[i];
            if (!text.IsNullOrEmpty())
            {
                value = value.Replace(text, newValue);
            }
        }
        return value;
    }
    public static string TrimStart(this string target, params string[] trimStrings)
    {
        if (trimStrings == null || trimStrings.Length == 0)
        {
            return target;
        }
        string text = target;
        for (int i = 0; i < trimStrings.Length; i++)
        {
            string text2 = trimStrings[i];
            while (text.StartsWith(text2, StringComparison.Ordinal))
            {
                text = text.Substring(text2.Length);
            }
        }
        return text;
    }
    public static string TrimEnd(this string target, params string[] trimStrings)
    {
        if (trimStrings == null || trimStrings.Length == 0)
        {
            return target;
        }
        string text = target;
        for (int i = 0; i < trimStrings.Length; i++)
        {
            string text2 = trimStrings[i];
            while (text.EndsWith(text2, StringComparison.Ordinal))
            {
                text = text.Substring(0, text.Length - text2.Length);
            }
        }
        return text;
    }
    public static bool IsNullOrEmpty(this string text)
    {
        return string.IsNullOrEmpty(text);
    }
    public static string Join(this IEnumerable<string> values, string separator)
    {
        return string.Join(separator, values);
    }
    public static string Join(this IEnumerable<string> values, string separator, int length, int startIndex = 0)
    {
        if (values == null || separator.IsNullOrEmpty())
        {
            return "";
        }
        int num = 0;
        string text = "";
        foreach (string current in values)
        {
            if (startIndex == num)
            {
                text += current;
            }
            else
            {
                if (startIndex <= num)
                {
                    text = text + separator + current;
                }
            }
            num++;
            if (length >= num)
            {
                return text;
            }
        }
        return text;
    }
    public static string HtmlEscape(this string value)
    {
        return TextExtenseions.EscapeHTMLString(value);
    }
    public static string HtmlUnescape(this string value)
    {
        return TextExtenseions.UnescapeHTMLString(value);
    }
    public static string ToSHA256(this string text, string key = null)
    {
        if (text.IsNullOrEmpty() || key.IsNullOrEmpty())
        {
            return "";
        }
        UTF8Encoding uTF8Encoding = new UTF8Encoding();
        byte[] bytes = uTF8Encoding.GetBytes(text);
        byte[] bytes2 = uTF8Encoding.GetBytes(key);
        HMACSHA256 hMACSHA = new HMACSHA256(bytes2);
        byte[] array = hMACSHA.ComputeHash(bytes);
        string text2 = "";
        for (int i = 0; i < array.Length; i++)
        {
            text2 += string.Format("{0,0:x2}", array[i]);
        }
        return text2;
    }
    public static DateTime ToDateTime(this long unixTime)
    {
        return Date.UnixTime2DateTime(unixTime);
    }
    public static DateTime ToDateTimeMilliSeconds(this long unixMilliSeconds)
    {
        return Date.UnixTime2DateTimeMilliSeconds(unixMilliSeconds);
    }
    public static long ToUnixTime(this DateTime dateTime)
    {
        return Date.DateTime2UnixTime(dateTime);
    }
    public static long ToUnixTimeMilliSeconds(this DateTime dateTime)
    {
        return Date.DateTime2UnixTimeMilliSeconds(dateTime);
    }
    public static int ToWeekNumber(this DateTime dateTime, DayOfWeek startDay = DayOfWeek.Sunday)
    {
        return Date.GetWeekNumber(dateTime, startDay);
    }
    public static int ToMonthNumber(this DateTime dateTime, int startDay = 1)
    {
        return Date.GetMonthNumber(dateTime, startDay);
    }
    public static DateTime ToMonthDateTime(this int monthNumber, bool atLastDay = false)
    {
        return Date.MonthNumber2DateTime(monthNumber, atLastDay);
    }
    public static StringBuilder Reverse(this StringBuilder builder)
    {
        int num = builder.Length - 1;
        int num2 = 0;
        while (num - num2 > 0)
        {
            char value = builder[num];
            builder[num] = builder[num2];
            builder[num2] = value;
            num2++;
            num--;
        }
        return builder;
    }
}
