
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class VulnerableStatusSettings : StatusConditionSettings
    {
        protected const bool DefaultRemoveOnHit = true;
        protected const float DefaultDamageMultiplier = 2.0f;

        public VulnerableStatusSettings() : base()
        {
            removeOnHit = DefaultRemoveOnHit;
            damageMultiplier = DefaultDamageMultiplier;
        }

        public VulnerableStatusSettings(int durationInMilliseconds = 10000, int effectIntervalInMilliseconds = DefaultEffectInterval, 
            bool removeOnHit = DefaultRemoveOnHit, float damageMultiplier = DefaultDamageMultiplier) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            this.removeOnHit = removeOnHit;
            this.damageMultiplier = damageMultiplier;
        }

        public bool removeOnHit { get; }
        public float damageMultiplier { get; }
    }

    public class Vulnerable : StatusCondition
    {
        VulnerableStatusSettings vulnerableStatusSettings => statusConditionSettings as VulnerableStatusSettings;

        CreatureEffect vulnerableEffect;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Vulnerable;

        public override string name => "Vulnerable";
        public override string description => $"Damage taken increased by {vulnerableStatusSettings.damageMultiplier}";

        public Vulnerable() : base() { }

        public Vulnerable(Creature creature, StatusConditionSettings vulnerableStatusSettings, string source, bool startDuration = true) : base(creature, vulnerableStatusSettings, source, startDuration)
        {
            if (this.vulnerableStatusSettings.removeOnHit)
                affectedCreature.OnDamageTaken += DamageTaken;
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Vulnerable(creature, new VulnerableStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void StartDuration()
        {
            base.StartDuration();

            float damageMultiplier = vulnerableStatusSettings.damageMultiplier;

            vulnerableEffect = new CreatureEffect(
                name: "Vulnerable",
                description: description,
                source: "Vulnerable Status Effect",
                damageTaken: damageMultiplier
                );

            affectedCreature.modifiers.AddEffect(vulnerableEffect);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();

            affectedCreature.modifiers.RemoveEffect(vulnerableEffect);
            if (vulnerableStatusSettings.removeOnHit)
                affectedCreature.OnDamageTaken -= DamageTaken;
        }

        private void DamageTaken()
        {
            affectedCreature.statusConditionTracker.RemoveStatusCondition(statusConditionName);
        }
    }
}