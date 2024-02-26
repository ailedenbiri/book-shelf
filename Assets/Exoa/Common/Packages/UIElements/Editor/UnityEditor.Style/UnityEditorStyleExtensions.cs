using System;
using System.Reflection;
using UnityEditor.Style;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Expansions;
namespace UnityEditor.UIElements
{
    public static class UnityEditorStyleExtensions
    {
        private static Assembly Router
        {
            get;
            set;
        }
        private static Type RouterExtensions
        {
            get;
            set;
        }
        private static MethodInfo Routing
        {
            get;
            set;
        }
        public static VisualElement ApplyStyle(this EditorWindow window)
        {
            string arg_18_0;
            if (window == null)
            {
                arg_18_0 = null;
            }
            else
            {
                Type expr_0C = window.GetType();
                arg_18_0 = ((expr_0C != null) ? expr_0C.ToString() : null);
            }
            string text = arg_18_0;
            string arg_31_0;
            if (window == null)
            {
                arg_31_0 = null;
            }
            else
            {
                Type expr_25 = window.GetType();
                arg_31_0 = ((expr_25 != null) ? expr_25.FullName : null);
            }
            string text2 = arg_31_0;
            if (text.IsNullOrEmpty() || text2.IsNullOrEmpty())
            {
                return null;
            }
            VisualTreeAsset visualTreeAsset = VisualTreeAssetFactory.Create(text2);
            //UnityEngine.Debug.Log("text2:" + text2);
            //UnityEngine.Debug.Log("visualTreeAsset:" + visualTreeAsset);

            if (visualTreeAsset == null)
            {
                return null;
            }
            VisualTreeAsset routing = visualTreeAsset.RoutingInternal();
            if (routing == null)
            {
                return null;
            }

            VisualElement visualElement = routing.CloneTree();
            EditorStyleInfo.AddStyleSheetToVisualElement(window.rootVisualElement);
            StyleSheet styleSheet = StyleSheetFactory.Create(text2);
            if (styleSheet != null)
            {
                visualElement.styleSheets.Add(styleSheet);
            }
            EditorStyleInfo.AddResponsiveElement(window.rootVisualElement);
            visualElement.AddToClassList("exoa-uielements-expansions");
            window.rootVisualElement.Add(visualElement);
            return visualElement;
        }
        internal static VisualTreeAsset RoutingInternal(this VisualTreeAsset visualTree)
        {
            try
            {
                if (UnityEditorStyleExtensions.Router == null)
                {
                    UnityEditorStyleExtensions.Router = Assembly.Load("UnityEditor.UIElements.Router");
                }
                if (UnityEditorStyleExtensions.Router == null)
                {
                    VisualTreeAsset result = visualTree;
                    return result;
                }
                if (UnityEditorStyleExtensions.RouterExtensions == null)
                {
                    UnityEditorStyleExtensions.RouterExtensions = UnityEditorStyleExtensions.Router.GetType("UnityEditor.UIElements.RouterExtensions");
                }
                if (UnityEditorStyleExtensions.RouterExtensions == null)
                {
                    VisualTreeAsset result = visualTree;
                    return result;
                }
                if (UnityEditorStyleExtensions.Routing == null)
                {
                    UnityEditorStyleExtensions.Routing = UnityEditorStyleExtensions.RouterExtensions.GetMethod("Routing", BindingFlags.Static | BindingFlags.Public);
                }
                if (UnityEditorStyleExtensions.Routing == null)
                {
                    VisualTreeAsset result = visualTree;
                    return result;
                }
                VisualTreeAsset visualTreeAsset = UnityEditorStyleExtensions.Routing.Invoke(null, new object[]
                {
                    visualTree
                }) as VisualTreeAsset;
                if (visualTreeAsset != null)
                {
                    VisualTreeAsset result = visualTreeAsset;
                    return result;
                }
            }
            catch
            {
            }
            return visualTree;
        }
        public static bool IsInspectorPreview(this VisualElement element)
        {
            if (element == null)
            {
                return false;
            }
            if (element.ClassListContains("exoa-uielements-expansions"))
            {
                return false;
            }
            while (element.parent != null)
            {
                element = element.parent;
                if (element.ClassListContains("exoa-uielements-expansions"))
                {
                    return false;
                }
            }
            return true;
        }
        /*public static Overlay MoveToEditorScreenOverlay(this VisualElement element)
		{
			if (element == null)
			{
				return null;
			}
			Overlay screenOverlay = UnityEditorStyleExtensions.GetScreenOverlay(element);
			if (UnityEditorStyleExtensions.ContainsScreenOverlay(screenOverlay, element))
			{
				return screenOverlay;
			}
			VisualElement expr_1D = element.parent;
			if (expr_1D != null)
			{
				expr_1D.Remove(element);
			}
			screenOverlay.Add(element);
			return screenOverlay;
		}
		private static Overlay GetScreenOverlay(VisualElement element)
		{
			if (element == null)
			{
				return null;
			}
			element = element.GetRootContainer();
			element = ((element != null) ? element.parent : null);
			if (element != null && element.childCount > 0)
			{
				foreach (VisualElement current in element.Children())
				{
					Overlay result;
					if ((result = (current as Overlay)) != null)
					{
						return result;
					}
				}
			}
			Overlay overlay = new Overlay();
			EditorStyleInfo.AddStyleSheetToVisualElement(overlay);
			EditorStyleInfo.AddResponsiveElement(overlay);
			element.Add(overlay);
			return overlay;
		}
		private static bool ContainsScreenOverlay(Overlay overlay, VisualElement element)
		{
			if (overlay == null || element == null)
			{
				return false;
			}
			if (overlay.childCount <= 0)
			{
				return false;
			}
			foreach (VisualElement current in overlay.Children())
			{
				if (current == element)
				{
					return true;
				}
			}
			return false;
		}*/
        private static VisualElement GetRootContainer(this VisualElement element)
        {
            if (element == null)
            {
                return null;
            }
            element = element.GetRoot();
            string n = null;
            return UQueryExtensions.Q<TemplateContainer>(element, null, n);
        }
    }
}
