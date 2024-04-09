using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class ArcaneBlast : ProjectileSpell
    {
        public static ArcaneBlast Instance { get; } = new ArcaneBlast();
        private ArcaneBlast() : base() { }
        private ArcaneBlast(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new ArcaneBlast(ref _caster);
        }

        public override string name => "Arcane Blast";
        public override string effectDescription => "Create a concentrated blast of arcane energy to damage enemies in front of you.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 20;

        public override float channelCost => 0;
        protected override float baseDuration => 0;
        protected override float _radius => 0f;
        public override bool hasDuration => false;

        public override float cooldown => 5;

        protected override float _range => 30.0f;
        public  override float knockbackDistance { get; } = 1.0f;
        protected override Damage _damage => new Damage(2.5f * caster.stats.subtotalSpirit, Combat.DamageType.Magical);
    }
}
