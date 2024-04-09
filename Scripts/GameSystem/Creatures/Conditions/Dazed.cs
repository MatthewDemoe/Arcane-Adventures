using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class DazedStatusSettings : StatusConditionSettings
    {
        protected const float DefaultSpeedDecrease = 0.5f;

        public DazedStatusSettings() : base()
        {
            this.moveSpeedDecrease = DefaultSpeedDecrease;
        }

        public DazedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval, float moveSpeedDecrease = DefaultSpeedDecrease) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            this.moveSpeedDecrease = moveSpeedDecrease;
        }

        public float moveSpeedDecrease { get; }
    }

    public class Dazed : StatusCondition
    {
        DazedStatusSettings dazedStatusSettings => statusConditionSettings as DazedStatusSettings;
        CreatureEffect creatureEffect;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Dazed;

        public override string name => "Dazed";
        public override string description => $"Decrease movement speed by {this.dazedStatusSettings.moveSpeedDecrease}";

        public Dazed() : base() { }

        public Dazed(Creature creature, StatusConditionSettings dazedStatusSettings, string source, bool startDuration = true) : base(creature, dazedStatusSettings, source, startDuration)
        {
            this.source = source;

            creatureEffect = new CreatureEffect(
                name: "Dazed Effect", 
                description: description,
                source: source,
                moveSpeed: this.dazedStatusSettings.moveSpeedDecrease
                );

            affectedCreature.modifiers.AddEffect(creatureEffect);
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Dazed(creature, new DazedStatusSettings(durationInMilliseconds: statusConditionData.durationInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();

            affectedCreature.modifiers.RemoveEffect(creatureEffect);
        }
    }
}