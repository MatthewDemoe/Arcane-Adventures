using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Common;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public class SpiritBar : MonoBehaviour
    {
        [SerializeField] private RectTransform spiritRepresentation;
        [SerializeField] private bool lookAtCamera = true;
        private CreatureReference creatureReference;

        private void Start()
        {
            creatureReference = GetComponentInParent<CreatureReference>();

            var canvas = this.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;

            if (lookAtCamera)
            {
                var lookAt = this.gameObject.AddComponent<LookAt>();
                lookAt.target = Camera.main.gameObject;
            }
        }

        private void Update()
        {
            if (creatureReference.creature is Creature && creatureReference.creature.stats.maxSpirit > 0)
            {
                var spiritPercentLeftInDecimal = creatureReference.creature.stats.currentSpirit / (float)creatureReference.creature.stats.maxSpirit;
                spiritRepresentation.localScale = new Vector3(spiritPercentLeftInDecimal, spiritRepresentation.localScale.y, spiritRepresentation.localScale.z);
            }
        }
    }
}
