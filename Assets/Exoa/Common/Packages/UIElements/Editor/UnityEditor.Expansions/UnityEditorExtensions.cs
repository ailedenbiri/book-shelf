using System;
using System.Collections;
using System.Reflection;
namespace UnityEditor
{
	public static class UnityEditorExtensions
	{
		public static object GetTargetObject(this SerializedProperty property)
		{
			if (property == null)
			{
				return null;
			}
			string text = property.propertyPath.Replace(".Array.data[", "[");
			object obj = property.serializedObject.targetObject;
			string[] array = text.Split(new char[]
			{
				'.'
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				if (text2.Contains("["))
				{
					string name = text2.Substring(0, text2.IndexOf("["));
					int index = Convert.ToInt32(text2.Substring(text2.IndexOf("[")).Replace("[", "").Replace("]", ""));
					obj = UnityEditorExtensions.GetValue_Imp(obj, name, index);
				}
				else
				{
					obj = UnityEditorExtensions.GetValue_Imp(obj, text2);
				}
			}
			return obj;
		}
		private static object GetValue_Imp(object source, string name)
		{
			if (source == null)
			{
				return null;
			}
			Type type = source.GetType();
			while (type != null)
			{
				FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					return field.GetValue(source);
				}
				PropertyInfo property = type.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					return property.GetValue(source, null);
				}
				type = type.BaseType;
			}
			return null;
		}
		private static object GetValue_Imp(object source, string name, int index)
		{
			IEnumerable enumerable;
			if ((enumerable = (UnityEditorExtensions.GetValue_Imp(source, name) as IEnumerable)) == null)
			{
				return null;
			}
			IEnumerator enumerator = enumerable.GetEnumerator();
			for (int i = 0; i <= index; i++)
			{
				if (!enumerator.MoveNext())
				{
					return null;
				}
			}
			return enumerator.Current;
		}
	}
}
