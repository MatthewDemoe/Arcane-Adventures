using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class TitansGrip : Spell
    {
        public static TitansGrip Instance { get; } = new TitansGrip();
        private TitansGrip() { caster = null; }
        private TitansGrip(ref Creature _caster)
        {
            caster = _caster;
            spellEffects = new List<SpellEffect>() { new UpkeepDamageBuff(ref caster, damage: 3.0f), };
        }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new TitansGrip(ref _caster);
        }

        public override string name => "Titan's Grip";
        public override string effectDescription => "Spirit strengthens you, allowing a giant swing of your weapon to bypass blocks and send the target flying in the direction of your swing, stunning them if they hit a surface.";
        public override int spellLevel => 2;

        public override SpellType spellType => SpellType.Enhancement;
        public override float initialCost => 0.0f;
        public override float knockbackDistance => 3.0f;

        public override bool hasUpkeep => true;
        public override float upkeepCost => 7.0f;

        public override float cooldown => 20;

    }
}
