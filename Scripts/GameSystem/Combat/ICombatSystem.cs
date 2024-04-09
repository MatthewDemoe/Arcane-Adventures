using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using Injection;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public interface ICombatSystem : IInjectable
    {
        public void ReportHit(Hit hit);
        public Creature GetOverwhelmWinner(Creature creatureA, Weapon weaponA, StrikeType strikeTypeA, Creature creatureB, Weapon weaponB, StrikeType strikeTypeB);
        public CombatSystemSettings settings { get; }
        public CreatureEffectFactory creatureEffectFactory { get; }
        public string[] combatLog { get; }
    }
}