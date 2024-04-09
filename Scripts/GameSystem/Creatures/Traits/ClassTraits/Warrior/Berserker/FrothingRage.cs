using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class FrothingRage : Trait
    {
        Frenzied frenziedCondition;

        public static FrothingRage Instance { get; } = new FrothingRage();
        private FrothingRage() { }

        protected FrothingRage(Creature creature) : base(creature)
        {
            creature.OnDamageTaken += CheckCondition;
            creature.OnHealthGained += CheckCondition;

            frenziedCondition = new Frenzied(creature, new FrenziedStatusSettings(durationInMilliseconds: 0), name);
        }

        public override Trait Get(Creature creature)
        {
            return new FrothingRage(creature);
        }

        public override string name => "Frothing Rage";
        public override string description => "While you are below 50% of your maximum health you gain the Frenzied condition.";

        private void CheckCondition()
        {
            bool hasFrenzied = creature.statusConditionTracker.HasStatusCondition(AllStatusConditions.StatusConditionName.Frenzied);
            bool frenziedIsFromTrait = creature.statusConditionTracker.GetStatusCondition(AllStatusConditions.StatusConditionName.Frenzied).source.Equals(name);            

            if (creature.stats.currentHealthPercent > 0.5f)
            {
                if(hasFrenzied && frenziedIsFromTrait)
                    creature.statusConditionTracker.RemoveStatusCondition(AllStatusConditions.StatusConditionName.Frenzied);

                return;
            }

            if (hasFrenzied && !frenziedIsFromTrait)
                creature.statusConditionTracker.RemoveStatusCondition(AllStatusConditions.StatusConditionName.Frenzied);

            creature.statusConditionTracker.AddStatusCondition(frenziedCondition);
        }

        public override void Disable()
        {
            base.Disable();

            creature.OnDamageTaken -= CheckCondition;
            creature.OnHealthGained -= CheckCondition;

            creature.statusConditionTracker.RemoveStatusCondition(AllStatusConditions.StatusConditionName.Frenzied);
        }
    }
}
