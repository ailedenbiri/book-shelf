using System;
using UnityEngine;
namespace Exoa.Utils
{
    public static class QuaternionExtenseions
    {
        public static readonly Quaternion Zero = Quaternion.Euler(0f, 0f, 0f);
        public static readonly Quaternion RightDirection = Quaternion.AngleAxis(90f, new Vector3(0f, 1f, 0f));
        public static readonly Quaternion LeftDirection = Quaternion.AngleAxis(-90f, new Vector3(0f, 1f, 0f));
        public static readonly Quaternion ForwardRightDirection = Quaternion.AngleAxis(45f, new Vector3(0f, 1f, 0f));
        public static readonly Quaternion ForwardLeftDirection = Quaternion.AngleAxis(-45f, new Vector3(0f, 1f, 0f));
        public static readonly Quaternion BackDirection = Quaternion.AngleAxis(180f, new Vector3(0f, 1f, 0f));
        public static readonly Quaternion BackRightDirection = Quaternion.AngleAxis(135f, new Vector3(0f, 1f, 0f));
        public static readonly Quaternion BackLeftDirection = Quaternion.AngleAxis(-135f, new Vector3(0f, 1f, 0f));
    }
}
