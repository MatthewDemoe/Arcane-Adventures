using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.PostProcessing
{
    [RequireComponent(typeof(Volume))]
    public class DeathEffect : MonoBehaviour
    {
        private Volume volume;
        private ColorCurves colorCurves;
        private SplitToning splitToning;
        private bool hasDeathEffectBeenActivated = false;

        private void Start()
        {
            volume = gameObject.GetComponent<Volume>();

            if (!volume.profile.TryGet(out colorCurves) || !volume.profile.TryGet(out splitToning))
            {
                throw new System.Exception("Expected overrides not present on volume");
            }
        }

        private void Update()
        {
            if (!hasDeathEffectBeenActivated && PlayerCharacterReference.Instance.creature.isDead)
            {
                colorCurves.active = true;
                splitToning.active = true;
                hasDeathEffectBeenActivated = true;
            }
        }
    }
}