using Exoa;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
namespace UnityEditor.UIElements.Expansions
{
    public class BindableImage : Image, IBindable
    {
        public new class UxmlFactory : UxmlFactory<BindableImage, BindableImage.UxmlTraits>
        {
            internal const string ElementName = "BindableImage";
            internal const string UxmlNamespace = "UnityEditor.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "BindableImage";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEditor.UIElements.BindableImage";
                }
            }
        }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlStringAttributeDescription _src;
            private UxmlStringAttributeDescription _propertyPath;
            public UxmlTraits()
            {
                UxmlStringAttributeDescription expr_0C = new UxmlStringAttributeDescription();
                expr_0C.name = ("src");
                this._src = expr_0C;
                UxmlStringAttributeDescription expr_22 = new UxmlStringAttributeDescription();
                expr_22.name = ("binding-path");
                this._propertyPath = expr_22;
            }
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                string valueFromBag = this._src.GetValueFromBag(bag, cc);
                try
                {
                    BindableImage bindableImage;
                    if ((bindableImage = (ve as BindableImage)) != null)
                    {
                        string valueFromBag2 = this._propertyPath.GetValueFromBag(bag, cc);
                        if (!valueFromBag2.IsNullOrEmpty())
                        {
                            bindableImage.bindingPath = valueFromBag2;
                        }
                        if (!valueFromBag.IsNullOrEmpty())
                        {
                            if (valueFromBag.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                            {
                                //bindableImage.Process(valueFromBag, false);
                            }
                            else
                            {
                                bindableImage.image = (AssetDatabase.LoadAssetAtPath<Texture2D>(valueFromBag));
                            }
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
        private SerializedProperty _serializedProperty;
        private string _src;
        public IBinding binding
        {
            get;
            set;
        }
        public string bindingPath
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
        public string src
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = (value);
            }
        }
        string Value
        {
            get
            {
                return this._src;
            }
            set
            {
                this._src = value;
                if (this._src.IsNullOrEmpty())
                {
                    return;
                }
                this.Rebuild(this._src);
            }
        }
        /*
        [AsyncStateMachine(typeof(BindableImage.< Process > d__5))]
        private Task Process(string src, bool force = false)
        {
            BindableImage.< Process > d__5 < Process > d__;

            < Process > d__.<> 4__this = this;

            < Process > d__.src = src;

            < Process > d__.force = force;

            < Process > d__.<> t__builder = AsyncTaskMethodBuilder.Create();

            < Process > d__.<> 1__state = -1;
            AsyncTaskMethodBuilder<> t__builder = < Process > d__.<> t__builder;

            <> t__builder.Start < BindableImage.< Process > d__5 > (ref < Process > d__);
            return < Process > d__.<> t__builder.Task;
        }*/
        public BindableImage() : this(null, string.Empty)
        {
        }
        public BindableImage(SerializedProperty property) : this(property, string.Empty)
        {
        }
        public BindableImage(SerializedProperty property, string label)
        {
            base.AddToClassList(Image.ussClassName);
            if (property == null)
            {
                return;
            }
            this.bindingPath = property.propertyPath;
        }
        protected override void ExecuteDefaultActionAtTarget(EventBase evt)
        {
            base.ExecuteDefaultActionAtTarget(evt);
            if (!this.bindingPath.IsNullOrEmpty())
            {
                Type type = evt.GetType();
                string a = type.ToString();
                if (a == "UnityEditor.UIElements.SerializedObjectBindEvent")
                {
                    if (BindableImage.bindObject == null)
                    {
                        BindableImage.bindObject = type.GetProperty("bindObject");
                    }
                    SerializedObject serializedObject = BindableImage.bindObject.GetValue(evt) as SerializedObject;
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
                        if (BindableImage.bindProperty == null)
                        {
                            BindableImage.bindProperty = type.GetProperty("bindProperty");
                        }
                        this._serializedProperty = (BindableImage.bindProperty.GetValue(evt) as SerializedProperty);
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
            this.Rebuild(this._serializedProperty.stringValue);
        }
        private void Rebuild(string src)
        {
            if (src.IsNullOrEmpty())
            {
                return;
            }
            if (src.StartsWith("http", StringComparison.OrdinalIgnoreCase) || src.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
            {
                //this.Process(src, true);
                return;
            }
            Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(src);
            if (texture2D != null)
            {
                base.image = (texture2D);
            }
        }
        void SetValueWithoutNotify(string newValue)
        {
            this._src = newValue;
            if (this._src.IsNullOrEmpty())
            {
                return;
            }
            this.Rebuild(this._src);
        }
    }
}
