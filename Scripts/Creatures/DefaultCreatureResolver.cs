using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Creatures
{
    public static class DefaultCreatureResolver
    {
        public static Creature GetDefaultCreature<T>() where T : Creature => GetDefaultCreature(typeof(T));

        public static Creature GetDefaultCreature(Type type, bool isArmed = true)
        {
            if (type == typeof(OrcRaider))
            {
                return new OrcRaider(
                    stats: new Stats(
                        strength: 10,
                        vitality: 10,
                        spirit: 10,
                        level: 1,
                        characterClass: CharacterClassAttributes.WarriorAttributes),
                    race: Identifiers.Race.Orc,
                    //TODO: Find out why orc cannot be ambidextrous
                    leftHandItem: isArmed ? ItemCache.GetItem(ItemCache.ItemNames.Shortsword) : ItemCache.GetItem(ItemCache.ItemNames.Fist),
                    rightHandItem: isArmed ? ItemCache.GetItem(ItemCache.ItemNames.Shortsword) : ItemCache.GetItem(ItemCache.ItemNames.Fist));
            }

            if (type == typeof(OrcShaman))
            {
                return new OrcShaman(
                    stats: new Stats(
                        strength: 10,
                        vitality: 10,
                        spirit: 10,
                        level: 1,
                        characterClass: CharacterClassAttributes.WizardAttributes),
                    race: Identifiers.Race.Orc,
                    leftHandItem: null,
                    rightHandItem: isArmed ? ItemCache.GetItem(ItemCache.ItemNames.Staff) : null);
            }

            if (type == typeof(Grell))
            {
                return new Grell(
                    stats: new Stats(
                        strength: 5,
                        vitality: 5,
                        spirit: 5,
                        level: 1,
                        characterClass: CharacterClassAttributes.WarriorAttributes),
                    race: Identifiers.Race.Orc,
                    leftHandItem: null,
                    rightHandItem: isArmed ? ItemCache.GetItem(ItemCache.ItemNames.Shortsword) : null);
            }

            else if (type == typeof(PossessedDummy))
            {
                return new PossessedDummy(
                    stats: new Stats(
                        strength: 10,
                        vitality: 10,
                        spirit: 10,
                        level: 1,
                        characterClass: CharacterClassAttributes.WarriorAttributes),
                    race: Identifiers.Race.Other);
            }
            else if (type == typeof(TutorialFairy))
            {
                return new TutorialFairy(
                    stats: new Stats(
                        strength: 1,
                        vitality: 1,
                        spirit: 1,
                        level: 1,
                        characterClass: CharacterClassAttributes.WarriorAttributes),
                    race: Identifiers.Race.Other);
            }

            else if (type == typeof(Ogre))
            {
                return new Ogre(
                    stats: new Stats(
                        strength: 12,
                        vitality: 16,
                        spirit: 8,
                        level: 1,
                        characterClass: CharacterClassAttributes.WarriorAttributes),
                    race: Identifiers.Race.Other,
                    leftHandItem: ItemCache.GetItem(ItemCache.ItemNames.OgreFist),
                    rightHandItem: isArmed ? ItemCache.GetItem(ItemCache.ItemNames.Tree) : ItemCache.GetItem(ItemCache.ItemNames.OgreFist));
            }

            else if (type == typeof(PlayerCharacter))
            {
                return new PlayerCharacter(
                    name: "",
                    new Stats(
                        strength: 10,
                        vitality: 10,
                        spirit: 10,
                        level: 1,
                        characterClass: CharacterClassAttributes.WarriorAttributes
                    ),
                    race: Identifiers.Race.Human,
                    gender: Identifiers.Gender.Male,
                    characterClass: CharacterClassAttributes.WarriorAttributes.identifier,
                    leftHandItem: null,
                    rightHandItem: null
                );
            }
            else throw new NotImplementedException();
        }
    }
}