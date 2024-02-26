using System;
using System.Collections.Generic;
using UnityEngine;
namespace Exoa.Utils
{
    public static class ResourcesExtenseions
    {
        public static T LoadAny<T>(string name = null, Func<T, bool> predicate = null) where T : UnityEngine.Object
        {
            T[] array = Resources.LoadAll<T>(name ?? "");
            if (predicate != null)
            {
                T[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    T t = array2[i];
                    if (!(t == null) && predicate(t))
                    {
                        return t;
                    }
                }
            }
            if (array != null && array.Length != 0)
            {
                return array[0];
            }
            return default(T);
        }
        public static List<T> LoadAll<T>(string name = null, Func<T, bool> predicate = null) where T : UnityEngine.Object
        {
            List<T> list = new List<T>();
            T[] array = Resources.LoadAll<T>(name ?? "");
            T[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                T t = array2[i];
                if (!(t == null))
                {
                    if (predicate != null && predicate(t))
                    {
                        list.Add(t);
                    }
                    else
                    {
                        if (predicate == null)
                        {
                            list.Add(t);
                        }
                    }
                }
            }
            return list;
        }
    }
}
