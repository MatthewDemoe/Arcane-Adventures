using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using System;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public class CombatSystem : ICombatSystem
    {
        private readonly CreatureEffectFactory creatureEffectFactory;
        private readonly CombatSystemSettings settings;
        private readonly Queue<string> combatLog;

        CombatSystemSettings ICombatSystem.settings => settings;
        CreatureEffectFactory ICombatSystem.creatureEffectFactory => creatureEffectFactory;

        string[] ICombatSystem.combatLog {
            get {
                var array = combatLog.ToArray();
                Array.Reverse(array);

                return array;
            }
        } 

        public CombatSystem()
        {
            creatureEffectFactory = new CreatureEffectFactory(this);
            settings = new CombatSystemSettings();
            combatLog = new Queue<string>();
        }

        public void ReportHit(Hit hit)
        {
            if (hit.reported)
            {
                throw new Exception("Cannot report hit twice");
            }

            if (hit.target.preventDamage)
            {
                hit.Report(0, 0);

                return;
            }

            hit.CalculateHealthChange();

            int hpAdjusted = hit.target.stats.AdjustHealth(hit.healthChange.Value);

            if (hit.target.isDead)
                hit.hitSource.OnTargetKilled.Invoke();

            hit.Report(hit.healthChange.Value, hpAdjusted);
        }

        public Creature GetOverwhelmWinner(Creature creatureA, Weapon weaponA, StrikeType strikeTypeA, Creature creatureB, Weapon weaponB, StrikeType strikeTypeB)
        {
            var overwhelmScoreA = GetOverwhelmScore(creatureA, weaponA, strikeTypeA);
            var overwhelmScoreB = GetOverwhelmScore(creatureB, weaponB, strikeTypeB);

            if (Math.Abs(overwhelmScoreA - overwhelmScoreB) <= settings.overwhelmDrawThreshold)
            {
                combatLog.Enqueue($"Overwhelm was draw.");
                return null;
            }

            var winner = overwhelmScoreA > overwhelmScoreB ? creatureA : creatureB;

            WriteToCombatLog($"{winner.race} won overwhelm.");//TODO: Use better reference than race.

            return winner;
        }

        private float GetOverwhelmScore(Creature creature, Weapon weapon, StrikeType strikeType)
        {
            var sizeBonus = settings.overwhelmBonusByCreatureSize[creature.size];
            var strengthBonus = creature.stats.subtotalStrength;
            var weightCategoryBonus = weapon.weightCategory * settings.overwhelmWeightCategoryMultiplier;
            var strikeBonus = settings.overwhelmStrikeBonus[strikeType];
            var overwhelmScore = ((strengthBonus * sizeBonus) + weightCategoryBonus + creature.overwhelmFlatModifier) * (strikeBonus + creature.overwhelmMultiplier);

            WriteToCombatLog($"{creature.race} got an overwhelm score of {overwhelmScore} (size bonus: {sizeBonus} / strength bonus: {strengthBonus} / weight category bonus: {weightCategoryBonus} / strike bonus: {strikeBonus} / flat modifier: {creature.overwhelmFlatModifier} / multiplier: {creature.overwhelmMultiplier}).");//TODO: Use better reference than race.

            return overwhelmScore;
        }

        private void WriteToCombatLog(string message)
        {
            combatLog.Enqueue($"[{DateTime.Now}] {message}");
        }
    }
}