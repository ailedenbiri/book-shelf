using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Pool.Internal;

namespace UnityEditor.Internal
{
    public class AssetFiles : AssetPostprocessor
    {

        private static List<string> _assetDatabase
        {
            get;
            set;
        }
        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            if (AssetFiles._assetDatabase != null)
            {
                //AssetFiles._assetDatabase.Release<string>();
            }
            AssetFiles.UpdateList();
        }
        public static IEnumerable<string> Get()
        {
            if (AssetFiles._assetDatabase == null)
            {
                AssetFiles.UpdateList();
            }
            return AssetFiles._assetDatabase;
        }
        private static void UpdateList()
        {
            IEnumerable<string> enumerable = Directory.EnumerateFiles(Application.dataPath, "*", SearchOption.AllDirectories);
            //AssetFiles._assetDatabase = new List<string>();
            AssetFiles._assetDatabase = new List<string>();
            foreach (string current in enumerable)
            {
                if (!current.IsNullOrEmpty() && !current.EndsWith(".meta", StringComparison.Ordinal))
                {
                    AssetFiles._assetDatabase.Add(current.Replace("\\", "/"));
                }
            }
        }
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (importedAssets != null && importedAssets.Length != 0)
            {
                for (int i = 0; i < importedAssets.Length; i++)
                {
                    string text = importedAssets[i];
                    if (!text.IsNullOrEmpty())
                    {
                        string item = (Application.dataPath + text.TrimStart(new string[]
                        {
                            "Assets"
                        })).Replace("\\", "/");
                        if (!AssetFiles._assetDatabase.Contains(item))
                        {
                            AssetFiles._assetDatabase.Add(item);
                        }
                    }
                }
            }
            if (deletedAssets != null && deletedAssets.Length != 0)
            {
                for (int j = 0; j < deletedAssets.Length; j++)
                {
                    string text2 = deletedAssets[j];
                    if (!text2.IsNullOrEmpty())
                    {
                        string item2 = (Application.dataPath + text2.TrimStart(new string[]
                        {
                            "Assets"
                        })).Replace("\\", "/");
                        AssetFiles._assetDatabase.Remove(item2);
                    }
                }
            }
            if (movedAssets != null && movedAssets.Length != 0 && movedFromAssetPaths != null && movedFromAssetPaths.Length != 0)
            {
                for (int k = 0; k < movedFromAssetPaths.Length; k++)
                {
                    string text3 = movedAssets[k];
                    string text4 = movedFromAssetPaths[k];
                    if (!text3.IsNullOrEmpty() && !text4.IsNullOrEmpty())
                    {
                        string item3 = (Application.dataPath + text3.TrimStart(new string[]
                        {
                            "Assets"
                        })).Replace("\\", "/");
                        string item4 = (Application.dataPath + text4.TrimStart(new string[]
                        {
                            "Assets"
                        })).Replace("\\", "/");
                        AssetFiles._assetDatabase.Remove(item4);
                        if (!AssetFiles._assetDatabase.Contains(item3))
                        {
                            AssetFiles._assetDatabase.Add(item3);
                        }
                    }
                }
            }
        }
    }
}
