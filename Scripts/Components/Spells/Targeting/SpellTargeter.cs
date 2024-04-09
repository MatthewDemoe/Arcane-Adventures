using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting
{
    public class SpellTargeter : MonoBehaviour
    {
        protected SpellCaster spellCaster;
        protected PhysicalWeapon playerWeapon;
        HandSide handSide;

        protected bool isSelfTargeted = false;

        public virtual Vector3 targetDirection => GetTargetDirection();

        public virtual Vector3 targetPoint => GetTargetDirection();

        public Spell correspondingSpell { get; protected set; } = null;

        public virtual void InitializeWithSpellInformation(Spell spell, SpellCaster spellCaster, HandSide handSide)
        {
            this.spellCaster = spellCaster;
            this.handSide = handSide;
            isSelfTargeted = spell.isSelfTargeted;
            correspondingSpell = spell;
        }

        protected Vector3 GetTargetDirection()
        {
            if (isSelfTargeted)
                return Vector3.down;

            Vector3 forwardDirection = spellCaster.WeaponTransform(handSide).up * (handSide == HandSide.Right ? 1.0f : -1.0f);
            Vector3 upDirection = -spellCaster.WeaponTransform(handSide).right;

            return Vector3.RotateTowards(upDirection, forwardDirection, spellCaster.GetAimAngleAdjust, 0.0f);
        }
    }
}