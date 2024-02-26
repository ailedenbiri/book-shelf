using UnityEngine;

namespace Maskable
{
	public interface IMasking
	{
		bool isAlive { get; }
		bool isMaskingEnabled { get; }
		// May return null.
		Material GetReplacement(Material original);
		void ReleaseReplacement(Material replacement);
		void UpdateTransformChildren(Transform transform);
	}
}
