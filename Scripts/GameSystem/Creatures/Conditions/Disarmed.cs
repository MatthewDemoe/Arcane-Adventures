
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class DisarmedStatusSettings : StatusConditionSettings
    {
        public DisarmedStatusSettings() : base() { }

        public DisarmedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
        }
    }

    public class Disarmed : StatusCondition
    {
        DisarmedStatusSettings disarmedStatusSettings => statusConditionSettings as DisarmedStatusSettings;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Disarmed;

        public override string name => "Disarmed";
        public override string description => "Your equipment is dropped and you are unable to pick up new equipment until effect is removed.";

        public Disarmed() : base() { }

        public Disarmed(Creature creature, StatusConditionSettings disarmedStatusSettings, string source, bool startDuration = true) : base(creature, disarmedStatusSettings, source, startDuration)
        {

        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Disarmed(creature, new DisarmedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void StartDuration()
        {
            base.StartDuration();

        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();

        }
    }
}