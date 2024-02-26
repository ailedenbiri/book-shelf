using System;
namespace UnityEngine.UIElements.Expansions
{
	public class H5 : Label
	{
		public new class UxmlFactory : UxmlFactory<H5, Label.UxmlTraits>
		{
			internal const string ElementName = "H5";
			internal const string UxmlNamespace = "UnityEngine.UIElements";
			public override string uxmlName
			{
				get
				{
					return "H5";
				}
			}
			public override string uxmlQualifiedName
			{
				get
				{
					return "UnityEngine.UIElements.H5";
				}
			}
		}
		public static readonly new string ussClassName = "unity-headline";
		public static readonly string headlineClassName = "unity-headline-5";
		private static readonly string labelUssClassName = "unity-label";
		public H5() : this(null)
		{
		}
		public H5(string text) : base(text)
		{
			base.RemoveFromClassList(H5.labelUssClassName);
			base.AddToClassList(H5.ussClassName);
			base.AddToClassList(H5.headlineClassName);
		}
	}
}
