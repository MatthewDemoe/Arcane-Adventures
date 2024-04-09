
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class ImmobilizedStatusSettings : StatusConditionSettings
    {
        public ImmobilizedStatusSettings() : base() { }

        public ImmobilizedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
        }
    }

    public class Immobilized : StatusCondition
    {
        ImmobilizedStatusSettings immobilizedStatusSettings => statusConditionSettings as ImmobilizedStatusSettings;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Immobilized;

        public override bool shouldDisableAction => true;
        public override bool shouldDisableMovement => true;

        public override string name => "Immobilized";
        public override string description => "Movement of legs and arms is disabled.";

        public Immobilized() : base() { }

        public Immobilized(Creature creature, StatusConditionSettings immobilizedStatusSettings, string source, bool startDuration = true) : base(creature, immobilizedStatusSettings, source, startDuration)
        {
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Immobilized(creature, new ImmobilizedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();
        }
    }
}