using System;

namespace UnityToolkit
{
    public  static class MathExtensions
    {
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static float ClampAngle(this float angle, float min, float max)
        {
            if (angle < -180f)
                angle += 360f;
            if (angle > 180f)
                angle -= 360f;
            return Math.Clamp(angle, min, max);
        }
    }
}