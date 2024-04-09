using UnityEngine;
using TMPro;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Book;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages
{
    public class CharacterClassBook : BookController
    {
        [SerializeField]
        public Identifiers.PrimaryCharacterClass characterClassIdentifier;

        [SerializeField]
        TextMeshProUGUI coverClassTitle;

        [SerializeField]
        GameObject coverClassIconSlot;

        public PrimaryCharacterClass characterClass { get; private set; }

        new void Start()
        {
            base.Start();

            characterClass = PrimaryCharacterClass.Get(characterClassIdentifier);

            coverClassTitle.text = characterClass.classAttributes.name;

            Instantiate(CharacterClassUI.GetCharacterClassKeyArtByName()[characterClass.classAttributes.name], coverClassIconSlot.transform);
        }
    }
}
