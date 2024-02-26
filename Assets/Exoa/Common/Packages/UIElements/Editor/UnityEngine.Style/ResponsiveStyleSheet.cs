using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
namespace UnityEngine.Style.Internal
{
	[Serializable]
	public class ResponsiveStyleSheet : SerializableBehaviour
	{
		public enum Type
		{
			GreaterThan,
			LessThan
		}
		[SerializeField]
		public ResponsiveStyleSheet.Type type;
		[SerializeField]
		public int width;
		[SerializeField]
		private List<StyleSheet> _styleSheets = new List<StyleSheet>();
		public List<StyleSheet> styleSheets
		{
			get
			{
				return this._styleSheets;
			}
		}
	}
}
