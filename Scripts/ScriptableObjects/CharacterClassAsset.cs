using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Character Class", order = 1)]
    public class CharacterClassAsset : ScriptableObject
    {
        [SerializeField] public Identifiers.PrimaryCharacterClass characterClass;
        [SerializeField] public string className;

        [SerializeField][Multiline] public string classDescription;
        [SerializeField][Multiline] public string traitsDescription;
        [SerializeField][Multiline] public string spellsDescription;

        [SerializeField][Range(0, 200)] public int baseMaxHpModifier;
        [SerializeField][Range(0, 200)] public int baseMaxSpiritModifier;

        private static CharacterClassAsset[] LoadAll() => Resources.LoadAll<CharacterClassAsset>($"{nameof(ScriptableObject)}s/{nameof(Identifiers.PrimaryCharacterClass)}es/");
        public static IEnumerable<CharacterClassAttributes> LoadCharacterClasses() => LoadAll()
            .Select(classAsset => new CharacterClassAttributes(classAsset));
    }
}