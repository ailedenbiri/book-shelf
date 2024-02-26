using Exoa;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;
namespace UnityEngine.Expansions
{
    public static class Reflection
    {
        private static MethodInfo ResolveTemplate
        {
            get;
            set;
        }
        private static Assembly UnityEngine
        {
            get;
            set;
        }
        private static PropertyInfo ContextAttributeOverrides
        {
            get;
            set;
        }
        private static PropertyInfo TemplateAttributeOverrides
        {
            get;
            set;
        }
        private static MethodInfo AddRange
        {
            get;
            set;
        }
        private static MethodInfo CloneTree
        {
            get;
            set;
        }
        private static Type TemplateAsset
        {
            get;
            set;
        }
        private static Type AttributeOverride
        {
            get;
            set;
        }
        private static Type AttributeOverrideList
        {
            get;
            set;
        }
        private static MethodInfo RegisterTemplate
        {
            get;
            set;
        }
        private static MethodInfo TemplateExists
        {
            get;
            set;
        }
        private static Type UsingEntry
        {
            get;
            set;
        }
        private static Type UsingEntryList
        {
            get;
            set;
        }
        private static FieldInfo Using
        {
            get;
            set;
        }
        private static FieldInfo UsingAlias
        {
            get;
            set;
        }
        private static FieldInfo UsingPath
        {
            get;
            set;
        }
        private static FieldInfo UsingAsset
        {
            get;
            set;
        }
        private static MethodInfo RegisterTemplatePath
        {
            get;
            set;
        }
        private static MethodInfo RegisterTemplateAsset
        {
            get;
            set;
        }
        static Reflection()
        {
            Reflection.Init();
        }
        private static void Init()
        {
            try
            {
                if (Reflection.UnityEngine == null)
                {
                    Reflection.UnityEngine = Assembly.Load("UnityEngine");
                }
                if (Reflection.ResolveTemplate == null)
                {
                    Reflection.ResolveTemplate = typeof(VisualTreeAsset).GetMethod("ResolveTemplate", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                if (Reflection.TemplateAsset == null)
                {
                    Reflection.TemplateAsset = Reflection.UnityEngine.GetType("UnityEngine.UIElements.TemplateAsset");
                }
                if (Reflection.AttributeOverride == null)
                {
                    Reflection.AttributeOverride = Reflection.UnityEngine.GetType("UnityEngine.UIElements.TemplateAsset+AttributeOverride");
                }
                if (Reflection.ContextAttributeOverrides == null)
                {
                    Reflection.ContextAttributeOverrides = typeof(CreationContext).GetProperty("attributeOverrides", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty);
                }
                if (Reflection.AttributeOverrideList == null)
                {
                    Reflection.AttributeOverrideList = typeof(List<>).MakeGenericType(new Type[]
                    {
                        Reflection.AttributeOverride
                    });
                }
                if (Reflection.TemplateAttributeOverrides == null)
                {
                    Reflection.TemplateAttributeOverrides = Reflection.TemplateAsset.GetProperty("attributeOverrides", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                }
                if (Reflection.AddRange == null)
                {
                    Reflection.AddRange = Reflection.AttributeOverrideList.GetMethod("AddRange", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
                }
                if (Reflection.CloneTree == null)
                {
                    Reflection.CloneTree = typeof(VisualTreeAsset).GetMethod("CloneTree", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
                    {
                        typeof(VisualElement),
                        typeof(Dictionary<string, VisualElement>),
                        Reflection.AttributeOverrideList
                    }, null);
                }
                if (Reflection.RegisterTemplate == null)
                {
                    Reflection.RegisterTemplate = typeof(VisualTreeAsset).GetMethod("RegisterTemplate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[]
                    {
                        typeof(string),
                        typeof(string)
                    }, null);
                }
                if (Reflection.TemplateExists == null)
                {
                    Reflection.TemplateExists = typeof(VisualTreeAsset).GetMethod("TemplateExists", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[]
                    {
                        typeof(string)
                    }, null);
                }
                if (Reflection.UsingEntry == null)
                {
                    Reflection.UsingEntry = Reflection.UnityEngine.GetType("UnityEngine.UIElements.VisualTreeAsset+UsingEntry");
                }
                if (Reflection.UsingEntryList == null)
                {
                    Reflection.UsingEntryList = typeof(List<>).MakeGenericType(new Type[]
                    {
                        Reflection.UsingEntry
                    });
                }
                if (Reflection.Using == null)
                {
                    Reflection.Using = typeof(VisualTreeAsset).GetField("m_Usings", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
                }
                if (Reflection.UsingAlias == null)
                {
                    Reflection.UsingAlias = Reflection.UsingEntry.GetField("alias", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
                }
                if (Reflection.UsingPath == null)
                {
                    Reflection.UsingPath = Reflection.UsingEntry.GetField("path", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
                }
                if (Reflection.UsingAsset == null)
                {
                    Reflection.UsingAsset = Reflection.UsingEntry.GetField("asset", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
                }
                if (Reflection.RegisterTemplatePath == null)
                {
                    Reflection.RegisterTemplatePath = typeof(VisualTreeAsset).GetMethod("RegisterTemplate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[]
                    {
                        typeof(string),
                        typeof(string)
                    }, null);
                }
                if (Reflection.RegisterTemplateAsset == null)
                {
                    Reflection.RegisterTemplateAsset = typeof(VisualTreeAsset).GetMethod("RegisterTemplate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[]
                    {
                        typeof(string),
                        typeof(VisualTreeAsset)
                    }, null);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message + ex.StackTrace);
            }
        }
        internal static void Invoke(string assemblyName, string typeName, string methodName, string value = null)
        {
            try
            {
                if (assemblyName.IsNullOrEmpty())
                {
                    assemblyName = "Assembly-CSharp";
                }
                if (!typeName.IsNullOrEmpty() && !methodName.IsNullOrEmpty())
                {
                    Reflection.Init();
                    Assembly assembly = Assembly.Load(assemblyName);
                    Type type = (assembly != null) ? assembly.GetType(typeName, false, true, true) : null;
                    if (!value.IsNullOrEmpty())
                    {
                        MethodInfo methodInfo = (type != null) ? type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, null, new Type[]
                        {
                            typeof(string)
                        }, null) : null;
                        if (methodInfo == null)
                        {
                            //UnityEngine.Debug.LogError("The method was not found: " + assemblyName + " " + typeName + " " + methodName);

                        }
                        else
                        {
                            object[] parameters = new object[]
                            {
                                value
                            };
                            methodInfo.Invoke(null, parameters);
                        }
                    }
                    else
                    {
                        MethodInfo methodInfo = (type != null) ? type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public) : null;
                        object[] parameters = new object[0];
                        if (methodInfo == null)
                        {
                            //Log.Error("The method was not found: {0}.{1}.{2}", new object[]
                            //{
                            //assemblyName,
                            //typeName,
                            //methodName
                            //});
                        }
                        else
                        {
                            methodInfo.Invoke(null, parameters);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Log.Error(ex.Message + ex.StackTrace, new object[0]);
            }
        }
        internal static VisualTreeAsset GetVisualTreeAssetInternal(string path, VisualTreeAsset visualTreeAsset, VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            try
            {
                VisualTreeAsset result;
                if (visualTreeAsset == null)
                {
                    result = null;
                    return result;
                }
                Reflection.Init();
                visualTreeAsset = (Reflection.ResolveTemplate.Invoke(visualTreeAsset, new object[]
                {
                    path
                }) as VisualTreeAsset);
                if (visualTreeAsset == null)
                {
                    ve.Add(new Label(string.Format("Unknown Template: '{0}'", path)));
                    result = null;
                    return result;
                }
                result = visualTreeAsset;
                return result;
            }
            catch (Exception)
            {
                //Log.Error(ex.Message + ex.StackTrace, new object[0]);
            }
            return null;
        }
        internal static Action<VisualElement> CloneTreeInternal(string path, VisualTreeAsset visualTreeAsset, Dictionary<string, VisualElement> slotInsertionPoints, VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            try
            {
                Action<VisualElement> result;
                if (visualTreeAsset == null)
                {
                    result = null;
                    return result;
                }
                Reflection.Init();
                object attributeOverrides = Activator.CreateInstance(Reflection.AttributeOverrideList);
                object value = Reflection.ContextAttributeOverrides.GetValue(cc);
                if (value != null)
                {
                    Reflection.AddRange.Invoke(attributeOverrides, new object[]
                    {
                        value
                    });
                }
                if (bag.GetType().IsSubclassOf(Reflection.TemplateAsset))
                {
                    object value2 = Reflection.TemplateAttributeOverrides.GetValue(bag);
                    if (value2 != null)
                    {
                        Reflection.AddRange.Invoke(attributeOverrides, new object[]
                        {
                            value2
                        });
                    }
                }
                result = delegate (VisualElement parent)
                {
                    Reflection.CloneTree.Invoke(visualTreeAsset, new object[]
                    {
                        parent,
                        slotInsertionPoints,
                        attributeOverrides
                    });
                };
                return result;
            }
            catch (Exception)
            {
                //Log.Error(ex.Message + ex.StackTrace, new object[0]);
            }
            return null;
        }
        internal static void CopyTemplateInternal(VisualTreeAsset visualTreeAsset, VisualTreeAsset targetTreeAsset)
        {
            try
            {
                Reflection.Init();
                IList list = Reflection.Using.GetValue(visualTreeAsset) as IList;
                foreach (object current in list)
                {
                    if (current != null)
                    {
                        string text = Reflection.UsingAlias.GetValue(current) as string;
                        string text2 = Reflection.UsingPath.GetValue(current) as string;
                        VisualTreeAsset visualTreeAsset2 = Reflection.UsingAsset.GetValue(current) as VisualTreeAsset;
                        if (!text.IsNullOrEmpty())
                        {
                            if (!text2.IsNullOrEmpty())
                            {
                                Reflection.RegisterTemplatePath.Invoke(targetTreeAsset, new object[]
                                {
                                    text,
                                    text2
                                });
                            }
                            else
                            {
                                if (visualTreeAsset2 != null)
                                {
                                    Reflection.RegisterTemplateAsset.Invoke(targetTreeAsset, new object[]
                                    {
                                        text,
                                        visualTreeAsset2
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Log.Error(ex.Message + ex.StackTrace, new object[0]);
            }
        }
        internal static void RegisterTemplateInternal(string path, string name, VisualTreeAsset visualTreeAsset)
        {
            try
            {
                Reflection.Init();
                if (!(bool)Reflection.TemplateExists.Invoke(visualTreeAsset, new object[]
                {
                    name
                }))
                {
                    Reflection.RegisterTemplate.Invoke(visualTreeAsset, new object[]
                    {
                        name,
                        path
                    });
                }
            }
            catch (Exception)
            {
                //Log.Error(ex.Message + ex.StackTrace, new object[0]);
            }
        }
    }
}
