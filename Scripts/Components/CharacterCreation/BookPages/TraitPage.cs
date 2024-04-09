using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Book;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;


namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages
{
    public class TraitPage : BookPage
    {
        [SerializeField]
        CharacterClassBook characterClassBook;

        PrimaryCharacterClass characterClass;

        [SerializeField]
        TextMeshProUGUI traitBody;

        [SerializeField]
        Image traitImage;

        [SerializeField]
        TextMeshProUGUI classTraitTitle;

        [SerializeField]
        TextMeshProUGUI allTraits;

        void Start()
        {
            characterClass = PrimaryCharacterClass.Get(characterClassBook.characterClassIdentifier);

            traitBody.text = characterClass.classAttributes.traitsDescription;

            traitImage.sprite = CharacterClassUI.GetCharacterClassIconsByName()[characterClass.classAttributes.name].GetComponent<Image>().sprite;

            classTraitTitle.text = $"{characterClass.classAttributes.name} Traits";

            List<string> piecesOfBodyText = new List<string>();

            allTraits.text = "";
            for (int i = 0; i < characterClass.traits.Count; i++)
            {
               piecesOfBodyText.Add($"<i><u>{characterClass.traits[i].name}</i></u>: {characterClass.traits[i].description}");
            }

            allTraits.text = string.Join("\n\n", piecesOfBodyText);
        }
    }
}
