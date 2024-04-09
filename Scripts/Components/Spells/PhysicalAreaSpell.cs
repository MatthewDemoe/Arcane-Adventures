using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class PhysicalAreaSpell : PhysicalSpell
    {
        public bool isSelfTargeted { get; private set; } = false;
        public override Type playerSpellTargeter => typeof(SpellAreaTargeter);

        public override void InitializeSpellInformation(Spell spell, SpellCaster playerSpellCaster, HandSide handSide)
        {
            base.InitializeSpellInformation(spell, playerSpellCaster, handSide);

            isSelfTargeted = spell.isSelfTargeted;

            if(isSelfTargeted)
                spellSource = playerSpellCaster.gameObject.transform;
        }
    }
}