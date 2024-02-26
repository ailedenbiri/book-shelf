using System;
namespace UnityEngine.Events
{
	public struct OrientationEvent
	{
		public Component target
		{
			get;
			set;
		}
		public Action<DeviceOrientation> action
		{
			get;
			set;
		}
		public OrientationEvent(Component target, Action<DeviceOrientation> action)
		{
			this.target = target;
			this.action = action;
		}
	}
}
