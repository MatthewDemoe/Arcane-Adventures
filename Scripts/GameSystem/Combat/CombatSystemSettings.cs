using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public sealed class CombatSystemSettings
    {
        public readonly Dictionary<StrikeType, float> strengthModifierByStrikeType = new Dictionary<StrikeType, float>();
        public readonly Dictionary<int, Dictionary<StrikeType, int>> damageByStrikeTypeByWeightCategory = new Dictionary<int, Dictionary<StrikeType, int>>();

        public readonly Dictionary<CreatureSize, float> overwhelmBonusByCreatureSize = new Dictionary<CreatureSize, float>();
        public float overwhelmWeightCategoryMultiplier;
        public readonly Dictionary<StrikeType, float> overwhelmStrikeBonus = new Dictionary<StrikeType, float>();
        public float overwhelmGuardedStanceMultiplierBonus;
        public float overwhelmDrawThreshold;
        public float guardedStanceTimeToStandStillInSeconds;
        public float guardedStanceMovementThresholdInMeters;

        public void SetAllValuesToDefault()
        {
            strengthModifierByStrikeType.ReplaceWith(new Dictionary<StrikeType, float>
            {
                { StrikeType.Perfect, 1f },
                { StrikeType.Imperfect, 0.5f },
                { StrikeType.Incomplete, 0.25f }
            });

            damageByStrikeTypeByWeightCategory.ReplaceWith(new Dictionary<int, Dictionary<StrikeType, int>>
            {
                {
                    0, new Dictionary<StrikeType, int>
                    {
                        { StrikeType.Perfect, 0 },
                        { StrikeType.Imperfect, 0 },
                        { StrikeType.Incomplete, 0 },
                    }
                },
                {
                    1, new Dictionary<StrikeType, int>
                    {
                        { StrikeType.Perfect, 2 },
                        { StrikeType.Imperfect, 1 },
                        { StrikeType.Incomplete, 0 },
                    }
                },
                {
                    2, new Dictionary<StrikeType, int>
                    {
                        { StrikeType.Perfect, 5 },
                        { StrikeType.Imperfect, 3 },
                        { StrikeType.Incomplete, 1 },
                    }
                },
                {
                    3, new Dictionary<StrikeType, int>
                    {
                        { StrikeType.Perfect, 10 },
                        { StrikeType.Imperfect, 6 },
                        { StrikeType.Incomplete, 2 },
                    }
                }
                //TODO: Add more as needed.
            });

            overwhelmBonusByCreatureSize.ReplaceWith(new Dictionary<CreatureSize, float>
            {
                { CreatureSize.Tiny, 0.25f },
                { CreatureSize.Small, 0.6f },
                { CreatureSize.Medium, 1f },
                { CreatureSize.Large, 1.5f },
                { CreatureSize.Massive, 2.5f },
            });

            overwhelmWeightCategoryMultiplier = 3;

            overwhelmStrikeBonus.ReplaceWith(new Dictionary<StrikeType, float>
            {
                { StrikeType.NotStrike, 1f },
                { StrikeType.Incomplete, 1.25f },
                { StrikeType.Imperfect, 1.5f },
                { StrikeType.Perfect, 2f },
            });

            overwhelmGuardedStanceMultiplierBonus = 0.5f;
            overwhelmDrawThreshold = 5;
            guardedStanceTimeToStandStillInSeconds = 3;
            guardedStanceMovementThresholdInMeters = 0.05f;
        }

        public CombatSystemSettings()
        {
            SetAllValuesToDefault();
        }
    }
}