using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using UnityEngine;
using System;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    [RequireComponent(typeof(TargetedSpellEffects))]
    public class PhysicalTargetedSpell : PhysicalSpell
    {
        public override Type playerSpellTargeter => typeof(SpellSingleTargeter);
        public override Type enemySpellTargeter => typeof(EnemySingleTargeter);

        public override void InitializeSpellInformation(Spell spell, SpellCaster playerSpellCaster, HandSide handSide)
        {
            base.InitializeSpellInformation(spell, playerSpellCaster, handSide);

            spellSource = playerSpellCaster.SpellSourceTransform(handSide);
            OnEndCast.AddListener(() => playerSpellCaster.spellTargetTypeSwitcher.DestroyCurrentTargeter(handSide));
        }
    }
}