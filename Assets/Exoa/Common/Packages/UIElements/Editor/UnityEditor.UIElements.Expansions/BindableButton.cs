using Exoa;
using System;
using System.Reflection;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Expansions;
namespace UnityEditor.UIElements.Expansions
{
    public class BindableButton : Button
    {
        public new class UxmlFactory : UxmlFactory<BindableButton, BindableButton.UxmlTraits>
        {
            internal const string ElementName = "BindableButton";
            internal const string UxmlNamespace = "UnityEditor.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "BindableButton";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEditor.UIElements.BindableButton";
                }
            }
        }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public class Values
            {
                public string assemblyName;
                public string typeName;
                public string methodName;
                public string value;
                public string path;
                public string text;
                public string control;
                public string target;

                public Action Exec()
                {

                    MethodInfo m = Assembly.Load(assemblyName).GetType(assemblyName + "." + typeName).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
                    return m.Invoke(null, new object[] { value }) as Action;
                }
            }
            private UxmlStringAttributeDescription _assemblyName;
            private UxmlStringAttributeDescription _typeName;
            private UxmlStringAttributeDescription _methodName;
            private UxmlStringAttributeDescription _value;
            private UxmlStringAttributeDescription _path;
            private UxmlStringAttributeDescription _text;
            private UxmlStringAttributeDescription _control;
            private UxmlStringAttributeDescription _target;
            public UxmlTraits()
            {
                UxmlStringAttributeDescription expr_0C = new UxmlStringAttributeDescription();
                expr_0C.name = ("method");
                this._methodName = expr_0C;
                UxmlStringAttributeDescription expr_22 = new UxmlStringAttributeDescription();
                expr_22.name = ("type");
                this._typeName = expr_22;
                UxmlStringAttributeDescription expr_38 = new UxmlStringAttributeDescription();
                expr_38.name = ("assembly");
                this._assemblyName = expr_38;
                UxmlStringAttributeDescription expr_4E = new UxmlStringAttributeDescription();
                expr_4E.name = ("value");
                this._value = expr_4E;
                UxmlStringAttributeDescription expr_64 = new UxmlStringAttributeDescription();
                expr_64.name = ("path");
                this._path = expr_64;
                UxmlStringAttributeDescription expr_7A = new UxmlStringAttributeDescription();
                expr_7A.name = ("text");
                this._text = expr_7A;
                UxmlStringAttributeDescription expr_90 = new UxmlStringAttributeDescription();
                expr_90.name = ("control");
                this._control = expr_90;
                UxmlStringAttributeDescription expr_A6 = new UxmlStringAttributeDescription();
                expr_A6.name = ("target");
                this._target = expr_A6;
            }
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                BindableButton.UxmlTraits.Values var1 = new BindableButton.UxmlTraits.Values();
                base.Init(ve, bag, cc);
                var1.assemblyName = this._assemblyName.GetValueFromBag(bag, cc);
                var1.typeName = this._typeName.GetValueFromBag(bag, cc);
                var1.methodName = this._methodName.GetValueFromBag(bag, cc);
                var1.value = this._value.GetValueFromBag(bag, cc);
                string valueFromBag = this._path.GetValueFromBag(bag, cc);
                string valueFromBag2 = this._text.GetValueFromBag(bag, cc);
                var1.control = this._control.GetValueFromBag(bag, cc);
                var1.target = this._target.GetValueFromBag(bag, cc);
                if (var1.assemblyName.IsNullOrEmpty())
                {
                    var1.assemblyName = "Assembly-CSharp";
                }
                try
                {
                    BindableButton field;
                    if ((field = (ve as BindableButton)) != null)
                    {
                        field.text = (valueFromBag2);
                        if (!var1.assemblyName.IsNullOrEmpty() && !var1.typeName.IsNullOrEmpty() && !var1.methodName.IsNullOrEmpty())
                        {
                            if (BindableButton.Invoke == null)
                            {
                                //BindableButton.Invoke = Assembly.Load("UnityEngine.Expansions").GetType("UnityEngine.Expansions.Reflection").GetMethod("Invoke", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                                BindableButton.Invoke = typeof(UnityEngine.Expansions.Reflection).GetMethod("Invoke", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                            }
                            field.clickable.clicked += (new Action(() => var1.Exec()));
                            //UnityEngine.Debug.Log("Click Action 1");
                        }
                        if (!valueFromBag.IsNullOrEmpty() && !var1.target.IsNullOrEmpty())
                        {
                            if (!(cc.visualTreeAsset == null))
                            {
                                if (BindableButton.GetVisualTreeAssetInternal == null)
                                {
                                    //BindableButton.GetVisualTreeAssetInternal = Assembly.Load("UnityEngine.Expansions").GetType("UnityEngine.Expansions.Reflection").GetMethod("GetVisualTreeAssetInternal", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                                    BindableButton.GetVisualTreeAssetInternal = typeof(UnityEngine.Expansions.Reflection).GetMethod("GetVisualTreeAssetInternal", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                                }
                                if (BindableButton.CloneTreeInternal == null)
                                {
                                    //BindableButton.CloneTreeInternal = Assembly.Load("UnityEngine.Expansions").GetType("UnityEngine.Expansions.Reflection").GetMethod("CloneTreeInternal", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                                    BindableButton.CloneTreeInternal = typeof(UnityEngine.Expansions.Reflection).GetMethod("CloneTreeInternal", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                                }
                                VisualTreeAsset visualTreeAsset;
                                if ((visualTreeAsset = (BindableButton.GetVisualTreeAssetInternal.Invoke(null, new object[]
                                {
                                       valueFromBag,
                                       cc.visualTreeAsset,
                                       ve,
                                       bag,
                                       cc
                                }) as VisualTreeAsset)) != null)
                                {
                                    Action<VisualElement> action;
                                    if ((action = (BindableButton.CloneTreeInternal.Invoke(null, new object[]
                                    {
                                           valueFromBag,
                                           visualTreeAsset,
                                           cc.slotInsertionPoints,
                                           ve,
                                           bag,
                                           cc
                                    }) as Action<VisualElement>)) != null)
                                    {
                                        //UnityEngine.Debug.Log("Click Action 2");
                                        field.clickable.clicked += (delegate
                                        {
                                            VisualElement expr_10 = field.GetRoot();
                                            VisualElement visualElement = (expr_10 != null) ? UQueryExtensions.Q(expr_10, var1.target, (string)null) : null;
                                            if (visualElement == null)
                                            {
                                                return;
                                            }
                                            visualElement.Clear();
                                            action(visualElement);
                                        });
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!var1.control.IsNullOrEmpty() && !var1.target.IsNullOrEmpty())
                            {
                                var1.control = var1.control.ToLower();
                                //UnityEngine.Debug.Log("Click Action 3");
                                field.clickable.clicked += (LoadTemplateOnClick(var1, field));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError(ex.Message);
                }

            }

            private static Action LoadTemplateOnClick(Values var1, BindableButton field)
            {
                return delegate
                {
                    BindableButton expr_06 = field;
                    VisualElement arg_2A_0;
                    if (expr_06 == null)
                    {
                        arg_2A_0 = null;
                    }
                    else
                    {
                        VisualElement expr_12 = expr_06.GetRoot();
                        arg_2A_0 = ((expr_12 != null) ? UQueryExtensions.Q(expr_12, var1.target, (string)null) : null);
                    }
                    VisualElement visualElement = arg_2A_0;
                    VisualElement visualElement2 = visualElement;
                    string control;
                    if (visualElement2 != null)
                    {
                        IVisible visible;
                        if ((visible = (visualElement2 as IVisible)) == null)
                        {
                            IPlayback playback;
                            if ((playback = (visualElement2 as IPlayback)) == null)
                            {
                                IPagination pagination;
                                if ((pagination = (visualElement2 as IPagination)) == null)
                                {
                                    IAppendable appendable;
                                    if ((appendable = (visualElement2 as IAppendable)) != null)
                                    {
                                        IAppendable appendable2 = appendable;
                                        control = var1.control;
                                        if (control == "append")
                                        {
                                            appendable2.Append(var1.value, null);
                                            goto IL_224;
                                        }
                                        if (!(control == "remove"))
                                        {
                                            if (!(control == "clear"))
                                            {
                                                goto IL_224;
                                            }
                                            appendable2.Clear();
                                            goto IL_224;
                                        }
                                        else
                                        {
                                            int index;
                                            if (var1.value.IsNullOrEmpty() || !int.TryParse(var1.value, out index))
                                            {
                                                appendable2.Remove(null);
                                                goto IL_224;
                                            }
                                            appendable2.RemoveAt(index);
                                            goto IL_224;
                                        }
                                    }
                                }
                                else
                                {
                                    IPagination pagination2 = pagination;
                                    control = var1.control;
                                    if (control == "next")
                                    {
                                        pagination2.Next();
                                        goto IL_224;
                                    }
                                    if (!(control == "prev"))
                                    {
                                        goto IL_224;
                                    }
                                    pagination2.Prev();
                                    goto IL_224;
                                }
                            }
                            else
                            {
                                IPlayback playback2 = playback;
                                control = var1.control;
                                if (control == "play")
                                {
                                    playback2.Play();
                                    goto IL_224;
                                }
                                if (control == "stop")
                                {
                                    playback2.Stop();
                                    goto IL_224;
                                }
                                if (!(control == "pause"))
                                {
                                    goto IL_224;
                                }
                                playback2.Pause();
                                goto IL_224;
                            }
                        }
                        else
                        {
                            IVisible visible2 = visible;
                            control = var1.control;
                            if (control == "show")
                            {
                                visible2.Show(-1);
                                goto IL_224;
                            }
                            if (!(control == "hide"))
                            {
                                visible2.Toggle(-1);
                                goto IL_224;
                            }
                            visible2.Hide(-1);
                            goto IL_224;
                        }
                    }
                    control = var1.control;
                    if (control == "remove")
                    {
                        visualElement.RemoveFromHierarchy();
                    }
IL_224:
                    control = var1.control;
                    if (control == "repaint")
                    {
                        visualElement.MarkDirtyRepaint();
                    }
                };
            }
        }
        private static MethodInfo Invoke
        {
            get;
            set;
        }
        private static MethodInfo GetVisualTreeAssetInternal
        {
            get;
            set;
        }
        private static MethodInfo CloneTreeInternal
        {
            get;
            set;
        }
        private static MethodInfo AccordionInvokeInternal
        {
            get;
            set;
        }
    }
}
