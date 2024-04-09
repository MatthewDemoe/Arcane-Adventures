using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class KnockedBackStatusSettings : StatusConditionSettings
    {
        public KnockedBackStatusSettings() : base()
        {
            damage = 0;
        }

        public KnockedBackStatusSettings(int durationInMilliseconds = 3000, int effectIntervalInMilliseconds = DefaultEffectInterval, Creature source = null) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            damage = source == null ? 0 : source.stats.subtotalStrength;
            sourceCreature = source;
        }

        public Creature sourceCreature { get; } = null;
        public int damage { get; }
    }

    public class KnockedBack : StatusCondition
    {
        KnockedBackStatusSettings knockedBackStatusSettings => statusConditionSettings as KnockedBackStatusSettings;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.KnockedBack;

        public override string name => "Knocked Back";
        public override string description => "While knocked back, if you collide with the environment, take damage and become stunned unless you pass a contested check.";

        public KnockedBack() : base() { }
        
        public KnockedBack(Creature creature, StatusConditionSettings knockedBackStatusSettings, string source, bool startDuration = true) : base(creature, knockedBackStatusSettings, source, startDuration)
        {
        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new KnockedBack(creature, new KnockedBackStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public void OnObjectCollision()
        {
            if (!affectedCreature.statusConditionTracker.HasStatusCondition(statusConditionName))
                return;

            _combatSystem.ReportHit(new StatusConditionHit(affectedCreature, knockedBackStatusSettings.damage));

            bool statContestResult = StatContest.PerformStatContest(affectedCreature, knockedBackStatusSettings.sourceCreature, Stats.Stat.Vitality, Stats.Stat.Strength);

            if (!statContestResult)
            {
                StatusCondition dazedCondition = new Dazed(affectedCreature, new DazedStatusSettings(durationInMilliseconds: 3000), name, true);
                affectedCreature.statusConditionTracker.AddStatusCondition(dazedCondition);  
            }

            affectedCreature.statusConditionTracker.RemoveStatusCondition(statusConditionName);
        }
    }
}