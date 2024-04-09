using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Races;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures
{
    [Serializable]
    public class Stats
    {
        private const int ResourcePerStatMultiplier = 2;

        [SerializeField]
        private int _level = 0;
        public int level { get { return _level; } private set { _level = value; } }

        public enum Stat { Strength, Vitality, Spirit };

        #region Stats
        #region Strength
        [SerializeField] private int _baseStrength = 0;
        public int baseStrength { get { return _baseStrength; } private set { _baseStrength = value; } }

        [SerializeField] private int _assignedStrength = 0;
        private int _raceStrength = 0;

        public int subtotalStrength => baseStrength + _assignedStrength + _raceStrength;
        #endregion

        #region Vitality
        [SerializeField] private int _baseVitality = 0;
        public int baseVitality { get { return _baseVitality; } private set { _baseVitality = value; } }

        [SerializeField] private int _assignedVitality = 0;
        private int _raceVitality = 0;
        public int subtotalVitality => baseVitality + _assignedVitality + _raceVitality;
        #endregion

        #region Spirit
        [SerializeField] private int _baseSpirit = 0;
        public int baseSpirit { get { return _baseSpirit; } private set { _baseSpirit = value; } }
        [SerializeField] private int _assignedSpirit = 0;
        private int _raceSpirit = 0;
        public int subtotalSpirit => baseSpirit + _assignedSpirit + _raceSpirit;
        #endregion
        #endregion

        [SerializeField]
        private int _baseMaxHp = 0;
        public int baseMaxHp { get { return _baseMaxHp; } private set { _baseMaxHp = value; } }
        public int maxHp => baseMaxHp + (subtotalVitality * ResourcePerStatMultiplier * level);

        [SerializeField]
        private int _currentHP = 0;
        public int currentHP { get { return _currentHP; } private set { _currentHP = value; } }
        public float currentHealthPercent => (float)currentHP / maxHp;

        [SerializeField]
        private int _baseMaxSpirit = 0;
        public int baseMaxSpirit { get { return _baseMaxSpirit; } private set { _baseMaxSpirit = value; } }
        public float maxSpirit => baseMaxSpirit + (subtotalSpirit * ResourcePerStatMultiplier * level);

        [SerializeField]
        private float _currentSpirit = 0.0f;
        public float currentSpirit { get { return _currentSpirit; } private set { _currentSpirit = value; } }
        public float currentSpiritPercent => (float)currentSpirit / maxSpirit;
        public float spiritRegenerationPerSecond { get; private set; } = 4.0f;

        public int baseStatPoints { get; set; } = 4;
        public int traitStatPoints = 0;

        public int totalStatPoints => baseStatPoints + traitStatPoints;

        [SerializeField]
        private int _usedStatPoints = 0;
        public int usedStatPoints { get { return _usedStatPoints; } set { _usedStatPoints = value; } }
        public int remainingStatPoints => totalStatPoints - usedStatPoints;

        public Dictionary<Stat, int> totalStatsByName => new Dictionary<Stat, int>()
        {
            { Stat.Strength, subtotalStrength },
            { Stat.Vitality, subtotalVitality },
            { Stat.Spirit, subtotalSpirit },
        };
        public Stats() { }

        public Stats(int strength, int vitality, int spirit, int level, CharacterClassAttributes characterClass)
        {
            baseStrength = strength;
            baseVitality = vitality;
            baseSpirit = spirit;
            this.level = level;
            baseMaxHp = characterClass.baseMaxHpModifier;
            baseMaxSpirit = characterClass.baseMaxSpiritModifier;

            currentHP = maxHp;
            currentSpirit = maxSpirit;
        }

        public void AdjustStatByName(Stat stat, int amount)
        {
            GetAssignedStatByName(stat) += amount;

            if(stat == Stat.Vitality)
                currentHP = maxHp;

            if (stat == Stat.Spirit)
                currentSpirit = maxSpirit;
        }

        public void UseStatPoint(Stat stat, int amount)
        {
            if (((usedStatPoints + amount) > totalStatPoints) || ((usedStatPoints + amount) < 0))
                return;

            usedStatPoints += amount;

            AdjustStatByName(stat, amount);
        }

        public int AdjustHealth(int amount)
        {
            int adjustment = currentHP - (int)Mathf.Clamp(currentHP - amount, 0.0f, maxHp);
            currentHP -= adjustment;

            return adjustment;
        }

        public bool TryAdjustSpirit(float amount, out float adjusted)
        {
            adjusted = 0;

            if (currentSpirit - amount < 0)
                return false;

            float adjustment = currentSpirit - Mathf.Clamp(currentSpirit - amount, 0.0f, maxSpirit);
            currentSpirit -= adjustment;

            adjusted = adjustment;

            return true;
        }

        public ref int GetAssignedStatByName(Stat stat)
        {
            switch (stat)
            {
                case (Stat.Strength):
                    return ref _assignedStrength;

                case (Stat.Vitality):
                    return ref _assignedVitality;

                case (Stat.Spirit):
                    return ref _assignedSpirit;

                default:
                    throw new ArgumentException("Unhandled Stat : " + stat.ToString());
            }
        }

        public void SetTraitStatPoints(CreatureEffect creatureEffect)
        {
            traitStatPoints = creatureEffect.traitStatPoints;
        }

        public void SetRaceStats(Race race)
        {
            _raceStrength = race.strengthModifier;
            _raceSpirit = race.spiritModifier;
            _raceVitality = race.vitalityModifier;

            currentHP = maxHp;
            currentSpirit = maxSpirit;
        }

        public void ResetAssignedStats()
        {
            GetAssignedStatByName(Stat.Strength) = 0;
            GetAssignedStatByName(Stat.Vitality) = 0;
            GetAssignedStatByName(Stat.Spirit) = 0;

            usedStatPoints = 0;
        }
    }
}
