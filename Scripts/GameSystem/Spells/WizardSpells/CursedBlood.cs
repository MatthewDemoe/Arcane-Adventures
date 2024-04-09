using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class CursedBlood : Spell
    {
        public static CursedBlood Instance { get; } = new CursedBlood();
        private CursedBlood() : base() { }
        private CursedBlood(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new CursedBlood(ref _caster);
        }

        public override string name => "Cursed Blood";
        public override string effectDescription => "Place a vicious curse on the blood of your target. Their blood becomes toxic to them, damaging and slowing them from the inside.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        protected override Damage _damage => new Damage(3.0f * caster.stats.subtotalSpirit, Combat.DamageType.Magical);
        protected override bool scaleDamageOverDuration => true;

        public override float initialCost => 25;

        public override float channelCost => 0;
        public override float channelInterval => 1.0f;
        protected override float baseDuration => 5;
        public override bool hasDuration => true;

        public override float cooldown => 10;

        protected override float _range => 10.0f;
    }
}
