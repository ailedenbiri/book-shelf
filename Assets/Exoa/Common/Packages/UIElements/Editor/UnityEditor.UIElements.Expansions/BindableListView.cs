using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
namespace UnityEditor.UIElements.Expansions
{
    public class BindableListView : PropertyField
    {
        public new class UxmlFactory : UxmlFactory<BindableListView, PropertyField.UxmlTraits>
        {
            internal const string ElementName = "BindableListView";
            internal const string UxmlNamespace = "UnityEditor.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "BindableListView";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEditor.UIElements.BindableListView";
                }
            }
        }
        private const string SerializedObjectBindEvent = "UnityEditor.UIElements.SerializedObjectBindEvent";
        private const string SerializedPropertyBindEvent = "UnityEditor.UIElements.SerializedPropertyBindEvent";
        private const string FoldoutTitleBoundLabelProperty = "unity-foldout-bound-title";
        public static readonly new string ussClassName = "unity-listview-field";
        public static readonly string contentUssClassName = BindableListView.ussClassName + "__content";
        private static PropertyInfo bindProperty
        {
            get;
            set;
        }
        private static MethodInfo GetHandler
        {
            get;
            set;
        }
        private static PropertyInfo hasPropertyDrawer
        {
            get;
            set;
        }
        private static PropertyInfo propertyDrawer
        {
            get;
            set;
        }
        private static PropertyInfo bindObject
        {
            get;
            set;
        }
        private static MethodInfo SetWideModeForWidth
        {
            get;
            set;
        }
        private static MethodInfo CreateFieldFromProperty
        {
            get;
            set;
        }
        private static FieldInfo m_SerializedProperty
        {
            get;
            set;
        }
        private static FieldInfo m_ParentPropertyField
        {
            get;
            set;
        }
        private static MethodInfo HasVisibleChildFields
        {
            get;
            set;
        }
        private static PropertyInfo localizedDisplayName
        {
            get;
            set;
        }
        private static MethodInfo SetProperty
        {
            get;
            set;
        }
        protected override void ExecuteDefaultActionAtTarget(EventBase evt)
        {
            base.ExecuteDefaultActionAtTarget(evt);
            Type type = evt.GetType();
            string a = type.ToString();
            if (a == "UnityEditor.UIElements.SerializedObjectBindEvent")
            {
                if (BindableListView.bindObject == null)
                {
                    BindableListView.bindObject = type.GetProperty("bindObject");
                }
                SerializedObject serializedObject = BindableListView.bindObject.GetValue(evt) as SerializedObject;
                this.Reset((serializedObject != null) ? serializedObject.FindProperty(base.bindingPath) : null);
                return;
            }
            if (a == "UnityEditor.UIElements.SerializedPropertyBindEvent")
            {
                if (BindableListView.bindProperty == null)
                {
                    BindableListView.bindProperty = type.GetProperty("bindProperty");
                }
                this.Reset(BindableListView.bindProperty.GetValue(evt) as SerializedProperty);
            }
        }
        public BindableListView() : this(null, string.Empty)
        {
        }
        public BindableListView(SerializedProperty property) : this(property, string.Empty)
        {
        }
        public BindableListView(SerializedProperty property, string label)
        {
            this.GetReflection();
            base.AddToClassList(BindableListView.ussClassName);
            base.label = (label);
            if (property == null)
            {
                return;
            }
            base.bindingPath = (property.propertyPath);
        }
        private void GetReflection()
        {
            if (BindableListView.GetHandler != null && BindableListView.hasPropertyDrawer != null && BindableListView.propertyDrawer != null && BindableListView.SetWideModeForWidth != null && BindableListView.CreateFieldFromProperty != null && BindableListView.m_SerializedProperty != null && BindableListView.HasVisibleChildFields != null)
            {
                return;
            }
            Assembly assembly = Assembly.Load("UnityEditor");
            Type type = assembly.GetType("UnityEditor.ScriptAttributeUtility");
            Type type2 = assembly.GetType("UnityEditor.PropertyHandler");
            BindableListView.GetHandler = type.GetMethod("GetHandler", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            BindableListView.hasPropertyDrawer = type2.GetProperty("hasPropertyDrawer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            BindableListView.propertyDrawer = type2.GetProperty("propertyDrawer", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty);
            BindableListView.SetWideModeForWidth = typeof(InspectorElement).GetMethod("SetWideModeForWidth", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            BindableListView.CreateFieldFromProperty = typeof(PropertyField).GetMethod("CreateFieldFromProperty", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            BindableListView.HasVisibleChildFields = typeof(EditorGUI).GetMethod("HasVisibleChildFields", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            BindableListView.m_SerializedProperty = typeof(PropertyField).GetField("m_SerializedProperty", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField);
            BindableListView.m_ParentPropertyField = typeof(PropertyField).GetField("m_ParentPropertyField", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField);
            BindableListView.localizedDisplayName = typeof(SerializedProperty).GetProperty("localizedDisplayName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty);
            BindableListView.SetProperty = typeof(VisualElement).GetMethod("SetProperty", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[]
            {
                typeof(PropertyName),
                typeof(object)
            }, null);
        }
        private void Reset(SerializedProperty bindProperty)
        {
            base.Clear();
            if (bindProperty == null)
            {
                return;
            }
            BindableListView.m_SerializedProperty.SetValue(this, bindProperty);
            bool originalWideMode = (bool)BindableListView.SetWideModeForWidth.Invoke(null, new object[]
            {
                this
            });
            object[] parameters = new object[]
            {
                bindProperty
            };
            object obj = BindableListView.GetHandler.Invoke(null, parameters);
            bool flag = (bool)BindableListView.hasPropertyDrawer.GetValue(obj);
            object targetObject = bindProperty.GetTargetObject();
            if (targetObject != null && targetObject.GetType().IsSubclassOf(typeof(SerializableBehaviour)) && VisualTreeAssetFactory.Create(bindProperty.type) == null)
            {
                flag = false;
            }
            if (flag)
            {
                PropertyDrawer expr_D1 = BindableListView.propertyDrawer.GetValue(obj) as PropertyDrawer;
                VisualElement visualElement = (expr_D1 != null) ? expr_D1.CreatePropertyGUI(bindProperty) : null;
                if (visualElement == null)
                {
                    visualElement = new IMGUIContainer(delegate
                    {
                        try
                        {
                            EditorGUI.BeginChangeCheck();
                            bindProperty.serializedObject.Update();
                            EditorGUILayout.PropertyField(bindProperty, true, new GUILayoutOption[0]);
                            bindProperty.serializedObject.ApplyModifiedProperties();
                            EditorGUI.EndChangeCheck();
                        }
                        finally
                        {
                            EditorGUIUtility.wideMode = (originalWideMode);
                        }
                    });
                }
                base.hierarchy.Add(visualElement);
                return;
            }
            if ((bool)BindableListView.HasVisibleChildFields.Invoke(null, parameters))
            {
                VisualElement visualElement2 = this.CreateFoldout(bindProperty);
                if (visualElement2 != null)
                {
                    base.hierarchy.Add(visualElement2);
                    return;
                }
            }
            else
            {
                VisualElement visualElement3;
                if ((visualElement3 = (BindableListView.CreateFieldFromProperty.Invoke(this, new object[]
                {
                    bindProperty
                }) as VisualElement)) != null)
                {
                    base.hierarchy.Add(visualElement3);
                }
            }
        }
        private VisualElement CreateFoldout(SerializedProperty property)
        {
            //Debug.Log("BindlableList CreateFoldout");

            property = property.Copy();
            Foldout foldout = new Foldout();
            foldout.text = (BindableListView.localizedDisplayName.GetValue(property) as string);
            foldout.value = (property.isExpanded);
            foldout.bindingPath = (property.propertyPath);
            foldout.name = ("unity-foldout-" + property.propertyPath);
            //Debug.Log("Test:" + foldout.name);

            Foldout foldout2 = foldout;
            Toggle toggle = UQueryExtensions.Q<Toggle>(foldout2, null, Foldout.toggleUssClassName);
            Label obj = UQueryExtensions.Q<Label>(toggle, null, Toggle.textUssClassName);
            BindableListView.SetProperty.Invoke(obj, new object[]
            {
                new PropertyName("unity-foldout-bound-title"),
                true
            });
            SerializedProperty endProperty = property.GetEndProperty();
            property.NextVisible(true);
            while (!SerializedProperty.EqualContents(property, endProperty))
            {
                object[] parameters = new object[]
                {
                    property
                };
                object obj2 = BindableListView.GetHandler.Invoke(null, parameters);
                bool flag = (bool)BindableListView.hasPropertyDrawer.GetValue(obj2);
                object targetObject = property.GetTargetObject();
                if (targetObject != null && targetObject.GetType().IsSubclassOf(typeof(SerializableBehaviour)) && VisualTreeAssetFactory.Create(property.type) == null)
                {
                    flag = false;
                }
                PropertyField expr_12A = new PropertyField(property);
                expr_12A.name = ("unity-property-field-" + property.propertyPath);
                PropertyField propertyField = expr_12A;
                BindableListView.m_ParentPropertyField.SetValue(propertyField, this);
                if (propertyField != null)
                {
                    if (flag || (property.isArray && property.propertyType != (SerializedPropertyType)3))
                    {
                        foldout2.Add(propertyField);
                    }
                    else
                    {
                        VisualElement visualElement = new VisualElement();
                        visualElement.AddToClassList(BindableListView.contentUssClassName);
                        visualElement.Add(propertyField);
                        foldout2.Add(visualElement);
                    }
                }
                if (!property.NextVisible(false))
                {
                    break;
                }
            }
            return foldout2;
        }
    }
}
