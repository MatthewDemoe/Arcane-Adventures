using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class TakeDown : Spell
    {
        public static TakeDown Instance { get; } = new TakeDown();
        private TakeDown() { caster = null; }
        private TakeDown(ref Creature _caster) : base(ref _caster)
        {
            spellEffects = new List<SpellEffect>() { new BuffNextWeaponAttack(ref caster, damage: 3.0f, strikeType: Combat.StrikeType.Perfect), };
        }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new TakeDown(ref _caster);
        }

        public override string name => "Take Down";
        public override string effectDescription => "Spirit fills your weapon with otherworldly force, damaging and knocking down your opponent.";
        public override int spellLevel => 2;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 50;

        public override bool hasDuration => true;
        protected override float baseDuration => 10;

        public override float cooldown => 10;
    }
}
