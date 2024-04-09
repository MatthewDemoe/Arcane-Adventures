using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class GiantsCleave : Spell
    {
        public static GiantsCleave Instance { get; } = new GiantsCleave();
        private GiantsCleave() { caster = null; }
        private GiantsCleave(ref Creature _caster) 
        { 
            caster = _caster;
            spellEffects = new List<SpellEffect>() { new BuffNextWeaponAttack(ref caster, damage: 1.5f), };
        }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new GiantsCleave(ref _caster);
        }

        public override string name => "Giant's Cleave";
        public override string effectDescription => "Spirit strengthens you, allowing a giant swing of your weapon to bypass blocks and send the target flying in the direction of your swing, stunning them if they hit a surface.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Control;
        public override float initialCost => 25;
        public override float knockbackDistance => 3.0f;

        public override bool hasDuration => false;

        public override float cooldown => 20;

    }
}
