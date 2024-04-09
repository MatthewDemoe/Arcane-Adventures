using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using Injection;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class ReserveSpirit : Trait
    {
        [Inject] protected ICombatSystem _combatSystem;

        public override string name => "Reserve Spirit";
        public override string description => "When you are at 0 Spirit you begin to cast spells and maintain upkeep spells using your health as a resource instead of spirit. Health lost in this way acts as 125% of what would be used as Spirit.";

        public static ReserveSpirit Instance { get; } = new ReserveSpirit();
        private ReserveSpirit() { }

        protected ReserveSpirit(Creature creature) : base(creature)
        {
            InjectorContainer.Injector.Inject(this);

            creature.SpiritAdjustedActions += TrySpendLife;
        }

        public override Trait Get(Creature creature)
        {
            return new ReserveSpirit(creature);
        }

        private bool TrySpendLife(float amount, out float adjusted)
        {
            adjusted = 0.0f;

            if (creature.stats.currentSpirit - amount > 0)
                return false;

            float spiritCostOverRemainingSpirit = creature.stats.currentSpirit - amount;

            creature.stats.TryAdjustSpirit(creature.stats.currentSpirit, out adjusted);

            StatusConditionHit healthCostHit = new StatusConditionHit(creature, -(int)spiritCostOverRemainingSpirit);
            _combatSystem.ReportHit(healthCostHit);

            return true;
        }

        public override void Disable()
        {
            base.Disable();

            creature.SpiritAdjustedActions -= TrySpendLife;
        }
    }
}
