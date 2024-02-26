using System;
using System.Reflection;
using UnityEngine;
namespace UnityEditor.UIElements.Expansions
{
    /*
    [InitializeOnLoad]
    public static class ReorderableListFactoryRegistry2
    {
        static ReorderableListFactoryRegistry2()
        {
            try
            {
                if (Application.isEditor || Application.platform != (RuntimePlatform)17)
                {
                    
                    MethodInfo method = Assembly.Load("UnityEngine").GetType("UnityEngine.UIElements.VisualElementFactoryRegistry").GetMethod("RegisterFactory", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                    method.Invoke(null, new object[]
                    {
                        new ReorderableList.UxmlFactory()
                    });
                }
            }
            catch (TargetInvocationException ex)
            {
                if (!(ex.InnerException is ArgumentException))
                {
                    Debug.LogError(ex.InnerException.Message);
                }
            }
            catch (Exception ex2)
            {
                Debug.LogError(ex2.InnerException.Message);
            }
        }
    }*/
}
