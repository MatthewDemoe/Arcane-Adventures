using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class HeavyBlow : Spell
    {
        public static HeavyBlow Instance { get; } = new HeavyBlow();
        private HeavyBlow() { caster = null; }
        private HeavyBlow(ref Creature _caster) 
        { 
            caster = _caster;
            spellEffects = new List<SpellEffect>() { new BuffNextWeaponAttack(ref caster, damage: 2.0f), };
        }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new HeavyBlow(ref _caster);
        }

        public override string name => "Heavy Blow";
        public override string effectDescription => "Infuse your weapon with spirit, increasing it’s weight and damage for one attack.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 25;

        public override bool hasDuration => false;

        public override float cooldown => 5;
    }
}
