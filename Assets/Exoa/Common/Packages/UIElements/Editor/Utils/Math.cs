using System;
namespace Exoa.Utils
{
    public static class Math
    {
        public static float ELerp(float a, float b, float t)
        {
            return (b - a) * t + a;
        }
        public static int Mod(int a, int b)
        {
            if (a >= 0)
            {
                return a % b;
            }
            while (a < 0)
            {
                a += b;
            }
            return a;
        }
        public static void ValidHighLow(ref short low, ref short high)
        {
            if (low <= high)
            {
                return;
            }
            short num = high;
            high = low;
            low = num;
        }
        public static void ValidHighLow(ref int low, ref int high)
        {
            if (low <= high)
            {
                return;
            }
            int num = high;
            high = low;
            low = num;
        }
        public static void ValidHighLow(ref long low, ref long high)
        {
            if (low <= high)
            {
                return;
            }
            long num = high;
            high = low;
            low = num;
        }
        public static void ValidHighLow(ref float low, ref float high)
        {
            if (low <= high)
            {
                return;
            }
            float num = high;
            high = low;
            low = num;
        }
        public static void ValidHighLow(ref double low, ref double high)
        {
            if (low <= high)
            {
                return;
            }
            double num = high;
            high = low;
            low = num;
        }
        public static void ValidHighLow(ref decimal low, ref decimal high)
        {
            if (low <= high)
            {
                return;
            }
            decimal num = high;
            high = low;
            low = num;
        }
        public static float GetInclinationRandom(float fineness = 1000000f, int strength = 3)
        {
            float num = 0f;
            strength = strength.LimitLow(1);
            fineness = fineness.LimitLow(100f);
            for (int i = 0; i < strength; i++)
            {
                num = RandomFactory.Create(DateTime.UtcNow.Millisecond + i, 0).Range(0f, fineness);
            }
            return num / (float)strength;
        }
    }
}
