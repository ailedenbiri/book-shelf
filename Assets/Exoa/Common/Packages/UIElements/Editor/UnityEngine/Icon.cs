using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.UIElements;
namespace UnityEngine
{
    public sealed class Icon
    {
        private static List<Icon> IconList
        {
            get;
            set;
        }
        public string ID
        {
            get;
            private set;
        }
        public string unicode
        {
            get;
            private set;
        }
        public string text
        {
            get;
            private set;
        }
        public Font font
        {
            get;
            private set;
        }
        public static implicit operator string(Icon icon)
        {
            if (icon == null)
            {
                return null;
            }
            return icon.text;
        }
        public static implicit operator Font(Icon icon)
        {
            if (icon == null)
            {
                return null;
            }
            return icon.font;
        }
        public static implicit operator StyleFont(Icon icon)
        {
            return (icon != null) ? icon.font : null;
        }
        public new string ToString()
        {
            return this.text;
        }
        public static Icon Get(string ID)
        {
            if (Icon.IconList == null || Icon.IconList.Count <= 0)
            {
                return null;
            }
            foreach (Icon current in Icon.IconList)
            {
                if (current != null && !(current.font == null) && !current.ID.IsNullOrEmpty() && !current.unicode.IsNullOrEmpty() && (!(current.ID != ID) || !(current.unicode != ID)))
                {
                    return current;
                }
            }
            return null;
        }
        public static bool Contains(string ID)
        {
            if (Icon.IconList == null || Icon.IconList.Count <= 0)
            {
                return false;
            }
            foreach (Icon current in Icon.IconList)
            {
                if (current != null && !(current.font == null) && !current.ID.IsNullOrEmpty() && !current.unicode.IsNullOrEmpty() && (!(current.ID != ID) || !(current.unicode != ID)))
                {
                    return true;
                }
            }
            return false;
        }
        public static void Add(string ID, string unicode, Font font)
        {
            if (Icon.Contains(ID))
            {
                return;
            }
            Icon.IconList.Add(new Icon(ID, unicode, font));
        }
        private Icon(string ID, string unicode, Font font)
        {
            this.ID = ID;
            this.unicode = unicode;
            ushort num = ushort.Parse(unicode, NumberStyles.AllowHexSpecifier);
            char c = (char)num;
            this.text = c.ToString();
            this.font = font;
        }
        private Icon()
        {
        }
        static Icon()
        {
            // Note: this type is marked as 'beforefieldinit'.
            IconList = new List<Icon>();
        }
    }
}
