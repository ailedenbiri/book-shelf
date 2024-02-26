using Exoa;
using System;
namespace UnityEngine.UIElements.Expansions
{
    public class IconButton : Button
    {
        public new class UxmlFactory : UxmlFactory<IconButton, IconButton.UxmlTraits>
        {
            internal const string ElementName = "IconButton";
            internal const string UxmlNamespace = "UnityEngine.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "IconButton";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEngine.UIElements.IconButton";
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
                    IconButton iconButton;
                    if ((iconButton = (ve as IconButton)) != null)
                    {
                        string valueFromBag = this._icon.GetValueFromBag(bag, cc);
                        if (!valueFromBag.IsNullOrEmpty())
                        {
                            iconButton.text = (valueFromBag);
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
        public IconButton()
        {
        }
        public IconButton(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return;
            }
            this.text = (id);
        }
    }
}
