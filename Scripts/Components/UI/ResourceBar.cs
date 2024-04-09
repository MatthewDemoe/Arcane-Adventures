using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public enum Resource { Health, Spirit }

    public abstract class ResourceBar : MonoBehaviour
    {
        [SerializeField]
        protected Resource resourceToTrack;
        public Resource trackedResource => resourceToTrack;

        protected CreatureReference creatureReference;

        protected float previousScale = 1.0f;

        protected const float LagDelay = 0.5f;

        protected const float LaggingEmptyTime = 1.0f;
        protected float laggingTimer = 0.0f;

        protected float currentResourcePercent => (resourceToTrack == Resource.Health) ?
            creatureReference.creature.stats.currentHealthPercent :
            creatureReference.creature.stats.currentSpiritPercent;

        protected virtual void Start()
        {
            creatureReference = GetComponentInParent<CreatureReference>();
        }

        public abstract void ProcessResourceChange();
    }
}