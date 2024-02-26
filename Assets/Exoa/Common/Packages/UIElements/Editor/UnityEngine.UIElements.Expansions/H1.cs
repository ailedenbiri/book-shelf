using System;
namespace UnityEngine.UIElements.Expansions
{
	public class H1 : Label
	{
		public new class UxmlFactory : UxmlFactory<H1, Label.UxmlTraits>
		{
			internal const string ElementName = "H1";
			internal const string UxmlNamespace = "UnityEngine.UIElements";
			public override string uxmlName
			{
				get
				{
					return "H1";
				}
			}
			public override string uxmlQualifiedName
			{
				get
				{
					return "UnityEngine.UIElements.H1";
				}
			}
		}
		 public static readonly new string ussClassName = "unity-headline";
		public static readonly string headlineClassName = "unity-headline-1";
		private static readonly string labelUssClassName = "unity-label";
		public H1() : this(null)
		{
		}
		public H1(string text) : base(text)
		{
			base.RemoveFromClassList(H1.labelUssClassName);
			base.AddToClassList(H1.ussClassName);
			base.AddToClassList(H1.headlineClassName);
		}
	}
}
