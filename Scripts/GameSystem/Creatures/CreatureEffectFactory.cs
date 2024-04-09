using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures
{
    public class CreatureEffectFactory
    {
        private ICombatSystem combatSystem;

        public CreatureEffectFactory(ICombatSystem combatSystem)
        {
            this.combatSystem = combatSystem;
        }

        public CreatureEffect BuildGuardedStance()
        {
            var overwhelmMultiplierBonus = combatSystem.settings.overwhelmGuardedStanceMultiplierBonus;

            return new CreatureEffect(
                name: "Guarded Stance",
                description: $"{GetAsMultiplierAsPercent(overwhelmMultiplierBonus)} bonus to overwhelm score",
                source: $"Standing still for {combatSystem.settings.guardedStanceTimeToStandStillInSeconds} seconds",
                overwhelmMultiplier: overwhelmMultiplierBonus
            );
        }

        private static string GetAsMultiplierAsPercent(float multiplier) => $"{multiplier * 100}%";
    }
}