using System;
using UnityEngine;
namespace Exoa.Utils
{
    public static class VectorExtenseions
    {
        public static Vector2 ELerp(Vector2 a, Vector2 b, float t)
        {
            return (b - a) * t + a;
        }
        public static Vector3 ELerp(Vector3 a, Vector3 b, float t)
        {
            return (b - a) * t + a;
        }
        public static Vector4 ELerp(Vector4 a, Vector4 b, float t)
        {
            return (b - a) * t + a;
        }
    }
}
