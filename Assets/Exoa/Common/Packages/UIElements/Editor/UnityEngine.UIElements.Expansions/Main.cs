using System;
namespace UnityEngine.UIElements.Expansions
{
    public class Main : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<Main, VisualElement.UxmlTraits>
        {
            internal const string ElementName = "Main";
            internal const string UxmlNamespace = "UnityEngine.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "Main";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEngine.UIElements.Main";
                }
            }
        }
    }
}
