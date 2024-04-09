using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class SpellAnimatorPropertySetter : MonoBehaviour, ISpellReferencer
    {
        public PhysicalSpell physicalSpell { get; set; }

        Animator animator;
        Spell spell;

        //TODO: When/if more properties need to be set, define which need to be set in editor
        public void SetAnimatorRange()
        {
            animator = GetComponent<AddAnimatorToCaster>().animator;
            spell = physicalSpell.correspondingSpell;
            SpellPointTargeter pointTargeter = GetComponentInParent<SpellPointTargeter>();

            float normalizedRange = UtilMath.Lmap(pointTargeter.fromSelfToTargetPoint.magnitude, 0.0f, spell.range, 0.0f, 1.0f);

            animator.SetFloat(SpellAnimatorParameters.Range, normalizedRange);
        }
    }
}