using Exoa;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;
namespace UnityEditor.UIElements.Expansions
{
    public class TextPopupField : PopupField<string>
    {
        public new class UxmlFactory : UxmlFactory<TextPopupField, TextPopupField.UxmlTraits>
        {
            internal const string ElementName = "TextPopupField";
            internal const string UxmlNamespace = "UnityEditor.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "TextPopupField";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEditor.UIElements.TextPopupField";
                }
            }
        }
        public new class UxmlTraits : BaseField<string>.UxmlTraits
        {
            private UxmlStringAttributeDescription _data;
            private UxmlStringAttributeDescription _selected;
            public UxmlTraits()
            {
                UxmlStringAttributeDescription expr_0C = new UxmlStringAttributeDescription();
                expr_0C.name = ("data");
                this._data = expr_0C;
                UxmlStringAttributeDescription expr_22 = new UxmlStringAttributeDescription();
                expr_22.name = ("selected");
                this._selected = expr_22;
            }
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                string valueFromBag = this._selected.GetValueFromBag(bag, cc);
                string valueFromBag2 = this._data.GetValueFromBag(bag, cc);
                try
                {
                    TextPopupField textPopupField;
                    if ((textPopupField = (ve as TextPopupField)) != null)
                    {
                        if (!SystemExtensions.IsNullOrEmpty(valueFromBag2))
                        {
                            if (TextPopupField.choices == null)
                            {
                                TextPopupField.choices = typeof(PopupField<string>).GetProperty("choices", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty);
                            }
                            List<string> list;
                            if ((list = (TextPopupField.choices.GetValue(textPopupField) as List<string>)) != null)
                            {
                                list.AddRange(valueFromBag2.Split(new char[]
                                {
                                    ','
                                }));
                            }
                        }
                        if (!SystemExtensions.IsNullOrEmpty(valueFromBag))
                        {
                            textPopupField.value = (valueFromBag);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError(ex.Message);
                }
            }
        }
        public static readonly new string ussClassName = "unity-text-popup-field";
        private static new PropertyInfo choices
        {
            get;
            set;
        }

        public TextPopupField() : this(null)
        {
        }
        public TextPopupField(string label = null) : base(label)
        {
            base.AddToClassList(TextPopupField.ussClassName);
        }
        public TextPopupField(List<string> choices, string defaultValue, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null) : this(null, choices, defaultValue, formatSelectedValueCallback, formatListItemCallback)
        {
        }
        public TextPopupField(List<string> choices, int defaultIndex, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null) : this(null, choices, defaultIndex, formatSelectedValueCallback, formatListItemCallback)
        {
        }
        public TextPopupField(string label, List<string> choices, string defaultValue, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null) : base(label, choices, defaultValue, formatSelectedValueCallback, formatListItemCallback)
        {
            base.AddToClassList(TextPopupField.ussClassName);
        }
        public TextPopupField(string label, List<string> choices, int defaultIndex, Func<string, string> formatSelectedValueCallback = null, Func<string, string> formatListItemCallback = null) : base(label, choices, defaultIndex, formatSelectedValueCallback, formatListItemCallback)
        {
            base.AddToClassList(TextPopupField.ussClassName);
        }
    }
}
