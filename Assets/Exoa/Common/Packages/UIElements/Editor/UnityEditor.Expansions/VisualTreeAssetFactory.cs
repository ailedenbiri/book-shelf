using System;
using UnityEngine.UIElements.Pool;
namespace UnityEngine.UIElements
{
	public static class VisualTreeAssetFactory
	{
		public static VisualTreeAsset Create(string key)
		{
			return VisualTreeAssetCache.Get(key);
		}
	}
}
