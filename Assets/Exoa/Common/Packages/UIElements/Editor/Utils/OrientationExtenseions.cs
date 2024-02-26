using System;
using UnityEngine;
namespace Exoa.Utils
{
    public static class OrientationExtenseions
    {
        public static DeviceOrientation Get()
        {
            if (Screen.safeArea.width < Screen.safeArea.height)
            {
                return DeviceOrientation.Portrait;
            }
            return DeviceOrientation.LandscapeLeft;
        }
        public static DeviceOrientation Get(float width, float height)
        {
            if (width < height)
            {
                return DeviceOrientation.Portrait;
            }
            return DeviceOrientation.LandscapeLeft;
        }
    }
}
