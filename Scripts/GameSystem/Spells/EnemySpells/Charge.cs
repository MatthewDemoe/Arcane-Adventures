using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.EnemySpells
{
    public class Charge : Spell
    {
        public static Charge Instance { get; } = new Charge();
        private Charge() : base() { }
        private Charge(ref Creature _caster) : base(ref _caster) { }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new Charge(ref _caster);
        }

        public override string name => "Charge";
        public override string effectDescription => "Charge...";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 0.0f;
        protected override float _radius => 2;
        public override bool hasDuration => false;

        public override float cooldown => 5.0f;
        public override float knockbackDistance => 2.0f;

        protected override Damage _damage => new Damage(15.0f, DamageType.Bludgeoning);
    }
}