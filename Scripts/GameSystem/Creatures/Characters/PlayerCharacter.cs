using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using System;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters
{
    [Serializable]
    public class PlayerCharacter : Character
    {
        [SerializeField]
        public string playerName = string.Empty;

        public PlayerCharacter(string name, Stats stats, Identifiers.Race race, Identifiers.Gender gender, Identifiers.PrimaryCharacterClass characterClass, Item leftHandItem = null, Item rightHandItem = null) 
            : base(stats, race, gender, characterClass, leftHandItem, rightHandItem, CharacterAppearance.Custom)
        {
            playerName = name;
        }

        public PlayerCharacter(PlayerCharacter copyPlayerCharacter) : base(copyPlayerCharacter)
        {
            playerName = copyPlayerCharacter.playerName;
        }

        public static PlayerCharacter BasicPlayerCharacter =>
            new PlayerCharacter(
                    name: "",
                    new Stats(
                        strength: 3,
                        vitality: 3,
                        spirit: 3,
                        level: 1,
                        characterClass: CharacterClassAttributes.WarriorAttributes
                        ),
                    race: Identifiers.Race.Human,
                    gender: Identifiers.Gender.Male,
                    characterClass: CharacterClassAttributes.WarriorAttributes.identifier
                );   
    }
}