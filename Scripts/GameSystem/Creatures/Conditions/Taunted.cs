using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class TauntedStatusSettings : StatusConditionSettings
    {
        public TauntedStatusSettings() : base() { }

        public TauntedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval, Creature source = null) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            sourceCreature = source;
        }

        public Creature sourceCreature { get; } = null;
    }

    public class Taunted : StatusCondition
    {
        public TauntedStatusSettings tauntedStatusSettings => statusConditionSettings as TauntedStatusSettings;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Taunted;

        public override string name => "Taunted";
        public override string description => "Focus all your attacks on the enemy who taunted you";

        public Taunted() : base() { }

        public Taunted(Creature creature, StatusConditionSettings tauntedStatusSettings, string source, bool startDuration = true) : base(creature, tauntedStatusSettings, source, startDuration)
        {
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Taunted(creature, new TauntedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }
    }
}