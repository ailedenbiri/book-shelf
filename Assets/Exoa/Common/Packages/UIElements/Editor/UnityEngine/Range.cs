using System;
namespace UnityEngine
{
	public struct Range
	{
		public float min
		{
			get;
			set;
		}
		public float max
		{
			get;
			set;
		}
		public Range(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}
