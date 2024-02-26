using System;
using UnityEditor.Style;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Expansions;

namespace UnityEditor
{
    [CanEditMultipleObjects, CustomEditor(typeof(ScriptableObject), true)]
    internal sealed class ScriptableObjectEditor : Editor
    {
        private VisualElement visualElement;
        public override VisualElement CreateInspectorGUI()
        {
            UnityEngine.Object expr_06 = base.target;
            Type type = (expr_06 != null) ? expr_06.GetType() : null;
            if (type == null)
            {
                UnityEngine.Object expr_22 = base.target;
                if (expr_22 != null)
                {
                    expr_22.GetType();
                }
            }
            string text = type.ToString();
            string fullName = type.FullName;
            if (text.IsNullOrEmpty() || fullName.IsNullOrEmpty())
            {
                return base.CreateInspectorGUI();
            }
            VisualTreeAsset visualTreeAsset = VisualTreeAssetFactory.Create(fullName);
            if (visualTreeAsset == null)
            {
                return base.CreateInspectorGUI();
            }
            this.visualElement = visualTreeAsset.RoutingInternal().CloneTree();
            EditorStyleInfo.AddStyleSheetToVisualElement(this.visualElement);
            StyleSheet styleSheet = StyleSheetFactory.Create(fullName);
            if (styleSheet != null)
            {
                this.visualElement.styleSheets.Add(styleSheet);
            }
            EditorStyleInfo.AddResponsiveElement(this.visualElement);
            this.visualElement.AddToClassList("exoa-uielements-expansions");
            return this.visualElement;
        }
        private void OnDisable()
        {
            if (this.visualElement == null)
            {
                return;
            }
            EditorStyleInfo.RemoveResponsiveElement(this.visualElement);
        }
        private void OnDestroy()
        {
            if (this.visualElement == null)
            {
                return;
            }
            VisualElement expr_14 = this.visualElement.GetRoot();
            OverlayPopup overlay = (expr_14 != null) ? UQueryExtensions.Q<OverlayPopup>(expr_14, null, (string)null) : null;
            if (overlay != null)
            {
                overlay.RemoveFromHierarchy();
            }
        }
    }
}
