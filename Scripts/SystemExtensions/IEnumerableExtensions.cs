using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions
{
    public static class IEnumerableExtensions 
    {
        public static float Product(this IEnumerable<float> listToMultiply)
        {
            return listToMultiply.Aggregate(1.0f, (accumulatedValue, currentValue) => accumulatedValue * currentValue);
        }

        public static float Product(this IEnumerable<int> listToMultiply)
        {
            return listToMultiply.Aggregate(1, (accumulatedValue, currentValue) => accumulatedValue * currentValue);
        }
    }
}