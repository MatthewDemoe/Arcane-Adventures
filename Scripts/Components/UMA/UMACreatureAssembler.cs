using UMA.CharacterSystem;
using com.AlteredRealityLabs.ArcaneAdventures.Appearance.UMA;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using System.Collections;
using UnityEngine;
using System;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UMA
{
    public class UMACreatureAssembler : MonoBehaviour
    {
        [SerializeField]
        DynamicCharacterAvatar characterAvatar;
        public CreatureReference creatureReference;
        Character character => creatureReference.creature as Character;

        private void Start()
        {
            if (creatureReference == null)
            {
                creatureReference = GetComponent<CreatureReference>();

                if (creatureReference.creature.race.Equals(Identifiers.Race.Ghost))
                {
                    Destroy(this);
                }
                else
                {
                    StartCoroutine(InitializeRaceData());
                }
            }
        }

        private void InitializeFeatures()
        {
            var features = Enum.GetValues(typeof(Wardrobe.Feature)).Cast<Wardrobe.Feature>();

            for (int i = 0; i < features.Count(); i++)
            {
                if (features.ElementAt(i) != Wardrobe.Feature.Gender)
                    NavigateFeature(features.ElementAt(i), 0);
            }
        }

        public void NavigateFeature(Wardrobe.Feature feature, int direction)
        {
            int thisFeatureNumber = character.selectedWardrobeFeatures[feature];

            Wardrobe.Gender thisGender = Wardrobe.GendersByIdentifier[character.gender];
            Wardrobe.RaceData raceData = Wardrobe.WardrobesByRace[character.race].raceDataByGender[thisGender];
            Enum thisFeature = Wardrobe.WardrobesByRace[character.race].featureListByRaceData[raceData][feature];

            int numberOfFeatures = Enum.GetValues(thisFeature.GetType()).Length;

            thisFeatureNumber = (thisFeatureNumber + direction + numberOfFeatures) % numberOfFeatures;

            character.selectedWardrobeFeatures[feature] = thisFeatureNumber;

            if (feature == Wardrobe.Feature.Gender)
            {
                character.gender = Wardrobe.IdentifierByGender[(Wardrobe.Gender)thisFeatureNumber];

                UpdateRaceData();
                InitializeFeatures();

                return;
            }

            characterAvatar.SetSlot(Wardrobe.FeatureSlotsByFeature[feature].ToString(), Enum.GetNames(thisFeature.GetType())[thisFeatureNumber]);
            StartCoroutine(BuildCharacter());
        }

        public void UpdateRaceData()
        {
            characterAvatar.ChangeRace(Wardrobe.WardrobesByRace[character.race].raceDataByGender[Wardrobe.GendersByIdentifier[character.gender]].ToString());

            StartCoroutine(UpdateRaceDataRoutine());
        }

        IEnumerator UpdateRaceDataRoutine()
        {
            while (characterAvatar.umaData == null)
            {
                yield return null;
            }

            InitializeFeatures();
        }

        IEnumerator InitializeRaceData()
        {
            while (characterAvatar.umaData == null)
            {
                yield return null;
            }

            UpdateRaceData();
        }

        IEnumerator BuildCharacter()
        {
            yield return null;

            characterAvatar.BuildCharacter();
        }
    }
}
