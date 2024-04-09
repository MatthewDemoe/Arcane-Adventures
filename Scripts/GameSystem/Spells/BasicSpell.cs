using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells
{
    public class BasicSpell : ProjectileSpell
    {
        public static BasicSpell Instance { get; } = new BasicSpell();
        private BasicSpell() : base() { }
        private BasicSpell(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new BasicSpell(ref _caster);
        }

        public override string name => "Basic Spell";
        public override string effectDescription => "Create a concentrated blast of arcane energy to damage enemies in front of you.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 0;

        public override float channelCost => 0;
        protected override float _radius => 0.0f;

        public override float channelDuration => 0.5f;
        public override float channelInterval => 0.1f;

        public override float cooldown => 0;

        protected override float _range => 30.0f;
        protected override float _force => CombatSettings.Spells.ProjectileForce;
        public override float knockbackDistance { get; } = 1.0f;
        protected override Damage _damage => new Damage(1.0f * caster.stats.subtotalSpirit, Combat.DamageType.Magical);
    }
}
