using System;
namespace UnityEngine.UIElements.Expansions
{
	public class H2 : Label
	{
		public new class UxmlFactory : UxmlFactory<H2, Label.UxmlTraits>
		{
			internal const string ElementName = "H2";
			internal const string UxmlNamespace = "UnityEngine.UIElements";
			public override string uxmlName
			{
				get
				{
					return "H2";
				}
			}
			public override string uxmlQualifiedName
			{
				get
				{
					return "UnityEngine.UIElements.H2";
				}
			}
		}
		public static readonly new string ussClassName = "unity-headline";
		public static readonly string headlineClassName = "unity-headline-2";
		private static readonly string labelUssClassName = "unity-label";
		public H2() : this(null)
		{
		}
		public H2(string text) : base(text)
		{
			base.RemoveFromClassList(H2.labelUssClassName);
			base.AddToClassList(H2.ussClassName);
			base.AddToClassList(H2.headlineClassName);
		}
	}
}
