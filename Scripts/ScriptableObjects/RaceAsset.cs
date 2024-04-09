using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Races;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Race", order = 0)]
    public class RaceAsset : ScriptableObject
    {
        [SerializeField] private Identifiers.Race race;
        [SerializeField] private string raceName;
        [SerializeField] private int strengthModifier;
        [SerializeField] private int vitalityModifier;
        [SerializeField] private int spiritModifier;
        [SerializeField] Identifiers.Trait trait;
        [SerializeField] [TextArea(2, 5)] private string lore;
        [SerializeField] private float maleMinimumHeight;
        [SerializeField] private float maleMaximumHeight;
        [SerializeField] private float femaleMinimumHeight;
        [SerializeField] private float femaleMaximumHeight;

        private static RaceAsset[] LoadAll() => Resources.LoadAll<RaceAsset>($"{nameof(ScriptableObject)}s/{nameof(Identifiers.Race)}s/");
        public static IEnumerable<Race> LoadRaces() => LoadAll()
            .Select(raceAsset => new Race(raceAsset.name, raceAsset.race, raceAsset.strengthModifier, raceAsset.vitalityModifier, raceAsset.spiritModifier, raceAsset.trait, raceAsset.lore, raceAsset.maleMinimumHeight, raceAsset.maleMaximumHeight, raceAsset.femaleMinimumHeight, raceAsset.femaleMaximumHeight));
    }
}