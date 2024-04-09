using System.Collections.Generic;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WarriorClasses
{
    public class Warrior : PrimaryCharacterClass
    {        
        public static Warrior Instance { get; } = new Warrior();
        protected Warrior() { }
        public override Identifiers.PrimaryCharacterClass identifier => CharacterClassAttributes.WarriorAttributes.identifier;

        public override CharacterClassAttributes classAttributes => CharacterClassAttributes.WarriorAttributes;

        public override List<Trait> traits => new List<Trait> 
        {
            BluntForce.Instance,
            DefensiveStance.Instance,
            EnduranceTraining.Instance,
            HeavyWeaponMaster.Instance,
            PiercingStrikes.Instance,
        };

        public override List<EquipmentSet> equipmentSets => EquipmentSetCache.EquipmentSets;

        public override List<CharacterClassArchetype> classArchetypes => new List<CharacterClassArchetype> { Berserker.Instance, Sentinel.Instance, WeaponMaster.Instance};
        
        public override List<Spell> spells => Berserker.Instance.spells
            .Concat(Sentinel.Instance.spells)
            .Concat(WeaponMaster.Instance.spells)
            .ToList();
    }
}