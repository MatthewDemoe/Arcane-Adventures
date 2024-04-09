using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions
{
    public static class FloatExtensions
    {
        public static float Round(this float value, int decimals)
        {
            return (float)(Math.Round(value * Math.Pow(10, decimals)) / Math.Pow(10, decimals));
        }
    }
}