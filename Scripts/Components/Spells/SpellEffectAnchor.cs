using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class SpellEffectAnchor : MonoBehaviour, ISpellReferencer
    {
        [SerializeField]
        float _spellEffectScale = 1.0f;
        public float spellEffectScale => _spellEffectScale;

        public PhysicalSpell physicalSpell { get; set; }
    }
}