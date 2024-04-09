using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class ReactiveBarrier : Spell
    {
        public static ReactiveBarrier Instance { get; } = new ReactiveBarrier();
        private ReactiveBarrier() { caster = null; }
        private ReactiveBarrier(ref Creature _caster) : base(ref _caster)
        {
        }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new ReactiveBarrier(ref _caster);
        }

        public override string name => "Reactive Barrier";
        public override string effectDescription => "Create a barrier that protects you from harm. Deals damage in an AOE when destroyed.";
        public override int spellLevel => 2;

        public override SpellType spellType => SpellType.Control;
        public override float initialCost => 50;

        public override bool hasDuration => true;
        protected override float baseDuration => 15;
        protected override float _radius => 5.0f;
        public override bool isSelfTargeted => true;

        public override float cooldown => 30;

        protected override Damage _damage => new Damage(1.5f * caster.stats.subtotalVitality, DamageType.Magical);
    }
}
