using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Injection;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public enum Features { Actions, Movement, Spellcasting, Input }

    public class StatusConditionSettings
    {
        protected const int DefaultDuration = 5000;
        protected const int DefaultEffectInterval = 1000;

        public StatusConditionSettings()
        {
            this.durationInMilliseconds = durationInMilliseconds;
            this.effectIntervalInMilliseconds = effectIntervalInMilliseconds;
        }

        public StatusConditionSettings(int durationInMilliseconds = DefaultDuration, int effectIntervalInMilliseconds = DefaultEffectInterval)
        {
            this.durationInMilliseconds = durationInMilliseconds;
            this.effectIntervalInMilliseconds = effectIntervalInMilliseconds;
        }

        public virtual int durationInMilliseconds { get; protected set; } = DefaultDuration;
        public virtual int effectIntervalInMilliseconds { get; protected set; } = DefaultEffectInterval;
    }

    public abstract class StatusCondition : ITraceable
    {
        public Creature affectedCreature { get; protected set; }
        protected bool hasBeenRemoved = false;
        [Inject] protected ICombatSystem _combatSystem;

        public int durationTimer { get; protected set; } = 0;

        public bool durationStarted { get; protected set; } = false;

        protected Timer _timer = null;
        protected AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public StatusConditionSettings statusConditionSettings { get; protected set; }

        public abstract AllStatusConditions.StatusConditionName statusConditionName { get; }

        public virtual bool shouldDisableAction => false;
        public virtual bool shouldDisableMovement => false;
        public virtual bool shouldDisableSpellcasting => false;
        public virtual bool shouldDisableInput => false;

        public abstract string name { get; }
        public abstract string description { get; }
        public string source { get; protected set; }

        float creatureDurationModifier = 1.0f;

        private Dictionary<Features, bool> shouldDisableFeaturesByFeature => new Dictionary<Features, bool>() 
        {
            { Features.Actions, shouldDisableAction },
            { Features.Movement, shouldDisableMovement },
            { Features.Spellcasting, shouldDisableSpellcasting },
            { Features.Input, shouldDisableInput },
        };

        public static StatusCondition CreateStatusCondition<T>(T conditionType, Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true) where T : StatusCondition
        {
            return conditionType.GetStatusCondition(creature, statusConditionData, source, startDuration);
        }

        protected abstract StatusCondition GetStatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true);

        public StatusCondition(Creature creature, StatusConditionSettings statusConditionData, string source, bool startDuration = true)
        {
            InjectorContainer.Injector.Inject(this);

            this.statusConditionSettings = statusConditionData;
            this.source = source;

            affectedCreature = creature;
            hasBeenRemoved = false;
            creatureDurationModifier = affectedCreature.modifiers.effects.Select(effect => effect.statusConditionDurationModifier).Product();

            _timer = new Timer(UpdateTimer, autoResetEvent, statusConditionData.effectIntervalInMilliseconds, statusConditionData.effectIntervalInMilliseconds);

            DisableFeatures();

            if (startDuration)
            {
                StartDuration();
            }
        }

        public StatusCondition() { }

        public virtual void RemoveCondition()
        {
            if (hasBeenRemoved)
                return;

            hasBeenRemoved = true;

            autoResetEvent.Set();
            _timer.Dispose();

            TryEnableFeatures();
        }

        public virtual void StartDuration()
        {
            durationStarted = true;
        }

        protected virtual void UpdateTimer(Object stateInfo)
        {
            if (durationStarted)
                durationTimer += statusConditionSettings.effectIntervalInMilliseconds;

            autoResetEvent.Set();

            if (statusConditionSettings.durationInMilliseconds == 0)
                return;

            if (durationTimer >= statusConditionSettings.durationInMilliseconds * creatureDurationModifier)
            {
                affectedCreature.statusConditionTracker.RemoveStatusCondition(statusConditionName);
            }
        }

        private void DisableFeatures()
        {
            affectedCreature.isActionEnabled = !shouldDisableAction;

            affectedCreature.isMovementEnabled = !shouldDisableMovement;

            affectedCreature.isSpellcastingEnabled = !shouldDisableSpellcasting;

            affectedCreature.isInputEnabled = !shouldDisableInput;
        }

        private void TryEnableFeatures()
        {
            if (shouldDisableAction)
                TryEnableFeature(Features.Actions);

            if (shouldDisableMovement)
                TryEnableFeature(Features.Movement);

            if (shouldDisableSpellcasting)
                TryEnableFeature(Features.Spellcasting);

            if (shouldDisableInput)
                TryEnableFeature(Features.Input);
        }

        private void TryEnableFeature(Features feature)
        {
            if (affectedCreature.statusConditionTracker.GetStatusConditions.Any((statusCondition) => statusCondition.shouldDisableFeaturesByFeature[feature]))
            {
                return;
            };

            switch (feature)
            {
                case Features.Actions:
                    affectedCreature.isActionEnabled = true;
                    break;

                case Features.Movement:
                    affectedCreature.isMovementEnabled = true;
                    break;

                case Features.Spellcasting:
                    affectedCreature.isSpellcastingEnabled = true;
                    break;

                case Features.Input:
                    affectedCreature.isInputEnabled = true;
                    break;

                default:
                    return;
            }
        }
    }
}