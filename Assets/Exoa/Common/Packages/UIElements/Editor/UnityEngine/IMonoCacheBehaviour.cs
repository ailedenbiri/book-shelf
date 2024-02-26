using System;
namespace UnityEngine
{
	public interface IMonoCacheBehaviour
	{
		GameObject cachedGameObject
		{
			get;
		}
		Transform cachedTransform
		{
			get;
		}
		RectTransform cachedRectTransform
		{
			get;
		}
		bool active
		{
			get;
			set;
		}
	}
}
