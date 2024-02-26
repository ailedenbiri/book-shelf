using System;
using System.Reflection;
namespace UnityEngine.UIElements.Expansions
{
    internal static class IconFactoryRegistry
    {
        /*
        [RuntimeInitializeOnLoadMethod]
        private static void FactoryRegistry()
        {
            try
            {
                if (Application.isEditor || Application.platform != (RuntimePlatform)17)
                {
                    MethodInfo method = Assembly.Load("UnityEngine").GetType("UnityEngine.UIElements.VisualElementFactoryRegistry").GetMethod("RegisterFactory", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                    method.Invoke(null, new object[]
                    {
                        new Icon.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                        new IconButton.UxmlFactory()
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
        }*/
    }
}
