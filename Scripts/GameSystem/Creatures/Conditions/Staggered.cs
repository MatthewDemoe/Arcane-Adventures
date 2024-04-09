 
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class StaggeredStatusSettings : StatusConditionSettings
    {
        protected const int DefaultStunDuration = 2000;
        protected const int DefaultDazeDuration = 2000;

        public StaggeredStatusSettings() : base()
        {
            stunDuration = DefaultStunDuration;
            dazeDuration = DefaultDazeDuration;

            durationInMilliseconds = DefaultStunDuration + DefaultDazeDuration;
        }

        public StaggeredStatusSettings(int durationInMilliseconds = 7000, int effectIntervalInMilliseconds = DefaultEffectInterval, int stunDuration = 2000, int dazeDuration = 5000)
            : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            this.stunDuration = stunDuration;
            this.dazeDuration = dazeDuration;

            this.durationInMilliseconds = stunDuration + dazeDuration;
        }

        public int stunDuration { get; }
        public int dazeDuration { get; }
    }

    public class Staggered : StatusCondition
    {
        StaggeredStatusSettings staggeredStatusSettings => statusConditionSettings as StaggeredStatusSettings;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Staggered;

        private bool stunRemoved = false;

        public override string name => "Staggered";
        public override string description => $"Become Stunned for {staggeredStatusSettings.stunDuration / 1000} seconds, then become Dazed for {staggeredStatusSettings.dazeDuration / 1000} seconds.";

        public Staggered() : base() { }

        public Staggered(Creature creature, StatusConditionSettings staggeredStatusSettings, string source, bool startDuration = true) : base(creature, staggeredStatusSettings, source, startDuration)
        {
            affectedCreature.statusConditionTracker.AddStatusCondition(new Stunned(creature, new StunnedStatusSettings(durationInMilliseconds: 2000), name, true));
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Staggered(creature, new StaggeredStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();

            if(!stunRemoved)
                affectedCreature.statusConditionTracker.RemoveStatusCondition(AllStatusConditions.StatusConditionName.Stunned);

            affectedCreature.statusConditionTracker.RemoveStatusCondition(AllStatusConditions.StatusConditionName.Dazed);
        }

        protected override void UpdateTimer(object stateInfo)
        {
            base.UpdateTimer(stateInfo);

            if (durationTimer <= staggeredStatusSettings.stunDuration || stunRemoved)
                return;

            stunRemoved = true;

            affectedCreature.statusConditionTracker.RemoveStatusCondition(AllStatusConditions.StatusConditionName.Stunned);

            affectedCreature.statusConditionTracker.AddStatusCondition(new Dazed(affectedCreature, 
                new StunnedStatusSettings(durationInMilliseconds: staggeredStatusSettings.dazeDuration), name, true));
        }
    }
}