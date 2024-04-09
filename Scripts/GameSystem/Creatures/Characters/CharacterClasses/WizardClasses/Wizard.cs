using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WizardClasses
{
    public class Wizard : PrimaryCharacterClass
    {
        public static Wizard Instance { get; } = new Wizard();
        protected Wizard() { }
        public override Identifiers.PrimaryCharacterClass identifier => CharacterClassAttributes.WizardAttributes.identifier;

        public override CharacterClassAttributes classAttributes => CharacterClassAttributes.WizardAttributes;

        public override List<Trait> traits => new List<Trait> 
        { 
            ArcaneResistance.Instance,
            EmpoweredCasting.Instance,
            ExpansiveKnowledge.Instance,
            GreaterSpiritualConnection.Instance,
            OverwhelmingSpirit.Instance,
        };

        public override List<EquipmentSet> equipmentSets => EquipmentSetCache.EquipmentSets;

        public override List<CharacterClassArchetype> classArchetypes => new List<CharacterClassArchetype> { EssenceMage.Instance, ArcaneAdept.Instance, Elementalist.Instance, };

        public override List<Spell> spells => ArcaneAdept.Instance.spells
            .Concat(Elementalist.Instance.spells)
            .Concat(EssenceMage.Instance.spells)
            .ToList();
    }
}