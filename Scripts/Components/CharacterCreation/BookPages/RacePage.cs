using UnityEngine;
using TMPro;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Races;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Book;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages
{
    public class RacePage : BookPage
    {
        [SerializeField]
        TextMeshProUGUI raceTitle;

        [SerializeField]
        TextMeshProUGUI raceModifiers;

        [SerializeField]
        TextMeshProUGUI raceLore;

        public void UpdateRaceInfo(Race race)
        {
            if (CharacterCreator.Instance.swappingRace)
                return;

            raceTitle.text = race.name;
            raceModifiers.text = GenerateRaceModifiers(race);
            raceLore.text = race.lore;
        }

        private string GenerateRaceModifiers(Race race)
        {
            return $"Strength Modifier: {race.strengthModifier}\nVitality Modifier: {race.vitalityModifier}\nSpirit Modifier: {race.spiritModifier}";
        }
    }
}
