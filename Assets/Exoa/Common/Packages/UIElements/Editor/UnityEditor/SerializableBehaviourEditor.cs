using System;
using System.Collections;
using UnityEditor.Style;
using UnityEngine;
using UnityEngine.UIElements;
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(SerializableBehaviour), true)]
    internal sealed class SerializableBehaviourEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            Type arg_18_0;
            if (property == null)
            {
                arg_18_0 = null;
            }
            else
            {
                object expr_0C = property.GetTargetObject();
                arg_18_0 = ((expr_0C != null) ? expr_0C.GetType() : null);
            }
            Type type = arg_18_0;
            if (type == null)
            {
                return base.CreatePropertyGUI(property);
            }
            string text = type.ToString();
            string fullName = type.FullName;
            if (text.IsNullOrEmpty() || fullName.IsNullOrEmpty())
            {
                return base.CreatePropertyGUI(property);
            }
            VisualTreeAsset visualTreeAsset = VisualTreeAssetFactory.Create(fullName);
            if (visualTreeAsset == null)
            {
                return base.CreatePropertyGUI(property);
            }
            VisualElement visualElement = visualTreeAsset.CloneTree(property.propertyPath);
            EditorStyleInfo.AddStyleSheetToVisualElement(visualElement);
            StyleSheet styleSheet = StyleSheetFactory.Create(fullName);
            if (styleSheet != null)
            {
                visualElement.styleSheets.Add(styleSheet);
            }
            visualElement.AddToClassList("exoa-uielements-expansions");
            return visualElement;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float num = 0f;
            IEnumerator enumerator = property.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializedProperty serializedProperty;
                if ((serializedProperty = (enumerator.Current as SerializedProperty)) != null)
                {
                    num += EditorGUI.GetPropertyHeight(serializedProperty, true);
                }
            }
            return num;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            IEnumerator enumerator = property.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializedProperty serializedProperty;
                if ((serializedProperty = (enumerator.Current as SerializedProperty)) != null)
                {
                    float propertyHeight = EditorGUI.GetPropertyHeight(serializedProperty, true);
                    position.height = (propertyHeight);
                    EditorGUI.PropertyField(position, serializedProperty, true);
                    position.y = (position.y + propertyHeight);
                }
            }
        }
    }
}
