
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures
{
    public class PlayerCharacterSaveFileReference : CreatureReference
    {
        protected override System.Type creatureType { get { return typeof(GameSystem.Creatures.Characters.PlayerCharacter); } }
    }
}