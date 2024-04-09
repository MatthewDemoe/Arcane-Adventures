using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class EnhancedRange : Spell
    {
        public static EnhancedRange Instance { get; } = new EnhancedRange();
        private EnhancedRange() : base() { }
        private EnhancedRange(ref Creature _caster) : base(ref _caster)
        {
            spellEffects = new List<SpellEffect>()
            {
                new BuffNextSpell(ref caster, SpellProperties.Range, 10.0f),
                new BuffNextSpell(ref caster, SpellProperties.AreaOfEffect, 5.0f),
                new BuffNextSpell(ref caster, SpellProperties.Force, 0.25f),
            };
        }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new EnhancedRange(ref _caster);
        }

        public override string name => "Enhanced Range";
        public override string effectDescription => "Enhance your spells to increase their range, speed, and AOE.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Enhancement;
        public override float initialCost => 10.0f;

        public override float channelCost => 0.0f;
        public override bool hasDuration => false;

        public override float cooldown => 10;

        public override bool StartCast(Spell spell)
        {
            bool canStartCast = base.StartCast(spell);

            if (canStartCast)
            {
                persistentEffectIsActive = true;                
            }

            return canStartCast;
        }

        public override void EndCast()
        {
            base.EndCast();
            character.OnStartCastSpell += SetPersistentEffectFalse;
        }

        private void SetPersistentEffectFalse(Spell spell)
        {
            persistentEffectIsActive = false;
            character.OnStartCastSpell -= SetPersistentEffectFalse;
        }
    }
}
