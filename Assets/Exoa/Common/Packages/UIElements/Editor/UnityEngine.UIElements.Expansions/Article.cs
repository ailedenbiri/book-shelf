using System;
namespace UnityEngine.UIElements.Expansions
{
	public class Article : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<Article, VisualElement.UxmlTraits>
		{
			internal const string ElementName = "Article";
			internal const string UxmlNamespace = "UnityEngine.UIElements";
			public override string uxmlName
			{
				get
				{
					return "Article";
				}
			}
			public override string uxmlQualifiedName
			{
				get
				{
					return "UnityEngine.UIElements.Article";
				}
			}
		}
	}
}
