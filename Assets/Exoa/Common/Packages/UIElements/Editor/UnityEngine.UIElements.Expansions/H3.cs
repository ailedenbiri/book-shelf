using System;
namespace UnityEngine.UIElements.Expansions
{
	public class H3 : Label
	{
		public new class UxmlFactory : UxmlFactory<H3, Label.UxmlTraits>
		{
			internal const string ElementName = "H3";
			internal const string UxmlNamespace = "UnityEngine.UIElements";
			public override string uxmlName
			{
				get
				{
					return "H3";
				}
			}
			public override string uxmlQualifiedName
			{
				get
				{
					return "UnityEngine.UIElements.H3";
				}
			}
		}
		public static readonly new string  ussClassName = "unity-headline";
		public static readonly string headlineClassName = "unity-headline-3";
		private static readonly string labelUssClassName = "unity-label";
		public H3() : this(null)
		{
		}
		public H3(string text) : base(text)
		{
			base.RemoveFromClassList(H3.labelUssClassName);
			base.AddToClassList(H3.ussClassName);
			base.AddToClassList(H3.headlineClassName);
		}
	}
}
