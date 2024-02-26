using System;
using UnityEngine.UIElements.Pool;
namespace UnityEngine.UIElements
{
	public static class StyleSheetFactory
	{
		public static StyleSheet Create(string key)
		{
			return StyleSheetCache.Get(key);
		}
	}
}
