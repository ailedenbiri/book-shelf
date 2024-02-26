using System;
namespace UnityEngine.UIElements
{
	public static class UnityEngineStyleExtensions
	{
		public static VisualElement GetRoot(this VisualElement element)
		{
			if (element == null)
			{
				return element;
			}
			while (element.parent != null)
			{
				element = element.parent;
			}
			return element;
		}
		public static T QRoot<T>(this VisualElement element, string name = null, params string[] classes) where T : VisualElement
		{
			if (element == null)
			{
				return default(T);
			}
			VisualElement expr_13 = element.GetRoot();
			if (expr_13 == null)
			{
				return default(T);
			}
			return UQueryExtensions.Q<T>(expr_13, name, classes);
		}
		public static VisualElement QRoot(this VisualElement element, string name = null, params string[] classes)
		{
			if (element == null)
			{
				return null;
			}
			VisualElement expr_0B = element.GetRoot();
			if (expr_0B == null)
			{
				return null;
			}
			return UQueryExtensions.Q(expr_0B, name, classes);
		}
		public static T QRoot<T>(this VisualElement element, string name = null, string className = null) where T : VisualElement
		{
			if (element == null)
			{
				return default(T);
			}
			VisualElement expr_13 = element.GetRoot();
			if (expr_13 == null)
			{
				return default(T);
			}
			return UQueryExtensions.Q<T>(expr_13, name, className);
		}
		public static VisualElement QRoot(this VisualElement element, string name = null, string className = null)
		{
			if (element == null)
			{
				return null;
			}
			VisualElement expr_0B = element.GetRoot();
			if (expr_0B == null)
			{
				return null;
			}
			return UQueryExtensions.Q(expr_0B, name, className);
		}
		public static UQueryBuilder<T>? QueryRoot<T>(this VisualElement element, string name = null, params string[] classes) where T : VisualElement
		{
			if (element == null)
			{
				return null;
			}
			VisualElement expr_13 = element.GetRoot();
			if (expr_13 == null)
			{
				return null;
			}
			return new UQueryBuilder<T>?(UQueryExtensions.Query<T>(expr_13, name, classes));
		}
		public static UQueryBuilder<VisualElement>? QueryRoot(this VisualElement element, string name = null, params string[] classes)
		{
			if (element == null)
			{
				return null;
			}
			VisualElement expr_13 = element.GetRoot();
			if (expr_13 == null)
			{
				return null;
			}
			return new UQueryBuilder<VisualElement>?(UQueryExtensions.Query(expr_13, name, classes));
		}
		public static UQueryBuilder<T>? QueryRoot<T>(this VisualElement element, string name = null, string className = null) where T : VisualElement
		{
			if (element == null)
			{
				return null;
			}
			VisualElement expr_13 = element.GetRoot();
			if (expr_13 == null)
			{
				return null;
			}
			return new UQueryBuilder<T>?(UQueryExtensions.Query<T>(expr_13, name, className));
		}
		public static UQueryBuilder<VisualElement>? QueryRoot(this VisualElement element, string name = null, string className = null)
		{
			if (element == null)
			{
				return null;
			}
			VisualElement expr_13 = element.GetRoot();
			if (expr_13 == null)
			{
				return null;
			}
			return new UQueryBuilder<VisualElement>?(UQueryExtensions.Query(expr_13, name, className));
		}
	}
}
