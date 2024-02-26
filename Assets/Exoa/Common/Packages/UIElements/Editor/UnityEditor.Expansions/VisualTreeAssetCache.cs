using System;
using System.Collections.Generic;
//using System.Path;
using System.Pool.Internal;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Internal;
namespace UnityEngine.UIElements.Pool
{
    internal static class VisualTreeAssetCache
    {
        [CompilerGenerated]
        [Serializable]
        private sealed class Cache
        {
            public static readonly VisualTreeAssetCache.Cache c = new VisualTreeAssetCache.Cache();
            internal VisualTreeAsset GetByKey(string key)
            {
                if (key.IsNullOrEmpty())
                {
                    return null;
                }
                if (key.Contains("/"))
                {
                    VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>((key != null) ? key.Replace(Application.dataPath, "Assets").TrimEnd(new string[]
                    {
                        "/"
                    }) : null);
                    if (visualTreeAsset != null)
                    {
                        return visualTreeAsset;
                    }
                }
                IEnumerable<string> enumerable = AssetFiles.Get();
                foreach (string current in enumerable)
                {
                    //Debug.Log("current:" + current + " " + key + ".uxml");

                    if (!current.IsNullOrEmpty() && current.EndsWith(key + ".uxml", StringComparison.Ordinal))
                    {
                        VisualTreeAsset visualTreeAsset2 = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>((current != null) ? current.Replace(Application.dataPath, "Assets").TrimEnd(new string[]
                        {
                            "/"
                        }) : null);
                        if (visualTreeAsset2 != null)
                        {
                            VisualTreeAsset result = visualTreeAsset2;
                            return result;
                        }
                    }
                }
                //string text = key.Last('.');
                string text = key.Substring(key.LastIndexOf('.') + 1);
                //Debug.Log("key:" + key + " text:" + text);

                if (text.IsNullOrEmpty())
                {
                    return null;
                }
                foreach (string current2 in enumerable)
                {
                    //Debug.Log("current2:" + current2 + " " + text + ".uxml");

                    if (!current2.IsNullOrEmpty() && current2.EndsWith(text + ".uxml", StringComparison.Ordinal))
                    {
                        VisualTreeAsset visualTreeAsset3 = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>((current2 != null) ? current2.Replace(Application.dataPath, "Assets").TrimEnd(new string[]
                        {
                            "/"
                        }) : null);
                        if (visualTreeAsset3 != null)
                        {
                            VisualTreeAsset result = visualTreeAsset3;
                            return result;
                        }
                    }
                }
                if (text.Contains("+"))
                {
                    //text = key.Last('+');
                    text = key.Substring(key.LastIndexOf('+') + 1);
                    //Debug.Log("key:" + key + " text:" + text);
                    if (text.IsNullOrEmpty())
                    {
                        return null;
                    }
                    foreach (string current3 in enumerable)
                    {
                        //Debug.Log("current3:" + current3 + " " + text + ".uxml");
                        if (!current3.IsNullOrEmpty() && current3.EndsWith(text + ".uxml", StringComparison.Ordinal))
                        {
                            VisualTreeAsset visualTreeAsset4 = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>((current3 != null) ? current3.Replace(Application.dataPath, "Assets").TrimEnd(new string[]
                            {
                                "/"
                            }) : null);
                            if (visualTreeAsset4 != null)
                            {
                                VisualTreeAsset result = visualTreeAsset4;
                                return result;
                            }
                        }
                    }
                }
                return null;
            }
        }
        private static readonly ObjectCache<string, VisualTreeAsset> _pool = new ObjectCache<string, VisualTreeAsset>(new Func<string, VisualTreeAsset>(VisualTreeAssetCache.Cache.c.GetByKey), null);
        public static VisualTreeAsset Get(string key)
        {
            return VisualTreeAssetCache._pool.Get(key);
        }
    }
}
