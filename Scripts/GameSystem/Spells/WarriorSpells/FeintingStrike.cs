using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class FeintingStrike : Spell
    {
        public static FeintingStrike Instance { get; } = new FeintingStrike();
        private FeintingStrike() { caster = null; }
        private FeintingStrike(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new FeintingStrike(ref _caster);
        }

        public override string name => "Feinting Strike";
        public override string effectDescription => "Ready an ethereal blade to bypass your opponent's defenses.";
        public override int spellLevel => 2;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 50;
        public override bool hasDuration => true;
        protected override float baseDuration => 15.0f;

        public override float cooldown => 5;

        protected override Damage _damage => new Damage(3.0f * caster.GetWeaponDamage(), Combat.DamageType.Magical);
    }
}
