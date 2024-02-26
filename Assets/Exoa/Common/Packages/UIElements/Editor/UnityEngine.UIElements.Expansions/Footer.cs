using System;
namespace UnityEngine.UIElements.Expansions
{
	public class Footer : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<Footer, VisualElement.UxmlTraits>
		{
			internal const string ElementName = "Footer";
			internal const string UxmlNamespace = "UnityEngine.UIElements";
			public override string uxmlName
			{
				get
				{
					return "Footer";
				}
			}
			public override string uxmlQualifiedName
			{
				get
				{
					return "UnityEngine.UIElements.Footer";
				}
			}
		}
	}
}
