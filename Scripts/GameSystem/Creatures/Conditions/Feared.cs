using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class FearedStatusSettings : StatusConditionSettings
    {
        private const float DefaultMoveSpeedDecrease = 0.5f;

        public FearedStatusSettings() : base() 
        {
            this.moveSpeedDecrease = DefaultMoveSpeedDecrease;
        }

        public FearedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval, float moveSpeedDecrease = DefaultMoveSpeedDecrease) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            this.moveSpeedDecrease = moveSpeedDecrease;
        }

        public float moveSpeedDecrease;
    }

    public class Feared : StatusCondition
    {
        FearedStatusSettings fearedStatusSettings => statusConditionSettings as FearedStatusSettings;
        CreatureEffect creatureEffect;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Feared;

        public override bool shouldDisableMovement => affectedCreature is PlayerCharacter;

        public override string name => "Feared";
        public override string description => $"Decrease movement speed by {this.fearedStatusSettings.moveSpeedDecrease}";

        public Feared() : base() { }

        public Feared(Creature creature, StatusConditionSettings fearedStatusSettings, string source, bool startDuration = true) : base(creature, fearedStatusSettings, source, startDuration)
        {
            creatureEffect = new CreatureEffect(
                name: "Feared Effect",
                description: description, 
                source: "Feared Condition",
                moveSpeed: this.fearedStatusSettings.moveSpeedDecrease
                );

            affectedCreature.modifiers.AddEffect(creatureEffect);
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Feared(creature, new FearedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();

            affectedCreature.modifiers.RemoveEffect(creatureEffect);
        }
    }
}