
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class StunnedStatusSettings : StatusConditionSettings
    {
        public StunnedStatusSettings() : base() { }

        public StunnedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {

        }

    }

    public class Stunned : StatusCondition
    {
        StunnedStatusSettings stunnedStatusSettings => statusConditionSettings as StunnedStatusSettings;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Stunned;

        public override bool shouldDisableAction => true;
        public override bool shouldDisableMovement => true;
        public override bool shouldDisableSpellcasting => true;

        public override string name => "Stunned";
        public override string description => "Disable movement, actions, and spellcasting.";

        public Stunned() : base() { }

        public Stunned(Creature creature, StatusConditionSettings stunnedStatusSettings, string source, bool startDuration = true) : base(creature, stunnedStatusSettings, source, startDuration)
        {
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Stunned(creature, new StunnedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();
        }
    }
}