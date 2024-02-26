using Exoa.Utils;
using System;
public static class RandomFactory
{
    public static Random Create(int seed = 0, int rev = 0)
    {
        if (seed == 0)
        {
            DateTime utcNow = DateTime.UtcNow;
            return new Random(utcNow.Millisecond + utcNow.Second + utcNow.Day + utcNow.Month + utcNow.Year + rev);
        }
        return new Random(seed + rev);
    }
    public static int Range(this Random rand, int min, int max)
    {
        Exoa.Utils.Math.ValidHighLow(ref min, ref max);
        return (int)System.Math.Round((double)((float)(rand.NextDouble() * (double)(max - min) + (double)min))).Limit((double)min, (double)max);
    }
    public static long Range(this Random rand, long min, long max)
    {
        Exoa.Utils.Math.ValidHighLow(ref min, ref max);
        return ((long)System.Math.Round((double)((float)(rand.NextDouble() * (double)(max - min) + (double)min)))).Limit(min, max);
    }
    public static float Range(this Random rand, float min, float max)
    {
        Exoa.Utils.Math.ValidHighLow(ref min, ref max);
        return ((float)(rand.NextDouble() * (double)(max - min) + (double)min)).Limit(min, max);
    }
    public static double Range(this Random rand, double min, double max)
    {
        Exoa.Utils.Math.ValidHighLow(ref min, ref max);
        return (rand.NextDouble() * (max - min) + min).Limit(min, max);
    }
}
