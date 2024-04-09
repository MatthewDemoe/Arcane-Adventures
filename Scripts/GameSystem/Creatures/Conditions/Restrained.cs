
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class RestrainedStatusSettings : StatusConditionSettings
    {
        public RestrainedStatusSettings() : base() { }

        public RestrainedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
        }
    }

    public class Restrained : StatusCondition
    {
        RestrainedStatusSettings restrainedStatusSettings => statusConditionSettings as RestrainedStatusSettings;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Restrained;

        public override bool shouldDisableMovement => true;

        public override string name => "Restrained";
        public override string description => "Unable to move.";

        public Restrained() : base() { }

        public Restrained(Creature creature, StatusConditionSettings restrainedStatusSettings, string source, bool startDuration = true) : base(creature, restrainedStatusSettings, source, startDuration)
        {
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Restrained(creature, new RestrainedStatusSettings(statusConditionData.durationInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();
        }
    }
}