using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class MotivatedStatusSettings : StatusConditionSettings
    {
        public MotivatedStatusSettings() : base() { }

        public MotivatedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {

        }

        public float buffAmount { get; } = 1.1f;
    }

    public class Motivated : StatusCondition
    {
        MotivatedStatusSettings motivatedStatusSettings => statusConditionSettings as MotivatedStatusSettings;

        CreatureEffect creatureEffect;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Motivated;

        public override string name => "Motivated";
        public override string description => $"Damage increased by {this.motivatedStatusSettings.buffAmount}. Movement speed increased by {this.motivatedStatusSettings.buffAmount}.";

        public Motivated() : base() { }

        public Motivated(Creature creature, StatusConditionSettings motivatedStatusSettings, string source, bool startDuration = true) : base(creature, motivatedStatusSettings, source, startDuration)
        {
            creatureEffect = new CreatureEffect(
                name: "Motivated Effect",
                description: description,
                source: "Motivated Condition",
                trueDamageDealt: this.motivatedStatusSettings.buffAmount,
                moveSpeed: this.motivatedStatusSettings.buffAmount
                );

            affectedCreature.modifiers.AddEffect(creatureEffect);
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Motivated(creature, new MotivatedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void StartDuration()
        {
            base.StartDuration();
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();

            affectedCreature.modifiers.AddEffect(creatureEffect);
        }
    }
}