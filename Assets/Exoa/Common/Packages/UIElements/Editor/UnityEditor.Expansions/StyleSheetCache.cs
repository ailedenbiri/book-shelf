using System;
using System.Collections.Generic;
//using System.Path;
using System.Pool.Internal;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Internal;
namespace UnityEngine.UIElements.Pool
{
	internal static class StyleSheetCache
	{
		[CompilerGenerated]
		[Serializable]
		private sealed class Cache
		{
			public static readonly StyleSheetCache.Cache c = new StyleSheetCache.Cache();
			internal StyleSheet GetByKey(string key)
			{
				if (key.IsNullOrEmpty())
				{
					return null;
				}
				if (key.Contains("/"))
				{
					StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>((key != null) ? key.Replace(Application.dataPath, "Assets").TrimEnd(new string[]
					{
						"/"
					}) : null);
					if (styleSheet != null)
					{
						return styleSheet;
					}
				}
				IEnumerable<string> enumerable = AssetFiles.Get();
				foreach (string current in enumerable)
				{
					if (!current.IsNullOrEmpty() && current.EndsWith(key + ".uss", StringComparison.Ordinal))
					{
						StyleSheet styleSheet2 = AssetDatabase.LoadAssetAtPath<StyleSheet>((current != null) ? current.Replace(Application.dataPath, "Assets").TrimEnd(new string[]
						{
							"/"
						}) : null);
						if (styleSheet2 != null)
						{
							StyleSheet result = styleSheet2;
							return result;
						}
					}
				}
				string text = key.Substring(key.LastIndexOf('.') + 1);
				//string text = key.Last('.');
				if (text.IsNullOrEmpty())
				{
					return null;
				}
				foreach (string current2 in enumerable)
				{
					if (!current2.IsNullOrEmpty() && current2.EndsWith(text + ".uss", StringComparison.Ordinal))
					{
						StyleSheet styleSheet3 = AssetDatabase.LoadAssetAtPath<StyleSheet>((current2 != null) ? current2.Replace(Application.dataPath, "Assets").TrimEnd(new string[]
						{
							"/"
						}) : null);
						if (styleSheet3 != null)
						{
							StyleSheet result = styleSheet3;
							return result;
						}
					}
				}
				if (text.Contains("+"))
				{
					//text = key.Last('+');
					text = key.Substring(key.LastIndexOf('+') + 1);
					if (text.IsNullOrEmpty())
					{
						return null;
					}
					foreach (string current3 in enumerable)
					{
						if (!current3.IsNullOrEmpty() && current3.EndsWith(text + ".uss", StringComparison.Ordinal))
						{
							StyleSheet styleSheet4 = AssetDatabase.LoadAssetAtPath<StyleSheet>((current3 != null) ? current3.Replace(Application.dataPath, "Assets").TrimEnd(new string[]
							{
								"/"
							}) : null);
							if (styleSheet4 != null)
							{
								StyleSheet result = styleSheet4;
								return result;
							}
						}
					}
				}
				return null;
			}
		}
		private static readonly ObjectCache<string, StyleSheet> _pool = new ObjectCache<string, StyleSheet>(new Func<string, StyleSheet>(StyleSheetCache.Cache.c.GetByKey), null);
		public static StyleSheet Get(string key)
		{
			return StyleSheetCache._pool.Get(key);
		}
	}
}
