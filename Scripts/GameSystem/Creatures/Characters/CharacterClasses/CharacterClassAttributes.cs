using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses
{
    public class CharacterClassAttributes 
    {
        public CharacterClassAttributes(CharacterClassAsset characterClassAsset) 
        {
            name = characterClassAsset.className;
            identifier = characterClassAsset.characterClass;
            classDescription = characterClassAsset.classDescription;
            traitsDescription = characterClassAsset.traitsDescription;
            spellsDescription = characterClassAsset.spellsDescription;

            baseMaxHpModifier = characterClassAsset.baseMaxHpModifier;
            baseMaxSpiritModifier = characterClassAsset.baseMaxSpiritModifier;
        }

        public static Dictionary<Identifiers.PrimaryCharacterClass, CharacterClassAttributes> CharacterClassesByIdentifier;

        public static CharacterClassAttributes WarriorAttributes = Get(Identifiers.PrimaryCharacterClass.Warrior);
        public static CharacterClassAttributes WizardAttributes = Get(Identifiers.PrimaryCharacterClass.Wizard);

        public string name { get;  private set;}

        public Identifiers.PrimaryCharacterClass identifier { get; private set; }

        public string classDescription { get; }
        public string traitsDescription { get; }
        public string spellsDescription { get; }

        public int baseMaxHpModifier { get; }
        public int baseMaxSpiritModifier { get; }

        public static CharacterClassAttributes Get(Identifiers.PrimaryCharacterClass identifier)
        {
            if (CharacterClassesByIdentifier == null)
                CharacterClassesByIdentifier = CharacterClassAsset.LoadCharacterClasses().ToDictionary(characterClass => characterClass.identifier, characterClass => characterClass);

            return CharacterClassesByIdentifier.TryGetValue(identifier, out var characterClass) ? characterClass : null;
        }
    }
}