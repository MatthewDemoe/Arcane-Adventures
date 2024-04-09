using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class ShadowBind : Spell
    {
        public static ShadowBind Instance { get; } = new ShadowBind();
        private ShadowBind() : base() { }
        private ShadowBind(ref Creature _caster) : base(ref _caster) { }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new ShadowBind(ref _caster);
        }

        public override string name => "Shadow Bind";
        public override string effectDescription => "Shadowy tendrils sprout from the shadow of your target, restraining them.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Control;
        public override float initialCost => 30.0f;

        public override float upkeepInterval => 5.0f;
        public override bool hasDuration => true;

        protected override float baseDuration => 15;

        public override float cooldown => 20;
        protected override float _range => 25.0f;

        protected override Damage _damage => new Damage(caster.stats.subtotalSpirit, Combat.DamageType.Bludgeoning);
    }
}
