using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class RecklessAbandon : Spell
    {
        public static RecklessAbandon Instance { get; } = new RecklessAbandon();
        private RecklessAbandon() { caster = null; }
        private RecklessAbandon(ref Creature _caster) 
        {
            caster = _caster; 
            spellEffects = new List<SpellEffect>() 
            { 
                new BuffNextWeaponAttack(ref caster, 2.0f),
            };
        }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new RecklessAbandon(ref _caster);
        }

        public override string name => "Reckless Abandon";
        public override string effectDescription => "Throw caution to the wind and make one massive attack that damages and dazes its victim, but leaves you vulnerable after.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 25;

        public override bool hasDuration => false;

        public override float cooldown => 10;
    }
}
