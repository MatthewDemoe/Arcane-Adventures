using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public static class StatContest
    {
        private const int baseNumberToBeat = 10;

        public static bool PerformStatContest(Creature targetCreature, Creature initiatingCreature, Stats.Stat targetStat, Stats.Stat initiatorStat)
        {
            Random rand = new Random();

            int targetDieRoll = rand.Next(21);

            if (targetDieRoll == 0)
                return false;
            if (targetDieRoll == 20)
                return true;

            int contestedCheckBonus = targetCreature.modifiers.effects.Sum(effect => effect.contestedCheckBonus);

            switch (targetStat)
            {
                case Stats.Stat.Spirit:
                    contestedCheckBonus += targetCreature.modifiers.effects.Sum(effect => effect.spiritContestedCheckBonus);
                    break;

                case Stats.Stat.Vitality:
                    contestedCheckBonus += targetCreature.modifiers.effects.Sum(effect => effect.vitalityContestedCheckBonus);
                    break;

                case Stats.Stat.Strength:
                    contestedCheckBonus += targetCreature.modifiers.effects.Sum(effect => effect.strengthContestedCheckBonus);
                    break;

                default:
                    throw new NotImplementedException($"Stat doesn't exist {nameof(targetStat)}");
            }

            int numberToBeat = baseNumberToBeat + initiatingCreature.stats.totalStatsByName[initiatorStat];
            int targetTotal = targetDieRoll + targetCreature.stats.totalStatsByName[targetStat] + contestedCheckBonus;

            return targetTotal >= numberToBeat;
        }
    }
}