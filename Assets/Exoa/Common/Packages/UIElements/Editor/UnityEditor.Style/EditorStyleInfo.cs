using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Style.Internal;
using UnityEngine.UIElements;
namespace UnityEditor.Style
{
	[CreateAssetMenu(menuName = "Module/EditorStyle", fileName = "EditorStyleInfo", order = 0)]
	public sealed class EditorStyleInfo : ScriptableObject
	{
		private struct VisualElementStructure
		{
			public VisualElement visualElement;
			public float width;
			public VisualElementStructure(VisualElement element)
			{
				this.width = element.localBound.width;
				this.visualElement = element;
			}
		}
		private static readonly List<EditorStyleInfo> _editorStyleInfoCache;
		private static readonly Dictionary<VisualElement, EditorStyleInfo.VisualElementStructure> _responsiveElements;
		[SerializeField]
		private List<StyleSheet> _styleSheets = new List<StyleSheet>();
		[SerializeField]
		private List<StyleSheet> _styleSheetsForProSkin = new List<StyleSheet>();
		[SerializeField]
		private List<ResponsiveStyleSheet> _styleSheetsForResponsive = new List<ResponsiveStyleSheet>();
		private static int count
		{
			get;
			set;
		}
		public List<StyleSheet> styleSheets
		{
			get
			{
				return this._styleSheets;
			}
		}
		public List<StyleSheet> styleSheetsForProSkin
		{
			get
			{
				return this._styleSheetsForProSkin;
			}
		}
		public List<ResponsiveStyleSheet> styleSheetsForResponsive
		{
			get
			{
				return this._styleSheetsForResponsive;
			}
		}
		[InitializeOnLoadMethod]
		private static void EntryUpdate()
		{
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(EditorStyleInfo.Update));
		}
		private static void Update()
		{
			//Debug.Log("EditorStyleInfo._responsiveElements:" + EditorStyleInfo._responsiveElements);
			if (EditorStyleInfo._responsiveElements == null)
				return;
			if (EditorStyleInfo.count > 10)
			{
				foreach (KeyValuePair<VisualElement, EditorStyleInfo.VisualElementStructure> current in EditorStyleInfo._responsiveElements)
				{
					if (current.Key != null)
					{
						EditorStyleInfo.VisualElementStructure value = current.Value;
						if (!value.visualElement.localBound.width.EqualTo(value.width, 10f))
						{
							value.width = value.visualElement.localBound.width;
							EditorStyleInfo.InvokeResponsiveStyleSheetToVisualElement(current.Key);
						}
					}
				}
				EditorStyleInfo.count = 0;
			}
			EditorStyleInfo.count++;
		}
		public static void AddResponsiveElement(VisualElement element)
		{
			if (element == null || EditorStyleInfo._responsiveElements.ContainsKey(element))
			{
				return;
			}
			EditorStyleInfo._responsiveElements.Add(element, new EditorStyleInfo.VisualElementStructure(element));
		}
		public static void RemoveResponsiveElement(VisualElement element)
		{
			if (element == null || !EditorStyleInfo._responsiveElements.ContainsKey(element))
			{
				return;
			}
			EditorStyleInfo._responsiveElements.Remove(element);
		}
		public static void AddStyleSheetToVisualElement(VisualElement element)
		{
			if (element == null)
			{
				return;
			}
			if (EditorStyleInfo._editorStyleInfoCache.Count <= 0)
			{
				EditorStyleInfo._editorStyleInfoCache.AddRange(Resources.LoadAll<EditorStyleInfo>("EditorStyleInfo"));
			}
			if (EditorStyleInfo._editorStyleInfoCache.Count <= 0)
			{
				return;
			}
			foreach (EditorStyleInfo current in EditorStyleInfo._editorStyleInfoCache)
			{
				if (!(current == null))
				{
					foreach (StyleSheet current2 in current.styleSheets)
					{
						if (current2 == null)
						{
							string text;
							long num;
							if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(current2, out text, out num))
							{
								string text2 = AssetDatabase.GUIDToAssetPath(text);
								StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(text2);
								if (styleSheet != null && !element.styleSheets.Contains(styleSheet))
								{
									element.styleSheets.Add(styleSheet);
								}
							}
						}
						else
						{
							if (!element.styleSheets.Contains(current2))
							{
								element.styleSheets.Add(current2);
							}
						}
					}
					if (EditorGUIUtility.isProSkin)
					{
						foreach (StyleSheet current3 in current.styleSheetsForProSkin)
						{
							if (current3 == null)
							{
								string text3;
								long num2;
								if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(current3, out text3, out num2))
								{
									string text4 = AssetDatabase.GUIDToAssetPath(text3);
									StyleSheet styleSheet2 = AssetDatabase.LoadAssetAtPath<StyleSheet>(text4);
									if (styleSheet2 != null && !element.styleSheets.Contains(styleSheet2))
									{
										element.styleSheets.Add(styleSheet2);
									}
								}
							}
							else
							{
								if (!element.styleSheets.Contains(current3))
								{
									element.styleSheets.Add(current3);
								}
							}
						}
					}
				}
			}
		}
		private static void InvokeResponsiveStyleSheetToVisualElement(VisualElement element)
		{
			if (element == null)
			{
				return;
			}
			if (EditorStyleInfo._editorStyleInfoCache.Count <= 0)
			{
				EditorStyleInfo._editorStyleInfoCache.AddRange(Resources.LoadAll<EditorStyleInfo>("EditorStyleInfo"));
			}
			if (EditorStyleInfo._editorStyleInfoCache.Count <= 0)
			{
				return;
			}
			foreach (EditorStyleInfo current in EditorStyleInfo._editorStyleInfoCache)
			{
				if (!(current == null))
				{
					foreach (ResponsiveStyleSheet current2 in current.styleSheetsForResponsive)
					{
						if (current2 != null)
						{
							if (current2.type == ResponsiveStyleSheet.Type.GreaterThan)
							{
								if (element.localBound.width >= (float)current2.width)
								{
									goto IL_229;
								}
								using (List<StyleSheet>.Enumerator enumerator3 = current2.styleSheets.GetEnumerator())
								{
									while (enumerator3.MoveNext())
									{
										StyleSheet current3 = enumerator3.Current;
										if (current3 == null)
										{
											string text;
											long num;
											if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(current3, out text, out num))
											{
												string text2 = AssetDatabase.GUIDToAssetPath(text);
												StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(text2);
												if (styleSheet != null && element.styleSheets.Contains(styleSheet))
												{
													element.styleSheets.Remove(styleSheet);
												}
											}
										}
										else
										{
											if (element.styleSheets.Contains(current3))
											{
												element.styleSheets.Remove(current3);
											}
										}
									}
									continue;
								}
							}
							if (element.localBound.width > (float)current2.width)
							{
								using (List<StyleSheet>.Enumerator enumerator4 = current2.styleSheets.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										StyleSheet current4 = enumerator4.Current;
										if (current4 == null)
										{
											string text3;
											long num2;
											if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(current4, out text3, out num2))
											{
												string text4 = AssetDatabase.GUIDToAssetPath(text3);
												StyleSheet styleSheet2 = AssetDatabase.LoadAssetAtPath<StyleSheet>(text4);
												if (styleSheet2 != null && element.styleSheets.Contains(styleSheet2))
												{
													element.styleSheets.Remove(styleSheet2);
												}
											}
										}
										else
										{
											if (element.styleSheets.Contains(current4))
											{
												element.styleSheets.Remove(current4);
											}
										}
									}
									continue;
								}
							}
							IL_229:
							foreach (StyleSheet current5 in current2.styleSheets)
							{
								if (current5 == null)
								{
									string text5;
									long num3;
									if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(current5, out text5, out num3))
									{
										string text6 = AssetDatabase.GUIDToAssetPath(text5);
										StyleSheet styleSheet3 = AssetDatabase.LoadAssetAtPath<StyleSheet>(text6);
										if (styleSheet3 != null && !element.styleSheets.Contains(styleSheet3))
										{
											element.styleSheets.Add(styleSheet3);
										}
									}
								}
								else
								{
									if (!element.styleSheets.Contains(current5))
									{
										element.styleSheets.Add(current5);
									}
								}
							}
						}
					}
				}
			}
		}
		
		static EditorStyleInfo()
		{
			// Note: this type is marked as 'beforefieldinit'.
			//EditorStyleInfo.<count>k__BackingField = 0;
			EditorStyleInfo._editorStyleInfoCache = new List<EditorStyleInfo>();
			EditorStyleInfo._responsiveElements = new Dictionary<VisualElement, EditorStyleInfo.VisualElementStructure>();
			//EditorStyleInfo._responsiveElements = Dictionary.Create<VisualElement, EditorStyleInfo.VisualElementStructure>();
		}
	}
}
