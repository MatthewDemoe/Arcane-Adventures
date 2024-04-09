using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using Injection;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class SiphonedLife : Trait
    {
        [Inject] protected ICombatSystem _combatSystem;

        public override string name => "SiphonedLife";
        public override string description => "When you kill an enemy you gain Spirit and Health equal to 50% of your Spirit Score.";

        public static SiphonedLife Instance { get; } = new SiphonedLife();
        private SiphonedLife() { }

        protected SiphonedLife(Creature creature) : base(creature)
        {
            InjectorContainer.Injector.Inject(this);

            creature.OnTargetKilled += RegenerateResources;
        }

        public override Trait Get(Creature creature)
        {
            return new SiphonedLife(creature);
        }        

        private void RegenerateResources()
        {
            int regenerationAmount = (int)(creature.stats.subtotalSpirit * 0.5f);

            StatusConditionHit healthRegeneration = new StatusConditionHit(creature, -regenerationAmount);
            creature.TryAdjustSpirit(regenerationAmount, out float _);

            _combatSystem.ReportHit(healthRegeneration);
        }

        public override void Disable()
        {
            base.Disable();

            creature.OnTargetKilled -= RegenerateResources;
        }
    }
}
