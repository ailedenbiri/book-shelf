using Exoa;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace UnityEngine
{
    public static class UnityEngineExtensions
    {
        public static Vector2 Add(this Vector2 value, float x, float y)
        {
            value.x += x;
            value.y += y;
            return value;
        }
        public static Vector3 Add(this Vector3 value, float x, float y, float z)
        {
            value.x += x;
            value.y += y;
            value.z += z;
            return value;
        }
        public static Vector4 Add(this Vector4 value, float x, float y, float z, float w)
        {
            value.x += x;
            value.y += y;
            value.z += z;
            value.w += w;
            return value;
        }
        public static Rect Add(this Rect value, float x, float y, float width, float height)
        {
            value.x = (value.x + x);
            value.y = (value.y + y);
            value.width = (value.width + width);
            value.height = (value.height + height);
            return value;
        }
        public static bool EqualTo(this Vector2 a, Vector2 b, float range = 0f)
        {
            return Mathf.Abs(a.x - b.x) <= range && Mathf.Abs(a.y - b.y) <= range;
        }
        public static bool EqualTo(this Vector3 a, Vector3 b, float range = 0f)
        {
            return Mathf.Abs(a.x - b.x) <= range && Mathf.Abs(a.y - b.y) <= range && Mathf.Abs(a.z - b.z) <= range;
        }
        public static bool EqualTo(this Vector4 a, Vector4 b, float range = 0f)
        {
            return Mathf.Abs(a.x - b.x) <= range && Mathf.Abs(a.y - b.y) <= range && Mathf.Abs(a.z - b.z) <= range && Mathf.Abs(a.w - b.w) <= range;
        }
        public static bool EqualTo(this Rect a, Rect b, float range = 0f)
        {
            return Mathf.Abs(a.x - b.x) <= range && Mathf.Abs(a.y - b.y) <= range;
        }
        public static ResourceRequestAwaiter GetAwaiter(this ResourceRequest asyncOp)
        {
            return new ResourceRequestAwaiter(asyncOp);
        }
        public static Color ToColor(this string code)
        {
            Color result;
            if (!ColorUtility.TryParseHtmlString(code, out result))
            {
                return Color.white;
            }
            return result;
        }
        public static string ToCode(this Color color, bool useAlpha = false)
        {
            if (useAlpha)
            {
                return "#" + ColorUtility.ToHtmlStringRGBA(color);
            }
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }
        public static Color Clone(this Color color)
        {
            return new Color(color.r, color.g, color.b, color.a);
        }
        public static T Instantiate<T>(this T original, string name, Transform parent) where T : Object
        {
            T t = Object.Instantiate<T>(original, parent);
            t.name = (name);
            return t;
        }
        public static T GetComponentInScene<T>(this GameObject gameObject, bool includeThis = false, bool includeInactive = true) where T : Component
        {
            Scene arg_06_0 = gameObject.scene;
            if (includeInactive)
            {
                if (includeThis)
                {
                    T component = gameObject.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
                T[] array = Resources.FindObjectsOfTypeAll<T>();
                if (array.Length == 0)
                {
                    return default(T);
                }
                T[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    T t = array2[i];
                    if (!(t == null) && !(t.gameObject == null) && !(t.gameObject.scene != gameObject.scene) && (includeThis || t.gameObject != gameObject))
                    {
                        return t;
                    }
                }
                return default(T);
            }
            else
            {
                if (includeThis && gameObject.activeInHierarchy)
                {
                    T component2 = gameObject.GetComponent<T>();
                    if (component2 != null)
                    {
                        return component2;
                    }
                }
                T[] array3 = Object.FindObjectsOfType<T>();
                if (array3.Length == 0)
                {
                    return default(T);
                }
                T[] array4 = array3;
                for (int j = 0; j < array4.Length; j++)
                {
                    T t2 = array4[j];
                    if (!(t2 == null) && !(t2.gameObject == null) && !(t2.gameObject.scene != gameObject.scene) && (includeThis || t2.gameObject != gameObject))
                    {
                        return t2;
                    }
                }
                return default(T);
            }
        }
        public static Component GetComponentInScene(this GameObject gameObject, Type type, bool includeThis = false, bool includeInactive = true)
        {
            if (gameObject == null)
            {
                return null;
            }
            Scene arg_0F_0 = gameObject.scene;
            if (includeInactive)
            {
                if (includeThis)
                {
                    Component component = gameObject.GetComponent<Component>();
                    if (component != null)
                    {
                        return component;
                    }
                }
                Object[] array = Resources.FindObjectsOfTypeAll(type);
                if (array.Length == 0)
                {
                    return null;
                }
                Object[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    Object @object = array2[i];
                    Component component2;
                    if ((component2 = (@object as Component)) != null && !(component2 == null) && !(component2.gameObject == null) && !(component2.gameObject.scene != gameObject.scene) && (includeThis || component2.gameObject != gameObject))
                    {
                        return component2;
                    }
                }
                return null;
            }
            else
            {
                if (includeThis && gameObject.activeInHierarchy)
                {
                    Component component3 = gameObject.GetComponent<Component>();
                    if (component3 != null)
                    {
                        return component3;
                    }
                }
                Object[] array3 = Object.FindObjectsOfType(type);
                if (array3.Length == 0)
                {
                    return null;
                }
                Object[] array4 = array3;
                for (int j = 0; j < array4.Length; j++)
                {
                    Object object2 = array4[j];
                    Component component4;
                    if ((component4 = (object2 as Component)) != null && !(component4 == null) && !(component4.gameObject == null) && !(component4.gameObject.scene != gameObject.scene) && (includeThis || component4.gameObject != gameObject))
                    {
                        return component4;
                    }
                }
                return null;
            }
        }
        public static T GetComponentInScene<T>(this Component component, bool includeThis = false, bool includeInactive = true) where T : Component
        {
            if (component.gameObject == null)
            {
                return default(T);
            }
            Scene arg_19_0 = component.gameObject.scene;
            if (includeInactive)
            {
                if (includeThis)
                {
                    T component2 = component.GetComponent<T>();
                    if (component2 != null)
                    {
                        return component2;
                    }
                }
                T[] array = Resources.FindObjectsOfTypeAll<T>();
                if (array.Length == 0)
                {
                    return default(T);
                }
                T[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    T t = array2[i];
                    if (!(t == null) && !(t.gameObject == null) && !(t.gameObject.scene != component.gameObject.scene) && (includeThis || t != component))
                    {
                        return t;
                    }
                }
                return default(T);
            }
            else
            {
                if (includeThis && component.gameObject.activeInHierarchy)
                {
                    T component3 = component.GetComponent<T>();
                    if (component3 != null)
                    {
                        return component3;
                    }
                }
                T[] array3 = Object.FindObjectsOfType<T>();
                if (array3.Length == 0)
                {
                    return default(T);
                }
                T[] array4 = array3;
                for (int j = 0; j < array4.Length; j++)
                {
                    T t2 = array4[j];
                    if (!(t2 == null) && !(t2.gameObject == null) && !(t2.gameObject.scene != component.gameObject.scene) && (includeThis || t2 != component))
                    {
                        return t2;
                    }
                }
                return default(T);
            }
        }
        public static Component GetComponentInScene(this Component component, Type type, bool includeThis = false, bool includeInactive = true)
        {
            if (component.gameObject == null)
            {
                return null;
            }
            Scene arg_19_0 = component.gameObject.scene;
            if (includeInactive)
            {
                if (includeThis)
                {
                    Component component2 = component.GetComponent<Component>();
                    if (component2 != null)
                    {
                        return component2;
                    }
                }
                Object[] array = Resources.FindObjectsOfTypeAll(type);
                if (array.Length == 0)
                {
                    return null;
                }
                Object[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    Object @object = array2[i];
                    Component component3;
                    if ((component3 = (@object as Component)) != null && !(component3 == null) && !(component3.gameObject == null) && !(component3.gameObject.scene != component.gameObject.scene) && (includeThis || component3 != component))
                    {
                        return component3;
                    }
                }
                return null;
            }
            else
            {
                if (includeThis && component.gameObject.activeInHierarchy)
                {
                    Component component4 = component.GetComponent<Component>();
                    if (component4 != null)
                    {
                        return component4;
                    }
                }
                Object[] array3 = Object.FindObjectsOfType(type);
                if (array3.Length == 0)
                {
                    return null;
                }
                Object[] array4 = array3;
                for (int j = 0; j < array4.Length; j++)
                {
                    Object object2 = array4[j];
                    Component component5;
                    if ((component5 = (object2 as Component)) != null && !(component5 == null) && !(component5.gameObject == null) && !(component5.gameObject.scene != component.gameObject.scene) && (includeThis || component5 != component))
                    {
                        return component5;
                    }
                }
                return null;
            }
        }
        public static void GetComponentsInScene<T>(this GameObject gameObject, List<T> components) where T : Component
        {
            Scene arg_06_0 = gameObject.scene;
            T[] array = Object.FindObjectsOfType<T>();
            if (array.Length == 0)
            {
                return;
            }
            T[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                T t = array2[i];
                if (!(t == null) && !(t.gameObject == null) && !(t.gameObject.scene != gameObject.scene))
                {
                    components.Add(t);
                }
            }
        }
        public static void GetComponentsInScene(this GameObject gameObject, Type type, List<Component> components)
        {
            Scene arg_06_0 = gameObject.scene;
            Object[] array = Object.FindObjectsOfType(type);
            if (array.Length == 0)
            {
                return;
            }
            Object[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                Object @object = array2[i];
                Component component;
                if ((component = (@object as Component)) != null && !(component == null) && !(component.gameObject == null) && !(component.gameObject.scene != gameObject.scene))
                {
                    components.Add(component);
                }
            }
        }
        public static void GetComponentsInScene<T>(this Component component, List<T> components) where T : Component
        {
            if (component.gameObject == null)
            {
                return;
            }
            Scene arg_19_0 = component.gameObject.scene;
            T[] array = Object.FindObjectsOfType<T>();
            if (array.Length == 0)
            {
                return;
            }
            T[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                T t = array2[i];
                if (!(t == null) && !(t.gameObject == null) && !(t.gameObject.scene != component.gameObject.scene))
                {
                    components.Add(t);
                }
            }
        }
        public static void GetComponentsInScene(this Component component, Type type, List<Component> components)
        {
            if (component.gameObject == null)
            {
                return;
            }
            Scene arg_19_0 = component.gameObject.scene;
            Object[] array = Object.FindObjectsOfType(type);
            if (array.Length == 0)
            {
                return;
            }
            Object[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                Object @object = array2[i];
                Component component2;
                if ((component2 = (@object as Component)) != null && !(component2 == null) && !(component2.gameObject == null) && !(component2.gameObject.scene != component.gameObject.scene))
                {
                    components.Add(component2);
                }
            }
        }
        public static T GetComponentInParents<T>(this Component component, bool includeThis = false, bool includeInactive = true)
        {
            T t = default(T);
            if (includeThis)
            {
                t = component.GetComponent<T>();
                if (includeInactive)
                {
                    if (t != null)
                    {
                        return t;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour = t as MonoBehaviour;
                    if (monoBehaviour != null && monoBehaviour.isActiveAndEnabled && !component.Equals(monoBehaviour))
                    {
                        return t;
                    }
                }
            }
            Transform parent = component.transform.parent;
            while (parent != null)
            {
                t = parent.GetComponent<T>();
                if (includeInactive)
                {
                    if (t != null)
                    {
                        return t;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour2 = t as MonoBehaviour;
                    if (monoBehaviour2 != null && monoBehaviour2.isActiveAndEnabled)
                    {
                        return t;
                    }
                }
                parent = parent.parent;
            }
            return t;
        }
        public static Component GetComponentInParents(this Component component, Type type, bool includeThis = false, bool includeInactive = true)
        {
            Component component2 = null;
            if (includeThis)
            {
                component2 = component.GetComponent(type);
                if (includeInactive)
                {
                    if (component2 != null)
                    {
                        return component2;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour = component2 as MonoBehaviour;
                    if (monoBehaviour != null && monoBehaviour.isActiveAndEnabled && !component.Equals(monoBehaviour))
                    {
                        return component2;
                    }
                }
            }
            Transform parent = component.transform.parent;
            while (parent != null)
            {
                component2 = parent.GetComponent(type);
                if (includeInactive)
                {
                    if (component2 != null)
                    {
                        return component2;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour2 = component2 as MonoBehaviour;
                    if (monoBehaviour2 != null && monoBehaviour2.isActiveAndEnabled)
                    {
                        return component2;
                    }
                }
                parent = parent.parent;
            }
            return component2;
        }
        public static T GetComponentInParents<T>(this GameObject gameObject, bool includeThis = false, bool includeInactive = true)
        {
            T t = default(T);
            if (includeThis)
            {
                t = gameObject.GetComponent<T>();
                if (includeInactive)
                {
                    if (t != null)
                    {
                        return t;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour = t as MonoBehaviour;
                    if (monoBehaviour != null && monoBehaviour.isActiveAndEnabled && !gameObject.Equals(monoBehaviour.gameObject))
                    {
                        return t;
                    }
                }
            }
            Transform parent = gameObject.transform.parent;
            while (parent != null)
            {
                t = parent.GetComponent<T>();
                if (includeInactive)
                {
                    if (t != null)
                    {
                        return t;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour2 = t as MonoBehaviour;
                    if (monoBehaviour2 != null && monoBehaviour2.isActiveAndEnabled)
                    {
                        return t;
                    }
                }
                parent = parent.parent;
            }
            return t;
        }
        public static Component GetComponentInParents(this GameObject gameObject, Type type, bool includeThis = false, bool includeInactive = true)
        {
            Component component = null;
            if (includeThis)
            {
                component = gameObject.GetComponent(type);
                if (includeInactive)
                {
                    if (component != null)
                    {
                        return component;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour = component as MonoBehaviour;
                    if (monoBehaviour != null && monoBehaviour.isActiveAndEnabled && !gameObject.Equals(monoBehaviour.gameObject))
                    {
                        return component;
                    }
                }
            }
            Transform parent = gameObject.transform.parent;
            while (parent != null)
            {
                component = parent.GetComponent(type);
                if (includeInactive)
                {
                    if (component != null)
                    {
                        return component;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour2 = component as MonoBehaviour;
                    if (monoBehaviour2 != null && monoBehaviour2.isActiveAndEnabled)
                    {
                        return component;
                    }
                }
                parent = parent.parent;
            }
            return component;
        }
        public static T GetComponentInChildren<T>(this Component component, bool includeThis = false, bool includeInactive = true, bool debug = false)
        {
            T t = default(T);
            if (includeThis)
            {
                t = component.GetComponent<T>();
                if (includeInactive)
                {
                    if (t != null)
                    {
                        return t;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour = t as MonoBehaviour;
                    if (monoBehaviour != null && monoBehaviour.isActiveAndEnabled && !component.Equals(monoBehaviour))
                    {
                        return t;
                    }
                }
            }
            Transform[] componentsInChildren = component.GetComponentsInChildren<Transform>();
            if (componentsInChildren != null && componentsInChildren.Length != 0)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    Transform transform = componentsInChildren[i];
                    if (!(transform == component.transform))
                    {
                        t = transform.GetComponentInChildren<T>(true, includeInactive, false);
                        if (t != null)
                        {
                            return t;
                        }
                    }
                }
            }
            return t;
        }
        public static Component GetComponentInChildren(this Component component, Type type, bool includeThis = false, bool includeInactive = true, bool debug = false)
        {
            Component component2 = null;
            if (includeThis)
            {
                component2 = component.GetComponent(type);
                if (includeInactive)
                {
                    if (component2 != null)
                    {
                        return component2;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour = component2 as MonoBehaviour;
                    if (monoBehaviour != null && monoBehaviour.isActiveAndEnabled && !component.Equals(monoBehaviour))
                    {
                        return component2;
                    }
                }
            }
            Transform[] componentsInChildren = component.GetComponentsInChildren<Transform>();
            if (componentsInChildren != null && componentsInChildren.Length != 0)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    Transform transform = componentsInChildren[i];
                    if (!(transform == component.transform))
                    {
                        component2 = transform.GetComponentInChildren(type, true, includeInactive);
                        if (component2 != null)
                        {
                            return component2;
                        }
                    }
                }
            }
            return component2;
        }
        public static T GetComponentInChildren<T>(this GameObject gameObject, bool includeThis = false, bool includeInactive = true, bool debug = false)
        {
            T t = default(T);
            if (includeThis)
            {
                t = gameObject.GetComponent<T>();
                if (includeInactive)
                {
                    if (t != null)
                    {
                        return t;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour = t as MonoBehaviour;
                    if (monoBehaviour != null && monoBehaviour.isActiveAndEnabled && !gameObject.Equals(monoBehaviour.gameObject))
                    {
                        return t;
                    }
                }
            }
            Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
            if (componentsInChildren != null && componentsInChildren.Length != 0)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    Transform transform = componentsInChildren[i];
                    if (!(transform == gameObject.transform))
                    {
                        t = transform.GetComponentInChildren<T>(true, includeInactive, false);
                        if (t != null)
                        {
                            return t;
                        }
                    }
                }
            }
            return t;
        }
        public static Component GetComponentInChildren(this GameObject gameObject, Type type, bool includeThis = false, bool includeInactive = true)
        {
            Component component = null;
            if (includeThis)
            {
                component = gameObject.GetComponent(type);
                if (includeInactive)
                {
                    if (component != null)
                    {
                        return component;
                    }
                }
                else
                {
                    MonoBehaviour monoBehaviour = component as MonoBehaviour;
                    if (monoBehaviour != null && monoBehaviour.isActiveAndEnabled && !gameObject.Equals(monoBehaviour.gameObject))
                    {
                        return component;
                    }
                }
            }
            Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
            if (componentsInChildren != null && componentsInChildren.Length != 0)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    Transform transform = componentsInChildren[i];
                    if (!(transform == gameObject.transform))
                    {
                        component = transform.GetComponentInChildren(type, true, includeInactive);
                        if (component != null)
                        {
                            return component;
                        }
                    }
                }
            }
            return component;
        }
        public static void GetComponentsInChildren<T>(this Component component, List<T> components, bool includeThis = false, bool includeInactive = false)
        {
            if (components == null)
            {
                return;
            }
            if (includeThis)
            {
                List<T> list = new List<T>();
                component.GetComponents<T>(list);
                foreach (T current in list)
                {
                    if (!components.Contains(current))
                    {
                        components.Add(current);
                    }
                }
                //List.Release<T>(list);
            }
            Transform[] componentsInChildren = component.GetComponentsInChildren<Transform>(includeInactive);
            if (componentsInChildren != null && componentsInChildren.Length != 0)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    Transform transform = componentsInChildren[i];
                    if (!(transform == component.transform))
                    {
                        transform.GetComponentsInChildren(components, true, false);
                    }
                }
            }
        }
        public static void GetComponentsInChildren(this Component component, Type type, List<Component> components, bool includeThis = false, bool includeInactive = false)
        {
            if (components == null)
            {
                return;
            }
            if (includeThis)
            {
                List<Component> list = new List<Component>();
                component.GetComponents<Component>(list);
                foreach (Component current in list)
                {
                    if (!components.Contains(current))
                    {
                        components.Add(current);
                    }
                }
                //List.Release<Component>(list);
            }
            Transform[] componentsInChildren = component.GetComponentsInChildren<Transform>(includeInactive);
            if (componentsInChildren != null && componentsInChildren.Length != 0)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    Transform transform = componentsInChildren[i];
                    if (!(transform == component.transform))
                    {
                        transform.GetComponentsInChildren(type, components, true, false);
                    }
                }
            }
        }
        public static void GetComponentsInChildren<T>(this GameObject gameObject, List<T> components, bool includeThis = false, bool includeInactive = false)
        {
            if (components == null)
            {
                return;
            }
            if (includeThis)
            {
                List<T> list = new List<T>();
                gameObject.GetComponents<T>(list);
                foreach (T current in list)
                {
                    if (!components.Contains(current))
                    {
                        components.Add(current);
                    }
                }
                //List.Release<T>(list);
            }
            Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(includeInactive);
            if (componentsInChildren != null && componentsInChildren.Length != 0)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    Transform transform = componentsInChildren[i];
                    if (!(transform == gameObject.transform))
                    {
                        transform.GetComponentsInChildren(components, true, false);
                    }
                }
            }
        }
        public static void GetComponentsInChildren(this GameObject gameObject, Type type, List<Component> components, bool includeThis = false, bool includeInactive = false)
        {
            if (components == null)
            {
                return;
            }
            if (includeThis)
            {
                List<Component> list = new List<Component>();
                gameObject.GetComponents(type, list);
                foreach (Component current in list)
                {
                    if (!components.Contains(current))
                    {
                        components.Add(current);
                    }
                }
                //List.Release<Component>(list);
            }
            Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(includeInactive);
            if (componentsInChildren != null && componentsInChildren.Length != 0)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    Transform transform = componentsInChildren[i];
                    if (!(transform == gameObject.transform))
                    {
                        transform.GetComponentsInChildren(type, components, true, false);
                    }
                }
            }
        }
        public static T ValidateComponent<T>(this MonoBehaviour behaviour, ref T component) where T : MonoBehaviour
        {
            if (behaviour == null || behaviour.gameObject == null)
            {
                return component;
            }
            if (component != null && component.gameObject != null && behaviour.gameObject.scene != component.gameObject.scene)
            {
                component = default(T);
            }
            if (component == null)
            {
                component = behaviour.GetComponent<T>();
            }
            if (component == null)
            {
                component = behaviour.gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}
