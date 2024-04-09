using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.UnityExtensions
{
    public static class Vector3Extensions
    {
        public static Vector3 GetClosestPointOnLine(this Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            var span = lineEnd - lineStart;
            var normalizedScalar = Vector3.Dot(point - lineStart, span) / span.sqrMagnitude;
            var closestPoint = lineStart + Mathf.Clamp01(normalizedScalar) * span;

            return closestPoint;
        }
    }
}


