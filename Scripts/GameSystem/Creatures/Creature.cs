using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System;
using System.Linq;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures
{
    [Serializable]
    public abstract class Creature
    {
        [SerializeField]
        private Race _race = Race.Other;
        public Race race { get { return _race; } set { _race = value; } }
        public CreatureSize size => CreatureSize.Medium;//TODO: Set depending on race or monster type.

        [SerializeField]
        private Stats _stats;
        public Stats stats { get { return _stats; } private set { _stats = value; } }

        public StatusConditionTracker statusConditionTracker { get; } = new StatusConditionTracker();
        public CreatureModifiers modifiers { get; } = new CreatureModifiers();
        public float overwhelmFlatModifier => modifiers.GetOverwhelmFlatModifer();
        public float overwhelmMultiplier => modifiers.GetOverwhelmMultiplier();

        public const float SpiritRegenerationInterval = 0.5f;

        public bool isDead => stats.currentHP <= 0;

        public virtual float moveSpeed => 2.0f * modifiers.effects.Select((effect) => effect.moveSpeed).Product();

        private bool _isMovementEnabled = true;
        public bool isMovementEnabled 
        {
            get { return _isMovementEnabled; }
            set
            {
                _isMovementEnabled = value;
                OnMovementToggled.Invoke();
            }
        }

        public bool isActionEnabled = true;
        public bool isSpellcastingEnabled = true;
        public bool isInputEnabled = true;

        public bool preventDamage = false;

        public delegate void ReportResourceChange();
        public ReportResourceChange OnDamageTaken = () => { };
        public ReportResourceChange OnHealthGained = () => { };
        public ReportResourceChange OnSpiritChange = () => { };

        public delegate void WeaponHitBeforeReportListener(Creature target);
        public WeaponHitBeforeReportListener OnWeaponHitBeforeReport = (target) => { };

        public delegate void TargetKilledListener();
        public TargetKilledListener OnTargetKilled = () => { };

        public delegate void ReportWeaponHit(Creature target, StrikeType strikeType);
        public ReportWeaponHit OnWeaponHitReported = (target, strikeType) => { };

        public delegate void ReportSpellCasted(Spell spell);
        public ReportSpellCasted OnSpellCasted = (spell) => { };

        public delegate float ProcessHitDamage(Hit hit);
        public ProcessHitDamage OnProcessHit = (hit) => 0;
        public ProcessHitDamage OnProcessHeal = (hit) => 0;

        public delegate bool SpiritAdjustmentAction(float amount, out float adjusted);
        public SpiritAdjustmentAction SpiritAdjustedActions = null;

        public bool TryAdjustSpirit(float amount, out float adjusted)
        {
            bool spiritAdjusted = false;
            adjusted = 0;
            float iterationAdjustment = 0;

            foreach (SpiritAdjustmentAction action in SpiritAdjustedActions.GetInvocationList())
            {
                bool actionResult = action(amount, out iterationAdjustment);
                adjusted += iterationAdjustment;

                if (actionResult)
                    spiritAdjusted = true;
            }

            return spiritAdjusted;
        }

        public delegate void MovementToggled();
        public MovementToggled OnMovementToggled = () => { };

        //TODO: Invoke when levels exist
        public delegate void LevelUpListener();
        public LevelUpListener OnLevelUp = () => { };

        public Creature(Stats stats, Race race)
        {
            Races.Race raceObject = Races.Race.Get(race);

            this.race = race;
            this.stats = stats;

            raceObject?.InitializeStats(this.stats);
            raceObject?.InitializeTrait(this);

            SpiritAdjustedActions = stats.TryAdjustSpirit;
        }

        public virtual int GetWeaponDamage()
        {
            return 0;
        }

        public virtual void TryRegenerateSpirit()
        {
            if (stats.currentSpirit >= stats.maxSpirit)
                return;

            SpiritAdjustedActions.Invoke(-stats.spiritRegenerationPerSecond * (SpiritRegenerationInterval), out float _);
            OnSpiritChange.Invoke();
        }
    }
}