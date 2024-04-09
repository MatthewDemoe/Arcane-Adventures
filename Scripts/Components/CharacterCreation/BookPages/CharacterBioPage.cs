using com.AlteredRealityLabs.ArcaneAdventures.Components.Book;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Races;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages
{
    public class CharacterBioPage : BookPage
    {
        PlayerCharacterReference playerCharacterReference;
        PlayerCharacter playerCharacter;

        [SerializeField] TextMeshProUGUI characterName;
        [SerializeField] TextMeshProUGUI characterRace;
        [SerializeField] TextMeshProUGUI characterClass;

        [SerializeField] TextMeshProUGUI characterVitality;
        [SerializeField] TextMeshProUGUI characterSpirit;
        [SerializeField] TextMeshProUGUI characterStrength;

        [SerializeField] TextMeshProUGUI characterMaxHealth;
        [SerializeField] TextMeshProUGUI characterMaxSpirit;

        [SerializeField] TextMeshProUGUI raceTraitName;
        [SerializeField] TextMeshProUGUI raceTraitDescription;
        [SerializeField] TextMeshProUGUI classTraitName;
        [SerializeField] TextMeshProUGUI classTraitDescription;

        int currentIndex = 0;

        void Start()
        {
            playerCharacterReference = InjectorContainer.Injector.GetInstance<PlayerCharacterReference>();

            if (playerCharacterReference.isGhost)
            {
                GetComponentInParent<BookController>().gameObject.SetActive(false);
                return;
            }

            InitializeTextFields(); 
        }

        private void OnEnable()
        {
            InitializeTextFields();
        }

        private void InitializeTextFields()
        {
            if (playerCharacterReference is null)
                return;

            playerCharacter = playerCharacterReference.creature as PlayerCharacter;
            Creature playerCreature = playerCharacterReference.creature;

            Trait initialTrait = playerCharacter.archetypeTraits.First(trait => trait.GetType() == playerCharacter.trait.GetType());
            currentIndex = playerCharacter.archetypeTraits.IndexOf(initialTrait);

            characterName.text = playerCharacter.playerName;
            characterRace.text = playerCharacter.race.ToString();
            characterClass.text = playerCharacter.characterClass.identifier.ToString();

            characterVitality.text = playerCharacter.stats.subtotalVitality.ToString();
            characterSpirit.text = playerCharacter.stats.subtotalSpirit.ToString();
            characterStrength.text = playerCharacter.stats.subtotalStrength.ToString();

            characterMaxHealth.text = playerCharacter.stats.maxHp.ToString();
            characterMaxSpirit.text = playerCharacter.stats.maxSpirit.ToString();

            Trait raceTrait = Trait.GetTrait(AllTraits.GetTraitType(Race.Get(playerCharacter.race).traitIdentifier), playerCreature);
            raceTraitName.text = raceTrait.name;
            raceTraitDescription.text = raceTrait.description;

            classTraitName.text = playerCharacter.trait.name;
            classTraitDescription.text = playerCharacter.trait.description;
        }

        public void NavigateClassTraits(int direction)
        {
            currentIndex = (playerCharacter.archetypeTraits.Count + currentIndex + direction) % playerCharacter.archetypeTraits.Count;

            playerCharacter.SetTrait(Trait.GetTrait(playerCharacter.archetypeTraits[currentIndex], playerCharacter));

            classTraitName.text = playerCharacter.trait.name;
            classTraitDescription.text = playerCharacter.trait.description;
        }
    }
}