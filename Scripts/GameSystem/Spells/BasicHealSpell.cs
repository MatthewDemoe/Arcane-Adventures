using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells
{
    public class BasicHealSpell : ProjectileSpell
    {
        public static BasicHealSpell Instance { get; } = new BasicHealSpell();
        private BasicHealSpell() : base() { }
        private BasicHealSpell(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new BasicHealSpell(ref _caster);
        }

        public override string name => "Basic Heal Spell";
        public override string effectDescription => "Heal an ally for a portion of their missing health.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Enhancement;
        public override float initialCost => 0.0f;
        public override bool hasDuration => false;

        public override float cooldown => 10;

        protected override float _range => 30.0f;
        protected override Damage _damage => new Damage(caster.stats.subtotalVitality * -3.0f, DamageType.Magical);
    }
}
