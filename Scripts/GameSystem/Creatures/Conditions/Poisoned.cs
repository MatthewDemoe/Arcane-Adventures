using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class PoisonedStatusSettings : StatusConditionSettings
    {
        protected const float DefaultDamageDecrease = 0.75f;
        protected const float DefaultDamageMultiplier = 2.0f;

        public PoisonedStatusSettings() : base()
        {
            damageDecrease = DefaultDamageDecrease;
            damageMultiplier = DefaultDamageMultiplier;
        }

        public PoisonedStatusSettings(int durationInMilliseconds = 10000, int effectIntervalInMilliseconds = DefaultEffectInterval, float damageDecrease = DefaultDamageDecrease, float damageMultiplier = DefaultDamageMultiplier) 
            : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            this.damageDecrease = damageDecrease;
            this.damageMultiplier = damageMultiplier;
        }

        public float damageDecrease { get; }
        public float damageMultiplier { get; }
    }

    public class Poisoned : StatusCondition
    {
        PoisonedStatusSettings poisonedStatusSettings => statusConditionSettings as PoisonedStatusSettings;

        CreatureEffect creatureEffect;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Poisoned;

        private float _damage => affectedCreature.stats.level * poisonedStatusSettings.damageMultiplier;

        public override string name => "Poisoned";
        public override string description => $"Damage dealt decreased by {poisonedStatusSettings.damageDecrease}. Take {_damage} damage every {poisonedStatusSettings.effectIntervalInMilliseconds / 1000} seconds";

        public Poisoned() : base() { }

        public Poisoned(Creature creature, StatusConditionSettings poisonedStatusSettings, string source, bool startDuration = true) : base(creature, poisonedStatusSettings, source, startDuration)
        {
            creatureEffect = new CreatureEffect(
                name: "Poisoned", 
                description: $"Damage dealt decreased by {this.poisonedStatusSettings.damageDecrease}",
                source: "Poisoned Condition",
                trueDamageDealt: this.poisonedStatusSettings.damageDecrease
                );

            affectedCreature.modifiers.AddEffect(creatureEffect);
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Poisoned(creature, new PoisonedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();

            affectedCreature.modifiers.RemoveEffect(creatureEffect);
        }

        protected override void UpdateTimer(Object stateInfo)
        {
            base.UpdateTimer(stateInfo);

            StatusConditionHit poisonHit = new StatusConditionHit(affectedCreature, (int)_damage);

            _combatSystem.ReportHit(poisonHit);
        }
    }
}