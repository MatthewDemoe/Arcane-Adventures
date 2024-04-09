using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class SlicingDash : Spell
    {
        public static SlicingDash Instance { get; } = new SlicingDash();
        private SlicingDash() { caster = null; }
        private SlicingDash(ref Creature _caster) : base(ref _caster) { }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new SlicingDash(ref _caster);
        }

        public override string name => "Slicing Dash";
        public override string effectDescription => "Quickly dash forward, damaging enemies in your path.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 25;

        public override bool hasDuration => false;

        public override bool isSelfTargeted => false;
        protected override float _radius => 2.0f;

        protected override float _range => 12.5f;

        public override float cooldown => 10;

        //TODO: get damage from equipped weapons somehow. 
        protected override Damage _damage => new Damage(15.0f, Combat.DamageType.Slashing);
    }
}
