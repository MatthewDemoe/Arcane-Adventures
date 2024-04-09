using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class MaddenedStatusSettings : StatusConditionSettings
    {
        protected const int DefaultSavingThrowInterval = 5000;

        public MaddenedStatusSettings() : base()
        {
            savingThrowInterval = DefaultSavingThrowInterval;
        }

        public MaddenedStatusSettings(int durationInMilliseconds = 30000, int effectIntervalInMilliseconds = DefaultEffectInterval, int savingThrowInterval = DefaultSavingThrowInterval) : base(durationInMilliseconds, effectIntervalInMilliseconds)
        {
            this.savingThrowInterval = savingThrowInterval;
        }

        public int savingThrowInterval { get; }
    }

    public class Maddened : StatusCondition
    {
        MaddenedStatusSettings maddenedStatusSettings => statusConditionSettings as MaddenedStatusSettings;

        int contestTimer = 0;

        public override bool shouldDisableSpellcasting => true;
        public override bool shouldDisableInput => true;

        public override AllStatusConditions.StatusConditionName statusConditionName => AllStatusConditions.StatusConditionName.Maddened;

        public Maddened() : base() { }

        public override string name => "Maddened";
        public override string description => "Lose control and attack the closest creature to you.";

        public Maddened(Creature creature, StatusConditionSettings maddenedStatusSettings, string source, bool startDuration = true) : base(creature, maddenedStatusSettings, source, startDuration)
        {

        }

        protected override StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            return new Maddened(creature, new MaddenedStatusSettings(statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds), source, startDuration);
        }

        public override void RemoveCondition()
        {
            base.RemoveCondition();
        }

        protected override void UpdateTimer(object stateInfo)
        {
            base.UpdateTimer(stateInfo);

            contestTimer += maddenedStatusSettings.effectIntervalInMilliseconds;

            if (contestTimer < maddenedStatusSettings.savingThrowInterval)
                return;

            contestTimer = 0;

            bool savingThrowPassed = SavingThrow.PerformSavingThrow(affectedCreature, Stats.Stat.Spirit);

            if (savingThrowPassed)
                affectedCreature.statusConditionTracker.RemoveStatusCondition(statusConditionName);
        }
    }
}