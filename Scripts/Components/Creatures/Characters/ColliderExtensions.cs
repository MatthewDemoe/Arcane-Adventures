using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters
{
    public static class ColliderExtensions
    {
        public static bool IsPlayerCharacter(this Collider collider)
        {
            return collider.GetComponentInParent<PlayerCharacterReference>() != null;
        }
    }
}