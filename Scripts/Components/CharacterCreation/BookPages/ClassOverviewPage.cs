using System.Collections.Generic;
using UnityEngine;
using TMPro;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Book;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages
{
    public class ClassOverviewPage : BookPage
    {
        [SerializeField]
        CharacterClassBook characterClassBook;
        PrimaryCharacterClass characterClass;

        [SerializeField]
        TextMeshProUGUI characterClassName;

        [SerializeField]
        TextMeshProUGUI classDescription;

        [SerializeField]
        TextMeshProUGUI archetypesTitle;

        [SerializeField]
        TextMeshProUGUI archetypesBody;

        private void Start()
        {
            characterClass = PrimaryCharacterClass.Get(characterClassBook.characterClassIdentifier);
            
            characterClassName.text = characterClass.classAttributes.name;
            classDescription.text = characterClass.classAttributes.classDescription;

            archetypesTitle.text = $"{characterClass.classAttributes.name} Archetypes";

            archetypesBody.text = "";

            List<string> piecesOfBodyText = new List<string>();

            for (int i = 0; i < characterClass.classArchetypes.Count; i++)
            {
                piecesOfBodyText.Add($"<i><u>{characterClass.classArchetypes[i].name}</i></u>: {characterClass.classArchetypes[i].classDescription}");
            }

            archetypesBody.text = string.Join("\n\n", piecesOfBodyText);
        }
    }
}
