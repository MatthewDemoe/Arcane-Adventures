using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class DemotivatedStatusSettings : StatusConditionSettings
    {
        public DemotivatedStatusSettings() : base() { }

        public DemotivatedStatusSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {

        }

        public float debuffAmount { get; } = 0.9f;
    }

    public class Demotivated : StatusCondition
    {
        DemotivatedStatusSettings demotivatedStatusSettings => statusConditionSettings as DemotivatedStatusSettings;
        CreatureEffect creatureEffect;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Demotivated;

        public override string name => "Demotivated";
        public override string description => $"Decrease movement speed and damage dealt by {this.demotivatedStatusSettings.debuffAmount}.";

        public Demotivated() : base() { }

        public Demotivated(Creature creature, StatusConditionSettings demotivatedStatusSettings, string source, bool startDuration = true) : base(creature, demotivatedStatusSettings, source, startDuration)
        {
            this.source = source;

            creatureEffect = new CreatureEffect(
                name: "Demotivated Effect",
                description: description,
                source: source,
                moveSpeed: this.demotivatedStatusSettings.debuffAmount, 
                trueDamageDealt: this.demotivatedStatusSettings.debuffAmount
                );

            affectedCreature.modifiers.AddEffect(creatureEffect);
        }
        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Demotivated(creature, new DemotivatedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();

            affectedCreature.modifiers.RemoveEffect(creatureEffect);
        }
    }
}