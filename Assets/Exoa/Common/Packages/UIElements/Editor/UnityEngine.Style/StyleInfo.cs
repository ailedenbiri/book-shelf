using System;
using System.Collections.Generic;
using UnityEngine.Style.Internal;
using UnityEngine.UIElements;
namespace UnityEngine.Style
{
    [CreateAssetMenu(menuName = "Module/Style", fileName = "StyleInfo", order = 0)]
    public sealed class StyleInfo : ScriptableObject
    {
        private static readonly List<StyleInfo> _editorStyleInfoCache = new List<StyleInfo>();
        [SerializeField]
        private List<StyleSheet> _styleSheets = new List<StyleSheet>();
        [SerializeField]
        private List<ResponsiveStyleSheet> _styleSheetsForResponsive = new List<ResponsiveStyleSheet>();
        public List<StyleSheet> styleSheets
        {
            get
            {
                return this._styleSheets;
            }
        }
        public List<ResponsiveStyleSheet> styleSheetsForResponsive
        {
            get
            {
                return this._styleSheetsForResponsive;
            }
        }
        public static void AddStyleSheetToVisualElement(ref VisualElement element)
        {
            if (element == null)
            {
                return;
            }
            if (StyleInfo._editorStyleInfoCache.Count <= 0)
            {
                StyleInfo._editorStyleInfoCache.AddRange(Resources.LoadAll<StyleInfo>("StyleInfo"));
            }
            if (StyleInfo._editorStyleInfoCache.Count <= 0)
            {
                return;
            }
            foreach (StyleInfo current in StyleInfo._editorStyleInfoCache)
            {
                if (!(current == null) && current.styleSheets.Count > 0)
                {
                    foreach (StyleSheet current2 in current.styleSheets)
                    {
                        if (!(current2 == null) && !element.styleSheets.Contains(current2))
                        {
                            element.styleSheets.Add(current2);
                        }
                    }
                }
            }
        }
        internal static void InvokeResponsiveStyleSheetToVisualElement(VisualElement element)
        {
            if (element == null)
            {
                return;
            }
            if (StyleInfo._editorStyleInfoCache.Count <= 0)
            {
                StyleInfo._editorStyleInfoCache.AddRange(Resources.LoadAll<StyleInfo>("StyleInfo"));
            }
            if (StyleInfo._editorStyleInfoCache.Count <= 0)
            {
                return;
            }
            foreach (StyleInfo current in StyleInfo._editorStyleInfoCache)
            {
                if (!(current == null))
                {
                    foreach (ResponsiveStyleSheet current2 in current.styleSheetsForResponsive)
                    {
                        if (current2 != null)
                        {
                            if (current2.type == ResponsiveStyleSheet.Type.GreaterThan)
                            {
                                if (element.localBound.width >= (float)current2.width)
                                {
                                    goto IL_174;
                                }
                                using (List<StyleSheet>.Enumerator enumerator3 = current2.styleSheets.GetEnumerator())
                                {
                                    while (enumerator3.MoveNext())
                                    {
                                        StyleSheet current3 = enumerator3.Current;
                                        if (!(current3 == null) && element.styleSheets.Contains(current3))
                                        {
                                            element.styleSheets.Remove(current3);
                                        }
                                    }
                                    continue;
                                }
                            }
                            if (element.localBound.width > (float)current2.width)
                            {
                                using (List<StyleSheet>.Enumerator enumerator4 = current2.styleSheets.GetEnumerator())
                                {
                                    while (enumerator4.MoveNext())
                                    {
                                        StyleSheet current4 = enumerator4.Current;
                                        if (!(current4 == null) && element.styleSheets.Contains(current4))
                                        {
                                            element.styleSheets.Remove(current4);
                                        }
                                    }
                                    continue;
                                }
                            }
IL_174:
                            foreach (StyleSheet current5 in current2.styleSheets)
                            {
                                if (!(current5 == null) && !element.styleSheets.Contains(current5))
                                {
                                    element.styleSheets.Add(current5);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
