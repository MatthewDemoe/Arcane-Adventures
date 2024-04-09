using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public static class SavingThrow
    {
        private const int numberToBeat = 10;

        public static bool PerformSavingThrow(Creature savingCreature, Stats.Stat savingStat)
        {
            Random rand = new Random();

            int savingRoll = rand.Next(21);

            if (savingRoll == 0)
                return false;
            if (savingRoll == 20)
                return true;

            savingRoll += (savingCreature.stats.totalStatsByName[savingStat] - 10) / 2;

            return savingRoll > numberToBeat;
        }
    }
}