using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class SharpenedStance : Spell
    {
        public static SharpenedStance Instance { get; } = new SharpenedStance();
        private SharpenedStance() 
        { 
            caster = null;
        }

        private SharpenedStance(ref Creature _caster) : base(ref _caster)
        { 
            spellEffects = new List<SpellEffect>() { new UpkeepDamageBuff(ref caster, damage: 1.05f), };
        }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new SharpenedStance(ref _caster);
        }

        public override string name => "Sharpened Stance";
        public override string effectDescription => "Spirit flows through your body, sharpening all your senses. increases damage and penetration.";
        public override int spellLevel => 2;

        public override SpellType spellType => SpellType.Enhancement;
        
        public override bool hasUpkeep => true;
        public override float upkeepCost => 7.0f;
        public override float initialCost => 0.0f;

        public override float cooldown => 8;
    }
}
