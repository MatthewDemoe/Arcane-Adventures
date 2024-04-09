using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WarriorClasses
{
    public class Sentinel : CharacterClassArchetype
    {
        public override string name => "Sentinel";
        public override string classDescription => "Use Magic to enhance and protect your allies all while you maintain the frontlines using your body as the barrier between your enemy and your allies.";
        public override Identifiers.CharacterClassArchetype identifier => Identifiers.CharacterClassArchetype.Sentinel;
        public static Sentinel Instance { get; } = new Sentinel();
        protected Sentinel() { }

        public override List<Trait> traits => new List<Trait>() 
        { 
            EmpoweringAura.Instance,
            GuardianOfThePeople.Instance,
            HeroicPresence.Instance,
            Revenge.Instance,
            ShieldMaster.Instance,
        };

        public override List<Spell> spells => new List<Spell>();
    }
}
