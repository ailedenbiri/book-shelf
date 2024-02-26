using System;
namespace UnityEngine.UIElements.Expansions
{
	public class H4 : Label
	{
		public new class UxmlFactory : UxmlFactory<H4, Label.UxmlTraits>
		{
			internal const string ElementName = "H4";
			internal const string UxmlNamespace = "UnityEngine.UIElements";
			public override string uxmlName
			{
				get
				{
					return "H4";
				}
			}
			public override string uxmlQualifiedName
			{
				get
				{
					return "UnityEngine.UIElements.H4";
				}
			}
		}
		public static readonly new string ussClassName = "unity-headline";
		public static readonly string headlineClassName = "unity-headline-4";
		private static readonly string labelUssClassName = "unity-label";
		public H4() : this(null)
		{
		}
		public H4(string text) : base(text)
		{
			base.RemoveFromClassList(H4.labelUssClassName);
			base.AddToClassList(H4.ussClassName);
			base.AddToClassList(H4.headlineClassName);
		}
	}
}
