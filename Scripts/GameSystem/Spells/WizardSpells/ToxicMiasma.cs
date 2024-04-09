using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class ToxicMiasma : ProjectileSpell
    {
        public static ToxicMiasma Instance { get; } = new ToxicMiasma();
        private ToxicMiasma() : base() { }
        private ToxicMiasma(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new ToxicMiasma(ref _caster);
        }

        public override string name => "Toxic Miasma";
        public override string effectDescription => "Corrupt your own spirit to create toxic fumes in your lungs. Exhale poison in an area around you to make the air toxic to enemies.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Enhancement;

        public override float initialCost => 25.0f;

        public override bool hasDuration => true;
        public override float upkeepCost => 5.0f;

        public override float cooldown => 10.0f;
        protected override float baseDuration => 10.0f;

        protected override float _range => 30.0f;
        protected override float _radius => 3.0f;

        protected override float _force => base._force / 2.0f;

        public override float channelInterval => 0.1f;
        public override float channelDuration => 0.1f;
    }
}
