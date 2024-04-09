
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class MutedStatusSettings : StatusConditionSettings
    {
        public MutedStatusSettings() : base() { }

        public MutedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
        }
    }

    public class Muted : StatusCondition
    {
        MutedStatusSettings mutedStatusSettings => statusConditionSettings as MutedStatusSettings;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Muted;

        public override bool shouldDisableSpellcasting => true;

        public override string name => "Muted";
        public override string description => "Unable to speak or cast spells until this effect is removed.";

        public Muted() : base() { }

        public Muted(Creature creature, StatusConditionSettings mutedStatusSettings, string source, bool startDuration = true) : base(creature, mutedStatusSettings, source, startDuration)
        {
        }
        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Muted(creature, new MutedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();
        }
    }
}