using System;
using UnityEngine;

namespace Exoa.Utils
{
    public static class ColorExtenseions
    {
        public static Color ELerp(Color a, Color b, float t)
        {
            return (b - a) * t + a;
        }
    }
}
