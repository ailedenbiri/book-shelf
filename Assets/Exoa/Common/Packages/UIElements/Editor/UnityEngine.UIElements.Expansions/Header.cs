using System;

namespace UnityEngine.UIElements.Expansions
{
    public class Header : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<Header, VisualElement.UxmlTraits>
        {
            internal const string ElementName = "Header";
            internal const string UxmlNamespace = "UnityEngine.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "Header";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEngine.UIElements.Header";
                }
            }
        }
    }
}
