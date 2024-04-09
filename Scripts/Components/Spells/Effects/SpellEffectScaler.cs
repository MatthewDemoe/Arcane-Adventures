using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class SpellEffectScaler : MonoBehaviour
    {
        [SerializeField]
        private bool scaleWidth = true;

        [SerializeField]
        private bool scaleHeight = true;

        [SerializeField]
        private float scaleWidthModifier = 1.0f;

        [SerializeField]
        private float scaleHeightModifier = 1.0f;

        CreatureReference creatureReference;

        void Start()
        {
            creatureReference = GetComponentInParent<CreatureReference>();

            ScaleToCreatureSize();
        }

        private void ScaleToCreatureSize()
        {
            SpellEffectAnchor spellEffectAnchor = creatureReference.GetComponentInChildren<SpellEffectAnchor>();

            transform.localScale = new Vector3(
                transform.localScale.x * (!scaleWidth || spellEffectAnchor.spellEffectScale == 1.0f ? 1.0f : spellEffectAnchor.spellEffectScale * scaleWidthModifier),
                transform.localScale.y * (!scaleHeight || spellEffectAnchor.spellEffectScale == 1.0f ? 1.0f : spellEffectAnchor.spellEffectScale * scaleHeightModifier),
                transform.localScale.z * (!scaleWidth || spellEffectAnchor.spellEffectScale == 1.0f ? 1.0f : spellEffectAnchor.spellEffectScale * scaleWidthModifier));
        }
    }
}