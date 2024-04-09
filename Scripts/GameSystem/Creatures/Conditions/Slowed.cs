
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class SlowedStatusSettings : StatusConditionSettings
    {
        protected const float DefaultMoveSpeedDecrease = 0.5f;

        public SlowedStatusSettings() : base()
        {
            moveSpeedDecrease = DefaultMoveSpeedDecrease;
        }

        public SlowedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval, float moveSpeedDecrease = 0.5f) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            this.moveSpeedDecrease = moveSpeedDecrease;
        }

        public float moveSpeedDecrease;
    }

    public class Slowed : StatusCondition
    {
        SlowedStatusSettings slowedStatusSettings => statusConditionSettings as SlowedStatusSettings;
        CreatureEffect slowedEffect;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Slowed;

        public override string name => "Slowed";
        public override string description => $"Movement speed decreased by {slowedStatusSettings.moveSpeedDecrease}.";

        public Slowed() : base() { }

        public Slowed(Creature creature, StatusConditionSettings slowedStatusSettings, string source, bool startDuration = true) : base(creature, slowedStatusSettings, source, startDuration)
        {
            float slowAmount = this.slowedStatusSettings.moveSpeedDecrease;

            slowedEffect = new CreatureEffect
            (
                name: "Slowed",
                description: description,
                source: "Slowed Effect", 
                moveSpeed: slowAmount
            );

            affectedCreature.modifiers.AddEffect(slowedEffect);
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Slowed(creature, new SlowedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();
            affectedCreature.modifiers.RemoveEffect(slowedEffect);
        }
    }
}