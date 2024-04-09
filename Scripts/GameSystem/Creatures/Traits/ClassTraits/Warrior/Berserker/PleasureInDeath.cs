using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class PleasureInDeath : Trait
    {
        Motivated motivatedContition;

        public static PleasureInDeath Instance { get; } = new PleasureInDeath();
        private PleasureInDeath() { }

        protected PleasureInDeath(Creature creature) : base(creature)
        {
            motivatedContition = new Motivated(creature, new MotivatedStatusSettings(durationInMilliseconds: 15000), name);
            creature.OnTargetKilled += AddMotivatedCondition;
        }

        public override Trait Get(Creature creature)
        {
            return new PleasureInDeath(creature);
        }

        public override string name => "Pleasure In Death";
        public override string description => "When you land the killing blow on an enemy or a critical hit you gain the Motivated condition for 15 seconds.";

        private void AddMotivatedCondition()
        {
            creature.statusConditionTracker.AddStatusCondition(motivatedContition);
        }

        public override void Disable()
        {
            base.Disable();

            creature.OnTargetKilled -= AddMotivatedCondition;
        }
    }
}
