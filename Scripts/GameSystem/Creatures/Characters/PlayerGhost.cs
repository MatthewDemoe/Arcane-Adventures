using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters
{
    [System.Serializable]
    public class PlayerGhost : PlayerCharacter
    {
        //TODO: Temporary implementation to allow instantiation, clean up when ghost implementation details become clearer.
        public PlayerGhost() : 
            base(
                name: string.Empty, 
                new Stats(strength: 30, vitality: 30, spirit:30, level: 1, characterClass: CharacterClassAttributes.WarriorAttributes), 
                race: Identifiers.Race.Ghost, gender: Identifiers.Gender.NotSet, characterClass: CharacterClassAttributes.WarriorAttributes.identifier
                ) {}
    }
}