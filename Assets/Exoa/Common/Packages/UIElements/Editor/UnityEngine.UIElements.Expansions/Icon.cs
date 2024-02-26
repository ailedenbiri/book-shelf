using Exoa;
using System;
namespace UnityEngine.UIElements.Expansions
{
    public class Icon : Label
    {
        public new class UxmlFactory : UxmlFactory<Icon, Icon.UxmlTraits>
        {
            internal const string ElementName = "Icon";
            internal const string UxmlNamespace = "UnityEngine.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "Icon";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEngine.UIElements.Icon";
                }
            }
        }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlStringAttributeDescription _icon;
            public UxmlTraits()
            {
                UxmlStringAttributeDescription expr_0C = new UxmlStringAttributeDescription();
                expr_0C.name = ("icon");
                this._icon = expr_0C;
            }
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                try
                {
                    Icon icon;
                    if ((icon = (ve as Icon)) != null)
                    {
                        string valueFromBag = this._icon.GetValueFromBag(bag, cc);
                        if (!valueFromBag.IsNullOrEmpty())
                        {
                            icon.text = (valueFromBag);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError(ex.Message);
                }
            }
        }
        public override string text
        {
            get
            {
                UnityEngine.Icon icon = UnityEngine.Icon.Get(base.text);
                if (base.style.unityFont != icon)
                {
                    base.style.unityFont = (icon);
                }
                return icon;
            }
            set
            {
                base.text = (value);
            }
        }
        public Icon() : this(null)
        {
        }
        public Icon(string icon) : base(icon)
        {
            if (!icon.IsNullOrEmpty())
            {
                this.text = (icon);
            }
        }
    }
}
