using Exoa;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
namespace UnityEngine.UIElements.Expansions
{
    public class OverlayPopup : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<OverlayPopup, OverlayPopup.UxmlTraits>
        {
            internal const string ElementName = "OverlayPopup";
            internal const string UxmlNamespace = "UnityEngine.UIElements";
            public override string uxmlName
            {
                get
                {
                    return "OverlayPopup";
                }
            }
            public override string uxmlQualifiedName
            {
                get
                {
                    return "UnityEngine.UIElements.OverlayPopup";
                }
            }
        }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlStringAttributeDescription _assemblyName;
            private UxmlStringAttributeDescription _typeName;
            private UxmlStringAttributeDescription _methodName;
            private UxmlStringAttributeDescription _value;
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
            }
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                string assemblyName = this._assemblyName.GetValueFromBag(bag, cc);
                string typeName = this._typeName.GetValueFromBag(bag, cc);
                string methodName = this._methodName.GetValueFromBag(bag, cc);
                string value = this._value.GetValueFromBag(bag, cc);
                if (assemblyName.IsNullOrEmpty())
                {
                    assemblyName = "Assembly-CSharp";
                }
                try
                {
                    OverlayPopup overlay;
                    if ((overlay = (ve as OverlayPopup)) != null && !assemblyName.IsNullOrEmpty() && !typeName.IsNullOrEmpty() && !methodName.IsNullOrEmpty())
                    {
                        if (OverlayPopup.Invoke == null)
                        {
                            OverlayPopup.Invoke = Assembly.Load("UnityEngine.Expansions").GetType("UnityEngine.Expansions.Reflection").GetMethod("Invoke", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
                        }
                        overlay.clickable.clicked += (delegate
                        {
                            OverlayPopup.Invoke.Invoke(null, new object[]
                            {
                                assemblyName,
                                typeName,
                                methodName,
                                value
                            });
                        });
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError(ex.Message);
                }
            }
        }
        private Button _button;
        private static MethodInfo Invoke
        {
            get;
            set;
        }
        public bool IsShow
        {
            get;
            private set;
        }
        public Clickable clickable
        {
            get
            {
                if (this._button == null)
                {
                    return null;
                }
                return this._button.clickable;
            }
            set
            {
                if (this._button == null)
                {
                    return;
                }
                this._button.clickable = value;
            }
        }
        /*[AsyncStateMachine(typeof(Overlay.< Show > d__10))]
       public Task Show(int duration = 500)
       {
          Overlay.< Show > d__10 < Show > d__;

           < Show > d__.<> 4__this = this;

           < Show > d__.duration = duration;

           < Show > d__.<> t__builder = AsyncTaskMethodBuilder.Create();

           < Show > d__.<> 1__state = -1;
           AsyncTaskMethodBuilder<> t__builder = < Show > d__.<> t__builder;

           <> t__builder.Start < Overlay.< Show > d__10 > (ref < Show > d__);
           return < Show > d__.<> t__builder.Task;
    }
    [AsyncStateMachine(typeof(Overlay.< Hide > d__11))]
        public Task Hide(int duration = 500)
        {
            Overlay.< Hide > d__11 < Hide > d__;

            < Hide > d__.<> 4__this = this;

            < Hide > d__.duration = duration;

            < Hide > d__.<> t__builder = AsyncTaskMethodBuilder.Create();

            < Hide > d__.<> 1__state = -1;
            AsyncTaskMethodBuilder<> t__builder = < Hide > d__.<> t__builder;

            <> t__builder.Start < Overlay.< Hide > d__11 > (ref < Hide > d__);
            return < Hide > d__.<> t__builder.Task;
        }
        [AsyncStateMachine(typeof(Overlay.< Toggle > d__12))]
        public Task Toggle(int duration = 500)
        {
          /*  Overlay.< Toggle > d__12 < Toggle > d__;

            < Toggle > d__.<> 4__this = this;

            < Toggle > d__.duration = duration;

            < Toggle > d__.<> t__builder = AsyncTaskMethodBuilder.Create();

            < Toggle > d__.<> 1__state = -1;
            AsyncTaskMethodBuilder<> t__builder = < Toggle > d__.<> t__builder;

            <> t__builder.Start < Overlay.< Toggle > d__12 > (ref < Toggle > d__);
            return < Toggle > d__.<> t__builder.Task;
        }
        public OverlayPopup()
        {
            this.< IsShow > k__BackingField = true;
            base..ctor();
            this.AddButton(this);
            this.Hide(0);
        }*/
        private void AddButton(VisualElement parent)
        {
            if (this._button != null)
            {
                return;
            }
            this._button = new Button();
            this._button.style.position = (Position)1;
            this._button.style.top = (0f);
            this._button.style.right = (0f);
            this._button.style.bottom = (0f);
            this._button.style.left = (0f);
            this._button.style.opacity = (0f);
            this._button.style.visibility = (Visibility)1;
            parent.hierarchy.Add(this._button);
        }
    }
}
