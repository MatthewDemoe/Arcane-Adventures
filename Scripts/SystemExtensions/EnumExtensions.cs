using System;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions
{
    public static class EnumExtensions
    {
        public static IReadOnlyList<T> GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}