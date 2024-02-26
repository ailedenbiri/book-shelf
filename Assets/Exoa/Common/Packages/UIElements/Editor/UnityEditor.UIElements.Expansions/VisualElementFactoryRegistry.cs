using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements.Expansions;

namespace UnityEditor.UIElements.Expansions
{

    /*

    [InitializeOnLoad]
    public static class VisualElementFactoryRegistry2
    {
        static VisualElementFactoryRegistry2()
        {

          

            try
            {
                if (Application.isEditor || Application.platform != (RuntimePlatform)17)
                {

                    MethodInfo method = Assembly.Load("UnityEngine").GetType("UnityEngine.UIElements.VisualElementFactoryRegistry").GetMethod("RegisterFactory", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                    //MethodInfo method = typeof(VisualElementFactoryRegistry).GetMethod("RegisterFactory", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

                    method.Invoke(null, new object[]
                    {
                         new Main.UxmlFactory()
                    });



                    method.Invoke(null, new object[]
                    {
                         new Header.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new Footer.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new Div.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new Article.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new H1.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new H2.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new H3.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new H4.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new H5.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new H6.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new OverlayPopup.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new BindableButton.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new BindableImage.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new BindableH1.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new BindableH2.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new BindableH3.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new BindableH4.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new BindableH5.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new BindableH6.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new BindableLabel.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new BindableListView.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                         new TextPopupField.UxmlFactory()
                    });

                    method.Invoke(null, new object[]
                    {
                       new ReorderableList.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                       new UnityEngine.UIElements.Expansions.Icon.UxmlFactory()
                    });
                    method.Invoke(null, new object[]
                    {
                       new IconButton.UxmlFactory()
                    });

                }
            }

            catch (ArgumentException ex3)
            {
                Debug.LogError(ex3.Message);
            }
            catch (TargetInvocationException ex)
            {
                if (!(ex.InnerException is ArgumentException))
                {
                    //Debug.LogError(ex.InnerException.Message);
                }
            }
            catch (Exception ex2)
            {
                Debug.LogError(ex2.Message);
            }
        }
    }*/
}
