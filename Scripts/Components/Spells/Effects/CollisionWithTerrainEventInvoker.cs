using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    [RequireComponent(typeof(Collider))]
    public class CollisionWithTerrainEventInvoker : MonoBehaviour
    {
        public UnityEvent OnTerrainCollision = new UnityEvent();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != (int)Layers.Environment)
                return;

            OnTerrainCollision.Invoke();
            Destroy(this);
        }
    }
}