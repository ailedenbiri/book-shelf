using Exoa;
using System;
using System.Reflection;
using UnityEngine.UIElements;
namespace UnityEditor.UIElements.Expansions
{
    public class BindableH4 : Label, IBindable
    {
        public new class UxmlFactory : UxmlFactory<BindableH4, BindableH4.UxmlTraits>
        {
            internal const string ElementName = "BindableH4";
            internal const string UxmlNamespace = "UnityEditor.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "BindableH4";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEditor.UIElements.BindableH4";
                }
            }
        }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlStringAttributeDescription _text;
            private UxmlStringAttributeDescription _propertyPath;
            public UxmlTraits()
            {
                UxmlStringAttributeDescription expr_0C = new UxmlStringAttributeDescription();
                expr_0C.name = ("text");
                this._text = expr_0C;
                UxmlStringAttributeDescription expr_22 = new UxmlStringAttributeDescription();
                expr_22.name = ("binding-path");
                this._propertyPath = expr_22;
            }
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                try
                {
                    BindableH4 bindableH;
                    if ((bindableH = (ve as BindableH4)) != null)
                    {
                        string valueFromBag = this._propertyPath.GetValueFromBag(bag, cc);
                        if (!SystemExtensions.IsNullOrEmpty(valueFromBag))
                        {
                            bindableH.bindingPath = valueFromBag;
                        }
                        string valueFromBag2 = this._text.GetValueFromBag(bag, cc);
                        if (!SystemExtensions.IsNullOrEmpty(valueFromBag2))
                        {
                            bindableH.text = (valueFromBag2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError(ex.Message);
                }
            }
        }
        private const string SerializedObjectBindEvent = "UnityEditor.UIElements.SerializedObjectBindEvent";
        private const string SerializedPropertyBindEvent = "UnityEditor.UIElements.SerializedPropertyBindEvent";
        public static readonly new string ussClassName = "unity-headline";
        public static readonly string headlineClassName = "unity-headline-4";
        private static readonly string labelUssClassName = "unity-label";
        private SerializedProperty _serializedProperty;
        public new IBinding binding
        {
            get;
            set;
        }
        public new string bindingPath
        {
            get;
            set;
        }
        private static PropertyInfo bindProperty
        {
            get;
            set;
        }
        private static PropertyInfo bindObject
        {
            get;
            set;
        }
        public BindableH4() : this(null, string.Empty)
        {
        }
        public BindableH4(SerializedProperty property) : this(property, string.Empty)
        {
        }
        public BindableH4(SerializedProperty property, string label)
        {
            base.RemoveFromClassList(BindableH4.labelUssClassName);
            base.AddToClassList(BindableH4.ussClassName);
            base.AddToClassList(BindableH4.headlineClassName);
            if (!SystemExtensions.IsNullOrEmpty(label))
            {
                this.text = (label);
            }
            if (property == null)
            {
                return;
            }
            this.bindingPath = property.propertyPath;
        }
        protected override void ExecuteDefaultActionAtTarget(EventBase evt)
        {
            base.ExecuteDefaultActionAtTarget(evt);
            if (!SystemExtensions.IsNullOrEmpty(this.bindingPath))
            {
                Type type = evt.GetType();
                string a = type.ToString();
                if (a == "UnityEditor.UIElements.SerializedObjectBindEvent")
                {
                    if (BindableH4.bindObject == null)
                    {
                        BindableH4.bindObject = type.GetProperty("bindObject");
                    }
                    SerializedObject serializedObject = BindableH4.bindObject.GetValue(evt) as SerializedObject;
                    this._serializedProperty = ((serializedObject != null) ? serializedObject.FindProperty(this.bindingPath) : null);
                    if (this._serializedProperty != null)
                    {
                        this.CreateView();
                        return;
                    }
                }
                else
                {
                    if (a == "UnityEditor.UIElements.SerializedPropertyBindEvent")
                    {
                        if (BindableH4.bindProperty == null)
                        {
                            BindableH4.bindProperty = type.GetProperty("bindProperty");
                        }
                        this._serializedProperty = (BindableH4.bindProperty.GetValue(evt) as SerializedProperty);
                        if (this._serializedProperty != null)
                        {
                            this.CreateView();
                        }
                    }
                }
            }
        }
        private void CreateView()
        {
            if (this._serializedProperty == null)
            {
                return;
            }
            if (this._serializedProperty.propertyType != (SerializedPropertyType)3)
            {
                return;
            }
            string stringValue = this._serializedProperty.stringValue;
            if (SystemExtensions.IsNullOrEmpty(stringValue))
            {
                return;
            }
            this.text = (stringValue);
        }
    }
}
