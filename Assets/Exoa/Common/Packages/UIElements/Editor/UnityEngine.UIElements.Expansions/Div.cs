using System;
namespace UnityEngine.UIElements.Expansions
{
	public class Div : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<Div, VisualElement.UxmlTraits>
		{
			internal const string ElementName = "Div";
			internal const string UxmlNamespace = "UnityEngine.UIElements";
			public override string uxmlName
			{
				get
				{
					return "Div";
				}
			}
			public override string uxmlQualifiedName
			{
				get
				{
					return "UnityEngine.UIElements.Div";
				}
			}
		}
	}
}
