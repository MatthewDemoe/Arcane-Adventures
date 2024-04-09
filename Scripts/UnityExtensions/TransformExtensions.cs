using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.UnityExtensions
{
    public static class TransformExtensions
    {
        public static Vector3 TransformPoint(this Transform transform, Vector3 position, bool ignoreScale)
        {
            if (!ignoreScale)
            {
                return transform.TransformPoint(position);
            }

            return Matrix4x4
                .TRS(transform.position, transform.rotation, Vector3.one)
                .MultiplyPoint3x4(position);
        }
    }
}


