using System.Collections.Generic;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;

namespace com.AlteredRealityLabs.ArcaneAdventures.Lookups
{
    public static class CharacterClassUI 
    {
        public static Dictionary<string, GameObject> GetCharacterClassIconsByName()
        {
            Dictionary<string, GameObject> characterIconDictionary = new Dictionary<string, GameObject>();

            characterIconDictionary.Add(CharacterClassAttributes.WarriorAttributes.name, Prefabs.UI.Load(Prefabs.UI.WarriorIcon));
            characterIconDictionary.Add(CharacterClassAttributes.WizardAttributes.name, Prefabs.UI.Load(Prefabs.UI.WizardIcon));

            return characterIconDictionary;
        }

        public static Dictionary<string, GameObject> GetCharacterClassKeyArtByName()
        {
            Dictionary<string, GameObject> characterIconDictionary = new Dictionary<string, GameObject>();

            characterIconDictionary.Add(CharacterClassAttributes.WarriorAttributes.name, Prefabs.UI.Load(Prefabs.UI.WarriorKeyArt));
            characterIconDictionary.Add(CharacterClassAttributes.WizardAttributes.name, Prefabs.UI.Load(Prefabs.UI.WizardKeyArt));

            return characterIconDictionary;
        }
    }
}
