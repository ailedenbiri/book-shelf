using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
namespace UnityEditor.UIElements.Expansions
{
    public class ReorderableList : PropertyField
    {
        public new class UxmlFactory : UxmlFactory<ReorderableList, ReorderableList.UxmlTraits>
        {
            internal const string ElementName = "ReorderableList";
            internal const string UxmlNamespace = "UnityEditor.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "ReorderableList";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEditor.UIElements.ReorderableList";
                }
            }
        }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlStringAttributeDescription _propertyPath;
            private UxmlStringAttributeDescription _label;
            private UxmlStringAttributeDescription _icon;
            private UxmlStringAttributeDescription _addIcon;
            private UxmlStringAttributeDescription _removeIcon;
            private UxmlStringAttributeDescription _handleIcon;
            public UxmlTraits()
            {
                UxmlStringAttributeDescription expr_0C = new UxmlStringAttributeDescription();
                expr_0C.name = ("binding-path");
                this._propertyPath = expr_0C;
                UxmlStringAttributeDescription expr_22 = new UxmlStringAttributeDescription();
                expr_22.name = ("label");
                this._label = expr_22;
                UxmlStringAttributeDescription expr_38 = new UxmlStringAttributeDescription();
                expr_38.name = ("icon");
                this._icon = expr_38;
                UxmlStringAttributeDescription expr_4E = new UxmlStringAttributeDescription();
                expr_4E.name = ("add-icon");
                this._addIcon = expr_4E;
                UxmlStringAttributeDescription expr_64 = new UxmlStringAttributeDescription();
                expr_64.name = ("remove-icon");
                this._removeIcon = expr_64;
                UxmlStringAttributeDescription expr_7A = new UxmlStringAttributeDescription();
                expr_7A.name = ("handle-icon");
                this._handleIcon = expr_7A;
            }
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                string valueFromBag = this._propertyPath.GetValueFromBag(bag, cc);
                string valueFromBag2 = this._label.GetValueFromBag(bag, cc);
                string valueFromBag3 = this._icon.GetValueFromBag(bag, cc);
                string valueFromBag4 = this._addIcon.GetValueFromBag(bag, cc);
                string valueFromBag5 = this._removeIcon.GetValueFromBag(bag, cc);
                string valueFromBag6 = this._handleIcon.GetValueFromBag(bag, cc);
                ReorderableList reorderableList;
                if (!valueFromBag.IsNullOrEmpty() && (reorderableList = (ve as ReorderableList)) != null)
                {
                    reorderableList.bindingPath = (valueFromBag);
                    reorderableList.label = (valueFromBag2);
                    reorderableList.icon = valueFromBag3;
                    reorderableList.addIcon = valueFromBag4;
                    reorderableList.removeIcon = valueFromBag5;
                    reorderableList.handleIcon = valueFromBag6;
                }
            }
        }
        private class Selectable : VisualElement
        {
            private bool _select;
            private string styleName;
            public bool select
            {
                get
                {
                    return this._select;
                }
                set
                {
                    this._select = value;
                    if (this._select)
                    {
                        base.AddToClassList(this.styleName);
                        return;
                    }
                    base.RemoveFromClassList(this.styleName);
                }
            }
            internal string bindingPath
            {
                get;
                private set;
            }
            public Selectable(string styleName, SerializedProperty property)
            {
                this.select = false;
                this.bindingPath = property.propertyPath;
                this.styleName = styleName + "_selected";
                base.AddToClassList(styleName);
            }
        }
        private class Dragger : MouseManipulator
        {
            private Vector2 start;
            private float offset;
            private bool active;
            private int index;
            private ReorderableList parent;
            public Dragger(ReorderableList parent)
            {
                List<ManipulatorActivationFilter> arg_1D_0 = base.activators;
                ManipulatorActivationFilter item = default(ManipulatorActivationFilter);
                item.button = (0);
                arg_1D_0.Add(item);
                this.active = false;
                this.parent = parent;
            }
            protected override void RegisterCallbacksOnTarget()
            {
                base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), 0);
                base.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), 0);
                base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), 0);
            }
            protected override void UnregisterCallbacksFromTarget()
            {
                base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), 0);
                base.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), 0);
                base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), 0);
            }
            protected void OnMouseDown(MouseDownEvent e)
            {
                if (this.active)
                {
                    e.StopImmediatePropagation();
                    return;
                }
                this.parent.property.serializedObject.Update();
                if (base.CanStartManipulation(e))
                {
                    this.active = true;
                    this.start = e.mousePosition;
                    this.index = this.parent.content.hierarchy.IndexOf(base.target);
                    this.Unselect(base.target.parent.parent);
                    this.Select(base.target);
                    this.offset = (base.target.localBound.yMax + base.target.localBound.yMin) / 2f;
                    MouseCaptureController.CaptureMouse(base.target);
                    e.StopPropagation();
                }
            }
            protected void OnMouseMove(MouseMoveEvent e)
            {
                if (!this.active || !MouseCaptureController.HasMouseCapture(base.target))
                {
                    return;
                }
                Vector2 vector = e.mousePosition - this.start;
                float num = this.offset + vector.y;
                VisualElement visualElement = this.parent.content.hierarchy[this.index];
                float num2 = visualElement.localBound.height / 2f;
                if (this.index > 0)
                {
                    VisualElement visualElement2 = this.parent.content.hierarchy[this.index - 1];
                    if ((visualElement2.localBound.yMin + visualElement2.localBound.yMax) / 2f >= num - num2)
                    {
                        this.parent.property.MoveArrayElement(this.index, this.index - 1);
                        this.Repaint(visualElement, visualElement2, this.index, this.index - 1);
                        this.index--;
                        this.parent.property.serializedObject.ApplyModifiedProperties();
                        e.StopPropagation();
                        return;
                    }
                }
                if (this.index < this.parent.content.hierarchy.childCount - 1)
                {
                    VisualElement visualElement3 = this.parent.content.hierarchy[this.index + 1];
                    if ((visualElement3.localBound.yMin + visualElement3.localBound.yMax) / 2f <= num + num2)
                    {
                        this.parent.property.MoveArrayElement(this.index, this.index + 1);
                        this.Repaint(visualElement, visualElement3, this.index, this.index + 1);
                        this.index++;
                        this.parent.property.serializedObject.ApplyModifiedProperties();
                        e.StopPropagation();
                        return;
                    }
                }
                e.StopPropagation();
            }
            private void Select(VisualElement a)
            {
                if (a == null)
                {
                    return;
                }
                ReorderableList.Selectable selectable;
                if ((selectable = (a as ReorderableList.Selectable)) != null && selectable.bindingPath == this.parent.property.propertyPath)
                {
                    selectable.select = true;
                }
                int childCount = a.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    this.Select(a[i]);
                }
            }
            private void Unselect(VisualElement a)
            {
                //Debug.Log("Unselect");

                if (a == null)
                {
                    return;
                }
                ReorderableList.Selectable selectable;
                if ((selectable = (a as ReorderableList.Selectable)) != null && selectable.bindingPath == this.parent.property.propertyPath)
                {
                    selectable.select = false;
                }
                int childCount = a.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    this.Unselect(a[i]);
                }
            }
            private void Repaint(VisualElement a, VisualElement b, int aIndex, int bIndex)
            {
                if (a == null || b == null)
                {
                    return;
                }
                this.Exchange(a, b);
                a.Clear();
                b.Clear();
                VisualElement visualElement = this.parent.CreateItem(aIndex, new object[]
                {
                    this.parent.property
                }, this.parent.content.hierarchy, (bool)ReorderableList.SetWideModeForWidth.Invoke(null, new object[]
                {
                    this.parent
                }));
                VisualElement visualElement2 = this.parent.CreateItem(bIndex, new object[]
                {
                    this.parent.property
                }, this.parent.content.hierarchy, (bool)ReorderableList.SetWideModeForWidth.Invoke(null, new object[]
                {
                    this.parent
                }));
                if (visualElement == null || visualElement2 == null)
                {
                    return;
                }
                BindingExtensions.Bind(visualElement, this.parent.property.serializedObject);
                BindingExtensions.Bind(visualElement2, this.parent.property.serializedObject);
                a.Add(visualElement);
                b.Add(visualElement2);
                this.Select(b);
                //Debug.Log("Repaint a:" + a);
                //Debug.Log("Repaint b:" + b);
                //Debug.Log("Repaint visualElement:" + visualElement);
                //Debug.Log("Repaint visualElement2:" + visualElement2);
            }
            private void Exchange(VisualElement a, VisualElement b)
            {
                //Debug.Log("Exchange");

                if (a == null || b == null)
                {
                    return;
                }
                Foldout foldout;
                Foldout foldout2;
                if ((foldout = (a as Foldout)) != null && (foldout2 = (b as Foldout)) != null)
                {
                    bool value = foldout.value;
                    string text = foldout.text;
                    foldout.value = (foldout2.value);
                    foldout.text = (foldout2.text);
                    foldout2.value = (value);
                    foldout2.text = (text);
                    //Debug.Log("foldout.value:" + foldout.value + " foldout.text:" + foldout.text);
                    //Debug.Log("foldout2.value:" + foldout2.value + " foldout2.text:" + foldout2.text);
                }
                ReorderableList.Selectable selectable;
                ReorderableList.Selectable selectable2;
                if ((selectable = (a as ReorderableList.Selectable)) != null && (selectable2 = (b as ReorderableList.Selectable)) != null)
                {
                    bool select = selectable.select;
                    selectable.select = selectable2.select;
                    selectable2.select = select;
                }
                int childCount = a.childCount;
                if (childCount != b.childCount)
                {
                    return;
                }
                for (int i = 0; i < childCount; i++)
                {
                    this.Exchange(a[i], b[i]);
                }
            }
            protected void OnMouseUp(MouseUpEvent e)
            {
                if (!this.active || !MouseCaptureController.HasMouseCapture(base.target) || !base.CanStopManipulation(e))
                {
                    return;
                }
                this.active = false;
                MouseCaptureController.ReleaseMouse(base.target);
                e.StopPropagation();
            }
        }
        private const string SerializedObjectBindEvent = "UnityEditor.UIElements.SerializedObjectBindEvent";
        private const string SerializedPropertyBindEvent = "UnityEditor.UIElements.SerializedPropertyBindEvent";
        public static readonly new string ussClassName = "unity-reorderable-list";
        public static readonly string headerUssClassName = ReorderableList.ussClassName + "__header";
        public static readonly string footerUssClassName = ReorderableList.ussClassName + "__footer";
        public static readonly string iconUssClassName = ReorderableList.ussClassName + "__icon";
        public static readonly new string labelUssClassName = ReorderableList.ussClassName + "__label";
        public static readonly string contentUssClassName = ReorderableList.ussClassName + "__content";
        public static readonly string itemUssClassName = ReorderableList.ussClassName + "__item";
        public static readonly string handleUssClassName = ReorderableList.ussClassName + "__handle";
        public static readonly string addUssClassName = ReorderableList.ussClassName + "__add";
        public static readonly string removeUssClassName = ReorderableList.ussClassName + "__remove";
        public static readonly string singleItemUssClassName = ReorderableList.ussClassName + "__single";
        public string icon
        {
            get;
            private set;
        }
        public string addIcon
        {
            get;
            private set;
        }
        public string removeIcon
        {
            get;
            private set;
        }
        public string handleIcon
        {
            get;
            private set;
        }
        private SerializedProperty property
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
        private static MethodInfo CreateFoldout
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
        private static MethodInfo SetWideModeForWidth
        {
            get;
            set;
        }
        private VisualElement content
        {
            get;
            set;
        }
        private static MethodInfo HasVisibleChildFields
        {
            get;
            set;
        }
        private static MethodInfo FillPropertyContextMenu
        {
            get;
            set;
        }
        private static MethodInfo ExtendsANativeType
        {
            get;
            set;
        }
        private static MethodInfo GetFieldInfoFromProperty
        {
            get;
            set;
        }
        public ReorderableList() : this(null, string.Empty)
        {
        }
        public ReorderableList(SerializedProperty property) : this(property, string.Empty)
        {
        }
        public ReorderableList(SerializedProperty property, string label)
        {
            this.GetReflection();
            base.AddToClassList(ReorderableList.ussClassName);
            base.label = (label);
            //Debug.Log("ReorderableList lalbe:" + label);

            if (property == null)
            {
                return;
            }
            this.property = property;
            base.bindingPath = (property.propertyPath);
        }
        protected override void ExecuteDefaultActionAtTarget(EventBase evt)
        {
            base.ExecuteDefaultActionAtTarget(evt);
            Type type = evt.GetType();
            string a = type.ToString();
            if (a == "UnityEditor.UIElements.SerializedObjectBindEvent")
            {
                if (ReorderableList.bindObject == null)
                {
                    ReorderableList.bindObject = type.GetProperty("bindObject");
                }
                SerializedObject serializedObject = ReorderableList.bindObject.GetValue(evt) as SerializedObject;
                this.property = ((serializedObject != null) ? serializedObject.FindProperty(base.bindingPath) : null);
                if (this.property != null)
                {
                    this.CreateListView();
                    return;
                }
            }
            else
            {
                if (a == "UnityEditor.UIElements.SerializedPropertyBindEvent")
                {
                    if (ReorderableList.bindProperty == null)
                    {
                        ReorderableList.bindProperty = type.GetProperty("bindProperty");
                    }
                    this.property = (ReorderableList.bindProperty.GetValue(evt) as SerializedProperty);
                    if (this.property != null)
                    {
                        this.CreateListView();
                    }
                }
            }
        }
        private void GetReflection()
        {
            //Debug.Log("Get Reflection visibieChilds:" + HasVisibleChildFields + " CreateFoldout:" + CreateFoldout);

            if (ReorderableList.GetHandler != null && ReorderableList.hasPropertyDrawer != null && ReorderableList.propertyDrawer != null && ReorderableList.CreateFoldout != null && ReorderableList.SetWideModeForWidth != null && ReorderableList.HasVisibleChildFields != null && ReorderableList.FillPropertyContextMenu != null && ReorderableList.ExtendsANativeType != null && ReorderableList.GetFieldInfoFromProperty != null)
            {
                return;
            }
            Assembly assembly = Assembly.Load("UnityEditor");
            Type type = assembly.GetType("UnityEditor.ScriptAttributeUtility");
            Type type2 = assembly.GetType("UnityEditor.PropertyHandler");
            Type type3 = assembly.GetType("UnityEditor.NativeClassExtensionUtilities");

            ReorderableList.GetHandler = type.GetMethod("GetHandler", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            ReorderableList.hasPropertyDrawer = type2.GetProperty("hasPropertyDrawer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            ReorderableList.propertyDrawer = type2.GetProperty("propertyDrawer", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty);
            ReorderableList.SetWideModeForWidth = typeof(InspectorElement).GetMethod("SetWideModeForWidth", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            ReorderableList.CreateFoldout = typeof(PropertyField).GetMethod("CreateFoldout", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            ReorderableList.HasVisibleChildFields = typeof(EditorGUI).GetMethod("HasVisibleChildFields", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            ReorderableList.FillPropertyContextMenu = typeof(EditorGUI).GetMethod("HasVisibleChildFields", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            ReorderableList.ExtendsANativeType = type3.GetMethod("ExtendsANativeType", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, new Type[]
            {
                typeof(UnityEngine.Object)
            }, null);




            ReorderableList.GetFieldInfoFromProperty = type.GetMethod("GetFieldInfoFromProperty", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
        }
        private void CreateListView()
        {
            base.Clear();
            VisualElement visualElement = new VisualElement();
            VisualElement visualElement2 = new VisualElement();
            visualElement.AddToClassList(ReorderableList.headerUssClassName);
            visualElement2.AddToClassList(ReorderableList.footerUssClassName);
            this.AddHeader(visualElement, "List");
            base.hierarchy.Add(visualElement);
            if (!this.property.isArray || this.property.propertyType == (SerializedPropertyType)3)
            {
                return;
            }
            this.AddButton(visualElement, new Vector2(20f, 0f), "+", ReorderableList.addUssClassName, new Action(this.OnAdd));
            this.AddButton(visualElement, new Vector2(0f, 0f), "-", ReorderableList.removeUssClassName, new Action(this.OnRemove));
            //this.AddButton(visualElement, new Vector2(20f, 0f), Icon.Get(this.addIcon.IsNullOrEmpty() ? "f067" : this.addIcon), ReorderableList.addUssClassName, new Action(this.OnAdd));
            //this.AddButton(visualElement, new Vector2(0f, 0f), Icon.Get(this.removeIcon.IsNullOrEmpty() ? "f068" : this.removeIcon), ReorderableList.removeUssClassName, new Action(this.OnRemove));

            //Debug.Log("Create new content");
            this.content = new VisualElement();
            this.content.AddToClassList(ReorderableList.contentUssClassName);
            int arraySize = this.property.arraySize;
            object[] propertyParam = new object[]
            {
                this.property
            };
            bool originalWideMode = (bool)ReorderableList.SetWideModeForWidth.Invoke(null, new object[]
            {
                this
            });
            for (int i = 0; i < arraySize; i++)
            {
                VisualElement visualElement3 = new VisualElement();
                VisualElement visualElement4 = this.CreateItem(i, propertyParam, this.content.hierarchy, originalWideMode);
                visualElement3.Add(visualElement4);
                VisualElementExtensions.AddManipulator(visualElement3, new ReorderableList.Dragger(this));
                //Debug.Log("visualElement3:" + visualElement3.name);
                //Debug.Log("visualElement4:" + visualElement4);

                if (visualElement4 != null)
                {
                    //Debug.Log("add VE to content");
                    this.content.hierarchy.Add(visualElement3);
                }
            }
            //Debug.Log("Add content to hierarchy");
            base.hierarchy.Add(this.content);
            base.hierarchy.Add(visualElement2);
        }





        internal VisualElement CreateItem(int index, object[] propertyParam, VisualElement.Hierarchy hierarchy, bool originalWideMode)
        {
            SerializedProperty p = this.property.GetArrayElementAtIndex(index);
            //Debug.Log("CreateItem p:" + p.name + " " + p.type + " index:" + index + " prop:" + this.property);

            object[] parameters = new object[]
            {
                p
            };
            object obj = ReorderableList.GetHandler.Invoke(null, parameters);
            bool hasPropertyDrawer = (bool)ReorderableList.hasPropertyDrawer.GetValue(obj);
            object targetObject = p.GetTargetObject();
            if (targetObject != null && targetObject.GetType().IsSubclassOf(typeof(SerializableBehaviour)) && VisualTreeAssetFactory.Create(p.type) == null)
            {
                hasPropertyDrawer = false;
            }

            //Debug.Log("hasPropertyDrawer:" + hasPropertyDrawer);
            hasPropertyDrawer = true;

            if (hasPropertyDrawer)
            {

                PropertyDrawer drawer = ReorderableList.propertyDrawer.GetValue(obj) as PropertyDrawer;
                VisualElement visualElement = (drawer != null) ? drawer.CreatePropertyGUI(p) : null;


                if (visualElement == null)
                {

                    visualElement = new IMGUIContainer(delegate
                    {
                        try
                        {
                            EditorGUI.BeginChangeCheck();
                            this.property.serializedObject.Update();
                            EditorGUILayout.PropertyField(p, true, new GUILayoutOption[0]);
                            this.property.serializedObject.ApplyModifiedProperties();
                            EditorGUI.EndChangeCheck();
                        }
                        finally
                        {
                            EditorGUIUtility.wideMode = (originalWideMode);
                        }
                    });
                }


                VisualElement visualElement2 = new VisualElement();
                IStyle style = visualElement2.style;
                StyleLength marginTop;
                visualElement2.style.marginBottom = (marginTop = 2f);
                style.marginTop = (marginTop);
                ReorderableList.Selectable selectable = new ReorderableList.Selectable(ReorderableList.itemUssClassName, this.property);
                selectable.style.paddingLeft = (25f);
                selectable.style.minHeight = (24f);
                selectable.style.justifyContent = (Justify)1;
                selectable.style.unityTextAlign = (TextAnchor)3;
                selectable.Add(visualElement);
                visualElement2.Add(selectable);
                this.AddHandle(visualElement2, visualElement);
                return visualElement2;
            }

            VisualElement visualElement3 = this.CreateFieldFromProperty(p);
            if (visualElement3 != null)
            {

                VisualElement visualElement4 = new VisualElement();
                IStyle style = visualElement4.style;
                StyleLength marginTop;
                visualElement4.style.marginBottom = (marginTop = 2f);
                style.marginTop = (marginTop);
                ReorderableList.Selectable selectable2 = new ReorderableList.Selectable(ReorderableList.itemUssClassName, this.property);
                selectable2.style.paddingLeft = (25f);
                selectable2.style.minHeight = (24f);
                selectable2.style.justifyContent = (Justify)1;
                selectable2.style.unityTextAlign = (TextAnchor)3;
                selectable2.Add(visualElement3);
                visualElement4.Add(selectable2);
                this.AddHandle(visualElement4, visualElement3);
                //Debug.Log("CreateItem visualElement4:" + visualElement4);

                return visualElement4;
            }
            return null;
        }



        private void OnAdd()
        {
            if (!this.property.isArray)
            {
                return;
            }
            SerializedProperty expr_14 = this.property;
            int arraySize = expr_14.arraySize;
            expr_14.arraySize = (arraySize + 1);
            this.property.serializedObject.ApplyModifiedProperties();
            //Debug.Log("OnAdd");

            VisualElement visualElement = this.CreateItem(this.property.arraySize - 1, new object[]
            {
                this.property
            }, this.content.hierarchy, (bool)ReorderableList.SetWideModeForWidth.Invoke(null, new object[]
            {
                this
            }));
            if (visualElement != null)
            {
                VisualElement visualElement2 = new VisualElement();
                BindingExtensions.Bind(visualElement, this.property.serializedObject);
                visualElement2.Add(visualElement);
                VisualElementExtensions.AddManipulator(visualElement2, new ReorderableList.Dragger(this));
                this.content.hierarchy.Add(visualElement2);
                //Debug.Log("OnAdd visualElement2:" + visualElement2);
            }

        }
        private void OnRemove()
        {
            if (!this.property.isArray)
            {
                return;
            }
            int num = -1;
            int childCount = this.content.hierarchy.childCount;
            if (childCount <= 0)
            {
                return;
            }
            int arraySize = this.property.arraySize;
            for (int i = 0; i < childCount; i++)
            {
                ReorderableList.Selectable selectable = null;
                this.GetSelectable(this.content.hierarchy[i], ref selectable);
                if (selectable != null)
                {
                    num = i;
                    break;
                }
            }
            if (num < 0)
            {
                return;
            }
            this.property.DeleteArrayElementAtIndex(num);
            this.property.serializedObject.ApplyModifiedProperties();
            if (arraySize != this.property.arraySize)
            {
                this.content.RemoveAt(childCount - 1);
            }
        }
        private void GetSelectable(VisualElement a, ref ReorderableList.Selectable output)
        {
            if (a == null)
            {
                return;
            }
            ReorderableList.Selectable selectable;
            if ((selectable = (a as ReorderableList.Selectable)) != null && selectable.bindingPath == this.property.propertyPath)
            {
                if (selectable.select && output == null)
                {
                    output = selectable;
                }
                return;
            }
            int childCount = a.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.GetSelectable(a[i], ref output);
            }
        }
        private void AddHeader(VisualElement parent, string headertext)
        {
            Icon icon = Icon.Get(this.icon.IsNullOrEmpty() ? "f03a" : this.icon);

            Label label = new Label(icon);
            label.AddToClassList(ReorderableList.iconUssClassName);
            label.style.unityFont = (icon);
            label.style.fontSize = (10f);
            label.style.width = (30f);
            label.style.minHeight = (20f);
            label.style.unityTextAlign = (TextAnchor)(4);
            label.style.position = (Position)1;
            label.style.left = (0f);
            label.style.top = (0f);
            label.style.bottom = (0f);
            parent.Add(label);
            Label label2 = new Label(base.label.IsNullOrEmpty() ? this.property.displayName : base.label);
            label2.AddToClassList(ReorderableList.labelUssClassName);
            label2.style.unityTextAlign = (TextAnchor)3;
            label2.style.left = (30f);
            label2.style.minHeight = (20f);
            label2.style.top = (0f);
            label2.style.bottom = (0f);
            parent.Add(label2);
        }
        private void AddButton(VisualElement parent, Vector2 position, string icon, string className, Action action)
        {
            Button expr_07 = new Button(action);
            expr_07.text = (icon);
            Button button = expr_07;
            //button.style.unityFont = (icon);
            button.style.fontSize = (10f);
            button.AddToClassList(className);
            button.style.width = (20f);
            button.style.minHeight = (20f);
            button.style.unityTextAlign = (TextAnchor)(4);
            button.style.position = (Position)1;
            button.style.right = (position.x);
            button.style.top = (position.y);
            button.style.backgroundColor = (new Color(0f, 0f, 0f, 0f));
            button.style.borderBottomWidth = (0f);
            button.style.top = (0f);
            button.style.bottom = (0f);
            parent.Add(button);
        }
        private void AddHandle(VisualElement parent, VisualElement content)
        {
            ReorderableList.Selectable selectable = new ReorderableList.Selectable(ReorderableList.handleUssClassName, this.property);
            //Icon icon = Icon.Get(this.handleIcon.IsNullOrEmpty() ? "f039" : this.handleIcon);
            string icon = "=";
            TextElement expr_36 = new TextElement();
            expr_36.text = icon;
            TextElement textElement = expr_36;
            //textElement.style.unityFont = (icon);
            textElement.style.fontSize = (10f);
            textElement.style.top = (0f);
            textElement.style.left = (0f);
            textElement.style.right = (0f);
            textElement.style.height = (20f);
            textElement.style.paddingTop = (4f);
            textElement.style.position = (Position)1;
            textElement.style.unityTextAlign = (TextAnchor)4;
            selectable.style.unityTextAlign = (TextAnchor)1;
            selectable.style.width = (20f);
            selectable.style.minHeight = (24f);
            selectable.style.position = (Position)1;
            selectable.style.left = (0f);
            selectable.style.top = (0f);
            selectable.style.bottom = (0f);
            selectable.Add(textElement);
            parent.Add(selectable);
        }
        private void RightClickMenuEvent(MouseUpEvent evt)
        {
            if (evt.button != 1)
            {
                return;
            }
            BindableLabel bindableLabel;
            if ((bindableLabel = (evt.target as BindableLabel)) == null)
            {
                return;
            }
            SerializedProperty serializedProperty;
            if ((serializedProperty = (bindableLabel.userData as SerializedProperty)) == null)
            {
                return;
            }
            GenericMenu genericMenu = ReorderableList.FillPropertyContextMenu.Invoke(null, new object[]
            {
                serializedProperty
            }) as GenericMenu;
            Vector2 vector = new Vector2(bindableLabel.layout.xMin, bindableLabel.layout.height);
            vector = VisualElementExtensions.LocalToWorld(bindableLabel, vector);
            Rect rect = new Rect(vector, Vector2.zero);
            genericMenu.DropDown(rect);
            evt.PreventDefault();
            evt.StopPropagation();
        }
        private VisualElement ConfigureField<TField, TValue>(TField field, SerializedProperty property) where TField : BaseField<TValue>
        {
            string label = string.IsNullOrEmpty(base.label) ? "" : base.label;
            field.bindingPath = (property.propertyPath);
            field.name = ("unity-input-" + property.propertyPath);
            field.label = (label);
            BindableLabel bindableLabel = UQueryExtensions.Q<BindableLabel>(field, null, BaseField<TValue>.labelUssClassName);
            if (bindableLabel != null)
            {
                bindableLabel.userData = (property.Copy());
                bindableLabel.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.RightClickMenuEvent), 0);
            }
            VisualElement visualElement = new VisualElement();
            visualElement.AddToClassList(ReorderableList.singleItemUssClassName);
            IStyle style = visualElement.style;
            StyleLength top;
            field.style.bottom = (top = 0f);
            style.top = (top);
            visualElement.style.minHeight = (24f);
            field.labelElement.AddToClassList(ReorderableList.labelUssClassName);
            ((VisualElement)typeof(TField).GetProperty("visualInput", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty).GetValue(field)).AddToClassList(PropertyField.inputUssClassName);
            visualElement.Add(field);
            return visualElement;
        }


        private VisualElement CreateFieldFromProperty(SerializedProperty property)
        {
            object[] parameters1 = new object[]
            {
                property
            };
            object[] parameters2 = new object[]
            {
                property,
                true
            };


            bool hasVisibleChildFields = false;
            int paramCount = ReorderableList.HasVisibleChildFields.GetParameters().Length;

            if (paramCount == 2)
            {
                hasVisibleChildFields = (bool)ReorderableList.HasVisibleChildFields.Invoke(null, parameters2);
            }
            else if (paramCount == 1)
            {
                hasVisibleChildFields = (bool)ReorderableList.HasVisibleChildFields.Invoke(null, parameters1);
            }

            //Debug.Log("hasVisibleChildFields:" + hasVisibleChildFields);
            if (hasVisibleChildFields)
            {
                paramCount = ReorderableList.CreateFoldout.GetParameters().Length;
                //Debug.Log("Create Foldout " + ReorderableList.CreateFoldout + " debug:" + debug);

                if (paramCount == 2)
                {
                    return ReorderableList.CreateFoldout.Invoke(this, parameters2) as VisualElement;
                }
                else
                {
                    return ReorderableList.CreateFoldout.Invoke(this, parameters1) as VisualElement;
                }
            }


            SerializedPropertyType propertyType = property.propertyType;

            //Debug.Log("propertyType:" + propertyType);


            switch (propertyType)
            {

                case (SerializedPropertyType)0:
                    return this.ConfigureField<IntegerField, int>(new IntegerField(), property);
                case (SerializedPropertyType)1:
                    return this.ConfigureField<Toggle, bool>(new Toggle(), property);
                case (SerializedPropertyType)2:
                    return this.ConfigureField<FloatField, float>(new FloatField(), property);
                case (SerializedPropertyType)3:
                    return this.ConfigureField<TextField, string>(new TextField(), property);
                case (SerializedPropertyType)4:
                    return this.ConfigureField<ColorField, Color>(new ColorField(), property);
                case (SerializedPropertyType)5:
                    {
                        ObjectField objectField = new ObjectField();
                        Type type = null;
                        UnityEngine.Object targetObject = property.serializedObject.targetObject;
                        if ((bool)ReorderableList.ExtendsANativeType.Invoke(null, new object[]
                        {
                    targetObject
                        }))
                        {
                            object[] expr_121 = new object[2];
                            expr_121[0] = property;
                            object[] array = expr_121;
                            ReorderableList.GetFieldInfoFromProperty.Invoke(null, array);
                            type = (array[1] as Type);
                        }
                        if (type == null)
                        {
                            type = typeof(UnityEngine.Object);
                        }
                        objectField.objectType = (type);
                        return this.ConfigureField<ObjectField, UnityEngine.Object>(objectField, property);
                    }
                case (SerializedPropertyType)6:
                    return this.ConfigureField<LayerMaskField, int>(new LayerMaskField(), property);
                case (SerializedPropertyType)7:
                    {
                        PopupField<string> expr_188 = new PopupField<string>(property.enumDisplayNames.ToList<string>(), property.enumValueIndex, null, null);
                        expr_188.index = (property.enumValueIndex);
                        PopupField<string> field = expr_188;
                        return this.ConfigureField<PopupField<string>, string>(field, property);
                    }
                case (SerializedPropertyType)8:
                    return this.ConfigureField<Vector2Field, Vector2>(new Vector2Field(), property);
                case (SerializedPropertyType)9:
                    return this.ConfigureField<Vector3Field, Vector3>(new Vector3Field(), property);
                case (SerializedPropertyType)10:
                    return this.ConfigureField<Vector4Field, Vector4>(new Vector4Field(), property);
                case (SerializedPropertyType)11:
                    return this.ConfigureField<RectField, Rect>(new RectField(), property);
                case (SerializedPropertyType)12:
                    return new PropertyField(property);
                case (SerializedPropertyType)13:
                    {
                        TextField expr_1E0 = new TextField();
                        expr_1E0.maxLength = (1);
                        TextField field2 = expr_1E0;
                        return this.ConfigureField<TextField, string>(field2, property);
                    }
                case (SerializedPropertyType)14:
                    return this.ConfigureField<CurveField, AnimationCurve>(new CurveField(), property);
                case (SerializedPropertyType)15:
                    return this.ConfigureField<BoundsField, Bounds>(new BoundsField(), property);
                case (SerializedPropertyType)16:
                    return this.ConfigureField<GradientField, Gradient>(new GradientField(), property);
                case (SerializedPropertyType)17:
                    return null;
                case (SerializedPropertyType)18:
                    return null;
                case (SerializedPropertyType)19:
                    return null;
                case (SerializedPropertyType)20:
                    return this.ConfigureField<Vector2IntField, Vector2Int>(new Vector2IntField(), property);
                case (SerializedPropertyType)21:
                    return this.ConfigureField<Vector3IntField, Vector3Int>(new Vector3IntField(), property);
                case (SerializedPropertyType)22:
                    return this.ConfigureField<RectIntField, RectInt>(new RectIntField(), property);
                case (SerializedPropertyType)23:
                    return this.ConfigureField<BoundsIntField, BoundsInt>(new BoundsIntField(), property);
            }
            return null;
        }
    }
}
