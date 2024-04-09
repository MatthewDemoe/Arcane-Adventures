using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class HastedStatusSettings : StatusConditionSettings
    {
        protected const float DefaultMoveSpeedIncrease = 1.5f;

        public HastedStatusSettings() : base()
        {
            moveSpeedIncrease = DefaultMoveSpeedIncrease;
        }

        public HastedStatusSettings(int durationInMilliseconds = 5000, int effectIntervalInMilliseconds = 1000, float moveSpeedIncrease = 1.5f) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            this.moveSpeedIncrease = moveSpeedIncrease;
        }

        public float moveSpeedIncrease { get; }
    }

    public class Hasted : StatusCondition
    {
        HastedStatusSettings hastedStatusSettings => statusConditionSettings as HastedStatusSettings;
        CreatureEffect creatureEffect;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Hasted;

        public override string name => "Hasted";
        public override string description => $"Increase movement speed by {this.hastedStatusSettings.moveSpeedIncrease}.";

        public Hasted() : base() { }
        public Hasted(Creature creature, StatusConditionSettings hastedStatusSettings, string source, bool startDuration = true) : base(creature, hastedStatusSettings, source, startDuration)
        {
            creatureEffect = new CreatureEffect(
                name: "Hasted Effect",
                description: description,
                source: "Hasted Condition", 
                moveSpeed: this.hastedStatusSettings.moveSpeedIncrease
                );

            affectedCreature.modifiers.AddEffect(creatureEffect);
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Hasted(creature, new HastedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();

            affectedCreature.modifiers.RemoveEffect(creatureEffect);
        }
    }
}