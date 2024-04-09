using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class BleedingStatusSettings : StatusConditionSettings
    {
        protected const float DefaultHealingDecrease = 1.25f;

        public BleedingStatusSettings() : base()
        {
            this.healingDecrease = DefaultHealingDecrease;
        }

        public BleedingStatusSettings(int durationInMilliseconds = 15000, int effectIntervalInMilliseconds = DefaultEffectInterval, float healingDecrease = DefaultHealingDecrease)
            : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            this.healingDecrease = healingDecrease;
        }

        //TODO: Use this when characters can be healed
        public float healingDecrease { get; }
    }

    public class Bleeding : StatusCondition
    {
        BleedingStatusSettings bleedingStatusSettings => statusConditionSettings as BleedingStatusSettings;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Bleeding;


        private float _damage => Math.Max(affectedCreature.stats.baseVitality * 0.5f, 5.0f);

        public override string name => "Bleeding";
        public override string description => $"Take {_damage} damage every {bleedingStatusSettings.effectIntervalInMilliseconds / 1000} seconds. " +
            $"Reduce incoming healing by {(bleedingStatusSettings.healingDecrease - 1.0f) * 100.0f}%";

        public Bleeding() : base() { }

        public Bleeding(Creature creature, StatusConditionSettings bleedingStatusSettings, string source, bool startDuration = true) : base(creature, bleedingStatusSettings, source, startDuration)
        {

        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Bleeding(creature, new BleedingStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        protected override void UpdateTimer(Object stateInfo)
        {
            base.UpdateTimer(stateInfo);

            StatusConditionHit bleedingHit = new StatusConditionHit(affectedCreature, (int)_damage);

            _combatSystem.ReportHit(bleedingHit);
        }
    }
}