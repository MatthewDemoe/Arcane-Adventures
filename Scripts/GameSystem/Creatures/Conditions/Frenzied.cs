using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class FrenziedStatusSettings : StatusConditionSettings
    {
        protected const float DefaultDamageTakenMultiplier = 1.1f;
        protected const float DefaultDamageDealtMultiplier = 1.2f;
        protected const int DefaultContestedCheckBonus = 3;

        public FrenziedStatusSettings() : base()
        {
            damageTakenMultiplier = DefaultDamageTakenMultiplier;
            damageDealtMultiplier = DefaultDamageDealtMultiplier;
            contestedCheckBonus = DefaultContestedCheckBonus;
        }

        public FrenziedStatusSettings(int durationInMilliseconds = 10000, int effectIntervalInMilliseconds = DefaultEffectInterval, 
            float damageTakenMultiplier = DefaultDamageTakenMultiplier, float damageDealtMultiplier = DefaultDamageDealtMultiplier, int contestedCheckBonus = DefaultContestedCheckBonus) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            this.damageTakenMultiplier = damageTakenMultiplier;
            this.damageDealtMultiplier = damageDealtMultiplier;
            this.contestedCheckBonus = contestedCheckBonus;
        }

        public float damageTakenMultiplier { get; }
        public float damageDealtMultiplier { get; }
        public int contestedCheckBonus { get; }
    }

    public class Frenzied : StatusCondition
    {
        FrenziedStatusSettings frenziedStatusSettings => statusConditionSettings as FrenziedStatusSettings;
        CreatureEffect creatureEffect;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Frenzied;

        public override string name => "Frenzied";
        public override string description => $"Increase damage taken by {this.frenziedStatusSettings.damageTakenMultiplier}, damage dealt by {this.frenziedStatusSettings.damageDealtMultiplier}, and gain a bonus of {this.frenziedStatusSettings.contestedCheckBonus} on Contested Checks.";
        public Frenzied() : base() { }
        
        public Frenzied(Creature creature, StatusConditionSettings frenziedStatusSettings, string source, bool startDuration = true) : base(creature, frenziedStatusSettings, source, startDuration)
        {
            
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Frenzied(creature, new FrenziedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void StartDuration()
        {
            base.StartDuration();

            creatureEffect = new CreatureEffect(
                name: "Frenzied Effect",
                description: description,
                source: "Frenzied Condition",
                contestedCheckBonus: this.frenziedStatusSettings.contestedCheckBonus,
                damageTaken: this.frenziedStatusSettings.damageTakenMultiplier,
                trueDamageDealt: this.frenziedStatusSettings.damageDealtMultiplier
                );

            affectedCreature.modifiers.AddEffect(creatureEffect);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();

            affectedCreature.modifiers.RemoveEffect(creatureEffect);
        }
    }
}