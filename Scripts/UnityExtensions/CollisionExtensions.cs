using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.UnityExtensions
{
    public static class CollisionExtensions
    {
        public static bool TryGetCollidingComponent<T>(this Collision collision, out T component, out Collider thisCollider) where T : MonoBehaviour
        {
            component = GetCollidingComponent<T>(collision, out thisCollider);

            return component != null;
        }

        public static T GetCollidingComponent<T>(this Collision collision, out Collider thisCollider) where T : MonoBehaviour
        {
            T component = null;
            thisCollider = null;

            for (var i = 0; i < collision.contactCount; i++)
            {
                var contactPoint = collision.GetContact(i);
                component = contactPoint.otherCollider.GetComponent<T>();

                if (component != null)
                {
                    thisCollider = contactPoint.thisCollider;
                    break;//TODO: If multiple, sort out which one is the "most valid" one (or pass in argument on how to determine).
                }
            }

            return component;
        }
    }
}


