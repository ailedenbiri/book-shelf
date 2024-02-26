using System;
namespace UnityEngine.UIElements.Expansions
{
	public class H6 : Label
	{
		public new class UxmlFactory : UxmlFactory<H6, Label.UxmlTraits>
		{
			internal const string ElementName = "H6";
			internal const string UxmlNamespace = "UnityEngine.UIElements";
			public override string uxmlName
			{
				get
				{
					return "H6";
				}
			}
			public override string uxmlQualifiedName
			{
				get
				{
					return "UnityEngine.UIElements.H6";
				}
			}
		}
		public static readonly new string ussClassName = "unity-headline";
		public static readonly string headlineClassName = "unity-headline-6";
		private static readonly string labelUssClassName = "unity-label";
		public H6() : this(null)
		{
		}
		public H6(string text) : base(text)
		{
			base.RemoveFromClassList(H6.labelUssClassName);
			base.AddToClassList(H6.ussClassName);
			base.AddToClassList(H6.headlineClassName);
		}
	}
}
